// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="SeismicDesignCategoriesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Seismic Design Categories Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.SeismicDesignCategories
{
    using System;
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
    /// Class Seismic Design Categories Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class SeismicDesignCategoriesController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the seismic design categories.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the seismic design categories List.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetSeismicDesignCategories([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var seismicDesignCategories = rpoContext.SeismicDesignCategories.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = seismicDesignCategories.Count();
                var recordsFiltered = recordsTotal;

                var result = seismicDesignCategories
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
        /// Gets the seismic design categories dropdown.
        /// </summary>
        /// <returns>Gets the seismic design categories List for dropdown.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/seismicdesigncategories/dropdown")]
        public IHttpActionResult GetSeismicDesignCategoriesDropdown()
        {
            var result = rpoContext.SeismicDesignCategories.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Code != null && c.Code != "" ? c.Code + " " + c.Description : string.Empty,
                Description = c.Description,
                Code = c.Code
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the seismic design category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the seismic design categories detail .</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(SeismicDesignCategoryDetail))]
        public IHttpActionResult GetSeismicDesignCategory(int id)
        {
            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                SeismicDesignCategory seismicDesignCategory = rpoContext.SeismicDesignCategories.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (seismicDesignCategory == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(seismicDesignCategory));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the seismic design category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="seismicDesignCategory">The seismic design category.</param>
        /// <returns>update the detail in database.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(SeismicDesignCategoryDetail))]
        public IHttpActionResult PutSeismicDesignCategory(int id, SeismicDesignCategory seismicDesignCategory)
        {
            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != seismicDesignCategory.Id)
                {
                    return BadRequest();
                }

                if (SeismicDesignCategoryNameExists(seismicDesignCategory.Description, seismicDesignCategory.Id))
                {
                    throw new RpoBusinessException(StaticMessages.SeismicDesignCategoryNameExistsMessage);
                }
                seismicDesignCategory.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    seismicDesignCategory.LastModifiedBy = employee.Id;
                }

                rpoContext.Entry(seismicDesignCategory).State = EntityState.Modified;

                try
                {
                    rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeismicDesignCategoryExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                SeismicDesignCategory seismicDesignCategoryResponse = rpoContext.SeismicDesignCategories.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return Ok(FormatDetails(seismicDesignCategoryResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the seismic design category.
        /// </summary>
        /// <param name="seismicDesignCategory">The seismic design category.</param>
        /// <returns>create a new entry into database.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(SeismicDesignCategoryDetail))]
        public IHttpActionResult PostSeismicDesignCategory(SeismicDesignCategory seismicDesignCategory)
        {
            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (SeismicDesignCategoryNameExists(seismicDesignCategory.Description, seismicDesignCategory.Id))
                {
                    throw new RpoBusinessException(StaticMessages.SeismicDesignCategoryNameExistsMessage);
                }
                seismicDesignCategory.LastModifiedDate = DateTime.UtcNow;
                seismicDesignCategory.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    seismicDesignCategory.CreatedBy = employee.Id;
                }

                rpoContext.SeismicDesignCategories.Add(seismicDesignCategory);
                rpoContext.SaveChanges();

                SeismicDesignCategory seismicDesignCategoryResponse = rpoContext.SeismicDesignCategories.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == seismicDesignCategory.Id);
                return Ok(FormatDetails(seismicDesignCategoryResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the seismic design category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(SeismicDesignCategory))]
        public IHttpActionResult DeleteSeismicDesignCategory(int id)
        {
            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                SeismicDesignCategory seismicDesignCategory = rpoContext.SeismicDesignCategories.Find(id);
                if (seismicDesignCategory == null)
                {
                    return this.NotFound();
                }

                rpoContext.SeismicDesignCategories.Remove(seismicDesignCategory);
                rpoContext.SaveChanges();

                return Ok(seismicDesignCategory);
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
                rpoContext.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary>
        /// Seismics the design category exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool SeismicDesignCategoryExists(int id)
        {
            return rpoContext.SeismicDesignCategories.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified seismic design category.
        /// </summary>
        /// <param name="seismicDesignCategory">The seismic design category.</param>
        /// <returns>SeismicDesignCategoryDTO.</returns>
        private SeismicDesignCategoryDTO Format(SeismicDesignCategory seismicDesignCategory)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new SeismicDesignCategoryDTO
            {
                Id = seismicDesignCategory.Id,
                Description = seismicDesignCategory.Description,
                Code = seismicDesignCategory.Code,
                CreatedBy = seismicDesignCategory.CreatedBy,
                LastModifiedBy = seismicDesignCategory.LastModifiedBy,
                CreatedByEmployeeName = seismicDesignCategory.CreatedByEmployee != null ? seismicDesignCategory.CreatedByEmployee.FirstName + " " + seismicDesignCategory.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = seismicDesignCategory.LastModifiedByEmployee != null ? seismicDesignCategory.LastModifiedByEmployee.FirstName + " " + seismicDesignCategory.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = seismicDesignCategory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(seismicDesignCategory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : seismicDesignCategory.CreatedDate,
                LastModifiedDate = seismicDesignCategory.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(seismicDesignCategory.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : seismicDesignCategory.LastModifiedDate,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="seismicDesignCategory">The seismic design category.</param>
        /// <returns>SeismicDesignCategoryDetail.</returns>
        private SeismicDesignCategoryDetail FormatDetails(SeismicDesignCategory seismicDesignCategory)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new SeismicDesignCategoryDetail
            {
                Id = seismicDesignCategory.Id,
                Description = seismicDesignCategory.Description,
                Code = seismicDesignCategory.Code,
                CreatedBy = seismicDesignCategory.CreatedBy,
                LastModifiedBy = seismicDesignCategory.LastModifiedBy,
                CreatedByEmployeeName = seismicDesignCategory.CreatedByEmployee != null ? seismicDesignCategory.CreatedByEmployee.FirstName + " " + seismicDesignCategory.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = seismicDesignCategory.LastModifiedByEmployee != null ? seismicDesignCategory.LastModifiedByEmployee.FirstName + " " + seismicDesignCategory.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = seismicDesignCategory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(seismicDesignCategory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : seismicDesignCategory.CreatedDate,
                LastModifiedDate = seismicDesignCategory.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(seismicDesignCategory.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : seismicDesignCategory.LastModifiedDate,
            };
        }

        /// <summary>
        /// Seismics the design category name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool SeismicDesignCategoryNameExists(string name, int id)
        {
            return rpoContext.SeismicDesignCategories.Count(e => e.Description == name && e.Id != id) > 0;
        }
    }
}