using HelpDesk.Web.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HelpDesk.Web.Utils
{
    public class Common
    {
        public static string[] symbols = new string[] { " ", "!", "\"", "'", "@", "%", "&", "(", ")", "/", "+", "'", "|", "`", "]", "[", "\\", "?", ";", "<", ">", "#", "$" };
        public static int getRandomNumber()
        {
            Random r = new Random();
            return r.Next(1000, 9999);
        }
        public static FileAttribs PrepareFileAttributes(HttpPostedFileBase objFile)
        {
            Stream strm = objFile.InputStream;
            byte[] bytes = StreamToBytes(strm);
            FileAttribs _fileAttr = new FileAttribs();
            _fileAttr.Base64FileData = Convert.ToBase64String(bytes);
            _fileAttr.FileName = objFile.FileName;
            return _fileAttr;
        }
        public static byte[] StreamToBytes(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
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