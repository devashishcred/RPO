// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-31-2018
// ***********************************************************************
// <copyright file="JobsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Jobs Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Jobs namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Jobs
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

    /// <summary>
    /// Class Jobs Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class JobsController : HubApiController<GroupHub>
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
        [ResponseType(typeof(AdvancedSearchParameters))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetJobs([FromUri] AdvancedSearchParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
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
                        case GlobalSearchType.ViolationNumber:
                            var jobViolation = rpoContext.JobViolations.Where(x => x.SummonsNumber.Contains(dataTableParameters.GlobalSearchText.Trim())).Select(x => x.IdJob ?? 0).Distinct().ToList();
                            jobs = jobs.Where(x => jobViolation.Contains(x.Id));
                            break;
                    }
                }
                //else
                //{
                if (!string.IsNullOrWhiteSpace(dataTableParameters.JobNumber))
                {
                    jobs = jobs.Where(j => j.JobNumber == dataTableParameters.JobNumber);
                }
                else
                {
                    if ((dataTableParameters.OnlyMyJobs == null || dataTableParameters.OnlyMyJobs.Value))
                    {
                        if (employee != null)
                        {
                            string employeeId = Convert.ToString(employee.Id);
                            jobs = jobs.Where(j =>
                            (
                                (
                                j.DOTProjectTeam == employeeId || j.DOTProjectTeam.StartsWith(employeeId + ",") || j.DOTProjectTeam.Contains("," + employeeId + ",") || j.DOTProjectTeam.EndsWith("," + employeeId)
                                )
                                && j.DOTProjectTeam != null && j.DOTProjectTeam != ""
                            ) ||
                            (
                                (
                                j.DOBProjectTeam == employeeId || j.DOBProjectTeam.StartsWith(employeeId + ",") || j.DOBProjectTeam.Contains("," + employeeId + ",") || j.DOBProjectTeam.EndsWith("," + employeeId)
                                )
                                && j.DOBProjectTeam != null && j.DOBProjectTeam != ""
                            ) ||
                            (
                                (
                                j.ViolationProjectTeam == employeeId || j.ViolationProjectTeam.StartsWith(employeeId + ",") || j.ViolationProjectTeam.Contains("," + employeeId + ",") || j.ViolationProjectTeam.EndsWith("," + employeeId)
                                )
                                && j.ViolationProjectTeam != null && j.ViolationProjectTeam != ""
                            ) ||
                            (
                                (
                                j.DEPProjectTeam == employeeId || j.DEPProjectTeam.StartsWith(employeeId + ",") || j.DEPProjectTeam.Contains("," + employeeId + ",") || j.DEPProjectTeam.EndsWith("," + employeeId)
                                )
                                && j.DEPProjectTeam != null && j.DEPProjectTeam != ""
                            ) ||
                            j.IdProjectManager == employee.Id);
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

        /// <summary>
        /// Gets the jobs.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Get the job List against advance search filters.</returns>
        [ResponseType(typeof(AdvancedSearchParameters))]
        [Authorize]
        [RpoAuthorize]
        //  [ResponseType(typeof(List<ContactList>))]
        [Route("api/JobsList/{PageIndex}/{PageSize}")]
        public IHttpActionResult GetJobsList([FromUri] AdvancedSearchParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewContact)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddContact)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteContact))
            {
                //var contacts = rpoContext.Contacts.Include(p => p.ContactLicenses).AsQueryable();


                string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();
                string Firstname = string.Empty;
                string Lastname = string.Empty;
                if (dataTableParameters.GlobalSearchType != null && dataTableParameters.GlobalSearchType > 0)
                {
                    string[] strArray = dataTableParameters.GlobalSearchText.Split(' ');
                    if (strArray.Count() > 0)
                    {
                        Firstname = strArray[0].Trim();
                    }
                    if (strArray.Count() > 1)
                    {
                        Lastname = strArray[1].Trim();
                    }
                }

                DataSet ds = new DataSet();

                SqlParameter[] spParameter = new SqlParameter[3];

                spParameter[0] = new SqlParameter("@PageIndex", SqlDbType.Int);
                spParameter[0].Direction = ParameterDirection.Input;
                spParameter[0].Value = dataTableParameters.PageIndex;

                spParameter[1] = new SqlParameter("@PageSize", SqlDbType.Int);
                spParameter[1].Direction = ParameterDirection.Input;
                spParameter[1].Value = dataTableParameters.PageSize;

                spParameter[2] = new SqlParameter("@RecordCount", SqlDbType.Int);
                spParameter[2].Direction = ParameterDirection.Output;
                //spParameter[1].Value = dataTableParameters.PageSize;

                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "Jobs_List", spParameter);

                int totalrecord = 0;
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    totalrecord = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                }
                if (ds.Tables.Count > 1)
                {
                    ds.Tables[1].TableName = "Data";
                }
                return Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.PageIndex.Value,
                    RecordsFiltered = totalrecord,
                    RecordsTotal = totalrecord,
                    Data = ds.Tables[1]
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the jobs.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Get the job List against advance search filters</returns>
        [HttpPost]
        [ResponseType(typeof(DataTableParameters))]
        [Authorize]
        [RpoAuthorize]
        //  [ResponseType(typeof(List<ContactList>))]
        [Route("api/JobsListPost")]
        public IHttpActionResult GetJobsPostList(DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewContact)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddContact)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteContact))
            {
                //var contacts = rpoContext.Contacts.Include(p => p.ContactLicenses).AsQueryable();


                string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();
                string Firstname = string.Empty;
                string Lastname = string.Empty;
                //if (dataTableParameters.GlobalSearchType != null && dataTableParameters.GlobalSearchType > 0)
                //{
                //    string[] strArray = dataTableParameters.GlobalSearchText.Split(' ');
                //    if (strArray.Count() > 0)
                //    {
                //        Firstname = strArray[0].Trim();
                //    }
                //    if (strArray.Count() > 1)
                //    {
                //        Lastname = strArray[1].Trim();
                //    }
                //}

                DataSet ds = new DataSet();

                SqlParameter[] spParameter = new SqlParameter[6];

                spParameter[0] = new SqlParameter("@PageIndex", SqlDbType.Int);
                spParameter[0].Direction = ParameterDirection.Input;
                spParameter[0].Value = dataTableParameters.Start;

                spParameter[1] = new SqlParameter("@PageSize", SqlDbType.Int);
                spParameter[1].Direction = ParameterDirection.Input;
                spParameter[1].Value = dataTableParameters.Length;

                spParameter[2] = new SqlParameter("@Column", SqlDbType.NVarChar, 50);
                spParameter[2].Direction = ParameterDirection.Input;
                spParameter[2].Value = dataTableParameters.OrderedColumn != null ? dataTableParameters.OrderedColumn.Column : null;

                spParameter[3] = new SqlParameter("@Dir", SqlDbType.VarChar, 50);
                spParameter[3].Direction = ParameterDirection.Input;
                spParameter[3].Value = dataTableParameters.OrderedColumn != null ? dataTableParameters.OrderedColumn.Dir : null;


                spParameter[4] = new SqlParameter("@Search", SqlDbType.NVarChar, 50);
                spParameter[4].Direction = ParameterDirection.Input;
                spParameter[4].Value = !string.IsNullOrEmpty(dataTableParameters.Search) ? dataTableParameters.Search : string.Empty;

                spParameter[5] = new SqlParameter("@RecordCount", SqlDbType.Int);
                spParameter[5].Direction = ParameterDirection.Output;

                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "Jobs_List", spParameter);



                int totalrecord = 0;
                int Recordcount = 0;
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    totalrecord = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                    Recordcount = int.Parse(spParameter[5].SqlValue.ToString());

                }
                if (ds.Tables.Count > 1)
                {
                    ds.Tables[1].TableName = "Data";
                }
                return Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Start,
                    RecordsFiltered = Recordcount,
                    RecordsTotal = totalrecord,
                    Data = ds.Tables[1]
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }


        /// <summary>
        /// Gets the job.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Get the job detail.</returns>
        [ResponseType(typeof(Job))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetJob(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewJob)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJob)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteJob))
            {
                Job job = rpoContext
                .Jobs
                .Include("Applications")
                .Include("RfpAddress.Borough")
                .Include("RfpAddress.OwnerType")
                .Include("RfpAddress.Company")
                .Include("RfpAddress.OwnerContact")
                .Include("RfpAddress.OccupancyClassification")
                .Include("RfpAddress.ConstructionClassification")
                .Include("RfpAddress.MultipleDwellingClassification")
                .Include("RfpAddress.PrimaryStructuralSystem")
                .Include("RfpAddress.StructureOccupancyCategory")
                .Include("RfpAddress.SeismicDesignCategory")
                .Include("Rfp")
                .Include("Borough")
                .Include("Company")
                .Include("Company.Addresses")
                .Include("Company.CompanyTypes")
                .Include("JobApplicationTypes")
                .Include("Contact")
                .Include("Contact.Prefix")
                .Include("ProjectManager")
                //.Include("ProjectCoordinator")
                //.Include("SignoffCoordinator")
                .Include("CreatedByEmployee")
                .Include("LastModifiedByEmployee")
                .Include("Contacts")
                .Include("Documents")
                .Include("Transmittals")
                .Include("Tasks")
                .FirstOrDefault(j => j.Id == id);

                if (job == null)
                {
                    return this.NotFound();
                }

                rpoContext.Configuration.LazyLoadingEnabled = false;

                return this.Ok(this.FormatDetails(job));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the job application Type.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Get the job application Type List.</returns>
        [ResponseType(typeof(Job))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/JobApplicationType/{idJob}")]
        public IHttpActionResult GetJobApplicationType(int idJob)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewJob)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJob)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteJob))
            {
                Job job = rpoContext
                .Jobs
                .Include("Applications")
                .Include("JobApplicationTypes")
                .FirstOrDefault(j => j.Id == idJob);

                if (job == null)
                {
                    return this.NotFound();
                }

                JobsDTO objApplicationType = new JobsDTO();

                objApplicationType.JobApplicationType = string.Join(",", job.JobApplicationTypes.Select(x => x.Id.ToString()));

                return this.Ok(objApplicationType);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the job.
        /// </summary>
        /// <remarks>To get the detail of jobs and its references  all the detail load</remarks>
        /// <param name="idJob">The identifier.</param>
        /// <returns>Get the job detail.</returns>
        [ResponseType(typeof(Job))]
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Jobs/{idJob}/details")]
        public IHttpActionResult GetJobDetails(int idJob)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewJob)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJob)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteJob))
            {
                Job job = rpoContext
                .Jobs
                .Include("Applications")
                .Include("RfpAddress.Borough")
                .Include("RfpAddress.OwnerType")
                .Include("RfpAddress.Company")
                .Include("RfpAddress.OwnerContact")
                .Include("RfpAddress.OccupancyClassification")
                .Include("RfpAddress.ConstructionClassification")
                .Include("RfpAddress.MultipleDwellingClassification")
                .Include("RfpAddress.PrimaryStructuralSystem")
                .Include("RfpAddress.StructureOccupancyCategory")
                .Include("RfpAddress.SeismicDesignCategory")
                .Include("RfpAddress.SecondOfficer")
                .Include("Rfp")
                .Include("Borough")
                .Include("Company")
                .Include("Company.Addresses")
                .Include("Company.CompanyTypes")
                .Include("JobApplicationTypes")
                .Include("Contact")
                .Include("Contact.Prefix")
                .Include("ProjectManager")
                //.Include("ProjectCoordinator")
                //.Include("SignoffCoordinator")
                .Include("Contacts")
                .Include("Documents")
                .Include("Transmittals")
                .Include("Tasks")
                .FirstOrDefault(j => j.Id == idJob);

                if (job == null)
                {
                    return this.NotFound();
                }

                rpoContext.Configuration.LazyLoadingEnabled = false;

                return this.Ok(this.FormatDetailsWithoutObject(job));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the jobs list on dropdown.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Get the detail of job with address List for dropdown.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]

        [Route("api/Jobs/dropdown")]
        public IHttpActionResult GetDropdown()
        {
            var result = this.rpoContext.Jobs.Include("RfpAddress.Borough").AsEnumerable().Where(x => x.JobNumber != null).Select(c => new
            {
                Id = c.Id,
                ItemName = c.JobNumber + " - " + (c != null && c.RfpAddress != null ?
                                           (c.RfpAddress.HouseNumber + " " + (c.RfpAddress.Street != null ? c.RfpAddress.Street + ", " : string.Empty)
                                         + (c.RfpAddress.Borough != null ? c.RfpAddress.Borough.Description : string.Empty)) : string.Empty),
                Status = c.Status

            }).Distinct().OrderBy(c => c.Id).ToList();


            return Ok(result);
        }



        /// <summary>
        /// Puts the job.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dto">The dto.</param>
        /// <returns>IHttpActionResult.</returns>
        [ResponseType(typeof(void))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult PutJob(int id, JobCreateOrUpdateDTO dto)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJob))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != dto.Id)
                {
                    return BadRequest();
                }

                var job = rpoContext.Jobs.Find(id);

                if (job == null)
                {
                    return this.NotFound();
                }

                List<int> oldProjectTeam = new List<int>();
                List<int> dobProjectTeam = job.DOBProjectTeam != null && !string.IsNullOrEmpty(job.DOBProjectTeam) ? (job.DOBProjectTeam.Split(',') != null && job.DOBProjectTeam.Split(',').Any() ? job.DOBProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                List<int> dotProjectTeam = job.DOTProjectTeam != null && !string.IsNullOrEmpty(job.DOTProjectTeam) ? (job.DOTProjectTeam.Split(',') != null && job.DOTProjectTeam.Split(',').Any() ? job.DOTProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                List<int> violationProjectTeam = job.ViolationProjectTeam != null && !string.IsNullOrEmpty(job.ViolationProjectTeam) ? (job.ViolationProjectTeam.Split(',') != null && job.ViolationProjectTeam.Split(',').Any() ? job.ViolationProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                List<int> depProjectTeam = job.DEPProjectTeam != null && !string.IsNullOrEmpty(job.DEPProjectTeam) ? (job.DEPProjectTeam.Split(',') != null && job.DEPProjectTeam.Split(',').Any() ? job.DEPProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

                if (dobProjectTeam != null && dobProjectTeam.Count > 0)
                {
                    oldProjectTeam.AddRange(dobProjectTeam);
                }

                if (dotProjectTeam != null && dotProjectTeam.Count > 0)
                {
                    oldProjectTeam.AddRange(dotProjectTeam);
                }

                if (violationProjectTeam != null && violationProjectTeam.Count > 0)
                {
                    oldProjectTeam.AddRange(violationProjectTeam);
                }

                if (depProjectTeam != null && depProjectTeam.Count > 0)
                {
                    oldProjectTeam.AddRange(depProjectTeam);
                }

                if (job.IdProjectManager != null)
                {
                    int idProjectManager = Convert.ToInt32(job.IdProjectManager);
                    oldProjectTeam.Add(idProjectManager);
                }


                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                var jobIdRfpAddress = job.IdRfpAddress;

                dto.CloneTo(job);
                string Teams = string.Empty;
                if (dto.DOTProjectTeam != null && dto.DOTProjectTeam.Count() > 0)
                {
                    job.DOTProjectTeam = string.Join(",", dto.DOTProjectTeam.Select(x => x.ToString()));
                    List<string> DOBProjectTeamname = rpoContext.Employees.Where(x => dto.DOTProjectTeam.Contains(x.Id)).Select(x => x.FirstName + " " + x.LastName).ToList();

                    Teams = Teams + string.Join(",", DOBProjectTeamname.Select(x => x.ToString()));
                }
                else
                {
                    job.DOTProjectTeam = string.Empty;
                }

                if (dto.DOBProjectTeam != null && dto.DOBProjectTeam.Count() > 0)
                {
                    job.DOBProjectTeam = string.Join(",", dto.DOBProjectTeam.Select(x => x.ToString()));
                    List<string> DOBProjectTeamname = rpoContext.Employees.Where(x => dto.DOBProjectTeam.Contains(x.Id)).Select(x => x.FirstName + " " + x.LastName).ToList();

                    Teams = Teams + string.Join(",", DOBProjectTeamname.Select(x => x.ToString()));
                }
                else
                {
                    job.DOBProjectTeam = string.Empty;
                }

                if (dto.ViolationProjectTeam != null && dto.ViolationProjectTeam.Count() > 0)
                {
                    job.ViolationProjectTeam = string.Join(",", dto.ViolationProjectTeam.Select(x => x.ToString()));
                    List<string> DOBProjectTeamname = rpoContext.Employees.Where(x => dto.ViolationProjectTeam.Contains(x.Id)).Select(x => x.FirstName + " " + x.LastName).ToList();

                    Teams = Teams + string.Join(",", DOBProjectTeamname.Select(x => x.ToString()));
                }
                else
                {
                    job.ViolationProjectTeam = string.Empty;
                }

                if (dto.DEPProjectTeam != null && dto.DEPProjectTeam.Count() > 0)
                {
                    job.DEPProjectTeam = string.Join(",", dto.DEPProjectTeam.Select(x => x.ToString()));
                    List<string> DOBProjectTeamname = rpoContext.Employees.Where(x => dto.DEPProjectTeam.Contains(x.Id)).Select(x => x.FirstName + " " + x.LastName).ToList();

                    Teams = Teams + string.Join(",", DOBProjectTeamname.Select(x => x.ToString()));
                }
                else
                {
                    job.DEPProjectTeam = string.Empty;
                }

                job.IdContact = dto.IdContact;
                job.IdJobContactType = dto.IdJobContactType;
                job.StartDate = job.StartDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(job.StartDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : job.StartDate;
                job.EndDate = job.EndDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(job.EndDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : job.EndDate;
                job.LastModiefiedDate = DateTime.UtcNow;
                job.LastModifiedBy = employee.Id;
                job.QBJobName = dto.QBJobName;
                //ML
                job.IdReferredByCompany = dto.IdReferredByCompany;
                job.IdReferredByContact = dto.IdReferredByContact;
                //mb
                job.JobStatusNotes = job.JobStatusNotes;
                var jobTypesToDelete = job.JobApplicationTypes.Where(jt => !dto.JobApplicationTypes.Contains(jt.Id)).ToList();

                foreach (var item in jobTypesToDelete)
                {
                    job.JobApplicationTypes.Remove(item);
                }

                foreach (var item in dto.JobApplicationTypes.Where(jt => !job.JobApplicationTypes.Any(jjt => jjt.Id == jt)))
                {
                    job.JobApplicationTypes.Add(rpoContext.JobApplicationTypes.Find(item));
                }

                //if (dto.IdRfpAddress != jobIdRfpAddress)
                //{
                //    db.Jobs
                //        .Where(j => j.IdRfpAddress == jobIdRfpAddress)
                //        .ToList()
                //        .ForEach(j =>
                //        {
                //            job.Jobs.Remove(j);
                //            j.Jobs.Remove(job);
                //        });

                //    db.Jobs
                //        .Where(j => j.IdRfpAddress == dto.IdRfpAddress)
                //        .ToList()
                //        .ForEach(j =>
                //        {
                //            job.Jobs.Add(j);
                //            j.Jobs.Add(job);
                //        });
                //}

                //  Common.SaveJobHistory(employee.Id, job.Id, JobHistoryMessages.EditJobMessage, JobHistoryType.Job);

                JobContact objcontact = (from d in rpoContext.JobContacts where d.IdJob == id && d.IdContact == dto.IdContact select d).FirstOrDefault();

                List<JobContact> objcontact1 = (from d in rpoContext.JobContacts where d.IdJob == id select d).ToList();
                if (objcontact1 != null)
                {
                    foreach (var item in objcontact1)
                    {
                        item.IsMainCompany = false;
                        //  item.IsBilling = false;
                        rpoContext.SaveChanges();
                    }
                }

                if (objcontact == null && dto.IdContact != null)
                {
                    List<JobContact> objcontact2 = (from d in rpoContext.JobContacts where d.IdJob == id select d).ToList();
                    if (objcontact2 != null)
                    {
                        foreach (var item in objcontact2)
                        {
                            item.IsMainCompany = false;
                            item.IsBilling = false;
                            rpoContext.SaveChanges();
                        }
                    }

                    JobContact jobContact = new JobContact();
                    jobContact.IdJob = job.Id;
                    jobContact.IdCompany = dto.IdCompany;
                    jobContact.IdContact = dto.IdContact;
                    jobContact.IdAddress = null;
                    jobContact.IsMainCompany = true;
                    jobContact.IdJobContactType = job.IdJobContactType;
                    jobContact.IsBilling = true;
                    rpoContext.JobContacts.Add(jobContact);
                    rpoContext.SaveChanges();

                    string clientname = "Contact is set at Main Client & Billing Client.";
                    JobContact newJobContact = rpoContext.JobContacts.Include("Contact").Include("Job").Include("JobContactType").Include("Address").FirstOrDefault(x => x.Id == jobContact.Id);
                    string addContact = JobHistoryMessages.AddContact
                        .Replace("##JobContactName##", newJobContact != null && newJobContact.Contact != null ? newJobContact.Contact.FirstName + " " + newJobContact.Contact.LastName : JobHistoryMessages.NoSetstring)
                        .Replace("##MainClientBillingClient##", !string.IsNullOrEmpty(clientname) ? clientname : JobHistoryMessages.NoSetstring)
                        .Replace("##ContactType##", newJobContact != null && newJobContact.JobContactType != null ? newJobContact.JobContactType.Name : JobHistoryMessages.NoSetstring)
                        .Replace("##MailingAddress##", newJobContact != null && newJobContact.Address != null ? newJobContact.Address.Address1 + " " + newJobContact.Address.Address2 + " " + newJobContact.Address.City + " " + newJobContact.Address.ZipCode : JobHistoryMessages.NoSetstring);

                    Common.SaveJobHistory(employee.Id, newJobContact.IdJob, addContact, JobHistoryType.Contacts);

                }

                else
                {
                    if (dto.IdContact != null)
                    {
                        objcontact.IdCompany = dto.IdCompany;
                        objcontact.IdContact = dto.IdContact;
                        objcontact.IsMainCompany = true;
                    }
                }

                rpoContext.SaveChanges();

                try
                {
                    rpoContext.SaveChanges();



                    var oldEmployeeList = oldProjectTeam.Distinct().ToList();

                    var newEmployeeList = rpoContext.Employees.Where(x => x.Id == job.IdProjectManager).ToList();

                    foreach (var item in newEmployeeList)
                    {
                        Teams = !string.IsNullOrEmpty(Teams) ? Teams + "," + item.FirstName + " " + item.LastName : Teams + item.FirstName + " " + item.LastName;
                    }




                    Job jobteam = rpoContext.Jobs.FirstOrDefault(x => x.Id == job.Id);
                    List<int> objtm = new List<int>();

                    List<int> dobProjectTeamn = job != null && job.DOBProjectTeam != null && !string.IsNullOrEmpty(job.DOBProjectTeam) ? (job.DOBProjectTeam.Split(',') != null && job.DOBProjectTeam.Split(',').Any() ? job.DOBProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    List<int> dotProjectTeamn = job != null && job.DOTProjectTeam != null && !string.IsNullOrEmpty(job.DOTProjectTeam) ? (job.DOTProjectTeam.Split(',') != null && job.DOTProjectTeam.Split(',').Any() ? job.DOTProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    List<int> violationProjectTeamn = job != null && job.ViolationProjectTeam != null && !string.IsNullOrEmpty(job.ViolationProjectTeam) ? (job.ViolationProjectTeam.Split(',') != null && job.ViolationProjectTeam.Split(',').Any() ? job.ViolationProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    List<int> depProjectTeamn = job != null && job.DEPProjectTeam != null && !string.IsNullOrEmpty(job.DEPProjectTeam) ? (job.DEPProjectTeam.Split(',') != null && job.DEPProjectTeam.Split(',').Any() ? job.DEPProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

                    objtm.AddRange(dobProjectTeamn);
                    objtm.AddRange(dotProjectTeamn);
                    objtm.AddRange(violationProjectTeamn);
                    objtm.AddRange(depProjectTeamn);
                    if (job.IdProjectManager != null)
                    {
                        objtm.Add(job.IdProjectManager.Value);
                    }

                    var newEmployeeList1 = rpoContext.Employees.Where(x => objtm.Contains(x.Id)).Distinct().ToList();
                    string teamname = string.Empty;
                    foreach (var item in newEmployeeList1)
                    {
                        teamname = teamname + item.FirstName + " " + item.LastName + ",";
                    }

                    teamname = !string.IsNullOrEmpty(teamname) ? teamname.Remove(teamname.Length - 1) : string.Empty;

                    Job objjob = rpoContext.Jobs.Include("RfpAddress").Include("RfpAddress.Borough").Include("Contact").Include("Company").Include("JobApplicationTypes").FirstOrDefault(x => x.Id == job.Id);
                    string jobType = objjob != null && objjob.JobApplicationTypes != null ? string.Join(",", objjob.JobApplicationTypes.Select(x => x.Description)) : "Not Set";
                    string EditJobs = JobHistoryMessages.EditJob
                    .Replace("##jobnumber##", !string.IsNullOrEmpty(objjob.JobNumber) ? objjob.JobNumber : JobHistoryMessages.NoSetstring)
                    .Replace("##JobAddress##", objjob != null && objjob.RfpAddress != null ? (!string.IsNullOrEmpty(objjob.RfpAddress.HouseNumber) ? objjob.RfpAddress.HouseNumber : string.Empty) + " " + (!string.IsNullOrEmpty(objjob.RfpAddress.Street) ? objjob.RfpAddress.Street : string.Empty) + " " + (objjob.RfpAddress.Borough != null ? objjob.RfpAddress.Borough.Description : JobHistoryMessages.NoSetstring) + " " + (!string.IsNullOrEmpty(objjob.RfpAddress.ZipCode) ? objjob.RfpAddress.ZipCode : string.Empty) + " " + (!string.IsNullOrEmpty(objjob.SpecialPlace) ? "-" + objjob.SpecialPlace : string.Empty) : JobHistoryMessages.NoSetstring)
                    .Replace("##Floor##", objjob != null && !string.IsNullOrEmpty(objjob.FloorNumber) ? objjob.FloorNumber : JobHistoryMessages.NoSetstring)
                    .Replace("##Apartment##", objjob != null && !string.IsNullOrEmpty(objjob.Apartment) ? objjob.Apartment : JobHistoryMessages.NoSetstring)
                    .Replace("##ProjectDescription##", !string.IsNullOrEmpty(objjob.ProjectDescription) ? objjob.ProjectDescription : JobHistoryMessages.NoSetstring)
                    .Replace("##JobType##", jobType)
                    .Replace("##ProjectTeam##", !string.IsNullOrEmpty(teamname) ? "Project Team: " + teamname : JobHistoryMessages.NoSetstring);

                    Common.SaveJobHistory(employee.Id, job.Id, EditJobs, JobHistoryType.Job);
                    //string jobStatusChangeMessage = JobHistoryMessages.UpdateJob
                    //.Replace("##NewJobStatus##", job.Status.ToString())
                    //.Replace("##OldJobStatus##", oldStatus.ToString())
                    //.Replace("##Reason##", statusReason);
                    //Common.SaveJobHistory(employee.Id, job.Id, jobStatusChangeMessage, JobHistoryType.Job);

                    SendNewMemberAddedToJobProjectTeamMail(job.Id, oldProjectTeam);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok(Format(job));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the job.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>IHttpActionResult.</returns>
        [ResponseType(typeof(JobsDTO))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult PostJob(JobCreateOrUpdateDTO dto)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJob))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                Job job = dto.CloneAs<Job>();

                string Teams = string.Empty;
                List<int> objteam = new List<int>();

                if (dto.DOTProjectTeam != null && dto.DOTProjectTeam.Count() > 0)
                {
                    job.DOTProjectTeam = string.Join(",", dto.DOTProjectTeam.Select(x => x.ToString()));

                    List<string> DOBProjectTeamname = rpoContext.Employees.Where(x => dto.DOTProjectTeam.Contains(x.Id)).Select(x => x.FirstName + " " + x.LastName).ToList();

                    Teams = Teams + string.Join(",", DOBProjectTeamname.Select(x => x.ToString()));
                }
                else
                {
                    job.DOTProjectTeam = string.Empty;
                }

                if (dto.DOBProjectTeam != null && dto.DOBProjectTeam.Count() > 0)
                {
                    job.DOBProjectTeam = string.Join(",", dto.DOBProjectTeam.Select(x => x.ToString()));
                    List<string> DOBProjectTeamname = rpoContext.Employees.Where(x => dto.DOBProjectTeam.Contains(x.Id)).Select(x => x.FirstName + " " + x.LastName).ToList();

                    Teams = Teams + string.Join(",", DOBProjectTeamname.Select(x => x.ToString()));
                }
                else
                {
                    job.DOBProjectTeam = string.Empty;
                }

                if (dto.ViolationProjectTeam != null && dto.ViolationProjectTeam.Count() > 0)
                {
                    job.ViolationProjectTeam = string.Join(",", dto.ViolationProjectTeam.Select(x => x.ToString()));
                    List<string> DOBProjectTeamname = rpoContext.Employees.Where(x => dto.ViolationProjectTeam.Contains(x.Id)).Select(x => x.FirstName + " " + x.LastName).ToList();

                    Teams = Teams + string.Join(",", DOBProjectTeamname.Select(x => x.ToString()));
                }
                else
                {
                    job.ViolationProjectTeam = string.Empty;
                }

                if (dto.DEPProjectTeam != null && dto.DEPProjectTeam.Count() > 0)
                {
                    job.DEPProjectTeam = string.Join(",", dto.DEPProjectTeam.Select(x => x.ToString()));
                    List<string> DOBProjectTeamname = rpoContext.Employees.Where(x => dto.DEPProjectTeam.Contains(x.Id)).Select(x => x.FirstName + " " + x.LastName).ToList();

                    Teams = Teams + string.Join(",", DOBProjectTeamname.Select(x => x.ToString()));
                }
                else
                {
                    job.DEPProjectTeam = string.Empty;
                }

                job.IdContact = dto.IdContact;
                job.IdJobContactType = dto.IdJobContactType;
                job.PONumber = job.PONumber;
                job.StartDate = job.StartDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(job.StartDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : job.StartDate;
                job.EndDate = job.EndDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(job.EndDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : job.EndDate;
                job.LastModiefiedDate = DateTime.UtcNow;
                job.CreatedDate = DateTime.UtcNow;
                job.QBJobName = dto.QBJobName;
                if (employee != null)
                {
                    job.CreatedBy = employee.Id;
                    job.LastModifiedBy = employee.Id;
                }
                //ML
                job.IdReferredByCompany = dto.IdReferredByCompany;
                job.IdReferredByContact = dto.IdReferredByContact;

                //mb jobstatusnote
                job.JobStatusNotes = dto.JobStatusNotes;

                job = rpoContext.Jobs.Add(job);

                if (job.JobApplicationTypes == null)
                    job.JobApplicationTypes = new HashSet<JobApplicationType>();

                foreach (var item in dto.JobApplicationTypes)
                    job.JobApplicationTypes.Add(rpoContext.JobApplicationTypes.Find(item));

                rpoContext.SaveChanges();

                rpoContext.Entry(job).Collection(j => j.Jobs).Load();

                var instance = new DropboxIntegration();
                string uploadFilePath = Properties.Settings.Default.DropboxFolderPath + Convert.ToString(job.Id);
                instance.RunCreateFolder(uploadFilePath + "/DOB");
                instance.RunCreateFolder(uploadFilePath + "/DOT");
                instance.RunCreateFolder(uploadFilePath + "/VIO");

                rpoContext.Jobs
                    .Where(j => j.IdRfpAddress == job.IdRfpAddress && j.Id != job.Id)
                    .ToList()
                    .ForEach(j =>
                    {
                        //removed code for related jobs
                        //job.Jobs.Add(j);
                        j.Jobs.Add(job);
                    });

                job.JobNumber = job.Id.ToString(); //.ToString("000000");
                rpoContext.SaveChanges();

                Job jobteam = rpoContext.Jobs.FirstOrDefault(x => x.Id == job.Id);

                List<int> dobProjectTeam = job != null && job.DOBProjectTeam != null && !string.IsNullOrEmpty(job.DOBProjectTeam) ? (job.DOBProjectTeam.Split(',') != null && job.DOBProjectTeam.Split(',').Any() ? job.DOBProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                List<int> dotProjectTeam = job != null && job.DOTProjectTeam != null && !string.IsNullOrEmpty(job.DOTProjectTeam) ? (job.DOTProjectTeam.Split(',') != null && job.DOTProjectTeam.Split(',').Any() ? job.DOTProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                List<int> violationProjectTeam = job != null && job.ViolationProjectTeam != null && !string.IsNullOrEmpty(job.ViolationProjectTeam) ? (job.ViolationProjectTeam.Split(',') != null && job.ViolationProjectTeam.Split(',').Any() ? job.ViolationProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                List<int> depProjectTeam = job != null && job.DEPProjectTeam != null && !string.IsNullOrEmpty(job.DEPProjectTeam) ? (job.DEPProjectTeam.Split(',') != null && job.DEPProjectTeam.Split(',').Any() ? job.DEPProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

                objteam.AddRange(dobProjectTeam);
                objteam.AddRange(dotProjectTeam);
                objteam.AddRange(violationProjectTeam);
                objteam.AddRange(depProjectTeam);
                if (job.IdProjectManager != null)
                {
                    objteam.Add(job.IdProjectManager.Value);
                }

                var newEmployeeList = rpoContext.Employees.Where(x => objteam.Contains(x.Id)).Distinct().ToList();
                string teamname = string.Empty;
                foreach (var item in newEmployeeList)
                {
                    teamname = teamname + item.FirstName + " " + item.LastName + ",";
                }
                teamname = !string.IsNullOrEmpty(teamname) ? teamname.Remove(teamname.Length - 1) : string.Empty;

                Job objjob = rpoContext.Jobs.Include("RfpAddress").Include("RfpAddress.Borough").Include("Contact").Include("Company").Include("JobApplicationTypes").FirstOrDefault(x => x.Id == job.Id);
                string jobType = objjob != null && objjob.JobApplicationTypes != null ? string.Join(",", objjob.JobApplicationTypes.Select(x => x.Description)) : "Not Set";

                // string jobType = job != null && job.JobApplicationTypes != null ? string.Join(",", job.JobApplicationTypes.Select(x => x.Description)) : "Not Set";

                string addJobs = JobHistoryMessages.AddJob
                   .Replace("##jobnumber##", !string.IsNullOrEmpty(job.JobNumber) ? job.JobNumber : JobHistoryMessages.NoSetstring)
                   .Replace("##JobAddress##", objjob != null && objjob.RfpAddress != null ? (!string.IsNullOrEmpty(objjob.RfpAddress.HouseNumber) ? objjob.RfpAddress.HouseNumber : string.Empty) + " " + (!string.IsNullOrEmpty(objjob.RfpAddress.Street) ? objjob.RfpAddress.Street : string.Empty) + " " + (objjob.RfpAddress.Borough != null ? objjob.RfpAddress.Borough.Description : JobHistoryMessages.NoSetstring) + " " + (!string.IsNullOrEmpty(objjob.RfpAddress.ZipCode) ? objjob.RfpAddress.ZipCode : string.Empty) + " " + (!string.IsNullOrEmpty(objjob.SpecialPlace) ? "-" + objjob.SpecialPlace : string.Empty) : JobHistoryMessages.NoSetstring)
                   .Replace("##Floor##", job != null && !string.IsNullOrEmpty(job.FloorNumber) ? job.FloorNumber : JobHistoryMessages.NoSetstring)
                   .Replace("##Apartment##", job != null && !string.IsNullOrEmpty(job.Apartment) ? job.Apartment : JobHistoryMessages.NoSetstring)
                   .Replace("##ProjectDescription##", !string.IsNullOrEmpty(job.ProjectDescription) ? job.ProjectDescription : JobHistoryMessages.NoSetstring)
                   .Replace("##JobType##", jobType)
                   .Replace("##ProjectTeam##", !string.IsNullOrEmpty(teamname) ? "Project Team: " + teamname : JobHistoryMessages.NoSetstring)
                   .Replace("##RFP##", job.IdRfp != null ? "Job Created from RFP :" + job.IdRfp : string.Empty);


                // .Replace("##ContactName##", job != null && job.Contact != null ? job.Contact.FirstName + " " + job.Contact.LastName : string.Empty)
                // .Replace("##CompanyName##", job != null && job.Company != null ? job.Company.Name : "Individual");

                Common.SaveJobHistory(employee.Id, job.Id, addJobs, JobHistoryType.Job);



                if (job.IdContact != null)
                {
                    JobContact jobContact = new JobContact();
                    jobContact.IdJob = job.Id;
                    jobContact.IdCompany = job.IdCompany;
                    jobContact.IdContact = job.IdContact;
                    jobContact.IdAddress = null;
                    jobContact.IsMainCompany = true;
                    jobContact.IdJobContactType = job.IdJobContactType;
                    jobContact.IsBilling = true;
                    rpoContext.JobContacts.Add(jobContact);

                    rpoContext.SaveChanges();

                    string clientname = "Contact is set at Main Client & Billing Client.";
                    JobContact newJobContact = rpoContext.JobContacts.Include("Contact").Include("Job").Include("JobContactType").Include("Address").FirstOrDefault(x => x.Id == jobContact.Id);
                    string addContact = JobHistoryMessages.AddContact
                        .Replace("##JobContactName##", newJobContact != null && newJobContact.Contact != null ? newJobContact.Contact.FirstName + " " + newJobContact.Contact.LastName : JobHistoryMessages.NoSetstring)
                        .Replace("##MainClientBillingClient##", !string.IsNullOrEmpty(clientname) ? clientname : JobHistoryMessages.NoSetstring)
                        .Replace("##ContactType##", newJobContact != null && newJobContact.JobContactType != null ? newJobContact.JobContactType.Name : JobHistoryMessages.NoSetstring)
                        .Replace("##MailingAddress##", newJobContact != null && newJobContact.Address != null ? newJobContact.Address.Address1 + " " + newJobContact.Address.Address2 + " " + newJobContact.Address.City + " " + newJobContact.Address.ZipCode : JobHistoryMessages.NoSetstring);

                    Common.SaveJobHistory(employee.Id, newJobContact.IdJob, addContact, JobHistoryType.Contacts);

                }

                job = rpoContext.Jobs.Include("RfpAddress.Borough").Include("Contact").Include("Company").Include("JobApplicationTypes").FirstOrDefault(x => x.Id == job.Id);


                if (job.IdRfp > 0)
                {
                    Rfp rfp = rpoContext
                    .Rfps
                    .Include("RfpFeeSchedules")
                    .Include("Milestones.MilestoneServices")
                    .FirstOrDefault(r => r.Id == job.IdRfp);

                    if (rfp != null)
                    {
                        foreach (RfpFeeSchedule item in rfp.RfpFeeSchedules)
                        {
                            JobFeeSchedule jobFeeSchedule = new JobFeeSchedule();
                            jobFeeSchedule.IdRfp = item.IdRfp;
                            jobFeeSchedule.IdJob = job.Id;
                            jobFeeSchedule.IdRfpWorkType = item.IdRfpWorkType;

                            RfpCostType cstType = (from d in rpoContext.RfpJobTypes where d.Id == item.IdRfpWorkType select d.CostType).FirstOrDefault();

                            if (cstType != null && cstType.ToString() == "HourlyCost")
                            {
                                jobFeeSchedule.Quantity = item.Quantity * 60;
                                jobFeeSchedule.QuantityPending = item.Quantity * 60;
                            }
                            else
                            {
                                jobFeeSchedule.Quantity = item.Quantity;
                                jobFeeSchedule.QuantityPending = item.Quantity;
                            }
                            //  jobFeeSchedule.Quantity = item.Quantity;
                            jobFeeSchedule.Description = item.Description;
                            jobFeeSchedule.QuantityAchieved = 0;
                            jobFeeSchedule.Cost = item.Cost;
                            jobFeeSchedule.TotalCost = item.TotalCost;
                            jobFeeSchedule.PONumber = job.PONumber;
                            jobFeeSchedule.IdRfpFeeSchedule = item.Id;
                            jobFeeSchedule.CreatedBy = employee.Id;
                            jobFeeSchedule.ModifiedBy = employee.Id;
                            jobFeeSchedule.CreatedDate = DateTime.UtcNow;
                            jobFeeSchedule.LastModified = DateTime.UtcNow;
                            jobFeeSchedule.IsFromScope = true;
                            rpoContext.JobFeeSchedules.Add(jobFeeSchedule);
                        }
                        rpoContext.SaveChanges();

                        foreach (Milestone item in rfp.Milestones)
                        {
                            JobMilestone jobMilestone = new JobMilestone();
                            jobMilestone.IdJob = job.Id;
                            jobMilestone.Name = item.Name;
                            jobMilestone.IdRfp = item.IdRfp;
                            jobMilestone.Status = "Pending";
                            jobMilestone.Value = item.Value;
                            jobMilestone.PONumber = job.PONumber;
                            jobMilestone.CreatedBy = employee.Id;
                            jobMilestone.ModifiedBy = employee.Id;
                            jobMilestone.CreatedDate = DateTime.UtcNow;
                            jobMilestone.LastModified = DateTime.UtcNow;

                            rpoContext.JobMilestones.Add(jobMilestone);

                            rpoContext.SaveChanges();
                            foreach (MilestoneService milestoneService in item.MilestoneServices)
                            {
                                var jobFeeSchedule = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.IdRfpFeeSchedule == milestoneService.IdRfpFeeSchedule);
                                JobMilestoneService jobMilestoneService = new JobMilestoneService();
                                jobMilestoneService.IdMilestone = jobMilestone.Id;
                                jobMilestoneService.IdJobFeeSchedule = jobFeeSchedule.Id;
                                rpoContext.JobMilestoneServices.Add(jobMilestoneService);
                            }
                        }

                        rpoContext.SaveChanges();
                        SendRFPConvertedToJobMail(job.Id);
                    }
                }


                rpoContext.Entry(job).Reference(j => j.RfpAddress).Load();
                rpoContext.Entry(job).Reference(j => j.Borough).Load();
                rpoContext.Entry(job).Reference(j => j.Company).Load();
                rpoContext.Entry(job).Reference(j => j.Contact).Load();
                rpoContext.Entry(job).Collection(j => j.Applications).Load();
                rpoContext.Entry(job).Reference(j => j.ProjectManager).Load();
                //rpoContext.Entry(job).Reference(j => j.ProjectCoordinator).Load();
                //rpoContext.Entry(job).Reference(j => j.SignoffCoordinator).Load();

                SendJobAssignMail(job.Id);

                return this.CreatedAtRoute("DefaultApi", new { id = job.Id }, Format(job));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the job.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [ResponseType(typeof(string))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult DeleteJob(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteJob))
            {
                Job job = rpoContext.Jobs.Find(id);
                if (job == null)
                {
                    return this.NotFound();
                }

                while (job.Jobs.Any())
                {
                    var jb = job.Jobs.First();

                    if (jb.Jobs.Any(j => j.Id == id))
                        jb.Jobs.Remove(job);

                    job.Jobs.Remove(jb);
                }

                rpoContext.Jobs.Remove(job);
                rpoContext.SaveChanges();

                return Ok("Deleted!");
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the scopes.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Jobs/{idJob}/Scopes")]
        [ResponseType(typeof(IEnumerable<JobScopeDTO>))]
        public IHttpActionResult GetScopes(int idJob)
        {
            var jobScope = rpoContext.JobScopes.Include("ModifiedByEmployee").Where(j => j.IdJob == idJob).AsQueryable();

            if (jobScope == null)
            {
                return this.NotFound();
            }

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            var result = jobScope.AsEnumerable().Select(scope => new JobScopeDTO
            {
                Id = scope.Id,
                IdJob = scope.IdJob,
                Content = scope.Content,
                LastModified = scope.LastModified != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(scope.LastModified), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : scope.LastModified,
                LastModifiedBy = (scope.ModifiedByEmployee != null && scope.ModifiedByEmployee.FirstName != null) ? scope.ModifiedByEmployee.FirstName + " " + (scope.ModifiedByEmployee.LastName != null ? scope.ModifiedByEmployee.LastName : string.Empty) : string.Empty,
            });

            return Ok(result);
        }

        /// <summary>
        /// Gets the linked RFP list.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Jobs/{idJob}/linkedrfps")]
        public IHttpActionResult GetLinkedRfpList(int idJob)
        {
            var result = rpoContext.JobFeeSchedules.Include("Rfp").Where(j => j.IdJob == idJob && j.IdRfp != null && j.IdRfp > 0).AsQueryable().Select(x => new
            {
                IdRfp = x.IdRfp,
                RfpNumber = x.Rfp != null ? x.Rfp.RfpNumber : string.Empty,
            }).Distinct();

            var result1 = rpoContext.JobMilestones.Where(j => j.IdJob == idJob && j.IdRfp != null && j.IdRfp > 0).AsQueryable().Select(x => new
            {
                IdRfp = x.IdRfp,
                RfpNumber = x.IdRfp != null ? x.IdRfp.ToString() : string.Empty,
            }).Distinct();

            if (result == null && result1 == null)
            {
                return this.NotFound();
            }
            if (result.ToList().Count > 0)
            {
                return Ok(result);
            }
            else
            {
                return Ok(result1);
            }
        }

        ///// <summary>
        ///// Puts the scope.
        ///// </summary>
        ///// <param name="idJob">The identifier job.</param>
        ///// <param name="idScope">The identifier scope.</param>
        ///// <param name="jobScope">The job scope.</param>
        ///// <returns>IHttpActionResult.</returns>
        //[HttpPut]
        //[Authorize]
        //[RpoAuthorize]
        //[Route("api/Jobs/{idJob}/Scopes/{idScope}")]
        //[ResponseType(typeof(JobScope))]
        //public IHttpActionResult PutScope(int idJob, int idScope, JobScope jobScope)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    jobScope.Id = idScope;
        //    jobScope.IdJob = idJob;

        //    var job = rpoContext.Jobs.Find(idJob);

        //    if (job == null)
        //    {
        //        return this.NotFound();
        //    }

        //    var scope = job.Scopes.FirstOrDefault(s => s.Id == idScope);

        //    if (scope == null)
        //    {
        //        return this.NotFound();
        //    }

        //    jobScope.CloneTo(scope);
        //    jobScope.LastModified = DateTime.UtcNow;


        //    JobHistory jobHistory = new JobHistory();

        //    if (Request.GetRequestContext().Principal.Identity.IsAuthenticated)
        //    {
        //        var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
        //        if (employee != null)
        //        {
        //            jobHistory.IdEmployee = employee.Id;
        //            jobScope.ModifiedBy = employee.Id;
        //        }
        //    }


        //    jobHistory.Description = JobHistoryMessages.EditScopeMessage;
        //    jobHistory.HistoryDate = DateTime.UtcNow;
        //    jobHistory.JobHistoryType = JobHistoryType.Scope;
        //    jobHistory.IdJob = jobScope.IdJob;

        //    rpoContext.JobHistories.Add(jobHistory);

        //    rpoContext.SaveChanges();

        //    string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

        //    return Ok(new JobScopeDTO
        //    {
        //        Id = scope.Id,
        //        IdJob = scope.IdJob,
        //        Content = scope.Content,
        //        LastModified = scope.LastModified != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(scope.LastModified), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : scope.LastModified,
        //        LastModifiedBy = (scope.ModifiedByEmployee != null && scope.ModifiedByEmployee.FirstName != null) ? scope.ModifiedByEmployee.FirstName + " " + (scope.ModifiedByEmployee.LastName != null ? scope.ModifiedByEmployee.LastName : string.Empty) : string.Empty,
        //    });
        //}

        ///// <summary>
        ///// Posts the scope.
        ///// </summary>
        ///// <param name="idjob">The idjob.</param>
        ///// <param name="jobScope">The job scope.</param>
        ///// <returns>IHttpActionResult.</returns>
        //[HttpPost]
        //[Authorize]
        //[RpoAuthorize]
        //[Route("api/Jobs/{idjob}/Scopes/")]
        //[ResponseType(typeof(JobScopeDTO))]
        //public IHttpActionResult PostScope(int idjob, JobScope jobScope)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    Job job = rpoContext.Jobs.Find(idjob);

        //    if (job == null)
        //    {
        //        return this.NotFound();
        //    }
        //    jobScope.Id = 0;
        //    jobScope.IdJob = idjob;
        //    jobScope.LastModified = DateTime.UtcNow;

        //    job.Scopes.Add(jobScope);

        //    JobHistory jobHistory = new JobHistory();
        //    if (Request.GetRequestContext().Principal.Identity.IsAuthenticated)
        //    {
        //        var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

        //        if (employee != null)
        //        {
        //            jobHistory.IdEmployee = employee.Id;
        //            jobScope.ModifiedBy = employee.Id;
        //        }
        //    }


        //    jobHistory.Description = JobHistoryMessages.AddScopeMessage;
        //    jobHistory.HistoryDate = DateTime.UtcNow;
        //    jobHistory.JobHistoryType = JobHistoryType.Scope;
        //    jobHistory.IdJob = jobScope.IdJob;

        //    rpoContext.JobHistories.Add(jobHistory);

        //    rpoContext.SaveChanges();

        //    string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

        //    return Ok(new JobScopeDTO
        //    {
        //        Id = jobScope.Id,
        //        IdJob = jobScope.IdJob,
        //        Content = jobScope.Content,
        //        LastModified = jobScope.LastModified != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobScope.LastModified), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobScope.LastModified,
        //        LastModifiedBy = (jobScope.ModifiedByEmployee != null && jobScope.ModifiedByEmployee.FirstName != null) ? jobScope.ModifiedByEmployee.FirstName + " " + (jobScope.ModifiedByEmployee.LastName != null ? jobScope.ModifiedByEmployee.LastName : string.Empty) : string.Empty,
        //    });

        //}

        ///// <summary>
        ///// Deletes the scope.
        ///// </summary>
        ///// <param name="idJob">The identifier job.</param>
        ///// <param name="idScope">The identifier scope.</param>
        ///// <returns>IHttpActionResult.</returns>
        //[HttpDelete]
        //[Authorize]
        //[RpoAuthorize]
        //[Route("api/Jobs/{idjob}/Scopes/{idScope}")]
        //[ResponseType(typeof(void))]
        //public IHttpActionResult DeleteScope(int idJob, int idScope)
        //{
        //    Job job = rpoContext.Jobs.Find(idJob);
        //    if (job == null)
        //    {
        //        return this.NotFound();
        //    }

        //    JobScope scope = job.Scopes.FirstOrDefault(s => s.Id == idScope);

        //    if (scope == null)
        //    {
        //        return this.NotFound();
        //    }

        //    rpoContext.JobScopes.Remove(scope);

        //    var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

        //    JobHistory jobHistory = new JobHistory();
        //    jobHistory.Description = JobHistoryMessages.DeleteScopeMessage;
        //    jobHistory.HistoryDate = DateTime.UtcNow;
        //    jobHistory.JobHistoryType = JobHistoryType.Scope;
        //    jobHistory.IdJob = scope.IdJob;
        //    if (employee != null)
        //    {
        //        jobHistory.IdEmployee = employee.Id;
        //    }

        //    rpoContext.JobHistories.Add(jobHistory);

        //    rpoContext.SaveChanges();

        //    return Ok("Deleted");
        //}

        ///// <summary>
        ///// Puts the milestone.
        ///// </summary>
        ///// <param name="idJob">The identifier job.</param>
        ///// <param name="idMilestone">The identifier milestone.</param>
        ///// <param name="jobMilestone">The job milestone.</param>
        ///// <returns>IHttpActionResult.</returns>
        //[HttpPut]
        //[Route("api/Jobs/{idJob}/Milestones/{idMilestone}")]
        //[ResponseType(typeof(JobMilestoneDTO))]
        //public IHttpActionResult PutMilestone(int idJob, int idMilestone, JobMilestone jobMilestone)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    jobMilestone.Id = idMilestone;
        //    jobMilestone.IdJob = idJob;

        //    var job = rpoContext.Jobs.Find(idJob);

        //    if (job == null)
        //    {
        //        return this.NotFound();
        //    }

        //    var milestone = job.Milestones.FirstOrDefault(M => M.Id == idMilestone);

        //    if (milestone == null)
        //    {
        //        return this.NotFound();
        //    }

        //    jobMilestone.CloneTo(milestone);
        //    jobMilestone.LastModified = DateTime.UtcNow;

        //    //if (Request.GetRequestContext().Principal.Identity.IsAuthenticated)
        //    //    jobMilestone.LastModifiedBy = Request.GetRequestContext().Principal.Identity.Name;

        //    var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

        //    JobHistory jobHistory = new JobHistory();
        //    jobHistory.Description = JobHistoryMessages.EditMilestone_WithoutTypeMessage;
        //    jobHistory.HistoryDate = DateTime.UtcNow;
        //    jobHistory.JobHistoryType = JobHistoryType.Milestone;
        //    jobHistory.IdJob = jobMilestone.IdJob;
        //    if (employee != null)
        //    {
        //        jobHistory.IdEmployee = employee.Id;
        //        jobMilestone.ModifiedBy = employee.Id;
        //    }

        //    rpoContext.JobHistories.Add(jobHistory);

        //    rpoContext.SaveChanges();

        //    string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

        //    return Ok(new JobMilestoneDTO
        //    {
        //        Id = milestone.Id,
        //        IdJob = milestone.IdJob,
        //        Name = milestone.Name,
        //        Value = milestone.Value,
        //        LastModified = milestone.LastModified != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(milestone.LastModified), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : milestone.LastModified,
        //        LastModifiedBy = (milestone.ModifiedByEmployee != null && milestone.ModifiedByEmployee.FirstName != null) ? milestone.ModifiedByEmployee.FirstName + " " + (milestone.ModifiedByEmployee.LastName != null ? milestone.ModifiedByEmployee.LastName : string.Empty) : string.Empty,
        //    });
        //}


        ///// <summary>
        ///// Posts the milestone.
        ///// </summary>
        ///// <param name="idjob">The idjob.</param>
        ///// <param name="jobMilestone">The job milestone.</param>
        ///// <returns>IHttpActionResult.</returns>
        //[HttpPost]
        //[Route("api/Jobs/{idjob}/Milestones/")]
        //[ResponseType(typeof(JobMilestone))]
        //public IHttpActionResult PostMilestone(int idjob, JobMilestone jobMilestone)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    Job job = rpoContext.Jobs.Find(idjob);

        //    if (job == null)
        //    {
        //        return this.NotFound();
        //    }
        //    jobMilestone.Id = 0;
        //    jobMilestone.IdJob = idjob;

        //    jobMilestone.LastModified = DateTime.UtcNow;

        //    job.Milestones.Add(jobMilestone);

        //    var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

        //    JobHistory jobHistory = new JobHistory();

        //    jobHistory.Description = JobHistoryMessages.AddMilestone_WithoutTypeMessage;
        //    jobHistory.HistoryDate = DateTime.UtcNow;
        //    jobHistory.JobHistoryType = JobHistoryType.Milestone;
        //    jobHistory.IdJob = jobMilestone.IdJob;
        //    if (employee != null)
        //    {
        //        jobHistory.IdEmployee = employee.Id;
        //        jobMilestone.ModifiedBy = employee.Id;
        //    }

        //    rpoContext.JobHistories.Add(jobHistory);

        //    rpoContext.SaveChanges();

        //    return Ok(jobMilestone);
        //}

        /// <summary>
        /// Puts the scope general notes.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="generalNotesDTO">The general notes dto.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Jobs/{idJob}/ScopeGeneralNotes/")]
        [ResponseType(typeof(string))]
        public IHttpActionResult PutScopeGeneralNotes(int idJob, [FromBody] GeneralNotesDTO generalNotesDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var job = rpoContext.Jobs.Find(idJob);

            if (job == null)
            {
                return this.NotFound();
            }

            job.ScopeGeneralNotes = generalNotesDTO.GeneralNotes;

            rpoContext.SaveChanges();

            return Ok(job.ScopeGeneralNotes);
        }

        /// <summary>
        /// Puts the job status.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="status">The status.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Jobs/{idJob}/Status")]
        [ResponseType(typeof(JobStatus))]
        public IHttpActionResult PutJobStatus(int idJob, [FromBody] JobStatusDTO statusDTO)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJob))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var job = rpoContext.Jobs.Find(idJob);

                if (job == null)
                {
                    return this.NotFound();
                }

                JobStatus oldStatus = job.Status;

                switch (statusDTO.JobStatus)
                {
                    case JobStatus.Active:
                        if (job.Status == JobStatus.Hold)
                        {
                            SendJobPutInProgressMail(job.Id, statusDTO.StatusReason);
                        }
                        else
                        {
                            SendJobReOpenMail(job.Id);
                        }
                        break;
                    case JobStatus.Hold:
                        SendJobPutOnHoldMail(job.Id, statusDTO.StatusReason);
                        break;
                    case JobStatus.Close:
                        SendJobMarkedCompletedMail(job.Id, employee);
                        break;
                    default:
                        break;
                }

                if (job.Status == JobStatus.Close)
                {
                    job.EndDate = DateTime.UtcNow;
                }
                else
                {
                    job.EndDate = null;
                }

                job.EndDate = DateTime.UtcNow;
                job.Status = statusDTO.JobStatus;
                job.LastModiefiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    job.LastModifiedBy = employee.Id;
                }
                rpoContext.SaveChanges();

                string statusReason = string.Empty;
                if (!string.IsNullOrEmpty(statusDTO.StatusReason))
                {
                    statusReason = statusDTO.StatusReason;
                }
                if (job.Status == JobStatus.Close)
                {
                    string jobStatuscompletedMessage = JobHistoryMessages.JobStatusCompletedMessage
                   .Replace("##NewJobStatus##", job.Status.ToString())
                   .Replace("##OldJobStatus##", oldStatus.ToString());
                    Common.SaveJobHistory(employee.Id, job.Id, jobStatuscompletedMessage, JobHistoryType.Job);
                }
                else if (oldStatus != job.Status)
                {
                    string jobStatusChangeMessage = JobHistoryMessages.JobStatusChangeMessage
                    .Replace("##NewJobStatus##", job.Status.ToString())
                    .Replace("##OldJobStatus##", oldStatus.ToString())
                    .Replace("##Reason##", statusReason);
                    Common.SaveJobHistory(employee.Id, job.Id, jobStatusChangeMessage, JobHistoryType.Job);
                }

                return Ok(job.Status);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Labels the address.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>IHttpActionResult.</returns>
        [Route("api/Jobs/{idJob}/Label")]
        [ResponseType(typeof(string))]
        [HttpGet]
        public IHttpActionResult LabelAddress(int idJob)
        {
            var job = rpoContext.Jobs.Find(idJob);

            if (job == null)
            {
                return this.NotFound();
            }

            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

            return Ok($"{APIUrl}{(APIUrl.EndsWith("/") ? "" : "/")}api/Jobs/{idJob}/LabelPDF");
        }

        /// <summary>
        /// Labels the specified identifier job.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        [Route("api/Jobs/{idJob}/LabelPDF")]
        [ResponseType(typeof(string))]
        [HttpGet]
        public async Task<HttpResponseMessage> Label(int idJob)
        {
            var job = rpoContext.Jobs.Find(idJob);

            if (job == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            using (MemoryStream stream = new MemoryStream())
            {
                string filename = "Job-label-" + idJob + ".pdf";

                Document document = new Document(new Rectangle(577f, 286f));
                PdfWriter writer = PdfWriter.GetInstance(document, stream);
                document.Open();
                string jobStatus = job.Status == JobStatus.Active ? "In Progress" : job.Status.ToString();
                string jobCompany = job.Company != null ? job.Company.Name.ToUpper() : string.Empty;
                string jobBorough = job.Borough.Description != null ? job.Borough.Description.ToUpper() : string.Empty;
                string jobApartment = job.Apartment != null ? job.Apartment.ToUpper() : string.Empty;
                string jobSpecialPlace = job.SpecialPlace != null ? job.SpecialPlace.ToUpper() : string.Empty;
                string jobFirstName = job.Contact.FirstName != null ? job.Contact.FirstName.ToUpper() : string.Empty;
                string jobLastName = job.Contact.LastName != null ? job.Contact.LastName.ToUpper() : string.Empty;
                string jobHouseNumber = job.HouseNumber != null ? job.HouseNumber.ToUpper() : string.Empty;
                string jobStreetNumber = job.StreetNumber != null ? job.StreetNumber.ToUpper() : string.Empty;
                string jobFloorNumber = job.FloorNumber != null ? job.FloorNumber.ToUpper() : string.Empty;



                PdfPTable table = new PdfPTable(2);
                table.SetWidths(new float[] { 20, 80 });
                table.WidthPercentage = 100;
                PdfPCell cell = new PdfPCell(new Phrase(""));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);



                PdfPTable innerTable = new PdfPTable(3);
                innerTable.SetWidths(new float[] { 47, 47, 6 });
                innerTable.WidthPercentage = 100;

                PdfPCell innerCell = new PdfPCell(new Phrase("Job # " + job.JobNumber, new Font(Font.FontFamily.HELVETICA, 20, Font.BOLD)));
                innerCell.HorizontalAlignment = Element.ALIGN_LEFT;
                innerCell.Border = PdfPCell.NO_BORDER;
                innerTable.AddCell(innerCell);


                innerCell = new PdfPCell(new Phrase(jobStatus.ToUpper(), new Font(Font.FontFamily.HELVETICA, 17, Font.BOLD)));
                innerCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                innerCell.Border = PdfPCell.NO_BORDER;
                innerCell.PaddingRight = 10;
                innerTable.AddCell(innerCell);

                innerCell = new PdfPCell(new Phrase());
                innerCell.HorizontalAlignment = Element.ALIGN_LEFT;
                innerCell.BorderWidth = 3;
                innerCell.Padding = 10;
                innerTable.AddCell(innerCell);

                cell = new PdfPCell(innerTable);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingBottom = 8;
                table.AddCell(cell);


                cell = new PdfPCell(new Phrase(""));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(jobHouseNumber + "  " + jobStreetNumber + "  " + jobBorough + "  " + job.RfpAddress.ZipCode, new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD)));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingBottom = 14;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(""));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Apt: " + jobApartment, new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(""));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Floor(s): " + jobFloorNumber, new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(""));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Special Place Name: " + jobSpecialPlace, new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(""));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Client: " + jobFirstName + " " + jobLastName + "  " + jobCompany, new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                document.Add(table);
                document.Close();
                writer.Close();

                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(stream.ToArray())
                };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                result.Content.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("inline")
                    {
                        FileName = filename
                    };
                return result;

            }
        }

        /// <summary>
        /// Gets the children jobs.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Jobs/{idJob}/Jobs")]
        [ResponseType(typeof(IEnumerable<Job>))]
        public IHttpActionResult GetChildrenJobs(int idJob, [FromUri] DataTableParameters dataTableParameters = null)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewJob))
            {
                Job job = rpoContext.Jobs.FirstOrDefault(j => j.Id == idJob);

                if (job == null)
                    return this.NotFound();

                var result = job.Jobs.ToList();

                foreach (var j in result)
                    j.Jobs = null;

                return Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Draw,
                    RecordsFiltered = result.Count,
                    RecordsTotal = result.Count,
                    Data = result
                     .Select(j => Format(j, (int)job.Status))
                    .OrderBy(j => j.Status)
                    .ToArray()
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }


        //Added by mital
        /// <summary>
        /// Gets the children jobs.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Jobs/{idJob}/Jobs2")]
        [ResponseType(typeof(IEnumerable<Job>))]
        public IHttpActionResult GetChildrenJobs2(int idJob, [FromUri] DataTableParameters dataTableParameters = null)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewJob))
            {
                Job job = rpoContext.Jobs.FirstOrDefault(j => j.Id == idJob);
                if (job == null)
                    return this.NotFound();

                var result = rpoContext.Jobs.Where(x => x.IdRfp == job.IdRfp).ToList();
                //if (job == null)
                //    return this.NotFound();

                //var result = job.Jobs.ToList();

                //foreach (var j in result)
                //    j.Jobs = null;

                return Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Draw,
                    RecordsFiltered = result.Count,
                    RecordsTotal = result.Count,
                    Data = result
                     .Select(j => Format(j, (int)job.Status))
                    .OrderBy(j => j.Status)
                    .ToArray()
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        //Added by mital
        /// <summary>
        /// Gets the children jobs.
        /// </summary>
        /// <param name="idviolation">The identifier job.</param>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Projects/{idviolation}/PartofProjects")]
        [ResponseType(typeof(IEnumerable<Job>))]
        public IHttpActionResult GetPartofProjects(int idviolation, [FromUri] DataTableParameters dataTableParameters = null)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewJob))
            {
                List<Rfp> lstRFPs = new List<Rfp>();
                List<Job> lstJobs = new List<Job>();
                var BinNumber = rpoContext.JobViolations.FirstOrDefault(j => j.Id == idviolation).BinNumber;
                if (BinNumber == null)
                    return this.NotFound();
                var IdRFPAddresses = rpoContext.RfpAddresses.Where(x => x.BinNumber == BinNumber).Select(y => y.Id).ToList();
                // var RFPAddresses = rpoContext.RfpAddresses.Where(x => x.BinNumber == BinNumber).Select(y => y.Id).ToList();
                foreach (var ra in IdRFPAddresses)
                {
                    lstJobs.AddRange(rpoContext.Jobs.Where(x => x.IdRfpAddress == ra).ToList());
                }
                //foreach (var r in lstRFPs)
                //{
                //    lstJobs.AddRange(rpoContext.Jobs.Where(x => x.IdRfp == r.Id).ToList());
                //}
                var result = lstJobs;

                return Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Draw,
                    RecordsFiltered = result.Count,
                    RecordsTotal = result.Count,
                    Data = result
                     .Select(j => Format(j, 0))
                    .OrderBy(j => j.Status)
                    .ToArray()
                });

            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        //Added by mital
        /// <summary>
        /// Gets the children jobs.
        /// </summary>
        /// <param name="idviolation">The identifier job.</param>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Projects/{idviolation}/PartofProjectsForCustomer")]
        [ResponseType(typeof(IEnumerable<Job>))]
        public IHttpActionResult GetPartofProjectsForCustomer(int idviolation, [FromUri] DataTableParameters dataTableParameters = null)
        {
            var employee = this.rpoContext.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewJob))
            {
                List<Rfp> lstRFPs = new List<Rfp>();
                List<Job> lstJobs = new List<Job>();
                var BinNumber = rpoContext.JobViolations.FirstOrDefault(j => j.Id == idviolation).BinNumber;
                if (BinNumber == null)
                    return this.NotFound();
                var IdRFPAddresses = rpoContext.RfpAddresses.Where(x => x.BinNumber == BinNumber).Select(y => y.Id).ToList();
                // var RFPAddresses = rpoContext.RfpAddresses.Where(x => x.BinNumber == BinNumber).Select(y => y.Id).ToList();
                foreach (var ra in IdRFPAddresses)
                {
                    lstJobs.AddRange(rpoContext.Jobs.Where(x => x.IdRfpAddress == ra).ToList());
                }
                //foreach (var r in lstRFPs)
                //{
                //    lstJobs.AddRange(rpoContext.Jobs.Where(x => x.IdRfp == r.Id).ToList());
                //}
                var result = lstJobs;


                return Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Draw,
                    RecordsFiltered = result.Count,
                    RecordsTotal = result.Count,
                    Data = result
                     .Select(j => Format(j, 0))
                    .OrderBy(j => j.Status)
                    .ToArray()
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the child job.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="idJobChild">The identifier job child.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Jobs/{idJob}/Jobs/{idJobChild}")]
        [ResponseType(typeof(string))]
        public IHttpActionResult PutChildJob(int idJob, int idJobChild)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var job = rpoContext.Jobs.Find(idJob);

            if (job == null)
                return this.NotFound();

            var childJob = rpoContext.Jobs.Find(idJobChild);

            if (childJob == null)
                return this.NotFound();

            if (!job.Jobs.Any(j => j.Id == idJobChild))
            {
                job.Jobs.Add(childJob);

                childJob.Jobs.Add(job);

                rpoContext.SaveChanges();
            }

            return Ok("Record added");
        }

        /// <summary>
        /// Deletes the child job.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="idJobChild">The identifier job child.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpDelete]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Jobs/{idJob}/Jobs/{idJobChild}")]
        [ResponseType(typeof(string))]
        public IHttpActionResult DeleteChildJob(int idJob, int idJobChild)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var job = rpoContext.Jobs.Find(idJob);

            if (job == null)
                return this.NotFound();

            var childJob = job.Jobs.FirstOrDefault(j => j.Id == idJobChild);

            if (childJob != null)
            {
                job.Jobs.Remove(childJob);

                childJob.Jobs.Remove(job);

                rpoContext.SaveChanges();
            }

            return Ok("Record deleted");
        }

        /// <summary>
        /// Reminds the job.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        //[HttpGet]
        //[Authorize]
        //[RpoAuthorize]
        //[Route("api/Jobs/RemindJob")]
        //public IHttpActionResult RemindJob()
        //{
        //    ApplicationLog.WriteInformationLog("Task Reminder Job is executed at : " + DateTime.Now.ToLongDateString());
        //    if (Convert.ToBoolean(Properties.Settings.Default.TaskReminderSchedulerStart))
        //    {
        //        ApplicationLog.WriteInformationLog("Task Reminder Scheduler Start : " + DateTime.Now.ToLongDateString());
        //        using (var ctx = new Model.RpoContext())
        //        {
        //            DateTime todayDate = DateTime.UtcNow;

        //            List<TaskReminder> taskReminder = ctx.TaskReminders
        //                .Include("LastModified")
        //                .Include("Task")
        //                .Where(r => DbFunctions.TruncateTime(r.ReminderDate) == DbFunctions.TruncateTime(todayDate)).Distinct()
        //                .ToList();

        //            string body = string.Empty;
        //            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/TaskReminderTemplate.htm")))
        //            {
        //                body = reader.ReadToEnd();
        //            }

        //            foreach (TaskReminder item in taskReminder)
        //            {
        //                if (!EmailReminderExists(item.IdTask, Convert.ToInt32(item.LastModifiedBy)))
        //                {
        //                    string emailBody = body;
        //                    emailBody = emailBody.Replace("##EmployeeName##", item.LastModified != null ? item.LastModified.FirstName + " " + item.LastModified.LastName : string.Empty);
        //                    emailBody = emailBody.Replace("##TaskType##", item.Task != null && item.Task.TaskType != null ? item.Task.TaskType.Name : string.Empty);
        //                    emailBody = emailBody.Replace("##DueDate##", item.Task != null ? Convert.ToString(item.Task.CompleteBy) : string.Empty);
        //                    emailBody = emailBody.Replace("##AssignedBy##", item.Task != null && item.Task.AssignedBy != null ? item.Task.AssignedBy.FirstName + " " + item.Task.AssignedBy.LastName : string.Empty);
        //                    emailBody = emailBody.Replace("##AssignedTo##", item.Task != null && item.Task.AssignedTo != null ? item.Task.AssignedTo.FirstName + " " + item.Task.AssignedTo.LastName : string.Empty);
        //                    emailBody = emailBody.Replace("##TaskDetail##", item.Task != null ? item.Task.GeneralNotes : string.Empty);

        //                    ctx.TaskEmailReminderLogs.Add(
        //                        new TaskEmailReminderLog
        //                        {
        //                            EmailBody = emailBody,
        //                            FromName = "RPO APP",
        //                            FromEmail = Properties.Settings.Default.SmtpUserName,
        //                            EmailSubject = "TASK REMINDER",
        //                            BccEmail = "",
        //                            CcEmail = "",
        //                            IdEmployee = Convert.ToInt32(item.LastModifiedBy),
        //                            IdTask = item.IdTask,
        //                            IsMailSent = false,
        //                            ToEmail = item.LastModified.Email,
        //                            ToName = item.LastModified.FirstName + " " + item.LastModified.LastName
        //                        });
        //                    ctx.SaveChanges();
        //                }
        //            }
        //        }

        //        using (var ctx = new Model.RpoContext())
        //        {
        //            DateTime todayDate = DateTime.UtcNow.AddDays(1);

        //            List<Model.Models.Task> tasks = ctx.Tasks
        //                .Include("AssignedTo")
        //                .Include("AssignedBy")
        //                .Where(r => DbFunctions.TruncateTime(r.CompleteBy) == DbFunctions.TruncateTime(todayDate)).Distinct()
        //                .ToList();

        //            string body = string.Empty;
        //            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/TaskReminderTemplate.htm")))
        //            {
        //                body = reader.ReadToEnd();
        //            }

        //            foreach (Model.Models.Task item in tasks)
        //            {
        //                if (!EmailReminderExists(item.Id, Convert.ToInt32(item.IdAssignedBy)))
        //                {
        //                    if (item.AssignedBy != null && !string.IsNullOrWhiteSpace(item.AssignedBy.Email))
        //                    {
        //                        string emailBody = body;
        //                        emailBody = emailBody.Replace("##EmployeeName##", item.AssignedBy != null ? item.AssignedBy.FirstName + " " + item.AssignedBy.LastName : string.Empty);
        //                        emailBody = emailBody.Replace("##TaskType##", item.TaskType != null ? item.TaskType.Name : string.Empty);
        //                        emailBody = emailBody.Replace("##DueDate##", Convert.ToString(item.CompleteBy));
        //                        emailBody = emailBody.Replace("##AssignedBy##", item.AssignedBy != null ? item.AssignedBy.FirstName + " " + item.AssignedBy.LastName : string.Empty);
        //                        emailBody = emailBody.Replace("##AssignedTo##", item.AssignedTo != null ? item.AssignedTo.FirstName + " " + item.AssignedTo.LastName : string.Empty);
        //                        emailBody = emailBody.Replace("##TaskDetail##", item.GeneralNotes);

        //                        ctx.TaskEmailReminderLogs.Add(
        //                            new TaskEmailReminderLog
        //                            {
        //                                EmailBody = emailBody,
        //                                FromName = "RPO APP",
        //                                FromEmail = Properties.Settings.Default.SmtpUserName,
        //                                EmailSubject = "TASK REMINDER",
        //                                BccEmail = "",
        //                                CcEmail = "",
        //                                IdEmployee = Convert.ToInt32(item.IdAssignedBy),
        //                                IdTask = item.Id,
        //                                IsMailSent = false,
        //                                ToEmail = item.AssignedBy != null ? item.AssignedBy.Email : string.Empty,
        //                                ToName = item.AssignedBy != null ? item.AssignedBy.FirstName + " " + item.AssignedBy.LastName : string.Empty
        //                            });
        //                        ctx.SaveChanges();
        //                    }

        //                }

        //                if (!EmailReminderExists(item.Id, Convert.ToInt32(item.IdAssignedTo)))
        //                {
        //                    if (item.AssignedTo != null && !string.IsNullOrWhiteSpace(item.AssignedTo.Email))
        //                    {
        //                        string emailBody = body;
        //                        emailBody = emailBody.Replace("##EmployeeName##", item.AssignedTo != null ? item.AssignedTo.FirstName + " " + item.AssignedTo.LastName : string.Empty);
        //                        emailBody = emailBody.Replace("##TaskType##", item.TaskType != null ? item.TaskType.Name : string.Empty);
        //                        emailBody = emailBody.Replace("##DueDate##", Convert.ToString(item.CompleteBy));
        //                        emailBody = emailBody.Replace("##AssignedBy##", item.AssignedBy != null ? item.AssignedBy.FirstName + " " + item.AssignedBy.LastName : string.Empty);
        //                        emailBody = emailBody.Replace("##AssignedTo##", item.AssignedTo != null ? item.AssignedTo.FirstName + " " + item.AssignedTo.LastName : string.Empty);
        //                        emailBody = emailBody.Replace("##TaskDetail##", item.GeneralNotes);

        //                        ctx.TaskEmailReminderLogs.Add(
        //                            new TaskEmailReminderLog
        //                            {
        //                                EmailBody = emailBody,
        //                                FromName = "RPO APP",
        //                                FromEmail = Properties.Settings.Default.SmtpUserName,
        //                                EmailSubject = "TASK REMINDER",
        //                                BccEmail = "",
        //                                CcEmail = "",
        //                                IdEmployee = Convert.ToInt32(item.IdAssignedTo),
        //                                IdTask = item.Id,
        //                                IsMailSent = false,
        //                                ToEmail = item.AssignedTo != null ? item.AssignedTo.Email : string.Empty,
        //                                ToName = item.AssignedTo != null ? item.AssignedTo.FirstName + " " + item.AssignedTo.LastName : string.Empty
        //                            });
        //                        ctx.SaveChanges();
        //                    }

        //                }
        //            }
        //        }

        //        List<TaskEmailReminderLog> taskEmailReminderLogs = new List<TaskEmailReminderLog>();
        //        using (var ctx = new Model.RpoContext())
        //        {
        //            taskEmailReminderLogs = ctx.TaskEmailReminderLogs
        //                .Where(r => r.IsMailSent == false)
        //                .ToList();

        //        }

        //        foreach (TaskEmailReminderLog item in taskEmailReminderLogs)
        //        {
        //            List<KeyValuePair<string, string>> to = new List<KeyValuePair<string, string>>();
        //            to.Add(new KeyValuePair<string, string>(item.ToEmail, item.ToName));

        //            Tools.Mail.Send(new KeyValuePair<string, string>(item.FromEmail, item.FromName), to, item.EmailSubject, item.EmailBody, true);

        //            using (var ctx = new Model.RpoContext())
        //            {
        //                var taskEmailReminderLog = ctx.TaskEmailReminderLogs.Find(item.Id);
        //                if (taskEmailReminderLog != null)
        //                {
        //                    taskEmailReminderLog.IsMailSent = true;
        //                    ctx.SaveChanges();
        //                }
        //            }
        //        }

        //    }
        //    return Ok();
        //}


        /// <summary>
        /// Gets the jobs Project Team.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/Jobs/ProjectTeam")]
        public IHttpActionResult GetProjectTeamDropdown(int idJob)
        {
            Job job = this.rpoContext.Jobs.FirstOrDefault(x => x.Id == idJob);

            List<int> dobProjectTeam = job != null && job.DOBProjectTeam != null && !string.IsNullOrEmpty(job.DOBProjectTeam) ? (job.DOBProjectTeam.Split(',') != null && job.DOBProjectTeam.Split(',').Any() ? job.DOBProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> dotProjectTeam = job != null && job.DOTProjectTeam != null && !string.IsNullOrEmpty(job.DOTProjectTeam) ? (job.DOTProjectTeam.Split(',') != null && job.DOTProjectTeam.Split(',').Any() ? job.DOTProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> violationProjectTeam = job != null && job.ViolationProjectTeam != null && !string.IsNullOrEmpty(job.ViolationProjectTeam) ? (job.ViolationProjectTeam.Split(',') != null && job.ViolationProjectTeam.Split(',').Any() ? job.ViolationProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> depProjectTeam = job != null && job.DEPProjectTeam != null && !string.IsNullOrEmpty(job.DEPProjectTeam) ? (job.DEPProjectTeam.Split(',') != null && job.DEPProjectTeam.Split(',').Any() ? job.DEPProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

            var result = rpoContext.Employees.Where(x => dotProjectTeam.Contains(x.Id)
                                                        || violationProjectTeam.Contains(x.Id)
                                                        || depProjectTeam.Contains(x.Id)
                                                        || dobProjectTeam.Contains(x.Id)
                                                        || x.Id == job.IdProjectManager
                                                        ).Select(x => new
                                                        {
                                                            Id = x.Id,
                                                            ItemName = x.FirstName + " " + x.LastName
                                                        }).ToArray();

            return this.Ok(result);
        }

        /// <summary>
        /// Gets the project team with job contact dropdown.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/Jobs/ProjectTeamWithJobContact")]
        public IHttpActionResult GetProjectTeamWithJobContactDropdown(int idJob)
        {
            Job job = this.rpoContext.Jobs.FirstOrDefault(x => x.Id == idJob);

            List<int> dobProjectTeam = job != null && job.DOBProjectTeam != null && !string.IsNullOrEmpty(job.DOBProjectTeam) ? (job.DOBProjectTeam.Split(',') != null && job.DOBProjectTeam.Split(',').Any() ? job.DOBProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> dotProjectTeam = job != null && job.DOTProjectTeam != null && !string.IsNullOrEmpty(job.DOTProjectTeam) ? (job.DOTProjectTeam.Split(',') != null && job.DOTProjectTeam.Split(',').Any() ? job.DOTProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> violationProjectTeam = job != null && job.ViolationProjectTeam != null && !string.IsNullOrEmpty(job.ViolationProjectTeam) ? (job.ViolationProjectTeam.Split(',') != null && job.ViolationProjectTeam.Split(',').Any() ? job.ViolationProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> depProjectTeam = job != null && job.DEPProjectTeam != null && !string.IsNullOrEmpty(job.DEPProjectTeam) ? (job.DEPProjectTeam.Split(',') != null && job.DEPProjectTeam.Split(',').Any() ? job.DEPProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

            var result = rpoContext.Employees.Where(x => dotProjectTeam.Contains(x.Id)
                                                        || violationProjectTeam.Contains(x.Id)
                                                        || depProjectTeam.Contains(x.Id)
                                                        || dobProjectTeam.Contains(x.Id)
                                                        || x.Id == job.IdProjectManager
                                                        ).Select(x => new
                                                        {
                                                            Id = x.Id,
                                                            ItemName = x.FirstName + " " + x.LastName

                                                        }).ToArray();


            return this.Ok(result);
        }

        /// <summary>
        /// Emails the reminder exists.
        /// </summary>
        /// <param name="idTask">The identifier task.</param>
        /// <param name="idEmployee">The identifier employee.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool EmailReminderExists(int idTask, int idEmployee)
        {
            using (var db = new Model.RpoContext())
            {
                return db.TaskEmailReminderLogs.Count(e => e.IdTask == idTask && e.IdEmployee == idEmployee) > 0;
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
        /// Jobs the exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool JobExists(int id)
        {
            return rpoContext.Jobs.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified j.
        /// </summary>
        /// <param name="j">The j.</param>
        /// <param name="parentStatusId">The parent status identifier.</param>
        /// <returns>JobsDTO.</returns>
        private JobsDTO Format(Job j, int parentStatusId = 0)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            string jobApplicationType = string.Empty;

            if (j.JobApplicationTypes != null && j.JobApplicationTypes.Count() > 0)
            {
                jobApplicationType = string.Join(",", j.JobApplicationTypes.Select(x => x.Id.ToString()));
            }
            //var Customer= rpoContext.Customers.Where(x => x.IdContcat == j.IdContact).FirstOrDefault();
            // string IsAuthorized = "Un-Authorized";
            // if (Customer != null)
            // {
            //    if( rpoContext.CustomerJobAccess.Any(x => x.IdJob == j.Id && x.IdCustomer == Customer.Id))
            //     {
            //         IsAuthorized = "Authorized";
            //     }
            // }
            return new JobsDTO
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
                //IsAuthorized= IsAuthorized
                ProjectHighlights = rpoContext.JobProgressNotes.Where(x => x.IdJob == j.Id).OrderByDescending(x => x.Id).Select(y => y.Notes).FirstOrDefault(),
                HighlightLastModiefiedDate = rpoContext.JobProgressNotes.Where(x => x.IdJob == j.Id).OrderByDescending(x => x.Id).Select(y => y.LastModifiedDate).FirstOrDefault()




            };
        }
        private JobsDTO Format(Job j, int IdContact, int parentStatusId = 0)
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
            return new JobsDTO
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
                ProjectHighlights = rpoContext.JobProgressNotes.Where(x => x.IdJob == j.Id).OrderByDescending(x => x.Id).Select(y => y.Notes).FirstOrDefault(),
                HighlightLastModiefiedDate = rpoContext.JobProgressNotes.Where(x => x.IdJob == j.Id).OrderByDescending(x => x.Id).Select(y => y.LastModifiedDate).FirstOrDefault(),
                IsAuthorized = IsAuthorized


            };
        }


        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="job">The j.</param>
        /// <returns>JobDetailsDTO.</returns>
        private JobDetailsDTO FormatDetails(Job job)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            List<int> dobProjectTeam = job.DOBProjectTeam != null && !string.IsNullOrEmpty(job.DOBProjectTeam) ? (job.DOBProjectTeam.Split(',') != null && job.DOBProjectTeam.Split(',').Any() ? job.DOBProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> dotProjectTeam = job.DOTProjectTeam != null && !string.IsNullOrEmpty(job.DOTProjectTeam) ? (job.DOTProjectTeam.Split(',') != null && job.DOTProjectTeam.Split(',').Any() ? job.DOTProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> violationProjectTeam = job.ViolationProjectTeam != null && !string.IsNullOrEmpty(job.ViolationProjectTeam) ? (job.ViolationProjectTeam.Split(',') != null && job.ViolationProjectTeam.Split(',').Any() ? job.ViolationProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> depProjectTeam = job.DEPProjectTeam != null && !string.IsNullOrEmpty(job.DEPProjectTeam) ? (job.DEPProjectTeam.Split(',') != null && job.DEPProjectTeam.Split(',').Any() ? job.DEPProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

            string jobApplicationType = string.Empty;

            if (job.JobApplicationTypes != null && job.JobApplicationTypes.Count() > 0)
            {
                jobApplicationType = string.Join(",", job.JobApplicationTypes.Select(x => x.Id.ToString()));
            }

            return new JobDetailsDTO
            {
                Id = job.Id,
                JobNumber = job.JobNumber,
                PoNumber = job.PONumber,
                IdRfpAddress = job.IdRfpAddress,
                RfpAddress = job.RfpAddress,
                IdRfp = job.IdRfp,
                Rfp = job.Rfp,
                ZipCode = job.RfpAddress != null ? job.RfpAddress.ZipCode : null,
                IdBorough = job.RfpAddress != null ? job.RfpAddress.IdBorough : null,
                Borough = job.RfpAddress != null && job.RfpAddress.Borough != null ? job.RfpAddress.Borough : null,
                HouseNumber = job.RfpAddress != null ? job.RfpAddress.HouseNumber : string.Empty,
                StreetNumber = job.RfpAddress != null ? job.RfpAddress.Street : string.Empty,
                FloorNumber = job.FloorNumber,
                Apartment = job.Apartment,
                SpecialPlace = job.SpecialPlace,
                Block = job.Block,
                BinNumber = job.RfpAddress != null ? job.RfpAddress.BinNumber : null,
                Lot = job.Lot,
                HasLandMarkStatus = job.HasLandMarkStatus,
                HasEnvironmentalRestriction = job.HasEnvironmentalRestriction,
                HasOpenWork = job.HasOpenWork,
                JobApplicationTypes = job.JobApplicationTypes,
                IdCompany = job.IdCompany,
                Company = job.Company,
                IdJobContactType = job.IdJobContactType,
                JobContactType = job.JobContactType,
                IdContact = job.IdContact,
                Contact = job.Contact,
                IdProjectManager = job.IdProjectManager,
                ProjectManager = job.ProjectManager,
                StartDate = job.StartDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(job.StartDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : job.StartDate,
                EndDate = job.EndDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(job.EndDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : job.EndDate,
                Status = job.Status,
                ScopeGeneralNotes = job.ScopeGeneralNotes,
                Applications = job.Applications,
                Contacts = job.Contacts,
                Documents = job.Documents,
                Transmittals = job.Transmittals,
                Tasks = job.Tasks,
                Scopes = job.Scopes,
                Milestones = job.Milestones,
                Jobs = job.Jobs,
                HasHolidayEmbargo = job.HasHolidayEmbargo,
                TimeNotes = job.TimeNotes,
                ProjectDescription = job.ProjectDescription,
                CreatedBy = job.CreatedBy,
                LastModifiedBy = job.LastModifiedBy != null ? job.LastModifiedBy : job.CreatedBy,
                CreatedByEmployeeName = job.CreatedByEmployee != null ? job.CreatedByEmployee.FirstName + " " + job.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = job.LastModifiedByEmployee != null ? job.LastModifiedByEmployee.FirstName + " " + job.LastModifiedByEmployee.LastName : (job.CreatedByEmployee != null ? job.CreatedByEmployee.FirstName + " " + job.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = job.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(job.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : job.CreatedDate,
                LastModiefiedDate = job.LastModiefiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(job.LastModiefiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (job.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(job.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : job.CreatedDate),
                OCMCNumber = job.OCMCNumber,
                StreetWorkingFrom = job.StreetWorkingFrom,
                StreetWorkingOn = job.StreetWorkingOn,
                StreetWorkingTo = job.StreetWorkingTo,
                DOBProjectTeam = rpoContext.Employees.Where(x => dobProjectTeam.Contains(x.Id)).Select(x => new JobProjectTeam
                {
                    Id = x.Id,
                    ItemName = x.FirstName + " " + x.LastName
                }),
                DOTProjectTeam = rpoContext.Employees.Where(x => dotProjectTeam.Contains(x.Id)).Select(x => new JobProjectTeam
                {
                    Id = x.Id,
                    ItemName = x.FirstName + " " + x.LastName
                }),
                ViolationProjectTeam = rpoContext.Employees.Where(x => violationProjectTeam.Contains(x.Id)).Select(x => new JobProjectTeam
                {
                    Id = x.Id,
                    ItemName = x.FirstName + " " + x.LastName
                }),
                DEPProjectTeam = rpoContext.Employees.Where(x => depProjectTeam.Contains(x.Id)).Select(x => new JobProjectTeam
                {
                    Id = x.Id,
                    ItemName = x.FirstName + " " + x.LastName
                }),
                JobApplicationType = jobApplicationType,
                QBJobName = job.QBJobName,
                //mb job status note
                JobStatusNotes = job.JobStatusNotes
            };
        }

        /// <summary>
        /// Formats the details without object.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns>JobDetailsDTO.</returns>
        private JobDetailsDTO FormatDetailsWithoutObject(Job job)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            var employee = this.rpoContext.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            List<int> dobProjectTeam = job.DOBProjectTeam != null && !string.IsNullOrEmpty(job.DOBProjectTeam) ? (job.DOBProjectTeam.Split(',') != null && job.DOBProjectTeam.Split(',').Any() ? job.DOBProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> dotProjectTeam = job.DOTProjectTeam != null && !string.IsNullOrEmpty(job.DOTProjectTeam) ? (job.DOTProjectTeam.Split(',') != null && job.DOTProjectTeam.Split(',').Any() ? job.DOTProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> violationProjectTeam = job.ViolationProjectTeam != null && !string.IsNullOrEmpty(job.ViolationProjectTeam) ? (job.ViolationProjectTeam.Split(',') != null && job.ViolationProjectTeam.Split(',').Any() ? job.ViolationProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> depProjectTeam = job.DEPProjectTeam != null && !string.IsNullOrEmpty(job.DEPProjectTeam) ? (job.DEPProjectTeam.Split(',') != null && job.DEPProjectTeam.Split(',').Any() ? job.DEPProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

            List<int> Jobworktype = job.DOBProjectTeam != null && !string.IsNullOrEmpty(job.DOBProjectTeam) ? (job.DOBProjectTeam.Split(',') != null && job.DOBProjectTeam.Split(',').Any() ? job.DOBProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            string jobApplicationType = string.Empty;

            if (job.JobApplicationTypes != null && job.JobApplicationTypes.Count() > 0)
            {
                jobApplicationType = string.Join(",", job.JobApplicationTypes.Select(x => x.Id.ToString()));
            }
            string ContactLink = "";
            if (employee != null)
            {
                ContactLink = employee.IdContcat != 0 ? "<a href=" + Properties.Settings.Default.FrontEndUrl + "/contactdetail/" + employee.IdContcat + "\">" + employee.FirstName + " " + employee.LastName + "</a>" : string.Empty;
            }
            return new JobDetailsDTO
            {
                Id = job.Id,
                JobNumber = job.JobNumber,
                PoNumber = job.PONumber,
                IdRfpAddress = job.IdRfpAddress,
                RfpAddress = job.RfpAddress,
                IdRfp = job.IdRfp,
                Rfp = null,
                ZipCode = job.RfpAddress != null ? job.RfpAddress.ZipCode : null,
                IdBorough = job.RfpAddress != null ? job.RfpAddress.IdBorough : null,
                Borough = job.RfpAddress != null ? job.RfpAddress.Borough : null,
                BinNumber = job.RfpAddress != null ? job.RfpAddress.BinNumber : null,
                HouseNumber = job.RfpAddress != null ? job.RfpAddress.HouseNumber : string.Empty,
                StreetNumber = job.RfpAddress != null ? job.RfpAddress.Street : string.Empty,
                FloorNumber = job.FloorNumber,
                Apartment = job.Apartment,
                SpecialPlace = job.SpecialPlace,
                Block = job.Block,
                Lot = job.Lot,
                HasLandMarkStatus = job.HasLandMarkStatus,
                HasEnvironmentalRestriction = job.HasEnvironmentalRestriction,
                HasOpenWork = job.HasOpenWork,
                JobApplicationTypes = job.JobApplicationTypes,
                IdCompany = job.IdCompany,
                Company = job.Company,
                IdJobContactType = job.IdJobContactType,
                JobContactType = job.JobContactType,
                IdContact = job.IdContact,
                Contact = job.Contact,
                ProjectDescription = job.ProjectDescription,
                IdProjectManager = job.IdProjectManager,
                ProjectManager = job.ProjectManager,

                //IdProjectCoordinator = job.IdProjectCoordinator,
                //ProjectCoordinator = job.ProjectCoordinator,
                //IdSignoffCoordinator = job.IdSignoffCoordinator,
                //SignoffCoordinator = job.SignoffCoordinator,
                StartDate = job.StartDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(job.StartDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : job.StartDate,
                EndDate = job.EndDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(job.EndDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : job.EndDate,
                Status = job.Status,
                ScopeGeneralNotes = job.ScopeGeneralNotes,
                //Applications = j.Applications,
                Applications = null,
                Contacts = null,
                Documents = null,
                Transmittals = null,
                Tasks = null,
                Scopes = null,
                Milestones = null,
                Jobs = null,
                HasHolidayEmbargo = job.HasHolidayEmbargo,
                TimeNotes = job.TimeNotes,
                CreatedBy = job.CreatedBy,
                LastModifiedBy = job.LastModifiedBy != null ? job.LastModifiedBy : job.CreatedBy,
                CreatedByEmployeeName = job.CreatedByEmployee != null ? job.CreatedByEmployee.FirstName + " " + job.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = job.LastModifiedByEmployee != null ? job.LastModifiedByEmployee.FirstName + " " + job.LastModifiedByEmployee.LastName : (job.CreatedByEmployee != null ? job.CreatedByEmployee.FirstName + " " + job.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = job.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(job.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : job.CreatedDate,
                LastModiefiedDate = job.LastModiefiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(job.LastModiefiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (job.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(job.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : job.CreatedDate),
                OCMCNumber = job.OCMCNumber,
                StreetWorkingFrom = job.StreetWorkingFrom,
                StreetWorkingOn = job.StreetWorkingOn,
                StreetWorkingTo = job.StreetWorkingTo,
                QBJobName = job.QBJobName,
                //ML
                IdReferredByCompany = job.IdReferredByCompany,
                IdReferredByContact = job.IdReferredByContact,
                IsBSADecision = job.RfpAddress != null ? job.RfpAddress.IsBSADecision : false,
                //mb jobstatus notes
                JobStatusNotes = job.JobStatusNotes,
                DOBProjectTeam = rpoContext.Employees.Where(x => dobProjectTeam.Contains(x.Id)).Select(x => new JobProjectTeam
                {
                    Id = x.Id,
                    ItemName = x.FirstName + " " + x.LastName,
                    EmailAddress = x.Email,
                    ProjectNumberLink = job.Id != 0 ? "<a href=" + Properties.Settings.Default.FrontEndUrl + "/job/" + job.Id + "/application; idJobAppType=1" + "\">" + job.Id + "</a>" : string.Empty,
                    ContactLink = ContactLink
                }),
                DOTProjectTeam = rpoContext.Employees.Where(x => dotProjectTeam.Contains(x.Id)).Select(x => new JobProjectTeam
                {
                    Id = x.Id,
                    ItemName = x.FirstName + " " + x.LastName,
                    EmailAddress = x.Email,
                    ProjectNumberLink = job.Id != 0 ? "<a href=" + Properties.Settings.Default.FrontEndUrl + "/job/" + job.Id + "/application; idJobAppType=1" + "\">" + job.Id + "</a>," : string.Empty,
                    ContactLink = ContactLink
                }),
                ViolationProjectTeam = rpoContext.Employees.Where(x => violationProjectTeam.Contains(x.Id)).Select(x => new JobProjectTeam
                {
                    Id = x.Id,
                    ItemName = x.FirstName + " " + x.LastName,
                    EmailAddress = x.Email,
                    ProjectNumberLink = job.Id != 0 ? "<a href=" + Properties.Settings.Default.FrontEndUrl + "/job/" + job.Id + "/application; idJobAppType=1" + "\">" + job.Id + "</a>," : string.Empty,
                    ContactLink = ContactLink
                }),
                DEPProjectTeam = rpoContext.Employees.Where(x => depProjectTeam.Contains(x.Id)).Select(x => new JobProjectTeam
                {
                    Id = x.Id,
                    ItemName = x.FirstName + " " + x.LastName,
                    EmailAddress = x.Email,
                    ProjectNumberLink = job.Id != 0 ? "<a href=" + Properties.Settings.Default.FrontEndUrl + "/job/" + job.Id + "/application; idJobAppType=1" + "\">" + job.Id + "</a>," : string.Empty,
                    ContactLink = ContactLink
                }),
                JobApplicationType = jobApplicationType
            };
        }

        /// <summary>
        /// Sends the job assign mail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        private void SendJobAssignMail(int id)
        {
            Job jobs = rpoContext.Jobs.Include("RfpAddress.Borough").Include("Contact").Include("ProjectManager").Where(x => x.Id == id).FirstOrDefault();
            List<JobAssign> jobAssignList = new List<JobAssign>();

            if (jobs.IdProjectManager != null && jobs.IdProjectManager > 0)
            {
                jobAssignList.Add(new JobAssign
                {
                    IdEmployee = jobs.IdProjectManager,
                    JobAplicationType = "DOB",
                });
            }

            if (jobs.DOBProjectTeam != null && !string.IsNullOrEmpty(jobs.DOBProjectTeam))
            {
                foreach (var item in jobs.DOBProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "DOB",
                    });
                }
            }

            if (jobs.DOTProjectTeam != null && !string.IsNullOrEmpty(jobs.DOTProjectTeam))
            {
                foreach (var item in jobs.DOTProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "DOT",
                    });
                }
            }

            if (jobs.ViolationProjectTeam != null && !string.IsNullOrEmpty(jobs.ViolationProjectTeam))
            {
                foreach (var item in jobs.ViolationProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "VIOLATION",
                    });
                }
            }

            if (jobs.DEPProjectTeam != null && !string.IsNullOrEmpty(jobs.DEPProjectTeam))
            {
                foreach (var item in jobs.DEPProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "DEP",
                    });
                }
            }

            var employeelist = jobAssignList.Select(x => x.IdEmployee).Distinct();

            foreach (var item in employeelist)
            {
                int employeeid = Convert.ToInt32(item);
                Employee employee = this.rpoContext.Employees.FirstOrDefault(x => x.Id == employeeid);

                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/NewJobEmailTemplate.htm")))
                {
                    body = reader.ReadToEnd();
                }

                string jobTypeList = string.Empty;

                jobTypeList = string.Join(",", jobAssignList.Where(x => x.IdEmployee == employeeid).Select(r => r.JobAplicationType));
                string emailBody = body;

                emailBody = emailBody.Replace("##EmployeeName##", employee != null ? employee.FirstName : string.Empty);
                emailBody = emailBody.Replace("##JobNumber##", jobs.JobNumber);
                emailBody = emailBody.Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty);
                emailBody = emailBody.Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty);
                emailBody = emailBody.Replace("##CompanyName##", jobs.Company != null ? jobs.Company.Name : "**Individual**");
                emailBody = emailBody.Replace("##ContactName##", jobs.Contact != null ? jobs.Contact.FirstName + " " + jobs.Contact.LastName : string.Empty);
                emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/application");

                List<KeyValuePair<string, string>> to = new List<KeyValuePair<string, string>>();
                to.Add(new KeyValuePair<string, string>(employee.Email, employee.FirstName + " " + employee.LastName));

                List<KeyValuePair<string, string>> cc = new List<KeyValuePair<string, string>>();
                SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.NewJobIsCreated);
                if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                {
                    foreach (EmployeeDetail employeeDetail in systemSettingDetail.Value)
                    {
                        if (employeeDetail.Email != employee.Email)
                        {
                            cc.Add(new KeyValuePair<string, string>(employeeDetail.Email, employeeDetail.EmployeeName));

                            string newJobAssignedNotificationSetting = InAppNotificationMessage.NewJobAssigned
                                .Replace("##JobNumber##", jobs.JobNumber)
                                .Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty)
                                .Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty)
                                .Replace("##CompanyName##", jobs.Company != null ? jobs.Company.Name : "**Individual**")
                                .Replace("##ContactName##", jobs.Contact != null ? jobs.Contact.FirstName + " " + jobs.Contact.LastName : string.Empty)
                                .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/application");
                            Common.SendInAppNotifications(employeeDetail.Id, newJobAssignedNotificationSetting, Hub, "job/" + jobs.Id + "/application");
                        }
                    }
                }

                string newJobAssigned = InAppNotificationMessage.NewJobAssigned
                                .Replace("##JobNumber##", jobs.JobNumber)
                                .Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty)
                                .Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty)
                                .Replace("##CompanyName##", jobs.Company != null ? jobs.Company.Name : "**Individual**")
                                .Replace("##ContactName##", jobs.Contact != null ? jobs.Contact.FirstName + " " + jobs.Contact.LastName : string.Empty)
                                .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/application");
                Tools.Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, EmailNotificationSubject.NewJobAssigned, emailBody, true);
                Common.SendInAppNotifications(employee.Id, newJobAssigned, Hub, "job/" + jobs.Id + "/application");
            }
        }

        public void SendCronJobAssignMail(int id, string message)
        {
            Job jobs = rpoContext.Jobs.Include("RfpAddress.Borough").Include("Contact").Include("ProjectManager").Where(x => x.Id == id).FirstOrDefault();
            List<JobAssign> jobAssignList = new List<JobAssign>();

            if (jobs.IdProjectManager != null && jobs.IdProjectManager > 0)
            {
                jobAssignList.Add(new JobAssign
                {
                    IdEmployee = jobs.IdProjectManager,
                    JobAplicationType = "DOB",
                });
            }

            if (jobs.DOBProjectTeam != null && !string.IsNullOrEmpty(jobs.DOBProjectTeam))
            {
                foreach (var item in jobs.DOBProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "DOB",
                    });
                }
            }


            var employeelist = jobAssignList.Select(x => x.IdEmployee).Distinct();

            foreach (var item in employeelist)
            {
                int employeeid = Convert.ToInt32(item);
                Employee employee = this.rpoContext.Employees.FirstOrDefault(x => x.Id == employeeid);
                Common.SendInAppNotifications(employeeid, message, Hub, "job/" + jobs.Id + "/application");
            }
        }

        private void SendNewMemberAddedToJobProjectTeamMail(int id, List<int> oldProjectTeam)
        {
            Job jobs = rpoContext.Jobs.Include("RfpAddress.Borough").Include("Contact").Include("ProjectManager").Where(x => x.Id == id).FirstOrDefault();
            List<JobAssign> jobAssignList = new List<JobAssign>();

            if (jobs.IdProjectManager != null && jobs.IdProjectManager > 0)
            {
                jobAssignList.Add(new JobAssign
                {
                    IdEmployee = jobs.IdProjectManager,
                    JobAplicationType = "DOB",
                });
            }

            if (jobs.DOBProjectTeam != null && !string.IsNullOrEmpty(jobs.DOBProjectTeam))
            {
                foreach (var item in jobs.DOBProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "DOB",
                    });
                }
            }

            if (jobs.DOTProjectTeam != null && !string.IsNullOrEmpty(jobs.DOTProjectTeam))
            {
                foreach (var item in jobs.DOTProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "DOT",
                    });
                }
            }

            if (jobs.ViolationProjectTeam != null && !string.IsNullOrEmpty(jobs.ViolationProjectTeam))
            {
                foreach (var item in jobs.ViolationProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "VIOLATION",
                    });
                }
            }

            if (jobs.DEPProjectTeam != null && !string.IsNullOrEmpty(jobs.DEPProjectTeam))
            {
                foreach (var item in jobs.DEPProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "DEP",
                    });
                }
            }

            var employeelist = jobAssignList.Select(x => x.IdEmployee).Distinct();
            var oldEmployeeList = oldProjectTeam.Distinct().ToList();

            var newEmployeeList = employeelist.Where(x => !oldEmployeeList.Contains((x ?? 0))).ToList();

            foreach (var item in newEmployeeList)
            {
                int employeeid = Convert.ToInt32(item);
                Employee employee = this.rpoContext.Employees.FirstOrDefault(x => x.Id == employeeid);

                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/NewMemberAddedToJobProjectTeam.htm")))
                {
                    body = reader.ReadToEnd();
                }

                string jobTypeList = string.Empty;

                jobTypeList = string.Join(",", jobAssignList.Where(x => x.IdEmployee == employeeid).Select(r => r.JobAplicationType));
                string emailBody = body;

                emailBody = emailBody.Replace("##EmployeeName##", employee != null ? employee.FirstName : string.Empty);
                emailBody = emailBody.Replace("##JobNumber##", jobs.JobNumber);
                emailBody = emailBody.Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty);
                emailBody = emailBody.Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty);
                emailBody = emailBody.Replace("##CompanyName##", jobs.Company != null ? jobs.Company.Name : "**Individual**");
                emailBody = emailBody.Replace("##ContactName##", jobs.Contact != null ? jobs.Contact.FirstName + " " + jobs.Contact.LastName : string.Empty);
                emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/application");

                List<KeyValuePair<string, string>> to = new List<KeyValuePair<string, string>>();
                to.Add(new KeyValuePair<string, string>(employee.Email, employee.FirstName + " " + employee.LastName));

                List<KeyValuePair<string, string>> cc = new List<KeyValuePair<string, string>>();
                SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.NewMemberAddedToJobProjectTeam);
                if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                {
                    foreach (EmployeeDetail employeeDetail in systemSettingDetail.Value)
                    {
                        if (employeeDetail.Email != employee.Email)
                        {
                            cc.Add(new KeyValuePair<string, string>(employeeDetail.Email, employeeDetail.EmployeeName));

                            string newMemberAddedToJobProjectTeamNotificationSetting = InAppNotificationMessage.NewMemberAddedToJobProjectTeam
                                .Replace("##EmployeeName##", jobs != null && jobs.LastModifiedByEmployee != null ? jobs.LastModifiedByEmployee.FirstName + " " + jobs.LastModifiedByEmployee.LastName : string.Empty)
                                .Replace("##JobNumber##", jobs.JobNumber)
                                .Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty)
                                .Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty)
                                .Replace("##CompanyName##", jobs.Company != null ? jobs.Company.Name : "**Individual**")
                                .Replace("##ContactName##", jobs.Contact != null ? jobs.Contact.FirstName + " " + jobs.Contact.LastName : string.Empty)
                                .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/application");
                            Common.SendInAppNotifications(employeeDetail.Id, newMemberAddedToJobProjectTeamNotificationSetting, Hub, "job/" + jobs.Id + "/application");
                        }
                    }
                }

                string newMemberAddedToJobProjectTeam = InAppNotificationMessage.NewMemberAddedToJobProjectTeam
                                .Replace("##EmployeeName##", jobs != null && jobs.LastModifiedByEmployee != null ? jobs.LastModifiedByEmployee.FirstName + " " + jobs.LastModifiedByEmployee.LastName : string.Empty)
                                .Replace("##JobNumber##", jobs.JobNumber)
                                .Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty)
                                .Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty)
                                .Replace("##CompanyName##", jobs.Company != null ? jobs.Company.Name : "**Individual**")
                                .Replace("##ContactName##", jobs.Contact != null ? jobs.Contact.FirstName + " " + jobs.Contact.LastName : string.Empty)
                                .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/application");
                Tools.Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, EmailNotificationSubject.NewMemberAddedToJobProjectTeam, emailBody, true);
                Common.SendInAppNotifications(employee.Id, newMemberAddedToJobProjectTeam, Hub, "job/" + jobs.Id + "/application");
            }
        }

        private void SendJobPutOnHoldMail(int id, string statusReason)
        {
            //Job jobs = rpoContext.Jobs.Include("RfpAddress.Borough").Include("Contact").Include("ProjectManager").Where(x => x.Id == id).FirstOrDefault();
            Job jobs = rpoContext.Jobs.Include("RfpAddress.Borough").Include("Contact").Include("ProjectManager").Include("LastModifiedByEmployee").Where(x => x.Id == id).FirstOrDefault();
            List<JobAssign> jobAssignList = new List<JobAssign>();

            if (jobs.IdProjectManager != null && jobs.IdProjectManager > 0)
            {
                jobAssignList.Add(new JobAssign
                {
                    IdEmployee = jobs.IdProjectManager,
                    JobAplicationType = "DOB",
                });
            }

            if (jobs.DOBProjectTeam != null && !string.IsNullOrEmpty(jobs.DOBProjectTeam))
            {
                foreach (var item in jobs.DOBProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "DOB",
                    });
                }
            }

            if (jobs.DOTProjectTeam != null && !string.IsNullOrEmpty(jobs.DOTProjectTeam))
            {
                foreach (var item in jobs.DOTProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "DOT",
                    });
                }
            }

            if (jobs.ViolationProjectTeam != null && !string.IsNullOrEmpty(jobs.ViolationProjectTeam))
            {
                foreach (var item in jobs.ViolationProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "VIOLATION",
                    });
                }
            }

            if (jobs.DEPProjectTeam != null && !string.IsNullOrEmpty(jobs.DEPProjectTeam))
            {
                foreach (var item in jobs.DEPProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "DEP",
                    });
                }
            }

            var employeelist = jobAssignList.Select(x => x.IdEmployee).Distinct();

            foreach (var item in employeelist)
            {
                int employeeid = Convert.ToInt32(item);
                Employee employee = this.rpoContext.Employees.FirstOrDefault(x => x.Id == employeeid);

                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/JobPutOnHoldEmailTemplate.htm")))
                {
                    body = reader.ReadToEnd();
                }

                string jobTypeList = string.Empty;

                jobTypeList = string.Join(",", jobAssignList.Where(x => x.IdEmployee == employeeid).Select(r => r.JobAplicationType));
                string emailBody = body;

                emailBody = emailBody.Replace("##EmployeeFirstName##", employee != null ? employee.FirstName : string.Empty);
                emailBody = emailBody.Replace("##JobNumber##", jobs.JobNumber);
                emailBody = emailBody.Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty);
                emailBody = emailBody.Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty);
                emailBody = emailBody.Replace("##StatusReason##", statusReason);
                emailBody = emailBody.Replace("##EmployeeName##", jobs.LastModifiedByEmployee != null ? jobs.LastModifiedByEmployee.FirstName + " " + jobs.LastModifiedByEmployee.LastName : string.Empty);
                emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/application");

                List<KeyValuePair<string, string>> to = new List<KeyValuePair<string, string>>();
                to.Add(new KeyValuePair<string, string>(employee.Email, employee.FirstName + " " + employee.LastName));

                List<KeyValuePair<string, string>> cc = new List<KeyValuePair<string, string>>();
                SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.JobPutOnHold);
                if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                {
                    foreach (EmployeeDetail employeeDetail in systemSettingDetail.Value)
                    {
                        if (employeeDetail.Email != employee.Email)
                        {
                            cc.Add(new KeyValuePair<string, string>(employeeDetail.Email, employeeDetail.EmployeeName));

                            string jobPutOnHoldSetting = InAppNotificationMessage.JobPutOnHold
                                .Replace("##JobNumber##", jobs.JobNumber)
                                .Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty)
                                .Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty)
                                .Replace("##StatusReason##", statusReason)
                                .Replace("##EmployeeName##", jobs.LastModifiedByEmployee != null ? jobs.LastModifiedByEmployee.FirstName + " " + jobs.LastModifiedByEmployee.LastName : string.Empty)
                                .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/application");
                            Common.SendInAppNotifications(employeeDetail.Id, jobPutOnHoldSetting, Hub, "job/" + jobs.Id + "/application");
                        }
                    }
                }

                string jobPutOnHold = InAppNotificationMessage.JobPutOnHold
                                .Replace("##JobNumber##", jobs.JobNumber)
                                .Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty)
                                .Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty)
                                .Replace("##StatusReason##", statusReason)
                                .Replace("##EmployeeName##", jobs.LastModifiedByEmployee != null ? jobs.LastModifiedByEmployee.FirstName + " " + jobs.LastModifiedByEmployee.LastName : string.Empty)
                                .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/application");
                Tools.Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, EmailNotificationSubject.JobPutOnHold, emailBody, true);
                Common.SendInAppNotifications(employee.Id, jobPutOnHold, Hub, "job/" + jobs.Id + "/application");
            }
            jobs.OnHoldCompletedDate = DateTime.Now;
        }

        private void SendJobPutInProgressMail(int id, string statusReason)
        {
            Job jobs = rpoContext.Jobs.Include("RfpAddress.Borough").Include("Contact").Include("ProjectManager").Where(x => x.Id == id).FirstOrDefault();
            List<JobAssign> jobAssignList = new List<JobAssign>();

            if (jobs.IdProjectManager != null && jobs.IdProjectManager > 0)
            {
                jobAssignList.Add(new JobAssign
                {
                    IdEmployee = jobs.IdProjectManager,
                    JobAplicationType = "DOB",
                });
            }

            if (jobs.DOBProjectTeam != null && !string.IsNullOrEmpty(jobs.DOBProjectTeam))
            {
                foreach (var item in jobs.DOBProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "DOB",
                    });
                }
            }

            if (jobs.DOTProjectTeam != null && !string.IsNullOrEmpty(jobs.DOTProjectTeam))
            {
                foreach (var item in jobs.DOTProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "DOT",
                    });
                }
            }

            if (jobs.ViolationProjectTeam != null && !string.IsNullOrEmpty(jobs.ViolationProjectTeam))
            {
                foreach (var item in jobs.ViolationProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "VIOLATION",
                    });
                }
            }

            if (jobs.DEPProjectTeam != null && !string.IsNullOrEmpty(jobs.DEPProjectTeam))
            {
                foreach (var item in jobs.DEPProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "DEP",
                    });
                }
            }

            var employeelist = jobAssignList.Select(x => x.IdEmployee).Distinct();

            foreach (var item in employeelist)
            {
                int employeeid = Convert.ToInt32(item);
                Employee employee = this.rpoContext.Employees.FirstOrDefault(x => x.Id == employeeid);

                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/JobPutInProgressFromHoldEmailTemplate.htm")))
                {
                    body = reader.ReadToEnd();
                }

                string jobTypeList = string.Empty;

                jobTypeList = string.Join(",", jobAssignList.Where(x => x.IdEmployee == employeeid).Select(r => r.JobAplicationType));
                string emailBody = body;

                emailBody = emailBody.Replace("##EmployeeFirstName##", employee != null ? employee.FirstName : string.Empty);
                emailBody = emailBody.Replace("##JobNumber##", jobs.JobNumber);
                emailBody = emailBody.Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty);
                emailBody = emailBody.Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty);
                emailBody = emailBody.Replace("##StatusReason##", statusReason);
                emailBody = emailBody.Replace("##EmployeeName##", jobs.LastModifiedByEmployee != null ? jobs.LastModifiedByEmployee.FirstName + " " + jobs.LastModifiedByEmployee.LastName : string.Empty);
                emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/application");

                List<KeyValuePair<string, string>> to = new List<KeyValuePair<string, string>>();
                to.Add(new KeyValuePair<string, string>(employee.Email, employee.FirstName + " " + employee.LastName));

                List<KeyValuePair<string, string>> cc = new List<KeyValuePair<string, string>>();
                SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.JobPutInProgressFromHold);
                if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                {
                    foreach (EmployeeDetail employeeDetail in systemSettingDetail.Value)
                    {
                        if (employeeDetail.Email != employee.Email)
                        {
                            cc.Add(new KeyValuePair<string, string>(employeeDetail.Email, employeeDetail.EmployeeName));

                            string jobPutInProgressFromHoldSetting = InAppNotificationMessage.JobPutInProgressFromHold
                                .Replace("##JobNumber##", jobs.JobNumber)
                                .Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty)
                                .Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty)
                                .Replace("##StatusReason##", statusReason)
                                .Replace("##EmployeeName##", jobs.LastModifiedByEmployee != null ? jobs.LastModifiedByEmployee.FirstName + " " + jobs.LastModifiedByEmployee.LastName : string.Empty)
                                .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/application");
                            Common.SendInAppNotifications(employeeDetail.Id, jobPutInProgressFromHoldSetting, Hub, "job/" + jobs.Id + "/application");
                        }
                    }
                }

                string jobPutInProgressFromHold = InAppNotificationMessage.JobPutInProgressFromHold
                                .Replace("##JobNumber##", jobs.JobNumber)
                                .Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty)
                                .Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty)
                                .Replace("##StatusReason##", statusReason)
                                .Replace("##EmployeeName##", jobs.LastModifiedByEmployee != null ? jobs.LastModifiedByEmployee.FirstName + " " + jobs.LastModifiedByEmployee.LastName : string.Empty)
                                .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/application");
                Tools.Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, EmailNotificationSubject.JobPutInProgressFromHold, emailBody, true);
                Common.SendInAppNotifications(employee.Id, jobPutInProgressFromHold, Hub, "job/" + jobs.Id + "/application");
            }
            jobs.OnHoldCompletedDate = null;
        }

        private void SendRFPConvertedToJobMail(int id)
        {
            Job jobs = rpoContext.Jobs.Include("Rfp.RfpAddress.Borough").Include("RfpAddress.Borough").Include("Contact").Include("ProjectManager").Where(x => x.Id == id).FirstOrDefault();
            List<JobAssign> jobAssignList = new List<JobAssign>();

            if (jobs.IdProjectManager != null && jobs.IdProjectManager > 0)
            {
                jobAssignList.Add(new JobAssign
                {
                    IdEmployee = jobs.IdProjectManager,
                    JobAplicationType = "DOB",
                });
            }

            if (jobs.DOBProjectTeam != null && !string.IsNullOrEmpty(jobs.DOBProjectTeam))
            {
                foreach (var item in jobs.DOBProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "DOB",
                    });
                }
            }

            if (jobs.DOTProjectTeam != null && !string.IsNullOrEmpty(jobs.DOTProjectTeam))
            {
                foreach (var item in jobs.DOTProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "DOT",
                    });
                }
            }

            if (jobs.ViolationProjectTeam != null && !string.IsNullOrEmpty(jobs.ViolationProjectTeam))
            {
                foreach (var item in jobs.ViolationProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "VIOLATION",
                    });
                }
            }

            if (jobs.DEPProjectTeam != null && !string.IsNullOrEmpty(jobs.DEPProjectTeam))
            {
                foreach (var item in jobs.DEPProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "DEP",
                    });
                }
            }

            var employeelist = jobAssignList.Select(x => x.IdEmployee).Distinct();

            foreach (var item in employeelist)
            {
                int employeeid = Convert.ToInt32(item);
                Employee employee = this.rpoContext.Employees.FirstOrDefault(x => x.Id == employeeid);

                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/RFPConvertedToJobEmailTemplate.htm")))
                {
                    body = reader.ReadToEnd();
                }

                string jobTypeList = string.Empty;

                jobTypeList = string.Join(",", jobAssignList.Where(x => x.IdEmployee == employeeid).Select(r => r.JobAplicationType));
                string emailBody = body;

                emailBody = emailBody.Replace("##EmployeeName##", employee != null ? employee.FirstName : string.Empty);
                emailBody = emailBody.Replace("##JobNumber##", jobs.JobNumber);
                emailBody = emailBody.Replace("##RfpNumber##", jobs.Rfp != null ? jobs.Rfp.RfpNumber : string.Empty);
                emailBody = emailBody.Replace("##CompanyName##", jobs.Company != null ? jobs.Company.Name : "**Individual**");
                emailBody = emailBody.Replace("##ContactName##", jobs.Contact != null ? jobs.Contact.FirstName + " " + jobs.Contact.LastName : string.Empty);
                emailBody = emailBody.Replace("##RFPFullAddress##", jobs.Rfp != null && jobs.Rfp.RfpAddress != null ? jobs.Rfp.RfpAddress.HouseNumber + " " + jobs.Rfp.RfpAddress.Street + (jobs.Rfp.RfpAddress.Borough != null ? " " + jobs.Rfp.RfpAddress.Borough.Description : string.Empty) + " " + jobs.Rfp.RfpAddress.ZipCode : string.Empty);
                emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/application");

                List<KeyValuePair<string, string>> to = new List<KeyValuePair<string, string>>();
                to.Add(new KeyValuePair<string, string>(employee.Email, employee.FirstName + " " + employee.LastName));

                List<KeyValuePair<string, string>> cc = new List<KeyValuePair<string, string>>();
                SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.RFPConvertedToJob);
                if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                {
                    foreach (EmployeeDetail employeeDetail in systemSettingDetail.Value)
                    {
                        if (employeeDetail.Email != employee.Email)
                        {
                            cc.Add(new KeyValuePair<string, string>(employeeDetail.Email, employeeDetail.EmployeeName));

                            string rfpConvertedToJobNotificationSetting = InAppNotificationMessage.RFPConvertedToJob
                                        .Replace("##JobNumber##", jobs.JobNumber)
                                        .Replace("##RfpNumber##", jobs.Rfp != null ? jobs.Rfp.RfpNumber : string.Empty)
                                        .Replace("##CompanyName##", jobs.Company != null ? jobs.Company.Name : "**Individual**")
                                        .Replace("##ContactName##", jobs.Contact != null ? jobs.Contact.FirstName + " " + jobs.Contact.LastName : string.Empty)
                                        .Replace("##RFPFullAddress##", jobs.Rfp != null && jobs.Rfp.RfpAddress != null ? jobs.Rfp.RfpAddress.HouseNumber + " " + jobs.Rfp.RfpAddress.Street + (jobs.Rfp.RfpAddress.Borough != null ? " " + jobs.Rfp.RfpAddress.Borough.Description : string.Empty) + " " + jobs.Rfp.RfpAddress.ZipCode : string.Empty)
                                        .Replace("##RedirectionLinkJob##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/application")
                                        .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/editSiteInformation/" + jobs.IdRfp);
                            Common.SendInAppNotifications(employeeDetail.Id, rfpConvertedToJobNotificationSetting, Hub, "job/" + jobs.Id + "/application");
                        }
                    }
                }

                string rfpConvertedToJob = InAppNotificationMessage.RFPConvertedToJob
                                .Replace("##JobNumber##", jobs.JobNumber)
                                        .Replace("##RfpNumber##", jobs.Rfp != null ? jobs.Rfp.RfpNumber : string.Empty)
                                        .Replace("##CompanyName##", jobs.Company != null ? jobs.Company.Name : "**Individual**")
                                        .Replace("##ContactName##", jobs.Contact != null ? jobs.Contact.FirstName + " " + jobs.Contact.LastName : string.Empty)
                                        .Replace("##RFPFullAddress##", jobs.Rfp != null && jobs.Rfp.RfpAddress != null ? jobs.Rfp.RfpAddress.HouseNumber + " " + jobs.Rfp.RfpAddress.Street + (jobs.Rfp.RfpAddress.Borough != null ? " " + jobs.Rfp.RfpAddress.Borough.Description : string.Empty) + " " + jobs.Rfp.RfpAddress.ZipCode : string.Empty)
                                        .Replace("##RedirectionLinkJob##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/application")
                                        .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/editSiteInformation/" + jobs.IdRfp);
                Tools.Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, EmailNotificationSubject.RFPConvertedToJob, emailBody, true);
                Common.SendInAppNotifications(employee.Id, rfpConvertedToJob, Hub, "job/" + jobs.Id + "/application");
            }
        }

        private void SendJobMarkedCompletedMail(int id, Employee emp)
        {
            Job jobs = rpoContext.Jobs.Include("RfpAddress.Borough").Include("Contact").Include("ProjectManager").Where(x => x.Id == id).FirstOrDefault();

            string jobMarkedCompletedSetting = InAppNotificationMessage.JobMarkedCompleted
                        .Replace("##JobNumber##", jobs.JobNumber)
                        .Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty)
                        .Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty)
                        .Replace("##EmployeeName##", emp != null ? emp.FirstName + " " + emp.LastName : string.Empty)
                        .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/application");

            SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.JobMarkedCompleted);
            if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
            {
                foreach (EmployeeDetail employeeDetail in systemSettingDetail.Value)
                {
                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/JobMarkedCompletedEmailTemplate.htm")))
                    {
                        body = reader.ReadToEnd();
                    }
                    string emailBody = body;

                    emailBody = emailBody.Replace("##EmployeeFirstName##", employeeDetail != null ? employeeDetail.FirstName : string.Empty);
                    emailBody = emailBody.Replace("##JobNumber##", jobs.JobNumber);
                    emailBody = emailBody.Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty);
                    emailBody = emailBody.Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty);
                    emailBody = emailBody.Replace("##EmployeeName##", emp != null ? emp.FirstName + " " + emp.LastName : string.Empty);
                    emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/application");

                    List<KeyValuePair<string, string>> to = new List<KeyValuePair<string, string>>();
                    List<KeyValuePair<string, string>> cc = new List<KeyValuePair<string, string>>();
                    to.Add(new KeyValuePair<string, string>(employeeDetail.Email, employeeDetail.EmployeeName));

                    Tools.Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, EmailNotificationSubject.JobMarkedCompleted, emailBody, true);
                    Common.SendInAppNotifications(employeeDetail.Id, jobMarkedCompletedSetting, Hub, "job/" + jobs.Id + "/application");

                }
            }
            #region ProjectTeam
            Job job = this.rpoContext.Jobs.FirstOrDefault(x => x.Id == id);

            List<int> dobProjectTeam = job != null && job.DOBProjectTeam != null && !string.IsNullOrEmpty(job.DOBProjectTeam) ? (job.DOBProjectTeam.Split(',') != null && job.DOBProjectTeam.Split(',').Any() ? job.DOBProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> dotProjectTeam = job != null && job.DOTProjectTeam != null && !string.IsNullOrEmpty(job.DOTProjectTeam) ? (job.DOTProjectTeam.Split(',') != null && job.DOTProjectTeam.Split(',').Any() ? job.DOTProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> violationProjectTeam = job != null && job.ViolationProjectTeam != null && !string.IsNullOrEmpty(job.ViolationProjectTeam) ? (job.ViolationProjectTeam.Split(',') != null && job.ViolationProjectTeam.Split(',').Any() ? job.ViolationProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> depProjectTeam = job != null && job.DEPProjectTeam != null && !string.IsNullOrEmpty(job.DEPProjectTeam) ? (job.DEPProjectTeam.Split(',') != null && job.DEPProjectTeam.Split(',').Any() ? job.DEPProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

            var result = rpoContext.Employees.Where(x => dotProjectTeam.Contains(x.Id)
                                                        || violationProjectTeam.Contains(x.Id)
                                                        || depProjectTeam.Contains(x.Id)
                                                        || dobProjectTeam.Contains(x.Id)
                                                        || x.Id == job.IdProjectManager
                                                        ).Select(x => new
                                                        {
                                                            Id = x.Id,
                                                            ItemName = x.FirstName + " " + x.LastName
                                                        }).ToArray().Distinct();
            foreach (var item in result)
            {
                Common.SendInAppNotifications(item.Id, jobMarkedCompletedSetting, Hub, "job/" + jobs.Id + "/application");
            }
            #endregion
            jobs.OnHoldCompletedDate = DateTime.Now;
        }

        private void SendJobReOpenMail(int id)
        {
            Job jobs = rpoContext.Jobs.Include("RfpAddress.Borough").Include("Contact").Include("ProjectManager").Where(x => x.Id == id).FirstOrDefault();
            List<JobAssign> jobAssignList = new List<JobAssign>();

            if (jobs.IdProjectManager != null && jobs.IdProjectManager > 0)
            {
                jobAssignList.Add(new JobAssign
                {
                    IdEmployee = jobs.IdProjectManager,
                    JobAplicationType = "DOB",
                });
            }

            if (jobs.DOBProjectTeam != null && !string.IsNullOrEmpty(jobs.DOBProjectTeam))
            {
                foreach (var item in jobs.DOBProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "DOB",
                    });
                }
            }

            if (jobs.DOTProjectTeam != null && !string.IsNullOrEmpty(jobs.DOTProjectTeam))
            {
                foreach (var item in jobs.DOTProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "DOT",
                    });
                }
            }

            if (jobs.ViolationProjectTeam != null && !string.IsNullOrEmpty(jobs.ViolationProjectTeam))
            {
                foreach (var item in jobs.ViolationProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "VIOLATION",
                    });
                }
            }

            if (jobs.DEPProjectTeam != null && !string.IsNullOrEmpty(jobs.DEPProjectTeam))
            {
                foreach (var item in jobs.DEPProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = employeeid,
                        JobAplicationType = "DEP",
                    });
                }
            }

            var employeelist = jobAssignList.Select(x => x.IdEmployee).Distinct();

            foreach (var item in employeelist)
            {
                int employeeid = Convert.ToInt32(item);
                Employee employee = this.rpoContext.Employees.FirstOrDefault(x => x.Id == employeeid);

                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/JobReOpenEmailTemplate.htm")))
                {
                    body = reader.ReadToEnd();
                }

                string jobTypeList = string.Empty;

                jobTypeList = string.Join(",", jobAssignList.Where(x => x.IdEmployee == employeeid).Select(r => r.JobAplicationType));
                string emailBody = body;

                emailBody = emailBody.Replace("##EmployeeFirstName##", employee != null ? employee.FirstName : string.Empty);
                emailBody = emailBody.Replace("##JobNumber##", jobs.JobNumber);
                emailBody = emailBody.Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty);
                emailBody = emailBody.Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty);
                emailBody = emailBody.Replace("##EmployeeName##", jobs.LastModifiedByEmployee != null ? jobs.LastModifiedByEmployee.FirstName + " " + jobs.LastModifiedByEmployee.LastName : string.Empty);
                emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/application");

                List<KeyValuePair<string, string>> to = new List<KeyValuePair<string, string>>();
                to.Add(new KeyValuePair<string, string>(employee.Email, employee.FirstName + " " + employee.LastName));

                List<KeyValuePair<string, string>> cc = new List<KeyValuePair<string, string>>();
                SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.JobReOpen);
                if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                {
                    foreach (EmployeeDetail employeeDetail in systemSettingDetail.Value)
                    {
                        if (employeeDetail.Email != employee.Email)
                        {
                            cc.Add(new KeyValuePair<string, string>(employeeDetail.Email, employeeDetail.EmployeeName));

                            string jobReOpenSetting = InAppNotificationMessage.JobReOpen
                                .Replace("##JobNumber##", jobs.JobNumber)
                                .Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty)
                                .Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty)
                                .Replace("##EmployeeName##", jobs.LastModifiedByEmployee != null ? jobs.LastModifiedByEmployee.FirstName + " " + jobs.LastModifiedByEmployee.LastName : string.Empty)
                                .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/application");
                            Common.SendInAppNotifications(employeeDetail.Id, jobReOpenSetting, Hub, "job/" + jobs.Id + "/application");
                        }
                    }
                }

                string jobReOpen = InAppNotificationMessage.JobReOpen
                                .Replace("##JobNumber##", jobs.JobNumber)
                                .Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty)
                                .Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty)
                                .Replace("##EmployeeName##", jobs.LastModifiedByEmployee != null ? jobs.LastModifiedByEmployee.FirstName + " " + jobs.LastModifiedByEmployee.LastName : string.Empty)
                                .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/application");
                Tools.Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, EmailNotificationSubject.JobReOpen, emailBody, true);
                Common.SendInAppNotifications(employee.Id, jobReOpen, Hub, "job/" + jobs.Id + "/application");
            }
            jobs.OnHoldCompletedDate = null;
        }
        /// <summary>
        /// Jobs list to export into excel.
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <param name="companyType"></param>
        /// <returns>Create a excel file and write the all Jobs detail.</returns>
        /// 
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/Jobs/exporttoexcel")]
        public IHttpActionResult JobsExport(AdvancedSearchParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewJob)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJob)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteJob))
            {
                var recordsTotal = rpoContext.Jobs.Count();
                var recordsFiltered = recordsTotal;
                string hearingDate = string.Empty;
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
                                jobs = jobs.Where(x =>
                                   (x.RfpAddress != null && x.RfpAddress.HouseNumber.Contains(item.Trim()))
                                || (x.RfpAddress != null && x.RfpAddress.Street.Contains(item.Trim()))
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
                        case GlobalSearchType.ViolationNumber:
                            var jobViolation = rpoContext.JobViolations.Where(x => x.SummonsNumber.Contains(dataTableParameters.GlobalSearchText.Trim())).Select(x => x.IdJob ?? 0).Distinct().ToList();
                            jobs = jobs.Where(x => jobViolation.Contains(x.Id));
                            break;
                    }
                }
                //else
                //{
                if (!string.IsNullOrWhiteSpace(dataTableParameters.JobNumber))
                {
                    jobs = jobs.Where(j => j.JobNumber == dataTableParameters.JobNumber);
                    string filteredJob = string.Join(", ", dataTableParameters.JobNumber);
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredJob) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredJob) ? "Job #: " + filteredJob : string.Empty);

                }
                else
                {
                    if ((dataTableParameters.OnlyMyJobs == null || dataTableParameters.OnlyMyJobs.Value))
                    {
                        if (employee != null)
                        {
                            string employeeId = Convert.ToString(employee.Id);
                            jobs = jobs.Where(j =>
                            (
                                (
                                j.DOTProjectTeam == employeeId || j.DOTProjectTeam.StartsWith(employeeId + ",") || j.DOTProjectTeam.Contains("," + employeeId + ",") || j.DOTProjectTeam.EndsWith("," + employeeId)
                                )
                                && j.DOTProjectTeam != null && j.DOTProjectTeam != ""
                            ) ||
                            (
                                (
                                j.DOBProjectTeam == employeeId || j.DOBProjectTeam.StartsWith(employeeId + ",") || j.DOBProjectTeam.Contains("," + employeeId + ",") || j.DOBProjectTeam.EndsWith("," + employeeId)
                                )
                                && j.DOBProjectTeam != null && j.DOBProjectTeam != ""
                            ) ||
                            (
                                (
                                j.ViolationProjectTeam == employeeId || j.ViolationProjectTeam.StartsWith(employeeId + ",") || j.ViolationProjectTeam.Contains("," + employeeId + ",") || j.ViolationProjectTeam.EndsWith("," + employeeId)
                                )
                                && j.ViolationProjectTeam != null && j.ViolationProjectTeam != ""
                            ) ||
                            (
                                (
                                j.DEPProjectTeam == employeeId || j.DEPProjectTeam.StartsWith(employeeId + ",") || j.DEPProjectTeam.Contains("," + employeeId + ",") || j.DEPProjectTeam.EndsWith("," + employeeId)
                                )
                                && j.DEPProjectTeam != null && j.DEPProjectTeam != ""
                            ) ||
                            j.IdProjectManager == employee.Id);
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
                        string filteredBrough = string.Join(", ", dataTableParameters.Borough);
                        hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredBrough) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredBrough) ? "Brough #: " + filteredBrough : string.Empty);

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
                        string filteredHouse = string.Join(", ", dataTableParameters.HouseNumber);
                        hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredHouse) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredHouse) ? "HouseNumber #: " + filteredHouse : string.Empty);
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

                    if (!string.IsNullOrWhiteSpace(dataTableParameters.OtherTeamMember))
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
                        string filteredSpecialPlaceName = string.Join(", ", dataTableParameters.SpecialPlaceName);
                        hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredSpecialPlaceName) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredSpecialPlaceName) ? "Special Place Name #: " + filteredSpecialPlaceName : string.Empty);
                    }

                    if (!string.IsNullOrWhiteSpace(dataTableParameters.Street))
                    {
                        jobs = jobs.Where(j => j.StreetNumber.Contains(dataTableParameters.Street) || j.RfpAddress.Street.Contains(dataTableParameters.Street));
                        string filteredStreet = string.Join(", ", dataTableParameters.Street);
                        hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredStreet) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredStreet) ? "Street #: " + filteredStreet : string.Empty);
                    }

                    if (dataTableParameters.IdCompany != null)
                    {
                        List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany.ToString()) ? (dataTableParameters.IdCompany.ToString().Split('-') != null && dataTableParameters.IdCompany.ToString().Split('-').Any() ? dataTableParameters.IdCompany.ToString().Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                        jobs = jobs.Where(j => (j.IdCompany == dataTableParameters.IdCompany) || objcompanycontactjobs.Contains(j.Id));
                        string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => jobCompanyTeam.Contains(x.Id)).Select(x => x.Name));
                        hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "Company : " + filteredCompany : string.Empty);

                    }

                    if (dataTableParameters.IdContact != null)
                    {
                        jobs = jobs.Where(j => (j.IdContact == dataTableParameters.IdContact) || objcompanycontactjobs.Contains(j.Id));
                        List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact.ToString()) ? (dataTableParameters.IdContact.ToString().Split('-') != null && dataTableParameters.IdContact.ToString().Split('-').Any() ? dataTableParameters.IdContact.ToString().Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                        string filteredContact = string.Join(", ", rpoContext.Contacts.Where(x => jobContactTeam.Contains(x.Id)).Select(x => (x.FirstName ?? "") + " " + (x.LastName ?? "")));
                        hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredContact) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredContact) ? "Contact : " + filteredContact : string.Empty);
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

                string direction = string.Empty;
                string orderBy = string.Empty;

                List<JobsDTO> result = new List<JobsDTO>();

                if (dataTableParameters.OrderedColumn != null && !string.IsNullOrEmpty(dataTableParameters.OrderedColumn.Dir))
                {
                    direction = dataTableParameters.OrderedColumn.Dir;
                }
                if (dataTableParameters.OrderedColumn != null && !string.IsNullOrEmpty(dataTableParameters.OrderedColumn.Column))
                {
                    orderBy = dataTableParameters.OrderedColumn.Column;
                }


                if (!string.IsNullOrEmpty(direction) && direction == "desc")
                {
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobs = jobs.OrderByDescending(o => o.Id);
                    if (orderBy.Trim().ToLower() == "startDate".Trim().ToLower())
                        jobs = jobs.OrderByDescending(o => o.StartDate);
                    if (orderBy.Trim().ToLower() == "floorNumber".Trim().ToLower())
                        jobs = jobs.OrderByDescending(o => o.FloorNumber);
                    if (orderBy.Trim().ToLower() == "apartment".Trim().ToLower())
                        jobs = jobs.OrderByDescending(o => o.Apartment);
                    if (orderBy.Trim().ToLower() == "Address".Trim().ToLower())
                        jobs = jobs.OrderByDescending(o => o.RfpAddress.HouseNumber).ThenByDescending(o => o.RfpAddress.Street).ThenByDescending(o => o.RfpAddress.Borough.Description);
                    if (orderBy.Trim().ToLower() == "specialPlace".Trim().ToLower())
                        jobs = jobs.OrderByDescending(o => o.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "company".Trim().ToLower())
                        jobs = jobs.OrderByDescending(o => o.Company.Name);
                    if (orderBy.Trim().ToLower() == "contact".Trim().ToLower())
                        jobs = jobs.OrderByDescending(o => o.Contact.FirstName);
                }
                else
                {
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobs = jobs.OrderBy(o => o.Id);
                    if (orderBy.Trim().ToLower() == "startDate".Trim().ToLower())
                        jobs = jobs.OrderBy(o => o.StartDate);
                    if (orderBy.Trim().ToLower() == "floorNumber".Trim().ToLower())
                        jobs = jobs.OrderBy(o => o.FloorNumber);
                    if (orderBy.Trim().ToLower() == "apartment".Trim().ToLower())
                        jobs = jobs.OrderBy(o => o.Apartment);
                    if (orderBy.Trim().ToLower() == "Address".Trim().ToLower())
                        jobs = jobs.OrderBy(o => o.RfpAddress.HouseNumber).ThenBy(o => o.RfpAddress.Street).ThenBy(o => o.RfpAddress.Borough.Description);
                    if (orderBy.Trim().ToLower() == "specialPlace".Trim().ToLower())
                        jobs = jobs.OrderBy(o => o.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "company".Trim().ToLower())
                        jobs = jobs.OrderBy(o => o.Company.Name);
                    if (orderBy.Trim().ToLower() == "contact".Trim().ToLower())
                        jobs = jobs.OrderBy(o => o.Contact.FirstName);
                }

                result = jobs
                 .AsEnumerable()
                 .Select(j => Format(j))
                 .AsQueryable()
                .ToList();

                // result = jobs
                //.AsEnumerable()
                //.Select(j => Format(j))
                //.AsQueryable()
                //.DataTableParameters(dataTableParameters, out recordsFiltered)
                //.ToList();

                string exportFilename = "ProjectsReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                string exportFilePath = ExportToExcel(hearingDate, result, exportFilename);
                FileInfo fileinfo = new FileInfo(exportFilePath);
                long fileinfoSize = fileinfo.Length;

                var downloadFilePath = Properties.Settings.Default.APIUrl + "/" + Properties.Settings.Default.ReportExcelExportPath + "/" + exportFilename;
                List<KeyValuePair<string, string>> fileResult = new List<KeyValuePair<string, string>>();
                fileResult.Add(new KeyValuePair<string, string>("exportFilePath", downloadFilePath));
                fileResult.Add(new KeyValuePair<string, string>("exportFilesize", Convert.ToString(fileinfoSize)));

                return Ok(fileResult);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        /// Create a excel File and write companies List.
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="exportFilename"></param>
        /// <returns>exportFilename path</returns>
        private string ExportToExcel(string hearingDate, List<JobsDTO> result, string exportFilename)
        {
            string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
            string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string templateFileName = "Projects_ReportExportTemplate - Copy.xlsx";
            string templateFilePath = templatePath + templateFileName;
            string exportFilePath = path + exportFilename;
            File.Delete(exportFilePath);

            XSSFWorkbook templateWorkbook = new XSSFWorkbook(templateFilePath);
            ISheet sheet = templateWorkbook.GetSheet("Sheet1");

            XSSFFont myFont = (XSSFFont)templateWorkbook.CreateFont();
            myFont.FontHeightInPoints = (short)12;
            myFont.FontName = "Times New Roman";
            myFont.IsBold = false;
            XSSFFont Center_myFont = (XSSFFont)templateWorkbook.CreateFont();
            Center_myFont.FontHeightInPoints = (short)14;
            Center_myFont.FontName = "Times New Roman";
            Center_myFont.IsBold = true;



            XSSFCellStyle leftAlignCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
            leftAlignCellStyle.SetFont(myFont);
            leftAlignCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            leftAlignCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            leftAlignCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            leftAlignCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            leftAlignCellStyle.WrapText = true;
            leftAlignCellStyle.VerticalAlignment = VerticalAlignment.Top;
            leftAlignCellStyle.Alignment = HorizontalAlignment.Left;

            XSSFCellStyle rightAlignCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
            rightAlignCellStyle.SetFont(myFont);
            rightAlignCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            rightAlignCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            rightAlignCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            rightAlignCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            rightAlignCellStyle.WrapText = true;
            rightAlignCellStyle.VerticalAlignment = VerticalAlignment.Top;
            rightAlignCellStyle.Alignment = HorizontalAlignment.Right;
            XSSFCellStyle CenterAlignCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
            CenterAlignCellStyle.SetFont(Center_myFont);
            //CenterAlignCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            //CenterAlignCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            //CenterAlignCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            //CenterAlignCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            CenterAlignCellStyle.WrapText = true;
            CenterAlignCellStyle.VerticalAlignment = VerticalAlignment.Center;
            CenterAlignCellStyle.Alignment = HorizontalAlignment.Center;

            IRow iDateRow = sheet.GetRow(2);
            if (iDateRow != null)
            {
                if (iDateRow.GetCell(0) != null)
                {
                    iDateRow.GetCell(0).SetCellValue(hearingDate);
                }
                else
                {
                    iDateRow.CreateCell(0).SetCellValue(hearingDate);
                }
            }
            else
            {
                iDateRow = sheet.CreateRow(2);
                if (iDateRow.GetCell(0) != null)
                {
                    iDateRow.GetCell(0).SetCellValue(hearingDate);
                }
                else
                {
                    iDateRow.CreateCell(0).SetCellValue(hearingDate);
                }
            }
            string reportDate = "Date:" + DateTime.Today.ToString(Common.ExportReportDateFormat);
            IRow iReportDateRow = sheet.GetRow(3);
            //if (iReportDateRow != null)
            //{
            //    if (iReportDateRow.GetCell(4) != null)
            //    {
            //        iReportDateRow.GetCell(4).SetCellValue(reportDate);
            //    }
            //    else
            //    {
            //        iReportDateRow.CreateCell(4).SetCellValue(reportDate);
            //    }
            //}
            //mb job status notes
            if (iReportDateRow != null)
            {
                if (iReportDateRow.GetCell(5) != null)
                {
                    iReportDateRow.GetCell(5).SetCellValue(reportDate);
                }
                else
                {
                    iReportDateRow.CreateCell(5).SetCellValue(reportDate);
                }
            }
            else
            {
                iReportDateRow = sheet.CreateRow(3);
                if (iReportDateRow.GetCell(5) != null)
                {
                    iReportDateRow.GetCell(5).SetCellValue(reportDate);
                }
                else
                {
                    iReportDateRow.CreateCell(5).SetCellValue(reportDate);
                }
            }
            //else
            //{
            //    iReportDateRow = sheet.CreateRow(3);
            //    if (iReportDateRow.GetCell(4) != null)
            //    {
            //        iReportDateRow.GetCell(4).SetCellValue(reportDate);
            //    }
            //    else
            //    {
            //        iReportDateRow.CreateCell(4).SetCellValue(reportDate);
            //    }
            //}

            int index = 5;
            foreach (JobsDTO item in result)
            {
                IRow iRow = sheet.GetRow(index);
                if (iRow != null)
                {
                    if (iRow.GetCell(0) != null)
                    {
                        iRow.GetCell(0).SetCellValue(item.JobNumber);
                    }
                    else
                    {
                        iRow.CreateCell(0).SetCellValue(item.JobNumber);
                    }

                    if (iRow.GetCell(1) != null)
                    {
                        iRow.GetCell(1).SetCellValue(item.SpecialPlace);
                    }
                    else
                    {
                        iRow.CreateCell(1).SetCellValue(item.SpecialPlace);
                    }

                    if (iRow.GetCell(2) != null)
                    {
                        iRow.GetCell(2).SetCellValue(item.Address);
                    }
                    else
                    {
                        iRow.CreateCell(2).SetCellValue(item.Address);
                    }

                    if (iRow.GetCell(3) != null)
                    {
                        iRow.GetCell(3).SetCellValue(item.Company);
                    }
                    else
                    {
                        iRow.CreateCell(3).SetCellValue(item.Company);
                    }
                    if (iRow.GetCell(4) != null)
                    {
                        iRow.GetCell(4).SetCellValue(item.Contact);
                    }
                    else
                    {
                        iRow.CreateCell(4).SetCellValue(item.Contact);
                    }
                    //mb job status notes
                    //if (iRow.GetCell(5) != null)
                    //{
                    //    iRow.GetCell(5).SetCellValue(item.JobStatusNotes);
                    //    //iRow.GetCell(4).SetCellValue(dr["Contact"].ToString());
                    //}
                    //else
                    //{
                    //    iRow.CreateCell(5).SetCellValue(item.JobStatusNotes);
                    //    // iRow.CreateCell(4).SetCellValue(dr["Contact"].ToString());
                    //}
                    string hightLights = string.Empty;
                    if (!string.IsNullOrEmpty(item.ProjectHighlights))
                    {
                        hightLights = "Date " + Convert.ToDateTime(item.HighlightLastModiefiedDate).ToString("MM/dd/yyyy") + " " + item.ProjectHighlights;
                    }

                    if (iRow.GetCell(5) != null)
                    {
                        iRow.GetCell(5).SetCellValue(hightLights);
                    }
                    else
                    {
                        iRow.CreateCell(5).SetCellValue(hightLights);
                    }


                    iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                    //mb job status notes
                    iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                }
                else
                {
                    iRow = sheet.CreateRow(index);


                    if (iRow.GetCell(0) != null)
                    {
                        iRow.GetCell(0).SetCellValue(item.JobNumber);
                        //iRow.GetCell(0).SetCellValue(dr["JobNumber"].ToString());                      
                    }
                    else
                    {
                        iRow.CreateCell(0).SetCellValue(item.JobNumber);
                        //iRow.CreateCell(0).SetCellValue(dr["JobNumber"].ToString());
                    }

                    if (iRow.GetCell(1) != null)
                    {
                        iRow.GetCell(1).SetCellValue(item.SpecialPlace);
                        // iRow.GetCell(1).SetCellValue(dr["SpecialPlace"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(1).SetCellValue(item.SpecialPlace);
                        //iRow.CreateCell(1).SetCellValue(dr["SpecialPlace"].ToString());
                    }

                    if (iRow.GetCell(2) != null)
                    {
                        iRow.GetCell(2).SetCellValue(item.Address);
                        //iRow.GetCell(2).SetCellValue(dr["RfpAddress"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(2).SetCellValue(item.Address);
                        //iRow.CreateCell(2).SetCellValue(dr["RfpAddress"].ToString());
                    }

                    if (iRow.GetCell(3) != null)
                    {
                        iRow.GetCell(3).SetCellValue(item.Company);
                        //iRow.GetCell(3).SetCellValue(dr["Company"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(3).SetCellValue(item.Company);
                        //iRow.CreateCell(3).SetCellValue(dr["Company"].ToString());
                    }
                    if (iRow.GetCell(4) != null)
                    {
                        iRow.GetCell(4).SetCellValue(item.Contact);
                        //iRow.GetCell(4).SetCellValue(dr["Contact"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(4).SetCellValue(item.Contact);
                        // iRow.CreateCell(4).SetCellValue(dr["Contact"].ToString());
                    }
                    //mb job status notes
                    //if (iRow.GetCell(5) != null)
                    //{
                    //    iRow.GetCell(5).SetCellValue(item.JobStatusNotes);
                    //}
                    //else
                    //{
                    //    iRow.CreateCell(5).SetCellValue(item.JobStatusNotes);
                    //}
                    string hightLights = string.Empty;
                    if (!string.IsNullOrEmpty(item.ProjectHighlights))
                    {
                        hightLights = "Date " + Convert.ToDateTime(item.HighlightLastModiefiedDate).ToString("MM/dd/yyyy") + " " + item.ProjectHighlights;
                    }
                    if (iRow.GetCell(5) != null)
                    {
                        iRow.GetCell(5).SetCellValue(hightLights);
                    }
                    else
                    {
                        iRow.CreateCell(5).SetCellValue(hightLights);
                    }

                    iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                    //mb job status notes
                    iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                    //iRow.GetCell(6).CellStyle = leftAlignCellStyle;

                }

                index = index + 1;
            }

            using (var file2 = new FileStream(exportFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                templateWorkbook.Write(file2);
                file2.Close();
            }

            return exportFilePath;
        }
        /// <summary>
        /// Get the report of Application Status Report with filter and sorting and export to pdf
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/Jobs/exporttopdf")]
        public IHttpActionResult JobExportPdf(AdvancedSearchParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewJob)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJob)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteJob))
            {
                var recordsTotal = rpoContext.Jobs.Count();
                var recordsFiltered = recordsTotal;
                string hearingDate = string.Empty;
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
                                jobs = jobs.Where(x =>
                                   (x.RfpAddress != null && x.RfpAddress.HouseNumber.Contains(item.Trim()))
                                || (x.RfpAddress != null && x.RfpAddress.Street.Contains(item.Trim()))
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
                        case GlobalSearchType.ViolationNumber:
                            var jobViolation = rpoContext.JobViolations.Where(x => x.SummonsNumber.Contains(dataTableParameters.GlobalSearchText.Trim())).Select(x => x.IdJob ?? 0).Distinct().ToList();
                            jobs = jobs.Where(x => jobViolation.Contains(x.Id));
                            break;
                    }
                }
                //else
                //{
                if (!string.IsNullOrWhiteSpace(dataTableParameters.JobNumber))
                {
                    jobs = jobs.Where(j => j.JobNumber == dataTableParameters.JobNumber);
                    string filteredJob = string.Join(", ", dataTableParameters.JobNumber);
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredJob) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredJob) ? "Job #: " + filteredJob : string.Empty);
                }
                else
                {
                    if ((dataTableParameters.OnlyMyJobs == null || dataTableParameters.OnlyMyJobs.Value))
                    {
                        if (employee != null)
                        {
                            string employeeId = Convert.ToString(employee.Id);
                            jobs = jobs.Where(j =>
                            (
                                (
                                j.DOTProjectTeam == employeeId || j.DOTProjectTeam.StartsWith(employeeId + ",") || j.DOTProjectTeam.Contains("," + employeeId + ",") || j.DOTProjectTeam.EndsWith("," + employeeId)
                                )
                                && j.DOTProjectTeam != null && j.DOTProjectTeam != ""
                            ) ||
                            (
                                (
                                j.DOBProjectTeam == employeeId || j.DOBProjectTeam.StartsWith(employeeId + ",") || j.DOBProjectTeam.Contains("," + employeeId + ",") || j.DOBProjectTeam.EndsWith("," + employeeId)
                                )
                                && j.DOBProjectTeam != null && j.DOBProjectTeam != ""
                            ) ||
                            (
                                (
                                j.ViolationProjectTeam == employeeId || j.ViolationProjectTeam.StartsWith(employeeId + ",") || j.ViolationProjectTeam.Contains("," + employeeId + ",") || j.ViolationProjectTeam.EndsWith("," + employeeId)
                                )
                                && j.ViolationProjectTeam != null && j.ViolationProjectTeam != ""
                            ) ||
                            (
                                (
                                j.DEPProjectTeam == employeeId || j.DEPProjectTeam.StartsWith(employeeId + ",") || j.DEPProjectTeam.Contains("," + employeeId + ",") || j.DEPProjectTeam.EndsWith("," + employeeId)
                                )
                                && j.DEPProjectTeam != null && j.DEPProjectTeam != ""
                            ) ||
                            j.IdProjectManager == employee.Id);
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
                        string filteredBrough = string.Join(", ", dataTableParameters.Borough);
                        hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredBrough) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredBrough) ? "Brough #: " + filteredBrough : string.Empty);

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
                        string filteredHouse = string.Join(", ", dataTableParameters.HouseNumber);
                        hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredHouse) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredHouse) ? "HouseNumber #: " + filteredHouse : string.Empty);

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

                    if (!string.IsNullOrWhiteSpace(dataTableParameters.OtherTeamMember))
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

                        string filteredSpecialPlaceName = string.Join(", ", dataTableParameters.SpecialPlaceName);
                        hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredSpecialPlaceName) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredSpecialPlaceName) ? "Special Place Name #: " + filteredSpecialPlaceName : string.Empty);
                    }

                    if (!string.IsNullOrWhiteSpace(dataTableParameters.Street))
                    {
                        jobs = jobs.Where(j => j.StreetNumber.Contains(dataTableParameters.Street) || j.RfpAddress.Street.Contains(dataTableParameters.Street));

                        string filteredStreet = string.Join(", ", dataTableParameters.Street);
                        hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredStreet) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredStreet) ? "Street #: " + filteredStreet : string.Empty);

                    }

                    if (dataTableParameters.IdCompany != null)
                    {
                        List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany.ToString()) ? (dataTableParameters.IdCompany.ToString().Split('-') != null && dataTableParameters.IdCompany.ToString().Split('-').Any() ? dataTableParameters.IdCompany.ToString().Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

                        jobs = jobs.Where(j => (j.IdCompany == dataTableParameters.IdCompany) || objcompanycontactjobs.Contains(j.Id));

                        string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => jobCompanyTeam.Contains(x.Id)).Select(x => x.Name));
                        hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "Company : " + filteredCompany : string.Empty);

                    }

                    if (dataTableParameters.IdContact != null)
                    {
                        jobs = jobs.Where(j => (j.IdContact == dataTableParameters.IdContact) || objcompanycontactjobs.Contains(j.Id));
                        List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact.ToString()) ? (dataTableParameters.IdContact.ToString().Split('-') != null && dataTableParameters.IdContact.ToString().Split('-').Any() ? dataTableParameters.IdContact.ToString().Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                        string filteredContact = string.Join(", ", rpoContext.Contacts.Where(x => jobContactTeam.Contains(x.Id)).Select(x => (x.FirstName ?? "") + " " + (x.LastName ?? "")));
                        hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredContact) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredContact) ? "Contact : " + filteredContact : string.Empty);

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

                string direction = string.Empty;
                string orderBy = string.Empty;

                List<JobsDTO> result = new List<JobsDTO>();

                if (dataTableParameters.OrderedColumn != null && !string.IsNullOrEmpty(dataTableParameters.OrderedColumn.Dir))
                {
                    direction = dataTableParameters.OrderedColumn.Dir;
                }
                if (dataTableParameters.OrderedColumn != null && !string.IsNullOrEmpty(dataTableParameters.OrderedColumn.Column))
                {
                    orderBy = dataTableParameters.OrderedColumn.Column;
                }



                if (!string.IsNullOrEmpty(direction) && direction == "desc")
                {
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobs = jobs.OrderByDescending(o => o.Id);
                    if (orderBy.Trim().ToLower() == "startDate".Trim().ToLower())
                        jobs = jobs.OrderByDescending(o => o.StartDate);
                    if (orderBy.Trim().ToLower() == "floorNumber".Trim().ToLower())
                        jobs = jobs.OrderByDescending(o => o.FloorNumber);
                    if (orderBy.Trim().ToLower() == "apartment".Trim().ToLower())
                        jobs = jobs.OrderByDescending(o => o.Apartment);
                    if (orderBy.Trim().ToLower() == "Address".Trim().ToLower())
                        jobs = jobs.OrderByDescending(o => o.RfpAddress.HouseNumber).ThenByDescending(o => o.RfpAddress.Street).ThenByDescending(o => o.RfpAddress.Borough.Description);
                    if (orderBy.Trim().ToLower() == "specialPlace".Trim().ToLower())
                        jobs = jobs.OrderByDescending(o => o.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "company".Trim().ToLower())
                        jobs = jobs.OrderByDescending(o => o.Company.Name);
                    if (orderBy.Trim().ToLower() == "contact".Trim().ToLower())
                        jobs = jobs.OrderByDescending(o => o.Contact.FirstName);
                }
                else
                {
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobs = jobs.OrderBy(o => o.Id);
                    if (orderBy.Trim().ToLower() == "startDate".Trim().ToLower())
                        jobs = jobs.OrderBy(o => o.StartDate);
                    if (orderBy.Trim().ToLower() == "floorNumber".Trim().ToLower())
                        jobs = jobs.OrderBy(o => o.FloorNumber);
                    if (orderBy.Trim().ToLower() == "apartment".Trim().ToLower())
                        jobs = jobs.OrderBy(o => o.Apartment);
                    if (orderBy.Trim().ToLower() == "Address".Trim().ToLower())
                        jobs = jobs.OrderBy(o => o.RfpAddress.HouseNumber).ThenBy(o => o.RfpAddress.Street).ThenBy(o => o.RfpAddress.Borough.Description);
                    if (orderBy.Trim().ToLower() == "specialPlace".Trim().ToLower())
                        jobs = jobs.OrderBy(o => o.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "company".Trim().ToLower())
                        jobs = jobs.OrderBy(o => o.Company.Name);
                    if (orderBy.Trim().ToLower() == "contact".Trim().ToLower())
                        jobs = jobs.OrderBy(o => o.Contact.FirstName);
                }


                result = jobs
                 .AsEnumerable()
                 .Select(j => Format(j))
                 .AsQueryable()
                .ToList();

                string exportFilename = "ProjectsReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                string exportFilePath = ExportToPdf(hearingDate, result, exportFilename);
                FileInfo fileinfo = new FileInfo(exportFilePath);
                long fileinfoSize = fileinfo.Length;

                var downloadFilePath = Properties.Settings.Default.APIUrl + "/" + Properties.Settings.Default.ReportExcelExportPath + "/" + exportFilename;
                List<KeyValuePair<string, string>> fileResult = new List<KeyValuePair<string, string>>();
                fileResult.Add(new KeyValuePair<string, string>("exportFilePath", downloadFilePath));
                fileResult.Add(new KeyValuePair<string, string>("exportFilesize", Convert.ToString(fileinfoSize)));

                return Ok(fileResult);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        /// create file and Export to pdf
        /// </summary>
        /// <param name="hearingDate"></param>
        /// <param name="result"></param>
        /// <param name="exportFilename"></param>
        /// <returns></returns>
        private string ExportToPdf(string hearingDate, List<JobsDTO> result, string exportFilename)
        {

            string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
            string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string exportFilePath = path + exportFilename;
            File.Delete(exportFilePath);

            using (MemoryStream stream = new MemoryStream())
            {
                Document document = new Document(PageSize.LETTER.Rotate());
                document.SetMargins(18, 18, 12, 36);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(exportFilePath, FileMode.Create));
                writer.PageEvent = new Header();
                document.Open();

                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~\\HTML\\images\\logo-new.jpg"));
                logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                logo.ScaleToFit(120, 60);
                logo.SetAbsolutePosition(260, 760);

                Font font_10_Normal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, 0);
                Font font_12_Normal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 0);

                Font font_10_Bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, 1);
                Font font_12_Bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 1);
                Font font_16_Bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 16, 1);

                Font font_Table_Header = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, 1);
                Font font_Table_Data = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, 0);

                //mb job status 5-6
                PdfPTable table = new PdfPTable(6);
                //PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.SplitLate = false;

                string reportHeader = //"RPO INCORPORATED" + Environment.NewLine +
                   "146 West 29th Street, Suite 2E"
                   + Environment.NewLine + "New York, NY 10001"
                   + Environment.NewLine + "(212) 566-5110"
                   + Environment.NewLine + "www.rpoinc.com";

                PdfPCell cell = new PdfPCell(logo);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Rowspan = 2;
                cell.PaddingBottom = 5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Construction Consultants", font_16_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -65;
                cell.Colspan = 6;
                cell.PaddingTop = -5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportHeader, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -65;
                cell.Colspan = 6;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.PaddingBottom = -10;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("PROJECTS REPORT", font_16_Bold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.TOP_BORDER;
                cell.Colspan = 6;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(hearingDate, font_12_Bold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.TOP_BORDER;
                cell.Colspan = 6;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 6;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 6;
                //cell.Colspan = 4;
                table.AddCell(cell);


                string reportDate = "Date:" + DateTime.Today.ToString(Common.ExportReportDateFormat);

                cell = new PdfPCell(new Phrase(reportDate, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Border = PdfPCell.NO_BORDER;
                //cell.Colspan = 1;
                cell.Colspan = 6;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Project #", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Special Place Name", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Address", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Company Name", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Contact", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                //cell = new PdfPCell(new Phrase("Project status Notes", font_Table_Header));
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.Border = PdfPCell.BOX;
                //cell.Colspan = 1;
                //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                //table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Project Highlights", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);


                foreach (JobsDTO item in result)
                {
                    cell = new PdfPCell(new Phrase(item.JobNumber, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.SpecialPlace, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Address, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Company, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Contact, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    //cell = new PdfPCell(new Phrase(item.JobStatusNotes, font_Table_Data));
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.Border = PdfPCell.BOX;
                    //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //table.AddCell(cell);
                    string hightLights = string.Empty;
                    if (!string.IsNullOrEmpty(item.ProjectHighlights))
                    {
                        hightLights = "Date " + Convert.ToDateTime(item.HighlightLastModiefiedDate).ToString("MM/dd/yyyy") + " " + item.ProjectHighlights; }
                    cell = new PdfPCell(new Phrase(hightLights, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);
                }

                document.Add(table);
                document.Close();

                writer.Close();
            }

            return exportFilePath;
        }

        public partial class Header : PdfPageEventHelper
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Header"/> class.
            /// </summary>
            /// <param name="FirstLastName">First name of the last.</param>
            /// <param name="proposalName">Name of the proposal.</param>
            public Header()
            {
            }

            /// <summary>
            /// Called when [end page].
            /// </summary>
            /// <param name="writer">The writer.</param>
            /// <param name="doc">The document.</param>
            public override void OnEndPage(PdfWriter writer, Document doc)
            {
                base.OnEndPage(writer, doc);

                int pageNumber = writer.PageNumber;

                iTextSharp.text.Image SnapCorLogo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~\\HTML\\images\\logo-footer.jpg"));
                SnapCorLogo.Alignment = iTextSharp.text.Image.ALIGN_RIGHT;
                SnapCorLogo.ScaleToFit(112, 32);
                SnapCorLogo.SetAbsolutePosition(260, 760);

                PdfPTable table = new PdfPTable(1);

                table.TotalWidth = doc.PageSize.Width - 80f;
                table.WidthPercentage = 70;

                PdfPCell cell = new PdfPCell(new Phrase("" + pageNumber, new Font(Font.FontFamily.TIMES_ROMAN, 12)));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                //cell.PaddingTop = 10f;
                table.AddCell(cell);


                cell = new PdfPCell(SnapCorLogo);
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingTop = -15;
                table.AddCell(cell);

                table.WriteSelectedRows(0, -1, 40, doc.Bottom, writer.DirectContent);
                //doc.Add(table);
                //table.WriteSelectedRows(0, -1, 0, doc.Bottom, writer.DirectContent);
            }
        }


        /// <summary>
        /// .
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Get the jobs List.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/Jobs/GetDropBoxFolder")]
        public IHttpActionResult CreateDropBoxFolder()
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewJob)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJob)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteJob))
            {
                // var job1 = rpoContext.Jobs.Join(rpoContext.JobDocuments, r => r.Id, p => p.IdJob, (r, p) => new { p.IdJob }).ToList().Distinct();
                // var job2 = rpoContext.Jobs.Select(p => p.Id).ToList().Distinct();
                var job = rpoContext.Jobs.Where(c => !rpoContext.JobDocuments.Select(b => b.IdJob).Contains(c.Id)).Select(c => c.Id).ToList();
                if (job == null)
                {
                    return this.NotFound();
                }
                foreach (var jobItem in job)
                {
                    var instance = new DropboxIntegration();
                    string uploadFilePath = Properties.Settings.Default.DropboxFolderPath + Convert.ToString(jobItem);
                    var isFolderExists = instance.RunFolderExists(uploadFilePath);
                    if (isFolderExists == "1")
                    {
                        //instance.RunCreateFolder(uploadFilePath + "/DOB");
                        //instance.RunCreateFolder(uploadFilePath + "/DOT");
                        //instance.RunCreateFolder(uploadFilePath + "/VIO");
                    }
                }
                return Ok(new DataTableResponse
                {
                    RecordsTotal = job.Count,
                    Data = job.Distinct()
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }


        //Customer API
        /// <summary>
        /// Gets the jobs.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Get the job List against advance search filters</returns>
        [HttpPost]
        [ResponseType(typeof(DataTableParameters))]
        [Authorize]
        [RpoAuthorize]
        //  [ResponseType(typeof(List<ContactList>))]
        [Route("api/CustomerJobsListPost/{IdCustomer}")]
        public IHttpActionResult GetCustomerJobsPostList(DataTableParameters dataTableParameters, int IdCustomer)
        {
            var customer = this.rpoContext.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(customer.Permissions, Enums.Permission.ViewJob))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();
                string Firstname = string.Empty;
                string Lastname = string.Empty;

                DataSet ds = new DataSet();

                SqlParameter[] spParameter = new SqlParameter[7];

                spParameter[0] = new SqlParameter("@PageIndex", SqlDbType.Int);
                spParameter[0].Direction = ParameterDirection.Input;
                spParameter[0].Value = dataTableParameters.Start;

                spParameter[1] = new SqlParameter("@PageSize", SqlDbType.Int);
                spParameter[1].Direction = ParameterDirection.Input;
                spParameter[1].Value = dataTableParameters.Length;

                spParameter[2] = new SqlParameter("@Column", SqlDbType.NVarChar, 50);
                spParameter[2].Direction = ParameterDirection.Input;
                spParameter[2].Value = dataTableParameters.OrderedColumn != null ? dataTableParameters.OrderedColumn.Column : null;

                spParameter[3] = new SqlParameter("@Dir", SqlDbType.VarChar, 50);
                spParameter[3].Direction = ParameterDirection.Input;
                spParameter[3].Value = dataTableParameters.OrderedColumn != null ? dataTableParameters.OrderedColumn.Dir : null;


                spParameter[4] = new SqlParameter("@Search", SqlDbType.NVarChar, 50);
                spParameter[4].Direction = ParameterDirection.Input;
                spParameter[4].Value = !string.IsNullOrEmpty(dataTableParameters.Search) ? dataTableParameters.Search : string.Empty;

                spParameter[5] = new SqlParameter("@customerId", SqlDbType.VarChar, 50);
                spParameter[5].Direction = ParameterDirection.Input;
                spParameter[5].Value = IdCustomer;

                spParameter[6] = new SqlParameter("@RecordCount", SqlDbType.Int);
                spParameter[6].Direction = ParameterDirection.Output;



                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "Jobs_CustomerList", spParameter);



                int totalrecord = 0;
                int Recordcount = 0;
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    totalrecord = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                    Recordcount = int.Parse(spParameter[6].SqlValue.ToString());

                }
                if (ds.Tables.Count > 1)
                {
                    ds.Tables[1].TableName = "Data";
                }
                return Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Start,
                    RecordsFiltered = Recordcount,
                    RecordsTotal = totalrecord,
                    Data = ds.Tables[1]
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        /// Gets the job.
        /// </summary>
        /// <remarks>To get the detail of jobs and its references  all the detail load</remarks>
        /// <param name="idJob">The identifier.</param>
        /// <returns>Get the job detail.</returns>
        [ResponseType(typeof(Job))]
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        // [Route("api/Jobs/{idJob}/Customerdetails")]
        [Route("api/Jobs/{idJob}/CustomerJobdetails")]
        public IHttpActionResult GetCustomerJobDetails(int idJob)
        {
            var customer = this.rpoContext.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(customer.Permissions, Enums.Permission.ViewJob))
            {
                Job job = rpoContext
                .Jobs
                .Include("Applications")
                .Include("RfpAddress.Borough")
                .Include("RfpAddress.OwnerType")
                .Include("RfpAddress.Company")
                .Include("RfpAddress.OwnerContact")
                .Include("RfpAddress.OccupancyClassification")
                .Include("RfpAddress.ConstructionClassification")
                .Include("RfpAddress.MultipleDwellingClassification")
                .Include("RfpAddress.PrimaryStructuralSystem")
                .Include("RfpAddress.StructureOccupancyCategory")
                .Include("RfpAddress.SeismicDesignCategory")
                .Include("RfpAddress.SecondOfficer")
                .Include("Rfp")
                .Include("Borough")
                .Include("Company")
                .Include("Company.Addresses")
                .Include("Company.CompanyTypes")
                .Include("JobApplicationTypes")
                .Include("Contact")
                .Include("Contact.Prefix")
                .Include("ProjectManager")
                //.Include("ProjectCoordinator")
                //.Include("SignoffCoordinator")
                .Include("Contacts")
                .Include("Documents")
                .Include("Transmittals")
                .Include("Tasks")
                .FirstOrDefault(j => j.Id == idJob);

                if (job == null)
                {
                    return this.NotFound();
                }

                rpoContext.Configuration.LazyLoadingEnabled = false;

                return this.Ok(this.FormatDetailsWithoutObject(job));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        /// Posts the job.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>IHttpActionResult.</returns>
        // [ResponseType(typeof(JobsDTO))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Jobs/PostCustomerJobName/{IdJob}/{ProjectName}")]
        public IHttpActionResult PostCustomerJobName(int IdJob, string ProjectName)
        {
            var customer = this.rpoContext.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);

            var customerjobaccess = rpoContext.CustomerJobAccess.Where(x => x.IdJob == IdJob && x.IdCustomer == customer.Id).FirstOrDefault();
            var CustomerJobNamedb = rpoContext.CustomerJobNames.Where(x => x.IdCustomerJobAccess == customerjobaccess.Id).FirstOrDefault();
            if (CustomerJobNamedb == null)
            {
                CustomerJobName CustomerJobName = new CustomerJobName();
                CustomerJobName.IdCustomerJobAccess = rpoContext.CustomerJobAccess.Where(x => x.IdJob == IdJob && x.IdCustomer == customer.Id).Select(x => x.Id).FirstOrDefault();
                CustomerJobName.CreatedDate = DateTime.UtcNow;
                CustomerJobName.ProjectName = ProjectName;
                rpoContext.CustomerJobNames.Add(CustomerJobName);
            }
            else
            {
                CustomerJobNamedb.ProjectName = ProjectName;
                rpoContext.Entry(CustomerJobNamedb).State = EntityState.Modified;
            }
            try
            {
                rpoContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new RpoBusinessException("Error in adding Project Name");
            }
            return Ok("Project Name Saved Successfully");
        }
        [ResponseType(typeof(CustomerJobName))]
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Jobs/GetJobName/{idJob}")]
        public IHttpActionResult GetJobName(int idJob)
        {
            var customer = this.rpoContext.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            var CustomerJobAccess = rpoContext.CustomerJobAccess.Where(x => x.IdJob == idJob && x.IdCustomer == customer.Id).FirstOrDefault();
            if (CustomerJobAccess != null)
            {
                CustomerJobName CustomerJobNames = rpoContext.CustomerJobNames.Where(x => x.IdCustomerJobAccess == CustomerJobAccess.Id).FirstOrDefault();
                if (CustomerJobNames != null)
                {
                    return this.Ok(CustomerJobNames.ProjectName);
                }
                return this.NotFound();
            }
            else
                return Ok();
        }
    }
}

