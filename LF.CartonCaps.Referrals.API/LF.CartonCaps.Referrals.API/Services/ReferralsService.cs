using LF.CartonCaps.Referrals.API.Models;
using LF.CartonCaps.Referrals.API.Proxies;

namespace LF.CartonCaps.Referrals.API.Services
{
    public class ReferralsService
    {
        public IList<Referral>? GetReferrals(string userId) => MochDatabaseProxy.GetReferrals(userId);

        public void PatchReferral(string userId, string referralId, ReferralStatus referralStatus) 
            => MochDatabaseProxy.PatchReferral(userId, referralId, referralStatus);
    }
}
