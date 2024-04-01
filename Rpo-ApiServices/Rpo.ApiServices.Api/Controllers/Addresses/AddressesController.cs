// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-20-2018
// ***********************************************************************
// <copyright file="AddressesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Addresses Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Addresses namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Addresses
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Addresses Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [Authorize]
    public class AddressesController : ApiController
    {
        /// <summary>
        /// The database.
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the address.
        /// </summary>
        /// <returns>List of Address.</returns>
        [Authorize]
        [RpoAuthorize]
        public IQueryable<Address> GetAddresses()
        {
            return this.rpoContext.Addresses;
        }

        /// <summary>
        /// Gets the address.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttp Action Result.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(Address))]

        public IHttpActionResult GetAddress(int id)
        {
            Address address = this.rpoContext.Addresses.Find(id);
            if (address == null)
            {
                return this.NotFound();
            }

            return this.Ok(address);
        }

        /// <summary>
        /// Puts the address.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="address">The address.</param>
        /// <returns>Object of Address.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAddress(int id, Address address)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (id != address.Id)
            {
                return this.BadRequest();
            }

            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            address.LastModifiedDate = DateTime.UtcNow;
            if (employee != null)
            {
                address.LastModifiedBy = employee.Id;
            }

            this.rpoContext.Entry(address).State = EntityState.Modified;

            try
            {
                this.rpoContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.AddressExists(id))
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

        /// <summary>
        /// Posts the address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>create a new Address.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(Address))]
        public IHttpActionResult PostAddress(Address address)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            address.LastModifiedDate = DateTime.UtcNow;
            address.CreatedDate = DateTime.UtcNow;
            if (employee != null)
            {
                address.CreatedBy = employee.Id;
            }

            this.rpoContext.Addresses.Add(address);
            this.rpoContext.SaveChanges();

            return this.CreatedAtRoute("DefaultApi", new { id = address.Id }, address);
        }

        /// <summary>
        /// Deletes the address.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Object of Address.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(Address))]
        public IHttpActionResult DeleteAddress(int id)
        {
            Address address = this.rpoContext.Addresses.Find(id);
            if (address == null)
            {
                return this.NotFound();
            }

            this.rpoContext.Addresses.Remove(address);
            this.rpoContext.SaveChanges();

            return this.Ok(address);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.rpoContext.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Addresses the exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool AddressExists(int id)
        {
            return this.rpoContext.Addresses.Count(e => e.Id == id) > 0;
        }
    }
}