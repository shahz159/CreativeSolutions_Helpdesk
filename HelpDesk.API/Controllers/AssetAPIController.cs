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
        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewCheckSerialNumber(AssetDTO obj)
        {
            var result = service.CheckSerialNumber(obj);
            return Ok(result);
        }

        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewCheckSerialNumberM(AssetDTO obj)
        {
            var result = service.CheckSerialNumber(obj);
            string msg = "";
            bool val = false;

            if (result.message == "2")
                val = true;
            msg = val == true ? "Not Exists." : "Exists";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
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

        

        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewInsertUpdateAssetM(AssetDTO obj)
        {
            var result = service.InsertUpdateAsset(obj);
            string msg = "";
            bool val = false;

            if (result.message != "0")
                val = true;
            msg = val == true ? "Added Successfully." : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
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
        public IHttpActionResult NewUpdatedAssetM(AssetDTO obj)
        {
            var result = service.UpdatedAsset(obj);
            string msg = "";
            bool val = false;

            if (result.message != "0")
                val = true;

            if (obj.FlagId == 2)
                msg = "Asset Update Request Sent Successfully.";
           
            msg = val == true ? "" : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
        }

        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewVerifyAsset(AssetDTO obj)
        {
            var result = service.VerifyAsset(obj);
            return Ok(result);
        }

        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewVerifyAssetM(AssetDTO obj)
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

            var JVMOrderJson = objects.SelectToken("JVMOrdersJson");
            if (JVMOrderJson is null)
                objects.Add(new JProperty("JVMOrdersJson", "[]"));
            else
            {
                var JVMOrderJsonj = JArray.Parse(JVMOrderJson.ToString());
                objects.Add(new JProperty("JJVMOrdersJson", JVMOrderJsonj));
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

        public IEnumerable<AssetDTO> NewGetCityListM(AssetDTO obj)
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
            //string msg = "";
            //bool val = true;
            //JArray JProductList = JArray.Parse("[]");
            //JArray JAccoountList = JArray.Parse("[]");
            //JArray JRegionList = JArray.Parse("[]");

            //if (result != null)
            //{
            //    if (!string.IsNullOrEmpty(result.datasetxml))
            //    {
            //        var document = new XmlDocument();
            //        document.LoadXml(result.datasetxml);
            //        DataSet ds = new DataSet();
            //        ds.ReadXml(new XmlNodeReader(document));
            //        if (ds.Tables.Count > 0)
            //        {
            //            if (ds.Tables[0].Rows.Count > 0)
            //            {
            //                var str = JsonConvert.SerializeObject(ds.Tables[0]);
            //                var strjarry = JArray.Parse(str);

            //                if (!string.IsNullOrEmpty(str))
            //                    JProductList = strjarry;
            //            }
            //            else
            //            {
            //                var str = JsonConvert.SerializeObject(ds.Tables[0]);
            //                var strjarry = JArray.Parse(str);

            //                if (!string.IsNullOrEmpty(str))
            //                    JProductList = strjarry;
            //            }
            //            if (ds.Tables[1].Rows.Count > 0)
            //            {
            //                var str = JsonConvert.SerializeObject(ds.Tables[1]);
            //                var strjarry = JArray.Parse(str);

            //                if (!string.IsNullOrEmpty(str))
            //                    JAccoountList = strjarry;
            //            }
            //            else
            //            {
            //                var str = JsonConvert.SerializeObject(ds.Tables[1]);
            //                var strjarry = JArray.Parse(str);

            //                if (!string.IsNullOrEmpty(str))
            //                    JAccoountList = strjarry;
            //            }

            //            if (ds.Tables[2].Rows.Count > 0)
            //            {
            //                var str = JsonConvert.SerializeObject(ds.Tables[2]);
            //                var strjarry = JArray.Parse(str);

            //                if (!string.IsNullOrEmpty(str))
            //                    JRegionList = strjarry;
            //            }
            //            else
            //            {
            //                var str = JsonConvert.SerializeObject(ds.Tables[2]);
            //                var strjarry = JArray.Parse(str);

            //                if (!string.IsNullOrEmpty(str))
            //                    JRegionList = strjarry;
            //            }
            //        }
            //    }
            //}
            //JObject res1 = new JObject(new JProperty("ProductList", JProductList), new JProperty("AccountList", JAccoountList), new JProperty("RegionList", JRegionList)
            //            );
            //msg = val == true ? "Success." : "Failure";
            //JObject res = new JObject(new JProperty("Status", true),
            //                   (new JProperty("Message", msg)),
            //                   (new JProperty("Data", res1)));
            //return Ok(res);
            return list;
        }

        public IEnumerable<AssetDTO> NewGetPOContractList(AssetDTO obj)
        {
            var list = service.GetPOContractList(obj);
            return list;
        }

        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewGetPOContractListM(AssetDTO obj)
        {
            var list = service.GetPOContractList(obj);
           

            string msg = "";
            bool val = true;
            JArray JProductList = JArray.Parse("[]");

            var str = JsonConvert.SerializeObject(list);
            var strjarry = JArray.Parse(str);

            if (!string.IsNullOrEmpty(str))
                JProductList = strjarry;

            JObject res1 = new JObject(new JProperty("ProductList", JProductList)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);

            //return list;
        }

        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewGetModelListM(AssetDTO obj)
        {
            var result = service.GetModelM(obj);
            string msg = "";
            bool val = true;
            JArray JModelList = JArray.Parse("[]");
              
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
                                JModelList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JModelList = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("ModelList", JModelList)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
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

        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewGetAssetDropDownsM(AssetDTO obj)
        {
            var result = service.GetDropDowns(obj);
            string msg = "";
            bool val = true;
            JArray JProductList = JArray.Parse("[]");
            JArray JAccoountList = JArray.Parse("[]");
            JArray JRegionList = JArray.Parse("[]");

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
                                JProductList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JProductList = strjarry;
                        }
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[1]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JAccoountList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[1]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JAccoountList = strjarry;
                        }

                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[2]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JRegionList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[2]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JRegionList = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("ProductList", JProductList), new JProperty("AccountList", JAccoountList), new JProperty("RegionList", JRegionList)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
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
        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewApprovalAssetsM(AssetDTO obj)
        {
            var result = service.GetApprovalAssets(obj);
            string msg = "";
            bool val = true;
            JArray JUnderApprovalAssets = JArray.Parse("[]");
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
                                JUnderApprovalAssets = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JUnderApprovalAssets = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("AssetApprovalList", JUnderApprovalAssets)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
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

        [ResponseType(typeof(AssetDTO))]

        public IHttpActionResult NewPPMDateChangeRequestM(AssetDTO obj)
        {
            var result = service.GetPPMChnageRequest(obj);
            string msg = "";
            bool val = true;
            JArray JUnderApprovalAssets = JArray.Parse("[]");
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
                                JUnderApprovalAssets = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JUnderApprovalAssets = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("PPMDateChangeRequestList", JUnderApprovalAssets)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }

        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewGetAssetListByPOContract(AssetDTO obj)
        {
            var result = service.GetASsetListByPOContract(obj);
            return Ok(result);
        }

        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewGetAssetListByPOContractM(AssetDTO obj)
        {
            var result = service.GetASsetListByPOContract(obj);
            string msg = "";
            bool val = true;
            JArray JUnderApprovalAssets = JArray.Parse("[]");
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
                                JUnderApprovalAssets = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JUnderApprovalAssets = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("AssetListByPOContract", JUnderApprovalAssets)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
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


        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewUpdateAssetStatusM(AssetDTO obj)
        {
            var result = service.UpdateAssetStatus(obj);
            string msg = "";
            bool val = false;
            if (result.message != "0")
                val = true;
            msg = val == true ? "Updated Successfully." : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
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

        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewUpdatePPMDateChangeM(AssetDTO obj)
        {
            var result = service.UpdatePPMDateChangeRequest(obj);
            string msg = "";
            bool val = false;
            if (result.message != "0")
                val = true;
            msg = val == true ? "Updated Successfully." : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
        }
        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewAssetModelRemove(AssetDTO obj)
        {
            var result = service.removeAssetModel(obj);
            return Ok(result);
        }
        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewJVMOrders(AssetDTO obj)
        {
            var result = service.AddJVMOrders(obj);
            return Ok(result);
        }

      

        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewJVMOrdersM(AssetDTO obj)
        {
            var result = service.AddJVMOrders(obj);
            string msg = "";
            bool val = false;

            if (result.message != "0")
                val = true;
            msg = val == true ? "Added Successfully." : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
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

        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewRenewalAssetsListM(AssetDTO obj)
        {
            var result = service.GetAssetsRenewalList(obj);
            string msg = "";
            bool val = true;
            JArray JExpiredAssetList = JArray.Parse("[]");
            JArray JNoContracAssettList = JArray.Parse("[]");
            JArray JActiveContracAssettList = JArray.Parse("[]");

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
                                JExpiredAssetList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JExpiredAssetList = strjarry;
                        }

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[1]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JNoContracAssettList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[1]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JNoContracAssettList = strjarry;
                        }
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[2]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JActiveContracAssettList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[2]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JActiveContracAssettList = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("ExpiredAssetList", JExpiredAssetList),
                        new JProperty("NoContractAssetList", JNoContracAssettList),
                        new JProperty("ActiveContractAssetList", JActiveContracAssettList)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
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


        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewRenewalRequestAssetsListM(AssetDTO obj)
        {
            var result = service.GetAssetsRenewalRequestList(obj);
            string msg = "";
            bool val = true;
            JArray JUnderApprovalAssets = JArray.Parse("[]");
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
                                JUnderApprovalAssets = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JUnderApprovalAssets = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("AssetRenewalList", JUnderApprovalAssets)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
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

        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewInsertRenewalRequestAssetsM(AssetDTO obj)
        {
            var detail = service.InsertAssetRenewalRequest(obj);
            string msg = "";
            bool val = true;

            msg = val == true ? "Renewal Request Sent Successfully." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)));
            return Ok(res);
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

        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewUpdateAssetRenewalRequestM(AssetDTO obj)
        {
            var result = service.UpdateAssetRenewalRequest(obj);
            string msg = "";
            bool val = false;

            if (result.message != "0")
            {
                val = true;
            }
            //JObject res1 = new JObject(new JProperty("SystemManagerId", result.SystemManagerId.ToString())
            //            );
            msg = val == true ? "Updated Successfully." : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
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

        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewRenewalAssetsDetailsM(AssetDTO obj)
        {
            var result = service.GetAssetsRenewalDetails(obj);
            string msg = "";
            bool val = true;
            JArray JUnderApprovalAssets = JArray.Parse("[]");
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
                                JUnderApprovalAssets = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JUnderApprovalAssets = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("AssetRenewalDetails", JUnderApprovalAssets)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
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

        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewAssetModelsInsertM(AssetDTO obj)
        {
            var detail = service.InsertAssetModels(obj);
            string msg = "";
            bool val = false;

            if (detail.message != "0")
                val = true;
            msg = val == true ? "Added Successfully." : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
        }


        /// <summary>
        /// PPM Email Notofication service
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(AssetDTO))]
        public IHttpActionResult NewSendEmailNotificationOfPPM()
        {
            var result = service.ppmemailnotification();
            return Ok(result);
        }
    }
}
