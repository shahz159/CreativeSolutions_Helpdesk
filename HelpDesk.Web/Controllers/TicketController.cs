using HelpDesk.Web.Handlers;
using HelpDesk.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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
                        obj.CompanyId = comid;
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
                                            ModelName = dataRow.Field<string>("ModelName"),
                                            AMId = dataRow.Field<int>("AMId")
                                        }).ToList();

                                        List<TicketDTO> _objlst = _objStudew;
                                        ddlmodels = new SelectList(_objlst, "AMId", "ModelName", obj.AMId);
                                        ViewData["ddlModels"] = ddlmodels;
                                    }
                                    else
                                    {
                                        List<TicketDTO> _objStudent = _objStudew;
                                        ddlmodels = new SelectList(_objStudent, "AMId", "ModelName", obj.AMId);
                                        ViewData["ddlModels"] = ddlmodels;
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
        public async Task<ActionResult> NewTicket(TicketDTO obj, IEnumerable<HttpPostedFileBase> TicketDocument)
        {
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

                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                         
                        if (TicketDocument != null)
                        {
                            var xmldoc_docs = new XmlDocument();
                            var parentelemeng_docs = xmldoc_docs.CreateElement("MultiDocuments");
                            var parent_docs = xmldoc_docs.CreateElement("MultiDocument");

                            foreach (HttpPostedFileBase item in TicketDocument)
                            {
                                if (item==null)
                                {
                                    obj.ContentType = "";
                                    obj.Url = "";
                                    obj.multipledocuments_xml = "";
                                    break;
                                }
                                var parentelement = xmldoc_docs.CreateElement("Row");
                                var filepath_xml = xmldoc_docs.CreateElement("filepath");
                                var ContentType_xml = xmldoc_docs.CreateElement("ContentType");
                                var UniqueId_xml = xmldoc_docs.CreateElement("uniqueid");



                                obj.ContentType = Path.GetExtension(item.FileName);
                                var ext = Path.GetExtension(item.FileName);
                                string uniqueid = Guid.NewGuid().ToString();
                                string targetPath = Server.MapPath("~/TicketDocuments/" + uniqueid + ext);
                                Stream strm = item.InputStream;
                                var targetFile = Path.GetFullPath(item.FileName);
                                obj.Url = targetPath;
                                item.SaveAs(targetPath);
                                obj.Url = "/TicketDocuments/" + uniqueid + ext;


                                filepath_xml.InnerText = "/TicketDocuments/" + uniqueid + ext;
                                ContentType_xml.InnerText = ext;
                                UniqueId_xml.InnerText = uniqueid;

                                parentelement.AppendChild(filepath_xml);
                                parentelement.AppendChild(ContentType_xml);
                                parentelement.AppendChild(UniqueId_xml);

                                parentelemeng_docs.AppendChild(parent_docs);
                                parent_docs.AppendChild(parentelement);
                            }

                            obj.multipledocuments_xml = parentelemeng_docs.InnerXml;
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
                        if (accntid == 0)
                            obj.AccountId = obj.AccountId;
                        else
                            obj.AccountId = accntid;

                        if (obj.Description == null)
                            obj.Description = "";

                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/TicketsAPI/NewInsertTicketRequest", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var tickets = JsonConvert.DeserializeObject<TicketDTO>(responseData);
                            obj.message = tickets.message;
                            string msg = obj.message;
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
                                            ProductId = dataRow.Field<int>("ProductId")
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
                                            MappedWarehouseId = dataRow.Field<int>("MappedWarehouseId"),
                                            WarehouseJson = dataRow.Field<string>("WarehouseJson"),
                                            SparePartRequestJson = dataRow.Field<string>("SparePartRequestJson"),
                                            StatusJson = dataRow.Field<string>("StatusJson"),
                                            commentsjson = dataRow.Field<string>("commentsjson"),
                                            Area = dataRow.Field<string>("Area")
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
                                            Area = dataRow.Field<string>("Area")
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
                        obj.CompanyId = comid;
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

                        if (roleid == 501 || roleid == 502)
                            obj.CreatedBy = 0;

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
                        if (roleid == 501 || roleid == 502)
                            obj.CreatedBy = 0;
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
                                            CreatedOn = dataRow.Field<DateTime>("CreatedOn")
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
        public async Task<ActionResult> UpdateTicketStatus(int id, long TicketNumber)
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
                        obj.Comments = "";

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
        public async Task<ActionResult> AddResponseTime(string ResponseTime, int ReportId, long TicketNumber)
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
                        obj.ReportId = ReportId;

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

        public async Task<ActionResult> GetSparePartList(int warehouseid)
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
    }
}