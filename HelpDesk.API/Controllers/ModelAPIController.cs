using HelpDesk.API.Bussiness;
using HelpDesk.API.DTO_s;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HelpDesk.API.Controllers
{
    public class ModelAPIController : ApiController
    {
        private readonly IModelService service;
        public ModelAPIController(ModelService _service)
        {
            service = _service;
        }
        /// <summary>
        /// Get List of Product  with active and inactive
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<ModelDTO> NewGetProductsList(ModelDTO obj)
        {
            var list = service.GetProductList(obj);
            return list;
        }
        /// <summary>
        /// Get Model List
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<ModelDTO> NewGetModelList(ModelDTO obj)
        {
            var list = service.GetModelList(obj);
            return list;
        }

        /// <summary>
        /// Insert a Model record if FlagId = 1 
        /// Update a Model record if FlagId = 2
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IHttpActionResult NewInsertUpdateModel(ModelDTO obj)
        {
            var result = service.InsertUpdateModel(obj);
            return Ok(result);
        }

        public IHttpActionResult NewInsertUpdateModelM(ModelDTO obj)
        {
            var result = service.InsertUpdateModel(obj);
            string msg = "";
            bool val = false;

            if (result.message != "0")
                val = true;
            msg = val == true ? "Added Successfully." : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
        }
        /// <summary>
        /// Get Model Details by Id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IHttpActionResult NewGetModelById(ModelDTO obj)
        {
            var result = service.GetModelDetailsById(obj);
            return Ok(result);
        }
    }
}
