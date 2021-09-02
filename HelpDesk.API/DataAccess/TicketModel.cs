using HelpDesk.API.DatabaseConnector;
using HelpDesk.API.DbHelpers;
using HelpDesk.API.DTO_s;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HelpDesk.API.DataAccess
{
    public class TicketModel : ITicketModel
    {
        public SqlDataReader NewTicketRequest(TicketDTO obj)
        {
            try
            {
                var para = new[]
                {
                    new SqlParameter("@Description",obj.Description),
                    new SqlParameter("@ProductId",obj.ProductId),
                    new SqlParameter("@AMModelId",obj.AMModelId),
                    new SqlParameter("@Priority",obj.Priority),
                    new SqlParameter("@Status",obj.Status),
                    new SqlParameter("@CompanyId",obj.CompanyId),
                    new SqlParameter("@AccountId",obj.AccountId),
                    new SqlParameter("@CreatedBy",obj.CreatedBy),
                    new SqlParameter("@OrganizationId",obj.OrganizationId),
                    new SqlParameter("@DocumentUrl",obj.Url),
                    new SqlParameter("@ContentType",obj.ContentType),
                    new SqlParameter("@xml",obj.multipledocuments_xml),
                    new SqlParameter("@ReportId",obj.ReportId),
                    new SqlParameter("@APPMId",obj.APPMId),
                    new SqlParameter("@AMId",obj.AMId),
                    new SqlParameter("@SystemManagerId",obj.SystemManagerId),
                    new SqlParameter("@IsMobile",obj.IsMobile)
                };
                return DbConnector.ExecuteReader("uspNewTicketRequest", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> NewTicketRequest");
                return null;
            }
        }
        public string GetApprovalTickets(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@CompanyId",obj.CompanyId)
                };
                return DbConnector.ExecuteDataSet("uspGetUnderApprovalTickets", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> GetApprovalTickets");
                return null;
            }
        }

        public string GetSystemUserProducts(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@UserId",obj.CreatedBy),
                new SqlParameter("@companyid",obj.CompanyId),
                new SqlParameter("@OrganizationId",obj.OrganizationId)
                };
                return DbConnector.ExecuteDataSet("uspSystemUserProducts", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> GetSystemUserProducts");
                return null;
            }
        }

        public string GetSystemUserModels(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@UserId",obj.CreatedBy),
                new SqlParameter("@ProductId",obj.ProductId),
                new SqlParameter("@RoleId",obj.RoleId),
                new SqlParameter("@AccountId",obj.AccountId)
                };
                return DbConnector.ExecuteDataSet("uspSystemUserProductModels", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> GetSystemUserModels");
                return null;
            }
        }
        public string GetAccounts(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@CompanyId",obj.CompanyId)
                };
                return DbConnector.ExecuteDataSet("[dbo].[uspGetCompanyAccounts]", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> GetAccounts");
                return null;
            }
        }
        public string Getproducts(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@AccountId",obj.AccountId),
                new SqlParameter("@UserId",obj.CreatedBy),
                new SqlParameter("@RoleId",obj.RoleId)
                };
                return DbConnector.ExecuteDataSet("uspGetProductsByAccountId", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> Getproducts");
                return null;
            }
        }


        public string GetSystemUserTicketsMobile(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@UserId",obj.CreatedBy),
                 new SqlParameter("@StatusId",obj.Status),
                  new SqlParameter("@ServiceEngineerId",obj.ServiceEngineerId),
                   new SqlParameter("@ReportTypeId",obj.ReportId),
                    new SqlParameter("@PageNumber",obj.PageNumber),
                     new SqlParameter("@PageSize",obj.PageSize)

                };
                return DbConnector.ExecuteDataSet("uspTicketListMobile", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> GetSystemUserTicketsMobile");
                return null;
            }
        }
        public string GetSystemUserTickets(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@UserId",obj.CreatedBy)
                };
                return DbConnector.ExecuteDataSet("uspGetSystemUserTicket", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> GetSystemUserTickets");
                return null;
            }
        }
        public string GetRejectedTickets(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@UserId",obj.CreatedBy),
                new SqlParameter("@RoleId",obj.RoleId)
                };
                return DbConnector.ExecuteDataSet("uspGetRejectedTicktes", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> GetRejectedTickets");
                return null;
            }
        }

        public SqlDataReader UpdateTicketStatus(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@Status",obj.Status),
                new SqlParameter("@CreatedBy",obj.CreatedBy),
                new SqlParameter("@TicketNumber",obj.TicketNumber),
                new SqlParameter("@Comments",obj.Comments),
                new SqlParameter("@ProblemDescription",obj.ProblemDescription)
                };
                return DbConnector.ExecuteReader("uspUpdateTicketStatus", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UpdateTicketStatus -> UpdateTicketStatus");
                return null;
            }
        }

        public SqlDataReader AddticketRating(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@TicketId",obj.TicketNumber),
                new SqlParameter("@RatingCount",obj.RatingCount),
                new SqlParameter("@description",obj.Description),
                new SqlParameter("@UserId",obj.CreatedBy)
                };
                return DbConnector.ExecuteReader("uspAddRating", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "AddticketRating -> AddticketRating");
                return null;
            }
        }
        public string TicketRatingList(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@TicketId",obj.TicketNumber)
                };
                return DbConnector.ExecuteDataSet("uspGetRating", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "ticketRatinglist -> ticketRatinglist");
                return null;
            }
        }
        public SqlDataReader TicketTransfer(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@TicketNumber",obj.TicketNumber),
                new SqlParameter("@UserId",obj.UserId),
                new SqlParameter("@CreatedBy",obj.CreatedBy)
                };
                return DbConnector.ExecuteReader("uspTransferTicket", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> TicketTransfer");
                return null;
            }
        }
        public SqlDataReader GetSystemManagerId(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@ProductId",obj.ProductId),
                new SqlParameter("@AccountId",obj.AccountId)
                };
                return DbConnector.ExecuteReader("uspGetSystemManagerId", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> GetSystemManagerId");
                return null;
            }
        }

        public SqlDataReader AddResponseTieme(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@ResponseTime",obj.ResponseTime),
                new SqlParameter("@CreatedBy",obj.CreatedBy),
                new SqlParameter("@TicketNumber",obj.TicketNumber)
                //new SqlParameter("@ReportTypeId",obj.ReportId)
                };
                return DbConnector.ExecuteReader("uspAddResponseTime", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> AddResponseTieme");
                return null;
            }
        }
        public SqlDataReader AddSparePartRequest(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@TicketNumber",obj.TicketNumber),
                new SqlParameter("@UserId",obj.UserId),
                new SqlParameter("@json",obj.message),
                new SqlParameter("@CompanyId",obj.CompanyId),
                new SqlParameter("@OrganizationId",obj.OrganizationId),
                new SqlParameter("@FlagId",obj.FlagId)
                };
                return DbConnector.ExecuteReader("uspAddSparePartRequest", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> AddSparePartRequest");
                return null;
            }
        }
        public SqlDataReader Addcomments(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@TicketNumber",obj.TicketNumber),
                new SqlParameter("@UserId",obj.UserId),
                new SqlParameter("@Comment",obj.message)
                };
                return DbConnector.ExecuteReader("uspAddComments", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> Addcomments");
                return null;
            }
        }

        public string GetServiceEngineerTickets(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@UserId",obj.CreatedBy),
                new SqlParameter("@OrganizationId",obj.OrganizationId),
                new SqlParameter("@UserIdF",obj.UserId),
                new SqlParameter("@StatusIdF",obj.Status),
                new SqlParameter("@AccountIdF",obj.AccountId),
                new SqlParameter("@pageNumber",obj.PageNumber),
                new SqlParameter("@pageSize",obj.PageSize),
                new SqlParameter("@CompanyId",obj.CompanyId),
                new SqlParameter("@searchtxt",obj.message)
                };
                return DbConnector.ExecuteDataSet("uspGetServiceEngineerTickets", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> GetServiceEngineerTickets");
                return null;
            }
        }
        public string GetDashBoardCount(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@UserId",obj.CreatedBy)
                };
                return DbConnector.ExecuteDataSet("uspGetDashboardCount", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> GetDashBoardCount");
                return null;
            }
        }


        public string GetServiceEngineerCounts(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@UserId",obj.UserId)
                };
                return DbConnector.ExecuteDataSet("uspGetEngineerTicketCount", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> GetServiceEngineerCounts");
                return null;
            }
        }
        public string GetSparePartRequestTickets(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@CompanyId",obj.CompanyId)
                };
                return DbConnector.ExecuteDataSet("uspGetSparePartRequestTickets", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> GetSparePartRequestTickets");
                return null;
            }
        }
        public string GetEnquiryList(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@CompanyId",obj.CompanyId),
                new SqlParameter("@UserId",obj.UserId),
                new SqlParameter("@RoleId",obj.RoleId)
                };
                return DbConnector.ExecuteDataSet("uspGetEnquiries", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> GetEnquiryList");
                return null;
            }
        }

        public string GetEnquiryDetails(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@EnquiryId",obj.EnquiryId)
                };
                return DbConnector.ExecuteDataSet("uspGetEnquiryDetails", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> GetEnquiryDetails");
                return null;
            }
        }

        public string GetServiceEngineerTicketsFilerts(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@UserId",obj.CreatedBy),
                new SqlParameter("@OrganizationId",obj.OrganizationId),
                new SqlParameter("@CompanyId",obj.CompanyId)
                };
                return DbConnector.ExecuteDataSet("uspGetAssignedTicketsFilters", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> GetServiceEngineerTicketsFilerts");
                return null;
            }
        }

        public string GetTicketDetails(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@TicketNumber",obj.TicketNumber)
                };
                return DbConnector.ExecuteDataSet("UspGetTicketDetails", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> GetTicketDetails");
                return null;
            }
        }
        public string GetSparePartListByWarehouseId(TicketDTO obj)
        {
            try
            {

                var para = new[] {
                new SqlParameter("@WarehouseId",obj.WarehouseId) ,
                new SqlParameter("@TicketNumber",obj.TicketNumber)
                };
                return DbConnector.ExecuteDataSet("uspGetSparePartsByWHId", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> GetSparePartListByWarehouseId");
                return null;
            }
        }

        public SqlDataReader AddEnquirycomments(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@UserId",obj.UserId),
                new SqlParameter("@EnquiyId",obj.EnquiryId),
                new SqlParameter("@Comments",obj.message)
                };
                return DbConnector.ExecuteReader("uspAddEnquiryComments", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> AddEnquirycomments");
                return null;
            }
        }
        public SqlDataReader AddEnquiry(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@Comments",obj.message),
                new SqlParameter("@UserId",obj.UserId),
                new SqlParameter("@CompanyId",obj.CompanyId),
                };
                return DbConnector.ExecuteReader("uspAddEnquiry", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> AddEnquiry");
                return null;
            }
        }
        public SqlDataReader CrmRawData(TicketDTO obj)
        {
            try
            {
                return DbConnector.ExecuteReader("uspCRMRawDataReport", null);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> CrmRawData");
                return null;
            }
        }
        public SqlDataReader ServiceArchiveTicket(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@Month",obj.MonthId),
                new SqlParameter("@Year",obj.YearId),
                new SqlParameter("@UserId",obj.CreatedBy)
                };
                return DbConnector.ExecuteReader("uspGetTicketListByMonthYear", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> ServiceArchiveTicket");
                return null;
            }
        }

        public SqlDataReader AssetListReport(TicketDTO obj)
        {
            try
            {
                return DbConnector.ExecuteReader("uspAssetListReport", null);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> AssetListReport");
                return null;
            }
        }

        public SqlDataReader ProductReport(TicketDTO obj)
        {
            try
            {

                return DbConnector.ExecuteReader("uspProductReport", null);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> ProductReport");
                return null;
            }
        }

        public SqlDataReader EngineerWiseStatusReport(TicketDTO obj)
        {
            try
            {

                return DbConnector.ExecuteReader("uspEngineerWiseStatusReport", null);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> EngineerWiseStatusReport");
                return null;
            }
        }

        public SqlDataReader AccountTicketReport(TicketDTO obj)
        {
            try
            {

                return DbConnector.ExecuteReader("uspAccountTicketReport", null);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> AccountTicketReport");
                return null;
            }
        }

        public SqlDataReader PerMonthStatus(TicketDTO obj)
        {
            try
            {

                return DbConnector.ExecuteReader("uspPerMonthStatus", null);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> PerMonthStatus");
                return null;
            }
        }

        public SqlDataReader RepeatedErrorReport(TicketDTO obj)
        {
            try
            {

                return DbConnector.ExecuteReader("uspRepeatedErrorReport", null);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> RepeatedErrorReport");
                return null;
            }
        }

        public SqlDataReader SparePartTicketsCountReport(TicketDTO obj)
        {
            try
            {

                return DbConnector.ExecuteReader("uspSparePartTicketsCountReport", null);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> SparePartTicketsCountReport");
                return null;
            }
        }
    }

    public interface ITicketModel
    {
        SqlDataReader NewTicketRequest(TicketDTO obj);
        string GetApprovalTickets(TicketDTO obj);

        string GetSystemUserProducts(TicketDTO obj);
        string GetSystemUserModels(TicketDTO obj);
        string GetAccounts(TicketDTO obj);
        string Getproducts(TicketDTO obj);
        string GetSystemUserTickets(TicketDTO obj);
        string GetSystemUserTicketsMobile(TicketDTO obj);
        string GetRejectedTickets(TicketDTO obj);
        SqlDataReader UpdateTicketStatus(TicketDTO obj);
        SqlDataReader AddticketRating(TicketDTO obj);
        string TicketRatingList(TicketDTO obj);

        SqlDataReader TicketTransfer(TicketDTO obj);
        SqlDataReader GetSystemManagerId(TicketDTO obj);
        SqlDataReader AddResponseTieme(TicketDTO obj);

        string GetDashBoardCount(TicketDTO obj);

        string GetServiceEngineerTickets(TicketDTO obj);
        string GetSparePartRequestTickets(TicketDTO obj);
        string GetServiceEngineerCounts(TicketDTO obj);
        string GetEnquiryList(TicketDTO obj);
        string GetEnquiryDetails(TicketDTO obj);
        string GetServiceEngineerTicketsFilerts(TicketDTO obj);
        string GetTicketDetails(TicketDTO obj);
        string GetSparePartListByWarehouseId(TicketDTO obj);

        SqlDataReader AddSparePartRequest(TicketDTO obj);
        SqlDataReader Addcomments(TicketDTO obj);

        SqlDataReader AddEnquirycomments(TicketDTO obj);
        SqlDataReader AddEnquiry(TicketDTO obj);
        SqlDataReader CrmRawData(TicketDTO obj);
        SqlDataReader ServiceArchiveTicket(TicketDTO obj);
        SqlDataReader AssetListReport(TicketDTO obj);
        SqlDataReader ProductReport(TicketDTO obj);
        SqlDataReader EngineerWiseStatusReport(TicketDTO obj);
        SqlDataReader AccountTicketReport(TicketDTO obj);
        SqlDataReader PerMonthStatus(TicketDTO obj);
        SqlDataReader RepeatedErrorReport(TicketDTO obj);
        SqlDataReader SparePartTicketsCountReport(TicketDTO obj);
    }
}

