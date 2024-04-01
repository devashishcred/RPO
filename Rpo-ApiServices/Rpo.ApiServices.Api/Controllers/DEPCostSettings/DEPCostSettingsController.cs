
namespace Rpo.ApiServices.Api.Controllers.DEPCostSettings
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

    public class DEPCostSettingsController : ApiController
    {
        /// <summary>
        /// The database.
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Get the DEPCostSettings
        /// </summary>
        /// /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttpActionResult. Get the GetDEPCostSettings List </returns>

        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetDEPCostSettings([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var depCostSettings = this.rpoContext.DEPCostSettings.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = depCostSettings.Count();
                var recordsFiltered = recordsTotal;

                var result = depCostSettings
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
                    Data = result.OrderBy(x => x.LastModifiedDate)
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Get the DEPCostSettings
        /// </summary>
        /// /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttpActionResult. Get the GetDEPCostSettings List for dropdown </returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/DEPCostSettings/dropdown")]
        public IHttpActionResult GetDEPCostSettingDropdown()
        {
            var result = this.rpoContext.DEPCostSettings.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name,
                Price = c.Price,
                NumberOfDays = c.NumberOfDays
            }).ToArray();

            return this.Ok(result);
        }
        /// <summary>
        /// Get the DEPCostSettings
        /// </summary>
        /// /// <param name="id">The id parameters.</param>
        /// <returns>IHttpActionResult. Get the GetDEPCostSettings detail </returns>

        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DEPCostSettingDetail))]
        public IHttpActionResult GetDEPCostSetting(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                DEPCostSetting depCostSetting = this.rpoContext.DEPCostSettings.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (depCostSetting == null)
                {
                    return this.NotFound();
                }

                return this.Ok(this.FormatDetails(depCostSetting));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        /// put the DEPCostSettings
        /// </summary>
        /// /// <param name="DEPCostSetting">The data table parameters.</param>
        /// <returns>IHttpActionResult.update the GetDEPCostSettings detail </returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DEPCostSettingDetail))]
        public IHttpActionResult PutDEPCostSetting(int id, DEPCostSetting depCostSetting)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (id != depCostSetting.Id)
                {
                    return this.BadRequest();
                }

                depCostSetting.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    depCostSetting.LastModifiedBy = employee.Id;
                }

                this.rpoContext.Entry(depCostSetting).State = EntityState.Modified;

                try
                {
                    this.rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.DEPCostSettingExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                DEPCostSetting depCostSettingResponse = this.rpoContext.DEPCostSettings.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return this.Ok(this.FormatDetails(depCostSettingResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        /// post the DEPCostSettings
        /// </summary>
        /// /// <param name="DEPCostSetting">The data table parameters.</param>
        /// <returns>IHttpActionResult.add new GetDEPCostSettings detail </returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DEPCostSettingDetail))]
        public IHttpActionResult PostDEPCostSetting(DEPCostSetting depCostSetting)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                depCostSetting.LastModifiedDate = DateTime.UtcNow;
                depCostSetting.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    depCostSetting.CreatedBy = employee.Id;
                }

                this.rpoContext.DEPCostSettings.Add(depCostSetting);
                this.rpoContext.SaveChanges();

                DEPCostSetting depCostSettingResponse = this.rpoContext.DEPCostSettings.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == depCostSetting.Id);
                return this.Ok(this.FormatDetails(depCostSettingResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        /// delete the DEPCostSettings
        /// </summary>
        /// /// <param name="id">The id parameters.</param>
        /// <returns>IHttpActionResult. delete the GetDEPCostSettings List </returns>

        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DEPCostSetting))]
        public IHttpActionResult DeleteDEPCostSetting(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                DEPCostSetting depCostSetting = this.rpoContext.DEPCostSettings.Find(id);
                if (depCostSetting == null)
                {
                    return this.NotFound();
                }

                this.rpoContext.DEPCostSettings.Remove(depCostSetting);
                this.rpoContext.SaveChanges();

                return this.Ok(depCostSetting);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
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

        private bool DEPCostSettingExists(int id)
        {
            return this.rpoContext.DEPCostSettings.Count(e => e.Id == id) > 0;
        }

        private DEPCostSettingDTO Format(DEPCostSetting depCostSetting)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new DEPCostSettingDTO
            {
                Id = depCostSetting.Id,
                Name = depCostSetting.Name,
                Description = depCostSetting.Description,
                Price = depCostSetting.Price,
                NumberOfDays = depCostSetting.NumberOfDays,
                CreatedBy = depCostSetting.CreatedBy,
                LastModifiedBy = depCostSetting.LastModifiedBy != null ? depCostSetting.LastModifiedBy : depCostSetting.CreatedBy,
                CreatedByEmployeeName = depCostSetting.CreatedByEmployee != null ? depCostSetting.CreatedByEmployee.FirstName + " " + depCostSetting.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = depCostSetting.LastModifiedByEmployee != null ? depCostSetting.LastModifiedByEmployee.FirstName + " " + depCostSetting.LastModifiedByEmployee.LastName : (depCostSetting.CreatedByEmployee != null ? depCostSetting.CreatedByEmployee.FirstName + " " + depCostSetting.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = depCostSetting.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(depCostSetting.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : depCostSetting.CreatedDate,
                LastModifiedDate = depCostSetting.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(depCostSetting.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (depCostSetting.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(depCostSetting.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : depCostSetting.CreatedDate),
            };
        }

        private DEPCostSettingDetail FormatDetails(DEPCostSetting depCostSetting)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new DEPCostSettingDetail
            {
                Id = depCostSetting.Id,
                Name = depCostSetting.Name,
                Description = depCostSetting.Description,
                Price = depCostSetting.Price,
                NumberOfDays = depCostSetting.NumberOfDays,
                CreatedBy = depCostSetting.CreatedBy,
                LastModifiedBy = depCostSetting.LastModifiedBy != null ? depCostSetting.LastModifiedBy : depCostSetting.CreatedBy,
                CreatedByEmployeeName = depCostSetting.CreatedByEmployee != null ? depCostSetting.CreatedByEmployee.FirstName + " " + depCostSetting.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = depCostSetting.LastModifiedByEmployee != null ? depCostSetting.LastModifiedByEmployee.FirstName + " " + depCostSetting.LastModifiedByEmployee.LastName : (depCostSetting.CreatedByEmployee != null ? depCostSetting.CreatedByEmployee.FirstName + " " + depCostSetting.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = depCostSetting.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(depCostSetting.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : depCostSetting.CreatedDate,
                LastModifiedDate = depCostSetting.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(depCostSetting.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (depCostSetting.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(depCostSetting.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : depCostSetting.CreatedDate),
            };
        }

    }
}