using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using SgBot.Open.DataTypes.SgGame;
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
using SgBot.Open.Utils.Extra;
using Mirai.Net.Sessions.Http.Managers;

namespace SgBot.Open.Responders.Commands.GameCommands
{
    public static partial class BotGameCommands
    {
        /// <summary>
        /// PVE战斗处理
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "pve" }, new string[] { "/game.pve", "/g.pve" }, true)]
        public static async Task Pve(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            var times = 1;
            if (groupMessageReceivedInfo.PlainMessages.Count > 1)
            {
                var temp = Regex.Replace(groupMessageReceivedInfo.PlainMessages[1], @"[^0-9]+", "");
                if (temp != "")
                {
                    times = int.Parse(temp);
                }
                else
                {
                    await groupMessageReceiver.QuoteMessageAsync("参数错误");
                    return;
                }
            }
            if (times > 20)
            {
                await groupMessageReceiver.QuoteMessageAsync("最多一次20把");
                return;
            }
            var player = await DataBaseOperator.FindPlayer(groupMessageReceivedInfo.Member.UserId);
            player.Refresh();
            if (player.Power >= times)
            {
                player.Power -= times;
                var originLevel = player.Level;
                var originCoin = player.Coin;

                var result = new SgGamePveResult();
                result.MakeResult(times, player);

                var allCoinGet = player.Coin - originCoin;
                var levelUp = player.Level - originLevel;

                player.SortBag();
                await DataBaseOperator.UpdatePlayer(player);

                var pic = GameImageMaker.MakeSgGamePveImage(player, result, allCoinGet, levelUp);

                var id = await FileManager.UploadImageAsync(pic);
                var chain = new MessageChainBuilder().ImageFromId(id.Item1).Build();

                await groupMessageReceiver.SendMessageAsync(chain);
                TaskHolder.DeleteTask(pic);
            }
            else
            {
                await groupMessageReceiver.QuoteMessageAsync("体力不足");
            }
        }
    }
}
