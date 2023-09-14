using SgBot.Open.DataTypes.BotFunction;
using SlpzLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Manganese.Array;
using Microsoft.EntityFrameworkCore;
using SgBot.Open.DataTypes.SgGame;
using Manganese.Text;

namespace SgBot.Open.Utils.Basic
{
    internal static class DataBaseOperator
    {
        /// <summary>
        /// 根据群号搜索数据库中的群，没有搜索到就添加条目
        /// </summary>
        /// <param name="groupId">群ID</param>
        /// <returns></returns>
        public static async Task<GroupInfo> FindGroup(string groupId)
        {
            var db = new DataBaseContext();
            var search = await db.Groups.SingleOrDefaultAsync(x => x.GroupId == groupId);
            var ret = new GroupInfo("temp");
            if (search == null)
            {
                ret = new GroupInfo(groupId);
                db.Groups.Add(ret);
                await db.SaveChangesAsync();
                Logger.Log("收录新群聊信息\n" + DataOperator.ToJsonString(ret, true), LogLevel.Important);
            }
            else
            {
                ret = search;
            }
            return ret;
        }
        /// <summary>
        /// 根据qq号搜索数据库中的用户，没有搜索到就添加条目
        /// </summary>
        /// <param name="userId">qq号</param>
        /// <returns></returns>
        public static async Task<UserInfo> FindUser(string userId)
        {
            var db = new DataBaseContext();
            var search = await db.Users.SingleOrDefaultAsync(x => x.UserId == userId);
            var ret = new UserInfo("temp");
            if (search == null)
            {
                ret = new UserInfo(userId);
                db.Users.Add(ret);
                await db.SaveChangesAsync();
                Logger.Log("收录新成员信息\n" + DataOperator.ToJsonString(ret, true), LogLevel.Important);
            }
            else
            {
                ret = search;
            }
            return ret;
        }
        /// <summary>
        /// 根据qq号搜索数据库中的玩家，没有搜索到就添加条目
        /// </summary>
        /// <param name="playerId">玩家的qq号</param>
        /// <returns></returns>
        public static async Task<Player> FindPlayer(string playerId)
        {
            await using var db = new DataBaseContext();
            var search = await db.Players.SingleOrDefaultAsync(x => x.Id == playerId);
            var ret = new Player("temp");
            if (search == null)
            {
                ret = new Player(playerId);
                var user = await FindUser(playerId);
                if (!user.Nickname.IsNullOrEmpty())
                {
                    var u = await FindUser(playerId);
                    ret.Name = u.Nickname;
                }
                ret.MakeString();
                db.Players.Add(ret);
                await db.SaveChangesAsync();
                Logger.Log("收录新玩家信息\n" + DataOperator.ToJsonString(ret, true), LogLevel.Important);
            }
            else
            {
                search.AnalyseString();
                search.SortBag();
                ret = search;
            }
            return ret;
        }
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public static async Task UpdateUserInfo(UserInfo userInfo)
        {
            await using (var db = new DataBaseContext())
            {
                db.Users.Update(userInfo);
                // var temp = db.Users.Single(x => x.UserId == userInfo.UserId);
                // temp = userInfo;
                await db.SaveChangesAsync();
            }
            Logger.Log("更新用户信息 " + userInfo.UserId, LogLevel.Simple);
        }
        /// <summary>
        /// 更新群聊信息
        /// </summary>
        /// <param name="groupInfo"></param>
        /// <returns></returns>
        public static async Task UpdateGroupInfo(GroupInfo groupInfo)
        {
            await using (var db = new DataBaseContext())
            {
                db.Groups.Update(groupInfo);
                // var temp = db.Groups.Single(x => x.GroupId == groupInfo.GroupId);
                // temp = groupInfo;
                await db.SaveChangesAsync();
            }
            Logger.Log("更新群聊信息 " + groupInfo.GroupId, LogLevel.Simple);
        }
        /// <summary>
        /// 更新玩家信息
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static async Task UpdatePlayer(Player player)
        {
            player.MakeString();
            await using (var db = new DataBaseContext())
            {
                db.Players.Update(player);
                await db.SaveChangesAsync();
            }
            Logger.Log("更新玩家信息 " + player.Id, LogLevel.Simple);
        }
        /// <summary>
        /// 根据段位寻找玩家
        /// </summary>
        /// <param name="rank">段位</param>
        /// <param name="except">除了哪一位，一般是除去玩家自己</param>
        /// <returns></returns>
        public static Player FindPlayerByRank(Rank rank, string except)
        {
            var db = new DataBaseContext();
            var player = db.Players.Where(x => x.Rank == rank && x.Id != except).Random();
            player.AnalyseString();
            return player;
        }
        /// <summary>
        /// 输出段位在M以上的前10名
        /// </summary>
        /// <returns></returns>
        public static List<string> OutGameRank()
        {
            var db = new DataBaseContext();
            var ret = new List<string>();
            var playerList = new List<Player>();
            var Gm = db.Players.Where(x => x.Rank == Rank.GM);
            if (Gm.Any())
            {
                var temp = Gm.ToList();
                var tt = new List<Player>();
                foreach (var item in temp)
                {
                    if (long.Parse(item.Id) > 0)
                    {
                        tt.Add(item);
                    }
                }
                tt.Sort();
                playerList.AddRange(tt);
            }
            if (playerList.Count < 10)
            {
                var m = db.Players.Where(x => x.Rank == Rank.M);
                var temp = m.ToList();
                var tt = new List<Player>();
                foreach (var item in temp)
                {
                    if (long.Parse(item.Id) > 0)
                    {
                        // Console.WriteLine(item.Id+" "+item.RankScore.ToString());
                        tt.Add(item);
                    }
                }
                var tempp = tt.OrderByDescending(rs => rs.RankScore).ToList();
                playerList.AddRange(tempp);
            }
            if (playerList.Count > 0)
            {
                var i = 0;
                foreach (var p in playerList)
                {
                    if (i >= 10)
                        break;
                    ret.Add("第" + $"{i + 1}".PadRight(2) + $"位:{p.Name},段位{p.Rank}");
                    i++;
                }
            }
            else
            {
                ret.Add("傻狗之巅无人问津……");
            }
            return ret;
        }
        /// <summary>
        /// 查看傻狗大陆的段位情况，去掉机器人
        /// </summary>
        /// <returns></returns>
        public static string OutGameStatus()
        {
            var db = new DataBaseContext();
            var Gm = db.Players.Where(x => x.Rank == Rank.GM);
            var gm1 = Gm.Count() - 1;
            var M = db.Players.Where(x => x.Rank == Rank.M);
            var m1 = M.Count() - 1;
            var aa = db.Players.Where(x => x.Rank == Rank.AA);
            var aa1 = aa.Count() - 1;
            var a = db.Players.Where(x => x.Rank == Rank.A);
            var a1 = a.Count() - 1;
            var b = db.Players.Where(x => x.Rank == Rank.B);
            var b1 = b.Count() - 2;
            var c = db.Players.Where(x => x.Rank == Rank.C);
            var c1 = c.Count() - 4;
            var d = db.Players.Where(x => x.Rank == Rank.D);
            var d1 = d.Count() - 3;
            return $"D{d1} C{c1} B{b1} A{a1} AA{aa1} M{m1} GM{gm1}";
        }
        /// <summary>
        /// 获取collected data
        /// </summary>
        /// <returns></returns>
        public static CollectedData GetCollectedData()
        {
            var db = new DataBaseContext();
            var ret = db.CollectedDatas.Single(x => x.Id == "1");
            return ret;
        }
        /// <summary>
        /// 色图计数器++
        /// </summary>
        public static void UpSetuCount()
        {
            var db = new DataBaseContext();
            var data = db.CollectedDatas.Single(x => x.Id == "1");
            data.SetuCount++;
            db.SaveChanges();
        }
        /// <summary>
        /// 影之诗计数器++
        /// </summary>
        public static void UpSvCount()
        {
            var db = new DataBaseContext();
            var data = db.CollectedDatas.Single(x => x.Id == "1");
            data.SvCount++;
            db.SaveChanges();
        }
        /// <summary>
        /// 丁真计数器++
        /// </summary>
        public static void UpYydzCount()
        {
            var db = new DataBaseContext();
            var data = db.CollectedDatas.Single(x => x.Id == "1");
            data.YydzCount++;
            db.SaveChanges();
        }
        /// <summary>
        /// 根据傻狗力数量排序
        /// </summary>
        /// <returns>排序后的用户列表</returns>
        public static List<UserInfo> TokenSort()
        {
            using (var db = new DataBaseContext())
            {
                return db.Users.OrderByDescending(x => x.Token).ToList();
            }
        }
    }
}
