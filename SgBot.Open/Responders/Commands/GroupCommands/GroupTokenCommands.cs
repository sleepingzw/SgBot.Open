﻿using System.Text.RegularExpressions;
using Manganese.Text;
using Mirai.Net.Data.Messages.Receivers;
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
        /// <summary>
        /// 签到获取傻狗力
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("傻狗签到", "/sign")]
        public static async Task Sign(GroupMessageReceivedInfo groupMessageReceivedInfo, GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.Member.FeedTime != DateTime.Now.Day)
            {
                groupMessageReceivedInfo.Member.FeedTime = DateTime.Now.Day;
                var tokenAdd = 9 + (int)UsefulMethods.MakeRandom(6, 0) + (int)UsefulMethods.MakeRandom(6, 0);
                groupMessageReceivedInfo.Member.Token += tokenAdd;
                await DataBaseOperator.UpdateUserInfo(groupMessageReceivedInfo.Member);
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver,
                    $"{groupMessageReceivedInfo.Member.Nickname} 签到成功,增加了{tokenAdd}傻狗力,现在您有{groupMessageReceivedInfo.Member.Token}傻狗力了"));
            }
            else
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver,
                    $"{groupMessageReceivedInfo.Member.Nickname},今天你已经签过到了,爪巴"));
            }
        }
        /// <summary>
        /// 提高自己的权限
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("权限升级", "/upgradeoneself")]
        public static async Task UpgradeOneself(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            switch (groupMessageReceivedInfo.Member.Permission)
            {
                case Permission.User:
                    if (groupMessageReceivedInfo.Member.Token > 1000)
                    {
                        groupMessageReceivedInfo.Member.Permission = Permission.Admin;
                        groupMessageReceivedInfo.Member.Token -= 1000;
                        // await groupMessageReceiver.QuoteMessageAsync("你的权限已提高至1级");
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "你的权限已提高至1级", true));
                        await DataBaseOperator.UpdateUserInfo(groupMessageReceivedInfo.Member);
                        return;
                    }
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "你的傻狗力不足1000", true));
                    // await groupMessageReceiver.QuoteMessageAsync("你的傻狗力不足1000");
                    return;
                case Permission.Admin:
                    if (groupMessageReceivedInfo.Member.Token > 2000)
                    {
                        groupMessageReceivedInfo.Member.Permission = Permission.SuperAdmin;
                        groupMessageReceivedInfo.Member.Token -= 2000;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "你的权限已提高至2级", true));
                        await DataBaseOperator.UpdateUserInfo(groupMessageReceivedInfo.Member);
                        return;
                    }

                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "你的傻狗力不足2000", true));
                    return;
                default:
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "无效指令", true));
                    break;
            }
        }
        /// <summary>
        /// 查询自己有多少傻狗力
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "查询傻狗力", "傻狗力查询" }, "/inquiretoken")]
        public static async Task TokenSearch(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver,
                $"{groupMessageReceivedInfo.Member.Nickname},您的傻狗力数量为{groupMessageReceivedInfo.Member.Token}"));
            //await groupMessageReceiver.SendMessageAsync($"{groupMessageReceivedInfo.Member.Nickname},您的傻狗力数量为{groupMessageReceivedInfo.Member.Token}");
        }
        /// <summary>
        /// 查询自己有多少傻狗牌
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "查询傻狗牌", "傻狗牌查询" }, "/inquirecard")]
        public static async Task CardSearch(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver,
                $"{groupMessageReceivedInfo.Member.Nickname},您的傻狗牌数量为{groupMessageReceivedInfo.Member.Card}张"));
        }
        /// <summary>
        /// 将傻狗力转给其他人
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("傻狗力转账", "/vtk")]
        public static async Task TokenTransfer(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.AtMessages.Count == 0) return;
            var target = groupMessageReceivedInfo.AtMessages[0].Target;
            if (target == StaticData.BotConfig.BotQQ)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "傻狗的傻狗力是无限的哦", true));
                return;
            }
            if (target == groupMessageReceivedInfo.Member.UserId)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "你不能给自己转账", true));
                return;
            }
            if (groupMessageReceivedInfo.PlainMessages.Count < 2) return;
            var stringAmount = Regex.Replace(groupMessageReceivedInfo.PlainMessages[1], @"[^0-9]+", "");
            if (stringAmount.IsNullOrEmpty())
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "参数错误", true));
                return;
            }

            if (long.TryParse(stringAmount, out var amount))
            {
                if (amount <= 0)
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "转账数额不合法", true));
                    return;
                }
                if (amount <= groupMessageReceivedInfo.Member.Token)
                {
                    groupMessageReceivedInfo.Member.Token -= amount;
                    await DataBaseOperator.UpdateUserInfo(groupMessageReceivedInfo.Member);
                    var targetUser = await DataBaseOperator.FindUser(target);
                    targetUser.Token += amount;
                    await DataBaseOperator.UpdateUserInfo(targetUser);
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver,
                        $"成功为({targetUser.Nickname})转入 {amount} 傻狗力", true));
                    // await groupMessageReceiver.QuoteMessageAsync($"成功为({targetUser.Nickname})转入 {amount} 傻狗力");
                    return;
                }

                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "您没有这么多傻狗力", true));
                return;
            }

            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "参数错误", true));

        }
        /// <summary>
        /// 抽傻狗牌
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "翻傻狗牌", "随机狗牌" }, "/drawdogcard")]
        public static async Task DrawDogCard(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.Member.Card == 0)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "你没有傻狗牌可以翻,每日傻狗大陆pvp首胜可以获得傻狗牌", true));
                return;
            }
            if (groupMessageReceivedInfo.Member.Token < 90)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "你没有足够傻狗力，开启一次傻狗牌需要90傻狗力", true));
                return;
            }
            var rd = new Random((int)DateTime.Now.Ticks);
            var cardList = new int[9];
            for (var i = 0; i < 9; i++)
            {
                cardList[i] = rd.Next() % 1000;
            }
            var tokenGet = 0;
            for (var i = 0; i < 9; i++)
            {
                tokenGet += cardList[i] switch
                {
                    < 600 => 1,
                    < 850 => 10,
                    < 950 => 40,
                    < 990 => 100,
                    _ => 1000
                };
            }
            groupMessageReceivedInfo.Member.Token -= 90;
            groupMessageReceivedInfo.Member.Token += tokenGet;
            groupMessageReceivedInfo.Member.Card -= 1;
            await DataBaseOperator.UpdateUserInfo(groupMessageReceivedInfo.Member);
            var ret = ImageMaker.MakeCardImage(groupMessageReceivedInfo.Member.UserId, cardList);
            var id = await FileManager.UploadImageAsync(ret);
            var chain = new MessageChainBuilder().ImageFromId(id.Item1)
                .Plain($"{groupMessageReceivedInfo.Member.Nickname},消耗90傻狗力翻牌,翻出{tokenGet}傻狗力,你现在有{groupMessageReceivedInfo.Member.Token}傻狗力了").Build();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, chain));

            TaskHolder.DeleteTask(ret);
        }
    }
}
