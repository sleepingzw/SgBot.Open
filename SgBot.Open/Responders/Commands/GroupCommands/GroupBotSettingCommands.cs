using System.Text.RegularExpressions;
using Manganese.Text;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Data.Shared;
using Mirai.Net.Utils.Scaffolds;
using SgBot.Open.DataTypes.BotFunction;
using SgBot.Open.Utils.Basic;
using SlpzLibrary;

namespace SgBot.Open.Responders.Commands.GroupCommands
{
    internal static partial class BotGroupCommands
    {
        /// <summary>
        /// bot设置的命令
        /// </summary>
        /// <param name="groupReceiverInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("傻狗set", "/sgset")]
        public static async Task BotSet(GroupMessageReceivedInfo groupReceiverInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceiver.Sender.Permission != Permissions.Member || groupReceiverInfo.IsOwner)
            {
                if (groupReceiverInfo.PlainMessages.Count == 3)
                {
                    var whatSetting = groupReceiverInfo.PlainMessages[1];
                    var settingPara = groupReceiverInfo.PlainMessages[2];
                    switch (whatSetting)
                    {
                        case "草":
                            switch (settingPara)
                            {
                                case "on":
                                    if (groupReceiverInfo.Group.Cao == RepeatStatus.Disabled)
                                    {
                                        groupReceiverInfo.Group.Cao = RepeatStatus.Idle;
                                        await DataBaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
                                        await groupMessageReceiver.QuoteMessageAsync("开启草复读成功");
                                        return;
                                    }

                                    await groupMessageReceiver.QuoteMessageAsync("无需重复开启草复读");
                                    return;
                                case "off":
                                    if (groupReceiverInfo.Group.Cao != RepeatStatus.Disabled)
                                    {
                                        groupReceiverInfo.Group.Cao = RepeatStatus.Disabled;
                                        await DataBaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
                                        await groupMessageReceiver.QuoteMessageAsync("关闭草复读成功");
                                        return;
                                    }

                                    await groupMessageReceiver.QuoteMessageAsync("无需重复关闭草复读");
                                    return;
                                default:
                                    await groupMessageReceiver.QuoteMessageAsync("参数错误");
                                    break;
                            }

                            break;
                        case "问号":
                            switch (settingPara)
                            {
                                case "on":
                                    if (groupReceiverInfo.Group.QuestionMark == RepeatStatus.Disabled)
                                    {
                                        groupReceiverInfo.Group.QuestionMark = RepeatStatus.Idle;
                                        await DataBaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
                                        await groupMessageReceiver.QuoteMessageAsync("开启问号复读成功");
                                        return;
                                    }

                                    await groupMessageReceiver.QuoteMessageAsync("无需重复开启问号复读");
                                    return;
                                case "off":
                                    if (groupReceiverInfo.Group.QuestionMark != RepeatStatus.Disabled)
                                    {
                                        groupReceiverInfo.Group.QuestionMark = RepeatStatus.Disabled;
                                        await DataBaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
                                        await groupMessageReceiver.QuoteMessageAsync("关闭问号复读成功");
                                        return;
                                    }

                                    await groupMessageReceiver.QuoteMessageAsync("无需重复关闭问号复读");
                                    return;
                                default:
                                    await groupMessageReceiver.QuoteMessageAsync("参数错误");
                                    break;
                            }

                            break;
                        case "复读":
                            switch (settingPara)
                            {
                                case "on":
                                    if (groupReceiverInfo.Group.RepeatFrequency == 0)
                                    {
                                        groupReceiverInfo.Group.RepeatFrequency = 200;
                                        await DataBaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
                                        await groupMessageReceiver.QuoteMessageAsync("已开启随机群复读,默认频率0.5%");
                                        return;
                                    }

                                    await groupMessageReceiver.QuoteMessageAsync("群随机复读处于开启状态,无需重复开启,需要调整复读频率请单独设置");
                                    return;
                                case "off":
                                    if (groupReceiverInfo.Group.RepeatFrequency != 0)
                                    {
                                        groupReceiverInfo.Group.RepeatFrequency = 0;
                                        await DataBaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
                                        await groupMessageReceiver.QuoteMessageAsync("已关闭随机群复读");
                                        return;
                                    }

                                    await groupMessageReceiver.QuoteMessageAsync("群随机复读处于关闭状态,无需重复关闭");
                                    return;
                                default:
                                    var temp = Regex.Replace(settingPara, @"[^0-9]+", "");
                                    if (temp.IsNullOrEmpty())
                                    {
                                        await groupMessageReceiver.QuoteMessageAsync("参数错误");
                                    }
                                    else
                                    {
                                        if (int.TryParse(temp, out var rf))
                                        {
                                            if (rf < 10)
                                            {
                                                await groupMessageReceiver.QuoteMessageAsync("参数不合法,应该大于10分之一");
                                            }

                                            groupReceiverInfo.Group.RepeatFrequency = rf;
                                            await DataBaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
                                            await groupMessageReceiver.QuoteMessageAsync($"设置成功,当前复读频率为{rf}分之一");
                                        }
                                        else
                                        {
                                            await groupMessageReceiver.QuoteMessageAsync("参数错误");
                                        }
                                    }

                                    break;
                            }

                            break;
                        case "群管":
                            switch (settingPara)
                            {
                                case "on":
                                    if (!groupReceiverInfo.Group.CanManage)
                                    {
                                        groupReceiverInfo.Group.CanManage = true;
                                        await DataBaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
                                        await groupMessageReceiver.QuoteMessageAsync("开启群管功能成功");
                                        return;
                                    }

                                    await groupMessageReceiver.QuoteMessageAsync("无需重复开启群管功能");
                                    return;
                                case "off":
                                    if (groupReceiverInfo.Group.CanManage)
                                    {
                                        groupReceiverInfo.Group.CanManage = false;
                                        await DataBaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
                                        await groupMessageReceiver.QuoteMessageAsync("关闭群管功能成功");
                                        return;
                                    }

                                    await groupMessageReceiver.QuoteMessageAsync("无需重复关闭群管功能");
                                    return;
                                default:
                                    await groupMessageReceiver.QuoteMessageAsync("参数错误");
                                    break;
                            }

                            break;
                        case "色图":
                            if (groupReceiverInfo.Member.Permission != Permission.User)
                            {
                                switch (settingPara)
                                {
                                    case "on":
                                        if (!groupReceiverInfo.Group.CanSetu)
                                        {
                                            groupReceiverInfo.Group.CanSetu = true;
                                            await DataBaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
                                            await groupMessageReceiver.QuoteMessageAsync("开启色图功能成功");
                                            return;
                                        }

                                        await groupMessageReceiver.QuoteMessageAsync("无需重复开启色图功能");
                                        return;
                                    case "off":
                                        if (groupReceiverInfo.Group.CanSetu)
                                        {
                                            groupReceiverInfo.Group.CanSetu = false;
                                            await DataBaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
                                            await groupMessageReceiver.QuoteMessageAsync("关闭色图功能成功");
                                            return;
                                        }

                                        await groupMessageReceiver.QuoteMessageAsync("无需重复关闭色图功能");
                                        return;
                                    default:
                                        await groupMessageReceiver.QuoteMessageAsync("参数错误");
                                        break;
                                }
                            }
                            else
                            {
                                await groupMessageReceiver.QuoteMessageAsync("权限不足");
                            }

                            break;
                        case "r18":
                            if (groupReceiverInfo.Member.Permission is Permission.Owner or Permission.SuperAdmin)
                            {
                                switch (settingPara)
                                {
                                    case "mix":
                                        if (groupReceiverInfo.Group.SetuR18Status != SetuR18Status.Enabled)
                                        {
                                            groupReceiverInfo.Group.SetuR18Status = SetuR18Status.Enabled;
                                            await DataBaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
                                            await groupMessageReceiver.QuoteMessageAsync("R18状态为混合模式");
                                            return;
                                        }

                                        await groupMessageReceiver.QuoteMessageAsync("无需重复设置R18状态为混合模式");
                                        return;
                                    case "off":
                                        if (groupReceiverInfo.Group.SetuR18Status != SetuR18Status.Disabled)
                                        {
                                            groupReceiverInfo.Group.SetuR18Status = SetuR18Status.Disabled;
                                            await DataBaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
                                            await groupMessageReceiver.QuoteMessageAsync("R18状态为青少年模式");
                                            return;
                                        }

                                        await groupMessageReceiver.QuoteMessageAsync("无需重复设置R18状态为青少年模式");
                                        return;
                                    case "only":
                                        if (groupReceiverInfo.Group.SetuR18Status != SetuR18Status.OnlyR18)
                                        {
                                            groupReceiverInfo.Group.SetuR18Status = SetuR18Status.OnlyR18;
                                            await DataBaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
                                            await groupMessageReceiver.QuoteMessageAsync("R18状态为色色模式");
                                            return;
                                        }

                                        await groupMessageReceiver.QuoteMessageAsync("无需重复设置R18状态为色色模式");
                                        return;
                                    default:
                                        await groupMessageReceiver.QuoteMessageAsync("参数错误");
                                        break;
                                }
                            }
                            else
                            {
                                await groupMessageReceiver.QuoteMessageAsync("权限不足");
                            }

                            break;
                        case "傻狗大陆":
                            switch (settingPara)
                            {
                                case "on":
                                    if (!groupReceiverInfo.Group.CanGame)
                                    {
                                        groupReceiverInfo.Group.CanGame = true;
                                        await DataBaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
                                        await groupMessageReceiver.QuoteMessageAsync("开启傻狗大陆功能成功");
                                        return;
                                    }

                                    await groupMessageReceiver.QuoteMessageAsync("无需重复开启傻狗大陆功能");
                                    return;
                                case "off":
                                    if (groupReceiverInfo.Group.CanGame)
                                    {
                                        groupReceiverInfo.Group.CanGame = false;
                                        await DataBaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
                                        await groupMessageReceiver.QuoteMessageAsync("关闭傻狗大陆功能成功");
                                        return;
                                    }

                                    await groupMessageReceiver.QuoteMessageAsync("无需重复关闭傻狗大陆功能");
                                    return;
                                default:
                                    await groupMessageReceiver.QuoteMessageAsync("参数错误");
                                    break;
                            }

                            break;
                    }
                }
                else
                {
                    await groupMessageReceiver.SendMessageAsync("设置参数错误");
                }
            }
            else
            {
                await groupMessageReceiver.SendMessageAsync("您无操作权限");
            }
        }
    }
}
