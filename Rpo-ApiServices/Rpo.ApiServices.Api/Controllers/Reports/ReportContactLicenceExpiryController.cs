
namespace Rpo.ApiServices.Api.Controllers.Reports
{
    using DataTable;
    using Filters;
    using Model.Models.Enums;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;

    [Authorize]
    public class ReportContactLicenceExpiryController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();
        /// <summary>
        ///  /// <summary>
        /// /// Get the report ofContactLicenceExpiry Report with filter and sorting  
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetReportContactLicenceExpiry([FromUri] ContactLicenceExpiryDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewContactLicenseExpiryReport))
            {
                var contacts = this.rpoContext.ContactLicenses.Where(x =>
                            (x.ContactLicenseType.Name.ToLower() == "site safety coordinator"
                            || x.ContactLicenseType.Name.ToLower() == "concrete safety manager"
                            || x.ContactLicenseType.Name.ToLower() == "site safety manager"
                            || x.ContactLicenseType.Name.ToLower() == "architect"
                            || x.ContactLicenseType.Name.ToLower() == "engineer"
                            || x.ContactLicenseType.Name.ToLower() == "carting"
                            || x.ContactLicenseType.Name.ToLower() == "landmark architect"
                            || x.ContactLicenseType.Name.ToLower() == "registered landscape architect"
                            || x.ContactLicenseType.Name.ToLower() == "expeditor")
                            //&& DbFunctions.TruncateTime(DateTime.SpecifyKind(Convert.ToDateTime(x.ExpirationLicenseDate), DateTimeKind.Utc)) >= DbFunctions.TruncateTime(DateTime.SpecifyKind(Convert.ToDateTime(dataTableParameters.ExpiresFromDate), DateTimeKind.Utc))
                            //  && DbFunctions.TruncateTime(DateTime.SpecifyKind(Convert.ToDateTime(x.ExpirationLicenseDate), DateTimeKind.Utc)) <= DbFunctions.TruncateTime(DateTime.SpecifyKind(Convert.ToDateTime(dataTableParameters.ExpiresToDate), DateTimeKind.Utc))
                            && (DbFunctions.TruncateTime(x.ExpirationLicenseDate) >= DbFunctions.TruncateTime(dataTableParameters.ExpiresFromDate))
                            && (DbFunctions.TruncateTime(x.ExpirationLicenseDate) <= DbFunctions.TruncateTime(dataTableParameters.ExpiresToDate))
                            && ((rpoContext.Jobs.Where(y => y.IdContact == x.IdContact && y.Status == JobStatus.Active).Count() > 0) 
                             || (rpoContext.JobContacts.Where(p => p.IdContact == x.IdContact).Count() > 0))
                            ).AsQueryable();

                if (!string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    contacts = contacts.Where(x => rpoContext.Jobs.Where(y => y.IdContact == x.IdContact && joNumbers.Contains(y.Id) && y.Status == JobStatus.Active).Count() > 0);
                }

                var recordsTotal = contacts.Count();
                var recordsFiltered = recordsTotal;

                var result = contacts
                    .AsEnumerable()
                    .Select(c => this.FormatContactLicenceExpiryReport(c)).Distinct()
                    .AsQueryable()
                    .DataTableParameters(dataTableParameters, out recordsFiltered)
                    .ToArray();


                var data1 = result.Select(x => new
                {
                    x.Company,
                    x.IdContact,
                    x.Contact,
                    x.JobNumber,
                    x.IdCompany,
                    x.ContactLicenseType,
                    x.ExpirationLicenseDate
                }).Distinct().ToList();

                return this.Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Draw,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal,
                    Data = data1.OrderBy(x => x.ExpirationLicenseDate)
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        private ContactLicenceExpiryDTO FormatContactLicenceExpiryReport(ContactLicense contactLicenseResponse)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

            string JobNumber = string.Empty;
            if (contactLicenseResponse.IdContact != null)
            {
                JobNumber = string.Join(", ", rpoContext.Jobs.Where(x => x.IdContact == contactLicenseResponse.IdContact).Select(x => x.JobNumber));
            }

            return new ContactLicenceExpiryDTO
            {
                IdContact = contactLicenseResponse.IdContact,
                IdCompany = contactLicenseResponse.Contact != null ? contactLicenseResponse.Contact.IdCompany : null,
                IdContactLicense = contactLicenseResponse.Id,
                Contact = contactLicenseResponse.Contact != null ? contactLicenseResponse.Contact.FirstName + " " + contactLicenseResponse.Contact.LastName : string.Empty,
                Company = contactLicenseResponse.Contact != null && contactLicenseResponse.Contact.Company != null ? contactLicenseResponse.Contact.Company.Name : string.Empty,
                ContactLicenseType = contactLicenseResponse.ContactLicenseType != null ? contactLicenseResponse.ContactLicenseType.Name : string.Empty,
                ExpirationLicenseDate = contactLicenseResponse != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactLicenseResponse.ExpirationLicenseDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactLicenseResponse.ExpirationLicenseDate,
                JobNumber = JobNumber,
            };
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.rpoContext.Dispose();
            }

            base.Dispose(disposing);
        }
    }


}