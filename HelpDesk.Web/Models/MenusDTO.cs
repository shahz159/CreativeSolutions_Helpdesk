using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.Web
{
    public class MenusDTO
    {
        public int MenuId { get; set; }
        public int ParentId { get; set; }
        public int OrderId { get; set; }
        public string MenuName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

    }
}