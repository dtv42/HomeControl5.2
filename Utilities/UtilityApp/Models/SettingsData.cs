﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Testdata.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>10-12-2020 16:36</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityApp.Models
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    #endregion Using Directives

    /// <summary>
    ///  Complex data type example.
    /// </summary>
    public class ComplexData
    {
        public string StringValue { get; set; } = string.Empty;
        public bool BooleanValue { get; set; }
        public int IntegerValue { get; set; }
        public long LongValue { get; set; }
        public float FloatValue { get; set; }
        public double DoubleValue { get; set; }
        public decimal DecimalValue { get; set; }
    }

    /// <summary>
    ///  Settings data type with a selection of supported data types.
    ///  The "appsettings.json" JSON file is used to initialize the fields.
    ///
    ///  JSON represents six basic data types:
    ///
    ///      Primitive Types
    ///          String
    ///          Number
    ///          Boolean
    ///          Null
    ///      Structure Type
    ///          Object
    ///          Array
    ///
    ///  The data types are mapped to configuration data and binding sets the property values.
    ///  Default binding only works with public properties using { get; set; }.
    ///  Collections and GenericTypes are supported (arrays and lists).
    ///  The actual conversion is done using type converters (TypeDescriptor.GetConverter())
    ///  and converts the given string to the type of the converter, using the invariant culture.
    /// </summary>
    public class SettingsData
    {
        public string StringValue { get; set; } = string.Empty;
        public bool BooleanValue { get; set; }
        public int IntegerValue { get; set; }
        public long LongValue { get; set; }
        public float FloatValue { get; set; }
        public double DoubleValue { get; set; }
        public decimal DecimalValue { get; set; }
        public DateTime DateTimeValue { get; set; } = DateTime.Now;
        public DateTimeOffset DateTimeOffsetValue { get; set; } = new DateTimeOffset(DateTime.Now);
        public string[] StringArray { get; set; } = Array.Empty<string>();
        public bool[] BooleanArray { get; set; } = Array.Empty<bool>();
        public int[] IntegerArray { get; set; } = Array.Empty<int>();
        public long[] LongArray { get; set; } = Array.Empty<long>();
        public float[] FloatArray { get; set; } = Array.Empty<float>();
        public double[] DoubleArray { get; set; } = Array.Empty<double>();
        public decimal[] DecimalArray { get; set; } = Array.Empty<decimal>();
        public DateTime[] DateTimeArray { get; set; } = Array.Empty<DateTime>();
        public DateTimeOffset[] DateTimeOffsetArray { get; set; } = Array.Empty<DateTimeOffset>();
        public List<string> StringList { get; set; } = new List<string> { };
        public List<bool> BooleanList { get; set; } = new List<bool> { };
        public List<int> IntegerList { get; set; } = new List<int> { };
        public List<long> LongList { get; set; } = new List<long> { };
        public List<float> FloatList { get; set; } = new List<float> { };
        public List<double> DoubleList { get; set; } = new List<double> { };
        public List<decimal> DecimalList { get; set; } = new List<decimal> { };
        public List<DateTime> DateTimeList { get; set; } = new List<DateTime> { };
        public List<DateTimeOffset> DateTimeOffsetList { get; set; } = new List<DateTimeOffset> { };
        public Dictionary<string, string> Dictionary { get; set; } = new Dictionary<string, string> { };
        public ComplexData Settings { get; set; } = new ComplexData();
    }
}