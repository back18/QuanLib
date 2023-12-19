using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO
{
    public abstract class WriteTask
    {
        protected WriteTask(string destinationPath)
        {
            ArgumentException.ThrowIfNullOrEmpty(destinationPath, nameof(destinationPath));

            DestinationPath = destinationPath;
        }

        public string DestinationPath { get; }

        public abstract void Run();

        public abstract Task RunAsync(CancellationToken cancellationToken = default);
    }
}
