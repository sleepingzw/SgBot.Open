using Mirai.Net.Data.Messages.Receivers;
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
        [ChatCommand("发红包", "/sendredbag")]
        public static async Task CreateRedPackage(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.PlainMessages.Count < 3)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "参数不合法", true));
                return;
            }
            var token = int.Parse(Regex.Replace(groupMessageReceivedInfo.PlainMessages[1], @"[^0-9]+", ""));
            var amount = int.Parse(Regex.Replace(groupMessageReceivedInfo.PlainMessages[2], @"[^0-9]+", ""));
            if (groupMessageReceivedInfo.Member.Token < token)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "你没有这么多傻狗力", true));
                return;
            }
            if (token < amount)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "总傻狗力必须大于红包数量", true));
                return;
            }
            var id=RedBagManager.CreateRedBag(groupMessageReceivedInfo.Group.GroupId,token,amount);
            groupMessageReceivedInfo.Member.Token -= token;
            await DataBaseOperator.UpdateUserInfo(groupMessageReceivedInfo.Member);
            // await groupMessageReceiver.QuoteMessageAsync($"发红包成功,红包id {id},红包总傻狗力{token},红包总数量{amount}");
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"发红包成功,红包id {id},红包总傻狗力{token},红包总数量{amount}", true));
        }

        [ChatCommand("抢红包", "/robredbag")]
        public static async Task RobRedPackage(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.PlainMessages.Count < 2)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "参数不合法", true));
                return;
            }
            var which = int.Parse(Regex.Replace(groupMessageReceivedInfo.PlainMessages[1], @"[^0-9]+", ""));
            var status = RedBagManager.GetRedBag(groupMessageReceivedInfo.Group.GroupId, which,
                groupMessageReceivedInfo.Member.UserId);
            switch (status)
            {
                case RedBagStatus.CouldNotFind:
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "红包不存在或者参数不合法", true));
                    break;
                case RedBagStatus.OneHaveGot:
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "你已经领过了这个红包", true));
                    break;
                case RedBagStatus.Success:
                    var tokenGet = RedBagManager.OpenRedBag(groupMessageReceivedInfo.Group.GroupId, which,
                        groupMessageReceivedInfo.Member.UserId);
                    groupMessageReceivedInfo.Member.Token += tokenGet;
                    await DataBaseOperator.UpdateUserInfo(groupMessageReceivedInfo.Member);
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"抢红包成功,获得{tokenGet}傻狗力", true));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
