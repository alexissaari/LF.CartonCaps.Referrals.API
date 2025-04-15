using LF.CartonCaps.Referrals.API.Models;
using LF.CartonCaps.Referrals.API.Proxies;

namespace LF.CartonCaps.Referrals.API.Services
{
    public class ReferralsService
    {
        private DatabaseProxy _proxy = new DatabaseProxy();

        public IList<Referral>? GetReferrals(string userId) => _proxy.GetReferrals(userId);

        public void UpdateReferralStatus(string userId, string referralId, ReferralStatus referralStatus) 
            => _proxy.UpdateReferralStatus(userId, referralId, referralStatus);

        public string InviteFriend(string userId, string firstName, string lastName)
            => _proxy.InviteFriend(userId, firstName, lastName);
    }
}
