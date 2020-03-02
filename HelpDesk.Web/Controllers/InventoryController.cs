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
using SautinSoft;

namespace HelpDesk.Web.Controllers
{
    public class InventoryController : Controller
    {
        // GET: Inventory
        public async Task<ActionResult> SparePartMaster()
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
                        obj.OrganizationId = int.Parse(Session["SSOrganizationId"].ToString());
                        List<InventoryDTO> sparelst = new List<InventoryDTO>();

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
                                            ProductId = dataRow.Field<int>("ProductId"),
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
                        obj.RoleId = int.Parse(Session["SSRoleId"].ToString());
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
                        obj.OrganizationId = int.Parse(Session["SSOrganizationId"].ToString());
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
                        obj.RoleId = int.Parse(Session["SSRoleId"].ToString());
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
                        obj.OrganizationId = orgid;
                        obj.WarehouseId = id;

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/InventoryAPI/NewSparePartListByWHId", obj);
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
                                            //WarehouseId = dataRow.Field<int>("WarehouseId")
                                            SparePartName = dataRow.Field<string>("SparePartName"),
                                            SparePartNumber = dataRow.Field<string>("SparePartNumber"),
                                            Quantity = dataRow.Field<int>("Quantity"),
                                            BaseQuantity = dataRow.Field<int>("BaseQuantity"),
                                            Price = dataRow.Field<string>("Price"),
                                            ProductName = dataRow.Field<string>("ProductName")
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

