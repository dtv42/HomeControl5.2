// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandExtensions.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>9-12-2020 11:26</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityLib.Console
{
    #region Using Directives

    using System;
    using System.CommandLine;

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
            services.AddSingleton(typeof(Command), typeof(TCommand));

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
            services.AddSingleton(typeof(Command), typeof(TCommand));

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
            services.AddSingleton(typeof(RootCommand), typeof(TCommand));

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
            services.AddSingleton(typeof(RootCommand), typeof(TCommand));

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
