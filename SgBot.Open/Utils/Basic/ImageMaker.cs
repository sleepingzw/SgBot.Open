using SgBot.Open.DataTypes.BotFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgBot.Open.DataTypes.StaticData;
using SkiaSharp;

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
            // using var skTypeface = SKTypeface.FromFile("C:\\AlisaBot\\Fonts\\华康少女文字W5.ttf", 0);
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
                $"Data\\Temp\\SortTempImage\\{DateTime.Now:yyyy-M-dd--HH-mm-ss}.png");
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
