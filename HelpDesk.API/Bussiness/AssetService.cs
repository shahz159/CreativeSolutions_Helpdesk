using HelpDesk.API.DataAccess;
using HelpDesk.API.DTO_s;
using HelpDesk.API.GenericHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API.Bussiness
{
    public class AssetService: IAssetService
    {
        private readonly IAssetModel model;
        public AssetService(AssetModel _model)
        {
            model = _model;
        }

        #region CURD Operations Services of Asset Management
        public AssetDTO GetAssetById(AssetDTO obj)
        {
            try
            {
                var data = model.GetAssetDetailsById(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.AccountId = int.Parse(data["AccountId"].ToString());
                        obj.AccountName = data["AccountName"].ToString();
                        obj.AccountCode = data["AccountCode"].ToString();

                        obj.ProductId = int.Parse(data["ProductId"].ToString());
                        obj.ProductName = data["ProductName"].ToString();
                        obj.ProductCode = data["ProductCode"].ToString();

                        obj.ModelId = int.Parse(data["ModelId"].ToString());
                        obj.ModelName = data["ModelName"].ToString();

                        obj.StationName = data["StationName"].ToString();
                        obj.IPAddress = data["IPAddress"].ToString();
                        obj.SerialNo = data["SerialNo"].ToString();
                        obj.Configuration = data["Configuration"].ToString();
                        obj.Area = data["Area"].ToString();
                        obj.RegionName = data["RegionName"].ToString();
                        obj.RegionId = int.Parse(data["RegionId"].ToString());
                        obj.CityId = int.Parse(data["CityId"].ToString());
                        obj.CityName = data["CityName"].ToString();
                        obj.InstallationDate =DateTime.Parse(data["InstallationDate"].ToString());
                        obj.IsContract =bool.Parse(data["IsContract"].ToString());
                        obj.POContract = data["POContract"].ToString();

                        obj.WarrantyExpiryDate = DateTime.Parse(data["WarrantyExpiryDate"].ToString());
                        obj.PPMType =int.Parse(data["PPMType"].ToString());
                        obj.SystemNo = data["SystemNo"].ToString();
                        obj.IsApproved =bool.Parse(data["IsApproved"].ToString());
                        obj.IsRejected = bool.Parse(data["IsRejected"].ToString());
                        obj.ApprovedBy =long.Parse(data["ApprovedBy"].ToString());
                        obj.isActive = bool.Parse(data["isActive"].ToString());

                        obj.CreatedOn =DateTime.Parse(data["CreatedOn"].ToString());
                        obj.CreatedBy =long.Parse(data["CreatedBy"].ToString());
                        obj.FullName = data["FullName"].ToString();
                        obj.AMId = int.Parse(data["AMId"].ToString());
                        obj.PPMJson = data["PPMJson"].ToString();
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
        public IEnumerable<AssetDTO> GetAssetList(AssetDTO obj)
        {
            var data = model.GetAssetList(obj);
            var list = CustomDataReaderToGenericExtension.GetDataObjects<AssetDTO>(data);
            data.Close();
            return list;
        }
        public IEnumerable<AssetDTO> GetCity(AssetDTO obj)
        {
            var data = model.GetCity(obj);
            var list = CustomDataReaderToGenericExtension.GetDataObjects<AssetDTO>(data);
            data.Close();
            return list;
        }
        public AssetDTO GetDropDowns(AssetDTO obj)
        {
            obj.datasetxml = model.GetDropDownList(obj);
            return obj;
        }
        public AssetDTO GetApprovalAssets(AssetDTO obj)
        {
            obj.datasetxml = model.GetApprovalAssets(obj);
            return obj;
        }

        
        public IEnumerable<AssetDTO> GetModel(AssetDTO obj)
        {
            var data = model.GetModels(obj);
            var list = CustomDataReaderToGenericExtension.GetDataObjects<AssetDTO>(data);
            data.Close();
            return list;
        }
        public AssetDTO InsertUpdateAsset(AssetDTO obj)
        {
            try
            {
                var data = model.InsertUpdateAsset(obj);
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
        public AssetDTO UpdateAssetStatus(AssetDTO obj)
        {
            try
            {
                var data = model.UpdateAssetStatus(obj);
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
        #endregion
    }

    public interface IAssetService
    {
        AssetDTO InsertUpdateAsset(AssetDTO obj);
        IEnumerable<AssetDTO> GetAssetList(AssetDTO obj);
        IEnumerable<AssetDTO> GetCity(AssetDTO obj);
        IEnumerable<AssetDTO> GetModel(AssetDTO obj);
        AssetDTO GetAssetById(AssetDTO obj);
        AssetDTO GetDropDowns(AssetDTO obj);
        AssetDTO GetApprovalAssets(AssetDTO obj);
        AssetDTO UpdateAssetStatus(AssetDTO obj);
    }
}