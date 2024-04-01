// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 08-23-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 08-23-2018
// ***********************************************************************
// <copyright file="OwnerTypesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Owner Types Controller.</summary>
// ***********************************************************************


/// <summary>
/// The OwnerTypes namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.OwnerTypes
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using DataTable;
    using Filters;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Owner Types Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class OwnerTypesController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the owner types.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the owner types List.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetOwnerTypes([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var addressTypes = this.rpoContext.OwnerTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = addressTypes.Count();
                var recordsFiltered = recordsTotal;

                var result = addressTypes
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
        /// Gets the owner type dropdown.
        /// </summary>
        /// <returns>Gets the owner types list for bind dropdown.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/ownertypes/dropdown")]
        public IHttpActionResult GetOwnerTypeDropdown()
        {
            var result = this.rpoContext.OwnerTypes.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name,
                IsSecondOfficerRequire = c.IsSecondOfficerRequire,
            }).ToArray();

            return this.Ok(result);
        }

        /// <summary>
        /// Gets the type of the owner.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the owner types in detail.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(OwnerTypeDetail))]
        public IHttpActionResult GetOwnerType(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                OwnerType ownerType = this.rpoContext.OwnerTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (ownerType == null)
                {
                    return this.NotFound();
                }

                return this.Ok(this.FormatDetails(ownerType));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the type of the owner.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="ownerType">Type of the owner.</param>
        /// <returns>update the detail of owner types.</returns>
        /// <exception cref="RpoBusinessException">
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOwnerType(int id, OwnerType ownerType)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (id != ownerType.Id)
                {
                    return this.BadRequest();
                }

                if (this.OwnerTypeNameExists(ownerType.Name, ownerType.Id))
                {
                    throw new RpoBusinessException(StaticMessages.OwnerTypeNameExistsMessage);
                }

                ownerType.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    ownerType.LastModifiedBy = employee.Id;
                }

                this.rpoContext.Entry(ownerType).State = EntityState.Modified;

                try
                {
                    this.rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.OwnerTypeExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                OwnerType ownerTypeResponse = this.rpoContext.OwnerTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return this.Ok(this.FormatDetails(ownerTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the type of the owner.
        /// </summary>
        /// <param name="ownerType">Type of the owner.</param>
        /// <returns>Create a detail of owner types.</returns>
        /// <exception cref="RpoBusinessException">
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(OwnerType))]
        public IHttpActionResult PostOwnerType(OwnerType ownerType)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (this.OwnerTypeNameExists(ownerType.Name, ownerType.Id))
                {
                    throw new RpoBusinessException(StaticMessages.OwnerTypeNameExistsMessage);
                }

                ownerType.LastModifiedDate = DateTime.UtcNow;
                ownerType.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    ownerType.CreatedBy = employee.Id;
                }

                this.rpoContext.OwnerTypes.Add(ownerType);
                this.rpoContext.SaveChanges();

                OwnerType ownerTypeResponse = this.rpoContext.OwnerTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == ownerType.Id);
                return this.Ok(this.FormatDetails(ownerTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the type of the owner.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>delete the detail of owner types..</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(OwnerType))]
        public IHttpActionResult DeleteOwnerType(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                OwnerType ownerType = this.rpoContext.OwnerTypes.Find(id);
                if (ownerType == null)
                {
                    return this.NotFound();
                }

                this.rpoContext.OwnerTypes.Remove(ownerType);
                this.rpoContext.SaveChanges();

                return this.Ok(ownerType);
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
        /// Owners the type exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool OwnerTypeExists(int id)
        {
            return this.rpoContext.OwnerTypes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified owner type.
        /// </summary>
        /// <param name="ownerType">Type of the owner.</param>
        /// <returns>OwnerTypeDTO.</returns>
        private OwnerTypeDTO Format(OwnerType ownerType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new OwnerTypeDTO
            {
                Id = ownerType.Id,
                Name = ownerType.Name,
                IsSecondOfficerRequire = ownerType.IsSecondOfficerRequire,
                CreatedBy = ownerType.CreatedBy,
                LastModifiedBy = ownerType.LastModifiedBy != null ? ownerType.LastModifiedBy : ownerType.CreatedBy,
                CreatedByEmployeeName = ownerType.CreatedByEmployee != null ? ownerType.CreatedByEmployee.FirstName + " " + ownerType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = ownerType.LastModifiedByEmployee != null ? ownerType.LastModifiedByEmployee.FirstName + " " + ownerType.LastModifiedByEmployee.LastName : (ownerType.CreatedByEmployee != null ? ownerType.CreatedByEmployee.FirstName + " " + ownerType.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = ownerType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(ownerType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : ownerType.CreatedDate,
                LastModifiedDate = ownerType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(ownerType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (ownerType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(ownerType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : ownerType.CreatedDate),
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="ownerType">Type of the owner.</param>
        /// <returns>OwnerTypeDetail.</returns>
        private OwnerTypeDetail FormatDetails(OwnerType ownerType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new OwnerTypeDetail
            {
                Id = ownerType.Id,
                Name = ownerType.Name,
                IsSecondOfficerRequire = ownerType.IsSecondOfficerRequire,
                CreatedBy = ownerType.CreatedBy,
                LastModifiedBy = ownerType.LastModifiedBy != null ? ownerType.LastModifiedBy : ownerType.CreatedBy,
                CreatedByEmployeeName = ownerType.CreatedByEmployee != null ? ownerType.CreatedByEmployee.FirstName + " " + ownerType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = ownerType.LastModifiedByEmployee != null ? ownerType.LastModifiedByEmployee.FirstName + " " + ownerType.LastModifiedByEmployee.LastName : (ownerType.CreatedByEmployee != null ? ownerType.CreatedByEmployee.FirstName + " " + ownerType.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = ownerType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(ownerType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : ownerType.CreatedDate,
                LastModifiedDate = ownerType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(ownerType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (ownerType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(ownerType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : ownerType.CreatedDate),
            };
        }

        /// <summary>
        /// Owners the type name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool OwnerTypeNameExists(string name, int id)
        {
            return this.rpoContext.OwnerTypes.Count(e => e.Name == name && e.Id != id) > 0;
        }
    }
}