using System.Diagnostics;
using System.Text.RegularExpressions;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Utils.Scaffolds;
using SgBot.Open.DataTypes.BotFunction;
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

            var cpu = DeviceMonitor.GetCurrentCpuUsage();
            var ram = DeviceMonitor.GetAvailableRam();
            var processCpu = DeviceMonitor.GetCpuUsageByProcessName(processName);
            var processMem = DeviceMonitor.MemoryUsingByApp(processName);
            var threadCt = DeviceMonitor.GetThreadCount(processName);
            var hdd = DeviceMonitor.GetHddSpace(processName);
            var currentStatus =
                $"总CPU占用率={cpu},\r\n" +
                $"{processName}占用的CPU={processCpu},\r\n" +
                $"空闲可用内存={ram},\r\n" +
                $"{processName}占用内存={processMem},\r\n" +
                $"{processName}占用线程数={threadCt},\r\n" +
                $"磁盘={hdd}";
            await groupMessageReceiver.QuoteMessageAsync(currentStatus);
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
            await groupMessageReceiver.SendMessageAsync($"ADD {targetUser.Nickname} {temp} SUCCEED");
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
            await groupMessageReceiver.SendMessageAsync($"Setu {ret.SetuCount}\nSv {ret.SvCount}\nYydz {ret.YydzCount}\n{rr}");
        }
    }
}
