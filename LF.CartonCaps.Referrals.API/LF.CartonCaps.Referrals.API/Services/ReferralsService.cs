namespace LF.CartonCaps.Referrals.API.Services
{
    // Services are where we handle our business logic.
    // The logic done here is the same regardless of what data we're preforming logic rules on,
    // so let's make this static, because we don't need multiple instances of it.
    public static class ReferralsService
    {
        static List<string> MyReferrals { get; }

        static ReferralsService()
        {
            MyReferrals = [
                "Test",
                "Test2"
                ];
        }

        public static List<string> GetMyReferrals() => MyReferrals;
    }
}
