namespace LF.CartonCaps.Referrals.API.Models.Exceptions
{
    public class ReferralDoesNotExistException : Exception
    {
        public string UserId { get; }
        public string ReferralId { get; }

        public ReferralDoesNotExistException(string message, string userId, string referralId)
        : base(message)
        {
            UserId = userId;
            ReferralId = referralId;
        }
    }
}
