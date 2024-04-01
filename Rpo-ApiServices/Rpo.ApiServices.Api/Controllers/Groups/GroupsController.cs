// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-05-2018
// ***********************************************************************
// <copyright file="GroupsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Groups Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.Groups
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Permissions;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Filters;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Model.Models.Enums;
    using Tools;
    /// <summary>
    /// Class Groups Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />

    public class GroupsController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the groups.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttpActionResult.</returns>
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetGroups([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewEmployeeUserGroup)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEmployeeUserGroup)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteEmployeeUserGroup))
            {
                var recordsTotal = rpoContext.Groups.Count();
                var recordsFiltered = recordsTotal;

                var result = rpoContext.Groups
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
        /// Lists the groups.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [Route("api/groups/list")]
        [ResponseType(typeof(IEnumerable<Group>))]
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        public IHttpActionResult ListGroups()
        {
            var result = rpoContext.Groups.ToArray();
            return Ok(result);
        }

        /// <summary>
        /// Gets the group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(Group))]
        public IHttpActionResult GetGroup(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewEmployeeUserGroup)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEmployeeUserGroup)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteEmployeeUserGroup))
            {
                Group group = rpoContext.Groups.Find(id);
                if (group == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetail(group));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="groupCreateOrUpdateDTO">The group create or update dto.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGroup(int id, GroupCreateOrUpdateDTO groupCreateOrUpdateDTO)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEmployeeUserGroup))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Group group = groupCreateOrUpdateDTO.CloneAs<Group>();

                if (id != group.Id)
                {
                    return BadRequest();
                }

                if (groupCreateOrUpdateDTO.Permissions != null && groupCreateOrUpdateDTO.Permissions.Count() > 0)
                {
                    group.Permissions = string.Join(",", groupCreateOrUpdateDTO.Permissions.Select(x => x.ToString()));
                }
                else
                {
                    group.Permissions = string.Empty;
                }

                rpoContext.Entry(group).State = EntityState.Modified;

                try
                {
                    rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the group.
        /// </summary>
        /// <param name="groupCreateOrUpdateDTO">The group create or update dto.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(Group))]
        public IHttpActionResult PostGroup(GroupCreateOrUpdateDTO groupCreateOrUpdateDTO)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEmployeeUserGroup))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Group group = groupCreateOrUpdateDTO.CloneAs<Group>();

                if (groupCreateOrUpdateDTO.Permissions != null && groupCreateOrUpdateDTO.Permissions.Count() > 0)
                {
                    group.Permissions = string.Join(",", groupCreateOrUpdateDTO.Permissions.Select(x => x.ToString()));
                }
                else
                {
                    group.Permissions = string.Empty;
                }


                rpoContext.Groups.Add(group);
                rpoContext.SaveChanges();

                return this.CreatedAtRoute("DefaultApi", new { id = group.Id }, group);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        /// <exception cref="RpoBusinessException">Cannot Delete - User Group Assigned to Active Employees</exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(Group))]
        public IHttpActionResult DeleteGroup(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteEmployeeUserGroup))
            {
                Group group = rpoContext.Groups.Find(id);
                if (group == null)
                {
                    return this.NotFound();
                }

                if (rpoContext.Employees.Any(e => e.IdGroup == id))
                    throw new RpoBusinessException("Cannot Delete - User Group Assigned to Active Employees");

                rpoContext.Groups.Remove(group);
                rpoContext.SaveChanges();

                return Ok(group);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

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
        /// Groups the exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool GroupExists(int id)
        {
            return rpoContext.Groups.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Statuses the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">if set to <c>true</c> [status].</param>
        /// <returns>IHttpActionResult.</returns>
        [ResponseType(typeof(void))]
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/groups/{id:int}/status")]
        public IHttpActionResult Status(int id, [FromBody] bool status)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEmployeeUserGroup))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Group group = rpoContext.Groups.Find(id);
                if (group == null)
                {
                    return this.NotFound();
                }

                group.IsActive = status;

                rpoContext.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        private GroupDTO FormatDetail(Group group)
        {
            List<int> groupPermissions = group.Permissions != null && !string.IsNullOrEmpty(group.Permissions) ? (group.Permissions.Split(',') != null && group.Permissions.Split(',').Any() ? group.Permissions.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

            return new GroupDTO
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                Permissions = groupPermissions.ToArray(),
                AllPermissions = GetPermission(groupPermissions)

            };
        }

        private List<PermissionModuleDTO> GetPermission(List<int> groupPermissions)
        {
            var permissions = rpoContext.Permissions.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

            List<PermissionModuleDTO> permissionModuleList = new List<PermissionModuleDTO>();
            var moduleList = permissions.Select(x => x.ModuleName).Distinct();
            PermissionsController permissionsController = new PermissionsController();

            foreach (var module in moduleList)
            {
                PermissionModuleDTO permissionModule = new PermissionModuleDTO();
                List<PermissionGroupDTO> permissionGroupList = new List<PermissionGroupDTO>();
                permissionModule.ModuleName = module;

                var groupList = permissions.Where(x => x.ModuleName == module).Select(x => new { x.GroupName, x.DisplayOrder }).Distinct().OrderBy(x => x.DisplayOrder);
                foreach (var group in groupList)
                {
                    PermissionGroupDTO permissionGroup = new PermissionGroupDTO();
                    List<PermissionsDTO> permissionDTOList = new List<PermissionsDTO>();
                    permissionGroup.GroupName = group.GroupName;
                    var permissionList = permissions.Where(x => x.GroupName == group.GroupName).ToList();
                    foreach (var item in permissionList)
                    {
                        PermissionsDTO permissionsDTO = permissionsController.Format(item);
                        if (groupPermissions.Contains(item.Id))
                        {
                            permissionsDTO.IsChecked = true;
                        }
                        else
                        {
                            permissionsDTO.IsChecked = false;
                        }
                        permissionDTOList.Add(permissionsDTO);
                    }

                    permissionGroup.Permissions = permissionDTOList;
                    permissionGroupList.Add(permissionGroup);
                }

                permissionModule.Groups = permissionGroupList;
                permissionModuleList.Add(permissionModule);
            }

            return permissionModuleList;
        }
    }
}