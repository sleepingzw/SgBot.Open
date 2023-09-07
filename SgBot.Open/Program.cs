using System.Reactive.Linq;
using Mirai.Net.Data.Events.Concretes.Group;
using Mirai.Net.Data.Events.Concretes.Message;
using Mirai.Net.Data.Events.Concretes.Request;
using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Data.Shared;
using Mirai.Net.Sessions.Http.Managers;
using SgBot.Open.DataTypes.BotFunction;
using SgBot.Open.DataTypes.StaticData;
using SgBot.Open.Responders;
using SgBot.Open.Utils.Basic;
using SlpzLibrary;

var exit = new ManualResetEvent(false);
var init = Initializer.Initial();
if (!init)
    return;
if (args.Length != 0)
{
    if (args[0] == "debug")
    {
        if (args[1].Trim() == "outplayer")
        {
            var db = new DataBaseContext();
            var players = db.Players.ToList();
            DataOperator.WriteJsonFile("player.json", players);
            Console.WriteLine("Out player data sucess");
        }
        else if (args[1] == "update")
        {
            //var db = new DataBaseContext();
            //var players = db.Players.ToList();
            //foreach (var p in players)
            //{
            //    var b = JsonConvert.DeserializeObject<List<EquipmentOld>>(p.BagString)!;
            //    var bnew = new List<Equipment>();
            //    foreach (var e in b)
            //    {
            //        if(e.OnBody)
            //        {
            //            // Console.WriteLine("body");
            //        }
            //        bnew.Add(new Equipment(e.Category, e.Name!, e.Description!, e.Level, e.OnBody)
            //        {
            //            EquipmentEffect = e.EquipmentEffect                        
            //        });
            //    }
            //    var bstring = JsonConvert.SerializeObject(bnew);
            //    p.BagString = bstring;
            //    db.Players.Update(p);
            //    await db.SaveChangesAsync();
            //}
            //Console.WriteLine("Update player data sucess");
            Console.WriteLine("Nothing to update");
        }
        Console.WriteLine("Debug finish. Press any key to continue");
        Console.ReadLine();
        Console.Clear();
    }
}

var friendDic = new Dictionary<string, NewFriendRequestedEvent>();

var bot = Initializer.InitBot();
Logger.Log($"初始化Bot {StaticData.BotConfig.BotQQ} 成功", LogLevel.Important);
var respond = new WebPicResponder();

await bot.LaunchAsync();
Initializer.StartQueueOut();

Logger.Log($"登录Bot {StaticData.BotConfig.BotQQ} 成功",LogLevel.Important);

// await MessageManager.SendFriendMessageAsync("2826241064", "bot已经登录");

bot.MessageReceived.OfType<GroupMessageReceiver>().Subscribe(receiver=>
{
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

exit.WaitOne();