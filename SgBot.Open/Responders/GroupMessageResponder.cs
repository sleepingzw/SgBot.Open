using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Manganese.Text;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Sessions.Http.Managers;
using Mirai.Net.Utils.Scaffolds;
using SgBot.Open.DataTypes.BotFunction;
using SgBot.Open.DataTypes.StaticData;
using SgBot.Open.Responders.Commands;
using SgBot.Open.Responders.Commands.GameCommands;
using SgBot.Open.Responders.Commands.GroupCommands;
using SgBot.Open.Utils.Basic;
using SlpzLibrary;

namespace SgBot.Open.Responders
{
    public static class GroupMessageResponder
    {
        public static async Task<bool> Respond(GroupMessageReceivedInfo groupReceiverInfo,GroupMessageReceiver groupMessageReceiver)
        {
            try
            {
                // 1 先判断短指令
                if (await TryShortCommandRespond(groupReceiverInfo, groupMessageReceiver))
                {
                    return true;
                }

                // 2 响应正常指令
                if (await TrySimpleCommandRespond(groupReceiverInfo, groupMessageReceiver))
                {
                    return true;
                }

                // 3 响应纯at
                if (groupReceiverInfo.AtMe &&
                    groupReceiverInfo.PlainMessages.Count == 0 && groupReceiverInfo.ImageMessages.Count == 0)
                {
                    await groupMessageReceiver.SendMessageAsync("你有什么b事吗\n没有就快爬\n有事自己看menu\n（输入 傻狗menu）");
                    await UsefulMethods.Fresh(groupReceiverInfo.Group);
                    return true;
                }

                // 4 响应游戏
                if (groupReceiverInfo.Group.CanGame)
                {
                    if (await TryGameCommandRespond(groupReceiverInfo, groupMessageReceiver))
                    {
                        return true;
                    }
                }

                // 5 响应复读
                if (await TryRepeatCommandRespond(groupReceiverInfo, groupMessageReceiver))
                {
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                if (e.Message.StartsWith("原因: 上传文件错误")|| e.Message.StartsWith("原因: 指定文件不存在"))
                {
                    return false;
                }
                if (e.Message.StartsWith("原因: Bot被禁言"))
                {
                    if (!BanJudge.BanActTrigger(groupReceiverInfo.Group.GroupId)) return false;
                    if (groupReceiverInfo.Group.IsBanned) return false;
                    groupReceiverInfo.Group.IsBanned = true;
                    await DataBaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
                    await MessageManager.SendFriendMessageAsync("2826241064",
                        groupReceiverInfo.Group.GroupId + " " + groupReceiverInfo.Group.GroupName + " 进入黑名单 " +
                        DateTime.Now);
                    return false;
                }
                var exceptionAddress = Path.Combine(StaticData.ExePath!, $"Data/Exception/{DateTime.Now:yyyy-M-dd--HH-mm-ss}.json");
                await MessageManager.SendFriendMessageAsync("2826241064",
                    e + "\n" + DateTime.Now);
                DataOperator.WriteJsonFile(exceptionAddress, DataOperator.ToJsonString(e));
                Logger.Log(e.Message, LogLevel.Fatal);
                return false;
            }
        }
        /// <summary>
        /// 短指令的回应器，不需要满足at，需要没有special act
        /// </summary>
        /// <param name="groupReceiverInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        private static async Task<bool> TryShortCommandRespond(GroupMessageReceivedInfo groupReceiverInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            // 没有文本就直接结束短指令回应了
            if (groupReceiverInfo.AllPlainMessage.IsNullOrEmpty())
                return false;
            var type = typeof(BotGroupCommands);
            foreach (var methodInfo in type.GetMethods())
            {
                var chatCommand = methodInfo.GetCustomAttribute<ChatCommandAttribute>();
                if (chatCommand == null) continue;

                if (chatCommand.ShortTrigger.Any(st => chatCommand.SpecialAct == ""
                                                       && groupReceiverInfo.PlainMessages[0].ToLower() == st))
                {
                    await (Task)methodInfo.Invoke(null, new object[] { groupReceiverInfo, groupMessageReceiver })!;
                    await UsefulMethods.Fresh(groupReceiverInfo.Group);
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// 常规命令的回应器，需要满足at，没有special act
        /// </summary>
        /// <param name="groupReceiverInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        private static async Task<bool> TrySimpleCommandRespond(GroupMessageReceivedInfo groupReceiverInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            // 没有文本就直接结束常规指令回应了
            if (groupReceiverInfo.AllPlainMessage.IsNullOrEmpty())
                return false;
            var type = typeof(BotGroupCommands);
            foreach (var methodInfo in type.GetMethods())
            {
                var chatCommand = methodInfo.GetCustomAttribute<ChatCommandAttribute>();
                if (chatCommand == null) continue;
                if (chatCommand.CommandTrigger.Any(t => chatCommand.SpecialAct == "" &&
                                                        chatCommand.IsAt == groupReceiverInfo.AtMe
                                                        && groupReceiverInfo.PlainMessages[0].ToLower() == t))
                {
                    await (Task)methodInfo.Invoke(null, new object[] { groupReceiverInfo, groupMessageReceiver })!;
                    await UsefulMethods.Fresh(groupReceiverInfo.Group);
                    return true;
                }

            }
            return false;
        }
        /// <summary>
        /// 游戏命令的回应器
        /// </summary>
        /// <param name="groupReceiverInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        public static async Task<bool> TryGameCommandRespond(GroupMessageReceivedInfo groupReceiverInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            // 没有文本就直接结束游戏指令回应了
            if (groupReceiverInfo.AllPlainMessage.IsNullOrEmpty())
                return false;
            var type = typeof(BotGameCommands);
            foreach (var methodInfo in type.GetMethods())
            {
                var chatCommand = methodInfo.GetCustomAttribute<ChatCommandAttribute>();
                if (chatCommand == null) continue;
                if (chatCommand.ShortTrigger.Any(st => groupReceiverInfo.PlainMessages[0].ToLower() == st))
                {
                    await (Task)methodInfo.Invoke(null, new object[] { groupReceiverInfo, groupMessageReceiver })!;
                    await UsefulMethods.Fresh(groupReceiverInfo.Group);
                    return true;
                }

                if (chatCommand.IsAt == groupReceiverInfo.AtMe &&
                    chatCommand.CommandTrigger.Any(t => t == groupReceiverInfo.PlainMessages[0].ToLower()))
                {
                    await (Task)methodInfo.Invoke(null, new object[] { groupReceiverInfo, groupMessageReceiver })!;
                    await UsefulMethods.Fresh(groupReceiverInfo.Group);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 复读的回应器
        /// </summary>
        /// <param name="groupReceiverInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        public static async Task<bool> TryRepeatCommandRespond(GroupMessageReceivedInfo groupReceiverInfo, GroupMessageReceiver groupMessageReceiver)
        {
            // 如果禁止复读，不响应
            if (groupReceiverInfo.Group.RepeatFrequency == 0) return false;
            // 判断特定词条响应
            switch (groupReceiverInfo.AllPlainMessage)
            {
                case "草":
                    {
                        if (groupReceiverInfo.Group.Cao == RepeatStatus.Idle)
                        {
                            if (UsefulMethods.IsOk(5))
                            {
                                groupReceiverInfo.Group.Cao = RepeatStatus.Done;
                                await DataBaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
                                await groupMessageReceiver.SendMessageAsync("草");
                                return true;
                            }
                        }
                        break;
                    }
                case "?":
                    {
                        if (groupReceiverInfo.Group.QuestionMark == RepeatStatus.Idle)
                        {
                            if (UsefulMethods.IsOk(5))
                            {
                                groupReceiverInfo.Group.QuestionMark = RepeatStatus.Done;
                                await DataBaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
                                await groupMessageReceiver.SendMessageAsync("?");
                            }
                        }
                        break;
                    }
                case "？":
                    {
                        if (groupReceiverInfo.Group.QuestionMark == RepeatStatus.Idle)
                        {
                            if (UsefulMethods.IsOk(5))
                            {
                                groupReceiverInfo.Group.QuestionMark = RepeatStatus.Done;
                                await DataBaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
                                await groupMessageReceiver.SendMessageAsync("？");
                            }
                        }
                        break;
                    }
            }
            // 判断随机复读响应
            if (UsefulMethods.IsOk((int)groupReceiverInfo.Group.RepeatFrequency))
            {
                await groupMessageReceiver.SendMessageAsync(groupReceiverInfo.PureMessageChain);
                return true;
            }
            return false;
        }
    }
}
