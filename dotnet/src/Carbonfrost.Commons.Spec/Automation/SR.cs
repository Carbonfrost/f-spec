
// This file was automatically generated.  DO NOT EDIT or else
// your changes could be lost!

#pragma warning disable 1570

using System;
using System.Globalization;
using System.Resources;
using System.Reflection;

namespace Carbonfrost.Commons.Spec.Resources {

    /// <summary>
    /// Contains strongly-typed string resources.
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("srgen", "1.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
    internal static partial class SR {

        private static global::System.Resources.ResourceManager _resources;
        private static global::System.Globalization.CultureInfo _currentCulture;
        private static global::System.Func<string, string> _resourceFinder;

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(_resources, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Carbonfrost.Commons.Spec.Automation.SR", typeof(SR).GetTypeInfo().Assembly);
                    _resources = temp;
                }
                return _resources;
            }
        }

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return _currentCulture;
            }
            set {
                _currentCulture = value;
            }
        }

        private static global::System.Func<string, string> ResourceFinder {
            get {
                if (object.ReferenceEquals(_resourceFinder, null)) {
                    try {
                        global::System.Resources.ResourceManager rm = ResourceManager;
                        _resourceFinder = delegate (string s) {
                            return rm.GetString(s);
                        };
                    } catch (global::System.Exception ex) {
                        _resourceFinder = delegate (string s) {
                            return string.Format("localization error! {0}: {1} ({2})", s, ex.GetType(), ex.Message);
                        };
                    }
                }
                return _resourceFinder;
            }
        }


  /// <summary>Can't apply approximate comparisons to `${typeSelf}'.</summary>
    internal static string BadEpsilonComparerTypes(
    object @typeSelf
    ) {
        return string.Format(Culture, ResourceFinder("BadEpsilonComparerTypes") , @typeSelf);
    }

  /// <summary>Can't use `ExpectedExceptionAttribute` or `ThrowsAttribute` on a test that throws assertions exceptions if those exceptions might be caught by the attribute.  Replace with `Assert.Throws`.</summary>
    internal static string CannotAssertAssertExceptions(

    ) {
        return string.Format(Culture, ResourceFinder("CannotAssertAssertExceptions") );
    }

  /// <summary>Couldn't find data field ${name} on test class.</summary>
    internal static string CannotFindDataField(
    object @name
    ) {
        return string.Format(Culture, ResourceFinder("CannotFindDataField") , @name);
    }

  /// <summary>Couldn't find data property ${prop} on test class.</summary>
    internal static string CannotFindDataProperty(
    object @prop
    ) {
        return string.Format(Culture, ResourceFinder("CannotFindDataProperty") , @prop);
    }

  /// <summary>Can't find fixture '${fileName}' in (${searchDirectories})</summary>
    internal static string CannotFindFixture(
    object @fileName, object @searchDirectories
    ) {
        return string.Format(Culture, ResourceFinder("CannotFindFixture") , @fileName, @searchDirectories);
    }

  /// <summary>Can't assert that instance of `${type}' has keys because it doesn't support it.</summary>
    internal static string CannotTreatAsDictionaryOrGroupings(
    object @type
    ) {
        return string.Format(Culture, ResourceFinder("CannotTreatAsDictionaryOrGroupings") , @type);
    }

  /// <summary>Can't use instance-of or not-instance-of on null actual values.  Assert that the value is non-null before trying to assert its type.</summary>
    internal static string CannotUseInstanceOfOnNullActual(

    ) {
        return string.Format(Culture, ResourceFinder("CannotUseInstanceOfOnNullActual") );
    }

  /// <summary>Can't use null or not-null on value type ${type}.  Assert that the value is a reference type before trying to assert whether it is null.</summary>
    internal static string CannotUseNullOnValueType(
    object @type
    ) {
        return string.Format(Culture, ResourceFinder("CannotUseNullOnValueType") , @type);
    }

  /// <summary>Make sure that Between() specifies min < max</summary>
    internal static string CardinalityMinGreaterThanMax(

    ) {
        return string.Format(Culture, ResourceFinder("CardinalityMinGreaterThanMax") );
    }

  /// <summary>Invalid cast required by `${matcher}'.  This conversion may have been implicit; make sure that the type is supported by the assertion you are trying to use.</summary>
    internal static string CastRequiredByMatcherFailure(
    object @matcher
    ) {
        return string.Format(Culture, ResourceFinder("CastRequiredByMatcherFailure") , @matcher);
    }

  /// <summary>Elapsed before ${duration}.  Consistently expected to:</summary>
    internal static string ConsistentlyElapsedBefore(
    object @duration
    ) {
        return string.Format(Culture, ResourceFinder("ConsistentlyElapsedBefore") , @duration);
    }

  /// <summary>Couldn't load the required type: ${type}</summary>
    internal static string CouldNotLoadType(
    object @type
    ) {
        return string.Format(Culture, ResourceFinder("CouldNotLoadType") , @type);
    }

  /// <summary>Expected data property to have a get method and to be of type IEnumerable<object[]> or another compatible IEnumerable<> return type.</summary>
    internal static string DataPropertyIncorrectGetter(

    ) {
        return string.Format(Culture, ResourceFinder("DataPropertyIncorrectGetter") );
    }

  /// <summary>Timed out after ${duration}.  Eventually expected to:</summary>
    internal static string EventuallyTimedOutAfter(
    object @duration
    ) {
        return string.Format(Culture, ResourceFinder("EventuallyTimedOutAfter") , @duration);
    }

  /// <summary>Don't use Exactly(0).Item() or No.Item() -- use Items</summary>
    internal static string ExactlyOnePlural(

    ) {
        return string.Format(Culture, ResourceFinder("ExactlyOnePlural") );
    }

  /// <summary>Don't use Exactly(1).Items() or Single.Items() -- use Item</summary>
    internal static string ExactlyOneSingular(

    ) {
        return string.Format(Culture, ResourceFinder("ExactlyOneSingular") );
    }

  /// <summary>Expected all elements to:</summary>
    internal static string ExpectedAllElementsTo(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedAllElementsTo") );
    }

  /// <summary>Expected any element to:</summary>
    internal static string ExpectedAnyElementTo(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedAnyElementTo") );
    }

  /// <summary>Expected assignable from {Expected}</summary>
    internal static string ExpectedAssignableFrom(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedAssignableFrom") );
    }

  /// <summary>Expected at least ${number} to:</summary>
    internal static string ExpectedAtLeastTo(
    object @number
    ) {
        return string.Format(Culture, ResourceFinder("ExpectedAtLeastTo") , @number);
    }

  /// <summary>Expected at most ${number} to:</summary>
    internal static string ExpectedAtMostTo(
    object @number
    ) {
        return string.Format(Culture, ResourceFinder("ExpectedAtMostTo") , @number);
    }

  /// <summary>Expected to be between</summary>
    internal static string ExpectedBetween(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedBetween") );
    }

  /// <summary>Expected between ${low} and ${high} to:</summary>
    internal static string ExpectedBetweenTo(
    object @low, object @high
    ) {
        return string.Format(Culture, ResourceFinder("ExpectedBetweenTo") , @low, @high);
    }

  /// <summary>Expected to contain {Expected}</summary>
    internal static string ExpectedContains(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedContains") );
    }

  /// <summary>Expected to contain substring "{Expected}"</summary>
    internal static string ExpectedContainsSubstring(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedContainsSubstring") );
    }

  /// <summary>Expected to be distinct</summary>
    internal static string ExpectedDistinct(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedDistinct") );
    }

  /// <summary>Expected empty</summary>
    internal static string ExpectedEmpty(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedEmpty") );
    }

  /// <summary>Expected to end with {Expected}</summary>
    internal static string ExpectedEndWith(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedEndWith") );
    }

  /// <summary>Expected to end with substring {Expected}</summary>
    internal static string ExpectedEndWithSubstring(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedEndWithSubstring") );
    }

  /// <summary>Expected equal</summary>
    internal static string ExpectedEqual(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedEqual") );
    }

  /// <summary>Expected to equal download contents {Source}</summary>
    internal static string ExpectedEqualDownloadContents(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedEqualDownloadContents") );
    }

  /// <summary>Expected to equal file contents {FileName}</summary>
    internal static string ExpectedEqualFileContents(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedEqualFileContents") );
    }

  /// <summary>Expected exactly ${number} to:</summary>
    internal static string ExpectedExactly(
    object @number
    ) {
        return string.Format(Culture, ResourceFinder("ExpectedExactly") , @number);
    }

  /// <summary>Expected false</summary>
    internal static string ExpectedFalse(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedFalse") );
    }

  /// <summary>Expected > {Expected}</summary>
    internal static string ExpectedGreaterThan(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedGreaterThan") );
    }

  /// <summary>Expected >= {Expected}</summary>
    internal static string ExpectedGreaterThanOrEqualTo(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedGreaterThanOrEqualTo") );
    }

  /// <summary>Expected to have count {Expected}</summary>
    internal static string ExpectedHaveCount(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedHaveCount") );
    }

  /// <summary>Expected key</summary>
    internal static string ExpectedHaveKey(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedHaveKey") );
    }

  /// <summary>Expected key with value</summary>
    internal static string ExpectedHaveKeyWithValue(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedHaveKeyWithValue") );
    }

  /// <summary>Expected to have length {Expected}</summary>
    internal static string ExpectedHaveLength(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedHaveLength") );
    }

  /// <summary>Expected single item</summary>
    internal static string ExpectedHaveSingle(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedHaveSingle") );
    }

  /// <summary>Expected instance of {Expected}</summary>
    internal static string ExpectedInstanceOf(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedInstanceOf") );
    }

  /// <summary>Expected instance of type</summary>
    internal static string ExpectedInstanceOfType(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedInstanceOfType") );
    }

  /// <summary>Expected < {Expected}</summary>
    internal static string ExpectedLessThan(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedLessThan") );
    }

  /// <summary>Expected <= {Expected}</summary>
    internal static string ExpectedLessThanOrEqualTo(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedLessThanOrEqualTo") );
    }

  /// <summary>Expected to match {Expected}</summary>
    internal static string ExpectedMatch(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedMatch") );
    }

  /// <summary>Expected not all elements to:</summary>
    internal static string ExpectedNotAllElementsTo(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedNotAllElementsTo") );
    }

  /// <summary>Expected not any element to:</summary>
    internal static string ExpectedNotAnyElementTo(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedNotAnyElementTo") );
    }

  /// <summary>Expected null</summary>
    internal static string ExpectedNull(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedNull") );
    }

  /// <summary>Expected overlap</summary>
    internal static string ExpectedOverlap(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedOverlap") );
    }

  /// <summary>Expected same</summary>
    internal static string ExpectedSame(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedSame") );
    }

  /// <summary>Expected to satisfy all:</summary>
    internal static string ExpectedSatisfyAll(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedSatisfyAll") );
    }

  /// <summary>Expected to satisfy any:</summary>
    internal static string ExpectedSatisfyAny(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedSatisfyAny") );
    }

  /// <summary>Expected set-equal</summary>
    internal static string ExpectedSetEqual(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedSetEqual") );
    }

  /// <summary>Expected to start with {Expected}</summary>
    internal static string ExpectedStartWith(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedStartWith") );
    }

  /// <summary>Expected to start with substring {Expected}</summary>
    internal static string ExpectedStartWithSubstring(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedStartWithSubstring") );
    }

  /// <summary>Expected to throw "{Expected}"</summary>
    internal static string ExpectedThrows(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedThrows") );
    }

  /// <summary>Expected to:</summary>
    internal static string ExpectedTo(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedTo") );
    }

  /// <summary>Expected true</summary>
    internal static string ExpectedTrue(

    ) {
        return string.Format(Culture, ResourceFinder("ExpectedTrue") );
    }

  /// <summary>Test is configured to require explicitly passing, but test completed without invoking Assert.Pass().</summary>
    internal static string ExplicitPassNotSet(

    ) {
        return string.Format(Culture, ResourceFinder("ExplicitPassNotSet") );
    }

  /// <summary>Fact method ${name} cannot have parameters</summary>
    internal static string FactMethodParamCount(
    object @name
    ) {
        return string.Format(Culture, ResourceFinder("FactMethodParamCount") , @name);
    }

  /// <summary>Explicitly failed</summary>
    internal static string Failed(

    ) {
        return string.Format(Culture, ResourceFinder("Failed") );
    }

  /// <summary>Not an assembly: ${path}</summary>
    internal static string FailedToLoadAssembly(
    object @path
    ) {
        return string.Format(Culture, ResourceFinder("FailedToLoadAssembly") , @path);
    }

  /// <summary>Assembly file could not load: ${path} (${error})</summary>
    internal static string FailedToLoadAssemblyGeneralIO(
    object @path, object @error
    ) {
        return string.Format(Culture, ResourceFinder("FailedToLoadAssemblyGeneralIO") , @path, @error);
    }

  /// <summary>Assembly file not found: ${path}</summary>
    internal static string FailedToLoadAssemblyPath(
    object @path
    ) {
        return string.Format(Culture, ResourceFinder("FailedToLoadAssemblyPath") , @path);
    }

  /// <summary>Illegal tabs</summary>
    internal static string FixtureParserIllegalTabs(

    ) {
        return string.Format(Culture, ResourceFinder("FixtureParserIllegalTabs") );
    }

  /// <summary>Can't assert length on type `${type}'.  It works only for strings and arrays.  Investigate asserting on count for collections instead.</summary>
    internal static string HaveLengthWorksWith(
    object @type
    ) {
        return string.Format(Culture, ResourceFinder("HaveLengthWorksWith") , @type);
    }

  /// <summary>Actual</summary>
    internal static string LabelActual(

    ) {
        return string.Format(Culture, ResourceFinder("LabelActual") );
    }

  /// <summary>Bounds Exclusive</summary>
    internal static string LabelBoundsExclusive(

    ) {
        return string.Format(Culture, ResourceFinder("LabelBoundsExclusive") );
    }

  /// <summary>Comparer</summary>
    internal static string LabelComparer(

    ) {
        return string.Format(Culture, ResourceFinder("LabelComparer") );
    }

  /// <summary>Comparison</summary>
    internal static string LabelComparison(

    ) {
        return string.Format(Culture, ResourceFinder("LabelComparison") );
    }

  /// <summary>Diff</summary>
    internal static string LabelDiff(

    ) {
        return string.Format(Culture, ResourceFinder("LabelDiff") );
    }

  /// <summary>Expected</summary>
    internal static string LabelExpected(

    ) {
        return string.Format(Culture, ResourceFinder("LabelExpected") );
    }

  /// <summary>File name</summary>
    internal static string LabelFileName(

    ) {
        return string.Format(Culture, ResourceFinder("LabelFileName") );
    }

  /// <summary>Given</summary>
    internal static string LabelGiven(

    ) {
        return string.Format(Culture, ResourceFinder("LabelGiven") );
    }

  /// <summary>High</summary>
    internal static string LabelHigh(

    ) {
        return string.Format(Culture, ResourceFinder("LabelHigh") );
    }

  /// <summary>Low</summary>
    internal static string LabelLow(

    ) {
        return string.Format(Culture, ResourceFinder("LabelLow") );
    }

  /// <summary>Matchers</summary>
    internal static string LabelMatchers(

    ) {
        return string.Format(Culture, ResourceFinder("LabelMatchers") );
    }

  /// <summary>Predicate</summary>
    internal static string LabelPredicate(

    ) {
        return string.Format(Culture, ResourceFinder("LabelPredicate") );
    }

  /// <summary>Source</summary>
    internal static string LabelSource(

    ) {
        return string.Format(Culture, ResourceFinder("LabelSource") );
    }

  /// <summary>Subject</summary>
    internal static string LabelSubject(

    ) {
        return string.Format(Culture, ResourceFinder("LabelSubject") );
    }

  /// <summary>Invalid cross join: Theory method must have exactly as many parameters as members in the data source.</summary>
    internal static string MultiAccessorsTheoryParameterMismatch(

    ) {
        return string.Format(Culture, ResourceFinder("MultiAccessorsTheoryParameterMismatch") );
    }

  /// <summary>Make sure that AtLeast or min value is non-negative</summary>
    internal static string NegativeCardinality(

    ) {
        return string.Format(Culture, ResourceFinder("NegativeCardinality") );
    }

  /// <summary>Timeout must be positive or exactly equal to 0 or -1 to indicate an infinite timeout.</summary>
    internal static string NegativeTimeout(

    ) {
        return string.Format(Culture, ResourceFinder("NegativeTimeout") );
    }

  /// <summary>(No exception)</summary>
    internal static string NoException(

    ) {
        return string.Format(Culture, ResourceFinder("NoException") );
    }

  /// <summary>Test class has no test methods</summary>
    internal static string NoTestMethods(

    ) {
        return string.Format(Culture, ResourceFinder("NoTestMethods") );
    }

  /// <summary>Test class has no test subjects</summary>
    internal static string NoTestSubjects(

    ) {
        return string.Format(Culture, ResourceFinder("NoTestSubjects") );
    }

  /// <summary>Not expected to be assignable from {Expected}</summary>
    internal static string NotExpectedAssignableFrom(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedAssignableFrom") );
    }

  /// <summary>Not expected to be between</summary>
    internal static string NotExpectedBetween(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedBetween") );
    }

  /// <summary>Not expected to contain {Expected}</summary>
    internal static string NotExpectedContains(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedContains") );
    }

  /// <summary>Not expected to contain substring "{Expected}"</summary>
    internal static string NotExpectedContainsSubstring(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedContainsSubstring") );
    }

  /// <summary>Not expected to be distinct</summary>
    internal static string NotExpectedDistinct(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedDistinct") );
    }

  /// <summary>Not expected to be empty</summary>
    internal static string NotExpectedEmpty(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedEmpty") );
    }

  /// <summary>Not expected to end with {Expected}</summary>
    internal static string NotExpectedEndWith(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedEndWith") );
    }

  /// <summary>Not expected to end with substring {Expected}</summary>
    internal static string NotExpectedEndWithSubstring(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedEndWithSubstring") );
    }

  /// <summary>Not expected to be equal to</summary>
    internal static string NotExpectedEqual(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedEqual") );
    }

  /// <summary>Not expected to equal download contents {Source}</summary>
    internal static string NotExpectedEqualDownloadContents(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedEqualDownloadContents") );
    }

  /// <summary>Not expected to equal file contents {FileName}</summary>
    internal static string NotExpectedEqualFileContents(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedEqualFileContents") );
    }

  /// <summary>Not expected to be false</summary>
    internal static string NotExpectedFalse(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedFalse") );
    }

  /// <summary>Not expected to be > {Expected}</summary>
    internal static string NotExpectedGreaterThan(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedGreaterThan") );
    }

  /// <summary>Not expected to be >= {Expected}</summary>
    internal static string NotExpectedGreaterThanOrEqualTo(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedGreaterThanOrEqualTo") );
    }

  /// <summary>Not expected to have count {Expected}</summary>
    internal static string NotExpectedHaveCount(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedHaveCount") );
    }

  /// <summary>Expected not to have key {Expected}</summary>
    internal static string NotExpectedHaveKey(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedHaveKey") );
    }

  /// <summary>Expected not to have key with value</summary>
    internal static string NotExpectedHaveKeyWithValue(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedHaveKeyWithValue") );
    }

  /// <summary>Not expected to have length {Expected}</summary>
    internal static string NotExpectedHaveLength(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedHaveLength") );
    }

  /// <summary>Not expected to have single item</summary>
    internal static string NotExpectedHaveSingle(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedHaveSingle") );
    }

  /// <summary>Expected not instance of {Expected}</summary>
    internal static string NotExpectedInstanceOf(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedInstanceOf") );
    }

  /// <summary>Not expected to be < {Expected}</summary>
    internal static string NotExpectedLessThan(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedLessThan") );
    }

  /// <summary>Not expected to be <= {Expected}</summary>
    internal static string NotExpectedLessThanOrEqualTo(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedLessThanOrEqualTo") );
    }

  /// <summary>Not expected to match {Expected}</summary>
    internal static string NotExpectedMatch(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedMatch") );
    }

  /// <summary>Not expected to be null</summary>
    internal static string NotExpectedNull(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedNull") );
    }

  /// <summary>Not expected to overlap</summary>
    internal static string NotExpectedOverlap(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedOverlap") );
    }

  /// <summary>Not expected to be same</summary>
    internal static string NotExpectedSame(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedSame") );
    }

  /// <summary>Not expected to satisfy all</summary>
    internal static string NotExpectedSatisfyAll(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedSatisfyAll") );
    }

  /// <summary>Not expected to satisfy any</summary>
    internal static string NotExpectedSatisfyAny(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedSatisfyAny") );
    }

  /// <summary>Not expected to be set-equal</summary>
    internal static string NotExpectedSetEqual(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedSetEqual") );
    }

  /// <summary>Not expected to start with {Expected}</summary>
    internal static string NotExpectedStartWith(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedStartWith") );
    }

  /// <summary>Not expected to start with substring {Expected}</summary>
    internal static string NotExpectedStartWithSubstring(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedStartWithSubstring") );
    }

  /// <summary>Not expected to throw "{Expected}"</summary>
    internal static string NotExpectedThrows(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedThrows") );
    }

  /// <summary>Not expected to:</summary>
    internal static string NotExpectedTo(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedTo") );
    }

  /// <summary>Not expected to be true</summary>
    internal static string NotExpectedTrue(

    ) {
        return string.Format(Culture, ResourceFinder("NotExpectedTrue") );
    }

  /// <summary>not be assignable from {Expected}</summary>
    internal static string NotPredicateAssignableFrom(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateAssignableFrom") );
    }

  /// <summary>not to be between {Low} {High} {BoundsExclusive:B:(exclusive)}</summary>
    internal static string NotPredicateBetween(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateBetween") );
    }

  /// <summary>not to contain {Expected}</summary>
    internal static string NotPredicateContains(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateContains") );
    }

  /// <summary>not to contain substring "{Expected}"</summary>
    internal static string NotPredicateContainsSubstring(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateContainsSubstring") );
    }

  /// <summary>not be distinct</summary>
    internal static string NotPredicateDistinct(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateDistinct") );
    }

  /// <summary>not be empty</summary>
    internal static string NotPredicateEmpty(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateEmpty") );
    }

  /// <summary>not end with {Expected}</summary>
    internal static string NotPredicateEndWith(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateEndWith") );
    }

  /// <summary>not end with substring {Expected}</summary>
    internal static string NotPredicateEndWithSubstring(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateEndWithSubstring") );
    }

  /// <summary>not be equal to {Expected}</summary>
    internal static string NotPredicateEqual(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateEqual") );
    }

  /// <summary>not to equal download contents {Source}</summary>
    internal static string NotPredicateEqualDownloadContents(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateEqualDownloadContents") );
    }

  /// <summary>not to equal file contents {FileName}</summary>
    internal static string NotPredicateEqualFileContents(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateEqualFileContents") );
    }

  /// <summary>not be false</summary>
    internal static string NotPredicateFalse(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateFalse") );
    }

  /// <summary>not be > {Expected}</summary>
    internal static string NotPredicateGreaterThan(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateGreaterThan") );
    }

  /// <summary>not be >=not  {Expected}</summary>
    internal static string NotPredicateGreaterThanOrEqualTo(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateGreaterThanOrEqualTo") );
    }

  /// <summary>not have count {Expected}</summary>
    internal static string NotPredicateHaveCount(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateHaveCount") );
    }

  /// <summary>not have key {Expected}</summary>
    internal static string NotPredicateHaveKey(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateHaveKey") );
    }

  /// <summary>not have key with value {ExpectedKey}={ExpectedValue}</summary>
    internal static string NotPredicateHaveKeyWithValue(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateHaveKeyWithValue") );
    }

  /// <summary>not have length {Expected}</summary>
    internal static string NotPredicateHaveLength(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateHaveLength") );
    }

  /// <summary>not have single item</summary>
    internal static string NotPredicateHaveSingle(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateHaveSingle") );
    }

  /// <summary>not be instance of {Expected}</summary>
    internal static string NotPredicateInstanceOf(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateInstanceOf") );
    }

  /// <summary>not be < {Expected}</summary>
    internal static string NotPredicateLessThan(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateLessThan") );
    }

  /// <summary>not be <=not  {Expected}</summary>
    internal static string NotPredicateLessThanOrEqualTo(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateLessThanOrEqualTo") );
    }

  /// <summary>not match {Expected}</summary>
    internal static string NotPredicateMatch(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateMatch") );
    }

  /// <summary>not be null</summary>
    internal static string NotPredicateNull(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateNull") );
    }

  /// <summary>not overlap</summary>
    internal static string NotPredicateOverlap(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateOverlap") );
    }

  /// <summary>not be same</summary>
    internal static string NotPredicateSame(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateSame") );
    }

  /// <summary>not satisfy all</summary>
    internal static string NotPredicateSatisfyAll(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateSatisfyAll") );
    }

  /// <summary>not satisfy any</summary>
    internal static string NotPredicateSatisfyAny(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateSatisfyAny") );
    }

  /// <summary>not be set-equal</summary>
    internal static string NotPredicateSetEqual(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateSetEqual") );
    }

  /// <summary>not start with {Expected}</summary>
    internal static string NotPredicateStartWith(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateStartWith") );
    }

  /// <summary>not start with substring {Expected}</summary>
    internal static string NotPredicateStartWithSubstring(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateStartWithSubstring") );
    }

  /// <summary>not to throw "{Expected}"</summary>
    internal static string NotPredicateThrows(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateThrows") );
    }

  /// <summary>not be true</summary>
    internal static string NotPredicateTrue(

    ) {
        return string.Format(Culture, ResourceFinder("NotPredicateTrue") );
    }

  /// <summary>${message}, line ${line}</summary>
    internal static string ParserErrorLinePosition(
    object @message, object @line
    ) {
        return string.Format(Culture, ResourceFinder("ParserErrorLinePosition") , @message, @line);
    }

  /// <summary>Explicitly passed</summary>
    internal static string Passed(

    ) {
        return string.Format(Culture, ResourceFinder("Passed") );
    }

  /// <summary>Pending</summary>
    internal static string Pending(

    ) {
        return string.Format(Culture, ResourceFinder("Pending") );
    }

  /// <summary>be assignable from {Expected}</summary>
    internal static string PredicateAssignableFrom(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateAssignableFrom") );
    }

  /// <summary>to be between {Low} {High}</summary>
    internal static string PredicateBetween(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateBetween") );
    }

  /// <summary>to contain {Expected}</summary>
    internal static string PredicateContains(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateContains") );
    }

  /// <summary>to contain substring "{Expected}"</summary>
    internal static string PredicateContainsSubstring(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateContainsSubstring") );
    }

  /// <summary>be distinct</summary>
    internal static string PredicateDistinct(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateDistinct") );
    }

  /// <summary>be empty</summary>
    internal static string PredicateEmpty(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateEmpty") );
    }

  /// <summary>end with {Expected}</summary>
    internal static string PredicateEndWith(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateEndWith") );
    }

  /// <summary>end with substring {Expected}</summary>
    internal static string PredicateEndWithSubstring(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateEndWithSubstring") );
    }

  /// <summary>be equal to {Expected}</summary>
    internal static string PredicateEqual(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateEqual") );
    }

  /// <summary>to equal download contents {Source}</summary>
    internal static string PredicateEqualDownloadContents(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateEqualDownloadContents") );
    }

  /// <summary>to equal file contents {FileName}</summary>
    internal static string PredicateEqualFileContents(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateEqualFileContents") );
    }

  /// <summary>be false</summary>
    internal static string PredicateFalse(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateFalse") );
    }

  /// <summary>be > {Expected}</summary>
    internal static string PredicateGreaterThan(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateGreaterThan") );
    }

  /// <summary>be >= {Expected}</summary>
    internal static string PredicateGreaterThanOrEqualTo(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateGreaterThanOrEqualTo") );
    }

  /// <summary>have count {Expected}</summary>
    internal static string PredicateHaveCount(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateHaveCount") );
    }

  /// <summary>have key {ExpectedKey}</summary>
    internal static string PredicateHaveKey(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateHaveKey") );
    }

  /// <summary>have key with value {ExpectedKey}={ExpectedValue}</summary>
    internal static string PredicateHaveKeyWithValue(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateHaveKeyWithValue") );
    }

  /// <summary>have length {Expected}</summary>
    internal static string PredicateHaveLength(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateHaveLength") );
    }

  /// <summary>have single item</summary>
    internal static string PredicateHaveSingle(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateHaveSingle") );
    }

  /// <summary>be instance of {Expected}</summary>
    internal static string PredicateInstanceOf(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateInstanceOf") );
    }

  /// <summary>be < {Expected}</summary>
    internal static string PredicateLessThan(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateLessThan") );
    }

  /// <summary>be <= {Expected}</summary>
    internal static string PredicateLessThanOrEqualTo(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateLessThanOrEqualTo") );
    }

  /// <summary>match {Expected}</summary>
    internal static string PredicateMatch(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateMatch") );
    }

  /// <summary>be null</summary>
    internal static string PredicateNull(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateNull") );
    }

  /// <summary>overlap</summary>
    internal static string PredicateOverlap(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateOverlap") );
    }

  /// <summary>be same</summary>
    internal static string PredicateSame(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateSame") );
    }

  /// <summary>satisfy all</summary>
    internal static string PredicateSatisfyAll(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateSatisfyAll") );
    }

  /// <summary>satisfy any</summary>
    internal static string PredicateSatisfyAny(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateSatisfyAny") );
    }

  /// <summary>be set-equal</summary>
    internal static string PredicateSetEqual(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateSetEqual") );
    }

  /// <summary>start with {Expected}</summary>
    internal static string PredicateStartWith(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateStartWith") );
    }

  /// <summary>start with substring {Expected}</summary>
    internal static string PredicateStartWithSubstring(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateStartWithSubstring") );
    }

  /// <summary>to throw "{Expected}"</summary>
    internal static string PredicateThrows(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateThrows") );
    }

  /// <summary>be true</summary>
    internal static string PredicateTrue(

    ) {
        return string.Format(Culture, ResourceFinder("PredicateTrue") );
    }

  /// <summary>Unexpectedly tried to assert on a sequence that was a null reference.</summary>
    internal static string SequenceNullConversion(

    ) {
        return string.Format(Culture, ResourceFinder("SequenceNullConversion") );
    }

  /// <summary>Path must be an absolute path or navigate beyond the directory.</summary>
    internal static string TemporaryDirectoryFileNameRooted(

    ) {
        return string.Format(Culture, ResourceFinder("TemporaryDirectoryFileNameRooted") );
    }

  /// <summary>TestFileDataAttribute can only be used on theories with one parameter.</summary>
    internal static string TestFileDataRequiresOneParameter(

    ) {
        return string.Format(Culture, ResourceFinder("TestFileDataRequiresOneParameter") );
    }

  /// <summary>Test timed out.  Allowed execution time ${time} was execeeded</summary>
    internal static string TestTimedOut(
    object @time
    ) {
        return string.Format(Culture, ResourceFinder("TestTimedOut") , @time);
    }

  /// <summary>No test data for theory</summary>
    internal static string TheoryHasNoDataProviders(

    ) {
        return string.Format(Culture, ResourceFinder("TheoryHasNoDataProviders") );
    }

  /// <summary>Unable to compare values using the specified comparer</summary>
    internal static string UnusableComparer(

    ) {
        return string.Format(Culture, ResourceFinder("UnusableComparer") );
    }

  /// <summary>Test case #${num} for theory method ${clazz}.${method} has incorrect number of arguments.</summary>
    internal static string WrongNumberOfTheoryArguments(
    object @num, object @clazz, object @method
    ) {
        return string.Format(Culture, ResourceFinder("WrongNumberOfTheoryArguments") , @num, @clazz, @method);
    }

    }
}
