// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-31-2018
// ***********************************************************************
// <copyright file="ContactsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************


/// <summary>
/// The Controllers namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Rpo.ApiServices.Api.Controllers.Contacts;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Filters;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Model.Models.Enums;
    using Addresses;
    using Microsoft.ApplicationBlocks.Data;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Data.Entity.Validation;
    using SystemSettings;
    using NPOI.XSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Class ContactsController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RpoAuthorize(FunctionGrantType.Contacts)]
    public class ContactsController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();

        private RpoContext newdb = new RpoContext();

        /// <summary>
        /// Maps the contact to contact dto.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns>ContactDTO.</returns>
        private ContactDTO MapContactToContactDTO(Contact c)
        {
            return new ContactDTO()
            {
                Id = c.Id,
                IdPrefix = c.IdPrefix,
                Prefix = c.IdPrefix != null ? rpoContext.Prefixes.Find(c.IdPrefix).Name : null,
                FirstName = c.FirstName,
                LastName = c.LastName,
                IdCompany = c.IdCompany,
                Company = c.IdCompany != null ? rpoContext.Companies.Find(c.IdCompany).Name : null,
                Address = c.Addresses.Any() ? c.Addresses.FirstOrDefault().Address1 : null,
                City = c.Addresses.Any() ? c.Addresses.FirstOrDefault().City : null,
                State = c.Addresses.Any() ? c.Addresses.FirstOrDefault().State.Acronym : null,
                Zip = c.Addresses.Any() ? c.Addresses.FirstOrDefault().ZipCode : null,
                Phone = c.Addresses.Any() ? c.Addresses.FirstOrDefault().Phone : null,
                WorkPhone = c.WorkPhone,
                CellNumber = c.MobilePhone,
                Email = c.Email,
                License = c.ContactLicenses.Any() ? c.ContactLicenses.FirstOrDefault().ContactLicenseType.Name : null,
                LicenseNumber = c.ContactLicenses.Any() ? c.ContactLicenses.FirstOrDefault().Number : null,
                Notes = c.Notes,
                Suffix = c.Suffix?.Description,
                IdSuffix = c.IdSuffix,
                ImageUrl = c.ContactImagePath,
                ImageThumbUrl = c.ContactImageThumbPath
            };
        }

        /// <summary>
        /// Gets the contacts.
        /// </summary>
        /// <remarks>List of contacts in detail.</remarks>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Get the contact list</returns>
        [Authorize]
        [RpoAuthorize]
        //  [ResponseType(typeof(List<ContactList>))]
        public HttpResponseMessage GetContacts([FromUri] ContactDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewContact)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddContact)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteContact))
            {
                //var contacts = rpoContext.Contacts.Include(p => p.ContactLicenses).AsQueryable();


                string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();
                string Firstname = string.Empty;
                string Lastname = string.Empty;
                if (dataTableParameters.GlobalSearchType != null && dataTableParameters.GlobalSearchType > 0)
                {
                    string[] strArray = dataTableParameters.GlobalSearchText.Split(' ');
                    if (strArray.Count() > 0)
                    {
                        Firstname = strArray[0].Trim();
                    }
                    if (strArray.Count() > 1)
                    {
                        Lastname = strArray[1].Trim();
                    }
                }

                DataSet ds = new DataSet();

                SqlParameter[] spParameter = new SqlParameter[2];

                spParameter[0] = new SqlParameter("@FirstName", SqlDbType.NVarChar, 50);
                spParameter[0].Direction = ParameterDirection.Input;
                spParameter[0].Value = Firstname;

                spParameter[1] = new SqlParameter("@LastName", SqlDbType.NVarChar, 50);
                spParameter[1].Direction = ParameterDirection.Input;
                spParameter[1].Value = Lastname;

                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "Contacts_List", spParameter);

                ds.Tables[0].TableName = "Data";


                return Request.CreateResponse(HttpStatusCode.OK, ds);
               
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        /// Get the contact List
        /// </summary>
        /// <remarks>To get the list of all contacts</remarks>
        /// <param name="dataTableParameters"></param>
        /// <returns>Return Contact with pagingation List</returns>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/ContactListPost")]
        public IHttpActionResult ContactsList(ContactDataTableParameters dataTableParameters)

        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewContact)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddContact)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteContact))
            {               
                string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();
                string Firstname = string.Empty;
                string Lastname = string.Empty;
                if (dataTableParameters != null && dataTableParameters.GlobalSearchType != null && dataTableParameters.GlobalSearchText != null && dataTableParameters.GlobalSearchType > 0)
                {
                    string[] strArray = dataTableParameters.GlobalSearchText.Split(' ');
                    if (strArray.Count() > 0)
                    {
                        Firstname = strArray[0].Trim();
                    }
                    if (strArray.Count() > 1)
                    {
                        Lastname = strArray[1].Trim();
                    }
                }

                DataSet ds = new DataSet();

                SqlParameter[] spParameter = new SqlParameter[11];

                spParameter[0] = new SqlParameter("@FirstName", SqlDbType.NVarChar, 50);
                spParameter[0].Direction = ParameterDirection.Input;
                spParameter[0].Value = Firstname;

                spParameter[1] = new SqlParameter("@LastName", SqlDbType.NVarChar, 50);
                spParameter[1].Direction = ParameterDirection.Input;
                spParameter[1].Value = Lastname;

                spParameter[2] = new SqlParameter("@IdContactLicenseType", SqlDbType.Int);
                spParameter[2].Direction = ParameterDirection.Input;
                spParameter[2].Value = dataTableParameters.IdContactLicenseType != null ? dataTableParameters.IdContactLicenseType : null;

                spParameter[3] = new SqlParameter("@PageIndex", SqlDbType.Int);
                spParameter[3].Direction = ParameterDirection.Input;
                spParameter[3].Value = dataTableParameters.Start;

                spParameter[4] = new SqlParameter("@PageSize", SqlDbType.Int);
                spParameter[4].Direction = ParameterDirection.Input;
                spParameter[4].Value = dataTableParameters.Length;

                spParameter[5] = new SqlParameter("@Column", SqlDbType.NVarChar, 50);
                spParameter[5].Direction = ParameterDirection.Input;
                spParameter[5].Value = dataTableParameters.OrderedColumn != null ? dataTableParameters.OrderedColumn.Column : null;

                spParameter[6] = new SqlParameter("@Dir", SqlDbType.VarChar, 50);
                spParameter[6].Direction = ParameterDirection.Input;
                spParameter[6].Value = dataTableParameters.OrderedColumn != null ? dataTableParameters.OrderedColumn.Dir : null;

                spParameter[7] = new SqlParameter("@Search", SqlDbType.NVarChar, 50);
                spParameter[7].Direction = ParameterDirection.Input;
                spParameter[7].Value = !string.IsNullOrEmpty(dataTableParameters.Search) ? dataTableParameters.Search : string.Empty;

                spParameter[8] = new SqlParameter("@IdCompany", SqlDbType.Int);
                spParameter[8].Direction = ParameterDirection.Input;
                spParameter[8].Value = dataTableParameters.IdCompany != null ? dataTableParameters.IdCompany : null;

                spParameter[9] = new SqlParameter("@Individual", SqlDbType.Int);
                spParameter[9].Direction = ParameterDirection.Input;
                spParameter[9].Value = dataTableParameters.Individual != null ? dataTableParameters.Individual : null;

                spParameter[10] = new SqlParameter("@RecordCount", SqlDbType.Int);
                spParameter[10].Direction = ParameterDirection.Output;

                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "Contacts_Pagination_List", spParameter);
                int totalrecord = 0;
                int Recordcount = 0;
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    totalrecord = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                    Recordcount = int.Parse(spParameter[10].SqlValue.ToString());
                }
                if (ds.Tables.Count > 1)
                {
                    ds.Tables[1].TableName = "Data";
                }
                return Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Start,
                    RecordsFiltered = Recordcount,
                    RecordsTotal = totalrecord,
                    Data = ds.Tables[1]
                });

            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        private ContactDTO FormatContact(Contact c)
        {
            Address address = c.Addresses != null ? c.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
            address = address != null ? address : c.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault();

            //ContactLicense contactLicense = (from d in rpoContext.ContactLicenses where d.IdContact == c.Id select d).FirstOrDefault() != null ? (from d in rpoContext.ContactLicenses where d.IdContact == c.Id select d).FirstOrDefault() : null;
            ContactLicense contactLicense = c.ContactLicenses != null ? c.ContactLicenses.FirstOrDefault() : null;
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            ContactDTO contactDTO = new ContactDTO();
            contactDTO.Id = c.Id;
            contactDTO.FirstName = c.FirstName;
            contactDTO.LastName = c.LastName;
            contactDTO.IdCompany = c.IdCompany;
            contactDTO.Company = c.Company != null ? c.Company.Name : null;
            contactDTO.IdPrefix = c.IdPrefix != null ? c.IdPrefix : null;
            contactDTO.Prefix = c.Prefix != null ? c.Prefix.Name : string.Empty;
            contactDTO.OtherPhone = c.OtherPhone;
            contactDTO.Ext = c.WorkPhoneExt;
            contactDTO.Address = address != null ? address.Address1 : string.Empty;
            contactDTO.Address2 = address != null ? address.Address2 : string.Empty;           
            contactDTO.City = address != null ? address.City : string.Empty;          
            contactDTO.State = address != null ? address.State.Acronym : string.Empty;         
            contactDTO.Zip = address != null ? address.ZipCode : string.Empty;          
            contactDTO.Phone = address != null ? address.Phone : string.Empty;            
            contactDTO.WorkPhone = c.WorkPhone;
            contactDTO.CellNumber = c.MobilePhone;
            contactDTO.Email = c.Email;
            contactDTO.License = contactLicense != null && contactLicense.ContactLicenseType != null ? contactLicense.ContactLicenseType.Name : string.Empty;
            contactDTO.IdContactLicenseType = contactLicense != null ? contactLicense.IdContactLicenseType : 0;
            contactDTO.LicenseNumber = contactLicense != null ? contactLicense.Number : string.Empty;
            contactDTO.Notes = c.Notes;
            contactDTO.IdContactTitle = c.IdContactTitle;
            contactDTO.ContactTitle = c.ContactTitle != null ? c.ContactTitle.Name : null;
            contactDTO.IdSuffix = c.IdSuffix;
            contactDTO.Suffix = c.Suffix == null ? null : c.Suffix.Description;
            contactDTO.ImageUrl = string.IsNullOrEmpty(c.ContactImagePath) ? string.Empty : APIUrl + "/" + Properties.Settings.Default.ContactImagePath + "/" + c.Id + "_" + c.ContactImagePath;
            contactDTO.ImageThumbUrl = string.IsNullOrEmpty(c.ContactImagePath) ? string.Empty : APIUrl + "/" + Properties.Settings.Default.ContactImagePath + "/" + c.Id + "_" + c.ContactImagePath;
            contactDTO.IsActive = c.IsActive;
            return contactDTO;
        }
        private ContactDTO FormatContactDemo(Contact c)
        {
            Address address = c.Addresses != null ? c.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
            address = address != null ? address : c.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault();           
            ContactLicense contactLicense = c.ContactLicenses != null ? c.ContactLicenses.FirstOrDefault() : null;
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            ContactDTO contactDTO = new ContactDTO();
            contactDTO.Id = c.Id;
            contactDTO.FirstName = c.FirstName;
            contactDTO.LastName = c.LastName;
            contactDTO.IdCompany = c.IdCompany;
            contactDTO.Company = c.Company != null ? c.Company.Name : null;
            contactDTO.IdPrefix = c.IdPrefix != null ? c.IdPrefix : null;
            contactDTO.Prefix = c.Prefix != null ? c.Prefix.Name : string.Empty;
            contactDTO.OtherPhone = c.OtherPhone;
            contactDTO.Ext = c.WorkPhoneExt;
            contactDTO.Address = address != null ? address.Address1 : string.Empty;
            contactDTO.Address2 = address != null ? address.Address2 : string.Empty;
            
            contactDTO.City = address != null ? address.City : string.Empty;
            
            contactDTO.State = address != null ? address.State.Acronym : string.Empty;
           
            contactDTO.Zip = address != null ? address.ZipCode : string.Empty;
            
            contactDTO.Phone = address != null ? address.Phone : string.Empty;           
            contactDTO.WorkPhone = c.WorkPhone;
            contactDTO.CellNumber = c.MobilePhone;
            contactDTO.Email = c.Email;
            contactDTO.License = contactLicense != null && contactLicense.ContactLicenseType != null ? contactLicense.ContactLicenseType.Name : string.Empty;
            contactDTO.IdContactLicenseType = contactLicense != null ? contactLicense.IdContactLicenseType : 0;
            contactDTO.LicenseNumber = contactLicense != null ? contactLicense.Number : string.Empty;
            contactDTO.Notes = c.Notes;
            contactDTO.IdContactTitle = c.IdContactTitle;
            contactDTO.ContactTitle = c.ContactTitle != null ? c.ContactTitle.Name : null;
            contactDTO.IdSuffix = c.IdSuffix;
            contactDTO.Suffix = c.Suffix == null ? null : c.Suffix.Description;
            contactDTO.ImageUrl = string.IsNullOrEmpty(c.ContactImagePath) ? string.Empty : APIUrl + "/" + Properties.Settings.Default.ContactImagePath + "/" + c.Id + "_" + c.ContactImagePath;
            contactDTO.ImageThumbUrl = string.IsNullOrEmpty(c.ContactImagePath) ? string.Empty : APIUrl + "/" + Properties.Settings.Default.ContactImagePath + "/" + c.Id + "_" + c.ContactImagePath;
            return contactDTO;
        }

        /// <summary>
        /// Gets the contact.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult. Get the contact detail </returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ContactDTODetail))]
        public IHttpActionResult GetContact(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewContact)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddContact)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteContact))
            {
                Contact contactresponse = rpoContext.Contacts
                .Include("Prefix")
                .Include("Suffix")
                .Include("Company")
                .Include("ContactTitle")
                //.Include("Addresses")
                .Include("ContactLicenses")
                .Include("Documents")
                .Include("LastModifiedByEmployee")
                .Include("CreatedByEmployee")
                .Where(x => x.Id == id).FirstOrDefault();
                if (contactresponse == null)
                {
                    return this.NotFound();
                }

                string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
               string ContactEmail= rpoContext.Contacts.Where(x => x.Id == id).FirstOrDefault().Email;
                CustomerInvitationStatus customerInvitationStatus = rpoContext.CustomerInvitationStatus.Where(x => x.EmailAddress == ContactEmail).FirstOrDefault();
                
                int invitation = 0;
                if(customerInvitationStatus!=null)
                {
                    invitation = customerInvitationStatus.CUI_Invitatuionstatus;
                }
                return Ok(new ContactDTODetail
                {
                    Id = contactresponse.Id,
                    PersonalType = ((int)contactresponse.PersonalType).ToString(),
                    IdPrefix = contactresponse.IdPrefix,
                    Prefix = contactresponse.IdPrefix != null ? contactresponse.Prefix.Name : null,
                    FirstName = contactresponse.FirstName,
                    MiddleName = contactresponse.MiddleName,
                    LastName = contactresponse.LastName,
                    IdCompany = contactresponse.IdCompany,
                    Company = contactresponse.Company != null ? contactresponse.Company.Name : null,
                    IdContactTitle = contactresponse.IdContactTitle,
                    ContactTitle = contactresponse.IdContactTitle != null ? contactresponse.ContactTitle.Name : null,
                    BirthDate = contactresponse.BirthDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactresponse.BirthDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactresponse.BirthDate,
                    CompanyAddresses = rpoContext.Addresses.Where(d => d.Id == (contactresponse.IdPrimaryCompanyAddress != null ? contactresponse.IdPrimaryCompanyAddress : null)).Select(a => new Addresses.AddressDTO
                    {
                        Id = a.Id,
                        Address1 = a.Address1,
                        Address2 = a.Address2,
                        AddressType = a.AddressType,
                        IdAddressType = a.IdAddressType,
                        City = a.City,
                        IdState = a.IdState,
                        State = a.State.Acronym,
                        ZipCode = a.ZipCode,
                        Phone = a.Phone,
                        IsMainAddress = a.IsMainAddress,
                    }).OrderBy(x => x.AddressType.DisplayOrder),
                    Addresses = rpoContext.Addresses.Where(d => d.IdContact == contactresponse.Id).Select(a => new Addresses.AddressDTO
                    {
                        Id = a.Id,
                        Address1 = a.Address1,
                        Address2 = a.Address2,
                        AddressType = a.AddressType,
                        IdAddressType = a.IdAddressType,
                        City = a.City,
                        IdState = a.IdState,
                        State = a.State.Acronym,
                        ZipCode = a.ZipCode,
                        Phone = a.Phone,
                        IsMainAddress = a.IsMainAddress,
                    }).OrderBy(x => x.AddressType.DisplayOrder),
                    WorkPhone = contactresponse.WorkPhone,
                    WorkPhoneExt = contactresponse.WorkPhoneExt,
                    Ext = contactresponse.WorkPhoneExt,
                    MobilePhone = contactresponse.MobilePhone,
                    OtherPhone = contactresponse.OtherPhone,
                    Email = contactresponse.Email,
                    ContactLicenses = contactresponse.ContactLicenses != null && contactresponse.ContactLicenses.Count > 0 ? contactresponse.ContactLicenses.Select(cl => new ContactLicenseDTODetail
                    {
                        Id = cl.Id,
                        ContactLicenseType = cl.ContactLicenseType,
                        IdContactLicenseType = cl.IdContactLicenseType,
                        ExpirationLicenseDate = cl.ExpirationLicenseDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(cl.ExpirationLicenseDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : cl.ExpirationLicenseDate,
                        Number = cl.Number
                    }) : null,
                    Notes = contactresponse.Notes,
                    Documents = contactresponse.Documents != null && contactresponse.Documents.Count > 0 ? contactresponse.Documents
                    .Select(d => new ContactDocument
                    {
                        Name = d.Name,
                        Id = d.Id,
                        DocumentPath = APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactDocumentPath) + "/" + Convert.ToString(d.Id) + "_" + d.DocumentPath
                    }) : null,
                    DocumentsToDelete = new int[0],
                    Suffix = contactresponse.Suffix?.Description,
                    IdSuffix = contactresponse.IdSuffix,
                    ImageUrl = string.IsNullOrEmpty(contactresponse.ContactImagePath) ? string.Empty : APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactImagePath) + "/" + Convert.ToString(contactresponse.Id) + "_" + contactresponse.ContactImagePath,
                    ImageThumbUrl = string.IsNullOrEmpty(contactresponse.ContactImagePath) ? string.Empty : APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactImagePath) + "/" + Convert.ToString(contactresponse.Id) + "_" + contactresponse.ContactImagePath,
                    CreatedBy = contactresponse.CreatedBy,
                    LastModifiedBy = contactresponse.LastModifiedBy != null ? contactresponse.LastModifiedBy : contactresponse.CreatedBy,
                    CreatedByEmployeeName = contactresponse.CreatedByEmployee != null ? contactresponse.CreatedByEmployee.FirstName + " " + contactresponse.CreatedByEmployee.LastName : string.Empty,
                    LastModifiedByEmployeeName = contactresponse.LastModifiedByEmployee != null ? contactresponse.LastModifiedByEmployee.FirstName + " " + contactresponse.LastModifiedByEmployee.LastName : (contactresponse.CreatedByEmployee != null ? contactresponse.CreatedByEmployee.FirstName + " " + contactresponse.CreatedByEmployee.LastName : string.Empty),
                    CreatedDate = contactresponse.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactresponse.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactresponse.CreatedDate,
                    LastModifiedDate = contactresponse.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactresponse.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (contactresponse.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactresponse.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactresponse.CreatedDate),
                    IdPrimaryCompanyAddress = contactresponse.IdPrimaryCompanyAddress != null ? contactresponse.IdPrimaryCompanyAddress : null,
                    IsPrimaryCompanyAddress = contactresponse.IsPrimaryCompanyAddress != null ? contactresponse.IsPrimaryCompanyAddress : null,
                    IsActive = contactresponse.IsActive,
                    CUIInvitationStatus= invitation
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the contact.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="contactDTO">The contact dto.</param>
        /// <returns>IHttpActionResult. update the contact detail </returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(Contact))]
        public IHttpActionResult PutContact(int id, ContactDTODetail contactDTO)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddContact))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != contactDTO.Id)
                {
                    return BadRequest();
                }

                Contact contact = rpoContext.Contacts.Find(id);
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                if (contact == null)
                {
                    return BadRequest();
                }

                contact.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    contact.LastModifiedBy = employee.Id;
                }

                contact.BirthDate = contactDTO.BirthDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(contactDTO.BirthDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactDTO.BirthDate;
                contact.IdCompany = contactDTO.IdCompany;
                contact.IdContactTitle = contactDTO.IdContactTitle;
                contact.Email = contactDTO.Email;
                contact.FirstName = contactDTO.FirstName;
                contact.IdPrefix = contactDTO.IdPrefix;
                contact.LastName = contactDTO.LastName;
                contact.MiddleName = contactDTO.MiddleName;
                contact.MobilePhone = contactDTO.MobilePhone;
                contact.Notes = contactDTO.Notes;
                contact.PersonalType = (PersonalType)int.Parse(contactDTO.PersonalType);
                contact.WorkPhone = contactDTO.WorkPhone;
                contact.WorkPhoneExt = contactDTO.WorkPhoneExt;
                contact.OtherPhone = contactDTO.OtherPhone;
                contact.IdSuffix = contactDTO.IdSuffix;
                contact.FaxNumber = contactDTO.FaxNumber;
                contact.IdPrimaryCompanyAddress = contactDTO.IdPrimaryCompanyAddress;
                contact.IsPrimaryCompanyAddress = contactDTO.IsPrimaryCompanyAddress;
                if (contactDTO.ContactLicenses != null)
                {
                    foreach (var license in contactDTO.ContactLicenses)
                    {
                        license.ContactLicenseType = null;
                        rpoContext.Entry(
                            new ContactLicense
                            {
                                ExpirationLicenseDate = license.ExpirationLicenseDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(license.ExpirationLicenseDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : license.ExpirationLicenseDate,
                                Id = license.Id,
                                ContactLicenseType = rpoContext.ContactLicenseTypes.Find(license.IdContactLicenseType),
                                IdContact = contactDTO.Id,
                                IdContactLicenseType = license.IdContactLicenseType,
                                Number = license.Number
                            }).State = license.Id == 0 ? EntityState.Added : EntityState.Modified;
                    }

                    var ids = contactDTO.ContactLicenses.Select(l => l.Id);

                    rpoContext.ContactLicenses.RemoveRange(rpoContext.ContactLicenses.Where(cl => cl.IdContact == contact.Id && !ids.Any(cclIds => cclIds == cl.Id)));
                }

                if (contactDTO.Addresses != null)
                {
                    foreach (var address in contactDTO.Addresses)
                    {
                        rpoContext.Entry(
                            new Address
                            {
                                AddressType = rpoContext.AddressTypes.Find(address.IdAddressType),
                                City = address.City,
                                Address1 = address.Address1,
                                Address2 = address.Address2,
                                Id = address.Id,
                                IdAddressType = address.IdAddressType,
                                IdState = address.IdState,
                                State = rpoContext.States.Find(address.IdState),
                                IdContact = contactDTO.Id,
                                Phone = address.Phone,
                                ZipCode = address.ZipCode,
                                IsMainAddress = address.IsMainAddress

                            }).State = address.Id == 0 ? EntityState.Added : EntityState.Modified;
                    }

                    var ids = contactDTO.Addresses.Select(a => a.Id);

                    rpoContext.Addresses.RemoveRange(rpoContext.Addresses.Where(a => a.IdContact == contact.Id && !ids.Any(aIds => aIds == a.Id)));
                }

                if (contactDTO.DocumentsToDelete != null)
                {
                    foreach (var item in contactDTO.DocumentsToDelete)
                    {
                        int contactDocumentId = Convert.ToInt32(item);
                        ContactDocument contactDocument = rpoContext.ContactDocuments.Where(x => x.Id == contactDocumentId).FirstOrDefault();
                        if (contactDocument != null)
                        {
                            rpoContext.ContactDocuments.Remove(contactDocument);
                            var path = HttpRuntime.AppDomainAppPath;
                            string directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.ContactDocumentPath));
                            string directoryDelete = Convert.ToString(contactDocument.Id) + "_" + contactDocument.DocumentPath;
                            string deletefilename = System.IO.Path.Combine(directoryName, directoryDelete);

                            if (File.Exists(deletefilename))
                            {
                                File.Delete(deletefilename);
                            }
                        }
                    }
                }

                using (var ctx = new Model.RpoContext())
                {
                    List<JobContact> objJobContacts = ctx.JobContacts.Where(d => d.IdContact == id).ToList();

                    if (objJobContacts != null)
                    {
                        foreach (var item in objJobContacts)
                        {
                            item.IdCompany = contactDTO.IdCompany;
                            Address objAddress = (from d in rpoContext.Addresses where d.Id == item.IdAddress select d).FirstOrDefault();
                            if (objAddress != null && objAddress.IdCompany != null && objAddress.IdCompany != item.IdCompany)
                            {
                                item.IdAddress = null;
                            }
                        }
                    }
                    ctx.SaveChanges();
                }
                using (var ctx = new Model.RpoContext())
                {
                    List<Job> objJobs = ctx.Jobs.Where(d => d.IdContact == id).ToList();

                    if (objJobs != null)
                    {
                        foreach (var item in objJobs)
                        {
                            item.IdCompany = contactDTO.IdCompany;
                        }
                    }
                    ctx.SaveChanges();
                }


                try
                {
                    rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

                Contact contactresponse = rpoContext.Contacts.Find(contact.Id);
                return Ok(new ContactDTODetail
                {
                    Id = contactresponse.Id,
                    PersonalType = ((int)contactresponse.PersonalType).ToString(),
                    IdPrefix = contactresponse.IdPrefix,
                    Prefix = contactresponse.IdPrefix != null ? contactresponse.Prefix.Name : null,
                    FirstName = contactresponse.FirstName,
                    MiddleName = contactresponse.MiddleName,
                    LastName = contactresponse.LastName,
                    IdCompany = contactresponse.IdCompany,
                    Company = contactresponse.Company != null ? contactresponse.Company.Name : null,
                    IdContactTitle = contactresponse.IdContactTitle,
                    ContactTitle = contactresponse.IdContactTitle != null ? contactresponse.ContactTitle.Name : null,
                    BirthDate = contactresponse.BirthDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactresponse.BirthDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactresponse.BirthDate,
                    CompanyAddresses = rpoContext.Addresses.Where(d => d.Id == (contactresponse.IdPrimaryCompanyAddress != null ? contactresponse.IdPrimaryCompanyAddress : null)).Select(a => new Addresses.AddressDTO
                    {
                        Id = a.Id,
                        Address1 = a.Address1,
                        Address2 = a.Address2,
                        AddressType = a.AddressType,
                        IdAddressType = a.IdAddressType,
                        City = a.City,
                        IdState = a.IdState,
                        State = a.State.Acronym,
                        ZipCode = a.ZipCode,
                        Phone = a.Phone,
                        IsMainAddress = a.IsMainAddress,
                    }).OrderBy(x => x.AddressType.DisplayOrder),
                    Addresses = rpoContext.Addresses.Where(d => d.IdContact == contactresponse.Id).Select(a => new Addresses.AddressDTO
                    {
                        Id = a.Id,
                        Address1 = a.Address1,
                        Address2 = a.Address2,
                        AddressType = a.AddressType,
                        IdAddressType = a.IdAddressType,
                        City = a.City,
                        IdState = a.IdState,
                        State = a.State.Acronym,
                        ZipCode = a.ZipCode,
                        Phone = a.Phone,
                        IsMainAddress = a.IsMainAddress,
                    }).OrderBy(x => x.AddressType.DisplayOrder),
                    WorkPhone = contactresponse.WorkPhone,
                    WorkPhoneExt = contactresponse.WorkPhoneExt,
                    Ext = contactresponse.WorkPhoneExt,
                    MobilePhone = contactresponse.MobilePhone,
                    OtherPhone = contactresponse.OtherPhone,
                    Email = contactresponse.Email,
                    ContactLicenses = contactresponse.ContactLicenses.Select(cl => new ContactLicenseDTODetail
                    {
                        Id = cl.Id,
                        ContactLicenseType = cl.ContactLicenseType,
                        IdContactLicenseType = cl.IdContactLicenseType,
                        ExpirationLicenseDate = cl.ExpirationLicenseDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(cl.ExpirationLicenseDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : cl.ExpirationLicenseDate,
                        Number = cl.Number
                    }),
                    Notes = contactresponse.Notes,
                    Documents = contactresponse.Documents.Select(d => new ContactDocument
                    {
                        Name = d.Name,
                        Id = d.Id,
                        DocumentPath = APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactDocumentPath) + "/" + Convert.ToString(d.Id) + "_" + d.DocumentPath
                    }),
                    DocumentsToDelete = new int[0],
                    Suffix = contactresponse.Suffix?.Description,
                    IdSuffix = contactresponse.IdSuffix,
                    ImageUrl = string.IsNullOrEmpty(contactresponse.ContactImagePath) ? string.Empty : APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactImagePath) + "/" + Convert.ToString(contactresponse.Id) + "_" + contactresponse.ContactImagePath,
                    ImageThumbUrl = string.IsNullOrEmpty(contactresponse.ContactImagePath) ? string.Empty : APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactImagePath) + "/" + Convert.ToString(contactresponse.Id) + "_" + contactresponse.ContactImagePath,
                    CreatedBy = contactresponse.CreatedBy,
                    LastModifiedBy = contactresponse.LastModifiedBy != null ? contactresponse.LastModifiedBy : contactresponse.CreatedBy,
                    CreatedByEmployeeName = contactresponse.CreatedByEmployee != null ? contactresponse.CreatedByEmployee.FirstName + " " + contactresponse.CreatedByEmployee.LastName : string.Empty,
                    LastModifiedByEmployeeName = contactresponse.LastModifiedByEmployee != null ? contactresponse.LastModifiedByEmployee.FirstName + " " + contactresponse.LastModifiedByEmployee.LastName : (contactresponse.CreatedByEmployee != null ? contactresponse.CreatedByEmployee.FirstName + " " + contactresponse.CreatedByEmployee.LastName : string.Empty),
                    CreatedDate = contactresponse.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactresponse.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactresponse.CreatedDate,
                    LastModifiedDate = contactresponse.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactresponse.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (contactresponse.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactresponse.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactresponse.CreatedDate),
                    FaxNumber = string.IsNullOrEmpty(contactresponse.FaxNumber) ? string.Empty : contactresponse.FaxNumber,
                    IsActive = contactresponse.IsActive
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the contact is Active.
        /// </summary>
        /// <param name="contactDTO">The contact dto.</param>
        /// <returns>IHttpActionResult. update the contact inactive stag to active </returns>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(Contact))]
        [Route("api/Contact/IsActive")]
        public IHttpActionResult PutContactIsActive(ContactDTODetail contactDTO)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddContact))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                Contact contacts = this.rpoContext.Contacts.FirstOrDefault(x => x.Id == contactDTO.Id);

                if (contacts == null)
                {
                    return this.NotFound();
                }

                if (contactDTO.IsActive == false)
                {
                    List<JobContact> objJobContact = (from d in rpoContext.JobContacts.Include("Job") where d.IdContact == contactDTO.Id && d.IsBilling == true select d).ToList();
                    string jobmessage = string.Empty;

                    if (objJobContact != null && objJobContact.Count > 0)
                    {
                        foreach (var item in objJobContact)
                        {
                            jobmessage = jobmessage + "Job# <a href='" + Properties.Settings.Default.FrontEndUrl + "/job/" + item.Job.Id + "/application" + "'> " + item.Job.Id + "</a> - " + item.Job.HouseNumber + " " + item.Job.StreetNumber + (!string.IsNullOrEmpty(item.Job.SpecialPlace) ? " - " + item.Job.SpecialPlace : string.Empty) + " <br>";

                        }
                        string subject = string.Empty;
                        string jobs = string.Join(", ", objJobContact.Select(i => i.IdJob));
                        string message = string.Empty;
                        subject = "Contact " + contacts.FirstName + " " + contactDTO.LastName + " has been marked as Inactive in Snapcor.";
                        message = "Hi,<br><br>Contact <a href='" + Properties.Settings.Default.FrontEndUrl + "/contactdetail/" + contacts.Id + "'>" + contacts.FirstName + " " + contactDTO.LastName + "</a> has been marked as Inactive.<br>";
                        if (contacts.IdCompany != null)
                        {
                            Company objcomp = (from d in rpoContext.Companies where d.Id == contacts.IdCompany select d).FirstOrDefault();

                            message = message + "Contact belongs to company " + objcomp.Name + ".<br>";
                        }
                        message = message + "This contact is a billing contact in the following Job <br><br>";
                        message = message + jobmessage;

                        //  message = "Contact " + contacts.FirstName + " " + contactDTO.LastName + " has been marked as inactive, however he is a billing contact in Job# " + jobs + "";
                        sendemailInactivecontact(message, subject);
                    }
                }

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                contacts.PersonalType = contacts.PersonalType;
                contacts.FirstName = contacts.FirstName;
                contacts.IsActive = contactDTO.IsActive;
                contacts.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    contacts.LastModifiedBy = employee.Id;
                }

                this.rpoContext.SaveChanges();

                string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

                Contact contactresponse = rpoContext.Contacts.Find(contacts.Id);
                return Ok(new ContactDTODetail
                {
                    Id = contactresponse.Id,
                    PersonalType = ((int)contactresponse.PersonalType).ToString(),
                    IdPrefix = contactresponse.IdPrefix,
                    Prefix = contactresponse.IdPrefix != null ? contactresponse.Prefix.Name : null,
                    FirstName = contactresponse.FirstName,
                    MiddleName = contactresponse.MiddleName,
                    LastName = contactresponse.LastName,
                    IdCompany = contactresponse.IdCompany,
                    Company = contactresponse.Company != null ? contactresponse.Company.Name : null,
                    IdContactTitle = contactresponse.IdContactTitle,
                    ContactTitle = contactresponse.IdContactTitle != null ? contactresponse.ContactTitle.Name : null,
                    BirthDate = contactresponse.BirthDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactresponse.BirthDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactresponse.BirthDate,
                    CompanyAddresses = rpoContext.Addresses.Where(d => d.Id == (contactresponse.IdPrimaryCompanyAddress != null ? contactresponse.IdPrimaryCompanyAddress : null)).Select(a => new Addresses.AddressDTO
                    {
                        Id = a.Id,
                        Address1 = a.Address1,
                        Address2 = a.Address2,
                        AddressType = a.AddressType,
                        IdAddressType = a.IdAddressType,
                        City = a.City,
                        IdState = a.IdState,
                        State = a.State.Acronym,
                        ZipCode = a.ZipCode,
                        Phone = a.Phone,
                        IsMainAddress = a.IsMainAddress,
                    }).OrderBy(x => x.AddressType.DisplayOrder),
                    Addresses = rpoContext.Addresses.Where(d => d.IdContact == contactresponse.Id).Select(a => new Addresses.AddressDTO
                    {
                        Id = a.Id,
                        Address1 = a.Address1,
                        Address2 = a.Address2,
                        AddressType = a.AddressType,
                        IdAddressType = a.IdAddressType,
                        City = a.City,
                        IdState = a.IdState,
                        State = a.State.Acronym,
                        ZipCode = a.ZipCode,
                        Phone = a.Phone,
                        IsMainAddress = a.IsMainAddress,
                    }).OrderBy(x => x.AddressType.DisplayOrder),
                    WorkPhone = contactresponse.WorkPhone,
                    WorkPhoneExt = contactresponse.WorkPhoneExt,
                    Ext = contactresponse.WorkPhoneExt,
                    MobilePhone = contactresponse.MobilePhone,
                    OtherPhone = contactresponse.OtherPhone,
                    Email = contactresponse.Email,
                    ContactLicenses = contactresponse.ContactLicenses.Select(cl => new ContactLicenseDTODetail
                    {
                        Id = cl.Id,
                        ContactLicenseType = cl.ContactLicenseType,
                        IdContactLicenseType = cl.IdContactLicenseType,
                        ExpirationLicenseDate = cl.ExpirationLicenseDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(cl.ExpirationLicenseDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : cl.ExpirationLicenseDate,
                        Number = cl.Number
                    }),
                    Notes = contactresponse.Notes,
                    Documents = contactresponse.Documents.Select(d => new ContactDocument
                    {
                        Name = d.Name,
                        Id = d.Id,
                        DocumentPath = APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactDocumentPath) + "/" + Convert.ToString(d.Id) + "_" + d.DocumentPath
                    }),
                    DocumentsToDelete = new int[0],
                    Suffix = contactresponse.Suffix?.Description,
                    IdSuffix = contactresponse.IdSuffix,
                    ImageUrl = string.IsNullOrEmpty(contactresponse.ContactImagePath) ? string.Empty : APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactImagePath) + "/" + Convert.ToString(contactresponse.Id) + "_" + contactresponse.ContactImagePath,
                    ImageThumbUrl = string.IsNullOrEmpty(contactresponse.ContactImagePath) ? string.Empty : APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactImagePath) + "/" + Convert.ToString(contactresponse.Id) + "_" + contactresponse.ContactImagePath,
                    CreatedBy = contactresponse.CreatedBy,
                    LastModifiedBy = contactresponse.LastModifiedBy != null ? contactresponse.LastModifiedBy : contactresponse.CreatedBy,
                    CreatedByEmployeeName = contactresponse.CreatedByEmployee != null ? contactresponse.CreatedByEmployee.FirstName + " " + contactresponse.CreatedByEmployee.LastName : string.Empty,
                    LastModifiedByEmployeeName = contactresponse.LastModifiedByEmployee != null ? contactresponse.LastModifiedByEmployee.FirstName + " " + contactresponse.LastModifiedByEmployee.LastName : (contactresponse.CreatedByEmployee != null ? contactresponse.CreatedByEmployee.FirstName + " " + contactresponse.CreatedByEmployee.LastName : string.Empty),
                    CreatedDate = contactresponse.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactresponse.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactresponse.CreatedDate,
                    LastModifiedDate = contactresponse.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactresponse.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (contactresponse.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactresponse.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactresponse.CreatedDate),
                    FaxNumber = string.IsNullOrEmpty(contactresponse.FaxNumber) ? string.Empty : contactresponse.FaxNumber,
                    IsActive = contactresponse.IsActive
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the contact.
        /// </summary>
        /// <param name="contactDTO">The contact dto.</param>
        /// <returns>IHttpActionResult. create new contact </returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ContactDTODetail))]
        public IHttpActionResult PostContact(ContactDTODetail contactDTO)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddContact))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                int contactId = 0;

                using (DbContextTransaction transaction = rpoContext.Database.BeginTransaction())
                {
                    try
                    {
                        Contact contact = rpoContext.Contacts.Add(new Contact
                        {
                            BirthDate = contactDTO.BirthDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(contactDTO.BirthDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactDTO.BirthDate,
                            IdCompany = contactDTO.IdCompany,
                            IdContactTitle = contactDTO.IdContactTitle,
                            Email = contactDTO.Email,
                            FirstName = contactDTO.FirstName,
                            IdPrefix = contactDTO.IdPrefix,
                            LastName = contactDTO.LastName,
                            MiddleName = contactDTO.MiddleName,
                            MobilePhone = contactDTO.MobilePhone,
                            Notes = contactDTO.Notes,
                            PersonalType = contactDTO.PersonalType == null ? PersonalType.Individual : (PersonalType)int.Parse(contactDTO.PersonalType),
                            WorkPhone = contactDTO.WorkPhone,
                            WorkPhoneExt = contactDTO.WorkPhoneExt,
                            OtherPhone = contactDTO.OtherPhone,
                            ContactLicenses = new HashSet<ContactLicense>(),
                            Addresses = new HashSet<Address>(),
                            IdSuffix = contactDTO.IdSuffix,
                            LastModifiedDate = DateTime.UtcNow,
                            CreatedDate = DateTime.UtcNow,
                            FaxNumber = contactDTO.FaxNumber,
                            IdPrimaryCompanyAddress = contactDTO.IdPrimaryCompanyAddress,
                            IsPrimaryCompanyAddress = contactDTO.IsPrimaryCompanyAddress,
                            IsActive = contactDTO.IsActive
                        });

                        if (employee != null)
                        {
                            contact.CreatedBy = employee.Id;
                        }

                        if (contactDTO.ContactLicenses != null)
                        {
                            foreach (var license in contactDTO.ContactLicenses)
                            {
                                rpoContext.Entry(
                                    new ContactLicense
                                    {
                                        ExpirationLicenseDate = license.ExpirationLicenseDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(license.ExpirationLicenseDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : license.ExpirationLicenseDate,
                                        Id = license.Id,
                                        ContactLicenseType = rpoContext.ContactLicenseTypes.Find(license.IdContactLicenseType),
                                        IdContact = contactDTO.Id,
                                        IdContactLicenseType = license.IdContactLicenseType,
                                        Number = license.Number
                                    }).State = EntityState.Added;
                            }
                        }

                        if (contactDTO.Addresses != null)
                        {
                            foreach (var address in contactDTO.Addresses)
                            {
                                rpoContext.Entry(
                                    new Address
                                    {
                                        AddressType = rpoContext.AddressTypes.Find(address.IdAddressType),
                                        City = address.City,
                                        Address1 = address.Address1,
                                        Address2 = address.Address2,
                                        Id = address.Id,
                                        IdAddressType = address.IdAddressType,
                                        IdState = address.IdState,
                                        State = rpoContext.States.Find(address.IdState),
                                        IdContact = contactDTO.Id,
                                        Phone = address.Phone,
                                        ZipCode = address.ZipCode,
                                        IsMainAddress = address.IsMainAddress
                                    }).State = address.Id == 0 ? EntityState.Added : EntityState.Modified;
                            }
                        }

                        rpoContext.SaveChanges();
                       var customer= rpoContext.Customers.Where(x => x.IdContcat == contact.Id).FirstOrDefault();
                        if (customer != null)
                        {
                            customer.FirstName = contactDTO.FirstName;
                            customer.LastName = contactDTO.LastName;
                            customer.EmailAddress = contactDTO.Email;
                            customer.CompanyName = rpoContext.Companies.Find(contact.IdCompany).Name;
                            rpoContext.Entry(customer).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                        contactId = contact.Id;
                        rpoContext.Entry(contact).Reference(c => c.Prefix).Load();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                try
                {
                    string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

                    Contact contactresponse = newdb.Contacts.Include("CreatedByEmployee").Include("LastModifiedByEmployee").Where(x => x.Id == contactId).FirstOrDefault();
                    return Ok(new ContactDTODetail
                    {
                        Id = contactresponse.Id,
                        PersonalType = ((int)contactresponse.PersonalType).ToString(),
                        IdPrefix = contactresponse.IdPrefix,
                        Prefix = contactresponse.IdPrefix != null ? contactresponse.Prefix.Name : null,
                        FirstName = contactresponse.FirstName,
                        MiddleName = contactresponse.MiddleName,
                        LastName = contactresponse.LastName,
                        IdCompany = contactresponse.IdCompany,
                        Company = contactresponse.Company != null ? contactresponse.Company.Name : null,
                        IdContactTitle = contactresponse.IdContactTitle,
                        ContactTitle = contactresponse.IdContactTitle != null ? contactresponse.ContactTitle.Name : null,
                        BirthDate = contactresponse.BirthDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactresponse.BirthDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactresponse.BirthDate,
                        CompanyAddresses = rpoContext.Addresses.Where(d => d.Id == (contactresponse.IdPrimaryCompanyAddress != null ? contactresponse.IdPrimaryCompanyAddress : null)).Select(a => new Addresses.AddressDTO
                        {
                            Id = a.Id,
                            Address1 = a.Address1,
                            Address2 = a.Address2,
                            AddressType = a.AddressType,
                            IdAddressType = a.IdAddressType,
                            City = a.City,
                            IdState = a.IdState,
                            State = a.State.Acronym,
                            ZipCode = a.ZipCode,
                            Phone = a.Phone,
                            IsMainAddress = a.IsMainAddress,
                        }).OrderBy(x => x.AddressType.DisplayOrder),
                        Addresses = rpoContext.Addresses.Where(d => d.IdContact == contactresponse.Id).Select(a => new Addresses.AddressDTO
                        {
                            Id = a.Id,
                            Address1 = a.Address1,
                            Address2 = a.Address2,
                            AddressType = a.AddressType,
                            IdAddressType = a.IdAddressType,
                            City = a.City,
                            IdState = a.IdState,
                            State = a.State.Acronym,
                            ZipCode = a.ZipCode,
                            Phone = a.Phone,
                            IsMainAddress = a.IsMainAddress,
                        }).OrderBy(x => x.AddressType.DisplayOrder),
                        WorkPhone = contactresponse.WorkPhone,
                        WorkPhoneExt = contactresponse.WorkPhoneExt,
                        Ext = contactresponse.WorkPhoneExt,
                        MobilePhone = contactresponse.MobilePhone,
                        OtherPhone = contactresponse.OtherPhone,
                        Email = contactresponse.Email,
                        ContactLicenses = contactresponse.ContactLicenses.Select(cl => new ContactLicenseDTODetail
                        {
                            Id = cl.Id,
                            ContactLicenseType = cl.ContactLicenseType,
                            IdContactLicenseType = cl.IdContactLicenseType,
                            ExpirationLicenseDate = cl.ExpirationLicenseDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(cl.ExpirationLicenseDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : cl.ExpirationLicenseDate,
                            Number = cl.Number
                        }),
                        Notes = contactresponse.Notes,
                        Documents = contactresponse.Documents.Select(d => new ContactDocument
                        {
                            Name = d.Name,
                            Id = d.Id,
                            DocumentPath = APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactDocumentPath) + "/" + Convert.ToString(d.Id) + "_" + d.DocumentPath
                        }),
                        DocumentsToDelete = new int[0],
                        Suffix = contactresponse.Suffix?.Description,
                        IdSuffix = contactresponse.IdSuffix,
                        ImageUrl = string.IsNullOrEmpty(contactresponse.ContactImagePath) ? string.Empty : APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactImagePath) + "/" + Convert.ToString(contactresponse.Id) + "_" + contactresponse.ContactImagePath,
                        ImageThumbUrl = string.IsNullOrEmpty(contactresponse.ContactImagePath) ? string.Empty : APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactImagePath) + "/" + Convert.ToString(contactresponse.Id) + "_" + contactresponse.ContactImagePath,
                        CreatedBy = contactresponse.CreatedBy,
                        LastModifiedBy = contactresponse.LastModifiedBy != null ? contactresponse.LastModifiedBy : contactresponse.CreatedBy,
                        CreatedByEmployeeName = contactresponse.CreatedByEmployee != null ? contactresponse.CreatedByEmployee.FirstName + " " + contactresponse.CreatedByEmployee.LastName : string.Empty,
                        LastModifiedByEmployeeName = contactresponse.LastModifiedByEmployee != null ? contactresponse.LastModifiedByEmployee.FirstName + " " + contactresponse.LastModifiedByEmployee.LastName : (contactresponse.CreatedByEmployee != null ? contactresponse.CreatedByEmployee.FirstName + " " + contactresponse.CreatedByEmployee.LastName : string.Empty),
                        CreatedDate = contactresponse.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactresponse.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactresponse.CreatedDate,
                        LastModifiedDate = contactresponse.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactresponse.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (contactresponse.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactresponse.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactresponse.CreatedDate),
                        FaxNumber = string.IsNullOrEmpty(contactresponse.FaxNumber) ? string.Empty : contactresponse.FaxNumber,
                        IdPrimaryCompanyAddress = contactresponse.IdPrimaryCompanyAddress != null ? contactresponse.IdPrimaryCompanyAddress : null,
                        IsPrimaryCompanyAddress = contactresponse.IsPrimaryCompanyAddress != null ? contactresponse.IsPrimaryCompanyAddress : null,
                        IsActive = contactresponse.IsActive
                    });
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }


        /// <summary>
        /// Parameter 1 : DeletedDocumentIds (String)
        /// Parameter 2 : IdContact (String)
        /// Parameter 3 : Documentfiles want to Upload (File)
        /// </summary>
        /// <returns>Task&lt;HttpResponseMessage&gt; update the detail of contact.</returns>
        /// <exception cref="System.Web.Http.HttpResponseException"></exception>
        [Authorize]
        [RpoAuthorize]
        [Route("api/contacts/Document")]
        [ResponseType(typeof(ContactDocument))]
        public async Task<HttpResponseMessage> PutContactdocument()
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddContact))
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
                System.Collections.Specialized.NameValueCollection formData = provider.FormData;
                IList<HttpContent> files = provider.Files;
                string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
                int idContact = Convert.ToInt32(formData["idContact"]);

                string deletedDocumentIds = Convert.ToString(formData["deletedDocumentIds"]);

                if (!string.IsNullOrEmpty(deletedDocumentIds))
                {
                    foreach (var item in deletedDocumentIds.Split(','))
                    {
                        int contactDocumentId = Convert.ToInt32(item);
                        ContactDocument contactDocument = rpoContext.ContactDocuments.Where(x => x.Id == contactDocumentId).FirstOrDefault();
                        if (contactDocument != null)
                        {
                            rpoContext.ContactDocuments.Remove(contactDocument);
                            var path = HttpRuntime.AppDomainAppPath;
                            string directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.ContactDocumentPath));
                            string directoryDelete = Convert.ToString(contactDocument.Id) + "_" + contactDocument.DocumentPath;
                            string deletefilename = System.IO.Path.Combine(directoryName, directoryDelete);

                            if (File.Exists(deletefilename))
                            {
                                File.Delete(deletefilename);
                            }
                        }
                    }
                    rpoContext.SaveChanges();
                }

                foreach (HttpContent item in files)
                {
                    HttpContent file1 = item;
                    var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');

                    string filename = string.Empty;
                    Stream input = await file1.ReadAsStreamAsync();
                    string directoryName = string.Empty;
                    string URL = string.Empty;

                    ContactDocument contactDocument = new ContactDocument();
                    contactDocument.DocumentPath = thisFileName;
                    contactDocument.IdContact = idContact;
                    contactDocument.Name = thisFileName;
                    rpoContext.ContactDocuments.Add(contactDocument);
                    rpoContext.SaveChanges();

                    var path = HttpRuntime.AppDomainAppPath;
                    directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.ContactDocumentPath));

                    string directoryFileName = Convert.ToString(contactDocument.Id) + "_" + thisFileName;
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
                }

                var contactDocumentList = rpoContext.ContactDocuments
                    .Where(x => x.IdContact == idContact).ToList();
                //    .Select(rd => new ContactDocument
                //    {
                //        Id = rd.Id,
                //        Name = rd.Name,
                //        Content = rd.Content,
                //        IdContact = rd.IdContact
                //        DocumentPath = APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactDocumentPath) + "/" + rd.DocumentPath
                //    }).ToList();

                var response = Request.CreateResponse<List<ContactDocument>>(HttpStatusCode.OK, contactDocumentList);
                return response;
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
        [Route("api/contacts/Images")]
        [ResponseType(typeof(Contact))]
        public async Task<HttpResponseMessage> PutContactImage()
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddContact))
            {
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
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the contact.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult. Delete the contact which is not available in rfps and jobs</returns>
        /// <exception cref="RpoBusinessException">Cannot delete contact as it is associated with RFP/Jobs</exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(Contact))]
        public IHttpActionResult DeleteContact(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteContact))
            {
                Contact contact = rpoContext.Contacts.Find(id);
                if (contact == null)
                {
                    return this.NotFound();
                }

                if (rpoContext.Rfps.Any(r => r.IdContact == id) || rpoContext.Jobs.Any(j => j.IdContact == id))
                {
                    throw new RpoBusinessException("Cannot delete contact as it is associated with RFP/Jobs");
                }

                rpoContext.Contacts.Remove(contact);
                rpoContext.SaveChanges();

                return Ok(contact);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Get the contact dropdown.
        /// </summary>
        /// <returns>IHttpActionResult. Get the list of contacts</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/contacts/Dropdown")]
        public IHttpActionResult GetContactDropdown()
        {
            var result = rpoContext.Contacts.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName + (c.Suffix != null ? " " + c.Suffix.Description : string.Empty) : string.Empty),
                NameWithEmail = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty) + (!string.IsNullOrWhiteSpace(c.Email) ? " (" + c.Email + ")" : string.Empty),
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                IdCompany = c.IdCompany,
            }).ToArray();

            return Ok(result);
        }
        /// <summary>
        /// Gets the contact dropdown.
        /// </summary>
        /// <returns>IHttpActionResult.Get the list of active contacts</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/Activecontacts/Dropdown")]
        public IHttpActionResult GetActiveContactDropdown()
        {
            //    List<Company> objcompany = rpoContext.Companies.ToList();
            //    var result = rpoContext.Contacts.AsEnumerable().Where(c => c.IsActive == true).Select(c => new
            //    {
            //        Id = c.Id,
            //        ItemName = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty) + (c.Suffix != null ? " " + c.Suffix.Description:string.Empty) + ((!string.IsNullOrWhiteSpace(c.Email) && c.IdCompany != null) ? " (" + (!string.IsNullOrWhiteSpace((from d in objcompany where d.Id == c.IdCompany select d.Name).FirstOrDefault()) ? (from d in objcompany where d.Id == c.IdCompany select d.Name).FirstOrDefault() : string.Empty) + (!string.IsNullOrWhiteSpace(c.Email) ? " - " + c.Email : string.Empty) + ")" : (!string.IsNullOrWhiteSpace(c.Email) || c.IdCompany != null) ? " (" + (!string.IsNullOrWhiteSpace(c.Email) ? c.Email : string.Empty) + (!string.IsNullOrWhiteSpace((from d in objcompany where d.Id == c.IdCompany select d.Name).FirstOrDefault()) ? ((!string.IsNullOrWhiteSpace(c.Email) ? " - " : string.Empty) + (from d in objcompany where d.Id == c.IdCompany select d.Name).FirstOrDefault()) : string.Empty) + ")" : string.Empty),
            //        NameWithEmail = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty) + (!string.IsNullOrWhiteSpace(c.Email) ? " (" + c.Email + ")" : string.Empty),
            //        FirstName = c.FirstName,
            //        LastName = c.LastName,
            //        Email = c.Email,
            //        IdCompany = c.IdCompany,
            //    }).ToArray();
            //    return Ok(result);
            string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "ActiveContactslist");
            var result = ds.Tables[0];
            return Ok(result);
        }
        /// <summary>
        /// Gets the contact dropdown.
        /// </summary>
        /// <returns>IHttpActionResult.Get the list of active contacts</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/ActiveCompany/Dropdown/{id}")]
        public IHttpActionResult GetActiveCompanyDropdown(int id)
        {
            List<Company> objcompany = rpoContext.Companies.ToList();
            var result = rpoContext.Contacts.AsEnumerable().Where(c => c.IsActive == true && c.IdCompany==id).Select(c => new
            {
                Id = c.Id,
                ItemName = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty) + (c.Suffix != null ? " " + c.Suffix.Description : string.Empty) + ((!string.IsNullOrWhiteSpace(c.Email) && c.IdCompany != null) ? " (" + (!string.IsNullOrWhiteSpace((from d in objcompany where d.Id == id select d.Name).FirstOrDefault()) ? (from d in objcompany where d.Id == id select d.Name).FirstOrDefault() : string.Empty) + (!string.IsNullOrWhiteSpace(c.Email) ? " - " + c.Email : string.Empty) + ")" : (!string.IsNullOrWhiteSpace(c.Email) || c.IdCompany != null) ? " (" + (!string.IsNullOrWhiteSpace(c.Email) ? c.Email : string.Empty) + (!string.IsNullOrWhiteSpace((from d in objcompany where d.Id == id select d.Name).FirstOrDefault()) ? ((!string.IsNullOrWhiteSpace(c.Email) ? " - " : string.Empty) + (from d in objcompany where d.Id == id select d.Name).FirstOrDefault()) : string.Empty) + ")" : string.Empty),
                NameWithEmail = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty) + (!string.IsNullOrWhiteSpace(c.Email) ? " (" + c.Email + ")" : string.Empty),
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                IdCompany = c.IdCompany,
            }).ToArray();
            return Ok(result);
          
        }
        /// <summary>
        /// Gets the contact dropdown.
        /// </summary>
        /// <returns>IHttpActionResult.Get the list of active contacts</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/contactsTransmittal/Dropdown")]
        public IHttpActionResult GetContactTransmittalDropdown()
        {
            List<Company> objcompany = rpoContext.Companies.ToList();

            var result = rpoContext.Contacts.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty) + ((!string.IsNullOrWhiteSpace(c.Email) && c.IdCompany != null) ? " ( " + (!string.IsNullOrWhiteSpace(c.Email) ? c.Email : string.Empty) + (!string.IsNullOrWhiteSpace((from d in objcompany where d.Id == c.IdCompany select d.Name).FirstOrDefault()) ? " - " + (from d in objcompany where d.Id == c.IdCompany select d.Name).FirstOrDefault() : string.Empty) + " )" : (!string.IsNullOrWhiteSpace(c.Email) || c.IdCompany != null) ? " ( " + (!string.IsNullOrWhiteSpace(c.Email) ? c.Email : string.Empty) + (!string.IsNullOrWhiteSpace((from d in objcompany where d.Id == c.IdCompany select d.Name).FirstOrDefault()) ? ((!string.IsNullOrWhiteSpace(c.Email) ? " - " : string.Empty) + (from d in objcompany where d.Id == c.IdCompany select d.Name).FirstOrDefault()) : string.Empty) + " )" : string.Empty),
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                IdCompany = c.IdCompany,

            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the contact dropdown.
        /// </summary>
        /// <returns>IHttpActionResult.Get the list of active contacts</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/contactsTransmittalActive/Dropdown")]
        public IHttpActionResult GetContactTransmittalActiveDropdown()
        {
            List<Company> objcompany = rpoContext.Companies.ToList();

            var result = rpoContext.Contacts.AsEnumerable().Where(d => d.IsActive == true).Select(c => new
            {
                Id = c.Id,
                ItemName = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty) + ((!string.IsNullOrWhiteSpace(c.Email) && c.IdCompany != null) ? " ( " + (!string.IsNullOrWhiteSpace(c.Email) ? c.Email : string.Empty) + (!string.IsNullOrWhiteSpace((from d in objcompany where d.Id == c.IdCompany select d.Name).FirstOrDefault()) ? " - " + (from d in objcompany where d.Id == c.IdCompany select d.Name).FirstOrDefault() : string.Empty) + " )" : (!string.IsNullOrWhiteSpace(c.Email) || c.IdCompany != null) ? " ( " + (!string.IsNullOrWhiteSpace(c.Email) ? c.Email : string.Empty) + (!string.IsNullOrWhiteSpace((from d in objcompany where d.Id == c.IdCompany select d.Name).FirstOrDefault()) ? ((!string.IsNullOrWhiteSpace(c.Email) ? " - " : string.Empty) + (from d in objcompany where d.Id == c.IdCompany select d.Name).FirstOrDefault()) : string.Empty) + " )" : string.Empty),
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                IdCompany = c.IdCompany,

            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="emailDTO">The email dto.</param>
        /// <returns>IHttpActionResult. To send the email of selected contact</returns>
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
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/contacts/{id}/email/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult SendEmail(int id, [FromBody] ContactEmailDTO emailDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Contact contact = rpoContext.Contacts.Find(id);

            if (contact == null)
            {
                return this.NotFound();
            }

            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

            int responseIdContact = 0;
            int responseIdContactEmailHistory = 0;

            if (emailDTO.IsAdditionalAtttachment)
            {
                var dest = rpoContext.Employees.Where(c => emailDTO.IdContactsCc.Contains(c.Id)).ToList();
                foreach (var item in dest)
                {
                    if (item == null)
                    {
                        throw new RpoBusinessException("Contact Id not found");
                    }

                    if (string.IsNullOrEmpty(item.Email))
                    {
                        throw new RpoBusinessException($"Contact {item.FirstName} not has a valid e-mail");
                    }
                }

                //foreach (var dest in emailDTO.IdContactsCc.Select(c => rpoContext.Contacts.Find(c)))
                //{
                //    if (dest == null)
                //    {
                //        throw new RpoBusinessException("Contact Id not found");
                //    }

                //    if (dest.Email == null)
                //    {
                //        throw new RpoBusinessException($"Contact {dest.FirstName} not has a valid e-mail");
                //    }
                //}

                var contactTo = rpoContext.Contacts.Where(x => x.Id == emailDTO.IdContactAttention).FirstOrDefault();
                if (contactTo == null)
                {
                    throw new RpoBusinessException("Contact Id not found");
                }
                if (contactTo.Email == null)
                {
                    throw new RpoBusinessException($"Contact {contactTo.FirstName} not has a valid e-mail");
                }

                ContactEmailHistory contactEmailHistory = new ContactEmailHistory();
                contactEmailHistory.IdContact = contact.Id;
                contactEmailHistory.IdFromEmployee = emailDTO.IdFromEmployee;
                contactEmailHistory.IdToCompany = emailDTO.IdContactsTo;
                contactEmailHistory.IdContactAttention = emailDTO.IdContactAttention;
                contactEmailHistory.IdTransmissionType = emailDTO.IdTransmissionType;
                contactEmailHistory.IdEmailType = emailDTO.IdEmailType;
                contactEmailHistory.SentDate = DateTime.UtcNow;
                if (employee != null)
                {
                    contactEmailHistory.IdSentBy = employee.Id;
                }
                contactEmailHistory.EmailSubject = emailDTO.Subject;
                contactEmailHistory.EmailMessage = emailDTO.Body;
                contactEmailHistory.IsAdditionalAtttachment = emailDTO.IsAdditionalAtttachment;
                contactEmailHistory.IsEmailSent = false;

                rpoContext.ContactEmailHistories.Add(contactEmailHistory);
                rpoContext.SaveChanges();

                //foreach (int idCc in emailDTO.IdContactsCc.Distinct())
                //{
                //    ContactEmailCCHistory contactEmailCCHistory = new ContactEmailCCHistory();
                //    contactEmailCCHistory.IdContact = idCc;
                //    contactEmailCCHistory.IdContactEmailHistory = contactEmailHistory.Id;

                //    rpoContext.ContactEmailCCHistories.Add(contactEmailCCHistory);
                //    rpoContext.SaveChanges();
                //}

                responseIdContact = contact.Id;
                responseIdContactEmailHistory = contactEmailHistory.Id;

            }
            else
            {

                var cc = new List<KeyValuePair<string, string>>();

                var dest = rpoContext.Employees.Where(c => emailDTO.IdContactsCc.Contains(c.Id)).ToList();
                foreach (var item in dest)
                {
                    if (item == null)
                    {
                        throw new RpoBusinessException("Contact Id not found");
                    }

                    if (string.IsNullOrEmpty(item.Email))
                    {
                        throw new RpoBusinessException($"Contact {item.FirstName} not has a valid e-mail");
                    }
                    cc.Add(new KeyValuePair<string, string>(item.Email, item.FirstName + " " + item.LastName));
                }

                var to = new List<KeyValuePair<string, string>>();

                var contactTo = rpoContext.Contacts.Where(x => x.Id == emailDTO.IdContactAttention).FirstOrDefault();
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

                Employee employeeFrom = rpoContext.Employees.Find(emailDTO.IdFromEmployee);

                cc.Add(new KeyValuePair<string, string>(employeeFrom.Email, employeeFrom.FirstName + " " + employeeFrom.LastName));

                ContactEmailHistory contactEmailHistory = new ContactEmailHistory();
                contactEmailHistory.IdContact = contact.Id;
                contactEmailHistory.IdFromEmployee = emailDTO.IdFromEmployee;
                contactEmailHistory.IdToCompany = emailDTO.IdContactsTo;
                contactEmailHistory.IdContactAttention = emailDTO.IdContactAttention;
                contactEmailHistory.IdTransmissionType = emailDTO.IdTransmissionType;
                contactEmailHistory.IdEmailType = emailDTO.IdEmailType;
                contactEmailHistory.SentDate = DateTime.UtcNow;
                if (employee != null)
                {
                    contactEmailHistory.IdSentBy = employee.Id;
                }
                contactEmailHistory.EmailSubject = emailDTO.Subject;
                contactEmailHistory.EmailMessage = emailDTO.Body;
                contactEmailHistory.IsAdditionalAtttachment = emailDTO.IsAdditionalAtttachment;
                contactEmailHistory.IsEmailSent = true;

                rpoContext.ContactEmailHistories.Add(contactEmailHistory);
                rpoContext.SaveChanges();

                /*CC Histroy code commented with mansih */
                //foreach (int idCc in emailDTO.IdContactsCc.Distinct())
                //{
                //    ContactEmailCCHistory contactEmailCCHistory = new ContactEmailCCHistory();
                //    contactEmailCCHistory.IdContact = idCc;
                //    contactEmailCCHistory.IdContactEmailHistory = contactEmailHistory.Id;

                //    rpoContext.ContactEmailCCHistories.Add(contactEmailCCHistory);
                //    rpoContext.SaveChanges();
                //}

                responseIdContact = contact.Id;
                responseIdContactEmailHistory = contactEmailHistory.Id;

                contactEmailHistory.IsEmailSent = true;
                rpoContext.SaveChanges();

                TransmissionType transmissionType = rpoContext.TransmissionTypes.FirstOrDefault(x => x.Id == contactEmailHistory.IdTransmissionType);
                if (transmissionType != null && transmissionType.IsSendEmail)
                {
                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/MasterEmailTemplate.htm")))
                    {
                        body = reader.ReadToEnd();
                    }

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

                    Mail.Send(
                        new KeyValuePair<string, string>(employeeFrom.Email, employeeFrom.FirstName + " " + employeeFrom.LastName),
                        to,
                        cc,
                        emailDTO.Subject,
                        emailBody,
                        attachments
                    );
                }
            }

            return Ok(new { Message = "Mail sent successfully", idContact = responseIdContact.ToString(), idContactEmailHistory = responseIdContactEmailHistory.ToString() });
        }

        /// <summary>
        /// Posts the attachment.
        /// </summary>
        /// <returns>Task&lt;HttpResponseMessage&gt;. External attachment save on path againgst contact</returns>
        /// <exception cref="HttpResponseException">
        /// </exception>
        /// <exception cref="RpoBusinessException">
        /// Contact Id not found
        /// or
        /// or
        /// Contact Id not found
        /// or
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [Route("api/contacts/Attachment")]
        [ResponseType(typeof(ContactEmailAttachmentHistory))]
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
            int idContact = Convert.ToInt32(formData["idContact"]);
            int idContactEmailHistory = Convert.ToInt32(formData["idContactEmailHistory"]);

            var contactEmail = rpoContext.ContactEmailHistories.Include("ContactEmailCCHistories.Contact").Include("Contact").Include("ContactAttention").Include("FromEmployee").Where(x => x.Id == idContactEmailHistory).FirstOrDefault();

            if (contactEmail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var cc = new List<KeyValuePair<string, string>>();

            foreach (ContactEmailCCHistory item in contactEmail.ContactEmailCCHistories)
            {
                if (item == null)
                    throw new RpoBusinessException("Contact Id not found");

                if (item.Contact.Email == null)
                    throw new RpoBusinessException($"Contact {item.Contact.FirstName} not has a valid e-mail");

                cc.Add(new KeyValuePair<string, string>(item.Contact.Email, item.Contact.FirstName + " " + item.Contact.LastName));
            }

            var to = new List<KeyValuePair<string, string>>();

            var contactTo = contactEmail.ContactAttention;
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

            Employee employeeFrom = contactEmail.FromEmployee;

            if (employeeFrom != null)
            {
                cc.Add(new KeyValuePair<string, string>(employeeFrom.Email, employeeFrom.FirstName + " " + employeeFrom.LastName));
            }

            try
            {
                var path = HttpRuntime.AppDomainAppPath;

                foreach (HttpContent item in files)
                {
                    HttpContent file1 = item;
                    var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');

                    string filename = string.Empty;
                    Stream input = await file1.ReadAsStreamAsync();
                    string directoryName = string.Empty;
                    string URL = string.Empty;

                    ContactEmailAttachmentHistory contactEmailAttachmentHistory = new ContactEmailAttachmentHistory();
                    contactEmailAttachmentHistory.DocumentPath = thisFileName;
                    contactEmailAttachmentHistory.IdContactEmailHistory = idContactEmailHistory;
                    contactEmailAttachmentHistory.Name = thisFileName;
                    rpoContext.ContactEmailAttachmentHistories.Add(contactEmailAttachmentHistory);
                    rpoContext.SaveChanges();


                    directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.ContactAttachmentsPath));

                    string directoryFileName = Convert.ToString(contactEmailAttachmentHistory.Id) + "_" + thisFileName;
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

                    if (File.Exists(filename))
                    {
                        attachments.Add(filename);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            TransmissionType transmissionType = rpoContext.TransmissionTypes.FirstOrDefault(x => x.Id == contactEmail.IdTransmissionType);
            if (transmissionType != null && transmissionType.IsSendEmail)
            {
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/MasterEmailTemplate.htm")))
                {
                    body = reader.ReadToEnd();
                }

                string emailBody = body;
                emailBody = emailBody.Replace("##EmailBody##", contactEmail.EmailMessage);

                //List<TransmissionTypeDefaultCC> transmissionTypeDefaultCC = rpoContext.TransmissionTypeDefaultCCs.Include("Employee").Where(x => x.IdTransmissionType == contactEmail.IdTransmissionType).ToList();
                List<TransmissionTypeDefaultCC> transmissionTypeDefaultCC = rpoContext.TransmissionTypeDefaultCCs.Include("Employee").Where(x => x.IdEamilType == contactEmail.IdEmailType).ToList();
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
                        contactEmail.EmailSubject,
                        emailBody,
                        attachments
                    );
            }

            contactEmail.IsEmailSent = true;
            rpoContext.SaveChanges();

            var contactEmailAttachmentHistoryList = rpoContext.ContactEmailAttachmentHistories
                .Where(x => x.IdContactEmailHistory == idContactEmailHistory).ToList();
            var response = Request.CreateResponse<List<ContactEmailAttachmentHistory>>(HttpStatusCode.OK, contactEmailAttachmentHistoryList);
            return response;
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
        /// Contacts the exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ContactExists(int id)
        {
            return rpoContext.Contacts.Count(e => e.Id == id) > 0;
        }
        /// <summary>
        /// Contacts the List .
        /// </summary>
        /// <param name="Companyid">The identifier.</param>
        /// <param name="Contactid">The identifier.</param>
        /// <returns>Get the company contactaddresses</returns>
        [Authorize]
        [RpoAuthorize]
        [Route("api/CompanyContactAddresses/{Companyid}/{Contactid}")]
        [ResponseType(typeof(AddressDTO))]
        public IHttpActionResult GetCompanyContactAddresses(int? Companyid, int? Contactid)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            //if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewContact)
            //      || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddContact)
            //      || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteContact)
            //      || Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewCompany)
            //      || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddCompany)
            //      || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteCompany))

            AddressDTO objAddress = new AddressDTO();
            List<Address> objaddressList = new List<Address>();

            if (Companyid != null && Contactid != null && Companyid != 0 && Contactid != 0)
            {
                objaddressList = rpoContext.Addresses.Include("Company").Include("Contact").Where(d => d.IdCompany == Companyid || d.IdContact == Contactid).ToList();
            }
            else if (Companyid != null && Companyid != 0)
            {
                objaddressList = rpoContext.Addresses.Include("Company").Where(d => d.IdCompany == Companyid).ToList();
            }
            else if (Contactid != null && Contactid != 0)
            {
                objaddressList = rpoContext.Addresses.Include("Contact").Where(d => d.IdContact == Contactid).ToList();
            }

            if (objaddressList == null || objaddressList.Count <= 0)
            {
                return this.NotFound();
            }

            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return Ok(objaddressList.Select(a => new AddressDTO
            {
                Id = a.Id,
                Address1 = a.Address1,
                Address2 = a.Address2,
                AddressType = a.AddressType,
                IdAddressType = a.IdAddressType,
                City = a.City,
                IdState = a.IdState,
                State = a.State.Acronym,
                ZipCode = a.ZipCode,
                Phone = a.Phone,
                IsMainAddress = a.IsMainAddress,
                WorkPhone = a.Contact != null && !string.IsNullOrEmpty(a.Contact.WorkPhone) ? a.Contact.WorkPhone : null,
                Email = a.Contact != null && !string.IsNullOrEmpty(a.Contact.Email) ? a.Contact.Email : string.Empty,
                //WorkPhone = a.Contact !=null && !string.IsNullOrEmpty(a.Contact.WorkPhone) ? a.Contact.WorkPhone : !string.IsNullOrEmpty(a.Company.Addresses.Select(p=>p.Phone).FirstOrDefault()) ? a.Company.Addresses.Select(p => p.Phone).FirstOrDefault() : !string.IsNullOrEmpty(a.Contact.MobilePhone) ? a.Contact.MobilePhone : string.Empty,
            }).OrderBy(x => x.AddressType.DisplayOrder));
        }
        //         else
        //            {
        //                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
        //    }
        //}

        private void sendemailInactivecontact(string message, string subject)
        {
            var to = new List<KeyValuePair<string, string>>();
            SystemSettingDetail systemSettingDetail1 = Common.ReadSystemSetting(Enums.SystemSetting.ContactInActive);
            if (systemSettingDetail1 != null && systemSettingDetail1.Value != null && systemSettingDetail1.Value.Count() > 0)
            {
                foreach (var item in systemSettingDetail1.Value)
                {
                    to.Add(new KeyValuePair<string, string>(item.Email, item.EmployeeName));
                }
            }
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/MasterEmailTemplate.htm")))
            {
                body = reader.ReadToEnd();
            }

            var cc = new List<KeyValuePair<string, string>>();

            string emailBody = body;
            emailBody = emailBody.Replace("##EmailBody##", message);
            if (to != null && to.Count() > 0)
            {
                Mail.Send(
                       new KeyValuePair<string, string>("noreply@rpoinc.com", "RPO"),
                       to,
                       cc,
                       subject,
                       emailBody,
                       true
                   );
            }
        }

        /// <summary>
        /// Contact List Export to Excel
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <param name="companyType"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/contacts/exporttoexcel")]
        public IHttpActionResult ContactsExport([FromUri] ContactDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

            string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();
            string Firstname = string.Empty;
            string Lastname = string.Empty;
            DataSet ds = new DataSet();
            if (dataTableParameters != null && dataTableParameters.GlobalSearchType != null && dataTableParameters.GlobalSearchType > 0)
            {
                string[] strArray = dataTableParameters.GlobalSearchText.Split(' ');
                if (strArray.Count() > 0)
                {
                    Firstname = strArray[0].Trim();
                }
                if (strArray.Count() > 1)
                {
                    Lastname = strArray[1].Trim();
                }
            }

            SqlParameter[] spParameter = new SqlParameter[2];

            spParameter[0] = new SqlParameter("@FirstName", SqlDbType.NVarChar, 50);
            spParameter[0].Direction = ParameterDirection.Input;
            spParameter[0].Value = Firstname;

            spParameter[1] = new SqlParameter("@LastName", SqlDbType.NVarChar, 50);
            spParameter[1].Direction = ParameterDirection.Input;
            spParameter[1].Value = Lastname;

            ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "Contacts_List", spParameter);

            ds.Tables[0].TableName = "Data";

            string exportFilename = "ExportContacts_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
            string exportFilePath = ExportToExcel(ds, exportFilename);
            FileInfo fileinfo = new FileInfo(exportFilePath);
            long fileinfoSize = fileinfo.Length;

            var downloadFilePath = Properties.Settings.Default.APIUrl + "/" + Properties.Settings.Default.ReportExcelExportPath + "/" + exportFilename;
            List<KeyValuePair<string, string>> fileResult = new List<KeyValuePair<string, string>>();
            fileResult.Add(new KeyValuePair<string, string>("exportFilePath", downloadFilePath));
            fileResult.Add(new KeyValuePair<string, string>("exportFilesize", Convert.ToString(fileinfoSize)));

            return Ok(fileResult);
        }
        /// <summary>
        /// Create a excel File and write contacts List.
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="exportFilename"></param>
        /// <returns>exportFilename path</returns>
        private string ExportToExcel(DataSet ds, string exportFilename)
        {
            string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
            string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string templateFileName = string.Empty;
            templateFileName = "ContactsTemplate.xlsx";
            string templateFilePath = templatePath + templateFileName;
            string exportFilePath = path + exportFilename;
            File.Delete(exportFilePath);

            XSSFWorkbook templateWorkbook = new XSSFWorkbook(templateFilePath);
            ISheet sheet = templateWorkbook.GetSheet("Sheet1");

            XSSFFont myFont = (XSSFFont)templateWorkbook.CreateFont();
            myFont.FontHeightInPoints = (short)12;
            myFont.FontName = "Times New Roman";
            myFont.IsBold = false;

            XSSFCellStyle leftAlignCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
            leftAlignCellStyle.SetFont(myFont);
            leftAlignCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            leftAlignCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            leftAlignCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            leftAlignCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            leftAlignCellStyle.WrapText = true;
            leftAlignCellStyle.VerticalAlignment = VerticalAlignment.Top;
            leftAlignCellStyle.Alignment = HorizontalAlignment.Left;

            XSSFCellStyle rightAlignCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
            rightAlignCellStyle.SetFont(myFont);
            rightAlignCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            rightAlignCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            rightAlignCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            rightAlignCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            rightAlignCellStyle.WrapText = true;
            rightAlignCellStyle.VerticalAlignment = VerticalAlignment.Top;
            rightAlignCellStyle.Alignment = HorizontalAlignment.Right;


            int index = 3;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                IRow iRow = sheet.GetRow(index);
                if (iRow != null)
                {
                    if (iRow.GetCell(0) != null)
                    {
                        iRow.GetCell(0).SetCellValue(dr["FirstName"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(0).SetCellValue(dr["FirstName"].ToString());
                    }

                    if (iRow.GetCell(1) != null)
                    {
                        iRow.GetCell(1).SetCellValue(dr["LastName"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(1).SetCellValue(dr["LastName"].ToString());
                    }

                    if (iRow.GetCell(2) != null)
                    {
                        iRow.GetCell(2).SetCellValue(dr["Name"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(2).SetCellValue(dr["Name"].ToString());
                    }

                    if (iRow.GetCell(3) != null)
                    {
                        iRow.GetCell(3).SetCellValue(dr["Address"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(3).SetCellValue(dr["Address"].ToString());
                    }

                    if (iRow.GetCell(4) != null)
                    {
                        iRow.GetCell(4).SetCellValue(dr["WorkPhone"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(4).SetCellValue(dr["WorkPhone"].ToString());
                    }

                    if (iRow.GetCell(5) != null)
                    {
                        iRow.GetCell(5).SetCellValue(dr["Email"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(5).SetCellValue(dr["Email"].ToString());
                    }
                    if (iRow.GetCell(6) != null)
                    {
                        bool status = Convert.ToBoolean(dr["IsActive"].ToString());
                        string statusval = string.Empty;
                        if (status == true)
                        {
                            statusval = "Active";
                        }
                        else
                        {
                            statusval = "InActive";
                        }


                        iRow.GetCell(6).SetCellValue(statusval);
                    }
                    else
                    {
                        bool status = Convert.ToBoolean(dr["IsActive"].ToString());
                        string statusval = string.Empty;
                        if (status == true)
                        {
                            statusval = "Active";
                        }
                        else
                        {
                            statusval = "InActive";
                        }


                        iRow.CreateCell(6).SetCellValue(statusval);
                    }


                    if (iRow.GetCell(7) != null)
                    {
                        iRow.GetCell(7).SetCellValue(dr["ContactLicenseType"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(7).SetCellValue(dr["ContactLicenseType"].ToString());
                    }

                    if (iRow.GetCell(8) != null)
                    {
                        iRow.GetCell(8).SetCellValue(dr["LicensesNumber"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(8).SetCellValue(dr["LicensesNumber"].ToString());
                    }



                    iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(6).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(7).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(8).CellStyle = leftAlignCellStyle;

                }
                else
                {
                    iRow = sheet.CreateRow(index);

                    if (iRow.GetCell(0) != null)
                    {
                        iRow.GetCell(0).SetCellValue(dr["FirstName"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(0).SetCellValue(dr["FirstName"].ToString());
                    }

                    if (iRow.GetCell(1) != null)
                    {
                        iRow.GetCell(1).SetCellValue(dr["LastName"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(1).SetCellValue(dr["LastName"].ToString());
                    }

                    if (iRow.GetCell(2) != null)
                    {
                        iRow.GetCell(2).SetCellValue(dr["Name"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(2).SetCellValue(dr["Name"].ToString());
                    }

                    if (iRow.GetCell(3) != null)
                    {
                        iRow.GetCell(3).SetCellValue(dr["Address"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(3).SetCellValue(dr["Address"].ToString());
                    }

                    if (iRow.GetCell(4) != null)
                    {
                        iRow.GetCell(4).SetCellValue(dr["WorkPhone"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(4).SetCellValue(dr["WorkPhone"].ToString());
                    }

                    if (iRow.GetCell(5) != null)
                    {
                        iRow.GetCell(5).SetCellValue(dr["Email"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(5).SetCellValue(dr["Email"].ToString());
                    }

                    if (iRow.GetCell(6) != null)
                    {
                        bool status = Convert.ToBoolean(dr["IsActive"].ToString());
                        string statusval = string.Empty;
                        if (status == true)
                        {
                            statusval = "Active";
                        }
                        else
                        {
                            statusval = "InActive";
                        }


                        iRow.GetCell(6).SetCellValue(statusval);
                    }
                    else
                    {
                        bool status = Convert.ToBoolean(dr["IsActive"].ToString());
                        string statusval = string.Empty;
                        if (status == true)
                        {
                            statusval = "Active";
                        }
                        else
                        {
                            statusval = "InActive";
                        }


                        iRow.CreateCell(6).SetCellValue(statusval);
                    }

                    if (iRow.GetCell(7) != null)
                    {
                        iRow.GetCell(7).SetCellValue(dr["ContactLicenseType"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(7).SetCellValue(dr["ContactLicenseType"].ToString());
                    }

                    if (iRow.GetCell(8) != null)
                    {
                        iRow.GetCell(8).SetCellValue(dr["LicensesNumber"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(8).SetCellValue(dr["LicensesNumber"].ToString());
                    }



                    iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(6).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(7).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(8).CellStyle = leftAlignCellStyle;
                }

                index = index + 1;
            }

            using (var file2 = new FileStream(exportFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                templateWorkbook.Write(file2);
                file2.Close();
            }

            return exportFilePath;
        }

        /// <summary>
        /// Puts the contact.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="contactDTO">The contact dto.</param>
        /// <returns>IHttpActionResult. update the contact detail </returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(Contact))]
        [Route("api/contacts/PutContactIsHidden/{id}/{IsHiddenFromCustomer}")]
        public IHttpActionResult PutContactIsHidden(int id, bool IsHiddenFromCustomer)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddContact))
            {           

                Contact contact = rpoContext.Contacts.Find(id);
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                if (contact == null)
                {
                    return BadRequest();
                }

                contact.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    contact.LastModifiedBy = employee.Id;
                }

                contact.IsHidden = IsHiddenFromCustomer;
                rpoContext.Entry(contact).State = EntityState.Modified;            

                try
                {
                    rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

                Contact contactresponse = rpoContext.Contacts.Find(contact.Id);
                return Ok(new ContactDTODetail
                {
                    Id = contactresponse.Id,
                    PersonalType = ((int)contactresponse.PersonalType).ToString(),
                    IdPrefix = contactresponse.IdPrefix,
                    Prefix = contactresponse.IdPrefix != null ? contactresponse.Prefix.Name : null,
                    FirstName = contactresponse.FirstName,
                    MiddleName = contactresponse.MiddleName,
                    LastName = contactresponse.LastName,
                    IdCompany = contactresponse.IdCompany,
                    Company = contactresponse.Company != null ? contactresponse.Company.Name : null,
                    IdContactTitle = contactresponse.IdContactTitle,
                    ContactTitle = contactresponse.IdContactTitle != null ? contactresponse.ContactTitle.Name : null,
                    BirthDate = contactresponse.BirthDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactresponse.BirthDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactresponse.BirthDate,
                    CompanyAddresses = rpoContext.Addresses.Where(d => d.Id == (contactresponse.IdPrimaryCompanyAddress != null ? contactresponse.IdPrimaryCompanyAddress : null)).Select(a => new Addresses.AddressDTO
                    {
                        Id = a.Id,
                        Address1 = a.Address1,
                        Address2 = a.Address2,
                        AddressType = a.AddressType,
                        IdAddressType = a.IdAddressType,
                        City = a.City,
                        IdState = a.IdState,
                        State = a.State.Acronym,
                        ZipCode = a.ZipCode,
                        Phone = a.Phone,
                        IsMainAddress = a.IsMainAddress,
                    }).OrderBy(x => x.AddressType.DisplayOrder),
                    Addresses = rpoContext.Addresses.Where(d => d.IdContact == contactresponse.Id).Select(a => new Addresses.AddressDTO
                    {
                        Id = a.Id,
                        Address1 = a.Address1,
                        Address2 = a.Address2,
                        AddressType = a.AddressType,
                        IdAddressType = a.IdAddressType,
                        City = a.City,
                        IdState = a.IdState,
                        State = a.State.Acronym,
                        ZipCode = a.ZipCode,
                        Phone = a.Phone,
                        IsMainAddress = a.IsMainAddress,
                    }).OrderBy(x => x.AddressType.DisplayOrder),
                    WorkPhone = contactresponse.WorkPhone,
                    WorkPhoneExt = contactresponse.WorkPhoneExt,
                    Ext = contactresponse.WorkPhoneExt,
                    MobilePhone = contactresponse.MobilePhone,
                    OtherPhone = contactresponse.OtherPhone,
                    Email = contactresponse.Email,
                    ContactLicenses = contactresponse.ContactLicenses.Select(cl => new ContactLicenseDTODetail
                    {
                        Id = cl.Id,
                        ContactLicenseType = cl.ContactLicenseType,
                        IdContactLicenseType = cl.IdContactLicenseType,
                        ExpirationLicenseDate = cl.ExpirationLicenseDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(cl.ExpirationLicenseDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : cl.ExpirationLicenseDate,
                        Number = cl.Number
                    }),
                    Notes = contactresponse.Notes,
                    Documents = contactresponse.Documents.Select(d => new ContactDocument
                    {
                        Name = d.Name,
                        Id = d.Id,
                        DocumentPath = APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactDocumentPath) + "/" + Convert.ToString(d.Id) + "_" + d.DocumentPath
                    }),
                    DocumentsToDelete = new int[0],
                    Suffix = contactresponse.Suffix?.Description,
                    IdSuffix = contactresponse.IdSuffix,
                    ImageUrl = string.IsNullOrEmpty(contactresponse.ContactImagePath) ? string.Empty : APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactImagePath) + "/" + Convert.ToString(contactresponse.Id) + "_" + contactresponse.ContactImagePath,
                    ImageThumbUrl = string.IsNullOrEmpty(contactresponse.ContactImagePath) ? string.Empty : APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactImagePath) + "/" + Convert.ToString(contactresponse.Id) + "_" + contactresponse.ContactImagePath,
                    CreatedBy = contactresponse.CreatedBy,
                    LastModifiedBy = contactresponse.LastModifiedBy != null ? contactresponse.LastModifiedBy : contactresponse.CreatedBy,
                    CreatedByEmployeeName = contactresponse.CreatedByEmployee != null ? contactresponse.CreatedByEmployee.FirstName + " " + contactresponse.CreatedByEmployee.LastName : string.Empty,
                    LastModifiedByEmployeeName = contactresponse.LastModifiedByEmployee != null ? contactresponse.LastModifiedByEmployee.FirstName + " " + contactresponse.LastModifiedByEmployee.LastName : (contactresponse.CreatedByEmployee != null ? contactresponse.CreatedByEmployee.FirstName + " " + contactresponse.CreatedByEmployee.LastName : string.Empty),
                    CreatedDate = contactresponse.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactresponse.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactresponse.CreatedDate,
                    LastModifiedDate = contactresponse.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactresponse.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (contactresponse.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(contactresponse.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : contactresponse.CreatedDate),
                    FaxNumber = string.IsNullOrEmpty(contactresponse.FaxNumber) ? string.Empty : contactresponse.FaxNumber,
                    IsActive = contactresponse.IsActive,
                    IsHiddenFromCustomer = IsHiddenFromCustomer
                }); ;
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/CustomerContactsListPost")]
        public IHttpActionResult CustomerContactsList(ContactDataTableParameters dataTableParameters)

        {
            var employee = this.rpoContext.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewContact)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddContact)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteContact))
            {
                //var contacts = rpoContext.Contacts.Include(p => p.ContactLicenses).AsQueryable();


                string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();
                string Firstname = string.Empty;
                string Lastname = string.Empty;
                if (dataTableParameters != null && dataTableParameters.GlobalSearchType != null && dataTableParameters.GlobalSearchText != null && dataTableParameters.GlobalSearchType > 0)
                {
                    string[] strArray = dataTableParameters.GlobalSearchText.Split(' ');
                    if (strArray.Count() > 0)
                    {
                        Firstname = strArray[0].Trim();
                    }
                    if (strArray.Count() > 1)
                    {
                        Lastname = strArray[1].Trim();
                    }
                }

                DataSet ds = new DataSet();

                SqlParameter[] spParameter = new SqlParameter[11];

                spParameter[0] = new SqlParameter("@FirstName", SqlDbType.NVarChar, 50);
                spParameter[0].Direction = ParameterDirection.Input;
                spParameter[0].Value = Firstname;

                spParameter[1] = new SqlParameter("@LastName", SqlDbType.NVarChar, 50);
                spParameter[1].Direction = ParameterDirection.Input;
                spParameter[1].Value = Lastname;

                spParameter[2] = new SqlParameter("@IdContactLicenseType", SqlDbType.Int);
                spParameter[2].Direction = ParameterDirection.Input;
                spParameter[2].Value = dataTableParameters.IdContactLicenseType != null ? dataTableParameters.IdContactLicenseType : null;

                spParameter[3] = new SqlParameter("@PageIndex", SqlDbType.Int);
                spParameter[3].Direction = ParameterDirection.Input;
                spParameter[3].Value = dataTableParameters.Start;

                spParameter[4] = new SqlParameter("@PageSize", SqlDbType.Int);
                spParameter[4].Direction = ParameterDirection.Input;
                spParameter[4].Value = dataTableParameters.Length;

                spParameter[5] = new SqlParameter("@Column", SqlDbType.NVarChar, 50);
                spParameter[5].Direction = ParameterDirection.Input;
                spParameter[5].Value = dataTableParameters.OrderedColumn != null ? dataTableParameters.OrderedColumn.Column : null;

                spParameter[6] = new SqlParameter("@Dir", SqlDbType.VarChar, 50);
                spParameter[6].Direction = ParameterDirection.Input;
                spParameter[6].Value = dataTableParameters.OrderedColumn != null ? dataTableParameters.OrderedColumn.Dir : null;

                spParameter[7] = new SqlParameter("@Search", SqlDbType.NVarChar, 50);
                spParameter[7].Direction = ParameterDirection.Input;
                spParameter[7].Value = !string.IsNullOrEmpty(dataTableParameters.Search) ? dataTableParameters.Search : string.Empty;

                spParameter[8] = new SqlParameter("@IdCompany", SqlDbType.Int);
                spParameter[8].Direction = ParameterDirection.Input;
                spParameter[8].Value = dataTableParameters.IdCompany != null ? dataTableParameters.IdCompany : null;

                spParameter[9] = new SqlParameter("@Individual", SqlDbType.Int);
                spParameter[9].Direction = ParameterDirection.Input;
                spParameter[9].Value = dataTableParameters.Individual != null ? dataTableParameters.Individual : null;

                spParameter[10] = new SqlParameter("@RecordCount", SqlDbType.Int);
                spParameter[10].Direction = ParameterDirection.Output;

                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "[Customer_Contacts_Pagination_List]", spParameter);

         

                int totalrecord = 0;
                int Recordcount = 0;
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    totalrecord = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                    Recordcount = int.Parse(spParameter[10].SqlValue.ToString());

                }
                if (ds.Tables.Count > 1)
                {
                    ds.Tables[1].TableName = "Data";
                }

                
                return Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Start,
                    RecordsFiltered = Recordcount,
                    RecordsTotal = totalrecord,
                    Data = ds.Tables[1]                    
                });

            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

    }

    public class ResponseContacts
    {
        bool IsSuccess = false;
        string Message;
        object ResponseData;

        public ResponseContacts(bool status, string message, object data)
        {
            IsSuccess = status;
            Message = message;
            ResponseData = data;
        }
    }
}

