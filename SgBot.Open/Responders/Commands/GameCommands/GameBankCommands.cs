using Mirai.Net.Data.Messages.Receivers;
using SgBot.Open.Utils.Basic;
using SlpzLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mirai.Net.Utils.Scaffolds;
using SgBot.Open.DataTypes.BotFunction;
using Manganese.Text;

namespace SgBot.Open.Responders.Commands.GameCommands
{
    public static partial class BotGameCommands
    {
        /// <summary>
        /// 用金币购买体力，注意是梯度付费
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "体力" }, new string[] { "/game.power", "/g.power" }, true)]
        public static async Task GetPower(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.PlainMessages.Count < 2)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "参数错误", true));
                return;
            }
            var temp = Regex.Replace(groupMessageReceivedInfo.PlainMessages[1], @"[^0-9]+", "");
            if (temp.IsNullOrEmpty())
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "参数错误", true));
                return;
            }
            var what = int.Parse(temp);
            if (what == 0)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "数值不能为0", true));
                return;
            }

            var player = await DataBaseOperator.FindPlayer(groupMessageReceivedInfo.Member.UserId);
            player.Refresh();
            var need = player.PowerNeedCoin(what) * player.Level;
            if (player.Coin < need)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver,
                    $"金币不足{player.PowerNeedCoin(what) * player.Level}\n" + $"今天已经兑换了{player.BuyPowerTime}\n" +
                    "购买体力是梯度付费", true));
                return;
            }
            player.Coin -= need;
            player.Power += what;
            player.BuyPowerTime += what;
            await DataBaseOperator.UpdatePlayer(player);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver,
                $"{player.Name}({player.Id})购买{what} 体力成功", true));
        }
        /// <summary>
        /// 用傻狗力购买金币
        /// </summary>
        /// <param name="GroupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "金币" }, new string[] { "/game.coin", "/g.coin" }, true)]
        public static async Task TokenGetCoin(GroupMessageReceivedInfo GroupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (GroupMessageReceivedInfo.PlainMessages.Count < 2)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "参数错误", true));
                // await groupMessageReceiver.QuoteMessageAsync("参数错误");
                return;
            }
            var temp = Regex.Replace(GroupMessageReceivedInfo.PlainMessages[1], @"[^0-9]+", "");
            if (temp.IsNullOrEmpty())
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "参数错误", true));
                // await groupMessageReceiver.QuoteMessageAsync("参数错误");
                return;
            }
            var what = int.Parse(temp);
            if (what == 0)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "数值不能为0", true));
                // await groupMessageReceiver.QuoteMessageAsync("数值不能为0");
                return;
            }
            var player = await DataBaseOperator.FindPlayer(GroupMessageReceivedInfo.Member.UserId);

            player.Refresh();
            if (GroupMessageReceivedInfo.Member.Token < what)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"傻狗力不足{what} 1傻狗力可以兑换（玩家等级）金币",
                    true));
                // await groupMessageReceiver.QuoteMessageAsync($"傻狗力不足{what} 1傻狗力可以兑换（玩家等级）金币");
                return;
            }

            player.Coin += what * player.Level;
            GroupMessageReceivedInfo.Member.Token -= what;
            await DataBaseOperator.UpdateUserInfo(GroupMessageReceivedInfo.Member);
            await DataBaseOperator.UpdatePlayer(player);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"{player.Name}({player.Id})购买 {what * player.Level} 金币成功", true));
        }
        /// <summary>
        /// 用金币提取傻狗力
        /// </summary>
        /// <param name="GroupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "提取傻狗力" }, new string[] { "/game.token", "/g.token" }, true)]
        public static async Task CoinToToken(GroupMessageReceivedInfo GroupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (GroupMessageReceivedInfo.PlainMessages.Count < 2)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "参数错误", true));
                return;
            }
            var temp = Regex.Replace(GroupMessageReceivedInfo.PlainMessages[1], @"[^0-9]+", "");
            if (temp.IsNullOrEmpty())
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "参数错误", true));
                return;
            }
            var what = int.Parse(temp);
            if (what == 0)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "数值不能为0", true));
                return;
            }
            var player = await DataBaseOperator.FindPlayer(GroupMessageReceivedInfo.Member.UserId);

            player.Refresh();
            if (player.Coin < what * player.Level * 2)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"金币不足{what * player.Level * 2} 1傻狗力需要 2倍玩家等级 的金币", true));
                return;
            }

            player.Coin -= what * player.Level * 2;
            GroupMessageReceivedInfo.Member.Token += what;
            await DataBaseOperator.UpdateUserInfo(GroupMessageReceivedInfo.Member);
            await DataBaseOperator.UpdatePlayer(player);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"{player.Name}({player.Id})提取 {what} 傻狗力成功", true));
        }
    }
}
