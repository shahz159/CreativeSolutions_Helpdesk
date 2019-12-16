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
    public class TicketService: ITicketService
    {
        private readonly ITicketModel model;
        public TicketService(TicketModel _model)
        {
            model = _model;
        }
        public TicketDTO InsertTicketRequest(TicketDTO obj)
        {
            try
            {
                var data = model.NewTicketRequest(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.message = data["message"].ToString();
                        //obj.UserEmail = data["UserEmail"].ToString();
                        //obj.ServiceEngineerEmail = data["ServiceEngineerEmail"].ToString();
                    }
                }
                else
                    obj.message = "0";
                if (obj.message == "1")
                {
                    //Send Email
                    //string UserEmail = obj.UserEmail;
                    //string ServiceEngineer = obj.ServiceEngineerEmail;
                    //SendEmail(UserEmail);
                }
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }

        private void SendEmail(string UserEmail)
        {
            try
            {
                // Hr Mail
                string htmlstr = @"";
                StringBuilder HeaderHtml = new StringBuilder();
               
                HeaderHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /><title>AHC Helpdesk</title><link href='https://fonts.googleapis.com/css?family=Open+Sans&display=swap' rel='stylesheet'><style type='text/css'>body{font-family:'Open Sans',sans-serif;background:#f1f1f1;color:#0f0f0f;font-size:14px;padding:20px}.pb-15{padding-bottom:15px}.mb-15{margin-bottom:15px}.text-center{text-align:center}.button{padding:8px 12px;background-color:#2cafdd;border-radius:4px;color:#fff;text-decoration:none;display:inline-block;margin-bottom:15px}.button:hover{background-color:#0d96c6;transition:1s all}</style></head><body style='background-color: #f1f1f1; padding: 15px;'><table align='center' style='width: 600px; margin: 0 auto 0 auto;background-color: #fff;padding: 20px 15px;'><tr><td> <img src='ahc_logo.png' class='pb-15' height='44px;'><h4 style='color: #2cafdd'>Dear Vali Noor,</h4><p> Your ticket has been marked as resolved</p><p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. <a href='#' style='color: #2cafdd; text-decoration: none;'>Click Here</a></p><h4>Ticket Information :</h4><div style='width: 70px; height: 2px; background-color: #000;'></div><div style='background-color: #fff; padding-top: 20px; padding-bottom: 15px;'><table><tr><td>Ticket No</td><td>: AHS-10215</td></tr><tr><td>Resolved Date</td><td>: 03-12-2019</td></tr><tr><td>Register By</td><td>: Vali Noor</td></tr><tr><td>Status</td><td>: Resolved</td></tr><tr><td>Problem Description</td><td>: User cancel the order it shows the file or directory not found error</td></tr><tr><td>Resolved By</td><td>: Mirza Hussaini Baig</td></tr></table></div><table style='margin-bottom: 10px;'><tr><td> <a href='#' target='_blank' style='text-decoration: none; padding: 8px 12px;border: 1px solid #2cafdd; border-radius: 4px; color: #2cafdd; text-decoration: none;'>Button Click</a></td></tr></table><hr><div class='text-center'> <small>please do not hesitate to contact our customer Service support center <strong> +966593631341</strong>,<br> one of our representatives will do their best to assist you.</small></div></td></tr></table></body></html>");

               
                htmlstr = HeaderHtml.ToString();
                string Subject = "New Ticket Email";                
                string mailFrom = System.Configuration.ConfigurationManager.AppSettings["mailFrom"].ToString();
                string mailHRBCC = string.Empty;
                UserEmail = "m.baig@devboxsoftware.com";
                Models.EmailUtility.sendEmail(mailFrom, UserEmail, htmlstr, Subject, mailHRBCC);

                ////Applicant Mail
                //string htmlstr = @"";
                //StringBuilder HeaderHtml = new StringBuilder();
                //HeaderHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'> <html xmlns='http://www.w3.org/1999/xhtml'> <head> <meta http-equiv='Content-Type' content='text/html;charset=utf-8' /> <title>Email</title> <style type='text/css'>body{background-color:#f1f1f1;font-size:16px}body table.main-table{padding:15px}td{padding-bottom:10px}.border{border-top:8px solid #c90}.border-1{border-top:2px solid #c90}.text-center{text-align:center}.text-left{text-align:left}.text-right{text-align:right}.text-justify{text-align:justify}.pad-b-10{padding-bottom:10px}.Discount{font-size:45px;color:#3E9FD2;width:450px;margin:0 auto;padding:5px 15px;border-radius:10px;background-color:#fff}</style> </head> <body bgcolor='#f1f1f1' style='background-color: #f1f1f1;padding:10px;'> <table align='center' class='main-table' style='width: 600px;margin:0 auto 0 auto;background-color:#fff;padding:15px;'> <tr> <td> <table width='100%'> <tr> <td> <img src='https://www.creative-sols.com/webassets/images/cs-images/logo-dark.png' /> </td> <td align='right'> <span>sales@creative-sols.com</span> <br /> <span>+966 593453627</span> </td> </tr> </table> </td> </tr> <tr> <th class='border pad-b-10'></th> </tr> <tr> <td> <h3> Dear Applicant Name,</h3> <p style='font-size: 18px;'>A Contact application has been submitted via website. We will get back you as soon as possible</p> </td> </tr> <tr> <td> <table width='100%'> <tr> <td width='20%'>Full Name:</td> <td>");
                //HeaderHtml.Append(obj.Name);
                //HeaderHtml.Append("</td> </tr> <tr> <td>Email</td> <td>");
                //HeaderHtml.Append(obj.Email);
                //HeaderHtml.Append("</td> </tr> <tr> <td>Phone Number:</td> <td>");
                //HeaderHtml.Append(obj.CountryCode + ' ' + obj.Phone);
                //HeaderHtml.Append("</td> </tr> <tr> <td valign='top'>Message:</td> <td>");
                //HeaderHtml.Append(obj.Description);
                //HeaderHtml.Append("</td> </tr> </table> </td> </tr> <tr> <th class='border-1'></th> </tr> <tr> <td align='center'> <p>As this is an automated response, please do not reply to this email.</p> </td> </tr> <tr> <th class='border'></th> </tr> <tr> <td> <p class='text-center'>© copyright Creative Solutions</p> <p class='text-center'>All rights reserved. Riyadh, Kingdom of Saudi Arabia</p> <center><table><tr><td> <a href='https://www.facebook.com/creativesolutionsksa' target='_blank'> <img src='https://www.creative-sols.com/assets/images/facebook.png' /> </a></td><td> <a href='https://twitter.com/Creative_sols' target='_blank'> <img src='https://www.creative-sols.com/assets/images/twitter.png' /> </a></td><td> <a href='https://accounts.google.com/ServiceLogin/identifier?passive=1209600&osid=1&continue=https%3A%2F%2Fplus.google.com%2F%2BCreativeSolutionsCoLtdRiyadh%2F&followup=https%3A%2F%2Fplus.google.com%2F%2BCreativeSolutionsCoLtdRiyadh%2F&flowName=GlifWebSignIn&flowEntry=ServiceLogin' target='_blank'> <img src='https://www.creative-sols.com/assets/images/googleplus.png' /> </a></td> ​<td> <a href='https://www.linkedin.com/company/creative-sols' target='_blank'> <img src='https://www.creative-sols.com/assets/images/linkedins.png' /> </a></td><td> <a href='https://www.youtube.com/user/creativesols' target='_blank'> <img src='https://www.creative-sols.com/assets/images/youtube.png' /> </a></td><td> <a href='#' target='_blank'> <img src='https://www.creative-sols.com/assets/images/map.png' /> </a></td></tr></table></center> </td> </tr> </table> </body> </html>");
                //htmlstr = HeaderHtml.ToString();
                //string Subject = "Your Contact Request Submitted Successful.";
                //string mailTo = obj.Email;
                //string mailfrom = System.Configuration.ConfigurationManager.AppSettings["mailFrom"].ToString();
                //string mailBCC = string.Empty;
                //UtilityEmail.sendEmail(mailfrom, mailTo, htmlstr, Subject, mailBCC);
            }
            catch (Exception)
            {

            }
        }

        public TicketDTO GetUnderApprovalTickets(TicketDTO obj)
        {
            obj.datasetxml = model.GetApprovalTickets(obj);
            return obj;
        }

        public TicketDTO UpdateTicketStatus(TicketDTO obj)
        {
            try
            {
                var data = model.UpdateTicketStatus(obj);
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

        public TicketDTO AddResponseTime(TicketDTO obj)
        {
            try
            {
                var data = model.AddResponseTieme(obj);
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
        public TicketDTO AddSparePartRequest(TicketDTO obj)
        {
            try
            {
                var data = model.AddSparePartRequest(obj);
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
        public TicketDTO Addcomments(TicketDTO obj)
        {
            try
            {
                var data = model.Addcomments(obj);
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
        public TicketDTO AddEnquirycomments(TicketDTO obj)
        {
            try
            {
                var data = model.AddEnquirycomments(obj);
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
        public TicketDTO AddEnquiry(TicketDTO obj)
        {
            try
            {
                var data = model.AddEnquiry(obj);
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

        public TicketDTO GetSystemUserProducts(TicketDTO obj)
        {
            obj.datasetxml = model.GetSystemUserProducts(obj);
            return obj;
        }
        public TicketDTO GetSystemUserModels(TicketDTO obj)
        {
            obj.datasetxml = model.GetSystemUserModels(obj);
            return obj;
        }
        public TicketDTO GetAccounts(TicketDTO obj)
        {
            obj.datasetxml = model.GetAccounts(obj);
            return obj;
        }
        public TicketDTO GetProducts(TicketDTO obj)
        {
            obj.datasetxml = model.Getproducts(obj);
            return obj;
        }
        
        public TicketDTO GetSystemUserTickets(TicketDTO obj)
        {
            obj.datasetxml = model.GetSystemUserTickets(obj);
            return obj;
        }
        public TicketDTO GetServiceEngineerTickets(TicketDTO obj)
        {
            obj.datasetxml = model.GetServiceEngineerTickets(obj);
            return obj;
        }
        public TicketDTO GetDashboardCount(TicketDTO obj)
        {
            obj.datasetxml = model.GetDashBoardCount(obj);
            return obj;
        }

        
        public TicketDTO GetSparePartRequestTickets(TicketDTO obj)
        {
            obj.datasetxml = model.GetSparePartRequestTickets(obj);
            return obj;
        }
        public TicketDTO GetEnquiryList(TicketDTO obj)
        {
            obj.datasetxml = model.GetEnquiryList(obj);
            return obj;
        }
        public TicketDTO GetEnquiryDetails(TicketDTO obj)
        {
            obj.datasetxml = model.GetEnquiryDetails(obj);
            return obj;
        }

        

        public TicketDTO GetServiceEngineerTicketsFiletrs(TicketDTO obj)
        {
            obj.datasetxml = model.GetServiceEngineerTicketsFilerts(obj);
            return obj;
        }
        public TicketDTO GetTicketDetails(TicketDTO obj)
        {
            obj.datasetxml = model.GetTicketDetails(obj);
            return obj;
        }
        public TicketDTO GetSparePartListById(TicketDTO obj)
        {
            obj.datasetxml = model.GetSparePartListByWarehouseId(obj);
            return obj;
        }
        
    }

    public interface ITicketService
    {
        TicketDTO InsertTicketRequest(TicketDTO obj);
        TicketDTO GetUnderApprovalTickets(TicketDTO obj);
        TicketDTO UpdateTicketStatus(TicketDTO obj);
        TicketDTO AddResponseTime(TicketDTO obj);
        TicketDTO AddSparePartRequest(TicketDTO obj);
        TicketDTO Addcomments(TicketDTO obj);
        TicketDTO AddEnquirycomments(TicketDTO obj);
        TicketDTO AddEnquiry(TicketDTO obj);

        TicketDTO GetSystemUserProducts(TicketDTO obj);
        TicketDTO GetSystemUserModels(TicketDTO obj);
        TicketDTO GetAccounts(TicketDTO obj);
        TicketDTO GetProducts(TicketDTO obj);
        TicketDTO GetSystemUserTickets(TicketDTO obj);
        TicketDTO GetServiceEngineerTickets(TicketDTO obj);
        TicketDTO GetDashboardCount(TicketDTO obj);
        TicketDTO GetSparePartRequestTickets(TicketDTO obj);
        TicketDTO GetEnquiryDetails(TicketDTO obj);
        TicketDTO GetEnquiryList(TicketDTO obj);
        TicketDTO GetServiceEngineerTicketsFiletrs(TicketDTO obj);

        TicketDTO GetTicketDetails(TicketDTO obj);
        TicketDTO GetSparePartListById(TicketDTO obj);
    }
}