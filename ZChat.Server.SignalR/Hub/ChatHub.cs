using System;
using System.Management.Instrumentation;
using System.Threading.Tasks;
using ZChat.Shared;

namespace ZChat.Server.SignalR.Hub
{
    public class ChatHub : Microsoft.AspNet.SignalR.Hub
    {
        private readonly ISignalRServerConnectionManager _connectionManager;

        public ChatHub(ISignalRServerConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;

            if (_connectionManager == null)
                throw new InstanceNotFoundException();
        }

        public ServiceDataResult<ConnectionData> Join(string username)
        {
            var connectionId = Context.ConnectionId;

            return _connectionManager.Join(username, connectionId);
        }

        public ServiceResult MessageToServer(Message message)
        {
            return _connectionManager.MessageRecieved(message, Context.ConnectionId);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _connectionManager.Left(Context.ConnectionId, stopCalled);

            return base.OnDisconnected(stopCalled);
        }

    }
}