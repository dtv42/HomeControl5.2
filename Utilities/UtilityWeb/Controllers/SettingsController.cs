// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsController.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>14-12-2020 15:59</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityWeb.Controllers
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using UtilityLib.Webapp;
    using UtilityWeb.Models;

    #endregion Using Directives

    /// <summary>
    ///  AppSettings controller providing various GET operations.
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class SettingsController : BaseController
    {
        private readonly AppSettings _settings = new AppSettings();

        /// <summary>
        ///  Initializes a new instance of the <see cref="SettingsController"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public SettingsController(IConfiguration configuration, ILogger<SettingsController> logger)
             : base(configuration, logger)
        {
            _logger.LogDebug("SettingsController()");

            configuration.GetSection("AppSettings").Bind(_settings);
        }

        [HttpGet]
        [ActionName("")]
        [Produces("application/json")]
        public IActionResult Get()
        {
            return Ok(_settings);
        }

        [HttpGet]
        [ActionName("String")]
        [Produces("application/json")]
        public IActionResult GetStringValue()
        {
            return Ok(_settings.Data.StringValue);
        }

        [HttpGet]
        [ActionName("Boolean")]
        [Produces("application/json")]
        public IActionResult GetBooleanValue()
        {
            return Ok(_settings.Data.BooleanValue);
        }

        [HttpGet]
        [ActionName("Integer")]
        [Produces("application/json")]
        public IActionResult GetIntegerValue()
        {
            return Ok(_settings.Data.IntegerValue);
        }

        [HttpGet]
        [ActionName("Long")]
        [Produces("application/json")]
        public IActionResult GetLongValue()
        {
            return Ok(_settings.Data.LongValue);
        }

        [HttpGet]
        [ActionName("Float")]
        [Produces("application/json")]
        public IActionResult GetFloatValue()
        {
            return Ok(_settings.Data.FloatValue);
        }

        [HttpGet]
        [ActionName("Double")]
        [Produces("application/json")]
        public IActionResult GetDoubleValue()
        {
            return Ok(_settings.Data.DoubleValue);
        }

        [HttpGet]
        [ActionName("Decimal")]
        [Produces("application/json")]
        public IActionResult GetDecimalValue()
        {
            return Ok(_settings.Data.DecimalValue);
        }

        [HttpGet]
        [ActionName("DateTime")]
        [Produces("application/json")]
        public IActionResult GetDateTimeValue()
        {
            return Ok(_settings.Data.DateTimeValue);
        }

        [HttpGet]
        [ActionName("DateTimeOffset")]
        [Produces("application/json")]
        public IActionResult GetDateTimeOffsetValue()
        {
            return Ok(_settings.Data.DateTimeOffsetValue);
        }

        [HttpGet]
        [ActionName("StringList")]
        [Produces("application/json")]
        public IActionResult GetStringList()
        {
            return Ok(_settings.Data.StringList);
        }

        [HttpGet]
        [ActionName("BooleanList")]
        [Produces("application/json")]
        public IActionResult GetBooleanList()
        {
            return Ok(_settings.Data.BooleanList);
        }

        [HttpGet]
        [ActionName("IntegerList")]
        [Produces("application/json")]
        public IActionResult GetIntegerList()
        {
            return Ok(_settings.Data.IntegerList);
        }

        [HttpGet]
        [ActionName("LongList")]
        [Produces("application/json")]
        public IActionResult GetLongList()
        {
            return Ok(_settings.Data.LongList);
        }

        [HttpGet]
        [ActionName("FloatList")]
        [Produces("application/json")]
        public IActionResult GetFloatList()
        {
            return Ok(_settings.Data.FloatList);
        }

        [HttpGet]
        [ActionName("DoubleList")]
        [Produces("application/json")]
        public IActionResult GetDoubleList()
        {
            return Ok(_settings.Data.DoubleList);
        }

        [HttpGet]
        [ActionName("DecimalList")]
        [Produces("application/json")]
        public IActionResult GetDecimalList()
        {
            return Ok(_settings.Data.DecimalList);
        }

        [HttpGet]
        [ActionName("DateTimeList")]
        [Produces("application/json")]
        public IActionResult GetDateTimeList()
        {
            return Ok(_settings.Data.DateTimeList);
        }

        [HttpGet]
        [ActionName("DateTimeOffsetList")]
        [Produces("application/json")]
        public IActionResult GetDateTimeOffsetList()
        {
            return Ok(_settings.Data.DateTimeOffsetList);
        }

        [HttpGet("{i}")]
        [ActionName("StringList")]
        [Produces("application/json")]
        public IActionResult GetStringList(ushort i)
        {
            if (i < _settings.Data.StringList.Count)
                return Ok(_settings.Data.StringList[i]);
            else
                return NotFound();
        }

        [HttpGet("{i}")]
        [ActionName("BooleanList")]
        [Produces("application/json")]
        public IActionResult GetBooleanList(ushort i)
        {
            if (i < _settings.Data.BooleanList.Count)
                return Ok(_settings.Data.BooleanList[i]);
            else
                return NotFound();
        }

        [HttpGet("{i}")]
        [ActionName("IntegerList")]
        [Produces("application/json")]
        public IActionResult GetIntegerList(ushort i)
        {
            if (i < _settings.Data.IntegerList.Count)
                return Ok(_settings.Data.IntegerList[i]);
            else
                return NotFound();
        }

        [HttpGet("{i}")]
        [ActionName("LongList")]
        [Produces("application/json")]
        public IActionResult GetLongList(ushort i)
        {
            if (i < _settings.Data.LongList.Count)
                return Ok(_settings.Data.LongList[i]);
            else
                return NotFound();
        }

        [HttpGet("{i}")]
        [ActionName("FloatList")]
        [Produces("application/json")]
        public IActionResult GetFloatList(ushort i)
        {
            if (i < _settings.Data.FloatList.Count)
                return Ok(_settings.Data.FloatList[i]);
            else
                return NotFound();
        }

        [HttpGet("{i}")]
        [ActionName("DoubleList")]
        [Produces("application/json")]
        public IActionResult GetDoubleList(ushort i)
        {
            if (i < _settings.Data.DoubleList.Count)
                return Ok(_settings.Data.DoubleList[i]);
            else
                return NotFound();
        }

        [HttpGet("{i}")]
        [ActionName("DecimalList")]
        [Produces("application/json")]
        public IActionResult GetDecimalList(ushort i)
        {
            if (i < _settings.Data.DecimalList.Count)
                return Ok(_settings.Data.DecimalList[i]);
            else
                return NotFound();
        }

        [HttpGet("{i}")]
        [ActionName("DateTimeList")]
        [Produces("application/json")]
        public IActionResult GetDateTimeList(ushort i)
        {
            if (i < _settings.Data.DateTimeList.Count)
                return Ok(_settings.Data.DateTimeList[i]);
            else
                return NotFound();
        }

        [HttpGet("{i}")]
        [ActionName("DateTimeOffsetList")]
        [Produces("application/json")]
        public IActionResult GetDateTimeOffsetList(ushort i)
        {
            if (i < _settings.Data.DateTimeOffsetList.Count)
                return Ok(_settings.Data.DateTimeOffsetList[i]);
            else
                return NotFound();
        }

        [HttpGet]
        [ActionName("Dictionary")]
        [Produces("application/json")]
        public IActionResult GetDictionary()
        {
            return Ok(_settings.Data.Dictionary);
        }

        [HttpGet("{i}")]
        [ActionName("Dictionary")]
        [Produces("application/json")]
        public IActionResult GetDictionary(ushort i)
        {
            if (i < _settings.Data.Dictionary.Count)
                return Ok(new KeyValuePair<string, string>
                (_settings.Data.Dictionary.Keys.ToArray()[i],
                 _settings.Data.Dictionary.Values.ToArray()[i]));
            else
                return NotFound();
        }

        [HttpGet]
        [ActionName("Settings")]
        [Produces("application/json")]
        public IActionResult GetSettings()
        {
            return Ok(_settings.Data.Settings);
        }
    }
}