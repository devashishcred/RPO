using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Rpo.ApiServices.Api.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        [Route("api/TestApiForIss")]
        public IHttpActionResult TestApiForIss()
        {
            return Ok(new { Status = HttpStatusCode.OK });
        }

        [HttpGet]
        [Route("api/CurrentTimeTestApi")]
        public IHttpActionResult CurrentTimeTestApi()
        {
            var UTCresult = DateTime.UtcNow.ToString();
            var result = DateTime.Now.ToString();
            return Ok(result + " UTC: "+ UTCresult);
        }
    }
}
