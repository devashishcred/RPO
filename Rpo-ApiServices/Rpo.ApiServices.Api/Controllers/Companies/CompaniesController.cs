// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-31-2018
// ***********************************************************************
// <copyright file="CompaniesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Companies Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Companies namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Companies
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
    using Rpo.ApiServices.Model.Models;
    using System.Configuration;
    using Microsoft.ApplicationBlocks.Data;
    using System.Data.SqlClient;
    using NPOI.XSSF.UserModel;
    using NPOI.SS.UserModel;
    using System.Web.Configuration;


    /// <summary>
    /// Class Companies Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [Authorize]
    public class CompaniesController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();




        /// <summary>
        /// Maps the company to company dto.
        /// </summary>
        /// <param name="companyDTO">The company dto.</param>
        /// <returns>CompanyDTO.  Get company List</returns>
        [Authorize]
        [RpoAuthorize]
         private CompanyDTO MapCompanyToCompanyDTO(Company companyDTO)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            Tools.EncryptDecrypt ED = new Tools.EncryptDecrypt();
            return new CompanyDTO()
            {
                Id = companyDTO.Id,
                Address = (companyDTO.Addresses.Any() ? companyDTO.Addresses.FirstOrDefault().Address1 : null) + (companyDTO.Addresses.Any() && companyDTO.Addresses.FirstOrDefault().Address2 != null ? ", " + companyDTO.Addresses.FirstOrDefault().Address2 : null) + (companyDTO.Addresses.Any() && companyDTO.Addresses.FirstOrDefault().City != null ? ", " + companyDTO.Addresses.FirstOrDefault().City : null) + (companyDTO.Addresses.Any() && companyDTO.Addresses.FirstOrDefault().State != null && companyDTO.Addresses.FirstOrDefault().State.Name != null ? ", " + companyDTO.Addresses.FirstOrDefault().State.Name : null) + (companyDTO.Addresses.Any() && companyDTO.Addresses.FirstOrDefault().ZipCode != null ? ", " + companyDTO.Addresses.FirstOrDefault().ZipCode : null),
                Address1 = companyDTO.Addresses.Any() ? companyDTO.Addresses.FirstOrDefault().Address1 : null,
                Address2 = companyDTO.Addresses.Any() ? companyDTO.Addresses.FirstOrDefault().Address2 : null,
                Addresses = companyDTO.Addresses.ToArray(),
                CompanyTypes = companyDTO.CompanyTypes,
                Name = companyDTO.Name,
                ZipCode = companyDTO.Addresses.Any() ? companyDTO.Addresses.FirstOrDefault().ZipCode : null,
                TrackingNumber = companyDTO.TrackingNumber,
                SpecialInspectionAgencyNumber = companyDTO.SpecialInspectionAgencyNumber,
                HICNumber = companyDTO.HICNumber,
                IBMNumber = companyDTO.IBMNumber,
                CTLicenseNumber = companyDTO.CTLicenseNumber,
                IdState = companyDTO.Addresses.Any() ? companyDTO.Addresses.FirstOrDefault().IdState : 0,
                InsuranceDisability = companyDTO.InsuranceDisability != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyDTO.InsuranceDisability), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyDTO.InsuranceDisability,
                InsuranceGeneralLiability = companyDTO.InsuranceGeneralLiability != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyDTO.InsuranceGeneralLiability), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyDTO.InsuranceGeneralLiability,
                InsuranceObstructionBond = companyDTO.InsuranceObstructionBond != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyDTO.InsuranceObstructionBond), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyDTO.InsuranceObstructionBond,
                InsuranceWorkCompensation = companyDTO.InsuranceWorkCompensation != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyDTO.InsuranceWorkCompensation), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyDTO.InsuranceWorkCompensation,
                SpecialInspectionAgencyExpiry = companyDTO.SpecialInspectionAgencyExpiry != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyDTO.SpecialInspectionAgencyExpiry), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyDTO.SpecialInspectionAgencyExpiry,
                TrackingExpiry = companyDTO.TrackingExpiry != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyDTO.TrackingExpiry), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyDTO.TrackingExpiry,
                HICExpiry = companyDTO.HICExpiry != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyDTO.HICExpiry), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyDTO.HICExpiry,
                CTExpirationDate = companyDTO.CTExpirationDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyDTO.CTExpirationDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyDTO.CTExpirationDate,
                TaxIdNumber = companyDTO.TaxIdNumber,
                City = companyDTO.Addresses.Any() ? companyDTO.Addresses.FirstOrDefault().City : null,
                State = companyDTO.Addresses.Any() ? companyDTO.Addresses.FirstOrDefault().State.Acronym : null,
                Phone = companyDTO.Addresses.Any() ? companyDTO.Addresses.FirstOrDefault().Phone : null,
                Notes = companyDTO.Notes,
                Url = companyDTO.Url,
                CreatedBy = companyDTO.CreatedBy,
                LastModifiedBy = companyDTO.LastModifiedBy != null ? companyDTO.LastModifiedBy : companyDTO.CreatedBy,
                CreatedByEmployeeName = companyDTO.CreatedByEmployee != null ? companyDTO.CreatedByEmployee.FirstName + " " + companyDTO.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = companyDTO.LastModifiedByEmployee != null ? companyDTO.LastModifiedByEmployee.FirstName + " " + companyDTO.LastModifiedByEmployee.LastName : (companyDTO.CreatedByEmployee != null ? companyDTO.CreatedByEmployee.FirstName + " " + companyDTO.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = companyDTO.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyDTO.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyDTO.CreatedDate,
                LastModifiedDate = companyDTO.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyDTO.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (companyDTO.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyDTO.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyDTO.CreatedDate),
                DOTInsuranceWorkCompensation = companyDTO.DOTInsuranceWorkCompensation != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyDTO.DOTInsuranceWorkCompensation), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyDTO.DOTInsuranceWorkCompensation,
                DOTInsuranceGeneralLiability = companyDTO.DOTInsuranceGeneralLiability != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyDTO.DOTInsuranceGeneralLiability), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyDTO.DOTInsuranceGeneralLiability,
                //mb respo
                IdResponsibility = companyDTO.IDResponsibility,
                ResponsibilityName = companyDTO?.Responsibilities?.ResponsibilityName,
                //mb Email
                EmailAddress = companyDTO.EmailAddress != null ? ED.Decrypt(companyDTO.EmailAddress, WebConfigurationManager.AppSettings["saltPassword"].ToString()) : null,
                EmailPassword = null,
                //mb
                CompanyLicenses = companyDTO.CompanyLicenses.Select(cl => new CompanyLicenseDTODetail
                {
                    Id = cl.Id,
                    CompanyLicenseType = cl.CompanyLicenseType,
                    IdCompanyLicenseType = cl.IdCompanyLicenseType,
                    ExpirationLicenseDate = cl.ExpirationLicenseDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(cl.ExpirationLicenseDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : cl.ExpirationLicenseDate,
                    Number = cl.Number
                }),
                //Documents = companyDTO.Documents.Select(d => new CompanyDocument
                //{
                //    Name = d.Name,
                //    Id = d.Id,
                //    DocumentPath = APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ComanyDocumentPath) + "/" + Convert.ToString(d.Id) + "_" + d.DocumentPath
                //}),
                Documents = companyDTO.Documents != null && companyDTO.Documents.Count > 0 ? companyDTO.Documents
                    .Select(d => new CompanyDocument
                    {
                        Name = d.Name,
                        Id = d.Id,
                        DocumentPath = APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ComanyDocumentPath) + "/" + Convert.ToString(d.Id) + "_" + d.DocumentPath
                    }) : null,
                DocumentsToDelete = new int[0],
            };
        }

        /// <summary>
        /// Gets the companies.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <param name="companyType">Type of the company.</param>
        /// <returns>IHttpActionResult.  Get company List</returns>
        /// <summary>
        /// Gets the companies.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <param name="companyType">Type of the company.</param>
        /// <returns> Get company List</returns>
        [Authorize]
        [RpoAuthorize]
        // [ResponseType(typeof(DataTableResponse))]
        public HttpResponseMessage GetCompanies([FromUri] CompanyAdvancedSearchParameters dataTableParameters, [FromUri]string companyType = null)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

            string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();

            DataSet ds = new DataSet();

            SqlParameter[] spParameter = new SqlParameter[1];
            spParameter[0] = new SqlParameter("@Search", SqlDbType.VarChar);
            spParameter[0].Direction = ParameterDirection.Input;
            if (dataTableParameters.GlobalSearchType != null && dataTableParameters.GlobalSearchType > 0)
            {
                spParameter[0].Value = dataTableParameters.GlobalSearchText;
            }
            else
            {
                //  spParameter[0].Value = "Null";
            }
            ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "Company_List", spParameter);           
            ds.Tables[0].TableName = "Data";

            return Request.CreateResponse(HttpStatusCode.OK, ds);
        }

        /// <summary>
        /// Gets the companies using Store Procedure.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <param name="companyType">Type of the company.</param>
        /// <returns> Get company List</returns>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/CompanyListPost")]
        // [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult CompaniesList(CompanyAdvancedSearchParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

            string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();

            DataSet ds = new DataSet();
            //mb parameter[9]
            SqlParameter[] spParameter = new SqlParameter[9];

            spParameter[0] = new SqlParameter("@PageIndex", SqlDbType.Int);
            spParameter[0].Direction = ParameterDirection.Input;
            spParameter[0].Value = dataTableParameters.Start;

            spParameter[1] = new SqlParameter("@PageSize", SqlDbType.Int);
            spParameter[1].Direction = ParameterDirection.Input;
            spParameter[1].Value = dataTableParameters.Length;

            spParameter[2] = new SqlParameter("@Column", SqlDbType.NVarChar, 50);
            spParameter[2].Direction = ParameterDirection.Input;
            spParameter[2].Value = dataTableParameters.OrderedColumn != null ? dataTableParameters.OrderedColumn.Column : null;

            spParameter[3] = new SqlParameter("@Dir", SqlDbType.VarChar, 50);
            spParameter[3].Direction = ParameterDirection.Input;
            spParameter[3].Value = dataTableParameters.OrderedColumn != null ? dataTableParameters.OrderedColumn.Dir : null;

            spParameter[4] = new SqlParameter("@Search", SqlDbType.NVarChar, 50);
            spParameter[4].Direction = ParameterDirection.Input;
            spParameter[4].Value = !string.IsNullOrEmpty(dataTableParameters.Search) ? dataTableParameters.Search : string.Empty;

            spParameter[5] = new SqlParameter("@CompanyTypes", SqlDbType.NVarChar, 50);
            spParameter[5].Direction = ParameterDirection.Input;
            spParameter[5].Value = !string.IsNullOrEmpty(dataTableParameters.CompanyTypes) ? dataTableParameters.CompanyTypes : string.Empty;

            spParameter[6] = new SqlParameter("@GlobalSearchText", SqlDbType.NVarChar, 50);
            spParameter[6].Direction = ParameterDirection.Input;
            spParameter[6].Value = !string.IsNullOrEmpty(dataTableParameters.GlobalSearchText) ? dataTableParameters.GlobalSearchText : string.Empty;

            spParameter[7] = new SqlParameter("@RecordCount", SqlDbType.Int);
            spParameter[7].Direction = ParameterDirection.Output;
            //mb
            spParameter[8] = new SqlParameter("@IdCompanyLicenseType", SqlDbType.Int);
            spParameter[8].Direction = ParameterDirection.Input;
            spParameter[8].Value = dataTableParameters.IdCompanyLicenseType != null ? dataTableParameters.IdCompanyLicenseType : null;

            if (dataTableParameters.GlobalSearchType != null && dataTableParameters.GlobalSearchType > 0)
            {
                spParameter[6].Value = dataTableParameters.GlobalSearchText;
            }
            else
            {
                //  spParameter[0].Value = "Null";
            }
            ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "Company_Pagination_List", spParameter);          

            int totalrecord = 0;
            int Recordcount = 0;
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                totalrecord = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                Recordcount = int.Parse(spParameter[7].SqlValue.ToString());

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

        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns>IHttpActionResult. Get company List to bind dropdown</returns>
        [ResponseType(typeof(CompanyItem))]
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/companies/dropdown")]
        public IHttpActionResult List()
        {
            return Ok(rpoContext.Companies
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    ItemName = c.Name
                }));
        }

        /// <summary>
        /// Gets the contacts.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Get the contact list again the company</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Companies/{id}/contacts")]
        public IHttpActionResult GetContacts(int id)
        {
            Company company = rpoContext.Companies.Find(id);
            if (company == null)
            {
                return this.NotFound();
            }

            var result = rpoContext.Contacts
                .Include("Prefix")
                .Include("Suffix")
                .Include("Company")
                .Include("ContactTitle")
                .Include("Addresses.AddressType")
                .Include("ContactLicenses")
                .Include("Documents")
                .Include("CreatedByEmployee").Include("LastModifiedByEmployee")
                .Where(c => c.IdCompany == company.Id)
                .AsEnumerable()
                .Select(c => FormatContact(c)).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the contact dropdown.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>get the contact list against company</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Companies/{id}/ActivecontactsDropdown")]
        public IHttpActionResult GetActiveContactDropdown(int id)
        {
            if (id == -1)
            {
                var result = rpoContext.Contacts
                    .Where(c => c.IdCompany == null && c.IsActive == true).AsEnumerable()
                    .Select(c => new
                    {
                        Id = c.Id,
                        ContactName = c.FirstName + " " + c.LastName,
                        NameWithEmail = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty) + (!string.IsNullOrWhiteSpace(c.Email) ? " (" + c.Email + ")" : string.Empty),
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        IdCompany = c.IdCompany,
                        CompanyName = c.Company != null ? c.Company.Name : string.Empty,
                        ItemName = c.FirstName + " " + c.LastName
                    })
                    .ToArray();

                return Ok(result);
            }
            else
            {
                Company company = rpoContext.Companies.Find(id);
                if (company == null)
                {
                    return this.NotFound();
                }

                var result = rpoContext.Contacts
                    .Where(c => c.IdCompany == company.Id && c.IsActive == true).AsEnumerable()
                    .Select(c => new ContactDetail()
                    {
                        Id = c.Id,
                        ContactName = c.FirstName + " " + c.LastName,
                        NameWithEmail = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty) + (!string.IsNullOrWhiteSpace(c.Email) ? " (" + c.Email + ")" : string.Empty),
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        IdCompany = c.IdCompany,
                        CompanyName = c.Company != null ? c.Company.Name : string.Empty,
                        ItemName = c.FirstName + " " + c.LastName
                    })
                    .ToArray();

                return Ok(result);
            }
        }

        /// <summary>
        /// Gets the contact dropdown.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Get the contact list against the company</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Companies/{id}/contactsDropdown")]
        public IHttpActionResult GetContactDropdown(int id)
        {
            if (id == -1)
            {
                var result = rpoContext.Contacts
                    .Where(c => c.IdCompany == null).AsEnumerable()
                    .Select(c => new
                    {
                        Id = c.Id,
                        ContactName = c.FirstName + " " + c.LastName,
                        NameWithEmail = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty) + (!string.IsNullOrWhiteSpace(c.Email) ? " (" + c.Email + ")" : string.Empty),
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        IdCompany = c.IdCompany,
                        CompanyName = c.Company != null ? c.Company.Name : string.Empty,
                        ItemName = c.FirstName + " " + c.LastName
                    })
                    .ToArray();

                return Ok(result);
            }
            else
            {
                Company company = rpoContext.Companies.Find(id);
                if (company == null)
                {
                    return this.NotFound();
                }

                var result = rpoContext.Contacts
                    .Where(c => c.IdCompany == company.Id).AsEnumerable()
                    .Select(c => new ContactDetail()
                    {
                        Id = c.Id,
                        ContactName = c.FirstName + " " + c.LastName,
                        NameWithEmail = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty) + (!string.IsNullOrWhiteSpace(c.Email) ? " (" + c.Email + ")" : string.Empty),
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        IdCompany = c.IdCompany,
                        CompanyName = c.Company != null ? c.Company.Name : string.Empty,
                        ItemName = c.FirstName + " " + c.LastName
                    })
                    .ToArray();

                return Ok(result);
            }
        }
        /// <summary>
        /// Gets the contact dropdown.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.get the contact list</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/Companies/EditContact/{id}/{idContact}/contactsDropdown")]
        public IHttpActionResult GetContactDropdown(int id, int idContact)
        {
            if (idContact == 0)
            {
                throw new RpoBusinessException("Contact is not Null or zero!");
            }

            if (id == -1)
            {
                var result1 = rpoContext.Contacts
                    .Where(c => c.IdCompany == null && c.IsActive == true).AsEnumerable()
                    .Select(c => new
                    {
                        Id = c.Id,
                        ContactName = c.FirstName + " " + c.LastName,
                        NameWithEmail = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty) + (!string.IsNullOrWhiteSpace(c.Email) ? " (" + c.Email + ")" : string.Empty),
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        IdCompany = c.IdCompany,
                        CompanyName = c.Company != null ? c.Company.Name : string.Empty,
                        ItemName = c.FirstName + " " + c.LastName
                    })
                    .ToArray();

                var result2 = rpoContext.Contacts
                    .Where(c => c.IdCompany == null && c.Id == idContact).AsEnumerable()
                    .Select(c => new
                    {
                        Id = c.Id,
                        ContactName = c.FirstName + " " + c.LastName,
                        NameWithEmail = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty) + (!string.IsNullOrWhiteSpace(c.Email) ? " (" + c.Email + ")" : string.Empty),
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        IdCompany = c.IdCompany,
                        CompanyName = c.Company != null ? c.Company.Name : string.Empty,
                        ItemName = c.FirstName + " " + c.LastName
                    })
                    .ToArray();
                var result = result1.Union(result2);
                return Ok(result);
            }
            else
            {
                Company company = rpoContext.Companies.Find(id);
                if (company == null)
                {
                    return this.NotFound();
                }

                var result1 = rpoContext.Contacts
                    .Where(c => c.IdCompany == company.Id && c.IsActive == true).AsEnumerable()
                    .Select(c => new ContactDetail()
                    {
                        Id = c.Id,
                        ContactName = c.FirstName + " " + c.LastName,
                        NameWithEmail = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty) + (!string.IsNullOrWhiteSpace(c.Email) ? " (" + c.Email + ")" : string.Empty),
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        IdCompany = c.IdCompany,
                        CompanyName = c.Company != null ? c.Company.Name : string.Empty,
                        ItemName = c.FirstName + " " + c.LastName
                    })
                    .ToArray();

                var result2 = rpoContext.Contacts
                    .Where(c => c.IdCompany == company.Id && c.Id == idContact).AsEnumerable()
                    .Select(c => new ContactDetail()
                    {
                        Id = c.Id,
                        ContactName = c.FirstName + " " + c.LastName,
                        NameWithEmail = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty) + (!string.IsNullOrWhiteSpace(c.Email) ? " (" + c.Email + ")" : string.Empty),
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        IdCompany = c.IdCompany,
                        CompanyName = c.Company != null ? c.Company.Name : string.Empty,
                        ItemName = c.FirstName + " " + c.LastName
                    })
                    .ToArray();

                var result = result1.Union(result2);
                return Ok(result);
            }
        }


        /// <summary>
        /// Gets the company.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns> Get company detail</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(Company))]
        public IHttpActionResult GetCompany(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewCompany)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddCompany)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteCompany))
            {

                Company companyDTO = rpoContext.Companies
            .Include("CompanyTypes")
            .Include("Addresses")
            .Include("Documents")
            .Include("CompanyLicenses")
            .Include("CreatedByEmployee")
            .Include("LastModifiedByEmployee")
            // .Include("Responsibilities")
            .FirstOrDefault(x => x.Id == id);
            Tools.EncryptDecrypt ED = new Tools.EncryptDecrypt();
            if (companyDTO == null)
            {
                return this.NotFound();
            }
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            // CompanyDTODetail a = new CompanyDTODetail();
            //string email = companyDTO.EmailPassword != null ? ED.Decrypt(companyDTO.EmailPassword, WebConfigurationManager.AppSettings["saltPassword"].ToString()) : null;
            ; return Ok(new CompanyDTODetail
            {
                Addresses = companyDTO.Addresses.Select(a => new Addresses.AddressDTO
                {
                    Address1 = a.Address1,
                    Address2 = a.Address2,
                    AddressType = a.AddressType,
                    City = a.City,
                    Id = a.Id,
                    IdAddressType = a.IdAddressType,
                    IdState = a.IdState,
                    Phone = a.Phone,
                    State = a.State.Acronym,
                    ZipCode = a.ZipCode,
                    FaxNumber = a.Fax
                }).OrderBy(x => x.AddressType.DisplayOrder),
                CompanyTypes = companyDTO.CompanyTypes.ToArray(),
                Name = companyDTO.Name,
                HICNumber = companyDTO.HICNumber,
                CTLicenseNumber = companyDTO.CTLicenseNumber,
                IBMNumber = companyDTO.IBMNumber,
                Id = companyDTO.Id,
                IdCompanyTypes = companyDTO.CompanyTypes != null ? companyDTO.CompanyTypes.Select(x => x.Id).ToList() : null,
                SpecialInspectionAgencyNumber = companyDTO.SpecialInspectionAgencyNumber,
                SpecialInspectionAgencyExpiry = companyDTO.SpecialInspectionAgencyExpiry != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.SpecialInspectionAgencyExpiry), DateTimeKind.Utc) : companyDTO.SpecialInspectionAgencyExpiry,
                InsuranceDisability = companyDTO.InsuranceDisability != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.InsuranceDisability), DateTimeKind.Utc) : companyDTO.InsuranceDisability,
                InsuranceGeneralLiability = companyDTO.InsuranceGeneralLiability != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.InsuranceGeneralLiability), DateTimeKind.Utc) : companyDTO.InsuranceGeneralLiability,
                InsuranceObstructionBond = companyDTO.InsuranceObstructionBond != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.InsuranceObstructionBond), DateTimeKind.Utc) : companyDTO.InsuranceObstructionBond,
                InsuranceWorkCompensation = companyDTO.InsuranceWorkCompensation != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.InsuranceWorkCompensation), DateTimeKind.Utc) : companyDTO.InsuranceWorkCompensation,
                TrackingExpiry = companyDTO.TrackingExpiry != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.TrackingExpiry), DateTimeKind.Utc) : companyDTO.TrackingExpiry,
                HICExpiry = companyDTO.HICExpiry != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.HICExpiry), DateTimeKind.Utc) : companyDTO.HICExpiry,
                CTExpirationDate = companyDTO.CTExpirationDate != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.CTExpirationDate), DateTimeKind.Utc) : companyDTO.CTExpirationDate,

                Notes = companyDTO.Notes,
                Documents = companyDTO.Documents != null && companyDTO.Documents.Count > 0 ? companyDTO.Documents
                    .Select(d => new CompanyDocument
                    {
                        Name = d.Name,
                        Id = d.Id,
                        DocumentPath = APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ComanyDocumentPath) + "/" + Convert.ToString(d.Id) + "_" + d.DocumentPath
                    }) : null,
                 
                ResponsibilityName = companyDTO.Responsibilities != null ? Convert.ToString(companyDTO.Responsibilities.ResponsibilityName) : null,
                IdResponsibility = companyDTO.IDResponsibility != null ? companyDTO.IDResponsibility : null,                
                EmailAddress = companyDTO.EmailAddress != null ? ED.Decrypt(companyDTO.EmailAddress, Convert.ToString(WebConfigurationManager.AppSettings["saltPassword"])) : null,               
                EmailPassword= companyDTO.EmailPassword != null ? ED.Decrypt(companyDTO.EmailPassword, WebConfigurationManager.AppSettings["saltPassword"].ToString()) : null,               
                CompanyLicenses = companyDTO.CompanyLicenses != null && companyDTO.CompanyLicenses.Count > 0 ? companyDTO.CompanyLicenses.Select(cl => new CompanyLicenseDTODetail
                {
                    Id = cl.Id,
                    CompanyLicenseType = cl.CompanyLicenseType,
                    IdCompanyLicenseType = cl.IdCompanyLicenseType,
                    ExpirationLicenseDate = cl.ExpirationLicenseDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(cl.ExpirationLicenseDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : cl.ExpirationLicenseDate,
                    Number = cl.Number
                }) : null,
                DocumentsToDelete = new int[0],
                TaxIdNumber = companyDTO.TaxIdNumber,
                TrackingNumber = companyDTO.TrackingNumber,
                Url = companyDTO.Url,
                CreatedBy = companyDTO.CreatedBy,
                LastModifiedBy = companyDTO.LastModifiedBy != null ? companyDTO.LastModifiedBy : companyDTO.CreatedBy,
                CreatedByEmployeeName = companyDTO.CreatedByEmployee != null ? companyDTO.CreatedByEmployee.FirstName + " " + companyDTO.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = companyDTO.LastModifiedByEmployee != null ? companyDTO.LastModifiedByEmployee.FirstName + " " + companyDTO.LastModifiedByEmployee.LastName : (companyDTO.CreatedByEmployee != null ? companyDTO.CreatedByEmployee.FirstName + " " + companyDTO.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = companyDTO.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyDTO.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyDTO.CreatedDate,
                LastModifiedDate = companyDTO.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyDTO.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (companyDTO.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyDTO.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyDTO.CreatedDate),                
                DOTInsuranceWorkCompensation = companyDTO.DOTInsuranceWorkCompensation != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.DOTInsuranceWorkCompensation), DateTimeKind.Utc) : companyDTO.DOTInsuranceWorkCompensation,
                DOTInsuranceGeneralLiability = companyDTO.DOTInsuranceGeneralLiability != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.DOTInsuranceGeneralLiability), DateTimeKind.Utc) : companyDTO.DOTInsuranceGeneralLiability,

            });
        }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
    }
}

        /// <summary>
        /// Gets the contact company.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns> Get contact against the company</returns>
        [Route("api/contacts/{id}/company")]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CompanyDTODetail))]
        public IHttpActionResult GetContactCompany(int id)
        {
            Contact contact = rpoContext.Contacts.Find(id);
            if (contact == null)
            {
                return this.NotFound();
            }

            Company c = rpoContext.Companies.Where(comp => comp.Id == contact.IdCompany).FirstOrDefault();
            if (c == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }

            return GetCompany(c.Id);
        }

        /// <summary>
        /// Puts the company.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="companyDTO">The company dto.</param>
        /// <returns> update the company detail</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CompanyDTO))]
        public IHttpActionResult PutCompany(int id, CompanyDTODetail companyDTO)
        {
            int generalContractor = 13; //id 13="General Contractor"
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddCompany))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != companyDTO.Id)
                {
                    return BadRequest();
                }

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                Company company = rpoContext.Companies.Find(id);

                company.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    company.LastModifiedBy = employee.Id;
                }

                company.HICNumber = companyDTO.HICNumber;
                company.CTLicenseNumber = companyDTO.CTLicenseNumber;
                company.IBMNumber = companyDTO.IBMNumber;
                company.InsuranceDisability = companyDTO.InsuranceDisability != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.InsuranceDisability), DateTimeKind.Utc) : companyDTO.InsuranceDisability;
                company.InsuranceGeneralLiability = companyDTO.InsuranceGeneralLiability != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.InsuranceGeneralLiability), DateTimeKind.Utc) : companyDTO.InsuranceGeneralLiability;
                company.InsuranceObstructionBond = companyDTO.InsuranceObstructionBond != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.InsuranceObstructionBond), DateTimeKind.Utc) : companyDTO.InsuranceObstructionBond;
                company.InsuranceWorkCompensation = companyDTO.InsuranceWorkCompensation != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.InsuranceWorkCompensation), DateTimeKind.Utc) : companyDTO.InsuranceWorkCompensation;
                company.SpecialInspectionAgencyExpiry = companyDTO.SpecialInspectionAgencyExpiry != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.SpecialInspectionAgencyExpiry), DateTimeKind.Utc) : companyDTO.SpecialInspectionAgencyExpiry;
                company.TrackingExpiry = companyDTO.TrackingExpiry != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.TrackingExpiry), DateTimeKind.Utc) : companyDTO.TrackingExpiry;
                company.HICExpiry = companyDTO.HICExpiry != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.HICExpiry), DateTimeKind.Utc) : companyDTO.HICExpiry;
                company.CTExpirationDate = companyDTO.CTExpirationDate != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.CTExpirationDate), DateTimeKind.Utc) : companyDTO.CTExpirationDate;
                company.SpecialInspectionAgencyNumber = companyDTO.SpecialInspectionAgencyNumber;            
                company.Name = companyDTO.Name;
                company.Notes = companyDTO.Notes;
                company.TaxIdNumber = companyDTO.TaxIdNumber;
                company.TrackingNumber = companyDTO.TrackingNumber;
                company.Url = companyDTO.Url;               
                company.DOTInsuranceWorkCompensation = companyDTO.DOTInsuranceWorkCompensation != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.DOTInsuranceWorkCompensation), DateTimeKind.Utc) : companyDTO.DOTInsuranceWorkCompensation;
                company.DOTInsuranceGeneralLiability = companyDTO.DOTInsuranceGeneralLiability != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.DOTInsuranceGeneralLiability), DateTimeKind.Utc) : companyDTO.DOTInsuranceGeneralLiability;
               
                Tools.EncryptDecrypt ED = new Tools.EncryptDecrypt();
                if (!string.IsNullOrWhiteSpace(companyDTO.EmailAddress))
                {
                    company.EmailAddress = ED.Encrypt(companyDTO.EmailAddress, Convert.ToString(WebConfigurationManager.AppSettings["saltPassword"]));

                };
                if (!string.IsNullOrWhiteSpace(companyDTO.EmailPassword))
                {
                    company.EmailPassword = ED.Encrypt(companyDTO.EmailPassword, Convert.ToString(WebConfigurationManager.AppSettings["saltPassword"]));
                }                
                company.IDResponsibility = companyDTO.IdResponsibility;
                if (company.Responsibilities != null)
                {
                    company.Responsibilities.ResponsibilityName = companyDTO.ResponsibilityName != null ? companyDTO.ResponsibilityName : null;
                }
                else if (companyDTO.IdResponsibility != null)
                {
                    company.Responsibilities = this.rpoContext.Responsibilities.Where(r => r.Id == companyDTO.IdResponsibility).FirstOrDefault();
                    company.Responsibilities.Id = Convert.ToInt32(companyDTO.IdResponsibility);
                    company.Responsibilities.ResponsibilityName = companyDTO.ResponsibilityName != null ? companyDTO.ResponsibilityName : null;
                }
                else
                {
                    company.Responsibilities = null;
                }                
                if (companyDTO.CompanyLicenses != null)
                {
                    foreach (var license in companyDTO.CompanyLicenses)
                    {
                        license.CompanyLicenseType = null;
                        rpoContext.Entry(
                            new CompanyLicense
                            {
                                ExpirationLicenseDate = license.ExpirationLicenseDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(license.ExpirationLicenseDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : license.ExpirationLicenseDate,
                                Id = license.Id,
                                CompanyLicenseType = rpoContext.CompanyLicenseTypes.Find(license.IdCompanyLicenseType),
                                IdCompany = companyDTO.Id,
                                IdCompanyLicenseType = license.IdCompanyLicenseType,
                                Number = license.Number
                            }).State = license.Id == 0 ? EntityState.Added : EntityState.Modified;
                    }

                    var ids = companyDTO.CompanyLicenses.Select(l => l.Id);

                    rpoContext.CompanyLicenses.RemoveRange(rpoContext.CompanyLicenses.Where(cl => cl.IdCompany == company.Id && !ids.Any(cclIds => cclIds == cl.Id)));
                }
                if (companyDTO.Addresses != null)
                {
                    foreach (var address in companyDTO.Addresses)
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
                                IdCompany = companyDTO.Id,
                                Phone = address.Phone,
                                ZipCode = address.ZipCode,
                                Fax = address.FaxNumber
                            }).State = address.Id == 0 ? EntityState.Added : EntityState.Modified;
                    }

                    var ids = companyDTO.Addresses.Select(a => a.Id);

                    rpoContext.Addresses.RemoveRange(rpoContext.Addresses.Where(a => a.IdCompany == companyDTO.Id && !ids.Any(aIds => aIds == a.Id)));
                }

                if (companyDTO.CompanyTypes != null)
                {
                    foreach (var companyType in companyDTO.CompanyTypes.Where(ct => !company.CompanyTypes.Any(ctdb => ctdb.Id == ct.Id)))
                    {
                        company.CompanyTypes.Add(rpoContext.CompanyTypes.Find(companyType.Id));
                        //if(companyType.Id== generalContractor)
                        //{
                        //    company.IDResponsibility = companyDTO.IdResponsibility != null ? companyDTO.IdResponsibility : null;
                        //}

                    }

                    var companyTypesToRemove = company.CompanyTypes.Where(ctdb => !companyDTO.CompanyTypes.Any(ct => ct.Id == ctdb.Id)).ToList();

                    foreach (var item in companyTypesToRemove)
                    {
                        company.CompanyTypes.Remove(rpoContext.CompanyTypes.Find(item.Id));
                        if (item.Id == generalContractor)
                        {
                            company.IDResponsibility = null;
                        }
                    }
                }
                if (companyDTO.DocumentsToDelete != null)
                {
                    foreach (var item in companyDTO.DocumentsToDelete)
                    {
                        int companyDocumentId = Convert.ToInt32(item);
                        CompanyDocument companyDocument = rpoContext.CompanyDocuments.Where(x => x.Id == companyDocumentId).FirstOrDefault();
                        if (companyDocument != null)
                        {
                            rpoContext.CompanyDocuments.Remove(companyDocument);
                            var path = HttpRuntime.AppDomainAppPath;
                            string directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.ComanyDocumentPath));
                            string directoryDelete = Convert.ToString(companyDocument.Id) + "_" + companyDocument.DocumentPath;
                            string deletefilename = System.IO.Path.Combine(directoryName, directoryDelete);

                            if (File.Exists(deletefilename))
                            {
                                File.Delete(deletefilename);
                            }
                        }
                    }
                }
                try
                {
                    rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok(MapCompanyToCompanyDTO(company));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the company.
        /// </summary>
        /// <param name="companyDTO">The company dto.</param>
        /// <returns>Create a new company.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CompanyDTO))]
        public IHttpActionResult PostCompany(CompanyDTODetail companyDTO)
        {
            int generalContractor = 13;
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddCompany))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                Company company = new Company();

                company.HICNumber = companyDTO.HICNumber;
                company.CTLicenseNumber = companyDTO.CTLicenseNumber;
                company.IBMNumber = companyDTO.IBMNumber;
                company.InsuranceDisability = companyDTO.InsuranceDisability != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.InsuranceDisability), DateTimeKind.Utc) : companyDTO.InsuranceDisability;
                company.InsuranceGeneralLiability = companyDTO.InsuranceGeneralLiability != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.InsuranceGeneralLiability), DateTimeKind.Utc) : companyDTO.InsuranceGeneralLiability;
                company.InsuranceObstructionBond = companyDTO.InsuranceObstructionBond != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.InsuranceObstructionBond), DateTimeKind.Utc) : companyDTO.InsuranceObstructionBond;
                company.InsuranceWorkCompensation = companyDTO.InsuranceWorkCompensation != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.InsuranceWorkCompensation), DateTimeKind.Utc) : companyDTO.InsuranceWorkCompensation;
                company.SpecialInspectionAgencyExpiry = companyDTO.SpecialInspectionAgencyExpiry != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.SpecialInspectionAgencyExpiry), DateTimeKind.Utc) : companyDTO.SpecialInspectionAgencyExpiry;
                company.TrackingExpiry = companyDTO.TrackingExpiry != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.TrackingExpiry), DateTimeKind.Utc) : companyDTO.TrackingExpiry;
                company.HICExpiry = companyDTO.HICExpiry != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.HICExpiry), DateTimeKind.Utc) : companyDTO.HICExpiry;
                company.CTExpirationDate = companyDTO.CTExpirationDate != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.CTExpirationDate), DateTimeKind.Utc) : companyDTO.CTExpirationDate;

               
                company.SpecialInspectionAgencyNumber = companyDTO.SpecialInspectionAgencyNumber;
                company.Name = companyDTO.Name;
                company.Notes = companyDTO.Notes;
                company.TaxIdNumber = companyDTO.TaxIdNumber;
                company.TrackingNumber = companyDTO.TrackingNumber;
                company.Addresses = new HashSet<Address>();
                company.CompanyTypes = new HashSet<CompanyType>();
                //mb
                company.CompanyLicenses = new HashSet<CompanyLicense>();
                company.Url = companyDTO.Url;
                company.LastModifiedDate = DateTime.UtcNow;
                company.CreatedDate = DateTime.UtcNow;               
                company.DOTInsuranceWorkCompensation = companyDTO.DOTInsuranceWorkCompensation != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.DOTInsuranceWorkCompensation), DateTimeKind.Utc) : companyDTO.DOTInsuranceWorkCompensation;
                company.DOTInsuranceGeneralLiability = companyDTO.DOTInsuranceGeneralLiability != null ? DateTime.SpecifyKind(Convert.ToDateTime(companyDTO.DOTInsuranceGeneralLiability), DateTimeKind.Utc) : companyDTO.DOTInsuranceGeneralLiability;
                              
                Tools.EncryptDecrypt ED = new Tools.EncryptDecrypt();
                if (!string.IsNullOrWhiteSpace(companyDTO.EmailAddress))
                {
                    string EncryptedEmailAddress = ED.Encrypt(companyDTO.EmailAddress, Convert.ToString(WebConfigurationManager.AppSettings["saltPassword"]));
                    company.EmailAddress = EncryptedEmailAddress;
                };
                if (!string.IsNullOrWhiteSpace(companyDTO.EmailPassword))
                {
                    company.EmailPassword = ED.Encrypt(companyDTO.EmailPassword, Convert.ToString(WebConfigurationManager.AppSettings["saltPassword"])); ;
                };

                if (employee != null)
                {
                    company.CreatedBy = employee.Id;
                }                
                if (companyDTO.CompanyLicenses != null)
                {
                    foreach (var license in companyDTO.CompanyLicenses)
                    {
                        rpoContext.Entry(
                            new CompanyLicense
                            {
                                ExpirationLicenseDate = license.ExpirationLicenseDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(license.ExpirationLicenseDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : license.ExpirationLicenseDate,
                                Id = license.Id,
                                CompanyLicenseType = rpoContext.CompanyLicenseTypes.Find(license.IdCompanyLicenseType),
                                IdCompany = companyDTO.Id,
                                IdCompanyLicenseType = license.IdCompanyLicenseType,
                                Number = license.Number
                            }).State = EntityState.Added;
                    }
                }

                rpoContext.Companies.Add(company);

                if (companyDTO.Addresses != null)
                {
                    foreach (var address in companyDTO.Addresses)
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
                                IdCompany = companyDTO.Id,
                                Phone = address.Phone,
                                ZipCode = address.ZipCode,
                                Fax = address.FaxNumber
                            }).State = address.Id == 0 ? EntityState.Added : EntityState.Modified;
                    }
                }

                if (companyDTO.CompanyTypes != null)
                {
                    foreach (var companyType in companyDTO.CompanyTypes)
                    {
                        company.CompanyTypes.Add(rpoContext.CompanyTypes.Find(companyType.Id));
                        //mb
                        if (companyType.Id == generalContractor) //Id 13 ="General contractor" 
                        {
                            company.IDResponsibility = companyDTO.IdResponsibility;
                            if (company.Responsibilities != null && companyDTO.IdResponsibility != null)
                            {
                                company.Responsibilities.ResponsibilityName = companyDTO.ResponsibilityName != null ? companyDTO.ResponsibilityName : null;
                            }
                            else if (companyDTO.IdResponsibility != null)
                            {
                                company.Responsibilities = this.rpoContext.Responsibilities.Where(r => r.Id == companyDTO.IdResponsibility).FirstOrDefault();
                                company.Responsibilities.Id = Convert.ToInt32(companyDTO.IdResponsibility);
                                company.Responsibilities.ResponsibilityName = companyDTO.ResponsibilityName != null ? companyDTO.ResponsibilityName : null;
                            }
                            else
                            {
                                company.Responsibilities = null;
                            }                            
                        }
                    }
                }

                rpoContext.SaveChanges();

                Company newcompany = rpoContext.Companies.Find(company.Id);
                return Ok(MapCompanyToCompanyDTO(newcompany));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the company.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Delete the company.</returns>
        /// <exception cref="RpoBusinessException">Cannot delete company as it is associated with contacts/jobs/rfps!</exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(Company))]
        public IHttpActionResult DeleteCompany(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddCompany))
            {
                Company company = rpoContext.Companies.Find(id);
                if (company == null)
                    return this.NotFound();

                if (rpoContext.Jobs.Any(j => j.IdCompany == id) || rpoContext.Rfps.Any(r => r.IdCompany == id) || rpoContext.Contacts.Any(c => c.IdCompany == id))
                    throw new RpoBusinessException("Cannot delete company as it is associated with contacts/jobs/rfps!");

                if (company.Addresses.Any())
                    foreach (Address ad in company.Addresses.ToList())
                        rpoContext.Addresses.Remove(ad);

                rpoContext.Companies.Remove(company);
                rpoContext.SaveChanges();

                return Ok(company);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the bis.
        /// </summary>
        /// <param name="licensetype">The licensetype.</param>
        /// <param name="licno">The licno.</param>
        /// <returns>Get the detal of company company types expiry details.</returns>
        [Authorize]
        [RpoAuthorize]
        [Route("api/bis/{licensetype}/{licno}")]
        [ResponseType(typeof(Company))]
        public IHttpActionResult GetBis(string licensetype, string licno)
        {
            var result = new BisDTO();

            string urlAddress = $"https://a810-bisweb.nyc.gov/bisweb/LicenseQueryServlet?licensetype={licensetype}&licno={licno}&requestid=1";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12;

            HtmlDocument doc = new HtmlWeb().Load(urlAddress);

            var descendants = doc.DocumentNode.Descendants();

            if (descendants.Any(n => n.InnerText == "LICENSE RECORD NOT FOUND"))
                return this.NotFound();

            var insuranceGeneralLiability = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "General Liability");

            var insuranceWorkCompensation = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Workers' Compensation");

            var insuranceDisability = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Disability");

            var generalContractorExpiry = descendants.FirstOrDefault(n => n.Name == "b" && n.InnerText == "Expiration:");

            var concreteTestingLabExpiry = descendants.FirstOrDefault(n => n.Name == "b" && n.InnerText == "Expiration:");

            var specialInspectionAgencyExpiry = descendants.FirstOrDefault(n => n.Name == "b" && n.InnerText == "Expiration:");

            var concreteTestingLabNumber = descendants.FirstOrDefault(n => n.Name == "b" && n.InnerText == "License #:");

            var specialInspectionAgencyNumber = descendants.FirstOrDefault(n => n.Name == "b" && n.InnerText == "License #:");

            var endorsementsDemolition = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText.Contains("DM - DEMOLITION"));
            var endorsementsConcrete = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText.Contains("CC - CONCRETE"));
            var endorsementsConstruction = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText.Contains("CN - CONSTRUCTION"));



            DateTime dt;

            if (insuranceGeneralLiability != null && DateTime.TryParseExact(insuranceGeneralLiability.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                result.InsuranceGeneralLiability = dt;

            }

            if (insuranceWorkCompensation != null && DateTime.TryParseExact(insuranceWorkCompensation.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                result.InsuranceWorkCompensation = dt;
            }

            if (insuranceDisability != null && DateTime.TryParseExact(insuranceDisability.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                result.InsuranceDisability = dt;
            }

            if (licensetype == "G" && concreteTestingLabExpiry != null)
            {
                string generalContractor = Convert.ToString(generalContractorExpiry.ParentNode.InnerText).Replace("Expiration:&nbsp;&nbsp;", "");
                if (DateTime.TryParseExact(generalContractor, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                {
                    result.GeneralContractorExpiry = dt;
                }
            }

            if (licensetype == "C" && concreteTestingLabExpiry != null)
            {
                string concreteTestingLab = Convert.ToString(concreteTestingLabExpiry.ParentNode.InnerText).Replace("Expiration:&nbsp;&nbsp;", "");
                if (DateTime.TryParseExact(concreteTestingLab, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                {
                    result.ConcreteTestingLabExpiry = dt;
                }
            }

            if (licensetype == "C" && concreteTestingLabNumber != null)
            {
                string concreteTestingLab = Convert.ToString(concreteTestingLabNumber.ParentNode.InnerText).Replace("License #:&nbsp;&nbsp;", "");
                result.ConcreteTestingLabNumber = concreteTestingLab;
            }

            if (licensetype == "I" && specialInspectionAgencyExpiry != null)
            {
                string specialInspectionAgency = Convert.ToString(specialInspectionAgencyExpiry.ParentNode.InnerText).Replace("Expiration:&nbsp;&nbsp;", "");
                if (DateTime.TryParseExact(specialInspectionAgency, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                {
                    result.SpecialInspectionAgencyExpiry = dt;
                }
            }

            if (licensetype == "I" && specialInspectionAgencyNumber != null)
            {
                string specialInspectionAgency = Convert.ToString(specialInspectionAgencyNumber.ParentNode.InnerText).Replace("License #:&nbsp;&nbsp;", "");
                result.SpecialInspectionAgencyNumber = specialInspectionAgency;
            }

            if (endorsementsDemolition != null)
            {
                result.EndorsementsDemolition = true;
            }

            if (endorsementsConcrete != null)
            {
                result.EndorsementsConcrete = true;
            }

            if (endorsementsConstruction != null)
            {
                result.EndorsementsConstruction = true;
            }

            return Ok(result);
        }

        /// <summary>
        /// Posts the bis number.
        /// </summary>
        /// <param name="bisRequest">The bis request.</param>
        /// <returns>get the company expiry detail through company.</returns>
        [Authorize]
        [RpoAuthorize]
        [Route("api/bisNumber")]
        [ResponseType(typeof(Company))]
        [HttpPost]
        public IHttpActionResult PostBisNumber(BISRequestDTO bisRequest)
        {
            if (bisRequest.LicenseType == "CONCRETE TESTING LAB")
            {
                bisRequest.LicenseType = "CONCRETE TEST LAB / SAFETY MANAGER";
            }

            bisRequest.BusinessName = HttpContext.Current.Server.UrlEncode(bisRequest.BusinessName);
            bisRequest.LicenseType = HttpContext.Current.Server.UrlEncode(bisRequest.LicenseType);
            string businessnameWhereCondition = string.Empty;
            if (!string.IsNullOrEmpty(bisRequest.LicenseNumber))
            {
                businessnameWhereCondition = "$where=license_number=" + Convert.ToString(bisRequest.LicenseNumber).ToUpper();
            }
            else
            {
                businessnameWhereCondition = "$where=business_name%20like%20%27%25" + Convert.ToString(bisRequest.BusinessName).ToUpper() + "%25%27";
            }

            string urlAddress = $"http://data.cityofnewyork.us/resource/rj63-4334.json?$limit=5000&$$app_token=jL9ARtblKn6UD7NcmMexG1NIi&{businessnameWhereCondition}&license_type={bisRequest.LicenseType}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                string data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
                return Ok(data);
            }
            else
            {
                return BadRequest(response.StatusCode.ToString());
            }

        }

        /// <summary>
        /// Gets the jobs.
        /// </summary>
        /// <param name="IdCompany">The identifier company.</param>
        /// <returns>Get the job list against the company.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/companies/{idCompany}/jobs")]
        [ResponseType(typeof(IEnumerable<Job>))]
        public IHttpActionResult GetJobs(int IdCompany)
        {
            Company company = rpoContext.Companies.Find(IdCompany);
            if (company == null)
                return this.NotFound();

            rpoContext.Configuration.LazyLoadingEnabled = false;

            return Ok(rpoContext
                .Jobs
                .Include("Applications")
                .Include("RfpAddress")
                .Include("Rfp")
                .Include("Borough")
                .Include("Company")
                .Include("Company.Addresses")
                .Include("JobTypes")
                .Include("Contact")
                .Include("Contact.Prefix")
                .Include("ProjectManager")
                //.Include("ProjectCoordinator")
                //.Include("SignoffCoordinator")
                .Include("Contacts")
                .Include("Documents")
                .Include("Transmittals")
                .Include("Tasks").Where(j => j.IdCompany == IdCompany));
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="emailDTO">The email dto.</param>
        /// <returns>Comapny email send</returns>
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
        [Authorize]
        [RpoAuthorize]
        [Route("api/companies/{id}/email/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult SendEmail(int id, [FromBody] CompanyEmailDTO emailDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Company company = rpoContext.Companies.Find(id);

            if (company == null)
            {
                return this.NotFound();
            }

            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

            int responseIdCompany = 0;
            int responseIdCompanyEmailHistory = 0;

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

                var contactTo = rpoContext.Contacts.Where(x => x.Id == emailDTO.IdContactAttention).FirstOrDefault();
                if (contactTo == null)
                {
                    throw new RpoBusinessException("Contact Id not found");
                }

                if (string.IsNullOrEmpty(contactTo.Email))
                {
                    throw new RpoBusinessException($"Contact {contactTo.FirstName} not has a valid e-mail");
                }

                CompanyEmailHistory companyEmailHistory = new CompanyEmailHistory();
                companyEmailHistory.IdCompany = company.Id;
                companyEmailHistory.IdFromEmployee = emailDTO.IdFromEmployee;
                companyEmailHistory.IdToCompany = emailDTO.IdContactsTo;
                companyEmailHistory.IdContactAttention = emailDTO.IdContactAttention;
                companyEmailHistory.IdTransmissionType = emailDTO.IdTransmissionType;
                companyEmailHistory.IdEmailType = emailDTO.IdEmailType;
                companyEmailHistory.SentDate = DateTime.UtcNow;
                if (employee != null)
                {
                    companyEmailHistory.IdSentBy = employee.Id;
                }
                companyEmailHistory.EmailSubject = emailDTO.Subject;
                companyEmailHistory.EmailMessage = emailDTO.Body;
                companyEmailHistory.IsAdditionalAtttachment = emailDTO.IsAdditionalAtttachment;
                companyEmailHistory.IsEmailSent = false;

                rpoContext.CompanyEmailHistories.Add(companyEmailHistory);
                rpoContext.SaveChanges();

                /*CC Histroy code commented with mansih */
                //foreach (int idCc in emailDTO.IdContactsCc.Distinct())
                //{
                //    CompanyEmailCCHistory companyEmailCCHistory = new CompanyEmailCCHistory();
                //    companyEmailCCHistory.IdContact = idCc;
                //    companyEmailCCHistory.IdCompanyEmailHistory = companyEmailHistory.Id;

                //    rpoContext.CompanyEmailCCHistories.Add(companyEmailCCHistory);
                //    rpoContext.SaveChanges();
                //}

                responseIdCompany = company.Id;
                responseIdCompanyEmailHistory = companyEmailHistory.Id;

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

                if (string.IsNullOrEmpty(contactTo.Email))
                {
                    throw new RpoBusinessException($"Contact {contactTo.FirstName} not has a valid e-mail");
                }

                to.Add(new KeyValuePair<string, string>(contactTo.Email, contactTo.FirstName + " " + contactTo.LastName));


                var attachments = new List<string>();

                Employee employeeFrom = rpoContext.Employees.Find(emailDTO.IdFromEmployee);

                cc.Add(new KeyValuePair<string, string>(employeeFrom.Email, employeeFrom.FirstName + " " + employeeFrom.LastName));

                CompanyEmailHistory companyEmailHistory = new CompanyEmailHistory();
                companyEmailHistory.IdCompany = company.Id;
                companyEmailHistory.IdFromEmployee = emailDTO.IdFromEmployee;
                companyEmailHistory.IdToCompany = emailDTO.IdContactsTo;
                companyEmailHistory.IdContactAttention = emailDTO.IdContactAttention;
                companyEmailHistory.IdTransmissionType = emailDTO.IdTransmissionType;
                companyEmailHistory.IdEmailType = emailDTO.IdEmailType;
                companyEmailHistory.SentDate = DateTime.UtcNow;
                if (employee != null)
                {
                    companyEmailHistory.IdSentBy = employee.Id;
                }
                companyEmailHistory.EmailSubject = emailDTO.Subject;
                companyEmailHistory.EmailMessage = emailDTO.Body;
                companyEmailHistory.IsAdditionalAtttachment = emailDTO.IsAdditionalAtttachment;
                companyEmailHistory.IsEmailSent = true;

                rpoContext.CompanyEmailHistories.Add(companyEmailHistory);
                rpoContext.SaveChanges();

                //foreach (int idCc in emailDTO.IdContactsCc.Distinct())
                //{
                //    CompanyEmailCCHistory companyEmailCCHistory = new CompanyEmailCCHistory();
                //    companyEmailCCHistory.IdContact = idCc;
                //    companyEmailCCHistory.IdCompanyEmailHistory = companyEmailHistory.Id;

                //    rpoContext.CompanyEmailCCHistories.Add(companyEmailCCHistory);
                //    rpoContext.SaveChanges();
                //}

                responseIdCompany = company.Id;
                responseIdCompanyEmailHistory = companyEmailHistory.Id;

                companyEmailHistory.IsEmailSent = true;
                rpoContext.SaveChanges();

                TransmissionType transmissionType = rpoContext.TransmissionTypes.FirstOrDefault(x => x.Id == companyEmailHistory.IdTransmissionType);
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

            return Ok(new { Message = "Mail sent successfully", idCompany = responseIdCompany.ToString(), idCompanyEmailHistory = responseIdCompanyEmailHistory.ToString() });
        }

        /// <summary>
        /// Posts the attachment.
        /// </summary>
        /// <returns>Task&lt;HttpResponseMessage&gt; Save the document on folder and save into database.</returns>
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
        [Route("api/companies/Attachment")]
        [ResponseType(typeof(CompanyEmailAttachmentHistory))]
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
            int idCompany = Convert.ToInt32(formData["idCompany"]);
            int idCompanyEmailHistory = Convert.ToInt32(formData["idCompanyEmailHistory"]);

            var companyEmail = rpoContext.CompanyEmailHistories.Include("CompanyEmailCCHistories.Contact").Include("Company").Include("ContactAttention").Include("FromEmployee").Where(x => x.Id == idCompanyEmailHistory).FirstOrDefault();

            if (companyEmail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var cc = new List<KeyValuePair<string, string>>();

            foreach (CompanyEmailCCHistory item in companyEmail.CompanyEmailCCHistories)
            {
                if (item == null)
                    throw new RpoBusinessException("Contact Id not found");

                if (item.Contact.Email == null)
                    throw new RpoBusinessException($"Contact {item.Contact.FirstName} not has a valid e-mail");

                cc.Add(new KeyValuePair<string, string>(item.Contact.Email, item.Contact.FirstName + " " + item.Contact.LastName));
            }

            var to = new List<KeyValuePair<string, string>>();

            var contactTo = companyEmail.ContactAttention;
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

            Employee employeeFrom = companyEmail.FromEmployee;

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

                    CompanyEmailAttachmentHistory companyEmailAttachmentHistory = new CompanyEmailAttachmentHistory();
                    companyEmailAttachmentHistory.DocumentPath = thisFileName;
                    companyEmailAttachmentHistory.IdCompanyEmailHistory = idCompanyEmailHistory;
                    companyEmailAttachmentHistory.Name = thisFileName;
                    rpoContext.CompanyEmailAttachmentHistories.Add(companyEmailAttachmentHistory);
                    rpoContext.SaveChanges();


                    directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.CompanyAttachmentsPath));

                    string directoryFileName = Convert.ToString(companyEmailAttachmentHistory.Id) + "_" + thisFileName;
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


            TransmissionType transmissionType = rpoContext.TransmissionTypes.FirstOrDefault(x => x.Id == companyEmail.IdTransmissionType);
            if (transmissionType != null && transmissionType.IsSendEmail)
            {
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/MasterEmailTemplate.htm")))
                {
                    body = reader.ReadToEnd();
                }

                string emailBody = body;
                emailBody = emailBody.Replace("##EmailBody##", companyEmail.EmailMessage);

                //List<TransmissionTypeDefaultCC> transmissionTypeDefaultCC = rpoContext.TransmissionTypeDefaultCCs.Include("Employee").Where(x => x.IdTransmissionType == companyEmail.IdTransmissionType).ToList();
                List<TransmissionTypeDefaultCC> transmissionTypeDefaultCC = rpoContext.TransmissionTypeDefaultCCs.Include("Employee").Where(x => x.IdEamilType == companyEmail.IdEmailType).ToList();
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
                        companyEmail.EmailSubject,
                        emailBody,
                        attachments
                    );
            }

            companyEmail.IsEmailSent = true;
            rpoContext.SaveChanges();

            var companyEmailAttachmentHistoryList = rpoContext.CompanyEmailAttachmentHistories
                .Where(x => x.IdCompanyEmailHistory == idCompanyEmailHistory).ToList();
            var response = Request.CreateResponse<List<CompanyEmailAttachmentHistory>>(HttpStatusCode.OK, companyEmailAttachmentHistoryList);
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
        /// Companies the exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool CompanyExists(int id)
        {
            return rpoContext.Companies.Count(e => e.Id == id) > 0;
        }

        //[Route("api/jobtransmittals/{idJobTransmittal}/Print")]
        //[ResponseType(typeof(string))]
        //[HttpGet]
        //public async Task<HttpResponseMessage> Print(int idJobTransmittal)
        //{
        //    var companyEmail = db.CompanyEmailHistories.Include("ToCompany.Addresses").Include("Job.RfpAddress.Borough").Include("TransmissionType")
        //        .Include("EmailType").Include("ContactAttention").Include("FromEmployee").Include("JobTransmittalCCs.Contact").Include("JobTransmittalAttachments")
        //        .Include("Job.Company").FirstOrDefault(x => x.Id == idJobTransmittal);

        //    if (companyEmail == null)
        //    {
        //        return new HttpResponseMessage(HttpStatusCode.NotFound);
        //    }

        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        string filename = "JobTransmittal-" + idJobTransmittal + ".pdf";
        //        string transmittalNumber = companyEmail != null && companyEmail.TransmittalNumber != null ? companyEmail.TransmittalNumber : string.Empty;
        //        string emailMessage = companyEmail != null && companyEmail.EmailMessage != null ? companyEmail.EmailMessage : string.Empty;
        //        string subject = companyEmail != null && companyEmail.EmailSubject != null ? companyEmail.EmailSubject : string.Empty;
        //        string jobNumber = companyEmail != null && companyEmail.Job != null && companyEmail.Job.JobNumber != null ? companyEmail.Job.JobNumber : string.Empty;
        //        string sentVia = companyEmail != null && companyEmail.TransmissionType != null ? companyEmail.TransmissionType.Name : string.Empty;
        //        string emailType = companyEmail != null && companyEmail.EmailType != null ? companyEmail.EmailType.Name : string.Empty;
        //        string companyName = companyEmail != null && companyEmail.Job != null && companyEmail.Job.Company != null && companyEmail.Job.Company.Name != null ? companyEmail.Job.Company.Name : string.Empty;

        //        Address companyaddress = companyEmail != null && companyEmail.ToCompany != null && companyEmail.ToCompany.Addresses != null ? companyEmail.ToCompany.Addresses.OrderByDescending(x => x.AddressType.DisplayOrder).FirstOrDefault() : new Address();

        //        string address2 = companyaddress.Address2 != null ? companyaddress.Address2 : string.Empty;
        //        string address1 = companyaddress.Address1 != null ? companyaddress.Address1 : string.Empty;
        //        string city = companyaddress.City != null ? companyaddress.City : string.Empty;
        //        string state = companyaddress.State != null ? companyaddress.State.Name : string.Empty;
        //        string zipCode = companyaddress.ZipCode != null ? companyaddress.ZipCode : string.Empty;

        //        string contactAttention = companyEmail.ContactAttention != null ? companyEmail.ContactAttention.FirstName + (companyEmail.ContactAttention.LastName != null ? " " + companyEmail.ContactAttention.LastName : string.Empty) : string.Empty;

        //        string from = companyEmail.FromEmployee != null ? companyEmail.FromEmployee.FirstName + (companyEmail.FromEmployee.LastName != null ? " " + companyEmail.FromEmployee.LastName : string.Empty) : string.Empty;

        //        string attentionName = companyEmail.ContactAttention != null && !string.IsNullOrEmpty(companyEmail.ContactAttention.LastName)
        //                               ? (companyEmail.ContactAttention.Prefix != null ? companyEmail.ContactAttention.Prefix.Name : string.Empty)
        //                               + (companyEmail.ContactAttention.Prefix != null ? companyEmail.ContactAttention.LastName : companyEmail.ContactAttention.FirstName + (!string.IsNullOrEmpty(companyEmail.ContactAttention.LastName) ? " " + companyEmail.ContactAttention.LastName : string.Empty)) : companyEmail.ContactAttention.FirstName;


        //        Document document = new Document(PageSize.A4);
        //        PdfWriter writer = PdfWriter.GetInstance(document, stream);
        //        document.Open();

        //        Image logo = Image.GetInstance(HttpContext.Current.Server.MapPath("~\\HTML\\images\\logo.jpg"));
        //        logo.Alignment = Image.ALIGN_CENTER;
        //        logo.ScaleToFit(120, 60);
        //        logo.SetAbsolutePosition(260, 760);

        //        PdfPTable table = new PdfPTable(3);
        //        table.WidthPercentage = 100;
        //        PdfPCell cell = new PdfPCell(logo);
        //        cell.Colspan = 3;
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 3;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("NEW YORK CITY AGENCY FILINGS, APPROVALS & PERMITS", new Font(Font.FontFamily.HELVETICA, 9, 1)));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Colspan = 3;
        //        cell.VerticalAlignment = Element.ALIGN_TOP;
        //        cell.PaddingBottom = 10;
        //        cell.Border = PdfPCell.BOTTOM_BORDER;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("146 WEST 29TH STREET, SUITE 2E", new Font(Font.FontFamily.HELVETICA, 9, 1)));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(""));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("TEL: 212-566-5110", new Font(Font.FontFamily.HELVETICA, 9, 1)));
        //        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("NEW YORK, NY 10001", new Font(Font.FontFamily.HELVETICA, 9, 1)));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(""));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("FAX: 212-566-5112", new Font(Font.FontFamily.HELVETICA, 9, 1)));
        //        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 3;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(emailType, new Font(Font.FontFamily.HELVETICA, 11, 1)));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 3;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 3;
        //        table.AddCell(cell);

        //        string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
        //        DateTime sentDate = companyEmail.SentDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(companyEmail.SentDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : companyEmail.SentDate;

        //        cell = new PdfPCell(new Phrase("Date: " + sentDate.ToShortDateString(), new Font(Font.FontFamily.HELVETICA, 11)));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 3;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 3;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(companyName, new Font(Font.FontFamily.HELVETICA, 11)));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(""));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Colspan = 1;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Transmittal#: " + transmittalNumber, new Font(Font.FontFamily.HELVETICA, 11)));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(address1 + " " + address2, new Font(Font.FontFamily.HELVETICA, 11)));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(""));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Colspan = 1;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Sent Via: " + sentVia, new Font(Font.FontFamily.HELVETICA, 11)));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(city + " " + state + " " + zipCode, new Font(Font.FontFamily.HELVETICA, 11)));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(""));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Colspan = 1;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Job#: " + jobNumber, new Font(Font.FontFamily.HELVETICA, 11)));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("ATTN: " + contactAttention, new Font(Font.FontFamily.HELVETICA, 11)));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(""));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Colspan = 2;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 3;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("RE: " + subject, new Font(Font.FontFamily.HELVETICA, 11, 1)));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 3;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 3;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Dear " + attentionName, new Font(Font.FontFamily.HELVETICA, 11)));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 3;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 3;
        //        table.AddCell(cell);

        //        document.Add(table);

        //        using (StringReader sr = new StringReader(emailMessage))
        //        {
        //            List<IElement> elements = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(sr, null);
        //            if (elements != null)
        //            {
        //                foreach (IElement e in elements)
        //                {
        //                    document.Add(e);
        //                }
        //            }
        //        }

        //        table = new PdfPTable(3);
        //        table.WidthPercentage = 100;

        //        cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 3;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 3;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Paragraph("Very truly yours,", new Font(Font.FontFamily.HELVETICA, 11)));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 3;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Paragraph(from, new Font(Font.FontFamily.HELVETICA, 11)));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 3;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 3;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 3;
        //        table.AddCell(cell);

        //        PdfPTable tableAttachment = new PdfPTable(3);
        //        tableAttachment.SetWidths(new float[] { 10, 35, 55 });
        //        tableAttachment.WidthPercentage = 100;

        //        PdfPCell cellAttachment = new PdfPCell(new Paragraph("Enclosure", new Font(Font.FontFamily.HELVETICA, 11, 1)));
        //        cellAttachment.Colspan = 3;
        //        cellAttachment.Border = PdfPCell.NO_BORDER;
        //        cellAttachment.PaddingBottom = 10;
        //        cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
        //        tableAttachment.AddCell(cellAttachment);

        //        int attachementCount = 0;
        //        if (companyEmail.JobTransmittalAttachments != null)
        //        {
        //            foreach (JobTransmittalAttachment item in companyEmail.JobTransmittalAttachments)
        //            {
        //                if (attachementCount == 0)
        //                {
        //                    cellAttachment = new PdfPCell(new Paragraph("Copies", new Font(Font.FontFamily.HELVETICA, 11, 1)));
        //                    cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                    cellAttachment.Padding = 3;
        //                    tableAttachment.AddCell(cellAttachment);

        //                    cellAttachment = new PdfPCell(new Paragraph("Name", new Font(Font.FontFamily.HELVETICA, 11, 1)));
        //                    cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                    cellAttachment.Padding = 3;
        //                    tableAttachment.AddCell(cellAttachment);

        //                    cellAttachment = new PdfPCell(new Paragraph("Description", new Font(Font.FontFamily.HELVETICA, 11, 1)));
        //                    cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                    cellAttachment.Padding = 3;
        //                    tableAttachment.AddCell(cellAttachment);

        //                    cellAttachment = new PdfPCell(new Paragraph(Convert.ToString(item.Copies), new Font(Font.FontFamily.HELVETICA, 11)));
        //                    cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                    cellAttachment.Padding = 3;
        //                    tableAttachment.AddCell(cellAttachment);

        //                    cellAttachment = new PdfPCell(new Paragraph(item.Name, new Font(Font.FontFamily.HELVETICA, 11)));
        //                    cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                    cellAttachment.Padding = 3;
        //                    tableAttachment.AddCell(cellAttachment);

        //                    cellAttachment = new PdfPCell(new Paragraph("", new Font(Font.FontFamily.HELVETICA, 11)));
        //                    cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                    cellAttachment.Padding = 3;
        //                    tableAttachment.AddCell(cellAttachment);

        //                    attachementCount = attachementCount + 1;
        //                }
        //                else
        //                {
        //                    cellAttachment = new PdfPCell(new Paragraph(Convert.ToString(item.Copies), new Font(Font.FontFamily.HELVETICA, 11)));
        //                    cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                    cell.Padding = 3;
        //                    tableAttachment.AddCell(cellAttachment);

        //                    cellAttachment = new PdfPCell(new Paragraph(item.Name, new Font(Font.FontFamily.HELVETICA, 11)));
        //                    cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                    cell.Padding = 3;
        //                    tableAttachment.AddCell(cellAttachment);

        //                    cellAttachment = new PdfPCell(new Paragraph("", new Font(Font.FontFamily.HELVETICA, 11)));
        //                    cellAttachment.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cellAttachment.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                    cell.Padding = 3;
        //                    tableAttachment.AddCell(cellAttachment);
        //                }
        //            }
        //        }

        //        cell = new PdfPCell(tableAttachment);
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 3;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 3;
        //        table.AddCell(cell);

        //        int ccCount = 0;
        //        if (companyEmail.JobTransmittalCCs != null)
        //        {
        //            foreach (JobTransmittalCC item in companyEmail.JobTransmittalCCs)
        //            {
        //                if (ccCount == 0)
        //                {
        //                    string contactcc = item.Contact != null ? item.Contact.FirstName + (item.Contact.LastName != null ? " " + item.Contact.LastName : string.Empty) : string.Empty;
        //                    cell = new PdfPCell(new Paragraph("cc: " + contactcc, new Font(Font.FontFamily.HELVETICA, 11, 2)));
        //                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell.Border = PdfPCell.NO_BORDER;
        //                    cell.Colspan = 3;
        //                    table.AddCell(cell);
        //                    ccCount = ccCount + 1;
        //                }
        //                else
        //                {
        //                    string contactcc = item.Contact != null ? item.Contact.FirstName + (item.Contact.LastName != null ? " " + item.Contact.LastName : string.Empty) : string.Empty;
        //                    cell = new PdfPCell(new Paragraph(contactcc, new Font(Font.FontFamily.HELVETICA, 11, 2)));
        //                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell.Border = PdfPCell.NO_BORDER;
        //                    cell.Colspan = 3;
        //                    table.AddCell(cell);
        //                }
        //            }
        //        }

        //        document.Add(table);
        //        document.Close();
        //        writer.Close();

        //        var result = new HttpResponseMessage(HttpStatusCode.OK)
        //        {
        //            Content = new ByteArrayContent(stream.ToArray())
        //        };
        //        result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        //        result.Content.Headers.ContentDisposition =
        //            new ContentDispositionHeaderValue("inline")
        //            {
        //                FileName = filename
        //            };
        //        return result;
        //    }
        //}

        private ContactDTO FormatContact(Contact c)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

            ContactDTO contactDTO = new ContactDTO();

            contactDTO.Id = c.Id;
            contactDTO.Prefix = c.Prefix != null ? c.Prefix.Name : string.Empty;
            contactDTO.OtherPhone = c.OtherPhone;
            contactDTO.WorkPhoneExt = c.WorkPhoneExt;
            contactDTO.FirstName = c.FirstName;
            contactDTO.LastName = c.LastName;
            contactDTO.IdCompany = c.IdCompany;
            contactDTO.Company = c.Company != null ? c.Company.Name : null;
            if (c.IdPrimaryCompanyAddress != null)
            {
                contactDTO.Address = c.Company.Addresses != null && c.Company.Addresses.Count > 0 ? c.Company.Addresses.Where(x => x.Id == c.IdPrimaryCompanyAddress).FirstOrDefault().Address1 : string.Empty;
                contactDTO.Address2 = c.Company.Addresses != null && c.Company.Addresses.Count > 0 ? c.Company.Addresses.Where(x => x.Id == c.IdPrimaryCompanyAddress).FirstOrDefault().Address2 : string.Empty;
                contactDTO.Ext = c.WorkPhoneExt;
                contactDTO.City = c.Company.Addresses != null && c.Company.Addresses.Count > 0 ? c.Company.Addresses.Where(x => x.Id == c.IdPrimaryCompanyAddress).FirstOrDefault().City : string.Empty;
                contactDTO.State = c.Company.Addresses != null && c.Company.Addresses.Count > 0 ? c.Company.Addresses.Where(x => x.Id == c.IdPrimaryCompanyAddress).FirstOrDefault().State.Acronym : string.Empty;
                contactDTO.Zip = c.Company.Addresses != null && c.Company.Addresses.Count > 0 ? c.Company.Addresses.Where(x => x.Id == c.IdPrimaryCompanyAddress).FirstOrDefault().ZipCode : string.Empty;
                contactDTO.Phone = c.Company.Addresses != null && c.Company.Addresses.Count > 0 ? c.Company.Addresses.Where(x => x.Id == c.IdPrimaryCompanyAddress).FirstOrDefault().Phone : string.Empty;
            }
            else
            {
                contactDTO.Address = c.Addresses != null && c.Addresses.Count > 0 ? c.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault().Address1 : string.Empty;
                contactDTO.Address2 = c.Addresses != null && c.Addresses.Count > 0 ? c.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault().Address2 : string.Empty;
                contactDTO.Ext = c.WorkPhoneExt;
                contactDTO.City = c.Addresses != null && c.Addresses.Count > 0 ? c.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault().City : string.Empty;
                contactDTO.State = c.Addresses != null && c.Addresses.Count > 0 ? c.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault().State.Acronym : string.Empty;
                contactDTO.Zip = c.Addresses != null && c.Addresses.Count > 0 ? c.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault().ZipCode : string.Empty;
                contactDTO.Phone = c.Addresses != null && c.Addresses.Count > 0 ? c.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault().Phone : string.Empty;
            }
            contactDTO.WorkPhone = c.WorkPhone;
            contactDTO.CellNumber = c.MobilePhone;
            contactDTO.Email = c.Email;
            contactDTO.License = c.ContactLicenses != null && c.ContactLicenses.Count > 0 ? c.ContactLicenses.FirstOrDefault().ContactLicenseType.Name : string.Empty;
            contactDTO.ContactLicenses = c.ContactLicenses;
            contactDTO.Addresses = c.Addresses != null && c.Addresses.Count > 0 ? c.Addresses.OrderBy(x => x.AddressType.DisplayOrder).ToList() : new List<Address>();
            contactDTO.LicenseNumber = c.ContactLicenses != null && c.ContactLicenses.Count > 0 ? c.ContactLicenses.FirstOrDefault().Number : string.Empty;
            contactDTO.Notes = c.Notes;
            contactDTO.IdContactTitle = c.IdContactTitle;
            contactDTO.ContactTitle = c.ContactTitle == null ? null : c.ContactTitle.Name;
            contactDTO.Suffix = c.Suffix == null ? null : c.Suffix.Description;
            contactDTO.IdSuffix = c.IdSuffix;
            contactDTO.ImageUrl = string.IsNullOrEmpty(c.ContactImagePath) ? string.Empty : APIUrl + "/" + Properties.Settings.Default.ContactImagePath + "/" + c.Id + "_" + c.ContactImagePath;
            contactDTO.ImageThumbUrl = string.IsNullOrEmpty(c.ContactImagePath) ? string.Empty : APIUrl + "/" + Properties.Settings.Default.ContactImagePath + "/" + c.Id + "_" + c.ContactImagePath;
            contactDTO.CreatedBy = c.CreatedBy;
            contactDTO.LastModifiedBy = c.LastModifiedBy != null ? c.LastModifiedBy : c.CreatedBy;
            contactDTO.CreatedByEmployeeName = c.CreatedByEmployee != null ? c.CreatedByEmployee.FirstName + " " + c.CreatedByEmployee.LastName : string.Empty;
            contactDTO.LastModifiedByEmployeeName = c.LastModifiedByEmployee != null ? c.LastModifiedByEmployee.FirstName + " " + c.LastModifiedByEmployee.LastName : (c.CreatedByEmployee != null ? c.CreatedByEmployee.FirstName + " " + c.CreatedByEmployee.LastName : string.Empty);
            contactDTO.CreatedDate = c.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(c.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : c.CreatedDate;
            contactDTO.LastModifiedDate = c.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(c.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (c.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(c.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : c.CreatedDate);
            contactDTO.IsActive = c.IsActive;
            return contactDTO;

        }
        /// <summary>
        /// Companies list to export into excel.
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <param name="companyType"></param>
        /// <returns>Create a excel file and write the all companies detail.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/companies/exporttoexcel")]
        public IHttpActionResult CompaniesExport([FromUri] CompanyAdvancedSearchParameters dataTableParameters, [FromUri]string companyType = null)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

            string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();

            DataSet ds = new DataSet();

            SqlParameter[] spParameter = new SqlParameter[1];
            spParameter[0] = new SqlParameter("@Search", SqlDbType.VarChar);
            spParameter[0].Direction = ParameterDirection.Input;
            if (dataTableParameters != null && dataTableParameters.GlobalSearchType != null && dataTableParameters.GlobalSearchType > 0)
            {
                spParameter[0].Value = dataTableParameters.GlobalSearchText;
            }
            ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "Company_List", spParameter);

            ds.Tables[0].TableName = "Data";

            string exportFilename = "ExportCompanies_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
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
        /// Create a excel File and write companies List.
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
            templateFileName = "CompaniesTemplate.xlsx";
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
                        iRow.GetCell(0).SetCellValue(dr["Name"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(0).SetCellValue(dr["Name"].ToString());
                    }

                    if (iRow.GetCell(1) != null)
                    {
                        iRow.GetCell(1).SetCellValue(dr["Address"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(1).SetCellValue(dr["Address"].ToString());
                    }

                    if (iRow.GetCell(2) != null)
                    {
                        iRow.GetCell(2).SetCellValue(dr["Phone"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(2).SetCellValue(dr["Phone"].ToString());
                    }

                    if (iRow.GetCell(3) != null)
                    {
                        iRow.GetCell(3).SetCellValue(dr["Url"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(3).SetCellValue(dr["Url"].ToString());
                    }

                    if (iRow.GetCell(4) != null)
                    {
                        iRow.GetCell(4).SetCellValue(dr["TrackingNumber"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(4).SetCellValue(dr["TrackingNumber"].ToString());
                    }

                    if (iRow.GetCell(5) != null)
                    {
                        iRow.GetCell(5).SetCellValue(dr["IBMNumber"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(5).SetCellValue(dr["IBMNumber"].ToString());
                    }

                    if (iRow.GetCell(6) != null)
                    {
                        iRow.GetCell(6).SetCellValue(dr["TaxIdNumber"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(6).SetCellValue(dr["TaxIdNumber"].ToString());
                    }

                    if (iRow.GetCell(7) != null)
                    {
                        iRow.GetCell(7).SetCellValue(dr["HICNumber"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(7).SetCellValue(dr["HICNumber"].ToString());
                    }

                    if (iRow.GetCell(8) != null)
                    {
                        iRow.GetCell(8).SetCellValue(dr["InsuranceWorkCompensation"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(8).SetCellValue(dr["InsuranceWorkCompensation"].ToString());
                    }

                    if (iRow.GetCell(9) != null)
                    {
                        iRow.GetCell(9).SetCellValue(dr["InsuranceDisability"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(9).SetCellValue(dr["InsuranceDisability"].ToString());
                    }

                    if (iRow.GetCell(10) != null)
                    {
                        iRow.GetCell(10).SetCellValue(dr["InsuranceGeneralLiability"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(10).SetCellValue(dr["InsuranceDisability"].ToString());
                    }

                    if (iRow.GetCell(11) != null)
                    {
                        iRow.GetCell(11).SetCellValue(dr["insuranceObstructionBond"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(11).SetCellValue(dr["insuranceObstructionBond"].ToString());
                    }
                    if (iRow.GetCell(12) != null)
                    {
                        iRow.GetCell(12).SetCellValue(dr["Notes"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(12).SetCellValue(dr["Notes"].ToString());
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
                    iRow.GetCell(9).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(10).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(11).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(12).CellStyle = leftAlignCellStyle;
                }
                else
                {
                    iRow = sheet.CreateRow(index);

                    if (iRow.GetCell(0) != null)
                    {
                        iRow.GetCell(0).SetCellValue(dr["Name"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(0).SetCellValue(dr["Name"].ToString());
                    }

                    if (iRow.GetCell(1) != null)
                    {
                        iRow.GetCell(1).SetCellValue(dr["Address"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(1).SetCellValue(dr["Address"].ToString());
                    }

                    if (iRow.GetCell(2) != null)
                    {
                        iRow.GetCell(2).SetCellValue(dr["Phone"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(2).SetCellValue(dr["Phone"].ToString());
                    }

                    if (iRow.GetCell(3) != null)
                    {
                        iRow.GetCell(3).SetCellValue(dr["Url"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(3).SetCellValue(dr["Url"].ToString());
                    }

                    if (iRow.GetCell(4) != null)
                    {
                        iRow.GetCell(4).SetCellValue(dr["TrackingNumber"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(4).SetCellValue(dr["TrackingNumber"].ToString());
                    }

                    if (iRow.GetCell(5) != null)
                    {
                        iRow.GetCell(5).SetCellValue(dr["IBMNumber"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(5).SetCellValue(dr["IBMNumber"].ToString());
                    }

                    if (iRow.GetCell(6) != null)
                    {
                        iRow.GetCell(6).SetCellValue(dr["TaxIdNumber"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(6).SetCellValue(dr["TaxIdNumber"].ToString());
                    }

                    if (iRow.GetCell(7) != null)
                    {
                        iRow.GetCell(7).SetCellValue(dr["HICNumber"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(7).SetCellValue(dr["HICNumber"].ToString());
                    }

                    if (iRow.GetCell(8) != null)
                    {
                        iRow.GetCell(8).SetCellValue(dr["InsuranceWorkCompensation"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(8).SetCellValue(dr["InsuranceWorkCompensation"].ToString());
                    }

                    if (iRow.GetCell(9) != null)
                    {
                        iRow.GetCell(9).SetCellValue(dr["InsuranceDisability"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(9).SetCellValue(dr["InsuranceDisability"].ToString());
                    }

                    if (iRow.GetCell(10) != null)
                    {
                        iRow.GetCell(10).SetCellValue(dr["InsuranceGeneralLiability"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(10).SetCellValue(dr["InsuranceDisability"].ToString());
                    }

                    if (iRow.GetCell(11) != null)
                    {
                        iRow.GetCell(11).SetCellValue(dr["insuranceObstructionBond"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(11).SetCellValue(dr["insuranceObstructionBond"].ToString());
                    }
                    if (iRow.GetCell(12) != null)
                    {
                        iRow.GetCell(12).SetCellValue(dr["Notes"].ToString());
                    }
                    else
                    {
                        iRow.CreateCell(12).SetCellValue(dr["Notes"].ToString());
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
                    iRow.GetCell(9).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(10).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(11).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(12).CellStyle = leftAlignCellStyle;
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
        /// Parameter 1 : DeletedDocumentIds (String)
        /// Parameter 2 : IdCompany (String)
        /// Parameter 3 : Documentfiles want to Upload (File)
        /// </summary>
        /// <returns>Task&lt;HttpResponseMessage&gt; update the detail of Company.</returns>
        /// <exception cref="System.Web.Http.HttpResponseException"></exception>
        [Authorize]
        [RpoAuthorize]
        [Route("api/companies/Document")]
        [ResponseType(typeof(ContactDocument))]
        public async Task<HttpResponseMessage> PutCompanydocument()
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddCompany))
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
                System.Collections.Specialized.NameValueCollection formData = provider.FormData;
                IList<HttpContent> files = provider.Files;
                string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
                int idCompany = Convert.ToInt32(formData["idCompany"]);

                string deletedDocumentIds = Convert.ToString(formData["deletedDocumentIds"]);

                if (!string.IsNullOrEmpty(deletedDocumentIds))
                {
                    foreach (var item in deletedDocumentIds.Split(','))
                    {
                        int companyDocumentId = Convert.ToInt32(item);
                        CompanyDocument companyDocument = rpoContext.CompanyDocuments.Where(x => x.Id == companyDocumentId).FirstOrDefault();
                        if (companyDocument != null)
                        {
                            rpoContext.CompanyDocuments.Remove(companyDocument);
                            var path = HttpRuntime.AppDomainAppPath;
                            string directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.ComanyDocumentPath));
                            string directoryDelete = Convert.ToString(companyDocument.Id) + "_" + companyDocument.DocumentPath;
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

                    CompanyDocument companyDocument = new CompanyDocument();
                    companyDocument.DocumentPath = thisFileName;
                    companyDocument.IdCompany = idCompany;
                    companyDocument.Name = thisFileName;
                    rpoContext.CompanyDocuments.Add(companyDocument);
                    rpoContext.SaveChanges();

                    var path = HttpRuntime.AppDomainAppPath;
                    directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.ComanyDocumentPath));

                    string directoryFileName = Convert.ToString(companyDocument.Id) + "_" + thisFileName;
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

                var companyDocumentList = rpoContext.CompanyDocuments
                    .Where(x => x.IdCompany == idCompany).ToList();
                //    .Select(rd => new ContactDocument
                //    {
                //        Id = rd.Id,
                //        Name = rd.Name,
                //        Content = rd.Content,
                //        IdContact = rd.IdContact
                //        DocumentPath = APIUrl + "/" + Convert.ToString(Properties.Settings.Default.ContactDocumentPath) + "/" + rd.DocumentPath
                //    }).ToList();

                var response = Request.CreateResponse<List<CompanyDocument>>(HttpStatusCode.OK, companyDocumentList);
                return response;
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        //mb Responsibility
        [Route("api/companies/Responsibilitydropdown")]
        public IHttpActionResult GetResponsibilityDropdown()
        {
            var result = rpoContext.Responsibilities.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.ResponsibilityName,
                Name = c.ResponsibilityName
            }).ToArray();

            return Ok(result);

        }


    }
}