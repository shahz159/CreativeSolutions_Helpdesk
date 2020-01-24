using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API.DTO_s
{
    public class TicketDTO
    {
        public long EnquiryId { get; set; }

        public string multipledocuments_xml { get; set; }
        public int ReportId { get; set; }
        public string  ReportTypeName { get; set; }
        public string message { get; set; }
        public string datasetxml { get; set; }
        public string Description { get; set; }
        public int ProductId { get; set; }
        public int AMId { get; set; }
        public string Priority { get; set; }
        public int Status { get; set; }
        public int CompanyId { get; set; }
        public int AccountId { get; set; }
        public int RoleId { get; set; }
        public long CreatedBy { get; set; }
        public int OrganizationId { get; set; }
        public int FlagId { get; set; }
        public long UserId { get; set; }
        public string Url { get; set; }
        public string ContentType { get; set; }
        public long TicketNumber { get; set; }
        public int WarehouseId { get; set; }
        public string Comments { get; set; }
        public string ProblemDescription { get; set; }
        public DateTime ResponseTime { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string UserEmail { get; set; }
        public string ServiceEngineerEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ProductName { get; set; }
        public string SystemId { get; set; }
        public string AccountName { get; set; }
        public string Location { get; set; }
        public string CustomerFullName { get; set; }
        public string CustomerEmail { get; set; }
        public string EngineerEmail { get; set; }
        public string EngineerFullName { get; set; }
    }
}