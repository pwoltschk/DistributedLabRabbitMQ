using System;
using System.Net;
using System.Web.Http;
using Measurements.Models;
using Measurements.RabbitMQ;

namespace Measurements.Controllers
{
    public class DirectDeviceMeasurementController : ApiController
    {       
[HttpPost]
public IHttpActionResult MakeMeasurement([FromBody] DeviceMeasurement measurement)
{
    string reply;

    try
    {
        RabbitMQDirectClient client = new RabbitMQDirectClient();
        client.CreateConnection();
        reply = client.MakeMeasurement(measurement);

        client.Close();
    }
    catch (Exception)
    {
        return StatusCode(HttpStatusCode.BadRequest);
    }

    return Ok(reply);
}
    }
}
