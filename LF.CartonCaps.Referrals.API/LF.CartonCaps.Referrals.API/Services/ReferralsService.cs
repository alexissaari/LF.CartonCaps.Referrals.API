using LF.CartonCaps.Referrals.API.Models;
using LF.CartonCaps.Referrals.API.Proxies;

namespace LF.CartonCaps.Referrals.API.Services
{
    public class ReferralsService
    {
        public IList<Referral>? GetReferrals(string userId) => DatabaseProxy.GetReferrals(userId);

        public void UpdateReferralStatus(string userId, string referralId, ReferralStatus referralStatus) 
            => DatabaseProxy.UpdateReferralStatus(userId, referralId, referralStatus);

        public string InviteFriend(string userId, string firstName, string lastName)
            => DatabaseProxy.InviteFriend(userId, firstName, lastName);
    }
}
