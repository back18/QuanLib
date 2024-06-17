using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Events
{
    public class ValueChangedEventArgs<T>(T oldValue, T newValue) : EventArgs
    {
        public T OldValue { get; } = oldValue;

        public T NewValue { get; } = newValue;
    }
}
