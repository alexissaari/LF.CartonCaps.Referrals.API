using LF.CartonCaps.Referrals.API.Models;
using LF.CartonCaps.Referrals.API.Proxies;

namespace LF.CartonCaps.Referrals.API.Services
{
    public class ReferralsService
    {
        public IList<Referral>? GetReferrals(int userId) => MochDatabaseProxy.GetReferrals(userId);

        public void PatchReferral(int userId, int referralId, ReferralStatus referralStatus) 
            => MochDatabaseProxy.PatchReferral(userId, referralId, referralStatus);
    }
}
