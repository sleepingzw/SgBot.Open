using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBot.Open.Utils.Basic
{
    public static class RespondLimiter
    {
        private static readonly Dictionary<string, DateTime> GroupRespondDictionary = new();
        public static void AddRespond(string groupId, DateTime timeNow)
        {
            GroupRespondDictionary[groupId] = timeNow;
        }
        public static bool CanRespond(string groupId, DateTime timeNow)
        {
            if (GroupRespondDictionary.ContainsKey(groupId))
            {
                return timeNow - GroupRespondDictionary[groupId] > new TimeSpan(0, 0, 3);
            }
            GroupRespondDictionary.Add(groupId, timeNow);
            return true;
        }
    }
}
