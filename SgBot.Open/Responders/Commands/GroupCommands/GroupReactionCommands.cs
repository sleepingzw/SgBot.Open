using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Data.Shared;
using Mirai.Net.Sessions.Http.Managers;
using Mirai.Net.Utils.Scaffolds;
using SgBot.Open.DataTypes.BotFunction;
using SgBot.Open.DataTypes.StaticData;
using SgBot.Open.Utils.Basic;
using SlpzLibrary;

namespace SgBot.Open.Responders.Commands.GroupCommands
{
    internal static partial class BotGroupCommands
    {
        /// <summary>
        /// 询问傻狗在不在的交互
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("傻狗在吗", "/whereareyou")]
        public static async Task WhereAreTheBot(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "buzai,cnm"));
        }
        /// <summary>
        /// 和傻狗说晚安吧
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("傻狗晚安", "/sgsleep")]
        public static async Task GoodNightBot(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (DateTime.Now.Hour > 7 && DateTime.Now.Hour < 22)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"{groupMessageReceivedInfo.Member.Nickname},晚安锤子,起来嗨"));
                // await groupMessageReceiver.SendMessageAsync($"{groupMessageReceivedInfo.Member.Nickname},晚安锤子,起来嗨");
            }
            else
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"{groupMessageReceivedInfo.Member.Nickname},晚安"));
            }
        }
        /// <summary>
        /// 让傻狗爬，但是傻狗不一定听你的，听了也要付出代价
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("傻狗爬", "/sgcreep")]
        public static async Task TryCreep(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            var rd = new Random();
            if (UsefulMethods.IsOk(5) && groupMessageReceivedInfo.Member.Token > 10)
            {
                var temp = groupMessageReceivedInfo.Member;
                temp.Token -= 5;
                await DataBaseOperator.UpdateUserInfo(temp);
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "我爬，我爬",true));
                // await groupMessageReceiver.QuoteMessageAsync("我爬，我爬");
            }
            else
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "我不爬，你爬", true));
                // await groupMessageReceiver.QuoteMessageAsync("我不爬，你爬");
            }
        }
        /// <summary>
        /// 我超！郭楠
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "透傻狗", "透透傻狗" }, "/fucksg")]
        public static async Task TryFuck(GroupMessageReceivedInfo groupMessageReceivedInfo, GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.Member.Token > 1)
            {
                var temp = groupMessageReceivedInfo.Member;
                temp.Token--;
                await DataBaseOperator.UpdateUserInfo(temp);
            }
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "gnk48"));
            // await groupMessageReceiver.SendMessageAsync("gnk48");
        }
        /// <summary>
        /// 贴贴！
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "贴贴傻狗", "傻狗贴贴" }, "/ttsg")]
        public static async Task TryLove(GroupMessageReceivedInfo groupMessageReceivedInfo, GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.Member.Token > 3000 || groupMessageReceivedInfo.Member.Permission > Permission.User)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "贴贴~", true));
            }
            else
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "你寄吧谁", true));
            }
        }
        /// <summary>
        /// 让傻狗当传话筒
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("传话作者", "/talktoowner")]
        public static async Task TalkToOwner(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.PlainMessages.Count < 2)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "没有找到传话内容", true));
                return;
            }
            var msg = new OwnerMessageInfo()
            {
                What = groupMessageReceivedInfo.PlainMessages[1],
                Who = groupMessageReceivedInfo.Member.UserId,
                Name = groupMessageReceivedInfo.Member.Nickname,
                GroupFrom = groupMessageReceivedInfo.Group.GroupId,
                GroupName = groupMessageReceivedInfo.Group.GroupName,
                Time = DateTime.Now
            };
            var json = DataOperator.ToJsonString(msg, true);
            await MessageManager.SendFriendMessageAsync("2826241064", json);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "已传话(有建设性意见可以直接加用户反馈群442069136)", true));
            var commentAddress = Path.Combine(StaticData.ExePath!, $"Data/Comments/{DateTime.Now:yyyy-M-dd--HH-mm-ss}.json");
            DataOperator.WriteJsonFile(commentAddress, msg);
            Logger.Log(
                $"{msg.Who}({msg.Name})在{msg.GroupFrom}({msg.GroupName})发送了一条评论",
                LogLevel.Simple);
        }
        /// <summary>
        /// 算命，，，赛博封建迷信
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("今日霉运", "/jrmy")]
        public static async Task TodayUnlucky(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            long date = (DateTime.Now.Year) * 114 + (DateTime.Now.Month) * 514 + DateTime.Now.DayOfYear;
            var tempmy = long.Parse(groupMessageReceivedInfo.Member.UserId);
            tempmy = tempmy % date;
            tempmy = tempmy % ((400 - DateTime.Now.DayOfYear) * DateTime.Now.Month);
            var jrmy = tempmy % 101;
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver,
                $"{groupMessageReceivedInfo.Member.Nickname},你的今日霉运值为{jrmy}"));
            // await groupMessageReceiver.SendMessageAsync($"{groupMessageReceivedInfo.Member.Nickname},你的今日霉运值为{jrmy}");
        }
        /// <summary>
        /// 摸摸傻狗
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "摸摸傻狗", "傻狗摸摸" }, "/touchsg")]
        public static async Task TouchDog(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.Member.Token > 3000 || groupMessageReceivedInfo.Member.Permission > Permission.User)
            {
                if (groupMessageReceivedInfo.Member.Permission > Permission.Admin)
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "蹭蹭", true));
                    return;
                }
                if (UsefulMethods.IsOk(3))
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "蹭蹭", true));
                    return;
                }
            }
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "小手不是很干净", true));
        }
    }
}
