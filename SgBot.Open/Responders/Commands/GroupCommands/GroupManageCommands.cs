using System.Text.RegularExpressions;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Data.Shared;
using Mirai.Net.Sessions.Http.Managers;
using Mirai.Net.Utils.Scaffolds;
using SgBot.Open.DataTypes.BotFunction;
using SgBot.Open.DataTypes.StaticData;
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
                            await groupMessageReceiver.QuoteMessageAsync("?");
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
                                        await groupMessageReceiver.QuoteMessageAsync("时间超出阈值");
                                    }

                                    var ts = new TimeSpan(0, time, 0);
                                    await GroupManager.MuteAsync(target, groupMessageReceiver.GroupId, ts);
                                    await groupMessageReceiver.QuoteMessageAsync($"已禁言{mem.Name} {time} 分钟");
                                }
                                else
                                {
                                    await groupMessageReceiver.QuoteMessageAsync("指令错误");
                                }
                            }
                            else
                            {
                                await groupMessageReceiver.QuoteMessageAsync("目标权限过高");
                            }
                        }
                    }
                    else
                    {
                        await groupMessageReceiver.QuoteMessageAsync("bot权限不足");
                    }
                }
                else
                {
                    await groupMessageReceiver.QuoteMessageAsync("您无操作权限");
                }
            }
        }

        [ChatCommand("傻狗unmute", "/unmute")]
        public static async Task UnMute(GroupMessageReceivedInfo GroupMessageReceivedInfo, GroupMessageReceiver groupMessageReceiver)
        {
            if (GroupMessageReceivedInfo.Group.CanManage)
            {
                if (groupMessageReceiver.Sender.Permission != Permissions.Member || GroupMessageReceivedInfo.IsOwner)
                {
                    if (groupMessageReceiver.BotPermission != Permissions.Member)
                    {
                        if (GroupMessageReceivedInfo.AtMessages.Count == 0) return;
                        var target = GroupMessageReceivedInfo.AtMessages[0].Target;
                        var mem = await GroupManager.GetMemberAsync(target, groupMessageReceiver.GroupId);
                        if (mem != null)
                        {
                            if (mem.Permission == Permissions.Member)
                            {
                                await GroupManager.UnMuteAsync(target, groupMessageReceiver.GroupId);
                                await groupMessageReceiver.QuoteMessageAsync($"解除 {mem.Name} 禁言成功");
                            }
                            else
                            {
                                await groupMessageReceiver.QuoteMessageAsync("目标权限过高");
                            }
                        }
                    }
                    else
                    {
                        await groupMessageReceiver.QuoteMessageAsync("bot权限不足");
                    }
                }
                else
                {
                    await groupMessageReceiver.QuoteMessageAsync("您无操作权限");
                }
            }
        }

        [ChatCommand("傻狗muteall", "/muteall")]
        public static async Task MuteAll(GroupMessageReceivedInfo GroupMessageReceivedInfo, GroupMessageReceiver groupMessageReceiver)
        {
            if (GroupMessageReceivedInfo.Group.CanManage)
            {
                if (groupMessageReceiver.Sender.Permission != Permissions.Member || GroupMessageReceivedInfo.IsOwner)
                {
                    if (groupMessageReceiver.BotPermission != Permissions.Member)
                    {
                        await GroupManager.MuteAllAsync(groupMessageReceiver.GroupId);
                        await groupMessageReceiver.QuoteMessageAsync("全体禁言成功");
                    }
                    else
                    {
                        await groupMessageReceiver.QuoteMessageAsync("bot权限不足");
                    }
                }
                else
                {
                    await groupMessageReceiver.QuoteMessageAsync("您无操作权限");
                }
            }
        }

        [ChatCommand("傻狗unmuteall", "/unmuteall")]
        public static async Task UnMuteAll(GroupMessageReceivedInfo GroupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (GroupMessageReceivedInfo.Group.CanManage)
            {
                if (groupMessageReceiver.Sender.Permission != Permissions.Member || GroupMessageReceivedInfo.IsOwner)
                {
                    if (groupMessageReceiver.BotPermission != Permissions.Member)
                    {
                        await GroupManager.MuteAllAsync(groupMessageReceiver.GroupId, false);
                        await groupMessageReceiver.QuoteMessageAsync("解除全体禁言成功");
                    }
                    else
                    {
                        await groupMessageReceiver.QuoteMessageAsync("bot权限不足");
                    }
                }
                else
                {
                    await groupMessageReceiver.QuoteMessageAsync("您无操作权限");
                }
            }
        }

        [ChatCommand("傻狗kick", "/kick")]
        public static async Task Kick(GroupMessageReceivedInfo GroupMessageReceivedInfo, GroupMessageReceiver groupMessageReceiver)
        {
            if (GroupMessageReceivedInfo.Group.CanManage)
            {
                if (groupMessageReceiver.Sender.Permission != Permissions.Member || GroupMessageReceivedInfo.IsOwner)
                {
                    if (groupMessageReceiver.BotPermission != Permissions.Member)
                    {
                        if (GroupMessageReceivedInfo.AtMessages.Count == 0) return;
                        var target = GroupMessageReceivedInfo.AtMessages[0].Target;
                        if (target == StaticData.BotConfig.OwnerQQ || target == StaticData.BotConfig.BotQQ)
                        {
                            await groupMessageReceiver.QuoteMessageAsync("?");
                            return;
                        }

                        var mem = await GroupManager.GetMemberAsync(target, groupMessageReceiver.GroupId);
                        if (mem != null)
                        {
                            if (mem.Permission == Permissions.Member)
                            {
                                await GroupManager.KickAsync(target, groupMessageReceiver.GroupId);
                                await groupMessageReceiver.QuoteMessageAsync($"献 中 {mem.Name} 成 功");
                            }
                            else
                            {
                                await groupMessageReceiver.QuoteMessageAsync("目标权限过高");
                            }
                        }
                    }
                    else
                    {
                        await groupMessageReceiver.QuoteMessageAsync("bot权限不足");
                    }
                }
                else
                {
                    await groupMessageReceiver.QuoteMessageAsync("您无操作权限");
                }
            }
        }
    }
}
