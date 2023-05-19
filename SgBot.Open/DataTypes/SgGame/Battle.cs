using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgBot.Open.DataTypes.SgGame.GameLibrary;
using SgBot.Open.Utils.Basic;

namespace SgBot.Open.DataTypes.SgGame
{
    public class Battle
    {
        private const int DefaultSwift = 75;
        private const int DefaultCrit = 10;
        public BattleLog MakeBattle(Player playerp, Player enemyp)
        {
            var player = new BattleUnit(playerp);
            var enemy = new BattleUnit(enemyp);

            var ret = new BattleLog();

            BattleUnit fastUnit;
            BattleUnit slowUnit;

            string fname, sname;
            var isPlayerFast = true;
            if (player.BattleSpeed >= enemy.BattleSpeed)
            {
                fastUnit = player;
                fname = playerp.Name;
                slowUnit = enemy;
                sname = enemyp.Name;
            }
            else
            {
                fastUnit = enemy;
                sname = playerp.Name;
                slowUnit = player;
                fname = enemyp.Name;
                isPlayerFast = false;
            }
            // Console.WriteLine(JsonConvert.SerializeObject(fastUnit,Formatting.Indented));
            var speed = fastUnit.BattleSpeed / slowUnit.BattleSpeed;
            var speedFlag = speed;
            var round = 1;
            while (fastUnit.Hp > 0 && slowUnit.Hp > 0)
            {
                fastUnit.Refresh();
                slowUnit.Refresh();

                fastUnit.MagicAtkBattle *= 1 + (int)((round - 1) / 10);
                fastUnit.PhysicalAtkBattle *= 1 + (int)((round - 1) / 10);
                slowUnit.MagicAtkBattle *= 1 + (int)((round - 1) / 10);
                slowUnit.PhysicalAtkBattle *= 1 + (int)((round - 1) / 10);

                var skillActiveLog = "";
                var isCrit = false;
                if (speedFlag >= 1) // 速度高的攻击
                {
                    speedFlag--;
                    var value = fastUnit.SkillActiveProbability - slowUnit.SkillActiveProbability;
                    value += 40;
                    if (value > 100)
                    {
                        value = 100;

                    }

                    if (value < 20)
                    {
                        value = 20;
                    }
                    foreach (var skill in fastUnit.Skills.Where(skill => UsefulMethods.IsOk(100, (int)value)))
                    {
                        if (!SkillLibrary.Skills.TryGetValue(skill, out var whatSkill)) continue;
                        fastUnit = whatSkill.ActiveSkill(fastUnit, ref slowUnit, 1);
                        skillActiveLog += $"{whatSkill.Name} 发动! {fname}{whatSkill.Action}";
                        break;
                    }

                    var temp = slowUnit.CriticalProbabilityBattle - fastUnit.CriticalProbabilityBattle;
                    double swiftFlag = 0;
                    if (temp < 0)
                    {
                        swiftFlag = 10;

                    }
                    else
                    {
                        swiftFlag = 5 * Math.Log2(2 * temp) / 15;
                    }
                    swiftFlag = 100 - swiftFlag;
                    if (swiftFlag > 90)
                    {
                        swiftFlag = 90;
                    }

                    if (swiftFlag < 10)
                    {
                        swiftFlag = 10;
                    }

                    temp = -temp;
                    if (temp < 0)
                    {
                        temp = 1;
                    }
                    var critFlag = (int)(DefaultCrit + Math.Log2(2 * temp));

                    #region 暴击处理
                    if (critFlag > 95)
                    {
                        critFlag = 95;
                    }
                    if (critFlag < 5)
                    {
                        critFlag = 5;
                    }
                    #endregion

                    if (UsefulMethods.IsOk(100, critFlag))
                    {
                        isCrit = true;
                    }

                    if (UsefulMethods.IsOk(100, (int)swiftFlag))
                    {
                        var jsp = Math.Log2(2 * slowUnit.PhysicalDefBattle) / Math.Log2(2 * fastUnit.PhysicalAtkBattle);
                        jsp += slowUnit.PhysicalDefBattle;
                        jsp *= Math.Sqrt(Math.Sqrt(1 + slowUnit.PhysicalDefBattle));
                        var phyDmg = fastUnit.PhysicalAtkBattle - jsp;
                        phyDmg = (int)(phyDmg * UsefulMethods.MakeRandom(110, 90) / 100);
                        if (phyDmg < 0) phyDmg = 0;
                        var jsm = Math.Log2(2 * slowUnit.MagicDefBattle) / Math.Log2(2 * fastUnit.MagicAtkBattle);
                        jsm += slowUnit.MagicDefBattle;
                        jsm *= Math.Sqrt(Math.Sqrt(1 + slowUnit.MagicDefBattle));
                        var magDmg = fastUnit.MagicAtkBattle - jsm;
                        magDmg = (int)(magDmg * UsefulMethods.MakeRandom(110, 90) / 100);
                        if (magDmg < 0) magDmg = 0;
                        var dmgDouble = phyDmg + magDmg;
                        var dmg = (long)dmgDouble;
                        if (isCrit)
                        {
                            var trueTime = fastUnit.CriticalDamageBattle - slowUnit.CriticalDamageBattle;
                            if (trueTime < 0)
                            {
                                trueTime = 0;
                            }
                            trueTime++;
                            trueTime = Math.Log2(2 * trueTime);
                            trueTime *= 1.5;
                            dmg = (long)(dmgDouble * trueTime);
                        }

                        if (slowUnit.Shield > 0)
                        {
                            slowUnit.Shield -= dmg;
                            if (slowUnit.Shield < 0) slowUnit.Shield = 0;
                        }
                        else
                        {
                            slowUnit.Hp -= dmg;
                        }
                        // 护盾回复
                        if (fastUnit.Shield != 0)
                        {
                            fastUnit.Shield += (long)(fastUnit.MaxShield * 0.1);
                            if (fastUnit.Shield > fastUnit.MaxShield)
                            {
                                fastUnit.Shield = fastUnit.MaxShield;
                            }
                        }
                        if (isPlayerFast)
                        {
                            var detail = new BattleLogDetail(true, false, fastUnit.MaxHp,
                                fastUnit.Hp, fastUnit.MaxShield,
                                fastUnit.Shield, slowUnit.MaxHp,
                                slowUnit.Hp, slowUnit.MaxShield,
                                slowUnit.Shield, dmg,
                                isCrit, skillActiveLog);
                            ret.Details.Add(detail);
                        }
                        else
                        {
                            var detail = new BattleLogDetail(false, false, slowUnit.MaxHp,
                                slowUnit.Hp, slowUnit.MaxShield,
                                slowUnit.Shield, fastUnit.MaxHp,
                                fastUnit.Hp, fastUnit.MaxShield,
                                fastUnit.Shield, dmg,
                                isCrit, skillActiveLog);
                            ret.Details.Add(detail);
                        }
                    }
                    else
                    {
                        // 护盾回复
                        if (fastUnit.Shield != 0)
                        {
                            fastUnit.Shield += (long)(fastUnit.MaxShield * 0.1);
                            if (fastUnit.Shield > fastUnit.MaxShield)
                            {
                                fastUnit.Shield = fastUnit.MaxShield;
                            }
                        }
                        if (isPlayerFast)
                        {
                            var detail = new BattleLogDetail(true, true, fastUnit.MaxHp,
                                fastUnit.Hp, fastUnit.MaxShield,
                                fastUnit.Shield, slowUnit.MaxHp,
                                slowUnit.Hp, slowUnit.MaxShield,
                                slowUnit.Shield, 0,
                                isCrit, skillActiveLog);
                            ret.Details.Add(detail);
                        }
                        else
                        {
                            var detail = new BattleLogDetail(false, true, slowUnit.MaxHp,
                                slowUnit.Hp, slowUnit.MaxShield,
                                slowUnit.Shield, fastUnit.MaxHp,
                                fastUnit.Hp, fastUnit.MaxShield,
                                fastUnit.Shield, 0,
                                isCrit, skillActiveLog);
                            ret.Details.Add(detail);
                        }
                    }

                }
                else // 速度低的攻击
                {
                    speedFlag += speed;
                    var value = slowUnit.SkillActiveProbability - fastUnit.SkillActiveProbability;
                    value += 40;
                    if (value > 100)
                    {
                        value = 100;

                    }
                    if (value < 20)
                    {
                        value = 20;
                    }
                    foreach (var skill in slowUnit.Skills.Where(skill => UsefulMethods.IsOk(100, (int)value)))
                    {
                        if (!SkillLibrary.Skills.TryGetValue(skill, out var whatSkill)) continue;
                        slowUnit = whatSkill.ActiveSkill(slowUnit, ref fastUnit, 1);
                        skillActiveLog += $"{whatSkill.Name} 发动! {sname}{whatSkill.Description}";
                        break;
                    }

                    var temp = fastUnit.CriticalProbabilityBattle - slowUnit.CriticalProbabilityBattle;
                    double swiftFlag = 0;
                    if (temp < 0)
                    {
                        swiftFlag = 10;

                    }
                    else
                    {
                        swiftFlag = 5 * Math.Log2(2 * temp) / 15;
                    }

                    swiftFlag = 100 - swiftFlag;
                    if (swiftFlag > 90)
                    {
                        swiftFlag = 90;
                    }
                    if (swiftFlag < 10)
                    {
                        swiftFlag = 10;
                    }
                    temp = -temp;
                    if (temp < 0)
                    {
                        temp = 1;
                    }
                    var critFlag = (int)(DefaultCrit + Math.Log2(2 * temp));
                    #region 暴击处理
                    if (critFlag > 95)
                    {
                        critFlag = 95;
                    }

                    if (critFlag < 5)
                    {
                        critFlag = 5;
                    }
                    #endregion
                    if (UsefulMethods.IsOk(100, critFlag))
                    {
                        isCrit = true;
                    }

                    if (UsefulMethods.IsOk(100, (int)swiftFlag))
                    {
                        var jsp = Math.Log2(2 * fastUnit.PhysicalDefBattle) / Math.Log2(2 * slowUnit.PhysicalAtkBattle);
                        jsp += fastUnit.PhysicalDefBattle;
                        jsp *= Math.Sqrt(Math.Sqrt(1 + fastUnit.PhysicalDefBattle));
                        var phyDmg = slowUnit.PhysicalAtkBattle - jsp;
                        phyDmg = (int)(phyDmg * UsefulMethods.MakeRandom(110, 90) / 100);
                        if (phyDmg < 0) phyDmg = 0;
                        var jsm = Math.Log2(2 * fastUnit.MagicDefBattle) / Math.Log2(2 * slowUnit.MagicAtkBattle);
                        jsm += fastUnit.MagicDefBattle;
                        jsm *= Math.Sqrt(Math.Sqrt(1 + fastUnit.MagicDefBattle));
                        var magDmg = slowUnit.MagicAtkBattle - jsm;
                        magDmg = (int)(magDmg * UsefulMethods.MakeRandom(110, 90) / 100);
                        if (magDmg < 0) magDmg = 0;
                        var dmgDouble = phyDmg + magDmg;
                        var dmg = (long)dmgDouble;
                        if (isCrit)
                        {
                            var trueTime = slowUnit.CriticalDamageBattle - fastUnit.CriticalDamageBattle;
                            if (trueTime < 0)
                            {
                                trueTime = 0;
                            }
                            trueTime++;
                            trueTime = Math.Log2(2 * trueTime);
                            trueTime *= 1.5;
                            dmg = (long)(dmgDouble * trueTime);
                        }

                        if (fastUnit.Shield > 0)
                        {
                            fastUnit.Shield -= dmg;
                            if (fastUnit.Shield < 0) fastUnit.Shield = 0;
                        }
                        else
                        {
                            fastUnit.Hp -= dmg;
                        }
                        // 护盾回复
                        if (slowUnit.Shield != 0)
                        {
                            slowUnit.Shield += (long)(slowUnit.MaxShield * 0.1);
                            if (slowUnit.Shield > slowUnit.MaxShield)
                            {
                                slowUnit.Shield = slowUnit.MaxShield;
                            }
                        }
                        if (isPlayerFast)
                        {
                            var detail = new BattleLogDetail(false, false, fastUnit.MaxHp,
                                fastUnit.Hp, fastUnit.MaxShield,
                                fastUnit.Shield, slowUnit.MaxHp,
                                slowUnit.Hp, slowUnit.MaxShield,
                                slowUnit.Shield, dmg,
                                isCrit, skillActiveLog);
                            ret.Details.Add(detail);
                        }
                        else
                        {
                            var detail = new BattleLogDetail(true, false, slowUnit.MaxHp,
                                slowUnit.Hp, slowUnit.MaxShield,
                                slowUnit.Shield, fastUnit.MaxHp,
                                fastUnit.Hp, fastUnit.MaxShield,
                                fastUnit.Shield, dmg,
                                isCrit, skillActiveLog);
                            ret.Details.Add(detail);
                        }
                    }
                    else
                    {
                        // 护盾回复
                        if (slowUnit.Shield != 0)
                        {
                            slowUnit.Shield += (long)(slowUnit.MaxShield * 0.1);
                            if (slowUnit.Shield > slowUnit.MaxShield)
                            {
                                slowUnit.Shield = slowUnit.MaxShield;
                            }
                        }
                        if (isPlayerFast)
                        {
                            var detail = new BattleLogDetail(false, true, fastUnit.MaxHp,
                                fastUnit.Hp, fastUnit.MaxShield,
                                fastUnit.Shield, slowUnit.MaxHp,
                                slowUnit.Hp, slowUnit.MaxShield,
                                slowUnit.Shield, 0,
                                isCrit, skillActiveLog);
                            ret.Details.Add(detail);
                        }
                        else
                        {
                            var detail = new BattleLogDetail(true, true, slowUnit.MaxHp,
                                slowUnit.Hp, slowUnit.MaxShield,
                                slowUnit.Shield, fastUnit.MaxHp,
                                fastUnit.Hp, fastUnit.MaxShield,
                                fastUnit.Shield, 0,
                                isCrit, skillActiveLog);
                            ret.Details.Add(detail);
                        }
                    }

                }

                speed = fastUnit.BattleSpeed / slowUnit.BattleSpeed;
                round++;
            }

            if (fastUnit.Hp <= 0)
            {
                ret.IsWin = !isPlayerFast;
            }
            else
            {
                ret.IsWin = isPlayerFast;
            }

            ret.PlayerName = playerp.Name;
            ret.EnemyName = enemyp.Name;

            return ret;
        }
    }
}
