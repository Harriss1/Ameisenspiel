using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//ggf. noch Read-Only implementieren: https://docs.microsoft.com/de-de/dotnet/api/system.io.file.setaccesscontrol?view=netframework-4.8
namespace Ameisenspiel {
    internal class Log {


        static List<string> messages = new List<string>();
        private static string logFileName = "antAppLog.txt";

        static bool newLog = true;

        enum MessageType {
            Information,
            Message,
            Warning,
            Error
        }

        private string sourceObjectDescription;
        public Log(string sourceObjectDescription) {
            
            this.sourceObjectDescription = sourceObjectDescription;
            if (newLog) {
                this.AddHeader();
                newLog = false;
            }
        }
        public void AddError(string message) {
            String text = GetTime() + "### ERROR [" + sourceObjectDescription + "]:" + message + "###";
            messages.Add(text);
            WriteToLogFile(text);
        }
        public void AddWarning(string message) {
            String text = GetTime() + "WARNING [" + sourceObjectDescription + "]:" + message;
            messages.Add(text);
            WriteToLogFile(text);
        }
        public void Add(string message) {
            String text = GetTime() + "Info [" + sourceObjectDescription + "]:" + message;
            messages.Add(text);
            WriteToLogFile(text);
        }
        private void AddHeader() {
            String text = "\n### Log (" + System.DateTime.Now.ToString() + ") ###\n";
            messages.Add(text);
            WriteToLogFile(text);
        }

        private string GetTime() {
            return "[" + System.DateTime.Now.TimeOfDay.ToString() + "] ";
        }
        private static string GetFile() {
            //Set Directory

            // This will get the current WORKING directory (i.e. \bin\Debug)
            string workingDirectory = Environment.CurrentDirectory;
            // or: Directory.GetCurrentDirectory() gives the same result

            // This will get the current PROJECT bin directory (ie ../bin/)
            string projectDirectoryA = Directory.GetParent(workingDirectory).Parent.FullName;

            // This will get the current PROJECT directory
            string projectDirectoryB = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

            String filepath = projectDirectoryA + "/" + logFileName;

            return filepath;
        }
        public static void WriteArrayToLogFile() {
            string file = GetFile();
            if (File.Exists(file)) {
                using (StreamWriter sw = File.AppendText(file)) {
                    foreach (string message in messages) {
                        sw.WriteLine(message);
                    }
                }
            } 
            else {
                using (StreamWriter sw = File.CreateText(file)) {
                    foreach (string message in messages) {
                        sw.WriteLine(message);
                    }
                }
            }
        }
        private static void WriteToLogFile(String text) {
            string file = GetFile();
            if (File.Exists(file)) {
                using (StreamWriter sw = File.AppendText(file)) {
                    sw.WriteLine(text);
                }
            }
            else {
                using (StreamWriter sw = File.CreateText(file)) {
                    sw.WriteLine(text);
                }
            }
        }
    }
}
