using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API
{
    public class AccountDTO
    {
        public string message { get; set; }
        public int AccountId { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public bool isActive { get; set; }
        public long CreatedBy { get; set; }
        public int FlagId { get; set; }
        public int CompanyId { get; set; }
    }
}