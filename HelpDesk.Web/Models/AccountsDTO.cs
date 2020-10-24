using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelpDesk.Web.Models
{
    public class AccountsDTO
    {
        public string message { get; set; }
        public int AccountId { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Max Length is 100")]
        public string AccountCode { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max Length is 100")]
        public string AccountName { get; set; }
        public bool isActive { get; set; }
        public long CreatedBy { get; set; }
        public int FlagId { get; set; }
        public int CompanyId { get; set; }
        public IEnumerable<AccountsDTO> AccountsLst { get; set; }
    }
}