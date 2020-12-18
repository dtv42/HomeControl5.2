// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGateway.cs" company="DTV-Online">
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

    using System.Threading.Tasks;

    #endregion

    public interface IGateway
    {
        /// <summary>
        /// Returns a flag that the gateway has completed the startup successfully.
        /// </summary>
        bool IsStartupOk { get; }

        /// <summary>
        /// Returns a flag that the gateway is locked (access is in progress).
        /// </summary>
        bool IsLocked { get; }

        /// <summary>
        /// The data status. Note that the set operation updates the timestamp.
        /// </summary>
        DataStatus Status { get; set; }

        /// <summary>
        /// The gateway startup routine.
        /// </summary>
        /// <returns>True if startup is successful.</returns>
        bool Startup();

        /// <summary>
        /// The gateway checks the access to data provider.
        /// </summary>
        /// <returns>True if access is successful.</returns>
        bool CheckAccess();

        /// <summary>
        /// The gateway checks the access to data provider using an async call.
        /// </summary>
        /// <returns>True if access is successful.</returns>
        Task<bool> CheckAccessAsync();

        /// <summary>
        /// Lock the gateway access.
        /// </summary>
        void Lock();

        /// <summary>
        /// Unlock the gateway using an async call.
        /// </summary>
        Task LockAsync();

        /// <summary>
        ///  Unlock the gateway.
        /// </summary>
        void Unlock();
    }
}
