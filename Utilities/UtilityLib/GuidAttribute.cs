// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UriAttribute.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>13-5-2020 13:53</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityLib
{
    #region Using Directives

    using System;
    using System.ComponentModel.DataAnnotations;

    #endregion Using Directives

    /// <summary>
    ///  Validates a Guid option value.
    /// </summary>
    public sealed class GuidAttribute : ValidationAttribute
    {
        public GuidAttribute()
            : base("The Guid value must be valid: '{dddddddd-dddd-dddd-dddd-dddddddddddd}' or 'dddddddd-dddd-dddd-dddd-dddddddddddd' or (dddddddd-dddd-dddd-dddd-dddddddddddd)")
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            if ((value is null) || !Guid.TryParse((string)value, out Guid _))
            {
                return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}