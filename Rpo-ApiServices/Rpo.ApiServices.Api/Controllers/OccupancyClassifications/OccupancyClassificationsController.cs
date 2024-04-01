// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="OccupancyClassificationsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Occupancy Classifications Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.OccupancyClassifications
{
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Occupancy Classifications Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class OccupancyClassificationsController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the occupancy classifications.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the occupancy classifications List.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetOccupancyClassifications([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var occupancyClassifications = db.OccupancyClassifications.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = occupancyClassifications.Count();
                var recordsFiltered = recordsTotal;

                var result = occupancyClassifications
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
        /// Gets the occupancy classification dropdown.
        /// </summary>
        /// <returns>Gets the occupancy classifications for dropdown.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/occupancyclassifications/dropdown")]
        public IHttpActionResult GetOccupancyClassificationDropdown()
        {
            var result = db.OccupancyClassifications.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Code != null && c.Code != "" ? c.Code + " " + c.Description : string.Empty,
                Description = c.Description,
                Code = c.Code,
                Is_2008_2014 = c.Is_2008_2014
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the occupancy classification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the occupancy classifications in detail.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(OccupancyClassificationDetail))]
        public IHttpActionResult GetOccupancyClassification(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                OccupancyClassification occupancyClassification = db.OccupancyClassifications.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (occupancyClassification == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(occupancyClassification));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the occupancy classification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="occupancyClassification">The occupancy classification.</param>
        /// <returns>update the detail of occupancy classification.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(OccupancyClassificationDetail))]
        public IHttpActionResult PutOccupancyClassification(int id, OccupancyClassification occupancyClassification)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != occupancyClassification.Id)
                {
                    return BadRequest();
                }

                if (OccupancyClassificationNameExists(occupancyClassification.Description, occupancyClassification.Id))
                {
                    throw new RpoBusinessException(StaticMessages.OccupancyClassificationNameExistsMessage);
                }

                occupancyClassification.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    occupancyClassification.LastModifiedBy = employee.Id;
                }

                db.Entry(occupancyClassification).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OccupancyClassificationExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                OccupancyClassification occupancyClassificationResponse = db.OccupancyClassifications.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return Ok(FormatDetails(occupancyClassificationResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the occupancy classification.
        /// </summary>
        /// <param name="occupancyClassification">The occupancy classification.</param>
        /// <returns>create a new detail of occupancy classification.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(OccupancyClassificationDetail))]
        public IHttpActionResult PostOccupancyClassification(OccupancyClassification occupancyClassification)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (OccupancyClassificationNameExists(occupancyClassification.Description, occupancyClassification.Id))
                {
                    throw new RpoBusinessException(StaticMessages.OccupancyClassificationNameExistsMessage);
                }
                occupancyClassification.LastModifiedDate = DateTime.UtcNow;
                occupancyClassification.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    occupancyClassification.CreatedBy = employee.Id;
                }

                db.OccupancyClassifications.Add(occupancyClassification);
                db.SaveChanges();

                OccupancyClassification occupancyClassificationResponse = db.OccupancyClassifications.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == occupancyClassification.Id);
                return Ok(FormatDetails(occupancyClassificationResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the occupancy classification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>delete the detail of occupancy classification.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(OccupancyClassification))]
        public IHttpActionResult DeleteOccupancyClassification(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                OccupancyClassification occupancyClassification = db.OccupancyClassifications.Find(id);
                if (occupancyClassification == null)
                {
                    return this.NotFound();
                }

                db.OccupancyClassifications.Remove(occupancyClassification);
                db.SaveChanges();

                return Ok(occupancyClassification);
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
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Occupancies the classification exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool OccupancyClassificationExists(int id)
        {
            return db.OccupancyClassifications.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified occupancy classification.
        /// </summary>
        /// <param name="occupancyClassification">The occupancy classification.</param>
        /// <returns>OccupancyClassificationDTO.</returns>
        private OccupancyClassificationDTO Format(OccupancyClassification occupancyClassification)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new OccupancyClassificationDTO
            {
                Id = occupancyClassification.Id,
                Description = occupancyClassification.Description,
                Is_2008_2014 = occupancyClassification.Is_2008_2014,
                Code = occupancyClassification.Code,
                CreatedBy = occupancyClassification.CreatedBy,
                LastModifiedBy = occupancyClassification.LastModifiedBy,
                CreatedByEmployeeName = occupancyClassification.CreatedByEmployee != null ? occupancyClassification.CreatedByEmployee.FirstName + " " + occupancyClassification.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = occupancyClassification.LastModifiedByEmployee != null ? occupancyClassification.LastModifiedByEmployee.FirstName + " " + occupancyClassification.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = occupancyClassification.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(occupancyClassification.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : occupancyClassification.CreatedDate,
                LastModifiedDate = occupancyClassification.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(occupancyClassification.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : occupancyClassification.LastModifiedDate,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="occupancyClassification">The occupancy classification.</param>
        /// <returns>OccupancyClassificationDetail.</returns>
        private OccupancyClassificationDetail FormatDetails(OccupancyClassification occupancyClassification)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new OccupancyClassificationDetail
            {
                Id = occupancyClassification.Id,
                Description = occupancyClassification.Description,
                Is_2008_2014 = occupancyClassification.Is_2008_2014,
                Code = occupancyClassification.Code,
                CreatedBy = occupancyClassification.CreatedBy,
                LastModifiedBy = occupancyClassification.LastModifiedBy,
                CreatedByEmployeeName = occupancyClassification.CreatedByEmployee != null ? occupancyClassification.CreatedByEmployee.FirstName + " " + occupancyClassification.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = occupancyClassification.LastModifiedByEmployee != null ? occupancyClassification.LastModifiedByEmployee.FirstName + " " + occupancyClassification.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = occupancyClassification.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(occupancyClassification.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : occupancyClassification.CreatedDate,
                LastModifiedDate = occupancyClassification.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(occupancyClassification.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : occupancyClassification.LastModifiedDate,
            };
        }

        /// <summary>
        /// Occupancies the classification name exists.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool OccupancyClassificationNameExists(string description, int id)
        {
            return db.OccupancyClassifications.Count(e => e.Description == description && e.Id != id) > 0;
        }
    }
}