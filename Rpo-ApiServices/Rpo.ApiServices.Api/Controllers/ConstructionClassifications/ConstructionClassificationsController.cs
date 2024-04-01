// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-20-2018
// ***********************************************************************
// <copyright file="ConstructionClassificationsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Construction Classifications Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Construction Classifications namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.ConstructionClassifications
{
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Api.Tools;
    using Filters;

    /// <summary>
    /// Class Construction Classifications Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class ConstructionClassificationsController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the construction classifications.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttpActionResult. Get ConstructionClassifications List</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetConstructionClassifications([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var constructionClassifications = db.ConstructionClassifications.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = constructionClassifications.Count();
                var recordsFiltered = recordsTotal;

                var result = constructionClassifications
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
        /// Gets the construction classification dropdown.
        /// </summary>
        /// <returns>IHttpActionResult. Get ConstructionClassifications List</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/constructionclassifications/dropdown")]
        public IHttpActionResult GetConstructionClassificationDropdown()
        {
            var result = db.ConstructionClassifications.AsEnumerable().Select(c => new
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
        /// Gets the construction classification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult. Get ConstructionClassifications detail</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ConstructionClassificationDetail))]
        public IHttpActionResult GetConstructionClassification(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                ConstructionClassification constructionClassification = db.ConstructionClassifications.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (constructionClassification == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(constructionClassification));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the construction classification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="constructionClassification">The construction classification.</param>
        /// <returns>IHttpActionResult. update the ConstructionClassifications</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ConstructionClassificationDetail))]
        public IHttpActionResult PutConstructionClassification(int id, ConstructionClassification constructionClassification)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != constructionClassification.Id)
                {
                    return BadRequest();
                }

                if (ConstructionClassificationNameExists(constructionClassification.Description, constructionClassification.Id))
                {
                    throw new RpoBusinessException(StaticMessages.ConstructionClassificationNameExistsMessage);
                }
                constructionClassification.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    constructionClassification.LastModifiedBy = employee.Id;
                }

                db.Entry(constructionClassification).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConstructionClassificationExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                ConstructionClassification constructionClassificationResponse = db.ConstructionClassifications.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return Ok(FormatDetails(constructionClassificationResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the construction classification.
        /// </summary>
        /// <param name="constructionClassification">The construction classification.</param>
        /// <returns>IHttpActionResult. Add new ConstructionClassifications</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ConstructionClassificationDetail))]
        public IHttpActionResult PostConstructionClassification(ConstructionClassification constructionClassification)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (ConstructionClassificationNameExists(constructionClassification.Description, constructionClassification.Id))
                {
                    throw new RpoBusinessException(StaticMessages.ConstructionClassificationNameExistsMessage);
                }
                constructionClassification.LastModifiedDate = DateTime.UtcNow;
                constructionClassification.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    constructionClassification.CreatedBy = employee.Id;
                }

                db.ConstructionClassifications.Add(constructionClassification);
                db.SaveChanges();

                ConstructionClassification constructionClassificationResponse = db.ConstructionClassifications.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == constructionClassification.Id);
                return Ok(FormatDetails(constructionClassificationResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the construction classification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult. Delete ConstructionClassifications in the List</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ConstructionClassification))]
        public IHttpActionResult DeleteConstructionClassification(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                ConstructionClassification constructionClassification = db.ConstructionClassifications.Find(id);
                if (constructionClassification == null)
                {
                    return this.NotFound();
                }

                db.ConstructionClassifications.Remove(constructionClassification);
                db.SaveChanges();

                return Ok(constructionClassification);
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
        /// Constructions the classification exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ConstructionClassificationExists(int id)
        {
            return db.ConstructionClassifications.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified construction classification.
        /// </summary>
        /// <param name="constructionClassification">The construction classification.</param>
        /// <returns>ConstructionClassificationDTO.</returns>
        private ConstructionClassificationDTO Format(ConstructionClassification constructionClassification)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new ConstructionClassificationDTO
            {
                Id = constructionClassification.Id,
                Description = constructionClassification.Description,
                Code = constructionClassification.Code,
                Is_2008_2014 = constructionClassification.Is_2008_2014,
                CreatedBy = constructionClassification.CreatedBy,
                LastModifiedBy = constructionClassification.LastModifiedBy,
                CreatedByEmployeeName = constructionClassification.CreatedByEmployee != null ? constructionClassification.CreatedByEmployee.FirstName + " " + constructionClassification.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = constructionClassification.LastModifiedByEmployee != null ? constructionClassification.LastModifiedByEmployee.FirstName + " " + constructionClassification.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = constructionClassification.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(constructionClassification.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : constructionClassification.CreatedDate,
                LastModifiedDate = constructionClassification.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(constructionClassification.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : constructionClassification.LastModifiedDate,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="constructionClassification">The construction classification.</param>
        /// <returns>ConstructionClassificationDetail.</returns>
        private ConstructionClassificationDetail FormatDetails(ConstructionClassification constructionClassification)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new ConstructionClassificationDetail
            {
                Id = constructionClassification.Id,
                Description = constructionClassification.Description,
                Code = constructionClassification.Code,
                Is_2008_2014 = constructionClassification.Is_2008_2014,
                CreatedBy = constructionClassification.CreatedBy,
                LastModifiedBy = constructionClassification.LastModifiedBy,
                CreatedByEmployeeName = constructionClassification.CreatedByEmployee != null ? constructionClassification.CreatedByEmployee.FirstName + " " + constructionClassification.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = constructionClassification.LastModifiedByEmployee != null ? constructionClassification.LastModifiedByEmployee.FirstName + " " + constructionClassification.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = constructionClassification.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(constructionClassification.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : constructionClassification.CreatedDate,
                LastModifiedDate = constructionClassification.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(constructionClassification.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : constructionClassification.LastModifiedDate,
            };
        }

        /// <summary>
        /// Constructions the classification name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ConstructionClassificationNameExists(string name, int id)
        {
            return db.ConstructionClassifications.Count(e => e.Description == name && e.Id != id) > 0;
        }
    }
}