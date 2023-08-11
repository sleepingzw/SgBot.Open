using Manganese.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBot.Open.DataTypes.SgGame
{
    public class BattleLog
    {
        public bool IsWin;
        public string PlayerName;
        public string EnemyName;
        public DateTime BattleTime;
        public List<BattleLogDetail> Details;

        public BattleLog()
        {
            IsWin = true;
            PlayerName = "";
            EnemyName = "";
            BattleTime = DateTime.Now;
            Details = new List<BattleLogDetail>();
        }

        public int OutLines()
        {
            return Details.Count * 3 + Details.Count(detail => !detail.PostiveSkillAction.IsNullOrEmpty()) +
                   Details.Count() / 10;
        }
    }

    public class BattleLogDetail
    {
        public bool IsPlayerAttack;


        public long PlayerHpMax;
        public long PlayerHpNow;
        public long PlayerShieldMax;
        public long PlayerShieldNow;

        public long EnemyHpMax;
        public long EnemyHpNow;
        public long EnemyShieldMax;
        public long EnemyShieldNow;

        public long AttackerDmg;

        public bool IsCritical;
        public bool IsMiss;

        public string PostiveSkillAction;

        public BattleLogDetail(bool isPlayerAttack, bool isMiss, long playerHpMax, long playerHpNow,
            long playerShieldMax,
            long playerShieldNow, long enemyHpMax, long enemyHpNow, long enemyShieldMax, long enemyShieldNow,
            long attackerDmg, bool isCritical, string postiveSkillAction)
        {
            IsPlayerAttack = isPlayerAttack;
            PlayerHpMax = playerHpMax;
            PlayerHpNow = playerHpNow;
            PlayerShieldMax = playerShieldMax;
            PlayerShieldNow = playerShieldNow;
            EnemyHpMax = enemyHpMax;
            EnemyHpNow = enemyHpNow;
            EnemyShieldMax = enemyShieldMax;
            EnemyShieldNow = enemyShieldNow;
            AttackerDmg = attackerDmg;
            IsCritical = isCritical;
            PostiveSkillAction = postiveSkillAction;
            IsMiss = isMiss;
        }

    }
}
