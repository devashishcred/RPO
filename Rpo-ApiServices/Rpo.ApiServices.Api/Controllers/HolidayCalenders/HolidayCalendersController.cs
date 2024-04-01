
namespace Rpo.ApiServices.Api.Controllers.HolidayCalenders
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

    public class HolidayCalendersController : ApiController
    {
        /// <summary>
        /// The database.
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetHolidayCalenders([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var holidayCalenders = this.rpoContext.HolidayCalenders.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = holidayCalenders.Count();
                var recordsFiltered = recordsTotal;

                var result = holidayCalenders
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
        
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/HolidayCalenders/dropdown")]
        public IHttpActionResult GetHolidayCalenderDropdown()
        {
            var result = this.rpoContext.HolidayCalenders.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name,
                HolidayDate = c.HolidayDate
            }).ToArray();

            return this.Ok(result);
        }
        
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(HolidayCalenderDetail))]
        public IHttpActionResult GetHolidayCalender(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                HolidayCalender holidayCalender = this.rpoContext.HolidayCalenders.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (holidayCalender == null)
                {
                    return this.NotFound();
                }

                return this.Ok(this.FormatDetails(holidayCalender));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(HolidayCalenderDetail))]
        public IHttpActionResult PutHolidayCalender(int id, HolidayCalender holidayCalender)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (id != holidayCalender.Id)
                {
                    return this.BadRequest();
                }
                
                holidayCalender.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    holidayCalender.LastModifiedBy = employee.Id;
                }

                this.rpoContext.Entry(holidayCalender).State = EntityState.Modified;

                try
                {
                    this.rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.HolidayCalenderExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                HolidayCalender holidayCalenderResponse = this.rpoContext.HolidayCalenders.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return this.Ok(this.FormatDetails(holidayCalenderResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(HolidayCalenderDetail))]
        public IHttpActionResult PostHolidayCalender(HolidayCalender holidayCalender)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }
                
                holidayCalender.LastModifiedDate = DateTime.UtcNow;
                holidayCalender.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    holidayCalender.CreatedBy = employee.Id;
                }

                this.rpoContext.HolidayCalenders.Add(holidayCalender);
                this.rpoContext.SaveChanges();

                HolidayCalender holidayCalenderResponse = this.rpoContext.HolidayCalenders.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == holidayCalender.Id);
                return this.Ok(this.FormatDetails(holidayCalenderResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(HolidayCalender))]
        public IHttpActionResult DeleteHolidayCalender(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                HolidayCalender holidayCalender = this.rpoContext.HolidayCalenders.Find(id);
                if (holidayCalender == null)
                {
                    return this.NotFound();
                }

                this.rpoContext.HolidayCalenders.Remove(holidayCalender);
                this.rpoContext.SaveChanges();

                return this.Ok(holidayCalender);
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

        private bool HolidayCalenderExists(int id)
        {
            return this.rpoContext.HolidayCalenders.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified address type.
        /// </summary>
        /// <param name="holidayCalender">Type of the address.</param>
        /// <returns>Address Type DTO.</returns>
        private HolidayCalenderDTO Format(HolidayCalender holidayCalender)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new HolidayCalenderDTO
            {
                Id = holidayCalender.Id,
                Name = holidayCalender.Name,
                HolidayDate = holidayCalender.HolidayDate,
                CreatedBy = holidayCalender.CreatedBy,
                LastModifiedBy = holidayCalender.LastModifiedBy != null ? holidayCalender.LastModifiedBy : holidayCalender.CreatedBy,
                CreatedByEmployeeName = holidayCalender.CreatedByEmployee != null ? holidayCalender.CreatedByEmployee.FirstName + " " + holidayCalender.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = holidayCalender.LastModifiedByEmployee != null ? holidayCalender.LastModifiedByEmployee.FirstName + " " + holidayCalender.LastModifiedByEmployee.LastName : (holidayCalender.CreatedByEmployee != null ? holidayCalender.CreatedByEmployee.FirstName + " " + holidayCalender.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = holidayCalender.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(holidayCalender.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : holidayCalender.CreatedDate,
                LastModifiedDate = holidayCalender.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(holidayCalender.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (holidayCalender.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(holidayCalender.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : holidayCalender.CreatedDate),
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="holidayCalender">Type of the address.</param>
        /// <returns>Address Type Detail.</returns>
        private HolidayCalenderDetail FormatDetails(HolidayCalender holidayCalender)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new HolidayCalenderDetail
            {
                Id = holidayCalender.Id,
                Name = holidayCalender.Name,
                HolidayDate = holidayCalender.HolidayDate,
                CreatedBy = holidayCalender.CreatedBy,
                LastModifiedBy = holidayCalender.LastModifiedBy != null ? holidayCalender.LastModifiedBy : holidayCalender.CreatedBy,
                CreatedByEmployeeName = holidayCalender.CreatedByEmployee != null ? holidayCalender.CreatedByEmployee.FirstName + " " + holidayCalender.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = holidayCalender.LastModifiedByEmployee != null ? holidayCalender.LastModifiedByEmployee.FirstName + " " + holidayCalender.LastModifiedByEmployee.LastName : (holidayCalender.CreatedByEmployee != null ? holidayCalender.CreatedByEmployee.FirstName + " " + holidayCalender.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = holidayCalender.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(holidayCalender.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : holidayCalender.CreatedDate,
                LastModifiedDate = holidayCalender.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(holidayCalender.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (holidayCalender.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(holidayCalender.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : holidayCalender.CreatedDate),
            };
        }
        
    }
}