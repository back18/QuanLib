using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Events
{
    public class ValueEventArgs<T>(T value) : EventArgs
    {
        public T Value { get; } = value;
    }

    public class ValueEventArgs<T1, T2>(T1 value1, T2 value2) : EventArgs
    {
        public T1 Value1 { get; } = value1;
        public T2 Value2 { get; } = value2;
    }

    public class ValueEventArgs<T1, T2, T3>(T1 value1, T2 value2, T3 value3) : EventArgs
    {
        public T1 Value1 { get; } = value1;
        public T2 Value2 { get; } = value2;
        public T3 Value3 { get; } = value3;
    }

    public class ValueEventArgs<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4) : EventArgs
    {
        public T1 Value1 { get; } = value1;
        public T2 Value2 { get; } = value2;
        public T3 Value3 { get; } = value3;
        public T4 Value4 { get; } = value4;
    }

    public class ValueEventArgs<T1, T2, T3, T4, T5>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5) : EventArgs
    {
        public T1 Value1 { get; } = value1;
        public T2 Value2 { get; } = value2;
        public T3 Value3 { get; } = value3;
        public T4 Value4 { get; } = value4;
        public T5 Value5 { get; } = value5;
    }

    public class ValueEventArgs<T1, T2, T3, T4, T5, T6>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6) : EventArgs
    {
        public T1 Value1 { get; } = value1;
        public T2 Value2 { get; } = value2;
        public T3 Value3 { get; } = value3;
        public T4 Value4 { get; } = value4;
        public T5 Value5 { get; } = value5;
        public T6 Value6 { get; } = value6;
    }

    public class ValueEventArgs<T1, T2, T3, T4, T5, T6, T7>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7) : EventArgs
    {
        public T1 Value1 { get; } = value1;
        public T2 Value2 { get; } = value2;
        public T3 Value3 { get; } = value3;
        public T4 Value4 { get; } = value4;
        public T5 Value5 { get; } = value5;
        public T6 Value6 { get; } = value6;
        public T7 Value7 { get; } = value7;
    }

    public class ValueEventArgs<T1, T2, T3, T4, T5, T6, T7, T8>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8) : EventArgs
    {
        public T1 Value1 { get; } = value1;
        public T2 Value2 { get; } = value2;
        public T3 Value3 { get; } = value3;
        public T4 Value4 { get; } = value4;
        public T5 Value5 { get; } = value5;
        public T6 Value6 { get; } = value6;
        public T7 Value7 { get; } = value7;
        public T8 Value8 { get; } = value8;
    }
}
