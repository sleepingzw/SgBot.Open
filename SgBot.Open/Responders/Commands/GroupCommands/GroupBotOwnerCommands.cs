using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Utils.Scaffolds;
using SgBot.Open.DataTypes.BotFunction;
using SgBot.Open.DataTypes.Extra;
using SgBot.Open.DataTypes.SgGame;
using SgBot.Open.Utils.Basic;
using SlpzLibrary;

namespace SgBot.Open.Responders.Commands.GroupCommands
{
    internal static partial class BotGroupCommands
    {
        /// <summary>
        /// 查询bot的状态
        /// </summary>
        /// <param name="groupReceiverInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("傻狗状态", "/botstatus")]
        public static async Task BotStatus(GroupMessageReceivedInfo groupReceiverInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (!groupReceiverInfo.IsOwner) return;
            var processName = Process.GetCurrentProcess().ProcessName;

            var currentStatus =
                $"总CPU占用率={'%'},\r\n" +
                $"{processName}占用的CPU={'%'},\r\n" +
                $"空闲可用内存={DeviceMonitor.GetMemoryMetricsMetrics().Free}MB,\r\n";
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, currentStatus, true));
        }
        /// <summary>
        /// 给某个人增加傻狗力
        /// </summary>
        /// <param name="groupReceiverInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("赠送", "/add")]
        public static async Task OwnerSend(GroupMessageReceivedInfo groupReceiverInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (!groupReceiverInfo.IsOwner) return;
            var target = groupReceiverInfo.AtMessages[0].Target;
            var targetUser = await DataBaseOperator.FindUser(target);
            var settingPara = groupReceiverInfo.PlainMessages[1];
            var temp = Regex.Replace(settingPara, @"[^0-9]+", "");
            targetUser.Token += int.Parse(temp);
            await DataBaseOperator.UpdateUserInfo(targetUser);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver,
                $"ADD {targetUser.Nickname} {temp} TOKEN SUCCEED"));
        }
        /// <summary>
        /// 给某个人增加金币
        /// </summary>
        /// <param name="groupReceiverInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("赠送金币", "/addcoin")]
        public static async Task OwnerSendCoin(GroupMessageReceivedInfo groupReceiverInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (!groupReceiverInfo.IsOwner) return;
            var target = groupReceiverInfo.AtMessages[0].Target;
            var targetPlayer = await DataBaseOperator.FindPlayer(target);
            var settingPara = groupReceiverInfo.PlainMessages[1];
            var temp = Regex.Replace(settingPara, @"[^0-9]+", "");
            targetPlayer.Coin += int.Parse(temp);
            await DataBaseOperator.UpdatePlayer(targetPlayer);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver,
                $"ADD {targetPlayer.Name} {temp} Coin SUCCEED"));
        }
        /// <summary>
        /// 给别人送傻狗牌
        /// </summary>
        /// <param name="groupReceiverInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("加傻狗牌", "/addcard")]
        public static async Task OwnerSendCard(GroupMessageReceivedInfo groupReceiverInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (!groupReceiverInfo.IsOwner) return;
            var target = groupReceiverInfo.AtMessages[0].Target;
            var targetUser = await DataBaseOperator.FindUser(target);
            var settingPara = groupReceiverInfo.PlainMessages[1];
            var temp = Regex.Replace(settingPara, @"[^0-9]+", "");
            targetUser.Card += int.Parse(temp);
            await DataBaseOperator.UpdateUserInfo(targetUser);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver,
                $"ADD {targetUser.Nickname} {temp} CARD SUCCEED"));
        }
        /// <summary>
        /// 查看bot的全局数据
        /// </summary>
        /// <param name="groupReceiverInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("全局数据", "/globaldata")]
        public static async Task SendGlobalData(GroupMessageReceivedInfo groupReceiverInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (!groupReceiverInfo.IsOwner) return;
            var ret = DataBaseOperator.GetCollectedData();
            var rr = DataBaseOperator.OutGameStatus();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver,
                $"Setu {ret.SetuCount}\nSv {ret.SvCount}\nYydz {ret.YydzCount}\n{rr}"));
        }
        [ChatCommand("展示全部红包", "/showredbag")]
        public static async Task ShowRedPackage(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.IsOwner)
            {

                var ret = RedBagManager.ShowAllBag(groupMessageReceivedInfo.Group.GroupId);
                await groupMessageReceiver.QuoteMessageAsync(ret);
            }
        }
        [ChatCommand("重置玩家每日状态", "/freshplayer")]
        public static async Task RefreshPlayer(GroupMessageReceivedInfo groupReceiverInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (!groupReceiverInfo.IsOwner) return;
            var target = groupReceiverInfo.AtMessages[0].Target;
            var targetPlayer = await DataBaseOperator.FindPlayer(target);
            targetPlayer.IsWinToday = false;
            targetPlayer.IsHitBoss = false;
            targetPlayer.Power = 20;
            await DataBaseOperator.UpdatePlayer(targetPlayer);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"{targetPlayer.Id} REFRESH"));
        }
        [ChatCommand("查看战斗细节", "/checkbattledetail")]
        public static async Task CheckBattleDetail (GroupMessageReceivedInfo groupReceiverInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (!groupReceiverInfo.IsOwner) return;
            var target = groupReceiverInfo.AtMessages[0].Target;
            var targetPlayer = await DataBaseOperator.FindPlayer(target);
            var unit = new BattleUnit(targetPlayer);

            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"{DataOperator.ToJsonString(unit)}"));
        }
    }
}
