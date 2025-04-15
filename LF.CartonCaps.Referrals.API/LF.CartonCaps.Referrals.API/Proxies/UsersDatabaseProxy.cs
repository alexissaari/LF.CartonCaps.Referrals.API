using LF.CartonCaps.Referrals.API.Models;
using LF.CartonCaps.Referrals.API.Models.Exceptions;

namespace LF.CartonCaps.Referrals.API.Proxies
{
    /* 
     * Ideally, REST Apis don't directly handle connections to a datastore and 
     * instead rely on calling another Api that is responsible for datastore CRUD operations.
     * 
     * Since this service is not responsible for creating users or modifying non-referral attributes,
     * let's give ourselves a few Users to work with off the bat.
     * 
     * Since this service is responsible for creating and supporting referees, 
     * let's store all our active referees in a seperate datastore, for easy lookup.
     */
    public static class UsersDatabaseProxy
    {
        private static Dictionary<string, User> users;

        static UsersDatabaseProxy()
        {
            users = new Dictionary<string, User>();
            PopulateDatastore();
        }


        public static IList<Referral>? GetReferrals(string userId)
        {
            return GetUser(userId).Referrals;
        }

        public static void AddReferee(string userId, Referral referral)
        {
            if (users.ContainsKey(userId))
            {
                users[userId].Referrals?.Add(referral);
            }
        }

        public static void UpdateReferralStatus(string userId, string referralId, ReferralStatus referralStatus)
        {
            users.TryGetValue(userId, out User? user);
            var referral = user?.Referrals?.FirstOrDefault(x => x.RefereeId == referralId);
            if (referral != null)
            {
                referral.ReferralStatus = referralStatus;
            }
        }

        public static Referral? GetReferral(string userId, string firstName, string lastName)
        {
            var user = GetUser(userId);
            return user?.Referrals?.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);
        }

        private static User GetUser(string userId)
        {
            users.TryGetValue(userId, out User? user);

            if (user == null)
            {
                // Ideally, we should never have a null user,
                // as the app cannot ask for the referrals of a user without that user being logged in.
                throw new UserDoesNotExistException($"User Not Found. UserId = {userId}.", userId);
            }

            return user;
        }

        private static void PopulateDatastore()
        {
            users.Add("1111", new User() { UserId = "1111", FirstName = "First", LastName = "User1", ShareableReferralCode = "ABC123" });
            users.Add("2222", new User() { UserId = "2222", FirstName = "Second", LastName = "User2", ShareableReferralCode = "XYZ789" });
            users.Add("3333", new User() { UserId = "3333", FirstName = "Third", LastName = "User3", ShareableReferralCode = "LMN555",
                Referrals = [ new Referral() { 
                    RefereeId = "1234", 
                    FirstName = "Alexis", 
                    LastName = "Saari", 
                    ReferralStatus = ReferralStatus.Sent,
                }]});
        }
    }
}
