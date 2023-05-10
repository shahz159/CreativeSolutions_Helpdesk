using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;

using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace AHCHelpDeskEmailNotificationUtility
{
    public partial class Service1 : ServiceBase
    {
        private System.Timers.Timer Schedular;
        public Service1()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStop()
        {
            Schedular.Enabled = false;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Schedular = new System.Timers.Timer();
                this.Schedular.Interval = 24 * 60 * 60 * 1000;  //6 *60*60*1000; //1000 every 1 sec  
                /**
                 * Above time should be around 6 hours
                 * Interval = 6*60*60*1000
                 * i.e., dr.Cafe will initiate subscriptions orders every 6 hours due to following reasons:
                 * 1. If payment failed for the first time, dr.Cafe will ward customer to either recharge his card or change his payment card
                 * 2. If payment was failed earlier and custer has taken certain action like change of his card, dr.Cafe will be having sufficient time to restart transaction and continue with order.
                 * 
                 * like this dr.Cafe provides customer around 4 chances to carry-on with his order.
                 * */
                this.Schedular.Elapsed += new System.Timers.ElapsedEventHandler(this.Schedular_Tick);
                Schedular.Enabled = true;
                //Library.WriteErrorLog("Push Notification window service started");
            }
            catch (Exception ex)
            {
                // Log the exception.
                EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
        }

        private void Schedular_Tick(object sender, ElapsedEventArgs e)
        {
            Schedular.Stop();
            PPMEmailNotificationService();
            //Library.WriteErrorLog("Timer ticked and some job has been done successfully");
            Schedular.Start();
        }

        #region Actual business logic
        private async Task<string> PPMEmailNotificationService()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage();
                CommonHeader.setHeaders(client);
                AssetDTO obj = new AssetDTO();
                HttpResponseMessage CheckoutResponse = new HttpResponseMessage();
                CheckoutResponse = await client.PostAsJsonAsync("api/AssetAPI/NewSendEmailNotificationOfPPM", obj);
                if (CheckoutResponse.IsSuccessStatusCode)
                {
                    bool status = false;
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    var check = JsonConvert.DeserializeObject<AssetDTO>(responseData);
                    string msg = check.message;
                    if (msg == "1")
                        status = true;
                    else if (msg == "2")
                        status = false;
                    else
                        status = false;
                }
            }
            return String.Empty;
        }
        #endregion
    }
    public class CommonHeader
    {
        public static void setHeaders(HttpClient client)
        {
            client.BaseAddress = new Uri("http://208.109.10.196/ahchelpdeskapi/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
    public class AssetDTO{
        public string message { get; set; }
        
    }
}
