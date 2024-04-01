// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-24-2018
// ***********************************************************************
// <copyright file="VerbiagesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Verbiages Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Verbiages namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Verbiages
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Verbiages Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class VerbiagesController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the verbiages.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the verbiages List.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetVerbiages([FromUri] DataTableParameters dataTableParameters)
        {
            //var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            //if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
            //      || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
            //      || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            //{
            var recordsTotal = this.rpoContext.Verbiages.Count();
            var recordsFiltered = recordsTotal;

            var result = this.rpoContext.Verbiages.Where(x => x.IsEditable == true)
                .AsQueryable()
                .DataTableParameters(dataTableParameters, out recordsFiltered)
                .ToArray();

            return this.Ok(new DataTableResponse
            {
                Draw = dataTableParameters.Draw,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal,
                Data = result
            });
            //}
            //else
            //{
            //    throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            //}
        }

        /// <summary>
        /// Gets the verbiage.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the verbiages.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(Verbiage))]
        public IHttpActionResult GetVerbiage(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                Verbiage verbiage = this.rpoContext.Verbiages.Find(id);
                if (verbiage == null)
                {
                    return this.NotFound();
                }

                return this.Ok(verbiage);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the verbiage dropdown.
        /// </summary>
        /// <returns>Gets the verbiages for dropdown.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/verbiages/dropdown")]
        public IHttpActionResult GetVerbiageDropdown()
        {
            var result = rpoContext.Verbiages.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                Name = c.Name,
                IsDefault = c.IsDefault,
                IsEditable = c.IsEditable,
                VerbiageType = c.VerbiageType
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Puts the verbiage.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="verbiage">The verbiage.</param>
        /// <returns>update the verbiage.<returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVerbiage(int id, Verbiage verbiage)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {

                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (id != verbiage.Id)
                {
                    return this.BadRequest();
                }

                if (VerbiageNameExists(verbiage.Name, verbiage.Id))
                {
                    throw new RpoBusinessException($"Verbiage name {verbiage.Name} is already exists.");
                }
                verbiage.LastModifiedByEmployee = null;
                verbiage.CreatedByEmployee = null;
                verbiage.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    verbiage.LastModifiedBy = employee.Id;
                }

                this.rpoContext.Entry(verbiage).State = EntityState.Modified;

                try
                {
                    this.rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.VerbiageExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the verbiage.
        /// </summary>
        /// <param name="verbiage">The verbiage.</param>
        /// <returns>IHttpActionResult.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(Verbiage))]
        public IHttpActionResult PostVerbiage(Verbiage verbiage)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (VerbiageNameExists(verbiage.Name, verbiage.Id))
                {
                    throw new RpoBusinessException($"Verbiage name {verbiage.Name} is already exists.");
                }
                verbiage.LastModifiedDate = DateTime.UtcNow;
                verbiage.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    verbiage.CreatedBy = employee.Id;
                }

                this.rpoContext.Verbiages.Add(verbiage);
                this.rpoContext.SaveChanges();

                return this.CreatedAtRoute("DefaultApi", new { id = verbiage.Id }, verbiage);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the verbiage.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(Verbiage))]
        public IHttpActionResult DeleteVerbiage(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                Verbiage verbiage = this.rpoContext.Verbiages.Find(id);
                if (verbiage == null)
                {
                    return this.NotFound();
                }

                this.rpoContext.Verbiages.Remove(verbiage);
                this.rpoContext.SaveChanges();

                return this.Ok(verbiage);
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
                this.rpoContext.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Verbiages the exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool VerbiageExists(int id)
        {
            return this.rpoContext.Verbiages.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Verbiages the name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool VerbiageNameExists(string name, int id)
        {
            return this.rpoContext.Verbiages.Count(e => e.Name == name && e.Id != id) > 0;
        }
    }
}