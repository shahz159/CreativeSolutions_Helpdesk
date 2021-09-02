using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.Web.Models
{
    public class UserDTO
    {
        public bool isCancelled { get; set; }

        public bool Cancelled { get; set; }
        public string Accountsxml { get; set; }
        public string Productsxml { get; set; }
        public string[] Accounts { get; set; }
        public string[] Products { get; set; }
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
        public int Type { get; set; }
        public bool SignUp { get; set; }
        public bool isActive { get; set; }
        public int OrganizationId { get; set; }
        public int CompanyId { get; set; }
        public long CreatedBy { get; set; }
        public string xml { get; set; }
        public string datasetxml { get; set; }
        public long UserId { get; set; }
        public string RoleName { get; set; }
        public string CompanyName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public IEnumerable<UserDTO> UsersList { get; set; }
        public IEnumerable<UserDTO> AccountList { get; set; }
        public IEnumerable<UserDTO> ProductList { get; set; }


        public IEnumerable<UserDTO> AccountddlList { get; set; }
        public IEnumerable<UserDTO> ProductddlList { get; set; }

        public long MUPId { get; set; }
        public long MUACId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string AccountCode { get; set; }

        public string EnquiryType { get; set; }
        public string Enquiry { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
    }
}