// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-31-2018
// ***********************************************************************
// <copyright file="ProposalReviewController.cs" company="CREDENCYS">
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
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using Hubs;
    using Model.Models.Enums;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using SystemSettings;
    using System.Text.RegularExpressions;
    using iTextSharp.text.pdf;
    using iTextSharp.text;
    using System.Globalization;
    using iTextSharp.tool.xml;

    /// <summary>
    /// Class ProposalReviewController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class ProposalReviewController : HubApiController<GroupHub>
    {
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the proposal review.
        /// </summary>
        /// <param name="idRfp">The identifier RFP.</param>
        /// <returns> Gets the proposal review List of items.</returns>
        [Authorize]
        [RpoAuthorize]
        [Route("api/Rfps/{idRfp}/ProposalReview")]
        [ResponseType(typeof(RfpProposalReview))]
        public IHttpActionResult GetProposalReview(int idRfp)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewRFP)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddRFP)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteRFP))
            {
                var rfp = rpoContext.Rfps.Include("ProposalReview.Verbiages")
                                     .Include("Milestones.MilestoneServices.RfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent")
                                     .FirstOrDefault(r => r.Id == idRfp);

                if (rfp == null)
                    return this.NotFound();

                double? calculatedCost = rpoContext.RfpFeeSchedules.Where(x => x.IdRfp == idRfp).Sum(x => x.TotalCost);

                rfp.SetResponseGoNextStepHeader();
                return Ok(Format(rfp.ProposalReview, rfp.Milestones, rfp.Id, rfp.Cost, Convert.ToDouble(calculatedCost), Convert.ToBoolean(rfp.IsSignatureNewPage), rfp.IdSignature));
            }

            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        /// Get the Verbiages list againgst RFPs
        /// </summary>
        /// <param name="idRfp"></param>
        /// <param name="idVerbiages"></param>
        /// <returns>et the Verbiages list againgst RFPs</returns>
        [Authorize]
        [RpoAuthorize]
        [Route("api/Rfps/{idRfp}/{idVerbiages}/Verbiages")]
        [ResponseType(typeof(RfpProposalReview))]
        public IHttpActionResult GetProposalReviewVerbiages(int idRfp, int idVerbiages)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewRFP)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddRFP)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteRFP))
            {
                Rfp rfp = rpoContext.Rfps.FirstOrDefault(r => r.Id == idRfp);

                if (rfp == null)
                    return this.NotFound();

                Verbiage objverbagies = rpoContext.Verbiages.Find(idVerbiages);

                RfpProposalReviewDetail objReview = new RfpProposalReviewDetail();
                string content = string.Empty;
                content = objverbagies.Content.Replace("##Name##", rfp.Contact != null ? (rfp.Contact.Prefix != null ? rfp.Contact.Prefix.Name : string.Empty) + " " + (rfp.Contact.LastName != null ? rfp.Contact.LastName : string.Empty) : string.Empty);
                objReview.Content = content;
                objReview.IdVerbiage = objverbagies.Id;

                return Ok(objReview);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// For ProposalReviewType use Introduction = 1, Scope = 2, AdditionalService = 3
        /// </summary>
        /// <param name="idRfp">The identifier RFP.</param>
        /// <param name="proposalReview">The proposal review.</param>
        /// <returns>update the detail of proposal review.</returns>
        [Authorize]
        [RpoAuthorize]
        [Route("api/Rfps/{idRfp}/ProposalReview")]
        [ResponseType(typeof(RfpProposalReview))]
        public IHttpActionResult PutProposalReview(int idRfp, RfpProposalReviewCreateUpdate proposalReview)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddRFP))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (proposalReview.MilestoneServices != null && proposalReview.MilestoneServices.Count > 0)
                {
                    var duplicateService = proposalReview.MilestoneServices.GroupBy(x => x.IdRfpFeeSchedule)
                                           .Where(g => g.Count() > 1)
                                           .ToDictionary(x => x.Key, y => y.Count());

                    if (duplicateService != null && duplicateService.Count > 0)
                    {
                        throw new RpoBusinessException(StaticMessages.DuplicateMilestoneServiceMessage);
                    }
                }

                string isSaveAndExit = string.Empty;
                isSaveAndExit = (this.Request.Headers.GetValues("isSaveAndExit")).FirstOrDefault();

                string isApproveSend = string.Empty;
                isApproveSend = (this.Request.Headers.GetValues("isApproveSend")).FirstOrDefault();

                var rfp = rpoContext.Rfps.Include("CreatedBy").FirstOrDefault(r => r.Id == idRfp);

                if (rfp == null)
                {
                    return this.NotFound();
                }

                rfp.LastModifiedDate = DateTime.UtcNow;
                rfp.IsSignatureNewPage = proposalReview.IsSignatureNewPage;
                rfp.IdSignature = proposalReview.IdSignature != null ? proposalReview.IdSignature : string.Empty;
                rfp.ProcessGoNextStepHeader();

                rfp.Cost = proposalReview.Cost;

                var rfpProposalReviewList = rfp.ProposalReview.Select(x => x).ToList();
                if (rfpProposalReviewList != null)
                {
                    foreach (RfpProposalReview item in rfpProposalReviewList)
                   {
                        if (proposalReview.RfpProposalReviewList != null && proposalReview.RfpProposalReviewList.Count() > 0)
                        {
                            var rfpProposalReview = proposalReview.RfpProposalReviewList.FirstOrDefault(x => x.Id == item.Id);
                            //remove div tag in rfps content editor
                            if (rfpProposalReview.IdVerbiage == 2)
                            {
                                rfpProposalReview.Content = RemoveDivtag(rfpProposalReview.Content);
                            }
                            if (rfpProposalReview == null)
                            {
                                rpoContext.RfpProposalReviews.Remove(item);
                            }
                        }
                        else
                        {
                            rpoContext.RfpProposalReviews.Remove(item);
                        }
                    }
                }
                rpoContext.SaveChanges();

                if (proposalReview.RfpProposalReviewList != null && proposalReview.RfpProposalReviewList.Count() > 0)
                {
                    foreach (RfpProposalReview item in proposalReview.RfpProposalReviewList)
                    {
                        if (item.Id > 0)
                        {
                            var rfpProposalReview = rpoContext.RfpProposalReviews.FirstOrDefault(x => x.Id == item.Id);
                            rfpProposalReview.IdRfp = item.IdRfp;
                            rfpProposalReview.Content = item.Content;
                            rfpProposalReview.IdVerbiage = item.IdVerbiage;
                            rfpProposalReview.DisplayOrder = item.DisplayOrder;
                            rpoContext.SaveChanges();
                        }
                        else
                        {
                            RfpProposalReview rfpProposalReview = new RfpProposalReview();
                            rfpProposalReview.IdRfp = item.IdRfp;
                            rfpProposalReview.Content = item.Content;
                            rfpProposalReview.IdVerbiage = item.IdVerbiage;
                            rfpProposalReview.DisplayOrder = item.DisplayOrder;
                            rpoContext.RfpProposalReviews.Add(rfpProposalReview);
                            rpoContext.SaveChanges();
                        }
                    }
                }

                var rfpMilestonesList = rfp.Milestones.Select(x => x).ToList();
                if (rfpMilestonesList != null)
                {
                    foreach (Milestone item in rfpMilestonesList)
                    {
                        if (proposalReview.RfpMilestoneList != null && proposalReview.RfpMilestoneList.Count() > 0)
                        {
                            var rfpMilestone = proposalReview.RfpMilestoneList.FirstOrDefault(x => x.Id == item.Id);
                            if (rfpMilestone == null)
                            {
                                if (item.MilestoneServices != null)
                                {
                                    rpoContext.MilestoneServices.RemoveRange(item.MilestoneServices);
                                }
                                rpoContext.Milestones.Remove(item);
                            }
                        }
                        else
                        {
                            if (item.MilestoneServices != null)
                            {
                                rpoContext.MilestoneServices.RemoveRange(item.MilestoneServices);
                            }
                            rpoContext.Milestones.Remove(item);
                        }
                    }
                    rpoContext.SaveChanges();
                }

                if (proposalReview.RfpMilestoneList != null && proposalReview.RfpMilestoneList.Count() > 0)
                {
                    foreach (Milestone item in proposalReview.RfpMilestoneList)
                    {
                        if (item.Id > 0)
                        {
                            var rfpMilestone = rpoContext.Milestones.FirstOrDefault(x => x.Id == item.Id);
                            rfpMilestone.IdRfp = item.IdRfp;
                            rfpMilestone.Name = item.Name;
                            rfpMilestone.Value = item.Value;
                            rpoContext.SaveChanges();

                            var milestoneServicesList = rpoContext.MilestoneServices.Where(x => x.IdMilestone == item.Id).ToList();
                            if (milestoneServicesList != null && milestoneServicesList.Count() > 0)
                            {
                                foreach (MilestoneService milestoneService in milestoneServicesList)
                                {
                                    if (item.MilestoneServices != null && item.MilestoneServices.Count() > 0)
                                    {
                                        var rfpMilestoneService = item.MilestoneServices.FirstOrDefault(x => x.Id == milestoneService.Id);
                                        if (rfpMilestoneService == null)
                                        {
                                            rpoContext.MilestoneServices.Remove(milestoneService);
                                        }
                                    }
                                    else
                                    {
                                        rpoContext.MilestoneServices.Remove(milestoneService);
                                    }
                                }
                            }

                            if (item.MilestoneServices != null && item.MilestoneServices.Count > 0)
                            {
                                foreach (MilestoneService milestoneService in item.MilestoneServices)
                                {
                                    if (milestoneService.Id > 0)
                                    {
                                        var service = rpoContext.MilestoneServices.FirstOrDefault(x => x.Id == item.Id);
                                        service.IdMilestone = milestoneService.IdMilestone;
                                        service.IdRfpFeeSchedule = milestoneService.IdRfpFeeSchedule;
                                        rpoContext.SaveChanges();
                                    }
                                    else
                                    {
                                        MilestoneService service = new MilestoneService();
                                        service.IdMilestone = milestoneService.IdMilestone;
                                        service.IdRfpFeeSchedule = milestoneService.IdRfpFeeSchedule;
                                        rpoContext.MilestoneServices.Add(service);
                                        rpoContext.SaveChanges();
                                    }
                                }
                            }
                        }
                        else
                        {
                            Milestone rfpMilestone = new Milestone();
                            rfpMilestone.IdRfp = item.IdRfp;
                            rfpMilestone.Name = item.Name;
                            rfpMilestone.Value = item.Value;
                            rpoContext.Milestones.Add(rfpMilestone);
                            rpoContext.SaveChanges();

                            if (item.MilestoneServices != null && item.MilestoneServices.Count > 0)
                            {
                                foreach (MilestoneService milestoneService in item.MilestoneServices)
                                {
                                    MilestoneService service = new MilestoneService();
                                    service.IdMilestone = rfpMilestone.Id;
                                    service.IdRfpFeeSchedule = milestoneService.IdRfpFeeSchedule;
                                    rpoContext.MilestoneServices.Add(service);
                                }
                            }
                            rpoContext.SaveChanges();
                        }
                    }
                }

                if (rfp.ScopeReview != null)
                {
                    int idVerbiage = rpoContext.Verbiages.Where(x => x.Name == "scope").Select(x => x.Id).FirstOrDefault();
                    RfpProposalReview rfpProposalReview = rpoContext.RfpProposalReviews.FirstOrDefault(x => x.IdRfp == idRfp && x.IdVerbiage == idVerbiage);
                    if (rfpProposalReview != null)
                    {
                        rfp.ScopeReview.Description = rfpProposalReview.Content;
                    }
                }

                if (employee != null)
                {
                    rfp.IdLastModifiedBy = employee.Id;
                }
                rfp.LastModifiedDate = DateTime.UtcNow;

                //rfp.LastUpdatedStep = 4;
                //if (rfp.CompletedStep < 4)
                //{
                //    rfp.CompletedStep = 4;
                //}

                rfp.LastUpdatedStep = proposalReview.RfpMilestoneList.Count != 0 ? 5 : 4;
                if (rfp.CompletedStep < 4)
                {
                    rfp.CompletedStep = 4;
                }
                try
                {
                    rpoContext.SaveChanges();

                    if (!Convert.ToBoolean(isSaveAndExit))
                        if (Convert.ToBoolean(isApproveSend))
                        {
                            if (employee.Id != rfp.IdCreatedBy)
                            {
                                this.SendApprovalEmail(rfp, employee);
                            }
                            //this.SendApprovalEmailSystemSettings(rfp, employee);
                        }

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

                var rfpResponse = rpoContext.Rfps.Include("ProposalReview.Verbiages")
                                         .Include("Milestones.MilestoneServices.RfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent")
                                         .FirstOrDefault(r => r.Id == idRfp);
                double? calculatedCost = rpoContext.RfpFeeSchedules.Where(x => x.IdRfp == idRfp).Sum(x => x.TotalCost);

                string directoryName = HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.ProposalFileExportPath;

                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                var rfpForPdf = rpoContext.Rfps
                    .Include("Borough")
                    .Include("RfpAddress.Borough")
                    .Include("Company")
                    .Include("Contact")
                    .Include("ProjectDetails")
                    .Include("ScopeReview")
                    .Include("ProposalReview.Verbiages").FirstOrDefault(x => x.Id == idRfp);

                if (rfpForPdf == null)
                {
                    return this.NotFound();
                }

                string filename = this.CreateProposalPdf(rfpForPdf);
                FileInfo fileinfo = new FileInfo(HttpContext.Current.Server.MapPath("~") + Properties.Settings.Default.ProposalFileExportPath + "/" + filename);
                long fileinfoSize = fileinfo.Length;

                var downloadFilePath = Properties.Settings.Default.APIUrl + Properties.Settings.Default.ProposalFileExportPath + "/" + filename;


                return Ok(Format(rfpResponse.ProposalReview, rfpResponse.Milestones, rfpResponse.Id, rfpResponse.Cost, Convert.ToDouble(calculatedCost), Convert.ToBoolean(rfpResponse.IsSignatureNewPage), rfpResponse.IdSignature));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        private string CreateProposalPdf(Rfp rfp)
        {
            string filename = "RFP #" + rfp.RfpNumber + " " + rfp.RfpAddress.HouseNumber + " " + rfp.RfpAddress.Street + " "
                    + (rfp.RfpAddress.Borough != null ? rfp.RfpAddress.Borough.Description : string.Empty) + (!string.IsNullOrEmpty(rfp.SpecialPlace) ? " " + rfp.SpecialPlace : string.Empty) + "_" +
                    (rfp.Company != null ? rfp.Company.Name : "Individual") + (rfp.Contact != null ? "_" + rfp.Contact.FirstName + (!string.IsNullOrEmpty(rfp.Contact.LastName) ? " " + rfp.Contact.LastName : string.Empty) : string.Empty)
                    + ".pdf";
            //filename = filename.R(filename, @"<[^>]+>|&nbsp;", "");
            filename = Regex.Replace(filename, @"[^0-9a-zA-Z_.]+", "-");


            using (MemoryStream stream = new MemoryStream())
            {
                //filename = "RFP-Pdf_" + rfp.Id + ".pdf";

                //RFP #115 1530 Broadway Old Navy | Chipman Design Architecture / Mark Scheerhorn


                string companyName = rfp != null && rfp.Company != null && rfp.Company.Name != null ? rfp.Company.Name : string.Empty;
                int idJob = rpoContext.Jobs.Where(x => x.IdRfp == rfp.Id).Select(x => x.Id).FirstOrDefault();
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
                string address = string.Empty;
                string headeAddress = string.Empty;
                string idSignature = rfp.IdSignature != null ? rfp.IdSignature : string.Empty;
                //   RfpProposalReview rfpcontact = rfp.ProposalReview.OrderBy(x => x.DisplayOrder).Where(x => x.Verbiages.Name == "Header").FirstOrDefault();

                string contactAttentionHeader = string.Empty;
                //  string contactAttention = rfp.Contact != null ? (rfp.Contact.Prefix != null ? rfp.Contact.Prefix.Name : string.Empty) + " " + rfp.Contact.FirstName + (rfp.Contact.LastName != null ? " " + rfp.Contact.LastName : string.Empty) + " " + (rfp.Contact.Suffix != null ? rfp.Contact.Suffix.Description : string.Empty) : string.Empty;
                // string contactAttention = !string.IsNullOrEmpty(rfpcontact.Content) ? rfpcontact.Content : "Not Set";
                string attentionName = rfp.Contact != null ? (rfp.Contact != null && !string.IsNullOrEmpty(rfp.Contact.LastName)
                                                       ? (rfp.Contact.Prefix != null ? rfp.Contact.Prefix.Name + " " : string.Empty)
                                                       + (rfp.Contact.Prefix != null ? rfp.Contact.LastName : rfp.Contact.FirstName + (!string.IsNullOrEmpty(rfp.Contact.LastName) ? " " + rfp.Contact.LastName : string.Empty)) : rfp.Contact.FirstName) : string.Empty;

                string fontRegular = HttpContext.Current.Server.MapPath("~\\HTML\\fonts\\AvenirNextLTPro-Regular.ttf");
                string fontBold = HttpContext.Current.Server.MapPath("~\\HTML\\fonts\\Avenir Next LT Pro Bold.ttf");
                BaseFont customfontRegualr = BaseFont.CreateFont(fontRegular, BaseFont.CP1252, BaseFont.EMBEDDED);
                BaseFont customfontBold = BaseFont.CreateFont(fontBold, BaseFont.CP1252, BaseFont.EMBEDDED);
                Font font__Bold_16 = new Font(customfontBold, 16, 1);
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
                string subjectHeader = string.Empty;
                string subjectRepeatForNewPage = Regex.Replace(subject, @"<[^>]*>", String.Empty).Replace("RE : Proposal for Consulting Services - ", "");
                if (!string.IsNullOrEmpty(companyName) || !string.IsNullOrEmpty(address_HouseNumber) || !string.IsNullOrEmpty(address_Street))
                {
                    address = (!string.IsNullOrEmpty(subjectHeader) ? (address_HouseNumber + " " + address_Street + " " + address_Borough + " " + address_Zipcode) : (address_HouseNumber + " " + address_Street + " " + address_Borough));
                    headeAddress = (!string.IsNullOrEmpty(subjectHeader) ? (address_HouseNumber + " " + address_Street + " " + address_Borough) : (address_HouseNumber + " " + address_Street + " " + address_Borough));
                    subjectHeader = address;
                }
                contactAttentionHeader = Regex.Replace(contactAttentionHeader, @"<(.|n)*?>", string.Empty).Replace("&nbsp;", "").Replace("&amp;", "&").Replace("\n\n", "\n");

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
                //writer.PageEvent = new Header(contactAttention.Trim(), "Proposal for Consulting Services - " + subjectRepeatForNewPage + Environment.NewLine + headeAddress, companyName + Environment.NewLine);
                writer.PageEvent = new Header(contactAttentionHeader.Trim(), "Proposal for Consulting Services " + Environment.NewLine + subjectHeader + Environment.NewLine + specialPlaceName, Environment.NewLine, idJob, address);

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

                string reportTitle = "RPO, Inc." + Environment.NewLine + "Construction Consultants";

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
                cell.PaddingTop = -10;
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
                                    //Font font = new Font(Font.FontFamily.TIMES_ROMAN, 12, 1);
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

        public static String CSS = "p { font-family: 'Avenir Next LT Pro', color:'red', font-size:'20px',list-style:'disc'} li { font-family: 'Times New Roman'}";
        //private PdfPTable AddUL2(string content)
        //{
        //    PdfPTable tableContent = new PdfPTable(1);
        //    tableContent.WidthPercentage = 100;
        //    List<IElement> elements = XMLWorkerHelper.ParseToElementList(content, CSS);
        //    PdfPCell cellContent = new PdfPCell();
        //    foreach (IElement e in elements)
        //    {
        //        if (e is Paragraph)
        //        {
        //            ((Paragraph)e).Font = new Font(Font.FontFamily.TIMES_ROMAN, 12);
        //            ((Paragraph)e).SetLeading(0.0f, 1.0f);
        //        }

        //        if (e is ListItem)
        //        {
        //            ((ListItem)e).Font = new Font(Font.FontFamily.TIMES_ROMAN, 12);
        //            ((ListItem)e).SetLeading(0.0f, 1.0f);
        //        }

        //        if (e is List)
        //        {
        //            List abc = (List)e;
        //            List list = new List(List.UNORDERED, 10f);
        //            Chunk bullet = new Chunk("\u2022");
        //            list.IndentationLeft = 15f;
        //            foreach (var dataItem in abc.Items)
        //            {
        //                bool isNestedList = false;
        //                if (((ListItem)dataItem) != null && ((ListItem)dataItem).Count > 1)
        //                {
        //                    for (int i = 0; i < ((ListItem)dataItem).Count; i++)
        //                    {
        //                        if (((ListItem)dataItem)[i].Type == 14)
        //                        {
        //                            isNestedList = true;
        //                        }
        //                    }
        //                }

        //                if (isNestedList)
        //                {
        //                    for (int i = 0; i < ((ListItem)dataItem).Count; i++)
        //                    {
        //                        if (((ListItem)dataItem)[i] is Paragraph)
        //                        {
        //                            ((Paragraph)((ListItem)dataItem)[i]).Font = new Font(Font.FontFamily.TIMES_ROMAN, 12);
        //                            string liContent = ((Paragraph)((ListItem)dataItem)[i]).Content;
        //                            Font font = new Font(Font.FontFamily.TIMES_ROMAN, 12);
        //                            ListItem listItem = new ListItem(liContent, font);
        //                            listItem.ListSymbol = bullet;
        //                            listItem.SetLeading(0.0f, 1.0f);
        //                            list.Add(listItem);
        //                        }

        //                        if (((ListItem)dataItem)[i] is ListItem)
        //                        {
        //                            ((ListItem)((ListItem)dataItem)[i]).Font = new Font(Font.FontFamily.TIMES_ROMAN, 12);
        //                            string liContent = ((ListItem)((ListItem)dataItem)[i]).Content;
        //                            Font font = new Font(Font.FontFamily.TIMES_ROMAN, 12);
        //                            ListItem listItem = new ListItem(liContent, font);
        //                            listItem.ListSymbol = bullet;
        //                            listItem.SetLeading(0.0f, 1.0f);
        //                            list.Add(listItem);
        //                        }

        //                        if (((ListItem)dataItem)[i] is List)
        //                        {
        //                            List abc_sub = (List)((ListItem)dataItem)[i];
        //                            List list_sub = new List(List.UNORDERED, 10f);
        //                            Chunk bullet_sub = new Chunk("\u2022");
        //                            list_sub.IndentationLeft = 15f;
        //                            foreach (var item in abc_sub.Items)
        //                            {
        //                                string liContent_sub = ((ListItem)item).Content;
        //                                Font font_sub = new Font(Font.FontFamily.TIMES_ROMAN, 12);
        //                                ListItem listItem_sub = new ListItem(liContent_sub, font_sub);
        //                                listItem_sub.ListSymbol = bullet_sub;
        //                                listItem_sub.SetLeading(0.0f, 1.0f);
        //                                list_sub.Add(listItem_sub);
        //                            }
        //                            list.Add(list_sub);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    string liContent = ((ListItem)dataItem).Content;
        //                    Font font = new Font(Font.FontFamily.TIMES_ROMAN, 12);
        //                    ListItem listItem = new ListItem(liContent, font);
        //                    listItem.ListSymbol = bullet;
        //                    listItem.SetLeading(0.0f, 1.0f);
        //                    list.Add(listItem);
        //                }
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
        //    //document.Add(tableContent);
        //    //document.Add(new Paragraph(Chunk.NEWLINE));
        //    return tableContent;
        //}

        private PdfPTable AddUL(string content)
        {

            string fontRegular = HttpContext.Current.Server.MapPath("~\\HTML\\fonts\\AvenirNextLTPro-Regular.ttf");
            BaseFont customfontRegualr = BaseFont.CreateFont(fontRegular, BaseFont.CP1252, BaseFont.EMBEDDED);
            Font font_Reguar_11 = new Font(customfontRegualr, 11, 0);

            PdfPTable tableContent = new PdfPTable(1);
            tableContent.WidthPercentage = 100;
            List<IElement> elements = XMLWorkerHelper.ParseToElementList(content, CSS);
            PdfPCell cellContent = new PdfPCell();
            foreach (IElement e in elements)
            {
                if (e is Paragraph)
                {
                    ((Paragraph)e).Font = font_Reguar_11;
                    ((Paragraph)e).SetLeading(0.0f, 1.0f);
                }

                if (e is ListItem)
                {
                    ((ListItem)e).Font = font_Reguar_11;
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
                                        ListItem listItem_sub = new ListItem(liContent_sub, font_Reguar_11);
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
                                    ((Paragraph)((ListItem)dataItem)[i]).Font = font_Reguar_11;
                                    string liContent = ((Paragraph)((ListItem)dataItem)[i]).Content;
                                    //Font font = new Font(Font.FontFamily.TIMES_ROMAN, 12);
                                    ListItem listItem = new ListItem(liContent, font_Reguar_11);
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
                                    ((ListItem)((ListItem)dataItem)[i]).Font = font_Reguar_11;
                                    string liContent = ((ListItem)((ListItem)dataItem)[i]).Content;
                                    //Font font = new Font(Font.FontFamily.TIMES_ROMAN, 12);
                                    ListItem listItem = new ListItem(liContent, font_Reguar_11);
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
                                       // Font font_sub = new Font(Font.FontFamily.TIMES_ROMAN, 12);
                                        ListItem listItem_sub = new ListItem(liContent_sub, font_Reguar_11);
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
                            ListItem listItem = new ListItem(liContent, font_Reguar_11);
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
        /// Sends the approval email.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="idEmployee">The identifier employee.</param>
        private void SendApprovalEmail(Rfp rfp, Employee employee)
        {
            string javascript = "click=\"redirectFromNotification(j)\"";
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/RFPReviewedAndApproved.htm")))
            {
                body = reader.ReadToEnd();
            }

            string emailBody = body;
            emailBody = emailBody.Replace("##EmployeeName##", rfp.CreatedBy != null ? rfp.CreatedBy.FirstName : string.Empty);
            emailBody = emailBody.Replace("##RfpNumber##", rfp.RfpNumber);
            emailBody = emailBody.Replace("##ReviewerName##", employee != null ? employee.FirstName + " " + employee.LastName : string.Empty);
            emailBody = emailBody.Replace("##CompanyName##", rfp.Company != null ? rfp.Company.Name : "**Individual**");
            emailBody = emailBody.Replace("##ContactName##", rfp.Contact != null ? rfp.Contact.FirstName + " " + rfp.Contact.LastName : string.Empty);
            emailBody = emailBody.Replace("##RFPFullAddress##", rfp.RfpAddress != null ? rfp.RfpAddress.HouseNumber + " " + rfp.RfpAddress.Street + (rfp.RfpAddress.Borough != null ? " " + rfp.RfpAddress.Borough.Description : string.Empty) + " " + rfp.RfpAddress.ZipCode : string.Empty);

            emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/editSiteInformation/" + rfp.Id);

            string subject = EmailNotificationSubject.ProposalIsReviewedAndMarkedForSending.Replace("##RfpNumber##", rfp.RfpNumber);

            Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), new KeyValuePair<string, string>(rfp.CreatedBy.Email, rfp.CreatedBy.FirstName + " " + rfp.CreatedBy.LastName), subject, emailBody, true);

            string proposalIsReviewedAndMarkedForSending = InAppNotificationMessage._ProposalIsReviewedAndMarkedForSending
            .Replace("##RfpNumber##", rfp.RfpNumber)
            .Replace("##ReviewerName##", employee != null ? employee.FirstName + " " + employee.LastName : string.Empty)
            .Replace("##CompanyName##", rfp.Company != null ? rfp.Company.Name : "**Individual**")
            .Replace("##ContactName##", rfp.Contact != null ? rfp.Contact.FirstName + " " + rfp.Contact.LastName : string.Empty)
            .Replace("##RFPFullAddress##", rfp.RfpAddress != null ? rfp.RfpAddress.HouseNumber + " " + rfp.RfpAddress.Street + (rfp.RfpAddress.Borough != null ? " " + rfp.RfpAddress.Borough.Description : string.Empty) + " " + rfp.RfpAddress.ZipCode : string.Empty)
            .Replace("##RedirectionLink##", javascript);
            Common.SendInAppNotifications(Convert.ToInt32(rfp.IdCreatedBy), proposalIsReviewedAndMarkedForSending, Hub, "editSiteInformation/" + rfp.Id);
        }

        /// <summary>
        /// Sends the approval email system settings.
        /// </summary>
        /// <param name="rfp">The RFP.</param>
        /// <param name="employee">The employee.</param>
        private void SendApprovalEmailSystemSettings(Rfp rfp, Employee employee)
        {
            string javascript = "click=\"redirectFromNotification(j)\"";
            SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenProposalIsReviewedAndMarkedForSendingwhenUserPressesApproveSend);
            if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
            {
                foreach (var item in systemSettingDetail.Value)
                {
                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/RFPReviewedAndApproved.htm")))
                    {
                        body = reader.ReadToEnd();
                    }

                    string emailBody = body;
                    emailBody = emailBody.Replace("##EmployeeName##", item.EmployeeName);
                    emailBody = emailBody.Replace("##RfpNumber##", rfp.RfpNumber);
                    emailBody = emailBody.Replace("##ReviewerName##", employee != null ? employee.FirstName + " " + employee.LastName : string.Empty);
                    emailBody = emailBody.Replace("##CompanyName##", rfp.Company != null ? rfp.Company.Name : "**Individual**");
                    emailBody = emailBody.Replace("##ContactName##", rfp.Contact != null ? rfp.Contact.FirstName + " " + rfp.Contact.LastName : string.Empty);
                    emailBody = emailBody.Replace("##RFPFullAddress##", rfp.RfpAddress != null ? rfp.RfpAddress.HouseNumber + " " + rfp.RfpAddress.Street + (rfp.RfpAddress.Borough != null ? " " + rfp.RfpAddress.Borough.Description : string.Empty) + " " + rfp.RfpAddress.ZipCode : string.Empty);

                    emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/editSiteInformation/" + rfp.Id);

                    string subject = EmailNotificationSubject.ProposalIsReviewedAndMarkedForSending.Replace("##RfpNumber##", rfp.RfpNumber);

                    Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), new KeyValuePair<string, string>(item.Email, item.EmployeeName), subject, emailBody, true);

                    string proposalIsReviewedAndMarkedForSending = InAppNotificationMessage._ProposalIsReviewedAndMarkedForSending
                           .Replace("##RfpNumber##", rfp.RfpNumber)
                           .Replace("##ReviewerName##", employee != null ? employee.FirstName + " " + employee.LastName : string.Empty)
                           .Replace("##CompanyName##", rfp.Company != null ? rfp.Company.Name : "**Individual**")
                           .Replace("##ContactName##", rfp.Contact != null ? rfp.Contact.FirstName + " " + rfp.Contact.LastName : string.Empty)
                           .Replace("##RFPFullAddress##", rfp.RfpAddress != null ? rfp.RfpAddress.HouseNumber + " " + rfp.RfpAddress.Street + (rfp.RfpAddress.Borough != null ? " " + rfp.RfpAddress.Borough.Description : string.Empty) + " " + rfp.RfpAddress.ZipCode : string.Empty)
                           .Replace("##RedirectionLink##", javascript);

                    Common.SendInAppNotifications(item.Id, proposalIsReviewedAndMarkedForSending, Hub, "editSiteInformation/" + rfp.Id);
                }
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
        /// Formats the specified proposal review.
        /// </summary>
        /// <param name="proposalReview">The proposal review.</param>
        /// <param name="milestones">The milestones.</param>
        /// <param name="idRfp">The identifier RFP.</param>
        /// <param name="cost">The cost.</param>
        /// <returns>RfpProposalReviewDTO.</returns>
        private RfpProposalReviewDTO Format(ICollection<RfpProposalReview> proposalReview, ICollection<Milestone> milestones, int idRfp, double cost, double calculatedCost, bool isSignatureNewPage, string IdSignature)
        {
            RfpProposalReviewDTO rfpProposalReviewDTO = new RfpProposalReviewDTO();
            rfpProposalReviewDTO.IdRfp = idRfp;
            rfpProposalReviewDTO.Cost = Math.Round(cost, 2);
            rfpProposalReviewDTO.CalculatedCost = calculatedCost;
            rfpProposalReviewDTO.IsSignatureNewPage = isSignatureNewPage;
            rfpProposalReviewDTO.IdSignature = IdSignature;
            rfpProposalReviewDTO.RfpProposalReviewList = proposalReview.AsEnumerable().Select(x => new RfpProposalReviewDetail()
            {
                Id = x.Id,
                Content = x.Content,
                DisplayOrder = x.DisplayOrder,
                IdRfp = x.IdRfp,
                IdVerbiage = x.IdVerbiage,
                VerbiageName = x.Verbiages != null ? x.Verbiages.Name : string.Empty,
                VerbiageType = x.Verbiages != null ? x.Verbiages.VerbiageType : null
            }).OrderBy(x => x.DisplayOrder).ToList();
            rfpProposalReviewDTO.RfpMilestoneList = milestones.AsEnumerable().Select(x => new MilestoneDetail()
            {
                Id = x.Id,
                Name = x.Name,
                Value = x.Value,
                IdRfp = x.IdRfp,
                MilestoneServices = x.MilestoneServices != null ? x.MilestoneServices.AsEnumerable().Select(mi => new MilestoneServiceDetail()
                {
                    IdRfpFeeSchedule = mi.IdRfpFeeSchedule,
                    IdMilestone = mi.IdMilestone,
                    Id = mi.Id,
                    ItemName = mi.RfpFeeSchedule != null && mi.RfpFeeSchedule.RfpWorkType != null ? FormatDetail(mi.RfpFeeSchedule) : string.Empty
                }).ToList() : null
            }).ToList();
            return rfpProposalReviewDTO;
        }

        /// <summary>
        /// Formats the detail.
        /// </summary>
        /// <param name="rfpFeeSchedule">The RFP fee schedule.</param>
        /// <returns>System.String.</returns>
        private string FormatDetail(RfpFeeSchedule rfpFeeSchedule)
        {
            string currentTimeZone = Common.FetchHeaderValues(Request, Common.CurrentTimezoneHeaderKey);

            MilestoneServiceDetail milestoneServiceDetail = new MilestoneServiceDetail();
            milestoneServiceDetail.IdRfpFeeSchedule = rfpFeeSchedule.Id;

            string rfpServiceItem = rfpFeeSchedule.RfpWorkType != null ? rfpFeeSchedule.RfpWorkType.Name : string.Empty;
            string rfpServiceGroup = string.Empty;
            string rfpSubJobType = string.Empty;
            string rfpSubJobTypeCategory = string.Empty;
            string rfpJobType = string.Empty;

            if (rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Level == 4)
            {
                rfpServiceGroup = rfpFeeSchedule.RfpWorkType.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                if (rfpFeeSchedule.RfpWorkType.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Level == 3)
                {
                    rfpSubJobType = rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

                    if (rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Level == 2)
                    {
                        rfpSubJobTypeCategory = rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;

                        if (rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent != null)
                        {
                            rfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent.Name : string.Empty;
                        }
                    }
                    else if (rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Level == 1)
                    {
                        rfpSubJobTypeCategory = string.Empty;
                        rfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (rfpFeeSchedule.RfpWorkType.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Level == 2)
                {
                    rfpSubJobType = string.Empty;

                    rfpSubJobTypeCategory = rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

                    if (rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null)
                    {
                        rfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (rfpFeeSchedule.RfpWorkType.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Level == 1)
                {
                    rfpSubJobType = string.Empty;

                    rfpSubJobTypeCategory = string.Empty;

                    rfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
                }
            }
            else if (rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Level == 3)
            {
                rfpServiceGroup = string.Empty;

                rfpSubJobType = rfpFeeSchedule.RfpWorkType.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                if (rfpFeeSchedule.RfpWorkType.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Level == 2)
                {
                    rfpSubJobTypeCategory = rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

                    if (rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null)
                    {
                        rfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (rfpFeeSchedule.RfpWorkType.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Level == 1)
                {
                    rfpSubJobTypeCategory = string.Empty;

                    rfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
                }
            }
            else if (rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Level == 2)
            {
                rfpServiceGroup = string.Empty;

                rfpSubJobType = string.Empty;

                rfpSubJobTypeCategory = rfpFeeSchedule.RfpWorkType.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                if (rfpFeeSchedule.RfpWorkType.Parent.Parent != null)
                {
                    rfpJobType = rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;
                }
            }
            else if (rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Level == 1)
            {

                rfpJobType = rfpFeeSchedule.RfpWorkType.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                rfpSubJobTypeCategory = string.Empty;

                rfpSubJobType = string.Empty;

                rfpServiceGroup = string.Empty;
            }

            string finalstr = (!string.IsNullOrEmpty(rfpJobType) ? rfpJobType + " - " : string.Empty) +
                (!string.IsNullOrEmpty(rfpSubJobTypeCategory) ? rfpSubJobTypeCategory + " - " : string.Empty) +
                (!string.IsNullOrEmpty(rfpSubJobType) ? rfpSubJobType + " - " : string.Empty) +
                (!string.IsNullOrEmpty(rfpServiceGroup) ? rfpServiceGroup + " - " : string.Empty) +
                (!string.IsNullOrEmpty(rfpServiceItem) ? rfpServiceItem : string.Empty);

            if (rfpFeeSchedule.RfpWorkType.PartOf == null)
            {
                var result = rpoContext.RfpFeeSchedules
                .Include("RfpWorkType")
                .Include("RfpWorkTypeCategory")
                .Include("ProjectDetail.RfpSubJobType")
                .Include("ProjectDetail.RfpJobType")
                .Include("ProjectDetail.RfpSubJobTypeCategory")
                .Where(x => x.IdRfp == rfpFeeSchedule.IdRfp && x.RfpWorkType.PartOf == rfpFeeSchedule.IdRfpWorkType && x.RfpWorkType.IsShowScope == false).Select(d => d.RfpWorkType.Name).ToList();

                string stringJoin = string.Empty;

                foreach (var itemname in result)
                {
                    stringJoin = stringJoin + " " + itemname + ",";
                }
                if (!string.IsNullOrEmpty(stringJoin))
                {
                    stringJoin = stringJoin.Remove(stringJoin.Length - 1);
                    milestoneServiceDetail.ItemName = finalstr + " (" + stringJoin + ")";
                }
                else
                {
                    milestoneServiceDetail.ItemName = finalstr;
                }
            }
            else
            {
                milestoneServiceDetail.ItemName = finalstr;
            }

            return milestoneServiceDetail.ItemName;
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
                Font font_BOLDITALIC_11 = new Font(customfontRegualr, 11, Font.BOLDITALIC);

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
    }
}
