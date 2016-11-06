using System.Collections.Generic;

namespace ZChat.Shared
{
    public class ConnectionData
    {
        public IEnumerable<string> UsersInChat { get; set; }
        public string ServerName { get; set; }

        public ConnectionData(IEnumerable<string> usersInChat, string serverName)
        {
            UsersInChat = usersInChat;
            ServerName = serverName;
        }
    }
}
