using System.Collections;
using System.Numerics;
using LF.CartonCaps.Referrals.API.Models;
using LF.CartonCaps.Referrals.API.Models.Exceptions;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

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
    public static class DatabaseProxy
    {
        private static Dictionary<string, User> users;
        private static Dictionary<string, ReferralStatus> activeReferees;

        static DatabaseProxy()
        {
            users = new Dictionary<string, User>();
            activeReferees = new Dictionary<string, ReferralStatus>();
            PopulateDatastore();
        }

        public static IList<Referral>? GetReferrals(string userId)
        {
            var user = GetUser(userId);
            return user.Referrals;
        }

        public static void UpdateReferralStatus(string userId, string referralId, ReferralStatus newReferralStatus)
        {
            var referral = GetReferral(userId, referralId);

            if (referral == null)
            {
                throw new ReferralDoesNotExistException(
                    $"Attempted to update referral status, but referralId \"{referralId}\" does not exist for userId \"{userId}\".",
                    userId,
                    referralId);
            }

            referral.ReferralStatus = newReferralStatus;

            if (referral.ReferralStatus == ReferralStatus.Complete)
            {
                RemoveRefereeFromActiveReferees(referralId);
            } 
            else
            {
                AddOrUpdateRefereeInActiveReferees(referralId, newReferralStatus);
            }
        }

        public static string InviteFriend(string userId, string firstName, string lastName)
        {
            var referral = GetReferral(userId, firstName, lastName);

            // We have already referred this friend
            if (referral != null)
            {
                return referral.RefereeId;
            }

            var user = GetUser(userId);
            var refereeId = Guid.NewGuid().ToString();
            
            if (user.Referrals == null)
            {
                user.Referrals = new List<Referral>();
            }

            var newReferee = new Referral()
            {
                RefereeId = refereeId,
                FirstName = firstName,
                LastName = lastName,
                ReferralStatus = ReferralStatus.Sent
            };

            user.Referrals.Add(newReferee);

            AddOrUpdateRefereeInActiveReferees(newReferee.RefereeId, newReferee.ReferralStatus);

            return newReferee.RefereeId;
        }

        private static Referral? GetReferral(string userId, string referralId)
        {
            var user = GetUser(userId);
            return user?.Referrals?.FirstOrDefault(x => x.RefereeId.Equals(referralId));
        }

        private static Referral? GetReferral(string userId, string firstName, string lastName)
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
            users.Add("1111", new User() { UserId = "1111", FirstName = "First", LastName = "User1", ReferralCode = "ABC123" });
            users.Add("2222", new User() { UserId = "2222", FirstName = "Second", LastName = "User2", ReferralCode = "XYZ789" });
            users.Add("3333", new User() { UserId = "3333", FirstName = "Third", LastName = "User3", ReferralCode = "LMN555",
                Referrals = [ new Referral() { 
                    RefereeId = "1234", 
                    FirstName = "Alexis", 
                    LastName = "Saari", 
                    ReferralStatus = ReferralStatus.Sent,
                }]});
        }

        private static void AddOrUpdateRefereeInActiveReferees(string refereeId, ReferralStatus referralStatus)
        {
            if (activeReferees.ContainsKey(refereeId))
            {
                activeReferees[refereeId] = referralStatus;
            }
            else
            {
                activeReferees.Add(refereeId, referralStatus);
            }
        }

        private static void RemoveRefereeFromActiveReferees(string refereeId)
        {
            activeReferees.Remove(refereeId);
        }
    }
}
