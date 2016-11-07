using System;
using ZChat.Shared;

namespace ZChat.Client.Communcation.Event
{
    public class ConnectedToServerEventArgs : EventArgs
    {
        public string Username { get; set; }
        public ConnectionData ConnectionData { get; set; }
    }
}
