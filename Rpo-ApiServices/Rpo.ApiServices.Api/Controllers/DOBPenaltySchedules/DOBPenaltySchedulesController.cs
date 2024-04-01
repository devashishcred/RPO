// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-15-2018
// ***********************************************************************
// <copyright file="DOBPenaltySchedulesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Address Types Controller.</summary>
// ***********************************************************************

/// <summary>
/// The DOBPenaltySchedules namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.DOBPenaltySchedules
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
    public class DOBPenaltySchedulesController : ApiController
    {
        /// <summary>
        /// The database.
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the DOB Penalty Schedule.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>DOB Penalty Schedule List.</returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetDOBPenaltySchedules([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var dobPenaltySchedules = this.rpoContext.DOBPenaltySchedules.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = dobPenaltySchedules.Count();
                var recordsFiltered = recordsTotal;

                var result = dobPenaltySchedules
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
        [ResponseType(typeof(DOBPenaltyScheduleDetail))]
        public IHttpActionResult GetDOBPenaltySchedule(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                DOBPenaltySchedule dobPenaltySchedule = this.rpoContext.DOBPenaltySchedules.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (dobPenaltySchedule == null)
                {
                    return this.NotFound();
                }

                return this.Ok(this.FormatDetails(dobPenaltySchedule));
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
        /// <param name="dobPenaltySchedule">Type of the address.</param>
        /// <returns>update the detail in database.</returns>
        /// <exception cref="RpoBusinessException">
        /// Business Exception.
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDOBPenaltySchedule(int id, DOBPenaltySchedule dobPenaltySchedule)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (id != dobPenaltySchedule.Id)
                {
                    return this.BadRequest();
                }

                if (this.DOBPenaltyScheduleInfractionCodeExists(dobPenaltySchedule.InfractionCode, dobPenaltySchedule.Id))
                {
                    throw new RpoBusinessException(StaticMessages.DOBPenaltyScheduleInfractionCodeExistsMessage);
                }

                dobPenaltySchedule.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    dobPenaltySchedule.LastModifiedBy = employee.Id;
                }

                this.rpoContext.Entry(dobPenaltySchedule).State = EntityState.Modified;

                try
                {
                    this.rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.DOBPenaltyScheduleExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                DOBPenaltySchedule dobPenaltyScheduleResponse = this.rpoContext.DOBPenaltySchedules.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return this.Ok(this.FormatDetails(dobPenaltyScheduleResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the type of the address.
        /// </summary>
        /// <param name="dobPenaltySchedule">Type of the address.</param>
        /// <returns>create a new DOB Penalty Schedule.</returns>
        /// <exception cref="RpoBusinessException">
        /// Business Exception.
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DOBPenaltySchedule))]
        public IHttpActionResult PostDOBPenaltySchedule(DOBPenaltySchedule dobPenaltySchedule)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (this.DOBPenaltyScheduleInfractionCodeExists(dobPenaltySchedule.InfractionCode, dobPenaltySchedule.Id))
                {
                    throw new RpoBusinessException(StaticMessages.DOBPenaltyScheduleInfractionCodeExistsMessage);
                }

                dobPenaltySchedule.LastModifiedDate = DateTime.UtcNow;
                dobPenaltySchedule.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    dobPenaltySchedule.CreatedBy = employee.Id;
                }

                this.rpoContext.DOBPenaltySchedules.Add(dobPenaltySchedule);
                this.rpoContext.SaveChanges();

                DOBPenaltySchedule dobPenaltyScheduleResponse = this.rpoContext.DOBPenaltySchedules.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == dobPenaltySchedule.Id);
                return this.Ok(this.FormatDetails(dobPenaltyScheduleResponse));
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
        /// <returns>delete DOB Penalty Schedule.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DOBPenaltySchedule))]
        public IHttpActionResult DeleteDOBPenaltySchedule(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                DOBPenaltySchedule dobPenaltySchedule = this.rpoContext.DOBPenaltySchedules.Find(id);
                if (dobPenaltySchedule == null)
                {
                    return this.NotFound();
                }

                this.rpoContext.DOBPenaltySchedules.Remove(dobPenaltySchedule);
                this.rpoContext.SaveChanges();

                return this.Ok(dobPenaltySchedule);
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
        private bool DOBPenaltyScheduleExists(int id)
        {
            return this.rpoContext.DOBPenaltySchedules.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified address type.
        /// </summary>
        /// <param name="dobPenaltySchedule">Type of the address.</param>
        /// <returns>Address Type DTO.</returns>
        private DOBPenaltyScheduleDTO Format(DOBPenaltySchedule dobPenaltySchedule)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new DOBPenaltyScheduleDTO
            {
                Id = dobPenaltySchedule.Id,
                SectionOfLaw = dobPenaltySchedule.SectionOfLaw,
                StandardPenalty = dobPenaltySchedule.StandardPenalty,
                Stipulation = dobPenaltySchedule.Stipulation,
                ViolationDescription = dobPenaltySchedule.ViolationDescription,
                MitigatedPenalty = dobPenaltySchedule.MitigatedPenalty,
                InfractionCode = dobPenaltySchedule.InfractionCode,
                DefaultPenalty = dobPenaltySchedule.DefaultPenalty,
                Cure = dobPenaltySchedule.Cure,
                Classification = dobPenaltySchedule.Classification,
                AggravatedPenalty_II = dobPenaltySchedule.AggravatedPenalty_II,
                AggravatedPenalty_I = dobPenaltySchedule.AggravatedPenalty_I,
                AggravatedDefaultPenalty_I = dobPenaltySchedule.AggravatedDefaultPenalty_I,
                AggravatedDefaultMaxPenalty_II = dobPenaltySchedule.AggravatedDefaultMaxPenalty_II,
                FormattedStandardPenalty = dobPenaltySchedule.StandardPenalty != null ? Convert.ToDouble(dobPenaltySchedule.StandardPenalty).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedDefaultPenalty = dobPenaltySchedule.DefaultPenalty != null ? Convert.ToDouble(dobPenaltySchedule.DefaultPenalty).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedAggravatedPenalty_II = dobPenaltySchedule.AggravatedPenalty_II != null ? Convert.ToDouble(dobPenaltySchedule.AggravatedPenalty_II).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedAggravatedPenalty_I = dobPenaltySchedule.AggravatedPenalty_I != null ? Convert.ToDouble(dobPenaltySchedule.AggravatedPenalty_I).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedAggravatedDefaultPenalty_I = dobPenaltySchedule.AggravatedDefaultPenalty_I != null ? Convert.ToDouble(dobPenaltySchedule.AggravatedDefaultPenalty_I).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedAggravatedDefaultMaxPenalty_II = dobPenaltySchedule.AggravatedDefaultMaxPenalty_II != null ? Convert.ToDouble(dobPenaltySchedule.AggravatedDefaultMaxPenalty_II).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                CreatedBy = dobPenaltySchedule.CreatedBy,
                LastModifiedBy = dobPenaltySchedule.LastModifiedBy != null ? dobPenaltySchedule.LastModifiedBy : dobPenaltySchedule.CreatedBy,
                CreatedByEmployeeName = dobPenaltySchedule.CreatedByEmployee != null ? dobPenaltySchedule.CreatedByEmployee.FirstName + " " + dobPenaltySchedule.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = dobPenaltySchedule.LastModifiedByEmployee != null ? dobPenaltySchedule.LastModifiedByEmployee.FirstName + " " + dobPenaltySchedule.LastModifiedByEmployee.LastName : (dobPenaltySchedule.CreatedByEmployee != null ? dobPenaltySchedule.CreatedByEmployee.FirstName + " " + dobPenaltySchedule.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = dobPenaltySchedule.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(dobPenaltySchedule.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : dobPenaltySchedule.CreatedDate,
                LastModifiedDate = dobPenaltySchedule.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(dobPenaltySchedule.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (dobPenaltySchedule.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(dobPenaltySchedule.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : dobPenaltySchedule.CreatedDate),
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="dobPenaltySchedule">Type of the address.</param>
        /// <returns>Address Type Detail.</returns>
        private DOBPenaltyScheduleDetail FormatDetails(DOBPenaltySchedule dobPenaltySchedule)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new DOBPenaltyScheduleDetail
            {
                Id = dobPenaltySchedule.Id,
                SectionOfLaw = dobPenaltySchedule.SectionOfLaw,
                StandardPenalty = dobPenaltySchedule.StandardPenalty,
                Stipulation = dobPenaltySchedule.Stipulation,
                ViolationDescription = dobPenaltySchedule.ViolationDescription,
                MitigatedPenalty = dobPenaltySchedule.MitigatedPenalty,
                InfractionCode = dobPenaltySchedule.InfractionCode,
                DefaultPenalty = dobPenaltySchedule.DefaultPenalty,
                Cure = dobPenaltySchedule.Cure,
                Classification = dobPenaltySchedule.Classification,
                AggravatedPenalty_II = dobPenaltySchedule.AggravatedPenalty_II,
                AggravatedPenalty_I = dobPenaltySchedule.AggravatedPenalty_I,
                AggravatedDefaultPenalty_I = dobPenaltySchedule.AggravatedDefaultPenalty_I,
                AggravatedDefaultMaxPenalty_II = dobPenaltySchedule.AggravatedDefaultMaxPenalty_II,
                FormattedStandardPenalty = dobPenaltySchedule.StandardPenalty != null ? Convert.ToDouble(dobPenaltySchedule.StandardPenalty).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedDefaultPenalty = dobPenaltySchedule.DefaultPenalty != null ? Convert.ToDouble(dobPenaltySchedule.DefaultPenalty).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedAggravatedPenalty_II = dobPenaltySchedule.AggravatedPenalty_II != null ? Convert.ToDouble(dobPenaltySchedule.AggravatedPenalty_II).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedAggravatedPenalty_I = dobPenaltySchedule.AggravatedPenalty_I != null ? Convert.ToDouble(dobPenaltySchedule.AggravatedPenalty_I).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedAggravatedDefaultPenalty_I = dobPenaltySchedule.AggravatedDefaultPenalty_I != null ? Convert.ToDouble(dobPenaltySchedule.AggravatedDefaultPenalty_I).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                FormattedAggravatedDefaultMaxPenalty_II = dobPenaltySchedule.AggravatedDefaultMaxPenalty_II != null ? Convert.ToDouble(dobPenaltySchedule.AggravatedDefaultMaxPenalty_II).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                CreatedBy = dobPenaltySchedule.CreatedBy,
                LastModifiedBy = dobPenaltySchedule.LastModifiedBy != null ? dobPenaltySchedule.LastModifiedBy : dobPenaltySchedule.CreatedBy,
                CreatedByEmployeeName = dobPenaltySchedule.CreatedByEmployee != null ? dobPenaltySchedule.CreatedByEmployee.FirstName + " " + dobPenaltySchedule.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = dobPenaltySchedule.LastModifiedByEmployee != null ? dobPenaltySchedule.LastModifiedByEmployee.FirstName + " " + dobPenaltySchedule.LastModifiedByEmployee.LastName : (dobPenaltySchedule.CreatedByEmployee != null ? dobPenaltySchedule.CreatedByEmployee.FirstName + " " + dobPenaltySchedule.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = dobPenaltySchedule.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(dobPenaltySchedule.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : dobPenaltySchedule.CreatedDate,
                LastModifiedDate = dobPenaltySchedule.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(dobPenaltySchedule.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (dobPenaltySchedule.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(dobPenaltySchedule.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : dobPenaltySchedule.CreatedDate),
            };
        }

        /// <summary>
        /// Addresses the type name exists.
        /// </summary>
        /// <param name="infractionCode">The name address type.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>True</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool DOBPenaltyScheduleInfractionCodeExists(string infractionCode, int id)
        {
            return this.rpoContext.DOBPenaltySchedules.Count(e => e.InfractionCode == infractionCode && e.Id != id) > 0;
        }
    }
}