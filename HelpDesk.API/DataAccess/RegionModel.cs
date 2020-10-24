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
    public class RegionModel: IRegionModel
    {
        public SqlDataReader GetRegionList(RegionDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@organizationId", obj.OrganizationId)
            };
                return DbConnector.ExecuteReader("UspGetRegionList", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "RegionModel -> GetRegionList");
                return null;
            }
        }
        public SqlDataReader InsertUpdateRegion(RegionDTO obj)
        {
            try
            {
                var para = new[] {
                    
                    new SqlParameter("@RegionName",obj.RegionName),
                    new SqlParameter("@isActive",obj.isActive),
                    new SqlParameter("@RegionId",obj.RegionId),
                    new SqlParameter("@FlagId",obj.FlagId),
                    new SqlParameter("@CreatedBy",obj.CreatedBy) ,
                    new SqlParameter("@organizationId",obj.OrganizationId)

                };
                return DbConnector.ExecuteReader("uspInsertUpdateRegion", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "RegionModel -> InsertUpdateRegion");
                return null;
            }

        }

        public SqlDataReader GetRegionDetailsById(RegionDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@RegionId",obj.RegionId)
                };
                return DbConnector.ExecuteReader("uspGetRegionDetailsById", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "RegionModel -> GetRegionDetailsById");
                return null;
            }
        }
    }
    public interface IRegionModel
    {
        SqlDataReader GetRegionList(RegionDTO obj);
        SqlDataReader InsertUpdateRegion(RegionDTO obj);
        SqlDataReader GetRegionDetailsById(RegionDTO obj);
    }
}