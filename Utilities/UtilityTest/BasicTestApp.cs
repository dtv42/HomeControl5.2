namespace UtilityTest
{
    #region Using Directives

    using System;
    using System.Diagnostics;
    using System.IO;
    using UtilityLib;
    using Xunit;

    #endregion Using Directives

    public class BasicTestApp
    {
        #region Private Methods

        /// <summary>
        /// Starts the console application. Specify empty string to run with no arguments.
        /// </summary>
        /// <param name="args">The arguments for the console application.</param>
        /// <param name="delay">The optional wait for exit delay in msec.</param>
        /// <returns>The exit code.</returns>
        private static (int code, string result) StartConsoleApplication(string args, int delay = 1000)
        {
            var sw = new StringWriter();
            Console.SetOut(sw);

            // Initialize process here
            Process proc = new Process();
            proc.StartInfo.FileName = @"dotnet";

            // add arguments as whole string
            proc.StartInfo.Arguments = "run --no-restore --no-build -- " + args;

            // use it to start from testing environment
            proc.StartInfo.UseShellExecute = false;

            // redirect outputs to have it in testing console
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;

            // set working directory
            proc.StartInfo.WorkingDirectory = @"D:\Development\source\repos\HomeControl5.2\Utilities\UtilityApp\";

            // start and wait for exit
            proc.Start();
            proc.WaitForExit(delay);

            // get output to testing console.
            Console.WriteLine(proc.StandardOutput.ReadToEnd());
            Console.Write(proc.StandardError.ReadToEnd());

            // return exit code and results
            return (proc.ExitCode, sw.ToString());
        }

        #endregion Private Methods

        [Theory]
        [InlineData("",                        "Hello Console!",                       ExitCodes.SuccessfullyCompleted)]
        [InlineData("",                        "Time elapsed",                         ExitCodes.SuccessfullyCompleted)]
        [InlineData("--password secret",       "Time elapsed",                         ExitCodes.SuccessfullyCompleted)]
        [InlineData("--password=secret",       "Time elapsed",                         ExitCodes.SuccessfullyCompleted)]
        [InlineData("--password:secret",       "Time elapsed",                         ExitCodes.SuccessfullyCompleted)]
        [InlineData("--host http://localhost", "Time elapsed",                         ExitCodes.SuccessfullyCompleted)]
        [InlineData("-?",                      "UtilityApp [options] [command]",       ExitCodes.SuccessfullyCompleted)]
        [InlineData("--help",                  "UtilityApp [options] [command]",       ExitCodes.SuccessfullyCompleted)]
        [InlineData("--configuration",         "Configuration:",                       ExitCodes.SuccessfullyCompleted)]
        [InlineData("--settings",              "AppSettings:",                         ExitCodes.SuccessfullyCompleted)]
        [InlineData("--verbose",               "Commandline Application: UtilityApp",  ExitCodes.SuccessfullyCompleted)]
        [InlineData("--version",               "1.0.0",                                ExitCodes.SuccessfullyCompleted)]
        [InlineData("-",                       "Unrecognized command or argument '-'", ExitCodes.IncorrectFunction    )]
        [InlineData("---",                     "'---' was not matched.",               ExitCodes.IncorrectFunction    )]
        public void TestAppCommand(string args, string text, ExitCodes exit)
        {
            var (code, result) = StartConsoleApplication(args);
            Assert.Equal((int)exit, code);
            Assert.Contains(text, result);
        }

        [Theory]
        [InlineData("test XXX",             "Time elapsed",                                 ExitCodes.SuccessfullyCompleted)]
        [InlineData("test XXX",             "1 Argument(s) provided",                       ExitCodes.SuccessfullyCompleted)]
        [InlineData("test XXX YYY",         "Time elapsed",                                 ExitCodes.SuccessfullyCompleted)]
        [InlineData("test XXX YYY",         "2 Argument(s) provided",                       ExitCodes.SuccessfullyCompleted)]
        [InlineData("test -a",              "Time elapsed",                                 ExitCodes.SuccessfullyCompleted)]
        [InlineData("test -a",              "Option A provided",                            ExitCodes.SuccessfullyCompleted)]
        [InlineData("test -a --verbose",    "Commandline Application: UtilityApp",          ExitCodes.SuccessfullyCompleted)]
        [InlineData("test -a -h",           "Time elapsed",                                 ExitCodes.SuccessfullyCompleted)]
        [InlineData("test -a -h",           "Option A provided",                            ExitCodes.SuccessfullyCompleted)]
        [InlineData("test -a -h",           "Option H provided",                            ExitCodes.SuccessfullyCompleted)]
        [InlineData("test -a --hoption",    "Time elapsed",                                 ExitCodes.SuccessfullyCompleted)]
        [InlineData("test -?",              "UtilityApp test [options] [<name> [<value>]]", ExitCodes.SuccessfullyCompleted)]
        [InlineData("test --help",          "UtilityApp test [options] [<name> [<value>]]", ExitCodes.SuccessfullyCompleted)]
        [InlineData("test",                 "Please select at least one option (-a|-b|-c)", ExitCodes.IncorrectFunction    )]
        [InlineData("test --verbose",       "Please select at least one option (-a|-b|-c)", ExitCodes.IncorrectFunction    )]
        [InlineData("test -h",              "Please select at least one option (-a|-b|-c)", ExitCodes.IncorrectFunction    )]
        public void TestTestCommand(string args, string text, ExitCodes exit)
        {
            var (code, result) = StartConsoleApplication(args);
            Assert.Equal((int)exit, code);
            Assert.Contains(text, result);
        }

        [Theory]
        [InlineData("async",           "Time elapsed",                        ExitCodes.SuccessfullyCompleted, 10000)]
        [InlineData("async --verbose", "Commandline Application: UtilityApp", ExitCodes.SuccessfullyCompleted, 10000)]
        [InlineData("async -d 1",      "Time elapsed",                        ExitCodes.SuccessfullyCompleted, 1000 )]
        [InlineData("async --delay 1", "Time elapsed",                        ExitCodes.SuccessfullyCompleted, 1000 )]
        [InlineData("async -?",        "UtilityApp async [options]",          ExitCodes.SuccessfullyCompleted, 1000 )]
        [InlineData("async --help",    "UtilityApp async [options]",          ExitCodes.SuccessfullyCompleted, 1000 )]
        public void TestAsyncCommand(string args, string text, ExitCodes exit, int delay)
        {
            var (code, result) = StartConsoleApplication(args, delay);
            Assert.Equal((int)exit, code);
            Assert.Contains(text, result);
        }

        [Theory]
        [InlineData("error",             "Exit code: NotSuccessfullyCompleted",     ExitCodes.NotSuccessfullyCompleted)]
        [InlineData("error --verbose",   "Commandline Application: UtilityApp",     ExitCodes.NotSuccessfullyCompleted)]
        [InlineData("error -x",          "Exception: Application exception thrown", ExitCodes.UnhandledException      )]
        [InlineData("error -x",          "Exit code: UnhandledException",           ExitCodes.UnhandledException      )]
        [InlineData("error --exception", "Exception: Application exception thrown", ExitCodes.UnhandledException      )]
        [InlineData("error --exception", "Exit code: UnhandledException",           ExitCodes.UnhandledException      )]
        [InlineData("error -?",          "UtilityApp error [options]",              ExitCodes.SuccessfullyCompleted   )]
        [InlineData("error --help",      "UtilityApp error [options]",              ExitCodes.SuccessfullyCompleted   )]
        public void TestErrorCommand(string args, string text, ExitCodes exit)
        {
            var (code, result) = StartConsoleApplication(args);
            Assert.Equal((int)exit, code);
            Assert.Contains(text, result);
        }

        [Theory]
        [InlineData("greet",           "Hello world!",                        ExitCodes.SuccessfullyCompleted)]
        [InlineData("greet",           "Time elapsed",                        ExitCodes.SuccessfullyCompleted)]
        [InlineData("greet --verbose", "Commandline Application: UtilityApp", ExitCodes.SuccessfullyCompleted)]
        [InlineData("greet dtv",       "Hello dtv!",                          ExitCodes.SuccessfullyCompleted)]
        [InlineData("greet dtv",       "Time elapsed",                        ExitCodes.SuccessfullyCompleted)]
        [InlineData("greet -?",        "UtilityApp greet [options] [<name>]", ExitCodes.SuccessfullyCompleted)]
        [InlineData("greet --help",    "UtilityApp greet [options] [<name>]", ExitCodes.SuccessfullyCompleted)]
        public void TestGreetCommand(string args, string text, ExitCodes exit)
        {
            var (code, result) = StartConsoleApplication(args);
            Assert.Equal((int)exit, code);
            Assert.Contains(text, result);
        }

        [Theory]
        [InlineData("logging",           "Time elapsed",                        ExitCodes.SuccessfullyCompleted)]
        [InlineData("logging",           "[INF] Information Message",           ExitCodes.SuccessfullyCompleted)]
        [InlineData("logging",           "[WRN] Warning Message",               ExitCodes.SuccessfullyCompleted)]
        [InlineData("logging",           "[ERR] Error Message",                 ExitCodes.SuccessfullyCompleted)]
        [InlineData("logging",           "[FTL] Critical Message",              ExitCodes.SuccessfullyCompleted)]
        [InlineData("logging --verbose", "Commandline Application: UtilityApp", ExitCodes.SuccessfullyCompleted)]
        [InlineData("logging --verbose", "MinimumLevel Default:    Debug",      ExitCodes.SuccessfullyCompleted)]
        [InlineData("logging --verbose", "MinimumLevel System:     Warning",    ExitCodes.SuccessfullyCompleted)]
        [InlineData("logging --verbose", "MinimumLevel Microsoft:  Warning",    ExitCodes.SuccessfullyCompleted)]
        [InlineData("logging -?",        "UtilityApp logging [options]",        ExitCodes.SuccessfullyCompleted)]
        [InlineData("logging --help",    "UtilityApp logging [options]",        ExitCodes.SuccessfullyCompleted)]
        public void TestLoggingCommand(string args, string text, ExitCodes exit)
        {
            var (code, result) = StartConsoleApplication(args);
            Assert.Equal((int)exit, code);
            Assert.Contains(text, result);
        }

        [Theory]
        [InlineData("settings",           "Settings:",                           ExitCodes.SuccessfullyCompleted)]
        [InlineData("settings -o",        "GlobalOptions:",                      ExitCodes.SuccessfullyCompleted)]
        [InlineData("settings --options", "GlobalOptions:",                      ExitCodes.SuccessfullyCompleted)]
        [InlineData("settings -j",        "AppSettings:",                        ExitCodes.SuccessfullyCompleted)]
        [InlineData("settings --json",    "AppSettings:",                        ExitCodes.SuccessfullyCompleted)]
        [InlineData("settings --verbose", "Commandline Application: UtilityApp", ExitCodes.SuccessfullyCompleted)]
        [InlineData("settings -?",        "UtilityApp settings [options]",       ExitCodes.SuccessfullyCompleted)]
        [InlineData("settings --help",    "UtilityApp settings [options]",       ExitCodes.SuccessfullyCompleted)]
        public void TestSettingsCommand(string args, string text, ExitCodes exit)
        {
            var (code, result) = StartConsoleApplication(args);
            Assert.Equal((int)exit, code);
            Assert.Contains(text, result);
        }

        [Theory]
        [InlineData("testdata",                                                "Time elapsed",                                   ExitCodes.SuccessfullyCompleted)]
        [InlineData("testdata -j",                                             "TestData: {",                                    ExitCodes.SuccessfullyCompleted)]
        [InlineData("testdata --json",                                         "TestData: {",                                    ExitCodes.SuccessfullyCompleted)]
        [InlineData("testdata -n text",                                        "Name:     text",                                 ExitCodes.SuccessfullyCompleted)]
        [InlineData("testdata --name text",                                    "Name:     text",                                 ExitCodes.SuccessfullyCompleted)]
        [InlineData("testdata -g 6dceb058-526f-4413-8fce-b60257a496a2",        "Guid:     6dceb058-526f-4413-8fce-b60257a496a2", ExitCodes.SuccessfullyCompleted)]
        [InlineData("testdata --guid 6dceb058-526f-4413-8fce-b60257a496a2",    "Guid:     6dceb058-526f-4413-8fce-b60257a496a2", ExitCodes.SuccessfullyCompleted)]
        [InlineData("testdata -a \"0.0.0.0\"",                                 "Address:  0.0.0.0",                              ExitCodes.SuccessfullyCompleted)]
        [InlineData("testdata --address \"0.0.0.0\"",                          "Address:  0.0.0.0",                              ExitCodes.SuccessfullyCompleted)]
        [InlineData("testdata -e \"0.0.0.0:88\"",                              "Endpoint: 0.0.0.0:88",                           ExitCodes.SuccessfullyCompleted)]
        [InlineData("testdata --endpoint \"0.0.0.0:88\"",                      "Endpoint: 0.0.0.0:88",                           ExitCodes.SuccessfullyCompleted)]
        [InlineData("testdata -u \"http://0.0.0.0\"",                          "Uri:      http://0.0.0.0",                       ExitCodes.SuccessfullyCompleted)]
        [InlineData("testdata --uri \"http://0.0.0.0\"",                       "Uri:      http://0.0.0.0",                       ExitCodes.SuccessfullyCompleted)]
        [InlineData("testdata -c \"ok\"",                                      "Code:     OK",                                   ExitCodes.SuccessfullyCompleted)]
        [InlineData("testdata --code \"ok\"",                                  "Code:     OK",                                   ExitCodes.SuccessfullyCompleted)]
        [InlineData("testdata -n verylongtext",                                "name value length must be max. 10",              ExitCodes.IncorrectFunction    )]
        [InlineData("testdata --name verylongtext",                            "name value length must be max. 10",              ExitCodes.IncorrectFunction    )]
        [InlineData("testdata -g 6dceb058-526f-4413-8fce-b60257a496a2-xx",     "guid value is not a valid GUID",                 ExitCodes.IncorrectFunction    )]
        [InlineData("testdata --guid 6dceb058-526f-4413-8fce-b60257a496a2-xx", "guid value is not a valid GUID",                 ExitCodes.IncorrectFunction    )]
        [InlineData("testdata -a \"0.0.0.0.0\"",                               "address value is not a valid IP address",        ExitCodes.IncorrectFunction    )]
        [InlineData("testdata --address \"0.0.0.0.0\"",                        "address value is not a valid IP address",        ExitCodes.IncorrectFunction    )]
        [InlineData("testdata -e \"0.0.0.0.0:88\"",                            "endpoint value is not a valid IP endpoint",      ExitCodes.IncorrectFunction    )]
        [InlineData("testdata --endpoint \"0.0.0.0.0:88\"",                    "endpoint value is not a valid IP endpoint",      ExitCodes.IncorrectFunction    )]
        [InlineData("testdata -u \"httpx://0.0.0.0\"",                         "uri schema value is not valid",                  ExitCodes.IncorrectFunction    )]
        [InlineData("testdata --uri \"httpx:0.0.0.0\"",                        "uri schema value is not valid",                  ExitCodes.IncorrectFunction    )]
        [InlineData("testdata -u \"http://0.0.0.0:100000\"",                   "uri value is not a valid URI",                   ExitCodes.IncorrectFunction    )]
        [InlineData("testdata --uri \"http:0.0.0.0:100000\"",                  "uri value is not a valid URI",                   ExitCodes.IncorrectFunction    )]
        [InlineData("testdata -c \"xxx\"",                                     "Cannot parse argument 'xxx' for option '-c'",    ExitCodes.IncorrectFunction    )]
        [InlineData("testdata --code \"xxx\"",                                 "Cannot parse argument 'xxx' for option '-c'",    ExitCodes.IncorrectFunction    )]
        [InlineData("testdata --verbose",                                      "Commandline Application: UtilityApp",            ExitCodes.SuccessfullyCompleted)]
        [InlineData("testdata -?",                                             "UtilityApp testdata [options]",                  ExitCodes.SuccessfullyCompleted)]
        [InlineData("testdata --help",                                         "UtilityApp testdata [options]",                  ExitCodes.SuccessfullyCompleted)]
        public void TestTestdataCommand(string args, string text, ExitCodes exit)
        {
            var (code, result) = StartConsoleApplication(args);
            Assert.Equal((int)exit, code);
            Assert.Contains(text, result);
        }

        [Theory]
        [InlineData("property",                     "Please select at least a property type",           ExitCodes.IncorrectFunction)]
        [InlineData("property -p",                  "Time elapsed",                                     ExitCodes.SuccessfullyCompleted)]
        [InlineData("property -a",                  "Property StringArray",                             ExitCodes.SuccessfullyCompleted)]
        [InlineData("property -l",                  "Property StringList",                              ExitCodes.SuccessfullyCompleted)]
        [InlineData("property -s",                  "Property StringValue",                             ExitCodes.SuccessfullyCompleted)]
        [InlineData("property -d",                  "Dictionary",                                       ExitCodes.SuccessfullyCompleted)]
        [InlineData("property StringValue",         "Property StringValue",                             ExitCodes.SuccessfullyCompleted)]
        [InlineData("property StringValue -v",      "Value:         a string",                          ExitCodes.SuccessfullyCompleted)]
        [InlineData("property StringValue --value", "Value:         a string",                          ExitCodes.SuccessfullyCompleted)]
        [InlineData("property -p --verbose",        "Commandline Application: UtilityApp",              ExitCodes.SuccessfullyCompleted)]
        [InlineData("property -?",                  "UtilityApp property [options] [<name> [<value>]]", ExitCodes.SuccessfullyCompleted)]
        [InlineData("property --help",              "UtilityApp property [options] [<name> [<value>]]", ExitCodes.SuccessfullyCompleted)]
        public void TestPropertyCommand(string args, string text, ExitCodes exit)
        {
            var (code, result) = StartConsoleApplication(args);
            Assert.Equal((int)exit, code);
            Assert.Contains(text, result);
        }

        [Theory]
        [InlineData("validate -v 42",                        "V Integer Value: 42",                                        ExitCodes.SuccessfullyCompleted)]
        [InlineData("validate -v 42 -d 10",                  "D Integer Value: 10",                                        ExitCodes.SuccessfullyCompleted)]
        [InlineData("validate -v 42 -o",                     "O Integer Value: ",                                          ExitCodes.SuccessfullyCompleted)]
        [InlineData("validate -v 42 -o 10",                  "O Integer Value: 10",                                        ExitCodes.SuccessfullyCompleted)]
        [InlineData("validate -v 42 -n 1",                   "N Integer Value: 1",                                         ExitCodes.SuccessfullyCompleted)]
        [InlineData("validate -v 42 -r 10",                  "R Integer Value: 10",                                        ExitCodes.SuccessfullyCompleted)]
        [InlineData("validate -v 42 -l 1000000",             "L Integer Value: 1000000",                                   ExitCodes.SuccessfullyCompleted)]
        [InlineData("validate -v 42 -s string",              "S String  Value: 'string'",                                  ExitCodes.SuccessfullyCompleted)]
        [InlineData("validate -v 42 -m string",              "M String  Value: 'string'",                                  ExitCodes.SuccessfullyCompleted)]
        [InlineData("validate -v 42 -e string",              "E String  Value: 'string'",                                  ExitCodes.SuccessfullyCompleted)]
        [InlineData("validate -v 42 -w string",              "W String  Value: 'string'",                                  ExitCodes.SuccessfullyCompleted)]
        [InlineData("validate -v 42 -x name@host.com",       "X String  Value: 'name@host.com'",                           ExitCodes.SuccessfullyCompleted)]
        [InlineData("validate -v 42 -a 127.0.0.1",           "A String  Value: '127.0.0.1'",                               ExitCodes.SuccessfullyCompleted)]
        [InlineData("validate -v 42 -p 127.0.0.1:502",       "P String  Value: '127.0.0.1:502'",                           ExitCodes.SuccessfullyCompleted)]
        [InlineData("validate -v 42 -u http://127.0.0.1:88", "U String  Value: 'http://127.0.0.1:88'",                     ExitCodes.SuccessfullyCompleted)]
        [InlineData("validate -v 42 --verbose",              "Commandline Application: UtilityApp",                        ExitCodes.SuccessfullyCompleted)]
        [InlineData("validate -?",                           "UtilityApp validate [options]",                              ExitCodes.SuccessfullyCompleted)]
        [InlineData("validate --help",                       "UtilityApp validate [options]",                              ExitCodes.SuccessfullyCompleted)]
        [InlineData("validate",                              "Option '-v' is required.",                                   ExitCodes.IncorrectFunction    )]
        [InlineData("validate -v 42 -o 1 -o 2",              "Option '-o' expects a single argument but 2 were provided.", ExitCodes.IncorrectFunction    )]
        [InlineData("validate -v 42 -n 0",                   "Argument '0' not recognized. Must be one of:",               ExitCodes.IncorrectFunction    )]
        [InlineData("validate -v 42 -r 100",                 "number value must be between 0 and 10 (incl.)",              ExitCodes.IncorrectFunction    )]
        [InlineData("validate -v 42 -l 100000000",           "number value must be between 0 and 10000000 (incl.)",        ExitCodes.IncorrectFunction    )]
        [InlineData("validate -v 42 -m stringstring",        "string value length must be max. 10",                        ExitCodes.IncorrectFunction    )]
        [InlineData("validate -v 42 -e \"\"",                "string value must not be empty",                             ExitCodes.IncorrectFunction    )]
        [InlineData("validate -v 42 -w \" \"",               "string value must not be white space only",                  ExitCodes.IncorrectFunction    )]
        [InlineData("validate -v 42 -x string",              "string value must match the regular expression:",            ExitCodes.IncorrectFunction    )]
        [InlineData("validate -v 42 -a string",              "address value is not a valid IP address",                    ExitCodes.IncorrectFunction    )]
        [InlineData("validate -v 42 -a 127.0.0.1.1",         "address value is not a valid IP address",                    ExitCodes.IncorrectFunction    )]
        [InlineData("validate -v 42 -p string",              "endpoint value is not a valid IP endpoint",                  ExitCodes.IncorrectFunction    )]
        [InlineData("validate -v 42 -p 127.0.0.1.1",         "endpoint value is not a valid IP endpoint",                  ExitCodes.IncorrectFunction    )]
        [InlineData("validate -v 42 -p 127.0.0.1:100000",    "endpoint value is not a valid IP endpoint",                  ExitCodes.IncorrectFunction    )]
        [InlineData("validate -v 42 -u stringstring",        "uri value is not a valid URI",                               ExitCodes.IncorrectFunction    )]
        [InlineData("validate -v 42 -u 127.0.0.1",           "uri value is not a valid URI",                               ExitCodes.IncorrectFunction    )]
        [InlineData("validate -v 42 -u file://string",       "uri schema value is not valid",                              ExitCodes.IncorrectFunction    )]
        public void TestValidateCommand(string args, string text, ExitCodes exit)
        {
            var (code, result) = StartConsoleApplication(args);
            Assert.Equal((int)exit, code);
            Assert.Contains(text, result);
        }
    }
}
