using HelpDesk.API.DataAccess;
using HelpDesk.API.DTO_s;
using HelpDesk.API.GenericHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API.Bussiness
{
    public class WarehouseService: IWarehouseService
    {
        private readonly IWarehouseModel model;
        public WarehouseService(WarehouseModel _model)
        {
            model = _model;
        }

        #region Warehouse CURD Operation Interface implementation
        public IEnumerable<WarehouseDTO> GetWarehouseList(WarehouseDTO obj)
        {
            var data = model.GetWarehouseList(obj);
            var list = CustomDataReaderToGenericExtension.GetDataObjects<WarehouseDTO>(data);
            data.Close();
            return list;
        }
        public WarehouseDTO InsertUpdateWarehouse(WarehouseDTO obj)
        {
            try
            {
                var data = model.InsertUpdateWarehouse(obj);
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
        public WarehouseDTO GetWarehouseDetailsById(WarehouseDTO obj)
        {
            try
            {
                var data = model.GetWarehouseDetailsById(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.WarehouseName = data["WarehouseName"].ToString();
                        obj.AHCCode = data["AHCCode"].ToString();
                        obj.isActive = bool.Parse(data["isActive"].ToString());
                        obj.WarehouseId = obj.WarehouseId;
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
        public WarehouseDTO AssignWarehouse(WarehouseDTO obj)
        {
            try
            {
                var data = model.AssignWarehouse(obj);
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
        public WarehouseDTO UpdateAssignWarehouse(WarehouseDTO obj)
        {
            try
            {
                var data = model.UpdateAssignWarehouse(obj);
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
        public WarehouseDTO drpdslist(WarehouseDTO obj)
        {
            obj.datasetxml = model.drpslist(obj);
            return obj;
            
        }
        
        #endregion
    }
    public interface IWarehouseService
    {
        #region Warehouse CURD Operation Interface (declartion)
        IEnumerable<WarehouseDTO> GetWarehouseList(WarehouseDTO obj);
        WarehouseDTO InsertUpdateWarehouse(WarehouseDTO obj);
        WarehouseDTO AssignWarehouse(WarehouseDTO obj);
        WarehouseDTO UpdateAssignWarehouse(WarehouseDTO obj);
        WarehouseDTO drpdslist(WarehouseDTO obj);
        WarehouseDTO GetWarehouseDetailsById(WarehouseDTO obj);
        #endregion
    }
}