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
    public class InventoryController : Controller
    {
        // GET: Inventory
        public async Task<ActionResult> SparePart()
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
                        InventoryDTO obj = new InventoryDTO();
                        obj.Organizationid= int.Parse(Session["SSOrganizationId"].ToString());
                        obj.CreatedBy = int.Parse(Session["SSUserId"].ToString());
                        List<InventoryDTO> warelst = new List<InventoryDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/InventoryAPI/NewGetWarehouseDropDowns", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<InventoryDTO>(responseData);

                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    SelectList ddlwarehouse = new SelectList("", "WarehouseId", "WarehouseName", 0);
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        warelst = ds.Tables[0].AsEnumerable().Select(dataRow => new InventoryDTO
                                        {
                                            WarehouseId = dataRow.Field<int>("WareHouseId"),
                                            WarehouseName = dataRow.Field<string>("WarehouseName"),
                                            Gruop = dataRow.Field<string>("Gruop")
                                        }).ToList();
                                        obj.WarehouseList = warelst;
                                        //List<InventoryDTO> _objroles = warelst;
                                        //ddlwarehouse = new SelectList(_objroles, "WarehouseId", "WarehouseName", obj.WarehouseId);
                                        //ViewData["ddlWarehouseLst"] = ddlwarehouse;
                                    }
                                    else
                                    {
                                        obj.WarehouseList = null;
                                        //List<InventoryDTO> _objroles = warelst;
                                        //ddlwarehouse = new SelectList(_objroles, "WarehouseId", "WarehouseName", obj.WarehouseId);
                                        //ViewData["ddlWarehouseLst"] = ddlwarehouse;
                                    } 
                                }
                            }
                        }
                        obj.RoleId= int.Parse(Session["SSRoleId"].ToString());
                        return View(obj);
                    }
                    catch (Exception ex)
                    {
                        InventoryDTO obj = new InventoryDTO();
                        return View(obj);
                    }
                }
            }
        }
        public async Task<ActionResult> SparePartListByWarehouseId(int id)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                int roleid = int.Parse(Session["SSRoleId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        InventoryDTO obj = new InventoryDTO();
                        List<InventoryDTO> sparelst = new List<InventoryDTO>();
                        obj.Organizationid = orgid;
                        obj.WarehouseId = id;

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/InventoryAPI/NewSparePartList", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<InventoryDTO>(responseData);

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
                                        sparelst = ds.Tables[0].AsEnumerable().Select(dataRow => new InventoryDTO
                                        {
                                            SparePartId = dataRow.Field<long>("SparePartId"),
                                            WarehouseId = dataRow.Field<int>("WarehouseId"),
                                            SparePartName = dataRow.Field<string>("SparePartName"),
                                            SparePartNumber = dataRow.Field<string>("SparePartNumber"),
                                            Quantity = dataRow.Field<int>("Quantity"),
                                            BaseQuantity = dataRow.Field<int>("BaseQuantity"),
                                            Price = dataRow.Field<string>("Price") 
                                        }).ToList();
                                        obj.SparePartList = sparelst;
                                    }
                                    else
                                        obj.SparePartList = sparelst;
                                }
                            }
                        }
                        obj.RoleId = roleid;
                        return PartialView("SparePartListPV", obj);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Authenticate", "Authentication");
                    }
                }
            }
        }
        public async Task<ActionResult> NewSparePart()
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
                        InventoryDTO obj = new InventoryDTO();
                        if (TempData["obj"] != null)
                        {
                            obj = (InventoryDTO)TempData["obj"];  //retrieve TempData values here
                            obj.FlagId = 2;
                        }
                        else
                            obj.FlagId = 1;
                       
                        obj.Organizationid = int.Parse(Session["SSOrganizationId"].ToString());
                        List<InventoryDTO> warelst = new List<InventoryDTO>();
                        obj.CreatedBy = int.Parse(Session["SSUserId"].ToString());
                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/InventoryAPI/NewGetWarehouseDropDowns", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<InventoryDTO>(responseData);

                            var data = docs.datasetxml;
                            if (data != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml(data);
                                DataSet ds = new DataSet();
                                ds.ReadXml(new XmlNodeReader(document));
                                if (ds.Tables.Count > 0)
                                {
                                    SelectList ddlwarehouse = new SelectList("", "WarehouseId", "WarehouseName", 0);
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        warelst = ds.Tables[0].AsEnumerable().Select(dataRow => new InventoryDTO
                                        {
                                            WarehouseId = dataRow.Field<int>("WareHouseId"),
                                            WarehouseName = dataRow.Field<string>("WarehouseName")
                                        }).ToList();
                                        List<InventoryDTO> _objroles = warelst;
                                        ddlwarehouse = new SelectList(_objroles, "WarehouseId", "WarehouseName", obj.WarehouseId);
                                        ViewData["ddlWarehouseLst"] = ddlwarehouse;
                                    }
                                    else
                                    {
                                        List<InventoryDTO> _objroles = warelst;
                                        ddlwarehouse = new SelectList(_objroles, "WarehouseId", "WarehouseName", obj.WarehouseId);
                                        ViewData["ddlWarehouseLst"] = ddlwarehouse;
                                    }
                                }
                            }
                        }
                        return View(obj);
                    }
                    catch (Exception ex)
                    {
                        InventoryDTO obj = new InventoryDTO();
                        return View(obj);
                    }
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> NewSparePartJson(InventoryDTO obj)
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
                        obj.Organizationid = orgid;
                        obj.CreatedBy = userid;
                        
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/InventoryAPI/NewInsertSparePart", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var categories = JsonConvert.DeserializeObject<AccountsDTO>(responseData);
                            obj.message = categories.message;
                            string msg = obj.message;
                        }
                        return Json(new { success = true });
                    }
                    catch (Exception ex)
                    {
                        return View();
                    }
                }
            }
        }

        public async Task<ActionResult> SparePartDetails(long id)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    InventoryDTO a = new InventoryDTO();
                    a.SparePartId = id;
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/InventoryAPI/NewGetSparePartDetailsById", a);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var user = JsonConvert.DeserializeObject<InventoryDTO>(responseData);

                        a = user;

                        string commentsj = user.ConsignmentsJson;
                        var modalcomments = JsonConvert.DeserializeObject<List<InventoryDTO>>(commentsj);
                        a.ConsignmentsList = modalcomments;
                        
                        return PartialView("SparePartDetailsPV", a);
                        //TempData["obj"] = user;
                        //return RedirectToAction("NewSparePart");
                    }
                    return View("Error");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Login");
                }
            }
        }

        public async Task<ActionResult> Edit(long id)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    InventoryDTO a = new InventoryDTO();
                    a.SparePartId = id;
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/InventoryAPI/NewGetSparePartDetailsById", a);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var user = JsonConvert.DeserializeObject<InventoryDTO>(responseData);

                        TempData["obj"] = user;
                        return RedirectToAction("NewSparePart");
                    }
                    return View("Error");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Login");
                }
            }
        }

        public async Task<JsonResult> CheckSparePartName(string name,int id)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    bool status = false;
                    InventoryDTO obj = new InventoryDTO();
                    obj.SparePartName = name;
                    obj.WarehouseId = id;

                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/InventoryAPI/NewCheckSparePartName", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var check = JsonConvert.DeserializeObject<InventoryDTO>(responseData);
                        string msg = check.message;
                        if (msg == "1")
                            status = true;
                        else if (msg == "2")
                            status = false;
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

        [HttpPost]
        public async Task<ActionResult> NewConsignmentJson(string txt,int Quantity, long SparePartId)
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
                        InventoryDTO obj = new InventoryDTO();
                        obj.Organizationid = orgid;
                        obj.CreatedBy = userid;
                        obj.Comments = txt;
                        obj.Quantity = Quantity;
                        obj.SparePartId = SparePartId;

                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/InventoryAPI/NewInsertConsignment", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var categories = JsonConvert.DeserializeObject<InventoryDTO>(responseData);
                            obj.message = categories.message;
                            string msg = obj.message;
                        }
                        return Json(new { success = true });
                    }
                    catch (Exception ex)
                    {
                        return View();
                    }
                }
            }
        }

        public async Task<ActionResult> ConsignmentApproval()
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                int roleid = int.Parse(Session["SSRoleId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        InventoryDTO obj = new InventoryDTO();
                        List<InventoryDTO> sparelst = new List<InventoryDTO>();
                        obj.Organizationid = orgid;
                        
                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/InventoryAPI/NewConsignmentList", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<InventoryDTO>(responseData);

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
                                        sparelst = ds.Tables[0].AsEnumerable().Select(dataRow => new InventoryDTO
                                        {
                                            ConsignmentId = dataRow.Field<long>("ConsignmentId"),
                                            SparePartName = dataRow.Field<string>("SparePartName"),
                                            SparePartNumber = dataRow.Field<string>("SparePartNumber"),
                                            WarehouseName = dataRow.Field<string>("WarehouseName"),
                                            AHCCode = dataRow.Field<string>("AHCCode"),
                                            Quantity = dataRow.Field<int>("Quantity"),
                                            ConsignmentDate = dataRow.Field<string>("ConsignmentDate")
                                        }).ToList();
                                        obj.SparePartList = sparelst;
                                    }
                                    else
                                        obj.SparePartList = sparelst;
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


        //UpdateConsignmentStatus

        public async Task<ActionResult> UpdateConsignmentStatus(int id, long ConsignmentId, string comments)
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

                        InventoryDTO obj = new InventoryDTO();

                        obj.CreatedBy = userid;
                        obj.Statusid = id;
                        obj.ConsignmentId = ConsignmentId;
                        obj.Comments = comments;

                        bool status = false;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/InventoryAPI/NewConsignmentStatus", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<InventoryDTO>(responseData);
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
    }
}