using System;
using System.Collections.Generic;
using System.IO;

namespace WebAPI
{
    public class Log
    {
        public Log()
        {
            LogFile = "log.txt";
        }

        public Log(string logFile)
        {
            LogFile = logFile;
        }

        public string LogFile { get; set; }

        public List<object> messages = new List<object>();

        public void Add(params object[] list)
        {
            foreach (object item in list)
                messages.Add($"\t{item}");
        }

        public void Write()
        {
            using (StreamWriter w = File.AppendText(LogFile))
            {
                w.WriteLine($"\nLog Entry: {DateTime.Now}");

                foreach(object msg in messages)
                    w.WriteLine($"\t{msg}");

                w.WriteLine("-------------------------------");
            }
        }

        public void Write(params object[] messages)
        {
            using (StreamWriter w = File.AppendText(LogFile))
            {
                w.WriteLine($"\nLog Entry: {DateTime.Now}");

                foreach (object msg in messages)
                    w.WriteLine($"\t{msg}");

                w.WriteLine("-------------------------------");
            }
        }
    }
}
