
//
// File was automatically generated at 05/07/2020 18:58:17
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

    public struct GivenExpectationBuilder {

        private readonly bool _assumption;


        internal GivenExpectationBuilder(bool assumption) {
          _assumption = assumption;
        }

        public IExpectationBuilder<TResult> Expect<TResult>(Func<TResult> func) {
            return new ExpectationBuilder<TResult>(() => func(),
                                                   false,
                                                   TextUtility.FormatArgs(),
                                                   _assumption);
        }

        public IExpectationBuilder Expect(Action func) {
            return new ExpectationBuilder(() => func(),
                                          false,
                                          TextUtility.FormatArgs(),
                                          _assumption);
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

    public struct GivenExpectationBuilder<T> {

        private readonly bool _assumption;

        private readonly T _arg1;

        internal GivenExpectationBuilder(T arg1, bool assumption) {
          _assumption = assumption;
        _arg1 = arg1;
        }

        public IExpectationBuilder<TResult> Expect<TResult>(Func<T, TResult> func) {
        var arg1 = _arg1;
            return new ExpectationBuilder<TResult>(() => func(arg1),
                                                   false,
                                                   TextUtility.FormatArgs(arg1),
                                                   _assumption);
        }

        public IExpectationBuilder Expect(Action<T> func) {
        var arg1 = _arg1;
            return new ExpectationBuilder(() => func(arg1),
                                          false,
                                          TextUtility.FormatArgs(arg1),
                                          _assumption);
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

    public struct GivenExpectationBuilder<T1, T2> {

        private readonly bool _assumption;

        private readonly T1 _arg1;
        private readonly T2 _arg2;

        internal GivenExpectationBuilder(T1 arg1, T2 arg2, bool assumption) {
          _assumption = assumption;
        _arg1 = arg1;
        _arg2 = arg2;
        }

        public IExpectationBuilder<TResult> Expect<TResult>(Func<T1, T2, TResult> func) {
        var arg1 = _arg1;
        var arg2 = _arg2;
            return new ExpectationBuilder<TResult>(() => func(arg1, arg2),
                                                   false,
                                                   TextUtility.FormatArgs(arg1, arg2),
                                                   _assumption);
        }

        public IExpectationBuilder Expect(Action<T1, T2> func) {
        var arg1 = _arg1;
        var arg2 = _arg2;
            return new ExpectationBuilder(() => func(arg1, arg2),
                                          false,
                                          TextUtility.FormatArgs(arg1, arg2),
                                          _assumption);
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

    public struct GivenExpectationBuilder<T1, T2, T3> {

        private readonly bool _assumption;

        private readonly T1 _arg1;
        private readonly T2 _arg2;
        private readonly T3 _arg3;

        internal GivenExpectationBuilder(T1 arg1, T2 arg2, T3 arg3, bool assumption) {
          _assumption = assumption;
        _arg1 = arg1;
        _arg2 = arg2;
        _arg3 = arg3;
        }

        public IExpectationBuilder<TResult> Expect<TResult>(Func<T1, T2, T3, TResult> func) {
        var arg1 = _arg1;
        var arg2 = _arg2;
        var arg3 = _arg3;
            return new ExpectationBuilder<TResult>(() => func(arg1, arg2, arg3),
                                                   false,
                                                   TextUtility.FormatArgs(arg1, arg2, arg3),
                                                   _assumption);
        }

        public IExpectationBuilder Expect(Action<T1, T2, T3> func) {
        var arg1 = _arg1;
        var arg2 = _arg2;
        var arg3 = _arg3;
            return new ExpectationBuilder(() => func(arg1, arg2, arg3),
                                          false,
                                          TextUtility.FormatArgs(arg1, arg2, arg3),
                                          _assumption);
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

    public struct GivenExpectationBuilder<T1, T2, T3, T4> {

        private readonly bool _assumption;

        private readonly T1 _arg1;
        private readonly T2 _arg2;
        private readonly T3 _arg3;
        private readonly T4 _arg4;

        internal GivenExpectationBuilder(T1 arg1, T2 arg2, T3 arg3, T4 arg4, bool assumption) {
          _assumption = assumption;
        _arg1 = arg1;
        _arg2 = arg2;
        _arg3 = arg3;
        _arg4 = arg4;
        }

        public IExpectationBuilder<TResult> Expect<TResult>(Func<T1, T2, T3, T4, TResult> func) {
        var arg1 = _arg1;
        var arg2 = _arg2;
        var arg3 = _arg3;
        var arg4 = _arg4;
            return new ExpectationBuilder<TResult>(() => func(arg1, arg2, arg3, arg4),
                                                   false,
                                                   TextUtility.FormatArgs(arg1, arg2, arg3, arg4),
                                                   _assumption);
        }

        public IExpectationBuilder Expect(Action<T1, T2, T3, T4> func) {
        var arg1 = _arg1;
        var arg2 = _arg2;
        var arg3 = _arg3;
        var arg4 = _arg4;
            return new ExpectationBuilder(() => func(arg1, arg2, arg3, arg4),
                                          false,
                                          TextUtility.FormatArgs(arg1, arg2, arg3, arg4),
                                          _assumption);
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

    public struct GivenExpectationBuilder<T1, T2, T3, T4, T5> {

        private readonly bool _assumption;

        private readonly T1 _arg1;
        private readonly T2 _arg2;
        private readonly T3 _arg3;
        private readonly T4 _arg4;
        private readonly T5 _arg5;

        internal GivenExpectationBuilder(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, bool assumption) {
          _assumption = assumption;
        _arg1 = arg1;
        _arg2 = arg2;
        _arg3 = arg3;
        _arg4 = arg4;
        _arg5 = arg5;
        }

        public IExpectationBuilder<TResult> Expect<TResult>(Func<T1, T2, T3, T4, T5, TResult> func) {
        var arg1 = _arg1;
        var arg2 = _arg2;
        var arg3 = _arg3;
        var arg4 = _arg4;
        var arg5 = _arg5;
            return new ExpectationBuilder<TResult>(() => func(arg1, arg2, arg3, arg4, arg5),
                                                   false,
                                                   TextUtility.FormatArgs(arg1, arg2, arg3, arg4, arg5),
                                                   _assumption);
        }

        public IExpectationBuilder Expect(Action<T1, T2, T3, T4, T5> func) {
        var arg1 = _arg1;
        var arg2 = _arg2;
        var arg3 = _arg3;
        var arg4 = _arg4;
        var arg5 = _arg5;
            return new ExpectationBuilder(() => func(arg1, arg2, arg3, arg4, arg5),
                                          false,
                                          TextUtility.FormatArgs(arg1, arg2, arg3, arg4, arg5),
                                          _assumption);
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

    public struct GivenExpectationBuilder<T1, T2, T3, T4, T5, T6> {

        private readonly bool _assumption;

        private readonly T1 _arg1;
        private readonly T2 _arg2;
        private readonly T3 _arg3;
        private readonly T4 _arg4;
        private readonly T5 _arg5;
        private readonly T6 _arg6;

        internal GivenExpectationBuilder(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, bool assumption) {
          _assumption = assumption;
        _arg1 = arg1;
        _arg2 = arg2;
        _arg3 = arg3;
        _arg4 = arg4;
        _arg5 = arg5;
        _arg6 = arg6;
        }

        public IExpectationBuilder<TResult> Expect<TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func) {
        var arg1 = _arg1;
        var arg2 = _arg2;
        var arg3 = _arg3;
        var arg4 = _arg4;
        var arg5 = _arg5;
        var arg6 = _arg6;
            return new ExpectationBuilder<TResult>(() => func(arg1, arg2, arg3, arg4, arg5, arg6),
                                                   false,
                                                   TextUtility.FormatArgs(arg1, arg2, arg3, arg4, arg5, arg6),
                                                   _assumption);
        }

        public IExpectationBuilder Expect(Action<T1, T2, T3, T4, T5, T6> func) {
        var arg1 = _arg1;
        var arg2 = _arg2;
        var arg3 = _arg3;
        var arg4 = _arg4;
        var arg5 = _arg5;
        var arg6 = _arg6;
            return new ExpectationBuilder(() => func(arg1, arg2, arg3, arg4, arg5, arg6),
                                          false,
                                          TextUtility.FormatArgs(arg1, arg2, arg3, arg4, arg5, arg6),
                                          _assumption);
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

    public struct GivenExpectationBuilder<T1, T2, T3, T4, T5, T6, T7> {

        private readonly bool _assumption;

        private readonly T1 _arg1;
        private readonly T2 _arg2;
        private readonly T3 _arg3;
        private readonly T4 _arg4;
        private readonly T5 _arg5;
        private readonly T6 _arg6;
        private readonly T7 _arg7;

        internal GivenExpectationBuilder(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, bool assumption) {
          _assumption = assumption;
        _arg1 = arg1;
        _arg2 = arg2;
        _arg3 = arg3;
        _arg4 = arg4;
        _arg5 = arg5;
        _arg6 = arg6;
        _arg7 = arg7;
        }

        public IExpectationBuilder<TResult> Expect<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func) {
        var arg1 = _arg1;
        var arg2 = _arg2;
        var arg3 = _arg3;
        var arg4 = _arg4;
        var arg5 = _arg5;
        var arg6 = _arg6;
        var arg7 = _arg7;
            return new ExpectationBuilder<TResult>(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7),
                                                   false,
                                                   TextUtility.FormatArgs(arg1, arg2, arg3, arg4, arg5, arg6, arg7),
                                                   _assumption);
        }

        public IExpectationBuilder Expect(Action<T1, T2, T3, T4, T5, T6, T7> func) {
        var arg1 = _arg1;
        var arg2 = _arg2;
        var arg3 = _arg3;
        var arg4 = _arg4;
        var arg5 = _arg5;
        var arg6 = _arg6;
        var arg7 = _arg7;
            return new ExpectationBuilder(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7),
                                          false,
                                          TextUtility.FormatArgs(arg1, arg2, arg3, arg4, arg5, arg6, arg7),
                                          _assumption);
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

    public struct GivenExpectationBuilder<T1, T2, T3, T4, T5, T6, T7, T8> {

        private readonly bool _assumption;

        private readonly T1 _arg1;
        private readonly T2 _arg2;
        private readonly T3 _arg3;
        private readonly T4 _arg4;
        private readonly T5 _arg5;
        private readonly T6 _arg6;
        private readonly T7 _arg7;
        private readonly T8 _arg8;

        internal GivenExpectationBuilder(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, bool assumption) {
          _assumption = assumption;
        _arg1 = arg1;
        _arg2 = arg2;
        _arg3 = arg3;
        _arg4 = arg4;
        _arg5 = arg5;
        _arg6 = arg6;
        _arg7 = arg7;
        _arg8 = arg8;
        }

        public IExpectationBuilder<TResult> Expect<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func) {
        var arg1 = _arg1;
        var arg2 = _arg2;
        var arg3 = _arg3;
        var arg4 = _arg4;
        var arg5 = _arg5;
        var arg6 = _arg6;
        var arg7 = _arg7;
        var arg8 = _arg8;
            return new ExpectationBuilder<TResult>(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8),
                                                   false,
                                                   TextUtility.FormatArgs(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8),
                                                   _assumption);
        }

        public IExpectationBuilder Expect(Action<T1, T2, T3, T4, T5, T6, T7, T8> func) {
        var arg1 = _arg1;
        var arg2 = _arg2;
        var arg3 = _arg3;
        var arg4 = _arg4;
        var arg5 = _arg5;
        var arg6 = _arg6;
        var arg7 = _arg7;
        var arg8 = _arg8;
            return new ExpectationBuilder(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8),
                                          false,
                                          TextUtility.FormatArgs(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8),
                                          _assumption);
        }
    }


}
