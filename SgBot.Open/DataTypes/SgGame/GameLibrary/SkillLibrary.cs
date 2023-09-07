using SgBot.Open.Utils.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBot.Open.DataTypes.SgGame.GameLibrary
{
    public static class SkillLibrary
    {
        public static Dictionary<int, Skill> Skills = new Dictionary<int, Skill>();

        static SkillLibrary()
        {
            Skills.Add(-1, new GMPower());
            Skills.Add(0,new GodAttack());            
            Skills.Add(3, new HotBlood());
            //Skills.Add(2, new TrueDamage());
        }
    }

    public abstract class Skill
    {
        protected Skill(string name, string description, string effect, string action, SkillTypeEnum skillType = SkillTypeEnum.Active)
        {
            Name = name;
            Description = description;
            Action = action;
            SkillType = skillType;
            Effect = effect;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Effect { get; set; }
        public string Action { get; set; }
        public SkillTypeEnum SkillType { get; set; }

        public abstract bool ActiveSkill(double activePossibilty, ref BattleUnit unit, ref BattleUnit enemyUnit, int skillLevel, SkillTypeEnum type, bool isAttack = true);

        public enum SkillTypeEnum
        {
            Passive,
            Active

        }
    }
    // 宗师之力
    public class GMPower : Skill
    {
        public override bool ActiveSkill(double activePossibilty, ref BattleUnit unit, ref BattleUnit enemyUnit, int skillLevel, SkillTypeEnum type, bool isAttack = true)
        {
            if (type != SkillType)
            {
                return false;
            }
            if (unit.Buffs.ContainsKey("GMPower"))
            {
                return false;
            }
            if (unit.Hp <= 0)
            {
                unit.Hp = unit.MaxHp;
                unit.Buffs.Add("GMPower", new Buff()
                {
                    CriticalProbabilityBattle = 1,
                    BattleSpeed = 1
                });
            }
            Action = "宗师之力！";
            return true;
        }
        public GMPower() : base("元初宗师之力", "作为最古老的宗师，一次死亡不能击倒你，反而会呼死你变得更加强大", "免疫一次死亡,满状态复活后暴击数值提高100%,速度提高100%", "", SkillTypeEnum.Passive) { }
    }
    // 天地闪耀傻狗之星
    public class GodAttack : Skill
    {

        public override bool ActiveSkill(double activePossibilty,ref BattleUnit unit, ref BattleUnit enemyUnit, int skillLevel, SkillTypeEnum type, bool isAttack = true)
        {        
            if (type != SkillType)
            {
                return false;
            }
            if(!UsefulMethods.IsOk(100,(int)activePossibilty))
            {
                return false;
            }
            enemyUnit.Hp -= 114514;
            Action = "随手一击，造成了114514真实伤害";
            return true;
        }
        public GodAttack() : base("天地闪耀傻狗之星", "傻狗一击", "造成114514真实伤害", "") { }
    }

    public class HotBlood : Skill
    {
        public HotBlood() : base("热血战魂", "心中的热血", "每次攻击后攻速增加9%", "", SkillTypeEnum.Passive) { }
        public override bool ActiveSkill(double activePossibilty,ref BattleUnit unit, ref BattleUnit enemyUnit, int skillLevel, SkillTypeEnum type, bool isAttack = true)
        {
            if (type != SkillType)
            {
                return false;
            }
            if (!isAttack)
            {
                return false;
            }
            if (unit.Buffs.ContainsKey(this.Name))
            {
                unit.Buffs[this.Name].BattleSpeed +=  0.09;
            }
            else
            {
                var temp = new Buff()
                {
                    BattleSpeed =  0.09,
                };
                unit.Buffs.Add(this.Name, temp);
            }
            return true;
        }        
    }
    
    // 奶一口
    //public class HealMyself : Skill
    //{
    //    string ISkill.Name { get; set; } = "奶一口";
    //    string ISkill.Description { get; set; } = "回复（20%*等级）最大生命";
    //    string ISkill.Action { get; set; }

    //    public BattleUnit ActiveSkill(BattleUnit unit, ref BattleUnit enemyUnit, int skillLevel)
    //    {
    //        long org = unit.Hp;
    //        long much;
    //        unit.Hp = unit.Hp + (long)(unit.MaxHp * 0.2 * skillLevel);
    //        if (unit.Hp > unit.MaxHp)
    //        {
    //            unit.Hp = unit.MaxHp;
    //        }
    //        much = unit.MaxHp - org;
    //        ISkill.Action = $"回复了{org}的生命!";

    //        return unit;
    //    }
    //}
    //// 真实伤害
    //public class TrueDamage : Skill
    //{
    //    string ISkill.Name { get; set; } = "真实之击";
    //    string ISkill.Description { get; set; } = "给予敌人（等级*（物理伤害+魔法伤害）*0.1）的真实伤害";
    //    public BattleUnit ActiveSkill(BattleUnit unit, ref BattleUnit enemyUnit, int skillLevel)
    //    {

    //        return unit;
    //    }
    //}
}
