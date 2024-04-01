// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-15-2018
// ***********************************************************************
// <copyright file="CompanyTypesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Company Types Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.CompanyTypes
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
    using Model.Models.Enums;

    /// <summary>
    /// Class Company Types Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />

    [Authorize]
    [RpoAuthorize]
    [RpoAuthorize(FunctionGrantType.Master)]
    public class CompanyTypesController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the company types.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttpActionResult.  Get companyType List</returns>
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetCompanyTypes([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var companyTypes = db.CompanyTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = companyTypes.Count();
                var recordsFiltered = recordsTotal;

                var result = companyTypes
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
        /// Gets the company type dropdown.
        /// </summary>
        /// <returns>IHttpActionResult. Get companyType List</returns>
        [HttpGet]
        [Route("api/companytypes/dropdown")]
        public IHttpActionResult GetCompanyTypeDropdown()
        {
            var result = db.CompanyTypes.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.ItemName,
                IdParent = c.IdParent
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the type of the company.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult. Get companyType detail</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CompanyTypeDetail))]
        public IHttpActionResult GetCompanyType(int id)
        {
            CompanyType companyType = db.CompanyTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

            if (companyType == null)
            {
                return this.NotFound();
            }

            return Ok(FormatDetails(companyType));
        }

        /// <summary>
        /// Puts the type of the company.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="companyType">Type of the company.</param>
        /// <returns>IHttpActionResult.Update the company Type</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCompanyType(int id, CompanyType companyType)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != companyType.Id)
                {
                    return BadRequest();
                }

                if (CompanyTypeNameExists(companyType.ItemName, companyType.Id))
                {
                    throw new RpoBusinessException(StaticMessages.CompanyTypeNameExistsMessage);
                }
                companyType.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    companyType.LastModifiedBy = employee.Id;
                }

                db.Entry(companyType).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyTypeExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                CompanyType companyTypeResponse = db.CompanyTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return Ok(FormatDetails(companyTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the type of the company.
        /// </summary>
        /// <param name="companyType">Type of the company.</param>
        /// <returns>IHttpActionResult. Insert the new company type</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CompanyType))]
        public IHttpActionResult PostCompanyType(CompanyType companyType)
        {

            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (CompanyTypeNameExists(companyType.ItemName, companyType.Id))
                {
                    throw new RpoBusinessException(StaticMessages.CompanyTypeNameExistsMessage);
                }

                companyType.LastModifiedDate = DateTime.UtcNow;
                companyType.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    companyType.CreatedBy = employee.Id;
                }

                db.CompanyTypes.Add(companyType);
                db.SaveChanges();

                CompanyType companyTypeResponse = db.CompanyTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == companyType.Id);
                return Ok(FormatDetails(companyTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the type of the company.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult. Delete company type</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CompanyType))]
        public IHttpActionResult DeleteCompanyType(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                CompanyType companyType = db.CompanyTypes.Find(id);
                if (companyType == null)
                {
                    return this.NotFound();
                }

                db.CompanyTypes.Remove(companyType);
                db.SaveChanges();

                return Ok(companyType);
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
        /// Companies the type exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool CompanyTypeExists(int id)
        {
            return db.CompanyTypes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified company type.
        /// </summary>
        /// <param name="companyType">Type of the company.</param>
        /// <returns>CompanyTypeDTO.</returns>
        private CompanyTypeDTO Format(CompanyType companyType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new CompanyTypeDTO
            {
                Id = companyType.Id,
                ItemName = companyType.ItemName,
                IdParent = companyType.IdParent,
                CreatedBy = companyType.CreatedBy,
                LastModifiedBy = companyType.LastModifiedBy,
                CreatedByEmployeeName = companyType.CreatedByEmployee != null ? companyType.CreatedByEmployee.FirstName + " " + companyType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = companyType.LastModifiedByEmployee != null ? companyType.LastModifiedByEmployee.FirstName + " " + companyType.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = companyType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyType.CreatedDate,
                LastModifiedDate = companyType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyType.LastModifiedDate,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="companyType">Type of the company.</param>
        /// <returns>CompanyTypeDetail.</returns>
        private CompanyTypeDetail FormatDetails(CompanyType companyType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new CompanyTypeDetail
            {
                Id = companyType.Id,
                ItemName = companyType.ItemName,
                IdParent = companyType.IdParent,
                CreatedBy = companyType.CreatedBy,
                LastModifiedBy = companyType.LastModifiedBy,
                CreatedByEmployeeName = companyType.CreatedByEmployee != null ? companyType.CreatedByEmployee.FirstName + " " + companyType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = companyType.LastModifiedByEmployee != null ? companyType.LastModifiedByEmployee.FirstName + " " + companyType.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = companyType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyType.CreatedDate,
                LastModifiedDate = companyType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyType.LastModifiedDate,
            };
        }

        /// <summary>
        /// Companies the type name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool CompanyTypeNameExists(string name, int id)
        {
            return db.CompanyTypes.Count(e => e.ItemName == name && e.Id != id) > 0;
        }
    }
}