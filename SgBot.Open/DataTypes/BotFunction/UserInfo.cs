using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBot.Open.DataTypes.BotFunction
{
    public class UserInfo
    {
        [Key]
        public string UserId { get; set; }
        public string Nickname { get; set; }
        public long Token { get; set; }
        public Permission Permission { get; set; }
        public bool IsBanned { get; set; }
        public int FeedTime { get; set; }

        public UserInfo(string userId)
        {
            UserId = userId;
            Nickname = "";
            Token = 0;
            Permission = Permission.User;
            IsBanned = false;
        }
    }
    public enum Permission
    {
        User,
        Admin,
        SuperAdmin,
        Owner
    }
}
