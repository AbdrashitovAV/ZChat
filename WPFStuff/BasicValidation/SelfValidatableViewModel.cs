using System;
using System.Collections.Generic;

namespace WPFStuff.BasicValidation
{
    public class SelfValidatableViewModel : BasicValidatableViewModel
    {
        private readonly IDictionary<string, IBasicValidator> _validators;

        protected SelfValidatableViewModel(IDictionary<string, IBasicValidator> validators)
        {
            _validators = validators;
        }
        
        public override string this[string columnName] => _validators.ContainsKey(columnName) ? base[columnName] : String.Empty;

        protected override string ValidateProperty(string propertyName)
        {
            var propertyValue = GetType().GetProperty(propertyName).GetValue(this);
            var validator = _validators[propertyName];

            return validator.Validate(propertyValue);
        }
    }
}
