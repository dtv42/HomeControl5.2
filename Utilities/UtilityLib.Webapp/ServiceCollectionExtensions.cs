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
namespace UtilityLib.Webapp
{
    #region Using Directives

    using System;
    using System.Net.Http;

    using Microsoft.Extensions.DependencyInjection;

    using Polly;

    #endregion

    /// <summary>
    ///  Extension methods for command line options.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPollyHttpClient<TService>(this IServiceCollection services, string name, IGatewaySettings settings)
            where TService : class
        {
            services.AddHttpClient(name, client =>
            {
                client.BaseAddress = new Uri(settings.Address);
                client.Timeout = TimeSpan.FromMilliseconds(settings.Timeout);
            })
            .ConfigureHttpMessageHandlerBuilder(config => _ = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            })
            .AddTransientHttpErrorPolicy(policy =>
                policy.WaitAndRetryAsync(
                    settings.Retries,
                    _ => TimeSpan.FromMilliseconds(settings.Wait))
                );

            return services;
        }
    }
}
