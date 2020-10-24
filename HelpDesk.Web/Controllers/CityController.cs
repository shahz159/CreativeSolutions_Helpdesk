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
    public class CityController : Controller
    {
        // GET: City
        public async Task<ActionResult> City()
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
                        CityDTO obj = new CityDTO();
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                        obj.OrganizationId = orgid;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CityAPI/NewGetCityList", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var categories = JsonConvert.DeserializeObject<List<CityDTO>>(responseData);
                            if (categories.Count != 0)
                                obj.CityLst = categories;
                            else
                                obj.CityLst = null;
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
        public async Task<ActionResult> Create()
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
                        CityDTO obj = new CityDTO();
                        if (TempData["obj"] != null)
                        {
                            obj = (CityDTO)TempData["obj"];  //retrieve TempData values here
                            ViewData["Submit"] = "false";
                            ViewData["Update"] = "true";
                        }
                        else
                        {
                            ViewData["Submit"] = "true";
                            ViewData["Update"] = "false";
                            obj.CityId = 0;
                        }
                        long userid = long.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        obj.OrganizationId = int.Parse(Session["SSOrganizationId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                        List<TicketDTO> Citylst = new List<TicketDTO>();
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CityAPI/NewGetRegionList", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var categories = JsonConvert.DeserializeObject<List<CityDTO>>(responseData);
                            if (categories.Count != 0)
                                obj.CityLst = categories;
                            else
                                obj.CityLst = null;

                            SelectList ddlCitys = new SelectList("", "RegionId", "RegionName", 0);
                            List<CityDTO> _objStudent = obj.CityLst.ToList();
                            ddlCitys = new SelectList(_objStudent, "RegionId", "RegionName", obj.RegionId);
                            ViewData["ddlRegionList"] = ddlCitys;
                        }
                        return View(obj);
                    }
                    catch (Exception ex)
                    {
                        CityDTO obj = new CityDTO();
                        return View(obj);
                    }
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(CityDTO obj, string Submit, string Update)
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
                        obj.OrganizationId = int.Parse(Session["SSOrganizationId"].ToString());

                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CityAPI/NewInsertUpdateCity", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var categories = JsonConvert.DeserializeObject<CityDTO>(responseData);
                            obj.message = categories.message;
                            string msg = obj.message;
                        }
                        return RedirectToAction("City");
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
                    CityDTO a = new CityDTO();
                    a.CityId = id;
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CityAPI/NewGetCityById", a);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var user = JsonConvert.DeserializeObject<CityDTO>(responseData);
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