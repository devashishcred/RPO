// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-21-2018
// ***********************************************************************
// <copyright file="PrefixesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Prefixes Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Prefixes namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Prefixes
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
    /// Class Prefixes Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class PrefixesController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the prefixes.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the prefixe List.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetPrefixes([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var Prefixes = this.rpoContext.Prefixes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = Prefixes.Count();
                var recordsFiltered = recordsTotal;

                var result = Prefixes
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
                    Data = result.OrderByDescending(x => x.LastModifiedDate)
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the prefixes dropdown.
        /// </summary>
        /// <returns>Gets the prefixe for dropdown.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/Prefixes/dropdown")]
        public IHttpActionResult GetSufixDropdown()
        {
            var result = this.rpoContext.Prefixes.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name
            }).ToArray();

            return this.Ok(result);
        }

        /// <summary>
        /// Gets the prefix.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the prefixe detail.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(PrefixDetail))]
        public IHttpActionResult GetPrefix(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                Prefix prefix = this.rpoContext.Prefixes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (prefix == null)
                {
                    return this.NotFound();
                }

                return this.Ok(this.FormatDetails(prefix));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the prefix.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns>Update the prefix detail.</returns>
        /// <exception cref="RpoBusinessException">
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(PrefixDetail))]
        public IHttpActionResult PutPrefix(int id, Prefix prefix)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (id != prefix.Id)
                {
                    return this.BadRequest();
                }

                if (this.PrefixNameExists(prefix.Name, prefix.Id))
                {
                    throw new RpoBusinessException(StaticMessages.PrefixExistsMessage);
                }

                prefix.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    prefix.LastModifiedBy = employee.Id;
                }

                this.rpoContext.Entry(prefix).State = EntityState.Modified;

                try
                {
                    this.rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.PrefixExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                Prefix prefixResponse = this.rpoContext.Prefixes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return this.Ok(this.FormatDetails(prefixResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns>create new prefix..</returns>
        /// <exception cref="RpoBusinessException">
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(PrefixDetail))]
        public IHttpActionResult PostPrefix(Prefix prefix)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (this.PrefixNameExists(prefix.Name, prefix.Id))
                {
                    throw new RpoBusinessException(StaticMessages.PrefixExistsMessage);
                }

                prefix.LastModifiedDate = DateTime.UtcNow;
                prefix.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    prefix.CreatedBy = employee.Id;
                }

                this.rpoContext.Prefixes.Add(prefix);
                this.rpoContext.SaveChanges();

                Prefix prefixResponse = this.rpoContext.Prefixes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == prefix.Id);
                return this.Ok(this.FormatDetails(prefixResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the prefix.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Delete the prefix on table.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(PrefixDetail))]
        public IHttpActionResult DeletePrefix(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                Prefix prefix = this.rpoContext.Prefixes.Find(id);
                if (prefix == null)
                {
                    return this.NotFound();
                }

                this.rpoContext.Prefixes.Remove(prefix);
                this.rpoContext.SaveChanges();

                return this.Ok(FormatDetails(prefix));
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
        /// Prefixes the exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool PrefixExists(int id)
        {
            return this.rpoContext.Prefixes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns>PrefixDTO.</returns>
        private PrefixDTO Format(Prefix prefix)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new PrefixDTO
            {
                Id = prefix.Id,
                Name = prefix.Name,
                CreatedBy = prefix.CreatedBy,
                LastModifiedBy = prefix.LastModifiedBy != null ? prefix.LastModifiedBy : prefix.CreatedBy,
                CreatedByEmployeeName = prefix.CreatedByEmployee != null ? prefix.CreatedByEmployee.FirstName + " " + prefix.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = prefix.LastModifiedByEmployee != null ? prefix.LastModifiedByEmployee.FirstName + " " + prefix.LastModifiedByEmployee.LastName : (prefix.CreatedByEmployee != null ? prefix.CreatedByEmployee.FirstName + " " + prefix.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = prefix.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(prefix.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : prefix.CreatedDate,
                LastModifiedDate = prefix.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(prefix.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (prefix.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(prefix.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : prefix.CreatedDate),
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns>PrefixDetail.</returns>
        private PrefixDetail FormatDetails(Prefix prefix)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new PrefixDetail
            {
                Id = prefix.Id,
                Name = prefix.Name,
                CreatedBy = prefix.CreatedBy,
                LastModifiedBy = prefix.LastModifiedBy != null ? prefix.LastModifiedBy : prefix.CreatedBy,
                CreatedByEmployeeName = prefix.CreatedByEmployee != null ? prefix.CreatedByEmployee.FirstName + " " + prefix.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = prefix.LastModifiedByEmployee != null ? prefix.LastModifiedByEmployee.FirstName + " " + prefix.LastModifiedByEmployee.LastName : (prefix.CreatedByEmployee != null ? prefix.CreatedByEmployee.FirstName + " " + prefix.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = prefix.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(prefix.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : prefix.CreatedDate,
                LastModifiedDate = prefix.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(prefix.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (prefix.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(prefix.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : prefix.CreatedDate),
            };
        }

        /// <summary>
        /// Prefixes the name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool PrefixNameExists(string name, int id)
        {
            return this.rpoContext.Prefixes.Count(e => e.Name == name && e.Id != id) > 0;
        }
    }
}