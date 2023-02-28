using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBot.Open.DataTypes.SgGame.GameLibrary
{
    public static partial class EquipmentMaker
    {
        static EquipmentMaker()
        {
            #region 创建字典
            WeaponList.Add(1, new List<Equipment>());
            WeaponList.Add(2, new List<Equipment>());
            WeaponList.Add(3, new List<Equipment>());
            WeaponList.Add(4, new List<Equipment>());
            WeaponList.Add(5, new List<Equipment>());

            ArmorList.Add(1, new List<Equipment>());
            ArmorList.Add(2, new List<Equipment>());
            ArmorList.Add(3, new List<Equipment>());
            ArmorList.Add(4, new List<Equipment>());
            ArmorList.Add(5, new List<Equipment>());

            JewelryList.Add(1, new List<Equipment>());
            JewelryList.Add(2, new List<Equipment>());
            JewelryList.Add(3, new List<Equipment>());
            JewelryList.Add(4, new List<Equipment>());
            JewelryList.Add(5, new List<Equipment>());
            #endregion

            #region 1级道具
            // weapon1
            var weapon1_1 = new Equipment(EquipmentCategory.Weapon, "全村最好的剑", "村里最好的剑", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = 0.1
                }
            };
            WeaponList[1].Add(weapon1_1);
            var weapom1_2 = new Equipment(EquipmentCategory.Weapon, "全村最好的法杖", "村里最好的法杖", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    MagicAtkBonus = 0.1
                }
            };
            WeaponList[1].Add(weapom1_2);
            var weapom1_3 = new Equipment(EquipmentCategory.Weapon, "最强新手剑", "最好的新手武器", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = 0.1,
                    MagicAtkBonus = 0.1
                }
            };
            WeaponList[1].Add(weapom1_3);
            var weapom1_4 = new Equipment(EquipmentCategory.Weapon, "生锈的不锈钢小刀", "锈迹斑斑", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = 0.05,
                    MagicAtkBonus = 0.02
                }
            };
            WeaponList[1].Add(weapom1_4);
            var weapom1_5 = new Equipment(EquipmentCategory.Weapon, "不锈钢小刀", "崭新出厂", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = 0.08,
                }
            };
            WeaponList[1].Add(weapom1_5);
            var weapom1_6 = new Equipment(EquipmentCategory.Weapon, "黯淡的水晶球", "光要熄灭了", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    MagicAtkBonus = 0.05
                }
            };
            WeaponList[1].Add(weapom1_6);
            var weapom1_7 = new Equipment(EquipmentCategory.Weapon, "树枝", "你确定要用这玩意战斗？", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    MagicAtkBonus = 0.01,
                    PhysicalAtkBonus = 0.01
                }
            };
            WeaponList[1].Add(weapom1_7);
            var weapom1_8 = new Equipment(EquipmentCategory.Weapon, "大葱", "也许是歌姬的武器", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    MagicAtkBonus = 0.02,
                    PhysicalAtkBonus = 0.01
                }
            };
            WeaponList[1].Add(weapom1_8);
            var weapom1_9 = new Equipment(EquipmentCategory.Weapon, "自然法杖", "指的是外观", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    MagicAtkBonus = 0.08,
                    PhysicalAtkBonus = 0.03
                }
            };
            WeaponList[1].Add(weapom1_9);
            var weapom1_10 = new Equipment(EquipmentCategory.Weapon, "冰之法杖", "一根冰棍", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    MagicAtkBonus = 0.1,
                    PhysicalAtkBonus = 0.04
                }
            };
            WeaponList[1].Add(weapom1_10);

            // armor1
            var armor1_1 = new Equipment(EquipmentCategory.Armor, "最强新手甲", "最好的新手防具", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalDefBonus = 0.1,
                    MagicDefBonus = 0.1
                }
            };
            ArmorList[1].Add(armor1_1);
            var armor1_2 = new Equipment(EquipmentCategory.Armor, "破布", "一块破布", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalDefBonus = 0.01,
                }
            };
            ArmorList[1].Add(armor1_2);
            ArmorList[1].Add(armor1_2);
            var armor1_3 = new Equipment(EquipmentCategory.Armor, "发霉背心", "发霉的背心,真的要穿吗", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalDefBonus = 0.01,
                    MagicDefBonus = 0.01
                }
            };
            ArmorList[1].Add(armor1_3);
            var armor1_4 = new Equipment(EquipmentCategory.Armor, "陈旧的外套", "打了好几个补丁", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalDefBonus = 0.05,
                    MagicDefBonus = 0.03
                }
            };
            ArmorList[1].Add(armor1_4);
            var armor1_5 = new Equipment(EquipmentCategory.Armor, "农夫的蓑衣", "自然的感觉", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalDefBonus = 0.03,
                    MagicDefBonus = 0.03
                }
            };
            ArmorList[1].Add(armor1_5);
            var armor1_6 = new Equipment(EquipmentCategory.Armor, "皮质护甲", "野兽的皮制成", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalDefBonus = 0.1,
                    MagicDefBonus = 0.04
                }
            };
            ArmorList[1].Add(armor1_6);
            var armor1_7 = new Equipment(EquipmentCategory.Armor, "智慧帽", "帽子上写着智慧,但真的有用吗", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalDefBonus = 0.01,
                    MagicDefBonus = 0.1
                }
            };
            ArmorList[1].Add(armor1_7);
            var armor1_8 = new Equipment(EquipmentCategory.Armor, "远征护甲", "准备好远征了吗", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalDefBonus = 0.1,
                    MagicDefBonus = 0.1,
                    SwiftBonus = -0.1
                }
            };
            ArmorList[1].Add(armor1_8);

            // jewelry1
            var jewelry1_1 = new Equipment(EquipmentCategory.Jewelry, "破损的防御挂坠", "快坏了", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalDefBonus = 0.03,
                    MagicDefBonus = 0.1
                }
            };
            JewelryList[1].Add(jewelry1_1);
            var jewelry1_2 = new Equipment(EquipmentCategory.Jewelry, "普通的防御挂坠", "闪着微光", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalDefBonus = 0.07,
                    MagicDefBonus = 0.12
                }
            };
            JewelryList[1].Add(jewelry1_2);
            var jewelry1_3 = new Equipment(EquipmentCategory.Jewelry, "破损的攻击挂坠", "快坏了", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    MagicAtkBonus = 0.1,
                    PhysicalAtkBonus = 0.03
                }
            };
            JewelryList[1].Add(jewelry1_3);
            var jewelry1_4 = new Equipment(EquipmentCategory.Jewelry, "普通的攻击挂坠", "闪着微光", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    MagicAtkBonus = 0.12,
                    PhysicalAtkBonus = 0.07
                }
            };
            JewelryList[1].Add(jewelry1_4);
            var jewelry1_5 = new Equipment(EquipmentCategory.Jewelry, "破损的生存挂坠", "快坏了", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    MaxHpBonus = 0.03,
                    MaxShieldBonus = 0.1
                }
            };
            JewelryList[1].Add(jewelry1_5);
            var jewelry1_6 = new Equipment(EquipmentCategory.Jewelry, "普通的生存挂坠", "闪着微光", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    MaxHpBonus = 0.07,
                    MaxShieldBonus = 0.12
                }
            };
            JewelryList[1].Add(jewelry1_6);
            var jewelry1_7 = new Equipment(EquipmentCategory.Jewelry, "破损的迅捷挂坠", "快坏了", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    SwiftBonus = 0.03,
                    SpeedBonus = 0.1
                }
            };
            JewelryList[1].Add(jewelry1_7);
            var jewelry1_8 = new Equipment(EquipmentCategory.Jewelry, "普通的迅捷挂坠", "闪着微光", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    SwiftBonus = 0.07,
                    SpeedBonus = 0.12
                }
            };
            JewelryList[1].Add(jewelry1_8);
            var jewelry1_9 = new Equipment(EquipmentCategory.Jewelry, "破损的幸运挂坠", "快坏了", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    CriticalProbabilityBonus = 0.1,
                    CriticalDamageBonus = 0.03
                }
            };
            JewelryList[1].Add(jewelry1_9);
            var jewelry1_10 = new Equipment(EquipmentCategory.Jewelry, "普通的幸运挂坠", "闪着微光", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    CriticalProbabilityBonus = 0.12,
                    CriticalDamageBonus = 0.07
                }
            };
            JewelryList[1].Add(jewelry1_10);
            #endregion

            #region 2级道具
            // weapon2
            var weapon2_1 = new Equipment(EquipmentCategory.Weapon, "寒光不锈钢小刀", "闪着寒光", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = 0.15
                }
            };
            WeaponList[2].Add(weapon2_1);
            var weapon2_2 = new Equipment(EquipmentCategory.Weapon, "铁制细剑", "很细,应该不会断吧", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = 0.11,
                    MagicAtkBonus = 0.08,
                    SwiftBonus = 0.27,
                    CriticalProbabilityBonus = 0.03
                }
            };
            WeaponList[2].Add(weapon2_2);
            var weapon2_3 = new Equipment(EquipmentCategory.Weapon, "铁大剑", "铁质大剑", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = 0.18,
                    MagicAtkBonus = 0.1,
                    SwiftBonus = -0.07,
                    CriticalDamageBonus = 0.3
                }
            };
            WeaponList[2].Add(weapon2_3);
            var weapon2_4 = new Equipment(EquipmentCategory.Weapon, "海盗的勾子", "带着血迹哦", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = 0.1,
                    MagicAtkBonus = 0.04,
                    SwiftBonus = 0.3,
                    SpeedBonus = 0.15
                }
            };
            WeaponList[2].Add(weapon2_4);
            var weapon2_5 = new Equipment(EquipmentCategory.Weapon, "勇者之剑", "英雄的第一步", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = 0.2,
                    MagicAtkBonus = 0.11,
                    SwiftBonus = 0.06,
                    CriticalDamageBonus = 0.05
                }
            };
            WeaponList[2].Add(weapon2_5);
            var weapon2_6 = new Equipment(EquipmentCategory.Weapon, "精致法杖", "你足够精致吗？", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = 0.1,
                    MagicAtkBonus = 0.16,
                    SwiftBonus = 0.07,
                    CriticalDamageBonus = 0.02
                }
            };
            WeaponList[2].Add(weapon2_6);
            var weapon2_7 = new Equipment(EquipmentCategory.Weapon, "光剑", "在剑护手上装备了R!G!B!", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = 0.1,
                    MagicAtkBonus = 0.1,
                    SwiftBonus = 0.1,
                    CriticalDamageBonus = 0.05
                }
            };
            WeaponList[2].Add(weapon2_7);
            var weapon2_8 = new Equipment(EquipmentCategory.Weapon, "彩虹喵", "喵喵喵", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = 0.11,
                    MagicAtkBonus = 0.12,
                    SwiftBonus = 0.03,
                    CriticalProbabilityBonus = 0.07
                }
            };
            WeaponList[2].Add(weapon2_8);
            var weapon2_9 = new Equipment(EquipmentCategory.Weapon, "马桶蹶子", "蹶蹶你的", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = 0.1,
                    MagicAtkBonus = 0.2,
                    SwiftBonus = 0.06,
                    CriticalDamageBonus = 0.2
                }
            };
            WeaponList[2].Add(weapon2_9);
            var weapon2_10 = new Equipment(EquipmentCategory.Weapon, "金光大板砖", "最好的伴侣", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = 0.3,
                    MagicAtkBonus = 0.2,
                }
            };
            WeaponList[2].Add(weapon2_10);
            var weapon2_11 = new Equipment(EquipmentCategory.Weapon, "大鸟转转转", "卧槽，男同。", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = 0.2,
                    SpeedBonus = 0.3,
                    CriticalProbabilityBonus = 0.3
                }
            };
            WeaponList[2].Add(weapon2_11);
            // armor2
            var armor2_1 = new Equipment(EquipmentCategory.Armor, "比基尼铠甲", "这个东西竟然有防御力?", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalDefBonus = 0.2,
                    MagicDefBonus = 0.2,
                    CriticalProbabilityBonus = 0.1
                }
            };
            ArmorList[2].Add(armor2_1);
            var armor2_2 = new Equipment(EquipmentCategory.Armor, "射手护腕", "使弓箭射的更准。不过不是你，而是对方呢", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    SwiftBonus = -0.12
                }
            };
            ArmorList[2].Add(armor2_2);
            var armor2_3 = new Equipment(EquipmentCategory.Armor, "精灵护腕", "精灵以纤细灵巧著称，穿戴上这个精灵族的护腕，你感觉脚下生风", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    SpeedBonus = 0.5
                }
            };
            ArmorList[2].Add(armor2_3);
            var armor2_4 = new Equipment(EquipmentCategory.Armor, "精灵披风", "精灵以纤细灵巧著称，穿戴上这个精灵族的披风，你感觉整个世界都变慢了一点", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    SwiftBonus = 0.5
                }
            };
            ArmorList[2].Add(armor2_4);
            var armor2_5 = new Equipment(EquipmentCategory.Armor, "精灵披风", "一枚盾牌形状的胸针。穿戴后你感觉一层无形的护盾包裹着你", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    MaxShieldBonus = 0.8
                }
            };
            ArmorList[2].Add(armor2_5);
            var armor2_6 = new Equipment(EquipmentCategory.Armor, "引弹护甲", "这身护甲上有着弓、箭矢和靶子的花纹", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalDefBonus = 0.2,
                    SwiftBonus = -0.25,
                    MaxShieldBonus = 0.3
                }
            };
            ArmorList[2].Add(armor2_6);
            var armor2_7 = new Equipment(EquipmentCategory.Armor, "矮人腰带", "穿戴它，可能会发生一些胡子上的异变", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    MaxHpBonus = 0.5
                }
            };
            ArmorList[2].Add(armor2_7);
            var armor2_8 = new Equipment(EquipmentCategory.Armor, "秘银甲", "秘银打造", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalDefBonus = 0.2,
                    MagicDefBonus = -0.1,
                    SwiftBonus = 1
                }
            };
            ArmorList[2].Add(armor2_8);
            var armor2_9 = new Equipment(EquipmentCategory.Armor, "破碎三重冕", "其形似教宗三重冕，有神圣尊贵气质流露", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalDefBonus = 0.1,
                    MagicDefBonus = 0.2,
                    MaxHpBonus = 0.5,
                    MaxShieldBonus = 0.5
                }
            };
            ArmorList[2].Add(armor2_9);
            // jewelry2
            JewelryList[1].Add(jewelry1_1);
            var jewelry2_1 = new Equipment(EquipmentCategory.Jewelry, "精良的防御挂坠", "闪着亮光", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalDefBonus = 0.14,
                    MagicDefBonus = 0.24
                }
            };
            JewelryList[2].Add(jewelry2_1);
            var jewelry2_2 = new Equipment(EquipmentCategory.Jewelry, "精良的攻击挂坠", "闪着亮光", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    MagicAtkBonus = 0.24,
                    PhysicalAtkBonus = 0.14
                }
            };
            JewelryList[2].Add(jewelry2_2);
            var jewelry2_3 = new Equipment(EquipmentCategory.Jewelry, "精良的生存挂坠", "闪着亮光", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    MaxHpBonus = 0.24,
                    MaxShieldBonus = 0.14
                }
            };
            JewelryList[2].Add(jewelry2_3);
            var jewelry2_4 = new Equipment(EquipmentCategory.Jewelry, "精良的迅捷挂坠", "闪着亮光", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    SwiftBonus = 0.14,
                    SpeedBonus = 0.24
                }
            };
            JewelryList[2].Add(jewelry2_4);
            var jewelry2_5 = new Equipment(EquipmentCategory.Jewelry, "精良的幸运挂坠", "闪着亮光", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    CriticalProbabilityBonus = 0.24,
                    CriticalDamageBonus = 0.14
                }
            };
            JewelryList[2].Add(jewelry2_5);
            var jewelry2_6 = new Equipment(EquipmentCategory.Jewelry, "小黄书", "你懂的 注意身体", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    SpeedBonus = 0.4,
                    CriticalProbabilityBonus = 0.2,
                    CriticalDamageBonus = 0.1
                }
            };
            JewelryList[2].Add(jewelry2_6);
            #endregion

            #region 3级道具
            //weapon3
            var weapon3_1 = new Equipment(EquipmentCategory.Weapon, "HK416D", "指挥官只要有416就够了", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = 0.8,
                    MagicAtkBonus = -0.4,
                    SpeedBonus = 1,
                    SwiftBonus = 1,
                    CriticalDamageBonus = 1,
                    CriticalProbabilityBonus = 1
                }
            };
            WeaponList[3].Add(weapon3_1);
            //armor3
            var armor3_1 = new Equipment(EquipmentCategory.Armor, "诅咒精金铠甲", "这套护甲被上个主人的恶灵所附，还是不要穿的好", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    MagicDefBonus = -0.2,
                    PhysicalDefBonus = -0.2
                }
            };
            ArmorList[3].Add(armor3_1);
            var armor3_2 = new Equipment(EquipmentCategory.Armor, "精金铠甲", "这套护甲以一种现存最坚硬的物质——精金强化", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    MagicDefBonus = 0.4,
                    PhysicalDefBonus = 0.4
                }
            };
            ArmorList[3].Add(armor3_2);
            //jewelry3
            var jewelry3_1 = new Equipment(EquipmentCategory.Jewelry, "雪豹之牙", "由雪豹中的变种芝士雪豹所产出的牙齿", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = 0.1,
                    PhysicalDefBonus = 0.1,
                    MagicAtkBonus = 0.1,
                    MagicDefBonus = 0.1,
                    CriticalDamageBonus = 0.1
                }
            };
            JewelryList[3].Add(jewelry3_1);
            #endregion

            #region 4级道具
            //weapon4
            var weapon4_1 = new Equipment(EquipmentCategory.Weapon, "381mm鱼雷", "VV IS WATCHING YOU", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = 0.8,
                    MagicAtkBonus = 0.6,
                    SpeedBonus = 2,
                    CriticalDamageBonus = 2,
                    CriticalProbabilityBonus = 0.5
                }
            };
            WeaponList[4].Add(weapon4_1);
            var weapon4_2 = new Equipment(EquipmentCategory.Weapon, "IWS-2000", "格里芬木星炮（确信）", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = -0.6,
                    MagicAtkBonus = 0.8,
                    SpeedBonus = 2,
                    SwiftBonus = 2,
                    CriticalDamageBonus = 4,
                    CriticalProbabilityBonus = 1
                }
            };
            WeaponList[4].Add(weapon4_2);
            var weapon4_3 = new Equipment(EquipmentCategory.Weapon, "残虹", "天外陨铁打造 荆轲同款", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = 1.6,
                    MagicAtkBonus = 0.1,
                    SpeedBonus = -0.3,
                    SwiftBonus = -0.3,
                    CriticalDamageBonus = 2,
                    CriticalProbabilityBonus = 2
                }
            };
            WeaponList[4].Add(weapon4_3);
            //armor4
            var armor4_1 = new Equipment(EquipmentCategory.Armor, "黄金甲", "不是圣斗士哦", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    MagicDefBonus = 0.8,
                    PhysicalDefBonus = 0.8,
                    MaxHpBonus = 2,
                    MaxShieldBonus = 2.4
                }
            };
            ArmorList[4].Add(armor4_1);
            //jewelry4
            var jewelry4_1 = new Equipment(EquipmentCategory.Jewelry, "牛子挂饰", "神秘的力量", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    MagicDefBonus = 0.8,
                    PhysicalDefBonus = 0.8,
                    MaxHpBonus = 2,
                    MaxShieldBonus = 2.4,
                    PhysicalAtkBonus = 1.6,
                    MagicAtkBonus = 1.6,
                    SpeedBonus = 2,
                }
            };
            JewelryList[4].Add(jewelry4_1);
            #endregion

            #region 5级道具
            //weapon5
            var weapon5_1 = new Equipment(EquipmentCategory.Weapon, "KA-50黑鲨", "挂载恐怖的涡流反坦克导弹，但其实是白毛萝莉美少女", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = 3.2,
                    MagicAtkBonus = -0.91,
                    SpeedBonus = 4,
                    SwiftBonus = -0.5,
                    CriticalDamageBonus = 8,
                    CriticalProbabilityBonus = 2
                }
            };
            WeaponList[5].Add(weapon5_1);
            //armor5
            var armor5_1 = new Equipment(EquipmentCategory.Armor, "金光龟壳", "好像是某种仙人的遗物", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    MagicDefBonus = 1.6,
                    PhysicalDefBonus = 3.2,
                    SpeedBonus = -0.4,
                    SwiftBonus = -0.4,
                    MaxHpBonus = 4,
                    MaxShieldBonus = 9.6
                }
            };
            ArmorList[5].Add(armor5_1);
            //jewelry5
            var jewelry5_1 = new Equipment(EquipmentCategory.Jewelry, "雪风的快速修理装置", "雪风会保护大家的", 1, false)
            {
                EquipmentEffect = new EquipmentEffect()
                {
                    PhysicalAtkBonus = 1.6,
                    PhysicalDefBonus = 0.114,
                    MagicAtkBonus = 1.6,
                    MagicDefBonus = 0.114,
                    SpeedBonus = 2,
                    SwiftBonus = -0.7,
                    MaxHpBonus = 0.19,
                    MaxShieldBonus = 0.81
                }
            };
            JewelryList[5].Add(jewelry5_1);
            #endregion
        }
    }
}
