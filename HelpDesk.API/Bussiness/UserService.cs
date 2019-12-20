using HelpDesk.API.DataAccess;
using HelpDesk.API.DTO_s;
using HelpDesk.API.GenericHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HelpDesk.API.Bussiness
{
    public class UserService : IUserService
    {
        private readonly IUserModel model;
        public UserService(UserModel _model)
        {
            model = _model;
        }
        string WebURLPath = System.Configuration.ConfigurationManager.AppSettings["weburl"];
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
                        if (obj.message == "1")
                        {
                            obj.AccountName = data["AccountName"].ToString();
                            obj.FullName= data["FullName"].ToString();
                            obj.Email= data["EMail"].ToString();
                            obj.Mobile = data["Mobile"].ToString();
                            obj.CreatedDate = Convert.ToDateTime(data["CreatedOn"].ToString());
                            obj.SuperUserEmail = data["SuperUserEmail"].ToString();
                        }
                    }
                }
                else
                    obj.message = "0";
                if (obj.message == "1")
                {
                    //Send Email to SuperUser                    
                    SendEmailToSuperUser(obj.AccountName, obj.FullName, obj.Email, obj.Mobile, obj.CreatedDate, obj.SuperUserEmail);
                }
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }

        private void SendEmailToSuperUser(string AccountName, string FullName, string Email, string Mobile, DateTime CreatedDate, string SuperUserEmail)
        {
            try
            {
                // Customer Email
                string htmlstr = @"";
                StringBuilder HeaderHtml = new StringBuilder();

                HeaderHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /><title>AHC Helpdesk</title><link href='https://fonts.googleapis.com/css?family=Open+Sans&display=swap' rel='stylesheet'><style type='text/css'>body{font-family:'Open Sans',sans-serif;background:#f1f1f1;color:#0f0f0f;font-size:14px;padding:20px}.pb-15{padding-bottom:15px}.mb-15{margin-bottom:15px}.text-center{text-align:center}.button{padding:8px 12px;background-color:#2cafdd;border-radius:4px;color:#fff;text-decoration:none;display:inline-block;margin-bottom:15px}.button:hover{background-color:#0d96c6;transition:1s all}</style></head><body style='background-color: #f1f1f1; padding: 15px;'><table align='center' style='width: 600px; margin: 0 auto 0 auto;background-color: #fff;padding: 20px 15px;'><tr><td>");
                HeaderHtml.Append("<img src='" + WebURLPath + "assets/images/ahc_new_logo.png' class='pb-15' height='44px;'><h4 style='color: #2cafdd'>Dear Super User,");
                HeaderHtml.Append("</h4><p> This is an automated message from the AHC Helpdesk System to inform you that new Customer has been Registered in Portal. Please review and need your Approval to activate.</p><h4>Customer Information :</h4><div style='width: 70px; height: 2px; background-color: #000;'></div><div style='background-color: #fff; padding-top: 20px; padding-bottom: 15px;'><table><tr>");
                HeaderHtml.Append("<td>Account Name</td><td>:" + AccountName + "</td>");                
                HeaderHtml.Append("</tr><tr><td>Customer Name</td><td>: " + FullName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Email</td><td>: " + Email + "</td>");
                HeaderHtml.Append("</tr><tr><td>Mobile</td><td>: " + Mobile + "</td>");                
                HeaderHtml.Append("</tr><tr><td>Registered Date</td><td>: " + CreatedDate + "</td</tr></table><br>");
                HeaderHtml.Append("<table style='margin-bottom: 10px;'><tr><td> <a href='#' target='_blank' style='text-decoration: none; padding: 8px 12px;border: 1px solid #2cafdd; border-radius: 4px; color: #2cafdd; text-decoration: none;'>Button Click</a></td></tr></table><br><hr><div class='text-center'> <small>please do not hesitate to contact our customer Service support center <strong> +966511111111</strong>,<br> one of our representatives will do their best to assist you.</small></div></td></tr></table></body></html>");

                htmlstr = HeaderHtml.ToString();
                string Subject = "New Customer Registration";
                string mailFrom = System.Configuration.ConfigurationManager.AppSettings["mailFrom"].ToString();
                string mailHRBCC = string.Empty;
                SuperUserEmail = "hussainibaigm@gmail.com";
                Models.EmailUtility.sendEmail(mailFrom, SuperUserEmail, htmlstr, Subject, mailHRBCC);
            }
            catch (Exception)
            {

            }
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
