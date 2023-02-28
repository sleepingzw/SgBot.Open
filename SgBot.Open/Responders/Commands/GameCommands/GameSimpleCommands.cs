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
            var img = new ImageMessage()
            {
                Path = pic
            };
            await groupMessageReceiver.SendMessageAsync(img);
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
            var img = new ImageMessage()
            {
                Path = pic
            };
            await groupMessageReceiver.SendMessageAsync(img);
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
            var img = new ImageMessage()
            {
                Path = pic
            };
            await groupMessageReceiver.SendMessageAsync(img);
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
                await groupMessageReceiver.QuoteMessageAsync("参数错误");
                return;
            }
            var name = groupMessageReceivedInfo.PlainMessages[1];
            if (name.Contains('[') || name.Contains(']') || name.Contains('【') || name.Contains('】') || name.Contains("傻狗"))
            {
                await groupMessageReceiver.QuoteMessageAsync("非法名称");
                return;
            }
            var player = await DataBaseOperator.FindPlayer(groupMessageReceivedInfo.Member.UserId);
            player.Name = name;
            await DataBaseOperator.UpdatePlayer(player);
            await groupMessageReceiver.QuoteMessageAsync($"{player.Name}({player.Id}) 改名成功");
        }
    }
}
