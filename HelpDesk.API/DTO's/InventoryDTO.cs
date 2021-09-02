using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API.DTO_s
{
    public class InventoryDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Statusid { get; set; }
        public long WarehousestockId { get; set; }
        public string Type { get; set; }
        public long ConsignmentId { get; set; }
        public string message { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string WarehouseCode { get; set; }
        public bool Status { get; set; }
        public long CreatedBy { get; set; }
        public int OrganizationId { get; set; }
        public string SAPCode { get; set; }
        public int FlagId { get; set; }
        //public int Organizationid { get; set; }

        public string datasetxml { get; set; }
        public int Quantity { get; set; }
        public int ToWarehouseId { get; set; }
        public int BaseQuantity { get; set; }
        public long SparePartId { get; set; }
        public string Comments { get; set; }

        public string SparePartName { get; set; }
        public string SparePartNumber { get; set; }
        public string Price { get; set; }
        public string ConsignmentsJson { get; set; }
        public string WarehouseJson { get; set; }

        public string EnquiryType { get; set; }
        public string CompanyName { get; set; }
        public string Enquiry { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
    }
}