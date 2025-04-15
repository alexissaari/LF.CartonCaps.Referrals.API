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
        public ActionResult<List<Referral>> GetReferrals(string userId)
        {
            return Ok(this.referralsService.GetReferrals(userId));
        }

        [HttpPatch]
        [Route("ReferralStatus/{userId}/{referralId}/{referralStatus}")]
        public ActionResult PatchReferral(string userId, string referralId, ReferralStatus referralStatus)
        {
            this.referralsService.UpdateReferralStatus(userId, referralId, referralStatus);

            return Ok();
        }

        [HttpGet]
        [Route("InviteFriend/{userId}/{firstName}/{lastName}")]
        public ActionResult InviteFriend(string userId, string firstName, string lastName)
        {
            return Ok(this.referralsService.InviteFriend(userId, firstName, lastName));
        }
    }
}
