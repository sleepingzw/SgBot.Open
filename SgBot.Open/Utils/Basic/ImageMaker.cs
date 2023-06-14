using SgBot.Open.DataTypes.BotFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using SgBot.Open.DataTypes.StaticData;
using SkiaSharp;
using System.Xml.Linq;

namespace SgBot.Open.Utils.Basic
{
    /// <summary>
    /// 生成常规图片
    /// </summary>
    public static class ImageMaker
    {
        /// <summary>
        /// 生成傻狗力数量排行图片
        /// </summary>
        /// <param name="infoList">总的傻狗力数据榜</param>
        /// <param name="info">需要查询的用户数据</param>
        /// <returns></returns>
        public static string MakeSortImage(List<UserInfo> infoList, UserInfo info)
        {
            var u = 0;
            foreach (var i in infoList)
            {
                u++;
                if (i.UserId == info.UserId)
                {
                    break;
                }
            }
            var skInfo = new SKImageInfo(900, 380);
            using var surface = SKSurface.Create(skInfo);
            using var glCanvas = surface.Canvas;
            var index = SKFontManager.Default.FontFamilies.ToList().IndexOf("宋体"); // 创建宋体字形
            var skTypeface = SKFontManager.Default.GetFontStyles(index).CreateTypeface(0);
            // using var skTypeface = SKTypeface.FromFile("C:/AlisaBot/Fonts/华康少女文字W5.ttf", 0);
            using var skFont = new SKFont(skTypeface, 30);
            using var skTextPaint = new SKPaint
            {
                Color = SKColors.Black,
                TextEncoding = SKTextEncoding.Utf8,
                Typeface = skTypeface,
                TextSize = 30,
                IsAntialias = true
            };
            using var skPaint = new SKPaint
            {
                BlendMode = SKBlendMode.SrcATop,
                FilterQuality = SKFilterQuality.High,
                IsAntialias = true
            };
            glCanvas.DrawColor(SKColors.White, SKBlendMode.Src);
            for (var i = 0; i < 10; i++)
            {
                glCanvas.DrawText($"第{i + 1}".PadRight(2) + $"位:{infoList[i].Nickname},持有{infoList[i].Token}傻狗力", 3f,
                    (i + 1) * 30 + 5, skTextPaint);
            }

            glCanvas.DrawText($"您是第{u}位,共有{infoList.Count}条数据", 3f, 340, skTextPaint);
            var ret = Path.Combine(StaticData.ExePath!,
                $"Data/Temp/SortTempImage/{DateTime.Now:yyyy-M-dd--HH-mm-ss}.png");
            using (var image = surface.Snapshot())
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var stream = File.OpenWrite(ret))
            {
                data.SaveTo(stream);
            }

