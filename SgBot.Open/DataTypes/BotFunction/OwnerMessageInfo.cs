using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBot.Open.DataTypes.BotFunction
{
    public class OwnerMessageInfo
    {
        public string? What { get; set; }
        public string? Who { get; set; }
        public string? Name { get; set; }
        public string? GroupFrom { get; set; }
        public string? GroupName { get; set; }
        public DateTime Time { get; set; }
    }
}
