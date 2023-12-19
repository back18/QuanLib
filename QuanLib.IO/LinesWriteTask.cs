using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO
{
    public class LinesWriteTask : WriteTask
    {
        public LinesWriteTask(string destinationPath, IEnumerable<string> lines, Encoding encoding) : base(destinationPath)
        {
            ArgumentNullException.ThrowIfNull(lines, nameof(lines));
            ArgumentNullException.ThrowIfNull(encoding, nameof(encoding));

            _lines = lines;
            Encoding = encoding;
        }

        public LinesWriteTask(string destinationPath, IEnumerable<string> lines) : this(destinationPath, lines, Encoding.UTF8) { }

        private readonly IEnumerable<string> _lines;

        private Encoding Encoding { get; }

        public override void Run()
        {
            File.WriteAllLines(DestinationPath, _lines, Encoding);
        }

        public override async Task RunAsync(CancellationToken cancellationToken = default)
        {
            await File.WriteAllLinesAsync(DestinationPath, _lines, Encoding, cancellationToken);
        }
    }
}
