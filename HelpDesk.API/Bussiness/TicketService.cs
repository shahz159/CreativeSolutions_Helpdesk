using HelpDesk.API.DataAccess;
using HelpDesk.API.DTO_s;
using HelpDesk.API.GenericHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Net.Http;

namespace HelpDesk.API.Bussiness
{
    public class TicketService : ITicketService
    {
        private readonly ITicketModel model;
        public TicketService(TicketModel _model)
        {
            model = _model;
        }
        string WebURLPath = System.Configuration.ConfigurationManager.AppSettings["weburl"];
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
                            obj.CreatedDateStr = data["CreatedDateStr"].ToString();
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
                            obj.ServiceEngineerId = long.Parse(data["ServiceEngineerId"].ToString());
                            obj.ManagerName = data["ManagerName"].ToString();
                            obj.ManagerEmail = data["ManagerEmail"].ToString();
                            obj.StatusText = data["StatusText"].ToString();
                            obj.SuperUserEmail = data["SuperUserEmail"].ToString();
                            obj.SuperUserName = data["SuperUserName"].ToString();
                            obj.SupervisorEmail = data["SupervisorEmail"].ToString();
                            obj.SupervisorName = data["SupervisorName"].ToString();
                            obj.SupervisorUserId = long.Parse(data["SupervisorUserId"].ToString());
                            obj.ReportTypeName = data["ReportTypeName"].ToString();
                            obj.StationName = data["StationName"].ToString();
                            obj.ModelName = data["ModelName"].ToString();
                            obj.SerialNo = data["SerialNo"].ToString();
                        }
                    }
                }
                else
                    obj.message = "0";
                if (obj.message == "1")
                {

                    if (obj.ServiceEngineerEmail != "0")
                    {
                        SendEmailToEngineer(
                                        obj.TicketNumber, obj.CreatedDateStr, obj.ProductName,
            obj.SystemId, obj.AccountName, obj.Location, obj.ProblemDescription,
            obj.ServiceEngineerEmail, obj.CustomerFullName, obj.EngineerFullName
            , obj.StatusText, obj.ReportTypeName, obj.StationName
            , obj.ModelName, obj.SerialNo
                                        );
                    }
                    if (obj.CreatedBy != obj.SystemManagerId)
                    {
                        SendEmailToManager(
                                        obj.TicketNumber, obj.CreatedDateStr, obj.ProductName,
            obj.SystemId, obj.AccountName, obj.Location, obj.ProblemDescription,
            obj.ManagerEmail, obj.CustomerFullName, obj.ManagerName, obj.EngineerFullName
            , obj.StatusText, obj.ReportTypeName, obj.StationName
            , obj.ModelName, obj.SerialNo
                                       );
                    }
                    //          SendEmailToSuperUser(
                    //                            obj.TicketNumber, obj.CreatedDateStr, obj.ProductName,
                    //obj.SystemId, obj.AccountName, obj.Location, obj.ProblemDescription,
                    //obj.SuperUserEmail, obj.CustomerFullName, obj.EngineerFullName
                    //, obj.StatusText, obj.SuperUserName, obj.SupervisorEmail, obj.SupervisorName, obj.ReportTypeName, obj.StationName
                    //  , obj.ModelName, obj.SerialNo
                    //                            );
                    if (obj.CreatedBy != obj.SupervisorUserId)
                    {
                        //Send Email to Customer                    
                        SendEmailToCustomer(
                                        obj.TicketNumber, obj.CreatedDateStr, obj.ProductName,
            obj.SystemId, obj.AccountName, obj.Location, obj.ProblemDescription,
            obj.CustomerEmail, obj.CustomerFullName, obj.EngineerFullName
            , obj.StatusText, obj.ReportTypeName, obj.StationName
            , obj.ModelName, obj.SerialNo
                                    );
                    }
                    SendEmailToSupervisorUser(
                                        obj.TicketNumber, obj.CreatedDateStr, obj.ProductName,
           obj.SystemId, obj.AccountName, obj.Location, obj.ProblemDescription,
           obj.SupervisorEmail, obj.CustomerFullName, obj.EngineerFullName
           , obj.StatusText, obj.SuperUserEmail, obj.SuperUserName, obj.SupervisorName, obj.ReportTypeName, obj.StationName
            , obj.ModelName, obj.SerialNo
                                     );

                }
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }

        private void SendEmailToSuperUser(long TicketNumber, string CreatedDate, string ProductName,
          string SystemId, string AccountName, string Location, string ProblemDescription,
          string SuperUserEmail, string CustomerFullName, string EngineerFullName
          , string StatusText, string SuperUserName, string SupervisorEmail, string SupervisorName, string ReportTypeName, string StationName
            , string ModelName, string SerialNo)
        {
            try
            {
                // Customer Email
                string htmlstr = @"";
                StringBuilder HeaderHtml = new StringBuilder();

                HeaderHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /><title>AHC Helpdesk</title><link href='https://fonts.googleapis.com/css?family=Open+Sans&display=swap' rel='stylesheet'><style type='text/css'>body{font-family:'Open Sans',sans-serif;background:#f1f1f1;color:#0f0f0f;font-size:14px;padding:20px}.pb-15{padding-bottom:15px}.mb-15{margin-bottom:15px}.text-center{text-align:center}.button{padding:8px 12px;background-color:#2cafdd;border-radius:4px;color:#fff;text-decoration:none;display:inline-block;margin-bottom:15px}.button:hover{background-color:#0d96c6;transition:1s all}</style></head><body style='background-color: #f1f1f1; padding: 15px;'><table align='center' style='width: 600px; margin: 0 auto 0 auto;background-color: #fff;padding: 20px 15px;'><tr><td>");
                HeaderHtml.Append("<img src='http://support.arabianhc.com/assets/images/ahc_new_logo.png' class='pb-15' height='44px;'><h4 style='color: #2cafdd'>Dear ");
                HeaderHtml.Append("" + SuperUserName + ",</h4><p style='color:black;'> Thank you for contacting AHC Helpdesk, http://support.arabianhc.com .</p><p style='color:black;'> The service ticket with below information has been generated and your request will be attended shortly.</p><h4>Ticket Information :</h4><div style='width: 70px; height: 2px; background-color: #000;'></div><div style='background-color: #fff; padding-top: 20px; padding-bottom: 15px;'><table><tr>");
                HeaderHtml.Append("<td>Ticket No</td><td>:" + TicketNumber + "</td>");
                HeaderHtml.Append("</tr><tr><td>Ticket Status</td><td>:" + StatusText + "</td>");
                HeaderHtml.Append("<tr><td>Created Date</td><td>: " + CreatedDate + "</td>");
                HeaderHtml.Append("</tr><tr><td>Report Type</td><td>: " + ReportTypeName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Product</td><td>: " + ProductName + "</td>");
                HeaderHtml.Append("</tr><tr><td>System Id</td><td>: " + SystemId + "</td>");
                HeaderHtml.Append("</tr><tr><td>Account</td><td>: " + AccountName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Station Name</td><td>: " + StationName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Model Name</td><td>: " + ModelName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Serial No</td><td>: " + SerialNo + "</td>");
                HeaderHtml.Append("</tr><tr><td>Problem Description</td><td>: " + ProblemDescription + "</td>");
                HeaderHtml.Append("</tr><tr><td>Assigned By</td><td>: " + CustomerFullName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Assigned To</td><td>: " + EngineerFullName + "</td></tr></table><br>");
                //HeaderHtml.Append("<table style='margin-bottom: 10px;'><tr><td> <a href='#' target='_blank' style='text-decoration: none; padding: 8px 12px;border: 1px solid #2cafdd; border-radius: 4px; color: #2cafdd; text-decoration: none;'>Button Click</a></td></tr></table><br><hr><div class='text-center'> <small>please do not hesitate to contact our customer Service support center <strong> +96651111111</strong>,<br> one of our representatives will do their best to assist you.</small></div></td></tr></table></body></html>");
                HeaderHtml.Append("<br><hr><div class='text-center'> <small>Please do not hesitate to contact <code style='font-size: 14px; color:black;'>AHC</code> Customer Service Support Center <strong> 800 2444416</strong>,</small></div></td></tr></table></body></html>");

                htmlstr = HeaderHtml.ToString();
                string Subject = "AHC Helpdesk Support Centre";
                string mailFrom = System.Configuration.ConfigurationManager.AppSettings["mailFrom"].ToString();
                string mailHRBCC = string.Empty;
                //SuperUserEmail = "aqibshahbaz@gmail.com";
                Models.EmailUtility.sendEmail(mailFrom, SuperUserEmail, htmlstr, Subject, mailHRBCC);
            }
            catch (Exception)
            {

            }
        }

        private void SendEmailToSupervisorUser(long TicketNumber, string CreatedDate, string ProductName,
           string SystemId, string AccountName, string Location, string ProblemDescription,
           string SupervisorEmail, string CustomerFullName, string EngineerFullName
           , string StatusText, string SuperUserEmail, string SuperUserName, string SupervisorName, string ReportTypeName, string StationName
            , string ModelName, string SerialNo)
        {
            try
            {
                // Customer Email
                string htmlstr = @"";
                StringBuilder HeaderHtml = new StringBuilder();

                HeaderHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /><title>AHC Helpdesk</title><link href='https://fonts.googleapis.com/css?family=Open+Sans&display=swap' rel='stylesheet'><style type='text/css'>body{font-family:'Open Sans',sans-serif;background:#f1f1f1;color:#0f0f0f;font-size:14px;padding:20px}.pb-15{padding-bottom:15px}.mb-15{margin-bottom:15px}.text-center{text-align:center}.button{padding:8px 12px;background-color:#2cafdd;border-radius:4px;color:#fff;text-decoration:none;display:inline-block;margin-bottom:15px}.button:hover{background-color:#0d96c6;transition:1s all}</style></head><body style='background-color: #f1f1f1; padding: 15px;'><table align='center' style='width: 600px; margin: 0 auto 0 auto;background-color: #fff;padding: 20px 15px;'><tr><td>");
                HeaderHtml.Append("<img src='http://support.arabianhc.com/assets/images/ahc_new_logo.png' class='pb-15' height='44px;'><h4 style='color: #2cafdd'>Dear ");
                HeaderHtml.Append("" + SupervisorName + ",</h4><p style='color:black;'> Thank you for contacting AHC Helpdesk, http://support.arabianhc.com .</p><p style='color:black;'> The service ticket with below information has been generated and your request will be attended shortly.</p><h4>Ticket Information :</h4><div style='width: 70px; height: 2px; background-color: #000;'></div><div style='background-color: #fff; padding-top: 20px; padding-bottom: 15px;'><table><tr>");
                HeaderHtml.Append("<td>Ticket No</td><td>:" + TicketNumber + "</td>");
                HeaderHtml.Append("</tr><tr><td>Ticket Status</td><td>: " + StatusText + "</td>");
                HeaderHtml.Append("<tr><td>Created Date</td><td>: " + CreatedDate + "</td>");
                HeaderHtml.Append("</tr><tr><td>Report Type</td><td>: " + ReportTypeName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Product</td><td>: " + ProductName + "</td>");
                HeaderHtml.Append("</tr><tr><td>System Id</td><td>: " + SystemId + "</td>");
                HeaderHtml.Append("</tr><tr><td>Account</td><td>: " + AccountName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Station Name</td><td>: " + StationName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Model Name</td><td>: " + ModelName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Serial No</td><td>: " + SerialNo + "</td>");
                HeaderHtml.Append("</tr><tr><td>Problem Description</td><td>: " + ProblemDescription + "</td>");
                HeaderHtml.Append("</tr><tr><td>Assigned By</td><td>: " + CustomerFullName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Assigned To</td><td>: " + EngineerFullName + "</td></tr></table><br>");
                //HeaderHtml.Append("<table style='margin-bottom: 10px;'><tr><td> <a href='#' target='_blank' style='text-decoration: none; padding: 8px 12px;border: 1px solid #2cafdd; border-radius: 4px; color: #2cafdd; text-decoration: none;'>Button Click</a></td></tr></table><br><hr><div class='text-center'> <small>please do not hesitate to contact our customer Service support center <strong> +96651111111</strong>,<br> one of our representatives will do their best to assist you.</small></div></td></tr></table></body></html>");
                HeaderHtml.Append("<br><hr><div class='text-center'> <small>Please do not hesitate to contact <code style='font-size: 14px; color:black;'>AHC</code> Customer Service Support Center <strong> 800 2444416</strong>,</small></div></td></tr></table></body></html>");

                htmlstr = HeaderHtml.ToString();
                string Subject = "AHC Helpdesk Support Centre";
                string mailFrom = System.Configuration.ConfigurationManager.AppSettings["mailFrom"].ToString();
                string mailHRBCC = string.Empty;
                //SupervisorEmail = "aqibshahbaz@gmail.com";
                Models.EmailUtility.sendEmail(mailFrom, SupervisorEmail, htmlstr, Subject, mailHRBCC);
            }
            catch (Exception)
            {

            }
        }

        private void SendEmailToEngineer(long TicketNumber, string CreatedDate, string ProductName,
            string SystemId, string AccountName, string Location, string ProblemDescription,
            string ServiceEngineerEmail, string CustomerFullName, string EngineerFullName
            , string StatusText, string ReportTypeName, string StationName
            , string ModelName, string SerialNo)
        {
            try
            {
                // Customer Email
                string htmlstr = @"";
                StringBuilder HeaderHtml = new StringBuilder();

                HeaderHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /><title>AHC Helpdesk</title><link href='https://fonts.googleapis.com/css?family=Open+Sans&display=swap' rel='stylesheet'><style type='text/css'>body{font-family:'Open Sans',sans-serif;background:#f1f1f1;color:#0f0f0f;font-size:14px;padding:20px}.pb-15{padding-bottom:15px}.mb-15{margin-bottom:15px}.text-center{text-align:center}.button{padding:8px 12px;background-color:#2cafdd;border-radius:4px;color:#fff;text-decoration:none;display:inline-block;margin-bottom:15px}.button:hover{background-color:#0d96c6;transition:1s all}</style></head><body style='background-color: #f1f1f1; padding: 15px;'><table align='center' style='width: 600px; margin: 0 auto 0 auto;background-color: #fff;padding: 20px 15px;'><tr><td>");
                HeaderHtml.Append("<img src='http://support.arabianhc.com/assets/images/ahc_new_logo.png' class='pb-15' height='44px;'><h4 style='color: #2cafdd'>Dear ");
                HeaderHtml.Append("" + EngineerFullName + ",</h4><p style='color:black;'> Thank you for contacting AHC Helpdesk, http://support.arabianhc.com .</p><p style='color:black;'> The service ticket with below information has been generated and your request will be attended shortly.</p><h4>Ticket Information :</h4><div style='width: 70px; height: 2px; background-color: #000;'></div><div style='background-color: #fff; padding-top: 20px; padding-bottom: 15px;'><table><tr>");
                HeaderHtml.Append("<td>Ticket No</td><td>:" + TicketNumber + "</td>");
                HeaderHtml.Append("</tr><tr><td>Ticket Status</td><td>:" + StatusText + "</td>");
                HeaderHtml.Append("<tr><td>Created Date</td><td>: " + CreatedDate + "</td>");
                HeaderHtml.Append("</tr><tr><td>Report Type</td><td>: " + ReportTypeName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Product</td><td>: " + ProductName + "</td>");
                HeaderHtml.Append("</tr><tr><td>System Id</td><td>: " + SystemId + "</td>");
                HeaderHtml.Append("</tr><tr><td>Account</td><td>: " + AccountName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Station Name</td><td>: " + StationName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Model Name</td><td>: " + ModelName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Serial No</td><td>: " + SerialNo + "</td>");
                HeaderHtml.Append("</tr><tr><td>Problem Description</td><td>: " + ProblemDescription + "</td>");
                HeaderHtml.Append("</tr><tr><td>Assigned By</td><td>: " + CustomerFullName + "</td></tr></table><br>");
                //HeaderHtml.Append("<table style='margin-bottom: 10px;'><tr><td> <a href='#' target='_blank' style='text-decoration: none; padding: 8px 12px;border: 1px solid #2cafdd; border-radius: 4px; color: #2cafdd; text-decoration: none;'>Button Click</a></td></tr></table><br><hr><div class='text-center'> <small>please do not hesitate to contact our customer Service support center <strong> +96651111111</strong>,<br> one of our representatives will do their best to assist you.</small></div></td></tr></table></body></html>");
                HeaderHtml.Append("<br><hr><div class='text-center'> <small>Please do not hesitate to contact <code style='font-size: 14px; color:black;'>AHC</code> Customer Service Support Center <strong> 800 2444416</strong>,</small></div></td></tr></table></body></html>");

                htmlstr = HeaderHtml.ToString();
                string Subject = "AHC Helpdesk Support Centre";
                string mailFrom = System.Configuration.ConfigurationManager.AppSettings["mailFrom"].ToString();
                string mailHRBCC = string.Empty;
                //ServiceEngineerEmail = "aqibshahbaz@gmail.com";
                Models.EmailUtility.sendEmail(mailFrom, ServiceEngineerEmail, htmlstr, Subject, mailHRBCC);
            }
            catch (Exception)
            {

            }
        }

        private void SendEmailToCustomer(long TicketNumber, string CreatedDate, string ProductName,
            string SystemId, string AccountName, string Location, string ProblemDescription,
            string CustomerEmail, string CustomerFullName, string EngineerFullName
            , string StatusText, string ReportTypeName, string StationName
            , string ModelName, string SerialNo)
        {
            try
            {
                // Customer Email
                string htmlstr = @"";
                StringBuilder HeaderHtml = new StringBuilder();

                HeaderHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /><title>AHC Helpdesk</title><link href='https://fonts.googleapis.com/css?family=Open+Sans&display=swap' rel='stylesheet'><style type='text/css'>body{font-family:'Open Sans',sans-serif;background:#f1f1f1;color:#0f0f0f;font-size:14px;padding:20px}.pb-15{padding-bottom:15px}.mb-15{margin-bottom:15px}.text-center{text-align:center}.button{padding:8px 12px;background-color:#2cafdd;border-radius:4px;color:#fff;text-decoration:none;display:inline-block;margin-bottom:15px}.button:hover{background-color:#0d96c6;transition:1s all}</style></head><body style='background-color: #f1f1f1; padding: 15px;'><table align='center' style='width: 600px; margin: 0 auto 0 auto;background-color: #fff;padding: 20px 15px;'><tr><td>");
                HeaderHtml.Append("<img src='http://support.arabianhc.com/assets/images/ahc_new_logo.png' class='pb-15' height='44px;'><h4 style='color: #2cafdd'>Dear ");
                //HeaderHtml.Append("" + CustomerFullName + ",</h4><p> This is an automated message from the AHC Helpdesk System to confirm that this ticket has been created. Our aim to continuously provide a fast and efficient service.</p><h4>Ticket Information :</h4><div style='width: 70px; height: 2px; background-color: #000;'></div><div style='background-color: #fff; padding-top: 20px; padding-bottom: 15px;'><table><tr>");
                //HeaderHtml.Append("<td>Ticket No</td><td>:" + TicketNumber + "</td>");
                HeaderHtml.Append("" + CustomerFullName + ",</h4><p style='color:black;'> Thank you for contacting AHC Helpdesk, http://support.arabianhc.com .</p><p style='color:black;'> The service ticket with below information has been generated and your request will be attended shortly.</p><h4>Ticket Information :</h4><div style='width: 70px; height: 2px; background-color: #000;'></div><div style='background-color: #fff; padding-top: 20px; padding-bottom: 15px;'><table><tr>");
                HeaderHtml.Append("<td>Ticket No</td><td>:" + TicketNumber + "</td>");
                HeaderHtml.Append("</tr><tr><td>Ticket Status</td><td>:" + StatusText + "</td>");
                HeaderHtml.Append("</tr><tr><td>Created Date</td><td>: " + CreatedDate + "</td>");
                HeaderHtml.Append("</tr><tr><td>Report Type</td><td>: " + ReportTypeName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Product</td><td>: " + ProductName + "</td>");
                HeaderHtml.Append("</tr><tr><td>System Id</td><td>: " + SystemId + "</td>");
                HeaderHtml.Append("</tr><tr><td>Account</td><td>: " + AccountName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Station Name</td><td>: " + StationName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Model Name</td><td>: " + ModelName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Serial No</td><td>: " + SerialNo + "</td>");
                HeaderHtml.Append("</tr><tr><td>Problem Description</td><td>: " + ProblemDescription + "</td>");
                HeaderHtml.Append("</tr><tr><td>Assigned To</td><td>: " + EngineerFullName + "</td></tr></table><br>");
                //HeaderHtml.Append("<table style='margin-bottom: 10px;'><tr><td> <a href='#' target='_blank' style='text-decoration: none; padding: 8px 12px;border: 1px solid #2cafdd; border-radius: 4px; color: #2cafdd; text-decoration: none;'>Button Click</a></td></tr></table><br><hr><div class='text-center'> <small>please do not hesitate to contact our customer Service support center <strong> +966511111111</strong>,<br> one of our representatives will do their best to assist you.</small></div></td></tr></table></body></html>");
                HeaderHtml.Append("<br><hr><div class='text-center'> <small>Please do not hesitate to contact <code style='font-size: 14px; color:black;'>AHC</code> Customer Service Support Center <strong> 800 2444416</strong>,</small></div></td></tr></table></body></html>");

                htmlstr = HeaderHtml.ToString();
                string Subject = "AHC Helpdesk Support Centre";
                string mailFrom = System.Configuration.ConfigurationManager.AppSettings["mailFrom"].ToString();
                string mailHRBCC = string.Empty;
                //CustomerEmail = "aqibshahbaz@gmail.com";
                Models.EmailUtility.sendEmail(mailFrom, CustomerEmail, htmlstr, Subject, mailHRBCC);
            }
            catch (Exception)
            {

            }
        }

        private void SendEmailToManager(long TicketNumber, string CreatedDate, string ProductName,
            string SystemId, string AccountName, string Location, string ProblemDescription,
            string ManagerEmail, string CustomerFullName, string ManagerFullName, string EngineerFullName
            , string StatusText, string ReportTypeName, string StationName
            , string ModelName, string SerialNo)
        {
            try
            {
                // Customer Email
                string htmlstr = @"";
                StringBuilder HeaderHtml = new StringBuilder();

                HeaderHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /><title>AHC Helpdesk</title><link href='https://fonts.googleapis.com/css?family=Open+Sans&display=swap' rel='stylesheet'><style type='text/css'>body{font-family:'Open Sans',sans-serif;background:#f1f1f1;color:#0f0f0f;font-size:14px;padding:20px}.pb-15{padding-bottom:15px}.mb-15{margin-bottom:15px}.text-center{text-align:center}.button{padding:8px 12px;background-color:#2cafdd;border-radius:4px;color:#fff;text-decoration:none;display:inline-block;margin-bottom:15px}.button:hover{background-color:#0d96c6;transition:1s all}</style></head><body style='background-color: #f1f1f1; padding: 15px;'><table align='center' style='width: 600px; margin: 0 auto 0 auto;background-color: #fff;padding: 20px 15px;'><tr><td>");
                HeaderHtml.Append("<img src='http://support.arabianhc.com/assets/images/ahc_new_logo.png' class='pb-15' height='44px;'><h4 style='color: #2cafdd'>Dear ");
                HeaderHtml.Append("" + ManagerFullName + ",</h4><p style='color:black;'> Thank you for contacting AHC Helpdesk, http://support.arabianhc.com .</p><p style='color:black;'> The service ticket with below information has been generated and your request will be attended shortly.</p><h4>Ticket Information :</h4><div style='width: 70px; height: 2px; background-color: #000;'></div><div style='background-color: #fff; padding-top: 20px; padding-bottom: 15px;'><table><tr>");
                HeaderHtml.Append("<td>Ticket No</td><td>:" + TicketNumber + "</td>");
                HeaderHtml.Append("</tr><tr><td>Ticket Status</td><td>:" + StatusText + "</td>");
                HeaderHtml.Append("</tr><tr><td>Created Date</td><td>: " + CreatedDate + "</td>");
                HeaderHtml.Append("</tr><tr><td>Report Type</td><td>: " + ReportTypeName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Product</td><td>: " + ProductName + "</td>");
                HeaderHtml.Append("</tr><tr><td>System Id</td><td>: " + SystemId + "</td>");
                HeaderHtml.Append("</tr><tr><td>Account</td><td>: " + AccountName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Station Name</td><td>: " + StationName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Model Name</td><td>: " + ModelName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Serial No</td><td>: " + SerialNo + "</td>");
                HeaderHtml.Append("</tr><tr><td>Problem Description</td><td>: " + ProblemDescription + "</td>");
                HeaderHtml.Append("</tr><tr><td>Assigned To</td><td>: " + EngineerFullName + "</td></tr></table><br>");
                //HeaderHtml.Append("<table style='margin-bottom: 10px;'><tr><td> <a href='#' target='_blank' style='text-decoration: none; padding: 8px 12px;border: 1px solid #2cafdd; border-radius: 4px; color: #2cafdd; text-decoration: none;'>Button Click</a></td></tr></table><br><hr><div class='text-center'> <small>please do not hesitate to contact our customer Service support center <strong> +966511111111</strong>,<br> one of our representatives will do their best to assist you.</small></div></td></tr></table></body></html>");
                HeaderHtml.Append("<br><hr><div class='text-center'> <small>Please do not hesitate to contact <code style='font-size: 14px; color:black;'>AHC</code> Customer Service Support Center <strong> 800 2444416</strong>,</small></div></td></tr></table></body></html>");

                htmlstr = HeaderHtml.ToString();
                string Subject = "AHC Helpdesk Support Centre";
                string mailFrom = System.Configuration.ConfigurationManager.AppSettings["mailFrom"].ToString();
                string mailHRBCC = string.Empty;
                //ManagerEmail = "aqibshahbaz@gmail.com";
                Models.EmailUtility.sendEmail(mailFrom, ManagerEmail, htmlstr, Subject, mailHRBCC);
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
                        if (obj.message == "1")
                        {
                            obj.TicketNumber = Convert.ToInt64(data["TicketNumber"].ToString());
                            obj.CreatedDateStr = data["CreatedDateStr"].ToString();
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
                            obj.ServiceEngineerId = long.Parse(data["ServiceEngineerId"].ToString());
                            obj.ManagerName = data["ManagerName"].ToString();
                            obj.ManagerEmail = data["ManagerEmail"].ToString();
                            obj.StatusText = data["StatusText"].ToString();
                            obj.SuperUserEmail = data["SuperUserEmail"].ToString();
                            obj.SuperUserName = data["SuperUserName"].ToString();
                            obj.SupervisorEmail = data["SupervisorEmail"].ToString();
                            obj.SupervisorName = data["SupervisorName"].ToString();
                            obj.SupervisorUserId = long.Parse(data["SupervisorUserId"].ToString());
                            obj.ReportTypeName = data["ReportTypeName"].ToString();
                            obj.StationName = data["StationName"].ToString();
                            obj.ModelName = data["ModelName"].ToString();
                            obj.SerialNo = data["SerialNo"].ToString();
                        }
                    }
                }
                else
                    obj.message = "0";

                if (obj.message == "1")
                {
                    SendEmailToCustomer(
                                        obj.TicketNumber, obj.CreatedDateStr, obj.ProductName,
            obj.SystemId, obj.AccountName, obj.Location, obj.ProblemDescription,
            obj.CustomerEmail, obj.CustomerFullName, obj.EngineerFullName
            , obj.StatusText, obj.ReportTypeName, obj.StationName
            , obj.ModelName, obj.SerialNo
                                    );
                    if (obj.ServiceEngineerEmail != "0")
                    {
                        SendEmailToEngineer(
                                         obj.TicketNumber, obj.CreatedDateStr, obj.ProductName,
             obj.SystemId, obj.AccountName, obj.Location, obj.ProblemDescription,
             obj.ServiceEngineerEmail, obj.CustomerFullName, obj.EngineerFullName
             , obj.StatusText, obj.ReportTypeName, obj.StationName
             , obj.ModelName, obj.SerialNo
                                         );
                    }
                    SendEmailToSupervisorUser(
                                          obj.TicketNumber, obj.CreatedDateStr, obj.ProductName,
             obj.SystemId, obj.AccountName, obj.Location, obj.ProblemDescription,
             obj.SupervisorEmail, obj.CustomerFullName, obj.EngineerFullName
             , obj.StatusText, obj.SuperUserEmail, obj.SuperUserName, obj.SupervisorName, obj.ReportTypeName, obj.StationName
              , obj.ModelName, obj.SerialNo
                                       );
                }
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }
        public TicketDTO TicketTransfer(TicketDTO obj)
        {
            try
            {
                var data = model.TicketTransfer(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.message = data["message"].ToString();
                        if (obj.message == "1")
                        {
                            obj.TicketNumber = Convert.ToInt64(data["TicketNumber"].ToString());
                            obj.CreatedDateStr = data["CreatedDateStr"].ToString();
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
                            obj.ServiceEngineerId = long.Parse(data["ServiceEngineerId"].ToString());
                            obj.ManagerName = data["ManagerName"].ToString();
                            obj.ManagerEmail = data["ManagerEmail"].ToString();
                            obj.StatusText = data["StatusText"].ToString();
                            obj.SuperUserEmail = data["SuperUserEmail"].ToString();
                            obj.SuperUserName = data["SuperUserName"].ToString();
                            obj.SupervisorEmail = data["SupervisorEmail"].ToString();
                            obj.SupervisorName = data["SupervisorName"].ToString();
                            obj.SupervisorUserId = long.Parse(data["SupervisorUserId"].ToString());
                            obj.ReportTypeName = data["ReportTypeName"].ToString();
                            obj.StationName = data["StationName"].ToString();
                            obj.ModelName = data["ModelName"].ToString();
                            obj.SerialNo = data["SerialNo"].ToString();
                        }
                    }
                }
                else
                    obj.message = "0";

                if (obj.message == "1")
                {
                    SendEmailToCustomer(
                                        obj.TicketNumber, obj.CreatedDateStr, obj.ProductName,
            obj.SystemId, obj.AccountName, obj.Location, obj.ProblemDescription,
            obj.CustomerEmail, obj.CustomerFullName, obj.EngineerFullName
            , obj.StatusText, obj.ReportTypeName, obj.StationName
            , obj.ModelName, obj.SerialNo
                                    );
                    if (obj.ServiceEngineerEmail != "0")
                    {
                        SendEmailToEngineer(
                                         obj.TicketNumber, obj.CreatedDateStr, obj.ProductName,
             obj.SystemId, obj.AccountName, obj.Location, obj.ProblemDescription,
             obj.ServiceEngineerEmail, obj.CustomerFullName, obj.EngineerFullName
             , obj.StatusText, obj.ReportTypeName, obj.StationName
             , obj.ModelName, obj.SerialNo
                                         );
                    }
                    SendEmailToSupervisorUser(
                                          obj.TicketNumber, obj.CreatedDateStr, obj.ProductName,
             obj.SystemId, obj.AccountName, obj.Location, obj.ProblemDescription,
             obj.SupervisorEmail, obj.CustomerFullName, obj.EngineerFullName
             , obj.StatusText, obj.SuperUserEmail, obj.SuperUserName, obj.SupervisorName, obj.ReportTypeName, obj.StationName
              , obj.ModelName, obj.SerialNo
                                       );
                }
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
        public TicketDTO GetSystemUserTicketsMobile(TicketDTO obj)
        {
            obj.datasetxml = model.GetSystemUserTicketsMobile(obj);
            return obj;
        }


        public TicketDTO GetRejectedTickets(TicketDTO obj)
        {
            obj.datasetxml = model.GetRejectedTickets(obj);
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
        public TicketDTO GetSystemManagerId(TicketDTO obj)
        {
            try
            {
                var data = model.GetSystemManagerId(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.SystemManagerId = int.Parse(data["UserId"].ToString());
                    }
                }
                else
                    obj.SystemManagerId = 0;
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }

        public TicketDTO GetServiceEngineerCounts(TicketDTO obj)
        {
            obj.datasetxml = model.GetServiceEngineerCounts(obj);
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
        public TicketDTO GetTicketRatingList(TicketDTO obj)
        {
            obj.datasetxml = model.TicketRatingList(obj);
            return obj;
        }

        public TicketDTO AddTicketRating(TicketDTO obj)
        {
            try
            {
                var data = model.AddticketRating(obj);
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


        public TicketDTO CrmRawData(TicketDTO obj)
        {
            try
            {
                var data = model.CrmRawData(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.message = data["JsonData"].ToString();
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
        public TicketDTO TicketServiceTab(TicketDTO obj)
        {
            try
            {
                var data = model.ServiceArchiveTicket(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.message = data["JsonData"].ToString();
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

        public IEnumerable<TicketDTO> AssetListReport(TicketDTO obj)
        {
            var data = model.AssetListReport(obj);
            var list = CustomDataReaderToGenericExtension.GetDataObjects<TicketDTO>(data);
            data.Close();
            return list;
        }
        public IEnumerable<TicketDTO> ProductReport(TicketDTO obj)
        {
            var data = model.ProductReport(obj);
            var list = CustomDataReaderToGenericExtension.GetDataObjects<TicketDTO>(data);
            data.Close();
            return list;
        }
        public IEnumerable<TicketDTO> EngineerWiseStatusReport(TicketDTO obj)
        {
            var data = model.EngineerWiseStatusReport(obj);
            var list = CustomDataReaderToGenericExtension.GetDataObjects<TicketDTO>(data);
            data.Close();
            return list;
        }
        public IEnumerable<TicketDTO> AccountTicketReport(TicketDTO obj)
        {
            var data = model.AccountTicketReport(obj);
            var list = CustomDataReaderToGenericExtension.GetDataObjects<TicketDTO>(data);
            data.Close();
            return list;
        }
        public IEnumerable<TicketDTO> PerMonthStatus(TicketDTO obj)
        {
            var data = model.PerMonthStatus(obj);
            var list = CustomDataReaderToGenericExtension.GetDataObjects<TicketDTO>(data);
            data.Close();
            return list;
        }
        public IEnumerable<TicketDTO> RepeatedErrorReport(TicketDTO obj)
        {
            var data = model.RepeatedErrorReport(obj);
            var list = CustomDataReaderToGenericExtension.GetDataObjects<TicketDTO>(data);
            data.Close();
            return list;
        }
        public IEnumerable<TicketDTO> SparePartTicketsCountReport(TicketDTO obj)
        {
            var data = model.SparePartTicketsCountReport(obj);
            var list = CustomDataReaderToGenericExtension.GetDataObjects<TicketDTO>(data);
            data.Close();
            return list;
        }

    }

    public interface ITicketService
    {
        TicketDTO InsertTicketRequest(TicketDTO obj);
        TicketDTO GetUnderApprovalTickets(TicketDTO obj);
        TicketDTO UpdateTicketStatus(TicketDTO obj);
        TicketDTO TicketTransfer(TicketDTO obj);
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
        TicketDTO GetSystemUserTicketsMobile(TicketDTO obj);
        TicketDTO GetRejectedTickets(TicketDTO obj);
        TicketDTO GetServiceEngineerTickets(TicketDTO obj);
        TicketDTO GetDashboardCount(TicketDTO obj);

        TicketDTO GetSystemManagerId(TicketDTO obj);
        TicketDTO GetServiceEngineerCounts(TicketDTO obj);
        TicketDTO GetSparePartRequestTickets(TicketDTO obj);
        TicketDTO GetEnquiryDetails(TicketDTO obj);
        TicketDTO GetTicketRatingList(TicketDTO obj);
        TicketDTO AddTicketRating(TicketDTO obj);
        TicketDTO GetEnquiryList(TicketDTO obj);
        TicketDTO GetServiceEngineerTicketsFiletrs(TicketDTO obj);

        TicketDTO GetTicketDetails(TicketDTO obj);
        TicketDTO GetSparePartListById(TicketDTO obj);
        TicketDTO CrmRawData(TicketDTO obj);
        TicketDTO TicketServiceTab(TicketDTO obj);
        IEnumerable<TicketDTO> AssetListReport(TicketDTO obj);
        IEnumerable<TicketDTO> ProductReport(TicketDTO obj);
        IEnumerable<TicketDTO> EngineerWiseStatusReport(TicketDTO obj);
        IEnumerable<TicketDTO> AccountTicketReport(TicketDTO obj);
        IEnumerable<TicketDTO> PerMonthStatus(TicketDTO obj);
        IEnumerable<TicketDTO> RepeatedErrorReport(TicketDTO obj);
        IEnumerable<TicketDTO> SparePartTicketsCountReport(TicketDTO obj);

    }
}