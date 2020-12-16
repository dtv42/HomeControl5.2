// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyOptions.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>13-5-2020 17:37</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityApp.Options
{
    /// <summary>
    ///  A collection of options for the validate command.
    /// </summary>
    internal class PropertyOptions
    {
        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="PropertyOptions"/> class.
        /// </summary>
        public PropertyOptions() { }

        /// <summary>
        ///  Initializes a new instance of the <see cref="PropertyOptions"/> class
        ///  mapping the input parameters to the corresponding properties.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        /// <param name="a"></param>
        /// <param name="l"></param>
        /// <param name="d"></param>
        /// <param name="v"></param>
        public PropertyOptions(bool properties = false,
                               bool simple     = false,
                               bool array      = false,
                               bool list       = false,
                               bool dictionary = false,
                               bool value      = false)
        {
            All           = properties;
            Simple        = simple;
            Arrays        = array;
            Lists         = list;
            Dictionaries  = dictionary;
            Value         = value;
        }

        #endregion

        public bool All             { get; set; }
        public bool Simple          { get; set; }
        public bool Arrays          { get; set; }
        public bool Lists           { get; set; }
        public bool Dictionaries    { get; set; }
        public bool Value           { get; set; }
    }
}
