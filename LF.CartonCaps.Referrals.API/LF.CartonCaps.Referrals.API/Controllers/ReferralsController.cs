using LF.CartonCaps.Referrals.API.Models;
using LF.CartonCaps.Referrals.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LF.CartonCaps.Referrals.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReferralsController : ControllerBase
    {
        private readonly ILogger<ReferralsController> logger;
        private readonly ReferralsService referralsService;

        public ReferralsController(ILogger<ReferralsController> logger)
        {
            this.logger = logger;
            this.referralsService = new ReferralsService();
        }

        [HttpGet]
        [Route("{userId}")]
        public ActionResult<List<Referral>> GetReferrals(int userId)
        {
            return Ok(this.referralsService.GetReferrals(userId));
        }

        [HttpPatch]
        [Route("{userId}/{referralId}/{referralStatus}")]
        public ActionResult PatchReferral(int userId, int referralId, ReferralStatus referralStatus)
        {
            this.referralsService.PatchReferral(userId, referralId, referralStatus);

            return Ok();
        }
    }
}
