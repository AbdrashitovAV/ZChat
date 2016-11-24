using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WPFStuff.BasicValidation;

namespace ZChat.ViewModel
{
    internal class SettingsViewModel : BasicValidatableViewModel
    {

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

        protected sealed override string ValidateProperty(string propertyName)
        {
            var error = String.Empty;

            switch (propertyName)
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
                    error = base.ValidateProperty(propertyName);
                    break;
            }

            return error;
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
    }
}