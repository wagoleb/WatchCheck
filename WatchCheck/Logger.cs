using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WatchCheck
{
    class Logger
    {
        private string path = Directory.GetCurrentDirectory().ToString();

        public Logger()
        {
            this.GetFullLogPath = $"{path}\\{DateTime.Now.ToString("yyyyMMdd_HHmmss")}-fileLog.log";
        }

        public string GetFullLogPath { get; }

        public void LogMessage(string message)
        {
            string newMessage = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")} - {message}{Environment.NewLine}";
            File.AppendAllText(this.GetFullLogPath, newMessage);
        }
    }
}
