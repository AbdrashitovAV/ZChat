using System;
using System.ServiceModel;
using System.Threading.Tasks;
using ZChat.Shared;
using ZChat.Shared.Contracts;

namespace ZChat.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                     ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ChatService : IChatService
    {
        public Task<bool> LogUser(string userName)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// Keeps the connection between the client and server.
        /// Connection between a client and server has a time-out,
        /// so the client needs to call this before that happens
        /// to remain connected to the server.
        /// </summary>
        public Task KeepConnection()
        {
            throw new NotImplementedException();
        }

        public Task SendMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
