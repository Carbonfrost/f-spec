fspec (1) -- a test framework for .NET Core
===========================================

## SYNOPSIS

* :
      **fspec** [options] [<assembly>]...

## DESCRIPTION

The fspec(1) command runs tests that were built with **f-spec**, a test framework for .NET Core.

Each <assembly> to load is specified as an argument to the command.  Assemblies are scanned for tests, and dependency assemblies are automatically loaded if they are in the loader path.  The loader path is discussed in the [LOADER PATH][] section.

## OPTIONS

* `--context-lines`=<count>:
  When string comparisons generate differences, this parameter is used to control how many lines of context are displayed before and after a contiguous hunk.

* `--exclude`=<string>:
  Skip tests whose full name excludes the specified <string>.  See [SELECTING TESTS][] for information about the syntax to use for <string>.

* `--exclude-pattern`=<regex>:
  Skip tests whose full name excludes the specified <regex> pattern.  This is the same as specifying `--exclude=regex:`<regex>

* `--fail-fast`:
  Instead of running the entire test plan, run until the first test that fails.

* `--fail-focused`:
  When used, specifies that the test plan fails if there are any tests that are focused.

* `--fail-pending`:
  When used, specifies that the test plan fails if there are any pending tests.

* `-i, --fixture`=<path>:
  Add a path to the fixture search path.  If the path is a file, then the file can be loaded as a fixture.  If the path is a directory, then the directory is used as the prefix when searching for a fixture.  This option can be specified multiple times.

* `-F, --focus`=<string>:
  Focus tests whose descriptions or names match the given <string>.  See [SELECTING TESTS][] for information about the syntax to use for <string>.

* `--full-stack-traces`:
  When specified, show full stack traces when errors occur.  By default, stack traces are filtered so that stack frames that are part of `fspec` itself are left out.  This hides the non-user stack frames and makes for more concise output.  This option is implied by `--self-test` because any errors that occur are likely to have occurred within these stack frames.  When set, the environment variable `DEBUG` also enables this option.

* `-e, --include`=<string>:
  Run tests whose full name includes the specified <string>.  See [SELECTING TESTS][] for information about the syntax to use for <string>.

* `-E, --include-pattern`=<regex>:
  Run tests whose full name includes the specified <regex> pattern.  This is the same as specifying `--include=regex:`<regex>

* `--loader-path`=<path>:
  Add a path to the loader search path.  When the path is a file, this is the same as loading the specified assembly.  If the path is a directory, then the directory is used to search for assemblies when an assembly reference must be loaded.  This option can be specified multiple times.

* `--no-config`:
  When specified, configuration files are not loaded, which causes fspec(1) to use defaults.  This also prevents options like `--previous-failures` from working because they implicitly depend upon results that were cached in `.fspec` directory.  See [THE .FSPEC DIRECTORY][] for information.

* `--no-diff`:
  Don't use unified diffs when assertion messages contain long strings

* `--no-focus`:
  Ignore all focused elements and run everything

* `--no-random`:
  Don't randomize specs.  Typically, tests are randomized within their containing theory, class, namespace, or assembly.  Using this option causes tests to be run in a stable order.

* `--no-summary`:
  Don't show test run summary of failed tests at the end

*  `--no-whitespace`:
   When comparing strings, special characters will not be used when printing whitespace in assertion failure messages

* `-p, --package`=<formula>:
  Load the NuGet package dependency <formula>.  The <formula> is in the format <name>[/<version>].  Though technically optional, fspec(1) raises an error if the version is not specified.  This may change in a future version.

* `--pause`:
  Attach tty and wait for a keypress before exiting.  This is primarily used when a debugger is attached to prevent fspec(1) from exiting too soon.

* `--plan-timeout`=<time>:
  Provides the limit on the amount of <time> that the entire test plan is allowed to take.  When reached, the test plan fails.  The value for <time> is either in seconds or using the format described in the description for  `--timeout`

* `--previous-failures`:
  When specified, runs all tests that failed in the previous test run.  If there is no previous test run or if all tests in the previous test run passed, then all tests are run.

  A common idiom is to re-run previous failures in order to focus on tests which are broken and repeat this process until the tests are fixed.  For example, the following command could be used to successively run the test suite until everything passes consistently:

  fspec --previous-failures && fspec

* `-C|--project-dir`=<directory>:
  Change directory into the specified directory when starting up.  This has the effect of specifying how paths are resolved for the [LOADER PATH][], include path, and configuration directories.

* `--random-seed`=<seed> :
  Use the specified <seed> to randomize the ordering in which specs are executed

* `--self-test`:
  If `fspec` was built with its own tests, then run the self-tests.

* `--show-pass-explicit`:
  Display messages when Assert.Pass is used

* `--show-tests`:
  Display names of tests and test cases even on success

* `--show-whitespace`:
  When comparing strings, special characters will be used when printing whitespace in assertion failure messages.

* `--slow-test`=<time>:
  Specify the amount of time before a test is considered a slow test.  The value for <time> is either in seconds or using the format described in the description for  `--timeout`.  The default is 500ms.

