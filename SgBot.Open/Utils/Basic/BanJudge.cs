using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBot.Open.Utils.Basic
{
    public static class BanJudge
    {
        private static readonly Dictionary<string, (DateTime lastTime, int weight)> BanWeightDictionary = new();
        /// <summary>
        /// 判断group是否应该进入黑名单
        /// </summary>
        /// <param name="groupId">传入group的Id</param>
        /// <returns></returns>
        public static bool BanActTrigger(string groupId)
        {
            if (BanWeightDictionary.ContainsKey(groupId))
            {
                if (DateTime.Now - BanWeightDictionary[groupId].lastTime > new TimeSpan(1, 0, 0))
                {
                    BanWeightDictionary[groupId] = (DateTime.Now, 0);
                }
                else
                {
                    BanWeightDictionary[groupId] = (DateTime.Now, BanWeightDictionary[groupId].weight + 1);
                }
                return BanWeightDictionary[groupId].weight > 5;
            }
            BanWeightDictionary.Add(groupId, (DateTime.Now, 1));
            return false;
        }
    }
}
