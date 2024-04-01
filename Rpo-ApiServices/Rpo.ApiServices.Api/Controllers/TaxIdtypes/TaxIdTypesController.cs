// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-20-2018
// ***********************************************************************
// <copyright file="TaxIdTypesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class TaxId Types Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Tax Id Types namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.TaxIdTypes
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
    /// Class TaxId Types Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class TaxIdTypesController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the tax identifier types.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the tax identifier types.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetTaxIdTypes([FromUri] DataTableParameters dataTableParameters)
        {
            var taxIdTypes = rpoContext.TaxIdTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

            var recordsTotal = taxIdTypes.Count();
            var recordsFiltered = recordsTotal;

            var result = taxIdTypes
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

        /// <summary>
        /// Gets the tax identifier type drop down.
        /// </summary>
        /// <returns>Gets the tax identifier types for dropdwon.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/taxidtypes/dropdown")]
        public IHttpActionResult GetTaxIdTypeDropdown()
        {
            var result = rpoContext.TaxIdTypes.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the type of the tax identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the type of the tax identifier.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TaxIdTypeDetail))]
        public IHttpActionResult GetTaxIdType(int id)
        {
            TaxIdType taxIdType = rpoContext.TaxIdTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

            if (taxIdType == null)
            {
                return this.NotFound();
            }

            return Ok(FormatDetails(taxIdType));
        }

        /// <summary>
        /// Puts the type of the tax identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="taxIdType">Type of the tax identifier.</param>
        /// <returns>update the type of the tax identifier.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TaxIdTypeDetail))]
        public IHttpActionResult PutTaxIdType(int id, TaxIdType taxIdType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != taxIdType.Id)
            {
                return BadRequest();
            }

            if (TaxIdTypeNameExists(taxIdType.Name, taxIdType.Id))
            {
                throw new RpoBusinessException(StaticMessages.TaxIdTypeNameExistsMessage);
            }

            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            taxIdType.LastModifiedDate = DateTime.UtcNow;
            if (employee != null)
            {
                taxIdType.LastModifiedBy = employee.Id;
            }

            rpoContext.Entry(taxIdType).State = EntityState.Modified;

            try
            {
                rpoContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaxIdTypeExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            TaxIdType taxIdTypeResponse = rpoContext.TaxIdTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
            return Ok(FormatDetails(taxIdTypeResponse));
        }

        /// <summary>
        /// Posts the type of the tax identifier.
        /// </summary>
        /// <param name="taxIdType">Type of the tax identifier.</param>
        /// <returns>create the type of the tax identifier.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TaxIdTypeDetail))]
        public IHttpActionResult PostTaxIdType(TaxIdType taxIdType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (TaxIdTypeNameExists(taxIdType.Name, taxIdType.Id))
            {
                throw new RpoBusinessException(StaticMessages.TaxIdTypeNameExistsMessage);
            }

            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            taxIdType.LastModifiedDate = DateTime.UtcNow;
            taxIdType.CreatedDate = DateTime.UtcNow;
            if (employee != null)
            {
                taxIdType.CreatedBy = employee.Id;
            }

            rpoContext.TaxIdTypes.Add(taxIdType);
            rpoContext.SaveChanges();

            TaxIdType taxIdTypeResponse = rpoContext.TaxIdTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == taxIdType.Id);
            return Ok(FormatDetails(taxIdTypeResponse));
        }

        /// <summary>
        /// Deletes the type of the tax identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns> Deletes the type of the tax identifier.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TaxIdType))]
        public IHttpActionResult DeleteTaxIdType(int id)
        {
            TaxIdType taxIdType = rpoContext.TaxIdTypes.Find(id);
            if (taxIdType == null)
            {
                return this.NotFound();
            }

            rpoContext.TaxIdTypes.Remove(taxIdType);
            rpoContext.SaveChanges();

            return Ok(taxIdType);
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
        /// Taxes the identifier type exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool TaxIdTypeExists(int id)
        {
            return rpoContext.TaxIdTypes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified tax identifier type.
        /// </summary>
        /// <param name="taxIdType">Type of the tax identifier.</param>
        /// <returns>TaxIdTypeDTO.</returns>
        private TaxIdTypeDTO Format(TaxIdType taxIdType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new TaxIdTypeDTO
            {
                Id = taxIdType.Id,
                Name = taxIdType.Name,
                CreatedBy = taxIdType.CreatedBy,
                LastModifiedBy = taxIdType.LastModifiedBy,
                CreatedByEmployeeName = taxIdType.CreatedByEmployee != null ? taxIdType.CreatedByEmployee.FirstName + " " + taxIdType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = taxIdType.LastModifiedByEmployee != null ? taxIdType.LastModifiedByEmployee.FirstName + " " + taxIdType.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = taxIdType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(taxIdType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : taxIdType.CreatedDate,
                LastModifiedDate = taxIdType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(taxIdType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : taxIdType.LastModifiedDate,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="taxIdType">Type of the tax identifier.</param>
        /// <returns>TaxIdTypeDetail.</returns>
        private TaxIdTypeDetail FormatDetails(TaxIdType taxIdType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new TaxIdTypeDetail
            {
                Id = taxIdType.Id,
                Name = taxIdType.Name,
                CreatedBy = taxIdType.CreatedBy,
                LastModifiedBy = taxIdType.LastModifiedBy,
                CreatedByEmployeeName = taxIdType.CreatedByEmployee != null ? taxIdType.CreatedByEmployee.FirstName + " " + taxIdType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = taxIdType.LastModifiedByEmployee != null ? taxIdType.LastModifiedByEmployee.FirstName + " " + taxIdType.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = taxIdType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(taxIdType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : taxIdType.CreatedDate,
                LastModifiedDate = taxIdType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(taxIdType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : taxIdType.LastModifiedDate,
            };
        }

        /// <summary>
        /// Taxes the identifier type name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool TaxIdTypeNameExists(string name, int id)
        {
            return rpoContext.TaxIdTypes.Count(e => e.Name == name && e.Id != id) > 0;
        }
    }
}