            Logger.Log($"Img {ret} 生成成功", LogLevel.Simple);
            return ret;
        }
        /// <summary>
        /// 生成傻狗牌
        /// </summary>
        /// <param name="cardList"></param>
        /// <returns></returns>
        public static string MakeCardImage(string name,int[] clist)
        {
            var skInfo = new SKImageInfo(122, 68);
            var ret = Path.Combine(StaticData.ExePath!,
                $"Data/Temp/CardTempImage/{name}-{DateTime.Now:yyyy-M-dd--HH-mm-ss}.png");
            using var surface = SKSurface.Create(skInfo);
            using var glCanvas = surface.Canvas;
            var index = SKFontManager.Default.FontFamilies.ToList().IndexOf("宋体"); // 创建宋体字形
            var skTypeface = SKFontManager.Default.GetFontStyles(index).CreateTypeface(0);
            using var skFont = new SKFont(skTypeface, 20);
            using var skBlackPaint = new SKPaint
            {
                Color = SKColors.Black,
                TextEncoding = SKTextEncoding.Utf8,
                Typeface = skTypeface,
                TextSize = 20,
                IsAntialias = true
            };
            using var skBigBlackPaint = new SKPaint
            {
                Color = SKColors.Black,
                TextEncoding = SKTextEncoding.Utf8,
                Typeface = skTypeface,
                TextSize = 40,
                IsAntialias = true
            };
            using var skGrayPaint = new SKPaint
            {
                Color = SKColors.Gray,
                TextEncoding = SKTextEncoding.Utf8,
                Typeface = skTypeface,
                TextSize = 20,
                IsAntialias = true
            };
            using var skWhitePaint = new SKPaint
            {
                Color = SKColors.White,
                TextEncoding = SKTextEncoding.Utf8,
                Typeface = skTypeface,
                TextSize = 20,
                IsAntialias = true
            };
            using var skOrangeRedPaint = new SKPaint
            {
                Color = SKColors.OrangeRed,
                TextEncoding = SKTextEncoding.Utf8,
                Typeface = skTypeface,
                TextSize = 20,
                IsAntialias = true
            };
            using var skGreenPaint = new SKPaint
            {
                Color = SKColors.Green,
                TextEncoding = SKTextEncoding.Utf8,
                Typeface = skTypeface,
                TextSize = 20,
                IsAntialias = true
            };
            using var skPurplePaint = new SKPaint
            {
                Color = SKColors.Purple,
                TextEncoding = SKTextEncoding.Utf8,
                Typeface = skTypeface,
                TextSize = 20,
                IsAntialias = true
            };
            using var skBluePaint = new SKPaint
            {
                Color = SKColors.Blue,
                TextEncoding = SKTextEncoding.Utf8,
                Typeface = skTypeface,
                TextSize = 20,
                IsAntialias = true
            };
            glCanvas.DrawColor(SKColors.White, SKBlendMode.Src);

            for (var i = 0; i < 9; i++)
            {
                switch (clist[i])
                {
                    case < 600:
                        DrawNCard(glCanvas, i % 3, i / 3);
                        break;
                    case < 850:
                        DrawRCard(glCanvas, i % 3, i / 3);
                        break;
                    case < 950:
                        DrawSrCard(glCanvas, i % 3, i / 3);
                        break;
                    case < 990:
                        DrawSsrCard(glCanvas, i % 3, i / 3);
                        break;
                    default:
                        DrawUrCard(glCanvas, i % 3, i / 3);
                        break;
                }
            }

            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = File.OpenWrite(ret);
            data.SaveTo(stream);

            void DrawNCard(SKCanvas canvas, int x, int y)
            {
                canvas.DrawRect(3 + x * 40, 2 + y * 22, 36, 20, skGrayPaint);
                canvas.DrawText("N", 3 + 5 + 9 + x * 40, 18 + 22 * y, skWhitePaint);
            }
            void DrawRCard(SKCanvas canvas, int x, int y)
            {
                canvas.DrawRect(3 + x * 40, 2 + y * 22, 36, 20, skBluePaint);
                canvas.DrawText("R", 3 + 5 + 9 + x * 40, 18 + 22 * y, skWhitePaint);
            }
            void DrawSrCard(SKCanvas canvas, int x, int y)
            {
                canvas.DrawRect(3 + x * 40, 2 + y * 22, 36, 20, skGreenPaint);
                canvas.DrawText("SR", 3 + 4 + 4 + x * 40, 18 + 22 * y, skWhitePaint);
            }
            void DrawSsrCard(SKCanvas canvas, int x, int y)
            {
                canvas.DrawRect(3 + x * 40, 2 + y * 22, 36, 20, skOrangeRedPaint);
                canvas.DrawText("SSR", 3 + 3 + x * 40, 18 + 22 * y, skWhitePaint);
            }
            void DrawUrCard(SKCanvas canvas, int x, int y)
            {
                canvas.DrawRect(3 + x * 40, 2 + y * 22, 36, 20, skPurplePaint);
                canvas.DrawText("UR", 3 + 4 + 4 + x * 40, 18 + 22 * y, skWhitePaint);
            }

            return ret;
        }
    }
}
