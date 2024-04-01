// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-09-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-31-2018
// ***********************************************************************
// <copyright file="JobApplicationWorkPermitsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Application Work Permits Controller.</summary>
// ***********************************************************************

/// <summary>
/// The JobApplicationWorkPermits namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobApplicationWorkPermits
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity.Infrastructure;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Enums;
    using Filters;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using iTextSharp.text.pdf.parser;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Model.Models.Enums;
    using Newtonsoft.Json;

    /// <summary>
    /// Class Job Application Work Permits Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class JobApplicationWorkPermitsController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the job application work permits.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the job application work permits List.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetJobApplicationWorkPermits([FromUri] JobApplicationWorkPermitDataTableParameters dataTableParameters)
        {
            var jobApplicationWorkPermitTypes = rpoContext.JobApplicationWorkPermitTypes.Include("JobApplication.JobApplicationType")
               .Include("JobWorkType")
               .Include("CreatedByEmployee")
               .Include("LastModifiedByEmployee")
               .Include("ContactResponsible").Where(c => c.IsCompleted == null || c.IsCompleted == false).AsQueryable();

            //  string dropboxDocumentUrl = Properties.Settings.Default.DropboxExternalUrl + "/" + jobDocument.IdJob + "/" + Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;

            var recordsTotal = jobApplicationWorkPermitTypes.Count();
            var recordsFiltered = recordsTotal;

            if (dataTableParameters.IdJobApplication != null)
            {
                jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes
                    .Where(c => c.IdJobApplication == dataTableParameters.IdJobApplication && c.JobApplication.IdJob == dataTableParameters.IdJob);
            }

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

            var result = jobApplicationWorkPermitTypes
                .AsEnumerable()
                .Select(jobApplicationWorkPermitResponse => new JobApplicationWorkPermitDTO()
                {
                    Id = jobApplicationWorkPermitResponse.Id,
                    DetailURL = jobApplicationWorkPermitResponse != null && jobApplicationWorkPermitResponse.DefaultUrl != null ? jobApplicationWorkPermitResponse.DefaultUrl : string.Empty,
                    IdJobApplication = jobApplicationWorkPermitResponse.IdJobApplication,
                    IdJob = jobApplicationWorkPermitResponse.JobApplication.IdJob,
                    JobApplicationNumber = jobApplicationWorkPermitResponse.JobApplication.ApplicationNumber,
                    JobApplicationFor = jobApplicationWorkPermitResponse.JobApplication.ApplicationFor,
                    JobApplicationStatusId = jobApplicationWorkPermitResponse.JobApplication.IdApplicationStatus,
                    JobApplicationStatus = jobApplicationWorkPermitResponse.JobApplication.ApplicationStatus != null ? jobApplicationWorkPermitResponse.JobApplication.ApplicationStatus.Name : string.Empty,
                    JobApplicationFloor = jobApplicationWorkPermitResponse.JobApplication.FloorWorking,
                    JobApplicationTypeName = jobApplicationWorkPermitResponse.JobApplication != null && jobApplicationWorkPermitResponse.JobApplication.JobApplicationType != null ? jobApplicationWorkPermitResponse.JobApplication.JobApplicationType.Description : string.Empty,
                    IdJobApplicationType = jobApplicationWorkPermitResponse.JobApplication.IdJobApplicationType,
                    IdJobWorkType = jobApplicationWorkPermitResponse.IdJobWorkType,
                    JobWorkTypeDescription = jobApplicationWorkPermitResponse.JobWorkType != null ? jobApplicationWorkPermitResponse.JobWorkType.Description : string.Empty,
                    JobWorkTypeContent = jobApplicationWorkPermitResponse.JobWorkType != null ? jobApplicationWorkPermitResponse.JobWorkType.Content : string.Empty,
                    //JobWorkTypeNumber = jobApplicationWorkPermitResponse.JobWorkType != null ? jobApplicationWorkPermitResponse.JobWorkType.Number : string.Empty,
                    JobWorkTypeCode = jobApplicationWorkPermitResponse.JobWorkType != null ? jobApplicationWorkPermitResponse.JobWorkType.Code : string.Empty,
                    Code = jobApplicationWorkPermitResponse.Code,
                    EstimatedCost = jobApplicationWorkPermitResponse.EstimatedCost,
                    RenewalFee = jobApplicationWorkPermitResponse.RenewalFee,
                    HasSiteSafetyCoordinator = jobApplicationWorkPermitResponse.HasSiteSafetyCoordinator,
                    HasSiteSafetyManager = jobApplicationWorkPermitResponse.HasSiteSafetyManager,
                    HasSuperintendentofconstruction = jobApplicationWorkPermitResponse.HasSuperintendentofconstruction,
                    PreviousPermitNumber = jobApplicationWorkPermitResponse.PreviousPermitNumber,
                    PermitNumber = jobApplicationWorkPermitResponse.PermitNumber,
                    Withdrawn = jobApplicationWorkPermitResponse.Withdrawn != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.Withdrawn), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.Withdrawn,
                    Filed = jobApplicationWorkPermitResponse.Filed != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.Filed), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.Filed,
                    Issued = jobApplicationWorkPermitResponse.Issued != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.Issued), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.Issued,
                    Expires = jobApplicationWorkPermitResponse.Expires != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.Expires), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.Expires,
                    SignedOff = jobApplicationWorkPermitResponse.SignedOff != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.SignedOff), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.SignedOff,
                    //Withdrawn = jobApplicationWorkPermitResponse.Withdrawn != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.Withdrawn), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.Withdrawn,
                    //Filed = jobApplicationWorkPermitResponse.Filed != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.Filed), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.Filed,
                    //Issued = jobApplicationWorkPermitResponse.Issued != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.Issued), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.Issued,
                    //Expires = jobApplicationWorkPermitResponse.Expires != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.Expires), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.Expires,
                    //SignedOff = jobApplicationWorkPermitResponse.SignedOff != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.SignedOff), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.SignedOff,
                    //WorkDescription = jobApplicationWorkPermitResponse.WorkDescription,
                    IdResponsibility = jobApplicationWorkPermitResponse.IdResponsibility,
                    //  DocumentPath = jobApplicationWorkPermitResponse.DocumentPath != null ? Properties.Settings.Default.DropboxExternalUrl + "/" + jobApplicationWorkPermitResponse.JobApplication.IdJob + "/" + jobApplicationWorkPermitResponse.Id.ToString() + "_" + jobApplicationWorkPermitResponse.DocumentPath : string.Empty,

                    //DocumentPath = (rpoContext.JobDocuments.Where(d => d.IdJob == jobApplicationWorkPermitResponse.JobApplication.IdJob && d.IdJobApplication == jobApplicationWorkPermitResponse.IdJobApplication && d.IdJobApplicationWorkPermitType == jobApplicationWorkPermitResponse.Id).Select(d => d.DocumentPath).FirstOrDefault() != null) ? Properties.Settings.Default.DropboxExternalUrl + "/" + jobApplicationWorkPermitResponse.JobApplication.IdJob + "/" + rpoContext.JobDocuments.Where(d => d.IdJob == jobApplicationWorkPermitResponse.JobApplication.IdJob && d.IdJobApplication == jobApplicationWorkPermitResponse.IdJobApplication && d.IdJobApplicationWorkPermitType == jobApplicationWorkPermitResponse.Id).Select(d => d.Id.ToString() + "_" + d.DocumentPath).FirstOrDefault() : string.Empty,
                    DocumentPath = (rpoContext.JobDocuments.Where(d => d.IdJob == jobApplicationWorkPermitResponse.JobApplication.IdJob && d.IdJobApplication == jobApplicationWorkPermitResponse.IdJobApplication && d.IdJobApplicationWorkPermitType == jobApplicationWorkPermitResponse.Id && d.DocumentPath == jobApplicationWorkPermitResponse.DocumentPath).Select(d => d.DocumentPath).FirstOrDefault() != null) ? Properties.Settings.Default.DropboxExternalUrl + "/" + jobApplicationWorkPermitResponse.JobApplication.IdJob + "/" + rpoContext.JobDocuments.Where(d => d.IdJob == jobApplicationWorkPermitResponse.JobApplication.IdJob && d.IdJobApplication == jobApplicationWorkPermitResponse.IdJobApplication && d.IdJobApplicationWorkPermitType == jobApplicationWorkPermitResponse.Id && d.DocumentPath == jobApplicationWorkPermitResponse.DocumentPath).Select(d => d.Id.ToString() + "_" + d.DocumentPath).FirstOrDefault() : string.Empty,
                    //DocumentPath = jobApplicationWorkPermitResponse.DocumentPath != null ? APIUrl + "/" + Properties.Settings.Default.DOTWorkPermitDocument + "/" + jobApplicationWorkPermitResponse.Id + "_" + jobApplicationWorkPermitResponse.DocumentPath : string.Empty,
                    LastModifiedDate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)),
                    CompanyResponsible = jobApplicationWorkPermitResponse.ContactResponsible != null && jobApplicationWorkPermitResponse.ContactResponsible.Company != null ? jobApplicationWorkPermitResponse.ContactResponsible.Company.Name : string.Empty,
                    PersonalResponsible = jobApplicationWorkPermitResponse.ContactResponsible != null && jobApplicationWorkPermitResponse.ContactResponsible.FirstName != null ? jobApplicationWorkPermitResponse.ContactResponsible.FirstName + (jobApplicationWorkPermitResponse.ContactResponsible.LastName != null ? jobApplicationWorkPermitResponse.ContactResponsible.LastName : string.Empty) : string.Empty,
                    PermitType = jobApplicationWorkPermitResponse.PermitType != null? jobApplicationWorkPermitResponse.PermitType : string.Empty,
                    ForPurposeOf = jobApplicationWorkPermitResponse.ForPurposeOf,
                    EquipmentType = jobApplicationWorkPermitResponse.EquipmentType,
                    PlumbingSignedOff = jobApplicationWorkPermitResponse.PlumbingSignedOff != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.PlumbingSignedOff), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.PlumbingSignedOff,
                    FinalElevator = jobApplicationWorkPermitResponse.FinalElevator != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.FinalElevator), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.FinalElevator,
                    TempElevator = jobApplicationWorkPermitResponse.TempElevator != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.TempElevator), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.TempElevator,
                    ConstructionSignedOff = jobApplicationWorkPermitResponse.ConstructionSignedOff != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.ConstructionSignedOff), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.ConstructionSignedOff,
                    //PlumbingSignedOff = jobApplicationWorkPermitResponse.PlumbingSignedOff != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.PlumbingSignedOff), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.PlumbingSignedOff,
                    //FinalElevator = jobApplicationWorkPermitResponse.FinalElevator != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.FinalElevator), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.FinalElevator,
                    //TempElevator = jobApplicationWorkPermitResponse.TempElevator != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.TempElevator), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.TempElevator,
                    //ConstructionSignedOff = jobApplicationWorkPermitResponse.ConstructionSignedOff != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.ConstructionSignedOff), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.ConstructionSignedOff,
                    Permittee = jobApplicationWorkPermitResponse.Permittee,
                    IsPGL = jobApplicationWorkPermitResponse.IsPGL,
                    CreatedBy = jobApplicationWorkPermitResponse.CreatedBy,
                    CreatedByEmployeeName = jobApplicationWorkPermitResponse.CreatedByEmployee != null ? jobApplicationWorkPermitResponse.CreatedByEmployee.FirstName + " " + jobApplicationWorkPermitResponse.CreatedByEmployee.LastName : string.Empty,
                    CreatedDate = jobApplicationWorkPermitResponse.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.CreatedDate,
                    LastModifiedByEmployeeName = jobApplicationWorkPermitResponse.LastModifiedByEmployee != null ? jobApplicationWorkPermitResponse.LastModifiedByEmployee.FirstName + " " + jobApplicationWorkPermitResponse.LastModifiedByEmployee.LastName : string.Empty,
                })
                .AsQueryable()
                .DataTableParameters(dataTableParameters, out recordsFiltered)
                .OrderByDescending(x => x.LastModifiedDate)
                .ToArray();

            return Ok(new DataTableResponse
            {
                Draw = dataTableParameters.Draw,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal,
                Data = result
            });
        }

        /// <summary>
        /// Gets the job application work permit.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the job application work permits against the job applications.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobApplicationWorkPermitDTO))]
        public IHttpActionResult GetJobApplicationWorkPermit(int id)
        {
            //JobApplicationWorkPermitType jobApplicationWorkPermitResponse = db.JobApplicationWorkPermitTypes.Where(x => x.Id == id).FirstOrDefault();
            JobApplicationWorkPermitType jobApplicationWorkPermitResponse = rpoContext.JobApplicationWorkPermitTypes.Include("JobApplication.JobApplicationType")
               .Include("JobApplication.ApplicationStatus")
               .Include("JobWorkType")
               .Include("ContactResponsible")
               .Include("JobWorkPermitHistories")
               .Include("CreatedByEmployee")
               .Include("LastModifiedByEmployee")
               .FirstOrDefault(r => r.Id == id);

            if (jobApplicationWorkPermitResponse == null)
            {
                return this.NotFound();
            }

            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            return Ok(new JobApplicationWorkPermitDTO
            {
                Id = jobApplicationWorkPermitResponse.Id,
                IdJobApplication = jobApplicationWorkPermitResponse.IdJobApplication,
                IdJob = jobApplicationWorkPermitResponse.JobApplication.IdJob,
                JobApplicationNumber = jobApplicationWorkPermitResponse.JobApplication.ApplicationNumber,
                JobApplicationTypeName = jobApplicationWorkPermitResponse.JobApplication != null && jobApplicationWorkPermitResponse.JobApplication.JobApplicationType != null ? jobApplicationWorkPermitResponse.JobApplication.JobApplicationType.Description : string.Empty,
                IdJobApplicationType = jobApplicationWorkPermitResponse.JobApplication.IdJobApplicationType,
                JobApplicationFor = jobApplicationWorkPermitResponse.JobApplication.ApplicationFor,
                JobApplicationStatusId = jobApplicationWorkPermitResponse.JobApplication.IdApplicationStatus,
                JobApplicationStatus = jobApplicationWorkPermitResponse.JobApplication.ApplicationStatus != null ? jobApplicationWorkPermitResponse.JobApplication.ApplicationStatus.Name : string.Empty,
                JobApplicationFloor = jobApplicationWorkPermitResponse.JobApplication.FloorWorking,
                IdJobWorkType = jobApplicationWorkPermitResponse.IdJobWorkType,
                JobWorkTypeDescription = jobApplicationWorkPermitResponse.JobWorkType != null ? jobApplicationWorkPermitResponse.JobWorkType.Description : string.Empty,
                JobWorkTypeContent = jobApplicationWorkPermitResponse.JobWorkType != null ? jobApplicationWorkPermitResponse.JobWorkType.Content : string.Empty,
                //JobWorkTypeNumber = jobApplicationWorkPermitResponse.JobWorkType != null ? jobApplicationWorkPermitResponse.JobWorkType.Number : string.Empty,
                JobWorkTypeCode = jobApplicationWorkPermitResponse.JobWorkType != null ? jobApplicationWorkPermitResponse.JobWorkType.Code : string.Empty,
                Code = jobApplicationWorkPermitResponse.Code,
                EstimatedCost = jobApplicationWorkPermitResponse.EstimatedCost,
                Withdrawn = jobApplicationWorkPermitResponse.Withdrawn != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.Withdrawn), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.Withdrawn,
                Filed = jobApplicationWorkPermitResponse.Filed != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.Filed), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.Filed,
                Issued = jobApplicationWorkPermitResponse.Issued != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.Issued), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.Issued,
                Expires = jobApplicationWorkPermitResponse.Expires != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.Expires), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.Expires,
                SignedOff = jobApplicationWorkPermitResponse.SignedOff != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.SignedOff), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.SignedOff,
                //Withdrawn = jobApplicationWorkPermitResponse.Withdrawn != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.Withdrawn), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.Withdrawn,
                //Filed = jobApplicationWorkPermitResponse.Filed != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.Filed), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.Filed,
                //Issued = jobApplicationWorkPermitResponse.Issued != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.Issued), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.Issued,
                //Expires = jobApplicationWorkPermitResponse.Expires != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.Expires), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.Expires,
                //SignedOff = jobApplicationWorkPermitResponse.SignedOff != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.SignedOff), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.SignedOff,
                CompanyResponsible = jobApplicationWorkPermitResponse.ContactResponsible != null && jobApplicationWorkPermitResponse.ContactResponsible.Company != null ? jobApplicationWorkPermitResponse.ContactResponsible.Company.Name : string.Empty,
                PersonalResponsible = jobApplicationWorkPermitResponse.ContactResponsible != null && jobApplicationWorkPermitResponse.ContactResponsible.FirstName != null ? jobApplicationWorkPermitResponse.ContactResponsible.FirstName + (jobApplicationWorkPermitResponse.ContactResponsible.LastName != null ? jobApplicationWorkPermitResponse.ContactResponsible.LastName : string.Empty) : string.Empty,
                IdContactResponsible = jobApplicationWorkPermitResponse.IdContactResponsible,
                IsPersonResponsible = jobApplicationWorkPermitResponse.IsPersonResponsible,
                ContactResponsible = jobApplicationWorkPermitResponse.ContactResponsible,
                IdResponsibility = jobApplicationWorkPermitResponse.IdResponsibility,
                WorkDescription = jobApplicationWorkPermitResponse.WorkDescription,
                RenewalFee = jobApplicationWorkPermitResponse.RenewalFee,
                HasSiteSafetyCoordinator = jobApplicationWorkPermitResponse.HasSiteSafetyCoordinator,
                HasSiteSafetyManager = jobApplicationWorkPermitResponse.HasSiteSafetyManager,
                HasSuperintendentofconstruction = jobApplicationWorkPermitResponse.HasSuperintendentofconstruction,
                PreviousPermitNumber = jobApplicationWorkPermitResponse.PreviousPermitNumber,
                PermitNumber = jobApplicationWorkPermitResponse.PermitNumber,
                DocumentPath = jobApplicationWorkPermitResponse.DocumentPath != null ? APIUrl + "/" + Properties.Settings.Default.DOTWorkPermitDocument + "/" + jobApplicationWorkPermitResponse.Id + "_" + jobApplicationWorkPermitResponse.DocumentPath : string.Empty,
                PermitType = jobApplicationWorkPermitResponse.PermitType,
                ForPurposeOf = jobApplicationWorkPermitResponse.ForPurposeOf,
                EquipmentType = jobApplicationWorkPermitResponse.EquipmentType,
                PlumbingSignedOff = jobApplicationWorkPermitResponse.PlumbingSignedOff != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.PlumbingSignedOff), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.PlumbingSignedOff,
                FinalElevator = jobApplicationWorkPermitResponse.FinalElevator != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.FinalElevator), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.FinalElevator,
                TempElevator = jobApplicationWorkPermitResponse.TempElevator != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.TempElevator), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.TempElevator,
                ConstructionSignedOff = jobApplicationWorkPermitResponse.ConstructionSignedOff != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.ConstructionSignedOff), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.ConstructionSignedOff,
                //PlumbingSignedOff = jobApplicationWorkPermitResponse.PlumbingSignedOff != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.PlumbingSignedOff), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.PlumbingSignedOff,
                //FinalElevator = jobApplicationWorkPermitResponse.FinalElevator != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.FinalElevator), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.FinalElevator,
                //TempElevator = jobApplicationWorkPermitResponse.TempElevator != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.TempElevator), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.TempElevator,
                //ConstructionSignedOff = jobApplicationWorkPermitResponse.ConstructionSignedOff != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.ConstructionSignedOff), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.ConstructionSignedOff,
                Permittee = jobApplicationWorkPermitResponse.Permittee,
                IsPGL = jobApplicationWorkPermitResponse.IsPGL,
                CreatedBy = jobApplicationWorkPermitResponse.CreatedBy,
                CreatedByEmployeeName = jobApplicationWorkPermitResponse.CreatedByEmployee != null ? jobApplicationWorkPermitResponse.CreatedByEmployee.FirstName + " " + jobApplicationWorkPermitResponse.CreatedByEmployee.LastName : string.Empty,
                CreatedDate = jobApplicationWorkPermitResponse.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.CreatedDate,
                LastModifiedByEmployeeName = jobApplicationWorkPermitResponse.LastModifiedByEmployee != null ? jobApplicationWorkPermitResponse.LastModifiedByEmployee.FirstName + " " + jobApplicationWorkPermitResponse.LastModifiedByEmployee.LastName : string.Empty,
                LastModifiedDate = jobApplicationWorkPermitResponse.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.LastModifiedDate,
                JobWorkPermitHistories = jobApplicationWorkPermitResponse.JobWorkPermitHistories != null ? jobApplicationWorkPermitResponse.JobWorkPermitHistories.Select(x => new JobWorkPermitHistoryDTO
                {
                    Id = x.Id,
                    NewNumber = x.NewNumber,
                    OldNumber = x.OldNumber,
                    IdJobApplication = x.IdJobApplication,
                    IdWorkPermit = x.IdWorkPermit,
                    CreatedBy = x.CreatedBy,
                    CreatedByEmployeeName = x.CreatedByEmployee != null ? x.CreatedByEmployee.FirstName + " " + x.CreatedByEmployee.LastName : string.Empty,
                    CreatedDate = x.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : x.CreatedDate,
                }) : null
            });
        }

        /// <summary>
        /// Gets the job application work permits dropdown.
        /// </summary>
        /// <returns>Gets the job application work permits Responsible Dropdown List..</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/JobApplicationWorkPermits/ResponsibleDropdown")]
        public IHttpActionResult GetJobApplicationWorkPermitsDropdown()
        {
            List<PersonResponsible> personResponsibleList = new List<PersonResponsible>();
            personResponsibleList.Add(new PersonResponsible { Id = 1, ItemName = "RPO" });
            personResponsibleList.Add(new PersonResponsible { Id = 2, ItemName = "Other" });

            var result = personResponsibleList.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.ItemName
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the work permits dropdown.
        /// </summary>
        /// <param name="idJobApplication">The identifier job application.</param>
        /// <returns>Gets the job application work permits list for dropdown.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/JobApplicationWorkPermits/dropdown")]
        public IHttpActionResult GetWorkPermitsDropdown(int idJobApplication)
        {
            var result = rpoContext.JobApplicationWorkPermitTypes.Where(x => x.IdJobApplication == idJobApplication).AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.PermitType != null && c.PermitType != "" ? c.PermitType + "|" + c.PermitNumber : (c.JobWorkType != null ? c.JobWorkType.Description : string.Empty)
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// This method is used to update the Job Work permit details
        /// </summary>
        /// <param name="id">work permit Id</param>
        /// <param name="jobApplicationWorkPermitDTO">The job application work permit dto.</param>
        /// <returns>Update the detail of application permit type.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobApplicationWorkPermitType))]
        public IHttpActionResult PutJobApplicationWorkPermit(int id, JobApplicationWorkPermitDTO jobApplicationWorkPermitDTO)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddApplicationsWorkPermits))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != jobApplicationWorkPermitDTO.Id)
                {
                    return BadRequest();
                }

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                JobApplicationWorkPermitType jobApplicationWorkPermitType = rpoContext.JobApplicationWorkPermitTypes.Include("JobApplication.JobApplicationType")
                   .Include("JobApplication.ApplicationStatus")
                   .Include("JobApplication.Job")
                   .Include("JobWorkType")
                   .Include("ContactResponsible")
                   .FirstOrDefault(r => r.Id == id);

                if (jobApplicationWorkPermitType == null)
                {
                    return BadRequest();
                }


                if (jobApplicationWorkPermitType.PermitNumber != jobApplicationWorkPermitDTO.PermitNumber)
                {
                    Common.SaveJobWorkPermitHistory(jobApplicationWorkPermitType.Id, null, jobApplicationWorkPermitType.PermitNumber, jobApplicationWorkPermitDTO.PermitNumber, employee.Id);
                }

                jobApplicationWorkPermitType.IdJobApplication = jobApplicationWorkPermitDTO.IdJobApplication;
                jobApplicationWorkPermitType.Code = jobApplicationWorkPermitDTO.Code;
                jobApplicationWorkPermitType.EstimatedCost = jobApplicationWorkPermitDTO.EstimatedCost;
                jobApplicationWorkPermitType.Withdrawn = jobApplicationWorkPermitDTO.Withdrawn != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitDTO.Withdrawn), DateTimeKind.Utc) : jobApplicationWorkPermitDTO.Withdrawn;
                jobApplicationWorkPermitType.Filed = jobApplicationWorkPermitDTO.Filed != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitDTO.Filed), DateTimeKind.Utc) : jobApplicationWorkPermitDTO.Filed;
                jobApplicationWorkPermitType.Issued = jobApplicationWorkPermitDTO.Issued != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitDTO.Issued), DateTimeKind.Utc) : jobApplicationWorkPermitDTO.Issued;
                jobApplicationWorkPermitType.Expires = jobApplicationWorkPermitDTO.Expires != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitDTO.Expires), DateTimeKind.Utc) : jobApplicationWorkPermitDTO.Expires;
                jobApplicationWorkPermitType.SignedOff = jobApplicationWorkPermitDTO.SignedOff != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitDTO.SignedOff), DateTimeKind.Utc) : jobApplicationWorkPermitDTO.SignedOff;
                //jobApplicationWorkPermitType.Withdrawn = jobApplicationWorkPermitDTO.Withdrawn != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(jobApplicationWorkPermitDTO.Withdrawn), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitDTO.Withdrawn;
                //jobApplicationWorkPermitType.Filed = jobApplicationWorkPermitDTO.Filed != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(jobApplicationWorkPermitDTO.Filed), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitDTO.Filed;
                //jobApplicationWorkPermitType.Issued = jobApplicationWorkPermitDTO.Issued != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(jobApplicationWorkPermitDTO.Issued), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitDTO.Issued;
                //jobApplicationWorkPermitType.Expires = jobApplicationWorkPermitDTO.Expires != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(jobApplicationWorkPermitDTO.Expires), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitDTO.Expires;
                //jobApplicationWorkPermitType.SignedOff = jobApplicationWorkPermitDTO.SignedOff != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(jobApplicationWorkPermitDTO.SignedOff), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitDTO.SignedOff;
                jobApplicationWorkPermitType.IdJobWorkType = jobApplicationWorkPermitDTO.IdJobWorkType;
                jobApplicationWorkPermitType.WorkDescription = jobApplicationWorkPermitDTO.WorkDescription;
                jobApplicationWorkPermitType.IdContactResponsible = jobApplicationWorkPermitDTO.IdContactResponsible;
                jobApplicationWorkPermitType.IsPersonResponsible = jobApplicationWorkPermitDTO.IsPersonResponsible;
                jobApplicationWorkPermitType.IdResponsibility = jobApplicationWorkPermitDTO.IdResponsibility;
                jobApplicationWorkPermitType.RenewalFee = jobApplicationWorkPermitDTO.RenewalFee;
                jobApplicationWorkPermitType.HasSiteSafetyCoordinator = jobApplicationWorkPermitDTO.HasSiteSafetyCoordinator;
                jobApplicationWorkPermitType.HasSiteSafetyManager = jobApplicationWorkPermitDTO.HasSiteSafetyManager;
                jobApplicationWorkPermitType.HasSuperintendentofconstruction = jobApplicationWorkPermitDTO.HasSuperintendentofconstruction;
                jobApplicationWorkPermitType.PermitNumber = jobApplicationWorkPermitDTO.PermitNumber;
                jobApplicationWorkPermitType.PreviousPermitNumber = jobApplicationWorkPermitDTO.PreviousPermitNumber;
                jobApplicationWorkPermitType.LastModifiedDate = DateTime.UtcNow;
                jobApplicationWorkPermitType.JobApplication.Job.LastModiefiedDate = DateTime.UtcNow;
                jobApplicationWorkPermitType.PermitType = jobApplicationWorkPermitDTO.PermitType;
                jobApplicationWorkPermitType.ForPurposeOf = jobApplicationWorkPermitDTO.ForPurposeOf;
                jobApplicationWorkPermitType.EquipmentType = jobApplicationWorkPermitDTO.EquipmentType;
                jobApplicationWorkPermitType.Permittee = jobApplicationWorkPermitDTO.Permittee;
                //jobApplicationWorkPermitType.PlumbingSignedOff = jobApplicationWorkPermitDTO.PlumbingSignedOff != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(jobApplicationWorkPermitDTO.PlumbingSignedOff), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitDTO.PlumbingSignedOff;
                //jobApplicationWorkPermitType.FinalElevator = jobApplicationWorkPermitDTO.FinalElevator != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(jobApplicationWorkPermitDTO.FinalElevator), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitDTO.FinalElevator;
                //jobApplicationWorkPermitType.TempElevator = jobApplicationWorkPermitDTO.TempElevator != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(jobApplicationWorkPermitDTO.TempElevator), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitDTO.TempElevator;
                //jobApplicationWorkPermitType.ConstructionSignedOff = jobApplicationWorkPermitDTO.ConstructionSignedOff != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(jobApplicationWorkPermitDTO.ConstructionSignedOff), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitDTO.ConstructionSignedOff;
                jobApplicationWorkPermitType.PlumbingSignedOff = jobApplicationWorkPermitDTO.PlumbingSignedOff != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitDTO.PlumbingSignedOff), DateTimeKind.Utc) : jobApplicationWorkPermitDTO.PlumbingSignedOff;
                jobApplicationWorkPermitType.FinalElevator = jobApplicationWorkPermitDTO.FinalElevator != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitDTO.FinalElevator), DateTimeKind.Utc) : jobApplicationWorkPermitDTO.FinalElevator;
                jobApplicationWorkPermitType.TempElevator = jobApplicationWorkPermitDTO.TempElevator != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitDTO.TempElevator), DateTimeKind.Utc) : jobApplicationWorkPermitDTO.TempElevator;
                jobApplicationWorkPermitType.ConstructionSignedOff = jobApplicationWorkPermitDTO.ConstructionSignedOff != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitDTO.ConstructionSignedOff), DateTimeKind.Utc) : jobApplicationWorkPermitDTO.ConstructionSignedOff;
                jobApplicationWorkPermitType.IsPGL = jobApplicationWorkPermitDTO.IsPGL;

                if (employee != null)
                {
                    jobApplicationWorkPermitType.LastModifiedBy = employee.Id;
                    jobApplicationWorkPermitType.JobApplication.Job.LastModifiedBy = employee.Id;
                }

                //var jobApplication = db.JobApplications.FirstOrDefault(e => e.Id == jobApplicationWorkPermitType.IdJobApplication);
                string jobApplicationTypeName = string.Empty;
                if (jobApplicationWorkPermitType.JobApplication != null && jobApplicationWorkPermitType.JobApplication.JobApplicationType != null)
                {
                    jobApplicationTypeName = Convert.ToString(jobApplicationWorkPermitType.JobApplication.JobApplicationType.Description);
                }

                string jobWorkTypeName = string.Empty;
                if (jobApplicationWorkPermitType.JobWorkType != null)
                {
                    jobWorkTypeName = jobApplicationWorkPermitType.JobWorkType.Description;
                }

                int? jobApplicationTypeParent = 0;
                if (jobApplicationWorkPermitType.JobApplication != null && jobApplicationWorkPermitType.JobApplication.JobApplicationType != null)
                {
                    jobApplicationTypeName = Convert.ToString(jobApplicationWorkPermitType.JobApplication.JobApplicationType.Description);
                    jobApplicationTypeParent = jobApplicationWorkPermitType.JobApplication.JobApplicationType.IdParent;
                }

                switch ((ApplicationType)Convert.ToInt32(jobApplicationTypeParent))
                {
                    case ApplicationType.DOB:
                        string EditWorkPermit_DOB = JobHistoryMessages.EditWorkPermit_DOB
                                                    .Replace("##PermitType##", jobApplicationWorkPermitType.JobWorkType != null ? jobApplicationWorkPermitType.JobWorkType.Description : JobHistoryMessages.NoSetstring)
                                                    .Replace("##ApplicationType##", !string.IsNullOrEmpty(jobApplicationTypeName) ? jobApplicationTypeName : JobHistoryMessages.NoSetstring)
                                                    .Replace("##ApplicationNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.ApplicationNumber) ? jobApplicationWorkPermitType.JobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring)
                                                    .Replace("##EstimatedCost##", jobApplicationWorkPermitType.EstimatedCost != null && jobApplicationWorkPermitType.EstimatedCost != 0 ? jobApplicationWorkPermitType.EstimatedCost.ToString() : JobHistoryMessages.NoSetstring)
                                                    .Replace("##WorkDescription##", !string.IsNullOrEmpty(jobApplicationWorkPermitType.WorkDescription) ? jobApplicationWorkPermitType.WorkDescription : JobHistoryMessages.NoSetstring)
                                                    .Replace("##RPOORPersonName##", jobApplicationWorkPermitType.IdResponsibility == 2 ? ((from d in rpoContext.Contacts where d.Id == jobApplicationWorkPermitType.IdContactResponsible.Value select d.FirstName + " " + d.LastName).FirstOrDefault() != null ? (from d in rpoContext.Contacts where d.Id == jobApplicationWorkPermitType.IdContactResponsible.Value select d.FirstName + " " + d.LastName).FirstOrDefault() : JobHistoryMessages.NoSetstring) : (jobApplicationWorkPermitType.IdResponsibility == 1 ? "RPO" : JobHistoryMessages.NoSetstring))
                                                    .Replace("##FiledDate##", jobApplicationWorkPermitDTO.Filed != null ? jobApplicationWorkPermitDTO.Filed.Value.ToShortDateString() : JobHistoryMessages.NoSetstring)
                                                    .Replace("##IssuedDate##", jobApplicationWorkPermitDTO.Issued != null ? jobApplicationWorkPermitDTO.Issued.Value.ToShortDateString() : JobHistoryMessages.NoSetstring)
                                                    .Replace("##ExpiryDate##", jobApplicationWorkPermitDTO.Expires != null ? jobApplicationWorkPermitDTO.Expires.Value.ToShortDateString() : JobHistoryMessages.NoSetstring)
                                                    .Replace("##Permittee##", jobApplicationWorkPermitType.Permittee != null ? jobApplicationWorkPermitType.Permittee.ToString() : JobHistoryMessages.NoSetstring);

                        Common.SaveJobHistory(employee.Id, jobApplicationWorkPermitType.JobApplication.IdJob, EditWorkPermit_DOB, JobHistoryType.WorkPermits);
                        break;
                    case ApplicationType.DOT:

                        string EditApplication_DOT = JobHistoryMessages.EditWorkPermit_DOT
                                                   .Replace("##OldPermitNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermitType.PreviousPermitNumber) ? jobApplicationWorkPermitType.PreviousPermitNumber : JobHistoryMessages.NoSetstring)
                                                   .Replace("##PermitType##", !string.IsNullOrEmpty(jobApplicationWorkPermitType.PermitType) ? jobApplicationWorkPermitType.PermitType : JobHistoryMessages.NoSetstring)
                                                   .Replace("##NewPermitNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermitType.PermitNumber) ? jobApplicationWorkPermitType.PermitNumber : JobHistoryMessages.NoSetstring)
                                                   .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetWorkingOn) ? jobApplicationWorkPermitType.JobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetFrom) ? jobApplicationWorkPermitType.JobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetTo) ? jobApplicationWorkPermitType.JobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                   .Replace("##ApplicationNumber##", jobApplicationWorkPermitType.JobApplication != null && !string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.ApplicationNumber) ? jobApplicationWorkPermitType.JobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);
                        Common.SaveJobHistory(employee.Id, jobApplicationWorkPermitType.JobApplication.IdJob, EditApplication_DOT, JobHistoryType.Applications);

                        break;
                    case ApplicationType.DEP:
                        string editWorkPermit_DEP = JobHistoryMessages.EditWorkPermit_DEP
                                                   .Replace("##PermitNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermitType.PermitNumber) ? jobApplicationWorkPermitType.PermitNumber : JobHistoryMessages.NoSetstring)
                                                   .Replace("##PermitType##", jobApplicationWorkPermitType.JobWorkType != null ? jobApplicationWorkPermitType.JobWorkType.Description : JobHistoryMessages.NoSetstring)
                                                   .Replace("##ApplicationType##", jobApplicationTypeName != null ? jobApplicationTypeName : JobHistoryMessages.NoSetstring)
                                                   .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetWorkingOn) ? jobApplicationWorkPermitType.JobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetFrom) ? jobApplicationWorkPermitType.JobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetTo) ? jobApplicationWorkPermitType.JobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                   .Replace("##PermitIssued##", jobApplicationWorkPermitDTO.Issued != null ? jobApplicationWorkPermitDTO.Issued.Value.ToShortDateString() : JobHistoryMessages.NoSetstring)
                                                   .Replace("##PermitExpiry##", jobApplicationWorkPermitDTO.Expires != null ? jobApplicationWorkPermitDTO.Expires.Value.ToShortDateString() : JobHistoryMessages.NoSetstring);

                        Common.SaveJobHistory(employee.Id, jobApplicationWorkPermitType.JobApplication.IdJob, editWorkPermit_DEP, JobHistoryType.WorkPermits);
                        break;
                }


                //  Common.SaveJobHistory(employee.Id, jobApplicationWorkPermitType.JobApplication.IdJob, string.Format(JobHistoryMessages.EditWorkpermitMessage, jobWorkTypeName, jobApplicationTypeName), JobHistoryType.WorkPermits);

                try
                {
                    rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobJobApplicationWorkPermitExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                UpdateApplicationFor(jobApplicationWorkPermitType.IdJobApplication);

                return Ok(new JobApplicationWorkPermitDTO
                {
                    Id = jobApplicationWorkPermitType.Id,
                    IdJobApplication = jobApplicationWorkPermitType.IdJobApplication,
                    IdJob = jobApplicationWorkPermitType.JobApplication.IdJob,
                    JobApplicationNumber = jobApplicationWorkPermitType.JobApplication.ApplicationNumber,
                    JobApplicationFor = jobApplicationWorkPermitType.JobApplication.ApplicationFor,
                    JobApplicationStatusId = jobApplicationWorkPermitType.JobApplication.IdApplicationStatus,
                    JobApplicationStatus = jobApplicationWorkPermitType.JobApplication.ApplicationStatus != null ? jobApplicationWorkPermitType.JobApplication.ApplicationStatus.Name : string.Empty,
                    JobApplicationTypeName = jobApplicationWorkPermitType.JobApplication != null && jobApplicationWorkPermitType.JobApplication.JobApplicationType != null ? jobApplicationWorkPermitType.JobApplication.JobApplicationType.Description : string.Empty,
                    IdJobApplicationType = jobApplicationWorkPermitType.JobApplication.IdJobApplicationType,
                    JobApplicationFloor = jobApplicationWorkPermitType.JobApplication.FloorWorking,
                    IdJobWorkType = jobApplicationWorkPermitType.IdJobWorkType,
                    JobWorkTypeDescription = jobApplicationWorkPermitType.JobWorkType != null ? jobApplicationWorkPermitType.JobWorkType.Description : string.Empty,
                    JobWorkTypeContent = jobApplicationWorkPermitType.JobWorkType != null ? jobApplicationWorkPermitType.JobWorkType.Content : string.Empty,
                    //JobWorkTypeNumber = jobApplicationWorkPermitType.JobWorkType != null ? jobApplicationWorkPermitType.JobWorkType.Number : string.Empty,
                    JobWorkTypeCode = jobApplicationWorkPermitType.JobWorkType != null ? jobApplicationWorkPermitType.JobWorkType.Code : string.Empty,
                    Code = jobApplicationWorkPermitType.Code,
                    EstimatedCost = jobApplicationWorkPermitType.EstimatedCost,
                    Withdrawn = jobApplicationWorkPermitType.Withdrawn != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitType.Withdrawn), DateTimeKind.Utc) : jobApplicationWorkPermitType.Withdrawn,
                    Filed = jobApplicationWorkPermitType.Filed != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitType.Filed), DateTimeKind.Utc) : jobApplicationWorkPermitType.Filed,
                    Issued = jobApplicationWorkPermitType.Issued != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitType.Issued), DateTimeKind.Utc) : jobApplicationWorkPermitType.Issued,
                    Expires = jobApplicationWorkPermitType.Expires != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitType.Expires), DateTimeKind.Utc) : jobApplicationWorkPermitType.Expires,
                    SignedOff = jobApplicationWorkPermitType.SignedOff != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitType.SignedOff), DateTimeKind.Utc) : jobApplicationWorkPermitType.SignedOff,
                    //Withdrawn = jobApplicationWorkPermitType.Withdrawn != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitType.Withdrawn), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitType.Withdrawn,
                    //Filed = jobApplicationWorkPermitType.Filed != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitType.Filed), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitType.Filed,
                    //Issued = jobApplicationWorkPermitType.Issued != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitType.Issued), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitType.Issued,
                    //Expires = jobApplicationWorkPermitType.Expires != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitType.Expires), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitType.Expires,
                    //SignedOff = jobApplicationWorkPermitType.SignedOff != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitType.SignedOff), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitType.SignedOff,
                    CompanyResponsible = jobApplicationWorkPermitType.ContactResponsible != null && jobApplicationWorkPermitType.ContactResponsible.Company != null ? jobApplicationWorkPermitType.ContactResponsible.Company.Name : string.Empty,
                    PersonalResponsible = jobApplicationWorkPermitType.ContactResponsible != null && jobApplicationWorkPermitType.ContactResponsible.FirstName != null ? jobApplicationWorkPermitType.ContactResponsible.FirstName + (jobApplicationWorkPermitType.ContactResponsible.LastName != null ? jobApplicationWorkPermitType.ContactResponsible.LastName : string.Empty) : string.Empty,
                    IdContactResponsible = jobApplicationWorkPermitType.IdContactResponsible,
                    IsPersonResponsible = jobApplicationWorkPermitType.IsPersonResponsible,
                    ContactResponsible = jobApplicationWorkPermitType.ContactResponsible,
                    WorkDescription = jobApplicationWorkPermitType.WorkDescription,
                    RenewalFee = jobApplicationWorkPermitType.RenewalFee,
                    HasSiteSafetyCoordinator = jobApplicationWorkPermitType.HasSiteSafetyCoordinator,
                    HasSiteSafetyManager = jobApplicationWorkPermitType.HasSiteSafetyManager,
                    HasSuperintendentofconstruction = jobApplicationWorkPermitType.HasSuperintendentofconstruction,
                    PreviousPermitNumber = jobApplicationWorkPermitType.PreviousPermitNumber,
                    PermitNumber = jobApplicationWorkPermitType.PermitNumber,
                    PermitType = jobApplicationWorkPermitType.PermitType,
                    ForPurposeOf = jobApplicationWorkPermitType.ForPurposeOf,
                    EquipmentType = jobApplicationWorkPermitType.EquipmentType,
                    PlumbingSignedOff = jobApplicationWorkPermitType.PlumbingSignedOff != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitType.PlumbingSignedOff), DateTimeKind.Utc) : jobApplicationWorkPermitType.PlumbingSignedOff,
                    FinalElevator = jobApplicationWorkPermitType.FinalElevator != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitType.FinalElevator), DateTimeKind.Utc) : jobApplicationWorkPermitType.FinalElevator,
                    TempElevator = jobApplicationWorkPermitType.TempElevator != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitType.TempElevator), DateTimeKind.Utc) : jobApplicationWorkPermitType.TempElevator,
                    ConstructionSignedOff = jobApplicationWorkPermitType.ConstructionSignedOff != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitType.ConstructionSignedOff), DateTimeKind.Utc) : jobApplicationWorkPermitType.ConstructionSignedOff,
                    //PlumbingSignedOff = jobApplicationWorkPermitType.PlumbingSignedOff != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitType.PlumbingSignedOff), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitType.PlumbingSignedOff,
                    //FinalElevator = jobApplicationWorkPermitType.FinalElevator != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitType.FinalElevator), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitType.FinalElevator,
                    //TempElevator = jobApplicationWorkPermitType.TempElevator != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitType.TempElevator), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitType.TempElevator,
                    //ConstructionSignedOff = jobApplicationWorkPermitType.ConstructionSignedOff != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitType.ConstructionSignedOff), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitType.ConstructionSignedOff,
                    Permittee = jobApplicationWorkPermitType.Permittee,
                    IsPGL = jobApplicationWorkPermitType.IsPGL,

                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the job application work permit.
        /// </summary>
        /// <param name="jobApplicationWorkPermit">The job application work permit.</param>
        /// <returns>create a new job application permit.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobApplicationWorkPermitType))]
        public IHttpActionResult PostJobApplicationWorkPermit(JobApplicationWorkPermitType jobApplicationWorkPermit)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddApplicationsWorkPermits))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                jobApplicationWorkPermit.LastModifiedDate = DateTime.UtcNow;
                jobApplicationWorkPermit.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobApplicationWorkPermit.CreatedBy = employee.Id;
                }

                rpoContext.JobApplicationWorkPermitTypes.Add(jobApplicationWorkPermit);

                rpoContext.SaveChanges();

                var jobApplication = rpoContext.JobApplications.Include("JobApplicationType").Include("Job").FirstOrDefault(e => e.Id == jobApplicationWorkPermit.IdJobApplication);
                string jobApplicationTypeName = string.Empty;
                int? jobApplicationTypeParent = 0;
                if (jobApplication != null && jobApplication.JobApplicationType != null)
                {
                    jobApplicationTypeName = Convert.ToString(jobApplication.JobApplicationType.Description);
                    jobApplicationTypeParent = jobApplication.JobApplicationType.IdParent;
                }

                string jobWorkTypeName = string.Empty;
                if (jobApplicationWorkPermit.JobWorkType != null)
                {
                    jobWorkTypeName = jobApplicationWorkPermit.JobWorkType.Description;
                }

                jobApplication.Job.LastModiefiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobApplication.Job.LastModifiedBy = employee.Id;
                }

                rpoContext.SaveChanges();



                UpdateApplicationFor(jobApplicationWorkPermit.IdJobApplication);

                //return this.CreatedAtRoute("DefaultApi", new { id = jobApplicationWorkPermit.Id }, jobApplicationWorkPermit);

                JobApplicationWorkPermitType jobApplicationWorkPermitResponse = rpoContext.JobApplicationWorkPermitTypes.Include("JobApplication.JobApplicationType")
                   .Include("JobApplication.ApplicationStatus")
                  .Include("JobWorkType")
                  .Include("ContactResponsible")
                  .FirstOrDefault(r => r.Id == jobApplicationWorkPermit.Id);

                switch ((ApplicationType)Convert.ToInt32(jobApplicationTypeParent))
                {
                    case ApplicationType.DOB:

                        string addWorkPermit_DOB = JobHistoryMessages.AddWorkPermit_DOB
                                                   .Replace("##PermitType##", jobApplicationWorkPermitResponse.JobWorkType != null ? jobApplicationWorkPermitResponse.JobWorkType.Description : JobHistoryMessages.NoSetstring)
                                                   .Replace("##ApplicationType##", jobApplicationTypeName != null ? jobApplicationTypeName : JobHistoryMessages.NoSetstring)
                                                   .Replace("##ApplicationNumber##", !string.IsNullOrEmpty(jobApplication.ApplicationNumber) ? jobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring)
                                                   .Replace("##EstimatedCost##", jobApplicationWorkPermitResponse.EstimatedCost != null && jobApplicationWorkPermitResponse.EstimatedCost != 0 ? jobApplicationWorkPermitResponse.EstimatedCost.ToString() : JobHistoryMessages.NoSetstring)
                                                   .Replace("##WorkDescription##", !string.IsNullOrEmpty(jobApplicationWorkPermit.WorkDescription) ? jobApplicationWorkPermit.WorkDescription : JobHistoryMessages.NoSetstring)
                                                   .Replace("##RPOORPersonName##", jobApplicationWorkPermit.IdResponsibility == 2 ? ((from d in rpoContext.Contacts where d.Id == jobApplicationWorkPermit.IdContactResponsible.Value select d.FirstName + " " + d.LastName).FirstOrDefault() != null ? (from d in rpoContext.Contacts where d.Id == jobApplicationWorkPermit.IdContactResponsible.Value select d.FirstName + " " + d.LastName).FirstOrDefault() : JobHistoryMessages.NoSetstring) : (jobApplicationWorkPermit.IdResponsibility == 1 ? "RPO" : JobHistoryMessages.NoSetstring))
                                                   .Replace("##FiledDate##", jobApplicationWorkPermitResponse.Filed != null ? jobApplicationWorkPermitResponse.Filed.Value.ToShortDateString() : JobHistoryMessages.NoSetstring)
                                                   .Replace("##IssuedDate##", jobApplicationWorkPermitResponse.Issued != null ? jobApplicationWorkPermitResponse.Issued.Value.ToShortDateString() : JobHistoryMessages.NoSetstring)
                                                   .Replace("##ExpiryDate##", jobApplicationWorkPermitResponse.Expires != null ? jobApplicationWorkPermitResponse.Expires.Value.ToShortDateString() : JobHistoryMessages.NoSetstring)
                                                   .Replace("##Permittee##", jobApplicationWorkPermitResponse.Permittee != null ? jobApplicationWorkPermitResponse.Permittee.ToString() : JobHistoryMessages.NoSetstring);

                        Common.SaveJobHistory(employee.Id, jobApplication.IdJob, addWorkPermit_DOB, JobHistoryType.WorkPermits);
                        break;
                    case ApplicationType.DOT:

                        string addWorkPermit_DOT = JobHistoryMessages.AddWorkPermit_DOT
                                                   .Replace("##PermitNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermit.PermitNumber) ? jobApplicationWorkPermit.PermitNumber : JobHistoryMessages.NoSetstring)
                                                   .Replace("##PermitType##", !string.IsNullOrEmpty(jobApplicationWorkPermit.PermitType) ? jobApplicationWorkPermit.PermitType : JobHistoryMessages.NoSetstring)
                                                   .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplicationWorkPermit.JobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplicationWorkPermit.JobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplicationWorkPermit.JobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplicationWorkPermit.JobApplication.StreetWorkingOn) ? jobApplicationWorkPermit.JobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplicationWorkPermit.JobApplication.StreetFrom) ? jobApplicationWorkPermit.JobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplicationWorkPermit.JobApplication.StreetTo) ? jobApplicationWorkPermit.JobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                   .Replace("##ApplicationNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermit.JobApplication.ApplicationNumber) ? jobApplicationWorkPermit.JobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);
                        Common.SaveJobHistory(employee.Id, jobApplication.IdJob, addWorkPermit_DOT, JobHistoryType.WorkPermits);
                        break;
                    case ApplicationType.DEP:
                        string addWorkPermit_DEP = JobHistoryMessages.AddWorkPermit_DEP
                                                   .Replace("##PermitNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermitResponse.PermitNumber) ? jobApplicationWorkPermitResponse.PermitNumber : JobHistoryMessages.NoSetstring)
                                                   .Replace("##PermitType##", jobApplicationWorkPermitResponse.JobWorkType != null ? jobApplicationWorkPermitResponse.JobWorkType.Description : JobHistoryMessages.NoSetstring)
                                                   .Replace("##ApplicationType##", jobApplicationTypeName != null ? jobApplicationTypeName : JobHistoryMessages.NoSetstring)
                                                   .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplicationWorkPermit.JobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplicationWorkPermit.JobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplicationWorkPermit.JobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplicationWorkPermit.JobApplication.StreetWorkingOn) ? jobApplicationWorkPermit.JobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplicationWorkPermit.JobApplication.StreetFrom) ? jobApplicationWorkPermit.JobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplicationWorkPermit.JobApplication.StreetTo) ? jobApplicationWorkPermit.JobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                   .Replace("##PermitIssued##", jobApplicationWorkPermitResponse.Issued != null ? jobApplicationWorkPermitResponse.Issued.Value.ToShortDateString() : JobHistoryMessages.NoSetstring)
                                                   .Replace("##PermitExpiry##", jobApplicationWorkPermitResponse.Expires != null ? jobApplicationWorkPermitResponse.Expires.Value.ToShortDateString() : JobHistoryMessages.NoSetstring);

                        Common.SaveJobHistory(employee.Id, jobApplication.IdJob, addWorkPermit_DEP, JobHistoryType.WorkPermits);
                        break;
                }

                string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                return Ok(new JobApplicationWorkPermitDTO
                {
                    Id = jobApplicationWorkPermitResponse.Id,
                    IdJobApplication = jobApplicationWorkPermitResponse.IdJobApplication,
                    IdJob = jobApplicationWorkPermitResponse.JobApplication.IdJob,
                    JobApplicationNumber = jobApplicationWorkPermitResponse.JobApplication.ApplicationNumber,
                    JobApplicationTypeName = jobApplicationWorkPermitResponse.JobApplication != null && jobApplicationWorkPermitResponse.JobApplication.JobApplicationType != null ? jobApplicationWorkPermitResponse.JobApplication.JobApplicationType.Description : string.Empty,
                    IdJobApplicationType = jobApplicationWorkPermitResponse.JobApplication.IdJobApplicationType,
                    JobApplicationFor = jobApplicationWorkPermitResponse.JobApplication.ApplicationFor,
                    JobApplicationStatusId = jobApplicationWorkPermitResponse.JobApplication.IdApplicationStatus,
                    JobApplicationStatus = jobApplicationWorkPermitResponse.JobApplication.ApplicationStatus != null ? jobApplicationWorkPermitResponse.JobApplication.ApplicationStatus.Name : string.Empty,
                    JobApplicationFloor = jobApplicationWorkPermitResponse.JobApplication.FloorWorking,
                    IdJobWorkType = jobApplicationWorkPermitResponse.IdJobWorkType,
                    JobWorkTypeDescription = jobApplicationWorkPermitResponse.JobWorkType != null ? jobApplicationWorkPermitResponse.JobWorkType.Description : string.Empty,
                    JobWorkTypeContent = jobApplicationWorkPermitResponse.JobWorkType != null ? jobApplicationWorkPermitResponse.JobWorkType.Content : string.Empty,
                    //JobWorkTypeNumber = jobApplicationWorkPermitResponse.JobWorkType != null ? jobApplicationWorkPermitResponse.JobWorkType.Number : string.Empty,
                    JobWorkTypeCode = jobApplicationWorkPermitResponse.JobWorkType != null ? jobApplicationWorkPermitResponse.JobWorkType.Code : string.Empty,
                    Code = jobApplicationWorkPermitResponse.Code,
                    EstimatedCost = jobApplicationWorkPermitResponse.EstimatedCost,
                    Withdrawn = jobApplicationWorkPermitResponse.Withdrawn != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.Withdrawn), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.Withdrawn,
                    Filed = jobApplicationWorkPermitResponse.Filed != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.Filed), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.Filed,
                    Issued = jobApplicationWorkPermitResponse.Issued != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.Issued), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.Issued,
                    Expires = jobApplicationWorkPermitResponse.Expires != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.Expires), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.Expires,
                    SignedOff = jobApplicationWorkPermitResponse.SignedOff != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.SignedOff), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.SignedOff,
                    //Withdrawn = jobApplicationWorkPermitResponse.Withdrawn != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.Withdrawn), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.Withdrawn,
                    //Filed = jobApplicationWorkPermitResponse.Filed != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.Filed), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.Filed,
                    //Issued = jobApplicationWorkPermitResponse.Issued != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.Issued), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.Issued,
                    //Expires = jobApplicationWorkPermitResponse.Expires != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.Expires), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.Expires,
                    //SignedOff = jobApplicationWorkPermitResponse.SignedOff != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.SignedOff), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.SignedOff,
                    CompanyResponsible = jobApplicationWorkPermitResponse.ContactResponsible != null && jobApplicationWorkPermitResponse.ContactResponsible.Company != null ? jobApplicationWorkPermitResponse.ContactResponsible.Company.Name : string.Empty,
                    PersonalResponsible = jobApplicationWorkPermitResponse.ContactResponsible != null && jobApplicationWorkPermitResponse.ContactResponsible.FirstName != null ? jobApplicationWorkPermitResponse.ContactResponsible.FirstName + (jobApplicationWorkPermitResponse.ContactResponsible.LastName != null ? jobApplicationWorkPermitResponse.ContactResponsible.LastName : string.Empty) : string.Empty,
                    IdContactResponsible = jobApplicationWorkPermitResponse.IdContactResponsible,
                    IsPersonResponsible = jobApplicationWorkPermitResponse.IsPersonResponsible,
                    ContactResponsible = jobApplicationWorkPermitResponse.ContactResponsible,
                    IdResponsibility = jobApplicationWorkPermitResponse.IdResponsibility,
                    WorkDescription = jobApplicationWorkPermitResponse.WorkDescription,
                    RenewalFee = jobApplicationWorkPermitResponse.RenewalFee,
                    HasSiteSafetyCoordinator = jobApplicationWorkPermitResponse.HasSiteSafetyCoordinator,
                    HasSiteSafetyManager = jobApplicationWorkPermitResponse.HasSiteSafetyManager,
                    HasSuperintendentofconstruction = jobApplicationWorkPermitResponse.HasSuperintendentofconstruction,
                    PreviousPermitNumber = jobApplicationWorkPermitResponse.PreviousPermitNumber,
                    PermitNumber = jobApplicationWorkPermitResponse.PermitNumber,
                    PermitType = jobApplicationWorkPermitResponse.PermitType,
                    ForPurposeOf = jobApplicationWorkPermitResponse.ForPurposeOf,
                    EquipmentType = jobApplicationWorkPermitResponse.EquipmentType,
                    PlumbingSignedOff = jobApplicationWorkPermitResponse.PlumbingSignedOff != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.PlumbingSignedOff), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.PlumbingSignedOff,
                    FinalElevator = jobApplicationWorkPermitResponse.FinalElevator != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.FinalElevator), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.FinalElevator,
                    TempElevator = jobApplicationWorkPermitResponse.TempElevator != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.TempElevator), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.TempElevator,
                    ConstructionSignedOff = jobApplicationWorkPermitResponse.ConstructionSignedOff != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.ConstructionSignedOff), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.ConstructionSignedOff,
                    //PlumbingSignedOff = jobApplicationWorkPermitResponse.PlumbingSignedOff != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.PlumbingSignedOff), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.PlumbingSignedOff,
                    //FinalElevator = jobApplicationWorkPermitResponse.FinalElevator != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.FinalElevator), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.FinalElevator,
                    //TempElevator = jobApplicationWorkPermitResponse.TempElevator != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.TempElevator), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.TempElevator,
                    //ConstructionSignedOff = jobApplicationWorkPermitResponse.ConstructionSignedOff != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.ConstructionSignedOff), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.ConstructionSignedOff,
                    Permittee = jobApplicationWorkPermitResponse.Permittee,
                    IsPGL = jobApplicationWorkPermitResponse.IsPGL,
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        ///Upload the multiple permit with location/application  Posts the job application work permits.
        /// </summary>
        /// <param name="jobApplicationWorkPermit">The job application work permit.</param>
        /// <returns>Upload the multiple permit with location/application  Posts the job application work permits.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobApplicationWorkPermitType))]
        [HttpPost]
        [Route("api/JobApplicationWorkPermits/multiple")]
        public IHttpActionResult PostJobApplicationWorkPermits(List<JobApplicationWorkPermitTypeCreateUpdate> jobApplicationWorkPermitList)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            if (jobApplicationWorkPermitList != null && jobApplicationWorkPermitList.Count > 0)
            {
                var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
                if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddApplicationsWorkPermits))
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    foreach (JobApplicationWorkPermitTypeCreateUpdate jobApplicationWorkPermit in jobApplicationWorkPermitList)
                    {
                        JobApplication jobApplication = rpoContext.JobApplications.Include("Job").FirstOrDefault(e => e.ApplicationNumber == jobApplicationWorkPermit.TrackingNumber && e.StreetWorkingOn.ToLower().Contains(jobApplicationWorkPermit.StreetWorkingOn.ToLower()) && e.StreetFrom.ToLower().Contains(jobApplicationWorkPermit.StreetWorkingFrom.ToLower()) && e.StreetTo.ToLower().Contains(jobApplicationWorkPermit.StreetWorkingTo.ToLower()) && e.WorkPermitTypes.FirstOrDefault(p => p.PermitNumber == jobApplicationWorkPermit.PermitNumber) != null);
                        if (jobApplication != null && jobApplication.IdJob != jobApplicationWorkPermit.IdJob)
                        {
                            throw new RpoBusinessException(string.Format(StaticMessages.TrackingNumberAleadyExists, (jobApplication.Job != null ? jobApplication.Job.JobNumber : string.Empty)));
                        }
                    }

                    //foreach (JobApplicationWorkPermitTypeCreateUpdate jobApplicationWorkPermit in jobApplicationWorkPermitList)
                    //{
                    //    JobApplication jobApplication = rpoContext.JobApplications.Include("Job").FirstOrDefault(e => e.StreetFrom == jobApplicationWorkPermit.StreetWorkingFrom &&
                    //    e.StreetTo == jobApplicationWorkPermit.StreetWorkingTo && e.StreetWorkingOn == jobApplicationWorkPermit.StreetWorkingOn
                    //    && e.IdJob == jobApplicationWorkPermit.IdJob );
                    //    if (jobApplication != null)
                    //    {
                    //        throw new RpoBusinessException(StaticMessages.TrackingNumberAlreadyInOtherJobExistsMessage);
                    //    }
                    //}

                    string workPermitIds = string.Empty;
                    int idApplicationType = ApplicationType.DOT.GetHashCode();
                    JobApplicationType jobApplicationType = rpoContext.JobApplicationTypes.FirstOrDefault(x => x.IdParent == idApplicationType);

                    foreach (JobApplicationWorkPermitTypeCreateUpdate jobApplicationWorkPermit in jobApplicationWorkPermitList)
                    {
                        JobApplication jobApplication = rpoContext.JobApplications.Include("Job").FirstOrDefault
                            (e => e.StreetFrom == jobApplicationWorkPermit.StreetWorkingFrom &&
                                  e.StreetTo == jobApplicationWorkPermit.StreetWorkingTo &&
                                  e.StreetWorkingOn == jobApplicationWorkPermit.StreetWorkingOn && e.IdJob == jobApplicationWorkPermit.IdJob
                                  && e.IdJobApplicationType == jobApplicationType.Id);
                        if (jobApplication == null)
                        {
                            jobApplication = new JobApplication();
                            jobApplication.IdJob = jobApplicationWorkPermit.IdJob;
                            jobApplication.ApplicationNumber = jobApplicationWorkPermit.TrackingNumber;
                            jobApplication.IdApplicationStatus = DOTApplicationStatus.New.GetHashCode();
                            jobApplication.StreetFrom = jobApplicationWorkPermit.StreetWorkingFrom;
                            jobApplication.StreetTo = jobApplicationWorkPermit.StreetWorkingTo;
                            jobApplication.StreetWorkingOn = jobApplicationWorkPermit.StreetWorkingOn;
                            if (jobApplicationType != null)
                            {
                                jobApplication.IdJobApplicationType = jobApplicationType.Id;
                            }

                            rpoContext.JobApplications.Add(jobApplication);
                            rpoContext.SaveChanges();

                            jobApplication = rpoContext.JobApplications.Include("Job").FirstOrDefault(e => e.Id == jobApplication.Id);
                            ApplicationStatus applicationStatus = rpoContext.ApplicationStatus.FirstOrDefault(x => x.Id == jobApplication.IdApplicationStatus);

                            string addApplication_DOT = JobHistoryMessages.AddApplication_DOT
                                                       .Replace("##ApplicationType##", jobApplicationType != null ? jobApplicationType.Description : JobHistoryMessages.NoSetstring)
                                                       .Replace("##ApplicationStatus##", applicationStatus != null && !string.IsNullOrEmpty(applicationStatus.Name) ? applicationStatus.Name : JobHistoryMessages.NoSetstring)
                                                       .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) ? jobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetFrom) ? jobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetTo) ? jobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                       .Replace("##Tracking##", !string.IsNullOrEmpty(jobApplication.ApplicationNumber) ? jobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);
                            Common.SaveJobHistory(employee.Id, jobApplication.IdJob, addApplication_DOT, JobHistoryType.Applications);

                        }
                        else
                        {
                            jobApplication.IdJob = jobApplicationWorkPermit.IdJob;
                            if (jobApplication.ApplicationNumber != jobApplicationWorkPermit.TrackingNumber)
                            {
                                Common.SaveJobWorkPermitHistory(null, jobApplication.Id, jobApplication.ApplicationNumber, jobApplicationWorkPermit.TrackingNumber, employee.Id);
                            }
                            jobApplication.ApplicationNumber = jobApplicationWorkPermit.TrackingNumber;
                            jobApplication.StreetFrom = jobApplicationWorkPermit.StreetWorkingFrom;
                            jobApplication.StreetTo = jobApplicationWorkPermit.StreetWorkingTo;
                            jobApplication.StreetWorkingOn = jobApplicationWorkPermit.StreetWorkingOn;
                            if (jobApplication.IdJobApplicationType == null && jobApplicationType != null)
                            {
                                jobApplication.IdJobApplicationType = jobApplicationType.Id;
                            }

                            rpoContext.SaveChanges();

                            ApplicationStatus applicationStatus = rpoContext.ApplicationStatus.FirstOrDefault(x => x.Id == jobApplication.IdApplicationStatus);

                            string editApplicationLocation_DOT = JobHistoryMessages.EditApplicationLocation_DOT
                                                     .Replace("##ApplicationType##", jobApplicationType != null ? jobApplicationType.Description : JobHistoryMessages.NoSetstring)
                                                     .Replace("##ApplicationStatus##", applicationStatus != null && !string.IsNullOrEmpty(applicationStatus.Name) ? applicationStatus.Name : JobHistoryMessages.NoSetstring)
                                                      .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) ? jobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetFrom) ? jobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetTo) ? jobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                     .Replace("##Tracking##", !string.IsNullOrEmpty(jobApplication.ApplicationNumber) ? jobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);


                            Common.SaveJobHistory(employee.Id, jobApplication.IdJob, editApplicationLocation_DOT, JobHistoryType.Applications);
                        }

                        string jobApplicationTypeName = string.Empty;
                        if (jobApplication != null && jobApplication.JobApplicationType != null)
                        {
                            jobApplicationTypeName = Convert.ToString(jobApplication.JobApplicationType.Description);
                        }


                        JobApplicationWorkPermitType jobApplicationWorkPermitType = rpoContext.JobApplicationWorkPermitTypes
                            .FirstOrDefault(x => (x.IsCompleted == null || x.IsCompleted == false) && x.PermitNumber == jobApplicationWorkPermit.PermitNumber);
                        //var jobApplicationWorkTypeExists = rpoContext.JobApplicationWorkPermitTypes.Where(w => w.PermitNumber == jobApplicationWorkPermit.PreviousPermitNumber
                        //   && w.JobApplication.IdJob == jobApplicationWorkPermit.IdJob).FirstOrDefault();

                        if (jobApplicationWorkPermitType == null)
                        {
                            jobApplicationWorkPermitType = rpoContext.JobApplicationWorkPermitTypes.FirstOrDefault(x => (x.IsCompleted == null || x.IsCompleted == false) 
                            && x.PermitNumber == jobApplicationWorkPermit.PreviousPermitNumber);
                                                     
                             var jobApplicationWorkTypeExists = rpoContext.JobApplicationWorkPermitTypes.Where(w => w.PermitNumber == jobApplicationWorkPermit.PreviousPermitNumber 
                            && w.JobApplication.IdJob == jobApplicationWorkPermit.IdJob).FirstOrDefault();

                            //var jobApplications = rpoContext.JobApplications.Include("Job").Include("JobWorkPermitHistories").Include("JobApplicationType").
                            //    Where(e => (e.ApplicationNumber == trackingNumber || e.JobWorkPermitHistories.Any(p => p.NewNumber == trackingNumber || p.OldNumber == trackingNumber))
                            //    && e.StreetWorkingOn.ToLower().Contains(streetWorkingOn.ToLower()) && e.StreetFrom.ToLower().Contains(streetWorkingFrom.ToLower()) 
                            //    && e.StreetTo.ToLower().Contains(streetWorkingTo.ToLower()) && e.WorkPermitTypes.FirstOrDefault(p => p.PermitNumber == Permitnumber) != null).ToList();

                            if (jobApplicationWorkPermitType == null)
                            {
                                jobApplicationWorkPermitType = new JobApplicationWorkPermitType();

                                //JobWorkType jobWorkType = rpoContext.JobWorkTypes.FirstOrDefault(x => x.Code == jobApplicationWorkPermit.WorkTypeCode && x.JobApplicationType_Id == jobApplication.IdJobApplicationType);
                                jobApplicationWorkPermitType.LastModifiedDate = DateTime.UtcNow;
                                jobApplicationWorkPermitType.CreatedDate = DateTime.UtcNow;
                                if (employee != null)
                                {
                                    jobApplicationWorkPermitType.CreatedBy = employee.Id;
                                }

                                jobApplicationWorkPermitType.IdJobApplication = jobApplication.Id;
                                jobApplicationWorkPermitType.EstimatedCost = jobApplicationWorkPermit.EstimatedCost;
                                jobApplicationWorkPermitType.PermitType = jobApplicationWorkPermit.PermitType;
                                jobApplicationWorkPermitType.ForPurposeOf = jobApplicationWorkPermit.ForPurposeOf;
                                jobApplicationWorkPermitType.EquipmentType = jobApplicationWorkPermit.EquipmentType;
                                jobApplicationWorkPermitType.Code = jobApplicationWorkPermit.Code;

                                jobApplicationWorkPermitType.Withdrawn = jobApplicationWorkPermit.Withdrawn;
                                jobApplicationWorkPermitType.Filed = jobApplicationWorkPermit.Filed;
                                jobApplicationWorkPermitType.Issued = jobApplicationWorkPermit.Issued;
                                jobApplicationWorkPermitType.Expires = jobApplicationWorkPermit.Expires;
                                jobApplicationWorkPermitType.SignedOff = jobApplicationWorkPermit.SignedOff;
                                jobApplicationWorkPermitType.WorkDescription = jobApplicationWorkPermit.WorkDescription;
                                jobApplicationWorkPermitType.IsPersonResponsible = false;
                                jobApplicationWorkPermitType.PermitNumber = jobApplicationWorkPermit.PermitNumber;
                                jobApplicationWorkPermitType.PreviousPermitNumber = jobApplicationWorkPermit.PreviousPermitNumber;
                                jobApplicationWorkPermitType.RenewalFee = jobApplicationWorkPermit.RenewalFee;
                                jobApplicationWorkPermitType.HasSiteSafetyCoordinator= jobApplicationWorkPermit.HasSiteSafetyCoordinator;
                                jobApplicationWorkPermitType.HasSiteSafetyManager = jobApplicationWorkPermit.HasSiteSafetyManager;
                                jobApplicationWorkPermitType.HasSuperintendentofconstruction = jobApplicationWorkPermit.HasSuperintendentofconstruction;

                                rpoContext.JobApplicationWorkPermitTypes.Add(jobApplicationWorkPermitType);
                                rpoContext.SaveChanges();

                                if (string.IsNullOrEmpty(workPermitIds))
                                {
                                    workPermitIds = workPermitIds + jobApplicationWorkPermitType.Id;
                                }
                                else
                                {
                                    workPermitIds = workPermitIds + "," + jobApplicationWorkPermitType.Id;
                                }

                                string jobWorkTypeName = string.Empty;
                                if (jobApplicationWorkPermitType.JobWorkType != null)
                                {
                                    jobWorkTypeName = jobApplicationWorkPermitType.JobWorkType.Description;
                                }

                                jobApplication.Job.LastModiefiedDate = DateTime.UtcNow;
                                if (employee != null)
                                {
                                    jobApplication.Job.LastModifiedBy = employee.Id;
                                }

                                JobApplicationWorkPermitType jobApplicationWorkPermitResponse = rpoContext.JobApplicationWorkPermitTypes.Include("JobApplication.JobApplicationType")
                                                                                                 .Include("JobApplication.ApplicationStatus")
                                                                                                 .Include("JobWorkType")
                                                                                                 .Include("ContactResponsible")
                                                                                                 .FirstOrDefault(r => r.Id == jobApplicationWorkPermitType.Id);

                                string addWorkPermit_DOT = JobHistoryMessages.AddWorkPermit_DOT
                                                           .Replace("##PermitNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermitResponse.PermitNumber) ? jobApplicationWorkPermitResponse.PermitNumber : JobHistoryMessages.NoSetstring)
                                                           .Replace("##PermitType##", !string.IsNullOrEmpty(jobApplicationWorkPermitResponse.PermitType) ? jobApplicationWorkPermitResponse.PermitType : JobHistoryMessages.NoSetstring)
                                                            .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) ? jobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetFrom) ? jobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetTo) ? jobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                           .Replace("##ApplicationNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermitResponse.JobApplication.ApplicationNumber) ? jobApplicationWorkPermitResponse.JobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);
                                Common.SaveJobHistory(employee.Id, jobApplication.IdJob, addWorkPermit_DOT, JobHistoryType.WorkPermits);

                                rpoContext.SaveChanges();
                            }
                            else if (jobApplicationWorkTypeExists==null)
                            {
                                jobApplicationWorkPermitType = new JobApplicationWorkPermitType();

                                //JobWorkType jobWorkType = rpoContext.JobWorkTypes.FirstOrDefault(x => x.Code == jobApplicationWorkPermit.WorkTypeCode && x.JobApplicationType_Id == jobApplication.IdJobApplicationType);
                                jobApplicationWorkPermitType.LastModifiedDate = DateTime.UtcNow;
                                jobApplicationWorkPermitType.CreatedDate = DateTime.UtcNow;
                                if (employee != null)
                                {
                                    jobApplicationWorkPermitType.CreatedBy = employee.Id;
                                }

                                jobApplicationWorkPermitType.IdJobApplication = jobApplication.Id;
                                jobApplicationWorkPermitType.EstimatedCost = jobApplicationWorkPermit.EstimatedCost;
                                jobApplicationWorkPermitType.PermitType = jobApplicationWorkPermit.PermitType;
                                jobApplicationWorkPermitType.ForPurposeOf = jobApplicationWorkPermit.ForPurposeOf;
                                jobApplicationWorkPermitType.EquipmentType = jobApplicationWorkPermit.EquipmentType;
                                jobApplicationWorkPermitType.Code = jobApplicationWorkPermit.Code;

                                jobApplicationWorkPermitType.Withdrawn = jobApplicationWorkPermit.Withdrawn;
                                jobApplicationWorkPermitType.Filed = jobApplicationWorkPermit.Filed;
                                jobApplicationWorkPermitType.Issued = jobApplicationWorkPermit.Issued;
                                jobApplicationWorkPermitType.Expires = jobApplicationWorkPermit.Expires;
                                jobApplicationWorkPermitType.SignedOff = jobApplicationWorkPermit.SignedOff;
                                jobApplicationWorkPermitType.WorkDescription = jobApplicationWorkPermit.WorkDescription;
                                jobApplicationWorkPermitType.IsPersonResponsible = false;
                                jobApplicationWorkPermitType.PermitNumber = jobApplicationWorkPermit.PermitNumber;
                                jobApplicationWorkPermitType.PreviousPermitNumber = jobApplicationWorkPermit.PreviousPermitNumber;
                                jobApplicationWorkPermitType.RenewalFee = jobApplicationWorkPermit.RenewalFee;
                                jobApplicationWorkPermitType.HasSiteSafetyCoordinator = jobApplicationWorkPermit.HasSiteSafetyCoordinator;
                                jobApplicationWorkPermitType.HasSiteSafetyManager = jobApplicationWorkPermit.HasSiteSafetyManager;
                                jobApplicationWorkPermitType.HasSuperintendentofconstruction = jobApplicationWorkPermit.HasSuperintendentofconstruction;

                                rpoContext.JobApplicationWorkPermitTypes.Add(jobApplicationWorkPermitType);
                                rpoContext.SaveChanges();

                                if (string.IsNullOrEmpty(workPermitIds))
                                {
                                    workPermitIds = workPermitIds + jobApplicationWorkPermitType.Id;
                                }
                                else
                                {
                                    workPermitIds = workPermitIds + "," + jobApplicationWorkPermitType.Id;
                                }

                                string jobWorkTypeName = string.Empty;
                                if (jobApplicationWorkPermitType.JobWorkType != null)
                                {
                                    jobWorkTypeName = jobApplicationWorkPermitType.JobWorkType.Description;
                                }

                                jobApplication.Job.LastModiefiedDate = DateTime.UtcNow;
                                if (employee != null)
                                {
                                    jobApplication.Job.LastModifiedBy = employee.Id;
                                }

                                JobApplicationWorkPermitType jobApplicationWorkPermitResponse = rpoContext.JobApplicationWorkPermitTypes.Include("JobApplication.JobApplicationType")
                                                                                                 .Include("JobApplication.ApplicationStatus")
                                                                                                 .Include("JobWorkType")
                                                                                                 .Include("ContactResponsible")
                                                                                                 .FirstOrDefault(r => r.Id == jobApplicationWorkPermitType.Id);

                                string addWorkPermit_DOT = JobHistoryMessages.AddWorkPermit_DOT
                                                           .Replace("##PermitNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermitResponse.PermitNumber) ? jobApplicationWorkPermitResponse.PermitNumber : JobHistoryMessages.NoSetstring)
                                                           .Replace("##PermitType##", !string.IsNullOrEmpty(jobApplicationWorkPermitResponse.PermitType) ? jobApplicationWorkPermitResponse.PermitType : JobHistoryMessages.NoSetstring)
                                                            .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) ? jobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetFrom) ? jobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetTo) ? jobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                           .Replace("##ApplicationNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermitResponse.JobApplication.ApplicationNumber) ? jobApplicationWorkPermitResponse.JobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);
                                Common.SaveJobHistory(employee.Id, jobApplication.IdJob, addWorkPermit_DOT, JobHistoryType.WorkPermits);

                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                jobApplicationWorkPermitType.LastModifiedDate = DateTime.UtcNow;
                                if (employee != null)
                                {
                                    jobApplicationWorkPermitType.LastModifiedBy = employee.Id;
                                }


                                if (jobApplicationWorkPermitType.PermitNumber != jobApplicationWorkPermit.PermitNumber)
                                {
                                    Common.SaveJobWorkPermitHistory(jobApplicationWorkPermitType.Id, null, jobApplicationWorkPermitType.PermitNumber, jobApplicationWorkPermit.PermitNumber, employee.Id);
                                }

                                jobApplicationWorkPermitType.IdJobApplication = jobApplication.Id;
                                jobApplicationWorkPermitType.EstimatedCost = jobApplicationWorkPermit.EstimatedCost;
                                jobApplicationWorkPermitType.PermitType = jobApplicationWorkPermit.PermitType;
                                jobApplicationWorkPermitType.ForPurposeOf = jobApplicationWorkPermit.ForPurposeOf;
                                jobApplicationWorkPermitType.EquipmentType = jobApplicationWorkPermit.EquipmentType;
                                jobApplicationWorkPermitType.Withdrawn = jobApplicationWorkPermit.Withdrawn;
                                jobApplicationWorkPermitType.Code = jobApplicationWorkPermit.Code;
                                jobApplicationWorkPermitType.Filed = jobApplicationWorkPermit.Filed;
                                jobApplicationWorkPermitType.Issued = jobApplicationWorkPermit.Issued;
                                jobApplicationWorkPermitType.Expires = jobApplicationWorkPermit.Expires;
                                jobApplicationWorkPermitType.SignedOff = jobApplicationWorkPermit.SignedOff;
                                jobApplicationWorkPermitType.WorkDescription = jobApplicationWorkPermit.WorkDescription;
                                jobApplicationWorkPermitType.IsPersonResponsible = false;
                                jobApplicationWorkPermitType.PermitNumber = jobApplicationWorkPermit.PermitNumber;
                                jobApplicationWorkPermitType.PreviousPermitNumber = jobApplicationWorkPermit.PreviousPermitNumber;
                                jobApplicationWorkPermitType.RenewalFee = jobApplicationWorkPermit.RenewalFee;
                                jobApplicationWorkPermitType.HasSiteSafetyCoordinator = jobApplicationWorkPermit.HasSiteSafetyCoordinator;
                                jobApplicationWorkPermitType.HasSiteSafetyManager = jobApplicationWorkPermit.HasSiteSafetyManager;
                                jobApplicationWorkPermitType.HasSuperintendentofconstruction = jobApplicationWorkPermit.HasSuperintendentofconstruction;

                                rpoContext.SaveChanges();

                                if (string.IsNullOrEmpty(workPermitIds))
                                {
                                    workPermitIds = workPermitIds + jobApplicationWorkPermitType.Id;
                                }
                                else
                                {
                                    workPermitIds = workPermitIds + "," + jobApplicationWorkPermitType.Id;
                                }

                                string jobWorkTypeName = string.Empty;
                                jobWorkTypeName = jobApplicationWorkPermit.PermitType;

                                jobApplication.Job.LastModiefiedDate = DateTime.UtcNow;
                                if (employee != null)
                                {
                                    jobApplication.Job.LastModifiedBy = employee.Id;
                                }

                                string EditApplication_DOT = JobHistoryMessages.EditWorkPermit_DOT
                                                                                .Replace("##OldPermitNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermitType.PreviousPermitNumber) ? jobApplicationWorkPermitType.PreviousPermitNumber : JobHistoryMessages.NoSetstring)
                                                                                .Replace("##PermitType##", !string.IsNullOrEmpty(jobApplicationWorkPermitType.PermitType) ? jobApplicationWorkPermitType.PermitType : JobHistoryMessages.NoSetstring)
                                                                                .Replace("##NewPermitNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermitType.PermitNumber) ? jobApplicationWorkPermitType.PermitNumber : JobHistoryMessages.NoSetstring)
                                                                                .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetWorkingOn) ? jobApplicationWorkPermitType.JobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetFrom) ? jobApplicationWorkPermitType.JobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetTo) ? jobApplicationWorkPermitType.JobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                                                .Replace("##ApplicationNumber##", jobApplicationWorkPermitType.JobApplication != null && jobApplicationWorkPermitType.JobApplication.ApplicationNumber != null ? jobApplicationWorkPermitType.JobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);
                                Common.SaveJobHistory(employee.Id, jobApplicationWorkPermitType.JobApplication.IdJob, EditApplication_DOT, JobHistoryType.Applications);

                                rpoContext.SaveChanges();
                            }
                        }                       
                        else
                        {
                            jobApplicationWorkPermitType.LastModifiedDate = DateTime.UtcNow;
                            if (employee != null)
                            {
                                jobApplicationWorkPermitType.LastModifiedBy = employee.Id;
                            }

                            if (jobApplicationWorkPermitType.PermitNumber != jobApplicationWorkPermit.PermitNumber)
                            {
                                Common.SaveJobWorkPermitHistory(jobApplicationWorkPermitType.Id, null, jobApplicationWorkPermitType.PermitNumber, jobApplicationWorkPermit.PermitNumber, employee.Id);
                            }

                            jobApplicationWorkPermitType.IdJobApplication = jobApplication.Id;
                            jobApplicationWorkPermitType.EstimatedCost = jobApplicationWorkPermit.EstimatedCost;
                            jobApplicationWorkPermitType.PermitType = jobApplicationWorkPermit.PermitType;
                            jobApplicationWorkPermitType.ForPurposeOf = jobApplicationWorkPermit.ForPurposeOf;
                            jobApplicationWorkPermitType.EquipmentType = jobApplicationWorkPermit.EquipmentType;
                            jobApplicationWorkPermitType.Withdrawn = jobApplicationWorkPermit.Withdrawn;
                            jobApplicationWorkPermitType.Code = jobApplicationWorkPermit.Code;
                            jobApplicationWorkPermitType.Filed = jobApplicationWorkPermit.Filed;
                            jobApplicationWorkPermitType.Issued = jobApplicationWorkPermit.Issued;
                            jobApplicationWorkPermitType.Expires = jobApplicationWorkPermit.Expires;
                            jobApplicationWorkPermitType.SignedOff = jobApplicationWorkPermit.SignedOff;
                            jobApplicationWorkPermitType.WorkDescription = jobApplicationWorkPermit.WorkDescription;
                            jobApplicationWorkPermitType.IsPersonResponsible = false;
                            jobApplicationWorkPermitType.PermitNumber = jobApplicationWorkPermit.PermitNumber;
                            jobApplicationWorkPermitType.PreviousPermitNumber = jobApplicationWorkPermit.PreviousPermitNumber;
                            jobApplicationWorkPermitType.RenewalFee = jobApplicationWorkPermit.RenewalFee;
                            jobApplicationWorkPermitType.HasSiteSafetyCoordinator = jobApplicationWorkPermit.HasSiteSafetyCoordinator;
                            jobApplicationWorkPermitType.HasSiteSafetyManager = jobApplicationWorkPermit.HasSiteSafetyManager;
                            jobApplicationWorkPermitType.HasSuperintendentofconstruction = jobApplicationWorkPermit.HasSuperintendentofconstruction;

                            rpoContext.SaveChanges();

                            if (string.IsNullOrEmpty(workPermitIds))
                            {
                                workPermitIds = workPermitIds + jobApplicationWorkPermitType.Id;
                            }
                            else
                            {
                                workPermitIds = workPermitIds + "," + jobApplicationWorkPermitType.Id;
                            }

                            string jobWorkTypeName = string.Empty;
                            jobWorkTypeName = jobApplicationWorkPermit.PermitType;

                            jobApplication.Job.LastModiefiedDate = DateTime.UtcNow;
                            if (employee != null)
                            {
                                jobApplication.Job.LastModifiedBy = employee.Id;
                            }

                            string EditApplication_DOT = JobHistoryMessages.EditWorkPermit_DOT
                                                 .Replace("##OldPermitNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermitType.PreviousPermitNumber) ? jobApplicationWorkPermitType.PreviousPermitNumber : JobHistoryMessages.NoSetstring)
                                                 .Replace("##PermitType##", !string.IsNullOrEmpty(jobApplicationWorkPermitType.PermitType) ? jobApplicationWorkPermitType.PermitType : JobHistoryMessages.NoSetstring)
                                                 .Replace("##NewPermitNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermitType.PermitNumber) ? jobApplicationWorkPermitType.PermitNumber : JobHistoryMessages.NoSetstring)
                                                .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetWorkingOn) ? jobApplicationWorkPermitType.JobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetFrom) ? jobApplicationWorkPermitType.JobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetTo) ? jobApplicationWorkPermitType.JobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                 .Replace("##ApplicationNumber##", jobApplicationWorkPermitType.JobApplication != null && jobApplicationWorkPermitType.JobApplication.ApplicationNumber != null ? jobApplicationWorkPermitType.JobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);
                            Common.SaveJobHistory(employee.Id, jobApplicationWorkPermitType.JobApplication.IdJob, EditApplication_DOT, JobHistoryType.Applications);

                            rpoContext.SaveChanges();
                        }

                        UpdateApplicationFor(jobApplicationWorkPermitType.IdJobApplication);
                    }

                    return Ok(new { WorkPermitIds = workPermitIds, Message = "Work permit imported successfully" });
                }
                else
                {
                    throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        /// <summary>
        ///the multiple permit complated to update the job application work permits.
        /// </summary>
        /// <param name="jobApplicationWorkPermit">The job application work permit.</param>
        /// <returns>multiple permit complated to update the job application work permits..</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobApplicationWorkPermitType))]
        [HttpPut]
        [Route("api/JobApplicationWorkPermitsCompleted/{id}")]
        public IHttpActionResult CompletedJobApplicationWorkPermit(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteApplicationsWorkPermits))
            {
                JobApplicationWorkPermitType jobApplicationWorkPermit = rpoContext.JobApplicationWorkPermitTypes.Include("JobApplication").Include("JobApplication.JobApplicationType")
               .Include("JobApplication.Job")
                .Include("JobWorkType")
               .Include("JobWorkPermitHistories")
               .FirstOrDefault(r => r.Id == id);

                if (jobApplicationWorkPermit == null)
                {
                    return this.NotFound();
                }
                // DeleteJobDcumentsthroughWorkPermit(id);
                //var jobApplication = db.JobApplications.FirstOrDefault(e => e.Id == jobApplicationWorkPermit.IdJobApplication);
                string jobApplicationTypeName = string.Empty;
                int? jobApplicationTypeParent = 0;
                int idJob = jobApplicationWorkPermit.JobApplication != null ? jobApplicationWorkPermit.JobApplication.IdJob : 0;
                JobApplication jobApplication = jobApplicationWorkPermit.JobApplication;
                if (jobApplication != null && jobApplication.JobApplicationType != null)
                {
                    jobApplicationTypeName = Convert.ToString(jobApplication.JobApplicationType.Description);
                    jobApplicationTypeParent = jobApplication.JobApplicationType.IdParent;
                }

                string jobWorkTypeName = string.Empty;
                if (jobApplicationWorkPermit.JobWorkType != null)
                {
                    jobWorkTypeName = jobApplicationWorkPermit.JobWorkType.Description;
                }

                jobApplicationWorkPermit.JobApplication.Job.LastModiefiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobApplicationWorkPermit.JobApplication.Job.LastModifiedBy = employee.Id;
                }
                string deleteWorkPermit_DOB = string.Empty;
                string CompleteWorkPermit_DOT = string.Empty;
                string deleteWorkPermit_DEP = string.Empty;

                switch ((ApplicationType)Convert.ToInt32(jobApplicationTypeParent))
                {
                    case ApplicationType.DOB:
                        deleteWorkPermit_DOB = JobHistoryMessages.DeleteWorkPermit_DOB
                                                   .Replace("##PermitType##", jobApplicationWorkPermit.JobWorkType != null ? jobApplicationWorkPermit.JobWorkType.Description : JobHistoryMessages.NoSetstring)
                                                    .Replace("##ApplicationType##", !string.IsNullOrEmpty(jobApplicationTypeName) ? jobApplicationTypeName : JobHistoryMessages.NoSetstring)
                                                    .Replace("##ApplicationNumber##", !string.IsNullOrEmpty(jobApplication.ApplicationNumber) ? jobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);

                        //Common.SaveJobHistory(employee.Id, idJob, deleteWorkPermit_DOB, JobHistoryType.WorkPermits);
                        break;
                    case ApplicationType.DOT:
                        CompleteWorkPermit_DOT = JobHistoryMessages.CompleteWorkPermit_DOT
                                                  .Replace("##PermitNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermit.PermitNumber) ? jobApplicationWorkPermit.PermitNumber : JobHistoryMessages.NoSetstring)
                                                  .Replace("##PermitType##", !string.IsNullOrEmpty(jobApplicationWorkPermit.PermitType) ? jobApplicationWorkPermit.PermitType : JobHistoryMessages.NoSetstring)
                                                .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) ? jobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetFrom) ? jobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetTo) ? jobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                  .Replace("##ApplicationNumber##", !string.IsNullOrEmpty(jobApplication.ApplicationNumber) ? jobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);
                        //Common.SaveJobHistory(employee.Id, idJob, CompleteWorkPermit_DOT, JobHistoryType.WorkPermits);
                        break;
                    case ApplicationType.DEP:
                        deleteWorkPermit_DEP = JobHistoryMessages.DeleteWorkPermit_DEP
                                                      .Replace("##PermitNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermit.PermitNumber) ? jobApplicationWorkPermit.PermitNumber : JobHistoryMessages.NoSetstring)
                                                      .Replace("##PermitType##", jobApplicationWorkPermit.JobWorkType != null ? jobApplicationWorkPermit.JobWorkType.Description : JobHistoryMessages.NoSetstring)
                                                      .Replace("##ApplicationType##", !string.IsNullOrEmpty(jobApplicationTypeName) ? jobApplicationTypeName : JobHistoryMessages.NoSetstring)
                                                      .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) ? jobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetFrom) ? jobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetTo) ? jobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                     .Replace("##ApplicationNumber##", !string.IsNullOrEmpty(jobApplication.ApplicationNumber) ? jobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);
                        // Common.SaveJobHistory(employee.Id, idJob, deleteWorkPermit_DEP, JobHistoryType.WorkPermits);
                        break;
                }

                if ((ApplicationType)Convert.ToInt32(jobApplicationTypeParent) == ApplicationType.DOT)
                {
                    jobApplicationWorkPermit.IsCompleted = true;
                }
                //else
                //{
                //    rpoContext.JobApplicationWorkPermitTypes.Remove(jobApplicationWorkPermit);
                //}

                rpoContext.SaveChanges();

                switch ((ApplicationType)Convert.ToInt32(jobApplicationTypeParent))
                {
                    case ApplicationType.DOB:
                        Common.SaveJobHistory(employee.Id, idJob, deleteWorkPermit_DOB, JobHistoryType.WorkPermits);
                        break;
                    case ApplicationType.DOT:
                        Common.SaveJobHistory(employee.Id, idJob, CompleteWorkPermit_DOT, JobHistoryType.WorkPermits);
                        break;
                    case ApplicationType.DEP:
                        Common.SaveJobHistory(employee.Id, idJob, deleteWorkPermit_DEP, JobHistoryType.WorkPermits);
                        break;
                }

                UpdateApplicationFor(jobApplicationWorkPermit.IdJobApplication);

                return Ok(new { status = "succssess" });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the job application work permit.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Deletes the job application work permit.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobApplicationWorkPermitType))]
        public IHttpActionResult DeleteJobApplicationWorkPermit(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteApplicationsWorkPermits))
            {
                JobApplicationWorkPermitType jobApplicationWorkPermit = rpoContext.JobApplicationWorkPermitTypes.Include("JobApplication").Include("JobApplication.JobApplicationType")
               .Include("JobApplication.Job")
                .Include("JobWorkType")
               .Include("JobWorkPermitHistories")
               .FirstOrDefault(r => r.Id == id);

                if (jobApplicationWorkPermit == null)
                {
                    return this.NotFound();
                }
                // DeleteJobDcumentsthroughWorkPermit(id);
                //var jobApplication = db.JobApplications.FirstOrDefault(e => e.Id == jobApplicationWorkPermit.IdJobApplication);
                string jobApplicationTypeName = string.Empty;
                int? jobApplicationTypeParent = 0;
                int idJob = jobApplicationWorkPermit.JobApplication != null ? jobApplicationWorkPermit.JobApplication.IdJob : 0;
                JobApplication jobApplication = jobApplicationWorkPermit.JobApplication;
                if (jobApplication != null && jobApplication.JobApplicationType != null)
                {
                    jobApplicationTypeName = Convert.ToString(jobApplication.JobApplicationType.Description);
                    jobApplicationTypeParent = jobApplication.JobApplicationType.IdParent;
                }

                string jobWorkTypeName = string.Empty;
                if (jobApplicationWorkPermit.JobWorkType != null)
                {
                    jobWorkTypeName = jobApplicationWorkPermit.JobWorkType.Description;
                }

                jobApplicationWorkPermit.JobApplication.Job.LastModiefiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobApplicationWorkPermit.JobApplication.Job.LastModifiedBy = employee.Id;
                }
                string deleteWorkPermit_DOB = string.Empty;
                string CompleteWorkPermit_DOT = string.Empty;
                string deleteWorkPermit_DEP = string.Empty;

                switch ((ApplicationType)Convert.ToInt32(jobApplicationTypeParent))
                {
                    case ApplicationType.DOB:
                        deleteWorkPermit_DOB = JobHistoryMessages.DeleteWorkPermit_DOB
                                                   .Replace("##PermitType##", jobApplicationWorkPermit.JobWorkType != null ? jobApplicationWorkPermit.JobWorkType.Description : JobHistoryMessages.NoSetstring)
                                                    .Replace("##ApplicationType##", !string.IsNullOrEmpty(jobApplicationTypeName) ? jobApplicationTypeName : JobHistoryMessages.NoSetstring)
                                                    .Replace("##ApplicationNumber##", !string.IsNullOrEmpty(jobApplication.ApplicationNumber) ? jobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);

                        //Common.SaveJobHistory(employee.Id, idJob, deleteWorkPermit_DOB, JobHistoryType.WorkPermits);
                        break;
                    case ApplicationType.DOT:
                        CompleteWorkPermit_DOT = JobHistoryMessages.CompleteWorkPermit_DOT
                                                  .Replace("##PermitNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermit.PermitNumber) ? jobApplicationWorkPermit.PermitNumber : JobHistoryMessages.NoSetstring)
                                                  .Replace("##PermitType##", !string.IsNullOrEmpty(jobApplicationWorkPermit.PermitType) ? jobApplicationWorkPermit.PermitType : JobHistoryMessages.NoSetstring)
                                                .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) ? jobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetFrom) ? jobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetTo) ? jobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                  .Replace("##ApplicationNumber##", !string.IsNullOrEmpty(jobApplication.ApplicationNumber) ? jobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);
                        //Common.SaveJobHistory(employee.Id, idJob, CompleteWorkPermit_DOT, JobHistoryType.WorkPermits);
                        break;
                    case ApplicationType.DEP:
                        deleteWorkPermit_DEP = JobHistoryMessages.DeleteWorkPermit_DEP
                                                      .Replace("##PermitNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermit.PermitNumber) ? jobApplicationWorkPermit.PermitNumber : JobHistoryMessages.NoSetstring)
                                                      .Replace("##PermitType##", jobApplicationWorkPermit.JobWorkType != null ? jobApplicationWorkPermit.JobWorkType.Description : JobHistoryMessages.NoSetstring)
                                                      .Replace("##ApplicationType##", !string.IsNullOrEmpty(jobApplicationTypeName) ? jobApplicationTypeName : JobHistoryMessages.NoSetstring)
                                                      .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) ? jobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetFrom) ? jobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetTo) ? jobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                     .Replace("##ApplicationNumber##", !string.IsNullOrEmpty(jobApplication.ApplicationNumber) ? jobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);
                        // Common.SaveJobHistory(employee.Id, idJob, deleteWorkPermit_DEP, JobHistoryType.WorkPermits);
                        break;
                }

                //if ((ApplicationType)Convert.ToInt32(jobApplicationTypeParent) == ApplicationType.DOT)
                //{
                //    jobApplicationWorkPermit.IsCompleted = true;
                //}
                //else
                //{
                rpoContext.JobApplicationWorkPermitTypes.Remove(jobApplicationWorkPermit);
                // }

                rpoContext.SaveChanges();

                switch ((ApplicationType)Convert.ToInt32(jobApplicationTypeParent))
                {
                    case ApplicationType.DOB:
                        Common.SaveJobHistory(employee.Id, idJob, deleteWorkPermit_DOB, JobHistoryType.WorkPermits);
                        break;
                    case ApplicationType.DOT:
                        Common.SaveJobHistory(employee.Id, idJob, CompleteWorkPermit_DOT, JobHistoryType.WorkPermits);
                        break;
                    case ApplicationType.DEP:
                        Common.SaveJobHistory(employee.Id, idJob, deleteWorkPermit_DEP, JobHistoryType.WorkPermits);
                        break;
                }

                UpdateApplicationFor(jobApplicationWorkPermit.IdJobApplication);

                return Ok(new { status = "succssess" });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        private bool DeleteJobDcumentsthroughWorkPermit(int Permitid)
        {
            bool status = false;
            if (Permitid != 0)
            {
                var path = HttpRuntime.AppDomainAppPath;
                string newFileDirectoryName = string.Empty;
                string fileName = string.Empty;
                string idJob = string.Empty;
                string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

                List<JobDocument> jobDocument = this.rpoContext.JobDocuments.Include("JobDocumentFields").Include("JobDocumentAttachments").Where(x => x.IdJobApplicationWorkPermitType == Permitid).ToList();

                foreach (var item in jobDocument)
                {

                    fileName = path + Properties.Settings.Default.JobDocumentExportPath + Convert.ToString(item.IdJob) + "\\" + Convert.ToString(item.Id) + "_" + item.DocumentPath;
                    this.rpoContext.JobDocumentAttachments.RemoveRange(item.JobDocumentAttachments);
                    this.rpoContext.JobDocumentFields.RemoveRange(item.JobDocumentFields);
                    this.rpoContext.JobDocuments.Remove(item);
                    this.rpoContext.SaveChanges();

                    if (fileName != null)
                    {
                        if (File.Exists(fileName))
                        {
                            File.Delete(fileName);
                        }
                    }

                    var instance = new DropboxIntegration();
                    string uploadFilePath = Properties.Settings.Default.DropboxFolderPath + Convert.ToString(item.IdJob);
                    string dropBoxFileName = uploadFilePath + "/" + Convert.ToString(item.Id) + "_" + item.DocumentPath;
                    var task = instance.RunDelete(dropBoxFileName);
                }
                status = true;
            }
            return status;
        }

        /// <summary>
        /// Puts the job application work permit document.
        /// </summary>
        /// <returns>Update the job application permits for upload type document.</returns>
        /// <exception cref="HttpResponseException"></exception>
        [Authorize]
        [RpoAuthorize]
        [Route("api/JobApplicationWorkPermits/document")]
        [ResponseType(typeof(JobApplicationWorkPermitType))]
        public async Task<HttpResponseMessage> PutJobApplicationWorkPermitDocument()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            System.Collections.Specialized.NameValueCollection formData = provider.FormData;
            IList<HttpContent> files = provider.Files;
            HttpContent file1 = files[0];
            int idJob2 = Convert.ToInt32(formData["idJob"]);
            Stream input = await file1.ReadAsStreamAsync();

            PdfReader reader = new PdfReader(input);

            int intPageNum = reader.NumberOfPages;
            string[] words;
            string line = string.Empty;
            string text = string.Empty;

            bool isNewPermit = true;
            string permit = string.Empty;
            string previousPermit = string.Empty;
            string oldPermit = string.Empty;
            string issuedDate = string.Empty;
            string fromDate = string.Empty;
            string toDate = string.Empty;
            string permitType = string.Empty;
            int? IdJob = null;
            string JobNumber = string.Empty;

            string dotDescription = string.Empty;
            string trackingNumber = string.Empty;
            string sequenceNumber = string.Empty;
            string contractNumber = string.Empty;

            string totalFee = string.Empty;
            string streetWorkingOn = string.Empty;
            string streetWorkingFrom = string.Empty;
            string streetWorkingTo = string.Empty;
            string workTypeCode = string.Empty;
            string forPurposeOf = string.Empty;
            string equipmentType = string.Empty;

            string datFiled = string.Empty;

            List<WorkPermitDocumentReadResponse> workPermitDocumentReadResponseList = new List<WorkPermitDocumentReadResponse>();

            for (int i = 1; i <= intPageNum; i++)
            {
                text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());

                words = text.Split('\n');
                for (int j = 0, len = words.Length; j < len; j++)
                {
                    line = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j]));

                    if (line.Contains("PERMIT#:"))
                    {
                        int start = line.IndexOf("PREVIOUS#:");

                        if (start > 0)
                        {
                            permit = line.Substring(0, start);
                            if (!string.IsNullOrEmpty(permit))
                            {
                                permit = permit.Replace("PERMIT#:", "").Trim();
                            }

                            previousPermit = line.Substring(start, line.Length - start);

                            if (!string.IsNullOrEmpty(previousPermit))
                            {
                                previousPermit = previousPermit.Replace("PREVIOUS#:", "").Trim();

                                JobApplicationWorkPermitType jobApplicationWorkPermitType = rpoContext.JobApplicationWorkPermitTypes.Include("JobApplication").Where(d => d.PreviousPermitNumber.Contains(previousPermit)).FirstOrDefault();
                                if (jobApplicationWorkPermitType != null)
                                {
                                    JobNumber = jobApplicationWorkPermitType.JobApplication.Job.JobNumber;
                                    IdJob = jobApplicationWorkPermitType.JobApplication.IdJob;
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(line))
                            {
                                permit = line.Replace("PERMIT#:", "").Trim();
                            }
                        }
                    }
                    else if (line.Contains("ISSUED DATE:"))
                    {
                        int start = line.IndexOf("PERMIT VALID FROM:");
                        int between = line.IndexOf("TO:");
                        if (start > 0 && between > 0)
                        {
                            issuedDate = line.Substring(0, start);
                            if (!string.IsNullOrEmpty(permit))
                            {
                                issuedDate = issuedDate.Replace("ISSUED DATE:", "").Trim();
                            }

                            fromDate = line.Substring(start, between - start);

                            if (!string.IsNullOrEmpty(fromDate))
                            {
                                fromDate = fromDate.Replace("PERMIT VALID FROM:", "").Trim();
                            }

                            toDate = line.Substring(between, line.Length - between);

                            if (!string.IsNullOrEmpty(toDate))
                            {
                                toDate = toDate.Replace("TO:", "").Trim();
                            }
                        }
                        else if (start > 0)
                        {
                            issuedDate = line.Substring(0, start);
                            if (!string.IsNullOrEmpty(permit))
                            {
                                issuedDate = issuedDate.Replace("ISSUED DATE:", "").Trim();
                            }

                            fromDate = line.Substring(start, line.Length - start);

                            if (!string.IsNullOrEmpty(fromDate))
                            {
                                fromDate = fromDate.Replace("PERMIT VALID FROM:", "").Trim();
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(line))
                            {
                                issuedDate = line.Replace("ISSUED DATE:", "").Trim();
                            }
                        }
                    }
                    else if (line.Contains("CONVERTED DATE:"))
                    {
                        int start = line.IndexOf("PERMIT VALID FROM:");
                        int between = line.IndexOf("TO:");
                        if (start > 0 && between > 0)
                        {
                            issuedDate = line.Substring(0, start);
                            if (!string.IsNullOrEmpty(permit))
                            {
                                issuedDate = issuedDate.Replace("CONVERTED DATE:", "").Trim();
                            }

                            fromDate = line.Substring(start, between - start);

                            if (!string.IsNullOrEmpty(fromDate))
                            {
                                fromDate = fromDate.Replace("PERMIT VALID FROM:", "").Trim();
                            }

                            toDate = line.Substring(between, line.Length - between);

                            if (!string.IsNullOrEmpty(toDate))
                            {
                                toDate = toDate.Replace("TO:", "").Trim();
                            }
                        }
                        else if (start > 0)
                        {
                            issuedDate = line.Substring(0, start);
                            if (!string.IsNullOrEmpty(permit))
                            {
                                issuedDate = issuedDate.Replace("CONVERTED DATE:", "").Trim();
                            }

                            fromDate = line.Substring(start, line.Length - start);

                            if (!string.IsNullOrEmpty(fromDate))
                            {
                                fromDate = fromDate.Replace("PERMIT VALID FROM:", "").Trim();
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(line))
                            {
                                issuedDate = line.Replace("CONVERTED DATE:", "").Trim();
                            }
                        }
                    }
                    else if (line.Contains("PERMIT TYPE:"))
                    {
                        int start = line.IndexOf("PERMIT TYPE:");
                        dotDescription = line.Substring(start, line.Length - start);
                        dotDescription = dotDescription.Replace("PERMIT TYPE:", "").Trim();

                        if (!string.IsNullOrEmpty(dotDescription) && dotDescription.Split('-') != null && dotDescription.Split('-').Count() > 0)
                        {
                            string[] code = dotDescription.Split('-');
                            workTypeCode = code[0].Trim();
                        }

                        if (j < words.Length - 1)
                        {
                            string nextline = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j + 1]));
                            if (!nextline.Contains("FEES (NON-REFUNDABLE):"))
                            {
                                dotDescription = dotDescription + " " + nextline;
                                if (j < words.Length - 2)
                                {
                                    nextline = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j + 2]));
                                    if (!nextline.Contains("FEES (NON-REFUNDABLE):"))
                                    {
                                        dotDescription = dotDescription + " " + nextline;
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(dotDescription) && dotDescription.Split('-') != null && dotDescription.Split('-').Count() > 1)
                        {
                            string[] code = dotDescription.Split('-');
                            permitType = code[1].Trim();
                        }
                    }
                    else if (line.Contains("FOR PURPOSE OF:"))
                    {
                        int start = line.IndexOf("FOR PURPOSE OF:");
                        string perposefor = line.Substring(start, line.Length - start);
                        perposefor = perposefor.Replace("FOR PURPOSE OF:", "").Trim();
                        forPurposeOf = perposefor;
                    }

                    else if (line.Contains("TRACKING #:"))
                    {
                        int start = line.IndexOf("TRACKING #:");
                        trackingNumber = line.Substring(start, line.Length - start);
                        trackingNumber = trackingNumber.Replace("TRACKING #:", "").Trim();


                        if (!string.IsNullOrEmpty(trackingNumber) && trackingNumber.Length > 8)
                        {
                            int year = int.Parse(trackingNumber.Substring(0, 4));
                            int mnth = int.Parse(trackingNumber.Substring(4, 2));
                            int day = int.Parse(trackingNumber.Substring(6, 2));

                            datFiled = mnth + "/" + day + "/" + year;
                        }
                    }
                    else if (line.Contains("SEQUENCE #:"))
                    {
                        int start = line.IndexOf("SEQUENCE #:");
                        sequenceNumber = line.Substring(start, line.Length - start);
                        sequenceNumber = sequenceNumber.Replace("SEQUENCE #:", "").Trim();
                    }
                    else if (line.Contains("TOTAL : $"))
                    {
                        totalFee = string.Empty;
                        string renewalFees = string.Empty;
                        renewalFees = line.Replace("TOTAL : ", "").Trim();

                        if (j < words.Length - 1)
                        {
                            string nextline = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j + 1]));
                            if (!nextline.Contains("PERMISSION HEREBY GRANTED TO:"))
                            {
                                renewalFees = renewalFees + " " + nextline;
                            }
                        }

                        if (renewalFees.Contains("WAIVED"))
                        {
                            totalFee = "0";
                        }
                        else
                        {
                            bool isStart = false;
                            foreach (char item in renewalFees)
                            {
                                if (isStart)
                                {
                                    totalFee = totalFee + item;
                                    if (item == ' ')
                                    {
                                        isStart = false;
                                    }
                                }

                                if (item == '$')
                                {
                                    isStart = true;
                                }
                            }
                        }
                    }
                    else if (line.Contains("EQUIPMENT TYPE:"))
                    {
                        int start = line.IndexOf("EQUIPMENT TYPE:");
                        equipmentType = line.Substring(start, line.Length - start);
                        equipmentType = line.Replace("EQUIPMENT TYPE:", "").Trim();

                        if (j < words.Length - 1)
                        {
                            string nextline = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j + 1]));
                            if (!nextline.Contains("INSPECT DIST"))
                            {
                                equipmentType = equipmentType + " " + nextline;
                            }
                            if (j < words.Length - 2)
                            {
                                nextline = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j + 2]));
                                if (!nextline.Contains("INSPECT DIST:") && !nextline.Contains("RECORDED:"))
                                {
                                    equipmentType = equipmentType + " " + nextline;
                                }
                            }
                        }
                    }
                    else if (line.Contains("ON STREET:"))
                    {
                        streetWorkingOn = line.Replace("ON STREET:", "").Trim();
                    }
                    else if (line.Contains("FROM STREET:"))
                    {
                        streetWorkingFrom = line.Replace("FROM STREET:", "").Trim();
                    }
                    else if (line.Contains("TO STREET:"))
                    {
                        streetWorkingTo = line.Replace("TO STREET:", "").Trim();
                    }
                    else if (line.Contains("CONTRACT #:"))
                    {
                        int start = line.IndexOf("CONTRACT #:");
                        contractNumber = line.Substring(start, line.Length - start);
                        contractNumber = contractNumber.Replace("CONTRACT #:", "").Trim();
                    }
                    
                    List<JobApplication> jobApplications = JobTrackingNumberExists(trackingNumber, 0, 0, 0, streetWorkingOn, streetWorkingFrom, streetWorkingTo, permit);
                   
                    if (jobApplications != null && jobApplications.Count > 0 && !string.IsNullOrEmpty(trackingNumber))
                    {
                        if (idJob2 == jobApplications[0].IdJob)
                        {
                        }
                        else
                        {
                            throw new RpoBusinessException(string.Format(StaticMessages.TrackingNumberAleadyExists, (jobApplications[0].Job != null ? jobApplications[0].Job.JobNumber : string.Empty)));
                        }
                    }
                }
                if (!string.IsNullOrEmpty(permit))
                {
                    if (permit != oldPermit)
                    {
                        isNewPermit = true;
                        oldPermit = permit;
                    }

                    if (isNewPermit)
                    {
                        WorkPermitDocumentReadResponse workPermitDocumentReadResponse = new WorkPermitDocumentReadResponse();
                        workPermitDocumentReadResponse.PermitNumber = permit;
                        workPermitDocumentReadResponse.PreviousPermitNumber = previousPermit;
                        workPermitDocumentReadResponse.IssuedDate = issuedDate;
                        workPermitDocumentReadResponse.FromDate = datFiled;
                        workPermitDocumentReadResponse.ExpiredDate = toDate;
                        workPermitDocumentReadResponse.RenewalFees = totalFee.Trim();
                        workPermitDocumentReadResponse.DOTDescription = dotDescription;
                        workPermitDocumentReadResponse.PermitType = permitType;
                        workPermitDocumentReadResponse.SequenceNumber = sequenceNumber;
                        workPermitDocumentReadResponse.TrackingNumber = trackingNumber;
                        workPermitDocumentReadResponse.StreetWorkingOn = streetWorkingOn;
                        workPermitDocumentReadResponse.StreetWorkingTo = streetWorkingTo;
                        workPermitDocumentReadResponse.StreetWorkingFrom = streetWorkingFrom;
                        workPermitDocumentReadResponse.ContractNumber = contractNumber;
                        workPermitDocumentReadResponse.WorkTypeCode = workTypeCode;
                        workPermitDocumentReadResponse.ForPurposeOf = forPurposeOf;
                        workPermitDocumentReadResponse.EquipmentType = equipmentType;
                        workPermitDocumentReadResponse.JobNumber = JobNumber;
                        workPermitDocumentReadResponse.IdJob = IdJob;
                        workPermitDocumentReadResponseList.Add(workPermitDocumentReadResponse);
                        isNewPermit = false;
                    }
                }
            }

            if (workPermitDocumentReadResponseList != null && workPermitDocumentReadResponseList.Count > 0)
            {
                var response = Request.CreateResponse<List<WorkPermitDocumentReadResponse>>(HttpStatusCode.OK, workPermitDocumentReadResponseList.OrderBy(x => x.SequenceNumber).ToList());
                return response;
            }
            else
            {
                throw new RpoBusinessException("File is not in correct format");
            }

        }

        private List<JobApplication> JobTrackingNumberExists(string trackingNumber, int id, int idJob, int idJobApplicationTypeParent, string streetWorkingOn, string streetWorkingFrom, string streetWorkingTo, string Permitnumber)
        {
            var jobApplications = rpoContext.JobApplications.Include("Job").Include("JobWorkPermitHistories").Include("JobApplicationType").Where(e => (e.ApplicationNumber == trackingNumber || e.JobWorkPermitHistories.Any(p => p.NewNumber == trackingNumber || p.OldNumber == trackingNumber)) && e.StreetWorkingOn.ToLower().Contains(streetWorkingOn.ToLower()) && e.StreetFrom.ToLower().Contains(streetWorkingFrom.ToLower()) && e.StreetTo.ToLower().Contains(streetWorkingTo.ToLower()) && e.WorkPermitTypes.FirstOrDefault(p => p.PermitNumber == Permitnumber) != null).ToList();
            //var jobApplications = rpoContext.JobApplications.Include("Job").Include("JobApplicationType").Where(e => e.ApplicationNumber == trackingNumber && e.Id != id && e.IdJob != idJob && e.JobApplicationType.IdParent == idJobApplicationTypeParent).ToList();
            return jobApplications;
        }

        /// <summary>
        /// Puts the create jobdocument. and upload type of  document
        /// </summary>
        /// <returns>Puts the create jobdocument. and upload type of  document.</returns>
        /// <exception cref="HttpResponseException"></exception>
        [Authorize]
        [RpoAuthorize]
        [Route("api/JobApplicationWorkPermits/createjobdocument")]
        [ResponseType(typeof(JobApplicationWorkPermitType))]
        public async Task<HttpResponseMessage> PutCreateJobdocument()
        {

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            System.Collections.Specialized.NameValueCollection formData = provider.FormData;
            IList<HttpContent> files = provider.Files;
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            int idJob = Convert.ToInt32(formData["idJob"]);

            string workPermitIds = Convert.ToString(formData["WorkPermitIds"]);

            var path = HttpRuntime.AppDomainAppPath;
            HttpContent file1 = files[0];
            var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');
            string tempFilename = string.Empty;
            using (Stream input = await file1.ReadAsStreamAsync())
            {
                string tempDirectoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.TempDocument));

                string tempDirectoryFileName = Convert.ToString(Guid.NewGuid()) + "_" + thisFileName;
                tempFilename = System.IO.Path.Combine(tempDirectoryName, tempDirectoryFileName);

                if (!Directory.Exists(tempDirectoryName))
                {
                    Directory.CreateDirectory(tempDirectoryName);
                }

                using (Stream file = File.OpenWrite(tempFilename))
                {
                    input.CopyTo(file);
                    file.Close();
                }
            }

            JobDocument jobDocument = new JobDocument();
            if (!string.IsNullOrEmpty(workPermitIds) && workPermitIds.Split(',') != null && workPermitIds.Split(',').Count() > 0)
            {
                foreach (string idworkPermit in workPermitIds.Split(','))
                {
                    string filename = string.Empty;
                    string directoryName = string.Empty;
                    string URL = string.Empty;

                    int idDocument = Enums.Document.DOT_Permit.GetHashCode();
                    DocumentMaster documentMaster = rpoContext.DocumentMasters.FirstOrDefault(x => x.Id == idDocument);

                    int idjobApplicationWorkPermitType = Convert.ToInt32(idworkPermit);
                    JobApplicationWorkPermitType jobApplicationWorkPermitType = rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType").Include("JobApplication.JobApplicationType").FirstOrDefault(x => x.Id == idjobApplicationWorkPermitType);

                    jobDocument = new JobDocument();
                    jobDocument.IdJob = idJob;
                    jobDocument.IdDocument = idDocument;
                    jobDocument.DocumentName = documentMaster.DocumentName;
                    jobDocument.IdJobApplication = jobApplicationWorkPermitType.IdJobApplication;
                    jobDocument.IdJobApplicationWorkPermitType = jobApplicationWorkPermitType.Id;
                    jobDocument.DocumentDescription =
                        //(!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.ApplicationNumber) ? "Tracking# :" + jobApplicationWorkPermitType.JobApplication.ApplicationNumber + ", " : string.Empty)
                        //+
                        (!string.IsNullOrEmpty(jobApplicationWorkPermitType.PermitNumber) ? "Permit# :" + jobApplicationWorkPermitType.PermitNumber + ", " : string.Empty)
                        + (!string.IsNullOrEmpty(jobApplicationWorkPermitType.PermitType) ? "Work/Permit type :" + jobApplicationWorkPermitType.PermitType + ", " : string.Empty)
                        + (jobApplicationWorkPermitType.JobApplication != null && (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetFrom) || !string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.StreetTo)) ? ("Location :" + (jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.StreetWorkingOn : string.Empty) + " | " + (jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.StreetFrom : string.Empty) + " | " + (jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.StreetTo : string.Empty)) : string.Empty);
                    jobDocument.IsArchived = false;
                    jobDocument.CreatedDate = DateTime.UtcNow;
                    jobDocument.CreatedBy = employee.Id;
                    jobDocument.LastModifiedDate = DateTime.UtcNow;
                    jobDocument.LastModifiedBy = employee.Id;
                    jobDocument.TrackingNumber = jobApplicationWorkPermitType.JobApplication.ApplicationNumber;
                    jobDocument.PermitNumber = jobApplicationWorkPermitType.PermitNumber;
                    rpoContext.JobDocuments.Add(jobDocument);
                    rpoContext.SaveChanges();

                    JobDocumentAttachment jobDocumentAttachment = new JobDocumentAttachment();
                    jobDocumentAttachment.IdJobDocument = jobDocument.Id;
                    jobDocumentAttachment.DocumentName = jobApplicationWorkPermitType.PermitNumber + ".pdf";
                    jobDocumentAttachment.Path = jobApplicationWorkPermitType.PermitNumber + ".pdf";

                    rpoContext.JobDocumentAttachments.Add(jobDocumentAttachment);
                    rpoContext.SaveChanges();

                    var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == idDocument).ToList();

                    foreach (var item in documentFieldList)
                    {
                        if (item.Field.FieldName == "Application Type")
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = jobDocument.Id,
                                IdDocumentField = item.Id,
                                Value = jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null ? Convert.ToString(jobApplicationWorkPermitType.IdJobApplication) : string.Empty,
                                ActualValue = jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null && jobApplicationWorkPermitType.JobApplication.JobApplicationType != null ? jobApplicationWorkPermitType.JobApplication.JobApplicationType.Description : string.Empty
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                            rpoContext.SaveChanges();
                        }
                        else if (item.Field.FieldName == "WorkPermit")
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = jobDocument.Id,
                                IdDocumentField = item.Id,
                                Value = jobApplicationWorkPermitType != null ? Convert.ToString(jobApplicationWorkPermitType.Id) : string.Empty,
                                ActualValue = jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobWorkType != null ? jobApplicationWorkPermitType.JobWorkType.Description : string.Empty
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                            rpoContext.SaveChanges();
                        }
                        else if (item.Field.FieldName == "Attachment")
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = jobDocument.Id,
                                IdDocumentField = item.Id,
                                Value = jobApplicationWorkPermitType.PermitNumber + ".pdf",
                                ActualValue = jobApplicationWorkPermitType.PermitNumber + ".pdf"
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                            rpoContext.SaveChanges();
                        }
                        else
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = jobDocument.Id,
                                IdDocumentField = item.Id,
                                Value = string.Empty,
                                ActualValue = string.Empty
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                            rpoContext.SaveChanges();
                        }
                    }


                    jobApplicationWorkPermitType.DocumentPath = jobApplicationWorkPermitType.PermitNumber + ".pdf";
                    jobDocument.DocumentPath = jobApplicationWorkPermitType.PermitNumber + ".pdf";

                    rpoContext.SaveChanges();

                    directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentAttachmentPath));

                    string directoryFileName = Convert.ToString(jobDocumentAttachment.Id) + "_" + jobApplicationWorkPermitType.PermitNumber + ".pdf";
                    filename = System.IO.Path.Combine(directoryName, directoryFileName);



                    string directoryNameWorkPermit = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.DOTWorkPermitDocument));

                    string directoryFileNameWorkPermit = Convert.ToString(jobApplicationWorkPermitType.Id) + "_" + jobApplicationWorkPermitType.PermitNumber + ".pdf";
                    string Workpermitfilename = System.IO.Path.Combine(directoryNameWorkPermit, directoryFileNameWorkPermit);

                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }

                    //if (File.Exists(tempFilename))
                    //{
                    //    File.Copy(tempFilename, filename);
                    //}

                    PdfReader reader = new PdfReader(tempFilename);

                    int intPageNum = reader.NumberOfPages;
                    string[] words;
                    string line = string.Empty;
                    string text = string.Empty;

                    int startPage = 0;
                    int interval = 0;
                    for (int i = 1; i <= intPageNum; i++)
                    {
                        text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());
                        words = text.Split('\n');
                        for (int j = 0, len = words.Length; j < len; j++)
                        {
                            line = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j]));

                            if (line.Contains(jobApplicationWorkPermitType.PermitNumber))
                            {
                                if (startPage == 0)
                                {
                                    startPage = i;
                                }
                                interval++;
                            }
                        }
                    }
                    reader.Close();

                    SplitAndSaveInterval(tempFilename, startPage, interval, filename);

                    if (!Directory.Exists(directoryNameWorkPermit))
                    {
                        Directory.CreateDirectory(directoryNameWorkPermit);
                    }

                    if (File.Exists(Workpermitfilename))
                    {
                        File.Delete(Workpermitfilename);
                    }

                    //if (File.Exists(tempFilename))
                    //{
                    //    File.Copy(tempFilename, Workpermitfilename);
                    //}
                    SplitAndSaveInterval(tempFilename, startPage, interval, Workpermitfilename);

                    var instance = new DropboxIntegration();
                    string uploadFilePath = Properties.Settings.Default.DropboxFolderPath + Convert.ToString(jobDocument.IdJob);
                    string dropBoxFileName = Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;
                    string filepath = Workpermitfilename;
                    var task = instance.RunUpload(uploadFilePath, dropBoxFileName, filepath);
                    if (task.Result == 0)
                    {

                    }
                }
            }

            if (File.Exists(tempFilename))
            {
                File.Delete(tempFilename);
            }

            //var jobDocumentList = rpoContext.JobDocuments
            //       .Where(x => x.IdJob == idJob).Take(1).ToList();


            //  var response = Request.CreateResponse<List<JobDocument>>(HttpStatusCode.OK, jobDocumentList);

            var response = Request.CreateResponse(HttpStatusCode.OK, "Permits uploaded successfully.");
            //   string myJsonString = JsonConvert.SerializeObject(response, Formatting.Indented);
            //var ojj=JsonConvert.DeserializeObject<List<JobDocument>>(response);

            return response;

        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                rpoContext.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Jobs the job application work permit exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool JobJobApplicationWorkPermitExists(int id)
        {
            return rpoContext.JobApplicationWorkPermitTypes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Updates the application for.
        /// </summary>
        /// <param name="id">The identifier.</param>
        private void UpdateApplicationFor(int id)
        {
            List<JobApplicationWorkPermitType> jobApplicationWorkPermitTypeList = rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType").Where(c => c.IdJobApplication == id).OrderBy(c => c.Id).ToList();
            string applicationFor = string.Empty;

            foreach (JobApplicationWorkPermitType item in jobApplicationWorkPermitTypeList)
            {
                if (item.JobWorkType != null && item.JobWorkType.Code != null && !string.IsNullOrEmpty(item.JobWorkType.Code))
                {
                    if (string.IsNullOrEmpty(applicationFor))
                    {
                        applicationFor = item.JobWorkType.Code;
                    }
                    else
                    {
                        applicationFor = applicationFor + ", " + item.JobWorkType.Code;
                    }

                }
            }

            JobApplication jobApplicationresponse = rpoContext.JobApplications.Where(x => x.Id == id).FirstOrDefault();
            if (jobApplicationresponse != null)
            {
                jobApplicationresponse.ApplicationFor = applicationFor;
                rpoContext.SaveChanges();
            }
        }

        private void SplitAndSaveInterval(string pdfFilePath, int startPage, int interval, string pdfFileName)
        {
            using (PdfReader reader = new PdfReader(pdfFilePath))
            {
                iTextSharp.text.Document document = new iTextSharp.text.Document();
                PdfCopy copy = new PdfCopy(document, new FileStream(pdfFileName, FileMode.Create));
                document.Open();

                for (int pagenumber = startPage; pagenumber < (startPage + interval); pagenumber++)
                {
                    if (reader.NumberOfPages >= pagenumber)
                    {
                        copy.AddPage(copy.GetImportedPage(reader, pagenumber));
                    }
                    else
                    {
                        break;
                    }
                }
                document.Close();
            }
        }

    }
}