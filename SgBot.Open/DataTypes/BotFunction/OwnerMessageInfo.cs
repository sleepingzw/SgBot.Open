using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBot.Open.DataTypes.BotFunction
{
    public class OwnerMessageInfo
    {
        public string What;
        public string Who;
        public string Name;
        public string GroupFrom;
        public string GroupName;
        public DateTime Time;
        public OwnerMessageInfo(string what, string who, string name, string groupFrom, string groupName, DateTime time)
        {
            What = what;
            Who = who;
            Name = name;
            GroupFrom = groupFrom;
            GroupName = groupName;
            Time = time;
        }
    }
}
