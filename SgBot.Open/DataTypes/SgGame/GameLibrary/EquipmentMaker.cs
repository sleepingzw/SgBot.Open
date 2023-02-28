using Newtonsoft.Json;
using SgBot.Open.Utils.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBot.Open.DataTypes.SgGame.GameLibrary
{
    public static partial class EquipmentMaker
    {
        private static readonly Dictionary<int, List<Equipment>> WeaponList = new Dictionary<int, List<Equipment>>();
        private static readonly Dictionary<int, List<Equipment>> ArmorList = new Dictionary<int, List<Equipment>>();
        private static readonly Dictionary<int, List<Equipment>> JewelryList = new Dictionary<int, List<Equipment>>();

        public static Equipment OutEquipment(long level)
        {
            var outs = OutLevel(level);
            if (UsefulMethods.IsOk(3))
            {
                // var equip = UsefulMethods.GetRandomFromList(WeaponList1).Clone();
                var e = JsonConvert.SerializeObject(UsefulMethods.GetRandomFromList(WeaponList[outs]));
                var equip = JsonConvert.DeserializeObject<Equipment>(e)!;
                equip.EquipmentEffect.Randomize();
                return equip;
            }
            if (UsefulMethods.IsOk(2))
            {
                // var equip = UsefulMethods.GetRandomFromList(ArmorList1).Clone();
                var e = JsonConvert.SerializeObject(UsefulMethods.GetRandomFromList(ArmorList[outs]));
                var equip = JsonConvert.DeserializeObject<Equipment>(e)!;
                equip.EquipmentEffect.Randomize();
                return equip;
            }
            else
            {
                var e = JsonConvert.SerializeObject(UsefulMethods.GetRandomFromList(JewelryList[outs]));
                var equip = JsonConvert.DeserializeObject<Equipment>(e)!;
                // var equip = UsefulMethods.GetRandomFromList(JewelryList1).Clone();
                equip.EquipmentEffect.Randomize();
                return equip;
            }
        }

        private static int OutLevel(long level)
        {
            switch (level)
            {
                case > 500 when UsefulMethods.IsOk(3):
                    return 5;
                case > 500:
                    return UsefulMethods.IsOk(2) ? 4 : 3;
                case > 300 when UsefulMethods.IsOk(3):
                    return 4;
                case > 300:
                    return UsefulMethods.IsOk(2) ? 3 : 2;
                case > 200 when UsefulMethods.IsOk(3):
                    return 3;
                case > 200:
                    return UsefulMethods.IsOk(2) ? 2 : 1;
                case > 100:
                    return UsefulMethods.IsOk(3) ? 2 : 1;
                default:
                    return 1;
            }
        }

    }
}
