// Author           : shanthi karthika
// Created          : 20-10-2022
//
// Last Modified By : shanthi karthika
// Last Modified On : 20-10-2022
// ***********************************************************************
// <copyright file="CheckListApplicationDropDownController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class CheckList Application DropDown Controller.</summary>
// ***********************************************************************

/// <summary>
/// The CheclList Application DropDown namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.CheckListApplicationDropDown
{
    using System.Linq;
    using System.Web.Http;
    using System.Data;
    using System.Data.Entity;
    using Filters;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using System.Data.Entity.Infrastructure;
    using System.Collections.Generic;
    using System.Configuration;
    using Microsoft.ApplicationBlocks.Data;
    using System;
    using System.Data.SqlClient;
    using Rpo.ApiServices.Api.Tools;
    using System.Web.Http.Description;
    using Models;


    /// <summary>
    /// Class CheckList Application DropDown Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class CheckListApplicationDropDownController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the CheckList Application DropDown.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/CheckListApplicationDropDown/CheckListApplicationDropDown/{IdJob}")]
        public IHttpActionResult CheckListApplicationDropDown(int IdJob)
        {
            var result = this.rpoContext.JobApplications.Include("JobApplicationType").Where(x => x.IdJob == IdJob)               
                .AsEnumerable().Select(c => new
                {
                    Id = c.Id,
                    value = GetDropDownValues(c.JobApplicationType.Description.ToString(), c.ApplicationNumber, c.JobApplicationStatus ?? string.Empty)                   
                }).ToList();

            return this.Ok(result);
        }
        private string GetDropDownValues(string applicationType, string applicationNumber, string applicationStatus)
        {
            string dropvalues = string.Empty;
            if (applicationType != null && applicationType != string.Empty && applicationType != "")
                dropvalues = applicationType;
            if (applicationNumber != null && applicationNumber != string.Empty && applicationNumber != "")
                dropvalues += '-' + applicationNumber;
            if (applicationStatus != null && applicationStatus != string.Empty && applicationStatus != "")
                dropvalues += '-' + applicationStatus;
            return (dropvalues);
        }
        /// <summary>
        /// Gets the CheckList Application DropDown.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/CheckListApplicationDropDown/CheckListSwitcherApplicationDropDown/{IdJob}")]
        public IHttpActionResult CheckListSwitcherApplicationDropDown(int IdJob)
        {  
            var result = (from p in rpoContext.JobChecklistHeaders
                          join jobapp in rpoContext.JobApplications on p.IdJob equals jobapp.IdJob
                          join jt in rpoContext.JobApplicationTypes on jobapp.IdJobApplicationType equals jt.Id
                          where p.IdJob == IdJob && p.IdJobApplication == jobapp.Id
                          select new
                          {
                              Id = p.IdJobCheckListHeader,
                              ChecklistName = p.ChecklistName,
                              IdJob = p.IdJob,
                              LastModifiedDate = p.LastModifiedDate,
                          }).ToList();

            return Ok(result);            
        }
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/CheckListApplicationDropDown/CompositeCheckListSwitcherDropDown/{IdJob}")]
        public IHttpActionResult CompositeCheckListSwitcherDropDown(int IdJob)
        {           
            var result = (from co in rpoContext.CompositeChecklists
                          where co.ParentJobId == IdJob
                          select new
                          {
                              Id = co.Id,
                              CompositeChecklistName = co.Name,
                              LastModifiedDate = co.LastModifiedDate,
                          }).Distinct().ToList();        

            return Ok(result);
        }
        /// <summary>
        /// Gets the work permits dropdown.
        /// </summary>
        /// <param name="idJobApplication">The identifier Checklist workpermit.</param>
        /// <returns> Gets the checklist job application work permits dropdown.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/CheckListApplicationDropDown/WorkPermitsDropdown/{idJobApplication}")]
        public IHttpActionResult WorkPermitsDropdown(int idJobApplication)
        {
            var result = rpoContext.JobApplicationWorkPermitTypes.Where(x => x.IdJobApplication == idJobApplication).AsEnumerable().Select(c => new
            {
                Id = c.Id,
                workTypecode = c.Code,
                permittype = c.PermitType,
                permitnumber = c.PermitNumber,
                worktypedescription = c.PermitType != null && c.PermitType != "" ? c.PermitType + "|" + c.PermitNumber : (c.JobWorkType != null ? c.JobWorkType.Description : string.Empty),
                IdJobWorkType = c.IdJobWorkType
            }).ToArray();

            return Ok(result);
        }



        /// <summary>
        /// Gets the checklist dropdown based on worktype.
        /// </summary>
        /// <param name="workpermittype">The identifier.</param>
        /// <returns>get the checlist group names</returns>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/CheckList/GetChecklistGroupdropdownPermitwise")]
        public IHttpActionResult GetChecklistGroupdropdownPermitwise([FromBody] string[] Code)
        {
            var result = this.rpoContext.CheckListGroups.Where(x => x.IsActive == true).Where(x => x.Type != "PL")
                     .Select(c => new
                     {
                         Id = c.Id,
                         GroupName = c.Name,
                         Type = c.Type,
                         order = c.Displayorder
                     })
                     .ToArray();
            foreach (var a in Code)
            {

                if (a == "PL" || a == "SD" || a == "SP" || a == " PL" || a == " SD" || a == " SP")
                {
                    var result1 = this.rpoContext.CheckListGroups.Where(x => x.IsActive == true)
                           .Select(c => new
                           {
                               Id = c.Id,
                               GroupName = c.Name,
                               Type = c.Type,
                               order = c.Displayorder
                           })
                           .ToArray();
                    return Ok(result1);
                }               
            }
            return Ok(result);


        }  

        /// <summary>
        /// Deletes the CheckList.
        /// </summary>
        /// <param name="id">The identifier.</param>
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/DeleteCheckList/{idChecklist}")]
        public IHttpActionResult DeleteCheckList(int idChecklist)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteChecklist))
            {
                if (this.rpoContext.JobChecklistHeaders.Count(b => b.IdJobCheckListHeader == idChecklist) > 0)
                {
                    JobChecklistHeader checklistHeader = this.rpoContext.JobChecklistHeaders.Find(idChecklist);

                    if (this.rpoContext.JobChecklistGroups.Count(b => b.IdJobCheckListHeader == idChecklist) > 0)
                    {
                        var checklistGroupDetail = this.rpoContext.JobChecklistGroups.Where(x => x.IdJobCheckListHeader == idChecklist).ToList();
                        foreach (var detail in checklistGroupDetail)
                        {
                            var checklistemdetail_item = rpoContext.JobChecklistItemDetails.Where(b => b.IdJobChecklistGroup == detail.Id).ToList();
                            var Plumbingchecklistemdetail_item = rpoContext.JobPlumbingInspection.Where(b => b.IdJobChecklistGroup == detail.Id).ToList();
                            if (checklistemdetail_item.Count > 0)
                            {
                                foreach (var ct in checklistemdetail_item)
                                {
                                    if (this.rpoContext.JobChecklistItemDueDate.Count(b => b.IdJobChecklistItemDetail == ct.Id) > 0)
                                    {
                                        var jobduedate = this.rpoContext.JobChecklistItemDueDate.Where(a => a.IdJobChecklistItemDetail == ct.Id).ToList();
                                        foreach (var item in jobduedate)
                                        {
                                            this.rpoContext.JobChecklistItemDueDate.Remove(item);
                                            // rpoContext.SaveChanges();
                                        }
                                    }
                                    if (this.rpoContext.JobCheckListCommentHistories.Count(b => b.IdJobChecklistItemDetail == ct.Id) > 0)
                                    {
                                        var commentHistory = this.rpoContext.JobCheckListCommentHistories.Where(a => a.IdJobChecklistItemDetail == ct.Id).ToList();
                                        foreach (var item1 in commentHistory)
                                        {
                                            this.rpoContext.JobCheckListCommentHistories.Remove(item1);
                                            // rpoContext.SaveChanges();
                                        }
                                    }
                                    if (this.rpoContext.ClientNoteCustomers.Count(b => b.IdJobChecklistItemDetail == ct.Id) > 0)
                                    {
                                        var commentHistory = this.rpoContext.ClientNoteCustomers.Where(a => a.IdJobChecklistItemDetail == ct.Id).ToList();
                                        foreach (var item1 in commentHistory)
                                        {
                                            this.rpoContext.ClientNoteCustomers.Remove(item1);
                                            // rpoContext.SaveChanges();
                                        }
                                    }
                                    if (this.rpoContext.JobCheckListProgressNoteHistories.Count(b => b.IdJobChecklistItemDetail == ct.Id) > 0)
                                    {
                                        var commentHistory = this.rpoContext.JobCheckListProgressNoteHistories.Where(a => a.IdJobChecklistItemDetail == ct.Id).ToList();
                                        foreach (var item1 in commentHistory)
                                        {
                                            this.rpoContext.JobCheckListProgressNoteHistories.Remove(item1);
                                            // rpoContext.SaveChanges();
                                        }
                                    }
                                    if (this.rpoContext.JobDocuments.Count(b => b.IdJobchecklistItemDetails == ct.Id) > 0)
                                    {
                                        var documents = this.rpoContext.JobDocuments.Where(x => x.IdJobchecklistItemDetails == ct.Id).ToList();
                                        foreach (var item1 in documents)
                                        {
                                            JobDocument j = this.rpoContext.JobDocuments.Find(item1.Id);
                                            j.IdJobchecklistItemDetails = null;
                                            // rpoContext.SaveChanges();
                                            // this.rpoContext.JobCheckListProgressNoteHistories.Remove(item1);
                                        }

                                    }
                                    this.rpoContext.JobChecklistItemDetails.Remove(ct);
                                    // rpoContext.SaveChanges();
                                }
                            }
                            else if (Plumbingchecklistemdetail_item.Count > 0)
                            {
                                foreach (var ct in Plumbingchecklistemdetail_item)
                                {
                                    if (this.rpoContext.JobPlumbingInspectionDueDate.Count(b => b.IdJobPlumbingInspection == ct.IdJobPlumbingInspection) > 0)
                                    {
                                        var jobduedate = this.rpoContext.JobPlumbingInspectionDueDate.Where(a => a.IdJobPlumbingInspection == ct.IdJobPlumbingInspection).ToList();
                                        foreach (var item in jobduedate)
                                        {
                                            this.rpoContext.JobPlumbingInspectionDueDate.Remove(item);
                                        }
                                    }
                                    if (this.rpoContext.JobPlumbingInspectionComments.Count(b => b.IdJobPlumbingInspection == ct.IdJobPlumbingInspection) > 0)
                                    {
                                        var commentHistory = this.rpoContext.JobPlumbingInspectionComments.Where(a => a.IdJobPlumbingInspection == ct.IdJobPlumbingInspection).ToList();
                                        foreach (var item1 in commentHistory)
                                        {
                                            this.rpoContext.JobPlumbingInspectionComments.Remove(item1);
                                        }
                                    }
                                    if (this.rpoContext.ClientNotePlumbingCustomers.Count(b => b.IdJobPlumbingInspection == ct.IdJobPlumbingInspection) > 0)
                                    {
                                        var commentHistory = this.rpoContext.ClientNotePlumbingCustomers.Where(a => a.IdJobPlumbingInspection == ct.IdJobPlumbingInspection).ToList();
                                        foreach (var item1 in commentHistory)
                                        {
                                            this.rpoContext.ClientNotePlumbingCustomers.Remove(item1);
                                        }
                                    }
                                    if (this.rpoContext.JobPlumbingInspectionProgressNoteHistory.Count(b => b.IdJobPlumbingInspection == ct.IdJobPlumbingInspection) > 0)
                                    {
                                        var commentHistory = this.rpoContext.JobPlumbingInspectionProgressNoteHistory.Where(a => a.IdJobPlumbingInspection == ct.IdJobPlumbingInspection).ToList();
                                        foreach (var item1 in commentHistory)
                                        {
                                            this.rpoContext.JobPlumbingInspectionProgressNoteHistory.Remove(item1);
                                        }
                                    }
                                    if (this.rpoContext.JobDocuments.Count(b => b.IdJobPlumbingInspections == ct.IdJobPlumbingInspection) > 0)
                                    {
                                        var documents = this.rpoContext.JobDocuments.Where(x => x.IdJobPlumbingInspections == ct.IdJobPlumbingInspection).ToList();
                                        foreach (var item1 in documents)
                                        {
                                            JobDocument j = this.rpoContext.JobDocuments.Find(item1.Id);
                                            j.IdJobPlumbingInspections = null;
                                            // rpoContext.SaveChanges();
                                            // this.rpoContext.JobCheckListProgressNoteHistories.Remove(item1);
                                        }
                                        //  rpoContext.SaveChanges();
                                    }
                                    this.rpoContext.JobPlumbingChecklistFloors.Remove(this.rpoContext.JobPlumbingChecklistFloors.Where(x => x.Id == ct.IdJobPlumbingCheckListFloors).FirstOrDefault());
                                    this.rpoContext.JobPlumbingInspection.Remove(ct);
                                }
                            }
                            this.rpoContext.JobChecklistGroups.Remove(detail);
                        }
                    }

                    if (rpoContext.CompositeChecklistDetails.Any(x => x.IdJobChecklistHeader == idChecklist))
                    {
                        List<string> lstCompositechecklist = new List<string>();
                        var compositedetails = rpoContext.CompositeChecklistDetails.Where(x => x.IdJobChecklistHeader == idChecklist).Select(x => x.IdCompositeChecklist).Distinct().ToList();
                        foreach (var a in compositedetails)
                        {
                            var compositechecklist = rpoContext.CompositeChecklists.Where(x => x.Id == a).FirstOrDefault();
                            string compositechecklistName = compositechecklist.Name;
                            string Jobid = compositechecklist.ParentJobId.ToString(); ;
                            lstCompositechecklist.Add("Job:" + Jobid + " Composite Checklist Name: " + compositechecklistName);
                        }

                        throw new RpoBusinessException("You cannot delete this checklist as it is already present under Composite Checklists" + string.Join(", ", lstCompositechecklist.ToArray()));
                       
                    }
                    else
                        this.rpoContext.JobChecklistHeaders.Remove(this.rpoContext.JobChecklistHeaders.Find(idChecklist));
                }
                try
                {
                    this.rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChecklistExists(idChecklist))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok();
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        private bool ChecklistExists(int id)
        {
            return this.rpoContext.JobChecklistHeaders.Count(e => e.IdJobCheckListHeader == id) > 0;
        }

        private bool ChecklistItemDetailsExists(int id)
        {
            return this.rpoContext.JobChecklistItemDetails.Count(e => e.Id == id) > 0;

        }

        private bool PLInspectionExists(int id)
        {
            return this.rpoContext.JobPlumbingInspection.Count(e => e.IdJobPlumbingInspection == id) > 0;
        }

        /// <summary>
        /// View the CheckList.
        /// </summary>
        /// <param name="id">The identifier.</param>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/ViewCheckList")]
        public IHttpActionResult ViewCheckList(ChecklistSearch checkListSearch)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewChecklist)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteChecklist))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();

                DataSet ds = new DataSet();
                SqlParameter[] spParameter = new SqlParameter[3];

                spParameter[0] = new SqlParameter("@IdJobCheckListHeader", SqlDbType.NVarChar);
                spParameter[0].Direction = ParameterDirection.Input;
                spParameter[0].Value = checkListSearch.ChecklistIds;

                spParameter[1] = new SqlParameter("@OrderFlag", SqlDbType.VarChar);
                spParameter[1].Direction = ParameterDirection.Input;
                spParameter[1].Value = checkListSearch.OrderFlag;

                spParameter[2] = new SqlParameter("@SearchText", SqlDbType.VarChar);
                spParameter[2].Direction = ParameterDirection.Input;
                spParameter[2].Value = !string.IsNullOrEmpty(checkListSearch.SearchText) ? checkListSearch.SearchText : string.Empty;

                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "ChecklistView", spParameter);
                List<CheckListGroupHeader> headerlist = new List<CheckListGroupHeader>();
                string[] headerid = checkListSearch.ChecklistIds.Split(',');


                List<DataTable> distinctheaders = ds.Tables[0].AsEnumerable()
                        .GroupBy(row => new
                        {
                            JobChecklistHeaderId = row.Field<int>("JobChecklistHeaderId"),                           
                        }).Select(g => g.CopyToDataTable()).ToList();                
                int headerrow = 0;
                foreach (var loopheader in distinctheaders)
                {
                    CheckListGroupHeader header = new CheckListGroupHeader();
                    Int32 checklistHeaderId = Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                    header.jobChecklistHeaderId = loopheader.Rows[0]["JobChecklistHeaderId"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                    header.Others = loopheader.Rows[0]["Others"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["Others"].ToString();
                    header.applicationName = loopheader.Rows[0]["checklistNamewithFillingstatus"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["checklistNamewithFillingstatus"].ToString();
                    header.ChecklistCreatedDate = loopheader.Rows[0]["ChecklistCreatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(loopheader.Rows[0]["ChecklistCreatedDate"]);
                    header.ChecklistLastmodifiedDate = loopheader.Rows[0]["ChecklistLastmodifiedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(loopheader.Rows[0]["ChecklistLastmodifiedDate"]);
                    header.groups = new List<ChecklistGroup>();
                    header.IdJob= loopheader.Rows[0]["IdJob"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["IdJob"]);                  
                    string s=null;
                    int id = checklistHeaderId;                                         
                        var temp = rpoContext.JobChecklistHeaders.Include("JobApplicationWorkPermitTypes").Where(x => x.IdJobCheckListHeader == id).ToList();
                    var lstidjobworktype=temp[0].JobApplicationWorkPermitTypes.Where(x => x.Code.ToLower().Trim() == "pl" || x.Code.ToLower().Trim() == "sp" || x.Code.ToLower().Trim() == "sd").Select(y => y.IdJobWorkType).ToList();
                  foreach(var i in lstidjobworktype)
                    {
                        s += i.ToString() + ",";
                    }
                    if (s != null)
                        header.IdWorkPermits = s.Remove(s.Length - 1, 1);
                 
                    var distinctGroup = ds.Tables[0].AsEnumerable().Where(i => i.Field<int>("JobChecklistHeaderId") == checklistHeaderId)
                        .GroupBy(r => new { group = r.Field<int>("IdJobChecklistGroup") }).Select(g => g.CopyToDataTable()).ToList();                  

                    foreach (var eachGroup in distinctGroup)
                    {

                        ChecklistGroup group = new ChecklistGroup();
                        group.item = new List<Item>();
                        Int32 IdJobChecklistGroup = Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                        group.checkListGroupName = eachGroup.Rows[0]["ChecklistGroupName"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["ChecklistGroupName"].ToString();
                        group.jobChecklistGroupId = eachGroup.Rows[0]["IdJobChecklistGroup"] == DBNull.Value ? (int?)null : Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                        group.checkListGroupType = eachGroup.Rows[0]["checklistGroupType"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["checklistGroupType"].ToString();
                        group.DisplayOrder1 = eachGroup.Rows[0]["DisplayOrder1"] == DBNull.Value ? (int?)null : Convert.ToInt32(eachGroup.Rows[0]["DisplayOrder1"]);


                        if (eachGroup.Rows[0]["checklistGroupType"].ToString().ToLower().TrimEnd() != "pl")
                        {


                                var groupsitems = ds.Tables[0].AsEnumerable().Where(l => l.Field<int>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();
                               
                                for (int j = 0; j < groupsitems.Count; j++)
                                {     
                                    Item item = new Item();
                                    item.Details = new List<Details>();

                                #region Items
                                Int32 IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
                                if (IdChecklistItem != 0)
                                {
                                    item.checklistItemName = groupsitems[j]["checklistItemName"] == DBNull.Value ? string.Empty : groupsitems[j]["checklistItemName"].ToString();
                                    item.IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
                                    item.jocbChecklistItemDetailsId = groupsitems[j]["JocbChecklistItemDetailsId"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["JocbChecklistItemDetailsId"]);
                                    int itemid = Convert.ToInt32(item.jocbChecklistItemDetailsId);
                                    item.HasDocument = rpoContext.JobDocuments.Any(x => x.IdJobchecklistItemDetails == itemid);
                                    item.comments = groupsitems[j]["Comments"] == DBNull.Value ? string.Empty : groupsitems[j]["Comments"].ToString();
                                    item.idDesignApplicant = groupsitems[j]["IdDesignApplicant"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdDesignApplicant"]);
                                    item.idInspector = groupsitems[j]["IdInspector"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdInspector"]);
                                    item.stage = groupsitems[j]["Stage"] == DBNull.Value ? string.Empty : groupsitems[j]["Stage"].ToString();
                                    item.partyResponsible1 = groupsitems[j]["PartyResponsible1"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["PartyResponsible1"]);
                                    item.manualPartyResponsible = groupsitems[j]["ManualPartyResponsible"] == DBNull.Value ? string.Empty : groupsitems[j]["ManualPartyResponsible"].ToString();
                                    item.idContact = groupsitems[j]["IdContact"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdContact"]);
                                    item.referenceDocumentId = groupsitems[j]["IdReferenceDocument"] == DBNull.Value ? string.Empty : groupsitems[j]["IdReferenceDocument"].ToString();
                                    item.dueDate = groupsitems[j]["DueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(groupsitems[j]["DueDate"]);
                                    item.status = groupsitems[j]["Status"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["Status"]);
                                    item.checkListLastModifiedDate = groupsitems[j]["checklistItemLastDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(groupsitems[j]["checklistItemLastDate"]);
                                    item.idCreateFormDocument = groupsitems[j]["IdCreateFormDocument"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdCreateFormDocument"]);
                                    item.idUploadFormDocument = groupsitems[j]["IdUploadFormDocument"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdUploadFormDocument"]);
                                    item.Displayorder = groupsitems[j]["Displayorder"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["Displayorder"]);
                                    item.ChecklistDetailDisplayOrder = groupsitems[j]["ChecklistDetailDisplayOrder"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["ChecklistDetailDisplayOrder"]);
                                    item.PartyResponsible = groupsitems[j]["PartyResponsible"] == DBNull.Value ? string.Empty : groupsitems[j]["PartyResponsible"].ToString();
                                    if (item.idContact != null && item.idContact != 0)
                                    {
                                        var c = rpoContext.Contacts.Find(item.idContact);
                                        item.ContactName = c.FirstName + c.LastName;
                                    }
                                    #endregion

                                    group.item.Add(item);
                                }
                            }
                        }
                        else
                        {
                            var groupsitems2 = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();

                            var PlumbingCheckListFloors = groupsitems2.AsEnumerable().Select(a => a.Field<Int32?>("IdJobPlumbingCheckListFloors")).Distinct().ToList();

                            for (int j = 0; j < PlumbingCheckListFloors.Count(); j++)
                            {

                                Item item = new Item();
                                item.Details = new List<Details>();
                                var IdJobPlumbingCheckListFloors1 = PlumbingCheckListFloors[j];
                                var detailItem = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobPlumbingCheckListFloors") == IdJobPlumbingCheckListFloors1).ToList();
                                #region Items
                                item.FloorNumber = detailItem[0]["FloorNumber"] == DBNull.Value ? string.Empty : detailItem[0]["FloorNumber"].ToString();
                                item.idJobPlumbingCheckListFloors = IdJobPlumbingCheckListFloors1 != 0 ? IdJobPlumbingCheckListFloors1 : 0;
                                item.FloorDisplayOrder = detailItem[0]["FloorDisplayOrder"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[0]["FloorDisplayOrder"]);
                                #endregion

                                for (int i = 0; i < detailItem.Count; i++)
                                {
                                    Details detail = new Details();
                                    #region Items details
                                    detail.checklistGroupType = detailItem[i]["checklistGroupType"] == DBNull.Value ? string.Empty : detailItem[i]["checklistGroupType"].ToString();
                                    detail.idJobPlumbingInspection = detailItem[i]["IdJobPlumbingInspection"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingInspection"]);
                                    detail.idJobPlumbingCheckListFloors = detailItem[i]["IdJobPlumbingCheckListFloors"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingCheckListFloors"]);
                                    detail.inspectionPermit = detailItem[i]["checklistItemName"] == DBNull.Value ? string.Empty : detailItem[i]["checklistItemName"].ToString();
                                    //detail.floorName = ds.Tables[0].Rows[k]["FloorName"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[k]["FloorName"].ToString();
                                    detail.workOrderNumber = detailItem[i]["WorkOrderNo"] == DBNull.Value ? string.Empty : detailItem[i]["WorkOrderNo"].ToString();
                                    detail.plComments = detailItem[i]["Comments"] == DBNull.Value ? string.Empty : detailItem[i]["Comments"].ToString();
                                    detail.DueDate = detailItem[i]["DueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["DueDate"]);
                                    detail.nextInspection = detailItem[i]["NextInspection"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["NextInspection"]);
                                    detail.result = detailItem[i]["Result"] == DBNull.Value ? string.Empty : detailItem[i]["Result"].ToString();
                                    detail.HasDocument = rpoContext.JobDocuments.Any(x => x.IdJobPlumbingInspections == detail.idJobPlumbingInspection);
                                    detail.PlumbingInspectionDisplayOrder = detailItem[i]["PlumbingInspectionDisplayOrder"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["PlumbingInspectionDisplayOrder"]);
                                    item.Details.Add(detail);
                                    #endregion

                                }
                                group.item.Add(item);
                            }
                        }
                        ////copy done

                        header.groups.Add(group);
                    }

                    headerlist.Add(header);
                    headerrow++;
                }
                return Ok(headerlist);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }

        }


        /// <summary>
        /// View the CheckList.
        /// </summary>
        /// <param name="id">The identifier.</param>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/customerViewCheckList")]
        public IHttpActionResult customerViewCheckList(ChecklistSearch checkListSearch)
        {
            
            var employee = this.rpoContext.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewChecklist)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteChecklist))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();

                DataSet ds = new DataSet();
                SqlParameter[] spParameter = new SqlParameter[3];

                spParameter[0] = new SqlParameter("@IdJobCheckListHeader", SqlDbType.NVarChar);
                spParameter[0].Direction = ParameterDirection.Input;
                spParameter[0].Value = checkListSearch.ChecklistIds;

                spParameter[1] = new SqlParameter("@OrderFlag", SqlDbType.VarChar);
                spParameter[1].Direction = ParameterDirection.Input;
                spParameter[1].Value = checkListSearch.OrderFlag;

                spParameter[2] = new SqlParameter("@SearchText", SqlDbType.VarChar);
                spParameter[2].Direction = ParameterDirection.Input;
                spParameter[2].Value = !string.IsNullOrEmpty(checkListSearch.SearchText) ? checkListSearch.SearchText : string.Empty;

                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "ChecklistView", spParameter);
                List<CheckListGroupHeader> headerlist = new List<CheckListGroupHeader>();
                string[] headerid = checkListSearch.ChecklistIds.Split(',');


                List<DataTable> distinctheaders = ds.Tables[0].AsEnumerable()
                        .GroupBy(row => new
                        {
                            JobChecklistHeaderId = row.Field<int>("JobChecklistHeaderId"),                           
                        }).Select(g => g.CopyToDataTable()).ToList();
               
                int headerrow = 0;
                foreach (var loopheader in distinctheaders)
                {
                    CheckListGroupHeader header = new CheckListGroupHeader();
                    Int32 checklistHeaderId = Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                    header.jobChecklistHeaderId = loopheader.Rows[0]["JobChecklistHeaderId"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                    header.Others = loopheader.Rows[0]["Others"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["Others"].ToString();
                    header.applicationName = loopheader.Rows[0]["checklistNamewithFillingstatus"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["checklistNamewithFillingstatus"].ToString();
                    header.ChecklistCreatedDate = loopheader.Rows[0]["ChecklistCreatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(loopheader.Rows[0]["ChecklistCreatedDate"]);
                    header.ChecklistLastmodifiedDate = loopheader.Rows[0]["ChecklistLastmodifiedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(loopheader.Rows[0]["ChecklistLastmodifiedDate"]);
                    header.groups = new List<ChecklistGroup>();
                    header.IdJob = loopheader.Rows[0]["IdJob"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["IdJob"]);
                
                    string s = null;
                    int id = checklistHeaderId;                                    
                    var temp = rpoContext.JobChecklistHeaders.Include("JobApplicationWorkPermitTypes").Where(x => x.IdJobCheckListHeader == id).ToList();
                    var lstidjobworktype = temp[0].JobApplicationWorkPermitTypes.Where(x => x.Code.ToLower().Trim() == "pl" || x.Code.ToLower().Trim() == "sp" || x.Code.ToLower().Trim() == "sd").Select(y => y.IdJobWorkType).ToList();
                    foreach (var i in lstidjobworktype)
                    {
                        s += i.ToString() + ",";
                    }
                    if (s != null)
                        header.IdWorkPermits = s.Remove(s.Length - 1, 1);                   
                    var distinctGroup = ds.Tables[0].AsEnumerable().Where(i => i.Field<int>("JobChecklistHeaderId") == checklistHeaderId)
                        .GroupBy(r => new { group = r.Field<int>("IdJobChecklistGroup") }).Select(g => g.CopyToDataTable()).ToList();                  
                    foreach (var eachGroup in distinctGroup)
                    {

                        ChecklistGroup group = new ChecklistGroup();
                        group.item = new List<Item>();                      
                        {
                            Int32 IdJobChecklistGroup = Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                            group.checkListGroupName = eachGroup.Rows[0]["ChecklistGroupName"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["ChecklistGroupName"].ToString();
                            group.jobChecklistGroupId = eachGroup.Rows[0]["IdJobChecklistGroup"] == DBNull.Value ? (int?)null : Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                            group.checkListGroupType = eachGroup.Rows[0]["checklistGroupType"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["checklistGroupType"].ToString();
                            group.DisplayOrder1 = eachGroup.Rows[0]["DisplayOrder1"] == DBNull.Value ? (int?)null : Convert.ToInt32(eachGroup.Rows[0]["DisplayOrder1"]);


                            if (eachGroup.Rows[0]["checklistGroupType"].ToString().ToLower().TrimEnd() != "pl")
                            {


                                var groupsitems = ds.Tables[0].AsEnumerable().Where(l => l.Field<int>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();
                              
                                for (int j = 0; j < groupsitems.Count; j++)
                                {                                   
                                    Item item = new Item();
                                    item.Details = new List<Details>();

                                    #region Items
                                    Int32 IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
                                    if (IdChecklistItem != 0)
                                    {
                                        item.checklistItemName = groupsitems[j]["checklistItemName"] == DBNull.Value ? string.Empty : groupsitems[j]["checklistItemName"].ToString();
                                        item.IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
                                        item.jocbChecklistItemDetailsId = groupsitems[j]["JocbChecklistItemDetailsId"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["JocbChecklistItemDetailsId"]);
                                        int itemid = Convert.ToInt32(item.jocbChecklistItemDetailsId);
                                        item.HasDocument = rpoContext.JobDocuments.Any(x => x.IdJobchecklistItemDetails == itemid);
                                        item.comments = groupsitems[j]["Comments"] == DBNull.Value ? string.Empty : groupsitems[j]["Comments"].ToString();
                                        item.idDesignApplicant = groupsitems[j]["IdDesignApplicant"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdDesignApplicant"]);
                                        item.idInspector = groupsitems[j]["IdInspector"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdInspector"]);
                                        item.stage = groupsitems[j]["Stage"] == DBNull.Value ? string.Empty : groupsitems[j]["Stage"].ToString();
                                        item.partyResponsible1 = groupsitems[j]["PartyResponsible1"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["PartyResponsible1"]);
                                        item.manualPartyResponsible = groupsitems[j]["ManualPartyResponsible"] == DBNull.Value ? string.Empty : groupsitems[j]["ManualPartyResponsible"].ToString();
                                        item.idContact = groupsitems[j]["IdContact"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdContact"]);
                                        item.referenceDocumentId = groupsitems[j]["IdReferenceDocument"] == DBNull.Value ? string.Empty : groupsitems[j]["IdReferenceDocument"].ToString();
                                        item.dueDate = groupsitems[j]["DueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(groupsitems[j]["DueDate"]);
                                        item.status = groupsitems[j]["Status"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["Status"]);
                                        item.checkListLastModifiedDate = groupsitems[j]["checklistItemLastDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(groupsitems[j]["checklistItemLastDate"]);
                                        item.idCreateFormDocument = groupsitems[j]["IdCreateFormDocument"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdCreateFormDocument"]);
                                        item.idUploadFormDocument = groupsitems[j]["IdUploadFormDocument"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdUploadFormDocument"]);
                                        item.Displayorder = groupsitems[j]["Displayorder"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["Displayorder"]);
                                        item.ChecklistDetailDisplayOrder = groupsitems[j]["ChecklistDetailDisplayOrder"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["ChecklistDetailDisplayOrder"]);
                                        item.PartyResponsible = groupsitems[j]["PartyResponsible"] == DBNull.Value ? string.Empty : groupsitems[j]["PartyResponsible"].ToString();
                                        if (item.idContact != null && item.idContact != 0)
                                        {
                                            var c = rpoContext.Contacts.Find(item.idContact);
                                            item.ContactName = c.FirstName + c.LastName;
                                        }
                                        #endregion

                                        group.item.Add(item);
                                    }
                                }
                            }
                            else
                            {
                                var groupsitems2 = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();

                                var PlumbingCheckListFloors = groupsitems2.AsEnumerable().Select(a => a.Field<Int32?>("IdJobPlumbingCheckListFloors")).Distinct().ToList();

                                for (int j = 0; j < PlumbingCheckListFloors.Count(); j++)
                                {

                                    Item item = new Item();
                                    item.Details = new List<Details>();
                                    var IdJobPlumbingCheckListFloors1 = PlumbingCheckListFloors[j];
                                    var detailItem = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobPlumbingCheckListFloors") == IdJobPlumbingCheckListFloors1).ToList();
                                    #region Items
                                    item.FloorNumber = detailItem[0]["FloorNumber"] == DBNull.Value ? string.Empty : detailItem[0]["FloorNumber"].ToString();
                                    item.idJobPlumbingCheckListFloors = IdJobPlumbingCheckListFloors1 != 0 ? IdJobPlumbingCheckListFloors1 : 0;
                                    item.FloorDisplayOrder = detailItem[0]["FloorDisplayOrder"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[0]["FloorDisplayOrder"]);
                                    #endregion

                                    for (int i = 0; i < detailItem.Count; i++)
                                    {
                                        Details detail = new Details();
                                        #region Items details
                                        detail.checklistGroupType = detailItem[i]["checklistGroupType"] == DBNull.Value ? string.Empty : detailItem[i]["checklistGroupType"].ToString();
                                        detail.idJobPlumbingInspection = detailItem[i]["IdJobPlumbingInspection"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingInspection"]);
                                        detail.idJobPlumbingCheckListFloors = detailItem[i]["IdJobPlumbingCheckListFloors"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingCheckListFloors"]);
                                        detail.inspectionPermit = detailItem[i]["checklistItemName"] == DBNull.Value ? string.Empty : detailItem[i]["checklistItemName"].ToString();                                       
                                        detail.workOrderNumber = detailItem[i]["WorkOrderNo"] == DBNull.Value ? string.Empty : detailItem[i]["WorkOrderNo"].ToString();
                                        detail.plComments = detailItem[i]["Comments"] == DBNull.Value ? string.Empty : detailItem[i]["Comments"].ToString();
                                        detail.DueDate = detailItem[i]["DueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["DueDate"]);
                                        detail.nextInspection = detailItem[i]["NextInspection"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["NextInspection"]);
                                        detail.result = detailItem[i]["Result"] == DBNull.Value ? string.Empty : detailItem[i]["Result"].ToString();
                                        detail.HasDocument = rpoContext.JobDocuments.Any(x => x.IdJobPlumbingInspections == detail.idJobPlumbingInspection);
                                        detail.PlumbingInspectionDisplayOrder = detailItem[i]["PlumbingInspectionDisplayOrder"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["PlumbingInspectionDisplayOrder"]);
                                        item.Details.Add(detail);
                                        #endregion

                                    }
                                    group.item.Add(item);
                                }
                            }
                        }
                        ////copy done

                        header.groups.Add(group);
                    }

                    headerlist.Add(header);
                    headerrow++;
                }
                return Ok(headerlist);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }

        }

        /// <summary>
        /// Plumbing Inspection Get Comment.
        /// </summary>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/GetPLcommentHistory/{IdJobPlumbingInspection}")]
        public IHttpActionResult GetPLcommentHistory(int IdJobPlumbingInspection)
        {

            var getPLCommentHistory = this.rpoContext.JobPlumbingInspectionComments.Where(x => x.IdJobPlumbingInspection == IdJobPlumbingInspection);
            if (getPLCommentHistory == null)
            {
                return this.NotFound();
            }

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            var result = this.rpoContext.JobPlumbingInspectionComments
                        .Where(x => x.IdJobPlumbingInspection == IdJobPlumbingInspection)
                .AsEnumerable()
                .Select(pLcommentHistory => new
                {
                    IdJobPlumbingInspection = pLcommentHistory.IdJobPlumbingInspection,
                    Description = pLcommentHistory.Description,
                    CreatedDate = pLcommentHistory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc
                                            (Convert.ToDateTime(pLcommentHistory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone))
                                            : pLcommentHistory.CreatedDate,
                    LastModifiedDate = pLcommentHistory.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc
                                            (Convert.ToDateTime(pLcommentHistory.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone))
                                            : (pLcommentHistory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(pLcommentHistory.CreatedDate),
                                            TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : pLcommentHistory.CreatedDate)
                }).OrderByDescending(x => x.LastModifiedDate);

            return Ok(result);
        }

        /// <summary>
        /// Plumbing Inspection Edit Comment.
        /// </summary>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/PostPLInspectionComment")]
        public IHttpActionResult PostPLInspectionComment(PlumbingInspectionDTO inspectionAddComment)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            JobPlumbingInspectionComment plumbingInspectionCommentHistory = new JobPlumbingInspectionComment();
            plumbingInspectionCommentHistory.IdJobPlumbingInspection = inspectionAddComment.IdJobPlumbingInspection;
            plumbingInspectionCommentHistory.Description = inspectionAddComment.Description;
            plumbingInspectionCommentHistory.CreatedDate = DateTime.UtcNow;
            plumbingInspectionCommentHistory.LastModifiedDate = DateTime.UtcNow;

            rpoContext.JobPlumbingInspectionComments.Add(plumbingInspectionCommentHistory);

            try { rpoContext.SaveChanges(); }
            catch (Exception ex) { throw ex; }           
            return Ok("Comment Added Successfully");
        }

        /// <summary>
        /// Plumbing Inspection Edit DueDate.
        /// </summary>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/PostPLInspectionDueDate")]
        public IHttpActionResult PostPLInspectionDueDate(PLInspectionDueDateDTO pLInspectionDueDate)
        {
            var Message = string.Empty;
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            if (pLInspectionDueDate != null)
            {
                JobPlumbingInspectionDueDate plumbingInspectionDueDate = new JobPlumbingInspectionDueDate();
                plumbingInspectionDueDate.IdJobPlumbingInspection = pLInspectionDueDate.IdJobPlumbingInspection;
                plumbingInspectionDueDate.DueDate = pLInspectionDueDate.DueDate;
                plumbingInspectionDueDate.CreatedDate = DateTime.UtcNow;
                plumbingInspectionDueDate.LastModifiedDate = DateTime.UtcNow;
                this.rpoContext.JobPlumbingInspectionDueDate.Add(plumbingInspectionDueDate);

                try
                {
                    this.rpoContext.SaveChanges();
                }
                catch (Exception ex) { throw ex.InnerException; }
            }
            return Ok();
        }

        /// <summary>
        /// Plumbing Inspection Edit WorkOrder.
        /// </summary>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/PutPLInspectionWorkOrder")]
        public IHttpActionResult PutPLInspectionWorkOrder(PlumbingInspectionDTO plumbingInspectiondto)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (plumbingInspectiondto != null)
            {
                var result = rpoContext.JobPlumbingInspection.Where(y => y.IdJobPlumbingInspection
                                                                                     == plumbingInspectiondto.IdJobPlumbingInspection).ToList();
                foreach (var res in result)
                {
                    res.WorkOrderNo = plumbingInspectiondto.WorkOrder;
                    res.LastModifiedDate = DateTime.UtcNow;
                    res.LastModifiedBy = employee != null ? employee.Id : 0;
                }
                try
                {
                    this.rpoContext.SaveChanges();
                }
                catch (Exception ex) { throw ex.InnerException; }
            }

            return Ok();
        }

        /// <summary>
        /// Manage the orders of the checlist Group.
        /// </summary>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/ManageChecklistGroupOrder")]
        public IHttpActionResult ManageChecklistGroupOrder([FromBody] ManageOrderDTO manageOrderDTO)
        {
            var Message = string.Empty;
            try
            {
                if (manageOrderDTO != null)
                {
                    foreach (var detailid in manageOrderDTO.Groups)
                    {
                        var result = rpoContext.JobChecklistGroups.Where(x => x.Id == detailid.JobChecklistGroupId &&
                                                                           x.IdJobCheckListHeader == manageOrderDTO.JobChecklistHeaderId).ToList();
                        foreach (var res in result)
                        {
                            res.Displayorder1 = detailid.DisplayOrder1;
                        }
                        this.rpoContext.SaveChanges();                       
                        JobChecklistHeader jobChecklistHeader = rpoContext.JobChecklistHeaders.Find(manageOrderDTO.JobChecklistHeaderId);
                        jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;
                        this.rpoContext.SaveChanges();
                    }
                    Message = StaticMessages.ManageGroupOrder_Successful;
                }
                else { Message = StaticMessages.ManageOrder_UnSuccessful; }
            }
            catch (Exception ex) { throw ex.InnerException; }
            return Ok(Message);
        }

        /// <summary>
        /// Deletes the CheckListItem from a checklist.
        /// </summary>
        /// <param name="id">The identifier.</param>
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/DeleteItemFromChecklist/{JocbChecklistItemDetailsId}")]
        public IHttpActionResult DeleteItemFromChecklist(int JocbChecklistItemDetailsId)
        {
            string Message = string.Empty;
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            var checklistemdetail_item = rpoContext.JobChecklistItemDetails.Where(b => b.Id == JocbChecklistItemDetailsId).ToList();
            foreach (var ct in checklistemdetail_item)
            {
                if (this.rpoContext.JobChecklistItemDueDate.Count(b => b.IdJobChecklistItemDetail == ct.Id) > 0)
                {
                    var jobduedate = this.rpoContext.JobChecklistItemDueDate.Where(a => a.IdJobChecklistItemDetail == ct.Id).ToList();
                    foreach (var item in jobduedate)
                    {
                        this.rpoContext.JobChecklistItemDueDate.Remove(item);
                        rpoContext.SaveChanges();
                    }
                }
                if (this.rpoContext.JobCheckListCommentHistories.Count(b => b.IdJobChecklistItemDetail == ct.Id) > 0)
                {
                    var commentHistory = this.rpoContext.JobCheckListCommentHistories.Where(a => a.IdJobChecklistItemDetail == ct.Id).ToList();
                    foreach (var item1 in commentHistory)
                    {
                        this.rpoContext.JobCheckListCommentHistories.Remove(item1);
                        rpoContext.SaveChanges();
                    }

                }
                if (this.rpoContext.JobCheckListProgressNoteHistories.Count(b => b.IdJobChecklistItemDetail == ct.Id) > 0)
                {
                    var jobCheckListProgressNoteHistories = this.rpoContext.JobCheckListProgressNoteHistories.Where(a => a.IdJobChecklistItemDetail == ct.Id).ToList();
                    foreach (var item1 in jobCheckListProgressNoteHistories)
                    {
                        this.rpoContext.JobCheckListProgressNoteHistories.Remove(item1);
                        rpoContext.SaveChanges();
                    }

                }


                if (this.rpoContext.JobDocuments.Count(b => b.IdJobchecklistItemDetails == ct.Id) > 0)
                {
                    var documents = this.rpoContext.JobDocuments.Where(x => x.IdJobchecklistItemDetails == ct.Id).ToList();
                    foreach (var item1 in documents)
                    {
                        JobDocument j = rpoContext.JobDocuments.Find(item1.Id);
                        j.IdJobchecklistItemDetails = null;
                        rpoContext.SaveChanges();                       
                    }
                    rpoContext.SaveChanges();
                }
                int JobChecklistGroupsID = rpoContext.JobChecklistItemDetails.Find(JocbChecklistItemDetailsId).IdJobChecklistGroup;
                if (this.rpoContext.JobChecklistItemDetails.Count(b => b.Id == ct.Id) > 0)
                {
                    var checklistItemDetail = this.rpoContext.JobChecklistItemDetails.Where(a => a.Id == ct.Id).ToList();
                    foreach (var item1 in checklistItemDetail)
                    {
                        this.rpoContext.JobChecklistItemDetails.Remove(item1);
                        rpoContext.SaveChanges();
                    }
                }


                var jobchecklistheaderid = rpoContext.JobChecklistGroups.Where(x => x.Id == JobChecklistGroupsID).FirstOrDefault().IdJobCheckListHeader;
                JobChecklistHeader jobChecklistHeader = rpoContext.JobChecklistHeaders.Find(jobchecklistheaderid);
                jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;              
               
            }
            try
            {
                this.rpoContext.SaveChanges();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChecklistItemDetailsExists(JocbChecklistItemDetailsId))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        /// <summary>
        /// Deletes the CheckListItem from a checklist.
        /// </summary>
        /// <param name="id">The identifier.</param>
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/DeletePLInspection/{IdjobchecklistHeader}")]
        public IHttpActionResult DeletePLInspection(int IdjobchecklistHeader)
        {
            string Message = string.Empty;
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

            var pLdeleteId = rpoContext.JobPlumbingInspection.Where(b => b.IdJobPlumbingInspection == IdjobchecklistHeader).ToList();
            if (pLdeleteId != null)
            {
                foreach (var pldelete in pLdeleteId)
                {
                    if (this.rpoContext.JobPlumbingInspectionComments.Count(b => b.IdJobPlumbingInspection == pldelete.IdJobPlumbingInspection) > 0)
                    {
                        var commentHistory = this.rpoContext.JobPlumbingInspectionComments.Where(a => a.IdJobPlumbingInspection == pldelete.IdJobPlumbingInspection).ToList();
                        foreach (var item1 in commentHistory)
                        {
                            this.rpoContext.JobPlumbingInspectionComments.Remove(item1);
                            rpoContext.SaveChanges();
                        }
                    }
                    if (this.rpoContext.ClientNotePlumbingCustomers.Count(b => b.IdJobPlumbingInspection == pldelete.IdJobPlumbingInspection) > 0)
                    {
                        var clientnoteHistory = this.rpoContext.ClientNotePlumbingCustomers.Where(a => a.IdJobPlumbingInspection == pldelete.IdJobPlumbingInspection).ToList();
                        foreach (var item1 in clientnoteHistory)
                        {
                            this.rpoContext.ClientNotePlumbingCustomers.Remove(item1);
                            rpoContext.SaveChanges();
                        }
                    }
                    if (this.rpoContext.JobPlumbingInspectionDueDate.Count(b => b.IdJobPlumbingInspection == pldelete.IdJobPlumbingInspection) > 0)
                    {
                        var inspectduedate = this.rpoContext.JobPlumbingInspectionDueDate.Where(a => a.IdJobPlumbingInspection == pldelete.IdJobPlumbingInspection).ToList();
                        foreach (var item1 in inspectduedate)
                        {
                            this.rpoContext.JobPlumbingInspectionDueDate.Remove(item1);
                            rpoContext.SaveChanges();
                        }

                    }
                    if (this.rpoContext.JobPlumbingInspectionProgressNoteHistory.Count(b => b.IdJobPlumbingInspection == pldelete.IdJobPlumbingInspection) > 0)
                    {
                        var inspectduedate = this.rpoContext.JobPlumbingInspectionProgressNoteHistory.Where(a => a.IdJobPlumbingInspection == pldelete.IdJobPlumbingInspection).ToList();
                        foreach (var item1 in inspectduedate)
                        {
                            this.rpoContext.JobPlumbingInspectionProgressNoteHistory.Remove(item1);
                            rpoContext.SaveChanges();
                        }
                    }
                    if (this.rpoContext.JobDocuments.Count(b => b.IdJobPlumbingInspections == pldelete.IdJobPlumbingInspection) > 0)
                    {
                        var documents = this.rpoContext.JobDocuments.Where(x => x.IdJobPlumbingInspections == pldelete.IdJobPlumbingInspection).ToList();
                        foreach (var item1 in documents)
                        {
                            JobDocument j = this.rpoContext.JobDocuments.Find(item1.Id);
                            j.IdJobPlumbingInspections = null;

                          rpoContext.SaveChanges();                           
                        }                

                    }
                    rpoContext.SaveChanges();

                    this.rpoContext.JobPlumbingInspection.Remove(pldelete);
                }
            }
            try
            {
                this.rpoContext.SaveChanges();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChecklistExists(IdjobchecklistHeader))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok("Deleted Successfully");
        }
        /// <summary>
        /// Plumbing Inspection Status Update.
        /// </summary>

        [Authorize]
        [RpoAuthorize]
        [Route("api/ChecklistL/PutPlumbingInspectionResult")]
        public IHttpActionResult PutPlumbingInspectionResult(PlumbingInsceptionResult plumbingInsceptionResult)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            JobPlumbingInspection jobPlumbingInspection = rpoContext.JobPlumbingInspection.FirstOrDefault(x => x.IdJobPlumbingInspection == plumbingInsceptionResult.IdJobPlumbingInspection);

            if (jobPlumbingInspection == null)
            {
                return this.NotFound();
            }

            jobPlumbingInspection.Result = plumbingInsceptionResult.Result != null ? plumbingInsceptionResult.Result : string.Empty;
            jobPlumbingInspection.LastModifiedDate = DateTime.UtcNow;
            if (employee != null)
            {
                jobPlumbingInspection.LastModifiedBy = employee.Id;
            }
            try
            {
                this.rpoContext.SaveChanges();
            }
            catch (Exception ex) { throw ex.InnerException; }
            return this.Ok(plumbingInsceptionResult);
        }
        //Search in checklist
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/GetChecklistSearchResponse")]
        public IHttpActionResult GetChecklistSearchResponse(ChecklistSearchResponseDTO checklistSearchResponseDTO)
        {
            CheckListViewDTO checklistviewdto;
            List<CheckListViewDTO> checklistdto_All = new List<CheckListViewDTO>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();

                DataSet ds = new DataSet();
                SqlParameter[] spParameter = new SqlParameter[3];

                spParameter[0] = new SqlParameter("@IDs", SqlDbType.NVarChar);
                spParameter[0].Value = checklistSearchResponseDTO.jobchecklistheadersID;
                spParameter[1] = new SqlParameter("@OrderFlag", SqlDbType.VarChar);
                spParameter[1].Value = checklistSearchResponseDTO.OrderFlag;
                spParameter[2] = new SqlParameter("@SearchText", SqlDbType.VarChar);
                spParameter[2].Value = checklistSearchResponseDTO.searchText;

                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "SearchChecklist", spParameter);

                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string permitCode = string.Empty;
                        int IdJobApplication = Convert.ToInt32(ds.Tables[0].Rows[i]["IdJobApplication"]);
                        int IdJob = Convert.ToInt32(ds.Tables[0].Rows[i]["IdJob"]);
                        if (IdJobApplication != 0 && IdJob != 0)
                        {
                            var jobApplicationWorkPermitTypes = rpoContext.JobApplicationWorkPermitTypes.Include("JobApplication.JobApplicationType")
                                .Include("JobWorkType").Include("CreatedByEmployee").Include("LastModifiedByEmployee").Include("ContactResponsible").Where(c => c.IdJobApplication == IdJobApplication && c.JobApplication.IdJob == IdJob).AsQueryable();


                            foreach (var item in jobApplicationWorkPermitTypes)
                            {
                                if (item.Code != null)
                                {
                                    permitCode += ", " + item.Code;
                                }
                            }
                        }
                        checklistviewdto = new CheckListViewDTO();
                        checklistviewdto.ChecklistGroupName = ds.Tables[0].Rows[i]["ChecklistGroupName"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[i]["ChecklistGroupName"].ToString();
                        checklistviewdto.ChecklistGroupType = ds.Tables[0].Rows[i]["ChecklistGroupType"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[i]["ChecklistGroupType"].ToString();
                        checklistviewdto.checklistItems = ds.Tables[0].Rows[i]["checklistItems"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[i]["checklistItems"].ToString();
                        checklistviewdto.Comments = ds.Tables[0].Rows[i]["Comments"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[i]["Comments"].ToString();                        
                        checklistviewdto.IdReferenceDocument = ds.Tables[0].Rows[i]["IdReferenceDocument"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[i]["IdReferenceDocument"].ToString();
                        checklistviewdto.DocumentName = ds.Tables[0].Rows[i]["DocumentName"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[i]["DocumentName"].ToString();
                        checklistviewdto.DueDate = ds.Tables[0].Rows[i]["DueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(ds.Tables[0].Rows[i]["DueDate"]);
                        checklistviewdto.Status = Convert.IsDBNull(ds.Tables[0].Rows[i]["Status"]) ? (int?)null : Convert.ToInt32(ds.Tables[0].Rows[i]["Status"]);
                        checklistviewdto.checklistItemLastDate = Convert.IsDBNull(ds.Tables[0].Rows[i]["checklistItemLastDate"]) ? (DateTime?)null : Convert.ToDateTime(ds.Tables[0].Rows[i]["checklistItemLastDate"]);
                        checklistviewdto.IdCreateFormDocument = Convert.IsDBNull(ds.Tables[0].Rows[i]["IdCreateFormDocument"]) ? (int?)null : Convert.ToInt32(ds.Tables[0].Rows[i]["IdCreateFormDocument"]);
                        checklistviewdto.IdUploadFormDocument = Convert.IsDBNull(ds.Tables[0].Rows[i]["IdUploadFormDocument"]) ? (int?)null : Convert.ToInt32(ds.Tables[0].Rows[i]["IdUploadFormDocument"]);
                        checklistviewdto.JobChecklistHeaderId = ds.Tables[0].Rows[i]["JobChecklistHeaderId"] == DBNull.Value ? (int?)null : Convert.ToInt32(ds.Tables[0].Rows[i]["JobChecklistHeaderId"]);
                        checklistviewdto.JobChecklistGroupId = ds.Tables[0].Rows[i]["JobChecklistGroupId"] == DBNull.Value ? (int?)null : Convert.ToInt32(ds.Tables[0].Rows[i]["JobChecklistGroupId"]);
                        checklistviewdto.JocbChecklistItemDetailsId = ds.Tables[0].Rows[i]["JocbChecklistItemDetailsId"] == DBNull.Value ? (int?)null : Convert.ToInt32(ds.Tables[0].Rows[i]["JocbChecklistItemDetailsId"]);
                        checklistviewdto.jobchecklistItemdetail_checklistItem = ds.Tables[0].Rows[i]["jobchecklistItemdetail_checklistItem"] == DBNull.Value ? (int?)null : Convert.ToInt32(ds.Tables[0].Rows[i]["jobchecklistItemdetail_checklistItem"]);
                        checklistviewdto.ApplicationNumber = ds.Tables[0].Rows[i]["ApplicationNumber"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[i]["ApplicationNumber"].ToString();
                        checklistviewdto.IdJobPlumbingInspection = ds.Tables[0].Rows[i]["IdJobPlumbingInspection"] == DBNull.Value ? "NULL" : ds.Tables[0].Rows[i]["IdJobPlumbingInspection"].ToString();
                        checklistviewdto.InspectionPermit = ds.Tables[0].Rows[i]["InspectionPermit"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[i]["InspectionPermit"].ToString();
                        checklistviewdto.FloorName = ds.Tables[0].Rows[i]["FloorName"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[i]["FloorName"].ToString();
                        checklistviewdto.WorkOrderNumber = ds.Tables[0].Rows[i]["WorkOrderNumber"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[i]["WorkOrderNumber"].ToString();                       
                        checklistviewdto.NextInspection = ds.Tables[0].Rows[i]["NextInspection"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(ds.Tables[0].Rows[i]["NextInspection"]);                      
                        //for TR Items
                        checklistviewdto.Stage = ds.Tables[0].Rows[i]["Stage"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[i]["Stage"].ToString();
                        checklistviewdto.IdInspector = Convert.IsDBNull(ds.Tables[0].Rows[i]["IdInspector"]) ? (int?)null : Convert.ToInt32(ds.Tables[0].Rows[i]["IdInspector"]);
                        checklistviewdto.IdDesignApplicant = Convert.IsDBNull(ds.Tables[0].Rows[i]["IdDesignApplicant"]) ? (int?)null : Convert.ToInt32(ds.Tables[0].Rows[i]["IdDesignApplicant"]);
                        checklistviewdto.Code = permitCode.TrimStart(',');
                        checklistdto_All.Add(checklistviewdto);
                    }
                }                
            }
            catch (SqlException ex) { throw ex.InnerException; }

            var searchDTO = checklistdto_All
                   .GroupBy(u => new { u.JobChecklistHeaderId, u.ApplicationNumber })
                   .Select(grp => new
                   {
                       ApplicationNumber = grp.Key.ApplicationNumber,
                       JobChecklistHeaderId = grp.Key.JobChecklistHeaderId,
                       Groups = grp.GroupBy(x => new
                       {
                           x.JobChecklistGroupId,
                           x.ChecklistGroupName,
                           x.ChecklistGroupType,
                           x.Code
                       }).Select(y => new
                       {
                           JobChecklistGroupId = y.Key.JobChecklistGroupId,
                           ChecklistGroupName = y.Key.ChecklistGroupName,
                           ChecklistGroupType = y.Key.ChecklistGroupType,                          
                           
                           items = y.Select(p => new
                           {
                               JocbChecklistItemDetailsId = p.JocbChecklistItemDetailsId,
                               ChecklistItemName = p.checklistItems,
                               Comments = p.Comments,
                               PartyResponsible = p.ManualPartyResponsible,
                               ReferenceDocumentId = p.IdReferenceDocument,
                               DocumentName = p.DocumentName,
                               DueDate = p.DueDate,
                               Status = p.Status,
                               checklistItem_LastModifiedDate = p.checklistItemLastDate,
                               IdCreateFormDocument = p.IdCreateFormDocument,
                               IdUploadFormDocument = p.IdUploadFormDocument,
                               jobchecklistItemdetail_checklistItem = p.jobchecklistItemdetail_checklistItem,
                               PLItems = y.Key.ChecklistGroupType == "PL" ? y.Select(a => new
                               {
                                   ChecklistGroupType = y.Key.ChecklistGroupType,
                                   IdJobPlumbingInspection = a.IdJobPlumbingInspection,
                                   InspectionPermit = a.InspectionPermit,
                                   FloorName = a.FloorName,
                                   WorkOrderNumber = a.WorkOrderNumber,
                                   PLComments = a.Comments,
                                   NextInspection = a.NextInspection,
                                   PLStatus = a.Status
                               }).ToArray() : null
                           }).ToArray()
                       }).ToArray()
                   }).ToList();
            return Ok(searchDTO);

        }

        /// <summary>
        /// Gets the task.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns> Gets the task.</returns>
        [ResponseType(typeof(checklitItemTransmittalDTO))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/GetChecklistTransmittal/{jobchecklistItemID}")]
        public IHttpActionResult GetChecklistTransmittal(int jobchecklistItemID)
        {           
            var checklst = rpoContext.JobChecklistItemDetails.Include("JobChecklistGroups").FirstOrDefault(x => x.Id == jobchecklistItemID);
            int IdJobCheckListHeader = checklst.JobChecklistGroups.IdJobCheckListHeader;
            int Idjob = rpoContext.JobChecklistHeaders.Include("Jobs").FirstOrDefault(x => x.IdJobCheckListHeader == IdJobCheckListHeader).IdJob;
            var job = rpoContext.Jobs.Include("Companies").FirstOrDefault(x => x.Id == Idjob);
            int IdCompany = (int)job.IdCompany;
            int IdContact = (int)job.IdContact;
            checklitItemTransmittalDTO objchecklitItemTransmittalDTO = new checklitItemTransmittalDTO();
            objchecklitItemTransmittalDTO.IdCompany = (int)job.IdCompany; ;
            objchecklitItemTransmittalDTO.IdContact = (int)job.IdContact;
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            return Ok(objchecklitItemTransmittalDTO);
        }
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/checklist/ActiveCheckListItemDropDown")]
        public IHttpActionResult ActiveCheckListItemDropDown()
        {
            var result = this.rpoContext.ChecklistItems.Where(x => x.IsActive == true);


            return this.Ok(result);
        }
        private checklitItemTransmittalDTO FormatChecklistItem(/*checklistItem checklistitem*/ JobChecklistItemDetail jobChecklistItemDetail)
        {
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            return new checklitItemTransmittalDTO
            {  
                ContactName = jobChecklistItemDetail.Contacts != null ? jobChecklistItemDetail.Contacts.FirstName + " " + jobChecklistItemDetail.Contacts.LastName : string.Empty,                
                CompanyName = (from cmpy in rpoContext.Companies select cmpy.Name).ToList(),
            };
        }

    }
}