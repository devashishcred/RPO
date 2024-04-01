using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.Contacts
{

    public class ContactList
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string WorkPhone { get; set; }

        public string Email { get; set; }

        public string ContactLicenseType { get; set; }

        public string LicensesNumber { get; set; }
        public string Notes { get; set; }
    }
}