* `-t, --tag`=<tag>:
  Run tests that have the specified <tag>.  Tags are either simple names or key-value pairs.  For example, `acceptance` is a valid tag, and so is `platform:nixos`.  You can _exclude_ tests that match the tag by using the prefix `~`.  For example, to exclude slow tests, you would specify `~slow`.  This flag can be specified multiple times.

* `--timeout`=<time>:
  The maximum <time> allowed for any particular test to execute.  The syntax of TIME is either a floating point number representing the whole and partial seconds to allow or it is the syntax of `System.TimeSpan` which looks like <days>.<hours>:<minutes>:<seconds>.<ticks>

* `--verify`={**strict**|**none**}:
  Use the specified verification mode to check for errors in tests and assertions.  If `strict` is used, then tests could fail if tests or assertions are structured incorrectly.  If `none` is used, then no verification is used, but a warning is printed when certain verification errors occur.

* `--help`:
  Display the help screen

* `--version`:
  Report the version information and exit

## SELECTING TESTS

By default, all tests will be run in the test suite except for tests that are marked as "explicit" or have user-defined tags.  Various options let you specify which tests are included in the test plan.  Each option has a string argument:

1.  The string must be contained in the full name or description of the test (case insensitive).
2.  _But_ if a wildcard pattern character '*', '?', '[', or ']', is present, then the string must match the wildcard expression.  Therefore, if you specify `test`, the pattern will match strings that _contain "test"_, but if you specify `test*` the pattern will match strings that _start with "test"_.
3.  When you prefix the string with `regex:`, then the string is interpretted as a regular expression and is case sensitive.

If you specify none of the test selection options, then the default set of tests are run, which is all tests _except_ those marked with "explicit" or that have user-defined tags.
Otherwise, the tests that will be run will be the tests that match the `--include` or `--tag` options but do not match the `--exclude` option.

When present, fspec(1) loads the contents of the `.fspec` directory to determine information about the previous test run.  Special tags are automatically assigned to tests representing the outcome of the previously run test:

* `previously:failed`
* `previously:passed`
* `previously:pending`
* `previously:skipped`
* `previously:slow`

### EXAMPLES

* **Run tests whose names contain a string "watermelon"**:
    fspec -e watermelon Assembly.dll

* **Run tests whose names start with "lemon"**:
    fspec -e 'lemon*' Assembly.dll

* **Run tests which were slow in the last test run**:
    fspec -t previously:slow Assembly.dll


## THE .FSPEC DIRECTORY

A special directory named `.fspec` is created in the project directory when fspec(1) runs.  This directory will store information about the test run including the results.  This can be used to obtain information about the previous test run.  The following sections describe the files stored there and what you can do with them.

### results.json

Contains the results of the previous test run.

Using jq(1), several common queries with `results.json` are available.

* **Get the result of the test run**:
  jq -r .failureReason .fspec/results.json

* **Get display names of failed tests**:
  jq -r '.results[] | select(.status=="FAILED") | .displayName' .fspec/results.json

* **Get tests sorted on their execution durations**:
  jq -r '.results | sort_by(.executionTime) | map(.displayName + "\t" +   .executionTime) | .[]' .fspec/results.json

## LOADER PATH

The loader path specifies the directories which are probed to find additional assemblies to load.  By default, the loader path contains each directory for each assembly that was specified at the command line.

You can specify the environment variable `FSPEC_LOADER_PATH` as described in [ENVIRONMENT][] to set up the loader path from the environment.

If you need to add another loader path, you specify it with the `--loader-path` option.  This can be used to load an assembly directly or can be used to add a search directory from which assemblies can be loaded.  The loader path specified from the command line is searched before those set by an environment variable, and the implicit search of the containing directory of an assembly reference is performed last.

## ENVIRONMENT

* `DEBUG`:
   When set, provide debug trace output.

* `FSPEC_FIXTURE_PATH`:
  Specifies the fixture path where fixtures can be loaded.  This environment variable uses the format that `PATH` does; that is, it is a colon-delimited list of paths on Unix-like platforms or a semicolon-delimited list on Windows.  The other way to set fixture paths is with the `--fixture-path` option, and when it is specified, fixture paths are first loaded from the command line arguments.

* `FSPEC_LOADER_PATH`:
  Specifies the loader path, which contains assembly file names or search directories.  This environment variable uses the format that `PATH` does; that is, it is a colon-delimited list of paths on Unix-like platforms or a semicolon-delimited list on Windows.  See [LOADER PATH][] for an overview of how the loader path works.  The other way to set loader paths is with the `--loader-path` option, and when it is specified, loader paths are first loaded from the command line arguments.

## EXIT CODES

* 0:
  Success; all tests passed.

* 1:
  Failed; one or more tests failed.  If `--fail-pending` was used, then this is the result when there are pending tests.

* 2:
  There was a problem configuring or starting up `fspec`

## COPYRIGHT

Copyright © 2020 Carbonfrost Systems, Inc.  Licensed under the terms of the Apache 2.0 license (https://apache.org/licenses/LICENSE-2.0.txt)
