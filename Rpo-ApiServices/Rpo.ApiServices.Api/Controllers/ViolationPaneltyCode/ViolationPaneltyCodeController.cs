// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Krunal Pandya
// Created          : 05-11-2018
//
// Last Modified By : Krunal Pandya
// Last Modified On : 05-11-2018
// ***********************************************************************
// <copyright file="ViolationPaneltyCodeController.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary>Class Violation Penalty CodeController.</summary>
// ***********************************************************************

/// <summary>
/// The Violation Penalty Code namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.ViolationPaneltyCode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;
    using DataTable;
    using Filters;
    using Model;
    using Models;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Api.Tools;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    /// <summary>
    /// Class Violation Penalty CodeController.
    /// </summary>
    public class ViolationPaneltyCodeController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the violation penalty codes.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the violation penalty codes List.</returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetViolationPaneltyCodes([FromUri] DataTableParameters dataTableParameters)
        {
            var violationPaneltyCodes = this.rpoContext.ViolationPaneltyCodes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

            var recordsTotal = violationPaneltyCodes.Count();
            var recordsFiltered = recordsTotal;

            var result = violationPaneltyCodes
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

        /// <summary>
        /// Gets the violation penalty codes drop down.
        /// </summary>
        /// <returns>Gets the violation penalty code List for Dropdown.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/ViolationPaneltyCodes/dropdown")]
        public IHttpActionResult GetViolationPaneltyCodesDropdown()
        {
            var result = this.rpoContext.ViolationPaneltyCodes.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.PaneltyCode,
                Name = c.PaneltyCode,
                CodeSection = c.CodeSection
            }).ToArray();

            return this.Ok(result);
        }

        /// <summary>
        /// Gets the violation penalty codes.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the violation penalty codes.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ViolationPaneltyCodeDetails))]
        public IHttpActionResult GetViolationPaneltyCodes(int id)
        {
            ViolationPaneltyCode violationPaneltyCode = this.rpoContext.ViolationPaneltyCodes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

            if (violationPaneltyCode == null)
            {
                return this.NotFound();
            }

            return this.Ok(this.FormatDetails(violationPaneltyCode));

        }

        /// <summary>
        /// Puts the violation penalty code.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="violationPaneltyCode">The violation penalty code.</param>
        /// <returns>update the violation penalty codes.</returns>
        /// <exception cref="RpoBusinessException">
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutViolationPaneltyCode(int id, ViolationPaneltyCode violationPaneltyCode)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (id != violationPaneltyCode.Id)
                {
                    return this.BadRequest();
                }

                if (this.ViolationPaneltyCodeExists(violationPaneltyCode.PaneltyCode, violationPaneltyCode.Id))
                {
                    throw new RpoBusinessException(StaticMessages.ViolationPaneltyCodeExistsMessage);
                }
                
                violationPaneltyCode.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    violationPaneltyCode.LastModifiedBy = employee.Id;
                }

                this.rpoContext.Entry(violationPaneltyCode).State = EntityState.Modified;

                try
                {
                    this.rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.ViolationPaneltyCodeExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                ViolationPaneltyCode violationPaneltyCodeResponse = this.rpoContext.ViolationPaneltyCodes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return this.Ok(this.FormatDetails(violationPaneltyCodeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }

        }

        /// <summary>
        /// Posts the violation penalty code.
        /// </summary>
        /// <param name="violationPaneltyCode">The violation penalty code.</param>
        /// <returns>create the violation penalty codes.</returns>
        /// <exception cref="RpoBusinessException">
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ViolationPaneltyCode))]
        public IHttpActionResult PostViolationPaneltyCode(ViolationPaneltyCode violationPaneltyCode)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (this.ViolationPaneltyCodeExists(violationPaneltyCode.PaneltyCode, violationPaneltyCode.Id))
                {
                    throw new RpoBusinessException(StaticMessages.ViolationPaneltyCodeExistsMessage);
                }
                violationPaneltyCode.LastModifiedDate = DateTime.UtcNow;
                violationPaneltyCode.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    violationPaneltyCode.CreatedBy = employee.Id;
                }

                this.rpoContext.ViolationPaneltyCodes.Add(violationPaneltyCode);
                this.rpoContext.SaveChanges();

                ViolationPaneltyCode violationPaneltyCodeResponse = this.rpoContext.ViolationPaneltyCodes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == violationPaneltyCode.Id);
                return this.Ok(this.FormatDetails(violationPaneltyCodeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the violation penalty code.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns> Deletes the violation penalty code.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ViolationPaneltyCode))]
        public IHttpActionResult DeleteViolationPaneltyCode(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                ViolationPaneltyCode violationPaneltyCode = this.rpoContext.ViolationPaneltyCodes.Find(id);
                if (violationPaneltyCode == null)
                {
                    return this.NotFound();
                }

                this.rpoContext.ViolationPaneltyCodes.Remove(violationPaneltyCode);
                this.rpoContext.SaveChanges();

                return this.Ok(violationPaneltyCode);
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
        /// Violations the penalty code exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ViolationPaneltyCodeExists(int id)
        {
            return this.rpoContext.ViolationPaneltyCodes.Count(e => e.Id == id) > 0;
        }
        
        /// <summary>
        /// Formats the specified violation penalty code.
        /// </summary>
        /// <param name="violationPaneltyCode">The violation penalty code.</param>
        /// <returns>Violation penalty Code DTO.</returns>
        private ViolationPaneltyCodeDTO Format(ViolationPaneltyCode violationPaneltyCode)
        {
            string currentTimeZone = Common.FetchHeaderValues(Request, Common.CurrentTimezoneHeaderKey);

            return new ViolationPaneltyCodeDTO
            {
                Id = violationPaneltyCode.Id,
                PaneltyCode = violationPaneltyCode.PaneltyCode,
                CodeSection = violationPaneltyCode.CodeSection,
                Description = violationPaneltyCode.Description,
                CreatedBy = violationPaneltyCode.CreatedBy,
                LastModifiedBy = violationPaneltyCode.LastModifiedBy != null ? violationPaneltyCode.LastModifiedBy : violationPaneltyCode.CreatedBy,
                CreatedByEmployeeName = violationPaneltyCode.CreatedByEmployee != null ? violationPaneltyCode.CreatedByEmployee.FirstName + " " + violationPaneltyCode.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = violationPaneltyCode.LastModifiedByEmployee != null ? violationPaneltyCode.LastModifiedByEmployee.FirstName + " " + violationPaneltyCode.LastModifiedByEmployee.LastName : (violationPaneltyCode.CreatedByEmployee != null ? violationPaneltyCode.CreatedByEmployee.FirstName + " " + violationPaneltyCode.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = violationPaneltyCode.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(violationPaneltyCode.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : violationPaneltyCode.CreatedDate,
                LastModifiedDate = violationPaneltyCode.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(violationPaneltyCode.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (violationPaneltyCode.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(violationPaneltyCode.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : violationPaneltyCode.CreatedDate),
            };
        }
        
        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="violationPaneltyCode">The violation penalty code.</param>
        /// <returns>Violation penalty Code Details.</returns>
        private ViolationPaneltyCodeDetails FormatDetails(ViolationPaneltyCode violationPaneltyCode)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new ViolationPaneltyCodeDetails
            {
                Id = violationPaneltyCode.Id,
                PaneltyCode = violationPaneltyCode.PaneltyCode,
                CodeSection = violationPaneltyCode.CodeSection,
                CreatedBy = violationPaneltyCode.CreatedBy,
                Description = violationPaneltyCode.Description,
                LastModifiedBy = violationPaneltyCode.LastModifiedBy != null ? violationPaneltyCode.LastModifiedBy : violationPaneltyCode.CreatedBy,
                CreatedByEmployeeName = violationPaneltyCode.CreatedByEmployee != null ? violationPaneltyCode.CreatedByEmployee.FirstName + " " + violationPaneltyCode.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = violationPaneltyCode.LastModifiedByEmployee != null ? violationPaneltyCode.LastModifiedByEmployee.FirstName + " " + violationPaneltyCode.LastModifiedByEmployee.LastName : (violationPaneltyCode.CreatedByEmployee != null ? violationPaneltyCode.CreatedByEmployee.FirstName + " " + violationPaneltyCode.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = violationPaneltyCode.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(violationPaneltyCode.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : violationPaneltyCode.CreatedDate,
                LastModifiedDate = violationPaneltyCode.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(violationPaneltyCode.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (violationPaneltyCode.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(violationPaneltyCode.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : violationPaneltyCode.CreatedDate),
            };
        }

        /// <summary>
        /// Violations the penalty code exists.
        /// </summary>
        /// <param name="paneltyCode">The penalty code.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ViolationPaneltyCodeExists(string paneltyCode, int id)
        {
            return this.rpoContext.ViolationPaneltyCodes.Count(e => e.PaneltyCode == paneltyCode && e.Id != id) > 0;
        }
    }
}
