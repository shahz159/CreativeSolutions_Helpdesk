using HelpDesk.API.Bussiness;
using HelpDesk.API.DTO_s;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Xml;

namespace HelpDesk.API.Controllers
{
    public class InventoryAPIController : ApiController
    {
        private readonly IInventoryService service;
        public InventoryAPIController(InventoryService _service)
        {
            service = _service;
        }
        /// <summary>
        /// Insert update spare part request
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewInsertSparePart(InventoryDTO obj)
        {
            var result = service.InsertUpdateSparePart(obj);
            return Ok(result);
        }

        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewInsertSparePartM(InventoryDTO obj)
        {
            var result = service.InsertUpdateSparePart(obj);
            string msg = "";
            bool val = false;

            if (result.message != "0")
                val = true;

            if (obj.message == "2")
                msg = "Spare Part Update Successfully.";
            else if (obj.message == "1")
                msg = "Spare Part Added Successfully.";
            else
                msg = "Something went wrong.";
            msg = val == true ? "" : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
        }

        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewInsertConsignment(InventoryDTO obj)
        {
            var result = service.InsertUpdateConsignment(obj);
            return Ok(result);
        }


        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewInsertConsignmentM(InventoryDTO obj)
        {
            var result = service.InsertUpdateConsignment(obj);
            string msg = "";
            bool val = false;

            if (result.message != "0")
                val = true;
            msg = "Stock Added Successfully.";
            msg = val == true ? "" : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
        }

        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewInsertBulkTransfer(InventoryDTO obj)
        {
            var result = service.InsertBulkTransfer(obj);
            return Ok(result);
        }

        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewUpdateSparePart(InventoryDTO obj)
        {
            var result = service.UpdateSparePart(obj);
            return Ok(result);
        }

        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewStockChangeRequest(InventoryDTO obj)
        {
            var result = service.StockChange(obj);
            return Ok(result);
        }

        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewStockChangeRequestM(InventoryDTO obj)
        {
            var result = service.StockChange(obj);
            string msg = "";
            bool val = false;
            if (result.message != "0")
                val = true;
            msg = val == true ? "Stock Updated Successfully." : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
        }

        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewTrasnferQuantity(InventoryDTO obj)
        {
            var result = service.TrasnferQuantity(obj);
            return Ok(result);
        }


        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewTrasnferQuantityM(InventoryDTO obj)
        {
            var result = service.TrasnferQuantity(obj);
            return Ok(result);
        }

