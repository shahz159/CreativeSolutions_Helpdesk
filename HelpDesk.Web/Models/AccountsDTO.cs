using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.Web.Models
{
    public class AccountsDTO
    {
        public string message { get; set; }
        public int AccountId { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public bool isActive { get; set; }
        public long CreatedBy { get; set; }
        public int FlagId { get; set; }
        public int CompanyId { get; set; }
        public IEnumerable<AccountsDTO> AccountsLst { get; set; }
    }
}