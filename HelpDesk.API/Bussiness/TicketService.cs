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
    public class TicketService : ITicketService
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
                        if (obj.message == "1")
                        {
                            obj.TicketNumber = Convert.ToInt64(data["TicketNumber"].ToString());
                            obj.CreatedDate = Convert.ToDateTime(data["CreatedOn"].ToString());
                            obj.ProductName = data["ProductName"].ToString();
                            obj.SystemId = data["SystemNo"].ToString();
                            obj.AccountName = data["AccountName"].ToString();
                            obj.Location = data["Area"].ToString();
                            obj.ProblemDescription = data["Description"].ToString();
                            obj.CustomerEmail = data["CustomerEmail"].ToString();
                            obj.CustomerFullName = data["CustomerFullName"].ToString();
                            obj.EngineerEmail = data["EngineerEmail"].ToString();
                            obj.EngineerFullName = data["EngineerFullName"].ToString();
                            obj.ServiceEngineerEmail = data["EngineerEmail"].ToString();
                        }
                    }
                }
                else
                    obj.message = "0";
                if (obj.message == "1")
                {
                    //Send Email to Customer                    
                    SendEmailToCustomer(obj.TicketNumber, obj.CreatedDate, obj.ProductName, obj.SystemId, obj.AccountName, obj.Location, obj.ProblemDescription, obj.CustomerEmail, obj.CustomerFullName, obj.EngineerFullName);
                    SendEmailToEngineer(obj.TicketNumber, obj.CreatedDate, obj.ProductName, obj.SystemId, obj.AccountName, obj.Location, obj.ProblemDescription, obj.ServiceEngineerEmail, obj.CustomerFullName, obj.EngineerFullName);
                }
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }

        private void SendEmailToEngineer(long TicketNumber, DateTime CreatedDate, string ProductName, string SystemId, string AccountName, string Location, string ProblemDescription, string ServiceEngineerEmail, string CustomerFullName, string EngineerFullName)
        {
            try
            {
                // Customer Email
                string htmlstr = @"";
                StringBuilder HeaderHtml = new StringBuilder();

                HeaderHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /><title>AHC Helpdesk</title><link href='https://fonts.googleapis.com/css?family=Open+Sans&display=swap' rel='stylesheet'><style type='text/css'>body{font-family:'Open Sans',sans-serif;background:#f1f1f1;color:#0f0f0f;font-size:14px;padding:20px}.pb-15{padding-bottom:15px}.mb-15{margin-bottom:15px}.text-center{text-align:center}.button{padding:8px 12px;background-color:#2cafdd;border-radius:4px;color:#fff;text-decoration:none;display:inline-block;margin-bottom:15px}.button:hover{background-color:#0d96c6;transition:1s all}</style></head><body style='background-color: #f1f1f1; padding: 15px;'><table align='center' style='width: 600px; margin: 0 auto 0 auto;background-color: #fff;padding: 20px 15px;'><tr><td> <img src='ahc_logo.png' class='pb-15' height='44px;'><h4 style='color: #2cafdd'>Dear ");
                HeaderHtml.Append("" + EngineerFullName + ",</h4><p> This is an automated message from the AHC Helpdesk system to confirm that this ticket has been assigned to you. <a href='#' style='color: #2cafdd; text-decoration: none;'>Click Here</a></p><h4>Ticket Information :</h4><div style='width: 70px; height: 2px; background-color: #000;'></div><div style='background-color: #fff; padding-top: 20px; padding-bottom: 15px;'><table><tr>");
                HeaderHtml.Append("<td>Ticket No</td><td>:" + TicketNumber + "</td>");
                HeaderHtml.Append("</tr><tr><td>Created Date</td><td>: " + CreatedDate + "</td>");
                HeaderHtml.Append("</tr><tr><td>Product</td><td>: " + ProductName + "</td>");
                HeaderHtml.Append("</tr><tr><td>System Id</td><td>: " + SystemId + "</td>");
                HeaderHtml.Append("</tr><tr><td>Account</td><td>: " + AccountName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Problem Description</td><td>: " + ProblemDescription + "</td>");
                HeaderHtml.Append("</tr><tr><td>Assigned By</td><td>: " + CustomerFullName + "</td>");
                HeaderHtml.Append("<table style='margin-bottom: 10px;'><tr><td> <a href='#' target='_blank' style='text-decoration: none; padding: 8px 12px;border: 1px solid #2cafdd; border-radius: 4px; color: #2cafdd; text-decoration: none;'>Button Click</a></td></tr></table><hr><div class='text-center'> <small>please do not hesitate to contact our customer Service support center <strong> +966593631341</strong>,<br> one of our representatives will do their best to assist you.</small></div></td></tr></table></body></html>");

                htmlstr = HeaderHtml.ToString();
                string Subject = "New Ticket Email";
                string mailFrom = System.Configuration.ConfigurationManager.AppSettings["mailFrom"].ToString();
                string mailHRBCC = string.Empty;
                ServiceEngineerEmail = "hussainibaigm@gmail.com";
                Models.EmailUtility.sendEmail(mailFrom, ServiceEngineerEmail, htmlstr, Subject, mailHRBCC);
            }
            catch (Exception)
            {

            }
        }

        private void SendEmailToCustomer(long TicketNumber, DateTime CreatedDate, string ProductName, string SystemId, string AccountName, string Location, string ProblemDescription, string CustomerEmail, string CustomerFullName, string EngineerFullName)
        {
            try
            {
                // Customer Email
                string htmlstr = @"";
                StringBuilder HeaderHtml = new StringBuilder();

                HeaderHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /><title>AHC Helpdesk</title><link href='https://fonts.googleapis.com/css?family=Open+Sans&display=swap' rel='stylesheet'><style type='text/css'>body{font-family:'Open Sans',sans-serif;background:#f1f1f1;color:#0f0f0f;font-size:14px;padding:20px}.pb-15{padding-bottom:15px}.mb-15{margin-bottom:15px}.text-center{text-align:center}.button{padding:8px 12px;background-color:#2cafdd;border-radius:4px;color:#fff;text-decoration:none;display:inline-block;margin-bottom:15px}.button:hover{background-color:#0d96c6;transition:1s all}</style></head><body style='background-color: #f1f1f1; padding: 15px;'><table align='center' style='width: 600px; margin: 0 auto 0 auto;background-color: #fff;padding: 20px 15px;'><tr><td> <img src='ahc_logo.png' class='pb-15' height='44px;'><h4 style='color: #2cafdd'>Dear ");
                HeaderHtml.Append("" + CustomerFullName + ",</h4><p> This is an automated message from the AHC Helpdesk System to confirm that this ticket has been created. Our aim to continuously provide a fast and efficient service.  . <a href='#' style='color: #2cafdd; text-decoration: none;'>Click Here</a></p><h4>Ticket Information :</h4><div style='width: 70px; height: 2px; background-color: #000;'></div><div style='background-color: #fff; padding-top: 20px; padding-bottom: 15px;'><table><tr>");
                HeaderHtml.Append("<td>Ticket No</td><td>:" + TicketNumber + "</td>");
                HeaderHtml.Append("</tr><tr><td>Created Date</td><td>: " + CreatedDate + "</td>");
                HeaderHtml.Append("</tr><tr><td>Product</td><td>: " + ProductName + "</td>");
                HeaderHtml.Append("</tr><tr><td>System Id</td><td>: " + SystemId + "</td>");
                HeaderHtml.Append("</tr><tr><td>Account</td><td>: " + AccountName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Problem Description</td><td>: " + ProblemDescription + "</td>");
                HeaderHtml.Append("</tr><tr><td>Assigned To</td><td>: " + EngineerFullName + "</td>");
                HeaderHtml.Append("<table style='margin-bottom: 10px;'><tr><td> <a href='#' target='_blank' style='text-decoration: none; padding: 8px 12px;border: 1px solid #2cafdd; border-radius: 4px; color: #2cafdd; text-decoration: none;'>Button Click</a></td></tr></table><hr><div class='text-center'> <small>please do not hesitate to contact our customer Service support center <strong> +966593631341</strong>,<br> one of our representatives will do their best to assist you.</small></div></td></tr></table></body></html>");

                htmlstr = HeaderHtml.ToString();
                string Subject = "New Ticket Email";
                string mailFrom = System.Configuration.ConfigurationManager.AppSettings["mailFrom"].ToString();
                string mailHRBCC = string.Empty;
                CustomerEmail = "hussainibaigm@gmail.com";
                Models.EmailUtility.sendEmail(mailFrom, CustomerEmail, htmlstr, Subject, mailHRBCC);
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

        TicketDTO GetSystemUserProducts(TicketDTO obj);
        TicketDTO GetSystemUserModels(TicketDTO obj);
        TicketDTO GetAccounts(TicketDTO obj);
        TicketDTO GetProducts(TicketDTO obj);
        TicketDTO GetSystemUserTickets(TicketDTO obj);
        TicketDTO GetServiceEngineerTickets(TicketDTO obj);
        TicketDTO GetDashboardCount(TicketDTO obj);
        TicketDTO GetSparePartRequestTickets(TicketDTO obj);
        TicketDTO GetServiceEngineerTicketsFiletrs(TicketDTO obj);

        TicketDTO GetTicketDetails(TicketDTO obj);
        TicketDTO GetSparePartListById(TicketDTO obj);
    }
}