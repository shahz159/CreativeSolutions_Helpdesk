using HelpDesk.API.DataAccess;
using HelpDesk.API.DTO_s;
using HelpDesk.API.GenericHelpers;
using Newtonsoft.Json;
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
        public UsersDTO ChangePasswordRequest(string email)
        {
            UsersDTO obj = new UsersDTO();
            try
            {
                Guid obj_token = Guid.NewGuid();
                string token = obj_token.ToString();

                var data = model.changepasswordrequest(email, token);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.message = data["message"].ToString();
                    }
                }
                else
                    obj.message = "0";

                //comment for demo which was on 16-may-2023
                //start here
                //if (obj.message == "1")
                //{
                //    //Send Email to Customer                    
                //    SendEmailToCustomer(
                //                    email, token
                //                );
                //}
                //end here
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }
        public UsersDTO EmailNotificationService()
        {
            UsersDTO obj = new UsersDTO();
            var data = model.emailnotificationmodel();
            var List = CustomDataReaderToGenericExtension.GetDataObjects<UsersDTO>(data);
            data.Close();
            foreach (var item in List)
            {
                try
                {
                    if (WindowsServiceEmailNotification(item.Email, item.Subject, item.Body) == true)
                    {
                        var dataII = model.updateemailnotificationmodel(item.EmailId);
                        obj.message = "1";
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return obj;
        }

        public UsersDTO verifyPasswordRequest(string email, string token)
        {
            UsersDTO obj = new UsersDTO();
            try
            {
                var data = model.verifypasswordrequest(email, token);
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
        private void SendEmailToCustomer(string email, string token)
        {
            try
            {
                //Customer Email
                //string restlink = "http://208.109.10.196/AHCHelpdesk/Login/ResetPassword?username=" + email + "&token=" + token;
                string restlink = "http://support.arabianhc.com/Login/ResetPassword?username=" + email + "&token=" + token;
                string htmlstr = @"";
                StringBuilder HeaderHtml = new StringBuilder();
                HeaderHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /><title>AHC Helpdesk</title><link href='https://fonts.googleapis.com/css?family=Open+Sans&display=swap' rel='stylesheet'><style type='text/css'>body{font-family:'Open Sans',sans-serif;background:#f1f1f1;color:#0f0f0f;font-size:14px;padding:20px}.pb-15{padding-bottom:15px}.mb-15{margin-bottom:15px}.text-center{text-align:center}.button{padding:8px 12px;background-color:#2cafdd;border-radius:4px;color:#fff;text-decoration:none;display:inline-block;margin-bottom:15px}</style></head><body style='background-color: #f1f1f1; padding: 15px;'><table align='center' style='width: 600px; margin: 0 auto 0 auto;background-color: #fff;padding: 20px 15px;'><tr><td> <img src='http://support.arabianhc.com/assets/images/ahc_new_logo.png' class='pb-15' height='44px;'><h4 style='color: #2cafdd;font-size: 30px;margin: 15px 0;'>Forget Your Password..?</h4><p style='color:black; font-size: 14px;line-height: 1.5;'> It seems like you forgot your password for [customer portal]. If this is true, click the link below to reset your password.</p><div style='background-color: #fff; padding-top: 20px; padding-bottom: 15px;'><table><tr><td align='ce'> <a href='" + restlink + "' class='button'>Reset password</a></td></tr></table></div><p style='color:black;'> If you did not forget your password, please disregard this email.</p> <br><hr><div class='text-center'> <small>Please do not hesitate to contact <code style='font-size: 14px; color:black;'>AHC</code> Customer Service Support Center <strong> 800 2444416</strong>, </small></div></td></tr></table></body></html>");
                htmlstr = HeaderHtml.ToString();
                string Subject = "AHC Helpdesk Support Centre";
                string mailFrom = System.Configuration.ConfigurationManager.AppSettings["mailFrom"].ToString();
                string mailHRBCC = string.Empty;
                //CustomerEmail = "aqibshahbaz@gmail.com";
                Models.EmailUtility.sendEmail(mailFrom, email, htmlstr, Subject, mailHRBCC);
            }
            catch (Exception)
            {

            }
        }

        private bool WindowsServiceEmailNotification(string email, string subject, string body)
        {
            try
            {
                //Customer Email
                //string restlink = "http://208.109.10.196/AHCHelpdeskTest/Login/ResetPassword?username=" + email;
                string htmlstr = @"";
                StringBuilder HeaderHtml = new StringBuilder();
                HeaderHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /><title>AHC Helpdesk</title><link href='https://fonts.googleapis.com/css?family=Open+Sans&display=swap' rel='stylesheet'><style type='text/css'>body{font-family:'Open Sans',sans-serif;background:#f1f1f1;color:#0f0f0f;font-size:14px;padding:20px}.pb-15{padding-bottom:15px}.mb-15{margin-bottom:15px}.text-center{text-align:center}.button{padding:8px 12px;background-color:#2cafdd;border-radius:4px;color:#fff;text-decoration:none;display:inline-block;margin-bottom:15px}</style></head><body style='background-color: #f1f1f1; padding: 15px;'><table align='center' style='width: 600px; margin: 0 auto 0 auto;background-color: #fff;padding: 20px 15px;'><tr><td> <img src='http://support.arabianhc.com/assets/images/ahc_new_logo.png' class='pb-15' height='44px;'><p style='color:black; font-size: 14px;line-height: 1.5;'> Auto Generated Email Notification.</p><div style='background-color: #fff; padding-top: 20px; padding-bottom: 15px;'></div> <br><hr><div class='text-center'> <small>Please do not hesitate to contact <code style='font-size: 14px; color:black;'>AHC</code> Customer Service Support Center <strong> 800 2444416</strong>, </small></div></td></tr></table></body></html>");
                htmlstr = HeaderHtml.ToString();
                string Subject = "AHC Helpdesk Support Centre";
                string mailFrom = System.Configuration.ConfigurationManager.AppSettings["mailFrom"].ToString();
                string mailHRBCC = string.Empty;
                //CustomerEmail = "aqibshahbaz@gmail.com";
                Models.EmailUtility.sendEmail(mailFrom, email, htmlstr, Subject, mailHRBCC);
                return true;

            }
            catch (Exception)
            {
                return false;
            }
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
        public IEnumerable<UsersDTO> GetCompanyManagerAccounts(UsersDTO obj)
        {
            var data = model.GetCompanyManagerAccounts(obj);
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

        public IEnumerable<UsersDTO> GetCompanyProductsRoleWise(UsersDTO obj)
        {
            var data = model.GetCompanyProductsRoleWise(obj);
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

        public UsersDTO NewUserSignup(UsersDTO obj)
        {
            try
            {

                var data = model.NewUserSignUp(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.message = data["message"].ToString();
                        if (obj.message == "1")
                        {
                            obj.EmailJson = data["EmailJson"].ToString();
                            obj.ProductName = data["ProductNames"].ToString();
                        }
                    }
                }
                else
                    obj.message = "0";

                //comment for demo which was on 16-may-2023
                //start here
                //var modell = JsonConvert.DeserializeObject<List<UsersDTO>>(obj.EmailJson);
                //obj.EmailList = modell;
                //foreach (var item in obj.EmailList)
                //{
                //    SendEmailToSuperUserSignUp(obj.Accounts[0], obj.FullName, obj.Email, obj.Mobile, obj.CreatedDate, item.Email,obj.ProductName);
                //}
                //end here

                //if (obj.message == "1")
                //{
                //    //Send Email to SuperUser                    
                //    SendEmailToSuperUser(obj.Accounts[0], obj.FullName, obj.Email, obj.Mobile, obj.CreatedDate, obj.SuperUserEmail);
                //    //SendEmailOfSignUp(obj.AccountName, obj.ProductName, obj.FullName, obj.Gender, obj.Mobile,, obj.Email, obj.SuperUserEmail);
                //}
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
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
                            obj.FullName = data["FullName"].ToString();
                            obj.Email = data["EMail"].ToString();
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
                    //comment for demo which was on 16-may-2023
                    //start here
                    //SendEmailToSuperUser(obj.Accounts[0], obj.FullName, obj.Email, obj.Mobile, obj.CreatedDate, obj.SuperUserEmail);
                    //end here

                    //SendEmailOfSignUp(obj.AccountName, obj.ProductName, obj.FullName, obj.Gender, obj.Mobile,, obj.Email, obj.SuperUserEmail);
                }
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }
        //private void SendEmailOfSignUp(string AccountName, string ProductName, string FullName, string Gender, string PhoneNumber, string EmailId, string SuperUserEmail)
        //{
        //    try
        //    {
        //        string htmlstr = @"";
        //        StringBuilder HeaderHtml = new StringBuilder();

        //        HeaderHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
        //        HeaderHtml.Append("<html xmlns='http://www.w3.org/1999/xhtml'>");
        //        HeaderHtml.Append("<head>");
        //        HeaderHtml.Append("<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />");
        //        HeaderHtml.Append("<title>AHC Helpdesk</title>");
        //        HeaderHtml.Append("<link href='https://fonts.googleapis.com/css?family=Open+Sans&display=swap' rel='stylesheet'>");
        //        HeaderHtml.Append("<style type='text/css'>body {font-family: 'Open Sans', sans-serif;background: #f1f1f1;color: #0f0f0f;font-size: 14px;padding: 20px}.pb-15 {padding-bottom: 15px}.mb-15 {margin-bottom: 15px}.text-center {text-align: center}.button {padding: 8px 12px;background-color: #2cafdd;border-radius: 4px;color: #fff;text-decoration: none;display: inline-block;margin-bottom: 15px}.button:hover {background-color: #0d96c6;transition: 1s all}table.details-table tr td{vertical-align: top;padding-bottom: 5px;}</style>");
        //        HeaderHtml.Append("</head>");
        //        HeaderHtml.Append("<body style='background-color: #f1f1f1; padding: 15px;'>");
        //        HeaderHtml.Append("<table align='center' style='width: 600px; margin: 0 auto 0 auto;background-color: #fff;padding: 20px 15px;'><tr><td><img src='http://support.arabianhc.com/assets/images/ahc_new_logo.png' class='pb-15' height='44px;'><p style='color:black;'> Thank you for contacting AHC Helpdesk, <a href='http://support.arabianhc.com' target='_blank'>http://support.arabianhc.com</a>.</p><p style='color:black;'> The Signup Service with below information has been generated and your request will be attended shortly.</p><h4>Signup Information :</h4><div style='width: 70px; height: 2px; background-color: #000;'></div><div style='background-color: #fff; padding-top: 20px; padding-bottom: 15px;'>");
        //        HeaderHtml.Append("<table class='details-table'width='100%'>");
        //        HeaderHtml.Append("<tr>");
        //        HeaderHtml.Append("<td>Account</td><td>:</td><td>:" + AccountName + "</td>");
        //        HeaderHtml.Append("<td>Product</td><td>:</td><td>:" + ProductName + "</td>");
        //        HeaderHtml.Append("<td>Full Name</td><td>:</td><td>:" + FullName + "</td>");
        //        HeaderHtml.Append("<td>Gender</td><td>:</td><td>:" + Gender + "</td>");
        //        HeaderHtml.Append("<td>Phone Number</td><td>:</td><td>:" + PhoneNumber + "</td>");
        //        HeaderHtml.Append("<td>Email Id</td><td>:</td><td>:" + EmailId + "</td>");
        //        HeaderHtml.Append("</table><br><hr>");
        //        HeaderHtml.Append("<div class='text-center'> <small>please do not hesitate to contact our customer Service support center <strong> +96651111111</strong>,<br> one of our representatives will do their best to assist you.</small></div>");
        //        HeaderHtml.Append("</td>");
        //        HeaderHtml.Append("</tr>");
        //        HeaderHtml.Append("</table>");
        //        HeaderHtml.Append("</body>");
        //        HeaderHtml.Append("</html>");
        //        HeaderHtml.Append("<hr>");
        //        HeaderHtml.Append("<div class='text-center'> <small>Please do not hesitate to contact <code style='font-size: 14px; color:black;'>AHC</code> Customer Service Support Center <strong> 800 2444416</strong>,</small></div>");
        //        HeaderHtml.Append("</td>");
        //        HeaderHtml.Append("</tr>");
        //        HeaderHtml.Append("</table>");
        //        HeaderHtml.Append("</body>");
        //        HeaderHtml.Append("</html>");

        //        htmlstr = HeaderHtml.ToString();
        //        string Subject = "AHC Helpdesk Support Centre";
        //        string mailFrom = System.Configuration.ConfigurationManager.AppSettings["mailFrom"].ToString();
        //        string mailHRBCC = string.Empty;
        //        //ServiceEngineerEmail = "aqibshahbaz@gmail.com";
        //        Models.EmailUtility.sendEmail(mailFrom, SuperUserEmail, htmlstr, Subject, mailHRBCC);
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}
        private void SendEmailToSuperUser(string AccountName, string FullName, string Email, string Mobile, DateTime CreatedDate, string SuperUserEmail)
        {
            try
            {
                // Customer Email
                string htmlstr = @"";
                StringBuilder HeaderHtml = new StringBuilder();

                HeaderHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /><title>AHC Helpdesk</title><link href='https://fonts.googleapis.com/css?family=Open+Sans&display=swap' rel='stylesheet'><style type='text/css'>body{font-family:'Open Sans',sans-serif;background:#f1f1f1;color:#0f0f0f;font-size:14px;padding:20px}.pb-15{padding-bottom:15px}.mb-15{margin-bottom:15px}.text-center{text-align:center}.button{padding:8px 12px;background-color:#2cafdd;border-radius:4px;color:#fff;text-decoration:none;display:inline-block;margin-bottom:15px}.button:hover{background-color:#0d96c6;transition:1s all}</style></head><body style='background-color: #f1f1f1; padding: 15px;'><table align='center' style='width: 600px; margin: 0 auto 0 auto;background-color: #fff;padding: 20px 15px;'><tr><td>");
                HeaderHtml.Append("<img src='" + WebURLPath + "assets/images/ahc_new_logo.png' class='pb-15' height='44px;'><h4 style='color: #2cafdd'>Dear Super User,");
                HeaderHtml.Append("</h4><p> This is an automated message from the AHC Helpdesk System to inform you that new Customer has been Registered in Portal. Please review and add it.</p><h4>Customer Information :</h4><div style='width: 70px; height: 2px; background-color: #000;'></div><div style='background-color: #fff; padding-top: 20px; padding-bottom: 15px;'><table><tr>");
                HeaderHtml.Append("<td>Account Name</td><td>:" + AccountName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Customer Name</td><td>: " + FullName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Email</td><td>: " + Email + "</td>");
                HeaderHtml.Append("</tr><tr><td>Mobile</td><td>: " + Mobile + "</td>");
                HeaderHtml.Append("</tr><tr><td>Registered Date</td><td>: " + CreatedDate + "</td></tr></table><br>");
                HeaderHtml.Append("<hr><div class='text-center'> <small>please do not hesitate to contact our customer Service support center <strong> +966511111111</strong>,<br> one of our representatives will do their best to assist you.</small></div></td></tr></table></body></html>");

                htmlstr = HeaderHtml.ToString();
                string Subject = "New Customer Registration";
                string mailFrom = System.Configuration.ConfigurationManager.AppSettings["mailFrom"].ToString();
                string mailHRBCC = string.Empty;
                //SuperUserEmail = "hussainibaigm@gmail.com";
                Models.EmailUtility.sendEmail(mailFrom, SuperUserEmail, htmlstr, Subject, mailHRBCC);
            }
            catch (Exception)
            {

            }
        }
        private void SendEmailToSuperUserSignUp(string AccountName, string FullName, string Email, string Mobile, DateTime CreatedDate, string SuperUserEmail, string ProductName)
        {
            try
            {
                // Customer Email
                string htmlstr = @"";
                StringBuilder HeaderHtml = new StringBuilder();

                HeaderHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /><title>AHC Helpdesk</title><link href='https://fonts.googleapis.com/css?family=Open+Sans&display=swap' rel='stylesheet'><style type='text/css'>body{font-family:'Open Sans',sans-serif;background:#f1f1f1;color:#0f0f0f;font-size:14px;padding:20px}.pb-15{padding-bottom:15px}.mb-15{margin-bottom:15px}.text-center{text-align:center}.button{padding:8px 12px;background-color:#2cafdd;border-radius:4px;color:#fff;text-decoration:none;display:inline-block;margin-bottom:15px}.button:hover{background-color:#0d96c6;transition:1s all}</style></head><body style='background-color: #f1f1f1; padding: 15px;'><table align='center' style='width: 600px; margin: 0 auto 0 auto;background-color: #fff;padding: 20px 15px;'><tr><td>");
                HeaderHtml.Append("<img src='" + WebURLPath + "assets/images/ahc_new_logo.png' class='pb-15' height='44px;'><h4 style='color: #2cafdd'>Dear Super User,");
                HeaderHtml.Append("</h4><p> This is an automated message from the AHC Helpdesk System to inform you that new Customer has been Registered in Portal. Please review and add it.</p><h4>Customer Information :</h4><div style='width: 70px; height: 2px; background-color: #000;'></div><div style='background-color: #fff; padding-top: 20px; padding-bottom: 15px;'><table><tr>");
                HeaderHtml.Append("<td>Account Name</td><td>:" + AccountName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Customer Name</td><td>: " + FullName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Email</td><td>: " + Email + "</td>");
                HeaderHtml.Append("</tr><tr><td>Mobile</td><td>: " + Mobile + "</td>");
                //HeaderHtml.Append("</tr><tr><td>Registered Date</td><td>: " + CreatedDate + "</td>");
                HeaderHtml.Append("</tr><tr><td>Products</td><td>: " + ProductName + "</td></tr></table><br>");
                HeaderHtml.Append("<hr><div class='text-center'> <small>please do not hesitate to contact our customer Service support center <strong> +966511111111</strong>,<br> one of our representatives will do their best to assist you.</small></div></td></tr></table></body></html>");

                htmlstr = HeaderHtml.ToString();
                string Subject = "New Customer Registration";
                string mailFrom = System.Configuration.ConfigurationManager.AppSettings["mailFrom"].ToString();
                string mailHRBCC = string.Empty;
                //SuperUserEmail = "hussainibaigm@gmail.com";
                Models.EmailUtility.sendEmail(mailFrom, SuperUserEmail, htmlstr, Subject, mailHRBCC);
            }
            catch (Exception)
            {

            }
        }
        public UsersDTO UpdateUserInfoBasic(UsersDTO obj)
        {
            try
            {
                var data = model.UpdateUser(obj);
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
        public UsersDTO UpdateSignUpUserStatus(UsersDTO obj)
        {
            try
            {
                var data = model.UpdateSignUpUser(obj);
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
        public UsersDTO UpdateUserStatusActive(UsersDTO obj)
        {
            try
            {
                var data = model.UpdateUserStatusActive(obj);
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




        public UsersDTO UpdateUserPassword(UsersDTO obj)
        {
            try
            {
                var data = model.UpdateUserpassword(obj);
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
        public UsersDTO UpdateUserPasswordWithEmail(UsersDTO obj)
        {
            try
            {
                var data = model.UpdateUserpasswordwithemail(obj);
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
        public UsersDTO addaccount(UsersDTO obj)
        {
            try
            {
                var data = model.addAccount(obj);
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
        UsersDTO UpdateUserPasswordWithEmail(UsersDTO obj);
        UsersDTO UpdateUserPassword(UsersDTO obj);
        UsersDTO UpdateUserInfoBasic(UsersDTO obj);
        UsersDTO UpdateSignUpUserStatus(UsersDTO obj);
        UsersDTO UpdateUserStatusActive(UsersDTO obj);
        UsersDTO NewUser(UsersDTO obj);
        UsersDTO NewUserSignup(UsersDTO obj);
        UsersDTO RoleCompanyDropDowns(UsersDTO obj);
        UsersDTO GetUserDetailsById(UsersDTO obj);
        UsersDTO GetUserList(UsersDTO obj);
        UsersDTO updateStatus(UsersDTO obj);
        UsersDTO addproduct(UsersDTO obj);
        UsersDTO addaccount(UsersDTO obj);
        UsersDTO removeaccountproduct(UsersDTO obj);
        UsersDTO GetSystemUserforApprovalList(UsersDTO obj);
        UsersDTO GetSystemUserDetailsById(UsersDTO obj);
        UsersDTO CheckEmailExists(string email);
        UsersDTO ChangePasswordRequest(string email);
        UsersDTO EmailNotificationService();
        UsersDTO verifyPasswordRequest(string email, string token);
        UsersDTO CheckEmpIdExists(string empid);
        IEnumerable<UsersDTO> GetCompanyAccounts(UsersDTO obj);
        IEnumerable<UsersDTO> GetCompanyManagerAccounts(UsersDTO obj);
        IEnumerable<UsersDTO> GetCompanyProducts(UsersDTO obj);
        IEnumerable<UsersDTO> GetCompanyProductsRoleWise(UsersDTO obj);
    }
}
