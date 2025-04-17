namespace LF.CartonCaps.Referrals.API.Models
{
    public class Referral
    {
        public required string ReferralId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public ReferralStatus ReferralStatus { get; set; }
    }
}
