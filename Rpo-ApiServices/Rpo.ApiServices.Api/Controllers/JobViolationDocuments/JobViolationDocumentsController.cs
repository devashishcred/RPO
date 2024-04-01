
namespace Rpo.ApiServices.Api.Controllers.JobViolationDocuments
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

    public class JobViolationDocumentsController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Parameter 1 : DeletedDocumentIds (String)
        /// Parameter 2 : idJobViolation (String)
        /// Parameter 3 : Document files want to Upload (File)
        /// </summary>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        /// <exception cref="System.Web.Http.HttpResponseException"></exception>
        [Authorize]
        [RpoAuthorize]
        [Route("api/JobViolationDocuments/document")]
        [ResponseType(typeof(JobViolationDocument))]
        public async Task<HttpResponseMessage> PutJobViolationDocuments()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            System.Collections.Specialized.NameValueCollection formData = provider.FormData;
            IList<HttpContent> files = provider.Files;
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            int idJobViolation = Convert.ToInt32(formData["idJobViolation"]);

            string deletedDocumentIds = Convert.ToString(formData["deletedDocumentIds"]);

            if (!string.IsNullOrEmpty(deletedDocumentIds))
            {
                foreach (var item in deletedDocumentIds.Split(','))
                {
                    int jobViolationDocumentId = Convert.ToInt32(item);
                    JobViolationDocument jobViolationDocument = rpoContext.JobViolationDocuments.Where(x => x.Id == jobViolationDocumentId).FirstOrDefault();
                    if (jobViolationDocument != null)
                    {
                        rpoContext.JobViolationDocuments.Remove(jobViolationDocument);
                        var path = HttpRuntime.AppDomainAppPath;
                        string directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobViolationDocumentPath));
                        string directoryDelete = Convert.ToString(jobViolationDocument.Id) + "_" + jobViolationDocument.DocumentPath;
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

                JobViolationDocument jobViolationDocument = new JobViolationDocument();
                jobViolationDocument.DocumentPath = thisFileName;
                jobViolationDocument.IdJobViolation = idJobViolation;
                jobViolationDocument.Name = thisFileName;
                rpoContext.JobViolationDocuments.Add(jobViolationDocument);
                rpoContext.SaveChanges();

                var path = HttpRuntime.AppDomainAppPath;
                directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobViolationDocumentPath));

                string directoryFileName = Convert.ToString(jobViolationDocument.Id) + "_" + thisFileName;
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

            var jobViolationDocumentList = rpoContext.JobViolationDocuments
                .Where(x => x.IdJobViolation == idJobViolation).ToList();

            var response = Request.CreateResponse<List<JobViolationDocument>>(HttpStatusCode.OK, jobViolationDocumentList);
            return response;
        }
    }
}