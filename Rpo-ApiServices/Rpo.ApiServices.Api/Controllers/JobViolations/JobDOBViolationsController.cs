// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 05-04-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-21-2018
// ***********************************************************************
// <copyright file="JobViolationsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Violations Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Job Violations namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobViolations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Api.Tools;
    using System.Text;
    using System.IO;
    using HtmlAgilityPack;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Model.Models.Enums;
    using SODA;
    using System.Net.Http;
    using Rpo.ApiServices.Api.Controllers.JobViolations.Models;

    /// <summary>
    /// Class Job DOB Violations Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class JobDOBViolationsController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the job violations.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the job DOB violations List.</returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetJobDOBViolations([FromUri] JobViolationDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

            List<Rfp> lstRFPs = new List<Rfp>();
            List<Job> lstJobs = new List<Job>();
            List<JobViolation> lstJobViolation = new List<JobViolation>();

            var job = this.rpoContext.Jobs.Where(x => x.Id == dataTableParameters.IdJob).FirstOrDefault();
            var binnumber = this.rpoContext.RfpAddresses.Where(x => x.Id == job.IdRfpAddress).Select(y => y.BinNumber).FirstOrDefault();
            IQueryable<JobViolation> jobViolations;
            if (job.Status == JobStatus.Close || job.Status == JobStatus.Hold)
            {
                jobViolations = this.rpoContext.JobViolations.Where(x => x.BinNumber == binnumber && (x.IdJob == null || x.IdJob == 0) || x.IdJob == dataTableParameters.IdJob).Where(x => x.Type_ECB_DOB == "DOB").Where(x => x.CreatedDate <= job.OnHoldCompletedDate)
                 .Include("CreatedByEmployee").Include("LastModifiedByEmployee")
                 .AsQueryable();
            }
            else
            {
                jobViolations = this.rpoContext.JobViolations.Where(x => x.BinNumber == binnumber && (x.IdJob == null || x.IdJob == 0) || x.IdJob == dataTableParameters.IdJob).Where(x => x.Type_ECB_DOB == "DOB")
                    .Include("CreatedByEmployee").Include("LastModifiedByEmployee")
                    .AsQueryable();
            }
            var recordsTotal = jobViolations.Count();
            var recordsFiltered = recordsTotal;
            var result = jobViolations
                .AsEnumerable()
                .Select(c => this.Format(c))
                .AsQueryable()
                .DataTableParameters(dataTableParameters, out recordsFiltered)
                .ToArray();

            return this.Ok(new DataTableResponse
            {
                Draw = dataTableParameters.Draw,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal,
                Data = result.OrderByDescending(x => x.LastModifiedDate)
            });
        }
        /// <summary>
        /// Gets the job violation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the job violations in detail.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobViolationDetail))]
        public IHttpActionResult GetJobDOBViolation(int id)
        {
            JobViolation jobViolation = this.rpoContext.JobViolations.Include("CreatedByEmployee")
                .Include("LastModifiedByEmployee")               
                .FirstOrDefault(x => x.Id == id);
            JobViolationDetail jobViolationDetail = this.FormatDetails(jobViolation);
            if (jobViolation.IdJob == null || jobViolation.IdJob == 0)
            {
                var rfpaddresses = this.rpoContext.RfpAddresses.Where(x => x.BinNumber == jobViolation.BinNumber).Select(y => y.Id).ToList();
                var jobids = this.rpoContext.Jobs.Where(x => rfpaddresses.Contains(x.IdRfpAddress)).Select(y => y.Id).ToList();
                var ViolationPartOfJobsQuery = from j in this.rpoContext.Jobs
                                               join ra in this.rpoContext.RfpAddresses on j.IdRfpAddress equals ra.Id into raGroup
                                               from ra in raGroup.DefaultIfEmpty()
                                               join b in this.rpoContext.Boroughes on j.IdBorough equals b.Id into bGroup
                                               from b in bGroup.DefaultIfEmpty()
                                               join c in this.rpoContext.Companies on j.IdCompany equals c.Id into cGroup
                                               from c in cGroup.DefaultIfEmpty()
                                               join cn in this.rpoContext.Contacts on j.IdContact equals cn.Id into cnGroup
                                               from cn in cnGroup.DefaultIfEmpty()
                                               join su in this.rpoContext.Sufixes on cn.IdSuffix equals su.Id into suGroup
                                               from su in suGroup.DefaultIfEmpty()
                                               join jc in this.rpoContext.JobContactTypes on j.IdJobContactType equals jc.Id into jcGroup
                                               from jc in jcGroup.DefaultIfEmpty()
                                               join e in this.rpoContext.Employees on j.IdProjectManager equals e.Id into eGroup
                                               from e in eGroup.DefaultIfEmpty()
                                               where jobids.Contains(j.Id)                                              
                                               select new ViolationPartOfJobs()
                                               {
                                                   JobId = j.Id,
                                                   JobNumber = j.JobNumber,
                                                   Status = j.Status,
                                                   StatusDescription = j.Status == JobStatus.Active ? "Active" : j.Status == JobStatus.Hold ? "Hold" : j.Status == JobStatus.Close ? "Close" : null,
                                                   IdRfpAddress = j.IdRfpAddress,
                                                   IdRfp = j.IdRfp,
                                                   RfpAddress = ra.Street,
                                                   ZipCode = ra.ZipCode,
                                                   RAIdBorough = ra.IdBorough,
                                                   Borough = b.Description,
                                                   HouseNumber = ra.HouseNumber,
                                                   StreetNumber = ra.Street, // Note: you have the same column alias twice; you may want to adjust this accordingly.
                                                   FloorNumber = j.FloorNumber,
                                                   Apartment = j.Apartment,
                                                   SpecialPlace = j.SpecialPlace,
                                                   Block = j.Block,
                                                   Lot = j.Lot,
                                                   BinNumber = ra.BinNumber,
                                                   HasLandMarkStatus = j.HasLandMarkStatus,
                                                   HasEnvironmentalRestriction = j.HasEnvironmentalRestriction,
                                                   HasOpenWork = j.HasOpenWork,
                                                   IdCompany = j.IdCompany,
                                                   CompanyName = c.Name,
                                                   IdContact = j.IdContact,
                                                   ContactName = cn.FirstName + " " + (cn.LastName ?? "") + " " + (su.Description ?? ""),
                                                   LastModiefiedDate = j.LastModiefiedDate,
                                                   IdJobContactType = j.IdJobContactType,
                                                   JobContactTypeDescription = jc.Name,
                                                   IdProjectManager = j.IdProjectManager,
                                                   ProjectManagerName = e.FirstName + " " + e.LastName,
                                                   StartDate = j.StartDate,
                                                   EndDate = j.EndDate,
                                                   ParentStatusId = 0,
                                                   OCMCNumber = j.OCMCNumber,
                                                   StreetWorkingFrom = j.StreetWorkingFrom,
                                                   StreetWorkingOn = j.StreetWorkingOn,
                                                   StreetWorkingTo = j.StreetWorkingTo,
                                                   QBJobName = j.QBJobName,
                                                   JobStatusNotes = j.JobStatusNotes
                                               };

                List<ViolationPartOfJobs> lstViolationPartOfJobs = ViolationPartOfJobsQuery.ToList();

                if (jobViolation == null)
                {
                    return this.NotFound();
                }

                jobViolationDetail.PartOfJobs = new List<ViolationPartOfJobs>();
                jobViolationDetail.PartOfJobs = lstViolationPartOfJobs;
                return this.Ok(jobViolationDetail);
            }
            else
            {
                return this.Ok(jobViolationDetail);
            }
        }

        /// <summary>
        /// Posts the job violation.
        /// </summary>
        /// <param name="jobViolationCreateUpdate">The job violation create update.</param>
        /// <returns>create a new job violations.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobViolationDetail))]
        public IHttpActionResult PostJobDOBViolation(JobViolationCreateUpdate jobViolationCreateUpdate)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddDOBViolations))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                List<JobViolation> jobViolations = this.JobViolationNumberExists(jobViolationCreateUpdate.SummonsNumber, jobViolationCreateUpdate.Id);
                if (jobViolations != null && jobViolations.Count > 0)
                {
                    throw new RpoBusinessException(string.Format(StaticMessages.JobViolationNumberExistsMessage, jobViolationCreateUpdate.SummonsNumber, (jobViolations[0].Job != null ? jobViolations[0].Job.JobNumber : string.Empty)));
                }

                JobViolation jobViolation = new JobViolation();

            jobViolation.IdJob = jobViolationCreateUpdate.IdJob;
            jobViolation.SummonsNumber = jobViolationCreateUpdate.SummonsNumber;           
            jobViolation.Disposition_Date = jobViolationCreateUpdate.DispositionDate != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobViolationCreateUpdate.DispositionDate), DateTimeKind.Utc) : jobViolationCreateUpdate.DispositionDate;            
            jobViolation.Disposition_Comments = jobViolationCreateUpdate.DispositionComments;
            jobViolation.device_number = jobViolationCreateUpdate.DeviceNumber;
            jobViolation.ViolationDescription = jobViolationCreateUpdate.ViolationDescription;
            jobViolation.ECBnumber = jobViolationCreateUpdate.ECBNumber;
            jobViolation.violation_category = jobViolationCreateUpdate.ViolationCategory;
            jobViolation.PartyResponsible = jobViolationCreateUpdate.PartyResponsible;
            if (jobViolationCreateUpdate.PartyResponsible == 3) //3 means other 1 means RPOteam
                jobViolation.ManualPartyResponsible = jobViolationCreateUpdate.ManualPartyResponsible;
            int IdRfpAddress = this.rpoContext.Jobs.Where(x => x.Id == jobViolationCreateUpdate.IdJob).Select(y => y.IdRfpAddress).FirstOrDefault();
            jobViolation.BinNumber = this.rpoContext.RfpAddresses.Where(x => x.Id == IdRfpAddress).Select(y => y.BinNumber).FirstOrDefault();
                if (jobViolation.Notes != jobViolationCreateUpdate.Notes)
                {
                    jobViolation.NotesLastModifiedDate = DateTime.UtcNow;
                    if (employee != null)
                    {
                        jobViolation.NotesLastModifiedBy = employee.Id;
                    }
                }
                jobViolation.Notes = jobViolationCreateUpdate.Notes;
            jobViolation.Type_ECB_DOB = "DOB";
            jobViolation.violation_number = jobViolationCreateUpdate.ViolationNumber;           
            jobViolation.DateIssued = jobViolationCreateUpdate.DateIssued != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobViolationCreateUpdate.DateIssued), DateTimeKind.Utc) : jobViolationCreateUpdate.DateIssued;
            jobViolation.IsManually = true;
            jobViolation.IsNewMailsent = true;              
                jobViolation.LastModifiedDate = DateTime.UtcNow;
            jobViolation.CreatedDate = DateTime.UtcNow;
             jobViolation.Status = 1;            
                if (employee != null)
                {
                    jobViolation.CreatedBy = employee.Id;
                }

            this.rpoContext.JobViolations.Add(jobViolation);
            this.rpoContext.SaveChanges();
            var compositecheklists = this.rpoContext.CompositeChecklists.Where(x => x.ParentJobId == jobViolationCreateUpdate.IdJob);
            List<CompositeViolations> lstCompositeViolations = new List<CompositeViolations>();
            foreach (var c in compositecheklists)
            {
                CompositeViolations compositeViolations = new CompositeViolations();
                compositeViolations.IdCompositeChecklist = c.Id;
                compositeViolations.IdJobViolations = jobViolation.Id;
                lstCompositeViolations.Add(compositeViolations);
            }
            rpoContext.CompositeViolations.AddRange(lstCompositeViolations);
            this.rpoContext.SaveChanges();           

                string addApplication_Violation = JobHistoryMessages.AddApplication_Violation
                                                         .Replace("##Violation##", jobViolation != null ? jobViolation.SummonsNumber : JobHistoryMessages.NoSetstring);


                Common.SaveJobHistory(employee.Id, jobViolation.IdJob.Value, addApplication_Violation, JobHistoryType.Applications);


                JobViolation jobViolationResponse = this.rpoContext.JobViolations.Include("CreatedByEmployee").Include("LastModifiedByEmployee").Include("explanationOfCharges").FirstOrDefault(x => x.Id == jobViolation.Id);
                return this.Ok(this.FormatDetails(jobViolationResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the job violation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="jobViolationCreateUpdate">The job violation create update.</param>
        /// <returns>update the detail of job violation.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobViolationDetail))]
        public IHttpActionResult PutJobDOBViolation(int id, JobViolationCreateUpdate jobViolationCreateUpdate)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddDOBViolations))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (id != jobViolationCreateUpdate.Id)
                {
                    return this.BadRequest();
                }

                List<JobViolation> jobViolations = this.JobViolationNumberExists(jobViolationCreateUpdate.SummonsNumber, jobViolationCreateUpdate.Id);
                if (jobViolations != null && jobViolations.Count > 0)
                {
                    throw new RpoBusinessException(string.Format(StaticMessages.JobViolationNumberExistsMessage, (jobViolations[0].Job != null ? jobViolations[0].Job.JobNumber : string.Empty)));
                }

                JobViolation jobViolation = rpoContext.JobViolations.FirstOrDefault(x => x.Id == jobViolationCreateUpdate.Id);


                jobViolation.IdJob = jobViolationCreateUpdate.IdJob;
                jobViolation.SummonsNumber = jobViolationCreateUpdate.SummonsNumber;
                jobViolation.DateIssued = jobViolationCreateUpdate.DateIssued; 
                jobViolation.violation_number = jobViolationCreateUpdate.ViolationNumber;              
                jobViolation.Disposition_Date = jobViolationCreateUpdate.DispositionDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolationCreateUpdate.DispositionDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolationCreateUpdate.DispositionDate;
                jobViolation.Disposition_Comments = jobViolationCreateUpdate.DispositionComments;
                jobViolation.device_number = jobViolationCreateUpdate.DeviceNumber;
                jobViolation.ViolationDescription = jobViolationCreateUpdate.ViolationDescription;
                jobViolation.ECBnumber = jobViolationCreateUpdate.ECBNumber;
                jobViolation.violation_category = jobViolationCreateUpdate.ViolationCategory;
                jobViolation.Type_ECB_DOB = "DOB";
                jobViolation.PartyResponsible = jobViolationCreateUpdate.PartyResponsible;
                jobViolation.IsManually = jobViolationCreateUpdate.isManually;
                if (jobViolation.Notes != jobViolationCreateUpdate.Notes)
                {
                    jobViolation.NotesLastModifiedDate = DateTime.UtcNow;
                    if (employee != null)
                    {
                        jobViolation.NotesLastModifiedBy = employee.Id;
                    }
                }
                jobViolation.Notes = jobViolationCreateUpdate.Notes;              
                if (jobViolationCreateUpdate.PartyResponsible == 3) //3 means other 1 means RPOteam
                    jobViolation.ManualPartyResponsible = jobViolationCreateUpdate.ManualPartyResponsible;               
                jobViolation.LastModifiedDate = DateTime.UtcNow;                
                if (employee != null)
                {
                    jobViolation.LastModifiedBy = employee.Id;
                }

                try
                {
                    this.rpoContext.SaveChanges();
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!this.JobViolationExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                string editApplication_Violation = JobHistoryMessages.EditApplication_Violation
                                                         .Replace("##Violation##", jobViolation != null ? jobViolation.violation_number : JobHistoryMessages.NoSetstring);
     
                JobViolation jobViolationResponse = this.rpoContext.JobViolations.Include("CreatedByEmployee").Include("LastModifiedByEmployee").Include("explanationOfCharges").FirstOrDefault(x => x.Id == id);
                return this.Ok(this.FormatDetails(jobViolationResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }

        }

        /// <summary>
        /// Jobs the violation number exists.
        /// </summary>
        /// <param name="summonsNumber">The summons number.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private List<JobViolation> JobViolationNumberExists(string SummonsNumber, int id)
        {
            var jobViolations = this.rpoContext.JobViolations.Where(e => e.SummonsNumber == SummonsNumber && e.Id != id).ToList();

            return jobViolations;
        }
        /// <summary>
        /// Jobs the violation exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool JobViolationExists(int id)
        {
            return this.rpoContext.JobViolations.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Deletes the job violation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>delete the job violation.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobViolationDetail))]
        public IHttpActionResult DeleteJobDOBViolation(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteDOBViolations))
            {                
                    JobViolation jobViolation = rpoContext.JobViolations.Find(id);
                    if (jobViolation == null)
                    {
                        return this.NotFound();
                    }
                int jobid = 0;
                if (jobViolation.IdJob != null)
                {
                    jobid = jobViolation.IdJob.Value;
                }

                string editApplication_Violation = JobHistoryMessages.DeleteApplication_Violation
                                                     .Replace("##Violation##", jobViolation != null ? jobViolation.violation_number : JobHistoryMessages.NoSetstring);
                var compositeviolation = rpoContext.CompositeViolations.Where(x => x.IdJobViolations == id).ToList();
                if (compositeviolation.Count > 0)
                    rpoContext.CompositeViolations.RemoveRange(compositeviolation);

                var jobviolationnotes = rpoContext.JobViolationNotes.Where(x => x.IdJobViolation == id).ToList();
                if (jobviolationnotes.Count > 0)
                    rpoContext.JobViolationNotes.RemoveRange(jobviolationnotes);
                var checklistJobViolationComments = rpoContext.ChecklistJobViolationComments.Where(x => x.IdJobViolation == id).ToList();
                if (checklistJobViolationComments.Count > 0)
                    rpoContext.ChecklistJobViolationComments.RemoveRange(checklistJobViolationComments);
                rpoContext.JobViolations.Remove(jobViolation);
                    rpoContext.SaveChanges(); 
                    return Ok(FormatDetails(jobViolation));                
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        /// Formats the specified job violation.
        /// </summary>
        /// <param name="jobViolation">The job violation.</param>
        /// <returns>JobViolationDTO.</returns>
        private JobViolationDTO Format(JobViolation jobViolation)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            JobViolationDTO jobViolationDTO = new JobViolationDTO();

            jobViolationDTO.Id = jobViolation.Id;
            jobViolationDTO.IdJob = jobViolation.IdJob;           
            jobViolationDTO.DateIssued = jobViolation.DateIssued;      
            jobViolationDTO.NotesLastModifiedDate = jobViolation.NotesLastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.NotesLastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.NotesLastModifiedDate;
            jobViolationDTO.NotesLastModifiedByEmployeeName = jobViolation.NotesLastModifiedByEmployee != null ? jobViolation.NotesLastModifiedByEmployee.FirstName + " " + jobViolation.NotesLastModifiedByEmployee.LastName : string.Empty;
            jobViolationDTO.PartyResponsible = jobViolation.PartyResponsible;
            if (jobViolationDTO.PartyResponsible == 3) //3 means other 1 means RPOteam
                jobViolationDTO.ManualPartyResponsible = jobViolation.ManualPartyResponsible;
            jobViolationDTO.DispositionDate = jobViolation.Disposition_Date != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.Disposition_Date), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.Disposition_Date;
            jobViolationDTO.DispositionComments = jobViolation.Disposition_Comments;
            jobViolationDTO.DeviceNumber = jobViolation.device_number;
            jobViolationDTO.ViolationDescription = jobViolation.ViolationDescription;
            jobViolationDTO.ECBNumber = jobViolation.ECBnumber;
            jobViolationDTO.SummonsNumber = jobViolation.SummonsNumber;
            jobViolationDTO.ViolationCategory = jobViolation.violation_category;
            jobViolationDTO.BinNumber = jobViolation.BinNumber;
            jobViolationDTO.Status = jobViolation.Status;
            jobViolationDTO.Notes = jobViolation.Notes;
            jobViolationDTO.ISNViolation = jobViolation.ISNViolation;
            return jobViolationDTO;
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="jobViolation">The job violation.</param>
        /// <returns>JobViolationDetail.</returns>
        private JobViolationDetail FormatDetails(JobViolation jobViolation)
        {
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            JobViolationDetail jobViolationDetail = new JobViolationDetail();

            jobViolationDetail.Id = jobViolation.Id;
            jobViolationDetail.IdJob = jobViolation.IdJob;
            jobViolationDetail.SummonsNumber = jobViolation.SummonsNumber;           
            jobViolationDetail.DateIssued = jobViolation.DateIssued;            
            jobViolationDetail.IsFullyResolved = jobViolation.IsFullyResolved;           
            jobViolationDetail.PartyResponsible = jobViolation.PartyResponsible;
            jobViolationDetail.Status = jobViolation.Status;
            if (jobViolationDetail.PartyResponsible == 3) //3 means other 1 means RPOteam
                jobViolationDetail.ManualPartyResponsible = jobViolation.ManualPartyResponsible;
            jobViolationDetail.CreatedBy = jobViolation.CreatedBy;
            jobViolationDetail.LastModifiedBy = jobViolation.LastModifiedBy != null ? jobViolation.LastModifiedBy : jobViolation.CreatedBy;
            jobViolationDetail.CreatedByEmployeeName = jobViolation.CreatedByEmployee != null ? jobViolation.CreatedByEmployee.FirstName + " " + jobViolation.CreatedByEmployee.LastName : string.Empty;
            jobViolationDetail.LastModifiedByEmployeeName = jobViolation.LastModifiedByEmployee != null ? jobViolation.LastModifiedByEmployee.FirstName + " " + jobViolation.LastModifiedByEmployee.LastName : (jobViolation.CreatedByEmployee != null ? jobViolation.CreatedByEmployee.FirstName + " " + jobViolation.CreatedByEmployee.LastName : string.Empty);
            jobViolationDetail.CreatedDate = jobViolation.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.CreatedDate;
            jobViolationDetail.NotesLastModifiedDate = jobViolation.NotesLastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.NotesLastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.NotesLastModifiedDate;
            jobViolationDetail.NotesLastModifiedByEmployeeName = jobViolation.NotesLastModifiedByEmployee != null ? jobViolation.NotesLastModifiedByEmployee.FirstName + " " + jobViolation.NotesLastModifiedByEmployee.LastName : string.Empty;
            jobViolationDetail.LastModifiedDate = jobViolation.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (jobViolation.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.CreatedDate);
            jobViolationDetail.DispositionDate = jobViolation.Disposition_Date != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.Disposition_Date), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.Disposition_Date;
            jobViolationDetail.DispositionComments = jobViolation.Disposition_Comments;
            jobViolationDetail.DeviceNumber = jobViolation.device_number;
            jobViolationDetail.ViolationDescription = jobViolation.ViolationDescription;
            jobViolationDetail.ECBNumber = jobViolation.ECBnumber;
            jobViolationDetail.ViolationNumber = jobViolation.violation_number;
            jobViolationDetail.ViolationCategory = jobViolation.violation_category;
            jobViolationDetail.BinNumber = jobViolation.BinNumber;
            jobViolationDetail.Notes = jobViolation.Notes;
            jobViolationDetail.IsManually = jobViolation.IsManually;
            jobViolationDetail.JobViolationDocuments = jobViolation.JobViolationDocuments != null && jobViolation.JobViolationDocuments.Count > 0 ?
                                                           jobViolation.JobViolationDocuments.Select(x => new JobViolationDocumentDetail()
                                                           {
                                                               Id = x.Id,
                                                               DocumentPath = APIUrl + "/" + Properties.Settings.Default.JobViolationDocumentPath + "/" + x.Id + "_" + x.DocumentPath,
                                                               IdJobViolation = x.IdJobViolation,
                                                               Name = x.Name
                                                           }).ToList() : null;

            if (!string.IsNullOrEmpty(jobViolationDetail.IssuingAgency) && jobViolationDetail.IssuingAgency.ToUpper() == "DEPT OF BUILDINGS")
            {
                jobViolationDetail.explanationOfCharges = jobViolation.explanationOfCharges != null && jobViolation.explanationOfCharges.Count > 0 ?
                                                      jobViolation.explanationOfCharges.Select(x => new JobViolationExplanationOfChargesDTO()
                                                      {
                                                          Id = x.Id,
                                                          IdViolation = x.IdViolation,
                                                          Code = x.Code,
                                                          CodeSection = x.CodeSection,
                                                          Description = x.Description,
                                                          PaneltyAmount = x.PaneltyAmount,
                                                          IsFromAuth = x.IsFromAuth,
                                                          CreatedBy = x.CreatedBy,
                                                          LastModifiedBy = x.LastModifiedBy != null ? x.LastModifiedBy : x.CreatedBy,
                                                          CreatedByEmployeeName = x.CreatedByEmployee != null ? x.CreatedByEmployee.FirstName + " " + x.CreatedByEmployee.LastName : string.Empty,
                                                          LastModifiedByEmployeeName = x.CreatedByEmployee != null ? x.CreatedByEmployee.FirstName + " " + x.CreatedByEmployee.LastName : string.Empty,
                                                          CreatedDate = x.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : x.CreatedDate,
                                                          LastModifiedDate = x.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (x.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : x.CreatedDate),
                                                          DOBPenaltySchedule = this.rpoContext.DOBPenaltySchedules.FirstOrDefault(d => d.InfractionCode == x.Code)
                                                      }).ToList() : null;
            }
            else if (!string.IsNullOrEmpty(jobViolationDetail.IssuingAgency) && jobViolationDetail.IssuingAgency.ToUpper() == "FIRE DEPARTMENT OF NYC")
            {

                jobViolationDetail.explanationOfCharges = jobViolation.explanationOfCharges != null && jobViolation.explanationOfCharges.Count > 0 ?
                                                      jobViolation.explanationOfCharges.Select(x => new JobViolationExplanationOfChargesDTO()
                                                      {
                                                          Id = x.Id,
                                                          IdViolation = x.IdViolation,
                                                          Code = x.Code,
                                                          CodeSection = x.CodeSection,
                                                          Description = x.Description,
                                                          PaneltyAmount = x.PaneltyAmount,
                                                          IsFromAuth = x.IsFromAuth,
                                                          CreatedBy = x.CreatedBy,
                                                          LastModifiedBy = x.LastModifiedBy != null ? x.LastModifiedBy : x.CreatedBy,
                                                          CreatedByEmployeeName = x.CreatedByEmployee != null ? x.CreatedByEmployee.FirstName + " " + x.CreatedByEmployee.LastName : string.Empty,
                                                          LastModifiedByEmployeeName = x.CreatedByEmployee != null ? x.CreatedByEmployee.FirstName + " " + x.CreatedByEmployee.LastName : string.Empty,
                                                          CreatedDate = x.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : x.CreatedDate,
                                                          LastModifiedDate = x.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (x.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : x.CreatedDate),
                                                          FDNYPenaltySchedule = this.rpoContext.FDNYPenaltySchedules.FirstOrDefault(d => d.OATHViolationCode == x.Code)
                                                      }).ToList() : null;
            }
            else
            {
                jobViolationDetail.explanationOfCharges = jobViolation.explanationOfCharges != null && jobViolation.explanationOfCharges.Count > 0 ?
                                                      jobViolation.explanationOfCharges.Select(x => new JobViolationExplanationOfChargesDTO()
                                                      {
                                                          Id = x.Id,
                                                          IdViolation = x.IdViolation,
                                                          Code = x.Code,
                                                          CodeSection = x.CodeSection,
                                                          Description = x.Description,
                                                          PaneltyAmount = x.PaneltyAmount,
                                                          IsFromAuth = x.IsFromAuth,
                                                          CreatedBy = x.CreatedBy,
                                                          LastModifiedBy = x.LastModifiedBy != null ? x.LastModifiedBy : x.CreatedBy,
                                                          CreatedByEmployeeName = x.CreatedByEmployee != null ? x.CreatedByEmployee.FirstName + " " + x.CreatedByEmployee.LastName : string.Empty,
                                                          LastModifiedByEmployeeName = x.CreatedByEmployee != null ? x.CreatedByEmployee.FirstName + " " + x.CreatedByEmployee.LastName : string.Empty,
                                                          CreatedDate = x.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : x.CreatedDate,
                                                          LastModifiedDate = x.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (x.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : x.CreatedDate)
                                                      }).ToList() : null;
            }

            return jobViolationDetail;

        }
    }
}