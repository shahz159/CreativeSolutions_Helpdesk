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

        public InventoryDTO SparePartById(InventoryDTO vm)
        {
            var data = model.SparePartById(vm);
            if (data.HasRows)
            {
                while (data.Read())
                {
                    vm.SparePartId = long.Parse(data["SparePartId"].ToString());
                    vm.WarehouseId = int.Parse(data["WarehouseId"].ToString());
                    vm.SparePartName =data["SparePartName"].ToString();
                    vm.SparePartNumber = data["SparePartNumber"].ToString();
                    vm.Quantity = int.Parse(data["Quantity"].ToString());
                    vm.BaseQuantity = int.Parse(data["BaseQuantity"].ToString());
                    vm.Price = data["Price"].ToString();
                }
                data.Close();
            }
            return vm;
        }

        public InventoryDTO SparePartList(InventoryDTO obj)
        {
            obj.datasetxml = model.SparePartList(obj);
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
        InventoryDTO CheckSparePartName(InventoryDTO obj);
        InventoryDTO SparePartList(InventoryDTO obj);
        InventoryDTO SparePartById(InventoryDTO obj);
        InventoryDTO Warehouseddl(InventoryDTO obj);
    }
}