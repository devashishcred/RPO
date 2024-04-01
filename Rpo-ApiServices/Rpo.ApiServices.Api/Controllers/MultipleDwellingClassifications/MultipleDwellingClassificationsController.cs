// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="MultipleDwellingClassificationsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Multiple Dwelling Classifications Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.MultipleDwellingClassifications
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
    /// Class Multiple Dwelling Classifications Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class MultipleDwellingClassificationsController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the multiple dwelling classifications.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the multiple dwelling classifications List.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetMultipleDwellingClassifications([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var multipleDwellingClassifications = db.MultipleDwellingClassifications.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = multipleDwellingClassifications.Count();
                var recordsFiltered = recordsTotal;

                var result = multipleDwellingClassifications
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
        /// Gets the multiple dwelling classification dropdown.
        /// </summary>
        /// <returns>Gets the multiple dwelling classifications for dropdown.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/multipledwellingclassifications/dropdown")]
        public IHttpActionResult GetMultipleDwellingClassificationDropdown()
        {
            var result = db.MultipleDwellingClassifications.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Code != null && c.Code != "" ? c.Code + " " + c.Description : string.Empty,
                Description = c.Description,
                Code=c.Code
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the multiple dwelling classification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns> Gets the multiple dwelling classification in detail.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(MultipleDwellingClassificationDetail))]
        public IHttpActionResult GetMultipleDwellingClassification(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                MultipleDwellingClassification multipleDwellingClassification = db.MultipleDwellingClassifications.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                if (multipleDwellingClassification == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(multipleDwellingClassification));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the multiple dwelling classification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="multipleDwellingClassification">The multiple dwelling classification.</param>
        /// <returns>update the multiple dwelling classification.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMultipleDwellingClassification(int id, MultipleDwellingClassification multipleDwellingClassification)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != multipleDwellingClassification.Id)
                {
                    return BadRequest();
                }

                if (MultipleDwellingClassificationDescriptionExists(multipleDwellingClassification.Description, multipleDwellingClassification.Id))
                {
                    throw new RpoBusinessException(StaticMessages.MultipleDwellingClassificationDescriptionExistsMessage);
                }
                multipleDwellingClassification.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    multipleDwellingClassification.LastModifiedBy = employee.Id;
                }

                db.Entry(multipleDwellingClassification).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MultipleDwellingClassificationExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                MultipleDwellingClassification multipleDwellingClassificationResponse = db.MultipleDwellingClassifications.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return Ok(FormatDetails(multipleDwellingClassificationResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the multiple dwelling classification.
        /// </summary>
        /// <param name="multipleDwellingClassification">The multiple dwelling classification.</param>
        /// <returns>create a multiple dwelling classification.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(MultipleDwellingClassification))]
        public IHttpActionResult PostMultipleDwellingClassification(MultipleDwellingClassification multipleDwellingClassification)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (MultipleDwellingClassificationDescriptionExists(multipleDwellingClassification.Description, multipleDwellingClassification.Id))
                {
                    throw new RpoBusinessException(StaticMessages.MultipleDwellingClassificationDescriptionExistsMessage);
                }
                multipleDwellingClassification.LastModifiedDate = DateTime.UtcNow;
                multipleDwellingClassification.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    multipleDwellingClassification.CreatedBy = employee.Id;
                }

                db.MultipleDwellingClassifications.Add(multipleDwellingClassification);
                db.SaveChanges();

                MultipleDwellingClassification multipleDwellingClassificationResponse = db.MultipleDwellingClassifications.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == multipleDwellingClassification.Id);
                return Ok(FormatDetails(multipleDwellingClassificationResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the multiple dwelling classification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Delete the multiple dwelling classification.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(MultipleDwellingClassification))]
        public IHttpActionResult DeleteMultipleDwellingClassification(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                MultipleDwellingClassification multipleDwellingClassification = db.MultipleDwellingClassifications.Find(id);
                if (multipleDwellingClassification == null)
                {
                    return this.NotFound();
                }

                db.MultipleDwellingClassifications.Remove(multipleDwellingClassification);
                db.SaveChanges();

                return Ok(multipleDwellingClassification);
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
        /// Multiples the dwelling classification exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool MultipleDwellingClassificationExists(int id)
        {
            return db.MultipleDwellingClassifications.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified multiple dwelling classification.
        /// </summary>
        /// <param name="multipleDwellingClassification">The multiple dwelling classification.</param>
        /// <returns>MultipleDwellingClassificationDTO.</returns>
        private MultipleDwellingClassificationDTO Format(MultipleDwellingClassification multipleDwellingClassification)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new MultipleDwellingClassificationDTO
            {
                Id = multipleDwellingClassification.Id,
                Description = multipleDwellingClassification.Description,
                Code = multipleDwellingClassification.Code,
                CreatedBy = multipleDwellingClassification.CreatedBy,
                LastModifiedBy = multipleDwellingClassification.LastModifiedBy,
                CreatedByEmployeeName = multipleDwellingClassification.CreatedByEmployee != null ? multipleDwellingClassification.CreatedByEmployee.FirstName + " " + multipleDwellingClassification.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = multipleDwellingClassification.LastModifiedByEmployee != null ? multipleDwellingClassification.LastModifiedByEmployee.FirstName + " " + multipleDwellingClassification.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = multipleDwellingClassification.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(multipleDwellingClassification.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : multipleDwellingClassification.CreatedDate,
                LastModifiedDate = multipleDwellingClassification.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(multipleDwellingClassification.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : multipleDwellingClassification.LastModifiedDate,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="multipleDwellingClassification">The multiple dwelling classification.</param>
        /// <returns>MultipleDwellingClassificationDetail.</returns>
        private MultipleDwellingClassificationDetail FormatDetails(MultipleDwellingClassification multipleDwellingClassification)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new MultipleDwellingClassificationDetail
            {
                Id = multipleDwellingClassification.Id,
                Description = multipleDwellingClassification.Description,
                Code = multipleDwellingClassification.Code,
                CreatedBy = multipleDwellingClassification.CreatedBy,
                LastModifiedBy = multipleDwellingClassification.LastModifiedBy,
                CreatedByEmployeeName = multipleDwellingClassification.CreatedByEmployee != null ? multipleDwellingClassification.CreatedByEmployee.FirstName + " " + multipleDwellingClassification.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = multipleDwellingClassification.LastModifiedByEmployee != null ? multipleDwellingClassification.LastModifiedByEmployee.FirstName + " " + multipleDwellingClassification.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = multipleDwellingClassification.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(multipleDwellingClassification.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : multipleDwellingClassification.CreatedDate,
                LastModifiedDate = multipleDwellingClassification.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(multipleDwellingClassification.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : multipleDwellingClassification.LastModifiedDate,
            };
        }

        /// <summary>
        /// Multiples the dwelling classification description exists.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool MultipleDwellingClassificationDescriptionExists(string description, int id)
        {
            return db.MultipleDwellingClassifications.Count(e => e.Description == description && e.Id != id) > 0;
        }
    }
}