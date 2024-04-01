// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-15-2018
// ***********************************************************************
// <copyright file="DocumentTypesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Document Types Controller.</summary>
// ***********************************************************************

/// <summary>
/// The DocumentTypes namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.DocumentTypes
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
    /// Class Document Types Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class DocumentTypesController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the document types.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the document types List.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetDocumentTypes([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var documentTypes = db.DocumentTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = documentTypes.Count();
                var recordsFiltered = recordsTotal;

                var result = documentTypes
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
        /// Gets the document type dropdown.
        /// </summary>
        /// <returns>Gets the list document types for dropdown.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/documenttypes/dropdown")]
        public IHttpActionResult GetDocumentTypeDropdown()
        {
            var result = db.DocumentTypes.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the type of the document.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the detail document types.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DocumentTypeDetail))]
        public IHttpActionResult GetDocumentType(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                DocumentType documentType = db.DocumentTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (documentType == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(documentType));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the type of the document.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="documentType">Type of the document.</param>
        /// <returns>update the document type in database</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DocumentTypeDetail))]
        public IHttpActionResult PutDocumentType(int id, DocumentType documentType)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != documentType.Id)
                {
                    return BadRequest();
                }

                if (DocumentTypeNameExists(documentType.Name, documentType.Id))
                {
                    throw new RpoBusinessException(StaticMessages.DocumentTypeNameExistsMessage);
                }
                documentType.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    documentType.LastModifiedBy = employee.Id;
                }

                db.Entry(documentType).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentTypeExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                DocumentType documentTypeResponse = db.DocumentTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return Ok(FormatDetails(documentTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the type of the document.
        /// </summary>
        /// <param name="documentType">Type of the document.</param>
        /// <returns>create a new documentType in db</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DocumentTypeDetail))]
        public IHttpActionResult PostDocumentType(DocumentType documentType)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (DocumentTypeNameExists(documentType.Name, documentType.Id))
                {
                    throw new RpoBusinessException(StaticMessages.DocumentTypeNameExistsMessage);
                }
                documentType.LastModifiedDate = DateTime.UtcNow;
                documentType.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    documentType.CreatedBy = employee.Id;
                }

                db.DocumentTypes.Add(documentType);
                db.SaveChanges();

                DocumentType documentTypeResponse = db.DocumentTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == documentType.Id);
                return Ok(FormatDetails(documentTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the type of the document.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>delete the entry of document type</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DocumentType))]
        public IHttpActionResult DeleteDocumentType(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                DocumentType documentType = db.DocumentTypes.Find(id);
                if (documentType == null)
                {
                    return this.NotFound();
                }

                db.DocumentTypes.Remove(documentType);
                db.SaveChanges();

                return Ok(documentType);
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
        /// Documents the type exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool DocumentTypeExists(int id)
        {
            return db.DocumentTypes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified document type.
        /// </summary>
        /// <param name="documentType">Type of the document.</param>
        /// <returns>DocumentTypeDTO.</returns>
        private DocumentTypeDTO Format(DocumentType documentType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new DocumentTypeDTO
            {
                Id = documentType.Id,
                Name = documentType.Name,
                CreatedBy = documentType.CreatedBy,
                LastModifiedBy = documentType.LastModifiedBy,
                CreatedByEmployeeName = documentType.CreatedByEmployee != null ? documentType.CreatedByEmployee.FirstName + " " + documentType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = documentType.LastModifiedByEmployee != null ? documentType.LastModifiedByEmployee.FirstName + " " + documentType.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = documentType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(documentType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : documentType.CreatedDate,
                LastModifiedDate = documentType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(documentType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : documentType.LastModifiedDate,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="documentType">Type of the document.</param>
        /// <returns>DocumentTypeDetail.</returns>
        private DocumentTypeDetail FormatDetails(DocumentType documentType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new DocumentTypeDetail
            {
                Id = documentType.Id,
                Name = documentType.Name,
                CreatedBy = documentType.CreatedBy,
                LastModifiedBy = documentType.LastModifiedBy,
                CreatedByEmployeeName = documentType.CreatedByEmployee != null ? documentType.CreatedByEmployee.FirstName + " " + documentType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = documentType.LastModifiedByEmployee != null ? documentType.LastModifiedByEmployee.FirstName + " " + documentType.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = documentType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(documentType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : documentType.CreatedDate,
                LastModifiedDate = documentType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(documentType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : documentType.LastModifiedDate,
            };
        }

        /// <summary>
        /// Documents the type name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool DocumentTypeNameExists(string name, int id)
        {
            return db.DocumentTypes.Count(e => e.Name == name && e.Id != id) > 0;
        }
    }
}