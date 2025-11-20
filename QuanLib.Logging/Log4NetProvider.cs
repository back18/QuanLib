using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository;
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
    public class Log4NetProvider : ILog4NetProvider
    {
        public Log4NetProvider(string logFilePath, Stream xmlConfigStream, bool enableBackup)
        {
            ArgumentException.ThrowIfNullOrEmpty(logFilePath, nameof(logFilePath));
            ArgumentNullException.ThrowIfNull(xmlConfigStream, nameof(xmlConfigStream));

            if (enableBackup && File.Exists(logFilePath))
                BackupHelper.Backup(logFilePath, "yyyy-MM-dd");

            Assembly assembly = Assembly.GetExecutingAssembly();
            _repository = LogManager.GetRepository(assembly);
            XmlConfigurator.Configure(_repository, xmlConfigStream);

            _loggers = [];
            _incrementId = 1;
        }

        private readonly ILoggerRepository _repository;

        private readonly Dictionary<string, Log4NetLogger> _loggers;

        private int _incrementId;

        public ILoggerRepository GetRepository()
        {
            return _repository;
        }

        public TAppender[] GetAppenders<TAppender>() where TAppender : IAppender
        {
            List<TAppender> appenders = [];
            foreach (IAppender appender in _repository.GetAppenders())
            {
                if (appender is TAppender tappender)
                    appenders.Add(tappender);
            }

            return appenders.ToArray();
        }

        public ILogger GetLogger()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            StackTrace stackTrace = new();
            StackFrame[] stackFrames = stackTrace.GetFrames();

            foreach (StackFrame stackFrame in stackFrames)
            {
                MethodBase? method = stackFrame.GetMethod();
                Type? type = method?.DeclaringType;

                if (type is not null && type.Assembly != assembly)
                    return GetLogger(type);
            }

            return GetLogger(GetUnnamedLogger());
        }

        public ILogger GetLogger(Type type)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            if (type.FullName is null)
                return GetLogger(type.Name);

            return GetLogger(type.FullName);
        }

        public ILogger GetLogger(string name)
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

            if (_loggers.TryGetValue(name, out var logger))
                return logger;

            lock (_loggers)
            {
                if (_loggers.TryGetValue(name, out logger))
                    return logger;

                Assembly assembly = Assembly.GetExecutingAssembly();
                ILog log = LogManager.GetLogger(assembly, name);
                Log4NetLogger log4NetLogger = new(log);
                _loggers.Add(name, log4NetLogger);
                return log4NetLogger;
            }
        }

        public ILog GetLog4NetLogger()
        {
            return ((Log4NetLogger)GetLogger()).GetLog4NetLogger();
        }

        public ILog GetLog4NetLogger(Type type)
        {
            return ((Log4NetLogger)GetLogger(type)).GetLog4NetLogger();
        }

        public ILog GetLog4NetLogger(string name)
        {
            return ((Log4NetLogger)GetLogger(name)).GetLog4NetLogger();
        }

        private string GetUnnamedLogger()
        {
            return "UnnamedLogger-" + GetNextIncrementId();
        }

        private int GetNextIncrementId()
        {
            Interlocked.Increment(ref _incrementId);
            return _incrementId;
        }
    }
}
