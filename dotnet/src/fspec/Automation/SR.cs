
// This file was automatically generated.  DO NOT EDIT or else
// your changes could be lost!

#pragma warning disable 1570

using System;
using System.Globalization;
using System.Resources;
using System.Reflection;

namespace Carbonfrost.CFSpec.Resources {

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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Carbonfrost.CFSpec.Resources.SR", typeof(SR).GetTypeInfo().Assembly);
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


  /// <summary>invalid number of context lines</summary>
    internal static string InvalidContextLines(

    ) {
        return string.Format(Culture, ResourceFinder("InvalidContextLines") );
    }

  /// <summary>invalid package reference syntax</summary>
    internal static string InvalidPackageReference(

    ) {
        return string.Format(Culture, ResourceFinder("InvalidPackageReference") );
    }

  /// <summary>invalid random seed</summary>
    internal static string InvalidRandomSeed(

    ) {
        return string.Format(Culture, ResourceFinder("InvalidRandomSeed") );
    }

  /// <summary>invalid regex: ${reason}</summary>
    internal static string InvalidRegex(
    object @reason
    ) {
        return string.Format(Culture, ResourceFinder("InvalidRegex") , @reason);
    }

  /// <summary>invalid time span</summary>
    internal static string InvalidTimeSpan(

    ) {
        return string.Format(Culture, ResourceFinder("InvalidTimeSpan") );
    }

  /// <summary>invalid verification mode</summary>
    internal static string InvalidVerify(

    ) {
        return string.Format(Culture, ResourceFinder("InvalidVerify") );
    }

  /// <summary>Show as many lines of context in unified diffs</summary>
    internal static string UContextLines(

    ) {
        return string.Format(Culture, ResourceFinder("UContextLines") );
    }

  /// <summary>Skip tests whose descriptions or names match the given {{PATTERN}}</summary>
    internal static string UExclude(

    ) {
        return string.Format(Culture, ResourceFinder("UExclude") );
    }

  /// <summary>Skip tests whose full names match {{REGEX}}</summary>
    internal static string UExcludePattern(

    ) {
        return string.Format(Culture, ResourceFinder("UExcludePattern") );
    }

  /// <summary>Exit when the first test fails</summary>
    internal static string UFailFast(

    ) {
        return string.Format(Culture, ResourceFinder("UFailFast") );
    }

  /// <summary>Exit with an error code if any pending specs</summary>
    internal static string UFailOnPending(

    ) {
        return string.Format(Culture, ResourceFinder("UFailOnPending") );
    }

  /// <summary>Include a fixture search {{PATH}}</summary>
    internal static string UFixture(

    ) {
        return string.Format(Culture, ResourceFinder("UFixture") );
    }

  /// <summary>Focus tests whose descriptions or names match the given {{PATTERN}}</summary>
    internal static string UFocus(

    ) {
        return string.Format(Culture, ResourceFinder("UFocus") );
    }

  /// <summary>Don't hide f-spec stack frames in exception stack traces</summary>
    internal static string UFullStackTraces(

    ) {
        return string.Format(Culture, ResourceFinder("UFullStackTraces") );
    }

  /// <summary>Display this help screen</summary>
    internal static string UHelp(

    ) {
        return string.Format(Culture, ResourceFinder("UHelp") );
    }

  /// <summary>Run tests whose full names match the given {{PATTERN}}</summary>
    internal static string UInclude(

    ) {
        return string.Format(Culture, ResourceFinder("UInclude") );
    }

  /// <summary>Run tests whose full names match {{REGEX}}</summary>
    internal static string UIncludePattern(

    ) {
        return string.Format(Culture, ResourceFinder("UIncludePattern") );
    }

  /// <summary>Loader options</summary>
    internal static string ULoaderOptions(

    ) {
        return string.Format(Culture, ResourceFinder("ULoaderOptions") );
    }

  /// <summary>Add {{PATH}} to loader path</summary>
    internal static string ULoaderPath(

    ) {
        return string.Format(Culture, ResourceFinder("ULoaderPath") );
    }

