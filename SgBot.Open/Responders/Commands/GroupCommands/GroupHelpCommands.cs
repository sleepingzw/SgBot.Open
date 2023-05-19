using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
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
        /// 查看菜单
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "傻狗help", "傻狗menu" }, new string[] { "/help", "/menu" })]
        public static async Task Menu(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            var id = await FileManager.UploadImageAsync(Path.Combine(StaticData.ExePath!, "Data/Img/Menu.png"));
            var chain = new MessageChainBuilder().ImageFromId(id.Item1).Build();

            //await groupMessageReceiver.SendMessageAsync(chain);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, chain));
        }
        /// <summary>
        /// 查看群管菜单
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("群管help", "/managehelp")]
        public static async Task ManageHelp(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            var id = await FileManager.UploadImageAsync(Path.Combine(StaticData.ExePath!, "Data/Img/ManageHelp.png"));
            var chain = new MessageChainBuilder().ImageFromId(id.Item1).Build();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, chain));
        }
        /// <summary>
        /// 查看设置菜单
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("设置help", "/settinghelp")]
        public static async Task SettingHelp(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            var id = await FileManager.UploadImageAsync(Path.Combine(StaticData.ExePath!, "Data/Img/SettingHelp.png"));
            var chain = new MessageChainBuilder().ImageFromId(id.Item1).Build();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, chain));
        }
        /// <summary>
        /// 查看色图菜单
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("色图help", "/setuhelp")]
        public static async Task SetuHelp(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            var id = await FileManager.UploadImageAsync(Path.Combine(StaticData.ExePath!, "Data/Img/SetuHelp.png"));
            var chain = new MessageChainBuilder().ImageFromId(id.Item1).Build();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, chain));
        }
        /// <summary>
        /// 查看傻狗大陆菜单
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("傻狗大陆help", "/game.help")]
        public static async Task GameHelp(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            var id = await FileManager.UploadImageAsync(Path.Combine(StaticData.ExePath!, "Data/Img/GameHelp.png"));
            var chain = new MessageChainBuilder().ImageFromId(id.Item1).Build();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, chain));
        }
    }
}
