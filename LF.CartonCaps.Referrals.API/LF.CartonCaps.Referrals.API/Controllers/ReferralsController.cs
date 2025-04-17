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

            if (result == null)
            {
                return NoContent();
            }

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
        [Route("InviteFriend/{userId}/{referralFirstName}/{referralLastName}")]
        public ActionResult InviteFriend(string userId, string referralFirstName, string referralLastName)
        {
            var newReferralId = this.referralsService.InviteFriend(userId, referralFirstName, referralLastName);

            if (string.IsNullOrWhiteSpace(newReferralId))
            {
                return BadRequest($"Failed to invite friend {referralFirstName} {referralLastName} to user {userId}");
            }

            var uri = Url.Action(nameof(GetReferrals), new { userId = userId });
            return Created(uri, newReferralId);
        }
    }
}
