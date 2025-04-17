namespace LF.CartonCaps.Referrals.API.Models.Exceptions
{
    public class ActiveReferralDoesNotExistException : Exception
    {
        public string ReferralId { get; }

        public ActiveReferralDoesNotExistException(string message, string referralId)
        : base(message)
        {
            ReferralId = referralId;
        }
    }
}
