using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace QuanLib.Logging
{
    public class LogManager : ISingleton<LogManager, LogManager.InstantiateArgs>
    {
        private LogManager(string pattern, string logFilePath, Encoding logFileEncoding, Stream xmlConfigStream, bool isSaveHistoricalLogFile)
        {
            ArgumentException.ThrowIfNullOrEmpty(pattern, nameof(pattern));
            ArgumentException.ThrowIfNullOrEmpty(logFilePath, nameof(logFilePath));
            ArgumentNullException.ThrowIfNull(logFileEncoding, nameof(logFileEncoding));
            ArgumentNullException.ThrowIfNull(xmlConfigStream, nameof(xmlConfigStream));

            if (xmlConfigStream.CanSeek && xmlConfigStream.Position != 0)
                xmlConfigStream.Seek(0, SeekOrigin.Begin);

            XmlConfigurator.Configure(xmlConfigStream);
            _repository = (Hierarchy)log4net.LogManager.GetRepository();

            PatternLayout layout = new(pattern);
            layout.ActivateOptions();

            _consoleAppender = new()
            {
                Threshold = Level.All,
                Layout = layout
            };
            _consoleAppender.ActivateOptions();

            _fileAppender = new()
            {
                Threshold = Level.All,
                Layout = layout,
                Encoding = logFileEncoding,
                File = logFilePath,
                LockingModel = new FileAppender.MinimalLock()
            };
            _fileAppender.ActivateOptions();

            Logbuilder = new Logbuilder(this);
            _incrementId = 1;
            _loggers = [];

            Initialize(logFilePath, isSaveHistoricalLogFile);
        }

        private static readonly object _slock = new();

        public static bool IsInstanceLoaded => _Instance is not null;

        public static LogManager Instance => _Instance ?? throw new InvalidOperationException("实例未加载");
        private static LogManager? _Instance;

        private readonly Hierarchy _repository;

        private readonly ConsoleAppender _consoleAppender;

        private readonly RollingFileAppender _fileAppender;

        private int _incrementId;

        private readonly Dictionary<string, LogImpl> _loggers;

        public ILogbuilder Logbuilder { get; }

        public Level ConsoleThreshold
        {
            get => _consoleAppender.Threshold;
            set => _consoleAppender.Threshold = value;
        }

        public Level FileThreshold
        {
            get => _fileAppender.Threshold;
            set => _fileAppender.Threshold = value;
        }

        public static LogManager LoadInstance(InstantiateArgs args)
        {
            ArgumentNullException.ThrowIfNull(args, nameof(args));

            lock (_slock)
            {
                if (_Instance is not null)
                    throw new InvalidOperationException("试图重复加载单例实例");

                _Instance = new(args.Pattern, args.LogFilePath, args.LogFileEncoding, args.XmlConfigStream, args.IsSaveHistoricalLogFile);
                return _Instance;
            }
        }

        public static void Initialize(string logFilePath, bool isSaveHistoricalLogFile)
        {
            ArgumentException.ThrowIfNullOrEmpty(logFilePath, nameof(logFilePath));

            if (isSaveHistoricalLogFile)
                SaveHistoricalLogFile(logFilePath);

            if (File.Exists(logFilePath))
                File.Delete(logFilePath);
        }

        private static void SaveHistoricalLogFile(string logFilePath)
        {
            ArgumentException.ThrowIfNullOrEmpty(logFilePath, nameof(logFilePath));

            logFilePath = Path.GetFullPath(logFilePath);
            FileInfo logFileInfo = new(logFilePath);
            if (!logFileInfo.Exists)
                return;

            string? directory = logFileInfo.DirectoryName;
            if (string.IsNullOrEmpty(directory))
                throw new IOException($"找不到文件“{logFilePath}”所在的目录");

            string? path = null;
            for (int i = 1; i <= 4096; i++)
            {
                string format = Path.Combine(directory, logFileInfo.LastWriteTime.ToString("yyyy-MM-dd") + "-{0}.log.gz");
                path = string.Format(format, i);

                if (!File.Exists(path))
                    break;
            }

            if (string.IsNullOrEmpty(path))
                return;

            using FileStream fileStream = new(path, FileMode.Create);
            using GZipStream gZipStream = new(fileStream, CompressionMode.Compress);
            byte[] bytes = File.ReadAllBytes(logFilePath);
            gZipStream.Write(bytes, 0, bytes.Length);
        }

        public static MemoryStream CreateDefaultXmlConfigStream()
        {
            XmlDocument xmlDocument = new();
            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDocument.AppendChild(xmlDeclaration);
            XmlElement xmlElement = xmlDocument.CreateElement("log4net");
            xmlDocument.AppendChild(xmlElement);

            MemoryStream memoryStream = new();
            xmlDocument.Save(memoryStream);

            return memoryStream;
        }

        public LogImpl GetLogger()
        {
            StackFrame frame = new(1);
            MethodBase? method = frame.GetMethod();
            Type? type = method?.DeclaringType;
            if (type is null)
                return GetLogger(GetNextLoggerName());
            return GetLogger(type);
        }

        public LogImpl GetLogger(Type type)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            if (type.FullName is null)
                return GetLogger(GetNextLoggerName());

            return GetLogger(type.FullName);
        }

        public LogImpl GetLogger(string name)
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

            lock (_loggers)
            {
                if (_loggers.TryGetValue(name, out var logImpl))
                    return logImpl;

                Logger logger = _repository.LoggerFactory.CreateLogger(_repository, name);
                logger.Hierarchy = _repository;
                logger.Parent = _repository.Root;
                logger.Level = Level.All;
                logger.Additivity = false;
                logger.AddAppender(_consoleAppender);
                logger.AddAppender(_fileAppender);
                logImpl = new(logger);
                _loggers.Add(name, logImpl);
                return logImpl;
            }
        }

        public void EnableConsoleOutput()
        {
            ConsoleThreshold = Level.All;
        }

        public void DisableConsoleOutput()
        {
            ConsoleThreshold = Level.Off;
        }

        private string GetNextLoggerName()
        {
            return "Logger" + GetNextIncrementId();
        }

        private int GetNextIncrementId()
        {
            Interlocked.Increment(ref _incrementId);
            return _incrementId;
        }

        public class InstantiateArgs : Core.InstantiateArgs
        {
            public InstantiateArgs(string pattern, string logFilePath, Encoding logFileEncoding, Stream xmlConfigStream, bool isSaveHistoricalLogFile)
            {
                ArgumentException.ThrowIfNullOrEmpty(pattern, nameof(pattern));
                ArgumentException.ThrowIfNullOrEmpty(logFilePath, nameof(logFilePath));
                ArgumentNullException.ThrowIfNull(logFileEncoding, nameof(logFileEncoding));
                ArgumentNullException.ThrowIfNull(xmlConfigStream, nameof(xmlConfigStream));

                Pattern = pattern;
                LogFilePath = logFilePath;
                LogFileEncoding = logFileEncoding;
                XmlConfigStream = xmlConfigStream;
                IsSaveHistoricalLogFile = isSaveHistoricalLogFile;
            }

            public string Pattern { get; }

            public string LogFilePath { get; }

            public Encoding LogFileEncoding { get; }

            public Stream XmlConfigStream { get; }

            public bool IsSaveHistoricalLogFile { get; }
        }
    }
}
