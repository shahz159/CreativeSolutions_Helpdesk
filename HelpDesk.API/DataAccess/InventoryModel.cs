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
                new SqlParameter("@ProductId",obj.ProductId),
                new SqlParameter("@Name",obj.SparePartName),
                 new SqlParameter("@Number",obj.SparePartNumber),
                new SqlParameter("@Quantity",obj.Quantity),
                 new SqlParameter("@BaseQuantity",obj.BaseQuantity),
                new SqlParameter("@Price",obj.Price),
                new SqlParameter("@CreatedBy",obj.CreatedBy),
                new SqlParameter("@FlagId",obj.FlagId),
                 new SqlParameter("@SparePartId",obj.SparePartId),
                new SqlParameter("@OrganizationId",obj.OrganizationId) 
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
                //new SqlParameter("@Sparepartid",obj.SparePartId),
                //new SqlParameter("@quantity",obj.Quantity),
                //new SqlParameter("@comments",obj.Comments),
                new SqlParameter("@Json",obj.message),
                    new SqlParameter("@userid",obj.CreatedBy),
                new SqlParameter("@OrganizationId",obj.OrganizationId)
                //new SqlParameter("@WarehouseId",obj.WarehouseId)
                };
                return DbConnector.ExecuteReader("uspAddConsignments", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "InventoryModel -> InsertUpdateConsginment");
                return null;
            }
        }
        public SqlDataReader InsertBulkTransfer(InventoryDTO obj)
        {
            try
            {
                var para = new[] {
                //new SqlParameter("@Sparepartid",obj.SparePartId),
                //new SqlParameter("@quantity",obj.Quantity),
                //new SqlParameter("@comments",obj.Comments),
                new SqlParameter("@Json",obj.message),
                    new SqlParameter("@CreatedBy",obj.CreatedBy)
                //new SqlParameter("@WarehouseId",obj.WarehouseId)
                };
                return DbConnector.ExecuteReader("uspBulkTransfer", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "InventoryModel -> InsertUpdateConsginment");
                return null;
            }
        }

        
        public SqlDataReader UpdateSparePart(InventoryDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@SparePartName",obj.SparePartName),
                new SqlParameter("@Price",obj.Price),
                new SqlParameter("@Quantity",obj.Quantity),
                new SqlParameter("@BaseQuantity",obj.BaseQuantity),
                new SqlParameter("@SparepartId",obj.SparePartId)
                };
                return DbConnector.ExecuteReader("uspUpdateSparePart", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "InventoryModel -> UpdateSparePart");
                return null;
            }
        }
        public SqlDataReader stockchnage(InventoryDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@WarehouseStockId",obj.WarehousestockId),
                new SqlParameter("@Quantity",obj.Quantity),
                new SqlParameter("@Type",obj.Type),
                new SqlParameter("@Status",obj.Statusid),
                new SqlParameter("@CreatedBy",obj.CreatedBy),
                new SqlParameter("@Organizationid",obj.OrganizationId)
                };
                return DbConnector.ExecuteReader("uspStockApproval", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "InventoryModel -> stockchnage");
                return null;
            }
        }
        public SqlDataReader transferquantity(InventoryDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@Quantity",obj.Quantity),
                new SqlParameter("@ToWHId",obj.ToWarehouseId),
                new SqlParameter("@SparePartId",obj.SparePartId),
                new SqlParameter("@CreatedBy",obj.CreatedBy),
                new SqlParameter("@WarehousestockId",obj.WarehousestockId)
                };
                return DbConnector.ExecuteReader("uspTransferQuantity", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "InventoryModel -> transferquantity");
                return null;
            }
        }

        
        public string SparePartById(InventoryDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@SparePartId",obj.SparePartId) ,
                new SqlParameter("@WarehouseId",obj.WarehouseId)
                };
                return DbConnector.ExecuteDataSet("uspGetSparepartById", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "InventoryModel -> SparePartById");
                return null;
            }
        }
        public string SparePartByIdSP(InventoryDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@SparePartId",obj.SparePartId) ,
                new SqlParameter("@WarehouseId",obj.WarehouseId)
                };
                return DbConnector.ExecuteDataSet("uspGetSPDetails", para);
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
                new SqlParameter("@OrganizationId",obj.OrganizationId)
                };
                return DbConnector.ExecuteDataSet("uspGetSparePartMasterList", para);

                //Get SparePart by WarehouseId
                //var para = new[] {
                //new SqlParameter("@Warehouseid",obj.WarehouseId),
                //new SqlParameter("@OrganizationId",obj.OrganizationId) 
                //};
                //return DbConnector.ExecuteDataSet("uspGetSparePartList", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "InventoryModel -> SparePartList");
                return null;
            }
        }
        public string WarehouseBySparePart(InventoryDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@SparePartId",obj.SparePartId)
                };
                return DbConnector.ExecuteDataSet("uspGetListOfWarehouseBySparePart", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "InventoryModel -> WarehouseBySparePart");
                return null;
            }
        }
        public string WarehouseStockDetailsById(InventoryDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@WarehouseStockId",obj.WarehousestockId)
                };
                return DbConnector.ExecuteDataSet("uspGetSparePartDetailsByWareHouseStock", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "InventoryModel -> WarehouseStockDetailsById");
                return null;
            }
        }
        

        public string SparePartListByWHId(InventoryDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@Warehouseid",obj.WarehouseId),
                new SqlParameter("@OrganizationId",obj.OrganizationId)
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
                new SqlParameter("@OrganizationId",obj.OrganizationId)
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
                new SqlParameter("@OrganizationId",obj.OrganizationId),
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
        SqlDataReader InsertBulkTransfer(InventoryDTO obj);
        SqlDataReader UpdateSparePart(InventoryDTO obj);
        SqlDataReader stockchnage(InventoryDTO obj);
        SqlDataReader transferquantity(InventoryDTO obj);
        SqlDataReader CheckSparePartName(InventoryDTO obj);
        SqlDataReader ConsignmentStatus(InventoryDTO obj);
        string SparePartList(InventoryDTO obj);
        string WarehouseBySparePart(InventoryDTO obj);
        string WarehouseStockDetailsById(InventoryDTO obj);
        string SparePartListByWHId(InventoryDTO obj);
        string ConsignmentList(InventoryDTO obj);
        string SparePartById(InventoryDTO obj);
        string SparePartByIdSP(InventoryDTO obj);
        string Warehouseddl(InventoryDTO obj);
    }
}