﻿using HelpDesk.API.DatabaseConnector;
using HelpDesk.API.DbHelpers;
using HelpDesk.API.DTO_s;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HelpDesk.API.DataAccess
{
    public class AssetModel : IAssetModel
    {
        public SqlDataReader GetAssetDetailsById(AssetDTO obj)
        {
            try
            {
                var para = new[] 
                {
                    new SqlParameter("@AMId",obj.AMId)
                };
                return DbConnector.ExecuteReader("uspGetAssetManagementDetails", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "AssetModel -> GetAssetDetailsById");
                return null;
            }
        }

        public SqlDataReader GetAssetList(AssetDTO obj)
        {
            try
            {
                var para = new[]
                {
                    //new SqlParameter("@CompanyId",obj.CompanyId),
                    new SqlParameter("@UserId",obj.CreatedBy)
                };
                return DbConnector.ExecuteReader("uspGetAssetList", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "AssetModel -> GetAssetList");
                return null;
            }
        }

        public SqlDataReader GetCity(AssetDTO obj)
        {
            try
            {
                var para = new[]
                {
                    new SqlParameter("@RegionId",obj.RegionId)
                };
                return DbConnector.ExecuteReader("uspGetCity", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "AssetModel -> GetCity");
                return null;
            }
        }

        public string GetDropDownList(AssetDTO obj)
        {
            try
            {
                var para = new[]
                {
                    new SqlParameter("@CompanyId",obj.CompanyId),
                    new SqlParameter("@OrganizationId",obj.OrganizationId),
                    new SqlParameter("@UserId",obj.CreatedBy),
                    new SqlParameter("@RoleId",obj.RoleId)
                };
                return DbConnector.ExecuteDataSet("UspGetDropDownsAssetManagement", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "AssetModel -> GetDropDownList");
                return null;
            }
        }
        public string GetApprovalAssets(AssetDTO obj)
        {
            try
            {
                var para = new[]
                {
                    new SqlParameter("@OrganizationId",obj.OrganizationId)
                };
                return DbConnector.ExecuteDataSet("uspGetApprovalAssets", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "AssetModel -> GetApprovalAssets");
                return null;
            }
        }
        
        public SqlDataReader GetModels(AssetDTO obj)
        {
            try
            {
                var para = new[]
                {
                    new SqlParameter("@ProductId",obj.ProductId) 
                };
                return DbConnector.ExecuteReader("UspGetModels", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "AssetModel -> GetModels");
                return null;
            }
        }

        public SqlDataReader InsertUpdateAsset(AssetDTO obj)
        {
            try
            {
                var para = new[]
                {
                    new SqlParameter("@AccountId",obj.AccountId),
                    new SqlParameter("@ProductId",obj.ProductId),
                    new SqlParameter("@ModelId",obj.ModelId),
                    new SqlParameter("@StationName",obj.StationName),
                    new SqlParameter("@IPAddress",obj.IPAddress),
                    new SqlParameter("@SerialNo",obj.SerialNo),
                    new SqlParameter("@Configuration",obj.Configuration),
                    new SqlParameter("@Area",obj.Area),
                    new SqlParameter("@RegionId",obj.RegionId),
                    new SqlParameter("@CityId",obj.CityId),
                    new SqlParameter("@InstallationDate",obj.InstallationDate),
                    new SqlParameter("@IsContract",obj.IsContract),
                    new SqlParameter("@POContract",obj.POContract),
                    new SqlParameter("@WarrantyExpiryDate",obj.WarrantyExpiryDate),
                    new SqlParameter("@PPMType",obj.PPMType),
                    new SqlParameter("@SystemNo",obj.SystemNo),
                    new SqlParameter("@isActive",obj.isActive),
                    new SqlParameter("@CreatedBy",obj.CreatedBy),
                    new SqlParameter("@FlagId",obj.FlagId),
                    new SqlParameter("@AMId",obj.AMId),
                    new SqlParameter("@CompanyId",obj.CompanyId) 
                };
                return DbConnector.ExecuteReader("UspInsertUpdateAsset", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "AssetModel -> GetModels");
                return null;
            }
        }
        public SqlDataReader UpdatedAsset(AssetDTO obj)
        {
            try
            {
                var para = new[]
                {
                   
                    new SqlParameter("@ProductId",obj.ProductId),
                    new SqlParameter("@ModelId",obj.ModelId),
                    new SqlParameter("@StationName",obj.StationName),
                    new SqlParameter("@IPAddress",obj.IPAddress),
                    new SqlParameter("@Configuration",obj.Configuration),
                    new SqlParameter("@Area",obj.Area),
                    new SqlParameter("@RegionId",obj.RegionId),
                    new SqlParameter("@CityId",obj.CityId),
                    new SqlParameter("@isActive",obj.isActive),
                    new SqlParameter("@CreatedBy",obj.CreatedBy),
                    new SqlParameter("@AMId",obj.AMId) ,
                    new SqlParameter("@SerialNo",obj.SerialNo)
                };
                return DbConnector.ExecuteReader("uspAddUpdatingRecord", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "AssetModel -> UpdatedAsset");
                return null;
            }
        }
        public SqlDataReader VerifyAsset(AssetDTO obj)
        {
            try
            {
                var para = new[]
                {

                    new SqlParameter("@UpdateAMId",obj.UpdatedAMId),
                    new SqlParameter("@Status",obj.StatusId),
                    new SqlParameter("@CreatedBy",obj.CreatedBy),
                    new SqlParameter("@AMId",obj.AMId) 
                };
                return DbConnector.ExecuteReader("uspVerifyUpdatedAssets", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "AssetModel -> VerifyAsset");
                return null;
            }
        }
        public SqlDataReader UpdatePPMDate(AssetDTO obj)
        {
            try
            {
                var para = new[]
                {

                    new SqlParameter("@APPMId",obj.APPMId),
                    new SqlParameter("@date",obj.InstallationDate) ,
                    new SqlParameter("@UserId",obj.CreatedBy)
                };
                return DbConnector.ExecuteReader("uspUpdateScheduleDate", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "AssetModel -> VerifyAsset");
                return null;
            }
        }
        

        public SqlDataReader UpdateAssetStatus(AssetDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@isApproved",obj.IsApproved),
                new SqlParameter("@isRejected",obj.IsRejected),
                new SqlParameter("@CreatedBy",obj.CreatedBy),
                new SqlParameter("@AMId",obj.AMId)
                };
                return DbConnector.ExecuteReader("uspUpdateAssetStatus", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "AssetModel -> UpdateAssetStatus");
                return null;
            }
        }
        public SqlDataReader UpdatePPMChangeRequest(AssetDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@Status",obj.StatusId),
                new SqlParameter("@UserId",obj.CreatedBy),
                new SqlParameter("@UpdatedId",obj.UpdatedId)
                };
                return DbConnector.ExecuteReader("uspUpdatePPMDateRequest", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "AssetModel -> UpdatePPMChangeRequest");
                return null;
            }
        }
        
        public string GetPPMChangeDateRequestList(AssetDTO obj)
        {
            try
            {
                var para = new[]
                {
                    new SqlParameter("@OrganizationId",obj.OrganizationId)
                };
                return DbConnector.ExecuteDataSet("uspGetPPMDateChangeRequestList", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "AssetModel -> GetPPMChangeDateRequestList");
                return null;
            }
        }
    }

    public interface IAssetModel
    {
        string GetPPMChangeDateRequestList(AssetDTO obj);
        string GetDropDownList(AssetDTO obj);
        string GetApprovalAssets(AssetDTO obj);
        SqlDataReader GetCity(AssetDTO obj);
        SqlDataReader GetModels(AssetDTO obj);

        SqlDataReader InsertUpdateAsset(AssetDTO obj);
        SqlDataReader UpdatedAsset(AssetDTO obj);
        SqlDataReader VerifyAsset(AssetDTO obj);
        SqlDataReader UpdatePPMDate(AssetDTO obj);
        SqlDataReader GetAssetList(AssetDTO obj);
        SqlDataReader UpdateAssetStatus(AssetDTO obj);
        SqlDataReader UpdatePPMChangeRequest(AssetDTO obj);
        SqlDataReader GetAssetDetailsById(AssetDTO obj);

    }
}