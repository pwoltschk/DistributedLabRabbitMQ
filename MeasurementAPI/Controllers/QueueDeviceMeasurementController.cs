using System;
using System.Net;
using System.Web.Http;
using Measurements.Models;

namespace Measurements.Controllers
{
    public class QueueDeviceMeasurementController : ApiController
    {       
        [HttpPost]
        public IHttpActionResult MakeMeasurement([FromBody] DeviceMeasurement measurement)
        {
            
        }
    }
}
