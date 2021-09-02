using HelpDesk.API.Bussiness;
using HelpDesk.API.DTO_s;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HelpDesk.API.Controllers
{
    public class ProductAPIController : ApiController
    {
        private readonly IProductService service;
        public ProductAPIController(ProductService _service)
        {
            service = _service;
        }
        /// <summary>
        /// Get List of Product  with active and inactive
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<ProductDTO> NewGetProductsList(ProductDTO obj)
        {
            var list = service.GetProductList(obj);
            return list;
        }
        /// <summary>
        /// Insert a Product record if FlagId = 1 
        /// Update a Product record if FlagId = 2
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IHttpActionResult NewInsertUpdateProducts(ProductDTO obj)
        {
            var result = service.InsertUpdateProduct(obj);
            return Ok(result);
        }

        public IHttpActionResult NewInsertUpdateProductsM(ProductDTO obj)
        {
            var result = service.InsertUpdateProduct(obj);
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
        /// Get Product Details by Id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IHttpActionResult NewGetProductById(ProductDTO obj)
        {
            var result = service.GetProductDetailsById(obj);
            return Ok(result);
        }
    }
}
