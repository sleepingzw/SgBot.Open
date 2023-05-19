using Mirai.Net.Data.Messages.Receivers;
using SgBot.Open.DataTypes.BotFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Shared;
using Mirai.Net.Sessions.Http.Managers;
using Mirai.Net.Utils.Scaffolds;
using SgBot.Open.Utils.Basic;

namespace SgBot.Open.Responders
{
    public class FriendMessageResponder
    {
        public async void Respond(FriendMessageReceiver receiver)
        {
            if (receiver.FriendId != "2826241064")
            {
                return;
            }

            var cc = receiver.MessageChain.OfType<PlainMessage>().ToList()[0].Text;
            var id = cc.Split(' ')[1];
            var tf = cc.Split(" ")[2];
            switch (cc.Split(' ')[0])
            {
                case "通过好友":
                    var request =
                        FriendRequestOperator.TryHandleRequest(id, out var requested);
                    if (tf == "true")
                    {
                        await RequestManager.HandleNewFriendRequestedAsync(requested, NewFriendRequestHandlers.Approve);
                        await receiver.SendMessageAsync($"{id} approve");
                        FriendRequestOperator.RemoveRequest(id);
                    }
                    else
                    {
                        await RequestManager.HandleNewFriendRequestedAsync(requested, NewFriendRequestHandlers.Reject);
                        await receiver.SendMessageAsync($"{id} reject");
                        FriendRequestOperator.RemoveRequest(id);
                    }

                    break;
                default:
                    break;
            }
            return;
        }
    }
}
