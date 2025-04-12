namespace LF.CartonCaps.Referrals.API.Models.Exceptions
{
    public class UserDoesNotExistException : Exception
    {
        public int UserId { get; }

        public UserDoesNotExistException(string message, int userId)
        : base(message)
        {
            UserId = userId;
        }
    }
}
