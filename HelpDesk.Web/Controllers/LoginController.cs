﻿using HelpDesk.Web.Handlers;
using HelpDesk.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;

namespace HelpDesk.Web.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Index(LoginDTO obj)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/LoginAPI/NewLogin", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var ResponseJ = JObject.Parse(responseData);
                        bool isStatus = Convert.ToBoolean(ResponseJ.SelectToken("Status"));
                        string Message = ResponseJ.SelectToken("Message").ToString();
                        var Data = ResponseJ.SelectToken("Data");
                        if (isStatus == true)
                        {
                            if (Data != null)
                            {
                                var jtok = Data;
                                var JLoginUserList = jtok.SelectToken("Login");
                                var JMenusDetails = jtok.SelectToken("Menus");
                                var JApprovalMenuCountDetails = jtok.SelectToken("ApprovalCounts");

                                List<LoginDTO> LoginDetails = new List<LoginDTO>();
                                List<MenusDTO> MenusDetails = new List<MenusDTO>();
                                List<MenusDTO> MenusCountDetails = new List<MenusDTO>();

                                LoginDetails = JsonConvert.DeserializeObject<List<LoginDTO>>(JLoginUserList.ToString());
                                MenusDetails = JsonConvert.DeserializeObject<List<MenusDTO>>(JMenusDetails.ToString());
                                MenusCountDetails = JsonConvert.DeserializeObject<List<MenusDTO>>(JApprovalMenuCountDetails.ToString());

                                //List<MenusDTO> MainMenusDetails = new List<MenusDTO>();
                                //List<MenusDTO> SubMenusDetails = new List<MenusDTO>();

                                //MainMenusDetails = MenusDetails.Where(x => x.ParentId == 0).ToList();
                                //SubMenusDetails = MenusDetails.Where(x => x.ParentId != 0).ToList();

                                Session["SSMenusLst"] = MenusDetails;
                                Session["SSMenusCountLst"] = MenusCountDetails;
                                //Session["SSSubMenusLst"] = SubMenusDetails;

                                foreach (var item in MenusCountDetails)
                                {
                                    Session["SSNewUser"] = long.Parse(item.NewUser.ToString());
                                    Session["SSWarrantyExpiredApprovalCount"] = long.Parse(item.WarrantyExpiredApprovalCount.ToString());
                                    Session["SSAssetRenewalCount"] = long.Parse(item.AssetRenewalCount.ToString());
                                    Session["SSAssetApprovalCount"] = long.Parse(item.AssetApprovalCount.ToString());
                                    Session["SSInventoryAdjustment"] = long.Parse(item.InventoryAdjustment.ToString());
                                    Session["SSPPMDatesApprovalCount"] = long.Parse(item.PPMDatesApprovalCount.ToString());
                                    Session["SSSparePartRequestCount"] = long.Parse(item.SparePartRequestCount.ToString());
                                }
                                long UserId = 0;
                                foreach (var item in LoginDetails)
                                {
                                    UserId = long.Parse(item.UserId.ToString());
                                    int RoleId = int.Parse(item.RoleId.ToString());

                                    int OrganizationId = int.Parse(item.OrganizationId.ToString());
                                    int CompanyId = int.Parse(item.CompanyId.ToString());
                                    int AccountId = int.Parse(item.AccountId.ToString());
                                    string RoleName = item.RoleName.ToString();
                                    string Email = item.Email.ToString();
                                    string Password = item.Password.ToString();
                                    string profileimage = item.ProfileImage.ToString();

                                    string UserName = Convert.ToString(item.FullName);
                                    Session["SSUserId"] = UserId;
                                    Session["SSUserName"] = UserName;
                                    Session["SSRoleId"] = RoleId;
                                    Session["SSOrganizationId"] = OrganizationId;
                                    Session["SSCompanyId"] = CompanyId;
                                    Session["SSAccountId"] = AccountId;
                                    Session["SSRoleName"] = RoleName;
                                    Session["SSEmail"] = Email;
                                    Session["SSPassword"] = Password;
                                    Session["SSprofileimage"] = profileimage;

                                    List<TicketDTO> multipleimages = new List<TicketDTO>();
                                    Session["MultipleImagesLst" + UserId] = multipleimages;
                                }

                                string SessionTokenKey = "__SessionToken";
                                string guid = Guid.NewGuid().ToString();
                                var responseCookie = new HttpCookie(SessionTokenKey)
                                {
                                    HttpOnly = true,
                                    Value = UserId.ToString()
                                };
                                if (System.Web.Security.FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                                    responseCookie.Secure = true;
                                Response.Cookies.Set(responseCookie);

                                return RedirectToAction("Index", "Dashboard");
                            }
                        }
                        else
                        {
                            TempData["Error"] = Message;
                        }
                    }
                    return View(obj);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error");
                }
            }
        }
        public ActionResult Logout()
        {
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            HttpCookie cookie = new HttpCookie("__SessionToken");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index", "Login");
        }


        [HttpPost]
        public async Task<ActionResult> NewUser(UserDTO obj)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    obj.OrganizationId = 1001;
                    obj.CreatedBy = 0;

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

                    //for (int i = 0; i < obj.Products.Length; i++)
                    //{
                    //    var parentelement_p = xmldoc_docs_p.CreateElement("Row");
                    //    var productid_xml_p = xmldoc_docs_p.CreateElement("ProductId");

                    //    productid_xml_p.InnerText = obj.Products[i];
                    //    parentelement_p.AppendChild(productid_xml_p);

                    //    parentelemeng_docs_p.AppendChild(parent_docs_p);
                    //    parent_docs_p.AppendChild(parentelement_p);
                    //}
                    for (int i = 0; i < obj.ProductIds.Length; i++)
                    {
                        var parentelement_p = xmldoc_docs_p.CreateElement("Row");
                        var productid_xml_p = xmldoc_docs_p.CreateElement("ProductId");

                        productid_xml_p.InnerText = obj.ProductIds[i];
                        parentelement_p.AppendChild(productid_xml_p);

                        parentelemeng_docs_p.AppendChild(parent_docs_p);
                        parent_docs_p.AppendChild(parentelement_p);
                    }
                    obj.Productsxml = parentelemeng_docs_p.InnerXml;
                    bool status = false;
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/UserAPI/NewInsertUserSignup", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var categories = JsonConvert.DeserializeObject<AccountsDTO>(responseData);
                        obj.message = categories.message;
                        string msg = obj.message;
                        if (msg == "1")
                            status = true;
                        else
                            status = false;
                    }
                    return Json(new { success = status });
                }
                catch (Exception ex)
                {
                    return View();
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
        public ActionResult Signup()
        {
            UserDTO obj = new UserDTO();
            obj.RoleId = 503;
            obj.CompanyId = 10;
            return View(obj);
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

        public ActionResult Enquiry()
        {
            UserDTO obj = new UserDTO();
            obj.RoleId = 503;
            obj.CompanyId = 10;
            return View(obj);
        }

        [HttpPost]
        public async Task<ActionResult> NewMainEnquiry(UserDTO obj)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    obj.OrganizationId = 1001;
                    obj.CreatedBy = 0;

                    bool status = false;
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/InventoryAPI/NewMainEnquiry", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var categories = JsonConvert.DeserializeObject<AccountsDTO>(responseData);
                        obj.message = categories.message;
                        string msg = obj.message;
                        if (msg == "1")
                            status = true;
                        else
                            status = false;
                    }
                    return Json(new { success = status });
                }
                catch (Exception ex)
                {
                    return View();
                }
            }
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }
        public async Task<JsonResult> SendChangePasswordRequest(string useremail)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    Guid obj_token = Guid.NewGuid();
                    string token = obj_token.ToString();



                    bool status = false;
                    HttpResponseMessage responseMessage = await client.GetAsync("api/UserAPI/GetChangePasswordRequest?email=" + useremail);
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

        public async Task<ActionResult> ResetPassword(string username, string token)
        {
            UserDTO model = new UserDTO();
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {

                    HttpResponseMessage responseMessage = await client.GetAsync("api/UserAPI/GetVerifyPasswordChangeRequest?email=" + username + "&token=" + token);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var check = JsonConvert.DeserializeObject<UserDTO>(responseData);
                        string msg = check.message;
                        if (msg == "1")
                            model.isCancelled = false;
                        else if (msg == "0")
                            model.isCancelled = true;
                    }
                    model.Email = username;
                    return View(model);
                }
                catch (Exception ex)
                {
                    return View(model);
                }
            }
        }

        public async Task<JsonResult> UpdatePassword(string Password, string email)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    bool status = false;
                    UserDTO obj = new UserDTO();
                    obj.Email = email;
                    obj.Password = Password;

                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/UserAPI/NewUpdateUserPasswordwithEmail", obj);
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
                    return Json(new SelectList("", "Value", "Text"));
                }
            }
        }

    }
}