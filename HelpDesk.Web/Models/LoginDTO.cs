using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.Web.Models
{
    public class LoginDTO
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int OrganizationId { get; set; }
        public int CompanyId { get; set; }
        public int AccountId { get; set; }
        public string RoleName { get; set; }
        public string ProfileImage { get; set; }
    }
}