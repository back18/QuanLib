using QuanLib.Core.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Proxys
{
    public class TextWriterProxy : TextWriter
    {
        public TextWriterProxy(TextWriter owner)
        {
            ArgumentNullException.ThrowIfNull(owner, nameof(owner));

            _owner = owner;
            _interceptionText = new();
            RequestInterception = false;

            CharWriting += OnCharWriting;
            StringWriting += OnStringWriting;
            CharWritten += OnCharWritten;
            StringWritten += OnStringWritten;
        }

        private readonly TextWriter _owner;

        private readonly StringBuilder _interceptionText;

        public override IFormatProvider FormatProvider => _owner.FormatProvider;

        public override string NewLine => _owner.NewLine;

        public override Encoding Encoding => _owner.Encoding;

        public bool RequestInterception { get; set; }

        public event EventHandler<TextWriterProxy, EventArgs<char>> CharWriting;

        public event EventHandler<TextWriterProxy, EventArgs<string>> StringWriting;

        public event EventHandler<TextWriterProxy, EventArgs<char>> CharWritten;

        public event EventHandler<TextWriterProxy, EventArgs<string>> StringWritten;

        protected virtual void OnCharWriting(TextWriterProxy sender, EventArgs<char> e) { }

        protected virtual void OnStringWriting(TextWriterProxy sender, EventArgs<string> e) { }

        protected virtual void OnCharWritten(TextWriterProxy sender, EventArgs<char> e) { }

        protected virtual void OnStringWritten(TextWriterProxy sender, EventArgs<string> e) { }

        private void TriggerWritingEvent(char value)
        {
            StringWriting.Invoke(this, new(value.ToString()));
            CharWriting.Invoke(this, new(value));
        }

        private void TriggerWrittenEvent(char value)
        {
            StringWriting.Invoke(this, new(value.ToString()));
            CharWriting.Invoke(this, new(value));
        }

        private void TriggerWritingEvent(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return;

            StringWriting.Invoke(this, new(value));
            foreach (char c in value)
                CharWriting.Invoke(this, new(c));
        }

        private void TriggerWrittenEvent(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return;

            StringWriting.Invoke(this, new(value));
            foreach (char c in value)
                CharWriting.Invoke(this, new(c));
        }

        public string GetInterceptionText()
        {
            lock (_interceptionText)
            {
                string text = _interceptionText.ToString();
                _interceptionText.Clear();
                return text;
            }
        }

        public void WriteOnly(char value)
        {
            _owner.Write(value);
        }

        public void WriteOnly(string value)
        {
            _owner.Write(value);
        }

        public async Task WriteOnlyAsync(char value)
        {
            await _owner.WriteAsync(value);
        }

        public async Task WriteOnlyAsync(string value)
        {
            await _owner.WriteAsync(value);
        }

        public void WriteLineOnly()
        {
            _owner.WriteLine();
        }

        public void WriteLineOnly(char value)
        {
            _owner.WriteLine(value);
        }

        public void WriteLineOnly(string value)
        {
            _owner.WriteLine(value);
        }

        public async Task WriteLineOnlyAsync()
        {
            await _owner.WriteLineAsync();
        }

        public async Task WriteLineOnlyAsync(char value)
        {
            await _owner.WriteLineAsync(value);
        }

        public async Task WriteLineOnlyAsync(string value)
        {
            await _owner.WriteLineAsync(value);
        }

        public override void Write(char value)
        {
            TriggerWritingEvent(value);
            if (RequestInterception)
            {
                lock (_interceptionText)
                    _interceptionText.Append(value);
                return;
            }

            _owner.Write(value);
            TriggerWrittenEvent(value);
        }

        public override void Write(string? value)
        {
            TriggerWritingEvent(value);
            if (RequestInterception)
            {
                lock (_interceptionText)
                    _interceptionText.Append(value);
                return;
            }

            _owner.Write(value);
            TriggerWrittenEvent(value);
        }

        public override async Task WriteAsync(char value)
        {
            TriggerWritingEvent(value);
            if (RequestInterception)
            {
                lock (_interceptionText)
                    _interceptionText.Append(value);
                return;
            }

            await _owner.WriteAsync(value);
            TriggerWrittenEvent(value);
        }

        public override async Task WriteAsync(string? value)
        {
            TriggerWritingEvent(value);
            if (RequestInterception)
            {
                lock (_interceptionText)
                    _interceptionText.Append(value);
                return;
            }

            await _owner.WriteAsync(value);
            TriggerWrittenEvent(value);
        }

        public override void WriteLine()
        {
            TriggerWritingEvent(NewLine);
            if (RequestInterception)
            {
                lock (_interceptionText)
                    _interceptionText.Append(NewLine);
                return;
            }

            _owner.WriteLine();
            TriggerWrittenEvent(NewLine);
        }

        public override void WriteLine(char value)
        {
            TriggerWritingEvent(value + NewLine);
            if (RequestInterception)
            {
                lock (_interceptionText)
                    _interceptionText.Append(value + NewLine);
                return;
            }

            _owner.WriteLine(value);
            TriggerWrittenEvent(value + NewLine);
        }

        public override void WriteLine(string? value)
        {
            TriggerWritingEvent(value + NewLine);
            if (RequestInterception)
            {
                lock (_interceptionText)
                    _interceptionText.Append(value + NewLine);
                return;
            }

            _owner.WriteLine(value);
            TriggerWrittenEvent(value + NewLine);
        }

        public override async Task WriteLineAsync()
        {
            TriggerWritingEvent(NewLine);
            if (RequestInterception)
            {
                lock (_interceptionText)
                    _interceptionText.Append(NewLine);
                return;
            }

            await _owner.WriteLineAsync();
            TriggerWrittenEvent(NewLine);
        }

        public override async Task WriteLineAsync(char value)
        {
            TriggerWritingEvent(value + NewLine);
            if (RequestInterception)
            {
                lock (_interceptionText)
                    _interceptionText.Append(value + NewLine);
                return;
            }

            await _owner.WriteLineAsync(value);
            TriggerWrittenEvent(value + NewLine);
        }

        public override async Task WriteLineAsync(string? value)
        {
            TriggerWritingEvent(value + NewLine);
            if (RequestInterception)
            {
                lock (_interceptionText)
                    _interceptionText.Append(value + NewLine);
                return;
            }

            await _owner.WriteLineAsync(value);
            TriggerWrittenEvent(value + NewLine);
        }

        public override void Flush()
        {
            _owner.Flush();
        }

        public override Task FlushAsync()
        {
            return _owner.FlushAsync();
        }

        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            return _owner.FlushAsync(cancellationToken);
        }

        public override void Close()
        {
            _owner.Close();
        }

        public override async ValueTask DisposeAsync()
        {
            await _owner.DisposeAsync();
            GC.SuppressFinalize(this);
        }

        public override bool Equals(object? obj)
        {
            return _owner.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _owner.GetHashCode();
        }

        public override string ToString()
        {
            return _owner.ToString() ?? GetType().ToString();
        }
    }
}
