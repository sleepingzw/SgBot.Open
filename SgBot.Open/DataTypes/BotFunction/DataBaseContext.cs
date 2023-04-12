using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mirai.Net.Data.Shared;
using SgBot.Open.DataTypes.SgGame;

namespace SgBot.Open.DataTypes.BotFunction
{
    internal class DataBaseContext:DbContext
    {
        public DbSet<UserInfo> Users { get; set; }
        public DbSet<GroupInfo> Groups { get; set; }
        public DbSet<CollectedData> CollectedDatas { get; set; }
        public DbSet<Player> Players { get; set; }

        public string DbPath { get; }

        public DataBaseContext()
        {
            DbPath = Path.Combine(StaticData.StaticData.ExePath!, "Data/data.db");
            // DbPath = "D:\\vspjt\\SgBot.Open\\SgBot.Open\\bin\\Debug\\net6.0\\Data\\data.db";
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
