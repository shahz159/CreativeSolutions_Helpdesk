using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.Web.Utils
{
    public class FileAttribs
    {
        public string FileName { get; set; }
        public string Base64FileData { get; set; }
        public String FileUploadLocation { get; set; }
        public string FileURLLocation { get; set; }
        public string _ext { get; set; }

    }
}