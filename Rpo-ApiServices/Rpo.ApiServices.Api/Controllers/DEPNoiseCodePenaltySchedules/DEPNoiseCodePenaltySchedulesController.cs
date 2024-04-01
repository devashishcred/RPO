// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-15-2018
// ***********************************************************************
// <copyright file="DEPNoiseCodePenaltySchedulesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Address Types Controller.</summary>
// ***********************************************************************

/// <summary>
/// The DEPNoiseCodePenaltySchedules namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.DEPNoiseCodePenaltySchedules
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
    public class DEPNoiseCodePenaltySchedulesController : ApiController
    {
        /// <summary>
        /// The database.
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the DOB Penalty Schedule.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the DOB Penalty Schedule List.</returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetDEPNoiseCodePenaltySchedules([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var depNoiseCodePenaltySchedules = this.rpoContext.DEPNoiseCodePenaltySchedules.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = depNoiseCodePenaltySchedules.Count();
                var recordsFiltered = recordsTotal;

                var result = depNoiseCodePenaltySchedules
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
        /// <returns>Gets the DOB Penalty Schedule in detail.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DEPNoiseCodePenaltyScheduleDetail))]
        public IHttpActionResult GetDEPNoiseCodePenaltySchedule(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                DEPNoiseCodePenaltySchedule depNoiseCodePenaltySchedule = this.rpoContext.DEPNoiseCodePenaltySchedules.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (depNoiseCodePenaltySchedule == null)
                {
                    return this.NotFound();
                }

                return this.Ok(this.FormatDetails(depNoiseCodePenaltySchedule));
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
        /// <param name="depNoiseCodePenaltySchedule">Type of the address.</param>
        /// <returns>IHttp Action Result.</returns>
        /// <exception cref="RpoBusinessException">
        /// Business Exception.
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDEPNoiseCodePenaltySchedule(int id, DEPNoiseCodePenaltySchedule depNoiseCodePenaltySchedule)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (id != depNoiseCodePenaltySchedule.Id)
                {
                    return this.BadRequest();
                }

                depNoiseCodePenaltySchedule.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    depNoiseCodePenaltySchedule.LastModifiedBy = employee.Id;
                }

                this.rpoContext.Entry(depNoiseCodePenaltySchedule).State = EntityState.Modified;

                try
                {
                    this.rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.DEPNoiseCodePenaltyScheduleExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                DEPNoiseCodePenaltySchedule depNoiseCodePenaltyScheduleResponse = this.rpoContext.DEPNoiseCodePenaltySchedules.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return this.Ok(this.FormatDetails(depNoiseCodePenaltyScheduleResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the type of the address.
        /// </summary>
        /// <param name="depNoiseCodePenaltySchedule">Type of the address.</param>
        /// <returns>IHttp Action Result.</returns>
        /// <exception cref="RpoBusinessException">
        /// Business Exception.
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DEPNoiseCodePenaltySchedule))]
        public IHttpActionResult PostDEPNoiseCodePenaltySchedule(DEPNoiseCodePenaltySchedule depNoiseCodePenaltySchedule)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }
                
                depNoiseCodePenaltySchedule.LastModifiedDate = DateTime.UtcNow;
                depNoiseCodePenaltySchedule.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    depNoiseCodePenaltySchedule.CreatedBy = employee.Id;
                }

                this.rpoContext.DEPNoiseCodePenaltySchedules.Add(depNoiseCodePenaltySchedule);
                this.rpoContext.SaveChanges();

                DEPNoiseCodePenaltySchedule depNoiseCodePenaltyScheduleResponse = this.rpoContext.DEPNoiseCodePenaltySchedules.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == depNoiseCodePenaltySchedule.Id);
                return this.Ok(this.FormatDetails(depNoiseCodePenaltyScheduleResponse));
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
        [ResponseType(typeof(DEPNoiseCodePenaltySchedule))]
        public IHttpActionResult DeleteDEPNoiseCodePenaltySchedule(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                DEPNoiseCodePenaltySchedule depNoiseCodePenaltySchedule = this.rpoContext.DEPNoiseCodePenaltySchedules.Find(id);
                if (depNoiseCodePenaltySchedule == null)
                {
                    return this.NotFound();
                }

                this.rpoContext.DEPNoiseCodePenaltySchedules.Remove(depNoiseCodePenaltySchedule);
                this.rpoContext.SaveChanges();

                return this.Ok(depNoiseCodePenaltySchedule);
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
        private bool DEPNoiseCodePenaltyScheduleExists(int id)
        {
            return this.rpoContext.DEPNoiseCodePenaltySchedules.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified address type.
        /// </summary>
        /// <param name="depNoiseCodePenaltySchedule">Type of the address.</param>
        /// <returns>Address Type DTO.</returns>
        private DEPNoiseCodePenaltyScheduleDTO Format(DEPNoiseCodePenaltySchedule depNoiseCodePenaltySchedule)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new DEPNoiseCodePenaltyScheduleDTO
            {
                Id = depNoiseCodePenaltySchedule.Id,
                SectionOfLaw = depNoiseCodePenaltySchedule.SectionOfLaw,
                ViolationDescription = depNoiseCodePenaltySchedule.ViolationDescription,
                Compliance = depNoiseCodePenaltySchedule.Compliance,
                Offense_1 = depNoiseCodePenaltySchedule.Offense_1,
                Offense_2 = depNoiseCodePenaltySchedule.Offense_2,
                Offense_3 = depNoiseCodePenaltySchedule.Offense_3,
                Offense_4 = depNoiseCodePenaltySchedule.Offense_4,
                Penalty_1 = depNoiseCodePenaltySchedule.Penalty_1,
                Penalty_2 = depNoiseCodePenaltySchedule.Penalty_2,
                Penalty_3 = depNoiseCodePenaltySchedule.Penalty_3,
                Penalty_4 = depNoiseCodePenaltySchedule.Penalty_4,
                DefaultPenalty_1 = depNoiseCodePenaltySchedule.DefaultPenalty_1,
                DefaultPenalty_2 = depNoiseCodePenaltySchedule.DefaultPenalty_2,
                DefaultPenalty_3 = depNoiseCodePenaltySchedule.DefaultPenalty_3,
                DefaultPenalty_4 = depNoiseCodePenaltySchedule.DefaultPenalty_4,
                Stipulation_1 = depNoiseCodePenaltySchedule.Stipulation_1,
                Stipulation_2 = depNoiseCodePenaltySchedule.Stipulation_2,
                Stipulation_3 = depNoiseCodePenaltySchedule.Stipulation_3,
                Stipulation_4 = depNoiseCodePenaltySchedule.Stipulation_4,
                FormattedPenalty_1 = depNoiseCodePenaltySchedule.Penalty_1 != null ? Convert.ToDouble(depNoiseCodePenaltySchedule.Penalty_1).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedPenalty_2 = depNoiseCodePenaltySchedule.Penalty_2 != null ? Convert.ToDouble(depNoiseCodePenaltySchedule.Penalty_2).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedPenalty_3 = depNoiseCodePenaltySchedule.Penalty_3 != null ? Convert.ToDouble(depNoiseCodePenaltySchedule.Penalty_3).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedPenalty_4 = depNoiseCodePenaltySchedule.Penalty_4 != null ? Convert.ToDouble(depNoiseCodePenaltySchedule.Penalty_4).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedDefaultPenalty_1 = depNoiseCodePenaltySchedule.DefaultPenalty_1 != null ? Convert.ToDouble(depNoiseCodePenaltySchedule.DefaultPenalty_1).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedDefaultPenalty_2 = depNoiseCodePenaltySchedule.DefaultPenalty_1 != null ? Convert.ToDouble(depNoiseCodePenaltySchedule.DefaultPenalty_2).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedDefaultPenalty_3 = depNoiseCodePenaltySchedule.DefaultPenalty_1 != null ? Convert.ToDouble(depNoiseCodePenaltySchedule.DefaultPenalty_3).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedDefaultPenalty_4 = depNoiseCodePenaltySchedule.DefaultPenalty_1 != null ? Convert.ToDouble(depNoiseCodePenaltySchedule.DefaultPenalty_4).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                CreatedBy = depNoiseCodePenaltySchedule.CreatedBy,
                LastModifiedBy = depNoiseCodePenaltySchedule.LastModifiedBy != null ? depNoiseCodePenaltySchedule.LastModifiedBy : depNoiseCodePenaltySchedule.CreatedBy,
                CreatedByEmployeeName = depNoiseCodePenaltySchedule.CreatedByEmployee != null ? depNoiseCodePenaltySchedule.CreatedByEmployee.FirstName + " " + depNoiseCodePenaltySchedule.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = depNoiseCodePenaltySchedule.LastModifiedByEmployee != null ? depNoiseCodePenaltySchedule.LastModifiedByEmployee.FirstName + " " + depNoiseCodePenaltySchedule.LastModifiedByEmployee.LastName : (depNoiseCodePenaltySchedule.CreatedByEmployee != null ? depNoiseCodePenaltySchedule.CreatedByEmployee.FirstName + " " + depNoiseCodePenaltySchedule.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = depNoiseCodePenaltySchedule.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(depNoiseCodePenaltySchedule.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : depNoiseCodePenaltySchedule.CreatedDate,
                LastModifiedDate = depNoiseCodePenaltySchedule.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(depNoiseCodePenaltySchedule.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (depNoiseCodePenaltySchedule.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(depNoiseCodePenaltySchedule.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : depNoiseCodePenaltySchedule.CreatedDate),
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="depNoiseCodePenaltySchedule">Type of the address.</param>
        /// <returns>Address Type Detail.</returns>
        private DEPNoiseCodePenaltyScheduleDetail FormatDetails(DEPNoiseCodePenaltySchedule depNoiseCodePenaltySchedule)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new DEPNoiseCodePenaltyScheduleDetail
            {
                Id = depNoiseCodePenaltySchedule.Id,
                SectionOfLaw = depNoiseCodePenaltySchedule.SectionOfLaw,
                ViolationDescription = depNoiseCodePenaltySchedule.ViolationDescription,
                Compliance = depNoiseCodePenaltySchedule.Compliance,
                Offense_1 = depNoiseCodePenaltySchedule.Offense_1,
                Offense_2 = depNoiseCodePenaltySchedule.Offense_2,
                Offense_3 = depNoiseCodePenaltySchedule.Offense_3,
                Offense_4 = depNoiseCodePenaltySchedule.Offense_4,
                Penalty_1 = depNoiseCodePenaltySchedule.Penalty_1,
                Penalty_2 = depNoiseCodePenaltySchedule.Penalty_2,
                Penalty_3 = depNoiseCodePenaltySchedule.Penalty_3,
                Penalty_4 = depNoiseCodePenaltySchedule.Penalty_4,
                DefaultPenalty_1 = depNoiseCodePenaltySchedule.DefaultPenalty_1,
                DefaultPenalty_2 = depNoiseCodePenaltySchedule.DefaultPenalty_2,
                DefaultPenalty_3 = depNoiseCodePenaltySchedule.DefaultPenalty_3,
                DefaultPenalty_4 = depNoiseCodePenaltySchedule.DefaultPenalty_4,
                Stipulation_1 = depNoiseCodePenaltySchedule.Stipulation_1,
                Stipulation_2 = depNoiseCodePenaltySchedule.Stipulation_2,
                Stipulation_3 = depNoiseCodePenaltySchedule.Stipulation_3,
                Stipulation_4 = depNoiseCodePenaltySchedule.Stipulation_4,
                FormattedPenalty_1 = depNoiseCodePenaltySchedule.Penalty_1 != null ? Convert.ToDouble(depNoiseCodePenaltySchedule.Penalty_1).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedPenalty_2 = depNoiseCodePenaltySchedule.Penalty_2 != null ? Convert.ToDouble(depNoiseCodePenaltySchedule.Penalty_2).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedPenalty_3 = depNoiseCodePenaltySchedule.Penalty_3 != null ? Convert.ToDouble(depNoiseCodePenaltySchedule.Penalty_3).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedPenalty_4 = depNoiseCodePenaltySchedule.Penalty_4 != null ? Convert.ToDouble(depNoiseCodePenaltySchedule.Penalty_4).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedDefaultPenalty_1 = depNoiseCodePenaltySchedule.DefaultPenalty_1 != null ? Convert.ToDouble(depNoiseCodePenaltySchedule.DefaultPenalty_1).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedDefaultPenalty_2 = depNoiseCodePenaltySchedule.DefaultPenalty_1 != null ? Convert.ToDouble(depNoiseCodePenaltySchedule.DefaultPenalty_2).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedDefaultPenalty_3 = depNoiseCodePenaltySchedule.DefaultPenalty_1 != null ? Convert.ToDouble(depNoiseCodePenaltySchedule.DefaultPenalty_3).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedDefaultPenalty_4 = depNoiseCodePenaltySchedule.DefaultPenalty_1 != null ? Convert.ToDouble(depNoiseCodePenaltySchedule.DefaultPenalty_4).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                CreatedBy = depNoiseCodePenaltySchedule.CreatedBy,
                LastModifiedBy = depNoiseCodePenaltySchedule.LastModifiedBy != null ? depNoiseCodePenaltySchedule.LastModifiedBy : depNoiseCodePenaltySchedule.CreatedBy,
                CreatedByEmployeeName = depNoiseCodePenaltySchedule.CreatedByEmployee != null ? depNoiseCodePenaltySchedule.CreatedByEmployee.FirstName + " " + depNoiseCodePenaltySchedule.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = depNoiseCodePenaltySchedule.LastModifiedByEmployee != null ? depNoiseCodePenaltySchedule.LastModifiedByEmployee.FirstName + " " + depNoiseCodePenaltySchedule.LastModifiedByEmployee.LastName : (depNoiseCodePenaltySchedule.CreatedByEmployee != null ? depNoiseCodePenaltySchedule.CreatedByEmployee.FirstName + " " + depNoiseCodePenaltySchedule.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = depNoiseCodePenaltySchedule.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(depNoiseCodePenaltySchedule.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : depNoiseCodePenaltySchedule.CreatedDate,
                LastModifiedDate = depNoiseCodePenaltySchedule.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(depNoiseCodePenaltySchedule.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (depNoiseCodePenaltySchedule.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(depNoiseCodePenaltySchedule.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : depNoiseCodePenaltySchedule.CreatedDate),
            };
        }

    }
}