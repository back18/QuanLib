using QuanLib.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public interface IRunnable
    {
        public Thread? Thread { get; }

        public bool IsRunning { get; }

        public event EventHandler<IRunnable, EventArgs> Started;

        public event EventHandler<IRunnable, EventArgs> Stopped;

        public event EventHandler<IRunnable, EventArgs<Exception>> ThrowException;

        public bool Start();

        public void Stop();

        public void WaitForStop();

        public Task WaitForStopAsync();
    }
}
