// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-16-2018
// ***********************************************************************
// <copyright file="TransmissionTypesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Transmission Types Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Transmission Types namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.TransmissionTypes
{
    using System;
    using System.Data;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using DataTable;
    using Filters;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Transmission Types Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class TransmissionTypesController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the transmission types.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the transmission types List.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetTransmissionTypes([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var transmissionTypes = rpoContext.TransmissionTypes.Include("DefaultCC.Employee").Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = transmissionTypes.Count();
                var recordsFiltered = recordsTotal;

                var result = transmissionTypes
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
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the transmission type drop down.
        /// </summary>
        /// <returns> Gets the transmission type drop down.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/transmissiontypes/dropdown")]
        public IHttpActionResult GetTransmissionTypeDropdown()
        {
            var result = rpoContext.TransmissionTypes.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the type of the transmission.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns> Gets the transmission type .</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TransmissionTypeDetail))]
        public IHttpActionResult GetTransmissionType(int id)
        {
            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                TransmissionType transmissionType = rpoContext.TransmissionTypes.Include("DefaultCC.Employee").Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (transmissionType == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(transmissionType));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the type of the transmission.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="transmissionType">Type of the transmission.</param>
        /// <returns>update the transmission type drop down.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TransmissionTypeDetail))]
        public IHttpActionResult PutTransmissionType(int id, TransmissionTypeCreateUpdateDTO transmissionType)
        {
            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != transmissionType.Id)
                {
                    return BadRequest();
                }

                if (TransmissionTypeNameExists(transmissionType.Name, transmissionType.Id))
                {
                    throw new RpoBusinessException(StaticMessages.TransmissionTypeNameExistsMessage);
                }
                
                TransmissionType newTransmissionType = rpoContext.TransmissionTypes.Include("DefaultCC").FirstOrDefault(x => x.Id == id);
                newTransmissionType.Name = transmissionType.Name;
                newTransmissionType.IsSendEmail = transmissionType.IsSendEmail;
                newTransmissionType.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    newTransmissionType.LastModifiedBy = employee.Id;
                }

                try
                {
                    rpoContext.SaveChanges();
                    //if (newTransmissionType.DefaultCC.Any())
                    //{
                    //    rpoContext.TransmissionTypeDefaultCCs.RemoveRange(newTransmissionType.DefaultCC);
                    //    rpoContext.SaveChanges();
                    //}

                    //foreach (TransmissionTypeDefaultCC item in transmissionType.DefaultCC)
                    //{
                    //    TransmissionTypeDefaultCC transmissionTypeDefaultCC = new TransmissionTypeDefaultCC();
                    //    transmissionTypeDefaultCC.IdEmployee = item.IdEmployee;
                    //    transmissionTypeDefaultCC.IdTransmissionType = newTransmissionType.Id;
                    //    rpoContext.TransmissionTypeDefaultCCs.Add(transmissionTypeDefaultCC);
                    //    rpoContext.SaveChanges();
                    //}

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransmissionTypeExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TransmissionType transmissionTypeResponse = rpoContext.TransmissionTypes.Include("DefaultCC.Employee").Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return Ok(FormatDetails(transmissionTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the type of the transmission.
        /// </summary>
        /// <param name="transmissionType">Type of the transmission.</param>
        /// <returns>create the transmission type.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TransmissionTypeDetail))]
        public IHttpActionResult PostTransmissionType(TransmissionTypeCreateUpdateDTO transmissionType)
        {
            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (TransmissionTypeNameExists(transmissionType.Name, transmissionType.Id))
                {
                    throw new RpoBusinessException(StaticMessages.TransmissionTypeNameExistsMessage);
                }

                TransmissionType newTransmissionType = new TransmissionType();
                newTransmissionType.Name = transmissionType.Name;
                newTransmissionType.IsSendEmail = transmissionType.IsSendEmail;
                newTransmissionType.LastModifiedDate = DateTime.UtcNow;
                newTransmissionType.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    newTransmissionType.CreatedBy = employee.Id;
                }

                rpoContext.TransmissionTypes.Add(newTransmissionType);
                rpoContext.SaveChanges();

                //foreach (TransmissionTypeDefaultCC item in transmissionType.DefaultCC)
                //{
                //    TransmissionTypeDefaultCC transmissionTypeDefaultCC = new TransmissionTypeDefaultCC();
                //    transmissionTypeDefaultCC.IdEmployee = item.IdEmployee;
                //    transmissionTypeDefaultCC.IdTransmissionType = newTransmissionType.Id;
                //    rpoContext.TransmissionTypeDefaultCCs.Add(transmissionTypeDefaultCC);
                //    rpoContext.SaveChanges();
                //}

                TransmissionType transmissionTypeResponse = rpoContext.TransmissionTypes.Include("DefaultCC.Employee").Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == newTransmissionType.Id);
                return Ok(FormatDetails(transmissionTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the type of the transmission.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Deletes the type of the transmission.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [ResponseType(typeof(TransmissionType))]
        public IHttpActionResult DeleteTransmissionType(int id)
        {
            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                TransmissionType transmissionType = rpoContext.TransmissionTypes.Find(id);
                if (transmissionType == null)
                {
                    return this.NotFound();
                }

                rpoContext.TransmissionTypes.Remove(transmissionType);
                rpoContext.SaveChanges();

                return Ok(transmissionType);
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
        /// Transmissions the type exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool TransmissionTypeExists(int id)
        {
            return rpoContext.TransmissionTypes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified transmission type.
        /// </summary>
        /// <param name="transmissionType">Type of the transmission.</param>
        /// <returns>TransmissionTypeDTO.</returns>
        private TransmissionTypeDTO Format(TransmissionType transmissionType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new TransmissionTypeDTO
            {
                Id = transmissionType.Id,
                Name = transmissionType.Name,
                //DefaultCC = transmissionType.DefaultCC != null ? string.Join(", ", transmissionType.DefaultCC.Select(x =>
                //    x.Employee != null ? (x.Employee.FirstName
                //    + (!string.IsNullOrWhiteSpace(x.Employee.LastName) ? " " + x.Employee.LastName : string.Empty)
                //    + (!string.IsNullOrWhiteSpace(x.Employee.Email) ? " (" + x.Employee.Email + ")" : string.Empty)).ToString()
                //    : string.Empty

                //)
                //) : string.Empty,
                IsSendEmail = transmissionType.IsSendEmail,
                CreatedBy = transmissionType.CreatedBy,
                LastModifiedBy = transmissionType.LastModifiedBy,
                CreatedByEmployeeName = transmissionType.CreatedByEmployee != null ? transmissionType.CreatedByEmployee.FirstName + " " + transmissionType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = transmissionType.LastModifiedByEmployee != null ? transmissionType.LastModifiedByEmployee.FirstName + " " + transmissionType.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = transmissionType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(transmissionType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : transmissionType.CreatedDate,
                LastModifiedDate = transmissionType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(transmissionType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : transmissionType.LastModifiedDate,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="transmissionType">Type of the transmission.</param>
        /// <returns>TransmissionTypeDetail.</returns>
        private TransmissionTypeDetail FormatDetails(TransmissionType transmissionType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new TransmissionTypeDetail
            {
                Id = transmissionType.Id,
                Name = transmissionType.Name,
                //DefaultCC = transmissionType.DefaultCC.Select(x => new TransmissionTypeDefaultCCDetail
                //{
                //    Id = x.Id,
                //    IdEmployee = x.IdEmployee,
                //    //IdTransmissionType = x.IdTransmissionType,
                //    Employee = x.Employee != null
                //    ? (x.Employee.FirstName
                //    + (!string.IsNullOrWhiteSpace(x.Employee.LastName) ? " " + x.Employee.LastName : string.Empty)
                //    + (!string.IsNullOrWhiteSpace(x.Employee.Email) ? " (" + x.Employee.Email + ")" : string.Empty)).ToString()
                //    : string.Empty
                //}),
                IsSendEmail = transmissionType.IsSendEmail,
                CreatedBy = transmissionType.CreatedBy,
                LastModifiedBy = transmissionType.LastModifiedBy,
                CreatedByEmployeeName = transmissionType.CreatedByEmployee != null ? transmissionType.CreatedByEmployee.FirstName + " " + transmissionType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = transmissionType.LastModifiedByEmployee != null ? transmissionType.LastModifiedByEmployee.FirstName + " " + transmissionType.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = transmissionType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(transmissionType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : transmissionType.CreatedDate,
                LastModifiedDate = transmissionType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(transmissionType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : transmissionType.LastModifiedDate,
            };
        }

        /// <summary>
        /// Transmissions the type name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool TransmissionTypeNameExists(string name, int id)
        {
            return rpoContext.TransmissionTypes.Count(e => e.Name == name && e.Id != id) > 0;
        }
    }
}