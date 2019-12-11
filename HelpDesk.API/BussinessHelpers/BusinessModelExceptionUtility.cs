using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HelpDesk.API.BussinessHelpers
{
    public sealed class BusinessModelExceptionUtility
    {
        // All methods are static, so this can be private 
        private BusinessModelExceptionUtility()
        { }

        // Log an Exception 
        public static void LogException(string exc, string source)
        {
            // Include enterprise logic for logging exceptions 
            // Get the absolute path to the log file 
            string logFile = "~/App_Data/BusinessErrorLog.txt";
            logFile = HttpContext.Current.Server.MapPath(logFile);

            // Open the log file for append and write the log
            StreamWriter sw = new StreamWriter(logFile, true);
            sw.WriteLine("********** {0} **********", DateTime.Now);
            if (exc != null)
            {
                sw.Write("Inner Exception: ");
                sw.WriteLine(exc);
                sw.Write("Inner Source: ");
                sw.WriteLine(exc);
            }
            sw.Close();
        }

        // Notify System Operators about an exception 
        public static void NotifySystemOperators(Exception exc)
        {
            // Include code for notifying IT system operators
        }
    }
}