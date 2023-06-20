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
            Skills.Add(0,new GodAttack());
            //Skills.Add(1, new HealMyself());
            //Skills.Add(2, new TrueDamage());
        }
    }

    public abstract class Skill
    {
        protected Skill(string name, string description, string action, SkillTypeEnum skillType = SkillTypeEnum.Active)
        {
            Name = name;
            Description = description;
            Action = action;
            SkillType = skillType;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }
        public SkillTypeEnum SkillType { get; set; }
        public abstract BattleUnit ActiveSkill(BattleUnit unit, ref BattleUnit enemyUnit,int skillLevel, SkillTypeEnum type);

        public enum SkillTypeEnum
        {
            Passive,
            Active

        }
    }
    // 天地闪耀傻狗之星
    public class GodAttack : Skill
    {
        public override BattleUnit ActiveSkill(BattleUnit unit, ref BattleUnit enemyUnit, int skillLevel, SkillTypeEnum type)
        {
            if (type != SkillType)
            {
                return unit;
            }
            enemyUnit.Hp -= 114514;
            Action = "随手一击，造成了114514真实伤害";
            return unit;
        }
        public GodAttack() : base("天地闪耀傻狗之星", "傻狗一击", ""){}
    }
    public class HotBlood : Skill
    {
        public override BattleUnit ActiveSkill(BattleUnit unit, ref BattleUnit enemyUnit, int skillLevel, SkillTypeEnum type)
        {
            if (type != SkillType)
            {
                return unit;
            }
            enemyUnit.Hp -= 114514;
            Action = "热血！";
            return unit;
        }
        public HotBlood() : base("热血战魂", "心中的热血,每次攻击后攻速增加9%", "",SkillTypeEnum.Passive) { }
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
