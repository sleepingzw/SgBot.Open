using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Utils.Scaffolds;
using Newtonsoft.Json;
using SgBot.Open.DataTypes.BotFunction;
using SgBot.Open.DataTypes.StaticData;
using SgBot.Open.Utils.Basic;
using SlpzLibrary;

namespace SgBot.Open.Responders.Commands.GroupCommands
{
    internal static partial class BotGroupCommands
    {
        /// <summary>
        /// 根据关键词搜色图
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("搜色图", "/setufind")]
        public static async Task SetuSearch(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.Member.Token > 0)
            {
                groupMessageReceivedInfo.Member.Token--;
                await DataBaseOperator.UpdateUserInfo(groupMessageReceivedInfo.Member);
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "搜索中", true));

                var datago = new Datago();
                var size = new List<string>();
                var tag = new List<string>();
                size.Add("regular");
                var key = "";
                if (groupMessageReceivedInfo.PlainMessages.Count > 1)
                {
                    key = groupMessageReceivedInfo.PlainMessages[1];
                }
                datago.r18 = (int)groupMessageReceivedInfo.Group.SetuR18Status;
                datago.size = size;
                datago.keyword = key;
                datago.tag = tag;
                var data = JsonConvert.SerializeObject(datago);
                try
                {
                    string json;
                    try
                    {
                        json = HttpPoster.SendPost("https://api.lolicon.app/setu/v2", data);
                    }
                    catch
                    {
                        throw new Exception("链接setuAPI失败,无法得到返回数据");
                    }

                    var rb = JsonConvert.DeserializeObject<RootObject>(json)!;

                    if (rb.data.Count == 0)
                    {
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "无指定色图", true));
                        return;
                    }

                    var setu = new SetuInfo(rb);
                    var taglong = string.Join(",", setu.tags);
                    var urls = $"http://{StaticData.BotConfig.Ip!}:1145/pics?" + setu.imgname.Split('.')[0];
                    try
                    {
                        if (!File.Exists(setu.address))
                        {
                            var dler = new ImageDownloader(setu);
                            dler.DownloadPicture();
                        }
                        else if (new FileInfo(setu.address).Length == 0)
                        {
                            var dler = new ImageDownloader(setu);
                            dler.DownloadPicture();
                        }
                    }
                    catch
                    {
                        throw new Exception("下载图片失败,可能是节点流量耗尽,请联系管理员");
                    }

                    DataBaseOperator.UpSetuCount();
                    // await groupMessageReceiver.QuoteMessageAsync(
                    // $"标题:{setu.title}\n作者:{setu.author}\n标签:{taglong}\nPID:{setu.pid}\nURL:{urls}");
                    await groupMessageReceiver.QuoteMessageAsync(
                        $"标题:{setu.title}\n作者:{setu.author}\nPID:{setu.pid}\nURL:{urls}");
                }
                catch (Exception exception)
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, exception.Message, true));
                    // await groupMessageReceiver.QuoteMessageAsync(exception.Message);
                }
            }
            else
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "你的傻狗力不足哦", true));
            }
        }
        /// <summary>
        /// 根据tag搜色图
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand("搜色图tag", "/setutag")]
        public static async Task SetuSearchTag(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.Member.Token > 0)
            {
                groupMessageReceivedInfo.Member.Token--;
                await DataBaseOperator.UpdateUserInfo(groupMessageReceivedInfo.Member);
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "搜索中", true));

                var datago = new Datago();
                var size = new List<string>();
                var tag = new List<string>();
                size.Add("regular");
                var msg = "";
                if (groupMessageReceivedInfo.PlainMessages.Count > 1)
                {
                    msg = groupMessageReceivedInfo.PlainMessages[1];
                }
                tag.Add(msg);
                datago.r18 = (int)groupMessageReceivedInfo.Group.SetuR18Status;
                datago.size = size;
                datago.keyword = "";
                datago.tag = tag;
                var data = JsonConvert.SerializeObject(datago);
                try
                {
                    string json;
                    try
                    {
                        json = HttpPoster.SendPost("https://api.lolicon.app/setu/v2", data);
                    }
                    catch
                    {
                        throw new Exception("链接setuAPI失败,无法得到返回数据");
                    }
                    var rb = JsonConvert.DeserializeObject<RootObject>(json)!;

                    if (rb.data.Count == 0)
                    {
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "无指定色图", true));
                        //await groupMessageReceiver.QuoteMessageAsync("无指定色图");
                        return;
                    }

                    var setu = new SetuInfo(rb);
                    var taglong = string.Join(",", setu.tags);
                    var urls = $"http://{StaticData.BotConfig.Ip!}:1145/pics?" + setu.imgname.Split('.')[0];
                    try
                    {
                        if (!File.Exists(setu.address))
                        {
                            var dler = new ImageDownloader(setu);
                            dler.DownloadPicture();
                        }
                        else if (new FileInfo(setu.address).Length == 0)
                        {
                            var dler = new ImageDownloader(setu);
                            dler.DownloadPicture();
                        }
                    }
                    catch
                    {
                        throw new Exception("下载图片失败,可能是节点流量耗尽,请联系管理员");
                    }

                    DataBaseOperator.UpSetuCount();
                    // await groupMessageReceiver.QuoteMessageAsync(
                    // $"标题:{setu.title}\n作者:{setu.author}\n标签:{taglong}\nPID:{setu.pid}\nURL:{urls}");
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver,
                        $"标题:{setu.title}\n作者:{setu.author}\nPID:{setu.pid}\nURL:{urls}", true));
                }
                catch (Exception exception)
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, exception.Message, true));
                }
            }
            else
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "你的傻狗力不足哦", true));
            }
        }
        /// <summary>
        /// 随机招一张色图
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "色图时间", "涩图时间", "来点色图", "来点涩图" }, "/setu")]
        public static async Task Setu(GroupMessageReceivedInfo groupMessageReceivedInfo, GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.Member.Token > 0)
            {
                groupMessageReceivedInfo.Member.Token--;
                await DataBaseOperator.UpdateUserInfo(groupMessageReceivedInfo.Member);
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "搜索中", true));

                var datago = new Datago();
                var size = new List<string>();
                var tag = new List<string>();
                size.Add("regular");

                datago.r18 = (int)groupMessageReceivedInfo.Group.SetuR18Status;
                datago.size = size;
                datago.keyword = "";
                datago.tag = tag;
                var data = JsonConvert.SerializeObject(datago);
                try
                {
                    string json;
                    try
                    {
                        json = HttpPoster.SendPost("https://api.lolicon.app/setu/v2", data);
                    }
                    catch
                    {
                        throw new Exception("链接setuAPI失败,无法得到返回数据");
                    }
                    var rb = JsonConvert.DeserializeObject<RootObject>(json)!;

                    if (rb.data.Count == 0)
                    {
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "无指定色图", true));
                        return;
                    }

                    var setu = new SetuInfo(rb);
                    var taglong = string.Join(",", setu.tags);
                    var urls = $"http://{StaticData.BotConfig.Ip!}:1145/pics?" + setu.imgname.Split('.')[0];
                    try
                    {
                        if (!File.Exists(setu.address))
                        {
                            var dler = new ImageDownloader(setu);
                            dler.DownloadPicture();
                        }
                        else if (new FileInfo(setu.address).Length == 0)
                        {
                            var dler = new ImageDownloader(setu);
                            dler.DownloadPicture();
                        }
                    }
                    catch
                    {
                        throw new Exception("下载图片失败,可能是节点流量耗尽,请联系管理员");
                    }

                    DataBaseOperator.UpSetuCount();
                    // await groupMessageReceiver.QuoteMessageAsync(
                    // $"标题:{setu.title}\n作者:{setu.author}\n标签:{taglong}\nPID:{setu.pid}\nURL:{urls}");
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, $"标题:{setu.title}\n作者:{setu.author}\nPID:{setu.pid}\nURL:{urls}", true));
                }
                catch (Exception exception)
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, exception.Message, true));
                }
            }
            else
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMessageReceiver, "你的傻狗力不足哦", true));
            }
        }
    }
}
