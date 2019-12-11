using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace HelpDesk.Web.Handlers
{
    public class CommonHeader
    {
        public static void setHeaders(HttpClient client)
        {
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiurl"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}