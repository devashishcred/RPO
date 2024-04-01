// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-21-2018
// ***********************************************************************
// <copyright file="ContactTitlesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Contact Titles Controller.</summary>
// ***********************************************************************


namespace Rpo.ApiServices.Api.Controllers.ContactTitles
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
    /// Class Contact Titles Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class ContactTitlesController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the contact titles.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttpActionResult. Get the List of contactTitles</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetContactTitles([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddAddress)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddContact)
                  )
            {
                var contactTitles = db.ContactTitles.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = contactTitles.Count();
                var recordsFiltered = recordsTotal;

                var result = contactTitles
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
        /// Gets the contact title dropdown.
        /// </summary>
        /// <returns>IHttpActionResult. Get the List of contactTitles</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/contacttitles/dropdown")]
        public IHttpActionResult GetContactTitleDropdown()
        {
            var result = db.ContactTitles.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the contact title.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult. Get the List of contactTitle detail</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ContactTitleDetail))]
        public IHttpActionResult GetContactTitle(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                ContactTitle contactTitle = db.ContactTitles.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                if (contactTitle == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(contactTitle));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the contact title.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="contactTitle">The contact title.</param>
        /// <returns>IHttpActionResult. Update the contactTitle</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutContactTitle(int id, ContactTitle contactTitle)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contactTitle.Id)
            {
                return BadRequest();
            }

            if (ContactTitleNameExists(contactTitle.Name, contactTitle.Id))
            {
                throw new RpoBusinessException(StaticMessages.ContactTitleNameExistsMessage);
            }
            contactTitle.LastModifiedDate = DateTime.UtcNow;
            if (employee != null)
            {
                contactTitle.LastModifiedBy = employee.Id;
            }

            db.Entry(contactTitle).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactTitleExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            ContactTitle contactTitleResponse = db.ContactTitles.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
            return Ok(FormatDetails(contactTitleResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the contact title.
        /// </summary>
        /// <param name="contactTitle">The contact title.</param>
        /// <returns>IHttpActionResult. Add new ContactTitle</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ContactTitleDetail))]
        public IHttpActionResult PostContactTitle(ContactTitle contactTitle)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (ContactTitleNameExists(contactTitle.Name, contactTitle.Id))
            {
                throw new RpoBusinessException(StaticMessages.ContactTitleNameExistsMessage);
            }
            contactTitle.LastModifiedDate = DateTime.UtcNow;
            contactTitle.CreatedDate = DateTime.UtcNow;
            if (employee != null)
            {
                contactTitle.CreatedBy = employee.Id;
            }

            db.ContactTitles.Add(contactTitle);
            db.SaveChanges();

            ContactTitle contactTitleResponse = db.ContactTitles.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == contactTitle.Id);
            return Ok(FormatDetails(contactTitleResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the contact title.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult. Delete the contactTitle</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ContactTitle))]
        public IHttpActionResult DeleteContactTitle(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                ContactTitle contactTitle = db.ContactTitles.Find(id);
            if (contactTitle == null)
            {
                return this.NotFound();
            }

            db.ContactTitles.Remove(contactTitle);
            db.SaveChanges();

            return Ok(contactTitle);
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
        /// Contacts the title exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ContactTitleExists(int id)
        {
            return db.ContactTitles.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified contact title.
        /// </summary>
        /// <param name="contactTitle">The contact title.</param>
        /// <returns>ContactTitleDTO.</returns>
        private ContactTitleDTO Format(ContactTitle contactTitle)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new ContactTitleDTO
            {
                Id = contactTitle.Id,
                Name = contactTitle.Name,
                CreatedBy = contactTitle.CreatedBy,
                LastModifiedBy = contactTitle.LastModifiedBy,
                CreatedByEmployeeName = contactTitle.CreatedByEmployee != null ? contactTitle.CreatedByEmployee.FirstName + " " + contactTitle.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = contactTitle.LastModifiedByEmployee != null ? contactTitle.LastModifiedByEmployee.FirstName + " " + contactTitle.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = contactTitle.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactTitle.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactTitle.CreatedDate,
                LastModifiedDate = contactTitle.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactTitle.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactTitle.LastModifiedDate,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="contactTitle">The contact title.</param>
        /// <returns>ContactTitleDetail.</returns>
        private ContactTitleDetail FormatDetails(ContactTitle contactTitle)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new ContactTitleDetail
            {
                Id = contactTitle.Id,
                Name = contactTitle.Name,
                CreatedBy = contactTitle.CreatedBy,
                LastModifiedBy = contactTitle.LastModifiedBy,
                CreatedByEmployeeName = contactTitle.CreatedByEmployee != null ? contactTitle.CreatedByEmployee.FirstName + " " + contactTitle.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = contactTitle.LastModifiedByEmployee != null ? contactTitle.LastModifiedByEmployee.FirstName + " " + contactTitle.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = contactTitle.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactTitle.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactTitle.CreatedDate,
                LastModifiedDate = contactTitle.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactTitle.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactTitle.LastModifiedDate,
            };
        }

        /// <summary>
        /// Contacts the title name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ContactTitleNameExists(string name, int id)
        {
            return db.ContactTitles.Count(e => e.Name == name && e.Id != id) > 0;
        }
    }
}