using System;

namespace ZChat.Shared
{
    public class Message
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }

        public DateTime TimeStamp { get; set; }
        public string Content { get; set; }
    }
}
