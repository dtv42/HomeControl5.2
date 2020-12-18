// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WallboxClient.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 20:20</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace WallboxLib
{
    #region Using Directives

    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using WallboxLib.Models;

    #endregion

    public class WallboxClient : BaseClass<WallboxSettings>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WallboxClient"/> class.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="logger"></param>
        public WallboxClient(WallboxSettings settings,
                             ILogger<WallboxClient> logger)
            : base(settings, logger)
        {}

        #endregion

        #region Public Methods

        /// <summary>
        /// Helper method to send a request and return the response as a string.
        /// </summary>
        /// <param name="message">The UDP message</param>
        /// <returns>The string result.</returns>
        public async Task<string> SendReceiveAsync(string message)
        {
            try
            {
                var bytes = Encoding.ASCII.GetBytes(message);
                var endpoint = IPEndPoint.Parse(_settings.EndPoint);
                using var client = new UdpClient(_settings.Port);
                client.Connect(endpoint);
                var count = await client.SendAsync(bytes, bytes.Length);

                if (count == bytes.Length)
                {
                    var timeToWait = TimeSpan.FromSeconds(_settings.Timeout);
                    var asyncResult = client.BeginReceive(null, null);
                    asyncResult.AsyncWaitHandle.WaitOne(timeToWait);

                    if (asyncResult.IsCompleted)
                    {
                        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, _settings.Port);
                        bytes = client.EndReceive(asyncResult, ref remoteEP);
                        client.Close();
                        return Encoding.ASCII.GetString(bytes);
                    }
                    else
                    {
                        _logger?.LogError("SendReceiveAsync timeout receiving.");
                        throw new TimeoutException("Timeout receiving.");
                    }
                }
                else
                {
                    _logger?.LogError("SendReceiveAsync not all bytes sent.");
                    throw new InvalidDataException("Not all bytes sent.");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError("SendReceiveAsync exception: {0}.", ex.Message);
                throw ex;
            }
        }

        #endregion
    }
}
