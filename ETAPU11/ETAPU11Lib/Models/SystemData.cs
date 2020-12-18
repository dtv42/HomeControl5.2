// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemData.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>24-4-2020 16:40</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11Lib.Models
{
    public class SystemData
    {
        #region Public Properties

        /// <summary>
        /// The ETAPU11 property subset.
        /// </summary>
        public double OutsideTemperature { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the Properties used in SystemData.
        /// </summary>
        /// <param name="data">The ETAPU11 data.</param>
        public void Refresh(ETAPU11Data data)
        {
            OutsideTemperature = data.OutsideTemperature;
        }

        #endregion
    }
}