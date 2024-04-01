// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-20-2018
// ***********************************************************************
// <copyright file="PrimaryStructuralSystemsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Primary Structural Systems Controller.</summary>
// ***********************************************************************


namespace Rpo.ApiServices.Api.Controllers.PrimaryStructuralSystems
{
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Primary Structural Systems Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class PrimaryStructuralSystemsController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the primary structural systems.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the list primary structural systems.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetPrimaryStructuralSystems([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var primaryStructuralSystems = db.PrimaryStructuralSystems.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = primaryStructuralSystems.Count();
                var recordsFiltered = recordsTotal;

                var result = primaryStructuralSystems
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
        /// Gets the primary structural system dropdown.
        /// </summary>
        /// <returns>Gets the primary structural systems for dropdown.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/primarystructuralsystems/dropdown")]
        public IHttpActionResult GetPrimaryStructuralSystemDropdown()
        {
            var result = db.PrimaryStructuralSystems.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Description,
                Description = c.Description
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the primary structural system.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the detail of primary structural systems.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(PrimaryStructuralSystemDetail))]
        public IHttpActionResult GetPrimaryStructuralSystem(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                PrimaryStructuralSystem primaryStructuralSystem = db.PrimaryStructuralSystems.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (primaryStructuralSystem == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(primaryStructuralSystem));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the primary structural system.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="primaryStructuralSystem">The primary structural system.</param>
        /// <returns>update the detail of primary structural systems.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(PrimaryStructuralSystemDetail))]
        public IHttpActionResult PutPrimaryStructuralSystem(int id, PrimaryStructuralSystem primaryStructuralSystem)
        {

            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != primaryStructuralSystem.Id)
                {
                    return BadRequest();
                }

                if (PrimaryStructuralSystemNameExists(primaryStructuralSystem.Description, primaryStructuralSystem.Id))
                {
                    throw new RpoBusinessException(StaticMessages.PrimaryStructuralSystemNameExistsMessage);
                }

                primaryStructuralSystem.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    primaryStructuralSystem.LastModifiedBy = employee.Id;
                }

                db.Entry(primaryStructuralSystem).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrimaryStructuralSystemExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                PrimaryStructuralSystem primaryStructuralSystemResponse = db.PrimaryStructuralSystems.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return Ok(FormatDetails(primaryStructuralSystemResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the primary structural system.
        /// </summary>
        /// <param name="primaryStructuralSystem">The primary structural system.</param>
        /// <returns>create a new primary structural systems.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(PrimaryStructuralSystemDetail))]
        public IHttpActionResult PostPrimaryStructuralSystem(PrimaryStructuralSystem primaryStructuralSystem)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (PrimaryStructuralSystemNameExists(primaryStructuralSystem.Description, primaryStructuralSystem.Id))
                {
                    throw new RpoBusinessException(StaticMessages.PrimaryStructuralSystemNameExistsMessage);
                }
                primaryStructuralSystem.LastModifiedDate = DateTime.UtcNow;
                primaryStructuralSystem.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    primaryStructuralSystem.CreatedBy = employee.Id;
                }

                db.PrimaryStructuralSystems.Add(primaryStructuralSystem);
                db.SaveChanges();

                PrimaryStructuralSystem primaryStructuralSystemResponse = db.PrimaryStructuralSystems.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == primaryStructuralSystem.Id);
                return Ok(FormatDetails(primaryStructuralSystemResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the primary structural system.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>delete the detail of primary structural systems.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(PrimaryStructuralSystem))]
        public IHttpActionResult DeletePrimaryStructuralSystem(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                PrimaryStructuralSystem primaryStructuralSystem = db.PrimaryStructuralSystems.Find(id);
                if (primaryStructuralSystem == null)
                {
                    return this.NotFound();
                }

                db.PrimaryStructuralSystems.Remove(primaryStructuralSystem);
                db.SaveChanges();

                return Ok(primaryStructuralSystem);
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
        }


        /// <summary>
        /// Primaries the structural system exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool PrimaryStructuralSystemExists(int id)
        {
            return db.PrimaryStructuralSystems.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified primary structural system.
        /// </summary>
        /// <param name="primaryStructuralSystem">The primary structural system.</param>
        /// <returns>PrimaryStructuralSystemDTO.</returns>
        private PrimaryStructuralSystemDTO Format(PrimaryStructuralSystem primaryStructuralSystem)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new PrimaryStructuralSystemDTO
            {
                Id = primaryStructuralSystem.Id,
                Description = primaryStructuralSystem.Description,
                CreatedBy = primaryStructuralSystem.CreatedBy,
                LastModifiedBy = primaryStructuralSystem.LastModifiedBy,
                CreatedByEmployeeName = primaryStructuralSystem.CreatedByEmployee != null ? primaryStructuralSystem.CreatedByEmployee.FirstName + " " + primaryStructuralSystem.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = primaryStructuralSystem.LastModifiedByEmployee != null ? primaryStructuralSystem.LastModifiedByEmployee.FirstName + " " + primaryStructuralSystem.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = primaryStructuralSystem.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(primaryStructuralSystem.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : primaryStructuralSystem.CreatedDate,
                LastModifiedDate = primaryStructuralSystem.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(primaryStructuralSystem.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : primaryStructuralSystem.LastModifiedDate,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="primaryStructuralSystem">The primary structural system.</param>
        /// <returns>PrimaryStructuralSystemDetail.</returns>
        private PrimaryStructuralSystemDetail FormatDetails(PrimaryStructuralSystem primaryStructuralSystem)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new PrimaryStructuralSystemDetail
            {
                Id = primaryStructuralSystem.Id,
                Description = primaryStructuralSystem.Description,
                CreatedBy = primaryStructuralSystem.CreatedBy,
                LastModifiedBy = primaryStructuralSystem.LastModifiedBy,
                CreatedByEmployeeName = primaryStructuralSystem.CreatedByEmployee != null ? primaryStructuralSystem.CreatedByEmployee.FirstName + " " + primaryStructuralSystem.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = primaryStructuralSystem.LastModifiedByEmployee != null ? primaryStructuralSystem.LastModifiedByEmployee.FirstName + " " + primaryStructuralSystem.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = primaryStructuralSystem.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(primaryStructuralSystem.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : primaryStructuralSystem.CreatedDate,
                LastModifiedDate = primaryStructuralSystem.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(primaryStructuralSystem.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : primaryStructuralSystem.LastModifiedDate,
            };
        }

        /// <summary>
        /// Primaries the structural system name exists.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool PrimaryStructuralSystemNameExists(string description, int id)
        {
            return db.PrimaryStructuralSystems.Count(e => e.Description == description && e.Id != id) > 0;
        }
    }
}