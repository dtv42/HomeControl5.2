// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FroniusExtensions.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace FroniusLib.Extensions
{
    using FroniusLib.Models;

    public static class FroniusExtensions
    {
        public static StatusCodes ToStatusCode(this int code)
        {
            return code switch
            {
                int c when (c >= 0 && c < 7) => StatusCodes.Startup,
                7 => StatusCodes.Running,
                8 => StatusCodes.Standby,
                9 => StatusCodes.Bootloading,
                10 => StatusCodes.Error,
                _ => StatusCodes.Unknown,
            };
        }
    }
}
