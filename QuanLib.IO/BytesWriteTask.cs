using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO
{
    public class BytesWriteTask : WriteTask
    {
        public BytesWriteTask(string destinationPath, byte[] bytes) : base(destinationPath)
        {
            ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));

            _bytes = bytes;
        }

        private readonly byte[] _bytes;

        public override void Run()
        {
            FileUtil.CreateFileDirectoryIfNotExists(DestinationPath);
            File.WriteAllBytes(DestinationPath, _bytes);
        }

        public override async Task RunAsync(CancellationToken cancellationToken = default)
        {
            FileUtil.CreateFileDirectoryIfNotExists(DestinationPath);
            await File.WriteAllBytesAsync(DestinationPath, _bytes, cancellationToken);
        }
    }
}
