using HelpDesk.API.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HelpDesk.API.Utils
{
    public class Common
    {
        public static string[] symbols = new string[] { " ", "!", "\"", "'", "@", "%", "&", "(", ")", "/", "+", "'", "|", "`", "]", "[", "\\", "?", ";", "<", ">", "#", "$" };
        public static int getRandomNumber()
        {
            Random r = new Random();
            return r.Next(1000, 9999);
        }

        public static FileAttribs UploadFile(FileAttribs obj)
        {
            
            String uploadLocation = System.Configuration.ConfigurationManager.AppSettings["UploadLocation"];
            
            obj.FileUploadLocation = obj.FileUploadLocation;
            obj.FileName = SuggestValidFileName(obj);
            string saveFileName = obj.FileName;
            obj._ext = Path.GetExtension(obj.FileName);

            obj.FileURLLocation = saveFileName;
            string localFile = Path.Combine(HttpContext.Current.Server.MapPath(obj.FileUploadLocation), obj.FileName);
            File.WriteAllBytes(localFile, Convert.FromBase64String(obj.Base64FileData));
            return obj;
        }
        
        public static bool doesFileExist(string str, string uploadLocation)
        {
            //uploadLocation = System.Configuration.ConfigurationManager.AppSettings["UploadLocation"];
            string path = uploadLocation + str;
            if (File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string SuggestValidFileName(FileAttribs obj)
        {
            bool fileExist = false;
            //string newFName = fName.Replace(" ","-");
            obj.FileName = ReplaceSymbols(obj.FileName);
            string newFName = obj.FileName;
            do
            {
                fileExist = doesFileExist(newFName, obj.FileUploadLocation);
                if (fileExist)
                {
                    int rNumber = 0;
                    Random r = new Random();
                    rNumber = r.Next(1000, 9999);
                    newFName = rNumber.ToString() + obj.FileName;
                }
            } while (fileExist);
            return newFName;
        }
        public static string ReplaceSymbols(String str)
        {
            var tempstr = str;
            foreach (var syb in symbols)
            {
                tempstr = tempstr.Replace(syb, "-");
            }
            return tempstr;
        }
        public static string generateInvoiceNo(long branchid, Int64 userId, String PaymentReg)
        {
            int rno = getRandomNumber();
            TimeZoneInfo AST = TimeZoneInfo.FindSystemTimeZoneById("Arabic Standard Time");
            DateTime astTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, AST);
            string dt = astTime.ToString("ddMMyyhms");
            string invoiceNo = PaymentReg.ToUpper() + "-" + branchid + "-" + userId + "-" + dt + "-" + rno;
            return invoiceNo;
        }
    }
}