using System.Collections;
using System.Numerics;
using LF.CartonCaps.Referrals.API.Models;
using LF.CartonCaps.Referrals.API.Models.Exceptions;

namespace LF.CartonCaps.Referrals.API.Proxies
{
    /* 
     * Ideally, this REST Api would call another API that is responsible for datastore CRUD operations.
     * 
     * Since this service is not responsible for creating users or modifying non-referral attributes,
     * let's give ourselves a datastore and a few Users to work with off the bat.
     */
    public static class MochDatabaseProxy
    {
        private static Dictionary<int, User> Users;

        static MochDatabaseProxy()
        {
            Users = new Dictionary<int, User>();
            populateMochDatasource();
        }

        public static IList<Referral>? GetReferrals(int userId)
        {
            var user = GetUser(userId);

            return user.Referrals;
        }

        public static void PatchReferral(int userId, int referralId, ReferralStatus referralStatus)
        {
            var user = GetUser(userId);
            var referral = GetReferral(user, referralId);

            referral.ReferralStatus = referralStatus;
        }

        private static User GetUser(int userId)
        {
            Users.TryGetValue(userId, out User? user);

            if (user == null)
            {
                // Ideally, we should never have a null user as the app
                // cannot ask for the referrals of a user without that user being logged in.
                throw new UserDoesNotExistException($"User Not Found. UserId = {userId}.", userId);
            }

            return user;
        }

        private static Referral GetReferral(User user, int referralId)
        {
            var referral = user?.Referrals?.FirstOrDefault(x => x.ReferralId == referralId);

            if (referral == null)
            {
                throw new ReferralDoesNotExistException(
                    $"Attempted to update referral status, but referralId \"{referralId}\" does not exist for userId \"{user.UserId}\".", 
                    user.UserId,
                    referralId);
            }

            return referral;
        }

        private static void populateMochDatasource()
        {
            Users.Add(1111, new User() { UserId = 1111, FirstName = "First", LastName = "User1", });
            Users.Add(2222, new User() { UserId = 2222, FirstName = "Second", LastName = "User2" });
            Users.Add(3333, new User() { UserId = 3333, FirstName = "Third", LastName = "User3", 
                Referrals = [ new Referral() { 
                    ReferralId = 123, 
                    FirstName = "Alexis", 
                    LastName = "Saari", 
                    ReferralStatus = ReferralStatus.Sent
                }]});
        }
    }
}
