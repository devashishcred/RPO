// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-15-2018
// ***********************************************************************
// <copyright file="FDNYPenaltySchedulesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Address Types Controller.</summary>
// ***********************************************************************

/// <summary>
/// The FDNYPenaltySchedules namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.FDNYPenaltySchedules
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
    public class FDNYPenaltySchedulesController : ApiController
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
        public IHttpActionResult GetFDNYPenaltySchedules([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var fdnyPenaltySchedules = this.rpoContext.FDNYPenaltySchedules.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = fdnyPenaltySchedules.Count();
                var recordsFiltered = recordsTotal;

                var result = fdnyPenaltySchedules
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
        [ResponseType(typeof(FDNYPenaltyScheduleDetail))]
        public IHttpActionResult GetFDNYPenaltySchedule(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                FDNYPenaltySchedule fdnyPenaltySchedule = this.rpoContext.FDNYPenaltySchedules.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (fdnyPenaltySchedule == null)
                {
                    return this.NotFound();
                }

                return this.Ok(this.FormatDetails(fdnyPenaltySchedule));
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
        /// <param name="fdnyPenaltySchedule">Type of the address.</param>
        /// <returns>IHttp Action Result.</returns>
        /// <exception cref="RpoBusinessException">
        /// Business Exception.
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFDNYPenaltySchedule(int id, FDNYPenaltySchedule fdnyPenaltySchedule)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (id != fdnyPenaltySchedule.Id)
                {
                    return this.BadRequest();
                }

                if (this.FDNYPenaltyScheduleOATHViolationCodeExists(fdnyPenaltySchedule.OATHViolationCode, fdnyPenaltySchedule.Id))
                {
                    throw new RpoBusinessException(StaticMessages.FDNYPenaltyScheduleOATHViolationCodeExistsMessage);
                }

                fdnyPenaltySchedule.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    fdnyPenaltySchedule.LastModifiedBy = employee.Id;
                }

                this.rpoContext.Entry(fdnyPenaltySchedule).State = EntityState.Modified;

                try
                {
                    this.rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.FDNYPenaltyScheduleExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                FDNYPenaltySchedule fdnyPenaltyScheduleResponse = this.rpoContext.FDNYPenaltySchedules.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return this.Ok(this.FormatDetails(fdnyPenaltyScheduleResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the type of the address.
        /// </summary>
        /// <param name="fdnyPenaltySchedule">Type of the address.</param>
        /// <returns>IHttp Action Result.</returns>
        /// <exception cref="RpoBusinessException">
        /// Business Exception.
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(FDNYPenaltySchedule))]
        public IHttpActionResult PostFDNYPenaltySchedule(FDNYPenaltySchedule fdnyPenaltySchedule)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (this.FDNYPenaltyScheduleOATHViolationCodeExists(fdnyPenaltySchedule.OATHViolationCode, fdnyPenaltySchedule.Id))
                {
                    throw new RpoBusinessException(StaticMessages.FDNYPenaltyScheduleOATHViolationCodeExistsMessage);
                }

                fdnyPenaltySchedule.LastModifiedDate = DateTime.UtcNow;
                fdnyPenaltySchedule.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    fdnyPenaltySchedule.CreatedBy = employee.Id;
                }

                this.rpoContext.FDNYPenaltySchedules.Add(fdnyPenaltySchedule);
                this.rpoContext.SaveChanges();

                FDNYPenaltySchedule fdnyPenaltyScheduleResponse = this.rpoContext.FDNYPenaltySchedules.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == fdnyPenaltySchedule.Id);
                return this.Ok(this.FormatDetails(fdnyPenaltyScheduleResponse));
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
        [ResponseType(typeof(FDNYPenaltySchedule))]
        public IHttpActionResult DeleteFDNYPenaltySchedule(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                FDNYPenaltySchedule fdnyPenaltySchedule = this.rpoContext.FDNYPenaltySchedules.Find(id);
                if (fdnyPenaltySchedule == null)
                {
                    return this.NotFound();
                }

                this.rpoContext.FDNYPenaltySchedules.Remove(fdnyPenaltySchedule);
                this.rpoContext.SaveChanges();

                return this.Ok(fdnyPenaltySchedule);
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
        private bool FDNYPenaltyScheduleExists(int id)
        {
            return this.rpoContext.FDNYPenaltySchedules.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified address type.
        /// </summary>
        /// <param name="fdnyPenaltySchedule">Type of the address.</param>
        /// <returns>Address Type DTO.</returns>
        private FDNYPenaltyScheduleDTO Format(FDNYPenaltySchedule fdnyPenaltySchedule)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new FDNYPenaltyScheduleDTO
            {
                Id = fdnyPenaltySchedule.Id,
                Category_RCNY = fdnyPenaltySchedule.Category_RCNY,
                DescriptionOfViolation = fdnyPenaltySchedule.DescriptionOfViolation,
                OATHViolationCode = fdnyPenaltySchedule.OATHViolationCode,
                FirstViolationPenalty = fdnyPenaltySchedule.FirstViolationPenalty,
                FirstViolationMitigatedPenalty = fdnyPenaltySchedule.FirstViolationMitigatedPenalty,
                FirstViolationMaximumPenalty = fdnyPenaltySchedule.FirstViolationMaximumPenalty,
                SecondSubsequentViolationPenalty = fdnyPenaltySchedule.SecondSubsequentViolationPenalty,
                SecondSubsequentViolationMitigatedPenalty = fdnyPenaltySchedule.SecondSubsequentViolationMitigatedPenalty,
                SecondSubsequentViolationMaximumPenalty = fdnyPenaltySchedule.SecondSubsequentViolationMaximumPenalty,
                FormattedFirstViolationPenalty = fdnyPenaltySchedule.FirstViolationPenalty != null ? Convert.ToDouble(fdnyPenaltySchedule.FirstViolationPenalty).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedFirstViolationMitigatedPenalty = fdnyPenaltySchedule.FirstViolationMitigatedPenalty != null ? Convert.ToDouble(fdnyPenaltySchedule.FirstViolationMitigatedPenalty).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedFirstViolationMaximumPenalty = fdnyPenaltySchedule.FirstViolationMaximumPenalty != null ? Convert.ToDouble(fdnyPenaltySchedule.FirstViolationMaximumPenalty).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedSecondSubsequentViolationPenalty = fdnyPenaltySchedule.SecondSubsequentViolationPenalty != null ? Convert.ToDouble(fdnyPenaltySchedule.SecondSubsequentViolationPenalty).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedSecondSubsequentViolationMitigatedPenalty = fdnyPenaltySchedule.SecondSubsequentViolationMitigatedPenalty != null ? Convert.ToDouble(fdnyPenaltySchedule.SecondSubsequentViolationMitigatedPenalty).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedSecondSubsequentViolationMaximumPenalty = fdnyPenaltySchedule.SecondSubsequentViolationMaximumPenalty != null ? Convert.ToDouble(fdnyPenaltySchedule.SecondSubsequentViolationMaximumPenalty).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                CreatedBy = fdnyPenaltySchedule.CreatedBy,
                LastModifiedBy = fdnyPenaltySchedule.LastModifiedBy != null ? fdnyPenaltySchedule.LastModifiedBy : fdnyPenaltySchedule.CreatedBy,
                CreatedByEmployeeName = fdnyPenaltySchedule.CreatedByEmployee != null ? fdnyPenaltySchedule.CreatedByEmployee.FirstName + " " + fdnyPenaltySchedule.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = fdnyPenaltySchedule.LastModifiedByEmployee != null ? fdnyPenaltySchedule.LastModifiedByEmployee.FirstName + " " + fdnyPenaltySchedule.LastModifiedByEmployee.LastName : (fdnyPenaltySchedule.CreatedByEmployee != null ? fdnyPenaltySchedule.CreatedByEmployee.FirstName + " " + fdnyPenaltySchedule.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = fdnyPenaltySchedule.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(fdnyPenaltySchedule.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : fdnyPenaltySchedule.CreatedDate,
                LastModifiedDate = fdnyPenaltySchedule.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(fdnyPenaltySchedule.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (fdnyPenaltySchedule.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(fdnyPenaltySchedule.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : fdnyPenaltySchedule.CreatedDate),
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="fdnyPenaltySchedule">Type of the address.</param>
        /// <returns>Address Type Detail.</returns>
        private FDNYPenaltyScheduleDetail FormatDetails(FDNYPenaltySchedule fdnyPenaltySchedule)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new FDNYPenaltyScheduleDetail
            {
                Id = fdnyPenaltySchedule.Id,
                Category_RCNY = fdnyPenaltySchedule.Category_RCNY,
                DescriptionOfViolation = fdnyPenaltySchedule.DescriptionOfViolation,
                OATHViolationCode = fdnyPenaltySchedule.OATHViolationCode,
                FirstViolationPenalty = fdnyPenaltySchedule.FirstViolationPenalty,
                FirstViolationMitigatedPenalty = fdnyPenaltySchedule.FirstViolationMitigatedPenalty,
                FirstViolationMaximumPenalty = fdnyPenaltySchedule.FirstViolationMaximumPenalty,
                SecondSubsequentViolationPenalty = fdnyPenaltySchedule.SecondSubsequentViolationPenalty,
                SecondSubsequentViolationMitigatedPenalty = fdnyPenaltySchedule.SecondSubsequentViolationMitigatedPenalty,
                SecondSubsequentViolationMaximumPenalty = fdnyPenaltySchedule.SecondSubsequentViolationMaximumPenalty,
                FormattedFirstViolationPenalty = fdnyPenaltySchedule.FirstViolationPenalty != null ? Convert.ToDouble(fdnyPenaltySchedule.FirstViolationPenalty).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedFirstViolationMitigatedPenalty = fdnyPenaltySchedule.FirstViolationMitigatedPenalty != null ? Convert.ToDouble(fdnyPenaltySchedule.FirstViolationMitigatedPenalty).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedFirstViolationMaximumPenalty = fdnyPenaltySchedule.FirstViolationMaximumPenalty != null ? Convert.ToDouble(fdnyPenaltySchedule.FirstViolationMaximumPenalty).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedSecondSubsequentViolationPenalty = fdnyPenaltySchedule.SecondSubsequentViolationPenalty != null ? Convert.ToDouble(fdnyPenaltySchedule.SecondSubsequentViolationPenalty).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedSecondSubsequentViolationMitigatedPenalty = fdnyPenaltySchedule.SecondSubsequentViolationMitigatedPenalty != null ? Convert.ToDouble(fdnyPenaltySchedule.SecondSubsequentViolationMitigatedPenalty).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedSecondSubsequentViolationMaximumPenalty = fdnyPenaltySchedule.SecondSubsequentViolationMaximumPenalty != null ? Convert.ToDouble(fdnyPenaltySchedule.SecondSubsequentViolationMaximumPenalty).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                CreatedBy = fdnyPenaltySchedule.CreatedBy,
                LastModifiedBy = fdnyPenaltySchedule.LastModifiedBy != null ? fdnyPenaltySchedule.LastModifiedBy : fdnyPenaltySchedule.CreatedBy,
                CreatedByEmployeeName = fdnyPenaltySchedule.CreatedByEmployee != null ? fdnyPenaltySchedule.CreatedByEmployee.FirstName + " " + fdnyPenaltySchedule.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = fdnyPenaltySchedule.LastModifiedByEmployee != null ? fdnyPenaltySchedule.LastModifiedByEmployee.FirstName + " " + fdnyPenaltySchedule.LastModifiedByEmployee.LastName : (fdnyPenaltySchedule.CreatedByEmployee != null ? fdnyPenaltySchedule.CreatedByEmployee.FirstName + " " + fdnyPenaltySchedule.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = fdnyPenaltySchedule.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(fdnyPenaltySchedule.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : fdnyPenaltySchedule.CreatedDate,
                LastModifiedDate = fdnyPenaltySchedule.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(fdnyPenaltySchedule.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (fdnyPenaltySchedule.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(fdnyPenaltySchedule.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : fdnyPenaltySchedule.CreatedDate),
            };
        }

        /// <summary>
        /// Addresses the type name exists.
        /// </summary>
        /// <param name="oathViolationCode">The name address type.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>True</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool FDNYPenaltyScheduleOATHViolationCodeExists(string oathViolationCode, int id)
        {
            return this.rpoContext.FDNYPenaltySchedules.Count(e => e.OATHViolationCode == oathViolationCode && e.Id != id) > 0;
        }
    }
}