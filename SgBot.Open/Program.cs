using System.Numerics;
using System.Reactive.Linq;
using System.Reflection.Emit;
using Manganese.Array;
using Mirai.Net.Data.Events.Concretes.Group;
using Mirai.Net.Data.Events.Concretes.Message;
using Mirai.Net.Data.Events.Concretes.Request;
using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Data.Shared;
using Mirai.Net.Sessions;
using Mirai.Net.Sessions.Http.Managers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SgBot.Open.DataTypes.BotFunction;
using SgBot.Open.DataTypes.SgGame;
using SgBot.Open.DataTypes.StaticData;
using SgBot.Open.Responders;
using SgBot.Open.Utils.Basic;
using Spectre.Console;

var friendDic = new Dictionary<string, NewFriendRequestedEvent>();
var exit = new ManualResetEvent(false);
var init=Initializer.Initial();
if(!init)
    return;
var bot = Initializer.InitBot();
Logger.Log($"初始化Bot {StaticData.BotConfig.BotQQ} 成功", LogLevel.Important);
var respond = new WebPicResponder();

await bot.LaunchAsync();
Initializer.StartQueueOut();

Logger.Log($"登录Bot {StaticData.BotConfig.BotQQ} 成功",LogLevel.Important);
// await MessageManager.SendFriendMessageAsync("2826241064", "test");
bot.MessageReceived.OfType<GroupMessageReceiver>().Subscribe(receiver=>
{
    //var temp = new GroupInfo("temp");
    //var tempm = new UserInfo("temp");
    ////如果在黑名单内，不回复消息
    //Task.Run(async () =>
    //{
    //    temp = await DataBaseOperator.FindGroup(receiver.GroupId);
    //    tempm = await DataBaseOperator.FindUser(receiver.Sender.Id);
    //});
    //if (temp.IsBanned || tempm.IsBanned)
    //    return;
    //ReceiverQueue.AddGroupReceiver(receiver);
    Task.Run(async () =>
    {
        var info = await MessagePreOperator.GetGroupReceiverInfo(receiver);
        if (!RespondLimiter.CanRespond(info.Group.GroupId, DateTime.Now))
        {
            return;
        }
        if (info.Member.IsBanned || info.Group.IsBanned)
        {
            return;
        }
        await GroupMessageResponder.Respond(info, receiver);
    });

});
bot.EventReceived.OfType<NewInvitationRequestedEvent>().Subscribe(x =>
{
    var temp = new GroupInfo("temp");
    var tempm = new UserInfo("temp");
    //如果在黑名单内，不接受加群邀请
    Task.Run(async () =>
    {
        temp = await DataBaseOperator.FindGroup(x.GroupId);
        tempm = await DataBaseOperator.FindUser(x.FromId);
    });
    if(temp.IsBanned||tempm.IsBanned)
        return;
    RequestManager.HandleNewInvitationRequestedAsync(x, NewInvitationRequestHandlers.Approve, "");
});
bot.EventReceived.OfType<MemberLeftEvent>().Subscribe(x =>
{
    MessageManager.SendGroupMessageAsync(x.Member.Group.Id, $"{x.Member.Name} 离开了我们");
});
bot.EventReceived.OfType<MemberKickedEvent>().Subscribe(x =>
{
    MessageManager.SendFriendMessageAsync(x.Member.Group.Id, $"{x.Member.Name} 喜提飞机票");
});
bot.EventReceived.OfType<JoinedEvent>().Subscribe(x =>
{
    MessageManager.SendGroupMessageAsync(x.Group.Id, "输入 傻狗menu 查看菜单");
});
bot.EventReceived.OfType<KickedEvent>().Subscribe(x =>
{
    MessageManager.SendFriendMessageAsync("2826241064", $"{x.Group.Name} {x.Group.Id}被踢出");
});
bot.EventReceived.OfType<MutedEvent>().Subscribe(x =>
{
    MessageManager.SendFriendMessageAsync("2826241064", $"被{x.Operator.Name} {x.Operator.Id}禁言于{x.Operator.Group.Name} {x.Operator.Group.Id} 时长{x.Period}");
});
bot.EventReceived.OfType<NudgeEvent>().Subscribe(x =>
{
    if (x.Target != "939126365" || x.FromId == "939126365") return;
    if (UsefulMethods.IsOk(3,1))
    {
        MessageManager.SendNudgeAsync(x.FromId, x.Subject.Id, MessageReceivers.Group);
    }
});
bot.EventReceived.OfType<MemberJoinedEvent>().Subscribe(x =>
{
    MessageManager.SendGroupMessageAsync(x.Member.Group.Id, $"欢迎 {x.Member.Name} 加入本群");
});
bot.EventReceived.OfType<NewFriendRequestedEvent>().Subscribe(x =>
{
    MessageManager.SendFriendMessageAsync("2826241064", $"{x.FromId} 加好友");
    FriendRequestOperator.AddRequest(x.FromId, x);
});
bot.MessageReceived.OfType<FriendMessageReceiver>().Subscribe(x =>
{
    var responder = new FriendMessageResponder();
    responder.Respond(x);
});

