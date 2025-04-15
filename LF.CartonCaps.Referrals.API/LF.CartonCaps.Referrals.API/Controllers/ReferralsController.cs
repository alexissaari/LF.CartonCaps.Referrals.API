using LF.CartonCaps.Referrals.API.Models;
using LF.CartonCaps.Referrals.API.Models.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace LF.CartonCaps.Referrals.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReferralsController : ControllerBase
    {
        private readonly ILogger<ReferralsController> logger;
        private readonly IReferralsService referralsService;

        public ReferralsController(ILogger<ReferralsController> logger, IReferralsService referralsService)
        {
            this.logger = logger;
            this.referralsService = referralsService;
        }

        [HttpGet]
        [Route("{userId}")]
        public ActionResult<List<Referral>> GetReferrals(string userId)
        {
            return Ok(this.referralsService.GetReferrals(userId));
        }

        [HttpPatch]
        [Route("ReferralStatus/{referralId}/{referralStatus}")]
        public ActionResult PatchReferral(string referralId, ReferralStatus referralStatus)
        {
            this.referralsService.UpdateReferralStatus(referralId, referralStatus);

            return Ok();
        }

        [HttpGet]
        [Route("InviteFriend/{userId}/{refereeFirstName}/{refereeLastName}")]
        public ActionResult InviteFriend(string userId, string refereeFirstName, string refereeLastName)
        {
            return Ok(this.referralsService.InviteFriend(userId, refereeFirstName, refereeLastName));
        }
    }
}
