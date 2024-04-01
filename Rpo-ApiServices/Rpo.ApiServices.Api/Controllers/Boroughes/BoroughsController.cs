// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-09-2018
// ***********************************************************************
// <copyright file="BoroughsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Boroughs Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.Boroughes
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
    /// Class Boroughs Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [Authorize]
    public class BoroughsController : ApiController
    {
        /// <summary>
        /// The database.
        /// </summary>
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the boroughs.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the boroughs List.</returns>
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetBoroughes([FromUri] DataTableParameters dataTableParameters)
        {
            var boroughes = this.db.Boroughes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

            var recordsTotal = boroughes.Count();
            var recordsFiltered = recordsTotal;

            var result = boroughes
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
                Data = result
            });
        }

        /// <summary>
        /// Gets the borough dropdown.
        /// </summary>
        /// <returns>Gets the boroughs List for binddropdown.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/boroughs/dropdown")]
        public IHttpActionResult GetBoroughDropdown()
        {
            var result = this.db.Boroughes.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Description,
                Description = c.Description
            }).ToArray();

            return this.Ok(result);
        }

        /// <summary>
        /// Gets the borough.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the boroughs in detail.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(Borough))]
        public IHttpActionResult GetBorough(int id)
        {
            Borough borough = this.db.Boroughes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
            if (borough == null)
            {
                return this.NotFound();
            }

            return this.Ok(this.FormatDetails(borough));
        }

        /// <summary>
        /// Puts the borough.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="borough">The borough.</param>
        /// <returns>update the detail of broughs.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBorough(int id, Borough borough)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (id != borough.Id)
            {
                return this.BadRequest();
            }

            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

            borough.LastModifiedDate = DateTime.UtcNow;
            if (employee != null)
            {
                borough.LastModifiedBy = employee.Id;
            }

            this.db.Entry(borough).State = EntityState.Modified;

            try
            {
                this.db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.BoroughExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            Borough boroughResponse = this.db.Boroughes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
            return this.Ok(this.FormatDetails(boroughResponse));
        }

        /// <summary>
        /// Posts the borough.
        /// </summary>
        /// <param name="borough">The borough.</param>
        /// <returns>create a new bororughs.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(Borough))]
        public IHttpActionResult PostBorough(Borough borough)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            borough.LastModifiedDate = DateTime.UtcNow;
            borough.CreatedDate = DateTime.UtcNow;
            if (employee != null)
            {
                borough.CreatedBy = employee.Id;
            }

            this.db.Boroughes.Add(borough);
            this.db.SaveChanges();

            Borough boroughResponse = this.db.Boroughes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == borough.Id);
            return this.Ok(this.FormatDetails(boroughResponse));
        }

        /// <summary>
        /// Deletes the borough.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Delete the borough in database.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(Borough))]
        public IHttpActionResult DeleteBorough(int id)
        {
            Borough borough = this.db.Boroughes.Find(id);
            if (borough == null)
            {
                return this.NotFound();
            }

            this.db.Boroughes.Remove(borough);
            this.db.SaveChanges();

            return this.Ok(borough);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Boroughs the exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>True</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool BoroughExists(int id)
        {
            return this.db.Boroughes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified borough.
        /// </summary>
        /// <param name="borough">The borough.</param>
        /// <returns>Borough DTO.</returns>
        private BoroughDTO Format(Borough borough)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new BoroughDTO
            {
                Id = borough.Id,
                Description = borough.Description,
                BisCode = borough.BisCode,
                CreatedBy = borough.CreatedBy,
                LastModifiedBy = borough.LastModifiedBy,
                CreatedByEmployeeName = borough.CreatedByEmployee != null ? borough.CreatedByEmployee.FirstName + " " + borough.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = borough.LastModifiedByEmployee != null ? borough.LastModifiedByEmployee.FirstName + " " + borough.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = borough.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(borough.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : borough.CreatedDate,
                LastModifiedDate = borough.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(borough.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : borough.LastModifiedDate,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="borough">The borough.</param>
        /// <returns>Borough Detail.</returns>
        private BoroughDetail FormatDetails(Borough borough)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new BoroughDetail
            {
                Id = borough.Id,
                Description = borough.Description,
                BisCode = borough.BisCode,
                CreatedByEmployee = borough.CreatedByEmployee,
                LastModifiedByEmployee = borough.LastModifiedByEmployee,
                IdCreatedBy = borough.CreatedBy,
                IdLastModifiedBy = borough.LastModifiedBy,
                CreatedByEmployeeName = borough.CreatedByEmployee != null ? borough.CreatedByEmployee.FirstName + " " + borough.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = borough.LastModifiedByEmployee != null ? borough.LastModifiedByEmployee.FirstName + " " + borough.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = borough.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(borough.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : borough.CreatedDate,
                LastModifiedDate = borough.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(borough.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : borough.LastModifiedDate,
            };
        }
    }
}