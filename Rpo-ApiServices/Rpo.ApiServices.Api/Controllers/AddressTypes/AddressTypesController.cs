// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-15-2018
// ***********************************************************************
// <copyright file="AddressTypesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Address Types Controller.</summary>
// ***********************************************************************

/// <summary>
/// The AddressTypes namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.AddressTypes
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using DataTable;
    using Filters;
    using Model.Models.Enums;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Address Types Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class AddressTypesController : ApiController
    {
        /// <summary>
        /// The database.
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the address types.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttp Action Result.</returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetAddressTypes([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var addressTypes = this.rpoContext.AddressTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = addressTypes.Count();
                var recordsFiltered = recordsTotal;

                var result = addressTypes
                    .AsEnumerable()
                    .Select(c => this.Format(c))
                    .AsQueryable()
                    .DataTableParameters(dataTableParameters, out recordsFiltered)
                    .ToArray();

                return this.Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Draw,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal,
                    Data = result.OrderBy(x => x.DisplayOrder)
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the address type drop down.
        /// </summary>
        /// <returns>IHttp Action Result.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/addresstypes/dropdown")]
        public IHttpActionResult GetAddressTypeDropdown()
        {
            var result = this.rpoContext.AddressTypes.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name,
                DisplayOrder = c.DisplayOrder
            }).ToArray();

            return this.Ok(result);
        }

        /// <summary>
        /// Gets the type of the address.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttp Action Result.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(AddressTypeDetail))]
        public IHttpActionResult GetAddressType(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                AddressType addressType = this.rpoContext.AddressTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (addressType == null)
                {
                    return this.NotFound();
                }

                return this.Ok(this.FormatDetails(addressType));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the type of the address.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="addressType">Type of the address.</param>
        /// <returns>IHttp Action Result.</returns>
        /// <exception cref="RpoBusinessException">
        /// Business Exception.
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAddressType(int id, AddressType addressType)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (id != addressType.Id)
                {
                    return this.BadRequest();
                }

                if (this.AddressTypeNameExists(addressType.Name, addressType.Id))
                {
                    throw new RpoBusinessException(StaticMessages.AddressTypeNameExistsMessage);
                }

                if (this.AddressTypePriorityExists(addressType.DisplayOrder, addressType.Id))
                {
                    throw new RpoBusinessException(StaticMessages.AddressTypePriorityExistsMessage);
                }

                addressType.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    addressType.LastModifiedBy = employee.Id;
                }

                this.rpoContext.Entry(addressType).State = EntityState.Modified;

                try
                {
                    this.rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.AddressTypeExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                AddressType addressTypeResponse = this.rpoContext.AddressTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return this.Ok(this.FormatDetails(addressTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the type of the address.
        /// </summary>
        /// <param name="addressType">Type of the address.</param>
        /// <returns>IHttp Action Result.</returns>
        /// <exception cref="RpoBusinessException">
        /// Business Exception.
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(AddressType))]
        public IHttpActionResult PostAddressType(AddressType addressType)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (this.AddressTypeNameExists(addressType.Name, addressType.Id))
                {
                    throw new RpoBusinessException(StaticMessages.AddressTypeNameExistsMessage);
                }

                if (this.AddressTypePriorityExists(addressType.DisplayOrder, addressType.Id))
                {
                    throw new RpoBusinessException(StaticMessages.AddressTypePriorityExistsMessage);
                }

                addressType.LastModifiedDate = DateTime.UtcNow;
                addressType.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    addressType.CreatedBy = employee.Id;
                }

                this.rpoContext.AddressTypes.Add(addressType);
                this.rpoContext.SaveChanges();

                AddressType addressTypeResponse = this.rpoContext.AddressTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == addressType.Id);
                return this.Ok(this.FormatDetails(addressTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the type of the address.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttp Action Result.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(AddressType))]
        public IHttpActionResult DeleteAddressType(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                AddressType addressType = this.rpoContext.AddressTypes.Find(id);
                if (addressType == null)
                {
                    return this.NotFound();
                }

                this.rpoContext.AddressTypes.Remove(addressType);
                this.rpoContext.SaveChanges();

                return this.Ok(addressType);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
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
        /// Addresses the type exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool AddressTypeExists(int id)
        {
            return this.rpoContext.AddressTypes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified address type.
        /// </summary>
        /// <param name="addressType">Type of the address.</param>
        /// <returns>Address Type DTO.</returns>
        private AddressTypeDTO Format(AddressType addressType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new AddressTypeDTO
            {
                Id = addressType.Id,
                Name = addressType.Name,
                DisplayOrder = addressType.DisplayOrder,
                CreatedBy = addressType.CreatedBy,
                LastModifiedBy = addressType.LastModifiedBy != null ? addressType.LastModifiedBy : addressType.CreatedBy,
                CreatedByEmployeeName = addressType.CreatedByEmployee != null ? addressType.CreatedByEmployee.FirstName + " " + addressType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = addressType.LastModifiedByEmployee != null ? addressType.LastModifiedByEmployee.FirstName + " " + addressType.LastModifiedByEmployee.LastName : (addressType.CreatedByEmployee != null ? addressType.CreatedByEmployee.FirstName + " " + addressType.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = addressType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(addressType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : addressType.CreatedDate,
                LastModifiedDate = addressType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(addressType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (addressType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(addressType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : addressType.CreatedDate),
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="addressType">Type of the address.</param>
        /// <returns>Address Type Detail.</returns>
        private AddressTypeDetail FormatDetails(AddressType addressType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new AddressTypeDetail
            {
                Id = addressType.Id,
                Name = addressType.Name,
                DisplayOrder = addressType.DisplayOrder,
                CreatedBy = addressType.CreatedBy,
                LastModifiedBy = addressType.LastModifiedBy != null ? addressType.LastModifiedBy : addressType.CreatedBy,
                CreatedByEmployeeName = addressType.CreatedByEmployee != null ? addressType.CreatedByEmployee.FirstName + " " + addressType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = addressType.LastModifiedByEmployee != null ? addressType.LastModifiedByEmployee.FirstName + " " + addressType.LastModifiedByEmployee.LastName : (addressType.CreatedByEmployee != null ? addressType.CreatedByEmployee.FirstName + " " + addressType.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = addressType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(addressType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : addressType.CreatedDate,
                LastModifiedDate = addressType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(addressType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (addressType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(addressType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : addressType.CreatedDate),
            };
        }

        /// <summary>
        /// Addresses the type name exists.
        /// </summary>
        /// <param name="name">The name address type.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>True</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool AddressTypeNameExists(string name, int id)
        {
            return this.rpoContext.AddressTypes.Count(e => e.Name == name && e.Id != id) > 0;
        }

        /// <summary>
        /// Addresses the type priority exists.
        /// </summary>
        /// <param name="priorityOrder">The priority order.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool AddressTypePriorityExists(int priorityOrder, int id)
        {
            return this.rpoContext.AddressTypes.Count(e => e.DisplayOrder == priorityOrder && e.Id != id) > 0;
        }
    }
}