using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace WPFStuff
{

    public abstract class PropertyChangedImplementation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged<T>(Expression<Func<T>> raiser)
        {
            var e = PropertyChanged;
            if (e != null)
            {
                var propName = ((MemberExpression)raiser.Body).Member.Name;
                e(this, new PropertyChangedEventArgs(propName));
            }
        }

        protected bool Set<T>( Expression<Func<T>> propertyExpression,
                               ref T field, 
                               T newValue)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            field = newValue;
            RaisePropertyChanged(propertyExpression);

            return true;
        }

    }

}
