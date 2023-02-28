using Mirai.Net.Data.Messages.Receivers;
using SgBot.Open.DataTypes.SgGame;
using SgBot.Open.Utils.Basic;
using SlpzLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mirai.Net.Utils.Scaffolds;
using Manganese.Text;
using SgBot.Open.DataTypes.BotFunction;

namespace SgBot.Open.Responders.Commands.GameCommands
{
    public static partial class BotGameCommands
    {
        /// <summary>
        /// 将背包中的某个装备装备到身上
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "装备物品" }, new string[] { "/game.equip", "/g.equip" }, true)]
        public static async Task Equip(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.PlainMessages.Count < 2)
            {
                await groupMessageReceiver.QuoteMessageAsync("参数错误");
                return;
            }
            var temp = Regex.Replace(groupMessageReceivedInfo.PlainMessages[1], @"[^0-9]+", "");
            if (temp.IsNullOrEmpty())
            {
                await groupMessageReceiver.QuoteMessageAsync("参数错误");
                return;
            }
            var what = int.Parse(temp);
            var player = await DataBaseOperator.FindPlayer(groupMessageReceivedInfo.Member.UserId);
            player.Refresh();
            if (player.Bag.Count < what)
            {
                await groupMessageReceiver.QuoteMessageAsync("装备不存在");
                return;
            }

            if (player.Bag[what - 1].OnBody)
            {
                await groupMessageReceiver.QuoteMessageAsync($"物品 {player.Bag[what - 1].Name} 已经处于装备状态");
                return;
            }

            var targetEquip = player.Bag[what - 1];
            var changed = false;
            switch (targetEquip.Category)
            {
                case EquipmentCategory.Weapon:
                    foreach (var item in player.Bag.Where(item => item.OnBody && item.Category == EquipmentCategory.Weapon))
                    {
                        item.OnBody = false;
                        player.Bag[what - 1].OnBody = true;
                        await groupMessageReceiver.QuoteMessageAsync($"已经替换武器 {targetEquip.Name}");
                        changed = true;
                        break;
                    }
                    if (!changed)
                    {
                        player.Bag[what - 1].OnBody = true;
                        await groupMessageReceiver.QuoteMessageAsync($"已经装备武器 {targetEquip.Name}");
                    }
                    break;
                case EquipmentCategory.Armor:
                    foreach (var item in player.Bag.Where(item => item.OnBody && item.Category == EquipmentCategory.Armor))
                    {
                        item.OnBody = false;
                        player.Bag[what - 1].OnBody = true;
                        await groupMessageReceiver.QuoteMessageAsync($"已经替换防具 {targetEquip.Name}");
                        changed = true;
                        break;
                    }
                    if (!changed)
                    {
                        player.Bag[what - 1].OnBody = true;
                        await groupMessageReceiver.QuoteMessageAsync($"已经装备防具 {targetEquip.Name}");
                    }
                    break;
                case EquipmentCategory.Jewelry:
                    foreach (var item in player.Bag.Where(item => item.OnBody && item.Category == EquipmentCategory.Jewelry))
                    {
                        item.OnBody = false;
                        player.Bag[what - 1].OnBody = true;
                        await groupMessageReceiver.QuoteMessageAsync($"已经替换饰品 {targetEquip.Name}");
                        changed = true;
                        break;
                    }
                    if (!changed)
                    {
                        player.Bag[what - 1].OnBody = true;
                        await groupMessageReceiver.QuoteMessageAsync($"已经装备饰品 {targetEquip.Name}");
                    }
                    break;
            }
            player.SortBag();
            await DataBaseOperator.UpdatePlayer(player);
        }
        /// <summary>
        /// 升级背包中的某个装备
        /// </summary>
        /// <param name="groupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "升级物品" }, new string[] { "/game.upe", "/g.upe" }, true)]
        public static async Task UpgradeEquip(GroupMessageReceivedInfo groupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (groupMessageReceivedInfo.PlainMessages.Count != 2)
            {
                await groupMessageReceiver.QuoteMessageAsync("参数错误");
                return;
            }
            var temp = Regex.Replace(groupMessageReceivedInfo.PlainMessages[1], @"[^0-9]+", "");
            if (temp.IsNullOrEmpty())
            {
                await groupMessageReceiver.QuoteMessageAsync("参数错误");
                return;
            }

            var what = int.Parse(temp);
            var player = await DataBaseOperator.FindPlayer(groupMessageReceivedInfo.Member.UserId);
            player.Refresh();
            if (player.Bag.Count < what)
            {
                await groupMessageReceiver.QuoteMessageAsync("装备不存在");
                return;
            }
            var needCoin = 10 * (9 * player.Bag[what - 1].Level + player.Bag[what - 1].Level * player.Bag[what - 1].Level);
            needCoin *= player.Bag[what - 1].Level;
            if (player.Coin < needCoin)
            {
                await groupMessageReceiver.QuoteMessageAsync($"金币不足,需要 {needCoin} 金币");
                return;
            }
            var success = player.Bag[what - 1].UpgradeEquipment();
            if (!success)
            {
                await groupMessageReceiver.QuoteMessageAsync($"装备 {player.Bag[what - 1].Name} 已经升至最高等级");
                return;
            }
            player.Coin -= needCoin;
            player.SortBag();
            await DataBaseOperator.UpdatePlayer(player);
            await groupMessageReceiver.QuoteMessageAsync($"装备 {player.Bag[what - 1].Name} 升级成功 当前Rk{player.Bag[what - 1].Level}");
        }
        /// <summary>
        /// 丢弃背包中的某个装备
        /// </summary>
        /// <param name="GroupMessageReceivedInfo"></param>
        /// <param name="groupMessageReceiver"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "丢弃物品" }, new string[] { "/game.throw", "/g.throw" }, true)]
        public static async Task ThrowEquip(GroupMessageReceivedInfo GroupMessageReceivedInfo,
            GroupMessageReceiver groupMessageReceiver)
        {
            if (GroupMessageReceivedInfo.PlainMessages.Count != 2)
            {
                await groupMessageReceiver.QuoteMessageAsync("参数错误");
                return;
            }
            var player = await DataBaseOperator.FindPlayer(GroupMessageReceivedInfo.Member.UserId);
            player.Refresh();
            if (GroupMessageReceivedInfo.PlainMessages[1] == "all")
            {
                var tempList = player.Bag.Where(i => i.OnBody).ToList();
                player.Bag.Clear();
                player.Bag = tempList;
                await DataBaseOperator.UpdatePlayer(player);
                await groupMessageReceiver.QuoteMessageAsync("已丢弃所有非装备物品");
                return;
            }
            var temp = Regex.Replace(GroupMessageReceivedInfo.PlainMessages[1], @"[^0-9]+", "");
            if (temp.IsNullOrEmpty())
            {
                await groupMessageReceiver.QuoteMessageAsync("参数错误");
                return;
            }
            var what = int.Parse(temp);

            if (player.Bag.Count < what)
            {
                await groupMessageReceiver.QuoteMessageAsync("装备不存在");
                return;
            }
            player.Bag.RemoveAt(what - 1);
            player.SortBag();
            await DataBaseOperator.UpdatePlayer(player);
            await groupMessageReceiver.QuoteMessageAsync("丢弃装备成功");
        }
    }
}
