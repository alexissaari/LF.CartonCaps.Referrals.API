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

        public static void UpdateUser(string userId, User user)
        {
            if (users.ContainsKey(userId))
            {
                users[userId] = user;
            }
            else
            {
                users.TryAdd(userId, user);
            }
        }

        private static void PopulateDatastore()
        {
            users.TryAdd("1111", new User() { UserId = "1111", FirstName = "First", LastName = "User1", ShareableReferralCode = "ABC123" });
            users.TryAdd("2222", new User() { UserId = "2222", FirstName = "Second", LastName = "User2", ShareableReferralCode = "XYZ789" });
            users.TryAdd("3333", new User()
            {
                UserId = "3333",
                FirstName = "Third",
                LastName = "User3",
                ShareableReferralCode = "LMN555",
                Referrals = [ new Referral() {
                    RefereeId = "1234",
                    FirstName = "Alexis",
                    LastName = "Saari",
                    ReferralStatus = ReferralStatus.Sent,
                }]
            });
        }
    }
}
