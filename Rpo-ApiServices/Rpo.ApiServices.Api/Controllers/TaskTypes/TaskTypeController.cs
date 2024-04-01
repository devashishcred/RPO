// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-24-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="TaskTypeController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Task Types Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.TaskTypes
{
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Rpo.ApiServices.Api.Tools;
    using System;
    using Rpo.ApiServices.Api.DataTable;
    using Filters;

    /// <summary>
    /// Class Task Types Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class TaskTypesController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the task types.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the task types list.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetTaskTypes([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var taskTypes = rpoContext.TaskTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = taskTypes.Count();
                var recordsFiltered = recordsTotal;

                var result = taskTypes
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
        /// Gets the task type drop down.
        /// </summary>
        /// <returns>Gets the task types list for dropdown.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/tasktypes/dropdown")]
        public IHttpActionResult GetTaskTypeDropdown()
        {
            var result = rpoContext.TaskTypes.AsEnumerable().Where(d => d.IsActive == true).Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name,
                IdDefaultContact = c.IdDefaultContact,
                IsDisplayContact = c.IsDisplayContact,
                IsDisplayDuration = c.IsDisplayDuration,
                IsDisplayTime = c.IsDisplayTime,
            }).ToArray();

            return Ok(result);
        }
        /// <summary>
        /// Gets the task type drop down.
        /// </summary>
        /// <param name="idTaskType"></param>
        /// <returns>Gets the task types list for dropdown on edit window</returns>

        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/tasktypes/{idTaskType}/dropdown")]
        public IHttpActionResult GetEditTaskTypeDropdown(int idTaskType)
        {
            if (idTaskType == 0)
            {
                throw new RpoBusinessException("TaskType is not Null or zero!");
            }
            var result1 = rpoContext.TaskTypes.AsEnumerable().Where(d => d.IsActive == true).Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name,
                IdDefaultContact = c.IdDefaultContact,
                IsDisplayContact = c.IsDisplayContact,
                IsDisplayDuration = c.IsDisplayDuration,
                IsDisplayTime = c.IsDisplayTime,
            }).ToArray();

            var result2 = rpoContext.TaskTypes.AsEnumerable().Where(d => d.Id == idTaskType).Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name,
                IdDefaultContact = c.IdDefaultContact,
                IsDisplayContact = c.IsDisplayContact,
                IsDisplayDuration = c.IsDisplayDuration,
                IsDisplayTime = c.IsDisplayTime,
            }).ToArray();

            var result = result1.Union(result2);
            return Ok(result);
        }

        /// <summary>
        /// Gets the type of the task.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the task types list.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TaskTypeDetail))]
        public IHttpActionResult GetTaskType(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                TaskType taskType = rpoContext.TaskTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (taskType == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(taskType));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the type of the task.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="taskType">Type of the task.</param>
        /// <returns>update the task type.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TaskTypeDetail))]
        public IHttpActionResult PutTaskType(int id, TaskType taskType)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != taskType.Id)
                {
                    return BadRequest();
                }

                if (TaskTypeNameExists(taskType.Name, taskType.Id))
                {
                    throw new RpoBusinessException(StaticMessages.TaskTypeNameExistsMessage);
                }

                taskType.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    taskType.LastModifiedBy = employee.Id;
                }

                rpoContext.Entry(taskType).State = EntityState.Modified;

                try
                {
                    rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskTypeExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TaskType taskTypeResponse = rpoContext.TaskTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return Ok(FormatDetails(taskTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        /// Update the tasktype status
        /// </summary>
        /// <param name="taskType"></param>
        /// <returns>update the task type  isactive</returns>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TaskTypeDetail))]
        [Route("api/TaskTypes/IsActive")]
        public IHttpActionResult PutTypesIsActive(TaskType taskType)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                TaskType task = rpoContext.TaskTypes.FirstOrDefault(x => x.Id == taskType.Id);

                if (task == null)
                {
                    return this.NotFound();
                }

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                task.IsActive = taskType.IsActive;
                task.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    task.LastModifiedBy = employee.Id;
                }

                this.rpoContext.SaveChanges();

                TaskType taskTypeResponse = rpoContext.TaskTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == taskType.Id);
                return Ok(FormatDetails(taskTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the type of the task.
        /// </summary>
        /// <param name="taskType">Type of the task.</param>
        /// <returns>create a new task type .</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TaskTypeDetail))]
        public IHttpActionResult PostTaskType(TaskType taskType)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (TaskTypeNameExists(taskType.Name, taskType.Id))
                {
                    throw new RpoBusinessException(StaticMessages.TaskTypeNameExistsMessage);
                }

                taskType.LastModifiedDate = DateTime.UtcNow;
                taskType.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    taskType.CreatedBy = employee.Id;
                }

                rpoContext.TaskTypes.Add(taskType);
                rpoContext.SaveChanges();

                TaskType taskTypeResponse = rpoContext.TaskTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == taskType.Id);
                return Ok(FormatDetails(taskTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the type of the task.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>delete task type.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TaskType))]
        public IHttpActionResult DeleteTaskType(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                TaskType taskType = rpoContext.TaskTypes.Find(id);
                if (taskType == null)
                {
                    return this.NotFound();
                }

                rpoContext.TaskTypes.Remove(taskType);
                rpoContext.SaveChanges();

                return Ok(taskType);
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
        /// Tasks the type exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool TaskTypeExists(int id)
        {
            return rpoContext.TaskTypes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified task type.
        /// </summary>
        /// <param name="taskType">Type of the task.</param>
        /// <returns>TaskTypeDTO.</returns>
        private TaskTypeDTO Format(TaskType taskType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new TaskTypeDTO
            {
                Id = taskType.Id,
                Name = taskType.Name,
                IsDisplayTime = taskType.IsDisplayTime,
                IsDisplayContact = taskType.IsDisplayContact,
                IsDisplayDuration = taskType.IsDisplayDuration,
                IdDefaultContact = taskType.IdDefaultContact,
                DefaultContact = taskType.DefaultContact != null ? taskType.DefaultContact.FirstName + " " + taskType.DefaultContact.LastName : string.Empty,
                CreatedBy = taskType.CreatedBy,
                LastModifiedBy = taskType.LastModifiedBy,
                CreatedByEmployeeName = taskType.CreatedByEmployee != null ? taskType.CreatedByEmployee.FirstName + " " + taskType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = taskType.LastModifiedByEmployee != null ? taskType.LastModifiedByEmployee.FirstName + " " + taskType.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = taskType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(taskType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : taskType.CreatedDate,
                LastModifiedDate = taskType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(taskType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : taskType.LastModifiedDate,
                IsActive = taskType.IsActive,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="taskType">Type of the task.</param>
        /// <returns>TaskTypeDetail.</returns>
        private TaskTypeDetail FormatDetails(TaskType taskType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new TaskTypeDetail
            {
                Id = taskType.Id,
                Name = taskType.Name,
                IsDisplayTime = taskType.IsDisplayTime,
                IsDisplayContact = taskType.IsDisplayContact,
                IsDisplayDuration = taskType.IsDisplayDuration,
                IdDefaultContact = taskType.IdDefaultContact,
                DefaultContact = taskType.DefaultContact != null ? taskType.DefaultContact.FirstName + " " + taskType.DefaultContact.LastName : string.Empty,
                CreatedBy = taskType.CreatedBy,
                LastModifiedBy = taskType.LastModifiedBy,
                CreatedByEmployeeName = taskType.CreatedByEmployee != null ? taskType.CreatedByEmployee.FirstName + " " + taskType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = taskType.LastModifiedByEmployee != null ? taskType.LastModifiedByEmployee.FirstName + " " + taskType.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = taskType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(taskType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : taskType.CreatedDate,
                LastModifiedDate = taskType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(taskType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : taskType.LastModifiedDate,
                IsActive = taskType.IsActive,
            };
        }

        /// <summary>
        /// Tasks the type name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool TaskTypeNameExists(string name, int id)
        {
            return rpoContext.TaskTypes.Count(e => e.Name == name && e.Id != id) > 0;
        }
    }
}