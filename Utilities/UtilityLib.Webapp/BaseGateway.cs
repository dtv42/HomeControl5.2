// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseGateway.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>13-5-2020 13:53</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityLib.Webapp
{
    #region Using Directives

    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    #endregion Using Directives

    /// <summary>
    /// Base class for a gateway providing logger and configuration data members.
    /// The gateway accesses a data provider and updates the <see cref="DataStatus"/> field.
    /// </summary>
    public class BaseGateway : IGateway
    {
        #region Private Data Members

        /// <summary>
        ///  Instantiate a Singleton of the Semaphore with a value of 1.
        ///  This means that only 1 thread can be granted access at a time.
        /// </summary>
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        ///  The data status is modified using the public property Status.
        /// </summary>
        private DataStatus _status = DataStatus.Good;

        #endregion Private Data Members

        #region Protected Data Members

        protected readonly ILogger<BaseGateway>? _logger;

        #endregion Protected Data Members

        #region Public Properties

        /// <summary>
        /// Returns a flag that the gateway has completed the startup successfully.
        /// </summary>
        public bool IsStartupOk { get => true; }

        /// <summary>
        /// Returns a flag that the gateway is locked (access is in progress).
        /// </summary>
        public bool IsLocked { get => _semaphore.CurrentCount == 0; }

        /// <summary>
        /// The data status. Note that the set operation updates the timestamp.
        /// </summary>
        public DataStatus Status 
        { 
            get => _status;
            set
            {
                _status = value;
                _status.Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            }
        }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="BaseGateway"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public BaseGateway(ILogger<BaseGateway> logger)
        {
            _logger = logger;

            _logger?.LogDebug($"BaseGateway()");
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// The gateway startup routine.
        /// </summary>
        /// <returns>True if startup is successful.</returns>
        public virtual bool Startup() => true;

        /// <summary>
        /// The gateway checks the access to data provider.
        /// </summary>
        /// <returns>True if access is successful.</returns>
        public virtual bool CheckAccess() => true;

        /// <summary>
        /// The gateway checks the access to data provider using an async call.
        /// </summary>
        /// <returns>True if access is successful.</returns>
        public virtual async Task<bool> CheckAccessAsync() => await Task.FromResult(true);

        /// <summary>
        /// Lock the gateway access.
        /// </summary>
        public void Lock()
        {
            _logger.LogTrace("Enter semaphore");
            _semaphore.Wait();
        }

        /// <summary>
        /// Unlock the gateway using an async call.
        /// </summary>
        public async Task LockAsync()
        {
            _logger.LogTrace("Enter semaphore");
            await _semaphore.WaitAsync();
        }

        /// <summary>
        ///  Unlock the gateway.
        /// </summary>
        public void Unlock()
        {
            _logger.LogTrace("Release semaphore");
            _semaphore.Release();
        }

        #endregion Public Methods
    }
}