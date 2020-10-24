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
    public class WarehouseModel: IWarehouseModel
    {
        public SqlDataReader GetWarehouseList(WarehouseDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@organizationId", obj.OrganizationId)
            };
                return DbConnector.ExecuteReader("UspGetWarehouseListMastr", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "WarehouseModel -> GetWarehouseList");
                return null;
            }
        }
        public SqlDataReader InsertUpdateWarehouse(WarehouseDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@WarehouseId",obj.WarehouseId),
                    new SqlParameter("@WarehouseName",obj.WarehouseName),
                    new SqlParameter("@AHCCode",obj.AHCCode),
                    new SqlParameter("@isActive",obj.isActive),
                    new SqlParameter("@FlagId",obj.FlagId),
                    new SqlParameter("@CreatedBy",obj.CreatedBy),
                    new SqlParameter("@OrgId",obj.OrganizationId)

                };
                return DbConnector.ExecuteReader("uspInsertUpdateWarehouse", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "WarehouseModel -> InsertUpdateWarehouse");
                return null;
            }

        }
        public SqlDataReader AssignWarehouse(WarehouseDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@WarehouseId",obj.WarehouseId),
                    new SqlParameter("@UserId",obj.UserId),
                    new SqlParameter("@CreatedBy",obj.CreatedBy) 

                };
                return DbConnector.ExecuteReader("uspInsertAssignWarehouse", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "WarehouseModel -> uspInsertAssignWarehouse");
                return null;
            }

        }

        public SqlDataReader UpdateAssignWarehouse(WarehouseDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@MUWid",obj.MUWId) 

                };
                return DbConnector.ExecuteReader("uspUpdateStatusofAsignWarehouse", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "WarehouseModel -> uspUpdateStatusofAsignWarehouse");
                return null;
            }

        }

        public SqlDataReader GetWarehouseDetailsById(WarehouseDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@WarehouseId",obj.WarehouseId)
                };
                return DbConnector.ExecuteReader("uspGetWarehouseDetailsById", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "WarehouseModel -> GetAccountDetailsById");
                return null;
            }
        }
        public string drpslist(WarehouseDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@OrganizationId",obj.OrganizationId)
                };
                return DbConnector.ExecuteDataSet("uspGetWarehouseDrp", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "WarehouseModel -> GetAccountDetailsById");
                return null;
            }
        }
    }
    public interface IWarehouseModel
    {
        SqlDataReader GetWarehouseList(WarehouseDTO obj);
        SqlDataReader InsertUpdateWarehouse(WarehouseDTO obj);
        SqlDataReader AssignWarehouse(WarehouseDTO obj);
        SqlDataReader UpdateAssignWarehouse(WarehouseDTO obj);
        SqlDataReader GetWarehouseDetailsById(WarehouseDTO obj);
        string drpslist(WarehouseDTO obj);
    }
}