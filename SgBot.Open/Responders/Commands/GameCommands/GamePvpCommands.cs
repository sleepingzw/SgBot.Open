using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using SgBot.Open.DataTypes.SgGame;
using SgBot.Open.Utils.Basic;
using SgBot.Open.Utils.Extra;
using SlpzLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirai.Net.Utils.Scaffolds;
using SgBot.Open.DataTypes.BotFunction;
using SgBot.Open.DataTypes.StaticData;
using Mirai.Net.Sessions.Http.Managers;

namespace SgBot.Open.Responders.Commands.GameCommands
{
    public static partial class BotGameCommands
    {
        /// <summary>
        /// 进行段位狠狠匹配
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "pvp" }, new string[] { "/game.pvp", "/g.pvp" }, true)]
        public static async Task PvpFight(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            var player = await DataBaseOperator.FindPlayer(groupMessageReceivedInfo.Member.UserId);
            player.Refresh();
            if (player.Power < 1)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "体力不足", true));
                return;
            }

            player.Power--;
            var battle = new Battle();
            var enemy = DataBaseOperator.FindPlayerByRank(player.Rank, player.Id);
            var log = battle.MakeBattle(player, enemy);
            SgGamePvpResult result;
            if (log.IsWin)
            {
                if (!player.IsWinToday)
                {
                    // Logger.Log("shou sheng",LogLevel.Simple);
                    player.IsWinToday = true;
                    switch (player.Rank)
                    {
                        case Rank.D:
                            groupMessageReceivedInfo.Member.Card += 1;
                            break;
                        case Rank.C:
                            groupMessageReceivedInfo.Member.Card += 2;
                            break;
                        case Rank.B:
                            groupMessageReceivedInfo.Member.Card += 3;
                            break;
                        case Rank.A:
                            groupMessageReceivedInfo.Member.Card += 4;
                            break;
                        case Rank.AA:
                            groupMessageReceivedInfo.Member.Card += 5;
                            break;
                        case Rank.M:
                            groupMessageReceivedInfo.Member.Card += 6;
                            break;
                        case Rank.GM:
                            groupMessageReceivedInfo.Member.Card += 7;
                            break;
                        default:
                            break;
                    }
                }
                var expGet = (long)((player.Level + enemy.Level) * 8 * UsefulMethods.MakeRandom(110, 90) / 100);
                var coinGet = (long)((player.Level + enemy.Level) * 1.3 * UsefulMethods.MakeRandom(110, 90) / 100);
                var rankGet = player.RankScore > enemy.RankScore ? 10 : 15;
                if (player.Rank != Rank.D)
                {
                    rankGet *= (int)player.Rank;
                }

                var isUp = player.GetRankScore(rankGet);
                var isLevelUp = player.GetExp(expGet);
                player.Coin += coinGet;
                result = new SgGamePvpResult()
                {
                    CoinGet = coinGet,
                    ExpGet = expGet,
                    IsRankChange = isUp,
                    RankScoreChange = rankGet,
                    IsUpgrade = isLevelUp
                };
            }
            else
            {
                var expGet = (long)((player.Level + enemy.Level) * 5 * UsefulMethods.MakeRandom(110, 90) / 100);
                var coinGet = (long)((player.Level + enemy.Level) * 0.6 * UsefulMethods.MakeRandom(110, 90) / 100);
                var rankLost = player.RankScore > enemy.RankScore ? 15 : 10;
                if (player.Rank != Rank.D)
                {
                    rankLost *= (int)player.Rank;
                }

                var isDown = player.LostRankScore(rankLost);
                var isLevelUp = player.GetExp(expGet);
                player.Coin += coinGet;
                result = new SgGamePvpResult()
                {
                    CoinGet = coinGet,
                    ExpGet = expGet,
                    IsRankChange = isDown,
                    RankScoreChange = rankLost,
                    IsUpgrade = isLevelUp
                };
            }
            await DataBaseOperator.UpdateUserInfo(groupMessageReceivedInfo.Member);
            await DataBaseOperator.UpdatePlayer(player);
            var pic = GameImageMaker.MakePVPBattleImage(log, player.Id, result);

            var id = await FileManager.UploadImageAsync(pic);
            var chain = new MessageChainBuilder().ImageFromId(id.Item1).Build();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, chain));
            TaskHolder.DeleteTask(pic);
        }

        /// <summary>
        /// 和指定目标狠狠滴击剑
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "决斗" }, new string[] { "/game.duel", "/g.duel" }, true)]
        public static async Task Duel(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.AtMessages.Count == 0)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "没有决斗对象", true));
                return;
            }

            var target = groupMessageReceivedInfo.AtMessages[0].Target;

            var player = await DataBaseOperator.FindPlayer(groupMessageReceivedInfo.Member.UserId);
            player.Refresh();
            var battle = new Battle();
            var enemy = await DataBaseOperator.FindPlayer(target);
            var log = battle.MakeBattle(player, enemy);
            var result = new SgGamePvpResult()
            {
                CoinGet = 0,
                ExpGet = 0,
                IsRankChange = false,
                RankScoreChange = 0,
                IsUpgrade = false
            };
            var pic = GameImageMaker.MakePVPBattleImage(log, player.Id, result);
            var id = await FileManager.UploadImageAsync(pic);
            var chain = new MessageChainBuilder().ImageFromId(id.Item1).Build();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, chain));
            TaskHolder.DeleteTask(pic);
        }
    }

}
