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
    public class RegionAPIController : ApiController
    {
        private readonly IRegionService service;
        public RegionAPIController(RegionService _service)
        {
            service = _service;
        }
        /// <summary>
        /// Get List of Region  with active and inactive
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<RegionDTO> NewGetRegionsList(RegionDTO obj)
        {
            var list = service.GetRegionList(obj);
            return list;
        }

        /// <summary>
        /// Insert a Region record if FlagId = 1 
        /// Update a Region record if FlagId = 2
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IHttpActionResult NewInsertUpdateRegions(RegionDTO obj)
        {
            var result = service.InsertUpdateRegion(obj);
            return Ok(result);
        }
        /// <summary>
        /// Get Region Details by Id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IHttpActionResult NewGetRegionById(RegionDTO obj)
        {
            var result = service.GetRegionDetailsById(obj);
            return Ok(result);
        }
    }
}
