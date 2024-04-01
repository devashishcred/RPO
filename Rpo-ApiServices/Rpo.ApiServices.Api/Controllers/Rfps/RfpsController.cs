// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-31-2018
// ***********************************************************************
// <copyright file="RfpsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Rfps Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Rfps namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Rfps
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using GlobalSearch;
    using iTextSharp.text;
    using iTextSharp.text.html;
    using iTextSharp.text.pdf;
    using iTextSharp.text.pdf.parser;
    using iTextSharp.tool.xml;
    using Model.Models.Enums;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Hubs;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using System.Net.Http.Headers;
    using System.Configuration;
    using System.Data.SqlClient;
    using Microsoft.ApplicationBlocks.Data;
    using Newtonsoft.Json;

    /// <summary>
    /// Class Rfps Controller.
    /// </summary>
    /// <seealso cref="Rpo.ApiServices.Api.Controllers.HubApiController{Rpo.ApiServices.Api.Hubs.GroupHub}" />
    public class RfpsController : HubApiController<GroupHub>
    {
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the RFPS.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns> To Get hte RFPs List</returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        public IHttpActionResult GetRfps([FromUri] RfpAdvancedSearchParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewRFP)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddRFP)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteRFP))
            {
                var recordsTotal = rpoContext.Rfps.Count();
                var recordsFiltered = recordsTotal;

                if (dataTableParameters.OrderedColumn == null)
                {
                    dataTableParameters.OrderedColumn = new OrderedColumn
                    {
                        Column = "Id",
                        Dir = "asc"
                    };
                }

                IQueryable<Rfp> rfps = rpoContext.Rfps.Include("RfpStatus").Include("CreatedBy").AsQueryable();

                if (dataTableParameters.GlobalSearchType != null && dataTableParameters.GlobalSearchType > 0)
                {
                    switch ((GlobalSearchType)dataTableParameters.GlobalSearchType)
                    {
                        case GlobalSearchType.RFPNumber:
                            rfps = rfps.Where(r => r.RfpNumber.Contains(dataTableParameters.GlobalSearchText.Trim()));
                            break;
                    }
                }

                if (dataTableParameters.IdCompany != null)
                {
                    rfps = rfps.Where(r => r.IdCompany == dataTableParameters.IdCompany);
                }

                if (dataTableParameters.IdContact != null)
                {
                    rfps = rfps.Where(r => r.IdContact == dataTableParameters.IdContact);
                }

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                IEnumerable<rfpListDTO> result = rfps.AsEnumerable()
                    .Select(rfp => new rfpListDTO
                    {
                        //Address1 = rfp.Address1,
                        //Address2 = rfp.Address2,
                        //Apartment = rfp.Apartment,
                        //Block = rfp.Block,
                        Borough = rfp.Borough != null ? rfp.Borough.Description : null,
                        Company = rfp.Company != null ? rfp.Company.Name : null,
                        //  Contact = rfp.Contact != null ? rfp.Contact.FirstName + " " + rfp.Contact.LastName : null,
                        Cost = rfp.Cost != null ? rfp.Cost.ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                        //Email = rfp.Email,
                        //FloorNumber = rfp.FloorNumber,
                        //HasEnvironmentalRestriction = rfp.HasEnvironmentalRestriction,
                        //HasLandMarkStatus = rfp.HasLandMarkStatus,
                        //HasOpenWork = rfp.HasOpenWork,
                        HouseNumber = rfp.RfpAddress != null ? rfp.RfpAddress.HouseNumber : null,
                        Id = rfp.Id,
                        //IdBorough = rfp.IdBorough,
                        //IdCompany = rfp.IdCompany,
                        IdContact = rfp.IdContact,
                        //IdReferredByCompany = rfp.IdReferredByCompany,
                        //IdReferredByContact = rfp.IdReferredByContact,
                        IdRfpAddress = rfp.IdRfpAddress,
                        LastModifiedDate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfp.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)),
                        LastUpdatedStep = rfp.LastUpdatedStep,
                        //CompletedStep = rfp.CompletedStep,
                        //Lot = rfp.Lot,
                        //Phone = rfp.Phone,
                        CreatedBy = rfp.CreatedBy != null ? rfp.CreatedBy.FirstName + " " + rfp.CreatedBy.LastName : null,
                        //ReferredByCompany = rfp.ReferredByCompany != null ? rfp.ReferredByCompany.Name : null,
                        //ReferredByContact = rfp.ReferredByContact != null ? rfp.ReferredByContact.FirstName + " " + rfp.ReferredByContact.LastName : null,
                        RfpAddress = rfp.RfpAddress != null ? rfp.RfpAddress.Street : null,
                        ZipCode = rfp.RfpAddress != null ? rfp.RfpAddress.ZipCode : null,
                        RfpNumber = rfp.RfpNumber,
                        SpecialPlace = rfp.SpecialPlace,
                        IdRfpStatus = rfp.IdRfpStatus,
                        RfpStatusDisplayName = rfp.RfpStatus != null ? rfp.RfpStatus.DisplayName : string.Empty,
                        RfpStatus = rfp.RfpStatus != null ? rfp.RfpStatus.Name : string.Empty,
                        StreetNumber = rfp.StreetNumber,
                        //  IdClientAddress = rfp.IdClientAddress != null ? rfp.IdClientAddress : null,
                        ProjectDescription = rfp.ProjectDescription != null ? rfp.ProjectDescription : string.Empty,
                        //  OutsideNYC = rfp.RfpAddress != null ? rfp.RfpAddress.OutsideNYC : null,
                        // IdSignature = rfp.IdSignature != null ? rfp.IdSignature : string.Empty,
                    })
                    .AsQueryable()
                    .DataTableParameters(dataTableParameters, out recordsFiltered).OrderByDescending(x => x.LastModifiedDate)
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
        /// Gets the RFP.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>To Get hte RFPs in Detail</returns>
        [ResponseType(typeof(Rfp))]
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/Rfps/{id}")]
        public IHttpActionResult GetRfp(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewRFP)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddRFP)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteRFP))
            {
                Rfp rfp = rpoContext
                .Rfps
                .Include("Borough")
                .Include("RfpAddress")
                .Include("RfpAddress.Borough")
                .Include("Company")
                .Include("Contact")
                .Include("ProjectDetails")
                .Include("ScopeReview")
                .Include("ProposalReview")
                .Include("RfpReviewers")
                .Include("CreatedBy")
                .Include("LastModifiedBy")
                .Include("RfpDocuments")
                .FirstOrDefault(r => r.Id == id);

                if (rfp == null)
                {
                    return this.NotFound();
                }

                rpoContext.Configuration.LazyLoadingEnabled = false;

                rfp.SetResponseGoNextStepHeader();

                return Ok(FormatRfp(rfp));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the time zone list.
        /// </summary>
        /// <returns>To Get the time Zone list.</returns>
        [Route("api/Rfps/timezonelist")]
        [ResponseType(typeof(TimeZoneInfo))]
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        public IHttpActionResult GetTimeZoneList()
        {
            List<TimeZoneInfo> timeZoneInfoList = TimeZoneInfo.GetSystemTimeZones().ToList();
            return Ok(timeZoneInfoList);
        }

        /// <summary>
        /// Puts the RFP.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="rfp">The RFP.</param>
        /// <returns>Update the RFP 1st step in database.</returns>
        [ResponseType(typeof(void))]
        [Authorize]
        [RpoAuthorize]
        [HttpPut]
        [Route("api/Rfps/{id}")]
        public IHttpActionResult PutRfp(int id, RfpCreateUpdate rfpDTO)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddRFP))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != rfpDTO.Id)
                {
                    return BadRequest();
                }

                Rfp rfp = rfpDTO.CloneAs<Rfp>();

                Address objCompanycontactAddress = (from d in rpoContext.Addresses where d.Id == rfp.IdClientAddress select d).FirstOrDefault();

                string Add1 = objCompanycontactAddress != null && !string.IsNullOrEmpty(rfp.Address1) ? objCompanycontactAddress.Address1 : string.Empty;
                string Add2 = objCompanycontactAddress != null && !string.IsNullOrEmpty(rfp.Address2) ? objCompanycontactAddress.Address2 : string.Empty;
                string City = objCompanycontactAddress != null && !string.IsNullOrEmpty(rfp.City) ? objCompanycontactAddress.City : string.Empty;

                rfp.Address1 = !string.IsNullOrEmpty(rfp.Address1) ? rfp.Address1 : Add1;
                rfp.Address2 = !string.IsNullOrEmpty(rfp.Address2) ? rfp.Address2 : Add2;
                rfp.City = !string.IsNullOrEmpty(rfp.City) ? rfp.City : City;


                CleanNavigation(rfp);
                rfp.RfpNumber = rfp.Id.ToString();//.PadLeft(8, '0');
                rfp.ProcessGoNextStepHeader();

                if (employee != null)
                {
                    rfp.IdLastModifiedBy = employee.Id;
                }
                rfp.LastModifiedDate = DateTime.UtcNow;

                if (rfpDTO.DocumentsToDelete != null)
                {
                    List<int> deletedDocs = rfpDTO.DocumentsToDelete.ToList();
                    rpoContext.RfpDocuments.RemoveRange(rpoContext.RfpDocuments.Where(ac => ac.IdRfp == rfp.Id && deletedDocs.Any(eacIds => eacIds == ac.Id)));
                }


                if (rfpDTO.DocumentsToDelete != null)
                {
                    foreach (var item in rfpDTO.DocumentsToDelete)
                    {
                        int rfpDocumentId = Convert.ToInt32(item);
                        RfpDocument rfpDocuments = rpoContext.RfpDocuments.Where(x => x.Id == rfpDocumentId).FirstOrDefault();
                        if (rfpDocuments != null)
                        {
                            rpoContext.RfpDocuments.Remove(rfpDocuments);
                            var path = HttpRuntime.AppDomainAppPath;
                            string directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.RFPDocumentPath));
                            string directoryDelete = Convert.ToString(rfpDocuments.Id) + "_" + rfpDocuments.DocumentPath;
                            string deletefilename = System.IO.Path.Combine(directoryName, directoryDelete);

                            if (File.Exists(deletefilename))
                            {
                                File.Delete(deletefilename);
                            }
                        }
                    }
                }

                rpoContext.Entry(rfp).State = EntityState.Modified;

                try
                {
                    if (rfp.IdContact == 0)
                    {
                        rfp.IdContact = null;
                    }

                    rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RfpExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                rfp.SetResponseGoNextStepHeader();

                Rfp rfpnew = rpoContext
                .Rfps
                .Include("Borough")
                .Include("RfpAddress")
                .Include("RfpAddress.Borough")
                .Include("State")
                .Include("Company")
                .Include("Contact")
                .Include("ProjectDetails")
                .Include("ScopeReview")
                .Include("ProposalReview")
                .Include("RfpReviewers")
                .Include("CreatedBy")
                .Include("LastModifiedBy")
                .Include("RfpDocuments")
                .FirstOrDefault(r => r.Id == id);

                RfpProposalReview updateSubjectinVerbiage = rpoContext.RfpProposalReviews.Include("Verbiages").FirstOrDefault(x => x.IdRfp == id && x.Verbiages.Name == "subject");
                if (updateSubjectinVerbiage != null)
                {
                    updateSubjectinVerbiage.Content = rfpnew.RfpAddress != null ? "<p><strong>RE : Proposal for Consulting Services - " + ((rfpnew.RfpAddress.HouseNumber != null ? rfpnew.RfpAddress.HouseNumber + " " : string.Empty) +
                                                                        (rfpnew.RfpAddress.Street != null ? rfpnew.RfpAddress.Street + ", " : string.Empty) +
                                                                        (rfpnew.RfpAddress.Borough != null ? rfpnew.RfpAddress.Borough.Description : string.Empty) +
                                                                        (rfpnew.SpecialPlace != null && !string.IsNullOrEmpty(rfpnew.SpecialPlace) ? " | " + rfpnew.SpecialPlace : string.Empty) +
                                                                        (rfpnew.RfpAddress.OutsideNYC != null ? ", " + rfpnew.RfpAddress.OutsideNYC : string.Empty))
                                                                        + "</strong></p>" : string.Empty;
                    rpoContext.SaveChanges();
                }
                RfpProposalReview updateAddressVerbiage = rpoContext.RfpProposalReviews.Include("Verbiages").FirstOrDefault(x => x.IdRfp == id && x.Verbiages.Name == "addressee");
                if (updateAddressVerbiage != null)
                {
                    string subject = string.Empty;
                    //subject = rfpnew.Contact != null ? (rfpnew.Contact.Prefix != null ? "<p>" + rfpnew.Contact.Prefix.Name + " " : string.Empty) : string.Empty;
                    subject = rfpnew.Contact != null ? (rfpnew.Contact.Prefix != null ? "<p>" + rfpnew.Contact.Prefix.Name + " " : "<p>") : string.Empty;
                    //subject += rfpnew.Contact != null ? (rfpnew.Contact.FirstName + " " + (rfpnew.Contact.LastName != null ? rfpnew.Contact.LastName + "</p>" : "</p>" + string.Empty)) : string.Empty;
                    subject += rfpnew.Contact != null ? (rfpnew.Contact.FirstName + " " + (rfpnew.Contact.LastName != null ? rfpnew.Contact.LastName + " " + (rfpnew.Contact.Suffix != null ? rfpnew.Contact.Suffix.Description : string.Empty) + "</p>" : "</p>" + string.Empty)) : string.Empty;
                    subject += rfpnew != null && rfpnew.Company != null && rfpnew.Company.Name != null ? "<p>" + rfpnew.Company.Name + "</p>" : string.Empty;
                    subject += rfpnew.Address1 != null ? "<p>" + rfpnew.Address1 + " " + (rfpnew.Address2 != null ? rfpnew.Address2 : string.Empty) + "</p>" : string.Empty;
                    subject += rfpnew.City != null ? "<p>" + rfpnew.City + ", " + (rfpnew.State != null ? rfpnew.State.Acronym + "  " : string.Empty) + (rfpnew.ZipCode != null ? rfpnew.ZipCode : string.Empty) + "</p > " : string.Empty;
                    updateAddressVerbiage.Content = subject;
                    rpoContext.SaveChanges();
                }

                RfpProposalReview updatenameVerbiage = rpoContext.RfpProposalReviews.Include("Verbiages").FirstOrDefault(x => x.IdRfp == rfp.Id && x.Verbiages.Name.Contains("Introduction"));
                if (updatenameVerbiage != null)
                {
                    string content = string.Empty;
                    content = updatenameVerbiage.Verbiages.Content.Replace("##Name##", rfpnew.Contact != null ? (rfpnew.Contact.Prefix != null ? rfpnew.Contact.Prefix.Name : string.Empty) + " " + (rfpnew.Contact.LastName != null ? rfpnew.Contact.LastName : string.Empty) : string.Empty);
                    updatenameVerbiage.Content = content;
                    rpoContext.SaveChanges();
                }

                RfpProposalReview updateCompanynameVerbiage = rpoContext.RfpProposalReviews.Include("Verbiages").FirstOrDefault(x => x.IdRfp == rfp.Id && x.Verbiages.Name == "Header");
                if (updateCompanynameVerbiage != null)
                {
                    string content = string.Empty;
                    content = updateCompanynameVerbiage.Verbiages.Content.Replace("##Name##", rfpnew.Contact != null ? (rfpnew.Contact.Prefix != null ? rfpnew.Contact.Prefix.Name.Trim() : string.Empty) + " " + (rfpnew.Contact.FirstName != null ? rfpnew.Contact.FirstName + " " : string.Empty) + " " + (rfpnew.Contact.LastName != null ? rfpnew.Contact.LastName : string.Empty).Trim() : string.Empty).Replace("##Company##", rfpnew.Company != null && rfpnew.Company.Name != null ? rfpnew.Company.Name : string.Empty);
                    updateCompanynameVerbiage.Content = content;
                    rpoContext.SaveChanges();
                }

                return Ok(FormatDetails(rfp));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Cleans the navigation.
        /// </summary>
        /// <param name="rfp">The RFP.</param>
        private static void CleanNavigation(Rfp rfp)
        {
            rfp.Borough = null;
            rfp.Company = null;
            rfp.Contact = null;
            rfp.ProjectDetails = null;
            rfp.RfpFeeSchedules = null;
            rfp.ProposalReview = null;
            rfp.ReferredByCompany = null;
            rfp.ReferredByContact = null;
            rfp.RfpAddress = null;
            rfp.ScopeReview = null;
            rfp.Milestones = null;
            rfp.CreatedBy = null;
            rfp.LastModifiedBy = null;
            rfp.State = null;
            rfp.RfpDocuments = null;
            rfp.LastModifiedDate = DateTime.UtcNow;

            if (rfp.RfpReviewers != null && rfp.RfpReviewers.Count > 0)
            {
                foreach (RfpReviewer item in rfp.RfpReviewers)
                {
                    item.Reviewer = null;
                }
            }

        }

        /// <summary>
        /// Posts the RFP.
        /// </summary>
        /// <param name="rfp">The RFP.</param>
        /// <returns>Create new RFPs</returns>
        [ResponseType(typeof(Rfp))]
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        public IHttpActionResult PostRfp(Rfp rfp)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddRFP))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                CleanNavigation(rfp);
                rfp.ProcessGoNextStepHeader();
                if (rfp.IdContact == 0)
                {
                    rfp.IdContact = null;
                }
                if (rfp.CompletedStep < 1)
                {
                    rfp.CompletedStep = 1;
                }

                if (employee != null)
                {
                    rfp.IdCreatedBy = employee.Id;
                    rfp.IdLastModifiedBy = employee.Id;
                }

                Address objCompanycontactAddress = (from d in rpoContext.Addresses where d.Id == rfp.IdClientAddress select d).FirstOrDefault();

                string Add1 = objCompanycontactAddress != null && !string.IsNullOrEmpty(rfp.Address1) ? objCompanycontactAddress.Address1 : string.Empty;
                string Add2 = objCompanycontactAddress != null && !string.IsNullOrEmpty(rfp.Address2) ? objCompanycontactAddress.Address2 : string.Empty;
                string City = objCompanycontactAddress != null && !string.IsNullOrEmpty(rfp.City) ? objCompanycontactAddress.City : string.Empty;

                rfp.Address1 = !string.IsNullOrEmpty(rfp.Address1) ? rfp.Address1 : Add1;
                rfp.Address2 = !string.IsNullOrEmpty(rfp.Address2) ? rfp.Address2 : Add2;
                rfp.City = !string.IsNullOrEmpty(rfp.City) ? rfp.City : City;

                rfp.IdRfpStatus = 1;
                rfp.CreatedDate = DateTime.UtcNow;
                rfp.LastModifiedDate = DateTime.UtcNow;
                rfp.StatusChangedDate = DateTime.UtcNow;
                rfp = rpoContext.Rfps.Add(rfp);
                rpoContext.SaveChanges();

                rfp.RfpNumber = rfp.Id.ToString();//.PadLeft(8, '0');
                rpoContext.SaveChanges();

                Contact objcontact = (from d in rpoContext.Contacts where d.Id == rfp.IdContact select d).FirstOrDefault();
                Company objCompany = (from d in rpoContext.Companies where d.Id == rfp.IdCompany select d).FirstOrDefault();
                var verbiageList = rpoContext.Verbiages.ToList();


                var distinctVarbiage = verbiageList.Select(x => x.VerbiageType).Distinct();

                foreach (VerbiageType item in distinctVarbiage)
                {
                    Verbiage verbiage = verbiageList.FirstOrDefault(x => x.VerbiageType == item);

                    if (verbiage != null)
                    {
                        RfpProposalReview rfpProposalReview = new RfpProposalReview();

                        rfpProposalReview.Content = verbiage.Content;
                        rfpProposalReview.IdRfp = rfp.Id;
                        rfpProposalReview.IdVerbiage = verbiage.Id;
                        switch (verbiage.VerbiageType)
                        {
                            case VerbiageType.Introduction:
                                rfpProposalReview.Content = verbiage.Content.Replace("##Name##", objcontact != null ? (objcontact.Prefix != null ? objcontact.Prefix.Name : string.Empty) + " " + (objcontact.LastName != null ? objcontact.LastName : string.Empty) : string.Empty);
                                rfpProposalReview.DisplayOrder = 3;
                                break;
                            case VerbiageType.Header:
                                rfpProposalReview.Content = verbiage.Content.Replace("##Name##", objcontact != null ? (objcontact.Prefix != null ? objcontact.Prefix.Name : string.Empty) + " " + (objcontact.FirstName != null ? objcontact.FirstName + " " : string.Empty) + " " + (objcontact.LastName != null ? objcontact.LastName : string.Empty) : string.Empty).Replace("##Company##", objCompany != null && objCompany.Name != null ? objCompany.Name : "Not Set");
                                rfpProposalReview.DisplayOrder = 4;
                                break;
                            case VerbiageType.Scope:
                                rfpProposalReview.DisplayOrder = 5;
                                break;
                            case VerbiageType.Cost:
                                rfpProposalReview.DisplayOrder = 6;
                                break;
                            case VerbiageType.Milestone:
                                rfpProposalReview.DisplayOrder = 7;
                                break;
                            case VerbiageType.AdditionalScope:
                                rfpProposalReview.DisplayOrder = 8;
                                break;
                            case VerbiageType.Conclusion:
                                rfpProposalReview.DisplayOrder = 9;
                                break;
                            case VerbiageType.Sign:
                                rfpProposalReview.DisplayOrder = 10;
                                break;
                            case VerbiageType.Addressee:
                                rfpProposalReview.DisplayOrder = 1;
                                break;
                            case VerbiageType.Subject:
                                rfpProposalReview.DisplayOrder = 2;
                                break;
                        }
                        rpoContext.RfpProposalReviews.Add(rfpProposalReview);
                    }
                }
                rpoContext.SaveChanges();

                rfp.SetResponseGoNextStepHeader();

                Rfp rfpnew = rpoContext
                .Rfps
                .Include("Borough")
                .Include("RfpAddress")
                .Include("RfpAddress.Borough")
                .Include("State")
                .Include("Company")
                .Include("Contact")
                .Include("ProjectDetails")
                .Include("ScopeReview")
                .Include("ProposalReview")
                .Include("ProposalReview.Verbiages")
                .Include("RfpReviewers")
                .Include("CreatedBy")
                .Include("LastModifiedBy")
                .Include("RfpDocuments")
                .FirstOrDefault(r => r.Id == rfp.Id);

                RfpProposalReview updateSubjectinVerbiage = rpoContext.RfpProposalReviews.Include("Verbiages").FirstOrDefault(x => x.IdRfp == rfp.Id && x.Verbiages.Name == "subject");
                if (updateSubjectinVerbiage != null)
                {
                    updateSubjectinVerbiage.Content = rfpnew.RfpAddress != null ? "<p><strong>RE : Proposal for Consulting Services - " + ((rfpnew.RfpAddress.HouseNumber != null ? rfpnew.RfpAddress.HouseNumber + " " : string.Empty) +
                                                                        (rfpnew.RfpAddress.Street != null ? rfpnew.RfpAddress.Street + ", " : string.Empty) +
                                                                        (rfpnew.RfpAddress.Borough != null ? rfpnew.RfpAddress.Borough.Description : string.Empty) +
                                                                        (rfpnew.SpecialPlace != null && !string.IsNullOrEmpty(rfpnew.SpecialPlace) ? " | " + rfpnew.SpecialPlace : string.Empty) +
                                                                        (rfpnew.RfpAddress.OutsideNYC != null ? ", " + rfpnew.RfpAddress.OutsideNYC : string.Empty))
                                                                        + "</strong></p>" : string.Empty;
                }
                RfpProposalReview updateAddressVerbiage = rpoContext.RfpProposalReviews.Include("Verbiages").FirstOrDefault(x => x.IdRfp == rfp.Id && x.Verbiages.Name == "addressee");
                if (updateAddressVerbiage != null)
                {
                    string subject = string.Empty;
                    subject = rfpnew.Contact != null ? (rfpnew.Contact.Prefix != null ? "<p>" + rfpnew.Contact.Prefix.Name + " " : "<p>") : string.Empty;
                    subject += rfpnew.Contact != null ? (rfpnew.Contact.FirstName + " " + (rfpnew.Contact.LastName != null ? rfpnew.Contact.LastName + " " + (rfpnew.Contact.Suffix != null ? rfpnew.Contact.Suffix.Description : string.Empty) + "</p>" : "</p>" + string.Empty)) : string.Empty;
                    subject += rfpnew != null && rfpnew.Company != null && rfpnew.Company.Name != null ? "<p>" + rfpnew.Company.Name + "</p>" : string.Empty;
                    //subject += rfpnew.Address1 != null ? "<p>" + rfpnew.Address1 + "</p>" : string.Empty;
                    //subject += rfpnew.Address2 != null ? "<p>" + rfpnew.Address2 + "</p>" : string.Empty;
                    //subject += rfpnew.City != null ? "<p>" + rfpnew.City + " " + (rfpnew.State != null ? rfpnew.State.Acronym + "</p>" : string.Empty) : "</p>" + string.Empty;
                    //subject += rfpnew.ZipCode != null ? "<p>" + rfpnew.ZipCode + "</p>" : string.Empty;
                    subject += rfpnew.Address1 != null ? "<p>" + rfpnew.Address1 + " " + (rfpnew.Address2 != null ? rfpnew.Address2 : string.Empty) + "</p>" : string.Empty;
                    subject += rfpnew.City != null ? "<p>" + rfpnew.City + ", " + (rfpnew.State != null ? rfpnew.State.Acronym + "  " : string.Empty) + (rfpnew.ZipCode != null ? rfpnew.ZipCode : string.Empty) + "</p > " : string.Empty;
                    updateAddressVerbiage.Content = subject;
                }
                RfpProposalReview updatenameVerbiage = rpoContext.RfpProposalReviews.Include("Verbiages").FirstOrDefault(x => x.IdRfp == rfp.Id && x.Verbiages.Name.Contains("Introduction"));
                if (updateAddressVerbiage != null)
                {
                    string content = string.Empty;
                    content = updatenameVerbiage.Verbiages.Content.Replace("##Name##", rfpnew.Contact != null ? (rfpnew.Contact.Prefix != null ? rfpnew.Contact.Prefix.Name : string.Empty) + " " + (rfpnew.Contact.LastName != null ? rfpnew.Contact.LastName : string.Empty) : string.Empty);
                    updatenameVerbiage.Content = content;
                    //rpoContext.SaveChanges();
                }

                RfpProposalReview updateCompanynameVerbiage = rpoContext.RfpProposalReviews.Include("Verbiages").FirstOrDefault(x => x.IdRfp == rfp.Id && x.Verbiages.Name == "Header");
                if (updateCompanynameVerbiage != null)
                {
                    string content = string.Empty;
                    content = updateCompanynameVerbiage.Verbiages.Content.Replace("##Name##", rfpnew.Contact != null ? (rfpnew.Contact.Prefix != null ? rfpnew.Contact.Prefix.Name.Trim() : string.Empty) + " " + (rfpnew.Contact.FirstName != null ? rfpnew.Contact.FirstName + " " : string.Empty) + " " + (rfpnew.Contact.LastName != null ? rfpnew.Contact.LastName : string.Empty).Trim() : string.Empty).Replace("##Company##", rfpnew.Company != null && rfpnew.Company.Name != null ? rfpnew.Company.Name : string.Empty);
                    updateCompanynameVerbiage.Content = content;
                    //rpoContext.SaveChanges();
                }
                rpoContext.SaveChanges();
                return Ok(FormatDetails(rfp));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the clone RFP.
        /// </summary>
        /// <remarks>To create clon of the selected same RFP</remarks>
        /// <param name="idRfp">The identifier RFP.</param>
        /// <returns>To create a clone of the same RFP.</returns>
        [ResponseType(typeof(Rfp))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Rfps/{idRfp}/clone")]
        [HttpPost]
        public IHttpActionResult PostCloneRfp(int idRfp)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddRFP))
            {
                var rfp = rpoContext.Rfps.Include("ScopeReview")
                                     .Include("RfpFeeSchedules")
                                     .Include("RfpFeeSchedules.RfpWorkType")
                                     .Include("ProjectDetails.RfpFeeSchedules")
                                     .Include("RfpReviewers")
                                     .Include("ProposalReview")
                                     .Include("Milestones.MilestoneServices")
                                     .Include("RfpDocuments")
                                     .FirstOrDefault(x => x.Id == idRfp);

                if (rfp == null)
                {
                    return this.NotFound();
                }

                Rfp cloneRFP = new Rfp();

                cloneRFP.IdRfpStatus = rfp.IdRfpStatus;
                cloneRFP.IdRfpAddress = rfp.IdRfpAddress;
                cloneRFP.IdBorough = rfp.IdBorough;
                cloneRFP.HouseNumber = rfp.HouseNumber;
                cloneRFP.StreetNumber = rfp.StreetNumber;
                cloneRFP.FloorNumber = rfp.FloorNumber;
                cloneRFP.Apartment = rfp.Apartment;
                cloneRFP.SpecialPlace = rfp.SpecialPlace;
                cloneRFP.Block = rfp.Block;
                cloneRFP.Lot = rfp.Lot;
                cloneRFP.HasLandMarkStatus = rfp.HasLandMarkStatus;
                cloneRFP.HasEnvironmentalRestriction = rfp.HasEnvironmentalRestriction;
                cloneRFP.HasOpenWork = rfp.HasOpenWork;
                cloneRFP.IdCompany = rfp.IdCompany;
                cloneRFP.IdContact = rfp.IdContact;
                cloneRFP.Address1 = rfp.Address1;
                cloneRFP.Address2 = rfp.Address2;
                cloneRFP.ProjectDescription = rfp.ProjectDescription;
                cloneRFP.Phone = rfp.Phone;
                cloneRFP.Email = rfp.Email;
                cloneRFP.IdReferredByCompany = rfp.IdReferredByCompany;
                cloneRFP.IdReferredByContact = rfp.IdReferredByContact;
                cloneRFP.GoNextStep = rfp.GoNextStep;
                cloneRFP.LastUpdatedStep = rfp.LastUpdatedStep;
                cloneRFP.CompletedStep = rfp.CompletedStep;
                cloneRFP.IdRfpScopeReview = rfp.IdRfpScopeReview;
                cloneRFP.City = rfp.City;
                cloneRFP.IdState = rfp.IdState;
                cloneRFP.ZipCode = rfp.ZipCode;
                cloneRFP.Cost = rfp.Cost;
                cloneRFP.IdClientAddress = rfp.IdClientAddress;
                if (employee != null)
                {
                    cloneRFP.IdCreatedBy = employee.Id;
                    cloneRFP.IdLastModifiedBy = employee.Id;
                }

                cloneRFP.IdRfpStatus = 1;
                cloneRFP.CreatedDate = DateTime.UtcNow;
                cloneRFP.LastModifiedDate = DateTime.UtcNow;
                cloneRFP.StatusChangedDate = DateTime.UtcNow;

                cloneRFP = rpoContext.Rfps.Add(cloneRFP);
                rpoContext.SaveChanges();

                cloneRFP.RfpNumber = cloneRFP.Id.ToString();//.PadLeft(8, '0');
                rpoContext.SaveChanges();

                if (rfp.RfpDocuments != null)
                {
                    foreach (var itemdoc in rfp.RfpDocuments)
                    {
                        RfpDocument rfpDocument = new RfpDocument();
                        rfpDocument.DocumentPath = itemdoc.DocumentPath;
                        rfpDocument.IdRfp = cloneRFP.Id;
                        rfpDocument.Name = itemdoc.DocumentPath;
                        rpoContext.RfpDocuments.Add(rfpDocument);
                        rpoContext.SaveChanges();
                    }
                }


                if (rfp.ScopeReview != null)
                {
                    RfpScopeReview rfpScopeReview = new RfpScopeReview();
                    rfpScopeReview.GeneralNotes = rfp.ScopeReview.GeneralNotes;
                    rfpScopeReview.Description = rfp.ScopeReview.Description;
                    rfpScopeReview.ContactsCc = rfp.ScopeReview.ContactsCc;

                    rfpScopeReview = rpoContext.RfpScopeReviews.Add(rfpScopeReview);
                    rpoContext.SaveChanges();

                    cloneRFP.IdRfpScopeReview = rfpScopeReview.Id;
                    rpoContext.SaveChanges();
                }

                if (rfp.ProjectDetails != null && rfp.ProjectDetails.Count > 0)
                {
                    foreach (ProjectDetail item in rfp.ProjectDetails)
                    {
                        ProjectDetail projectDetail = new ProjectDetail();
                        projectDetail.IdRfp = cloneRFP.Id;
                        projectDetail.WorkDescription = item.WorkDescription;
                        projectDetail.ArePlansNotPrepared = item.ArePlansNotPrepared;
                        projectDetail.ArePlansCompleted = item.ArePlansCompleted;
                        projectDetail.IsApproved = item.IsApproved;
                        projectDetail.IsDisaproved = item.IsDisaproved;
                        projectDetail.IsPermitted = item.IsPermitted;
                        projectDetail.ApprovedJobNumber = item.ApprovedJobNumber;
                        projectDetail.DisApprovedJobNumber = item.DisApprovedJobNumber;
                        projectDetail.PermittedJobNumber = item.PermittedJobNumber;
                        projectDetail.IdRfpJobType = item.IdRfpJobType;
                        projectDetail.IdRfpSubJobTypeCategory = item.IdRfpSubJobTypeCategory;
                        projectDetail.IdRfpSubJobType = item.IdRfpSubJobType;
                        projectDetail.DisplayOrder = item.DisplayOrder;

                        projectDetail = rpoContext.ProjectDetails.Add(projectDetail);
                        rpoContext.SaveChanges();

                        if (item.RfpFeeSchedules != null && item.RfpFeeSchedules.Count > 0)
                        {
                            foreach (RfpFeeSchedule feeSchedule in item.RfpFeeSchedules)
                            {
                                RfpFeeSchedule rfpFeeSchedule = new RfpFeeSchedule();
                                rfpFeeSchedule.IdRfp = cloneRFP.Id;
                                rfpFeeSchedule.IdProjectDetail = projectDetail.Id;

                                rfpFeeSchedule.IdRfpWorkTypeCategory = feeSchedule.IdRfpWorkTypeCategory;
                                rfpFeeSchedule.IdRfpWorkType = feeSchedule.IdRfpWorkType;
                                rfpFeeSchedule.Cost = feeSchedule.Cost;
                                rfpFeeSchedule.Quantity = feeSchedule.Quantity;
                                rfpFeeSchedule.TotalCost = feeSchedule.TotalCost;
                                rfpFeeSchedule.Description = feeSchedule.Description;
                                rfpFeeSchedule.IdOldRfpFeeSchedule = feeSchedule.Id;
                                rfpFeeSchedule.IdPartof = feeSchedule.RfpWorkType.PartOf;
                                rfpFeeSchedule = rpoContext.RfpFeeSchedules.Add(rfpFeeSchedule);
                            }
                            rpoContext.SaveChanges();
                        }

                    }
                }

                if (rfp.Milestones != null && rfp.Milestones.Count > 0)
                {
                    foreach (Milestone item in rfp.Milestones)
                    {
                        Milestone milestone = new Milestone();
                        milestone.IdRfp = cloneRFP.Id;
                        milestone.Name = item.Name;
                        milestone.Value = item.Value;
                        milestone = rpoContext.Milestones.Add(milestone);

                        rpoContext.SaveChanges();

                        if (item.MilestoneServices != null && item.MilestoneServices.Count > 0)
                        {
                            foreach (MilestoneService milestoneService in item.MilestoneServices)
                            {
                                RfpFeeSchedule rfpFeeSchedule = rpoContext.RfpFeeSchedules.FirstOrDefault(x => x.IdOldRfpFeeSchedule == milestoneService.IdRfpFeeSchedule && x.IdRfp == cloneRFP.Id);
                                MilestoneService rfpMilestoneService = new MilestoneService();
                                rfpMilestoneService.IdMilestone = milestone.Id;
                                rfpMilestoneService.IdRfpFeeSchedule = rfpFeeSchedule.Id;
                                rfpMilestoneService = rpoContext.MilestoneServices.Add(rfpMilestoneService);
                            }
                            rpoContext.SaveChanges();
                        }
                    }
                }

                //if (rfp.RfpReviewers != null && rfp.RfpReviewers.Count > 0)
                //{
                //    foreach (RfpReviewer item in rfp.RfpReviewers)
                //    {
                //        RfpReviewer rfpReviewer = new RfpReviewer();
                //        rfpReviewer.IdRfp = cloneRFP.Id;
                //        rfpReviewer.IdReviewer = item.IdReviewer;
                //        rfpReviewer = rpoContext.RfpReviewers.Add(rfpReviewer);
                //        rpoContext.SaveChanges();
                //    }
                //}

                if (rfp.ProposalReview != null && rfp.ProposalReview.Count > 0)
                {
                    foreach (RfpProposalReview item in rfp.ProposalReview)
                    {
                        RfpProposalReview rfpProposalReview = new RfpProposalReview();
                        rfpProposalReview.IdRfp = cloneRFP.Id;
                        rfpProposalReview.Content = item.Content;
                        rfpProposalReview.IdVerbiage = item.IdVerbiage;
                        rfpProposalReview.DisplayOrder = item.DisplayOrder;

                        rfpProposalReview = rpoContext.RfpProposalReviews.Add(rfpProposalReview);
                        rpoContext.SaveChanges();
                    }
                }

                return Ok(FormatDetails(cloneRFP));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the RFP.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Delete the RFPs</returns>
        [ResponseType(typeof(Rfp))]
        [Authorize]
        [RpoAuthorize]
        [HttpDelete]
        public IHttpActionResult DeleteRfp(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteRFP))
            {
                Rfp rfp = rpoContext.Rfps.Find(id);
                if (rfp == null)
                {
                    return this.NotFound();
                }

                rpoContext.Rfps.Remove(rfp);
                rpoContext.SaveChanges();

                return Ok(rfp);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Retrieve the RFP Status
        /// {
        /// Draft = 0,
        /// InReview = 1,
        /// Approved = 2,
        /// Cancel = 3,
        /// Lost = 4,
        /// OnHold = 5
        /// }
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>get the status of RFPs</returns>
        [HttpGet]
        [Route("api/Rfps/{id}/status")]
        [ResponseType(typeof(RfpStatus))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetStatus(int id)
        {
            Rfp rfp = rpoContext.Rfps.Find(id);
            if (rfp == null)
            {
                return this.NotFound();
            }
            rfp.SetResponseGoNextStepHeader();
            return Ok(rfp.RfpStatus);
        }

        /// <summary>
        /// Set the RFP Status
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">Draft = 0,
        /// InReview = 1,
        /// Approved = 2,
        /// Cancel = 3,
        /// Lost = 4,
        /// OnHold = 5</param>
        /// <returns>update the status of RFPs.</returns>
        [HttpPut]
        [Route("api/Rfps/{id}/status/{idRfpStatus}")]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult SetStatus(int id, int idRfpStatus)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddRFP))
            {
                Rfp rfp = rpoContext.Rfps.Find(id);
                if (rfp == null)
                {
                    return this.NotFound();
                }
                rfp.IdRfpStatus = idRfpStatus;
                rfp.LastUpdatedStep = 5;
                if (rfp.CompletedStep < 5)
                {
                    rfp.CompletedStep = 5;
                }
                rfp.ProcessGoNextStepHeader();

                if (employee != null)
                {
                    rfp.IdLastModifiedBy = employee.Id;
                }
                rfp.LastModifiedDate = DateTime.UtcNow;
                rfp.StatusChangedDate = DateTime.UtcNow;

                rpoContext.SaveChanges();

                //Hub.Clients.Group("rfp-status").status(id, "Ram Ram");
                //Hub.Clients.Group("2").notificationcount(id, "7");

                rfp.SetResponseGoNextStepHeader();
                return Ok(rfp);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="emailDTO">The email dto.</param>
        /// <returns>To send the email of selected contacts email</returns>
        /// <exception cref="RpoBusinessException">
        /// Contact Id not found
        /// or
        /// or
        /// Contact Id not found
        /// or
        /// or
        /// Contact Id not found
        /// or
        /// or
        /// Contact Id not found
        /// or
        /// </exception>
        [HttpPost]
        [Route("api/Rfps/{id}/email/")]
        [ResponseType(typeof(void))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult SendEmail(int id, [FromBody] RfpEmailDTO emailDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Rfp rfp = rpoContext.Rfps.Find(id);

            var rfpForPdf = rpoContext.Rfps
                    .Include("Borough")
                    .Include("RfpAddress.Borough")
                    .Include("Company")
                    .Include("Contact")
                    .Include("ProjectDetails")
                    .Include("ScopeReview")
                    .Include("ProposalReview.Verbiages").FirstOrDefault(x => x.Id == id);

            if (rfp == null)
            {
                return this.NotFound();
            }
            //update the functionality, if the attachment could not found
            string attachmentPath = string.Empty;
            if (emailDTO.IsProposalPDFAttached)
            {
                string fullpath = HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.ProposalFileExportPath;
                //string pdffilename = "RFP-Pdf_" + id + ".pdf";

                string pdffilename = "RFP #" + rfpForPdf.RfpNumber + " " + rfpForPdf.RfpAddress.HouseNumber + " " + rfpForPdf.RfpAddress.Street + " "
                + (rfpForPdf.RfpAddress.Borough != null ? rfpForPdf.RfpAddress.Borough.Description : string.Empty) + (!string.IsNullOrEmpty(rfpForPdf.SpecialPlace) ? " " + rfpForPdf.SpecialPlace : string.Empty) + "_" +
                (rfpForPdf.Company != null ? rfpForPdf.Company.Name : "Individual") + (rfpForPdf.Contact != null ? "_" + rfpForPdf.Contact.FirstName + (!string.IsNullOrEmpty(rfpForPdf.Contact.LastName) ? " " + rfpForPdf.Contact.LastName : string.Empty) : string.Empty)
                + ".pdf";
                //filename = filename.R(filename, @"<[^>]+>|&nbsp;", "");
                pdffilename = Regex.Replace(pdffilename, @"[^0-9a-zA-Z_.]+", "-");

                attachmentPath = fullpath + pdffilename;

                if (!File.Exists(attachmentPath))
                {
                    throw new RpoBusinessException("There was a problem while attaching the Proposal PDF. Kindly recreate the proposal.");
                }
            }

            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

            int responseIdRfp = 0;
            int responseIdRFPEmailHistory = 0;

            if (emailDTO.IsAdditionalAtttachment)
            {

                foreach (var dest in emailDTO.IdContactsCc.Select(c => rpoContext.Contacts.Find(c)))
                {
                    if (dest == null)
                    {
                        throw new RpoBusinessException("Contact Id not found");
                    }

                    if (dest.Email == null)
                    {
                        throw new RpoBusinessException($"Contact {dest.FirstName} not has a valid e-mail");
                    }
                    if (dest.IsActive == false)
                    {
                        throw new RpoBusinessException("Email can not send to inactive contact");
                    }
                }

                var contactTo1 = rpoContext.Contacts.Where(x => x.Id == emailDTO.IdContactAttention).FirstOrDefault();
                if (contactTo1 == null)
                {
                    throw new RpoBusinessException("Contact Id not found");
                }
                if (contactTo1.IsActive == false)
                {
                    throw new RpoBusinessException("Email can not send to inactive contact");
                }
                if (contactTo1.Email == null)
                {
                    throw new RpoBusinessException($"Contact {contactTo1.FirstName} not has a valid e-mail");
                }

                RFPEmailHistory rFPEmailHistory1 = new RFPEmailHistory();
                rFPEmailHistory1.IdRfp = rfp.Id;
                rFPEmailHistory1.IdFromEmployee = emailDTO.IdFromEmployee;
                rFPEmailHistory1.IdToCompany = emailDTO.IdContactsTo;
                rFPEmailHistory1.IdContactAttention = emailDTO.IdContactAttention;
                rFPEmailHistory1.IdTransmissionType = emailDTO.IdTransmissionType;
                rFPEmailHistory1.IdEmailType = emailDTO.IdEmailType;
                rFPEmailHistory1.SentDate = DateTime.UtcNow;
                rFPEmailHistory1.EmailSubject = emailDTO.Subject;
                if (employee != null)
                {
                    rFPEmailHistory1.IdSentBy = employee.Id;
                }
                rFPEmailHistory1.EmailMessage = emailDTO.Body;
                rFPEmailHistory1.IsAdditionalAtttachment = emailDTO.IsAdditionalAtttachment;
                // set email sent false by default M.B
                rFPEmailHistory1.IsEmailSent = false;

                rpoContext.RFPEmailHistories.Add(rFPEmailHistory1);
                rpoContext.SaveChanges();

                foreach (int idCc in emailDTO.IdContactsCc)
                {
                    RFPEmailCCHistory rFPEmailCCHistory = new RFPEmailCCHistory();
                    rFPEmailCCHistory.IdContact = idCc;
                    rFPEmailCCHistory.IdRFPEmailHistory = rFPEmailHistory1.Id;

                    rpoContext.RFPEmailCCHistories.Add(rFPEmailCCHistory);
                    rpoContext.SaveChanges();
                }

                responseIdRfp = rfp.Id;
                responseIdRFPEmailHistory = rFPEmailHistory1.Id;

            }
            else
            {

                var cc = new List<KeyValuePair<string, string>>();

                foreach (var dest in emailDTO.IdContactsCc.Select(c => rpoContext.Contacts.Find(c)))
                {
                    if (dest == null)
                        throw new RpoBusinessException("Contact Id not found");
                    if (dest.IsActive == false)
                    {
                        throw new RpoBusinessException("Email can not send to inactive contact");
                    }
                    if (dest.Email == null)
                        throw new RpoBusinessException($"Contact {dest.FirstName} not has a valid e-mail");

                    cc.Add(new KeyValuePair<string, string>(dest.Email, dest.FirstName + " " + dest.LastName));
                }

                var to = new List<KeyValuePair<string, string>>();

                var contactTo = rpoContext.Contacts.Where(x => x.Id == emailDTO.IdContactAttention).FirstOrDefault();
                if (contactTo == null)
                {
                    throw new RpoBusinessException("Contact Id not found");
                }
                if (contactTo.IsActive == false)
                {
                    throw new RpoBusinessException("Email can not send to inactive contact");
                }
                if (contactTo.Email == null)
                {
                    throw new RpoBusinessException($"Contact {contactTo.FirstName} not has a valid e-mail");
                }
                to.Add(new KeyValuePair<string, string>(contactTo.Email, contactTo.FirstName + " " + contactTo.LastName));


                var attachments = new List<string>();

                Employee employeeFrom = rpoContext.Employees.Find(emailDTO.IdFromEmployee);

                cc.Add(new KeyValuePair<string, string>(employeeFrom.Email, employeeFrom.FirstName + " " + employeeFrom.LastName));

                RFPEmailHistory rFPEmailHistory = new RFPEmailHistory();
                rFPEmailHistory.IdRfp = rfp.Id;
                rFPEmailHistory.IdFromEmployee = emailDTO.IdFromEmployee;
                rFPEmailHistory.IdToCompany = emailDTO.IdContactsTo;
                rFPEmailHistory.IdContactAttention = emailDTO.IdContactAttention;
                rFPEmailHistory.IdTransmissionType = emailDTO.IdTransmissionType;
                rFPEmailHistory.IdEmailType = emailDTO.IdEmailType;
                rFPEmailHistory.SentDate = DateTime.UtcNow;
                rFPEmailHistory.EmailSubject = emailDTO.Subject;
                if (employee != null)
                {
                    rFPEmailHistory.IdSentBy = employee.Id;
                }
                rFPEmailHistory.EmailMessage = emailDTO.Body;
                rFPEmailHistory.IsAdditionalAtttachment = emailDTO.IsAdditionalAtttachment;
                //commented by M.B
                //  rFPEmailHistory.IsEmailSent = true;

                rpoContext.RFPEmailHistories.Add(rFPEmailHistory);
                rpoContext.SaveChanges();

                foreach (int idCc in emailDTO.IdContactsCc)
                {
                    RFPEmailCCHistory rFPEmailCCHistory = new RFPEmailCCHistory();
                    rFPEmailCCHistory.IdContact = idCc;
                    rFPEmailCCHistory.IdRFPEmailHistory = rFPEmailHistory.Id;

                    rpoContext.RFPEmailCCHistories.Add(rFPEmailCCHistory);
                    rpoContext.SaveChanges();
                }
                //string attachmentPath = string.Empty;
                if (emailDTO.IsProposalPDFAttached)
                {
                    string fullpath = HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.ProposalFileExportPath;
                    //string pdffilename = "RFP-Pdf_" + id + ".pdf";

                    string pdffilename = "RFP #" + rfpForPdf.RfpNumber + " " + rfpForPdf.RfpAddress.HouseNumber + " " + rfpForPdf.RfpAddress.Street + " "
                    + (rfpForPdf.RfpAddress.Borough != null ? rfpForPdf.RfpAddress.Borough.Description : string.Empty) + (!string.IsNullOrEmpty(rfpForPdf.SpecialPlace) ? " " + rfpForPdf.SpecialPlace : string.Empty) + "_" +
                    (rfpForPdf.Company != null ? rfpForPdf.Company.Name : "Individual") + (rfpForPdf.Contact != null ? "_" + rfpForPdf.Contact.FirstName + (!string.IsNullOrEmpty(rfpForPdf.Contact.LastName) ? " " + rfpForPdf.Contact.LastName : string.Empty) : string.Empty)
                    + ".pdf";
                    //filename = filename.R(filename, @"<[^>]+>|&nbsp;", "");
                    pdffilename = Regex.Replace(pdffilename, @"[^0-9a-zA-Z_.]+", "-");

                    attachmentPath = fullpath + pdffilename;

                    RFPEmailAttachmentHistory rFPEmailAttachment = new RFPEmailAttachmentHistory();
                    rFPEmailAttachment.DocumentPath = pdffilename;
                    rFPEmailAttachment.IdRFPEmailHistory = rFPEmailHistory.Id;
                    rFPEmailAttachment.Name = pdffilename;
                    rpoContext.RFPEmailAttachmentHistories.Add(rFPEmailAttachment);
                    rpoContext.SaveChanges();

                    var path = HttpRuntime.AppDomainAppPath;
                    string pdfdirectoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.RFPAttachmentsPath));

                    string pdfdirectoryFileName = Convert.ToString(rFPEmailAttachment.Id) + "_" + pdffilename;
                    string newfilename = System.IO.Path.Combine(pdfdirectoryName, pdfdirectoryFileName);

                    if (!Directory.Exists(pdfdirectoryName))
                    {
                        Directory.CreateDirectory(pdfdirectoryName);
                    }

                    if (File.Exists(newfilename))
                    {
                        File.Delete(newfilename);
                    }

                    if (File.Exists(attachmentPath))
                    {

                        File.Copy(attachmentPath, newfilename);
                    }

                    if (File.Exists(newfilename) && !attachments.Contains(pdffilename))
                    {
                        attachments.Add(newfilename);
                    }

                }
                else
                {
                    List<RFPEmailAttachmentHistory> rFPEmailAttachmentHistory = rpoContext.RFPEmailAttachmentHistories.Where(x => x.IdRFPEmailHistory == rFPEmailHistory.Id).ToList();
                    if (rFPEmailAttachmentHistory != null && rFPEmailAttachmentHistory.Count > 0)
                    {
                        foreach (var item in rFPEmailAttachmentHistory)
                        {
                            var path = HttpRuntime.AppDomainAppPath;
                            string pdfdirectoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.RFPAttachmentsPath));

                            string pdfdirectoryFileName = Convert.ToString(item.Id) + "_" + item.DocumentPath;
                            string newfilename = System.IO.Path.Combine(pdfdirectoryName, pdfdirectoryFileName);

                            if (File.Exists(newfilename) && !attachments.Contains(pdfdirectoryFileName))
                            {
                                attachments.Add(newfilename);
                            }
                        }
                    }
                }

                responseIdRfp = rfp.Id;
                responseIdRFPEmailHistory = rFPEmailHistory.Id;
                //commented by M.B
                //    rFPEmailHistory.IsEmailSent = true;
                rpoContext.SaveChanges();



                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/MasterEmailTemplate.htm")))
                {
                    body = reader.ReadToEnd();
                }

                TransmissionType transmissionType = rpoContext.TransmissionTypes.FirstOrDefault(x => x.Id == rFPEmailHistory.IdTransmissionType);
                if (transmissionType != null && transmissionType.IsSendEmail)
                {
                    string emailBody = body;
                    emailBody = emailBody.Replace("##EmailBody##", emailDTO.Body);

                    //List<TransmissionTypeDefaultCC> transmissionTypeDefaultCC = rpoContext.TransmissionTypeDefaultCCs.Include("Employee").Where(x => x.IdTransmissionType == emailDTO.IdTransmissionType).ToList();
                    List<TransmissionTypeDefaultCC> transmissionTypeDefaultCC = rpoContext.TransmissionTypeDefaultCCs.Include("Employee").Where(x => x.IdEamilType == emailDTO.IdEmailType).ToList();
                    if (transmissionTypeDefaultCC != null && transmissionTypeDefaultCC.Any())
                    {
                        foreach (TransmissionTypeDefaultCC item in transmissionTypeDefaultCC)
                        {
                            cc.Add(new KeyValuePair<string, string>(item.Employee.Email, item.Employee.FirstName + " " + item.Employee.LastName));
                        }
                    }

                    if (File.Exists(attachmentPath))
                    {
                        Mail.Send(
                        new KeyValuePair<string, string>(employeeFrom.Email, employeeFrom.FirstName + " " + employeeFrom.LastName),
                        to,
                        cc,
                        rFPEmailHistory.EmailSubject,
                        emailBody,
                        attachments
                    );
                        //added by M.B
                        rFPEmailHistory.IsEmailSent = true;
                        rpoContext.SaveChanges();


                    }
                    else
                    { throw new RpoBusinessException("There was a problem while attaching the Proposal PDF. Kindly recreate the proposal."); }

                }

            }

            return Ok(new { Message = "Mail sent successfully", idRfp = responseIdRfp.ToString(), idRFPEmailHistory = responseIdRFPEmailHistory.ToString() });
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
        /// Downloads the PDF.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        [Route("api/Rfps/{id}/Download")]
        [ResponseType(typeof(string))]
        [HttpGet]
        public IHttpActionResult DownloadPdf(int id)
        {
            //string filename = "RFP-Pdf_" + id + ".pdf";
            string directoryName = HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.ProposalFileExportPath;

            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            var rfp = rpoContext.Rfps
                .Include("Borough")
                .Include("RfpAddress.Borough")
                .Include("Company")
                .Include("Contact")
                .Include("ProjectDetails")
                .Include("ScopeReview")
                .Include("ProposalReview.Verbiages").FirstOrDefault(x => x.Id == id);

            if (rfp == null)
            {
                return this.NotFound();
            }

            string filename = this.CreateProposalPdf(rfp);
            FileInfo fileinfo = new FileInfo(HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.ProposalFileExportPath + "/" + filename);
            long fileinfoSize = fileinfo.Length;

            var downloadFilePath = Properties.Settings.Default.APIUrl + Properties.Settings.Default.ProposalFileExportPath + "/" + filename;
            //var myObj = "{\"pdfFilePath\": \"" + downloadFilePath + "\",\"pdfFilesize\": \"" + fileinfoSize + "\"}";
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            result.Add(new KeyValuePair<string, string>("pdfFilePath", downloadFilePath));
            result.Add(new KeyValuePair<string, string>("pdfFilesize", Convert.ToString(fileinfoSize)));

            return Ok(result);
        }

        public static String CSS = "p { font-family: 'Avenir Next LT Pro', color:'red', font-size:'20px',list-style:'disc'} li { font-family: 'Times New Roman'}";

        /// <summary>
        /// Creates the job transmittal PDF.
        /// </summary>
        /// <param name="idRfp">The identifier RFP.</param>
        private string CreateProposalPdf(Rfp rfp)
        {
            string filename = "RFP #" + rfp.RfpNumber + " " + rfp.RfpAddress.HouseNumber + " " + rfp.RfpAddress.Street + " "
                    + (rfp.RfpAddress.Borough != null ? rfp.RfpAddress.Borough.Description : string.Empty) + (!string.IsNullOrEmpty(rfp.SpecialPlace) ? " " + rfp.SpecialPlace : string.Empty) + "_" +
                    (rfp.Company != null ? rfp.Company.Name : "Individual") + (rfp.Contact != null ? "_" + rfp.Contact.FirstName + (!string.IsNullOrEmpty(rfp.Contact.LastName) ? " " + rfp.Contact.LastName : string.Empty) : string.Empty)
                    + ".pdf";

            //string filename = "RFP #" + rfp.RfpNumber + " " + rfp.RfpAddress.HouseNumber + " " + rfp.RfpAddress.Street + " "
            //        + (rfp.RfpAddress.Borough != null ? rfp.RfpAddress.Borough.Description : string.Empty) + "_" +
            //        (rfp.Company != null ? rfp.Company.Name : "Individual") + ".pdf";


            //filename = filename.R(filename, @"<[^>]+>|&nbsp;", "");
            filename = Regex.Replace(filename, @"[^0-9a-zA-Z_.]+", "-");


            using (MemoryStream stream = new MemoryStream())
            {
                //string filename = "RFP-Pdf_" + rfp.Id + ".pdf";

                //RFP #115 1530 Broadway Old Navy | Chipman Design Architecture / Mark Scheerhorn


                string companyName = rfp != null && rfp.Company != null && rfp.Company.Name != null ? rfp.Company.Name : string.Empty;
                int idJob = rpoContext.Jobs.Where(x => x.IdRfpAddress == rfp.IdRfpAddress).Select(x => x.Id).FirstOrDefault();
                string companyaddress1 = rfp.Address1 != null ? rfp.Address1 : string.Empty;
                string companyaddress2 = rfp.Address2 != null ? rfp.Address2 : string.Empty;
                string companycity = rfp.City != null ? rfp.City : string.Empty;
                string companystate = rfp.State != null ? rfp.State.Acronym : string.Empty;
                string companyzipCode = rfp.ZipCode != null ? rfp.ZipCode : string.Empty;

                string address_Street = rfp.RfpAddress != null && rfp.RfpAddress.Street != null ? rfp.RfpAddress.Street : string.Empty;
                string address_HouseNumber = rfp.RfpAddress != null && rfp.RfpAddress.HouseNumber != null ? rfp.RfpAddress.HouseNumber : string.Empty;
                string address_Borough = rfp.RfpAddress != null && rfp.RfpAddress.Borough != null ? rfp.RfpAddress.Borough.Description : string.Empty;
                string address_Zipcode = rfp.RfpAddress != null && rfp.RfpAddress.ZipCode != null ? rfp.RfpAddress.ZipCode : string.Empty;
                string specialPlaceName = rfp.SpecialPlace;
                string rfpNumber = rfp.RfpNumber;
                string subject = string.Empty;
                string subjectHeader = string.Empty;
                string address = string.Empty;
                string headeAddress = string.Empty;
                string idSignature = rfp.IdSignature != null ? rfp.IdSignature : string.Empty;
                //if (!string.IsNullOrEmpty(companyName) || !string.IsNullOrEmpty(address_HouseNumber) || !string.IsNullOrEmpty(address_Street))
                //{
                //    subject = "Proposal for Consulting Services ";
                //    address = (!string.IsNullOrEmpty(subject) ? (" - " + address_HouseNumber + " " + address_Street + " " + address_Borough + " " + address_Zipcode) : (address_HouseNumber + " " + address_Street + " " + address_Borough + " " + address_Zipcode));
                //    headeAddress = (!string.IsNullOrEmpty(subject) ? (address_HouseNumber + " " + address_Street + " " + address_Borough) : (address_HouseNumber + " " + address_Street + " " + address_Borough + " " + address_Zipcode));
                //    subject = subject + address;
                //}


                //if (rfp.ProposalReview != null && rfp.ProposalReview.Count() > 0)
                //{
                //    foreach (RfpProposalReview item in rfp.ProposalReview.OrderBy(x => x.DisplayOrder).Where(x => x.Verbiages.Name == "subject"))
                //    {
                //        if (item.Verbiages != null)
                //        {
                //            PdfPTable tableContent = new PdfPTable(1);
                //            tableContent.WidthPercentage = 100;
                //            //string content = item.Content.Replace("##FirstMilestoneAmount##", firstMilestoneAmount.ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "$ "));
                //            string content = "Proposal for Consulting Services " + item.Content;
                //            subject = content;
                //            tableContent = AddUL(content);
                //            tableContent.SplitLate = false;
                //            //document.Add(tableContent);
                //            //document.Add(new Paragraph(Chunk.NEWLINE));
                //            //table.AddCell(cell);
                //        }
                //    }
                //}

                // RfpProposalReview rfpcontact = rfp.ProposalReview.OrderBy(x => x.DisplayOrder).Where(x => x.Verbiages.Name == "Header").FirstOrDefault();
                string contactAttentionHeader = string.Empty;
                //string contactAttention = rfp.Contact != null ? (rfp.Contact.Prefix != null ? rfp.Contact.Prefix.Name : string.Empty) + " " + rfp.Contact.FirstName + (rfp.Contact.LastName != null ? " " + rfp.Contact.LastName : string.Empty) + " " + (rfp.Contact.Suffix != null ? rfp.Contact.Suffix.Description : string.Empty) : string.Empty;

                string attentionName = rfp.Contact != null ? (rfp.Contact != null && !string.IsNullOrEmpty(rfp.Contact.LastName)
                                                       ? (rfp.Contact.Prefix != null ? rfp.Contact.Prefix.Name + " " : string.Empty)
                                                       + (rfp.Contact.Prefix != null ? rfp.Contact.LastName : rfp.Contact.FirstName + (!string.IsNullOrEmpty(rfp.Contact.LastName) ? " " + rfp.Contact.LastName : string.Empty)) : rfp.Contact.FirstName) : string.Empty;
               
                string fontRegular = HttpContext.Current.Server.MapPath("~\\HTML\\fonts\\AvenirNextLTPro-Regular.ttf");
                string fontBold = HttpContext.Current.Server.MapPath("~\\HTML\\fonts\\Avenir Next LT Pro Bold.ttf");
                BaseFont customfontRegualr = BaseFont.CreateFont(fontRegular, BaseFont.CP1252, BaseFont.EMBEDDED);
                BaseFont customfontBold = BaseFont.CreateFont(fontBold, BaseFont.CP1252, BaseFont.EMBEDDED);
                Font font__Bold_16 = new Font(customfontBold, 16,1);
                Font font_Bold_12 = new Font(customfontRegualr, 11, 1);
                Font font_Reguar_12 = new Font(customfontRegualr, 11, 0);

                Document document = new Document(PageSize.LETTER);
                document.SetMargins(54, 54, 36, 48);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.ProposalFileExportPath + "/" + filename, FileMode.Create));

                if (rfp.ProposalReview != null && rfp.ProposalReview.Count() > 0)
                {
                    foreach (RfpProposalReview item in rfp.ProposalReview.OrderBy(x => x.DisplayOrder).Where(x => x.Verbiages.Name == "subject"))
                    {
                        if (item.Verbiages != null)
                        {
                            string content = item.Content;
                            subject = content;
                        }
                    }

                    foreach (RfpProposalReview item in rfp.ProposalReview.OrderBy(x => x.DisplayOrder).Where(x => x.Verbiages.Name == "Header"))
                    {
                        if (item.Verbiages != null)
                        {
                            string content = item.Content;
                            //contactAttentionHeader = content.Replace("<p>","").Replace("</p>","").Replace("&nbsp;", "").Replace("&amp;", "&").Trim();
                            contactAttentionHeader = content.Trim();
                        }
                    }
                }
                //string subjectRepeatForNewPage = Regex.Replace(subject, @"<[^>]*>", String.Empty).Replace("\n", "").Replace("RE : Proposal for Consulting Services - ", "").Substring(39);

                if (!string.IsNullOrEmpty(companyName) || !string.IsNullOrEmpty(address_HouseNumber) || !string.IsNullOrEmpty(address_Street))
                {
                    address = (!string.IsNullOrEmpty(subjectHeader) ? (address_HouseNumber + " " + address_Street + " " + address_Borough + " " + address_Zipcode) : (address_HouseNumber + " " + address_Street + " " + address_Borough));
                    headeAddress = (!string.IsNullOrEmpty(subjectHeader) ? (address_HouseNumber + " " + address_Street + " " + address_Borough) : (address_HouseNumber + " " + address_Street + " " + address_Borough));
                    subjectHeader = address;
                }

                string subjectRepeatForNewPage = Regex.Replace(subject, @"<[^>]*>", String.Empty).Replace("\n", "").Replace("&nbsp;", "").Substring(39);
                subjectRepeatForNewPage = subjectHeader;

                string Address = string.Empty;
                string Address2 = string.Empty;

                if (!string.IsNullOrEmpty(companyaddress1))
                {
                    Address2 = companyaddress1;
                }
                if (string.IsNullOrEmpty(companyaddress2))
                {

                    Address2 = companyaddress2 + Environment.NewLine + companycity + ", " + companystate;
                }
                if (!string.IsNullOrEmpty(companyaddress2))
                {
                    Address2 = Address2 + Environment.NewLine + companyaddress2 + ", " + companycity + ", " + companystate;
                }

                contactAttentionHeader = Regex.Replace(contactAttentionHeader, @"<(.|n)*?>", string.Empty).Replace("&nbsp;", "").Replace("&amp;", "&").Replace("\n\n", "\n");
                //subjectHeader = string.IsNullOrEmpty(specialPlaceName) ? subjectHeader : subjectHeader + " | " + specialPlaceName;
                writer.PageEvent = new Header(contactAttentionHeader.Trim(), "Proposal for Consulting Services " + Environment.NewLine + subjectHeader + Environment.NewLine + specialPlaceName, Environment.NewLine, idJob, Address2);
                document.Open();

                Image logo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~\\HTML\\images\\RPO-Logo-pdf.png"));
                logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                logo.ScaleToFit(100, 80);
                logo.SetAbsolutePosition(260, 760);

                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;

                PdfPCell cell = new PdfPCell(logo);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Rowspan = 2;
                cell.PaddingBottom = 5;
                table.AddCell(cell);

                string reportHeader = //"RPO INCORPORATED" + Environment.NewLine +
                "146 West 29th Street, Suite 2E"
                + Environment.NewLine + "New York, NY 10001"
                + Environment.NewLine + "(212) 566-5110"
                + Environment.NewLine + "www.rpoinc.com";

                string reportTitle = "RPO, Inc." +Environment.NewLine + "Construction Consultants";

                cell = new PdfPCell(new Phrase(reportTitle, font__Bold_16));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -30;
                cell.Colspan = 5;
                cell.PaddingTop = -2;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportHeader, font_Bold_12));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -30;
                cell.Colspan = 5;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.Border = PdfPCell.BOTTOM_BORDER;
                cell.Colspan = 5;
                cell.PaddingTop = -15;
                table.AddCell(cell);

                document.Add(table);

                table = new PdfPTable(3);
                table.WidthPercentage = 100;

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                DateTime sentDate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(DateTime.UtcNow), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 3;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(sentDate.ToString("MMMM dd, yyyy"), font_Reguar_12));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 3;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 3;
                table.AddCell(cell);

                document.Add(table);
                table = new PdfPTable(2);
                table.WidthPercentage = 100;
                if (rfp.ProposalReview != null && rfp.ProposalReview.Count() > 0)
                {
                    foreach (RfpProposalReview item in rfp.ProposalReview.OrderBy(x => x.DisplayOrder).Where(x => x.Verbiages.Name == "addressee"))
                    {
                        if (item.Verbiages != null)
                        {
                            PdfPTable tableContent = new PdfPTable(1);
                            tableContent.WidthPercentage = 100;
                            //string content = item.Content.Replace("##FirstMilestoneAmount##", firstMilestoneAmount.ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "$ "));
                            string content = item.Content;

                            tableContent = AddUL(content);
                            tableContent.SplitLate = false;
                            document.Add(tableContent);
                            document.Add(new Paragraph(Chunk.NEWLINE));
                        }
                    }
                }
                //cell.Border = PdfPCell.BODY;
                document.Add(table);
                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 3;
                table.AddCell(cell);

                table = new PdfPTable(2);
                table.WidthPercentage = 100;
                if (rfp.ProposalReview != null && rfp.ProposalReview.Count() > 0)
                {
                    foreach (RfpProposalReview item in rfp.ProposalReview.OrderBy(x => x.DisplayOrder).Where(x => x.Verbiages.Name == "subject"))
                    {
                        if (item.Verbiages != null)
                        {
                            Font font = font_Bold_12;
                            PdfPTable tableContent = new PdfPTable(1);
                            tableContent.WidthPercentage = 100;
                            string content = item.Content;
                            subject = content;
                            tableContent = AddUL(content);
                            tableContent.SplitLate = false;
                            document.Add(tableContent);
                        }
                    }
                }
                //cell.Border = PdfPCell.BODY;
                //document.Add(table);

                table = new PdfPTable(2);
                table.WidthPercentage = 100;
                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 3;
                table.AddCell(cell);

                //cell = new PdfPCell(new Phrase("Dear " + attentionName + ",", new Font(Font.FontFamily.TIMES_ROMAN, 12)));
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.Border = PdfPCell.NO_BORDER;
                //cell.Colspan = 3;
                //table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 3;
                table.AddCell(cell);

                document.Add(table);

                double firstMilestoneAmount = 0;
                if (rfp.Milestones != null && rfp.Milestones.Count > 0)
                {
                    firstMilestoneAmount = rfp.Milestones.OrderBy(x => x.Id).Select(x => x.Value).FirstOrDefault();
                }

                if (rfp.ProposalReview != null && rfp.ProposalReview.Count() > 0)
                {
                    foreach (RfpProposalReview item in rfp.ProposalReview.OrderBy(x => x.DisplayOrder).Where(x => x.Verbiages.Name != "addressee" && x.Verbiages.Name != "subject"))
                    {
                        if (item.Verbiages != null)
                        {
                            if (item.Verbiages.Name.ToLower() == "cost")
                            {
                                table = new PdfPTable(1);
                                table.WidthPercentage = 100;

                                cell = new PdfPCell(new Phrase("Total cost for service associated with the scope listed above is: " + rfp.Cost.ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "$ "), font_Bold_12));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.NO_BORDER;
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                cell.Border = PdfPCell.NO_BORDER;
                                table.AddCell(cell);

                                document.Add(table);
                            }
                            else if (item.Verbiages.Name.ToLower() == "milestone")
                            {
                                if (rfp.Milestones != null)
                                {                                   
                                    document.Add(new Paragraph("Payments for service are required in accordance with the following schedule: ", font_Bold_12));
                                    PdfPTable milestoneTable = new PdfPTable(4);

                                    BaseFont baseFont = font_Bold_12.GetCalculatedBaseFont(true);
                                    float width = baseFont.GetWidthPoint("$", 12);
                                    float amountWidth = 0;

                                    foreach (Milestone milestone in rfp.Milestones.OrderBy(x => x.Id))
                                    {
                                        string milestoneAmount = milestone.Value.ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "");
                                        float tempwidth = baseFont.GetWidthPoint(milestoneAmount, 12);

                                        if (tempwidth > amountWidth)
                                        {
                                            amountWidth = tempwidth;
                                        }
                                    }

                                    //float[] widths = { fixedWidth + padding, userdefinedWidth + padding};
                                    //table.setTotalWidth(widths);
                                    //table.setLockedWidth(true);
                                    milestoneTable.WidthPercentage = 100;
                                    //float totalWidth = milestoneTable.TotalWidth;
                                    iTextSharp.text.Rectangle rect = PageSize.LETTER;
                                    float pageWidth = rect.Width;
                                    float padding = 15f;
                                    float paddingAmount = 55f;
                                    milestoneTable.SetWidths(new float[] { 30f, width + padding, amountWidth + paddingAmount, pageWidth - (amountWidth + padding + width + paddingAmount) });


                                    foreach (Milestone milestone in rfp.Milestones.OrderBy(x => x.Id))
                                    {
                                        //Paragraph paragraph = new Paragraph(milestone.Value.ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "$ ") + " - " + milestone.Name, new Font(Font.FontFamily.TIMES_ROMAN, 12));
                                        //paragraph.IndentationLeft = 35f;
                                        //document.Add(paragraph);

                                        PdfPCell milestoneCell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                                        milestoneCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //milestoneCell.Width = 35f;
                                        milestoneCell.Border = PdfPCell.NO_BORDER;
                                        milestoneTable.AddCell(milestoneCell);

                                        milestoneCell = new PdfPCell(new Phrase("$", font_Reguar_12));
                                        milestoneCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        milestoneCell.Border = PdfPCell.NO_BORDER;
                                        milestoneTable.AddCell(milestoneCell);

                                        milestoneCell = new PdfPCell(new Phrase(milestone.Value.ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", ""), font_Reguar_12));
                                        milestoneCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        milestoneCell.PaddingRight = 10f;
                                        milestoneCell.Border = PdfPCell.NO_BORDER;
                                        milestoneTable.AddCell(milestoneCell);

                                        milestoneCell = new PdfPCell(new Phrase(milestone.Name, font_Reguar_12));
                                        milestoneCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        milestoneCell.Border = PdfPCell.NO_BORDER;
                                        milestoneTable.AddCell(milestoneCell);

                                    }
                                    document.Add(new Paragraph(Chunk.NEWLINE));
                                    document.Add(milestoneTable);
                                    document.Add(new Paragraph(Chunk.NEWLINE));
                                }
                            }
                            else if (item.Verbiages.VerbiageType == VerbiageType.Sign)
                            {
                                if (rfp.IsSignatureNewPage != null && rfp.IsSignatureNewPage == true)
                                {
                                    document.NewPage();
                                }

                                PdfPTable tableSign = new PdfPTable(1);
                                tableSign.WidthPercentage = 100;
                                string content = item.Content.Replace("##FirstMilestoneAmount##", firstMilestoneAmount.ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "$ "));

                                tableSign = AddUL(content);
                                document.Add(tableSign);
                                document.Add(new Paragraph(Chunk.NEWLINE));

                            }
                            else if (item.Verbiages.VerbiageType == VerbiageType.Header)
                            {
                            }
                            else
                            {
                                PdfPTable tableContent = new PdfPTable(1);
                                tableContent.WidthPercentage = 100;
                                string content = item.Content.Replace("##FirstMilestoneAmount##", firstMilestoneAmount.ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "$ "));

                                tableContent = AddUL(content);
                                tableContent.SplitLate = false;
                                document.Add(tableContent);
                                document.Add(new Paragraph(Chunk.NEWLINE));


                                //PdfPCell contentCell = new PdfPCell(new Phrase(content));
                                //contentCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                //contentCell.Border = PdfPCell.NO_BORDER;
                                //tableContent.AddCell(contentCell);

                                //document.Add(tableContent);
                                //document.Add(new Paragraph(Chunk.NEWLINE));

                                //using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(CSS)))
                                //{
                                //    using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content)))
                                //    {

                                //        //Parse the HTML
                                //        iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, msHtml, msCss);
                                //    }
                                //}

                                //using (StringReader sr = new StringReader(content))
                                //{
                                //    //Pass our StringReader into iTextSharp's HTML parser, get back a list of iTextSharp elements
                                //    List<IElement> ies = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(sr, null);

                                //    //Loop through each element and add to the document
                                //    foreach (IElement ie in ies)
                                //    {
                                //        document.Add(ie);
                                //    }
                                //}

                            }
                        }
                    }
                }

                table = new PdfPTable(3);
                table.WidthPercentage = 100;

                cell = new PdfPCell(new Paragraph("Very truly yours,", font_Reguar_12));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 3;
                cell.PaddingTop = -15;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 3;
                table.AddCell(cell);

                if (idSignature == "18")
                {
                    Image logosignature = Image.GetInstance(HttpContext.Current.Server.MapPath("~\\HTML\\images\\Robot_1_177x68.JPG"));
                    logosignature.Alignment = Image.ALIGN_CENTER;
                    logosignature.ScaleToFit(120, 60);
                    logosignature.SetAbsolutePosition(260, 760);

                    cell = new PdfPCell(logosignature);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 3;
                    table.AddCell(cell);

                    //cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.Border = PdfPCell.NO_BORDER;
                    //cell.Colspan = 3;
                    //table.AddCell(cell);

                    cell = new PdfPCell(new Paragraph("Robert Anic", font_Reguar_12));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 3;
                    table.AddCell(cell);
                }
                else if (idSignature == "19")
                {
                    Image logosignature = Image.GetInstance(HttpContext.Current.Server.MapPath("~\\HTML\\images\\kd-sign.jpg"));
                    logosignature.Alignment = Image.ALIGN_CENTER;
                    logosignature.ScaleToFit(120, 60);
                    logosignature.SetAbsolutePosition(260, 760);

                    cell = new PdfPCell(logosignature);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 3;
                    table.AddCell(cell);

                    //cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.Border = PdfPCell.NO_BORDER;
                    //cell.Colspan = 3;
                    //table.AddCell(cell);

                    cell = new PdfPCell(new Paragraph("Kevin Danielson", font_Reguar_12));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 3;
                    table.AddCell(cell);
                }
                else
                {
                    Image logosignature = Image.GetInstance(HttpContext.Current.Server.MapPath("~\\HTML\\images\\Signature-MGP_bBya.JPG"));
                    logosignature.Alignment = Image.ALIGN_CENTER;
                    logosignature.ScaleToFit(120, 60);
                    logosignature.SetAbsolutePosition(260, 760);

                    cell = new PdfPCell(logosignature);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 3;
                    table.AddCell(cell);

                    //cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.Border = PdfPCell.NO_BORDER;
                    //cell.Colspan = 3;
                    //table.AddCell(cell);

                    cell = new PdfPCell(new Paragraph("Michael G. Pressel", font_Reguar_12));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 3;
                    table.AddCell(cell);
                }
                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 3;
                table.AddCell(cell);

                cell = new PdfPCell(new Paragraph("Accepted by:", font_Reguar_12));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 3;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 3;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 3;
                table.AddCell(cell);
                document.Add(table);

                table = new PdfPTable(5);
                table.SetWidths(new float[] { 30, 10, 30, 10, 20 });
                table.WidthPercentage = 100;

                cell = new PdfPCell(new Paragraph("", font_Reguar_12));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.PaddingBottom = 10;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Paragraph("", font_Reguar_12));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.PaddingBottom = 10;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Paragraph("", font_Reguar_12));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.PaddingBottom = 10;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Paragraph("", font_Reguar_12));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.PaddingBottom = 10;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Paragraph("", font_Reguar_12));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.PaddingBottom = 10;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Paragraph("Print Name", font_Reguar_12));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.TOP_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Paragraph("", font_Reguar_12));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Paragraph("Signature", font_Reguar_12));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.TOP_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Paragraph("", font_Reguar_12));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Paragraph("Date", font_Reguar_12));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.TOP_BORDER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);

                cell = new PdfPCell(new Paragraph("RFP #" + rfpNumber, font_Reguar_12));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);               

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 4;
                table.AddCell(cell);

                document.Add(table);
                document.Close();

                writer.Close();
            }

            return filename;
        }

        //private void AddUL(string content, ref PdfPTable tableContent, ref Document document)
        //{
        //    List<IElement> elements = XMLWorkerHelper.ParseToElementList(content, CSS);
        //    PdfPCell cellContent = new PdfPCell();
        //    foreach (IElement e in elements)
        //    {
        //        if (e is Paragraph)
        //        {
        //            ((Paragraph)e).Font = new Font(Font.FontFamily.TIMES_ROMAN, 12);
        //        }

        //        if (e is ListItem)
        //        {
        //            ((ListItem)e).Font = new Font(Font.FontFamily.TIMES_ROMAN, 12);
        //        }

        //        if (e is List)
        //        {
        //            List abc = (List)e;
        //            List list = new List(List.UNORDERED, 10f);
        //            Chunk bullet = new Chunk("\u2022");
        //            list.IndentationLeft = 15f;
        //            foreach (var dataItem in abc.Items)
        //            {
        //                string liContent = ((ListItem)dataItem).Content;
        //                Font font = new Font(Font.FontFamily.TIMES_ROMAN, 12);
        //                ListItem listItem = new ListItem(liContent, font);
        //                listItem.ListSymbol = bullet;
        //                list.Add(listItem);

        //            }
        //            cellContent.AddElement((IElement)list);
        //        }

        //        if (e is List) { }
        //        else
        //        {
        //            cellContent.AddElement((IElement)e);
        //        }
        //    }
        //    cellContent.HorizontalAlignment = Element.ALIGN_LEFT;
        //    cellContent.Border = PdfPCell.NO_BORDER;
        //    tableContent.AddCell(cellContent);
        //    document.Add(tableContent);
        //    document.Add(new Paragraph(Chunk.NEWLINE));
        //}

        private PdfPTable AddUL(string content)
        {
            string fontRegular = HttpContext.Current.Server.MapPath("~\\HTML\\fonts\\AvenirNextLTPro-Regular.ttf");
            BaseFont customfontRegualr = BaseFont.CreateFont(fontRegular, BaseFont.CP1252, BaseFont.EMBEDDED);            
            Font font_Reguar_12 = new Font(customfontRegualr, 11, 0);

            PdfPTable tableContent = new PdfPTable(1);
            tableContent.WidthPercentage = 100;
            List<IElement> elements = XMLWorkerHelper.ParseToElementList(content, CSS);
            PdfPCell cellContent = new PdfPCell();
            foreach (IElement e in elements)
            {
                if (e is Paragraph)
                {
                    ((Paragraph)e).Font = font_Reguar_12;
                    ((Paragraph)e).SetLeading(0.0f, 1.0f);
                }

                if (e is ListItem)
                {
                    ((ListItem)e).Font = font_Reguar_12;
                    ((ListItem)e).SetLeading(0.0f, 1.0f);
                }

                if (e is List)
                {
                    List abc = (List)e;
                    List list = new List(List.UNORDERED, 10f);
                    Chunk bullet = new Chunk("\u2022");
                    list.IndentationLeft = 15f;
                    foreach (var dataItem in abc.Items)
                    {
                        bool isNestedList = false;
                        if (((ListItem)dataItem) != null && ((ListItem)dataItem).Count > 1)
                        {
                            for (int i = 0; i < ((ListItem)dataItem).Count; i++)
                            {
                                if (((ListItem)dataItem)[i].Type == 14)
                                {
                                    isNestedList = true;

                                    string liContent_sub = ((ListItem)dataItem)[0].ToString();
                                    if (liContent_sub != "iTextSharp.text.Paragraph")
                                    {
                                        //Font font_sub = new Font(Font.FontFamily.TIMES_ROMAN, 12);
                                        ListItem listItem_sub = new ListItem(liContent_sub, font_Reguar_12);
                                        listItem_sub.ListSymbol = bullet;
                                        listItem_sub.SetLeading(0.0f, 1.0f);
                                        list.Add(listItem_sub);
                                    }

                                }
                            }
                        }

                        if (isNestedList)
                        {
                            for (int i = 0; i < ((ListItem)dataItem).Count; i++)
                            {
                                if (((ListItem)dataItem)[i] is Paragraph)
                                {
                                    ((Paragraph)((ListItem)dataItem)[i]).Font = font_Reguar_12;
                                    string liContent = ((Paragraph)((ListItem)dataItem)[i]).Content;
                                    //Font font = new Font(Font.FontFamily.TIMES_ROMAN, 12);
                                    ListItem listItem = new ListItem(liContent, font_Reguar_12);
                                    listItem.ListSymbol = bullet;
                                    if (liContent == " ")
                                    {
                                        listItem.ListSymbol = new Chunk("");
                                    }
                                    else
                                    {
                                        listItem.ListSymbol = bullet;
                                    }
                                    listItem.SetLeading(0.0f, 1.0f);
                                    list.Add(listItem);
                                }

                                if (((ListItem)dataItem)[i] is ListItem)
                                {
                                    ((ListItem)((ListItem)dataItem)[i]).Font = font_Reguar_12;
                                    string liContent = ((ListItem)((ListItem)dataItem)[i]).Content;
                                    //Font font = new Font(Font.FontFamily.TIMES_ROMAN, 12);
                                    ListItem listItem = new ListItem(liContent, font_Reguar_12);
                                    listItem.ListSymbol = bullet;
                                    listItem.SetLeading(0.0f, 1.0f);
                                    list.Add(listItem);
                                }

                                if (((ListItem)dataItem)[i] is List)
                                {
                                    List abc_sub = (List)((ListItem)dataItem)[i];
                                    List list_sub = new List(List.UNORDERED, 10f);
                                    Chunk bullet_sub = new Chunk("\u2022");
                                    list_sub.IndentationLeft = 15f;
                                    foreach (var item in abc_sub.Items)
                                    {
                                        string liContent_sub = ((ListItem)item).Content;
                                        //Font font_sub = new Font(Font.FontFamily.TIMES_ROMAN, 12);
                                        ListItem listItem_sub = new ListItem(liContent_sub, font_Reguar_12);
                                        listItem_sub.ListSymbol = bullet_sub;
                                        listItem_sub.SetLeading(0.0f, 1.0f);
                                        list_sub.Add(listItem_sub);
                                    }
                                    list.Add(list_sub);
                                }
                                //if (((ListItem)dataItem)[i].IsContent==false)
                                //{

                                //}
                            }
                        }
                        else
                        {
                            string liContent = ((ListItem)dataItem).Content;
                            //Font font = new Font(Font.FontFamily.TIMES_ROMAN, 12);
                            ListItem listItem = new ListItem(liContent, font_Reguar_12);
                            listItem.ListSymbol = bullet;
                            listItem.SetLeading(0.0f, 1.0f);
                            list.Add(listItem);
                        }
                    }
                    cellContent.AddElement((IElement)list);
                }

                if (e is List) { }
                else
                {
                    cellContent.AddElement((IElement)e);
                }
            }
            cellContent.HorizontalAlignment = Element.ALIGN_LEFT;
            cellContent.Border = PdfPCell.NO_BORDER;
            tableContent.AddCell(cellContent);
            //document.Add(tableContent);
            //document.Add(new Paragraph(Chunk.NEWLINE));
            return tableContent;
        }


        /// <summary>
        /// Parameter 1 : idRfp (int)
        /// Parameter 2 : idRFPEmailHistory (int)
        /// Parameter 3 : Files want to send as an attachment in email (File)
        /// </summary>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        /// <exception cref="System.Web.Http.HttpResponseException">
        /// </exception>
        /// <exception cref="RpoBusinessException">
        /// Contact Id not found
        /// or
        /// or
        /// Contact Id not found
        /// or
        /// </exception>
        [Route("api/Rfps/Attachment")]
        [ResponseType(typeof(RFPEmailAttachmentHistory))]
        [Authorize]
        [RpoAuthorize]
        public async Task<HttpResponseMessage> PostAttachment()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            System.Collections.Specialized.NameValueCollection formData = provider.FormData;
            IList<HttpContent> files = provider.Files;
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            int idRfp = Convert.ToInt32(formData["idRfp"]);
            int idRFPEmailHistory = Convert.ToInt32(formData["idRFPEmailHistory"]);
            bool isProposalPDFAttached = Convert.ToBoolean(formData["IsProposalPDFAttached"]);


            var rfpEmail = rpoContext.RFPEmailHistories.Include("RFPEmailCCHistories.Contact").Include("Rfp").Include("ContactAttention").Include("FromEmployee").Where(x => x.Id == idRFPEmailHistory).FirstOrDefault();

            if (rfpEmail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var cc = new List<KeyValuePair<string, string>>();

            foreach (RFPEmailCCHistory item in rfpEmail.RFPEmailCCHistories)
            {
                if (item == null)
                    throw new RpoBusinessException("Contact Id not found");

                if (item.Contact.Email == null)
                    throw new RpoBusinessException($"Contact {item.Contact.FirstName} not has a valid e-mail");

                cc.Add(new KeyValuePair<string, string>(item.Contact.Email, item.Contact.FirstName + " " + item.Contact.LastName));
            }

            var to = new List<KeyValuePair<string, string>>();

            var contactTo = rfpEmail.ContactAttention;
            if (contactTo == null)
            {
                throw new RpoBusinessException("Contact Id not found");
            }
            if (contactTo.Email == null)
            {
                throw new RpoBusinessException($"Contact {contactTo.FirstName} not has a valid e-mail");
            }
            to.Add(new KeyValuePair<string, string>(contactTo.Email, contactTo.FirstName + " " + contactTo.LastName));


            var attachments = new List<string>();

            Employee employeeFrom = rfpEmail.FromEmployee;

            if (employeeFrom != null)
            {
                cc.Add(new KeyValuePair<string, string>(employeeFrom.Email, employeeFrom.FirstName + " " + employeeFrom.LastName));
            }

            try
            {
                var path = HttpRuntime.AppDomainAppPath;
                if (isProposalPDFAttached)
                {
                    string fullpath = HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.ProposalFileExportPath;
                    //string pdffilename = "RFP-Pdf_" + id + ".pdf";

                    var rfpForPdf = rpoContext.Rfps
                   .Include("Borough")
                   .Include("RfpAddress.Borough")
                   .Include("Company")
                   .Include("Contact")
                   .Include("ProjectDetails")
                   .Include("ScopeReview")
                   .Include("ProposalReview.Verbiages").FirstOrDefault(x => x.Id == idRfp);


                    string pdffilename = "RFP #" + rfpForPdf.RfpNumber + " " + rfpForPdf.RfpAddress.HouseNumber + " " + rfpForPdf.RfpAddress.Street + " "
                    + (rfpForPdf.RfpAddress.Borough != null ? rfpForPdf.RfpAddress.Borough.Description : string.Empty) + (!string.IsNullOrEmpty(rfpForPdf.SpecialPlace) ? " " + rfpForPdf.SpecialPlace : string.Empty) + "_" +
                    (rfpForPdf.Company != null ? rfpForPdf.Company.Name : "Individual") + (rfpForPdf.Contact != null ? "_" + rfpForPdf.Contact.FirstName + (!string.IsNullOrEmpty(rfpForPdf.Contact.LastName) ? " " + rfpForPdf.Contact.LastName : string.Empty) : string.Empty)
                    + ".pdf";
                    //filename = filename.R(filename, @"<[^>]+>|&nbsp;", "");
                    pdffilename = Regex.Replace(pdffilename, @"[^0-9a-zA-Z_.]+", "-");

                    string attachmentPath = fullpath + pdffilename;

                    RFPEmailAttachmentHistory rFPEmailAttachment = new RFPEmailAttachmentHistory();
                    rFPEmailAttachment.DocumentPath = pdffilename;
                    rFPEmailAttachment.IdRFPEmailHistory = idRFPEmailHistory;
                    rFPEmailAttachment.Name = pdffilename;
                    rpoContext.RFPEmailAttachmentHistories.Add(rFPEmailAttachment);
                    rpoContext.SaveChanges();

                    string pdfdirectoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.RFPAttachmentsPath));

                    string pdfdirectoryFileName = Convert.ToString(rFPEmailAttachment.Id) + "_" + pdffilename;
                    string newfilename = System.IO.Path.Combine(pdfdirectoryName, pdfdirectoryFileName);

                    if (!Directory.Exists(pdfdirectoryName))
                    {
                        Directory.CreateDirectory(pdfdirectoryName);
                    }

                    if (File.Exists(newfilename))
                    {
                        File.Delete(newfilename);
                    }

                    if (File.Exists(attachmentPath))
                    {
                        File.Copy(attachmentPath, newfilename);
                    }

                    if (File.Exists(newfilename) && !attachments.Contains(pdfdirectoryFileName))
                    {
                        attachments.Add(newfilename);
                    }

                }

                foreach (HttpContent item in files)
                {
                    HttpContent file1 = item;
                    var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');

                    string filename = string.Empty;
                    Stream input = await file1.ReadAsStreamAsync();
                    string directoryName = string.Empty;
                    string URL = string.Empty;

                    RFPEmailAttachmentHistory rFPEmailAttachmentHistory = new RFPEmailAttachmentHistory();
                    rFPEmailAttachmentHistory.DocumentPath = thisFileName;
                    rFPEmailAttachmentHistory.IdRFPEmailHistory = idRFPEmailHistory;
                    rFPEmailAttachmentHistory.Name = thisFileName;
                    rpoContext.RFPEmailAttachmentHistories.Add(rFPEmailAttachmentHistory);
                    rpoContext.SaveChanges();


                    directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.RFPAttachmentsPath));

                    string directoryFileName = Convert.ToString(rFPEmailAttachmentHistory.Id) + "_" + thisFileName;
                    filename = System.IO.Path.Combine(directoryName, directoryFileName);

                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }

                    using (Stream file = File.OpenWrite(filename))
                    {
                        input.CopyTo(file);
                        file.Close();
                    }

                    if (File.Exists(filename) && !attachments.Contains(directoryFileName))
                    {
                        attachments.Add(filename);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/MasterEmailTemplate.htm")))
            {
                body = reader.ReadToEnd();
            }

            TransmissionType transmissionType = rpoContext.TransmissionTypes.FirstOrDefault(x => x.Id == rfpEmail.IdTransmissionType);
            if (transmissionType != null && transmissionType.IsSendEmail)
            {
                string emailBody = body;
                emailBody = emailBody.Replace("##EmailBody##", rfpEmail.EmailMessage);

                //List<TransmissionTypeDefaultCC> transmissionTypeDefaultCC = rpoContext.TransmissionTypeDefaultCCs.Include("Employee").Where(x => x.IdTransmissionType == rfpEmail.IdTransmissionType).ToList();
                List<TransmissionTypeDefaultCC> transmissionTypeDefaultCC = rpoContext.TransmissionTypeDefaultCCs.Include("Employee").Where(x => x.IdEamilType == rfpEmail.IdEmailType).ToList();
                if (transmissionTypeDefaultCC != null && transmissionTypeDefaultCC.Any())
                {
                    foreach (TransmissionTypeDefaultCC item in transmissionTypeDefaultCC)
                    {
                        cc.Add(new KeyValuePair<string, string>(item.Employee.Email, item.Employee.FirstName + " " + item.Employee.LastName));
                    }
                }

                Mail.Send(
                        new KeyValuePair<string, string>(employeeFrom.Email, employeeFrom.FirstName + " " + employeeFrom.LastName),
                        to,
                        cc,
                        rfpEmail.EmailSubject,
                        emailBody,
                        attachments
                    );
            }

            rfpEmail.IsEmailSent = true;
            rpoContext.SaveChanges();

            var rFPEmailAttachmentHistoryList = rpoContext.RFPEmailAttachmentHistories
                .Where(x => x.IdRFPEmailHistory == idRFPEmailHistory).ToList();
            var response = Request.CreateResponse<List<RFPEmailAttachmentHistory>>(HttpStatusCode.OK, rFPEmailAttachmentHistoryList);
            return response;
        }
        /// <summary>
        /// Get the details or Progressnotes against the RFP
        /// </summary>
        /// <param name="idRfp"></param>
        /// <returns></returns>
        [Route("api/rfps/{idRfp}/rfpprogressnotes")]
        [ResponseType(typeof(RfpProgressNote))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetRfpProgressNote(int idRfp)
        {
            Rfp rfp = rpoContext.Rfps.Find(idRfp);
            if (rfp == null)
            {
                return this.NotFound();
            }

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            var result = rpoContext.RfpProgressNotes
                .Include("LastModified")
                .Include("CreatedByEmployee")
                .Where(x => x.IdRfp == idRfp)
                .AsEnumerable()
                .Select(rfpProgressNote => new
                {
                    Id = rfpProgressNote.Id,
                    IdRfp = rfpProgressNote.IdRfp,
                    Notes = rfpProgressNote.Notes,
                    CreatedBy = rfpProgressNote.CreatedBy,
                    LastModifiedBy = rfpProgressNote.LastModifiedBy != null ? rfpProgressNote.LastModifiedBy : rfpProgressNote.CreatedBy,
                    CreatedByEmployee = rfpProgressNote.CreatedByEmployee != null ? rfpProgressNote.CreatedByEmployee.FirstName + " " + rfpProgressNote.CreatedByEmployee.LastName : string.Empty,
                    LastModified = rfpProgressNote.LastModified != null ? rfpProgressNote.LastModified.FirstName + " " + rfpProgressNote.LastModified.LastName : (rfpProgressNote.CreatedByEmployee != null ? rfpProgressNote.CreatedByEmployee.FirstName + " " + rfpProgressNote.CreatedByEmployee.LastName : string.Empty),
                    CreatedDate = rfpProgressNote.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpProgressNote.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpProgressNote.CreatedDate,
                    LastModifiedDate = rfpProgressNote.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpProgressNote.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (rfpProgressNote.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpProgressNote.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpProgressNote.CreatedDate)
                }).OrderByDescending(x => x.LastModifiedDate);

            return Ok(result);
        }

        /// <summary>
        /// Gets the RFP linked jobs.
        /// </summary>
        /// <remarks>To update the RFP number in job and also to migrate the RFP milestone to the job scope. </remarks>
        /// <param name="idRfp">The identifier RFP.</param>
        /// <returns>RFP link with job.</returns>
        [HttpGet]
        [Route("api/Rfps/{idRfp}/rfplinkedjobs")]
        [ResponseType(typeof(bool))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetRfpLinkedJobs(int idRfp)
        {
            //var job = rpoContext.Jobs.Where(x => (x.Id == (rpoContext.JobFeeSchedules.Where(jf => jf.IdRfp == idRfp).Select(jf => jf.IdJob).FirstOrDefault())) || (x.Id == (rpoContext.JobMilestones.Where(jf => jf.IdRfp == idRfp).Select(jf => jf.IdJob).FirstOrDefault()))).Select(j => new
            //{
            //    Id = j.Id,
            //    JobNumber = j.JobNumber,
            //    IdRfp = j.IdRfp,
            //})).FirstOrDefault();
            //return Ok(job);

            string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();

            DataSet ds = new DataSet();

            SqlParameter[] spParameter = new SqlParameter[1];

            spParameter[0] = new SqlParameter("@IdRfp", SqlDbType.Int);
            spParameter[0].Direction = ParameterDirection.Input;
            spParameter[0].Value = idRfp;

            ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "RFPs_JobList", spParameter);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return Ok(new JobLink
                {
                    id = ds.Tables[0].Rows[0]["id"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["id"]),
                    jobNumber = ds.Tables[0].Rows[0]["jobNumber"] == DBNull.Value ? null : Convert.ToString(ds.Tables[0].Rows[0]["jobNumber"]),
                    idRfp = ds.Tables[0].Rows[0]["idRfp"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["idRfp"])
                });
            }
            else
            { return Ok(DBNull.Value); }

        }

        /// <summary>
        /// Gets to be link job list.
        /// </summary>
        /// <remarks>To update the RFP number in job and also to migrate the RFP milestone to the job scope. </remarks>
        /// <returns>IHttp Action Result.</returns>
        [HttpGet]
        [Route("api/Rfps/tobelinkjoblist")]
        [ResponseType(typeof(Job))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetToBeLinkJobList()
        {
            List<Job> jobs = rpoContext.Jobs.ToList();
            if (jobs == null)
            {
                return this.NotFound();
            }

            return Ok(
                jobs.Select(x => new
                {
                    Id = x.Id,
                    JobNumber = x.JobNumber
                }));
        }

        /// <summary>
        /// Puts the link job.
        /// </summary>
        /// <remarks>To update the RFP number in job and also to migrate the RFP milestone to the job scope. </remarks>
        /// <param name="linkJob">The link job.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpPut]
        [Route("api/Rfps/linkjob")]
        [ResponseType(typeof(LinkJobDTO))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult PutLinkJob(LinkJobDTO linkJob)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

            Rfp rfp = rpoContext.Rfps.Include("RfpFeeSchedules").Include("Milestones.MilestoneServices").FirstOrDefault(x => x.Id == linkJob.IdRfp);
            if (rfp == null)
            {
                return this.NotFound();
            }

            foreach (RfpFeeSchedule item in rfp.RfpFeeSchedules)
            {
                JobFeeSchedule jobFeeSchedule = new JobFeeSchedule();
                jobFeeSchedule.IdRfp = item.IdRfp;
                jobFeeSchedule.IdJob = linkJob.IdJob;
                jobFeeSchedule.IdRfpWorkType = item.IdRfpWorkType;


                RfpCostType cstType = (from d in rpoContext.RfpJobTypes where d.Id == item.IdRfpWorkType select d.CostType).FirstOrDefault();

                if (cstType != null && cstType.ToString() == "HourlyCost")
                {
                    jobFeeSchedule.Quantity = item.Quantity * 60;
                    jobFeeSchedule.QuantityPending = item.Quantity * 60;
                }
                else
                {
                    jobFeeSchedule.Quantity = item.Quantity;
                    jobFeeSchedule.QuantityPending = item.Quantity;
                }
                //jobFeeSchedule.Quantity = item.Quantity;
                jobFeeSchedule.Description = item.Description;
                jobFeeSchedule.QuantityAchieved = 0;
                // jobFeeSchedule.QuantityPending = item.Quantity;
                jobFeeSchedule.PONumber = linkJob.PONumber;
                jobFeeSchedule.Cost = item.Cost;
                jobFeeSchedule.TotalCost = item.TotalCost;
                jobFeeSchedule.IdRfpFeeSchedule = item.Id;
                jobFeeSchedule.IsFromScope = true;
                rpoContext.JobFeeSchedules.Add(jobFeeSchedule);
            }
            rpoContext.SaveChanges();

            foreach (Milestone item in rfp.Milestones)
            {
                JobMilestone jobMilestone = new JobMilestone();
                jobMilestone.IdJob = linkJob.IdJob;
                jobMilestone.Name = item.Name;
                jobMilestone.Value = item.Value;
                jobMilestone.PONumber = linkJob.PONumber;
                jobMilestone.CreatedBy = employee.Id;
                jobMilestone.ModifiedBy = employee.Id;
                jobMilestone.CreatedDate = DateTime.UtcNow;
                jobMilestone.LastModified = DateTime.UtcNow;
                jobMilestone.IdRfp = item.IdRfp;
                jobMilestone.Status = "Pending";
                rpoContext.JobMilestones.Add(jobMilestone);

                rpoContext.SaveChanges();

                foreach (MilestoneService milestoneService in item.MilestoneServices)
                {
                    var jobFeeSchedule = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.IdRfpFeeSchedule == milestoneService.IdRfpFeeSchedule);
                    JobMilestoneService jobMilestoneService = new JobMilestoneService();
                    jobMilestoneService.IdMilestone = jobMilestone.Id;
                    jobMilestoneService.IdJobFeeSchedule = jobFeeSchedule.Id;
                    rpoContext.JobMilestoneServices.Add(jobMilestoneService);
                }
            }

            rpoContext.SaveChanges();

            Job job = rpoContext.Jobs.FirstOrDefault(x => x.Id == linkJob.IdJob);

            if (job != null)
            {
                linkJob.JobNumber = job.JobNumber;
            }

            string RFPLinkMessage = JobHistoryMessages.RFPLinkMessage
                  .Replace("##RFP##", rfp.RfpNumber);
            Common.SaveJobHistory(employee.Id, linkJob.IdJob, RFPLinkMessage, JobHistoryType.Job);

            return Ok(linkJob);
        }
        /// <summary>
        /// Puts the Unlink job.
        /// </summary>
        /// <remarks>To update the RFP number in job and also to migrate the RFP milestone to the job scope. </remarks>
        /// <param name="UnlinkJob">The Unlink job.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpPut]
        [Route("api/Rfps/Unlinkjob")]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult PutUnLinkJob(LinkJobDTO UnlinkJob)
        {
            try
            {
                var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
                if (UnlinkJob.IdJob.ToString() != null)
                {
                    var rfp_JFcheck = rpoContext.JobFeeSchedules.Where(x => x.IdRfp == UnlinkJob.IdRfp).ToList();
                    if (rfp_JFcheck.Count != 0)
                    {
                        var unlink_jobfeeschedule = (from jfee in rpoContext.JobFeeSchedules
                                                     where jfee.IdJob == UnlinkJob.IdJob && jfee.IdRfp == UnlinkJob.IdRfp
                                                     select jfee);
                        foreach (var jobschedule in unlink_jobfeeschedule)
                        {
                            jobschedule.IdRfp = null;
                        }
                        rpoContext.SaveChanges();
                    }
                    var rfp_JMcheck = rpoContext.JobMilestones.Where(x => x.IdRfp == UnlinkJob.IdRfp).ToList();
                    if (rfp_JMcheck.Count != 0 )
                    {
                       var unlink_Milestones = (from jm in rpoContext.JobMilestones
                                                     where jm.IdJob == UnlinkJob.IdJob && jm.IdRfp == UnlinkJob.IdRfp
                                                     select jm);
                        foreach (var milestone in unlink_Milestones)
                        {
                            milestone.IdRfp = null;
                        }
                        rpoContext.SaveChanges();
                    }
                    var rfp_jb = rpoContext.Jobs.Where(x => x.IdRfp == UnlinkJob.IdRfp).FirstOrDefault();
                    if (rfp_jb != null)
                    {
                        var unlink_rfp_job = (from p in rpoContext.Jobs
                                              where p.Id == UnlinkJob.IdJob
                                              select p).FirstOrDefault(x => x.IdRfp == null);
                        rpoContext.SaveChanges();
                    }
                    //if(rfp_JFcheck.Count == 0 && rfp_JMcheck.Count == 0 && rfp_jb == null) { } 
                }
                
            }catch(Exception ex) { throw ex.InnerException; }

            return Ok("RFP is Un-linked from job #" + UnlinkJob.IdJob + " Successfully!");
        }
        /// <summary>
        /// Formats the RFP.
        /// </summary>
        /// <param name="rfp">The RFP.</param>
        /// <returns>RfpDetailDTO.</returns>
        private RfpDetailDTO FormatRfp(Rfp rfp)
        {
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            Address objCompanycontactAddress = (from d in rpoContext.Addresses where d.Id == rfp.IdClientAddress select d).FirstOrDefault();

            string Add1 = objCompanycontactAddress != null && !string.IsNullOrEmpty(rfp.Address1) ? objCompanycontactAddress.Address1 : string.Empty;
            string Add2 = objCompanycontactAddress != null && !string.IsNullOrEmpty(rfp.Address2) ? objCompanycontactAddress.Address2 : string.Empty;
            string City = objCompanycontactAddress != null && !string.IsNullOrEmpty(rfp.City) ? objCompanycontactAddress.City : string.Empty;
            //  string State = objCompanycontactAddress != null && objCompanycontactAddress.State != null && !string.IsNullOrEmpty(rfp.State.Name) ? objCompanycontactAddress.State.Name : string.Empty;
            //  string Zipcode = objCompanycontactAddress != null && !string.IsNullOrEmpty(rfp.ZipCode) ? objCompanycontactAddress.ZipCode : string.Empty;
           
            return new RfpDetailDTO
            {
                Id = rfp.Id,
                Address1 = !string.IsNullOrEmpty(rfp.Address1) ? rfp.Address1 : Add1,
                Address2 = !string.IsNullOrEmpty(rfp.Address2) ? rfp.Address2 : Add2,
                Apartment = rfp.Apartment,
                Block = rfp.Block,
                //Borough = rfp.Borough != null ? rfp.Borough.Description : null,
                //Company = rfp.Company != null ? rfp.Company.Name : null,
                //Contact = rfp.Contact != null ? rfp.Contact.FirstName + " " + rfp.Contact.LastName : null,
                Cost = rfp.Cost,
                Email = rfp.Email,
                FloorNumber = rfp.FloorNumber,
                HasEnvironmentalRestriction = rfp.HasEnvironmentalRestriction,
                HasLandMarkStatus = rfp.HasLandMarkStatus,
                HasOpenWork = rfp.HasOpenWork,
                HouseNumber = rfp.HouseNumber,
                IdBorough = rfp.IdBorough,
                IdCompany = rfp.IdCompany,
                IdContact = rfp.IdContact,
                IdReferredByCompany = rfp.IdReferredByCompany,
                IdReferredByContact = rfp.IdReferredByContact,
                IdRfpAddress = rfp.IdRfpAddress,
                ProjectDescription = rfp.ProjectDescription,
                LastUpdatedStep = rfp.LastUpdatedStep,
                CompletedStep = rfp.CompletedStep,
                Lot = rfp.Lot,
                Phone = rfp.Phone,
                ReferredByCompany = rfp.ReferredByCompany,
                ReferredByContact = rfp.ReferredByContact,
                RfpAddress = rfp.RfpAddress,
                RfpNumber = rfp.RfpNumber,
                SpecialPlace = rfp.SpecialPlace,
                IdRfpStatus = rfp.IdRfpStatus,
                RfpStatus = rfp.RfpStatus,
                StreetNumber = rfp.StreetNumber,
                CreatedDate = rfp.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfp.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfp.CreatedDate,
                LastModifiedDate = rfp.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfp.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfp.LastModifiedDate,
                StatusChangedDate = rfp.StatusChangedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfp.StatusChangedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfp.StatusChangedDate,
                City = !string.IsNullOrEmpty(rfp.City) ? rfp.City : City,
                CreatedByEmployee = rfp.CreatedBy != null ? rfp.CreatedBy.FirstName + " " + rfp.CreatedBy.LastName : string.Empty,
                LastModifiedByEmployee = rfp.LastModifiedBy != null ? rfp.LastModifiedBy.FirstName + " " + rfp.LastModifiedBy.LastName : (rfp.CreatedBy != null ? rfp.CreatedBy.FirstName + " " + rfp.CreatedBy.LastName : string.Empty),
                GoNextStep = rfp.GoNextStep,
                IdCreatedBy = rfp.IdCreatedBy,
                IdLastModifiedBy = rfp.IdLastModifiedBy,
                IdRfpScopeReview = rfp.IdRfpScopeReview,
                IdState = rfp.IdState,
                Milestones = rfp.Milestones,
                ProjectDetails = rfp.ProjectDetails,
                ProposalReview = rfp.ProposalReview,
                RfpFeeSchedules = rfp.RfpFeeSchedules,
                RfpReviewers = rfp.RfpReviewers,
                ScopeReview = rfp.ScopeReview,
                State = rfp.State,
                ZipCode = rfp.ZipCode,
                IdSignature = rfp.IdSignature,
                IdClientAddress = rfp.IdClientAddress,
                RfpDocuments = rfp.RfpDocuments != null && rfp.RfpDocuments.Count > 0 ?
                rfp.RfpDocuments.Select(x => new RfpDocumentDetail()
                {
                    Id = x.Id,
                    DocumentPath = APIUrl + "/" + Properties.Settings.Default.RFPDocumentPath + "/" + x.Id + "_" + x.DocumentPath,
                    IdRfp = x.IdRfp,
                    Name = x.Name
                }).ToList()
                : null,
                RfpProposalReviewToDetails = new RfpProposalReviewToDetails()
                {
                    contactAttention = rfp.Contact != null ? (rfp.Contact.Prefix != null ? rfp.Contact.Prefix.Name : string.Empty) + " " + rfp.Contact.FirstName + (rfp.Contact.LastName != null ? " " + rfp.Contact.LastName : string.Empty) : string.Empty,
                    companyName = rfp != null && rfp.Company != null && rfp.Company.Name != null ? rfp.Company.Name : string.Empty,
                    companyaddress1 = rfp.Address1 != null ? rfp.Address1 : string.Empty,
                    companyaddress2 = rfp.Address2 != null ? rfp.Address2 : string.Empty,
                    companycity = rfp.City != null ? rfp.City : string.Empty,
                    companystate = rfp.State != null ? rfp.State.Acronym : string.Empty,
                    companyzipCode = rfp.ZipCode != null ? rfp.ZipCode : string.Empty
                },
                RfpProposalReviewSubjectDetails = new RfpProposalReviewSubjectDetails()
                {
                    subjectLineDetails = rfp.RfpAddress != null ? ((rfp.RfpAddress.HouseNumber != null ? rfp.RfpAddress.HouseNumber + " " : string.Empty) +
                                                                    (rfp.RfpAddress.Street != null ? rfp.RfpAddress.Street + ", " : string.Empty) +
                                                                    (rfp.RfpAddress.Borough != null ? rfp.RfpAddress.Borough.Description : string.Empty) +
                                                                    (rfp.SpecialPlace != null && !string.IsNullOrEmpty(rfp.SpecialPlace) ? " | " + rfp.SpecialPlace : string.Empty) +
                                                                    (rfp.RfpAddress.OutsideNYC != null ? ", " + rfp.RfpAddress.OutsideNYC : string.Empty))
                                                                    : string.Empty
                }
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="rfp">The RFP.</param>
        /// <returns>RfpDetail.</returns>
        private RfpDetail FormatDetails(Rfp rfp)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new RfpDetail
            {
                Id = rfp.Id,
                Address1 = rfp.Address1,
                Address2 = rfp.Address2,
                Apartment = rfp.Apartment,
                Block = rfp.Block,
                //Borough = rfp.Borough != null ? rfp.Borough.Description : null,
                //Company = rfp.Company != null ? rfp.Company.Name : null,
                //Contact = rfp.Contact != null ? rfp.Contact.FirstName + " " + rfp.Contact.LastName : null,
                Cost = rfp.Cost,
                Email = rfp.Email,
                FloorNumber = rfp.FloorNumber,
                HasEnvironmentalRestriction = rfp.HasEnvironmentalRestriction,
                HasLandMarkStatus = rfp.HasLandMarkStatus,
                HasOpenWork = rfp.HasOpenWork,
                HouseNumber = rfp.HouseNumber,
                IdBorough = rfp.IdBorough,
                IdCompany = rfp.IdCompany,
                IdContact = rfp.IdContact,
                IdReferredByCompany = rfp.IdReferredByCompany,
                IdReferredByContact = rfp.IdReferredByContact,
                IdRfpAddress = rfp.IdRfpAddress,
                LastModifiedDate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfp.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)),
                LastUpdatedStep = rfp.LastUpdatedStep,
                CompletedStep = rfp.CompletedStep,
                Lot = rfp.Lot,
                Phone = rfp.Phone,
                ReferredByCompany = rfp.ReferredByCompany != null ? rfp.ReferredByCompany.Name : null,
                ReferredByContact = rfp.ReferredByContact != null ? rfp.ReferredByContact.FirstName + " " + rfp.ReferredByContact.LastName : null,
                ZipCode = rfp.RfpAddress != null ? rfp.RfpAddress.ZipCode : null,
                RfpNumber = rfp.RfpNumber,
                SpecialPlace = rfp.SpecialPlace,
                IdRfpStatus = rfp.IdRfpStatus,
                RfpStatusDisplayName = rfp.RfpStatus != null ? rfp.RfpStatus.DisplayName : string.Empty,
                RfpStatus = rfp.RfpStatus != null ? rfp.RfpStatus.Name : string.Empty,
                StreetNumber = rfp.StreetNumber,
                CreatedDate = rfp.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfp.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfp.CreatedDate,
                IdClientAddress = rfp.IdClientAddress != null ? rfp.IdClientAddress : null,
                ProjectDescription = rfp.ProjectDescription != null ? rfp.ProjectDescription : string.Empty,
                IdSignature = rfp.IdSignature != null ? rfp.IdSignature : string.Empty,
            };
        }
        /// <summary>
        /// get the download the RFP PDF download
        /// </summary>
        /// <param name="id"></param>
        /// <returns>to return  the file path</returns>
        [HttpGet]
        [Route("api/Rfps/{id}/DownloadMsg")]
        [Authorize]
        [RpoAuthorize]
        public HttpResponseMessage DownLoadFile(int id)
        {
            var rfp = rpoContext.RfpDocuments.FirstOrDefault(x => x.Id == id);

            if (rfp == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Not Found");
            }


            Byte[] bytes = null;
            if (rfp.DocumentPath != null)
            {
                var path = HttpRuntime.AppDomainAppPath;
                string directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.RFPDocumentPath));
                string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(directoryName, rfp.Id + "_" + rfp.DocumentPath));
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                bytes = br.ReadBytes((Int32)fs.Length);
                br.Close();
                fs.Close();
            }

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            System.IO.MemoryStream stream = new MemoryStream(bytes);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-outlook");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = rfp.Id + "_" + rfp.DocumentPath
            };
            return (result);
        }
    }
    public class Downloadmsg
    {
        public string FileName { get; set; }

        public string FileType { get; set; }
    }

    /// <summary>
    /// Class Header.
    /// </summary>
    /// <seealso cref="iTextSharp.text.pdf.PdfPageEventHelper" />
    public partial class Header : PdfPageEventHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        /// <param name="FirstLastName">First name of the last.</param>
        /// <param name="proposalName">Name of the proposal.</param>
        public Header(string FirstLastName, string proposalName, string companyName, int idJob, string address)
        {
            this.Name = FirstLastName;
            this.ProposalName = proposalName;
            this.CompanyName = companyName;
            this.IdJob = idJob;
            this.Address = address;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the proposal.
        /// </summary>
        /// <value>The name of the proposal.</value>
        public string ProposalName { get; set; }

        public string CompanyName { get; set; }
        public int IdJob { get; set; }
        public string Address { get; set; }


        /// <summary>
        /// Called when [start page].
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="doc">The document.</param>
        public override void OnStartPage(PdfWriter writer, Document doc)
        {
            string fontRegular = HttpContext.Current.Server.MapPath("~\\HTML\\fonts\\AvenirNextLTPro-Regular.ttf");
            string fontBold = HttpContext.Current.Server.MapPath("~\\HTML\\fonts\\Avenir Next LT Pro Bold.ttf");
            BaseFont customfontRegualr = BaseFont.CreateFont(fontRegular, BaseFont.CP1252, BaseFont.EMBEDDED);
            BaseFont customfontBold = BaseFont.CreateFont(fontBold, BaseFont.CP1252, BaseFont.EMBEDDED);
            Font font_Table_Header = new Font(customfontBold, 16, 1);
            Font font_Bold_11 = new Font(customfontRegualr, 11, 1);
            Font font_Regular_11 = new Font(customfontRegualr, 11, 0);
            Font font_BOLDITALIC_11 = new Font(customfontRegualr, 11,Font.BOLDITALIC);

            DateTime today = DateTime.Today;
            string s_today = today.ToString("MMMM dd, yyyy");

            int pageNumber = writer.PageNumber - 1;

            if (pageNumber > 0)
            {
                Image logo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~\\HTML\\images\\RPO-Logo-pdf.png"));
                logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                logo.ScaleToFit(100, 80);
                logo.SetAbsolutePosition(260, 760);

                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;

                PdfPCell cell = new PdfPCell(logo);
               // cell.Colspan = 2;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Rowspan = 2;
                cell.PaddingBottom = 5;
                cell.Border = PdfPCell.NO_BORDER;
                table.AddCell(cell);

                var page = pageNumber + 1;

                string reportHeader = //"RPO INCORPORATED" + Environment.NewLine +
                "146 West 29th Street, Suite 2E"
                + Environment.NewLine + "New York, NY 10001"
                + Environment.NewLine + "(212) 566-5110"
                + Environment.NewLine + "www.rpoinc.com";

                string reportTitle = "RPO, Inc." + Environment.NewLine + "Construction Consultants";

                cell = new PdfPCell(new Phrase(reportTitle, font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -30;
                cell.Colspan = 5;
                cell.PaddingTop = -2;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportHeader, font_Bold_11));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -30;
                cell.Colspan = 5;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table.AddCell(cell);
                doc.Add(table);

                PdfPTable table2 = new PdfPTable(2);
                table2.WidthPercentage = 100;

                PdfPTable righttable = new PdfPTable(2);
                righttable.SetWidths(new float[] { 10, 90 });
                righttable.WidthPercentage = 100;

                PdfPCell rightcell = new PdfPCell(new Phrase("RE: ", font_BOLDITALIC_11));
                rightcell.HorizontalAlignment = Element.ALIGN_LEFT;
                rightcell.Border = Rectangle.NO_BORDER;
                righttable.AddCell(rightcell);

                rightcell = new PdfPCell(new Phrase(ProposalName, font_BOLDITALIC_11));
                rightcell.HorizontalAlignment = Element.ALIGN_LEFT;
                rightcell.Border = Rectangle.NO_BORDER;
                righttable.AddCell(rightcell);

                PdfPTable lefttable = new PdfPTable(1);
                lefttable.WidthPercentage = 100;

                PdfPCell leftcell = new PdfPCell(new Phrase(Name + (!string.IsNullOrEmpty(CompanyName) ? Environment.NewLine + CompanyName : string.Empty) + Environment.NewLine + s_today, font_Regular_11));
                leftcell.HorizontalAlignment = Element.ALIGN_LEFT;
                leftcell.Border = Rectangle.NO_BORDER;
                lefttable.AddCell(leftcell);

                cell = new PdfPCell(lefttable);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table2.AddCell(cell);

                cell = new PdfPCell(righttable);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                table2.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table2.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                table2.AddCell(cell);

                doc.Add(table2);

            }

        }

        /// <summary>
        /// Called when [end page].
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="doc">The document.</param>
        public override void OnEndPage(PdfWriter writer, Document doc)
        {
            base.OnEndPage(writer, doc);

            int pageNumber = writer.PageNumber;

            var FontColour = new BaseColor(85, 85, 85);
            var MyFont = FontFactory.GetFont("Avenir Next LT Pro", 10, FontColour);

            PdfPTable table = new PdfPTable(1);
            table.WidthPercentage = 100;
            //table.TotalWidth = 592F;
            table.TotalWidth = doc.PageSize.Width - 40f;

         
            PdfPCell cell = new PdfPCell(new Phrase("Page | " + pageNumber, new Font(MyFont)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = PdfPCell.NO_BORDER;
            cell.PaddingTop = 10f;
            cell.Colspan = 1;
            table.AddCell(cell);

            table.WriteSelectedRows(0, -1, 0, doc.Bottom, writer.DirectContent);
        }


    }
    public class JobLink
    {
        public int id { get; set; }
        public string jobNumber { get; set; }
        public int idRfp { get; set; }
    }
}