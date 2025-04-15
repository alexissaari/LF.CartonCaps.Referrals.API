using LF.CartonCaps.Referrals.API.Models;
using LF.CartonCaps.Referrals.API.Models.Abstractions;
using LF.CartonCaps.Referrals.API.Proxies;

namespace LF.CartonCaps.Referrals.API.Services
{
    public class ReferralsService : IReferralsService
    {
        private IDatabaseProxy databaseProxy;

        public ReferralsService(IDatabaseProxy databaseProxy) 
        {
            this.databaseProxy = databaseProxy;
        }

        public IList<Referral>? GetReferrals(string userId) => this.databaseProxy.GetReferrals(userId);

        public void UpdateReferralStatus(string userId, string referralId, ReferralStatus referralStatus) 
            => this.databaseProxy.UpdateReferralStatus(userId, referralId, referralStatus);

        public string InviteFriend(string userId, string firstName, string lastName)
            => this.databaseProxy.InviteFriend(userId, firstName, lastName);
    }
}
