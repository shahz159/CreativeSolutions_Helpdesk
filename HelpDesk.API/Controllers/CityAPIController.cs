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
    public class CityAPIController : ApiController
    {
        private readonly ICityService service;
        public CityAPIController(CityService _service)
        {
            service = _service;
        }
        /// <summary>
        /// Get List of Product  with active and inactive
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<CityDTO> NewGetRegionList(CityDTO obj)
        {
            var list = service.GetRegionList(obj);
            return list;
        }
        /// <summary>
        /// Get City List
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<CityDTO> NewGetCityList(CityDTO obj)
        {
            var list = service.GetCityList(obj);
            return list;
        }

        /// <summary>
        /// Insert a City record if FlagId = 1 
        /// Update a City record if FlagId = 2
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IHttpActionResult NewInsertUpdateCity(CityDTO obj)
        {
            var result = service.InsertUpdateCity(obj);
            return Ok(result);
        }
        /// <summary>
        /// Get City Details by Id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IHttpActionResult NewGetCityById(CityDTO obj)
        {
            var result = service.GetCityDetailsById(obj);
            return Ok(result);
        }
    }
}
