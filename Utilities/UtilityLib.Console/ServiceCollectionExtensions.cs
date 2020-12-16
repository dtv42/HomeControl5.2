// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandExtensions.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>15-12-2020 07:47</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityLib.Console
{
    #region Using Directives

    using System;
    using System.CommandLine;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    #endregion

    /// <summary>
    ///  Extension methods for command line options.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSingletonFromSection<TService>(this IServiceCollection services, string? section = null)
            where TService : class
        {
            services.AddSingleton(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();

                if (section is null)
                {
                    return configuration.GetSection($"AppSettings:{typeof(TService).Name}").Get<TService>()
                    ?? throw new ArgumentException($"Configuration 'AppSettings:{typeof(TService).Name}' cannot be missing");
                }
                else
                {
                    return configuration.GetSection(section).Get<TService>()
                    ?? throw new ArgumentException($"Configuration '{section}' cannot be missing");
                }
            });

            return services;
        }

        public static IServiceCollection AddSingletonFromSection<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>
            (this IServiceCollection services, string? section = null)
            where TService : class
            where TImplementation : class, TService
        {
            services.AddSingleton<TService, TImplementation>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();

                if (section is null)
                {
                    return configuration.GetSection($"AppSettings:{typeof(TImplementation).Name}").Get<TImplementation>()
                    ?? throw new ArgumentException($"Configuration 'AppSettings:{typeof(TImplementation).Name}' cannot be missing");
                }
                else
                {
                    return configuration.GetSection(section).Get<TImplementation>()
                    ?? throw new ArgumentException($"Configuration '{section}' cannot be missing");
                }
            });

            return services;
        }
    }
}