                        obj.OrganizationId = int.Parse(Session["SSOrganizationId"].ToString());
                        List<InventoryDTO> warelst = new List<InventoryDTO>();
                        List<InventoryDTO> productlst = new List<InventoryDTO>();
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
                                    SelectList ddlwarehouse = new SelectList("", "ProductId", "ProductName", 0);
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        productlst = ds.Tables[1].AsEnumerable().Select(dataRow => new InventoryDTO
                                        {
                                            ProductId = dataRow.Field<int>("ProductId"),
                                            ProductName = dataRow.Field<string>("ProductName")
                                        }).ToList();
                                        List<InventoryDTO> _objroles = productlst;
                                        ddlwarehouse = new SelectList(_objroles, "ProductId", "ProductName", obj.ProductId);
                                        ViewData["ddlProductLst"] = ddlwarehouse;
                                    }
                                    else
                                    {
                                        List<InventoryDTO> _objroles = warelst;
                                        ddlwarehouse = new SelectList(_objroles, "ProductId", "ProductName", obj.ProductId);
                                        ViewData["ddlProductLst"] = ddlwarehouse;
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
                        obj.OrganizationId = orgid;
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
        public async Task<ActionResult> SparePartDetails(long id, int WarehouseId)
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
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        InventoryDTO obj = new InventoryDTO();
                        obj.SparePartId = id;
                        obj.WarehouseId = WarehouseId;
                        List<InventoryDTO> warelst = new List<InventoryDTO>();
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/InventoryAPI/NewGetSparePartDetailsById", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
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
                                            SparePartId = dataRow.Field<long>("SparePartId"),
                                            ProductId = dataRow.Field<int>("ProductId"),
                                            SparePartName = dataRow.Field<string>("SparePartName"),
                                            SparePartNumber = dataRow.Field<string>("SparePartNumber"),
                                            Quantity = dataRow.Field<int>("Quantity"),
                                            BaseQuantity = dataRow.Field<int>("BaseQuantity"),
                                            Price = dataRow.Field<string>("Price"),
                                            ConsignmentDate= dataRow.Field<string>("ConsignmentDate"),
                                            Stock = dataRow.Field<int>("Stock"),
                                            WarehousestockId = dataRow.Field<long>("WarehousestockId"),
                                            RequestPending = dataRow.Field<int>("RequestPending"),
                                            RequestStatus = dataRow.Field<int>("RequestStatus"),
                                            WarehouseJson = dataRow.Field<string>("WarehouseJson"),
                                            HistoryJson = dataRow.Field<string>("HistoryJson")
                                        }).ToList();
                                        obj.WarehouseList = warelst;
                                        obj.Quantity= obj.WarehouseList.FirstOrDefault().Quantity;
                                        obj.WarehousestockId= obj.WarehouseList.FirstOrDefault().WarehousestockId;

                                        string warehousej = obj.WarehouseList.FirstOrDefault().WarehouseJson;
                                        var modalwarehouse = JsonConvert.DeserializeObject<List<InventoryDTO>>(warehousej);
                                        obj.WHddlList = modalwarehouse;


                                        string warehousejs = obj.WarehouseList.SingleOrDefault().HistoryJson;
                                        var modalwarehousesd = JsonConvert.DeserializeObject<List<InventoryDTO>>(warehousejs);
                                        obj.ConsignmentsList = modalwarehousesd;
                                    }
                                    else
                                    {
                                        obj.WarehouseList = null;
                                    }
                                }
                            }

                            //var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            //var user = JsonConvert.DeserializeObject<InventoryDTO>(responseData);
                            //a = user;

                            //string commentsj = user.ConsignmentsJson;
                            //var modalcomments = JsonConvert.DeserializeObject<List<InventoryDTO>>(commentsj);
                            //a.ConsignmentsList = modalcomments;

                            //string warehousej = user.WarehouseJson;
                            //var modalwarehouse = JsonConvert.DeserializeObject<List<InventoryDTO>>(warehousej);
                            //a.WarehouseList = modalwarehouse;
                            obj.RoleId = roleid;
                            obj.SparePartId = id;
                            obj.WarehouseId = WarehouseId;
                            return PartialView("SparePartDetailsPV", obj);
                        }
                        return View("Error");
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index", "Login");
                    }
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
        public async Task<JsonResult> CheckSparePartName(string name, int id)
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
        public async Task<ActionResult> NewConsignmentJson(string message)
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
                        obj.OrganizationId = orgid;
                        obj.CreatedBy = userid;
                        obj.message = message;
                        //obj.Quantity = Quantity;
                        //obj.SparePartId = SparePartId;
                        //obj.WarehouseId = warehouseid;

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
                        obj.OrganizationId = orgid;

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
                                            ConsignmentId = dataRow.Field<long>("ApprovalId"),
                                            SparePartName = dataRow.Field<string>("SparePartName"),
                                            SparePartNumber = dataRow.Field<string>("SparePartNumber"),
                                            WarehouseName = dataRow.Field<string>("WarehouseName"),
                                            AHCCode = dataRow.Field<string>("AHCCode"),
                                            Quantity = dataRow.Field<int>("Quantity"),
                                            ConsignmentDate = dataRow.Field<string>("ConsignmentDate"),
                                            CreatedOn = dataRow.Field<DateTime>("CreatedDate"),
                                            Type = dataRow.Field<string>("Type")
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

        public async Task<ActionResult> UpdateSparePart(string SparePartName, string Price, int Quantity, int BaseQuantity, long SparePartId)
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
                        obj.SparePartName = SparePartName;
                        obj.Price = Price;
                        obj.Quantity = Quantity;
                        obj.BaseQuantity = BaseQuantity;
                        obj.SparePartId = SparePartId;

                        bool status = false;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/InventoryAPI/NewUpdateSparePart", obj);
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

        public async Task<ActionResult> StockConsignment()
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
                        obj.OrganizationId = int.Parse(Session["SSOrganizationId"].ToString());
                        obj.CreatedBy = int.Parse(Session["SSUserId"].ToString());
                        List<InventoryDTO> warelst = new List<InventoryDTO>();
                        List<InventoryDTO> sparepartlst = new List<InventoryDTO>();

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
                                    }
                                    else
                                        obj.WarehouseList = null;

                                    SelectList ddlsparepart = new SelectList("", "SparePartId", "SparePartName", 0);
                                    if (ds.Tables[2].Rows.Count > 0)
                                    {
                                        sparepartlst = ds.Tables[2].AsEnumerable().Select(dataRow => new InventoryDTO
                                        {
                                            SparePartId = dataRow.Field<long>("SparePartId"),
                                            SparePartName = dataRow.Field<string>("SparePartName")
                                        }).ToList();
                                        obj.SparePartList = sparepartlst;
                                    }
                                    else
                                        obj.SparePartList = null;
                                }
                            }
                        }
                        obj.RoleId = int.Parse(Session["SSRoleId"].ToString());
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
        public async Task<ActionResult> StockList()
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
                        obj.OrganizationId = int.Parse(Session["SSOrganizationId"].ToString());
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
                        obj.RoleId = int.Parse(Session["SSRoleId"].ToString());
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

        public async Task<ActionResult> AddStockChangeRequest(long WarehousestockId, int Quantity,string Type,int Statusid)
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
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());

                        InventoryDTO obj = new InventoryDTO();

                        obj.CreatedBy = userid;
                        obj.WarehousestockId = WarehousestockId;
                        obj.Quantity = Quantity;
                        obj.Type = Type;
                        obj.Statusid = Statusid;
                        obj.OrganizationId = orgid;

                        bool status = false;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/InventoryAPI/NewStockChangeRequest", obj);
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
        public async Task<ActionResult> TrasnferQuantity(long WarehousestockId, int Quantity, int ToWHID, long SparePartId)
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
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());

                        InventoryDTO obj = new InventoryDTO();

                        obj.CreatedBy = userid;
                        obj.WarehousestockId = WarehousestockId;
                        obj.Quantity = Quantity;
                        obj.ToWarehouseId = ToWHID;
                        obj.OrganizationId = orgid;
                        obj.SparePartId = SparePartId;

                        bool status = false;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/InventoryAPI/NewTrasnferQuantity", obj);
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

        public ActionResult PdfToHtml()
        {
            SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
            f.OpenPdf(@"C:\Users\Syed's\Downloads\AHC Helpdesk.pdf");
            if (f.PageCount>0)
            {
                int result = f.ToHtml(@"C:\Users\Syed's\Downloads\AHC Helpdesk_html.html");
            }
            return View();
        }

        public async Task<JsonResult> SparePartDetailsConsignmentJson(int warehouseid,long SparePartId)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return Json("Index", "Login");
            }
            else
            {
                int userid = int.Parse(Session["SSUserId"].ToString());
                int roleid = int.Parse(Session["SSRoleId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        InventoryDTO obj = new InventoryDTO();
                        obj.SparePartId = SparePartId;
                        obj.WarehouseId = warehouseid;
                        List<InventoryDTO> warelst = new List<InventoryDTO>();
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/InventoryAPI/NewGetSparePartDetailsByIdSP", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
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
                                        warelst = ds.Tables[0].AsEnumerable().Select(dataRow => new InventoryDTO
                                        {
                                            SparePartId = dataRow.Field<long>("SparePartId"),
                                            ProductId = dataRow.Field<int>("ProductId"),
                                            SparePartName = dataRow.Field<string>("SparePartName"),
                                            SparePartNumber = dataRow.Field<string>("SparePartNumber"),
                                            Quantity = dataRow.Field<int>("Quantity"),
                                            BaseQuantity = dataRow.Field<int>("BaseQuantity"),
                                            Price = dataRow.Field<string>("Price") 
                                        }).ToList();
                                        obj.WarehouseList = warelst;
                                    }
                                    else
                                        obj.WarehouseList = null;
                                }
                            }
                        }
                        return Json(obj.WarehouseList);
                    }
                    catch (Exception ex)
                    {
                        return Json("Index", "Login");
                    }
                }
            }
        }

        public async Task<ActionResult> Transfer()
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
                        obj.OrganizationId = int.Parse(Session["SSOrganizationId"].ToString());
                        List<InventoryDTO> sparelst = new List<InventoryDTO>();

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
                                            ProductId = dataRow.Field<int>("ProductId"),
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
                        obj.RoleId = int.Parse(Session["SSRoleId"].ToString());
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

        public async Task<ActionResult> WarehouseListBySparePartId(long id)
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
                        obj.SparePartId = id;

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/InventoryAPI/NewGetListOfWarehouseBySparePart", obj);
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
                                            WarehouseId = dataRow.Field<int>("WareHouseId"),
                                            WarehousestockId = dataRow.Field<long>("WarehouseStockId"),
                                            Stock = dataRow.Field<int>("Stock"),
                                            SparePartId = dataRow.Field<long>("SparePartId"),
                                            WarehouseName = dataRow.Field<string>("WarehouseName") 
                                        }).ToList();
                                        obj.SparePartList = sparelst;
                                    }
                                    else
                                        obj.SparePartList = sparelst;
                                }
                            }
                        }
                        return PartialView("WarehouseListBySparePartPV", obj);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Authenticate", "Authentication");
                    }
                }
            }
        }
        public async Task<ActionResult> SparePartStockDetails(long id)
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
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        InventoryDTO obj = new InventoryDTO();
                        obj.WarehousestockId =id;
                        List<InventoryDTO> warelst = new List<InventoryDTO>();
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/InventoryAPI/NewGetSparePartDetailsByWareHouseStock", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
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
                                            WarehousestockId = dataRow.Field<long>("WarehousestockId"),
                                            WarehouseId = dataRow.Field<int>("WareHouseId"),
                                            Stock = dataRow.Field<int>("Stock"),
                                            SparePartId = dataRow.Field<long>("SparePartId"),
                                            WarehouseName = dataRow.Field<string>("WarehouseName"),
                                            SparePartName = dataRow.Field<string>("SparePartName"),
                                            SparePartNumber = dataRow.Field<string>("SparePartNumber"),
                                            WarehouseJson = dataRow.Field<string>("WarehouseJson")
                                        }).ToList();
                                        obj.WarehouseList = warelst;
                                        string warehousej = obj.WarehouseList.FirstOrDefault().WarehouseJson;
                                        var modalwarehouse = JsonConvert.DeserializeObject<List<InventoryDTO>>(warehousej);
                                        obj.WHddlList = modalwarehouse;
                                    }
                                    else
                                    {
                                        obj.WarehouseList = null;
                                        obj.WHddlList = null;
                                    }
                                }
                            }
                            return PartialView("StockDetailsPV", obj);
                        }
                        return View("Error");
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
            }
        }


        [HttpPost]
        public async Task<ActionResult> NewBulkTransfer(string message)
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
                        obj.OrganizationId = orgid;
                        obj.CreatedBy = userid;
                        obj.message = message;
                        //obj.Quantity = Quantity;
                        //obj.SparePartId = SparePartId;
                        //obj.WarehouseId = warehouseid;

                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/InventoryAPI/NewInsertBulkTransfer", obj);
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
    }
}