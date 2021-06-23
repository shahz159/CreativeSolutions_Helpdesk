using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API.DTO_s
{
    public class TicketDTO
    {
        public bool IsMobile { get; set; }
        public long EnquiryId { get; set; }

        public string multipledocuments_xml { get; set; }
        public int ReportId { get; set; }
        public long APPMId { get; set; }
        public string  ReportTypeName { get; set; }
        public string message { get; set; }
        public int SystemManagerId { get; set; }
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
        public int RatingCount { get; set; }
        public int OrganizationId { get; set; }
        public int FlagId { get; set; }
        public long UserId { get; set; }
        public string Url { get; set; }
        public string ContentType { get; set; }
        public string UniqueId { get; set; }
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
        public long AMModelId { get; set; }
        public string CreatedDateStr { get; set; }
        public string ManagerName { get; set; }
        public string ManagerEmail { get; set; }
        public string StatusText { get; set; }

        public string SuperUserEmail { get; set; }
        public string SuperUserName { get; set; }
        public string SupervisorEmail { get; set; }
        public string SupervisorName { get; set; }
        public long SupervisorUserId { get; set; }
        public long ServiceEngineerId { get; set; }

        public string SerialNo { get; set; }
        public string ModelName { get; set; }
        public string StationName { get; set; }
        public string TicketClosedDate { get; set; }

        //public List<Utils.FileAttribs> TicketDocuments { get; set; }
        public Utils.FileAttribs TicketDocuments { get; set; }
        public string Base64FileData { get; set; }
        public List<TicketDTO> multiple_images { get; set; }

    }
}