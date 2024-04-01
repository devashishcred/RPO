// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="StructureOccupancyCategoriesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Structure Occupancy Categories Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.StructureOccupancyCategories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Structure Occupancy Categories Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class StructureOccupancyCategoriesController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the structure occupancy categories.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns> Gets the structure occupancy List.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetStructureOccupancyCategories([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                 || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                 || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var structureOccupancyCategories = db.StructureOccupancyCategories.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = structureOccupancyCategories.Count();
                var recordsFiltered = recordsTotal;

                var result = structureOccupancyCategories
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
        /// Gets the structure occupancy categories dropdown.
        /// </summary>
        /// <returns> Gets the structure occupancy categories for dropdown.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/structureoccupancycategories/dropdown")]
        public IHttpActionResult GetStructureOccupancyCategoriesDropdown()
        {
            var result = db.StructureOccupancyCategories.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Code != null && c.Code != "" ? c.Code + " " + c.Description : string.Empty,
                Description = c.Description,
                Code = c.Code
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the structure occupancy category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns> Gets the structure occupancy categories in detail.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(StructureOccupancyCategoryDetail))]
        public IHttpActionResult GetStructureOccupancyCategory(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                StructureOccupancyCategory structureOccupancyCategory = db.StructureOccupancyCategories.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (structureOccupancyCategory == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(structureOccupancyCategory));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the structure occupancy category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="structureOccupancyCategory">The structure occupancy category.</param>
        /// <returns>Update the structure occupancy categories.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(StructureOccupancyCategoryDetail))]
        public IHttpActionResult PutStructureOccupancyCategory(int id, StructureOccupancyCategory structureOccupancyCategory)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != structureOccupancyCategory.Id)
                {
                    return BadRequest();
                }

                if (StructureOccupancyCategoryNameExists(structureOccupancyCategory.Description, structureOccupancyCategory.Id))
                {
                    throw new RpoBusinessException(StaticMessages.StructureOccupancyCategoryNameExistsMessage);
                }

                structureOccupancyCategory.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    structureOccupancyCategory.LastModifiedBy = employee.Id;
                }

                db.Entry(structureOccupancyCategory).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StructureOccupancyCategoryExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                StructureOccupancyCategory structureOccupancyCategoryResponse = db.StructureOccupancyCategories.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return Ok(FormatDetails(structureOccupancyCategoryResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the structure occupancy category.
        /// </summary>
        /// <param name="structureOccupancyCategory">The structure occupancy category.</param>
        /// <returns>Create a new  the structure occupancy categories.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(StructureOccupancyCategoryDetail))]
        public IHttpActionResult PostStructureOccupancyCategory(StructureOccupancyCategory structureOccupancyCategory)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (StructureOccupancyCategoryNameExists(structureOccupancyCategory.Description, structureOccupancyCategory.Id))
                {
                    throw new RpoBusinessException(StaticMessages.StructureOccupancyCategoryNameExistsMessage);
                }
                structureOccupancyCategory.LastModifiedDate = DateTime.UtcNow;
                structureOccupancyCategory.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    structureOccupancyCategory.CreatedBy = employee.Id;
                }

                db.StructureOccupancyCategories.Add(structureOccupancyCategory);
                db.SaveChanges();

                StructureOccupancyCategory structureOccupancyCategoryResponse = db.StructureOccupancyCategories.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == structureOccupancyCategory.Id);
                return Ok(FormatDetails(structureOccupancyCategoryResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the structure occupancy category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Deletes the structure occupancy category.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(StructureOccupancyCategory))]
        public IHttpActionResult DeleteStructureOccupancyCategory(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                StructureOccupancyCategory structureOccupancyCategory = db.StructureOccupancyCategories.Find(id);
                if (structureOccupancyCategory == null)
                {
                    return this.NotFound();
                }

                db.StructureOccupancyCategories.Remove(structureOccupancyCategory);
                db.SaveChanges();

                return Ok(structureOccupancyCategory);
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
        /// Structures the occupancy category exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool StructureOccupancyCategoryExists(int id)
        {
            return db.StructureOccupancyCategories.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified structure occupancy category.
        /// </summary>
        /// <param name="structureOccupancyCategory">The structure occupancy category.</param>
        /// <returns>StructureOccupancyCategoryDTO.</returns>
        private StructureOccupancyCategoryDTO Format(StructureOccupancyCategory structureOccupancyCategory)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new StructureOccupancyCategoryDTO
            {
                Id = structureOccupancyCategory.Id,
                Description = structureOccupancyCategory.Description,
                Code = structureOccupancyCategory.Code,
                CreatedBy = structureOccupancyCategory.CreatedBy,
                LastModifiedBy = structureOccupancyCategory.LastModifiedBy,
                CreatedByEmployeeName = structureOccupancyCategory.CreatedByEmployee != null ? structureOccupancyCategory.CreatedByEmployee.FirstName + " " + structureOccupancyCategory.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = structureOccupancyCategory.LastModifiedByEmployee != null ? structureOccupancyCategory.LastModifiedByEmployee.FirstName + " " + structureOccupancyCategory.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = structureOccupancyCategory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(structureOccupancyCategory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : structureOccupancyCategory.CreatedDate,
                LastModifiedDate = structureOccupancyCategory.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(structureOccupancyCategory.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : structureOccupancyCategory.LastModifiedDate,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="structureOccupancyCategory">The structure occupancy category.</param>
        /// <returns>StructureOccupancyCategoryDetail.</returns>
        private StructureOccupancyCategoryDetail FormatDetails(StructureOccupancyCategory structureOccupancyCategory)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new StructureOccupancyCategoryDetail
            {
                Id = structureOccupancyCategory.Id,
                Description = structureOccupancyCategory.Description,
                Code = structureOccupancyCategory.Code,
                CreatedBy = structureOccupancyCategory.CreatedBy,
                LastModifiedBy = structureOccupancyCategory.LastModifiedBy,
                CreatedByEmployeeName = structureOccupancyCategory.CreatedByEmployee != null ? structureOccupancyCategory.CreatedByEmployee.FirstName + " " + structureOccupancyCategory.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = structureOccupancyCategory.LastModifiedByEmployee != null ? structureOccupancyCategory.LastModifiedByEmployee.FirstName + " " + structureOccupancyCategory.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = structureOccupancyCategory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(structureOccupancyCategory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : structureOccupancyCategory.CreatedDate,
                LastModifiedDate = structureOccupancyCategory.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(structureOccupancyCategory.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : structureOccupancyCategory.LastModifiedDate,
            };
        }

        /// <summary>
        /// Structures the occupancy category name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool StructureOccupancyCategoryNameExists(string name, int id)
        {
            return db.StructureOccupancyCategories.Count(e => e.Description == name && e.Id != id) > 0;
        }
    }
}