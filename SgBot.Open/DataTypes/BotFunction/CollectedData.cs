using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBot.Open.DataTypes.BotFunction
{
    internal class CollectedData
    {
        [Key]
        public string Id { get; set; }
        public int SetuCount { get; set; }
        public int SvCount { get; set; }
        public int YydzCount { get; set; }
        public string UpdateInfo { get; set; }

        public CollectedData(string id)
        {
            Id = id;
            SetuCount = 0;
            SvCount = 0;
            YydzCount = 0;
            UpdateInfo = "";
        }
    }
}
