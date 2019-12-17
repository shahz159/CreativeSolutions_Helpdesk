using HelpDesk.API.DataAccess;
using HelpDesk.API.DTO_s;
using HelpDesk.API.GenericHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API.Bussiness
{
    public class UserService: IUserService
    {
        private readonly IUserModel model;
        public UserService(UserModel _model)
        {
            model = _model;
        }

        public UsersDTO CheckEmailExists(string email)
        {
            UsersDTO obj = new UsersDTO();
            try
            {
                var data = model.CheckEmailExists(email);
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

        public UsersDTO CheckEmpIdExists(string empid)
        {
            UsersDTO obj = new UsersDTO();
            try
            {
                var data = model.CheckEmpIdExists(empid);
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

        public IEnumerable<UsersDTO> GetCompanyAccounts(UsersDTO obj)
        {
            var data = model.GetCompanyAccounts(obj);
            var List = CustomDataReaderToGenericExtension.GetDataObjects<UsersDTO>(data);
            data.Close();
            return List;
        }
        public IEnumerable<UsersDTO> GetCompanyProducts(UsersDTO obj)
        {
            var data = model.GetCompanyProducts(obj);
            var List = CustomDataReaderToGenericExtension.GetDataObjects<UsersDTO>(data);
            data.Close();
            return List;
        }
        
        public UsersDTO GetUserDetailsById(UsersDTO obj)
        {
            obj.datasetxml = model.GetUserDetailsById(obj);
            return obj;
        }

        public UsersDTO GetUserList(UsersDTO obj)
        {
            obj.datasetxml = model.GetUserList(obj);
            return obj;
        }
        public UsersDTO GetSystemUserforApprovalList(UsersDTO obj)
        {
            obj.datasetxml = model.GetSystemUserListforApproval(obj);
            return obj;
        }
        public UsersDTO GetSystemUserDetailsById(UsersDTO obj)
        {
            obj.datasetxml = model.GetSystemUserListforApproval(obj);
            return obj;
        }
        
        public UsersDTO NewUser(UsersDTO obj)
        {
            try
            {
                var data = model.NewUser(obj);
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

        public UsersDTO RoleCompanyDropDowns(UsersDTO obj)
        {
            obj.datasetxml = model.RoleCompanyDropDowns(obj);
            return obj;
        }

        public UsersDTO removeaccountproduct(UsersDTO obj)
        {
            try
            {
                var data = model.removeaccountorproduct(obj);
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
        public UsersDTO updateStatus(UsersDTO obj)
        {
            try
            {
                var data = model.updateuserstatus(obj);
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
        public UsersDTO addproduct(UsersDTO obj)
        {
            try
            {
                var data = model.addproduct(obj);
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

        
    }

    public interface IUserService
    {
        UsersDTO NewUser(UsersDTO obj);
        UsersDTO RoleCompanyDropDowns(UsersDTO obj);
        UsersDTO GetUserDetailsById(UsersDTO obj);
        UsersDTO GetUserList(UsersDTO obj);
        UsersDTO updateStatus(UsersDTO obj);
        UsersDTO addproduct(UsersDTO obj);
        UsersDTO removeaccountproduct(UsersDTO obj);
        UsersDTO GetSystemUserforApprovalList(UsersDTO obj);
        UsersDTO GetSystemUserDetailsById(UsersDTO obj);
        UsersDTO CheckEmailExists(string email);
        UsersDTO CheckEmpIdExists(string empid);
        IEnumerable<UsersDTO> GetCompanyAccounts(UsersDTO obj);
        IEnumerable<UsersDTO> GetCompanyProducts(UsersDTO obj);
    }
}
