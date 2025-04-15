namespace LF.CartonCaps.Referrals.API.Models.Exceptions
{
    public class ReferralDoesNotExistException : Exception
    {
        public string ReferralId { get; }
        public string? UserId { get; }

        public ReferralDoesNotExistException(string message, string referralId)
        : base(message)
        {
            ReferralId = referralId;
        }

        public ReferralDoesNotExistException(string message, string referralId, string userId)
        : base(message)
        {
            ReferralId = referralId;
            UserId = userId;
        }
    }
}
