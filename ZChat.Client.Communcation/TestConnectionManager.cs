using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZChat.Client.Communcation.Event;
using ZChat.Shared;
using Timer = System.Timers.Timer;

namespace ZChat.Client.Communcation
{
    public class TestConnectionManager : IConnectionManager
    {
        private const string _timeServer = "Time server";
        private const string _joe = "joe";

        private Timer _timeServerTimer;
        private Timer _joeTimer;
        private bool _isJoeHere = false;
        public bool IsConnecting { get; private set; }
        public bool IsConnected { get; private set; }

        public event EventHandler<Message> MessageRecieved;
        public event EventHandler<ConnectedToServerEventArgs> ConnectedToServer;
        public event EventHandler DisconnectedFromServer;

        public TestConnectionManager()
        {
            _timeServerTimer = new Timer { Interval = 5000 };
            _timeServerTimer.Elapsed += (sender, args) => { MessageRecieved?.Invoke(this, new Message() { Sender = _timeServer, Content = "Current time: " + DateTime.Now.ToShortTimeString(), TimeStamp = DateTime.Now }); };

            _joeTimer = new Timer(10000);
            _joeTimer.Elapsed += (sender, args) =>
            {
                var message = new Message { Sender = Constants.ServerId, TimeStamp = DateTime.Now };
                if (_isJoeHere)
                {
                    message.Content = $"user {_joe} left";
                    MessageRecieved?.Invoke(this, message);
                }
                else
                {
                    message.Content = $"user {_joe} joined";
                    MessageRecieved?.Invoke(this, message);

                }

                _isJoeHere = !_isJoeHere;
            };
        }
        public async Task<ServiceDataResult<ConnectionData>> OpenConnectionAsync(string username, string hostname, int port)
        {
            if (!IsConnecting && !IsConnected)
            {
                IsConnecting = true;
                await Task.Factory.StartNew(
                    () =>
                    {
                        Thread.Sleep(2000);
                    });

                _timeServerTimer.Start();
                _joeTimer.Start();

                ConnectedToServer?.Invoke(this, null);

                IsConnecting = false;
                IsConnected = true;
            }
            return new ServiceDataResult<ConnectionData>(new ConnectionData(new List<string> { _timeServer }, "test server"));
        }

        public Task<ServiceResult> SendMessageAsync(Message message)
        {
            var sendingTask = Task<ServiceResult>.Factory.StartNew(() =>
            {
                message.TimeStamp = DateTime.Now;
                Thread.Sleep(2000); // Имитация отправки...

                return new ServiceResult();
            });

            return sendingTask;
        }

        public void CloseConnection()
        {
            _timeServerTimer.Stop();
            _joeTimer.Stop();

            IsConnected = false;
        }
    }
}
