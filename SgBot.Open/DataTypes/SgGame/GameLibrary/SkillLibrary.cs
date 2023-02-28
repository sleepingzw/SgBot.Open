using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBot.Open.DataTypes.SgGame.GameLibrary
{
    public static class SkillLibrary
    {
        public static Dictionary<int, ISkill> Skills = new Dictionary<int, ISkill>();

        static SkillLibrary()
        {
            Skills.Add(1, new HealMyself());
            Skills.Add(2, new TrueDamage());
        }
    }

    public interface ISkill
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public BattleUnit ActiveSkill(BattleUnit unit, ref BattleUnit enemyUnit,int skillLevel);
    }
    // 奶一口
    public class HealMyself : ISkill
    {
        string ISkill.Name { get; set; } = "奶一口";
        string ISkill.Description { get; set; } = "回复（20%*等级）最大生命";

        public BattleUnit ActiveSkill(BattleUnit unit, ref BattleUnit enemyUnit, int skillLevel)
        {
            long org = unit.Hp;
            long much;
            unit.Hp = unit.Hp + (long)(unit.MaxHp * 0.2 * skillLevel);
            if (unit.Hp > unit.MaxHp)
            {
                unit.Hp = unit.MaxHp;
            }

            much = unit.MaxHp - org;
            return unit;
        }
    }
    // 真实伤害
    public class TrueDamage : ISkill
    {
        string ISkill.Name { get; set; } = "真实之击";
        string ISkill.Description { get; set; } = "给予敌人（等级*（物理伤害+魔法伤害）*0.1）的真实伤害";
        public BattleUnit ActiveSkill(BattleUnit unit, ref BattleUnit enemyUnit, int skillLevel)
        {

            return unit;
        }
    }
}
