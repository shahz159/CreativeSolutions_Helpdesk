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
using System.Xml;

namespace HelpDesk.API.Controllers
{
    public class WarehouseAPIController : ApiController
    {
        private readonly IWarehouseService service;
        public WarehouseAPIController(WarehouseService _service)
        {
            service = _service;
        }
        /// <summary>
        /// Get List of Warehouse  with active and inactive
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<WarehouseDTO> NewGetWarehousesList(WarehouseDTO obj)
        {
            var list = service.GetWarehouseList(obj);
            return list;
        }
        
        /// <summary>
        /// Insert a Warehouse record if FlagId = 1 
        /// Update a Warehouse record if FlagId = 2
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IHttpActionResult NewInsertUpdateWarehouses(WarehouseDTO obj)
        {
            var result = service.InsertUpdateWarehouse(obj);
            return Ok(result);
        }

        public IHttpActionResult NewInsertUpdateWarehousesM(WarehouseDTO obj)
        {
            var result = service.InsertUpdateWarehouse(obj);
            string msg = "";
            bool val = false;

            if (result.message != "0")
                val = true;

            if (obj.FlagId == 2)
                msg = "Warehouse Added Successfully.";
            else if (obj.FlagId == 1)
                msg = "Warehouse Updated Successfully.";
            msg = val == true ? "" : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
        }

        /// <summary>
        /// Get Warehouse Details by Id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IHttpActionResult NewGetWarehouseById(WarehouseDTO obj)
        {
            var result = service.GetWarehouseDetailsById(obj);
            return Ok(result);
        }
        /// <summary>
        /// Assign service engineer to warhouse
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IHttpActionResult NewAssignWarehouses(WarehouseDTO obj)
        {
            var result = service.AssignWarehouse(obj);
            return Ok(result);
        }

        public IHttpActionResult NewAssignWarehousesM(WarehouseDTO obj)
        {
            var result = service.AssignWarehouse(obj);
            string msg = "";
            bool val = false;

            if (result.message != "0")
                val = true;

            if (obj.FlagId == 2)
                msg = "Assigned Successfully.";
            else if (obj.FlagId == 1)
                msg = "Assigned Successfully.";
            msg = val == true ? "" : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
        }
        /// <summary>
        /// Update Status of Assign warehouse
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IHttpActionResult NewUpdateAssignWarehouses(WarehouseDTO obj)
        {
            var result = service.UpdateAssignWarehouse(obj);
            return Ok(result);
        }

        public IHttpActionResult NewDrpDpwnsandList(WarehouseDTO obj)
        {
            var result = service.drpdslist(obj);
            return Ok(result);
        }
        public IHttpActionResult NewDrpDpwnsandListM(WarehouseDTO obj)
        {
            var result = service.drpdslist(obj);
            string msg = "";
            bool val = true;
            JArray JWarehouseList = JArray.Parse("[]");
            JArray JServiceEngList = JArray.Parse("[]");
            JArray JAssignedServiceEngList = JArray.Parse("[]");
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
                                JServiceEngList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[1]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JServiceEngList = strjarry;
                        }

                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[2]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JAssignedServiceEngList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[2]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JAssignedServiceEngList = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("WarehouseList", JWarehouseList)
                , new JProperty("ServiceEngineerList", JServiceEngList)
                , new JProperty("AssignedServiceEngineerList", JAssignedServiceEngList)
                
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }
    }
}
