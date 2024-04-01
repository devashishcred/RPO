// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-15-2018
// ***********************************************************************
// <copyright file="DOTPenaltySchedulesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Address Types Controller.</summary>
// ***********************************************************************

/// <summary>
/// The DOTPenaltySchedules namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.DOTPenaltySchedules
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
    public class DOTPenaltySchedulesController : ApiController
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
        public IHttpActionResult GetDOTPenaltySchedules([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var dotPenaltySchedules = this.rpoContext.DOTPenaltySchedules.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = dotPenaltySchedules.Count();
                var recordsFiltered = recordsTotal;

                var result = dotPenaltySchedules
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
        [ResponseType(typeof(DOTPenaltyScheduleDetail))]
        public IHttpActionResult GetDOTPenaltySchedule(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                DOTPenaltySchedule dotPenaltySchedule = this.rpoContext.DOTPenaltySchedules.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (dotPenaltySchedule == null)
                {
                    return this.NotFound();
                }

                return this.Ok(this.FormatDetails(dotPenaltySchedule));
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
        /// <param name="dotPenaltySchedule">Type of the address.</param>
        /// <returns>IHttp Action Result.</returns>
        /// <exception cref="RpoBusinessException">
        /// Business Exception.
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDOTPenaltySchedule(int id, DOTPenaltySchedule dotPenaltySchedule)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (id != dotPenaltySchedule.Id)
                {
                    return this.BadRequest();
                }
                
                dotPenaltySchedule.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    dotPenaltySchedule.LastModifiedBy = employee.Id;
                }

                this.rpoContext.Entry(dotPenaltySchedule).State = EntityState.Modified;

                try
                {
                    this.rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.DOTPenaltyScheduleExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                DOTPenaltySchedule dotPenaltyScheduleResponse = this.rpoContext.DOTPenaltySchedules.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return this.Ok(this.FormatDetails(dotPenaltyScheduleResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the type of the address.
        /// </summary>
        /// <param name="dotPenaltySchedule">Type of the address.</param>
        /// <returns>IHttp Action Result.</returns>
        /// <exception cref="RpoBusinessException">
        /// Business Exception.
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DOTPenaltySchedule))]
        public IHttpActionResult PostDOTPenaltySchedule(DOTPenaltySchedule dotPenaltySchedule)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }
                
                dotPenaltySchedule.LastModifiedDate = DateTime.UtcNow;
                dotPenaltySchedule.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    dotPenaltySchedule.CreatedBy = employee.Id;
                }

                this.rpoContext.DOTPenaltySchedules.Add(dotPenaltySchedule);
                this.rpoContext.SaveChanges();

                DOTPenaltySchedule dotPenaltyScheduleResponse = this.rpoContext.DOTPenaltySchedules.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == dotPenaltySchedule.Id);
                return this.Ok(this.FormatDetails(dotPenaltyScheduleResponse));
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
        [ResponseType(typeof(DOTPenaltySchedule))]
        public IHttpActionResult DeleteDOTPenaltySchedule(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                DOTPenaltySchedule dotPenaltySchedule = this.rpoContext.DOTPenaltySchedules.Find(id);
                if (dotPenaltySchedule == null)
                {
                    return this.NotFound();
                }

                this.rpoContext.DOTPenaltySchedules.Remove(dotPenaltySchedule);
                this.rpoContext.SaveChanges();

                return this.Ok(dotPenaltySchedule);
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
        private bool DOTPenaltyScheduleExists(int id)
        {
            return this.rpoContext.DOTPenaltySchedules.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified address type.
        /// </summary>
        /// <param name="dotPenaltySchedule">Type of the address.</param>
        /// <returns>Address Type DTO.</returns>
        private DOTPenaltyScheduleDTO Format(DOTPenaltySchedule dotPenaltySchedule)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new DOTPenaltyScheduleDTO
            {
                Id = dotPenaltySchedule.Id,
                Section = dotPenaltySchedule.Section,
                Description = dotPenaltySchedule.Description,
                Penalty = dotPenaltySchedule.Penalty,
                DefaultPenalty = dotPenaltySchedule.DefaultPenalty,
                FormattedPenalty = dotPenaltySchedule.Penalty != null ? Convert.ToDouble(dotPenaltySchedule.Penalty).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedDefaultPenalty = dotPenaltySchedule.DefaultPenalty != null ? Convert.ToDouble(dotPenaltySchedule.DefaultPenalty).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                CreatedBy = dotPenaltySchedule.CreatedBy,
                LastModifiedBy = dotPenaltySchedule.LastModifiedBy != null ? dotPenaltySchedule.LastModifiedBy : dotPenaltySchedule.CreatedBy,
                CreatedByEmployeeName = dotPenaltySchedule.CreatedByEmployee != null ? dotPenaltySchedule.CreatedByEmployee.FirstName + " " + dotPenaltySchedule.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = dotPenaltySchedule.LastModifiedByEmployee != null ? dotPenaltySchedule.LastModifiedByEmployee.FirstName + " " + dotPenaltySchedule.LastModifiedByEmployee.LastName : (dotPenaltySchedule.CreatedByEmployee != null ? dotPenaltySchedule.CreatedByEmployee.FirstName + " " + dotPenaltySchedule.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = dotPenaltySchedule.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(dotPenaltySchedule.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : dotPenaltySchedule.CreatedDate,
                LastModifiedDate = dotPenaltySchedule.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(dotPenaltySchedule.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (dotPenaltySchedule.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(dotPenaltySchedule.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : dotPenaltySchedule.CreatedDate),
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="dotPenaltySchedule">Type of the address.</param>
        /// <returns>Address Type Detail.</returns>
        private DOTPenaltyScheduleDetail FormatDetails(DOTPenaltySchedule dotPenaltySchedule)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new DOTPenaltyScheduleDetail
            {
                Id = dotPenaltySchedule.Id,
                Section = dotPenaltySchedule.Section,
                Description = dotPenaltySchedule.Description,
                Penalty = dotPenaltySchedule.Penalty,
                DefaultPenalty = dotPenaltySchedule.DefaultPenalty,
                FormattedPenalty = dotPenaltySchedule.Penalty != null ? Convert.ToDouble(dotPenaltySchedule.Penalty).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedDefaultPenalty = dotPenaltySchedule.DefaultPenalty != null ? Convert.ToDouble(dotPenaltySchedule.DefaultPenalty).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                CreatedBy = dotPenaltySchedule.CreatedBy,
                LastModifiedBy = dotPenaltySchedule.LastModifiedBy != null ? dotPenaltySchedule.LastModifiedBy : dotPenaltySchedule.CreatedBy,
                CreatedByEmployeeName = dotPenaltySchedule.CreatedByEmployee != null ? dotPenaltySchedule.CreatedByEmployee.FirstName + " " + dotPenaltySchedule.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = dotPenaltySchedule.LastModifiedByEmployee != null ? dotPenaltySchedule.LastModifiedByEmployee.FirstName + " " + dotPenaltySchedule.LastModifiedByEmployee.LastName : (dotPenaltySchedule.CreatedByEmployee != null ? dotPenaltySchedule.CreatedByEmployee.FirstName + " " + dotPenaltySchedule.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = dotPenaltySchedule.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(dotPenaltySchedule.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : dotPenaltySchedule.CreatedDate,
                LastModifiedDate = dotPenaltySchedule.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(dotPenaltySchedule.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (dotPenaltySchedule.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(dotPenaltySchedule.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : dotPenaltySchedule.CreatedDate),
            };
        }

    }
}