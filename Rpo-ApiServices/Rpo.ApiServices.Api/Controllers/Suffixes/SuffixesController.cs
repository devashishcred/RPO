
namespace Rpo.ApiServices.Api.Controllers.Suffixes
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

    /// <summary>
    /// Class Suffixes Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class SuffixesController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the suffixes.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Get the list of suffixes.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetSufixes([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var sufixes = this.rpoContext.Sufixes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = sufixes.Count();
                var recordsFiltered = recordsTotal;

                var result = sufixes
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
                    Data = result.OrderByDescending(x=>x.LastModifiedDate)
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the suffix drop down.
        /// </summary>
        /// <returns>Get the list of suffixes.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/Suffixes/dropdown")]
        public IHttpActionResult GetSufixDropdown()
        {
            var result = this.rpoContext.Sufixes.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Description,
                Description = c.Description
            }).ToArray();

            return this.Ok(result);
        }

        /// <summary>
        /// Gets the suffix.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Get the list of suffixes.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(SuffixDetail))]
        public IHttpActionResult GetSuffix(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                Suffix suffix = this.rpoContext.Sufixes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (suffix == null)
                {
                    return this.NotFound();
                }

                return this.Ok(this.FormatDetails(suffix));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the suffix.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns>update the detail of suffix.</returns>
        /// <exception cref="RpoBusinessException">
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(SuffixDetail))]
        public IHttpActionResult PutSuffix(int id, Suffix suffix)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (id != suffix.Id)
                {
                    return this.BadRequest();
                }

                if (this.SuffixNameExists(suffix.Description, suffix.Id))
                {
                    throw new RpoBusinessException(StaticMessages.SuffixExistsMessage);
                }

                suffix.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    suffix.LastModifiedBy = employee.Id;
                }

                this.rpoContext.Entry(suffix).State = EntityState.Modified;

                try
                {
                    this.rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.SuffixExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                Suffix suffixResponse = this.rpoContext.Sufixes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return this.Ok(this.FormatDetails(suffixResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the suffix.
        /// </summary>
        /// <param name="suffix">The suffix.</param>
        /// <returns>create a new suffix in database.</returns>
        /// <exception cref="RpoBusinessException">
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(SuffixDetail))]
        public IHttpActionResult PostSuffix(Suffix suffix)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (this.SuffixNameExists(suffix.Description, suffix.Id))
                {
                    throw new RpoBusinessException(StaticMessages.SuffixExistsMessage);
                }
                
                suffix.LastModifiedDate = DateTime.UtcNow;
                suffix.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    suffix.CreatedBy = employee.Id;
                }

                this.rpoContext.Sufixes.Add(suffix);
                this.rpoContext.SaveChanges();

                Suffix suffixResponse = this.rpoContext.Sufixes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == suffix.Id);
                return this.Ok(this.FormatDetails(suffixResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the suffix.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Delete the suffix in database.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(SuffixDetail))]
        public IHttpActionResult DeleteSuffix(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                Suffix suffix = this.rpoContext.Sufixes.Find(id);
                if (suffix == null)
                {
                    return this.NotFound();
                }

                this.rpoContext.Sufixes.Remove(suffix);
                this.rpoContext.SaveChanges();

                return this.Ok(FormatDetails(suffix));
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
                this.rpoContext.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Suffixes the exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool SuffixExists(int id)
        {
            return this.rpoContext.Sufixes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified suffix.
        /// </summary>
        /// <param name="suffix">The suffix.</param>
        /// <returns>SuffixDTO.</returns>
        private SuffixDTO Format(Suffix suffix)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new SuffixDTO
            {
                Id = suffix.Id,
                Description = suffix.Description,
                CreatedBy = suffix.CreatedBy,
                LastModifiedBy = suffix.LastModifiedBy != null ? suffix.LastModifiedBy : suffix.CreatedBy,
                CreatedByEmployeeName = suffix.CreatedByEmployee != null ? suffix.CreatedByEmployee.FirstName + " " + suffix.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = suffix.LastModifiedByEmployee != null ? suffix.LastModifiedByEmployee.FirstName + " " + suffix.LastModifiedByEmployee.LastName : (suffix.CreatedByEmployee != null ? suffix.CreatedByEmployee.FirstName + " " + suffix.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = suffix.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(suffix.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : suffix.CreatedDate,
                LastModifiedDate = suffix.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(suffix.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (suffix.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(suffix.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : suffix.CreatedDate),
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="suffix">The suffix.</param>
        /// <returns>SuffixDetail.</returns>
        private SuffixDetail FormatDetails(Suffix suffix)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new SuffixDetail
            {
                Id = suffix.Id,
                Description = suffix.Description,
                CreatedBy = suffix.CreatedBy,
                LastModifiedBy = suffix.LastModifiedBy != null ? suffix.LastModifiedBy : suffix.CreatedBy,
                CreatedByEmployeeName = suffix.CreatedByEmployee != null ? suffix.CreatedByEmployee.FirstName + " " + suffix.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = suffix.LastModifiedByEmployee != null ? suffix.LastModifiedByEmployee.FirstName + " " + suffix.LastModifiedByEmployee.LastName : (suffix.CreatedByEmployee != null ? suffix.CreatedByEmployee.FirstName + " " + suffix.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = suffix.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(suffix.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : suffix.CreatedDate,
                LastModifiedDate = suffix.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(suffix.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (suffix.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(suffix.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : suffix.CreatedDate),
            };
        }

        /// <summary>
        /// Suffixes the name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool SuffixNameExists(string name, int id)
        {
            return this.rpoContext.Sufixes.Count(e => e.Description == name && e.Id != id) > 0;
        }
    }
}