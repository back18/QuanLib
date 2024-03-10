using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Logging
{
    public class LoggerGetter : ILoggerGetter
    {
        public LoggerGetter(LogManager owner)
        {
            ArgumentNullException.ThrowIfNull(owner, nameof(owner));

            _owner = owner;
        }

        private readonly LogManager _owner;

        public ILogger GetLogger() => new Log4NetLogger(_owner.GetLogger());

        public ILogger GetLogger(Type type) => new Log4NetLogger(_owner.GetLogger(type));

        public ILogger GetLogger(string name) => new Log4NetLogger(_owner.GetLogger(name));
    }
}
