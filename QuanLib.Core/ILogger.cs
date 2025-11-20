using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public interface ILogger
    {
        public bool IsDebugEnabled { get; }

        public bool IsInfoEnabled { get; }

        public bool IsWarnEnabled { get; }

        public bool IsErrorEnabled { get; }

        public bool IsFatalEnabled { get; }

        public void Debug(string message);

        public void Debug(string message, Exception? exception);

        public void Info(string message);

        public void Info(string message, Exception? exception);

        public void Warn(string message);

        public void Warn(string message, Exception? exception);

        public void Error(string message);

        public void Error(string message, Exception? exception);

        public void Fatal(string message);

        public void Fatal(string message, Exception? exception);
    }
}
