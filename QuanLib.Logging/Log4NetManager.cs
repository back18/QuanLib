using log4net.Appender;
using log4net.Core;
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
    public class Log4NetManager : ILoggerProvider, ISingleton<Log4NetManager, Log4NetManager.InstantiateArgs>
    {
        private Log4NetManager(ILog4NetProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider, nameof(provider));

            _provider = provider;
            _consoleAppenders = [];
        }

        private static readonly object _slock = new();

        private readonly ILog4NetProvider _provider;

        private readonly List<AppenderLevel> _consoleAppenders;

        public static bool IsInstanceLoaded => _Instance is not null;

        public static Log4NetManager Instance => _Instance ?? throw new InvalidOperationException("实例未加载");
        private static Log4NetManager? _Instance;

        public ILog4NetProvider GetProvider()
        {
            return _provider;
        }

        public Core.ILogger GetLogger()
        {
            return _provider.GetLogger();
        }

        public Core.ILogger GetLogger(Type type)
        {
            return _provider.GetLogger(type);
        }

        public Core.ILogger GetLogger(string name)
        {
            return _provider.GetLogger(name);
        }

        public void DisableConsoleOutput()
        {
            ConsoleAppender[] consoleAppenders = _provider.GetAppenders<ConsoleAppender>();
            if (consoleAppenders.Length == 0)
                return;

            lock (_consoleAppenders)
            {
                foreach (ConsoleAppender consoleAppender in consoleAppenders)
                {
                    if (_consoleAppenders.Any(i => i.Appender == consoleAppender))
                        continue;

                    Level original = consoleAppender.Threshold;
                    consoleAppender.Threshold = Level.Off;
                    _consoleAppenders.Add(new(consoleAppender, original));
                }
            }
        }

        public void ResumeConsoleOutput()
        {
            if (_consoleAppenders.Count == 0)
                return;

            lock (_consoleAppenders)
            {
                foreach ((ConsoleAppender Appender, Level Original) in _consoleAppenders)
                    Appender.Threshold = Original;
                _consoleAppenders.Clear();
            }
        }

        public static Log4NetManager LoadInstance(InstantiateArgs args)
        {
            ArgumentNullException.ThrowIfNull(args, nameof(args));

            lock (_slock)
            {
                if (_Instance is not null)
                    throw new InvalidOperationException("试图重复加载单例实例");

                _Instance = new(args.Provider);
                return _Instance;
            }
        }

        private record AppenderLevel(ConsoleAppender Appender, Level Original);

        public class InstantiateArgs : Core.InstantiateArgs
        {
            public InstantiateArgs(ILog4NetProvider provider)
            {
                ArgumentNullException.ThrowIfNull(provider, nameof(provider));

                Provider = provider;
            }

            public ILog4NetProvider Provider { get; }

        }
    }
}
