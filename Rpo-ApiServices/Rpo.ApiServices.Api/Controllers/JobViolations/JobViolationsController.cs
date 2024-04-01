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
    using System.Globalization;

    /// <summary>
    /// Class Job Violations Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class JobViolationsController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the job violations.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the job violations List.</returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetJobViolations([FromUri] JobViolationDataTableParameters dataTableParameters)
        {
            var job = this.rpoContext.Jobs.Where(x => x.Id == dataTableParameters.IdJob).FirstOrDefault();
            var binnumber = this.rpoContext.RfpAddresses.Where(x => x.Id == job.IdRfpAddress).Select(y => y.BinNumber).FirstOrDefault();
            IQueryable<JobViolation> jobViolations;
            if (job.Status == JobStatus.Close || job.Status == JobStatus.Hold)
            {
                jobViolations = this.rpoContext.JobViolations.Where(x => x.BinNumber == binnumber && (x.IdJob == null || x.IdJob == 0) || x.IdJob == dataTableParameters.IdJob).Where(x => x.Type_ECB_DOB == "ECB").Where(x => x.CreatedDate <= job.OnHoldCompletedDate)
                   .Include("CreatedByEmployee")
                   .Include("LastModifiedByEmployee")
                   .Include("explanationOfCharges")
                   .AsQueryable();

            }
            else
            {
                jobViolations = this.rpoContext.JobViolations.Where(x => x.BinNumber == binnumber && (x.IdJob == null || x.IdJob == 0) || x.IdJob == dataTableParameters.IdJob).Where(x => x.Type_ECB_DOB == "ECB")
                      .Include("CreatedByEmployee")
                      .Include("LastModifiedByEmployee")
                      .Include("explanationOfCharges")
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
        public IHttpActionResult GetJobViolation(int id)
        {
            JobViolation jobViolation = this.rpoContext.JobViolations.Include("CreatedByEmployee")
                .Include("LastModifiedByEmployee").Include("explanationOfCharges")
                .FirstOrDefault(x => x.Id == id);

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
                                           where ra.BinNumber == jobViolation.BinNumber
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

            JobViolationDetail jobViolationDetail = this.FormatDetails(jobViolation);
            jobViolationDetail.PartOfJobs = new List<ViolationPartOfJobs>();
            jobViolationDetail.PartOfJobs = lstViolationPartOfJobs;
            return this.Ok(jobViolationDetail);
        }


        /// <summary>
        /// Gets the job violation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the job violations in detail.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobViolationDetail))]
        [Route("api/JobViolations/GetJobViolationBySummons/{Summonsnumber}")]
        public IHttpActionResult GetJobViolationBySummons(string Summonsnumber)
        {
            JobViolation jobViolation = this.rpoContext.JobViolations.Include("CreatedByEmployee")
                .Include("LastModifiedByEmployee").Include("explanationOfCharges")
                .FirstOrDefault(x => x.SummonsNumber == Summonsnumber);         

            if (jobViolation == null)
            {
                return this.NotFound();
            }

            JobViolationDetail jobViolationDetail = this.FormatDetails(jobViolation);          
            return this.Ok(jobViolationDetail);
        }

        /// <summary>
        /// Gets the job violation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the job violations List for Bind dropdown.</returns>
        [Authorize]
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/JobViolations/dropdown")]
        public IHttpActionResult GetJobViolationsDropdown(int idJob)
        {
            //var result = rpoContext.JobViolations.Where(x => x.IdJob == idJob).AsEnumerable().Select(c => new
            //    {
            //    Id = c.Id,
            //    ItemName = c.SummonsNumber != null ? c.SummonsNumber : string.Empty
            //    }).ToArray();

            var binNumber = rpoContext.Jobs.Include("RfpAddress").Where(x => x.Id == idJob).Select(i => i.RfpAddress.BinNumber).FirstOrDefault();
            var result = rpoContext.JobViolations.Where(x => x.BinNumber == binNumber).AsEnumerable().Select(c => new IdItemName
            {
                Id = c.Id.ToString(),
                ItemName = c.SummonsNumber != null ? c.SummonsNumber : string.Empty
            }).ToArray();
            return Ok(result);
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
        public IHttpActionResult PutJobViolation(int id, JobViolationCreateUpdate jobViolationCreateUpdate)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddECBViolations))
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


                if (jobViolation.Notes != jobViolationCreateUpdate.Notes)
                {
                    jobViolation.NotesLastModifiedDate = DateTime.UtcNow;
                    if (employee != null)
                    {
                        jobViolation.NotesLastModifiedBy = employee.Id;
                    }
                }
                jobViolation.IdJob = jobViolationCreateUpdate.IdJob;
                jobViolation.SummonsNumber = jobViolationCreateUpdate.SummonsNumber;
                jobViolation.DateIssued = jobViolationCreateUpdate.DateIssued;
                jobViolation.HearingDate = jobViolationCreateUpdate.HearingDate;
                jobViolation.CureDate = jobViolationCreateUpdate.CureDate;
                jobViolation.HearingLocation = jobViolationCreateUpdate.HearingLocation;
                jobViolation.HearingResult = jobViolationCreateUpdate.HearingResult;
                jobViolation.StatusOfSummonsNotice = jobViolationCreateUpdate.StatusOfSummonsNotice;
                jobViolation.RespondentAddress = jobViolationCreateUpdate.RespondentAddress;
                jobViolation.InspectionLocation = jobViolationCreateUpdate.InspectionLocation;
                jobViolation.BalanceDue = jobViolationCreateUpdate.BalanceDue;
                jobViolation.RespondentName = jobViolationCreateUpdate.RespondentName;
                jobViolation.IssuingAgency = jobViolationCreateUpdate.IssuingAgency;
                jobViolation.IsCOC = jobViolationCreateUpdate.IsCOC;
                jobViolation.COCDate = jobViolationCreateUpdate.COCDate;
                jobViolation.Notes = jobViolationCreateUpdate.Notes;
                jobViolation.PaneltyAmount = jobViolationCreateUpdate.PaneltyAmount;
                jobViolation.ComplianceOn = jobViolationCreateUpdate.ComplianceOn;
                jobViolation.CertificationStatus = jobViolationCreateUpdate.CertificationStatus;
                jobViolation.IsFullyResolved = jobViolationCreateUpdate.IsFullyResolved;
                jobViolation.ResolvedDate = jobViolationCreateUpdate.ResolvedDate;
                jobViolation.LastModifiedDate = DateTime.UtcNow;
                jobViolation.PartyResponsible = jobViolationCreateUpdate.PartyResponsible;
                jobViolation.IdContact = jobViolationCreateUpdate.IdContact;
                jobViolation.ManualPartyResponsible = jobViolationCreateUpdate.ManualPartyResponsible;
                jobViolation.aggravated_level = jobViolationCreateUpdate.AggravatedLevel;
                jobViolation.violation_type = jobViolationCreateUpdate.ViolationType;
                jobViolation.ViolationDescription = jobViolationCreateUpdate.ViolationDescription;               
                jobViolation.IsManually = jobViolationCreateUpdate.isManually;
                jobViolation.HearingTime = jobViolationCreateUpdate.HearingTime != null && !string.IsNullOrEmpty(jobViolationCreateUpdate.HearingTime) ? DateTime.ParseExact(jobViolationCreateUpdate.HearingTime, "HH:mm", CultureInfo.InvariantCulture) : DateTime.ParseExact("00:00", "HH:mm", CultureInfo.InvariantCulture);
                if (employee != null)
                {
                    jobViolation.LastModifiedBy = employee.Id;
                }

                if (jobViolationCreateUpdate.DocumentsToDelete != null)
                {
                    List<int> deletedDocs = jobViolationCreateUpdate.DocumentsToDelete.ToList();
                    rpoContext.JobViolationDocuments.RemoveRange(rpoContext.JobViolationDocuments.Where(ac => ac.IdJobViolation == jobViolation.Id && deletedDocs.Any(eacIds => eacIds == ac.Id)));
                }

                if (jobViolationCreateUpdate.DocumentsToDelete != null)
                {
                    foreach (var item in jobViolationCreateUpdate.DocumentsToDelete)
                    {
                        int rfpDocumentId = Convert.ToInt32(item);
                        JobViolationDocument jobViolationDocument = rpoContext.JobViolationDocuments.Where(x => x.Id == rfpDocumentId).FirstOrDefault();
                        if (jobViolationDocument != null)
                        {
                            rpoContext.JobViolationDocuments.Remove(jobViolationDocument);
                            var path = System.Web.HttpRuntime.AppDomainAppPath;
                            string directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobViolationDocumentPath));
                            string directoryDelete = Convert.ToString(jobViolationDocument.Id) + "_" + jobViolationDocument.DocumentPath;
                            string deletefilename = System.IO.Path.Combine(directoryName, directoryDelete);

                            if (File.Exists(deletefilename))
                            {
                                File.Delete(deletefilename);
                            }
                        }
                    }
                }
                // for violation code Explanation Of Charges.
                if (jobViolationCreateUpdate.explanationOfCharges != null)
                {
                    var idList = jobViolationCreateUpdate.explanationOfCharges.Select(x => x.Id);
                    var deletedIds = jobViolation.explanationOfCharges.Where(x => !idList.Contains(x.Id)).ToList();

                    if (deletedIds.Count > 0 && deletedIds != null)
                    {
                        rpoContext.JobViolationExplanationOfCharges.RemoveRange(deletedIds);
                        rpoContext.SaveChanges();
                    }
                    foreach (var item in jobViolationCreateUpdate.explanationOfCharges)
                    {
                        if (!string.IsNullOrEmpty(item.Code) && !string.IsNullOrEmpty(item.CodeSection) && !string.IsNullOrEmpty(item.Description) && item.PaneltyAmount != null && item.PaneltyAmount > 0)
                        {
                            JobViolationExplanationOfCharges updateItem = rpoContext.JobViolationExplanationOfCharges.FirstOrDefault(x => x.Id == item.Id);
                            if (updateItem != null)
                            {
                                updateItem.IdViolation = item.IdViolation;
                                updateItem.Code = item.Code;
                                updateItem.CodeSection = item.CodeSection;
                                updateItem.Description = item.Description;
                                updateItem.PaneltyAmount = item.PaneltyAmount;
                                updateItem.IsFromAuth = item.IsFromAuth;
                                updateItem.LastModifiedDate = DateTime.UtcNow;
                                if (employee != null)
                                {
                                    updateItem.LastModifiedBy = employee.Id;
                                }
                                this.rpoContext.SaveChanges();
                            }
                            else
                            {
                                JobViolationExplanationOfCharges explanationOfCharges = new JobViolationExplanationOfCharges();
                                explanationOfCharges.IdViolation = jobViolation.Id;
                                explanationOfCharges.Code = item.Code;
                                explanationOfCharges.CodeSection = item.CodeSection;
                                explanationOfCharges.Description = item.Description;
                                explanationOfCharges.PaneltyAmount = item.PaneltyAmount;
                                explanationOfCharges.IsFromAuth = item.IsFromAuth;
                                explanationOfCharges.LastModifiedDate = DateTime.UtcNow;
                                explanationOfCharges.CreatedDate = DateTime.UtcNow;
                                if (employee != null)
                                {
                                    explanationOfCharges.CreatedBy = employee.Id;
                                }
                                this.rpoContext.JobViolationExplanationOfCharges.Add(explanationOfCharges);
                                this.rpoContext.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    if (jobViolation.explanationOfCharges != null)
                    {
                        rpoContext.JobViolationExplanationOfCharges.RemoveRange(jobViolation.explanationOfCharges);
                        rpoContext.SaveChanges();
                    }
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
                                                         .Replace("##Violation##", jobViolation != null ? jobViolation.SummonsNumber : JobHistoryMessages.NoSetstring);
                JobViolation jobViolationResponse = this.rpoContext.JobViolations.Include("CreatedByEmployee").Include("LastModifiedByEmployee").Include("explanationOfCharges").FirstOrDefault(x => x.Id == id);
                return this.Ok(this.FormatDetails(jobViolationResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
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
        public IHttpActionResult PostJobViolation(JobViolationCreateUpdate jobViolationCreateUpdate)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddECBViolations))
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
                jobViolation.DateIssued = jobViolationCreateUpdate.DateIssued;
                jobViolation.CureDate = jobViolationCreateUpdate.CureDate;
                jobViolation.HearingDate = jobViolationCreateUpdate.HearingDate;
                jobViolation.HearingLocation = jobViolationCreateUpdate.HearingLocation;
                jobViolation.HearingResult = jobViolationCreateUpdate.HearingResult;
                jobViolation.StatusOfSummonsNotice = jobViolationCreateUpdate.StatusOfSummonsNotice;
                jobViolation.RespondentAddress = jobViolationCreateUpdate.RespondentAddress;
                jobViolation.InspectionLocation = jobViolationCreateUpdate.InspectionLocation;
                jobViolation.BalanceDue = jobViolationCreateUpdate.BalanceDue;
                jobViolation.RespondentName = jobViolationCreateUpdate.RespondentName;
                jobViolation.IssuingAgency = jobViolationCreateUpdate.IssuingAgency;
                jobViolation.IsCOC = jobViolationCreateUpdate.IsCOC;
                jobViolation.COCDate = jobViolationCreateUpdate.COCDate;
                if (jobViolation.Notes != jobViolationCreateUpdate.Notes)
                {
                    jobViolation.NotesLastModifiedDate = DateTime.UtcNow;
                    if (employee != null)
                    {
                        jobViolation.NotesLastModifiedBy = employee.Id;
                    }
                }
                jobViolation.Notes = jobViolationCreateUpdate.Notes;
                jobViolation.PaneltyAmount = jobViolationCreateUpdate.PaneltyAmount;
                jobViolation.ComplianceOn = jobViolationCreateUpdate.ComplianceOn;
                jobViolation.CertificationStatus = jobViolationCreateUpdate.CertificationStatus;
                jobViolation.LastModifiedDate = DateTime.UtcNow;
                jobViolation.CreatedDate = DateTime.UtcNow;
                jobViolation.IsFullyResolved = jobViolationCreateUpdate.IsFullyResolved;
                jobViolation.ResolvedDate = jobViolationCreateUpdate.ResolvedDate;
                jobViolation.PartyResponsible = jobViolationCreateUpdate.PartyResponsible;
                jobViolation.IdContact = jobViolationCreateUpdate.IdContact;
                jobViolation.ManualPartyResponsible = jobViolationCreateUpdate.ManualPartyResponsible;
                jobViolation.aggravated_level = jobViolationCreateUpdate.AggravatedLevel;
                jobViolation.violation_type = jobViolationCreateUpdate.ViolationType;
                jobViolation.Type_ECB_DOB = "ECB";
                jobViolation.Status = 1;
                jobViolation.IsNewMailsent = true;
                jobViolation.IsManually = true;
                int IdRfpAddress = this.rpoContext.Jobs.Where(x => x.Id == jobViolationCreateUpdate.IdJob).Select(y => y.IdRfpAddress).FirstOrDefault();
                jobViolation.BinNumber = this.rpoContext.RfpAddresses.Where(x => x.Id == IdRfpAddress).Select(y => y.BinNumber).FirstOrDefault();              
                jobViolation.ViolationDescription = jobViolationCreateUpdate.ViolationDescription;
                if (employee != null)
                {
                    jobViolation.CreatedBy = employee.Id;
                }             
                jobViolation.HearingTime = jobViolationCreateUpdate.HearingTime != null && !string.IsNullOrEmpty(jobViolationCreateUpdate.HearingTime) ? DateTime.ParseExact(jobViolationCreateUpdate.HearingTime, "HH:mm", CultureInfo.InvariantCulture) : DateTime.ParseExact("00:00", "HH:mm", CultureInfo.InvariantCulture);
                if (employee != null)
                {
                    jobViolation.CreatedBy = employee.Id;
                }

                this.rpoContext.JobViolations.Add(jobViolation);
                this.rpoContext.SaveChanges();


                if (jobViolationCreateUpdate.explanationOfCharges != null)
                {
                    foreach (var item in jobViolationCreateUpdate.explanationOfCharges)
                    {
                        JobViolationExplanationOfCharges explanationOfCharges = new JobViolationExplanationOfCharges();
                        explanationOfCharges.IdViolation = jobViolation.Id;
                        explanationOfCharges.Code = item.Code;
                        explanationOfCharges.CodeSection = item.CodeSection;
                        explanationOfCharges.Description = item.Description;
                        explanationOfCharges.PaneltyAmount = item.PaneltyAmount;
                        explanationOfCharges.IsFromAuth = item.IsFromAuth;
                        explanationOfCharges.LastModifiedDate = DateTime.UtcNow;
                        explanationOfCharges.CreatedDate = DateTime.UtcNow;
                        if (employee != null)
                        {
                            explanationOfCharges.CreatedBy = employee.Id;
                        }
                        this.rpoContext.JobViolationExplanationOfCharges.Add(explanationOfCharges);
                        this.rpoContext.SaveChanges();
                    }
                }
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
        /// Puts the job violation status.
        /// </summary>
        /// <param name="jobViolationStatusUpdate">The job violation status update.</param>
        /// <returns>update the status detail of job violation.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobViolationDetail))]
        [Route("api/jobviolations/status")]
        public IHttpActionResult PutJobViolationStatus(JobViolationStatusUpdate jobViolationStatusUpdate)
        {

            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddECBViolations))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                JobViolation jobViolation = rpoContext.JobViolations.FirstOrDefault(x => x.Id == jobViolationStatusUpdate.Id);

                if (jobViolation == null)
                {
                    return this.NotFound();
                }

                jobViolation.StatusOfSummonsNotice = jobViolationStatusUpdate.StatusOfSummonsNotice;
                jobViolation.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobViolation.LastModifiedBy = employee.Id;
                }

                this.rpoContext.SaveChanges();

                JobViolation jobViolationResponse = this.rpoContext.JobViolations.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == jobViolation.Id);
                return this.Ok(this.FormatDetails(jobViolationResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the job violation hearing date.
        /// </summary>
        /// <param name="jobViolationHearingDateUpdate">The job violation hearing date update.</param>
        /// <returns>update the job violation hearing date.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobViolationDetail))]
        [Route("api/jobviolations/HearingDate")]
        public IHttpActionResult PutJobViolationHearingDate(JobViolationHearingDateUpdate jobViolationHearingDateUpdate)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddECBViolations))
            {    

                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                JobViolation jobViolation = rpoContext.JobViolations.FirstOrDefault(x => x.Id == jobViolationHearingDateUpdate.Id);

                if (jobViolation == null)
                {
                    return this.NotFound();
                }

                jobViolation.HearingDate = jobViolationHearingDateUpdate.HearingDate;
                jobViolation.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobViolation.LastModifiedBy = employee.Id;
                }

                this.rpoContext.SaveChanges();

                JobViolation jobViolationResponse = this.rpoContext.JobViolations.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == jobViolation.Id);
                return this.Ok(this.FormatDetails(jobViolationResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the job violation Resolve date.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>update the job violation Resolve date.</returns>
        [Authorize]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobViolationDetail))]
        [Route("api/jobviolations/ResolveDate")]
        public IHttpActionResult PutJobViolationResolveDate(JobViolationResolveDateUpdate jobViolationResolveDateUpdate)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddECBViolations))
            {


                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                JobViolation jobViolation = rpoContext.JobViolations.FirstOrDefault(x => x.Id == jobViolationResolveDateUpdate.Id);

                if (jobViolation == null)
                {
                    return this.NotFound();
                }

                jobViolation.ResolvedDate = jobViolationResolveDateUpdate.ResolveDate;
                jobViolation.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobViolation.LastModifiedBy = employee.Id;
                }

                this.rpoContext.SaveChanges();

                JobViolation jobViolationResponse = this.rpoContext.JobViolations.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == jobViolation.Id);
                return this.Ok(this.FormatDetails(jobViolationResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the job violation FullyResolved.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>update the job violationFully Resolved.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobViolationDetail))]
        [Route("api/jobviolations/FullyResolved")]
        public IHttpActionResult PutJobViolationIsFullyResolved(JobViolationFullyResolvedUpdate jobViolationFullyResolvedUpdate)
        {

            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddECBViolations))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                JobViolation jobViolation = rpoContext.JobViolations.FirstOrDefault(x => x.Id == jobViolationFullyResolvedUpdate.Id);

                if (jobViolation == null)
                {
                    return this.NotFound();
                }

                jobViolation.IsFullyResolved = jobViolationFullyResolvedUpdate.IsFullyResolved;

                if (!jobViolation.IsFullyResolved)
                {
                    jobViolation.ResolvedDate = null;
                }
                jobViolation.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobViolation.LastModifiedBy = employee.Id;
                }

                this.rpoContext.SaveChanges();

                JobViolation jobViolationResponse = this.rpoContext.JobViolations.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == jobViolation.Id);
                return this.Ok(this.FormatDetails(jobViolationResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the violation details from oath.
        /// </summary>
        /// <param name="summonsNoticeNumber">The summons notice number.</param>
        /// <returns>get the job violation list against summonsnumber.</returns>
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobviolations/OATH")]
        [ResponseType(typeof(JobViolationGetInfoResponse))]
        public IHttpActionResult GetViolationDetailsFromOATH(string summonsNoticeNumber)
        {
            JobViolationGetInfoResponse violationGetInfoResponse = new JobViolationGetInfoResponse();
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("http://a820-ecbticketfinder.nyc.gov/getViolationbyID.action");

                string actualSummonsNumber = summonsNoticeNumber;
                summonsNoticeNumber = "0" + summonsNoticeNumber;

                var postData = "searchViolationObject.violationNo=" + summonsNoticeNumber;
                postData += "&searchViolationObject.searchOptions=All";
                postData += "&submit=Search";
                postData += "&searchViolationObject.searchType=violationNumber";

                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();



                var doc = new HtmlDocument();
                doc.LoadHtml(responseString);

                var descendants = doc.DocumentNode.Descendants();

                var summonsNumber = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Summons/Notice Number:	");
                var dateIssued = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Date Issued:");
                var issuingAgency = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Issuing Agency:");
                var respondentName = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Respondent Name:");

                var balanceDue = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Balance Due:");
                var inspectionLocation = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Inspection Location:");
                var respondentAddress = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Respondent Address:");
                var statusOfSummonsNotice = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Status of Summons/Notice:	");

                var hearingResult = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Hearing Result:	");
                var hearingLocation = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Hearing Location:	");
                var hearingDate = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Hearing Date:	");

                var code = descendants.FirstOrDefault(n => n.Name == "th" && n.InnerText == "Code");

                if (summonsNumber != null)
                {
                    violationGetInfoResponse.SummonsNumber = summonsNumber.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    violationGetInfoResponse.SummonsNumber = !string.IsNullOrEmpty(violationGetInfoResponse.SummonsNumber) ? Regex.Replace(violationGetInfoResponse.SummonsNumber, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                }

                if (violationGetInfoResponse.SummonsNumber != summonsNoticeNumber)
                {
                    throw new RpoBusinessException(StaticMessages.ViolationNotFound);
                }

                summonsNoticeNumber = actualSummonsNumber;
                List<ExplanationOfCharge> explanationOfCharges = new List<ExplanationOfCharge>();
                if (code != null)
                {
                    HtmlNode table = doc.DocumentNode.SelectSingleNode("//*[@id='vioInfrAccordion']");
                    table = table.ChildNodes.FirstOrDefault(x => x.Id == "infraDetails");
                    table = table.ChildNodes.FirstOrDefault(x => x.Id == "details");
                    int rowindex = 0;

                    foreach (HtmlNode row in table.ChildNodes.Where(x => x.Name == "tr"))
                    {
                        row.InnerHtml = row.InnerHtml.Replace("<tr class=\"whtbkgd\">", "").Replace("<tr class=\"altrow\">", "");
                        if (rowindex != 0)
                        {
                            List<HtmlNode> lst = row.ChildNodes.Where(x => x.Name == "tr").ToList();
                            List<HtmlNode> temp1 = new List<HtmlNode>();
                            for (int i = 0; i < lst.Count; i++)
                            {
                                if (lst[i].OuterHtml.Contains("whtbkgd") || lst[i].OuterHtml.Contains("altrow"))
                                {
                                    for (int j = 0; j < lst[i].ChildNodes.Where(x => x.Name == "tr").ToList().Count; j++)
                                    {
                                        HtmlNode ht;
                                        if (lst[i].ChildNodes.Where(x => x.Name == "tr").ToList()[j].OuterHtml.Contains("whtbkgd") || lst[i].ChildNodes.Where(x => x.Name == "tr").ToList()[j].OuterHtml.Contains("altrow"))
                                        {
                                            ht = lst[i].ChildNodes.Where(x => x.Name == "tr").ToList()[j];
                                        }
                                        else
                                        {
                                            ht = lst[i].ChildNodes.Where(x => x.Name == "tr").ToList()[j];
                                        }
                                        temp1.Add(ht);
                                    }
                                    lst.RemoveAt(i);
                                }
                            }
                            lst.AddRange(temp1);
                            foreach (HtmlNode childRow in lst)
                            {
                                HtmlNode item = childRow.ChildNodes.Where(x => x.Name == "tr") != null && childRow.ChildNodes.Where(x => x.Name == "tr").Count() > 0 ? childRow.ChildNodes.FirstOrDefault(x => x.Name == "tr") : childRow;
                                ExplanationOfCharge explanationOfCharge = new ExplanationOfCharge();
                                int cellindex = 0;
                                foreach (HtmlNode cell in item.ChildNodes.Where(x => x.Name == "td"))
                                {
                                    switch (cellindex)
                                    {
                                        case 0:
                                            explanationOfCharge.Code = cell.InnerText;
                                            explanationOfCharge.Code = !string.IsNullOrEmpty(explanationOfCharge.Code) ? Regex.Replace(explanationOfCharge.Code, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                            break;
                                        case 1:
                                            explanationOfCharge.CodeSection = cell.InnerText;
                                            explanationOfCharge.CodeSection = !string.IsNullOrEmpty(explanationOfCharge.CodeSection) ? Regex.Replace(explanationOfCharge.CodeSection, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                            break;
                                        case 2:
                                            explanationOfCharge.Description = cell.InnerText;
                                            explanationOfCharge.Description = !string.IsNullOrEmpty(explanationOfCharge.Description) ? Regex.Replace(explanationOfCharge.Description, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                            break;
                                        case 3:
                                            explanationOfCharge.PaneltyAmount = cell.InnerText;
                                            explanationOfCharge.PaneltyAmount = !string.IsNullOrEmpty(explanationOfCharge.PaneltyAmount) ? Regex.Replace(explanationOfCharge.PaneltyAmount, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                            explanationOfCharge.PaneltyAmount = explanationOfCharge.PaneltyAmount.Replace("$", "");
                                            break;
                                        default:
                                            break;
                                    }

                                    cellindex++;
                                }
                                explanationOfCharge.IsFromAuth = true;
                                explanationOfCharges.Add(explanationOfCharge);

                                if (childRow.ChildNodes.Where(x => x.Name == "tr").Count() > 0)
                                {
                                    var item1 = childRow.ChildNodes.Where(x => x.Name == "tr").ToList();

                                    foreach (var itemchld in item1)
                                    {
                                        ExplanationOfCharge explanationOfCharge1 = new ExplanationOfCharge();
                                        int cellindex1 = 0;
                                        foreach (HtmlNode cell in itemchld.ChildNodes.Where(x => x.Name == "td"))
                                        {
                                            switch (cellindex1)
                                            {
                                                case 0:
                                                    explanationOfCharge1.Code = cell.InnerText;
                                                    explanationOfCharge1.Code = !string.IsNullOrEmpty(explanationOfCharge1.Code) ? Regex.Replace(explanationOfCharge1.Code, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                                    break;
                                                case 1:
                                                    explanationOfCharge1.CodeSection = cell.InnerText;
                                                    explanationOfCharge1.CodeSection = !string.IsNullOrEmpty(explanationOfCharge1.CodeSection) ? Regex.Replace(explanationOfCharge1.CodeSection, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                                    break;
                                                case 2:
                                                    explanationOfCharge1.Description = cell.InnerText;
                                                    explanationOfCharge1.Description = !string.IsNullOrEmpty(explanationOfCharge1.Description) ? Regex.Replace(explanationOfCharge1.Description, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                                    break;
                                                case 3:
                                                    explanationOfCharge1.PaneltyAmount = cell.InnerText;
                                                    explanationOfCharge1.PaneltyAmount = !string.IsNullOrEmpty(explanationOfCharge1.PaneltyAmount) ? Regex.Replace(explanationOfCharge1.PaneltyAmount, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                                    explanationOfCharge1.PaneltyAmount = explanationOfCharge1.PaneltyAmount.Replace("$", "");
                                                    break;
                                                default:
                                                    break;
                                            }

                                            cellindex1++;
                                        }
                                        explanationOfCharge1.IsFromAuth = true;
                                        explanationOfCharges.Add(explanationOfCharge1);
                                    }
                                }
                            }
                        }
                        rowindex++;
                    }
                    violationGetInfoResponse.ExplanationOfCharges = explanationOfCharges;
                }

                if (dateIssued != null)
                {
                    violationGetInfoResponse.DateIssued = dateIssued.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    violationGetInfoResponse.DateIssued = !string.IsNullOrEmpty(violationGetInfoResponse.DateIssued) ? Regex.Replace(violationGetInfoResponse.DateIssued, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                    violationGetInfoResponse.DateIssued = !string.IsNullOrEmpty(violationGetInfoResponse.DateIssued) ? Regex.Replace(violationGetInfoResponse.DateIssued, @"\s+", " ", RegexOptions.Multiline).Trim() : string.Empty;
                }

                if (issuingAgency != null)
                {
                    violationGetInfoResponse.IssuingAgency = issuingAgency.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    violationGetInfoResponse.IssuingAgency = !string.IsNullOrEmpty(violationGetInfoResponse.IssuingAgency) ? Regex.Replace(violationGetInfoResponse.IssuingAgency, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                    violationGetInfoResponse.IssuingAgency = !string.IsNullOrEmpty(violationGetInfoResponse.IssuingAgency) ? Regex.Replace(violationGetInfoResponse.IssuingAgency, @"\s+", " ", RegexOptions.Multiline).Trim() : string.Empty;
                }

                if (respondentName != null)
                {
                    violationGetInfoResponse.RespondentName = respondentName.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    violationGetInfoResponse.RespondentName = !string.IsNullOrEmpty(violationGetInfoResponse.RespondentName) ? Regex.Replace(violationGetInfoResponse.RespondentName, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                    violationGetInfoResponse.RespondentName = !string.IsNullOrEmpty(violationGetInfoResponse.RespondentName) ? Regex.Replace(violationGetInfoResponse.RespondentName, @"\s+", " ", RegexOptions.Multiline).Trim() : string.Empty;
                }

                if (balanceDue != null)
                {
                    violationGetInfoResponse.BalanceDue = balanceDue.ParentNode.ChildNodes.Last(c => c.Name == "td").ChildNodes[0].InnerText;
                    violationGetInfoResponse.BalanceDue = !string.IsNullOrEmpty(violationGetInfoResponse.BalanceDue) ? Regex.Replace(violationGetInfoResponse.BalanceDue, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                    violationGetInfoResponse.BalanceDue = !string.IsNullOrEmpty(violationGetInfoResponse.BalanceDue) ? Regex.Replace(violationGetInfoResponse.BalanceDue, @"\s+", " ", RegexOptions.Multiline).Trim() : string.Empty;
                }

                if (inspectionLocation != null)
                {
                    violationGetInfoResponse.InspectionLocation = inspectionLocation.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    violationGetInfoResponse.InspectionLocation = !string.IsNullOrEmpty(violationGetInfoResponse.InspectionLocation) ? Regex.Replace(violationGetInfoResponse.InspectionLocation, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                    violationGetInfoResponse.InspectionLocation = !string.IsNullOrEmpty(violationGetInfoResponse.InspectionLocation) ? Regex.Replace(violationGetInfoResponse.InspectionLocation, @"\s+", " ", RegexOptions.Multiline).Trim() : string.Empty;
                }

                if (respondentAddress != null)
                {
                    violationGetInfoResponse.RespondentAddress = respondentAddress.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    violationGetInfoResponse.RespondentAddress = !string.IsNullOrEmpty(violationGetInfoResponse.RespondentAddress) ? Regex.Replace(violationGetInfoResponse.RespondentAddress, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                    violationGetInfoResponse.RespondentAddress = !string.IsNullOrEmpty(violationGetInfoResponse.RespondentAddress) ? Regex.Replace(violationGetInfoResponse.RespondentAddress, @"\s+", " ", RegexOptions.Multiline).Trim() : string.Empty;
                }

                if (statusOfSummonsNotice != null)
                {
                    violationGetInfoResponse.StatusOfSummonsNotice = statusOfSummonsNotice.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    violationGetInfoResponse.StatusOfSummonsNotice = !string.IsNullOrEmpty(violationGetInfoResponse.StatusOfSummonsNotice) ? Regex.Replace(violationGetInfoResponse.StatusOfSummonsNotice, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                    violationGetInfoResponse.StatusOfSummonsNotice = !string.IsNullOrEmpty(violationGetInfoResponse.StatusOfSummonsNotice) ? Regex.Replace(violationGetInfoResponse.StatusOfSummonsNotice, @"\s+", " ", RegexOptions.Multiline).Trim() : string.Empty;
                }

                if (hearingResult != null)
                {
                    violationGetInfoResponse.HearingResult = hearingResult.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    violationGetInfoResponse.HearingResult = !string.IsNullOrEmpty(violationGetInfoResponse.HearingResult) ? Regex.Replace(violationGetInfoResponse.HearingResult, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                    violationGetInfoResponse.HearingResult = !string.IsNullOrEmpty(violationGetInfoResponse.HearingResult) ? Regex.Replace(violationGetInfoResponse.HearingResult, @"\s+", " ", RegexOptions.Multiline).Trim() : string.Empty;
                }

                if (hearingLocation != null)
                {
                    violationGetInfoResponse.HearingLocation = hearingLocation.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    violationGetInfoResponse.HearingLocation = !string.IsNullOrEmpty(violationGetInfoResponse.HearingLocation) ? Regex.Replace(violationGetInfoResponse.HearingLocation, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                    violationGetInfoResponse.HearingLocation = !string.IsNullOrEmpty(violationGetInfoResponse.HearingLocation) ? Regex.Replace(violationGetInfoResponse.HearingLocation, @"\s+", " ", RegexOptions.Multiline).Trim() : string.Empty;
                }

                if (hearingDate != null)
                {
                    violationGetInfoResponse.HearingDate = hearingDate.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    violationGetInfoResponse.HearingDate = !string.IsNullOrEmpty(violationGetInfoResponse.HearingDate) ? Regex.Replace(violationGetInfoResponse.HearingDate, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                    violationGetInfoResponse.HearingDate = !string.IsNullOrEmpty(violationGetInfoResponse.HearingDate) ? Regex.Replace(violationGetInfoResponse.HearingDate, @"\s+", " ", RegexOptions.Multiline).Trim() : string.Empty;
                }

                string bisURL = $"http://a810-bisweb.nyc.gov/bisweb/ECBQueryByNumberServlet?ecbin={summonsNoticeNumber}&go7=+GO+&requestid=0";
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                   | SecurityProtocolType.Tls11
                                   | SecurityProtocolType.Tls12;
                HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlWeb().Load(bisURL);

                var bisDescendants = htmlDocument.DocumentNode.Descendants();

                var certificationStatusLabel = bisDescendants.FirstOrDefault(n => n.Name == "td" && n.InnerHtml.Contains("Certification Status:"));
                var complianceOnLabel = bisDescendants.FirstOrDefault(n => n.Name == "td" && n.InnerHtml.Contains("Compliance On:"));
                var violationresolved = bisDescendants.FirstOrDefault(n => n.Name == "td" && n.InnerHtml.Contains("ECB Violation Summary"));

                if (certificationStatusLabel != null && certificationStatusLabel.ParentNode != null && certificationStatusLabel.ParentNode.ChildNodes != null
                    && certificationStatusLabel.ParentNode.ChildNodes.Count > 5)
                {
                    string certificationStatus = certificationStatusLabel.ParentNode.ChildNodes[5].InnerHtml;
                    violationGetInfoResponse.CertificationStatus = certificationStatus;
                    violationGetInfoResponse.CertificationStatus = !string.IsNullOrEmpty(violationGetInfoResponse.CertificationStatus) ? Regex.Replace(violationGetInfoResponse.CertificationStatus, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                }

                if (complianceOnLabel != null)
                {
                    string complianceOn = complianceOnLabel.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    violationGetInfoResponse.ComplianceOn = complianceOn;
                    violationGetInfoResponse.ComplianceOn = !string.IsNullOrEmpty(violationGetInfoResponse.ComplianceOn) ? Regex.Replace(violationGetInfoResponse.ComplianceOn, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                }

                if (violationresolved != null)
                {
                    string resolvedStatus = violationresolved.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    if (resolvedStatus.Contains("RESOLVED"))
                    {
                        violationGetInfoResponse.IsFullyResolved = true;
                    }
                    else
                    {
                        violationGetInfoResponse.IsFullyResolved = false;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The remote server returned an error: (503) Server Unavailable"))
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Unable to connect to OATH. The remote system may be down. Please try again in some time" }));
                }
            }
            return Ok(violationGetInfoResponse);
        }

        /// <summary>
        /// Deletes the job violation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>delete the job violation.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobViolationDetail))]
        public IHttpActionResult DeleteJobViolation(int id)
        {

            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteECBViolations))
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
                                                     .Replace("##Violation##", jobViolation != null ? jobViolation.SummonsNumber : JobHistoryMessages.NoSetstring);


                var jobViolationExplanationOfCharges = this.rpoContext.JobViolationExplanationOfCharges.Where(x => x.IdViolation == id);
                if (jobViolationExplanationOfCharges.Any())
                {
                    this.rpoContext.JobViolationExplanationOfCharges.RemoveRange(jobViolationExplanationOfCharges);
                }

                var jobViolationDocuments = this.rpoContext.JobViolationDocuments.Where(x => x.IdJobViolation == id);
                if (jobViolationDocuments.Any())
                {
                    this.rpoContext.JobViolationDocuments.RemoveRange(jobViolationDocuments);
                }
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
            jobViolationDTO.SummonsNumber = jobViolation.SummonsNumber;
            jobViolationDTO.BalanceDue = jobViolation.BalanceDue;
            jobViolationDTO.DateIssued = jobViolation.DateIssued;
            jobViolationDTO.CureDate = jobViolation.CureDate;
            jobViolationDTO.RespondentName = jobViolation.RespondentName;
            jobViolationDTO.HearingDate = jobViolation.HearingDate;
            jobViolationDTO.HearingLocation = jobViolation.HearingLocation;
            jobViolationDTO.HearingResult = jobViolation.HearingResult;
            jobViolationDTO.InspectionLocation = jobViolation.InspectionLocation;
            jobViolationDTO.IssuingAgency = jobViolation.IssuingAgency;
            jobViolationDTO.RespondentAddress = jobViolation.RespondentAddress;
            jobViolationDTO.StatusOfSummonsNotice = jobViolation.StatusOfSummonsNotice;
            jobViolationDTO.ComplianceOn = jobViolation.ComplianceOn;
            jobViolationDTO.ResolvedDate = jobViolation.ResolvedDate;
            jobViolationDTO.IsFullyResolved = jobViolation.IsFullyResolved;
            jobViolationDTO.CertificationStatus = jobViolation.CertificationStatus;
            jobViolationDTO.Notes = jobViolation.Notes;
            jobViolationDTO.IsCOC = jobViolation.IsCOC;
            jobViolationDTO.COCDate = jobViolation.COCDate;
            jobViolationDTO.PaneltyAmount = jobViolation.PaneltyAmount;
            jobViolationDTO.CreatedBy = jobViolation.CreatedBy;
            jobViolationDTO.LastModifiedBy = jobViolation.LastModifiedBy != null ? jobViolation.LastModifiedBy : jobViolation.CreatedBy;
            jobViolationDTO.CreatedByEmployeeName = jobViolation.CreatedByEmployee != null ? jobViolation.CreatedByEmployee.FirstName + " " + jobViolation.CreatedByEmployee.LastName : string.Empty;
            jobViolationDTO.LastModifiedByEmployeeName = jobViolation.LastModifiedByEmployee != null ? jobViolation.LastModifiedByEmployee.FirstName + " " + jobViolation.LastModifiedByEmployee.LastName : (jobViolation.CreatedByEmployee != null ? jobViolation.CreatedByEmployee.FirstName + " " + jobViolation.CreatedByEmployee.LastName : string.Empty);
            jobViolationDTO.CreatedDate = jobViolation.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.CreatedDate;
            jobViolationDTO.LastModifiedDate = jobViolation.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (jobViolation.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.CreatedDate);
            jobViolationDTO.Code = jobViolation.explanationOfCharges != null ? String.Join(", ", jobViolation.explanationOfCharges.Select(c => c.Code)) : string.Empty;
            jobViolationDTO.CodeSection = jobViolation.explanationOfCharges != null ? String.Join(", ", jobViolation.explanationOfCharges.Select(c => c.CodeSection)) : string.Empty;
            jobViolationDTO.Description = jobViolation.explanationOfCharges != null ? String.Join(", ", jobViolation.explanationOfCharges.Select(c => c.Description)) : string.Empty;
            jobViolationDTO.NotesLastModifiedDate = jobViolation.NotesLastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.NotesLastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.NotesLastModifiedDate;
            jobViolationDTO.NotesLastModifiedByEmployeeName = jobViolation.NotesLastModifiedByEmployee != null ? jobViolation.NotesLastModifiedByEmployee.FirstName + " " + jobViolation.NotesLastModifiedByEmployee.LastName : string.Empty;

            jobViolationDTO.LastModifiedDate = jobViolation.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (jobViolation.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.CreatedDate);
            jobViolationDTO.Disposition_Date = jobViolation.Disposition_Date != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.Disposition_Date), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.Disposition_Date;
            jobViolationDTO.Disposition_Comments = jobViolation.Disposition_Comments;
            jobViolationDTO.device_number = jobViolation.device_number;
            jobViolationDTO.ViolationDescription = jobViolation.ViolationDescription;
            jobViolationDTO.ecb_number = jobViolation.ECBnumber;
            jobViolationDTO.violation_number = jobViolation.violation_number;
            jobViolationDTO.violation_category = jobViolation.violation_category;
            jobViolationDTO.BinNumber = jobViolation.BinNumber;
            jobViolationDTO.PartyResponsible = jobViolation.PartyResponsible;
            jobViolationDTO.IdContact = jobViolation.IdContact;
            jobViolationDTO.ManualPartyResponsible = jobViolation.ManualPartyResponsible;
            jobViolationDTO.HearingTime = jobViolation.HearingTime;
            jobViolationDTO.violation_type_code = jobViolation.violation_type_code;
            jobViolationDTO.AggravatedLevel = jobViolation.aggravated_level;
            jobViolationDTO.violation_type = jobViolation.violation_type;
            jobViolationDTO.Status = jobViolation.Status;
            jobViolation.ISNViolation = jobViolation.ISNViolation;


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
            jobViolationDetail.BalanceDue = jobViolation.BalanceDue;
            jobViolationDetail.DateIssued = jobViolation.DateIssued;
            jobViolationDetail.CureDate = jobViolation.CureDate;
            jobViolationDetail.RespondentName = jobViolation.RespondentName;
            jobViolationDetail.HearingDate = jobViolation.HearingDate;
            jobViolationDetail.HearingLocation = jobViolation.HearingLocation;
            jobViolationDetail.HearingResult = jobViolation.HearingResult;
            jobViolationDetail.InspectionLocation = jobViolation.InspectionLocation;
            jobViolationDetail.IssuingAgency = jobViolation.IssuingAgency;
            jobViolationDetail.RespondentAddress = jobViolation.RespondentAddress;
            jobViolationDetail.StatusOfSummonsNotice = jobViolation.StatusOfSummonsNotice;
            jobViolationDetail.Notes = jobViolation.Notes;
            jobViolationDetail.IsCOC = jobViolation.IsCOC;
            jobViolationDetail.COCDate = jobViolation.COCDate;
            jobViolationDetail.ResolvedDate = jobViolation.ResolvedDate;
            jobViolationDetail.IsFullyResolved = jobViolation.IsFullyResolved;
            jobViolationDetail.PaneltyAmount = jobViolation.PaneltyAmount;
            jobViolationDetail.ComplianceOn = jobViolation.ComplianceOn;
            jobViolationDetail.CertificationStatus = jobViolation.CertificationStatus;
            jobViolationDetail.CreatedBy = jobViolation.CreatedBy;
            jobViolationDetail.LastModifiedBy = jobViolation.LastModifiedBy != null ? jobViolation.LastModifiedBy : jobViolation.CreatedBy;
            jobViolationDetail.CreatedByEmployeeName = jobViolation.CreatedByEmployee != null ? jobViolation.CreatedByEmployee.FirstName + " " + jobViolation.CreatedByEmployee.LastName : string.Empty;
            jobViolationDetail.LastModifiedByEmployeeName = jobViolation.LastModifiedByEmployee != null ? jobViolation.LastModifiedByEmployee.FirstName + " " + jobViolation.LastModifiedByEmployee.LastName : (jobViolation.CreatedByEmployee != null ? jobViolation.CreatedByEmployee.FirstName + " " + jobViolation.CreatedByEmployee.LastName : string.Empty);
            jobViolationDetail.CreatedDate = jobViolation.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.CreatedDate;
            jobViolationDetail.NotesLastModifiedDate = jobViolation.NotesLastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.NotesLastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.NotesLastModifiedDate;
            jobViolationDetail.NotesLastModifiedByEmployeeName = jobViolation.NotesLastModifiedByEmployee != null ? jobViolation.NotesLastModifiedByEmployee.FirstName + " " + jobViolation.NotesLastModifiedByEmployee.LastName : string.Empty;
            jobViolationDetail.LastModifiedDate = jobViolation.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (jobViolation.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.CreatedDate);
            jobViolationDetail.Disposition_Date = jobViolation.Disposition_Date != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.Disposition_Date), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.Disposition_Date;
            jobViolationDetail.Disposition_Comments = jobViolation.Disposition_Comments;
            jobViolationDetail.device_number = jobViolation.device_number;
            jobViolationDetail.ViolationDescription = jobViolation.ViolationDescription;
            jobViolationDetail.ecb_number = jobViolation.ECBnumber;
            jobViolationDetail.violation_number = jobViolation.violation_number;
            jobViolationDetail.violation_category = jobViolation.violation_category;
            jobViolationDetail.BinNumber = jobViolation.BinNumber;
            jobViolationDetail.PartyResponsible = jobViolation.PartyResponsible;
            jobViolationDetail.IdContact = jobViolation.IdContact;
            jobViolationDetail.ManualPartyResponsible = jobViolation.ManualPartyResponsible;
            jobViolationDetail.HearingTime = jobViolation.HearingTime;
            jobViolationDetail.violation_type_code = jobViolation.violation_type_code;
            jobViolationDetail.AggravatedLevel = jobViolation.aggravated_level;
            jobViolationDetail.ViolationType = jobViolation.violation_type;
            jobViolationDetail.Status = jobViolation.Status;
            jobViolationDetail.IsManually = jobViolation.IsManually;
            jobViolationDetail.Type_ECB_DOB = jobViolation.Type_ECB_DOB;
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

        /// <summary>
        /// Jobs the violation number exists.
        /// </summary>
        /// <param name="summonsNumber">The summons number.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private List<JobViolation> JobViolationNumberExists(string summonsNumber, int id)
        {
            var jobViolations = this.rpoContext.JobViolations.Where(e => e.SummonsNumber == summonsNumber && e.Id != id).ToList();

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

        /// <summary>
        /// Jobs the Violation Explanation Of Charges Exists exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool JobViolationExplanationOfChargesExists(int id)
        {
            return this.rpoContext.JobViolationExplanationOfCharges.Count(e => e.Id == id) > 0;
        }

        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/JobViolationStat")]
        public IHttpActionResult GetviolationStat()
        {
            Tools.ClsApplicationStatus objapp = new Tools.ClsApplicationStatus();

            var client = new SodaClient("https://data.cityofnewyork.us", "7tPQriTSRiloiKdWIJoODgReN");
            var dataset = client.GetResource<object>("gszd-efwt");
            var rows = dataset.GetRows(limit: 1000);

            List<JobViolation> liJobViolation = rpoContext.JobViolations.Where(x => x.IsFullyResolved == false && x.Job.Status == JobStatus.Active).ToList();

            //Iterate over each Summons Number and fetch the data from site

            string qry = string.Empty;
            int i = 0;
            foreach (var jobApplication in liJobViolation)
            {
                if (!string.IsNullOrEmpty(jobApplication.SummonsNumber))
                {
                    string summonsNoticeNumber = "0" + jobApplication.SummonsNumber;

                    if (i == 0)
                    {
                        qry = qry + "(ticket_number='" + summonsNoticeNumber + "'";
                    }
                    else
                    {
                        qry = qry + " OR " + "ticket_number='" + summonsNoticeNumber + "'";
                    }
                    i++;
                }
            }
            qry = qry + ")";

            var soql = new SoqlQuery().Select("ticket_number", "hearing_date", "balance_due", "hearing_status", "hearing_result", "issuing_agency", "respondent_first_name", "respondent_last_name", "respondent_address_borough", "respondent_address_house", "respondent_address_street_name", "respondent_address_city", "respondent_address_zip_code", "respondent_address_state_name", "scheduled_hearing_location").Where(qry);

            var results = dataset.Query(soql);

            return Ok(results);
        }

        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/JobViolationStatBIs")]
        public IHttpActionResult GetViolationStatBIS()
        {
            Tools.ClsApplicationStatus objapp = new Tools.ClsApplicationStatus();

            var client = new SodaClient("https://data.cityofnewyork.us", "7tPQriTSRiloiKdWIJoODgReN");
            var dataset = client.GetResource<object>("6bgk-3dad");
            var rows = dataset.GetRows(limit: 1000);

            List<JobViolation> liJobViolation = rpoContext.JobViolations.Where(x => x.IsFullyResolved == false && x.Job.Status == JobStatus.Active).ToList();

            //Iterate over each Summons Number and fetch the data from site

            string qry = string.Empty;
            int i = 0;
            foreach (var jobApplication in liJobViolation)
            {
                if (!string.IsNullOrEmpty(jobApplication.SummonsNumber))
                {
                    string summonsNoticeNumber = jobApplication.SummonsNumber;

                    if (i == 0)
                    {
                        qry = qry + "(ecb_violation_number='" + summonsNoticeNumber + "'";
                    }
                    else
                    {
                        qry = qry + " OR " + "ecb_violation_number='" + summonsNoticeNumber + "'";
                    }
                    i++;
                }
            }
            qry = qry + ")";

            var soql = new SoqlQuery().Select("ecb_violation_number", "certification_status").Where(qry);

            var results = dataset.Query(soql);

            return Ok(results);
        }

    }
}