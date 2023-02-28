using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Data.Messages;
using Mirai.Net.Sessions.Http.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manganese.Text;
using SgBot.Open.DataTypes.BotFunction;
using SgBot.Open.DataTypes.StaticData;

namespace SgBot.Open.Utils.Basic
{
    public static class MessagePreOperator
    {
        public static async Task<GroupMessageReceivedInfo> GetGroupReceiverInfo(GroupMessageReceiver g)
        {
            var atMe = false;
            var isOwner = false;
            var canCommand = g.MessageChain.GetPlainMessage() != "";
            if (g.MessageChain.OfType<AtAllMessage>().Any())
            {
                canCommand = false;
            }

            if (g.MessageChain.OfType<AtMessage>().Any(at => at.Target == StaticData.BotConfig.BotQQ!))
            {
                atMe = true;
                canCommand = true;
            }

            //是否owner
            if (g.Sender.Id == "2826241064")
            {
                isOwner = true;
            }

            var list = new List<string>();
            foreach (var pl in g.MessageChain.GetSeparatedPlainMessage())
            {
                foreach (var pll in pl.Trim().Split('\n'))
                {
                    foreach (var p in pll.Trim().Split(' '))
                    {
                        if (p != " " && !p.IsNullOrEmpty())
                        {
                            list.Add(p);
                        }
                    }
                }
            }

            var messages = new List<MessageBase>();
            foreach (var message in g.MessageChain)
            {
                messages.Add(message);
            }
            messages.RemoveAt(0);
            var chain = new MessageChain(messages);
            var groupReceiverInfo = new GroupMessageReceivedInfo(canCommand,
                await DataBaseOperator.FindGroup(g.GroupId),
                await DataBaseOperator.FindUser(g.Sender.Id),
                g.MessageChain.GetPlainMessage(),
                list.ToList(),
                g.MessageChain.OfType<AtMessage>().ToList(),
                g.MessageChain.OfType<ImageMessage>().ToList(),
                atMe,
                isOwner,
                chain);

            if (list.Count == 0)
            {
                groupReceiverInfo.AllPlainMessage = "";
            }
            if (g.Sender.Group.Name != groupReceiverInfo.Group.GroupName)
            {
                groupReceiverInfo.Group.GroupName = g.Sender.Group.Name;
                await DataBaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
            }
            if (g.Sender.Id == "80000000")
            {
                return groupReceiverInfo;
            }
            var profile = await AccountManager.GetMemberProfileAsync(g.Sender.Id, g.GroupId);
            bool badName = groupReceiverInfo.Member.Nickname.IsNullOrEmpty();
            if (profile != null)
            {
                var name = profile.NickName;
                if (!name.IsNullOrEmpty())
                {
                    badName = false;
                    if (groupReceiverInfo.Member.Nickname != name)
                    {
                        groupReceiverInfo.Member.Nickname = name;
                        await DataBaseOperator.UpdateUserInfo(groupReceiverInfo.Member);
                    }
                }
            }

            if (badName)
            {
                groupReceiverInfo.Member.Nickname = g.Sender.Name;
            }
            return groupReceiverInfo;
        }
    }
}
