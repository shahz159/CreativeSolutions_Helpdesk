using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API.DTO_s
{
    public class InventoryDTO
    {
        public string message { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string WarehouseCode { get; set; }
        public bool Status { get; set; }
        public long CreatedBy { get; set; }
        public int FlagId { get; set; }
        public int Organizationid { get; set; }

        public string datasetxml { get; set; }
        public int Quantity { get; set; }
        public int BaseQuantity { get; set; }
        public long SparePartId { get; set; }
        public string SparePartName { get; set; }
        public string SparePartNumber { get; set; }
        public string Price { get; set; }

    }
}