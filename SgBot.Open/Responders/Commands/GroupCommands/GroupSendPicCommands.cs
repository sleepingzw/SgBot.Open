using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Utils.Scaffolds;
using SgBot.Open.DataTypes.BotFunction;
using SgBot.Open.DataTypes.StaticData;
using SgBot.Open.Utils.Basic;
using SgBot.Open.Utils.Extra;
using SlpzLibrary;

namespace SgBot.Open.Responders.Commands.GroupCommands
{
    internal static partial class BotGroupCommands
    {
        /// <summary>
        /// 从本地图库发送影之诗卡图
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "随机szb", "随机sv", "来点szb", "来点sv" }, "/rdsv")]
        public static async Task RandomSvPic(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            var pics = Directory.GetFiles(Path.Combine(StaticData.ExePath!, "Data\\Img\\RandomSv")).ToList();
            var pic = UsefulMethods.GetRandomFromList(pics);
            var img = new ImageMessage()
            {
                Path = pic
            };
            await groupMessageReceiver.SendMessageAsync(img);
            DataBaseOperator.UpSvCount();
        }
        /// <summary>
        /// 从本地图库发送龙图
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "一眼丁真", "一眼顶真", "yydz" }, "/yydz")]
        public static async Task RandomYydzPic(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            var pics = Directory.GetFiles(Path.Combine(StaticData.ExePath!, "Data\\Img\\Yydz")).ToList();
            var pic = UsefulMethods.GetRandomFromList(pics);
            var img = new ImageMessage()
            {
                Path = pic
            };
            await groupMessageReceiver.SendMessageAsync(img);
            DataBaseOperator.UpYydzCount();
        }
        /*
        [ChatCommand(new string[] { "看龙图", "随机龙图" }, "/dragon")]
        public static async Task DragonPic(GroupMessageReceivedInfo GroupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            GroupMessageReceivedInfo.Member.Token--;
            var img = new ImageMessage()
            {
                Url = "nmsl.su"
            };
            await groupMessageReceiver.SendMessageAsync(img);
            await DataBaseOperator.UpdateUserInfo(GroupMessageReceivedInfo.Member);
        }
        */
        /// <summary>
        /// 获取头像并且发送摸头图
        /// </summary>
        /// <param name="GroupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("摸头", "/petpet")]
        public static async Task PetPetPic(GroupMessageReceivedInfo GroupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (GroupMessageReceivedInfo.AtMessages.Count < 1)
            {
                await groupMessageReceiver.QuoteMessageAsync("找不到摸头的对象");
                return;
            }
            if (GroupMessageReceivedInfo.Member.Token < 10)
            {
                await groupMessageReceiver.QuoteMessageAsync("你的傻狗力不足哦");
                return;
            }
            var target = GroupMessageReceivedInfo.AtMessages[0].Target;
            if (target == StaticData.BotConfig.BotQQ)
            {
                await groupMessageReceiver.QuoteMessageAsync("?");
                return;
            }
            GroupMessageReceivedInfo.Member.Token -= 2;
            await DataBaseOperator.UpdateUserInfo(GroupMessageReceivedInfo.Member);

            var path = PetPetMaker.MakePetPet(target);
            var img = new ImageMessage()
            {
                Path = path
            };
            await groupMessageReceiver.SendMessageAsync(img);
        }
    }
}
