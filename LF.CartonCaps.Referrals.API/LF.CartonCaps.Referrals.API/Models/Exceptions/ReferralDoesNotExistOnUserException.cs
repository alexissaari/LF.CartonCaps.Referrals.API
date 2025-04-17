namespace LF.CartonCaps.Referrals.API.Models.Exceptions
{
    public class ReferralDoesNotExistOnUserException : Exception
    {
        public string UserId { get; }
        public string ReferralId { get; }

        public ReferralDoesNotExistOnUserException(string message, string userId, string referralId)
        : base(message)
        {
            UserId = userId;
            ReferralId = referralId;
        }
    }
}
