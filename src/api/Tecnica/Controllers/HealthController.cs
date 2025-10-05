using Microsoft.AspNetCore.Mvc;

namespace Tecnica.Controllers
{
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [Route("api/[controller]")]
        public IActionResult Get()
        {
            var healthStatus = new
            {
                status = "UP",
                timestamp = DateTime.UtcNow
            };

            return Ok(healthStatus);
        }
    }
}
