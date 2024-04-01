using Rpo.ApiServices.Model.Models;
using System;

namespace Rpo.ApiServices.Api.Controllers.Contacts
{
    public class ContactLicenseDTODetail
    {
        public int Id { get; set; }

        public virtual int IdContactLicenseType { get; set; }

        public ContactLicenseType ContactLicenseType { get; set; }

        public string Number { get; set; }

        public DateTime? ExpirationLicenseDate { get; set; }
    }
}
