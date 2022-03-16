using HelpDesk.API.Bussiness;
using HelpDesk.API.DTO_s;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace HelpDesk.API.Controllers
{
    public class LoginAPIController : ApiController
    {
        private readonly ILoginPageService service;
        public LoginAPIController(LoginPageService _service)
        {
            service = _service;
        }
        public IHttpActionResult NewLogin(LoginPageDTO obj)
        {
            var result = service.getLogin(obj);
            string msg = "";
            bool val = false;
            JArray JLoginDetails = JArray.Parse("[]");
            JArray JMenuDetails = JArray.Parse("[]");
            JArray JApprovalMenuCountDetails = JArray.Parse("[]");
            if (result != null)
            {
                if (!string.IsNullOrEmpty(result.datasetxml))
                {
                    var document = new XmlDocument();
                    document.LoadXml(result.datasetxml);
                    DataSet ds = new DataSet();
                    ds.ReadXml(new XmlNodeReader(document));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JLoginDetails = strjarry;
                            foreach (JObject item in strjarry)
                            {
                                string sas = item.SelectToken("Status").ToString();
                                if (sas == "1")
                                    val = true;
                                msg = val == true ? "Login Successful." : "Invalid Login Credentials";
                            }
                        }
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[1]);
                            var strarray = JArray.Parse(str);
                            if (!string.IsNullOrEmpty(str))
                                JMenuDetails = strarray;
                        }
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[2]);
                            var strarray = JArray.Parse(str);
                            if (!string.IsNullOrEmpty(str))
                                JApprovalMenuCountDetails = strarray;
                        }
                    }
                }
            }


            JObject res1 = new JObject(new JProperty("Login", JLoginDetails),
                         new JProperty("Menus", JMenuDetails),
                          new JProperty("ApprovalCounts", JApprovalMenuCountDetails)
                         );

            //JObject res12 = new JObject(new JProperty("UserId", result.Id),
            //                (new JProperty("FullName", result.FullName)),
            //                (new JProperty("Phone", result.Phone)),
            //                    (new JProperty("Email", result.Email)));

            JObject res = new JObject(new JProperty("Status", val),
                                (new JProperty("Message", msg)),
                                (new JProperty("Data", res1)));
            //obj.jobject = res;
            //var result1 = res;
            return Ok(res);
        }
    }
}
