// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-20-2018
// ***********************************************************************
// <copyright file="StatesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

/// <summary>
/// The States namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.States
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
    /// Class StatesController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class StatesController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the states.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetStates([FromUri] DataTableParameters dataTableParameters)
        {
            var states = db.States.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

            var recordsTotal = states.Count();
            var recordsFiltered = recordsTotal;

            var result = states
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

        /// <summary>
        /// Gets the state dropdown.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/states/dropdown")]
        public IHttpActionResult GetStateDropdown()
        {
            var result = db.States.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Acronym,
                Name = c.Name,
                Acronym = c.Acronym
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(StateDetail))]
        public IHttpActionResult GetState(int id)
        {
            State state = db.States.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

            if (state == null)
            {
                return this.NotFound();
            }

            return Ok(FormatDetails(state));
        }

        /// <summary>
        /// Puts the state.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="state">The state.</param>
        /// <returns>IHttpActionResult.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [ResponseType(typeof(StateDetail))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult PutState(int id, State state)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != state.Id)
            {
                return BadRequest();
            }

            if (StateNameExists(state.Name, state.Id))
            {
                throw new RpoBusinessException(StaticMessages.StateNameExistsMessage);
            }

            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            state.LastModifiedDate = DateTime.UtcNow;
            if (employee != null)
            {
                state.LastModifiedBy = employee.Id;
            }

            db.Entry(state).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StateExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            State stateResponse = db.States.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
            return Ok(FormatDetails(stateResponse));
        }

        /// <summary>
        /// Posts the state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>IHttpActionResult.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(StateDetail))]
        public IHttpActionResult PostState(State state)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (StateNameExists(state.Name, state.Id))
            {
                throw new RpoBusinessException(StaticMessages.StateNameExistsMessage);
            }

            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            state.LastModifiedDate = DateTime.UtcNow;
            state.CreatedDate = DateTime.UtcNow;
            if (employee != null)
            {
                state.CreatedBy = employee.Id;
            }

            db.States.Add(state);
            db.SaveChanges();

            State stateResponse = db.States.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == state.Id);
            return Ok(FormatDetails(stateResponse));
        }

        /// <summary>
        /// Deletes the state.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(State))]
        public IHttpActionResult DeleteState(int id)
        {
            State state = db.States.Find(id);
            if (state == null)
            {
                return this.NotFound();
            }

            db.States.Remove(state);
            db.SaveChanges();

            return Ok(state);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary>
        /// States the exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool StateExists(int id)
        {
            return db.States.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>StateDTO.</returns>
        private StateDTO Format(State state)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new StateDTO
            {
                Id = state.Id,
                Name = state.Name,
                Acronym = state.Acronym,
                CreatedBy = state.CreatedBy,
                LastModifiedBy = state.LastModifiedBy,
                CreatedByEmployeeName = state.CreatedByEmployee != null ? state.CreatedByEmployee.FirstName + " " + state.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = state.LastModifiedByEmployee != null ? state.LastModifiedByEmployee.FirstName + " " + state.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = state.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(state.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : state.CreatedDate,
                LastModifiedDate = state.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(state.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : state.LastModifiedDate,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>StateDetail.</returns>
        private StateDetail FormatDetails(State state)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new StateDetail
            {
                Id = state.Id,
                Name = state.Name,
                Acronym = state.Acronym,
                CreatedBy = state.CreatedBy,
                LastModifiedBy = state.LastModifiedBy,
                CreatedByEmployeeName = state.CreatedByEmployee != null ? state.CreatedByEmployee.FirstName + " " + state.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = state.LastModifiedByEmployee != null ? state.LastModifiedByEmployee.FirstName + " " + state.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = state.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(state.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : state.CreatedDate,
                LastModifiedDate = state.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(state.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : state.LastModifiedDate,
            };
        }

        /// <summary>
        /// States the name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool StateNameExists(string name, int id)
        {
            return db.States.Count(e => e.Name == name && e.Id != id) > 0;
        }
    }
}