using System;
using System.Threading.Tasks;
using ZChat.Shared;

namespace ZChat.Client.Communcation
{
    public interface IConnectionManager
    {
        event EventHandler<Message> MessageRecieved;
        event EventHandler ConnectedToServer;
        event EventHandler DisconnectedFromServer;

        bool IsConnecting { get; }
        bool IsConnected { get; }

        Task<ServiceDataResult<ConnectionData>> OpenConnectionAsync(string username, string hostname, int port);

        Task<ServiceResult> SendMessageAsync(Message message);

        void CloseConnection();
    }
}