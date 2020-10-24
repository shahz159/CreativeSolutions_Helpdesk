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
    public class CityModel: ICityModel
    {
       public SqlDataReader GetCityList(CityDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@organizationId", obj.OrganizationId)
            };
                return DbConnector.ExecuteReader("UspGetCityList", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "Model -> GetCityList");
                return null;
            }
        }
        public SqlDataReader InsertUpdateCity(CityDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@CityName",obj.CityName),
                    new SqlParameter("@isActive",obj.isActive),
                    new SqlParameter("@CityId",obj.CityId),
                    new SqlParameter("@FlagId",obj.FlagId),
                    new SqlParameter("@CreatedBy",obj.CreatedBy),
                    new SqlParameter("@RegionId",obj.RegionId)

                };
                return DbConnector.ExecuteReader("uspInsertUpdateCity", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "ProductModel -> InsertUpdateCity");
                return null;
            }

        }
        public SqlDataReader GetCityDetailsById(CityDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@CityId",obj.CityId)
                };
                return DbConnector.ExecuteReader("uspGetCityDetailsById", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "Model -> GetCityDetailsById");
                return null;
            }
        }

        public SqlDataReader GetRegionList(CityDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@organizationId", obj.OrganizationId)
            };
                return DbConnector.ExecuteReader("UspGetRegionDropDownList", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "Model -> GetRegionList");
                return null;
            }
        }
    }
    public interface ICityModel
    {
        SqlDataReader GetCityList(CityDTO obj);
        SqlDataReader InsertUpdateCity(CityDTO obj);
        SqlDataReader GetCityDetailsById(CityDTO obj);
        SqlDataReader GetRegionList(CityDTO obj);
    }
}