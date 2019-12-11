using HelpDesk.Web.Handlers;
using HelpDesk.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HelpDesk.Web.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts
        public async Task<ActionResult> Accounts()
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    AccountsDTO obj = new AccountsDTO();
                    obj.CompanyId = 10;
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/AccountAPI/NewGetAccountsList", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var categories = JsonConvert.DeserializeObject<List<AccountsDTO>>(responseData);
                        if (categories.Count != 0)
                            obj.AccountsLst = categories;
                        else
                            obj.AccountsLst = null;
                    }
                    return View(obj);
                }
                catch (Exception ex)
                {
                    return View();
                }
            }
        }
        public ActionResult Create()
        {
            AccountsDTO obj = new AccountsDTO();
            if (TempData["obj"] != null)
            {
                obj = (AccountsDTO)TempData["obj"];  //retrieve TempData values here
                ViewData["Submit"] = "true";
            }
            else
            {
                ViewData["Submit"] = "false";
                obj.AccountId = 0;
            }
            return View();
        }
      
        [HttpPost]
        public async Task<ActionResult> Create(AccountsDTO obj, string Save, string Update)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    if (Save== "Save")
                        obj.FlagId = 1;
                    if (Update== "Update")
                        obj.FlagId = 2;
                    obj.CompanyId = 1;
                    obj.CreatedBy = 1;

                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/AccountAPI/NewInsertUpdateAccounts", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var categories = JsonConvert.DeserializeObject<AccountsDTO>(responseData);
                        obj.message = categories.message;
                        string msg = obj.message;
                    }
                    return RedirectToAction("Accounts");
                }
                catch (Exception ex)
                {
                    return View();
                }
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    AccountsDTO a = new AccountsDTO();
                    a.AccountId = id;
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/AccountAPI/NewGetAccountById", a);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var user = JsonConvert.DeserializeObject<AccountsDTO>(responseData);
                        TempData["obj"] = user;
                        return RedirectToAction("Create");
                    }
                    return View("Error");
                }
                catch (Exception ex)
                {
                    return View();
                }
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    AccountsDTO a = new AccountsDTO();
                    a.AccountId = id;
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/AccountAPI/NewDeleteAccount", a);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var user = JsonConvert.DeserializeObject<AccountsDTO>(responseData);
                        a.message = user.message;
                        return RedirectToAction("Accounts");
                    }
                    return View("Error");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Authenticate", "Authentication");
                }
            }
        }
    }
}