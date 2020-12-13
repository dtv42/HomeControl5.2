﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestdataCommand.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>10-12-2020 16:49</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityApp.Commands
{
    #region Using Directives

    using System;
    using System.CommandLine;
    using System.CommandLine.IO;
    using System.CommandLine.Invocation;
    using System.Text.Json;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityApp.Models;
    using System.Net;
    using UtilityApp.Options;
    using System.CommandLine.Parsing;
    using Microsoft.Extensions.Configuration;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    #endregion Using Directives

    /// <summary>
    ///  Sample of a command using the custom TestData settings.
    /// </summary>
    public class TestdataCommand : BaseCommand
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _jsonoptions = JsonExtensions.DefaultSerializerOptions;

        #endregion Private Data Members

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="TestdataCommand"/> class.
        ///  Note that the code option is hidden in the help output (see https://github.com/dotnet/command-line-api/issues/629).
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public TestdataCommand(IConfiguration configuration, ILogger<TestdataCommand> logger)
            : base(logger, "testdata", "A dotnet console application sub command - testdata command")
        {
            logger.LogDebug("TestCommand()");

            // Get test data from configuration.
            TestData testdata = new TestData();
            configuration.GetSection("TestData").Bind(testdata);

            // Setup command options. The option variables are used to interpret the commandline parser results.
            var dOption = new Option<bool>          (new string[] { "-d", "--data"     }, "show new data");
            var vOption = new Option<int>           (new string[] { "-v", "--value"    }, "specify integer value[0..60]").Default(testdata.Value)   .Range(0, 60);
            var nOption = new Option<string>        (new string[] { "-n", "--name"     }, "specify name (max.10)")       .Default(testdata.Name)    .StringLength(10);
            var gOption = new Option<string>        (new string[] { "-g", "--guid"     }, "specify GUID")                .Default(testdata.Guid)    .Guid();
            var aOption = new Option<string>        (new string[] { "-a", "--address"  }, "specify IP address")          .Default(testdata.Address) .IPAddress();
            var eOption = new Option<string>        (new string[] { "-e", "--endpoint" }, "specify IP endpoint")         .Default(testdata.Endpoint).IPEndpoint();
            var uOption = new Option<string>        (new string[] { "-u", "--uri"      }, "specify absolute URI")        .Default(testdata.Uri)     .Uri();
            var cOption = new Option<HttpStatusCode>(new string[] { "-c", "--code"     }, "specify HTTP status code")    .Default(testdata.Code)    .IsHidden();

            AddOption(dOption);
            AddOption(vOption);
            AddOption(nOption);
            AddOption(gOption);
            AddOption(aOption);
            AddOption(eOption);
            AddOption(uOption);
            AddOption(cOption);

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, ParseResult, bool, TestdataOptions>((console, result, verbose, options) =>
            {
                logger.LogDebug("Handler()");

                // Validate the testdata.
                testdata.ValidateAndThrow();

                if (verbose)
                {
                    console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                    console.Out.WriteLine($"TestData: {JsonSerializer.Serialize(testdata, _jsonoptions)}");
                    console.Out.WriteLine();
                }

                if (options.Data)
                {
                    var data = new TestData();

                    console.Out.WriteLine($"New Data():");
                    console.Out.WriteLine($"    Value:    {data.Value}");
                    console.Out.WriteLine($"    Name:     {data.Name}");
                    console.Out.WriteLine($"    Guid:     {data.Guid}");
                    console.Out.WriteLine($"    Address:  {data.Address}");
                    console.Out.WriteLine($"    Endpoint: {data.Endpoint}");
                    console.Out.WriteLine($"    Uri:      {data.Uri}");
                    console.Out.WriteLine($"    Code:     {data.Code}");
                    console.Out.WriteLine();
                }

                console.Out.WriteLine($"Options:");
                console.Out.WriteLine($"    Data:     {options.Data}");
                console.Out.WriteLine($"    Value:    {options.Value}");
                console.Out.WriteLine($"    Name:     {options.Name}");
                console.Out.WriteLine($"    Guid:     {options.Guid}");
                console.Out.WriteLine($"    Address:  {options.Address}");
                console.Out.WriteLine($"    Endpoint: {options.Endpoint}");
                console.Out.WriteLine($"    Uri:      {options.Uri}");
                console.Out.WriteLine($"    Code:     {options.Code}");
                console.Out.WriteLine();

                console.Out.WriteLine($"CommandLine:");

                if (result.Has(dOption)) { console.Out.WriteLine($"    Data:     {options.Data}");    }
                if (result.Has(vOption)) { console.Out.WriteLine($"    Value:    {options.Value} ");  }
                if (result.Has(nOption)) { console.Out.WriteLine($"    Name:     {options.Name}");    }
                if (result.Has(gOption)) { console.Out.WriteLine($"    Guid:     {options.Guid}");    }
                if (result.Has(aOption)) { console.Out.WriteLine($"    Address:  {options.Address}"); }
                if (result.Has(eOption)) { console.Out.WriteLine($"    Endpoint: {options.Endpoint}");}
                if (result.Has(uOption)) { console.Out.WriteLine($"    Uri:      {options.Uri}");     }
                if (result.Has(cOption)) { console.Out.WriteLine($"    Code:     {options.Code}");    }

                console.Out.WriteLine();

                return (int)ExitCodes.SuccessfullyCompleted;
            });
        }

        #endregion Constructors
    }
}