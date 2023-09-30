using QuanLib.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.IO
{
    public interface ITextListener
    {
        public Encoding Encoding { get; }

        public event EventHandler<ITextListener, TextEventArgs> WriteText;

        public event EventHandler<ITextListener, TextEventArgs> WriteLineText;
    }
}
