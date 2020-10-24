using HelpDesk.API.DataAccess;
using HelpDesk.API.DTO_s;
using HelpDesk.API.GenericHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API.Bussiness
{
    public class ProductService:IProductService
    {
        private readonly IProductModel model;
        public ProductService(ProductModel _model)
        {
            model = _model;

        }


        #region Product CURD Operation Interface implementation
        public IEnumerable<ProductDTO> GetProductList(ProductDTO obj)
        {
            var data = model.GetProductList(obj);
            var list = CustomDataReaderToGenericExtension.GetDataObjects<ProductDTO>(data);
            data.Close();
            return list;
        }
        public ProductDTO InsertUpdateProduct(ProductDTO obj)
        {
            try
            {
                var data = model.InsertUpdateProduct(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.message = data["message"].ToString();
                    }
                }
                else
                    obj.message = "0";
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }
         
        public ProductDTO GetProductDetailsById(ProductDTO obj)
        {
            try
            {
                var data = model.GetProductDetailsById(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.ProductName = data["ProductName"].ToString();
                        obj.ProductCode = data["ProductCode"].ToString();
                        obj.isActive = bool.Parse(data["isActive"].ToString());
                        obj.ProductId = obj.ProductId;

                    }
                }
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }
        #endregion
    }
    public interface IProductService
    {
        #region Product CURD Operation Interface (declartion)
        IEnumerable<ProductDTO> GetProductList(ProductDTO obj);
        ProductDTO InsertUpdateProduct(ProductDTO obj);
        ProductDTO GetProductDetailsById(ProductDTO obj);
        #endregion
    }
}