using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mic.Logger
{
    public class FileLoggerHelper
    {
        private static readonly string sErrorLogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("Log{0}trace-error.log", Path.DirectorySeparatorChar.ToString()));
        private static readonly string sInfoLogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("Log{0}trace-info.log", Path.DirectorySeparatorChar.ToString()));

        private static readonly int sLogLimitSize = 6 * 1024 * 1024; //byte

        private static StreamWriter writer;
        private static FileStream fileStream = null;
        private static bool isWriteLog;

        static FileLoggerHelper()
        {
            isWriteLog = true;
        }
        public static void WriteErrorLog(string message)
        {
            WriteLog(message, sErrorLogFile);
        }
        public static void WriteInfoLog(string message)
        {
            WriteLog(message, sInfoLogFile);
        }
        private static void WriteLog(string message, string filePath)
        {
            if (!isWriteLog)
            {
                return;
            }
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo == null)
                {
                    return;
                }
                //文件目录不存在
                if (!fileInfo.Directory.Exists)
                {
                    Directory.CreateDirectory(fileInfo.Directory.FullName);
                }
                if (!fileInfo.Exists)
                {
                    fileStream = fileInfo.Create();
                }
                else
                {
                    if (fileInfo.Length > sLogLimitSize)
                    {
                        string destinationFile = string.Format("{0}{1}{2}",
                                fileInfo.FullName.TrimEnd(fileInfo.Extension.ToCharArray()), DateTime.Now.ToString("yyyyMMdd"), fileInfo.Extension);
                        fileInfo.CopyTo(destinationFile, true);
                        fileStream = fileInfo.Open(FileMode.Truncate, FileAccess.Write);
                    }
                    else
                    {
                        fileStream = fileInfo.Open(FileMode.Append, FileAccess.Write);
                    }
                }
                writer = new StreamWriter(fileStream);
                writer.WriteLineAsync(string.Format("{0}:{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message));
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
        }
    }
}