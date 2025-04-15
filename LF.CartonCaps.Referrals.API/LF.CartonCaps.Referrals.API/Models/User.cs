namespace LF.CartonCaps.Referrals.API.Models
{
    public class User
    {
        public required string UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string ReferralCode { get; set; }
        public IList<Referral>? Referrals { get; set; }
    }
}
