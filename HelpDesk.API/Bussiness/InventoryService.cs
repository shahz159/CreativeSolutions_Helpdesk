using HelpDesk.API.DataAccess;
using HelpDesk.API.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API.Bussiness
{
    public class InventoryService: IInventoryService
    {
        private readonly IInventoryModel model;
        public InventoryService(InventoryModel _model)
        {
            model = _model;
        }

        public InventoryDTO CheckSparePartName(InventoryDTO obj)
        {
            try
            {
                var data = model.CheckSparePartName(obj);
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
        public InventoryDTO ConsignmentStatus(InventoryDTO obj)
        {
            try
            {
                var data = model.ConsignmentStatus(obj);
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

        

        public InventoryDTO InsertUpdateSparePart(InventoryDTO obj)
        {
            try
            {
                var data = model.InsertUpdateSparePart(obj);
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
        public InventoryDTO InsertUpdateConsignment(InventoryDTO obj)
        {
            try
            {
                var data = model.InsertUpdateConsginment(obj);
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

        public InventoryDTO InsertBulkTransfer(InventoryDTO obj)
        {
            try
            {
                var data = model.InsertBulkTransfer(obj);
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
        
        public InventoryDTO UpdateSparePart(InventoryDTO obj)
        {
            try
            {
                var data = model.UpdateSparePart(obj);
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
        public InventoryDTO AddMainEnquiry(InventoryDTO obj)
        {
            try
            {
                var data = model.addmainenquiry(obj);
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

        
        public InventoryDTO StockChange(InventoryDTO obj)
        {
            try
            {
                var data = model.stockchnage(obj);
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
        public InventoryDTO TrasnferQuantity(InventoryDTO obj)
        {
            try
            {
                var data = model.transferquantity(obj);
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
        


        public InventoryDTO SparePartById(InventoryDTO obj)
        {

            obj.datasetxml = model.SparePartById(obj);
            return obj;

            //var data = model.SparePartById(vm);
            //if (data.HasRows)
            //{
            //    while (data.Read())
            //    {
            //        vm.SparePartId = long.Parse(data["SparePartId"].ToString());
            //        vm.ProductId = int.Parse(data["ProductId"].ToString());
            //        vm.SparePartName =data["SparePartName"].ToString();
            //        vm.SparePartNumber = data["SparePartNumber"].ToString();
            //        vm.Quantity = int.Parse(data["Quantity"].ToString());
            //        vm.BaseQuantity = int.Parse(data["BaseQuantity"].ToString());
            //        vm.Price = data["Price"].ToString();
            //        vm.ConsignmentsJson = data["ConsignmentsJson"].ToString();
            //        vm.WarehouseJson = data["WarehouseJson"].ToString();
            //    }
            //    data.Close();
            //}
            //return vm;
        }
        public InventoryDTO SparePartByIdSP(InventoryDTO obj)
        {

            obj.datasetxml = model.SparePartByIdSP(obj);
            return obj;
           
        }
        public InventoryDTO SparePartByIdEdit(InventoryDTO obj)
        {

            obj.datasetxml = model.SparePartByIdSEdit(obj);
            return obj;

        }
        
        public InventoryDTO SparePartList(InventoryDTO obj)
        {
            obj.datasetxml = model.SparePartList(obj);
            return obj;
        }
        public InventoryDTO WarehouseBySparePart(InventoryDTO obj)
        {
            obj.datasetxml = model.WarehouseBySparePart(obj);
            return obj;
        }
        public InventoryDTO WarehouseStockById(InventoryDTO obj)
        {
            obj.datasetxml = model.WarehouseStockDetailsById(obj);
            return obj;
        }
        
        public InventoryDTO SparePartListByWHId(InventoryDTO obj)
        {
            obj.datasetxml = model.SparePartListByWHId(obj);
            return obj;
        }
        
        public InventoryDTO ConsignmentList(InventoryDTO obj)
        {
            obj.datasetxml = model.ConsignmentList(obj);
            return obj;
        }
        
        public InventoryDTO Warehouseddl(InventoryDTO obj)
        {
            obj.datasetxml = model.Warehouseddl(obj);
            return obj;
        }
    }

    public interface IInventoryService
    {
        InventoryDTO InsertUpdateSparePart(InventoryDTO obj);
        InventoryDTO InsertUpdateConsignment(InventoryDTO obj);
        InventoryDTO InsertBulkTransfer(InventoryDTO obj);
        InventoryDTO UpdateSparePart(InventoryDTO obj);
        InventoryDTO AddMainEnquiry(InventoryDTO obj);
        InventoryDTO StockChange(InventoryDTO obj);
        InventoryDTO TrasnferQuantity(InventoryDTO obj);
        InventoryDTO CheckSparePartName(InventoryDTO obj);
        InventoryDTO ConsignmentStatus(InventoryDTO obj);
        InventoryDTO SparePartList(InventoryDTO obj);
        InventoryDTO WarehouseBySparePart(InventoryDTO obj);
        InventoryDTO WarehouseStockById(InventoryDTO obj);
        InventoryDTO SparePartListByWHId(InventoryDTO obj);
        InventoryDTO ConsignmentList(InventoryDTO obj);
        InventoryDTO SparePartById(InventoryDTO obj);
        InventoryDTO SparePartByIdSP(InventoryDTO obj);
        InventoryDTO SparePartByIdEdit(InventoryDTO obj);
        InventoryDTO Warehouseddl(InventoryDTO obj);
    }
}