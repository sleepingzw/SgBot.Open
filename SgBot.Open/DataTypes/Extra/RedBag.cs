using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SgBot.Open.Utils.Basic;
using SlpzLibrary;

namespace SgBot.Open.DataTypes.Extra
{
    internal class RedBag : IComparable
    {
        
        public int Id { get; set; }
        public int AmountLeft { get; set; }
        public int TokenLeft { get; set; }
        public List<string> WhoGot { get; set; }
        public RedBag(int id, int amountLeft, int tokenLeft)
        {
            Id = id;
            AmountLeft = amountLeft;
            TokenLeft = tokenLeft;
            WhoGot = new List<string>();
        }
        public int CompareTo(object? obj)
        {
            var p = (RedBag)obj!;
            return this.Id.CompareTo(p.Id);
        }

    }

    public static class RedBagManager
    {
        private static readonly Dictionary<string, List<RedBag>> AllRedBags = new();

        public static int CreateRedBag(string groupId,int allToken, int allAmount)
        {
            var id = 1;
            if (AllRedBags.ContainsKey(groupId))
            {
                AllRedBags[groupId].Sort();
                while (true)
                {
                    var sameFlag = AllRedBags[groupId].Any(pkgs => pkgs.Id == id);
                    if (sameFlag)
                    {
                        id++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                AllRedBags.Add(groupId, new List<RedBag>());
            }

            var rp = new RedBag(id, allAmount, allToken);
            AllRedBags[groupId].Add(rp);
            return id;
        }
        /// <summary>
        /// 返回红包状态
        /// </summary>
        /// <param name="groupId">红包所在群</param>
        /// <param name="whichPkg">群中哪个红包</param>
        /// <param name="who">领红包的是谁</param>
        /// <returns>红包状态</returns>
        public static RedBagStatus GetRedBag(string groupId, int whichPkg, string who)
        {
            if (AllRedBags.ContainsKey(groupId))
            {
                foreach (var pkg in AllRedBags[groupId])
                {
                    if (pkg.Id == whichPkg)
                    {
                        if (pkg.WhoGot.Contains(who))
                        {
                            return RedBagStatus.OneHaveGot;
                        }
                        return RedBagStatus.Success;
                    }
                }
                return RedBagStatus.CouldNotFind;
            }
            return RedBagStatus.CouldNotFind;
        }
        /// <summary>
        /// 打开红包
        /// </summary>
        /// <param name="groupId">红包所在群</param>
        /// <param name="whichPkg">群中哪个红包</param>
        /// <param name="who">领红包的是谁</param>
        /// <returns>领到的红包数量</returns>
        public static int OpenRedBag(string groupId, int whichPkg, string who)
        {
            var tempBag = AllRedBags[groupId].First(b => b.Id == whichPkg)!;

            var amountLeft = tempBag.AmountLeft;
            var tokenLeft = tempBag.TokenLeft;
            var ret = 0;
            if (amountLeft == 1)
            {
                ret = tokenLeft;
                RedBagDispose(groupId, whichPkg, who);
            }
            else
            {
                ret = (int)UsefulMethods.MakeRandom((2 * tokenLeft) / amountLeft,1 );
                tempBag.TokenLeft -= ret;
                RedBagDispose(groupId, whichPkg, who);
            }
            return ret;
        }
        /// <summary>
        /// 判断开了红包之后的红包存在与否
        /// </summary>
        /// <param name="groupId">红包所在群</param>
        /// <param name="whichPkg">群中哪个红包</param>
        /// <param name="who">谁领红包</param>
        /// <returns>true是红包被销毁，false是没有被销毁</returns>
        private static bool RedBagDispose(string groupId, int whichPkg, string who)
        {
            var tempBag = AllRedBags[groupId].First(b => b.Id == whichPkg)!;
            if (tempBag.AmountLeft == 1)
            {
                AllRedBags[groupId].Remove(tempBag);
                if (AllRedBags[groupId].Count == 0)
                {
                    AllRedBags.Remove(groupId);
                }
                return true;
            }
            tempBag.AmountLeft--;
            tempBag.WhoGot.Add(who);
            return false;
        }

        public static string ShowAllBag(string groupid)
        {
            var ret = "";
            foreach (var b in AllRedBags[groupid])
            {
                ret += DataOperator.ToJsonString(b);
                ret += '\n';
            }

            return ret;
        }
    }
    public enum RedBagStatus
    {
        CouldNotFind,
        OneHaveGot,
        Success
    }
}
