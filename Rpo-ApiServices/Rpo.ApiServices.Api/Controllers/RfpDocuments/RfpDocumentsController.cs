// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-21-2018
// ***********************************************************************
// <copyright file="RfpAddressesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Rfp Addresses Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.RfpDocuments
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Rfp Addresses Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class RfpDocumentsController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Parameter 1 : DeletedDocumentIds (String)
        /// Parameter 2 : idRfp (String)
        /// Parameter 3 : Document files want to Upload (File)
        /// </summary>
        /// <returns>Task&lt;HttpResponseMessage&gt;. Update the document detail on database</returns>
        /// <exception cref="System.Web.Http.HttpResponseException"></exception>
        //[Authorize]
        //[RpoAuthorize]
        [Route("api/rfpDocuments/document")]
        [ResponseType(typeof(EmployeeDocument))]
        public async Task<HttpResponseMessage> PutRfpDocuments()
        {

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            System.Collections.Specialized.NameValueCollection formData = provider.FormData;
            IList<HttpContent> files = provider.Files;
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            int idRfp = Convert.ToInt32(formData["idRfp"]);

            string deletedDocumentIds = Convert.ToString(formData["deletedDocumentIds"]);

            if (!string.IsNullOrEmpty(deletedDocumentIds))
            {
                foreach (var item in deletedDocumentIds.Split(','))
                {
                    int rfpDocumentId = Convert.ToInt32(item);
                    RfpDocument rfpDocument = rpoContext.RfpDocuments.Where(x => x.Id == rfpDocumentId).FirstOrDefault();
                    if (rfpDocument != null)
                    {
                        rpoContext.RfpDocuments.Remove(rfpDocument);
                        var path = HttpRuntime.AppDomainAppPath;
                        string directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.RFPDocumentPath));
                        string directoryDelete = Convert.ToString(rfpDocument.Id) + "_" + rfpDocument.DocumentPath;
                        string deletefilename = System.IO.Path.Combine(directoryName, directoryDelete);

                        if (File.Exists(deletefilename))
                        {
                            File.Delete(deletefilename);
                        }
                    }
                }
                rpoContext.SaveChanges();
            }

            foreach (HttpContent item in files)
            {
                HttpContent file1 = item;
                var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');

                string filename = string.Empty;
                Stream input = await file1.ReadAsStreamAsync();
                string directoryName = string.Empty;
                string URL = string.Empty;

                RfpDocument rfpDocument = new RfpDocument();
                rfpDocument.DocumentPath = thisFileName;
                rfpDocument.IdRfp = idRfp;
                rfpDocument.Name = thisFileName;
                rpoContext.RfpDocuments.Add(rfpDocument);
                rpoContext.SaveChanges();

                var path = HttpRuntime.AppDomainAppPath;
                directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.RFPDocumentPath));

                string directoryFileName = Convert.ToString(rfpDocument.Id) + "_" + thisFileName;
                filename = System.IO.Path.Combine(directoryName, directoryFileName);

                if (!Directory.Exists(directoryName))
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
            }

            var rfpDocumentList = rpoContext.RfpDocuments
                .Where(x => x.IdRfp == idRfp).ToList();

            var response = Request.CreateResponse<List<RfpDocument>>(HttpStatusCode.OK, rfpDocumentList);
            return response;
        }
    }
}