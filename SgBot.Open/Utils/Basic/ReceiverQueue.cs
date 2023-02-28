using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirai.Net.Data.Messages.Receivers;
using SgBot.Open.Responders;

namespace SgBot.Open.Utils.Basic
{
    internal static class ReceiverQueue
    {
        private const int GroupMessageReceiverQueueCapacity = 32;
        private static readonly Queue<GroupMessageReceiver> GroupMessageReceiverQueue = new(GroupMessageReceiverQueueCapacity);
        private static Thread _outReceiverThread = null!;

        public static bool AddGroupReceiver(GroupMessageReceiver groupMessageReceiver)
        {
            if (GroupMessageReceiverQueue.Count > 32)
            {
                Logger.Log("回复队列已满",LogLevel.Important);
                return false;
            }

            if (!RespondLimiter.CanRespond(groupMessageReceiver.GroupId, DateTime.Now))
            {
                return false;
            }
            GroupMessageReceiverQueue.Enqueue(groupMessageReceiver);
            return true;
        }

        public static void StartOutReceiver()
        {
            _outReceiverThread = new Thread(OutReceiver);
            _outReceiverThread.Start();
        }

        private static async void OutReceiver()
        {
            while (true)
            {
                if (GroupMessageReceiverQueue.TryDequeue(out var result))
                {
                    // 如果属于不可响应指令，不回答
                    var groupReceiverInfo = await MessagePreOperator.GetGroupReceiverInfo(result);
                    if (groupReceiverInfo.CanCommand)
                    {
                        var responded=await GroupMessageResponder.Respond(groupReceiverInfo, result);
                        // 如果回复了，更新回复限制器
                        if (responded)
                        {
                            RespondLimiter.AddRespond(result.GroupId, DateTime.Now);
                        }
                    }
                }
                Thread.Sleep(50);
            }
        }
        private static class RespondLimiter
        {
            private static readonly Dictionary<string, DateTime> GroupRespondDictionary = new ();
            public static void AddRespond(string groupId, DateTime timeNow)
            {
                GroupRespondDictionary[groupId] = timeNow;
            }
            public static bool CanRespond(string groupId, DateTime timeNow)
            {
                if (GroupRespondDictionary.ContainsKey(groupId))
                {
                    return timeNow - GroupRespondDictionary[groupId] > new TimeSpan(0, 0, 3);
                }
                GroupRespondDictionary.Add(groupId, timeNow);
                return true;
            }
        }
    }

    
}
