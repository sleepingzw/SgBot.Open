using SgBot.Open.DataTypes.BotFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBot.Open.Utils.Basic
{
    internal static class UsefulMethods
    {
        public static T GetRandomFromList<T>(List<T> list)
        {
            var rr = new Random().Next() % list.Count;
            return list[rr];
        }
        /// <summary>
        /// 使值在区间内
        /// </summary>
        /// <param name="what"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static double OutGoodNumber(double what, double max, double min)
        {
            if (what > max)
            {
                return max;
            }
            if (what < min)
            {
                return min;
            }
            return what;
        }
        public static async Task Fresh(GroupInfo groupInfo)
        {
            var changed = false;
            if (groupInfo.Cao == RepeatStatus.Done)
            {
                groupInfo.Cao = RepeatStatus.Idle;
                changed = true;
            }
            if (groupInfo.QuestionMark == RepeatStatus.Done)
            {
                groupInfo.QuestionMark = RepeatStatus.Idle;
                changed = true;
            }

            if (changed)
            {
                await DataBaseOperator.UpdateGroupInfo(groupInfo);
            }
        }
        /// <summary>
        /// 判断一个数的随机结果是否满足条件
        /// </summary>
        /// <param name="max">随机的最大值</param>
        /// <param name="belowWhatIsGood">小于等于这个值返回True</param>
        /// <returns></returns>
        public static bool IsOk(int max, int belowWhatIsGood = 0)
        {
            var rd = new Random();
            var a = rd.Next() % max;
            return a <= belowWhatIsGood;
        }
        /// <summary>
        /// 生成一个随机数
        /// </summary>
        /// <param name="max">最大值</param>
        /// <param name="min">最小值</param>
        /// <returns></returns>
        public static double MakeRandom(double max, double min = 0)
        {
            if (max <= min)
            {
                return 0;
            }
            var rd = new Random();
            var a = rd.Next() % (max - min);
            a += min;
            return a;
        }
    }
}
