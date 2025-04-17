using System.Collections.Concurrent;
using LF.CartonCaps.Referrals.API.Models;

namespace LF.CartonCaps.Referrals.API.ApiClients.FakeInMemoryDatastores
{
    /*
     * Since this REST Api is not responsible for creating users,
     * but we still want users to exist to for use to work with referrals,
     * let's give ourselves a few fake users to work with.
     */
    public static class UsersDatastore
    {
        private static readonly ConcurrentDictionary<string, User> users = new();

        static UsersDatastore()
        {
            users = new ConcurrentDictionary<string, User>();
            PopulateDatastore();
        }

        public static User? GetUser(string userId)
        {
            if (!users.TryGetValue(userId, out User? user))
            {
                return null;
            }

            return user;
        }

        public static bool UpdateUser(User user)
        {
            if (users.TryGetValue(user.UserId, out User? existingUser))
            {
                return users.TryUpdate(user.UserId, user, existingUser);
            }
            return false;
        }

        private static void PopulateDatastore()
        {
            users.TryAdd("1", new User() { UserId = "1", FirstName = "First", LastName = "User1", ShareableReferralCode = "A1B2C3" });
            
            users.TryAdd("2", new User() 
            { 
                UserId = "2", 
                FirstName = "Second", 
                LastName = "User2", 
                ShareableReferralCode = "X7Y8Z9",
                Referrals = new List<Referral>()
            });
            
            users.TryAdd("3", new User()
            {
                UserId = "3",
                FirstName = "Third",
                LastName = "User3",
                ShareableReferralCode = "LMNOP1",
                Referrals = [ new Referral() {
                    RefereeId = "123",
                    FirstName = "Alexis",
                    LastName = "Saari",
                    ReferralStatus = ReferralStatus.Sent,
                }]
            });
        }
    }
}
