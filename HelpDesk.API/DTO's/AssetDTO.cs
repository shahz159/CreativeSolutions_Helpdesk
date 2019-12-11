using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API.DTO_s
{
    public class AssetDTO
    {
        public string PPMJson { get; set; }
        public int AMId { get; set; }
        public int AccountId { get; set; }
        public int ProductId { get; set; }
        public int ModelId { get; set; }
        public string AccountName { get; set; }
        public string AccountCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ModelName { get; set; }

        public string StationName { get; set; }
        public string IPAddress { get; set; }
        public string SerialNo { get; set; }
        public string Configuration { get; set; }
        public string Area { get; set; }

        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public DateTime InstallationDate { get; set; }
        public bool IsContract { get; set; }
        public string POContract { get; set; }
        public DateTime WarrantyExpiryDate { get; set; }
        public int PPMType { get; set; }
        public string SystemNo { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }

        public long ApprovedBy { get; set; }
        public bool isActive { get; set; }
        public long CreatedBy { get; set; }
        public string FullName { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }

        public string message { get; set; }
        public int FlagId { get; set; }
        public string datasetxml { get; set; }
        public int OrganizationId { get; set; }

    }
}