using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.Web
{
    public class MenusDTO
    {
        public int MenuId { get; set; }
        public int ParentId { get; set; }
        public int OrderId { get; set; }
        public string MenuName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public int NewUser { get; set; }
        public int WarrantyExpiredApprovalCount { get; set; }
        public int AssetRenewalCount { get; set; }
        public int AssetApprovalCount { get; set; }
        public int InventoryAdjustment { get; set; }
        public int PPMDatesApprovalCount { get; set; }
        public int SparePartRequestCount { get; set; }
    }
}