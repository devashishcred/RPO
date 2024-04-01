using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.ChecklistItems
{
    using System;
    using System.Collections.Generic;
    using Rpo.ApiServices.Model.Models;
    public class ChecklistAddressPropertyMappingDTO
    {
        public int Id { get; set; }
        public int? IdChecklistAddressProperty { get; set; }
        public string ChecklistAddressPropertyName { get; set; }

        public int? IdChecklistItem { get; set; }     
       
        public string Value { get; set; }
        public bool IsActive { get; set; }

    }
}