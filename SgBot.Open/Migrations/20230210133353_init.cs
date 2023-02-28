using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SgBot.Open.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CollectedDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    SetuCount = table.Column<int>(type: "INTEGER", nullable: false),
                    SvCount = table.Column<int>(type: "INTEGER", nullable: false),
                    YydzCount = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateInfo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectedDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<string>(type: "TEXT", nullable: false),
                    GroupName = table.Column<string>(type: "TEXT", nullable: false),
                    IsBanned = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanSetu = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanShortCommand = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanManage = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanGame = table.Column<bool>(type: "INTEGER", nullable: false),
                    SetuR18Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CanYydz = table.Column<bool>(type: "INTEGER", nullable: false),
                    Cao = table.Column<int>(type: "INTEGER", nullable: false),
                    QuestionMark = table.Column<int>(type: "INTEGER", nullable: false),
                    RepeatFrequency = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    TitleString = table.Column<string>(type: "TEXT", nullable: false),
                    Coin = table.Column<long>(type: "INTEGER", nullable: false),
                    Power = table.Column<int>(type: "INTEGER", nullable: false),
                    PowerDay = table.Column<int>(type: "INTEGER", nullable: false),
                    Level = table.Column<long>(type: "INTEGER", nullable: false),
                    Exp = table.Column<long>(type: "INTEGER", nullable: false),
                    Rank = table.Column<int>(type: "INTEGER", nullable: false),
                    RankScore = table.Column<int>(type: "INTEGER", nullable: false),
                    IsHitBoss = table.Column<bool>(type: "INTEGER", nullable: false),
                    BuyPowerTime = table.Column<int>(type: "INTEGER", nullable: false),
                    Strength = table.Column<long>(type: "INTEGER", nullable: false),
                    Intelligence = table.Column<long>(type: "INTEGER", nullable: false),
                    Agility = table.Column<long>(type: "INTEGER", nullable: false),
                    Fitness = table.Column<long>(type: "INTEGER", nullable: false),
                    FreePoints = table.Column<long>(type: "INTEGER", nullable: false),
                    BagString = table.Column<string>(type: "TEXT", nullable: false),
                    SkillHaveString = table.Column<string>(type: "TEXT", nullable: false),
                    SkillActiveString = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Nickname = table.Column<string>(type: "TEXT", nullable: false),
                    Token = table.Column<long>(type: "INTEGER", nullable: false),
                    Permission = table.Column<int>(type: "INTEGER", nullable: false),
                    IsBanned = table.Column<bool>(type: "INTEGER", nullable: false),
                    FeedTime = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollectedDatas");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
