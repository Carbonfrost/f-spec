//
// Copyright 2016-2018 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public abstract class TestUnit {

        private int _isDisposed;
        private TestUnitFlags _flags;
        private string _reason;
        private Exception _initializeError;
        private TimeSpan? _timeout;
        private TestUnit _parent;
        private readonly HashSet<string> _tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        protected bool IsDisposed {
            get {
                return _isDisposed < 0;
            }
        }

        public bool IsReadOnly {
            get {
                return _flags.HasFlag(TestUnitFlags.Sealed);
            }
        }

        public virtual ITestSubjectProvider TestSubjectProvider {
            get {
                return null;
            }
        }

        public virtual TestDataProviderCollection TestDataProviders {
            get {
                return TestDataProviderCollection.Empty;
            }
        }

        internal Exception SetUpError {
            get {
                return _initializeError;
            }
        }

        public virtual object FindTestObject() {
            if (Parent != null) {
                return Parent.FindTestObject();
            }
            return null;
        }

        public virtual object FindTestSubject() {
            if (Parent != null) {
                return Parent.FindTestSubject();
            }
            return null;
        }

        internal void InitializeSafe(TestContext context) {
            using (context.ApplyingContext()) {
                try {
                    Initialize(context);
                } catch (Exception ex) {
                    _initializeError = ex;
                }
            }
        }

        internal void AfterExecutingSafe(TestContext context) {
            using (context.ApplyingContext()) {
                AfterExecuting(context);
            }
        }

        internal void BeforeExecutingSafe(TestContext context) {
            using (context.ApplyingContext()) {
                BeforeExecuting(context);
            }
        }

        internal void BeforeExecutingDescendentSafe(TestContext descendentTestContext) {
            BeforeExecutingDescendent(descendentTestContext);
        }

        internal void AfterExecutingDescendentSafe(TestContext descendentTestContext) {
            AfterExecutingDescendent(descendentTestContext);
        }

        protected virtual void Initialize(TestContext testContext) {
        }

        protected virtual void AfterExecuting(TestContext testContext) {
        }

        protected virtual void BeforeExecuting(TestContext testContext) {
            // Flush the log at this time.  Any log messages that were received
            // during initialization can be written now
            testContext.Log.Flush();
        }

        protected virtual void BeforeExecutingDescendent(TestContext descendentTestContext) {
        }

        protected virtual void AfterExecutingDescendent(TestContext descendentTestContext) {
        }

        internal bool NotifyStarting(ITestRunnerEventSink events) {
            var e = new TestUnitStartingEventArgs(this);
            events.NotifyUnitStarting(e);
            return !e.Cancel;
        }

        internal void NotifyStarted(ITestRunnerEventSink events) {
            events.NotifyUnitStarted(new TestUnitStartedEventArgs(this));
        }

        internal void NotifyFinished(ITestRunnerEventSink events, TestUnitResult result) {
            events.NotifyUnitFinished(new TestUnitFinishedEventArgs(this, result));
        }

        public TimeSpan? Timeout {
            get {
                return _timeout;
            }
            set {
                WritePreamble();
                _timeout = value;
            }
        }

        public bool IsExplicit {
            get {
                return _flags.HasFlag(TestUnitFlags.Explicit);
            }
            set {
                WritePreamble();
                SetFlag(TestUnitFlags.Explicit, value);
            }
        }

        public bool PassExplicitly {
            get {
                return _flags.HasFlag(TestUnitFlags.PassExplicitly);
            }
            set {
                WritePreamble();
                SetFlag(TestUnitFlags.PassExplicitly, value);
            }
        }

        public bool IsPending {
            get {
                return _flags.HasFlag(TestUnitFlags.Pending);
            }
            set {
                WritePreamble();
                SetFlag(TestUnitFlags.Pending, value);
            }
        }

        public bool Skipped {
            get {
                return _flags.HasFlag(TestUnitFlags.Skip);
            }
            set {
                WritePreamble();
                SetFlag(TestUnitFlags.Skip, value);
            }
        }

        public string Reason {
            get {
                return _reason;
            }
            set {
                WritePreamble();
                _reason = value;
            }
        }

        public bool IsFocused {
            get {
                return _flags.HasFlag(TestUnitFlags.Focus);
            }
            set {
                WritePreamble();
                SetFlag(TestUnitFlags.Focus, value);
            }
        }

        public abstract string DisplayName { get; }
        public abstract TestUnitType Type { get; }

        public IEnumerable<TestUnit> DescendentsAndSelf {
            get {
                return new[] { this }.Concat(Descendents);
            }
        }

        public IEnumerable<TestUnit> Descendents {
            get {
                return Children.SelectMany(c => c.DescendentsAndSelf);
            }
        }

        public virtual bool ContainsFocusedUnits {
            get {
                return Children.Any(t => t.ContainsFocusedUnits || t.IsFocused);
            }
        }

        public ICollection<string> Tags {
            get {
                return _tags;
            }
        }

        public TestUnit Parent {
            get {
                return _parent;
            }
        }

        public abstract TestUnitCollection Children {
            get;
        }

        protected TestUnit() {
        }

        public void Dispose() {
            // Dispose and suppress finalization
            if (_isDisposed == 0) {
                // Only do client disposal the first time
                GC.SuppressFinalize(this);
                try {
                    Dispose(true);
                } finally {
                    Interlocked.Decrement(ref _isDisposed);
                }
            }
        }

        internal void SetParent(TestUnit p) {
            _parent = p;
        }

        protected virtual void Dispose(bool disposing) {}

        protected void WritePreamble() {
            ThrowIfDisposed();
            ThrowIfSealed();
        }

        ~TestUnit() {
            Dispose(false);
        }

        protected void ThrowIfDisposed() {
            if (IsDisposed) {
                throw SpecFailure.Disposed(GetType().ToString());
            }
        }

        public override string ToString() {
            return Type + " " + DisplayName;
        }

        internal void ForceSkipped() {
            SetFlag(TestUnitFlags.Skip, true);

            foreach (var c in Children) {
                c.ForceSkipped();
            }
        }

        private void ThrowIfSealed() {
            if (IsReadOnly) {
                throw SpecFailure.Sealed();
            }
        }

        internal void Seal() {
            SetFlag(TestUnitFlags.Sealed, true);
            Children.MakeReadOnly();
        }

        private void SetFlag(TestUnitFlags tuf, bool value) {
            if (value) {
                _flags |= tuf;
            } else {
                _flags &= ~tuf;
            }
        }

        [Flags]
        enum TestUnitFlags {
            Sealed = 1,
            Focus = 2,
            Skip = 4,
            Pending = 8,
            Explicit = 0x10,
            PassExplicitly = 0x20,
        }

    }
}
