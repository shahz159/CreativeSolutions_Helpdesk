using HelpDesk.API.Bussiness;
using HelpDesk.API.DTO_s;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Xml;
using System.Web.Script.Serialization;

namespace HelpDesk.API.Controllers
{
    public class AssetAPIController : ApiController
    {
        private readonly IAssetService service;
        public AssetAPIController(AssetService _service)
        {
            service = _service;
        }
        /// <summary>
        /// Insert Update of Asset by differentiate by flag id
        /// for insert flagid=1 and for update flagid=2
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewInsertUpdateAsset(AssetDTO obj)
        {
            var result = service.InsertUpdateAsset(obj);
            return Ok(result);
        }
        /// <summary>
        /// Add upadted asset data
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewUpdatedAsset(AssetDTO obj)
        {
            var result = service.UpdatedAsset(obj);
            return Ok(result);
        }

        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewVerifyAsset(AssetDTO obj)
        {
            var result = service.VerifyAsset(obj);
            return Ok(result);
        }
        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewUpdatePPMDate(AssetDTO obj)
        {
            var result = service.updateppmdate(obj);
            return Ok(result);
        }

        /// <summary>
        /// Get Asset List by CompanyId
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //public IEnumerable<AssetDTO> NewGetAssetList(AssetDTO obj)
        //{
        //    var list = service.GetAssetList(obj);
        //    return list;
        //}
        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewGetAssetList(AssetDTO obj)
        {
            var result = service.GetAssetList(obj);

            
            return Ok(result);
        }

        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewGetAssetListM(AssetDTO obj)
        {
            var result = service.GetAssetList(obj);
            string msg = "";
            bool val = true;
            JArray JProductDetails = JArray.Parse("[]");
            JArray JAccountDetails = JArray.Parse("[]");
            JArray JAssetDetails = JArray.Parse("[]");
            JArray JTotalRecordsDetails = JArray.Parse("[]");
            if (result != null)
            {
                if (!string.IsNullOrEmpty(result.datasetxml))
                {
                    var document = new XmlDocument();
                    document.LoadXml(result.datasetxml);
                    DataSet ds = new DataSet();
                    ds.ReadXml(new XmlNodeReader(document));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JProductDetails = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JProductDetails = strjarry;
                        }
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[1]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JAccountDetails = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[1]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JAccountDetails = strjarry;
                        }
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[2]);
                            var strjarry = JArray.Parse(str);


                            foreach (JObject item in strjarry)
                            {
                                var pv = item.SelectToken("PPMScheduleJson");
                                var pvj = JArray.Parse(pv.ToString());
                                item.Add(new JProperty("JPPMScheduleJson", pvj));

                                var Url = item.SelectToken("AssetModelJson");
                                var Urlj = JArray.Parse(Url.ToString());
                                item.Add(new JProperty("JAssetModelJson", Urlj));
                            }

