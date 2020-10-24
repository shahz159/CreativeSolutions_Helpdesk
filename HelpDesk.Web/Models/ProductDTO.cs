using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelpDesk.Web.Models
{
    public class ProductDTO
    {
        public string message { get; set; }
        public int ProductId { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Max Length is 100")]
        public string ProductCode { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max Length is 100")]
        public string ProductName { get; set; }
        public bool isActive { get; set; }
        public long CreatedBy { get; set; }
        public int FlagId { get; set; }
        public int CompanyId { get; set; }
        public IEnumerable<ProductDTO> ProductsLst { get; set; }
    }
}