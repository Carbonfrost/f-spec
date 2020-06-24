
//
// File was automatically generated at 06/08/2020 19:40:10
//

using System;

namespace Carbonfrost.Commons.Spec {


    partial class TestClass {

        public GivenExpectationBuilder Given() {
            return Assert.Given();
        }
    }

    partial class Asserter {

        public GivenExpectationBuilder Given() {
            return new GivenExpectationBuilder( _assumption);
        }

    }

    partial class Assert {

        public static GivenExpectationBuilder Given() {
            return Global.Given();
        }

    }

    partial class Assume {

        public static GivenExpectationBuilder Given() {
            return Global.Given();
        }

    }

    public partial struct GivenExpectationBuilder {

        public bool Assumption {
            get;
            private set;
        }


        internal GivenExpectationBuilder(bool assumption) {
          Assumption = assumption;
        }

        public IExpectationBuilder<TResult> Expect<TResult>(Func<TResult> func) {
            return new ExpectationBuilder<TResult>(() => func(),
                                                   false,
                                                   TextUtility.FormatArgs(),
                                                   Assumption);
        }

        public IExpectationBuilder Expect(Action func) {
            return new ExpectationBuilder(() => func(),
                                          false,
                                          TextUtility.FormatArgs(),
                                          Assumption);
        }

        public GivenRecord Record {
            get {
                return new GivenRecord(this);
            }
        }

        public struct GivenRecord {

          private readonly GivenExpectationBuilder _parent;

          internal GivenRecord(GivenExpectationBuilder parent) {
              _parent = parent;
          }

          public Exception Exception<TResult>(Func<TResult> func) {
              return Carbonfrost.Commons.Spec.Record.Exception(() => func());
          }

          public Exception Exception(Action func) {
              return Carbonfrost.Commons.Spec.Record.Exception(() => func());
          }

          public TestCodeDispatchInfo DispatchInfo<TResult>(Func<TResult> func, RecordExceptionFlags flags) {
              return Carbonfrost.Commons.Spec.Record.DispatchInfo(() => func(), flags);
          }

          public TestCodeDispatchInfo DispatchInfo(Action func, RecordExceptionFlags flags) {
              return Carbonfrost.Commons.Spec.Record.DispatchInfo(() => func(), flags);
          }

        }
    }


    partial class TestClass {

        public GivenExpectationBuilder<T> Given<T>(T arg1) {
            return Assert.Given<T>(arg1);
        }
    }

    partial class Asserter {

        public GivenExpectationBuilder<T> Given<T>(T arg1) {
            return new GivenExpectationBuilder<T>(arg1,  _assumption);
        }

    }

    partial class Assert {

        public static GivenExpectationBuilder<T> Given<T>(T arg1) {
            return Global.Given<T>(arg1);
        }

    }

    partial class Assume {

        public static GivenExpectationBuilder<T> Given<T>(T arg1) {
            return Global.Given<T>(arg1);
        }

    }

    public partial struct GivenExpectationBuilder<T> {

        public bool Assumption {
            get;
            private set;
        }

        public T Arg1 {
            get;
            private set;
        }

        internal GivenExpectationBuilder(T arg1, bool assumption) {
          Assumption = assumption;
        Arg1 = arg1;
        }

        public IExpectationBuilder<TResult> Expect<TResult>(Func<T, TResult> func) {
        var arg1 = Arg1;
            return new ExpectationBuilder<TResult>(() => func(arg1),
                                                   false,
                                                   TextUtility.FormatArgs(arg1),
                                                   Assumption);
        }

        public IExpectationBuilder Expect(Action<T> func) {
        var arg1 = Arg1;
            return new ExpectationBuilder(() => func(arg1),
                                          false,
                                          TextUtility.FormatArgs(arg1),
                                          Assumption);
        }

        public GivenRecord Record {
            get {
                return new GivenRecord(this);
            }
        }

        public struct GivenRecord {

          private readonly GivenExpectationBuilder<T> _parent;

          internal GivenRecord(GivenExpectationBuilder<T> parent) {
              _parent = parent;
          }

          public Exception Exception<TResult>(Func<T, TResult> func) {
              var arg1 = _parent.Arg1;
              return Carbonfrost.Commons.Spec.Record.Exception(() => func(arg1));
          }

