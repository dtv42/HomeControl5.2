// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateOptions.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>10-12-2020 17:09</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityApp.Options
{
    /// <summary>
    ///  A collection of options for the validate command.
    /// </summary>
    public class ValidateOptions
    {
        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="ValidateOptions"/> class.
        /// </summary>
        public ValidateOptions() { }

        /// <summary>
        ///  Initializes a new instance of the <see cref="ValidateOptions"/> class
        ///  mapping the input parameters to the corresponding properties.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="v"></param>
        /// <param name="o"></param>
        /// <param name="n"></param>
        /// <param name="r"></param>
        /// <param name="l"></param>
        /// <param name="s"></param>
        /// <param name="m"></param>
        /// <param name="e"></param>
        /// <param name="w"></param>
        /// <param name="x"></param>
        /// <param name="g"></param>
        /// <param name="a"></param>
        /// <param name="p"></param>
        /// <param name="u"></param>
        public ValidateOptions(int d, int v, int o, int n, int r, long l,
                               string s, string m, string e, string w, string x, string g, string a, string p, string u)
        {
            IntegerD = d;
            IntegerV = v;
            IntegerO = o;
            IntegerN = n;
            IntegerR = r;
            IntegerL = l;

            StringS = s;
            StringM = m;
            StringE = e;
            StringW = w;
            StringX = x;
            StringG = g;
            StringA = a;
            StringP = p;
            StringU = u;
        }

        #endregion

        public int IntegerD { get; set; }
        public int IntegerV { get; set; }
        public int IntegerO { get; set; }
        public int IntegerN { get; set; }
        public int IntegerR { get; set; }
        public long IntegerL { get; set; }

        public string StringS { get; set; } = string.Empty;
        public string StringM { get; set; } = string.Empty;
        public string StringE { get; set; } = string.Empty;
        public string StringW { get; set; } = string.Empty;
        public string StringX { get; set; } = string.Empty;
        public string StringG { get; set; } = string.Empty;
        public string StringA { get; set; } = string.Empty;
        public string StringP { get; set; } = string.Empty;
        public string StringU { get; set; } = string.Empty;
    }
}
