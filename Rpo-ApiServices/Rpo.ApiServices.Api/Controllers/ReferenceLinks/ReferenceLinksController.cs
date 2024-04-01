// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 03-30-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-30-2018
// ***********************************************************************
// <copyright file="ReferenceLinksController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Reference Links Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Reference Links namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.ReferenceLinks
{
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Rpo.ApiServices.Api.Tools;
    using System;
    using Rpo.ApiServices.Api.DataTable;
    using Filters;
    using Model.Models.Enums;

    /// <summary>
    /// Class Reference Links Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
   
    public class ReferenceLinksController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the reference links.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the reference links list.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetReferenceLinks([FromUri] DataTableParameters dataTableParameters)
        {
            var referenceLinks = rpoContext.ReferenceLinks.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

            var recordsTotal = referenceLinks.Count();
            var recordsFiltered = recordsTotal;

            var result = referenceLinks
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
        /// Gets the reference link dropdown.
        /// </summary>
        /// <returns>Gets the reference link for dropdown.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/referencelinks/dropdown")]
        public IHttpActionResult GetReferenceLinkDropdown()
        {
            var result = rpoContext.ReferenceLinks.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the reference link.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the reference link in detail.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ReferenceLinkDetail))]
        public IHttpActionResult GetReferenceLink(int id)
        {
            ReferenceLink referenceLink = rpoContext.ReferenceLinks.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

            if (referenceLink == null)
            {
                return this.NotFound();
            }

            return Ok(FormatDetails(referenceLink));
        }

        /// <summary>
        /// Puts the reference link.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="referenceLink">The reference link.</param>
        /// <returns>update the reference link.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ReferenceLinkDetail))]
        public IHttpActionResult PutReferenceLink(int id, ReferenceLink referenceLink)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != referenceLink.Id)
            {
                return BadRequest();
            }

            if (ReferenceLinkNameExists(referenceLink.Name, referenceLink.Id))
            {
                throw new RpoBusinessException(StaticMessages.ReferenceLinkNameExistsMessage);
            }

            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            referenceLink.LastModifiedDate = DateTime.UtcNow;
            if (employee != null)
            {
                referenceLink.LastModifiedBy = employee.Id;
            }

            rpoContext.Entry(referenceLink).State = EntityState.Modified;

            try
            {
                rpoContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReferenceLinkExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            ReferenceLink referenceLinkResponse = rpoContext.ReferenceLinks.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
            return Ok(FormatDetails(referenceLinkResponse));
        }

        /// <summary>
        /// Posts the reference link.
        /// </summary>
        /// <param name="referenceLink">The reference link.</param>
        /// <returns>create a new reference link.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ReferenceLinkDetail))]
        public IHttpActionResult PostReferenceLink(ReferenceLink referenceLink)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (ReferenceLinkNameExists(referenceLink.Name, referenceLink.Id))
            {
                throw new RpoBusinessException(StaticMessages.ReferenceLinkNameExistsMessage);
            }

            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            referenceLink.LastModifiedDate = DateTime.UtcNow;
            referenceLink.CreatedDate = DateTime.UtcNow;
            if (employee != null)
            {
                referenceLink.CreatedBy = employee.Id;
            }

            rpoContext.ReferenceLinks.Add(referenceLink);
            rpoContext.SaveChanges();

            ReferenceLink referenceLinkResponse = rpoContext.ReferenceLinks.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == referenceLink.Id);
            return Ok(FormatDetails(referenceLinkResponse));
        }

        /// <summary>
        /// Deletes the reference link.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>delete reference link.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ReferenceLink))]
        public IHttpActionResult DeleteReferenceLink(int id)
        {
            ReferenceLink referenceLink = rpoContext.ReferenceLinks.Find(id);
            if (referenceLink == null)
            {
                return this.NotFound();
            }

            rpoContext.ReferenceLinks.Remove(referenceLink);
            rpoContext.SaveChanges();

            return Ok(referenceLink);
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
        /// References the link exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ReferenceLinkExists(int id)
        {
            return rpoContext.ReferenceLinks.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified reference link.
        /// </summary>
        /// <param name="referenceLink">The reference link.</param>
        /// <returns>ReferenceLinkDTO.</returns>
        private ReferenceLinkDTO Format(ReferenceLink referenceLink)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new ReferenceLinkDTO
            {
                Id = referenceLink.Id,
                Name = referenceLink.Name,
                Url = referenceLink.Url,
                CreatedBy = referenceLink.CreatedBy,
                LastModifiedBy = referenceLink.LastModifiedBy,
                CreatedByEmployeeName = referenceLink.CreatedByEmployee != null ? referenceLink.CreatedByEmployee.FirstName + " " + referenceLink.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = referenceLink.LastModifiedByEmployee != null ? referenceLink.LastModifiedByEmployee.FirstName + " " + referenceLink.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = referenceLink.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(referenceLink.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : referenceLink.CreatedDate,
                LastModifiedDate = referenceLink.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(referenceLink.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : referenceLink.LastModifiedDate,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="referenceLink">The reference link.</param>
        /// <returns>ReferenceLinkDetail.</returns>
        private ReferenceLinkDetail FormatDetails(ReferenceLink referenceLink)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new ReferenceLinkDetail
            {
                Id = referenceLink.Id,
                Name = referenceLink.Name,
                Url = referenceLink.Url,
                CreatedBy = referenceLink.CreatedBy,
                LastModifiedBy = referenceLink.LastModifiedBy,
                CreatedByEmployeeName = referenceLink.CreatedByEmployee != null ? referenceLink.CreatedByEmployee.FirstName + " " + referenceLink.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = referenceLink.LastModifiedByEmployee != null ? referenceLink.LastModifiedByEmployee.FirstName + " " + referenceLink.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = referenceLink.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(referenceLink.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : referenceLink.CreatedDate,
                LastModifiedDate = referenceLink.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(referenceLink.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : referenceLink.LastModifiedDate,
            };
        }

        /// <summary>
        /// References the link name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ReferenceLinkNameExists(string name, int id)
        {
            return rpoContext.ReferenceLinks.Count(e => e.Name == name && e.Id != id) > 0;
        }
    }
}