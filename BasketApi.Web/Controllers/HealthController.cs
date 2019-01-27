using System.Web.Http;

namespace BasketApi.Controllers
{
    public class HealthController : ApiController
    {
        [HttpGet]
        public IHttpActionResult HealthCheck()
        {
            return Ok("Healthy");
        }
    }
}
