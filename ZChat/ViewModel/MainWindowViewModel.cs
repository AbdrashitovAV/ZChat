using System.Windows;
using System.Windows.Threading;
using Prism.Commands;
using WPFStuff;
using ZChat.Client.Communcation;

namespace ZChat.ViewModel
{
    internal class MainWindowViewModel : PropertyChangedImplementation
    {
        private Dispatcher _uiDispatcher;
        private IConnectionManager _connectionManager;
        public string ApplicationTitle { get; set; }

        public SettingsViewModel SettingsViewModel { get; set; }
        public ChatWindowViewModel ChatWindowViewModel { get; set; }

        public bool AreSettingsSet { get; set; } = false;

        public DelegateCommand ApplySettingsCommand { get; set; }
        public DelegateCommand CloseWindowCommand { get; set; }

        public MainWindowViewModel()
        {
            _connectionManager = new SignalRConnectionManager();

            SettingsViewModel = new SettingsViewModel();
            ApplySettingsCommand = new DelegateCommand(ApplySettings, ApplySettingsCanExecute);
            CloseWindowCommand = new DelegateCommand(Close);
            ApplicationTitle = "ZChat";
            _uiDispatcher = Dispatcher.CurrentDispatcher; // make it nicer
            

            //todo: make initialization from events from connection manager when disconnect/reconnect functionality will be done
            SettingsViewModel.ValidStateChanged += (sender, args) => { ApplySettingsCommand.RaiseCanExecuteChanged(); };
        }

        private void Close()
        {
            _connectionManager.CloseConnection();
        }

        private async void ApplySettings()
        {
            //todo: show progress or just message "connecting..."

            var connectionAsync = _connectionManager.OpenConnectionAsync(SettingsViewModel.Username, SettingsViewModel.Hostname, SettingsViewModel.Port);

            ApplySettingsCommand.RaiseCanExecuteChanged();

            await connectionAsync;

            var connectionResult = connectionAsync.Result;
            if (connectionResult.IsSucessful)
            {
                var connectionData = connectionResult.Data;
                
                ChatWindowViewModel = new ChatWindowViewModel(_connectionManager, _uiDispatcher, SettingsViewModel.Username, connectionData);
                RaisePropertyChanged(() => ChatWindowViewModel);

                ApplicationTitle = $"ZChat | {SettingsViewModel.Username} at {connectionData.ServerName} ({SettingsViewModel.Hostname}:{SettingsViewModel.Port})";
                RaisePropertyChanged(() => ApplicationTitle);

                AreSettingsSet = true;
                RaisePropertyChanged(() => AreSettingsSet);
            }
            else
            {
                MessageBox.Show(connectionResult.ErrorMessage, "Connection error");
            }

            ApplySettingsCommand.RaiseCanExecuteChanged();
        }
        private bool ApplySettingsCanExecute()
        {
            return SettingsViewModel.IsValid && !_connectionManager.IsConnecting && !_connectionManager.IsConnected;
        }

    }
}
