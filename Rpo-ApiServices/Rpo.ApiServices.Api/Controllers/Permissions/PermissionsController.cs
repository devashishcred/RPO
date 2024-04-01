// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-17-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-19-2018
// ***********************************************************************
// <copyright file="PermissionsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Permissions Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Permissions namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Permissions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using DataTable;
    using Rpo.ApiServices.Api.Filters;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Model.Models.Enums;

    /// <summary>
    /// Class Permissions Controller.
    /// </summary>
    public class PermissionsController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the permission.
        /// </summary>
        /// <returns>Get the list of permissions.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(PermissionModuleDTO))]
        public IHttpActionResult GetPermission()
        {
            var permissions = rpoContext.Permissions.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

            List<PermissionModuleDTO> permissionModuleList = new List<PermissionModuleDTO>();
            var moduleList = permissions.Select(x => x.ModuleName).Distinct();

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
                        permissionDTOList.Add(Format(item));
                    }
                    permissionGroup.Permissions = permissionDTOList;
                    permissionGroupList.Add(permissionGroup);
                }
                permissionModule.Groups = permissionGroupList;
                permissionModuleList.Add(permissionModule);
            }



            //var recordsTotal = permissions.Count();
            //var recordsFiltered = recordsTotal;

            //var result = permissions
            //    .AsEnumerable()
            //    .Select(c => Format(c))
            //    .AsQueryable()
            //    .DataTableParameters(dataTableParameters, out recordsFiltered)
            //    .ToArray();

            //return Ok(new DataTableResponse
            //{
            //    Draw = dataTableParameters.Draw,
            //    RecordsFiltered = recordsFiltered,
            //    RecordsTotal = recordsTotal,
            //    Data = result
            //});

            return Ok(permissionModuleList);
        }

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Get the detail of permissions.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(Permission))]
        public IHttpActionResult GetPermissions(int id)
        {
            Permission permissions = rpoContext.Permissions.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

            if (permissions == null)
            {
                return this.NotFound();
            }

            return Ok(Format(permissions));
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
        /// Formats the specified permission.
        /// </summary>
        /// <param name="permission">Type of the permission.</param>
        /// <returns>PermissionsDTO.</returns>
        public PermissionsDTO Format(Permission permission)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new PermissionsDTO
            {
                Id = permission.Id,
                Name = permission.Name,
                DisplayName = permission.DisplayName,
                PermissionClass = permission.PermissionClass,
                ModuleName = permission.ModuleName,
                DisplayOrder = permission.DisplayOrder,
                GroupName = permission.GroupName,
                CreatedBy = permission.CreatedBy,
                LastModifiedBy = permission.LastModifiedBy,
                CreatedByEmployeeName = permission.CreatedByEmployee != null ? permission.CreatedByEmployee.FirstName + " " + permission.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = permission.LastModifiedByEmployee != null ? permission.LastModifiedByEmployee.FirstName + " " + permission.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = permission.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(permission.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : permission.CreatedDate,
                LastModifiedDate = permission.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(permission.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : permission.LastModifiedDate,
            };
        }
    }
}
