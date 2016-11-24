using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WPFStuff.BasicValidation
{
    public class BasicValidatableViewModel : PropertyChangedImplementation, IDataErrorInfo
    {
        protected Dictionary<string, string> _errors = new Dictionary<string, string>();
        protected bool _previousValidationState;

        public event EventHandler ValidStateChanged;
        
        public bool IsValid => !_errors.Any();
        public string Error { get; }

        public virtual string this[string columnName]
        {
            get
            {
                _errors.Remove(columnName);

                var error = ValidateProperty(columnName);

                if (!String.IsNullOrEmpty(error))
                    _errors[columnName] = error;

                CheckValidState();

                return error;
            }
        }

        protected virtual string ValidateProperty(string propertyName)
        {
            return String.Empty;
        }


        private void CheckValidState()
        {
            if (_previousValidationState == IsValid)
                return;

            _previousValidationState = IsValid;
            RaisePropertyChanged(() => IsValid);

            ValidStateChanged?.Invoke(this, null);
        }
    }
}