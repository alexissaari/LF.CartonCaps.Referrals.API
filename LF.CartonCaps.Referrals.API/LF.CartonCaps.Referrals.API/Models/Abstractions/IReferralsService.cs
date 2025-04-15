namespace LF.CartonCaps.Referrals.API.Models.Abstractions
{
    public interface IReferralsService
    {
        public IList<Referral>? GetReferrals(string userId);
        public void UpdateReferralStatus(string userId, string referralId, ReferralStatus referralStatus);
        public string InviteFriend(string userId, string firstName, string lastName);
    }
}
