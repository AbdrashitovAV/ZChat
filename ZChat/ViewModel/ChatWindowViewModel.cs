using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Threading;
using Prism.Commands;
using WPFStuff;
using ZChat.Client.Communcation;
using ZChat.Shared;

namespace ZChat.ViewModel
{
    internal class ChatWindowViewModel : PropertyChangedImplementation
    {
        private readonly IConnectionManager _connectionManager;
        private Dispatcher _uiDispatcher;
        private readonly string _username;

        private Dictionary<string, ObservableCollection<Message>> _allMessages = new Dictionary<string, ObservableCollection<Message>>();

        #region User properties

        private ObservableCollection<UserViewModel> _users = new ObservableCollection<UserViewModel>();
        public ObservableCollection<UserViewModel> Users
        {
            get { return _users; }
            set
            {
                if (Set(() => Users, ref _users, value))
                {
                    UsersViewSource.SortDescriptions.Add(new SortDescription("Username", ListSortDirection.Ascending));
                }
            }
        }

        public ICollectionView UsersViewSource => CollectionViewSource.GetDefaultView(Users);

        private UserViewModel _selectedUser;

        public UserViewModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                if (Set(() => SelectedUser, ref _selectedUser, value))
                {
                    if (_selectedUser != null)
                    {
                        _selectedUser.HaveNewMessage = false;
                        Messages = _allMessages[_selectedUser.Username];
                    }
                }
            }
        }

        #endregion

        private ObservableCollection<Message> _messages = new ObservableCollection<Message>();
        public ObservableCollection<Message> Messages
        {
            get { return _messages; }
            set { Set(() => Messages, ref _messages, value); }
        }
        
        #region Message sending

        private bool _isSendingMessage;
        public bool IsSendingMessage
        {
            get { return _isSendingMessage; }
            set { Set(() => IsSendingMessage, ref _isSendingMessage, value); }
        }

        private bool _isMessagingAllowed;
        public bool IsMessagingAllowed
        {
            get { return _isMessagingAllowed; }
            set
            {
                if (Set(() => IsMessagingAllowed, ref _isMessagingAllowed, value))
                {
                    SendMessageCommand.RaiseCanExecuteChanged();
                }
                ;
            }
        }

        private string _messageText;

        public string MessageText
        {
            get { return _messageText; }
            set { Set(() => MessageText, ref _messageText, value); }
        }

        public DelegateCommand SendMessageCommand { get; set; }

        #endregion

        public ChatWindowViewModel(IConnectionManager connectionManager, Dispatcher uiDispatcher, string username, ConnectionData connectionData)
        {
            _connectionManager = connectionManager;
            _uiDispatcher = uiDispatcher;
            _username = username;

            _connectionManager.MessageRecieved += HandleMessageRecieved;

            SendMessageCommand = new DelegateCommand(SendMessage, () => IsMessagingAllowed);

            InitializeUsersLists(connectionData.UsersInChat);
        }

        private void HandleDisconnectedFromServer(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleConnectedToServer(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void InitializeUsersLists(IEnumerable<string> usersInChat)
        {
            Users = new ObservableCollection<UserViewModel>(usersInChat.Select(username => new UserViewModel(username))) { new UserViewModel(Constants.EveryoneId) };

            foreach (var uservm in Users)
            {
                _allMessages.Add(uservm.Username, new ObservableCollection<Message>());
            }

            SelectedUser = Users.Single(x => x.Username == Constants.EveryoneId);

            IsMessagingAllowed = true;
        }

        private async void SendMessage()
        {
            if (String.IsNullOrEmpty(MessageText))
                return;

            _uiDispatcher.Invoke(() =>
            {
                IsMessagingAllowed = false;
                IsSendingMessage = true;
            });

            var message = new Message { Content = (string)MessageText.Clone(), Receiver = _selectedUser.Username, TimeStamp = DateTime.Now, Sender = _username };
            //TODO: should we use this message or wait it from server?

            var sendMessageTask = _connectionManager.SendMessageAsync(message);

            await sendMessageTask;
            if (sendMessageTask.Result.IsSucessful)
            {
                _allMessages[message.Receiver].Add(message);
                MessageText = String.Empty;
            };

            _uiDispatcher.Invoke(() =>
            {
                IsSendingMessage = false;
                IsMessagingAllowed = true;
            });
        }

        private void UserConnected(Message message, string newUser)
        {
            if (!_allMessages.ContainsKey(newUser))
                _allMessages.Add(newUser, new ObservableCollection<Message>());

            _uiDispatcher.Invoke(() =>
            {
                _allMessages[newUser].Add(message);
                _allMessages[Constants.EveryoneId].Add(message);
                Users.Add(new UserViewModel(newUser));
            });
        }

        private void UserDisconnected(Message message, string user)
        {
            _uiDispatcher.Invoke(() =>
            {
                _allMessages[user].Add(message);
                _allMessages[Constants.EveryoneId].Add(message);

                var userToDelete = Users.SingleOrDefault(userViewModel => userViewModel.Username == user);
                if (userToDelete != null)
                    Users.Remove(userToDelete);
            });
        }

        //TODO - выделить логику обработки, хранения и загрузки сообщений в отдельный класс
        private void HandleMessageRecieved(object sender, Message message)
        {
            _uiDispatcher.Invoke(() =>
            {
                if (message.Sender == Constants.ServerId)
                {
                    ProcessServerMessage(message);
                    return;
                }

                var usernameToUpdate = message.Receiver == Constants.EveryoneId ? Constants.EveryoneId : message.Sender;

                if (_selectedUser.Username != usernameToUpdate)
                    Users.First(x => x.Username == usernameToUpdate).HaveNewMessage = true;

                _allMessages[usernameToUpdate].Add(message);
            });
        }

        private void ProcessServerMessage(Message message)
        {
            if (message.Content.ToLower().StartsWith("user"))
            {
                var messageParts = message.Content.Split(' ').ToList();
                switch (messageParts[2])
                {
                    case "joined":
                        UserConnected(message, messageParts[1]);
                        break;
                    case "left":
                    case "disconnected":
                        UserDisconnected(message, messageParts[1]);
                        break;
                    default:
                        return;
                }
            }
        }
    }
}