// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestDataController.cs" company="DTV-Online">
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

    using System.Net;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityWeb.Models;

    #endregion Using Directives

    /// <summary>
    ///  TestData controller providing GET and POST operations.
    ///  Note that TryValidateModel() only validates the top level object.
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestDataController : BaseController
    {
        private TestData _testdata = new TestData();

        /// <summary>
        ///  Initializes a new instance of the <see cref="TestDataController"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public TestDataController(IConfiguration configuration, ILogger<TestDataController> logger)
            : base(configuration, logger)
        {
            _logger.LogDebug("TestDataController()");

            configuration.GetSection("TestData").Bind(_testdata);
        }

        [HttpGet]
        [ActionName("")]
        [Produces("application/json")]
        public IActionResult Get()
        {
            return Ok(_testdata);
        }

        [HttpGet]
        [ActionName("Value")]
        [Produces("application/json")]
        public IActionResult GetValue()
        {
            return Ok(_testdata.Value);
        }

        [HttpGet]
        [ActionName("Name")]
        [Produces("application/json")]
        public IActionResult GetName()
        {
            return Ok(_testdata.Name);
        }

        [HttpGet]
        [ActionName("Guid")]
        [Produces("application/json")]
        public IActionResult GetUuid()
        {
            return Ok(_testdata.Guid);
        }

        [HttpGet]
        [ActionName("Address")]
        [Produces("application/json")]
        public IActionResult GetAddress()
        {
            return Ok(_testdata.Address);
        }

        [HttpGet]
        [ActionName("Endpoint")]
        [Produces("application/json")]
        public IActionResult GetEndpoint()
        {
            return Ok(_testdata.Endpoint);
        }

        [HttpGet]
        [ActionName("Uri")]
        [Produces("application/json")]
        public IActionResult GetUri()
        {
            return Ok(_testdata.Uri);
        }

        [HttpGet]
        [ActionName("Code")]
        [Produces("application/json")]
        public IActionResult GetCode()
        {
            return Ok(_testdata.Code);
        }

        [HttpPost]
        [ActionName("")]
        [Produces("application/json")]
        public IActionResult Post([FromBody] TestData testdata)
        {
            if (ModelState.IsValid)
            {
                _testdata = testdata;
                return Accepted(_testdata);
            }
            else
                return NotAcceptable();
        }

        [HttpPost]
        [ActionName("Value")]
        [Produces("application/json")]
        public IActionResult PostValue([FromBody] int value)
        {
            if (TryValidateModel(new TestData() { Value = value }))
            {
                _testdata.Value = value;
                return Accepted(_testdata);
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [ActionName("Name")]
        [Produces("application/json")]
        public IActionResult PostName([FromBody] string name)
        {
            if (TryValidateModel(new TestData() { Name = name }))
            {
                _testdata.Name = name;
                return Accepted(_testdata);
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [ActionName("Uuid")]
        [Produces("application/json")]
        public IActionResult PostUuid([FromBody] string guid)
        {
            if (TryValidateModel(new TestData() { Guid = guid }))
            {
                _testdata.Guid = guid;
                return Accepted(_testdata);
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [ActionName("Address")]
        [Produces("application/json")]
        public IActionResult PostAddress([FromBody] string address)
        {
            if (TryValidateModel(new TestData() { Address = address }))
            {
                _testdata.Address = address;
                return Accepted(_testdata);
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [ActionName("Endpoint")]
        [Produces("application/json")]
        public IActionResult PostEndpoint([FromBody] string endpoint)
        {
            if (TryValidateModel(new TestData() { Endpoint = endpoint }))
            {
                _testdata.Endpoint = endpoint;
                return Accepted(_testdata);
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [ActionName("Uri")]
        [Produces("application/json")]
        public IActionResult PostUri([FromBody] string uri)
        {
            if (TryValidateModel(new TestData() { Uri = uri }))
            {
                _testdata.Uri = uri;
                return Accepted(_testdata);
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [ActionName("Code")]
        [Produces("application/json")]
        public IActionResult PostCode([FromBody] HttpStatusCode code)
        {
            _testdata.Code = code;
            return Accepted(_testdata);
        }
    }
}