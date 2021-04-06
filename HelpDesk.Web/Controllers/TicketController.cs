using HelpDesk.Web.Handlers;
using HelpDesk.Web.Models;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Xml;

namespace HelpDesk.Web.Controllers
{
    public class TicketController : Controller
    {
        // GET: Ticket
        public async Task<ActionResult> NewTicket()
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            //Session["MultipleImagesLst" + ses] = null;
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        TicketDTO obj = new TicketDTO();
                        int userid = int.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        int comid = int.Parse(Session["SSCompanyId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                        obj.CreatedBy = userid;
                        obj.CompanyId = 10;
                        obj.OrganizationId = orgid;

                        List<TicketDTO> tickettlst = new List<TicketDTO>();
                        List<TicketDTO> productlst = new List<TicketDTO>();
                        List<TicketDTO> accountlst = new List<TicketDTO>();
                        List<TicketDTO> companylst = new List<TicketDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/TicketsAPI/NewSystemUserProducts", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);

                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    SelectList ddlusers = new SelectList("", "ProductId", "ProductName", 0);
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        productlst = ds.Tables[0].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            ProductName = dataRow.Field<string>("ProductName"),
                                            ProductId = dataRow.Field<int>("ProductId")
                                        }).ToList();


                                        List<TicketDTO> _objStudent = productlst;
                                        ddlusers = new SelectList(_objStudent, "ProductId", "ProductName", obj.ProductId);
                                        ViewData["ddlProductList"] = ddlusers;
                                    }
                                    else
                                    {
                                        List<TicketDTO> _objStudent = productlst;
                                        ddlusers = new SelectList(_objStudent, "ProductId", "ProductName", obj.ProductId);
                                        ViewData["ddlProductList"] = ddlusers;
                                    }


