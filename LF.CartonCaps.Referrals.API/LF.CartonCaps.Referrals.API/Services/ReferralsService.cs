using LF.CartonCaps.Referrals.API.Models;

namespace LF.CartonCaps.Referrals.API.Services
{
    // Services are where we handle our business logic.
    // The logic done here is the same regardless of what data we're preforming logic rules on,
    // so let's make this static, because we don't need multiple instances of it.
    public static class ReferralsService
    {
        static List<Referral> MyReferrals { get; }

        static ReferralsService()
        {
            MyReferrals = [
                new Referral(){ FirstName = "Alexis", LastName = "Saari", ReferralStatus = ReferralStatus.Complete },
                new Referral(){ FirstName = "Jane", LastName = "Doe", ReferralStatus = ReferralStatus.Sent }
                ];
        }

        public static List<Referral> GetMyReferrals() => MyReferrals;
    }
}
