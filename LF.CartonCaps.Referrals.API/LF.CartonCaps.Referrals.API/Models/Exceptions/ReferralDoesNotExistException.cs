namespace LF.CartonCaps.Referrals.API.Models.Exceptions
{
    public class ReferralDoesNotExistException : Exception
    {
        public int UserId { get; }
        public int ReferralId { get; }

        public ReferralDoesNotExistException(string message, int userId, int referralId)
        : base(message)
        {
            UserId = userId;
            ReferralId = referralId;
        }
    }
}
