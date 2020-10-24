using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelpDesk.Web.Models
{
    public class ModelDTO
    {
        public string message { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public bool isActive { get; set; }
        public long CreatedBy { get; set; }
        public int FlagId { get; set; }
        public int CompanyId { get; set; }
        public IEnumerable<ModelDTO> ProductsLst { get; set; }
        public IEnumerable<ModelDTO> ModelLst { get; set; }

        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max Length is 100")]
        public string ModelName { get; set; }
        public int ModelId { get; set; }

    }
}