          public Exception Exception(Action<T> func) {
              var arg1 = _parent.Arg1;
              return Carbonfrost.Commons.Spec.Record.Exception(() => func(arg1));
          }

          public TestCodeDispatchInfo DispatchInfo<TResult>(Func<T, TResult> func, RecordExceptionFlags flags) {
              var arg1 = _parent.Arg1;
              return Carbonfrost.Commons.Spec.Record.DispatchInfo(() => func(arg1), flags);
          }

          public TestCodeDispatchInfo DispatchInfo(Action<T> func, RecordExceptionFlags flags) {
              var arg1 = _parent.Arg1;
              return Carbonfrost.Commons.Spec.Record.DispatchInfo(() => func(arg1), flags);
          }

        }
    }


    partial class TestClass {

        public GivenExpectationBuilder<T1, T2> Given<T1, T2>(T1 arg1, T2 arg2) {
            return Assert.Given<T1, T2>(arg1, arg2);
        }
    }

    partial class Asserter {

        public GivenExpectationBuilder<T1, T2> Given<T1, T2>(T1 arg1, T2 arg2) {
            return new GivenExpectationBuilder<T1, T2>(arg1, arg2,  _assumption);
        }

    }

    partial class Assert {

        public static GivenExpectationBuilder<T1, T2> Given<T1, T2>(T1 arg1, T2 arg2) {
            return Global.Given<T1, T2>(arg1, arg2);
        }

    }

    partial class Assume {

        public static GivenExpectationBuilder<T1, T2> Given<T1, T2>(T1 arg1, T2 arg2) {
            return Global.Given<T1, T2>(arg1, arg2);
        }

    }

    public partial struct GivenExpectationBuilder<T1, T2> {

        public bool Assumption {
            get;
            private set;
        }

        public T1 Arg1 {
            get;
            private set;
        }
        public T2 Arg2 {
            get;
            private set;
        }

        internal GivenExpectationBuilder(T1 arg1, T2 arg2, bool assumption) {
          Assumption = assumption;
        Arg1 = arg1;
        Arg2 = arg2;
        }

        public IExpectationBuilder<TResult> Expect<TResult>(Func<T1, T2, TResult> func) {
        var arg1 = Arg1;
        var arg2 = Arg2;
            return new ExpectationBuilder<TResult>(() => func(arg1, arg2),
                                                   false,
                                                   TextUtility.FormatArgs(arg1, arg2),
                                                   Assumption);
        }

        public IExpectationBuilder Expect(Action<T1, T2> func) {
        var arg1 = Arg1;
        var arg2 = Arg2;
            return new ExpectationBuilder(() => func(arg1, arg2),
                                          false,
                                          TextUtility.FormatArgs(arg1, arg2),
                                          Assumption);
        }

        public GivenRecord Record {
            get {
                return new GivenRecord(this);
            }
        }

        public struct GivenRecord {

          private readonly GivenExpectationBuilder<T1, T2> _parent;

          internal GivenRecord(GivenExpectationBuilder<T1, T2> parent) {
              _parent = parent;
          }

          public Exception Exception<TResult>(Func<T1, T2, TResult> func) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              return Carbonfrost.Commons.Spec.Record.Exception(() => func(arg1, arg2));
          }

          public Exception Exception(Action<T1, T2> func) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              return Carbonfrost.Commons.Spec.Record.Exception(() => func(arg1, arg2));
          }

