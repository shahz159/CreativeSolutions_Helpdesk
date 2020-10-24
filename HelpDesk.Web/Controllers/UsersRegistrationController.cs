using HelpDesk.Web.Handlers;
using HelpDesk.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace HelpDesk.Web.Controllers
{
    public class UsersRegistrationController : Controller
    {
        // GET: UsersRegistration
        public async Task<ActionResult> Index()
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                int comid = int.Parse(Session["SSCompanyId"].ToString());
                int roleid = int.Parse(Session["SSRoleId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        UserDTO obj = new UserDTO();
                        obj.OrganizationId = orgid;
                        obj.CompanyId = comid;

                        List<UserDTO> rolelst = new List<UserDTO>();
                        List<UserDTO> companylst = new List<UserDTO>();
                        List<UserDTO> productlst = new List<UserDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/UserAPI/NewGetDropDowns", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<UserDTO>(responseData);

                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    SelectList ddlroles = new SelectList("", "RoleId", "RoleName", 0);
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        rolelst = ds.Tables[0].AsEnumerable().Select(dataRow => new UserDTO
                                        {
                                            RoleName = dataRow.Field<string>("RoleName"),
                                            RoleId = dataRow.Field<int>("RoleId")
                                        }).ToList();
                                        List<UserDTO> _objroles = rolelst;
                                        ddlroles = new SelectList(_objroles, "RoleId", "RoleName", obj.RoleId);
                                        ViewData["ddlRoleLst"] = ddlroles;
                                    }
                                    else
                                    {
                                        List<UserDTO> _objroles = rolelst;
                                        ddlroles = new SelectList(_objroles, "RoleId", "RoleName", obj.RoleId);
                                        ViewData["ddlRoleLst"] = ddlroles;
                                    }

                                    SelectList ddlcompany = new SelectList("", "CompanyId", "CompanyName", 0);
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        companylst = ds.Tables[1].AsEnumerable().Select(dataRow => new UserDTO
                                        {
                                            CompanyName = dataRow.Field<string>("CompanyName"),
                                            CompanyId = dataRow.Field<int>("CompanyId")
                                        }).ToList();
                                        List<UserDTO> _objcompany = companylst;
                                        ddlcompany = new SelectList(_objcompany, "CompanyId", "CompanyName", obj.CompanyId);
                                        ViewData["ddlCompanyLst"] = ddlcompany;
                                    }
                                    else
                                    {
                                        List<UserDTO> _objcompany = rolelst;
                                        ddlcompany = new SelectList(_objcompany, "CompanyId", "CompanyName", obj.CompanyId);
                                        ViewData["ddlCompanyLst"] = ddlcompany;
                                    }

                                    SelectList ddlproduct = new SelectList("", "ProductId", "ProductName", 0);
                                    if (ds.Tables[2].Rows.Count > 0)
                                    {
                                        productlst = ds.Tables[2].AsEnumerable().Select(dataRow => new UserDTO
                                        {
                                            ProductName = dataRow.Field<string>("ProductName"),
                                            ProductId = dataRow.Field<int>("ProductId")
                                        }).ToList();
                                        List<UserDTO> _objcompany = productlst;
                                        ddlproduct = new SelectList(_objcompany, "ProductId", "ProductName", obj.ProductId);
                                        ViewData["ddlProductLst"] = ddlproduct;
                                    }
                                    else
                                    {
                                        List<UserDTO> _objcompany = productlst;
                                        ddlproduct = new SelectList(_objcompany, "ProductId", "ProductName", obj.ProductId);
                                        ViewData["ddlProductLst"] = ddlproduct;
                                    }
                                }
                            }
                            SelectList ddlaccounts = new SelectList("", "AccountId", "AccountName", 0);
                            List<UserDTO> _objStudent = rolelst;
                            ddlaccounts = new SelectList(_objStudent, "AccountId", "AccountName", obj.AccountId);
                            ViewData["ddlAccountLst"] = ddlaccounts;

                            var list = new List<SelectListItem>
    {
        new SelectListItem{ Text="Male", Value = "M" },
        new SelectListItem{ Text="Female", Value = "F" },
        new SelectListItem{ Text="Other", Value = "O", Selected = true },
    };

                            ViewData["ddlGender"] = list;

                        }
                        obj.RoleId = roleid;
                        obj.CompanyId = comid;
                        return View(obj);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Authenticate", "Authentication");
                    }
                }
            }
        }
        public async Task<ActionResult> Users()
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int comid = int.Parse(Session["SSCompanyId"].ToString());
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                int userid = int.Parse(Session["SSUserId"].ToString());

                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        UserDTO obj = new UserDTO();
                        obj.CompanyId = comid;
                        obj.OrganizationId = orgid;
                        obj.CreatedBy = userid;

                        List<UserDTO> userslst = new List<UserDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/UserAPI/NewGetUserList", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<UserDTO>(responseData);

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
                                        userslst = ds.Tables[0].AsEnumerable().Select(dataRow => new UserDTO
                                        {
                                            RoleId = dataRow.Field<int>("RoleId"),
                                            RoleName = dataRow.Field<string>("RoleName"),
                                            EmpId = dataRow.Field<string>("EmpId"),
                                            FullName = dataRow.Field<string>("FullName"),
                                            Gender = dataRow.Field<string>("Gender"),
                                            Mobile = dataRow.Field<string>("Mobile"),
                                            Email = dataRow.Field<string>("Email"),
                                            Password = dataRow.Field<string>("Password"),
                                            isApproved = dataRow.Field<bool>("isApproved"),
                                            isActive = dataRow.Field<bool>("isActive"),
                                            OrganizationId = dataRow.Field<int>("OrganizationId"),
                                            CompanyId = dataRow.Field<int>("CompanyId"),
                                            CompanyName = dataRow.Field<string>("CompanyName"),
                                            Accountsxml = dataRow.Field<string>("Accountsxml"),
                                            UserId=dataRow.Field<long>("UserId")

                                        }).ToList();
                                        obj.UsersList = userslst;
                                    }
                                    else
                                        obj.UsersList = userslst;
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
        public async Task<JsonResult> GetAccounts(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    UserDTO obj = new UserDTO();
                    obj.CompanyId = id;
                   
                    List<UserDTO> modellst = new List<UserDTO>(); 
                    SelectList ddlaccounts = new SelectList("", "AccountId", "AccountName", 0);
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/UserAPI/NewGetAccountList", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var model = JsonConvert.DeserializeObject<List<AssetsDTO>>(responseData);
                        List<AssetsDTO> _objModelLst = model;
                        ddlaccounts = new SelectList(_objModelLst, "AccountId", "AccountName", 0);
                    }
                    return Json(new SelectList(ddlaccounts, "Value", "Text"));
                }
                catch (Exception ex)
                {
                    TicketDTO obj = new TicketDTO();
                    return Json(obj.ProductList);
                }
            }
        }
        public async Task<JsonResult> GetRoleAccounts(int id,int RoleId)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return Json("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        UserDTO obj = new UserDTO();
                        obj.CompanyId = id;
                        obj.RoleId = RoleId;

                        List<UserDTO> modellst = new List<UserDTO>();
                        SelectList ddlaccounts = new SelectList("", "AccountId", "AccountName", 0);
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/UserAPI/NewGetManagerAccountList", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var model = JsonConvert.DeserializeObject<List<AssetsDTO>>(responseData);
                            List<AssetsDTO> _objModelLst = model;
                            ddlaccounts = new SelectList(_objModelLst, "AccountId", "AccountName", 0);
                        }
                        return Json(new SelectList(ddlaccounts, "Value", "Text"));
                    }
                    catch (Exception ex)
                    {
                        TicketDTO obj = new TicketDTO();
                        return Json(obj.ProductList);
                    }
                }
              
            }
        }
        //NewUser
        [HttpPost]
        public async Task<ActionResult> NewUser(UserDTO obj)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int userid = int.Parse(Session["SSUserId"].ToString());
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        obj.OrganizationId = orgid;
                        obj.CreatedBy = userid;
                        obj.Password = "12345";
                        var xmldoc_docs = new XmlDocument();
                        var parentelemeng_docs = xmldoc_docs.CreateElement("MultiAccounts");
                        var parent_docs = xmldoc_docs.CreateElement("MultiAccount");

                        for (int i = 0; i < obj.Accounts.Length; i++)
                        {
                            var parentelement = xmldoc_docs.CreateElement("Row");
                            var accountid_xml = xmldoc_docs.CreateElement("AccountId");

                            accountid_xml.InnerText = obj.Accounts[i];
                            parentelement.AppendChild(accountid_xml);

                            parentelemeng_docs.AppendChild(parent_docs);
                            parent_docs.AppendChild(parentelement);
                        }
                        obj.Accountsxml = parentelemeng_docs.InnerXml;

                        var xmldoc_docs_p = new XmlDocument();
                        var parentelemeng_docs_p = xmldoc_docs_p.CreateElement("MultiProducts");
                        var parent_docs_p = xmldoc_docs_p.CreateElement("MultiProduct");

                        for (int i = 0; i < obj.Products.Length; i++)
                        {
                            var parentelement_p = xmldoc_docs_p.CreateElement("Row");
                            var productid_xml_p = xmldoc_docs_p.CreateElement("ProductId");

                            productid_xml_p.InnerText = obj.Products[i];
                            parentelement_p.AppendChild(productid_xml_p);

                            parentelemeng_docs_p.AppendChild(parent_docs_p);
                            parent_docs_p.AppendChild(parentelement_p);
                        }
                        obj.Productsxml = parentelemeng_docs_p.InnerXml;
                        obj.Gender = obj.Gender.Substring(0, 1);
                        obj.isActive = true;
                        obj.isApproved = true;
                        obj.SignUp = false;
                        bool status = false;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/UserAPI/NewInsertUser", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var categories = JsonConvert.DeserializeObject<AccountsDTO>(responseData);
                            string msg = categories.message;
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
                        return View();
                    }
                }
            }
        }
        public async Task<JsonResult> GetProducts(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    UserDTO obj = new UserDTO();
                    obj.CompanyId = id;
                    List<UserDTO> modellst = new List<UserDTO>();
                    SelectList ddlproducts = new SelectList("", "ProductId", "ProductName", 0);
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/UserAPI/NewGetProductList", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var model = JsonConvert.DeserializeObject<List<AssetsDTO>>(responseData);
                        List<AssetsDTO> _objModelLst = model;
                        ddlproducts = new SelectList(_objModelLst, "ProductId", "ProductName", 0);
                    }
                    return Json(new SelectList(ddlproducts, "Value", "Text"));
                }
                catch (Exception ex)
                {
                    TicketDTO obj = new TicketDTO();
                    return Json(obj.ProductList);
                }
            }
        }
        public async Task<JsonResult> GetRoleProducts(int id,int RoleId,int AccountId)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    UserDTO obj = new UserDTO();
                    obj.CompanyId = id;
                    obj.RoleId = RoleId;
                    obj.AccountId = AccountId;

                    List<UserDTO> modellst = new List<UserDTO>();
                    SelectList ddlproducts = new SelectList("", "ProductId", "ProductName", 0);
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/UserAPI/NewGetProductListRolwWise", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var model = JsonConvert.DeserializeObject<List<AssetsDTO>>(responseData);
                        List<AssetsDTO> _objModelLst = model;
                        ddlproducts = new SelectList(_objModelLst, "ProductId", "ProductName", 0);
                    }
                    return Json(new SelectList(ddlproducts, "Value", "Text"));
                }
                catch (Exception ex)
                {
                    TicketDTO obj = new TicketDTO();
                    return Json(obj.ProductList);
                }
            }
        }
        public async Task<JsonResult> GetModels(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    AssetsDTO obj = new AssetsDTO();
                    obj.ProductId = id;
                    SelectList ddlmodels = new SelectList("", "ModelId", "ModelName", 0);
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/AssetAPI/NewGetModelList", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var model = JsonConvert.DeserializeObject<List<AssetsDTO>>(responseData);
                        List<AssetsDTO> _objModelLst = model.Where(x => x.isActive == true).ToList();
                        ddlmodels = new SelectList(_objModelLst, "ModelId", "ModelName", 0);
                    }
                    return Json(new SelectList(ddlmodels, "Value", "Text"));
                }
                catch (Exception ex)
                {
                    return Json(new SelectList("", "Value", "Text"));
                }
            }

        }
        public async Task<JsonResult> CheckUserEmail(string useremail)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    bool status = false;
                    HttpResponseMessage responseMessage = await client.GetAsync("api/UserAPI/GetCheckEmailExists?email=" + useremail);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var check = JsonConvert.DeserializeObject<UserDTO>(responseData);
                        string msg = check.message;
                        if (msg == "1")
                            status = true;
                        else if (msg == "2")
                        {
                            UserDTO model = new UserDTO();
                            status = false;

                        };
                        return Json(new { success = status });
                    }
                    else
                    {
                        var result = "3";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(new SelectList("", "Value", "Text"));
                }
            }
        }
        public async Task<JsonResult> CheckUserEmployeeId(string useremail)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    bool status = false;
                    HttpResponseMessage responseMessage = await client.GetAsync("api/UserAPI/GetCheckEmpidExists?email=" + useremail);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var check = JsonConvert.DeserializeObject<UserDTO>(responseData);
                        string msg = check.message;
                        if (msg == "1")
                            status = true;
                        else if (msg == "2")
                        {
                            UserDTO model = new UserDTO();
                            status = false;

                        };
                        return Json(new { success = status });
                    }
                    else
                    {
                        var result = "3";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(new SelectList("", "Value", "Text"));
                }
            }
        }
        public async Task<ActionResult> UserDetails(long id) 
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                int comid = int.Parse(Session["SSCompanyId"].ToString());
                int roleid = int.Parse(Session["SSRoleId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        UserDTO obj = new UserDTO();
                        obj.UserId = id;

                        List<UserDTO> rolelst = new List<UserDTO>();
                        List<UserDTO> companylst = new List<UserDTO>();
                        List<UserDTO> productlst = new List<UserDTO>();

                        List<UserDTO> userdetailslst = new List<UserDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/UserAPI/NewGetUserDetailsById", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<UserDTO>(responseData);

                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    //userdetailslst
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        userdetailslst = ds.Tables[0].AsEnumerable().Select(dataRow => new UserDTO
                                        {
                                            RoleId = dataRow.Field<int>("RoleId"),
                                            RoleName = dataRow.Field<string>("RoleName"),
                                            EmpId = dataRow.Field<string>("EmpId"),
                                            FullName = dataRow.Field<string>("FullName"),
                                            Gender = dataRow.Field<string>("Gender"),
                                            Mobile = dataRow.Field<string>("Mobile"),
                                            Email = dataRow.Field<string>("Email"),
                                            Password = dataRow.Field<string>("Password"),
                                            isApproved = dataRow.Field<bool>("isApproved"),
                                            isActive = dataRow.Field<bool>("isActive"),
                                            OrganizationId = dataRow.Field<int>("OrganizationId"),
                                            CompanyId = dataRow.Field<int>("CompanyId"),
                                            CompanyName = dataRow.Field<string>("CompanyName"),
                                            Accountsxml = dataRow.Field<string>("AccountsJson"),
                                            Productsxml = dataRow.Field<string>("ProductsJson"),
                                            SignUp = dataRow.Field<bool>("SignUp"),
                                            UserId = dataRow.Field<long>("UserId"),
                                            ProductName = dataRow.Field<string>("ProductsddlJson"),
                                            AccountName = dataRow.Field<string>("AccountsddlJson"),
                                        }).ToList();
                                        obj.UsersList = userdetailslst;
                                        
                                        string accountsjson = obj.UsersList.FirstOrDefault().Accountsxml;
                                        var model = JsonConvert.DeserializeObject<List<UserDTO>>(accountsjson);



                                        obj.AccountList = model;

                                        string productsjson = obj.UsersList.FirstOrDefault().Productsxml;
                                        var model_pro = JsonConvert.DeserializeObject<List<UserDTO>>(productsjson);
                                        obj.ProductList = model_pro;


                                        string accountsjsonddl = obj.UsersList.FirstOrDefault().AccountName;
                                        var modelddl = JsonConvert.DeserializeObject<List<UserDTO>>(accountsjsonddl);
                                        obj.AccountddlList = modelddl;

                                        string productsjsonddl = obj.UsersList.FirstOrDefault().ProductName;
                                        var model_prpddl = JsonConvert.DeserializeObject<List<UserDTO>>(productsjsonddl);
                                        obj.ProductddlList = model_prpddl;
                                    }

                                    else
                                        obj.UsersList = userdetailslst;
                                }
                            }
                            SelectList ddlaccounts = new SelectList("", "AccountId", "AccountName", 0);
                            List<UserDTO> _objStudent = rolelst;
                            ddlaccounts = new SelectList(_objStudent, "AccountId", "AccountName", obj.AccountId);
                            ViewData["ddlAccountLst"] = ddlaccounts;

                        }
                        obj.RoleId = roleid;
                        obj.CompanyId = comid;
                        return PartialView("DetailsPV", obj);
                        //return View(obj);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Authenticate", "Authentication");
                    }
                }
            }
            
        }
        public async Task<ActionResult> SignupUsers()
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int comid = int.Parse(Session["SSCompanyId"].ToString());
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                int userid = int.Parse(Session["SSUserId"].ToString());

                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        UserDTO obj = new UserDTO();
                        obj.CompanyId = comid;
                        obj.OrganizationId = orgid;
                        obj.CreatedBy = userid;

                        List<UserDTO> userslst = new List<UserDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/UserAPI/NewGetSystemUsersforApproval", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<UserDTO>(responseData);

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
                                        userslst = ds.Tables[0].AsEnumerable().Select(dataRow => new UserDTO
                                        {
                                            UserId = dataRow.Field<long>("UserId"),
                                            FullName = dataRow.Field<string>("FullName"),
                                            Gender = dataRow.Field<string>("Gender"),
                                            Mobile = dataRow.Field<string>("Mobile"),
                                            Email = dataRow.Field<string>("Email"),
                                            CreatedOn = dataRow.Field<DateTime>("CreatedOn"),
                                            AccountName = dataRow.Field<string>("AccountName"),
                                            AccountId = dataRow.Field<int>("AccountId"),
                                            AccountCode = dataRow.Field<string>("AccountCode") 
                                        }).ToList();
                                        obj.UsersList = userslst;
                                    }
                                    else
                                        obj.UsersList = userslst;
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
        public async Task<JsonResult> UpdateUserStatus(int id,long userid)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    UserDTO obj = new UserDTO();
                    obj.UserId = userid;
                    if (id==1)
                    {
                        obj.isApproved = true;
                        obj.Cancelled = false;
                    }
                    else if (id == 2)
                    {
                        obj.isApproved = false;
                        obj.Cancelled = true;
                    }
                    bool status = false;
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/UserAPI/NewUpdateUserStatus", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var model = JsonConvert.DeserializeObject<AssetsDTO>(responseData);
                        string msg = model.message;
                        if (msg == "1")
                            status = true;
                        else if (msg == "2")
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
        /// <summary>
        /// type differentiate for account and product
        /// 1 for account and 2 for product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<JsonResult> removeAccountOrProduct(long id,int type)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    UserDTO obj = new UserDTO();
                    
                    obj.MUPId = id;
                    obj.Type = type;

                    bool status = false;
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/UserAPI/NewRemoveAccountOrProduct", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var model = JsonConvert.DeserializeObject<AssetsDTO>(responseData);
                        string msg = model.message;
                        if (msg == "1")
                            status = true;
                        else if (msg == "2")
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
        public async Task<JsonResult> AddProducts(long id,int ProductId)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    UserDTO obj = new UserDTO();
                    obj.UserId = id;
                    obj.ProductId = ProductId;
                    bool status = false;
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/UserAPI/NewAddUserProduct", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var model = JsonConvert.DeserializeObject<AssetsDTO>(responseData);
                        string msg = model.message;
                        if (msg == "1")
                            status = true;
                        else if (msg == "2")
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
        public async Task<JsonResult> AddAccounts(long id, int AccountId)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    UserDTO obj = new UserDTO();
                    obj.UserId = id;
                    obj.AccountId = AccountId;
                    bool status = false;
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/UserAPI/NewAddUserAccount", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var model = JsonConvert.DeserializeObject<AssetsDTO>(responseData);
                        string msg = model.message;
                        if (msg == "1")
                            status = true;
                        else if (msg == "2")
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

        public async Task<JsonResult> UpdateUserInfo(string fullname, string gender,string mobileno,long userid,int roleid)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    UserDTO obj = new UserDTO();
                    obj.UserId = userid;
                    obj.FullName = fullname;
                    obj.Gender = gender;
                    obj.Mobile = mobileno;
                    obj.RoleId = roleid;

                    bool status = false;
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/UserAPI/NewUpdateUserInfo", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var model = JsonConvert.DeserializeObject<AssetsDTO>(responseData);
                        string msg = model.message;
                        if (msg == "1")
                            status = true;
                        else if (msg == "2")
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

        public async Task<JsonResult> UserStatusAPPRJCT(long userid, int val)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return Json("Index", "Login");
            }
            else
            {
                int userid_a = int.Parse(Session["SSUserId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        UserDTO obj = new UserDTO();
                        obj.UserId = userid;

                        if (val == 1)
                        {
                            obj.isApproved = true;
                            obj.isCancelled = false;
                            obj.isActive = true;
                        }
                        else if (val == 2)
                        {
                            obj.isApproved = false;
                            obj.isCancelled = true;
                            obj.isActive = false;
                        }
                        obj.CreatedBy = userid_a;
                        bool status = false;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/UserAPI/NewUpdateSignUpUserStatus", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var model = JsonConvert.DeserializeObject<AssetsDTO>(responseData);
                            string msg = model.message;
                            if (msg == "1")
                                status = true;
                            else if (msg == "2")
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

        public async Task<JsonResult> UserStatusIsActive(long userid)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return Json("Index", "Login");
            }
            else
            {
                int userid_a = int.Parse(Session["SSUserId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        UserDTO obj = new UserDTO();
                        obj.UserId = userid;
                        obj.CreatedBy = userid_a;
                        bool status = false;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/UserAPI/NewUpdateUserStatusActive", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var model = JsonConvert.DeserializeObject<AssetsDTO>(responseData);
                            string msg = model.message;
                            if (msg == "1")
                                status = true;
                            else if (msg == "2")
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




        public async Task<ActionResult> Profile()
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int userid = int.Parse(Session["SSUserId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        UserDTO obj = new UserDTO();
                        obj.UserId = userid;

                        List<UserDTO> rolelst = new List<UserDTO>();
                        List<UserDTO> companylst = new List<UserDTO>();
                        List<UserDTO> productlst = new List<UserDTO>();

                        List<UserDTO> userdetailslst = new List<UserDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/UserAPI/NewGetUserDetailsById", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<UserDTO>(responseData);

                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    //userdetailslst
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        userdetailslst = ds.Tables[0].AsEnumerable().Select(dataRow => new UserDTO
                                        {
                                            RoleId = dataRow.Field<int>("RoleId"),
                                            RoleName = dataRow.Field<string>("RoleName"),
                                            EmpId = dataRow.Field<string>("EmpId"),
                                            FullName = dataRow.Field<string>("FullName"),
                                            Gender = dataRow.Field<string>("Gender"),
                                            Mobile = dataRow.Field<string>("Mobile"),
                                            Email = dataRow.Field<string>("Email"),
                                            Password = dataRow.Field<string>("Password"),
                                            isApproved = dataRow.Field<bool>("isApproved"),
                                            isActive = dataRow.Field<bool>("isActive"),
                                            OrganizationId = dataRow.Field<int>("OrganizationId"),
                                            CompanyId = dataRow.Field<int>("CompanyId"),
                                            CompanyName = dataRow.Field<string>("CompanyName"),
                                            Accountsxml = dataRow.Field<string>("AccountsJson"),
                                            Productsxml = dataRow.Field<string>("ProductsJson"),
                                            SignUp = dataRow.Field<bool>("SignUp"),
                                            UserId = dataRow.Field<long>("UserId"),
                                            ProductName = dataRow.Field<string>("ProductsddlJson"),
                                            AccountName = dataRow.Field<string>("AccountsddlJson"),
                                        }).ToList();
                                        obj.UsersList = userdetailslst;

                                        string accountsjson = obj.UsersList.FirstOrDefault().Accountsxml;
                                        var model = JsonConvert.DeserializeObject<List<UserDTO>>(accountsjson);
                                        obj.AccountList = model;

                                        string productsjson = obj.UsersList.FirstOrDefault().Productsxml;
                                        var model_pro = JsonConvert.DeserializeObject<List<UserDTO>>(productsjson);
                                        obj.ProductList = model_pro;


                                        string accountsjsonddl = obj.UsersList.FirstOrDefault().AccountName;
                                        var modelddl = JsonConvert.DeserializeObject<List<UserDTO>>(accountsjsonddl);
                                        obj.AccountddlList = modelddl;

                                        string productsjsonddl = obj.UsersList.FirstOrDefault().ProductName;
                                        var model_prpddl = JsonConvert.DeserializeObject<List<UserDTO>>(productsjsonddl);
                                        obj.ProductddlList = model_prpddl;
                                    }

                                    else
                                        obj.UsersList = userdetailslst;
                                }
                            }
                            SelectList ddlaccounts = new SelectList("", "AccountId", "AccountName", 0);
                            List<UserDTO> _objStudent = rolelst;
                            ddlaccounts = new SelectList(_objStudent, "AccountId", "AccountName", obj.AccountId);
                            ViewData["ddlAccountLst"] = ddlaccounts;
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


        public ActionResult ChangePassword()
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return Json("Index");
            }
            else
            {
                return View();
            }
        }



        public async Task<JsonResult> PasswordUpdate(string password,string userprevious)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return Json("Index");
            }
            else
            {
                int comid = int.Parse(Session["SSCompanyId"].ToString());
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                int userid = int.Parse(Session["SSUserId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        UserDTO obj = new UserDTO();
                        obj.UserId = userid;
                        obj.Password = password;
                        bool status = false;

                        string previouspassword = (string)Session["SSPassword"];
                        if (previouspassword != userprevious)
                        {
                            return Json(new { success = false });
                        }

                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/UserAPI/NewUpdateUserPassword", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var model = JsonConvert.DeserializeObject<AssetsDTO>(responseData);
                            string msg = model.message;
                            if (msg == "1")
                                status = true;
                            else if (msg == "2")
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
    }
}