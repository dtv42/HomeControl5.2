// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeliosAttribute.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 10:05</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace HeliosLib.Models
{
    #region Using Directives

    using System;
    using System.Reflection;

    #endregion

    /// <summary>
    /// This attribute allows to mark a property with a Helios specific label attribute.
    /// 
    /// class HeliosClass
    /// {
    ///     [Helios("v00006")]
    ///     public ushort Value { get; set; } 
    /// }
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class HeliosAttribute : Attribute
    {
        #region Private Data Members

        private readonly string _name = string.Empty;

        #endregion

        #region Public Properties

        /// <summary>
        /// The Helios value name.
        /// </summary>
        public string Name { get { return _name; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HeliosAttribute"/> class.
        /// </summary>
        public HeliosAttribute(string name)
        {
            _name = name;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Helper function to return the Modbus custom attribute using a PropertyInfo.
        /// </summary>
        /// <param name="info">The property info.</param>
        /// <returns>The Modbus offset.</returns>
        public static HeliosAttribute GetHeliosAttribute(PropertyInfo? info)
        {
            if (!(info is null))
            {
                HeliosAttribute? attribute = info.GetCustomAttribute<HeliosAttribute>();

                if (attribute != null)
                {
                    return attribute;
                }
                else
                {
                    throw new InvalidOperationException($"Property '{info.Name}' has no Helios attribute set");
                }
            }
            else
            {
                throw new ArgumentException($"Specified PropertyInfo is null!");
            }
        }

        #endregion
    }
}
