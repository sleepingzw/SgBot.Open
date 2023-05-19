using System.Text.RegularExpressions;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Data.Shared;
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
        [ChatCommand("傻狗mute", "/mute")]
        public static async Task Mute(GroupMessageReceivedInfo groupMessageReceivedInfo, GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.Group.CanManage)
            {
                if (groupMessageReceiver.Sender.Permission != Permissions.Member || groupMessageReceivedInfo.IsOwner)
                {
                    if (groupMessageReceiver.BotPermission != Permissions.Member)
                    {
                        if (groupMessageReceivedInfo.AtMessages.Count == 0) return;
                        var target = groupMessageReceivedInfo.AtMessages[0].Target;
                        if (target == StaticData.BotConfig.OwnerQQ || target == StaticData.BotConfig.BotQQ)
                        {
                            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "?", true));
                            return;
                        }
                        var mem = await GroupManager.GetMemberAsync(target, groupMessageReceiver.GroupId);
                        if (mem != null)
                        {
                            if (groupMessageReceivedInfo.PlainMessages.Count < 2) return;
                            if (mem.Permission == Permissions.Member)
                            {
                                var timetp = Regex.Replace(groupMessageReceivedInfo.PlainMessages[1], @"[^0-9]+", "");
                                if (timetp != "")
                                {
                                    var time = int.Parse(timetp);
                                    if (time is <= 0 or > 43199)
                                    {
                                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "时间超出阈值", true));
                                        //await groupMessageReceiver.QuoteMessageAsync("时间超出阈值");
                                    }

                                    var ts = new TimeSpan(0, time, 0);
                                    await GroupManager.MuteAsync(target, groupMessageReceiver.GroupId, ts);
                                    // await groupMessageReceiver.QuoteMessageAsync($"已禁言{mem.Name} {time} 分钟");
                                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"已禁言{mem.Name} {time} 分钟", true));
                                }
                                else
                                {
                                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "指令错误", true));
                                }
                            }
                            else
                            {
                                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "目标权限过高", true));
                            }
                        }
                    }
                    else
                    {
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "bot权限不足", true));
                    }
                }
                else
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "您无操作权限", true));
                }
            }
        }

        [ChatCommand("傻狗unmute", "/unmute")]
        public static async Task UnMute(GroupMessageReceivedInfo groupMessageReceivedInfo, GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.Group.CanManage)
            {
                if (groupMessageReceiver.Sender.Permission != Permissions.Member || groupMessageReceivedInfo.IsOwner)
                {
                    if (groupMessageReceiver.BotPermission != Permissions.Member)
                    {
                        if (groupMessageReceivedInfo.AtMessages.Count == 0) return;
                        var target = groupMessageReceivedInfo.AtMessages[0].Target;
                        var mem = await GroupManager.GetMemberAsync(target, groupMessageReceiver.GroupId);
                        if (mem != null)
                        {
                            if (mem.Permission == Permissions.Member)
                            {
                                await GroupManager.UnMuteAsync(target, groupMessageReceiver.GroupId);
                                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"解除 {mem.Name} 禁言成功", true));
                            }
                            else
                            {
                                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "目标权限过高", true));
                            }
                        }
                    }
                    else
                    {
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "bot权限不足", true));
                    }
                }
                else
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "您无操作权限", true));
                }
            }
        }

        [ChatCommand("傻狗muteall", "/muteall")]
        public static async Task MuteAll(GroupMessageReceivedInfo groupMessageReceivedInfo, GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.Group.CanManage)
            {
                if (groupMessageReceiver.Sender.Permission != Permissions.Member || groupMessageReceivedInfo.IsOwner)
                {
                    if (groupMessageReceiver.BotPermission != Permissions.Member)
                    {
                        await GroupManager.MuteAllAsync(groupMessageReceiver.GroupId);
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "全体禁言成功", true));
                    }
                    else
                    {
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "bot权限不足", true));
                    }
                }
                else
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "您无操作权限", true));
                }
            }
        }

        [ChatCommand("傻狗unmuteall", "/unmuteall")]
        public static async Task UnMuteAll(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.Group.CanManage)
            {
                if (groupMessageReceiver.Sender.Permission != Permissions.Member || groupMessageReceivedInfo.IsOwner)
                {
                    if (groupMessageReceiver.BotPermission != Permissions.Member)
                    {
                        await GroupManager.MuteAllAsync(groupMessageReceiver.GroupId, false);
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "解除全体禁言成功", true));
                    }
                    else
                    {
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "bot权限不足", true));
                    }
                }
                else
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "您无操作权限", true));
                }
            }
        }

        [ChatCommand("傻狗kick", "/kick")]
        public static async Task Kick(GroupMessageReceivedInfo groupMessageReceivedInfo, GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.Group.CanManage)
            {
                if (groupMessageReceiver.Sender.Permission != Permissions.Member || groupMessageReceivedInfo.IsOwner)
                {
                    if (groupMessageReceiver.BotPermission != Permissions.Member)
                    {
                        if (groupMessageReceivedInfo.AtMessages.Count == 0) return;
                        var target = groupMessageReceivedInfo.AtMessages[0].Target;
                        if (target == StaticData.BotConfig.OwnerQQ || target == StaticData.BotConfig.BotQQ)
                        {
                            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "?", true));
                            return;
                        }

                        var mem = await GroupManager.GetMemberAsync(target, groupMessageReceiver.GroupId);
                        if (mem != null)
                        {
                            if (mem.Permission == Permissions.Member)
                            {
                                await GroupManager.KickAsync(target, groupMessageReceiver.GroupId);
                                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"献 中 {mem.Name} 成 功", true));
                            }
                            else
                            {
                                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "目标权限过高", true));
                            }
                        }
                    }
                    else
                    {
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "bot权限不足", true));
                    }
                }
                else
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "您无操作权限", true));
                }
            }
        }
    }
}