                            if (!string.IsNullOrEmpty(str))
                                JAssetDetails = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[2]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JAssetDetails = strjarry;
                        }
                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[3]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JTotalRecordsDetails = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[3]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JTotalRecordsDetails = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("ProductList", JProductDetails),
                        new JProperty("AccountList", JAccountDetails),
                        new JProperty("AssetList", JAssetDetails)
                        , new JProperty("TotalRecords", JTotalRecordsDetails)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);

        }

        /// <summary>
        /// Get Asset Details by Id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewGetAssetDetailsById(AssetDTO obj)
        {
            var result = service.GetAssetById(obj);
            return Ok(result);
        }

        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewGetAssetDetailsByIdM(AssetDTO obj)
        {
            var result = service.GetAssetById(obj);
            string msg = "";
            bool val = true;
            JArray JAssetDetails = JArray.Parse("[]");

            var json = new JavaScriptSerializer().Serialize(obj).ToString();

            var str = JsonConvert.SerializeObject(obj);
            var objects = JObject.Parse(str);

            var pv1 = objects.SelectToken("PPMJson");
            if (pv1 is null)
                objects.Add(new JProperty("JPPMJson", "[]"));
            else
            {
                var pvj1 = JArray.Parse(pv1.ToString());
                objects.Add(new JProperty("JPPMJson", pvj1));
            }
            

            var ModelsJson = objects.SelectToken("ModelJson");
            if(ModelsJson is null)
                objects.Add(new JProperty("JModelsJson", "[]"));
            else
            {
                var ModelsJsonj = JArray.Parse(ModelsJson.ToString());
                objects.Add(new JProperty("JModelsJson", ModelsJsonj));
            }
            

            var ProductJson = objects.SelectToken("ProductJson");
            if (ProductJson is null)
                objects.Add(new JProperty("JProductJson", "[]"));
            else
            {
                var ProductJsonj = JArray.Parse(ProductJson.ToString());
                objects.Add(new JProperty("JProductJson", ProductJsonj));
            }
            

            var RegionJson = objects.SelectToken("RegionJson");
            if (RegionJson is null)
                objects.Add(new JProperty("RegionJson", "[]"));
            else
            {
                var RegionJsonj = JArray.Parse(RegionJson.ToString());
                objects.Add(new JProperty("JRegionJson", RegionJsonj));
            }
            

            var CityJson = objects.SelectToken("CityJson");
            if (CityJson is null)
                objects.Add(new JProperty("JCityJson", "[]"));
            else
            {
                var CityJsonj = JArray.Parse(CityJson.ToString());
                objects.Add(new JProperty("JCityJson", CityJsonj));
            }
            

            var UpdatedAMJson = objects.SelectToken("UpdatedJson");
            if (UpdatedAMJson is null)
                objects.Add(new JProperty("UpdatedAMJson", "[]"));
            else
            {
                var UpdatedAMJsonj = JArray.Parse(UpdatedAMJson.ToString());
                objects.Add(new JProperty("JUpdatedAMJson", UpdatedAMJsonj));
            }
            

            var AMModelsJson = objects.SelectToken("AssetModelJson");
            if (AMModelsJson is null)
                objects.Add(new JProperty("AMModelsJson", "[]"));
            else
            {
                var AMModelsJsonj = JArray.Parse(AMModelsJson.ToString());
                objects.Add(new JProperty("JAssetModelJson", AMModelsJsonj));
            }

            

            var RemainingModelsJson = objects.SelectToken("RemainingModelsJson");
            if (RemainingModelsJson is null)
                objects.Add(new JProperty("RemainingModelsJson", "[]"));
            else
            {
                var RemainingModelsJsonj = JArray.Parse(RemainingModelsJson.ToString());
                objects.Add(new JProperty("JRemainingModelsJson", RemainingModelsJsonj));
            }
            

            JObject res1 = new JObject(new JProperty("AssetDetails", objects) 
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }

        /// <summary>
        /// Get List of Active and Non Active records of City by region Id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<AssetDTO> NewGetCityList(AssetDTO obj)
        {
            var list = service.GetCity(obj);
            return list;
        }
        /// <summary>
        /// Get List of Active and Non Active records of Model by Product Id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<AssetDTO> NewGetModelList(AssetDTO obj)
        {
            var list = service.GetModel(obj);
            return list;
        }

        /// <summary>
        /// Get lists of Products,Accounts by company id
        /// Get lists of Region by organization id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewGetDropDowns(AssetDTO obj)
        {
            var detail = service.GetDropDowns(obj);
            return Ok(detail);
        }
        /// <summary>
        /// Get Approval Assets by organiztionid
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewApprovalAssets(AssetDTO obj)
        {
            var detail = service.GetApprovalAssets(obj);
            return Ok(detail);
        }
        /// <summary>
        /// PPM Date change list
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewPPMDateChangeRequest(AssetDTO obj)
        {
            var detail = service.GetPPMChnageRequest(obj);
            return Ok(detail);
        }
        /// <summary>
        /// Update IsApproved or IsRejected status of Asset
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewUpdateAssetStatus(AssetDTO obj)
        {
            var result = service.UpdateAssetStatus(obj);
            return Ok(result);
        }
        /// <summary>
        /// PPM date change record approval or reject
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewUpdatePPMDateChange(AssetDTO obj)
        {
            var result = service.UpdatePPMDateChangeRequest(obj);
            return Ok(result);
        }

        /// <summary>
        /// Get All Expired Assets
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewRenewalAssetsList(AssetDTO obj)
        {
            var detail = service.GetAssetsRenewalList(obj);
            return Ok(detail);
        }
        /// <summary>
        /// Get renewal request list of asset
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewRenewalRequestAssetsList(AssetDTO obj)
        {
            var detail = service.GetAssetsRenewalRequestList(obj);
            return Ok(detail);
        }
        /// <summary>
        /// insert renewal request of expired asset
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewInsertRenewalRequestAssets(AssetDTO obj)
        {
            var detail = service.InsertAssetRenewalRequest(obj);
            return Ok(detail);
        }
        /// <summary>
        /// update status of renewal asset request
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewUpdateAssetRenewalRequest(AssetDTO obj)
        {
            var detail = service.UpdateAssetRenewalRequest(obj);
            return Ok(detail);
        }
        /// <summary>
        /// get asset reneawl asset details by AMId(Asset Management Id)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewRenewalAssetsDetails(AssetDTO obj)
        {
            var detail = service.GetAssetsRenewalDetails(obj);
            return Ok(detail);
        }

        /// <summary>
        /// Add more models of each asset 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewAssetModelsInsert(AssetDTO obj)
        {
            var detail = service.InsertAssetModels(obj);
            return Ok(detail);
        }
    }
}
