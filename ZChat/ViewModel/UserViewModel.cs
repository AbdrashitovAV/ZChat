using WPFStuff;

namespace ZChat.ViewModel
{
    internal class UserViewModel : PropertyChangedImplementation
    {
        private bool _haveNewMessage;
        public bool HaveNewMessage
        {
            get { return _haveNewMessage; }
            set { Set(() => HaveNewMessage, ref _haveNewMessage, value); }
        }

        public string Username { get; set; }

        public UserViewModel(string username)
        {
            Username = username;
        }

    }
}
