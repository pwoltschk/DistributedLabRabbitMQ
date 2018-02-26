using System;
using System.Net;
using System.Web.Http;
using Measurements.Models;
using Measurements.RabbitMQ;

namespace Measurements.Controllers
{
    public class QueueDeviceMeasurementController : ApiController
    {       
        [HttpPost]
        public IHttpActionResult MakeMeasurement([FromBody] DeviceMeasurement measurement)
        {
            try
            {
                RabbitMQClient client = new RabbitMQClient();
                client.SendMeasurement(measurement);
                client.Close();
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return Ok(measurement);
        }
    }
}
