using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBot.Open.DataTypes.Extra
{
    internal class RedPackage
    {
        public int Id { get; set; }
        public int AmountLeft { get; set; }
        public int TokenLeft { get; set; }
        public List<string> WhoGot { get; set; }
    }

    public static class RedPacketManager
    {
        private static Dictionary<string, List<RedPackage>> _allPackages = new();

        public static void CreatePackage(string groupId,int allToken, int allAmount)
        {
            var id = 0;
            if (_allPackages.ContainsKey(groupId))
            {
                while (true)
                {
                    var sameFlag = _allPackages[groupId].Any(pkgs => pkgs.Id == id);
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
                _allPackages.Add(groupId, new List<RedPackage>());
            }
            var rp = new RedPackage()
            {
                Id = id,
                AmountLeft = allAmount,
                TokenLeft = allToken,
                WhoGot = new List<string>()
            };
            _allPackages[groupId].Add(rp);
        }

        public static PackageStatus GetPackage(string groupId, int whichPkg, string who)
        {
            return PackageStatus.CouldNotFind;
        }

        
    }
    public enum PackageStatus
    {
        CouldNotFind,
        OneHaveGot
    }
}
