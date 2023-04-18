using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirai.Net.Data.Events.Concretes.Request;

namespace SgBot.Open.Utils.Basic
{
    internal static class FriendRequestOperator
    {
        private static Dictionary<string, NewFriendRequestedEvent> _newFriendRequested = new();

        public static bool AddRequest(string id, NewFriendRequestedEvent requested)
        {
            return _newFriendRequested.TryAdd(id, requested);
        }

        public static bool TryHandleRequest(string id,out NewFriendRequestedEvent? requested)
        {
            if (_newFriendRequested.ContainsKey(id))
            {
                requested = _newFriendRequested[id];
                return true;
            }
            requested = null;
            return false;
        }

        public static bool RemoveRequest(string id)
        {
            if (!_newFriendRequested.ContainsKey(id))
            {
                return false;
            }
            _newFriendRequested.Remove(id);
            return true;
        }
    }
}
