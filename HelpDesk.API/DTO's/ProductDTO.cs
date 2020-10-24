using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API.DTO_s
{
    public class ProductDTO
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public bool isActive { get; set; }
        public long CreatedBy { get; set; }
        public int FlagId { get; set; }
        public int ProductId { get; set; }
        public int CompanyId { get; set; }
        public string message { get; set; }
    }
}