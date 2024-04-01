// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-21-2018
// ***********************************************************************
// <copyright file="TaskDocumentsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Task Documents Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.TaskDocuments
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

    public class TaskDocumentsController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Parameter 1 : DeletedDocumentIds (String)
        /// Parameter 2 : idTask (String)
        /// Parameter 3 : Document files want to Upload (File)
        /// </summary>
        /// <returns>create a new task document.</returns>
        /// <exception cref="System.Web.Http.HttpResponseException"></exception>
        [Authorize]
        [RpoAuthorize]
        [Route("api/taskDocuments/document")]
        [ResponseType(typeof(EmployeeDocument))]
        public async Task<HttpResponseMessage> PutTaskDocuments()
        {

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            System.Collections.Specialized.NameValueCollection formData = provider.FormData;
            IList<HttpContent> files = provider.Files;
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            int idTask = Convert.ToInt32(formData["idTask"]);

            string deletedDocumentIds = Convert.ToString(formData["deletedDocumentIds"]);

            if (!string.IsNullOrEmpty(deletedDocumentIds))
            {
                foreach (var item in deletedDocumentIds.Split(','))
                {
                    int taskDocumentId = Convert.ToInt32(item);
                    TaskDocument taskDocument = rpoContext.TaskDocuments.Where(x => x.Id == taskDocumentId).FirstOrDefault();
                    if (taskDocument != null)
                    {
                        rpoContext.TaskDocuments.Remove(taskDocument);
                        var path = HttpRuntime.AppDomainAppPath;
                        string directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.TaskDocumentPath));
                        string directoryDelete = Convert.ToString(taskDocument.Id) + "_" + taskDocument.DocumentPath;
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

                TaskDocument taskDocument = new TaskDocument();
                taskDocument.DocumentPath = thisFileName;
                taskDocument.IdTask = idTask;
                taskDocument.Name = thisFileName;
                rpoContext.TaskDocuments.Add(taskDocument);
                rpoContext.SaveChanges();

                var path = HttpRuntime.AppDomainAppPath;
                directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.TaskDocumentPath));

                string directoryFileName = Convert.ToString(taskDocument.Id) + "_" + thisFileName;
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

            var taskDocumentList = rpoContext.TaskDocuments
                .Where(x => x.IdTask == idTask).ToList();

            var response = Request.CreateResponse<List<TaskDocument>>(HttpStatusCode.OK, taskDocumentList);
            return response;
        }
    }
}