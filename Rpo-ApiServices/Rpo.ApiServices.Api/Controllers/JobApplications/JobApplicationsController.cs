// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-05-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-30-2018
// ***********************************************************************
// <copyright file="JobApplicationsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Applications Controller.</summary>
// ***********************************************************************

/// <summary>
/// The JobApplications namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobApplications
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Enums;
    using Filters;
    using JobApplicationWorkPermits;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Model.Models.Enums;
    using SODA;
    using System.Net;
    using System.Text;

    /// <summary>
    /// Class Job Applications Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class JobApplicationsController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the job applications list.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetJobApplications([FromUri] JobApplicationDataTableParameters dataTableParameters)
        {
            //var jobApplications = rpoContext.JobApplications.Include("JobApplicationType").Include("JobWorkType").Include("ApplicationStatus").Include("CreatedByEmployee")
            //   .Include("LastModifiedByEmployee").AsQueryable();
            var jobApplications = rpoContext.JobApplications.Include("JobApplicationType").Include("ApplicationStatus").Include("CreatedByEmployee")
              .Include("LastModifiedByEmployee").AsQueryable();

            var recordsTotal = jobApplications.Count();
            var recordsFiltered = recordsTotal;

            if (dataTableParameters.IdJobApplicationType == null && dataTableParameters.IdJob != null)
            {
                var application = rpoContext.JobApplications.Include("JobApplicationType").Where(x => x.JobApplicationType != null && x.IdJob == dataTableParameters.IdJob).Distinct().ToList();

                if (application != null)
                {
                    dataTableParameters.IdJobApplicationType = application.Where(d => d.JobApplicationType.IdParent != null).Select(d => d.JobApplicationType.IdParent).FirstOrDefault();
                }
            }

            if (dataTableParameters.IdJob != null)
            {
                jobApplications = jobApplications.Where(c => c.IdJob == dataTableParameters.IdJob);
            }

            if (dataTableParameters.IdJobApplicationType != null)
            {
                jobApplications = jobApplications.Where(c => c.JobApplicationType.IdParent == dataTableParameters.IdJobApplicationType);
            }
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);        

            var result = jobApplications
                        .AsEnumerable()
                        .Select(j => format(j))
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
        /// Gets the job application dropdown.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobapplications/dropdown")]
        public IHttpActionResult GetJobApplicationDropdown(int idJob, int idJobType = 0)
        {
            if (ApplicationType.DOB.GetHashCode() == idJobType)
            {
                var result = this.rpoContext.JobApplications.Include("JobApplicationType").Where(x => x.IdJob == idJob && x.JobApplicationType.IdParent == idJobType).AsEnumerable().Select(c => new
                {
                    Id = c.Id,
                    ItemName = (c.JobApplicationType != null ? c.JobApplicationType.Description : string.Empty) + " " + (c.ApplicationNumber != null ? "(" + c.ApplicationNumber + ")" : string.Empty),
                    ApplicationNumber = c.ApplicationNumber,
                    JobApplicationTypeName = c.JobApplicationType != null ? c.JobApplicationType.Description : string.Empty,
                }).ToArray();

                return this.Ok(result);
            }
            else if (ApplicationType.DOT.GetHashCode() == idJobType)
            {
                var result = this.rpoContext.JobApplications.Include("JobApplicationType").Where(x => x.IdJob == idJob && x.JobApplicationType.IdParent == idJobType).AsEnumerable().Select(c => new
                {
                    Id = c.Id,
                    ItemName = (c.JobApplicationType != null ? c.JobApplicationType.Description + "|" : string.Empty) + c.StreetWorkingOn + "|" + c.StreetFrom + "|" + c.StreetTo,
                    ApplicationNumber = c.ApplicationNumber,
                    JobApplicationTypeName = c.JobApplicationType != null ? c.JobApplicationType.Description : string.Empty,
                }).ToArray();

                return this.Ok(result);
            }
            else if (ApplicationType.DEP.GetHashCode() == idJobType)
            {
                var result = this.rpoContext.JobApplications.Include("JobApplicationType").Where(x => x.IdJob == idJob && x.JobApplicationType.IdParent == idJobType).AsEnumerable().Select(c => new
                {
                    Id = c.Id,
                    ItemName = (c.JobApplicationType != null ? c.JobApplicationType.Description + "|" : string.Empty) + (string.IsNullOrEmpty(c.StreetWorkingOn) ? "-|" : c.StreetWorkingOn + "|") + (string.IsNullOrEmpty(c.StreetFrom) ? "-|" : c.StreetFrom + "|") + (string.IsNullOrEmpty(c.StreetTo) ? "-" : c.StreetTo + "|"),
                    ApplicationNumber = c.ApplicationNumber,
                    JobApplicationTypeName = c.JobApplicationType != null ? c.JobApplicationType.Description : string.Empty,
                }).ToArray();

                return this.Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Formats the specified job application.
        /// </summary>
        /// <param name="jobApplication">The job application.</param>
        /// <returns>JobApplicationDTO.</returns>
        private JobApplicationDTO format(JobApplication jobApplication)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            List<int> IdJobworktype = jobApplication.IdJobWorkType != null && !string.IsNullOrEmpty(jobApplication.IdJobWorkType) ? (jobApplication.IdJobWorkType.Split(',') != null && jobApplication.IdJobWorkType.Split(',').Any() ? jobApplication.IdJobWorkType.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

            var lstJobWorkTypeName = rpoContext.JobWorkTypes.Where(j => IdJobworktype.Contains(j.Id)).Select(j => j.Description);
            var jobWorkTypeName = string.Join(", ", lstJobWorkTypeName);
            return new JobApplicationDTO
            {
                Id = jobApplication.Id,
                IdJob = jobApplication.IdJob,
                ApplicationNumber = jobApplication.ApplicationNumber,
                IdJobApplicationType = jobApplication.IdJobApplicationType,
                JobApplicationTypeName = jobApplication.JobApplicationType != null ? jobApplication.JobApplicationType.Description : string.Empty,
                FloorWorking = jobApplication.FloorWorking,
                ApplicationNote = jobApplication.ApplicationNote,
                Status = jobApplication.ApplicationStatus != null ? jobApplication.ApplicationStatus.DisplayName : string.Empty,
                IdApplicationStatus = jobApplication.IdApplicationStatus,
                ApplicationFor = jobApplication.ApplicationFor,
                CreatedBy = jobApplication.CreatedBy,
                LastModifiedBy = jobApplication.LastModifiedBy != null ? jobApplication.LastModifiedBy : jobApplication.CreatedBy,
                CreatedByEmployeeName = jobApplication.CreatedByEmployee != null ? jobApplication.CreatedByEmployee.FirstName + " " + jobApplication.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = jobApplication.LastModifiedByEmployee != null ? jobApplication.LastModifiedByEmployee.FirstName + " " + jobApplication.LastModifiedByEmployee.LastName : (jobApplication.CreatedByEmployee != null ? jobApplication.CreatedByEmployee.FirstName + " " + jobApplication.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = jobApplication.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplication.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplication.CreatedDate,
                LastModifiedDate = jobApplication.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplication.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (jobApplication.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplication.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplication.CreatedDate),
                StreetWorkingOn = jobApplication.StreetWorkingOn,
                StreetTo = jobApplication.StreetTo,
                StreetFrom = jobApplication.StreetFrom,
                TotalCost = jobApplication.TotalCost,
                TotalDays = jobApplication.TotalDays,
                StartDate = jobApplication.StartDate,
                EndDate = jobApplication.EndDate,
                HydrantCost = jobApplication.HydrantCost,
                WaterCost = jobApplication.WaterCost,
                IsIncludeHoliday = jobApplication.IsIncludeHoliday,
                IsIncludeSaturday = jobApplication.IsIncludeSaturday,
                IsIncludeSunday = jobApplication.IsIncludeSunday,
                Description = jobApplication.Description,
                Purpose = jobApplication.Purpose,
                ModelNumber = jobApplication.ModelNumber,
                Manufacturer = jobApplication.Manufacturer,
                SerialNumber = jobApplication.SerialNumber,
                JobApplicationStatus = jobApplication.JobApplicationStatus,
                SignOff = jobApplication.SignOff,                
                IdJobWorkType = jobApplication.IdJobWorkType,
                JobWorkTypeName = jobWorkTypeName
              
            };
        }

        private JobApplicationDTO FormatDetails(JobApplication jobApplication)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new JobApplicationDTO
            {
                Id = jobApplication.Id,
                IdJob = jobApplication.IdJob,
                ApplicationNumber = jobApplication.ApplicationNumber,
                IdJobApplicationType = jobApplication.IdJobApplicationType,
                JobApplicationTypeName = jobApplication.JobApplicationType != null ? jobApplication.JobApplicationType.Description : string.Empty,
                FloorWorking = jobApplication.FloorWorking,
                ApplicationNote = jobApplication.ApplicationNote,
                Status = jobApplication.ApplicationStatus != null ? jobApplication.ApplicationStatus.DisplayName : string.Empty,
                IdApplicationStatus = jobApplication.IdApplicationStatus,
                ApplicationFor = jobApplication.ApplicationFor,
                CreatedBy = jobApplication.CreatedBy,
                LastModifiedBy = jobApplication.LastModifiedBy != null ? jobApplication.LastModifiedBy : jobApplication.CreatedBy,
                CreatedByEmployeeName = jobApplication.CreatedByEmployee != null ? jobApplication.CreatedByEmployee.FirstName + " " + jobApplication.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = jobApplication.LastModifiedByEmployee != null ? jobApplication.LastModifiedByEmployee.FirstName + " " + jobApplication.LastModifiedByEmployee.LastName : (jobApplication.CreatedByEmployee != null ? jobApplication.CreatedByEmployee.FirstName + " " + jobApplication.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = jobApplication.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplication.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplication.CreatedDate,
                LastModifiedDate = jobApplication.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplication.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (jobApplication.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplication.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplication.CreatedDate),
                StreetWorkingOn = jobApplication.StreetWorkingOn,
                StreetTo = jobApplication.StreetTo,
                StreetFrom = jobApplication.StreetFrom,
                TotalCost = jobApplication.TotalCost,
                TotalDays = jobApplication.TotalDays,
                StartDate = jobApplication.StartDate,
                EndDate = jobApplication.EndDate,
                HydrantCost = jobApplication.HydrantCost,
                WaterCost = jobApplication.WaterCost,
                IsIncludeHoliday = jobApplication.IsIncludeHoliday,
                IsIncludeSaturday = jobApplication.IsIncludeSaturday,
                IsIncludeSunday = jobApplication.IsIncludeSunday,
                Description = jobApplication.Description,
                Purpose = jobApplication.Purpose,
                ModelNumber = jobApplication.ModelNumber,
                Manufacturer = jobApplication.Manufacturer,
                SerialNumber = jobApplication.SerialNumber,
                JobApplicationStatus = jobApplication.JobApplicationStatus,
                SignOff = jobApplication.SignOff,               
                IsHighRise = jobApplication.IsHighRise,
                IdJobWorkType = jobApplication.IdJobWorkType,              
                JobWorkPermitHistories = jobApplication.JobWorkPermitHistories != null ? jobApplication.JobWorkPermitHistories.OrderByDescending(x => x.CreatedDate).Select(x => new JobWorkPermitHistoryDTO
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
            };
        }


        /// <summary>
        /// Gets the job application.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobApplicationDTO))]
        public IHttpActionResult GetJobApplication(int id)
        {
            JobApplication jobApplicationresponse = rpoContext.JobApplications.Include("JobApplicationType")                
                 //.Include("JobWorkType")
                 .Include("ApplicationStatus")
                 .Include("JobWorkPermitHistories")
                 .Include("LastModifiedByEmployee")
                 .Include("CreatedByEmployee")
                 .FirstOrDefault(r => r.Id == id);

            if (jobApplicationresponse == null)
            {
                return this.NotFound();
            }

            return Ok(FormatDetails(jobApplicationresponse));
        }

        /// <summary>
        /// Puts the job application.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="jobApplicationDTO">The job application dto.</param>
        /// <returns>IHttpActionResult.</returns>
        /// <returns>To check the DOB application number and DOT location number. The DOt have to check the unique location number in specific job.</returns>
        /// <remarks>Duplicate location allowed in different job.</remarks>
        /// <exception cref="RpoBusinessException">
        /// Application# required.
        /// or
        /// Application# already exists.
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobApplication))]
        public IHttpActionResult PutJobApplication(int id, JobApplicationDTO jobApplicationDTO)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddApplicationsWorkPermits))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != jobApplicationDTO.Id)
                {
                    return this.NotFound();
                }

                if (jobApplicationDTO.IdJobApplicationType != null && jobApplicationDTO.IdJobApplicationType > 0)
                {
                    JobApplicationType jobApplicationType = rpoContext.JobApplicationTypes.FirstOrDefault(x => x.Id == jobApplicationDTO.IdJobApplicationType);
                    ApplicationStatus applicationStatus = rpoContext.ApplicationStatus.FirstOrDefault(x => x.Id == jobApplicationDTO.IdApplicationStatus);

                    JobApplication jobApplication = rpoContext.JobApplications.Include("JobApplicationType")
                        .Include("ApplicationStatus")
                        .Include("Job")
                        .FirstOrDefault(r => r.Id == id);

                    if (jobApplication == null)
                    {
                        return this.NotFound();
                    }

                    string oldApplicationStatus = jobApplication.ApplicationStatus != null ? jobApplication.ApplicationStatus.Name : String.Empty;

                    string PreviousApplicationNumber = jobApplication.ApplicationNumber;
                    string PreviousFloorWorking = jobApplication.FloorWorking;
                    string applicationNumber = jobApplicationDTO.ApplicationNumber;
                    string oldLocationDetails = jobApplication.StreetWorkingOn + " | " + jobApplication.StreetFrom + " | " + jobApplication.StreetTo;
                    string newLocationDetails = jobApplicationDTO.StreetWorkingOn + " | " + jobApplicationDTO.StreetFrom + " | " + jobApplicationDTO.StreetTo;                  

                    if (Convert.ToInt32(jobApplicationType.IdParent) == ApplicationType.DOB.GetHashCode())
                    {
                        if (!JobApplicationNumberValid(jobApplication.ApplicationNumber, jobApplicationDTO.ApplicationNumber))
                        {
                            throw new RpoBusinessException(StaticMessages.ApplicationNumberRequired);
                        }
                    }
                    else if (Convert.ToInt32(jobApplicationType.IdParent) == ApplicationType.DOT.GetHashCode())
                    {
                        if (!string.IsNullOrEmpty(jobApplicationDTO.ApplicationNumber) && PreviousApplicationNumber != jobApplication.ApplicationNumber)
                        {
                            List<JobApplication> jobApplications = JobTrackingNumberExists(jobApplicationDTO.ApplicationNumber, jobApplication.Id, jobApplication.IdJob, Convert.ToInt32(jobApplicationType.IdParent), jobApplication.StreetWorkingOn, jobApplication.StreetFrom, jobApplication.StreetTo);
                            if (jobApplications != null && jobApplications.Count > 0)
                            {
                                throw new RpoBusinessException(string.Format(StaticMessages.TrackingNumberAleadyExists, (jobApplications[0].Job != null ? jobApplications[0].Job.JobNumber : string.Empty)));
                            }
                        }

                        if (JobApplicationLocationExists(jobApplication.StreetWorkingOn, jobApplication.StreetFrom, jobApplication.StreetTo, jobApplication.Id, jobApplication.IdJob, Convert.ToInt32(jobApplicationType.IdParent), Convert.ToInt32(jobApplication.IdJobApplicationType)))
                        {
                            throw new RpoBusinessException(StaticMessages.JobApplicationLocationAleadyExists);
                        }
                    }


                    jobApplication.ApplicationNumber = jobApplicationDTO.ApplicationNumber;
                    jobApplication.IdApplicationStatus = jobApplicationDTO.IdApplicationStatus;
                    jobApplication.IdJobApplicationType = jobApplicationDTO.IdJobApplicationType;
                    jobApplication.FloorWorking = jobApplicationDTO.FloorWorking;
                    jobApplication.StreetWorkingOn = jobApplicationDTO.StreetWorkingOn;
                    jobApplication.StreetFrom = jobApplicationDTO.StreetFrom;
                    jobApplication.StreetTo = jobApplicationDTO.StreetTo;
                    jobApplication.ApplicationNote = jobApplicationDTO.ApplicationNote;
                    jobApplication.LastModifiedDate = DateTime.UtcNow;
                    jobApplication.Job.LastModiefiedDate = DateTime.UtcNow;
                    jobApplication.TotalCost = jobApplicationDTO.TotalCost;
                    jobApplication.TotalDays = jobApplicationDTO.TotalDays;
                    jobApplication.StartDate = jobApplicationDTO.StartDate;
                    jobApplication.EndDate = jobApplicationDTO.EndDate;
                    jobApplication.HydrantCost = jobApplicationDTO.HydrantCost;
                    jobApplication.WaterCost = jobApplicationDTO.WaterCost;
                    jobApplication.IsIncludeHoliday = jobApplicationDTO.IsIncludeHoliday;
                    jobApplication.IsIncludeSaturday = jobApplicationDTO.IsIncludeSaturday;
                    jobApplication.IsIncludeSunday = jobApplicationDTO.IsIncludeSunday;
                    jobApplication.Description = jobApplicationDTO.Description;
                    jobApplication.Purpose = jobApplicationDTO.Purpose;
                    jobApplication.ModelNumber = jobApplicationDTO.ModelNumber;
                    jobApplication.Manufacturer = jobApplicationDTO.Manufacturer;
                    jobApplication.SerialNumber = jobApplicationDTO.SerialNumber;
                    jobApplication.JobApplicationStatus = jobApplicationDTO.JobApplicationStatus;
                    jobApplication.SignOff = jobApplicationDTO.SignOff;
                    jobApplication.IsHighRise = jobApplicationDTO.IsHighRise;
                    jobApplication.IdJobWorkType = jobApplicationDTO.IdJobWorkType;
                    if (employee != null)
                    {
                        jobApplication.LastModifiedBy = employee.Id;
                        jobApplication.Job.LastModifiedBy = employee.Id;
                    }

                    if (Convert.ToInt32(jobApplicationType.IdParent) == ApplicationType.DOB.GetHashCode())
                    {
                        if (!string.IsNullOrEmpty(jobApplication.ApplicationNumber))
                        {
                            List<JobApplication> jobApplications = JobApplicationNumberExists(jobApplication.ApplicationNumber, jobApplication.Id, jobApplication.IdJob, Convert.ToInt32(jobApplicationType.IdParent));
                            if (jobApplications != null && jobApplications.Count > 0)
                            {
                                throw new RpoBusinessException(string.Format(StaticMessages.ApplicationNumberAleadyExists, (jobApplications[0].Job != null ? jobApplications[0].Job.JobNumber : string.Empty)));
                            }
                        }
                    }
                    else if (Convert.ToInt32(jobApplicationType.IdParent) == ApplicationType.DOT.GetHashCode())
                    {
                        if (PreviousApplicationNumber != applicationNumber)
                        {
                            Common.SaveJobWorkPermitHistory(null, jobApplication.Id, PreviousApplicationNumber, applicationNumber, employee.Id);
                        }

                    }

                    string jobApplicationTypeName = string.Empty;
                    if (jobApplication.JobApplicationType != null)
                    {
                        jobApplicationTypeName = Convert.ToString(jobApplication.JobApplicationType.Description);
                    }

                    string newApplicationStatus = applicationStatus != null ? applicationStatus.Name : string.Empty;
                    switch ((ApplicationType)Convert.ToInt32(jobApplicationType.IdParent))
                    {
                        case ApplicationType.DOB:
                            //if (jobApplication.ApplicationNumber != applicationNumber)
                            //{
                            string editApplicationNumber_DOB = JobHistoryMessages.EditApplicationNumber_DOB
                                                   .Replace("##ApplicationType##", jobApplicationTypeName != null ? jobApplicationTypeName : JobHistoryMessages.NoSetstring)
                                                   .Replace("##ApplicationStatus##", !string.IsNullOrEmpty(jobApplication.JobApplicationStatus) ? jobApplication.JobApplicationStatus : JobHistoryMessages.NoSetstring)
                                                   .Replace("##ApplicationNumber##", !string.IsNullOrEmpty(jobApplication.ApplicationNumber) ? jobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);

                            Common.SaveJobHistory(employee.Id, jobApplication.IdJob, editApplicationNumber_DOB, JobHistoryType.Applications);
                            //}

                            //if (oldApplicationStatus != newApplicationStatus)
                            //{
                            //    string editApplicationStatus_DOB = JobHistoryMessages.EditApplicationStatus_DOB
                            //                           .Replace("##ApplicationType##", jobApplicationTypeName != null ? jobApplicationTypeName : JobHistoryMessages.NoSetstring)
                            //                           .Replace("##ApplicationStatus##", !string.IsNullOrEmpty(jobApplication.JobApplicationStatus) ? jobApplication.JobApplicationStatus : JobHistoryMessages.NoSetstring)
                            //                           .Replace("##ApplicationNumber##", !string.IsNullOrEmpty(jobApplication.ApplicationNumber) ? jobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);

                            //    Common.SaveJobHistory(employee.Id, jobApplication.IdJob, editApplicationStatus_DOB, JobHistoryType.Applications);
                            //}

                            break;
                        case ApplicationType.DOT:


                            // if (oldLocationDetails != newLocationDetails)
                            //  {
                            //string editApplicationLocation_DOT = JobHistoryMessages.EditApplicationLocation_DOT
                            //                        .Replace("##ApplicationNumber##", jobApplication.ApplicationNumber)
                            //                        .Replace("##ApplicationType##", jobApplicationTypeName)
                            //                        .Replace("##ApplicationStatus##", newApplicationStatus)
                            //                        .Replace("##OldLocationDetails##", oldLocationDetails)
                            //                        .Replace("##NewLocationDetails##", newLocationDetails);
                            string editApplicationLocation_DOT = JobHistoryMessages.EditApplicationLocation_DOT
                                                  .Replace("##ApplicationType##", jobApplicationType != null ? jobApplicationType.Description : JobHistoryMessages.NoSetstring)
                                                  .Replace("##ApplicationStatus##", applicationStatus != null ? applicationStatus.Name : JobHistoryMessages.NoSetstring)
                                                  .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) ? jobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetFrom) ? jobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetTo) ? jobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                  .Replace("##Tracking##", !string.IsNullOrEmpty(jobApplication.ApplicationNumber) ? jobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);


                            Common.SaveJobHistory(employee.Id, jobApplication.IdJob, editApplicationLocation_DOT, JobHistoryType.Applications);
                            //  }


                            //if (oldApplicationStatus != newApplicationStatus)
                            //{
                            //    string editLocationStatus_DOT = JobHistoryMessages.EditLocationStatus_DOT
                            //                                                            .Replace("##ApplicationType##", jobApplicationTypeName)
                            //                                                            .Replace("##ApplicationStatus##", newApplicationStatus)
                            //                                                            .Replace("##ApplicationNumber##", jobApplication.ApplicationNumber)
                            //                                                            .Replace("##LocationDetails##", newLocationDetails);

                            //    Common.SaveJobHistory(employee.Id, jobApplication.IdJob, editLocationStatus_DOT, JobHistoryType.Applications);
                            //}
                            if (newApplicationStatus.ToLower().Trim() == "completed")
                            {
                                List<JobApplicationWorkPermitType> jobApplicationWorkPermitTypes = rpoContext.JobApplicationWorkPermitTypes.Where(d => d.IdJobApplication == jobApplication.Id).ToList();

                                if (jobApplicationWorkPermitTypes.Count > 0)
                                {
                                    jobApplicationWorkPermitTypes.ForEach(a => a.IsCompleted = true);
                                    rpoContext.SaveChanges();

                                    foreach (var item in jobApplicationWorkPermitTypes)
                                    {
                                        string CompleteWorkPermit_DOT = JobHistoryMessages.CompleteWorkPermit_DOT
                                                 .Replace("##PermitNumber##", !string.IsNullOrEmpty(item.PermitNumber) ? item.PermitNumber : JobHistoryMessages.NoSetstring)
                                                 .Replace("##PermitType##", !string.IsNullOrEmpty(item.PermitType) ? item.PermitType : JobHistoryMessages.NoSetstring)
                                                 .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) ? jobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetFrom) ? jobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetTo) ? jobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                 .Replace("##ApplicationNumber##", !string.IsNullOrEmpty(jobApplication.ApplicationNumber) ? jobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring)
                                                 .Replace("##ApplicationStatus##", applicationStatus != null ? applicationStatus.Name : JobHistoryMessages.NoSetstring);
                                        Common.SaveJobHistory(employee.Id, jobApplication.IdJob, CompleteWorkPermit_DOT, JobHistoryType.WorkPermits);
                                    }
                                }
                            }

                            break;
                        case ApplicationType.DEP:

                            //if (oldApplicationStatus != newApplicationStatus)
                            //{
                            string editApplicationStatus_DEP = JobHistoryMessages.EditApplication_DEP
                                                     .Replace("##ApplicationType##", jobApplicationType != null ? jobApplicationType.Description : JobHistoryMessages.NoSetstring)
                                                     .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) ? jobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetFrom) ? jobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetTo) ? jobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                    .Replace("##ApplicationStatus##", applicationStatus != null ? applicationStatus.Name : JobHistoryMessages.NoSetstring)
                                                    .Replace("##Description##", !string.IsNullOrEmpty(jobApplication.Description) ? jobApplication.Description : JobHistoryMessages.NoSetstring);

                            Common.SaveJobHistory(employee.Id, jobApplication.IdJob, editApplicationStatus_DEP, JobHistoryType.Applications);
                            //}
                            break;
                    }

                    //Common.SaveJobHistory(employee.Id, jobApplication.IdJob, string.Format(JobHistoryMessages.EditApplicationMessage, jobApplicationTypeName), JobHistoryType.Applications);

                    try
                    {
                        rpoContext.SaveChanges();
                        if (PreviousApplicationNumber != applicationNumber || PreviousFloorWorking!=jobApplicationDTO.FloorWorking)
                        {
                            var jobChecklistHeader = rpoContext.JobChecklistHeaders.Include("JobApplicationWorkPermitTypes").Where(x => x.IdJobApplication == jobApplicationDTO.Id).ToList();
                            foreach (var j in jobChecklistHeader)
                            {
                                string checklistname = j.ChecklistName;
                                //  var s = rpoContext.JobApplications.Where(x => x.Id == jobChecklistHeaderDTO.IdJobApplication).FirstOrDefault();
                                var applicationtype = rpoContext.JobApplicationTypes.Where(x => x.Id == jobApplicationDTO.IdJobApplicationType).Select(x => x.Description).FirstOrDefault();
                                var applicationnumber = jobApplicationDTO.ApplicationNumber;
                                var abc = jobChecklistHeader.AsEnumerable();
                                var worktype = j.JobApplicationWorkPermitTypes.Select(x => x.Code);
                                string applicationstatus = jobApplicationDTO.JobApplicationStatus;
                                StringBuilder strworktype = new StringBuilder();
                                foreach (var d in worktype)
                                {
                                    strworktype.Append(d + ", ");
                                }
                              //  string title = applicationtype + (applicationnumber != null ? " - " + applicationnumber : string.Empty) + " - " + strworktype.Remove(strworktype.Length - 2, 2) + (applicationstatus != null ? " - " + applicationstatus : string.Empty) + (jobApplicationDTO.FloorWorking != null ? " - " + jobApplicationDTO.FloorWorking : string.Empty);
                                string title = applicationtype + (applicationnumber != null ? " - " + applicationnumber : string.Empty) + (strworktype != null ? " - " + strworktype.Remove(strworktype.Length - 2, 2) : string.Empty) + (applicationstatus != null ? " - " + applicationstatus : string.Empty) + (jobApplicationDTO.FloorWorking != null ? " - " + jobApplicationDTO.FloorWorking : string.Empty);

                                j.ChecklistName = title;
                                var compositeParents = rpoContext.CompositeChecklistDetails.Where(x => x.IsParentCheckList == true && x.IdJobChecklistHeader == j.IdJobCheckListHeader).Select(y => y.IdCompositeChecklist).ToList().Distinct();
                                foreach (var c in compositeParents)
                                {
                                    var composite = rpoContext.CompositeChecklists.Where(x => x.Id == c).FirstOrDefault();
                                    composite.Name = "CC - " + title;
                                }
                                rpoContext.SaveChanges();
                            }
                        }

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!JobApplicationExists(id))
                        {
                            return this.NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }


                    return Ok(format(jobApplication));
                }
                else
                {
                    throw new RpoBusinessException(StaticMessages.ApplicationTypeRequired);
                }
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the job application.
        /// </summary>
        /// <param name="jobApplication">The job application.</param>
        /// <returns>IHttpActionResult.</returns>
        /// <exception cref="RpoBusinessException">Application# already exists.</exception>
        /// <remark>To check the DOB application number and DOT location number. The DOt have to check the unique location number in specific job.</remark>
        /// <remarks>Duplicate location allowed in different job.</remarks>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobApplication))]
        public IHttpActionResult PostJobApplication(JobApplication jobApplication)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddApplicationsWorkPermits))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (jobApplication.IdJobApplicationType != null && jobApplication.IdJobApplicationType > 0)
                {
                    JobApplicationType jobApplicationType = rpoContext.JobApplicationTypes.FirstOrDefault(x => x.Id == jobApplication.IdJobApplicationType);
                    if (Convert.ToInt32(jobApplicationType.IdParent) == ApplicationType.DOB.GetHashCode())
                    {
                        if (!string.IsNullOrEmpty(jobApplication.ApplicationNumber))
                        {
                            List<JobApplication> jobApplications = JobApplicationNumberExists(jobApplication.ApplicationNumber, jobApplication.Id, jobApplication.IdJob, Convert.ToInt32(jobApplicationType.IdParent));
                            if (jobApplications != null && jobApplications.Count > 0)
                            {
                                throw new RpoBusinessException(string.Format(StaticMessages.ApplicationNumberAleadyExists, (jobApplications[0].Job != null ? jobApplications[0].Job.JobNumber : string.Empty)));
                            }
                        }
                    }
                    else if (Convert.ToInt32(jobApplicationType.IdParent) == ApplicationType.DOT.GetHashCode())
                    {
                        if (!string.IsNullOrEmpty(jobApplication.ApplicationNumber))
                        {
                            List<JobApplication> jobApplications = JobTrackingNumberExists(jobApplication.ApplicationNumber, jobApplication.Id, jobApplication.IdJob, Convert.ToInt32(jobApplicationType.IdParent), jobApplication.StreetWorkingOn, jobApplication.StreetFrom, jobApplication.StreetTo);
                            if (jobApplications != null && jobApplications.Count > 0)
                            {
                                throw new RpoBusinessException(string.Format(StaticMessages.TrackingNumberAleadyExists, (jobApplications[0].Job != null ? jobApplications[0].Job.JobNumber : string.Empty)));
                            }
                        }

                        if (JobApplicationLocationExists(jobApplication.StreetWorkingOn, jobApplication.StreetFrom, jobApplication.StreetTo, jobApplication.Id, jobApplication.IdJob, Convert.ToInt32(jobApplicationType.IdParent), Convert.ToInt32(jobApplication.IdJobApplicationType)))
                        {
                            throw new RpoBusinessException(StaticMessages.JobApplicationLocationAleadyExists);
                        }
                    }
                    //jobApplication.SignOff = jobApplication.SignOff;
                    //jobApplication.IdJobWorkType = jobApplication.IdJobWorkType;
                    jobApplication.LastModifiedDate = DateTime.UtcNow;
                    jobApplication.CreatedDate = DateTime.UtcNow;
                    if (employee != null)
                    {
                        jobApplication.CreatedBy = employee.Id;
                    }
                   
                    //if (jobApplication.JobWorkTypes != null)
                    //{
                    //    foreach (var jobworkType in jobApplication.JobWorkTypes)
                    //    {
                    //        jobApplication.JobWorkTypes.Add(rpoContext.JobWorkTypes.Find(jobworkType.Id));
                    //    }
                    //}
                 
                    //Removed as client dont want job worktype in appliction popup
                   // jobApplication.IdJobWorkType = jobApplication.IdJobWorkType;
                    rpoContext.JobApplications.Add(jobApplication);
                    rpoContext.SaveChanges();

                    if (Convert.ToInt32(jobApplicationType.IdParent) == ApplicationType.DEP.GetHashCode()
                        && !string.IsNullOrEmpty(jobApplicationType.Description) &&
                        (jobApplicationType.Description.ToLower() == "hydrant"
                        || jobApplicationType.Description.ToLower() == "boiler"))
                    {
                        string forDescription = string.Empty;
                        List<JobWorkType> jobWorkType = rpoContext.JobWorkTypes.Where(x => x.IdJobApplicationType == jobApplicationType.Id).ToList();
                        foreach (var item in jobWorkType)
                        {
                            JobApplicationWorkPermitType jobApplicationWorkPermitType = new JobApplicationWorkPermitType();
                            jobApplicationWorkPermitType.IdJobApplication = jobApplication.Id;
                            jobApplicationWorkPermitType.EstimatedCost = jobApplication.TotalCost;
                            jobApplicationWorkPermitType.Code = item.Code;
                            jobApplicationWorkPermitType.IdJobWorkType = item.Id;
                            jobApplicationWorkPermitType.WorkDescription = item.Content;
                            rpoContext.JobApplicationWorkPermitTypes.Add(jobApplicationWorkPermitType);

                            if (!string.IsNullOrEmpty(jobApplicationWorkPermitType.Code))
                            {
                                if (!string.IsNullOrEmpty(forDescription))
                                {
                                    forDescription = forDescription + ", " + jobApplicationWorkPermitType.Code;
                                }
                                else
                                {
                                    forDescription = jobApplicationWorkPermitType.Code;
                                }
                            }
                            rpoContext.SaveChanges();

                            JobApplicationWorkPermitType jobApplicationWorkPermitResponse = rpoContext.JobApplicationWorkPermitTypes.Include("JobApplication.JobApplicationType")
                   .Include("JobApplication.ApplicationStatus")
                  .Include("JobWorkType")
                  .Include("ContactResponsible")
                  .FirstOrDefault(r => r.Id == jobApplicationWorkPermitType.Id);

                            string addWorkPermit_DEP = JobHistoryMessages.AddWorkPermit_DEP
                                                       .Replace("##PermitNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermitResponse.PermitNumber) ? jobApplicationWorkPermitResponse.PermitNumber : JobHistoryMessages.NoSetstring)
                                                       .Replace("##PermitType##", jobApplicationWorkPermitResponse.JobWorkType != null ? jobApplicationWorkPermitResponse.JobWorkType.Description : JobHistoryMessages.NoSetstring)
                                                       .Replace("##ApplicationType##", !string.IsNullOrEmpty(jobApplicationType.Description) ? jobApplicationType.Description : JobHistoryMessages.NoSetstring)
                                                       .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) ? jobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetFrom) ? jobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetTo) ? jobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                       .Replace("##PermitIssued##", jobApplicationWorkPermitResponse.Issued != null ? jobApplicationWorkPermitResponse.Issued.Value.ToShortDateString() : JobHistoryMessages.NoSetstring)
                                                       .Replace("##PermitExpiry##", jobApplicationWorkPermitResponse.Expires != null ? jobApplicationWorkPermitResponse.Expires.Value.ToShortDateString() : JobHistoryMessages.NoSetstring);

                            Common.SaveJobHistory(employee.Id, jobApplication.IdJob, addWorkPermit_DEP, JobHistoryType.WorkPermits);

                        }

                        jobApplication.ApplicationFor = !string.IsNullOrEmpty(forDescription) ? forDescription.Trim(',') : string.Empty;
                        rpoContext.SaveChanges();
                    }

                    Common.UpdateJobLastModifiedDate(jobApplication.IdJob, employee.Id);

                    ApplicationStatus applicationStatus = rpoContext.ApplicationStatus.FirstOrDefault(x => x.Id == jobApplication.IdApplicationStatus);
                    switch ((ApplicationType)Convert.ToInt32(jobApplicationType.IdParent))
                    {
                        case ApplicationType.DOB:
                            string addApplication_DOB = JobHistoryMessages.AddApplication_DOB
                                                        .Replace("##ApplicationType##", jobApplicationType != null ? jobApplicationType.Description : JobHistoryMessages.NoSetstring)
                                                        .Replace("##ApplicationStatus##", !string.IsNullOrEmpty(jobApplication.JobApplicationStatus) ? jobApplication.JobApplicationStatus : JobHistoryMessages.NoSetstring)
                                                        .Replace("##ApplicationNumber##", !string.IsNullOrEmpty(jobApplication.ApplicationNumber) ? jobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);
                            Common.SaveJobHistory(employee.Id, jobApplication.IdJob, addApplication_DOB, JobHistoryType.Applications);
                            break;
                        case ApplicationType.DOT:
                            string addApplication_DOT = JobHistoryMessages.AddApplication_DOT
                                                        .Replace("##ApplicationType##", jobApplicationType != null ? jobApplicationType.Description : JobHistoryMessages.NoSetstring)
                                                        .Replace("##ApplicationStatus##", applicationStatus != null && !string.IsNullOrEmpty(applicationStatus.Name) ? applicationStatus.Name : JobHistoryMessages.NoSetstring)
                                                        .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) ? jobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetFrom) ? jobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetTo) ? jobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                        .Replace("##Tracking##", !string.IsNullOrEmpty(jobApplication.ApplicationNumber) ? jobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);
                            Common.SaveJobHistory(employee.Id, jobApplication.IdJob, addApplication_DOT, JobHistoryType.Applications);
                            break;
                        case ApplicationType.DEP:
                            string addApplication_DEP = JobHistoryMessages.AddApplication_DEP
                                                        .Replace("##ApplicationType##", jobApplicationType != null ? jobApplicationType.Description : JobHistoryMessages.NoSetstring)
                                                        .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) ? jobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetFrom) ? jobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetTo) ? jobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                        .Replace("##ApplicationStatus##", applicationStatus != null && !string.IsNullOrEmpty(applicationStatus.Name) ? applicationStatus.Name : JobHistoryMessages.NoSetstring)
                                                        .Replace("##Description##", jobApplication.Description != null && !string.IsNullOrEmpty(jobApplication.Description) ? jobApplication.Description : JobHistoryMessages.NoSetstring);
                            Common.SaveJobHistory(employee.Id, jobApplication.IdJob, addApplication_DEP, JobHistoryType.Applications);
                            break;
                        default:
                            break;
                    }

                    return Ok(format(jobApplication));
                }
                else
                {
                    throw new RpoBusinessException(StaticMessages.ApplicationTypeRequired);
                }
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the job application.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        /// <remarks>unable to delete the application/Location which have already assign the permit</remarks>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobApplicationType))]
        public IHttpActionResult DeleteJobApplication(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteApplicationsWorkPermits))
            {
                JobApplication jobApplication = rpoContext.JobApplications.Include("JobApplicationType")
                                                .Include("ApplicationStatus").Include("Job").Include("JobWorkPermitHistories")
                                                .FirstOrDefault(r => r.Id == id);

                if (jobApplication == null)
                {
                    return this.NotFound();
                }

                jobApplication.Job.LastModiefiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobApplication.Job.LastModifiedBy = employee.Id;
                }

                string jobApplicationTypeName = string.Empty;
                if (jobApplication.JobApplicationType != null)
                {
                    jobApplicationTypeName = Convert.ToString(jobApplication.JobApplicationType.Description);
                }

                int jobApplicationCount = rpoContext.JobApplications.Where(x => x.JobApplicationType.IdParent == jobApplication.JobApplicationType.IdParent && x.IdJob == jobApplication.IdJob).Count();
                string deleteApplication_DOB = string.Empty;
                string deleteApplication_DOT = string.Empty;
                string deleteApplication_DEP = string.Empty;

                ApplicationStatus applicationStatus = rpoContext.ApplicationStatus.FirstOrDefault(x => x.Id == jobApplication.IdApplicationStatus);
                JobApplicationType jobApplicationType = jobApplication.JobApplicationType;
                switch ((ApplicationType)Convert.ToInt32(jobApplicationType.IdParent))
                {
                    case ApplicationType.DOB:
                        deleteApplication_DOB = JobHistoryMessages.DeleteApplication_DOB
                                                    .Replace("##ApplicationType##", jobApplicationTypeName != null ? jobApplicationTypeName : JobHistoryMessages.NoSetstring)
                                                    .Replace("##ApplicationNumber##", !string.IsNullOrEmpty(jobApplication.ApplicationNumber) ? jobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);
                        //  Common.SaveJobHistory(employee.Id, jobApplication.IdJob, deleteApplication_DOB, JobHistoryType.Applications);
                        break;
                    case ApplicationType.DOT:
                        //string deleteApplication_DOT = JobHistoryMessages.DeleteApplication_DOT
                        //                            .Replace("##ApplicationType##", jobApplicationType != null ? jobApplicationType.Description : string.Empty)
                        //                            .Replace("##ApplicationNumber##", jobApplication.ApplicationNumber)
                        //                            .Replace("##LocationDetails##", jobApplication.StreetWorkingOn + " | " + jobApplication.StreetFrom + " | " + jobApplication.StreetTo);
                        deleteApplication_DOT = JobHistoryMessages.DeleteApplication_DOT
                                                      .Replace("##ApplicationType##", jobApplicationType != null ? jobApplicationType.Description : JobHistoryMessages.NoSetstring)
                                                     .Replace("##ApplicationStatus##", applicationStatus != null ? applicationStatus.Name : JobHistoryMessages.NoSetstring)
                                                      .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) ? jobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetFrom) ? jobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetTo) ? jobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                      .Replace("##Tracking##", !string.IsNullOrEmpty(jobApplication.ApplicationNumber) ? jobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);
                        //  Common.SaveJobHistory(employee.Id, jobApplication.IdJob, deleteApplication_DOT, JobHistoryType.Applications);
                        break;
                    case ApplicationType.DEP:
                        deleteApplication_DEP = JobHistoryMessages.DeleteApplication_DEP
                                                    .Replace("##ApplicationType##", jobApplicationType != null ? jobApplicationType.Description : JobHistoryMessages.NoSetstring)
                                                    .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) ? jobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetFrom) ? jobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetTo) ? jobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring);
                        // Common.SaveJobHistory(employee.Id, jobApplication.IdJob, deleteApplication_DEP, JobHistoryType.Applications);
                        break;
                    default:
                        break;
                }

                rpoContext.JobApplications.Remove(jobApplication);
                rpoContext.SaveChanges();
                switch ((ApplicationType)Convert.ToInt32(jobApplicationType.IdParent))
                {
                    case ApplicationType.DOB:
                        Common.SaveJobHistory(employee.Id, jobApplication.IdJob, deleteApplication_DOB, JobHistoryType.Applications);
                        break;
                    case ApplicationType.DOT:
                        Common.SaveJobHistory(employee.Id, jobApplication.IdJob, deleteApplication_DOT, JobHistoryType.Applications);
                        break;
                    case ApplicationType.DEP:
                        Common.SaveJobHistory(employee.Id, jobApplication.IdJob, deleteApplication_DEP, JobHistoryType.Applications);
                        break;
                    default:
                        break;
                }

                return Ok(new
                {
                    Id = jobApplication.Id,
                    JobApplicationCount = jobApplicationCount - 1,
                    IdJob = jobApplication.IdJob,
                    ApplicationNumber = jobApplication.ApplicationNumber,
                    IdJobApplicationType = jobApplication.IdJobApplicationType,
                    JobApplicationTypeName = jobApplication.JobApplicationType != null ? jobApplication.JobApplicationType.Description : string.Empty,
                    FloorWorking = jobApplication.FloorWorking,
                    ApplicationNote = jobApplication.ApplicationNote,
                    Status = jobApplication.ApplicationStatus != null && !string.IsNullOrEmpty(jobApplication.ApplicationStatus.Name) ? jobApplication.ApplicationStatus.Name : string.Empty,
                    IdApplicationStatus = jobApplication.IdApplicationStatus,
                    ApplicationFor = jobApplication.ApplicationFor,
                    StreetWorkingOn = jobApplication.StreetWorkingOn,
                    StreetTo = jobApplication.StreetTo,
                    StreetFrom = jobApplication.StreetFrom
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
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
        /// Jobs the application exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool JobApplicationExists(int id)
        {
            return rpoContext.JobApplications.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Jobs the application number exists.
        /// </summary>
        /// <param name="applicationNumber">The application number.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private List<JobApplication> JobApplicationNumberExists(string applicationNumber, int id, int idjob, int idJobApplicationTypeParent)
        {
            var jobApplications = rpoContext.JobApplications.Include("Job").Include("JobApplicationType").Where(e => e.ApplicationNumber == applicationNumber && e.Id != id && e.IdJob == idjob && e.JobApplicationType.IdParent == idJobApplicationTypeParent).ToList();
            return jobApplications;
        }
        /// <summary>
        /// Jobs the application number exists.
        /// </summary>
        /// <param name="trackingNumber">The application number.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private List<JobApplication> JobTrackingNumberExists(string trackingNumber, int id, int idJob, int idJobApplicationTypeParent, string streetWorkingOn, string streetWorkingFrom, string streetWorkingTo)
        {
            var jobApplications = rpoContext.JobApplications.Include("Job").Include("JobWorkPermitHistories").Include("JobApplicationType").Where(e => (e.ApplicationNumber == trackingNumber || e.JobWorkPermitHistories.Any(p => p.NewNumber == trackingNumber || p.OldNumber == trackingNumber)) && e.StreetWorkingOn.ToLower().Contains(streetWorkingOn.ToLower()) && e.StreetFrom.ToLower().Contains(streetWorkingFrom.ToLower()) && e.StreetTo.ToLower().Contains(streetWorkingOn.ToLower())).ToList();
            //var jobApplications = rpoContext.JobApplications.Include("Job").Include("JobApplicationType").Where(e => e.ApplicationNumber == trackingNumber && e.Id != id && e.IdJob != idJob && e.JobApplicationType.IdParent == idJobApplicationTypeParent).ToList();
            return jobApplications;
        }
        /// <summary>
        /// Jobs the application number exists.
        /// </summary>
        /// <param name="trackingNumber">The application number. comapre with Streetworkingon,Streetworkingfrom,StreetworkingTo to validate it</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool JobApplicationLocationExists(string streetWorkingOn, string streetWorkingFrom, string streetWorkingTo, int id, int idJob, int idJobApplicationTypeParent, int idJobApplicationType)
        {
            var jobApplications = rpoContext.JobApplications.Include("Job").Include("JobApplicationType").Count(e => e.StreetFrom == streetWorkingFrom &&
              e.StreetTo == streetWorkingTo &&
              e.StreetWorkingOn == streetWorkingOn
              && e.Id != id && e.IdJob == idJob && e.JobApplicationType.IdParent == idJobApplicationTypeParent && e.IdJobApplicationType == idJobApplicationType);

            return jobApplications > 0;

        }

        /// <summary>
        /// Jobs the application number valid.
        /// </summary>
        /// <param name="previousApplicationNumber">The previous application number.</param>
        /// <param name="newApplicationNumber">The new application number.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool JobApplicationNumberValid(string previousApplicationNumber, string newApplicationNumber)
        {
            if (string.IsNullOrEmpty(previousApplicationNumber))
            {
                return true;
            }
            else
            {
                if (string.IsNullOrEmpty(newApplicationNumber))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        [Authorize]
        [RpoAuthorize]
        [Route("api/dobnowInspection/{job_filing_number}")]
        //[ResponseType(typeof(DobNowDTO))]
        [HttpGet]
        public IHttpActionResult GetDOBNow(string job_filing_number)
        {
            string qry = string.Empty;

            Tools.ClsApplicationStatus objapp = new Tools.ClsApplicationStatus();

            var client = new SodaClient("https://data.cityofnewyork.us", "7tPQriTSRiloiKdWIJoODgReN");
            var dataset = client.GetResource<object>("w9ak-ipjd");

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12;

            //var rows = dataset.GetRows(limit: 1000);

            //qry = qry + "job_filing_number='" + "Q00538865-S2" + "'";
            qry = qry + "job_filing_number='" + job_filing_number + "'";

            var soql = new SoqlQuery().Select("job_filing_number", "filing_status", "job_type", "house_no", "street_name", "borough", "applicant_first_name", "applicant_last_name").Where(qry);

            var results = dataset.Query(soql);
            if (results != null && results.Count() > 0)
            {
                return Ok(results.FirstOrDefault());
              
            }

            return this.NotFound();
        }
    }
}