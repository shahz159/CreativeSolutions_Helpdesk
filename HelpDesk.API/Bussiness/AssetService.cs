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
    public class AssetService : IAssetService
    {
        string WebURLPath = System.Configuration.ConfigurationManager.AppSettings["weburl"];
        private readonly IAssetModel model;
        public AssetService(AssetModel _model)
        {
            model = _model;
        }
        #region CURD Operations Services of Asset Management
        public AssetDTO GetAssetById(AssetDTO obj)
        {
            try
            {
                var data = model.GetAssetDetailsById(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.AccountId = int.Parse(data["AccountId"].ToString());
                        obj.AccountName = data["AccountName"].ToString();
                        obj.AccountCode = data["AccountCode"].ToString();
                        obj.ProductId = int.Parse(data["ProductId"].ToString());
                        obj.ProductName = data["ProductName"].ToString();
                        obj.ProductCode = data["ProductCode"].ToString();
                        //obj.ModelId = int.Parse(data["ModelId"].ToString());
                        //obj.ModelName = data["ModelName"].ToString();
                        obj.StationName = data["StationName"].ToString();
                        //obj.IPAddress = data["IPAddress"].ToString();
                        //obj.SerialNo = data["SerialNo"].ToString();
                        //obj.Configuration = data["Configuration"].ToString();
                        obj.Area = data["Area"].ToString();
                        obj.RegionName = data["RegionName"].ToString();
                        obj.RegionId = int.Parse(data["RegionId"].ToString());
                        obj.CityId = int.Parse(data["CityId"].ToString());
                        obj.CityName = data["CityName"].ToString();
                        obj.InstallationDate = DateTime.Parse(data["InstallationDate"].ToString());
                        obj.IsContract = bool.Parse(data["IsContract"].ToString());
                        obj.POContract = data["POContract"].ToString();
                        obj.WarrantyExpiryDate = DateTime.Parse(data["WarrantyExpiryDate"].ToString());
                        obj.PPMType = int.Parse(data["PPMType"].ToString());
                        obj.SystemNo = data["SystemNo"].ToString();
                        obj.IsApproved = bool.Parse(data["IsApproved"].ToString());
                        obj.IsRejected = bool.Parse(data["IsRejected"].ToString());
                        obj.ApprovedBy = long.Parse(data["ApprovedBy"].ToString());
                        obj.isActive = bool.Parse(data["isActive"].ToString());
                        obj.CreatedOn = DateTime.Parse(data["CreatedOn"].ToString());
                        obj.CreatedBy = long.Parse(data["CreatedBy"].ToString());
                        obj.FullName = data["FullName"].ToString();
                        obj.AMId = int.Parse(data["AMId"].ToString());
                        obj.PPMJson = data["PPMJson"].ToString();
                        obj.AssetModelJson = data["AMModelsJson"].ToString();
                        obj.ModelJson = data["ModelsJson"].ToString();
                        obj.ProductJson = data["ProductJson"].ToString();
                        obj.RegionJson = data["RegionJson"].ToString();
                        obj.CityJson = data["CityJson"].ToString();
                        obj.UpdatedJson = data["UpdatedAMJson"].ToString();
                        obj.UpdatedAMId = long.Parse(data["UpdatedAMId"].ToString());
                        //obj.TicketNumber = long.Parse(data["TicketNumber"].ToString());
                        obj.EditMode = bool.Parse(data["EditMode"].ToString());
                        obj.ContractType = int.Parse(data["ContractType"].ToString());
                        obj.ContractTypetxt = data["ContractTypetxt"].ToString();
                        obj.UpdateUserName = data["UpdateUserName"].ToString();
                        obj.ModifiedOn = DateTime.Parse(data["ModifiedOn"].ToString());
                        obj.RemainingModelsJson = data["RemainingModelsJson"].ToString();
                        obj.TotalCanister = int.Parse(data["TotalCanister"].ToString());
                        obj.JVMOrdersJson = data["JVMOrdersJson"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }
        //public IEnumerable<AssetDTO> GetAssetList(AssetDTO obj)
        //{
        //    var data = model.GetAssetList(obj);
        //    var list = CustomDataReaderToGenericExtension.GetDataObjects<AssetDTO>(data);
        //    data.Close();
        //    return list;
        //}

        public AssetDTO GetAssetList(AssetDTO obj)
        {
            obj.datasetxml = model.GetAssetList(obj);
            return obj;
        }
        public IEnumerable<AssetDTO> GetCity(AssetDTO obj)
        {
            var data = model.GetCity(obj);
            var list = CustomDataReaderToGenericExtension.GetDataObjects<AssetDTO>(data);
            data.Close();
            return list;
        }
        public AssetDTO GetDropDowns(AssetDTO obj)
        {
            obj.datasetxml = model.GetDropDownList(obj);
            return obj;
        }
        public AssetDTO GetApprovalAssets(AssetDTO obj)
        {
            obj.datasetxml = model.GetApprovalAssets(obj);
            return obj;
        }
        public AssetDTO GetPPMChnageRequest(AssetDTO obj)
        {
            obj.datasetxml = model.GetPPMChangeDateRequestList(obj);
            return obj;
        }
        public AssetDTO GetASsetListByPOContract(AssetDTO obj)
        {
            obj.datasetxml = model.GetAssetListByPOContract(obj);
            return obj;
        }


        public IEnumerable<AssetDTO> GetModel(AssetDTO obj)
        {
            var data = model.GetModels(obj);
            var list = CustomDataReaderToGenericExtension.GetDataObjects<AssetDTO>(data);
            data.Close();
            return list;
        }
        public IEnumerable<AssetDTO> GetPOContractList(AssetDTO obj)
        {
            var data = model.GetPOContractList(obj);
            var list = CustomDataReaderToGenericExtension.GetDataObjects<AssetDTO>(data);
            data.Close();
            return list;
        }

        public AssetDTO CheckSerialNumber(AssetDTO obj)
        {
            try
            {
                var data = model.CheckSerialNumber(obj);
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
        public AssetDTO InsertUpdateAsset(AssetDTO obj)
        {
            try
            {
                var data = model.InsertUpdateAsset(obj);
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
        public AssetDTO UpdatedAsset(AssetDTO obj)
        {
            try
            {
                var data = model.UpdatedAsset(obj);
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
        public AssetDTO VerifyAsset(AssetDTO obj)
        {
            try
            {
                var data = model.VerifyAsset(obj);
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
        public AssetDTO updateppmdate(AssetDTO obj)
        {
            try
            {
                var data = model.UpdatePPMDate(obj);
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
        public AssetDTO UpdateAssetStatus(AssetDTO obj)
        {
            try
            {
                var data = model.UpdateAssetStatus(obj);
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
        public AssetDTO UpdatePPMDateChangeRequest(AssetDTO obj)
        {
            try
            {
                var data = model.UpdatePPMChangeRequest(obj);
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
        public AssetDTO AddJVMOrders(AssetDTO obj)
        {
            try
            {
                var data = model.AddJVMOrder(obj);
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

        public AssetDTO removeAssetModel(AssetDTO obj)
        {
            try
            {
                var data = model.removeAssetModel(obj);
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

        public AssetDTO GetAssetsRenewalDetails(AssetDTO obj)
        {
            obj.datasetxml = model.GetAssetRenewalDetails(obj);
            return obj;
        }
        public AssetDTO GetModelM(AssetDTO obj)
        {
            obj.datasetxml = model.GetModelsM(obj);
            return obj;
        }

        public AssetDTO GetAssetsRenewalList(AssetDTO obj)
        {
            obj.datasetxml = model.GetAssetRenewalList(obj);
            return obj;
        }
        public AssetDTO GetAssetsRenewalRequestList(AssetDTO obj)
        {
            obj.datasetxml = model.GetAssetRenewalRequestList(obj);
            return obj;
        }
        public AssetDTO UpdateAssetRenewalRequest(AssetDTO obj)
        {
            try
            {
                var data = model.UpdateAssetRenewalRequest(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.message = data["message"].ToString();
                        obj.SystemNo = data["SystemNo"].ToString();
                        obj.AccountName = data["AccountName"].ToString();
                        obj.ProductName = data["ProductName"].ToString();
                        obj.POContract = data["POContract"].ToString();
                        obj.ContractTypetxt = data["ContractType"].ToString();
                        obj.StartDateStr = data["StartDateStr"].ToString();
                        obj.EndDateStr = data["EndDateStr"].ToString();
                        obj.PPMTypeText = data["PPMTypeText"].ToString();
                        obj.SuperUserEmail = data["SuperUserEmail"].ToString();
                        obj.SuperUserName = data["SuperUserName"].ToString();
                        obj.SupervisorEmail = data["SupervisorEmail"].ToString();
                        obj.SupervisorName = data["SupervisorName"].ToString();
                    }
                }
                else
                    obj.message = "0";

                //comment code for demo, which was on 16 may 2023
                //start
                //                string _val = "";
                //                if (obj.StatusId == 2)
                //                    _val = "Approved";
                //                else if (obj.StatusId == 3)
                //                    _val = "Rejected";
                //                SendEmailToSuperUser(obj.SystemNo, obj.POContract, obj.ProductName, obj.SuperUserName,
                //       obj.ContractTypetxt, obj.AccountName, obj.PPMTypeText, obj.StartDateStr, obj.EndDateStr, obj.SuperUserEmail, _val);

                //                SendEmailToSupervisorUser(obj.SystemNo, obj.POContract, obj.ProductName, obj.SupervisorName,
                //obj.ContractTypetxt, obj.AccountName, obj.PPMTypeText, obj.StartDateStr, obj.EndDateStr, obj.SupervisorEmail, _val);
                //end
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }
        public AssetDTO InsertAssetModels(AssetDTO obj)
        {
            try
            {
                var data = model.InsertAssetModels(obj);
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


        public AssetDTO InsertAssetRenewalRequest(AssetDTO obj)
        {
            try
            {
                var data = model.InsertAssetRenewalRequest(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.message = data["message"].ToString();
                        obj.SystemNo = data["SystemNo"].ToString();
                        obj.AccountName = data["AccountName"].ToString();
                        obj.ProductName = data["ProductName"].ToString();
                        obj.POContract = data["POContract"].ToString();
                        obj.ContractTypetxt = data["ContractType"].ToString();
                        obj.StartDateStr = data["StartDateStr"].ToString();
                        obj.EndDateStr = data["EndDateStr"].ToString();
                        obj.PPMTypeText = data["PPMTypeText"].ToString();
                        obj.SuperUserEmail = data["SuperUserEmail"].ToString();
                        obj.SuperUserName = data["SuperUserName"].ToString();
                        obj.SupervisorEmail = data["SupervisorEmail"].ToString();
                        obj.SupervisorName = data["SupervisorName"].ToString();

                    }
                }
                else
                    obj.message = "0";

                SendEmailToSuperUser(obj.SystemNo, obj.POContract, obj.ProductName, obj.SuperUserName,
        obj.ContractTypetxt, obj.AccountName, obj.PPMTypeText, obj.StartDateStr, obj.EndDateStr, obj.SuperUserEmail, "Under Approval");

                SendEmailToSupervisorUser(obj.SystemNo, obj.POContract, obj.ProductName, obj.SupervisorName,
obj.ContractTypetxt, obj.AccountName, obj.PPMTypeText, obj.StartDateStr, obj.EndDateStr, obj.SupervisorEmail, "Under Approval");

            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }
        private void SendEmailToSuperUser(string SystemNo, string POContract, string ProductName, string SuperUserName,
       string ContractType, string AccountName, string PPMTypeText, string StartDateStr, string EndDateStr, string SuperUserEmail,
        string Status)
        {
            try
            {
                // Customer Email
                string htmlstr = @"";
                StringBuilder HeaderHtml = new StringBuilder();

                HeaderHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /><title>AHC Helpdesk</title><link href='https://fonts.googleapis.com/css?family=Open+Sans&display=swap' rel='stylesheet'><style type='text/css'>body{font-family:'Open Sans',sans-serif;background:#f1f1f1;color:#0f0f0f;font-size:14px;padding:20px}.pb-15{padding-bottom:15px}.mb-15{margin-bottom:15px}.text-center{text-align:center}.button{padding:8px 12px;background-color:#2cafdd;border-radius:4px;color:#fff;text-decoration:none;display:inline-block;margin-bottom:15px}.button:hover{background-color:#0d96c6;transition:1s all}</style></head><body style='background-color: #f1f1f1; padding: 15px;'><table align='center' style='width: 600px; margin: 0 auto 0 auto;background-color: #fff;padding: 20px 15px;'><tr><td>");
                HeaderHtml.Append("<img src='http://support.arabianhc.com/assets/images/ahc_new_logo.png' class='pb-15' height='44px;'><h4 style='color: #2cafdd'>Dear ");
                HeaderHtml.Append("" + SuperUserName + ",</h4><p> This is an automated message from the AHC Helpdesk System for below Asset details.</p><br><h4>Asset Renewal Information :</h4><div style='width: 160px; height: 2px; background-color: #000;'></div><div style='background-color: #fff; padding-top: 20px; padding-bottom: 15px;'><table><tr>");
                HeaderHtml.Append("<td>Status</td><td>: " + Status + "</td>");
                HeaderHtml.Append("</tr><tr><td>System Id</td><td>: " + SystemNo + "</td>");
                HeaderHtml.Append("</tr><tr><td>Account</td><td>: " + AccountName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Product</td><td>: " + ProductName + "</td>");
                HeaderHtml.Append("</tr><tr><td>PO Contract</td><td>: " + POContract + "</td>");
                HeaderHtml.Append("</tr><tr><td>Contract Type</td><td>: " + ContractType + "</td>");
                HeaderHtml.Append("</tr><tr><td>PPM</td><td>: " + PPMTypeText + "-Months</td>");
                HeaderHtml.Append("</tr><tr><td>Start Date</td><td>: " + StartDateStr + "</td>");
                HeaderHtml.Append("</tr><tr><td>End Date</td><td>: " + EndDateStr + "</td></tr></table><br>");
                //HeaderHtml.Append("<table style='margin-bottom: 10px;'><tr><td> <a href='#' target='_blank' style='text-decoration: none; padding: 8px 12px;border: 1px solid #2cafdd; border-radius: 4px; color: #2cafdd; text-decoration: none;'>Button Click</a></td></tr></table><br><hr><div class='text-center'> <small>please do not hesitate to contact our customer Service support center <strong> +96651111111</strong>,<br> one of our representatives will do their best to assist you.</small></div></td></tr></table></body></html>");
                HeaderHtml.Append("<br><hr><div class='text-center'> <small>please do not hesitate to contact our customer Service support center <strong> 800 2444416</strong></small></div></td></tr></table></body></html>");

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

        private void SendEmailToSupervisorUser(string SystemNo, string POContract, string ProductName, string SupervisorName,
        string ContractType, string AccountName, string PPMTypeText, string StartDateStr, string EndDateStr, string SupervisorEmail,
        string Status)
        {
            try
            {
                // Customer Email
                string htmlstr = @"";
                StringBuilder HeaderHtml = new StringBuilder();

                HeaderHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /><title>AHC Helpdesk</title><link href='https://fonts.googleapis.com/css?family=Open+Sans&display=swap' rel='stylesheet'><style type='text/css'>body{font-family:'Open Sans',sans-serif;background:#f1f1f1;color:#0f0f0f;font-size:14px;padding:20px}.pb-15{padding-bottom:15px}.mb-15{margin-bottom:15px}.text-center{text-align:center}.button{padding:8px 12px;background-color:#2cafdd;border-radius:4px;color:#fff;text-decoration:none;display:inline-block;margin-bottom:15px}.button:hover{background-color:#0d96c6;transition:1s all}</style></head><body style='background-color: #f1f1f1; padding: 15px;'><table align='center' style='width: 600px; margin: 0 auto 0 auto;background-color: #fff;padding: 20px 15px;'><tr><td>");
                HeaderHtml.Append("<img src='http://support.arabianhc.com/assets/images/ahc_new_logo.png' class='pb-15' height='44px;'><h4 style='color: #2cafdd'>Dear ");
                HeaderHtml.Append("" + SupervisorName + ",</h4><p> This is an automated message from the AHC Helpdesk System for below Asset details.</p><br><h4>Asset Renewal Information :</h4><div style='width: 160px; height: 2px; background-color: #000;'></div><div style='background-color: #fff; padding-top: 20px; padding-bottom: 15px;'><table><tr>");
                HeaderHtml.Append("<td>Status</td><td>: " + Status + "</td>");
                HeaderHtml.Append("</tr><tr><td>System Id</td><td>: " + SystemNo + "</td>");
                HeaderHtml.Append("</tr><tr><td>Account</td><td>: " + AccountName + "</td>");
                HeaderHtml.Append("</tr><tr><td>Product</td><td>: " + ProductName + "</td>");
                HeaderHtml.Append("</tr><tr><td>PO Contract</td><td>: " + POContract + "</td>");
                HeaderHtml.Append("</tr><tr><td>Contract Type</td><td>: " + ContractType + "</td>");
                HeaderHtml.Append("</tr><tr><td>PPM</td><td>: " + PPMTypeText + "-Months</td>");
                HeaderHtml.Append("</tr><tr><td>Start Date</td><td>: " + StartDateStr + "</td>");
                HeaderHtml.Append("</tr><tr><td>End Date</td><td>: " + EndDateStr + "</td></tr></table><br>");
                //HeaderHtml.Append("<table style='margin-bottom: 10px;'><tr><td> <a href='#' target='_blank' style='text-decoration: none; padding: 8px 12px;border: 1px solid #2cafdd; border-radius: 4px; color: #2cafdd; text-decoration: none;'>Button Click</a></td></tr></table><br><hr><div class='text-center'> <small>please do not hesitate to contact our customer Service support center <strong> +96651111111</strong>,<br> one of our representatives will do their best to assist you.</small></div></td></tr></table></body></html>");
                HeaderHtml.Append("<br><hr><div class='text-center'> <small>Please do not hesitate to contact our Customer Service Support Center <strong> 800 2444416</strong>,</small></div></td></tr></table></body></html>");

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

        public AssetDTO ppmemailnotification()
        {
            AssetDTO obj = new AssetDTO();
            try
            {
                var data = model.ppmemailnotification(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.message = data["PPMScheduleJson"].ToString();
                        obj.SupervisorEmail = data["UserEmail"].ToString();
                        obj.SuperUserName = data["UserName"].ToString();
                    }
                }
                else
                    obj.message = "0";

                //comment code for demo, which was on 16 may 2023
                //start
                //var datalist = JsonConvert.DeserializeObject<List<AssetDTO>>(obj.message);
                //List<AssetDTO> _objStudent = datalist;
                //if(_objStudent.Count()>0)
                //{
                //    string htmlstr = @"";
                //    StringBuilder HeaderHtml = new StringBuilder();
                //    HeaderHtml.Append("<!DOCTYPE html><html lang='en'><head> <title>Email Template</title> <meta charset='utf-8'> <meta name='viewport' content='width=device-width, initial-scale=1'> <style>body{font-size: 13px; font-weight: 400; font-family: Poppins, Helvetica, 'sans-serif';}.table{font-size: 14px; width: 100%; margin-bottom: 1rem; color: #3F4254; background-color: transparent;}.rounded{border-radius: 0.42rem !important;}table{border-collapse: collapse;}.table.table-dark th{text-align: left;}.table th, .table td{padding: 0.75rem; vertical-align: top; border-top: 1px solid #EBEDF3;}.table thead th{vertical-align: bottom; border-bottom: 2px solid #EBEDF3;}.table-dark th, .table-dark td, .table-dark thead th, .table-dark tbody + tbody{border-color: #878994;}.table-dark th, .table-dark td, .table-dark thead th{border-color: #242a4c;}.table thead th, .table thead td{font-weight: 600; font-size: 13px; border-bottom-width: 1px; padding-top: 1rem; padding-bottom: 1rem;}.table:not(.table-bordered) thead th, .table:not(.table-bordered) thead td{border-top: 0;}.leftie{text-align: left;}.b-0{border-top: 0 !important;}.table-bordered th, .table-bordered td{border: 1px solid #EBEDF3;}.table-bordered thead th, .table-bordered thead td{border-bottom-width: 2px;}.table-dark{color: #ffffff; background-color: #181C32;}.table-bordered{border: 1px solid #EBEDF3;}.table-dark th, .table-dark td, .table-dark thead th{border-color: #242a4c;}.table-dark.table-bordered{border: 0;}.table-dark.table-striped tbody tr:nth-of-type(odd){background-color: rgba(255, 255, 255, 0.05);}.table-dark.table-hover tbody tr:hover{color: #ffffff; background-color: rgba(255, 255, 255, 0.075);}.table-dark.table-striped tbody tr:nth-of-type(odd){background-color: rgba(255, 255, 255, 0.05);}.txt1{color: #000000; font-family: Helvetica,Arial,sans-serif; font-size: 16px; font-weight: normal; line-height: 1.5;}.line1{border-top: solid 3px #ffd56a; margin: 0; width: 60px; text-align: left; margin-bottom: 20px;}</style></head><body style='width:100% !important; margin:0 !important; padding:0 !important; -webkit-text-size-adjust:none; -ms-text-size-adjust:none; background-color:#f5f7fa;'><table cellpadding='0' cellspacing='0' border='0' id='backgroundTable' style='height:auto !important; margin:0; padding:0; width:100% !important; background-color: #f5f7fa; color:#222222;'><tr><td> <div id='tablewrap' style='padding: 25px; background: #fff; width:100% !important; max-width:680px !important; text-align:center !important; margin-top:0 !important; margin-right: auto !important; margin-bottom:0 !important; margin-left: auto !important;'> <table id='contenttable' width='600' align='center' cellpadding='0' cellspacing='0' border='0' style='background-color:#FFFFFF; text-align:center !important; margin-top:0 !important; margin-right: auto !important; margin-bottom:0 !important; margin-left: auto !important; border:none; width: 100% !important; max-width:680px !important;'> <tr> <td width='100%'> <table bgcolor='#FFFFFF' border='0' cellspacing='0' cellpadding='0' width='100%'> <tr> <td width='100%' bgcolor='#ffffff' style='background: aliceblue; padding: 40px; text-align:center; border-radius: 4px;'><a href='#'><img src='http://support.arabianhc.com/assets/images/ahc_new_logo.png' alt='Main banner image and link' style='display:inline-block; max-width:50% !important; width:50% !important; height:auto !important;border-bottom-right-radius:8px;border-bottom-left-radius:8px;' border='0'></a> </td></tr></table> <table bgcolor='#FFFFFF' border='0' cellspacing='0' cellpadding='25' width='100%'> <tr> <td width='100%' bgcolor='#ffffff' style='text-align:left;'> <h5 style='color:#0b0e1f; font-size:16px; line-height:20px; margin-top:5px; margin-bottom:5px; padding:0; '>");
                //    HeaderHtml.Append("Dear " + obj.SuperUserName + ", </h5> <p class='line1'></p><p class='txt1'> This is an automated message from the AHC Helpdesk System to inform you that below are the list of upcoming Schedule PPM list. </p></td></tr></table> <table bgcolor='#FFFFFF' border='0' cellspacing='0' cellpadding='25' width='100%'> <tr> <td width='100%' bgcolor='#ffffff' style='text-align:left; padding-top: 2px'> <h5 style='color:#0b0e1f; font-size:16px; line-height:20px; margin-top:5px; margin-bottom:5px; padding:0; '> PPM Information, </h5> <p class='line1'></p>");
                //    HeaderHtml.Append("<table border='0' cellspacing='0' cellpadding='0' width='100%' class='table table-dark table-striped rounded leftie'> <thead> <tr> <th>Account Name</th> <th>Product Name</th> <th>Contract Type</th> <th>Installation Date</th> <th>PPM Date</th> </tr></thead> <tbody> ");
                //    foreach (var item in _objStudent)
                //    {
                //        HeaderHtml.Append("<tr><td>" + item.AccountName + "</td><td>" + item.ProductName + "</td><td>" + item.ContractTypeText + "</td><td>" + item.InstallationDateStr + "</td><td>" + item.PPMDateStr + "</td></tr>");
                //    }
                //    HeaderHtml.Append("</tbody></table></td></tr></table></body></html>");

                //    htmlstr = HeaderHtml.ToString();
                //    string Subject = "AHC Helpdesk Support Centre";
                //    string mailFrom = System.Configuration.ConfigurationManager.AppSettings["mailFrom"].ToString();
                //    string mailHRBCC = string.Empty;
                //    Models.EmailUtility.sendEmail(mailFrom, obj.SupervisorEmail, htmlstr, Subject, mailHRBCC);
                //}
                //end
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }
        #endregion
    }

    public interface IAssetService
    {
        AssetDTO GetAssetsRenewalDetails(AssetDTO obj);
        AssetDTO GetAssetsRenewalList(AssetDTO obj);
        AssetDTO GetAssetsRenewalRequestList(AssetDTO obj);
        AssetDTO InsertAssetRenewalRequest(AssetDTO obj);
        AssetDTO UpdateAssetRenewalRequest(AssetDTO obj);
        AssetDTO InsertAssetModels(AssetDTO obj);
        AssetDTO InsertUpdateAsset(AssetDTO obj);
        AssetDTO CheckSerialNumber(AssetDTO obj);
        AssetDTO UpdatedAsset(AssetDTO obj);
        AssetDTO VerifyAsset(AssetDTO obj);
        AssetDTO updateppmdate(AssetDTO obj);
        //IEnumerable<AssetDTO> GetAssetList(AssetDTO obj);
        AssetDTO GetAssetList(AssetDTO obj);
        IEnumerable<AssetDTO> GetCity(AssetDTO obj);
        IEnumerable<AssetDTO> GetModel(AssetDTO obj);
        AssetDTO GetModelM(AssetDTO obj);
        IEnumerable<AssetDTO> GetPOContractList(AssetDTO obj);
        AssetDTO GetAssetById(AssetDTO obj);
        AssetDTO GetDropDowns(AssetDTO obj);
        AssetDTO GetApprovalAssets(AssetDTO obj);
        AssetDTO GetPPMChnageRequest(AssetDTO obj);
        AssetDTO GetASsetListByPOContract(AssetDTO obj);
        AssetDTO UpdateAssetStatus(AssetDTO obj);
        AssetDTO UpdatePPMDateChangeRequest(AssetDTO obj);
        AssetDTO AddJVMOrders(AssetDTO obj);
        AssetDTO removeAssetModel(AssetDTO obj);

        AssetDTO ppmemailnotification();
    }
}