  /// <summary>Don't use unified diffs when assertion messages contain long strings</summary>
    internal static string UNoDiff(

    ) {
        return string.Format(Culture, ResourceFinder("UNoDiff") );
    }

  /// <summary>Ignore all focused elements and run everything</summary>
    internal static string UNoFocus(

    ) {
        return string.Format(Culture, ResourceFinder("UNoFocus") );
    }

  /// <summary>Don't randomize specs</summary>
    internal static string UNoRandomizeSpecs(

    ) {
        return string.Format(Culture, ResourceFinder("UNoRandomizeSpecs") );
    }

  /// <summary>Don't show test run summary of failed tests at the end</summary>
    internal static string UNoSummary(

    ) {
        return string.Format(Culture, ResourceFinder("UNoSummary") );
    }

  /// <summary>Don't use special characters when printing whitespace in assertion failure messages</summary>
    internal static string UNoWhitespace(

    ) {
        return string.Format(Culture, ResourceFinder("UNoWhitespace") );
    }

  /// <summary>Output options</summary>
    internal static string UOutputOptions(

    ) {
        return string.Format(Culture, ResourceFinder("UOutputOptions") );
    }

  /// <summary>Load the NuGet package dependency {{FORMULA}}</summary>
    internal static string UPackage(

    ) {
        return string.Format(Culture, ResourceFinder("UPackage") );
    }

  /// <summary>Attach tty and wait for a keypress before exiting</summary>
    internal static string UPause(

    ) {
        return string.Format(Culture, ResourceFinder("UPause") );
    }

  /// <summary>Specify the maximum {{TIME}} allowed for the test plan to execute</summary>
    internal static string UPlanTimeout(

    ) {
        return string.Format(Culture, ResourceFinder("UPlanTimeout") );
    }

  /// <summary>Use the specified {{SEED}} to randomize specs</summary>
    internal static string URandom(

    ) {
        return string.Format(Culture, ResourceFinder("URandom") );
    }

  /// <summary>Runner options</summary>
    internal static string URunnerOptions(

    ) {
        return string.Format(Culture, ResourceFinder("URunnerOptions") );
    }

  /// <summary>Run the self-tests and exit</summary>
    internal static string USelfTest(

    ) {
        return string.Format(Culture, ResourceFinder("USelfTest") );
    }

  /// <summary>Display messages when Assert.Pass is used</summary>
    internal static string UShowPassExplicit(

    ) {
        return string.Format(Culture, ResourceFinder("UShowPassExplicit") );
    }

  /// <summary>Display names of tests and test cases even on success</summary>
    internal static string UShowTestNames(

    ) {
        return string.Format(Culture, ResourceFinder("UShowTestNames") );
    }

  /// <summary>Use special characters when printing whitespace in assertion failure messages</summary>
    internal static string UShowWhitespace(

    ) {
        return string.Format(Culture, ResourceFinder("UShowWhitespace") );
    }

  /// <summary>Threshold {{TIME}} span for whether a test is considered slow</summary>
    internal static string USlowTest(

    ) {
        return string.Format(Culture, ResourceFinder("USlowTest") );
    }

  /// <summary>Run tests with the specified TAG.  (Optionally, ~TAG for tests without tag)</summary>
    internal static string UTag(

    ) {
        return string.Format(Culture, ResourceFinder("UTag") );
    }

  /// <summary>Test selection options</summary>
    internal static string UTestSelectionOptions(

    ) {
        return string.Format(Culture, ResourceFinder("UTestSelectionOptions") );
    }

  /// <summary>Specify the maximum {{TIME}} allowed for any particular test to execute</summary>
    internal static string UTimeout(

    ) {
        return string.Format(Culture, ResourceFinder("UTimeout") );
    }

  /// <summary>Use the specified verification {{MODE}} (either strict or none) to check for errors in tests and assertions</summary>
    internal static string UVerify(

    ) {
        return string.Format(Culture, ResourceFinder("UVerify") );
    }

  /// <summary>Report the version information and exit</summary>
    internal static string UVersion(

    ) {
        return string.Format(Culture, ResourceFinder("UVersion") );
    }

    }
}
