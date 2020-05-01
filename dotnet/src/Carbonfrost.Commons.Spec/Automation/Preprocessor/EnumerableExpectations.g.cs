
//
// File was automatically generated at 04/30/2020 20:13:02
//

using System;
using System.Collections.Generic;

namespace Carbonfrost.Commons.Spec {

  partial class Extensions {


        public static void Distinct<TSource>(this Expectation<TSource[]> e) {
            Distinct<TSource>(e.As<IEnumerable<TSource>>(), message: (string) null);
        }

        public static void Distinct<TSource>(this Expectation<TSource[]> e, Comparison<TSource> comparison) {
            Distinct<TSource>(e.As<IEnumerable<TSource>>(), comparison, message: (string) null);
        }

        public static void Distinct<TSource>(this Expectation<TSource[]> e, IEqualityComparer<TSource> comparer) {
            Distinct<TSource>(e.As<IEnumerable<TSource>>(), comparer, message: (string) null);
        }

        public static void Distinct<TSource>(this Expectation<TSource[]> e, string message, params object[] args) {
            Distinct<TSource>(e.As<IEnumerable<TSource>>(), message, (object[]) args);
        }

        public static void Distinct<TSource>(this Expectation<TSource[]> e, Comparison<TSource> comparison, string message, params object[] args) {
            Distinct<TSource>(e.As<IEnumerable<TSource>>(), comparison, message, (object[]) args);
        }

        public static void Distinct<TSource>(this Expectation<TSource[]> e, IEqualityComparer<TSource> comparer, string message, params object[] args) {
            Distinct<TSource>(e.As<IEnumerable<TSource>>(), comparer, message, (object[]) args);
        }

    }

}

