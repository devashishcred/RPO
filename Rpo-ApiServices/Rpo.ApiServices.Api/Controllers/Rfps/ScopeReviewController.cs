// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-31-2018
// ***********************************************************************
// <copyright file="ScopeReviewController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

/// <summary>
/// The Rfps namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Rfps
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Get or Update ScopeReview
    /// For ScopeReviewRecientsType
    /// {
    /// NoAddContacts = 0,
    /// AddContactsToTo = 1,
    /// AddContactsToCc = 2,
    /// AddContactsToBcc = 3
    /// }
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class ScopeReviewController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the scope review.
        /// </summary>
        /// <param name="idRfp">The identifier RFP.</param>
        /// <returns>Get the list of scope reviews.</returns>
        [Authorize]
        [RpoAuthorize]
        [Route("api/Rfps/{idRfp}/ScopeReview")]
        [ResponseType(typeof(RfpScopeReviewDTO))]
        public IHttpActionResult GetScopeReview(int idRfp)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewRFP))
            {
                var rfp = rpoContext.Rfps.Include("ScopeReview").FirstOrDefault(r => r.Id == idRfp);

                if (rfp == null)
                {
                    return this.NotFound();
                }

                rfp.SetResponseGoNextStepHeader();
                rpoContext.Configuration.LazyLoadingEnabled = false;


                return Ok(Format(rfp.ScopeReview, rfp.Id));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Send on scopeReview.ContactCc a array of contacts with only Id filled. All other properties from contact will be ignored.
        /// </summary>
        /// <param name="idRfp">The identifier RFP.</param>
        /// <param name="rfpScopeReviewCreateUpdate">The scope review.</param>
        /// <returns>update the detail of scope review.</returns>
        [Authorize]
        [RpoAuthorize]
        [Route("api/Rfps/{idRfp}/ScopeReview")]
        [ResponseType(typeof(RfpScopeReviewDTO))]
        public IHttpActionResult PutScopeReview(int idRfp, RfpScopeReviewCreateUpdate rfpScopeReviewCreateUpdate)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddRFP))
            {
                var rfp = rpoContext.Rfps.FirstOrDefault(r => r.Id == idRfp);
                if (rfpScopeReviewCreateUpdate == null)
                {
                    return BadRequest();
                }
                if (rfpScopeReviewCreateUpdate.ContactsCc == null)
                {
                    return BadRequest();
                }

                if (rfp == null)
                {
                    return this.NotFound();
                }

                if (employee != null)
                {
                    rfp.IdLastModifiedBy = employee.Id;
                }
                rfp.LastModifiedDate = DateTime.UtcNow;

                rfp.ProcessGoNextStepHeader();

                if (rfpScopeReviewCreateUpdate.ContactsCc != null && rfpScopeReviewCreateUpdate.ContactsCc.Count() > 0)
                {
                    rfp.ScopeReview.ContactsCc = string.Join(",", rfpScopeReviewCreateUpdate.ContactsCc.Select(x => x.ToString()));
                }
                else
                {
                    rfp.ScopeReview.ContactsCc = string.Empty;
                }
                //remove div tag in rfps content editor
                if (rfpScopeReviewCreateUpdate.Description.Contains("<div") && rfpScopeReviewCreateUpdate.Description.Contains("</div>"))
                {
                    rfp.ScopeReview.Description = RemoveDivtag(rfpScopeReviewCreateUpdate.Description);
                }
                else
                {
                    rfp.ScopeReview.Description = rfpScopeReviewCreateUpdate.Description;
                }

                //rfp.ScopeReview.Description = rfpScopeReviewCreateUpdate.Description;
                rfp.ScopeReview.GeneralNotes = rfpScopeReviewCreateUpdate.GeneralNotes;

                rfp.LastUpdatedStep = 3;
                if (rfp.CompletedStep < 3)
                {
                    rfp.CompletedStep = 3;
                }
                try
                {
                    rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RfpExists(idRfp))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                int idVerbiage = rpoContext.Verbiages.Where(x => x.Name == "scope").Select(x => x.Id).FirstOrDefault();
                RfpProposalReview rfpProposalReview = rpoContext.RfpProposalReviews.FirstOrDefault(x => x.IdRfp == idRfp && x.IdVerbiage == idVerbiage);
                if (rfpProposalReview != null)
                {
                    rfpProposalReview.IdRfp = rfp.Id;
                    rfpProposalReview.Content = rfp.ScopeReview.Description;
                    rfpProposalReview.IdVerbiage = idVerbiage;
                }
                else
                {
                    rfpProposalReview = new RfpProposalReview();
                    rfpProposalReview.IdRfp = rfp.Id;
                    rfpProposalReview.Content = rfp.ScopeReview.Description;
                    rfpProposalReview.IdVerbiage = idVerbiage;
                    rpoContext.RfpProposalReviews.Add(rfpProposalReview);
                }

                try
                {
                    rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RfpExists(idRfp))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                rfp.SetResponseGoNextStepHeader();
                return Ok(Format(rfp.ScopeReview, rfp.Id));
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
        /// RFPs the exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool RfpExists(int id)
        {
            return rpoContext.Rfps.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified RFP scope review.
        /// </summary>
        /// <param name="rfpScopeReview">The RFP scope review.</param>
        /// <param name="idRfp">The identifier RFP.</param>
        /// <returns>RfpScopeReviewDTO.</returns>
        private RfpScopeReviewDTO Format(RfpScopeReview rfpScopeReview, int idRfp)
        {
            List<int> contactsCcList = rfpScopeReview.ContactsCc != null && !string.IsNullOrEmpty(rfpScopeReview.ContactsCc) ? (rfpScopeReview.ContactsCc.Split(',') != null && rfpScopeReview.ContactsCc.Split(',').Any() ? rfpScopeReview.ContactsCc.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            return new RfpScopeReviewDTO
            {
                Id = rfpScopeReview.Id,
                ContactsCc = rfpScopeReview.ContactsCc,
                Description = rfpScopeReview.Description,
                GeneralNotes = rfpScopeReview.GeneralNotes,
                IdRfp = idRfp,
                ContactsCcList = rpoContext.Contacts.Where(x => contactsCcList.Contains(x.Id)).Select(x => new RfpScopeContactTeam
                {
                    Id = x.Id,
                    ItemName = x.FirstName + " " + x.LastName
                }),
            };
        }

        string finalDescription = string.Empty;
        private String RemoveDivtag(string ScopeDescription)
        {

            string TempDescription = ScopeDescription;
            if (ScopeDescription.Contains("<div") && ScopeDescription.Contains("</div>"))
            {
                //     removespan.Remove(removespan.IndexOf("<span"), removespan.IndexOf("\">") - 2);
                TempDescription = ScopeDescription.Remove(ScopeDescription.IndexOf("<div"), 5);
                TempDescription = TempDescription.Remove(TempDescription.IndexOf("</div>"), 6);

                //rfp.ScopeReview.Description = Regex.Replace(rfpScopeReviewCreateUpdate.Description, "<((?!li).)*?>", String.Empty);                          

            }
            if (TempDescription.Contains("<div") && TempDescription.Contains("</div>"))
            {
                finalDescription = RemoveDivtag(TempDescription);

            }
            finalDescription = TempDescription;
            return finalDescription;

        }

    }
}
