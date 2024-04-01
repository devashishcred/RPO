



// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Mital Bhatt
// Created          : 20-08-2022
//
// Last Modified By :Mital Bhatt
// Last Modified On : 20-08-2022
// ***********************************************************************
// <copyright file="CheckListGroupController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/// <summary>
/// The CheckListGroups namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.CheckListGroups
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

    /// <summary>
    /// Class CheckListGroupController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class CheckListGroupController : ApiController
    {
        private RpoContext db = new RpoContext();


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
        public IHttpActionResult GetChekListGroup([FromUri] DataTableParameters dataTableParameters)
        {
            var CheckListGroups = db.CheckListGroups.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

            var recordsTotal = CheckListGroups.Count();
            var recordsFiltered = recordsTotal;

            var result = CheckListGroups
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
        public IHttpActionResult GetChecklistGroup(int id)
        {
            CheckListGroup ChecklistGroup = db.CheckListGroups.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

            if (ChecklistGroup == null)
            {
                return this.NotFound();
            }

            return Ok(FormatDetails(ChecklistGroup));
        }
        /// <summary>
        /// Posts the Checklist Group.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>IHttpActionResult.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        ///
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CheckListGroupDetail))]
        public IHttpActionResult PostCheckListGroup(CheckListGroup checkListGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (CheckListGroupNameExists(checkListGroup.Name, checkListGroup.Id))
            {
                throw new RpoBusinessException(StaticMessages.CheckListGroupNameExistsMessage);
            }
            if (CheckListGroupOrderExists(checkListGroup.Displayorder, checkListGroup.Id))
            {
                throw new RpoBusinessException(StaticMessages.CheckListGroupOrderExistsMessage);
            }           

            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            checkListGroup.LastModifiedDate = DateTime.UtcNow;
            checkListGroup.CreatedDate = DateTime.UtcNow;
            if (employee != null)
            {
                checkListGroup.CreatedBy = employee.Id;
            }

            db.CheckListGroups.Add(checkListGroup);
            try
            {
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new RpoBusinessException(ex.Message);
            }
            CheckListGroup CheckListGroupResponse = db.CheckListGroups.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == checkListGroup.Id);
            return Ok(FormatDetails(CheckListGroupResponse));
        }
        /// <summary>
        /// Puts the state.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="state">The state.</param>
        /// <returns>IHttpActionResult.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [ResponseType(typeof(CheckListGroupDetail))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult PutCheckListGroup(int id, CheckListGroup checkListGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != checkListGroup.Id)
            {
                return BadRequest();
            }

            if (CheckListGroupNameExists(checkListGroup.Name, checkListGroup.Id))
            {
                throw new RpoBusinessException(StaticMessages.StateNameExistsMessage);
            }
            if (CheckListGroupOrderExists(checkListGroup.Displayorder, checkListGroup.Id))
            {
                throw new RpoBusinessException(StaticMessages.CheckListGroupOrderExistsMessage);
            }

            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            checkListGroup.LastModifiedDate = DateTime.UtcNow;

            if (employee != null)
            {
                checkListGroup.LastModifiedBy = employee.Id;
            }

            db.Entry(checkListGroup).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckListGroupExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            CheckListGroup checkListGroupRespnse = db.CheckListGroups.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
            return Ok(FormatDetails(checkListGroupRespnse));
        }

        /// <summary>
        /// Gets the Ten Days Notice Salutation
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="idDocument">The identifier document.</param>
        /// <returns>Gets the Ten Days Notice Salutation list.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/GroupTypeDropdown")]
        public IHttpActionResult GetGroupTypeDropdown()
        {
            List<GroupTypeName> GroupTypeNameList = new List<GroupTypeName>();
            GroupTypeNameList.Add(new GroupTypeName { Id = "General", ItemName = "General" });
            GroupTypeNameList.Add(new GroupTypeName { Id = "TR", ItemName = "TR" });
            GroupTypeNameList.Add(new GroupTypeName { Id = "PL", ItemName = "PL" });
            return Ok(GroupTypeNameList);
        }
        /// <summary>
        /// Gets the contact dropdown.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>get the contact list against company</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/ChecklistGroup/ChecklistGroupdropdown")]
        public IHttpActionResult GetChecklistGroupdropdown()
        {          
                var result = db.CheckListGroups
                .Where(cg => cg.IsActive == true).AsEnumerable()
                    .Select(c => new
                    {
                        Id = c.Id,
                        GroupName=c.Name,
                       Type=c.Type
                    })
                    .ToArray();

                return Ok(result);   
        }
        /// <summary>
        /// Update the Checklist Group
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CheckListGroupDetail))]
        [Route("api/ChecklistGroup/IsActive")]
        public IHttpActionResult PutChecklistGroupIsActive(CheckListGroup checkListGroupDetail)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }
                CheckListGroup CheckListGroup = db.CheckListGroups.FirstOrDefault(x => x.Id == checkListGroupDetail.Id);
                if (CheckListGroup == null)
                {
                    return this.NotFound();
                }
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                CheckListGroup.IsActive = checkListGroupDetail.IsActive;
                CheckListGroup.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    CheckListGroup.LastModifiedBy = employee.Id;
                }
                try
                {
                    this.db.SaveChanges();
                }
                catch(Exception ex)
                {
                    throw new RpoBusinessException(ex.Message);
                }
                CheckListGroup CheckListGroupResponse = db.CheckListGroups.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == checkListGroupDetail.Id);
                return Ok(FormatDetails(CheckListGroupResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }   /// </summary>

            /// <summary>
            /// Deletes the CheckListGroup.
            /// </summary>
            /// <param name="id">The identifier.</param>
        [Authorize]
        [RpoAuthorize]
        [Route("api/Checklist/DeleteCheckListGroup/{id}")]
        public IHttpActionResult DeleteCheckListGroup(int id)
        {
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            var IscheckListGroupExists = db.JobChecklistGroups.Count(b => b.IdCheckListGroup == id);
            CheckListGroup checkListGroup = db.CheckListGroups.Find(id);           
            string isdelete = "Can Not Delete Because This Checklist Group Is Already In Use. It Is Inactivated Now.";
            if (IscheckListGroupExists == 0)
            {
                db.CheckListGroups.Remove(checkListGroup);
                isdelete = "Checklist Group Deleted Successfully";
            }
            else
            {
                checkListGroup.IsActive = false;
                checkListGroup.LastModifiedDate = DateTime.UtcNow;
                checkListGroup.LastModifiedBy = employee.Id; 
                
            }
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckListGroupExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(isdelete);
        }
        private CheckListGroupDTO Format(CheckListGroup checkListGroup)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new CheckListGroupDTO
            {
                Id = checkListGroup.Id,
                Name = checkListGroup.Name,
                Type = checkListGroup.Type,
                Displayorder = checkListGroup.Displayorder,
                IsActive = checkListGroup.IsActive,
                CreatedBy = checkListGroup.CreatedBy,
                LastModifiedBy = checkListGroup.LastModifiedBy,
                CreatedByEmployeeName = checkListGroup.CreatedByEmployee != null ? checkListGroup.CreatedByEmployee.FirstName + " " + checkListGroup.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = checkListGroup.LastModifiedByEmployee != null ? checkListGroup.LastModifiedByEmployee.FirstName + " " + checkListGroup.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = checkListGroup.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(checkListGroup.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : checkListGroup.CreatedDate,
                LastModifiedDate = checkListGroup.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(checkListGroup.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : checkListGroup.LastModifiedDate,
            };
        }
        private CheckListGroupDetail FormatDetails(CheckListGroup checkListGroup)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            return new CheckListGroupDetail
            {
                Id = checkListGroup.Id,
                Name = checkListGroup.Name,
                Type = checkListGroup.Type,
                Displayorder1 = checkListGroup.Displayorder,
                IsActive = checkListGroup.IsActive,
                CreatedBy = checkListGroup.CreatedBy,
                LastModifiedBy = checkListGroup.LastModifiedBy,
                CreatedByEmployeeName = checkListGroup.CreatedByEmployee != null ? checkListGroup.CreatedByEmployee.FirstName + " " + checkListGroup.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = checkListGroup.LastModifiedByEmployee != null ? checkListGroup.LastModifiedByEmployee.FirstName + " " + checkListGroup.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = checkListGroup.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(checkListGroup.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : checkListGroup.CreatedDate,
                LastModifiedDate = checkListGroup.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(checkListGroup.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : checkListGroup.LastModifiedDate,
            };
        }
        private bool CheckListGroupNameExists(string name, int id)
        {
            return db.CheckListGroups.Count(e => e.Name.ToLower() == name.ToLower() && e.Id != id) > 0;
        }
        private bool CheckListGroupExists(int id)
        {
            return db.CheckListGroups.Count(e => e.Id == id) > 0;
        }
        private bool CheckListGroupOrderExists(int? DisplayOrder, int id)
        {
            return db.CheckListGroups.Count(e => e.Displayorder == DisplayOrder && e.Id != id) > 0;
        }
        public class GroupTypeName
        {
            /// <summary>
            /// Gets or sets the identifier.
            /// </summary>
            /// <value>The identifier.</value>
            public string Id { get; set; }

            /// <summary>
            /// Gets or sets the name of the item.
            /// </summary>
            /// <value>The name of the item.</value>
            public string ItemName { get; set; }
        }

    }

}