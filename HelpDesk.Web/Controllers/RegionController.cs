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
    public class RegionController : Controller
    {
        public async Task<ActionResult> Region()
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
                        RegionDTO obj = new RegionDTO();

                        long userid = long.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                        obj.OrganizationId = orgid;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/RegionAPI/NewGetRegionsList", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var categories = JsonConvert.DeserializeObject<List<RegionDTO>>(responseData);
                            if (categories.Count != 0)
                                obj.RegionLst = categories;
                            else
                                obj.RegionLst = null;
                        }
                        return View(obj);
                    }
                    catch (Exception ex)
                    {
                        return View();
                    }
                }
            }
        }
        public ActionResult Create()
        {
            RegionDTO obj = new RegionDTO();
            if (TempData["obj"] != null)
            {
                obj = (RegionDTO)TempData["obj"];  //retrieve TempData values here
                ViewData["Submit"] = "false";
                ViewData["Update"] = "true";
            }
            else
            {
                ViewData["Submit"] = "true";
                ViewData["Update"] = "false";
                obj.RegionId = 0;
            }
            return View(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Create(RegionDTO obj, string Submit, string Update)
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
                        if (Submit == "Submit")
                            obj.FlagId = 1;
                        if (Update == "Update")
                            obj.FlagId = 2;
                        obj.CreatedBy = long.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        int.Parse(Session["SSCompanyId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                        obj.OrganizationId = orgid;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/RegionAPI/NewInsertUpdateRegions", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var categories = JsonConvert.DeserializeObject<RegionDTO>(responseData);
                            obj.message = categories.message;
                            string msg = obj.message;
                        }
                        return RedirectToAction("Region");
                    }
                    catch (Exception ex)
                    {
                        return View();
                    }
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
                    RegionDTO a = new RegionDTO();
                    a.RegionId = id;
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/RegionAPI/NewGetRegionById", a);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var user = JsonConvert.DeserializeObject<RegionDTO>(responseData);
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
    }
}