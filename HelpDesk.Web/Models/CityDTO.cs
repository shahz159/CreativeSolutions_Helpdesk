using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelpDesk.Web.Models
{
    public class CityDTO
    {
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max Length is 100")]
        public string CityName { get; set; }
        public int CityId { get; set; }
        public string RegionName { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        public int RegionId { get; set; }
        public bool isActive { get; set; }
        public long CreatedBy { get; set; }
        public int FlagId { get; set; }
        public string message { get; set; }
        public int OrganizationId { get; set; }

        public IEnumerable<CityDTO> CityLst { get; set; }
        public IEnumerable<CityDTO> RegionLst { get; set; }
    }
}