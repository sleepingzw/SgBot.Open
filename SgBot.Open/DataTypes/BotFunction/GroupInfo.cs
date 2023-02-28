using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBot.Open.DataTypes.BotFunction
{
    public class GroupInfo
    {
        [Key]
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public bool IsBanned { get; set; }
        public bool CanSetu { get; set; }
        public bool CanShortCommand { get; set; }
        public bool CanManage { get; set; }
        public bool CanGame { get; set; }
        public SetuR18Status SetuR18Status { get; set; }
        public bool CanYydz { get; set; }
        public RepeatStatus Cao { get; set; }
        public RepeatStatus QuestionMark { get; set; }
        public int RepeatFrequency { get; set; }

        public GroupInfo(string groupId)
        {
            GroupId = groupId;
            GroupName = "";
            IsBanned = false;
            CanSetu = false;
            CanManage = true;
            CanGame = false;
            CanShortCommand = true;
            SetuR18Status = SetuR18Status.Disabled;
            CanYydz = true;
            Cao = RepeatStatus.Idle;
            QuestionMark = RepeatStatus.Idle;
            RepeatFrequency = 200;
        }
    }

    public enum SetuR18Status
    {
        Disabled,
        OnlyR18,
        Enabled,
    }

    public enum RepeatStatus
    {
        Disabled,
        Idle,
        Done
    }
}
