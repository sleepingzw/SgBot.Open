using SgBot.Open.DataTypes.SgGame.GameLibrary;
using SgBot.Open.DataTypes.SgGame;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgBot.Open.DataTypes.StaticData;
using SgBot.Open.Utils.Basic;
using Manganese.Text;
using System.Numerics;
using Spectre.Console;

namespace SgBot.Open.Utils.Extra
{
    public static class GameImageMaker
    {
        #region 笔刷初始化
        static int index = SKFontManager.Default.FontFamilies.ToList().IndexOf("宋体"); // 创建宋体字形
        // using var skTypeface = SKTypeface.FromFile("C:\\AlisaBot\\Fonts\\华康少女文字W5.ttf", 0);
        static SKTypeface skTypeface = SKFontManager.Default.GetFontStyles(index).CreateTypeface(0);
        static SKFont skFont = new SKFont(skTypeface, 20);
        static SKPaint skTextPaint = new SKPaint
        {
            Color = SKColors.Black,
            TextEncoding = SKTextEncoding.Utf8,
            Typeface = skTypeface,
            TextSize = 18,
            IsAntialias = true
        };
        static SKPaint skSmallPaint = new SKPaint
        {
            Color = SKColors.Black,
            TextEncoding = SKTextEncoding.Utf8,
            Typeface = skTypeface,
            TextSize = 12,
            IsAntialias = true
        };
        static SKPaint skBlackPaint = new SKPaint
        {
            Color = SKColors.Black,
            TextEncoding = SKTextEncoding.Utf8,
            Typeface = skTypeface,
            TextSize = 20,
            IsAntialias = true
        };
        static SKPaint skBigBlackPaint = new SKPaint
        {
            Color = SKColors.Black,
            TextEncoding = SKTextEncoding.Utf8,
            Typeface = skTypeface,
            TextSize = 40,
            IsAntialias = true
        };
        static SKPaint skRedPaint = new SKPaint
        {
            Color = SKColors.Red,
            TextEncoding = SKTextEncoding.Utf8,
            Typeface = skTypeface,
            TextSize = 20,
            IsAntialias = true
        };
        static SKPaint skPurplePaint = new SKPaint
        {
            Color = SKColors.Purple,
            TextEncoding = SKTextEncoding.Utf8,
            Typeface = skTypeface,
            TextSize = 20,
            IsAntialias = true
        };
        static SKPaint skBluePaint = new SKPaint
        {
            Color = SKColors.Blue,
            TextEncoding = SKTextEncoding.Utf8,
            Typeface = skTypeface,
            TextSize = 20,
            IsAntialias = true
        };
        static SKPaint skOrangeRedPaint = new SKPaint
        {
            Color = SKColors.OrangeRed,
            TextEncoding = SKTextEncoding.Utf8,
            Typeface = skTypeface,
            TextSize = 20,
            IsAntialias = true
        };
        #endregion
        /// <summary>
        /// 生产玩家信息
        /// </summary>
        /// <param name="player">玩家实体</param>
        /// <returns></returns>
        public static string MakeSgGamePlayerImage(Player player)
        {
            var skInfo = new SKImageInfo(480, 290);
            using (var surface = SKSurface.Create(skInfo))
            {
                using var glCanvas = surface.Canvas;

                glCanvas.DrawColor(SKColors.White, SKBlendMode.Src);

                glCanvas.DrawText($"{player.Name}({player.Id}):", 3f, 25, skTextPaint);
                glCanvas.DrawText(player.IsWinToday ? "今日已获得首胜" : "今日未获得首胜", 350f, 25, skTextPaint);
                glCanvas.DrawText($"Lv.{player.Level} Rk.{player.Rank} RkScore:{player.RankScore}", 3f, 45, skTextPaint);
                glCanvas.DrawText($"Exp:{player.Exp},金币:{player.Coin},体力：{player.Power}", 3f, 65, skTextPaint);
                glCanvas.DrawText(
                    $"力量:{player.Strength},体质{player.Fitness},敏捷:{player.Agility},智力:{player.Intelligence}", 3f, 85,
                    skTextPaint);
                glCanvas.DrawText(
                    $"自由属性点:{player.FreePoints}", 3f, 105,
                    skTextPaint);
                var onBody = new List<Equipment>();
                foreach (var equip in player.Bag)
                {
                    if (equip.OnBody)
                    {
                        onBody.Add(equip);
                    }
                }

                var lines = 1;
                if (onBody.Count != 0)
                {
                    glCanvas.DrawText($"当前装备:", 3f, 125, skTextPaint);
                    var i = 0;
                    foreach (var it in onBody)
                    {
                        i++;
                        switch (it.Category)
                        {
                            case EquipmentCategory.Weapon:
                                glCanvas.DrawText($"武器:{it.Name}".PadRight(15) + $"Rk.{it.Level}", 3, 125 + 20 * lines,
                                    skTextPaint);
                                lines++;
                                glCanvas.DrawText(it.ShowShortEffect(), 3, 120 + 20 * lines,
                                    skSmallPaint);
                                lines++;
                                break;
                            case EquipmentCategory.Armor:
                                glCanvas.DrawText($"防具:{it.Name}".PadRight(15) + $"Rk.{it.Level}", 3, 125 + 20 * lines,
                                    skTextPaint);
                                lines++;
                                glCanvas.DrawText(it.ShowShortEffect(), 3, 120 + 20 * lines,
                                    skSmallPaint);
                                lines++;
                                break;
                            case EquipmentCategory.Jewelry:
                                glCanvas.DrawText($"饰品:{it.Name}".PadRight(15) + $"Rk.{it.Level}", 3, 125 + 20 * lines,
                                    skTextPaint);
                                lines++;
                                glCanvas.DrawText(it.ShowShortEffect(), 3, 120 + 20 * lines,
                                    skSmallPaint);
                                lines++;
                                break;
                        }
                    }
                }
                else
                {
                    glCanvas.DrawText($"当前装备: 无装备", 3f, 125, skTextPaint);
                }
                glCanvas.DrawText($"背包内有 {player.Bag.Count} 件装备 (输入 /game.bag 查看)", 3f, 265, skTextPaint);
                if (player.SkillActive.Count != 0)
                {
                    glCanvas.DrawText($"当前技能:{SkillLibrary.Skills[player.SkillActive.First().Key].Name} " +
                        $"等级：{player.SkillActive.First().Value}", 3f, 285, skTextPaint);
                }
                else
                {
                    glCanvas.DrawText($"当前技能:无技能", 3f, 285, skTextPaint);
                }

                var ret = Path.Combine(StaticData.ExePath!,
                    $"Data/Temp/PlayerInfoTempImage/{player.Id}-{DateTime.Now:yyyy-M-dd--HH-mm-ss-ff}.png");
                using (var image = surface.Snapshot())
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var stream = File.OpenWrite(ret))
                {
                    data.SaveTo(stream);
                }

                Logger.Log($"Img {ret} 生成成功", LogLevel.Simple);
                return ret;
            }
        }
        /// <summary>
        /// 生成PVP结果图
        /// </summary>
        /// <param name="player">玩家实体</param>
        /// <param name="result">结果</param>
        /// <param name="coinGet">获得的金币</param>
        /// <param name="levelUp">升级的等级</param>
        /// <returns></returns>
        public static string MakeSgGamePveImage(Player player, SgGamePveResult result, long coinGet,
            long levelUp)
        {
            var skInfo = new SKImageInfo(600, 10 + (result.Details.Count + 2) * 20);
            using (var surface = SKSurface.Create(skInfo))
            {
                using var glCanvas = surface.Canvas;

                glCanvas.DrawColor(SKColors.White, SKBlendMode.Src);

                glCanvas.DrawText($"{player.Name}({player.Id}):进入傻狗大陆探险", 3f, 25, skTextPaint);
                var i = 0;
                foreach (var detail in result.Details)
                {
                    i++;
                    switch (detail.Encounter)
                    {
                        case PveEncounter.GetEquip:
                            if (detail.AddEquipmentSucess)
                            {
                                glCanvas.DrawText($"第{i}次:获得装备{detail.EquipmentGet.Name}", 3f, 25 + i * 20, skTextPaint);
                            }
                            else
                            {
                                glCanvas.DrawText($"第{i}次:背包已满 无法拾取装备 输入 /game.throw 编号 丢弃不用的装备", 3f, 25 + i * 20, skTextPaint);
                            }
                            break;
                        case PveEncounter.GetCoin:
                            glCanvas.DrawText($"第{i}次:获得金币{detail.CoinGet}", 3f, 25 + i * 20, skTextPaint);
                            break;
                        case PveEncounter.GetExp:
                            glCanvas.DrawText($"第{i}次:获得Exp{detail.ExpGet}", 3f, 25 + i * 20, skTextPaint);
                            break;
                        case PveEncounter.Other:
                            break;
                    }

                    if (levelUp > 0)
                    {
                        glCanvas.DrawText($"总共获得{coinGet}金币,升了{levelUp}级", 3f, 45 + result.Details.Count * 20,
                            skTextPaint);
                    }
                    else
                    {
                        glCanvas.DrawText($"总共获得{coinGet}金币", 3f, 45 + result.Details.Count * 20, skTextPaint);
                    }
                }

                var ret = Path.Combine(StaticData.ExePath!,
                    $"Data/Temp/PlayerPveTempImage/{player.Id}-{DateTime.Now:yyyy-M-dd--HH-mm-ss-ff}.png");
                using (var image = surface.Snapshot())
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var stream = File.OpenWrite(ret))
                {
                    data.SaveTo(stream);
                }

                Logger.Log($"Img {ret} 生成成功", LogLevel.Simple);
                return ret;
            }
        }
        /// <summary>
        /// 生成背包图片
        /// </summary>
        /// <param name="player">玩家实体</param>
        /// <returns></returns>
        public static string MakeSgGameBag(Player player)
        {
            var skInfo = new SKImageInfo(680, 70 + (player.Bag.Count) * 60);
            using (var surface = SKSurface.Create(skInfo))
            {
                using var glCanvas = surface.Canvas;
                var lockimg = SKBitmap.Decode(Path.Combine(StaticData.ExePath!, "Data/Img/GameRepository/lock.png"));
                var strimg = SKBitmap.Decode(Path.Combine(StaticData.ExePath!, "Data/Img/GameRepository/str.png"));
                // Console.WriteLine(lockimg.Width);
                var lines = 1;
                glCanvas.DrawColor(SKColors.White, SKBlendMode.Src);
                glCanvas.DrawText($"{player.Name}({player.Id})", 3f, 25, skTextPaint);
                if (player.Bag.Count != 0)
                {
                    glCanvas.DrawText($"背包装备如下:", 3f, 25 + lines * 20, skTextPaint);
                    lines++;
                    var i = 0;
                    foreach (var equipment in player.Bag)
                    {
                        i++;
                        if (equipment.IsLock)
                        {
                            // glCanvas.DrawText("🔒", 3f,25 + lines * 20, skOrangeRedPaint);
                            glCanvas.DrawBitmap(lockimg, 0f, 4 + lines * 20);
                        }
                        if(equipment.OnBody)
                        {
                            // glCanvas.DrawText("💪", 3f, 25 + lines * 20, skPurplePaint);
                            glCanvas.DrawBitmap(strimg, 25f, 5 + lines * 20);
                        }
                        glCanvas.DrawText(
                            $"{i}".PadRight(2) + ":" + $"{equipment.Name}".PadRight(15) + $"Rk.{equipment.Level}", 50f,
                            25 + lines * 20, skTextPaint);
                        lines++;
                        glCanvas.DrawText(
                            $"  {equipment.Description}", 3f, 24 + lines * 20, skSmallPaint);
                        lines++;

                        glCanvas.DrawText($"  {equipment.ShowLongEffect()}", 3f, 25 + lines * 20, skSmallPaint);
                        lines++;
                    }
                }
                else
                {
                    glCanvas.DrawText("背包空空如也", 3f, 25 + lines * 20, skTextPaint);
                    lines++;
                }
                glCanvas.DrawText("输入 /game.equip 编号来装备背包中的物品", 3f, 25 + lines * 20, skTextPaint);
                var ret = Path.Combine(StaticData.ExePath!,
                    $"Data/Temp/PlayerBagTempImage/{player.Id}-{DateTime.Now:yyyy-M-dd--HH-mm-ss-ff}.png");
                using (var image = surface.Snapshot())
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var stream = File.OpenWrite(ret))
                {
                    data.SaveTo(stream);
                }

                Logger.Log($"Img {ret} 生成成功", LogLevel.Simple);
                return ret;
            }
        }
        /// <summary>
        /// 生成PVP战斗流程图
        /// </summary>
        /// <param name="log">战斗log</param>
        /// <param name="playerId">玩家id</param>
        /// <param name="result">战斗结果</param>
        /// <returns></returns>
        public static string MakePVPBattleImage(BattleLog log, string playerId, SgGamePvpResult result)
        {
            var skInfo = new SKImageInfo(1128,
                110 + log.OutLines() * 20 + (result.IsRankChange ? 1 : 0) + (result.IsUpgrade ? 1 : 0));
            using (var surface = SKSurface.Create(skInfo))
            {
                using var glCanvas = surface.Canvas;

                glCanvas.DrawColor(SKColors.White, SKBlendMode.Src);
                glCanvas.DrawText($"{log.PlayerName}({playerId})对阵{log.EnemyName}", 3f, 25, skBlackPaint);
                var i = 0;
                var lines = 1;
                var super = new List<bool>();
                for (var j = 0; j < 1 + log.Details.Count / 10; j++)
                {
                    super.Add(false);
                }
                foreach (var detail in log.Details)
                {
                    i++;
                    if (i > 10)
                    {
                        if (!super[i / 10])
                        {
                            glCanvas.DrawText($"{i / 10}阶狂暴模式开启", 480f, 25 + 20 * lines,
                                skRedPaint);
                            super[i / 10] = true;
                            lines++;
                        }
                    }

                    var battleText = "";
                    var isSkill = false;
                    if (!detail.PostiveSkillAction.IsNullOrEmpty())
                    {
                        glCanvas.DrawText($"{detail.PostiveSkillAction}", 3f, 25 + 20 * lines,
                            skPurplePaint);
                        lines++;
                    }

                    battleText += "第" + $"{i}".PadLeft(2, '0') + "轮";
                    if (detail.IsPlayerAttack)
                    {
                        battleText += $" {log.PlayerName} 攻击";
                    }
                    else
                    {
                        battleText += $" {log.EnemyName} 攻击";
                    }

                    if (detail.IsCritical)
                    {
                        battleText += " *暴击*";
                    }

                    if (detail.IsMiss)
                    {
                        battleText += " !但是没有命中!";
                    }
                    else
                    {
                        battleText += $" 造成了{detail.AttackerDmg}点伤害";
                    }
                    glCanvas.DrawText(battleText, 3f, 25 + 20 * lines,
                        skBlackPaint);
                    lines++;

                    var playerHpBar = (int)((double)detail.PlayerHpNow / (double)detail.PlayerHpMax * 50);
                    var playerHpBarString = "";
                    for (var j = 0; j < playerHpBar; j++)
                    {
                        playerHpBarString += "-";
                    }

                    var enemyHpBar = (int)((double)detail.EnemyHpNow / (double)detail.EnemyHpMax * 50);
                    var enemyHpBarString = "";
                    for (var j = 0; j < enemyHpBar; j++)
                    {
                        enemyHpBarString += "-";
                    }

                    var HpText = playerHpBarString.PadLeft(50) + detail.PlayerHpNow.ToString().PadRight(5) + "||" + detail.EnemyHpNow.ToString().PadLeft(5) +
                                 enemyHpBarString;
                    glCanvas.DrawText(HpText, 3f, 25 + 20 * lines,
                        skRedPaint);
                    lines++;

                    var playerShieldBar = (int)((double)detail.PlayerShieldNow / (double)detail.PlayerShieldMax * 50);
                    var playerShieldBarString = "";
                    for (var j = 0; j < playerShieldBar; j++)
                    {
                        playerShieldBarString += "-";
                    }

                    var enemyShieldBar = (int)((double)detail.EnemyShieldNow / (double)detail.EnemyShieldMax * 50);
                    var enemyShieldBarString = "";
                    for (var j = 0; j < enemyShieldBar; j++)
                    {
                        enemyShieldBarString += "-";
                    }

                    var ShieldText = playerShieldBarString.PadLeft(50) + detail.PlayerShieldNow.ToString().PadRight(5) + "||" + detail.EnemyShieldNow.ToString().PadLeft(5) +
                                     enemyShieldBarString;
                    glCanvas.DrawText(ShieldText, 3f, 25 + 20 * lines,
                        skBluePaint);
                    lines++;
                }

                if (log.IsWin)
                {
                    glCanvas.DrawText("您获胜了", 480f, 40 + 20 * lines,
                        skBigBlackPaint);
                    lines++;
                    glCanvas.DrawText(
                        result.IsUpgrade ? $"获得了 {result.CoinGet} 金币 {result.ExpGet}EXP 你升级了" : $"获得了 {result.CoinGet} 金币 {result.ExpGet}EXP",
                        3f, 40 + 20 * lines,
                        skBlackPaint);
                    lines++;
                    glCanvas.DrawText(
                        result.IsRankChange ? $"获得了 {result.RankScoreChange} 段位分 你上段了" : $"获得了 {result.RankScoreChange} 段位分",
                        3f, 40 + 20 * lines,
                        skBlackPaint);
                }
                else
                {
                    glCanvas.DrawText("您落败了", 480f, 40 + 20 * lines,
                        skBigBlackPaint);
                    lines++;
                    glCanvas.DrawText(
                        result.IsUpgrade ? $"获得了 {result.CoinGet} 金币 {result.ExpGet}EXP 你升级了" : $"获得了 {result.CoinGet} 金币 {result.ExpGet}EXP",
                        3f, 40 + 20 * lines,
                        skBlackPaint);
                    lines++;
                    glCanvas.DrawText(
                        result.IsRankChange ? $"失去了 {result.RankScoreChange} 段位分 你掉段了" : $"失去了 {result.RankScoreChange} 段位分",
                        3f, 40 + 20 * lines,
                        skBlackPaint);
                }
                var ret = Path.Combine(StaticData.ExePath!,
                    $"Data/Temp/PlayerBattleTempImage/{playerId}-{DateTime.Now:yyyy-M-dd--HH-mm-ss-ff}.png");
                using (var image = surface.Snapshot())
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var stream = File.OpenWrite(ret))
                {
                    data.SaveTo(stream);
                }

                Logger.Log($"Img {ret} 生成成功", LogLevel.Simple);
                return ret;
            }
        }
        /// <summary>
        /// 生成傻狗之巅排行版
        /// </summary>
        /// <param name="list">排行的表</param>
        /// <returns></returns>
        public static string MakeSgGameRankImage(List<string> list)
        {
            var skInfo = new SKImageInfo(480, 55 + (list.Count * 20));
            using (var surface = SKSurface.Create(skInfo))
            {
                using var glCanvas = surface.Canvas;

                glCanvas.DrawColor(SKColors.White, SKBlendMode.Src);
                glCanvas.DrawText("~傻狗之巅~", 150f, 40, skBigBlackPaint);
                var lines = 1;
                foreach (var line in list)
                {
                    glCanvas.DrawText(line, 3f, 50 + lines * 20, skTextPaint);
                    lines++;
                }
                var ret = Path.Combine(StaticData.ExePath!,
                    $"Data/Temp/PlayerRankTempImage/{DateTime.Now:yyyy-M-dd--HH-mm-ss}.png");
                if (File.Exists(ret))
                {
                    return ret;
                }
                using (var image = surface.Snapshot())
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var stream = File.OpenWrite(ret))
                {
                    data.SaveTo(stream);
                }

                Logger.Log($"Img {ret} 生成成功", LogLevel.Simple);
                return ret;
            }
        }
        /// <summary>
        /// 生产其他人信息
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static string MakeOtherInfoImage(Player player)
        {
            var skInfo = new SKImageInfo(360, 190);
            using (var surface = SKSurface.Create(skInfo))
            {
                using var glCanvas = surface.Canvas;

                glCanvas.DrawColor(SKColors.White, SKBlendMode.Src);

                glCanvas.DrawText($"{player.Name}({player.Id}):", 3f, 25, skTextPaint);
                glCanvas.DrawText($"Lv.{player.Level} Rk.{player.Rank}", 3f, 45, skTextPaint);

                var array = new long[4];
                array[0] = player.Strength;
                array[1] = player.Fitness;
                array[2] = player.Agility;
                array[3] = player.Intelligence;
                var max = array.Max();
                var num = array.TakeWhile(ii => ii != max).Count();

                switch (num)
                {
                    case 0:
                        glCanvas.DrawText("主属性为力量", 3f, 65, skTextPaint);
                        break;
                    case 1:
                        glCanvas.DrawText("主属性为体制", 3f, 65, skTextPaint);
                        break;
                    case 2:
                        glCanvas.DrawText("主属性为敏捷", 3f, 65, skTextPaint);
                        break;
                    case 3:
                        glCanvas.DrawText("主属性为智力", 3f, 65, skTextPaint);
                        break;
                    default:
                        break;
                }

                var onBody = player.Bag.Where(equip => equip.OnBody).ToList();
                var lines = 1;
                if (onBody.Count != 0)
                {
                    glCanvas.DrawText($"当前装备:", 3f, 85, skTextPaint);
                    var i = 0;
                    foreach (var it in onBody)
                    {
                        i++;
                        switch (it.Category)
                        {
                            case EquipmentCategory.Weapon:
                                glCanvas.DrawText($"武器:{it.Name}".PadRight(15) + $"Rk.{it.Level}", 3, 85 + 20 * lines,
                                    skTextPaint);
                                lines++;
                                break;
                            case EquipmentCategory.Armor:
                                glCanvas.DrawText($"防具:{it.Name}".PadRight(15) + $"Rk.{it.Level}", 3, 85 + 20 * lines,
                                    skTextPaint);
                                lines++;
                                break;
                            case EquipmentCategory.Jewelry:
                                glCanvas.DrawText($"饰品:{it.Name}".PadRight(15) + $"Rk.{it.Level}", 3, 85 + 20 * lines,
                                    skTextPaint);
                                lines++;
                                break;
                        }
                    }
                }
                else
                {
                    glCanvas.DrawText($"当前装备: 无装备", 3f, 125, skTextPaint);
                }

                glCanvas.DrawText(
                    player.SkillActive.Count != 0
                        ? $"当前技能:{SkillLibrary.Skills[player.SkillActive[0]].Name}"
                        : "当前技能:无技能", 3f,
                    165, skTextPaint);

                var ret = Path.Combine(StaticData.ExePath!,
                    $"Data/Temp/OtherInfoTempImage/{player.Id}-{DateTime.Now:yyyy-M-dd--HH-mm-ss-ff}.png");
                using (var image = surface.Snapshot())
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var stream = File.OpenWrite(ret))
                {
                    data.SaveTo(stream);
                }

                Logger.Log($"Img {ret} 生成成功", LogLevel.Simple);
                return ret;
            }
        }
    }
}