        /// <summary>
        /// get spare part details by id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>

        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewGetSparePartDetailsById(InventoryDTO obj)
        {
            var detail = service.SparePartById(obj);
            return Ok(detail);
        }


        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewGetSparePartDetailsByIdStockList(InventoryDTO obj)
        {
            var result = service.SparePartById(obj);
            string msg = "";
            bool val = true;
            JArray JSparePartList = JArray.Parse("[]");
            if (result != null)
            {
                if (!string.IsNullOrEmpty(result.datasetxml))
                {
                    var document = new XmlDocument();
                    document.LoadXml(result.datasetxml);
                    DataSet ds = new DataSet();
                    ds.ReadXml(new XmlNodeReader(document));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            foreach (JObject item in strjarry)
                            {
                                var pv = item.SelectToken("WarehouseJson");
                                var pvj = JArray.Parse(pv.ToString());
                                item.Add(new JProperty("JWarehouseJson", pvj));

                                var Url = item.SelectToken("HistoryJson");
                                var Urlj = JArray.Parse(Url.ToString());
                                item.Add(new JProperty("JHistoryJson", Urlj));
                            }


                            if (!string.IsNullOrEmpty(str))
                                JSparePartList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JSparePartList = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("SparePartList", JSparePartList)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }

        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewGetSparePartDetailsByIdSP(InventoryDTO obj)
        {
            var detail = service.SparePartByIdSP(obj);
            return Ok(detail);
        }

        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewGetSparePartDetailsByIdSPM(InventoryDTO obj)
        {
            var result = service.SparePartByIdSP(obj);
            string msg = "";
            bool val = true;
            JArray JSparePartList = JArray.Parse("[]");
            if (result != null)
            {
                if (!string.IsNullOrEmpty(result.datasetxml))
                {
                    var document = new XmlDocument();
                    document.LoadXml(result.datasetxml);
                    DataSet ds = new DataSet();
                    ds.ReadXml(new XmlNodeReader(document));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JSparePartList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JSparePartList = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("SparePartDetails", JSparePartList)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }
        /// <summary>
        /// Get Details for Edit
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewGetSparePartDetailsByIdEdit(InventoryDTO obj)
        {
            var detail = service.SparePartByIdEdit(obj);
            return Ok(detail);
        }

        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewGetSparePartDetailsByIdEditM(InventoryDTO obj)
        {
            var result = service.SparePartByIdEdit(obj);
            string msg = "";
            bool val = true;
            JArray JProductList = JArray.Parse("[]");
            JArray JSparePartList = JArray.Parse("[]");
            if (result != null)
            {
                if (!string.IsNullOrEmpty(result.datasetxml))
                {
                    var document = new XmlDocument();
                    document.LoadXml(result.datasetxml);
                    DataSet ds = new DataSet();
                    ds.ReadXml(new XmlNodeReader(document));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JProductList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JProductList = strjarry;
                        }

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[1]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JSparePartList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[1]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JSparePartList = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("SparePartDetails", JSparePartList), new JProperty("ProductList", JProductList)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }

        /// <summary>
        /// Get Spare Part List for Organization admin and Comany Admin user
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewSparePartList(InventoryDTO obj)
        {
            var result = service.SparePartList(obj);
            return Ok(result);
        }

        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewSparePartListM(InventoryDTO obj)
        {
            var result = service.SparePartList(obj);
            string msg = "";
            bool val = true;
            JArray JSparePartList = JArray.Parse("[]");
            if (result != null)
            {
                if (!string.IsNullOrEmpty(result.datasetxml))
                {
                    var document = new XmlDocument();
                    document.LoadXml(result.datasetxml);
                    DataSet ds = new DataSet();
                    ds.ReadXml(new XmlNodeReader(document));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JSparePartList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JSparePartList = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("SparePartList", JSparePartList)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }

        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewSparePartListByWHId(InventoryDTO obj)
        {
            var result = service.SparePartListByWHId(obj);
            return Ok(result);
        }

        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewSparePartListByWHIdM(InventoryDTO obj)
        {
            var result = service.SparePartListByWHId(obj);
            string msg = "";
            bool val = true;
            JArray JSparePartList = JArray.Parse("[]");
            if (result != null)
            {
                if (!string.IsNullOrEmpty(result.datasetxml))
                {
                    var document = new XmlDocument();
                    document.LoadXml(result.datasetxml);
                    DataSet ds = new DataSet();
                    ds.ReadXml(new XmlNodeReader(document));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JSparePartList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JSparePartList = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("SparePartList", JSparePartList)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }

        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewConsignmentList(InventoryDTO obj)
        {
            var result = service.ConsignmentList(obj);
            return Ok(result);
        }

        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewConsignmentListM(InventoryDTO obj)
        {
            var result = service.ConsignmentList(obj);
            string msg = "";
            bool val = true;
            JArray JSparePartList = JArray.Parse("[]");
            if (result != null)
            {
                if (!string.IsNullOrEmpty(result.datasetxml))
                {
                    var document = new XmlDocument();
                    document.LoadXml(result.datasetxml);
                    DataSet ds = new DataSet();
                    ds.ReadXml(new XmlNodeReader(document));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JSparePartList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JSparePartList = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("InventoryAdjustmentList", JSparePartList)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }

        /// <summary>
        /// get warehouse dropdown
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewGetWarehouseDropDowns(InventoryDTO obj)
        {
            var result = service.Warehouseddl(obj);
            return Ok(result); 
        }

        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewGetWarehouseDropDownsM(InventoryDTO obj)
        {
            var result = service.Warehouseddl(obj);

            string msg = "";
            bool val = true;
            JArray JWarehouseList = JArray.Parse("[]");
            JArray JProductList = JArray.Parse("[]");
            JArray JSparePartList = JArray.Parse("[]");
            if (result != null)
            {
                if (!string.IsNullOrEmpty(result.datasetxml))
                {

                    var document = new XmlDocument();
                    document.LoadXml(result.datasetxml);
                    DataSet ds = new DataSet();
                    ds.ReadXml(new XmlNodeReader(document));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JWarehouseList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JWarehouseList = strjarry;
                        }
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[1]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JProductList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[1]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JProductList = strjarry;
                        }
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[2]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JSparePartList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[2]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JSparePartList = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("WarehouseList", JWarehouseList), new JProperty("ProductList", JProductList), new JProperty("SparePartList", JSparePartList)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }
        /// <summary>
        /// check spare part name
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewCheckSparePartName(InventoryDTO obj)
        {
            var detail = service.CheckSparePartName(obj);
            return Ok(detail);
        }

        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewConsignmentStatus(InventoryDTO obj)
        {
            var detail = service.ConsignmentStatus(obj);
            return Ok(detail);
        }

        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewConsignmentStatusM(InventoryDTO obj)
        {
            var detail = service.ConsignmentStatus(obj);
            string msg = "";
            bool val = true;

            msg = val == true ? "Consignment Status Updated Successfully." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)));
            return Ok(res);
        }


        /// <summary>
        /// get list of warehouse by spare part id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewGetListOfWarehouseBySparePart(InventoryDTO obj)
        {
            var result = service.WarehouseBySparePart(obj);
            return Ok(result);
        }
        /// <summary>
        /// Get Warehouse Stock Details by warehouse stock id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewGetSparePartDetailsByWareHouseStock(InventoryDTO obj)
        {
            var result = service.WarehouseStockById(obj);
            return Ok(result);
        }

        /// <summary>
        /// Main Page Enquiry details add
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewMainEnquiry(InventoryDTO obj)
        {
            var result = service.AddMainEnquiry(obj);
            return Ok(result);
        }


        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewMainEnquiryM(InventoryDTO obj)
        {
            var result = service.AddMainEnquiry(obj);
            string msg = "";
            bool val = false;
            if (result.message != "0")
                val = true;
            msg = val == true ? "Enquiry Sent Successfully." : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
        }

        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewGetMainEnquiryM(InventoryDTO obj)
        {
            var result = service.GetMainEnquiry(obj);
            string msg = "";
            bool val = true;
            JArray JEnquiryList = JArray.Parse("[]");
            if (result != null)
            {
                if (!string.IsNullOrEmpty(result.datasetxml))
                {
                    var document = new XmlDocument();
                    document.LoadXml(result.datasetxml);
                    DataSet ds = new DataSet();
                    ds.ReadXml(new XmlNodeReader(document));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JEnquiryList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JEnquiryList = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("EnquiryList", JEnquiryList)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }

        [ResponseType(typeof(InventoryDTO))]
        public IHttpActionResult NewGetMainEnquiry(InventoryDTO obj)
        {
            var detail = service.GetMainEnquiry(obj);
            return Ok(detail);
        }
    }
}
