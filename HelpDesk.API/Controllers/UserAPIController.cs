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

namespace HelpDesk.API.Controllers
{
    public class UserAPIController : ApiController
    {
        private readonly IUserService service;
        public UserAPIController(UserService _service)
        {
            service = _service;
        }

        /// <summary>
        /// New User Creation
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult NewInsertUser(UsersDTO obj)
        {
            var result = service.NewUser(obj);
            return Ok(result);
        }

        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult NewUpdateUserInfo(UsersDTO obj)
        {
            var result = service.UpdateUserInfoBasic(obj);
            return Ok(result);
        }

        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult NewUpdateUserInfoM(UsersDTO obj)
        {
            var result = service.UpdateUserInfoBasic(obj);
            string msg = "";
            bool val = false;
            if (result.message != "0")
                val = true;
            msg = val == true ? "User Update Successfully." : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
        }

        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult NewUpdateSignUpUserStatus(UsersDTO obj)
        {
            var result = service.UpdateSignUpUserStatus(obj);
            return Ok(result);
        }

        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult NewUpdateSignUpUserStatusM(UsersDTO obj)
        {
            var result = service.UpdateSignUpUserStatus(obj);
            string msg = "";
            bool val = false;
            if (result.message != "0")
                val = true;
            msg = val == true ? "User Status Update Successfully." : "Failure";
            JObject res = new JObject(new JProperty("Status", val),
                                        (new JProperty("Message", msg))
                         );
            return Ok(res);
        }

        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult NewUpdateUserStatusActive(UsersDTO obj)
        {
            var result = service.UpdateUserStatusActive(obj);
            return Ok(result);
        }

        /// <summary>
        /// update password
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult NewUpdateUserPassword(UsersDTO obj)
        {
            var result = service.UpdateUserPassword(obj);
            return Ok(result);
        }

        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult NewUpdateUserPasswordwithEmail(UsersDTO obj)
        {
            var result = service.UpdateUserPasswordWithEmail(obj);
            return Ok(result);
        }

        /// <summary>
        /// Get User Details by id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult NewGetUserDetailsById(UsersDTO obj)
        {
            var result = service.GetUserDetailsById(obj);
            return Ok(result);
        }

        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult NewGetUserDetailsByIdM(UsersDTO obj)
        {
            var result = service.GetUserDetailsById(obj);
            string msg = "";
            bool val = true;
            JArray JSparePartList = JArray.Parse("[]");
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
                            foreach (JObject item in strjarry)
                            {
                                var pv = item.SelectToken("AccountsJson");
                                var pvj = JArray.Parse(pv.ToString());
                                item.Add(new JProperty("JAccountsJson", pvj));

                                var Url = item.SelectToken("ProductsJson");
                                var Urlj = JArray.Parse(Url.ToString());
                                item.Add(new JProperty("JProductsJson", Urlj));
                            }
                            if (!string.IsNullOrEmpty(str))
                                JSparePartList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JSparePartList = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("UserDetails", JSparePartList)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }

        /// <summary>
        /// Get User List
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult NewGetUserList(UsersDTO obj)
        {
            var result = service.GetUserList(obj);
            return Ok(result);
        }

        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult NewGetUserListM(UsersDTO obj)
        {
            var result = service.GetUserList(obj);
            string msg = "";
            bool val = true;
            JArray JUserList = JArray.Parse("[]");
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
                                JUserList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JUserList = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("UserList", JUserList)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }

        /// <summary>
        /// Get Role and Comany dropdowns
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult NewGetDropDowns(UsersDTO obj)
        {
            var result = service.RoleCompanyDropDowns(obj);
            return Ok(result);
        }

        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult NewGetDropDownsForUserRegistrationM(UsersDTO obj)
        {
            var result = service.RoleCompanyDropDowns(obj);
            string msg = "";
            bool val = true;
            JArray JRoleList = JArray.Parse("[]");
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
                                JRoleList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JRoleList = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("RoleList", JRoleList)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }

