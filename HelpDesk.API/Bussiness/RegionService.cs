using HelpDesk.API.DataAccess;
using HelpDesk.API.DTO_s;
using HelpDesk.API.GenericHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API.Bussiness
{
    public class RegionService: IRegionService
    {
        private readonly IRegionModel model;
        public RegionService(RegionModel _model)
        {
            model = _model;

        }


        #region Region CURD Operation Interface implementation
        public IEnumerable<RegionDTO> GetRegionList(RegionDTO obj)
        {
            var data = model.GetRegionList(obj);
            var list = CustomDataReaderToGenericExtension.GetDataObjects<RegionDTO>(data);
            data.Close();
            return list;
        }
        public RegionDTO InsertUpdateRegion(RegionDTO obj)
        {
            try
            {
                var data = model.InsertUpdateRegion(obj);
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
        public RegionDTO GetRegionDetailsById(RegionDTO obj)
        {
            try
            {
                var data = model.GetRegionDetailsById(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.RegionName = data["RegionName"].ToString();
                        obj.isActive = bool.Parse(data["isActive"].ToString());
                        obj.RegionId = obj.RegionId;
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
    public interface IRegionService
    {
        #region Region CURD Operation Interface (declartion)
        IEnumerable<RegionDTO> GetRegionList(RegionDTO obj);
        RegionDTO InsertUpdateRegion(RegionDTO obj);
        RegionDTO GetRegionDetailsById(RegionDTO obj);
        #endregion
    }
}