                                    SelectList ddlaccounts = new SelectList("", "AccountId", "AccountName", 0);
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        accountlst = ds.Tables[1].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            AccountName = dataRow.Field<string>("AccountName"),
                                            AccountId = dataRow.Field<int>("AccountId")
                                        }).ToList();

                                        List<TicketDTO> _objlst = accountlst;
                                        ddlaccounts = new SelectList(_objlst, "AccountId", "AccountName", obj.AccountId);
                                        ViewData["ddlAccountList"] = ddlaccounts;
                                    }
                                    else
                                    {
                                        List<TicketDTO> _objStudent = accountlst;
                                        ddlaccounts = new SelectList(_objStudent, "AccountId", "AccountName", obj.AccountId);
                                        ViewData["ddlAccountList"] = ddlaccounts;
                                    }

                                    SelectList ddlcompany = new SelectList("", "CompanyId", "CompanyName", 0);
                                    if (ds.Tables[2].Rows.Count > 0)
                                    {
                                        companylst = ds.Tables[2].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            CompanyName = dataRow.Field<string>("CompanyName"),
                                            CompanyId = dataRow.Field<int>("CompanyId")
                                        }).ToList();

                                        List<TicketDTO> _objlst = companylst;
                                        ddlcompany = new SelectList(_objlst, "CompanyId", "CompanyName", obj.CompanyId);
                                        ViewData["ddlComapanyList"] = ddlcompany;
                                    }
                                    else
                                    {
                                        List<TicketDTO> _objStudent = companylst;
                                        ddlcompany = new SelectList(_objStudent, "CompanyId", "CompanyName", obj.CompanyId);
                                        ViewData["ddlComapanyList"] = ddlcompany;
                                    }

                                    List<TicketDTO> _objStudew = new List<TicketDTO>();
                                    SelectList ddlmodels = new SelectList("", "AMId", "ModelName", 0);

                                    if (ds.Tables[3].Rows.Count > 0)
                                    {
                                        _objStudew = ds.Tables[3].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            ProductName = dataRow.Field<string>("ProductName"),
                                            ProductId = dataRow.Field<int>("ProductId"),
                                            ModelName = dataRow.Field<string>("ModelName"),
                                            //AMId = dataRow.Field<int>("AMId")
                                            AMModelId = dataRow.Field<long>("AMModelId"),
                                            AccountId = dataRow.Field<int>("AccountId"),
                                            SystemNoSerialNo = dataRow.Field<string>("SystemNoSerialNo")
                                        }).ToList();

                                        List<TicketDTO> _objlst = _objStudew;
                                        ddlmodels = new SelectList(_objlst, "AMModelId", "SystemNoSerialNo", 0);
                                        ViewData["ddlModels"] = ddlmodels;


                                        Session["SSModelList"] = _objStudew;

                                    }
                                    else
                                    {
                                        List<TicketDTO> _objStudent = _objStudew;
                                        ddlmodels = new SelectList(_objStudent, "AMModelId", "SystemNoSerialNo", 0);
                                        ViewData["ddlModels"] = ddlmodels;
                                    }

                                    List<TicketDTO> _objStudeRT = new List<TicketDTO>();
                                    SelectList ddlReportType = new SelectList("", "ReportId", "ReportTypeName", 0);

                                    if (ds.Tables[4].Rows.Count > 0)
                                    {
                                        _objStudeRT = ds.Tables[4].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            ReportTypeName = dataRow.Field<string>("ReportTypeName"),
                                            ReportId = dataRow.Field<int>("ReportId")
                                        }).ToList();

                                        List<TicketDTO> _objlst = _objStudeRT;
                                        ddlReportType = new SelectList(_objlst, "ReportId", "ReportTypeName", obj.ReportId);
                                        ViewData["ddlReportType"] = ddlReportType;
                                    }
                                    else
                                    {
                                        List<TicketDTO> _objStudent = _objStudeRT;
                                        ddlReportType = new SelectList(_objStudent, "ReportId", "ReportTypeName", obj.ReportId);
                                        ViewData["ddlReportType"] = ddlReportType;
                                    }
                                }
                            }

                            var list = new List<SelectListItem>
    {
        new SelectListItem{ Text="High", Value = "H" },
        new SelectListItem{ Text="Medium", Value = "M" },
        new SelectListItem{ Text="Low", Value = "L", Selected = true },
    };

                            ViewData["ddlPriority"] = list;
                        }
                        obj.RoleId = roleid;

                        return View(obj);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Authenticate", "Authentication");
                    }
                }
            }
        }
        [HttpPost]
        public async Task<ActionResult> NewTicket(TicketDTO obj, FormCollection fomr)
        {
            //obj.SystemManagerId=int.Parse(Request["hdnSystemManagerId"].ToString());
            obj.SystemManagerId = int.Parse(Request.Form["SystemManagerId"]);
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {

                long userid = long.Parse(Session["SSUserId"].ToString());
                int compid = int.Parse(Session["SSCompanyId"].ToString());
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                int accntid = int.Parse(Session["SSAccountId"].ToString());
                int roleid = int.Parse(Session["SSRoleId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        List<TicketDTO> multiple_images = (List<TicketDTO>)Session["MultipleImagesLst" + userid];


                        if (multiple_images.Count() > 0)
                        {
                            var xmldoc_docs = new XmlDocument();
                            var parentelemeng_docs = xmldoc_docs.CreateElement("MultiDocuments");
                            var parent_docs = xmldoc_docs.CreateElement("MultiDocument");

                            foreach (TicketDTO item in multiple_images)
                            {
                                if (item == null)
                                {
                                    obj.ContentType = "";
                                    obj.Url = "";
                                    obj.multipledocuments_xml = "";
                                    break;
                                }
                                var parentelement = xmldoc_docs.CreateElement("Row");
                                var filepath_xml = xmldoc_docs.CreateElement("filepath");
                                var ContentType_xml = xmldoc_docs.CreateElement("ContentType");
                                //var UniqueId_xml = xmldoc_docs.CreateElement("uniqueid");


                                filepath_xml.InnerText = item.Url;
                                ContentType_xml.InnerText = item.ContentType;
                                //UniqueId_xml.InnerText = uniqueid;

                                parentelement.AppendChild(filepath_xml);
                                parentelement.AppendChild(ContentType_xml);
                                //parentelement.AppendChild(UniqueId_xml);

                                parentelemeng_docs.AppendChild(parent_docs);
                                parent_docs.AppendChild(parentelement);
                            }

                            obj.multipledocuments_xml = parentelemeng_docs.InnerXml;
                            obj.Url = "";
                            obj.ContentType = "";
                        }
                        else
                        {
                            obj.ContentType = "";
                            obj.Url = "";
                            obj.multipledocuments_xml = "";
                        }

                        obj.CreatedBy = userid;

                        if (compid == 0)
                            obj.CompanyId = obj.CompanyId;
                        else
                            obj.CompanyId = compid;

                        obj.OrganizationId = orgid;



                        //if (accntid == 0)
                        //{
                        //    if (roleid == 505 || roleid == 502)
                        //        obj.AccountId = obj.AccountId;
                        //}
                        //else
                        //    obj.AccountId = accntid;


                        if (roleid == 505 || roleid == 502)
                            obj.AccountId = obj.AccountId;
                        else
                            obj.AccountId = accntid;


                        if (obj.Description == null)
                            obj.Description = "";

                        if (roleid == 503 || roleid == 504 || roleid == 505)
                        {
                            obj.ReportId = 1;
                        }
                        obj.AMId = 0;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/TicketsAPI/NewInsertTicketRequest", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var tickets = JsonConvert.DeserializeObject<TicketDTO>(responseData);
                            obj.message = tickets.message;
                            string msg = obj.message;

                            //List<TicketDTO> multiple_images = new List<TicketDTO>();
                            //multiple_images.Clear();
                            Session["MultipleImagesLst" + userid] = multiple_images;
                            //List<TicketDTO> multiple_images = (List<TicketDTO>)Session["MultipleImagesLst" + userid];
                        }
                        return RedirectToAction("Tickets");
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Authenticate", "Authentication");
                    }
                }
            }
        }
        public async Task<ActionResult> GetModels(int id, int acctid)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {

                        long userid = long.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        int comid = int.Parse(Session["SSCompanyId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());

                        TicketDTO obj = new TicketDTO();

                        obj.ProductId = id;
                        obj.CreatedBy = userid;
                        obj.RoleId = roleid;
                        obj.AccountId = acctid;

                        List<TicketDTO> modellst = new List<TicketDTO>();
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/TicketsAPI/NewSystemUserModels", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);
                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        modellst = ds.Tables[0].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            ModelName = dataRow.Field<string>("ModelName"),
                                            AMId = dataRow.Field<int>("AMId")
                                        }).ToList();
                                        obj.ModelList = modellst;
                                    }
                                    else
                                        obj.ModelList = modellst;
                                }
                                else
                                    obj.ModelList = modellst;
                            }
                        }
                        //return Json(new SelectList(ddlmodels, "Value", "Text"));
                        return Json(obj.ModelList);
                    }
                    catch (Exception ex)
                    {
                        TicketDTO obj = new TicketDTO();
                        return Json(obj.ProductList);
                    }
                }
            }
        }
        public async Task<ActionResult> GetAccounts(int id)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        long userid = long.Parse(Session["SSUserId"].ToString());
                        //int roleid = int.Parse(Session["SSRoleId"].ToString());
                        //int comid = int.Parse(Session["SSCompanyId"].ToString());
                        //int orgid = int.Parse(Session["SSOrganizationId"].ToString());

                        TicketDTO obj = new TicketDTO();

                        obj.CompanyId = id;
                        obj.CreatedBy = userid;

                        List<TicketDTO> accountlst = new List<TicketDTO>();

                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/TicketsAPI/NewAccountsByCompany", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);
                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        accountlst = ds.Tables[0].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            AccountName = dataRow.Field<string>("AccountName"),
                                            AccountId = dataRow.Field<int>("AccountId")
                                        }).ToList();
                                        obj.ModelList = accountlst;
                                    }
                                    else
                                        obj.ModelList = accountlst;
                                }
                                else
                                    obj.ModelList = accountlst;
                            }
                        }
                        return Json(obj.ModelList);
                    }
                    catch (Exception ex)
                    {
                        TicketDTO obj = new TicketDTO();
                        return Json(obj.ModelList);
                    }
                }
            }
        }
        public async Task<ActionResult> GetProducts(int id)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {

                        long userid = long.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        int comid = int.Parse(Session["SSCompanyId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());

                        TicketDTO obj = new TicketDTO();

                        obj.AccountId = id;
                        obj.CreatedBy = userid;
                        obj.RoleId = roleid;

                        List<TicketDTO> modellst = new List<TicketDTO>();
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/TicketsAPI/NewproductsByCompany", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);
                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        modellst = ds.Tables[0].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            ProductName = dataRow.Field<string>("ProductName"),
                                            ProductId = dataRow.Field<int>("ProductId"),
                                            ModelName = dataRow.Field<string>("ModelName"),
                                            AMModelId = dataRow.Field<long>("AMModelId"),
                                            SystemNoSerialNo = dataRow.Field<string>("SystemNoSerialNo"),
                                        }).ToList();
                                        obj.ModelList = modellst;

                                        Session["SSModelList"] = obj.ModelList;

                                        var lst = (from c in modellst
                                                   group c by new
                                                   {
                                                       c.ProductId,
                                                       c.ProductName
                                                   } into grp
                                                   select new TicketDTO()
                                                   {
                                                       ProductId = grp.Key.ProductId,
                                                       ProductName = grp.Key.ProductName
                                                   }).ToList();

                                        obj.ProductList = lst;



                                    }
                                    else
                                    {
                                        obj.ModelList = modellst;
                                        obj.ProductList = modellst;
                                    }

                                }
                                else
                                {
                                    obj.ModelList = modellst;
                                    obj.ProductList = modellst;
                                }
                            }
                        }
                        return Json(obj);
                    }
                    catch (Exception ex)
                    {
                        TicketDTO obj = new TicketDTO();
                        return Json(obj.ModelList);
                    }
                }
            }
        }
        public ActionResult GetProductsByAMId(long AMID)
        {
            TicketDTO obj = new TicketDTO();
            obj.ModelList = (IEnumerable<TicketDTO>)Session["SSModelList"];

            var query = from a in obj.ModelList
                        where a.AMModelId == AMID
                        select a.ProductId;

            //var ModelName = from a in obj.ModelList
            //            where a.AMModelId == AMID
            //            select a.ModelName;
            var employee = obj.ModelList
    .Where(x => x.AMModelId == AMID)
    .Select(x => new { x.ProductId, x.ProductName, x.ModelName });
            int ProductId = 0;
            string ModelName = "";
            string ProductName = "";
            foreach (var item in employee)
            {
                ProductName = item.ProductName;
                ModelName = item.ModelName;
                ProductId = int.Parse(item.ProductId.ToString());
            }

            //var ModelName = from a in obj.ModelList
            //                where a.AMModelId == AMID
            //                select a.ModelName;
            //var ssa=query.Selec
            //int ProductId = 0;
            //foreach (var item in query)
            //{ 
            //    ProductId = int.Parse(item.ToString());
            //}
            return Json(new { ProductId = ProductId, ModelName = ModelName, ProductName = ProductName });
        }
        public async Task<ActionResult> TicketDetails(long id)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                //return Json("../Login/Index");
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        int userid = int.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        int comid = int.Parse(Session["SSCompanyId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                        TicketDTO obj = new TicketDTO();
                        obj.TicketNumber = id;
                        obj.RoleId = roleid;
                        obj.CompanyId = comid;
                        obj.OrganizationId = orgid;

                        List<TicketDTO> tickettlst = new List<TicketDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/TicketsAPI/NewGetTicketDetailsById", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);

                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        tickettlst = ds.Tables[0].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            TicketNumber = dataRow.Field<long>("TicketNumber"),
                                            Description = dataRow.Field<string>("Description"),
                                            Priority = dataRow.Field<string>("Priority"),
                                            Statustxt = dataRow.Field<string>("Status"),
                                            Status = dataRow.Field<int>("StatusId"),
                                            AccountName = dataRow.Field<string>("AccountName"),
                                            ModelName = dataRow.Field<string>("ModelName"),
                                            SystemNo = dataRow.Field<string>("SystemNo"),
                                            ProductName = dataRow.Field<string>("ProductName"),
                                            FullName = dataRow.Field<string>("FullName"),
                                            UserId = dataRow.Field<long>("UserId"),
                                            SerialNo = dataRow.Field<string>("SerialNo"),
                                            CreatedOn = dataRow.Field<DateTime>("CreatedOn"),
                                            Url = dataRow.Field<string>("Url"),
                                            //ContentType = dataRow.Field<string>("ContentType"),
                                            CreatedUser = dataRow.Field<string>("CreatedUser"),
                                            ReportsJson = dataRow.Field<string>("ReportJson"),
                                            RequestResponseStr = dataRow.Field<string>("RequestResponseStr"),
                                            ActualStartTime = dataRow.Field<string>("ActualStartTime"),
                                            ResolvedTime = dataRow.Field<string>("ResolvedTime"),
                                            MappedWarehouseId = dataRow.Field<int>("MappedWarehouseId"),
                                            WarehouseJson = dataRow.Field<string>("WarehouseJson"),
                                            SparePartRequestJson = dataRow.Field<string>("SparePartRequestJson"),
                                            StatusJson = dataRow.Field<string>("StatusJson"),
                                            commentsjson = dataRow.Field<string>("commentsjson"),
                                            Area = dataRow.Field<string>("Area"),
                                            CreatedUserId = dataRow.Field<long>("CreatedUserId"),
                                            Mobile = dataRow.Field<string>("Mobile"),
                                            Email = dataRow.Field<string>("Email"),
                                            POContract = dataRow.Field<string>("POContract"),
                                            InstallationDate = dataRow.Field<string>("InstallationDate"),
                                            IsContract = dataRow.Field<bool>("IsContract"),
                                            WarrantyExpiryDate = dataRow.Field<string>("WarrantyExpiryDate"),
                                            PPMDate = dataRow.Field<string>("PPMDate"),
                                            ServiceStartDate = dataRow.Field<string>("ServiceStartDate"),
                                            ReportTypeName = dataRow.Field<string>("ReportTypeName"),
                                            ServiceEngineerResolvedDate = dataRow.Field<string>("ServiceEngineerResolvedDate"),
                                            CustomerConfirmationDate = dataRow.Field<string>("CustomerConfirmationDate"),
                                            ManagerConfirmationDate = dataRow.Field<string>("ManagerConfirmationDate"),
                                            ManagerName = dataRow.Field<string>("ManagerName"),
                                            Actioncomments = dataRow.Field<string>("Actioncomments"),
                                            ProblemDescription = dataRow.Field<string>("ProblemDescription"),
                                            WorkHours = dataRow.Field<string>("WorkHours"),
                                            ReportId = dataRow.Field<int>("ReportTypeId"),
                                            CreatedUserRoleId = dataRow.Field<int>("CreatedUserRoleId"),
                                            SupervisorConfirmationDate = dataRow.Field<string>("SupervisorConfirmationDate"),
                                            SupervisorName = dataRow.Field<string>("SupervisorName"),
                                            ServiceEngineerJson = dataRow.Field<string>("ServiceEngineerJson"),
                                            StationName = dataRow.Field<string>("StationName"),
                                            AccountCode = dataRow.Field<string>("AccountCode"),
                                            ManagerFullName = dataRow.Field<string>("ManagerFullName"),
                                            ManagerMobile = dataRow.Field<string>("ManagerMobile"),
                                            ManagerEmail = dataRow.Field<string>("ManagerEmail")
                                        }).ToList();

                                        obj.TicketList = tickettlst;

                                        string urlsjson = obj.TicketList.FirstOrDefault().Url;
                                        var modelurl = JsonConvert.DeserializeObject<List<TicketDTO>>(urlsjson);
                                        obj.UrlList = modelurl;

                                        string accountsjson = obj.TicketList.FirstOrDefault().ReportsJson;
                                        var model = JsonConvert.DeserializeObject<List<TicketDTO>>(accountsjson);
                                        obj.ReportList = model;

                                        string warehousejson = obj.TicketList.FirstOrDefault().WarehouseJson;
                                        var modelwa = JsonConvert.DeserializeObject<List<TicketDTO>>(warehousejson);
                                        obj.WarehouseList = modelwa;

                                        string sparepart = obj.TicketList.FirstOrDefault().SparePartRequestJson;
                                        var modelsp = JsonConvert.DeserializeObject<List<TicketDTO>>(sparepart);
                                        obj.SparePartList = modelsp;

                                        string statusj = obj.TicketList.FirstOrDefault().StatusJson;
                                        var modelstaj = JsonConvert.DeserializeObject<List<TicketDTO>>(statusj);
                                        obj.StatusLst = modelstaj;

                                        string commentsj = obj.TicketList.FirstOrDefault().commentsjson;
                                        var modalcomments = JsonConvert.DeserializeObject<List<TicketDTO>>(commentsj);
                                        obj.CommentsList = modalcomments;

                                        string serviceenglst = obj.TicketList.FirstOrDefault().ServiceEngineerJson;
                                        var modalserviceLst = JsonConvert.DeserializeObject<List<TicketDTO>>(serviceenglst);
                                        obj.ServiceEngineerList = modalserviceLst;

                                        //ServiceEngineerList
                                    }
                                    else
                                        obj.TicketList = tickettlst;
                                }
                            }
                        }
                        return PartialView("TicketDetailsPV", obj);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
            }
        }
        public async Task<ActionResult> Tickets()
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int userid = int.Parse(Session["SSUserId"].ToString());
                int roleid = int.Parse(Session["SSRoleId"].ToString());
                int comid = int.Parse(Session["SSCompanyId"].ToString());
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        TicketDTO obj = new TicketDTO();
                        obj.CompanyId = comid;
                        obj.OrganizationId = orgid;
                        obj.CreatedBy = userid;

                        List<TicketDTO> tickettlst = new List<TicketDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/TicketsAPI/NewSystemUserTickets", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);

                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        tickettlst = ds.Tables[0].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            TicketNumber = dataRow.Field<long>("TicketNumber"),
                                            Description = dataRow.Field<string>("Description"),
                                            Priority = dataRow.Field<string>("Priority"),
                                            Statustxt = dataRow.Field<string>("Status"),
                                            AccountName = dataRow.Field<string>("AccountName"),
                                            ModelName = dataRow.Field<string>("ModelName"),
                                            SystemNo = dataRow.Field<string>("SystemNo"),
                                            ProductName = dataRow.Field<string>("ProductName"),
                                            FullName = dataRow.Field<string>("FullName"),
                                            UserId = dataRow.Field<long>("UserId"),
                                            CreatedOn = dataRow.Field<DateTime>("CreatedOn"),
                                            Area = dataRow.Field<string>("Area"),
                                            ReportTypeName = dataRow.Field<string>("ReportTypeName")
                                        }).ToList();
                                        obj.TicketList = tickettlst;
                                    }
                                    else
                                        obj.TicketList = tickettlst;
                                }
                            }
                        }


                        return View(obj);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Authenticate", "Authentication");
                    }
                }
            }
        }
        public async Task<ActionResult> WarrantyExpiredTickets()
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        int userid = int.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        int comid = int.Parse(Session["SSCompanyId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                        TicketDTO obj = new TicketDTO();
                        obj.CompanyId = 10;
                        obj.UserId = userid;
                        obj.OrganizationId = orgid;
                        List<TicketDTO> companylst = new List<TicketDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/TicketsAPI/NewSystemUserProducts", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);

                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    SelectList ddlcompany = new SelectList("", "CompanyId", "CompanyName", 0);
                                    if (ds.Tables[2].Rows.Count > 0)
                                    {
                                        companylst = ds.Tables[2].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            CompanyName = dataRow.Field<string>("CompanyName"),
                                            CompanyId = dataRow.Field<int>("CompanyId")
                                        }).ToList();

                                        List<TicketDTO> _objlst = companylst;
                                        ddlcompany = new SelectList(_objlst, "CompanyId", "CompanyName", obj.CompanyId);
                                        ViewData["ddlComapanyList"] = ddlcompany;
                                    }
                                    else
                                    {
                                        List<TicketDTO> _objStudent = companylst;
                                        ddlcompany = new SelectList(_objStudent, "CompanyId", "CompanyName", obj.CompanyId);
                                        ViewData["ddlComapanyList"] = ddlcompany;
                                    }

                                }
                            }


                        }
                        obj.RoleId = roleid;
                        return View(obj);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
            }
        }
        public async Task<ActionResult> UnderApprovalTicketsPV(int id)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                //return Json("../Login/Index");
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        int userid = int.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        int comid = int.Parse(Session["SSCompanyId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                        TicketDTO obj = new TicketDTO();
                        obj.CompanyId = id;

                        List<TicketDTO> tickettlst = new List<TicketDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/TicketsAPI/NewGetUnderApprovalTickets", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);

                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        tickettlst = ds.Tables[0].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            TicketNumber = dataRow.Field<long>("TicketNumber"),
                                            Description = dataRow.Field<string>("Description"),
                                            Priority = dataRow.Field<string>("Priority"),
                                            Statustxt = dataRow.Field<string>("Status"),
                                            AccountName = dataRow.Field<string>("AccountName"),
                                            ModelName = dataRow.Field<string>("ModelName"),
                                            SystemNo = dataRow.Field<string>("SystemNo"),
                                            ProductName = dataRow.Field<string>("ProductName"),
                                            //FullName = dataRow.Field<string>("ProductName"),
                                            //UserId = dataRow.Field<long>("UserId"),
                                            SerialNo = dataRow.Field<string>("SerialNo"),
                                            CreatedOn = dataRow.Field<DateTime>("CreatedOn")
                                        }).ToList();
                                        obj.TicketList = tickettlst;
                                    }
                                    else
                                        obj.TicketList = tickettlst;
                                }
                            }
                        }
                        return PartialView("UnderApprovalTicketsPV", obj);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
            }
        }
        /// <summary>
        /// Service Engineer tickets
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> AssignedTickets()
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int userid = int.Parse(Session["SSUserId"].ToString());
                int roleid = int.Parse(Session["SSRoleId"].ToString());
                int comid = int.Parse(Session["SSCompanyId"].ToString());
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        TicketDTO obj = new TicketDTO();
                        obj.CreatedBy = userid;
                        obj.CompanyId = comid;
                        obj.OrganizationId = orgid;

                        //if (roleid == 501 || roleid == 502)
                        //    obj.CreatedBy = 0;

                        List<TicketDTO> tickettlst = new List<TicketDTO>();
                        List<TicketDTO> productlst = new List<TicketDTO>();
                        List<TicketDTO> accountlst = new List<TicketDTO>();
                        List<TicketDTO> companylst = new List<TicketDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/TicketsAPI/NewServiceEngineerTicketsFileters", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);

                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {

                                    SelectList ddlaccounts = new SelectList("", "AccountId", "AccountName", 0);
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        accountlst = ds.Tables[0].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            AccountName = dataRow.Field<string>("AccountName"),
                                            AccountId = dataRow.Field<int>("AccountId")
                                        }).ToList();

                                        List<TicketDTO> _objlst = accountlst;
                                        ddlaccounts = new SelectList(_objlst, "AccountId", "AccountName", obj.AccountId);
                                        ViewData["ddlAccountList"] = ddlaccounts;
                                    }
                                    else
                                    {
                                        List<TicketDTO> _objStudent = accountlst;
                                        ddlaccounts = new SelectList(_objStudent, "AccountId", "AccountName", obj.AccountId);
                                        ViewData["ddlAccountList"] = ddlaccounts;
                                    }



                                    SelectList ddlusers = new SelectList("", "UserId", "FullName", 0);
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        productlst = ds.Tables[1].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            FullName = dataRow.Field<string>("FullName"),
                                            UserId = dataRow.Field<long>("UserId")
                                        }).ToList();


                                        List<TicketDTO> _objStudent = productlst;
                                        ddlusers = new SelectList(_objStudent, "UserId", "FullName", obj.UserId);
                                        ViewData["ddlUsers"] = ddlusers;
                                    }
                                    else
                                    {
                                        List<TicketDTO> _objStudent = productlst;
                                        ddlusers = new SelectList(_objStudent, "UserId", "FullName", obj.UserId);
                                        ViewData["ddlUsers"] = ddlusers;
                                    }




                                    SelectList ddlcompany = new SelectList("", "Status", "Statustxt", 0);
                                    if (ds.Tables[2].Rows.Count > 0)
                                    {
                                        companylst = ds.Tables[2].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            Statustxt = dataRow.Field<string>("Statustxt"),
                                            Status = dataRow.Field<int>("StatusId")
                                        }).ToList();

                                        List<TicketDTO> _objlst = companylst;
                                        ddlcompany = new SelectList(_objlst, "Status", "Statustxt", obj.CompanyId);
                                        ViewData["ddlStatusList"] = ddlcompany;
                                    }
                                    else
                                    {
                                        List<TicketDTO> _objStudent = companylst;
                                        ddlcompany = new SelectList(_objStudent, "Status", "Statustxt", obj.CompanyId);
                                        ViewData["ddlStatusList"] = ddlcompany;
                                    }


                                }
                            }
                        }
                        obj.RoleId = roleid;
                        return View(obj);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
            }
        }
        public async Task<ActionResult> AssignedTicketsPV(long useridF, int statusF, int accountF, int pagenumber)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int userid = int.Parse(Session["SSUserId"].ToString());
                int roleid = int.Parse(Session["SSRoleId"].ToString());
                int comid = int.Parse(Session["SSCompanyId"].ToString());
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        TicketDTO obj = new TicketDTO();
                        obj.CreatedBy = userid;

                        List<TicketDTO> tickettlst = new List<TicketDTO>();
                        List<TicketDTO> productlst = new List<TicketDTO>();

                        obj.UserId = useridF;
                        obj.Status = statusF;
                        obj.AccountId = accountF;
                        obj.PageNumber = pagenumber;
                        obj.PageSize = 5;
                        obj.CompanyId = comid;
                        //if (roleid == 501 || roleid == 502)
                        //    obj.CreatedBy = 0;
                        obj.OrganizationId = orgid;
                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/TicketsAPI/NewServiceEngineerTickets", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);

                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        tickettlst = ds.Tables[0].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            TicketNumber = dataRow.Field<long>("TicketNumber"),
                                            Description = dataRow.Field<string>("Description"),
                                            Priority = dataRow.Field<string>("Priority"),
                                            Statustxt = dataRow.Field<string>("Statustxt"),
                                            AccountName = dataRow.Field<string>("AccountName"),
                                            ModelName = dataRow.Field<string>("ModelName"),
                                            SystemNo = dataRow.Field<string>("SystemNo"),
                                            ProductName = dataRow.Field<string>("ProductName"),
                                            SerialNo = dataRow.Field<string>("SerialNo"),
                                            CreatedOn = dataRow.Field<DateTime>("CreatedOn"),
                                            FullName = dataRow.Field<string>("FullName")

                                        }).ToList();
                                        obj.TicketList = tickettlst;
                                    }
                                    else
                                        obj.TicketList = tickettlst;
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        productlst = ds.Tables[1].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            TotalRecords = dataRow.Field<int>("TotalRecords")
                                        }).ToList();
                                        obj.TotalRecords = productlst.SingleOrDefault().TotalRecords;
                                        long toc = productlst.SingleOrDefault().TotalRecords;
                                        decimal d = decimal.Parse(toc.ToString());
                                        decimal e = decimal.Parse(obj.PageSize.ToString());
                                        decimal f = d / e;
                                        string s = f.ToString("0.00", CultureInfo.InvariantCulture);
                                        string[] parts = s.Split('.');
                                        int i1 = int.Parse(parts[0]);
                                        int i2 = int.Parse(parts[1]);
                                        if (i2 != 00)
                                            obj.pagingNumber = i1 + 1;
                                        else
                                            obj.pagingNumber = i1;
                                    }
                                }
                            }
                        }
                        obj.RoleId = roleid;
                        return PartialView("AssignedTicketsPV", obj);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Authenticate", "Authentication");
                    }
                }
            }
        }
        public async Task<ActionResult> UpdateTicketStatus(int id, long TicketNumber, string comments, string problemdescription)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        long userid = long.Parse(Session["SSUserId"].ToString());

                        TicketDTO obj = new TicketDTO();

                        obj.CreatedBy = userid;
                        obj.Status = id;
                        obj.TicketNumber = TicketNumber;
                        obj.Comments = comments;
                        obj.ProblemDescription = problemdescription;

                        bool status = false;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/TicketsAPI/NewUpdateTicketStatus", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);
                            string msg = docs.message;
                            if (msg == "1")
                                status = true;
                            else if (msg == "2")
                            {
                                status = true;
                            };
                        }
                        return Json(new { success = status });
                    }
                    catch (Exception ex)
                    {
                        TicketDTO obj = new TicketDTO();
                        return Json(obj.ModelList);
                    }
                }
            }
        }
        public async Task<ActionResult> AddResponseTime(string ResponseTime, long TicketNumber)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        long userid = long.Parse(Session["SSUserId"].ToString());

                        TicketDTO obj = new TicketDTO();

                        obj.CreatedBy = userid;
                        obj.ResponseTime = DateTime.Parse(ResponseTime.ToString());
                        obj.TicketNumber = TicketNumber;

                        bool status = false;

                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/TicketsAPI/NewResponseTime", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);
                            string msg = docs.message;
                            if (msg == "1")
                                status = true;
                            else if (msg == "2")
                            {
                                status = true;
                            };
                        }
                        return Json(new { success = status });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false });
                    }
                }
            }
        }
        public async Task<ActionResult> GetSparePartList(int warehouseid, long TicketNumber)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        TicketDTO obj = new TicketDTO();

                        obj.WarehouseId = warehouseid;
                        obj.TicketNumber = TicketNumber;

                        List<TicketDTO> modellst = new List<TicketDTO>();
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/TicketsAPI/NewGetSparePartsList", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);
                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        modellst = ds.Tables[0].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            //WarehouseStockId = dataRow.Field<long>("WarehouseStockId"),
                                            SparePartId = dataRow.Field<long>("SparePartId"),
                                            SparePartName = dataRow.Field<string>("SparePartName"),
                                            SparePartNumber = dataRow.Field<string>("SparePartNumber"),
                                            Quantity = dataRow.Field<int>("Quantity"),
                                            BaseQuantity = dataRow.Field<int>("BaseQuantity"),
                                            Price = dataRow.Field<string>("Price")
                                        }).ToList();
                                        obj.SparePartList = modellst;
                                        TempData["SparePartListtmp"] = obj.SparePartList;
                                    }
                                    else
                                    {
                                        obj.SparePartList = modellst;
                                        TempData["SparePartListtmp"] = obj.SparePartList;
                                    }
                                }
                                else
                                {
                                    obj.SparePartList = modellst;
                                    TempData["SparePartListtmp"] = obj.SparePartList;
                                }
                            }
                        }
                        return Json(obj.SparePartList);
                    }
                    catch (Exception ex)
                    {
                        TicketDTO obj = new TicketDTO();
                        return Json(obj.SparePartList);
                    }
                }
            }
        }
        public async Task<ActionResult> NewSparePartRequest(string json, long TicketNumber, int Type)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        long userid = long.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        int comid = int.Parse(Session["SSCompanyId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());

                        TicketDTO obj = new TicketDTO();
                        obj.TicketNumber = TicketNumber;
                        obj.message = json;
                        obj.UserId = userid;
                        obj.CompanyId = comid;
                        obj.OrganizationId = orgid;
                        obj.FlagId = Type;

                        bool status = false;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/TicketsAPI/NewSparePartRequest", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);
                            string msg = docs.message;
                            if (msg == "1")
                                status = true;
                            else
                            {
                                status = false;
                            };
                        }
                        return Json(new { success = status });
                    }
                    catch (Exception ex)
                    {
                        TicketDTO obj = new TicketDTO();
                        return Json(obj.ModelList);
                    }
                }
            }
        }
        public async Task<ActionResult> NewComments(string txt, long TicketNumber)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        long userid = long.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        int comid = int.Parse(Session["SSCompanyId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());

                        TicketDTO obj = new TicketDTO();
                        obj.TicketNumber = TicketNumber;
                        obj.message = txt;
                        obj.UserId = userid;

                        bool status = false;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/TicketsAPI/NewComments", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);
                            string msg = docs.message;
                            if (msg == "1")
                                status = true;
                            else
                            {
                                status = false;
                            };
                        }
                        return Json(new { success = status });
                    }
                    catch (Exception ex)
                    {
                        TicketDTO obj = new TicketDTO();
                        return Json(obj.ModelList);
                    }
                }
            }
        }
        public JsonResult GetSpareListDataById(long sparepartid)
        {
            List<TicketDTO> obj = TempData["SparePartListtmp"] as List<TicketDTO>;
            TempData.Keep();
            List<TicketDTO> _obj = obj.Where(x => x.SparePartId == sparepartid).ToList();
            return Json(_obj);
        }
        public async Task<ActionResult> EquipmentApproval()
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int userid = int.Parse(Session["SSUserId"].ToString());
                int roleid = int.Parse(Session["SSRoleId"].ToString());
                int comid = int.Parse(Session["SSCompanyId"].ToString());
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        TicketDTO obj = new TicketDTO();
                        obj.CreatedBy = userid;

                        List<TicketDTO> tickettlst = new List<TicketDTO>();
                        List<TicketDTO> productlst = new List<TicketDTO>();
                        obj.CompanyId = comid;
                        obj.OrganizationId = orgid;
                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/TicketsAPI/NewSparePartRequestTickets", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);

                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        tickettlst = ds.Tables[0].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            TicketNumber = dataRow.Field<long>("TicketNumber"),
                                            Description = dataRow.Field<string>("Description"),
                                            Priority = dataRow.Field<string>("Priority"),
                                            Statustxt = dataRow.Field<string>("Statustxt"),
                                            AccountName = dataRow.Field<string>("AccountName"),
                                            ModelName = dataRow.Field<string>("ModelName"),
                                            SystemNo = dataRow.Field<string>("SystemNo"),
                                            ProductName = dataRow.Field<string>("ProductName"),
                                            SerialNo = dataRow.Field<string>("SerialNo"),
                                            CreatedOn = dataRow.Field<DateTime>("CreatedOn")
                                        }).ToList();
                                        obj.TicketList = tickettlst;
                                    }
                                    else
                                        obj.TicketList = tickettlst;
                                }
                            }
                        }
                        obj.RoleId = roleid;
                        return View(obj);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Authenticate", "Authentication");
                    }
                }
            }
        }
        public ActionResult ServiceReportPV(long TicketNumber)
        {
            TicketDTO obj = new TicketDTO();
            obj.TicketNumber = TicketNumber;
            return PartialView("ServiceReportPreviewPV", obj);
        }
        public ActionResult NewEnquiry()
        {
            return View();
        }
        public async Task<ActionResult> AddEnquiry(string message)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        long userid = long.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        int comid = int.Parse(Session["SSCompanyId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                        TicketDTO obj = new TicketDTO();
                        obj.UserId = userid;
                        obj.CompanyId = comid;
                        obj.message = message;

                        bool status = false;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/TicketsAPI/NewEnquiry", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);
                            string msg = docs.message;
                            if (msg == "1")
                                status = true;
                            else
                            {
                                status = false;
                            };
                        }
                        return Json(new { success = status });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false });
                    }
                }
            }
        }
        public async Task<ActionResult> Enquiries()
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int userid = int.Parse(Session["SSUserId"].ToString());
                int roleid = int.Parse(Session["SSRoleId"].ToString());
                int comid = int.Parse(Session["SSCompanyId"].ToString());
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        TicketDTO obj = new TicketDTO();
                        obj.CompanyId = comid;
                        obj.OrganizationId = orgid;
                        obj.UserId = userid;
                        obj.RoleId = roleid;

                        List<TicketDTO> tickettlst = new List<TicketDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/TicketsAPI/NewEnquiryList", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);

                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        tickettlst = ds.Tables[0].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            EnquiryId = dataRow.Field<long>("EnquiryId"),
                                            Description = dataRow.Field<string>("Comments"),
                                            EnquiryDate = dataRow.Field<string>("EnquiryDate"),
                                            FullName = dataRow.Field<string>("FullName")
                                        }).ToList();
                                        obj.TicketList = tickettlst;
                                    }
                                    else
                                        obj.TicketList = tickettlst;
                                }
                            }
                        }
                        return View(obj);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
            }
        }
        public async Task<ActionResult> EnquiryDetails(long id)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                //return Json("../Login/Index");
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        int userid = int.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        int comid = int.Parse(Session["SSCompanyId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                        TicketDTO obj = new TicketDTO();
                        obj.EnquiryId = id;
                        obj.RoleId = roleid;
                        obj.CompanyId = comid;
                        obj.OrganizationId = orgid;

                        List<TicketDTO> tickettlst = new List<TicketDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/TicketsAPI/NewEnquiryDetails", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);

                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        tickettlst = ds.Tables[0].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            EnquiryId = dataRow.Field<long>("EnquiryId"),
                                            Description = dataRow.Field<string>("Comments"),
                                            EnquiryDate = dataRow.Field<string>("EnquiryDate"),
                                            FullName = dataRow.Field<string>("FullName"),
                                            Email = dataRow.Field<string>("Email"),
                                            Mobile = dataRow.Field<string>("Mobile"),
                                            EmpID = dataRow.Field<string>("EmpID"),
                                            commentsjson = dataRow.Field<string>("commentsjson")
                                        }).ToList();

                                        obj.TicketList = tickettlst;

                                        string commentsj = obj.TicketList.FirstOrDefault().commentsjson;
                                        var modalcomments = JsonConvert.DeserializeObject<List<TicketDTO>>(commentsj);
                                        obj.CommentsList = modalcomments;
                                    }
                                    else
                                        obj.TicketList = tickettlst;
                                }
                            }
                        }
                        return PartialView("EnquiryDetailsPV", obj);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
            }
        }
        public async Task<ActionResult> NewEnquiryComments(string txt, long EnquiryId)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        long userid = long.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        int comid = int.Parse(Session["SSCompanyId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());

                        TicketDTO obj = new TicketDTO();
                        obj.EnquiryId = EnquiryId;
                        obj.message = txt;
                        obj.UserId = userid;

                        bool status = false;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/TicketsAPI/NewEnquiryComments", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);
                            string msg = docs.message;
                            if (msg == "1")
                                status = true;
                            else
                            {
                                status = false;
                            };
                        }
                        return Json(new { success = status });
                    }
                    catch (Exception ex)
                    {
                        TicketDTO obj = new TicketDTO();
                        return Json(obj.ModelList);
                    }
                }
            }
        }
        public async Task<ActionResult> DashboardTickets(int id)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                //return Json("../Login/Index");
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        int userid = int.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        //int comid = int.Parse(Session["SSCompanyId"].ToString());
                        //int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                        TicketDTO obj = new TicketDTO();
                        //obj.RoleId = roleid;
                        //obj.CompanyId = comid;
                        //obj.OrganizationId = orgid;
                        obj.CreatedBy = userid;

                        List<TicketDTO> tickettlst = new List<TicketDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/TicketsAPI/NewDashBoardCount", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);

                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        tickettlst = ds.Tables[0].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            NewTickets = dataRow.Field<int>("NewTickets"),
                                            ReportsJson = dataRow.Field<string>("NewTicketsJson"),
                                            InProgressTickets = dataRow.Field<int>("InProgressTickets"),
                                            WarehouseJson = dataRow.Field<string>("InProgressTicketsJson"),
                                            ResolvedTickets = dataRow.Field<int>("ResolvedTickets"),
                                            SparePartRequestJson = dataRow.Field<string>("ResolvedTicketsJson")
                                        }).ToList();
                                        obj.TicketList = tickettlst;

                                        obj.NewTickets = obj.TicketList.FirstOrDefault().NewTickets;
                                        obj.InProgressTickets = obj.TicketList.FirstOrDefault().InProgressTickets;
                                        obj.ResolvedTickets = obj.TicketList.FirstOrDefault().ResolvedTickets;

                                        string accountsjson = obj.TicketList.FirstOrDefault().ReportsJson;
                                        var model = JsonConvert.DeserializeObject<List<TicketDTO>>(accountsjson);
                                        obj.ReportList = model;

                                        string warehousejson = obj.TicketList.FirstOrDefault().WarehouseJson;
                                        var modelwa = JsonConvert.DeserializeObject<List<TicketDTO>>(warehousejson);
                                        obj.WarehouseList = modelwa;

                                        string sparepart = obj.TicketList.FirstOrDefault().SparePartRequestJson;
                                        var modelsp = JsonConvert.DeserializeObject<List<TicketDTO>>(sparepart);
                                        obj.SparePartList = modelsp;
                                    }
                                    else
                                        obj.TicketList = tickettlst;
                                }
                            }
                        }
                        obj.RoleId = roleid;
                        // 1 for inprogress. 2 for actiontoclosed. 3 for closed
                        obj.val = id;
                        return View(obj);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadFile()
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return Json("../Login/Index");
            }
            else
            {
                int userid = int.Parse(Session["SSUserId"].ToString());
                //List<TicketDTO> multiple_images= new List<TicketDTO>();
                List<TicketDTO> multiple_images = (List<TicketDTO>)Session["MultipleImagesLst" + userid];


                string _imgname = string.Empty;
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var pic = System.Web.HttpContext.Current.Request.Files["MyImages"];
                    if (pic.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(pic.FileName);
                        var _ext = Path.GetExtension(pic.FileName);
                        _imgname = Guid.NewGuid().ToString();
                        var _comPath = Server.MapPath("~/TicketDocuments/") + _imgname + _ext;
                        _imgname = _imgname + _ext;
                        //ViewBag.Msg = _comPath;
                        var path = _comPath;

                        multiple_images.Add(new TicketDTO
                        {
                            Url = "/TicketDocuments/" + _imgname,
                            ContentType = _ext,
                            UniqueId = System.Web.HttpContext.Current.Request.Params["UniqueId"]
                        });

                        // Saving Image in Original Mode
                        pic.SaveAs(path);

                        //// resizing image
                        //MemoryStream ms = new MemoryStream();
                        //WebImage img = new WebImage(_comPath);

                        //if (img.Width > 200)
                        //    img.Resize(200, 200);
                        //img.Save(_comPath);
                        // end resize

                        Session["MultipleImagesLst" + userid] = multiple_images;
                    }
                }

                return Json(Convert.ToString(_imgname), JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult removeFile(string id)
        {
            int userid = int.Parse(Session["SSUserId"].ToString());
            int status = 1;
            List<TicketDTO> multiple_images = (List<TicketDTO>)Session["MultipleImagesLst" + userid];
            foreach (var item in multiple_images)
            {
                if (item.UniqueId == id)
                {
                    multiple_images.Remove(item);
                    break;
                }
            }
            Session["MultipleImagesLst" + userid] = null;
            Session["MultipleImagesLst" + userid] = multiple_images;
            return Json(new { success = status });
        }

        public async Task<ActionResult> ServiceReport(long id)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                //return Json("../Login/Index");
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        int userid = int.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        int comid = int.Parse(Session["SSCompanyId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                        TicketDTO obj = new TicketDTO();
                        obj.TicketNumber = id;
                        obj.RoleId = roleid;
                        obj.CompanyId = comid;
                        obj.OrganizationId = orgid;

                        List<TicketDTO> tickettlst = new List<TicketDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/TicketsAPI/NewGetTicketDetailsById", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);

                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        tickettlst = ds.Tables[0].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            TicketNumber = dataRow.Field<long>("TicketNumber"),
                                            Description = dataRow.Field<string>("Description"),
                                            Priority = dataRow.Field<string>("Priority"),
                                            Statustxt = dataRow.Field<string>("Status"),
                                            Status = dataRow.Field<int>("StatusId"),
                                            AccountName = dataRow.Field<string>("AccountName"),
                                            ModelName = dataRow.Field<string>("ModelName"),
                                            SystemNo = dataRow.Field<string>("SystemNo"),
                                            ProductName = dataRow.Field<string>("ProductName"),
                                            FullName = dataRow.Field<string>("FullName"),
                                            UserId = dataRow.Field<long>("UserId"),
                                            SerialNo = dataRow.Field<string>("SerialNo"),
                                            CreatedOn = dataRow.Field<DateTime>("CreatedOn"),
                                            Url = dataRow.Field<string>("Url"),
                                            //ContentType = dataRow.Field<string>("ContentType"),
                                            CreatedUser = dataRow.Field<string>("CreatedUser"),
                                            ReportsJson = dataRow.Field<string>("ReportJson"),
                                            RequestResponseStr = dataRow.Field<string>("RequestResponseStr"),
                                            ActualStartTime = dataRow.Field<string>("ActualStartTime"),
                                            ResolvedTime = dataRow.Field<string>("ResolvedTime"),
                                            MappedWarehouseId = dataRow.Field<int>("MappedWarehouseId"),
                                            WarehouseJson = dataRow.Field<string>("WarehouseJson"),
                                            SparePartRequestJson = dataRow.Field<string>("SparePartRequestJson"),
                                            StatusJson = dataRow.Field<string>("StatusJson"),
                                            commentsjson = dataRow.Field<string>("commentsjson"),
                                            Area = dataRow.Field<string>("Area"),
                                            CreatedUserId = dataRow.Field<long>("CreatedUserId"),
                                            Mobile = dataRow.Field<string>("Mobile"),
                                            Email = dataRow.Field<string>("Email"),
                                            POContract = dataRow.Field<string>("POContract"),
                                            InstallationDate = dataRow.Field<string>("InstallationDate"),
                                            IsContract = dataRow.Field<bool>("IsContract"),
                                            WarrantyExpiryDate = dataRow.Field<string>("WarrantyExpiryDate"),
                                            PPMDate = dataRow.Field<string>("PPMDate"),
                                            ServiceStartDate = dataRow.Field<string>("ServiceStartDate"),
                                            ReportTypeName = dataRow.Field<string>("ReportTypeName"),
                                            ServiceEngineerResolvedDate = dataRow.Field<string>("ServiceEngineerResolvedDate"),
                                            CustomerConfirmationDate = dataRow.Field<string>("CustomerConfirmationDate"),
                                            ManagerConfirmationDate = dataRow.Field<string>("ManagerConfirmationDate"),
                                            ManagerName = dataRow.Field<string>("ManagerName"),
                                            Actioncomments = dataRow.Field<string>("Actioncomments"),
                                            ProblemDescription = dataRow.Field<string>("ProblemDescription"),
                                            WorkHours = dataRow.Field<string>("WorkHours"),
                                            ReportId = dataRow.Field<int>("ReportTypeId"),
                                            CreatedUserRoleId = dataRow.Field<int>("CreatedUserRoleId"),
                                            SupervisorConfirmationDate = dataRow.Field<string>("SupervisorConfirmationDate"),
                                            SupervisorName = dataRow.Field<string>("SupervisorName"),
                                            ServiceEngineerJson = dataRow.Field<string>("ServiceEngineerJson"),
                                            StationName = dataRow.Field<string>("StationName"),
                                            AccountCode = dataRow.Field<string>("AccountCode"),
                                            ManagerFullName = dataRow.Field<string>("ManagerFullName"),
                                            ManagerMobile = dataRow.Field<string>("ManagerMobile"),
                                            ManagerEmail = dataRow.Field<string>("ManagerEmail")
                                        }).ToList();

                                        obj.TicketList = tickettlst;

                                        string urlsjson = obj.TicketList.FirstOrDefault().Url;
                                        var modelurl = JsonConvert.DeserializeObject<List<TicketDTO>>(urlsjson);
                                        obj.UrlList = modelurl;

                                        string accountsjson = obj.TicketList.FirstOrDefault().ReportsJson;
                                        var model = JsonConvert.DeserializeObject<List<TicketDTO>>(accountsjson);
                                        obj.ReportList = model;

                                        string warehousejson = obj.TicketList.FirstOrDefault().WarehouseJson;
                                        var modelwa = JsonConvert.DeserializeObject<List<TicketDTO>>(warehousejson);
                                        obj.WarehouseList = modelwa;

                                        string sparepart = obj.TicketList.FirstOrDefault().SparePartRequestJson;
                                        var modelsp = JsonConvert.DeserializeObject<List<TicketDTO>>(sparepart);
                                        obj.SparePartList = modelsp;

                                        string statusj = obj.TicketList.FirstOrDefault().StatusJson;
                                        var modelstaj = JsonConvert.DeserializeObject<List<TicketDTO>>(statusj);
                                        obj.StatusLst = modelstaj;

                                        string commentsj = obj.TicketList.FirstOrDefault().commentsjson;
                                        var modalcomments = JsonConvert.DeserializeObject<List<TicketDTO>>(commentsj);
                                        obj.CommentsList = modalcomments;

                                        string serviceenglst = obj.TicketList.FirstOrDefault().ServiceEngineerJson;
                                        var modalserviceLst = JsonConvert.DeserializeObject<List<TicketDTO>>(serviceenglst);
                                        obj.ServiceEngineerList = modalserviceLst;

                                        //ServiceEngineerList
                                    }
                                    else
                                        obj.TicketList = tickettlst;
                                }
                            }
                        }
                        return View(obj);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
            }
        }

        public async Task<ActionResult> TicketTransfer(long ServiceEngineerId, long TicketNumber)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        long CreatedBy = long.Parse(Session["SSUserId"].ToString());
                        TicketDTO obj = new TicketDTO();
                        obj.CreatedBy = CreatedBy;
                        obj.TicketNumber = TicketNumber;
                        obj.UserId = ServiceEngineerId;

                        bool status = false;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/TicketsAPI/NewTransferTicket", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);
                            string msg = docs.message;
                            if (msg == "1")
                                status = true;
                            else if (msg == "2")
                            {
                                status = true;
                            };
                        }
                        return Json(new { success = status });
                    }
                    catch (Exception ex)
                    {
                        TicketDTO obj = new TicketDTO();
                        return Json(obj.ModelList);
                    }
                }
            }
        }
        public async Task<ActionResult> RejectedTickets()
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int userid = int.Parse(Session["SSUserId"].ToString());
                int roleid = int.Parse(Session["SSRoleId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        TicketDTO obj = new TicketDTO();

                        obj.CreatedBy = userid;
                        obj.RoleId = roleid;

                        List<TicketDTO> tickettlst = new List<TicketDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/TicketsAPI/NewRejectedTickets", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);

                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        tickettlst = ds.Tables[0].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            TicketNumber = dataRow.Field<long>("TicketNumber"),
                                            Description = dataRow.Field<string>("Description"),
                                            Priority = dataRow.Field<string>("Priority"),
                                            Statustxt = dataRow.Field<string>("Status"),
                                            AccountName = dataRow.Field<string>("AccountName"),
                                            ModelName = dataRow.Field<string>("ModelName"),
                                            SystemNo = dataRow.Field<string>("SystemNo"),
                                            ProductName = dataRow.Field<string>("ProductName"),
                                            FullName = dataRow.Field<string>("FullName"),
                                            UserId = dataRow.Field<long>("UserId"),
                                            CreatedOn = dataRow.Field<DateTime>("CreatedOn"),
                                            Area = dataRow.Field<string>("Area"),
                                            ReportTypeName = dataRow.Field<string>("ReportTypeName")
                                        }).ToList();
                                        obj.TicketList = tickettlst;
                                    }
                                    else
                                        obj.TicketList = tickettlst;
                                }
                            }
                        }


                        return View(obj);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Authenticate", "Authentication");
                    }
                }
            }
        }

        public async Task<ActionResult> SystemManagerId(int ProductId, int AccountId)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        long CreatedBy = long.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());

                        TicketDTO obj = new TicketDTO();
                        obj.CreatedBy = CreatedBy;
                        obj.ProductId = ProductId;
                        obj.AccountId = AccountId;

                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/TicketsAPI/NewSystemManagerId", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);
                            if (roleid == 504)
                            {
                                obj.SystemManagerId = int.Parse(CreatedBy.ToString());
                            }
                            else
                            {
                                obj.SystemManagerId = docs.SystemManagerId;
                            }


                        }
                        return Json(new { SystemManagerId = obj.SystemManagerId });
                    }
                    catch (Exception ex)
                    {
                        TicketDTO obj = new TicketDTO();
                        return Json(obj.ModelList);
                    }
                }
            }
        }

        #region Report
        public async Task<ActionResult> GetProjectList()
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    TicketDTO obj = new TicketDTO();

                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/TicketsAPI/NewCrmRawData", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var response = JObject.Parse(responseMessage.Content.ReadAsStringAsync().Result);
                        var Data = response.SelectToken("message");
                        var lstReportLists = JsonConvert.DeserializeObject<List<TicketDTO>>(Data.ToString());

                        List<TicketDTO> RawDetails = new List<TicketDTO>();
                        if (response != null)
                        {
                            //var CategoryData = response.SelectToken("message");

                            if (lstReportLists != null)
                            {
                                obj.RawDataReportList = lstReportLists;
                            }
                            ReportViewer reportViewer = new ReportViewer();
                            reportViewer.ProcessingMode = ProcessingMode.Local;
                            reportViewer.SizeToReportContent = true;
                            reportViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/CRMRawdataReport.rdlc");

                            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("RawDataReportDS", obj.RawDataReportList));
                            ViewBag.ReportViewer = reportViewer;

                        }
                    }
                    return View();
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error");
                }
            }

        }
        #endregion
    }
}