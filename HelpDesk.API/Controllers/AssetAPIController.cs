﻿using HelpDesk.API.Bussiness;
using HelpDesk.API.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

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
        /// Get Asset List by CompanyId
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<AssetDTO> NewGetAssetList(AssetDTO obj)
        {
            var list = service.GetAssetList(obj);
            return list;
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
    }
}
