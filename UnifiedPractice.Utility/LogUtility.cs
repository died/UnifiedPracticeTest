using System;
using System.Data.Common;
using System.IO;
using System.Text;

namespace UnifiedPractice.Utility
{
    public class LogUtility
    {
        private static readonly string DirectoryName = AppDomain.CurrentDomain.BaseDirectory + @"\log";
        private static readonly object LockFile = new object();
        private static readonly object LockPrint = new object();

        public static void Init()
        {
            if (!Directory.Exists(DirectoryName))
                Directory.CreateDirectory(DirectoryName);
            WriteFile("LogUtility Init.");
        }

        public static void WriteFile(String value, DebugLevel level)
        {
            var logtxt = string.Format("{0:yyyy/MM/dd HH:mm:ss}[{2}]{1}", DateTime.Now, value, level);
            WriteLog(logtxt);
        }

        /// <summary>
        /// console print log on screen
        /// </summary>
        /// <param name="value"></param>
        /// <param name="level"></param>
        public static void PrintLog(String value, DebugLevel level)
        {
            lock (LockPrint)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"{DateTime.Now:HH:mm:ss} ");
                switch (level)
                {
                    case DebugLevel.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case DebugLevel.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    default:
                        Console.ResetColor();
                        break;
                }
                Console.WriteLine(value);
            }
        }

        public static void Log(String value, DebugLevel level)
        {
            PrintLog(value, level);
            WriteFile(value, level);
        }

        public static void Log(String value)
        {
            var level = DebugLevel.Info;
            PrintLog(value, level);
            WriteFile(value, level);
        }

        public static void WriteFile(String value)
        {
            WriteFile(value, DebugLevel.Info);
        }

        /// <summary> Using FileStream to write log
        /// </summary>
        /// <param name="logtxt"></param>
        private static void WriteLog(string logtxt)
        {
            string filePath = $"{DirectoryName}\\{DateTime.Now:yyyy_MM_dd}.log";
            using (var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (var log = new StreamWriter(fs, Encoding.UTF8))
                {
                    lock (LockFile)
                    {
                        log.WriteLine(logtxt);
                    }
                }
            }
        }

        /// <summary>Log Error to File 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ex"></param>
        public static void LogError(string value, Exception ex)
        {
            var msg = value + " Error:" + ex.Message;
            WriteFile(FetchInnerEx(msg, ex), DebugLevel.Error);
        }

        public static void LogError(Type type, string name, Exception ex)
        {
            var msg = $"[{type.Name}.{name}] Error: {ex.Message} Stack:{ex.StackTrace} Source:{ex.Source}";
            PrintLog(msg, DebugLevel.Error);
            WriteFile(FetchInnerEx(msg, ex), DebugLevel.Error);
        }

        public static void LogError(Type type, string name, DbException ex)
        {
            var msg = $"[{type.Name}.{name}] Error: {ex.Message} Stack:{ex.StackTrace} Source:{ex.Source}";
            PrintLog(msg, DebugLevel.Error);
            WriteFile(FetchInnerEx(msg, ex), DebugLevel.Error);
        }

        /// <summary>Get Inner Error Message 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static string FetchInnerEx(string msg, Exception ex)
        {
            while (true)
            {
                if (ex.InnerException == null) return msg;
                msg += Environment.NewLine + "Inner Error:" + ex.InnerException.Message;
                ex = ex.InnerException;
            }
        }
    }

    /// <summary>debug log level
    /// </summary>
    public enum DebugLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }
}
