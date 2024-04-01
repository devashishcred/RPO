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

    // using Models;

    public class JobChecklistController : ApiController
    {
        RpoContext db = new RpoContext();
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobChecklistHeaderDTO))]
        public IHttpActionResult PostJobChecklist(JobChecklistHeaderDTO jobChecklistHeaderDTO)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewChecklist)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteChecklist))
            {
                List<int?> WorkTypeIds = new List<int?>();
                RfpAddress rfpAddress = db.Jobs.Include("RfpAddress").FirstOrDefault(j => j.Id == jobChecklistHeaderDTO.IdJob).RfpAddress;
                JobApplication jobapplication = db.JobApplications.FirstOrDefault(j => j.Id == jobChecklistHeaderDTO.IdJobApplication);

                List<int> ListOfProperties = new List<int>();

                if (rfpAddress.CoastalErosionHazardAreaMapCheck == true)
                {
                    ListOfProperties.Add(Enums.AddressProperties.CoastalErosionHazardAreaMapCheck.GetHashCode());
                }
                if (rfpAddress.FreshwaterWetlandsMapCheck == true)
                {
                    ListOfProperties.Add(Enums.AddressProperties.FreshwaterWetlandsMapCheck.GetHashCode());
                }
                if (rfpAddress.IdOwnerType != null)
                {
                    ListOfProperties.Add(Enums.AddressProperties.OwnerType.GetHashCode());
                }
                if (rfpAddress.SpecialDistrict != null && rfpAddress.SpecialDistrict != string.Empty)
                {
                    ListOfProperties.Add(Enums.AddressProperties.SpecialDistrict.GetHashCode());
                }
                if (rfpAddress.SpecialFloodHazardAreaCheck == true)
                {
                    ListOfProperties.Add(Enums.AddressProperties.SpecialFloodHazardAreaCheck.GetHashCode());
                }
                if (rfpAddress.TidalWetlandsMapCheck == true)
                {
                    ListOfProperties.Add(Enums.AddressProperties.TidalWetlandsMapCheck.GetHashCode());
                }
                if (rfpAddress.IsLandmark == true)
                {
                    ListOfProperties.Add(Enums.AddressProperties.Landmark.GetHashCode());
                }
                if (rfpAddress.SRORestrictedCheck == true)
                {
                    ListOfProperties.Add(Enums.AddressProperties.SROrestricted.GetHashCode());
                }
                if (rfpAddress.IsLittleE == true)
                {
                    ListOfProperties.Add(Enums.AddressProperties.Environmentalrestrictions.GetHashCode());
                }
                if (rfpAddress.LoftLawCheck == true)
                {
                    ListOfProperties.Add(Enums.AddressProperties.LoftLaw.GetHashCode());
                }
                if (jobapplication.IsHighRise == true)
                {
                    ListOfProperties.Add(Enums.AddressProperties.Highrise.GetHashCode());
                }
                if (rfpAddress.CityOwnedCheck == true)
                {
                    ListOfProperties.Add(Enums.AddressProperties.CityOwned.GetHashCode());
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                JobChecklistHeader jobChecklistHeader = new JobChecklistHeader();
                jobChecklistHeader.IdJob = jobChecklistHeaderDTO.IdJob;
                jobChecklistHeader.IdJobApplication = jobChecklistHeaderDTO.IdJobApplication;

                jobChecklistHeader.CreatedDate = DateTime.UtcNow;
                jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobChecklistHeader.CreatedBy = employee.Id;
                }

                var application = db.JobApplications.Where(x => x.Id == jobChecklistHeaderDTO.IdJobApplication).FirstOrDefault();
                var applicationtype = db.JobApplicationTypes.Where(x => x.Id == application.IdJobApplicationType).Select(x => x.Description).FirstOrDefault();
                var applicationnumber = application.ApplicationNumber;
                var worktype = jobChecklistHeaderDTO.JobApplicationWorkPermitTypes.Select(x => x.Code);
                string applicationstatus = application.JobApplicationStatus;
                StringBuilder strworktype = new StringBuilder();
                foreach (var d in worktype)
                {
                    strworktype.Append(d + ", ");
                }
                //string title = applicationtype + (applicationnumber != null ? " - " + applicationnumber : string.Empty) + " - " + strworktype.Remove(strworktype.Length - 2, 2) + " - " + application.FloorWorking;
                // string title = applicationtype + (applicationnumber != null ? " - " + applicationnumber : string.Empty) + " - " + strworktype.Remove(strworktype.Length - 2, 2) + (applicationstatus != null ? " - " + applicationstatus : string.Empty) + (application.FloorWorking != null ? " - " + application.FloorWorking : string.Empty);               
                string title = applicationtype + (applicationnumber != null ? " - " + applicationnumber : string.Empty) + (strworktype != null ? " - " + strworktype.Remove(strworktype.Length - 2, 2) : string.Empty) + (applicationstatus != null ? " - " + applicationstatus : string.Empty) + (application.FloorWorking != null ? " - " + application.FloorWorking : string.Empty);
                jobChecklistHeader.ChecklistName = title;
                db.JobChecklistHeaders.Add(jobChecklistHeader);
                db.SaveChanges();// save in headertable

                if (jobChecklistHeaderDTO.JobApplicationWorkPermitTypes != null)
                {
                    if (jobChecklistHeader.JobApplicationWorkPermitTypes == null)
                    {
                        jobChecklistHeader.JobApplicationWorkPermitTypes = new HashSet<JobApplicationWorkPermitType>();
                    }
                    foreach (var jobApplicationWorkPermitTypes in jobChecklistHeaderDTO.JobApplicationWorkPermitTypes)
                    {
                        jobChecklistHeader.JobApplicationWorkPermitTypes.Add(db.JobApplicationWorkPermitTypes.Find(jobApplicationWorkPermitTypes.Id));
                        WorkTypeIds.Add(db.JobApplicationWorkPermitTypes.Find(jobApplicationWorkPermitTypes.Id).IdJobWorkType);
                    }
                    db.SaveChanges();
                }
                #region get properties based on workpermit like SiteSafetyCoordinator,Superintendentofconstruction,SiteSafetyManager
                //  getting address property based on workpermit which are in work permit popup like SiteSafetyCoordinator,Superintendentofconstruction,SiteSafetyManager
                var jobChecklistHeaderWorkpermits = db.JobChecklistHeaders.Include("JobApplicationWorkPermitTypes").
                                                    Where(x => x.IdJobCheckListHeader == jobChecklistHeader.IdJobCheckListHeader)
                                                    .Select(y => y.JobApplicationWorkPermitTypes);

                foreach (var a in jobChecklistHeaderWorkpermits)
                {
                    if (a.Select(x => x.HasSiteSafetyCoordinator).FirstOrDefault() == true)
                    {
                        if (!ListOfProperties.Contains(Enums.AddressProperties.SiteSafetyCoordinator.GetHashCode()))
                            ListOfProperties.Add(Enums.AddressProperties.SiteSafetyCoordinator.GetHashCode());
                    }
                    if (a.Select(x => x.HasSiteSafetyManager).FirstOrDefault() == true)
                    {
                        if (!ListOfProperties.Contains(Enums.AddressProperties.SiteSafetyManager.GetHashCode()))
                            ListOfProperties.Add(Enums.AddressProperties.SiteSafetyManager.GetHashCode());
                    }
                    if (a.Select(x => x.HasSuperintendentofconstruction).FirstOrDefault() == true)
                    {
                        if (!ListOfProperties.Contains(Enums.AddressProperties.SuperintendentOfConstruction.GetHashCode()))
                            ListOfProperties.Add(Enums.AddressProperties.SuperintendentOfConstruction.GetHashCode());
                    }
                }
                // End getting address property based on workpermit
                #endregion

                #region Add groups in JobChecklistGroups
                JobChecklistHeader newJobChecklistHeader = db.JobChecklistHeaders.Find(jobChecklistHeader.IdJobCheckListHeader);
                if (jobChecklistHeaderDTO.CheckListGroups != null)
                {
                    List<JobChecklistGroup> lstjobChecklistGroupDetail = new List<JobChecklistGroup>();
                    JobChecklistGroup jobChecklistGroupDetail;
                    foreach (var tmpCheckListGroup in jobChecklistHeaderDTO.CheckListGroups)
                    {
                        jobChecklistGroupDetail = new JobChecklistGroup();
                        jobChecklistGroupDetail.IdJobCheckListHeader = newJobChecklistHeader.IdJobCheckListHeader;
                        jobChecklistGroupDetail.IdCheckListGroup = tmpCheckListGroup.Id;
                        jobChecklistGroupDetail.Displayorder1 = tmpCheckListGroup.Displayorder;
                        db.JobChecklistGroups.Add(jobChecklistGroupDetail);
                    }
                    db.SaveChanges();
                }
                #endregion             

                #region NEW Bussiness logic for getting items based on application and permit and group and properties
                //start of business logic               

                List<ChecklistItem> groupwise_items = new List<ChecklistItem>();
                List<ChecklistItem> groupwise_final_items = new List<ChecklistItem>();
                List<ChecklistItem> final_items = new List<ChecklistItem>();


                foreach (var tmpCheckListGroup in jobChecklistHeaderDTO.CheckListGroups)
                {
                    var checklistgroup = db.CheckListGroups.Find(tmpCheckListGroup.Id);
                    if (checklistgroup.Type.Trim().ToLower() != "pl")
                    {
                        groupwise_items = db.ChecklistItems.Where(x => x.IdCheckListGroup == tmpCheckListGroup.Id).ToList(); //after filteration of items based on application and permit from that filter items which matches checklistGroup
                        foreach (var b in groupwise_items)
                        {
                            groupwise_final_items.Add(b);
                        }
                    }
                }
                int? Id_applicationtype = db.JobApplications.FirstOrDefault(x => x.Id == jobChecklistHeaderDTO.IdJobApplication).IdJobApplicationType;
                List<ChecklistItem> permitwisewise_items = new List<ChecklistItem>();
                List<ChecklistItem> applicationwise_items = new List<ChecklistItem>();

                var Item_jobapplication_wise = groupwise_final_items.Where(x => x.JobApplicationTypes.Any(ctdb => ctdb.Id == Id_applicationtype));
                //get items matches with application type        

                foreach (var c in Item_jobapplication_wise)
                {
                    applicationwise_items.Add(c);
                }

                //remove items which is not matched with given permit ex : MH
                List<ChecklistItem> removeitemswithoutsamepermit = new List<ChecklistItem>();

                Expression<Func<ChecklistItem, bool>> worktypeExpression = null;

                foreach (var WorkTypeId in WorkTypeIds)
                {
                    if (WorkTypeId != null)
                    {
                        if (WorkTypeIds.IndexOf(WorkTypeId) == 0)
                        {
                            // worktypeExpression = e => !string.IsNullOrWhiteSpace(e.IdJobWorkTypes) && !e.IdJobWorkTypes.Contains(Convert.ToString(WorkTypeId));
                            worktypeExpression = e => !string.IsNullOrWhiteSpace(e.IdJobWorkTypes)
                            && !((e.IdJobWorkTypes.Split(',')).ToArray().Contains(Convert.ToString(WorkTypeId)));
                        }
                        else
                        {
                            if (worktypeExpression != null)
                            {
                                var compile = worktypeExpression.Compile();
                                // worktypeExpression = e => compile(e) && !e.IdJobWorkTypes.Contains(Convert.ToString(WorkTypeId));
                                worktypeExpression = e => compile(e) && !((e.IdJobWorkTypes.Split(',')).ToArray().Contains(Convert.ToString(WorkTypeId)));
                            }

                        }
                    }
                }

                if (worktypeExpression != null)
                    removeitemswithoutsamepermit = applicationwise_items.Where(worktypeExpression.Compile()).ToList();

                //    //from above data get only that items which matches with workpermit
                foreach (var c in removeitemswithoutsamepermit)
                {
                    applicationwise_items.Remove(c);
                }
                // }
                // get only that items which matches with workpermit


                foreach (var WorkTypeId in WorkTypeIds)
                {
                    if (WorkTypeId != null)
                    {
                        var Item_permit_wise = groupwise_final_items.Where(x => x.JobWorkTypes.Any(ctdb => ctdb.Id == WorkTypeId.Value)).ToList();
                        //   
                        foreach (var c in Item_permit_wise)
                        {
                            permitwisewise_items.Add(c);
                        }
                    }
                }

                List<ChecklistItem> removeitemswithoutsameapplication = new List<ChecklistItem>();

                //Problem of multiple ids
                removeitemswithoutsameapplication = permitwisewise_items.Where(x => !string.IsNullOrWhiteSpace(x.IdJobApplicationTypes) && !x.IdJobApplicationTypes.Contains(Convert.ToString(Id_applicationtype))).ToList();

                //remove items which doesn't match with application and not null
                foreach (var c in removeitemswithoutsameapplication)
                {
                    permitwisewise_items.Remove(c);
                }
                //concat this later
                final_items = applicationwise_items;
                final_items.AddRange(permitwisewise_items);
                var Distinctfinalitem = final_items.Distinct().ToList();

                //Propertywise item start logic
                List<ChecklistAddressPropertyMaping> ChecklistAddressPropertyMaping1 = new List<ChecklistAddressPropertyMaping>();
                foreach (var item in groupwise_final_items)
                {
                    var iteminpropertmapping = db.ChecklistAddressPropertyMaping.Where(x => x.IdChecklistItem == item.Id).ToList();//fetch  final items' property from addrespropertmapping table which comes under slected application,permit and group
                    foreach (var a in iteminpropertmapping)
                    {
                        ChecklistAddressPropertyMaping1.Add(a);
                    }
                }

                List<ChecklistAddressPropertyMaping> ChecklistAddressPropertyMaping2 = new List<ChecklistAddressPropertyMaping>();//9,17,3
                foreach (var addressproperty in ListOfProperties)
                {
                    var mappeditems = ChecklistAddressPropertyMaping1.Where(x => x.IdChecklistAddressProperty == addressproperty);//fetch omly those items which matches with jobs properties
                    foreach (var b in mappeditems)
                    {
                        if (b.IdChecklistAddressProperty == 3) //check if it has ownertype property
                        {
                            if (Convert.ToInt32(b.Value) == rfpAddress.IdOwnerType)
                            {
                                ChecklistAddressPropertyMaping2.Add(b);
                                continue; ;
                            }
                            else
                                continue;
                        }
                        if (b.IdChecklistAddressProperty == 4)//check if it has special district property
                        {
                            string[] multipleDistricts = b.Value.Split(',');

                            foreach (string item in multipleDistricts)
                            {
                                if (rfpAddress.SpecialDistrict.Contains(item.Trim()))

                                {
                                    ChecklistAddressPropertyMaping2.Add(b);
                                    continue;
                                }
                            }

                            continue;
                        }
                        ChecklistAddressPropertyMaping2.Add(b);
                    }
                }

                List<ChecklistAddressPropertyMaping> ChecklistAddressPropertyMaping4 = new List<ChecklistAddressPropertyMaping>(); //all from distinct final item
                foreach (var item in Distinctfinalitem)
                {
                    var itemsinaddressproperty = db.ChecklistAddressPropertyMaping.Where(x => x.IdChecklistItem == item.Id).ToList();

                    foreach (var a in itemsinaddressproperty)
                    {
                        ChecklistAddressPropertyMaping4.Add(a);
                    }
                }

                List<ChecklistAddressPropertyMaping> ChecklistAddressPropertyMapingRemoveItems = new List<ChecklistAddressPropertyMaping>();

                Expression<Func<ChecklistAddressPropertyMaping, bool>> worktypeExpression2 = null;
                if (ListOfProperties != null && ListOfProperties.Count > 0)
                {
                    foreach (var property in ListOfProperties)
                    {
                        //if (property == 3 || property == 4)//check if it has special district property
                        //{
                        //    continue;
                        //}

                        if (worktypeExpression2 == null)
                        {
                            worktypeExpression2 = e => e.IdChecklistAddressProperty != null && e.IdChecklistAddressProperty != property;
                        }
                        else
                        {
                            var compile = worktypeExpression2.Compile();
                            worktypeExpression2 = e => compile(e) && e.IdChecklistAddressProperty != property;
                        }
                        //if (property == 3 || property == 4)//check if it has special district property
                        //{
                        //    continue;
                        //}
                    }

                    if (worktypeExpression2 != null)
                    {
                        ChecklistAddressPropertyMapingRemoveItems = ChecklistAddressPropertyMaping4.Where(worktypeExpression2.Compile()).ToList();
                    }
                    var removeItemsAddressProperty3 = ChecklistAddressPropertyMaping4
                        .Where(e => e.IdChecklistAddressProperty == 3 && e.Value != Convert.ToString(rfpAddress.IdOwnerType)).ToList();

                    var removeItemsAddressProperty4 = ChecklistAddressPropertyMaping4
                        .Where(e => e.IdChecklistAddressProperty == 4 && !e.Value.Contains(Convert.ToString(rfpAddress.SpecialDistrict))).ToList();

                    ChecklistAddressPropertyMapingRemoveItems.AddRange(removeItemsAddressProperty3);
                    ChecklistAddressPropertyMapingRemoveItems.AddRange(removeItemsAddressProperty4);  //2184 should come here
                }
                else
                {
                    ChecklistAddressPropertyMapingRemoveItems.AddRange(ChecklistAddressPropertyMaping4);
                }

                foreach (var item in ChecklistAddressPropertyMapingRemoveItems)
                {
                    var items = db.ChecklistItems.Where(cl => cl.Id == item.IdChecklistItem);
                    foreach (var a in items)
                        Distinctfinalitem.Remove(a);
                }
                //Propertywise item End logic
                List<ChecklistItem> Propertywise_Items1 = new List<ChecklistItem>();
                //check this

                foreach (var c in ChecklistAddressPropertyMaping2)
                {
                    var items = db.ChecklistItems.Where(cl => cl.Id == c.IdChecklistItem);
                    foreach (var i in items)
                        Propertywise_Items1.Add(i);
                }

                List<ChecklistItem> removeitemswithoutsameapplication1 = new List<ChecklistItem>();
                removeitemswithoutsameapplication1 = Propertywise_Items1.Where(x => (!string.IsNullOrWhiteSpace(x.IdJobApplicationTypes) && !x.IdJobApplicationTypes.Contains(Convert.ToString(Id_applicationtype)))).ToList();

                Expression<Func<ChecklistItem, bool>> worktypeExpression1 = null;
                foreach (var WorkTypeId in WorkTypeIds)
                {
                    if (WorkTypeIds.IndexOf(WorkTypeId) == 0)
                    {
                        // worktypeExpression1 = e => !string.IsNullOrWhiteSpace(e.IdJobWorkTypes) && !e.IdJobWorkTypes.Contains(Convert.ToString(WorkTypeId));
                        worktypeExpression1 = e => !string.IsNullOrWhiteSpace(e.IdJobWorkTypes) && !((e.IdJobWorkTypes.Split(',')).ToArray().Contains(Convert.ToString(WorkTypeId)));
                    }
                    else
                    {
                        if (worktypeExpression1 != null)
                        {
                            var compile = worktypeExpression1.Compile();
                            //worktypeExpression1 = e => compile(e) && !e.IdJobWorkTypes.Contains(Convert.ToString(WorkTypeId));
                            worktypeExpression1 = e => compile(e) && !((e.IdJobWorkTypes.Split(',')).ToArray().Contains(Convert.ToString(WorkTypeId)));

                        }
                    }
                }

                List<ChecklistItem> removeitemswithoutsamepermit1 = new List<ChecklistItem>();
                if (worktypeExpression1 != null)
                    removeitemswithoutsamepermit1 = Propertywise_Items1.Where(worktypeExpression1.Compile()).ToList();

                foreach (var item in removeitemswithoutsameapplication1)
                {
                    Propertywise_Items1.Remove(item);
                }
                foreach (var item in removeitemswithoutsamepermit1)
                {
                    Propertywise_Items1.Remove(item);
                }
                Distinctfinalitem.AddRange(Propertywise_Items1);
                var distinct_Final_Items1 = Distinctfinalitem.Distinct().ToList();



                //End of logic
                #endregion


                /*Plumbing-Inspection table-insert*/
                /*Plumbing-Inspection table-insert*/

                foreach (var tmpCheckListGroup in jobChecklistHeaderDTO.CheckListGroups)
                {
                    CheckListGroup checklistgroup = db.CheckListGroups.Find(tmpCheckListGroup.Id);
                    JobChecklistHeader JChecklistheader = db.JobChecklistHeaders.Find(jobChecklistHeader.IdJobCheckListHeader);
                    JobPlumbingInspection jobChecklistPlumbingInspection;
                    List<ChecklistItem> lstPLitems = new List<ChecklistItem>();
                    if (checklistgroup.Type == "PL")
                    {

                        var result = db.JobChecklistGroups.Where(y => (y.IdCheckListGroup == checklistgroup.Id) &&
                                                                   y.IdJobCheckListHeader == JChecklistheader.IdJobCheckListHeader).ToList();
                        jobChecklistPlumbingInspection = new JobPlumbingInspection();
                        foreach (var floorItem in jobChecklistHeaderDTO.JobPlumbingChecklistFloors.OrderBy(x => x.FloonNumber))
                        {
                            JobPlumbingChecklistFloors jobPlumbingCheckListFloor = new JobPlumbingChecklistFloors();
                            jobPlumbingCheckListFloor.FloonNumber = floorItem.FloonNumber;
                            jobPlumbingCheckListFloor.FloorDisplayOrder = floorItem.FloorDisplayOrder;
                            jobPlumbingCheckListFloor.IdJobCheckListHeader = jobChecklistHeader.IdJobCheckListHeader;
                            db.JobPlumbingChecklistFloors.Add(jobPlumbingCheckListFloor);
                            db.SaveChanges();
                            int AccountAddressID = jobPlumbingCheckListFloor.Id;

                            foreach (var inspectionTypeItem in floorItem.InspectionType)
                            {
                                jobChecklistPlumbingInspection.IdJobChecklistGroup = result[0].Id;
                                jobChecklistPlumbingInspection.IdChecklistItem = inspectionTypeItem.Id;
                                jobChecklistPlumbingInspection.Status = 1;
                                jobChecklistPlumbingInspection.IsActive = true;
                                jobChecklistPlumbingInspection.WorkOrderNo = "";
                                jobChecklistPlumbingInspection.CreatedBy = employee.Id;
                                jobChecklistPlumbingInspection.NextInspection = DateTime.UtcNow;
                                jobChecklistPlumbingInspection.Result = "3";
                                jobChecklistPlumbingInspection.IsRequiredTCO_CO = true;
                                jobChecklistPlumbingInspection.CreatedDate = DateTime.UtcNow;
                                jobChecklistPlumbingInspection.LastModifiedDate = DateTime.UtcNow;
                                jobChecklistPlumbingInspection.LastModifiedBy = employee.Id;
                                jobChecklistPlumbingInspection.IdJobPlumbingCheckListFloors = AccountAddressID;
                                jobChecklistPlumbingInspection.PlumbingInspectionDisplayOrder = db.ChecklistItems.Find(inspectionTypeItem.Id).DisplayOrderPlumbingInspection;

                                db.JobPlumbingInspection.Add(jobChecklistPlumbingInspection);
                                db.SaveChanges();
                            }
                        }

                    }
                }


                //start of Add in jobchecklistitem table
                List<JobChecklistItemDetail> lstjobChecklistItemDetail = new List<JobChecklistItemDetail>();

                foreach (var final_item in distinct_Final_Items1)
                {
                    //  fetch data from groupdetailtable according to itemid and header id and save in itemdetail table
                    JobChecklistItemDetail jobChecklistItemDetail = new JobChecklistItemDetail();
                    jobChecklistItemDetail.IdChecklistItem = final_item.Id;

                    var result = (from ca in db.JobChecklistGroups
                                  where ca.IdJobCheckListHeader == newJobChecklistHeader.IdJobCheckListHeader && ca.IdCheckListGroup == final_item.IdCheckListGroup
                                  select new
                                  {
                                      ca.Id,
                                  }).ToList();
                    jobChecklistItemDetail.IdJobChecklistGroup = result[0].Id;
                    jobChecklistItemDetail.CreatedDate = DateTime.UtcNow;
                    jobChecklistItemDetail.LastModifiedDate = DateTime.UtcNow;
                    jobChecklistItemDetail.Status = 1;
                    jobChecklistItemDetail.PartyResponsible1 = 1;
                    lstjobChecklistItemDetail.Add(jobChecklistItemDetail);

                }
                db.JobChecklistItemDetails.AddRange(lstjobChecklistItemDetail);
                if (lstjobChecklistItemDetail.Count > 0)
                {
                    db.SaveChanges();

                }
                return Ok(MapJobChecklistToDTO(newJobChecklistHeader));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobChecklistManualItem))]
        [Route("api/checklist/PostJobChecklistManualItem")]
        public IHttpActionResult PostJobChecklistManualItem(JobChecklistManualItem jobChecklistManualItem)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                var Groupdetail = db.JobChecklistGroups.Find(jobChecklistManualItem.IdJobChecklistGroup);
                string grouptype = Groupdetail.ChecklistGroups.Type;
                var IdPLgroups = db.CheckListGroups.Where(x => x.Type.ToLower() == "pl" && x.IsActive == true).Select(y => y.Id).ToList();
                int? lastid = db.ChecklistItems.Where(x => IdPLgroups.Contains(x.IdCheckListGroup)).OrderByDescending(x => x.Id).FirstOrDefault().DisplayOrderPlumbingInspection;
                ChecklistItem checklistItem = new ChecklistItem();
                checklistItem.Name = jobChecklistManualItem.ItemName;
                checklistItem.IdCheckListGroup = Groupdetail.IdCheckListGroup;
                checklistItem.IsActive = true;
                checklistItem.IsUserfillable = true;
                checklistItem.CreatedDate = DateTime.UtcNow;
                checklistItem.LastModifiedDate = DateTime.UtcNow;
                if (lastid != null)
                    checklistItem.DisplayOrderPlumbingInspection = lastid + 1;
                if (employee != null)
                {
                    checklistItem.CreatedBy = employee.Id;
                }
                db.ChecklistItems.Add(checklistItem);
                db.SaveChanges();
                var ManuallyaddeditemID = (from c in db.ChecklistItems
                                           orderby c.Id descending
                                           select c.Id).First();
                if (grouptype.ToLower() != "pl")
                {

                    JobChecklistItemDetail jobChecklistItemDetail = new JobChecklistItemDetail();
                    jobChecklistItemDetail.IdChecklistItem = ManuallyaddeditemID;
                    jobChecklistItemDetail.IdJobChecklistGroup = jobChecklistManualItem.IdJobChecklistGroup;
                    jobChecklistItemDetail.Status = 1;
                    jobChecklistItemDetail.CreatedDate = DateTime.UtcNow;
                    jobChecklistItemDetail.LastModifiedDate = DateTime.UtcNow;
                    db.JobChecklistItemDetails.Add(jobChecklistItemDetail);
                    if (employee != null)
                    {
                        jobChecklistItemDetail.CreatedBy = employee.Id;
                    }
                    int JobChecklistGroupsID = db.JobChecklistItemDetails.Find(jobChecklistItemDetail.Id).IdJobChecklistGroup;
                    var jobchecklistheaderid = db.JobChecklistGroups.Where(x => x.Id == JobChecklistGroupsID).FirstOrDefault().IdJobCheckListHeader;

                    JobChecklistHeader jobChecklistHeader = db.JobChecklistHeaders.Find(jobchecklistheaderid);
                    jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;
                    db.SaveChanges();
                }
                else
                {
                    int headerid = db.JobChecklistGroups.Find(jobChecklistManualItem.IdJobChecklistGroup).IdJobCheckListHeader;
                    JobPlumbingInspection jobPlumbingInspection = new JobPlumbingInspection();
                    jobPlumbingInspection.IdChecklistItem = ManuallyaddeditemID;
                    jobPlumbingInspection.IdJobChecklistGroup = jobChecklistManualItem.IdJobChecklistGroup;
                    jobPlumbingInspection.IdJobPlumbingCheckListFloors = jobChecklistManualItem.IdJobPlumbingCheckListFloors;
                    jobPlumbingInspection.IsActive = true;
                    jobPlumbingInspection.LastModifiedDate = DateTime.UtcNow;
                    jobPlumbingInspection.CreatedDate = DateTime.UtcNow;
                    jobPlumbingInspection.LastModifiedDate = DateTime.UtcNow;
                    jobPlumbingInspection.Result = "3";
                    jobPlumbingInspection.PlumbingInspectionDisplayOrder = lastid + 1;

                    db.JobPlumbingInspection.Add(jobPlumbingInspection);
                    if (employee != null)
                    {
                        jobPlumbingInspection.CreatedBy = employee.Id;
                        jobPlumbingInspection.LastModifiedBy = employee.Id;
                    }
                    int JobChecklistGroupsID = db.JobPlumbingInspection.Find(jobPlumbingInspection.IdJobPlumbingInspection).IdJobChecklistGroup;
                    var jobchecklistheaderid = db.JobChecklistGroups.Where(x => x.Id == JobChecklistGroupsID).FirstOrDefault().IdJobCheckListHeader;
                    JobChecklistHeader jobChecklistHeader = db.JobChecklistHeaders.Find(jobchecklistheaderid);
                    jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;

                    db.JobPlumbingInspection.Add(jobPlumbingInspection);
                    db.SaveChanges();

                }

                return Ok();
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/CheckList/GetChecklistItemdropdownGroupTypewise/{IdJobchecklistGroup}")]
        public IHttpActionResult GetChecklistItemdropdownGroupTypewise(int IdJobchecklistGroup)
        {
            int idChecklistgroup = this.db.JobChecklistGroups.Find(IdJobchecklistGroup).IdCheckListGroup;
            string type = this.db.CheckListGroups.Find(idChecklistgroup).Type;
            var checklistgroups = this.db.CheckListGroups.Where(x => x.Type == type);
            List<ChecklistItem> lstChecklistItem = new List<ChecklistItem>();
            foreach (var a in checklistgroups)
            {
                var items = this.db.ChecklistItems.Where(x => x.IdCheckListGroup == a.Id && x.IsUserfillable == false && x.IsActive == true).
                     ToList();
                lstChecklistItem.AddRange(items);

            }

            return Ok(lstChecklistItem);
        }


        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/CheckList/GetChecklistItemdropdownPrmitTypewise/{IdJobchecklistCheckListGroup}/{IdPermits}")]
        public IHttpActionResult GetChecklistItemdropdownPrmitTypewise(int IdJobchecklistCheckListGroup, string IdPermits)
        {
            List<string> Permits = IdPermits.Split(',').ToList();
            List<ChecklistItem> lstChecklistItem = new List<ChecklistItem>();
            var IdCheckListGroup = db.JobChecklistGroups.Where(x => x.Id == IdJobchecklistCheckListGroup).Select(y => y.IdCheckListGroup).FirstOrDefault();
            foreach (var a in Permits)
            {
                var items = this.db.ChecklistItems.Where(x => x.IdCheckListGroup == IdCheckListGroup && x.IdJobWorkTypes.Split(',').ToArray().Contains(a) && x.IsUserfillable == false && x.IsActive == true).
                     ToList();
                lstChecklistItem.AddRange(items);
            }

            return Ok(lstChecklistItem);
        }

        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/CheckList/GetChecklistItemdropdown/{IdCheckListGroup}/{WorkTypeIds}")]
        public IHttpActionResult GetChecklistItemdropdown(int IdCheckListGroup, string WorkTypeIds)
        {
            List<ChecklistItem> lstChecklistItem = new List<ChecklistItem>();

            var items = this.db.ChecklistItems.Where(x => x.IdCheckListGroup == IdCheckListGroup && x.IsUserfillable == false && x.IsActive == true).OrderBy(x => x.DisplayOrderPlumbingInspection).
                 ToList();
            string[] Para_worktypeIds = WorkTypeIds.Split(',');


            List<string> IdJobWorkTypes = items.Select(x => x.IdJobWorkTypes).ToList();
            foreach (var p in Para_worktypeIds)
            {
                int Idworktype = Convert.ToInt32(p);
                if (IdCheckListGroup == 8)
                {
                    string code = db.JobWorkTypes.Where(x => x.Id == Idworktype).Select(y => y.Code).FirstOrDefault();
                    if (code.ToLower().Trim() == "pl" | code.ToLower().Trim() == "sp" | code.ToLower().Trim() == "sd")
                    {
                        foreach (var i in items)
                        {
                            if (i.IdJobWorkTypes != null)
                            {
                                if (i.IdJobWorkTypes.Split(',').ToArray().Contains(p))
                                {
                                    lstChecklistItem.Add(i);
                                }
                            }

                        }
                    }
                }
                else
                {
                    foreach (var i in items)
                    {
                        if (i.IdJobWorkTypes != null)
                        {
                            if (i.IdJobWorkTypes.Split(',').ToArray().Contains(p))
                            {
                                lstChecklistItem.Add(i);
                            }
                        }

                    }
                }
            }
            return Ok(lstChecklistItem.Distinct());
        }


        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobChecklistItemDueDateDTO))]
        [Route("api/checklist/PostChecklistDueDate")]
        public IHttpActionResult PostChecklistitemDueDate(JobChecklistItemDueDateDTO jobChecklistItemDueDateDTO)
        {

            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                JobChecklistItemDueDate jobChecklistItemDueDate = new JobChecklistItemDueDate();
                jobChecklistItemDueDate.IdJobChecklistItemDetail = jobChecklistItemDueDateDTO.IdJobChecklistItemDetail;
                jobChecklistItemDueDate.DueDate = jobChecklistItemDueDateDTO.DueDate;

                jobChecklistItemDueDate.CreatedDate = DateTime.UtcNow;
                jobChecklistItemDueDate.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobChecklistItemDueDate.CreatedBy = employee.Id;
                }

                db.JobChecklistItemDueDate.Add(jobChecklistItemDueDate);
                int JobChecklistGroupsID = db.JobChecklistItemDetails.Find(jobChecklistItemDueDateDTO.IdJobChecklistItemDetail).IdJobChecklistGroup;
                var jobchecklistheaderid = db.JobChecklistGroups.Where(x => x.Id == JobChecklistGroupsID).FirstOrDefault().IdJobCheckListHeader;

                JobChecklistHeader jobChecklistHeader = db.JobChecklistHeaders.Find(jobchecklistheaderid);
                jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobChecklistItemDueDateDTO))]
        [Route("api/checklist/PostChecklistComment")]
        public IHttpActionResult PostChecklistComment(JobChecklistCommentHistoryDTO jobChecklistCommentHistoryDTO)
        {

            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                JobCheckListCommentHistory jobCheckListCommentHistory = new JobCheckListCommentHistory();
                jobCheckListCommentHistory.IdJobChecklistItemDetail = jobChecklistCommentHistoryDTO.IdJobChecklistItemDetail;
                jobCheckListCommentHistory.Description = jobChecklistCommentHistoryDTO.Description;
                jobCheckListCommentHistory.Isinternal = jobChecklistCommentHistoryDTO.Isinternal;
                jobCheckListCommentHistory.CreatedDate = DateTime.UtcNow;
                jobCheckListCommentHistory.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobCheckListCommentHistory.CreatedBy = employee.Id;
                    jobCheckListCommentHistory.LastModifiedBy = employee.Id;
                }

                db.JobCheckListCommentHistories.Add(jobCheckListCommentHistory);
                int JobChecklistGroupsID = db.JobChecklistItemDetails.Find(jobChecklistCommentHistoryDTO.IdJobChecklistItemDetail).IdJobChecklistGroup;
                var jobchecklistheaderid = db.JobChecklistGroups.Where(x => x.Id == JobChecklistGroupsID).FirstOrDefault().IdJobCheckListHeader;

                JobChecklistHeader jobChecklistHeader = db.JobChecklistHeaders.Find(jobchecklistheaderid);
                jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;
                db.SaveChanges();
                return Ok("Comment added successfully");
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobChecklistProgressNoteDTO))]
        [Route("api/checklist/PostchecklistProgressNote")]
        public IHttpActionResult PostChecklistProgressNote(JobChecklistProgressNoteDTO jobChecklistItemProgressNoteDTO)
        {

            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                JobCheckListProgressNoteHistory jobCheckListProgressNoteHistories = new JobCheckListProgressNoteHistory();
                jobCheckListProgressNoteHistories.IdJobChecklistItemDetail = jobChecklistItemProgressNoteDTO.IdJobChecklistItemDetail;
                jobCheckListProgressNoteHistories.Description = jobChecklistItemProgressNoteDTO.Description;
                jobCheckListProgressNoteHistories.CreatedDate = DateTime.UtcNow;
                jobCheckListProgressNoteHistories.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobCheckListProgressNoteHistories.CreatedBy = employee.Id;
                }

                db.JobCheckListProgressNoteHistories.Add(jobCheckListProgressNoteHistories);
                int JobChecklistGroupsID = db.JobChecklistItemDetails.Find(jobChecklistItemProgressNoteDTO.IdJobChecklistItemDetail).IdJobChecklistGroup;
                var jobchecklistheaderid = db.JobChecklistGroups.Where(x => x.Id == JobChecklistGroupsID).FirstOrDefault().IdJobCheckListHeader;

                JobChecklistHeader jobChecklistHeader = db.JobChecklistHeaders.Find(jobchecklistheaderid);
                jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }


        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobPlumbingInspectionProgressNoteDTO))]
        [Route("api/checklist/PostPlumbingInsceptionProgressNote")]
        public IHttpActionResult PostPlumbingInsceptionProgressNote(JobPlumbingInspectionProgressNoteDTO jobPlumbingInsceptionNoteDTO)
        {

            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                JobPlumbingInspectionProgressNoteHistory jobPlumbingInspectionProgressNoteHistories = new JobPlumbingInspectionProgressNoteHistory();
                jobPlumbingInspectionProgressNoteHistories.IdJobPlumbingInspection = jobPlumbingInsceptionNoteDTO.IdJobPlumbingInspection;
                jobPlumbingInspectionProgressNoteHistories.Description = jobPlumbingInsceptionNoteDTO.Description;
                jobPlumbingInspectionProgressNoteHistories.CreatedDate = DateTime.UtcNow;
                jobPlumbingInspectionProgressNoteHistories.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobPlumbingInspectionProgressNoteHistories.CreatedBy = employee.Id;
                }

                db.JobPlumbingInspectionProgressNoteHistory.Add(jobPlumbingInspectionProgressNoteHistories);
                int JobChecklistGroupsID = db.JobPlumbingInspection.Find(jobPlumbingInsceptionNoteDTO.IdJobPlumbingInspection).IdJobChecklistGroup;
                var jobchecklistheaderid = db.JobChecklistGroups.Where(x => x.Id == JobChecklistGroupsID).FirstOrDefault().IdJobCheckListHeader;

                JobChecklistHeader jobChecklistHeader = db.JobChecklistHeaders.Find(jobchecklistheaderid);
                jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }



        [Route("api/Checklist/PlumbingInspectionProgressNoteHistory/{IdJobPlumbingInspections}")]
        public IHttpActionResult GetPlumbingInspectionProgressNoteHistory(int IdJobPlumbingInspections)
        {
            var jobPlumbingInspectionCommentHistory = db.JobPlumbingInspectionProgressNoteHistory.Where(x => x.IdJobPlumbingInspection == IdJobPlumbingInspections);
            if (jobPlumbingInspectionCommentHistory == null)
            {
                return this.NotFound();
            }
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            var result = db.JobPlumbingInspectionProgressNoteHistory
                .Include("LastModifiedByEmployee")
                .Include("CreatedByEmployee")
                .Where(x => x.IdJobPlumbingInspection == IdJobPlumbingInspections)
                .AsEnumerable()
                .Select(JobPlumbingInspectionCommentHistory => new
                {
                    Id = JobPlumbingInspectionCommentHistory.Id,
                    IdJobChecklistItemDetail = JobPlumbingInspectionCommentHistory.IdJobPlumbingInspection,
                    Description = JobPlumbingInspectionCommentHistory.Description,
                    CreatedBy = JobPlumbingInspectionCommentHistory.CreatedBy,
                    LastModifiedBy = JobPlumbingInspectionCommentHistory.LastModifiedBy != null ? JobPlumbingInspectionCommentHistory.LastModifiedBy : JobPlumbingInspectionCommentHistory.CreatedBy,
                    CreatedByEmployee = JobPlumbingInspectionCommentHistory.CreatedByEmployee != null ? JobPlumbingInspectionCommentHistory.CreatedByEmployee.FirstName + " " + JobPlumbingInspectionCommentHistory.CreatedByEmployee.LastName : string.Empty,
                    LastModifiedByEmployee = JobPlumbingInspectionCommentHistory.LastModifiedByEmployee != null ? JobPlumbingInspectionCommentHistory.LastModifiedByEmployee.FirstName + " " + JobPlumbingInspectionCommentHistory.LastModifiedByEmployee.LastName : (JobPlumbingInspectionCommentHistory.CreatedByEmployee != null ? JobPlumbingInspectionCommentHistory.CreatedByEmployee.FirstName + " " + JobPlumbingInspectionCommentHistory.CreatedByEmployee.LastName : string.Empty),
                    CreatedDate = JobPlumbingInspectionCommentHistory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobPlumbingInspectionCommentHistory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : JobPlumbingInspectionCommentHistory.CreatedDate,
                    LastModifiedDate = JobPlumbingInspectionCommentHistory.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobPlumbingInspectionCommentHistory.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (JobPlumbingInspectionCommentHistory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobPlumbingInspectionCommentHistory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : JobPlumbingInspectionCommentHistory.CreatedDate)
                }).OrderByDescending(x => x.LastModifiedDate);

            return Ok(result);
        }

        [Route("api/Checklist/{IdJobChecklistItemDetail}/ChecklistcommentHistory")]
        public IHttpActionResult GetChecklistcommentHistory(int IdJobChecklistItemDetail)
        {

            var jobCheckListCommentHistory = db.JobCheckListCommentHistories.Where(x => x.IdJobChecklistItemDetail == IdJobChecklistItemDetail && x.Isinternal == true);
            if (jobCheckListCommentHistory == null)
            {
                return this.NotFound();
            }

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            var result = db.JobCheckListCommentHistories
                .Include("LastModifiedByEmployee")
                .Include("CreatedByEmployee")
                .Where(x => x.IdJobChecklistItemDetail == IdJobChecklistItemDetail)
                .AsEnumerable()
                .Select(JobCheckListCommentHistory => new
                {
                    Id = JobCheckListCommentHistory.Id,
                    IdJobChecklistItemDetail = JobCheckListCommentHistory.IdJobChecklistItemDetail,
                    Description = JobCheckListCommentHistory.Description,
                    CreatedBy = JobCheckListCommentHistory.CreatedBy,
                    LastModifiedBy = JobCheckListCommentHistory.LastModifiedBy != null ? JobCheckListCommentHistory.LastModifiedBy : JobCheckListCommentHistory.CreatedBy,
                    CreatedByEmployee = JobCheckListCommentHistory.CreatedByEmployee != null ? JobCheckListCommentHistory.CreatedByEmployee.FirstName + " " + JobCheckListCommentHistory.CreatedByEmployee.LastName : string.Empty,
                    LastModifiedByEmployee = JobCheckListCommentHistory.LastModifiedByEmployee != null ? JobCheckListCommentHistory.LastModifiedByEmployee.FirstName + " " + JobCheckListCommentHistory.LastModifiedByEmployee.LastName : (JobCheckListCommentHistory.CreatedByEmployee != null ? JobCheckListCommentHistory.CreatedByEmployee.FirstName + " " + JobCheckListCommentHistory.CreatedByEmployee.LastName : string.Empty),
                    CreatedDate = JobCheckListCommentHistory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobCheckListCommentHistory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : JobCheckListCommentHistory.CreatedDate,
                    LastModifiedDate = JobCheckListCommentHistory.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobCheckListCommentHistory.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (JobCheckListCommentHistory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobCheckListCommentHistory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : JobCheckListCommentHistory.CreatedDate)
                }).OrderByDescending(x => x.LastModifiedDate);

            return Ok(result);
        }


        [Route("api/Checklist/ChecklistProgressNoteHistory/{IdJobChecklistItemDetail}")]
        public IHttpActionResult GetChecklistProgressNoteHistory(int IdJobChecklistItemDetail)
        {

            var jobCheckListCommentHistory = db.JobCheckListProgressNoteHistories.Where(x => x.IdJobChecklistItemDetail == IdJobChecklistItemDetail);
            if (jobCheckListCommentHistory == null)
            {
                return this.NotFound();
            }

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            var result = db.JobCheckListProgressNoteHistories
                .Include("LastModifiedByEmployee")
                .Include("CreatedByEmployee")
                .Where(x => x.IdJobChecklistItemDetail == IdJobChecklistItemDetail)
                .AsEnumerable()
                .Select(JobCheckListCommentHistory => new
                {
                    Id = JobCheckListCommentHistory.Id,
                    IdJobChecklistItemDetail = JobCheckListCommentHistory.IdJobChecklistItemDetail,
                    Description = JobCheckListCommentHistory.Description,
                    CreatedBy = JobCheckListCommentHistory.CreatedBy,
                    LastModifiedBy = JobCheckListCommentHistory.LastModifiedBy != null ? JobCheckListCommentHistory.LastModifiedBy : JobCheckListCommentHistory.CreatedBy,
                    CreatedByEmployee = JobCheckListCommentHistory.CreatedByEmployee != null ? JobCheckListCommentHistory.CreatedByEmployee.FirstName + " " + JobCheckListCommentHistory.CreatedByEmployee.LastName : string.Empty,
                    LastModifiedByEmployee = JobCheckListCommentHistory.LastModifiedByEmployee != null ? JobCheckListCommentHistory.LastModifiedByEmployee.FirstName + " " + JobCheckListCommentHistory.LastModifiedByEmployee.LastName : (JobCheckListCommentHistory.CreatedByEmployee != null ? JobCheckListCommentHistory.CreatedByEmployee.FirstName + " " + JobCheckListCommentHistory.CreatedByEmployee.LastName : string.Empty),
                    CreatedDate = JobCheckListCommentHistory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobCheckListCommentHistory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : JobCheckListCommentHistory.CreatedDate,
                    LastModifiedDate = JobCheckListCommentHistory.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobCheckListCommentHistory.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (JobCheckListCommentHistory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobCheckListCommentHistory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : JobCheckListCommentHistory.CreatedDate)
                }).OrderByDescending(x => x.LastModifiedDate);

            return Ok(result);
        }


        [Route("api/Checklist/ReferenceNote/{IdJobChecklistItemDetail}")]

        public IHttpActionResult GetReferenceNote(int IdJobChecklistItemDetail)
        {

            JobChecklistItemDetail jobChecklistItemDetails = db.JobChecklistItemDetails.Where(x => x.Id == IdJobChecklistItemDetail).FirstOrDefault();
            if (jobChecklistItemDetails == null)
            {
                return this.NotFound();
            }

            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

            var result = db.ChecklistItems.Where(x => x.Id == jobChecklistItemDetails.IdChecklistItem).AsEnumerable()
                .Select(checklistItem => new
                {
                    ReferenceNote = checklistItem.ReferenceNote,
                    ReferenceDoc = checklistItem.ReferenceDocuments.Select(checklistItem2 => new { FileName = checklistItem2.FileName, Name = checklistItem2.Name, ContentPath = APIUrl + "/" + Convert.ToString(Properties.Settings.Default.UploadedDocumentPath) + "/" + Convert.ToString(checklistItem2.Id) + "_" + checklistItem2.ContentPath }),
                    ExternalLink = checklistItem.ExternalReferenceLink
                });

            if (result.Count() > 0)
                return Ok(result);
            else
                return this.NotFound();

        }

        [Route("api/Checklist/PlumbingReferenceNote/{IdJobPlumbingInspections}")]

        public IHttpActionResult GetPlumbingReferenceNote(int IdJobPlumbingInspections)
        {
            JobPlumbingInspection jobPlumbingInspections = db.JobPlumbingInspection.Where(x => x.IdJobPlumbingInspection == IdJobPlumbingInspections).FirstOrDefault();
            if (jobPlumbingInspections == null)
            {
                return this.NotFound();
            }

            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

            var result = db.ChecklistItems.Where(x => x.Id == jobPlumbingInspections.IdChecklistItem).AsEnumerable()
                .Select(checklistItem => new
                {
                    ReferenceNote = checklistItem.ReferenceNote,
                    ReferenceDoc = checklistItem.ReferenceDocuments.Select(checklistItem2 => new { FileName = checklistItem2.FileName, Name = checklistItem2.Name, ContentPath = APIUrl + "/" + Convert.ToString(Properties.Settings.Default.UploadedDocumentPath) + "/" + Convert.ToString(checklistItem2.Id) + "_" + checklistItem2.ContentPath }),
                    ExternalLink = checklistItem.ExternalReferenceLink
                });

            if (result.Count() > 0)
                return Ok(result);
            else
                return this.NotFound();

        }
        [Route("api/Checklist/GetcreateformDocument/{IdJobChecklistItemDetail}")]

        public IHttpActionResult GetcreateformDocument(int IdJobChecklistItemDetail)
        {

            JobChecklistItemDetail jobChecklistItemDetails = db.JobChecklistItemDetails.Where(x => x.Id == IdJobChecklistItemDetail).FirstOrDefault();
            if (jobChecklistItemDetails == null)
            {
                return this.NotFound();
            }

            var result = db.ChecklistItems.Where(x => x.Id == jobChecklistItemDetails.IdChecklistItem).AsEnumerable().Select(checklistItem => new { IdCreateFormDocument = checklistItem.IdCreateFormDocument });
            if (result.Count() > 0)
                if (result.FirstOrDefault().IdCreateFormDocument != null && result.FirstOrDefault().IdCreateFormDocument != 0)
                    return Ok(result);
                else
                    return Ok();
            else
                return this.NotFound();

        }
        [Route("api/Checklist/UploadDocumentChecklistItem/{IdJobChecklistItemDetail}")]

        public IHttpActionResult GetUploadDocumentChecklistItem(int IdJobChecklistItemDetail)
        {

            JobChecklistItemDetail jobChecklistItemDetails = db.JobChecklistItemDetails.Where(x => x.Id == IdJobChecklistItemDetail).FirstOrDefault();
            if (jobChecklistItemDetails == null)
            {
                return this.NotFound();
            }

            var result = db.ChecklistItems.Where(x => x.Id == jobChecklistItemDetails.IdChecklistItem).AsEnumerable().Select(checklistItem => new { IdUploadFormDocument = checklistItem.IdUploadFormDocument });
            if (result.Count() > 0)
                if (result.FirstOrDefault().IdUploadFormDocument != null && result.FirstOrDefault().IdUploadFormDocument != 0)
                    return Ok(result);
                else
                    return Ok();
            else
                return this.NotFound();

        }
        [Route("api/Checklist/UploadDocumentPlumbingInspections/{IdJobPlumbingInspections}")]

        public IHttpActionResult GetUploadDocumentPlumbingInspections(int IdJobPlumbingInspections)
        {

            JobPlumbingInspection jobPlumbingInspections = db.JobPlumbingInspection.Where(x => x.IdJobPlumbingInspection == IdJobPlumbingInspections).FirstOrDefault();
            if (jobPlumbingInspections == null)
            {
                return this.NotFound();
            }

            var result = db.ChecklistItems.Where(x => x.Id == jobPlumbingInspections.IdChecklistItem).AsEnumerable().Select(checklistItem => new { IdUploadFormDocument = checklistItem.IdUploadFormDocument });
            if (result.Count() > 0)
                if (result.FirstOrDefault().IdUploadFormDocument != null && result.FirstOrDefault().IdUploadFormDocument != 0)
                    return Ok(result);
                else
                    return Ok();
            else
                return this.NotFound();

        }
        private JobChecklistCommentHistoryDTO Format(JobCheckListCommentHistory JobChecklistCommentHistory)
        {

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new JobChecklistCommentHistoryDTO
            {
                Id = JobChecklistCommentHistory.Id,
                IdJobChecklistItemDetail = JobChecklistCommentHistory.IdJobChecklistItemDetail,
                Description = JobChecklistCommentHistory.Description,
                CreatedBy = JobChecklistCommentHistory.CreatedBy,
                LastModifiedBy = JobChecklistCommentHistory.LastModifiedBy != null ? JobChecklistCommentHistory.LastModifiedBy : JobChecklistCommentHistory.CreatedBy,
                CreatedByEmployee = JobChecklistCommentHistory.CreatedByEmployee != null ? JobChecklistCommentHistory.CreatedByEmployee.FirstName + " " + JobChecklistCommentHistory.CreatedByEmployee.LastName : string.Empty,
                LastModified = JobChecklistCommentHistory.LastModifiedByEmployee != null ? JobChecklistCommentHistory.LastModifiedByEmployee.FirstName + " " + JobChecklistCommentHistory.LastModifiedByEmployee.LastName : (JobChecklistCommentHistory.CreatedByEmployee != null ? JobChecklistCommentHistory.CreatedByEmployee.FirstName + " " + JobChecklistCommentHistory.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = JobChecklistCommentHistory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobChecklistCommentHistory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : JobChecklistCommentHistory.CreatedDate,
                LastModifiedDate = JobChecklistCommentHistory.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobChecklistCommentHistory.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (JobChecklistCommentHistory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobChecklistCommentHistory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : JobChecklistCommentHistory.CreatedDate),
            };
        }

        [ResponseType(typeof(JobChecklistItemDetail))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/PutJobChecklistItemPartyResponsible/{id}")]
        public IHttpActionResult PutJobChecklistItemPartyResponsible(int id, [FromBody] JobChecklistItemDetailDTO jobChecklistItemDetailDTO)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != jobChecklistItemDetailDTO.Id)
                {
                    return BadRequest();
                }
                JobChecklistItemDetail jobChecklistItemDetail1 = db.JobChecklistItemDetails.Find(id);

                if (jobChecklistItemDetailDTO.PartyResponsible1 == 1 || jobChecklistItemDetailDTO.PartyResponsible1 == 2 || jobChecklistItemDetailDTO.PartyResponsible1 == 3)
                {
                    jobChecklistItemDetail1.PartyResponsible1 = jobChecklistItemDetailDTO.PartyResponsible1;
                    jobChecklistItemDetail1.PartyResponsible = jobChecklistItemDetailDTO.PartyResponsible;
                }
                if (jobChecklistItemDetailDTO.PartyResponsible1 == 2)
                {
                    jobChecklistItemDetail1.IdContact = jobChecklistItemDetailDTO.IdContact != null ? jobChecklistItemDetailDTO.IdContact : null;
                }
                if (jobChecklistItemDetailDTO.PartyResponsible1 == 3)
                {
                    jobChecklistItemDetail1.ManualPartyResponsible = jobChecklistItemDetailDTO.ManualPartyResponsible != null ? jobChecklistItemDetailDTO.ManualPartyResponsible : null; ;
                }
                if (employee != null)
                {
                    jobChecklistItemDetail1.LastModifiedBy = employee.Id;
                }
                jobChecklistItemDetail1.LastModifiedDate = DateTime.UtcNow;
                int JobChecklistGroupsID = db.JobChecklistItemDetails.Find(id).IdJobChecklistGroup;
                var jobchecklistheaderid = db.JobChecklistGroups.Where(x => x.Id == JobChecklistGroupsID).FirstOrDefault().IdJobCheckListHeader;

                JobChecklistHeader jobChecklistHeader = db.JobChecklistHeaders.Find(jobchecklistheaderid);
                jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;
                db.SaveChanges();
                return Ok("Party responsible type Updated Successfully");
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        [ResponseType(typeof(JobChecklistItemDetail))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/PutJobChecklistItemManualPartyResponsible/{id}")]
        public IHttpActionResult PutJobChecklistItemManualPartyResponsible(int id, [FromBody] JobChecklistItemDetailDTO jobChecklistItemDetailDTO)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != jobChecklistItemDetailDTO.Id)
                {
                    return BadRequest();
                }
                JobChecklistItemDetail jobChecklistItemDetail1 = db.JobChecklistItemDetails.Find(id);
                jobChecklistItemDetail1.ManualPartyResponsible = jobChecklistItemDetailDTO.ManualPartyResponsible != null ? jobChecklistItemDetailDTO.ManualPartyResponsible : null; ;

                if (employee != null)
                {
                    jobChecklistItemDetail1.LastModifiedBy = employee.Id;
                }
                jobChecklistItemDetail1.LastModifiedDate = DateTime.UtcNow;
                var jobchecklistheaderid = db.JobChecklistGroups.Where(x => x.Id == jobChecklistItemDetail1.IdJobChecklistGroup).FirstOrDefault().IdJobCheckListHeader;

                JobChecklistHeader jobChecklistHeader = db.JobChecklistHeaders.Find(jobchecklistheaderid);
                jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;
                db.SaveChanges();
                return Ok("ManualPartyResponsible Updated Successfully");
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        [ResponseType(typeof(JobChecklistItemDetail))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/PutJobChecklistItemManualIdContact/{id}")]
        public IHttpActionResult PutJobChecklistItemManualIdContact(int id, [FromBody] JobChecklistItemDetailDTO jobChecklistItemDetailDTO)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != jobChecklistItemDetailDTO.Id)
                {
                    return BadRequest();
                }
                JobChecklistItemDetail jobChecklistItemDetail1 = db.JobChecklistItemDetails.Find(id);
                jobChecklistItemDetail1.IdContact = jobChecklistItemDetailDTO.IdContact != null ? jobChecklistItemDetailDTO.IdContact : null;

                if (employee != null)
                {
                    jobChecklistItemDetail1.LastModifiedBy = employee.Id;
                }
                jobChecklistItemDetail1.LastModifiedDate = DateTime.UtcNow;
                var jobchecklistheaderid = db.JobChecklistGroups.Where(x => x.Id == jobChecklistItemDetail1.IdJobChecklistGroup).FirstOrDefault().IdJobCheckListHeader;

                JobChecklistHeader jobChecklistHeader = db.JobChecklistHeaders.Find(jobchecklistheaderid);
                jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;
                db.SaveChanges();
                return Ok("Contact for party responsible Updated Successfully");
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        [ResponseType(typeof(JobChecklistItemDetail))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/PutJobChecklistItemDetailsStage/{id}")]

        public IHttpActionResult PutJobChecklistItemDetailsStage(int id, [FromBody] JobChecklistItemDetailDTO jobChecklistItemDetailDTO)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != jobChecklistItemDetailDTO.Id)
                {
                    return BadRequest();
                }
                JobChecklistItemDetail jobChecklistItemDetail1 = db.JobChecklistItemDetails.Find(id);
                if (jobChecklistItemDetailDTO.Stage != null && jobChecklistItemDetailDTO.Stage != string.Empty)
                {
                    jobChecklistItemDetail1.Stage = jobChecklistItemDetailDTO.Stage;
                    jobChecklistItemDetail1.Status = 1;
                }
                if (employee != null)
                {
                    jobChecklistItemDetail1.LastModifiedBy = employee.Id;
                }
                jobChecklistItemDetail1.LastModifiedDate = DateTime.UtcNow;
                int JobChecklistGroupsID = db.JobChecklistItemDetails.Find(jobChecklistItemDetailDTO.Id).IdJobChecklistGroup;
                var jobchecklistheaderid = db.JobChecklistGroups.Where(x => x.Id == JobChecklistGroupsID).FirstOrDefault().IdJobCheckListHeader;
                JobChecklistHeader jobChecklistHeader = db.JobChecklistHeaders.Find(jobchecklistheaderid);
                jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;
                db.SaveChanges();
                return Ok("Stage Updated Successfully");
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }


        [ResponseType(typeof(JobChecklistItemDetail))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/PutJobChecklistItemDetailsDesignApplicant/{id}")]
        public IHttpActionResult PutJobChecklistItemDetailsDesignApplicant(int id, [FromBody] JobChecklistItemDetailDTO jobChecklistItemDetailDTO)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != jobChecklistItemDetailDTO.Id)
                {
                    return BadRequest();
                }
                JobChecklistItemDetail jobChecklistItemDetail1 = db.JobChecklistItemDetails.Find(id);

                if (jobChecklistItemDetailDTO.IdDesignApplicant != null && jobChecklistItemDetailDTO.IdDesignApplicant != 0)
                    jobChecklistItemDetail1.IdDesignApplicant = jobChecklistItemDetailDTO.IdDesignApplicant;
                if (employee != null)
                {
                    jobChecklistItemDetail1.LastModifiedBy = employee.Id;
                }
                jobChecklistItemDetail1.LastModifiedDate = DateTime.UtcNow;
                int JobChecklistGroupsID = db.JobChecklistItemDetails.Find(jobChecklistItemDetailDTO.Id).IdJobChecklistGroup;
                var jobchecklistheaderid = db.JobChecklistGroups.Where(x => x.Id == JobChecklistGroupsID).FirstOrDefault().IdJobCheckListHeader;
                JobChecklistHeader jobChecklistHeader = db.JobChecklistHeaders.Find(jobchecklistheaderid);
                jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;
                db.SaveChanges();
                return Ok("DesignApplicant Updated Successfully");
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        [ResponseType(typeof(JobChecklistItemDetail))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/PutJobCheckliststatus/{id}")]
        public IHttpActionResult PutJobCheckliststatus(int id, [FromBody] JobChecklistItemDetailDTO jobChecklistItemDetailDTO)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != jobChecklistItemDetailDTO.Id)
                {
                    return BadRequest();
                }
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                JobChecklistItemDetail jobChecklistItemDetail1 = db.JobChecklistItemDetails.Find(id);
                if (jobChecklistItemDetailDTO.Status != 0)
                    jobChecklistItemDetail1.Status = jobChecklistItemDetailDTO.Status;
                jobChecklistItemDetail1.LastModifiedDate = DateTime.UtcNow;
                int JobChecklistGroupsID = db.JobChecklistItemDetails.Find(jobChecklistItemDetailDTO.Id).IdJobChecklistGroup;
                var jobchecklistheaderid = db.JobChecklistGroups.Where(x => x.Id == JobChecklistGroupsID).FirstOrDefault().IdJobCheckListHeader;

                JobChecklistHeader jobChecklistHeader = db.JobChecklistHeaders.Find(jobchecklistheaderid);
                jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;
                db.SaveChanges();
                return Ok("Status Updated Successfully");
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        [ResponseType(typeof(JobChecklistItemDetail))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/PutJobChecklistItemInspector/{id}")]
        public IHttpActionResult PutJobChecklistItemInspector(int id, [FromBody] JobChecklistItemDetailDTO jobChecklistItemDetailDTO)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != jobChecklistItemDetailDTO.Id)
                {
                    return BadRequest();
                }
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                JobChecklistItemDetail jobChecklistItemDetail1 = db.JobChecklistItemDetails.Find(id);

                if (jobChecklistItemDetailDTO.IdInspector != null && jobChecklistItemDetailDTO.IdInspector != 0)
                    jobChecklistItemDetail1.IdInspector = jobChecklistItemDetailDTO.IdInspector;
                if (employee != null)
                {
                    jobChecklistItemDetail1.LastModifiedBy = employee.Id;
                }
                jobChecklistItemDetail1.LastModifiedDate = DateTime.UtcNow;
                int JobChecklistGroupsID = db.JobChecklistItemDetails.Find(jobChecklistItemDetailDTO.Id).IdJobChecklistGroup;
                var jobchecklistheaderid = db.JobChecklistGroups.Where(x => x.Id == JobChecklistGroupsID).FirstOrDefault().IdJobCheckListHeader;

                JobChecklistHeader jobChecklistHeader = db.JobChecklistHeaders.Find(jobchecklistheaderid);
                jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;
                db.SaveChanges();
                return Ok("Inspector Updated Successfully");
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }


        #region Edit checklist New
        [ResponseType(typeof(JobChecklistHeaderDTO))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/PostJobChecklistgroups")]
        public IHttpActionResult PostJobChecklistgroups(JobChecklistHeaderDTO jobChecklistHeaderDTO)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                List<int?> WorkTypeIds = new List<int?>();
                JobChecklistHeader jobChecklistHeader = db.JobChecklistHeaders.Find(jobChecklistHeaderDTO.IdJobCheckListHeader);
                var JobchecklistGroupinDB = db.JobChecklistGroups.Where(x => x.IdJobCheckListHeader == jobChecklistHeaderDTO.IdJobCheckListHeader).Select(y => y.IdCheckListGroup).ToList();
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                List<JobChecklistGroup> lstJobChecklistGroupDetail = new List<JobChecklistGroup>();
                List<JobChecklistGroup> lstNewchecklistGroup = new List<JobChecklistGroup>();
                var groups = jobChecklistHeaderDTO.CheckListGroups.Select(x => x.Id).ToList();
                if (jobChecklistHeaderDTO.CheckListGroups != null)
                {
                    List<JobChecklistGroup> lstjobChecklistGroupDetail = new List<JobChecklistGroup>();
                    JobChecklistGroup jobChecklistGroupDetail;

                    //add new group
                    foreach (var tmpCheckListGroup in jobChecklistHeaderDTO.CheckListGroups)
                    {
                        if (!JobchecklistGroupinDB.Contains(tmpCheckListGroup.Id))
                        {
                            jobChecklistGroupDetail = new JobChecklistGroup();
                            jobChecklistGroupDetail.IdJobCheckListHeader = jobChecklistHeaderDTO.IdJobCheckListHeader;
                            jobChecklistGroupDetail.IdCheckListGroup = tmpCheckListGroup.Id;
                            jobChecklistGroupDetail.Displayorder1 = tmpCheckListGroup.Displayorder;
                            db.JobChecklistGroups.Add(jobChecklistGroupDetail);
                            lstNewchecklistGroup.Add(jobChecklistGroupDetail);
                            try
                            {
                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                throw new RpoBusinessException("Error in adding New Checklist Group");
                            }

                        }
                    }

                    //Remove group and it's items and details
                    foreach (var tmpCheckListGroupOfDB in JobchecklistGroupinDB)
                    {
                        if (!groups.Contains(tmpCheckListGroupOfDB))
                        {
                            JobChecklistGroup jobChecklistGroups = db.JobChecklistGroups.Where(x => x.IdCheckListGroup == tmpCheckListGroupOfDB && x.IdJobCheckListHeader == jobChecklistHeaderDTO.IdJobCheckListHeader).FirstOrDefault();
                            CheckListGroup checklistgroup = db.CheckListGroups.Find(jobChecklistGroups.IdCheckListGroup);
                            if (checklistgroup.Type == "PL")//Replace this plumbing group
                            {//remove all plumbing related details

                                var lstplumbinginspections = db.JobPlumbingInspection.Where(x => x.IdJobChecklistGroup == jobChecklistGroups.Id).Select(y => y.IdJobPlumbingInspection).ToList();
                                var Nulllstplumbinginspections = lstplumbinginspections.Cast<int?>().ToList();
                                db.JobPlumbingInspectionComments.RemoveRange(db.JobPlumbingInspectionComments.Where(x => lstplumbinginspections.Contains(x.IdJobPlumbingInspection)).ToList());
                                db.JobPlumbingInspectionDueDate.RemoveRange(db.JobPlumbingInspectionDueDate.Where(x => lstplumbinginspections.Contains(x.IdJobPlumbingInspection)).ToList());
                                db.JobPlumbingInspectionProgressNoteHistory.RemoveRange(db.JobPlumbingInspectionProgressNoteHistory.Where(x => lstplumbinginspections.Contains(x.IdJobPlumbingInspection)).ToList());
                                (from p in db.JobDocuments
                                 where Nulllstplumbinginspections.Contains((int?)p.IdJobPlumbingInspections)
                                 select p).ToList()
                                          .ForEach(x => x.IdJobPlumbingInspections = null);
                                db.JobPlumbingInspection.RemoveRange(db.JobPlumbingInspection.Where(x => x.IdJobChecklistGroup == jobChecklistGroups.Id).ToList());
                                db.JobPlumbingChecklistFloors.RemoveRange(db.JobPlumbingChecklistFloors.Where(x => x.IdJobCheckListHeader == jobChecklistHeaderDTO.IdJobCheckListHeader).ToList());
                                var jobcjhecklistgroup = db.JobChecklistGroups.Where(x => x.IdCheckListGroup == tmpCheckListGroupOfDB && x.IdJobCheckListHeader == jobChecklistHeaderDTO.IdJobCheckListHeader).FirstOrDefault();
                                var composite = db.CompositeChecklistDetails.Where(x => x.IdJobChecklistGroup == jobcjhecklistgroup.Id && x.IdJobChecklistHeader == jobChecklistHeaderDTO.IdJobCheckListHeader).Select(y => y.IdCompositeChecklist).ToList().Distinct();
                                foreach (var c in composite)
                                {
                                    db.CompositeChecklistDetails.RemoveRange(
                                     db.CompositeChecklistDetails.Where(x => x.IdJobChecklistGroup == jobcjhecklistgroup.Id && x.IdCompositeChecklist == c).ToList());
                                }
                                db.JobChecklistGroups.Remove(db.JobChecklistGroups.Where(x => x.IdCheckListGroup == tmpCheckListGroupOfDB && x.IdJobCheckListHeader == jobChecklistHeaderDTO.IdJobCheckListHeader).FirstOrDefault());
                                try
                                {
                                    db.SaveChanges();
                                }
                                catch (Exception ex)
                                { throw ex.InnerException; }

                            }
                            else
                            {
                                var lstItemdetail = db.JobChecklistItemDetails.Where(x => x.IdJobChecklistGroup == jobChecklistGroups.Id).Select(y => y.Id).ToList();
                                var NullLstItemdetail = lstItemdetail.Cast<int?>().ToList();
                                db.JobCheckListCommentHistories.RemoveRange(db.JobCheckListCommentHistories.Where(x => lstItemdetail.Contains(x.IdJobChecklistItemDetail)).ToList());
                                db.JobChecklistItemDueDate.RemoveRange(db.JobChecklistItemDueDate.Where(x => lstItemdetail.Contains(x.IdJobChecklistItemDetail)).ToList());
                                db.JobCheckListProgressNoteHistories.RemoveRange(db.JobCheckListProgressNoteHistories.Where(x => lstItemdetail.Contains(x.IdJobChecklistItemDetail)).ToList());
                                (from p in db.JobDocuments
                                 where NullLstItemdetail.Contains(p.IdJobchecklistItemDetails)
                                 select p).ToList()
                                           .ForEach(x => x.IdJobchecklistItemDetails = null);
                                //May be need to remove this save changes
                                db.SaveChanges();
                                db.JobChecklistItemDetails.RemoveRange(db.JobChecklistItemDetails.Where(x => x.IdJobChecklistGroup == jobChecklistGroups.Id).ToList());
                                var group = db.JobChecklistGroups.Where(x => x.IdCheckListGroup == tmpCheckListGroupOfDB && x.IdJobCheckListHeader == jobChecklistHeaderDTO.IdJobCheckListHeader).FirstOrDefault();
                                int groupid = Convert.ToInt32(group.Id);
                                db.CompositeChecklistDetails.RemoveRange(db.CompositeChecklistDetails.Where(x => x.IdJobChecklistGroup == groupid).ToList());
                                db.JobChecklistGroups.Remove(db.JobChecklistGroups.Where(x => x.IdCheckListGroup == tmpCheckListGroupOfDB && x.IdJobCheckListHeader == jobChecklistHeaderDTO.IdJobCheckListHeader).FirstOrDefault());
                                db.SaveChanges();
                            }
                        }
                    }
                    db.SaveChanges(); // Temporary commented
                    //If in new group list plumbing is selected then add floors and items
                    foreach (var tmpCheckListGroup in jobChecklistHeaderDTO.CheckListGroups)
                    {//If plumbing floor was not there previously
                        if (!JobchecklistGroupinDB.Contains(tmpCheckListGroup.Id))
                        {
                            CheckListGroup checklistgroup = db.CheckListGroups.Find(tmpCheckListGroup.Id);
                            if (checklistgroup.Type == "PL")
                            {
                                if (jobChecklistHeaderDTO.JobPlumbingChecklistFloors.Count() > 0)
                                {


                                    foreach (var a in jobChecklistHeaderDTO.JobPlumbingChecklistFloors)
                                    {
                                        JobPlumbingChecklistFloors jobPlumbingCheckListFloor = new JobPlumbingChecklistFloors();
                                        jobPlumbingCheckListFloor.FloonNumber = a.FloonNumber;
                                        jobPlumbingCheckListFloor.FloorDisplayOrder = a.FloorDisplayOrder;
                                        jobPlumbingCheckListFloor.IdJobCheckListHeader = jobChecklistHeaderDTO.IdJobCheckListHeader;
                                        db.JobPlumbingChecklistFloors.Add(jobPlumbingCheckListFloor);
                                        db.SaveChanges();

                                        if (a.InspectionType != null)
                                        {
                                            List<JobPlumbingInspection> lstJobPlumbingInspection = new List<JobPlumbingInspection>();
                                            foreach (var i in a.InspectionType)
                                            {
                                                JobPlumbingInspection jobPlumbingInspection = new JobPlumbingInspection();
                                                jobPlumbingInspection.IdChecklistItem = i.Id;
                                                jobPlumbingInspection.IsActive = true;
                                                jobPlumbingInspection.IdJobPlumbingCheckListFloors = jobPlumbingCheckListFloor.Id;
                                                jobPlumbingInspection.IdJobChecklistGroup = lstNewchecklistGroup.Where(x => x.IdCheckListGroup == tmpCheckListGroup.Id).Select(y => y.Id).FirstOrDefault();
                                                jobPlumbingInspection.Result = "3";
                                                if (employee != null)
                                                {
                                                    jobPlumbingInspection.CreatedBy = employee.Id;
                                                }
                                                jobPlumbingInspection.CreatedDate = DateTime.UtcNow;
                                                lstJobPlumbingInspection.Add(jobPlumbingInspection);
                                            }
                                            db.JobPlumbingInspection.AddRange(lstJobPlumbingInspection);
                                            try
                                            {
                                                db.SaveChanges();
                                            }
                                            catch (Exception ex)
                                            {
                                                throw new RpoBusinessException("Error in Saving Plumbing Inspections");
                                            }
                                        }

                                    }

                                }
                            }
                        }
                        //else if plumbing group was already there previously now we are editing some floors or inspection
                        else if (JobchecklistGroupinDB.Contains(tmpCheckListGroup.Id))
                        {
                            CheckListGroup checklistgroup = db.CheckListGroups.Find(tmpCheckListGroup.Id);
                            List<JobPlumbingChecklistFloors> lstJobPlumbingCheckListFloor = new List<JobPlumbingChecklistFloors>();
                            if (checklistgroup.Type == "PL")
                            {
                                if (jobChecklistHeaderDTO.JobPlumbingChecklistFloors.Count() > 0)
                                {
                                    JobPlumbingChecklistFloors jobPlumbingCheckListFloor = new JobPlumbingChecklistFloors();
                                    List<int> ids = jobChecklistHeaderDTO.JobPlumbingChecklistFloors.Select(x => x.Id).ToList();
                                    //remove floor which is not there in new payload
                                    foreach (var a in jobChecklistHeaderDTO.JobPlumbingChecklistFloors)
                                    {

                                        var floorsInDb = db.JobPlumbingChecklistFloors.Where(x => x.IdJobCheckListHeader == jobChecklistHeaderDTO.IdJobCheckListHeader).Select(x => x.Id).ToList();
                                        if (floorsInDb.Count() > 0)
                                        {
                                            foreach (var f in floorsInDb)
                                            {
                                                if (!ids.Contains(f))
                                                {
                                                    var removeinspection = db.JobPlumbingInspection.Where(x => x.IdJobPlumbingCheckListFloors == f).ToList();
                                                    foreach (var r in removeinspection)
                                                    {
                                                        if (removeinspection != null)
                                                        {
                                                            db.JobPlumbingInspectionComments.RemoveRange(db.JobPlumbingInspectionComments.Where(x => x.IdJobPlumbingInspection == r.IdJobPlumbingInspection).ToList());
                                                            db.JobPlumbingInspectionDueDate.RemoveRange(db.JobPlumbingInspectionDueDate.Where(x => x.IdJobPlumbingInspection == r.IdJobPlumbingInspection).ToList());
                                                            db.JobPlumbingInspectionProgressNoteHistory.RemoveRange(db.JobPlumbingInspectionProgressNoteHistory.Where(x => x.IdJobPlumbingInspection == r.IdJobPlumbingInspection).ToList());
                                                            db.JobPlumbingInspection.Remove(r);
                                                        }
                                                    }
                                                    db.JobPlumbingChecklistFloors.Remove(db.JobPlumbingChecklistFloors.Where(x => x.Id == f).FirstOrDefault());
                                                    try
                                                    {
                                                        db.SaveChanges();
                                                    }
                                                    catch (Exception Ex)
                                                    {
                                                        throw new RpoBusinessException("Error in Removing Plumbing Inspections and floor releted data");
                                                    }
                                                }
                                            }
                                        }

                                        //foreach (var a in jobChecklistHeaderDTO.JobPlumbingChecklistFloors)
                                        //{
                                        //if any new floor is added add it into db
                                        if (a.Id == 0)
                                        {
                                            JobPlumbingChecklistFloors j = new JobPlumbingChecklistFloors();
                                            j.FloonNumber = a.FloonNumber;

                                            j.IdJobCheckListHeader = jobChecklistHeaderDTO.IdJobCheckListHeader;
                                            j.FloorDisplayOrder = a.FloorDisplayOrder;

                                            db.JobPlumbingChecklistFloors.Add(j);
                                            db.SaveChanges();
                                            ids.Remove(0);
                                            ids.Add(j.Id);


                                            if (a.InspectionType != null)
                                            {
                                                foreach (var i in a.InspectionType)
                                                {
                                                    JobPlumbingInspection jobPlumbingInspection = new JobPlumbingInspection();
                                                    jobPlumbingInspection.IdChecklistItem = i.Id;
                                                    jobPlumbingInspection.IsActive = true;
                                                    jobPlumbingInspection.IdJobPlumbingCheckListFloors = j.Id;
                                                    jobPlumbingInspection.Result = "3";

                                                    jobPlumbingInspection.IdJobChecklistGroup = db.JobChecklistGroups.Where(y => y.IdCheckListGroup == tmpCheckListGroup.Id && y.IdJobCheckListHeader == jobChecklistHeaderDTO.IdJobCheckListHeader).Select(y => y.Id).FirstOrDefault();

                                                    if (employee != null)
                                                    {
                                                        jobPlumbingInspection.CreatedBy = employee.Id;
                                                    }
                                                    jobPlumbingInspection.CreatedDate = DateTime.UtcNow;
                                                    db.JobPlumbingInspection.Add(jobPlumbingInspection);
                                                }
                                            }
                                            try
                                            {
                                                db.SaveChanges();
                                            }
                                            catch (Exception)
                                            {
                                                throw new RpoBusinessException("Error in Saving Plumbing Inspections");
                                            }
                                        }
                                        if (a.Id != 0)
                                        {
                                            string floornumber = db.JobPlumbingChecklistFloors.Find(a.Id).FloonNumber;
                                            int floorDisplayOrder = db.JobPlumbingChecklistFloors.Find(a.Id).FloorDisplayOrder;
                                            if (a.FloonNumber != floornumber)
                                            {
                                                JobPlumbingChecklistFloors jobPlumbingChecklistFloors = db.JobPlumbingChecklistFloors.Find(a.Id);
                                                jobPlumbingChecklistFloors.FloonNumber = a.FloonNumber;
                                                jobPlumbingChecklistFloors.FloorDisplayOrder = a.FloorDisplayOrder;
                                                db.SaveChanges();
                                            }
                                            if (a.FloorDisplayOrder != floorDisplayOrder)
                                            {
                                                JobPlumbingChecklistFloors jobPlumbingChecklistFloors = db.JobPlumbingChecklistFloors.Find(a.Id);
                                                jobPlumbingChecklistFloors.FloonNumber = a.FloonNumber;
                                                jobPlumbingChecklistFloors.FloorDisplayOrder = a.FloorDisplayOrder;
                                                db.SaveChanges();
                                            }
                                        }
                                        // }

                                        // }


                                        var FloorinDB = db.JobPlumbingChecklistFloors.Where(x => x.FloonNumber == a.FloonNumber && x.IdJobCheckListHeader == jobChecklistHeaderDTO.IdJobCheckListHeader).FirstOrDefault();
                                        List<int> InspectionsinDB = db.JobPlumbingInspection.Where(x => x.IdJobPlumbingCheckListFloors == FloorinDB.Id).Select(x => x.IdChecklistItem).ToList();
                                        //ad new inspection if it was not there previously                                     
                                        if (a.InspectionType != null)
                                        {
                                            List<int> itemsinpayload = a.InspectionType.Select(x => x.Id).ToList();

                                            foreach (var i in itemsinpayload)
                                            {
                                                if (!InspectionsinDB.Contains(i))
                                                {
                                                    JobPlumbingInspection jobPlumbingInspection = new JobPlumbingInspection();
                                                    jobPlumbingInspection.IdChecklistItem = a.InspectionType.Where(y => y.Id == i).Select(x => x.Id).FirstOrDefault();
                                                    jobPlumbingInspection.Result = "3";
                                                    jobPlumbingInspection.IsActive = true;
                                                    jobPlumbingInspection.IdJobPlumbingCheckListFloors = a.Id;
                                                    jobPlumbingInspection.IdJobChecklistGroup = db.JobChecklistGroups.Where(x => x.IdCheckListGroup == tmpCheckListGroup.Id && x.IdJobCheckListHeader == jobChecklistHeaderDTO.IdJobCheckListHeader).Select(y => y.Id).FirstOrDefault(); ;

                                                    if (employee != null)
                                                    {
                                                        jobPlumbingInspection.CreatedBy = employee.Id;
                                                    }
                                                    jobPlumbingInspection.CreatedDate = DateTime.UtcNow;
                                                    db.JobPlumbingInspection.Add(jobPlumbingInspection);
                                                    try
                                                    {
                                                        db.SaveChanges();
                                                    }
                                                    catch (Exception)
                                                    {
                                                        throw new RpoBusinessException("Error in Saving Plumbing Inspections");
                                                    }
                                                }
                                            }
                                            //remove inspection if in editing user deselect it                                               
                                            foreach (var i in InspectionsinDB)
                                            {
                                                if (!itemsinpayload.Contains(i))
                                                {
                                                    var removeinspection = db.JobPlumbingInspection.Where(x => x.IdChecklistItem == i && x.IdJobPlumbingCheckListFloors == a.Id).FirstOrDefault();
                                                    db.JobPlumbingInspectionComments.RemoveRange(db.JobPlumbingInspectionComments.Where(x => x.IdJobPlumbingInspection == removeinspection.IdJobPlumbingInspection).ToList());
                                                    db.JobPlumbingInspectionDueDate.RemoveRange(db.JobPlumbingInspectionDueDate.Where(x => x.IdJobPlumbingInspection == removeinspection.IdJobPlumbingInspection).ToList());
                                                    db.JobPlumbingInspectionProgressNoteHistory.RemoveRange(db.JobPlumbingInspectionProgressNoteHistory.Where(x => x.IdJobPlumbingInspection == removeinspection.IdJobPlumbingInspection).ToList());
                                                    db.JobPlumbingInspection.Remove(removeinspection);
                                                }
                                            }
                                        }

                                    }
                                    db.JobPlumbingChecklistFloors.AddRange(lstJobPlumbingCheckListFloor);
                                }
                            }
                            try
                            {
                                db.SaveChanges();
                            }
                            catch (Exception)
                            {
                                throw new RpoBusinessException("Error in Removing Plumbing Inspections related data");
                            }
                        }
                    }
                }


                #region New Business Logic for new group add items

                RfpAddress rfpAddress = db.Jobs.Include("RfpAddress").FirstOrDefault(j => j.Id == jobChecklistHeaderDTO.IdJob).RfpAddress;
                JobApplication jobapplication = db.JobApplications.FirstOrDefault(j => j.Id == jobChecklistHeaderDTO.IdJobApplication);


                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (employee != null)
                {
                    jobChecklistHeader.LastModifiedBy = employee.Id;
                }

                var application = db.JobApplications.Where(x => x.Id == jobChecklistHeaderDTO.IdJobApplication).FirstOrDefault();
                var applicationtype = db.JobApplicationTypes.Where(x => x.Id == application.IdJobApplicationType).Select(x => x.Description).FirstOrDefault();
                var applicationnumber = application.ApplicationNumber;
                var worktype = jobChecklistHeaderDTO.JobApplicationWorkPermitTypes.Select(x => x.Code);
                string applicationstatus = application.JobApplicationStatus;
                StringBuilder strworktype = new StringBuilder();
                foreach (var d in worktype)
                {
                    strworktype.Append(d + ", ");
                }
                //  string title = applicationtype + (applicationnumber != null ? " - " + applicationnumber : string.Empty) + " - " + strworktype.Remove(strworktype.Length - 2, 2) + (applicationstatus != null ? " - " + applicationstatus : string.Empty)+ (application.FloorWorking != null ? " - " + application.FloorWorking : string.Empty); 
                string title = applicationtype + (applicationnumber != null ? " - " + applicationnumber : string.Empty) + (strworktype != null ? " - " + strworktype.Remove(strworktype.Length - 2, 2) : string.Empty) + (applicationstatus != null ? " - " + applicationstatus : string.Empty) + (application.FloorWorking != null ? " - " + application.FloorWorking : string.Empty);

                jobChecklistHeader.ChecklistName = title;
                var compositeParents = db.CompositeChecklistDetails.Where(x => x.IsParentCheckList == true && x.IdJobChecklistHeader == jobChecklistHeader.IdJobCheckListHeader).Select(y => y.IdCompositeChecklist).ToList();
                foreach (var c in compositeParents)
                {
                    var composite = db.CompositeChecklists.Where(x => x.Id == c).FirstOrDefault();
                    composite.Name = title;
                }

                try
                {
                    db.SaveChanges();// save in headertable New name of checklist with new permit or removed permit
                }
                catch (Exception)
                {
                    throw new RpoBusinessException("Error in Saving New Name of checklist");
                }

                #endregion
                if (jobChecklistHeaderDTO.JobApplicationWorkPermitTypes != null)
                {
                    if (jobChecklistHeader.JobApplicationWorkPermitTypes == null)
                    {
                        jobChecklistHeader.JobApplicationWorkPermitTypes = new HashSet<JobApplicationWorkPermitType>();
                    }
                    foreach (var jobApplicationWorkPermitTypes in jobChecklistHeaderDTO.JobApplicationWorkPermitTypes)
                    {
                        if (!jobChecklistHeader.JobApplicationWorkPermitTypes.Select(x => x.Id).Contains(jobApplicationWorkPermitTypes.Id))
                        {
                            jobChecklistHeader.JobApplicationWorkPermitTypes.Add(db.JobApplicationWorkPermitTypes.Find(jobApplicationWorkPermitTypes.Id));
                        }

                        WorkTypeIds.Add(db.JobApplicationWorkPermitTypes.Find(jobApplicationWorkPermitTypes.Id).IdJobWorkType);
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        throw new RpoBusinessException("Error in Saving Permits of checklist");
                    }
                    List<JobApplicationWorkPermitType> varJobApplicationWorkPermitTypes = new List<JobApplicationWorkPermitType>();
                    varJobApplicationWorkPermitTypes.AddRange(jobChecklistHeader.JobApplicationWorkPermitTypes);
                    foreach (var jobApplicationWorkPermitTypes in varJobApplicationWorkPermitTypes)
                    {
                        if (!jobChecklistHeaderDTO.JobApplicationWorkPermitTypes.Select(x => x.Id).Contains(jobApplicationWorkPermitTypes.Id))

                        {
                            jobChecklistHeader.JobApplicationWorkPermitTypes.Remove(db.JobApplicationWorkPermitTypes.Find(jobApplicationWorkPermitTypes.Id));
                            WorkTypeIds.Remove(db.JobApplicationWorkPermitTypes.Find(jobApplicationWorkPermitTypes.Id).IdJobWorkType);
                        }
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        throw new RpoBusinessException("Error in Removing Permits of checklist");
                    }
                }

                #region NEW Bussiness logic for getting items based on application and permit and group and properties
                if (lstNewchecklistGroup.Count > 0)
                {
                    int? Id_applicationtype = db.JobApplications.FirstOrDefault(x => x.Id == jobChecklistHeaderDTO.IdJobApplication).IdJobApplicationType;
                    var lstJobApplicationWorkPermitTypes = db.JobChecklistHeaders.Include("JobApplicationWorkPermitTypes").Where(x => x.IdJobCheckListHeader == jobChecklistHeaderDTO.IdJobCheckListHeader).ToList();
                    var workpermittypes = lstJobApplicationWorkPermitTypes.Select(x => x.JobApplicationWorkPermitTypes);
                    List<ChecklistItem> groupwise_items = new List<ChecklistItem>();
                    List<ChecklistItem> groupwise_final_items = new List<ChecklistItem>();
                    List<ChecklistItem> final_items = new List<ChecklistItem>();

                    foreach (var tmpCheckListGroup in lstNewchecklistGroup)
                    {
                        var checklistgroup = db.CheckListGroups.Find(tmpCheckListGroup.IdCheckListGroup);
                        if (checklistgroup.Type.Trim().ToLower() != "pl")
                        {
                            groupwise_items = db.ChecklistItems.Where(x => x.IdCheckListGroup == tmpCheckListGroup.IdCheckListGroup).ToList(); //after filteration of items based on application and permit from that filter items which matches checklistGroup
                            foreach (var b in groupwise_items)
                            {
                                groupwise_final_items.Add(b);
                            }
                        }
                    }

                    // int? Id_applicationtype = db.JobApplications.FirstOrDefault(x => x.Id == jobChecklistHeaderDTO.IdJobApplication).IdJobApplicationType;
                    List<ChecklistItem> permitwisewise_items = new List<ChecklistItem>();
                    List<ChecklistItem> applicationwise_items = new List<ChecklistItem>();
                    if (groupwise_final_items.Count > 0)
                    {
                        var Item_jobapplication_wise = groupwise_final_items.Where(x => x.JobApplicationTypes.Any(ctdb => ctdb.Id == Id_applicationtype)).ToList();
                        //get items matches with application type        

                        foreach (var c in Item_jobapplication_wise)
                        {
                            applicationwise_items.Add(c);
                        }

                        //remove items which is not matched with given permit ex : MH
                        List<ChecklistItem> removeitemswithoutsamepermit = new List<ChecklistItem>();

                        Expression<Func<ChecklistItem, bool>> worktypeExpression = null;

                        foreach (var WorkTypeId in WorkTypeIds)
                        {
                            if (WorkTypeIds.IndexOf(WorkTypeId) == 0)
                            {
                                //worktypeExpression = e => !string.IsNullOrWhiteSpace(e.IdJobWorkTypes) && !e.IdJobWorkTypes.Contains(Convert.ToString(WorkTypeId));
                                worktypeExpression = e => !string.IsNullOrWhiteSpace(e.IdJobWorkTypes) && !((e.IdJobWorkTypes.Split(',')).ToArray().Contains(Convert.ToString(WorkTypeId)));
                            }
                            else
                            {
                                if (worktypeExpression != null)
                                {
                                    var compile = worktypeExpression.Compile();
                                    //worktypeExpression = e => compile(e) && !e.IdJobWorkTypes.Contains(Convert.ToString(WorkTypeId));
                                    worktypeExpression = e => compile(e) && !((e.IdJobWorkTypes.Split(',')).ToArray().Contains(Convert.ToString(WorkTypeId)));
                                }
                            }
                        }
                        if (worktypeExpression != null)
                            removeitemswithoutsamepermit = applicationwise_items.Where(worktypeExpression.Compile()).ToList();

                        //    //from above data get only that items which matches with workpermit
                        foreach (var c in removeitemswithoutsamepermit)
                        {
                            applicationwise_items.Remove(c);
                        }
                        // }
                        // get only that items which matches with workpermit
                        foreach (var WorkTypeId in WorkTypeIds)
                        {
                            var Item_permit_wise = groupwise_final_items.Where(x => x.JobWorkTypes.Any(ctdb => ctdb.Id == WorkTypeId.Value)).ToList();
                            //   
                            foreach (var c in Item_permit_wise)
                            {
                                permitwisewise_items.Add(c);
                            }
                        }
                        List<ChecklistItem> removeitemswithoutsameapplication = new List<ChecklistItem>();

                        //Problem of multiple ids
                        removeitemswithoutsameapplication = permitwisewise_items.Where(x => !string.IsNullOrWhiteSpace(x.IdJobApplicationTypes) && !x.IdJobApplicationTypes.Contains(Convert.ToString(Id_applicationtype))).ToList();

                        //remove items which doesn't match with application and not null
                        foreach (var c in removeitemswithoutsameapplication)
                        {
                            permitwisewise_items.Remove(c);
                        }
                        //concat this later
                        final_items = applicationwise_items;
                        final_items.AddRange(permitwisewise_items);
                        var Distinctfinalitem = final_items.Distinct().ToList();

                        //Propertywise item start logic
                        List<ChecklistAddressPropertyMaping> ChecklistAddressPropertyMaping1 = new List<ChecklistAddressPropertyMaping>();
                        foreach (var item in groupwise_final_items)
                        {
                            var iteminpropertmapping = db.ChecklistAddressPropertyMaping.Where(x => x.IdChecklistItem == item.Id).ToList();//fetch  final items' property from addrespropertmapping table which comes under slected application,permit and group
                            foreach (var a in iteminpropertmapping)
                            {
                                ChecklistAddressPropertyMaping1.Add(a);
                            }
                        }
                        List<int> ListOfProperties = new List<int>();

                        if (rfpAddress.CoastalErosionHazardAreaMapCheck == true)
                        {
                            ListOfProperties.Add(Enums.AddressProperties.CoastalErosionHazardAreaMapCheck.GetHashCode());
                        }
                        if (rfpAddress.FreshwaterWetlandsMapCheck == true)
                        {
                            ListOfProperties.Add(Enums.AddressProperties.FreshwaterWetlandsMapCheck.GetHashCode());
                        }
                        if (rfpAddress.IdOwnerType != null)
                        {
                            ListOfProperties.Add(Enums.AddressProperties.OwnerType.GetHashCode());
                        }
                        if (rfpAddress.SpecialDistrict != null && rfpAddress.SpecialDistrict != string.Empty)
                        {
                            ListOfProperties.Add(Enums.AddressProperties.SpecialDistrict.GetHashCode());
                        }
                        if (rfpAddress.SpecialFloodHazardAreaCheck == true)
                        {
                            ListOfProperties.Add(Enums.AddressProperties.SpecialFloodHazardAreaCheck.GetHashCode());
                        }
                        if (rfpAddress.TidalWetlandsMapCheck == true)
                        {
                            ListOfProperties.Add(Enums.AddressProperties.TidalWetlandsMapCheck.GetHashCode());
                        }
                        if (rfpAddress.IsLandmark == true)
                        {
                            ListOfProperties.Add(Enums.AddressProperties.Landmark.GetHashCode());
                        }
                        if (rfpAddress.SRORestrictedCheck == true)
                        {
                            ListOfProperties.Add(Enums.AddressProperties.SROrestricted.GetHashCode());
                        }
                        if (rfpAddress.IsLittleE == true)
                        {
                            ListOfProperties.Add(Enums.AddressProperties.Environmentalrestrictions.GetHashCode());
                        }
                        if (rfpAddress.LoftLawCheck == true)
                        {
                            ListOfProperties.Add(Enums.AddressProperties.LoftLaw.GetHashCode());
                        }
                        if (jobapplication.IsHighRise == true)
                        {
                            ListOfProperties.Add(Enums.AddressProperties.Highrise.GetHashCode());
                        }
                        //if (rfpAddress.IsLittleE == true)
                        //{
                        //    ListOfProperties.Add(Enums.AddressProperties.li.GetHashCode());
                        //}
                        if (rfpAddress.CityOwnedCheck == true)
                        {
                            ListOfProperties.Add(Enums.AddressProperties.CityOwned.GetHashCode());
                        }

                        #region get properties based on workpermit like SiteSafetyCoordinator,Superintendentofconstruction,SiteSafetyManager
                        //  getting address property based on workpermit which are in work permit popup like SiteSafetyCoordinator,Superintendentofconstruction,SiteSafetyManager
                        var jobChecklistHeaderWorkpermits = db.JobChecklistHeaders.Include("JobApplicationWorkPermitTypes").
                                                            Where(x => x.IdJobCheckListHeader == jobChecklistHeader.IdJobCheckListHeader)
                                                            .Select(y => y.JobApplicationWorkPermitTypes);

                        foreach (var a in jobChecklistHeaderWorkpermits)
                        {
                            if (a.Select(x => x.HasSiteSafetyCoordinator).FirstOrDefault() == true)
                            {
                                if (!ListOfProperties.Contains(Enums.AddressProperties.SiteSafetyCoordinator.GetHashCode()))
                                    ListOfProperties.Add(Enums.AddressProperties.SiteSafetyCoordinator.GetHashCode());
                            }
                            if (a.Select(x => x.HasSiteSafetyManager).FirstOrDefault() == true)
                            {
                                if (!ListOfProperties.Contains(Enums.AddressProperties.SiteSafetyManager.GetHashCode()))
                                    ListOfProperties.Add(Enums.AddressProperties.SiteSafetyManager.GetHashCode());
                            }
                            if (a.Select(x => x.HasSuperintendentofconstruction).FirstOrDefault() == true)
                            {
                                if (!ListOfProperties.Contains(Enums.AddressProperties.SuperintendentOfConstruction.GetHashCode()))
                                    ListOfProperties.Add(Enums.AddressProperties.SuperintendentOfConstruction.GetHashCode());
                            }
                        }
                        // End getting address property based on workpermit

                        List<ChecklistAddressPropertyMaping> ChecklistAddressPropertyMaping2 = new List<ChecklistAddressPropertyMaping>();//9,17,3
                        foreach (var addressproperty in ListOfProperties)
                        {
                            var mappeditems = ChecklistAddressPropertyMaping1.Where(x => x.IdChecklistAddressProperty == addressproperty).ToList();//fetch omly those items which matches with jobs properties
                            foreach (var b in mappeditems)
                            {
                                if (b.IdChecklistAddressProperty == 3) //check if it has ownertype property
                                {
                                    if (Convert.ToInt32(b.Value) == rfpAddress.IdOwnerType)
                                    {
                                        ChecklistAddressPropertyMaping2.Add(b);
                                        continue; ;
                                    }
                                    else
                                        continue;
                                }
                                if (b.IdChecklistAddressProperty == 4)//check if it has special district property
                                {
                                    string[] multipleDistricts = b.Value.Split(',');

                                    foreach (string item in multipleDistricts)
                                    {
                                        if (rfpAddress.SpecialDistrict.Contains(item.Trim()))
                                        {
                                            ChecklistAddressPropertyMaping2.Add(b);
                                            continue;
                                        }
                                    }

                                    continue;
                                }
                                ChecklistAddressPropertyMaping2.Add(b);
                            }
                        }

                        List<ChecklistAddressPropertyMaping> ChecklistAddressPropertyMaping4 = new List<ChecklistAddressPropertyMaping>(); //all from distinct final item
                        foreach (var item in Distinctfinalitem)
                        {
                            var itemsinaddressproperty = db.ChecklistAddressPropertyMaping.Where(x => x.IdChecklistItem == item.Id).ToList();

                            foreach (var a in itemsinaddressproperty)
                            {
                                ChecklistAddressPropertyMaping4.Add(a);
                            }
                        }

                        List<ChecklistAddressPropertyMaping> ChecklistAddressPropertyMapingRemoveItems = new List<ChecklistAddressPropertyMaping>();
                        Expression<Func<ChecklistAddressPropertyMaping, bool>> worktypeExpression2 = null;
                        if (ListOfProperties != null && ListOfProperties.Count > 0)
                        {
                            foreach (var property in ListOfProperties)
                            {

                                //if (property == 3 || property == 4)//check if it has special district property
                                //{
                                //    continue;
                                //}

                                if (worktypeExpression2 == null)
                                {
                                    worktypeExpression2 = e => e.IdChecklistAddressProperty != null && e.IdChecklistAddressProperty != property;
                                }
                                else
                                {
                                    var compile = worktypeExpression2.Compile();
                                    worktypeExpression2 = e => compile(e) && e.IdChecklistAddressProperty != property;
                                }
                            }
                            if (ChecklistAddressPropertyMaping4.Count > 0)
                            {
                                if (worktypeExpression2 != null)
                                {
                                    ChecklistAddressPropertyMapingRemoveItems = ChecklistAddressPropertyMaping4.Where(worktypeExpression2.Compile()).ToList(); //remove from distinct final item not 3,9,17

                                }
                                var removeItemsAddressProperty3 = ChecklistAddressPropertyMaping4
                                    .Where(e => e.IdChecklistAddressProperty == 3 && e.Value != Convert.ToString(rfpAddress.IdOwnerType)).ToList();

                                var removeItemsAddressProperty4 = ChecklistAddressPropertyMaping4
                                    .Where(e => e.IdChecklistAddressProperty == 4 && e.Value.Contains(Convert.ToString(rfpAddress.SpecialDistrict))).ToList();

                                ChecklistAddressPropertyMapingRemoveItems.AddRange(removeItemsAddressProperty3);
                                ChecklistAddressPropertyMapingRemoveItems.AddRange(removeItemsAddressProperty4);  //2184 should come here
                            }
                        }
                        else
                        {
                            ChecklistAddressPropertyMapingRemoveItems.AddRange(ChecklistAddressPropertyMaping4);
                        }
                        foreach (var item in ChecklistAddressPropertyMapingRemoveItems)
                        {
                            var items = db.ChecklistItems.Where(cl => cl.Id == item.IdChecklistItem);
                            foreach (var a in items)
                                Distinctfinalitem.Remove(a);
                        }
                        //Propertywise item End logic
                        List<ChecklistItem> Propertywise_Items1 = new List<ChecklistItem>();
                        //check this

                        foreach (var c in ChecklistAddressPropertyMaping2)
                        {
                            var items = db.ChecklistItems.Where(cl => cl.Id == c.IdChecklistItem);
                            foreach (var i in items)
                                Propertywise_Items1.Add(i);
                        }

                        List<ChecklistItem> removeitemswithoutsameapplication1 = new List<ChecklistItem>();
                        removeitemswithoutsameapplication1 = Propertywise_Items1.Where(x => (!string.IsNullOrWhiteSpace(x.IdJobApplicationTypes) && !x.IdJobApplicationTypes.Contains(Convert.ToString(Id_applicationtype)))).ToList();

                        Expression<Func<ChecklistItem, bool>> worktypeExpression1 = null;
                        foreach (var WorkTypeId in WorkTypeIds)
                        {
                            if (WorkTypeIds.IndexOf(WorkTypeId) == 0)
                            {
                                // worktypeExpression1 = e => !string.IsNullOrWhiteSpace(e.IdJobWorkTypes) && !e.IdJobWorkTypes.Contains(Convert.ToString(WorkTypeId));
                                worktypeExpression1 = e => !string.IsNullOrWhiteSpace(e.IdJobWorkTypes) && !((e.IdJobWorkTypes.Split(',')).ToArray().Contains(Convert.ToString(WorkTypeId)));
                            }
                            else
                            {
                                if (worktypeExpression1 != null)
                                {
                                    var compile = worktypeExpression1.Compile();
                                    // worktypeExpression1 = e => compile(e) && !e.IdJobWorkTypes.Contains(Convert.ToString(WorkTypeId));
                                    worktypeExpression1 = e => compile(e) && !((e.IdJobWorkTypes.Split(',')).ToArray().Contains(Convert.ToString(WorkTypeId)));
                                }
                            }
                        }

                        List<ChecklistItem> removeitemswithoutsamepermit1 = new List<ChecklistItem>();
                        if (worktypeExpression1 != null)
                        {
                            removeitemswithoutsamepermit1 = Propertywise_Items1.Where(worktypeExpression1.Compile()).ToList();
                        }

                        foreach (var item in removeitemswithoutsameapplication1)
                        {
                            Propertywise_Items1.Remove(item);
                        }
                        foreach (var item in removeitemswithoutsamepermit1)
                        {
                            Propertywise_Items1.Remove(item);
                        }
                        Distinctfinalitem.AddRange(Propertywise_Items1);
                        var distinct_Final_Items1 = Distinctfinalitem.Distinct().ToList();
                        // //End of logic
                        #endregion
                        #endregion

                        //add item in jobchecklistitemdetail table
                        List<JobChecklistItemDetail> lstjobChecklistItemDetail = new List<JobChecklistItemDetail>();

                        foreach (var final_item in distinct_Final_Items1)
                        {
                            //  fetch data from groupdetailtable according to itemid and header id and save in itemdetail table
                            JobChecklistItemDetail jobChecklistItemDetail = new JobChecklistItemDetail();
                            jobChecklistItemDetail.IdChecklistItem = final_item.Id;

                            var result = (from ca in db.JobChecklistGroups
                                          where ca.IdJobCheckListHeader == jobChecklistHeaderDTO.IdJobCheckListHeader && ca.IdCheckListGroup == final_item.IdCheckListGroup
                                          select new
                                          {
                                              ca.Id
                                          }).ToList();
                            jobChecklistItemDetail.IdJobChecklistGroup = result[result.Count - 1].Id;
                            jobChecklistItemDetail.CreatedDate = DateTime.UtcNow;
                            jobChecklistItemDetail.LastModifiedDate = DateTime.UtcNow;
                            lstjobChecklistItemDetail.Add(jobChecklistItemDetail);

                            if (employee != null)
                            {
                                jobChecklistItemDetail.CreatedBy = employee.Id;
                            }
                        }
                        db.JobChecklistItemDetails.AddRange(lstjobChecklistItemDetail);
                        if (lstjobChecklistItemDetail.Count > 0)
                        {
                            try
                            {
                                db.SaveChanges();
                            }
                            catch (Exception)
                            {
                                throw new RpoBusinessException("Error in Saving/Retriving New items of checklist");
                            }
                        }
                    }
                    var lstcompositechecklists = db.CompositeChecklistDetails.Where(x => x.IdJobChecklistHeader == jobChecklistHeaderDTO.IdJobCheckListHeader).Select(j => j.IdCompositeChecklist).Distinct().ToList();
                    foreach (var c in lstcompositechecklists)
                    {
                        foreach (var g in lstNewchecklistGroup)
                        {
                            CompositeChecklistDetail compositeChecklistDetail = new CompositeChecklistDetail();
                            compositeChecklistDetail.IdJobChecklistGroup = g.Id;
                            compositeChecklistDetail.IdJobChecklistHeader = jobChecklistHeaderDTO.IdJobCheckListHeader;
                            compositeChecklistDetail.IdCompositeChecklist = c;
                            compositeChecklistDetail.IsParentCheckList = db.CompositeChecklistDetails.Where(a => a.IdCompositeChecklist == c).Select(a => a.IsParentCheckList).FirstOrDefault();
                            db.CompositeChecklistDetails.Add(compositeChecklistDetail);
                            try
                            {
                                db.SaveChanges();
                            }
                            catch (Exception)
                            {
                                throw new RpoBusinessException("Error in Saving New group in Composite checklist");
                            }
                        }
                    }
                }

                return Ok("Checklist Edited Successfully");
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        #endregion

        [ResponseType(typeof(JobChecklistHeaderDTO))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/GetJobChecklist/{id}")]
        public IHttpActionResult GetJobChecklist(int id)
        {
            JobChecklistHeader jobChecklistHeader = db.JobChecklistHeaders.Include("JobApplicationWorkPermitTypes").FirstOrDefault(x => x.IdJobCheckListHeader == id);

            if (jobChecklistHeader == null)
            {
                return this.NotFound();
            }
            var jobChecklistGroupDetail = db.JobChecklistGroups.Where(x => x.IdJobCheckListHeader == id);
            List<CheckListGroup> lstchecklistGroup = new List<CheckListGroup>();
            List<JobPlumbingChecklistFloors> lstJobPlumbingChecklistFloors = new List<JobPlumbingChecklistFloors>();


            if (jobChecklistGroupDetail != null)
            {
                foreach (var a in jobChecklistGroupDetail)
                {

                    CheckListGroup checklistGroup = db.CheckListGroups.FirstOrDefault(x => x.Id == a.IdCheckListGroup);
                    lstchecklistGroup.Add(checklistGroup);
                    //for plumbing details floors and inspections
                    if (checklistGroup.Type.ToLower() == "pl")
                    {
                        var floors = db.JobPlumbingChecklistFloors.Where(x => x.IdJobCheckListHeader == id).ToList();

                        foreach (var b in floors)
                        {
                            JobPlumbingChecklistFloors jobPlumbingChecklistFloors = new JobPlumbingChecklistFloors();
                            jobPlumbingChecklistFloors.Id = b.Id;
                            jobPlumbingChecklistFloors.FloonNumber = b.FloonNumber;
                            jobPlumbingChecklistFloors.FloorDisplayOrder = b.FloorDisplayOrder;
                            var inspections = db.JobPlumbingInspection.Where(x => x.IdJobPlumbingCheckListFloors == b.Id).ToList();
                            List<InspectionType> lstInspectionType = new List<InspectionType>();
                            foreach (var c in inspections)
                            {
                                InspectionType inspectionType = new InspectionType();
                                var item = db.ChecklistItems.Where(x => x.Id == c.IdChecklistItem).FirstOrDefault();
                                inspectionType.Id = item.Id;
                                inspectionType.Name = item.Name;
                                lstInspectionType.Add(inspectionType);
                            }
                            jobPlumbingChecklistFloors.InspectionType = lstInspectionType;
                            lstJobPlumbingChecklistFloors.Add(jobPlumbingChecklistFloors);

                        }
                    }
                }
            }
            string permittype = "";
            string IdPermit = "";
            foreach (var jobWorkTypes in jobChecklistHeader.JobApplicationWorkPermitTypes)
            {
                IdPermit += jobWorkTypes.Id.ToString() + ", ";
                permittype += jobWorkTypes.Code.ToString() + ", ";
            }

            var result = db.JobApplications.Include("JobApplicationType").Where(x => x.Id == jobChecklistHeader.IdJobApplication);
            string applicationnumber = result.FirstOrDefault().ApplicationNumber;
            string applicationtype = result.FirstOrDefault().JobApplicationType.Description;
            string Title = applicationtype + " - " + applicationnumber;

            JobChecklistHeaderDTO jobChecklistHeaderDTO = new JobChecklistHeaderDTO();

            jobChecklistHeaderDTO.IdJobCheckListHeader = jobChecklistHeader.IdJobCheckListHeader;
            jobChecklistHeaderDTO.IdJobApplication = jobChecklistHeader.IdJobApplication;
            jobChecklistHeaderDTO.IdJob = jobChecklistHeader.IdJob;
            jobChecklistHeaderDTO.ApplicationTypeNumber = Title;
            jobChecklistHeaderDTO.strIdJobApplicationWorkPermitTypes = IdPermit.Remove(IdPermit.Length - 2, 2);
            jobChecklistHeaderDTO.strJobApplicationWorkPermitTypes = permittype.Remove(permittype.Length - 2, 2);

            jobChecklistHeaderDTO.LastModifiedBy = jobChecklistHeader.LastModifiedBy;
            jobChecklistHeaderDTO.LastModifiedDate = jobChecklistHeader.LastModifiedDate;
            jobChecklistHeaderDTO.NoOfFloors = jobChecklistHeader.NoOfFloors;
            jobChecklistHeaderDTO.CheckListGroups = jobChecklistGroupDetail != null ? lstchecklistGroup : null;
            jobChecklistHeaderDTO.JobPlumbingChecklistFloors = lstJobPlumbingChecklistFloors;
            return Ok(jobChecklistHeaderDTO);

        }
        private JobChecklistHeaderDTO MapJobChecklistToDTO(JobChecklistHeader newjobChecklistHeader)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            JobChecklistHeaderDTO jobChecklistHeaderDTO = new JobChecklistHeaderDTO();
            jobChecklistHeaderDTO.IdJobCheckListHeader = newjobChecklistHeader.IdJobCheckListHeader;
            return jobChecklistHeaderDTO;
        }

        [Authorize]
        [RpoAuthorize]
        [Route("api/checklist/PostJobChecklistItemFromOtherGroup/{JobchecklistGroupDetailId}/{IdJobPlumbingCheckListFloors}")]
        public IHttpActionResult PostJobChecklistItemFromOtherGroup(int JobchecklistGroupDetailId, int IdJobPlumbingCheckListFloors, [FromBody] List<ChecklistItem> listChecklistItems)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                var Groupdetail = db.JobChecklistGroups.Find(JobchecklistGroupDetailId);
                string grouptype = Groupdetail.ChecklistGroups.Type;
                if (grouptype.ToLower() != "pl")
                {
                    foreach (var jobChecklistManualItem in listChecklistItems)
                    {
                        JobChecklistItemDetail eachchecklistItem = new JobChecklistItemDetail();
                        eachchecklistItem.IdChecklistItem = jobChecklistManualItem.Id;
                        eachchecklistItem.IdJobChecklistGroup = JobchecklistGroupDetailId;
                        eachchecklistItem.CreatedDate = DateTime.UtcNow;
                        eachchecklistItem.LastModifiedDate = DateTime.UtcNow;
                        eachchecklistItem.Status = 1;
                        eachchecklistItem.IsActive = true;
                        if (employee != null)
                        {
                            eachchecklistItem.CreatedBy = employee.Id;
                            eachchecklistItem.LastModifiedBy = employee.Id;
                        }
                        db.JobChecklistItemDetails.Add(eachchecklistItem);
                    }
                    var jobchecklistheaderid = db.JobChecklistGroups.Where(x => x.Id == JobchecklistGroupDetailId).FirstOrDefault().IdJobCheckListHeader;

                    JobChecklistHeader jobChecklistHeader = db.JobChecklistHeaders.Find(jobchecklistheaderid);
                    jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;
                    db.SaveChanges();
                }
                else
                {
                    int headerid = db.JobChecklistGroups.Find(JobchecklistGroupDetailId).IdJobCheckListHeader;
                    db.JobPlumbingChecklistFloors.Where(x => x.IdJobCheckListHeader == headerid);
                    foreach (var jobChecklistManualItem in listChecklistItems)
                    {
                        JobPlumbingInspection eachchecklistItem = new JobPlumbingInspection();
                        eachchecklistItem.IdChecklistItem = jobChecklistManualItem.Id;
                        eachchecklistItem.IdJobChecklistGroup = JobchecklistGroupDetailId;
                        eachchecklistItem.IdJobPlumbingCheckListFloors = IdJobPlumbingCheckListFloors;
                        eachchecklistItem.CreatedDate = DateTime.UtcNow;
                        eachchecklistItem.LastModifiedDate = DateTime.UtcNow;
                        eachchecklistItem.Status = 1;
                        eachchecklistItem.Result = "3";
                        eachchecklistItem.IsActive = true;
                        eachchecklistItem.PlumbingInspectionDisplayOrder = jobChecklistManualItem.DisplayOrderPlumbingInspection;
                        if (employee != null)
                        {
                            eachchecklistItem.CreatedBy = employee.Id;
                            eachchecklistItem.LastModifiedBy = employee.Id;
                        }
                        db.JobPlumbingInspection.Add(eachchecklistItem);
                    }
                    var jobchecklistheaderid = db.JobChecklistGroups.Where(x => x.Id == JobchecklistGroupDetailId).FirstOrDefault().IdJobCheckListHeader;

                    JobChecklistHeader jobChecklistHeader = db.JobChecklistHeaders.Find(jobchecklistheaderid);
                    jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;
                    db.SaveChanges();
                }

                return Ok("Added Items successfully");
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        [ResponseType(typeof(JobChecklistHeaderDTO))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/PutJobChecklistHeader/{headerId}")]
        public IHttpActionResult PutJobChecklistHeader(int headerId, [FromBody] JobChecklistItemDetailDTO checklistHeader)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                JobChecklistHeader jobChecklistHeader = db.JobChecklistHeaders.Find(headerId);
                if (jobChecklistHeader.IdJobCheckListHeader != 0)
                {
                    jobChecklistHeader.Others = checklistHeader.Others;
                    jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;
                }
                db.SaveChanges();
                return Ok("Header Updated Successfully");
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
    }
    public class JobCheckListCommentHistoryAdvancedSearchParameters : DataTableParameters
    {
        /// <summary>
        /// Gets or sets the identifier RFP.
        /// </summary>
        /// <value>The identifier RFP.</value>
        public int? Id { get; set; }
    }

}