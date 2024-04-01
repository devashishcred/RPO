using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using Rpo.ApiServices.Api.Controllers.Customer.Model;

namespace Rpo.ApiServices.Api.Controllers.Customer
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
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using HtmlAgilityPack;
    using Rpo.ApiServices.Api.Controllers.Contacts;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;

    using System.Configuration;
    using Microsoft.ApplicationBlocks.Data;
    using System.Data.SqlClient;
    using NPOI.XSSF.UserModel;
    using NPOI.SS.UserModel;
    using System.Web.Configuration;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Api.Controllers.Users;
    using Rpo.ApiServices.Api.Controllers.Permissions;
    using System.Text.RegularExpressions;
    using Rpo.Identity.Core.Managers;
    using Rpo.Identity.Core.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;


    /// <summary>
    /// Class Addresses Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    //[Authorize]
    public class CustomerAccountController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Puts the password.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="oldpassword">The address.</param>
        ///   /// <param name="NewPassword">The address.</param>
        /// <returns>Object of Address.</returns>
        [Authorize]
        [RpoAuthorize]
        [Route("api/CustomerAccount/PutchangePassword")]
        public IHttpActionResult PutchangePassword([FromBody] changepassword change)
        {
            var customer= rpoContext.Customers.Where(x => x.EmailAddress.ToLower() == this.User.Identity.Name.ToLower()).FirstOrDefault();
            if (customer != null)
            {
                Tools.EncryptDecrypt ED = new Tools.EncryptDecrypt();
                if (!string.IsNullOrWhiteSpace(change.OldPassword))
                {
                    //  temp  string oldPasswordinDb = ED.Decrypt(e.LoginPassword, Convert.ToString(WebConfigurationManager.AppSettings["saltPassword"]));
                    string oldPasswordinDb = customer.LoginPassword;
                    if (!string.IsNullOrWhiteSpace(oldPasswordinDb))
                    {//temp comment
                        //if (oldPasswordinDb == change.OldPassword)
                        //{
                        //    e.LoginPassword = ED.Encrypt(change.NewPassword, Convert.ToString(WebConfigurationManager.AppSettings["saltPassword"]));
                        //}
                        if (oldPasswordinDb == change.OldPassword)
                        {
                            customer.LoginPassword = change.NewPassword;

                            using (DbContextTransaction transaction = rpoContext.Database.BeginTransaction())
                            {
                                using (RpoUserManager userManager = new RpoUserManager(new UserStore<RpoIdentityUser>(rpoContext)))
                                {
                                    RpoIdentityUser systemUser = userManager.FindByEmail(customer.EmailAddress);
                                    userManager.UserValidator = new UserValidator<RpoIdentityUser>(userManager) { AllowOnlyAlphanumericUserNames = false, RequireUniqueEmail = true };
                                    var result = userManager.ChangePassword(systemUser.Id, change.OldPassword, change.NewPassword);

                                    if (!result.Succeeded)
                                    {
                                        throw new RpoBusinessException(string.Join("!" + Environment.NewLine, result.Errors));
                                    }
                                    try
                                    {
                                        transaction.Commit();
                                    }
                                    catch
                                    {
                                        transaction.Rollback();
                                        throw;
                                    }
                                }
                                try
                                {
                                    rpoContext.Entry(customer).State = EntityState.Modified;
                                    this.rpoContext.SaveChanges();
                                    try
                                    {
                                        var to = new List<KeyValuePair<string, string>>();
                                        var cc = new List<KeyValuePair<string, string>>();
                                        //real
                                        to.Add(new KeyValuePair<string, string>(customer.EmailAddress, customer.FirstName + customer.LastName));                                        
                                        string body = string.Empty;
                                        string Subject = "SnapCor Password changed successfully!";
                                
                                        using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/ChangePasswordEmailTemplate.html")))
                                        {
                                            body = reader.ReadToEnd();
                                        }                                      
                                        string emailBody = body;
                                        emailBody = emailBody.Replace("##Name##", customer.FirstName);
                                        Mail.Send(
                                            //real
                                            new KeyValuePair<string, string>("info@rpoinc.com", "RPO" + " " + "Admin"),
                                            to,
                                            cc,
                                            Subject,
                                            emailBody,
                                            true
                                        );
                                    }
                                    catch
                                    {
                                        throw new RpoBusinessException(StaticMessages.MailSendingFailed);
                                    }
                                
                                    return Ok("Password changed Successfully");

                                }
                                catch (DbUpdateConcurrencyException)
                                {
                                    if (customer == null)
                                    {
                                        return this.NotFound();
                                    }
                                    else
                                    {
                                        throw new RpoBusinessException("Password changed Failed");
                                    }
                                }
                            }
                           
                        }
                    }
                    
                };
            }

            return this.Ok("Wrong Old Password");
        }    

        [ResponseType(typeof(Customer))]
        [HttpPost]
        [Route("api/CustomerAccount/Signup")]
        public IHttpActionResult PostCustomer(Signup signup)
        {

            CustomerInvitationStatus customerInvitationStatus = new CustomerInvitationStatus();
            customerInvitationStatus = rpoContext.CustomerInvitationStatus.Where(x => x.IdContact == signup.IdContact && x.CUI_Invitatuionstatus == 1).FirstOrDefault();

            if (customerInvitationStatus != null)
            {
                if (customerInvitationStatus.CUI_Invitatuionstatus == 1)
                {
                    var createddate = customerInvitationStatus.CreatedDate;
                    var minus48 = DateTime.UtcNow.AddHours(-48);
                    if (customerInvitationStatus.CreatedDate > DateTime.UtcNow.AddHours(-48) && customerInvitationStatus.CreatedDate < DateTime.UtcNow)
                    {
                        string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                        string error = "";
                        using (DbContextTransaction transaction = rpoContext.Database.BeginTransaction())
                        {
                            try
                            {
                                if (!ModelState.IsValid)
                                {
                                    return BadRequest(ModelState);
                                }
                                if (rpoContext.Customers.Any(e => e.EmailAddress == signup.Email))
                                {
                                    error = "You are already registered with SnapCor. Please login with your credentials to access SnapCor";
                                    throw new RpoBusinessException(error);
                                }
                                var contact = rpoContext.Contacts.Where(x => x.Id == customerInvitationStatus.IdContact).FirstOrDefault();
                                Customer customertbl = new Customer();
                                customertbl.EmailAddress = signup.Email;
                                customertbl.CustomerConsent = true;
                                customertbl.IdContcat = signup.IdContact;
                                customertbl.IdContcat = contact.Id;
                                customertbl.FirstName = contact.FirstName;
                                customertbl.LastName = contact.LastName;
                                var company = rpoContext.Companies.Find(contact.IdCompany);
                                if (company != null)
                                    customertbl.CompanyName = company.Name;
                                customertbl.RenewalDate = DateTime.UtcNow.AddYears(1);
                                customertbl.Status = 2;//registered=2
                                customertbl.LoginPassword = signup.LoginPassword;
                                customertbl.IdGroup = signup.IdGroup; //change withcustomer group
                                                                      // customertbl.CreatedBy = rpoContext.CustomerInvitationStatus.Where(x => x.IdContact == Idcontact).FirstOrDefault().CreatedBy;

                                customertbl.CreatedDate = DateTime.UtcNow;
                                customertbl.IsActive = true;
                                customertbl.LastModifiedDate = DateTime.UtcNow;
                                rpoContext.Customers.Add(customertbl);

                                using (RpoUserManager userManager = new RpoUserManager(new UserStore<RpoIdentityUser>(rpoContext)))
                                {
                                    RpoIdentityUser systemUser = new RpoIdentityUser
                                    {
                                        UserName = signup.Email,
                                        Email = signup.Email,
                                        EmailConfirmed = true,
                                        LockoutEnabled = signup.IsActive
                                    };
                                    userManager.UserValidator = new UserValidator<RpoIdentityUser>(userManager) { AllowOnlyAlphanumericUserNames = false, RequireUniqueEmail = true };

                                    var result = userManager.Create(systemUser, signup.LoginPassword);

                                    if (!result.Succeeded)
                                    {
                                        if (rpoContext.Employees.Any(e => e.Email == signup.Email))
                                        {
                                            throw new RpoBusinessException(string.Join("!" + Environment.NewLine, "User is already part Of the system"));
                                        }
                                        error = string.Join("!" + Environment.NewLine, result.Errors);

                                        throw new RpoBusinessException(string.Join("!" + Environment.NewLine, result.Errors));
                                    }
                                }
                                var group = rpoContext.Groups.FirstOrDefault(g => g.Id == signup.IdGroup);
                                if (group != null)
                                {
                                    customertbl.Permissions = group.Permissions;
                                }

                                rpoContext.SaveChanges();

                                customerInvitationStatus.CUI_Invitatuionstatus = 2;
                                rpoContext.Entry(customerInvitationStatus).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                                var invitationsentBy = rpoContext.CustomerInvitationStatus.Where(x => x.IdContact == customertbl.IdContcat).Select(y => y.CreatedBy).FirstOrDefault();
                                if (customerInvitationStatus.IdJob != null)
                                {
                                    CustomerJobAccess CustomerJobAccess = new CustomerJobAccess();
                                    CustomerJobAccess.CUI_Status = 2;
                                    CustomerJobAccess.IdCustomer = customertbl.Id;
                                    CustomerJobAccess.IdJob = (int)customerInvitationStatus.IdJob;
                                    CustomerJobAccess.CreatedDate = DateTime.UtcNow;
                                    CustomerJobAccess.CreatedBy = invitationsentBy;
                                    rpoContext.CustomerJobAccess.Add(CustomerJobAccess);
                                    rpoContext.SaveChanges();
                                }
                                else
                                {
                                    var contactsId = rpoContext.Contacts.Where(x => x.Email == signup.Email).Select(x => x.Id).ToList();

                                    var jobcontacts = rpoContext.JobContacts.Where(x => contactsId.Contains((int)x.IdContact)).Select(x => x.IdJob).ToList();
                                    List<CustomerJobAccess> lstCustomerJobAccess = new List<CustomerJobAccess>();
                                    foreach (var j in jobcontacts)
                                    {
                                        CustomerJobAccess CustomerJobAccess = new CustomerJobAccess();
                                        CustomerJobAccess.CUI_Status = 2;
                                        CustomerJobAccess.IdCustomer = customertbl.Id;
                                        CustomerJobAccess.IdJob = j;
                                        CustomerJobAccess.CreatedDate = DateTime.UtcNow;
                                        CustomerJobAccess.CreatedBy = invitationsentBy;
                                        lstCustomerJobAccess.Add(CustomerJobAccess);

                                    }
                                    rpoContext.CustomerJobAccess.AddRange(lstCustomerJobAccess);
                                    rpoContext.SaveChanges();
                                }


                                transaction.Commit();
                                CustomerNotificationSetting customerNotificationSetting = new CustomerNotificationSetting();
                                customerNotificationSetting.IdCustomer = customertbl.Id;
                                customerNotificationSetting.ProjectAccessEmail = true;
                                customerNotificationSetting.ProjectAccessInApp = true;
                                customerNotificationSetting.ViolationEmail = true;
                                customerNotificationSetting.ViolationInapp = true;                              
                                rpoContext.CustomerNotificationSettings.Add(customerNotificationSetting);
                                rpoContext.SaveChanges();
                                return Ok(new { IdJob = customerInvitationStatus.IdJob, IdContact = customerInvitationStatus.IdContact });
                            }
                            catch
                            {
                                transaction.Rollback();                               
                                throw new RpoBusinessException(error);
                            }
                        }

                    }
                    else
                    {
                        throw new RpoBusinessException("This Invitatition Link is expired, you can request the RPO team to re-send the invitation (offline process)");
                    }
                }               
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
            return this.NotFound();
        }

        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/CustomerAccount/SendEmail_customerservice")]
        [ResponseType(typeof(void))]
        public IHttpActionResult SendEmail_customerservice(Customerservice Customerservice)
        {            
            try
            {
                var FromCustomer = rpoContext.Customers.Where(e => e.EmailAddress.ToLower() == this.User.Identity.Name.ToLower()).FirstOrDefault();
                var ToName = rpoContext.Employees.Where(x => x.FirstName == "Robert" && x.LastName == "Anic").FirstOrDefault();
                var CCName = rpoContext.Employees.Where(x => x.FirstName == "Michael" && x.LastName == "Pressel").FirstOrDefault();
                var to = new List<KeyValuePair<string, string>>();
                var cc = new List<KeyValuePair<string, string>>();
                //real
                to.Add(new KeyValuePair<string, string>(ToName.Email, ToName.FirstName + ToName.LastName));
                cc.Add(new KeyValuePair<string, string>(CCName.Email, CCName.FirstName + CCName.LastName));
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/CustomerServiceMailer.html")))
                {
                    body = reader.ReadToEnd();
                }
                string Subject = "Customer Support Request from " + FromCustomer.FirstName;

            string emailBody = body;
            string data = Customerservice.Message;
            string CustomerLink = "<a href = '" + Properties.Settings.Default.FrontEndUrl + "/contacts?email=" + FromCustomer.EmailAddress + "' > " + FromCustomer.FirstName + " " + FromCustomer.LastName +"</a>";

            //string ContactNumber = FromCustomer.Contcat.WorkPhone;
            emailBody = emailBody.Replace("##EmailBody##", data);
              //  emailBody = emailBody.Replace("##Name##", FromCustomer.FirstName+" "+ FromCustomer.LastName);
                //emailBody = emailBody.Replace("##CustomerLink##", CustomerLink);
                //emailBody = emailBody.Replace("##ContactNumber##", ContactNumber);
                emailBody = emailBody.Replace("##CustomerLink##", CustomerLink);

                try
                {
                    Mail.Send(
                         new KeyValuePair<string, string>(FromCustomer.EmailAddress, FromCustomer.FirstName + " " + FromCustomer.LastName),                         
                        to,
                        cc,
                        Subject,
                        emailBody,
                        true
                    );
                }
                catch
                {
                    throw new RpoBusinessException(StaticMessages.MailSendingFailed);
                }
                // }
            }
            catch(Exception ex)
            {
                throw new RpoBusinessException(ex.Message);
            }


            return Ok("Mail sent successfully");
        }


        [Authorize]
        [RpoAuthorize]
        // [Route("api/CustomerAccount/GetCustomerDetail")]
        [HttpGet]
        public IHttpActionResult GetCustomerAccount(int id)
        {
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            var e = this.rpoContext.Customers.Where(x => x.Id == id).FirstOrDefault();
            if (e != null)
            {
                Contact contact = rpoContext.Contacts.Where(x => x.Id == e.IdContcat).FirstOrDefault();
                Address address = rpoContext.Addresses.Where(x => x.IdContact == e.IdContcat).FirstOrDefault();
                CustomerDetails customerDetails = new CustomerDetails();
                customerDetails.Id = e.Id;
                customerDetails.IdContact = e.IdContcat;
                customerDetails.FirstName = e.FirstName;
                customerDetails.LastName = e.LastName;               
                customerDetails.MobilePhone = contact.MobilePhone;
                customerDetails.WorkPhone = contact.WorkPhone;
                customerDetails.WorkPhoneExt = contact.WorkPhoneExt;
                customerDetails.CompanyName = e.CompanyName;
                customerDetails.Email = e.EmailAddress;
                if (address != null)
                {
                    customerDetails.Address1 = address.Address1;
                    customerDetails.Address2 = address.Address2;
                    customerDetails.City = address.City;
                    customerDetails.IdState = address.IdState;
                    customerDetails.Email = contact.Email;
                    customerDetails.ZipCode = address.ZipCode;
                }
                customerDetails.RenewalDate = e.RenewalDate;
                customerDetails.ContactImagePath = string.IsNullOrEmpty(contact.ContactImagePath) ? string.Empty : APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactImagePath) + "/" + Convert.ToString(contact.Id) + "_" + contact.ContactImagePath;
                customerDetails.ContactImageThumbPath = string.IsNullOrEmpty(contact.ContactImagePath) ? string.Empty : APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactImagePath) + "/" + Convert.ToString(contact.Id) + "_" + contact.ContactImagePath;

                try
                {
                    return Ok(customerDetails);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (e == null)
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                return this.StatusCode(HttpStatusCode.NoContent);
            }


        }


        /// <summary>
        /// Puts the type of the customeraccount.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="customeraccount">Type of the company.</param>
        /// <returns>IHttpActionResult.Update the company Type</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [ResponseType(typeof(CustomerDetails))]
        [Authorize]
        [RpoAuthorize]
        //[Route("api/CustomerAccount/PutCustomerAccount/{id}")]
        public IHttpActionResult PutCustomerAccount(int id, CustomerDetails customerDetails)
        {
            var Customer = rpoContext.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            Contact contact = rpoContext.Contacts.Where(x => x.Id == Customer.IdContcat).FirstOrDefault();
            Address address = rpoContext.Addresses.Where(x => x.IdContact == Customer.IdContcat).FirstOrDefault();
            if (Common.CheckUserPermission(Customer.Permissions, Enums.Permission.EditContactInfo)) //open when permission and roles are done
            {
                if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customerDetails.Id)
            {
                return BadRequest();
            }
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            Customer.LastModifiedDate = DateTime.UtcNow;
            if (Customer != null)
            {
                Customer.LastModifiedByCus = Customer.Id;
                Customer.FirstName = customerDetails.FirstName;
                Customer.LastName = customerDetails.LastName;
                rpoContext.Entry(Customer).State = EntityState.Modified;
                rpoContext.SaveChanges();
                contact.MobilePhone = customerDetails.MobilePhone;
                contact.WorkPhone = customerDetails.WorkPhone;
                contact.WorkPhoneExt = customerDetails.WorkPhoneExt;
                contact.FirstName = customerDetails.FirstName;
                contact.LastName = customerDetails.LastName;
                contact.WorkPhone = customerDetails.WorkPhone;
                contact.WorkPhoneExt = customerDetails.WorkPhoneExt;
                contact.MobilePhone = customerDetails.MobilePhone;
                contact.ContactImagePath = string.IsNullOrEmpty(customerDetails.ContactImagePath) ? string.Empty : APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactImagePath) + "/" + Convert.ToString(customerDetails.IdContact) + "_" + customerDetails.ContactImagePath;
                contact.ContactImageThumbPath = string.IsNullOrEmpty(customerDetails.ContactImagePath) ? string.Empty : APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactImagePath) + "/" + Convert.ToString(customerDetails.IdContact) + "_" + customerDetails.ContactImagePath;
                 
                rpoContext.Entry(contact).State = EntityState.Modified;
                rpoContext.SaveChanges();
                if (address != null)
                {
                    address.Address1 = customerDetails.Address1;
                    address.Address2 = customerDetails.Address2;
                    address.City = customerDetails.City;
                    address.IdState = customerDetails.IdState;
                    address.ZipCode = customerDetails.ZipCode;
                    address.LastModifiedByCus = Customer.Id;
                    rpoContext.Entry(address).State = EntityState.Modified;
                    rpoContext.SaveChanges();
                }
                else
                {
                    Address newAddress = new Address();
                    newAddress.Address1 = customerDetails.Address1;
                    newAddress.Address2 = customerDetails.Address2;
                    newAddress.City = customerDetails.City;
                    newAddress.IdState = customerDetails.IdState;
                    newAddress.ZipCode = customerDetails.ZipCode;
                    newAddress.IdAddressType = 1;
                    newAddress.IdContact = Customer.IdContcat;
                    newAddress.LastModifiedByCus = Customer.Id;
                    rpoContext.Addresses.Add(newAddress);
                    rpoContext.SaveChanges();

                }

            }
            try
            {
                rpoContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }
           
            return Ok(FormatDetails(Customer, address, contact));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

      

        /// <summary>
        /// Parameter 1 : IdContact (String)
        /// Parameter 3 : ImageFile want to Upload (File)
        /// </summary>
        /// <returns>Task&lt;HttpResponseMessage&gt;. upload the images againg the contact</returns>
        /// <exception cref="System.Web.Http.HttpResponseException"></exception>
        [Authorize]
        [RpoAuthorize]
        [Route("api/CustomerAccount/Images")]
        [ResponseType(typeof(Contact))]
        public async Task<HttpResponseMessage> PutCustomerAccountImage()
        {
            var customer = this.rpoContext.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            //if (Common.CheckUserPermission(customer.Permissions, Enums.Permission.EditContactInfo))
            //{
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            System.Collections.Specialized.NameValueCollection formData = provider.FormData;
            IList<HttpContent> files = provider.Files;

            HttpContent file1 = files[0];
            var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');

            string filename = string.Empty;
            Stream input = await file1.ReadAsStreamAsync();
            string directoryName = string.Empty;
            string URL = string.Empty;
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

            int idContact = Convert.ToInt32(formData["idContact"]);

            Contact contact = rpoContext.Contacts.Where(x => x.Id == idContact).FirstOrDefault();
            contact.ContactImagePath = thisFileName;

            rpoContext.SaveChanges();

            var path = HttpRuntime.AppDomainAppPath;
            directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.ContactImagePath));

            string directoryFileName = Convert.ToString(contact.Id) + "_" + thisFileName;
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

            string directoryThumbFileName = Convert.ToString(contact.Id) + "_Thumb_" + thisFileName;
            string thumbFileName = System.IO.Path.Combine(directoryName, directoryThumbFileName);

            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            if (File.Exists(thumbFileName))
            {
                File.Delete(thumbFileName);
            }

            using (Stream file = File.OpenWrite(thumbFileName))
            {
                input.CopyTo(file);
                file.Close();
            }

            contact.ContactImagePath = APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactImagePath) + "/" + directoryFileName;
            var response = Request.CreateResponse<Contact>(HttpStatusCode.OK, contact);
            return response;
            // }
            // else
            //{
            // throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            // }
        }

        [HttpPost]
        //[Authorize]
        //[RpoAuthorize]
        [Route("api/CustomerAccount/SendForgotPasswordMail")]
        [ResponseType(typeof(void))]
        public IHttpActionResult SendForgotPasswordMail(Signup signup)
        {           
            Customer customer = rpoContext.Customers.Where(x => x.EmailAddress == signup.Email).FirstOrDefault();
            CustomerInvitationStatus customerInvitationStatus = new CustomerInvitationStatus();
            if (customer != null)
            {
                customerInvitationStatus = rpoContext.CustomerInvitationStatus.Where(x => x.IdContact == customer.IdContcat && x.CUI_Invitatuionstatus == 2).FirstOrDefault();

                if (customerInvitationStatus != null)
                {
                    if (!string.IsNullOrEmpty(customer.EmailAddress))
                    {
                        try
                        {
                            var to = new List<KeyValuePair<string, string>>();
                            var cc = new List<KeyValuePair<string, string>>();
                            //real
                            to.Add(new KeyValuePair<string, string>(customer.EmailAddress, customer.FirstName + customer.LastName));                                                    
                            string body = string.Empty;
                            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/Passwrod-ResetMailTemplate.html")))
                            {
                                body = reader.ReadToEnd();
                            }
                            string Subject = "SnapCor Password Reset Requested";
                           // string link = "<a style=\"padding: 10px 40px;background: #2DB32D; text-align: center; height: 50px;line-height: 50px; outline: 0;border: 0; border-radius: 6px; color: #fff; font-size: 16px; font-weight: bold; text-decoration: none\" href=\"" + Properties.Settings.Default.FrontEndUrl + "set-password?email=" + customer.EmailAddress + "\">" + "Reset my Password" + "</a >";
                            string link = "<a style=\"font-size:18px; font-weight:bold; color:#ffffff; text-decoration:none\" href=\"" + Properties.Settings.Default.FrontEndUrl + "set-password?email=" + customer.EmailAddress + "\">" + "Reset my Password" + "</a >";

                            //string name = customer.FirstName + " " + customer.LastName;
                            string emailBody = body;
                            emailBody = emailBody.Replace("##ResetLink##", link);
                            emailBody = emailBody.Replace("##Name##", customer.FirstName);
                            Mail.Send(
                                new KeyValuePair<string, string>("info@rpoinc.com", "RPO" + " " + "Admin"),
                                to,
                                cc,
                                Subject,
                                emailBody,
                                true
                            );
                        }
                        catch
                        {
                            throw new RpoBusinessException(StaticMessages.MailSendingFailed);
                        }
                        try
                        {
                            CustomerPasswordReset c = rpoContext.CustomerPasswordResets.Where(x => x.EmailAddress == customer.EmailAddress && x.IsPasswordchanged == false).FirstOrDefault();

                            if (c == null)
                            {
                                CustomerPasswordReset CustomerPasswordReset = new CustomerPasswordReset();
                                CustomerPasswordReset.EmailAddress = customer.EmailAddress;
                                CustomerPasswordReset.IdCustomer = customer.Id;
                                CustomerPasswordReset.IsPasswordchanged = false;
                                CustomerPasswordReset.customer = customer;
                                CustomerPasswordReset.RequestDate = DateTime.UtcNow;
                                CustomerPasswordReset.PasswordChangedDate = DateTime.UtcNow;
                                rpoContext.CustomerPasswordResets.Add(CustomerPasswordReset);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                c.RequestDate = DateTime.UtcNow;
                                rpoContext.Entry(c).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        catch { throw new RpoBusinessException("Password reset data save failed"); }
                        return Ok("Password Reset Link Has Been Sent On Your Registered Mail Id");
                    }
                    else
                    {
                        throw new RpoBusinessException("This Email Id has no email address");
                    }
                }
                throw new RpoBusinessException("This Email Id is not registered");
            }
            else
            {
                throw new RpoBusinessException("This Email Id is not registered");                
            }
        }
        [HttpPost]
        //[Authorize]
        //[RpoAuthorize]
        [Route("api/CustomerAccount/SendWelcomeMail")]
        [ResponseType(typeof(void))]
        public IHttpActionResult SendWelcomeMail(Signup signup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var From = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            Customer customer = rpoContext.Customers.Where(x => x.EmailAddress == signup.Email).FirstOrDefault();
            CustomerInvitationStatus customerInvitationStatus = new CustomerInvitationStatus();
            if (customer != null)
            {
                customerInvitationStatus = rpoContext.CustomerInvitationStatus.Where(x => x.IdContact == customer.IdContcat && x.CUI_Invitatuionstatus == 2).FirstOrDefault();
            }
            if (customerInvitationStatus != null)
            {
                if (!string.IsNullOrEmpty(customer.EmailAddress))
                {
                    var to = new List<KeyValuePair<string, string>>();
                    var cc = new List<KeyValuePair<string, string>>();
                    //real
                    to.Add(new KeyValuePair<string, string>(customer.EmailAddress, customer.FirstName + customer.LastName));
                    
                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/welcomeMailer.html")))
                    {
                        body = reader.ReadToEnd();
                    }
                    string Subject = "Welcome to SnapCor!";
                    string link = "<a style=\"font-size:18px; font-weight:bold; color:#ffffff; text-decoration:none\" href=\"" + Properties.Settings.Default.FrontEndUrl + "customer-login" + "\">" + "Log In" + "</a > ";
                    
                    string emailBody = body;
                    emailBody = emailBody.Replace("##PlandetailLink##", link);
                    emailBody = emailBody.Replace("##Name##", customer.FirstName);
                    try
                    {
                        Mail.Send(
                            //new KeyValuePair<string, string>(From.Email, From.FirstName + " " +From.LastName),
                            new KeyValuePair<string, string>("info@rpoinc.com", "RPO" + " " + "Admin"),
                            to,
                            cc,
                            Subject,
                            emailBody,
                            true
                        );
                    }
                    catch
                    {
                        throw new RpoBusinessException(StaticMessages.MailSendingFailed);
                    }
                    return Ok("Welcome Mail Sent Successfully");
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return this.Ok("This Email Id is not registered");
            }
        }

        [HttpPost]
        //[Authorize]
        //[RpoAuthorize]
        [Route("api/CustomerAccount/GiveProjectAccessSendMail/{IdContact}/{IdJob}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult GiveProjectAccessSendMail(int IdContact, int IdJob)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
           
            string ContactEmail = rpoContext.Contacts.Where(x => x.Id == IdContact).FirstOrDefault().Email;
            Customer customer = rpoContext.Customers.Where(x => x.EmailAddress == ContactEmail).FirstOrDefault();
            if (customer == null)
            {
                return this.NotFound(); //if not found then call send invitation mail api
            }
            else
            {
                if (customer.CustomerConsent == true)
                {
                    CustomerNotificationSetting customerNotificationSettings = rpoContext.CustomerNotificationSettings.Where(x => x.IdCustomer == customer.Id).FirstOrDefault();
                    Job jobs = rpoContext.Jobs.Find(IdJob);
                    string jobAddress = jobs != null && jobs.RfpAddress != null ? (!string.IsNullOrEmpty(jobs.RfpAddress.HouseNumber) ? jobs.RfpAddress.HouseNumber : string.Empty)
                            + ", " + (!string.IsNullOrEmpty(jobs.RfpAddress.Street) ? jobs.RfpAddress.Street : string.Empty) : string.Empty;
                    var boroughDescription = rpoContext.Boroughes.Where(x => x.Id == jobs.IdBorough).Select(y => y.Description).FirstOrDefault();

                    if (customerNotificationSettings.ProjectAccessEmail == true)
                    {
                        if (!string.IsNullOrEmpty(customer.EmailAddress))
                        {
                            var to = new List<KeyValuePair<string, string>>();
                            var cc = new List<KeyValuePair<string, string>>();
                            //real
                            to.Add(new KeyValuePair<string, string>(customer.EmailAddress, customer.FirstName + customer.LastName));
                              
                            string body = string.Empty;
                            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/ProjectAccessMailTemplate.html")))
                            {
                                body = reader.ReadToEnd();
                            }
                            string Subject = "You now have access to a new project in SnapCor.";                                           
                            string projectId = "<a href=\"" + Properties.Settings.Default.FrontEndUrl + "job/" + IdJob + "/application;idJobAppType=" + 1 + "\">" + IdJob + "</a > ";
                            string link = "<a style=\"font-size:18px; font-weight:bold; color:#ffffff; text-decoration:none\" href=\"" + Properties.Settings.Default.FrontEndUrl + "customer-login" + "\">" + "Log In" + "</a > ";
                          
                            string emailBody = body;
                            emailBody = emailBody.Replace("##Name##", customer.FirstName);
                            emailBody = emailBody.Replace("##EmailBody##", projectId);
                            emailBody = emailBody.Replace("##JobAddress##", jobAddress + ", " + boroughDescription);
                            emailBody = emailBody.Replace("##SignIn##", link);
                            try
                            {
                                Mail.Send(
                                    new KeyValuePair<string, string>(employee.Email, employee.FirstName + " " + employee.LastName),
                                    to,
                                    cc,
                                    Subject,
                                    emailBody,
                                    true
                                );
                            }
                            catch
                            {
                                throw new RpoBusinessException(StaticMessages.MailSendingFailed);
                            }
                        }
                    }                  
                    if (customerNotificationSettings.ProjectAccessInApp == true)
                    {
                        try
                        {
                            string newJobaccessSetting = string.Empty;

                            string link = "<a href=\"" + Properties.Settings.Default.FrontEndUrl + "job/" + IdJob + "/application;idJobAppType=" + 1 + "\">" + IdJob + "</a > ";
                            newJobaccessSetting = InAppNotificationMessage.CustomerJobaccess
                                        .Replace("##jobnumber##", link)
                                        .Replace("##jobaddress##", "<b>" + jobs != null && jobs.RfpAddress != null ? (!string.IsNullOrEmpty(jobs.RfpAddress.HouseNumber) ? jobs.RfpAddress.HouseNumber : string.Empty)
                                        + " " + (!string.IsNullOrEmpty(jobs.RfpAddress.Street) ? jobs.RfpAddress.Street : string.Empty) + " " +
                                        (jobs.RfpAddress.Borough != null ? jobs.RfpAddress.Borough.Description : JobHistoryMessages.NoSetstring) + " " +
                                        (!string.IsNullOrEmpty(jobs.RfpAddress.ZipCode) ? jobs.RfpAddress.ZipCode : string.Empty) + " " +
                                        (!string.IsNullOrEmpty(jobs.SpecialPlace) ? "-" + jobs.SpecialPlace : string.Empty + "</b>") : JobHistoryMessages.NoSetstring);

                            Common.SendCustomerInAppNotifications(customer.Id, newJobaccessSetting, "/job/" + IdJob + "/scope");
                        }
                        catch(Exception ex)
                        { throw new RpoBusinessException(ex.Message); }
                    }
                }

                return Ok("Project Access Mail Sent Successfully");
                // }           

            }


        }

        [Route("api/CustomerAccount/PutResetPassword")]
        public IHttpActionResult PutResetPassword([FromBody] changepassword change)
        {
            var e = rpoContext.Customers.Where(x => x.EmailAddress.ToLower() == change.strId).FirstOrDefault();
            CustomerPasswordReset customerPasswordResets = new CustomerPasswordReset();
            if (e != null)
            {
                customerPasswordResets = rpoContext.CustomerPasswordResets.Where(x => x.EmailAddress == change.strId && x.IsPasswordchanged == false).FirstOrDefault();

            }
            if (customerPasswordResets != null)
            {
                Tools.EncryptDecrypt ED = new Tools.EncryptDecrypt();
                if (!string.IsNullOrWhiteSpace(e.EmailAddress))
                {


                    using (DbContextTransaction transaction = rpoContext.Database.BeginTransaction())
                    {
                        using (RpoUserManager userManager = new RpoUserManager(new UserStore<RpoIdentityUser>(rpoContext)))
                        {
                            RpoIdentityUser systemUser = userManager.FindByEmail(e.EmailAddress);
                            userManager.UserValidator = new UserValidator<RpoIdentityUser>(userManager) { AllowOnlyAlphanumericUserNames = false, RequireUniqueEmail = true };
                            //   RpoIdentityUser systemUser = userManager.FindByEmail(e.Email);                           
                            var result = userManager.ChangePassword(systemUser.Id, e.LoginPassword, change.NewPassword);

                            if (!result.Succeeded)
                            {
                                throw new RpoBusinessException(string.Join("!" + Environment.NewLine, result.Errors));
                            }
                            try
                            {
                                transaction.Commit();
                                e.LoginPassword = change.NewPassword;
                                rpoContext.Entry(e).State = EntityState.Modified;
                                this.rpoContext.SaveChanges();
                                customerPasswordResets.IsPasswordchanged = true;
                                customerPasswordResets.PasswordChangedDate = DateTime.UtcNow;
                                rpoContext.Entry(customerPasswordResets).State = EntityState.Modified;
                                this.rpoContext.SaveChanges();
                                return Ok("Password changed Successfully");
                            }
                            catch
                            {
                                transaction.Rollback();
                                throw;
                            }

                        }

                    }
                }

            }
            throw new RpoBusinessException("You have already used this link for password reset.Please do forgot password process again");
        }

        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/CustomerAccount/SendEmail_ProposalRequest")]
        [ResponseType(typeof(void))]
        public IHttpActionResult SendEmail_ProposalRequest(ProposalReuest proposalReuest)
        {

            var FromCustomer = rpoContext.Customers.Where(e => e.EmailAddress.ToLower() == this.User.Identity.Name.ToLower()).FirstOrDefault();
           
            var ToName = rpoContext.Employees.Where(x => x.FirstName == "Robert" && x.LastName == "Anic").FirstOrDefault();
            var CCName = rpoContext.Employees.Where(x => x.FirstName == "Michael" && x.LastName == "Pressel").FirstOrDefault();
            var to = new List<KeyValuePair<string, string>>();
            var cc = new List<KeyValuePair<string, string>>();
            //real
            to.Add(new KeyValuePair<string, string>(ToName.Email, ToName.FirstName + ToName.LastName));
            cc.Add(new KeyValuePair<string, string>(CCName.Email, CCName.FirstName + CCName.LastName));
            
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/ProposalRequestMailTemplate.html")))
            {
                body = reader.ReadToEnd();
            }
            //string Subject = "New Request for Proposal Received - " + proposalReuest.ProposalAddress;
            string Subject = "New Proposal Request by - " + FromCustomer.FirstName;
            string emailBody = body;
            //string CutomerNamelink = "<a href = '" + Properties.Settings.Default.FrontEndUrl + "/contacts?email=" + FromCustomer.EmailAddress + "' > " + FromCustomer.FirstName + " " + FromCustomer.LastName + "</a>";
            string CutomerNamelink = "<a href = '" + Properties.Settings.Default.FrontEndUrl + "/contacts?email=" + FromCustomer.EmailAddress + "' > " + FromCustomer.FirstName + FromCustomer.LastName +"</a>";

            emailBody = emailBody.Replace("##CutomerNamelink##", CutomerNamelink);
           // emailBody = emailBody.Replace("##CompanyName##", ", " + FromCustomer.CompanyName);
            emailBody = emailBody.Replace("##ProposalTitle##", proposalReuest.ProposalnName);
            emailBody = emailBody.Replace("##ProposalDescription##", proposalReuest.ProposalDescription);
            emailBody = emailBody.Replace("##ProposalAddress##", proposalReuest.ProposalAddress);
            emailBody = emailBody.Replace("##ContactNumber##", FromCustomer.Contcat.MobilePhone);
            try
            {

                Mail.Send(
                    //real                    
                    new KeyValuePair<string, string>(FromCustomer.EmailAddress, FromCustomer.FirstName + " " + FromCustomer.LastName),                    
                    to,
                    cc,
                    Subject,
                    emailBody,
                    true
                );
            }
            catch
            {
                throw new RpoBusinessException(StaticMessages.MailSendingFailed);
            }
            // }


            return Ok("Mail sent successfully");
        }

        [ResponseType(typeof(CustomerDetails))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/CustomerAccount/PutCustomerConsent/{NotificationConsent}")]
        public IHttpActionResult PutCustomerConsent(bool NotificationConsent)
        {
            var Customer = rpoContext.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            if (Customer != null)
            {
                Customer.CustomerConsent = NotificationConsent;
                rpoContext.Entry(Customer).State = EntityState.Modified;
                rpoContext.SaveChanges();
                return this.Ok("Notification Settings Saved Successfully");
            }
            else
            {              
                throw new RpoBusinessException("Customer Not Found");
            }
        }


        [ResponseType(typeof(CustomerDetails))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/CustomerAccount/GetCustomerConsent")]
        public IHttpActionResult GetCustomerConsent()
        {
            var Customer = rpoContext.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            if (Customer != null)
            {
                return this.Ok(Customer.CustomerConsent.ToString());
            }            
            throw new RpoBusinessException("Customer Not Found");
        }

 
        private bool CustomerExists(int id)
        {
            return rpoContext.Customers.Count(e => e.Id == id) > 0;
        }
        private CustomerDetails FormatDetails(Customer customer, Address address, Contact contact)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new CustomerDetails
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.EmailAddress,
                IdContact = customer.IdContcat,
                CompanyName = customer.CompanyName               
            };
        }


    }

    public class changepassword
    {
        public int Id { get; set; }
        public string strId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class Signup
    {
        public int IdContact { get; set; }
        public string Email { get; set; }
        public string LoginPassword { get; set; }
        public bool IsActive { get; set; }
        public int IdGroup { get; set; }


    }
    public class Customerservice
    {
        public string Message { get; set; }

    }
    public class ProposalReuest
    {
        public string ProposalnName { get; set; }
        public string ProposalAddress { get; set; }
        public string ProposalDescription { get; set; }

    }



}