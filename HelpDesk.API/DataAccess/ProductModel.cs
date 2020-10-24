using HelpDesk.API.DatabaseConnector;
using HelpDesk.API.DbHelpers;
using HelpDesk.API.DTO_s;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HelpDesk.API.DataAccess
{
    public class ProductModel: IProductModel
    {
        public SqlDataReader GetProductList(ProductDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@CompanyId", obj.CompanyId)
            };
                return DbConnector.ExecuteReader("UspGetProductList", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "ProductModel -> GetProductList");
                return null;
            }
        }
        public SqlDataReader InsertUpdateProduct(ProductDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@ProductCode",obj.ProductCode),
                    new SqlParameter("@ProductName",obj.ProductName),
                    new SqlParameter("@isActive",obj.isActive),
                    new SqlParameter("@ProductId",obj.ProductId),
                    new SqlParameter("@FlagId",obj.FlagId),
                    new SqlParameter("@CreatedBy",obj.CreatedBy),
                    new SqlParameter("@CompanyId",obj.CompanyId)

                };
                return DbConnector.ExecuteReader("uspInsertUpdateProduct", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "ProductModel -> InsertUpdateProduct");
                return null;
            }

        }

        public SqlDataReader GetProductDetailsById(ProductDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@ProductId",obj.ProductId)
                };
                return DbConnector.ExecuteReader("uspGetProductDetailsById", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "ProductModel -> GetProductDetailsById");
                return null;
            }
        }
    }
    public interface IProductModel
    {
        SqlDataReader GetProductList(ProductDTO obj);
        SqlDataReader InsertUpdateProduct(ProductDTO obj);
        SqlDataReader GetProductDetailsById(ProductDTO obj);
    }
}