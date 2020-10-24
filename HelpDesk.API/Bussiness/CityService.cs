using HelpDesk.API.DataAccess;
using HelpDesk.API.DTO_s;
using HelpDesk.API.GenericHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API.Bussiness
{
    public class CityService:ICityService
    {
        private readonly ICityModel City;
        public CityService(CityModel _City)
        {
            City = _City;
        }

        #region Product CURD Operation Interface implementation
        public IEnumerable<CityDTO> GetCityList(CityDTO obj)
        {
            var data = City.GetCityList(obj);
            var list = CustomDataReaderToGenericExtension.GetDataObjects<CityDTO>(data);
            data.Close();
            return list;
        }
        public IEnumerable<CityDTO> GetRegionList(CityDTO obj)
        {
            var data = City.GetRegionList(obj);
            var list = CustomDataReaderToGenericExtension.GetDataObjects<CityDTO>(data);
            data.Close();
            return list;
        }
        public CityDTO InsertUpdateCity(CityDTO obj)
        {
            try
            {
                var data = City.InsertUpdateCity(obj);
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
        public CityDTO GetCityDetailsById(CityDTO obj)
        {
            try
            {
                var data = City.GetCityDetailsById(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.RegionId = int.Parse(data["RegionId"].ToString());
                        obj.CityId = int.Parse(data["CityId"].ToString());
                        obj.CityName = data["CityName"].ToString();
                        obj.isActive = bool.Parse(data["isActive"].ToString());
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
    public interface ICityService
    {
        #region City CURD Operation Interface (declartion)
        IEnumerable<CityDTO> GetCityList(CityDTO obj);
        IEnumerable<CityDTO> GetRegionList(CityDTO obj);
        CityDTO InsertUpdateCity(CityDTO obj);
        CityDTO GetCityDetailsById(CityDTO obj);
        #endregion
    }
}