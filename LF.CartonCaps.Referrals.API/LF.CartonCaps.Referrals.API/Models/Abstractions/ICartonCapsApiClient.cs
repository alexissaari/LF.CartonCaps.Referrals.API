namespace LF.CartonCaps.Referrals.API.Models.Abstractions
{
    public interface ICartonCapsApiClient
    {
        public IList<Referral>? GetReferrals(string userId);
        public bool AddRefereeToUser(string userId, Referral referral);
        public bool UpdateReferralStatusForUser(string userId, string referralId, ReferralStatus referralStatus);

        public ActiveReferral? GetActiveReferral(string referralId);
        public bool AddActiveReferral(string referralId, string originatingReferralUserId);
        public bool UpdateActiveReferral(string referralId, ReferralStatus referralStatus);
        public bool RemoveActiveReferral(string referralId);
    }
}
