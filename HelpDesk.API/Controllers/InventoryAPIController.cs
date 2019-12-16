using HelpDesk.API.Bussiness;
using HelpDesk.API.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

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

    }
}
