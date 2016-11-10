using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Microsoft.AspNet.SignalR;
using ZChat.Server.Logic.Hub;
using ZChat.Shared;

namespace ZChat.Server.Logic
{
    //TODO: исправить вызовы hub.client на специфицеские id из юзеров. Скорее всего это можно сделать через группы в signalR
    public class SignalRServerConnectionManager : ISignalRServerConnectionManager
    {
        private object _userLock = new object();

        private readonly ILog _generalLog;
        private readonly ILog _messageLog;
        private readonly string _serverName;

        private readonly Dictionary<string, string> _users = new Dictionary<string, string>();
        private IHubContext ChatHub => GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

        public SignalRServerConnectionManager(ILog generalLog, ILog messageLog, string serverName)
        {
            _generalLog = generalLog;
            _messageLog = messageLog;
            _serverName = serverName;
        }

        public ServiceDataResult<ConnectionData> Join(string username, string connectionId)
        {
            //TODO: show different text of whether we know user or not
            _generalLog.Debug($"Client with connectionId:\"{connectionId}\" trying to log into server with username:\"{username}\"");

            lock (_userLock)
            {
                if (_users.ContainsValue(connectionId))
                {
                    var errorMessage = $"Client with connection {connectionId} tried to log again";
                    _generalLog.Error(errorMessage);

                    return new ServiceDataResult<ConnectionData>($"User already logged from this connection");
                }

                if (_users.ContainsKey(username))
                {
                    var errorMessage = $"Client with connectionid:{connectionId} tried to log as \"{username}\". Username is occupied.";
                    _generalLog.Error(errorMessage);

                    return new ServiceDataResult<ConnectionData>($"Username {username} is occupied");
                }

                _users.Add(username, connectionId);
            }

            var userJoinedMessage = new Message { Sender = Constants.ServerId, Content = $"user {username} joined", TimeStamp = DateTime.Now };
            ChatHub.Clients.AllExcept(connectionId).MessageFromServer(userJoinedMessage);

            _generalLog.Debug($"User with connectionId:\"{connectionId}\" successfully logged into server with username:\"{username}\"");

            return new ServiceDataResult<ConnectionData>(new ConnectionData(_users.Select(x => x.Key).Except(new[] { username }), _serverName));
        }


        public ServiceResult MessageRecieved(Message message, string connectionId)
        {
            _generalLog.Debug($"Client with connectionId:\"{connectionId}\" trying to send message. \"{message.Sender}\" -> \"{message.Receiver}\" \"{message.Content}\"");

            _messageLog.Fatal($"\"{message.Sender}\" -> \"{message.Receiver}\" \"{message.Content}\"");

            if (!_users.ContainsValue(connectionId))
            {
                _generalLog.Error($"User is not authorized. Message ignored.");
                return new ServiceResult("Server don't accept message from unauthorized users.");
            }
            if (_users[message.Sender] != connectionId)
            {
                _generalLog.Error($"Sender of the message doesn't correspond with connectionId. Message ignored");
                return new ServiceResult("Incorrect sender was set.");
            }

            message.TimeStamp = DateTime.Now;

            if (message.Receiver.StartsWith("!"))
            {
                if (message.Receiver == Constants.EveryoneId)
                {
                    ChatHub.Clients.AllExcept(connectionId).MessageFromServer(message);
                    _generalLog.Debug($"Message successfully sent from {message.Sender} to everyone.");
                    return new ServiceResult();
                }
                if (message.Receiver == Constants.ServerId)
                {
                    var errorMessage = $"Message sent to {Constants.ServerId} and cannot be processed.";
                    _generalLog.Error(errorMessage);
                    return new ServiceResult(errorMessage);
                }

                _generalLog.Error($"Can't finder receiver with name {message.Receiver}");
                return new ServiceResult($"Can't finder receiver with name {message.Receiver}");
            }
            else
            {
                string receiverId;

                if (!_users.TryGetValue(message.Receiver, out receiverId))
                {
                    _generalLog.Error($"Can't finder receiver with name {message.Receiver}");
                    return new ServiceResult($"Can't finder receiver with name {message.Receiver}");
                }

                ChatHub.Clients.Client(receiverId).MessageFromServer(message);

                _generalLog.Debug($"Message successfully sent from {message.Sender} to {message.Receiver}.");

                return new ServiceResult();
            }


        }

        public ServiceResult Left(string connectionId, bool stopCalled)
        {
            if (!_users.ContainsValue(connectionId))
            {
                return new ServiceResult();
            }

            var username = _users.Single(x => x.Value == connectionId).Key;
            _users.Remove(username);

            var reason = String.Empty;

            if (stopCalled)
            {
                reason = "left";
                _generalLog.Info($"User {username } left chat");
            }
            else
            {
                reason = "disconnected";
                _generalLog.Info($"User {username } was disconnected from chat due to timeout");

            }

            var message = new Message { Sender = Constants.ServerId, Content = $"user {username} {reason}", TimeStamp = DateTime.Now };
            ChatHub.Clients.All.MessageFromServer(message);

            return new ServiceResult();
        }
    }
}
