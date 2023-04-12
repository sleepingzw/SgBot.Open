using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgBot.Open.DataTypes.Basic;

namespace SgBot.Open.DataTypes.StaticData
{
    internal static class StaticData
    {
        public static string? ExePath;
        public static List<BotInfo> BotInfos = new();
        public static BotConfig BotConfig = new();
        public static string MiraiPath = "C:\\Users\\Administrator\\Desktop\\SgBot";
    }
}
