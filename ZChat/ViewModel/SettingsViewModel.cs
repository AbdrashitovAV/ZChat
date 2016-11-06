using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using WPFStuff;

namespace ZChat.ViewModel
{
    internal class SettingsViewModel : PropertyChangedImplementation, IDataErrorInfo
    {
        private Dictionary<string, string> _errors = new Dictionary<string, string>();

        public event EventHandler ValidStateChanged;

        private bool _previousValidationState;
        public bool IsValid => !_errors.Any();

        private string _username;
        public string Username
        {
            get { return _username; }
            set { Set(() => Username, ref _username, value); }
        }

        private string _hostname = "localhost";
        public string Hostname
        {
            get { return _hostname; }
            set { Set(() => Hostname, ref _hostname, value); }
        }

        private int _port = 8080;
        public int Port
        {
            get { return _port; }
            set { Set(() => Port, ref _port, value); }
        }

        public string this[string columnName]
        {
            get
            {
                var error = String.Empty;
                _errors.Remove(columnName);

                switch (columnName)
                {
                    case nameof(Hostname):
                        error = ValidateHostname();
                        break;

                    case nameof(Username):
                        error = ValidateUsername();
                        break;

                    case nameof(Port):
                        error = ValidatePort();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (!String.IsNullOrEmpty(error))
                    _errors[columnName] = error;

                CheckValidState();

                return error;
            }
        }

        private string ValidatePort()
        {
            if (_port < 0 || _port > 65536)
            {
                return "Port should be number between 0 and 65536";
            }

            return null;
        }

        private string ValidateUsername()
        {
            if (String.IsNullOrEmpty(_username))
            {
                return "Port should be number between 0 and 65536";
            }

            var regex = new Regex(@"^\w+$");
            if (!regex.IsMatch(_username))
            {
                return "Port should be number between 0 and 65536";
            }

            return null;
        }

        private string ValidateHostname()
        {

            if (String.IsNullOrEmpty(_hostname))
            {
                return "Hostname should not be empty";
            }

            return null;
        }

        private void CheckValidState()
        {
            if (_previousValidationState == IsValid)
                return;

            _previousValidationState = IsValid;
            RaisePropertyChanged(() => IsValid);

            ValidStateChanged?.Invoke(this, null);
        }

        public string Error { get; }
    }
}