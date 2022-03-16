using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API.DTO_s
{
    public class UsersDTO
    {
        public string[] Accounts { get; set; }
        public string[] Products { get; set; }
        public int Type { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string message { get; set; }
        public int RoleId { get; set; }
        public string EmpId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool isApproved { get; set; }
        public bool isCancelled { get; set; }
        public bool isActive { get; set; }
        public int OrganizationId { get; set; }
        public int CompanyId { get; set; }
        public long CreatedBy { get; set; }
        public string Accountsxml { get; set; }
        public string Productsxml { get; set; }
        public string datasetxml { get; set; }
        public long UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public bool SignUp { get; set; }
        public long MUPId { get; set; }
        public DateTime CreatedDate{get;set;}
        public string SuperUserEmail { get; set; }
        public int EmailId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IEnumerable<UsersDTO> EmailList { get; set; }
        public string EmailJson { get; set; }

    }
}