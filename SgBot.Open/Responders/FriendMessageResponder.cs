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
            var ll= cc.Split(' ');
            switch (ll[0])
            {
                case "通过好友":
                    var id= ll[1];
                    if (FriendRequestOperator.TryHandleRequest(id, out var requested))
                    {
                        await RequestManager.HandleNewFriendRequestedAsync(requested, NewFriendRequestHandlers.Approve);
                        await receiver.QuoteMessageAsync($"{id} approve");
                        FriendRequestOperator.RemoveRequest(id);
                    }
                    else
                    {
                        await receiver.SendMessageAsync($"{id} did not exist");
                    }
                    break;
                case "改变群黑名单":
                    var gid = ll[1];
                    var g = await DataBaseOperator.FindGroup(gid);
                    g.IsBanned = !g.IsBanned;
                    if(g.IsBanned)
                    {
                        await receiver.QuoteMessageAsync($"{gid} 进入黑名单");
                    }
                    else
                    {
                        await receiver.QuoteMessageAsync($"{gid} 离开黑名单");
                    }
                    await DataBaseOperator.UpdateGroupInfo(g);
                    break;
                case "改变用户黑名单":
                    var uid = ll[1];
                    var u = await DataBaseOperator.FindUser(uid);
                    u.IsBanned = !u.IsBanned;
                    if (u.IsBanned)
                    {
                        await receiver.QuoteMessageAsync($"{uid} 进入黑名单");
                    }
                    else
                    {
                        await receiver.QuoteMessageAsync($"{uid} 离开黑名单");
                    }
                    await DataBaseOperator.UpdateUserInfo(u);
                    break;
                default:
                    break;
            }
            return;
        }
    }
}
