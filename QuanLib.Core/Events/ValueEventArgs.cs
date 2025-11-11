using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Events
{
    public readonly struct ValueEventArgs : IValueEventArgs
    {
        public static readonly ValueEventArgs Empty = default;
    }

    public readonly struct ValueEventArgs<T>(T argument) : IValueEventArgs
    {
        public readonly T Argument = argument;
    }

    public readonly struct ValueEventArgs<T1, T2>(T1 argument1, T2 argument2) : IValueEventArgs
    {
        public readonly T1 Argument1 = argument1;
        public readonly T2 Argument2 = argument2;
    }

    public readonly struct ValueEventArgs<T1, T2, T3>(T1 argument1, T2 argument2, T3 argument3) : IValueEventArgs
    {
        public readonly T1 Argument1 = argument1;
        public readonly T2 Argument2 = argument2;
        public readonly T3 Argument3 = argument3;
    }

    public readonly struct ValueEventArgs<T1, T2, T3, T4>(T1 argument1, T2 argument2, T3 argument3, T4 argument4) : IValueEventArgs
    {
        public readonly T1 Argument1 = argument1;
        public readonly T2 Argument2 = argument2;
        public readonly T3 Argument3 = argument3;
        public readonly T4 Argument4 = argument4;
    }

    public readonly struct ValueEventArgs<T1, T2, T3, T4, T5>(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5) : IValueEventArgs
    {
        public readonly T1 Argument1 = argument1;
        public readonly T2 Argument2 = argument2;
        public readonly T3 Argument3 = argument3;
        public readonly T4 Argument4 = argument4;
        public readonly T5 Argument5 = argument5;
    }

    public readonly struct ValueEventArgs<T1, T2, T3, T4, T5, T6>(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6) : IValueEventArgs
    {
        public readonly T1 Argument1 = argument1;
        public readonly T2 Argument2 = argument2;
        public readonly T3 Argument3 = argument3;
        public readonly T4 Argument4 = argument4;
        public readonly T5 Argument5 = argument5;
        public readonly T6 Argument6 = argument6;
    }

    public readonly struct ValueEventArgs<T1, T2, T3, T4, T5, T6, T7>(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7) : IValueEventArgs
    {
        public readonly T1 Argument1 = argument1;
        public readonly T2 Argument2 = argument2;
        public readonly T3 Argument3 = argument3;
        public readonly T4 Argument4 = argument4;
        public readonly T5 Argument5 = argument5;
        public readonly T6 Argument6 = argument6;
        public readonly T7 Argument7 = argument7;
    }

    public readonly struct ValueEventArgs<T1, T2, T3, T4, T5, T6, T7, T8>(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8) : IValueEventArgs
    {
        public readonly T1 Argument1 = argument1;
        public readonly T2 Argument2 = argument2;
        public readonly T3 Argument3 = argument3;
        public readonly T4 Argument4 = argument4;
        public readonly T5 Argument5 = argument5;
        public readonly T6 Argument6 = argument6;
        public readonly T7 Argument7 = argument7;
        public readonly T8 Argument8 = argument8;
    }
}
