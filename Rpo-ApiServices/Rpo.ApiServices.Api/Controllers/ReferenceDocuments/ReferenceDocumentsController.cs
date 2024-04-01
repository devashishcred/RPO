// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="ReferenceDocumentsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Reference Documents Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Filters;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Api.Tools;


    /// <summary>
    /// Class Reference Documents Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class ReferenceDocumentsController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the reference documents.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the reference documents List.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/ReferenceDocuments")]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetReferenceDocuments([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewReferenceDocument)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddReferenceDocument)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteReferenceDocument))
            {
                if (dataTableParameters == null)
                    dataTableParameters = new DataTableParameters();

                var recordsTotal = rpoContext.ReferenceDocuments.Count();
                var recordsFiltered = recordsTotal;

                var result = rpoContext.ReferenceDocuments
                    .Select(rd => new
                    {
                        Id = rd.Id,
                        Description = rd.Description,
                        FileName = rd.FileName,
                        Keywords = rd.Keywords,
                        Name = rd.Name
                    })
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
        /// Gets the reference document.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the reference document.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/ReferenceDocuments/{id}")]
        [ResponseType(typeof(ReferenceDocument))]
        public IHttpActionResult GetReferenceDocument(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewReferenceDocument)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddReferenceDocument)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteReferenceDocument))
            {
                ReferenceDocument referenceDocument = rpoContext.ReferenceDocuments.Find(id);
                if (referenceDocument == null)
                {
                    return this.NotFound();
                }

                string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
                string downloadPath = Convert.ToString(referenceDocument.Id) + "_" + referenceDocument.ContentPath;
                referenceDocument.ContentPath = APIUrl + "/" + Convert.ToString(Properties.Settings.Default.UploadedDocumentPath) + "/" + downloadPath;

                return Ok(referenceDocument);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the reference document.
        /// </summary>
        /// <returns>update the reference documents.</returns>
        /// <exception cref="HttpResponseException"></exception>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/ReferenceDocuments")]
        public async Task<HttpResponseMessage> PutReferenceDocument()
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddReferenceDocument))
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
                System.Collections.Specialized.NameValueCollection formData = provider.FormData;
                string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

                if (Convert.ToBoolean(formData["FileAttached"]))
                {
                    IList<HttpContent> files = provider.Files;

                    HttpContent file1 = files[0];
                    var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');

                    string filename = string.Empty;
                    Stream input = await file1.ReadAsStreamAsync();
                    string directoryName = string.Empty;
                    string URL = string.Empty;


                    var path = HttpRuntime.AppDomainAppPath;
                    directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.UploadedDocumentPath));

                    int id = int.Parse(formData["Id"]);


                    ReferenceDocument referenceDocument = rpoContext.ReferenceDocuments.Find(id);
                    if (referenceDocument == null)
                    {
                        return new HttpResponseMessage(HttpStatusCode.NotFound);
                    }

                    referenceDocument.ContentPath = thisFileName;
                    referenceDocument.FileName = formData["FileName"];
                    referenceDocument.Keywords = formData["Keywords"];
                    referenceDocument.Name = formData["Name"];
                    referenceDocument.Description = formData["Description"];
                    referenceDocument.LastModifiedDate = DateTime.UtcNow;
                    if (employee != null)
                    {
                        referenceDocument.LastModifiedBy = employee.Id;
                    }

                    if (ReferenceDocumentNameExists(referenceDocument.Name, referenceDocument.Id))
                    {
                        Dictionary<string, string> message = new Dictionary<string, string>();
                        message.Add("error", "Document name is already exists.");
                        var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, message);
                        return badResponse;
                    }

                    string directoryDelete = Convert.ToString(referenceDocument.Id) + "_" + referenceDocument.ContentPath;
                    string deletefilename = System.IO.Path.Combine(directoryName, directoryDelete);

                    if (File.Exists(deletefilename))
                    {
                        File.Delete(deletefilename);
                    }

                    rpoContext.SaveChanges();

                    string directoryFileName = Convert.ToString(referenceDocument.Id) + "_" + thisFileName;
                    filename = System.IO.Path.Combine(directoryName, directoryFileName);

                    if (Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }

                    using (Stream file = File.OpenWrite(filename))
                    {
                        input.CopyTo(file);
                        file.Close();
                    }

                    referenceDocument.ContentPath = APIUrl + "/" + Convert.ToString(Properties.Settings.Default.UploadedDocumentPath) + "/" + directoryFileName;
                    var response = Request.CreateResponse<ReferenceDocument>(HttpStatusCode.OK, referenceDocument);
                    return response;
                }
                else
                {
                    int id = int.Parse(formData["Id"]);

                    ReferenceDocument referenceDocument = rpoContext.ReferenceDocuments.Find(id);

                    if (referenceDocument == null)
                    {
                        return new HttpResponseMessage(HttpStatusCode.NotFound);
                    }

                    referenceDocument.FileName = formData["FileName"];
                    referenceDocument.Keywords = formData["Keywords"];
                    referenceDocument.Name = formData["Name"];
                    referenceDocument.Description = formData["Description"];
                    referenceDocument.LastModifiedDate = DateTime.UtcNow;
                    if (employee != null)
                    {
                        referenceDocument.LastModifiedBy = employee.Id;
                    }

                    if (ReferenceDocumentNameExists(referenceDocument.Name, referenceDocument.Id))
                    {
                        Dictionary<string, string> message = new Dictionary<string, string>();
                        message.Add("error", "Document name is already exists.");
                        var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, message);
                        return badResponse;
                    }

                    rpoContext.SaveChanges();
                    string directoryFileName = Convert.ToString(referenceDocument.Id) + "_" + referenceDocument.ContentPath;
                    referenceDocument.ContentPath = APIUrl + "/" + Convert.ToString(Properties.Settings.Default.UploadedDocumentPath) + "/" + directoryFileName;
                    var response = Request.CreateResponse<ReferenceDocument>(HttpStatusCode.OK, referenceDocument);
                    return response;
                }
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the reference document.
        /// </summary>
        /// <returns>create a new reference documents..</returns>
        /// <exception cref="HttpResponseException"></exception>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/ReferenceDocuments")]
        public async Task<HttpResponseMessage> PostReferenceDocument()
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddReferenceDocument))
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
                System.Collections.Specialized.NameValueCollection formData = provider.FormData;
                IList<HttpContent> files = provider.Files;

                HttpContent file1 = files[0];
                var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');

                string filename = string.Empty;
                Stream input = await file1.ReadAsStreamAsync();
                string directoryName = string.Empty;
                string URL = string.Empty;
                string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

                ReferenceDocument referenceDocument = new ReferenceDocument();

                referenceDocument.ContentPath = thisFileName;
                referenceDocument.FileName = formData["FileName"];
                referenceDocument.Keywords = formData["Keywords"];
                referenceDocument.Name = formData["Name"];
                referenceDocument.Description = formData["Description"];
                referenceDocument.LastModifiedDate = DateTime.UtcNow;
                referenceDocument.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    referenceDocument.CreatedBy = employee.Id;
                }

                if (ReferenceDocumentNameExists(referenceDocument.Name, referenceDocument.Id))
                {
                    Dictionary<string, string> message = new Dictionary<string, string>();
                    message.Add("error", "Document name is already exists.");
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, message);
                    return badResponse;
                }

                rpoContext.ReferenceDocuments.Add(referenceDocument);
                rpoContext.SaveChanges();

                var path = HttpRuntime.AppDomainAppPath;
                directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.UploadedDocumentPath));

                string directoryFileName = Convert.ToString(referenceDocument.Id) + "_" + thisFileName;
                filename = System.IO.Path.Combine(directoryName, directoryFileName);

                if (Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                using (Stream file = File.OpenWrite(filename))
                {
                    input.CopyTo(file);
                    file.Close();
                }

                referenceDocument.ContentPath = APIUrl + "/" + Convert.ToString(Properties.Settings.Default.UploadedDocumentPath) + "/" + directoryFileName;
                var response = Request.CreateResponse<ReferenceDocument>(HttpStatusCode.OK, referenceDocument);
                return response;
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the reference document.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Delete the reference documents..</returns>
        [HttpDelete]
        [Authorize]
        [RpoAuthorize]
        [Route("api/ReferenceDocuments/{id}")]
        [ResponseType(typeof(ReferenceDocument))]
        public IHttpActionResult DeleteReferenceDocument(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteReferenceDocument))
            {
                ReferenceDocument referenceDocument = rpoContext.ReferenceDocuments.Find(id);
                if (referenceDocument == null)
                {
                    return this.NotFound();
                }

                rpoContext.ReferenceDocuments.Remove(referenceDocument);
                rpoContext.SaveChanges();

                var path = HttpRuntime.AppDomainAppPath;
                string directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.UploadedDocumentPath));
                string directoryDelete = Convert.ToString(referenceDocument.Id) + "_" + referenceDocument.ContentPath;
                string deletefilename = System.IO.Path.Combine(directoryName, directoryDelete);

                if (File.Exists(deletefilename))
                {
                    File.Delete(deletefilename);
                }

                return Ok(referenceDocument);
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
        /// References the document exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ReferenceDocumentExists(int id)
        {
            return rpoContext.ReferenceDocuments.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// References the document name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ReferenceDocumentNameExists(string name, int id)
        {
            return rpoContext.ReferenceDocuments.Count(e => e.Name == name && e.Id != id) > 0;
        }
    }
}