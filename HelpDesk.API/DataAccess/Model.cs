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
    public class Model: IModel
    {
        public SqlDataReader GetModelList(ModelDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@CompanyId", obj.CompanyId)
            };
                return DbConnector.ExecuteReader("UspGetModelList", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "Model -> UspGetModelList");
                return null;
            }
        }
        public SqlDataReader InsertUpdateModel(ModelDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@ModelName",obj.ModelName),
                    new SqlParameter("@isActive",obj.isActive),
                    new SqlParameter("@ProductId",obj.ProductId),
                    new SqlParameter("@FlagId",obj.FlagId),
                    new SqlParameter("@CreatedBy",obj.CreatedBy),
                    new SqlParameter("@ModelId",obj.ModelId)

                };
                return DbConnector.ExecuteReader("uspInsertUpdateModel", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "ProductModel -> uspInsertUpdateModel");
                return null;
            }

        }
        public SqlDataReader GetModelDetailsById(ModelDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@ModelId",obj.ModelId)
                };
                return DbConnector.ExecuteReader("uspGetModelDetailsById", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "Model -> GetModelDetailsById");
                return null;
            }
        }

        public SqlDataReader GetProductList(ModelDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@CompanyId", obj.CompanyId)
            };
                return DbConnector.ExecuteReader("UspGetProductDropDownList", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "Model -> GetProductList");
                return null;
            }
        }
    }

    public interface IModel
    {
        SqlDataReader GetModelList(ModelDTO obj);
        SqlDataReader InsertUpdateModel(ModelDTO obj);
        SqlDataReader GetModelDetailsById(ModelDTO obj);
        SqlDataReader GetProductList(ModelDTO obj);
    }
}