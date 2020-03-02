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
    public class AssetsController : Controller
    {
        // GET: Assets
        public async Task<ActionResult> NewAssest()
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                //return Json("../Login/Index");
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        int userid = int.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        int comid = int.Parse(Session["SSCompanyId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                        AssetsDTO obj = new AssetsDTO();
                        obj.CompanyId = comid;
                        obj.OrganizationId = orgid;
                        obj.CreatedBy = userid;
                        obj.RoleId = roleid;

                        List<AssetsDTO> productlst = new List<AssetsDTO>();
                        List<AssetsDTO> accountlst = new List<AssetsDTO>();
                        List<AssetsDTO> regionlst = new List<AssetsDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/AssetAPI/NewGetDropDowns", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<AssetsDTO>(responseData);

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
                                        productlst = ds.Tables[0].AsEnumerable().Select(dataRow => new AssetsDTO
                                        {
                                            ProductId = dataRow.Field<int>("ProductId"),
                                            ProductName = dataRow.Field<string>("ProductName")
                                        }).ToList();
                                        obj.ProductList = productlst;
                                    }
                                    else
                                        obj.ProductList = productlst;

                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        accountlst = ds.Tables[1].AsEnumerable().Select(dataRow => new AssetsDTO
                                        {
                                            AccountId = dataRow.Field<int>("AccountId"),
                                            AccountName = dataRow.Field<string>("AccountName")
                                        }).ToList();
                                        obj.AccountList = accountlst;
                                    }
                                    else
                                        obj.AccountList = accountlst;

                                    if (ds.Tables[2].Rows.Count > 0)
                                    {
                                        regionlst = ds.Tables[2].AsEnumerable().Select(dataRow => new AssetsDTO
                                        {
                                            RegionId = dataRow.Field<int>("RegionId"),
                                            RegionName = dataRow.Field<string>("RegionName")
                                        }).ToList();
                                        obj.RegionList = regionlst;
                                    }
                                    else
                                        obj.RegionList = regionlst;
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

        public async Task<JsonResult> GetCity(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    AssetsDTO obj = new AssetsDTO();
                    obj.RegionId = id;
                    SelectList ddlcity = new SelectList("", "CityId", "CityName", 0);
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/AssetAPI/NewGetCityList", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var city = JsonConvert.DeserializeObject<List<AssetsDTO>>(responseData);
                        List<AssetsDTO> _objCitylLst = city.Where(x => x.isActive == true).ToList();
                        ddlcity = new SelectList(_objCitylLst, "CityId", "CityName", 0);
                    }
                    return Json(new SelectList(ddlcity, "Value", "Text"));
                }
                catch (Exception ex)
                {
                    return Json(new SelectList("", "Value", "Text"));
                }
            }

        }

        [HttpPost]
        public async Task<ActionResult> NewUpdateAssest(AssetsDTO obj)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                //return Json("../Login/Index");
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        int userid = int.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        int comid = int.Parse(Session["SSCompanyId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());

                        obj.CompanyId = comid;
                        obj.CreatedBy = userid;
                        obj.FlagId = 1;
                        if (obj.IsContract == false)
                        {
                            obj.POContract = "";
                            string dd = DateTime.Now.ToString("M/d/yyyy");
                            obj.WarrantyExpiryDate = DateTime.Parse(dd);
                        }

                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/AssetAPI/NewInsertUpdateAsset", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var categories = JsonConvert.DeserializeObject<AccountsDTO>(responseData);
                            obj.message = categories.message;
                            string msg = obj.message;
                        }
                        return Json(new { success = true });
                        //return RedirectToAction("NewAssest");
                    }
                    catch (Exception ex)
                    {
                        return View();
                    }
                }
            }
        }

        public async Task<ActionResult> Assets()
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                //return Json("../Login/Index");
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int userid = int.Parse(Session["SSUserId"].ToString());
                int roleid = int.Parse(Session["SSRoleId"].ToString());
                int comid = int.Parse(Session["SSCompanyId"].ToString());
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        AssetsDTO obj = new AssetsDTO();
                        //obj.CompanyId = comid;
                        obj.CreatedBy = userid;
                        obj.RoleId = roleid;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/AssetAPI/NewGetAssetList", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var assets = JsonConvert.DeserializeObject<List<AssetsDTO>>(responseData);
                            if (assets.Count != 0)
                                obj.AssetsList = assets;
                            else
                                obj.AssetsList = null;

                            //string assetmodeljson = obj.AssetModelJson;
                            //var asset_model_model = JsonConvert.DeserializeObject<List<AssetsDTO>>(assetmodeljson);
                            //obj.AssetModelList = asset_model_model;
                        }
                        return View(obj);
                    }
                    catch (Exception ex)
                    {
                        AssetsDTO obj = new AssetsDTO();
                        return View(obj);
                    }
                }
            }
        }

        public async Task<ActionResult> AssetDetails(int id)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                //int userid = int.Parse(Session["SSUserId"].ToString());
                int roleid = int.Parse(Session["SSRoleId"].ToString());
                //int comid = int.Parse(Session["SSCompanyId"].ToString());
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        AssetsDTO obj = new AssetsDTO();
                        obj.AMId = id;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/AssetAPI/NewGetAssetDetailsById", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var assets = JsonConvert.DeserializeObject<AssetsDTO>(responseData);
                            obj = assets;
                            string accountsjson = obj.PPMJson;
                            var model = JsonConvert.DeserializeObject<List<AssetsDTO>>(accountsjson);
                            obj.PPMList = model;

                            string productsjson = obj.ProductJson;
                            var model_pro = JsonConvert.DeserializeObject<List<AssetsDTO>>(productsjson);
                            obj.ProductList = model_pro;

                            string modeljson = obj.ModelJson;
                            var model_model = JsonConvert.DeserializeObject<List<AssetsDTO>>(modeljson);
                            obj.ModelList = model_model;

                            string regionjson = obj.RegionJson;
                            var model_region = JsonConvert.DeserializeObject<List<AssetsDTO>>(regionjson);
                            obj.RegionList = model_region;

                            string cityjson = obj.CityJson;
                            var model_city = JsonConvert.DeserializeObject<List<AssetsDTO>>(cityjson);
                            obj.CityList = model_city;

                            string updatedjson = obj.UpdatedJson;
                            var model_updated = JsonConvert.DeserializeObject<List<AssetsDTO>>(updatedjson);
                            obj.UpdatedList = model_updated;


                            string assetmodeljson = obj.AssetModelJson;
                            var asset_model_model = JsonConvert.DeserializeObject<List<AssetsDTO>>(assetmodeljson);
                            obj.AssetModelList = asset_model_model;

                        }
                        obj.RoleId = roleid;
                        return PartialView("AssetDetailsPV", obj);
                    }
                    catch (Exception ex)
                    {
                        AssetsDTO obj = new AssetsDTO();
                        return View(obj);
                    }
                }

            }
        }
        public ActionResult Edit()
        {
            return View();
        }
        public ActionResult Delete()
        {
            return View();
        }

        public async Task<ActionResult> UpdateAssetStatus(int id, int AMId)
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

                        AssetsDTO obj = new AssetsDTO();

                        obj.CreatedBy = userid;
                        if (id == 1)
                        {
                            obj.IsApproved = true;
                            obj.IsRejected = false;
                        }
                        else if (id == 2)
                        {
                            obj.IsApproved = false;
                            obj.IsRejected = true;
                        }
                        obj.AMId = AMId;
                        bool status = false;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/AssetAPI/NewUpdateAssetStatus", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);
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

        public async Task<ActionResult> PPMDateUpdateAssetStatus(int statusid, long UpdatedId)
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

                        AssetsDTO obj = new AssetsDTO();

                        obj.CreatedBy = userid;
                        obj.UpdatedId = UpdatedId;
                        obj.StatusId = statusid;

                        bool status = false;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/AssetAPI/NewUpdatePPMDateChange", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);
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
        public async Task<ActionResult> ApprovalAssets()
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                //int userid = int.Parse(Session["SSUserId"].ToString());
                //int roleid = int.Parse(Session["SSRoleId"].ToString());
                //int comid = int.Parse(Session["SSCompanyId"].ToString());
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        AssetsDTO obj = new AssetsDTO();
                        obj.OrganizationId = orgid;
                        List<AssetsDTO> tickettlst = new List<AssetsDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/AssetAPI/NewApprovalAssets", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<AssetsDTO>(responseData);

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
                                        tickettlst = ds.Tables[0].AsEnumerable().Select(dataRow => new AssetsDTO
                                        {
                                            AccountId = dataRow.Field<int>("AccountId"),
                                            AccountName = dataRow.Field<string>("AccountName"),
                                            AccountCode = dataRow.Field<string>("AccountCode"),
                                            ProductId = dataRow.Field<int>("ProductId"),
                                            ProductName = dataRow.Field<string>("ProductName"),
                                            ProductCode = dataRow.Field<string>("ProductCode"),
                                            //ModelId = dataRow.Field<int>("ModelId"),
                                            //ModelName = dataRow.Field<string>("ModelName"),
                                            StationName = dataRow.Field<string>("StationName"),
                                            //IPAddress = dataRow.Field<string>("IPAddress"),
                                            //SerialNo = dataRow.Field<string>("SerialNo"),
                                            SystemNo = dataRow.Field<string>("SystemNo"),
                                            //Configuration = dataRow.Field<string>("Configuration"),
                                            Area = dataRow.Field<string>("Area"),
                                            RegionName = dataRow.Field<string>("RegionName"),
                                            RegionId = dataRow.Field<int>("RegionId"),
                                            CityId = dataRow.Field<int>("CityId"),
                                            CityName = dataRow.Field<string>("CityName"),
                                            InstallationDate = dataRow.Field<DateTime>("InstallationDate"),
                                            IsContract = dataRow.Field<bool>("IsContract"),
                                            POContract = dataRow.Field<string>("POContract"),
                                            WarrantyExpiryDate = dataRow.Field<DateTime>("WarrantyExpiryDate"),
                                            AMId = dataRow.Field<int>("AMId"),
                                            FullName = dataRow.Field<string>("FullName"),
                                            EditMode = dataRow.Field<bool>("EditMode"),
                                            AssetModelJson = dataRow.Field<string>("ModelsJson")
                                        }).ToList();
                                        obj.AssetsList = tickettlst;

                                    }
                                    else
                                        obj.AssetsList = tickettlst;
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

        public async Task<ActionResult> PPMDatesApproval()
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                //int userid = int.Parse(Session["SSUserId"].ToString());
                //int roleid = int.Parse(Session["SSRoleId"].ToString());
                //int comid = int.Parse(Session["SSCompanyId"].ToString());
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        AssetsDTO obj = new AssetsDTO();
                        obj.OrganizationId = orgid;
                        List<AssetsDTO> tickettlst = new List<AssetsDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/AssetAPI/NewPPMDateChangeRequest", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<AssetsDTO>(responseData);

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
                                        tickettlst = ds.Tables[0].AsEnumerable().Select(dataRow => new AssetsDTO
                                        {
                                            AccountId = dataRow.Field<int>("AccountId"),
                                            AccountName = dataRow.Field<string>("AccountName"),
                                            AccountCode = dataRow.Field<string>("AccountCode"),
                                            ProductId = dataRow.Field<int>("ProductId"),
                                            ProductName = dataRow.Field<string>("ProductName"),
                                            ProductCode = dataRow.Field<string>("ProductCode"),
                                            ModelId = dataRow.Field<int>("ModelId"),
                                            ModelName = dataRow.Field<string>("ModelName"),
                                            StationName = dataRow.Field<string>("StationName"),
                                            IPAddress = dataRow.Field<string>("IPAddress"),
                                            SerialNo = dataRow.Field<string>("SerialNo"),

                                            Configuration = dataRow.Field<string>("Configuration"),
                                            Area = dataRow.Field<string>("Area"),
                                            RegionName = dataRow.Field<string>("RegionName"),
                                            RegionId = dataRow.Field<int>("RegionId"),
                                            CityId = dataRow.Field<int>("CityId"),
                                            CityName = dataRow.Field<string>("CityName"),
                                            InstallationDate = dataRow.Field<DateTime>("InstallationDate"),
                                            IsContract = dataRow.Field<bool>("IsContract"),
                                            POContract = dataRow.Field<string>("POContract"),
                                            WarrantyExpiryDate = dataRow.Field<DateTime>("WarrantyExpiryDate"),
                                            AMId = dataRow.Field<int>("AMId"),
                                            FullName = dataRow.Field<string>("FullName"),
                                            PreviousDate = dataRow.Field<DateTime>("PreviousDate"),
                                            NewDate = dataRow.Field<DateTime>("NewDate"),
                                            UpdatedId = dataRow.Field<long>("UpdatedId")

                                        }).ToList();
                                        obj.AssetsList = tickettlst;
                                    }
                                    else
                                        obj.AssetsList = tickettlst;
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


        public async Task<ActionResult> AddUpdatingAssetDetails(AssetsDTO obj)
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

                        obj.CreatedBy = userid;

                        bool status = false;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/AssetAPI/NewUpdatedAsset", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);
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
                        return Json(new { success = false });
                    }
                }
            }
        }
        public async Task<ActionResult> AssetUpdateApproval(AssetsDTO obj)
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

                        obj.CreatedBy = userid;

                        bool status = false;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/AssetAPI/NewVerifyAsset", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);
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
                        return Json(new { success = false });
                    }
                }
            }
        }

        public async Task<ActionResult> UpdatePPMSchedule(AssetsDTO obj)
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

                        obj.CreatedBy = userid;

                        bool status = false;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/AssetAPI/NewUpdatePPMDate", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);
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
                        return Json(new { success = false });
                    }
                }
            }
        }

        [HttpPost]
        public async Task<JsonResult> NewTicket(TicketDTO obj)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return Json(new { success = false });
            }
            else
            {

                long userid = long.Parse(Session["SSUserId"].ToString());
                int compid = int.Parse(Session["SSCompanyId"].ToString());
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());

                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {

                        obj.ContentType = "";
                        obj.Url = "";
                        obj.multipledocuments_xml = "";
                        obj.CreatedBy = userid;

                        if (compid == 0)
                            obj.CompanyId = obj.CompanyId;
                        else
                            obj.CompanyId = compid;
                        obj.OrganizationId = orgid;
                        obj.Description = "Auto Generated PPM Ticket.";
                        obj.ReportId = 9;
                        obj.Priority = "M";
                        obj.AMModelId = 0;

                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/TicketsAPI/NewInsertTicketRequest", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var tickets = JsonConvert.DeserializeObject<TicketDTO>(responseData);
                            obj.message = tickets.message;
                            string msg = obj.message;

                        }
                        return Json(new { success = true });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false });
                    }
                }
            }
        }

        public async Task<ActionResult> RenewalAssets()
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                //int userid = int.Parse(Session["SSUserId"].ToString());
                //int roleid = int.Parse(Session["SSRoleId"].ToString());
                //int comid = int.Parse(Session["SSCompanyId"].ToString());
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        AssetsDTO obj = new AssetsDTO();
                        obj.OrganizationId = orgid;
                        List<AssetsDTO> tickettlst = new List<AssetsDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/AssetAPI/NewRenewalAssetsList", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<AssetsDTO>(responseData);

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
                                        tickettlst = ds.Tables[0].AsEnumerable().Select(dataRow => new AssetsDTO
                                        {
                                            AccountId = dataRow.Field<int>("AccountId"),
                                            AccountName = dataRow.Field<string>("AccountName"),
                                            AccountCode = dataRow.Field<string>("AccountCode"),
                                            ProductId = dataRow.Field<int>("ProductId"),
                                            ProductName = dataRow.Field<string>("ProductName"),
                                            ProductCode = dataRow.Field<string>("ProductCode"),
                                            //ModelId = dataRow.Field<int>("ModelId"),
                                            //ModelName = dataRow.Field<string>("ModelName"),
                                            StationName = dataRow.Field<string>("StationName"),
                                            IPAddress = dataRow.Field<string>("IPAddress"),
                                            SerialNo = dataRow.Field<string>("SerialNo"),
                                            SystemNo = dataRow.Field<string>("SystemNo"),
                                            Configuration = dataRow.Field<string>("Configuration"),
                                            Area = dataRow.Field<string>("Area"),
                                            RegionName = dataRow.Field<string>("RegionName"),
                                            RegionId = dataRow.Field<int>("RegionId"),
                                            CityId = dataRow.Field<int>("CityId"),
                                            CityName = dataRow.Field<string>("CityName"),
                                            InstallationDate = dataRow.Field<DateTime>("InstallationDate"),
                                            IsContract = dataRow.Field<bool>("IsContract"),
                                            POContract = dataRow.Field<string>("POContract"),
                                            WarrantyExpiryDate = dataRow.Field<DateTime>("WarrantyExpiryDate"),
                                            AMId = dataRow.Field<int>("AMId"),
                                            FullName = dataRow.Field<string>("FullName")
                                            //,EditMode = dataRow.Field<bool>("EditMode")
                                        }).ToList();
                                        obj.AssetsList = tickettlst;
                                    }
                                    else
                                        obj.AssetsList = tickettlst;
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
        public ActionResult RenewalPartialView(int amid)
        {
            AssetsDTO obj = new AssetsDTO();
            obj.AMId = amid;
            return PartialView("RenewalAssetDetailsPV", obj);
        }

        [HttpPost]
        public async Task<ActionResult> NewAssestRenewalRequest(AssetsDTO obj)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                //return Json("../Login/Index");
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        int userid = int.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        int comid = int.Parse(Session["SSCompanyId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());

                        obj.CompanyId = comid;
                        obj.CreatedBy = userid;
                        obj.FlagId = 1;
                        if (obj.IsContract == false)
                        {
                            obj.POContract = "";
                            string dd = DateTime.Now.ToString("M/d/yyyy");
                            obj.WarrantyExpiryDate = DateTime.Parse(dd);
                            obj.InstallationDate = DateTime.Parse(dd);
                        }
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/AssetAPI/NewInsertRenewalRequestAssets", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var categories = JsonConvert.DeserializeObject<AccountsDTO>(responseData);
                            obj.message = categories.message;
                            string msg = obj.message;
                        }
                        return Json(new { success = true });
                        //return RedirectToAction("NewAssest");
                    }
                    catch (Exception ex)
                    {
                        return View();
                    }
                }
            }
        }
        public async Task<ActionResult> AssetRenewalRequest()
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                //int userid = int.Parse(Session["SSUserId"].ToString());
                //int roleid = int.Parse(Session["SSRoleId"].ToString());
                //int comid = int.Parse(Session["SSCompanyId"].ToString());
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        AssetsDTO obj = new AssetsDTO();
                        obj.OrganizationId = orgid;
                        List<AssetsDTO> tickettlst = new List<AssetsDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/AssetAPI/NewRenewalRequestAssetsList", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<AssetsDTO>(responseData);

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
                                        tickettlst = ds.Tables[0].AsEnumerable().Select(dataRow => new AssetsDTO
                                        {
                                            AccountId = dataRow.Field<int>("AccountId"),
                                            AccountName = dataRow.Field<string>("AccountName"),
                                            AccountCode = dataRow.Field<string>("AccountCode"),
                                            ProductId = dataRow.Field<int>("ProductId"),
                                            ProductName = dataRow.Field<string>("ProductName"),
                                            ProductCode = dataRow.Field<string>("ProductCode"),
                                            ModelId = dataRow.Field<int>("ModelId"),
                                            ModelName = dataRow.Field<string>("ModelName"),
                                            StationName = dataRow.Field<string>("StationName"),
                                            IPAddress = dataRow.Field<string>("IPAddress"),
                                            SerialNo = dataRow.Field<string>("SerialNo"),
                                            SystemNo = dataRow.Field<string>("SystemNo"),
                                            Configuration = dataRow.Field<string>("Configuration"),
                                            Area = dataRow.Field<string>("Area"),
                                            RegionName = dataRow.Field<string>("RegionName"),
                                            RegionId = dataRow.Field<int>("RegionId"),
                                            CityId = dataRow.Field<int>("CityId"),
                                            CityName = dataRow.Field<string>("CityName"),
                                            InstallationDate = dataRow.Field<DateTime>("InstallationDate"),
                                            IsContract = dataRow.Field<bool>("IsContract"),
                                            POContract = dataRow.Field<string>("POContract"),
                                            WarrantyExpiryDate = dataRow.Field<DateTime>("WarrantyExpiryDate"),
                                            AMId = dataRow.Field<int>("AMId"),
                                            FullName = dataRow.Field<string>("FullName")
                                        }).ToList();
                                        obj.AssetsList = tickettlst;
                                    }
                                    else
                                        obj.AssetsList = tickettlst;
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
        public async Task<ActionResult> RenewalAssetDetails(int id)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                //int userid = int.Parse(Session["SSUserId"].ToString());
                int roleid = int.Parse(Session["SSRoleId"].ToString());
                //int comid = int.Parse(Session["SSCompanyId"].ToString());
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        AssetsDTO obj = new AssetsDTO();
                        obj.AMId = id;
                        List<AssetsDTO> tickettlst = new List<AssetsDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/AssetAPI/NewRenewalAssetsDetails", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<AssetsDTO>(responseData);

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
                                        tickettlst = ds.Tables[0].AsEnumerable().Select(dataRow => new AssetsDTO
                                        {
                                            AMId = dataRow.Field<int>("AMId"),
                                            RenewalId = dataRow.Field<long>("RenewalId"),
                                            POContract = dataRow.Field<string>("POContract"),
                                            PPMType = dataRow.Field<int>("PPMType"),
                                            InstallationDate = dataRow.Field<DateTime>("InstallationDate"),
                                            WarrantyExpiryDate = dataRow.Field<DateTime>("WarrantyExpiryDate"),
                                            AccountId = dataRow.Field<int>("AccountId"),
                                            AccountName = dataRow.Field<string>("AccountName"),
                                            AccountCode = dataRow.Field<string>("AccountCode"),
                                            ProductId = dataRow.Field<int>("ProductId"),
                                            ProductName = dataRow.Field<string>("ProductName"),
                                            ProductCode = dataRow.Field<string>("ProductCode"),
                                            ModelId = dataRow.Field<int>("ModelId"),
                                            ModelName = dataRow.Field<string>("ModelName"),
                                            StationName = dataRow.Field<string>("StationName"),
                                            IPAddress = dataRow.Field<string>("IPAddress"),
                                            SerialNo = dataRow.Field<string>("SerialNo"),
                                            SystemNo = dataRow.Field<string>("SystemNo"),
                                            Configuration = dataRow.Field<string>("Configuration"),
                                            ContractTypetxt = dataRow.Field<string>("ContractTypetxt"),
                                            Area = dataRow.Field<string>("Area")
                                        }).ToList();
                                        obj.AssetsList = tickettlst;
                                    }
                                    else
                                        obj.AssetsList = tickettlst;
                                }
                            }
                        }
                        return PartialView("RenewalRequestDetailsPV", obj);
                    }
                    catch (Exception ex)
                    {
                        AssetsDTO obj = new AssetsDTO();
                        return View(obj);
                    }
                }

            }
        }
        public async Task<ActionResult> UpdateAssetRenewalStatus(long RenewalId, int AMId, int id)
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
                        AssetsDTO obj = new AssetsDTO();

                        obj.CreatedBy = userid;
                        obj.StatusId = id;
                        obj.AMId = AMId;
                        obj.RenewalId = RenewalId;

                        bool status = false;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/AssetAPI/NewUpdateAssetRenewalRequest", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);
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

        //NewAssetModelsInsert

        [HttpPost]
        public async Task<JsonResult> NewAssetModels(AssetsDTO obj)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return Json(new { success = false });
            }
            else
            {

                long userid = long.Parse(Session["SSUserId"].ToString());
                int compid = int.Parse(Session["SSCompanyId"].ToString());
                int orgid = int.Parse(Session["SSOrganizationId"].ToString());

                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        obj.CreatedBy = userid;

                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/AssetAPI/NewAssetModelsInsert", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var tickets = JsonConvert.DeserializeObject<TicketDTO>(responseData);
                            obj.message = tickets.message;
                            string msg = obj.message;

                        }
                        return Json(new { success = true });
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