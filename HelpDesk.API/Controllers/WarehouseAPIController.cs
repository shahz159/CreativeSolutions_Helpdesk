using HelpDesk.API.Bussiness;
using HelpDesk.API.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
    }
}
