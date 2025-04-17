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
            var result = this.referralsService.GetReferrals(userId);

            return Ok(result);
        }

        [HttpPatch]
        [Route("ReferralStatus/{referralId}/{referralStatus}")]
        public ActionResult PatchReferral(string referralId, ReferralStatus referralStatus)
        {
            var success = this.referralsService.UpdateReferralStatus(referralId, referralStatus);

            return success ? Ok() : BadRequest("Failed to update referral status.");
        }

        [HttpPost]
        [Route("InviteFriend/{userId}/{refereeFirstName}/{refereeLastName}")]
        public ActionResult InviteFriend(string userId, string refereeFirstName, string refereeLastName)
        {
            var newReferralId = this.referralsService.InviteFriend(userId, refereeFirstName, refereeLastName);

            if (string.IsNullOrWhiteSpace(newReferralId))
            {
                return BadRequest("Failed to invite friend.");
            }

            return Ok(newReferralId);
        }
    }
}
