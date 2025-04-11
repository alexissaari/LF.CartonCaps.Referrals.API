namespace LF.CartonCaps.Referrals.API.Models
{
    public class User
    {
        public required int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<Referral> MyReferrals { get; set; }
    }
}
