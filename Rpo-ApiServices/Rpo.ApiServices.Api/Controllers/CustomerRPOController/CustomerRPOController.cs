// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Mital Bhatt
// Last Modified On : 08-09-2023
// ***********************************************************************
// <copyright file="CustomerRPOController.cs" company="CREDENCYS">
//     Copyright ©  2023
// </copyright>
// <summary>Class Company Types Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.CustomerRPOController
{
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Api.Tools;
    using Filters;
    using Model.Models.Enums;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;

    public class CustomerRPOController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext rpoContext = new RpoContext();
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/CustomerAccount/SendEmail_Invitaion/{idContact}/{idJob}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult SendEmail_Invitation(int idContact, int idJob)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool sendMailFlag = false;
            var From = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);            
            var contactEmail = rpoContext.Contacts.Where(x => x.Id == idContact).FirstOrDefault().Email;
            var invitation = rpoContext.CustomerInvitationStatus;
            CustomerInvitationStatus CustomerInvitation= rpoContext.CustomerInvitationStatus.FirstOrDefault();
            if (invitation.Count() > 0)
            {
                CustomerInvitation = rpoContext.CustomerInvitationStatus.Where(x => x.EmailAddress == contactEmail).FirstOrDefault();
            }
            
                if (CustomerInvitation == null)
                {
                    CustomerInvitationStatus CustomerInvitationStatus = new CustomerInvitationStatus();
                    CustomerInvitationStatus.IdContact = idContact;
                    if (idJob == 0)
                        CustomerInvitationStatus.IdJob = null;
                    else
                        CustomerInvitationStatus.IdJob = idJob;
                    CustomerInvitationStatus.CUI_Invitatuionstatus = 1;
                    CustomerInvitationStatus.InvitationSentCount = 1;
                    CustomerInvitationStatus.EmailAddress = rpoContext.Contacts.Where(x => x.Id == idContact).FirstOrDefault().Email;
                    if (From != null)
                    {
                        CustomerInvitationStatus.CreatedBy = From.Id;
                    }
                    CustomerInvitationStatus.CreatedDate = DateTime.UtcNow;
                    rpoContext.CustomerInvitationStatus.Add(CustomerInvitationStatus);
                    sendMailFlag = true;
                }
                else
                {
                    if (CustomerInvitation.CUI_Invitatuionstatus == 2)
                    {
                        throw new RpoBusinessException("This Email Id is already registered as Customer. Please Login");
                    }
                    else
                    {
                        CustomerInvitation.CUI_Invitatuionstatus = 1;
                        CustomerInvitation.InvitationSentCount += 1;
                        CustomerInvitation.IdJob = null;
                        sendMailFlag = true;
                    }
                }
            
            var contact = rpoContext.Contacts.Where(x => x.Id == idContact).FirstOrDefault();
            if (contact != null)
            {
                if (!string.IsNullOrEmpty(contact.Email) && !string.IsNullOrEmpty(contact.FirstName))
                {
                    if (rpoContext.Employees.Any(x => x.Email == contact.Email))
                    {
                        throw new RpoBusinessException("Can not Send Invitation. This Email Id is already registered as Snapcor User.");
                    }
                    var to = new List<KeyValuePair<string, string>>();
                    var cc = new List<KeyValuePair<string, string>>();
                    //real        
                    to.Add(new KeyValuePair<string, string>(contact.Email, contact.FirstName + contact.LastName));
                   
                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/invitationMailer.html")))
                    {
                        body = reader.ReadToEnd();
                    }
                    string Subject = "Your SnapCor Invitation from RPO, Inc. Awaits!";
                   
                    string link = "<a style=\"font-size:18px; font-weight:bold; color:#ffffff; text-decoration:none\" href=\"" + Properties.Settings.Default.FrontEndUrl + "customer-signup?email=" + contact.Email + "&IdContact=" + idContact + "\">" + "Sign Up" + "</a > ";                   
                    //string name = contact.FirstName + " " + contact.LastName;                   
                    string emailBody = body;
                    emailBody = emailBody.Replace("##InvitationLink##", link);
                    emailBody = emailBody.Replace("##Name##", contact.FirstName);
                    Mail.Send(
                         new KeyValuePair<string, string>(From.Email, From.FirstName + " " + From.LastName),                        
                        to,
                        cc,
                        Subject,
                        emailBody,
                        true
                    );
                    // }

                }
                rpoContext.SaveChanges();
                return Ok("Mail sent successfully");
            }
            else
            {
                return BadRequest();
            }
        }

    }
}