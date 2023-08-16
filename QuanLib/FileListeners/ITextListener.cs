﻿using QuanLib.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.FileListeners
{
    public interface ITextListener
    {
        public event EventHandler<ITextListener, TextEventArgs> WriteLineText;
    }
}
