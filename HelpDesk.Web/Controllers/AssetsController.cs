﻿using HelpDesk.Web.Handlers;
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

                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/AssetAPI/NewGetAssetList", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var assets = JsonConvert.DeserializeObject<List<AssetsDTO>>(responseData);
                            if (assets.Count != 0)
                                obj.AssetsList = assets;
                            else
                                obj.AssetsList = null;
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
                        if (id==1)
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

    }
}