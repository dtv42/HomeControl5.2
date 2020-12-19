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
    using System.Collections.Generic;
    using System.Net.Http;

    using Microsoft.Extensions.DependencyInjection;

    using Polly;

    #endregion

    /// <summary>
    ///  Extension methods for command line options.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the IHttpClientFactory and related services to the IServiceCollection
        /// and configures a named HttpClient using HttpError policies.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="name">The logical name of the HttpClient to configure.</param>
        /// <param name="retries">The HttpError policy retry count.</param>
        /// <param name="wait">The time to wait for a retry (msec).</param>
        /// <param name="configureClient">A delegate that is used to configure a HttpClient.</param>
        /// <param name="ignoreServerCertificates">A flag indicating that invalid server certificates are ignored.</param>
        /// <returns>An IHttpClientBuilder that can be used to configure the client.</returns>
        public static IHttpClientBuilder AddPollyHttpClient(this IServiceCollection services,
                                                            string name,
                                                            int retries, int wait,
                                                            Action<IServiceProvider, HttpClient> configureClient,
                                                            bool ignoreServerCertificates = true)
        {
            var builder = services
                .AddHttpClient(name)
                .ConfigureHttpClient(configureClient);

            if (ignoreServerCertificates)
            {
                builder.ConfigureHttpMessageHandlerBuilder(config => _ = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
                });
            }

            return builder.AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(retries, _ => TimeSpan.FromMilliseconds(wait)));
        }

        /// <summary>
        /// Adds the IHttpClientFactory and related services to the IServiceCollection
        /// and configures a named HttpClient using HttpError policies.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="name">The logical name of the HttpClient to configure.</param>
        /// <param name="retries">The HttpError policy retry count.</param>
        /// <param name="wait">The time to wait for a retry (msec).</param>
        /// <param name="configureClient">A delegate that is used to configure a HttpClient.</param>
        /// <param name="ignoreServerCertificates">A flag indicating that invalid server certificates are ignored.</param>
        /// <returns>An IHttpClientBuilder that can be used to configure the client.</returns>
        public static IHttpClientBuilder AddPollyHttpClient(this IServiceCollection services,
                                                            string name,
                                                            int retries, int wait,
                                                            Action<HttpClient> configureClient,
                                                            bool ignoreServerCertificates = true)
        {
            var builder = services
                .AddHttpClient(name)
                .ConfigureHttpClient(configureClient);

            if (ignoreServerCertificates)
            {
                builder.ConfigureHttpMessageHandlerBuilder(config => _ = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
                });
            }

            return builder.AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(retries, _ => TimeSpan.FromMilliseconds(wait)));
        }

        /// <summary>
        /// Adds the IHttpClientFactory and related services to the IServiceCollection
        /// and configures a named HttpClient using HttpError policies.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="name">The logical name of the HttpClient to configure.</param>
        /// <param name="retries">The HttpError policy retry count.</param>
        /// <param name="wait">The time to wait for a retry (msec).</param>
        /// <param name="configureClient">A delegate that is used to configure a HttpClient.</param>
        /// <param name="ignoreServerCertificates">A flag indicating that invalid server certificates are ignored.</param>
        /// <returns>An IHttpClientBuilder that can be used to configure the client.</returns>
        public static IHttpClientBuilder AddPollyHttpClient(this IServiceCollection services,
                                                            string name,
                                                            int retries, int wait,
                                                            bool ignoreServerCertificates = true)
        {
            var builder = services
                .AddHttpClient(name);

            if (ignoreServerCertificates)
            {
                builder.ConfigureHttpMessageHandlerBuilder(config => _ = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
                });
            }

            return builder.AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(retries, _ => TimeSpan.FromMilliseconds(wait)));
        }

        /// <summary>
        ///  Adds a named HttpClient and related services to the service collection and configures the Polly retry policy.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="name">The logical name of the HttpClient to configure.</param>
        /// <param name="address">The HttpClient base address.</param>
        /// <param name="timeout">The HttpClient request timeout (msec).</param>
        /// <param name="retries">The HttpError policy retry count.</param>
        /// <param name="wait">The time to wait for a retry (msec).</param>
        /// <param name="ignoreServerCertificates">A flag indicating that invalid server certificates are ignored.</param>
        /// <returns>An IHttpClientBuilder that can be used to configure the client.</returns>
        public static IHttpClientBuilder AddPollyHttpClient(this IServiceCollection services, 
                                                            string name,
                                                            string address, int timeout,
                                                            int retries, int wait,
                                                            bool ignoreServerCertificates = true)
        {
            var builder = services.AddHttpClient(name, client =>
            {
                client.BaseAddress = new Uri(address);
                client.Timeout = TimeSpan.FromMilliseconds(timeout);
            });

            if (ignoreServerCertificates)
            {
                builder.ConfigureHttpMessageHandlerBuilder(config => _ = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
                });
            }

            return builder.AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(retries, _ => TimeSpan.FromMilliseconds(wait)));
        }

        /// <summary>
        /// Adds the IHttpClientFactory and related services to the IServiceCollection
        /// and configures a named HttpClient using HttpError policies.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="name">The logical name of the HttpClient to configure.</param>
        /// <param name="sleepDurations">The sleep durations to wait for on each retry.</param>
        /// <param name="configureClient">A delegate that is used to configure a HttpClient.</param>
        /// <param name="ignoreServerCertificates">A flag indicating that invalid server certificates are ignored.</param>
        /// <returns>An IHttpClientBuilder that can be used to configure the client.</returns>
        public static IHttpClientBuilder AddPollyHttpClient(this IServiceCollection services,
                                                            string name,
                                                            IEnumerable<TimeSpan> sleepDurations,
                                                            Action<IServiceProvider, HttpClient> configureClient,
                                                            bool ignoreServerCertificates = true)
        {
            var builder = services
                .AddHttpClient(name)
                .ConfigureHttpClient(configureClient);

            if (ignoreServerCertificates)
            {
                builder.ConfigureHttpMessageHandlerBuilder(config => _ = new HttpClientHandler
                 {
                     ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
                 });
            }
            
            return builder.AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(sleepDurations));
        }

        /// <summary>
        /// Adds the IHttpClientFactory and related services to the IServiceCollection
        /// and configures a named HttpClient using HttpError policies.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="name">The logical name of the HttpClient to configure.</param>
        /// <param name="sleepDurations">The sleep durations to wait for on each retry.</param>
        /// <param name="configureClient">A delegate that is used to configure a HttpClient.</param>
        /// <param name="ignoreServerCertificates">A flag indicating that invalid server certificates are ignored.</param>
        /// <returns>An IHttpClientBuilder that can be used to configure the client.</returns>
        public static IHttpClientBuilder AddPollyHttpClient(this IServiceCollection services,
                                                            string name,
                                                            IEnumerable<TimeSpan> sleepDurations,
                                                            Action<HttpClient> configureClient,
                                                            bool ignoreServerCertificates = true)
        {
            var builder = services
                .AddHttpClient(name)
                .ConfigureHttpClient(configureClient);

            if (ignoreServerCertificates)
            {
                builder.ConfigureHttpMessageHandlerBuilder(config => _ = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
                });
            }

            return builder.AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(sleepDurations));
        }

        /// <summary>
        /// Adds the IHttpClientFactory and related services to the IServiceCollection
        /// and configures a named HttpClient using HttpError policies.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="name">The logical name of the HttpClient to configure.</param>
        /// <param name="sleepDurations">The sleep durations to wait for on each retry.</param>
        /// <param name="ignoreServerCertificates">A flag indicating that invalid server certificates are ignored.</param>
        /// <returns>An IHttpClientBuilder that can be used to configure the client.</returns>
        public static IHttpClientBuilder AddPollyHttpClient(this IServiceCollection services,
                                                            string name,
                                                            IEnumerable<TimeSpan> sleepDurations,
                                                            bool ignoreServerCertificates = true)
        {
            var builder = services
                .AddHttpClient(name);

            if (ignoreServerCertificates)
            {
                builder.ConfigureHttpMessageHandlerBuilder(config => _ = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
                });
            }

            return builder.AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(sleepDurations));
        }
    }
}
