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

                var postiveSkillActiveLog = "";
                var isCrit = false;
                var isMiss = true;
                var isPlayerAttack = false;
                long dmg = 0;
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
                    //主动技能启动
                    foreach (var skill in fastUnit.Skills)
                    {
                        if (!SkillLibrary.Skills.TryGetValue(skill, out var whatSkill)) continue;
                        if (whatSkill.ActiveSkill(value, ref fastUnit, ref slowUnit, 1, Skill.SkillTypeEnum.Active))
                        {
                            postiveSkillActiveLog += $"{whatSkill.Name} 发动! {fname}{whatSkill.Action}";
                            break;
                        }
                    }

                    var temp = slowUnit.CriticalProbabilityBattle - fastUnit.CriticalProbabilityBattle;
                    double swiftFlag;
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

                    if (UsefulMethods.IsOk(100, (int)swiftFlag))//判定闪避,生成伤害
                    {
                        isMiss = false;
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

                        if (isCrit)//判定暴击
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
                        else
                        {
                            dmg = (long)dmgDouble;
                        }
                    }
                    // 造成伤害
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
                    //被动技能生效
                    //fastUnit.Skills

                    isPlayerAttack = isPlayerFast;

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
                    foreach (var skill in slowUnit.Skills)
                    {
                        if (!SkillLibrary.Skills.TryGetValue(skill, out var whatSkill)) continue;
                        if (whatSkill.ActiveSkill(value, ref slowUnit, ref fastUnit, 1, Skill.SkillTypeEnum.Active))
                        {
                            postiveSkillActiveLog += $"{whatSkill.Name} 发动! {sname}{whatSkill.Description}";
                            break;
                        }
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

                    //判定闪避,生成伤害
                    if (UsefulMethods.IsOk(100, (int)swiftFlag))
                    {
                        isMiss = false;
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
                        else
                        {
                            dmg = (long)dmgDouble;
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
                    isPlayerAttack = !isPlayerFast;
                    //被动技能生效
                }
                //写入detail

                var detail = new BattleLogDetail(isPlayerAttack, isMiss,
                            isPlayerFast ? fastUnit.MaxHp : slowUnit.MaxHp,
                            isPlayerFast ? fastUnit.Hp : slowUnit.Hp,
                            isPlayerFast ? fastUnit.MaxShield : slowUnit.MaxShield,
                            isPlayerFast ? fastUnit.Shield : slowUnit.Shield,
                            isPlayerFast ? slowUnit.MaxHp : fastUnit.MaxHp,
                            isPlayerFast ? slowUnit.Hp : fastUnit.Hp,
                            isPlayerFast ? slowUnit.MaxShield : fastUnit.MaxShield,
                            isPlayerFast ? slowUnit.Shield : fastUnit.Shield,
                            dmg, isCrit, postiveSkillActiveLog);
                ret.Details.Add(detail);

                speed = fastUnit.BattleSpeed / slowUnit.BattleSpeed;
                round++;
            }

            //退出战斗循环，结算结果
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
