using System;
using System.Net;
using System.Web.Http;
using Measurements.Models;
using Measurements.RabbitMQ;

namespace Measurements.Controllers
{
    public class QueueConcentrationSubstanceController : ApiController
    {       
        [HttpPost]
        public IHttpActionResult MakeMeasurement([FromBody] ConcentrationSubstance concentrationSubstance)
        {
            try
            {
                RabbitMQClient client = new RabbitMQClient();
                client.SendConcentrationSubstance(concentrationSubstance);
                client.Close();
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return Ok(concentrationSubstance);
        }
    }
}
