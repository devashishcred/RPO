namespace Rpo.ApiServices.Api.Controllers.DocumentMaster
{
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using System;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Api.DataTable;
    using Filters;
    using Rpo.ApiServices.Model.Models.Enums;
    using Models;
    using System.Net;
    using System.Net.Http;
    public class DocumentMastersController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the states.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetDocumentMasters([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var DocumentMasters = db.DocumentMasters.Where(d => d.IsNewDocument == true).AsQueryable();

                var recordsTotal = DocumentMasters.Count();
                var recordsFiltered = recordsTotal;

                var result = DocumentMasters
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

        private DocumentMasterDTO Format(DocumentMaster documentMaster)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new DocumentMasterDTO
            {
                Id = documentMaster.Id,
                DocumentName = documentMaster.DocumentName,
                ItemName = documentMaster.DocumentName,
                Code = documentMaster.Code,
                Path = documentMaster.Path,
                DisplayOrder = documentMaster.DisplayOrder,
                IsAddPage = documentMaster.IsAddPage,
                IsDevelopementCompleted = documentMaster.IsDevelopementCompleted,
                IsNewDocument = documentMaster.IsNewDocument,
            };
        }
        /// <summary>
        /// Gets the Document Master detail.
        /// </summary>
        /// <param name="id">The id parameters.</param>
        /// <returns>IHttp Action Result. get the document detail</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DocumentMasterDetail))]
        public IHttpActionResult GetDocumentMasters(int id)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                DocumentMaster documentmaster = db.DocumentMasters.FirstOrDefault(x => x.Id == id && x.IsNewDocument == true);

                if (documentmaster == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(documentmaster));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        private DocumentMasterDetail FormatDetails(DocumentMaster documentmaster)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new DocumentMasterDetail
            {
                Id = documentmaster.Id,
                DocumentName = documentmaster.DocumentName,
                ItemName = documentmaster.DocumentName,
                Code = documentmaster.Code,
                Path = documentmaster.Path,
                DisplayOrder = documentmaster.DisplayOrder,
                IsAddPage = documentmaster.IsAddPage,
                IsDevelopementCompleted = documentmaster.IsDevelopementCompleted,
                IsNewDocument = documentmaster.IsNewDocument,
            };
        }

        /// <summary>
        /// put the DocumentMasterDetail.
        /// </summary>
        /// <param name="DocumentMaster">The DocumentMaster parameters.</param>
        /// <returns>IHttp Action Result. update the detail of DocumentMaster</returns>

        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DocumentMasterDetail))]
        public IHttpActionResult PutDocumentMaster(int id, DocumentMaster documentMaster)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != documentMaster.Id)
                {
                    return BadRequest();
                }

                if (DocumentcodeExists(documentMaster.Code, documentMaster.Id))
                {
                    throw new RpoBusinessException(StaticMessages.DocumentCodeExistsMessage);
                }

                if (DocumentMasterExists(documentMaster.DocumentName, documentMaster.Id))
                {
                    throw new RpoBusinessException(StaticMessages.DocumentNameExistsMessage);
                }

                documentMaster.DisplayOrder = 0;
                documentMaster.IsAddPage = false;
                documentMaster.IsDevelopementCompleted = true;
                documentMaster.IsNewDocument = true;
                db.Entry(documentMaster).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentcodeExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }

                    if (!DocumentMasterExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                DocumentMaster documentMasterResponse = db.DocumentMasters.FirstOrDefault(x => x.Id == id && x.IsNewDocument == true);
                return Ok(FormatDetails(documentMasterResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the state.
        /// </summary>
        /// <param name="documentmaster">The documentmaster.</param>
        /// <returns>IHttpActionResult.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DocumentMasterDetail))]
        public IHttpActionResult PostDocumentMaster(DocumentMaster documentmaster)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (DocumentcodeExists(documentmaster.Code, documentmaster.Id))
                {
                    throw new RpoBusinessException(StaticMessages.DocumentCodeExistsMessage);
                }

                if (DocumentMasterExists(documentmaster.DocumentName, documentmaster.Id))
                {
                    throw new RpoBusinessException(StaticMessages.DocumentNameExistsMessage);
                }
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        documentmaster.DisplayOrder = 0;
                        documentmaster.IsAddPage = false;
                        documentmaster.IsDevelopementCompleted = true;
                        documentmaster.IsNewDocument = true;
                        db.DocumentMasters.Add(documentmaster);
                        db.SaveChanges();

                        Field objfield = new Field();
                        objfield.FieldName = "Application";
                        objfield.ControlType = ControlType.DropDown;
                        objfield.DataType = FieldDataType.Integer;
                        objfield.DisplayFieldName = "Application";
                        objfield.IsDisplayInFrontend = true;
                        objfield.IdDocument = documentmaster.Id;
                        db.Fields.Add(objfield);
                        db.SaveChanges();

                        DocumentField objDocumentField = new DocumentField();
                        objDocumentField.IdField = objfield.Id;
                        objDocumentField.IsRequired = false;
                        objDocumentField.APIUrl = "/api/jobdocumentdrodown/JobApplicationNumberType";
                        objDocumentField.IsPlaceHolder = false;
                        objDocumentField.DisplayOrder = 1;
                        objDocumentField.IdDocument = documentmaster.Id;
                        db.DocumentFields.Add(objDocumentField);
                        db.SaveChanges();

                        Field objfield1 = new Field();
                        objfield1.FieldName = "For";
                        objfield1.ControlType = ControlType.TextAreaFor;
                        objfield1.DataType = FieldDataType.String;
                        objfield1.DisplayFieldName = "For";
                        objfield1.IsDisplayInFrontend = true;
                        objfield1.IdDocument = documentmaster.Id;
                        db.Fields.Add(objfield1);
                        db.SaveChanges();

                        DocumentField objDocumentField1 = new DocumentField();
                        objDocumentField1.IdField = objfield1.Id;
                        objDocumentField1.IsRequired = false;
                        objDocumentField1.APIUrl = "api/jobdocumentdrodown/JobApplicationFor/";
                        objDocumentField1.IsPlaceHolder = false;
                        objDocumentField1.DisplayOrder = 2;
                        objDocumentField1.IdParentField = objDocumentField.Id;
                        objDocumentField1.IdDocument = documentmaster.Id;
                        db.DocumentFields.Add(objDocumentField1);
                        db.SaveChanges();

                        Field objfield2 = new Field();
                        objfield2.FieldName = "Attachment";
                        objfield2.ControlType = ControlType.FileType;
                        objfield2.DataType = FieldDataType.Integer;
                        objfield2.DisplayFieldName = "Attachment";
                        objfield2.IsDisplayInFrontend = true;
                        objfield2.IdDocument = documentmaster.Id;
                        db.Fields.Add(objfield2);
                        db.SaveChanges();

                        DocumentField objDocumentField2 = new DocumentField();
                        objDocumentField2.IdField = objfield2.Id;
                        objDocumentField2.IsRequired = false;
                        objDocumentField2.IsPlaceHolder = false;
                        objDocumentField2.DisplayOrder = 4;
                        objDocumentField2.IdDocument = documentmaster.Id;
                        db.DocumentFields.Add(objDocumentField2);
                        db.SaveChanges();

                        Field objfield3 = new Field();
                        objfield3.FieldName = "Additional Description";
                        objfield3.ControlType = ControlType.TextArea;
                        objfield3.DataType = FieldDataType.String;
                        objfield3.DisplayFieldName = "Additional Description";
                        objfield3.IdDocument = documentmaster.Id;
                        objfield3.IsDisplayInFrontend = true;
                        db.Fields.Add(objfield3);
                        db.SaveChanges();

                        DocumentField objDocumentField3 = new DocumentField();
                        objDocumentField3.IdField = objfield3.Id;
                        objDocumentField3.IsRequired = true;
                        objDocumentField3.IsPlaceHolder = false;
                        objDocumentField3.DisplayOrder = 3;
                        objDocumentField3.IdDocument = documentmaster.Id;
                        db.DocumentFields.Add(objDocumentField3);
                        db.SaveChanges();
                        transaction.Commit();

                        DocumentMaster documentmasterResponse = db.DocumentMasters.FirstOrDefault(x => x.Id == documentmaster.Id && x.IsNewDocument == true);
                        return Ok(FormatDetails(documentmasterResponse));
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Error in saving transmittal data in database. Please try again." }));
                    }
                }
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the state.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult. delete the document in documentmaster </returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DocumentMasterDetail))]
        public IHttpActionResult DeleteDocumentMaster(int id)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                DocumentMaster documentMaster = db.DocumentMasters.Find(id);
                if (documentMaster == null)
                {
                    return this.NotFound();
                }

                var jobDocuments = this.db.JobDocuments.Where(x => x.IdDocument == id);
                if (jobDocuments != null && jobDocuments.Count() > 0)
                {
                    throw new RpoBusinessException(StaticMessages.SupportDocumentExistForThisDocument);
                }

                var fields = this.db.Fields.Where(x => x.IdDocument == id);
                if (fields.Any())
                {
                    this.db.Fields.RemoveRange(fields);
                }
                var documentfields = this.db.DocumentFields.Where(x => x.IdDocument == id);
                if (documentfields.Any())
                {
                    this.db.DocumentFields.RemoveRange(documentfields);
                }

                db.DocumentMasters.Remove(documentMaster);
                db.SaveChanges();

                return Ok(documentMaster);
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

        private bool DocumentMasterExists(int id)
        {
            return db.DocumentMasters.Count(e => e.Id == id) > 0;
        }
        private bool DocumentMasterExists(string name, int id)
        {
            return db.DocumentMasters.Count(e => e.DocumentName == name && e.Id != id) > 0;
        }

        private bool DocumentcodeExists(int id)
        {
            return db.DocumentMasters.Count(e => e.Id == id) > 0;
        }
        private bool DocumentcodeExists(string code, int id)
        {
            return db.DocumentMasters.Count(e => e.Code == code && e.Id != id) > 0;
        }
    }
}
