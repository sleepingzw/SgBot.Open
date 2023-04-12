using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SgBot.Open.Utils.Basic;

namespace SgBot.Open.DataTypes.Extra
{
    internal class RedPackage : IComparable
    {
        
        public int Id { get; set; }
        public int AmountLeft { get; set; }
        public int TokenLeft { get; set; }
        public List<string> WhoGot { get; set; }
        public RedPackage(int id, int amountLeft, int tokenLeft)
        {
            Id = id;
            AmountLeft = amountLeft;
            TokenLeft = tokenLeft;
            WhoGot = new List<string>();
        }
        public int CompareTo(object? obj)
        {
            var p = (RedPackage)obj!;
            return this.Id.CompareTo(p.Id);
        }

    }

    public static class RedPacketManager
    {
        private static readonly Dictionary<string, List<RedPackage>> AllPackages = new();

        public static int CreatePackage(string groupId,int allToken, int allAmount)
        {
            var id = 0;
            if (AllPackages.ContainsKey(groupId))
            {
                AllPackages[groupId].Sort();
                while (true)
                {
                    var sameFlag = AllPackages[groupId].Any(pkgs => pkgs.Id == id);
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
                AllPackages.Add(groupId, new List<RedPackage>());
            }

            var rp = new RedPackage(id, allAmount, allToken);
            AllPackages[groupId].Add(rp);
            return id;
        }
        /// <summary>
        /// 返回红包状态
        /// </summary>
        /// <param name="groupId">红包所在群</param>
        /// <param name="whichPkg">群中哪个红包</param>
        /// <param name="who">领红包的是谁</param>
        /// <returns>红包状态</returns>
        public static PackageStatus GetPackage(string groupId, int whichPkg, string who)
        {
            if (AllPackages.ContainsKey(groupId))
            {
                foreach (var pkg in AllPackages[groupId])
                {
                    if (pkg.Id == whichPkg)
                    {
                        return pkg.WhoGot.Contains(who) ? PackageStatus.Success : PackageStatus.OneHaveGot;
                    }

                    return PackageStatus.CouldNotFind;
                }
            }
            else
            {
                return PackageStatus.CouldNotFind;
            }
            return PackageStatus.CouldNotFind;
        }
        /// <summary>
        /// 打开红包
        /// </summary>
        /// <param name="groupId">红包所在群</param>
        /// <param name="whichPkg">群中哪个红包</param>
        /// <param name="who">领红包的是谁</param>
        /// <returns>领到的红包数量</returns>
        public static int OpenPackage(string groupId, int whichPkg, string who)
        {
            var amountLeft = AllPackages[groupId][whichPkg].AmountLeft;
            var tokenLeft = AllPackages[groupId][whichPkg].TokenLeft;
            var ret = 0;
            if (amountLeft == 1)
            {
                ret = tokenLeft;
                PackageDispose(groupId, whichPkg, who);
            }
            else
            {
                ret = (int)UsefulMethods.MakeRandom(1, (2 * tokenLeft) / amountLeft);
                AllPackages[groupId][whichPkg].TokenLeft -= ret;
                PackageDispose(groupId, whichPkg, who);
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
        private static bool PackageDispose(string groupId, int whichPkg, string who)
        {
            if (AllPackages[groupId][whichPkg].AmountLeft == 1)
            {
                AllPackages[groupId].RemoveAt(whichPkg);
                if (AllPackages[groupId].Count == 0)
                {
                    AllPackages.Remove(groupId);
                }
                return true;
            }
            AllPackages[groupId][whichPkg].AmountLeft--;
            AllPackages[groupId][whichPkg].WhoGot.Add(who);
            return false;
        }
    }
    public enum PackageStatus
    {
        CouldNotFind,
        OneHaveGot,
        Success
    }
}
