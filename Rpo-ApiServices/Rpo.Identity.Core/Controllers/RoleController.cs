// ***********************************************************************
// Assembly         : Rpo.Identity.Model
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="RoleController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Role Controller.</summary>
// ***********************************************************************

namespace Rpo.Identity.Api.Controllers
{
    using Rpo.Identity.Core.Infrastructure;
    using System.Linq;
    using System.Web.Http;

    /// <summary>
    /// Class Role Controller.
    /// </summary>
    /// <seealso cref="Rpo.Identity.Core.Infrastructure.RpoIdentityApiController" />
    public class RoleController : RpoIdentityApiController
    {
        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        [ActionName("GetAllRoles")]
        public IHttpActionResult GetAllRoles()
        {
            return Ok(RoleManager.Roles
                .ToList()
                .Select(t => new
                {
                    id = t.Id,
                    name = t.Name
                }));
        }
    }
}