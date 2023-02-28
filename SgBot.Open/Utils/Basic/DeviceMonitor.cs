using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SgBot.Open.Utils.Basic
{
    public class DeviceMonitor
    {
        /// <summary>
        ///     % Processor Time 是所有进程线程使用处理器执行指令所花的时间百分比。指令是计算机执行的基础单位。线程是执行指令的对象，进程是程序运行时创建的对象。此计数包括处理某些硬件间隔和陷阱条件所执行的代码。
        /// </summary>
        private static readonly PerformanceCounter CpuCounter =
            new PerformanceCounter("Processor", "% Processor Time", "_Total");

        /// <summary>
        ///     Available MBytes 指能立刻分配给一个进程或系统使用的物理内存数量，以 MB 为单位表示。它等于分配给待机(缓存的)、空闲和零分页列表内存的总和。
        /// </summary>
        private static readonly PerformanceCounter RamCounter = new PerformanceCounter("Memory", "Available MBytes");
        
        /// <summary>
        ///     System Up Time 指计算机自上次启动后已经运行的时间(以秒计)。此计数器显示启动时间和当前时间之差。
        /// </summary>
        private static readonly PerformanceCounter Uptime = new PerformanceCounter("System", "System Up Time");
        
        /// <summary>
        ///     开机时间
        /// </summary>
        /// <returns></returns>
        public static TimeSpan GetSystemUpTime()
        {
            Uptime.NextValue();
            var ts = TimeSpan.FromSeconds(Uptime.NextValue());
            return ts;
        }

        /// <summary>
        ///     CPU
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentCpuUsage()
        {
            return CpuCounter.NextValue() + "%";
        }

        /// <summary>
        ///     内存
        /// </summary>
        /// <returns></returns>
        public static string GetAvailableRam()
        {
            return RamCounter.NextValue() + "MB";
        }


        /// <summary>
        ///     线程数
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static int GetThreadCount(string processName)
        {
            var ps = Process.GetProcessesByName(processName).FirstOrDefault();
            if (ps != null)
            {
                return ps.Threads.Count;
            }

            return -1;
        }

        /// <summary>
        ///     程序占用内存
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static string MemoryUsingByApp(string processName)
        {
            var ps = Process.GetProcessesByName(processName).FirstOrDefault();
            if (ps != null)
            {
                return GetDisplayByteSize(ps.WorkingSet64, 3);
            }

            return "--";
        }


        /// <summary>
        ///     所有网络状态
        /// </summary>
        /// <returns></returns>
        public static string GetAllNetworkStatus()
        {
            var allNetStatus = "";
            var allNet = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var i in allNet)
            {
                allNetStatus += " name=" + i.Name + ",type=" + i.NetworkInterfaceType + ",status=" +
                                i.OperationalStatus + "\r\n";
            }

            return allNetStatus;
        }

        /// <summary>
        ///     某程序cpu占用
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static object GetCpuUsageByProcessName(string processName)
        {
            var ct = new PerformanceCounter("Process", "% Processor Time", processName);
            var cpu = ct.NextValue();
            ct.Dispose();
            return cpu;
        }


        /// <summary>
        ///     磁盘控件
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static string GetHddSpace(string processName)
        {
            var ps = Process.GetProcessesByName(processName).FirstOrDefault();
            if (ps == null)
            {
                return "-";
            }

            //var fn = ps.MainModule.FileName;
            var str = "-";
            foreach (var drive in DriveInfo.GetDrives())
            {
                //判断是否是固定磁盘 
                //if (drive.Name == Path.GetPathRoot(fn))
                //{
                str +=
                    $"{drive.Name}可用{GetDisplayByteSize(drive.AvailableFreeSpace)}（总{GetDisplayByteSize(drive.TotalSize)}）\r\n";
                //}
            }

            return str;
        }

        /// <summary>
        ///     level=0==G
        ///     level=1==MB
        ///     level=2==Kb
        ///     level=3==B
        /// </summary>
        /// <param name="byteSize"></param>
        /// <param name="stopLevel"></param>
        /// <returns></returns>
        private static string GetDisplayByteSize(long byteSize, int stopLevel = 1)
        {
            if (byteSize >= 1024 * 1024 * 1024)
            {
                var remain = byteSize % (1024 * 1024 * 1024);
                return byteSize / (1024 * 1024 * 1024) + "G " + (stopLevel >= 1 ? GetDisplayByteSize(remain) : "");
            }

            if (byteSize >= 1024 * 1024)
            {
                var remain = byteSize % (1024 * 1024);
                return byteSize / (1024 * 1024) + "M " + (stopLevel >= 2 ? GetDisplayByteSize(remain) : "");
            }

            if (byteSize >= 1024)
            {
                var remain = byteSize % 1024;
                return byteSize / 1024 + "K " + (stopLevel >= 3 ? GetDisplayByteSize(remain) : "");
            }

            if (stopLevel > 3)
            {
                return byteSize + "B ";
            }

            return "";
        }
    }
}
