using HelpDesk.Web.Handlers;
using HelpDesk.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace HelpDesk.Web.Controllers
{
    public class WarehouseController : Controller
    {
        // GET: Warehouse
        public async Task<ActionResult> Warehouse()
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
                        WarehouseDTO obj = new WarehouseDTO();
                        long userid = long.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                        obj.OrganizationId = orgid;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/WarehouseAPI/NewGetWarehousesList", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var categories = JsonConvert.DeserializeObject<List<WarehouseDTO>>(responseData);
                            if (categories.Count != 0)
                                obj.WarehouseLst = categories;
                            else
                                obj.WarehouseLst = null;
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
            WarehouseDTO obj = new WarehouseDTO();
            if (TempData["obj"] != null)
            {
                obj = (WarehouseDTO)TempData["obj"];  //retrieve TempData values here
                ViewData["Submit"] = "false";
                ViewData["Update"] = "true";
            }
            else
            {
                ViewData["Submit"] = "true";
                ViewData["Update"] = "false";
                obj.WarehouseId = 0;
            }
            return View(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Create(WarehouseDTO obj, string Submit, string Update)
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
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                        obj.OrganizationId = orgid;

                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/WarehouseAPI/NewInsertUpdateWarehouses", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var categories = JsonConvert.DeserializeObject<WarehouseDTO>(responseData);
                            obj.message = categories.message;
                            string msg = obj.message;
                        }
                        return RedirectToAction("Warehouse");
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
                    WarehouseDTO a = new WarehouseDTO();
                    a.WarehouseId = id;
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/WarehouseAPI/NewGetWarehouseById", a);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var user = JsonConvert.DeserializeObject<WarehouseDTO>(responseData);
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

        public async Task<ActionResult> AssignWarehouse()
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
                        WarehouseDTO obj = new WarehouseDTO();
                        int userid = int.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        int comid = int.Parse(Session["SSCompanyId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());

                        obj.OrganizationId = orgid;

                        List<WarehouseDTO> assignwhlst = new List<WarehouseDTO>();
                        List<WarehouseDTO> whlst = new List<WarehouseDTO>();
                        List<WarehouseDTO> selst = new List<WarehouseDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/WarehouseAPI/NewDrpDpwnsandList", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<WarehouseDTO>(responseData);

                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    SelectList ddlusers = new SelectList("", "WarehouseId", "WarehouseName", 0);
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        whlst = ds.Tables[0].AsEnumerable().Select(dataRow => new WarehouseDTO
                                        {
                                            WarehouseName = dataRow.Field<string>("WarehouseName"),
                                            WarehouseId = dataRow.Field<int>("WarehouseId")
                                        }).ToList();


                                        List<WarehouseDTO> _objStudent = whlst;
                                        ddlusers = new SelectList(_objStudent, "WarehouseId", "WarehouseName", obj.WarehouseId);
                                        ViewData["ddlWarehouseList"] = ddlusers;
                                    }
                                    else
                                    {
                                        List<WarehouseDTO> _objStudent = whlst;
                                        ddlusers = new SelectList(_objStudent, "WarehouseId", "WarehouseName", obj.WarehouseId);
                                        ViewData["ddlWarehouseList"] = ddlusers;
                                    }


                                    SelectList ddlaccounts = new SelectList("", "UserId", "FullName", 0);
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        selst = ds.Tables[1].AsEnumerable().Select(dataRow => new WarehouseDTO
                                        {
                                            FullName = dataRow.Field<string>("FullName"),
                                            UserId = dataRow.Field<long>("UserId")
                                        }).ToList();

                                        List<WarehouseDTO> _objlst = selst;
                                        ddlaccounts = new SelectList(_objlst, "UserId", "FullName", obj.UserId);
                                        ViewData["ddlSEList"] = ddlaccounts;
                                    }
                                    else
                                    {
                                        List<WarehouseDTO> _objStudent = selst;
                                        ddlaccounts = new SelectList(_objStudent, "UserId", "FullName", obj.UserId);
                                        ViewData["ddlSEList"] = ddlaccounts;
                                    }

                                    //SelectList ddlcompany = new SelectList("", "CompanyId", "CompanyName", 0);
                                    if (ds.Tables[2].Rows.Count > 0)
                                    {
                                        assignwhlst = ds.Tables[2].AsEnumerable().Select(dataRow => new WarehouseDTO
                                        {
                                            MUWId = dataRow.Field<int>("MUWId"),
                                            WarehouseName = dataRow.Field<string>("WarehouseName"),
                                            FullName = dataRow.Field<string>("FullName"),
                                            isActive = dataRow.Field<bool>("isActive")
                                        }).ToList();

                                        obj.AssignLst = assignwhlst;
                                    }
                                    else
                                    {
                                        obj.AssignLst = assignwhlst;
                                    }
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

        public async Task<JsonResult> AssignSE(int WHId, int SEId)
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
                        WarehouseDTO obj = new WarehouseDTO();
                        obj.UserId = SEId;
                        obj.WarehouseId = WHId;

                        obj.CreatedBy = userid_a;
                        string statustxt = "";
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/WarehouseAPI/NewAssignWarehouses", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var model = JsonConvert.DeserializeObject<AssetsDTO>(responseData);
                            string msg = model.message;
                            statustxt = msg;
                        }
                        return Json(new { success = statustxt });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = "" });
                    }
                }
            }
        }

        public async Task<JsonResult> UpdateAssignSE(int id)
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
                        WarehouseDTO obj = new WarehouseDTO();
                        obj.MUWId = id;

                        obj.CreatedBy = userid_a;
                        string statustxt = "";
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/WarehouseAPI/NewUpdateAssignWarehouses", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var model = JsonConvert.DeserializeObject<AssetsDTO>(responseData);
                            string msg = model.message;
                            statustxt = msg;
                        }
                        return Json(new { success = statustxt });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = "" });
                    }
                }
            }
        }
    }
}