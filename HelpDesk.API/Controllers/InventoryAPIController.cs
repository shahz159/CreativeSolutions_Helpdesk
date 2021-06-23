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
        public IHttpActionResult NewInsertConsignment(InventoryDTO obj)
        {
            var result = service.InsertUpdateConsignment(obj);
            return Ok(result);
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
        public IHttpActionResult NewTrasnferQuantity(InventoryDTO obj)
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
        public IHttpActionResult NewGetSparePartDetailsByIdSP(InventoryDTO obj)
        {
            var detail = service.SparePartByIdSP(obj);
            return Ok(detail);
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
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("WarehouseList", JWarehouseList)
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
    }
}
