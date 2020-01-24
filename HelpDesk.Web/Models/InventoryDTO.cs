using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.Web.Models
{
    public class InventoryDTO
    {
        public int ToWarehouseId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Statusid { get; set; }
        public long ConsignmentId { get; set; }
        public string AHCCode { get; set; }
        public string Comments { get; set; }
        public string ConsignmentsJson { get; set; }
        public string ConsignmentDate { get; set; }
        public int Stock { get; set; }
        public long WarehousestockId { get; set; }
        public string Type { get; set; }
        public int RequestPending { get; set; }
        public int RequestStatus { get; set; }
        //public string WarehouseJson { get; set; }
        public string Gruop { get; set; }
        public string message { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string WarehouseCode { get; set; }
        public bool Status { get; set; }
        public long CreatedBy { get; set; }
        public int RoleId { get; set; }
        public int FlagId { get; set; }
        public int OrganizationId { get; set; }

        public string datasetxml { get; set; }
        public int Quantity { get; set; }
        public int BaseQuantity { get; set; }
        public long SparePartId { get; set; }
        public string SparePartName { get; set; }
        public string SparePartNumber { get; set; }
        public string Price { get; set; }

        public IEnumerable<InventoryDTO> WarehouseList { get; set; }
        public IEnumerable<InventoryDTO> SparePartList { get; set; }
        public IEnumerable<InventoryDTO> WHddlList { get; set; }

        public IEnumerable<InventoryDTO> ConsignmentsList { get; set; }

        public string Statustxt { get; set; }
        public string WarehouseJson { get; set; }
    }
}