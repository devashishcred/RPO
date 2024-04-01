// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-16-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-31-2018
// ***********************************************************************
// <copyright file="JobContactsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Contacts Controller.</summary>
// ***********************************************************************


/// <summary>
/// The JobContacts namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobContacts
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using Rpo.ApiServices.Api.Controllers.Contacts;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Model.Models.Enums;
    using System.Net;
    using System.Net.Http;

    /// <summary>
    /// Class Job Contacts Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class JobContactsController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the list of job contacts.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Get the list of active job contact .</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobs/{idJob}/contacts")]
        public IHttpActionResult GetJobContacts([FromUri] DataTableParameters dataTableParameters, [FromUri] int idJob)
        {
            IQueryable<JobContact> jobContacts = rpoContext
                .JobContacts.Include("Company")
                .Include("Contact")
                .Include("JobContactType")
                .Include("Address")
                .Include("JobContactJobContactGroups.JobContactGroup")
                .Where(jc => jc.IdJob == idJob && jc.Contact.IsActive == true);

            var recordsTotal = jobContacts.Count();
            var recordsFiltered = recordsTotal;

            var result = jobContacts.AsEnumerable()
                .Select(jc => Format(jc))
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

        /// <summary>
        /// Gets the list of all job contacts.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Get the list of all job contact.</returns>
        //[HttpGet]
        //[Authorize]
        //[RpoAuthorize]
        //[Route("api/jobs/{idJob}/Allcontacts")]
        //public IHttpActionResult GetJobAllContacts([FromUri] DataTableParameters dataTableParameters, [FromUri] int idJob)
        //{
        //    IQueryable<JobContact> jobContacts = rpoContext
        //        .JobContacts.Include("Company")
        //        .Include("Contact")
        //        .Include("JobContactType")
        //        .Include("Address")
        //        .Include("JobContactJobContactGroups.JobContactGroup")
        //        .Where(jc => jc.IdJob == idJob);

        //    var recordsTotal = jobContacts.Count();
        //    var recordsFiltered = recordsTotal;

        //    var result = jobContacts.AsEnumerable()
        //        .Select(jc => Format(jc))
        //        .AsQueryable()
        //        .DataTableParameters(dataTableParameters, out recordsFiltered)
        //        .ToArray();

        //    return Ok(new DataTableResponse
        //    {
        //        Draw = dataTableParameters.Draw,
        //        RecordsFiltered = recordsFiltered,
        //        RecordsTotal = recordsTotal,
        //        Data = result
        //    });
        //}
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobs/{idJob}/Allcontacts")]
        //public IHttpActionResult GetJobAllContacts([FromUri] DataTableParameters dataTableParameters, [FromUri] int idJob)
        //{
        //    IQueryable<JobContact> jobContacts = rpoContext
        //        .JobContacts.Include("Company")
        //        .Include("Contact")
        //        .Include("JobContactType")
        //        .Include("Address")
        //        .Include("JobContactJobContactGroups.JobContactGroup")
        //        .Where(jc => jc.IdJob == idJob);


        //    var recordsTotal = jobContacts.Count();
        //    var recordsFiltered = recordsTotal;

        //    var result = jobContacts.AsEnumerable()
        //        .Select(jc => Format(jc))
        //        .AsQueryable()
        //        .DataTableParameters(dataTableParameters, out recordsFiltered)
        //        .ToArray();

        //    return Ok(new DataTableResponse
        //    {
        //        Draw = dataTableParameters.Draw,
        //        RecordsFiltered = recordsFiltered,
        //        RecordsTotal = recordsTotal,
        //        Data = result
        //    });
        //}
        public IHttpActionResult GetJobAllContacts([FromUri] DataTableParameters dataTableParameters, [FromUri] int idJob)
        {
            IQueryable<JobContact> jobContacts = rpoContext
                .JobContacts.Include("Company")
                .Include("Contact")
                .Include("JobContactType")
                .Include("Address")
                .Include("JobContactJobContactGroups.JobContactGroup")
            // .Where(jc => jc.IdJob == idJob && jc.IdJobContactType==31);
            .Where(jc => jc.IdJob == idJob && (
            (jc.IsMainCompany && jc.IsBilling && !jc.Contact.IsActive) ||
            (jc.IsMainCompany && !jc.Contact.IsActive) ||
            (jc.IsBilling && jc.IsMainCompany && jc.Contact.IsActive) ||
            (jc.IsMainCompany && jc.Contact.IsActive))
            );

            var recordsTotal = jobContacts.Count();
            var recordsFiltered = recordsTotal;

            var result = jobContacts.AsEnumerable()
                .Select(jc => Format(jc))
                .AsQueryable()
                .DataTableParameters(dataTableParameters, out recordsFiltered)
                .ToArray();

            IQueryable<JobContact> jobContacts1 = rpoContext
               .JobContacts.Include("Company")
               .Include("Contact")
               .Include("JobContactType")
               .Include("Address")
               .Include("JobContactJobContactGroups.JobContactGroup")
              // .Where(jc => jc.IdJob == idJob && jc.IdJobContactType != 31);
              .Where(jc => jc.IdJob == idJob && !(
            (jc.IsMainCompany && jc.IsBilling && !jc.Contact.IsActive) ||
            (jc.IsMainCompany && !jc.Contact.IsActive) ||
            (jc.IsBilling && jc.IsMainCompany && jc.Contact.IsActive) ||
            (jc.IsMainCompany && jc.Contact.IsActive)));


            var result1 = jobContacts1.AsEnumerable()
              .Select(jc => Format(jc))
              .AsQueryable()
              .DataTableParameters(dataTableParameters, out recordsFiltered)
              .ToArray();
            var result2 = result.Concat(result1);

            return Ok(new DataTableResponse
            {
                Draw = dataTableParameters.Draw,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal,
                Data = result2
            });
        }

        /// <summary>
        /// Gets the company list bind on  dropdown.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Get the list of companies list for dropdwon.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/JobContacts/{idJob}/companydropdown")]
        public IHttpActionResult GetCompanyDropdown(int idJob)
        {
            var result = this.rpoContext.JobContacts.Where(x => x.IdJob == idJob && x.IdCompany != null).AsEnumerable().Select(c => new
            {
                Id = c.IdCompany,
                ItemName = c.Company != null ? c.Company.Name : string.Empty,
            }).Distinct().ToArray();

            return this.Ok(result);
        }

        /// <summary>
        /// Gets the company list bind on contact dropdown.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="idCompany">The identifier company.</param>
        /// <returns>Get the list of active contact against the job and company.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/JobContacts/{idJob}/company/{idCompany}/contactdropdown")]
        public IHttpActionResult GetCompanyContactDropdown(int idJob, int idCompany)
        {
            if (idCompany == -1)
            {
                var result = rpoContext.JobContacts
                    .Where(c => c.IdCompany == null && c.IdJob == idJob && c.Contact.IsActive == true).AsEnumerable()
                    .Select(c => new
                    {
                        Id = c.IdContact,
                        ItemName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName : string.Empty,
                        NameWithEmail = c.Contact != null ? (c.Contact.FirstName + (!string.IsNullOrWhiteSpace(c.Contact.LastName) ? " " + c.Contact.LastName + (c.Contact.Suffix != null ? " " + c.Contact.Suffix.Description : string.Empty):string.Empty) + (!string.IsNullOrWhiteSpace(c.Contact.Email) ? " (" + c.Contact.Email + ")" : string.Empty)) : string.Empty,
                        FirstName = c.Contact != null ? c.Contact.FirstName : string.Empty,
                        LastName = c.Contact != null ? c.Contact.LastName : string.Empty,
                        Email = c.Contact != null ? c.Contact.Email : string.Empty
                    }).Distinct()
                    .ToArray();

                return Ok(result);
            }
            else
            {
                Company company = rpoContext.Companies.Find(idCompany);
                if (company == null)
                {
                    return this.NotFound();
                }

                var result = rpoContext.JobContacts
                    .Where(c => c.IdCompany == company.Id && c.IdJob == idJob && c.Contact.IsActive == true).AsEnumerable()
                    .Select(c => new
                    {
                        Id = c.IdContact,
                        ItemName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName : string.Empty,
                        NameWithEmail = c.Contact != null ? (c.Contact.FirstName + (!string.IsNullOrWhiteSpace(c.Contact.LastName) ? " " + c.Contact.LastName  +(c.Contact.Suffix != null ? " " + c.Contact.Suffix.Description : string.Empty): string.Empty) + (!string.IsNullOrWhiteSpace(c.Contact.Email) ? " (" + c.Contact.Email + ")" : string.Empty)) : string.Empty,
                        FirstName = c.Contact != null ? c.Contact.FirstName : string.Empty,
                        LastName = c.Contact != null ? c.Contact.LastName : string.Empty,
                    }).Distinct()
                    .ToArray();

                return Ok(result);
            }
        }
        /// <summary>
        /// Gets the company list bind on contact dropdown.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="idCompany">The identifier company.</param>
        /// <returns>Get the list of all contact against the job and company</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/AllJobContacts/{idJob}/company/{idCompany}/contactdropdown")]
        public IHttpActionResult GetCompanyAllContactDropdown(int idJob, int idCompany)
        {
            if (idCompany == -1)
            {
                var result = rpoContext.JobContacts
                    .Where(c => c.IdCompany == null && c.IdJob == idJob).AsEnumerable()
                    .Select(c => new
                    {
                        Id = c.IdContact,
                        ItemName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName : string.Empty,
                        NameWithEmail = c.Contact != null ? (c.Contact.FirstName + (!string.IsNullOrWhiteSpace(c.Contact.LastName) ? " " + c.Contact.LastName + (c.Contact.Suffix != null ? " " + c.Contact.Suffix.Description : string.Empty) : string.Empty) + (!string.IsNullOrWhiteSpace(c.Contact.Email) ? " (" + c.Contact.Email + ")" : string.Empty)) : string.Empty,
                        FirstName = c.Contact != null ? c.Contact.FirstName : string.Empty,
                        LastName = c.Contact != null ? c.Contact.LastName : string.Empty,
                        Email = c.Contact != null ? c.Contact.Email : string.Empty
                    }).Distinct()
                    .ToArray();

                return Ok(result);
            }
            else
            {
                Company company = rpoContext.Companies.Find(idCompany);
                if (company == null)
                {
                    return this.NotFound();
                }

                var result = rpoContext.JobContacts
                    .Where(c => c.IdCompany == company.Id && c.IdJob == idJob).AsEnumerable()
                    .Select(c => new
                    {
                        Id = c.IdContact,
                        ItemName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName : string.Empty,
                        NameWithEmail = c.Contact != null ? (c.Contact.FirstName + (!string.IsNullOrWhiteSpace(c.Contact.LastName) ? " " + c.Contact.LastName + (c.Contact.Suffix != null ? " " + c.Contact.Suffix.Description : string.Empty) : string.Empty) + (!string.IsNullOrWhiteSpace(c.Contact.Email) ? " (" + c.Contact.Email + ")" : string.Empty)) : string.Empty,
                        FirstName = c.Contact != null ? c.Contact.FirstName : string.Empty,
                        LastName = c.Contact != null ? c.Contact.LastName : string.Empty,
                    }).Distinct()
                    .ToArray();

                return Ok(result);
            }
        }
        //[Authorize]
        //[RpoAuthorize]
        //[HttpGet]
        //[Route("api/JobContacts/{idJob}/company/{idCompany}/ContactEmployeedropdown")]
        //public IHttpActionResult GetCompanyContactEmployeeDropdown(int idJob, int idCompany)
        //{
        //    List<ContactEmployeeDTO> contactEmployeeDTOList = new List<ContactEmployeeDTO>();
        //    //if (idCompany == -1)
        //    //{
        //    contactEmployeeDTOList = rpoContext.JobContactJobContactGroups.Include("JobContact.Contact")
        //        .Where(c => c.JobContact.IdJob == idJob).AsEnumerable()
        //        .Select(c => new ContactEmployeeDTO()
        //        {
        //            IdContact = c.JobContact.IdContact,
        //            Id = "C" + c.JobContact.IdContact,
        //            ItemName = c.JobContact.Contact != null ? c.JobContact.Contact.FirstName + " " + c.JobContact.Contact.LastName + (c.JobContact.Contact.Company != null ? " (" + c.JobContact.Contact.Company.Name + ")" : string.Empty) : string.Empty,
        //            NameWithEmail = c.JobContact.Contact != null ? (c.JobContact.Contact.FirstName + (!string.IsNullOrWhiteSpace(c.JobContact.Contact.LastName) ? " " + c.JobContact.Contact.LastName : string.Empty) + (!string.IsNullOrWhiteSpace(c.JobContact.Contact.Email) ? " (" + c.JobContact.Contact.Email + ")" : string.Empty)) : string.Empty,
        //            FirstName = c.JobContact.Contact != null ? c.JobContact.Contact.FirstName : string.Empty,
        //            LastName = c.JobContact.Contact != null ? c.JobContact.Contact.LastName : string.Empty,
        //            Email = c.JobContact.Contact != null ? c.JobContact.Contact.Email : string.Empty,
        //            IsContact = true,
        //            IdGroup = c.IdJobContactGroup

        //        }).Distinct()
        //        .ToList();

        //    if (contactEmployeeDTOList == null)
        //    {
        //        contactEmployeeDTOList = new List<ContactEmployeeDTO>();
        //    }

        //    contactEmployeeDTOList.AddRange(rpoContext.JobContacts.Include("Contact")
        //        .Where(c => c.IdJob == idJob && c.JobContactJobContactGroups.Count() <= 0).AsEnumerable()
        //        .Select(c => new ContactEmployeeDTO()
        //        {
        //            IdContact = c.IdContact,
        //            Id = "C" + c.IdContact,
        //            ItemName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName + (c.Contact.Company != null ? " (" + c.Contact.Company.Name + ")" : string.Empty) : string.Empty,
        //            NameWithEmail = c.Contact != null ? (c.Contact.FirstName + (!string.IsNullOrWhiteSpace(c.Contact.LastName) ? " " + c.Contact.LastName : string.Empty) + (!string.IsNullOrWhiteSpace(c.Contact.Email) ? " (" + c.Contact.Email + ")" : string.Empty)) : string.Empty,
        //            FirstName = c.Contact != null ? c.Contact.FirstName : string.Empty,
        //            LastName = c.Contact != null ? c.Contact.LastName : string.Empty,
        //            Email = c.Contact != null ? c.Contact.Email : string.Empty,
        //            IsContact = true,
        //            IdGroup = null

        //        }).Distinct()
        //        .ToList());

        //    //}
        //    //else
        //    //{
        //    //    Company company = rpoContext.Companies.Find(idCompany);
        //    //    if (company == null)
        //    //    {
        //    //        return this.NotFound();
        //    //    }

        //    //    contactEmployeeDTOList = rpoContext.JobContacts
        //    //        .Where(c => c.IdCompany == company.Id && c.IdJob == idJob).AsEnumerable()
        //    //        .Select(c => new ContactEmployeeDTO()
        //    //        {
        //    //            IdContact = c.IdContact,
        //    //            Id = "C" + c.IdContact,
        //    //            ItemName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName + (c.Contact.Company != null ? " (" + c.Contact.Company.Name + ")" : string.Empty) : string.Empty,
        //    //            NameWithEmail = c.Contact != null ? (c.Contact.FirstName + (!string.IsNullOrWhiteSpace(c.Contact.LastName) ? " " + c.Contact.LastName : string.Empty) + (!string.IsNullOrWhiteSpace(c.Contact.Email) ? " (" + c.Contact.Email + ")" : string.Empty)) : string.Empty,
        //    //            FirstName = c.Contact != null ? c.Contact.FirstName : string.Empty,
        //    //            LastName = c.Contact != null ? c.Contact.LastName : string.Empty,
        //    //            Email = c.Contact != null ? c.Contact.Email : string.Empty,
        //    //            IsContact = true
        //    //        }).Distinct()
        //    //        .ToList();
        //    //}

        //    Job job = this.rpoContext.Jobs.FirstOrDefault(x => x.Id == idJob);

        //    List<int> dobProjectTeam = job != null && job.DOBProjectTeam != null && !string.IsNullOrEmpty(job.DOBProjectTeam) ? (job.DOBProjectTeam.Split(',') != null && job.DOBProjectTeam.Split(',').Any() ? job.DOBProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
        //    List<int> dotProjectTeam = job != null && job.DOTProjectTeam != null && !string.IsNullOrEmpty(job.DOTProjectTeam) ? (job.DOTProjectTeam.Split(',') != null && job.DOTProjectTeam.Split(',').Any() ? job.DOTProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
        //    List<int> violationProjectTeam = job != null && job.ViolationProjectTeam != null && !string.IsNullOrEmpty(job.ViolationProjectTeam) ? (job.ViolationProjectTeam.Split(',') != null && job.ViolationProjectTeam.Split(',').Any() ? job.ViolationProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
        //    List<int> depProjectTeam = job != null && job.DEPProjectTeam != null && !string.IsNullOrEmpty(job.DEPProjectTeam) ? (job.DEPProjectTeam.Split(',') != null && job.DEPProjectTeam.Split(',').Any() ? job.DEPProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

        //    List<ContactEmployeeDTO> employeeList = rpoContext.Employees.Where(x => dotProjectTeam.Contains(x.Id)
        //                                                || violationProjectTeam.Contains(x.Id)
        //                                                || depProjectTeam.Contains(x.Id)
        //                                                || dobProjectTeam.Contains(x.Id)
        //                                                || x.Id == job.IdProjectManager
        //                                                ).AsEnumerable().Select(c => new ContactEmployeeDTO()
        //                                                {
        //                                                    IdContact = c.Id,
        //                                                    Id = "E" + c.Id,
        //                                                    ItemName = c != null ? c.FirstName + " " + c.LastName + " (RPO INC)" : string.Empty,
        //                                                    NameWithEmail = c != null ? (c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty) + (!string.IsNullOrWhiteSpace(c.Email) ? " (" + c.Email + ")" : string.Empty)) : string.Empty,
        //                                                    FirstName = c != null ? c.FirstName : string.Empty,
        //                                                    LastName = c != null ? c.LastName : string.Empty,
        //                                                    Email = c != null ? c.Email : string.Empty,
        //                                                    IsContact = false,
        //                                                    IdGroup = -1

        //                                                }).ToList();

        //    if (contactEmployeeDTOList == null)
        //    {
        //        contactEmployeeDTOList = new List<ContactEmployeeDTO>();
        //    }

        //    if (employeeList != null && employeeList.Count > 0)
        //    {
        //        contactEmployeeDTOList.AddRange(employeeList);
        //    }

        //    return Ok(contactEmployeeDTOList);
        //}

        /// <summary>
        /// Gets the contact list bind on contact employeed dropdown.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="idCompany">The identifier company.</param>
        /// <returns>Get the list of employees list.</returns>

        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/JobContacts/{idJob}/company/{idCompany}/ContactEmployeedropdown")]
        public IHttpActionResult GetCompanyContactEmployeeDropdown(int idJob, int idCompany)
        {
            List<ContactEmployeeGroupDTO> contactEmployeeGroupDTOList = new List<ContactEmployeeGroupDTO>();
            contactEmployeeGroupDTOList = rpoContext.JobContactGroups
                               .Where(x => x.IdJob == idJob).AsEnumerable()
                               .Select(x => new ContactEmployeeGroupDTO()
                               {
                                   Value = x.Id.ToString(),
                                   Text = x.Name,
                                   Children = x.JobContactJobContactGroups.Where(c => c.JobContact.Contact.IsActive == true).Select(c => new ContactEmployeeDTO()
                                   {
                                       Value = x.Id + "_C_" + c.JobContact.IdContact,
                                       Text = c.JobContact.Contact != null ? c.JobContact.Contact.FirstName + " " + c.JobContact.Contact.LastName + (c.JobContact.Contact.Company != null ? " (" + c.JobContact.Contact.Company.Name + (c.JobContact.Contact != null && !string.IsNullOrEmpty(c.JobContact.Contact.Email) ? "-" + c.JobContact.Contact.Email : string.Empty) + ")" : string.Empty) : string.Empty,
                                       Checked = false,
                                       //IsContact = true
                                   }).Distinct().ToList()
                               })
                               .ToList();



            var contactList = rpoContext.JobContacts
                .Where(c => c.IdJob == idJob && c.JobContactJobContactGroups.Count() <= 0 && c.Contact.IsActive == true).AsEnumerable()
                .Select(c => new ContactEmployeeGroupDTO()
                {
                    Value = "0_C_" + c.IdContact,
                    Text = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName + (c.Contact.Suffix != null ? " " + c.Contact.Suffix.Description : string.Empty) + (c.Contact.Company != null ? " (" + c.Contact.Company.Name + (c.Contact != null && !string.IsNullOrEmpty(c.Contact.Email) ? "-" + c.Contact.Email : string.Empty) + ")" : string.Empty) : string.Empty,
                    Checked = false,
                    //IsContact = tru
                }).Distinct()
                .ToList();



            Job job = this.rpoContext.Jobs.FirstOrDefault(x => x.Id == idJob);

            List<int> dobProjectTeam = job != null && job.DOBProjectTeam != null && !string.IsNullOrEmpty(job.DOBProjectTeam) ? (job.DOBProjectTeam.Split(',') != null && job.DOBProjectTeam.Split(',').Any() ? job.DOBProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> dotProjectTeam = job != null && job.DOTProjectTeam != null && !string.IsNullOrEmpty(job.DOTProjectTeam) ? (job.DOTProjectTeam.Split(',') != null && job.DOTProjectTeam.Split(',').Any() ? job.DOTProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> violationProjectTeam = job != null && job.ViolationProjectTeam != null && !string.IsNullOrEmpty(job.ViolationProjectTeam) ? (job.ViolationProjectTeam.Split(',') != null && job.ViolationProjectTeam.Split(',').Any() ? job.ViolationProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> depProjectTeam = job != null && job.DEPProjectTeam != null && !string.IsNullOrEmpty(job.DEPProjectTeam) ? (job.DEPProjectTeam.Split(',') != null && job.DEPProjectTeam.Split(',').Any() ? job.DEPProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

            //List<ContactEmployeeDTO> employeeList2 = rpoContext.Employees.Where(x => dotProjectTeam.Contains(x.Id)
            //                                            || violationProjectTeam.Contains(x.Id)
            //                                            || depProjectTeam.Contains(x.Id)
            //                                            || dobProjectTeam.Contains(x.Id)
            //                                            || x.Id == job.IdProjectManager
            //                                            ).AsEnumerable().Select(c => new ContactEmployeeDTO()
            //                                            {
            //                                                Value = "-1_E_" + c.Id,
            //                                                Text = c != null ? c.FirstName + " " + c.LastName + " (RPO INC " + (!string.IsNullOrEmpty(c.Email) ? "-" + c.Email : string.Empty) + ")" : string.Empty,
            //                                                Checked = false,
            //                                                //IsContact = false
            //                                            }).ToList();

            List<ContactEmployeeDTO> employeeList = rpoContext.Employees.Where(x =>x.IsActive && x.Email!= "admin@rpoinc.com" && x.Email != "donanza.dodia@credencys.com" && x.Email != "manish@credencys.com" && x.Email != "meethalal.teli@credencys.com"
                                                     ).AsEnumerable().Select(c => new ContactEmployeeDTO()
                                                     {
                                                         Value = "-1_E_" + c.Id,
                                                         Text = c != null ? c.FirstName + " " + c.LastName + " (RPO INC " + (!string.IsNullOrEmpty(c.Email) ? "-" + c.Email : string.Empty) + ")" : string.Empty,
                                                         Checked = false,
                                                            //IsContact = false
                                                        }).ToList();

            if (contactEmployeeGroupDTOList == null)
            {
                contactEmployeeGroupDTOList = new List<ContactEmployeeGroupDTO>();
            }

            if (contactList != null && contactList.Count > 0)
            {
                //contactEmployeeGroupDTOList.Add(new ContactEmployeeGroupDTO
                //{
                //    Value = "0",
                //    Text = "Additional Contact",
                //    Children = contactList

                //});
                contactEmployeeGroupDTOList.AddRange(contactList);
            }

            if (employeeList != null && employeeList.Count > 0)
            {
                contactEmployeeGroupDTOList.Add(new ContactEmployeeGroupDTO
                {
                    Value = "-1",
                    Text = "RPO INC",
                    Children = employeeList

                });
            }

            return Ok(contactEmployeeGroupDTOList);
        }
        /// <summary>
        /// Gets the contact list bind on contact employeed dropdown.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="idCompany">The identifier company.</param>
        /// <returns>Get the list of contact against the job and company.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/AllJobContacts/{idJob}/company/{idCompany}/ContactEmployeedropdown")]
        public IHttpActionResult GetCompanyAllContactEmployeeDropdown(int idJob, int idCompany)
        {
            List<ContactEmployeeGroupDTO> contactEmployeeGroupDTOList = new List<ContactEmployeeGroupDTO>();
            contactEmployeeGroupDTOList = rpoContext.JobContactGroups
                               .Where(x => x.IdJob == idJob).AsEnumerable()
                               .Select(x => new ContactEmployeeGroupDTO()
                               {
                                   Value = x.Id.ToString(),
                                   Text = x.Name,
                                   Children = x.JobContactJobContactGroups.Select(c => new ContactEmployeeDTO()
                                   {
                                       Value = x.Id + "_C_" + c.JobContact.IdContact,
                                       Text = c.JobContact.Contact != null ? c.JobContact.Contact.FirstName + " " + c.JobContact.Contact.LastName + (c.JobContact.Contact.Company != null ? " (" + c.JobContact.Contact.Company.Name + (c.JobContact.Contact != null && !string.IsNullOrEmpty(c.JobContact.Contact.Email) ? "-" + c.JobContact.Contact.Email : string.Empty) + ")" : string.Empty) : string.Empty,
                                       Checked = false,
                                       //IsContact = true
                                   }).Distinct().ToList()
                               })
                               .ToList();



            var contactList = rpoContext.JobContacts
                .Where(c => c.IdJob == idJob && c.JobContactJobContactGroups.Count() <= 0).AsEnumerable()
                .Select(c => new ContactEmployeeGroupDTO()
                {
                    Value = "0_C_" + c.IdContact,
                    Text = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName + (c.Contact.Company != null ? " (" + c.Contact.Company.Name + (c.Contact != null && !string.IsNullOrEmpty(c.Contact.Email) ? "-" + c.Contact.Email : string.Empty) + ")" : string.Empty) : string.Empty,
                    Checked = false,
                    //IsContact = true
                }).Distinct()
                .ToList();



            Job job = this.rpoContext.Jobs.FirstOrDefault(x => x.Id == idJob);

            List<int> dobProjectTeam = job != null && job.DOBProjectTeam != null && !string.IsNullOrEmpty(job.DOBProjectTeam) ? (job.DOBProjectTeam.Split(',') != null && job.DOBProjectTeam.Split(',').Any() ? job.DOBProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> dotProjectTeam = job != null && job.DOTProjectTeam != null && !string.IsNullOrEmpty(job.DOTProjectTeam) ? (job.DOTProjectTeam.Split(',') != null && job.DOTProjectTeam.Split(',').Any() ? job.DOTProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> violationProjectTeam = job != null && job.ViolationProjectTeam != null && !string.IsNullOrEmpty(job.ViolationProjectTeam) ? (job.ViolationProjectTeam.Split(',') != null && job.ViolationProjectTeam.Split(',').Any() ? job.ViolationProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> depProjectTeam = job != null && job.DEPProjectTeam != null && !string.IsNullOrEmpty(job.DEPProjectTeam) ? (job.DEPProjectTeam.Split(',') != null && job.DEPProjectTeam.Split(',').Any() ? job.DEPProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

            //List<ContactEmployeeDTO> employeeList = rpoContext.Employees.Where(x => dotProjectTeam.Contains(x.Id)
            //                                            || violationProjectTeam.Contains(x.Id)
            //                                            || depProjectTeam.Contains(x.Id)
            //                                            || dobProjectTeam.Contains(x.Id)
            //                                            || x.Id == job.IdProjectManager
            //                                            ).AsEnumerable().Select(c => new ContactEmployeeDTO()
            //                                            {
            //                                                Value = "-1_E_" + c.Id,
            //                                                Text = c != null ? c.FirstName + " " + c.LastName + " (RPO INC " + (!string.IsNullOrEmpty(c.Email) ? "-" + c.Email : string.Empty) + ")" : string.Empty,
            //                                                Checked = false,
            //                                                //IsContact = false
            //                                            }).ToList();

            List<ContactEmployeeDTO> employeeList = rpoContext.Employees.Where(x => x.IsActive && x.Email != "admin@rpoinc.com" && x.Email != "donanza.dodia@credencys.com" && x.Email != "manish@credencys.com" && x.Email != "meethalal.teli@credencys.com"
                                                  ).AsEnumerable().Select(c => new ContactEmployeeDTO()
                                                  {
                                                      Value = "-1_E_" + c.Id,
                                                      Text = c != null ? c.FirstName + " " + c.LastName + " (RPO INC " + (!string.IsNullOrEmpty(c.Email) ? "-" + c.Email : string.Empty) + ")" : string.Empty,
                                                      Checked = false,
                                                         //IsContact = false
                                                     }).ToList();

            if (contactEmployeeGroupDTOList == null)
            {
                contactEmployeeGroupDTOList = new List<ContactEmployeeGroupDTO>();
            }

            if (contactList != null && contactList.Count > 0)
            {
                //contactEmployeeGroupDTOList.Add(new ContactEmployeeGroupDTO
                //{
                //    Value = "0",
                //    Text = "Additional Contact",
                //    Children = contactList

                //});
                contactEmployeeGroupDTOList.AddRange(contactList);
            }

            if (employeeList != null && employeeList.Count > 0)
            {
                contactEmployeeGroupDTOList.Add(new ContactEmployeeGroupDTO
                {
                    Value = "-1",
                    Text = "RPO INC",
                    Children = employeeList

                });
            }

            return Ok(contactEmployeeGroupDTOList);
        }


        /// <summary>
        /// Gets the job contact.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="idJobContact">The identifier job contact.</param>
        /// <returns>Get the list of contact against the job and jobaontact.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobs/{idJob}/contacts/{idJobContact}")]
        public IHttpActionResult GetJobContact(int idJob, int idJobContact)
        {
            Job job = rpoContext.Jobs.Find(idJob);
            if (job == null)
            {
                return this.NotFound();
            }

            JobContact jobContact = rpoContext.JobContacts.Find(idJobContact);
            if (jobContact == null)
            {
                return this.NotFound();
            }

            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            var result = rpoContext
                .JobContacts
                .Include("JobContactType")
                .Include("Address")
                .Include("Company")                
                .Where(jc => jc.Id == idJobContact)
                .AsEnumerable()
                .Select(jc => FormatDetails(jc))
                .FirstOrDefault();
            return Ok(result);
        }

        /// <summary>
        /// Posts the job contact.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="jobContact">The job contact.</param>
        /// <returns>create a bew job contacts.</returns>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobs/{idJob}/contacts/")]
        [ResponseType(typeof(JobContact))]
        public IHttpActionResult PostJobContact(int idJob, JobContactCreateDTO jobContact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int customerid = 0;
            Job job = rpoContext.Jobs.Find(idJob);
            if (job == null)
            {
                return this.NotFound();
            }

            if (jobContact.IdCompany != null && jobContact.IdCompany > 0)
            {
                jobContact.IdCompany = jobContact.IdCompany;
            }
            else
            {
                jobContact.IdCompany = null;
            }

            List<JobContact> objContacts = rpoContext.JobContacts.Where(d => d.IdJob == idJob && d.IdCompany == jobContact.IdCompany && d.IdContact == jobContact.IdContact).ToList();
            if (objContacts != null && objContacts.Count > 0)
            {
                throw new RpoBusinessException("Job contact already exists");
                //return new HttpActionResult(HttpStatusCode.InternalServerError, "Job contact is exists");
            }


            JobContact newJobContact = new JobContact
            {
                IdCompany = jobContact.IdCompany,
                IdContact = jobContact.IdContact,
                IdJob = idJob,
                IdAddress = jobContact.IdAddress,
                IsBilling = jobContact.IsBilling,
                IsMainCompany = jobContact.IsMainCompany,
                IdJobContactType = jobContact.IdJobContactType
            };

            if (jobContact.IsBilling)
            {
                var billingClientList = rpoContext.JobContacts.Where(x => x.IdJob == idJob);
                foreach (JobContact item in billingClientList)
                {
                    item.IsBilling = false;
                }
            }

            if (jobContact.IsMainCompany)
            {
                var mainClientList = rpoContext.JobContacts.Where(x => x.IdJob == idJob);
                foreach (JobContact item in mainClientList)
                {
                    item.IsMainCompany = false;
                }
            }



            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            newJobContact.LastModifiedDate = DateTime.UtcNow;
            newJobContact.CreatedDate = DateTime.UtcNow;
            job.LastModiefiedDate = DateTime.UtcNow;
            if (employee != null)
            {
                newJobContact.CreatedBy = employee.Id;
                job.LastModifiedBy = employee.Id;
            }

            rpoContext.JobContacts.Add(newJobContact);
            rpoContext.SaveChanges();

            if (jobContact.IsMainCompany == true)
            {
                Job objjob = (from d in rpoContext.Jobs where d.Id == idJob select d).FirstOrDefault();

                if (objjob != null)
                {
                    if (jobContact.IdCompany != null && jobContact.IdCompany > 0)
                    {
                        objjob.IdCompany = jobContact.IdCompany;
                    }
                    objjob.IdContact = jobContact.IdContact.Value;
                    objjob.IdJobContactType = jobContact.IdJobContactType;
                }
                rpoContext.SaveChanges();
            }

            if (jobContact.JobContactJobContactGroups != null && jobContact.JobContactJobContactGroups.Count > 0)
            {
                foreach (var item in jobContact.JobContactJobContactGroups)
                {
                    JobContactJobContactGroup jobContactJobContactGroup = new JobContactJobContactGroup();
                    jobContactJobContactGroup.IdJobContact = newJobContact.Id;
                    jobContactJobContactGroup.IdJobContactGroup = item.IdJobContactGroup;
                    rpoContext.JobContactJobContactGroups.Add(jobContactJobContactGroup);
                }
                rpoContext.SaveChanges();
            }
            string clientname = string.Empty;
            if (newJobContact.IsMainCompany == true && newJobContact.IsBilling == true)
            {
                clientname = "Contact is set at Main Client & Billing Client.";
            }
            else if (newJobContact.IsMainCompany == true)
            {
                clientname = "Contact is set at Main Client.";
            }
            else if (newJobContact.IsBilling == true)
            {
                clientname = "Contact is set at Billing Client.";
            }
            //else
            //{
            //    clientname = "Not Set";
            //}          
            
          string ContactEmail=  rpoContext.Contacts.Where(x => x.Id == jobContact.IdContact).FirstOrDefault().Email;
            var customer = rpoContext.Customers.Where(x => x.EmailAddress == ContactEmail).FirstOrDefault();
         
            #region projectaccess new
            bool MailSendflag = false;
            try
            {
                if (jobContact.hasJobAccess == true)
                {
                    if (customer != null)
                    {
                        var CustomerJobAccess = rpoContext.CustomerJobAccess.Where(x => x.IdCustomer == customer.Id && x.IdJob == jobContact.IdJob).FirstOrDefault();
                        if (CustomerJobAccess == null)
                        {
                            CustomerJobAccess customerJobAccess = new CustomerJobAccess();
                            customerJobAccess.IdJob = jobContact.IdJob;
                            customerJobAccess.IdCustomer = customer.Id;
                            customerJobAccess.CUI_Status = 2;
                            customerJobAccess.CreatedDate = DateTime.UtcNow;
                            customerJobAccess.CreatedBy = employee.Id;
                            rpoContext.CustomerJobAccess.Add(customerJobAccess);
                            rpoContext.SaveChanges();
                        }
                    }
                    else
                    {
                        var From = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
                        CustomerInvitationStatus CustomerInvitation = rpoContext.CustomerInvitationStatus.Where(x => x.EmailAddress == ContactEmail).FirstOrDefault();
                        if (CustomerInvitation == null)
                        {
                            CustomerInvitationStatus CustomerInvitationStatus = new CustomerInvitationStatus();
                            CustomerInvitationStatus.IdContact = (int)jobContact.IdContact;
                            CustomerInvitationStatus.EmailAddress = ContactEmail;
                            if (idJob == 0)
                                CustomerInvitationStatus.IdJob = 0;
                            else
                                CustomerInvitationStatus.IdJob = idJob;
                            CustomerInvitationStatus.CUI_Invitatuionstatus = 1;
                            CustomerInvitationStatus.InvitationSentCount = 1;
                            if (From != null)
                            {
                                CustomerInvitationStatus.CreatedBy = From.Id;
                            }
                            CustomerInvitationStatus.CreatedDate = DateTime.UtcNow;
                            if (!string.IsNullOrWhiteSpace(ContactEmail))                              
                            {
                                rpoContext.CustomerInvitationStatus.Add(CustomerInvitationStatus);
                                rpoContext.SaveChanges();
                                MailSendflag = true;
                            }
                        }
                        else
                        {
                            string email = rpoContext.Contacts.Where(x => x.Id == jobContact.IdContact).FirstOrDefault().Email;
                          
                            if(!string.IsNullOrWhiteSpace(email))
                            {
                               // CustomerInvitation.InvitationSentCount += 1;
                                CustomerInvitation.CreatedDate = DateTime.Now;
                                CustomerInvitation.IdJob = idJob;
                                rpoContext.Entry(CustomerInvitation).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                                MailSendflag = true;
                            }

                        }
                        var contact = rpoContext.Contacts.Where(x => x.Id == newJobContact.IdContact).FirstOrDefault();
                        if (contact != null)
                        {
                            if (!string.IsNullOrEmpty(contact.Email))
                            {
                                if (rpoContext.Employees.Any(x => x.Email == contact.Email))
                                {
                                    throw new RpoBusinessException("Can not Send Invitation. This Email Id is already registered as Snapcor User.");
                                }
                                if (MailSendflag == true)
                                {
                                    var to = new List<KeyValuePair<string, string>>();
                                    var cc = new List<KeyValuePair<string, string>>();
                                    //real
                                    to.Add(new KeyValuePair<string, string>(contact.Email, contact.FirstName + contact.LastName));
                                   
                                    string body = string.Empty;
                                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/invitationMailer.html")))
                                    {
                                        body = reader.ReadToEnd();
                                    }
                                    string Subject = "SnapCor Customer Portal Invitation";

                                    string link = "<a href=\"" + Properties.Settings.Default.FrontEndUrl + "customer-signup?email=" + contact.Email + "&IdContact=" + contact.Id + "\">" + "Sign Up" + "</a > ";
                                    //string name = contact.FirstName + " " + contact.LastName;
                                    string emailBody = body;
                                    emailBody = emailBody.Replace("##InvitationLink##", link);
                                    emailBody = emailBody.Replace("##Name##", contact.FirstName);
                                    try
                                    {
                                        Mail.Send(
                                             new KeyValuePair<string, string>(From.Email, From.FirstName + " " + From.LastName),
                                            to,
                                            cc,
                                            Subject,
                                            emailBody,
                                            true
                                        );
                                        if (customer != null)
                                        {
                                            customerid = customer.Id;
                                        }
                                        return Ok(new
                                        {
                                            idcustomer = customerid,
                                            message = "This Contact Is Not Registerd On SnapCor! Invitation Mail Sent"
                                        });
                                    }
                                    catch
                                      {
                                       var removelastaddedinvitation= rpoContext.CustomerInvitationStatus.Where(x => x.IdContact == (int)jobContact.IdContact).FirstOrDefault(); ;
                                        rpoContext.CustomerInvitationStatus.Remove(removelastaddedinvitation);
                                        throw new RpoBusinessException(StaticMessages.MailSendingFailed);
                                    }
                                }
                                else
                                {
                                    return Ok(new
                                    {
                                        idcustomer = customerid,
                                        message = "Project contact added successful. This contact does not have email id, so cannot send invitation email and project access email"
                                    });
                                    //  throw new RpoBusinessException("Can Not Send Invitation Email. Mail Id Is Not Available");
                                }
                                // }

                                //rpoContext.SaveChanges();
                                /// return Ok("Mail sent successfully");
                                //if (customer != null)
                                //{
                                //    customerid = customer.Id;
                                //}
                                //return Ok(new
                                //{
                                //    idcustomer = customerid,
                                //    message = "This Contact Is Not Registerd On SnapCor! Invitation Mail Sent"
                                //});
                                // return Ok("This Customer is not Registerd on CUI Portal ! Invitation mail Sent");
                            }
                            else
                            {
                                return Ok(new
                                {
                                    idcustomer = customerid,
                                    message = "Project Contact Added Successfully ! Can Not Send Invitation Email. Mail Id Is Not Available"
                                }); 
                               // throw new RpoBusinessException("Can Not Send Invitation Email. Mail Id Is Not Available");
                            }
                        }
                        else
                        {
                            return this.NotFound();
                        }
                    }
                }
                else if (jobContact.hasJobAccess == false)
                {
                    if (customer != null)
                    {
                        var CustomerJobAccessDB = rpoContext.CustomerJobAccess.Where(x => x.IdCustomer == customer.Id).FirstOrDefault();
                        if (CustomerJobAccessDB != null)
                        {
                            rpoContext.CustomerJobAccess.Remove(CustomerJobAccessDB);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw new RpoBusinessException(ex.Message);
            }
            #endregion

            newJobContact = rpoContext.JobContacts.Include("Contact").Include("Job").Include("JobContactType").Include("Address").FirstOrDefault(x => x.Id == newJobContact.Id);
            string addContact = JobHistoryMessages.AddContact
                .Replace("##JobContactName##", newJobContact != null && newJobContact.Contact != null ? newJobContact.Contact.FirstName + " " + newJobContact.Contact.LastName : JobHistoryMessages.NoSetstring)
                .Replace("##MainClientBillingClient##", !string.IsNullOrEmpty(clientname) ? clientname : string.Empty)
                .Replace("##ContactType##", newJobContact != null && newJobContact.JobContactType != null ? newJobContact.JobContactType.Name : JobHistoryMessages.NoSetstring)
                .Replace("##MailingAddress##", newJobContact != null && newJobContact.Address != null ? (!string.IsNullOrEmpty(newJobContact.Address.Address1) ? newJobContact.Address.Address1 + "," : string.Empty) + " " + (!string.IsNullOrEmpty(newJobContact.Address.Address2) ? newJobContact.Address.Address2 + "," : string.Empty) + " " + (!string.IsNullOrEmpty(newJobContact.Address.City) ? newJobContact.Address.City + "," : string.Empty) + " " + (!string.IsNullOrEmpty(newJobContact.Address.ZipCode) ? newJobContact.Address.ZipCode : string.Empty) : JobHistoryMessages.NoSetstring);

            Common.SaveJobHistory(employee.Id, newJobContact.IdJob, addContact, JobHistoryType.Contacts);
            // return Ok("Job contact created successfully!");
           
            if(customer!=null)
            {
                customerid = customer.Id;
            }

            return Ok(new
            {
                idcustomer = customerid,
                message = "Project Contact Created Successfully!"
            }); 
        }

        /// <summary>
        /// Puts the job contact.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="idJobContact">The identifier job contact.</param>
        /// <param name="jobContact">The job contact.</param>
        /// <returns>update the detail of jobcontacts .</returns>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobs/{idJob}/contacts/{idJobContact}")]
        public IHttpActionResult PutJobContact(int idJob, int idJobContact, JobContactCreateDTO jobContact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idJobContact != jobContact.Id)
            {
                return BadRequest();
            }

            JobContact oldJobContact = rpoContext.JobContacts.Include("Address").Include("Job").Include("JobContactType").FirstOrDefault(x => x.Id == idJobContact);
            if (oldJobContact == null)
            {
                return this.NotFound();
            }
            bool checkmainclient = oldJobContact.IsMainCompany;

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            oldJobContact.IdJob = jobContact.IdJob;
            oldJobContact.IdContact = jobContact.IdContact;
            if (jobContact.IdCompany != null && jobContact.IdCompany > 0)
            {
                oldJobContact.IdCompany = jobContact.IdCompany;
            }
            else
            {
                oldJobContact.IdCompany = null;
            }

            oldJobContact.IdJobContactType = jobContact.IdJobContactType;
            oldJobContact.IdAddress = jobContact.IdAddress;

            if (jobContact.IsBilling)
            {
                var billingClientList = rpoContext.JobContacts.Where(x => x.IdJob == oldJobContact.IdJob && x.Id != idJobContact);
                foreach (JobContact item in billingClientList)
                {
                    item.IsBilling = false;
                }
            }

            if (jobContact.IsMainCompany)
            {
                var mainClientList = rpoContext.JobContacts.Where(x => x.IdJob == oldJobContact.IdJob && x.Id != idJobContact);
                foreach (JobContact item in mainClientList)
                {
                    item.IsMainCompany = false;
                }
            }

            oldJobContact.IsBilling = jobContact.IsBilling;
            oldJobContact.IsMainCompany = jobContact.IsMainCompany;

            if (jobContact.IsMainCompany == true)
            {
                Job objjob = (from d in rpoContext.Jobs where d.Id == idJob select d).FirstOrDefault();

                if (objjob != null)
                {
                    if (jobContact.IdCompany != null && jobContact.IdCompany > 0)
                    {
                        objjob.IdCompany = jobContact.IdCompany;
                    }

                    objjob.IdContact = jobContact.IdContact.Value;
                    objjob.IdJobContactType = jobContact.IdJobContactType;
                }
                rpoContext.SaveChanges();
            }
            else
            {
                if (checkmainclient == true && jobContact.IsMainCompany == false)
                {
                    Job objjob = (from d in rpoContext.Jobs where d.Id == idJob select d).FirstOrDefault();

                    if (objjob != null)
                    {
                        objjob.IdCompany = null;
                        objjob.IdContact = null;
                        objjob.IdJobContactType = null;
                    }
                    rpoContext.SaveChanges();
                }
            }

            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            oldJobContact.LastModifiedDate = DateTime.UtcNow;
            oldJobContact.Job.LastModiefiedDate = DateTime.UtcNow;
            if (employee != null)
            {
                oldJobContact.LastModifiedBy = employee.Id;
                oldJobContact.Job.LastModifiedBy = employee.Id;
            }

            string clientname = string.Empty;
            if (jobContact.IsMainCompany == true && jobContact.IsBilling == true)
            {
                clientname = "Contact is set at Main Client & Billing Client.";
            }
            else if (jobContact.IsMainCompany == true)
            {
                clientname = "Contact is set at Main Client.";
            }
            else if (jobContact.IsBilling == true)
            {
                clientname = "Contact is set at Billing Client.";
            }
            //else
            //{
            //    clientname = "Not Set";
            //}



            var jobContactJobContactGroupList = oldJobContact.JobContactJobContactGroups.Select(x => x).ToList();
            if (jobContactJobContactGroupList != null)
            {
                foreach (JobContactJobContactGroup item in jobContactJobContactGroupList)
                {
                    if (jobContact.JobContactJobContactGroups != null && jobContact.JobContactJobContactGroups.Count() > 0)
                    {
                        var jobContactJobContactGroup = jobContact.JobContactJobContactGroups.FirstOrDefault(x => x.Id == item.Id);
                        if (jobContactJobContactGroup == null)
                        {
                            rpoContext.JobContactJobContactGroups.Remove(item);
                        }
                    }
                    else
                    {
                        rpoContext.JobContactJobContactGroups.Remove(item);
                    }
                }
            }
            int customerid=0;
            #region projectaccess
            bool MailSendflag = false;
            try
            {
                if (jobContact.hasJobAccess == true)
                {
                    string ContactEmail = rpoContext.Contacts.Where(x => x.Id == jobContact.IdContact).FirstOrDefault().Email;
                    //var customer = rpoContext.Customers.Where(x => x.IdContcat == jobContact.IdContact).FirstOrDefault();
                    var customer = rpoContext.Customers.Where(x => x.EmailAddress == ContactEmail).FirstOrDefault();
                    if (customer != null)
                    {
                        var CustomerJobAccess = rpoContext.CustomerJobAccess.Where(x => x.IdCustomer == customer.Id && x.IdJob == jobContact.IdJob).FirstOrDefault();
                        if (CustomerJobAccess == null)
                        {
                            CustomerJobAccess customerJobAccess = new CustomerJobAccess();
                            customerJobAccess.IdJob = jobContact.IdJob;
                            customerJobAccess.IdCustomer = customer.Id;
                            customerJobAccess.CUI_Status = 2;
                            customerJobAccess.CreatedDate = DateTime.UtcNow;
                            customerJobAccess.CreatedBy = employee.Id;
                            rpoContext.CustomerJobAccess.Add(customerJobAccess);
                        }
                    }
                    else
                    {
                        var From = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
                        CustomerInvitationStatus CustomerInvitation = rpoContext.CustomerInvitationStatus.Where(x => x.EmailAddress == ContactEmail).FirstOrDefault();
                        if (CustomerInvitation == null)
                        {
                            CustomerInvitationStatus CustomerInvitationStatus = new CustomerInvitationStatus();
                            CustomerInvitationStatus.IdContact = (int)jobContact.IdContact;
                            if (idJob == 0)
                                CustomerInvitationStatus.IdJob = 0;
                            else
                                CustomerInvitationStatus.IdJob = idJob;
                            CustomerInvitationStatus.CUI_Invitatuionstatus = 1;
                            CustomerInvitationStatus.InvitationSentCount = 1;
                            CustomerInvitationStatus.EmailAddress = ContactEmail;
                            if (From != null)
                            {
                                CustomerInvitationStatus.CreatedBy = From.Id;
                            }
                            CustomerInvitationStatus.CreatedDate = DateTime.UtcNow;
                            if (!string.IsNullOrWhiteSpace(ContactEmail))
                            {
                                rpoContext.CustomerInvitationStatus.Add(CustomerInvitationStatus);
                                rpoContext.SaveChanges();
                                MailSendflag = true;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(CustomerInvitation.EmailAddress))
                            {
                                CustomerInvitation.InvitationSentCount += 1;
                                rpoContext.Entry(CustomerInvitation).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                                MailSendflag = true;
                            }
                        }
                        var contact = rpoContext.Contacts.Where(x => x.Id == oldJobContact.IdContact).FirstOrDefault();

                        if (contact != null)
                        {
                            if (!string.IsNullOrEmpty(contact.Email))
                            {
                                if (rpoContext.Employees.Any(x => x.Email == contact.Email))
                                {
                                    throw new RpoBusinessException("Can not Send Invitation. This Email Id Is Already Registered as SnapCor User.");
                                }
                                if (MailSendflag == true)
                                {
                                    var to = new List<KeyValuePair<string, string>>();
                                    var cc = new List<KeyValuePair<string, string>>();
                                    //real
                                    to.Add(new KeyValuePair<string, string>(contact.Email, contact.FirstName + contact.LastName));
                                   
                                    string body = string.Empty;
                                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/invitationMailer.html")))
                                    {
                                        body = reader.ReadToEnd();
                                    }
                                    string Subject = "SnapCor Customer Portal Invitation";
                                    string link = "<a href=\"" + Properties.Settings.Default.FrontEndUrl + "customer-signup?email=" + contact.Email + "&IdContact=" + contact.Id + "\">" + "Sign Up" + "</a > ";
                                    //string name = contact.FirstName + " " + contact.LastName;
                                    string emailBody = body;
                                    emailBody = emailBody.Replace("##InvitationLink##", link);
                                    emailBody = emailBody.Replace("##Name##", contact.FirstName);
                                    try
                                    {
                                        Mail.Send(
                                            new KeyValuePair<string, string>(From.Email, From.FirstName + " " + From.LastName),
                                            to,
                                            cc,
                                            Subject,
                                            emailBody,
                                            true
                                        );
                                    }
                                      
                                    catch
                                    {
                                        var removeinvitation = rpoContext.CustomerInvitationStatus.Where(x => x.IdContact == (int)jobContact.IdContact).FirstOrDefault();
                                        rpoContext.CustomerInvitationStatus.Remove(removeinvitation);
                                        throw new RpoBusinessException(StaticMessages.MailSendingFailed);
                                    }
                                    return Ok(new
                                    {
                                        idcustomer = customerid,

                                        message = "This Contact Is Not Registerd On SnapCor! Invitation Mail Sent"
                                    });
                                }
                                // }

                                rpoContext.SaveChanges();
                                if (customer != null)
                                {
                                    customerid = customer.Id;
                                }
                                //return Ok(new
                                //{
                                //    idcustomer = customerid,

                                //    message = "This Contact Is Not Registerd On SnapCor! Invitation Mail Sent"
                                //});
                                //return Ok("Mail sent successfully");
                            }

                            else
                            {
                                throw new RpoBusinessException("This contact doens't have Email Id.So can't give access or send Invitation");
                            }
                        }
                        else
                        {
                            return this.NotFound();
                        }
                    }
                }
                else if (jobContact.hasJobAccess == false)
                {
                    string ContactEmail = rpoContext.Contacts.Where(x => x.Id == jobContact.IdContact).FirstOrDefault().Email;
                    var customer = rpoContext.Customers.Where(x => x.EmailAddress == ContactEmail).Select(y => y.Id).FirstOrDefault();
                    var CustomerJobAccessDB = rpoContext.CustomerJobAccess.Where(x => x.IdCustomer == customer).FirstOrDefault();
                    if (CustomerJobAccessDB != null)
                    {
                        rpoContext.CustomerJobAccess.Remove(CustomerJobAccessDB);
                        rpoContext.SaveChanges();
                    }
                }
                #endregion
                rpoContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new RpoBusinessException(ex.Message);
            }
            JobContact nwJobContact = rpoContext.JobContacts.Include("Address").Include("Job").Include("JobContactType").FirstOrDefault(x => x.Id == idJobContact);

            string EditContact = JobHistoryMessages.UpdateContact
             .Replace("##JobContactName##", nwJobContact != null && nwJobContact.Contact != null ? nwJobContact.Contact.FirstName + " " + nwJobContact.Contact.LastName : JobHistoryMessages.NoSetstring)
             .Replace("##ContactType##", nwJobContact != null && nwJobContact.JobContactType != null && !string.IsNullOrEmpty(nwJobContact.JobContactType.Name) ? nwJobContact.JobContactType.Name : JobHistoryMessages.NoSetstring)
             .Replace("##MainClientBillingClient##", !string.IsNullOrEmpty(clientname) ? clientname : string.Empty)
              .Replace("##MailingAddress##", nwJobContact != null && nwJobContact.Address != null ? (!string.IsNullOrEmpty(nwJobContact.Address.Address1) ? nwJobContact.Address.Address1 + "," : string.Empty) + " " + (!string.IsNullOrEmpty(nwJobContact.Address.Address2) ? nwJobContact.Address.Address2 + "," : string.Empty) + " " + (!string.IsNullOrEmpty(nwJobContact.Address.City) ? nwJobContact.Address.City + "," : string.Empty) + " " + (!string.IsNullOrEmpty(nwJobContact.Address.ZipCode) ? nwJobContact.Address.ZipCode : string.Empty) : JobHistoryMessages.NoSetstring);

            Common.SaveJobHistory(employee.Id, idJob, EditContact, JobHistoryType.Contacts);

            if (jobContact.JobContactJobContactGroups != null && jobContact.JobContactJobContactGroups.Count() > 0)
            {
                foreach (JobContactJobContactGroup item in jobContact.JobContactJobContactGroups)
                {
                    if (item.Id > 0)
                    {
                        var jobContactJobContactGroup = rpoContext.JobContactJobContactGroups.FirstOrDefault(x => x.Id == item.Id);
                        jobContactJobContactGroup.IdJobContact = item.IdJobContact;
                        jobContactJobContactGroup.IdJobContactGroup = item.IdJobContactGroup;
                        rpoContext.SaveChanges();
                    }
                    else
                    {
                        JobContactJobContactGroup jobContactJobContactGroup = new JobContactJobContactGroup();
                        jobContactJobContactGroup.IdJobContact = item.IdJobContact;
                        jobContactJobContactGroup.IdJobContactGroup = item.IdJobContactGroup;
                        rpoContext.JobContactJobContactGroups.Add(jobContactJobContactGroup);
                        rpoContext.SaveChanges();
                    }
                }
            }

            try
            {
                rpoContext.SaveChanges();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobContactExists(idJobContact))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }
            JobContact jobcontactresponse = rpoContext.JobContacts.Find(jobContact.Id);       
            return Ok(FormatDetails(jobcontactresponse));
        }
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobs/contacts/PutJobContactHiddenValue/{idJobContact}/{IshiddenFromCustomer}")]
        public IHttpActionResult PutJobContactHiddenValue(int idJobContact, bool IshiddenFromCustomer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            JobContact oldJobContact = rpoContext.JobContacts.Include("Address").Include("Job").Include("JobContactType").FirstOrDefault(x => x.Id == idJobContact);
            if (oldJobContact != null)
            {
                oldJobContact.IshiddenFromCustomer = IshiddenFromCustomer;
                rpoContext.Entry(oldJobContact).State = EntityState.Modified;
                rpoContext.SaveChanges();
                JobContact jobcontactresponse = rpoContext.JobContacts.Find(idJobContact);
                return Ok(FormatDetails(jobcontactresponse));
            }
            throw new RpoBusinessException("Job Contact doesn't exist");           
        }

            /// <summary>
            /// Deletes the job contact.
            /// </summary>
            /// <param name="idJob">The identifier job.</param>
            /// <param name="idJobContact">The identifier job contact.</param>
            /// <returns>delete the record of job contacts.</returns>
            [HttpDelete]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobs/{idJob}/contacts/{idJobContact}")]
        public IHttpActionResult DeleteJobContact(int idJob, int idJobContact)
        {
            Job job = rpoContext.Jobs.Find(idJob);
            if (job == null)
            {
                return this.NotFound();
            }

            JobContact jobContact = rpoContext.JobContacts.Find(idJobContact);
            if (jobContact == null)
            {
                return this.NotFound();
            }
            bool checkmainclient = jobContact.IsMainCompany;

            string deleteContact = JobHistoryMessages.DeleteContact
                .Replace("##JobContactName##", jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : JobHistoryMessages.NoSetstring)
                .Replace("##ContactType##", (from d in rpoContext.JobContactTypes where d.Id == jobContact.IdJobContactType select d.Name).FirstOrDefault() != null ? (from d in rpoContext.JobContactTypes where d.Id == jobContact.IdJobContactType select d.Name).FirstOrDefault() : JobHistoryMessages.NoSetstring);        
            string ContactEmail = rpoContext.Contacts.Where(x => x.Id == jobContact.IdContact).Select(x => x.Email).FirstOrDefault();
            int idcustomer= rpoContext.Customers.Where(x => x.EmailAddress == ContactEmail).Select(x => x.Id).FirstOrDefault();
           var customerjobaccess= rpoContext.CustomerJobAccess.Where(x => x.IdJob == idJob && x.IdCustomer == idcustomer).FirstOrDefault();
            if (customerjobaccess != null)
            {
                var jobname = rpoContext.CustomerJobNames.Where(x => x.IdCustomerJobAccess == customerjobaccess.Id).FirstOrDefault();
                if (jobname != null)
                rpoContext.CustomerJobNames.Remove(jobname);
                rpoContext.CustomerJobAccess.Remove(customerjobaccess);
              //  rpoContext.SaveChanges();
            }
            rpoContext.JobContacts.Remove(jobContact);

            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

            job.LastModiefiedDate = DateTime.UtcNow;
            job.LastModifiedBy = employee.Id;
            if (checkmainclient == true)
            {
                job.IdCompany = null;
                job.IdContact = null;
                job.IdJobContactType = null;
            }
            rpoContext.SaveChanges();

            Common.SaveJobHistory(employee.Id, idJob, deleteContact, JobHistoryType.Contacts);

            return Ok(new { status = "succssess" });
        }
        /// <summary>
        /// Gets the list of all job contacts.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Get the list of all job contact.</returns>
      
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobs/{idJob}/AllContactsForCustomer")] //customer side listing contact
        public IHttpActionResult AllContactsForCustomer([FromUri] DataTableParameters dataTableParameters, [FromUri] int idJob)
        {
            IQueryable<JobContact> jobContacts = rpoContext
                .JobContacts.Include("Company")
                .Include("Contact")
                .Include("JobContactType")
                .Include("Address")
                .Include("JobContactJobContactGroups.JobContactGroup")
                .Where(jc => jc.IdJob == idJob &&jc.IshiddenFromCustomer==false);

            var recordsTotal = jobContacts.Count();
            var recordsFiltered = recordsTotal;

            var result = jobContacts.AsEnumerable()
                .Select(jc => Format(jc))
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

        private JobContactDetailDTO FormatDetailsforcustomer(JobContact jobContact)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
           var customer = rpoContext.Customers.Where(x => x.IdContcat == jobContact.IdContact).FirstOrDefault();
            var jobaccess= rpoContext.CustomerJobAccess.Where(x => x.IdCustomer == customer.Id && x.IdJob== jobContact.IdJob).FirstOrDefault();
            bool hasjobaccess = false;
            if (jobaccess != null)
            {
                hasjobaccess = true;
            }
            return new JobContactDetailDTO
            {

                Id = jobContact.Id,
                IdJob = jobContact.IdJob,
                IdCompany = jobContact.IdCompany,
                IdContact = jobContact.IdContact,
                IdJobContactType = jobContact.IdJobContactType,
                IdAddress = jobContact.IdAddress,
                IsBilling = jobContact.IsBilling,
                IsMainCompany = jobContact.IsMainCompany,
                JobContactJobContactGroups = jobContact.JobContactJobContactGroups != null && jobContact.JobContactJobContactGroups.Count() > 0 ? jobContact.JobContactJobContactGroups.Select(x => new JobContactJobContactGroupDTO
                {
                    Id = x.Id,
                    IdJobContact = x.IdJobContact,
                    IdJobContactGroup = x.IdJobContactGroup,
                    JobContact = x.JobContact != null && x.JobContact.Contact != null ? x.JobContact.Contact.FirstName + " " + x.JobContact.Contact.LastName : string.Empty,
                    JobContactGroup = x.JobContactGroup != null ? x.JobContactGroup.Name : string.Empty,
                }).ToList() : null,
                CreatedBy = jobContact.CreatedBy,
                CreatedByEmployee = jobContact.CreatedByEmployee != null ? jobContact.CreatedByEmployee.FirstName + " " + jobContact.CreatedByEmployee.LastName : string.Empty,
                CreatedDate = jobContact.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobContact.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobContact.CreatedDate,
                LastModifiedDate = jobContact.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobContact.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobContact.LastModifiedDate,
                LastModifiedBy = jobContact.LastModifiedBy != null ? jobContact.LastModifiedBy : jobContact.CreatedBy,
                LastModifiedByEmployee = jobContact.LastModifiedByEmployee != null ? jobContact.LastModifiedByEmployee.FirstName + " " + jobContact.LastModifiedByEmployee.LastName : (jobContact.CreatedByEmployee != null ? jobContact.CreatedByEmployee.FirstName + " " + jobContact.CreatedByEmployee.LastName : string.Empty),
                HasJobAccess = hasjobaccess
                };
        }
        private JobContactDetailDTO FormatDetails(JobContact jobContact)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            // var customer = rpoContext.Customers.Where(x => x.IdContcat == jobContact.IdContact).FirstOrDefault();
          string ContactEmail=  rpoContext.Contacts.Where(x => x.Id == jobContact.IdContact).FirstOrDefault().Email;
            var customer = rpoContext.Customers.Where(x => x.EmailAddress == ContactEmail).FirstOrDefault();
            bool hasjobaccess=false;
            bool IsRegisteredCustomer = false;
            if (customer != null)
            {
                IsRegisteredCustomer = true;
                var jobaccess = rpoContext.CustomerJobAccess.Where(x => x.IdCustomer == customer.Id && x.IdJob == jobContact.IdJob).FirstOrDefault();                
                if (jobaccess != null)
                {
                    hasjobaccess = true;
                }
              
            }
        
            return new JobContactDetailDTO
            {

                Id = jobContact.Id,
                IdJob = jobContact.IdJob,
                IdCompany = jobContact.IdCompany,
                IdContact = jobContact.IdContact,
                IdJobContactType = jobContact.IdJobContactType,
                IdAddress = jobContact.IdAddress,
                IsBilling = jobContact.IsBilling,
                IsMainCompany = jobContact.IsMainCompany,
                JobContactJobContactGroups = jobContact.JobContactJobContactGroups != null && jobContact.JobContactJobContactGroups.Count() > 0 ? jobContact.JobContactJobContactGroups.Select(x => new JobContactJobContactGroupDTO
                {
                    Id = x.Id,
                    IdJobContact = x.IdJobContact,
                    IdJobContactGroup = x.IdJobContactGroup,
                    JobContact = x.JobContact != null && x.JobContact.Contact != null ? x.JobContact.Contact.FirstName + " " + x.JobContact.Contact.LastName : string.Empty,
                    JobContactGroup = x.JobContactGroup != null ? x.JobContactGroup.Name : string.Empty,
                }).ToList() : null,
                CreatedBy = jobContact.CreatedBy,
                CreatedByEmployee = jobContact.CreatedByEmployee != null ? jobContact.CreatedByEmployee.FirstName + " " + jobContact.CreatedByEmployee.LastName : string.Empty,
                CreatedDate = jobContact.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobContact.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobContact.CreatedDate,
                LastModifiedDate = jobContact.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobContact.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobContact.LastModifiedDate,
                LastModifiedBy = jobContact.LastModifiedBy != null ? jobContact.LastModifiedBy : jobContact.CreatedBy,
                LastModifiedByEmployee = jobContact.LastModifiedByEmployee != null ? jobContact.LastModifiedByEmployee.FirstName + " " + jobContact.LastModifiedByEmployee.LastName : (jobContact.CreatedByEmployee != null ? jobContact.CreatedByEmployee.FirstName + " " + jobContact.CreatedByEmployee.LastName : string.Empty),
                HasJobAccess = customer!=null?hasjobaccess:false,
                IsRegisteredCustomer= IsRegisteredCustomer
            };
        }

        private JobContactListDTO Format(JobContact jc)
        {
            string jobContactGroup = string.Empty;

            if (jc.JobContactJobContactGroups != null && jc.JobContactJobContactGroups.Count() > 0)
            {
                jobContactGroup = string.Join(",", jc.JobContactJobContactGroups.Select(x => x.JobContactGroup.Name));
            }
            string ContactEmail = rpoContext.Contacts.Where(x => x.Id == jc.IdContact).FirstOrDefault().Email;
            //var customer = rpoContext.Customers.Where(x => x.IdContcat == jc.IdContact).FirstOrDefault();
            var customer = rpoContext.Customers.Where(x => x.EmailAddress == ContactEmail).FirstOrDefault();
            string IsAuthorized = "Un-Authorized";
            if (customer != null)
            { 
            var jobaccess = rpoContext.CustomerJobAccess.Where(x => x.IdJob == jc.IdJob && x.IdCustomer == customer.Id).FirstOrDefault();
          
            if (jobaccess != null)
            {
                IsAuthorized = "Authorized";
            }
           }
            return new JobContactListDTO
            {
                companyName = jc.Company != null ? jc.Company.Name : string.Empty,
                contactName = jc.Contact != null ? jc.Contact.FirstName + " " + jc.Contact.LastName + (jc.Contact.Suffix != null ? " " + jc.Contact.Suffix.Description : string.Empty) : string.Empty,
                itemName = jc.Contact != null ? jc.Contact.FirstName + " " + jc.Contact.LastName  +(jc.Contact.Suffix != null ? " " + jc.Contact.Suffix.Description : string.Empty) : string.Empty,
                email = jc.Contact != null ? jc.Contact.Email : string.Empty,
                ext = jc.Contact != null ? jc.Contact.WorkPhoneExt : string.Empty,
                id = jc.Id,
                idContact = jc.IdContact,
                idJob = jc.IdJob,
                jobcontactType = jc.JobContactType != null ? jc.JobContactType.Name : string.Empty,
                workPhone = jc.Contact != null ? jc.Contact.WorkPhone : string.Empty,
                address = jc.Address != null ? jc.Address.Address1 + " " + jc.Address.Address2 + " " + jc.Address.City + " " + (jc.Address.State != null ? jc.Address.State.Name : string.Empty) : string.Empty,
                IsMainCompany = jc.IsMainCompany,
                IsBilling = jc.IsBilling,
                GroupName = jobContactGroup,
                IsActive = jc.Contact.IsActive,
                IsHidden = jc.IshiddenFromCustomer,
                IsAuthorized= IsAuthorized,
                mobilePhone=jc.Contact.MobilePhone

            };
        }


        /// <summary>
        /// Jobs the contact exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool JobContactExists(int id)
        {
            return rpoContext.JobContacts.Count(e => e.Id == id) > 0;
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
    }
}