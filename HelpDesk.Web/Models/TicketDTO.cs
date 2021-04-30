using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelpDesk.Web.Models
{
    public class TicketDTO
    {
        public int SystemManagerId { get; set; }

        public long WarehouseStockId { get; set; }
        public long APPMId { get; set; }

        public string EnquiryDate { get; set; }

        public long EnquiryId { get; set; }
        public int FlagId { get; set; }
        public int RoleId { get; set; }
        public int val { get; set; }
        public string PriorityText { get; set; }
        public int ModelId { get; set; }
        public string message { get; set; }
        public string datasetxml { get; set; }

        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        public string Description { get; set; }

        public int? ProductId { get; set; }

        [Required(ErrorMessage = "*")]
        public int AMId { get; set; }
        [Required(ErrorMessage = "*")]
        public long AMModelId { get; set; }

        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        public string Priority { get; set; }
        public string CreatedUser { get; set; }
        public string ReportsJson { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        public int ReportId { get; set; }
        public int CreatedUserRoleId { get; set; }
        public string ReportTypeName { get; set; }

        public int Status { get; set; }
        public string Statustxt { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        public int CompanyId { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        public int AccountId { get; set; }
        public long CreatedBy { get; set; }
        public int OrganizationId { get; set; }
        public string Url { get; set; }
        public string multipledocuments_xml { get; set; }
        public string ContentType { get; set; }
        public string UniqueId { get; set; }
        public long TicketNumber { get; set; }
        public string Comments { get; set; }
        public string ProblemDescription { get; set; }
        public string WorkHours { get; set; }
        public DateTime ResponseTime { get; set; }
        public string AccountName { get; set; }
        public string ModelName { get; set; }
        public string SystemNoSerialNo { get; set; }
        public string SystemNo { get; set; }
        public string FullName { get; set; }
        public string Area { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public IEnumerable<TicketDTO> TicketList { get; set; }
        public IEnumerable<TicketDTO> ProductList { get; set; }
        public IEnumerable<TicketDTO> ModelList { get; set; }
        public IEnumerable<TicketDTO> ReportList { get; set; }
        public IEnumerable<TicketDTO> UrlList { get; set; }
        public long UserId { get; set; }
        public string SerialNo { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedOnNew { get; set; }
        public string CompanyName { get; set; }
        public string RequestResponseStr { get; set; }
        public string ActualStartTime { get; set; }
        public string ResolvedTime { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int pagingNumber { get; set; }

        public int MappedWarehouseId { get; set; }
        public string Actioncomments { get; set; }
        public string WarehouseJson { get; set; }
        public string SparePartRequestJson { get; set; }
        public string StatusJson { get; set; }

        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public IEnumerable<TicketDTO> WarehouseList { get; set; }
        public long SparePartId { get; set; }
        public string SparePartName { get; set; }
        public string SparePartNumber { get; set; }
        public int Quantity { get; set; }
        public int BaseQuantity { get; set; }
        public string Price { get; set; }
        public  IEnumerable<TicketDTO> SparePartList { get; set; }
        public IEnumerable<TicketDTO> StatusLst { get; set; }
        public IEnumerable<TicketDTO> CommentsList { get; set; }
        public IEnumerable<TicketDTO> ServiceEngineerList { get; set; }
        public string Comment { get; set; }
        public string Commentsdate { get; set; }
        public string commentsjson { get; set; }
        public int ParentId { get; set; }
        public long ECommentsId { get; set; }
       // public string CommentsDate { get; set; }
        public string Gruop { get; set; }
        public int NewTickets { get; set; }
        public int InProgressTickets { get; set; }
        public int ResolvedTickets { get; set; }
        public string ManagerName { get; set; }
        public string SupervisorName { get; set; }
        public string ServiceEngineerJson { get; set; }
        public string StationName { get; set; }
        public string ManagerConfirmationDate { get; set; }
        public string SupervisorConfirmationDate { get; set; }
        public string CustomerConfirmationDate { get; set; }
        public string ServiceEngineerResolvedDate { get; set; }
        public string ServiceStartDate { get; set; }
        public string PPMDate { get; set; }
        public string WarrantyExpiryDate { get; set; }
        public bool IsContract { get; set; }
        public string InstallationDate { get; set; }
        public string POContract { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string AccountCode { get; set; }
        public string EmpID { get; set; }
        public long CreatedUserId { get; set; }

        public IEnumerable<TicketDTO> CountLst { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public int New { get; set; }
        public int Closed { get; set; }
        public int Resolved { get; set; }
        public string New_array { get; set; }
        public string Closed_array { get; set; }
        public string Resolved_array { get; set; }
        public string Months_array { get; set; }
        public string Pie_array { get; set; }

        public string ManagerFullName { get; set; }
        public string ManagerMobile { get; set; }
        public string ManagerEmail { get; set; }

        public IEnumerable<TicketDTO> RawDataReportList { get; set; }
        public string CreatedByName { get; set; }
        public string Duration { get; set; }
        public string ClosedName { get; set; }
        public string RequestMethod { get; set; }
        public string RequestType { get; set; }
        public string AccountPhones { get; set; }
        public string ContactName { get; set; }
        public string ContactPhones { get; set; }
        public string AddressName { get; set; }
        public string RequestMainStatus { get; set; }
        public string RequestNotes { get; set; }
        public string Action { get; set; }
        public string EngineerName { get; set; }
        public string EngineerAssignedDate { get; set; }
        public string EngineerClosedDate { get; set; }
        public string ServiceReportId { get; set; }
        public string Configuration { get; set; }
        public string ClosedDate { get; set; }
        public string CreatedandScheduleDuration { get; set; }
        public string ScheduleandActualRespocsetimeDuration { get; set; }
        public string ResolvedtoCloseDate { get; set; }
        public string ScheduleTime { get; set; }
       
    }
}


