// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-19-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-19-2018
// ***********************************************************************
// <copyright file="JobTransmittalsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

/// <summary>
/// The Job Transmittals namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobTransmittals
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using DataTable;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using iTextSharp;
    using Model.Models.Enums;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using System.Text;
    using Filters;
    using System.Data.Entity;
    using iTextSharp.tool.xml;
    using iTextSharp.text.html.simpleparser;/// <summary>
                                            /// Class Job Transmittals Controller.
                                            /// </summary>
                                            /// <seealso cref="System.Web.Http.ApiController" />
    public class JobTransmittalsController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();

        #region API's

        /// <summary>
        /// Gets the Job transmital list.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the Job transmital list.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetJobTransmittals([FromUri] JobTransmittalDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewTransmittals)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddTransmittals)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteTransmittals))
            {
                var jobTransmittals = rpoContext.JobTransmittals.Include("FromEmployee")
                                                    .Include("ToCompany")
                                                    .Include("Task")
                                                    .Include("ChecklistItem")/* As part of Update Database Columns for checklist*/
                                                    .Include("ContactAttention")
                                                    .Include("TransmissionType")
                                                    .Include("EmailType")
                                                    .Include("SentBy")
                                                    .Include("CreatedByEmployee")
                                                    .Include("LastModifiedByEmployee").AsQueryable();

                if (dataTableParameters.IdJob != null && dataTableParameters.IdJob > 0)
                {
                    jobTransmittals = jobTransmittals.Where(jc => jc.IdJob == dataTableParameters.IdJob).AsQueryable();
                }

                var recordsTotal = jobTransmittals.Count();
                var recordsFiltered = recordsTotal;

                var result = jobTransmittals
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
        /// Gets the job transmittal.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the detail Job transmital.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobTransmittalDetail))]
        public IHttpActionResult GetJobTransmittal(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewTransmittals)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddTransmittals)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteTransmittals))
            {
                JobTransmittal jobTransmittal = rpoContext.JobTransmittals
                                                    .Include("FromEmployee")
                                                    .Include("ToCompany")
                                                    .Include("ContactAttention")
                                                    .Include("TransmissionType")
                                                    .Include("EmailType")
                                                    .Include("SentBy")
                                                    .Include("Task")
                                                    .Include("ChecklistItem")/* As part of Update Database Columns for checklist*/
                                                    .Include("JobTransmittalCCs.Contact")
                                                    .Include("JobTransmittalCCs.Employee")
                                                    .Include("JobTransmittalAttachments")
                                                    .Include("JobTransmittalJobDocuments.JobDocument.DocumentMaster")
                                                    .Include("JobTransmittalJobDocuments.JobTransmittal")
                                                    .Include("CreatedByEmployee")
                                                    .Include("LastModifiedByEmployee")
                                                    .FirstOrDefault(x => x.Id == id);

                if (jobTransmittal == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(jobTransmittal));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="emailDTO">The email dto.</param>
        /// <returns>Create and send trenamital.</returns>
        /// <exception cref="RpoBusinessException">
        /// Contact Id not found
        /// or
        /// or
        /// Contact Id not found
        /// or
        /// or
        /// Contact Id not found
        /// or
        /// or   
        /// Contact Id not found
        /// or
        /// </exception>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobtransmittals/{id}/email/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult SendEmail(int id, [FromBody] JobTransmittalEmailDTO emailDTO)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddTransmittals))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Job job = rpoContext.Jobs.Find(id);

                if (job == null)
                {
                    return this.NotFound();
                }

                job.LastModiefiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    job.LastModifiedBy = employee.Id;
                }

                int responseIdJob = 0;
                int responseIdJobTransmittal = 0;

                using (DbContextTransaction transaction = rpoContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (emailDTO.IsAdditionalAtttachment)
                        {
                            #region TransmitalCC
                            var cc = new List<KeyValuePair<string, string>>();
                            List<TransmittalCC> transmittalCCList = new List<TransmittalCC>();

                            if (emailDTO.TransmittalCCs != null)
                            {
                                foreach (var item in emailDTO.TransmittalCCs)
                                {
                                    string[] transmittalCC = item.Split('_');
                                    if (transmittalCC != null && transmittalCC.Count() > 2)
                                    {
                                        TransmittalCC transmittal = new TransmittalCC();
                                        transmittal.Id = Convert.ToInt32(transmittalCC[2]);
                                        transmittal.IdGroup = Convert.ToInt32(transmittalCC[0]);
                                        if (transmittalCC[1] == "C")
                                        {
                                            transmittal.IsContact = true;
                                        }
                                        else if (transmittalCC[1] == "E")
                                        {
                                            transmittal.IsContact = false;
                                        }

                                        transmittalCCList.Add(transmittal);
                                    }

                                }
                            }
                            #endregion

                            #region ProjectTeam
                            List<int> projectTeam = transmittalCCList != null ? transmittalCCList.Where(x => x.IsContact == false).Select(x => x.Id).ToList() : new List<int>();
                            List<int> contactTeam = transmittalCCList != null ? transmittalCCList.Where(x => x.IsContact == true).Select(x => x.Id).ToList() : new List<int>();

                            foreach (var dest in contactTeam.Select(c => rpoContext.Contacts.Find(c)))
                            {
                                if (dest == null)
                                {
                                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Contact Id not found." }));
                                }
                                if (dest.Email == null || string.IsNullOrEmpty(dest.Email))
                                {
                                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = $"Contact {dest.FirstName} does not have a valid e-mail address." }));
                                }
                                if (dest.IsActive == false)
                                {
                                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Transmittal can not send to inactive contact." }));
                                }
                                cc.Add(new KeyValuePair<string, string>(dest.Email, dest.FirstName + " " + dest.LastName));
                            }

                            foreach (var dest in projectTeam.Select(c => rpoContext.Employees.Find(c)))
                            {
                                if (dest == null)
                                {
                                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Employee Id not found." }));
                                }
                                if (dest.Email == null || string.IsNullOrEmpty(dest.Email))
                                {
                                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = $"Employee {dest.FirstName} does not have a valid e-mail address." }));
                                }
                                cc.Add(new KeyValuePair<string, string>(dest.Email, dest.FirstName + " " + dest.LastName));
                            }

                            var to = new List<KeyValuePair<string, string>>();
                            if (emailDTO.IsDraft == null || !Convert.ToBoolean(emailDTO.IsDraft))
                            {
                                var contactTo = rpoContext.Contacts.Where(x => x.Id == emailDTO.IdContactAttention).FirstOrDefault();
                                if (contactTo == null)
                                {
                                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Contact Id not found." }));
                                }
                                if (contactTo.Email == null || string.IsNullOrEmpty(contactTo.Email))
                                {
                                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = $"Contact {contactTo.FirstName} not has a valid e-mail" }));
                                }
                                if (contactTo.IsActive == false)
                                {
                                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Transmittal can not send to inactive contact." }));
                                }
                                to.Add(new KeyValuePair<string, string>(contactTo.Email, contactTo.FirstName + " " + contactTo.LastName));
                            }
                            #endregion

                            #region CreateTransmittal
                            JobTransmittal jobTransmittal = new JobTransmittal();
                            jobTransmittal.IdJob = job.Id;
                            jobTransmittal.IdFromEmployee = emailDTO.IdFromEmployee;
                            jobTransmittal.IdToCompany = emailDTO.IdContactsTo;
                            jobTransmittal.IdContactAttention = emailDTO.IdContactAttention;
                            jobTransmittal.IdTransmissionType = emailDTO.IdTransmissionType;
                            jobTransmittal.IdEmailType = emailDTO.IdEmailType;
                            jobTransmittal.EmailSubject = emailDTO.Subject;
                            jobTransmittal.IdTask = emailDTO.IdTask;
                            jobTransmittal.IdChecklistItem = emailDTO.IdChecklistItem;/* As part of Update Database Columns for checklist*/
                            jobTransmittal.SentDate = DateTime.UtcNow;
                            if (employee != null)
                            {
                                jobTransmittal.IdSentBy = employee.Id;
                            }
                            jobTransmittal.IsEmailSent = false;
                            jobTransmittal.EmailMessage = emailDTO.Body;
                            jobTransmittal.IsAdditionalAtttachment = emailDTO.IsAdditionalAtttachment;

                            rpoContext.JobTransmittals.Add(jobTransmittal);
                            rpoContext.SaveChanges();
                            #endregion

                            #region JobTransmitalDcoument
                            try
                            {
                                if (emailDTO.JobTransmittalJobDocuments != null && emailDTO.JobTransmittalJobDocuments.Count > 0)
                                {
                                    foreach (JobTransmittalJobDocument item in emailDTO.JobTransmittalJobDocuments)
                                    {
                                        JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == item.IdJobDocument);
                                        JobTransmittalJobDocument jobTransmittalJobDocument = rpoContext.JobTransmittalJobDocuments.FirstOrDefault(x => x.Id == item.Id);
                                        if (item.Id > 0 && jobTransmittalJobDocument != null && emailDTO.IsResend == false && emailDTO.IsRevise == false)
                                        {
                                            jobTransmittalJobDocument.IdJobDocument = item.IdJobDocument;
                                            jobTransmittalJobDocument.IdJobTransmittal = jobTransmittal.Id;
                                            jobTransmittalJobDocument.Copies = item.Copies;
                                            jobTransmittalJobDocument.DocumentPath = jobDocument.DocumentPath;
                                            rpoContext.SaveChanges();

                                            var path = HttpRuntime.AppDomainAppPath;

                                            string transmittalDirectoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.TransmittalJobDocumentPath));
                                            string transmittalDeleteFileName = System.IO.Path.Combine(transmittalDirectoryName, jobTransmittalJobDocument.IdJobTransmittal + "_" + jobTransmittalJobDocument.IdJobDocument + "_" + jobTransmittalJobDocument.DocumentPath);
                                            string transmittalFileName = System.IO.Path.Combine(transmittalDirectoryName, jobTransmittalJobDocument.IdJobTransmittal + "_" + jobTransmittalJobDocument.IdJobDocument + "_" + jobTransmittalJobDocument.DocumentPath);

                                            string returnfile = DowanloadJobdocument(jobDocument);

                                            if (!Directory.Exists(transmittalDirectoryName))
                                            {
                                                Directory.CreateDirectory(transmittalDirectoryName);
                                            }
                                            if (System.IO.File.Exists(transmittalDeleteFileName))
                                            {
                                                System.IO.File.Delete(transmittalDeleteFileName);
                                            }
                                            if (File.Exists(returnfile) && !File.Exists(transmittalFileName))
                                            {
                                                File.Copy(returnfile, transmittalFileName);
                                            }
                                        }
                                        else
                                        {
                                            JobTransmittalJobDocument jobTransmittalJobDocumentnw = new JobTransmittalJobDocument();
                                            jobTransmittalJobDocumentnw.IdJobDocument = item.IdJobDocument;
                                            jobTransmittalJobDocumentnw.IdJobTransmittal = jobTransmittal.Id;
                                            jobTransmittalJobDocumentnw.Copies = item.Copies;
                                            jobTransmittalJobDocumentnw.DocumentPath = jobDocument.DocumentPath;
                                            rpoContext.JobTransmittalJobDocuments.Add(jobTransmittalJobDocumentnw);
                                            rpoContext.SaveChanges();

                                            var path = HttpRuntime.AppDomainAppPath;

                                            string transmittalDirectoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.TransmittalJobDocumentPath));
                                            string transmittalFileName = System.IO.Path.Combine(transmittalDirectoryName, jobTransmittalJobDocumentnw.IdJobTransmittal + "_" + jobTransmittalJobDocumentnw.IdJobDocument + "_" + jobTransmittalJobDocumentnw.DocumentPath);

                                            string returnfile = DowanloadJobdocument(jobDocument);

                                            if (!Directory.Exists(transmittalDirectoryName))
                                            {
                                                Directory.CreateDirectory(transmittalDirectoryName);
                                            }
                                            if (File.Exists(returnfile) && !File.Exists(transmittalFileName))
                                            {
                                                File.Copy(returnfile, transmittalFileName);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                TransmittalLog(ex);
                                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Unable to attach documents to the email. Please try again." }));
                            }
                            #endregion

                            #region ExporttoExcelpdf
                            if (!string.IsNullOrEmpty(emailDTO.ReportDocumentName))
                            {
                                JobTransmittalAttachment jobTransmittalAttachment = new JobTransmittalAttachment();
                                jobTransmittalAttachment.DocumentPath = emailDTO.ReportDocumentName;
                                jobTransmittalAttachment.IdJobTransmittal = jobTransmittal.Id;
                                jobTransmittalAttachment.Name = emailDTO.ReportDocumentName;
                                rpoContext.JobTransmittalAttachments.Add(jobTransmittalAttachment);
                                rpoContext.SaveChanges();

                                var path = HttpRuntime.AppDomainAppPath;
                                string directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobTrasmittalAttachmentsPath));

                                string directoryFileName = Convert.ToString(jobTransmittalAttachment.IdJobTransmittal) + "_" + Convert.ToString(jobTransmittalAttachment.Id) + "_" + emailDTO.ReportDocumentName;
                                string filename = System.IO.Path.Combine(directoryName, directoryFileName);

                                if (!Directory.Exists(directoryName))
                                {
                                    Directory.CreateDirectory(directoryName);
                                }

                                if (File.Exists(emailDTO.ReportDocumentPath))
                                {
                                    File.Copy(emailDTO.ReportDocumentPath, filename);
                                }
                            }
                            #endregion


                            #region Revice
                            if (emailDTO.IsRevise)
                            {
                                if (emailDTO.JobTransmittalAttachments != null && emailDTO.JobTransmittalAttachments.Any())
                                {
                                    foreach (JobTransmittalAttachment item in emailDTO.JobTransmittalAttachments)
                                    {
                                        JobTransmittalAttachment attachment = rpoContext.JobTransmittalAttachments.FirstOrDefault(x => x.Id == item.Id);
                                        JobTransmittalAttachment jobTransmittalAttachment = new JobTransmittalAttachment();
                                        jobTransmittalAttachment.DocumentPath = attachment.DocumentPath;
                                        jobTransmittalAttachment.IdJobTransmittal = jobTransmittal.Id;
                                        jobTransmittalAttachment.Name = attachment.Name;
                                        rpoContext.JobTransmittalAttachments.Add(jobTransmittalAttachment);
                                        rpoContext.SaveChanges();

                                        var path = HttpRuntime.AppDomainAppPath;
                                        string directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobTrasmittalAttachmentsPath));
                                        string docpath = string.Empty;
                                        if (attachment.Name.Contains("%23"))
                                        {
                                            docpath = Uri.UnescapeDataString(attachment.Name);
                                        }
                                        else
                                        {
                                            docpath = attachment.Name;
                                        }
                                        // string originalDirectory = Convert.ToString(attachment.IdJobTransmittal) + "_" + Convert.ToString(attachment.Id) + "_" + docpath; 
                                        string originalDirectory = Convert.ToString(attachment.IdJobTransmittal) + "_" + Convert.ToString(attachment.Id) + "_" + docpath;

                                        string originalFileName = System.IO.Path.Combine(directoryName, originalDirectory);

                                        // string directoryFileName = Convert.ToString(jobTransmittalAttachment.IdJobTransmittal) + "_" + Convert.ToString(jobTransmittalAttachment.Id) + "_" + docpath;
                                        string directoryFileName = Convert.ToString(jobTransmittalAttachment.IdJobTransmittal) + "_" + Convert.ToString(jobTransmittalAttachment.Id) + "_" + docpath;
                                        string filename = System.IO.Path.Combine(directoryName, directoryFileName);

                                        if (!Directory.Exists(directoryName))
                                        {
                                            Directory.CreateDirectory(directoryName);
                                        }

                                        if (File.Exists(originalFileName))
                                        {
                                            File.Copy(originalFileName, filename);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region Resend
                            if (emailDTO.IsResend)
                            {
                                string transmittalNumber = rpoContext.JobTransmittals.Where(x => x.Id == emailDTO.IdOldTrasmittal).Select(x => x.TransmittalNumber).FirstOrDefault();
                                jobTransmittal.TransmittalNumber = transmittalNumber;

                                if (emailDTO.JobTransmittalAttachments != null && emailDTO.JobTransmittalAttachments.Any())
                                {
                                    foreach (JobTransmittalAttachment item in emailDTO.JobTransmittalAttachments)
                                    {
                                        JobTransmittalAttachment attachment = rpoContext.JobTransmittalAttachments.FirstOrDefault(x => x.Id == item.Id);
                                        JobTransmittalAttachment jobTransmittalAttachment = new JobTransmittalAttachment();
                                        jobTransmittalAttachment.DocumentPath = attachment.DocumentPath;
                                        jobTransmittalAttachment.IdJobTransmittal = jobTransmittal.Id;
                                        jobTransmittalAttachment.Name = attachment.Name;
                                        rpoContext.JobTransmittalAttachments.Add(jobTransmittalAttachment);
                                        rpoContext.SaveChanges();

                                        var path = HttpRuntime.AppDomainAppPath;
                                        string directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobTrasmittalAttachmentsPath));
                                        string docpath = string.Empty;
                                        if (attachment.Name.Contains("%23"))
                                        {
                                            docpath = Uri.UnescapeDataString(attachment.Name);
                                        }
                                        else
                                        {
                                            docpath = attachment.Name;
                                        }
                                        // string originalDirectory = Convert.ToString(attachment.IdJobTransmittal) + "_" + Convert.ToString(attachment.Id) + "_" + docpath; 
                                        string originalDirectory = Convert.ToString(attachment.IdJobTransmittal) + "_" + Convert.ToString(attachment.Id) + "_" + docpath;
                                        string originalFileName = System.IO.Path.Combine(directoryName, originalDirectory);

                                        //  string directoryFileName = Convert.ToString(jobTransmittalAttachment.IdJobTransmittal) + "_" + Convert.ToString(jobTransmittalAttachment.Id) + "_" + docpath; 
                                        string directoryFileName = Convert.ToString(jobTransmittalAttachment.IdJobTransmittal) + "_" + Convert.ToString(jobTransmittalAttachment.Id) + "_" + docpath;
                                        string filename = System.IO.Path.Combine(directoryName, directoryFileName);

                                        if (!Directory.Exists(directoryName))
                                        {
                                            Directory.CreateDirectory(directoryName);
                                        }

                                        if (File.Exists(originalFileName))
                                        {
                                            File.Copy(originalFileName, filename);
                                        }
                                    }
                                }

                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                jobTransmittal.TransmittalNumber = Convert.ToString(jobTransmittal.Id);
                                rpoContext.SaveChanges();
                            }

                            #endregion

                            #region transmittalCC
                            foreach (TransmittalCC transmittalCC in transmittalCCList)
                            {
                                JobTransmittalCC jobTransmittalCC = new JobTransmittalCC();
                                if (transmittalCC.IsContact)
                                {
                                    jobTransmittalCC.IdContact = transmittalCC.Id;
                                }
                                else
                                {
                                    jobTransmittalCC.IdEmployee = transmittalCC.Id;
                                }

                                jobTransmittalCC.IdJobTransmittal = jobTransmittal.Id;
                                jobTransmittalCC.IdGroup = transmittalCC.IdGroup;

                                rpoContext.JobTransmittalCCs.Add(jobTransmittalCC);
                                rpoContext.SaveChanges();
                            }
                            #endregion

                            responseIdJob = job.Id;
                            responseIdJobTransmittal = jobTransmittal.Id;

                        }
                        else
                        {
                            #region TransmitalCC
                            var cc = new List<KeyValuePair<string, string>>();

                            List<TransmittalCC> transmittalCCList = new List<TransmittalCC>();

                            if (emailDTO.TransmittalCCs != null)
                            {
                                foreach (var item in emailDTO.TransmittalCCs)
                                {
                                    string[] transmittalCC = item.Split('_');
                                    if (transmittalCC != null && transmittalCC.Count() > 2)
                                    {
                                        TransmittalCC transmittal = new TransmittalCC();
                                        transmittal.Id = Convert.ToInt32(transmittalCC[2]);
                                        transmittal.IdGroup = Convert.ToInt32(transmittalCC[0]);
                                        if (transmittalCC[1] == "C")
                                        {
                                            transmittal.IsContact = true;
                                        }
                                        else if (transmittalCC[1] == "E")
                                        {
                                            transmittal.IsContact = false;
                                        }

                                        transmittalCCList.Add(transmittal);
                                    }

                                }
                            }
                            #endregion

                            #region ProjectTeam
                            List<int> projectTeam = transmittalCCList != null ? transmittalCCList.Where(x => x.IsContact == false).Select(x => x.Id).ToList() : new List<int>();
                            List<int> contactTeam = transmittalCCList != null ? transmittalCCList.Where(x => x.IsContact == true).Select(x => x.Id).ToList() : new List<int>();

                            foreach (var dest in contactTeam.Select(c => rpoContext.Contacts.Find(c)))
                            {
                                if (dest == null)
                                {
                                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Contact Id not found." }));
                                }

                                if (dest.Email == null || string.IsNullOrEmpty(dest.Email))
                                {
                                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = $"Contact {dest.FirstName} does not have a valid e-mail address." }));
                                }

                                if (dest.IsActive == false)
                                {
                                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Transmittal can not send to inactive contact." }));
                                }
                                cc.Add(new KeyValuePair<string, string>(dest.Email, dest.FirstName + " " + dest.LastName));
                            }

                            foreach (var dest in projectTeam.Select(c => rpoContext.Employees.Find(c)))
                            {
                                if (dest == null)
                                {
                                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Employee Id not found." }));
                                }
                                if (dest.Email == null || string.IsNullOrEmpty(dest.Email))
                                {
                                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = $"Employee {dest.FirstName} does not have a valid e-mail address." }));
                                }
                                cc.Add(new KeyValuePair<string, string>(dest.Email, dest.FirstName + " " + dest.LastName));
                            }

                            var to = new List<KeyValuePair<string, string>>();
                            if (emailDTO.IsDraft == null || !Convert.ToBoolean(emailDTO.IsDraft))
                            {
                                var contactTo = rpoContext.Contacts.Where(x => x.Id == emailDTO.IdContactAttention).FirstOrDefault();
                                if (contactTo == null)
                                {
                                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Contact Id not found." }));
                                }
                                if (contactTo.Email == null || string.IsNullOrEmpty(contactTo.Email))
                                {
                                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = $"Contact {contactTo.FirstName} not has a valid e-mail" }));
                                }
                                if (contactTo.IsActive == false)
                                {
                                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Transmittal can not send to inactive contact." }));
                                }
                                to.Add(new KeyValuePair<string, string>(contactTo.Email, contactTo.FirstName + " " + contactTo.LastName));
                            }
                            #endregion

                            var attachments = new List<string>();

                            Employee employeeFrom = rpoContext.Employees.Find(emailDTO.IdFromEmployee);

                            cc.Add(new KeyValuePair<string, string>(employeeFrom.Email, employeeFrom.FirstName + " " + employeeFrom.LastName));

                            #region CreateTransmittal
                            JobTransmittal jobTransmittal = new JobTransmittal();
                            jobTransmittal.IdJob = job.Id;
                            jobTransmittal.IdFromEmployee = emailDTO.IdFromEmployee;
                            jobTransmittal.IdToCompany = emailDTO.IdContactsTo;
                            jobTransmittal.IdContactAttention = emailDTO.IdContactAttention;
                            jobTransmittal.IdTransmissionType = emailDTO.IdTransmissionType;
                            jobTransmittal.IdEmailType = emailDTO.IdEmailType;
                            jobTransmittal.EmailSubject = emailDTO.Subject;
                            jobTransmittal.IdTask = emailDTO.IdTask;
                            //jobTransmittal.IdChecklist = emailDTO.IdChecklist;

                            jobTransmittal.SentDate = DateTime.UtcNow;
                            if (employee != null)
                            {
                                jobTransmittal.IdSentBy = employee.Id;
                            }
                            jobTransmittal.EmailMessage = emailDTO.Body;
                            jobTransmittal.IsAdditionalAtttachment = emailDTO.IsAdditionalAtttachment;
                            jobTransmittal.IsEmailSent = true;

                            rpoContext.JobTransmittals.Add(jobTransmittal);
                            rpoContext.SaveChanges();
                            #endregion

                            #region JobTransmitalDcouments
                            try
                            {
                                if (emailDTO.JobTransmittalJobDocuments != null && emailDTO.JobTransmittalJobDocuments.Count > 0)
                                {
                                    foreach (JobTransmittalJobDocument item in emailDTO.JobTransmittalJobDocuments)
                                    {
                                        JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == item.IdJobDocument);
                                        JobTransmittalJobDocument jobTransmittalJobDocument = rpoContext.JobTransmittalJobDocuments.FirstOrDefault(x => x.Id == item.Id);
                                        if (item.Id > 0 && jobTransmittalJobDocument != null && emailDTO.IsRevise == false && emailDTO.IsResend == false)
                                        {
                                            jobTransmittalJobDocument.IdJobDocument = item.IdJobDocument;
                                            jobTransmittalJobDocument.IdJobTransmittal = jobTransmittal.Id;
                                            jobTransmittalJobDocument.Copies = item.Copies;
                                            jobTransmittalJobDocument.DocumentPath = jobDocument.DocumentPath;
                                            rpoContext.SaveChanges();
                                            var path = HttpRuntime.AppDomainAppPath;

                                            string transmittalDirectoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.TransmittalJobDocumentPath));
                                            string transmittalDeleteFileName = System.IO.Path.Combine(transmittalDirectoryName, jobTransmittalJobDocument.IdJobTransmittal + "_" + jobTransmittalJobDocument.IdJobDocument + "_" + jobTransmittalJobDocument.DocumentPath);
                                            string transmittalFileName = System.IO.Path.Combine(transmittalDirectoryName, jobTransmittalJobDocument.IdJobTransmittal + "_" + jobTransmittalJobDocument.IdJobDocument + "_" + jobTransmittalJobDocument.DocumentPath);

                                            string jobdocumentFileName = string.Empty;

                                            if (jobDocument.DocumentName.Trim().ToLower() == "dot permit")
                                            {
                                                JobDocumentAttachment jobJobDocumentAttach = rpoContext.JobDocumentAttachments.FirstOrDefault(x => x.IdJobDocument == jobDocument.Id);
                                                if (jobJobDocumentAttach != null)
                                                {
                                                    string dirpath = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentAttachmentPath));
                                                    jobdocumentFileName = System.IO.Path.Combine(dirpath, jobJobDocumentAttach.Id + "_" + jobJobDocumentAttach.Path);
                                                }

                                            }
                                            else
                                            {
                                                string jobdocumentDirectoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentExportPath));
                                                jobdocumentDirectoryName = System.IO.Path.Combine(jobdocumentDirectoryName, Convert.ToString(jobDocument.IdJob));
                                                jobdocumentFileName = System.IO.Path.Combine(jobdocumentDirectoryName, jobDocument.Id + "_" + jobDocument.DocumentPath);
                                            }

                                            string returnfile = DowanloadJobdocument(jobDocument);

                                            if (!Directory.Exists(transmittalDirectoryName))
                                            {
                                                Directory.CreateDirectory(transmittalDirectoryName);
                                            }
                                            if (System.IO.File.Exists(transmittalDeleteFileName))
                                            {
                                                System.IO.File.Delete(transmittalDeleteFileName);
                                            }
                                            if (File.Exists(returnfile) && !File.Exists(transmittalFileName))
                                            {
                                                File.Copy(returnfile, transmittalFileName);
                                            }
                                        }
                                        else
                                        {
                                            JobTransmittalJobDocument jobTransmittalJobDocumentAdd = new JobTransmittalJobDocument();
                                            jobTransmittalJobDocumentAdd.IdJobDocument = item.IdJobDocument;
                                            jobTransmittalJobDocumentAdd.IdJobTransmittal = jobTransmittal.Id;
                                            jobTransmittalJobDocumentAdd.Copies = item.Copies;
                                            jobTransmittalJobDocumentAdd.DocumentPath = jobDocument.DocumentPath;
                                            rpoContext.JobTransmittalJobDocuments.Add(jobTransmittalJobDocumentAdd);
                                            rpoContext.SaveChanges();

                                            var path = HttpRuntime.AppDomainAppPath;

                                            string transmittalDirectoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.TransmittalJobDocumentPath));
                                            string transmittalFileName = System.IO.Path.Combine(transmittalDirectoryName, jobTransmittalJobDocumentAdd.IdJobTransmittal + "_" + jobTransmittalJobDocumentAdd.IdJobDocument + "_" + jobTransmittalJobDocumentAdd.DocumentPath);


                                            string jobdocumentFileName = string.Empty;

                                            if (jobDocument.DocumentName.Trim().ToLower() == "dot permit")
                                            {
                                                JobDocumentAttachment jobJobDocumentAttach = rpoContext.JobDocumentAttachments.FirstOrDefault(x => x.IdJobDocument == jobDocument.Id);
                                                if (jobJobDocumentAttach != null)
                                                {
                                                    string dirpath = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentAttachmentPath));
                                                    jobdocumentFileName = System.IO.Path.Combine(dirpath, jobJobDocumentAttach.Id + "_" + jobJobDocumentAttach.Path);
                                                }

                                            }
                                            else
                                            {
                                                string jobdocumentDirectoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentExportPath));
                                                jobdocumentDirectoryName = System.IO.Path.Combine(jobdocumentDirectoryName, Convert.ToString(jobDocument.IdJob));
                                                jobdocumentFileName = System.IO.Path.Combine(jobdocumentDirectoryName, jobDocument.Id + "_" + jobDocument.DocumentPath);
                                            }
                                            PdfReader.unethicalreading = true;

                                            string returnfile = DowanloadJobdocument(jobDocument);

                                            if (!Directory.Exists(transmittalDirectoryName))
                                            {
                                                Directory.CreateDirectory(transmittalDirectoryName);
                                            }

                                            if (File.Exists(returnfile) && !File.Exists(transmittalFileName))
                                            {
                                                File.Copy(returnfile, transmittalFileName);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Unable to attach documents to the email. Please try again." }));
                                //  return Ok(new { Message = "Unable to download the Pdf.so please try again.", idJob = responseIdJob.ToString(), idJobTransmittal = responseIdJobTransmittal.ToString() });
                                // throw ex;
                            }
                            #endregion

                            #region ExporttoExcelpdf
                            if (!string.IsNullOrEmpty(emailDTO.ReportDocumentName))
                            {
                                JobTransmittalAttachment jobTransmittalAttachment = new JobTransmittalAttachment();
                                jobTransmittalAttachment.DocumentPath = emailDTO.ReportDocumentName;
                                jobTransmittalAttachment.IdJobTransmittal = jobTransmittal.Id;
                                jobTransmittalAttachment.Name = emailDTO.ReportDocumentName;
                                rpoContext.JobTransmittalAttachments.Add(jobTransmittalAttachment);
                                rpoContext.SaveChanges();

                                var path = HttpRuntime.AppDomainAppPath;
                                string directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobTrasmittalAttachmentsPath));

                                string directoryFileName = Convert.ToString(jobTransmittalAttachment.IdJobTransmittal) + "_" + Convert.ToString(jobTransmittalAttachment.Id) + "_" + emailDTO.ReportDocumentName;
                                string filename = System.IO.Path.Combine(directoryName, directoryFileName);

                                if (!Directory.Exists(directoryName))
                                {
                                    Directory.CreateDirectory(directoryName);
                                }

                                if (File.Exists(emailDTO.ReportDocumentPath))
                                {
                                    File.Copy(emailDTO.ReportDocumentPath, filename);
                                }
                            }
                            #endregion


                            #region ReviceAttachment
                            if (emailDTO.IsRevise)
                            {
                                if (emailDTO.JobTransmittalAttachments != null && emailDTO.JobTransmittalAttachments.Any())
                                {
                                    foreach (JobTransmittalAttachment item in emailDTO.JobTransmittalAttachments)
                                    {
                                        JobTransmittalAttachment attachment = rpoContext.JobTransmittalAttachments.FirstOrDefault(x => x.Id == item.Id);
                                        JobTransmittalAttachment jobTransmittalAttachment = new JobTransmittalAttachment();
                                        jobTransmittalAttachment.DocumentPath = attachment.DocumentPath;
                                        jobTransmittalAttachment.IdJobTransmittal = jobTransmittal.Id;
                                        jobTransmittalAttachment.Name = attachment.Name;
                                        rpoContext.JobTransmittalAttachments.Add(jobTransmittalAttachment);
                                        rpoContext.SaveChanges();

                                        var path = HttpRuntime.AppDomainAppPath;
                                        string directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobTrasmittalAttachmentsPath));

                                        string docpath = string.Empty;
                                        if (attachment.Name.Contains("%23"))
                                        {
                                            docpath = Uri.UnescapeDataString(attachment.Name);
                                        }
                                        else
                                        {
                                            docpath = attachment.Name;
                                        }

                                        //string originaldirectory = Convert.ToString(attachment.IdJobTransmittal) + "_" + Convert.ToString(attachment.Id) + "_" + docpath; 
                                        string originaldirectory = Convert.ToString(attachment.IdJobTransmittal) + "_" + Convert.ToString(attachment.Id) + "_" + docpath;
                                        string originalFileName = System.IO.Path.Combine(directoryName, originaldirectory);

                                        //string directoryFileName = Convert.ToString(jobTransmittalAttachment.IdJobTransmittal) + "_" + Convert.ToString(jobTransmittalAttachment.Id) + "_" + docpath;
                                        string directoryFileName = Convert.ToString(jobTransmittalAttachment.IdJobTransmittal) + "_" + Convert.ToString(jobTransmittalAttachment.Id) + "_" + docpath;
                                        string filename = System.IO.Path.Combine(directoryName, directoryFileName);

                                        if (!Directory.Exists(directoryName))
                                        {
                                            Directory.CreateDirectory(directoryName);
                                        }

                                        if (File.Exists(originalFileName))
                                        {
                                            File.Copy(originalFileName, filename);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region ResendDocument
                            if (emailDTO.IsResend)
                            {
                                string transmittalNumber = rpoContext.JobTransmittals.Where(x => x.Id == emailDTO.IdOldTrasmittal).Select(x => x.TransmittalNumber).FirstOrDefault();
                                jobTransmittal.TransmittalNumber = transmittalNumber;

                                if (emailDTO.JobTransmittalAttachments != null && emailDTO.JobTransmittalAttachments.Any())
                                {
                                    foreach (JobTransmittalAttachment item in emailDTO.JobTransmittalAttachments)
                                    {
                                        JobTransmittalAttachment attachment = rpoContext.JobTransmittalAttachments.FirstOrDefault(x => x.Id == item.Id);
                                        JobTransmittalAttachment jobTransmittalAttachment = new JobTransmittalAttachment();
                                        jobTransmittalAttachment.DocumentPath = attachment.DocumentPath;
                                        jobTransmittalAttachment.IdJobTransmittal = jobTransmittal.Id;
                                        jobTransmittalAttachment.Name = attachment.Name;
                                        rpoContext.JobTransmittalAttachments.Add(jobTransmittalAttachment);
                                        rpoContext.SaveChanges();

                                        var path = HttpRuntime.AppDomainAppPath;
                                        string directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobTrasmittalAttachmentsPath));

                                        string docpath = string.Empty;
                                        if (attachment.Name.Contains("%23"))
                                        {
                                            docpath = Uri.UnescapeDataString(attachment.Name);
                                        }
                                        else
                                        {
                                            docpath = attachment.Name;
                                        }

                                        string originaldirectory = Convert.ToString(attachment.IdJobTransmittal) + "_" + Convert.ToString(attachment.Id) + "_" + docpath;
                                        //string originaldirectory = Convert.ToString(attachment.IdJobTransmittal) + "_" + Convert.ToString(attachment.Id) + "_" + docpath;
                                        string originalFileName = System.IO.Path.Combine(directoryName, originaldirectory);

                                        string directoryFileName = Convert.ToString(jobTransmittalAttachment.IdJobTransmittal) + "_" + Convert.ToString(jobTransmittalAttachment.Id) + "_" + docpath;
                                        //string directoryFileName = Convert.ToString(jobTransmittalAttachment.IdJobTransmittal) + "_" + Convert.ToString(jobTransmittalAttachment.Id) + "_" + docpath;
                                        string filename = System.IO.Path.Combine(directoryName, directoryFileName);

                                        if (!Directory.Exists(directoryName))
                                        {
                                            Directory.CreateDirectory(directoryName);
                                        }

                                        if (File.Exists(originalFileName))
                                        {
                                            File.Copy(originalFileName, filename);
                                        }
                                    }
                                }
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                jobTransmittal.TransmittalNumber = Convert.ToString(jobTransmittal.Id);
                                rpoContext.SaveChanges();
                            }
                            #endregion

                            #region TransmittalCC
                            foreach (TransmittalCC transmittalCC in transmittalCCList)
                            {
                                JobTransmittalCC jobTransmittalCC = new JobTransmittalCC();
                                if (transmittalCC.IsContact)
                                {
                                    jobTransmittalCC.IdContact = transmittalCC.Id;
                                }
                                else
                                {
                                    jobTransmittalCC.IdEmployee = transmittalCC.Id;
                                }
                                jobTransmittalCC.IdJobTransmittal = jobTransmittal.Id;
                                jobTransmittalCC.IdGroup = transmittalCC.IdGroup;

                                rpoContext.JobTransmittalCCs.Add(jobTransmittalCC);
                                rpoContext.SaveChanges();
                            }
                            #endregion

                            responseIdJob = job.Id;
                            responseIdJobTransmittal = jobTransmittal.Id;

                            jobTransmittal.IsEmailSent = false;
                            rpoContext.SaveChanges();

                            string body = string.Empty;
                            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/MasterEmailTemplate.htm")))
                            {
                                body = reader.ReadToEnd();
                            }

                            #region JobTransmittalJobDocuments
                            var jobTransmittalJobDocuments = rpoContext.JobTransmittalJobDocuments.Where(x => x.IdJobTransmittal == jobTransmittal.Id).ToList();

                            if (jobTransmittalJobDocuments != null && jobTransmittalJobDocuments.Any())
                            {
                                foreach (JobTransmittalJobDocument item in jobTransmittalJobDocuments)
                                {
                                    string filename = string.Empty;
                                    string directoryName = string.Empty;
                                    var path = HttpRuntime.AppDomainAppPath;

                                    directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.TransmittalJobDocumentPath));
                                    string directoryFileName = Convert.ToString(item.IdJobTransmittal) + "_" + Convert.ToString(item.IdJobDocument) + "_" + item.DocumentPath;
                                    filename = System.IO.Path.Combine(directoryName, directoryFileName);

                                    if (File.Exists(filename))
                                    {
                                        attachments.Add(filename);
                                    }
                                }
                            }
                            #endregion

                            #region JobTransmittalAttachments
                            var jobTransmittalAttachments = rpoContext.JobTransmittalAttachments.Where(x => x.IdJobTransmittal == jobTransmittal.Id).ToList();

                            if (jobTransmittalAttachments != null && jobTransmittalAttachments.Any())
                            {
                                foreach (JobTransmittalAttachment item in jobTransmittalAttachments)
                                {
                                    string filename = string.Empty;
                                    string directoryName = string.Empty;
                                    var path = HttpRuntime.AppDomainAppPath;

                                    string docpath = string.Empty;
                                    if (item.Name.Contains("%23"))
                                    {
                                        docpath = Uri.UnescapeDataString(item.Name);
                                    }
                                    else
                                    {
                                        docpath = item.Name;
                                    }

                                    directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobTrasmittalAttachmentsPath));
                                    //string directoryFileName = Convert.ToString(item.IdJobTransmittal) + "_" + Convert.ToString(item.Id) + "_" + item.DocumentPath;
                                    string directoryFileName = Convert.ToString(item.IdJobTransmittal) + "_" + Convert.ToString(item.Id) + "_" + docpath;
                                    filename = System.IO.Path.Combine(directoryName, directoryFileName);

                                    if (File.Exists(filename))
                                    {
                                        attachments.Add(filename);
                                    }
                                }
                            }
                            #endregion

                            #region Draft
                            if (emailDTO.IsDraft != null && Convert.ToBoolean(emailDTO.IsDraft))
                            {
                                jobTransmittal.IsDraft = emailDTO.IsDraft;
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                jobTransmittal.IsDraft = emailDTO.IsDraft;
                                rpoContext.SaveChanges();

                                TransmissionType transmissionType = rpoContext.TransmissionTypes.FirstOrDefault(x => x.Id == jobTransmittal.IdTransmissionType);
                                if (transmissionType != null && transmissionType.IsSendEmail)
                                {
                                    string emailBody = body;
                                    emailBody = emailBody.Replace("##EmailBody##", emailDTO.Body);

                                    //List<TransmissionTypeDefaultCC> transmissionTypeDefaultCC = rpoContext.TransmissionTypeDefaultCCs.Include("Employee").Where(x => x.IdTransmissionType == emailDTO.IdTransmissionType).ToList();
                                    List<TransmissionTypeDefaultCC> transmissionTypeDefaultCC = rpoContext.TransmissionTypeDefaultCCs.Include("Employee").Where(x => x.IdEamilType == emailDTO.IdEmailType).ToList();
                                    if (transmissionTypeDefaultCC != null && transmissionTypeDefaultCC.Any())
                                    {
                                        foreach (TransmissionTypeDefaultCC item in transmissionTypeDefaultCC)
                                        {
                                            cc.Add(new KeyValuePair<string, string>(item.Employee.Email, item.Employee.FirstName + " " + item.Employee.LastName));
                                        }
                                    }
                                    string emailSubject = emailDTO.Subject;

                                    if (jobTransmittal != null && jobTransmittal.Id != 0)
                                    {
                                        emailSubject = emailSubject.Replace("##TransmittalNo##", "- Transmittal #" + jobTransmittal.Id + (jobTransmittal.EmailType != null && !string.IsNullOrEmpty(jobTransmittal.EmailType.Name) ? " | " + jobTransmittal.EmailType.Name : string.Empty));
                                    }
                                    else
                                    {
                                        emailSubject = emailSubject.Replace("##TransmittalNo##", "");
                                    }
                                    #region ProposalAttachment
                                    string ProposalAttach = CreateTransmittalpdfAttachment(job.Id, emailDTO, jobTransmittal.Id);

                                    if (!string.IsNullOrEmpty(ProposalAttach) && File.Exists(ProposalAttach))
                                    {
                                        attachments.Add(ProposalAttach);
                                    }
                                    #endregion
                                    try
                                    {
                                        string statusmail = Mail.SendTransmittal(
                                              new KeyValuePair<string, string>(employeeFrom.Email, employeeFrom.FirstName + " " + employeeFrom.LastName),
                                              to,
                                              cc,
                                             emailSubject,
                                              emailBody,
                                              attachments
                                          );
                                        if (statusmail == "Fail")
                                        {
                                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Unable to send email.please try again." }));
                                            jobTransmittal.IsEmailSent = false;
                                        }
                                        else
                                        {
                                            jobTransmittal.IsEmailSent = true;
                                        }

                                        rpoContext.SaveChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        TransmittalLog(ex);
                                        return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Unable to send email.please try again." }));
                                    }

                                }
                            }
                            #endregion
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        TransmittalLog(ex);
                        if (ex.Message.ToString().Trim().ToLower() == "Transmittal can not send to inactive contact".ToLower().Trim())
                        {
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Transmittal can not send to inactive contact." }));
                        }
                        else
                        {
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Error in saving transmittal data in database. Please try again." }));
                        }
                    }
                }
                if (emailDTO.IsDraft != null && Convert.ToBoolean(emailDTO.IsDraft))
                {

                }
                else
                {
                    JobTransmittal jobTransmittal_History = rpoContext.JobTransmittals.Include("ContactAttention").Include("EmailType").Include("ToCompany").Include("TransmissionType").FirstOrDefault(x => x.Id == responseIdJobTransmittal);
                    string emailSubject = emailDTO.Subject;

                    if (jobTransmittal_History != null && !string.IsNullOrEmpty(jobTransmittal_History.TransmittalNumber))
                    {
                        emailSubject = emailSubject.Replace("##TransmittalNo##", "- Transmittal #" + jobTransmittal_History.TransmittalNumber + (jobTransmittal_History.EmailType != null && !string.IsNullOrEmpty(jobTransmittal_History.EmailType.Name) ? " | " + jobTransmittal_History.EmailType.Name : string.Empty));
                    }
                    else
                    {
                        emailSubject = emailSubject.Replace("##TransmittalNo##", "");
                    }
                    string transmittalSend = JobHistoryMessages.TransmittalSend
                        .Replace("##TransmittalNumber##", !string.IsNullOrEmpty(jobTransmittal_History.TransmittalNumber) ? jobTransmittal_History.TransmittalNumber : JobHistoryMessages.NoSetstring)
                        .Replace("##TOContactName##", jobTransmittal_History != null && jobTransmittal_History.ContactAttention != null && !string.IsNullOrEmpty(jobTransmittal_History.ContactAttention.FirstName) ? jobTransmittal_History.ContactAttention.FirstName + " " + jobTransmittal_History.ContactAttention.LastName : JobHistoryMessages.NoSetstring)
                        //.Replace("##TOCompanyName##", jobTransmittal_History != null && jobTransmittal_History.ToCompany != null ? jobTransmittal_History.ToCompany.Name : JobHistoryMessages.NoSetstring)
                        .Replace("##SentVia##", jobTransmittal_History != null && jobTransmittal_History.TransmissionType != null && !string.IsNullOrEmpty(jobTransmittal_History.TransmissionType.Name) ? jobTransmittal_History.TransmissionType.Name : JobHistoryMessages.NoSetstring)
                        .Replace("##Subject##", !string.IsNullOrEmpty(emailSubject) ? emailSubject : JobHistoryMessages.NoSetstring);

                    Common.SaveJobHistory(employee.Id, responseIdJob, transmittalSend, JobHistoryType.Transmittals_Memo);
                }
                if (emailDTO.IsDraft != null && Convert.ToBoolean(emailDTO.IsDraft))
                {
                    return Ok(new { Message = "Transmittal saved successfully", idJob = responseIdJob.ToString(), idJobTransmittal = responseIdJobTransmittal.ToString() });
                }
                else
                {
                    return Ok(new { Message = "Mail sent successfully", idJob = responseIdJob.ToString(), idJobTransmittal = responseIdJobTransmittal.ToString() });
                }
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        private string DowanloadJobdocument(JobDocument jobDocument)
        {
            try
            {
                #region DownloadFileFromDropBox
                string path = HttpRuntime.AppDomainAppPath;
                string newFileDirectoryName = System.IO.Path.Combine(path, Properties.Settings.Default.JobDocumentExportPath);

                string folderDirectory = System.IO.Path.Combine(newFileDirectoryName, Convert.ToString(jobDocument.IdJob));
                string oldFilePath = System.IO.Path.Combine(folderDirectory, Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath);


                string jobDocumentDirectoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentExportPath));
                jobDocumentDirectoryName = System.IO.Path.Combine(jobDocumentDirectoryName, Convert.ToString(jobDocument.IdJob));
                string docpath = string.Empty;
                if (jobDocument.DocumentPath.Contains("%23"))
                {
                    docpath = Uri.UnescapeDataString(jobDocument.DocumentPath);
                }
                else
                {
                    docpath = jobDocument.DocumentPath;
                }

                string jobDocumentFileName = System.IO.Path.Combine(jobDocumentDirectoryName, jobDocument.Id + "_temp" + docpath);

                var instance = new DropboxIntegration();
                string dropBoxFilePath = Properties.Settings.Default.DropboxFolderPath + Convert.ToString(jobDocument.IdJob);
                //  string dropBoxFileName = Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;

                string dropBoxFileName = Convert.ToString(jobDocument.Id) + "_" + docpath;

                string sourceFileName = dropBoxFilePath + "/" + dropBoxFileName;

                if (!System.IO.Directory.Exists(newFileDirectoryName + jobDocument.IdJob))
                {
                    System.IO.Directory.CreateDirectory(newFileDirectoryName + jobDocument.IdJob);
                }

                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                if (System.IO.File.Exists(jobDocumentFileName))
                {
                    System.IO.File.Delete(jobDocumentFileName);
                }

                var task = instance.RunDownloadTransmittal(sourceFileName, jobDocumentFileName);

                //if(task.Result==1)
                //{
                //    jobDocumentFileName = string.Empty;
                //}

                #endregion
                return jobDocumentFileName;
            }
            catch (Exception ex)
            {
                throw ex;
                return "";
            }
        }
        public string CreateTransmittalpdfAttachment(int idJob, JobTransmittalEmailDTO jobTransmittalEmailDTO, int idTransmittal)
        {
            try
            {
                string folderName = DateTime.Today.ToString("yyyyMMdd");
                string idJobTransmittal = Guid.NewGuid().ToString();
                string filename = idTransmittal + "_JobTransmittal.pdf";

                if (!Directory.Exists(HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.JobTrasmittalPreviewPath + "/" + folderName))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.JobTrasmittalPreviewPath + "/" + folderName);
                }

                System.IO.DirectoryInfo baseDir = new System.IO.DirectoryInfo(HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.JobTrasmittalPreviewPath);
                if (baseDir.Exists)
                {
                    //delete sub directories:
                    foreach (var dir in baseDir.EnumerateDirectories())
                    {
                        if (dir.Name != folderName)
                        {
                            System.IO.Directory.Delete(dir.FullName, true);
                        }
                    }
                }

                var jobTransmittal = rpoContext.JobTransmittals.Include("JobTransmittalCCs.Employee").Include("JobTransmittalCCs.Contact").Include("Job").Include("ContactAttention")
                    .Include("EmailType").Include("FromEmployee").Where(x => x.Id == idTransmittal).FirstOrDefault();


                string specialplacename = this.rpoContext.Jobs.Where(x => x.Id == idJob).Select(x => x.SpecialPlace).SingleOrDefault();

                this.CreateJobTransmittalPreviewPdffromtrans(jobTransmittal, filename, idJob, specialplacename);

                FileInfo fileinfo = new FileInfo(HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.JobTrasmittalPreviewPath + "/" + folderName + "/" + filename);
                long fileinfoSize = fileinfo.Length;

                var downloadFilePath = Properties.Settings.Default.APIUrl + Properties.Settings.Default.JobTrasmittalPreviewPath + "/" + folderName + "/" + filename;

                return fileinfo.FullName;
            }
            catch (Exception ex)
            {
                throw ex;
                return "";
            }
        }
        public string CreateTransmittalpdfAttachment(int idJob, JobTransmittalEmailDTO jobTransmittalEmailDTO, JobTransmittal jobTransmittal)
        {
            try
            {
                string folderName = DateTime.Today.ToString("yyyyMMdd");
                string idJobTransmittal = Guid.NewGuid().ToString();
                string filename = jobTransmittal.Id + "_JobTransmittal.pdf";

                if (!Directory.Exists(HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.JobTrasmittalPreviewPath + "/" + folderName))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.JobTrasmittalPreviewPath + "/" + folderName);
                }

                System.IO.DirectoryInfo baseDir = new System.IO.DirectoryInfo(HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.JobTrasmittalPreviewPath);
                if (baseDir.Exists)
                {
                    //delete sub directories:
                    foreach (var dir in baseDir.EnumerateDirectories())
                    {
                        if (dir.Name != folderName)
                        {
                            System.IO.Directory.Delete(dir.FullName, true);
                        }
                    }
                }

                string specialplacename = this.rpoContext.Jobs.Where(x => x.Id == idJob).Select(x => x.SpecialPlace).SingleOrDefault();

                if (jobTransmittal != null)
                {
                    this.CreateJobTransmittalPreviewPdffromtrans(jobTransmittal, filename, idJob, specialplacename);
                }

                FileInfo fileinfo = new FileInfo(HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.JobTrasmittalPreviewPath + "/" + folderName + "/" + filename);
                long fileinfoSize = fileinfo.Length;

                var downloadFilePath = Properties.Settings.Default.APIUrl + Properties.Settings.Default.JobTrasmittalPreviewPath + "/" + folderName + "/" + filename;

                return fileinfo.FullName;
            }
            catch (Exception ex)
            {
                throw ex;
                return "";
            }
        }

        /// <summary>
        /// Posts the attachment.
        /// </summary>
        /// <remarks>To send the email with attachment.</remarks>
        /// <returns>send transmital to client.</returns>
        /// <exception cref="HttpResponseException">
        /// </exception>
        /// <exception cref="RpoBusinessException">
        /// Contact Id not found
        /// or
        /// or
        /// Contact Id not found
        /// or
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobtransmittals/Attachment")]
        [ResponseType(typeof(JobTransmittalAttachment))]
        public async Task<HttpResponseMessage> PostAttachment()
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddTransmittals))
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
                System.Collections.Specialized.NameValueCollection formData = provider.FormData;
                IList<HttpContent> files = provider.Files;
                string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
                int idJob = Convert.ToInt32(formData["idJob"]);
                bool IsDraft = Convert.ToBoolean(formData["isDraft"]);
                int idJobTransmittal = Convert.ToInt32(formData["idJobTransmittal"]);

                var jobTransmittal = rpoContext.JobTransmittals.Include("JobTransmittalCCs.Employee").Include("EmailType").Include("JobTransmittalCCs.Contact").Include("Job").Include("ContactAttention").Include("FromEmployee").Where(x => x.Id == idJobTransmittal).FirstOrDefault();

                if (jobTransmittal == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                #region CCEmail
                var cc = new List<KeyValuePair<string, string>>();

                foreach (JobTransmittalCC item in jobTransmittal.JobTransmittalCCs)
                {
                    if (item == null)
                    {
                        throw new RpoBusinessException("Contact/Employee Id not found.");
                    }

                    if (item.IdContact != null && item.IdContact > 0)
                    {
                        if (item.Contact.Email == null)
                        {
                            throw new RpoBusinessException($"Contact {item.Contact.FirstName} does not have a valid e-mail address.");
                        }
                        if (item.Contact.IsActive == false)
                        {
                            throw new RpoBusinessException("Transmittal can not send to inactive contact.");
                        }
                        cc.Add(new KeyValuePair<string, string>(item.Contact.Email, item.Contact.FirstName + " " + item.Contact.LastName));
                    }
                    else if (item.IdEmployee != null && item.IdEmployee > 0)
                    {
                        if (item.Employee.Email == null)
                        {
                            throw new RpoBusinessException($"Employee {item.Employee.FirstName} does not have a valid e-mail address.");
                        }

                        cc.Add(new KeyValuePair<string, string>(item.Employee.Email, item.Employee.FirstName + " " + item.Employee.LastName));
                    }

                }

                Employee employeeFrom = jobTransmittal.FromEmployee;

                if (employeeFrom != null)
                {
                    cc.Add(new KeyValuePair<string, string>(employeeFrom.Email, employeeFrom.FirstName + " " + employeeFrom.LastName));
                }
                #endregion

                #region ToEmail
                var to = new List<KeyValuePair<string, string>>();
                if (!IsDraft)
                {
                    var contactTo = jobTransmittal.ContactAttention;
                    if (contactTo == null)
                    {
                        throw new RpoBusinessException("Contact Id not found.");
                    }
                    if (contactTo.Email == null || string.IsNullOrEmpty(contactTo.Email))
                    {
                        throw new RpoBusinessException($"Contact {contactTo.FirstName} does not have a valid e-mail address.");
                    }
                    if (contactTo.IsActive == false)
                    {
                        throw new RpoBusinessException("Transmittal can not send to inactive contact.");
                    }
                    to.Add(new KeyValuePair<string, string>(contactTo.Email, contactTo.FirstName + " " + contactTo.LastName));
                }
                #endregion

                var attachments = new List<string>();

                using (DbContextTransaction transaction = rpoContext.Database.BeginTransaction())
                {
                    try
                    {

                        #region ExtAttachmentEntry
                        try
                        {
                            var path = HttpRuntime.AppDomainAppPath;
                            string thisFileName1 = string.Empty;

                            foreach (HttpContent item in files)
                            {
                                HttpContent file1 = item;
                                var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');

                                string filename = string.Empty;
                                Stream input = await file1.ReadAsStreamAsync();
                                string directoryName = string.Empty;
                                string URL = string.Empty;

                                if (thisFileName.Contains('#'))
                                {
                                    thisFileName1 = Uri.EscapeDataString(thisFileName);
                                }
                                else
                                {
                                    thisFileName1 = thisFileName;
                                }

                                JobTransmittalAttachment jobTransmittalAttachment = new JobTransmittalAttachment();
                                jobTransmittalAttachment.DocumentPath = thisFileName1;
                                jobTransmittalAttachment.IdJobTransmittal = idJobTransmittal;
                                jobTransmittalAttachment.Name = thisFileName;
                                rpoContext.JobTransmittalAttachments.Add(jobTransmittalAttachment);
                                rpoContext.SaveChanges();

                                directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobTrasmittalAttachmentsPath));

                                string directoryFileName = Convert.ToString(jobTransmittalAttachment.IdJobTransmittal) + "_" + Convert.ToString(jobTransmittalAttachment.Id) + "_" + thisFileName;
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
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                        #endregion

                        #region JodocumentAttachment
                        var jobTransmittalJobDocuments = rpoContext.JobTransmittalJobDocuments.Where(x => x.IdJobTransmittal == jobTransmittal.Id).ToList();

                        if (jobTransmittalJobDocuments != null && jobTransmittalJobDocuments.Any())
                        {
                            foreach (JobTransmittalJobDocument item in jobTransmittalJobDocuments)
                            {
                                string filename = string.Empty;
                                string directoryName = string.Empty;
                                var path = HttpRuntime.AppDomainAppPath;

                                directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.TransmittalJobDocumentPath));
                                string directoryFileName = Convert.ToString(item.IdJobTransmittal) + "_" + Convert.ToString(item.IdJobDocument) + "_" + item.DocumentPath;
                                filename = System.IO.Path.Combine(directoryName, directoryFileName);

                                if (File.Exists(filename))
                                {
                                    attachments.Add(filename);
                                }
                            }
                        }
                        #endregion

                        #region ExtAttachment
                        var jobTransmittalAttachments = rpoContext.JobTransmittalAttachments.Where(x => x.IdJobTransmittal == jobTransmittal.Id).ToList();

                        if (jobTransmittalAttachments != null && jobTransmittalAttachments.Any())
                        {
                            foreach (JobTransmittalAttachment item in jobTransmittalAttachments)
                            {
                                string filename = string.Empty;
                                string directoryName = string.Empty;
                                var path = HttpRuntime.AppDomainAppPath;

                                string docpath = string.Empty;
                                if (item.DocumentPath.Contains("%23"))
                                {
                                    docpath = Uri.UnescapeDataString(item.DocumentPath);
                                }
                                else
                                {
                                    docpath = item.DocumentPath;
                                }

                                directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobTrasmittalAttachmentsPath));
                                //  string directoryFileName = Convert.ToString(item.IdJobTransmittal) + "_" + Convert.ToString(item.Id) + "_" + item.DocumentPath;
                                string directoryFileName = Convert.ToString(item.IdJobTransmittal) + "_" + Convert.ToString(item.Id) + "_" + docpath;

                                filename = System.IO.Path.Combine(directoryName, directoryFileName);

                                if (File.Exists(filename))
                                {
                                    attachments.Add(filename);
                                }
                            }
                        }
                        #endregion

                        #region ProposalAttachment

                        JobTransmittalEmailDTO jobTransmittalEmailDTO = new JobTransmittalEmailDTO();

                        string ProposalAttach = CreateTransmittalpdfAttachment(idJob, jobTransmittalEmailDTO, jobTransmittal);

                        if (!string.IsNullOrEmpty(ProposalAttach) && File.Exists(ProposalAttach))
                        {
                            attachments.Add(ProposalAttach);
                        }
                        #endregion

                        string body = string.Empty;
                        using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/MasterEmailTemplate.htm")))
                        {
                            body = reader.ReadToEnd();
                        }

                        if (IsDraft)
                        {
                            jobTransmittal.IsDraft = IsDraft;
                            rpoContext.SaveChanges();
                        }
                        else
                        {
                            jobTransmittal.IsDraft = IsDraft;
                            rpoContext.SaveChanges();

                            TransmissionType transmissionType = rpoContext.TransmissionTypes.FirstOrDefault(x => x.Id == jobTransmittal.IdTransmissionType);
                            if (transmissionType != null && transmissionType.IsSendEmail)
                            {
                                string emailBody = body;
                                emailBody = emailBody.Replace("##EmailBody##", jobTransmittal.EmailMessage);

                                //List<TransmissionTypeDefaultCC> transmissionTypeDefaultCC = rpoContext.TransmissionTypeDefaultCCs.Include("Employee").Where(x => x.IdTransmissionType == jobTransmittal.IdTransmissionType).ToList();
                                List<TransmissionTypeDefaultCC> transmissionTypeDefaultCC = rpoContext.TransmissionTypeDefaultCCs.Include("Employee").Where(x => x.IdEamilType == jobTransmittal.IdEmailType).ToList();
                                if (transmissionTypeDefaultCC != null && transmissionTypeDefaultCC.Any())
                                {
                                    foreach (TransmissionTypeDefaultCC item in transmissionTypeDefaultCC)
                                    {
                                        cc.Add(new KeyValuePair<string, string>(item.Employee.Email, item.Employee.FirstName + " " + item.Employee.LastName));
                                    }
                                }

                                string emailSubject = jobTransmittal.EmailSubject;

                                if (jobTransmittal != null && jobTransmittal.Id != 0)
                                {
                                    emailSubject = emailSubject.Replace("##TransmittalNo##", "- Transmittal #" + jobTransmittal.Id + (jobTransmittal.EmailType != null && !string.IsNullOrEmpty(jobTransmittal.EmailType.Name) ? " | " + jobTransmittal.EmailType.Name : string.Empty));
                                }
                                else
                                {
                                    emailSubject = emailSubject.Replace("##TransmittalNo##", "");
                                }

                                try
                                {
                                    string statusmail = Mail.SendTransmittal(
                                          new KeyValuePair<string, string>(employeeFrom.Email, employeeFrom.FirstName + " " + employeeFrom.LastName),
                                          to,
                                          cc,
                                         emailSubject,
                                          emailBody,
                                          attachments
                                      );
                                    if (statusmail == "Fail")
                                    {
                                        throw new RpoUnAuthorizedException("Unable to send email.please try again.");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    TransmittalLog(ex);
                                    throw new RpoUnAuthorizedException("Unable to send email.please try again.");
                                }
                            }

                            jobTransmittal.IsEmailSent = true;
                            rpoContext.SaveChanges();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        TransmittalLog(ex);

                        List<JobTransmittalJobDocument> jobTransmittalJobdocument = rpoContext.JobTransmittalJobDocuments.Where(x => x.IdJobTransmittal == idJobTransmittal).ToList();
                        rpoContext.JobTransmittalJobDocuments.RemoveRange(jobTransmittalJobdocument);

                        List<JobTransmittalAttachment> jobTransmittalAttachmentremove = rpoContext.JobTransmittalAttachments.Where(x => x.IdJobTransmittal == idJobTransmittal).ToList();
                        rpoContext.JobTransmittalAttachments.RemoveRange(jobTransmittalAttachmentremove);

                        JobTransmittal jobTransmittalremove = rpoContext.JobTransmittals.FirstOrDefault(x => x.Id == idJobTransmittal);
                        rpoContext.JobTransmittals.Remove(jobTransmittalremove);
                        rpoContext.SaveChanges();
                    }
                }

                if (!IsDraft)
                {

                    string emailSubject = jobTransmittal.EmailSubject;

                    if (jobTransmittal != null && jobTransmittal.Id != 0)
                    {
                        emailSubject = emailSubject.Replace("##TransmittalNo##", "- Transmittal #" + jobTransmittal.Id + (jobTransmittal.EmailType != null && !string.IsNullOrEmpty(jobTransmittal.EmailType.Name) ? " | " + jobTransmittal.EmailType.Name : string.Empty));
                    }
                    else
                    {
                        emailSubject = emailSubject.Replace("##TransmittalNo##", "");
                    }

                    JobTransmittal jobTransmittal_History = rpoContext.JobTransmittals.Include("ContactAttention").Include("ToCompany").Include("TransmissionType").FirstOrDefault(x => x.Id == idJobTransmittal);
                    string transmittalSend = JobHistoryMessages.TransmittalSend
                        .Replace("##TransmittalNumber##", jobTransmittal_History.TransmittalNumber)
                        .Replace("##TOContactName##", jobTransmittal_History != null && jobTransmittal_History.ContactAttention != null ? jobTransmittal_History.ContactAttention.FirstName + " " + jobTransmittal_History.ContactAttention.LastName : string.Empty)
                        .Replace("##TOCompanyName##", jobTransmittal_History != null && jobTransmittal_History.ToCompany != null ? jobTransmittal_History.ToCompany.Name : string.Empty)
                        .Replace("##SentVia##", jobTransmittal_History != null && jobTransmittal_History.TransmissionType != null ? jobTransmittal_History.TransmissionType.Name : string.Empty)
                        .Replace("##Subject##", !string.IsNullOrEmpty(emailSubject) ? emailSubject : string.Empty);

                    Common.SaveJobHistory(employee.Id, idJob, transmittalSend, JobHistoryType.Transmittals_Memo);
                }

                var jobTransmittalAttachmentHistoryList = rpoContext.JobTransmittalAttachments
                    .Where(x => x.IdJobTransmittal == idJobTransmittal).ToList();
                var response = Request.CreateResponse<List<JobTransmittalAttachment>>(HttpStatusCode.OK, jobTransmittalAttachmentHistoryList);
                return response;

            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the job transmittal.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>delete the job transmital.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobTransmittal))]
        public IHttpActionResult DeleteJobTransmittal(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteTransmittals))
            {
                JobTransmittal jobTransmittal = rpoContext.JobTransmittals.Include("Job").Include("TransmissionType").Include("EmailType").Include("ContactAttention").FirstOrDefault(x => x.Id == id);
                if (jobTransmittal == null)
                {
                    return this.NotFound();
                }

                jobTransmittal.Job.LastModiefiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobTransmittal.Job.LastModifiedBy = employee.Id;
                }
                string emailSubject = jobTransmittal.EmailSubject;

                if (jobTransmittal != null && jobTransmittal.Id != 0)
                {
                    emailSubject = emailSubject.Replace("##TransmittalNo##", "- Transmittal #" + jobTransmittal.Id + (jobTransmittal.EmailType != null && !string.IsNullOrEmpty(jobTransmittal.EmailType.Name) ? " | " + jobTransmittal.EmailType.Name : string.Empty));
                }
                else
                {
                    emailSubject = emailSubject.Replace("##TransmittalNo##", "");
                }

                string transmittalDelete = JobHistoryMessages.TransmittalDelete
                .Replace("##TransmittalNumber##", jobTransmittal.TransmittalNumber)
                .Replace("##TOContactName##", jobTransmittal != null && jobTransmittal.ContactAttention != null && !string.IsNullOrEmpty(jobTransmittal.ContactAttention.FirstName) ? jobTransmittal.ContactAttention.FirstName + " " + jobTransmittal.ContactAttention.LastName : string.Empty)
                .Replace("##SentVia##", jobTransmittal != null && jobTransmittal.TransmissionType != null && !string.IsNullOrEmpty(jobTransmittal.TransmissionType.Name) ? jobTransmittal.TransmissionType.Name : string.Empty)
                .Replace("##Subject##", !string.IsNullOrEmpty(emailSubject) ? emailSubject : string.Empty);

                var jobTransmittalCCs = this.rpoContext.JobTransmittalCCs.Where(x => x.IdJobTransmittal == jobTransmittal.Id);
                if (jobTransmittalCCs.Any())
                {
                    this.rpoContext.JobTransmittalCCs.RemoveRange(jobTransmittalCCs);
                }

                var jobTransmittalAttachments = this.rpoContext.JobTransmittalAttachments.Where(x => x.IdJobTransmittal == jobTransmittal.Id);
                if (jobTransmittalAttachments.Any())
                {
                    this.rpoContext.JobTransmittalAttachments.RemoveRange(jobTransmittalAttachments);
                }
                rpoContext.SaveChanges();

                var jobTransmittalJobDocuments = rpoContext.JobTransmittalJobDocuments.Where(x => x.IdJobTransmittal == jobTransmittal.Id).ToList();

                if (jobTransmittalJobDocuments != null && jobTransmittalJobDocuments.Any())
                {
                    this.rpoContext.JobTransmittalJobDocuments.RemoveRange(jobTransmittalJobDocuments);
                }
                rpoContext.SaveChanges();

                rpoContext.JobTransmittals.Remove(jobTransmittal);
                rpoContext.SaveChanges();

                Common.SaveJobHistory(employee.Id, jobTransmittal.IdJob, transmittalDelete, JobHistoryType.Transmittals_Memo);
                return Ok(jobTransmittal);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Labels the specified identifier job transmittal.
        /// </summary>
        /// <param name="idJobTransmittal">The identifier job transmittal.</param>
        /// <returns>Job transmital generate pdf file.</returns>

        [Route("api/jobtransmittals/{idJobTransmittal}/LabelPDF")]
        [ResponseType(typeof(string))]
        [HttpGet]
        public async Task<HttpResponseMessage> Label(int idJobTransmittal)
        {
            var jobTransmittal = rpoContext.JobTransmittals.Include("Job.RfpAddress.Borough").Include("ContactAttention")
            .Include("Job.Company").Include("ToCompany.Addresses").FirstOrDefault(x => x.Id == idJobTransmittal);

            if (jobTransmittal == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            using (MemoryStream stream = new MemoryStream())
            {
                string filename = "JobTransmittal-label-" + idJobTransmittal + ".pdf";

                Document document = new Document(new Rectangle(577f, 386f));
                PdfWriter writer = PdfWriter.GetInstance(document, stream);
                document.Open();

                Image logo = Image.GetInstance(HttpContext.Current.Server.MapPath("~\\HTML\\images\\logo.jpg"));
                logo.Alignment = Image.ALIGN_CENTER;
                logo.ScaleToFit(120, 60);
                logo.SetAbsolutePosition(260, 760);

                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 100;
                PdfPCell cell = new PdfPCell(new Phrase(""));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(logo);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Colspan = 2;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(""));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("146 West 29th Street, Suite 2E New York, NY 10001", new Font(Font.FontFamily.HELVETICA, 9)));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(""));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("TEL: (212)-566-5110 FAX: (212)-566-5112", new Font(Font.FontFamily.HELVETICA, 9)));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 2;
                table.AddCell(cell);

                string companyName = jobTransmittal != null && jobTransmittal.ToCompany != null && jobTransmittal.ToCompany.Name != null ? jobTransmittal.ToCompany.Name : string.Empty;

                JobContact jobContact = rpoContext.JobContacts.Include("Address").FirstOrDefault(x => x.IdJob == jobTransmittal.IdJob && x.IdContact == jobTransmittal.IdContactAttention);
                Contact contacts = null;
                if (jobContact == null)
                {
                    contacts = rpoContext.Contacts.FirstOrDefault(x => x.Id == jobTransmittal.IdContactAttention);

                    contacts.Addresses = rpoContext.Addresses.Where(x => x.IdContact == jobTransmittal.IdContactAttention).ToList();
                }

                Address companyaddress = jobTransmittal != null && jobTransmittal.ToCompany != null && jobTransmittal.ToCompany.Addresses != null ? jobTransmittal.ToCompany.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : new Address();
                if (jobContact != null && jobContact.Address != null)
                {
                    companyaddress = jobContact.Address;
                }
                else
                {
                    companyaddress = contacts.Addresses.FirstOrDefault();
                }

                //Address companyaddress = jobTransmittal != null && jobTransmittal.ToCompany != null && jobTransmittal.ToCompany.Addresses != null ? jobTransmittal.ToCompany.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : new Address();

                string address2 = companyaddress.Address2 != null ? companyaddress.Address2 : string.Empty;
                string address1 = companyaddress.Address1 != null ? companyaddress.Address1 : string.Empty;
                string city = companyaddress.City != null ? companyaddress.City : string.Empty;
                string state = companyaddress.State != null ? companyaddress.State.Name : string.Empty;
                string zipCode = companyaddress.ZipCode != null ? companyaddress.ZipCode : string.Empty;
                string contactAttention = jobTransmittal.ContactAttention != null ? jobTransmittal.ContactAttention.FirstName + (jobTransmittal.ContactAttention.LastName != null ? " " + jobTransmittal.ContactAttention.LastName : string.Empty) : string.Empty;

                cell = new PdfPCell(new Phrase(""));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(companyName, new Font(Font.FontFamily.HELVETICA, 11)));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(""));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(address1, new Font(Font.FontFamily.HELVETICA, 11)));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(""));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(address2, new Font(Font.FontFamily.HELVETICA, 11)));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(""));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);


                cell = new PdfPCell(new Phrase(city + ", " + state + " " + zipCode, new Font(Font.FontFamily.HELVETICA, 11)));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 2;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(""));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("ATTN: " + contactAttention, new Font(Font.FontFamily.HELVETICA, 11)));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                document.Add(table);
                document.Close();
                writer.Close();

                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(stream.ToArray())
                };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                result.Content.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("inline")
                    {
                        FileName = filename
                    };
                return result;
            }
        }

        /// <summary>
        /// Prints the specified identifier job transmittal.
        /// </summary>
        /// <param name="idJobTransmittal">The identifier job transmittal.</param>
        /// <returns>Prints the specified identifier job transmittal.</returns>

        [Route("api/jobtransmittals/{idJobTransmittal}/Print")]
        [ResponseType(typeof(string))]
        [HttpGet]
        public IHttpActionResult Print(int idJobTransmittal)
        {
            string filename = idJobTransmittal + "_JobTransmittal.pdf";
            this.CreateJobTransmittalPdf(idJobTransmittal);
            FileInfo fileinfo = new FileInfo(HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.JobTrasmittalAttachmentsPath + "/" + filename);
            long fileinfoSize = fileinfo.Length;

            var downloadFilePath = Properties.Settings.Default.APIUrl + Properties.Settings.Default.JobTrasmittalAttachmentsPath + "/" + idJobTransmittal + "_JobTransmittal.pdf";
            return Ok(new KeyValuePair<string, string>("pdfFilePath", downloadFilePath));
        }

        /// <summary>
        /// Previews the specified identifier job.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="jobTransmittalEmailDTO">The job transmittal email dto.</param>
        /// <returns>Preview the job transmital pdf.</returns>
        [Route("api/jobtransmittals/{idJob}/Preview")]
        [ResponseType(typeof(string))]
        [HttpPost]
        public IHttpActionResult Preview(int idJob, JobTransmittalEmailDTO jobTransmittalEmailDTO)
        {
            string folderName = DateTime.Today.ToString("yyyyMMdd");
            string idJobTransmittal = Guid.NewGuid().ToString();
            string filename = idJobTransmittal + "_JobTransmittal.pdf";

            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.JobTrasmittalPreviewPath + "/" + folderName))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.JobTrasmittalPreviewPath + "/" + folderName);
            }

            System.IO.DirectoryInfo baseDir = new System.IO.DirectoryInfo(HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.JobTrasmittalPreviewPath);
            if (baseDir.Exists)
            {
                //delete sub directories:
                foreach (var dir in baseDir.EnumerateDirectories())
                {
                    if (dir.Name != folderName)
                    {
                        System.IO.Directory.Delete(dir.FullName, true);
                    }
                }
            }

            string specialplacename = this.rpoContext.Jobs.Where(x => x.Id == idJob).Select(x => x.SpecialPlace).SingleOrDefault();
            this.CreateJobTransmittalPreviewPdf(jobTransmittalEmailDTO, filename, idJob, specialplacename);
            FileInfo fileinfo = new FileInfo(HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.JobTrasmittalPreviewPath + "/" + folderName + "/" + filename);
            long fileinfoSize = fileinfo.Length;

            var downloadFilePath = Properties.Settings.Default.APIUrl + Properties.Settings.Default.JobTrasmittalPreviewPath + "/" + folderName + "/" + filename;
            return Ok(new KeyValuePair<string, string>("pdfFilePath", downloadFilePath));
        }
        /// <summary>
        /// Put the job Transmital
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idJobTransmittal"></param>
        /// <param name="emailDTO"></param>
        /// <returns>update the job transmital.</returns>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobtransmittals/{id}/email/{idJobTransmittal}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSendEmail(int id, int idJobTransmittal, [FromBody] JobTransmittalEmailDTO emailDTO)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddTransmittals))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Job job = rpoContext.Jobs.Find(id);

                if (job == null)
                {
                    return this.NotFound();
                }

                JobTransmittal jobTransmittal = rpoContext.JobTransmittals.Include("EmailType").Include("JobTransmittalJobDocuments").FirstOrDefault(x => x.Id == idJobTransmittal);

                job.LastModiefiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    job.LastModifiedBy = employee.Id;
                }

                int responseIdJob = 0;
                int responseIdJobTransmittal = 0;

                if (emailDTO.AttachmentsToDelete != null && emailDTO.AttachmentsToDelete.Count() > 0)
                {
                    DeleteTransamittalAttachments(emailDTO.AttachmentsToDelete);
                }

                if (emailDTO.IsAdditionalAtttachment)
                {
                    #region CCList
                    List<TransmittalCC> transmittalCCList = new List<TransmittalCC>();

                    if (emailDTO.TransmittalCCs != null)
                    {
                        foreach (var item in emailDTO.TransmittalCCs)
                        {
                            string[] transmittalCC = item.Split('_');
                            if (transmittalCC != null && transmittalCC.Count() > 2)
                            {
                                TransmittalCC transmittal = new TransmittalCC();
                                transmittal.Id = Convert.ToInt32(transmittalCC[2]);
                                transmittal.IdGroup = Convert.ToInt32(transmittalCC[0]);
                                if (transmittalCC[1] == "C")
                                {
                                    transmittal.IsContact = true;
                                }
                                else if (transmittalCC[1] == "E")
                                {
                                    transmittal.IsContact = false;
                                }

                                transmittalCCList.Add(transmittal);
                            }

                        }
                    }


                    List<int> projectTeam = transmittalCCList != null ? transmittalCCList.Where(x => x.IsContact == false).Select(x => x.Id).ToList() : new List<int>();
                    List<int> contactTeam = transmittalCCList != null ? transmittalCCList.Where(x => x.IsContact == true).Select(x => x.Id).ToList() : new List<int>();

                    foreach (var dest in contactTeam.Select(c => rpoContext.Contacts.Find(c)))
                    {
                        if (dest == null)
                        {
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Contact Id not found." }));
                        }

                        if (dest.Email == null)
                        {
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = $"Contact {dest.FirstName} not has a valid e-mail" }));
                        }

                    }

                    foreach (var dest in projectTeam.Select(c => rpoContext.Employees.Find(c)))
                    {
                        if (dest == null)
                        {
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Employee Id not found." }));
                        }

                        if (dest.Email == null)
                        {
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = $"Employee {dest.FirstName} not has a valid e-mail" }));
                        }
                    }
                    if (emailDTO.IsDraft == null || !Convert.ToBoolean(emailDTO.IsDraft))
                    {
                        var contactTo = rpoContext.Contacts.Where(x => x.Id == emailDTO.IdContactAttention).FirstOrDefault();
                        if (contactTo == null)
                        {
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Contact Id not found." }));
                        }
                        if (contactTo.Email == null || string.IsNullOrEmpty(contactTo.Email))
                        {
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = $"Contact {contactTo.FirstName} does not have a valid e-mail address." }));
                        }
                        if (contactTo.IsActive == false)
                        {
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Transmittal can not send to inactive contact." }));
                        }
                    }
                    #endregion

                    #region Update Transmittal
                    jobTransmittal.IdJob = job.Id;
                    jobTransmittal.IdFromEmployee = emailDTO.IdFromEmployee;
                    jobTransmittal.IdToCompany = emailDTO.IdContactsTo;
                    jobTransmittal.IdContactAttention = emailDTO.IdContactAttention;
                    jobTransmittal.IdTransmissionType = emailDTO.IdTransmissionType;
                    jobTransmittal.IdEmailType = emailDTO.IdEmailType;
                    jobTransmittal.EmailSubject = emailDTO.Subject;
                    jobTransmittal.IdTask = emailDTO.IdTask;
                    jobTransmittal.IdChecklistItem = emailDTO.IdChecklistItem;/* As part of Update Database Columns for checklist*/
                    jobTransmittal.SentDate = DateTime.UtcNow;
                    if (employee != null)
                    {
                        jobTransmittal.IdSentBy = employee.Id;
                    }
                    jobTransmittal.IsEmailSent = false;
                    jobTransmittal.EmailMessage = emailDTO.Body;
                    jobTransmittal.IsAdditionalAtttachment = emailDTO.IsAdditionalAtttachment;

                    rpoContext.SaveChanges();

                    #endregion

                    #region Delete&Add Jobdocuments

                    DeleteJobTransmitalDocuments(idJobTransmittal);

                    #region JobTransmitalDcouments
                    try
                    {
                        if (emailDTO.JobTransmittalJobDocuments != null && emailDTO.JobTransmittalJobDocuments.Count > 0)
                        {
                            foreach (JobTransmittalJobDocument item in emailDTO.JobTransmittalJobDocuments)
                            {
                                JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == item.IdJobDocument);
                                JobTransmittalJobDocument jobTransmittalJobDocument = rpoContext.JobTransmittalJobDocuments.FirstOrDefault(x => x.Id == item.Id);
                                if (item.Id > 0 && jobTransmittalJobDocument != null && emailDTO.IsRevise == false && emailDTO.IsResend == false)
                                {
                                    jobTransmittalJobDocument.IdJobDocument = item.IdJobDocument;
                                    jobTransmittalJobDocument.IdJobTransmittal = jobTransmittal.Id;
                                    jobTransmittalJobDocument.Copies = item.Copies;
                                    jobTransmittalJobDocument.DocumentPath = jobDocument.DocumentPath;
                                    rpoContext.SaveChanges();
                                    var path = HttpRuntime.AppDomainAppPath;

                                    string transmittalDirectoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.TransmittalJobDocumentPath));
                                    string transmittalDeleteFileName = System.IO.Path.Combine(transmittalDirectoryName, jobTransmittalJobDocument.IdJobTransmittal + "_" + jobTransmittalJobDocument.IdJobDocument + "_" + jobTransmittalJobDocument.DocumentPath);
                                    string transmittalFileName = System.IO.Path.Combine(transmittalDirectoryName, jobTransmittalJobDocument.IdJobTransmittal + "_" + jobTransmittalJobDocument.IdJobDocument + "_" + jobTransmittalJobDocument.DocumentPath);

                                    string jobdocumentFileName = string.Empty;

                                    if (jobDocument.DocumentName.Trim().ToLower() == "dot permit")
                                    {
                                        JobDocumentAttachment jobJobDocumentAttach = rpoContext.JobDocumentAttachments.FirstOrDefault(x => x.IdJobDocument == jobDocument.Id);
                                        if (jobJobDocumentAttach != null)
                                        {
                                            string dirpath = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentAttachmentPath));
                                            jobdocumentFileName = System.IO.Path.Combine(dirpath, jobJobDocumentAttach.Id + "_" + jobJobDocumentAttach.Path);
                                        }

                                    }
                                    else
                                    {
                                        string jobdocumentDirectoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentExportPath));
                                        jobdocumentDirectoryName = System.IO.Path.Combine(jobdocumentDirectoryName, Convert.ToString(jobDocument.IdJob));
                                        jobdocumentFileName = System.IO.Path.Combine(jobdocumentDirectoryName, jobDocument.Id + "_" + jobDocument.DocumentPath);
                                    }

                                    string returnfile = DowanloadJobdocument(jobDocument);

                                    if (!Directory.Exists(transmittalDirectoryName))
                                    {
                                        Directory.CreateDirectory(transmittalDirectoryName);
                                    }
                                    if (System.IO.File.Exists(transmittalDeleteFileName))
                                    {
                                        System.IO.File.Delete(transmittalDeleteFileName);
                                    }
                                    if (File.Exists(returnfile) && !File.Exists(transmittalFileName))
                                    {
                                        File.Copy(returnfile, transmittalFileName);
                                    }
                                }
                                else
                                {
                                    JobTransmittalJobDocument jobTransmittalJobDocumentAdd = new JobTransmittalJobDocument();
                                    jobTransmittalJobDocumentAdd.IdJobDocument = item.IdJobDocument;
                                    jobTransmittalJobDocumentAdd.IdJobTransmittal = jobTransmittal.Id;
                                    jobTransmittalJobDocumentAdd.Copies = item.Copies;
                                    jobTransmittalJobDocumentAdd.DocumentPath = jobDocument.DocumentPath;
                                    rpoContext.JobTransmittalJobDocuments.Add(jobTransmittalJobDocumentAdd);
                                    rpoContext.SaveChanges();

                                    var path = HttpRuntime.AppDomainAppPath;

                                    string transmittalDirectoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.TransmittalJobDocumentPath));
                                    string transmittalFileName = System.IO.Path.Combine(transmittalDirectoryName, jobTransmittalJobDocumentAdd.IdJobTransmittal + "_" + jobTransmittalJobDocumentAdd.IdJobDocument + "_" + jobTransmittalJobDocumentAdd.DocumentPath);


                                    string jobdocumentFileName = string.Empty;

                                    if (jobDocument.DocumentName.Trim().ToLower() == "dot permit")
                                    {
                                        JobDocumentAttachment jobJobDocumentAttach = rpoContext.JobDocumentAttachments.FirstOrDefault(x => x.IdJobDocument == jobDocument.Id);
                                        if (jobJobDocumentAttach != null)
                                        {
                                            string dirpath = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentAttachmentPath));
                                            jobdocumentFileName = System.IO.Path.Combine(dirpath, jobJobDocumentAttach.Id + "_" + jobJobDocumentAttach.Path);
                                        }

                                    }
                                    else
                                    {
                                        string jobdocumentDirectoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentExportPath));
                                        jobdocumentDirectoryName = System.IO.Path.Combine(jobdocumentDirectoryName, Convert.ToString(jobDocument.IdJob));
                                        jobdocumentFileName = System.IO.Path.Combine(jobdocumentDirectoryName, jobDocument.Id + "_" + jobDocument.DocumentPath);
                                    }
                                    PdfReader.unethicalreading = true;

                                    string returnfile = DowanloadJobdocument(jobDocument);

                                    if (!Directory.Exists(transmittalDirectoryName))
                                    {
                                        Directory.CreateDirectory(transmittalDirectoryName);
                                    }

                                    if (File.Exists(returnfile) && !File.Exists(transmittalFileName))
                                    {
                                        File.Copy(returnfile, transmittalFileName);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Unable to attach documents to the email. Please try again." }));
                    }
                    #endregion

                    #endregion

                    if (jobTransmittal.JobTransmittalCCs != null && jobTransmittal.JobTransmittalCCs.Count() > 0)
                    {
                        rpoContext.JobTransmittalCCs.RemoveRange(jobTransmittal.JobTransmittalCCs);
                    }

                    foreach (TransmittalCC transmittalCC in transmittalCCList)
                    {
                        JobTransmittalCC jobTransmittalCC = new JobTransmittalCC();
                        if (transmittalCC.IsContact)
                        {
                            jobTransmittalCC.IdContact = transmittalCC.Id;
                        }
                        else
                        {
                            jobTransmittalCC.IdEmployee = transmittalCC.Id;
                        }

                        jobTransmittalCC.IdJobTransmittal = jobTransmittal.Id;
                        jobTransmittalCC.IdGroup = transmittalCC.IdGroup;

                        rpoContext.JobTransmittalCCs.Add(jobTransmittalCC);
                        rpoContext.SaveChanges();
                    }

                    responseIdJob = job.Id;
                    responseIdJobTransmittal = jobTransmittal.Id;

                }
                else
                {

                    #region CCList
                    var cc = new List<KeyValuePair<string, string>>();

                    List<TransmittalCC> transmittalCCList = new List<TransmittalCC>();

                    if (emailDTO.TransmittalCCs != null)
                    {
                        foreach (var item in emailDTO.TransmittalCCs)
                        {
                            string[] transmittalCC = item.Split('_');
                            if (transmittalCC != null && transmittalCC.Count() > 2)
                            {
                                TransmittalCC transmittal = new TransmittalCC();
                                transmittal.Id = Convert.ToInt32(transmittalCC[2]);
                                transmittal.IdGroup = Convert.ToInt32(transmittalCC[0]);
                                if (transmittalCC[1] == "C")
                                {
                                    transmittal.IsContact = true;
                                }
                                else if (transmittalCC[1] == "E")
                                {
                                    transmittal.IsContact = false;
                                }

                                transmittalCCList.Add(transmittal);
                            }

                        }
                    }

                    List<int> projectTeam = transmittalCCList != null ? transmittalCCList.Where(x => x.IsContact == false).Select(x => x.Id).ToList() : new List<int>();
                    List<int> contactTeam = transmittalCCList != null ? transmittalCCList.Where(x => x.IsContact == true).Select(x => x.Id).ToList() : new List<int>();

                    foreach (var dest in contactTeam.Select(c => rpoContext.Contacts.Find(c)))
                    {
                        if (dest == null)
                        {
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Contact Id not found." }));
                        }
                        if (dest.Email == null)
                        {
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = $"Contact {dest.FirstName} does not have a valid e-mail address." }));
                        }
                        cc.Add(new KeyValuePair<string, string>(dest.Email, dest.FirstName + " " + dest.LastName));
                    }

                    foreach (var dest in projectTeam.Select(c => rpoContext.Employees.Find(c)))
                    {
                        if (dest == null)
                        {
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Employee Id not found." }));
                        }
                        if (dest.Email == null)
                        {
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = $"Employee {dest.FirstName} does not have a valid e-mail address." }));
                        }
                        cc.Add(new KeyValuePair<string, string>(dest.Email, dest.FirstName + " " + dest.LastName));
                    }

                    var to = new List<KeyValuePair<string, string>>();
                    if (emailDTO.IsDraft == null || !Convert.ToBoolean(emailDTO.IsDraft))
                    {
                        var contactTo = rpoContext.Contacts.Where(x => x.Id == emailDTO.IdContactAttention).FirstOrDefault();
                        if (contactTo == null)
                        {
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Contact Id not found." }));
                        }
                        if (contactTo.Email == null || string.IsNullOrEmpty(contactTo.Email))
                        {
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = $"Contact {contactTo.FirstName} does not have a valid e-mail address." }));
                        }
                        if (contactTo.IsActive == false)
                        {
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Transmittal can not send to inactive contact." }));
                        }
                        to.Add(new KeyValuePair<string, string>(contactTo.Email, contactTo.FirstName + " " + contactTo.LastName));
                    }
                    #endregion


                    var attachments = new List<string>();

                    Employee employeeFrom = rpoContext.Employees.Find(emailDTO.IdFromEmployee);

                    cc.Add(new KeyValuePair<string, string>(employeeFrom.Email, employeeFrom.FirstName + " " + employeeFrom.LastName));

                    #region UpdateTransmittal

                    jobTransmittal.IdJob = job.Id;
                    jobTransmittal.IdFromEmployee = emailDTO.IdFromEmployee;
                    jobTransmittal.IdToCompany = emailDTO.IdContactsTo;
                    jobTransmittal.IdContactAttention = emailDTO.IdContactAttention;
                    jobTransmittal.IdTransmissionType = emailDTO.IdTransmissionType;
                    jobTransmittal.IdEmailType = emailDTO.IdEmailType;
                    jobTransmittal.EmailSubject = emailDTO.Subject;
                    jobTransmittal.IdTask = emailDTO.IdTask;
                    jobTransmittal.IdChecklistItem = emailDTO.IdChecklistItem;/* As part of Update Database Columns for checklist*/
                    jobTransmittal.SentDate = DateTime.UtcNow;
                    if (employee != null)
                    {
                        jobTransmittal.IdSentBy = employee.Id;
                    }
                    jobTransmittal.EmailMessage = emailDTO.Body;
                    jobTransmittal.IsAdditionalAtttachment = emailDTO.IsAdditionalAtttachment;
                    jobTransmittal.IsEmailSent = true;

                    rpoContext.SaveChanges();

                    #endregion

                    #region Delete & Add Jobdocuments

                    DeleteJobTransmitalDocuments(idJobTransmittal);

                    #region JobTransmitalDcouments
                    try
                    {
                        if (emailDTO.JobTransmittalJobDocuments != null && emailDTO.JobTransmittalJobDocuments.Count > 0)
                        {
                            foreach (JobTransmittalJobDocument item in emailDTO.JobTransmittalJobDocuments)
                            {
                                JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == item.IdJobDocument);
                                JobTransmittalJobDocument jobTransmittalJobDocument = rpoContext.JobTransmittalJobDocuments.FirstOrDefault(x => x.Id == item.Id);
                                if (item.Id > 0 && jobTransmittalJobDocument != null && emailDTO.IsRevise == false && emailDTO.IsResend == false)
                                {
                                    jobTransmittalJobDocument.IdJobDocument = item.IdJobDocument;
                                    jobTransmittalJobDocument.IdJobTransmittal = jobTransmittal.Id;
                                    jobTransmittalJobDocument.Copies = item.Copies;
                                    jobTransmittalJobDocument.DocumentPath = jobDocument.DocumentPath;
                                    rpoContext.SaveChanges();
                                    var path = HttpRuntime.AppDomainAppPath;

                                    string transmittalDirectoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.TransmittalJobDocumentPath));
                                    string transmittalDeleteFileName = System.IO.Path.Combine(transmittalDirectoryName, jobTransmittalJobDocument.IdJobTransmittal + "_" + jobTransmittalJobDocument.IdJobDocument + "_" + jobTransmittalJobDocument.DocumentPath);
                                    string transmittalFileName = System.IO.Path.Combine(transmittalDirectoryName, jobTransmittalJobDocument.IdJobTransmittal + "_" + jobTransmittalJobDocument.IdJobDocument + "_" + jobTransmittalJobDocument.DocumentPath);

                                    string jobdocumentFileName = string.Empty;

                                    if (jobDocument.DocumentName.Trim().ToLower() == "dot permit")
                                    {
                                        JobDocumentAttachment jobJobDocumentAttach = rpoContext.JobDocumentAttachments.FirstOrDefault(x => x.IdJobDocument == jobDocument.Id);
                                        if (jobJobDocumentAttach != null)
                                        {
                                            string dirpath = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentAttachmentPath));
                                            jobdocumentFileName = System.IO.Path.Combine(dirpath, jobJobDocumentAttach.Id + "_" + jobJobDocumentAttach.Path);
                                        }

                                    }
                                    else
                                    {
                                        string jobdocumentDirectoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentExportPath));
                                        jobdocumentDirectoryName = System.IO.Path.Combine(jobdocumentDirectoryName, Convert.ToString(jobDocument.IdJob));
                                        jobdocumentFileName = System.IO.Path.Combine(jobdocumentDirectoryName, jobDocument.Id + "_" + jobDocument.DocumentPath);
                                    }

                                    startagianelse:
                                    string returnfile = DowanloadJobdocument(jobDocument);

                                    if (!Directory.Exists(transmittalDirectoryName))
                                    {
                                        Directory.CreateDirectory(transmittalDirectoryName);
                                    }
                                    if (System.IO.File.Exists(transmittalDeleteFileName))
                                    {
                                        System.IO.File.Delete(transmittalDeleteFileName);
                                    }
                                    if (File.Exists(returnfile) && !File.Exists(transmittalFileName))
                                    {
                                        File.Copy(returnfile, transmittalFileName);
                                    }
                                }
                                else
                                {
                                    JobTransmittalJobDocument jobTransmittalJobDocumentAdd = new JobTransmittalJobDocument();
                                    jobTransmittalJobDocumentAdd.IdJobDocument = item.IdJobDocument;
                                    jobTransmittalJobDocumentAdd.IdJobTransmittal = jobTransmittal.Id;
                                    jobTransmittalJobDocumentAdd.Copies = item.Copies;
                                    jobTransmittalJobDocumentAdd.DocumentPath = jobDocument.DocumentPath;
                                    rpoContext.JobTransmittalJobDocuments.Add(jobTransmittalJobDocumentAdd);
                                    rpoContext.SaveChanges();

                                    var path = HttpRuntime.AppDomainAppPath;

                                    string transmittalDirectoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.TransmittalJobDocumentPath));
                                    string transmittalFileName = System.IO.Path.Combine(transmittalDirectoryName, jobTransmittalJobDocumentAdd.IdJobTransmittal + "_" + jobTransmittalJobDocumentAdd.IdJobDocument + "_" + jobTransmittalJobDocumentAdd.DocumentPath);


                                    string jobdocumentFileName = string.Empty;

                                    if (jobDocument.DocumentName.Trim().ToLower() == "dot permit")
                                    {
                                        JobDocumentAttachment jobJobDocumentAttach = rpoContext.JobDocumentAttachments.FirstOrDefault(x => x.IdJobDocument == jobDocument.Id);
                                        if (jobJobDocumentAttach != null)
                                        {
                                            string dirpath = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentAttachmentPath));
                                            jobdocumentFileName = System.IO.Path.Combine(dirpath, jobJobDocumentAttach.Id + "_" + jobJobDocumentAttach.Path);
                                        }

                                    }
                                    else
                                    {
                                        string jobdocumentDirectoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentExportPath));
                                        jobdocumentDirectoryName = System.IO.Path.Combine(jobdocumentDirectoryName, Convert.ToString(jobDocument.IdJob));
                                        jobdocumentFileName = System.IO.Path.Combine(jobdocumentDirectoryName, jobDocument.Id + "_" + jobDocument.DocumentPath);
                                    }
                                    PdfReader.unethicalreading = true;

                                    startagianelsenw:
                                    string returnfile = DowanloadJobdocument(jobDocument);

                                    if (!Directory.Exists(transmittalDirectoryName))
                                    {
                                        Directory.CreateDirectory(transmittalDirectoryName);
                                    }

                                    if (File.Exists(returnfile) && !File.Exists(transmittalFileName))
                                    {
                                        File.Copy(returnfile, transmittalFileName);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Unable to attach documents to the email. Please try again." }));
                    }
                    #endregion

                    #endregion

                    if (jobTransmittal.JobTransmittalCCs != null && jobTransmittal.JobTransmittalCCs.Count() > 0)
                    {
                        rpoContext.JobTransmittalCCs.RemoveRange(jobTransmittal.JobTransmittalCCs);
                    }

                    foreach (TransmittalCC transmittalCC in transmittalCCList)
                    {
                        JobTransmittalCC jobTransmittalCC = new JobTransmittalCC();
                        if (transmittalCC.IsContact)
                        {
                            jobTransmittalCC.IdContact = transmittalCC.Id;
                        }
                        else
                        {
                            jobTransmittalCC.IdEmployee = transmittalCC.Id;
                        }
                        jobTransmittalCC.IdJobTransmittal = jobTransmittal.Id;
                        jobTransmittalCC.IdGroup = transmittalCC.IdGroup;

                        rpoContext.JobTransmittalCCs.Add(jobTransmittalCC);
                        rpoContext.SaveChanges();
                    }

                    responseIdJob = job.Id;
                    responseIdJobTransmittal = jobTransmittal.Id;

                    jobTransmittal.IsEmailSent = true;
                    rpoContext.SaveChanges();

                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/MasterEmailTemplate.htm")))
                    {
                        body = reader.ReadToEnd();
                    }

                    #region JobTransmittalJobDocuments
                    var jobTransmittalJobDocuments = rpoContext.JobTransmittalJobDocuments.Where(x => x.IdJobTransmittal == jobTransmittal.Id).ToList();

                    if (jobTransmittalJobDocuments != null && jobTransmittalJobDocuments.Any())
                    {
                        foreach (JobTransmittalJobDocument item in jobTransmittalJobDocuments)
                        {
                            string filename = string.Empty;
                            string directoryName = string.Empty;
                            var path = HttpRuntime.AppDomainAppPath;

                            directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.TransmittalJobDocumentPath));
                            string directoryFileName = Convert.ToString(item.IdJobTransmittal) + "_" + Convert.ToString(item.IdJobDocument) + "_" + item.DocumentPath;
                            filename = System.IO.Path.Combine(directoryName, directoryFileName);

                            if (File.Exists(filename))
                            {
                                attachments.Add(filename);
                            }
                        }
                    }
                    #endregion

                    #region JobTransmittalAttachments
                    var jobTransmittalAttachments = rpoContext.JobTransmittalAttachments.Where(x => x.IdJobTransmittal == jobTransmittal.Id).ToList();

                    if (jobTransmittalAttachments != null && jobTransmittalAttachments.Any())
                    {
                        foreach (JobTransmittalAttachment item in jobTransmittalAttachments)
                        {
                            string filename = string.Empty;
                            string directoryName = string.Empty;
                            var path = HttpRuntime.AppDomainAppPath;

                            directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobTrasmittalAttachmentsPath));

                            string docpath = string.Empty;
                            if (item.DocumentPath.Contains("%23"))
                            {
                                docpath = Uri.UnescapeDataString(item.DocumentPath);
                            }
                            else
                            {
                                docpath = item.DocumentPath;
                            }

                            // string directoryFileName = Convert.ToString(item.IdJobTransmittal) + "_" + Convert.ToString(item.Id) + "_" + item.DocumentPath;
                            string directoryFileName = Convert.ToString(item.IdJobTransmittal) + "_" + Convert.ToString(item.Id) + "_" + docpath;
                            filename = System.IO.Path.Combine(directoryName, directoryFileName);

                            if (File.Exists(filename))
                            {
                                attachments.Add(filename);
                            }
                        }
                    }
                    #endregion

                    if (emailDTO.IsDraft != null && Convert.ToBoolean(emailDTO.IsDraft))
                    {
                        jobTransmittal.IsDraft = emailDTO.IsDraft;
                        rpoContext.SaveChanges();
                    }
                    else
                    {
                        jobTransmittal.IsDraft = emailDTO.IsDraft;
                        rpoContext.SaveChanges();

                        TransmissionType transmissionType = rpoContext.TransmissionTypes.FirstOrDefault(x => x.Id == jobTransmittal.IdTransmissionType);
                        if (transmissionType != null && transmissionType.IsSendEmail)
                        {
                            string emailBody = body;
                            emailBody = emailBody.Replace("##EmailBody##", emailDTO.Body);

                            //List<TransmissionTypeDefaultCC> transmissionTypeDefaultCC = rpoContext.TransmissionTypeDefaultCCs.Include("Employee").Where(x => x.IdTransmissionType == emailDTO.IdTransmissionType).ToList();
                            List<TransmissionTypeDefaultCC> transmissionTypeDefaultCC = rpoContext.TransmissionTypeDefaultCCs.Include("Employee").Where(x => x.IdEamilType == emailDTO.IdEmailType).ToList();
                            if (transmissionTypeDefaultCC != null && transmissionTypeDefaultCC.Any())
                            {
                                foreach (TransmissionTypeDefaultCC item in transmissionTypeDefaultCC)
                                {
                                    cc.Add(new KeyValuePair<string, string>(item.Employee.Email, item.Employee.FirstName + " " + item.Employee.LastName));
                                }
                            }

                            string emailSubject = jobTransmittal.EmailSubject;

                            if (jobTransmittal != null && jobTransmittal.Id != 0)
                            {
                                emailSubject = emailSubject.Replace("##TransmittalNo##", "- Transmittal #" + jobTransmittal.Id + (jobTransmittal.EmailType != null && !string.IsNullOrEmpty(jobTransmittal.EmailType.Name) ? " | " + jobTransmittal.EmailType.Name : string.Empty));
                            }
                            else
                            {
                                emailSubject = emailSubject.Replace("##TransmittalNo##", "");
                            }

                            #region ProposalAttachment
                            string ProposalAttach = CreateTransmittalpdfAttachment(job.Id, emailDTO, jobTransmittal.Id);

                            if (!string.IsNullOrEmpty(ProposalAttach) && File.Exists(ProposalAttach))
                            {
                                attachments.Add(ProposalAttach);
                            }
                            #endregion

                            try
                            {
                                string statusmail = Mail.SendTransmittal(
                                      new KeyValuePair<string, string>(employeeFrom.Email, employeeFrom.FirstName + " " + employeeFrom.LastName),
                                      to,
                                      cc,
                                     emailSubject,
                                      emailBody,
                                      attachments
                                  );
                                if (statusmail == "Fail")
                                {
                                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Unable to send email.please try again." }));
                                    jobTransmittal.IsEmailSent = false;
                                }
                                else
                                {
                                    jobTransmittal.IsEmailSent = true;
                                }
                                rpoContext.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                TransmittalLog(ex);
                                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = "Unable to send email.please try again." }));
                            }


                        }
                    }
                }

                if (emailDTO.IsDraft != null && Convert.ToBoolean(emailDTO.IsDraft))
                {

                }
                else
                {

                    string emailSubject = jobTransmittal.EmailSubject;

                    if (jobTransmittal != null && jobTransmittal.Id != 0)
                    {
                        emailSubject = emailSubject.Replace("##TransmittalNo##", "- Transmittal #" + jobTransmittal.Id + (jobTransmittal.EmailType != null && !string.IsNullOrEmpty(jobTransmittal.EmailType.Name) ? " | " + jobTransmittal.EmailType.Name : string.Empty));
                    }
                    else
                    {
                        emailSubject = emailSubject.Replace("##TransmittalNo##", "");
                    }


                    JobTransmittal jobTransmittal_History = rpoContext.JobTransmittals.Include("ContactAttention").Include("ToCompany").Include("TransmissionType").FirstOrDefault(x => x.Id == responseIdJobTransmittal);
                    string transmittalSend = JobHistoryMessages.TransmittalSend
                        .Replace("##TransmittalNumber##", !string.IsNullOrEmpty(jobTransmittal_History.TransmittalNumber) ? jobTransmittal_History.TransmittalNumber : string.Empty)
                        .Replace("##TOContactName##", jobTransmittal_History != null && jobTransmittal_History.ContactAttention != null && !string.IsNullOrEmpty(jobTransmittal_History.ContactAttention.FirstName) ? jobTransmittal_History.ContactAttention.FirstName + " " + jobTransmittal_History.ContactAttention.LastName : string.Empty)
                        .Replace("##TOCompanyName##", jobTransmittal_History != null && jobTransmittal_History.ToCompany != null && !string.IsNullOrEmpty(jobTransmittal_History.ToCompany.Name) ? jobTransmittal_History.ToCompany.Name : string.Empty)
                        .Replace("##SentVia##", jobTransmittal_History != null && jobTransmittal_History.TransmissionType != null && !string.IsNullOrEmpty(jobTransmittal_History.TransmissionType.Name) ? jobTransmittal_History.TransmissionType.Name : string.Empty)
                        .Replace("##Subject##", !string.IsNullOrEmpty(emailSubject) ? emailSubject : string.Empty);

                    Common.SaveJobHistory(employee.Id, responseIdJob, transmittalSend, JobHistoryType.Transmittals_Memo);
                }
                // return Ok(new { Message = "Mail sent successfully", idJob = responseIdJob.ToString(), idJobTransmittal = responseIdJobTransmittal.ToString() });
                if (emailDTO.IsDraft != null && Convert.ToBoolean(emailDTO.IsDraft))
                {
                    return Ok(new { Message = "Transmittal saved successfully", idJob = responseIdJob.ToString(), idJobTransmittal = responseIdJobTransmittal.ToString() });
                }
                else
                {
                    return Ok(new { Message = "Mail sent successfully", idJob = responseIdJob.ToString(), idJobTransmittal = responseIdJobTransmittal.ToString() });
                }
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        #endregion

        #region Private Methods
        // public static String CSS = "p { font-family: 'Times New Roman', color:'red', font-size:'20px',list-style:'disc', margin: '10px'} ul {margin: '5px 0', padding-bottom: '10px',line-height: '5px'} ol {margin: '5px 0', padding-bottom: '10px',line-height: '5px'} li { font-family: 'Times New Roman'}";
        public static String CSS = "p { font-family: 'Avenir Next LT Pro', color:'red', font-size:'20px',list-style:'disc'} li { font-family: 'Avenir Next LT Pro'}";
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
        /// Creates the job transmittal PDF.
        /// </summary>
        /// <param name="idJobTransmittal">The identifier job transmittal.</param>
        private void CreateJobTransmittalPdf(int idJobTransmittal)
        {
            var jobTransmittal = rpoContext.JobTransmittals.Include("FromEmployee").Include("ToCompany.Addresses").Include("Job.RfpAddress.Borough").Include("TransmissionType")
                .Include("EmailType").Include("ContactAttention").Include("FromEmployee").Include("JobTransmittalCCs.Contact").Include("JobTransmittalCCs.Employee").Include("JobTransmittalAttachments")
                .Include("JobTransmittalJobDocuments")
                .Include("Job.Company").FirstOrDefault(x => x.Id == idJobTransmittal);

            string tmpjobnum = "Job #" + jobTransmittal.IdJob.ToString();

            string emailSubject = jobTransmittal.EmailSubject.Replace(tmpjobnum, "").Replace("##TransmittalNo##", "").Trim();
            if (!string.IsNullOrEmpty(emailSubject))
            {
                emailSubject = "" + emailSubject.Substring(1, emailSubject.Length - 1).Trim();
            }

            using (MemoryStream stream = new MemoryStream())
            {
                string filename = idJobTransmittal + "_JobTransmittal.pdf";
                string transmittalNumber = jobTransmittal != null && jobTransmittal.TransmittalNumber != null ? jobTransmittal.TransmittalNumber : string.Empty;
                string ProjectDescription = jobTransmittal != null && jobTransmittal.Job.ProjectDescription != null ? jobTransmittal.Job.ProjectDescription : "-";
                string emailMessage = jobTransmittal != null && jobTransmittal.EmailMessage != null ? jobTransmittal.EmailMessage : string.Empty;
                string subject = !string.IsNullOrEmpty(emailSubject) ? emailSubject : string.Empty;
                //subject = jobTransmittal != null && string.IsNullOrEmpty(jobTransmittal.Job.SpecialPlace) ? subject : subject = subject + " | " + jobTransmittal.Job.SpecialPlace;
                string jobNumber = jobTransmittal != null && jobTransmittal.Job != null && jobTransmittal.Job.JobNumber != null ? jobTransmittal.Job.JobNumber : string.Empty;
                string poNumber = jobTransmittal != null && jobTransmittal.Job.PONumber != null ? jobTransmittal.Job.PONumber : "-";
                string sentVia = jobTransmittal != null && jobTransmittal.TransmissionType != null ? jobTransmittal.TransmissionType.Name : string.Empty;
                string emailType = jobTransmittal != null && jobTransmittal.EmailType != null ? jobTransmittal.EmailType.Name : string.Empty;
                string companyName = jobTransmittal != null && jobTransmittal.ToCompany != null && jobTransmittal.ToCompany.Name != null ? jobTransmittal.ToCompany.Name : string.Empty;
                string companyNameCheck = jobTransmittal != null && jobTransmittal.ToCompany != null && jobTransmittal.ToCompany.Name != null ? jobTransmittal.ToCompany.Name : string.Empty;
                string fromEmployee = jobTransmittal != null && jobTransmittal.FromEmployee != null ? jobTransmittal.FromEmployee.FirstName + " " + jobTransmittal.FromEmployee.LastName : string.Empty;
                Address companyaddress = jobTransmittal != null && jobTransmittal.ToCompany != null && jobTransmittal.ToCompany.Addresses != null ? jobTransmittal.ToCompany.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : new Address();
                JobContact jobContact = rpoContext.JobContacts.Include("Address").FirstOrDefault(x => x.IdJob == jobTransmittal.IdJob && x.IdContact == jobTransmittal.IdContactAttention);
                if (jobContact != null && jobContact.Address != null)
                {
                    companyaddress = jobContact.Address;
                }

                string address2 = companyaddress.Address2 != null ? companyaddress.Address2 : string.Empty;
                string address1 = companyaddress.Address1 != null ? companyaddress.Address1 : string.Empty;
                string city = companyaddress.City != null ? companyaddress.City : string.Empty;
                string state = companyaddress.State != null ? companyaddress.State.Acronym : string.Empty;
                string zipCode = companyaddress.ZipCode != null ? companyaddress.ZipCode : string.Empty;

                string contactAttention = jobTransmittal.ContactAttention != null ? jobTransmittal.ContactAttention.FirstName + (jobTransmittal.ContactAttention.LastName != null ? " " + jobTransmittal.ContactAttention.LastName : string.Empty) : string.Empty;

                if (string.IsNullOrEmpty(companyNameCheck))
                {
                    companyName = contactAttention;
                }

                string from = jobTransmittal.FromEmployee != null ? jobTransmittal.FromEmployee.FirstName + (jobTransmittal.FromEmployee.LastName != null ? " " + jobTransmittal.FromEmployee.LastName : string.Empty) : string.Empty;
                string attentionName = string.Empty;
                if (jobTransmittal != null)
                {
                    attentionName = jobTransmittal.ContactAttention != null && !string.IsNullOrEmpty(jobTransmittal.ContactAttention.LastName)
                                          ? (jobTransmittal.ContactAttention.Prefix != null ? jobTransmittal.ContactAttention.Prefix.Name : string.Empty)
                                          + (jobTransmittal.ContactAttention.Prefix != null ? jobTransmittal.ContactAttention.LastName : jobTransmittal.ContactAttention.FirstName + (!string.IsNullOrEmpty(jobTransmittal.ContactAttention.LastName) ? " " + jobTransmittal.ContactAttention.LastName : string.Empty)) : jobTransmittal.ContactAttention.FirstName;
                }
                iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                styles.LoadTagStyle("p", "font-size", "50px");
                styles.LoadTagStyle("p", "font-family", "Avenir Next LT Pro");
                styles.LoadTagStyle("p", "color", "red");
                //                styles.LoadTagStyle("p", "list-style", "disc");
                styles.LoadTagStyle("p", "margin-bottom", "10px");
                styles.LoadTagStyle("p", "padding-bottom", "10px 0");
                styles.LoadTagStyle("li", "font-family", "Avenir Next LT Pro");

                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.JobTrasmittalAttachmentsPath + "/" + filename, FileMode.Create));
                writer.PageEvent = new Header(jobNumber);

                document.Open();


                string fontRegular = HttpContext.Current.Server.MapPath("~\\HTML\\fonts\\AvenirNextLTPro-Regular.ttf");
                string fontBold = HttpContext.Current.Server.MapPath("~\\HTML\\fonts\\Avenir Next LT Pro Bold.ttf");
                BaseFont customfontRegualr = BaseFont.CreateFont(fontRegular, BaseFont.CP1252, BaseFont.EMBEDDED);
                BaseFont customfontBold = BaseFont.CreateFont(fontBold, BaseFont.CP1252, BaseFont.EMBEDDED);

                Font font__Bold_16 = new Font(customfontBold, 16, 1);
                Font font_Bold_11 = new Font(customfontRegualr, 11, 1);
                Font font_Reguar_11 = new Font(customfontRegualr, 11, 0);
                Font font_Italic_11 = new Font(customfontRegualr, 11, 2);

                Image logo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~\\HTML\\images\\RPO-Logo-pdf.png"));
                logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                logo.ScaleToFit(100, 80);
                logo.SetAbsolutePosition(260, 760);

                PdfPTable table2 = new PdfPTable(5);
                table2.WidthPercentage = 100;

                PdfPCell cell = new PdfPCell(logo);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Rowspan = 2;
                cell.PaddingBottom = 5;
                table2.AddCell(cell);

                string reportHeader = //"RPO INCORPORATED" + Environment.NewLine +
                "146 West 29th Street, Suite 2E"
                + Environment.NewLine + "New York, NY 10001"
                + Environment.NewLine + "(212) 566-5110"
                + Environment.NewLine + "www.rpoinc.com";

                string reportTitle = "RPO, Inc." + Environment.NewLine + "Construction Consultants";

                cell = new PdfPCell(new Phrase(reportTitle, font__Bold_16));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -30;
                cell.Colspan = 5;
                cell.PaddingTop = -2;
                table2.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportHeader, font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -30;
                cell.Colspan = 5;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                table2.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.Border = PdfPCell.BOTTOM_BORDER;
                cell.Colspan = 5;
                cell.PaddingTop = -15;
                table2.AddCell(cell);
                document.Add(table2);

                PdfPTable tableT = new PdfPTable(3);
                tableT.WidthPercentage = 100;
                PdfPCell cell2 = new PdfPCell(new Phrase(Chunk.NEWLINE));
                tableT.SetWidths(new float[] { 28f, 50f, 27f });

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 3;
                tableT.AddCell(cell);

                cell2 = new PdfPCell(new Phrase("Transmittal#:  " + String.Format("{0}", transmittalNumber), font_Bold_11));
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2.Border = PdfPCell.NO_BORDER;
                tableT.AddCell(cell2);

                cell2 = new PdfPCell(new Phrase("LETTER OF TRANSMITTAL", font_Bold_11));
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2.Border = PdfPCell.NO_BORDER;
                tableT.AddCell(cell2);

                cell2 = new PdfPCell(new Phrase("", font_Bold_11));
                cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell2.Border = PdfPCell.NO_BORDER;
                tableT.AddCell(cell2);
                document.Add(tableT);

                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;

                cell = new PdfPCell(new Phrase(emailType, font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);
                table.SetWidths(new float[] { 25f, 120f, 50f, 48f, 70f });
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                DateTime sentDate = jobTransmittal.SentDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTransmittal.SentDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTransmittal.SentDate;

                cell = new PdfPCell(new Phrase("Date: ", font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);


                cell = new PdfPCell(new Phrase(sentDate.ToShortDateString(), font_Reguar_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 4;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("To: ", font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Colspan = 1;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(companyName, font_Reguar_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 4;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(""));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Colspan = 1;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(address1, font_Reguar_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Colspan = 2;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Sent Via: ", font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Colspan = 1;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(sentVia, font_Reguar_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(""));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Colspan = 1;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);
                if (string.IsNullOrEmpty(address2))
                {
                    cell = new PdfPCell(new Phrase(city + ", " + state + " " + zipCode, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 2;
                    table.AddCell(cell);
                }

                if (!string.IsNullOrEmpty(address2))
                {
                    cell = new PdfPCell(new Phrase(address2, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 2;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("Project#: ", font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(jobNumber, font_Reguar_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);
                if (!string.IsNullOrEmpty(address2))
                {
                    cell = new PdfPCell(new Phrase(""));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(city + ", " + state + " " + zipCode, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 2;
                    table.AddCell(cell);
                }
                else
                {
                    cell = new PdfPCell(new Phrase(""));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 3;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("PO#: ", font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(poNumber, font_Reguar_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);
                if (!string.IsNullOrEmpty(companyNameCheck))
                {
                    cell = new PdfPCell(new Phrase("Attn: ", font_Bold_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(contactAttention, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 2;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Desc: ", font_Bold_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(ProjectDescription, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Rowspan = 2;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("From:", font_Bold_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(from, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                }
                else
                {
                    cell = new PdfPCell(new Phrase("From:", font_Bold_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(from, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 2;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Desc: ", font_Bold_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(ProjectDescription, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Rowspan = 2;
                    table.AddCell(cell);
                }
                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("RE: " + subject, font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);

                document.Add(table);

                PdfPTable tableContent = new PdfPTable(1);
                tableContent.WidthPercentage = 100;
                string content = emailMessage;
                tableContent = AddUL(content);
                tableContent.SplitLate = false;
                document.Add(tableContent);

                //using (StringReader sr = new StringReader(emailMessage))
                //{
                //    List<IElement> elements = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(sr, null);
                //    if (elements != null)
                //    {
                //        foreach (IElement e in elements)
                //        {
                //            document.Add(e);
                //        }
                //    }
                //}

                table = new PdfPTable(5);
                table.WidthPercentage = 100;

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);
                document.Add(table);

                PdfPTable tableAttachment = new PdfPTable(4);
                //tableAttachment.SetWidths(new float[] { 10, 35, 55 });
                tableAttachment.SetWidths(new float[] { 15, 15, 15, 55 });
                tableAttachment.WidthPercentage = 100;

                bool isEnclosure = false;
                int documentEncloserCount = 0;
                if (jobTransmittal.JobTransmittalAttachments != null && jobTransmittal.JobTransmittalAttachments.Count > 0)
                {
                    isEnclosure = true;
                }
                if (!isEnclosure && jobTransmittal.JobTransmittalJobDocuments != null && jobTransmittal.JobTransmittalJobDocuments.Count > 0)
                {
                    isEnclosure = true;
                }

                //documentEncloserCount = jobTransmittal.JobTransmittalAttachments.Count + jobTransmittal.JobTransmittalJobDocuments.Count;
                documentEncloserCount = (jobTransmittal.JobTransmittalAttachments != null ? jobTransmittal.JobTransmittalAttachments.Count : 0) + (jobTransmittal.JobTransmittalJobDocuments != null ? jobTransmittal.JobTransmittalJobDocuments.Count : 0);
                PdfPCell cellAttachment = new PdfPCell();
                if (isEnclosure)
                {
                    cellAttachment = new PdfPCell(new Paragraph("Enclosure (" + documentEncloserCount + ")", font_Bold_11));
                    cellAttachment.Colspan = 5;
                    cellAttachment.Border = PdfPCell.NO_BORDER;
                    cellAttachment.PaddingBottom = 10;
                    cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                    tableAttachment.AddCell(cellAttachment);
                }

                int attachementCount = 0;
                string permitCode = string.Empty;
                if (jobTransmittal.JobTransmittalAttachments != null)
                {
                    foreach (JobTransmittalAttachment item in jobTransmittal.JobTransmittalAttachments)
                    {
                        if (attachementCount == 0)
                        {
                            cellAttachment = new PdfPCell(new Paragraph("Name", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("A#/LOC/VIO#", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("Application Type", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("Description", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph(item.Name, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("", font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("", font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("", font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            attachementCount = attachementCount + 1;
                        }
                        else
                        {
                            cellAttachment = new PdfPCell(new Paragraph(item.Name, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("", font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("", font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);


                            cellAttachment = new PdfPCell(new Paragraph("", font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);
                        }
                    }
                }

                if (jobTransmittal.JobTransmittalJobDocuments != null)
                {
                    var jobDocumentIds = jobTransmittal.JobTransmittalJobDocuments.Select(x => new
                    {
                        IdJobDocument = x.IdJobDocument ?? 0,
                        JobDocument = rpoContext.JobDocuments.Include("DocumentMaster").FirstOrDefault(c => c.Id == x.IdJobDocument),
                        Copies = x.Copies

                    }).ToList();
                    string jobAppValLoc = string.Empty;
                    foreach (var item in jobDocumentIds)
                    {
                        if (attachementCount == 0)
                        {
                            cellAttachment = new PdfPCell(new Paragraph("Name", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("A#/LOC/VIO#", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("Application Type", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("Description", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph(item.JobDocument.DocumentMaster.Code, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            jobAppValLoc = item.JobDocument != null && item.JobDocument.JobApplication != null ? item.JobDocument.JobApplication.ApplicationNumber : (item.JobDocument.JobViolation != null ? item.JobDocument.JobViolation.SummonsNumber : string.Empty);
                            cellAttachment = new PdfPCell(new Paragraph(jobAppValLoc, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            var applicationTypeId = item.JobDocument != null && item.JobDocument.JobApplication != null ? item.JobDocument.JobApplication.IdJobApplicationType : null;
                            string applicationType = string.Empty;
                            if (applicationTypeId != null)
                            {
                                applicationType = rpoContext.JobApplicationTypes.Where(x => x.Id == applicationTypeId).Select(x => x.Description).SingleOrDefault();
                            }
                            cellAttachment = new PdfPCell(new Paragraph(applicationType, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            if (item.JobDocument.DocumentMaster.Code == "VARPMT")
                            {
                                string[] multiArray = item.JobDocument.DocumentDescription.Split(new Char[] { '|', '\n', '\t' });
                                var varPMTDescription = multiArray.ToList();
                                for (int i = 0; i < varPMTDescription.Count; i++)
                                {
                                    if (varPMTDescription[i].Contains("Work Description:"))
                                    {
                                        varPMTDescription.RemoveAt(i);
                                    }
                                    if (varPMTDescription[i].Contains("AHV Reference Number:"))
                                    {
                                        varPMTDescription.RemoveAt(i);
                                    }
                                }
                                cellAttachment = new PdfPCell(new Paragraph(String.Join("|", varPMTDescription), font_Reguar_11));
                                cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                                cellAttachment.Padding = 3;
                                tableAttachment.AddCell(cellAttachment);
                            }
                            else
                            {
                                cellAttachment = new PdfPCell(new Paragraph(item.JobDocument.DocumentDescription, font_Reguar_11));
                                cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                                cellAttachment.Padding = 3;
                                tableAttachment.AddCell(cellAttachment);
                            }

                            attachementCount = attachementCount + 1;
                            permitCode += item.JobDocument.PermitNumber != null ? item.JobDocument.PermitNumber.TrimEnd() + ", " : string.Empty;
                        }
                        else
                        {
                            cellAttachment = new PdfPCell(new Paragraph(item.JobDocument.DocumentMaster.Code, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            jobAppValLoc = item.JobDocument != null && item.JobDocument.JobApplication != null ? item.JobDocument.JobApplication.ApplicationNumber : (item.JobDocument.JobViolation != null ? item.JobDocument.JobViolation.SummonsNumber : string.Empty);
                            cellAttachment = new PdfPCell(new Paragraph(jobAppValLoc, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            var applicationTypeId = item.JobDocument != null && item.JobDocument.JobApplication != null ? item.JobDocument.JobApplication.IdJobApplicationType : null;
                            string applicationType = string.Empty;
                            if (applicationTypeId != null)
                            {
                                applicationType = rpoContext.JobApplicationTypes.Where(x => x.Id == applicationTypeId).Select(x => x.Description).SingleOrDefault();
                            }
                            cellAttachment = new PdfPCell(new Paragraph(applicationType, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            if (item.JobDocument.DocumentMaster.Code == "VARPMT")
                            {
                                string[] multiArray = item.JobDocument.DocumentDescription.Split(new Char[] { '|', '\n', '\t' });
                                var varPMTDescription = multiArray.ToList();
                                for (int i = 0; i < varPMTDescription.Count; i++)
                                {
                                    if (varPMTDescription[i].Contains("Work Description:"))
                                    {
                                        varPMTDescription.RemoveAt(i);
                                    }
                                    if (varPMTDescription[i].Contains("AHV Reference Number:"))
                                    {
                                        varPMTDescription.RemoveAt(i);
                                    }
                                }
                                cellAttachment = new PdfPCell(new Paragraph(String.Join("|", varPMTDescription), font_Reguar_11));
                                cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                                cellAttachment.Padding = 3;
                                tableAttachment.AddCell(cellAttachment);

                            }
                            else
                            {
                                cellAttachment = new PdfPCell(new Paragraph(item.JobDocument.DocumentDescription, font_Reguar_11));
                                cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                                cellAttachment.Padding = 3;
                                tableAttachment.AddCell(cellAttachment);
                            }
                            permitCode += item.JobDocument.PermitNumber != null ? item.JobDocument.PermitNumber.TrimEnd() + ", " : string.Empty;
                        }
                    }
                }

                document.Add(tableAttachment);

                table = new PdfPTable(5);
                table.WidthPercentage = 100;
                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);

                int ccCount = 0;
                if (jobTransmittal.JobTransmittalCCs != null)
                {
                    foreach (JobTransmittalCC item in jobTransmittal.JobTransmittalCCs)
                    {
                        if (item.Contact != null)
                        {
                            if (ccCount == 0)
                            {
                                string contactcc = item.Contact != null ? item.Contact.FirstName + (item.Contact.LastName != null ? " " + item.Contact.LastName : string.Empty) : string.Empty;
                                cell = new PdfPCell(new Paragraph("cc: " + contactcc, font_Italic_11));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.NO_BORDER;
                                cell.Colspan = 5;
                                table.AddCell(cell);
                                ccCount = ccCount + 1;
                            }
                            else
                            {
                                string contactcc = item.Contact != null ? item.Contact.FirstName + (item.Contact.LastName != null ? " " + item.Contact.LastName : string.Empty) : string.Empty;
                                cell = new PdfPCell(new Paragraph(contactcc, font_Italic_11));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.NO_BORDER;
                                cell.Colspan = 5;
                                table.AddCell(cell);
                            }
                        }
                        else
                        {
                            if (ccCount == 0)
                            {
                                string employeecc = item.Employee != null ? item.Employee.FirstName + (item.Employee.LastName != null ? " " + item.Employee.LastName : string.Empty) : string.Empty;
                                cell = new PdfPCell(new Paragraph("cc: " + employeecc, font_Italic_11));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.NO_BORDER;
                                cell.Colspan = 5;
                                table.AddCell(cell);
                                ccCount = ccCount + 1;
                            }
                            else
                            {
                                string employeecc = item.Employee != null ? item.Employee.FirstName + (item.Employee.LastName != null ? " " + item.Employee.LastName : string.Empty) : string.Empty;
                                cell = new PdfPCell(new Paragraph(employeecc, font_Italic_11));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.NO_BORDER;
                                cell.Colspan = 5;
                                table.AddCell(cell);
                            }
                        }
                    }

                }
                if (emailType.Trim() == "DOT Permit(s)  with Noise Mitigation Stipulation" || emailType.Trim() == "Permit(s) - DOT" || emailType.Trim() == "Full Closing Permit(s) - DOT" && !string.IsNullOrEmpty(permitCode))
                {
                    string input = " " + permitCode.Remove(permitCode.Length - 2);
                    char delimiter = ',';
                    var allParts = input.Split(delimiter);
                    string result = allParts.Select((item, index) => (index != 0 && (index + 1) % 3 == 0)
                                      ? item + delimiter + Environment.NewLine.PadRight(18)
                                        : index != allParts.Count() - 1 ? item + delimiter : item)
                                        .Aggregate((i, j) => i + j);
                    cell = new PdfPCell(new Paragraph("Permit# - " + result, new Font(Font.FontFamily.HELVETICA, 11)));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 5;
                    table.AddCell(cell);
                }

                HTMLWorker worker = new HTMLWorker(document);
                worker.SetStyleSheet(styles);

                document.Add(table);
                document.Close();
                writer.Close();

            }
        }

        /// <summary>
        /// Creates the job transmittal preview PDF.
        /// </summary>
        /// <param name="jobTransmittalEmail">The job transmittal email.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="idJob">The identifier job.</param>
        private void CreateJobTransmittalPreviewPdf(JobTransmittalEmailDTO jobTransmittalEmail, string fileName, int idJob, string specialplacename = "")
        {
            //var jobTransmittal = rpoContext.JobTransmittals.Include("ToCompany.Addresses").Include("Job.RfpAddress.Borough").Include("TransmissionType")
            //    .Include("EmailType").Include("ContactAttention").Include("FromEmployee").Include("JobTransmittalCCs.Contact").Include("JobTransmittalAttachments")
            //    .Include("JobTransmittalJobDocuments")
            //    .Include("Job.Company").FirstOrDefault(x => x.Id == idJobTransmittal);

            string folderName = DateTime.Today.ToString("yyyyMMdd");

            using (MemoryStream stream = new MemoryStream())
            {
                string tmpjobnum = "Job #" + idJob.ToString();
                //string transmittalNumber = jobTransmittal != null && jobTransmittal.TransmittalNumber != null ? jobTransmittal.TransmittalNumber : string.Empty;
                string transmittalNumber = "#########";
                string emailMessage = jobTransmittalEmail != null && jobTransmittalEmail.Body != null ? jobTransmittalEmail.Body : string.Empty;
                //string subject = jobTransmittalEmail != null && jobTransmittalEmail.Subject != null ? jobTransmittalEmail.Subject : string.Empty;
                string subject = jobTransmittalEmail != null && jobTransmittalEmail.Subject != null ? jobTransmittalEmail.Subject.Replace(tmpjobnum, "").Replace("##TransmittalNo##", "").Trim() : string.Empty;

                if (!string.IsNullOrEmpty(subject))
                {
                    subject = "" + subject.Substring(1, subject.Length - 1).Trim();
                }

                //subject = string.IsNullOrEmpty(specialplacename) ? subject : subject = subject + " | " + specialplacename;
                Job job = rpoContext.Jobs.FirstOrDefault(x => x.Id == idJob);

                TransmissionType transmissionType = rpoContext.TransmissionTypes.FirstOrDefault(x => x.Id == jobTransmittalEmail.IdTransmissionType);
                EmailType emailTypeModel = rpoContext.EmailTypes.FirstOrDefault(x => x.Id == jobTransmittalEmail.IdEmailType);
                Company company = rpoContext.Companies.FirstOrDefault(x => x.Id == jobTransmittalEmail.IdContactsTo);

                string jobNumber = job != null && job.JobNumber != null ? job.JobNumber : string.Empty;
                string poNumber = job != null && job.PONumber != null ? job.PONumber : "-";
                string sentVia = transmissionType != null ? transmissionType.Name : string.Empty;
                string emailType = emailTypeModel != null ? emailTypeModel.Name : string.Empty;
                string ProjectDescription = job != null && job.ProjectDescription != null ? job.ProjectDescription : "-";
                string companyName = company != null && company.Name != null ? company.Name : string.Empty;
                string companyNameCheck = company != null && company.Name != null ? company.Name : string.Empty;

                Address companyaddress = company != null && company.Addresses != null ? company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : new Address();

                JobContact jobContact = rpoContext.JobContacts.Include("Address").FirstOrDefault(x => x.IdJob == idJob && x.IdContact == jobTransmittalEmail.IdContactAttention);
                if (jobContact != null && jobContact.Address != null)
                {
                    companyaddress = jobContact.Address;
                }

                string address2 = companyaddress.Address2 != null ? companyaddress.Address2 : string.Empty;
                string address1 = companyaddress.Address1 != null ? companyaddress.Address1 : string.Empty;
                string city = companyaddress.City != null ? companyaddress.City : string.Empty;
                string state = companyaddress.State != null ? companyaddress.State.Acronym : string.Empty;
                string zipCode = companyaddress.ZipCode != null ? companyaddress.ZipCode : string.Empty;

                Contact contact = rpoContext.Contacts.FirstOrDefault(x => x.Id == jobTransmittalEmail.IdContactAttention);
                Employee fromEmployee = rpoContext.Employees.FirstOrDefault(x => x.Id == jobTransmittalEmail.IdFromEmployee);

                string contactAttention = contact != null ? contact.FirstName + (contact.LastName != null ? " " + contact.LastName : string.Empty) : string.Empty;

                if (string.IsNullOrEmpty(companyNameCheck))
                {
                    companyName = contactAttention;
                }

                string from = fromEmployee != null ? fromEmployee.FirstName + (fromEmployee.LastName != null ? " " + fromEmployee.LastName : string.Empty) : string.Empty;
                string attentionName = string.Empty;
                if (contact != null)
                {
                    attentionName = contact != null && !string.IsNullOrEmpty(contact.LastName)
                                           ? (contact.Prefix != null ? contact.Prefix.Name : string.Empty)
                                           + (contact.Prefix != null ? contact.LastName : contact.FirstName + (!string.IsNullOrEmpty(contact.LastName) ? " " + contact.LastName : string.Empty)) : contact.FirstName;
                }
                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.JobTrasmittalPreviewPath + "/" + folderName + "/" + fileName, FileMode.Create));
                writer.PageEvent = new Header(jobNumber);
                document.Open();


                string fontRegular = HttpContext.Current.Server.MapPath("~\\HTML\\fonts\\AvenirNextLTPro-Regular.ttf");
                string fontBold = HttpContext.Current.Server.MapPath("~\\HTML\\fonts\\Avenir Next LT Pro Bold.ttf");
                BaseFont customfontRegualr = BaseFont.CreateFont(fontRegular, BaseFont.CP1252, BaseFont.EMBEDDED);
                BaseFont customfontBold = BaseFont.CreateFont(fontBold, BaseFont.CP1252, BaseFont.EMBEDDED);

                Font font__Bold_16 = new Font(customfontBold, 16, 1);
                Font font_Bold_11 = new Font(customfontRegualr, 11, 1);
                Font font_Reguar_11 = new Font(customfontRegualr, 11, 0);
                Font font_Italic_11 = new Font(customfontRegualr, 11, 2);

                Image logo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~\\HTML\\images\\RPO-Logo-pdf.png"));
                logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                logo.ScaleToFit(100, 80);
                logo.SetAbsolutePosition(260, 760);

                PdfPTable table2 = new PdfPTable(5);
                table2.WidthPercentage = 100;

                PdfPCell cell = new PdfPCell(logo);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Rowspan = 2;
                cell.PaddingBottom = 5;
                table2.AddCell(cell);

                string reportHeader = //"RPO INCORPORATED" + Environment.NewLine +
                "146 West 29th Street, Suite 2E"
                + Environment.NewLine + "New York, NY 10001"
                + Environment.NewLine + "(212) 566-5110"
                + Environment.NewLine + "www.rpoinc.com";

                string reportTitle = "RPO, Inc." + Environment.NewLine + "Construction Consultants";
                cell = new PdfPCell(new Phrase(reportTitle, font__Bold_16));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -30;
                cell.Colspan = 5;
                cell.PaddingTop = -2;
                table2.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportHeader, font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -30;
                cell.Colspan = 5;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                table2.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.Border = PdfPCell.BOTTOM_BORDER;
                cell.Colspan = 5;
                cell.PaddingTop = -15;
                table2.AddCell(cell);
                document.Add(table2);

                PdfPTable tableT = new PdfPTable(3);
                tableT.WidthPercentage = 100;
                PdfPCell cell2 = new PdfPCell(new Phrase(Chunk.NEWLINE));
                tableT.SetWidths(new float[] { 28f, 50f, 27f });


                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 3;
                tableT.AddCell(cell);

                cell2 = new PdfPCell(new Phrase("Transmittal#:  " + String.Format("{0}", transmittalNumber), font_Bold_11));
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2.Border = PdfPCell.NO_BORDER;
                tableT.AddCell(cell2);

                cell2 = new PdfPCell(new Phrase("LETTER OF TRANSMITTAL", font_Bold_11));
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2.Border = PdfPCell.NO_BORDER;
                tableT.AddCell(cell2);

                cell2 = new PdfPCell(new Phrase("", font_Bold_11));
                cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell2.Border = PdfPCell.NO_BORDER;
                tableT.AddCell(cell2);
                document.Add(tableT);

                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;
                //PdfPCell cell = new PdfPCell();

                cell = new PdfPCell(new Phrase(emailType, font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);

                table.SetWidths(new float[] { 25f, 120f, 50f, 48f, 70f });
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                DateTime sentDate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(DateTime.UtcNow), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));

                cell = new PdfPCell(new Phrase("Date: ", font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);


                cell = new PdfPCell(new Phrase(sentDate.ToShortDateString(), font_Reguar_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 4;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("To: ", font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Colspan = 1;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(companyName, font_Reguar_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 4;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(""));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Colspan = 1;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(address1, font_Reguar_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Colspan = 2;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Sent Via: ", font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Colspan = 1;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(sentVia, font_Reguar_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(""));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Colspan = 1;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);
                if (string.IsNullOrEmpty(address2))
                {
                    cell = new PdfPCell(new Phrase(city + ", " + state + " " + zipCode, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 2;
                    table.AddCell(cell);
                }

                if (!string.IsNullOrEmpty(address2))
                {
                    cell = new PdfPCell(new Phrase(address2, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 2;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("Project#: ", font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(jobNumber, font_Reguar_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);
                if (!string.IsNullOrEmpty(address2))
                {
                    cell = new PdfPCell(new Phrase(""));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(city + ", " + state + " " + zipCode, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 2;
                    table.AddCell(cell);
                }
                else
                {
                    cell = new PdfPCell(new Phrase(""));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 3;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("PO#: ", font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(poNumber, font_Reguar_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);
                if (!string.IsNullOrEmpty(companyNameCheck))
                {
                    cell = new PdfPCell(new Phrase("Attn: ", font_Bold_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(contactAttention, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 2;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Desc: ", font_Bold_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(ProjectDescription, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Rowspan = 2;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("From:", font_Bold_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(from, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                }
                else
                {
                    cell = new PdfPCell(new Phrase("From:", font_Bold_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(from, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 2;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Desc: ", font_Bold_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(ProjectDescription, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Rowspan = 2;
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("RE: " + subject, font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);

                document.Add(table);

                PdfPTable tableContent = new PdfPTable(1);
                tableContent.WidthPercentage = 100;
                string content = emailMessage;
                subject = content;
                tableContent = AddUL(content);
                tableContent.SplitLate = false;
                document.Add(tableContent);

                table = new PdfPTable(5);
                table.WidthPercentage = 100;

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);
                document.Add(table);

                PdfPTable tableAttachment = new PdfPTable(4);
                //tableAttachment.SetWidths(new float[] { 10, 35, 55 });
                //tableAttachment.SetWidths(new float[] { 10, 10, 80 });
                tableAttachment.SetWidths(new float[] { 15, 15, 15, 55 });
                tableAttachment.WidthPercentage = 100;

                bool isEnclosure = false;
                int documentEncloserCount = 0;
                if (jobTransmittalEmail.JobTransmittalAttachments != null && jobTransmittalEmail.JobTransmittalAttachments.Count > 0)
                {
                    isEnclosure = true;
                }
                if (!isEnclosure && jobTransmittalEmail.JobTransmittalJobDocuments != null && jobTransmittalEmail.JobTransmittalJobDocuments.Count > 0)
                {
                    isEnclosure = true;
                }
                documentEncloserCount = (jobTransmittalEmail.JobTransmittalAttachments != null ? jobTransmittalEmail.JobTransmittalAttachments.Count : 0) + (jobTransmittalEmail.JobTransmittalJobDocuments != null ? jobTransmittalEmail.JobTransmittalJobDocuments.Count : 0);

                PdfPCell cellAttachment = new PdfPCell();
                if (isEnclosure)
                {
                    cellAttachment = new PdfPCell(new Paragraph("Enclosure (" + documentEncloserCount + ")", font_Bold_11));
                    cellAttachment.Colspan = 5;
                    cellAttachment.Border = PdfPCell.NO_BORDER;
                    cellAttachment.PaddingBottom = 10;
                    cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                    tableAttachment.AddCell(cellAttachment);
                }

                int attachementCount = 0;
                string permitCode = string.Empty;
                if (jobTransmittalEmail.JobTransmittalAttachments != null)
                {
                    foreach (JobTransmittalAttachment item in jobTransmittalEmail.JobTransmittalAttachments)
                    {
                        if (attachementCount == 0)
                        {
                            cellAttachment = new PdfPCell(new Paragraph("Name", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("A#/LOC/VIO#", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("Application Type", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("Description", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph(item.Name, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("", font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("", font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("", font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            attachementCount = attachementCount + 1;
                        }
                        else
                        {
                            cellAttachment = new PdfPCell(new Paragraph(item.Name, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("", font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("", font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("", font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);
                        }
                    }
                }

                if (jobTransmittalEmail.JobTransmittalJobDocuments != null)
                {
                    var jobDocumentIds = jobTransmittalEmail.JobTransmittalJobDocuments.Select(x => new
                    {
                        IdJobDocument = x.IdJobDocument ?? 0,
                        JobDocument = rpoContext.JobDocuments.FirstOrDefault(c => c.Id == x.IdJobDocument),
                        Copies = x.Copies,
                        Code = (from ab in rpoContext.DocumentMasters
                                from cd in rpoContext.JobDocuments
                                where ab.Id == cd.IdDocument
                               && cd.Id == x.IdJobDocument
                                select ab.Code).FirstOrDefault()

                    }).ToList();
                    string jobAppValLoc = string.Empty;

                    foreach (var item in jobDocumentIds)
                    {
                        if (attachementCount == 0)
                        {
                            cellAttachment = new PdfPCell(new Paragraph("Name", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("A#/LOC/VIO#", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("Application Type", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("Description", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph(item.Code, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            jobAppValLoc = item.JobDocument != null && item.JobDocument.JobApplication != null ? item.JobDocument.JobApplication.ApplicationNumber : (item.JobDocument.JobViolation != null ? item.JobDocument.JobViolation.SummonsNumber : string.Empty);
                            cellAttachment = new PdfPCell(new Paragraph(jobAppValLoc, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            var applicationTypeId = item.JobDocument != null && item.JobDocument.JobApplication != null ? item.JobDocument.JobApplication.IdJobApplicationType : null;
                            string applicationType = string.Empty;
                            if (applicationTypeId != null)
                            {
                                applicationType = rpoContext.JobApplicationTypes.Where(x => x.Id == applicationTypeId).Select(x => x.Description).SingleOrDefault();
                            }
                            cellAttachment = new PdfPCell(new Paragraph(applicationType, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            if (item.Code == "VARPMT")
                            {
                                string[] multiArray = item.JobDocument.DocumentDescription.Split(new Char[] { '|', '\n', '\t' });
                                var varPMTDescription = multiArray.ToList();
                                for (int i = 0; i < varPMTDescription.Count; i++)
                                {
                                    if (varPMTDescription[i].Contains("Work Description:"))
                                    {
                                        varPMTDescription.RemoveAt(i);
                                    }
                                    if (varPMTDescription[i].Contains("AHV Reference Number:"))
                                    {
                                        varPMTDescription.RemoveAt(i);
                                    }
                                }
                                cellAttachment = new PdfPCell(new Paragraph(String.Join("|", varPMTDescription), font_Reguar_11));
                                cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                                cellAttachment.Padding = 3;
                                tableAttachment.AddCell(cellAttachment);

                            }
                            else
                            {
                                cellAttachment = new PdfPCell(new Paragraph(item.JobDocument.DocumentDescription, font_Reguar_11));
                                cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                                cellAttachment.Padding = 3;
                                tableAttachment.AddCell(cellAttachment);
                            }
                            attachementCount = attachementCount + 1;
                            permitCode += item.JobDocument.PermitNumber != null ? item.JobDocument.PermitNumber.TrimEnd() + ", " : string.Empty;

                        }
                        else
                        {
                            cellAttachment = new PdfPCell(new Paragraph(item.Code, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            jobAppValLoc = item.JobDocument != null && item.JobDocument.JobApplication != null ? item.JobDocument.JobApplication.ApplicationNumber : (item.JobDocument.JobViolation != null ? item.JobDocument.JobViolation.SummonsNumber : string.Empty);
                            cellAttachment = new PdfPCell(new Paragraph(jobAppValLoc, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            var applicationTypeId = item.JobDocument != null && item.JobDocument.JobApplication != null ? item.JobDocument.JobApplication.IdJobApplicationType : null;
                            string applicationType = string.Empty;
                            if (applicationTypeId != null)
                            {
                                applicationType = rpoContext.JobApplicationTypes.Where(x => x.Id == applicationTypeId).Select(x => x.Description).SingleOrDefault();
                            }
                            cellAttachment = new PdfPCell(new Paragraph(applicationType, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);



                            if (item.Code == "VARPMT")
                            {
                                string[] multiArray = item.JobDocument.DocumentDescription.Split(new Char[] { '|', '\n', '\t' });
                                var varPMTDescription = multiArray.ToList();
                                for (int i = 0; i < varPMTDescription.Count; i++)
                                {
                                    if (varPMTDescription[i].Contains("Work Description:"))
                                    {
                                        varPMTDescription.RemoveAt(i);
                                    }
                                    if (varPMTDescription[i].Contains("AHV Reference Number:"))
                                    {
                                        varPMTDescription.RemoveAt(i);
                                    }
                                }
                                cellAttachment = new PdfPCell(new Paragraph(String.Join("|", varPMTDescription), font_Reguar_11));
                                cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                                cellAttachment.Padding = 3;
                                tableAttachment.AddCell(cellAttachment);

                            }
                            else
                            {
                                cellAttachment = new PdfPCell(new Paragraph(item.JobDocument.DocumentDescription, font_Reguar_11));
                                cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                                cellAttachment.Padding = 3;
                                tableAttachment.AddCell(cellAttachment);
                            }

                            permitCode += item.JobDocument.PermitNumber != null ? item.JobDocument.PermitNumber.TrimEnd() + ", " : string.Empty;
                        }
                    }
                }

                //cell = new PdfPCell(tableAttachment);
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.Border = PdfPCell.NO_BORDER;
                //cell.Colspan = 5;
                //table.AddCell(cell);
                document.Add(tableAttachment);


                table = new PdfPTable(5);
                table.WidthPercentage = 100;
                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);

                List<TransmittalCC> transmittalCCList = new List<TransmittalCC>();

                if (jobTransmittalEmail.TransmittalCCs != null)
                {
                    foreach (var item in jobTransmittalEmail.TransmittalCCs)
                    {
                        string[] transmittalCC = item.Split('_');
                        if (transmittalCC != null && transmittalCC.Count() > 2)
                        {
                            TransmittalCC transmittal = new TransmittalCC();
                            transmittal.Id = Convert.ToInt32(transmittalCC[2]);
                            transmittal.IdGroup = Convert.ToInt32(transmittalCC[0]);
                            if (transmittalCC[1] == "C")
                            {
                                transmittal.IsContact = true;
                            }
                            else if (transmittalCC[1] == "E")
                            {
                                transmittal.IsContact = false;
                            }

                            transmittalCCList.Add(transmittal);
                        }

                    }
                }

                int ccCount = 0;
                if (jobTransmittalEmail.TransmittalCCs != null)
                {
                    foreach (TransmittalCC transmittalCC in transmittalCCList)
                    {
                        Contact transmitalContact = null;
                        Employee transmitalEmployee = null;
                        if (transmittalCC.IsContact)
                        {
                            transmitalContact = rpoContext.Contacts.FirstOrDefault(x => x.Id == transmittalCC.Id);
                        }
                        else
                        {
                            transmitalEmployee = rpoContext.Employees.FirstOrDefault(x => x.Id == transmittalCC.Id);
                        }

                        if (transmitalContact != null)
                        {
                            if (ccCount == 0)
                            {
                                string contactcc = transmitalContact != null ? transmitalContact.FirstName + (transmitalContact.LastName != null ? " " + transmitalContact.LastName : string.Empty) : string.Empty;
                                cell = new PdfPCell(new Paragraph("cc: " + contactcc, font_Italic_11));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.NO_BORDER;
                                cell.Colspan = 5;
                                table.AddCell(cell);
                                ccCount = ccCount + 1;
                            }
                            else
                            {
                                string contactcc = transmitalContact != null ? transmitalContact.FirstName + (transmitalContact.LastName != null ? " " + transmitalContact.LastName : string.Empty) : string.Empty;
                                cell = new PdfPCell(new Paragraph(contactcc, font_Italic_11));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.NO_BORDER;
                                cell.Colspan = 5;
                                table.AddCell(cell);
                            }
                        }
                        else if (transmitalEmployee != null)
                        {
                            if (ccCount == 0)
                            {
                                string employeecc = transmitalEmployee != null ? transmitalEmployee.FirstName + (transmitalEmployee.LastName != null ? " " + transmitalEmployee.LastName : string.Empty) : string.Empty;
                                cell = new PdfPCell(new Paragraph("cc: " + employeecc, font_Italic_11));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.NO_BORDER;
                                cell.Colspan = 5;
                                table.AddCell(cell);
                                ccCount = ccCount + 1;
                            }
                            else
                            {
                                string employeecc = transmitalEmployee != null ? transmitalEmployee.FirstName + (transmitalEmployee.LastName != null ? " " + transmitalEmployee.LastName : string.Empty) : string.Empty;
                                cell = new PdfPCell(new Paragraph(employeecc, font_Italic_11));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.NO_BORDER;
                                cell.Colspan = 5;
                                table.AddCell(cell);
                            }
                        }
                    }
                }
                if (emailType.Trim() == "DOT Permit(s)  with Noise Mitigation Stipulation" || emailType.Trim() == "Permit(s) - DOT" || emailType.Trim() == "Full Closing Permit(s) - DOT" && !string.IsNullOrEmpty(permitCode))
                {
                    string input = " " + permitCode.Remove(permitCode.Length - 2);
                    char delimiter = ',';
                    var allParts = input.Split(delimiter);
                    string result = allParts.Select((item, index) => (index != 0 && (index + 1) % 3 == 0)
                                      ? item + delimiter + Environment.NewLine.PadRight(18)
                                        : index != allParts.Count() - 1 ? item + delimiter : item)
                                        .Aggregate((i, j) => i + j);
                    cell = new PdfPCell(new Paragraph("Permit# - " + result, new Font(Font.FontFamily.HELVETICA, 11)));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 5;
                    table.AddCell(cell);
                }
                document.Add(table);
                document.Close();
                writer.Close();
            }
        }

        private void CreateJobTransmittalPreviewPdffromtrans(JobTransmittal jobTransmittalEmail, string fileName, int idJob, string specialplacename = "")
        {
            //var jobTransmittal = rpoContext.JobTransmittals.Include("ToCompany.Addresses").Include("Job.RfpAddress.Borough").Include("TransmissionType")
            //    .Include("EmailType").Include("ContactAttention").Include("FromEmployee").Include("JobTransmittalCCs.Contact").Include("JobTransmittalAttachments")
            //    .Include("JobTransmittalJobDocuments")
            //    .Include("Job.Company").FirstOrDefault(x => x.Id == idJobTransmittal);

            string folderName = DateTime.Today.ToString("yyyyMMdd");

            using (MemoryStream stream = new MemoryStream())
            {
                string tmpjobnum = "Job #" + idJob.ToString();
                //string transmittalNumber = jobTransmittal != null && jobTransmittal.TransmittalNumber != null ? jobTransmittal.TransmittalNumber : string.Empty;
                string transmittalNumber = jobTransmittalEmail.TransmittalNumber;
                string emailMessage = jobTransmittalEmail != null && jobTransmittalEmail.EmailMessage != null ? jobTransmittalEmail.EmailMessage : string.Empty;
                string subject = jobTransmittalEmail != null && jobTransmittalEmail.EmailSubject != null ? jobTransmittalEmail.EmailSubject.Replace(tmpjobnum, "").Replace("##TransmittalNo##", "").Trim() : string.Empty;

                if (!string.IsNullOrEmpty(subject))
                {
                    subject = "" + subject.Substring(1, subject.Length - 1).Trim();
                }
                //subject = string.IsNullOrEmpty(specialplacename) ? subject : subject = subject + " | " + specialplacename;
                Job job = rpoContext.Jobs.FirstOrDefault(x => x.Id == idJob);
                TransmissionType transmissionType = rpoContext.TransmissionTypes.FirstOrDefault(x => x.Id == jobTransmittalEmail.IdTransmissionType);
                EmailType emailTypeModel = rpoContext.EmailTypes.FirstOrDefault(x => x.Id == jobTransmittalEmail.IdEmailType);
                Company company = rpoContext.Companies.FirstOrDefault(x => x.Id == jobTransmittalEmail.IdToCompany);

                string jobNumber = job != null && job.JobNumber != null ? job.JobNumber : string.Empty;
                string poNumber = job != null && job.PONumber != null ? job.PONumber : "-";
                string sentVia = transmissionType != null ? transmissionType.Name : string.Empty;
                string emailType = emailTypeModel != null ? emailTypeModel.Name : string.Empty;
                string ProjectDescription = job != null && job.ProjectDescription != null ? job.ProjectDescription : "-";
                string companyName = company != null && company.Name != null ? company.Name : string.Empty;
                string companyNameCheck = company != null && company.Name != null ? company.Name : string.Empty;

                Address companyaddress = company != null && company.Addresses != null ? company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : new Address();

                JobContact jobContact = rpoContext.JobContacts.Include("Address").FirstOrDefault(x => x.IdJob == idJob && x.IdContact == jobTransmittalEmail.IdContactAttention);
                if (jobContact != null && jobContact.Address != null)
                {
                    companyaddress = jobContact.Address;
                }

                string address2 = companyaddress.Address2 != null ? companyaddress.Address2 : string.Empty;
                string address1 = companyaddress.Address1 != null ? companyaddress.Address1 : string.Empty;
                string city = companyaddress.City != null ? companyaddress.City : string.Empty;
                string state = companyaddress.State != null ? companyaddress.State.Acronym : string.Empty;
                string zipCode = companyaddress.ZipCode != null ? companyaddress.ZipCode : string.Empty;

                Contact contact = rpoContext.Contacts.FirstOrDefault(x => x.Id == jobTransmittalEmail.IdContactAttention);
                Employee fromEmployee = rpoContext.Employees.FirstOrDefault(x => x.Id == jobTransmittalEmail.IdFromEmployee);

                string contactAttention = contact != null ? contact.FirstName + (contact.LastName != null ? " " + contact.LastName : string.Empty) : string.Empty;

                if (string.IsNullOrEmpty(companyNameCheck))
                {
                    companyName = contactAttention;
                }

                string from = fromEmployee != null ? fromEmployee.FirstName + (fromEmployee.LastName != null ? " " + fromEmployee.LastName : string.Empty) : string.Empty;
                string attentionName = string.Empty;
                if (contact != null)
                {
                    attentionName = contact != null && !string.IsNullOrEmpty(contact.LastName)
                                       ? (contact.Prefix != null ? contact.Prefix.Name : string.Empty)
                                       + (contact.Prefix != null ? contact.LastName : contact.FirstName + (!string.IsNullOrEmpty(contact.LastName) ? " " + contact.LastName : string.Empty)) : contact.FirstName;
                }
                iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                styles.LoadTagStyle("p", "font-family", "Avenir Next LT Pro");
                styles.LoadTagStyle("p", "color", "red");
                styles.LoadTagStyle("p", "font-size", "20px");
                styles.LoadTagStyle("p", "list-style", "disc");
                styles.LoadTagStyle("li", "font-family", "Avenir Next LT Pro");

                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.JobTrasmittalPreviewPath + "/" + folderName + "/" + fileName, FileMode.Create));
                writer.PageEvent = new Header(jobNumber);
                document.Open();

                string fontRegular = HttpContext.Current.Server.MapPath("~\\HTML\\fonts\\AvenirNextLTPro-Regular.ttf");
                string fontBold = HttpContext.Current.Server.MapPath("~\\HTML\\fonts\\Avenir Next LT Pro Bold.ttf");
                BaseFont customfontRegualr = BaseFont.CreateFont(fontRegular, BaseFont.CP1252, BaseFont.EMBEDDED);
                BaseFont customfontBold = BaseFont.CreateFont(fontBold, BaseFont.CP1252, BaseFont.EMBEDDED);

                Font font__Bold_16 = new Font(customfontBold, 16, 1);
                Font font_Bold_11 = new Font(customfontRegualr, 11, 1);
                Font font_Reguar_11 = new Font(customfontRegualr, 11, 0);
                Font font_Italic_11 = new Font(customfontRegualr, 11, 2);

                Image logo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~\\HTML\\images\\RPO-Logo-pdf.png"));
                logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                logo.ScaleToFit(100, 80);
                logo.SetAbsolutePosition(260, 760);

                PdfPTable table2 = new PdfPTable(5);
                table2.WidthPercentage = 100;

                PdfPCell cell = new PdfPCell(logo);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Rowspan = 2;
                cell.PaddingBottom = 5;
                table2.AddCell(cell);

                string reportHeader = //"RPO INCORPORATED" + Environment.NewLine +
                "146 West 29th Street, Suite 2E"
                + Environment.NewLine + "New York, NY 10001"
                + Environment.NewLine + "(212) 566-5110"
                + Environment.NewLine + "www.rpoinc.com";


                string reportTitle = "RPO, Inc." + Environment.NewLine + "Construction Consultants";
                cell = new PdfPCell(new Phrase(reportTitle, font__Bold_16));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -30;
                cell.Colspan = 5;
                cell.PaddingTop = -2;
                table2.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportHeader, font_Reguar_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -30;
                cell.Colspan = 5;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                table2.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.Border = PdfPCell.BOTTOM_BORDER;
                cell.Colspan = 5;
                cell.PaddingTop = -15;
                table2.AddCell(cell);

                document.Add(table2);

                PdfPTable tableT = new PdfPTable(3);
                tableT.WidthPercentage = 100;
                PdfPCell cell2 = new PdfPCell(new Phrase(Chunk.NEWLINE));
                tableT.SetWidths(new float[] { 28f, 50f, 27f });

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 3;
                tableT.AddCell(cell);

                cell2 = new PdfPCell(new Phrase("Transmittal#:  " + String.Format("{0}", transmittalNumber), font_Bold_11));
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2.Border = PdfPCell.NO_BORDER;
                tableT.AddCell(cell2);

                cell2 = new PdfPCell(new Phrase("LETTER OF TRANSMITTAL", font_Bold_11));
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2.Border = PdfPCell.NO_BORDER;
                tableT.AddCell(cell2);

                cell2 = new PdfPCell(new Phrase("", font_Bold_11));
                cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell2.Border = PdfPCell.NO_BORDER;
                tableT.AddCell(cell2);
                document.Add(tableT);

                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;
                cell = new PdfPCell(new Phrase(emailType, font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);
                table.SetWidths(new float[] { 25f, 120f, 50f, 48f, 70f });
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                DateTime sentDate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(DateTime.UtcNow), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));

                cell = new PdfPCell(new Phrase("Date: ", font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);


                cell = new PdfPCell(new Phrase(sentDate.ToShortDateString(), font_Reguar_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 4;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("To: ", font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Colspan = 1;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(companyName, font_Reguar_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 4;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(""));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Colspan = 1;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(address1, font_Reguar_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Colspan = 2;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Sent Via: ", font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Colspan = 1;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(sentVia, font_Reguar_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(""));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Colspan = 1;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);
                if (string.IsNullOrEmpty(address2))
                {
                    cell = new PdfPCell(new Phrase(city + ", " + state + " " + zipCode, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 2;
                    table.AddCell(cell);
                }

                if (!string.IsNullOrEmpty(address2))
                {
                    cell = new PdfPCell(new Phrase(address2, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 2;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("Project#: ", font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(jobNumber, font_Reguar_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);
                if (!string.IsNullOrEmpty(address2))
                {
                    cell = new PdfPCell(new Phrase(""));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(city + ", " + state + " " + zipCode, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 2;
                    table.AddCell(cell);
                }
                else
                {
                    cell = new PdfPCell(new Phrase(""));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 3;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("PO#: ", font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(poNumber, font_Reguar_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                if (!string.IsNullOrEmpty(companyNameCheck))
                {
                    cell = new PdfPCell(new Phrase("Attn: ", font_Bold_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(contactAttention, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 2;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Desc: ", font_Bold_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(ProjectDescription, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Rowspan = 2;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("From:", font_Bold_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(from, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                }
                else
                {
                    cell = new PdfPCell(new Phrase("From:", font_Bold_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(from, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 2;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Desc: ", font_Bold_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(ProjectDescription, font_Reguar_11));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Rowspan = 2;
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("RE: " + subject, font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);

                document.Add(table);

                PdfPTable tableContent = new PdfPTable(1);
                tableContent.WidthPercentage = 100;
                string content = emailMessage;
                subject = content;
                tableContent = AddUL(content);
                tableContent.SplitLate = false;
                document.Add(tableContent);

                table = new PdfPTable(5);
                table.WidthPercentage = 100;

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);
                document.Add(table);

                PdfPTable tableAttachment = new PdfPTable(4);
                //tableAttachment.SetWidths(new float[] { 10, 35, 55 });
                tableAttachment.SetWidths(new float[] { 15, 15, 15, 55 });
                tableAttachment.WidthPercentage = 100;

                bool isEnclosure = false;
                int documentEncloserCount = 0;
                if (jobTransmittalEmail.JobTransmittalAttachments != null && jobTransmittalEmail.JobTransmittalAttachments.Count > 0)
                {
                    isEnclosure = true;
                }
                if (!isEnclosure && jobTransmittalEmail.JobTransmittalJobDocuments != null && jobTransmittalEmail.JobTransmittalJobDocuments.Count > 0)
                {
                    isEnclosure = true;
                }
                documentEncloserCount = (jobTransmittalEmail.JobTransmittalAttachments != null ? jobTransmittalEmail.JobTransmittalAttachments.Count : 0) + (jobTransmittalEmail.JobTransmittalJobDocuments != null ? jobTransmittalEmail.JobTransmittalJobDocuments.Count : 0);

                PdfPCell cellAttachment = new PdfPCell();
                if (isEnclosure)
                {
                    cellAttachment = new PdfPCell(new Paragraph("Enclosure (" + documentEncloserCount + ")", font_Bold_11));
                    cellAttachment.Colspan = 5;
                    cellAttachment.Border = PdfPCell.NO_BORDER;
                    cellAttachment.PaddingBottom = 10;
                    cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                    tableAttachment.AddCell(cellAttachment);
                }

                int attachementCount = 0;
                string permitCode = string.Empty;
                if (jobTransmittalEmail.JobTransmittalAttachments != null)
                {
                    foreach (JobTransmittalAttachment item in jobTransmittalEmail.JobTransmittalAttachments)
                    {
                        if (attachementCount == 0)
                        {
                            cellAttachment = new PdfPCell(new Paragraph("Name", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("A#/LOC/VIO#", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("Application Type", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("Description", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph(item.Name, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("", font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("", font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("", font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            attachementCount = attachementCount + 1;
                        }
                        else
                        {
                            cellAttachment = new PdfPCell(new Paragraph(item.Name, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("", font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("", font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("", font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);
                        }
                    }
                }

                if (jobTransmittalEmail.JobTransmittalJobDocuments != null)
                {
                    var jobDocumentIds = jobTransmittalEmail.JobTransmittalJobDocuments.Select(x => new
                    {
                        IdJobDocument = x.IdJobDocument ?? 0,
                        JobDocument = rpoContext.JobDocuments.FirstOrDefault(c => c.Id == x.IdJobDocument),
                        Copies = x.Copies,
                        Code = (from ab in rpoContext.DocumentMasters
                                from cd in rpoContext.JobDocuments
                                where ab.Id == cd.IdDocument
                               && cd.Id == x.IdJobDocument
                                select ab.Code).FirstOrDefault()

                    }).ToList();
                    string jobAppValLoc = string.Empty;
                    foreach (var item in jobDocumentIds)
                    {
                        if (attachementCount == 0)
                        {
                            cellAttachment = new PdfPCell(new Paragraph("Name", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("A#/LOC/VIO#", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("Application Type", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph("Description", font_Bold_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            cellAttachment = new PdfPCell(new Paragraph(item.Code, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            jobAppValLoc = item.JobDocument != null && item.JobDocument.JobApplication != null ? item.JobDocument.JobApplication.ApplicationNumber : (item.JobDocument.JobViolation != null ? item.JobDocument.JobViolation.SummonsNumber : string.Empty);
                            cellAttachment = new PdfPCell(new Paragraph(jobAppValLoc, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            var applicationTypeId = item.JobDocument != null && item.JobDocument.JobApplication != null ? item.JobDocument.JobApplication.IdJobApplicationType : null;
                            string applicationType = string.Empty;
                            if (applicationTypeId != null)
                            {
                                applicationType = rpoContext.JobApplicationTypes.Where(x => x.Id == applicationTypeId).Select(x => x.Description).SingleOrDefault();
                            }
                            cellAttachment = new PdfPCell(new Paragraph(applicationType, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            if (item.Code == "VARPMT")
                            {
                                string[] multiArray = item.JobDocument.DocumentDescription.Split(new Char[] { '|', '\n', '\t' });
                                var varPMTDescription = multiArray.ToList();
                                for (int i = 0; i < varPMTDescription.Count; i++)
                                {
                                    if (varPMTDescription[i].Contains("Work Description:"))
                                    {
                                        varPMTDescription.RemoveAt(i);
                                    }
                                    if (varPMTDescription[i].Contains("AHV Reference Number:"))
                                    {
                                        varPMTDescription.RemoveAt(i);
                                    }
                                }
                                cellAttachment = new PdfPCell(new Paragraph(String.Join("|", varPMTDescription), font_Reguar_11));
                                cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                                cellAttachment.Padding = 3;
                                tableAttachment.AddCell(cellAttachment);

                            }
                            else
                            {
                                cellAttachment = new PdfPCell(new Paragraph(item.JobDocument.DocumentDescription, font_Reguar_11));
                                cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                                cellAttachment.Padding = 3;
                                tableAttachment.AddCell(cellAttachment);
                            }
                            attachementCount = attachementCount + 1;
                            permitCode += item.JobDocument.PermitNumber != null ? item.JobDocument.PermitNumber.TrimEnd() + ", " : string.Empty;

                        }
                        else
                        {
                            cellAttachment = new PdfPCell(new Paragraph(item.Code, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            jobAppValLoc = item.JobDocument != null && item.JobDocument.JobApplication != null ? item.JobDocument.JobApplication.ApplicationNumber : (item.JobDocument.JobViolation != null ? item.JobDocument.JobViolation.SummonsNumber : string.Empty);
                            cellAttachment = new PdfPCell(new Paragraph(jobAppValLoc, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            var applicationTypeId = item.JobDocument != null && item.JobDocument.JobApplication != null ? item.JobDocument.JobApplication.IdJobApplicationType : null;
                            string applicationType = string.Empty;
                            if (applicationTypeId != null)
                            {
                                applicationType = rpoContext.JobApplicationTypes.Where(x => x.Id == applicationTypeId).Select(x => x.Description).SingleOrDefault();
                            }
                            cellAttachment = new PdfPCell(new Paragraph(applicationType, font_Reguar_11));
                            cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellAttachment.Padding = 3;
                            tableAttachment.AddCell(cellAttachment);

                            if (item.Code == "VARPMT")
                            {
                                string[] multiArray = item.JobDocument.DocumentDescription.Split(new Char[] { '|', '\n', '\t' });
                                var varPMTDescription = multiArray.ToList();
                                for (int i = 0; i < varPMTDescription.Count; i++)
                                {
                                    if (varPMTDescription[i].Contains("Work Description:"))
                                    {
                                        varPMTDescription.RemoveAt(i);
                                    }
                                    if (varPMTDescription[i].Contains("AHV Reference Number:"))
                                    {
                                        varPMTDescription.RemoveAt(i);
                                    }
                                }
                                cellAttachment = new PdfPCell(new Paragraph(String.Join("|", varPMTDescription), font_Reguar_11));
                                cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                                cellAttachment.Padding = 3;
                                tableAttachment.AddCell(cellAttachment);

                            }
                            else
                            {
                                cellAttachment = new PdfPCell(new Paragraph(item.JobDocument.DocumentDescription, font_Reguar_11));
                                cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
                                cellAttachment.Padding = 3;
                                tableAttachment.AddCell(cellAttachment);
                            }
                            permitCode += item.JobDocument.PermitNumber != null ? item.JobDocument.PermitNumber.TrimEnd() + ", " : string.Empty;
                        }
                    }
                }

                //cell = new PdfPCell(tableAttachment);
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.Border = PdfPCell.NO_BORDER;
                //cell.Colspan = 5;
                //table.AddCell(cell);
                document.Add(tableAttachment);

                table = new PdfPTable(5);
                table.WidthPercentage = 100;
                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);


                int ccCount = 0;
                if (jobTransmittalEmail.JobTransmittalCCs != null)
                {
                    foreach (JobTransmittalCC transmittalCC in jobTransmittalEmail.JobTransmittalCCs)
                    {
                        Contact transmitalContact = null;
                        Employee transmitalEmployee = null;
                        if (transmittalCC.IdContact != null)
                        {
                            transmitalContact = rpoContext.Contacts.FirstOrDefault(x => x.Id == transmittalCC.IdContact);
                        }
                        else
                        {
                            transmitalEmployee = rpoContext.Employees.FirstOrDefault(x => x.Id == transmittalCC.IdEmployee);
                        }

                        if (transmitalContact != null)
                        {
                            if (ccCount == 0)
                            {
                                string contactcc = transmitalContact != null ? transmitalContact.FirstName + (transmitalContact.LastName != null ? " " + transmitalContact.LastName : string.Empty) : string.Empty;
                                cell = new PdfPCell(new Paragraph("cc: " + contactcc, font_Italic_11));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.NO_BORDER;
                                cell.Colspan = 5;
                                table.AddCell(cell);
                                ccCount = ccCount + 1;
                            }
                            else
                            {
                                string contactcc = transmitalContact != null ? transmitalContact.FirstName + (transmitalContact.LastName != null ? " " + transmitalContact.LastName : string.Empty) : string.Empty;
                                cell = new PdfPCell(new Paragraph(contactcc, font_Italic_11));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.NO_BORDER;
                                cell.Colspan = 5;
                                table.AddCell(cell);
                            }
                        }
                        else if (transmitalEmployee != null)
                        {
                            if (ccCount == 0)
                            {
                                string employeecc = transmitalEmployee != null ? transmitalEmployee.FirstName + (transmitalEmployee.LastName != null ? " " + transmitalEmployee.LastName : string.Empty) : string.Empty;
                                cell = new PdfPCell(new Paragraph("cc: " + employeecc, font_Italic_11));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.NO_BORDER;
                                cell.Colspan = 5;
                                table.AddCell(cell);
                                ccCount = ccCount + 1;
                            }
                            else
                            {
                                string employeecc = transmitalEmployee != null ? transmitalEmployee.FirstName + (transmitalEmployee.LastName != null ? " " + transmitalEmployee.LastName : string.Empty) : string.Empty;
                                cell = new PdfPCell(new Paragraph(employeecc, font_Italic_11));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.NO_BORDER;
                                cell.Colspan = 5;
                                table.AddCell(cell);
                            }
                        }
                    }
                }
                if (emailType.Trim() == "DOT Permit(s)  with Noise Mitigation Stipulation" || emailType.Trim() == "Permit(s) - DOT" || emailType.Trim() == "Full Closing Permit(s) - DOT" && !string.IsNullOrEmpty(permitCode))
                {
                    string input = " " + permitCode.Remove(permitCode.Length - 2);
                    char delimiter = ',';
                    var allParts = input.Split(delimiter);
                    string result = allParts.Select((item, index) => (index != 0 && (index + 1) % 3 == 0)
                                      ? item + delimiter + Environment.NewLine.PadRight(18)
                                        : index != allParts.Count() - 1 ? item + delimiter : item)
                                        .Aggregate((i, j) => i + j);
                    cell = new PdfPCell(new Paragraph("Permit# - " + result, new Font(Font.FontFamily.HELVETICA, 11)));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 5;
                    table.AddCell(cell);
                }

                HTMLWorker worker = new HTMLWorker(document);
                worker.SetStyleSheet(styles);

                document.Add(table);
                document.Close();
                writer.Close();
            }
        }

        /// <summary>
        /// Formats the specified job transmittal.
        /// </summary>
        /// <param name="jobTransmittal">The job transmittal.</param>
        /// <returns>JobTransmittalDTO.</returns>
        private JobTransmittalDTO Format(JobTransmittal jobTransmittal)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new JobTransmittalDTO
            {
                Id = jobTransmittal.Id,
                IdJob = jobTransmittal.IdJob,
                JobNumber = jobTransmittal.Job != null ? jobTransmittal.Job.JobNumber : string.Empty,
                TransmittalNumber = jobTransmittal.TransmittalNumber,
                IdFromEmployee = jobTransmittal.IdFromEmployee,
                FromEmployee = jobTransmittal.FromEmployee != null ? jobTransmittal.FromEmployee.FirstName + " " + jobTransmittal.FromEmployee.LastName : string.Empty,
                IdToCompany = jobTransmittal.IdToCompany,
                ToCompany = jobTransmittal.ToCompany != null ? jobTransmittal.ToCompany.Name : string.Empty,
                IdContactAttention = jobTransmittal.IdContactAttention,
                ContactAttention = jobTransmittal.ContactAttention != null ? jobTransmittal.ContactAttention.FirstName + " " + jobTransmittal.ContactAttention.LastName + (jobTransmittal.ToCompany != null ? " (" + jobTransmittal.ToCompany.Name + ")" : string.Empty) : string.Empty,
                IdTransmissionType = jobTransmittal.IdTransmissionType,
                TransmissionType = jobTransmittal.TransmissionType != null ? jobTransmittal.TransmissionType.Name : string.Empty,
                IdEmailType = jobTransmittal.IdEmailType,
                EmailType = jobTransmittal.EmailType != null ? jobTransmittal.EmailType.Name : string.Empty,
                SentDate = jobTransmittal.SentDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTransmittal.SentDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTransmittal.SentDate,
                SentTimeStamp = jobTransmittal.SentDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTransmittal.SentDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)).ToString("hh:mm tt") : string.Empty,
                IdSentBy = jobTransmittal.IdSentBy,
                SentBy = jobTransmittal.SentBy != null ? jobTransmittal.SentBy.FirstName + " " + jobTransmittal.SentBy.LastName : string.Empty,
                EmailMessage = jobTransmittal.EmailMessage,
                EmailSubject = jobTransmittal.EmailSubject,
                IsAdditionalAtttachment = jobTransmittal.IsAdditionalAtttachment,
                IsEmailSent = jobTransmittal.IsEmailSent,
                IdTask = jobTransmittal.IdTask,
                IdChecklistItem = jobTransmittal.IdChecklistItem,/* As part of Update Database Columns for checklist*/
                IsDraft = jobTransmittal.IsDraft,
                TaskNumber = jobTransmittal.Task != null ? jobTransmittal.Task.TaskNumber : string.Empty,
                CreatedBy = jobTransmittal.CreatedBy,
                LastModifiedBy = jobTransmittal.LastModifiedBy,
                CreatedByEmployeeName = jobTransmittal.CreatedByEmployee != null ? jobTransmittal.CreatedByEmployee.FirstName + " " + jobTransmittal.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = jobTransmittal.LastModifiedByEmployee != null ? jobTransmittal.LastModifiedByEmployee.FirstName + " " + jobTransmittal.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = jobTransmittal.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTransmittal.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTransmittal.CreatedDate,
                LastModifiedDate = jobTransmittal.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTransmittal.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTransmittal.LastModifiedDate,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="jobTransmittal">The job transmittal.</param>
        /// <returns>JobTransmittalDetail.</returns>
        private JobTransmittalDetail FormatDetails(JobTransmittal jobTransmittal)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            string dropBoxFilePath = Properties.Settings.Default.DropboxExternalUrl + "/" + Convert.ToString(jobTransmittal.IdJob);

            return new JobTransmittalDetail
            {
                Id = jobTransmittal.Id,
                IdJob = jobTransmittal.IdJob,
                JobNumber = jobTransmittal.Job != null ? jobTransmittal.Job.JobNumber : string.Empty,
                TransmittalNumber = jobTransmittal.TransmittalNumber,
                IdFromEmployee = jobTransmittal.IdFromEmployee,
                FromEmployee = jobTransmittal.FromEmployee != null ? jobTransmittal.FromEmployee.FirstName + " " + jobTransmittal.FromEmployee.LastName : string.Empty,
                IdToCompany = jobTransmittal.IdToCompany,
                ToCompany = jobTransmittal.ToCompany != null ? jobTransmittal.ToCompany.Name : string.Empty,
                IdContactAttention = jobTransmittal.IdContactAttention,
                ContactAttention = jobTransmittal.ContactAttention != null ? jobTransmittal.ContactAttention.FirstName + " " + jobTransmittal.ContactAttention.LastName : string.Empty,
                IdTransmissionType = jobTransmittal.IdTransmissionType,
                TransmissionType = jobTransmittal.TransmissionType != null ? jobTransmittal.TransmissionType.Name : string.Empty,
                IdEmailType = jobTransmittal.IdEmailType,
                EmailType = jobTransmittal.EmailType != null ? jobTransmittal.EmailType.Name : string.Empty,
                SentDate = jobTransmittal.SentDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTransmittal.SentDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTransmittal.SentDate,
                SentTimeStamp = jobTransmittal.SentDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTransmittal.SentDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)).ToString("hh:mm tt") : string.Empty,

                IdSentBy = jobTransmittal.IdSentBy,
                SentBy = jobTransmittal.SentBy != null ? jobTransmittal.SentBy.FirstName + " " + jobTransmittal.SentBy.LastName : string.Empty,
                EmailMessage = jobTransmittal.EmailMessage,
                EmailSubject = jobTransmittal.EmailSubject,
                IsAdditionalAtttachment = jobTransmittal.IsAdditionalAtttachment,
                IsEmailSent = jobTransmittal.IsEmailSent,
                IdTask = jobTransmittal.IdTask,
                IdChecklistItem = jobTransmittal.IdChecklistItem,/* As part of Update Database Columns for checklist*/
                IsDraft = jobTransmittal.IsDraft,
                TaskNumber = jobTransmittal.Task != null ? jobTransmittal.Task.TaskNumber : string.Empty,
                //JobTransmittalCCs = jobTransmittal.JobTransmittalCCs.Select(x => new JobTransmittalCCDetails
                //{
                //    Id = x.Id,
                //    IdContact = x.IdContact != null && x.IdContact > 0 ? "C" + x.IdContact : "E" + x.IdEmployee,
                //    //IdContact = x.IdContact != null && x.IdContact > 0 ? x.IdContact : x.IdEmployee,
                //    IsContact = x.IdContact != null && x.IdContact > 0 ? true : false,
                //    IdJobTransmittal = x.IdJobTransmittal,
                //    ItemName = x.Contact != null ? x.Contact.FirstName + " " + x.Contact.LastName + (x.Contact.Company != null ? " (" + x.Contact.Company.Name + ")" : string.Empty) : (x.Employee != null ? x.Employee.FirstName + " " + x.Employee.LastName + " (RPO INC)" : string.Empty)
                //}),
                JobTransmittalCCs = jobTransmittal.JobTransmittalCCs.Select(x => (x.IdContact != null && x.IdContact > 0 ? x.IdGroup + "_C_" + x.IdContact : x.IdGroup + "_E_" + x.IdEmployee)).ToList(),
                JobTransmittalAttachments = jobTransmittal.JobTransmittalAttachments.Select(x => new JobTransmittalAttachmentDetails
                {
                    Id = x.Id,
                    DocumentPath = APIUrl + "/" + Properties.Settings.Default.JobTrasmittalAttachmentsPath + "/" + x.IdJobTransmittal + "_" + x.Id + "_" + (x.DocumentPath.Contains("%23") ? Uri.UnescapeDataString(x.DocumentPath) : x.DocumentPath),
                    //DocumentPath = APIUrl + "/" + Properties.Settings.Default.JobTrasmittalAttachmentsPath + "/" + x.IdJobTransmittal + "_" + x.Id + "_" + x.DocumentPath,
                    IdJobTransmittal = x.IdJobTransmittal,
                    Name = x.Name
                }),
                JobTransmittalJobDocuments = jobTransmittal.JobTransmittalJobDocuments.Select(x => new JobTransmittalJobDocumentDetails
                {
                    Id = x.Id,

                    DocumentPath = dropBoxFilePath + "/" + x.IdJobDocument + "_" + x.DocumentPath,
                    // DocumentPath = APIUrl + "/" + Properties.Settings.Default.TransmittalJobDocumentPath + "/" + x.IdJobTransmittal + "_" + x.IdJobDocument + "_" + x.DocumentPath,
                    IdJobTransmittal = x.IdJobTransmittal,
                    IdJobDocument = x.IdJobDocument,
                    Name = x.JobDocument != null ? x.JobDocument.DocumentName : string.Empty,
                    Copies = x.Copies,
                    IdDocument = x.JobDocument != null ? x.JobDocument.IdDocument : 0,
                    DocumentName = x.JobDocument != null ? x.JobDocument.DocumentName : string.Empty,
                    DocumentDescription = x.JobDocument != null ? x.JobDocument.DocumentDescription : string.Empty,
                    DocumentCode = x.JobDocument != null && x.JobDocument.DocumentMaster != null ? x.JobDocument.DocumentMaster.Code : string.Empty,
                    CreatedByEmployeeName = x.JobDocument != null && x.JobDocument.CreatedByEmployee != null ? x.JobDocument.CreatedByEmployee.FirstName + " " + x.JobDocument.CreatedByEmployee.LastName : string.Empty,
                    CreatedDate = x.JobDocument != null && x.JobDocument.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.JobDocument.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : x.JobDocument.CreatedDate,
                    LastModifiedByEmployeeName = x.JobDocument != null && x.JobDocument.LastModifiedByEmployee != null ? x.JobDocument.LastModifiedByEmployee.FirstName + " " + x.JobDocument.LastModifiedByEmployee.LastName : (x.JobDocument != null && x.JobDocument.CreatedByEmployee != null ? x.JobDocument.CreatedByEmployee.FirstName + " " + x.JobDocument.CreatedByEmployee.LastName : string.Empty),
                    LastModifiedDate = x.JobDocument != null && x.JobDocument.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.JobDocument.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (x.JobDocument != null && x.JobDocument.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.JobDocument.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : x.JobDocument.CreatedDate),
                    ApplicationNumber = x.JobDocument != null && x.JobDocument.JobApplication != null ? x.JobDocument.JobApplication.ApplicationNumber : string.Empty,
                    AppplicationType = x.JobDocument != null && x.JobDocument.JobApplication != null ? x.JobDocument.JobApplication.JobApplicationType != null ? x.JobDocument.JobApplication.JobApplicationType.Description : string.Empty : string.Empty,
                }),
                CreatedBy = jobTransmittal.CreatedBy,
                LastModifiedBy = jobTransmittal.LastModifiedBy,
                CreatedByEmployeeName = jobTransmittal.CreatedByEmployee != null ? jobTransmittal.CreatedByEmployee.FirstName + " " + jobTransmittal.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = jobTransmittal.LastModifiedByEmployee != null ? jobTransmittal.LastModifiedByEmployee.FirstName + " " + jobTransmittal.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = jobTransmittal.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTransmittal.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTransmittal.CreatedDate,
                LastModifiedDate = jobTransmittal.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTransmittal.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTransmittal.LastModifiedDate,
            };
        }

        public void TransmittalLog(Exception ex)
        {
            try
            {
                string strLogText = "";

                string innerExceptionmessage = string.Empty;
                string message = string.Empty;
                if (ex.InnerException != null)
                {
                    innerExceptionmessage = ex.InnerException.ToString();
                }
                else if (!string.IsNullOrEmpty(ex.Message))
                {
                    message = ex.Message;
                }

                strLogText += Environment.NewLine + "------------------------------------------------------------";
                strLogText += Environment.NewLine + "ErrorMessage ---\n{0}" + message;
                strLogText += Environment.NewLine + "InnerExceptionMessage ---\n{0}" + innerExceptionmessage;
                strLogText += Environment.NewLine + "------------------------------------------------------------";
                strLogText += Environment.NewLine + "Source ---\n{0}" + ex.Source;
                strLogText += Environment.NewLine + "StackTrace ---\n{0}" + ex.StackTrace;
                strLogText += Environment.NewLine + "TargetSite ---\n{0}" + ex.TargetSite;


                var timeUtc = DateTime.Now;

                string errorLogFilename = "TransmittalErrorLog_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

                string directory = AppDomain.CurrentDomain.BaseDirectory + "Log";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }


                string path = AppDomain.CurrentDomain.BaseDirectory + "Log/" + errorLogFilename;

                if (File.Exists(path))
                {
                    using (StreamWriter stwriter = new StreamWriter(path, true))
                    {
                        stwriter.WriteLine("-------------------Error Log Start-----------as on " + DateTime.Now.ToString("hh:mm tt"));
                        stwriter.WriteLine("Message: " + strLogText.ToString());
                        stwriter.WriteLine("-------------------End----------------------------");
                        stwriter.Close();
                    }
                }
                else
                {
                    StreamWriter stwriter = File.CreateText(path);
                    stwriter.WriteLine("-------------------Error Log Start-----------as on " + DateTime.Now.ToString("hh:mm tt"));
                    stwriter.WriteLine("Message: " + strLogText.ToString());
                    stwriter.WriteLine("-------------------End----------------------------");
                    stwriter.Close();
                }
            }
            catch (Exception)
            {
            }
        }
        private PdfPTable AddUL(string content)
        {
            string fontRegular = HttpContext.Current.Server.MapPath("~\\HTML\\fonts\\AvenirNextLTPro-Regular.ttf");
            BaseFont customfontRegualr = BaseFont.CreateFont(fontRegular, BaseFont.CP1252, BaseFont.EMBEDDED);
            Font font_Reguar_11 = new Font(customfontRegualr, 11, 0);

            PdfPTable tableContent = new PdfPTable(1);
            tableContent.WidthPercentage = 100;
            //List<IElement> elements = XMLWorkerHelper.ParseToElementList(content.Replace("<p>&nbsp;</p>","").Replace("\n\n","\n").Trim(), CSS);
            List<IElement> elements = XMLWorkerHelper.ParseToElementList(content, CSS);
            // List<IElement> elements = HTMLWorker.ParseToList(new StringReader(content), styles1);
            PdfPCell cellContent = new PdfPCell();
            foreach (IElement e in elements)
            {
                if (e is Paragraph)
                {
                    ((Paragraph)e).Font = font_Reguar_11;
                    ((Paragraph)e).SetLeading(0.0f, 1.0f);
                    // ((Paragraph)e).SpacingBefore = 2.75f;
                    ((Paragraph)e).SpacingAfter = 4f;
                }

                if (e is ListItem)
                {
                    ((ListItem)e).Font = font_Reguar_11;
                    ((ListItem)e).SetLeading(0.0f, 1.0f);
                    //  ((ListItem)e).PaddingTop=5f;
                    //  ((ListItem)e).SpacingBefore= 2f;
                    // ((ListItem)e).SpacingAfter = 2f;
                }

                if (e is List)
                {
                    List abc = (List)e;
                    List list = new List(List.UNORDERED, 10f);
                    Chunk bullet = new Chunk("\u2022");
                    list.IndentationLeft = 15f;
                    foreach (var dataItem in abc.Items)
                    {
                        bool isNestedList = false;
                        if (((ListItem)dataItem) != null && ((ListItem)dataItem).Count > 1)
                        {
                            for (int i = 0; i < ((ListItem)dataItem).Count; i++)
                            {
                                if (((ListItem)dataItem)[i].Type == 14)
                                {
                                    isNestedList = true;

                                    string liContent_sub = ((ListItem)dataItem)[0].ToString();
                                    if (liContent_sub != "iTextSharp.text.Paragraph")
                                    {
                                        //Font font_sub = new Font(Font.FontFamily.TIMES_ROMAN, 12);
                                        ListItem listItem_sub = new ListItem(liContent_sub, font_Reguar_11);
                                        listItem_sub.ListSymbol = bullet;
                                        listItem_sub.SetLeading(0.0f, 1.0f);
                                        //  listItem_sub.SpacingBefore = 2f;
                                        //  listItem_sub.SpacingAfter = 2f;
                                        list.Add(listItem_sub);
                                    }

                                }
                            }
                        }

                        if (isNestedList)
                        {
                            for (int i = 0; i < ((ListItem)dataItem).Count; i++)
                            {
                                if (((ListItem)dataItem)[i] is Paragraph)
                                {
                                    ((Paragraph)((ListItem)dataItem)[i]).Font = font_Reguar_11;
                                    string liContent = ((Paragraph)((ListItem)dataItem)[i]).Content;
                                    //Font font = new Font(Font.FontFamily.TIMES_ROMAN, 12);
                                    ListItem listItem = new ListItem(liContent, font_Reguar_11);
                                    listItem.ListSymbol = bullet;
                                    listItem.SetLeading(0.0f, 1.0f);
                                    // listItem.SpacingBefore = 2f;
                                    // listItem.SpacingAfter = 2f;
                                    list.Add(listItem);
                                }

                                if (((ListItem)dataItem)[i] is ListItem)
                                {
                                    ((ListItem)((ListItem)dataItem)[i]).Font = font_Reguar_11;
                                    string liContent = ((ListItem)((ListItem)dataItem)[i]).Content;
                                    //Font font = new Font(Font.FontFamily.TIMES_ROMAN, 12);
                                    ListItem listItem = new ListItem(liContent, font_Reguar_11);
                                    listItem.ListSymbol = bullet;
                                    listItem.SetLeading(0.0f, 1.0f);
                                    // listItem.SpacingBefore = 2f;
                                    //  listItem.SpacingAfter = 2f;
                                    list.Add(listItem);
                                }

                                if (((ListItem)dataItem)[i] is List)
                                {
                                    List abc_sub = (List)((ListItem)dataItem)[i];
                                    List list_sub = new List(List.UNORDERED, 10f);
                                    Chunk bullet_sub = new Chunk("\u2022");
                                    list_sub.IndentationLeft = 15f;
                                    foreach (var item in abc_sub.Items)
                                    {
                                        string liContent_sub = ((ListItem)item).Content;
                                        //Font font_sub = new Font(Font.FontFamily.TIMES_ROMAN, 12);
                                        ListItem listItem_sub = new ListItem(liContent_sub, font_Reguar_11);
                                        listItem_sub.ListSymbol = bullet_sub;
                                        listItem_sub.SetLeading(0.0f, 1.0f);
                                        // listItem_sub.SpacingBefore = 2f;
                                        // listItem_sub.SpacingAfter = 2f;
                                        list_sub.Add(listItem_sub);
                                    }
                                    list.Add(list_sub);
                                }
                                //if (((ListItem)dataItem)[i].IsContent==false)
                                //{

                                //}
                            }
                        }
                        else
                        {
                            string liContent = ((ListItem)dataItem).Content;
                            //Font font = new Font(Font.FontFamily.TIMES_ROMAN, 12);
                            ListItem listItem = new ListItem(liContent, font_Reguar_11);
                            listItem.ListSymbol = bullet;
                            listItem.SetLeading(0.0f, 1.0f);
                            listItem.SpacingBefore = 7f;
                            // listItem.SpacingAfter = 2f;
                            list.Add(listItem);
                        }
                    }
                    cellContent.AddElement((IElement)list);
                    cellContent.AddElement(new Phrase(Chunk.NEWLINE));
                }

                if (e is List) { }
                else
                {
                    cellContent.AddElement((IElement)e);
                    // cellContent.AddElement(new Phrase(Chunk.NEWLINE));
                }
            }
            cellContent.HorizontalAlignment = Element.ALIGN_LEFT;
            cellContent.Border = PdfPCell.NO_BORDER;
            tableContent.AddCell(cellContent);
            //document.Add(tableContent);
            //document.Add(new Paragraph(Chunk.NEWLINE));
            return tableContent;
        }

        private void DeleteJobTransmitalDocuments(int idJobTransmittal)
        {
            try
            {
                var jobTransmittalJobDocuments = this.rpoContext.JobTransmittalJobDocuments.Where(x => x.IdJobTransmittal == idJobTransmittal).ToList();

                if (jobTransmittalJobDocuments != null && jobTransmittalJobDocuments.Any())
                {
                    this.rpoContext.JobTransmittalJobDocuments.RemoveRange(jobTransmittalJobDocuments);

                    rpoContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void DeleteTransamittalAttachments(IEnumerable<int> idattachment)
        {
            try
            {
                var jobTransmittalAttachments = this.rpoContext.JobTransmittalAttachments.Where(x => idattachment.Contains(x.Id));
                if (jobTransmittalAttachments.Any())
                {
                    this.rpoContext.JobTransmittalAttachments.RemoveRange(jobTransmittalAttachments);
                    rpoContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion        
        /// <summary>
        /// Class Header.
        /// </summary>
        /// <seealso cref="iTextSharp.text.pdf.PdfPageEventHelper" />
        public partial class Header : PdfPageEventHelper
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Header"/> class.
            /// </summary>
            /// <param name="FirstLastName">First name of the last.</param>
            /// <param name="proposalName">Name of the proposal.</param>
            public Header(string idJob)
            {
                this.IdJob = idJob;
            }


            public string IdJob { get; set; }

            public override void OnEndPage(PdfWriter writer, Document doc)
            {
                base.OnEndPage(writer, doc);

                int pageNumber = writer.PageNumber;

                var FontColour = new BaseColor(85, 85, 85);
                var MyFont = FontFactory.GetFont("Avenir Next LT Pro", 10, FontColour);

                PdfPTable table = new PdfPTable(1);
                table.WidthPercentage = 100;
                //table.TotalWidth = 592F;
                table.TotalWidth = doc.PageSize.Width - 40f;


                PdfPCell cell = new PdfPCell(new Phrase("Page | " + pageNumber, new Font(MyFont)));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingTop = 10f;
                cell.Colspan = 1;
                table.AddCell(cell);

                table.WriteSelectedRows(0, -1, 0, doc.Bottom, writer.DirectContent);
            }
        }
    }
}