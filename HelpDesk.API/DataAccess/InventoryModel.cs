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
    public class InventoryModel : IInventoryModel
    {
        public SqlDataReader CheckSparePartName(InventoryDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@NAME",obj.SparePartName),
                new SqlParameter("@warehouseid",obj.WarehouseId)
                };
                return DbConnector.ExecuteReader("USPCHECKSparePartName", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "InventoryModel -> CheckSparePartName");
                return null;
            }
        }

        public SqlDataReader ConsignmentStatus(InventoryDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@statusid",obj.Statusid),
                new SqlParameter("@userid",obj.CreatedBy),
                new SqlParameter("@comments",obj.Comments),
                new SqlParameter("@consignmentid",obj.ConsignmentId)
                };
                return DbConnector.ExecuteReader("uspUpdateConsignmentStatus", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "InventoryModel -> ConsignmentStatus");
                return null;
            }
        }
        public SqlDataReader InsertUpdateSparePart(InventoryDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@WarehouseId",obj.WarehouseId),
                new SqlParameter("@Name",obj.SparePartName),
                 new SqlParameter("@Number",obj.SparePartNumber),
                new SqlParameter("@Quantity",obj.Quantity),
                 new SqlParameter("@BaseQuantity",obj.BaseQuantity),
                new SqlParameter("@Price",obj.Price),
                new SqlParameter("@CreatedBy",obj.CreatedBy),
                new SqlParameter("@FlagId",obj.FlagId),
                 new SqlParameter("@SparePartId",obj.SparePartId),
                new SqlParameter("@OrganizationId",obj.Organizationid) 
                };
                return DbConnector.ExecuteReader("uspInserUpdateSpareParts", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "InventoryModel -> InsertUpdateSparePart");
                return null;
            }
        }
        public SqlDataReader InsertUpdateConsginment(InventoryDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@Sparepartid",obj.SparePartId),
                new SqlParameter("@quantity",obj.Quantity),
                 new SqlParameter("@comments",obj.Comments),
                new SqlParameter("@userid",obj.CreatedBy) ,
                new SqlParameter("@OrganizationId",obj.OrganizationId)
                };
                return DbConnector.ExecuteReader("uspAddConsignments", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "InventoryModel -> InsertUpdateConsginment");
                return null;
            }
        }
        
        public SqlDataReader SparePartById(InventoryDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@SparePartId",obj.SparePartId) 
                };
                return DbConnector.ExecuteReader("uspGetSparepartById", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "InventoryModel -> SparePartById");
                return null;
            }
        }
        public string SparePartList(InventoryDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@Warehouseid",obj.WarehouseId),
                new SqlParameter("@OrganizationId",obj.Organizationid) 
                };
                return DbConnector.ExecuteDataSet("uspGetSparePartList", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "InventoryModel -> SparePartList");
                return null;
            }
        }
        public string ConsignmentList(InventoryDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@OrganizationId",obj.Organizationid)
                };
                return DbConnector.ExecuteDataSet("uspGetPendingConsignment", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "InventoryModel -> ConsignmentList");
                return null;
            }
        }
        
        public string Warehouseddl(InventoryDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@OrganizationId",obj.Organizationid),
                new SqlParameter("@UserId",obj.CreatedBy)
                };
                return DbConnector.ExecuteDataSet("uspGetWarehouseddl", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "InventoryModel -> Warehouseddl");
                return null;
            }
        }
    }

    public interface IInventoryModel
    {
        SqlDataReader InsertUpdateSparePart(InventoryDTO obj);
        SqlDataReader InsertUpdateConsginment(InventoryDTO obj);
        SqlDataReader CheckSparePartName(InventoryDTO obj);
        SqlDataReader ConsignmentStatus(InventoryDTO obj);
        string SparePartList(InventoryDTO obj);
        string ConsignmentList(InventoryDTO obj);
        SqlDataReader SparePartById(InventoryDTO obj);
        string Warehouseddl(InventoryDTO obj);
    }
}