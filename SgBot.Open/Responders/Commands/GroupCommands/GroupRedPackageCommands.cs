﻿using Mirai.Net.Data.Messages.Receivers;
using SgBot.Open.DataTypes.BotFunction;
using SlpzLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirai.Net.Utils.Scaffolds;
using System.Text.RegularExpressions;
using SgBot.Open.DataTypes.Extra;
using SgBot.Open.Utils.Basic;
using Newtonsoft.Json.Linq;

namespace SgBot.Open.Responders.Commands.GroupCommands
{
    internal static partial class BotGroupCommands
    {
        [ChatCommand("发红包", "/sendredpackage")]
        public static async Task CreateRedPackage(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.PlainMessages.Count < 3)
            {
                await groupMessageReceiver.QuoteMessageAsync("参数不合法");
                return;
            }
            var token = int.Parse(Regex.Replace(groupMessageReceivedInfo.PlainMessages[1], @"[^0-9]+", ""));
            var amount = int.Parse(Regex.Replace(groupMessageReceivedInfo.PlainMessages[1], @"[^0-9]+", ""));
            if (groupMessageReceivedInfo.Member.Token < token)
            {
                await groupMessageReceiver.QuoteMessageAsync("你没有这么多傻狗力");
                return;
            }
            if (token < amount)
            {
                await groupMessageReceiver.QuoteMessageAsync("总傻狗力必须大于红包数量");
                return;
            }
            var id=RedPacketManager.CreatePackage(groupMessageReceivedInfo.Group.GroupId,token,amount);
            groupMessageReceivedInfo.Member.Token -= token;
            await DataBaseOperator.UpdateUserInfo(groupMessageReceivedInfo.Member);
            await groupMessageReceiver.QuoteMessageAsync($"发红包成功,红包id{id},红包总傻狗力{token},红包总数量{amount}");
        }

        [ChatCommand("抢红包", "/robredpackage")]
        public static async Task RobRedPackage(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.PlainMessages.Count < 2)
            {
                await groupMessageReceiver.QuoteMessageAsync("参数不合法");
                return;
            }
            var which = int.Parse(Regex.Replace(groupMessageReceivedInfo.PlainMessages[1], @"[^0-9]+", ""));
            var status = RedPacketManager.GetPackage(groupMessageReceivedInfo.Group.GroupId, which,
                groupMessageReceivedInfo.Member.UserId);
            switch (status)
            {
                case PackageStatus.CouldNotFind:
                    await groupMessageReceiver.QuoteMessageAsync("红包不存在或者参数不合法");
                    break;
                case PackageStatus.OneHaveGot:
                    await groupMessageReceiver.QuoteMessageAsync("你已经领过了这个红包");
                    break;
                case PackageStatus.Success:
                    var tokenGet = RedPacketManager.OpenPackage(groupMessageReceivedInfo.Group.GroupId, which,
                        groupMessageReceivedInfo.Member.UserId);
                    groupMessageReceivedInfo.Member.Token += tokenGet;
                    await DataBaseOperator.UpdateUserInfo(groupMessageReceivedInfo.Member);
                    await groupMessageReceiver.QuoteMessageAsync($"抢红包成功,获得{tokenGet}傻狗力");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}