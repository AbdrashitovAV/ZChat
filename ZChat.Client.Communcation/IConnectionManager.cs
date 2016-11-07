using System;
using System.Threading.Tasks;
using ZChat.Client.Communcation.Event;
using ZChat.Shared;

namespace ZChat.Client.Communcation
{
    public interface IConnectionManager
    {
        event EventHandler<Message> MessageRecieved;
        event EventHandler<ConnectedToServerEventArgs> ConnectedToServer;
        event EventHandler DisconnectedFromServer;

        bool IsConnecting { get; }
        bool IsConnected { get; }

        Task<ServiceDataResult<ConnectionData>> OpenConnectionAsync(string username, string hostname, int port);

        Task<ServiceResult> SendMessageAsync(Message message);

        void CloseConnection();
    }
}