using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.Customer.Model
{
    public class CustomerPermissionCreateOrUpdate
    {
        public int IdCustomer { get; set; }

        public int[] Permissions { get; set; }
    }
}