// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-06-2018
// ***********************************************************************
// <copyright file="CitiesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Cities Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.Cities
{
    using System.Collections.Generic;
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
    /// Class Cities Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [Authorize]
    public class CitiesController : ApiController
    {
        /// <summary>
        /// The database.
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the cities.
        /// </summary>
        /// <returns>IEnumerable City.</returns>
        [Authorize]
        [RpoAuthorize]
        public IEnumerable<City> GetCities()
        {
            return this.rpoContext.Cities;
        }

        /// <summary>
        /// Gets the city.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttp Action Result.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(City))]
        public IHttpActionResult GetCity(int id)
        {
            City city = this.rpoContext.Cities.Find(id);
            if (city == null)
            {
                return this.NotFound();
            }

            return this.Ok(city);
        }

        /// <summary>
        /// Puts the city.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="city">The city name.</param>
        /// <returns>IHttp Action Result.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCity(int id, City city)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (id != city.Id)
            {
                return this.BadRequest();
            }

            this.rpoContext.Entry(city).State = EntityState.Modified;

            try
            {
                this.rpoContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.CityExists(id))
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
        /// Posts the city.
        /// </summary>
        /// <param name="city">The city object.</param>
        /// <returns>IHttp Action Result.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(City))]
        public IHttpActionResult PostCity(City city)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            this.rpoContext.Cities.Add(city);
            this.rpoContext.SaveChanges();

            return this.CreatedAtRoute("DefaultApi", new { id = city.Id }, city);
        }

        /// <summary>
        /// Deletes the city.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttp Action Result.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(City))]
        public IHttpActionResult DeleteCity(int id)
        {
            City city = this.rpoContext.Cities.Find(id);
            if (city == null)
            {
                return this.NotFound();
            }

            this.rpoContext.Cities.Remove(city);
            this.rpoContext.SaveChanges();

            return this.Ok(city);
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
        /// Cities the exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool CityExists(int id)
        {
            return this.rpoContext.Cities.Count(e => e.Id == id) > 0;
        }
    }
}