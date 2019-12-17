using HelpDesk.API.Bussiness;
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
