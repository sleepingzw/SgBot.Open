using SgBot.Open.DataTypes.BotFunction;
using SlpzLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Mirai.Net.Sessions.Http.Managers;

namespace SgBot.Open.Utils.Basic
{
    public class ImageDownloader
    {
        private string picUrl, savePath;
        int timeOut = 30000;
        private SetuInfo st;
        public ImageDownloader(SetuInfo st)
        {
            picUrl = st.urls;
            savePath = st.address;
            this.st = st;
        }
        public void DownloadPicture()
        {
            Stream stream = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(picUrl);
                if (timeOut != -1) request.Timeout = timeOut;
                var response = request.GetResponse();
                if (!response.ContentType.ToLower().StartsWith("text/"))
                    SaveBinaryFile(response, savePath, st.dicaddress);
                // 生成信息文件
                if (File.Exists(st.jsonaddress)) File.Delete(st.jsonaddress);
                DataOperator.WriteJsonFile(st.jsonaddress, DataOperator.ToJsonString(st));
                Logger.Log($"色图 {st.address} 下载完成", LogLevel.Important);
            }
            catch (WebException ex)
            {
                Logger.Log(ex.Message,LogLevel.Error);
                throw ex;
            }
            finally
            {
                if (stream != null) stream.Close();

            }
        }
        private static bool SaveBinaryFile(WebResponse response, string savePath, string dicadress)
        {
            bool value = false;
            byte[] buffer = new byte[1024];
            Stream outStream = null;
            Stream inStream = null;
            try
            {
                if (Directory.Exists(dicadress) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(dicadress);
                }
                if (File.Exists(savePath)) File.Delete(savePath);
                outStream = File.Create(savePath);
                inStream = response.GetResponseStream();
                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0) outStream.Write(buffer, 0, l);
                } while (l > 0);
                value = true;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString(), LogLevel.Error);
            }
            finally
            {
                if (outStream != null) outStream.Close();
                if (inStream != null) inStream.Close();
            }
            return value;
        }
    }
}
