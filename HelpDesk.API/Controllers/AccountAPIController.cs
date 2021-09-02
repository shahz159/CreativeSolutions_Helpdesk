using HelpDesk.API.Bussiness;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HelpDesk.API.Controllers
{
    public class AccountAPIController : ApiController
    {
        private readonly IAccountService service;
        public AccountAPIController(AccountService _service)
        {
            service = _service;
        }
        /// <summary>
        /// Get List of Account  with active and inactive
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<AccountDTO> NewGetAccountsList(AccountDTO obj)
        {
            var list = service.GetAccountList(obj);
            return list;
        }
        /// <summary>
        /// Change Status of Account by Account id, Not deleting permanently
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IHttpActionResult NewDeleteAccount(AccountDTO obj)
        {
            var result = service.DeleteAccount(obj);
            return Ok(result);
        }
        /// <summary>
        /// Insert a Account record if FlagId = 1 
        /// Update a Account record if FlagId = 2
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IHttpActionResult NewInsertUpdateAccounts(AccountDTO obj)
        {
            var result = service.InsertUpdateAccount(obj);
            return Ok(result);
        }
        public IHttpActionResult NewInsertUpdateAccountsM(AccountDTO obj)
        {
            var result = service.InsertUpdateAccount(obj);
            string msg = "";
            bool val = true;

            if (result.message == "3")
                val = false;

            if (obj.FlagId == 2)
                msg = "Update Successfully.";
            else if (obj.FlagId == 1)
                msg = "Added Successfully.";
            else if (obj.FlagId == 3)
                msg = "Already ASsigned.";
            msg = val == true ? "" : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
        }
        /// <summary>
        /// Get Account Details by Id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IHttpActionResult NewGetAccountById(AccountDTO obj)
        {
            var result = service.GetAccountDetailsById(obj);
            return Ok(result);
        }
    }
}
