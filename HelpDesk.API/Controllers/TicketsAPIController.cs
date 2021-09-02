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
using System.Web.Http.Description;
using System.Xml;

namespace HelpDesk.API.Controllers
{
    public class TicketsAPIController : ApiController
    {
        private readonly ITicketService service;
        public TicketsAPIController(TicketService _service)
        {
            service = _service;
        }
        /// <summary>
        /// New Ticket Request
        /// </summary>
        /// <param name="obj"></param>NewSystemUserTickets
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewInsertTicketRequest(TicketDTO obj)
        {
            obj.IsMobile = false;
            var result = service.InsertTicketRequest(obj);
            return Ok(result);
        }

        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewInsertTicketRequestM(TicketDTO obj)
        {
            //            "TicketDocuments":{
            //                "FileName":"1953shirtsb3.jpeg",
            //"Base64FileData":"/9j/4AAQSkZJRgABAQAAAQABAAD/2wBDABALDA4MChAODQ4SERATGCgaGBYWGDEjJR0oOjM9PDkzODdASFxOQERXRTc4UG1RV19iZ2hnPk1xeXBkeFxlZ2P/2wBDARESEhgVGC8aG9KNuTjvQAsnTHvUdSSc4PqT/OmD3pAFJS0lMBaBRSigDZ8J2ou/EFojDKq28/8AARn+letV5v8AD2Pfrrv/AHIWP6gV6TUPcYUUUUgCiiigAooooAKKKKACiiigAooooA//2Q==",
            //"FileUploadLocation":"/ProfilePicture/",
            //"FileURLLocation":null
            //}

            if (obj.TicketDocuments != null)
            {
                obj.TicketDocuments = Utils.Common.UploadFile(obj.TicketDocuments);
                string path = obj.TicketDocuments.FileURLLocation;

                var xmldoc_docs = new XmlDocument();
                var parentelemeng_docs = xmldoc_docs.CreateElement("MultiDocuments");
                var parent_docs = xmldoc_docs.CreateElement("MultiDocument");

                var parentelement = xmldoc_docs.CreateElement("Row");
                var filepath_xml = xmldoc_docs.CreateElement("filepath");
                var ContentType_xml = xmldoc_docs.CreateElement("ContentType");

                filepath_xml.InnerText = path;
                ContentType_xml.InnerText = obj.TicketDocuments._ext;

                parentelement.AppendChild(filepath_xml);
                parentelement.AppendChild(ContentType_xml);
                //parentelement.AppendChild(UniqueId_xml);

                parentelemeng_docs.AppendChild(parent_docs);
                parent_docs.AppendChild(parentelement);

                obj.multipledocuments_xml = parentelemeng_docs.InnerXml;
                obj.Url = "";
                obj.ContentType = "";
            }

            obj.IsMobile = true;
            var result = service.InsertTicketRequest(obj);

            string msg = "";
            bool val = false;

            if (result.message != "0")
            {
                val = true;
            }
            //JObject res1 = new JObject(new JProperty("SystemManagerId", result.SystemManagerId.ToString())
            //            );
            msg = val == true ? "Ticket created successfully." : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
        }
        /// <summary>
        /// Get Under Approval Tickets
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewGetUnderApprovalTickets(TicketDTO obj)
        {
            var result = service.GetUnderApprovalTickets(obj);
            return Ok(result);

        }

        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewGetUnderApprovalTicketsM(TicketDTO obj)
        {
            var result = service.GetUnderApprovalTickets(obj);
            string msg = "";
            bool val = true;
            JArray JUnderApprovalTickets = JArray.Parse("[]");
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
                                JUnderApprovalTickets = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JUnderApprovalTickets = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("UnderApprovalTickets", JUnderApprovalTickets)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }

        /// <summary>
        /// Update Ticket Status
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewUpdateTicketStatus(TicketDTO obj)
        {
            var result = service.UpdateTicketStatus(obj);
            return Ok(result);
        }

        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewUpdateTicketStatusM(TicketDTO obj)
        {
            var result = service.UpdateTicketStatus(obj);

            string msg = "";
            bool val = false;

            if (result.message != "0")
            {
                val = true;
            }
            //JObject res1 = new JObject(new JProperty("SystemManagerId", result.SystemManagerId.ToString())
            //            );
            msg = val == true ? "Ticket Status Updated Successfully." : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
        }
        /// <summary>
        /// Ticket transfer
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewTransferTicket(TicketDTO obj)
        {
            var result = service.TicketTransfer(obj);
            return Ok(result);
        }

        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewTransferTicketM(TicketDTO obj)
        {
            var result = service.TicketTransfer(obj);
            string msg = "";
            bool val = false;

            if (result.message != "0")
                val = true;
            msg = val == true ? "Transfer Successfully." : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
        }

