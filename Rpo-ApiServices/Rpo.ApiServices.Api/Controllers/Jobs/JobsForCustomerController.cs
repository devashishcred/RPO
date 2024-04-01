// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Mital Bhatt
// Created          : 27-11-2023
//
// Last Modified By : Mital Bhatt
// Last Modified On : 27-11-2023
// ***********************************************************************
// <copyright file="JobsForCustomerController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class JobsForCustomer Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Jobs namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobsForCustomer
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Employees;
    using GlobalSearch;
    using Hubs;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Filters;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Model.Models.Enums;
    using SystemSettings;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using Microsoft.ApplicationBlocks.Data;
    using NPOI.XSSF.UserModel;
    using NPOI.SS.UserModel;
    using Rpo.ApiServices.Api.Controllers.JobsForCustomer.Models;

    public class JobsForCustomerController : HubApiController<GroupHub>
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();
        /// <summary>
        /// Gets the jobs.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Get the jobs List.</returns>
        [ResponseType(typeof(JobsAdvancedSearchParameters))]
        [Authorize]
        [RpoAuthorize]       
        public IHttpActionResult GetJobsForCustomer([FromUri] JobsAdvancedSearchParameters dataTableParameters)
        {
            var employee = this.rpoContext.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewJob)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJob)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteJob))
            {
                var recordsTotal = rpoContext.Jobs.Count();
                var recordsFiltered = recordsTotal;

                List<int> objcompanycontactjobs = new List<int>();

                if (dataTableParameters.IdCompany != null)
                {
                    objcompanycontactjobs.AddRange(rpoContext.JobContacts.Where(j => j.IdCompany == dataTableParameters.IdCompany).Select(d => d.IdJob).ToList());
                }

                if (dataTableParameters.IdContact != null)
                {
                    objcompanycontactjobs.AddRange(rpoContext.JobContacts.Where(j => j.IdContact == dataTableParameters.IdContact).Select(d => d.IdJob).ToList());
                }

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                IQueryable<Job> jobs = rpoContext.Jobs.Include("RfpAddress.Borough");

                if (dataTableParameters.GlobalSearchType != null && dataTableParameters.GlobalSearchType > 0)
                {
                    switch ((GlobalSearchType)dataTableParameters.GlobalSearchType)
                    {
                        case GlobalSearchType.ApplicationNumber:
                            int dobApplicationType = Enums.ApplicationType.DOB.GetHashCode();
                            var application = rpoContext.JobApplications.Include("JobApplicationType").Where(x => x.ApplicationNumber.Contains(dataTableParameters.GlobalSearchText.Trim()) && x.JobApplicationType != null && x.JobApplicationType.IdParent == dobApplicationType).Select(x => x.IdJob).Distinct().ToList();
                            jobs = jobs.Where(x => application.Contains(x.Id));
                            break;
                        case GlobalSearchType.JobNumber:
                            jobs = jobs.Where(x => x.JobNumber.Contains(dataTableParameters.GlobalSearchText.Trim()));
                            break;
                        case GlobalSearchType.Address:

                            foreach (var item in dataTableParameters.GlobalSearchText.Split(','))
                            {
                                jobs = jobs.Where(x => x.RfpAddress != null && String.Concat(x.RfpAddress.HouseNumber + " " + x.RfpAddress.Street).Contains(item.Trim())
                                // (x.RfpAddress != null && x.RfpAddress.HouseNumber.Contains(item.Trim()) || x.RfpAddress.Street.Contains(item.Trim()))) ;
                                // || (x.RfpAddress != null && x.RfpAddress.Street.Contains(item.Trim()))
                                || (x.RfpAddress != null && x.RfpAddress.Borough != null && x.RfpAddress.Borough.Description.Contains(item.Trim())));
                                // || (x.RfpAddress != null && x.RfpAddress.ZipCode.Contains(item.Trim())));
                            }


                            break;
                        case GlobalSearchType.SpecialPlaceName:
                            jobs = jobs.Where(x => x.SpecialPlace.Contains(dataTableParameters.GlobalSearchText.Trim()));
                            break;
                        case GlobalSearchType.TransmittalNumber:
                            var jobTransmittal = rpoContext.JobTransmittals.Where(x => x.TransmittalNumber.Contains(dataTableParameters.GlobalSearchText.Trim())).Select(x => x.IdJob).ToList();
                            jobs = jobs.Where(x => jobTransmittal.Contains(x.Id));
                            break;
                        case GlobalSearchType.ZoneDistrict:
                            jobs = jobs.Where(x => x.RfpAddress.ZoneDistrict.Contains(dataTableParameters.GlobalSearchText.Trim()));
                            break;
                        case GlobalSearchType.Overlay:
                            jobs = jobs.Where(x => x.RfpAddress.Overlay.Contains(dataTableParameters.GlobalSearchText.Trim()));
                            break;
                        case GlobalSearchType.TrackingNumber:
                            int dotApplicationType = Enums.ApplicationType.DOT.GetHashCode();
                            var trackingApplication = rpoContext.JobApplications.Include("JobApplicationType").Where(x => x.ApplicationNumber.Contains(dataTableParameters.GlobalSearchText.Trim()) && x.JobApplicationType != null && x.JobApplicationType.IdParent == dotApplicationType).Select(x => x.IdJob).Distinct().ToList();
                            jobs = jobs.Where(x => trackingApplication.Contains(x.Id));
                            break;
                        //case GlobalSearchType.ViolationNumber:
                        //    var jobViolation = rpoContext.JobViolations.Where(x => x.SummonsNumber.Contains(dataTableParameters.GlobalSearchText.Trim())).Select(x => x.IdJob ?? 0).Distinct().ToList();
                        //    jobs = jobs.Where(x => jobViolation.Contains(x.Id));
                        //    break;
                        case GlobalSearchType.ViolationNumber: //mb
                            var jobViolation = rpoContext.JobViolations.Where(x => x.SummonsNumber.Contains(dataTableParameters.GlobalSearchText.Trim())).Select(x => x.BinNumber).Distinct().ToList();
                         var rfpaddresses=  jobViolation!=null?rpoContext.RfpAddresses.Where(x => jobViolation.Contains(x.BinNumber)).ToList().Select(x=>x.Id):null;
                         var idjobs= rfpaddresses!=null?rpoContext.Jobs.Where(x => rfpaddresses.Contains(x.IdRfpAddress)).ToList().Select(y => y.Id):null;
                         var projectaccess = rpoContext.CustomerJobAccess.Where(x => x.IdCustomer == employee.Id).ToList().Select(y=>y.IdJob);
                            jobs = jobs.Where(x => rfpaddresses.Contains(x.IdRfpAddress) && projectaccess.Contains(x.Id));
                            break;
                    }
                }
                //else
                //{
                if (!string.IsNullOrWhiteSpace(dataTableParameters.JobNumber))
                {
                    //jobs = jobs.Where(j => j.JobNumber == dataTableParameters.JobNumber);
                    var projectaccess = rpoContext.CustomerJobAccess.Where(x => x.IdCustomer == employee.Id).ToList().Select(y => y.IdJob);
                 var idjobs=   rpoContext.Jobs.Where(x => projectaccess.Contains(x.Id)).ToList().Select(y=>y.JobNumber);
                    jobs = jobs.Where(j =>
                     idjobs.Contains(dataTableParameters.JobNumber));
                }
                else
                {
                    if ((dataTableParameters.OnlyMyJobs == null || dataTableParameters.OnlyMyJobs.Value))
                    {
                        //if (employee != null)
                        //{
                        //    string employeeId = Convert.ToString(employee.Id);
                        //    jobs = jobs.Where(j =>
                        //    (
                        //        (
                        //        j.DOTProjectTeam == employeeId || j.DOTProjectTeam.StartsWith(employeeId + ",") || j.DOTProjectTeam.Contains("," + employeeId + ",") || j.DOTProjectTeam.EndsWith("," + employeeId)
                        //        )
                        //        && j.DOTProjectTeam != null && j.DOTProjectTeam != ""
                        //    ) ||
                        //    (
                        //        (
                        //        j.DOBProjectTeam == employeeId || j.DOBProjectTeam.StartsWith(employeeId + ",") || j.DOBProjectTeam.Contains("," + employeeId + ",") || j.DOBProjectTeam.EndsWith("," + employeeId)
                        //        )
                        //        && j.DOBProjectTeam != null && j.DOBProjectTeam != ""
                        //    ) ||
                        //    (
                        //        (
                        //        j.ViolationProjectTeam == employeeId || j.ViolationProjectTeam.StartsWith(employeeId + ",") || j.ViolationProjectTeam.Contains("," + employeeId + ",") || j.ViolationProjectTeam.EndsWith("," + employeeId)
                        //        )
                        //        && j.ViolationProjectTeam != null && j.ViolationProjectTeam != ""
                        //    ) ||
                        //    (
                        //        (
                        //        j.DEPProjectTeam == employeeId || j.DEPProjectTeam.StartsWith(employeeId + ",") || j.DEPProjectTeam.Contains("," + employeeId + ",") || j.DEPProjectTeam.EndsWith("," + employeeId)
                        //        )
                        //        && j.DEPProjectTeam != null && j.DEPProjectTeam != ""
                        //    ) ||
                        //    j.IdProjectManager == employee.Id);
                        //}
                        if(employee!=null)
                        {
                           var projectaccess= rpoContext.CustomerJobAccess.Where(x => x.IdCustomer == employee.Id).ToList().Select(y=>y.IdJob);
                            jobs = jobs.Where(j =>                                                       
                             projectaccess.Contains(j.Id));
                        }
                    }

                    if (dataTableParameters.IdRfpAddress != null)
                    {
                        jobs = jobs.Where(j => j.IdRfpAddress == dataTableParameters.IdRfpAddress);
                    }

                    if (!string.IsNullOrWhiteSpace(dataTableParameters.Apt))
                    {
                        jobs = jobs.Where(j => j.Apartment.Contains(dataTableParameters.Apt));
                    }

                    if (dataTableParameters.Borough != null)
                    {
                        jobs = jobs.Where(j => j.IdBorough == dataTableParameters.Borough);
                    }

                    if (dataTableParameters.Client != null)
                    {
                        jobs = jobs.Where(j => j.IdCompany == dataTableParameters.Client);
                    }

                    if (!string.IsNullOrWhiteSpace(dataTableParameters.Floor))
                    {
                        jobs = jobs.Where(j => j.FloorNumber.Contains(dataTableParameters.Floor));
                    }

                    if (dataTableParameters.HolidayEmbargo.HasValue)
                    {
                        jobs = jobs.Where(j => j.HasHolidayEmbargo == dataTableParameters.HolidayEmbargo.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(dataTableParameters.HouseNumber))
                    {
                        jobs = jobs.Where(j => j.HouseNumber.Contains(dataTableParameters.HouseNumber) || j.RfpAddress.HouseNumber.Contains(dataTableParameters.HouseNumber));
                    }

                    if (dataTableParameters.IsLandmark != null)
                    {
                        jobs = jobs.Where(j => j.HasLandMarkStatus == dataTableParameters.IsLandmark);
                    }

                    //if (dataTableParameters.JobStartDate != null && dataTableParameters.JobEndDate != null)
                    //{
                    //    dataTableParameters.JobEndDate = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(dataTableParameters.JobEndDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
                    //    dataTableParameters.JobStartDate = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(dataTableParameters.JobStartDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));

                    //    jobs = jobs.Where(j => (DbFunctions.TruncateTime(j.StartDate) >= DbFunctions.TruncateTime(dataTableParameters.JobStartDate)) &&
                    //    (DbFunctions.TruncateTime(j.EndDate) <= DbFunctions.TruncateTime(dataTableParameters.JobEndDate))
                    //    );
                    //}
                    if (dataTableParameters.JobStartDate != null)
                    {
                        dataTableParameters.JobStartDate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(dataTableParameters.JobStartDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)); // TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(dataTableParameters.JobStartDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
                        jobs = jobs.Where(j => DbFunctions.TruncateTime(j.StartDate) == DbFunctions.TruncateTime(dataTableParameters.JobStartDate));
                    }
                    if (dataTableParameters.JobEndDate != null)
                    {
                        dataTableParameters.JobEndDate = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(dataTableParameters.JobEndDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
                        jobs = jobs.Where(j => DbFunctions.TruncateTime(j.EndDate) == DbFunctions.TruncateTime(dataTableParameters.JobEndDate));
                    }

                    if (dataTableParameters.JobStatus != null)
                    {
                        jobs = jobs.Where(j => j.Status == dataTableParameters.JobStatus);
                    }

                    if (dataTableParameters.LittleE.HasValue)
                    {
                        jobs = jobs.Where(j => j.HasEnvironmentalRestriction == dataTableParameters.LittleE.Value);
                    }

                    //if (dataTableParameters.ProjectCoordinator != null)
                    //{
                    //    jobs = jobs.Where(j => j.IdProjectCoordinator == dataTableParameters.ProjectCoordinator);
                    //}

                    if (dataTableParameters.ProjectManager != null)
                    {
                        jobs = jobs.Where(j => j.IdProjectManager == dataTableParameters.ProjectManager);
                    }
                    if (!string.IsNullOrWhiteSpace(dataTableParameters.OtherTeamMember) && dataTableParameters.OtherTeamMember != "null")
                    //if (!string.IsNullOrWhiteSpace(dataTableParameters.OtherTeamMember))
                    {
                        jobs = jobs.Where(job =>
                        (job.DOBProjectTeam == dataTableParameters.OtherTeamMember || job.DOBProjectTeam.StartsWith(dataTableParameters.OtherTeamMember + ",") || job.DOBProjectTeam.Contains("," + dataTableParameters.OtherTeamMember + ",") || job.DOBProjectTeam.EndsWith("," + dataTableParameters.OtherTeamMember))
                        || (job.DOTProjectTeam == dataTableParameters.OtherTeamMember || job.DOTProjectTeam.StartsWith(dataTableParameters.OtherTeamMember + ",") || job.DOTProjectTeam.Contains("," + dataTableParameters.OtherTeamMember + ",") || job.DOTProjectTeam.EndsWith("," + dataTableParameters.OtherTeamMember))
                        || (job.DEPProjectTeam == dataTableParameters.OtherTeamMember || job.DEPProjectTeam.StartsWith(dataTableParameters.OtherTeamMember + ",") || job.DEPProjectTeam.Contains("," + dataTableParameters.OtherTeamMember + ",") || job.DEPProjectTeam.EndsWith("," + dataTableParameters.OtherTeamMember))
                        || (job.ViolationProjectTeam == dataTableParameters.OtherTeamMember || job.ViolationProjectTeam.StartsWith(dataTableParameters.OtherTeamMember + ",") || job.ViolationProjectTeam.Contains("," + dataTableParameters.OtherTeamMember + ",") || job.ViolationProjectTeam.EndsWith("," + dataTableParameters.OtherTeamMember))
                        );
                    }

                    if (!string.IsNullOrWhiteSpace(dataTableParameters.SpecialPlaceName))
                    {
                        jobs = jobs.Where(j => j.SpecialPlace.Contains(dataTableParameters.SpecialPlaceName));
                    }

                    if (!string.IsNullOrWhiteSpace(dataTableParameters.Street))
                    {
                        jobs = jobs.Where(j => j.StreetNumber.Contains(dataTableParameters.Street) || j.RfpAddress.Street.Contains(dataTableParameters.Street));
                    }

                    if (dataTableParameters.IdCompany != null)
                    {
                        jobs = jobs.Where(j => (j.IdCompany == dataTableParameters.IdCompany) || objcompanycontactjobs.Contains(j.Id));
                    }

                    if (dataTableParameters.IdContact != null)
                    {
                        jobs = jobs.Where(j => (j.IdContact == dataTableParameters.IdContact) || objcompanycontactjobs.Contains(j.Id));
                    }
                    //ML
                    if (dataTableParameters.IdReferredByCompany != null)
                    {
                        jobs = jobs.Where(j => (j.IdReferredByCompany == dataTableParameters.IdReferredByCompany));
                    }

                    if (dataTableParameters.IdReferredByContact != null)
                    {
                        jobs = jobs.Where(j => (j.IdReferredByContact == dataTableParameters.IdReferredByContact));
                    }


                    if (dataTableParameters.IdJobTypes != null)
                    {
                        List<int> jobTypes = dataTableParameters.IdJobTypes != null && !string.IsNullOrEmpty(dataTableParameters.IdJobTypes) ? (dataTableParameters.IdJobTypes.Split('-') != null && dataTableParameters.IdJobTypes.Split('-').Any() ? dataTableParameters.IdJobTypes.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

                        int idJobTypeDOB = 0;
                        int idJobTypeDOT = 0;
                        int idJobTypeDEP = 0;
                        int idJobTypeViolation = 0;

                        foreach (var item in jobTypes)
                        {
                            switch (item)
                            {
                                case 1:
                                    idJobTypeDOB = item;
                                    jobs = jobs.Where(x => x.JobApplicationTypes.Where(y => y.Id == idJobTypeDOB).Count() > 0);
                                    break;
                                case 2:
                                    idJobTypeDOT = item;
                                    jobs = jobs.Where(x => x.JobApplicationTypes.Where(y => y.Id == idJobTypeDOT).Count() > 0);
                                    break;
                                case 3:
                                    idJobTypeDEP = item;
                                    jobs = jobs.Where(x => x.JobApplicationTypes.Where(y => y.Id == idJobTypeDEP).Count() > 0);
                                    break;
                                case 4:
                                    idJobTypeViolation = item;
                                    jobs = jobs.Where(x => x.JobApplicationTypes.Where(y => y.Id == idJobTypeViolation).Count() > 0);
                                    break;
                            }
                        }
                    }
                }
                //}
                int idcontact = 0;
                if (dataTableParameters.IdContact != null)
                {
                    idcontact = (int)dataTableParameters.IdContact;
                }


                var result = jobs
                   .AsEnumerable()
                   .Select(j => Format(j, idcontact, 0))
                   .AsQueryable()
                   .DataTableParameters(dataTableParameters, out recordsFiltered)
                   .ToArray()
                   .OrderByDescending(x => x.LastModiefiedDate);


                return Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Draw,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal,
                    Data = result.Distinct()
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        private JobsForCustomerDTO Format(Job j, int IdContact, int parentStatusId = 0)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            string jobApplicationType = string.Empty;

            if (j.JobApplicationTypes != null && j.JobApplicationTypes.Count() > 0)
            {
                jobApplicationType = string.Join(",", j.JobApplicationTypes.Select(x => x.Id.ToString()));
            }
            string ContactEmail = rpoContext.Contacts.Where(x => x.Id == IdContact).Select(x => x.Email).FirstOrDefault();
            //  var Customer = rpoContext.Customers.Where(x => x.IdContcat == IdContact).FirstOrDefault();
            var Customer = rpoContext.Customers.Where(x => x.EmailAddress == ContactEmail).FirstOrDefault();
            string IsAuthorized = "Un-Authorized";
            if (Customer != null)
            {
                if (rpoContext.CustomerJobAccess.Any(x => x.IdJob == j.Id && x.IdCustomer == Customer.Id))
                {
                    IsAuthorized = "Authorized";
                }
            }
            return new JobsForCustomerDTO
            {
                Id = j.Id,
                JobNumber = j.JobNumber,
                PoNumber = j.PONumber,
                Status = j.Status,
                StatusDescription = j.Status == JobStatus.Active ? "Active" : (j.Status == JobStatus.Hold) ? "Hold" : "Close",
                IdRfpAddress = j.IdRfpAddress,
                IdRfp = j.IdRfp,
                RfpAddress = j.RfpAddress.Street,
                ZipCode = j.RfpAddress != null ? j.RfpAddress.ZipCode : null,
                IdBorough = j.RfpAddress != null ? j.RfpAddress.IdBorough : null,
                Borough = j.RfpAddress != null && j.RfpAddress.Borough != null ? j.RfpAddress.Borough.Description : null,
                HouseNumber = j.RfpAddress != null ? j.RfpAddress.HouseNumber : string.Empty,
                StreetNumber = j.RfpAddress != null ? j.RfpAddress.Street : string.Empty,
                FloorNumber = j.FloorNumber,
                Apartment = j.Apartment,
                SpecialPlace = j.SpecialPlace,
                Block = j.Block,
                Lot = j.Lot,
                BinNumber = j.RfpAddress != null ? j.RfpAddress.BinNumber : null,
                HasLandMarkStatus = j.HasLandMarkStatus,
                HasEnvironmentalRestriction = j.HasEnvironmentalRestriction,
                HasOpenWork = j.HasOpenWork,
                IdCompany = j.IdCompany,
                Company = j.Company != null ? j.Company.Name : string.Empty,
                IdContact = j.IdContact,
                //Contact = j.Contact != null ? j.Contact.FirstName + " " + j.Contact.LastName : string.Empty,
                Contact = j.Contact != null ? (j.Contact.FirstName + (!string.IsNullOrWhiteSpace(j.Contact.LastName) ? " " + j.Contact.LastName + (j.Contact.Suffix != null ? " " + j.Contact.Suffix.Description : string.Empty) : string.Empty)) : string.Empty,
                LastModiefiedDate = j.LastModiefiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(j.LastModiefiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : j.LastModiefiedDate,
                IdJobContactType = j.IdJobContactType,
                JobContactTypeDescription = j.JobContactType != null && j.JobContactType.Name != null ? j.JobContactType.Name : string.Empty,
                IdProjectManager = j.IdProjectManager,
                ProjectManager = j.ProjectManager != null ? j.ProjectManager.FirstName + " " + j.ProjectManager.LastName : string.Empty,
                StartDate = j.StartDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(j.StartDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : j.StartDate,
                EndDate = j.EndDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(j.EndDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : j.EndDate,
                ParentStatusId = parentStatusId,
                JobApplicationType = jobApplicationType,
                OCMCNumber = j.OCMCNumber,
                StreetWorkingFrom = j.StreetWorkingFrom,
                StreetWorkingOn = j.StreetWorkingOn,
                StreetWorkingTo = j.StreetWorkingTo,
                QBJobName = j.QBJobName,
                //Address = j.RfpAddress != null ?
                //                           (j.RfpAddress.HouseNumber + " " + (j.RfpAddress.Street != null ? j.RfpAddress.Street + ", " : string.Empty)
                //                         + (j.RfpAddress.Borough != null ? j.RfpAddress.Borough.Description : string.Empty)) : string.Empty,
                Address = j.RfpAddress != null ?
                                           (j.RfpAddress.HouseNumber + " " + (j.RfpAddress.Street != null ? j.RfpAddress.Street + ", " : string.Empty)
                                         + (j.RfpAddress.Borough != null ? j.RfpAddress.Borough.Description + " " : string.Empty)
                                         + (j.FloorNumber != null ? j.FloorNumber : string.Empty)) : string.Empty,
                IdReferredByCompany = j.IdReferredByCompany,
                IdReferredByContact = j.IdReferredByContact,
                //mb
                JobStatusNotes = j.JobStatusNotes,
                IsAuthorized = IsAuthorized


            };
        }

        public GlobalSearchResponse GetGlobalSearchForCustomer(int globalSearchType, string searchText)
        {
            var employee = this.rpoContext.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return new GlobalSearchResponse();
            }

            GlobalSearchResponse globalSearchResponse = new GlobalSearchResponse();
            switch ((GlobalSearchType)globalSearchType)
            {
                //Search by the Application Number in to jobapplication
                //case GlobalSearchType.ApplicationNumber:
                //    int dobApplicationType = ApplicationType.DOB.GetHashCode();
                //    var application = rpoContext.JobApplications.Include("JobApplicationType").Where(x => x.ApplicationNumber.Contains(searchText.Trim()) && x.JobApplicationType != null && x.JobApplicationType.IdParent == dobApplicationType).Select(x => x.IdJob).Distinct().ToList();
                //    globalSearchResponse.SearchResult = application;
                //    break;
                // get the result of selected companies wise 
                case GlobalSearchType.CompanyName:
                    var company = rpoContext.Companies.Where(x => x.Name.Contains(searchText.Trim())).Select(x => x.Id).ToList();
                    globalSearchResponse.SearchResult = company;
                    break;
                // get the result of selected contact wise
                case GlobalSearchType.ContactName:
                    var contact = rpoContext.Contacts.AsEnumerable();
                    searchText = searchText.ToLower();
                    foreach (var item in searchText.Split(' '))
                    {
                        contact = contact.Where(x =>
                           (x.FirstName != null && x.FirstName.ToLower().Contains(item.Trim().ToLower()))
                        || (x.LastName != null && x.LastName.ToLower().Contains(item.Trim().ToLower()))
                        || (x.MiddleName != null && x.MiddleName.ToLower().Contains(item.Trim().ToLower())));
                    }

                    globalSearchResponse.SearchResult = contact.Select(x => x.Id).ToList();
                    break;
                // get the result of selected jobnumber
                case GlobalSearchType.JobNumber:
                    var job = rpoContext.Jobs.Where(x => x.JobNumber.Contains(searchText.Trim())).Select(x => x.Id).ToList();
                   var customerjobaccess= rpoContext.CustomerJobAccess.Where(x => x.IdCustomer == employee.Id).ToList().Select(y=>y.IdJob);
                    foreach(var j in job)
                    {
                        if(customerjobaccess.Contains(j))
                        {
                            globalSearchResponse.SearchResult.Add(j);
                        }
                    }
                   // globalSearchResponse.SearchResult = job;
                    break;
                // get the result of selected RFPNumber
                case GlobalSearchType.RFPNumber:
                    var rfp = rpoContext.Rfps.Where(x => x.RfpNumber.Contains(searchText.Trim())).Select(x => x.Id).ToList();
                    globalSearchResponse.SearchResult = rfp;
                    break;
                // get the result of selected Address
                case GlobalSearchType.Address:
                    string[] strArray = searchText.Split(',');
                    string address = string.Empty;
                    string borough = string.Empty;
                    //if(strArray.Count()>0)
                    //{
                    //    address = searchText.Split(',')[0].Trim();
                    //}
                    //if (strArray.Count() > 1)
                    //{
                    //    borough = searchText.Split(',')[1].Trim();
                    //}
                    var jobAddress = rpoContext.Jobs.Include("RfpAddress.Borough").Select(x => new
                    {
                        Id = x.Id,
                        //HouseNumber = (x.RfpAddress != null ? x.RfpAddress.HouseNumber : string.Empty),
                        //Street = (x.RfpAddress != null ? x.RfpAddress.Street : string.Empty),
                        Borough = (x.RfpAddress != null && x.RfpAddress.Borough != null ? x.RfpAddress.Borough.Description : string.Empty).Trim(),
                        ZipCode = (x.RfpAddress != null ? x.RfpAddress.ZipCode : string.Empty),
                        NewAddress = (x.RfpAddress != null ? x.RfpAddress.HouseNumber + " " + x.RfpAddress.Street : string.Empty).Trim()
                        // }).Where(x => x.HouseNumber.Trim().ToLower().Equals(searchText.Trim().ToLower()) || x.Street.Trim().ToLower().Equals(searchText.Trim().ToLower()) || (x.NewAddress+", "+ x.Borough).ToLower().Trim().Equals(searchText.ToLower().Trim()) || x.ZipCode.Trim().ToLower().Equals(searchText.Trim().ToLower()) || x.Borough.Trim().ToLower().Equals((!string.IsNullOrEmpty(searchText) ? searchText.Trim().ToLower() : ""))).AsQueryable();
                    }).Where(x => (x.NewAddress + ", " + x.Borough).ToLower().Trim().Contains(searchText.ToLower().Trim())).AsQueryable();

                    #region ML Changes
                    //var jobAddress = db.Jobs.Include("RfpAddress.Borough").AsQueryable();
                    //foreach (var item in searchText.Split(','))
                    //{
                    //    jobAddress = jobAddress.Where(x =>
                    //       (x.RfpAddress != null && x.RfpAddress.HouseNumber.Contains(item.Trim()))
                    //    || (x.RfpAddress != null && x.RfpAddress.Street.Contains(item.Trim()))
                    //    || (x.RfpAddress != null && x.RfpAddress.Borough != null && x.RfpAddress.Borough.Description.Contains(item.Trim()))
                    //    || (x.RfpAddress != null && x.RfpAddress.ZipCode.Contains(item.Trim())));
                    //}
                    #endregion 
                    globalSearchResponse.SearchResult = jobAddress.Select(x => x.Id).ToList();

                    break;
                // get the result of selected specialplaceName
                case GlobalSearchType.SpecialPlaceName:
                    var jobSpecialPlace = rpoContext.Jobs.Where(x => x.SpecialPlace.Contains(searchText.Trim())).Select(x => x.Id).ToList();
                    globalSearchResponse.SearchResult = jobSpecialPlace;
                    break;
                // get the result of selected TransmittalNo
                case GlobalSearchType.TransmittalNumber:
                    var jobTransmittal = rpoContext.JobTransmittals.Where(x => x.TransmittalNumber.Contains(searchText.Trim()) && (x.IsDraft == false || x.IsDraft == null)).Select(x => x.IdJob).Distinct().ToList();
                    globalSearchResponse.SearchResult = jobTransmittal;
                    break;
                // get the result of selected ZoneDistrict of RFP address
                case GlobalSearchType.ZoneDistrict:
                    var jobZoneDistrict = rpoContext.Jobs.Include("RfpAddress").Where(x => x.RfpAddress.ZoneDistrict.Contains(searchText.Trim())).Select(x => x.Id).ToList();
                    globalSearchResponse.SearchResult = jobZoneDistrict;
                    break;
                // get the result of selected Overlay of RFP address
                case GlobalSearchType.Overlay:
                    var jobOverlay = rpoContext.Jobs.Include("RfpAddress").Where(x => x.RfpAddress.Overlay.Contains(searchText.Trim())).Select(x => x.Id).ToList();
                    globalSearchResponse.SearchResult = jobOverlay;
                    break;
                // get the result of selected Tracking number
                //case GlobalSearchType.TrackingNumber:
                //    int dotApplicationType = ApplicationType.DOT.GetHashCode();
                //    var tracking = rpoContext.JobApplications.Include("JobApplicationType").Where(x => x.ApplicationNumber.Contains(searchText.Trim()) && x.JobApplicationType != null && x.JobApplicationType.IdParent == dotApplicationType).Select(x => x.IdJob).Distinct().ToList();
                //    globalSearchResponse.SearchResult = tracking;
                  //  break;
                // get the result of selected violation number
                case GlobalSearchType.ViolationNumber:
                    //var violation = rpoContext.JobViolations.Where(x => x.SummonsNumber.Contains(searchText.Trim())).Select(x => x.IdJob ?? 0).Distinct().ToList();
                    //globalSearchResponse.SearchResult = violation;

                   // var jobViolation = rpoContext.JobViolations.Where(x => x.SummonsNumber.Contains(searchText.Trim())).Select(x => x.BinNumber).Distinct().ToList();
                    var jobViolation = rpoContext.JobViolations.Where(x => x.SummonsNumber.Contains(searchText.Trim())).Distinct().ToList();
                    foreach(var j in jobViolation)
                    {
                        var rfpaddresses= rpoContext.RfpAddresses.Where(x => j.BinNumber== x.BinNumber).ToList().Select(x => x.Id);
                        var idjobs = rfpaddresses != null ? rpoContext.Jobs.Where(x => rfpaddresses.Contains(x.IdRfpAddress)).ToList().Select(y => y.Id) : null;
                        var projectaccess = rpoContext.CustomerJobAccess.Where(x => x.IdCustomer == employee.Id).Where(z=>idjobs.Contains(z.IdJob)).Select(y => y.IdJob).ToList();
                    }
                  //  var rfpaddresses = jobViolation != null ? rpoContext.RfpAddresses.Where(x => jobViolation.Contains(x.BinNumber)).ToList().Select(x => x.Id) : null;
                  //  var idjobs = rfpaddresses != null ? rpoContext.Jobs.Where(x => rfpaddresses.Contains(x.IdRfpAddress)).ToList().Select(y => y.Id) : null;
                  //  var projectaccess = rpoContext.CustomerJobAccess.Where(x => x.IdCustomer == employee.Id).ToList().Select(y => y.IdJob);
                   // var(var p in projectaccess)
                   // {

                   // }

                  //  jobs = jobs.Where(x => rfpaddresses.Contains(x.IdRfpAddress) && projectaccess.Contains(x.Id));
                    break;
                // get the result of selected Task number
                case GlobalSearchType.Task:
                    var task = rpoContext.Tasks.Where(x => x.TaskNumber.Contains(searchText.Trim())).Select(x => x.Id).ToList();
                    globalSearchResponse.SearchResult = task;
                    break;
            }

            return globalSearchResponse;
        }
    }
}