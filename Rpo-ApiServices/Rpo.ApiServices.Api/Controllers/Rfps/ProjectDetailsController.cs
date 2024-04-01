// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-31-2018
// ***********************************************************************
// <copyright file="ProjectDetailsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Project Details Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Rfps namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Rfps
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Model.Models.Enums;

    /// <summary>
    /// Class Project Details Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class ProjectDetailsController : ApiController
    {
        /// <summary>
        /// The RPO context.
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the project details.
        /// </summary>
        /// <param name="idRfp">The identifier RFP.</param>
        /// <returns>Get list of all service items.</returns>
        [Authorize]
        [RpoAuthorize]
        [Route("api/Rfps/{idRfp}/ProjectDetails")]
        [ResponseType(typeof(IEnumerable<ProjectDetailDTO>))]
        public IHttpActionResult GetProjectDetails(int idRfp)
        {
            var rfp = this.rpoContext.Rfps.Include("ProjectDetails.RfpFeeSchedules").FirstOrDefault(x => x.Id == idRfp);

            if (rfp == null)
            {
                return this.NotFound();
            }

            this.rpoContext.Configuration.LazyLoadingEnabled = false;
            rfp.SetResponseGoNextStepHeader();

            var result = this.rpoContext.ProjectDetails.Where(x => x.IdRfp == idRfp).AsEnumerable().Select(x => this.Format(x)).OrderBy(x => x.DisplayOrder).ToList();

            return this.Ok(result);
        }

        /// <summary>
        /// Gets the project detail.
        /// </summary>
        /// <param name="idRfp">The identifier RFP.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>Get list of all service items against RFP.</returns>
        [Authorize]
        [RpoAuthorize]
        [Route("api/Rfps/{idRfp}/ProjectDetails/{id}")]
        [ResponseType(typeof(ProjectDetail))]
        public IHttpActionResult GetProjectDetail(int idRfp, int id)
        {
            var rfp = this.rpoContext
                .Rfps
                .FirstOrDefault(r => r.Id == idRfp);

            if (rfp == null)
            {
                return this.NotFound();
            }

            ProjectDetail projectDetail = this.rpoContext.ProjectDetails.FirstOrDefault(pd => pd.Id == id);
            if (projectDetail == null)
            {
                return this.NotFound();
            }

            this.rpoContext.Configuration.LazyLoadingEnabled = false;
            rfp.SetResponseGoNextStepHeader();
            return this.Ok(this.Format(projectDetail));
        }

        /// <summary>
        /// Posts the project detail.
        /// </summary>
        /// <param name="idRfp">The identifier RFP.</param>
        /// <param name="projectDetailList">The project detail list.</param>
        /// <returns>update the service items in database.</returns>
        /// <exception cref="System.Exception">Error during rollback.</exception>
        [Authorize]
        [RpoAuthorize]
        [Route("api/Rfps/{idRfp}/ProjectDetails")]
        [ResponseType(typeof(ProjectDetail))]
        public IHttpActionResult PostProjectDetail(int idRfp, IEnumerable<ProjectDetailDTO> projectDetailList)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddRFP))
            {
                using (var transaction = this.rpoContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (!ModelState.IsValid)
                        {
                            return this.BadRequest(this.ModelState);
                        }
                        var rfp = this.rpoContext.Rfps.FirstOrDefault(r => r.Id == idRfp);

                        if (rfp == null)
                            return this.NotFound();

                        var unselectedRfpFeeSchedules = projectDetailList.Where(x => x.RfpFeeSchedules.Count() <= 0).Select(x => x.IdRfp).ToList();
                        if (unselectedRfpFeeSchedules != null && unselectedRfpFeeSchedules.Count() > 0)
                        {
                            throw new RpoBusinessException(StaticMessages.FeeScheduleNotExistsMessage);
                        }

                        rfp.LastModifiedDate = DateTime.UtcNow;
                        rfp.LastUpdatedStep = 2;
                        if (rfp.CompletedStep < 2)
                        {
                            rfp.CompletedStep = 2;
                        }
                        rfp.ProcessGoNextStepHeader();

                        var projectlist = this.rpoContext.ProjectDetails.Include("RfpFeeSchedules").Where(x => x.IdRfp == rfp.Id).ToList();

                        foreach (ProjectDetail item in projectlist)
                        {
                            var projectDetail = projectDetailList.FirstOrDefault(x => x.Id == item.Id);
                            if (projectDetail == null)
                            {
                                if (item.RfpFeeSchedules != null && item.RfpFeeSchedules.Count() > 0)
                                {
                                    foreach (var rfpFeeSchedule in item.RfpFeeSchedules)
                                    {
                                        var milestoneServices = this.rpoContext.MilestoneServices.Where(x => x.IdRfpFeeSchedule == rfpFeeSchedule.Id).ToList();
                                        if (milestoneServices != null && milestoneServices.Count() > 0)
                                        {
                                            this.rpoContext.MilestoneServices.RemoveRange(milestoneServices);
                                        }
                                    }

                                    this.rpoContext.RfpFeeSchedules.RemoveRange(item.RfpFeeSchedules);
                                }
                                this.rpoContext.ProjectDetails.Remove(item);
                            }
                        }
                        this.rpoContext.SaveChanges();

                        foreach (ProjectDetailDTO item in projectDetailList)
                        {
                            if (item.Id > 0)
                            {
                                var projectDetail = this.rpoContext.ProjectDetails.Include("RfpFeeSchedules").FirstOrDefault(x => x.Id == item.Id);
                                projectDetail.WorkDescription = item.WorkDescription;
                                projectDetail.ArePlansNotPrepared = item.ArePlansNotPrepared;
                                projectDetail.ArePlansCompleted = item.ArePlansCompleted;
                                projectDetail.IsApproved = item.IsApproved;
                                projectDetail.IsDisaproved = item.IsDisaproved;
                                projectDetail.IsPermitted = item.IsPermitted;
                                projectDetail.ApprovedJobNumber = item.ApprovedJobNumber;
                                projectDetail.DisApprovedJobNumber = item.DisApprovedJobNumber;
                                projectDetail.PermittedJobNumber = item.PermittedJobNumber;
                                projectDetail.IdRfp = item.IdRfp;
                                projectDetail.IdRfpJobType = item.IdRfpJobType;
                                projectDetail.IdRfpSubJobTypeCategory = item.IdRfpSubJobTypeCategory;
                                projectDetail.IdRfpSubJobType = item.IdRfpSubJobType;
                                projectDetail.DisplayOrder = item.DisplayOrder;

                                var RfpFeeSchedulesList = projectDetail.RfpFeeSchedules.Select(x => x).ToList();
                                if (RfpFeeSchedulesList != null)
                                {
                                    foreach (RfpFeeSchedule rfpFeeSchedule in RfpFeeSchedulesList)
                                    {
                                        var feeSchedule = item.RfpFeeSchedules.FirstOrDefault(x => x.Id == rfpFeeSchedule.Id);
                                        if (feeSchedule == null)
                                        {
                                            var milestoneServices = this.rpoContext.MilestoneServices.Where(x => x.IdRfpFeeSchedule == rfpFeeSchedule.Id).ToList();
                                            if (milestoneServices != null && milestoneServices.Count() > 0)
                                            {
                                                this.rpoContext.MilestoneServices.RemoveRange(milestoneServices);
                                            }
                                            this.rpoContext.RfpFeeSchedules.Remove(rfpFeeSchedule);
                                        }
                                    }
                                    this.rpoContext.SaveChanges();
                                }

                                foreach (RfpFeeScheduleDTO rfpFeeSchedule in item.RfpFeeSchedules)
                                {
                                    if (rfpFeeSchedule.Id > 0)
                                    {
                                        var feeSchedule = item.RfpFeeSchedules.FirstOrDefault(x => x.Id == rfpFeeSchedule.Id);
                                        feeSchedule.IdProjectDetail = projectDetail.Id;
                                        feeSchedule.RfpWorkType = null;
                                        feeSchedule.RfpWorkTypeCategory = null;
                                        feeSchedule.Cost = rfpFeeSchedule.Cost;
                                        feeSchedule.Quantity = rfpFeeSchedule.Quantity;
                                        feeSchedule.Description = rfpFeeSchedule.Description;
                                        feeSchedule.TotalCost = (rfpFeeSchedule.Cost * rfpFeeSchedule.Quantity);
                                    }
                                    else
                                    {
                                        RfpFeeSchedule rfpFeeScheduleNew = new RfpFeeSchedule();
                                        rfpFeeScheduleNew.IdProjectDetail = projectDetail.Id;
                                        rfpFeeScheduleNew.IdRfp = projectDetail.IdRfp;
                                        rfpFeeScheduleNew.RfpWorkType = null;
                                        rfpFeeScheduleNew.RfpWorkTypeCategory = null;
                                        rfpFeeScheduleNew.IdRfpWorkType = rfpFeeSchedule.IdRfpWorkType;
                                        rfpFeeScheduleNew.IdRfpWorkTypeCategory = rfpFeeSchedule.IdRfpWorkTypeCategory;
                                        rfpFeeScheduleNew.Quantity = rfpFeeSchedule.Quantity;
                                        rfpFeeScheduleNew.Description = rfpFeeSchedule.Description;
                                        this.rpoContext.RfpFeeSchedules.Add(rfpFeeScheduleNew);
                                    }
                                }

                                this.rpoContext.SaveChanges();
                            }
                            else
                            {
                                ProjectDetail projectDetail = new ProjectDetail();
                                projectDetail.WorkDescription = item.WorkDescription;
                                projectDetail.ArePlansNotPrepared = item.ArePlansNotPrepared;
                                projectDetail.ArePlansCompleted = item.ArePlansCompleted;
                                projectDetail.IsApproved = item.IsApproved;
                                projectDetail.IsDisaproved = item.IsDisaproved;
                                projectDetail.IsPermitted = item.IsPermitted;
                                projectDetail.ApprovedJobNumber = item.ApprovedJobNumber;
                                projectDetail.DisApprovedJobNumber = item.DisApprovedJobNumber;
                                projectDetail.PermittedJobNumber = item.PermittedJobNumber;
                                projectDetail.IdRfp = item.IdRfp;
                                projectDetail.IdRfpJobType = item.IdRfpJobType;
                                projectDetail.IdRfpSubJobTypeCategory = item.IdRfpSubJobTypeCategory;
                                projectDetail.IdRfpSubJobType = item.IdRfpSubJobType;
                                projectDetail.DisplayOrder = item.DisplayOrder;

                                this.rpoContext.ProjectDetails.Add(projectDetail);
                                this.rpoContext.SaveChanges();

                                foreach (RfpFeeScheduleDTO rfpFeeSchedule in item.RfpFeeSchedules)
                                {
                                    RfpFeeSchedule rfpFeeScheduleNew = new RfpFeeSchedule();
                                    rfpFeeScheduleNew.IdProjectDetail = projectDetail.Id;
                                    rfpFeeScheduleNew.IdRfp = projectDetail.IdRfp;
                                    rfpFeeScheduleNew.RfpWorkType = null;
                                    rfpFeeScheduleNew.RfpWorkTypeCategory = null;
                                    rfpFeeScheduleNew.IdRfpWorkType = rfpFeeSchedule.IdRfpWorkType;
                                    rfpFeeScheduleNew.IdRfpWorkTypeCategory = rfpFeeSchedule.IdRfpWorkTypeCategory;
                                    rfpFeeScheduleNew.Quantity = rfpFeeSchedule.Quantity;
                                    rfpFeeScheduleNew.Description = rfpFeeSchedule.Description;
                                    this.rpoContext.RfpFeeSchedules.Add(rfpFeeScheduleNew);
                                }

                                this.rpoContext.SaveChanges();
                            }
                        }

                        updateScopeReview(rfp.Id);
                        transaction.Commit();
                        rfp.SetResponseGoNextStepHeader();
                        return this.Ok(projectDetailList);
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch
                        {
                            throw new Exception("Error during roolback", e);
                        }

                        throw e;
                    }
                }
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the project detail.
        /// </summary>
        /// <param name="idRfp">The identifier RFP.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttp Action Result.</returns>
        [Authorize]
        [RpoAuthorize]
        [Route("api/Rfps/{idRfp}/ProjectDetails/{id}")]
        public IHttpActionResult DeleteProjectDetail(int idRfp, int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddRFP))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var rfp = this.rpoContext.Rfps.Include("ProjectDetails.RfpFeeSchedules").FirstOrDefault(r => r.Id == idRfp);

                if (rfp == null)
                    return this.NotFound();

                rfp.ProcessGoNextStepHeader();
                if (!rfp.ProjectDetails.Any(pd => pd.Id == id))
                {
                    return BadRequest("Project detail not match with this RFP.");
                }

                ProjectDetail projectDetail = this.rpoContext.ProjectDetails.Find(id);
                if (projectDetail == null)
                {
                    return this.NotFound();
                }

                this.rpoContext.RfpFeeSchedules.RemoveRange(projectDetail.RfpFeeSchedules);
                this.rpoContext.ProjectDetails.Remove(projectDetail);
                this.rpoContext.SaveChanges();

                updateScopeReview(rfp.Id);
                rfp.SetResponseGoNextStepHeader();
                return Ok(projectDetail);
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
        /// Projects the detail exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ProjectDetailExists(int id)
        {
            return this.rpoContext.ProjectDetails.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified project detail.
        /// </summary>
        /// <param name="projectDetail">The project detail.</param>
        /// <returns>ProjectDetailDTO.</returns>
        private ProjectDetailDTO Format(ProjectDetail projectDetail)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new ProjectDetailDTO
            {
                Id = projectDetail.Id,
                IdRfp = projectDetail.IdRfp,
                IdRfpJobType = projectDetail.IdRfpJobType,
                IdRfpSubJobType = projectDetail.IdRfpSubJobType,
                IdRfpSubJobTypeCategory = projectDetail.IdRfpSubJobTypeCategory,
                ArePlansCompleted = projectDetail.ArePlansCompleted,
                ArePlansNotPrepared = projectDetail.ArePlansNotPrepared,
                DisplayOrder = projectDetail.DisplayOrder,
                IsApproved = projectDetail.IsApproved,
                IsDisaproved = projectDetail.IsDisaproved,
                IsPermitted = projectDetail.IsPermitted,
                ApprovedJobNumber = projectDetail.ApprovedJobNumber,
                DisApprovedJobNumber = projectDetail.DisApprovedJobNumber,
                PermittedJobNumber = projectDetail.PermittedJobNumber,
                RfpFeeSchedules = projectDetail.RfpFeeSchedules != null ? projectDetail.RfpFeeSchedules.Select(x => new RfpFeeScheduleDTO()
                {
                    Id = x.Id,
                    IdProjectDetail = x.IdProjectDetail,
                    IdRfpWorkTypeCategory = x.IdRfpWorkTypeCategory,
                    RfpWorkTypeCategory = x.RfpWorkTypeCategory != null ? x.RfpWorkTypeCategory.Name : string.Empty,
                    IdRfpWorkType = x.IdRfpWorkType,
                    RfpWorkType = x.RfpWorkType != null ? x.RfpWorkType.Name : string.Empty,
                    Cost = x.Cost,
                    Quantity = x.Quantity,
                    TotalCost = x.TotalCost,
                    Description = x.Description,
                    Checked = true
                }) : null,
                RfpJobType = projectDetail.RfpJobType != null ? projectDetail.RfpJobType.Name : string.Empty,
                RfpSubJobType = projectDetail.RfpSubJobType != null ? projectDetail.RfpSubJobType.Name : string.Empty,
                RfpSubJobTypeCategory = projectDetail.RfpSubJobTypeCategory != null ? projectDetail.RfpSubJobTypeCategory.Name : string.Empty,
                WorkDescription = projectDetail.WorkDescription,
            };
        }

        /// <summary>
        /// Updates the scope review.
        /// </summary>
        /// <param name="rfp">The RFP.</param>
        private void updateScopeReview(int Idrfp)
        {
            Rfp rfp = this.rpoContext.Rfps.Include("ScopeReview")
                             .Include("ProjectDetails.RfpFeeSchedules.RfpWorkType.RfpJobTypeCostRanges")
                             .Include("ProjectDetails.RfpFeeSchedules.RfpWorkType.RfpJobTypeCumulativeCosts").FirstOrDefault(x => x.Id == Idrfp);

            if (rfp.ScopeReview == null)
            {
                rfp.ScopeReview = new RfpScopeReview();
            }

            RfpScopeReview scopeReview = rfp.ScopeReview;

            scopeReview.Description = string.Empty;

            double totalCost = 0;
            string jobDescription = string.Empty;
            foreach (ProjectDetail item in rfp.ProjectDetails.OrderBy(x => x.DisplayOrder))
            {
        
                RfpFeeSchedule rfpfee = null;
              
                    rfpfee = item.RfpFeeSchedules.FirstOrDefault(x => x.RfpWorkType != null  && x.RfpWorkType.ServiceDescription != null &&  x.RfpWorkType.ServiceDescription.Contains("##AllSelectedBelow##"));
                    
                
                if (item.RfpFeeSchedules.Count > 0)
                {
                    jobDescription = jobDescription + "<ul>";
                }

                foreach (RfpFeeSchedule rfpFeeSchedule in item.RfpFeeSchedules.OrderBy(x => x.Id))
                {
                    if (rfpFeeSchedule.RfpWorkType != null)
                    {
                        if (!string.IsNullOrEmpty(rfpFeeSchedule.RfpWorkType.ServiceDescription))
                        {
                            string serviceDescription = rfpFeeSchedule.RfpWorkType.ServiceDescription;
                            if (rfpFeeSchedule.RfpWorkType.CustomServiceDescription)
                            {
                                serviceDescription = serviceDescription.Replace("##ServiceDescription##", rfpFeeSchedule.Description != null ? rfpFeeSchedule.Description : string.Empty);
                                serviceDescription = serviceDescription.Replace("##ServiceDescription##", rfpFeeSchedule.Description != null ? rfpFeeSchedule.Description : string.Empty);
                            }

                            if (rfpFeeSchedule.RfpWorkType.CostType == RfpCostType.PerUnitPrice || rfpFeeSchedule.RfpWorkType.CostType == RfpCostType.AdditionalCostPerUnit
                                || rfpFeeSchedule.RfpWorkType.CostType == RfpCostType.CummulativeCost || rfpFeeSchedule.RfpWorkType.CostType == RfpCostType.CostForUnitRange || rfpFeeSchedule.RfpWorkType.CostType == RfpCostType.HourlyCost)
                            {
                                serviceDescription = serviceDescription.Replace("##ServiceQuantity##", rfpFeeSchedule.Quantity != null ? Convert.ToString(rfpFeeSchedule.Quantity) : string.Empty);
                                serviceDescription = serviceDescription.Replace("##ServiceQuantity##", rfpFeeSchedule.Quantity != null ? Convert.ToString(rfpFeeSchedule.Quantity) : string.Empty);
                            }


                            if (serviceDescription.Contains("##AllSelectedBelow##"))
                            {
                                string allselectedServices = string.Empty;
                                foreach (RfpFeeSchedule appendRrfpFeeSchedule in item.RfpFeeSchedules.Where(x => x.IdRfpWorkTypeCategory == rfpFeeSchedule.IdRfpWorkTypeCategory).OrderBy(x => x.Id))
                                {
                                    string appendserviceDescription = string.Empty;
                                    if (appendRrfpFeeSchedule.RfpWorkType != null)
                                    {
                                        if (!string.IsNullOrEmpty(appendRrfpFeeSchedule.RfpWorkType.ServiceDescription))
                                        {
                                            appendserviceDescription = appendRrfpFeeSchedule.RfpWorkType.ServiceDescription;
                                            if (appendRrfpFeeSchedule.RfpWorkType.CustomServiceDescription)
                                            {
                                                appendserviceDescription = appendserviceDescription.Replace("##ServiceDescription##", appendRrfpFeeSchedule.Description != null ? appendRrfpFeeSchedule.Description : string.Empty);
                                                appendserviceDescription = appendserviceDescription.Replace("##ServiceDescription##", appendRrfpFeeSchedule.Description != null ? appendRrfpFeeSchedule.Description : string.Empty);
                                            }

                                            if (appendRrfpFeeSchedule.RfpWorkType.CostType == RfpCostType.PerUnitPrice || appendRrfpFeeSchedule.RfpWorkType.CostType == RfpCostType.AdditionalCostPerUnit
                                                || appendRrfpFeeSchedule.RfpWorkType.CostType == RfpCostType.CummulativeCost || appendRrfpFeeSchedule.RfpWorkType.CostType == RfpCostType.CostForUnitRange || appendRrfpFeeSchedule.RfpWorkType.CostType == RfpCostType.HourlyCost)
                                            {
                                                appendserviceDescription = appendserviceDescription.Replace("##ServiceQuantity##", appendRrfpFeeSchedule.Quantity != null ? Convert.ToString(appendRrfpFeeSchedule.Quantity) : string.Empty);
                                                appendserviceDescription = appendserviceDescription.Replace("##ServiceQuantity##", appendRrfpFeeSchedule.Quantity != null ? Convert.ToString(appendRrfpFeeSchedule.Quantity) : string.Empty);
                                            }
                                        }
                                    }

                                    //string allselectedServices = string.Join(", ", item.RfpFeeSchedules.Select(x => x.RfpWorkType.ServiceDescription));
                                    if (rfpFeeSchedule.IdRfpWorkType != appendRrfpFeeSchedule.IdRfpWorkType)
                                    {
                                        if (!string.IsNullOrEmpty(allselectedServices))
                                        {
                                            allselectedServices = allselectedServices + ", " + appendserviceDescription;
                                        }
                                        else
                                        {
                                            allselectedServices = allselectedServices + appendserviceDescription;
                                        }
                                    }
                                }

                                serviceDescription = serviceDescription.Replace("##AllSelectedBelow##", allselectedServices.Trim(','));

                                if (rfpfee != null && rfpfee.IdRfpWorkTypeCategory != null && rfpfee.IdRfpWorkTypeCategory == rfpFeeSchedule.IdRfpWorkTypeCategory)
                                {
                                    jobDescription = jobDescription + "<li>" + serviceDescription + "</li>";
                                }
                            }

                            if (rfpfee == null || rfpfee.IdRfpWorkTypeCategory != rfpFeeSchedule.IdRfpWorkTypeCategory)
                            {
                                jobDescription = jobDescription + "<li>" + serviceDescription + "</li>";
                            }

                        }

                        switch (rfpFeeSchedule.RfpWorkType.CostType)
                        {
                            case RfpCostType.AdditionalCostPerUnit:
                                rfpFeeSchedule.Cost = rfpFeeSchedule.RfpWorkType.Cost;
                                rfpFeeSchedule.TotalCost = rfpFeeSchedule.Cost + ((rfpFeeSchedule.Quantity - 1) * rfpFeeSchedule.RfpWorkType.AdditionalUnitPrice);
                                break;
                            case RfpCostType.CostForUnitRange:

                                var perunitcost = rfpFeeSchedule.RfpWorkType.RfpJobTypeCostRanges != null ?
                                                   rfpFeeSchedule.RfpWorkType.RfpJobTypeCostRanges
                                                  .Where(x => x.MaximumQuantity >= rfpFeeSchedule.Quantity && x.MinimumQuantity <= rfpFeeSchedule.Quantity)
                                                  .Select(x => x.Cost).FirstOrDefault()
                                                  : 0;
                                rfpFeeSchedule.Cost = perunitcost;
                                rfpFeeSchedule.TotalCost = (rfpFeeSchedule.Quantity * rfpFeeSchedule.Cost);

                                break;
                            case RfpCostType.CummulativeCost:
                                if (rfpFeeSchedule.RfpWorkType.RfpJobTypeCumulativeCosts != null)
                                {
                                    var cummulativeCostList = rfpFeeSchedule.RfpWorkType.RfpJobTypeCumulativeCosts.Where(x => x.Quantity <= rfpFeeSchedule.Quantity).ToList();

                                    if (cummulativeCostList != null && cummulativeCostList.Count > 0)
                                    {
                                        int maxQuantity = cummulativeCostList.Max(x => x.Quantity);
                                        if (maxQuantity == rfpFeeSchedule.Quantity)
                                        {
                                            rfpFeeSchedule.Cost = 0;
                                            rfpFeeSchedule.TotalCost = cummulativeCostList.Sum(x => x.Cost);
                                        }
                                        else
                                        {
                                            int samePriceQuantity = Convert.ToInt32(rfpFeeSchedule.Quantity) - maxQuantity;
                                            double maxQuantityCost = cummulativeCostList.Where(x => x.Quantity == maxQuantity).Select(x => (double)x.Cost).FirstOrDefault();
                                            double cummulativeCost = cummulativeCostList.Sum(x => (double)x.Cost);
                                            rfpFeeSchedule.Cost = 0;
                                            rfpFeeSchedule.TotalCost = cummulativeCost + (samePriceQuantity * maxQuantityCost);
                                        }
                                    }
                                    else
                                    {
                                        rfpFeeSchedule.Cost = 0;
                                        rfpFeeSchedule.TotalCost = (rfpFeeSchedule.Quantity * rfpFeeSchedule.Cost);
                                    }
                                }
                                else
                                {
                                    rfpFeeSchedule.Cost = 0;
                                    rfpFeeSchedule.TotalCost = (rfpFeeSchedule.Quantity * rfpFeeSchedule.Cost);
                                }
                                break;
                            case RfpCostType.FixedCost:
                                rfpFeeSchedule.Cost = rfpFeeSchedule.RfpWorkType.Cost;
                                rfpFeeSchedule.TotalCost = rfpFeeSchedule.Cost;
                                break;
                            case RfpCostType.MinimumCost:
                                rfpFeeSchedule.Cost = rfpFeeSchedule.RfpWorkType.Cost;
                                rfpFeeSchedule.TotalCost = rfpFeeSchedule.Cost;
                                break;
                            case RfpCostType.PerUnitPrice:
                                rfpFeeSchedule.Cost = rfpFeeSchedule.RfpWorkType.Cost;
                                rfpFeeSchedule.TotalCost = (rfpFeeSchedule.Quantity * rfpFeeSchedule.Cost);
                                break;
                            case RfpCostType.HourlyCost:
                                rfpFeeSchedule.Cost = rfpFeeSchedule.RfpWorkType.Cost;
                                rfpFeeSchedule.TotalCost = (rfpFeeSchedule.Quantity * rfpFeeSchedule.Cost);
                                break;
                        }
                    }
                    totalCost = totalCost + Convert.ToDouble(rfpFeeSchedule.TotalCost);
                }

                if (item.RfpFeeSchedules.Count > 0)
                {
                    jobDescription = jobDescription + "</ul>";
                }
            }

            scopeReview.Description = jobDescription;
            rfp.Cost = totalCost;

            this.rpoContext.SaveChanges();
        }
    }
}