          public TestCodeDispatchInfo DispatchInfo<TResult>(Func<T1, T2, TResult> func, RecordExceptionFlags flags) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              return Carbonfrost.Commons.Spec.Record.DispatchInfo(() => func(arg1, arg2), flags);
          }

          public TestCodeDispatchInfo DispatchInfo(Action<T1, T2> func, RecordExceptionFlags flags) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              return Carbonfrost.Commons.Spec.Record.DispatchInfo(() => func(arg1, arg2), flags);
          }

        }
    }


    partial class TestClass {

        public GivenExpectationBuilder<T1, T2, T3> Given<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3) {
            return Assert.Given<T1, T2, T3>(arg1, arg2, arg3);
        }
    }

    partial class Asserter {

        public GivenExpectationBuilder<T1, T2, T3> Given<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3) {
            return new GivenExpectationBuilder<T1, T2, T3>(arg1, arg2, arg3,  _assumption);
        }

    }

    partial class Assert {

        public static GivenExpectationBuilder<T1, T2, T3> Given<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3) {
            return Global.Given<T1, T2, T3>(arg1, arg2, arg3);
        }

    }

    partial class Assume {

        public static GivenExpectationBuilder<T1, T2, T3> Given<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3) {
            return Global.Given<T1, T2, T3>(arg1, arg2, arg3);
        }

    }

    public partial struct GivenExpectationBuilder<T1, T2, T3> {

        public bool Assumption {
            get;
            private set;
        }

        public T1 Arg1 {
            get;
            private set;
        }
        public T2 Arg2 {
            get;
            private set;
        }
        public T3 Arg3 {
            get;
            private set;
        }

        internal GivenExpectationBuilder(T1 arg1, T2 arg2, T3 arg3, bool assumption) {
          Assumption = assumption;
        Arg1 = arg1;
        Arg2 = arg2;
        Arg3 = arg3;
        }

        public IExpectationBuilder<TResult> Expect<TResult>(Func<T1, T2, T3, TResult> func) {
        var arg1 = Arg1;
        var arg2 = Arg2;
        var arg3 = Arg3;
            return new ExpectationBuilder<TResult>(() => func(arg1, arg2, arg3),
                                                   false,
                                                   TextUtility.FormatArgs(arg1, arg2, arg3),
                                                   Assumption);
        }

        public IExpectationBuilder Expect(Action<T1, T2, T3> func) {
        var arg1 = Arg1;
        var arg2 = Arg2;
        var arg3 = Arg3;
            return new ExpectationBuilder(() => func(arg1, arg2, arg3),
                                          false,
                                          TextUtility.FormatArgs(arg1, arg2, arg3),
                                          Assumption);
        }

        public GivenRecord Record {
            get {
                return new GivenRecord(this);
            }
        }

        public struct GivenRecord {

          private readonly GivenExpectationBuilder<T1, T2, T3> _parent;

          internal GivenRecord(GivenExpectationBuilder<T1, T2, T3> parent) {
              _parent = parent;
          }

          public Exception Exception<TResult>(Func<T1, T2, T3, TResult> func) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              return Carbonfrost.Commons.Spec.Record.Exception(() => func(arg1, arg2, arg3));
          }

          public Exception Exception(Action<T1, T2, T3> func) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              return Carbonfrost.Commons.Spec.Record.Exception(() => func(arg1, arg2, arg3));
          }

          public TestCodeDispatchInfo DispatchInfo<TResult>(Func<T1, T2, T3, TResult> func, RecordExceptionFlags flags) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              return Carbonfrost.Commons.Spec.Record.DispatchInfo(() => func(arg1, arg2, arg3), flags);
          }

          public TestCodeDispatchInfo DispatchInfo(Action<T1, T2, T3> func, RecordExceptionFlags flags) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              return Carbonfrost.Commons.Spec.Record.DispatchInfo(() => func(arg1, arg2, arg3), flags);
          }

        }
    }


    partial class TestClass {

        public GivenExpectationBuilder<T1, T2, T3, T4> Given<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
            return Assert.Given<T1, T2, T3, T4>(arg1, arg2, arg3, arg4);
        }
    }

    partial class Asserter {

        public GivenExpectationBuilder<T1, T2, T3, T4> Given<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
            return new GivenExpectationBuilder<T1, T2, T3, T4>(arg1, arg2, arg3, arg4,  _assumption);
        }

    }

    partial class Assert {

        public static GivenExpectationBuilder<T1, T2, T3, T4> Given<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
            return Global.Given<T1, T2, T3, T4>(arg1, arg2, arg3, arg4);
        }

    }

    partial class Assume {

        public static GivenExpectationBuilder<T1, T2, T3, T4> Given<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
            return Global.Given<T1, T2, T3, T4>(arg1, arg2, arg3, arg4);
        }

    }

    public partial struct GivenExpectationBuilder<T1, T2, T3, T4> {

        public bool Assumption {
            get;
            private set;
        }

        public T1 Arg1 {
            get;
            private set;
        }
        public T2 Arg2 {
            get;
            private set;
        }
        public T3 Arg3 {
            get;
            private set;
        }
        public T4 Arg4 {
            get;
            private set;
        }

        internal GivenExpectationBuilder(T1 arg1, T2 arg2, T3 arg3, T4 arg4, bool assumption) {
          Assumption = assumption;
        Arg1 = arg1;
        Arg2 = arg2;
        Arg3 = arg3;
        Arg4 = arg4;
        }

        public IExpectationBuilder<TResult> Expect<TResult>(Func<T1, T2, T3, T4, TResult> func) {
        var arg1 = Arg1;
        var arg2 = Arg2;
        var arg3 = Arg3;
        var arg4 = Arg4;
            return new ExpectationBuilder<TResult>(() => func(arg1, arg2, arg3, arg4),
                                                   false,
                                                   TextUtility.FormatArgs(arg1, arg2, arg3, arg4),
                                                   Assumption);
        }

        public IExpectationBuilder Expect(Action<T1, T2, T3, T4> func) {
        var arg1 = Arg1;
        var arg2 = Arg2;
        var arg3 = Arg3;
        var arg4 = Arg4;
            return new ExpectationBuilder(() => func(arg1, arg2, arg3, arg4),
                                          false,
                                          TextUtility.FormatArgs(arg1, arg2, arg3, arg4),
                                          Assumption);
        }

        public GivenRecord Record {
            get {
                return new GivenRecord(this);
            }
        }

        public struct GivenRecord {

          private readonly GivenExpectationBuilder<T1, T2, T3, T4> _parent;

          internal GivenRecord(GivenExpectationBuilder<T1, T2, T3, T4> parent) {
              _parent = parent;
          }

          public Exception Exception<TResult>(Func<T1, T2, T3, T4, TResult> func) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              var arg4 = _parent.Arg4;
              return Carbonfrost.Commons.Spec.Record.Exception(() => func(arg1, arg2, arg3, arg4));
          }

          public Exception Exception(Action<T1, T2, T3, T4> func) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              var arg4 = _parent.Arg4;
              return Carbonfrost.Commons.Spec.Record.Exception(() => func(arg1, arg2, arg3, arg4));
          }

          public TestCodeDispatchInfo DispatchInfo<TResult>(Func<T1, T2, T3, T4, TResult> func, RecordExceptionFlags flags) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              var arg4 = _parent.Arg4;
              return Carbonfrost.Commons.Spec.Record.DispatchInfo(() => func(arg1, arg2, arg3, arg4), flags);
          }

          public TestCodeDispatchInfo DispatchInfo(Action<T1, T2, T3, T4> func, RecordExceptionFlags flags) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              var arg4 = _parent.Arg4;
              return Carbonfrost.Commons.Spec.Record.DispatchInfo(() => func(arg1, arg2, arg3, arg4), flags);
          }

        }
    }


    partial class TestClass {

        public GivenExpectationBuilder<T1, T2, T3, T4, T5> Given<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
            return Assert.Given<T1, T2, T3, T4, T5>(arg1, arg2, arg3, arg4, arg5);
        }
    }

    partial class Asserter {

        public GivenExpectationBuilder<T1, T2, T3, T4, T5> Given<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
            return new GivenExpectationBuilder<T1, T2, T3, T4, T5>(arg1, arg2, arg3, arg4, arg5,  _assumption);
        }

    }

    partial class Assert {

        public static GivenExpectationBuilder<T1, T2, T3, T4, T5> Given<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
            return Global.Given<T1, T2, T3, T4, T5>(arg1, arg2, arg3, arg4, arg5);
        }

    }

    partial class Assume {

        public static GivenExpectationBuilder<T1, T2, T3, T4, T5> Given<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
            return Global.Given<T1, T2, T3, T4, T5>(arg1, arg2, arg3, arg4, arg5);
        }

    }

    public partial struct GivenExpectationBuilder<T1, T2, T3, T4, T5> {

        public bool Assumption {
            get;
            private set;
        }

        public T1 Arg1 {
            get;
            private set;
        }
        public T2 Arg2 {
            get;
            private set;
        }
        public T3 Arg3 {
            get;
            private set;
        }
        public T4 Arg4 {
            get;
            private set;
        }
        public T5 Arg5 {
            get;
            private set;
        }

        internal GivenExpectationBuilder(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, bool assumption) {
          Assumption = assumption;
        Arg1 = arg1;
        Arg2 = arg2;
        Arg3 = arg3;
        Arg4 = arg4;
        Arg5 = arg5;
        }

        public IExpectationBuilder<TResult> Expect<TResult>(Func<T1, T2, T3, T4, T5, TResult> func) {
        var arg1 = Arg1;
        var arg2 = Arg2;
        var arg3 = Arg3;
        var arg4 = Arg4;
        var arg5 = Arg5;
            return new ExpectationBuilder<TResult>(() => func(arg1, arg2, arg3, arg4, arg5),
                                                   false,
                                                   TextUtility.FormatArgs(arg1, arg2, arg3, arg4, arg5),
                                                   Assumption);
        }

        public IExpectationBuilder Expect(Action<T1, T2, T3, T4, T5> func) {
        var arg1 = Arg1;
        var arg2 = Arg2;
        var arg3 = Arg3;
        var arg4 = Arg4;
        var arg5 = Arg5;
            return new ExpectationBuilder(() => func(arg1, arg2, arg3, arg4, arg5),
                                          false,
                                          TextUtility.FormatArgs(arg1, arg2, arg3, arg4, arg5),
                                          Assumption);
        }

        public GivenRecord Record {
            get {
                return new GivenRecord(this);
            }
        }

        public struct GivenRecord {

          private readonly GivenExpectationBuilder<T1, T2, T3, T4, T5> _parent;

          internal GivenRecord(GivenExpectationBuilder<T1, T2, T3, T4, T5> parent) {
              _parent = parent;
          }

          public Exception Exception<TResult>(Func<T1, T2, T3, T4, T5, TResult> func) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              var arg4 = _parent.Arg4;
              var arg5 = _parent.Arg5;
              return Carbonfrost.Commons.Spec.Record.Exception(() => func(arg1, arg2, arg3, arg4, arg5));
          }

          public Exception Exception(Action<T1, T2, T3, T4, T5> func) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              var arg4 = _parent.Arg4;
              var arg5 = _parent.Arg5;
              return Carbonfrost.Commons.Spec.Record.Exception(() => func(arg1, arg2, arg3, arg4, arg5));
          }

          public TestCodeDispatchInfo DispatchInfo<TResult>(Func<T1, T2, T3, T4, T5, TResult> func, RecordExceptionFlags flags) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              var arg4 = _parent.Arg4;
              var arg5 = _parent.Arg5;
              return Carbonfrost.Commons.Spec.Record.DispatchInfo(() => func(arg1, arg2, arg3, arg4, arg5), flags);
          }

          public TestCodeDispatchInfo DispatchInfo(Action<T1, T2, T3, T4, T5> func, RecordExceptionFlags flags) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              var arg4 = _parent.Arg4;
              var arg5 = _parent.Arg5;
              return Carbonfrost.Commons.Spec.Record.DispatchInfo(() => func(arg1, arg2, arg3, arg4, arg5), flags);
          }

        }
    }


    partial class TestClass {

        public GivenExpectationBuilder<T1, T2, T3, T4, T5, T6> Given<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
            return Assert.Given<T1, T2, T3, T4, T5, T6>(arg1, arg2, arg3, arg4, arg5, arg6);
        }
    }

    partial class Asserter {

        public GivenExpectationBuilder<T1, T2, T3, T4, T5, T6> Given<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
            return new GivenExpectationBuilder<T1, T2, T3, T4, T5, T6>(arg1, arg2, arg3, arg4, arg5, arg6,  _assumption);
        }

    }

    partial class Assert {

        public static GivenExpectationBuilder<T1, T2, T3, T4, T5, T6> Given<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
            return Global.Given<T1, T2, T3, T4, T5, T6>(arg1, arg2, arg3, arg4, arg5, arg6);
        }

    }

    partial class Assume {

        public static GivenExpectationBuilder<T1, T2, T3, T4, T5, T6> Given<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
            return Global.Given<T1, T2, T3, T4, T5, T6>(arg1, arg2, arg3, arg4, arg5, arg6);
        }

    }

    public partial struct GivenExpectationBuilder<T1, T2, T3, T4, T5, T6> {

        public bool Assumption {
            get;
            private set;
        }

        public T1 Arg1 {
            get;
            private set;
        }
        public T2 Arg2 {
            get;
            private set;
        }
        public T3 Arg3 {
            get;
            private set;
        }
        public T4 Arg4 {
            get;
            private set;
        }
        public T5 Arg5 {
            get;
            private set;
        }
        public T6 Arg6 {
            get;
            private set;
        }

        internal GivenExpectationBuilder(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, bool assumption) {
          Assumption = assumption;
        Arg1 = arg1;
        Arg2 = arg2;
        Arg3 = arg3;
        Arg4 = arg4;
        Arg5 = arg5;
        Arg6 = arg6;
        }

        public IExpectationBuilder<TResult> Expect<TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func) {
        var arg1 = Arg1;
        var arg2 = Arg2;
        var arg3 = Arg3;
        var arg4 = Arg4;
        var arg5 = Arg5;
        var arg6 = Arg6;
            return new ExpectationBuilder<TResult>(() => func(arg1, arg2, arg3, arg4, arg5, arg6),
                                                   false,
                                                   TextUtility.FormatArgs(arg1, arg2, arg3, arg4, arg5, arg6),
                                                   Assumption);
        }

        public IExpectationBuilder Expect(Action<T1, T2, T3, T4, T5, T6> func) {
        var arg1 = Arg1;
        var arg2 = Arg2;
        var arg3 = Arg3;
        var arg4 = Arg4;
        var arg5 = Arg5;
        var arg6 = Arg6;
            return new ExpectationBuilder(() => func(arg1, arg2, arg3, arg4, arg5, arg6),
                                          false,
                                          TextUtility.FormatArgs(arg1, arg2, arg3, arg4, arg5, arg6),
                                          Assumption);
        }

        public GivenRecord Record {
            get {
                return new GivenRecord(this);
            }
        }

        public struct GivenRecord {

          private readonly GivenExpectationBuilder<T1, T2, T3, T4, T5, T6> _parent;

          internal GivenRecord(GivenExpectationBuilder<T1, T2, T3, T4, T5, T6> parent) {
              _parent = parent;
          }

          public Exception Exception<TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              var arg4 = _parent.Arg4;
              var arg5 = _parent.Arg5;
              var arg6 = _parent.Arg6;
              return Carbonfrost.Commons.Spec.Record.Exception(() => func(arg1, arg2, arg3, arg4, arg5, arg6));
          }

          public Exception Exception(Action<T1, T2, T3, T4, T5, T6> func) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              var arg4 = _parent.Arg4;
              var arg5 = _parent.Arg5;
              var arg6 = _parent.Arg6;
              return Carbonfrost.Commons.Spec.Record.Exception(() => func(arg1, arg2, arg3, arg4, arg5, arg6));
          }

          public TestCodeDispatchInfo DispatchInfo<TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func, RecordExceptionFlags flags) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              var arg4 = _parent.Arg4;
              var arg5 = _parent.Arg5;
              var arg6 = _parent.Arg6;
              return Carbonfrost.Commons.Spec.Record.DispatchInfo(() => func(arg1, arg2, arg3, arg4, arg5, arg6), flags);
          }

          public TestCodeDispatchInfo DispatchInfo(Action<T1, T2, T3, T4, T5, T6> func, RecordExceptionFlags flags) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              var arg4 = _parent.Arg4;
              var arg5 = _parent.Arg5;
              var arg6 = _parent.Arg6;
              return Carbonfrost.Commons.Spec.Record.DispatchInfo(() => func(arg1, arg2, arg3, arg4, arg5, arg6), flags);
          }

        }
    }


    partial class TestClass {

        public GivenExpectationBuilder<T1, T2, T3, T4, T5, T6, T7> Given<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
            return Assert.Given<T1, T2, T3, T4, T5, T6, T7>(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
    }

    partial class Asserter {

        public GivenExpectationBuilder<T1, T2, T3, T4, T5, T6, T7> Given<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
            return new GivenExpectationBuilder<T1, T2, T3, T4, T5, T6, T7>(arg1, arg2, arg3, arg4, arg5, arg6, arg7,  _assumption);
        }

    }

    partial class Assert {

        public static GivenExpectationBuilder<T1, T2, T3, T4, T5, T6, T7> Given<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
            return Global.Given<T1, T2, T3, T4, T5, T6, T7>(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

    }

    partial class Assume {

        public static GivenExpectationBuilder<T1, T2, T3, T4, T5, T6, T7> Given<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
            return Global.Given<T1, T2, T3, T4, T5, T6, T7>(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

    }

    public partial struct GivenExpectationBuilder<T1, T2, T3, T4, T5, T6, T7> {

        public bool Assumption {
            get;
            private set;
        }

        public T1 Arg1 {
            get;
            private set;
        }
        public T2 Arg2 {
            get;
            private set;
        }
        public T3 Arg3 {
            get;
            private set;
        }
        public T4 Arg4 {
            get;
            private set;
        }
        public T5 Arg5 {
            get;
            private set;
        }
        public T6 Arg6 {
            get;
            private set;
        }
        public T7 Arg7 {
            get;
            private set;
        }

        internal GivenExpectationBuilder(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, bool assumption) {
          Assumption = assumption;
        Arg1 = arg1;
        Arg2 = arg2;
        Arg3 = arg3;
        Arg4 = arg4;
        Arg5 = arg5;
        Arg6 = arg6;
        Arg7 = arg7;
        }

        public IExpectationBuilder<TResult> Expect<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func) {
        var arg1 = Arg1;
        var arg2 = Arg2;
        var arg3 = Arg3;
        var arg4 = Arg4;
        var arg5 = Arg5;
        var arg6 = Arg6;
        var arg7 = Arg7;
            return new ExpectationBuilder<TResult>(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7),
                                                   false,
                                                   TextUtility.FormatArgs(arg1, arg2, arg3, arg4, arg5, arg6, arg7),
                                                   Assumption);
        }

        public IExpectationBuilder Expect(Action<T1, T2, T3, T4, T5, T6, T7> func) {
        var arg1 = Arg1;
        var arg2 = Arg2;
        var arg3 = Arg3;
        var arg4 = Arg4;
        var arg5 = Arg5;
        var arg6 = Arg6;
        var arg7 = Arg7;
            return new ExpectationBuilder(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7),
                                          false,
                                          TextUtility.FormatArgs(arg1, arg2, arg3, arg4, arg5, arg6, arg7),
                                          Assumption);
        }

        public GivenRecord Record {
            get {
                return new GivenRecord(this);
            }
        }

        public struct GivenRecord {

          private readonly GivenExpectationBuilder<T1, T2, T3, T4, T5, T6, T7> _parent;

          internal GivenRecord(GivenExpectationBuilder<T1, T2, T3, T4, T5, T6, T7> parent) {
              _parent = parent;
          }

          public Exception Exception<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              var arg4 = _parent.Arg4;
              var arg5 = _parent.Arg5;
              var arg6 = _parent.Arg6;
              var arg7 = _parent.Arg7;
              return Carbonfrost.Commons.Spec.Record.Exception(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
          }

          public Exception Exception(Action<T1, T2, T3, T4, T5, T6, T7> func) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              var arg4 = _parent.Arg4;
              var arg5 = _parent.Arg5;
              var arg6 = _parent.Arg6;
              var arg7 = _parent.Arg7;
              return Carbonfrost.Commons.Spec.Record.Exception(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
          }

          public TestCodeDispatchInfo DispatchInfo<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, RecordExceptionFlags flags) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              var arg4 = _parent.Arg4;
              var arg5 = _parent.Arg5;
              var arg6 = _parent.Arg6;
              var arg7 = _parent.Arg7;
              return Carbonfrost.Commons.Spec.Record.DispatchInfo(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7), flags);
          }

          public TestCodeDispatchInfo DispatchInfo(Action<T1, T2, T3, T4, T5, T6, T7> func, RecordExceptionFlags flags) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              var arg4 = _parent.Arg4;
              var arg5 = _parent.Arg5;
              var arg6 = _parent.Arg6;
              var arg7 = _parent.Arg7;
              return Carbonfrost.Commons.Spec.Record.DispatchInfo(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7), flags);
          }

        }
    }


    partial class TestClass {

        public GivenExpectationBuilder<T1, T2, T3, T4, T5, T6, T7, T8> Given<T1, T2, T3, T4, T5, T6, T7, T8>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
            return Assert.Given<T1, T2, T3, T4, T5, T6, T7, T8>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
    }

    partial class Asserter {

        public GivenExpectationBuilder<T1, T2, T3, T4, T5, T6, T7, T8> Given<T1, T2, T3, T4, T5, T6, T7, T8>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
            return new GivenExpectationBuilder<T1, T2, T3, T4, T5, T6, T7, T8>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8,  _assumption);
        }

    }

    partial class Assert {

        public static GivenExpectationBuilder<T1, T2, T3, T4, T5, T6, T7, T8> Given<T1, T2, T3, T4, T5, T6, T7, T8>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
            return Global.Given<T1, T2, T3, T4, T5, T6, T7, T8>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

    }

    partial class Assume {

        public static GivenExpectationBuilder<T1, T2, T3, T4, T5, T6, T7, T8> Given<T1, T2, T3, T4, T5, T6, T7, T8>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
            return Global.Given<T1, T2, T3, T4, T5, T6, T7, T8>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

    }

    public partial struct GivenExpectationBuilder<T1, T2, T3, T4, T5, T6, T7, T8> {

        public bool Assumption {
            get;
            private set;
        }

        public T1 Arg1 {
            get;
            private set;
        }
        public T2 Arg2 {
            get;
            private set;
        }
        public T3 Arg3 {
            get;
            private set;
        }
        public T4 Arg4 {
            get;
            private set;
        }
        public T5 Arg5 {
            get;
            private set;
        }
        public T6 Arg6 {
            get;
            private set;
        }
        public T7 Arg7 {
            get;
            private set;
        }
        public T8 Arg8 {
            get;
            private set;
        }

        internal GivenExpectationBuilder(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, bool assumption) {
          Assumption = assumption;
        Arg1 = arg1;
        Arg2 = arg2;
        Arg3 = arg3;
        Arg4 = arg4;
        Arg5 = arg5;
        Arg6 = arg6;
        Arg7 = arg7;
        Arg8 = arg8;
        }

        public IExpectationBuilder<TResult> Expect<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func) {
        var arg1 = Arg1;
        var arg2 = Arg2;
        var arg3 = Arg3;
        var arg4 = Arg4;
        var arg5 = Arg5;
        var arg6 = Arg6;
        var arg7 = Arg7;
        var arg8 = Arg8;
            return new ExpectationBuilder<TResult>(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8),
                                                   false,
                                                   TextUtility.FormatArgs(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8),
                                                   Assumption);
        }

        public IExpectationBuilder Expect(Action<T1, T2, T3, T4, T5, T6, T7, T8> func) {
        var arg1 = Arg1;
        var arg2 = Arg2;
        var arg3 = Arg3;
        var arg4 = Arg4;
        var arg5 = Arg5;
        var arg6 = Arg6;
        var arg7 = Arg7;
        var arg8 = Arg8;
            return new ExpectationBuilder(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8),
                                          false,
                                          TextUtility.FormatArgs(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8),
                                          Assumption);
        }

        public GivenRecord Record {
            get {
                return new GivenRecord(this);
            }
        }

        public struct GivenRecord {

          private readonly GivenExpectationBuilder<T1, T2, T3, T4, T5, T6, T7, T8> _parent;

          internal GivenRecord(GivenExpectationBuilder<T1, T2, T3, T4, T5, T6, T7, T8> parent) {
              _parent = parent;
          }

          public Exception Exception<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              var arg4 = _parent.Arg4;
              var arg5 = _parent.Arg5;
              var arg6 = _parent.Arg6;
              var arg7 = _parent.Arg7;
              var arg8 = _parent.Arg8;
              return Carbonfrost.Commons.Spec.Record.Exception(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
          }

          public Exception Exception(Action<T1, T2, T3, T4, T5, T6, T7, T8> func) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              var arg4 = _parent.Arg4;
              var arg5 = _parent.Arg5;
              var arg6 = _parent.Arg6;
              var arg7 = _parent.Arg7;
              var arg8 = _parent.Arg8;
              return Carbonfrost.Commons.Spec.Record.Exception(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
          }

          public TestCodeDispatchInfo DispatchInfo<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, RecordExceptionFlags flags) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              var arg4 = _parent.Arg4;
              var arg5 = _parent.Arg5;
              var arg6 = _parent.Arg6;
              var arg7 = _parent.Arg7;
              var arg8 = _parent.Arg8;
              return Carbonfrost.Commons.Spec.Record.DispatchInfo(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), flags);
          }

          public TestCodeDispatchInfo DispatchInfo(Action<T1, T2, T3, T4, T5, T6, T7, T8> func, RecordExceptionFlags flags) {
              var arg1 = _parent.Arg1;
              var arg2 = _parent.Arg2;
              var arg3 = _parent.Arg3;
              var arg4 = _parent.Arg4;
              var arg5 = _parent.Arg5;
              var arg6 = _parent.Arg6;
              var arg7 = _parent.Arg7;
              var arg8 = _parent.Arg8;
              return Carbonfrost.Commons.Spec.Record.DispatchInfo(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), flags);
          }

        }
    }


}
