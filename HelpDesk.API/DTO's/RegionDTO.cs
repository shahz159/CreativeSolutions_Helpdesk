using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API.DTO_s
{
    public class RegionDTO
    {
        public string RegionName { get; set; }
        public int RegionId { get; set; }
        public bool isActive { get; set; }
        public long CreatedBy { get; set; }
        public int FlagId { get; set; }
        public string message { get; set; }
        public int OrganizationId { get; set; }
    }
}