
namespace Rpo.ApiServices.Api.Controllers.Reports
{
    using DataTable;
    using Filters;
    using Model.Models.Enums;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;

    [Authorize]
    public class ReportContractorInsurancesExpiryController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();
        /// <summary>
        ///  Get the report of ContractorInsurancesExpiry Report with filter and sorting  
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetReportContractorInsurancesExpiry([FromUri] ContractorInsurancesExpiryDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewContractorInsurancesExpiryReport))
            {
                // dataTableParameters.ExpiresFromDate = DateTime.UtcNow;
                //  dataTableParameters.ExpiresToDate = DateTime.UtcNow.AddMonths(1);
                int generalContractor = 13;
            //mb commented 
            //var jobApplicationWorkPermitTypes = this.rpoContext.Jobs.Include("CompanyTypes").Include("CreatedByEmployee").Include("LastModifiedByEmployee")
            //                                    .Where(x =>
            //                                               x.Status == JobStatus.Active  &&
            //                                               x.CompanyTypes.Where(y => y.Id == generalContractor).Count() > 0
            //                                               && (
            //                                               (DbFunctions.TruncateTime(x.TrackingExpiry) >= DbFunctions.TruncateTime(dataTableParameters.TrackingFromDate)
            //                                               && DbFunctions.TruncateTime(x.TrackingExpiry) <= DbFunctions.TruncateTime(dataTableParameters.TrackingToDate))
            //                                                ||
            //                                               // (DbFunctions.TruncateTime(x.HICExpiry) >= DbFunctions.TruncateTime(dataTableParameters.ExpiresFromDate)
            //                                               //&& DbFunctions.TruncateTime(x.HICExpiry) <= DbFunctions.TruncateTime(dataTableParameters.ExpiresToDate))
            //                                               //||
            //                                               (DbFunctions.TruncateTime(x.InsuranceWorkCompensation) >= DbFunctions.TruncateTime(dataTableParameters.DOBWCFromDate)
            //                                               && DbFunctions.TruncateTime(x.InsuranceWorkCompensation) <= DbFunctions.TruncateTime(dataTableParameters.DOBWCToDate))
            //                                               ||
            //                                               (DbFunctions.TruncateTime(x.InsuranceDisability) >= DbFunctions.TruncateTime(dataTableParameters.DOBDIFromDate)
            //                                               && DbFunctions.TruncateTime(x.InsuranceDisability) <= DbFunctions.TruncateTime(dataTableParameters.DOBDIToDate))

            //                                               ||
            //                                               (DbFunctions.TruncateTime(x.InsuranceGeneralLiability) >= DbFunctions.TruncateTime(dataTableParameters.DOBGLFromDate)
            //                                               && DbFunctions.TruncateTime(x.InsuranceGeneralLiability) <= DbFunctions.TruncateTime(dataTableParameters.DOBGLToDate))
            //                                               ||
            //                                                (DbFunctions.TruncateTime(x.DOTInsuranceGeneralLiability) >= DbFunctions.TruncateTime(dataTableParameters.DOTGLFromDate)
            //                                               && DbFunctions.TruncateTime(x.DOTInsuranceGeneralLiability) <= DbFunctions.TruncateTime(dataTableParameters.DOTGLToDate))
            //                                               ||
            //                                               (DbFunctions.TruncateTime(x.DOTInsuranceWorkCompensation) >= DbFunctions.TruncateTime(dataTableParameters.DOTWCFromDate)
            //                                               && DbFunctions.TruncateTime(x.DOTInsuranceWorkCompensation) <= DbFunctions.TruncateTime(dataTableParameters.DOTWCToDate))

            //                                               ||
            //                                               (DbFunctions.TruncateTime(x.InsuranceObstructionBond) >= DbFunctions.TruncateTime(dataTableParameters.DOTSOBFromDate)
            //                                               && DbFunctions.TruncateTime(x.InsuranceObstructionBond) <= DbFunctions.TruncateTime(dataTableParameters.DOTSOBToDate)))
            //                                               ).AsQueryable();
            var contractorInsuaranceExpiryReport = this.rpoContext.Companies.Include("CompanyTypes").Include("CreatedByEmployee").Include("LastModifiedByEmployee")
                                                   .Where(x =>
                                                              //x.Status == JobStatus.Active  &&
                                                              x.CompanyTypes.Where(y => y.Id == generalContractor).Count() > 0
                                                              && (
                                                              (DbFunctions.TruncateTime(x.TrackingExpiry) >= DbFunctions.TruncateTime(dataTableParameters.TrackingFromDate)
                                                              && DbFunctions.TruncateTime(x.TrackingExpiry) <= DbFunctions.TruncateTime(dataTableParameters.TrackingToDate))
                                                               ||
                                                              // (DbFunctions.TruncateTime(x.HICExpiry) >= DbFunctions.TruncateTime(dataTableParameters.ExpiresFromDate)
                                                              //&& DbFunctions.TruncateTime(x.HICExpiry) <= DbFunctions.TruncateTime(dataTableParameters.ExpiresToDate))
                                                              //||
                                                              (DbFunctions.TruncateTime(x.InsuranceWorkCompensation) >= DbFunctions.TruncateTime(dataTableParameters.DOBWCFromDate)
                                                              && DbFunctions.TruncateTime(x.InsuranceWorkCompensation) <= DbFunctions.TruncateTime(dataTableParameters.DOBWCToDate))
                                                              ||
                                                              (DbFunctions.TruncateTime(x.InsuranceDisability) >= DbFunctions.TruncateTime(dataTableParameters.DOBDIFromDate)
                                                              && DbFunctions.TruncateTime(x.InsuranceDisability) <= DbFunctions.TruncateTime(dataTableParameters.DOBDIToDate))

                                                              ||
                                                              (DbFunctions.TruncateTime(x.InsuranceGeneralLiability) >= DbFunctions.TruncateTime(dataTableParameters.DOBGLFromDate)
                                                              && DbFunctions.TruncateTime(x.InsuranceGeneralLiability) <= DbFunctions.TruncateTime(dataTableParameters.DOBGLToDate))
                                                              ||
                                                               (DbFunctions.TruncateTime(x.DOTInsuranceGeneralLiability) >= DbFunctions.TruncateTime(dataTableParameters.DOTGLFromDate)
                                                              && DbFunctions.TruncateTime(x.DOTInsuranceGeneralLiability) <= DbFunctions.TruncateTime(dataTableParameters.DOTGLToDate))
                                                              ||
                                                              (DbFunctions.TruncateTime(x.DOTInsuranceWorkCompensation) >= DbFunctions.TruncateTime(dataTableParameters.DOTWCFromDate)
                                                              && DbFunctions.TruncateTime(x.DOTInsuranceWorkCompensation) <= DbFunctions.TruncateTime(dataTableParameters.DOTWCToDate))

                                                              ||
                                                              (DbFunctions.TruncateTime(x.InsuranceObstructionBond) >= DbFunctions.TruncateTime(dataTableParameters.DOTSOBFromDate)
                                                              && DbFunctions.TruncateTime(x.InsuranceObstructionBond) <= DbFunctions.TruncateTime(dataTableParameters.DOTSOBToDate)))
                                                              ).AsQueryable();

            var recordsTotal = contractorInsuaranceExpiryReport.Count();
            var recordsFiltered = recordsTotal;

            var result = contractorInsuaranceExpiryReport
                .AsEnumerable()
                .Select(c => this.FormatContractorInsurancesExpiryReport(c)).Distinct()
                .AsQueryable()
                .DataTableParameters(dataTableParameters, out recordsFiltered)
                .ToArray();


            var data1 = result.Select(x => new
            {
                x.Company,
                x.Id,
                x.Address,
                x.Address1,
                x.Address2,
                x.City,
                x.State,
                x.PhoneNumber,
                x.InsuranceDisability,
                x.InsuranceWorkCompensation,
                x.InsuranceGeneralLiability,
                x.DOTInsuranceObstructionBond,
                x.DOTInsuranceGeneralLiability,
                x.DOTInsuranceWorkCompensation,
                x.TrackingExpiry,
                x.TrackingNumber,
                x.ZipCode,
                x.IBMNumber,
                //mb
                x.ResponsibilityName

            }).Distinct().ToList();

            return this.Ok(new DataTableResponse
            {
                Draw = dataTableParameters.Draw,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal,
                Data = data1.OrderBy(x => x.TrackingNumber)
            });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        // mb commented
        //private ContractorInsurancesExpiryDTO FormatContractorInsurancesExpiryReport(Job jobResponse)
        //{
        //    string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
        //    string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
        //    //mb resp
        //    string responsibility = jobResponse.Responsibilities != null ? jobResponse.Responsibilities.ResponsibilityName : null;
        //    Address address = jobResponse.Company != null && jobResponse.Addresses != null && jobResponse.Addresses.Count() > 0 ? jobResponse.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
        //    return new ContractorInsurancesExpiryDTO
        //    {
        //        Id = jobResponse.IdCompany,
        //        Company = jobResponse.Company != null ? jobResponse.Name : string.Empty,
        //        Address = address != null ? address.Address1 +
        //        (!string.IsNullOrEmpty(address.Address2) ? " " + address.Address2 : string.Empty)
        //        + (!string.IsNullOrEmpty(address.City) ? " " + address.City : string.Empty)
        //        + (address.State != null && !string.IsNullOrEmpty(address.State.Acronym) ? " " + address.State.Acronym : string.Empty)
        //        + (!string.IsNullOrEmpty(address.ZipCode) ? " " + address.ZipCode : string.Empty)
        //        : string.Empty,
        //        Address1 = address != null ? address.Address1 : string.Empty,
        //        Address2 = address != null ? address.Address2 : string.Empty,
        //        City = address != null ? address.City : string.Empty,
        //        State = address != null && address.State != null ? address.State.Acronym : string.Empty,
        //        ZipCode = address != null ? address.ZipCode : string.Empty,
        //        PhoneNumber = address != null ? address.Phone : string.Empty,
        //        TrackingNumber = jobResponse.Company != null ? jobResponse.TrackingNumber : string.Empty,
        //        //TrackingExpiry = jobResponse.TrackingExpiry != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobResponse.TrackingExpiry), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobResponse.TrackingExpiry,
        //        TrackingExpiry = jobResponse.TrackingExpiry != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobResponse.TrackingExpiry), DateTimeKind.Utc) : jobResponse.TrackingExpiry,
        //        IBMNumber = jobResponse.Company != null ? jobResponse.IBMNumber : string.Empty,
        //        //InsuranceWorkCompensation = jobResponse.InsuranceWorkCompensation != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobResponse.InsuranceWorkCompensation), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobResponse.InsuranceWorkCompensation,
        //        InsuranceWorkCompensation = jobResponse.InsuranceWorkCompensation != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobResponse.InsuranceWorkCompensation), DateTimeKind.Utc) : jobResponse.InsuranceWorkCompensation,
        //        //InsuranceDisability = jobResponse.InsuranceDisability != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobResponse.InsuranceDisability), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobResponse.InsuranceDisability,
        //        InsuranceDisability = jobResponse.InsuranceDisability != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobResponse.InsuranceDisability), DateTimeKind.Utc) : jobResponse.InsuranceDisability,
        //        //InsuranceGeneralLiability = jobResponse.InsuranceGeneralLiability != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobResponse.InsuranceGeneralLiability), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobResponse.InsuranceGeneralLiability,
        //        InsuranceGeneralLiability = jobResponse.InsuranceGeneralLiability != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobResponse.InsuranceGeneralLiability), DateTimeKind.Utc) : jobResponse.InsuranceGeneralLiability,
        //        DOTInsuranceObstructionBond = jobResponse.InsuranceObstructionBond != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobResponse.InsuranceObstructionBond), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobResponse.InsuranceObstructionBond,
        //        DOTInsuranceWorkCompensation = jobResponse.DOTInsuranceWorkCompensation != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobResponse.DOTInsuranceWorkCompensation), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobResponse.DOTInsuranceWorkCompensation,
        //        DOTInsuranceGeneralLiability = jobResponse.DOTInsuranceGeneralLiability != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobResponse.DOTInsuranceGeneralLiability), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobResponse.DOTInsuranceGeneralLiability,
        //        //mb resp
        //        ResponsibilityName = jobResponse.Responsibilities != null ? jobResponse.Responsibilities.ResponsibilityName : null
        //    };
        //}

        private ContractorInsurancesExpiryDTO FormatContractorInsurancesExpiryReport(Company companyResponse)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
          
            string responsibility = companyResponse.Responsibilities != null ? companyResponse.Responsibilities.ResponsibilityName : null;
            Address address = companyResponse.Addresses != null && companyResponse.Addresses != null && companyResponse.Addresses.Count() > 0 ? companyResponse.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
            return new ContractorInsurancesExpiryDTO
            {
                Id = companyResponse.Id,
                Company = companyResponse.Name != null ? companyResponse.Name : string.Empty,
                Address = address != null ? address.Address1 +
                (!string.IsNullOrEmpty(address.Address2) ? " " + address.Address2 : string.Empty)
                + (!string.IsNullOrEmpty(address.City) ? " " + address.City : string.Empty)
                + (address.State != null && !string.IsNullOrEmpty(address.State.Acronym) ? " " + address.State.Acronym : string.Empty)
                + (!string.IsNullOrEmpty(address.ZipCode) ? " " + address.ZipCode : string.Empty)
                : string.Empty,
                Address1 = address != null ? address.Address1 : string.Empty,
                Address2 = address != null ? address.Address2 : string.Empty,
                City = address != null ? address.City : string.Empty,
                State = address != null && address.State != null ? address.State.Acronym : string.Empty,
                ZipCode = address != null ? address.ZipCode : string.Empty,
                PhoneNumber = address != null ? address.Phone : string.Empty,
                TrackingNumber = companyResponse.TrackingNumber != null ? companyResponse.TrackingNumber : string.Empty,
                //TrackingExpiry = companyResponse.TrackingExpiry != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyResponse.TrackingExpiry), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyResponse.TrackingExpiry,
                TrackingExpiry = companyResponse.TrackingExpiry != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyResponse.TrackingExpiry), DateTimeKind.Utc) : companyResponse.TrackingExpiry,
                IBMNumber = companyResponse.IBMNumber != null ? companyResponse.IBMNumber : string.Empty,
                //InsuranceWorkCompensation = companyResponse.InsuranceWorkCompensation != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyResponse.InsuranceWorkCompensation), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyResponse.InsuranceWorkCompensation,
                InsuranceWorkCompensation = companyResponse.InsuranceWorkCompensation != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyResponse.InsuranceWorkCompensation), DateTimeKind.Utc) : companyResponse.InsuranceWorkCompensation,
                //InsuranceDisability = companyResponse.InsuranceDisability != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyResponse.InsuranceDisability), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyResponse.InsuranceDisability,
                InsuranceDisability = companyResponse.InsuranceDisability != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyResponse.InsuranceDisability), DateTimeKind.Utc) : companyResponse.InsuranceDisability,
                //InsuranceGeneralLiability = companyResponse.InsuranceGeneralLiability != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyResponse.InsuranceGeneralLiability), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyResponse.InsuranceGeneralLiability,
                InsuranceGeneralLiability = companyResponse.InsuranceGeneralLiability != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyResponse.InsuranceGeneralLiability), DateTimeKind.Utc) : companyResponse.InsuranceGeneralLiability,
                DOTInsuranceObstructionBond = companyResponse.InsuranceObstructionBond != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyResponse.InsuranceObstructionBond), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyResponse.InsuranceObstructionBond,
                DOTInsuranceWorkCompensation = companyResponse.DOTInsuranceWorkCompensation != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyResponse.DOTInsuranceWorkCompensation), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyResponse.DOTInsuranceWorkCompensation,
                DOTInsuranceGeneralLiability = companyResponse.DOTInsuranceGeneralLiability != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyResponse.DOTInsuranceGeneralLiability), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyResponse.DOTInsuranceGeneralLiability,
                //mb resp
                ResponsibilityName = companyResponse.Responsibilities != null ? companyResponse.Responsibilities.ResponsibilityName : null
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