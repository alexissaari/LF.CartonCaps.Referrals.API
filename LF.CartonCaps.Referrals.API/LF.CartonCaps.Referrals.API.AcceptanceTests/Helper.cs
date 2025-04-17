using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LF.CartonCaps.Referrals.API.Models;
using Newtonsoft.Json;

namespace LF.CartonCaps.Referrals.API.AcceptanceTests
{
    public static class Helper
    {
        public static async Task<IList<Referral>?> GetReferralsReadAndDeserialize(HttpClient client, string userId)
        {
            var response = await client.GetAsync(userId);
            var contentString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IList<Referral>>(contentString);
        }
    }
}
