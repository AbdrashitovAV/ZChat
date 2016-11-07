using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using ZChat.Client.Communcation.Event;
using ZChat.Shared;

namespace ZChat.Client.Communcation
{
    public class SignalRConnectionManager : IConnectionManager
    {
        private IHubProxy _hubProxy;
        private HubConnection _hubConnection;

        public event EventHandler<Message> MessageRecieved;
        public event EventHandler<ConnectedToServerEventArgs> ConnectedToServer;
        public event EventHandler DisconnectedFromServer;

        public bool IsConnecting { get; private set; } = false;
        public bool IsConnected { get; private set; } = false;

        public async Task<ServiceDataResult<ConnectionData>> OpenConnectionAsync(string username, string hostname, int port)
        {
            try
            {
                if (IsConnecting)
                {
                    return new ServiceDataResult<ConnectionData>("Already connecting");
                }
                if (IsConnected)
                {
                    return new ServiceDataResult<ConnectionData>("Already connected");
                }

                IsConnecting = true;

                SetHubConnection(hostname, port);
                await _hubConnection.Start();
            }
            catch (HttpRequestException e)
            {
                IsConnecting = false;

                return new ServiceDataResult<ConnectionData>()
                {
                    IsSucessful = false,
                    ErrorMessage = e.Message
                };
            }

            var task = _hubProxy.Invoke<ServiceDataResult<ConnectionData>>("Join", username);
            await task;

            IsConnecting = false;

            if (task.Result.IsSucessful)
            {
                IsConnected = true;

                ConnectedToServer?.Invoke(this, new ConnectedToServerEventArgs() { Username = username, ConnectionData = task.Result.Data });
            }
            else
            {
                ClearConnection();
            }

            return task.Result;
        }

        public Task<ServiceResult> SendMessageAsync(Message message)
        {
            if (!IsConnected)
                Task<ServiceResult>.Factory.StartNew(() => new ServiceResult("Connection is not opened"));

            return _hubProxy.Invoke<ServiceResult>("MessageToServer", message);
        }

        public void CloseConnection()
        {
            IsConnected = false;
            ClearConnection();
        }

        private void ClearConnection()
        {
            _hubProxy = null;
            _hubConnection?.Dispose();
            _hubConnection = null;
        }


        private void SetHubConnection(string hostname, int port)
        {
            _hubConnection = new HubConnection($"http://{hostname}:{port}/");

            _hubConnection.Error += q =>
            {
                IsConnected = false;
                ClearConnection();
                DisconnectedFromServer?.Invoke(this, null);
            };

            _hubProxy = _hubConnection.CreateHubProxy("ChatHub");
            _hubProxy.On<Message>("MessageFromServer", message => { MessageRecieved?.Invoke(this, message); });
            
            ServicePointManager.DefaultConnectionLimit = 10;
        }
    }
}
