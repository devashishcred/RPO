// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-21-2018
// ***********************************************************************
// <copyright file="EmailTypesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Email Types Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.EmailTypes
{
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using Model.Models.Enums;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Email Types Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />

    public class EmailTypesController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the email types.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttpActionResult.</returns>
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetEmailTypes([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var emailTypes = db.EmailTypes.Include("DefaultCC.Employee").Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();
                var recordsTotal = emailTypes.Count();
                var recordsFiltered = recordsTotal;

                var result = emailTypes
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
        /// Gets the email type dropdown.
        /// </summary>
        /// <param name="isRfp">if set to <c>true</c> [is RFP].</param>
        /// <param name="isCompany">if set to <c>true</c> [is company].</param>
        /// <param name="isJob">if set to <c>true</c> [is job].</param>
        /// <param name="isContact">if set to <c>true</c> [is contact].</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/emailtypes/dropdown")]
        public IHttpActionResult GetEmailTypeDropdown(bool isRfp = false, bool isCompany = false, bool isJob = false, bool isContact = false)
        {
            if (isRfp)
            {
                var result = db.EmailTypes.Where(x => x.IsRfp == isRfp).AsEnumerable().Select(c => new
                {
                    Id = c.Id,
                    ItemName = c.Name,
                    Name = c.Name,
                }).ToArray();

                return Ok(result);
            }
            else if (isCompany)
            {
                var result = db.EmailTypes.Where(x => x.IsCompany == isCompany).AsEnumerable().Select(c => new
                {
                    Id = c.Id,
                    ItemName = c.Name,
                    Name = c.Name,
                }).ToArray();

                return Ok(result);
            }
            else if (isJob)
            {
                var result = db.EmailTypes.Where(x => x.IsJob == isJob).AsEnumerable().Select(c => new
                {
                    Id = c.Id,
                    ItemName = c.Name,
                    Name = c.Name,
                }).ToArray();

                return Ok(result);
            }
            else if (isContact)
            {
                var result = db.EmailTypes.Where(x => x.IsContact == isContact).AsEnumerable().Select(c => new
                {
                    Id = c.Id,
                    ItemName = c.Name,
                    Name = c.Name,
                }).ToArray();

                return Ok(result);
            }
            else
            {
                var result = db.EmailTypes.AsEnumerable().Select(c => new
                {
                    Id = c.Id,
                    ItemName = c.Name,
                    Name = c.Name,
                }).ToArray();

                return Ok(result);
            }
        }

        /// <summary>
        /// Gets the type of the email.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(EmailType))]
        public IHttpActionResult GetEmailType(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddTransmittals)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewTransmittals))
            {
                EmailType emailType = db.EmailTypes.Include("DefaultCC.Employee").Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (emailType == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(emailType));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the type of the email.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="emailType">Type of the email.</param>
        /// <returns>IHttpActionResult.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmailType(int id, EmailTypeCreateUpdateDTO emailType)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != emailType.Id)
                {
                    return BadRequest();
                }

                if (EmailTypeNameExists(emailType.Name, emailType.Id))
                {
                    throw new RpoBusinessException(StaticMessages.EmailTypeNameExistsMessage);
                }

                EmailType newEmailType = db.EmailTypes.Include("DefaultCC").FirstOrDefault(x => x.Id == id);
                newEmailType.EmailBody = emailType.EmailBody;
                newEmailType.Name = emailType.Name;
                newEmailType.Description = emailType.Description;
                newEmailType.IsCompany = emailType.IsCompany;
                newEmailType.IsContact = emailType.IsContact;
                newEmailType.IsJob = emailType.IsJob;
                newEmailType.IsRfp = emailType.IsRfp;
                newEmailType.Subject = emailType.Subject;
                newEmailType.LastModifiedDate = DateTime.UtcNow;
                newEmailType.CreatedDate = DateTime.UtcNow;

                if (employee != null)
                {
                    newEmailType.LastModifiedBy = employee.Id;
                }

                db.Entry(newEmailType).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                    if (newEmailType.DefaultCC.Any())
                    {
                        db.TransmissionTypeDefaultCCs.RemoveRange(newEmailType.DefaultCC);
                        db.SaveChanges();
                    }

                    foreach (TransmissionTypeDefaultCC item in emailType.DefaultCC)
                    {
                        TransmissionTypeDefaultCC transmissionTypeDefaultCC = new TransmissionTypeDefaultCC();
                        transmissionTypeDefaultCC.IdEmployee = item.IdEmployee;
                        transmissionTypeDefaultCC.IdEamilType = newEmailType.Id;
                        db.TransmissionTypeDefaultCCs.Add(transmissionTypeDefaultCC);
                        db.SaveChanges();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmailTypeExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                EmailType emailTypeResponse = db.EmailTypes.Include("DefaultCC.Employee").Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                //EmailType emailTypeResponse = db.EmailTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return Ok(FormatDetails(emailTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the type of the email.
        /// </summary>
        /// <param name="emailType">Type of the email.</param>
        /// <returns>IHttpActionResult.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(EmailTypeDetail))]
        public IHttpActionResult PostEmailType(EmailTypeCreateUpdateDTO emailTypedto)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (EmailTypeNameExists(emailTypedto.Name, emailTypedto.Id))
                {
                    throw new RpoBusinessException(StaticMessages.EmailTypeNameExistsMessage);
                }
                EmailType newEmailType = new EmailType();
                newEmailType.EmailBody = emailTypedto.EmailBody;
                newEmailType.Name = emailTypedto.Name;
                newEmailType.Description = emailTypedto.Description;
                newEmailType.IsCompany = emailTypedto.IsCompany;
                newEmailType.IsContact = emailTypedto.IsContact;
                newEmailType.IsJob = emailTypedto.IsJob;
                newEmailType.IsRfp = emailTypedto.IsRfp;
                newEmailType.Subject = emailTypedto.Subject;
                newEmailType.LastModifiedDate = DateTime.UtcNow;
                newEmailType.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    newEmailType.CreatedBy = employee.Id;
                }

                db.EmailTypes.Add(newEmailType);
                db.SaveChanges();

                foreach (TransmissionTypeDefaultCC item in emailTypedto.DefaultCC)
                {
                    TransmissionTypeDefaultCC transmissionTypeDefaultCC = new TransmissionTypeDefaultCC();
                    transmissionTypeDefaultCC.IdEmployee = item.IdEmployee;
                    transmissionTypeDefaultCC.IdEamilType = newEmailType.Id;
                    //transmissionTypeDefaultCC.IdTransmissionType = newEmployeeType.Id;
                    db.TransmissionTypeDefaultCCs.Add(transmissionTypeDefaultCC);
                    db.SaveChanges();
                }

                EmailType emailTypeResponse = db.EmailTypes.Include("DefaultCC.Employee").Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == newEmailType.Id);
                //EmailType emailTypeResponse = db.EmailTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == emailTypedto.Id);
                return Ok(FormatDetails(emailTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the type of the email.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(EmailType))]
        public IHttpActionResult DeleteEmailType(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                EmailType emailType = db.EmailTypes.Find(id);
                if (emailType == null)
                {
                    return this.NotFound();
                }

                db.EmailTypes.Remove(emailType);
                db.SaveChanges();

                return Ok(emailType);
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
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Emails the type exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool EmailTypeExists(int id)
        {
            return db.EmailTypes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified email type.
        /// </summary>
        /// <param name="emailType">Type of the email.</param>
        /// <returns>EmailTypeDTO.</returns>
        private EmailTypeDTO Format(EmailType emailType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new EmailTypeDTO
            {
                Id = emailType.Id,
                Name = emailType.Name,
                Subject = emailType.Subject,
                Description = emailType.Description,
                EmailBody = emailType.EmailBody,
                IsCompany = emailType.IsCompany,
                DefaultCC = emailType.DefaultCC != null ? string.Join(", ", emailType.DefaultCC.Select(x =>
                    x.Employee != null ? (x.Employee.FirstName
                    + (!string.IsNullOrWhiteSpace(x.Employee.LastName) ? " " + x.Employee.LastName : string.Empty)
                    + (!string.IsNullOrWhiteSpace(x.Employee.Email) ? " (" + x.Employee.Email + ")" : string.Empty)).ToString()
                    : string.Empty

                )
                ) : string.Empty,
                IsContact = emailType.IsContact,
                IsJob = emailType.IsJob,
                IsRfp = emailType.IsRfp,
                CreatedBy = emailType.CreatedBy,
                LastModifiedBy = emailType.LastModifiedBy,
                CreatedByEmployeeName = emailType.CreatedByEmployee != null ? emailType.CreatedByEmployee.FirstName + " " + emailType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = emailType.LastModifiedByEmployee != null ? emailType.LastModifiedByEmployee.FirstName + " " + emailType.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = emailType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(emailType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : emailType.CreatedDate,
                LastModifiedDate = emailType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(emailType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : emailType.LastModifiedDate,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="emailType">Type of the email.</param>
        /// <returns>EmailTypeDetail.</returns>
        private EmailTypeDetail FormatDetails(EmailType emailType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new EmailTypeDetail
            {
                Id = emailType.Id,
                Name = emailType.Name,
                Subject = emailType.Subject,
                Description = emailType.Description,
                DefaultCC = emailType.DefaultCC.Select(x => new EmailTypeDefaultCCDetail
                {
                    Id = x.Id,
                    IdEmployee = x.IdEmployee,
                    Employee = x.Employee != null
                    ? (x.Employee.FirstName
                    + (!string.IsNullOrWhiteSpace(x.Employee.LastName) ? " " + x.Employee.LastName : string.Empty)
                    + (!string.IsNullOrWhiteSpace(x.Employee.Email) ? " (" + x.Employee.Email + ")" : string.Empty)).ToString()
                    : string.Empty
                }),
                EmailBody = emailType.EmailBody,
                IsCompany = emailType.IsCompany,
                IsContact = emailType.IsContact,
                IsJob = emailType.IsJob,
                IsRfp = emailType.IsRfp,
                CreatedBy = emailType.CreatedBy,
                LastModifiedBy = emailType.LastModifiedBy,
                CreatedByEmployeeName = emailType.CreatedByEmployee != null ? emailType.CreatedByEmployee.FirstName + " " + emailType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = emailType.LastModifiedByEmployee != null ? emailType.LastModifiedByEmployee.FirstName + " " + emailType.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = emailType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(emailType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : emailType.CreatedDate,
                LastModifiedDate = emailType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(emailType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : emailType.LastModifiedDate,
            };
        }

        /// <summary>
        /// Emails the type name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool EmailTypeNameExists(string name, int id)
        {
            return db.EmailTypes.Count(e => e.Name == name && e.Id != id) > 0;
        }
    }
}