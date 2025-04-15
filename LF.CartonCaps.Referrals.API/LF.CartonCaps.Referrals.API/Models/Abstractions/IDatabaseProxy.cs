using LF.CartonCaps.Referrals.API.Models;

namespace LF.CartonCaps.Referrals.API.Models.Abstractions
{
    public interface IDatabaseProxy
    {
        public IList<Referral>? GetReferrals(string userId);
        public void UpdateReferralStatus(string userId, string referralId, ReferralStatus newReferralStatus);
        public string InviteFriend(string userId, string firstName, string lastName);
    }
}
