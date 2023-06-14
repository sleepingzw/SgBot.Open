using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using SgBot.Open.Utils.Basic;
using SgBot.Open.Utils.Extra;
using SlpzLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirai.Net.Utils.Scaffolds;
using SgBot.Open.DataTypes.BotFunction;
using Mirai.Net.Sessions.Http.Managers;

namespace SgBot.Open.Responders.Commands.GameCommands
{
    public static partial class BotGameCommands
    {
        /// <summary>
        /// 发送人物属性图片
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "人物" }, new string[] { "/game.me", "/g.me" }, true)]
        public static async Task PrintPlayer(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            var player = await DataBaseOperator.FindPlayer(groupMessageReceivedInfo.Member.UserId);
            player.Refresh();
            // player.SortBag();
            await DataBaseOperator.UpdatePlayer(player);
            var pic = GameImageMaker.MakeSgGamePlayerImage(player);
            var id = await FileManager.UploadImageAsync(pic);
            var chain = new MessageChainBuilder().ImageFromId(id.Item1).Build();

            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, chain));
            TaskHolder.DeleteTask(pic);
        }
        /// <summary>
        /// 发送背包图片
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "背包" }, new string[] { "/game.bag", "/g.bag" }, true)]
        public static async Task PrintPlayerBag(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            var player = await DataBaseOperator.FindPlayer(groupMessageReceivedInfo.Member.UserId);
            player.Refresh();
            // player.SortBag();
            await DataBaseOperator.UpdatePlayer(player);
            var pic = GameImageMaker.MakeSgGameBag(player);
            var id = await FileManager.UploadImageAsync(pic);
            var chain = new MessageChainBuilder().ImageFromId(id.Item1).Build();

            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, chain));
            TaskHolder.DeleteTask(pic);
        }
        /// <summary>
        /// 发送傻狗之巅图片
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "傻狗之巅" }, new string[] { "/game.rank", "/g.rank" }, true)]
        public static async Task PrintRank(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            var player = await DataBaseOperator.FindPlayer(groupMessageReceivedInfo.Member.UserId);
            if (player.Refresh())
            {
                await DataBaseOperator.UpdatePlayer(player);
            }
            var pic = GameImageMaker.MakeSgGameRankImage(DataBaseOperator.OutGameRank());
            var id = await FileManager.UploadImageAsync(pic);
            var chain = new MessageChainBuilder().ImageFromId(id.Item1).Build();

            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, chain));
            TaskHolder.DeleteTask(pic);
        }
        /// <summary>
        /// 更改玩家名字
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "改名" }, new string[] { "/game.rename", "/g.rename" }, true)]
        public static async Task ReName(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.PlainMessages.Count < 2)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "参数错误", true));
                return;
            }
            var name = groupMessageReceivedInfo.PlainMessages[1];
            if (name.Length > 32)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "名称过长", true));
                return;
            }
            if (name.Contains('[') || name.Contains(']') || name.Contains('【') || name.Contains('】') || name.Contains("傻狗"))
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "非法名称", true));
                return;
            }
            var player = await DataBaseOperator.FindPlayer(groupMessageReceivedInfo.Member.UserId);
            player.Name = name;
            await DataBaseOperator.UpdatePlayer(player);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"{player.Name}({player.Id}) 改名成功", true));
        }
        /// <summary>
        /// 查询别人信息
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "信息" }, new string[] { "/game.info", "/g.info" }, true)]
        public static async Task GetInfo(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.AtMessages.Count == 0)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "没有对象", true));
                return;
            }
            var target = groupMessageReceivedInfo.AtMessages[0].Target;
            var other = await DataBaseOperator.FindPlayer(target);
            var pic = GameImageMaker.MakeOtherInfoImage(other);
            var id = await FileManager.UploadImageAsync(pic);
            var chain = new MessageChainBuilder().ImageFromId(id.Item1).Build();

            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, chain));
            TaskHolder.DeleteTask(pic);
        }
    }
}
