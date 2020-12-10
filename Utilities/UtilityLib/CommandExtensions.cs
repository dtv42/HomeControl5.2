﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandExtensions.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>9-12-2020 11:26</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityLib
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.Linq;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    #endregion

    /// <summary>
    ///  Extension methods for command line options.
    /// </summary>
    public static class CommandExtensions
    {
        /// <summary>
        /// Adding a command.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCommand<TCommand>(this IServiceCollection services) 
            where TCommand : Command
        {
            Type grabCommandType = typeof(TCommand);
            Type commandType = typeof(Command);

            IEnumerable<Type> commands = grabCommandType
                .Assembly
                .GetExportedTypes()
                .Where(x => x.Namespace == grabCommandType.Namespace && commandType.IsAssignableFrom(x));

            foreach (Type command in commands)
            {
                services.AddSingleton(commandType, command);
            }

            return services;
        }

        /// <summary>
        /// Adding a command with options.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCommandOptions<TCommand, TOptions>(this IServiceCollection services) 
            where TCommand : Command
            where TOptions : class
        {
            Type grabCommandType = typeof(TCommand);
            Type commandType = typeof(Command);

            IEnumerable<Type> commands = grabCommandType
                .Assembly
                .GetExportedTypes()
                .Where(x => x.Namespace == grabCommandType.Namespace && commandType.IsAssignableFrom(x));

            foreach (Type command in commands)
            {
                services.AddSingleton(commandType, command);
            }

            services.AddSingleton(sp =>
            {
                return
                   sp.GetRequiredService<IConfiguration>().GetSection($"AppSettings:{typeof(TOptions).Name}").Get<TOptions>()
                   ?? throw new ArgumentException($"{typeof(TOptions).Name} configuration cannot be missing.");
            });

            return services;
        }

        /// <summary>
        /// Adding a root command.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRootCommand<TCommand>(this IServiceCollection services)
            where TCommand : RootCommand
        {
            Type grabCommandType = typeof(TCommand);
            Type commandType = typeof(RootCommand);

            IEnumerable<Type> commands = grabCommandType
                .Assembly
                .GetExportedTypes()
                .Where(x => x.Namespace == grabCommandType.Namespace && commandType.IsAssignableFrom(x));

            foreach (Type command in commands)
            {
                services.AddSingleton(commandType, command);
            }

            return services;
        }

        /// <summary>
        /// Adding a root command with options.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRootCommandOptions<TCommand, TOptions>(this IServiceCollection services)
            where TCommand : RootCommand
            where TOptions : class
        {
            Type grabCommandType = typeof(TCommand);
            Type commandType = typeof(RootCommand);

            IEnumerable<Type> commands = grabCommandType
                .Assembly
                .GetExportedTypes()
                .Where(x => x.Namespace == grabCommandType.Namespace && commandType.IsAssignableFrom(x));

            foreach (Type command in commands)
            {
                services.AddSingleton(commandType, command);
            }

            services.AddSingleton(sp =>
            {
                return
                   sp.GetRequiredService<IConfiguration>().GetSection($"AppSettings:{typeof(TOptions).Name}").Get<TOptions>()
                   ?? throw new ArgumentException($"{typeof(TOptions).Name} configuration cannot be missing.");
            });

            return services;
        }
    }
}
