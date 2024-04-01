// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-21-2018
// ***********************************************************************
// <copyright file="ContactLicenseTypesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Contact License Types Controller.</summary>
// ***********************************************************************


namespace Rpo.ApiServices.Api.Controllers.ContactLicenseTypes
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
    /// Class Contact License Types Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class ContactLicenseTypesController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the contact license types.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttpActionResult.Get ContactLicenseTypes List</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetContactLicenseTypes([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {

                var contactLicenseTypes = db.ContactLicenseTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = contactLicenseTypes.Count();
                var recordsFiltered = recordsTotal;

                var result = contactLicenseTypes
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
        /// Gets the contact license type dropdown.
        /// </summary>
        /// <returns>IHttpActionResult. Get ContactLicenseTypes List for dropdown</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/contactlicensetype/dropdown")]
        public IHttpActionResult GetContactLicenseTypeDropdown()
        {
            var result = db.ContactLicenseTypes.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the type of the contact license.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult. Get ContactLicenseTypes detail</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ContactLicenseTypeDetail))]
        public IHttpActionResult GetContactLicenseType(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                ContactLicenseType contactLicenseType = db.ContactLicenseTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                if (contactLicenseType == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(contactLicenseType));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the type of the contact license.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="contactLicenseType">Type of the contact license.</param>
        /// <returns>IHttpActionResult. update the detail ContactLicenseTypes in database </returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ContactLicenseTypeDetail))]
        public IHttpActionResult PutContactLicenseType(int id, ContactLicenseType contactLicenseType)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != contactLicenseType.Id)
                {
                    return BadRequest();
                }

                if (ContactLicenseTypeNameExists(contactLicenseType.Name, contactLicenseType.Id))
                {
                    throw new RpoBusinessException(StaticMessages.ContactLicenseTypeNameExistsMessage);
                }
                contactLicenseType.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    contactLicenseType.LastModifiedBy = employee.Id;
                }

                db.Entry(contactLicenseType).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactLicenseTypeExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                ContactLicenseType contactLicenseTypeResponse = db.ContactLicenseTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return Ok(FormatDetails(contactLicenseTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the type of the contact license.
        /// </summary>
        /// <param name="contactLicenseType">Type of the contact license.</param>
        /// <returns>IHttpActionResult. Add new ContactLicenseTypes in database</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ContactLicenseType))]
        public IHttpActionResult PostContactLicenseType(ContactLicenseType contactLicenseType)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (ContactLicenseTypeNameExists(contactLicenseType.Name, contactLicenseType.Id))
                {
                    throw new RpoBusinessException(StaticMessages.ContactLicenseTypeNameExistsMessage);
                }

                contactLicenseType.LastModifiedDate = DateTime.UtcNow;
                contactLicenseType.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    contactLicenseType.CreatedBy = employee.Id;
                }

                db.ContactLicenseTypes.Add(contactLicenseType);
                db.SaveChanges();

                ContactLicenseType contactLicenseTypeResponse = db.ContactLicenseTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == contactLicenseType.Id);
                return Ok(FormatDetails(contactLicenseTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the type of the contact license.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult. delete ContactLicenseTypes</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ContactLicenseType))]
        public IHttpActionResult DeleteContactLicenseType(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                ContactLicenseType contactLicenseType = db.ContactLicenseTypes.Find(id);
                if (contactLicenseType == null)
                {
                    return this.NotFound();
                }

                db.ContactLicenseTypes.Remove(contactLicenseType);
                db.SaveChanges();

                return Ok(contactLicenseType);
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
        /// Contacts the license type exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ContactLicenseTypeExists(int id)
        {
            return db.ContactLicenseTypes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified contact license type.
        /// </summary>
        /// <param name="contactLicenseType">Type of the contact license.</param>
        /// <returns>ContactLicenseTypeDTO.</returns>
        private ContactLicenseTypeDTO Format(ContactLicenseType contactLicenseType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new ContactLicenseTypeDTO
            {
                Id = contactLicenseType.Id,
                Name = contactLicenseType.Name,
                CreatedBy = contactLicenseType.CreatedBy,
                LastModifiedBy = contactLicenseType.LastModifiedBy,
                CreatedByEmployeeName = contactLicenseType.CreatedByEmployee != null ? contactLicenseType.CreatedByEmployee.FirstName + " " + contactLicenseType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = contactLicenseType.LastModifiedByEmployee != null ? contactLicenseType.LastModifiedByEmployee.FirstName + " " + contactLicenseType.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = contactLicenseType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactLicenseType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactLicenseType.CreatedDate,
                LastModifiedDate = contactLicenseType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactLicenseType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactLicenseType.LastModifiedDate,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="contactLicenseType">Type of the contact license.</param>
        /// <returns>ContactLicenseTypeDetail.</returns>
        private ContactLicenseTypeDetail FormatDetails(ContactLicenseType contactLicenseType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new ContactLicenseTypeDetail
            {
                Id = contactLicenseType.Id,
                Name = contactLicenseType.Name,
                CreatedBy = contactLicenseType.CreatedBy,
                LastModifiedBy = contactLicenseType.LastModifiedBy,
                CreatedByEmployeeName = contactLicenseType.CreatedByEmployee != null ? contactLicenseType.CreatedByEmployee.FirstName + " " + contactLicenseType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = contactLicenseType.LastModifiedByEmployee != null ? contactLicenseType.LastModifiedByEmployee.FirstName + " " + contactLicenseType.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = contactLicenseType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactLicenseType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactLicenseType.CreatedDate,
                LastModifiedDate = contactLicenseType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactLicenseType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactLicenseType.LastModifiedDate,
            };
        }

        /// <summary>
        /// Contacts the license type name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ContactLicenseTypeNameExists(string name, int id)
        {
            return db.ContactLicenseTypes.Count(e => e.Name == name && e.Id != id) > 0;
        }
    }
}