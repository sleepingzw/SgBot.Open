using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Sessions.Http.Managers;
using Mirai.Net.Utils.Scaffolds;
using SgBot.Open.DataTypes.BotFunction;
using SgBot.Open.DataTypes.StaticData;
using SgBot.Open.Utils.Basic;
using SlpzLibrary;

namespace SgBot.Open.Responders.Commands.GroupCommands
{
    internal static partial class BotGroupCommands
    {
        private const string UpdateInfos = "傻狗Bot V0.3.0:\n总之就是重构了代码" +
                                           "傻狗Bot V0.3.1:\n总之就是修了bug\n加了随机延时\n加了些怪功能" +
                                           "傻狗Bot V0.3.2:\n傻狗大陆再开！公测" +
                                           "傻狗Bot V0.3.3:\n下载失败的图片会重新下载了\n增加了一些可能永远不会被发现的互动" +
                                           "傻狗Bot V0.3.4:\n自动黑名单功能回归" +
                                           "傻狗Bot V0.4.0:\n优化消息处理队列" +
                                           "傻狗Bot V0.4.1:\n修复了一些bug" +
                                           "傻狗Bot V0.4.2:\n优化了系统延迟";

        /// <summary>
        /// 查看bot的一些信息
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("傻狗info", "/info")]
        public static async Task Info(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver,
                "傻狗Bot V0.4.3\n感谢Mirai,Mirai.NET"));
            // await groupMessageReceiver.SendMessageAsync("傻狗Bot V0.4.1\n感谢Mirai,Mirai.NET");
        }
        /// <summary>
        /// 查看bot最近一个版本更新了什么
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("更新info", "/updateinfo")]
        public static async Task UpdateInfo(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver,
                "傻狗Bot V0.4.3:\n更新了傻狗牌系统"));
            // await groupMessageReceiver.SendMessageAsync("傻狗Bot V0.4.1:\n修复了一些bug");
        }
        /// <summary>
        /// 呼唤bot
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("傻狗", "/sgbot")]
        public static async Task CallBot(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            var id = await FileManager.UploadImageAsync(Path.Combine(StaticData.ExePath!, "Data/Img/CallMe.png"));
            var chain = new MessageChainBuilder().ImageFromId(id.Item1).Build();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, chain));
            // await groupMessageReceiver.SendMessageAsync(chain);
        }
        /// <summary>
        /// 查看傻狗力排名
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "傻狗排名", "傻狗排行", "傻狗总榜" }, "/sgsort")]
        public static async Task TokenSort(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            var list = DataBaseOperator.TokenSort();
            var ret = ImageMaker.MakeSortImage(list, groupMessageReceivedInfo.Member);

            var id = await FileManager.UploadImageAsync(ret);
            var chain = new MessageChainBuilder().ImageFromId(id.Item1).Build();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, chain));

            TaskHolder.DeleteTask(ret);
        }
        /// <summary>
        /// 展示作者的zfb二维码
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("投喂作者", "/pay")]
        public static async Task PayMe(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            var builder = new MessageChainBuilder();

            var id = await FileManager.UploadImageAsync(Path.Combine(StaticData.ExePath!, "Data/Img/PayMe.png"));
            var chain = builder.Plain("呜呜，非常感谢").ImageFromId(id.Item1).Build();

            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, chain));
        }
    }
}
