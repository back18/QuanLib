using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Events
{
    public class TextEventArgs : EventArgs
    {
        public TextEventArgs(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}
