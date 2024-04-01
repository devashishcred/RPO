
namespace Rpo.ApiServices.Api.Controllers.JobContactGroups
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using DataTable;
    using Filters;
    using Model.Models.Enums;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;

    public class JobContactGroupsController : ApiController
    {

        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Get The List of job contact groups list
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns>List of job contact groups list</returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetJobContactGroups([FromUri] JobContactGroupDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            //if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
            //      || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
            //      || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            //{
                var jobContactGroups = this.rpoContext.JobContactGroups.Include("CreatedByEmployee").Include("LastModifiedByEmployee").Where(x => x.IdJob == dataTableParameters.IdJob).AsQueryable();

                var recordsTotal = jobContactGroups.Count();
                var recordsFiltered = recordsTotal;

                var result = jobContactGroups
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
                    Data = result
                });
            //}
            //else
            //{
            //    throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            //}
        }
        /// <summary>
        /// The List of job contact groups list against the job
        /// </summary>
        /// <param name="idJob"></param>
        /// <returns> List of job contact groups list against the job</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobcontactgroups/dropdown/{idJob}")]
        public IHttpActionResult GetJobContactGroupDropdown(int idJob)
        {
            var result = this.rpoContext.JobContactGroups.Where(x => x.IdJob == idJob).AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name,
            }).ToArray();

            return this.Ok(result);
        }
        /// <summary>
        /// Get the detail of job contact group
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Get the detail of job contact group</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobContactGroupDetail))]
        public IHttpActionResult GetJobContactGroup(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            //if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
            //      || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
            //      || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            //{
                JobContactGroup jobContactGroup = this.rpoContext.JobContactGroups.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (jobContactGroup == null)
                {
                    return this.NotFound();
                }

                return this.Ok(this.FormatDetails(jobContactGroup));
            //}
            //else
            //{
            //    throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            //}
        }
        /// <summary>
        /// Update the detail of job contact group
        /// </summary>
        /// <param name="id"></param>
        /// <param name="jobContactGroup"></param>
        /// <returns>Update the detail of job contact group</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutJobContactGroup(int id, JobContactGroup jobContactGroup)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            //if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            //{
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (id != jobContactGroup.Id)
                {
                    return this.BadRequest();
                }

                if (this.JobContactGroupNameExists(jobContactGroup.Name, jobContactGroup.Id, jobContactGroup.IdJob))
                {
                    throw new RpoBusinessException(StaticMessages.JobContactGroupNameExistsMessage);
                }

                jobContactGroup.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobContactGroup.LastModifiedBy = employee.Id;
                }

                this.rpoContext.Entry(jobContactGroup).State = EntityState.Modified;

                try
                {
                    this.rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.JobContactGroupExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                JobContactGroup jobContactGroupResponse = this.rpoContext.JobContactGroups.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return this.Ok(this.FormatDetails(jobContactGroupResponse));
            //}
            //else
            //{
            //    throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            //}
        }
        /// <summary>
        /// Create a New job contact group
        /// </summary>
        /// <param name="jobContactGroup"></param>
        /// <returns>Create a New job contact group</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobContactGroupDetail))]
        public IHttpActionResult PostJobContactGroup(JobContactGroup jobContactGroup)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            //if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            //{
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (this.JobContactGroupNameExists(jobContactGroup.Name, jobContactGroup.Id, jobContactGroup.IdJob))
                {
                    throw new RpoBusinessException(StaticMessages.JobContactGroupNameExistsMessage);
                }

                jobContactGroup.LastModifiedDate = DateTime.UtcNow;
                jobContactGroup.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobContactGroup.CreatedBy = employee.Id;
                }

                this.rpoContext.JobContactGroups.Add(jobContactGroup);
                this.rpoContext.SaveChanges();

                JobContactGroup jobContactGroupResponse = this.rpoContext.JobContactGroups.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == jobContactGroup.Id);
                return this.Ok(this.FormatDetails(jobContactGroupResponse));
            //}
            //else
            //{
            //    throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            //}
        }
        /// <summary>
        /// Detete the job contact group
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Detete the job contact group</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobContactGroup))]
        public IHttpActionResult DeleteJobContactGroup(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            //if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            //{
                JobContactGroup jobContactGroup = this.rpoContext.JobContactGroups.Find(id);
                if (jobContactGroup == null)
                {
                    return this.NotFound();
                }

                this.rpoContext.JobContactGroups.Remove(jobContactGroup);
                this.rpoContext.SaveChanges();

                return this.Ok(jobContactGroup);
            //}
            //else
            //{
            //    throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            //}
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

        private bool JobContactGroupExists(int id)
        {
            return this.rpoContext.JobContactGroups.Count(e => e.Id == id) > 0;
        }

        private JobContactGroupDTO Format(JobContactGroup jobContactGroup)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new JobContactGroupDTO
            {
                Id = jobContactGroup.Id,
                Name = jobContactGroup.Name,
                IdJob = jobContactGroup.IdJob,
                JobNumber = jobContactGroup.Job != null ? jobContactGroup.Job.JobNumber : string.Empty,
                CreatedBy = jobContactGroup.CreatedBy,
                LastModifiedBy = jobContactGroup.LastModifiedBy != null ? jobContactGroup.LastModifiedBy : jobContactGroup.CreatedBy,
                CreatedByEmployeeName = jobContactGroup.CreatedByEmployee != null ? jobContactGroup.CreatedByEmployee.FirstName + " " + jobContactGroup.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = jobContactGroup.LastModifiedByEmployee != null ? jobContactGroup.LastModifiedByEmployee.FirstName + " " + jobContactGroup.LastModifiedByEmployee.LastName : (jobContactGroup.CreatedByEmployee != null ? jobContactGroup.CreatedByEmployee.FirstName + " " + jobContactGroup.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = jobContactGroup.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobContactGroup.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobContactGroup.CreatedDate,
                LastModifiedDate = jobContactGroup.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobContactGroup.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (jobContactGroup.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobContactGroup.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobContactGroup.CreatedDate),
            };
        }

        private JobContactGroupDetail FormatDetails(JobContactGroup jobContactGroup)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new JobContactGroupDetail
            {
                Id = jobContactGroup.Id,
                Name = jobContactGroup.Name,
                IdJob = jobContactGroup.IdJob,
                JobNumber = jobContactGroup.Job != null ? jobContactGroup.Job.JobNumber : string.Empty,
                CreatedBy = jobContactGroup.CreatedBy,
                LastModifiedBy = jobContactGroup.LastModifiedBy != null ? jobContactGroup.LastModifiedBy : jobContactGroup.CreatedBy,
                CreatedByEmployeeName = jobContactGroup.CreatedByEmployee != null ? jobContactGroup.CreatedByEmployee.FirstName + " " + jobContactGroup.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = jobContactGroup.LastModifiedByEmployee != null ? jobContactGroup.LastModifiedByEmployee.FirstName + " " + jobContactGroup.LastModifiedByEmployee.LastName : (jobContactGroup.CreatedByEmployee != null ? jobContactGroup.CreatedByEmployee.FirstName + " " + jobContactGroup.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = jobContactGroup.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobContactGroup.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobContactGroup.CreatedDate,
                LastModifiedDate = jobContactGroup.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobContactGroup.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (jobContactGroup.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobContactGroup.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobContactGroup.CreatedDate),
            };
        }

        private bool JobContactGroupNameExists(string name, int id, int idJob)
        {
            return this.rpoContext.JobContactGroups.Count(e => e.Name == name && e.Id != id && e.IdJob == idJob) > 0;
        }
    }
}