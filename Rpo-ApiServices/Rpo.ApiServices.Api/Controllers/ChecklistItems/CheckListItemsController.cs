// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Mital Bhatt
// Created          : 30-08-2022
//
// Last Modified By :Mital Bhatt
// Last Modified On : 19-09-2022
// ***********************************************************************
// <copyright file="CheckListItemController.cs" company="CREDENCYS">
//     Copyright ©  2022
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/// <summary>
/// The CheckListItems namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.ChecklistItems
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
    using Models;
    using System.Collections;


    /// <summary>
    /// Class CheckListItemController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class ChecklistItemsController : ApiController
    {

        private RpoContext db = new RpoContext();
        //old
        ///// <summary>
        ///// Gets the borough.
        ///// </summary>
        ///// <param name="id">The identifier.</param>
        ///// <returns>Gets the boroughs in detail.</returns>
        ///// 
        ///// <summary>
        ///// Gets the CheklistGroup.
        ///// </summary>
        ///// <param name="dataTableParameters">The data table parameters.</param>
        ///// <returns>IHttpActionResult.</returns>
        //[Authorize]
        //[RpoAuthorize]
        //[ResponseType(typeof(DataTableResponse))]
        //public IHttpActionResult GetCheklistItems([FromUri] DataTableParameters dataTableParameters)
        //{
        //    var ChecklistItems = db.ChecklistItems.Include("CreatedByEmployee").Include("LastModifiedByEmployee").Include("JobWorkTypes").Include("CheckListGroup").AsQueryable();

        //    var recordsTotal = ChecklistItems.Count();
        //    var recordsFiltered = recordsTotal;

        //    var result = ChecklistItems
        //        .AsEnumerable().Where(x=>x.IsUserfillable==false)
        //        .Select(c => MapChecklistToChecklistDTO(c))
        //        .AsQueryable()
        //        .DataTableParameters(dataTableParameters, out recordsFiltered)
        //        .ToArray();

        //    return Ok(new DataTableResponse
        //    {
        //        Draw = dataTableParameters.Draw,
        //        RecordsFiltered = recordsFiltered,
        //        RecordsTotal = recordsTotal,
                
        //        Data = result               
        //    });
        //}
        /// <summary>
        /// Gets the borough.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the boroughs in detail.</returns>
        /// 
        /// <summary>
        /// Gets the CheklistGroup.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetCheklistItems([FromUri] DataTableParameters dataTableParameters)
        {
            var ChecklistItems = db.ChecklistItems.Include("CreatedByEmployee").Include("LastModifiedByEmployee").Include("CheckListGroup").AsQueryable();

            var recordsTotal = ChecklistItems.Count();
            var recordsFiltered = recordsTotal;

            var result = ChecklistItems
                .AsEnumerable().Where(x => x.IsUserfillable == false)
                .Select(c => MapChecklistToChecklistDTO(c))
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
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ChecklistItem))]
        public IHttpActionResult GetChecklistItem(int id)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            ChecklistItem checklistItemDTO = db.ChecklistItems.FirstOrDefault(x => x.Id == id);

            if (checklistItemDTO == null)
            {
                return this.NotFound();
            }
           ; return Ok(new ChecklistItemDetail
           {
               Id = checklistItemDTO.Id,
               Name = checklistItemDTO.Name,
               IdCheckListGroup = checklistItemDTO.IdCheckListGroup,
               IsActive = checklistItemDTO.IsActive,
               IsUserfillable = checklistItemDTO.IsUserfillable,
               IdJobWorkTypes = checklistItemDTO.IdJobWorkTypes,
               IdJobApplicationTypes = checklistItemDTO.IdJobApplicationTypes,
               JobApplicationTypes = checklistItemDTO.JobApplicationTypes,
               JobWorkTypes = checklistItemDTO.JobWorkTypes,
               ReferenceNote = checklistItemDTO.ReferenceNote,
               ExternalReferenceLink = checklistItemDTO.ExternalReferenceLink,
               InternalReferenceLink = checklistItemDTO.InternalReferenceLink,
               IdReferenceDocument = checklistItemDTO.IdReferenceDocument,
               CreatedByEmployeeName = checklistItemDTO.CreatedByEmployee != null ? checklistItemDTO.CreatedByEmployee.FirstName + " " + checklistItemDTO.CreatedByEmployee.LastName : string.Empty,
               LastModifiedByEmployeeName = checklistItemDTO.LastModifiedByEmployee != null ? checklistItemDTO.LastModifiedByEmployee.FirstName + " " + checklistItemDTO.LastModifiedByEmployee.LastName : (checklistItemDTO.CreatedByEmployee != null ? checklistItemDTO.CreatedByEmployee.FirstName + " " + checklistItemDTO.CreatedByEmployee.LastName : string.Empty),
               CreatedDate = DateTime.UtcNow,
               LastModifiedDate = DateTime.UtcNow,
               IdCreateFormDocument = checklistItemDTO.IdCreateFormDocument,
               IdUploadFormDocument = checklistItemDTO.IdUploadFormDocument,
               ReferenceDocuments = checklistItemDTO.ReferenceDocuments,

           });
        }

        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/GetChecklistAddressProperty")]
        public IHttpActionResult GetChecklistAddressProperty()
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            var result1 = db.ChecklistAddressProperty.Select(
                c => new
                {
                    IdChecklistAddressProperty = c.Id,
                    Description = c.Name
                })
                .ToArray();

            return Ok(result1);
        }

        [Authorize]
        [RpoAuthorize]
        private string[] GetChecklistItemsPropertycharacteristics(int id)
        {            
            List<string> special_district = new List<string>();
            List<string> owner_type = new List<string>();
            string[] address_Propertycollection = new string[100];

            var result = (from ca in db.ChecklistAddressPropertyMaping
                          join cp in db.ChecklistAddressProperty on ca.IdChecklistAddressProperty equals cp.Id
                          where ca.IdChecklistItem == id && ca.IsActive == true
                          join ot in db.OwnerTypes on ca.Value equals ot.Id.ToString() into gp
                          from st in gp.DefaultIfEmpty()
                          select new
                          {
                              ca.IdChecklistAddressProperty,
                              cp.Name,
                              ca.IdChecklistItem,
                              ca.Value,
                              ca.IsActive,
                              owner_name = st.Name
                          }).ToList();

            foreach (var resval in result)
            {                
                if (resval.Name != null && resval.Name != string.Empty && resval.Name != "Owner Type" && resval.Name.ToLower().TrimEnd() != "special district")
                { special_district.Add(resval.Name); }
                if (resval.Name.ToLower().TrimEnd() == "special district") { special_district.Add("Special-District:" + resval.Value); }
                if (resval.owner_name != null && resval.owner_name != string.Empty) { owner_type.Add("Owner-Type:" + resval.owner_name); }
            }

            address_Propertycollection = special_district.Concat(owner_type).ToArray();
            return (address_Propertycollection);
        }

        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ChecklistAddressPropertyMappingDTO))]
        [Route("api/Checklist/GetChecklistAddressPropertyMaping/{id}")]
        public IHttpActionResult GetChecklistAddressPropertyMaping(int id)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = db.ChecklistAddressPropertyMaping.Where(x => x.IdChecklistItem == id && x.IsActive == true).AsEnumerable().Select(
            c => new
            {
                Id = c.Id,
                IdChecklistItem = c.IdChecklistItem,
                IdChecklistAddressProperty = c.IdChecklistAddressProperty,
                Value = c.Value,
            })
               .ToArray();
            return Ok(result);
        }

        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ChecklistAddressPropertyMappingDTO))]
        [Route("api/Checklist/PutChecklistAddressPropertyMaping/{IdChecklistItem}")]
        public IHttpActionResult PutChecklistAddressPropertyMaping(int IdChecklistItem, [FromBody] List<ChecklistAddressPropertyMappingDTO> checklistAddressPropertyMappingDTO)
        {       
            IQueryable<ChecklistAddressPropertyMaping> checklistAddressPropertyMaping = db.ChecklistAddressPropertyMaping.Where(x => x.IdChecklistItem == IdChecklistItem);
            List<ChecklistAddressPropertyMappingDTO> varchecklistAddressPropertyMaping = new List<ChecklistAddressPropertyMappingDTO>();

            var result = db.ChecklistAddressPropertyMaping.Where(x => x.IdChecklistItem == IdChecklistItem).ToList();
            foreach (var a in checklistAddressPropertyMappingDTO)
            {
                if (a.IdChecklistAddressProperty == 3)
                {
                    ChecklistAddressPropertyMaping dbchecklistAddressPropertyMaping = db.ChecklistAddressPropertyMaping.FirstOrDefault(x => x.IdChecklistAddressProperty == 3 && x.IdChecklistItem == IdChecklistItem);
                    if (dbchecklistAddressPropertyMaping != null)
                        dbchecklistAddressPropertyMaping.Value = a.Value;

                }
                if (a.IdChecklistAddressProperty == 4)
                {
                    ChecklistAddressPropertyMaping dbchecklistAddressPropertyMaping = db.ChecklistAddressPropertyMaping.FirstOrDefault(x => x.IdChecklistAddressProperty == 4 && x.IdChecklistItem == IdChecklistItem);
                    if (dbchecklistAddressPropertyMaping != null)
                        dbchecklistAddressPropertyMaping.Value = a.Value;

                }

            }
            if (result != null)
            {
                foreach (var addressproperty in checklistAddressPropertyMappingDTO.Where(ct => !result.Any(ctdb => ctdb.IdChecklistAddressProperty == ct.IdChecklistAddressProperty)))
                {                  
                    ChecklistAddressPropertyMaping addItem = new ChecklistAddressPropertyMaping();
                    addItem.Id = addressproperty.Id;
                    addItem.IdChecklistAddressProperty = addressproperty.IdChecklistAddressProperty;
                    addItem.IdChecklistItem = addressproperty.IdChecklistItem;
                    addItem.IsActive = addressproperty.IsActive;
                    addItem.Value = addressproperty.Value;
                    db.ChecklistAddressPropertyMaping.Add(addItem);
                }
            }

            var checklistAddressPropertyMapingToRemove = result
                                     .Where(ctdb => !checklistAddressPropertyMappingDTO.Any(ct => ct.IdChecklistAddressProperty == ctdb.IdChecklistAddressProperty))
                                     .ToList();

            foreach (var item in checklistAddressPropertyMapingToRemove)
            {
                db.ChecklistAddressPropertyMaping.Remove(item);                
            }

            if (!checklistAddressPropertyMappingDTO.Any())
            {
                foreach (var item in result)
                {
                    db.ChecklistAddressPropertyMaping.Remove(item);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChecklistItemExists(IdChecklistItem))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok("Editing Completed Successfully");
        }

        /// <summary>
        /// Posts the ChecklistItem.
        /// </summary>
        /// <param name="checkListItemDetail">The ChecklistItem.</param>
        /// <returns>IHttpActionResult.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        ///
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ChecklistItemDTO))]
        public IHttpActionResult PostCheckListItem(ChecklistItemDetail checkListItemDetail)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }        
            if (ChecklistItemNameExists(checkListItemDetail.Name, checkListItemDetail.IdCheckListGroup))
            {
                throw new RpoBusinessException(StaticMessages.ChecklistItemNameExistsMessage);
            }

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            ChecklistItem checklistItem = new ChecklistItem();
            checklistItem.Name = checkListItemDetail.Name;
            checklistItem.IdCheckListGroup = checkListItemDetail.IdCheckListGroup;
            checklistItem.IsActive = checkListItemDetail.IsActive;
            checklistItem.IsUserfillable = checkListItemDetail.IsUserfillable;
            checklistItem.ReferenceNote = checkListItemDetail.ReferenceNote;
            checklistItem.ExternalReferenceLink = checkListItemDetail.ExternalReferenceLink;
            checklistItem.InternalReferenceLink = checkListItemDetail.InternalReferenceLink;
            checklistItem.IdReferenceDocument = checkListItemDetail.IdReferenceDocument;
            checklistItem.IdCreateFormDocument = checkListItemDetail.IdCreateFormDocument;
            checklistItem.IdUploadFormDocument = checkListItemDetail.IdUploadFormDocument;
            checklistItem.CreatedDate = DateTime.UtcNow;
            checklistItem.LastModifiedDate = DateTime.UtcNow;
            if (employee != null)
            {
                checklistItem.CreatedBy = employee.Id;
            }
            db.ChecklistItems.Add(checklistItem);
            if (checkListItemDetail.JobApplicationTypes != null && checkListItemDetail.JobApplicationTypes.Count() > 0)
            {
                if (checklistItem.JobApplicationTypes == null)
                {
                    checklistItem.JobApplicationTypes = new HashSet<JobApplicationType>();
                }
                string a = "";
                foreach (var JobApplicationType in checkListItemDetail.JobApplicationTypes)
                {
                    checklistItem.JobApplicationTypes.Add(db.JobApplicationTypes.Find(JobApplicationType.Id));
                    a += JobApplicationType.Id.ToString() + ",";
                }   
                checklistItem.IdJobApplicationTypes = a.Remove(a.Length - 1, 1);

            }
            if (checkListItemDetail.JobWorkTypes != null && checkListItemDetail.JobWorkTypes.Count()>0)
            {
                if (checklistItem.JobWorkTypes == null)
                {
                    checklistItem.JobWorkTypes = new HashSet<JobWorkType>();
                }
                string b = "";
                foreach (var jobWorkTypes in checkListItemDetail.JobWorkTypes)
                {
                    checklistItem.JobWorkTypes.Add(db.JobWorkTypes.Find(jobWorkTypes.Id));
                    b += jobWorkTypes.Id.ToString() + ",";
                }
                checklistItem.IdJobWorkTypes = b.Remove(b.Length - 1, 1);
            }
            if (checkListItemDetail.ReferenceDocuments != null && checkListItemDetail.ReferenceDocuments.Count() > 0)
            {
                if (checklistItem.ReferenceDocuments == null)
                {
                    checklistItem.ReferenceDocuments = new HashSet<ReferenceDocument>();
                }
                string b = "";
                foreach (var referenceDocuments in checkListItemDetail.ReferenceDocuments)
                {
                    checklistItem.ReferenceDocuments.Add(db.ReferenceDocuments.Find(referenceDocuments.Id));
                    b += referenceDocuments.Id.ToString() + ",";
                }
                checklistItem.IdReferenceDocument = b.Remove(b.Length - 1, 1);
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new RpoBusinessException(ex.Message);
            }
            ChecklistItem newChecklistItem = db.ChecklistItems.Find(checklistItem.Id);

            return Ok(MapChecklistToChecklistDTO(newChecklistItem));

        }


        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ChecklistAddressPropertyMappingDTO))]
        [Route("api/Checklist/PostChecklistAddressPropertyMaping")]
        public IHttpActionResult PostChecklistAddressPropertyMaping([FromBody] List<ChecklistAddressPropertyMappingDTO> checklistAddressPropertyMappingDTO)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            List<ChecklistAddressPropertyMaping> lstChecklistProp = new List<ChecklistAddressPropertyMaping>();
            ChecklistAddressPropertyMaping checklistAddressPropertyMaping;
            foreach (var checklistAddressPropertyMappingDTOdetail in checklistAddressPropertyMappingDTO)
            {
                checklistAddressPropertyMaping = new ChecklistAddressPropertyMaping();
                checklistAddressPropertyMaping.Value = checklistAddressPropertyMappingDTOdetail.Value;
                checklistAddressPropertyMaping.IdChecklistItem = checklistAddressPropertyMappingDTOdetail.IdChecklistItem;
                checklistAddressPropertyMaping.IsActive = checklistAddressPropertyMappingDTOdetail.IsActive;
                checklistAddressPropertyMaping.IdChecklistAddressProperty = checklistAddressPropertyMappingDTOdetail.IdChecklistAddressProperty;
                checklistAddressPropertyMaping.ChecklistAddressProperty = db.ChecklistAddressProperty.FirstOrDefault(x => x.Id == checklistAddressPropertyMappingDTOdetail.IdChecklistAddressProperty);               
                lstChecklistProp.Add(checklistAddressPropertyMaping);               
            }
            db.ChecklistAddressPropertyMaping.AddRange(lstChecklistProp);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }

            return Ok(lstChecklistProp);

        }


        [ResponseType(typeof(ChecklistItemDetail))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult PutChecklistItem(int id, ChecklistItemDetail checkListItemDetail)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != checkListItemDetail.Id)
                {
                    return BadRequest();
                }
                if (OtherChecklistItemNameExists(checkListItemDetail.Name, checkListItemDetail.IdCheckListGroup, id))
                {
                    throw new RpoBusinessException(StaticMessages.ChecklistItemNameExistsMessage);
                }

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                ChecklistItem checklistItem = db.ChecklistItems.Find(id);

                checklistItem.Name = checkListItemDetail.Name;
                checklistItem.IsActive = checkListItemDetail.IsActive;
                checklistItem.ReferenceNote = checkListItemDetail.ReferenceNote;
                checklistItem.ExternalReferenceLink = checkListItemDetail.ExternalReferenceLink;
                checklistItem.InternalReferenceLink = checkListItemDetail.InternalReferenceLink;
                checklistItem.IdReferenceDocument = checkListItemDetail.IdReferenceDocument;
                checklistItem.IdCreateFormDocument = checkListItemDetail.IdCreateFormDocument;
                checklistItem.IdUploadFormDocument = checkListItemDetail.IdUploadFormDocument;
                checklistItem.CreatedDate = DateTime.UtcNow;
                checklistItem.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    checklistItem.LastModifiedBy = employee.Id;
                }

                if (checkListItemDetail.JobApplicationTypes != null)
                {
                    foreach (var jobApplicationTypes in checkListItemDetail.JobApplicationTypes.Where(ct => !checklistItem.JobApplicationTypes.Any(ctdb => ctdb.Id == ct.Id)))
                    {
                        checklistItem.JobApplicationTypes.Add(db.JobApplicationTypes.Find(jobApplicationTypes.Id));
                    }

                    var JobApplicationTypesToRemove = checklistItem.JobApplicationTypes.Where(ctdb => !checkListItemDetail.JobApplicationTypes.Any(ct => ct.Id == ctdb.Id)).ToList();

                    foreach (var item in JobApplicationTypesToRemove)
                    {
                        checklistItem.JobApplicationTypes.Remove(db.JobApplicationTypes.Find(item.Id));

                    }
                    checklistItem.IdJobApplicationTypes = checkListItemDetail.IdJobApplicationTypes;
                }

                if (checkListItemDetail.JobWorkTypes != null)
                {
                    foreach (var jobWorkTypes in checkListItemDetail.JobWorkTypes.Where(ct => !checklistItem.JobWorkTypes.Any(ctdb => ctdb.Id == ct.Id)))
                    {
                        checklistItem.JobWorkTypes.Add(db.JobWorkTypes.Find(jobWorkTypes.Id));

                    }

                    var JobWorkTypesToRemove = checklistItem.JobWorkTypes.Where(ctdb => !checkListItemDetail.JobWorkTypes.Any(ct => ct.Id == ctdb.Id)).ToList();

                    foreach (var item in JobWorkTypesToRemove)
                    {
                        checklistItem.JobWorkTypes.Remove(db.JobWorkTypes.Find(item.Id));

                    }
                    checklistItem.IdJobWorkTypes = checkListItemDetail.IdJobWorkTypes;
                }
                else
                {
                    var JobWorkTypesToRemove = checklistItem.JobWorkTypes.ToList();
                    foreach (var item in JobWorkTypesToRemove)
                    {
                        checklistItem.JobWorkTypes.Remove(db.JobWorkTypes.Find(item.Id));
                    }
                    checklistItem.IdJobWorkTypes = null;
                }

                if (checkListItemDetail.ReferenceDocuments != null)
                {
                    foreach (var referenceDocuments in checkListItemDetail.ReferenceDocuments.Where(ct => !checklistItem.ReferenceDocuments.Any(ctdb => ctdb.Id == ct.Id)))
                    {
                        checklistItem.ReferenceDocuments.Add(db.ReferenceDocuments.Find(referenceDocuments.Id));

                    }

                    var ReferenceDocumentsToRemove = checklistItem.ReferenceDocuments.Where(ctdb => !checkListItemDetail.ReferenceDocuments.Any(ct => ct.Id == ctdb.Id)).ToList();

                    foreach (var item in ReferenceDocumentsToRemove)
                    {
                        checklistItem.ReferenceDocuments.Remove(db.ReferenceDocuments.Find(item.Id));

                    }
                    checklistItem.IdReferenceDocument = checkListItemDetail.IdReferenceDocument;
                }
                else
                {
                    var ReferenceDocumentsToRemove = checklistItem.ReferenceDocuments.ToList();
                    foreach (var item in ReferenceDocumentsToRemove)
                    {
                        checklistItem.ReferenceDocuments.Remove(db.ReferenceDocuments.Find(item.Id));
                    }
                    checklistItem.IdReferenceDocument = null;
                }

                db.Entry(checklistItem).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChecklistItemExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok(MapChecklistToChecklistDTO(checklistItem));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }


        [ResponseType(typeof(ChecklistItemDetail))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/PutChecklistItemIsActive/{id}/{IsActive}")]
        public IHttpActionResult PutChecklistItemIsActive(int id, bool IsActive)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            ChecklistItem checklistItem = db.ChecklistItems.Find(id);
            checklistItem.IsActive = IsActive;
            checklistItem.CreatedDate = DateTime.UtcNow;
            checklistItem.LastModifiedDate = DateTime.UtcNow;
            if (employee != null)
            {
                checklistItem.LastModifiedBy = employee.Id;
            }
            db.Entry(checklistItem).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChecklistItemExists(id))
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
        /// Worktype Dropdown based on application type's multi selection.
        /// </summary>
        /// <param name="ids">The identifier.</param>        
        /// <returns>IHttpActionResult.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/WorkpermitTypeDropdown")]
        public IHttpActionResult GetWorkpermitTypeDropdown([FromBody] ChecklistItemPermitDTO permitDTO)
        {
            string[] a = permitDTO.PermitId.Split(',');
            int[] ApplicationTypeIds = Array.ConvertAll(a, int.Parse);           

            var result1 = db.JobWorkTypes.Include("JobApplicationType").Where(c => ApplicationTypeIds.Contains(c.IdJobApplicationType)).AsEnumerable().Select(
              c => new
              {
                  Id = c.Id,
                  ItemName = c.JobApplicationType.Description + " - " + c.Description
              })
              .ToArray();
            return Ok(result1);
        }


        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/AllWorkpermitTypeDropdown")]
        public IHttpActionResult GetAllWorkpermitTypeDropdown()
        {
            var result1 = db.JobWorkTypes.Include("JobApplicationType").AsEnumerable().Select(
              c => new
              {
                  Id = c.Id,
                  ItemName = c.JobApplicationType.Description + " - " + c.Description

              })
              .ToArray();
            return Ok(result1);
        }

        /// <summary>
        /// InternalReferenceDocumentDropdown.
        /// </summary>
        /// <param name="ids">The identifier.</param>        
        /// <returns>IHttpActionResult.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/InternalReferenceDocumentDropdown")]
        public IHttpActionResult GetInternalReferenceDocumentDropdown()
        {
            var result = db.ReferenceDocuments
                       .Select(rd => new
                       {
                           Id = rd.Id,
                           ItemName = rd.Name,
                           Description = rd.Description,
                           FileName = rd.FileName,
                           contentpath = rd.ContentPath,
                           Keywords = rd.Keywords,
                           Name = rd.Name
                       })
                       .ToArray();
            return Ok(result);
        }

        [Authorize]
        [RpoAuthorize]
        private ChecklistItemDTO MapChecklistToChecklistDTO(ChecklistItem checklistItemDTO)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            ChecklistItemDTO cd = new ChecklistItemDTO();
            cd.Id = checklistItemDTO.Id;
            cd.Name = checklistItemDTO.Name;
            cd.IdCheckListGroup = checklistItemDTO.IdCheckListGroup;
            if (checklistItemDTO.CheckListGroup != null)
                cd.CheckListGroupName = checklistItemDTO.CheckListGroup.Name;
            cd.JobApplicationTypes = checklistItemDTO.JobApplicationTypes;
            cd.IdJobApplicationTypes = checklistItemDTO.IdJobApplicationTypes;
            cd.IdJobWorkTypes = checklistItemDTO.IdJobWorkTypes;
            List<string> strworktypes = new List<string>();
            if (!string.IsNullOrWhiteSpace(cd.IdJobWorkTypes))
            {
                string[] worktypes = cd.IdJobWorkTypes.Split(',');

                foreach (var a in worktypes)
                {
                    int worktypeid = Convert.ToInt32(a);
                    var result1 = db.JobWorkTypes.Include("JobApplicationType").Where(x => x.Id == worktypeid).Select(
                   c => new
                   {   
                       code = c.JobApplicationType.Description + "-" + c.Code

                   }).FirstOrDefault();
                strworktypes.Add(result1.code);
            }
            }
            string WorkpermitCode = string.Join(", ", strworktypes);         
            cd.JobWorkTypewithapplication = WorkpermitCode;
            cd.JobWorkTypes = null;
           // cd.JobWorkTypes = checklistItemDTO.JobWorkTypes;
            cd.IsUserfillable = checklistItemDTO.IsUserfillable;
            cd.ReferenceNote = checklistItemDTO.ReferenceNote;
            cd.IsActive = checklistItemDTO.IsActive;
            cd.CreatedBy = checklistItemDTO.CreatedBy;
            cd.LastModifiedBy = checklistItemDTO.LastModifiedBy;
            cd.ExternalReferenceLink = checklistItemDTO.ExternalReferenceLink;
            cd.CreatedDate = checklistItemDTO.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(checklistItemDTO.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : checklistItemDTO.CreatedDate;
            cd.LastModifiedDate = checklistItemDTO.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(checklistItemDTO.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : checklistItemDTO.LastModifiedDate;
            cd.IdCreateFormDocument = checklistItemDTO.IdCreateFormDocument;
            cd.IdUploadFormDocument = checklistItemDTO.IdUploadFormDocument;
            cd.ReferenceDocuments = checklistItemDTO.ReferenceDocuments;
            cd.IdReferenceDocuments = checklistItemDTO.IdReferenceDocument;
            cd.ChecklistAddressPropertyMapings = GetChecklistItemsPropertycharacteristics(cd.Id);
            return cd;
        }
        private bool ChecklistItemNameExists(string name, int id)
        {
            return db.ChecklistItems.Count(e => e.Name == name && e.IdCheckListGroup == id && e.IsUserfillable == false) > 0;
        }
        private bool OtherChecklistItemNameExists(string name, int idgroup, int idItem)
        {
            return db.ChecklistItems.Count(e => e.Name == name && e.IdCheckListGroup == idgroup && e.Id!= idItem && e.IsUserfillable == false) > 0;
        }
        private bool ChecklistItemExists(int id)
        {
            return db.ChecklistItems.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Deletes the CheckListItem.
        /// </summary>
        /// <param name="id">The identifier.</param>
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/DeleteCheckListItem/{id}")]
        public IHttpActionResult DeleteCheckListItem(int id)
        {
            string Message = string.Empty;
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            try
            {
                var IschecklistItemExists = db.JobChecklistItemDetails.Count(b => b.IdChecklistItem == id);
                ChecklistItem checklistItem = db.ChecklistItems.Find(id);
                if (IschecklistItemExists == 0)
                {
                    var jobapplicationtypesToRemove = checklistItem.JobApplicationTypes.ToList();
                    foreach (var item in jobapplicationtypesToRemove)
                    {
                        checklistItem.JobApplicationTypes.Remove(db.JobApplicationTypes.Find(item.Id));
                    }
                    var jobWorkTypesToRemove = checklistItem.JobWorkTypes.ToList();
                    foreach (var item in jobWorkTypesToRemove)
                    {
                        db.ChecklistItems.Find(id).JobWorkTypes.Remove(db.JobWorkTypes.Find(item.Id));
                    }
                    var ChecklistAddressPropertyMapingsToRemove = db.ChecklistAddressPropertyMaping.Where(x => x.IdChecklistItem == id).ToList();
                    foreach (var item in ChecklistAddressPropertyMapingsToRemove)
                    {
                        db.ChecklistAddressPropertyMaping.Remove(item);                       
                    }
                    this.db.ChecklistItems.Remove(checklistItem);                 
                    
                    Message = StaticMessages.DeleteCheckListItem_Success;
                }
                else
                {
                    checklistItem.IsActive = false;
                    checklistItem.CreatedDate = DateTime.UtcNow;
                    checklistItem.LastModifiedDate = DateTime.UtcNow;
                    checklistItem.LastModifiedBy = employee.Id;
                    Message = StaticMessages.DeleteCheckListItem_InActive;
                }

                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChecklistItemExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(Message);
        }

        /// <summary>
        /// Gets the special district dropdown values.
        /// </summary>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/GetSpecialDistrictDropdown")]
        public IHttpActionResult GetSpecialDistrictDropdown()
        {
            var result = (from rf in db.RfpAddresses
                          where rf.SpecialDistrict != null && rf.SpecialDistrict != string.Empty
                          orderby rf.SpecialDistrict ascending
                          select new { rf.SpecialDistrict }).Distinct().ToList();
            return Ok(result);
        }

        /// <summary>
        /// Manage the orders of the checlist Items.
        /// </summary>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/ManageChecklistItemOrder")]
        public IHttpActionResult ManageChecklistItemOrder([FromBody] ManageOrderchecklistITemDTO manageOrderchecklistITemDTO)
        {
            var Message = string.Empty;
            try
            {
                if (manageOrderchecklistITemDTO != null)
                {
                    foreach (var detailid in manageOrderchecklistITemDTO.Items)
                    {
                        var result = db.JobChecklistItemDetails.Where(x => x.Id == detailid.jobChecklistItemsDetailsId).ToList();
                        foreach (var res in result)
                        {
                            res.Displayorder = detailid.DisplayOrder;
                        }
                        db.SaveChanges();
                        var JobChecklistGroupsID = db.JobChecklistItemDetails.Find(detailid.jobChecklistItemsDetailsId);
                        var jobchecklistheaderid = db.JobChecklistGroups.Where(x => x.Id == JobChecklistGroupsID.IdJobChecklistGroup).FirstOrDefault().IdJobCheckListHeader;
                        JobChecklistHeader jobChecklistHeader = db.JobChecklistHeaders.Find(jobchecklistheaderid);
                        jobChecklistHeader.LastModifiedDate = DateTime.UtcNow;
                        db.SaveChanges();
                    }
                    Message = StaticMessages.ManageGroupItem_Successful;
                }
                else { Message = StaticMessages.ManageOrder_UnSuccessful; }
            }
            catch (Exception ex) { throw ex.InnerException; }
            return Ok(Message);
        }
    }
}