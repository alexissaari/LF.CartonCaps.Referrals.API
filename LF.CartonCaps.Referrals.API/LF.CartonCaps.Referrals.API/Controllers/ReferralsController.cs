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
        [EndpointSummary("Get all the Referrals this User has sent to other people.")]
        public ActionResult<List<Referral>> GetReferrals(string userId)
        {
            var result = this.referralsService.GetReferrals(userId);

            if (result == null)
            {
                return NoContent();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("IsReferral/{referralId}")]
        [EndpointSummary("Returns if this referralId is in our collection of ActiveReferrals")]
        public ActionResult GetIsReferral(string referralId)
        {
            return Ok(this.referralsService.IsReferral(referralId));
        }

        [HttpPatch]
        [Route("ReferralStatus/{referralId}/{referralStatus}")]
        [EndpointSummary("Update Referral's ReferralStatus.")]
        public ActionResult PatchReferral(string referralId, ReferralStatus referralStatus)
        {
            var success = this.referralsService.UpdateReferralStatus(referralId, referralStatus);

            return success ? Ok() : BadRequest("Failed to update referral status.");
        }

        [HttpPost]
        [Route("InviteFriend/{userId}/{referralFirstName}/{referralLastName}")]
        [EndpointSummary("Refer a friend to join CartonCaps! A User's Referrals are unique by FirstName and LastName.")]
        public ActionResult InviteFriend(string userId, string referralFirstName, string referralLastName)
        {
            var newReferralId = this.referralsService.InviteFriend(userId, referralFirstName, referralLastName);

            if (string.IsNullOrWhiteSpace(newReferralId))
            {
                return BadRequest($"Failed to invite friend {referralFirstName} {referralLastName} to user {userId}.");
            }

            var uri = Url.Action(nameof(GetReferrals), new { userId = userId });
            return Created(uri, newReferralId);
        }
    }
}
