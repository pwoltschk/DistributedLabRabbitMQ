using System;
using System.Net;
using System.Web.Http;
using Measurements.Models;

namespace Measurements.Controllers
{
    public class DirectDeviceMeasurementController : ApiController
    {       
[HttpPost]
public IHttpActionResult MakeMeasurement([FromBody] DeviceMeasurement measurement)
{

}
    }
}
