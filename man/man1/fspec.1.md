fspec (1) -- a test framework for .NET Core
===========================================

## SYNOPSIS

  fspec [OPTION...] ASSEMBLY...

## DESCRIPTION

The fspec(1) command runs tests that were built with **f-spec**, a test framework for .NET Core.

Each <ASSEMBLY> to load is specified as an argument to the command.  Assemblies are scanned for tests, and dependency assemblies are automatically loaded if they are in the loader path.  The loader path is discussed in the [LOADER PATH][] section.

Inline markup for `code`, `user input`, and **strong** are displayed
boldface; <variable>, _emphasis_, *emphasis*, are displayed in italics
(HTML) or underline (roff).

## OPTIONS

* `--context-lines`=<COUNT>:
When string comparisons generate differences, this parameter is used to control how many lines of context are displayed before and after a contiguous hunk.

* `--fail-fast`:
  Instead of running the entire test plan, run until the first test that fails.

* `--fail-focused`:
  When used, specifies that the test plan fails if there are any tests that are focused.

* `--fail-pending`:
  When used, specifies that the test plan fails if there are any pending tests.

* `--focus`=<REGEX>:
  Focus tests whose descriptions or names match the given REGEX

* `-i, --fixture`=<PATH>:
  Add a path to the fixture search path.  If the path is a file, then the file can be loaded as a fixture.  If the path is a directory, then the directory is used as the prefix when searching for a fixture.  This option can be specified multiple times.

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

* `-p, --package`=<FORMULA>:
  Load the NuGet package dependency <FORMULA>.  The <FORMULA> is in the format <NAME>[/<VERSION>].  Though technically optional, fspec(1) raises an error if the version is not specified.  This may change in a future version.

* `--pause`:
  Attach tty and wait for a keypress before exiting.  This is primarily used when a debugger is attached to prevent fspec(1) from exiting too soon.

* `--plan-timeout`=<TIME>:
  Provides the limit on the amount of <TIME> that the entire test plan is allowed to take.  When reached, the test plan fails.  The value for <TIME> is either in seconds or using the format described in the description for  `--timeout`

* `--random-seed`=<SEED> :
  Use the specified <SEED> to randomize the ordering in which specs are executed

* `--self-test`:
  If `fspec` was built with its own tests, then run the self-tests.

* `--show-pass-explicit`:
  Display messages when Assert.Pass is used

* `--show-tests`:
  Display names of tests and test cases even on success

* `--show-whitespace`:
   When comparing strings, special characters will be used when printing whitespace in assertion failure messages.

* `--skip`=<REGEX>:
  Skip tests whose descriptions or names match the given REGEX

* `--timeout`=<TIME>:
  The maximum <TIME> allowed for any particular test to execute.  The syntax of TIME is either a floating point number representing the whole and partial seconds to allow or it is the syntax of `System.TimeSpan` which looks like <DAYS>.<HOURS>:<MINUTES>:<SECONDS>.<TICKS>

* `--verify`=<VALUE>:
  Use the specified verification mode to check for errors in tests and assertions.  If `strict` is used, then tests could fail if tests or assertions are structured incorrectly.  If `none` is used, then no verification is used, but a warning is printed when certain verification errors occur.

* `--help`:
  Display the help screen

* `--version`:
  Report the version information and exit

## LOADER PATH

The loader path specifies the directories which are probed to find additional assemblies to load.  By default, the loader path contains each directory for each assembly that was specified at the command line.

## EXIT CODES

* 0:
  Success, all tests passed.

* 1:
  Failed, one or more tests failed.  If `--fail-pending` was used, then this is used when there are pending tests.

* 2:
  There was a problem configuring or starting up `fspec`

## COPYRIGHT

Copyright © 2020 Carbonfrost Systems, Inc.  Licensed under the terms of the Apache 2.0 license (https://apache.org/licenses/LICENSE-2.0.txt)
