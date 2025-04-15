using LF.CartonCaps.Referrals.API.Models;
using LF.CartonCaps.Referrals.API.Proxies;

namespace LF.CartonCaps.Referrals.API.Services
{
    public class ReferralsService
    {
        private DatabaseProxy databaseProxy;

        public ReferralsService() 
        {
            this.databaseProxy = new DatabaseProxy();
        }

        public IList<Referral>? GetReferrals(string userId) => this.databaseProxy.GetReferrals(userId);

        public void UpdateReferralStatus(string userId, string referralId, ReferralStatus referralStatus) 
            => this.databaseProxy.UpdateReferralStatus(userId, referralId, referralStatus);

        public string InviteFriend(string userId, string firstName, string lastName)
            => this.databaseProxy.InviteFriend(userId, firstName, lastName);
    }
}
