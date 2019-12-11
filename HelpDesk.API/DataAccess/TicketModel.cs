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
    public class TicketModel: ITicketModel
    {
        public SqlDataReader NewTicketRequest(TicketDTO obj)
        {
            try
            {
                var para = new[]
                {
                    new SqlParameter("@Description",obj.Description),
                    new SqlParameter("@ProductId",obj.ProductId),
                    new SqlParameter("@AMId",obj.AMId),
                    new SqlParameter("@Priority",obj.Priority),
                    new SqlParameter("@Status",obj.Status),
                    new SqlParameter("@CompanyId",obj.CompanyId),
                    new SqlParameter("@AccountId",obj.AccountId),
                    new SqlParameter("@CreatedBy",obj.CreatedBy),
                    new SqlParameter("@OrganizationId",obj.OrganizationId),
                    new SqlParameter("@DocumentUrl",obj.Url),
                    new SqlParameter("@ContentType",obj.ContentType) 
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
                new SqlParameter("@AccountId",obj.AccountId)
                };
                return DbConnector.ExecuteDataSet("uspGetProductsByAccountId", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> Getproducts");
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
        
        public SqlDataReader UpdateTicketStatus(TicketDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@Status",obj.Status),
                new SqlParameter("@CreatedBy",obj.CreatedBy),
                new SqlParameter("@TicketNumber",obj.TicketNumber),
                new SqlParameter("@Comments",obj.Comments)
                };
                return DbConnector.ExecuteReader("uspUpdateTicketStatus", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UpdateTicketStatus -> UpdateTicketStatus");
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
                new SqlParameter("@TicketNumber",obj.TicketNumber),
                new SqlParameter("@ReportTypeId",obj.ReportId)
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
                new SqlParameter("@OrganizationId",obj.OrganizationId)
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
                new SqlParameter("@CompanyId",obj.CompanyId)
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
                new SqlParameter("@WarehouseId",obj.WarehouseId) 
                };
                return DbConnector.ExecuteDataSet("uspGetSparePartsByWHId", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "TicketModel -> GetSparePartListByWarehouseId");
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
        SqlDataReader UpdateTicketStatus(TicketDTO obj);
        SqlDataReader AddResponseTieme(TicketDTO obj);

        string GetDashBoardCount(TicketDTO obj);

        string GetServiceEngineerTickets(TicketDTO obj);
        string GetSparePartRequestTickets(TicketDTO obj);
        string GetServiceEngineerTicketsFilerts(TicketDTO obj);
        string GetTicketDetails(TicketDTO obj);
        string GetSparePartListByWarehouseId(TicketDTO obj);

        SqlDataReader AddSparePartRequest(TicketDTO obj);
        SqlDataReader Addcomments(TicketDTO obj);
    }
}

