using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBot.Open.DataTypes.BotFunction
{
    public class GroupMessageReceivedInfo
    {
        public bool CanCommand;
        public GroupInfo Group;
        public UserInfo Member;
        public string AllPlainMessage;
        public List<string> PlainMessages;
        public List<AtMessage> AtMessages;
        public List<ImageMessage> ImageMessages;
        public bool AtMe;
        public bool IsOwner;
        public MessageChain PureMessageChain;
        public GroupMessageReceivedInfo(bool canCommand, GroupInfo group, UserInfo member, string allPlainMessage, List<string> plainMessages, List<AtMessage> atMessages, List<ImageMessage> imageMessages, bool atMe, bool isOwner, MessageChain pureMessageChain)
        {
            CanCommand = canCommand;
            Group = group;
            Member = member;
            AllPlainMessage = allPlainMessage;
            PlainMessages = plainMessages;
            AtMessages = atMessages;
            ImageMessages = imageMessages;
            AtMe = atMe;
            IsOwner = isOwner;
            PureMessageChain = pureMessageChain;
        }
    }
}
