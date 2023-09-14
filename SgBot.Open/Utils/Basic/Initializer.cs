using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manganese.Text;
using Mirai.Net.Sessions;
using SgBot.Open.DataTypes.Basic;
using SgBot.Open.DataTypes.StaticData;
using SlpzLibrary;
using Spectre.Console.Rendering;

namespace SgBot.Open.Utils.Basic
{
    internal static class Initializer
    {
        /// <summary>
        /// 初始化整个程序，获取基本配置
        /// </summary>
        /// <returns></returns>
        public static bool Initial()
        {
            try
            {
                StaticData.ExePath = Environment.CurrentDirectory;
                if (StaticData.ExePath.IsNullOrEmpty())
                    return false;

                var infos = DataOperator.GetJsonFile<List<BotInfo>>(Path.Combine(StaticData.ExePath, "Data/BotInfo.json"));
                if (infos is null)
                {
                    Console.WriteLine("fatal");
                    return false;
                }
                else
                {
                    StaticData.BotInfos = infos;                        
                }

                var config= DataOperator.GetJsonFile<BotConfig>(Path.Combine(StaticData.ExePath, "Data/BotConfig.json"));
                if(config is null)
                {
                    Console.WriteLine("fatal");
                    return false;
                }
                else
                {
                    StaticData.BotConfig = config;
                }
                
                var flag = CheckDirectoryCreated();
                //Console.WriteLine(StaticData.BotConfig.MahAddress);
                //Console.WriteLine(StaticData.BotConfig.VerifyKey);
                //Console.WriteLine(StaticData.BotConfig.BotQQ);
                return flag;
            }
            catch(Exception exception)
            {
                Logger.Log(exception.Message, LogLevel.Fatal);
                return false;
            }
        }

        public static MiraiBot InitBot()
        {
            return new MiraiBot
            {
                Address = StaticData.BotConfig.MahAddress,
                VerifyKey = StaticData.BotConfig.VerifyKey,
                QQ = StaticData.BotConfig.BotQQ
            };
        }

        public static void StartQueueOut()
        {
            // ReceiverQueue.StartOutReceiver();
            RespondQueue.StartOutRespond();
        }
        private static bool CheckDirectoryCreated()
        {
            var flag = true;
            var path1= Path.Combine(StaticData.ExePath!, "Data");
            if (Directory.Exists(path1)) return flag;
            Logger.Log($"Data文件夹未创建 {StaticData.ExePath}", LogLevel.Fatal);
            Directory.CreateDirectory(path1);
            flag = false;
            return flag;
        }
    }
}
