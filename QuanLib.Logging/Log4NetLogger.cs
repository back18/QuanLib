﻿using log4net;
using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Logging
{
    public class Log4NetLogger(ILog log) : ILogger
    {
        private readonly ILog _log = log;

        public void Debug(string message) => _log.Debug(message);

        public void Debug(string message, Exception exception) => _log.Debug(message, exception);

        public void Info(string message) => _log.Info(message);

        public void Info(string message, Exception exception) => _log.Info(message, exception);

        public void Warn(string message) => _log.Warn(message);

        public void Warn(string message, Exception exception) => _log.Warn(message, exception);

        public void Error(string message) => _log.Error(message);

        public void Error(string message, Exception exception) => _log.Error(message, exception);

        public void Fatal(string message) => _log.Fatal(message);

        public void Fatal(string message, Exception exception) => _log.Fatal(message, exception);
    }
}
