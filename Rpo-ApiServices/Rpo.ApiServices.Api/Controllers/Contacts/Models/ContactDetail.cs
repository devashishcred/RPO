using System;
using System.Collections.Generic;
using Rpo.ApiServices.Model.Models;

namespace Rpo.ApiServices.Api.Controllers.Contacts
{
    public class ContactDetail
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CompanyName { get; set; }

        public string ContactName { get; set; }

        public int? IdCompany { get; set; }

        public string ItemName { get; set; }

        public string NameWithEmail { get; set; }
    }
}