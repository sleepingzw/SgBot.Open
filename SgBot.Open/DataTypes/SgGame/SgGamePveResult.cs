using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgBot.Open.DataTypes.SgGame.GameLibrary;
using SgBot.Open.Utils.Basic;

namespace SgBot.Open.DataTypes.SgGame
{
    public class SgGamePveResult
    {
        public List<SgGamePveResultDetail> Details { get; set; }

        public SgGamePveResult()
        {
            Details = new List<SgGamePveResultDetail>();
        }

        public void MakeResult(int times, Player player)
        {
            for (var i = 0; i < times; i++)
            {
                if (UsefulMethods.IsOk(5))
                {
                    var detail = new SgGamePveResultDetail(PveEncounter.GetEquip)
                    {
                        EquipmentGet = EquipmentMaker.OutEquipment(player.Level)
                    };
                    var success = player.GetEquipment(detail.EquipmentGet);
                    detail.AddEquipmentSucess = success;
                    Details.Add(detail);
                }
                else if (UsefulMethods.IsOk(3))
                {
                    var detail = new SgGamePveResultDetail(PveEncounter.GetCoin)
                    {
                        CoinGet = (long)(UsefulMethods.MakeRandom(110, 90) / 100 * player.Level * 1.15)
                    };
                    player.Coin += detail.CoinGet;
                    Details.Add(detail);
                }
                else
                {
                    var detail = new SgGamePveResultDetail(PveEncounter.GetExp)
                    {
                        ExpGet = (long)(UsefulMethods.MakeRandom(110, 90) / 100 * player.Level * 10)
                    };
                    player.GetExp(detail.ExpGet);
                    Details.Add(detail);
                }
            }
        }
    }
    public class SgGamePveResultDetail
    {
        public PveEncounter Encounter;
        public long ExpGet;
        public long CoinGet;
        public Equipment EquipmentGet;
        public bool AddEquipmentSucess;

        public SgGamePveResultDetail(PveEncounter encounter)
        {
            Encounter = encounter;
        }
    }

    public enum PveEncounter
    {
        GetExp,
        GetEquip,
        GetCoin,
        Other
    }
}
