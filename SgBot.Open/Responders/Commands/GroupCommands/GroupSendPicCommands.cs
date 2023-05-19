using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Data.Shared;
using Mirai.Net.Sessions.Http.Managers;
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
            var pics = Directory.GetFiles(Path.Combine(StaticData.ExePath!, "Data/Img/RandomSv")).ToList();
            var pic = UsefulMethods.GetRandomFromList(pics);

            var id = await FileManager.UploadImageAsync(pic);
            var chain = new MessageChainBuilder().ImageFromId(id.Item1).Build();

            // await groupMessageReceiver.SendMessageAsync(chain);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, chain));
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
            var pics = Directory.GetFiles(Path.Combine(StaticData.ExePath!, "Data/Img/Yydz")).ToList();
            var pic = UsefulMethods.GetRandomFromList(pics);

            var id = await FileManager.UploadImageAsync(pic);
            var chain = new MessageChainBuilder().ImageFromId(id.Item1).Build();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, chain));
            // await groupMessageReceiver.SendMessageAsync(chain);
            DataBaseOperator.UpYydzCount();
        }
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
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "找不到摸头的对象", true));
                // await groupMessageReceiver.QuoteMessageAsync("找不到摸头的对象");
                return;
            }
            if (GroupMessageReceivedInfo.Member.Token < 10)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "你的傻狗力不足哦", true));
                // await groupMessageReceiver.QuoteMessageAsync("你的傻狗力不足哦");
                return;
            }
            var target = GroupMessageReceivedInfo.AtMessages[0].Target;
            if (target == StaticData.BotConfig.BotQQ)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "?", true));
                return;
            }
            GroupMessageReceivedInfo.Member.Token -= 2;
            await DataBaseOperator.UpdateUserInfo(GroupMessageReceivedInfo.Member);

            var path = PetPetMaker.MakePetPet(target);

            var id = await FileManager.UploadImageAsync(path);
            var chain = new MessageChainBuilder().ImageFromId(id.Item1).Build();

            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, chain));
        }
    }
}
