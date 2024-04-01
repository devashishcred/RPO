
// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Mital Bhatt
// Created          : 03-17-2017
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="CompanyLicenseTypesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Company License Types Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.CompanyLicenseTypes
{
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Tools;
    using System;
    using Filters;

   
    /// <summary>
    /// Class Company License Types Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class CompanyLicenseTypesController : ApiController
    {
        private RpoContext db = new RpoContext();
        // GET: CompanyLicenseTypes
        /// <summary>
        /// Gets the Company license types.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttpActionResult.Get CompanyLicenseTypes List</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetCompanyLicenseTypes([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {

                var CompanyLicenseTypes = db.CompanyLicenseTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = CompanyLicenseTypes.Count();
                var recordsFiltered = recordsTotal;

                var result = CompanyLicenseTypes
                    .AsEnumerable()
                    .Select(c => Format(c))
                    .AsQueryable()
                    .DataTableParameters(dataTableParameters, out recordsFiltered)
                    .ToArray();

                return Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Draw,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal,
                    Data = result
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the Company license type dropdown.
        /// </summary>
        /// <returns>IHttpActionResult. Get CompanyLicenseTypes List for dropdown</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Companylicensetype/dropdown")]
        public IHttpActionResult GetCompanyLicenseTypeDropdown()
        {
            var result = db.CompanyLicenseTypes.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name
            }).ToArray();

            return Ok(result);
        }
        /// <summary>
        /// Gets the type of the Company license.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult. Get CompanyLicenseTypes detail</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CompanyLicenseTypeDetail))]
        public IHttpActionResult GetCompanyLicenseType(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                CompanyLicenseType CompanyLicenseType = db.CompanyLicenseTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                if (CompanyLicenseType == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(CompanyLicenseType));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        /// Puts the type of the Company license.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="CompanyLicenseType">Type of the Company license.</param>
        /// <returns>IHttpActionResult. update the detail CompanyLicenseTypes in database </returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CompanyLicenseTypeDetail))]
        public IHttpActionResult PutCompanyLicenseType(int id, CompanyLicenseType CompanyLicenseType)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != CompanyLicenseType.Id)
                {
                    return BadRequest();
                }

                if (CompanyLicenseTypeNameExists(CompanyLicenseType.Name, CompanyLicenseType.Id))
                {
                    throw new RpoBusinessException(StaticMessages.CompanyLicenseTypeNameExistsMessage);
                }
                CompanyLicenseType.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    CompanyLicenseType.LastModifiedBy = employee.Id;
                }

                db.Entry(CompanyLicenseType).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyLicenseTypeExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                CompanyLicenseType CompanyLicenseTypeResponse = db.CompanyLicenseTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return Ok(FormatDetails(CompanyLicenseTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        /// Posts the type of the Company license.
        /// </summary>
        /// <param name="CompanyLicenseType">Type of the Company license.</param>
        /// <returns>IHttpActionResult. Add new CompanyLicenseTypes in database</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CompanyLicenseType))]
        public IHttpActionResult PostCompanyLicenseType(CompanyLicenseType CompanyLicenseType)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (CompanyLicenseTypeNameExists(CompanyLicenseType.Name, CompanyLicenseType.Id))
                {
                    throw new RpoBusinessException(StaticMessages.CompanyLicenseTypeNameExistsMessage);
                }

                CompanyLicenseType.LastModifiedDate = DateTime.UtcNow;
                CompanyLicenseType.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    CompanyLicenseType.CreatedBy = employee.Id;
                }

                db.CompanyLicenseTypes.Add(CompanyLicenseType);
                db.SaveChanges();

                CompanyLicenseType CompanyLicenseTypeResponse = db.CompanyLicenseTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == CompanyLicenseType.Id);
                return Ok(FormatDetails(CompanyLicenseTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        /// Deletes the type of the Company license.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult. delete CompanyLicenseTypes</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CompanyLicenseType))]
        public IHttpActionResult DeleteCompanyLicenseType(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                CompanyLicenseType CompanyLicenseType = db.CompanyLicenseTypes.Find(id);
                if (CompanyLicenseType == null)
                {
                    return this.NotFound();
                }

                db.CompanyLicenseTypes.Remove(CompanyLicenseType);
                db.SaveChanges();

                return Ok(CompanyLicenseType);
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
                db.Dispose();
            }
            base.Dispose(disposing);
        }//public ActionResult Index()
        //{
        //   // return View();
        //}
        /// <summary>
        /// Companys the license type exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool CompanyLicenseTypeExists(int id)
        {
            return db.CompanyLicenseTypes.Count(e => e.Id == id) > 0;
        }
        /// <summary>
        /// Formats the specified Company license type.
        /// </summary>
        /// <param name="CompanyLicenseType">Type of the Company license.</param>
        /// <returns>CompanyLicenseTypeDTO.</returns>
        private CompanyLicenseTypeDTO Format(CompanyLicenseType CompanyLicenseType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new CompanyLicenseTypeDTO
            {
                Id = CompanyLicenseType.Id,
                Name = CompanyLicenseType.Name,
                CreatedBy = CompanyLicenseType.CreatedBy,
                LastModifiedBy = CompanyLicenseType.LastModifiedBy,
                CreatedByEmployeeName = CompanyLicenseType.CreatedByEmployee != null ? CompanyLicenseType.CreatedByEmployee.FirstName + " " + CompanyLicenseType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = CompanyLicenseType.LastModifiedByEmployee != null ? CompanyLicenseType.LastModifiedByEmployee.FirstName + " " + CompanyLicenseType.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = CompanyLicenseType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(CompanyLicenseType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : CompanyLicenseType.CreatedDate,
                LastModifiedDate = CompanyLicenseType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(CompanyLicenseType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : CompanyLicenseType.LastModifiedDate,
            };
        }
        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="CompanyLicenseType">Type of the Company license.</param>
        /// <returns>CompanyLicenseTypeDetail.</returns>
        private CompanyLicenseTypeDetail FormatDetails(CompanyLicenseType CompanyLicenseType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new CompanyLicenseTypeDetail
            {
                Id = CompanyLicenseType.Id,
                Name = CompanyLicenseType.Name,
                CreatedBy = CompanyLicenseType.CreatedBy,
                LastModifiedBy = CompanyLicenseType.LastModifiedBy,
                CreatedByEmployeeName = CompanyLicenseType.CreatedByEmployee != null ? CompanyLicenseType.CreatedByEmployee.FirstName + " " + CompanyLicenseType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = CompanyLicenseType.LastModifiedByEmployee != null ? CompanyLicenseType.LastModifiedByEmployee.FirstName + " " + CompanyLicenseType.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = CompanyLicenseType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(CompanyLicenseType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : CompanyLicenseType.CreatedDate,
                LastModifiedDate = CompanyLicenseType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(CompanyLicenseType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : CompanyLicenseType.LastModifiedDate,
            };
        }
        /// <summary>
        /// Companys the license type name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool CompanyLicenseTypeNameExists(string name, int id)
        {
            return db.CompanyLicenseTypes.Count(e => e.Name == name && e.Id != id) > 0;
        }
    }
}