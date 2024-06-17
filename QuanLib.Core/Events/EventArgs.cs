using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Events
{
    public class EventArgs<T>(T argument) : EventArgs
    {
        public T Argument { get; } = argument;
    }

    public class EventArgs<T1, T2>(T1 argument1, T2 argument2) : EventArgs
    {
        public T1 Argument1 { get; } = argument1;
        public T2 Argument2 { get; } = argument2;
    }

    public class EventArgs<T1, T2, T3>(T1 argument1, T2 argument2, T3 argument3) : EventArgs
    {
        public T1 Argument1 { get; } = argument1;
        public T2 Argument2 { get; } = argument2;
        public T3 Argument3 { get; } = argument3;
    }

    public class EventArgs<T1, T2, T3, T4>(T1 argument1, T2 argument2, T3 argument3, T4 argument4) : EventArgs
    {
        public T1 Argument1 { get; } = argument1;
        public T2 Argument2 { get; } = argument2;
        public T3 Argument3 { get; } = argument3;
        public T4 Argument4 { get; } = argument4;
    }

    public class EventArgs<T1, T2, T3, T4, T5>(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5) : EventArgs
    {
        public T1 Argument1 { get; } = argument1;
        public T2 Argument2 { get; } = argument2;
        public T3 Argument3 { get; } = argument3;
        public T4 Argument4 { get; } = argument4;
        public T5 Argument5 { get; } = argument5;
    }

    public class EventArgs<T1, T2, T3, T4, T5, T6>(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6) : EventArgs
    {
        public T1 Argument1 { get; } = argument1;
        public T2 Argument2 { get; } = argument2;
        public T3 Argument3 { get; } = argument3;
        public T4 Argument4 { get; } = argument4;
        public T5 Argument5 { get; } = argument5;
        public T6 Argument6 { get; } = argument6;
    }

    public class EventArgs<T1, T2, T3, T4, T5, T6, T7>(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7) : EventArgs
    {
        public T1 Argument1 { get; } = argument1;
        public T2 Argument2 { get; } = argument2;
        public T3 Argument3 { get; } = argument3;
        public T4 Argument4 { get; } = argument4;
        public T5 Argument5 { get; } = argument5;
        public T6 Argument6 { get; } = argument6;
        public T7 Argument7 { get; } = argument7;
    }

    public class EventArgs<T1, T2, T3, T4, T5, T6, T7, T8>(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8) : EventArgs
    {
        public T1 Argument1 { get; } = argument1;
        public T2 Argument2 { get; } = argument2;
        public T3 Argument3 { get; } = argument3;
        public T4 Argument4 { get; } = argument4;
        public T5 Argument5 { get; } = argument5;
        public T6 Argument6 { get; } = argument6;
        public T7 Argument7 { get; } = argument7;
        public T8 Argument8 { get; } = argument8;
    }
}
