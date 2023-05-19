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
        /// 给角色加点
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "加点" }, new string[] { "/game.point", "/g.point" }, true)]
        public static async Task AllotPoint(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.PlainMessages.Count == 3)
            {
                var player = await DataBaseOperator.FindPlayer(groupMessageReceivedInfo.Member.UserId);
                player.Refresh();
                var what = groupMessageReceivedInfo.PlainMessages[1];
                var points = Regex.Replace(groupMessageReceivedInfo.PlainMessages[2], @"[^0-9]+", "");
                if (points.IsNullOrEmpty())
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "参数错误", true));
                    return;
                }
                var point = int.Parse(points);
                if (point > player.FreePoints)
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "自由属性点不足", true));
                    return;
                }
                switch (what)
                {
                    case "体质":
                        player.Fitness += point;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"{player.Name}({player.Id})加点体质 {point} 成功", true));
                        break;
                    case "敏捷":
                        player.Agility += point;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"{player.Name}({player.Id})加点敏捷 {point} 成功", true));
                        break;
                    case "力量":
                        player.Strength += point;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"{player.Name}({player.Id})加点力量 {point} 成功", true));
                        break;
                    case "智力":
                        player.Intelligence += point;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"{player.Name}({player.Id})加点智力 {point} 成功", true));
                        break;
                    case "fit":
                        player.Fitness += point;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"{player.Name}({player.Id})加点体质 {point} 成功", true));
                        break;
                    case "agi":
                        player.Agility += point;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"{player.Name}({player.Id})加点敏捷 {point} 成功", true));
                        break;
                    case "str":
                        player.Strength += point;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"{player.Name}({player.Id})加点力量 {point} 成功", true));
                        break;
                    case "int":
                        player.Intelligence += point;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"{player.Name}({player.Id})加点智力 {point} 成功", true));
                        break;
                    default:
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "参数错误", true));
                        return;
                }
                player.FreePoints -= point;
                await DataBaseOperator.UpdatePlayer(player);
            }
            else
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "参数错误", true));
            }
        }
        /// <summary>
        /// 给角色洗点
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "洗点" }, new string[] { "/game.re", "/g.re" }, true)]
        public static async Task ReBirth(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            var player = await DataBaseOperator.FindPlayer(groupMessageReceivedInfo.Member.UserId);
            player.Refresh();
            if (player.Coin < 100 * player.Level)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "金币不足 需要 100*玩家等级 金币洗点", true));
                return;
            }

            player.Coin -= 100 * player.Level;
            player.Level -= (int)(player.Level * 0.1);
            player.FreePoints = player.Level - 1;
            player.Agility = 1;
            player.Strength = 1;
            player.Fitness = 1;
            player.Intelligence = 1;

            await DataBaseOperator.UpdatePlayer(player);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"洗点成功 洗点后等级 Lv.{player.Level} 自由属性点 {player.FreePoints}点", true));
        }
    }
}
