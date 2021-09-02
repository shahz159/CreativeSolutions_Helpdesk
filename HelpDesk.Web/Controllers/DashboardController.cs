using HelpDesk.Web.Handlers;
using HelpDesk.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;


namespace HelpDesk.Web.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public async Task<ActionResult> Index()
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
                        //int comid = int.Parse(Session["SSCompanyId"].ToString());
                        //int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                        TicketDTO obj = new TicketDTO();
                        obj.RoleId = roleid;
                        //obj.CompanyId = comid;
                        //obj.OrganizationId = orgid;
                        obj.CreatedBy = userid;

                        List<TicketDTO> tickettlst = new List<TicketDTO>();

                        HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/TicketsAPI/NewDashBoardCount", obj);
                        if (responseMessageViewDocuments.IsSuccessStatusCode)
                        {
                            var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);

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
                                        tickettlst = ds.Tables[0].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            NewTickets = dataRow.Field<int>("NewTickets"),
                                            ReportsJson = dataRow.Field<string>("NewTicketsJson"),
                                            InProgressTickets = dataRow.Field<int>("InProgressTickets"),
                                            WarehouseJson = dataRow.Field<string>("InProgressTicketsJson"),
                                            ResolvedTickets = dataRow.Field<int>("ResolvedTickets"),
                                            SparePartRequestJson = dataRow.Field<string>("ResolvedTicketsJson") ,
                                            message = dataRow.Field<string>("CountsJson"),
                                            NewUser = dataRow.Field<int>("NewUser"),
                                            WarrantyExpiredApprovalCount = dataRow.Field<int>("WarrantyExpiredApprovalCount"),
                                            SparePartRequestCount = dataRow.Field<int>("SparePartRequestCount"),
                                            AssetRenewalCount = dataRow.Field<int>("AssetRenewalCount"),
                                            AssetApprovalCount = dataRow.Field<int>("AssetApprovalCount"),
                                            InventoryAdjustment = dataRow.Field<int>("InventoryAdjustment"),
                                            PPMDatesApprovalCount = dataRow.Field<int>("PPMDatesApprovalCount"),
                                            ServiceEngineerListWithCount = dataRow.Field<string>("ServiceEngineerListWithCount"),
                                            ScheduleTickets = dataRow.Field<int>("ScheduleTickets"),
                                            PauseTickets = dataRow.Field<int>("PauseTickets"),
                                            ClosedTickets = dataRow.Field<int>("ClosedTickets"),
                                            ServiceEngineerJson = dataRow.Field<string>("ServiceEngineerJson")
                                        }).ToList();
                                        obj.TicketList = tickettlst;

                                        obj.NewTickets=obj.TicketList.FirstOrDefault().NewTickets;
                                        obj.InProgressTickets = obj.TicketList.FirstOrDefault().InProgressTickets;
                                        obj.ResolvedTickets = obj.TicketList.FirstOrDefault().ResolvedTickets;
                                        obj.SparePartRequestCount = obj.TicketList.FirstOrDefault().SparePartRequestCount;

                                        obj.ScheduleTickets = obj.TicketList.FirstOrDefault().ScheduleTickets;
                                        obj.PauseTickets = obj.TicketList.FirstOrDefault().PauseTickets;
                                        obj.ClosedTickets = obj.TicketList.FirstOrDefault().ClosedTickets;

                                        obj.NewUser = obj.TicketList.FirstOrDefault().NewUser;
                                        obj.WarrantyExpiredApprovalCount = obj.TicketList.FirstOrDefault().WarrantyExpiredApprovalCount;
                                        obj.AssetRenewalCount = obj.TicketList.FirstOrDefault().AssetRenewalCount;
                                        obj.AssetApprovalCount = obj.TicketList.FirstOrDefault().AssetApprovalCount;
                                        obj.InventoryAdjustment = obj.TicketList.FirstOrDefault().InventoryAdjustment;
                                        obj.PPMDatesApprovalCount = obj.TicketList.FirstOrDefault().PPMDatesApprovalCount;

                                        string ServiceEngineerList = obj.TicketList.FirstOrDefault().ServiceEngineerJson;
                                        var modelIIServiceEngineerList = JsonConvert.DeserializeObject<List<TicketDTO>>(ServiceEngineerList);
                                        obj.ServiceEngineerList = modelIIServiceEngineerList;

                                        string ServiceEngineerListWithCount = obj.TicketList.FirstOrDefault().ServiceEngineerListWithCount;
                                        var modelII = JsonConvert.DeserializeObject<List<TicketDTO>>(ServiceEngineerListWithCount);
                                        obj.ServiceEngineerListWithCountList = modelII;

                                        string accountsjson = obj.TicketList.FirstOrDefault().ReportsJson;
                                        var model = JsonConvert.DeserializeObject<List<TicketDTO>>(accountsjson);
                                        obj.ReportList = model;

                                        string warehousejson = obj.TicketList.FirstOrDefault().WarehouseJson;
                                        var modelwa = JsonConvert.DeserializeObject<List<TicketDTO>>(warehousejson);
                                        obj.WarehouseList = modelwa;

                                        string sparepart = obj.TicketList.FirstOrDefault().SparePartRequestJson;
                                        var modelsp = JsonConvert.DeserializeObject<List<TicketDTO>>(sparepart);
                                        obj.SparePartList = modelsp;

                                        string counts = obj.TicketList.FirstOrDefault().message;
                                        var countssp = JsonConvert.DeserializeObject<List<TicketDTO>>(counts);
                                        obj.CountLst = countssp;

                                        string[] New_array = obj.CountLst.Select(I => Convert.ToString(I.New)).ToArray();
                                        string[] Closed_array = obj.CountLst.Select(I => Convert.ToString(I.Closed)).ToArray();
                                        string[] Resolved_array = obj.CountLst.Select(I => Convert.ToString(I.Resolved)).ToArray();
                                        string[] Months_array = obj.CountLst.Select(I => Convert.ToString(I.Month)).ToArray();

                                        obj.New_array = new JavaScriptSerializer().Serialize(New_array);
                                        obj.Closed_array = new JavaScriptSerializer().Serialize(Closed_array);
                                        obj.Resolved_array = new JavaScriptSerializer().Serialize(Resolved_array);
                                        obj.Months_array = new JavaScriptSerializer().Serialize(Months_array);


                                        string[] Pie_array=new string[3];
                                        Pie_array[0] = obj.NewTickets.ToString();
                                        Pie_array[1] = obj.InProgressTickets.ToString();
                                        Pie_array[2] = obj.ResolvedTickets.ToString();
                                        obj.Pie_array = new JavaScriptSerializer().Serialize(Pie_array);
                                    }
                                    else
                                        obj.TicketList = tickettlst;
                                }
                            }
                        }
                        obj.RoleId = roleid;
                        return View(obj);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
            }
        }

        public async Task<ActionResult> GetEngineerTicketCounts(long UserId)
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
                        TicketDTO obj = new TicketDTO();

                        obj.UserId = UserId;

                        List<TicketDTO> modellst = new List<TicketDTO>();
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/TicketsAPI/NewServiceEngineerDashboardCounts", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                            var docs = JsonConvert.DeserializeObject<TicketDTO>(responseData);
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
                                        modellst = ds.Tables[0].AsEnumerable().Select(dataRow => new TicketDTO
                                        {
                                            //WarehouseStockId = dataRow.Field<long>("WarehouseStockId"),
                                            NewTickets = dataRow.Field<int>("NewTickets"),
                                            InProgressTickets = dataRow.Field<int>("InProgressTickets"),
                                            ResolvedTickets = dataRow.Field<int>("ResolvedTickets"),
                                            ScheduleTickets = dataRow.Field<int>("ScheduleTickets"),
                                            PauseTickets = dataRow.Field<int>("PauseTickets"),
                                            ClosedTickets = dataRow.Field<int>("ClosedTickets")
                                        }).ToList();
                                        obj.SparePartList = modellst;
                                    }
                                    else
                                        obj.SparePartList = modellst;
                                }
                                else
                                {
                                    obj.SparePartList = modellst;
                                    TempData["SparePartListtmp"] = obj.SparePartList;
                                }
                            }
                        }
                        return Json(obj.SparePartList);
                    }
                    catch (Exception ex)
                    {
                        TicketDTO obj = new TicketDTO();
                        return Json(obj.SparePartList);
                    }
                }
            }
        }

        // 
    }
}