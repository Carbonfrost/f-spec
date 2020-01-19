//
// Copyright 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Carbonfrost.Commons.Spec {

    public class TestProcess : DisposableObject {

        private Process _process;
        private StringWriter _output;
        private StringWriter _error;
        private string _workingDirectory;
        private readonly ProcessStartInfo _startInfo;
        private int _timeout;
        private bool _didTimeout;

        private readonly Queue<string> _stdOutQueue = new Queue<string>();
        private readonly Queue<string> _stdErrQueue = new Queue<string>();

        private readonly ManualResetEvent _stdOutSignal = new ManualResetEvent(false);
        private readonly ManualResetEvent _stdErrSignal = new ManualResetEvent(false);
        private volatile ManualResetEvent _processExitSignal = new ManualResetEvent(false);
        private volatile ManualResetEvent _processTimeoutSignal = new ManualResetEvent(false);

        public string Output {
            get {
                return _output.ToString();
            }
        }

        public int ExitCode {
            get {
                return RequireProcess().ExitCode;
            }
        }

        public string WorkingDirectory {
            get {
                return _workingDirectory;
            }
            set {
                WritePreamble();
                _workingDirectory = value;
            }
        }

        public int Timeout {
            get {
                return _timeout;
            }
            set {
                WritePreamble();
                _timeout = value;
            }
        }

        internal TestProcess(string pathToTool, string commandLineCommands) {
            string arguments = commandLineCommands;
            Timeout = -1;

            ProcessStartInfo info = new ProcessStartInfo(pathToTool, arguments);
            info.CreateNoWindow = true;
            info.RedirectStandardError = true;
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;

            _startInfo = info;
        }

        public void Start() {
            WritePreamble();

            string workingDirectory = WorkingDirectory ?? Directory.GetCurrentDirectory();
            if (!string.IsNullOrEmpty(workingDirectory)) {
                _startInfo.WorkingDirectory = workingDirectory;
            }

            _process = new Process();
            _process.StartInfo = _startInfo;
            _process.EnableRaisingEvents = true;
            _process.ErrorDataReceived += process_ErrorDataReceived;
            _process.OutputDataReceived += process_OutputDataReceived;
            _process.Exited += process_Exited;
            _process.Start();
            _process.StandardInput.Dispose();
            _process.BeginErrorReadLine();
            _process.BeginOutputReadLine();
            _output = new StringWriter();
            _error = new StringWriter();

            Timer timer = new Timer(timer_Elapsed, null, Timeout, System.Threading.Timeout.Infinite);
            new Thread(ThreadProcessEvents).Start();
        }

        public void WaitForExit() {
            RequireProcess().WaitForExit();
        }

        protected override void Dispose(bool manualDispose) {
            base.Dispose(manualDispose);
            if (manualDispose && _process != null) {
                try {
                    if (!_process.HasExited) {
                        _process.Kill();
                    }
                } catch {
                    // Don't fail in this finalizer
                }
                _process.Dispose();
            }
        }

        private void ProcessExited(bool timeout) {
            _didTimeout = timeout;
        }

        private void ThreadProcessEvents() {
            try {
                if (ProcessEvents()) {
                    ProcessExited(false);
                } else {
                    _process.Kill();
                    ProcessExited(true);
                }
            } finally {
                _process.Exited -= process_Exited;
                _process.ErrorDataReceived -= process_ErrorDataReceived;
                _process.OutputDataReceived -= process_OutputDataReceived;
            }
        }

        private bool ProcessEvents() {
            while (true) {
                switch (WaitHandle.WaitAny(new [] {
                                               _stdErrSignal,
                                               _stdOutSignal,
                                               _processExitSignal,
                                               _processTimeoutSignal
                                           })) {
                    case 0:
                        LogMessagesFromStandardErrorOrOutput(_stdErrQueue, _stdErrSignal, _error);
                        break;

                    case 1:
                        LogMessagesFromStandardErrorOrOutput(_stdOutQueue, _stdOutSignal, _output);
                        break;

                    case 2:
                        CleanupOnExit();
                        return true;

                    case 3:
                        CleanupOnExit();
                        return false;

                    default:
                        break; // Shouldn't happen
                }
            }
        }

        private void CleanupOnExit() {
            // Flush any pending events
            LogMessagesFromStandardErrorOrOutput(_stdErrQueue, _stdErrSignal, _error, true);
            LogMessagesFromStandardErrorOrOutput(_stdOutQueue, _stdOutSignal, _output, true);

            _processTimeoutSignal.Dispose();
            _processExitSignal.Dispose();
            _processExitSignal = null;
            _processTimeoutSignal = null;
        }

        void process_Exited(object sender, EventArgs e) {
            if (_processExitSignal != null) {
                _processExitSignal.Set();
            }
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e) {
            ReceiveStandardErrorOrOutputData(e, _stdOutQueue, _stdOutSignal);
        }

        void process_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
            ReceiveStandardErrorOrOutputData(e, _stdErrQueue, _stdErrSignal);
        }

        void timer_Elapsed(object state) {
            if (_processTimeoutSignal != null) {
                _processTimeoutSignal.Set();
            }
        }

        private void LogMessagesFromStandardErrorOrOutput(Queue<string> queue,
                                                          ManualResetEvent signal,
                                                          TextWriter target,
                                                          bool isClosing = false)
        {
            lock (((ICollection) queue).SyncRoot) {
                while (queue.Count > 0)  {
                    string singleLine = queue.Dequeue();
                    if (string.IsNullOrEmpty(singleLine)) {
                        continue;
                    }
                    target.WriteLine(singleLine);
                }

                if (isClosing) {
                    signal.Dispose();
                } else {
                    signal.Reset();
                }
            }
        }

        private static void ReceiveStandardErrorOrOutputData(DataReceivedEventArgs e, Queue<string> queue, ManualResetEvent signal) {
            if (e.Data != null) {
                lock (((ICollection) queue).SyncRoot) {
                    queue.Enqueue(e.Data);

                    signal.Set();
                }
            }
        }

        private void WritePreamble() {
            if (_process != null) {
                throw new NotImplementedException();
            }
        }

        private Process RequireProcess() {
            if (_process == null) {
                throw new NotImplementedException();
            }
            return _process;
        }
    }
}
