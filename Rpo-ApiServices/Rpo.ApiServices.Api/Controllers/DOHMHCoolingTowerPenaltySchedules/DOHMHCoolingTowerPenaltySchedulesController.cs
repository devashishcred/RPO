// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-15-2018
// ***********************************************************************
// <copyright file="DOHMHCoolingTowerPenaltySchedulesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Address Types Controller.</summary>
// ***********************************************************************

/// <summary>
/// The DOHMHCoolingTowerPenaltySchedules namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.DOHMHCoolingTowerPenaltySchedules
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
    using System.Globalization;
    /// <summary>
    /// Class Address Types Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class DOHMHCoolingTowerPenaltySchedulesController : ApiController
    {
        /// <summary>
        /// The database.
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the DOB Penalty Schedule.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttp Action Result.</returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetDOHMHCoolingTowerPenaltySchedules([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var dohmhCoolingTowerPenaltySchedules = this.rpoContext.DOHMHCoolingTowerPenaltySchedules.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = dohmhCoolingTowerPenaltySchedules.Count();
                var recordsFiltered = recordsTotal;

                var result = dohmhCoolingTowerPenaltySchedules
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
        /// Gets the type of the address.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttp Action Result.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DOHMHCoolingTowerPenaltyScheduleDetail))]
        public IHttpActionResult GetDOHMHCoolingTowerPenaltySchedule(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                DOHMHCoolingTowerPenaltySchedule dohmhCoolingTowerPenaltySchedule = this.rpoContext.DOHMHCoolingTowerPenaltySchedules.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (dohmhCoolingTowerPenaltySchedule == null)
                {
                    return this.NotFound();
                }

                return this.Ok(this.FormatDetails(dohmhCoolingTowerPenaltySchedule));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the type of the address.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dohmhCoolingTowerPenaltySchedule">Type of the address.</param>
        /// <returns>IHttp Action Result.</returns>
        /// <exception cref="RpoBusinessException">
        /// Business Exception.
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDOHMHCoolingTowerPenaltySchedule(int id, DOHMHCoolingTowerPenaltySchedule dohmhCoolingTowerPenaltySchedule)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (id != dohmhCoolingTowerPenaltySchedule.Id)
                {
                    return this.BadRequest();
                }

                dohmhCoolingTowerPenaltySchedule.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    dohmhCoolingTowerPenaltySchedule.LastModifiedBy = employee.Id;
                }

                this.rpoContext.Entry(dohmhCoolingTowerPenaltySchedule).State = EntityState.Modified;

                try
                {
                    this.rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.DOHMHCoolingTowerPenaltyScheduleExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                DOHMHCoolingTowerPenaltySchedule dohmhCoolingTowerPenaltyScheduleResponse = this.rpoContext.DOHMHCoolingTowerPenaltySchedules.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return this.Ok(this.FormatDetails(dohmhCoolingTowerPenaltyScheduleResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the type of the address.
        /// </summary>
        /// <param name="dohmhCoolingTowerPenaltySchedule">Type of the address.</param>
        /// <returns>IHttp Action Result.</returns>
        /// <exception cref="RpoBusinessException">
        /// Business Exception.
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DOHMHCoolingTowerPenaltySchedule))]
        public IHttpActionResult PostDOHMHCoolingTowerPenaltySchedule(DOHMHCoolingTowerPenaltySchedule dohmhCoolingTowerPenaltySchedule)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                dohmhCoolingTowerPenaltySchedule.LastModifiedDate = DateTime.UtcNow;
                dohmhCoolingTowerPenaltySchedule.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    dohmhCoolingTowerPenaltySchedule.CreatedBy = employee.Id;
                }

                this.rpoContext.DOHMHCoolingTowerPenaltySchedules.Add(dohmhCoolingTowerPenaltySchedule);
                this.rpoContext.SaveChanges();

                DOHMHCoolingTowerPenaltySchedule dohmhCoolingTowerPenaltyScheduleResponse = this.rpoContext.DOHMHCoolingTowerPenaltySchedules.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == dohmhCoolingTowerPenaltySchedule.Id);
                return this.Ok(this.FormatDetails(dohmhCoolingTowerPenaltyScheduleResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the type of the address.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttp Action Result.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DOHMHCoolingTowerPenaltySchedule))]
        public IHttpActionResult DeleteDOHMHCoolingTowerPenaltySchedule(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                DOHMHCoolingTowerPenaltySchedule dohmhCoolingTowerPenaltySchedule = this.rpoContext.DOHMHCoolingTowerPenaltySchedules.Find(id);
                if (dohmhCoolingTowerPenaltySchedule == null)
                {
                    return this.NotFound();
                }

                this.rpoContext.DOHMHCoolingTowerPenaltySchedules.Remove(dohmhCoolingTowerPenaltySchedule);
                this.rpoContext.SaveChanges();

                return this.Ok(dohmhCoolingTowerPenaltySchedule);
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

        /// <summary>
        /// Addresses the type exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool DOHMHCoolingTowerPenaltyScheduleExists(int id)
        {
            return this.rpoContext.DOHMHCoolingTowerPenaltySchedules.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified address type.
        /// </summary>
        /// <param name="dohmhCoolingTowerPenaltySchedule">Type of the address.</param>
        /// <returns>Address Type DTO.</returns>
        private DOHMHCoolingTowerPenaltyScheduleDTO Format(DOHMHCoolingTowerPenaltySchedule dohmhCoolingTowerPenaltySchedule)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new DOHMHCoolingTowerPenaltyScheduleDTO
            {
                Id = dohmhCoolingTowerPenaltySchedule.Id,
                SectionOfLaw = dohmhCoolingTowerPenaltySchedule.SectionOfLaw,
                Description = dohmhCoolingTowerPenaltySchedule.Description,
                PenaltyFirstViolation = dohmhCoolingTowerPenaltySchedule.PenaltyFirstViolation,
                PenaltyRepeatViolation = dohmhCoolingTowerPenaltySchedule.PenaltyRepeatViolation,
                FormattedPenaltyFirstViolation = dohmhCoolingTowerPenaltySchedule.PenaltyFirstViolation != null ? Convert.ToDouble(dohmhCoolingTowerPenaltySchedule.PenaltyFirstViolation).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedPenaltyRepeatViolation = dohmhCoolingTowerPenaltySchedule.PenaltyRepeatViolation != null ? Convert.ToDouble(dohmhCoolingTowerPenaltySchedule.PenaltyRepeatViolation).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                CreatedBy = dohmhCoolingTowerPenaltySchedule.CreatedBy,
                LastModifiedBy = dohmhCoolingTowerPenaltySchedule.LastModifiedBy != null ? dohmhCoolingTowerPenaltySchedule.LastModifiedBy : dohmhCoolingTowerPenaltySchedule.CreatedBy,
                CreatedByEmployeeName = dohmhCoolingTowerPenaltySchedule.CreatedByEmployee != null ? dohmhCoolingTowerPenaltySchedule.CreatedByEmployee.FirstName + " " + dohmhCoolingTowerPenaltySchedule.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = dohmhCoolingTowerPenaltySchedule.LastModifiedByEmployee != null ? dohmhCoolingTowerPenaltySchedule.LastModifiedByEmployee.FirstName + " " + dohmhCoolingTowerPenaltySchedule.LastModifiedByEmployee.LastName : (dohmhCoolingTowerPenaltySchedule.CreatedByEmployee != null ? dohmhCoolingTowerPenaltySchedule.CreatedByEmployee.FirstName + " " + dohmhCoolingTowerPenaltySchedule.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = dohmhCoolingTowerPenaltySchedule.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(dohmhCoolingTowerPenaltySchedule.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : dohmhCoolingTowerPenaltySchedule.CreatedDate,
                LastModifiedDate = dohmhCoolingTowerPenaltySchedule.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(dohmhCoolingTowerPenaltySchedule.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (dohmhCoolingTowerPenaltySchedule.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(dohmhCoolingTowerPenaltySchedule.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : dohmhCoolingTowerPenaltySchedule.CreatedDate),
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="dohmhCoolingTowerPenaltySchedule">Type of the address.</param>
        /// <returns>Address Type Detail.</returns>
        private DOHMHCoolingTowerPenaltyScheduleDetail FormatDetails(DOHMHCoolingTowerPenaltySchedule dohmhCoolingTowerPenaltySchedule)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new DOHMHCoolingTowerPenaltyScheduleDetail
            {
                Id = dohmhCoolingTowerPenaltySchedule.Id,
                SectionOfLaw = dohmhCoolingTowerPenaltySchedule.SectionOfLaw,
                Description = dohmhCoolingTowerPenaltySchedule.Description,
                PenaltyFirstViolation = dohmhCoolingTowerPenaltySchedule.PenaltyFirstViolation,
                PenaltyRepeatViolation = dohmhCoolingTowerPenaltySchedule.PenaltyRepeatViolation,
                FormattedPenaltyFirstViolation = dohmhCoolingTowerPenaltySchedule.PenaltyFirstViolation != null ? Convert.ToDouble(dohmhCoolingTowerPenaltySchedule.PenaltyFirstViolation).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedPenaltyRepeatViolation = dohmhCoolingTowerPenaltySchedule.PenaltyRepeatViolation != null ? Convert.ToDouble(dohmhCoolingTowerPenaltySchedule.PenaltyRepeatViolation).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                CreatedBy = dohmhCoolingTowerPenaltySchedule.CreatedBy,
                LastModifiedBy = dohmhCoolingTowerPenaltySchedule.LastModifiedBy != null ? dohmhCoolingTowerPenaltySchedule.LastModifiedBy : dohmhCoolingTowerPenaltySchedule.CreatedBy,
                CreatedByEmployeeName = dohmhCoolingTowerPenaltySchedule.CreatedByEmployee != null ? dohmhCoolingTowerPenaltySchedule.CreatedByEmployee.FirstName + " " + dohmhCoolingTowerPenaltySchedule.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = dohmhCoolingTowerPenaltySchedule.LastModifiedByEmployee != null ? dohmhCoolingTowerPenaltySchedule.LastModifiedByEmployee.FirstName + " " + dohmhCoolingTowerPenaltySchedule.LastModifiedByEmployee.LastName : (dohmhCoolingTowerPenaltySchedule.CreatedByEmployee != null ? dohmhCoolingTowerPenaltySchedule.CreatedByEmployee.FirstName + " " + dohmhCoolingTowerPenaltySchedule.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = dohmhCoolingTowerPenaltySchedule.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(dohmhCoolingTowerPenaltySchedule.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : dohmhCoolingTowerPenaltySchedule.CreatedDate,
                LastModifiedDate = dohmhCoolingTowerPenaltySchedule.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(dohmhCoolingTowerPenaltySchedule.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (dohmhCoolingTowerPenaltySchedule.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(dohmhCoolingTowerPenaltySchedule.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : dohmhCoolingTowerPenaltySchedule.CreatedDate),
            };
        }
    }
}