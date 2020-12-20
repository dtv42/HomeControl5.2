// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionsExtensions.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>15-12-2020 07:47</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityLib.Console
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
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

        public static Option<char> FromAmong(this Option<char> option, string values)
        {
            List<char> chars = new List<char>();
            chars.AddRange(values);
            option.FromAmong(chars.Select(v => v.ToString()).ToArray());
            return option;
        }

        public static Option<byte> FromAmong(this Option<byte> option, params byte[] values)
        {
            option.FromAmong(values.ToList().Select(v => v.ToString()).ToArray());
            return option;
        }

        public static Option<short> FromAmong(this Option<short> option, params short[] values)
        {
            option.FromAmong(values.ToList().Select(v => v.ToString()).ToArray());
            return option;
        }

        public static Option<ushort> FromAmong(this Option<ushort> option, params ushort[] values)
        {
            option.FromAmong(values.ToList().Select(v => v.ToString()).ToArray());
            return option;
        }

        public static Option<int> FromAmong(this Option<int> option, params int[] values)
        {
            option.FromAmong(values.ToList().Select(v => v.ToString()).ToArray());
            return option;
        }

        public static Option<uint> FromAmong(this Option<uint> option, params uint[] values)
        {
            option.FromAmong(values.ToList().Select(v => v.ToString()).ToArray());
            return option;
        }

        public static Option<long> FromAmong(this Option<long> option, params long[] values)
        {
            option.FromAmong(values.ToList().Select(v => v.ToString()).ToArray());
            return option;
        }

        public static Option<ulong> FromAmong(this Option<ulong> option, params ulong[] values)
        {
            option.FromAmong(values.ToList().Select(v => v.ToString()).ToArray());
            return option;
        }

        public static Option<float> Range(this Option<float> option, float minimum, float maximum)
        {
            if (minimum > maximum) throw new ArgumentException("The minimum value has to be smaller than the maximum value.");

            option.Argument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<float>();
                if ((value >= minimum) && (value <= maximum)) return null;
                return $"{r.Symbol.Name} value must be between {minimum} and {maximum} (incl.)";
            });

            return option;
        }

        public static Option<double> Range(this Option<double> option, double minimum, double maximum)
        {
            if (minimum > maximum) throw new ArgumentException("The minimum value has to be smaller than the maximum value.");

            option.Argument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<double>();
                if ((value >= minimum) && (value <= maximum)) return null;
                return $"{r.Symbol.Name} value must be between {minimum} and {maximum} (incl.)";
            });

            return option;
        }

        public static Option<byte> Range(this Option<byte> option, byte minimum, byte maximum)
        {
            if (minimum > maximum) throw new ArgumentException("The minimum value has to be smaller than the maximum value.");

            option.Argument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<byte>();
                if ((value >= minimum) && (value <= maximum)) return null;
                return $"{r.Symbol.Name} value must be between {minimum} and {maximum} (incl.)";
            });

            return option;
        }

        public static Option<short> Range(this Option<short> option, short minimum, short maximum)
        {
            if (minimum > maximum) throw new ArgumentException("The minimum value has to be smaller than the maximum value.");

            option.Argument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<short>();
                if ((value >= minimum) && (value <= maximum)) return null;
                return $"{r.Symbol.Name} value must be between {minimum} and {maximum} (incl.)";
            });

            return option;
        }

        public static Option<ushort> Range(this Option<ushort> option, ushort minimum, ushort maximum)
        {
            if (minimum > maximum) throw new ArgumentException("The minimum value has to be smaller than the maximum value.");

            option.Argument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<ushort>();
                if ((value >= minimum) && (value <= maximum)) return null;
                return $"{r.Symbol.Name} value must be between {minimum} and {maximum} (incl.)";
            });

            return option;
        }

        public static Option<int> Range(this Option<int> option, int minimum, int maximum)
        {
            if (minimum > maximum) throw new ArgumentException("The minimum value has to be smaller than the maximum value.");

            option.Argument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<int>();
                if ((value >= minimum) && (value <= maximum)) return null;
                return $"{r.Symbol.Name} value must be between {minimum} and {maximum} (incl.)";
            });

            return option;
        }

        public static Option<uint> Range(this Option<uint> option, uint minimum, uint maximum)
        {
            if (minimum > maximum) throw new ArgumentException("The minimum value has to be smaller than the maximum value.");

            option.Argument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<uint>();
                if ((value >= minimum) && (value <= maximum)) return null;
                return $"{r.Symbol.Name} value must be between {minimum} and {maximum} (incl.)";
            });

            return option;
        }

        public static Option<long> Range(this Option<long> option, long minimum, long maximum)
        {
            if (minimum > maximum) throw new ArgumentException("The minimum value has to be smaller than the maximum value.");

            option.Argument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<long>();
                if ((value >= minimum) && (value <= maximum)) return null;
                return $"{r.Symbol.Name} value must be between {minimum} and {maximum} (incl.)";
            });

            return option;
        }

        public static Option<ulong> Range(this Option<ulong> option, ulong minimum, ulong maximum)
        {
            if (minimum > maximum) throw new ArgumentException("The minimum value has to be smaller than the maximum value.");

            option.Argument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<ulong>();
                if ((value >= minimum) && (value <= maximum)) return null;
                return $"{r.Symbol.Name} value must be between {minimum} and {maximum} (incl.)";
            });

            return option;
        }

        public static Option<string> StringLength(this Option<string> option, int length)
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

        public static Option<string> NotEmpty(this Option<string> option)
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

        public static Option<string> NotWhiteSpace(this Option<string> option)
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

        public static Option<string> Regex(this Option<string> option, string pattern)
        {
            option.Argument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<string>();
                if (value is null) return $"{r.Symbol.Name} value is null";
                if (pattern is null) return "Pattern value is null";
            try
                {
                    var regex = new Regex(pattern);
                    if (regex.IsMatch(value)) return null;
                    return $"{r.Symbol.Name} value must match the regular expression: {pattern}";
                }
                catch(ArgumentException aex)
                {
                    return $"Pattern value invalid: {aex.Message}";
                }
            });

            return option;
        }

        public static Option<string> Guid(this Option<string> option)
        {
            option.Argument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<string>();
                if (value is null) return $"{r.Symbol.Name} value is null";
                if (System.Guid.TryParse(value, out _)) return null;
                return $"{r.Symbol.Name} value is not a valid GUID";
            });

            return option;
        }

        public static Option<string> IPAddress(this Option<string> option)
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

        public static Option<string> IPEndpoint(this Option<string> option)
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

        public static Option Uri(this Option<string> option, UriKind kind = UriKind.Absolute, bool httpOnly = true)
        {
            option.Argument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<string>();
                if (value is null) return $"{r.Symbol.Name} value is null";
                if (!System.Uri.TryCreate(value, kind, out System.Uri? uri)) return $"{r.Symbol.Name} value is not a valid URI";
                if (httpOnly && ((uri.Scheme.ToLower() != "http") && (uri.Scheme.ToLower() != "https"))) return $"{r.Symbol.Name} schema value is not valid";
                return null;
            });

            return option;
        }
    }
}
