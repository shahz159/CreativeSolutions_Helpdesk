using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.Web.Models
{
    public class AssetsDTO
    {
        public int Quantity { get; set; }
        public string JVMOrderDate { get; set; }
        public string JVMOrdersJson { get; set; }
        public int TotalCanister { get; set; }
        public bool IsJVM { get; set; }
        public int Canister { get; set; }
        public int NoOfPages { get; set; }
        public int TotalRecords { get; set; }
        public string AssetModelJson { get; set; }
        public long RenewalId { get; set; }
        public long TicketNumber { get; set; }
        public int TicketCreationStatus { get; set; }
        public int ContractType { get; set; }
        public string ContractTypetxt { get; set; }
        public string ProductJson { get; set; }
        public string ModelJson { get; set; }
        public string RegionJson { get; set; }
        public string CityJson { get; set; }
        public bool IsRejected { get; set; }
        public DateTime PPMDate { get; set; }
        public string isPPMDoneStr { get; set; }

        public bool isPPMDone { get; set; }
        public string PPMJson { get; set; }
        public IEnumerable<AssetsDTO> ProductList { get; set; }
        public IEnumerable<AssetsDTO> AccountList { get; set; }
        public IEnumerable<AssetsDTO> RegionList { get; set; }
        public IEnumerable<AssetsDTO> AssetsList { get; set; }
        public IEnumerable<AssetsDTO> PPMList { get; set; }
        public IEnumerable<AssetsDTO> ModelList { get; set; }
        public IEnumerable<AssetsDTO> AssetModelList { get; set; }
        public IEnumerable<AssetsDTO> RemainingModelList { get; set; }
        public IEnumerable<AssetsDTO> JVMOrderList { get; set; }
        public string RemainingModelsJson { get; set; }
        public IEnumerable<AssetsDTO> CityList { get; set; }
        public IEnumerable<AssetsDTO> UpdatedList { get; set; }
        public string UpdatedJson { get; set; }
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
        public DateTime NextPPMDate { get; set; }
        public bool IsContract { get; set; }
        public string POContract { get; set; }
        public DateTime WarrantyExpiryDate { get; set; }
        public int RemainingCanister { get; set; }
        public DateTime PreviousDate { get; set; }
        public DateTime NewDate { get; set; }
        public long UpdatedId { get; set; }
        public int PPMType { get; set; }
        public string SystemNo { get; set; }
        public bool IsApproved { get; set; }
        public bool All { get; set; }
        public int RoleId { get; set; }
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

        public long UpdatedAMId { get; set; }
        public bool EditMode { get; set; }
        public long APPMId { get; set; }
        public int StatusId { get; set; }
        public string UpdateUserName { get; set; }
        public DateTime ModifiedOn { get; set; }

        public int PageSize { get; set; }
        public int pageNumber { get; set; }
        public long AMModelId { get; set; }
    }
}