// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 03-08-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-08-2018
// ***********************************************************************
// <copyright file="RfpJobTypesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class RFP Job Types Controller.</summary>
// ***********************************************************************

/// <summary>
/// The RFP Job Types namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.RfpJobTypes
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using DataTable;
    using Filters;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using System.Collections.Generic;
    /// <summary>
    /// Class RFP Job Types Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />

    public class RfpJobTypesController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the RFP job types.
        /// </summary>
        /// <remarks>Job Type list filter with level 1.</remarks>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the RFP job types List.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetRfpJobTypes([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewFeeScheduleMaster)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddFeeScheduleMaster)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteFeeScheduleMaster))
            {
                var rfpJobTypes = this.rpoContext.RfpJobTypes.Where(x => x.Level == 1).Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = rfpJobTypes.Count();
                var recordsFiltered = recordsTotal;

                var result = rfpJobTypes
                    .AsEnumerable()
                    .Select(c => Format(c)).OrderByDescending(x => x.LastModifiedDate)
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
        /// Gets the RFP job type dropdown.
        /// </summary>
        /// <returns>Gets the RFP job type List for dropdown.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/rfpjobtypes/dropdown")]
        public IHttpActionResult GetRfpJobTypeDropdown()
        {
            var result = this.rpoContext.RfpJobTypes.Where(x => x.Level == 1 && x.IsActive == true).AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name,
                Level = c.Level,
                IsCurrentStatusOfFiling = c.IsCurrentStatusOfFiling
            }).ToArray();

            return Ok(result);
        }
        /// <summary>
        /// Gets the RFP job type against idJobtypes dropdown.
        /// </summary>
        /// <param name="idJobtypes"></param>
        /// <returns>Gets the RFP job type list against idJobtypes dropdown.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/rfpjobtypes/{idJobtypes}/dropdown")]
        public IHttpActionResult GetEditJobTypeDropdown(int idJobtypes)
        {
            if (idJobtypes == 0)
            {
                throw new RpoBusinessException("Value is not Null or zero!");
            }
            var result1 = this.rpoContext.RfpJobTypes.Where(x => x.Level == 1 && x.IsActive == true).AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name,
                Level = c.Level,
                IsCurrentStatusOfFiling = c.IsCurrentStatusOfFiling
            }).ToArray();

            var result2 = this.rpoContext.RfpJobTypes.Where(x => x.Id== idJobtypes).AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name,
                Level = c.Level,
                IsCurrentStatusOfFiling = c.IsCurrentStatusOfFiling
            }).ToArray();

            var result = result1.Union(result2);
            return Ok(result);
        }

        /// <summary>
        /// Gets the RFP job types by parent identifier.
        /// </summary>
        /// <param name="idParent">The identifier parent.</param>
        /// <returns>Gets the RFP job types List against Idparent.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/rfpjobtypes/{idParent}/jobtypes")]
        public IHttpActionResult GetRfpJobTypesByParentId(int idParent)
        {
            var result = this.rpoContext.RfpJobTypes.Where(x => x.IdParent == idParent && x.IsActive == true).AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name,
                IdParent = c.IdParent,
                Level = c.Level,
                ServiceDescription = c.ServiceDescription,
                AppendWorkDescription = c.AppendWorkDescription,
                CustomServiceDescription = c.CustomServiceDescription,
                AdditionalUnitPrice = c.AdditionalUnitPrice,
                IsCurrentStatusOfFiling = c.IsCurrentStatusOfFiling,
                Cost = c.Cost,
                CostType = c.CostType,
                RfpJobTypeCostRanges = c.RfpJobTypeCostRanges,
                RfpJobTypeCumulativeCosts = c.RfpJobTypeCumulativeCosts,
                DisplayOrder = c.DisplayOrder,
                PartOf = c.PartOf,
                IsShowScope = c.IsShowScope,
            }).OrderBy(x => x.Name).ToArray();

            return Ok(result);
        }
        /// <summary>
        /// Get the rfpjobtype against the idparents and idjobtypes
        /// </summary>
        /// <param name="idParent"></param>
        /// <param name="idJobtypes"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/rfpjobtypes/{idParent}/{idJobtypes}/jobtypes")]
        public IHttpActionResult GetRfpEditJobTypesByParentId(int idParent, int idJobtypes)
        {
            if (idJobtypes == 0)
            {
                throw new RpoBusinessException("Value is not Null or zero!");
            }

            var result1 = this.rpoContext.RfpJobTypes.Where(x => x.IdParent == idParent && x.IsActive == true).AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name,
                IdParent = c.IdParent,
                Level = c.Level,
                ServiceDescription = c.ServiceDescription,
                AppendWorkDescription = c.AppendWorkDescription,
                CustomServiceDescription = c.CustomServiceDescription,
                AdditionalUnitPrice = c.AdditionalUnitPrice,
                IsCurrentStatusOfFiling = c.IsCurrentStatusOfFiling,
                Cost = c.Cost,
                CostType = c.CostType,
                RfpJobTypeCostRanges = c.RfpJobTypeCostRanges,
                RfpJobTypeCumulativeCosts = c.RfpJobTypeCumulativeCosts,
                DisplayOrder = c.DisplayOrder,
                PartOf = c.PartOf,
                IsShowScope = c.IsShowScope,
            }).OrderBy(x => x.DisplayOrder).ToArray();

            var result2 = this.rpoContext.RfpJobTypes.Where(x => x.IdParent == idParent && x.Id == idJobtypes).AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name,
                IdParent = c.IdParent,
                Level = c.Level,
                ServiceDescription = c.ServiceDescription,
                AppendWorkDescription = c.AppendWorkDescription,
                CustomServiceDescription = c.CustomServiceDescription,
                AdditionalUnitPrice = c.AdditionalUnitPrice,
                IsCurrentStatusOfFiling = c.IsCurrentStatusOfFiling,
                Cost = c.Cost,
                CostType = c.CostType,
                RfpJobTypeCostRanges = c.RfpJobTypeCostRanges,
                RfpJobTypeCumulativeCosts = c.RfpJobTypeCumulativeCosts,
                DisplayOrder = c.DisplayOrder,
                PartOf = c.PartOf,
                IsShowScope = c.IsShowScope,
            }).OrderBy(x => x.DisplayOrder).ToArray();

            var result = result1.Union(result2);
            return Ok(result);
        }

        /// <summary>
        /// Gets the type of the RFP job.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the job type List..</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(RfpJobTypeDetail))]
        public IHttpActionResult GetRfpJobType(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewFeeScheduleMaster)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddFeeScheduleMaster)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteFeeScheduleMaster))
            {
                RfpJobType rfpJobType = this.rpoContext.RfpJobTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (rfpJobType == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(rfpJobType));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the type of the RFP job.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="rfpJobTypeInsertUpdate">Type of the RFP job.</param>
        /// <returns>Update the jobtypes.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRfpJobType(int id, RfpJobTypeInsertUpdate rfpJobTypeInsertUpdate)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddFeeScheduleMaster))
            {
                RfpJobType rfpJobType = rpoContext.RfpJobTypes.Find(id);

                if (rfpJobType == null)
                {
                    return this.NotFound();
                }

                if (!ModelState.IsValid)
                {
                    return this.BadRequest(ModelState);
                }

                if (id != rfpJobTypeInsertUpdate.Id)
                {
                    return this.BadRequest();
                }

                if (this.RfpJobTypeNameExists(rfpJobTypeInsertUpdate.Name, rfpJobTypeInsertUpdate.IdParent, rfpJobTypeInsertUpdate.Id))
                {
                    if (rfpJobTypeInsertUpdate.Level == 1)
                    {
                        throw new RpoBusinessException(StaticMessages.JobTypeNameExistsMessage);
                    }
                    else if (rfpJobTypeInsertUpdate.Level == 2)
                    {
                        throw new RpoBusinessException(StaticMessages.JobTypeDescriptionExistsMessage);
                    }
                    else if (rfpJobTypeInsertUpdate.Level == 3)
                    {
                        throw new RpoBusinessException(StaticMessages.JobSubTypeExistsMessage);
                    }
                    else if (rfpJobTypeInsertUpdate.Level == 4)
                    {
                        throw new RpoBusinessException(StaticMessages.ServiceGroupExistsMessage);
                    }
                    else
                    {
                        throw new RpoBusinessException(StaticMessages.ServiceItemExistsMessage);
                    }
                }

                if (id == rfpJobTypeInsertUpdate.PartOf)
                {
                    throw new RpoBusinessException(StaticMessages.ServiceItemExistsPartofMessage);
                }

                List<RfpJobType> rfpJobTypepartof = rpoContext.RfpJobTypes.Where(d => d.PartOf == id).ToList();

                if (rfpJobTypeInsertUpdate.PartOf != null && rfpJobTypepartof != null && rfpJobTypepartof.Count > 0)
                {
                    throw new RpoBusinessException(StaticMessages.ServiceItemParentExistsMessage);
                }

                rfpJobType.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    rfpJobType.LastModifiedBy = employee.Id;
                }

                if (rfpJobTypeInsertUpdate.Level != 5)
                {
                    rfpJobType.RfpJobTypeCostRanges = null;
                    rfpJobType.RfpJobTypeCumulativeCosts = null;
                }

                rfpJobType.IdParent = rfpJobTypeInsertUpdate.IdParent;
                rfpJobType.Name = rfpJobTypeInsertUpdate.Name;
                rfpJobType.Level = rfpJobTypeInsertUpdate.Level;
                rfpJobType.ServiceDescription = rfpJobTypeInsertUpdate.ServiceDescription;
                rfpJobType.AppendWorkDescription = rfpJobTypeInsertUpdate.AppendWorkDescription;
                rfpJobType.CustomServiceDescription = rfpJobTypeInsertUpdate.CustomServiceDescription;
                rfpJobType.AdditionalUnitPrice = rfpJobTypeInsertUpdate.AdditionalUnitPrice;
                rfpJobType.IsCurrentStatusOfFiling = rfpJobTypeInsertUpdate.IsCurrentStatusOfFiling;
                rfpJobType.Cost = rfpJobTypeInsertUpdate.Cost;
                rfpJobType.CostType = rfpJobTypeInsertUpdate.CostType;
                rfpJobType.DisplayOrder = rfpJobTypeInsertUpdate.DisplayOrder;
                rfpJobType.IsShowScope = rfpJobTypeInsertUpdate.IsShowScope;
                rfpJobType.PartOf = rfpJobTypeInsertUpdate.PartOf;
                if (rfpJobType.RfpJobTypeCostRanges != null && rfpJobType.RfpJobTypeCostRanges.Count > 0)
                {
                    rpoContext.RfpJobTypeCostRanges.RemoveRange(rfpJobType.RfpJobTypeCostRanges);
                }

                if (rfpJobTypeInsertUpdate.RfpJobTypeCostRanges != null && rfpJobTypeInsertUpdate.RfpJobTypeCostRanges.Count > 0)
                {
                    foreach (var item in rfpJobTypeInsertUpdate.RfpJobTypeCostRanges)
                    {
                        RfpJobTypeCostRange rfpJobTypeCostRange = new RfpJobTypeCostRange();
                        rfpJobTypeCostRange.Cost = item.RangeCost;
                        rfpJobTypeCostRange.IdRfpJobType = rfpJobType.Id;
                        rfpJobTypeCostRange.MaximumQuantity = item.MaximumQuantity;
                        rfpJobTypeCostRange.MinimumQuantity = item.MinimumQuantity;
                        rpoContext.RfpJobTypeCostRanges.Add(rfpJobTypeCostRange);
                    }
                }

                if (rfpJobType.RfpJobTypeCumulativeCosts != null && rfpJobType.RfpJobTypeCumulativeCosts.Count > 0)
                {
                    rpoContext.RfpJobTypeCumulativeCosts.RemoveRange(rfpJobType.RfpJobTypeCumulativeCosts);
                }

                if (rfpJobTypeInsertUpdate.RfpJobTypeCumulativeCosts != null && rfpJobTypeInsertUpdate.RfpJobTypeCumulativeCosts.Count > 0)
                {
                    foreach (var item in rfpJobTypeInsertUpdate.RfpJobTypeCumulativeCosts)
                    {
                        RfpJobTypeCumulativeCost rfpJobTypeCumulativeCost = new RfpJobTypeCumulativeCost();
                        rfpJobTypeCumulativeCost.Cost = item.CumulativeCost;
                        rfpJobTypeCumulativeCost.IdRfpJobType = rfpJobType.Id;
                        rfpJobTypeCumulativeCost.Quantity = item.Qty;
                        rpoContext.RfpJobTypeCumulativeCosts.Add(rfpJobTypeCumulativeCost);
                    }
                }

                try
                {
                    this.rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.RfpJobTypeExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                RfpJobType rfpJobTypeResponse = this.rpoContext.RfpJobTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return Ok(FormatDetails(rfpJobTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        ///put the RFPService item isactive
        /// </summary>
        /// <param name="rfpJobTypeInsertUpdate"></param>
        /// <returns>update the service item isactive flag.</returns>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        [Route("api/RfpServiceItem/IsActive")]
        public IHttpActionResult PutRfpJobTypeIsActive(RfpJobTypeInsertUpdate rfpJobTypeInsertUpdate)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddFeeScheduleMaster))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                RfpJobType rfpJobTypes = rpoContext.RfpJobTypes.FirstOrDefault(x => x.Id == rfpJobTypeInsertUpdate.Id);

                if (rfpJobTypes == null)
                {
                    return this.NotFound();
                }
                if (rfpJobTypeInsertUpdate.IsActive == false)
                {
                    List<RfpJobType> rfpJobTypespartof = rpoContext.RfpJobTypes.Where(x => x.PartOf == rfpJobTypeInsertUpdate.Id && x.IsActive == true).ToList();
                    if (rfpJobTypespartof != null && rfpJobTypespartof.Count() > 0)
                    {
                        throw new RpoBusinessException(StaticMessages.ServiceItemInactiveMessage);
                    }
                }
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                rfpJobTypes.IsActive = rfpJobTypeInsertUpdate.IsActive;
                rfpJobTypes.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    rfpJobTypes.LastModifiedBy = employee.Id;
                }

                this.rpoContext.SaveChanges();

                RfpJobType rfpJobTypeResponse = this.rpoContext.RfpJobTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == rfpJobTypeInsertUpdate.Id);
                return Ok(FormatDetails(rfpJobTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        /// Posts the type of the RFP job.
        /// </summary>
        /// <param name="rfpJobTypeInsertUpdate">Type of the RFP job.</param>
        /// <returns>Create a new JobType for RFP.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(RfpJobType))]
        public IHttpActionResult PostRfpJobType(RfpJobTypeInsertUpdate rfpJobTypeInsertUpdate)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddFeeScheduleMaster))
            {
                RfpJobType rfpJobType = new RfpJobType();

                if (!ModelState.IsValid)
                {
                    return this.BadRequest(ModelState);
                }

                if (this.RfpJobTypeNameExists(rfpJobTypeInsertUpdate.Name, rfpJobTypeInsertUpdate.IdParent, rfpJobTypeInsertUpdate.Id))
                {
                    if (rfpJobTypeInsertUpdate.Level == 1)
                    {
                        throw new RpoBusinessException(StaticMessages.JobTypeNameExistsMessage);
                    }
                    else if (rfpJobTypeInsertUpdate.Level == 2)
                    {
                        throw new RpoBusinessException(StaticMessages.JobTypeDescriptionExistsMessage);
                    }
                    else if (rfpJobTypeInsertUpdate.Level == 3)
                    {
                        throw new RpoBusinessException(StaticMessages.JobSubTypeExistsMessage);
                    }
                    else if (rfpJobTypeInsertUpdate.Level == 4)
                    {
                        throw new RpoBusinessException(StaticMessages.ServiceGroupExistsMessage);
                    }
                    else
                    {
                        throw new RpoBusinessException(StaticMessages.ServiceItemExistsMessage);
                    }
                }

                if (this.RfpPartofJobTypeNameExists(rfpJobTypeInsertUpdate.Name, rfpJobTypeInsertUpdate.PartOf, rfpJobTypeInsertUpdate.Id))
                {
                    throw new RpoBusinessException(StaticMessages.JobTypePartofExistsMessage);
                }

                rfpJobType.LastModifiedDate = DateTime.UtcNow;
                rfpJobType.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    rfpJobType.CreatedBy = employee.Id;
                }

                rfpJobType.IdParent = rfpJobTypeInsertUpdate.IdParent;
                rfpJobType.Name = rfpJobTypeInsertUpdate.Name;
                rfpJobType.Level = rfpJobTypeInsertUpdate.Level;
                rfpJobType.ServiceDescription = rfpJobTypeInsertUpdate.ServiceDescription;
                rfpJobType.AppendWorkDescription = rfpJobTypeInsertUpdate.AppendWorkDescription;
                rfpJobType.CustomServiceDescription = rfpJobTypeInsertUpdate.CustomServiceDescription;
                rfpJobType.AdditionalUnitPrice = rfpJobTypeInsertUpdate.AdditionalUnitPrice;
                rfpJobType.IsCurrentStatusOfFiling = rfpJobTypeInsertUpdate.IsCurrentStatusOfFiling;
                rfpJobType.Cost = rfpJobTypeInsertUpdate.Cost;
                rfpJobType.CostType = rfpJobTypeInsertUpdate.CostType;
                rfpJobType.DisplayOrder = rfpJobTypeInsertUpdate.DisplayOrder;
                rfpJobType.IsShowScope = rfpJobTypeInsertUpdate.IsShowScope;
                rfpJobType.PartOf = rfpJobTypeInsertUpdate.PartOf;
                rfpJobType.IsActive = rfpJobTypeInsertUpdate.IsActive;
                if (rfpJobTypeInsertUpdate.Level != 5)
                {
                    rfpJobType.RfpJobTypeCostRanges = null;
                    rfpJobType.RfpJobTypeCumulativeCosts = null;
                }

                this.rpoContext.RfpJobTypes.Add(rfpJobType);

                if (rfpJobType.RfpJobTypeCostRanges != null && rfpJobType.RfpJobTypeCostRanges.Count > 0)
                {
                    rpoContext.RfpJobTypeCostRanges.RemoveRange(rfpJobType.RfpJobTypeCostRanges);
                }

                if (rfpJobTypeInsertUpdate.RfpJobTypeCostRanges != null && rfpJobTypeInsertUpdate.RfpJobTypeCostRanges.Count > 0)
                {
                    foreach (var item in rfpJobTypeInsertUpdate.RfpJobTypeCostRanges)
                    {
                        RfpJobTypeCostRange rfpJobTypeCostRange = new RfpJobTypeCostRange();
                        rfpJobTypeCostRange.Cost = item.RangeCost;
                        rfpJobTypeCostRange.IdRfpJobType = rfpJobType.Id;
                        rfpJobTypeCostRange.MaximumQuantity = item.MaximumQuantity;
                        rfpJobTypeCostRange.MinimumQuantity = item.MinimumQuantity;
                        rpoContext.RfpJobTypeCostRanges.Add(rfpJobTypeCostRange);
                    }
                }

                this.rpoContext.SaveChanges();

                if (rfpJobType.RfpJobTypeCumulativeCosts != null && rfpJobType.RfpJobTypeCumulativeCosts.Count > 0)
                {
                    rpoContext.RfpJobTypeCumulativeCosts.RemoveRange(rfpJobType.RfpJobTypeCumulativeCosts);
                }

                if (rfpJobTypeInsertUpdate.RfpJobTypeCumulativeCosts != null && rfpJobTypeInsertUpdate.RfpJobTypeCumulativeCosts.Count > 0)
                {
                    foreach (var item in rfpJobTypeInsertUpdate.RfpJobTypeCumulativeCosts)
                    {
                        RfpJobTypeCumulativeCost rfpJobTypeCumulativeCost = new RfpJobTypeCumulativeCost();
                        rfpJobTypeCumulativeCost.Cost = item.CumulativeCost;
                        rfpJobTypeCumulativeCost.IdRfpJobType = rfpJobType.Id;
                        rfpJobTypeCumulativeCost.Quantity = item.Qty;
                        rpoContext.RfpJobTypeCumulativeCosts.Add(rfpJobTypeCumulativeCost);
                    }
                }

                this.rpoContext.SaveChanges();

                RfpJobType rfpJobTypeResponse = this.rpoContext.RfpJobTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == rfpJobType.Id);
                return Ok(FormatDetails(rfpJobTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the type of the RFP job.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Delete jobtype in the database..</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(RfpJobType))]
        public IHttpActionResult DeleteRfpJobType(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteFeeScheduleMaster))
            {
                RfpJobType rfpJobType = this.rpoContext.RfpJobTypes.Find(id);
                if (rfpJobType == null)
                {
                    return this.NotFound();
                }

                //try
                //{
                    this.rpoContext.RfpJobTypes.Remove(rfpJobType);
                    this.rpoContext.SaveChanges();
                //}
                //catch (Exception ex)
                //{

                //    throw new RpoBusinessException(StaticMessages.DeleteReferenceExistExceptionMessage);
                //}

                return Ok(rfpJobType);
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
        /// RFPs the job type exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool RfpJobTypeExists(int id)
        {
            return rpoContext.RfpJobTypes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified RFP job type.
        /// </summary>
        /// <param name="rfpJobType">Type of the RFP job.</param>
        /// <returns>Rfp JobType DTO.</returns>
        private RfpJobTypeDTO Format(RfpJobType rfpJobType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new RfpJobTypeDTO
            {
                Id = rfpJobType.Id,
                Name = rfpJobType.Name,
                Level = rfpJobType.Level,
                CreatedBy = rfpJobType.CreatedBy,
                LastModifiedBy = rfpJobType.LastModifiedBy != null ? rfpJobType.LastModifiedBy : rfpJobType.CreatedBy,
                CreatedByEmployeeName = rfpJobType.CreatedByEmployee != null ? rfpJobType.CreatedByEmployee.FirstName + " " + rfpJobType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = rfpJobType.LastModifiedByEmployee != null ? rfpJobType.LastModifiedByEmployee.FirstName + " " + rfpJobType.LastModifiedByEmployee.LastName : (rfpJobType.CreatedByEmployee != null ? rfpJobType.CreatedByEmployee.FirstName + " " + rfpJobType.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = rfpJobType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpJobType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpJobType.CreatedDate,
                LastModifiedDate = rfpJobType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpJobType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (rfpJobType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpJobType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpJobType.CreatedDate),
                IsShowScope = rfpJobType.IsShowScope,
                PartOf = rfpJobType.PartOf != null ? rfpJobType.PartOf : null,
                IsActive = rfpJobType.IsActive,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="rfpJobType">Type of the RFP job.</param>
        /// <returns>RfpJobTypeDetail.</returns>
        private RfpJobTypeDetail FormatDetails(RfpJobType rfpJobType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new RfpJobTypeDetail
            {
                Id = rfpJobType.Id,
                Name = rfpJobType.Name,
                Level = rfpJobType.Level,
                CreatedBy = rfpJobType.CreatedBy,
                LastModifiedBy = rfpJobType.LastModifiedBy != null ? rfpJobType.LastModifiedBy : rfpJobType.CreatedBy,
                CreatedByEmployeeName = rfpJobType.CreatedByEmployee != null ? rfpJobType.CreatedByEmployee.FirstName + " " + rfpJobType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = rfpJobType.LastModifiedByEmployee != null ? rfpJobType.LastModifiedByEmployee.FirstName + " " + rfpJobType.LastModifiedByEmployee.LastName : (rfpJobType.CreatedByEmployee != null ? rfpJobType.CreatedByEmployee.FirstName + " " + rfpJobType.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = rfpJobType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpJobType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpJobType.CreatedDate,
                LastModifiedDate = rfpJobType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpJobType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (rfpJobType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpJobType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpJobType.CreatedDate),
                IsShowScope = rfpJobType.IsShowScope,
                PartOf = rfpJobType.PartOf != null ? rfpJobType.PartOf : null,
                IsActive = rfpJobType.IsActive,
            };
        }

        /// <summary>
        /// RFPs the job type name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool RfpJobTypeNameExists(string name, int id)
        {
            return rpoContext.RfpJobTypes.Count(e => e.Name == name && e.Id != id) > 0;
        }

        private bool RfpJobTypeNameExists(string name, int? idParent, int id)
        {
            return this.rpoContext.RfpJobTypes.Count(e => e.Name == name && e.IdParent == idParent && e.Id != id) > 0;
        }

        private bool RfpPartofJobTypeNameExists(string name, int? idPartof, int id)
        {
            return this.rpoContext.RfpJobTypes.Count(e => e.Id == idPartof && e.PartOf != null) > 0;
        }
    }
}