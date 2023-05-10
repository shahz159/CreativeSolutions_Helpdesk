using HelpDesk.Web.Handlers;
using HelpDesk.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace HelpDesk.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("X-Frame-Options", "SAMEORIGIN");
        }

        protected void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
        {
            if (HttpContext.Current.Session == null) return;
            var authCookie = Request.Cookies["__SessionToken"];
            if (Request.Cookies["__SessionToken"] != null)
            {
                HttpCookie cookie = new HttpCookie("__SessionToken");
                cookie.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(cookie);

                LoginDTO obj = new LoginDTO();
                if (Session["SSEmail"] != null)
                {
                    obj.Email = Session["SSEmail"].ToString();
                    obj.Password = Session["SSPassword"].ToString();
                    using (HttpClient client = new HttpClient())
                    {
                        CommonHeader.setHeaders(client);
                        try
                        {
                            var responseMessage = client.PostAsJsonAsync("api/LoginAPI/NewLogin", obj).Result;
                            var result = responseMessage.Content.ReadAsStringAsync();
                            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var responseData = result.Result;
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

                                        Session["SSMenusLst"] = MenusDetails;
                                        Session["SSMenusCountLst"] = MenusCountDetails;

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
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }


        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Server.ClearError();
            Response.Redirect("/Error/ErrorPage");
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            try
            {
                if (HttpContext.Current.User != null)
                {
                    if (HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        if (HttpContext.Current.User.Identity is FormsIdentity)
                        {
                            FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                            HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(id, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