async Task Connect()
{
    while (true)
    {
        try
        {
            Logger.Log("尝试建立连接...", LogLevel.Important);
            await bot.LaunchAsync();
            Logger.Log($"登录Bot {StaticData.BotConfig.BotQQ} 成功", LogLevel.Important);
            break;
        }
        catch (Exception ex)
        {
            Logger.Log(ex.Message, LogLevel.Error);
        }
    }
}

bot.DisconnectionHappened.Subscribe(async x =>
{
    Logger.Log($"失去连接:{x}", LogLevel.Error);
    await Connect();
});

#region Update
//var ee = Console.ReadLine();
//if (ee == "read")
//{
//    var array = SlpzLibrary.DataOperator.GetJsonFile<JArray>("player.json");
//    var eqarry = SlpzLibrary.DataOperator.GetJsonFile<JArray>("eq.json");
//    var list = new List<Player>();
//    foreach (var a in array)
//    {
//        var player = new Player(a["Id"]!.ToString())
//        {
//            Name = a["Name"]!.ToString(),
//            Coin = long.Parse(a["Coin"]!.ToString()),
//            Power= int.Parse(a["Power"]!.ToString()),
//            PowerDay = int.Parse(a["PowerDay"]!.ToString()),
//            Level = int.Parse(a["Level"]!.ToString()),
//            Exp = int.Parse(a["Exp"]!.ToString()),
//            Rank = (Rank)int.Parse(a["Rank"]!.ToString()),
//            RankScore = int.Parse(a["RankScore"]!.ToString()),
//            BuyPowerTime = int.Parse(a["BuyPowerTime"]!.ToString()),
//            Strength = int.Parse(a["Strength"]!.ToString()),
//            Intelligence = int.Parse(a["Intelligence"]!.ToString()),
//            Agility = int.Parse(a["Agility"]!.ToString()),
//            Fitness = int.Parse(a["Fitness"]!.ToString()),
//            FreePoints = int.Parse(a["FreePoints"]!.ToString()),
//        };
//        foreach (var eq in eqarry)
//        {
//            if (eq["PlayerId"]!.ToString() == player.Id)
//            {
//                var q = new Equipment((EquipmentCategory)int.Parse(eq["Category"]!.ToString()), eq["Name"]!.ToString(),
//                    eq["Description"]!.ToString(),
//                    int.Parse(eq["Level"]!.ToString()), bool.Parse(eq["OnBody"]!.ToString()))
//                {
//                    EquipmentEffect = JsonConvert.DeserializeObject<EquipmentEffect>(eq["EquipmentEffect"]!.ToString())!
//                };
//                player.Bag.Add(q);
//            }
//        }
//        list.Add(player);
//        Console.WriteLine(SlpzLibrary.DataOperator.ToJsonString(player));
//    }

//    foreach (var p in list)
//    {
//        p.MakeString();
//        var db = new DataBaseContext();
//        db.Players.Add(p);
//        await db.SaveChangesAsync();
//    }
//}
#endregion
exit.WaitOne();