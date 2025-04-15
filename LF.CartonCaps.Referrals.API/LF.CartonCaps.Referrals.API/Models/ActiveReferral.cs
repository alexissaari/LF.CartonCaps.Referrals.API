namespace LF.CartonCaps.Referrals.API.Models
{
    public class ActiveReferral
    {
        public required string OriginatingReferralUserId { get; set; }
        public required ReferralStatus ReferralStatus { get; set; }
    }
}