        /// <summary>
        /// Add response time
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewResponseTime(TicketDTO obj)
        {
            var result = service.AddResponseTime(obj);
            return Ok(result);
        }


        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewResponseTimeM(TicketDTO obj)
        {
            var result = service.AddResponseTime(obj);
            string msg = "";
            bool val = false;

            if (result.message != "0")
                val = true;
            msg = val == true ? "Respomse Time Added Successfully." : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
        }


        /// <summary>
        /// Get System User Products
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewSystemUserProducts(TicketDTO obj)
        {
            var result = service.GetSystemUserProducts(obj);
            return Ok(result);
        }
        /// <summary>
        /// For Mobile
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewSystemUserProductsM(TicketDTO obj)
        {
            var result = service.GetSystemUserProducts(obj);
            string msg = "";
            bool val = true;
            JArray JProductDetails = JArray.Parse("[]");
            JArray JAccountDetails = JArray.Parse("[]");
            JArray JCompanyDetails = JArray.Parse("[]");
            JArray JAssetModelDetails = JArray.Parse("[]");
            JArray JReportDetails = JArray.Parse("[]");
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
                                JProductDetails = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JProductDetails = strjarry;
                        }
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[1]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JAccountDetails = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[1]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JAccountDetails = strjarry;
                        }
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[2]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JCompanyDetails = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[2]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JCompanyDetails = strjarry;
                        }
                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[3]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JAssetModelDetails = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[3]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JAssetModelDetails = strjarry;
                        }
                        if (ds.Tables[4].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[4]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JReportDetails = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[4]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JReportDetails = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("ProductList", JProductDetails),
                        new JProperty("AccountList", JAccountDetails),
                        new JProperty("CompanyList", JCompanyDetails)
                        , new JProperty("AssetModelDetails", JAssetModelDetails), new JProperty("ReportList", JReportDetails)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }

        /// <summary>
        /// Get System User Models
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewSystemUserModels(TicketDTO obj)
        {
            var result = service.GetSystemUserModels(obj);
            return Ok(result);
        }
        /// <summary>
        /// get  accounts by company id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewAccountsByCompany(TicketDTO obj)
        {
            var result = service.GetAccounts(obj);
            return Ok(result);
        }
        /// <summary>
        /// get products by company id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewproductsByCompany(TicketDTO obj)
        {
            var result = service.GetProducts(obj);
            return Ok(result);
        }
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewproductsByCompanyM(TicketDTO obj)
        {
            var result = service.GetProducts(obj);

            string msg = "";
            bool val = true;
            JArray JProductandModelDetails = JArray.Parse("[]");
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
                                JProductandModelDetails = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JProductandModelDetails = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("ProductandModelList", JProductandModelDetails)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }

        /// <summary>
        /// Get System User Ticktes
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewSystemUserTickets(TicketDTO obj)
        {
            var result = service.GetSystemUserTickets(obj);
            return Ok(result);
        }

        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewSystemUserTicketsM(TicketDTO obj)
        {
            var result = service.GetSystemUserTicketsMobile(obj);

            string msg = "";
            bool val = true;
            JArray JEngineerList = JArray.Parse("[]");
            JArray JReportList = JArray.Parse("[]");
            JArray JStatusList = JArray.Parse("[]");
            JArray JTicketList = JArray.Parse("[]");
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
                                JEngineerList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JEngineerList = strjarry;
                        }

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[1]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JReportList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[1]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JReportList = strjarry;
                        }
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[2]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JStatusList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[2]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JStatusList = strjarry;
                        }

                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[3]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JTicketList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[3]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JTicketList = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("TicketList", JTicketList),
                new JProperty("EngineerList", JEngineerList),
                new JProperty("ReportList", JReportList),
                new JProperty("StatusList", JStatusList)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewRejectedTickets(TicketDTO obj)
        {
            var result = service.GetRejectedTickets(obj);
            return Ok(result);
        }

        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewRejectedTicketsM(TicketDTO obj)
        {
            var result = service.GetRejectedTickets(obj);

            string msg = "";
            bool val = true;
            JArray JRejectedTicketList = JArray.Parse("[]");
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
                                JRejectedTicketList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JRejectedTicketList = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("RejectedTicketList", JRejectedTicketList)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }
        /// <summary>
        /// Get Service Engineer(user) tickets by id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewServiceEngineerTickets(TicketDTO obj)
        {
            var result = service.GetServiceEngineerTickets(obj);
            return Ok(result);
        }
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewServiceEngineerTicketsM(TicketDTO obj)
        {
            var result = service.GetServiceEngineerTickets(obj);

            string msg = "";
            bool val = true;
            JArray JTicketList = JArray.Parse("[]");
            JArray JTotalRecords = JArray.Parse("[]");

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
                                JTicketList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JTicketList = strjarry;
                        }

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[1]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JTotalRecords = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[1]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JTotalRecords = strjarry;
                        }


                    }
                }
            }
            JObject res1 = new JObject(new JProperty("TicketList", JTicketList),
                        new JProperty("TotalRecords", JTotalRecords)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }
        /// <summary>
        /// get dashboard count by user id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewDashBoardCount(TicketDTO obj)
        {
            var result = service.GetDashboardCount(obj);
            return Ok(result);
        }

        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewDashBoardCountM(TicketDTO obj)
        {
            var result = service.GetDashboardCount(obj);

            string msg = "";
            bool val = true;
            JArray JDashboardCount = JArray.Parse("[]");

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

                            foreach (JObject item in strjarry)
                            {
                                var pv = item.SelectToken("NewTicketsJson");
                                var pvj = JArray.Parse(pv.ToString());
                                item.Add(new JProperty("JNewTicketsJson", pvj));

                                var inprogress = item.SelectToken("InProgressTicketsJson");
                                var inprogressj = JArray.Parse(inprogress.ToString());
                                item.Add(new JProperty("JInProgressTicketsJson", inprogressj));

                                var resolvedjson = item.SelectToken("ResolvedTicketsJson");
                                var resolvedjsonj = JArray.Parse(resolvedjson.ToString());
                                item.Add(new JProperty("JResolvedTicketsJson", resolvedjsonj));

                                var CountJson = item.SelectToken("CountsJson");
                                var CountJsonj = JArray.Parse(CountJson.ToString());
                                item.Add(new JProperty("JCountsJson", CountJsonj));

                                var ServiceEngineerJson = item.SelectToken("ServiceEngineerJson");
                                var ServiceEngineerJsonj = JArray.Parse(ServiceEngineerJson.ToString());
                                item.Add(new JProperty("JServiceEngineerJson", ServiceEngineerJsonj));
                                
                            }

                            if (!string.IsNullOrEmpty(str))
                                JDashboardCount = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JDashboardCount = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("DashboardCount", JDashboardCount)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }
        /// <summary>
        /// Get System Manager UserId 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewSystemManagerId(TicketDTO obj)
        {
            var result = service.GetSystemManagerId(obj);
            return Ok(result);
        }

        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewSystemManagerIdM(TicketDTO obj)
        {
            var result = service.GetSystemManagerId(obj);

            string msg = "";
            bool val = false;

            if (result.SystemManagerId != 0)
            {
                val = true;
            }
            JObject res1 = new JObject(new JProperty("SystemManagerId", result.SystemManagerId.ToString())
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }

        /// <summary>
        /// get filters of service engineer tickets
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewServiceEngineerTicketsFileters(TicketDTO obj)
        {
            var result = service.GetServiceEngineerTicketsFiletrs(obj);
            return Ok(result);
        }

        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewServiceEngineerTicketsFiletersM(TicketDTO obj)
        {
            var result = service.GetServiceEngineerTicketsFiletrs(obj);
            string msg = "";
            bool val = true;
            JArray JServiceEngineerDetails = JArray.Parse("[]");
            JArray JAccountDetails = JArray.Parse("[]");
            JArray JStatusDetails = JArray.Parse("[]");
            JArray JReportTypeDetails = JArray.Parse("[]");

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
                                JAccountDetails = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JAccountDetails = strjarry;
                        }

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[1]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JServiceEngineerDetails = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[1]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JServiceEngineerDetails = strjarry;
                        }

                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[2]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JStatusDetails = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[2]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JStatusDetails = strjarry;
                        }

                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[3]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JReportTypeDetails = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[3]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JReportTypeDetails = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("AccountList", JAccountDetails),
                        new JProperty("ServiceEngineerList", JServiceEngineerDetails),
                        new JProperty("StatusList", JStatusDetails),
                        new JProperty("ReportTypeList", JReportTypeDetails)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }
        /// <summary>
        /// get ticket details by ticket number
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewGetTicketDetailsById(TicketDTO obj)
        {
            var result = service.GetTicketDetails(obj);
            return Ok(result);
        }

        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewGetTicketDetailsByIdM(TicketDTO obj)
        {
            var result = service.GetTicketDetails(obj);
            string msg = "";
            bool val = true;
            JArray JTicketDetails = JArray.Parse("[]");
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

                            foreach (JObject item in strjarry)
                            {
                                var pv = item.SelectToken("WarehouseJson");
                                var pvj = JArray.Parse(pv.ToString());
                                item.Add(new JProperty("JWarehouseJson", pvj));

                                var Url = item.SelectToken("Url");
                                var Urlj = JArray.Parse(Url.ToString());
                                item.Add(new JProperty("JUrl", Urlj));

                                var ReportJson = item.SelectToken("ReportJson");
                                var ReportJsonj = JArray.Parse(ReportJson.ToString());
                                item.Add(new JProperty("JReportJson", ReportJsonj));

                                var SparePartRequestJson = item.SelectToken("SparePartRequestJson");
                                var SparePartRequestJsonj = JArray.Parse(SparePartRequestJson.ToString());
                                item.Add(new JProperty("JSparePartRequestJson", SparePartRequestJsonj));

                                var StatusJson = item.SelectToken("StatusJson");
                                var StatusJsonj = JArray.Parse(StatusJson.ToString());
                                item.Add(new JProperty("JStatusJson", StatusJsonj));

                                var commentsjson = item.SelectToken("commentsjson");
                                var commentsjsonj = JArray.Parse(commentsjson.ToString());
                                item.Add(new JProperty("JCommentsjson", commentsjsonj));

                                var ServiceEngineerJson = item.SelectToken("ServiceEngineerJson");
                                var ServiceEngineerJsonj = JArray.Parse(ServiceEngineerJson.ToString());
                                item.Add(new JProperty("JServiceEngineerJson", ServiceEngineerJsonj));

                            }

                            if (!string.IsNullOrEmpty(str))
                                JTicketDetails = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JTicketDetails = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("TicketDetails", JTicketDetails)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }

        /// <summary>
        /// Get Spare part list by warehouseid
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewGetSparePartsList(TicketDTO obj)
        {
            var result = service.GetSparePartListById(obj);
            return Ok(result);
        }
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewGetSparePartsListM(TicketDTO obj)
        {

            var result = service.GetSparePartListById(obj);
            string msg = "";
            bool val = true;
            JArray JSparePartDetails = JArray.Parse("[]");
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
                                JSparePartDetails = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JSparePartDetails = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("SparePartList", JSparePartDetails)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }
        /// <summary>
        /// Insert spare part request
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewSparePartRequest(TicketDTO obj)
        {
            var result = service.AddSparePartRequest(obj);
            return Ok(result);
        }

        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewSparePartRequestM(TicketDTO obj)
        {
            var result = service.AddSparePartRequest(obj);
            string msg = "";
            bool val = false;

            if (result.message != "0")
                val = true;

            if (obj.FlagId == 2)
                msg = "Spare Part Requested Successfully.";
            else if (obj.FlagId == 1)
                msg = "Assigned Spare Part Added Successfully.";
            msg = val == true ? "" : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
        }
        /// <summary>
        /// Add comments
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewComments(TicketDTO obj)
        {
            var result = service.Addcomments(obj);
            return Ok(result);
        }

        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewCommentsM(TicketDTO obj)
        {
            var result = service.Addcomments(obj);
            string msg = "";
            bool val = true;

            msg = val == true ? "Comment Added Successfully." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)));
            return Ok(res);
        }
        /// <summary>
        /// get Spare part request tickets
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewSparePartRequestTickets(TicketDTO obj)
        {
            var result = service.GetSparePartRequestTickets(obj);
            return Ok(result);
        }

        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewSparePartRequestTicketsM(TicketDTO obj)
        {
            var result = service.GetSparePartRequestTickets(obj);

            string msg = "";
            bool val = true;
            JArray JSparePartRequestTickets = JArray.Parse("[]");

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
                                JSparePartRequestTickets = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JSparePartRequestTickets = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("SparePartRequestTickets", JSparePartRequestTickets)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }


        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewServiceEngineerDashboardCountsM(TicketDTO obj)
        {
            var result = service.GetServiceEngineerCounts(obj);

            string msg = "";
            bool val = true;
            JArray JSparePartRequestTickets = JArray.Parse("[]");

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
                                JSparePartRequestTickets = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JSparePartRequestTickets = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("ServiceEngineerTicketCounts", JSparePartRequestTickets)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }


        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewServiceEngineerDashboardCounts(TicketDTO obj)
        {
            var result = service.GetServiceEngineerCounts(obj);
            return Ok(result);
        }

        #region Enquiry
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewEnquiryComments(TicketDTO obj)
        {
            var result = service.AddEnquirycomments(obj);
            return Ok(result);
        }
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewEnquiryCommentsM(TicketDTO obj)
        {
            var result = service.AddEnquirycomments(obj);
            string msg = "";
            bool val = false;

            if (result.message != "0")
            {
                val = true;
            }
            //JObject res1 = new JObject(new JProperty("SystemManagerId", result.SystemManagerId.ToString())
            //            );
            msg = val == true ? "Comments Send Successfully." : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
        }
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewEnquiry(TicketDTO obj)
        {
            var result = service.AddEnquiry(obj);
            return Ok(result);
        }

        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewEnquiryM(TicketDTO obj)
        {
            var result = service.AddEnquiry(obj);
            string msg = "";
            bool val = false;

            if (result.message != "0")
            {
                val = true;
            }
            //JObject res1 = new JObject(new JProperty("SystemManagerId", result.SystemManagerId.ToString())
            //            );
            msg = val == true ? "Enquiry Added Successfully." : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
        }
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewEnquiryList(TicketDTO obj)
        {
            var result = service.GetEnquiryList(obj);
            return Ok(result);
        }

        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewEnquiryListM(TicketDTO obj)
        {
            var result = service.GetEnquiryList(obj);
            string msg = "";
            bool val = true;
            JArray JEnquiryList = JArray.Parse("[]");
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
                                JEnquiryList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JEnquiryList = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("EnquiryList", JEnquiryList)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewEnquiryDetails(TicketDTO obj)
        {
            var result = service.GetEnquiryDetails(obj);
            return Ok(result);
        }

        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewEnquiryDetailsM(TicketDTO obj)
        {
            var result = service.GetEnquiryDetails(obj);
            string msg = "";
            bool val = true;
            JArray JTicketDetails = JArray.Parse("[]");
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

                            foreach (JObject item in strjarry)
                            {

                                var commentsjson = item.SelectToken("commentsJson");
                                var commentsjsonj = JArray.Parse(commentsjson.ToString());
                                item.Add(new JProperty("JCommentsjson", commentsjsonj));

                            }

                            if (!string.IsNullOrEmpty(str))
                                JTicketDetails = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JTicketDetails = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("EnquiryDetails", JTicketDetails)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }
        #endregion

        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewAddTicketRatingM(TicketDTO obj)
        {
            var result = service.AddTicketRating(obj);
            string msg = "";
            bool val = true;

            msg = val == true ? "Rating Added Successfully." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)));
            return Ok(res);
        }

        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewTicketRatingDetailsM(TicketDTO obj)
        {
            var result = service.GetTicketRatingList(obj);
            string msg = "";
            bool val = true;
            JArray JTicketDetails = JArray.Parse("[]");
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
                                JTicketDetails = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JTicketDetails = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("RatingDetails", JTicketDetails)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }

        #region Report
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewCrmRawData(TicketDTO obj)
        {
            var result = service.CrmRawData(obj);
            return Ok(result);
        }

        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewArchiveReportTicket(TicketDTO obj)
        {
            var result = service.TicketServiceTab(obj);
            return Ok(result);
        }

        [ResponseType(typeof(TicketDTO))]
        public IEnumerable<TicketDTO> NewAssetListReport(TicketDTO obj)
        {
            var list = service.AssetListReport(obj);
            return list;
        }

        [ResponseType(typeof(TicketDTO))]
        public IEnumerable<TicketDTO> NewProductReport(TicketDTO obj)
        {
            var list = service.ProductReport(obj);
            return list;
        }

        [ResponseType(typeof(TicketDTO))]
        public IEnumerable<TicketDTO> NewEngineerWiseStatusReport(TicketDTO obj)
        {
            var list = service.EngineerWiseStatusReport(obj);
            return list;
        }
        [ResponseType(typeof(TicketDTO))]
        public IEnumerable<TicketDTO> NewAccountTicketReport(TicketDTO obj)
        {
            var list = service.AccountTicketReport(obj);
            return list;
        }
        [ResponseType(typeof(TicketDTO))]
        public IEnumerable<TicketDTO> NewPerMonthStatus(TicketDTO obj)
        {
            var list = service.PerMonthStatus(obj);
            return list;
        }
        [ResponseType(typeof(TicketDTO))]
        public IEnumerable<TicketDTO> NewRepeatedErrorReport(TicketDTO obj)
        {
            var list = service.RepeatedErrorReport(obj);
            return list;
        }
        [ResponseType(typeof(TicketDTO))]
        public IEnumerable<TicketDTO> NewSparePartTicketsCountReport(TicketDTO obj)
        {
            var list = service.SparePartTicketsCountReport(obj);
            return list;
        }

        #endregion
    }
}
