using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ActiveDirectorySearch
{
   
    public static class Logger
    {
        public static bool IsDebugLevelActive
        {
            get
            {
                return log.IsDebugEnabled;
            }
        }

        private static NLog.Logger log = LogManager.GetLogger("AzureADUsers");

        public static void Log(string message, [CallerMemberName]string memberName = "", [CallerFilePath]string filePath = "")
        {
            log.Debug($"Class : {Path.GetFileNameWithoutExtension(filePath)}  Method : {memberName} --- {message}");
        }

        public static void Debug(string message, [CallerMemberName]string memberName = "", [CallerFilePath]string filePath = "")
        {
            Log(message, memberName, filePath);
        }

        public static void Info(string message, [CallerMemberName]string memberName = "", [CallerFilePath]string filePath = "")
        {
            log.Info($"Class : {Path.GetFileNameWithoutExtension(filePath)}  Method : {memberName} --- {message}");
        }

        public static void Warn(string message, [CallerMemberName]string memberName = "", [CallerFilePath]string filePath = "")
        {
            log.Warn($"Class : {Path.GetFileNameWithoutExtension(filePath)}  Method : {memberName} --- {message}");
        }

        public static void Error(string message, [CallerMemberName]string memberName = "", [CallerFilePath]string filePath = "")
        {
            log.Error($"Class : {Path.GetFileNameWithoutExtension(filePath)}  Method : {memberName} --- {message}");
        }

        public static void Fatal(string message, [CallerMemberName]string memberName = "", [CallerFilePath]string filePath = "")
        {
            log.Fatal($"Class : {Path.GetFileNameWithoutExtension(filePath)}  Method : {memberName} --- {message}");
        }
    }
}
