using System;

namespace Mic.Logger
{
    public sealed class NLogLogger : ILogger
    {
        private readonly NLog.Logger logger;

        public NLogLogger(string name)
        {
            logger = NLog.LogManager.GetLogger(name);
        }

        public void Trace(string message, params object[] args)
        {
            logger.Trace(message, args);
        }

        public void Info(string message, params object[] args)
        {
            logger.Info(message, args);
        }

        public void Warn(string message, params object[] args)
        {
            logger.Warn(message, args);
        }

        public void Warn(Exception exception, string message, params object[] args)
        {
            logger.WarnException(string.Format(message, args), exception);
        }

        public void Error(string message, params object[] args)
        {
            logger.Error(message, args);
        }

        public void Error(Exception exception, string message, params object[] args)
        {
            logger.ErrorException(string.Format(message, args), exception);
        }

        public void Fatal(string message, params object[] args)
        {
            logger.Fatal(message, args);
        }

        public void Fatal(Exception exception, string message, params object[] args)
        {
            logger.FatalException(string.Format(message, args), exception);
        }
    }
}

