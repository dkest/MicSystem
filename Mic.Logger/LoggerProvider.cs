using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mic.Logger
{
    public static class LoggerProvider
    {
        static LoggerProvider()
        {
            Logger = new NLogLogger(AppDomain.CurrentDomain.FriendlyName);
        }

        public static ILogger Logger { get; private set; }

        public static void ConfigFileTarget(string mask, string value)
        {
            NLog.Config.LoggingConfiguration config = NLog.LogManager.Configuration;
            foreach (var rule in config.LoggingRules)
            {
                NLog.Targets.FileTarget file = rule.Targets[0] as NLog.Targets.FileTarget;
                if (file != null) file.FileName = NLog.Layouts.Layout.FromString(((NLog.Layouts.SimpleLayout)file.FileName).Text.Replace(mask, value));
            }
            NLog.LogManager.ReconfigExistingLoggers();
        }
    }
}
