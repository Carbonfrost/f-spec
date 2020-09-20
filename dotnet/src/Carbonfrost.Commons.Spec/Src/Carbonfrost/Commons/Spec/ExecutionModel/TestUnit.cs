//
// Copyright 2016-2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
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

namespace Carbonfrost.Commons.Spec.ExecutionModel {

    public abstract class TestUnit : ITestUnitState, ITestUnitApiConventions {

        private int _isDisposed;
        private TestUnitFlags _flags;
        private string _reason;
        private string _description;
        private Exception _initializeError;
        private TimeSpan? _timeout;
        private TestUnit _parent;
        private readonly TestTagCollection _tags = new TestTagCollection();

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

        internal abstract TestUnitMetadata Metadata {
            get;
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

        internal virtual object CreateTestObject() {
            if (Parent != null) {
                return Parent.CreateTestObject();
            }
            throw new NotSupportedException();
        }

        internal virtual object FindTestSubject() {
            if (Parent != null) {
                return Parent.FindTestSubject();
            }
            return null;
        }

        internal virtual TestClassInfo FindTestClass() {
            if (Parent == null) {
                throw new ArgumentException(GetType().Name);
            }
            return Parent.FindTestClass();
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

        internal void AfterExecutingSafe(TestExecutionContext context) {
            using (context.ApplyingContext()) {
                AfterExecuting(context);
            }
        }

        internal void BeforeExecutingSafe(TestContext context) {
            using (context.ApplyingContext()) {
                BeforeExecuting(context);
            }
        }

        protected virtual void Initialize(TestContext testContext) {
        }

        protected virtual void AfterExecuting(TestExecutionContext testContext) {
        }

        protected virtual void BeforeExecuting(TestContext testContext) {
            // Flush the log at this time.  Any log messages that were received
            // during initialization can be written now
            testContext.Log.Flush();
        }

        internal bool NotifyStarting(ITestRunnerEventSink events, out TestUnitStartingEventArgs e) {
            e = new TestUnitStartingEventArgs(this);
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

        public bool Failed {
            get {
                return _flags.HasFlag(TestUnitFlags.Failed);
            }
            set {
                WritePreamble();
                SetFlag(TestUnitFlags.Failed, value);
            }
        }

        public string Description {
            get {
                return _description;
            }
            set {
                WritePreamble();
                _description = value;
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

        public abstract string DisplayName {
            get;
        }

        public abstract string Name {
            get;
        }

        public abstract TestUnitType Type {
            get;
        }

        public IEnumerable<TestUnit> DescendantsAndSelf {
            get {
                return new[] { this }.Concat(Descendants);
            }
        }

        public IEnumerable<TestUnit> Descendants {
            get {
                return Children.SelectMany(c => c.DescendantsAndSelf);
            }
        }

        public virtual bool ContainsFocusedUnits {
            get {
                if (Children.Count == 0) {
                    return false;
                }
                return Children.Any(t => t.IsFocused || t.ContainsFocusedUnits);
            }
        }

        public TestTagCollection Tags {
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

        internal void ForcePredeterminedStatus(TestUnitFlags flags, string reason) {
            SetFlag(flags, true);
            _reason = reason;

            foreach (var c in Children) {
                c.ForcePredeterminedStatus(flags, null);
            }
        }

        internal static TestStatus? ConvertToStatus(ITestUnitState state) {
            if (state.Skipped && !state.IsFocused) { // Skip unless focussed
                return TestStatus.Skipped;
            }
            if (state.IsPending) {
                return TestStatus.Pending;
            }
            if (state.Failed) {
                return TestStatus.Failed;
            }
            return null;
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

        internal void CopyFlags(TestUnitFlags flags) {
            _flags |= flags;
        }

        private void SetFlag(TestUnitFlags tuf, bool value) {
            if (value) {
                _flags |= tuf;
            } else {
                _flags &= ~tuf;
            }
        }
    }
}
