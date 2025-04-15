namespace LF.CartonCaps.Referrals.API.Models.Exceptions
{
    public class UserDoesNotExistException : Exception
    {
        public string UserId { get; }

        public UserDoesNotExistException(string message, string userId)
        : base(message)
        {
            UserId = userId;
        }
    }
}
