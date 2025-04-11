using LF.CartonCaps.Referrals.API.Models;
using LF.CartonCaps.Referrals.API.Services;
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

        [HttpGet(Name = MyRoutes.GetMyReferrals)]
        public ActionResult<List<string>> GetMyFriends() => ReferralsService.GetMyReferrals();
    }
}
