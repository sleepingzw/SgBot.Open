using Mirai.Net.Data.Messages.Receivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirai.Net.Data.Messages;
using Mirai.Net.Sessions.Http.Managers;
using Mirai.Net.Utils.Scaffolds;
using SgBot.Open.Responders;

namespace SgBot.Open.Utils.Basic
{
    public class GroupRespondInfo
    {
        public GroupMessageReceiver Receiver;
        public MessageChain Chain;
        public bool IsQuote;
        public GroupRespondInfo(GroupMessageReceiver receiver, MessageChain chain, bool isQuote=false)
        {
            Receiver = receiver;
            Chain = chain;
            this.IsQuote = isQuote;
        }
        public GroupRespondInfo(GroupMessageReceiver receiver, string str, bool isQuote = false)
        {
            Receiver = receiver;
            Chain = new MessageChainBuilder().Plain(str).Build();
            this.IsQuote = isQuote;
        }
    }
    internal static class RespondQueue
    {
        private const int GroupMessageRespondQueueCapacity = 64;
        private static readonly Queue<GroupRespondInfo> GroupMessageRespondQueue = new(GroupMessageRespondQueueCapacity);
        private static Thread _outRespondThread = null!;
        public static bool AddGroupRespond(GroupRespondInfo groupMessageRespond)
        {
            if (GroupMessageRespondQueue.Count >= 64)
            {
                Logger.Log("回复队列已满", LogLevel.Important);
                GroupMessageRespondQueue.Clear();
                MessageManager.SendFriendMessageAsync("2826241064", "回复队列已清空");
                return false;
            }
            GroupMessageRespondQueue.Enqueue(groupMessageRespond);
            RespondLimiter.AddRespond(groupMessageRespond.Receiver.GroupId, DateTime.Now);
            return true;
        }
        public static void StartOutRespond()
        {
            _outRespondThread = new Thread(OutRespond);
            _outRespondThread.Start();
        }
        private static async void OutRespond()
        {
            while (true)
            {
                try
                {
                    if (GroupMessageRespondQueue.TryDequeue(out var result))
                    {
                        //_ = Task.Run(async () =>
                        //{
                        //    if (result.IsQuote)
                        //    {
                        //        await result.Receiver.QuoteMessageAsync(result.Chain);
                        //    }
                        //    else
                        //    {
                        //        await result.Receiver.SendMessageAsync(result.Chain);
                        //    }
                        //    Logger.Log($"队列剩余消息{GroupMessageRespondQueue.Count}", LogLevel.Simple);
                        //});
                        _ = result.IsQuote ? result.Receiver.QuoteMessageAsync(result.Chain) : result.Receiver.SendMessageAsync(result.Chain);
                        Logger.Log($"队列剩余消息{GroupMessageRespondQueue.Count}", LogLevel.Simple);
                    }
                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, LogLevel.Fatal);
                }
            }
        }
    }
}
