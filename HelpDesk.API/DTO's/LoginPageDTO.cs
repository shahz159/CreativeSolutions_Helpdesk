using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API.DTO_s
{
    public class LoginPageDTO
    {
        public JObject jobject { get; set; }
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public long StatusCode { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
        public string Result { get; set; }
        public string datasetxml { get; set; }
    }
}