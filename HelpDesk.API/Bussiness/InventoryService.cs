using HelpDesk.API.DataAccess;
using HelpDesk.API.DTO_s;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HelpDesk.API.Bussiness
{
    public class InventoryService: IInventoryService
    {
        private readonly IInventoryModel model;
        public InventoryService(InventoryModel _model)
        {
            model = _model;
        }

        public InventoryDTO CheckSparePartName(InventoryDTO obj)
        {
            try
            {
                var data = model.CheckSparePartName(obj);
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
        public InventoryDTO ConsignmentStatus(InventoryDTO obj)
        {
            try
            {
                var data = model.ConsignmentStatus(obj);
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

        

        public InventoryDTO InsertUpdateSparePart(InventoryDTO obj)
        {
            try
            {
                var data = model.InsertUpdateSparePart(obj);
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
        public InventoryDTO InsertUpdateConsignment(InventoryDTO obj)
        {
            try
            {
                var data = model.InsertUpdateConsginment(obj);
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

        public InventoryDTO InsertBulkTransfer(InventoryDTO obj)
        {
            try
            {
                var data = model.InsertBulkTransfer(obj);
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
        
        public InventoryDTO UpdateSparePart(InventoryDTO obj)
        {
            try
            {
                var data = model.UpdateSparePart(obj);
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
        public InventoryDTO AddMainEnquiry(InventoryDTO obj)
        {
            try
            {
                var data = model.addmainenquiry(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.message = data["message"].ToString();
                        obj.EmailJson = data["EmailJson"].ToString();
                    }
                }
                else
                    obj.message = "0";
                if (obj.message == "1")
                {
                    var model = JsonConvert.DeserializeObject<List<InventoryDTO>>(obj.EmailJson);
                    obj.EmailList = model;
                    foreach (var item in obj.EmailList)
                    {
                        SendEmailToSupervisorUser(obj.EnquiryType, obj.CompanyName, obj.ProductName, item.Email, obj.CustomerName, obj.CustomerEmail, obj.CustomerPhone, obj.Enquiry, obj.FullName);
                        SendEmailToEnquiryCustomer(obj.EnquiryType, obj.CompanyName, obj.ProductName, item.Email, obj.CustomerName, obj.CustomerEmail, obj.CustomerPhone, obj.Enquiry, obj.FullName);
                    }
                    //SendEmailToSupervisorUser()
                }
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }
        private void SendEmailToSupervisorUser(string EnquiryType, string Company, string ProductName,
        string SupervisorEmail, string CustomerFullName, string CustomerEmail
        , string PhoneNumber, string Enquiry,string SupervisorName)
        {
            try
            {
                // Customer Email
                string htmlstr = @"";
                StringBuilder HeaderHtml = new StringBuilder();

                HeaderHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /><title>AHC Helpdesk</title><link href='https://fonts.googleapis.com/css?family=Open+Sans&display=swap' rel='stylesheet'><style type='text/css'>body{font-family:'Open Sans',sans-serif;background:#f1f1f1;color:#0f0f0f;font-size:14px;padding:20px}.pb-15{padding-bottom:15px}.mb-15{margin-bottom:15px}.text-center{text-align:center}.button{padding:8px 12px;background-color:#2cafdd;border-radius:4px;color:#fff;text-decoration:none;display:inline-block;margin-bottom:15px}.button:hover{background-color:#0d96c6;transition:1s all}</style></head><body style='background-color: #f1f1f1; padding: 15px;'><table align='center' style='width: 600px; margin: 0 auto 0 auto;background-color: #fff;padding: 20px 15px;'><tr><td>");
                HeaderHtml.Append("<img src='http://support.arabianhc.com/assets/images/ahc_new_logo.png' class='pb-15' height='44px;'><h4 style='color: #2cafdd'>Dear ");
                HeaderHtml.Append("" + SupervisorName + ",</h4><p style='color:black;'> Thank you for contacting AHC Helpdesk, http://support.arabianhc.com .</p><p style='color:black;'> </p><h4>Enquiry Information :</h4><div style='width: 70px; height: 2px; background-color: #000;'></div><div style='background-color: #fff; padding-top: 20px; padding-bottom: 15px;'><table><tr>");
                HeaderHtml.Append("<td>Enquiry Type</td><td>:" + EnquiryType + "</td>");
                HeaderHtml.Append("</tr><tr><td>Company/Hospital Name</td><td>: " + Company + "</td>");
                HeaderHtml.Append("<tr><td>Product Name</td><td>: " + ProductName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Full Name</td><td>: " + CustomerFullName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Email</td><td>: " + CustomerEmail + "</td>");
                HeaderHtml.Append("</tr><tr><td>Phone Number</td><td>: " + PhoneNumber + "</td>");
                HeaderHtml.Append("</tr><tr><td>Enquiry</td><td>: " + Enquiry + "</td></tr></table>");
                HeaderHtml.Append("<br><hr><div class='text-center'> <small>Please do not hesitate to contact <code style='font-size: 14px; color:black;'>AHC</code> Customer Service Support Center <strong> 800 2444416</strong>,</small></div></body></html>");
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

        private void SendEmailToEnquiryCustomer(string EnquiryType, string Company, string ProductName,
        string SupervisorEmail, string CustomerFullName, string CustomerEmail
        , string PhoneNumber, string Enquiry, string SupervisorName)
        {
            try
            {
                // Customer Email
                string htmlstr = @"";
                StringBuilder HeaderHtml = new StringBuilder();

                HeaderHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /><title>AHC Helpdesk</title><link href='https://fonts.googleapis.com/css?family=Open+Sans&display=swap' rel='stylesheet'>");
                HeaderHtml.Append("<style type='text/css'>body{font-family:'Open Sans',sans-serif;background:#f1f1f1;color:#0f0f0f;font-size:14px;padding:20px}.pb-15{padding-bottom:15px}.mb-15{margin-bottom:15px}.text-center{text-align:center}.button{padding:8px 12px;background-color:#2cafdd;border-radius:4px;color:#fff;text-decoration:none;display:inline-block;margin-bottom:15px}.button:hover{background-color:#0d96c6;transition:1s all}.table{width: 100%}.table td{padding: 4px 0px;}</style>");
                HeaderHtml.Append("</head><body style='background-color: #f1f1f1; padding: 15px;'>");
                HeaderHtml.Append("<table align='center' style='width: 600px; margin: 0 auto 0 auto;background-color: #fff;padding: 20px 15px;'><tr><td><img src='http://support.arabianhc.com/assets/images/ahc_new_logo.png' class='pb-15' height='44px;'>");
                HeaderHtml.Append("<h4 style='color: #2cafdd'>Dear "+CustomerFullName+" ,</h4>");
                HeaderHtml.Append("<p style='color:black;'> Thank you for contacting AHC Helpdesk, http://support.arabianhc.com .</p>");
                HeaderHtml.Append("<p style='color:black;'> </p>");
                HeaderHtml.Append("<p>The Enquiry with the below information has been generated and your request will be attended shortly.</p>");
                HeaderHtml.Append("<div style='width: 70px; height: 2px; background-color: #000;'></div>");
                HeaderHtml.Append("<div style='background-color: #fff; padding-top: 20px; padding-bottom: 15px;'>");
                HeaderHtml.Append("<table class='table'><tr>");
                HeaderHtml.Append("<td>Enquiry Type</td><td>:" + EnquiryType + "</td>");
                HeaderHtml.Append("</tr><tr><td>Company/Hospital Name</td><td>: " + Company + "</td>");
                HeaderHtml.Append("<tr><td>Product Name</td><td>: " + ProductName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Full Name</td><td>: " + CustomerFullName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Email</td><td>: " + CustomerEmail + "</td>");
                HeaderHtml.Append("</tr><tr><td>Phone Number</td><td>: " + PhoneNumber + "</td>");
                HeaderHtml.Append("</tr><tr><td>Enquiry</td><td>: " + Enquiry + "</td></tr></table>");
                HeaderHtml.Append("<br><hr><div class='text-center'> <small>Please do not hesitate to contact <code style='font-size: 14px; color:black;'>AHC</code> Customer Service Support Center <strong> 800 2444416</strong>,</small></div></body></html>");
                htmlstr = HeaderHtml.ToString();

                string Subject = "AHC Helpdesk Support Centre";
                string mailFrom = System.Configuration.ConfigurationManager.AppSettings["mailFrom"].ToString();
                string mailHRBCC = string.Empty;
                Models.EmailUtility.sendEmail(mailFrom, CustomerEmail, htmlstr, Subject, mailHRBCC);
            }
            catch (Exception)
            {

            }
        }




        public InventoryDTO StockChange(InventoryDTO obj)
        {
            try
            {
                var data = model.stockchnage(obj);
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
        public InventoryDTO TrasnferQuantity(InventoryDTO obj)
        {
            try
            {
                var data = model.transferquantity(obj);
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
        


        public InventoryDTO SparePartById(InventoryDTO obj)
        {

            obj.datasetxml = model.SparePartById(obj);
            return obj;

            //var data = model.SparePartById(vm);
            //if (data.HasRows)
            //{
            //    while (data.Read())
            //    {
            //        vm.SparePartId = long.Parse(data["SparePartId"].ToString());
            //        vm.ProductId = int.Parse(data["ProductId"].ToString());
            //        vm.SparePartName =data["SparePartName"].ToString();
            //        vm.SparePartNumber = data["SparePartNumber"].ToString();
            //        vm.Quantity = int.Parse(data["Quantity"].ToString());
            //        vm.BaseQuantity = int.Parse(data["BaseQuantity"].ToString());
            //        vm.Price = data["Price"].ToString();
            //        vm.ConsignmentsJson = data["ConsignmentsJson"].ToString();
            //        vm.WarehouseJson = data["WarehouseJson"].ToString();
            //    }
            //    data.Close();
            //}
            //return vm;
        }
        
        public InventoryDTO GetMainEnquiry(InventoryDTO obj)
        {

            obj.datasetxml = model.GetMainEnquiry(obj);
            return obj;

        }
        public InventoryDTO SparePartByIdSP(InventoryDTO obj)
        {

            obj.datasetxml = model.SparePartByIdSP(obj);
            return obj;
           
        }
        public InventoryDTO SparePartByIdEdit(InventoryDTO obj)
        {

            obj.datasetxml = model.SparePartByIdSEdit(obj);
            return obj;

        }
        
        public InventoryDTO SparePartList(InventoryDTO obj)
        {
            obj.datasetxml = model.SparePartList(obj);
            return obj;
        }
        public InventoryDTO WarehouseBySparePart(InventoryDTO obj)
        {
            obj.datasetxml = model.WarehouseBySparePart(obj);
            return obj;
        }
        public InventoryDTO WarehouseStockById(InventoryDTO obj)
        {
            obj.datasetxml = model.WarehouseStockDetailsById(obj);
            return obj;
        }
        
        public InventoryDTO SparePartListByWHId(InventoryDTO obj)
        {
            obj.datasetxml = model.SparePartListByWHId(obj);
            return obj;
        }
        
        public InventoryDTO ConsignmentList(InventoryDTO obj)
        {
            obj.datasetxml = model.ConsignmentList(obj);
            return obj;
        }
        
        public InventoryDTO Warehouseddl(InventoryDTO obj)
        {
            obj.datasetxml = model.Warehouseddl(obj);
            return obj;
        }
    }

    public interface IInventoryService
    {
        InventoryDTO InsertUpdateSparePart(InventoryDTO obj);
        InventoryDTO InsertUpdateConsignment(InventoryDTO obj);
        InventoryDTO InsertBulkTransfer(InventoryDTO obj);
        InventoryDTO UpdateSparePart(InventoryDTO obj);
        InventoryDTO AddMainEnquiry(InventoryDTO obj);
        InventoryDTO StockChange(InventoryDTO obj);
        InventoryDTO TrasnferQuantity(InventoryDTO obj);
        InventoryDTO CheckSparePartName(InventoryDTO obj);
        InventoryDTO ConsignmentStatus(InventoryDTO obj);
        InventoryDTO SparePartList(InventoryDTO obj);
        InventoryDTO WarehouseBySparePart(InventoryDTO obj);
        InventoryDTO WarehouseStockById(InventoryDTO obj);
        InventoryDTO SparePartListByWHId(InventoryDTO obj);
        InventoryDTO ConsignmentList(InventoryDTO obj);
        InventoryDTO SparePartById(InventoryDTO obj);
        InventoryDTO SparePartByIdSP(InventoryDTO obj);
        InventoryDTO GetMainEnquiry(InventoryDTO obj);
        InventoryDTO SparePartByIdEdit(InventoryDTO obj);
        InventoryDTO Warehouseddl(InventoryDTO obj);
    }
}