        /// <summary>
        /// Get Accounts by company id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<UsersDTO> NewGetAccountList(UsersDTO obj)
        {
            var list = service.GetCompanyAccounts(obj);
            return list;
        }

        public IEnumerable<UsersDTO> NewGetManagerAccountList(UsersDTO obj)
        {
            var list = service.GetCompanyManagerAccounts(obj);
            return list;
        }

        /// <summary>
        /// Get Product List by Company id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<UsersDTO> NewGetProductList(UsersDTO obj)
        {
            var list = service.GetCompanyProducts(obj);
            return list;
        }

        public IEnumerable<UsersDTO> NewGetProductListRolwWise(UsersDTO obj)
        {
            var list = service.GetCompanyProductsRoleWise(obj);
            return list;
        }

        /// <summary>
        /// Check Email Exists
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// 
        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult GetCheckEmailExists(string email)
        {
            var detail = service.CheckEmailExists(email);
            return Ok(detail);
        }
        /// <summary>
        /// Change of password request
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult GetChangePasswordRequest(string email)
        {
            var detail = service.ChangePasswordRequest(email);
            return Ok(detail);
        }


        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult GetVerifyPasswordChangeRequest(string email,string token)
        {
            var detail = service.verifyPasswordRequest(email,token);
            return Ok(detail);
        }

        /// <summary>
        /// Check Emp Id exists
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult GetCheckEmpidExists(string empid)
        {
            var result = service.CheckEmpIdExists(empid);
            return Ok(result);
        }

        /// <summary>
        /// Get SystemUser which are registered through SINGUP form for approval
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult NewGetSystemUsersforApproval(UsersDTO obj)
        {
            var result = service.GetSystemUserforApprovalList(obj);
            return Ok(result);
        }

        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult NewGetSystemUsersforApprovalM(UsersDTO obj)
        {
            var result = service.GetSystemUserforApprovalList(obj);
            string msg = "";
            bool val = true;
            JArray JSignUpUserList = JArray.Parse("[]");
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
                                JSignUpUserList = strjarry;
                        }
                        else
                        {
                            var str = JsonConvert.SerializeObject(ds.Tables[0]);
                            var strjarry = JArray.Parse(str);

                            if (!string.IsNullOrEmpty(str))
                                JSignUpUserList = strjarry;
                        }
                    }
                }
            }
            JObject res1 = new JObject(new JProperty("SignUpUserList", JSignUpUserList)
                        );
            msg = val == true ? "Success." : "Failure";
            JObject res = new JObject(new JProperty("Status", true),
                               (new JProperty("Message", msg)),
                               (new JProperty("Data", res1)));
            return Ok(res);
        }
        /// <summary>
        /// Get System UserDetails by User Id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult NewGetSystemUserDetailsById(UsersDTO obj)
        {
            var result = service.GetSystemUserDetailsById(obj);
            return Ok(result);
        }
        /// <summary>
        /// Update User status (isApproved=true or false)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult NewUpdateUserStatus(UsersDTO obj)
        {
            var result = service.updateStatus(obj);
            return Ok(result);
        }


        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult NewAddUserProduct(UsersDTO obj)
        {
            var result = service.addproduct(obj);
            return Ok(result);
        }


        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult NewAddUserAccount(UsersDTO obj)
        {
            var result = service.addaccount(obj);
            return Ok(result);
        }

        //[ResponseType(typeof(UsersDTO))]
        //public IHttpActionResult NewAddUserAccount(UsersDTO obj)
        //{
        //    var result = service.addproduct(obj);
        //    return Ok(result);
        //}

        /// <summary>
        /// remove assigned account id or product id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(UsersDTO))]
        public IHttpActionResult NewRemoveAccountOrProduct(UsersDTO obj)
        {
            var result = service.removeaccountproduct(obj);
            return Ok(result);
        }
    }
}
