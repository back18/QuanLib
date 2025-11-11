using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public interface IWaitible
    {
        public void Wait();

        public void Wait(CancellationToken cancellationToken);

        public bool Wait(int millisecondsTimeout);

        public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken);

        public bool Wait(TimeSpan timeout);

        public bool Wait(TimeSpan timeout, CancellationToken cancellationToken);

        public Task WaitAsync();

        public Task WaitAsync(CancellationToken cancellationToken);

        public Task<bool> WaitAsync(int millisecondsTimeout);

        public Task<bool> WaitAsync(int millisecondsTimeout, CancellationToken cancellationToken);

        public Task<bool> WaitAsync(TimeSpan timeout);

        public Task<bool> WaitAsync(TimeSpan timeout, CancellationToken cancellationToken);
    }
}
