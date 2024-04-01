// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-15-2018
// ***********************************************************************
// <copyright file="SystemSettingsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class System Settings Controller.</summary>
// ***********************************************************************

/// <summary>
/// The System Settings namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.SystemSettings
{
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Rpo.ApiServices.Api.Tools;
    using System;
    using Rpo.ApiServices.Api.DataTable;
    using Filters;
    using Model.Models.Enums;
    using System.Collections.Generic;
    using Employees;

    /// <summary>
    /// Class System Settings Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />

    public class SystemSettingsController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the system settings.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the system settings Lists.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetSystemSettings([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {

                var systemSettings = rpoContext.SystemSettings.Include("CreatedByEmployee").Include("LastModified").AsQueryable();

                var recordsTotal = systemSettings.Count();
                var recordsFiltered = recordsTotal;

                var result = systemSettings
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
        /// Gets the system setting dropdown.
        /// </summary>
        /// <returns>Gets the system setting dropdown.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/systemsettings/dropdown")]
        public IHttpActionResult GetSystemSettingDropdown()
        {
            var result = rpoContext.SystemSettings.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the system setting.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the system setting .</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(SystemSettingDetail))]
        public IHttpActionResult GetSystemSetting(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                SystemSetting systemSetting = rpoContext.SystemSettings.Include("CreatedByEmployee").Include("LastModified").FirstOrDefault(x => x.Id == id);

                if (systemSetting == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(systemSetting));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }


        /// <summary>
        /// Puts the system setting.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="systemSettingDetail">The system setting detail.</param>
        /// <returns>update the system setting.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(SystemSettingDetail))]
        public IHttpActionResult PutSystemSetting(int id, SystemSettingCreateOrUpdate systemSettingDetail)
        {
            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != systemSettingDetail.Id)
                {
                    return BadRequest();
                }

                var systemSetting = rpoContext.SystemSettings.FirstOrDefault(e => e.Id == id);

                if (systemSetting == null)
                {
                    return NotFound();
                }

                if (systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                {
                    systemSetting.Value = string.Join(",", systemSettingDetail.Value.Select(x => x.ToString()));
                }
                else
                {
                    systemSetting.Value = string.Empty;
                }
                systemSetting.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    systemSetting.LastModifiedBy = employee.Id;
                }

                rpoContext.Entry(systemSetting).State = EntityState.Modified;

                try
                {
                    rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SystemSettingExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                SystemSetting systemSettingResponse = rpoContext.SystemSettings.Include("CreatedByEmployee").Include("LastModified").FirstOrDefault(x => x.Id == id);
                return Ok(FormatDetails(systemSettingResponse));
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
                rpoContext.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Systems the setting exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool SystemSettingExists(int id)
        {
            return rpoContext.SystemSettings.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified system setting.
        /// </summary>
        /// <param name="systemSetting">The system setting.</param>
        /// <returns>SystemSettingDTO.</returns>
        private SystemSettingDTO Format(SystemSetting systemSetting)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            List<int> employeeValue = systemSetting.Value != null && !string.IsNullOrEmpty(systemSetting.Value) ? (systemSetting.Value.Split(',') != null && systemSetting.Value.Split(',').Any() ? systemSetting.Value.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

            return new SystemSettingDTO
            {
                Id = systemSetting.Id,
                Name = systemSetting.Name,
                CreatedBy = systemSetting.CreatedBy,
                LastModifiedBy = systemSetting.LastModifiedBy,
                CreatedByEmployeeName = systemSetting.CreatedByEmployee != null ? systemSetting.CreatedByEmployee.FirstName + " " + systemSetting.CreatedByEmployee.LastName : string.Empty,
                LastModified = systemSetting.LastModified != null ? systemSetting.LastModified.FirstName + " " + systemSetting.LastModified.LastName : string.Empty,
                CreatedDate = systemSetting.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(systemSetting.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : systemSetting.CreatedDate,
                LastModifiedDate = systemSetting.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(systemSetting.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : systemSetting.LastModifiedDate,
                Value = string.Join(", ", rpoContext.Employees.Where(c => employeeValue.Contains(c.Id))
                .Select(c => c.Email
                //(c.FirstName +
                //(!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty)
                //+ (!string.IsNullOrWhiteSpace(c.Email) ? " (" + c.Email + ")" : string.Empty))
                ))
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="systemSetting">The system setting.</param>
        /// <returns>SystemSettingDetail.</returns>
        private SystemSettingDetail FormatDetails(SystemSetting systemSetting)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            List<int> employeeValue = systemSetting.Value != null && !string.IsNullOrEmpty(systemSetting.Value) ? (systemSetting.Value.Split(',') != null && systemSetting.Value.Split(',').Any() ? systemSetting.Value.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

            return new SystemSettingDetail
            {
                Id = systemSetting.Id,
                Name = systemSetting.Name,
                CreatedBy = systemSetting.CreatedBy,
                LastModifiedBy = systemSetting.LastModifiedBy,
                CreatedByEmployeeName = systemSetting.CreatedByEmployee != null ? systemSetting.CreatedByEmployee.FirstName + " " + systemSetting.CreatedByEmployee.LastName : string.Empty,
                LastModified = systemSetting.LastModified != null ? systemSetting.LastModified.FirstName + " " + systemSetting.LastModified.LastName : string.Empty,
                CreatedDate = systemSetting.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(systemSetting.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : systemSetting.CreatedDate,
                LastModifiedDate = systemSetting.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(systemSetting.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : systemSetting.LastModifiedDate,
                Value = rpoContext.Employees.Where(c => employeeValue.Contains(c.Id)).AsEnumerable().Select(c => new EmployeeDetail()
                {
                    Id = c.Id,
                    EmployeeName = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty),
                    ItemName = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty) + (!string.IsNullOrWhiteSpace(c.Email) ? " (" + c.Email + ")" : string.Empty),
                    Email = c.Email,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    IdEmployee = c.Id,
                }),
            };

        }

        /// <summary>
        /// Systems the setting name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool SystemSettingNameExists(string name, int id)
        {
            return rpoContext.SystemSettings.Count(e => e.Name == name && e.Id != id) > 0;
        }
    }
}