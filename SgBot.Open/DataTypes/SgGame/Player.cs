﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SgBot.Open.DataTypes.SgGame
{
    public class Player
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string TitleString { get; set; }
        public long Coin { get; set; }
        public int Power { get; set; }
        public int PowerDay { get; set; }
        public long Level { get; set; }
        public long Exp { get; set; }
        public Rank Rank { get; set; }
        public int RankScore { get; set; }
        public bool IsHitBoss { get; set; }
        public bool IsWinToday { get; set; }
        public int BuyPowerTime { get; set; }
        public long Strength { get; set; } // 影响物理伤害，破甲值
        public long Intelligence { get; set; } //影响魔法伤害，护盾值
        public long Agility { get; set; } // 影响战斗速度，闪避率
        public long Fitness { get; set; } // 影响HP，减伤率
        public long FreePoints { get; set; }
        public string BagString { get; set; }
        public string SkillHaveString { get; set; }
        public string SkillActiveString { get; set; }
        [NotMapped]
        public Dictionary<int,int> SkillHave { get; set; }
        [NotMapped]
        public Dictionary<int, int> SkillActive { get; set; }
        [NotMapped]
        public List<Equipment> Bag { get; set; }
        [NotMapped]
        public List<int> Title { get; set; }
        public Player(string id)
        {
            Id = id;
            Name = "";
            Title = new List<int>();
            Coin = 0;
            Power = 100;
            PowerDay = 0;
            Level = 1;
            Exp = 0;
            Strength = 1;
            Intelligence = 1;
            Agility = 1;
            Fitness = 1;
            FreePoints = 0;
            Bag = new List<Equipment>();
            SkillActive = new Dictionary<int, int>();
            SkillHave = new Dictionary<int, int>();
            Rank = Rank.D;
            RankScore = 0;
            IsHitBoss = false;
            BuyPowerTime = 0;

            BagString = "";
            TitleString = "";
            SkillActiveString = "";
            SkillHaveString = "";
        }
        public bool GetRankScore(int score)
        {
            var upRank = false;
            RankScore += score;
            switch (Rank)
            {
                case Rank.D:
                    if (RankScore >= 100)
                    {
                        Rank = Rank.C;
                        RankScore = 0;
                        upRank = true;
                    }
                    break;
                case Rank.C:
                    if (RankScore >= 200)
                    {
                        Rank = Rank.B;
                        RankScore = 0;
                        upRank = true;
                    }
                    break;
                case Rank.B:
                    if (RankScore >= 400)
                    {
                        Rank = Rank.A;
                        RankScore = 0;
                        upRank = true;
                    }
                    break;
                case Rank.A:
                    if (RankScore >= 800)
                    {
                        Rank = Rank.AA;
                        RankScore = 0;
                        upRank = true;
                    }
                    break;
                case Rank.AA:
                    if (RankScore >= 1600)
                    {
                        Rank = Rank.M;
                        RankScore = 0;
                        upRank = true;
                    }
                    break;
                case Rank.M:
                    if (RankScore >= 3200)
                    {
                        Rank = Rank.GM;
                        RankScore = 0;
                        upRank = true;
                    }
                    break;
                case Rank.GM:
                    break;
                default:
                    break;
            }
            return upRank;
        } 
        public bool LostRankScore(int score)
        {
            RankScore -= score;
            if (RankScore >= 0) return false;
            if (Rank == Rank.D || Rank == Rank.M || Rank == Rank.GM)
            {
                if (RankScore < 0)
                {
                    RankScore = 0;
                }
                return false;
            }
            Rank--;
            switch (Rank)
            {
                case Rank.D:
                    RankScore = 80;
                    break;
                case Rank.C:
                    RankScore = 140;
                    break;
                case Rank.B:
                    RankScore = 240;
                    break;
                case Rank.A:
                    RankScore = 400;
                    break;
                case Rank.AA:
                    RankScore = 800;
                    break;
                case Rank.GM:
                    RankScore = 1600;
                    break;
                default: break;
            }
            return true;
        }
        public bool GetExp(long exp)
        {
            Exp += exp;
            var isUp = false;
            while (Exp >= UpgradeCost())
            {
                Exp -= UpgradeCost();
                Level++;
                FreePoints++;
                isUp = true;
            }

            return isUp;
        }
        public bool GetEquipment(Equipment equipment)
        {
            if (Bag.Count >= 15) return false;
            Bag.Add(equipment);
            return true;
        }
        private long UpgradeCost()
        {
            return Level * 9 + Level * Level;
        }
        public bool Refresh()
        {
            if (PowerDay == DateTime.Now.Day) return false;
            Power = 20;
            PowerDay = DateTime.Now.Day;
            IsHitBoss = false;
            IsWinToday = false;
            BuyPowerTime = 0;
            return true;
        }
        public int PowerNeedCoin(int many)
        {
            var trueMany = many + BuyPowerTime;
            var temp = trueMany / 20;

            var ret = 0;
            int i;
            for (i = 0; i < temp; i++)
            {
                trueMany -= 20;
                ret += (i + 1) * 20;
            }
            ret += trueMany * (i + 2);

            var tt = BuyPowerTime;
            var ttt = tt / 20;
            var bought = 0;
            int j;
            for (j = 0; j < ttt; j++)
            {
                tt -= 20;
                bought += (j + 1) * 20;
            }
            bought += tt * (j + 2);

            ret -= bought;
            return ret;
        }
        public void SortBag()
        {
            if (Bag.Count == 0)
            {
                return;
            }
            var tt=Bag.GroupBy(x => x.OnBody).OrderByDescending(k=>k.Key).ToList();
            var onBody = new List<Equipment>();
            var isLock = new List<Equipment>();
            var notLock = new List<Equipment>();
            if (tt.Count == 1)
            {
                //只有装备的物品
                if (tt[0].First().OnBody)
                {
                    onBody = tt[0].OrderBy(o => o.Category).ToList();
                }
                //只有非装备的物品
                else
                {
                    var tt2 = tt[0].GroupBy(x => x.IsLock).OrderByDescending(k => k.Key).ToList();
                    if (tt2.Count == 1)
                    {
                        //只有锁定物品
                        if (tt2[0].First().IsLock)
                        {
                            isLock = tt2[0].OrderBy(o => o.Category).ToList();
                        }
                        //只有非锁定物品
                        else
                        {
                            notLock = tt2[0].OrderBy(o => o.Category).ToList();
                        }
                    }
                    //锁定和非锁定都有
                    else
                    {
                        isLock = tt2[0].OrderBy(o => o.Category).ToList();
                        notLock = tt2[1].OrderBy(o => o.Category).ToList();
                    }
                }
            }
            //既有装备的也有没装备的
            else
            {
                //先取出装备的物品
                onBody = tt[0].OrderBy(o => o.Category).ToList();
                //然后看非装备的
                var notOnBody = tt[1].GroupBy(x => x.IsLock).OrderByDescending(k => k.Key).ToList();
                //只有一种
                if (notOnBody.Count == 1)
                {
                    if (notOnBody[0].First().IsLock)
                    {
                        isLock = notOnBody[0].OrderBy(o => o.Category).ToList();
                    }
                    //只有非锁定物品
                    else
                    {
                        notLock = notOnBody[0].OrderBy(o => o.Category).ToList();
                    }
                }
                //都有
                else
                {
                    isLock = notOnBody[0].OrderBy(o => o.Category).ToList();
                    notLock = notOnBody[1].OrderBy(o => o.Category).ToList();
                }
            }

            var temp=new List<Equipment>();
            temp.AddRange(onBody);
            temp.AddRange(isLock);
            temp.AddRange(notLock);

            Bag=temp;
        }

        public List<Equipment> OutLock()
        {
            var temp= new List<Equipment>();
            if (Bag.Count == 0)
            {
                return temp;
            }
            
            var tt = Bag.GroupBy(x => x.OnBody).OrderByDescending(k => k.Key).ToList();
            if(tt.Count == 1)
            {
                //只有穿身上的
                if (tt[0].First().OnBody)
                {
                    temp.AddRange(Bag);
                    return temp;
                }
                else
                {
                    var tt2 = tt[0].GroupBy(x => x.IsLock).OrderByDescending(k => k.Key).ToList();
                    if(tt2.Count==1)
                    {
                        //只有锁着的
                        if (tt2[0].First().IsLock)
                        {
                            temp.AddRange(Bag);
                            return temp;
                        }
                        //只有没锁的
                        else
                        {
                            return temp;
                        }
                    }
                    else
                    {
                        //返回锁着的
                        temp.AddRange(tt2[0]);
                        return temp;
                    }
                }
            }
            else
            {
                //先取身上的
                var onBody = tt[0].OrderBy(o => o.Category);
                var tt2 = tt[1].GroupBy(x => x.IsLock).OrderByDescending(k => k.Key).ToList();
                if (tt2.Count == 1)
                {
                    //除了穿着的只有锁着的
                    if (tt2[0].First().IsLock)
                    {
                        temp.AddRange(Bag);
                        return temp;
                    }
                    else
                    {
                        temp.AddRange(onBody);
                        return temp;
                    }
                }
                else
                {
                    temp.AddRange(onBody);
                    temp.AddRange(tt2[0]);
                    return temp;
                }
            }
        }
        /// <summary>
        /// 每次保存数据前应该调用
        /// </summary>
        public void MakeString()
        {
            BagString = JsonConvert.SerializeObject(Bag);
            TitleString = JsonConvert.SerializeObject(Title);
            SkillActiveString = JsonConvert.SerializeObject(SkillActive);
            SkillHaveString = JsonConvert.SerializeObject(SkillHave);
        }
        /// <summary>
        /// 读取到数据之后调用
        /// </summary>
        public void AnalyseString()
        {
            // Console.WriteLine(BagString);
            Bag = JsonConvert.DeserializeObject<List<Equipment>>(BagString)!;
            // Console.WriteLine(Bag.Count);
            Title = JsonConvert.DeserializeObject<List<int>>(TitleString)!;
            SkillHave = JsonConvert.DeserializeObject<Dictionary<int, int>>(SkillHaveString)!;
            SkillActive = JsonConvert.DeserializeObject<Dictionary<int, int>>(SkillActiveString)!;
        }
        #region IComparable<Player>
        public int CompareTo(Player other) => RankScore.CompareTo(other.RankScore);
        #endregion
    }

    public enum Rank
    {
        D,
        C,
        B,
        A,
        AA,
        M,
        GM,
        GOD
    }
}
