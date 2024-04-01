// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Krunal Pandya
// Created          : 04-18-2018
//
// Last Modified By : Krunal Pandya
// Last Modified On : 04-18-2018
// ***********************************************************************
// <copyright file="JobDocumentController.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Document Controller.</summary>
// ***********************************************************************

using System;
/// <summary>
/// The JobDocument namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobDocument
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using DataTable;
    using DocumentFields;
    using Enums;
    using Filters;
    using HtmlAgilityPack;
    using Models;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Tools;
    using iTextSharp.text.pdf;
    using System.Globalization;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using Microsoft.ApplicationBlocks.Data;
    using SODA;
    using Model.Models.Enums;
    using Newtonsoft.Json;

    /// <summary>
    /// Class Job Document Controller.
    /// </summary>
    public class JobDocumentController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the list of job documents.
        /// </summary>
        /// <param name="jobDocumentDataTableParameters">The job document data table parameters.</param>
        /// <returns>Gets the list of job documents.</returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetJobDocuments([FromUri] JobDocumentDataTableParameters jobDocumentDataTableParameters)
        {
            IQueryable<JobDocument> jobDocuments = (dynamic)null;
            if (jobDocumentDataTableParameters.isTransmittal)
            {
                jobDocuments = this.rpoContext.JobDocuments.
                Include("DocumentMaster").
                ///Include("JobDocumentFields.DocumentField.Field").
                Include("JobApplication.JobApplicationType.Parent").
                Include("CreatedByEmployee").
                Include("LastModifiedByEmployee").
                Where(x => x.IdJob == jobDocumentDataTableParameters.IdJob && x.DocumentPath != null).AsQueryable();
            }
            else
            {
                jobDocuments = this.rpoContext.JobDocuments.
                Include("DocumentMaster").
                ///Include("JobDocumentFields.DocumentField.Field").
                Include("JobApplication.JobApplicationType.Parent").
                Include("LastModifiedByEmployee").
                Where(x => x.IdJob == jobDocumentDataTableParameters.IdJob).AsQueryable();
            }
            var recordsTotal = jobDocuments.Count();
            var recordsFiltered = recordsTotal;

            var result = jobDocuments
                .AsEnumerable()
                .Select(c => this.FormatDetails(c))
                .AsQueryable()
                .DataTableParameters(jobDocumentDataTableParameters, out recordsFiltered)
                .ToArray();

            return this.Ok(new DataTableResponse
            {
                Draw = jobDocumentDataTableParameters.Draw,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal,
                Data = result.OrderByDescending(x => x.LastModifiedDate)
            });

        }

        /// <summary>
        /// Gets the list of job documents.
        /// </summary>
        /// <param name="jobDocumentDataTableParameters">The job document data table parameters.</param>
        /// <returns>Gets the list of job documents.</returns>
        //[Route("api/JobDocumentsListPost")]
        //[HttpPost]
        //[ResponseType(typeof(DataTableParameters))]
        //[Authorize]
        //[RpoAuthorize]
        //public IHttpActionResult GetJobDocumentsPostList(JobDocumentDataTableParameters dataTableParameters)
        //{
        //    var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
        //    if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewContact)
        //          || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddContact)
        //          || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteContact))
        //    {
        //        //var contacts = rpoContext.Contacts.Include(p => p.ContactLicenses).AsQueryable();


        //        string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();
        //        string Firstname = string.Empty;
        //        string Lastname = string.Empty;
        //        //if (dataTableParameters.GlobalSearchType != null && dataTableParameters.GlobalSearchType > 0)
        //        //{
        //        //    string[] strArray = dataTableParameters.GlobalSearchText.Split(' ');
        //        //    if (strArray.Count() > 0)
        //        //    {
        //        //        Firstname = strArray[0].Trim();
        //        //    }
        //        //    if (strArray.Count() > 1)
        //        //    {
        //        //        Lastname = strArray[1].Trim();
        //        //    }
        //        //}

        //        DataSet ds = new DataSet();

        //        SqlParameter[] spParameter = new SqlParameter[7];

        //        spParameter[0] = new SqlParameter("@PageIndex", SqlDbType.Int);
        //        spParameter[0].Direction = ParameterDirection.Input;
        //        spParameter[0].Value = dataTableParameters.Start;

        //        spParameter[1] = new SqlParameter("@PageSize", SqlDbType.Int);
        //        spParameter[1].Direction = ParameterDirection.Input;
        //        spParameter[1].Value = dataTableParameters.Length;

        //        spParameter[2] = new SqlParameter("@Column", SqlDbType.NVarChar, 50);
        //        spParameter[2].Direction = ParameterDirection.Input;
        //        spParameter[2].Value = dataTableParameters.OrderedColumn != null ? dataTableParameters.OrderedColumn.Column : null;

        //        spParameter[3] = new SqlParameter("@Dir", SqlDbType.VarChar, 50);
        //        spParameter[3].Direction = ParameterDirection.Input;
        //        spParameter[3].Value = dataTableParameters.OrderedColumn != null ? dataTableParameters.OrderedColumn.Dir : null;


        //        spParameter[4] = new SqlParameter("@Search", SqlDbType.NVarChar, 50);
        //        spParameter[4].Direction = ParameterDirection.Input;
        //        spParameter[4].Value = !string.IsNullOrEmpty(dataTableParameters.Search) ? dataTableParameters.Search : string.Empty;

        //        spParameter[5] = new SqlParameter("@Idjob", SqlDbType.Int);
        //        spParameter[5].Direction = ParameterDirection.Input;
        //        spParameter[5].Value = dataTableParameters.IdJob;

        //        spParameter[6] = new SqlParameter("@RecordCount", SqlDbType.Int);
        //        spParameter[6].Direction = ParameterDirection.Output;

        //        ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "JobDocuments_List", spParameter);



        //        int totalrecord = 0;
        //        int Recordcount = 0;
        //        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
        //        {
        //            totalrecord = int.Parse(ds.Tables[0].Rows[0][0].ToString());
        //            Recordcount = int.Parse(spParameter[6].SqlValue.ToString());

        //        }
        //        if (ds.Tables.Count > 1)
        //        {
        //            ds.Tables[1].TableName = "Data";
        //        }
        //        return Ok(new DataTableResponse
        //        {
        //            Draw = dataTableParameters.Start,
        //            RecordsFiltered = Recordcount,
        //            RecordsTotal = totalrecord,
        //            Data = ds.Tables[1]
        //        });
        //    }
        //    else
        //    {
        //        throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
        //    }
        //}

        /// <summary>
        /// Gets the list of job documents.
        /// </summary>
        /// <param name="jobDocumentDataTableParameters">The job document data table parameters.</param>
        /// <returns>Gets the list of job documents.</returns>
        [Route("api/JobDocumentsListPost")]
        [HttpPost]
        [ResponseType(typeof(DataTableParameters))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetJobDocumentsPostListforchecklist(JobDocumentDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewContact)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddContact)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteContact))
            {
                //var contacts = rpoContext.Contacts.Include(p => p.ContactLicenses).AsQueryable();


                string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();
                string Firstname = string.Empty;
                string Lastname = string.Empty;
                //if (dataTableParameters.GlobalSearchType != null && dataTableParameters.GlobalSearchType > 0)
                //{
                //    string[] strArray = dataTableParameters.GlobalSearchText.Split(' ');
                //    if (strArray.Count() > 0)
                //    {
                //        Firstname = strArray[0].Trim();
                //    }
                //    if (strArray.Count() > 1)
                //    {
                //        Lastname = strArray[1].Trim();
                //    }
                //}

                DataSet ds = new DataSet();

                SqlParameter[] spParameter = new SqlParameter[9];

                spParameter[0] = new SqlParameter("@PageIndex", SqlDbType.Int);
                spParameter[0].Direction = ParameterDirection.Input;
                spParameter[0].Value = dataTableParameters.Start;

                spParameter[1] = new SqlParameter("@PageSize", SqlDbType.Int);
                spParameter[1].Direction = ParameterDirection.Input;
                spParameter[1].Value = dataTableParameters.Length;

                spParameter[2] = new SqlParameter("@Column", SqlDbType.NVarChar, 50);
                spParameter[2].Direction = ParameterDirection.Input;
                spParameter[2].Value = dataTableParameters.OrderedColumn != null ? dataTableParameters.OrderedColumn.Column : null;

                spParameter[3] = new SqlParameter("@Dir", SqlDbType.VarChar, 50);
                spParameter[3].Direction = ParameterDirection.Input;
                spParameter[3].Value = dataTableParameters.OrderedColumn != null ? dataTableParameters.OrderedColumn.Dir : null;


                spParameter[4] = new SqlParameter("@Search", SqlDbType.NVarChar, 50);
                spParameter[4].Direction = ParameterDirection.Input;
                spParameter[4].Value = !string.IsNullOrEmpty(dataTableParameters.Search) ? dataTableParameters.Search : string.Empty;

                spParameter[5] = new SqlParameter("@Idjob", SqlDbType.Int);
                spParameter[5].Direction = ParameterDirection.Input;
                spParameter[5].Value = dataTableParameters.IdJob;
                //new added for checklist
                spParameter[6] = new SqlParameter("@IdJobchecklistItemDetails", SqlDbType.Int);
                spParameter[6].Direction = ParameterDirection.Input;
                spParameter[6].Value = dataTableParameters.IdJobchecklistItemDetails;

                spParameter[7] = new SqlParameter("@IdJobPlumbinginspections", SqlDbType.Int);
                spParameter[7].Direction = ParameterDirection.Input;
                spParameter[7].Value = dataTableParameters.IdJobPlumbinginspections;

                spParameter[8] = new SqlParameter("@RecordCount", SqlDbType.Int);
                spParameter[8].Direction = ParameterDirection.Output;

                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "JobDocuments_List_Checklist", spParameter);

                int totalrecord = 0;
                int Recordcount = 0;
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    totalrecord = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                    Recordcount = int.Parse(spParameter[8].SqlValue.ToString());

                }
                if (ds.Tables.Count > 1)
                {
                    ds.Tables[1].TableName = "Data";
                }
                return Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Start,
                    RecordsFiltered = Recordcount,
                    RecordsTotal = totalrecord,
                    Data = ds.Tables[1]
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }


        /// <summary>
        /// Gets the list of job documents.
        /// </summary>
        /// <param name="jobDocumentDataTableParameters">The job document data table parameters.</param>
        /// <returns>Gets the list of job documents.</returns>
        [Route("api/JobDocumentsListPostChecklistItemwise")]
        [HttpPost]
        [ResponseType(typeof(DataTableParameters))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetJobDocumentsPostListChecklistItemwise(JobDocumentDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewContact)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddContact)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteContact))
            {
                //var contacts = rpoContext.Contacts.Include(p => p.ContactLicenses).AsQueryable();


                string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();
                string Firstname = string.Empty;
                string Lastname = string.Empty;
                //if (dataTableParameters.GlobalSearchType != null && dataTableParameters.GlobalSearchType > 0)
                //{
                //    string[] strArray = dataTableParameters.GlobalSearchText.Split(' ');
                //    if (strArray.Count() > 0)
                //    {
                //        Firstname = strArray[0].Trim();
                //    }
                //    if (strArray.Count() > 1)
                //    {
                //        Lastname = strArray[1].Trim();
                //    }
                //}

                DataSet ds = new DataSet();

                SqlParameter[] spParameter = new SqlParameter[9];

                spParameter[0] = new SqlParameter("@PageIndex", SqlDbType.Int);
                spParameter[0].Direction = ParameterDirection.Input;
                spParameter[0].Value = dataTableParameters.Start;

                spParameter[1] = new SqlParameter("@PageSize", SqlDbType.Int);
                spParameter[1].Direction = ParameterDirection.Input;
                spParameter[1].Value = dataTableParameters.Length;

                spParameter[2] = new SqlParameter("@Column", SqlDbType.NVarChar, 50);
                spParameter[2].Direction = ParameterDirection.Input;
                spParameter[2].Value = dataTableParameters.OrderedColumn != null ? dataTableParameters.OrderedColumn.Column : null;

                spParameter[3] = new SqlParameter("@Dir", SqlDbType.VarChar, 50);
                spParameter[3].Direction = ParameterDirection.Input;
                spParameter[3].Value = dataTableParameters.OrderedColumn != null ? dataTableParameters.OrderedColumn.Dir : null;


                spParameter[4] = new SqlParameter("@Search", SqlDbType.NVarChar, 50);
                spParameter[4].Direction = ParameterDirection.Input;
                spParameter[4].Value = !string.IsNullOrEmpty(dataTableParameters.Search) ? dataTableParameters.Search : string.Empty;

                spParameter[5] = new SqlParameter("@Idjob", SqlDbType.Int);
                spParameter[5].Direction = ParameterDirection.Input;
                spParameter[5].Value = dataTableParameters.IdJob;
                //new added for checklist
                spParameter[6] = new SqlParameter("@IdJobchecklistItemDetails", SqlDbType.Int);
                spParameter[6].Direction = ParameterDirection.Input;
                spParameter[6].Value = dataTableParameters.IdJobchecklistItemDetails;

                spParameter[7] = new SqlParameter("@IdJobPlumbinginspections", SqlDbType.Int);
                spParameter[7].Direction = ParameterDirection.Input;
                spParameter[7].Value = dataTableParameters.IdJobPlumbinginspections;

                spParameter[8] = new SqlParameter("@RecordCount", SqlDbType.Int);
                spParameter[8].Direction = ParameterDirection.Output;

                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "[JobDocuments_List_ChecklistItemWise]", spParameter);

                int totalrecord = 0;
                int Recordcount = 0;
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    totalrecord = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                    Recordcount = int.Parse(spParameter[8].SqlValue.ToString());

                }
                if (ds.Tables.Count > 1)
                {
                    ds.Tables[1].TableName = "Data";
                }
                return Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Start,
                    RecordsFiltered = Recordcount,
                    RecordsTotal = totalrecord,
                    Data = ds.Tables[1]
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the list of job documents for Transmittal.
        /// </summary>
        /// <param name="jobDocumentDataTableParameters">The job document data table parameters.</param>
        /// <returns>Gets the list of job documents for transmittal.</returns>
        [Route("api/JobDocumentsListTransmittal")]
        [HttpPost]
        [ResponseType(typeof(DataTableParameters))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetJobDocumentsPostTransmittalList(JobDocumentDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewContact)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddContact)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteContact))
            {
                //var contacts = rpoContext.Contacts.Include(p => p.ContactLicenses).AsQueryable();


                string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();
                string Firstname = string.Empty;
                string Lastname = string.Empty;
                //if (dataTableParameters.GlobalSearchType != null && dataTableParameters.GlobalSearchType > 0)
                //{
                //    string[] strArray = dataTableParameters.GlobalSearchText.Split(' ');
                //    if (strArray.Count() > 0)
                //    {
                //        Firstname = strArray[0].Trim();
                //    }
                //    if (strArray.Count() > 1)
                //    {
                //        Lastname = strArray[1].Trim();
                //    }
                //}

                DataSet ds = new DataSet();

                SqlParameter[] spParameter = new SqlParameter[5];

                spParameter[0] = new SqlParameter("@PageIndex", SqlDbType.Int);
                spParameter[0].Direction = ParameterDirection.Input;
                spParameter[0].Value = 1;// dataTableParameters.Start;

                spParameter[1] = new SqlParameter("@PageSize", SqlDbType.Int);
                spParameter[1].Direction = ParameterDirection.Input;
                spParameter[1].Value = 10000;// dataTableParameters.Length;


                spParameter[2] = new SqlParameter("@Search", SqlDbType.NVarChar, 50);
                spParameter[2].Direction = ParameterDirection.Input;
                spParameter[2].Value = !string.IsNullOrEmpty(dataTableParameters.Search) ? dataTableParameters.Search : string.Empty;

                spParameter[3] = new SqlParameter("@Idjob", SqlDbType.Int);
                spParameter[3].Direction = ParameterDirection.Input;
                spParameter[3].Value = dataTableParameters.IdJob;

                spParameter[4] = new SqlParameter("@RecordCount", SqlDbType.Int);
                spParameter[4].Direction = ParameterDirection.Output;

                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "JobDocumentsTransmittal_List", spParameter);



                int totalrecord = 0;
                int Recordcount = 0;
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    totalrecord = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                    Recordcount = int.Parse(spParameter[4].SqlValue.ToString());

                }
                if (ds.Tables.Count > 1)
                {
                    ds.Tables[1].TableName = "Data";
                }
                return Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Start,
                    RecordsFiltered = Recordcount,
                    RecordsTotal = totalrecord,
                    Data = ds.Tables[1]
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }


        /// <summary>
        /// Gets the type of the Job Document in detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the type of the Job Document in detail.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobDocumentGetDetailsDTO))]
        public IHttpActionResult GetJobDocument(int id)
        {
            JobDocument jobDocument = rpoContext.JobDocuments.Include("JobDocumentFields.DocumentField.Field").Include("DocumentMaster")
                                        .FirstOrDefault(x => x.Id == id);

            //AddressType addressType = this.rpoContext.AddressTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
            if (jobDocument == null)
            {
                return this.NotFound();
            }

            //return this.Ok(this.FormatDetails(jobDocument));
            return this.Ok(Format(jobDocument));

        }

        /// <summary>
        /// Gets the job document dropdown.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the type of the Job Document list for dropdown.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocuments/dropdown/{idJob}")]
        public IHttpActionResult GetJobDocumentDropdown(int idJob)
        {
            var result = this.rpoContext.JobDocuments.Include("DocumentMaster").Where(x => x.IdJob == idJob).AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = "[" + c.DocumentMaster.Code + "] - " + c.DocumentName + " " + c.DocumentDescription,
                Name = c.DocumentName + " " + c.DocumentDescription,
            }).ToArray();

            return this.Ok(result);
        }

        /// <summary>
        /// Posts the job document.
        /// </summary>
        /// <param name="jobDocumentCreateOrUpdateDTO">The job document create or update dto.</param>
        /// <returns>create and update the job document .</returns>
        [ResponseType(typeof(JobDocumentCreateOrUpdateDTO))]
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult PostJobDocument(JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            JobDocument jobDocument;
            if (jobDocumentCreateOrUpdateDTO.Id == 0)
            {
                if (jobDocumentCreateOrUpdateDTO.IdJobchecklistitemdetails != 0 || jobDocumentCreateOrUpdateDTO.IdJobchecklistitemdetails != null ||
                    jobDocumentCreateOrUpdateDTO.IdJobPlumbingInspections != 0 || jobDocumentCreateOrUpdateDTO.IdJobPlumbingInspections != null)
                {
                    jobDocument = new JobDocument
                    {
                        IdJob = jobDocumentCreateOrUpdateDTO.IdJob,
                        IdDocument = jobDocumentCreateOrUpdateDTO.IdDocument,
                        DocumentName = jobDocumentCreateOrUpdateDTO.DocumentName,
                        DocumentDescription = string.Empty,
                        IsArchived = jobDocumentCreateOrUpdateDTO.IsArchived,
                        IdJobApplication = jobDocumentCreateOrUpdateDTO.IdJobApplication,
                        JobDocumentFor = jobDocumentCreateOrUpdateDTO.JobDocumentFor,
                        IdJobViolation = jobDocumentCreateOrUpdateDTO.IdJobViolation,
                        IdJobApplicationWorkPermitType = jobDocumentCreateOrUpdateDTO.IdJobApplicationWorkPermitType,
                        IdJobchecklistItemDetails = jobDocumentCreateOrUpdateDTO.IdJobchecklistitemdetails,
                        IdJobPlumbingInspections = jobDocumentCreateOrUpdateDTO.IdJobPlumbingInspections
                    };

                    int JobChecklistGroupsID = 0;

                    if (jobDocumentCreateOrUpdateDTO.IdJobPlumbingInspections != null || jobDocumentCreateOrUpdateDTO.IdJobchecklistitemdetails != null)
                    {
                        if (jobDocumentCreateOrUpdateDTO.IdJobPlumbingInspections != null)
                        {
                            JobChecklistGroupsID = rpoContext.JobPlumbingInspection.Find(jobDocumentCreateOrUpdateDTO.IdJobPlumbingInspections).IdJobChecklistGroup;
                        }
                        else if (jobDocumentCreateOrUpdateDTO.IdJobchecklistitemdetails != null)
                        {
                            JobChecklistGroupsID = rpoContext.JobChecklistItemDetails.Find(jobDocumentCreateOrUpdateDTO.IdJobchecklistitemdetails).IdJobChecklistGroup;
                        }
                        var jobchecklistheaderid = rpoContext.JobChecklistGroups.Where(x => x.Id == JobChecklistGroupsID).FirstOrDefault().IdJobCheckListHeader;
                        // var jobchecklistheaderid = rpoContext.JobChecklistGroups.Where(x => x.Id == jobDocumentCreateOrUpdateDTO.IdJobChecklistGroup).FirstOrDefault().IdJobCheckListHeader;

                        JobChecklistHeader jobChecklistHeader = rpoContext.JobChecklistHeaders.Find(jobchecklistheaderid);
                        jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;
                    }
                    rpoContext.SaveChanges();
                    jobDocument.CreatedDate = DateTime.UtcNow;
                    jobDocument.LastModifiedDate = DateTime.UtcNow;
                    if (employee != null)
                    {
                        jobDocument.CreatedBy = employee.Id;
                        jobDocument.LastModifiedBy = employee.Id;
                    }

                    rpoContext.JobDocuments.Add(jobDocument);
                }
                else
                {
                    jobDocument = new JobDocument
                    {
                        IdJob = jobDocumentCreateOrUpdateDTO.IdJob,
                        IdDocument = jobDocumentCreateOrUpdateDTO.IdDocument,
                        DocumentName = jobDocumentCreateOrUpdateDTO.DocumentName,
                        DocumentDescription = string.Empty,
                        IsArchived = jobDocumentCreateOrUpdateDTO.IsArchived,
                        IdJobApplication = jobDocumentCreateOrUpdateDTO.IdJobApplication,
                        JobDocumentFor = jobDocumentCreateOrUpdateDTO.JobDocumentFor,
                        IdJobViolation = jobDocumentCreateOrUpdateDTO.IdJobViolation,
                        IdJobApplicationWorkPermitType = jobDocumentCreateOrUpdateDTO.IdJobApplicationWorkPermitType,
                        IdJobchecklistItemDetails = 0,
                        IdJobPlumbingInspections = 0
                        // IdJobchecklistItemDetails = jobDocumentCreateOrUpdateDTO.IdJobchecklistitemdetails
                    };

                    jobDocument.CreatedDate = DateTime.UtcNow;
                    jobDocument.LastModifiedDate = DateTime.UtcNow;
                    if (employee != null)
                    {
                        jobDocument.CreatedBy = employee.Id;
                        jobDocument.LastModifiedBy = employee.Id;
                    }

                    rpoContext.JobDocuments.Add(jobDocument);
                }
                rpoContext.SaveChanges();
            }
            else
            {
                jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == jobDocumentCreateOrUpdateDTO.Id);
                jobDocument.LastModifiedDate = DateTime.UtcNow;
                jobDocument.IdJobApplication = jobDocumentCreateOrUpdateDTO.IdJobApplication;
                if (employee != null)
                {
                    jobDocument.LastModifiedBy = employee.Id;
                }
                rpoContext.SaveChanges();
            }

            switch ((Document)jobDocumentCreateOrUpdateDTO.IdDocument)
            {
                case Document.TenDAYN62395:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.Generate10DaysNotice(jobDocument.Id, jobDocumentCreateOrUpdateDTO, employee.Id, jobDocumentCreateOrUpdateDTO.IdJob);
                        }
                        else
                        {
                            GenerateJobDocuments.Edit10DaysNotice(jobDocument.Id, jobDocumentCreateOrUpdateDTO, employee.Id, jobDocumentCreateOrUpdateDTO.IdJob);
                        }
                        //GenerateJobDocuments.Generate10DaysNotice(jobDocument.Id, jobDocumentCreateOrUpdateDTO, employee.Id, jobDocumentCreateOrUpdateDTO.IdJob);
                        break;
                    }





                case Document.AI162402:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateAI162402(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditAI162402(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        // GenerateJobDocuments.GenerateAI162402(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.DEP_PERMIT:
                    {
                        GenerateJobDocuments.GenerateDEPPermitDocument(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.TenDayNoticeScannedSigned:
                    {
                        GenerateJobDocuments.Generate10DaysNoticeScannedSigned(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.DOT_Permit:
                    {
                        GenerateJobDocuments.GeneratePermitDocument(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.DOB_PERMIT:
                    {
                        GenerateJobDocuments.GeneratePermitDocument(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.EquipmentUsePermitCard:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateEquipmentUsePermitCard(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditEquipmentUsePermitCard(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateEquipmentUsePermitCard(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.HIC1HomeImprovementContractorAffidavit:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateHIC1HomeImprovementContractorAffidavit(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateHIC1HomeImprovementContractorAffidavit(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }

                case Document.LandmarksCertificateOfAppropriateness:
                    {
                        GenerateJobDocuments.GenerateLandmarksCertificateOfAppropriateness(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.LandmarkCertificateOfNoEffect:
                    {
                        GenerateJobDocuments.GenerateLandmarkCertificateOfNoEffect(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }

                case Document.NarrativeFireProtectionForm:
                    {
                        GenerateJobDocuments.GenerateNarrativeFireProtectionForm(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.Objection:
                    {
                        GenerateJobDocuments.GenerateObjection(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.Survey:
                    {
                        GenerateJobDocuments.GenerateSurvey(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.SewerSelfCertificationForm:
                    {
                        GenerateJobDocuments.GenerateSewerSelfCertificationForm(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }

                case Document.WorkmansCompensationInsuranceForm:
                    {
                        GenerateJobDocuments.GenerateWorkmansCompensationInsuranceForm(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }

                case Document.DOTContactAwardLetter:
                    {
                        GenerateJobDocuments.GenerateDOTContactAwardLetter(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.TechnicalReportFormTR5:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateTechnicalReportFormTR5(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditTechnicalReportFormTR5(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.DOTContactNoticeToProceed:
                    {
                        GenerateJobDocuments.GenerateDOTContactNoticeToProceed(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.DOTOCMCStipulationSheet:
                    {
                        GenerateJobDocuments.GenerateDOTOCMCStipulationSheet(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.DOTPermitExpirationReport:
                    {
                        GenerateJobDocuments.GenerateDOTPermitExpirationReport(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.Drawings:
                    {
                        GenerateJobDocuments.GenerateDrawings(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.Miscellaneous:
                    {
                        GenerateJobDocuments.GenerateMiscellaneous(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.ProfessionalAndOwnerCertificationEasementAgreementOrRestrictiveDeclaration:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateProfessionalAndOwnerCertificationEasementAgreementOrRestrictiveDeclaration(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditProfessionalAndOwnerCertificationEasementAgreementOrRestrictiveDeclaration(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }

                case Document.ProfessionalAndOwnerCertification:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateProfessionalAndOwnerCertification(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditProfessionalAndOwnerCertification(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.AsbestosAssessmentReport:
                    {
                        GenerateJobDocuments.GenerateAsbestosAssessmentReport(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.LetterOfCompletion:
                    {
                        GenerateJobDocuments.GenerateLetterOfCompletion(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }

                case Document.NoticeforProposedBoilerInstallation:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateNoticeforProposedBoilerInstallation(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditNoticeforProposedBoilerInstallation(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        // GenerateJobDocuments.GenerateNoticeforProposedBoilerInstallation(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.EF1EfillingJobApplication:
                    {
                        GenerateJobDocuments.EF1EfillingJobApplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.CertificateOfCorrection:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateCertificateOfCorrection(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditCertificateOfCorrection(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateCertificateOfCorrection(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.CertificateOfCorrection2021:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateCertificateOfCorrection(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditCertificateOfCorrection(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateCertificateOfCorrection(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.CertificateOfCorrection2019:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateCertificateOfCorrection2019(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditCertificateOfCorrection2019(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateCertificateOfCorrection(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.AI1forFinalSurvey:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateAI1forFinalSurvey(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditAI1forFinalSurvey(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateAI1forFinalSurvey(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.Applicationforcertificateofoperation:
                    {
                        GenerateJobDocuments.GenerateApplicationForCertificateOfOperation(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.CertificateOfApplicationforRegistration:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateCertificateOfApplicationForRegistration(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditCertificateOfApplicationForRegistration(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateCertificateOfApplicationForRegistration(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.ScannedDEPRegistrationForDemolitionFormAR300:
                    {
                        GenerateJobDocuments.GenerateScannedDEPRegistrationForDemolitionFormAR300(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.ApplicationforInspectionPriortoDemolition:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateApplicationforInspectionPriortoDemolition(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditApplicationforInspectionPriortoDemolition(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        // GenerateJobDocuments.GenerateApplicationforInspectionPriortoDemolition(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }

                case Document.AfterHoursPermitApplication:
                    {
                        GenerateJobDocuments.GenerateAfterHoursPermitApplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.ApplicationForBestRecommendationForMechanicalMeansDemolition:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateApplicationforBestRecommendationforMechanicalMeansDemolition(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditApplicationforBestRecommendationforMechanicalMeansDemolition(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        // GenerateJobDocuments.GenerateApplicationforBestRecommendationforMechanicalMeansDemolition(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.BuilderPavementApplication:
                    {
                        GenerateJobDocuments.GenerateBuilderPavementApplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.BuilderPavementPlanChecklist:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateBuilderPavementPlanChecklist(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditBuilderPavementPlanChecklist(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }

                case Document.AffidavitAsToRoadwayPavementSidewalkCurbConstruction:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateAffidavitAsToRoadwayPavementAndSidewalkCurbConstruction(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditAffidavitAsToRoadwayPavementAndSidewalkCurbConstruction(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateAffidavitAsToRoadwayPavementAndSidewalkCurbConstruction(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.ApplicationForRoadwaySidewalkPermits:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateApplicationforRoadwayOrSidewalkPermits(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditApplicationforRoadwayOrSidewalkPermits(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.BuildersPavementPlanAuthorizationtotheDeptofTransportation:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateBuildersPavementPlanAuthorizationtotheDeptofTransportation(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditBuildersPavementPlanAuthorizationtotheDeptofTransportation(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }

                case Document.RequestforBuildersPavementFieldInspection:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforBuildersPavementFieldInspection(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestforBuildersPavementFieldInspection(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.DEPRegistrationforDemolitionFormAR:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforDEPRegistrationforDemolitionFormAR(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestforDEPRegistrationforDemolitionFormAR(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.DisabilityInsuranceFormDB:
                    {
                        GenerateJobDocuments.GenerateDisabilityInsuranceFormDB(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.CarbonMonoxideLetter:
                    {
                        GenerateJobDocuments.GenerateRequestforCarbonMonoxideLetter(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.BuildersPavementPlanStatementProfessionalCertificate:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforBuildersPavementPlanStatementProfessionalCertificate(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestforBuildersPavementPlanStatementProfessionalCertificate(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.DSDemolitionSubmittalCertificationForm:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateDSDemolitionSubmittalCertificationForm(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditDSDemolitionSubmittalCertificationForm(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.B45MRequestFireAlarmInspection:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateB45MRequestFireAlarmInspection(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditB45MRequestFireAlarmInspection(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateB45MRequestFireAlarmInspection(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.ManufacturersStatement:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateManufacturersStatement(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditManufacturersStatement(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }

                        //GenerateJobDocuments.GenerateManufacturersStatement(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.SuppliersStatement:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateSuppliersStatement(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditSuppliersStatement(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateSuppliersStatement(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.PreliminaryAffidavitByFabricatorErector:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GeneratePreliminaryAffidavitbyFabricatorOrErector(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditPreliminaryAffidavitbyFabricatorOrErector(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        // GenerateJobDocuments.GeneratePreliminaryAffidavitbyFabricatorOrErector(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.FinalCertificationByFabricatorErector:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateFinalCertificationbyFabricatorOrErector(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditFinalCertificationbyFabricatorOrErector(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateFinalCertificationbyFabricatorOrErector(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }

                case Document.AppointmentRequestForm:
                    {
                        GenerateJobDocuments.GenerateAppointmentRequestForm(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }

                case Document.DEPNonPremisePermitApplicationforHydrant:
                    {

                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforDEPNonPremisePermitApplicationforHydrant(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestforDEPNonPremisePermitApplicationforHydrant(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.DEPNonPremisePermitApplicationforHydrant2020:
                    {

                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforDEPNonPremisePermitApplicationforHydrant2020(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestforDEPNonPremisePermitApplicationforHydrant2020(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.DemolitionChecklist:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforDemolitionChecklist(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestforDemolitionChecklist(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.ContractorInformationSheet:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforContractorInformationSheet(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestforContractorInformationSheet(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        // GenerateJobDocuments.GenerateRequestforContractorInformationSheet(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.ContainerInformationForm:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforContainerInformationForm(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestforContainerInformationForm(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.NotificationOfStagingOutriggerBeamUse:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateNotificationOfStagingOutriggerBeamUse(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditNotificationOfStagingOutriggerBeamUse(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }

                case Document.LLYearFinalSprinklerReportApplicationforExtension:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforLLYearFinalSprinklerReportApplicationforExtension(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestforLLYearFinalSprinklerReportApplicationforExtension(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateRequestforLLYearFinalSprinklerReportApplicationforExtension(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.MultipleOATHECBViolationsSubmission:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforMultipleOATHECBViolationsSubmission(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestforMultipleOATHECBViolationsSubmission(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateRequestforMultipleOATHECBViolationsSubmission(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                //case Document.ApplicationForBESTRecommendationForMechanicalMeansDemolition:
                //    {
                //        GenerateJobDocuments.GenerateApplicationforBestRecommendationforMechanicalMeansDemolition(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                //        break;
                //    }
                case Document.StatementinSupportofCertificateCorrection:
                    {

                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforStatementinSupportofCertificateCorrection(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestforStatementinSupportofCertificateCorrection(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        // GenerateJobDocuments.GenerateRequestforStatementinSupportofCertificateCorrection(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;

                    }
                case Document.RegistrationforDemolition:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforRegistrationforDemolition(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestforRegistrationforDemolition(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateRequestforRegistrationforDemolition(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.RequestItemsforBuildersPaymentPlanSignoff:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestItemsforBuildersPaymentPlanSignoff(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestItemsforBuildersPaymentPlanSignoff(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.WaiverOfBuildersPaymentRequirement:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateWaiverofBuildersPaymentRequirement(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditWaiverofBuildersPaymentRequirement(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        // GenerateJobDocuments.GenerateWaiverofBuildersPaymentRequirement(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.WaiverOfCurbAlignmentAndSidewalkAndCurb:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateWaiverOfCurbAlignmentAndSidewalkAndCurb(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditWaiverOfCurbAlignmentAndSidewalkAndCurb(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateWaiverOfCurbAlignmentAndSidewalkAndCurb(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }

                case Document.BoilerWaiverofCivilPenaltiesForm:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforBoilerWaiverofCivilPenaltiesForm(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestforBoilerWaiverofCivilPenaltiesForm(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.DesignProfessionalLicenseeSealandSignatureForm:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforDesignProfessionalLicenseeSealandSignatureForm(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestforDesignProfessionalLicenseeSealandSignatureForm(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.ElevatorApplication:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforElevatorApplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateRequestforElevatorApplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.EmergencyResponseAgenciesCONotificationAffidavitPriorOrSignOff:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforEmergencyResponseAgenciesCONotificationAffidavitPriorOrSignOff(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateRequestforEmergencyResponseAgenciesCONotificationAffidavitPriorOrSignOff(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.EmergencyResponseAgenciesCONotificationAffidavitPriorToApproval:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforEmergencyResponseAgenciesCONotificationAffidavitPriorToApproval(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateRequestforEmergencyResponseAgenciesCONotificationAffidavitPriorToApproval(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.SewerSelfCertificationHouseConnectionProposal:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforSewerSelfCertificationHouseConnectionProposal(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateRequestforSewerSelfCertificationHouseConnectionProposal(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.SewerSelfCertificationSiteConnectionProposal:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateSewerSelfCertificationSiteConnectionProposal(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateSewerSelfCertificationSiteConnectionProposal(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.HighRiseInitiativeProgramRequest:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateHighRiseInitiativeProgramRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateHighRiseInitiativeProgramRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.OpenApplicationCoverLetter:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforOpenApplicationCoverLetter(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateRequestforOpenApplicationCoverLetter(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.SelfCertificationforRemovalofExistingBoiler:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforSelfCertificationforRemovalofExistingBoiler(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateRequestforSelfCertificationforRemovalofExistingBoiler(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.PropertyProfileChangeRequest:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforPropertyProfileChangeRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateRequestforPropertyProfileChangeRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.LL58of1987AccessibilityWaiverRequest:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforLL58of1987AccessibilityWaiverRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestforLL58of1987AccessibilityWaiverRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.LL5of1973InformationChecklist:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforLL5of1973InformationChecklist(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestforLL5of1973InformationChecklist(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.ProfessionalCertificationAuditsandInspectionsAppointment:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforProfessionalCertificationAuditsandInspectionsAppointment(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestforProfessionalCertificationAuditsandInspectionsAppointment(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.RequestforCertificateofOccupancywithOpenApplicationCoverLetter:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforRequestforCertificateofOccupancywithOpenApplicationCoverLetter(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestforRequestforCertificateofOccupancywithOpenApplicationCoverLetter(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.HPD1AntiHarassmentAreaChecklist:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateHPD1AntiHarassmentAreaChecklist(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateHPD1AntiHarassmentAreaChecklist(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.HPD2ClintonSpecialDistrictAntiHarssmentChecklist:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateHPD2ClintonSpecialDistrictAntiHarssmentChecklist(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateHPD2ClintonSpecialDistrictAntiHarssmentChecklist(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.HPD3SROMultipleDwellingAntiHarssmentChecklist:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateHPD3SROMultipleDwellingAntiHarssmentChecklist(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateHPD3SROMultipleDwellingAntiHarssmentChecklist(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }

                case Document.ApplicationforContractPermits:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateApplicationforContractPermits(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditApplicationforContractPermits(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.ZoningDiagram:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateZoningdiagram(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateZoningdiagram(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.ZoningResolutionDeterminationApplication:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateZoningResolutionDeterminationApplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditZoningResolutionDeterminationApplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.TechnicalReportEnergyProgressInspections:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateTechnicalReportEnergyProgressInspections(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditTechnicalReportEnergyProgressInspections(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateTechnicalReportEnergyProgressInspections(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.TechnicalReportLuminousExitMarkings:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateTechnicalReportLuminousExitMarkings(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditTechnicalReportLuminousExitMarkings(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }

                case Document.TechnicalReportFormTR6:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateTecTechnicalReportformTR6(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditTecTechnicalReportformTR6(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.LetterofNoObjection:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateLetterofNoObjection(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditLetterofNoObjection(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.TechnicalReportformTR5H:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforTechnicalReportformTR5HRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestforTechnicalReportformTR5HRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.FireDepartmentRooftopVarianceApplication:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforFireDepartmentRooftopVarianceApplicationRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestforFireDepartmentRooftopVarianceApplicationRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.PF1TreeFundPaymentApplication:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GeneratePF1TreeFundPaymentApplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditPF1TreeFundPaymentApplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.TenantProtectionPlanApplication:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateTenantProtectionPlanApplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditTenantProtectionPlanApplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.TenantProtectionPlanApplication2022:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateTenantProtectionPlanApplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditTenantProtectionPlanApplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.SignHangerCertificationFormBoroughIntakeForm:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateSignHangerCertificationFormBoroughIntakeFormRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateSignHangerCertificationFormBoroughIntakeFormRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.StreetTreeChecklist:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateStreetTreeChecklistRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateStreetTreeChecklistRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                //case Document.FireDepartmentApplicationForPlanExamination:
                //    {
                //        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                //        {
                //            GenerateJobDocuments.GenerateFireDepartmentApplicationForPlanExaminationRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                //        }
                //        else
                //        {
                //            GenerateJobDocuments.EditFireDepartmentApplicationForPlanExaminationRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                //        }
                //        // GenerateJobDocuments.GenerateFireDepartmentApplicationForPlanExaminationRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                //        break;
                //    }
                case Document.ApplicationForCondominiumApportionmentApproval:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateApplicationForCondominiumApportionmentApprovalRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateApplicationForCondominiumApportionmentApprovalRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.TechnicalReportForm2008Version:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateTechnicalReportForm2008VersionRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditTechnicalReportForm2008VersionRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.TechnicalReportForm2014Version:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateTechnicalReportForm2014VersionRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditTechnicalReportForm2014VersionRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.TechnicalReportForm2016Version:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateTechnicalReportForm2016VersionRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditTechnicalReportForm2016VersionRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.GeneralContractorRegistration:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateGeneralContractorRegistrationRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO, employee.Id, jobDocumentCreateOrUpdateDTO.IdJob);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateGeneralContractorRegistrationRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO, employee.Id, jobDocumentCreateOrUpdateDTO.IdJob);
                        }

                        break;
                    }
                case Document.BoroughIntakeForm:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateBoroughIntakeFormRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditBoroughIntakeFormRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        // GenerateJobDocuments.GenerateBoroughIntakeFormRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.BoroughIntakeForm2:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateBoroughIntakeFormRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditBoroughIntakeFormRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        // GenerateJobDocuments.GenerateBoroughIntakeFormRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.ProfessionalCertificationRequiredItemsChecklist:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateProfessionalCertificationRequiredItemsChecklistRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditProfessionalCertificationRequiredItemsChecklistRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }

                        break;
                    }
                case Document.FireDepartmentProfessionalCertificationApplication:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateFireDepartmentProfessionalCertificationApplicationRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateFireDepartmentProfessionalCertificationApplicationRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.PlotPlanDiagram:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GeneratePlotPlanDiagramRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditPlotPlanDiagramRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.TechnicalReportConcreteResults:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateTechnicalReportConcreteResults(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditTechnicalReportConcreteResults(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.TechnicalReportBoringTests:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateTechnicalReportBoringTestsResults(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditTechnicalReportBoringTestsResults(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.TechnicalReportConcreteDesignMix:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateTechnicalReportConcreteDesignMix(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditTechnicalReportConcreteDesignMix(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.TechnicalReportFormTR3P:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateTechnicalReportFormTR3PRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditTechnicalReportFormTR3PRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.SmokeDetectorLetter:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateSmokeDetectorLetter(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditSmokeDetectorLetter(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.SchedulePW1A:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {

                            GenerateJobDocuments.GenerateSchedulePW1ARequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditSchedulePW1ARequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateSchedulePW1ARequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.SchedulePW1A2023:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {

                            GenerateJobDocuments.GenerateSchedulePW1ARequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditSchedulePW1ARequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateSchedulePW1ARequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.CCD1ConstructionCodeDeterminationForm:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateCCD1ConstructionCodeDeterminationFormRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditCCD1ConstructionCodeDeterminationFormRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.PAForestryApplication:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GeneratePAForestryApplicationRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditPAForestryApplicationRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.EN2AsBuiltEnergyAnalysis:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateEN2AsBuiltEnergyAnalysisRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateEN2AsBuiltEnergyAnalysisRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.L2RequestForWorkPermitSWOCivilPenaltyOverrideReductionWaiver:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateL2RequestForWorkPermitSWOCivilPenaltyOverrideReductionWaiverRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditL2RequestForWorkPermitSWOCivilPenaltyOverrideReductionWaiverRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }

                        break;
                    }
                case Document.L2RequestforWorkPermitCivilPenalty_new:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateL2RequestForWorkPermitSWOCivilPenaltyOverrideReductionWaiverRequest_new(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditL2RequestForWorkPermitSWOCivilPenaltyOverrideReductionWaiverRequest_new(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }

                        break;
                    }




                case Document.MechanicalInfoRequestForm:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateMechanicalInfoRequestFormRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateMechanicalInfoRequestFormRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.LandmarkExpediteCertificatedNoEffectApplicationForm:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateLandmarkExpediteCertificatedNoEffectApplicationForm(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditLandmarkExpediteCertificatedNoEffectApplicationForm(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.ExpeditedCertificateNoEffectApplication2021:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateExpeditedCertificateNoEffectApplication2021(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditExpeditedCertificateNoEffectApplication2021(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.PlaceAssemblyApplication:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GeneratePlaceAssemblyApplicationRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditPlaceAssemblyApplicationRequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.LandmarkFasttrackServiceApplication:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateLandmarkFasttrackServiceApplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditLandmarkFasttrackServiceApplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.LandmarkPreservationCommissionApplicationForm:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateLandmarkPreservationCommissionApplicationForm(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditLandmarkPreservationCommissionApplicationForm(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.PlanExaminationAppointmentForm:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GeneratePlanExaminationAppointmentForm(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditPlanExaminationAppointmentForm(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.OwnerInformationSheet:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateOwnerInformationSheet(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditOwnerInformationSheet(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        // GenerateJobDocuments.GenerateOwnerInformationSheet(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.Applicationformergersorapportionments:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateApplicationformergersorapportionments(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditApplicationformergersorapportionments(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.Temporaryplaceofassemblycertificateofoperation:
                    {
                        GenerateJobDocuments.GenerateTemporaryplaceofassemblycertificateofoperation(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }

                case Document.PermitteeRegistrationApplication:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GeneratePermitteeRegistrationApplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditPermitteeRegistrationApplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.VarianceAfterHoursPermit_VARPMT:
                    {
                        GenerateJobDocuments.GenerateVarianceAfterHoursPermit(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.CertificateofOccupancy:
                    {
                        GenerateJobDocuments.GenerateCertificateofOccupancy(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.Placeofassemblycertificateofoperation:
                    {
                        GenerateJobDocuments.GeneratePlaceofassemblycertificateofoperation(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.ApplicationformPW2008Version:
                    {
                        GenerateJobDocuments.GenerateApplicationformPW2008Version(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.Application2014Present:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateApplication2014Present(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditApplication2014Present(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }

                case Document.Planorworkapprovalapplication2019:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GeneratePlanorworkapprovalapplication2019(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditPlanorworkapprovalapplication2019(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.Planorworkapprovalapplication2022:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GeneratePlanorworkapprovalapplication2022(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditPlanorworkapprovalapplication2022(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.PlanorworkapprovalapplicationNew2022:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GeneratePlanorworkapprovalapplication2022(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditPlanorworkapprovalapplication2022(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }

                case Document.RequestforFullRoadwayClosure:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRequestforFullRoadwayClosure(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRequestforFullRoadwayClosure(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateRequestforFullRoadwayClosure(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }

                case Document.ProjectSpecificGeneralLiabilityInsuranceSummaryandAffirmation:
                    {
                        GenerateJobDocuments.GenerateProjectSpecificGeneralLiabilityInsuranceSummaryandAffirmation(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.SafetyRegistration:
                    {

                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateSafetyRegistration(jobDocument.Id, jobDocumentCreateOrUpdateDTO, employee.Id, jobDocumentCreateOrUpdateDTO.IdJob);
                        }
                        else
                        {
                            GenerateJobDocuments.EditGenerateSafetyRegistration(jobDocument.Id, jobDocumentCreateOrUpdateDTO, employee.Id, jobDocumentCreateOrUpdateDTO.IdJob);
                        }

                        break;
                    }
                case Document.OCMCMeetingRequestLetter:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateOCMCMeetingRequestLetter(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditOCMCMeetingRequestLetter(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.RFIApplicantofRecord:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRFIApplicantofRecord(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRFIApplicantofRecord(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateRFIApplicantofRecord(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.RFICostEstimate:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRFICostEstimate(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRFICostEstimate(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        // GenerateJobDocuments.GenerateRFICostEstimate(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.RFIListofSpecialProgressEnergyInspections:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRFIListofSpecialProgressEnergyInspections(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRFIListofSpecialProgressEnergyInspections(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        // GenerateJobDocuments.GenerateRFIListofSpecialProgressEnergyInspections(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.RFISpecialInspectorInformation:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRFISpecialInspectorInformation(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRFISpecialInspectorInformation(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        // GenerateJobDocuments.GenerateRFISpecialInspectorInformation(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.RFIProjectInformationSheet:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRFIProjectInformationSheet(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRFIProjectInformationSheet(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateRFIProjectInformationSheet(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.ScheduleC:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateScheduleC(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditScheduleC(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.ScheduleB:
                    {

                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateScheduleB(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditScheduleB(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        // GenerateJobDocuments.GenerateScheduleB(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.DOTGWPREIApplicationWorkPermitReissue:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateDOTGWPREIApplicationWorkPermitReissue(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditDOTGWPREIApplicationWorkPermitReissue(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.DOTGWPRENApplicationWorkPermitReissue:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateDOTGWPRENApplicationWorkPermitReissue(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditDOTGWPRENApplicationWorkPermitReissue(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.Certifiateofoccupancyletterofcompletionfolderreviewrequest:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateCertifiateofoccupancyletterofcompletionfolderreviewrequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditCertifiateofoccupancyletterofcompletionfolderreviewrequest(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.CertificateofOccupancyWorksheet2022:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateCertificateofOccupancyWorksheet2022(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditCertificateofOccupancyWorksheet2022(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.CertificateofOccupancyWorksheet2023:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateCertificateofOccupancyWorksheet2022(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditCertificateofOccupancyWorksheet2022(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.LetterofCompletionRequest2021:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateLetterofCompletionRequest2021(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditLetterofCompletionRequest2021(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.LetterofCompletionRequestPW72022:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateLetterofCompletionRequest2021(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditLetterofCompletionRequest2021(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.GenerateWithdrawalRequestForm:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateWithdrawalRequestForm2023(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditWithdrawalRequestForm2023(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.GenerateLoftBoardForm:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateLoftBoardForm2023(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditLoftBoardForm2023(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.Certificateofoccupancyinspectionapplication:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateCertificateofoccupancyinspectionapplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditCertificateofoccupancyinspectionapplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.CostaffidavitPW103:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateCostaffidavitPW103Request(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditCostaffidavitPW103Request(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateCostaffidavitPW103Request(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.Equipmentusepermitapplication:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateEquipmentusepermitapplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditEquipmentusepermitapplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateEquipmentusepermitapplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }

                case Document.Workpermitapplication:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateWorkpermitapplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditWorkpermitapplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.Workpermitapplication2022:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateWorkpermitapplication2022(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditWorkpermitapplication2022(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.WorkpermitapplicationNew2022:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateWorkpermitapplication2022(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditWorkpermitapplication2022(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.LandmarkFasttrackServiceApplication2021:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateLandmarkFasttrackServiceApplication2021(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditLandmarkFasttrackServiceApplication2021(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.LandmarkPreservationCommissionApplicationForm2021:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateLandmarkPreservationCommissionApplicationForm2021(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditLandmarkPreservationCommissionApplicationForm2021(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.PostApprovalApplication:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GeneratePostApprovalApplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditPostApprovalApplication(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.CertificateofCorrectionandStatementinSupport:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateCertificateofCorrectionandStatementinSupport(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditCertificateofCorrectionandStatementinSupport(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.Generate10DaysNotice(jobDocument.Id, jobDocumentCreateOrUpdateDTO, employee.Id, jobDocumentCreateOrUpdateDTO.IdJob);
                        break;
                    }
                case Document.SafetyRegistrationSigned:
                    {
                        GenerateJobDocuments.GenerateSafetyRegistrationSigned(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }

                case Document.GeneralContractorRegistrationSigned:
                    {
                        GenerateJobDocuments.GenerateGeneralContractorRegistrationSigned(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                //case Document.Applicationfortechmgmtplanexamination2019:
                //    {
                //        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                //        {
                //            GenerateJobDocuments.GenerateApplicationfortechmgmtplanexamination2019(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                //        }
                //        else
                //        {
                //            GenerateJobDocuments.EditApplicationfortechmgmtplanexamination2019(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                //        }
                //        //GenerateJobDocuments.GenerateCertificateOfCorrection(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                //        break;
                //    }
                case Document.Applicationfortechmgmtplanexamination2023:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateApplicationfortechmgmtplanexamination2023(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditApplicationfortechmgmtplanexamination2023(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        //GenerateJobDocuments.GenerateCertificateOfCorrection(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        break;
                    }
                case Document.RFIDOBNOWMechanical:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRFIDOBNOWMechanical(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRFIDOBNOWMechanical(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.RFIDOBNOWStructural:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRFIDOBNOWStructural(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRFIDOBNOWStructural(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.RFIDOBNOWPlumbing:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateRFIDOBNOWPlumbing(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditRFIDOBNOWPlumbing(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.TREnergyProgressInspections82020:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateTREnergyProgressInspections82020(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            // GenerateJobDocuments.EditTREnergyProgressInspections82020(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                            GenerateJobDocuments.EditTREnergyProgressInspections820(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.Certifiateofoccupancyletterofcompletionfolderreviewrequest2020:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GenerateCertifiateofoccupancyletterofcompletionfolderreviewrequest2020(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditCertifiateofoccupancyletterofcompletionfolderreviewrequest2020(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                case Document.TopoApplicationSet2021:
                    {
                        if (jobDocumentCreateOrUpdateDTO.Id == 0)
                        {
                            GenerateJobDocuments.GeneratePostTopoApplicationSet2021(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        else
                        {
                            GenerateJobDocuments.EditPostTopoApplicationSet2021(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }
                        break;
                    }
                default:
                    {
                        DocumentMaster objDocumentMaster = rpoContext.DocumentMasters.Where(d => d.Id == jobDocument.IdDocument).FirstOrDefault();

                        if (objDocumentMaster != null && objDocumentMaster.IsNewDocument == true)
                        {
                            GenerateJobDocuments.GenerateCommonNewDocument(jobDocument.Id, jobDocumentCreateOrUpdateDTO);
                        }

                        break;
                    }
            }

            //// For Job Document Field value (child)
            //if (jobDocumentCreateOrUpdateDTO.JobDocumentFields != null)
            //{
            //    foreach (JobDocumentFieldDTO jobdocfield in jobDocumentCreateOrUpdateDTO.JobDocumentFields)
            //    {
            //        JobDocumentField JobDocumentField = new JobDocumentField
            //        {
            //            IdJobDocument = jobDocument.Id,
            //            IdDocumentField = jobdocfield.IdDocumentField,
            //            Value = jobdocfield.Value
            //        };

            //        RpoContext.JobDocumentFields.Add(JobDocumentField);

            //    }
            //    RpoContext.SaveChanges();
            //}


            RpoContext rpoContextResponse = new RpoContext();
            JobDocument jobDocumentResponse =
            rpoContextResponse.JobDocuments.Include("DocumentMaster").
            Include("JobDocumentFields.DocumentField.Field").
            Include("JobApplication.JobApplicationType.Parent").
            Include("CreatedByEmployee").
            Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == jobDocument.Id);

            return Ok(FormatDetails(jobDocumentResponse));
        }



        /// <summary>
        /// Deletes the type of the Job Document.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>delete a record of job document.</returns>
        [Authorize]
        [HttpDelete]
        [RpoAuthorize]
        [ResponseType(typeof(JobDocument))]
        public IHttpActionResult DeleteJobDocument(int id)
        {
            var path = HttpRuntime.AppDomainAppPath;
            string newFileDirectoryName = string.Empty;
            string fileName = string.Empty;
            string idJob = string.Empty;
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

            JobDocument jobDocument = this.rpoContext.JobDocuments.Include("JobDocumentFields").Include("JobDocumentAttachments").FirstOrDefault(x => x.Id == id);

            if (jobDocument == null)
            {
                return this.NotFound();
            }

            JobDocument jobDocumentParent = this.rpoContext.JobDocuments.FirstOrDefault(x => x.IdParent == id);
            if (jobDocumentParent != null)
            {
                throw new RpoBusinessException(StaticMessages.SupportDocumentExistForThisDocument);
            }

            fileName = path + Properties.Settings.Default.JobDocumentExportPath + Convert.ToString(jobDocument.IdJob) + "\\" + Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;
            this.rpoContext.JobDocumentAttachments.RemoveRange(jobDocument.JobDocumentAttachments);
            this.rpoContext.JobDocumentFields.RemoveRange(jobDocument.JobDocumentFields);
            this.rpoContext.JobDocuments.Remove(jobDocument);
            this.rpoContext.SaveChanges();


            if (fileName != null)
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }

            var instance = new DropboxIntegration();
            string uploadFilePath = Properties.Settings.Default.DropboxFolderPath + Convert.ToString(jobDocument.IdJob);
            string dropBoxFileName = uploadFilePath + "/" + Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;
            var task = instance.RunDelete(dropBoxFileName);

            return this.Ok(jobDocument);

        }

        /// <summary>
        /// put the type of the Job Document.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttp Action Result.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobDocument))]
        public IHttpActionResult PutJobDocumentpdf(int id)
        {
            var path = HttpRuntime.AppDomainAppPath;
            string newFileDirectoryName = string.Empty;
            string fileName = string.Empty;
            string idJob = string.Empty;
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

            // newFileDirectoryName = Path.Combine(path, Properties.Settings.Default.JobDocumentExportPath);
            JobDocument jobDocument = this.rpoContext.JobDocuments.Include("JobDocumentFields").FirstOrDefault(x => x.Id == id);

            if (jobDocument == null)
            {
                return this.NotFound();
            }


            idJob = Convert.ToString(jobDocument.IdJob);

            fileName = path + Properties.Settings.Default.JobDocumentExportPath + idJob + "\\" + Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;

            jobDocument.DocumentPath = "";
            jobDocument.LastModifiedDate = DateTime.UtcNow;
            if (employee != null)
            {
                jobDocument.LastModifiedBy = employee.Id;
            }
            rpoContext.Entry(jobDocument).State = EntityState.Modified;
            this.rpoContext.SaveChanges();


            if (fileName != null)
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }


            return this.Ok(jobDocument);

        }

        /// <summary>
        /// Regenrate the Job Document.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Regenrate the Job Document..</returns>

        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobDocumentCreateOrUpdateDTO))]
        [HttpPut]
        [Route("api/jobdocument/regenerate/{id}")]
        public IHttpActionResult PutRegenerateJobDocument(int id)
        {
            RpoContext rpoContextResponse = new RpoContext();
            JobDocument jobDocumentResponse =
            rpoContextResponse.JobDocuments.Include("DocumentMaster").
            Include("JobDocumentFields.DocumentField.Field").
            Include("JobApplication.JobApplicationType.Parent").
            Include("CreatedByEmployee").
            Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

            if (jobDocumentResponse == null)
            {
                return NotFound();
            }

            GenerateJobDocuments.GenerateJobDocument(jobDocumentResponse.Id);
            return Ok(FormatDetails(jobDocumentResponse));
        }

        /// <summary>
        /// upload the Job Document.
        /// </summary>
        /// <returns>upload type of document</returns>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobDocumentAttachment))]
        [Route("api/JobDocument/fileAttachment")]
        public async Task<HttpResponseMessage> JobDocumentAttachment()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }


            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            System.Collections.Specialized.NameValueCollection formData = provider.FormData;
            IList<HttpContent> files = provider.Files;
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

            int idJobDocument = Convert.ToInt32(formData["IdJobDocument"]);
            JobDocumentAttachment jobDocumentAttachment = null;

            if (files.Count > 0)
            {

                string thisFileName1 = string.Empty;

                HttpContent file1 = files[0];
                var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');
                // thisFileName1 = thisFileName;
                string filename = string.Empty;
                Stream input = await file1.ReadAsStreamAsync();
                string directoryName = string.Empty;
                string URL = string.Empty;
                JobDocument jobDocument = rpoContext.JobDocuments.Where(x => x.Id == idJobDocument).FirstOrDefault();
                if (jobDocument != null)
                {
                    jobDocumentAttachment = rpoContext.JobDocumentAttachments.FirstOrDefault(x => x.IdJobDocument == idJobDocument);
                    if (jobDocumentAttachment == null)
                    {
                        if (thisFileName.Contains('#'))
                        {
                            thisFileName1 = Uri.EscapeDataString(thisFileName);
                        }
                        else
                        {
                            thisFileName1 = thisFileName;
                        }

                        jobDocumentAttachment = new JobDocumentAttachment();
                        jobDocumentAttachment.DocumentName = thisFileName1;
                        jobDocumentAttachment.Path = thisFileName1;
                        jobDocumentAttachment.IdJobDocument = idJobDocument;
                        rpoContext.JobDocumentAttachments.Add(jobDocumentAttachment);

                        jobDocument.DocumentPath = thisFileName1;
                        rpoContext.SaveChanges();

                        int document_Attachment = DocumentPlaceHolderField.Document_Attachment.GetHashCode();
                        JobDocumentField jobDocumentField = rpoContext.JobDocumentFields.FirstOrDefault(x => x.IdJobDocument == jobDocument.Id && x.DocumentField.IdField == document_Attachment);
                        if (jobDocumentField != null)
                        {
                            jobDocumentField.Value = thisFileName1;
                            jobDocumentField.ActualValue = thisFileName1;
                        }

                        rpoContext.SaveChanges();
                    }
                    else
                    {
                        if (thisFileName.Contains('#'))
                        {
                            thisFileName1 = Uri.EscapeDataString(thisFileName);
                        }
                        else
                        {
                            thisFileName1 = thisFileName;
                        }

                        jobDocumentAttachment.DocumentName = thisFileName1;
                        jobDocumentAttachment.Path = thisFileName1;
                        jobDocument.DocumentPath = thisFileName1;
                        rpoContext.SaveChanges();
                    }
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                var path = HttpRuntime.AppDomainAppPath;

                directoryName = Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentAttachmentPath));

                string directoryFileName = Convert.ToString(jobDocumentAttachment.Id) + "_" + thisFileName;
                filename = Path.Combine(directoryName, directoryFileName);

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

                string jobDocumentDirectory = Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentExportPath));
                jobDocumentDirectory = Path.Combine(jobDocumentDirectory, Convert.ToString(jobDocument.IdJob));

                string jobDocumentFile = Convert.ToString(jobDocument.Id) + "_" + thisFileName;

                jobDocumentFile = Path.Combine(jobDocumentDirectory, jobDocumentFile);

                if (!Directory.Exists(jobDocumentDirectory))
                {
                    Directory.CreateDirectory(jobDocumentDirectory);
                }

                if (File.Exists(jobDocumentFile))
                {
                    File.Delete(jobDocumentFile);
                }

                if (File.Exists(filename))
                {
                    File.Copy(filename, jobDocumentFile);
                }


                var instance = new DropboxIntegration();
                string uploadFilePath = Properties.Settings.Default.DropboxFolderPath + Convert.ToString(jobDocument.IdJob);
                string dropBoxFileName = Convert.ToString(jobDocument.Id) + "_" + thisFileName;
                string filepath = jobDocumentFile;
                var task = instance.RunUpload(uploadFilePath, dropBoxFileName, filepath);

                // return Request.CreateResponse<JobDocumentAttachment>(HttpStatusCode.OK, jobDocumentAttachment);
                var response = Request.CreateResponse(HttpStatusCode.OK, "Document uploaded successfully.");
                return response;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        ///Save  the document of pw517 document
        /// </summary>
        /// <param name="createUpdatePW517">The identifier.</param>
        /// <returns>create a pw517 document and save the document of pw517.</returns>

        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CreateUpdatePW517))]
        [Route("api/JobDocument/SavePW517")]
        public IHttpActionResult PostPW517JobDocument(CreateUpdatePW517 createUpdatePW517)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            JobDocument jobDocument;
            if (createUpdatePW517.Id == 0)
            {
                DocumentMaster documentMaster = rpoContext.DocumentMasters.FirstOrDefault(x => x.Id == createUpdatePW517.IdDocument);
                jobDocument = new JobDocument
                {
                    IdJob = createUpdatePW517.IdJob,
                    IdDocument = createUpdatePW517.IdDocument,
                    DocumentName = documentMaster != null ? documentMaster.DocumentName : string.Empty,
                    DocumentDescription = string.Empty,
                    IsArchived = false,
                    IdJobApplication = createUpdatePW517.Application != null ? Convert.ToInt32(createUpdatePW517.Application) : 0,
                    JobDocumentFor = createUpdatePW517.ForDescription,
                    IdJobViolation = null,
                    IdJobApplicationWorkPermitType = createUpdatePW517.idWorkPermit != null ? Convert.ToInt32(createUpdatePW517.idWorkPermit) : 0
                };

                if (jobDocument.IdJobApplication <= 0)
                {
                    jobDocument.IdJobApplication = null;
                }

                if (jobDocument.IdJobApplicationWorkPermitType <= 0)
                {
                    jobDocument.IdJobApplicationWorkPermitType = null;
                }

                jobDocument.CreatedDate = DateTime.UtcNow;
                jobDocument.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobDocument.CreatedBy = employee.Id;
                    jobDocument.LastModifiedBy = employee.Id;
                }

                rpoContext.JobDocuments.Add(jobDocument);
                rpoContext.SaveChanges();
            }
            else
            {

                jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == createUpdatePW517.Id);
                jobDocument.IdJobApplication = createUpdatePW517.Application != null ? Convert.ToInt32(createUpdatePW517.Application) : 0;
                jobDocument.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobDocument.LastModifiedBy = employee.Id;
                }
                rpoContext.SaveChanges();
            }
            GenerateJobDocuments.GenerateAfterHoursPermitApplication_PW517(jobDocument.Id, createUpdatePW517, employee.Id);

            RpoContext rpoContext_Response = new RpoContext();
            JobDocument jobDocumentResponse = rpoContext_Response.JobDocuments.Include("JobDocumentFields.DocumentField.Field").FirstOrDefault(x => x.Id == jobDocument.Id);

            return Ok(FormatPW517(jobDocumentResponse));
        }

        /// <summary>
        /// get the detail of pw517 document
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Edit the document of pw-517 document.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CreateUpdatePW517))]
        [Route("api/JobDocument/PW517/{id}")]
        public IHttpActionResult GetPW517JobDocument(int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            JobDocument jobDocumentResponse = rpoContext.JobDocuments.Include("JobDocumentFields.DocumentField.Field").FirstOrDefault(x => x.Id == id);

            return Ok(FormatPW517(jobDocumentResponse));
        }

        /// <summary>
        /// Update the Job Document.
        /// </summary>
        /// <returns>Update the Job Document.</returns>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobDocumentAttachment))]
        [Route("api/JobDocument/updateJobDocument")]
        public async Task<HttpResponseMessage> UpdateJobDocument()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            System.Collections.Specialized.NameValueCollection formData = provider.FormData;
            IList<HttpContent> files = provider.Files;
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            int idJobDocument = Convert.ToInt32(formData["IdJobDocument"]);
            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            JobDocument jobDocumentData = rpoContext.JobDocuments.Where(x => x.Id == idJobDocument).FirstOrDefault();
            if (jobDocumentData == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                if (files.Count > 0)
                {

                    HttpContent file1 = files[0];
                    var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');
                    string thisFileName1 = HttpUtility.UrlEncode(thisFileName);
                    string filename = string.Empty;
                    Stream input = await file1.ReadAsStreamAsync();
                    string directoryName = string.Empty;
                    string URL = string.Empty;
                    string oldDocumentPath = jobDocumentData.DocumentPath;

                    jobDocumentData.DocumentPath = thisFileName1;

                    jobDocumentData.LastModifiedDate = DateTime.UtcNow;
                    if (employee != null)
                    {
                        jobDocumentData.LastModifiedBy = employee.Id;
                    }
                    rpoContext.Entry(jobDocumentData).State = EntityState.Modified;
                    rpoContext.SaveChanges();



                    var path = HttpRuntime.AppDomainAppPath;
                    directoryName = Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentExportPath));
                    string folderDirectory = Path.Combine(directoryName, Convert.ToString(jobDocumentData.IdJob));
                    string directoryFileName = Convert.ToString(idJobDocument) + "_" + thisFileName;

                    string oldFilePath = Path.Combine(folderDirectory, Convert.ToString(idJobDocument) + "_" + oldDocumentPath);

                    filename = Path.Combine(folderDirectory, directoryFileName);

                    if (!Directory.Exists(folderDirectory))
                    {
                        Directory.CreateDirectory(folderDirectory);
                    }

                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }

                    using (Stream file = File.OpenWrite(filename))
                    {
                        input.CopyTo(file);
                        file.Close();
                    }
                    return Request.CreateResponse<JobDocument>(HttpStatusCode.OK, jobDocumentData);
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

            }
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="jobDocument">The job document.</param>
        /// <returns>JobDocumentCreateOrUpdateDTO.</returns>
        private JobDocumentCreateOrUpdateDTO FormatDetails(JobDocument jobDocument)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            var path = HttpRuntime.AppDomainAppPath;
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            string dropboxDocumentUrl = Properties.Settings.Default.DropboxExternalUrl + "/" + jobDocument.IdJob + "/" + Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;

            dropboxDocumentUrl = Regex.Replace(dropboxDocumentUrl, @"#", "%23", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            int AHVReferenceNumber = DocumentPlaceHolderField.AHVReferenceNumber.GetHashCode();
            //List<JobDocumentField> jobDocumentFieldList = jobDocument.JobDocumentFields.Where(x => x.DocumentField.Field.FieldName == "Attachment").ToList();
            //bool isAttachment = false;
            //string attachmentPath = string.Empty;
            //if (jobDocumentFieldList != null && jobDocumentFieldList.Count > 0 && jobDocument.JobDocumentAttachments != null && jobDocument.JobDocumentAttachments.Count > 0)
            //{
            //    JobDocumentAttachment jobDocumentAttachment = jobDocument.JobDocumentAttachments.FirstOrDefault();
            //    isAttachment = true;
            //    attachmentPath = jobDocumentAttachment != null ? (APIUrl + "/" + Properties.Settings.Default.JobDocumentAttachmentPath + "/" + jobDocumentAttachment.Id + "_" + jobDocumentAttachment.Path) : string.Empty;
            //}

            List<JobDocumentField> objJobDocumentFields = (from d in rpoContext.JobDocumentFields where d.DocumentField.Field.Id == AHVReferenceNumber select d).ToList();

            return new JobDocumentCreateOrUpdateDTO
            {
                Id = jobDocument.Id,
                IdJobDocument = jobDocument.Id,
                IdJob = jobDocument.IdJob,
                IdDocument = jobDocument.IdDocument,
                DocumentName = jobDocument.DocumentName,
                IsArchived = jobDocument.IsArchived,
                DocumentCode = jobDocument.DocumentMaster != null ? jobDocument.DocumentMaster.Code : string.Empty,
                CreatedBy = jobDocument.CreatedBy,
                Copies = 1,
                JobDocumentFor = jobDocument.JobDocumentFor,
                DocumentDescription = jobDocument.DocumentDescription,
                //DocumentPath = isAttachment ? attachmentPath : (jobDocument.DocumentPath != null && jobDocument.DocumentPath != "" ? APIUrl + Properties.Settings.Default.JobDocumentExportPath + "/" + jobDocument.IdJob + "/" + Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath : string.Empty),
                DocumentPath = jobDocument.DocumentPath != null && jobDocument.DocumentPath != "" ? dropboxDocumentUrl : string.Empty,
                CreatedByEmployeeName = jobDocument.CreatedByEmployee != null ? jobDocument.CreatedByEmployee.FirstName + " " + jobDocument.CreatedByEmployee.LastName : string.Empty,
                CreatedDate = jobDocument.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobDocument.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobDocument.CreatedDate,
                LastModifiedByEmployeeName = jobDocument.LastModifiedByEmployee != null ? jobDocument.LastModifiedByEmployee.FirstName + " " + jobDocument.LastModifiedByEmployee.LastName : string.Empty,
                LastModifiedDate = jobDocument.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobDocument.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobDocument.LastModifiedDate,
                ApplicationNumber = jobDocument.JobApplication != null ? jobDocument.JobApplication.ApplicationNumber : (jobDocument.JobViolation != null ? jobDocument.JobViolation.SummonsNumber : string.Empty),
                AppplicationType = jobDocument.JobApplication != null && jobDocument.JobApplication.JobApplicationType != null ? jobDocument.JobApplication.JobApplicationType.Description : (jobDocument.JobViolation != null ? jobDocument.JobViolation.IssuingAgency : string.Empty),
                ParentApplicationType = jobDocument.TrackingNumber != null ? jobDocument.TrackingNumber : (jobDocument.JobApplication != null ? jobDocument.JobApplication.ApplicationNumber : (jobDocument.JobViolation != null ? jobDocument.JobViolation.SummonsNumber : string.Empty)),
                IdParentApplicationType = jobDocument.JobApplication != null ? (jobDocument.JobApplication.JobApplicationType != null ? jobDocument.JobApplication.JobApplicationType.IdParent : null) : null,
                IsAddPage = jobDocument.DocumentMaster != null ? jobDocument.DocumentMaster.IsAddPage : false,
                AHVReferenceNumber = jobDocument.IdParent != null ? (from d in objJobDocumentFields where d.IdJobDocument == jobDocument.IdParent select d.Value).FirstOrDefault() : string.Empty,
                TrackingNumber = jobDocument.TrackingNumber != null ? jobDocument.TrackingNumber : (jobDocument.JobApplication != null ? jobDocument.JobApplication.ApplicationNumber : (jobDocument.JobViolation != null ? jobDocument.JobViolation.SummonsNumber : string.Empty)),
                PermitNumber = jobDocument.PermitNumber != null ? jobDocument.PermitNumber : string.Empty,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="jobDocument">The job document.</param>
        /// <returns>JobDocumentCreateOrUpdateDTO.</returns>
        private JobDocumentGetDetailsDTO Format(JobDocument jobDocument)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            string dropboxDocumentUrl = Properties.Settings.Default.DropboxExternalUrl + "/" + jobDocument.IdJob + "/" + Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;

            JobDocumentAttachment jobDocumentAttachment = jobDocument.JobDocumentAttachments.FirstOrDefault();
            return new JobDocumentGetDetailsDTO
            {
                Id = jobDocument.Id,
                IdJob = jobDocument.IdJob,
                IdDocument = jobDocument.IdDocument,
                DocumentName = jobDocument.DocumentName,
                IsArchived = jobDocument.IsArchived,
                CreatedBy = jobDocument.CreatedBy,
                DocumentPath = jobDocument.DocumentPath != null && jobDocument.DocumentPath != "" ? APIUrl + Properties.Settings.Default.JobDocumentExportPath + "/" + jobDocument.IdJob + "/" + Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath : string.Empty,
                CreatedByEmployeeName = jobDocument.CreatedByEmployee != null ? jobDocument.CreatedByEmployee.FirstName + " " + jobDocument.CreatedByEmployee.LastName : string.Empty,
                CreatedDate = jobDocument.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobDocument.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobDocument.CreatedDate,
                LastModifiedByEmployeeName = jobDocument.LastModifiedByEmployee != null ? jobDocument.LastModifiedByEmployee.FirstName + " " + jobDocument.LastModifiedByEmployee.LastName : string.Empty,
                LastModifiedDate = jobDocument.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobDocument.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobDocument.LastModifiedDate,
                DocumentCode = jobDocument.DocumentMaster != null ? jobDocument.DocumentMaster.Code : string.Empty,
                ApplicationNo = jobDocument.JobApplication != null ? jobDocument.JobApplication.ApplicationNumber : (jobDocument.JobViolation != null ? jobDocument.JobViolation.SummonsNumber : string.Empty),
                AppplicationType = jobDocument.JobApplication != null ? jobDocument.JobApplication.JobApplicationType != null ? jobDocument.JobApplication.JobApplicationType.Description : string.Empty : string.Empty,
                IsAddPage = jobDocument.DocumentMaster != null ? jobDocument.DocumentMaster.IsAddPage : false,
                JobDocumentField = jobDocument.JobDocumentFields.Select(x =>
                new DocumentFieldDTO
                {
                    Id = x.DocumentField.Id,
                    FieldName = x.DocumentField.Field != null ? x.DocumentField.Field.FieldName : string.Empty,
                    ControlType = x.DocumentField.Field != null ? x.DocumentField.Field.ControlType : 0,
                    DataType = x.DocumentField.Field != null ? x.DocumentField.Field.DataType : 0,
                    IdDocument = x.DocumentField.IdDocument != null ? x.DocumentField.IdDocument : 0,
                    IdField = x.DocumentField.IdField != null ? x.DocumentField.IdField : 0,
                    IsRequired = x.DocumentField != null ? x.DocumentField.IsRequired : false,
                    Length = x.DocumentField != null ? x.DocumentField.Length : 0,
                    APIUrl = x.DocumentField != null ? x.DocumentField.APIUrl : string.Empty,
                    DocumentName = x.DocumentField.Document != null ? x.DocumentField.Document.DocumentName : string.Empty,
                    Code = x.DocumentField.Document != null ? x.DocumentField.Document.Code : string.Empty,
                    Path = x.DocumentField.Document != null ? x.DocumentField.Document.Path : string.Empty,
                    Field = x.DocumentField.Field != null ? x.DocumentField.Field : null,
                    Value = x.Value,
                    IdParentField = x.DocumentField.IdParentField != null ? x.DocumentField.IdParentField : null,
                    DisplayFieldName = x.DocumentField != null && x.DocumentField.Field != null ? x.DocumentField.Field.DisplayFieldName : string.Empty,
                    DisplayOrder = x.DocumentField != null ? x.DocumentField.DisplayOrder : 10000,
                    StaticDescription = x.DocumentField != null ? x.DocumentField.StaticDescription : null,
                    //AttachmentPath = jobDocumentAttachment != null ? (APIUrl + "/" + Properties.Settings.Default.JobDocumentAttachmentPath + "/" + jobDocumentAttachment.Id + "_" + jobDocumentAttachment.Path) : string.Empty
                    AttachmentPath = jobDocument.DocumentPath != null && jobDocument.DocumentPath != "" ? dropboxDocumentUrl : string.Empty,

                }).OrderBy(x => x.DisplayOrder).ToList()
            };
        }

        /// <summary>
        /// Gets the permit list from bis.
        /// </summary>
        /// <param name="jobApplicationNumber">The job application number.</param>
        /// <param name="binNumber">The bin number.</param>
        /// <returns>Get the list of permit from bis List.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/jobdocument/PermitListFromBIS")]
        public IHttpActionResult GetPermitListFromBIS(JobDocumentPermitDTO dto)
        {
            return this.Ok(ReadPermitList(dto.JobApplicationNumber, dto.binNumber, dto.DocumentDescription, dto.IdJob));
        }

        /// <summary>
        /// Gets the permit list from bis of COO Document.
        /// </summary>
        /// <param name="jobApplicationNumber">The job application number.</param>
        /// <param name="binNumber">The bin number.</param>
        /// <returns>Get the list of permit from bis List.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/jobdocument/PermitListFromBISForCOO")]
        public IHttpActionResult GetPermitListForCOOFromBIS(JobDocumentPermitDTO dto)
        {
            return this.Ok(ReadPermitListForCOO(dto.JobApplicationNumber));
        }

        private List<JobDocumentPermitDTO> ReadPermitListForCOO(string jobApplicationNumber)
        {
            string urlAddress = string.Empty;


            urlAddress = $"http://a810-bisweb.nyc.gov/bisweb/JobsQueryByNumberServlet?passjobnumber={jobApplicationNumber}&passdocnumber=&go10=+GO+&requestid=0";

            JobDocumentPermitDTO jobDocumentPermitDTO = new JobDocumentPermitDTO();
            List<JobDocumentPermitDTO> jobDocumentPermitDTOList = new List<JobDocumentPermitDTO>();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12;
            HtmlAgilityPack.HtmlDocument doc = new HtmlWeb().Load(urlAddress);
            var descendants = doc.DocumentNode.Descendants();

            var block = descendants.FirstOrDefault(n => n.HasClass("right") && n.InnerHtml.Contains("COApplicationSummaryServlet"));
            if (block != null)
            {
                var blockParent = block.ParentNode.ParentNode;


                var printPermit = descendants.FirstOrDefault(n => n.Name == "a" && n.Attributes.Where(x => x.Name == "href" && x.Value.Contains("COApplicationSummaryServlet")).Count() > 0);
                string printPdfUrl = string.Empty;
                if (printPermit != null)
                {
                    printPdfUrl = printPermit.Attributes.FirstOrDefault(x => x.Name == "href" && x.Value.Contains("COApplicationSummaryServlet")).Value;
                }

                string nexturl = string.Empty;
                if (printPdfUrl != null)
                {
                    nexturl = $"http://a810-bisweb.nyc.gov/bisweb/{printPdfUrl}";
                }
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                   | SecurityProtocolType.Tls11
                                   | SecurityProtocolType.Tls12;
                HtmlAgilityPack.HtmlDocument doc1 = new HtmlWeb().Load(nexturl);
                var descendants1 = doc1.DocumentNode.Descendants();

                var COOUrl = descendants1.FirstOrDefault(n => n.Name == "a" && n.Attributes.Where(x => x.Name == "href" && x.Value.Contains("COPdfListingServlet")).Count() > 0);
                string COOPageUrl = string.Empty;
                if (printPermit != null)
                {
                    COOPageUrl = COOUrl.Attributes.FirstOrDefault(x => x.Name == "href" && x.Value.Contains("COPdfListingServlet")).Value;
                }

                if (COOPageUrl != null)
                {
                    jobDocumentPermitDTO.DetailUrl = $"http://a810-bisweb.nyc.gov/bisweb/{COOPageUrl}";
                }
                jobDocumentPermitDTOList.Add(jobDocumentPermitDTO);
            }

            return jobDocumentPermitDTOList;
        }
        /// <summary>
        /// Post the peLoc Dcoumetn url
        /// </summary>
        /// <returns>The peLoc Dcoumetn url.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/jobdocument/LOCURL")]
        public IHttpActionResult GetLOCDocumentURL(JobDocumentPermitDTO dto)
        {
            return this.Ok(ReadLOCDocumentURL(dto.JobApplicationNumber));
        }


        /// <summary>
        /// Gets the permit list from bis.
        /// </summary>
        /// <param name="jobApplicationNumber">The job application number.</param>
        /// <param name="binNumber">The bin number.</param>
        /// <returns>IHttpActionResult.</returns>
        //[Authorize]
        //[RpoAuthorize]
        //[HttpGet]
        //[Route("api/jobdocument/PermitListFromBIS/{jobApplicationNumber}")]
        //public IHttpActionResult LinkWithBIS(string jobApplicationNumber)
        //{
        //    return this.Ok(ReadPermitList(jobApplicationNumber, string.Empty,string.Empty));
        //}

        /// <summary>
        /// Posts the permit list from bis.
        /// </summary>
        /// <param name="pullPermitDTO">The pull permit dto.</param>
        /// <returns>pull permit the update the detail..</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/jobdocument/PullPermit")]
        public IHttpActionResult PostPermitListFromBIS(PullPermitDTO pullPermitDTO)
        {
            string pdfDownloadLink = ReadPermitPdfLink(pullPermitDTO.DetailUrl);
            PullPermitResultDTO pullPermitResultDTO = new PullPermitResultDTO();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12;

            if (!string.IsNullOrEmpty(pdfDownloadLink))
            {
                JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == pullPermitDTO.IdJobDocument);
                if (jobDocument != null)
                {
                    string thisFileName = pullPermitDTO.NumberDocType + ".pdf";
                    JobDocumentAttachment jobDocumentAttachment = null;
                    jobDocumentAttachment = rpoContext.JobDocumentAttachments.FirstOrDefault(x => x.IdJobDocument == pullPermitDTO.IdJobDocument);
                    if (jobDocumentAttachment == null)
                    {
                        jobDocumentAttachment = new JobDocumentAttachment();
                        jobDocumentAttachment.DocumentName = thisFileName;
                        jobDocumentAttachment.Path = thisFileName;
                        jobDocumentAttachment.IdJobDocument = pullPermitDTO.IdJobDocument;
                        rpoContext.JobDocumentAttachments.Add(jobDocumentAttachment);

                        jobDocument.DocumentPath = thisFileName;
                        rpoContext.SaveChanges();

                        int document_Attachment = DocumentPlaceHolderField.Document_Attachment.GetHashCode();
                        JobDocumentField jobDocumentField = rpoContext.JobDocumentFields.FirstOrDefault(x => x.IdJobDocument == jobDocument.Id && x.DocumentField.IdField == document_Attachment);
                        if (jobDocumentField != null)
                        {
                            jobDocumentField.Value = thisFileName;
                            jobDocumentField.ActualValue = thisFileName;
                        }

                        rpoContext.SaveChanges();
                    }
                    else
                    {
                        jobDocumentAttachment.DocumentName = thisFileName;
                        jobDocumentAttachment.Path = thisFileName;
                        jobDocument.DocumentPath = thisFileName;
                        rpoContext.SaveChanges();
                    }

                    string path = HttpRuntime.AppDomainAppPath;
                    string directoryName = Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentAttachmentPath));

                    string directoryFileName = Convert.ToString(jobDocumentAttachment.Id) + "_" + thisFileName;
                    string filename = Path.Combine(directoryName, directoryFileName);

                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }

                    //using (Stream file = File.OpenWrite(filename))
                    //{
                    //    input.CopyTo(file);
                    //    file.Close();
                    //}


                    string jobDocumentDirectory = Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentExportPath));
                    jobDocumentDirectory = Path.Combine(jobDocumentDirectory, Convert.ToString(jobDocument.IdJob));

                    string jobDocumentFile = Convert.ToString(jobDocument.Id) + "_" + thisFileName;

                    jobDocumentFile = Path.Combine(jobDocumentDirectory, jobDocumentFile);

                    if (!Directory.Exists(jobDocumentDirectory))
                    {
                        Directory.CreateDirectory(jobDocumentDirectory);
                    }

                    if (File.Exists(jobDocumentFile))
                    {
                        File.Delete(jobDocumentFile);
                    }

                    //if (File.Exists(filename))
                    //{
                    //    File.Copy(filename, jobDocumentFile);
                    //}
                    WriteLogWebclient("Start Pull Permit Start");

                    string pdfPAth = $"http://a810-bisweb.nyc.gov/bisweb/{pdfDownloadLink}";
                    WriteLogWebclient("Permit File :" + pdfPAth);

                    WebClient client = new WebClient();
                    client.Headers.Add("User-Agent: Other");
                    WriteLogWebclient("client Header :" + client.Headers.ToString());
                    try
                    {
                        WriteLogWebclient("Before Download File Path :" + jobDocumentFile);
                        client.DownloadFile(pdfPAth, jobDocumentFile);
                        WriteLogWebclient("Download File Status : Success");
                    }
                    catch (Exception ex)
                    {
                        WriteLogWebclient("Download File Faild :" + ex.Message);
                        WriteLogWebclient("Download InnerException :" + ex.InnerException);
                    }


                    //WebClient client_2 = new WebClient();
                    //client_2.Headers.Add("User-Agent: Other");
                    //client_2.DownloadFile(pdfPAth, jobDocumentFile);

                    //Common.DownloadFile(pdfPAth, filename);
                    //Common.DownloadFile(pdfPAth, jobDocumentFile);

                    var instance = new DropboxIntegration();
                    string uploadFilePath = Properties.Settings.Default.DropboxFolderPath + Convert.ToString(jobDocument.IdJob);
                    string dropBoxFileName = Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;
                    string filepath = jobDocumentFile;
                    var task = instance.RunUpload(uploadFilePath, dropBoxFileName, filepath);

                    string dropboxDocumentUrl = Properties.Settings.Default.DropboxExternalUrl + "/" + jobDocument.IdJob + "/" + Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;
                    pullPermitResultDTO.JobDocumentUrl = dropboxDocumentUrl;
                    pullPermitResultDTO.IsPdfExist = true;

                    JobDocumentField jobDocumentField_WorkPermit = rpoContext.JobDocumentFields.FirstOrDefault(x => x.IdJobDocument == jobDocument.Id && x.DocumentField.Field.FieldName == "WorkPermit");

                    UpdatePermitDates(jobDocumentFile, pullPermitDTO.NumberDocType, Convert.ToInt32(jobDocumentField_WorkPermit.Value));

                }
            }
            else
            {
                pullPermitResultDTO.JobDocumentUrl = null;
                pullPermitResultDTO.IsPdfExist = false;
            }
            return this.Ok(pullPermitResultDTO);

        }

        public static void WriteLogWebclient(string message)
        {
            string errorLogFilename = "WebClientLog_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

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
                    stwriter.WriteLine("-------------------Web Client Log Start-----------as on " + DateTime.Now.ToString("hh:mm tt"));
                    stwriter.WriteLine("Message: " + message);
                    stwriter.WriteLine("-------------------End----------------------------");
                    stwriter.Close();
                }
            }
            else
            {
                StreamWriter stwriter = File.CreateText(path);
                stwriter.WriteLine("-------------------Web Client Log Start-----------as on " + DateTime.Now.ToString("hh:mm tt"));
                stwriter.WriteLine("Message: " + message);
                stwriter.WriteLine("-------------------End----------------------------");
                stwriter.Close();
            }


            var attachments = new List<string>();
            if (File.Exists(path))
            {
                attachments.Add(path);
            }

            var to = new List<KeyValuePair<string, string>>();

            to.Add(new KeyValuePair<string, string>("meethalal.teli@credencys.com", "RPO Team"));

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/MasterEmailTemplate.htm")))
            {
                body = reader.ReadToEnd();
            }

            var cc = new List<KeyValuePair<string, string>>();

            string emailBody = body;
            emailBody = emailBody.Replace("##EmailBody##", "Please correct the issue.");

            Mail.Send(
                       new KeyValuePair<string, string>("noreply@rpoinc.com", "RPO Log"),
                       to,
                       cc,
                       "[UAT]-Error Log",
                       emailBody,
                       attachments
                   );
        }
        /// <summary>
        /// get the pull permit of loc document.
        /// </summary>
        /// <param name="pullPermitDTO">The pull loc document.</param>
        /// <returns>get the detail of loc document.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocument/PullPermit_LOC/{idJobDocument}")]
        public IHttpActionResult GetPullPermit_LOC(int idJobDocument)
        {
            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);

            string applicationNumber = jobDocument != null && jobDocument.JobApplication != null ? jobDocument.JobApplication.ApplicationNumber : string.Empty;
            string pdfDownloadLink = ReadLOCPermitPdfLink(applicationNumber);
            PullPermitResultDTO pullPermitResultDTO = new PullPermitResultDTO();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12;
            if (!string.IsNullOrEmpty(pdfDownloadLink))
            {
                if (jobDocument != null)
                {
                    string thisFileName = "loc_" + applicationNumber + ".pdf";
                    JobDocumentAttachment jobDocumentAttachment = null;
                    jobDocumentAttachment = rpoContext.JobDocumentAttachments.FirstOrDefault(x => x.IdJobDocument == jobDocument.Id);
                    if (jobDocumentAttachment == null)
                    {
                        jobDocumentAttachment = new JobDocumentAttachment();
                        jobDocumentAttachment.DocumentName = thisFileName;
                        jobDocumentAttachment.Path = thisFileName;
                        jobDocumentAttachment.IdJobDocument = jobDocument.Id;
                        rpoContext.JobDocumentAttachments.Add(jobDocumentAttachment);

                        jobDocument.DocumentPath = thisFileName;
                        rpoContext.SaveChanges();

                        int document_Attachment = DocumentPlaceHolderField.Document_Attachment.GetHashCode();
                        JobDocumentField jobDocumentField = rpoContext.JobDocumentFields.FirstOrDefault(x => x.IdJobDocument == jobDocument.Id && x.DocumentField.IdField == document_Attachment);
                        if (jobDocumentField != null)
                        {
                            jobDocumentField.Value = thisFileName;
                            jobDocumentField.ActualValue = thisFileName;
                        }

                        rpoContext.SaveChanges();
                    }
                    else
                    {
                        jobDocumentAttachment.DocumentName = thisFileName;
                        jobDocumentAttachment.Path = thisFileName;
                        jobDocument.DocumentPath = thisFileName;
                        rpoContext.SaveChanges();
                    }

                    string path = HttpRuntime.AppDomainAppPath;
                    string directoryName = Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentAttachmentPath));

                    string directoryFileName = Convert.ToString(jobDocumentAttachment.Id) + "_" + thisFileName;
                    string filename = Path.Combine(directoryName, directoryFileName);

                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }

                    //using (Stream file = File.OpenWrite(filename))
                    //{
                    //    input.CopyTo(file);
                    //    file.Close();
                    //}


                    string jobDocumentDirectory = Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentExportPath));
                    jobDocumentDirectory = Path.Combine(jobDocumentDirectory, Convert.ToString(jobDocument.IdJob));

                    string jobDocumentFile = Convert.ToString(jobDocument.Id) + "_" + thisFileName;

                    jobDocumentFile = Path.Combine(jobDocumentDirectory, jobDocumentFile);

                    if (!Directory.Exists(jobDocumentDirectory))
                    {
                        Directory.CreateDirectory(jobDocumentDirectory);
                    }

                    if (File.Exists(jobDocumentFile))
                    {
                        File.Delete(jobDocumentFile);
                    }

                    //if (File.Exists(filename))
                    //{
                    //    File.Copy(filename, jobDocumentFile);
                    //}

                    string pdfPAth = $"http://a810-bisweb.nyc.gov/bisweb/{pdfDownloadLink}";
                    WebClient client = new WebClient();
                    client.Headers.Add("User-Agent: Other");
                    client.DownloadFile(pdfPAth, filename);

                    WebClient client_2 = new WebClient();
                    client_2.Headers.Add("User-Agent: Other");
                    client_2.DownloadFile(pdfPAth, jobDocumentFile);

                    var instance = new DropboxIntegration();
                    string uploadFilePath = Properties.Settings.Default.DropboxFolderPath + Convert.ToString(jobDocument.IdJob);
                    string dropBoxFileName = Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;
                    string filepath = jobDocumentFile;
                    var task = instance.RunUpload(uploadFilePath, dropBoxFileName, filepath);

                    string dropboxDocumentUrl = Properties.Settings.Default.DropboxExternalUrl + "/" + jobDocument.IdJob + "/" + Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;
                    pullPermitResultDTO.JobDocumentUrl = dropboxDocumentUrl;
                    pullPermitResultDTO.IsPdfExist = true;

                    //JobDocumentField jobDocumentField_WorkPermit = rpoContext.JobDocumentFields.FirstOrDefault(x => x.IdJobDocument == jobDocument.Id && x.DocumentField.Field.FieldName == "WorkPermit");

                    //UpdatePermitDates(jobDocumentFile, pullPermitDTO.NumberDocType, Convert.ToInt32(jobDocumentField_WorkPermit.Value));

                }
            }
            else
            {
                pullPermitResultDTO.JobDocumentUrl = null;
                pullPermitResultDTO.IsPdfExist = false;
            }
            return this.Ok(pullPermitResultDTO);

        }

        /// <summary>
        /// Add Page od the jobdocument
        /// </summary>
        /// <param name="idJobDocument">The pull id document.</param>
        /// <returns>Create a add page in documetn.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocument/AddPage/{idJobDocument}")]
        public IHttpActionResult GetAddPage(int idJobDocument)
        {
            JobDocument jobDocument = rpoContext.JobDocuments.Include("JobDocumentFields.DocumentField").FirstOrDefault(x => x.Id == idJobDocument);
            if (jobDocument != null)
            {
                string path = HttpRuntime.AppDomainAppPath;

                string jobDocumentDirectory = Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentExportPath));
                jobDocumentDirectory = Path.Combine(jobDocumentDirectory, Convert.ToString(jobDocument.IdJob));

                string jobDocumentFile = Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;
                jobDocumentFile = Path.Combine(jobDocumentDirectory, jobDocumentFile);

                var instance_Download = new DropboxIntegration();
                string dropboxFilePath = Properties.Settings.Default.DropboxFolderPath + Convert.ToString(jobDocument.IdJob);
                string dropboxFileName = Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;

                string sourceFileName = dropboxFilePath + "/" + dropboxFileName;
                var task_download = instance_Download.RunDownload(sourceFileName, jobDocumentFile);

                string jobDocumentFile_Temp = "Temp_" + Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;
                jobDocumentFile_Temp = Path.Combine(jobDocumentDirectory, jobDocumentFile_Temp);

                string jobDocumentFile_Template = "Template_" + Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;
                jobDocumentFile_Template = Path.Combine(jobDocumentDirectory, jobDocumentFile_Template);

                string templatefilePath = Properties.Settings.Default.AddPageTemplatePath;
                string templatefileName = string.Empty;

                JobDocument jobDocumentResponse = rpoContext.JobDocuments.Include("DocumentMaster").Include("JobDocumentFields.DocumentField.Field").Include("JobApplication.JobApplicationType.Parent").
                                                    Include("CreatedByEmployee").
                                                    Include("LastModifiedByEmployee")
                                                    .FirstOrDefault(x => x.Id == jobDocument.Id);
                #region ZRD 1
                if (jobDocument.IdDocument == Document.ZoningResolutionDeterminationApplication.GetHashCode())
                {
                    templatefilePath = Path.Combine(path, templatefilePath);
                    templatefileName = Path.Combine(templatefilePath, "ZRD1_AddPageTemplate.pdf");

                    PdfReader.unethicalreading = true;
                    var reader = new PdfReader(templatefileName);

                    var pdfStamper = new PdfStamper(reader, new FileStream(jobDocumentFile_Template, FileMode.Create));
                    var field = pdfStamper.AcroFields;
                    var reader_JobDocument = new PdfReader(jobDocumentFile);
                    int numberofPages = reader_JobDocument.NumberOfPages;
                    field.SetField("txtPage", Convert.ToString(numberofPages + 1));
                    field.SetField("txtDescription", string.Empty);
                    JobDocumentField df = jobDocumentResponse.JobDocumentFields.Where(x => x.DocumentField.PdfFields == "txtName").FirstOrDefault();


                    string applicantname = string.Empty;

                    if (df != null)
                    {
                        applicantname = df.ActualValue;
                    }

                    //field.SetField("txtName", jobDocumentResponse.DocumentDescription.IndexOf("Applicant:") == 0 ? jobDocumentResponse.DocumentDescription.Split('|')[0].Replace("Applicant:", "").Trim() : string.Empty);
                    field.SetField("txtName", applicantname);
                    field.SetField("txtDate", string.Empty);

                    field.RenameField("txtPage", "Page" + Convert.ToString(numberofPages + 1) + "_txtPage");
                    field.RenameField("txtDescription", "Page" + Convert.ToString(numberofPages + 1) + "_txtDescription");
                    field.RenameField("txtName", "Page" + Convert.ToString(numberofPages + 1) + "_txtName");
                    field.RenameField("txtDate", "Page" + Convert.ToString(numberofPages + 1) + "_txtDate");

                    field.GenerateAppearances = false;
                    pdfStamper.Close();
                    reader.Close();
                    reader_JobDocument.Close();
                    SplitAndAppendLastPage(jobDocumentFile, jobDocumentFile_Template, jobDocumentFile_Temp);
                }
                #endregion

                #region ZD 1

                else if (jobDocument.IdDocument == Document.ZoningDiagram.GetHashCode())
                {

                    templatefilePath = Path.Combine(path, templatefilePath);
                    templatefileName = Path.Combine(templatefilePath, "ZD1_AddPageTemplate.pdf");

                    PdfReader.unethicalreading = true;
                    var reader = new PdfReader(templatefileName);

                    var pdfStamper = new PdfStamper(reader, new FileStream(jobDocumentFile_Template, FileMode.Create));
                    var field = pdfStamper.AcroFields;
                    var reader_JobDocument = new PdfReader(jobDocumentFile);
                    int numberofPages = reader_JobDocument.NumberOfPages;

                    field.SetField("JobFileInfo", (jobDocument.Job != null ? jobDocument.Job.JobNumber : string.Empty) + " - ZD1-76625.pdf");
                    field.SetField("txtSheet2", Convert.ToString(numberofPages));
                    field.RenameField("txtSheet2", "Page" + Convert.ToString(numberofPages) + "_txtSheet2");
                    field.SetField("txtNumSheets", Convert.ToString(numberofPages));
                    string[] fieldList = { "txtLast", "txtFirst", "txtMI", "txtCompany", "txtPhone", "txtAddress", "txtFax", "txtCity", "txtState", "txtZip", "txtMobilePhone", "txtEMail", "txtLic_No", "txtDwellingUnits", "txtParkingArea", "txtParkingTotal", "txtParkingEnclosed", "chkBSA_Variance", "txtBSA_Variance_CalNo", "chkBSA_SpecialPermit", "txtBSA_SpecialPermit_CalNo", "txtBSA_SpecialPermit_AZS", "chkBSA_GCLW", "chkBSAOther", "txtBSA_GCLW_CalNo", "txtBSAOther_CalNo", "txtBSA_GCLW_Section", "chkCPC_SpecialPermit", "txtCPC_ULURP", "txtCPC_SpecialPermit_AZS", "chkCPC_Authorization", "txtCPC_Authorization_AppNo", "txtCPC_Authorization_AZS", "chkCPC_Certification", "txtCPC_Certification_AppNo", "txtCPC_Certification_AZS", "chkCPC_Other", "txtCPC_Other_AppNo" };
                    ICollection<JobDocumentField> jobDocumentFieldList = jobDocument.JobDocumentFields;
                    foreach (var item in fieldList)
                    {
                        JobDocumentField jobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.PdfFields == item);
                        if (jobDocumentField != null)
                        {
                            string actualValue = jobDocumentField.ActualValue;
                            field.SetField(item, string.Empty);
                            field.SetField(item, actualValue);
                        }
                    }

                    field.GenerateAppearances = false;
                    pdfStamper.Close();
                    reader.Close();
                    reader_JobDocument.Close();
                    SplitAndAppendLastPage(jobDocumentFile, jobDocumentFile_Template, jobDocumentFile_Temp);

                    ////sunay- code for updating page number///
                    var reader_changePageNumber = new PdfReader(jobDocumentFile);

                    string jobDocumentFile_ChangePage = "ChangePage_" + Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;
                    jobDocumentFile_ChangePage = Path.Combine(jobDocumentDirectory, jobDocumentFile_ChangePage);

                    var pdfStamper_ChangePageNumber = new PdfStamper(reader_changePageNumber, new FileStream(jobDocumentFile_ChangePage, FileMode.Create));
                    var field_changePageNumber = pdfStamper_ChangePageNumber.AcroFields;

                    //field_changePageNumber.SetField("txtPageCount", Convert.ToString(numberofPages + 1));

                    field_changePageNumber.SetField("txtNumSheets", Convert.ToString(numberofPages));
                    //for (int i = 1; i <= numberofPages; i++)
                    //{
                    //    if (field_changePageNumber.Fields.Keys.Contains("Page" + Convert.ToString(2 + i) + "_txtPageCount_1"))
                    //        field_changePageNumber.SetField("Page" + Convert.ToString(2 + i) + "_txtPageCount_1", Convert.ToString(numberofPages + 1));

                    //}

                    field_changePageNumber.GenerateAppearances = false;
                    pdfStamper_ChangePageNumber.Close();
                    reader_changePageNumber.Close();

                    if (File.Exists(jobDocumentFile))
                    {
                        File.Delete(jobDocumentFile);
                    }

                    if (File.Exists(jobDocumentFile_ChangePage))
                    {
                        File.Copy(jobDocumentFile_ChangePage, jobDocumentFile, true);
                    }

                    if (File.Exists(jobDocumentFile_ChangePage))
                    {
                        File.Delete(jobDocumentFile_ChangePage);
                    }

                    ////end sunay


                }
                #endregion

                #region CCD1
                else if (jobDocument.IdDocument == Document.CCD1ConstructionCodeDeterminationForm.GetHashCode())
                {
                    templatefilePath = Path.Combine(path, templatefilePath);
                    templatefileName = Path.Combine(templatefilePath, "CCD1_AddPageTemplate.pdf");
                    PdfReader.unethicalreading = true;
                    var reader = new PdfReader(templatefileName);

                    var pdfStamper = new PdfStamper(reader, new FileStream(jobDocumentFile_Template, FileMode.Create));
                    var field = pdfStamper.AcroFields;
                    var reader_JobDocument = new PdfReader(jobDocumentFile);
                    int numberofPages = reader_JobDocument.NumberOfPages;

                    JobDocumentField df = jobDocumentResponse.JobDocumentFields.Where(x => x.DocumentField.PdfFields == "txtName").FirstOrDefault();

                    string applicantname = string.Empty;

                    if (df != null)
                    {
                        applicantname = df.ActualValue;
                    }


                    field.SetField("txtPage", Convert.ToString(numberofPages + 1));
                    field.SetField("txtDescription ", string.Empty);
                    field.SetField("txtName", applicantname);
                    field.SetField("txtDate", string.Empty);

                    field.RenameField("txtPage", "Page" + Convert.ToString(numberofPages + 1) + "_txtPage");
                    field.RenameField("txtDescription", "Page" + Convert.ToString(numberofPages + 1) + "_txtDescription");
                    field.RenameField("txtName", "Page" + Convert.ToString(numberofPages + 1) + "_txtName");
                    field.RenameField("txtDate", "Page" + Convert.ToString(numberofPages + 1) + "_txtDate");

                    field.GenerateAppearances = false;
                    pdfStamper.Close();
                    reader.Close();
                    reader_JobDocument.Close();
                    SplitAndAppendLastPage(jobDocumentFile, jobDocumentFile_Template, jobDocumentFile_Temp);
                }
                #endregion

                #region PW-1A
                else if (jobDocument.IdDocument == Document.SchedulePW1A.GetHashCode())
                {
                    templatefilePath = Path.Combine(path, templatefilePath);
                    templatefileName = Path.Combine(templatefilePath, "PW1A_new.pdf");
                    PdfReader.unethicalreading = true;
                    var reader = new PdfReader(templatefileName);

                    var pdfStamper = new PdfStamper(reader, new FileStream(jobDocumentFile_Template, FileMode.Create));
                    var field = pdfStamper.AcroFields;
                    var reader_JobDocument = new PdfReader(jobDocumentFile);
                    int numberofPages = reader_JobDocument.NumberOfPages;

                    field.SetField("txtPageCount_0", Convert.ToString(numberofPages + 1));
                    field.SetField("txtPageCount_1", Convert.ToString(numberofPages + 1));
                    field.SetField("txtDescription ", string.Empty);
                    field.SetField("txtApplicant", string.Empty);
                    field.SetField("txtDate", string.Empty);
                    int pagesecondlast = numberofPages - 1;
                    field.SetField("txtPageNo", numberofPages.ToString());

                    field.SetField("txtPage1", pagesecondlast.ToString());
                    field.RenameField("txtPageCount_0", "Page" + Convert.ToString(numberofPages + 1) + "_txtPageCount_0");
                    field.RenameField("txtPageCount_1", "Page" + Convert.ToString(numberofPages + 1) + "_txtPageCount_1");
                    field.RenameField("txtDescription", "Page" + Convert.ToString(numberofPages + 1) + "_txtDescription");
                    field.RenameField("txtApplicant", "Page" + Convert.ToString(numberofPages + 1) + "_txtApplicant");
                    field.RenameField("txtDate", "Page" + Convert.ToString(numberofPages + 1) + "_txtDate");
                    field.RenameField("txtPageNo", "Page" + Convert.ToString(numberofPages + 1) + "txtPageNo");

                    string[] fieldList1 = { "f1", "f2", "f3", "f4", "f5", "f6", "f7", "f8", "f9", "f10", "f11", "f12", "f13", "f14", "f15", "g1", "g2", "g3", "g4", "g5", "g6", "g7", "g8", "g9", "g10", "g11", "g12", "g13", "g14", "g15", "h1", "h2", "h3", "h4", "h5", "h6", "h7", "h8", "h9", "h10", "h11", "h12", "h13", "h14", "h15", "i1", "i2", "i3", "i4", "i5", "i6", "i7", "i8", "i9", "i10", "i11", "i12", "i13", "i14", "i15", "j1", "j2", "j3", "j4", "j5", "j6", "j7", "j8", "j9", "j10", "j11", "j12", "j13", "j14", "j15" };

                    foreach (var item in fieldList1)
                    {

                        field.RenameField(item, "Page" + Convert.ToString(numberofPages + 1) + item);
                    }

                    //var docfield = new PdfStamper(reader_JobDocument, new FileStream(jobDocumentFile_Template, FileMode.OpenOrCreate));
                    //var docfield = reader_JobDocument.AcroFields;
                    //docfield.SetField("topmostSubform[0].Page1[0].Floor__Row_1[0]", "sunay");

                    field.GenerateAppearances = false;
                    pdfStamper.Close();
                    reader.Close();
                    reader_JobDocument.Close();
                    // SplitAndAppendLastPage(jobDocumentFile, jobDocumentFile_Template, jobDocumentFile_Temp);
                    AppendPageAtspecificposition_pw1(jobDocumentFile, jobDocumentFile_Template, jobDocumentFile_Temp);

                    var reader_changePageNumber = new PdfReader(jobDocumentFile);

                    string jobDocumentFile_ChangePage = "ChangePage_" + Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;
                    jobDocumentFile_ChangePage = Path.Combine(jobDocumentDirectory, jobDocumentFile_ChangePage);

                    var pdfStamper_ChangePageNumber = new PdfStamper(reader_changePageNumber, new FileStream(jobDocumentFile_ChangePage, FileMode.Create));
                    var field_changePageNumber = pdfStamper_ChangePageNumber.AcroFields;

                    field_changePageNumber.SetField("txtPageCount", Convert.ToString(numberofPages + 1));


                    for (int i = 1; i <= numberofPages; i++)
                    {
                        if (field_changePageNumber.Fields.Keys.Contains("Page" + Convert.ToString(2 + i) + "_txtPageCount_1"))
                            field_changePageNumber.SetField("Page" + Convert.ToString(2 + i) + "_txtPageCount_1", Convert.ToString(numberofPages + 1));


                        if (field_changePageNumber.Fields.Keys.Contains("Page" + (2 + i) + "_txtApplicant"))
                            field_changePageNumber.SetField("Page" + (2 + i) + "_txtApplicant", Convert.ToString(field_changePageNumber.GetField("txtApplicant")));


                    }

                    field_changePageNumber.GenerateAppearances = false;
                    pdfStamper_ChangePageNumber.Close();
                    reader_changePageNumber.Close();

                    if (File.Exists(jobDocumentFile))
                    {
                        File.Delete(jobDocumentFile);
                    }

                    if (File.Exists(jobDocumentFile_ChangePage))
                    {
                        File.Copy(jobDocumentFile_ChangePage, jobDocumentFile, true);
                    }

                    if (File.Exists(jobDocumentFile_ChangePage))
                    {
                        File.Delete(jobDocumentFile_ChangePage);
                    }
                }
                #endregion

                #region PW-1A-12/22
                else if (jobDocument.IdDocument == Document.SchedulePW1A2023.GetHashCode())
                {
                    templatefilePath = Path.Combine(path, templatefilePath);
                    templatefileName = Path.Combine(templatefilePath, "PW_1A_1222.pdf");
                    PdfReader.unethicalreading = true;
                    var reader = new PdfReader(templatefileName);

                    var pdfStamper = new PdfStamper(reader, new FileStream(jobDocumentFile_Template, FileMode.Create));
                    var field = pdfStamper.AcroFields;
                    var reader_JobDocument = new PdfReader(jobDocumentFile);
                    int numberofPages = reader_JobDocument.NumberOfPages;

                    field.SetField("txtPageCount_0", Convert.ToString(numberofPages + 1));
                    field.SetField("txtPageCount_1", Convert.ToString(numberofPages + 1));
                    field.SetField("txtDescription ", string.Empty);
                    field.SetField("txtApplicant", string.Empty);
                    field.SetField("txtDate", string.Empty);
                    int pagesecondlast = numberofPages - 1;
                    field.SetField("txtPageNo", numberofPages.ToString());

                    field.SetField("txtPage1", pagesecondlast.ToString());
                    field.RenameField("txtPageCount_0", "Page" + Convert.ToString(numberofPages + 1) + "_txtPageCount_0");
                    field.RenameField("txtPageCount_1", "Page" + Convert.ToString(numberofPages + 1) + "_txtPageCount_1");
                    field.RenameField("txtDescription", "Page" + Convert.ToString(numberofPages + 1) + "_txtDescription");
                    field.RenameField("txtApplicant", "Page" + Convert.ToString(numberofPages + 1) + "_txtApplicant");
                    field.RenameField("txtDate", "Page" + Convert.ToString(numberofPages + 1) + "_txtDate");
                    field.RenameField("txtPageNo", "Page" + Convert.ToString(numberofPages + 1) + "txtPageNo");

                    string[] fieldList1 = { "f1", "f2", "f3", "f4", "f5", "f6", "f7", "f8", "f9", "f10", "f11", "f12", "f13", "f14", "f15", "g1", "g2", "g3", "g4", "g5", "g6", "g7", "g8", "g9", "g10", "g11", "g12", "g13", "g14", "g15", "h1", "h2", "h3", "h4", "h5", "h6", "h7", "h8", "h9", "h10", "h11", "h12", "h13", "h14", "h15", "i1", "i2", "i3", "i4", "i5", "i6", "i7", "i8", "i9", "i10", "i11", "i12", "i13", "i14", "i15", "j1", "j2", "j3", "j4", "j5", "j6", "j7", "j8", "j9", "j10", "j11", "j12", "j13", "j14", "j15" };

                    foreach (var item in fieldList1)
                    {

                        field.RenameField(item, "Page" + Convert.ToString(numberofPages + 1) + item);
                    }

                    //var docfield = new PdfStamper(reader_JobDocument, new FileStream(jobDocumentFile_Template, FileMode.OpenOrCreate));
                    //var docfield = reader_JobDocument.AcroFields;
                    //docfield.SetField("topmostSubform[0].Page1[0].Floor__Row_1[0]", "sunay");

                    field.GenerateAppearances = false;
                    pdfStamper.Close();
                    reader.Close();
                    reader_JobDocument.Close();
                    // SplitAndAppendLastPage(jobDocumentFile, jobDocumentFile_Template, jobDocumentFile_Temp);
                    AppendPageAtspecificposition_pw1(jobDocumentFile, jobDocumentFile_Template, jobDocumentFile_Temp);

                    var reader_changePageNumber = new PdfReader(jobDocumentFile);

                    string jobDocumentFile_ChangePage = "ChangePage_" + Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;
                    jobDocumentFile_ChangePage = Path.Combine(jobDocumentDirectory, jobDocumentFile_ChangePage);

                    var pdfStamper_ChangePageNumber = new PdfStamper(reader_changePageNumber, new FileStream(jobDocumentFile_ChangePage, FileMode.Create));
                    var field_changePageNumber = pdfStamper_ChangePageNumber.AcroFields;

                    field_changePageNumber.SetField("txtPageCount", Convert.ToString(numberofPages + 1));


                    for (int i = 1; i <= numberofPages; i++)
                    {
                        if (field_changePageNumber.Fields.Keys.Contains("Page" + Convert.ToString(2 + i) + "_txtPageCount_1"))
                            field_changePageNumber.SetField("Page" + Convert.ToString(2 + i) + "_txtPageCount_1", Convert.ToString(numberofPages + 1));


                        if (field_changePageNumber.Fields.Keys.Contains("Page" + (2 + i) + "_txtApplicant"))
                            field_changePageNumber.SetField("Page" + (2 + i) + "_txtApplicant", Convert.ToString(field_changePageNumber.GetField("txtApplicant")));


                    }

                    field_changePageNumber.GenerateAppearances = false;
                    pdfStamper_ChangePageNumber.Close();
                    reader_changePageNumber.Close();

                    if (File.Exists(jobDocumentFile))
                    {
                        File.Delete(jobDocumentFile);
                    }

                    if (File.Exists(jobDocumentFile_ChangePage))
                    {
                        File.Copy(jobDocumentFile_ChangePage, jobDocumentFile, true);
                    }

                    if (File.Exists(jobDocumentFile_ChangePage))
                    {
                        File.Delete(jobDocumentFile_ChangePage);
                    }
                }

                #endregion
                #region AI-1
                else if (jobDocument.IdDocument == Document.AI162402.GetHashCode())
                {
                    templatefilePath = Path.Combine(path, templatefilePath);
                    templatefileName = Path.Combine(templatefilePath, "AI-1_AddPageTemplate.pdf");

                    PdfReader.unethicalreading = true;
                    var reader = new PdfReader(templatefileName);

                    var pdfStamper = new PdfStamper(reader, new FileStream(jobDocumentFile_Template, FileMode.Create));
                    var field = pdfStamper.AcroFields;
                    var reader_JobDocument = new PdfReader(jobDocumentFile);
                    int numberofPages = reader_JobDocument.NumberOfPages;

                    field.SetField("JobFileInfo", (jobDocument.Job != null ? jobDocument.Job.JobNumber : string.Empty) + " - AI-1-76625.pdf");
                    field.SetField("Sheet", Convert.ToString(numberofPages + 1));
                    field.SetField("of", Convert.ToString(numberofPages + 1));

                    field.RenameField("Sheet", "Page" + Convert.ToString(numberofPages + 1) + "_Sheet");



                    // string[] fieldList = { "topmostSubform[0].Page1[0].Action__Row_1[0]", "topmostSubform[0].Page1[0].OriginalNew_Omit_Page_ID__Row_1[0]", "topmostSubform[0].Page1[0].Superseding_Page_ID__Row_1[0]", "topmostSubform[0].Page1[0].Action__Row_1_2[0]", "topmostSubform[0].Page1[0].OriginalNew_Omit_Page_ID__Row_1_2[0]", "topmostSubform[0].Page1[0].Superseding_Page_ID__Row_1_2[0]", "topmostSubform[0].Page1[0].Action__Row_1_3[0]", "topmostSubform[0].Page1[0].OriginalNew_Omit_Page_ID__Row_1_3[0]", "topmostSubform[0].Page1[0].Superseding_Page_ID__Row_1_3[0]", "topmostSubform[0].Page1[0].Action__Row_1_4[0]", "topmostSubform[0].Page1[0].OriginalNew_Omit_Page_ID__Row_1_4[0]", "topmostSubform[0].Page1[0].Superseding_Page_ID__Row_1_4[0]", "topmostSubform[0].Page1[0].Action__Row_2[0]", "topmostSubform[0].Page1[0].OriginalNew_Omit_Page_ID__Row_2[0]", "topmostSubform[0].Page1[0].Superseding_Page_ID__Row_2[0]", "topmostSubform[0].Page1[0].Action__Row_2_2[0]", "topmostSubform[0].Page1[0].OriginalNew_Omit_Page_ID__Row_2_2[0]", "topmostSubform[0].Page1[0].Superseding_Page_ID__Row_2_2[0]", "topmostSubform[0].Page1[0].Action__Row_2_3[0]", "topmostSubform[0].Page1[0].OriginalNew_Omit_Page_ID__Row_2_3[0]", "topmostSubform[0].Page1[0].Superseding_Page_ID__Row_2_3[0]", "topmostSubform[0].Page1[0].Action__Row_2_4[0]", "topmostSubform[0].Page1[0].OriginalNew_Omit_Page_ID__Row_2_4[0]", "topmostSubform[0].Page1[0].Superseding_Page_ID__Row_2_4[0]", "topmostSubform[0].Page1[0].Action__Row_3[0]", "topmostSubform[0].Page1[0].OriginalNew_Omit_Page_ID__Row_3[0]", "topmostSubform[0].Page1[0].Superseding_Page_ID__Row_3[0]", "topmostSubform[0].Page1[0].Action__Row_3_2[0]", "topmostSubform[0].Page1[0].OriginalNew_Omit_Page_ID__Row_3_2[0]", "topmostSubform[0].Page1[0].Superseding_Page_ID__Row_3_2[0]", "topmostSubform[0].Page1[0].Action__Row_3_3[0]", "topmostSubform[0].Page1[0].OriginalNew_Omit_Page_ID__Row_3_3[0]", "topmostSubform[0].Page1[0].Superseding_Page_ID__Row_3_3[0]", "topmostSubform[0].Page1[0].Action__Row_3_4[0]", "topmostSubform[0].Page1[0].OriginalNew_Omit_Page_ID__Row_3_4[0]", "topmostSubform[0].Page1[0].Superseding_Page_ID__Row_3_4[0]", "topmostSubform[0].Page1[0].Action__Row_4[0]", "topmostSubform[0].Page1[0].OriginalNew_Omit_Page_ID__Row_4[0]", "topmostSubform[0].Page1[0].Superseding_Page_ID__Row_4[0]", "topmostSubform[0].Page1[0].Action__Row_4_2[0]", "topmostSubform[0].Page1[0].OriginalNew_Omit_Page_ID__Row_4_2[0]", "topmostSubform[0].Page1[0].Superseding_Page_ID__Row_4_2[0]", "topmostSubform[0].Page1[0].Action__Row_4_3[0]", "topmostSubform[0].Page1[0].OriginalNew_Omit_Page_ID__Row_4_3[0]", "topmostSubform[0].Page1[0].Superseding_Page_ID__Row_4_3[0]", " topmostSubform[0].Page1[0].Action__Row_4_4[0]", " topmostSubform[0].Page1[0].OriginalNew_Omit_Page_ID__Row_4_4[0]", "topmostSubform[0].Page1[0].Superseding_Page_ID__Row_4_4[0] " };
                    string[] fieldList = { "ActionRow1", "ActionRow2", "ActionRow3", "ActionRow4", "ActionRow5", "ActionRow6", "ActionRow7", "ActionRow8", "ActionRow9", "ActionRow10", "ActionRow11", "ActionRow12", "ActionRow13", "ActionRow14", "ActionRow15", "ActionRow16", "ActionRow17", "ActionRow18", "ActionRow19", "ActionRow20", "ActionRow21", "ActionRow22", "ActionRow23", "ActionRow24", "ActionRow25", "ActionRow26", "ActionRow27", "ActionRow28", "ActionRow29", "ActionRow30", "ActionRow31", "ActionRow32", "ActionRow33", "ActionRow34", "ActionRow35", "ActionRow36", "ActionRow37", "ActionRow38", "ActionRow39", "ActionRow40", "ActionRow41", "ActionRow42", "ActionRow43", "ActionRow44", "ActionRow45", "ActionRow46", "ActionRow47", "ActionRow48", "Amendment" };

                    ICollection<JobDocumentField> jobDocumentFieldList = jobDocument.JobDocumentFields;


                    foreach (var item in fieldList)
                    {
                        JobDocumentField jobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.PdfFields == item);
                        if (jobDocumentField != null)
                        {
                            string actualValue = jobDocumentField.ActualValue;
                            field.SetField(item, string.Empty);
                            field.SetField(item, actualValue);


                        }

                        //    field.RenameField(item.ToString(), "Page" + Convert.ToString(numberofPages + 1) + item);
                    }



                    foreach (var item in fieldList)
                    {

                        field.RenameField(item, "Page" + Convert.ToString(numberofPages + 1) + item);
                    }


                    field.GenerateAppearances = false;
                    pdfStamper.Close();
                    reader.Close();
                    reader_JobDocument.Close();
                    SplitAndAppendLastPage(jobDocumentFile, jobDocumentFile_Template, jobDocumentFile_Temp);
                    var reader_changePageNumber = new PdfReader(jobDocumentFile);
                    int numberofPages_changePageNumber = reader_changePageNumber.NumberOfPages;

                    string jobDocumentFile_ChangePage = "ChangePage_" + Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;
                    jobDocumentFile_ChangePage = Path.Combine(jobDocumentDirectory, jobDocumentFile_ChangePage);

                    var pdfStamper_ChangePageNumber = new PdfStamper(reader_changePageNumber, new FileStream(jobDocumentFile_ChangePage, FileMode.Create));
                    var field_changePageNumber = pdfStamper_ChangePageNumber.AcroFields;

                    field_changePageNumber.SetField("of", Convert.ToString(numberofPages_changePageNumber));

                    field_changePageNumber.GenerateAppearances = false;
                    pdfStamper_ChangePageNumber.Close();
                    reader_changePageNumber.Close();

                    if (File.Exists(jobDocumentFile))
                    {
                        File.Delete(jobDocumentFile);
                    }
                    if (File.Exists(jobDocumentFile_ChangePage))
                    {
                        File.Copy(jobDocumentFile_ChangePage, jobDocumentFile, true);
                    }
                    if (File.Exists(jobDocumentFile_ChangePage))
                    {
                        File.Delete(jobDocumentFile_ChangePage);
                    }
                }
                #endregion

                var instance = new DropboxIntegration();
                string uploadFilePath = Properties.Settings.Default.DropboxFolderPath + Convert.ToString(jobDocument.IdJob);
                string dropBoxFileName = Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;
                string filepath = jobDocumentFile;
                var task = instance.RunUpload(uploadFilePath, dropBoxFileName, filepath);
                //var a = jobDocumentResponse.JobDocumentFields.Select(x => x.DocumentField).Select(x => x.Field).Where(x => x.DisplayFieldName == "Applicant");
                return Ok(FormatDetails(jobDocumentResponse));
            }
            else
            {
                throw new RpoBusinessException("Document not found!");
            }
        }



        private static void SplitAndAppendLastPage(string jobDocumentPath, string pdfFilePath, string tempFileName)
        {
            PdfCopyFields copy = new PdfCopyFields(new FileStream(tempFileName, FileMode.Create));

            PdfReader reader = new PdfReader(jobDocumentPath);
            copy.AddDocument(reader);

            PdfReader reader_Addpage = new PdfReader(pdfFilePath);
            copy.AddDocument(reader_Addpage);

            copy.Close();
            reader.Close();
            reader_Addpage.Close();

            if (File.Exists(pdfFilePath))
            {
                File.Delete(pdfFilePath);
            }

            if (File.Exists(tempFileName))
            {
                File.Copy(tempFileName, jobDocumentPath, true);
            }

            if (File.Exists(tempFileName))
            {
                File.Delete(tempFileName);
            }
        }

        private static void CombineMultiplePDFs(string jobDocumentPath, string pdfFilePath, string tempFileName)
        {
            string[] fileNames = { jobDocumentPath, pdfFilePath };
            string outFile = tempFileName;

            iTextSharp.text.Document document = new iTextSharp.text.Document();
            using (FileStream newFileStream = new FileStream(outFile, FileMode.Create))
            {
                // step 2: we create a writer that listens to the document
                PdfCopy writer = new PdfCopy(document, newFileStream);
                if (writer == null)
                {
                    return;
                }

                // step 3: we open the document
                document.Open();
                writer.SetMergeFields();
                foreach (string fileName in fileNames)
                {
                    // we create a reader for a certain document
                    PdfReader reader = new PdfReader(fileName);
                    reader.ConsolidateNamedDestinations();

                    // step 4: we add content
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        writer.AddPage(page);
                    }

                    //PRAcroForm form = reader.AcroForm;
                    //if (form != null)
                    //{
                    //    writer.CopyAcroForm(reader);
                    //}

                    reader.Close();
                }

                // step 5: we close the document and writer
                writer.Close();
                document.Close();
            }//disposes the newFileStream object

            if (File.Exists(pdfFilePath))
            {
                File.Delete(pdfFilePath);
            }

            if (File.Exists(tempFileName))
            {
                File.Copy(tempFileName, jobDocumentPath, true);
            }

            if (File.Exists(tempFileName))
            {
                File.Delete(tempFileName);
            }
        }

        private static void AppendPageAtspecificposition_pw1(string jobDocumentPath, string pdfFilePath, string tempFileName)
        {
            PdfCopyFields copy = new PdfCopyFields(new FileStream(tempFileName, FileMode.Create));

            PdfReader reader = new PdfReader(jobDocumentPath);
            // copy.AddDocument(reader);
            var ilist = new List<int>();

            for (int i = 1; i <= reader.NumberOfPages - 1; i++)
            {
                ilist.Add(i);
            }

            copy.AddDocument(reader, ilist);

            PdfReader reader_Addpage = new PdfReader(pdfFilePath);
            copy.AddDocument(reader_Addpage);



            var ilist2 = new List<int>();

            ilist2.Add(reader.NumberOfPages);

            copy.AddDocument(reader, ilist2);

            copy.Close();
            reader.Close();
            reader_Addpage.Close();

            if (File.Exists(pdfFilePath))
            {
                File.Delete(pdfFilePath);
            }

            if (File.Exists(tempFileName))
            {
                File.Copy(tempFileName, jobDocumentPath, true);
            }

            if (File.Exists(tempFileName))
            {
                File.Delete(tempFileName);
            }
        }




        private void UpdatePermitDates(string filepath, string permitNumber, int idJobApplicationPermit)
        {
            try
            {
                if (idJobApplicationPermit > 0)
                {
                    JobApplicationWorkPermitType jobApplicationWorkPermitType = rpoContext.JobApplicationWorkPermitTypes.Include("JobApplication").FirstOrDefault(x => x.Id == idJobApplicationPermit);
                    if (jobApplicationWorkPermitType != null)
                    {
                        iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(filepath);
                        int intPageNum = reader.NumberOfPages;
                        string[] words;
                        string line = string.Empty;
                        string text = string.Empty;

                        for (int i = 1; i <= intPageNum; i++)
                        {
                            text = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, i, new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy());

                            words = text.Split('\n');

                            for (int j = 0, len = words.Length; j < len; j++)
                            {
                                line = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j]));

                                if (line.Contains(jobApplicationWorkPermitType.JobApplication.ApplicationNumber))
                                {
                                    string issue = string.Empty;
                                    string expire = string.Empty;
                                    //line = line.Replace(permitNumber, "");
                                    string[] dates = line.Split(' ');
                                    if (dates != null && dates.Length > 2)
                                    {
                                        issue = dates[1].Replace("/", ",").Replace("/", ",");
                                        expire = dates[2].Replace("/", ",").Replace("/", ",");

                                        string[] issueArray = issue.Split(',');
                                        if (issueArray != null && issueArray.Length > 2)
                                        {
                                            DateTime issueDate = new DateTime(Convert.ToInt32(issueArray[2]), Convert.ToInt32(issueArray[0]), Convert.ToInt32(issueArray[1]));
                                            jobApplicationWorkPermitType.Issued = issueDate;
                                        }
                                        string[] expireArray = expire.Split(',');

                                        if (expireArray != null && expireArray.Length > 2)
                                        {
                                            DateTime ExpireDate = new DateTime(Convert.ToInt32(expireArray[2]), Convert.ToInt32(expireArray[0]), Convert.ToInt32(expireArray[1]));
                                            jobApplicationWorkPermitType.Expires = ExpireDate;
                                        }
                                    }
                                }
                                else if (line.Contains("Business:"))
                                {
                                    string Permittee = line.Replace("Business:", "");
                                    jobApplicationWorkPermitType.Permittee = Permittee;
                                }
                            }
                        }


                        rpoContext.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {


            }
        }

        /// <summary>
        /// get and set the permit date of the jobdocument
        /// </summary>
        /// <param name="idJobDocument">The pull id document.</param>
        /// <returns>update ther permites dates.</returns>

        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/jobdocument/PermitDates")]
        public IHttpActionResult PermitDates(JobDocumentPermitDTO dto)
        {

            PdfReader reader = new PdfReader(dto.DetailUrl);
            string[] document = dto.NumberDocType.Split('-');
            int jobapplicationid = Convert.ToInt32(document[0]);
            string applicationnumber = document[0];
            string ApplicationFor = rpoContext.JobApplications.Where(x => x.ApplicationNumber == applicationnumber).Select(x => x.ApplicationFor).FirstOrDefault();
            int id = rpoContext.JobApplications.Where(x => x.ApplicationNumber == applicationnumber).Select(x => x.Id).FirstOrDefault();

            int intPageNum = reader.NumberOfPages;
            string[] words;
            string line = string.Empty;
            string text = string.Empty;
            for (int i = 1; i <= intPageNum; i++)
            {
                text = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, i, new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy());

                words = text.Split('\n');

                for (int j = 0, len = words.Length; j < len; j++)
                {
                    line = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j]));

                    if (line.Contains(Convert.ToString(jobapplicationid)))
                    {
                        string issue = string.Empty;
                        string expire = string.Empty;
                        //line = line.Replace(permitNumber, "");
                        string[] dates = line.Split(' ');
                        if (dates != null && dates.Length > 2)
                        {
                            issue = dates[1].Replace("/", ",").Replace("/", ",");
                            expire = dates[2].Replace("/", ",").Replace("/", ",");

                            string[] issueArray = issue.Split(',');
                            if (issueArray != null && issueArray.Length > 2)
                            {
                                DateTime issueDate = new DateTime(Convert.ToInt32(issueArray[2]), Convert.ToInt32(issueArray[0]), Convert.ToInt32(issueArray[1]));
                                dto.IssuedDate = issueDate;
                            }
                            string[] expireArray = expire.Split(',');

                            if (expireArray != null && expireArray.Length > 2)
                            {
                                DateTime ExpireDate = new DateTime(Convert.ToInt32(expireArray[2]), Convert.ToInt32(expireArray[0]), Convert.ToInt32(expireArray[1]));
                                dto.ExpiredDate = ExpireDate;
                            }
                        }
                    }
                    else if (line.Contains("Business:"))
                    {
                        string Permittee = line.Replace("Business:", "");
                        dto.Permitee = Permittee;
                    }
                }
            }
            dto.JobApplicationWorkPermitTypeId = rpoContext.JobApplicationWorkPermitTypes.Where(x => x.IdJobApplication == id).Select(x => x.Id).FirstOrDefault();
            return Ok(dto);

        }

        /// <summary>
        /// update ther permites dates
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>update ther permites dates</returns>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobdocument/PutPermitDates")]
        public IHttpActionResult PutPermitDates(JobDocumentPermitDTO dto)
        {
            if (dto.JobApplicationWorkPermitTypeId != null && dto.JobApplicationWorkPermitTypeId != 0)
            {
                JobApplicationWorkPermitType jobApplicationWorkPermitType = rpoContext.JobApplicationWorkPermitTypes.Include("JobApplication.JobApplicationType")
                      .Include("JobApplication.ApplicationStatus")
                      .Include("JobApplication.Job")
                      .Include("JobWorkType")
                      .Include("ContactResponsible")
                      .FirstOrDefault(r => r.Id == dto.JobApplicationWorkPermitTypeId);

                //JobApplicationWorkPermitType jobApplicationWorkPermitType = rpoContext.JobApplicationWorkPermitTypes.Where(x => x.Id == dto.JobApplicationWorkPermitTypeId).Select(x => x).FirstOrDefault();
                jobApplicationWorkPermitType.Issued = dto.IssuedDate;
                jobApplicationWorkPermitType.Expires = dto.ExpiredDate;
                jobApplicationWorkPermitType.Permittee = dto.Permitee;
                jobApplicationWorkPermitType.DefaultUrl = dto.DetailUrl;
                rpoContext.Entry(jobApplicationWorkPermitType).State = EntityState.Modified;
                try
                {
                    string jobApplicationTypeName = string.Empty;
                    if (jobApplicationWorkPermitType.JobApplication != null && jobApplicationWorkPermitType.JobApplication.JobApplicationType != null)
                    {
                        jobApplicationTypeName = Convert.ToString(jobApplicationWorkPermitType.JobApplication.JobApplicationType.Description);
                    }

                    string EditWorkPermit_DOB = JobHistoryMessages.EditWorkPermit_DOB
                                                      .Replace("##PermitType##", jobApplicationWorkPermitType.JobWorkType != null ? jobApplicationWorkPermitType.JobWorkType.Description : JobHistoryMessages.NoSetstring)
                                                      .Replace("##ApplicationType##", !string.IsNullOrEmpty(jobApplicationTypeName) ? jobApplicationTypeName : JobHistoryMessages.NoSetstring)
                                                      .Replace("##ApplicationNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.ApplicationNumber) ? jobApplicationWorkPermitType.JobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring)
                                                      .Replace("##EstimatedCost##", jobApplicationWorkPermitType.EstimatedCost != null && jobApplicationWorkPermitType.EstimatedCost != 0 ? jobApplicationWorkPermitType.EstimatedCost.ToString() : JobHistoryMessages.NoSetstring)
                                                      .Replace("##WorkDescription##", !string.IsNullOrEmpty(jobApplicationWorkPermitType.WorkDescription) ? jobApplicationWorkPermitType.WorkDescription : JobHistoryMessages.NoSetstring)
                                                      .Replace("##RPOORPersonName##", jobApplicationWorkPermitType.IdResponsibility == 2 ? ((from d in rpoContext.Contacts where d.Id == jobApplicationWorkPermitType.IdContactResponsible.Value select d.FirstName + " " + d.LastName).FirstOrDefault() != null ? (from d in rpoContext.Contacts where d.Id == jobApplicationWorkPermitType.IdContactResponsible.Value select d.FirstName + " " + d.LastName).FirstOrDefault() : JobHistoryMessages.NoSetstring) : (jobApplicationWorkPermitType.IdResponsibility == 1 ? "RPO" : JobHistoryMessages.NoSetstring))
                                                      .Replace("##FiledDate##", jobApplicationWorkPermitType.Filed != null ? jobApplicationWorkPermitType.Filed.Value.ToShortDateString() : JobHistoryMessages.NoSetstring)
                                                      .Replace("##IssuedDate##", dto.IssuedDate != null ? dto.IssuedDate.Value.ToShortDateString() : JobHistoryMessages.NoSetstring)
                                                      .Replace("##ExpiryDate##", dto.ExpiredDate != null ? dto.ExpiredDate.Value.ToShortDateString() : JobHistoryMessages.NoSetstring)
                                                      .Replace("##Permittee##", jobApplicationWorkPermitType.Permittee != null ? jobApplicationWorkPermitType.Permittee.ToString() : JobHistoryMessages.NoSetstring);

                    var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

                    Common.SaveJobHistory(employee.Id, jobApplicationWorkPermitType.JobApplication.IdJob, EditWorkPermit_DOB, Model.Models.Enums.JobHistoryType.WorkPermits);
                    string editApplicationNumber_DOB = JobHistoryMessages.EditApplicationNumber_DOB
                                        .Replace("##ApplicationType##", jobApplicationTypeName != null ? jobApplicationTypeName : JobHistoryMessages.NoSetstring)
                                        .Replace("##ApplicationStatus##", !string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.JobApplicationStatus) ? jobApplicationWorkPermitType.JobApplication.JobApplicationStatus : JobHistoryMessages.NoSetstring)
                                        .Replace("##ApplicationNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.ApplicationNumber) ? jobApplicationWorkPermitType.JobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);

                    Common.SaveJobHistory(employee.Id, jobApplicationWorkPermitType.JobApplication.IdJob, editApplicationNumber_DOB, Model.Models.Enums.JobHistoryType.Applications);

                    string Permit_Type = string.Empty;
                    string Permit_Sub_Type = string.Empty;
                    if (!string.IsNullOrEmpty(dto.NumberDocType))
                    {
                        string[] strsplit = dto.NumberDocType.Split('-');
                        if (strsplit != null && strsplit.Length > 1)
                        {

                        }
                        if (strsplit != null && strsplit.Length > 2)
                        {
                            string[] strspl = strsplit[2].ToString().Split(' ');
                            if (strspl != null && strspl.Length > 0)
                            {
                                Permit_Type = strspl[0].ToString();
                            }
                            if (strspl != null && strspl.Length > 1)
                            {
                                Permit_Sub_Type = strspl[1].ToString();
                            }
                        }
                    }


                    var objprmit = rpoContext.DOBPermitMappings.Where(d => d.IdJob == jobApplicationWorkPermitType.JobApplication.IdJob && d.IdJobApplication == jobApplicationWorkPermitType.IdJobApplication && d.IdWorkPermit == jobApplicationWorkPermitType.Id).OrderByDescending(d => d.Seq).FirstOrDefault();
                    if (objprmit != null)
                    {
                        objprmit.IdJob = jobApplicationWorkPermitType.JobApplication.IdJob;
                        objprmit.IdJobApplication = jobApplicationWorkPermitType.IdJobApplication;
                        objprmit.IdWorkPermit = jobApplicationWorkPermitType.Id;
                        objprmit.NumberDocType = dto.NumberDocType;
                        objprmit.Seq = dto.SeqNo;
                        objprmit.Permit = dto.Permitee;
                        objprmit.PermitType = Permit_Type;
                        objprmit.PermitSubType = Permit_Sub_Type;
                        objprmit.EntryDate = DateTime.UtcNow;
                    }
                    else
                    {
                        DOBPermitMapping objpermitmap = new DOBPermitMapping();
                        objpermitmap.IdJob = jobApplicationWorkPermitType.JobApplication.IdJob;
                        objpermitmap.IdJobApplication = jobApplicationWorkPermitType.IdJobApplication;
                        objpermitmap.IdWorkPermit = jobApplicationWorkPermitType.Id;
                        objpermitmap.NumberDocType = dto.NumberDocType;
                        objpermitmap.Seq = dto.SeqNo;
                        objpermitmap.Permit = dto.Permitee;
                        objpermitmap.PermitType = Permit_Type;
                        objpermitmap.PermitSubType = Permit_Sub_Type;
                        objpermitmap.EntryDate = DateTime.UtcNow;
                        rpoContext.DOBPermitMappings.Add(objpermitmap);
                    }
                    rpoContext.SaveChanges();
                }
                catch (Exception e)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                return Ok(dto);
            }
            else
            {
                throw new RpoBusinessException("JobApplication WorkPermitTypeId is not Null or zero!");
            }
        }
        /// <summary>
        /// Get the job documetn path
        /// </summary>
        /// <returns>Get the job documetn path</returns>
        [HttpGet]
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [Route("api/jobdocument/updatejobdocumentpath")]
        [RpoAuthorize]
        public IHttpActionResult updatejobdocumentpath()
        {
            IQueryable<JobDocument> jobDocuments = (dynamic)null;

            jobDocuments = this.rpoContext.JobDocuments.
            Include("DocumentMaster").
                ///Include("JobDocumentFields.DocumentField.Field").
                Include("JobApplication.JobApplicationType.Parent").
            Include("LastModifiedByEmployee").
            Where(x => x.DocumentPath.Contains("#")).AsQueryable();

            var recordsTotal = jobDocuments.Count();
            var recordsFiltered = recordsTotal;

            foreach (var item in jobDocuments)
            {
                item.DocumentPath = Uri.EscapeDataString(item.DocumentPath);
            }
            rpoContext.SaveChanges();

            return this.Ok();
        }

        private List<JobDocumentPermitDTO> ReadLOCDocumentURL(string jobApplicationNumber)
        {
            string urlAddress = string.Empty;

            urlAddress = $"http://a810-bisweb.nyc.gov/bisweb/JobsQueryByNumberServlet?passjobnumber={jobApplicationNumber}&passdocnumber=&go10=+GO+&requestid=0";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12;

            List<JobDocumentPermitDTO> jobDocumentPermitDTOList = new List<JobDocumentPermitDTO>();
            HtmlAgilityPack.HtmlDocument doc = new HtmlWeb().Load(urlAddress);
            var descendants = doc.DocumentNode.Descendants();
            JobDocumentPermitDTO dto = new JobDocumentPermitDTO();



            var printPermit = descendants.FirstOrDefault(n => n.Name == "a" && n.Attributes.Where(x => x.Name == "href" && x.Value.Contains("PrintLOC")).Count() > 0);
            //string zip = string.Empty;
            string printPdfUrl = string.Empty;
            if (printPermit != null)
            {
                //zip = printPermit.InnerHtml.Split(new string[] { "&nbsp;" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                //zip = !string.IsNullOrEmpty(zip) ? Regex.Replace(zip, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                printPdfUrl = printPermit.Attributes.FirstOrDefault(x => x.Name == "href" && x.Value.Contains("PrintLOC")).Value;
            }

            if (!string.IsNullOrEmpty(printPdfUrl))
            {
                string pdfPAth = $"http://a810-bisweb.nyc.gov/bisweb/{printPdfUrl}";

                dto.DownloadLink = pdfPAth;
            }
            jobDocumentPermitDTOList.Add(dto);
            return jobDocumentPermitDTOList;
        }

        /// <summary>
        /// Reads the permit list.
        /// </summary>
        /// <param name="jobApplicationNumber">The job application number.</param>
        /// <param name="binNumber">The bin number.</param>
        /// <returns>List&lt;JobDocumentPermitDTO&gt;.</returns>
        private List<JobDocumentPermitDTO> ReadPermitList(string jobApplicationNumber, string binNumber, string DocumentDescription, int idJob)
        {
            string urlAddress = string.Empty;
            List<JobDocumentPermitDTO> jobDocumentPermitDTOList = new List<JobDocumentPermitDTO>();
            try
            {
                if (string.IsNullOrEmpty(binNumber))
                {
                    urlAddress = $"https://a810-bisweb.nyc.gov/bisweb/PermitQueryByNumberServlet?passjobnumber={jobApplicationNumber}";
                }
                else
                {
                    urlAddress = $"https://a810-bisweb.nyc.gov/bisweb/JobsPermitsDisplayServlet?requestid=4&passjobnumber={jobApplicationNumber}&passdocnumber=01&allbin={binNumber}";
                }
                string[] desc = DocumentDescription.Split(':');
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                   | SecurityProtocolType.Tls11
                                   | SecurityProtocolType.Tls12;
                HtmlAgilityPack.HtmlDocument doc = new HtmlWeb().Load(urlAddress);
                try
                {

                }
                catch (Exception ex)
                {
                    DOBErrorLog("Load URL: " + doc);
                }
                if (doc != null && doc.DocumentNode != null && doc.DocumentNode.Descendants() != null)
                {
                    var descendants = doc.DocumentNode.Descendants();
                    try
                    {

                    }
                    catch (Exception ex)
                    {
                        DOBErrorLog("Document Node: " + descendants);

                        DOBErrorLog("Document Text: " + doc.ParsedText.ToLower());
                    }

                    if (doc.ParsedText.ToLower().Contains("You don't have permission to access".ToLower().Trim()))
                    {
                        JobDocumentPermitDTO jobDocumentPermitDTO = new JobDocumentPermitDTO();
                        jobDocumentPermitDTO.isError = true;
                        jobDocumentPermitDTOList.Add(jobDocumentPermitDTO);

                    }
                    else
                    {
                        var block = descendants.FirstOrDefault(n => n.HasClass("centercolhdg") && n.InnerHtml.Contains("NUMBER-DOC-TYPE"));
                        if (block != null)
                        {
                            var blockParent = block.ParentNode.ParentNode;
                            int i = 0;
                            CultureInfo provider = CultureInfo.InvariantCulture;
                            foreach (var item in blockParent.ChildNodes)
                            {
                                if (item.NodeType == HtmlNodeType.Element && i > 3)
                                {
                                    int columnIndex = 1;
                                    JobDocumentPermitDTO jobDocumentPermitDTO = new JobDocumentPermitDTO();

                                    foreach (var childItem in item.ChildNodes)
                                    {
                                        jobDocumentPermitDTO.isError = false;
                                        if (childItem.NodeType == HtmlNodeType.Element)
                                        {
                                            switch (columnIndex)
                                            {

                                                case 1:
                                                    jobDocumentPermitDTO.NumberDocType = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                                    if (!string.IsNullOrEmpty(jobDocumentPermitDTO.NumberDocType) && childItem != null && childItem.FirstChild != null && childItem.FirstChild.Attributes != null)
                                                    {

                                                        if (childItem.FirstChild.Name == "a")
                                                        {
                                                            jobDocumentPermitDTO.DetailUrl = childItem.FirstChild.Attributes.FirstOrDefault(x => x.Name == "href" && x.Value.Contains("WorkPermitDataServlet")).Value;
                                                            string seqNo = string.Empty;
                                                            if (item.ChildNodes[5] != null && item.ChildNodes[5].InnerHtml != "")
                                                            {
                                                                seqNo = (item.ChildNodes[5]).InnerHtml;
                                                            }
                                                            if (!string.IsNullOrEmpty(jobDocumentPermitDTO.DetailUrl))
                                                            {

                                                                string filter = string.Empty;

                                                                string Permit_Type = string.Empty;
                                                                string Permit_Sub_Type = string.Empty;
                                                                if (!string.IsNullOrEmpty(jobDocumentPermitDTO.NumberDocType))
                                                                {
                                                                    string[] strsplit = jobDocumentPermitDTO.NumberDocType.Split('-');
                                                                    if (strsplit != null && strsplit.Length > 1)
                                                                    {

                                                                    }
                                                                    if (strsplit != null && strsplit.Length > 2)
                                                                    {
                                                                        string[] strspl = strsplit[2].ToString().Split(' ');
                                                                        if (strspl != null && strspl.Length > 0)
                                                                        {
                                                                            Permit_Type = strspl[0].ToString();
                                                                            filter = filter + "&permit_type='" + Permit_Type + "'";
                                                                        }
                                                                        if (strspl != null && strspl.Length > 1)
                                                                        {
                                                                            Permit_Sub_Type = strspl[1].ToString();
                                                                            filter = filter + "&permit_subtype='" + Permit_Sub_Type + "'";
                                                                        }
                                                                    }
                                                                }

                                                                string qry = string.Empty;
                                                                string isnno = string.Empty;
                                                                var client = new SodaClient("https://data.cityofnewyork.us", "7tPQriTSRiloiKdWIJoODgReN");
                                                                var dataset = client.GetResource<object>("ipu4-2q9a");  //ic3t-wcy2

                                                                ServicePointManager.Expect100Continue = true;
                                                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                                                   | SecurityProtocolType.Tls11
                                                                                   | SecurityProtocolType.Tls12;

                                                                var rows = dataset.GetRows(limit: 1000);

                                                                qry = qry + "job__='" + jobApplicationNumber.TrimEnd() + "'&permit_sequence__='" + seqNo.Trim() + "'" + filter;

                                                                var soql = new SoqlQuery().Select("job__", "job_doc___", "permit_status", "permit_type", "permit_si_no", "permit_subtype", "permit_sequence__", "job_start_date", " filing_date", "issuance_date", "expiration_date", " permittee_s_business_name").Where(qry);

                                                                var results = dataset.Query(soql);
                                                                if (results != null && results.Count() > 0)
                                                                {
                                                                    string jsonstring = JsonConvert.SerializeObject(results);
                                                                    List<JobDocumentPermitDTO> records = JsonConvert.DeserializeObject<List<JobDocumentPermitDTO>>(jsonstring);
                                                                    if (records[0].expiration_date != null)
                                                                    {
                                                                        jobDocumentPermitDTO.ExpiredDate = Convert.ToDateTime(records[0].expiration_date, provider);
                                                                    }
                                                                    if (records[0].issuance_date != null)
                                                                    {
                                                                        jobDocumentPermitDTO.IssuedDate = Convert.ToDateTime(records[0].issuance_date, provider);
                                                                    }
                                                                    if (records[0].permittee_s_business_name != null && records[0].permittee_s_business_name != "")
                                                                    {
                                                                        jobDocumentPermitDTO.Permitee = records[0].permittee_s_business_name;
                                                                    }
                                                                    if (records[0].permit_si_no != null && records[0].permit_si_no != "")
                                                                    {
                                                                        isnno = records[0].permit_si_no;
                                                                    }
                                                                }
                                                                jobDocumentPermitDTO.DownloadLink = "https://a810-bisweb.nyc.gov/bisweb/PrintPermit/" + jobDocumentPermitDTO.NumberDocType + ".pdf?allisn=000" + isnno;
                                                                //string tmpUrl = "https://a810-bisweb.nyc.gov/bisweb/" + jobDocumentPermitDTO.DetailUrl;
                                                                
                                                                //string pdfDownloadLink = ReadPermitPdfLink(tmpUrl);
                                                                //string ExpiredDate = ReadPermitExpiredDate(tmpUrl);
                                                                //string IssuedDate = ReadPermitIssuedDate(tmpUrl);
                                                                //string Permitee = ReadPermitPermitee(tmpUrl);


                                                                //if (!string.IsNullOrEmpty(pdfDownloadLink))
                                                                //{
                                                                //    string pdfPAth = $"https://a810-bisweb.nyc.gov/bisweb/{pdfDownloadLink}";

                                                                //    jobDocumentPermitDTO.DownloadLink = pdfPAth;
                                                                //}
                                                                //else
                                                                //{
                                                                //    jobDocumentPermitDTO.DownloadLink = null;
                                                                //}
                                                                int appnumber = Convert.ToInt32(jobApplicationNumber);
                                                                string code = desc[1];

                                                                int id = rpoContext.JobApplications.Where(x => x.ApplicationNumber == jobApplicationNumber && x.IdJob == idJob).Select(x => x.Id).FirstOrDefault();
                                                                jobDocumentPermitDTO.JobApplicationWorkPermitTypeId = rpoContext.JobApplicationWorkPermitTypes.Include("JobApplication.JobApplicationType").Where(x => x.IdJobApplication == id && x.Code == code).Select(x => x.Id).FirstOrDefault();
                                                                JobApplicationWorkPermitType jobApplicationWorkPermitType = rpoContext.JobApplicationWorkPermitTypes.Include("JobApplication.JobApplicationType").Where(x => x.IdJobApplication == id && x.Code == code).FirstOrDefault();
                                                                if (jobApplicationWorkPermitType != null)
                                                                {
                                                                    jobDocumentPermitDTO.JobApplicationNumber = jobApplicationWorkPermitType.JobApplication.ApplicationNumber;
                                                                    jobDocumentPermitDTO.ApplicationType = jobApplicationWorkPermitType.JobApplication.JobApplicationType.Description;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                jobDocumentPermitDTO.DownloadLink = null;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (descendants.FirstOrDefault(n => n.HasClass("centercontent")) != null)
                                                            {
                                                                var block1 = descendants.FirstOrDefault(n => n.HasClass("centercontent"));
                                                                jobDocumentPermitDTO.NumberDocType = block1.InnerText;
                                                            }

                                                        }
                                                    }
                                                    break;
                                                case 2:
                                                    jobDocumentPermitDTO.History = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                                    if (jobDocumentPermitDTO.History != "")
                                                    {
                                                        jobDocumentPermitDTO.DownloadLink = jobDocumentPermitDTO.DownloadLink;
                                                    }
                                                    else
                                                    { jobDocumentPermitDTO.DownloadLink = null; }
                                                    break;
                                                case 3:
                                                    jobDocumentPermitDTO.SeqNo = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                                    break;
                                                case 4:
                                                    jobDocumentPermitDTO.FirstIssueDate = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                                    break;
                                                case 5:
                                                    jobDocumentPermitDTO.LastIssueDate = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                                    break;
                                                case 6:
                                                    jobDocumentPermitDTO.Status = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                                    break;
                                                case 7:
                                                    jobDocumentPermitDTO.Applicant = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                                    break;
                                            }
                                            columnIndex = columnIndex + 1;
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(jobDocumentPermitDTO.NumberDocType) || !string.IsNullOrEmpty(jobDocumentPermitDTO.History) || !string.IsNullOrEmpty(jobDocumentPermitDTO.SeqNo)
                                                     || !string.IsNullOrEmpty(jobDocumentPermitDTO.FirstIssueDate) || !string.IsNullOrEmpty(jobDocumentPermitDTO.LastIssueDate) || !string.IsNullOrEmpty(jobDocumentPermitDTO.Status)
                                                     || !string.IsNullOrEmpty(jobDocumentPermitDTO.Applicant))
                                    {
                                        jobDocumentPermitDTO.DetailUrl = !string.IsNullOrEmpty(jobDocumentPermitDTO.DetailUrl) ? "https://a810-bisweb.nyc.gov/bisweb/" + jobDocumentPermitDTO.DetailUrl : string.Empty;
                                        jobDocumentPermitDTO.DocumentDescription = desc[1];
                                        jobDocumentPermitDTOList.Add(jobDocumentPermitDTO);
                                    }
                                }
                                i = i + 1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DOBErrorLog("URL: " + urlAddress);
            }
            return jobDocumentPermitDTOList;
        }

        public void DOBErrorLog(string message)
        {
            try
            {
                string strLogText = "";

                string innerExceptionmessage = string.Empty;
                //string message = string.Empty;
                //if (ex.InnerException != null)
                //{
                //    innerExceptionmessage = ex.InnerException.ToString();
                //}
                //else if (!string.IsNullOrEmpty(ex.Message))
                //{
                //    message = ex.Message;
                //}

                strLogText += Environment.NewLine + "------------------------------------------------------------";
                strLogText += Environment.NewLine + "ErrorMessage ---\n{0}" + message;
                strLogText += Environment.NewLine + "------------------------------------------------------------";



                var timeUtc = DateTime.Now;

                string errorLogFilename = "DOBPullpermitErrorLog_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

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

                var attachments = new List<string>();
                if (File.Exists(path))
                {
                    attachments.Add(path);
                }

                var to = new List<KeyValuePair<string, string>>();

                to.Add(new KeyValuePair<string, string>("meethalal.teli@credencys.com", "RPO Team"));
                //  to.Add(new KeyValuePair<string, string>("chiriki.dinesh@credencys.com", "Dinesh Chiriki"));

                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/MasterEmailTemplate.htm")))
                {
                    body = reader.ReadToEnd();
                }

                var cc = new List<KeyValuePair<string, string>>();

                string emailBody = body;
                emailBody = emailBody.Replace("##EmailBody##", "Please correct the issue.");


                Mail.Send(
                       new KeyValuePair<string, string>("noreply@rpoinc.com", "RPO Log"),
                       to,
                       cc,
                       "DOB Pull Permit Error Log",
                       emailBody,
                       attachments
                   );


            }
            catch (Exception)
            {
            }
        }


        /// <summary>
        /// Reads the permit PDF link.
        /// </summary>
        /// <param name="urlAddress">The URL address.</param>
        /// <returns>System.String.</returns>
        private string ReadPermitPdfLink(string urlAddress)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12;
            HtmlAgilityPack.HtmlDocument doc = new HtmlWeb().Load(urlAddress);
            var descendants = doc.DocumentNode.Descendants();

            var printPermit = descendants.FirstOrDefault(n => n.Name == "a" && n.Attributes.Where(x => x.Name == "href" && x.Value.Contains("PrintPermit")).Count() > 0);
            //string zip = string.Empty;
            string printPdfUrl = string.Empty;
            if (printPermit != null)
            {
                //zip = printPermit.InnerHtml.Split(new string[] { "&nbsp;" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                //zip = !string.IsNullOrEmpty(zip) ? Regex.Replace(zip, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                printPdfUrl = printPermit.Attributes.FirstOrDefault(x => x.Name == "href" && x.Value.Contains("PrintPermit")).Value;
            }

            return printPdfUrl;
        }
        private string ReadPermitIssuedDate(string urlAddress)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12;
            HtmlAgilityPack.HtmlDocument doc = new HtmlWeb().Load(urlAddress);
            var descendants = doc.DocumentNode.Descendants();
            var Issued = descendants.FirstOrDefault(n => n.Name == "td" && n.Attributes.Where(x => x.Name == "class" && x.Value.Contains("label") && n.InnerText == "Issued:").Count() > 0);
            var Business = descendants.FirstOrDefault(n => n.Name == "a" && n.Attributes.Where(x => x.Name == "href" && x.Value.Contains("LookupGeneralContractor")).Count() > 0);


            var issueddate = Issued != null ? Convert.ToString(Issued.NextSibling.NextSibling.InnerText) : string.Empty;
            //string zip = string.Empty;
            string IssueDate = string.Empty;
            if (issueddate != null && !string.IsNullOrEmpty(issueddate))
            {
                IssueDate = issueddate;
            }
            return issueddate;
        }
        private string ReadPermitPermitee(string urlAddress)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12;
            HtmlAgilityPack.HtmlDocument doc = new HtmlWeb().Load(urlAddress);
            var descendants = doc.DocumentNode.Descendants();
            var Business = descendants.FirstOrDefault(n => n.Name == "td" && n.Attributes.Where(x => x.Name == "class" && x.Value.Contains("content") && n.InnerText.Contains("Business:")).Count() > 0);
            string business1 = Business != null ? Business.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim() : string.Empty;
            business1 = Regex.Replace(business1, "&nbsp;", " ").Replace("\t", "").Replace("Business:", "");
            business1 = Regex.Replace(business1, "<[^>]*>", "");
            //string business2 = Convert.ToString(Business.ParentNode.NextSibling.NextSibling.InnerText.Replace("\r", "")).Trim();
            //business2 = Regex.Replace(business2, "&nbsp;", " ").Replace("\t", "");
            //business2 = Regex.Replace(business2, "<[^>]*>", "");

            //if (business2.Contains("Phone:"))
            //{
            //    int start = business2.IndexOf("Phone:");
            //    business2 = business2.Substring(0, start);
            //}
            string Permitee = string.Empty;

            if (business1 != null && !string.IsNullOrEmpty(business1))
            {
                Permitee = business1.Replace("\n", " ").Trim();
            }
            return Permitee;
        }
        private string ReadPermitExpiredDate(string urlAddress)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12;
            HtmlAgilityPack.HtmlDocument doc = new HtmlWeb().Load(urlAddress);
            var descendants = doc.DocumentNode.Descendants();
            var Expired = descendants.FirstOrDefault(n => n.Name == "td" && n.Attributes.Where(x => x.Name == "class" && x.Value.Contains("label") && n.InnerText == "Expires:").Count() > 0);
            var expireddate = Expired != null ? Expired.NextSibling.NextSibling.InnerText.ToString() : string.Empty;
            //string zip = string.Empty;
            string ExpireDate = string.Empty;
            if (expireddate != null && !string.IsNullOrEmpty(expireddate))
            {
                ExpireDate = expireddate;
            }
            return ExpireDate;
        }

        private string ReadLOCPermitPdfLink(string applicationNumber)
        {
            string urlAddress = $"https://a810-bisweb.nyc.gov/bisweb/JobsQueryByNumberServlet?passjobnumber={applicationNumber}&passdocnumber=";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12;
            HtmlAgilityPack.HtmlDocument doc = new HtmlWeb().Load(urlAddress);
            var descendants = doc.DocumentNode.Descendants();

            var printPermit = descendants.FirstOrDefault(n => n.Name == "a" && n.Attributes.Where(x => x.Name == "href" && x.Value.Contains("PrintLOC")).Count() > 0);
            string printPdfUrl = string.Empty;
            if (printPermit != null)
            {
                printPdfUrl = printPermit.Attributes.FirstOrDefault(x => x.Name == "href" && x.Value.Contains("PrintLOC")).Value;
            }

            return printPdfUrl;
        }

        private CreateUpdatePW517 FormatPW517(JobDocument jobDocument)
        {
            CreateUpdatePW517 createUpdatePW517 = new CreateUpdatePW517();

            ICollection<JobDocumentField> jobDocumentFields = jobDocument.JobDocumentFields;
            createUpdatePW517.Id = jobDocument.Id;
            createUpdatePW517.IdDocument = jobDocument.IdDocument;
            createUpdatePW517.IdJob = jobDocument.IdJob;
            JobDocumentField jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Type");
            if (jobDocumentField != null)
            {
                createUpdatePW517.IdJobDocumentType = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtLast");
            if (jobDocumentField != null)
            {
                createUpdatePW517.Applicant = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Application");
            if (jobDocumentField != null)
            {
                createUpdatePW517.Application = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Work Permits");
            if (jobDocumentField != null)
            {
                createUpdatePW517.idWorkPermit = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Select Dates");
            if (jobDocumentField != null)
            {
                createUpdatePW517.EfilingDates = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "For");
            if (jobDocumentField != null)
            {
                createUpdatePW517.ForDescription = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtDate6");
            if (jobDocumentField != null)
            {
                createUpdatePW517.FridayDates = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtStart6");
            if (jobDocumentField != null)
            {
                createUpdatePW517.FridayStartTime = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtEnd6");
            if (jobDocumentField != null)
            {
                createUpdatePW517.FridayEndTime = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Same as Weekday");
            if (jobDocumentField != null)
            {
                createUpdatePW517.IsSameAsWeekday = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtMain_Last");
            if (jobDocumentField != null)
            {
                createUpdatePW517.MainAHVWorkContact = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtDate2");
            if (jobDocumentField != null)
            {
                createUpdatePW517.MondayDates = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtStart2");
            if (jobDocumentField != null)
            {
                createUpdatePW517.MondayStartTime = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtEnd2");
            if (jobDocumentField != null)
            {
                createUpdatePW517.MondayEndTime = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtNo_Days");
            if (jobDocumentField != null)
            {
                createUpdatePW517.NumberOfDays = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "opg200");
            if (jobDocumentField != null)
            {
                createUpdatePW517.Opg200 = jobDocumentField.Value != null ? Convert.ToBoolean(jobDocumentField.Value) : false;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "opgCrane");
            if (jobDocumentField != null)
            {
                createUpdatePW517.OpgCrane = jobDocumentField.Value != null ? Convert.ToBoolean(jobDocumentField.Value) : false;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "opgDemo");
            if (jobDocumentField != null)
            {
                createUpdatePW517.OpgDemo = jobDocumentField.Value != null ? Convert.ToBoolean(jobDocumentField.Value) : false;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "opgEnclosed");
            if (jobDocumentField != null)
            {
                createUpdatePW517.OpgEnclosed = jobDocumentField.Value != null ? Convert.ToBoolean(jobDocumentField.Value) : false;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtReasonForVariance");
            if (jobDocumentField != null)
            {
                createUpdatePW517.ReasonForVariance = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtDate7");
            if (jobDocumentField != null)
            {
                createUpdatePW517.SaturdayDates = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtStart7");
            if (jobDocumentField != null)
            {
                createUpdatePW517.SaturdayStartTime = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtEnd7");
            if (jobDocumentField != null)
            {
                createUpdatePW517.SaturdayEndTime = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Start Date");
            if (jobDocumentField != null)
            {
                createUpdatePW517.StartDate = jobDocumentField.Value;
            }
            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Next Date");
            if (jobDocumentField != null)
            {
                createUpdatePW517.NextDate = jobDocumentField.Value;
            }
            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtDate1");
            if (jobDocumentField != null)
            {
                createUpdatePW517.SundayDates = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtStart1");
            if (jobDocumentField != null)
            {
                createUpdatePW517.SundayStartTime = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtEnd1");
            if (jobDocumentField != null)
            {
                createUpdatePW517.SundayEndTime = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtDate5");
            if (jobDocumentField != null)
            {
                createUpdatePW517.ThursdayDates = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtStart5");
            if (jobDocumentField != null)
            {
                createUpdatePW517.ThursdayStartTime = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtEnd5");
            if (jobDocumentField != null)
            {
                createUpdatePW517.ThursdayEndTime = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtDate3");
            if (jobDocumentField != null)
            {
                createUpdatePW517.TuesdayDates = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtStart3");
            if (jobDocumentField != null)
            {
                createUpdatePW517.TuesdayStartTime = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtEnd3");
            if (jobDocumentField != null)
            {
                createUpdatePW517.TuesdayEndTime = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtDate4");
            if (jobDocumentField != null)
            {
                createUpdatePW517.WednesdayDates = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtStart4");
            if (jobDocumentField != null)
            {
                createUpdatePW517.WednesdayStartTime = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtEnd4");
            if (jobDocumentField != null)
            {
                createUpdatePW517.WednesdayEndTime = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtWork");
            if (jobDocumentField != null)
            {
                createUpdatePW517.WeekdayDescription = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Weekend Work Description");
            if (jobDocumentField != null)
            {
                createUpdatePW517.WeekendDescription = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Create Support Documents");
            if (jobDocumentField != null)
            {
                createUpdatePW517.CreateSupportDocument = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "AHVReferenceNumber");
            if (jobDocumentField != null)
            {
                createUpdatePW517.AHVReferenceNumber = jobDocumentField.Value;
            }

            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Job Site Contact");
            if (jobDocumentField != null)
            {
                createUpdatePW517.IdJobSiteContact = jobDocumentField.Value;
            }
            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Issued Date");
            if (jobDocumentField != null)
            {
                createUpdatePW517.IssuedDate = jobDocumentField.Value;
            }
            jobDocumentField = jobDocumentFields.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Submitted Date");
            if (jobDocumentField != null)
            {
                createUpdatePW517.SubmittedDate = jobDocumentField.Value;
            }
            string dropboxDocumentUrl = Properties.Settings.Default.DropboxExternalUrl + "/" + jobDocument.IdJob + "/" + Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;
            createUpdatePW517.DocumentPath = jobDocument.DocumentPath != null && jobDocument.DocumentPath != "" ? dropboxDocumentUrl : string.Empty;

            return createUpdatePW517;

        }
        /// <summary>
        /// Get the detail of AHV reports 
        /// </summary>
        /// <param name="jobApplicationNumber"></param>
        /// <param name="AHVReferenceNumber"></param>
        /// <returns> Get the detail of AHV reports </returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocument/VARPMTPermitListFromBIS/{jobApplicationNumber}/{AHVReferenceNumber}")]
        public IHttpActionResult GetVARPMTPermitListFromBIS(string jobApplicationNumber, string AHVReferenceNumber)
        {
            return this.Ok(ReadVARPMTPermitList(jobApplicationNumber, AHVReferenceNumber));
        }

        /// <summary>
        /// Reads the permit list.
        /// </summary>
        /// <param name="jobApplicationNumber">The job application number.</param>
        /// <param name="AHVReferenceNumber">The AHV Reference number.</param>
        /// <returns>List&lt;JobDocumentVARPMTPermit&gt;.</returns>
        private List<JobDocumentVARPMTPermit> ReadVARPMTPermitList(string jobApplicationNumber, string AHVReferenceNumber)
        {
            string urlAddress = $"http://a810-bisweb.nyc.gov/bisweb/AHVPermitsQueryByNumberServlet?requestid=4&allkey={jobApplicationNumber}A&passjobnumber={jobApplicationNumber}";

            List<JobDocumentVARPMTPermit> jobDocumentPermitVARPMTAhvList = new List<JobDocumentVARPMTPermit>();
            List<JobDocumentVARPMTPermit> jobDocumentPermitVARPMTList = new List<JobDocumentVARPMTPermit>();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12;
            HtmlAgilityPack.HtmlDocument doc = new HtmlWeb().Load(urlAddress);
            var descendants = doc.DocumentNode.Descendants();



            if (doc.ParsedText.ToLower().Contains("You don't have permission to access".ToLower().Trim()))
            {
                JobDocumentVARPMTPermit jobDocumentVARPMTPermit = new JobDocumentVARPMTPermit();
                jobDocumentVARPMTPermit.isError = true;
                jobDocumentPermitVARPMTList.Add(jobDocumentVARPMTPermit);

            }
            else
            {

                //string errorLogFilename = "PullPermitLog_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

                //string directory = AppDomain.CurrentDomain.BaseDirectory + "Log";
                //if (!Directory.Exists(directory))
                //{
                //    Directory.CreateDirectory(directory);
                //}

                //string path = AppDomain.CurrentDomain.BaseDirectory + "Log/" + errorLogFilename;

                //if (File.Exists(path))
                //{
                //    using (StreamWriter stwriter = new StreamWriter(path, true))
                //    {
                //        stwriter.WriteLine("-------------------Error Log Start-----------as on " + DateTime.Now.ToString("hh:mm tt"));
                //        stwriter.WriteLine("descendants :" + descendants.ToList());
                //        stwriter.WriteLine("doc :" + doc.ParsedText);
                //        stwriter.WriteLine("-------------------End----------------------------");
                //        stwriter.Close();
                //    }
                //}
                //else
                //{
                //    StreamWriter stwriter = File.CreateText(path);
                //    stwriter.WriteLine("-------------------Error Log Start-----------as on " + DateTime.Now.ToString("hh:mm tt"));
                //    stwriter.WriteLine("descendants :" + descendants.ToList());
                //    stwriter.WriteLine("doc :" + doc.ParsedText);
                //    stwriter.WriteLine("-------------------End----------------------------");
                //    stwriter.Close();
                //}

                var block = descendants.FirstOrDefault(n => n.HasClass("centercolhdg") && n.InnerHtml.Contains("Reference <br>Number"));
                var blockParent = block.ParentNode.ParentNode;
                int i = 0;

                foreach (var item in blockParent.ChildNodes)
                {
                    if (AHVReferenceNumber.Trim() != "" && AHVReferenceNumber.Trim() != "null")
                    {
                        JobDocumentVARPMTPermit jobDocumentVARPMTPermit = new JobDocumentVARPMTPermit();
                        if (item.NodeType == HtmlNodeType.Element && i > 2 && item.InnerHtml.Contains(AHVReferenceNumber))
                        {
                            int columnIndex = 1;
                            jobDocumentVARPMTPermit.isError = false;
                            // JobDocumentVARPMTPermit jobDocumentVARPMTPermit = new JobDocumentVARPMTPermit();
                            foreach (var childItem in item.ChildNodes)
                            {
                                if (childItem.NodeType == HtmlNodeType.Element)
                                {
                                    switch (columnIndex)
                                    {

                                        case 1:
                                            if (childItem.InnerText != null && childItem.InnerText.Trim() == AHVReferenceNumber.Trim())
                                            {
                                                jobDocumentVARPMTPermit.ReferenceNumber = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                            }
                                            if (!string.IsNullOrEmpty(jobDocumentVARPMTPermit.ReferenceNumber) && childItem != null && childItem.FirstChild != null && childItem.FirstChild.Attributes != null)
                                            {
                                                jobDocumentVARPMTPermit.DetailUrl = childItem.FirstChild.Attributes.FirstOrDefault(x => x.Name == "href" && x.Value.Contains("AHVPermitDetailsServlet")).Value;
                                                jobDocumentVARPMTPermit.DetailUrl = !string.IsNullOrEmpty(jobDocumentVARPMTPermit.DetailUrl) ? "http://a810-bisweb.nyc.gov/bisweb/" + jobDocumentVARPMTPermit.DetailUrl : string.Empty;

                                                if (jobDocumentVARPMTPermit.DetailUrl != null)
                                                {
                                                    jobDocumentVARPMTPermit.PDFLink = ReadVARPMTPDFURL(jobDocumentVARPMTPermit.DetailUrl);
                                                }
                                            }
                                            break;
                                        case 2:
                                            jobDocumentVARPMTPermit.EntryDate = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                            break;
                                        case 3:
                                            jobDocumentVARPMTPermit.Status = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                            break;
                                        case 4:
                                            jobDocumentVARPMTPermit.StartDate = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                            break;
                                        case 5:
                                            jobDocumentVARPMTPermit.EndDate = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                            break;
                                        case 6:
                                            jobDocumentVARPMTPermit.PermissibleDaysforeRenewal = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                            break;
                                        case 7:
                                            jobDocumentVARPMTPermit.Applicant = !string.IsNullOrEmpty(childItem.InnerText) ? Regex.Replace(childItem.InnerText, @"<[^>]+>|&nbsp;", "").Replace("GC - 002305//-->", "").Trim() : string.Empty;
                                            break;
                                        case 8:
                                            jobDocumentVARPMTPermit.Type = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                            break;
                                    }
                                    columnIndex = columnIndex + 1;
                                }
                            }

                            if (!string.IsNullOrEmpty(jobDocumentVARPMTPermit.ReferenceNumber) || !string.IsNullOrEmpty(jobDocumentVARPMTPermit.EntryDate) || !string.IsNullOrEmpty(jobDocumentVARPMTPermit.Status)
                                             || !string.IsNullOrEmpty(jobDocumentVARPMTPermit.StartDate) || !string.IsNullOrEmpty(jobDocumentVARPMTPermit.EndDate) || !string.IsNullOrEmpty(jobDocumentVARPMTPermit.PermissibleDaysforeRenewal)
                                             || !string.IsNullOrEmpty(jobDocumentVARPMTPermit.Applicant) || !string.IsNullOrEmpty(jobDocumentVARPMTPermit.Type))
                            {
                                // jobDocumentVARPMTPermit.DetailUrl = !string.IsNullOrEmpty(jobDocumentVARPMTPermit.DetailUrl) ? "http://a810-bisweb.nyc.gov/bisweb/" + jobDocumentVARPMTPermit.DetailUrl : string.Empty;

                                jobDocumentPermitVARPMTAhvList.Add(jobDocumentVARPMTPermit);
                            }

                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(jobDocumentVARPMTPermit.ReferenceNumber) || !string.IsNullOrEmpty(jobDocumentVARPMTPermit.EntryDate) || !string.IsNullOrEmpty(jobDocumentVARPMTPermit.Status)
                                            || !string.IsNullOrEmpty(jobDocumentVARPMTPermit.StartDate) || !string.IsNullOrEmpty(jobDocumentVARPMTPermit.EndDate) || !string.IsNullOrEmpty(jobDocumentVARPMTPermit.PermissibleDaysforeRenewal)
                                            || !string.IsNullOrEmpty(jobDocumentVARPMTPermit.Applicant) || !string.IsNullOrEmpty(jobDocumentVARPMTPermit.Type))
                            {
                                // jobDocumentVARPMTPermit.DetailUrl = !string.IsNullOrEmpty(jobDocumentVARPMTPermit.DetailUrl) ? "http://a810-bisweb.nyc.gov/bisweb/" + jobDocumentVARPMTPermit.DetailUrl : string.Empty;

                                jobDocumentPermitVARPMTAhvList.Add(jobDocumentVARPMTPermit);
                            }
                        }
                    }

                    else
                    {
                        if (item.NodeType == HtmlNodeType.Element && i > 2)
                        {
                            int columnIndex = 1;
                            JobDocumentVARPMTPermit jobDocumentVARPMTPermit = new JobDocumentVARPMTPermit();
                            foreach (var childItem in item.ChildNodes)
                            {
                                if (childItem.NodeType == HtmlNodeType.Element)
                                {
                                    switch (columnIndex)
                                    {
                                        case 1:
                                            jobDocumentVARPMTPermit.ReferenceNumber = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                            if (!string.IsNullOrEmpty(jobDocumentVARPMTPermit.ReferenceNumber) && childItem != null && childItem.FirstChild != null && childItem.FirstChild.Attributes != null)
                                            {
                                                jobDocumentVARPMTPermit.DetailUrl = childItem.FirstChild.Attributes.FirstOrDefault(x => x.Name == "href" && x.Value.Contains("AHVPermitDetailsServlet")).Value;
                                                jobDocumentVARPMTPermit.DetailUrl = !string.IsNullOrEmpty(jobDocumentVARPMTPermit.DetailUrl) ? "http://a810-bisweb.nyc.gov/bisweb/" + jobDocumentVARPMTPermit.DetailUrl : string.Empty;

                                                if (jobDocumentVARPMTPermit.DetailUrl != null)
                                                {
                                                    jobDocumentVARPMTPermit.PDFLink = ReadVARPMTPDFURL(jobDocumentVARPMTPermit.DetailUrl);
                                                }
                                            }
                                            break;
                                        case 2:
                                            jobDocumentVARPMTPermit.EntryDate = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                            break;
                                        case 3:
                                            jobDocumentVARPMTPermit.Status = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                            break;
                                        case 4:
                                            jobDocumentVARPMTPermit.StartDate = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                            break;
                                        case 5:
                                            jobDocumentVARPMTPermit.EndDate = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                            break;
                                        case 6:
                                            jobDocumentVARPMTPermit.PermissibleDaysforeRenewal = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                            break;
                                        case 7:
                                            jobDocumentVARPMTPermit.Applicant = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Replace("GC - 002305//-->", "").Trim() : string.Empty;
                                            break;
                                        case 8:
                                            jobDocumentVARPMTPermit.Type = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                            break;
                                    }
                                    columnIndex = columnIndex + 1;
                                }
                            }

                            if (!string.IsNullOrEmpty(jobDocumentVARPMTPermit.ReferenceNumber) || !string.IsNullOrEmpty(jobDocumentVARPMTPermit.EntryDate) || !string.IsNullOrEmpty(jobDocumentVARPMTPermit.Status)
                                             || !string.IsNullOrEmpty(jobDocumentVARPMTPermit.StartDate) || !string.IsNullOrEmpty(jobDocumentVARPMTPermit.EndDate) || !string.IsNullOrEmpty(jobDocumentVARPMTPermit.PermissibleDaysforeRenewal)
                                             || !string.IsNullOrEmpty(jobDocumentVARPMTPermit.Applicant) || !string.IsNullOrEmpty(jobDocumentVARPMTPermit.Type))
                            {
                                //jobDocumentVARPMTPermit.DetailUrl = !string.IsNullOrEmpty(jobDocumentVARPMTPermit.DetailUrl) ? "http://a810-bisweb.nyc.gov/bisweb/" + jobDocumentVARPMTPermit.DetailUrl : string.Empty;

                                //if (jobDocumentVARPMTPermit.DetailUrl != null)
                                //{
                                //    jobDocumentVARPMTPermit.PDFLink = ReadVARPMTPDFURL(jobDocumentVARPMTPermit.DetailUrl);
                                //}

                                jobDocumentPermitVARPMTList.Add(jobDocumentVARPMTPermit);
                            }

                        }
                    }
                    i = i + 1;
                }

                if (jobDocumentPermitVARPMTAhvList.Count > 0)
                {
                    return jobDocumentPermitVARPMTAhvList;
                }
            }


            return jobDocumentPermitVARPMTList;
        }


        private string ReadVARPMTPDFURL(string DetailURL)
        {
            string urlAddress = string.Empty;

            // urlAddress = $"http://a810-bisweb.nyc.gov/bisweb/JobsQueryByNumberServlet?passjobnumber={jobApplicationNumber}&passdocnumber=&go10=+GO+&requestid=0";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12;

            HtmlAgilityPack.HtmlDocument doc = new HtmlWeb().Load(DetailURL);
            var descendants = doc.DocumentNode.Descendants();
            var printPermit = descendants.FirstOrDefault(n => n.Name == "a" && n.Attributes.Where(x => x.Name == "href" && x.Value.Contains("PrintAHVPermit")).Count() > 0);
            string printPdfUrl = string.Empty;
            if (printPermit != null)
            {
                printPdfUrl = printPermit.Attributes.FirstOrDefault(x => x.Name == "href" && x.Value.Contains("PrintAHVPermit")).Value;
            }
            string pdfPAth = string.Empty;
            if (!string.IsNullOrEmpty(printPdfUrl))
            {
                pdfPAth = $"http://a810-bisweb.nyc.gov/bisweb/{printPdfUrl}";

            }
            return pdfPAth;
        }


        /// <summary>
        /// Posts the VARPMT permit list from bis.
        /// </summary>
        /// <param name="pullPermitVARPMT">The pull permit dto.</param>
        /// <returns>Posts the VARPMT permit list from bis.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/jobdocument/PullPermitVARPMT")]
        public IHttpActionResult PostVARPMTPermitListFromBIS(PullPermitVARPMT pullPermitVARPMT)
        {
            string pdfDownloadLink = ReadVARPMTPermitPdfLink(pullPermitVARPMT.DetailUrl);
            PullPermitResultVARPMT pullPermitResultVARPMT = new PullPermitResultVARPMT();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12;
            if (!string.IsNullOrEmpty(pdfDownloadLink))
            {
                JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == pullPermitVARPMT.IdJobDocument);
                if (jobDocument != null)
                {
                    string thisFileName = pullPermitVARPMT.ReferenceNumber + ".pdf";
                    JobDocumentAttachment jobDocumentAttachment = null;
                    jobDocumentAttachment = rpoContext.JobDocumentAttachments.FirstOrDefault(x => x.IdJobDocument == pullPermitVARPMT.IdJobDocument);
                    if (jobDocumentAttachment == null)
                    {
                        jobDocumentAttachment = new JobDocumentAttachment();
                        jobDocumentAttachment.DocumentName = thisFileName;
                        jobDocumentAttachment.Path = thisFileName;
                        jobDocumentAttachment.IdJobDocument = pullPermitVARPMT.IdJobDocument;
                        rpoContext.JobDocumentAttachments.Add(jobDocumentAttachment);

                        jobDocument.DocumentPath = thisFileName;
                        rpoContext.SaveChanges();

                        int document_Attachment = DocumentPlaceHolderField.VARPMTDocument_Attachment.GetHashCode();
                        JobDocumentField jobDocumentField = rpoContext.JobDocumentFields.FirstOrDefault(x => x.IdJobDocument == jobDocument.Id && x.DocumentField.IdField == document_Attachment);
                        if (jobDocumentField != null)
                        {
                            jobDocumentField.Value = thisFileName;
                            jobDocumentField.ActualValue = thisFileName;
                        }

                        rpoContext.SaveChanges();
                    }
                    else
                    {
                        jobDocumentAttachment.DocumentName = thisFileName;
                        jobDocumentAttachment.Path = thisFileName;
                        jobDocument.DocumentPath = thisFileName;
                        rpoContext.SaveChanges();
                    }

                    string path = HttpRuntime.AppDomainAppPath;
                    string directoryName = Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentAttachmentPath));

                    string directoryFileName = Convert.ToString(jobDocumentAttachment.Id) + "_" + thisFileName;
                    string filename = Path.Combine(directoryName, directoryFileName);

                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }

                    //using (Stream file = File.OpenWrite(filename))
                    //{
                    //    input.CopyTo(file);
                    //    file.Close();
                    //}


                    string jobDocumentDirectory = Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentExportPath));
                    jobDocumentDirectory = Path.Combine(jobDocumentDirectory, Convert.ToString(jobDocument.IdJob));

                    string jobDocumentFile = Convert.ToString(jobDocument.Id) + "_" + thisFileName;

                    jobDocumentFile = Path.Combine(jobDocumentDirectory, jobDocumentFile);

                    if (!Directory.Exists(jobDocumentDirectory))
                    {
                        Directory.CreateDirectory(jobDocumentDirectory);
                    }

                    if (File.Exists(jobDocumentFile))
                    {
                        File.Delete(jobDocumentFile);
                    }

                    //if (File.Exists(filename))
                    //{
                    //    File.Copy(filename, jobDocumentFile);
                    //}

                    string pdfPAth = $"http://a810-bisweb.nyc.gov/bisweb/{pdfDownloadLink}";
                    WebClient client = new WebClient();
                    client.Headers.Add("User-Agent: Other");
                    client.DownloadFile(pdfPAth, filename);

                    WebClient client_2 = new WebClient();
                    client_2.Headers.Add("User-Agent: Other");
                    client_2.DownloadFile(pdfPAth, jobDocumentFile);

                    //Common.DownloadFile(pdfPAth, filename);
                    //Common.DownloadFile(pdfPAth, jobDocumentFile);

                    var instance = new DropboxIntegration();
                    string uploadFilePath = Properties.Settings.Default.DropboxFolderPath + Convert.ToString(jobDocument.IdJob);
                    string dropBoxFileName = Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;
                    string filepath = jobDocumentFile;
                    var task = instance.RunUpload(uploadFilePath, dropBoxFileName, filepath);


                    string dropboxDocumentUrl = Properties.Settings.Default.DropboxExternalUrl + "/" + jobDocument.IdJob + "/" + Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;
                    pullPermitResultVARPMT.JobDocumentUrl = dropboxDocumentUrl;
                    pullPermitResultVARPMT.IsPdfExist = true;

                    JobDocumentField jobDocumentField_WorkPermit = rpoContext.JobDocumentFields.FirstOrDefault(x => x.IdJobDocument == jobDocument.Id && x.DocumentField.Field.FieldName == "Variance Permit");

                    UpdatePermitDates(jobDocumentFile, pullPermitVARPMT.ReferenceNumber, Convert.ToInt32(jobDocumentField_WorkPermit.Value));

                }
            }
            else
            {
                pullPermitResultVARPMT.JobDocumentUrl = null;
                pullPermitResultVARPMT.IsPdfExist = false;
            }
            return this.Ok(pullPermitResultVARPMT);

        }

        private string ReadVARPMTPermitPdfLink(string urlAddress)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12;
            HtmlAgilityPack.HtmlDocument doc = new HtmlWeb().Load(urlAddress);
            var descendants = doc.DocumentNode.Descendants();
            var block = descendants.FirstOrDefault(n => n.HasClass("centercontent"));

            var block1 = block.Descendants("a").FirstOrDefault();


            string printPdfUrl = string.Empty;
            if (block1 != null && block1.InnerText.Trim() == "Printable (PDF) version of After Hours Variance Permit")
            {
                printPdfUrl = block1.Attributes.FirstOrDefault(x => x.Name == "href").Value;

                //zip = printPermit.InnerHtml.Split(new string[] { "&nbsp;" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                //zip = !string.IsNullOrEmpty(zip) ? Regex.Replace(zip, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                //  printPdfUrl = printPermit.Attributes.FirstOrDefault(x => x.Name == "href" && x.GetAttributeValue() && x.Value.Contains("Printable(PDF) version of After Hours Variance Permit")).Value;
            }



            return printPdfUrl;
        }
        /// <summary>
        /// Get the detail of job document and 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Get the detail of job document</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/JobDocument/Clone/{id}")]
        public IHttpActionResult JobDocumentClone(int id)
        {
            List<JobDocument> jobDocumentlist = rpoContext.JobDocuments.Include("JobDocumentFields").Include("DocumentMaster")
                                        .Where(x => x.Id == id).ToList();
            if (jobDocumentlist == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

            if (jobDocumentlist != null && jobDocumentlist.Count() > 0)
            {
                using (DbContextTransaction transaction = rpoContext.Database.BeginTransaction())
                {
                    try
                    {
                        JobDocument jobDocuments = rpoContext.JobDocuments.Include("JobDocumentFields")
                                           .FirstOrDefault(x => x.Id == id);

                        JobDocument jobDocument;
                        jobDocument = new JobDocument
                        {
                            IdJob = jobDocuments.IdJob,
                            IdDocument = jobDocuments.IdDocument,
                            DocumentName = jobDocuments.DocumentName,
                            DocumentDescription = jobDocuments.DocumentDescription,
                            IsArchived = jobDocuments.IsArchived,
                            IdJobApplication = jobDocuments.IdJobApplication,
                            JobDocumentFor = jobDocuments.JobDocumentFor,
                            IdJobViolation = jobDocuments.IdJobViolation,
                            DocumentPath = jobDocuments.DocumentPath,
                            IdJobApplicationWorkPermitType = jobDocuments.IdJobApplicationWorkPermitType,
                        };
                        jobDocument.CreatedDate = DateTime.UtcNow;
                        jobDocument.LastModifiedDate = DateTime.UtcNow;
                        if (employee != null)
                        {
                            jobDocument.CreatedBy = employee.Id;
                            jobDocument.LastModifiedBy = employee.Id;
                        }

                        rpoContext.JobDocuments.Add(jobDocument);
                        rpoContext.SaveChanges();

                        List<JobDocumentField> jobDcoumentFields = jobDocuments.JobDocumentFields.Where(d => d.IdJobDocument == jobDocuments.Id).ToList();
                        List<JobDocumentAttachment> JobDocumentAttachments = jobDocuments.JobDocumentAttachments.Where(d => d.IdJobDocument == jobDocuments.Id).ToList();

                        foreach (var item in jobDcoumentFields)
                        {
                            JobDocumentField objdcfileds = new JobDocumentField();
                            objdcfileds.IdJobDocument = jobDocument.Id;
                            objdcfileds.IdDocumentField = item.IdDocumentField;
                            objdcfileds.Value = item.Value;
                            objdcfileds.ActualValue = item.ActualValue;
                            rpoContext.JobDocumentFields.Add(objdcfileds);
                            rpoContext.SaveChanges();
                        }
                        if (JobDocumentAttachments == null || JobDocumentAttachments.Count() <= 0)
                        {
                            string documentNamedoc = jobDocumentlist.Select(x => x.DocumentMaster.DocumentName).FirstOrDefault();

                            string newdropBoxFileNamedoc = jobDocuments.IdJob.ToString() + "/" + Convert.ToString(jobDocument.Id) + "_" + documentNamedoc + ".pdf";

                            string olddropBoxFileNamedoc = jobDocuments.IdJob.ToString() + "/" + Convert.ToString(jobDocuments.Id) + "_" + documentNamedoc + ".pdf";

                            var instancedoc = new DropboxIntegration();

                            string frompathdoc = Properties.Settings.Default.DropboxFolderPath + olddropBoxFileNamedoc;
                            string Topathdoc = Properties.Settings.Default.DropboxFolderPath + newdropBoxFileNamedoc;

                            var task = instancedoc.RunCopy(frompathdoc, Topathdoc);
                        }

                        foreach (var itemattachment in JobDocumentAttachments)
                        {
                            JobDocumentAttachment jobDocumentAttachment = new JobDocumentAttachment();
                            jobDocumentAttachment.IdJobDocument = jobDocument.Id;
                            jobDocumentAttachment.DocumentName = itemattachment.DocumentName;
                            jobDocumentAttachment.Path = itemattachment.Path;
                            rpoContext.JobDocumentAttachments.Add(jobDocumentAttachment);
                            rpoContext.SaveChanges();

                            string docpath = string.Empty;
                            if (itemattachment.Path.Contains("%23"))
                            {
                                docpath = Uri.UnescapeDataString(itemattachment.Path);
                            }
                            else
                            {
                                docpath = itemattachment.Path;
                            }

                            string newdropBoxFileName = jobDocuments.IdJob.ToString() + "/" + Convert.ToString(jobDocument.Id) + "_" + docpath;

                            string olddropBoxFileName = jobDocuments.IdJob.ToString() + "/" + Convert.ToString(jobDocuments.Id) + "_" + docpath;

                            var instance = new DropboxIntegration();

                            string frompath = Properties.Settings.Default.DropboxFolderPath + olddropBoxFileName;
                            string Topath = Properties.Settings.Default.DropboxFolderPath + newdropBoxFileName;

                            var taskattachmeny = instance.RunCopy(frompath, Topath);
                        }

                        transaction.Commit();

                        return Ok(new { Message = "Clone Document created successfully" });
                        //var response = Request.CreateResponse(HttpStatusCode.OK, "JobDocument clone successfully.");
                        //return response;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        return Ok(new { Message = ex.Message });
                    }
                }
            }
            //var instance = new DropboxIntegration();
            ////string uploadFilePath = Properties.Settings.Default.DropboxFolderPath + Convert.ToString(jobDocument.IdJob);
            ////string dropBoxFileName = Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;
            ////string filepath = newfile;

            //string frompath = Properties.Settings.Default.DropboxFolderPath + "Test/3709_Additional Information Form AI-1.pdf";
            //string Topath = Properties.Settings.Default.DropboxFolderPath + "Test/To_3709_Additional Information Form AI-1.pdf";

            //var task = instance.RunCopy(frompath, Topath);

            return Ok(new { Message = "" });
            //var responseend = Request.CreateResponse(HttpStatusCode.OK, "JobDocument clone successfully.");
            //return responseend;
        }
        [ResponseType(typeof(Job))]
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Jobs/GetDropBox/{idJob}")]
        public IHttpActionResult GetDropBox(int idJob)
        {
            if (idJob == null)
            {
                return this.NotFound();
            }
            string dropboxDocumentUrl = Properties.Settings.Default.DropboxExternalUrl + "/" + idJob;

            dropboxDocumentUrl = Regex.Replace(dropboxDocumentUrl, @"#", "%23", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            JobDocumentCreateOrUpdateDTO jocumentCreateOrUpdateDTO = new JobDocumentCreateOrUpdateDTO();
            jocumentCreateOrUpdateDTO.DocumentPath = dropboxDocumentUrl != "" ? dropboxDocumentUrl : string.Empty;

            return this.Ok(jocumentCreateOrUpdateDTO);
        }

    }
}



