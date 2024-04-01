using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rpo.ApiServices.Model.Models.Enums;

namespace Rpo.ApiServices.Api.Controllers.RfpServiceItems
{
    public class ServiceItemDTO
    {
        public int Id { get; set; }

        public string ItemName { get; set; }

        public RfpCostType CostType { get; set; }
    }
}