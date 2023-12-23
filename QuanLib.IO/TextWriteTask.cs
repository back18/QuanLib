using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO
{
    public class TextWriteTask : WriteTask
    {
        public TextWriteTask(string destinationPath, string text, Encoding encoding) : base(destinationPath)
        {
            ArgumentNullException.ThrowIfNull(text, nameof(text));
            ArgumentNullException.ThrowIfNull(encoding, nameof(encoding));

            _text = text;
            Encoding = encoding;
        }

        public TextWriteTask(string destinationPath, string text) : this(destinationPath, text, Encoding.UTF8) { }

        private readonly string _text;

        private Encoding Encoding { get; }

        public override void Run()
        {
            FileUtil.CreateFileDirectoryIfNotExists(DestinationPath);
            File.WriteAllText(DestinationPath, _text, Encoding);
        }

        public override async Task RunAsync(CancellationToken cancellationToken = default)
        {
            FileUtil.CreateFileDirectoryIfNotExists(DestinationPath);
            await File.WriteAllTextAsync(DestinationPath, _text, Encoding, cancellationToken);
        }
    }
}
