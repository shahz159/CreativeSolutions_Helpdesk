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
    public class ModelController : Controller
    {
        // GET: Model
        public async Task<ActionResult> Model()
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
                        ModelDTO obj = new ModelDTO();
                        obj.CompanyId = int.Parse(Session["SSCompanyId"].ToString());
                        obj.CompanyId = 10;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/ModelAPI/NewGetModelList", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var categories = JsonConvert.DeserializeObject<List<ModelDTO>>(responseData);
                            if (categories.Count != 0)
                                obj.ModelLst = categories;
                            else
                                obj.ModelLst = null;
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
                        ModelDTO obj = new ModelDTO();
                        if (TempData["obj"] != null)
                        {
                            obj = (ModelDTO)TempData["obj"];  //retrieve TempData values here
                            ViewData["Submit"] = "false";
                            ViewData["Update"] = "true";
                        }
                        else
                        {
                            ViewData["Submit"] = "true";
                            ViewData["Update"] = "false";
                            obj.ProductId = 0;
                        }
                        long userid = long.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        obj.CompanyId= int.Parse(Session["SSCompanyId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());

                        obj.CompanyId = 10;
                        List<TicketDTO> modellst = new List<TicketDTO>();
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/ModelAPI/NewGetProductsList", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var categories = JsonConvert.DeserializeObject<List<ModelDTO>>(responseData);
                            if (categories.Count != 0)
                                obj.ModelLst = categories;
                            else
                                obj.ModelLst = null;

                            SelectList ddlmodels= new SelectList("", "ProductId", "ProductName", 0);
                            List<ModelDTO> _objStudent = obj.ModelLst.ToList();
                            ddlmodels = new SelectList(_objStudent, "ProductId", "ProductName", obj.ProductId);
                            ViewData["ddlProductList"] = ddlmodels;
                        }
                        return View(obj);
                    }
                    catch (Exception ex)
                    {
                        ModelDTO obj = new ModelDTO();
                        return View(obj);
                    }
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(ModelDTO obj, string Submit, string Update)
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
                        obj.CompanyId = int.Parse(Session["SSCompanyId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());

                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/ModelAPI/NewInsertUpdateModel", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var categories = JsonConvert.DeserializeObject<ModelDTO>(responseData);
                            obj.message = categories.message;
                            string msg = obj.message;
                        }
                        return RedirectToAction("Model");
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
                    ModelDTO a = new ModelDTO();
                    a.ModelId = id;
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/ModelAPI/NewGetModelById", a);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var user = JsonConvert.DeserializeObject<ModelDTO>(responseData);
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