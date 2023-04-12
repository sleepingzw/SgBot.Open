using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SgBot.Open.DataTypes.StaticData;
using SgBot.Open.Utils.Basic;

namespace SgBot.Open.Utils.Extra
{
    internal class PetPetMaker
    {
        internal static HttpClient Client;
        static PetPetMaker()
        {
            Client = new HttpClient();
        }
        internal static string MakePetPet(string qqid, int delay = 7)
        {
            var resourcesDir = Path.Combine(StaticData.ExePath!, "Data/Img/GifBack");
            if (!Directory.Exists(resourcesDir)) { Directory.CreateDirectory(resourcesDir); }
            var saveDir = Path.Combine(resourcesDir, $"petpet_{qqid}.gif");
            if (File.Exists(saveDir))
            {
                File.Delete(saveDir);
                // return saveDir;
            }
            using var avatarImage = GetAvater(qqid);
            if (avatarImage != null)
            {
                var targetSize = avatarImage.Width;
                using var glSurface = SKSurface.Create(new SKImageInfo(targetSize, targetSize, SKColorType.Argb4444));
                using var skRoundRect = new SKRoundRect(SKRect.Create(0, 0, targetSize, targetSize), targetSize);
                using var skPaint = new SKPaint
                {
                    BlendMode = SKBlendMode.SrcATop,
                    FilterQuality = SKFilterQuality.High,
                    IsAntialias = true
                };
                glSurface.Canvas.Clear();
                glSurface.Canvas.DrawColor(SKColors.White, SKBlendMode.Src); //绘制底色
                glSurface.Canvas.DrawBitmap(avatarImage, 0, 0, skPaint); //绘制头像
                glSurface.Canvas.Scale(112, 112);
                using var avatar = SKBitmap.Decode(glSurface.Snapshot().Encode(SKEncodedImageFormat.Png, 100));
                var images = new List<SKImage> {
                    ProcessPetPetImage(avatar, 0, 100, 100, 12, 16, 0),
                    ProcessPetPetImage(avatar, 1, 105, 88, 12, 28, 0),
                    ProcessPetPetImage(avatar, 2, 110, 76, 12, 40, 6),
                    ProcessPetPetImage(avatar, 3, 107, 84, 12, 32, 0),
                    ProcessPetPetImage(avatar, 4, 100, 100, 12, 16, 0)
                };
                var frames = new List<string>();
                for (int i = 0; i < 5; i++)
                {
                    var image = images[i];
                    using var reader = new BinaryReader(image.Encode(SKEncodedImageFormat.Png, 100).AsStream());
                    var tempFileName = saveDir.Insert(saveDir.IndexOf(qqid) + qqid.Length, $"_{i}").Replace(".gif", ".png");
                    using var stream = File.Open(tempFileName, FileMode.Create);
                    stream.Write(reader.ReadBytes((int)reader.BaseStream.Length));
                    stream.Flush();
                    stream.Close();
                    frames.Add(tempFileName);
                }
                Convert(frames, images.ElementAt(0).Width, images.ElementAt(0).Height, saveDir, delay);
                foreach (var frame in frames)
                {
                    File.Delete(frame);
                }
                return saveDir;
            }
            return string.Empty;
        }
        private static SKImage ProcessPetPetImage(SKBitmap avatarImage, int index, int width, int height, int x, int y, int handoffsety)
        {
            var rgbaImageInfo = new SKImageInfo(width, height, SKColorType.Argb4444);
            var rgbImageInfo = new SKImageInfo(112, 112, SKColorType.Rgb888x);
            using var rgbaSurface = SKSurface.Create(rgbaImageInfo);
            rgbaSurface.Canvas.Clear();
            rgbaSurface.Canvas.DrawBitmap(avatarImage, new SKRect(0, 0, width, height));
            rgbaSurface.Flush();
            using var processingImage = rgbaSurface.Snapshot();
            rgbaSurface.Canvas.Clear();
            using var rgbSurface = SKSurface.Create(rgbImageInfo);
            rgbSurface.Canvas.DrawColor(SKColors.White); //绘制底色
            rgbSurface.Canvas.DrawImage(processingImage, x, y); //将头像置于底色顶部
            rgbSurface.Canvas.DrawBitmap(SKBitmap.Decode(Path.Combine(StaticData.ExePath!, $"Data/Img/PetPet/{index}.png")), 0, handoffsety); //绘制手
            rgbSurface.Flush();
            // rgbaSurface.Canvas.Clear();
            /*
            using var glRgbaSurface = SKManager.CreateSKGlSurface(rgbaImageInfo);
            glRgbaSurface.Canvas.Clear();
            glRgbaSurface.Canvas.DrawBitmap(avatarImage, new SKRect(0, 0, width, height)); //绘制头像
            glRgbaSurface.Flush();
            using var processingImage = glRgbaSurface.Snapshot();
            glRgbaSurface.Canvas.Clear(); //一个线程只能同时拥有一个OpenGL Context，下方创建的Surface依然来自于同一个Context。为避免重复创建Surface，这里不调用Dispose
            using var glRgbSurface = SKManager.CreateSKGlSurface(rgbImageInfo);
            //System.Diagnostics.Debug.Assert(glRgbaSurface == glRgbSurface);
            glRgbSurface.Canvas.DrawColor(SKColors.White); //绘制底色
            glRgbSurface.Canvas.DrawImage(processingImage, x, y); //将头像置于底色顶部
            glRgbSurface.Canvas.DrawBitmap(SKBitmap.Decode($"C:/SgBot/ImgResources/PetPet/{index}.png"), 0, handoffsety); //绘制手
            glRgbSurface.Flush();
            */
            return rgbSurface.Snapshot();
        }
        private static SKBitmap? GetAvater(string qqid)
        {
            var avaterDir = Path.Combine(StaticData.ExePath!, "Data/Img/Avatars");
            if (!Directory.Exists(avaterDir)) { Directory.CreateDirectory(avaterDir); }
            var avaterFile = Path.Combine(avaterDir, $"{qqid}.png");
            if (File.Exists(avaterFile))
            {
                /*
                var codec = SKCodec.Create(avaterFile);
                var rgba = BgraToRgbaLow(codec.Pixels, codec.Info.Width, codec.Info.Height);
                var data = SKBitmap.DecodeBounds(rgba);
                if (data.ColorType == SKColorType.Unknown)
                {
                    data = SKBitmap.DecodeBounds(BgraToRgbaHigh(codec.Pixels, codec.Info.Width, codec.Info.Height));
                }
                */
                File.Delete(avaterFile);
                // return SKBitmap.Decode(avaterFile);
            }
            var baseUrl = $"https://q.qlogo.cn/g?b=qq&nk={qqid}&s=640";
            try
            {
                using var response = Client.GetAsync(baseUrl).Result;
                using var stream = response.Content.ReadAsStream();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using var avaterReader = new BinaryReader(stream);
                    using var fileStream = File.OpenWrite(avaterFile);
                    fileStream.Write(avaterReader.ReadBytes((int)stream.Length));
                    fileStream.Flush();
                    fileStream.Close();
                    /*
                    var codec = SKCodec.Create(avaterFile);
                    var rgba = BgraToRgbaLow(codec.Pixels, codec.Info.Width, codec.Info.Height);
                    var data = SKBitmap.DecodeBounds(rgba);
                    if (data.ColorType == SKColorType.Unknown)
                    {
                        data = SKBitmap.DecodeBounds(BgraToRgbaHigh(codec.Pixels, codec.Info.Width, codec.Info.Height));
                    }
                    */
                    var image = SKBitmap.Decode(avaterFile);
                    return image;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"获取头像错误: {ex.Message}" + (ex.InnerException != null ? "\ncause by: " + ex.InnerException.Message : ""), LogLevel.Warning);
            }
            return null;
        }

        internal static void Convert(IEnumerable<string> images, int width, int height, string filepath, int delay)
        {
            using (var gif = new Image<Rgba32>(width, height))
            {
                var gifMetaData = gif.Metadata.GetGifMetadata();
                gifMetaData.RepeatCount = 0;
                var metadata = gif.Frames.RootFrame.Metadata.GetGifMetadata();
                metadata.FrameDelay = delay;
                for (int i = 0; i < images.Count(); i++)
                {
                    using (var image = Image.Load(images.ElementAt(i)))
                    {
                        // Set the duration of the image
                        metadata = image.Frames.RootFrame.Metadata.GetGifMetadata();
                        metadata.FrameDelay = delay;

                        // Add the image to the gif
                        gif.Frames.AddFrame(image.Frames.RootFrame);
                    }
                }

                // Removing gif's initial transparent frame before saving
                gif.Frames.RemoveFrame(0);
                // Save an encode the gif
                using (var fileStream = new FileStream(images.ElementAt(0).Replace("_0.png", ".gif"), FileMode.CreateNew))
                {
                    gif.SaveAsGif(fileStream);
                }
            }
        }
    }
}
