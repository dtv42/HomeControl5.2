// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionsExtensions.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>2-12-2020 11:28</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityLib
{
    #region Using Directives

    using System;
    using System.CommandLine;
    using System.CommandLine.Parsing;
    using System.Linq;
    using System.Text.RegularExpressions;

    #endregion

    /// <summary>
    ///  Extension methods for command line options.
    /// </summary>
    public static class OptionExtensions
    {
        public static TOption Name<TOption>(this TOption option, string value)
            where TOption : Option
        {
            option.Argument.Name = value;
            return option;
        }

        public static TOption IsHidden<TOption>(this TOption option, bool value = true)
            where TOption : Option
        {
            option.IsHidden = value;
            return option;
        }

        public static TOption IsRequired<TOption>(this TOption option, bool value = true)
            where TOption : Option
        {
            option.IsRequired = value;
            return option;
        }

        public static TOption Default<TOption>(this TOption option, object value)
            where TOption : Option
        {
            option.Argument.SetDefaultValue(value);
            return option;
        }

        public static TOption Arity<TOption>(this TOption option, IArgumentArity arity)
            where TOption : Option
        {
            option.Argument.Arity = arity;
            return option;
        }

        public static TOption FromAmong<TOption>(this TOption option, params int[] values)
            where TOption : Option<int>
        {
            option.FromAmong(values.ToList<int>().Select(v => v.ToString()).ToArray());
            return option;
        }

        public static TOption Range<TOption>(this TOption option, double minimum, double maximum)
            where TOption : Option<double>
        {
            option.Argument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<double>();
                if ((value >= minimum) && (value <= maximum)) return null;
                return $"{r.Symbol.Name} value must be between {minimum} and {maximum} (incl.)";
            });

            return option;
        }

        public static TOption Range<TOption>(this TOption option, int minimum, int maximum)
            where TOption : Option<int>
        {
            option.Argument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<int>();
                if ((value >= minimum) && (value <= maximum)) return null;
                return $"{r.Symbol.Name} value must be between {minimum} and {maximum} (incl.)";
            });

            return option;
        }

        public static TOption Range<TOption>(this TOption option, long minimum, long maximum)
            where TOption : Option<long>
        {
            option.Argument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<long>();
                if ((value >= minimum) && (value <= maximum)) return null;
                return $"{r.Symbol.Name} value must be between {minimum} and {maximum} (incl.)";
            });

            return option;
        }

        public static TOption StringLength<TOption>(this TOption option, int length)
            where TOption : Option<string>
        {
            option.Argument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<string>();
                if (value is null) return $"{r.Symbol.Name} value is null";
                if (value.Length <= length) return null;
                return $"{r.Symbol.Name} value length must be max. {length}";
            });

            return option;
        }

        public static TOption NotEmpty<TOption>(this TOption option)
            where TOption : Option<string>
        {
            option.Argument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<string>();
                if (value is null) return $"{r.Symbol.Name} value is null";
                if (!string.IsNullOrEmpty(value)) return null;
                return $"{r.Symbol.Name} value must not be empty";
            });

            return option;
        }

        public static TOption NotWhiteSpace<TOption>(this TOption option)
            where TOption : Option<string>
        {
            option.Argument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<string>();
                if (value is null) return $"{r.Symbol.Name} value is null";
                if (!string.IsNullOrWhiteSpace(value)) return null;
                return $"{r.Symbol.Name} value must not be white space only";
            });

            return option;
        }

        public static TOption Regex<TOption>(this TOption option, string pattern)
            where TOption : Option<string>
        {
            option.Argument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<string>();
                if (value is null) return $"{r.Symbol.Name} value is null";
                var regex = new Regex(pattern);
                if (regex.IsMatch(value)) return null;
                return $"{r.Symbol.Name} value must match the regular expression: {pattern}";
            });

            return option;
        }

        public static TOption IPAddress<TOption>(this TOption option)
            where TOption : Option<string>
        {
            option.Argument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<string>();
                if (value is null) return $"{r.Symbol.Name} value is null";
                if (System.Net.IPAddress.TryParse(value, out _)) return null;
                return $"{r.Symbol.Name} value is not a valid IP address";
            });

            return option;
        }

        public static TOption IPEndpoint<TOption>(this TOption option)
            where TOption : Option<string>
        {
            option.Argument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<string>();
                if (value is null) return $"{r.Symbol.Name} value is null";
                if (System.Net.IPEndPoint.TryParse(value, out _)) return null;
                return $"{r.Symbol.Name} value is not a valid IP endpoint";
            });

            return option;
        }

        public static TOption Uri<TOption>(this TOption option, UriKind kind = UriKind.Absolute, bool httpOnly = true)
            where TOption : Option<Uri>
        {
            option.Argument.AddValidator(r =>
            {
                try
                {
                    var uri = r.GetValueOrDefault<Uri>();
                    if (uri is null) return $"{r.Symbol.Name} value is null";
                    if (!System.Uri.IsWellFormedUriString(uri.OriginalString, kind)) return $"{r.Symbol.Name} value is not a well formed URI";
                    if (httpOnly && ((uri.Scheme.ToLower() != "http") && (uri.Scheme.ToLower() != "https"))) return $"{r.Symbol.Name} schema value is not valid";
                    return null;
                }
                catch (InvalidOperationException)
                {
                    return $"{r.Symbol.Name} value is not a valid URI";
                }
            });

            return option;
        }
    }
}
