using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
// Author           : Mital Bhatt 
// Created          : 
//
// Last Modified By : Mital Bhatt 
// Last Modified On : 29-12-2022
// ***********************************************************************
// <copyright file="JobChecklist.cs" company="CREDENCYS">
//     Copyright ©  2022
// </copyright>
// <summary>Class Composite Checklist Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Jobchecklist Checklist namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobChecklist
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using HtmlAgilityPack;
    using Rpo.ApiServices.Api.Controllers.Contacts;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using System.Configuration;
    using Microsoft.ApplicationBlocks.Data;
    using System.Data.SqlClient;
    using NPOI.XSSF.UserModel;
    using NPOI.SS.UserModel;
    using System.Web.Configuration;
    using Rpo.ApiServices.Api.Controllers.ChecklistItems;
    using Models;
    using System.Linq.Expressions;
    using Rpo.ApiServices.Api.Controllers.CheckListApplicationDropDown;

    public class CustomerJobChecklistController : ApiController
    {   
        RpoContext db = new RpoContext();
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobChecklistItemDueDateDTO))]
        [Route("api/checklist/PostClientNote")]
        public IHttpActionResult PostClientNote(JobChecklistClientNoteHistoryDTO jobChecklistClientNoteHistoryDTO)
        {

            var customer = this.db.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(customer.Permissions, Enums.Permission.ViewChecklistClientNote))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                ClientNoteCustomer jobCheckListClientNoteHistory = new ClientNoteCustomer();
                jobCheckListClientNoteHistory.IdJobChecklistItemDetail = jobChecklistClientNoteHistoryDTO.IdJobChecklistItemDetail;
                jobCheckListClientNoteHistory.Idcustomer = customer.Id;
                jobCheckListClientNoteHistory.Description = jobChecklistClientNoteHistoryDTO.Description;
                jobCheckListClientNoteHistory.Isinternal = jobChecklistClientNoteHistoryDTO.Isinternal;
                 jobCheckListClientNoteHistory.CreatedDate = DateTime.UtcNow;
                jobCheckListClientNoteHistory.LastModifiedDate = DateTime.UtcNow;

                if (customer != null)
                {
                    //jobCheckListClientNoteHistory.CreatedByCus = customer.Id;
                    //jobCheckListClientNoteHistory.LastModifiedByCus = customer.Id;
                    //jobCheckListClientNoteHistory.CustomerCreatedBy = customer;
                    //jobCheckListClientNoteHistory.CustomerLastModifiedBy = customer;
                }

                db.ClientNoteCustomers.Add(jobCheckListClientNoteHistory);
                db.SaveChanges();
                return Ok("Client Note added successfully");
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/{IdJobChecklistItemDetail}/ChecklistClientNoteHistory")]
        public IHttpActionResult GetChecklistClientNoteHistory(int IdJobChecklistItemDetail)
        {
            var customer = this.db.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);

            var jobCheckListClientNoteHistory = db.ClientNoteCustomers.Where(x => x.IdJobChecklistItemDetail == IdJobChecklistItemDetail).Where(x => x.Idcustomer == customer.Id);

            //if (jobCheckListClientNoteHistory == null)
            //{
            //    return this.NotFound();
            //}

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            if (jobCheckListClientNoteHistory != null)
            { 
                var result = db.ClientNoteCustomers
                  //  .Include("CustomerLastModifiedBy")
                   // .Include("CustomerCreatedBy")
                    .Where(x => x.IdJobChecklistItemDetail == IdJobChecklistItemDetail).Where(x => x.Idcustomer == customer.Id)
                    .AsEnumerable()
                    .Select(JobCheckListClientNoteHistory => new
                    {
                        Id = JobCheckListClientNoteHistory.Id,
                        IdCustomer = JobCheckListClientNoteHistory.Idcustomer,
                        IdJobChecklistItemDetail = JobCheckListClientNoteHistory.IdJobChecklistItemDetail,
                        Description = JobCheckListClientNoteHistory.Description,
                        // CreatedBy = JobCheckListClientNoteHistory.CreatedByCus,
                        // LastModifiedBy = JobCheckListClientNoteHistory.LastModifiedByCus != null ? JobCheckListClientNoteHistory.LastModifiedByCus : JobCheckListClientNoteHistory.CreatedByCus,
                        // CustomerCreatedBy = JobCheckListClientNoteHistory.CustomerCreatedBy != null ? JobCheckListClientNoteHistory.CustomerCreatedBy.FirstName + " " + JobCheckListClientNoteHistory.CustomerCreatedBy.LastName : string.Empty,
                        // CustomerLastModifiedBy = JobCheckListClientNoteHistory.CustomerLastModifiedBy != null ? JobCheckListClientNoteHistory.CustomerLastModifiedBy.FirstName + " " + JobCheckListClientNoteHistory.CustomerLastModifiedBy.LastName : (JobCheckListClientNoteHistory.CustomerCreatedBy != null ? JobCheckListClientNoteHistory.CustomerCreatedBy.FirstName + " " + JobCheckListClientNoteHistory.CustomerCreatedBy.LastName : string.Empty),
                        CreatedDate = JobCheckListClientNoteHistory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobCheckListClientNoteHistory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : JobCheckListClientNoteHistory.CreatedDate,
                        LastModifiedDate = JobCheckListClientNoteHistory.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobCheckListClientNoteHistory.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (JobCheckListClientNoteHistory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobCheckListClientNoteHistory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : JobCheckListClientNoteHistory.CreatedDate)
                    }).OrderByDescending(x => x.Id);

                return Ok(result);
            }
            else
                return Ok();
        }



        /// <summary>
        /// Plumbing Inspection Get Comment.
        /// </summary>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/GetPLClientNoteHistory/{IdJobPlumbingInspection}")]
        public IHttpActionResult GetPLClientNoteHistory(int IdJobPlumbingInspection)
        {

            var getPLCommentHistory = db.ClientNotePlumbingCustomers.Where(x => x.IdJobPlumbingInspection == IdJobPlumbingInspection);
            if (getPLCommentHistory == null)
            {
                return this.NotFound();
            }

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            var result = db.ClientNotePlumbingCustomers
                        .Where(x => x.IdJobPlumbingInspection == IdJobPlumbingInspection)
                .AsEnumerable()
                .Select(pLclientHistory => new
                {
                    IdJobPlumbingInspection = pLclientHistory.IdJobPlumbingInspection,
                    Description = pLclientHistory.Description,
                    CreatedDate = pLclientHistory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc
                                            (Convert.ToDateTime(pLclientHistory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone))
                                            : pLclientHistory.CreatedDate,
                    LastModifiedDate = pLclientHistory.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc
                                            (Convert.ToDateTime(pLclientHistory.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone))
                                            : (pLclientHistory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(pLclientHistory.CreatedDate),
                                            TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : pLclientHistory.CreatedDate),
                                             Idcustomer = pLclientHistory.Idcustomer
        }).OrderByDescending(x => x.LastModifiedDate);

            return Ok(result);
        }


        /// <summary>
        /// Plumbing Inspection Edit Comment.
        /// </summary>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/PostPLInspectionClientNote")]
        public IHttpActionResult PostPLInspectionClientNote(PlumbingInspectionDTO inspectionAddComment)
        {
            var employee = db.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            ClientNotePlumbingCustomer ClientNotePlumbingCustomer = new ClientNotePlumbingCustomer();
            ClientNotePlumbingCustomer.IdJobPlumbingInspection = inspectionAddComment.IdJobPlumbingInspection;
            ClientNotePlumbingCustomer.Description = inspectionAddComment.Description;
            ClientNotePlumbingCustomer.CreatedDate = DateTime.UtcNow;
            ClientNotePlumbingCustomer.LastModifiedDate = DateTime.UtcNow;
            ClientNotePlumbingCustomer.Idcustomer = employee.Id;

            db.ClientNotePlumbingCustomers.Add(ClientNotePlumbingCustomer);

            try { db.SaveChanges(); }
            catch (Exception ex) { throw ex; }
            return Ok("Client Note Added Successfully");
        }
    }
}