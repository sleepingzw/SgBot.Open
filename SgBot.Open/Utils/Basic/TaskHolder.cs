using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBot.Open.Utils.Basic
{
    /// <summary>
    /// 用来处理一些需要稍后完成的任务
    /// </summary>
    public static class TaskHolder
    {
        /// <summary>
        /// 延时，删除某个文件
        /// </summary>
        /// <param name="path">文件的路径</param>
        /// <param name="delay">延时的时间，默认500毫秒</param>
        public static void DeleteTask(string path,int delay=5000)
        {
            Task.Run(() =>
            {
                Thread.Sleep(delay);
                if (File.Exists(path))
                {
                    File.Delete(path);
                    Logger.Log($"删除 {path} 成功", LogLevel.Simple);
                }
                else
                {
                    Logger.Log($"删除 {path} 失败,目标文件不存在", LogLevel.Error);
                }
            });
        }
    }
}
