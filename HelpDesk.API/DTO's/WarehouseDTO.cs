using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API.DTO_s
{
    public class WarehouseDTO
    {
        public int WarehouseId { get; set; }
        public int OrganizationId { get; set; }
        public string WarehouseName { get; set; }
        public string AHCCode { get; set; }
        public bool isActive { get; set; }
        public long CreatedBy { get; set; }
        public int FlagId { get; set; }
        public string message { get; set; }
        public long UserId { get; set; }
        public int MUWId { get; set; }
        public string FullName { get; set; }
        public string datasetxml { get; set; }
    }
}