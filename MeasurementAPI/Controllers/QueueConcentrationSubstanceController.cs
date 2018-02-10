using System;
using System.Net;
using System.Web.Http;
using Measurements.Models;

namespace Measurements.Controllers
{
    public class QueueConcentrationSubstanceController : ApiController
    {       
        [HttpPost]
        public IHttpActionResult MakeMeasurement([FromBody] ConcentrationSubstance concentrationSubstance)
        {

        }
    }
}
