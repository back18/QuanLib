using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Format
{
    public abstract class BytesFormatText
    {
        public abstract string B { get; }

        public abstract string KB { get; }

        public abstract string MB { get; }

        public abstract string GB { get; }

        public abstract string TB { get; }

        public abstract string PB { get; }

        public abstract string EB { get; }

        public string Get(BytesUnit bytesUnit)
        {
            return bytesUnit switch
            {
                BytesUnit.B => B,
                BytesUnit.KB => KB,
                BytesUnit.MB => MB,
                BytesUnit.GB => GB,
                BytesUnit.TB => TB,
                BytesUnit.PB => PB,
                BytesUnit.EB => EB,
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
