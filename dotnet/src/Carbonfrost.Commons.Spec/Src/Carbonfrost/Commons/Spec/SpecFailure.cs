//
// Copyright 2016, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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

using Carbonfrost.Commons.Spec.ExecutionModel;
using Carbonfrost.Commons.Spec.Resources;

namespace Carbonfrost.Commons.Spec {

    static partial class SpecFailure {

        public static ArgumentException BadEpsilonComparerTypes(Type typeSelf) {
            return new ArgumentException(SR.BadEpsilonComparerTypes(typeSelf));
        }

        public static ArgumentException NegativeTimeout(string argumentName) {
            return new ArgumentException(SR.NegativeTimeout(), argumentName);
        }

        public static AssertException TestTimedOut(TimeSpan timeout) {
            return new AssertException(SR.TestTimedOut(timeout));
        }

        public static Exception CannotAliasDifferentTagNames() {
            return new ArgumentNullException(SR.CannotAliasDifferentTagNames());
        }

        public static InvalidOperationException FactMethodParamCount(string name) {
            return new InvalidOperationException(SR.FactMethodParamCount(name));
        }

        public static InvalidOperationException MultiAccessorsTheoryParameterMismatch() {
            return new InvalidOperationException(SR.MultiAccessorsTheoryParameterMismatch());
        }

        public static ArgumentException TemporaryDirectoryFileNameRooted(string argumentName) {
            return new ArgumentException(SR.TemporaryDirectoryFileNameRooted(), argumentName);
        }

        public static InvalidOperationException CannotFindDataProperty(object name) {
            return new InvalidOperationException(SR.CannotFindDataProperty(name));
        }

        public static InvalidOperationException CannotFindDataField(string name) {
            return new InvalidOperationException(SR.CannotFindDataField(name));
        }

        public static InvalidOperationException CannotFindFixture(string fileName, IEnumerable<string> fixtureDirectories) {
            return new InvalidOperationException(SR.CannotFindFixture(fileName, string.Join(":", fixtureDirectories)));
        }

        public static InvalidOperationException DataPropertyIncorrectGetter() {
            return new InvalidOperationException(SR.DataPropertyIncorrectGetter());
        }

        public static AssertException WrongNumberOfTheoryArguments(string typeName, string methodName, object index) {
            return new AssertException(SR.WrongNumberOfTheoryArguments(index, typeName, methodName));
        }

        public static InvalidOperationException CannotUseNullOnValueType(Type type) {
            return new InvalidOperationException(SR.CannotUseNullOnValueType(type));
        }

        public static AssertException CannotUseInstanceOfOnNullActual() {
            return new AssertException(SR.CannotUseInstanceOfOnNullActual());
        }

        public static AssertException CastRequiredByMatcherFailure(Exception inner, object matcher) {
            return new AssertException(SR.CastRequiredByMatcherFailure(matcher), inner);
        }

        public static AssertException HaveLengthWorksWith(Type type) {
            return new AssertException(SR.HaveLengthWorksWith());
        }

        public static AssertException CannotTreatAsDictionaryOrGroupings(Type type) {
            return new AssertException(SR.CannotTreatAsDictionaryOrGroupings(type));
        }

        public static AssertException TestFileDataRequiresOneParameter() {
            return new AssertException(SR.TestFileDataRequiresOneParameter());
        }

        public static AssertException UnusableComparer(TestMatcherName name, object comparer, Exception e) {
            var failure = new TestFailure(name) {
                UserData = {
                    { "Comparer", TextUtility.ConvertToString(comparer) }
                }
            };
            return new AssertException(
                SR.UnusableComparer(), failure, e
            );
        }

        public static AssertException ExplicitPassNotSet() {
            return new AssertException(SR.ExplicitPassNotSet());
        }

        public static AssertVerificationException ExactlyOnePlural() {
            return new AssertVerificationException(SR.ExactlyOnePlural());
        }

        public static AssertVerificationException ExactlyOneSingular() {
            return new AssertVerificationException(SR.ExactlyOneSingular());
        }

        public static AssertException SequenceNullConversion() {
            return new AssertException(SR.SequenceNullConversion());
        }

        public static AssertException CouldNotLoadType(Type type, Exception ex) {
            return new AssertException(SR.CouldNotLoadType(type), ex);
        }

        public static AssertVerificationException NegativeCardinality() {
            return new AssertVerificationException(SR.NegativeCardinality());
        }

        public static AssertVerificationException CardinalityMinGreaterThanMax() {
            return new AssertVerificationException(SR.CardinalityMinGreaterThanMax());
        }

        public static AssertVerificationException CannotAssertAssertExceptions() {
            return new AssertVerificationException(SR.CannotAssertAssertExceptions());
        }

        public static Exception Sealed() {
            return new InvalidOperationException(SR.Sealed());
        }

        public static Exception Disposed(string name) {
             return new ObjectDisposedException(name);
        }

        internal static Exception EmptyCollection(string argName) {
            return new ArgumentException("", argName);
        }

        internal static Exception ReadOnlyCollection() {
            return new InvalidOperationException();
        }

        internal static Exception NotParsable(string argName, Type type) {
            return new InvalidOperationException();
        }

        internal static Exception EmptyString(string argName) {
            return new InvalidOperationException();
        }

        internal static Exception AllWhitespace(string argName) {
            return new ArgumentException(SR.AllWhitespace(), argName);
        }

        internal static Exception NotValidDataUri() {
            return new ArgumentException();
        }

        internal static SpecException FailedToLoadAssembly(string asmPath) {
            return new SpecException(SR.FailedToLoadAssembly(asmPath));
        }

        internal static Exception FailedToLoadAssemblyPath(string asmPath) {
            return new SpecException(SR.FailedToLoadAssemblyPath(asmPath));
        }

        internal static Exception FailedToLoadAssemblyGeneralIO(string asmPath, string message) {
            return new SpecException(SR.FailedToLoadAssemblyGeneralIO(asmPath, message));
        }

        internal static ParserException FixtureParserIllegalTabs(int line) {
            return new ParserException(SR.ParserErrorLinePosition(
                SR.FixtureParserIllegalTabs(),
                line
            ));
        }

        internal static ParserException FixtureParserMissingFieldSeparator(int line) {
            return new ParserException(SR.ParserErrorLinePosition(
                SR.FixtureParserMissingFieldSeparator(),
                line
            ));
        }
    }
}
