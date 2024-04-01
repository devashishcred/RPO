using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// Author           : Mital Bhatt 
// Created          : 
//
// Last Modified By : Mital Bhatt 
// Last Modified On : 29-12-2022
// ***********************************************************************
// <copyright file="CompositeChecklist.cs" company="CREDENCYS">
//     Copyright ©  2022
// </copyright>
// <summary>Class Composite Checklist Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Composite Checklist namespace.
/// </summary>

namespace Rpo.ApiServices.Api.Controllers.CompositeChecklist
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
    using Rpo.ApiServices.Api.Controllers.CompositeChecklist.Models;
    using System.Text;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using Microsoft.ApplicationBlocks.Data;
    using Rpo.ApiServices.Api.Enums;
    using Rpo.ApiServices.Model.Models.Enums;
    using Rpo.ApiServices.Api.Controllers.JobViolations;
    using System.Globalization;

    public class CompositeChecklistController : ApiController
    {
        private RpoContext db = new RpoContext();
     
        #region New List of composite checklist
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/CompositeChecklist/ListOfChecklist/{IdJob}/{Parentchecklistheaderid}")]
        public IHttpActionResult ListOfChecklist(int IdJob,int Parentchecklistheaderid)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewChecklist))
            {
                Job job = db.Jobs.FirstOrDefault(j => j.Id == IdJob);

                if (job == null)
                    return this.NotFound();

                var result1 = job.Jobs.ToList();

                List<int> Headerids = new List<int>();
                var result = (from p in db.JobChecklistHeaders
                              join jobapp in db.JobApplications on p.IdJob equals jobapp.IdJob
                              join jt in db.JobApplicationTypes on jobapp.IdJobApplicationType equals jt.Id
                              where p.IdJob == IdJob && p.IdJobApplication == jobapp.Id
                              select new
                              {
                                  Id = p.IdJobCheckListHeader,
                                  ChecklistName = p.ChecklistName,
                                  IdJob = p.IdJob
                              }).ToList();
              var RemoveParentChecklistHeaderId= result.Where(x => x.Id == Parentchecklistheaderid).FirstOrDefault();
                result.Remove(RemoveParentChecklistHeaderId);
                foreach (var e in result)
                    Headerids.Add(e.Id);

                List<ListofGeneralChecklist> lstJobChecklistHeaders = new List<ListofGeneralChecklist>();

                foreach (var a in result1)
                {

                    var id = a.Id;
                    var result2 = (from p in db.JobChecklistHeaders
                                   join jobapp in db.JobApplications on p.IdJob equals jobapp.IdJob
                                   join jt in db.JobApplicationTypes on jobapp.IdJobApplicationType equals jt.Id
                                   where p.IdJob == id && p.IdJobApplication == jobapp.Id
                                   select new
                                   {
                                       Id = p.IdJobCheckListHeader,
                                   }).ToList();
                    foreach (var d in result2)
                        Headerids.Add(d.Id);
                }


                foreach (var d in Headerids)
                {
                    lstJobChecklistHeaders.Add(db.JobChecklistHeaders
                        .Where(x => x.IdJobCheckListHeader == d)
                        .Select(p => new ListofGeneralChecklist() { IdJob = p.IdJob, ChecklistName = p.ChecklistName, IdJobCheckListHeader = p.IdJobCheckListHeader })
                        .FirstOrDefault());

                }
                List<GeneralChecklistResult> lstJobChecklistHeaders1 = new List<GeneralChecklistResult>();
                GeneralChecklistResult obj;
                IEnumerable<int> ids = lstJobChecklistHeaders.Select(i => i.IdJob).Distinct();
                foreach (var e in ids)
                {
                    obj = new GeneralChecklistResult();
                    obj.IdJob = e;
                    obj.listofGeneralChecklist = lstJobChecklistHeaders
                                                .Where(c => c.IdJob == e)
                                                .Select(g => new ListofGeneralChecklist() { ChecklistName = g.ChecklistName, IdJobCheckListHeader = g.IdJobCheckListHeader, IdJob = g.IdJob })
                                                .ToList();
                    lstJobChecklistHeaders1.Add(obj);
                }
                return Ok(lstJobChecklistHeaders1);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }

        }
        #endregion
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/CompositeChecklist/CustomerListOfChecklist/{IdJob}/{Parentchecklistheaderid}")]
        public IHttpActionResult CustomerListOfChecklist(int IdJob, int Parentchecklistheaderid)
        {
            var employee = this.db.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewChecklist))
            {
                Job job = db.Jobs.FirstOrDefault(j => j.Id == IdJob);

                if (job == null)
                    return this.NotFound();

                var result1 = job.Jobs.ToList();

                List<int> Headerids = new List<int>();
                var result = (from p in db.JobChecklistHeaders
                              join jobapp in db.JobApplications on p.IdJob equals jobapp.IdJob
                              join jt in db.JobApplicationTypes on jobapp.IdJobApplicationType equals jt.Id
                              where p.IdJob == IdJob && p.IdJobApplication == jobapp.Id
                              select new
                              {
                                  Id = p.IdJobCheckListHeader,
                                  ChecklistName = p.ChecklistName,
                                  IdJob = p.IdJob
                              }).ToList();
                var RemoveParentChecklistHeaderId = result.Where(x => x.Id == Parentchecklistheaderid).FirstOrDefault();
                result.Remove(RemoveParentChecklistHeaderId);
                foreach (var e in result)
                    Headerids.Add(e.Id);

                List<ListofGeneralChecklist> lstJobChecklistHeaders = new List<ListofGeneralChecklist>();

                foreach (var a in result1)
                {

                    var id = a.Id;
                    var result2 = (from p in db.JobChecklistHeaders
                                   join jobapp in db.JobApplications on p.IdJob equals jobapp.IdJob
                                   join jt in db.JobApplicationTypes on jobapp.IdJobApplicationType equals jt.Id
                                   where p.IdJob == id && p.IdJobApplication == jobapp.Id
                                   select new
                                   {
                                       Id = p.IdJobCheckListHeader,
                                   }).ToList();
                    foreach (var d in result2)
                        Headerids.Add(d.Id);
                }


                foreach (var d in Headerids)
                {
                    lstJobChecklistHeaders.Add(db.JobChecklistHeaders
                        .Where(x => x.IdJobCheckListHeader == d)
                        .Select(p => new ListofGeneralChecklist() { IdJob = p.IdJob, ChecklistName = p.ChecklistName, IdJobCheckListHeader = p.IdJobCheckListHeader })
                        .FirstOrDefault());

                }
                List<GeneralChecklistResult> lstJobChecklistHeaders1 = new List<GeneralChecklistResult>();
                GeneralChecklistResult obj;
                IEnumerable<int> ids = lstJobChecklistHeaders.Select(i => i.IdJob).Distinct();
                foreach (var e in ids)
                {
                    obj = new GeneralChecklistResult();
                    obj.IdJob = e;
                    obj.listofGeneralChecklist = lstJobChecklistHeaders
                                                .Where(c => c.IdJob == e)
                                                .Select(g => new ListofGeneralChecklist() { ChecklistName = g.ChecklistName, IdJobCheckListHeader = g.IdJobCheckListHeader, IdJob = g.IdJob })
                                                .ToList();
                    lstJobChecklistHeaders1.Add(obj);
                }
                return Ok(lstJobChecklistHeaders1);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }

        }    
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CompositeChecklistDTO))]
        public IHttpActionResult PostCompositeChecklist(CompositeChecklistDTO compositeChecklistDTO)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewChecklist)
                   || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist)
                   || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                CompositeChecklist compositeChecklist = new CompositeChecklist();
                compositeChecklist.Name = "CC - " + compositeChecklistDTO.Name;
                compositeChecklist.CreatedDate = DateTime.UtcNow;
                compositeChecklist.LastModifiedDate = DateTime.UtcNow;
                compositeChecklist.ParentJobId = compositeChecklistDTO.ParentJobId;
                compositeChecklist.IsCOProject = compositeChecklistDTO.IsCOProject;
                if (employee != null)
                {
                    compositeChecklist.CreatedBy = employee.Id;
                }
                db.CompositeChecklists.Add(compositeChecklist);
                db.SaveChanges();
                List<CompositeChecklistDetail> lstCompositeChecklistDetail = new List<CompositeChecklistDetail>();

                var jobchecklistgroupIds = db.JobChecklistGroups.Where(x => x.IdJobCheckListHeader == compositeChecklistDTO.ParentChecklistheaderId).Select(y => y.Id).ToList();
                if (compositeChecklistDTO.IsCOProject == true) //If CO Project selected for composite checklist
                {                 
                    var items = db.JobChecklistItemDetails.Where(y => db.JobChecklistGroups.Where(x => x.IdJobCheckListHeader == compositeChecklistDTO.ParentChecklistheaderId).ToList().Select(z => z.Id)
                        .Contains(y.IdJobChecklistGroup)).ToList();
                    foreach (var item in items)
                    {
                        if (item.IsRequiredForTCO != true)
                        {
                            item.IsRequiredForTCO = false;
                        }
                    }                   
                   var plitems= db.JobPlumbingInspection.Where(y => db.JobChecklistGroups.Where(x => x.IdJobCheckListHeader == compositeChecklistDTO.ParentChecklistheaderId).ToList().Select(z => z.Id)
                                .Contains(y.IdJobChecklistGroup)).ToList();
                    foreach (var item in plitems)
                    {
                        if (item.IsRequiredForTCO != true)
                        {
                            item.IsRequiredForTCO = false;
                        }
                    }
                }

                foreach (var b in jobchecklistgroupIds)
                {
                    CompositeChecklistDetail ParentcompositeChecklistDetail = new CompositeChecklistDetail();

                    ParentcompositeChecklistDetail.IdCompositeChecklist = compositeChecklist.Id;
                    ParentcompositeChecklistDetail.IdJobChecklistHeader = compositeChecklistDTO.ParentChecklistheaderId;

                    ParentcompositeChecklistDetail.IsParentCheckList = true;
                    ParentcompositeChecklistDetail.CreatedDate = DateTime.UtcNow;
                    ParentcompositeChecklistDetail.LastModifiedDate = DateTime.UtcNow;
                    if (employee != null)
                    {
                        compositeChecklist.CreatedBy = employee.Id;
                    }

                    ParentcompositeChecklistDetail.IdJobChecklistGroup = b;
                    lstCompositeChecklistDetail.Add(ParentcompositeChecklistDetail);
                }
                // int i = 0;
                db.CompositeChecklistDetails.AddRange(lstCompositeChecklistDetail);
                foreach (var a in compositeChecklistDTO.ChecklistheaderIds)
                {
                    var jobchecklistgroupIds1 = db.JobChecklistGroups.Where(x => x.IdJobCheckListHeader == a).Select(y => y.Id).ToList();
                    foreach (var b in jobchecklistgroupIds1)
                    {
                        CompositeChecklistDetail compositeChecklistDetail = new CompositeChecklistDetail();
                        compositeChecklistDetail.IdCompositeChecklist = compositeChecklist.Id;
                        compositeChecklistDetail.IdJobChecklistHeader = a;
                        compositeChecklistDetail.IsParentCheckList = false; 
                        compositeChecklistDetail.CreatedDate = DateTime.UtcNow;
                        compositeChecklistDetail.LastModifiedDate = DateTime.UtcNow;
                        if (employee != null)
                        {
                            compositeChecklist.CreatedBy = employee.Id;
                        }
                      
                        compositeChecklistDetail.IdJobChecklistGroup = b;
                        lstCompositeChecklistDetail.Add(compositeChecklistDetail);
                    }
                   if(compositeChecklistDTO.IsCOProject==true) //If CO Project selected for composite checklist
                    {                       
                        var items = db.JobChecklistItemDetails.Where(y => db.JobChecklistGroups.Where(x => x.IdJobCheckListHeader == compositeChecklistDTO.ParentChecklistheaderId).ToList().Select(z => z.Id)
                      .Contains(y.IdJobChecklistGroup)).ToList();
                        foreach (var item in items)
                        {
                            if (item.IsRequiredForTCO != true)
                            {
                                item.IsRequiredForTCO = false;
                            }
                        }                       
                        var plitems = db.JobPlumbingInspection.Where(y => db.JobChecklistGroups.Where(x => x.IdJobCheckListHeader == compositeChecklistDTO.ParentChecklistheaderId).ToList().Select(z => z.Id)
                                      .Contains(y.IdJobChecklistGroup)).ToList();
                        foreach (var item in plitems)
                        {
                            if (item.IsRequiredForTCO != true)
                            {
                                item.IsRequiredForTCO = false;
                            }
                        }
                    }                   
                }
                db.CompositeChecklistDetails.AddRange(lstCompositeChecklistDetail);

                db.SaveChanges();
                //  CompositeViolationDTO compositeViolationDTO = new CompositeViolationDTO();


                int Idrfpaddress = db.Jobs.Find(compositeChecklistDTO.ParentJobId).IdRfpAddress;
                string BinNumber = db.RfpAddresses.Where(x => x.Id == Idrfpaddress).Select(y => y.BinNumber).FirstOrDefault();
                var Violations = db.JobViolations.Where(x => x.BinNumber == BinNumber).ToList();

                List<CompositeViolations> lstcompositeViolation = new List<CompositeViolations>();
                foreach (var v in Violations)
                {
                    CompositeViolations compositeViolations = new CompositeViolations();
                    //compositeViolationDTO.IdCompositeChecklist = compositeChecklist.Id;
                    //compositeViolationDTO.IdJobViolations = v.Id;
                    //compositeViolationDTO.JobViolation = v;
                    compositeViolations.IdCompositeChecklist = compositeChecklist.Id;
                    compositeViolations.IdJobViolations = v.Id;
                    compositeViolations.JobViolation = v;
                    if (compositeChecklistDTO.IsCOProject == true)
                    {
                        if (v.TCOToggle != true)
                            v.TCOToggle = false;
                    }
                    db.Entry(v).State = EntityState.Modified;
                    db.SaveChanges();

                    lstcompositeViolation.Add(compositeViolations);
                }
                db.CompositeViolations.AddRange(lstcompositeViolation);
                db.SaveChanges();


                return Ok("Composite Checklist created successfully");
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
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
        [Authorize]
        [RpoAuthorize]
        [HttpDelete]
        [Route("api/DeleteCompositeCheckList/{IdCompositeChecklist}")]
        public IHttpActionResult CompositeCheckList(int IdCompositeChecklist)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteChecklist))
            {
                var IsCompositecheckListdetailsxists = db.CompositeChecklistDetails.Count(b => b.IdCompositeChecklist == IdCompositeChecklist);
                CompositeChecklist compositeChecklist = db.CompositeChecklists.Find(IdCompositeChecklist);
                var result = db.CompositeChecklistDetails.Where(b => b.IdCompositeChecklist == IdCompositeChecklist);

                foreach (var b in result)
                {
                    db.CompositeChecklistDetails.Remove(b);
                }

                if (compositeChecklist != null)
                    db.CompositeChecklists.Remove(compositeChecklist);

                try
                {
                    db.SaveChanges();
                    return Ok("Composite Checklist Deleted Successfully");
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;

                }
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        [ResponseType(typeof(List<CompositeChecklistDetailDTO>))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/GetCompositeChecklist/{IdCompositeChecklist}")]
        public IHttpActionResult GetCompositeChecklist(int IdCompositeChecklist)
        {
            var result = db.CompositeChecklistDetails.Include("JobChecklistHeaders").Where(x => x.IdCompositeChecklist == IdCompositeChecklist);
            List<CompositeChecklistDetailDTO> lstcompositeChecklistDetailDTO = new List<CompositeChecklistDetailDTO>();
            foreach (var a in result)
            {
                CompositeChecklistDetailDTO compositeChecklistDetailDTO = new CompositeChecklistDetailDTO();
                compositeChecklistDetailDTO.IdCompositeChecklist = a.IdCompositeChecklist;
                compositeChecklistDetailDTO.CompositeChecklistName = a.CompositeChecklists.Name.Remove(0, 5);
                compositeChecklistDetailDTO.IdJobChecklistHeader = a.IdJobChecklistHeader;
                compositeChecklistDetailDTO.IdJob = a.JobChecklistHeaders.IdJob;
                compositeChecklistDetailDTO.ChecklistName = a.JobChecklistHeaders.ChecklistName;
                compositeChecklistDetailDTO.IsParentCheckList = a.IsParentCheckList;
                compositeChecklistDetailDTO.IsCOProject = db.CompositeChecklists.Where(x => x.Id == IdCompositeChecklist).Select(y => y.IsCOProject).FirstOrDefault();
                lstcompositeChecklistDetailDTO.Add(compositeChecklistDetailDTO);
            }
            var b = lstcompositeChecklistDetailDTO.GroupBy(x => x.IdJobChecklistHeader).ToList();
            var temp = b.Select(c => new
            {
                IdJobchecklist = c.Key,
                IsParentCheckList = c.FirstOrDefault().IsParentCheckList,
                IsCOProject = c.FirstOrDefault().IsCOProject
            }).ToArray();

            return Ok(temp);

        }


        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CompositeChecklistDTO))]
        [Route("api/Checklist/PostEditCompositeChecklist/{IdCompositeChecklist}/{IsCOProject}")]
        public IHttpActionResult PostEditCompositeChecklist([FromBody]List<int> compositeChecklistheaderids, int IdCompositeChecklist, bool IsCOProject)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                var result = db.CompositeChecklistDetails.Where(x => x.IdCompositeChecklist == IdCompositeChecklist && x.IsParentCheckList != true).Select(x => x.IdJobChecklistHeader).ToList();
                CompositeChecklist com = db.CompositeChecklists.Find(IdCompositeChecklist);
                com.IsCOProject = IsCOProject;

                foreach (var a in compositeChecklistheaderids)
                {

                    if (!result.Contains(a))
                    {
                        List<CompositeChecklistDetail> lstCompositeChecklistDetail = new List<CompositeChecklistDetail>();                     
                        var jobchecklistgroupIds = db.JobChecklistGroups.Where(x => x.IdJobCheckListHeader == a).Select(y => y.Id); ;

                        foreach (var b in jobchecklistgroupIds)
                        {
                            CompositeChecklistDetail compositeChecklistDetail = new CompositeChecklistDetail();
                            compositeChecklistDetail.IdCompositeChecklist = IdCompositeChecklist;
                            compositeChecklistDetail.IdJobChecklistHeader = a;
                            compositeChecklistDetail.IdJobChecklistGroup = b;
                            compositeChecklistDetail.CreatedDate = DateTime.UtcNow;
                            compositeChecklistDetail.LastModifiedDate = DateTime.UtcNow;
                            compositeChecklistDetail.IsParentCheckList = false;                     
                            if (employee != null)
                            {

                                com.LastModifiedBy = employee.Id;
                                compositeChecklistDetail.CreatedBy = employee.Id;
                            }
                            lstCompositeChecklistDetail.Add(compositeChecklistDetail);
                        }
                        db.CompositeChecklistDetails.AddRange(lstCompositeChecklistDetail);
                        db.SaveChanges();
                    }

                }
                foreach (var a in compositeChecklistheaderids)
                {
                    if (IsCOProject == true)                    {
                        
                        var items = db.JobChecklistItemDetails.Where(y => db.JobChecklistGroups.Where(x => x.IdJobCheckListHeader == a).ToList().Select(z => z.Id)
                     .Contains(y.IdJobChecklistGroup)).ToList();
                        foreach (var item in items)
                        {
                            if (item.IsRequiredForTCO != true)
                            {
                                item.IsRequiredForTCO = false;
                            }
                        }

                       
                        var plitems = db.JobPlumbingInspection.Where(y => db.JobChecklistGroups.Where(x => x.IdJobCheckListHeader == a).ToList().Select(z => z.Id)
                                      .Contains(y.IdJobChecklistGroup)).ToList();
                        foreach (var item in plitems)
                        {
                            if (item.IsRequiredForTCO != true)
                            {
                                item.IsRequiredForTCO = false;
                            }
                        }
                    }
                }
                foreach (var b in result)
                {
                    if (!compositeChecklistheaderids.Contains(b))
                    {
                        var removecompositeitems = db.CompositeChecklistDetails.Where(x => x.IdJobChecklistHeader == b && x.IdCompositeChecklist == IdCompositeChecklist).ToList();
                        db.CompositeChecklistDetails.RemoveRange(removecompositeitems);
                    }
                }

                var result1 = db.CompositeChecklistDetails.Where(x => x.IdCompositeChecklist == IdCompositeChecklist && x.IsParentCheckList == true).Select(x => x.IdJobChecklistHeader).FirstOrDefault();
                if (IsCOProject == true)
                {
                    var items = db.JobChecklistItemDetails.Where(y => db.JobChecklistGroups.Where(x => x.IdJobCheckListHeader == result1).ToList().Select(z => z.Id)
                    .Contains(y.IdJobChecklistGroup)).ToList();
                    foreach (var item in items)
                    {
                        if (item.IsRequiredForTCO != true)
                        {
                            item.IsRequiredForTCO = false;
                        }
                    }
                    var plitems = db.JobPlumbingInspection.Where(y => db.JobChecklistGroups.Where(x => x.IdJobCheckListHeader == result1).ToList().Select(z => z.Id)
                                  .Contains(y.IdJobChecklistGroup)).ToList();
                    foreach (var item in plitems)
                    {
                        if (item.IsRequiredForTCO != true)
                        {
                            item.IsRequiredForTCO = false;
                        }
                    }                  
                }       
                db.SaveChanges();
                if (IsCOProject == true)
                {
                    var violationids = db.CompositeViolations.Where(x => x.IdCompositeChecklist == IdCompositeChecklist).Select(y => y.IdJobViolations).ToList();
                    var Violations = db.JobViolations.Where(x => violationids.Contains(x.Id)).ToList();
                    foreach (var v in Violations)
                    {
                        if (IsCOProject == true)
                        {
                            if (v.TCOToggle != true)
                                v.TCOToggle = false;
                        }
                    }
                    db.SaveChanges();
                }
                return Ok("Composite Checklist Updated successfully");
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        [ResponseType(typeof(ForTCO_DTO))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/compositeChecklist/PutRequiredforTCO/{IdJobchecklistitemdetail}")]
        public IHttpActionResult PutRequiredforTCO(ForTCO_DTO forTCO)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                if (forTCO.isplumbingitem == false)
                {
                    JobChecklistItemDetail jobChecklistItemDetail1 = db.JobChecklistItemDetails.Find(forTCO.IdJobchecklistitemdetail);
                    if (jobChecklistItemDetail1 != null)
                    {
                        jobChecklistItemDetail1.IsRequiredForTCO = forTCO.IsRequiredForTCO;
                        jobChecklistItemDetail1.LastModifiedDate = DateTime.UtcNow;
                        db.SaveChanges();
                        return Ok("Updated Successfully");
                    }
                    return this.NotFound();

                }
                else
                {
                    JobPlumbingInspection jobPlumbingInspection = db.JobPlumbingInspection.Find(forTCO.IdJobchecklistitemdetail);
                    if (jobPlumbingInspection != null)
                    {
                        jobPlumbingInspection.IsRequiredForTCO = forTCO.IsRequiredForTCO;
                        jobPlumbingInspection.LastModifiedDate = DateTime.UtcNow;
                        db.SaveChanges();
                    }
                    return Ok("Updated Successfully");                   
                }
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        [ResponseType(typeof(ForTCO_DTO))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/compositeChecklist/PutRequiredforViolationTCO")]
        public IHttpActionResult PutRequiredforTCOViolation(ForViolationTCO_DTO forTCO)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                JobViolation JobViolation = db.JobViolations.Find(forTCO.IdJobViolation);
                if (JobViolation != null)
                    JobViolation.TCOToggle = forTCO.IsRequiredForTCO;
                db.SaveChanges();
                return Ok("TCO toggle Updated Successfully");

            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        [ResponseType(typeof(ForTCO_DTO))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/compositeChecklist/PutStatuForViolation")]
        public IHttpActionResult PutStatuForViolation(ForViolationStatus_DTO ForStatus)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                JobViolation JobViolation = db.JobViolations.Find(ForStatus.IdJobViolation);
                if (JobViolation != null)
                    JobViolation.Status = ForStatus.Status;
                db.SaveChanges();
                return Ok("Violation status Updated Successfully");

            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        [ResponseType(typeof(List<CompositeChecklistDetailDTO>))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/GetCompositeChecklistDetails/{IdCompositeChecklist}")]
        public IHttpActionResult GetCompositeChecklistDetails(int IdCompositeChecklist)
        {
            var result = db.CompositeChecklistDetails.Include("JobChecklistHeaders").Where(x => x.IdCompositeChecklist == IdCompositeChecklist);
            List<CompositeChecklistDetailDTO> lstcompositeChecklistDetailDTO = new List<CompositeChecklistDetailDTO>();
            foreach (var a in result)
            {
                CompositeChecklistDetailDTO compositeChecklistDetailDTO = new CompositeChecklistDetailDTO();
                compositeChecklistDetailDTO.IdCompositeChecklist = a.IdCompositeChecklist;
                compositeChecklistDetailDTO.CompositeChecklistName = a.CompositeChecklists.Name;
                compositeChecklistDetailDTO.IdJobChecklistHeader = a.IdJobChecklistHeader;
                compositeChecklistDetailDTO.ChecklistName = a.JobChecklistHeaders.ChecklistName;
                compositeChecklistDetailDTO.IdJobChecklistItemDetail = a.IdJobChecklistGroup;
                compositeChecklistDetailDTO.JobChecklistHeaders = a.JobChecklistHeaders;
                lstcompositeChecklistDetailDTO.Add(compositeChecklistDetailDTO);
            }           
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();

                DataSet ds = new DataSet();
                SqlParameter[] spParameter = new SqlParameter[2];

                spParameter[0] = new SqlParameter("@IDs", SqlDbType.NVarChar);
                spParameter[0].Value = 2160;
                spParameter[1] = new SqlParameter("@OrderFlag", SqlDbType.VarChar);
                spParameter[1].Value = "Group";


                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "CompositeChecklistView", spParameter);

                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string permitCode = string.Empty;
                        string permitdescription = string.Empty;
                        int IdJobApplication = Convert.ToInt32(ds.Tables[0].Rows[i]["IdJobApplication"]);
                        int IdJob = Convert.ToInt32(ds.Tables[0].Rows[i]["IdJob"]);
                        if (IdJobApplication != 0 && IdJob != 0)
                        {
                            var jobApplicationWorkPermitTypes = db.JobApplicationWorkPermitTypes.Include("JobApplication.JobApplicationType")
                                .Include("JobWorkType").Include("CreatedByEmployee").Include("LastModifiedByEmployee").Include("ContactResponsible").Where(c => c.IdJobApplication == IdJobApplication && c.JobApplication.IdJob == IdJob).AsQueryable();


                            foreach (var item in jobApplicationWorkPermitTypes)
                            {
                                if (item.Code != null)
                                {
                                    permitCode += ", " + item.Code;
                                }
                                if (item.Code == "PL")
                                {
                                    permitdescription = item.WorkDescription != null ? item.WorkDescription : null;
                                }
                            }
                        }
                    }

                }
                return Ok(lstcompositeChecklistDetailDTO);               

            }
            catch (DbUpdateConcurrencyException)
            {
                throw;

            }


        }


        /// <summary>
        /// View the Composite CheckList.
        /// </summary>
        /// <param name="id">The identifier.</param>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/CompositeCheckList/TCOViewCompositeCheckList/{IdCompositeChecklist}/{OrderFlag}/{TcoFlag}")]
        public IHttpActionResult TCOViewCompositeCheckList(string IdCompositeChecklist, string OrderFlag, bool TcoFlag)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewChecklist)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteChecklist))
            {
                DataSet ds = new DataSet();
                DataSet ds2 = new DataSet();
                SqlParameter[] spParameter = new SqlParameter[3];

                spParameter[0] = new SqlParameter("@IdCompositeChecklist", SqlDbType.NVarChar);
                spParameter[0].Value = IdCompositeChecklist;
                spParameter[1] = new SqlParameter("@OrderFlag", SqlDbType.VarChar);
                spParameter[1].Value = OrderFlag;
                spParameter[2] = new SqlParameter("@TCOFlag", SqlDbType.Bit);
                spParameter[2].Value = TcoFlag;

                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "TCOCompositeView", spParameter);
                ds2 = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "TCOCompositeViewItem", spParameter);
                List<CompositeChecklistGroupHeader> headerlist = new List<CompositeChecklistGroupHeader>();

                List<DataTable> distinctheaders = ds.Tables[0].AsEnumerable()
                        .GroupBy(row => new
                        {
                            JobChecklistHeaderId = row.Field<int>("JobChecklistHeaderId"),
                        }).Select(g => g.CopyToDataTable()).ToList();
               
                try
                {
                    foreach (var loopheader in distinctheaders)
                    {
                        CompositeChecklistGroupHeader header = new CompositeChecklistGroupHeader();
                        Int32 checklistHeaderId = Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                        header.IdCompositeChecklist = loopheader.Rows[0]["IdCompositeChecklist"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["IdCompositeChecklist"]);
                        header.CompositeChecklistName = loopheader.Rows[0]["CheckListName"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["CheckListName"].ToString();
                        header.jobChecklistHeaderId = loopheader.Rows[0]["JobChecklistHeaderId"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                        header.IdJob = loopheader.Rows[0]["IdJob"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["IdJob"]);
                        header.CompositeOrder = loopheader.Rows[0]["CompositeOrder"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["CompositeOrder"]);
                        header.IsCOProject = loopheader.Rows[0]["IsCOProject"] == DBNull.Value ? (bool)false : Convert.ToBoolean(loopheader.Rows[0]["IsCOProject"]);
                        header.IsParentCheckList = loopheader.Rows[0]["IsParentCheckList"] == DBNull.Value ? (bool)false : Convert.ToBoolean(loopheader.Rows[0]["IsParentCheckList"]);

                        header.groups = new List<CompositeChecklistGroup>();

                        var distinctGroup = ds.Tables[0].AsEnumerable().Where(i => i.Field<int>("JobChecklistHeaderId") == checklistHeaderId)
                            .GroupBy(r => new { group = r.Field<int>("IdJobChecklistGroup") }).Select(g => g.CopyToDataTable()).ToList();                      
                        foreach (var eachGroup in distinctGroup)
                        {

                            CompositeChecklistGroup group = new CompositeChecklistGroup();
                            group.item = new List<Item>();
                            //if (Convert.ToInt32(ds.Tables[0].Rows[i]["JobChecklistHeaderId"]) == checklistHeaderId)
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
                                            item.CompositeName = groupsitems[j]["CompositeName"] == DBNull.Value ? string.Empty : groupsitems[j]["CompositeName"].ToString();
                                            item.IdCompositeChecklist = groupsitems[j]["IdCompositeChecklist"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdCompositeChecklist"]);
                                            item.IsRequiredForTCO = groupsitems[j]["IsRequiredForTCO"] == DBNull.Value ? (bool)false : Convert.ToBoolean(groupsitems[j]["IsRequiredForTCO"]);
                                            item.PartyResponsible = groupsitems[j]["PartyResponsible"] == DBNull.Value ? string.Empty : groupsitems[j]["PartyResponsible"].ToString();
                                            //  item.IsCOProject = groupsitems[j]["IsCOProject"] == DBNull.Value ? (bool)false : Convert.ToBoolean(groupsitems[j]["IsCOProject"]);
                                            //item.IsParentCheckList = groupsitems[j]["IsParentCheckList"] == DBNull.Value ? (bool)false : Convert.ToBoolean(groupsitems[j]["IsParentCheckList"]);

                                            #endregion
                                            group.item.Add(item);
                                        }
                                    }
                                }
                                else
                                {
                                    var groupsitems2 = ds.Tables[0].AsEnumerable().Where(l => l.Field<int>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();
                                    if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                                    {
                                        for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                                        {
                                            Item item = new Item();
                                            item.Details = new List<Details>();

                                            #region Items
                                            Int32 IdChecklistItem = ds2.Tables[0].Rows[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(ds2.Tables[0].Rows[j]["IdChecklistItem"]);
                                            if (IdChecklistItem != 0)
                                            {
                                                item.checklistItemName = ds2.Tables[0].Rows[j]["checklistItemName"] == DBNull.Value ? string.Empty : ds2.Tables[0].Rows[j]["checklistItemName"].ToString();
                                                item.IdChecklistItem = ds2.Tables[0].Rows[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(ds2.Tables[0].Rows[j]["IdChecklistItem"]);
                                                //item.CompositeName = groupsitems[j]["CompositeName"] == DBNull.Value ? string.Empty : groupsitems[j]["CompositeName"].ToString();
                                                //item.IdCompositeDetail = groupsitems[j]["IdCompositeDetail"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdCompositeDetail"]);
                                                //item.Displayorder = groupsitems2[j]["Displayorder"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems2[j]["Displayorder"]);
                                                //item.CompositeName = groupsitems2[j]["CompositeName"] == DBNull.Value ? string.Empty : groupsitems2[j]["CompositeName"].ToString();
                                                //item.IdCompositeDetail = groupsitems2[j]["IdCompositeDetail"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems2[j]["IdCompositeDetail"]);

                                                #endregion

                                                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                                                {
                                                    Int32 IdChecklistItem2 = ds.Tables[0].Rows[k]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[0].Rows[k]["IdChecklistItem"]);
                                                    if (IdChecklistItem2 == IdChecklistItem && Convert.ToInt32(ds.Tables[0].Rows[k]["IdJobChecklistGroup"]) == IdJobChecklistGroup && Convert.ToInt32(ds.Tables[0].Rows[k]["JobChecklistHeaderId"]) == checklistHeaderId)
                                                    {

                                                        Details detail = new Details();

                                                        #region Items details
                                                        detail.checklistGroupType = ds.Tables[0].Rows[k]["checklistGroupType"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[k]["checklistGroupType"].ToString();
                                                        detail.idJobPlumbingInspection = ds.Tables[0].Rows[k]["IdJobPlumbingInspection"] == DBNull.Value ? (int?)null : Convert.ToInt32(ds.Tables[0].Rows[k]["IdJobPlumbingInspection"]);
                                                        detail.idJobPlumbingCheckListFloors = ds.Tables[0].Rows[k]["IdJobPlumbingCheckListFloors"] == DBNull.Value ? (int?)null : Convert.ToInt32(ds.Tables[0].Rows[k]["IdJobPlumbingCheckListFloors"]);
                                                        detail.inspectionPermit = ds.Tables[0].Rows[k]["checklistItemName"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[k]["checklistItemName"].ToString();
                                                        detail.floorName = ds.Tables[0].Rows[k]["FloorName"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[k]["FloorName"].ToString();
                                                        detail.workOrderNumber = ds.Tables[0].Rows[k]["WorkOrderNo"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[k]["WorkOrderNo"].ToString();
                                                        detail.plComments = ds.Tables[0].Rows[k]["Comments"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[k]["Comments"].ToString();
                                                        detail.DueDate = ds.Tables[0].Rows[k]["DueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(ds.Tables[0].Rows[k]["DueDate"]);
                                                        detail.nextInspection = ds.Tables[0].Rows[k]["NextInspection"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(ds.Tables[0].Rows[k]["NextInspection"]);
                                                        detail.result = ds.Tables[0].Rows[k]["Result"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[k]["Result"].ToString();
                                                        //detail.PlumbingInspectionDisplayOrder = ds.Tables[0].Rows[k]["IdJobPlumbingCheckListFloors"] == DBNull.Value ? (int?)null : Convert.ToInt32(ds.Tables[0].Rows[k]["IdJobPlumbingCheckListFloors"]);
                                                        detail.IsRequiredForTCO = ds.Tables[0].Rows[k]["IsRequiredForTCO"] == DBNull.Value ? (bool)false : Convert.ToBoolean(ds.Tables[0].Rows[k]["IsRequiredForTCO"]);
                                                        //  detail.IsParentCheckList = ds.Tables[0].Rows[k]["IsParentCheckList"] == DBNull.Value ? (bool)false : Convert.ToBoolean(ds.Tables[0].Rows[k]["IsParentCheckList"]);

                                                        #endregion

                                                        item.Details.Add(detail);
                                                    }
                                                }
                                            }

                                            group.item.Add(item);
                                        }
                                    }
                                }
                            }

                            header.groups.Add(group);
                        }

                        headerlist.Add(header);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return this.NotFound();
                }
                return Ok(headerlist);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }

        }

        /// <summary>
        /// View the Composite CheckList.
        /// </summary>
        /// <param name="id">The identifier.</param>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/CompositeCheckList/ViewCompositeCheckList")]
        public IHttpActionResult ViewCompositeCheckList(CompositeChecklistSearch compositeChecklistSearch)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewChecklist)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteChecklist))
            {
                DataSet ds = new DataSet();
                SqlParameter[] spParameter = new SqlParameter[3];

                spParameter[0] = new SqlParameter("@IdCompositeChecklist", SqlDbType.NVarChar);
                spParameter[0].Direction = ParameterDirection.Input;
                spParameter[0].Value = compositeChecklistSearch.IdCompositeChecklist;

                spParameter[1] = new SqlParameter("@OrderFlag", SqlDbType.VarChar);
                spParameter[1].Direction = ParameterDirection.Input;
                spParameter[1].Value = compositeChecklistSearch.OrderFlag;

                spParameter[2] = new SqlParameter("@SearchText", SqlDbType.VarChar);
                spParameter[2].Direction = ParameterDirection.Input;
                spParameter[2].Value = !string.IsNullOrEmpty(compositeChecklistSearch.SearchText) ? compositeChecklistSearch.SearchText : string.Empty;

                int Idcompo = Convert.ToInt32(compositeChecklistSearch.IdCompositeChecklist);

                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "CompositeView", spParameter);
                List<CompositeChecklistGroupHeader> headerlist = new List<CompositeChecklistGroupHeader>();

                List<DataTable> distinctheaders = ds.Tables[0].AsEnumerable()
                        .GroupBy(row => new
                        {
                            JobChecklistHeaderId = row.Field<int>("JobChecklistHeaderId"),
                        }).Select(g => g.CopyToDataTable()).ToList();
               
                try                   

                {
                    foreach (var loopheader in distinctheaders)
                    {
                        CompositeChecklistGroupHeader header = new CompositeChecklistGroupHeader();
                        Int32 checklistHeaderId = Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                        header.IdCompositeChecklist = loopheader.Rows[0]["IdCompositeChecklist"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["IdCompositeChecklist"]);                       
                        header.CompositeChecklistName = loopheader.Rows[0]["CheckListName"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["CheckListName"].ToString();
                        header.jobChecklistHeaderId = loopheader.Rows[0]["JobChecklistHeaderId"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                        header.Others = loopheader.Rows[0]["Others"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["Others"].ToString();
                        header.IdJob = loopheader.Rows[0]["IdJob"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["IdJob"]);                       
                        header.IsCOProject = loopheader.Rows[0]["IsCOProject"] == DBNull.Value ? (bool)false : Convert.ToBoolean(loopheader.Rows[0]["IsCOProject"]);                        
                        header.IsParentCheckList= db.CompositeChecklistDetails.Where(x => x.IdJobChecklistHeader == header.jobChecklistHeaderId && x.IdCompositeChecklist == Idcompo).Select(y => y.IsParentCheckList).FirstOrDefault();
                        header.CompositeOrder = db.CompositeChecklistDetails.Where(x => x.IdJobChecklistHeader == header.jobChecklistHeaderId && x.IdCompositeChecklist == Idcompo).Select(y => y.CompositeOrder).FirstOrDefault();
                        header.groups = new List<CompositeChecklistGroup>();
                        string s = null;
                        int id = checklistHeaderId;                        
                        var temp = db.JobChecklistHeaders.Include("JobApplicationWorkPermitTypes").Where(x => x.IdJobCheckListHeader == id).ToList();
                        var lstidjobworktype = temp[0].JobApplicationWorkPermitTypes.Where(x => x.Code.ToLower().Trim() == "pl" || x.Code.ToLower().Trim() == "sp" || x.Code.ToLower().Trim() == "sd").Select(y => y.IdJobWorkType).ToList();
                        foreach (var i in lstidjobworktype)
                        {
                            s += i.ToString() + ",";
                        }
                        if (s != null)
                            header.IdWorkPermits = s.Remove(s.Length - 1, 1);
                        if (s != null)
                            header.IdWorkPermits = s.Remove(s.Length - 1, 1);

                        var distinctGroup = ds.Tables[0].AsEnumerable().Where(i => i.Field<int>("JobChecklistHeaderId") == checklistHeaderId)
                            .GroupBy(r => new { group = r.Field<int>("IdJobChecklistGroup") }).Select(g => g.CopyToDataTable()).ToList();
                       
                        foreach (var eachGroup in distinctGroup)
                        {

                            CompositeChecklistGroup group = new CompositeChecklistGroup();
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
                                            item.CompositeName = groupsitems[j]["CompositeName"] == DBNull.Value ? string.Empty : groupsitems[j]["CompositeName"].ToString();
                                            item.IdCompositeChecklist = groupsitems[j]["IdCompositeChecklist"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdCompositeChecklist"]);
                                            item.IsRequiredForTCO = groupsitems[j]["IsRequiredForTCO"] == DBNull.Value ? (bool)false : Convert.ToBoolean(groupsitems[j]["IsRequiredForTCO"]);
                                            item.PartyResponsible = groupsitems[j]["PartyResponsible"] == DBNull.Value ? string.Empty : groupsitems[j]["PartyResponsible"].ToString();
                                            item.HasDocument = item.HasDocument = db.JobDocuments.Any(x => x.IdJobchecklistItemDetails == item.jocbChecklistItemDetailsId);
                                           
                                            item.IsParentCheckList = groupsitems[j]["IsParentCheckList"] == DBNull.Value ? (bool)false : Convert.ToBoolean(groupsitems[j]["IsParentCheckList"]);
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
                                            detail.IsRequiredForTCO = detailItem[i]["IsRequiredForTCO"] == DBNull.Value ? (bool)false : Convert.ToBoolean(detailItem[i]["IsRequiredForTCO"]);
                                            detail.HasDocument = db.JobDocuments.Any(x => x.IdJobPlumbingInspections == detail.idJobPlumbingInspection);
                                            item.Details.Add(detail);
                                            #endregion

                                        }
                                        group.item.Add(item);
                                    }
                                }
                            }

                            header.groups.Add(group);
                        }

                        headerlist.Add(header);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return this.NotFound();
                }
                return Ok(headerlist);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }

        }

        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/CompositeCheckList/ViewCustomerCompositeCheckList")]
        public IHttpActionResult ViewCustomerCompositeCheckList(CompositeChecklistSearch compositeChecklistSearch)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();
          
                var employee = this.db.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
           
       
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewChecklist)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteChecklist))
            {
                DataSet ds = new DataSet();
                SqlParameter[] spParameter = new SqlParameter[3];

                spParameter[0] = new SqlParameter("@IdCompositeChecklist", SqlDbType.NVarChar);
                spParameter[0].Direction = ParameterDirection.Input;
                spParameter[0].Value = compositeChecklistSearch.IdCompositeChecklist;

                spParameter[1] = new SqlParameter("@OrderFlag", SqlDbType.VarChar);
                spParameter[1].Direction = ParameterDirection.Input;
                spParameter[1].Value = compositeChecklistSearch.OrderFlag;

                spParameter[2] = new SqlParameter("@SearchText", SqlDbType.VarChar);
                spParameter[2].Direction = ParameterDirection.Input;
                spParameter[2].Value = !string.IsNullOrEmpty(compositeChecklistSearch.SearchText) ? compositeChecklistSearch.SearchText : string.Empty;

                int Idcompo = Convert.ToInt32(compositeChecklistSearch.IdCompositeChecklist);

                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "CompositeView", spParameter);
                List<CompositeChecklistGroupHeader> headerlist = new List<CompositeChecklistGroupHeader>();

                List<DataTable> distinctheaders = ds.Tables[0].AsEnumerable()
                        .GroupBy(row => new
                        {
                            JobChecklistHeaderId = row.Field<int>("JobChecklistHeaderId"),
                        }).Select(g => g.CopyToDataTable()).ToList();               
                try

                {
                    foreach (var loopheader in distinctheaders)
                    {
                        CompositeChecklistGroupHeader header = new CompositeChecklistGroupHeader();
                        Int32 checklistHeaderId = Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                        header.IdCompositeChecklist = loopheader.Rows[0]["IdCompositeChecklist"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["IdCompositeChecklist"]);                       
                        header.CompositeChecklistName = loopheader.Rows[0]["CheckListName"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["CheckListName"].ToString();
                        header.jobChecklistHeaderId = loopheader.Rows[0]["JobChecklistHeaderId"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                        header.Others = loopheader.Rows[0]["Others"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["Others"].ToString();
                        header.IdJob = loopheader.Rows[0]["IdJob"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["IdJob"]);                        
                        header.IsCOProject = loopheader.Rows[0]["IsCOProject"] == DBNull.Value ? (bool)false : Convert.ToBoolean(loopheader.Rows[0]["IsCOProject"]);                        
                        header.IsParentCheckList = db.CompositeChecklistDetails.Where(x => x.IdJobChecklistHeader == header.jobChecklistHeaderId && x.IdCompositeChecklist == Idcompo).Select(y => y.IsParentCheckList).FirstOrDefault();
                        header.CompositeOrder = db.CompositeChecklistDetails.Where(x => x.IdJobChecklistHeader == header.jobChecklistHeaderId && x.IdCompositeChecklist == Idcompo).Select(y => y.CompositeOrder).FirstOrDefault();
                        header.groups = new List<CompositeChecklistGroup>();
                        
                        string s = null;
                        int id = checklistHeaderId;                        
                        var temp = db.JobChecklistHeaders.Include("JobApplicationWorkPermitTypes").Where(x => x.IdJobCheckListHeader == id).ToList();
                        var lstidjobworktype = temp[0].JobApplicationWorkPermitTypes.Where(x => x.Code.ToLower().Trim() == "pl" || x.Code.ToLower().Trim() == "sp" || x.Code.ToLower().Trim() == "sd").Select(y => y.IdJobWorkType).ToList();
                        foreach (var i in lstidjobworktype)
                        {
                            s += i.ToString() + ",";
                        }
                        if (s != null)
                            header.IdWorkPermits = s.Remove(s.Length - 1, 1);
                        
                        if (s != null)
                            header.IdWorkPermits = s.Remove(s.Length - 1, 1);

                        var distinctGroup = ds.Tables[0].AsEnumerable().Where(i => i.Field<int>("JobChecklistHeaderId") == checklistHeaderId)
                            .GroupBy(r => new { group = r.Field<int>("IdJobChecklistGroup") }).Select(g => g.CopyToDataTable()).ToList();
                        
                        foreach (var eachGroup in distinctGroup)
                        {
                            CompositeChecklistGroup group = new CompositeChecklistGroup();
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
                                        item.CompositeName = groupsitems[j]["CompositeName"] == DBNull.Value ? string.Empty : groupsitems[j]["CompositeName"].ToString();
                                        item.IdCompositeChecklist = groupsitems[j]["IdCompositeChecklist"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdCompositeChecklist"]);
                                        item.IsRequiredForTCO = groupsitems[j]["IsRequiredForTCO"] == DBNull.Value ? (bool)false : Convert.ToBoolean(groupsitems[j]["IsRequiredForTCO"]);
                                        item.PartyResponsible = groupsitems[j]["PartyResponsible"] == DBNull.Value ? string.Empty : groupsitems[j]["PartyResponsible"].ToString();
                                        item.HasDocument = item.HasDocument = db.JobDocuments.Any(x => x.IdJobchecklistItemDetails == item.jocbChecklistItemDetailsId);

                                        item.IsParentCheckList = groupsitems[j]["IsParentCheckList"] == DBNull.Value ? (bool)false : Convert.ToBoolean(groupsitems[j]["IsParentCheckList"]);
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
                                        detail.IsRequiredForTCO = detailItem[i]["IsRequiredForTCO"] == DBNull.Value ? (bool)false : Convert.ToBoolean(detailItem[i]["IsRequiredForTCO"]);
                                        detail.HasDocument = db.JobDocuments.Any(x => x.IdJobPlumbingInspections == detail.idJobPlumbingInspection);
                                        item.Details.Add(detail);
                                        #endregion

                                    }
                                    group.item.Add(item);
                                }
                            }

                            header.groups.Add(group);
                        }

                        headerlist.Add(header);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return this.NotFound();
                }
                return Ok(headerlist);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }

        }

        /// <summary>
        /// Gets the job violations.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the job violations List.</returns>      
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/CompositeCheckList/CompositeViolations/{IdJob}")]
        public IHttpActionResult GetCompositeViolations(int IdJob)
        {
            Job job = db.Jobs.FirstOrDefault(j => j.Id == IdJob);

            if (job == null)
                return this.NotFound();

            List<JobCompositeViolationDTO> jobCompositeViolationList = new List<JobCompositeViolationDTO>();

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            List<int> IdJobs = new List<int>();
            var result = job.Jobs.ToList();
            IdJobs.Add(IdJob);
            foreach (var item in result)
                IdJobs.Add(item.Id);
            //var AdminID = db.Employees.Where(x => x.FirstName.ToLower() == "super" & x.LastName.ToLower() == "admin").FirstOrDefault().Id; //for violations created by admin
            List<string> listBin = new List<string>();
            foreach (var Id in IdJobs)
            {
                var idRfpAddress = this.db.Jobs.Where(x => x.Id == Id).FirstOrDefault().IdRfpAddress;
                var binnumber = this.db.RfpAddresses.Where(x => x.Id == idRfpAddress).Select(y => y.BinNumber).FirstOrDefault();                
                listBin.Add(binnumber);
            }
            foreach (var itemBin in listBin.Distinct())
            {
                //var jobViolations = this.db.JobViolations.Where(x => x.IdJob == Id && x.IsFullyResolved == false && x.Type_ECB_DOB == "ECB" && x.CreatedBy == AdminID).Include("CreatedByEmployee").Include("LastModifiedByEmployee").Include("explanationOfCharges");
               var jobViolations = this.db.JobViolations.Where(x => x.BinNumber == itemBin && x.IsFullyResolved == false && x.Type_ECB_DOB == "ECB").Include("CreatedByEmployee").Include("LastModifiedByEmployee").Include("explanationOfCharges");
               foreach (var jobViolation in jobViolations)
                {
                    JobCompositeViolationDTO jobCompositeViolation = new JobCompositeViolationDTO();
                    jobCompositeViolation.Id = jobViolation.Id;
                    jobCompositeViolation.IdJob = jobViolation.IdJob;
                    jobCompositeViolation.SummonsNumber = jobViolation.SummonsNumber;
                    jobCompositeViolation.BalanceDue = jobViolation.BalanceDue;
                    jobCompositeViolation.DateIssued = jobViolation.DateIssued;
                    jobCompositeViolation.CureDate = jobViolation.CureDate;
                    jobCompositeViolation.RespondentName = jobViolation.RespondentName;
                    jobCompositeViolation.HearingDate = jobViolation.HearingDate;
                    jobCompositeViolation.HearingLocation = jobViolation.HearingLocation;
                    jobCompositeViolation.HearingResult = jobViolation.HearingResult;
                    jobCompositeViolation.InspectionLocation = jobViolation.InspectionLocation;
                    jobCompositeViolation.IssuingAgency = jobViolation.IssuingAgency;
                    jobCompositeViolation.RespondentAddress = jobViolation.RespondentAddress;
                    jobCompositeViolation.StatusOfSummonsNotice = jobViolation.StatusOfSummonsNotice;
                    jobCompositeViolation.ComplianceOn = jobViolation.ComplianceOn;
                    jobCompositeViolation.ResolvedDate = jobViolation.ResolvedDate;
                    jobCompositeViolation.IsFullyResolved = jobViolation.IsFullyResolved;
                    jobCompositeViolation.CertificationStatus = jobViolation.CertificationStatus;
                    jobCompositeViolation.Notes = jobViolation.Notes;
                    jobCompositeViolation.IsCOC = jobViolation.IsCOC;
                    jobCompositeViolation.COCDate = jobViolation.COCDate;
                    jobCompositeViolation.PaneltyAmount = jobViolation.PaneltyAmount;
                    jobCompositeViolation.CreatedBy = jobViolation.CreatedBy;
                    jobCompositeViolation.LastModifiedBy = jobViolation.LastModifiedBy != null ? jobViolation.LastModifiedBy : jobViolation.CreatedBy;
                    jobCompositeViolation.CreatedByEmployeeName = jobViolation.CreatedByEmployee != null ? jobViolation.CreatedByEmployee.FirstName + " " + jobViolation.CreatedByEmployee.LastName : string.Empty;
                    jobCompositeViolation.LastModifiedByEmployeeName = jobViolation.LastModifiedByEmployee != null ? jobViolation.LastModifiedByEmployee.FirstName + " " + jobViolation.LastModifiedByEmployee.LastName : (jobViolation.CreatedByEmployee != null ? jobViolation.CreatedByEmployee.FirstName + " " + jobViolation.CreatedByEmployee.LastName : string.Empty);
                    jobCompositeViolation.CreatedDate = jobViolation.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.CreatedDate;
                    jobCompositeViolation.LastModifiedDate = jobViolation.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (jobViolation.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.CreatedDate);
                    jobCompositeViolation.Code = jobViolation.explanationOfCharges != null ? String.Join(", ", jobViolation.explanationOfCharges.Select(c => c.Code)) : string.Empty;
                    jobCompositeViolation.CodeSection = jobViolation.explanationOfCharges != null ? String.Join(", ", jobViolation.explanationOfCharges.Select(c => c.CodeSection)) : string.Empty;
                    jobCompositeViolation.Description = jobViolation.explanationOfCharges != null ? String.Join(", ", jobViolation.explanationOfCharges.Select(c => c.Description)) : string.Empty;
                    jobCompositeViolation.NotesLastModifiedDate = jobViolation.NotesLastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.NotesLastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.NotesLastModifiedDate;
                    jobCompositeViolation.NotesLastModifiedByEmployeeName = jobViolation.NotesLastModifiedByEmployee != null ? jobViolation.NotesLastModifiedByEmployee.FirstName + " " + jobViolation.NotesLastModifiedByEmployee.LastName : string.Empty;
                    jobCompositeViolationList.Add(jobCompositeViolation);
                }

            }

            return this.Ok(jobCompositeViolationList.Distinct());
        }

        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        //[HttpGet]
        [Route("api/CompositeCheckList/GetDOBCompositeViolations")]   
        public IHttpActionResult GetDOBCompositeViolations(CompositeViolationSearch compositeViolationSearch)
        {
            List<JobCompositeViolationDTO> jobCompositeViolationList = new List<JobCompositeViolationDTO>();

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);            
            List<JobViolation> lstjobviolation = new List<JobViolation>();
            var CompositeViolations = db.CompositeViolations.Where(x => x.IdCompositeChecklist == compositeViolationSearch.IdCompositeChecklist).ToList();
            foreach (var c in CompositeViolations)
            {
                //  var violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations & x.Type_ECB_DOB == "DOB").Include("ChecklistJobViolationComment").FirstOrDefault();
                //   var violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations & x.Type_ECB_DOB == "DOB").FirstOrDefault(); //29-1
                //var AdminID = db.Employees.Where(x => x.FirstName.ToLower() == "super" & x.LastName.ToLower() == "admin").FirstOrDefault().Id; //for violations created by admin
                var violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations && x.Type_ECB_DOB == "DOB").FirstOrDefault(); 
                if (violation != null)
                    lstjobviolation.Add(violation);
            }

            #region search
            List<JobViolation> filteredviolation = new List<JobViolation>();
           
            if (!(compositeViolationSearch.SearchText == null || compositeViolationSearch.SearchText == ""))
            {
                DateTime parsedDateTime;
                DateTime.TryParseExact(compositeViolationSearch.SearchText, "MM/dd/yyyy", new CultureInfo("en-US"),
                                   DateTimeStyles.None, out parsedDateTime);               
                var dateissuedwise = lstjobviolation.Where(x => x.DateIssued == parsedDateTime).ToList();              
                foreach (var a in dateissuedwise)
                {
                    filteredviolation.Add(a);
                }
                var DOB_ECBsummonwise = lstjobviolation.Where(x => x.SummonsNumber.ToLower().Contains(compositeViolationSearch.SearchText.ToLower())).ToList();

                foreach (var a in DOB_ECBsummonwise)
                {
                    filteredviolation.Add(a);
                }
                var relatedecbwise = lstjobviolation.Where(x => !string.IsNullOrEmpty(x.ECBnumber) && x.ECBnumber.ToLower().Contains(compositeViolationSearch.SearchText.ToLower())).ToList();

                foreach (var a in relatedecbwise)
                {
                    filteredviolation.Add(a);
                }

                var descriptionwise = lstjobviolation.Where(x => !string.IsNullOrEmpty(x.ViolationDescription) && x.ViolationDescription.ToLower().Contains(compositeViolationSearch.SearchText.ToLower())).ToList();

                foreach (var a in descriptionwise)
                {
                    filteredviolation.Add(a);
                }
               
                #region Comment search
                var comment = db.ChecklistJobViolationComments
                                 //.Where(c.AccessReason.Length > 0)
                                 .GroupBy(c => c.IdJobViolation)
                                 .Select(grp => new
                                 {
                                     grp.Key,
                                     LastAccess = grp
                                      .OrderByDescending(x => x.LastModifiedDate)
                                      .Select(x => x.Id)
                                      .FirstOrDefault()
                                 }).ToList();
                List<ChecklistJobViolationComment> lstlatcomment = new List<ChecklistJobViolationComment>();
                foreach (var c in comment)
                {
                    lstlatcomment.Add(db.ChecklistJobViolationComments.Where(x => x.Id == c.LastAccess).FirstOrDefault());
                }
                //foreach (var c in lstlatcomment)
                //{

                //}
                var commentwise = lstlatcomment.Where(x => !string.IsNullOrEmpty(x.Description) && x.Description.ToLower().Contains(compositeViolationSearch.SearchText.ToLower())).Select(x => x.IdJobViolation).ToList();
                //var com= db.ChecklistJobViolationComments.GroupBy(x => x.IdJobViolation);
                // com.Max(x => x.LastOrDefault());

                // var commentwise=db.ChecklistJobViolationComments.Where(x => x.Description.ToLower().Contains(compositeViolationSearch.SearchText)).Select(x => x.IdJobViolation).ToList().Distinct();

                List<JobViolation> lstjobviolationcommentwise = new List<JobViolation>();
                foreach (var c in commentwise)
                {
                    var m = lstjobviolation.Where(x => x.Id == c).FirstOrDefault();
                    if (m != null)
                        lstjobviolationcommentwise.Add(m);
                }
                foreach (var a in lstjobviolationcommentwise)
                {
                    filteredviolation.Add(a);
                }
                #endregion

                int partyresponsible = 0;
                if (compositeViolationSearch.SearchText.ToLower().Trim().Contains("rpoteam"))
                {
                    partyresponsible = 1;
                }
                if (compositeViolationSearch.SearchText.ToLower().Contains("other"))
                {
                    partyresponsible = 3;
                }
                var responsblitywise = lstjobviolation.Where(x => x.PartyResponsible != null && x.PartyResponsible == partyresponsible).ToList();
                foreach (var a in responsblitywise)
                {
                    filteredviolation.Add(a);
                }
                int status = 0;
                if (compositeViolationSearch.SearchText.ToLower().Contains("open"))
                {
                    status = 1;
                }
                if (compositeViolationSearch.SearchText.ToLower().Contains("completed"))
                {
                    status = 2;
                }
                var satuswise = lstjobviolation.Where(x => status > 0 && x.Status == status).ToList();
                foreach (var a in satuswise)
                {
                    filteredviolation.Add(a);
                }
            }
            var didtinctIds = filteredviolation.Select(x => x.Id).Distinct();
            filteredviolation = db.JobViolations.Where(x => didtinctIds.Contains(x.Id)).ToList(); //29-1 commented
            //filteredviolation = db.JobViolations.Where(x => didtinctIds.Contains(x.Id) &x.CreatedBy==2).ToList(); //created by admin
            List<JobViolation> finalresut = new List<JobViolation>();
            if (compositeViolationSearch.SearchText == "" || compositeViolationSearch.SearchText == null)
            {
                finalresut.AddRange(lstjobviolation);
            }
            else
            {
                finalresut.AddRange(filteredviolation);
            }
            #endregion
           
            foreach (var jobViolation in finalresut)
            {
                    JobCompositeViolationDTO jobCompositeViolation = new JobCompositeViolationDTO();
                    jobCompositeViolation.Id = jobViolation.Id;
                    jobCompositeViolation.IdJob = jobViolation.IdJob;
                    jobCompositeViolation.SummonsNumber = jobViolation.SummonsNumber;                
                    jobCompositeViolation.DateIssued = jobViolation.DateIssued;               
                     jobCompositeViolation.InspectionLocation = jobViolation.InspectionLocation;
                    jobCompositeViolation.isnViolation = jobViolation.ISNViolation;
                    jobCompositeViolation.DispositionDate = jobViolation.Disposition_Date != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.Disposition_Date), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.Disposition_Date;
                    jobCompositeViolation.DispositionComments = jobViolation.Disposition_Comments;
                    jobCompositeViolation.DeviceNumber = jobViolation.device_number;
                    jobCompositeViolation.ViolationDescription = jobViolation.ViolationDescription;
                    jobCompositeViolation.ECBNumber = jobViolation.ECBnumber;
                    jobCompositeViolation.ViolationNumber = jobViolation.violation_number;
                    jobCompositeViolation.ViolationCategory = jobViolation.violation_category;
                    jobCompositeViolation.BinNumber = jobViolation.BinNumber;
                    jobCompositeViolation.CreatedBy = jobViolation.CreatedBy;
                    jobCompositeViolation.LastModifiedBy = jobViolation.LastModifiedBy != null ? jobViolation.LastModifiedBy : jobViolation.CreatedBy;
                    jobCompositeViolation.CreatedByEmployeeName = jobViolation.CreatedByEmployee != null ? jobViolation.CreatedByEmployee.FirstName + " " + jobViolation.CreatedByEmployee.LastName : string.Empty;
                    jobCompositeViolation.LastModifiedByEmployeeName = jobViolation.LastModifiedByEmployee != null ? jobViolation.LastModifiedByEmployee.FirstName + " " + jobViolation.LastModifiedByEmployee.LastName : (jobViolation.CreatedByEmployee != null ? jobViolation.CreatedByEmployee.FirstName + " " + jobViolation.CreatedByEmployee.LastName : string.Empty);
                    jobCompositeViolation.CreatedDate = jobViolation.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.CreatedDate;
                    jobCompositeViolation.LastModifiedDate = jobViolation.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (jobViolation.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.CreatedDate);
                    jobCompositeViolation.Code = jobViolation.explanationOfCharges != null ? String.Join(", ", jobViolation.explanationOfCharges.Select(c => c.Code)) : string.Empty;
                    jobCompositeViolation.CodeSection = jobViolation.explanationOfCharges != null ? String.Join(", ", jobViolation.explanationOfCharges.Select(c => c.CodeSection)) : string.Empty;
                    jobCompositeViolation.Description = jobViolation.explanationOfCharges != null ? String.Join(", ", jobViolation.explanationOfCharges.Select(c => c.Description)) : string.Empty;
                    jobCompositeViolation.NotesLastModifiedDate = jobViolation.NotesLastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.NotesLastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.NotesLastModifiedDate;
                    jobCompositeViolation.NotesLastModifiedByEmployeeName = jobViolation.NotesLastModifiedByEmployee != null ? jobViolation.NotesLastModifiedByEmployee.FirstName + " " + jobViolation.NotesLastModifiedByEmployee.LastName : string.Empty;
                jobCompositeViolation.PartyResponsible = jobViolation.PartyResponsible;
                jobCompositeViolation.CertificationStatus = jobViolation.CertificationStatus;
                if (jobViolation.PartyResponsible == 3) //3 means other 1 means RPOteam
                    jobCompositeViolation.ManualPartyResponsible = jobViolation.ManualPartyResponsible;
                jobCompositeViolation.Status = jobViolation.Status;               
                jobCompositeViolation.Comment = (from cmt in db.ChecklistJobViolationComments where cmt.IdJobViolation== jobViolation.Id
                                                 orderby cmt.LastModifiedDate descending
                                                 select cmt.Description).FirstOrDefault();
                if (compositeViolationSearch.IsCOProject == true)
                {
                    if (jobViolation.TCOToggle == null)
                    {
                        jobCompositeViolation.TCOToggle = false;
                    }
                    else
                    {
                        jobCompositeViolation.TCOToggle = jobViolation.TCOToggle.Value;
                    }
                }

                jobCompositeViolationList.Add(jobCompositeViolation);
            }

            // }

            return this.Ok(jobCompositeViolationList);
        }


        [Authorize]
        [RpoAuthorize]
        //[HttpGet]
        [HttpPost]
        [Route("api/CompositeCheckList/GetECBCompositeViolations")]
        public IHttpActionResult GetECBCompositeViolations(CompositeViolationSearch compositeViolationSearch)
        {
            List<JobCompositeViolationDTO> jobCompositeViolationList = new List<JobCompositeViolationDTO>();
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);           
            List<JobViolation> lstjobviolation = new List<JobViolation>();
            var CompositeViolations = db.CompositeViolations.Where(x => x.IdCompositeChecklist == compositeViolationSearch.IdCompositeChecklist).ToList();
            //int AdminID = db.Employees.Where(x => x.FirstName.ToLower() == "super" & x.LastName.ToLower() == "admin").FirstOrDefault().Id; //for violations created by admin
            foreach (var c in CompositeViolations)
            {
                var violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations && x.Type_ECB_DOB == "ECB").FirstOrDefault();
                if (violation != null)
                    lstjobviolation.Add(violation);
            }

            #region search
            List<JobViolation> filteredviolation = new List<JobViolation>();
            if (!(compositeViolationSearch.SearchText == null || compositeViolationSearch.SearchText == ""))
            {
                DateTime parsedDateTime;
                DateTime.TryParseExact(compositeViolationSearch.SearchText, "MM/dd/yyyy", new CultureInfo("en-US"),
                                   DateTimeStyles.None, out parsedDateTime);
                var dateissuedwise = lstjobviolation.Where(x => x.DateIssued == parsedDateTime).ToList();              
                foreach (var a in dateissuedwise)
                {
                    filteredviolation.Add(a);
                }
                var DOB_ECBsummonwise = lstjobviolation.Where(x => x.SummonsNumber.ToLower().Contains(compositeViolationSearch.SearchText.ToLower())).ToList();

                foreach (var a in DOB_ECBsummonwise)
                {
                    filteredviolation.Add(a);
                }
                var descriptionwise = lstjobviolation.Where(x => !string.IsNullOrEmpty(x.ViolationDescription) && x.ViolationDescription.ToLower().Contains(compositeViolationSearch.SearchText.ToLower())).ToList();

                foreach (var a in descriptionwise)
                {
                    filteredviolation.Add(a);
                }                
                #region Comment search
                var comment = db.ChecklistJobViolationComments                                
                                 .GroupBy(c => c.IdJobViolation)
                                 .Select(grp => new
                                 {
                                     grp.Key,
                                     LastAccess = grp
                                      .OrderByDescending(x => x.LastModifiedDate)
                                      .Select(x => x.Id)
                                      .FirstOrDefault()
                                 }).ToList();
                List<ChecklistJobViolationComment> lstlatcomment = new List<ChecklistJobViolationComment>();
                foreach (var c in comment)
                {
                    lstlatcomment.Add(db.ChecklistJobViolationComments.Where(x => x.Id == c.LastAccess).FirstOrDefault());
                }               
                var commentwise = lstlatcomment.Where(x => !string.IsNullOrEmpty(x.Description.ToLower()) && x.Description.ToLower().Contains(compositeViolationSearch.SearchText.ToLower())).Select(x => x.IdJobViolation).ToList();                

                List<JobViolation> lstjobviolationcommentwise = new List<JobViolation>();
                foreach (var c in commentwise)
                {
                    var m = lstjobviolation.Where(x => x.Id == c).FirstOrDefault();
                    if (m != null)
                        lstjobviolationcommentwise.Add(m);
                }
                foreach (var a in lstjobviolationcommentwise)
                {
                    filteredviolation.Add(a);
                }
                #endregion

                int partyresponsible = 0;
                if (compositeViolationSearch.SearchText.ToLower().Trim().Contains("rpoteam"))
                {
                    partyresponsible = 1;
                }
                if (compositeViolationSearch.SearchText.ToLower().Contains("other"))
                {
                    partyresponsible = 3;
                }
                var responsblitywise = lstjobviolation.Where(x => x.PartyResponsible != null && x.PartyResponsible == partyresponsible).ToList();
                foreach (var a in responsblitywise)
                {
                    filteredviolation.Add(a);
                }
                int status = 0;
                if (compositeViolationSearch.SearchText.ToLower().Contains("open"))
                {
                    status = 1;
                }
                if (compositeViolationSearch.SearchText.ToLower().Contains("completed"))
                {
                    status = 2;
                }
                var satuswise = lstjobviolation.Where(x => status > 0 && x.Status == status).ToList();
                foreach (var a in satuswise)
                {
                    filteredviolation.Add(a);
                }
            }
            var didtinctIds = filteredviolation.Select(x => x.Id).Distinct();
            filteredviolation = db.JobViolations.Where(x => didtinctIds.Contains(x.Id)).ToList();
            List<JobViolation> finalresut1 = new List<JobViolation>();
            if (compositeViolationSearch.SearchText == "" || compositeViolationSearch.SearchText == null)
            {
                finalresut1 = lstjobviolation;
               
            }
            else
            {
                finalresut1 = filteredviolation;               
            }
            #endregion
            foreach (var jobViolation in finalresut1)
            {
                JobCompositeViolationDTO jobCompositeViolation = new JobCompositeViolationDTO();
                jobCompositeViolation.Id = jobViolation.Id;
                jobCompositeViolation.IdJob = jobViolation.IdJob;
                jobCompositeViolation.SummonsNumber = jobViolation.SummonsNumber;
                jobCompositeViolation.BalanceDue = jobViolation.BalanceDue;
                jobCompositeViolation.DateIssued = jobViolation.DateIssued;
                jobCompositeViolation.CureDate = jobViolation.CureDate;
                jobCompositeViolation.RespondentName = jobViolation.RespondentName;
                jobCompositeViolation.HearingDate = jobViolation.HearingDate;
                jobCompositeViolation.HearingLocation = jobViolation.HearingLocation;
                jobCompositeViolation.HearingResult = jobViolation.HearingResult;
                jobCompositeViolation.InspectionLocation = jobViolation.InspectionLocation;
                jobCompositeViolation.IssuingAgency = jobViolation.IssuingAgency;
                jobCompositeViolation.RespondentAddress = jobViolation.RespondentAddress;
                jobCompositeViolation.StatusOfSummonsNotice = jobViolation.StatusOfSummonsNotice;
                jobCompositeViolation.ComplianceOn = jobViolation.ComplianceOn;
                jobCompositeViolation.ResolvedDate = jobViolation.ResolvedDate;
                jobCompositeViolation.IsFullyResolved = jobViolation.IsFullyResolved;
                jobCompositeViolation.CertificationStatus = jobViolation.CertificationStatus;
                jobCompositeViolation.Notes = jobViolation.Notes;
                jobCompositeViolation.IsCOC = jobViolation.IsCOC;
                jobCompositeViolation.COCDate = jobViolation.COCDate;
                jobCompositeViolation.PaneltyAmount = jobViolation.PaneltyAmount;
                jobCompositeViolation.CreatedBy = jobViolation.CreatedBy;
                jobCompositeViolation.LastModifiedBy = jobViolation.LastModifiedBy != null ? jobViolation.LastModifiedBy : jobViolation.CreatedBy;
                jobCompositeViolation.CreatedByEmployeeName = jobViolation.CreatedByEmployee != null ? jobViolation.CreatedByEmployee.FirstName + " " + jobViolation.CreatedByEmployee.LastName : string.Empty;
                jobCompositeViolation.LastModifiedByEmployeeName = jobViolation.LastModifiedByEmployee != null ? jobViolation.LastModifiedByEmployee.FirstName + " " + jobViolation.LastModifiedByEmployee.LastName : (jobViolation.CreatedByEmployee != null ? jobViolation.CreatedByEmployee.FirstName + " " + jobViolation.CreatedByEmployee.LastName : string.Empty);
                jobCompositeViolation.CreatedDate = jobViolation.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.CreatedDate;
                jobCompositeViolation.LastModifiedDate = jobViolation.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (jobViolation.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.CreatedDate);
                jobCompositeViolation.Code = jobViolation.explanationOfCharges != null ? String.Join(", ", jobViolation.explanationOfCharges.Select(c => c.Code)) : string.Empty;
                jobCompositeViolation.CodeSection = jobViolation.explanationOfCharges != null ? String.Join(", ", jobViolation.explanationOfCharges.Select(c => c.CodeSection)) : string.Empty;
                jobCompositeViolation.Description = jobViolation.explanationOfCharges != null ? String.Join(", ", jobViolation.explanationOfCharges.Select(c => c.Description)) : string.Empty;
                jobCompositeViolation.NotesLastModifiedDate = jobViolation.NotesLastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.NotesLastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.NotesLastModifiedDate;
                jobCompositeViolation.NotesLastModifiedByEmployeeName = jobViolation.NotesLastModifiedByEmployee != null ? jobViolation.NotesLastModifiedByEmployee.FirstName + " " + jobViolation.NotesLastModifiedByEmployee.LastName : string.Empty;
                jobCompositeViolation.PartyResponsible = jobViolation.PartyResponsible;
                jobCompositeViolation.ViolationDescription = jobViolation.ViolationDescription;
                jobCompositeViolation.isnViolation = jobViolation.ISNViolation;
                jobCompositeViolation.BinNumber = jobViolation.BinNumber;
                if (jobViolation.PartyResponsible == 3) //3 means other 1 means RPOteam
                    jobCompositeViolation.ManualPartyResponsible = jobViolation.ManualPartyResponsible;
                jobCompositeViolation.Status = jobViolation.Status;               
                jobCompositeViolation.Comment =  (from cmt in db.ChecklistJobViolationComments
                                                  where cmt.IdJobViolation == jobViolation.Id
                                                  orderby cmt.LastModifiedDate descending
                 select cmt.Description).FirstOrDefault();

                if (compositeViolationSearch.IsCOProject == true)
                {
                    if (jobViolation.TCOToggle == null)
                    {
                        jobCompositeViolation.TCOToggle = false;
                    }
                    else
                    {
                        jobCompositeViolation.TCOToggle = jobViolation.TCOToggle.Value;
                    }
                }

                jobCompositeViolationList.Add(jobCompositeViolation);
            }

            // }

            return this.Ok(jobCompositeViolationList);
        }
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobApplication))]
        [Route("api/CompositeChecklist/AddExternalJobApplication/{IdCompositechecklist}")]
        public IHttpActionResult PostExternalJobApplication(JobApplication jobApplication, int IdCompositechecklist)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddApplicationsWorkPermits))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (jobApplication.IdJobApplicationType != null && jobApplication.IdJobApplicationType > 0)
                {
                    JobApplicationType jobApplicationType = db.JobApplicationTypes.FirstOrDefault(x => x.Id == jobApplication.IdJobApplicationType);
                    if (Convert.ToInt32(jobApplicationType.IdParent) == ApplicationType.DOB.GetHashCode())
                    {
                        if (!string.IsNullOrEmpty(jobApplication.ApplicationNumber))
                        {
                            List<JobApplication> jobApplications = JobApplicationNumberExists(jobApplication.ApplicationNumber, jobApplication.Id, jobApplication.IdJob, Convert.ToInt32(jobApplicationType.IdParent));
                            if (jobApplications != null && jobApplications.Count > 0)
                            {
                                throw new RpoBusinessException(string.Format(StaticMessages.ApplicationNumberAleadyExists, (jobApplications[0].Job != null ? jobApplications[0].Job.JobNumber : string.Empty)));
                            }
                        }
                    }
                    else if (Convert.ToInt32(jobApplicationType.IdParent) == ApplicationType.DOT.GetHashCode())
                    {
                        if (!string.IsNullOrEmpty(jobApplication.ApplicationNumber))
                        {
                            List<JobApplication> jobApplications = JobTrackingNumberExists(jobApplication.ApplicationNumber, jobApplication.Id, jobApplication.IdJob, Convert.ToInt32(jobApplicationType.IdParent), jobApplication.StreetWorkingOn, jobApplication.StreetFrom, jobApplication.StreetTo);
                            if (jobApplications != null && jobApplications.Count > 0)
                            {
                                throw new RpoBusinessException(string.Format(StaticMessages.TrackingNumberAleadyExists, (jobApplications[0].Job != null ? jobApplications[0].Job.JobNumber : string.Empty)));
                            }
                        }

                        if (JobApplicationLocationExists(jobApplication.StreetWorkingOn, jobApplication.StreetFrom, jobApplication.StreetTo, jobApplication.Id, jobApplication.IdJob, Convert.ToInt32(jobApplicationType.IdParent), Convert.ToInt32(jobApplication.IdJobApplicationType)))
                        {
                            throw new RpoBusinessException(StaticMessages.JobApplicationLocationAleadyExists);
                        }
                    }
                    jobApplication.IsExternalApplication = true;                   
                    jobApplication.LastModifiedDate = DateTime.UtcNow;
                    jobApplication.CreatedDate = DateTime.UtcNow;
                    if (employee != null)
                    {
                        jobApplication.CreatedBy = employee.Id;
                    }                   
                    jobApplication.IdJobWorkType = jobApplication.IdJobWorkType;
                    db.JobApplications.Add(jobApplication);
                    db.SaveChanges();

                    if (Convert.ToInt32(jobApplicationType.IdParent) == ApplicationType.DEP.GetHashCode()
                        && !string.IsNullOrEmpty(jobApplicationType.Description) &&
                        (jobApplicationType.Description.ToLower() == "hydrant"
                        || jobApplicationType.Description.ToLower() == "boiler"))
                    {
                        string forDescription = string.Empty;
                        List<JobWorkType> jobWorkType = db.JobWorkTypes.Where(x => x.IdJobApplicationType == jobApplicationType.Id).ToList();
                        foreach (var item in jobWorkType)
                        {
                            JobApplicationWorkPermitType jobApplicationWorkPermitType = new JobApplicationWorkPermitType();
                            jobApplicationWorkPermitType.IdJobApplication = jobApplication.Id;
                            jobApplicationWorkPermitType.EstimatedCost = jobApplication.TotalCost;
                            jobApplicationWorkPermitType.Code = item.Code;
                            jobApplicationWorkPermitType.IdJobWorkType = item.Id;
                            jobApplicationWorkPermitType.WorkDescription = item.Content;

                            db.JobApplicationWorkPermitTypes.Add(jobApplicationWorkPermitType);

                            if (!string.IsNullOrEmpty(jobApplicationWorkPermitType.Code))
                            {
                                if (!string.IsNullOrEmpty(forDescription))
                                {
                                    forDescription = forDescription + ", " + jobApplicationWorkPermitType.Code;
                                }
                                else
                                {
                                    forDescription = jobApplicationWorkPermitType.Code;
                                }
                            }
                            db.SaveChanges();

                            JobApplicationWorkPermitType jobApplicationWorkPermitResponse = db.JobApplicationWorkPermitTypes.Include("JobApplication.JobApplicationType")
                   .Include("JobApplication.ApplicationStatus")
                  .Include("JobWorkType")
                  .Include("ContactResponsible")
                  .FirstOrDefault(r => r.Id == jobApplicationWorkPermitType.Id);

                            string addWorkPermit_DEP = JobHistoryMessages.AddWorkPermit_DEP
                                                       .Replace("##PermitNumber##", !string.IsNullOrEmpty(jobApplicationWorkPermitResponse.PermitNumber) ? jobApplicationWorkPermitResponse.PermitNumber : JobHistoryMessages.NoSetstring)
                                                       .Replace("##PermitType##", jobApplicationWorkPermitResponse.JobWorkType != null ? jobApplicationWorkPermitResponse.JobWorkType.Description : JobHistoryMessages.NoSetstring)
                                                       .Replace("##ApplicationType##", !string.IsNullOrEmpty(jobApplicationType.Description) ? jobApplicationType.Description : JobHistoryMessages.NoSetstring)
                                                       .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) ? jobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetFrom) ? jobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetTo) ? jobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                       .Replace("##PermitIssued##", jobApplicationWorkPermitResponse.Issued != null ? jobApplicationWorkPermitResponse.Issued.Value.ToShortDateString() : JobHistoryMessages.NoSetstring)
                                                       .Replace("##PermitExpiry##", jobApplicationWorkPermitResponse.Expires != null ? jobApplicationWorkPermitResponse.Expires.Value.ToShortDateString() : JobHistoryMessages.NoSetstring);

                            Common.SaveJobHistory(employee.Id, jobApplication.IdJob, addWorkPermit_DEP, JobHistoryType.WorkPermits);

                        }

                        jobApplication.ApplicationFor = !string.IsNullOrEmpty(forDescription) ? forDescription.Trim(',') : string.Empty;
                        db.SaveChanges();
                    }

                    Common.UpdateJobLastModifiedDate(jobApplication.IdJob, employee.Id);

                    ApplicationStatus applicationStatus = db.ApplicationStatus.FirstOrDefault(x => x.Id == jobApplication.IdApplicationStatus);
                    switch ((ApplicationType)Convert.ToInt32(jobApplicationType.IdParent))
                    {
                        case ApplicationType.DOB:
                            string addApplication_DOB = JobHistoryMessages.AddApplication_DOB
                                                        .Replace("##ApplicationType##", jobApplicationType != null ? jobApplicationType.Description : JobHistoryMessages.NoSetstring)
                                                        .Replace("##ApplicationStatus##", !string.IsNullOrEmpty(jobApplication.JobApplicationStatus) ? jobApplication.JobApplicationStatus : JobHistoryMessages.NoSetstring)
                                                        .Replace("##ApplicationNumber##", !string.IsNullOrEmpty(jobApplication.ApplicationNumber) ? jobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);
                            Common.SaveJobHistory(employee.Id, jobApplication.IdJob, addApplication_DOB, JobHistoryType.Applications);
                            break;
                        case ApplicationType.DOT:
                            string addApplication_DOT = JobHistoryMessages.AddApplication_DOT
                                                        .Replace("##ApplicationType##", jobApplicationType != null ? jobApplicationType.Description : JobHistoryMessages.NoSetstring)
                                                        .Replace("##ApplicationStatus##", applicationStatus != null && !string.IsNullOrEmpty(applicationStatus.Name) ? applicationStatus.Name : JobHistoryMessages.NoSetstring)
                                                        .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) ? jobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetFrom) ? jobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetTo) ? jobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                        .Replace("##Tracking##", !string.IsNullOrEmpty(jobApplication.ApplicationNumber) ? jobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring);
                            Common.SaveJobHistory(employee.Id, jobApplication.IdJob, addApplication_DOT, JobHistoryType.Applications);
                            break;
                        case ApplicationType.DEP:
                            string addApplication_DEP = JobHistoryMessages.AddApplication_DEP
                                                        .Replace("##ApplicationType##", jobApplicationType != null ? jobApplicationType.Description : JobHistoryMessages.NoSetstring)
                                                        .Replace("##LocationDetails##", (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) || !string.IsNullOrEmpty(jobApplication.StreetTo) || !string.IsNullOrEmpty(jobApplication.StreetFrom)) ? (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) ? jobApplication.StreetWorkingOn : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetFrom) ? jobApplication.StreetFrom : string.Empty) + " - " + (!string.IsNullOrEmpty(jobApplication.StreetTo) ? jobApplication.StreetTo : string.Empty) : JobHistoryMessages.NoSetstring)
                                                        .Replace("##ApplicationStatus##", applicationStatus != null && !string.IsNullOrEmpty(applicationStatus.Name) ? applicationStatus.Name : JobHistoryMessages.NoSetstring)
                                                        .Replace("##Description##", jobApplication.Description != null && !string.IsNullOrEmpty(jobApplication.Description) ? jobApplication.Description : JobHistoryMessages.NoSetstring);
                            Common.SaveJobHistory(employee.Id, jobApplication.IdJob, addApplication_DEP, JobHistoryType.Applications);
                            break;
                        default:
                            break;
                    }                   
                    string[] idjobworktpypes = jobApplication.IdJobWorkType.Split(',');
                    foreach (var a in idjobworktpypes)
                    {
                        JobApplicationWorkPermitType objjobApplicationWorkPermitType = new JobApplicationWorkPermitType();
                        objjobApplicationWorkPermitType.IdJobApplication = jobApplication.Id;
                        objjobApplicationWorkPermitType.JobApplication = jobApplication;
                        objjobApplicationWorkPermitType.IdJobWorkType = Convert.ToInt32(a);
                        objjobApplicationWorkPermitType.IdResponsibility = 2;//other
                        int idjobworktype = Convert.ToInt32(a);
                        objjobApplicationWorkPermitType.Code = db.JobWorkTypes.Where(x => x.Id == idjobworktype).FirstOrDefault().Code;
                        objjobApplicationWorkPermitType.LastModifiedDate = DateTime.UtcNow;
                        objjobApplicationWorkPermitType.CreatedDate = DateTime.UtcNow;
                        if (employee != null)
                        {
                            objjobApplicationWorkPermitType.CreatedBy = employee.Id;
                        }

                        db.JobApplicationWorkPermitTypes.Add(objjobApplicationWorkPermitType);
                    }
                    db.SaveChanges();                  

                    JobChecklistHeader jobChecklistHeader = new JobChecklistHeader();
                    jobChecklistHeader.IdJob = jobApplication.IdJob;
                    jobChecklistHeader.IdJobApplication = jobApplication.Id;
                    jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;
                    jobChecklistHeader.CreatedDate = DateTime.UtcNow;
                    var applicationtype = db.JobApplicationTypes.Where(x => x.Id == jobApplication.IdJobApplicationType).Select(x => x.Description).FirstOrDefault();
                    var applicationnumber = jobApplication.ApplicationNumber;
                  
                    List<string> worktype= new List<string>();
                  
                    foreach (var a in idjobworktpypes)
                    {
                        int idworktype = Convert.ToInt32(a);
                        worktype.Add(db.JobWorkTypes.Where(x => x.Id == idworktype).FirstOrDefault().Code);
                    }
                    string workcode = worktype[0];
                    for (int i = 1; i < worktype.Count; i++)
                    {
                        workcode += ',' + worktype[i];
                    }
                    var applicationstatus = "";
                    string title = applicationtype + (applicationnumber != null ? " - " + applicationnumber : string.Empty) + " - " + workcode;                   
                    jobChecklistHeader.ChecklistName = title;


                    // End add in jobchecklistheader 
                    //add in jobchecklistGroup
                    //var codes = db.JobWorkTypes.Where(x => x.Id == idjobworktype).ToList().Select(x => x.Code); //no need of this because we dont want plumbing now
                    List<JobChecklistGroup> lstJobChecklistGroup = new List<JobChecklistGroup>();
                    List<CheckListGroup> resultChecklistGroup = new List<CheckListGroup>();                  
                    resultChecklistGroup = db.CheckListGroups.Where(x => x.Type.ToLower() == "tr" || x.Name.ToLower().Contains("sign")).ToList();
                    
                    foreach (var a in resultChecklistGroup)
                    {
                        JobChecklistGroup jobChecklistGroup = new JobChecklistGroup();
                        jobChecklistGroup.IdCheckListGroup = a.Id;
                        jobChecklistGroup.IdJobCheckListHeader = jobChecklistHeader.IdJobCheckListHeader;
                        jobChecklistGroup.IsActive = true;
                        jobChecklistGroup.JobChecklistHeaders = jobChecklistHeader;
                        jobChecklistGroup.LastModifiedDate = DateTime.UtcNow;
                        jobChecklistGroup.CreatedDate = DateTime.UtcNow;
                        if (employee != null)
                        {
                            jobChecklistGroup.CreatedBy = employee.Id;
                        }
                        lstJobChecklistGroup.Add(jobChecklistGroup);
                    }
                    db.JobChecklistGroups.AddRange(lstJobChecklistGroup);
                    db.SaveChanges();
                    //End add in jobchecklistGroup

                    //add in Compositechecklist

                    List<CompositeChecklistDetail> lstcompositeChecklistDetail = new List<CompositeChecklistDetail>();

                    var groupsinheader = db.JobChecklistGroups.Where(x => x.IdJobCheckListHeader == jobChecklistHeader.IdJobCheckListHeader).ToList();
                    foreach (var c in groupsinheader)
                    {
                        CompositeChecklistDetail objcompositeChecklistDetail = new CompositeChecklistDetail();
                        objcompositeChecklistDetail.IdCompositeChecklist = IdCompositechecklist;
                        objcompositeChecklistDetail.IdJobChecklistHeader = jobChecklistHeader.IdJobCheckListHeader;
                        objcompositeChecklistDetail.IdJobChecklistGroup = c.Id;
                        objcompositeChecklistDetail.JobChecklistGroups = c;
                        objcompositeChecklistDetail.LastModifiedDate = DateTime.UtcNow;
                        objcompositeChecklistDetail.CreatedDate = DateTime.UtcNow;
                        if (employee != null)
                        {
                            objcompositeChecklistDetail.CreatedBy = employee.Id;
                        }
                        lstcompositeChecklistDetail.Add(objcompositeChecklistDetail);
                    }
                    db.CompositeChecklistDetails.AddRange(lstcompositeChecklistDetail);
                    db.SaveChanges();   
                    //End add in Compositechecklist
                    return Ok();                    
                }
                else
                {
                    throw new RpoBusinessException(StaticMessages.ApplicationTypeRequired);
                }
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Manage the orders of the composite checklist.
        /// </summary>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/CompositeChecklist/ManageCompositeChecklistOrder/{IdCompositeChecklist}")]
        public IHttpActionResult ManageCompositeChecklistOrder([FromBody] ManageOrderDTO manageOrderDTO, int IdCompositeChecklist)
        {
            var Message = string.Empty;
            try
            {
                if (manageOrderDTO != null)
                {
                    foreach (var detailid in manageOrderDTO.Headers)
                    {
                        var result = db.CompositeChecklistDetails.Where(x => x.IdCompositeChecklist == IdCompositeChecklist && x.IdJobChecklistHeader == detailid.JobChecklistHeaderId).ToList();
                        foreach (var res in result)
                        {
                            res.CompositeOrder = detailid.CompositeOrder;
                        }
                        db.SaveChanges();
                    }
                    Message = StaticMessages.ManageGroupItem_Successful;
                }
                else { Message = StaticMessages.ManageOrder_UnSuccessful; }
            }
            catch (Exception ex) { throw ex.InnerException; }
            return Ok(Message);
        }
        [Authorize]
        [RpoAuthorize]
        [HttpDelete]
        [Route("api/DelinkCompositeCheckList/{IdCompositeChecklist}/{IdJobchecklist}")]
        public IHttpActionResult DelinkCompositeCheckList(int IdCompositeChecklist, int IdJobchecklist)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteChecklist))
            {
                var result = db.CompositeChecklistDetails.Where(b => b.IdJobChecklistHeader == IdJobchecklist && b.IdCompositeChecklist == IdCompositeChecklist).ToList();
                db.CompositeChecklistDetails.RemoveRange(result);
                try
                {
                    db.SaveChanges();
                    return Ok("De-linked chcklist from composite checklist");
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Jobs the application number exists.
        /// </summary>
        /// <param name="applicationNumber">The application number.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private List<JobApplication> JobApplicationNumberExists(string applicationNumber, int id, int idjob, int idJobApplicationTypeParent)
        {
            var jobApplications = db.JobApplications.Include("Job").Include("JobApplicationType").Where(e => e.ApplicationNumber == applicationNumber && e.Id != id && e.IdJob == idjob && e.JobApplicationType.IdParent == idJobApplicationTypeParent).ToList();
            return jobApplications;
        }
        private List<JobApplication> JobTrackingNumberExists(string trackingNumber, int id, int idJob, int idJobApplicationTypeParent, string streetWorkingOn, string streetWorkingFrom, string streetWorkingTo)
        {
            var jobApplications = db.JobApplications.Include("Job").Include("JobWorkPermitHistories").Include("JobApplicationType").Where(e => (e.ApplicationNumber == trackingNumber || e.JobWorkPermitHistories.Any(p => p.NewNumber == trackingNumber || p.OldNumber == trackingNumber)) && e.StreetWorkingOn.ToLower().Contains(streetWorkingOn.ToLower()) && e.StreetFrom.ToLower().Contains(streetWorkingFrom.ToLower()) && e.StreetTo.ToLower().Contains(streetWorkingOn.ToLower())).ToList();           
            return jobApplications;
        }
        private bool JobApplicationLocationExists(string streetWorkingOn, string streetWorkingFrom, string streetWorkingTo, int id, int idJob, int idJobApplicationTypeParent, int idJobApplicationType)
        {
            var jobApplications = db.JobApplications.Include("Job").Include("JobApplicationType").Count(e => e.StreetFrom == streetWorkingFrom &&
              e.StreetTo == streetWorkingTo &&
              e.StreetWorkingOn == streetWorkingOn
              && e.Id != id && e.IdJob == idJob && e.JobApplicationType.IdParent == idJobApplicationTypeParent && e.IdJobApplicationType == idJobApplicationType);

            return jobApplications > 0;

        }
        
        [HttpGet]
        [Route("api/Checklist/GetParentCompositeChecklistExists/{IdParentChecklist}")]
        public bool GetParentCompositeChecklistExists(int IdParentChecklist)
        {
            var result = db.CompositeChecklistDetails.Where(x => x.IdJobChecklistHeader == IdParentChecklist && x.IsParentCheckList == true).FirstOrDefault();
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        [Authorize]
        [RpoAuthorize]
        [HttpDelete]
        [Route("api/composite/DelinkViolation/{IdCompositechecklist}/{IdViolation}")]
        public IHttpActionResult DelinkViolation(int IdCompositechecklist, int IdViolation)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            CompositeViolations compositeViolation = db.CompositeViolations.Where(x => x.IdCompositeChecklist == IdCompositechecklist && x.IdJobViolations == IdViolation).FirstOrDefault();
            if (compositeViolation == null)
            {
                return this.NotFound();
            }
            db.CompositeViolations.Remove(compositeViolation);
            db.SaveChanges();

            return Ok("Delinked violation successfully");
        }

        [Authorize]
        [RpoAuthorize]
        [Route("api/composite/PostDOB_ECBViolation_CompositeChecklist")]
        public IHttpActionResult PostDOB_ECBViolation_CompositeChecklist(Violationchecklist Violationchecklist)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewChecklist)
                   || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist)
                   || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                List<CompositeViolations> lstCompositeViolations = new List<CompositeViolations>();
                var existingviolations = db.CompositeViolations.Where(x => x.IdCompositeChecklist == Violationchecklist.IdCompositechecklist).Select(y => y.IdJobViolations).ToList();
                var Violation = db.JobViolations.Where(x => existingviolations.Contains(x.Id)).ToList();
                List<JobViolation> lstjobviolation = new List<JobViolation>();
                foreach (var v in Violation)
                {
                    if (v.Type_ECB_DOB == Violationchecklist.Type_ECB_DOB)
                    {
                        lstjobviolation.Add(v);
                    }
                }
                foreach (var v in lstjobviolation)
                {
                    db.CompositeViolations.Remove(db.CompositeViolations.Where(x => x.IdJobViolations == v.Id && x.IdCompositeChecklist == Violationchecklist.IdCompositechecklist).FirstOrDefault());
                }             
                foreach (var l in Violationchecklist.lstIdViolations)
                {
                    CompositeViolations compositeViolations = new CompositeViolations();
                    compositeViolations.IdCompositeChecklist = Violationchecklist.IdCompositechecklist;
                    compositeViolations.IdJobViolations = l;
                    lstCompositeViolations.Add(compositeViolations);
                }
                db.CompositeViolations.AddRange(lstCompositeViolations);
                db.SaveChanges();

                return Ok("Violation added in Composite Checklist successfully");
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }


        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/CompositeCheckList/GetDOBViolationsNotAdded_Composite/{IdJob}/{Idcompositechecklist}")]
        public IHttpActionResult GetDOBViolationsNotAdded_Composite(int IdJob, int Idcompositechecklist)
        {
            Job job = db.Jobs.FirstOrDefault(j => j.Id == IdJob);

            if (job == null)
                return this.NotFound();

            List<JobCompositeViolationDTO> jobCompositeViolationList = new List<JobCompositeViolationDTO>();
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);        
          
            var idRfpAddress = db.Jobs.Where(x => x.Id == IdJob).Select(y => y.IdRfpAddress).FirstOrDefault();           
            var binnumber = db.RfpAddresses.Where(x => x.Id == idRfpAddress).Select(y => y.BinNumber).FirstOrDefault();
            var jobViolations = db.JobViolations.Where(x => x.BinNumber == binnumber && (x.IdJob == null || x.IdJob == 0) || x.IdJob == IdJob).Where(y=>y.Type_ECB_DOB=="DOB")
                                .Include("CreatedByEmployee").Include("LastModifiedByEmployee")                                
                                .ToList();
            List<JobViolationDTO_checklist> lstjobviolation = new List<JobViolationDTO_checklist>();
            foreach (var v in jobViolations)
            {
                var compositeviolation = db.CompositeViolations.Where(x => x.IdCompositeChecklist == Idcompositechecklist && x.IdJobViolations == v.Id).ToList();               
                JobViolationDTO_checklist jobViolationDTO_checklist = new JobViolationDTO_checklist();
                if (compositeviolation.Count>0)
                {                   
                     jobViolationDTO_checklist.IsPartofchecklist = true;
                } 
                jobViolationDTO_checklist.jobViolations= Format(v);
                   
                lstjobviolation.Add(jobViolationDTO_checklist);
            }

            return this.Ok(lstjobviolation);
        }

        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/CompositeCheckList/GetECBViolationsNotAdded_Composite/{IdJob}/{Idcompositechecklist}")]
        public IHttpActionResult GetECBViolationsNotAdded_Composite(int IdJob, int Idcompositechecklist)
        {
            Job job = db.Jobs.FirstOrDefault(j => j.Id == IdJob);

            if (job == null)
                return this.NotFound();

            List<JobCompositeViolationDTO> jobCompositeViolationList = new List<JobCompositeViolationDTO>();
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            var idRfpAddress = db.Jobs.Where(x => x.Id == IdJob).Select(y => y.IdRfpAddress).FirstOrDefault();          
            var binnumber = db.RfpAddresses.Where(x => x.Id == idRfpAddress).Select(y => y.BinNumber).FirstOrDefault();
            var jobViolations = db.JobViolations.Where(x => x.BinNumber == binnumber && (x.IdJob == null || x.IdJob == 0) || x.IdJob == IdJob).Where(y => y.Type_ECB_DOB == "ECB")             
                .ToList();

            List<JobViolationDTO_checklist> lstjobviolation = new List<JobViolationDTO_checklist>();
            foreach (var v in jobViolations)
            {               
                var compositeviolation = db.CompositeViolations.Where(x => x.IdCompositeChecklist == Idcompositechecklist && x.IdJobViolations == v.Id).ToList();             
                JobViolationDTO_checklist jobViolationDTO_checklist = new JobViolationDTO_checklist();
                if (compositeviolation.Count > 0)
                {
                    jobViolationDTO_checklist.IsPartofchecklist = true;
                }                
                jobViolationDTO_checklist.jobViolations = Format(v);                     
                lstjobviolation.Add(jobViolationDTO_checklist);
            }
            return this.Ok(lstjobviolation);
        }

        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/GetChecklistViolationcommentHistory/{IdJobviolation}")]
        public IHttpActionResult GetChecklistViolationcommentHistory(int IdJobviolation)            
        {
            var getchecklistviolationHistory = db.ChecklistJobViolationComments.Where(x => x.IdJobViolation == IdJobviolation);
            if (getchecklistviolationHistory == null)
            {
                return this.NotFound();
            }

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            var result = db.ChecklistJobViolationComments
                        .Where(x => x.IdJobViolation == IdJobviolation)
                .AsEnumerable()
                .Select(checklistviolationHistory => new
                {
                    IdJobviolation = checklistviolationHistory.IdJobViolation,
                    Description = checklistviolationHistory.Description,
                    CreatedDate = checklistviolationHistory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc
                                            (Convert.ToDateTime(checklistviolationHistory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone))
                                            : checklistviolationHistory.CreatedDate,
                    LastModifiedDate = checklistviolationHistory.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc
                                            (Convert.ToDateTime(checklistviolationHistory.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone))
                                            : (checklistviolationHistory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(checklistviolationHistory.CreatedDate),
                                            TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : checklistviolationHistory.CreatedDate),
                    CreatedBy = checklistviolationHistory.CreatedBy,
                    LastModifiedBy = checklistviolationHistory.LastModifiedBy != null ? checklistviolationHistory.LastModifiedBy : checklistviolationHistory.CreatedBy,
                    CreatedByEmployee = checklistviolationHistory.CreatedByEmployee != null ? checklistviolationHistory.CreatedByEmployee.FirstName + " " + checklistviolationHistory.CreatedByEmployee.LastName : string.Empty,
                    LastModifiedByEmployee = checklistviolationHistory.LastModifiedByEmployee != null ? checklistviolationHistory.LastModifiedByEmployee.FirstName + " " + checklistviolationHistory.LastModifiedByEmployee.LastName : (checklistviolationHistory.CreatedByEmployee != null ? checklistviolationHistory.CreatedByEmployee.FirstName + " " + checklistviolationHistory.CreatedByEmployee.LastName : string.Empty),
                }).OrderByDescending(x => x.LastModifiedDate);

            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/PostChecklistViolationComment")]
        public IHttpActionResult PostChecklistViolationComment(ChecklistViolationCommentDTO checklistViolationCommentDTO)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            ChecklistJobViolationComment checklistJobViolationComment = new ChecklistJobViolationComment();
            checklistJobViolationComment.IdJobViolation = checklistViolationCommentDTO.IdJobViolation;
            checklistJobViolationComment.Description = checklistViolationCommentDTO.Description;
            checklistJobViolationComment.CreatedDate = DateTime.UtcNow;
            checklistJobViolationComment.LastModifiedDate = DateTime.UtcNow;
            if (employee != null)
            {
                checklistJobViolationComment.CreatedBy = employee.Id;
                checklistJobViolationComment.LastModifiedBy = employee.Id;
            }
            db.ChecklistJobViolationComments.Add(checklistJobViolationComment);

            try { db.SaveChanges(); } catch (Exception ex) { throw ex; }
           
            return Ok("Comment Added Successfully");
        }

        /// <summary>
        /// Formats the specified job violation.
        /// </summary>
        /// <param name="jobViolation">The job violation.</param>
        /// <returns>JobViolationDTO.</returns>
        private JobViolationDTO Format(JobViolation jobViolation)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            JobViolationDTO jobViolationDTO = new JobViolationDTO();

            jobViolationDTO.Id = jobViolation.Id;
            jobViolationDTO.IdJob = jobViolation.IdJob;
            jobViolationDTO.SummonsNumber = jobViolation.SummonsNumber;
            jobViolationDTO.BalanceDue = jobViolation.BalanceDue;
            jobViolationDTO.DateIssued = jobViolation.DateIssued;
            jobViolationDTO.CureDate = jobViolation.CureDate;
            jobViolationDTO.RespondentName = jobViolation.RespondentName;
            jobViolationDTO.HearingDate = jobViolation.HearingDate;
            jobViolationDTO.HearingLocation = jobViolation.HearingLocation;
            jobViolationDTO.HearingResult = jobViolation.HearingResult;
            jobViolationDTO.InspectionLocation = jobViolation.InspectionLocation;
            jobViolationDTO.IssuingAgency = jobViolation.IssuingAgency;
            jobViolationDTO.RespondentAddress = jobViolation.RespondentAddress;
            jobViolationDTO.StatusOfSummonsNotice = jobViolation.StatusOfSummonsNotice;
            jobViolationDTO.ComplianceOn = jobViolation.ComplianceOn;
            jobViolationDTO.ResolvedDate = jobViolation.ResolvedDate;
            jobViolationDTO.IsFullyResolved = jobViolation.IsFullyResolved;
            jobViolationDTO.CertificationStatus = jobViolation.CertificationStatus;
            jobViolationDTO.Notes = jobViolation.Notes;
            jobViolationDTO.IsCOC = jobViolation.IsCOC;
            jobViolationDTO.COCDate = jobViolation.COCDate;
            jobViolationDTO.PaneltyAmount = jobViolation.PaneltyAmount;
            jobViolationDTO.CreatedBy = jobViolation.CreatedBy;
            jobViolationDTO.LastModifiedBy = jobViolation.LastModifiedBy != null ? jobViolation.LastModifiedBy : jobViolation.CreatedBy;
            jobViolationDTO.CreatedByEmployeeName = jobViolation.CreatedByEmployee != null ? jobViolation.CreatedByEmployee.FirstName + " " + jobViolation.CreatedByEmployee.LastName : string.Empty;
            jobViolationDTO.LastModifiedByEmployeeName = jobViolation.LastModifiedByEmployee != null ? jobViolation.LastModifiedByEmployee.FirstName + " " + jobViolation.LastModifiedByEmployee.LastName : (jobViolation.CreatedByEmployee != null ? jobViolation.CreatedByEmployee.FirstName + " " + jobViolation.CreatedByEmployee.LastName : string.Empty);
            jobViolationDTO.CreatedDate = jobViolation.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.CreatedDate;
            jobViolationDTO.LastModifiedDate = jobViolation.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (jobViolation.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.CreatedDate);
            jobViolationDTO.Code = jobViolation.explanationOfCharges != null ? String.Join(", ", jobViolation.explanationOfCharges.Select(c => c.Code)) : string.Empty;
            jobViolationDTO.CodeSection = jobViolation.explanationOfCharges != null ? String.Join(", ", jobViolation.explanationOfCharges.Select(c => c.CodeSection)) : string.Empty;
            jobViolationDTO.Description = jobViolation.explanationOfCharges != null ? String.Join(", ", jobViolation.explanationOfCharges.Select(c => c.Description)) : string.Empty;
            jobViolationDTO.NotesLastModifiedDate = jobViolation.NotesLastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.NotesLastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.NotesLastModifiedDate;
            jobViolationDTO.NotesLastModifiedByEmployeeName = jobViolation.NotesLastModifiedByEmployee != null ? jobViolation.NotesLastModifiedByEmployee.FirstName + " " + jobViolation.NotesLastModifiedByEmployee.LastName : string.Empty;

            jobViolationDTO.LastModifiedDate = jobViolation.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (jobViolation.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.CreatedDate);
            jobViolationDTO.Disposition_Date = jobViolation.Disposition_Date != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.Disposition_Date), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.Disposition_Date;
            jobViolationDTO.Disposition_Comments = jobViolation.Disposition_Comments;
            jobViolationDTO.device_number = jobViolation.device_number;
            jobViolationDTO.ViolationDescription = jobViolation.ViolationDescription;
            jobViolationDTO.ecb_number = jobViolation.ECBnumber;
            jobViolationDTO.violation_number = jobViolation.violation_number;
            jobViolationDTO.violation_category = jobViolation.violation_category;
            jobViolationDTO.BinNumber = jobViolation.BinNumber;
            jobViolationDTO.PartyResponsible = jobViolation.PartyResponsible;
            jobViolationDTO.IdContact = jobViolation.IdContact;
            jobViolationDTO.ManualPartyResponsible = jobViolation.ManualPartyResponsible;
            jobViolationDTO.HearingTime = jobViolation.HearingTime;
            jobViolationDTO.violation_type_code = jobViolation.violation_type_code;
            jobViolationDTO.AggravatedLevel = jobViolation.aggravated_level;
            jobViolationDTO.violation_type = jobViolation.violation_type;
            jobViolationDTO.Status = jobViolation.Status;
            return jobViolationDTO;
        }
        public class Violationchecklist
        {
            public int IdCompositechecklist;
            public List<int> lstIdViolations;
            public string Type_ECB_DOB;
        }
    }
}