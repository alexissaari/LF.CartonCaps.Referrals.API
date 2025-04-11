using Microsoft.AspNetCore.Mvc;

namespace LF.CartonCaps.Referrals.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReferralsController : ControllerBase
    {
        private readonly ILogger<ReferralsController> _logger;

        public ReferralsController(ILogger<ReferralsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetMyReferrals")]
        public string Get()
        {
            return "placeholder";
        }
    }
}
