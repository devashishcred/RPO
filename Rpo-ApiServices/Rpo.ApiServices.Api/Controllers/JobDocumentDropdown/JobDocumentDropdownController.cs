// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Richa Patel
// Created          : 04-28-2018
//
// Last Modified By : Richa Patel
// Last Modified On : 04-28-2018
// ***********************************************************************
// <copyright file="JobDocumentDrodownController.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************


/// <summary>
/// The JobDocumentDropdown namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobDocumentDrodown
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Enums;
    using Filters;
    using Model;
    using Model.Models;
    using Contacts;
    /// <summary>
    /// Class JobDocumentDrodownController.
    /// </summary>
    public class JobDocumentDrodownController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the job document type dropdown.
        /// </summary>
        /// <param name="idDocument">The identifier document.</param>
        /// <returns> Gets the job document type dropdown.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/JobDocumentType/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetJobDocumentTypeDropdown(int idJob, int idDocument, int idParent = 0)
        {
            var result = rpoContext.JobDocumentTypes.Where(x => x.IdDocument == idDocument).AsEnumerable().Select(c => new IdItemName
            {
                Id = c.Id.ToString(),
                ItemName = c.Type
            }).ToArray();

            return Ok(result);
        }
        /// <summary>
        /// Gets the job document type with ordering dropdown.
        /// </summary>
        /// <param name="idDocument">The identifier document.</param>
        /// <returns>ets the job document type with ordering dropdown.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/JobDocumentTypeOrder/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetJobDocumentTypeDropdownOrder(int idJob, int idDocument, int idParent = 0)
        {
            var result = rpoContext.JobDocumentTypes.Where(x => x.IdDocument == idDocument).AsEnumerable().Select(c => new IdItemName
            {
                Id = c.Id.ToString(),
                ItemName = c.Type
            }).ToArray();

            List<IdItemName> iditem = new List<IdItemName>();

            List<IdItemName> tlistFiltered = result
                    .Where(item => item.ItemName == "Design Applicant Initial Identification")
                        .ToList();
            iditem.AddRange(tlistFiltered);
            List<IdItemName> tlistFiltered1 = result
                   .Where(item => item.ItemName == "Design Applicant Reidentification")
                       .ToList();
            iditem.AddRange(tlistFiltered1);

            List<IdItemName> tlistFiltered2 = result
                  .Where(item => item.ItemName == "Design/Inspector Initial Identification")
                      .ToList();
            iditem.AddRange(tlistFiltered2);


            List<IdItemName> tlistFiltered3 = result
                  .Where(item => item.ItemName == "Inspector Initial Identification")
                      .ToList();
            iditem.AddRange(tlistFiltered3);

            List<IdItemName> tlistFiltered4 = result
                  .Where(item => item.ItemName == "Inspector Reidentification")
                      .ToList();
            iditem.AddRange(tlistFiltered4);

            List<IdItemName> tlistFiltered5 = result
                  .Where(item => item.ItemName == "Certification")
                      .ToList();
            iditem.AddRange(tlistFiltered5);

            List<IdItemName> tlistFiltered6 = result
                 .Where(item => item.ItemName == "Withdrawal")
                     .ToList();
            iditem.AddRange(tlistFiltered6);


            return Ok(iditem);
        }

        /// <summary>
        /// Gets the job certifier dropdown.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="idDocument">The identifier document.</param>
        /// <returns>Gets the job certifier list for dropdown.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/Certifier/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetJobCertifierDropdown(int idJob, int idDocument, int idParent = 0)
        {
            List<IdItemName> result = rpoContext.JobContacts.Where(x => x.IdJob == idJob).AsEnumerable().Select(c => new IdItemName
            {
                Id = c.IdContact.ToString(),
                ItemName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName : string.Empty
            }).ToList();

            if (result != null)
            {
                Job job = rpoContext.Jobs.Include("RfpAddress.OwnerContact").FirstOrDefault(x => x.Id == idJob);
                if (job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null
                    && !result.Contains(new IdItemName
                    {
                        Id = job.RfpAddress.IdOwnerContact.ToString(),
                        ItemName = job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName
                    }))
                {
                    result.Add(new IdItemName
                    {
                        Id = job.RfpAddress.IdOwnerContact.ToString(),
                        ItemName = job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName
                    });
                }
            }
            else
            {
                result = new List<IdItemName>();
                Job job = rpoContext.Jobs.Include("RfpAddress.OwnerContact").FirstOrDefault(x => x.Id == idJob);
                if (job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null)
                {
                    result.Add(new IdItemName
                    {
                        Id = job.RfpAddress.IdOwnerContact.ToString(),
                        ItemName = job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName
                    });
                }
            }
            return Ok(result.Select(c => new IdItemName
            {
                Id = c.Id.ToString(),
                ItemName = c.ItemName
            }).Distinct());
        }

        /// <summary>
        /// Gets the contacts dropdown.
        /// </summary>
        /// <returns>Gets the contacts list for dropdown.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/Contacts/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetContactsDropdown(int idJob, int idDocument, int idParent = 0)
        {
            var result = rpoContext.Contacts.AsEnumerable().Select(c => new IdItemName
            {
                Id = c.Id.ToString(),
                ItemName = c.FirstName + " " + c.LastName
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the job contacts type dropdown.
        /// </summary>
        /// <returns>Gets the job contacts type list for dropdown.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/JobContacts/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetJobContactsDropdown(int idJob, int idDocument, int idParent = 0)
        {
            var result = rpoContext.JobContacts.Where(c => c.IdJob == idJob).AsEnumerable().Select(c => new IdItemName
            {
                Id = c.Id.ToString(),
                ItemName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName + (c.Contact.Suffix != null ? " " + c.Contact.Suffix.Description : string.Empty) + (c.Contact.Company != null ? " (" + c.Contact.Company.Name + ")" : string.Empty) : string.Empty
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the job contacts SIA dropdown.
        /// </summary>
        /// <param name="idDocument">The identifier document.</param>
        /// <returns>Gets the job contacts SIA list for dropdown.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/JobContactsSIA/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetJobContactsDropdownWithSIA(int idJob, int idDocument, int idParent = 0)
        {


            var result = rpoContext.JobContacts.Where(c => c.IdJob == idJob).AsEnumerable().Select(c => new IdItemNameNew
            {
                Id = c.Id.ToString(),
                ItemName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName + (c.Contact.Company != null ? " (" + c.Contact.Company.Name + ")" : string.Empty) : string.Empty,
                SIANumber = c.Contact.Company != null && c.Contact.Company.SpecialInspectionAgencyNumber != null ? c.Contact.Company.SpecialInspectionAgencyNumber : string.Empty
            }).ToArray();

            return Ok(result);
        }
        /// <summary>
        /// Gets the job Work Permit Type dropdown.
        /// </summary>
        /// <param name="idDocument">The identifier document.</param>
        /// <returns>Gets the job Work Permit Type list for dropdown.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/JobWorkPermit/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetJobWorkPermitDropdown(int idJob, int idDocument, int idParent = 0)
        {
            if (idParent > 0)
            {
                var result = rpoContext.JobApplicationWorkPermitTypes.Include("JobApplication").Where(c => c.JobApplication.IdJob == idJob && c.IdJobApplication == idParent && c.IsCompleted == null || c.IsCompleted == false).Select(x => new IdItemName
                {
                    Id = x.Id.ToString(),
                    //   ItemName = x.PermitType != null && x.PermitType != "" ? x.PermitType : (x.JobWorkType != null ? x.JobWorkType.Description + "-" + x.JobWorkType.Code : string.Empty)
                    ItemName = (string.IsNullOrEmpty(x.Code) ? string.Empty : x.Code) + (string.IsNullOrEmpty(x.PermitType) ? string.Empty : " | " + x.PermitType) + (string.IsNullOrEmpty(x.PermitNumber) ? string.Empty : " | " + x.PermitNumber)


                }).ToArray();

                foreach (var item in result)
                {
                    item.ItemName = item.ItemName.TrimEnd('-').TrimEnd().TrimEnd('|');
                    item.ItemName = item.ItemName.TrimStart('-').TrimStart().TrimStart('|');

                }
                return Ok(result);
            }
            else
            {
                var result = rpoContext.JobApplicationWorkPermitTypes.Include("JobApplication").Where(c => c.JobApplication.IdJob == idJob && c.IsCompleted == null || c.IsCompleted == false).Select(x => new IdItemName
                {
                    Id = x.Id.ToString(),
                    ItemName = x.PermitType != null && x.PermitType != "" ? x.PermitType : (x.JobWorkType != null ? x.JobWorkType.Description + "-" + x.JobWorkType.Code : string.Empty)
                }).ToArray();

                return Ok(result);
            }

        }

        /// <summary>
        /// Gets the all Work Permit  dropdown.
        /// </summary>
        /// <param name="idDocument">The identifier document.</param>
        /// <returns>Gets the all Work Permit list for dropdown.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/JobWorkPermitAll")]
        public IHttpActionResult GetJobWorkPermitAllDropdown()
        {

            var result = rpoContext.JobWorkTypes
                .Include("JobApplication")
                //.Where(c => c.JobApplication.IdJob == idJob && c.IdJobApplication == idParent)
                .Select(x => new IdItemName
                {
                    Id = x.Id.ToString(),
                    ItemName = x.Description
                }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the job application number type dropdown.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>IHttpActionResult.</returns>
        //[Authorize]
        //[RpoAuthorize]
        //[HttpGet]
        //[Route("api/jobdocumentdrodown/JobApplicationNumberType/{idJob}/{idDocument}/{idParent}")]
        //public IHttpActionResult GetJobApplicationNumberTypeDropdown(int idJob, int idDocument, int idParent = 0)
        //{
        //    var result = rpoContext.JobApplications.Include("JobApplicationType").Where(c => c.IdJob == idJob).Select(x => new IdItemName
        //    {
        //        Id = x.Id.ToString(),
        //        ItemName = ((x.ApplicationNumber != null && x.ApplicationNumber != string.Empty) ? x.ApplicationNumber + " " : string.Empty) + (x.JobApplicationType != null ? x.JobApplicationType.Description : string.Empty)
        //    }).ToArray();

        //    return Ok(result);
        //}
        ///<summary>
        /// Gets the job application number type dropdown.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns> Gets the job application number type list for dropdown.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/JobApplicationNumberType/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetJobApplicationNumberTypeDropdown(int idJob, int idDocument, int idParent = 0)
        {
            int IdDOBApplicationType = ApplicationType.DOB.GetHashCode();
            int IdDOTApplicationType = ApplicationType.DOT.GetHashCode();
            int IdDEPApplicationType = ApplicationType.DEP.GetHashCode();
            if (idParent == 0)
            {
                var result = rpoContext.JobApplications.Include("JobApplicationType").Where(c => c.IdJob == idJob).Select(x => new IdItemName
                {

                    Id = x.Id.ToString(),
                    ItemName = x.JobApplicationType.IdParent == IdDOBApplicationType ?
                    (x.JobApplicationType != null ? x.JobApplicationType.Description : string.Empty) + ((x.ApplicationNumber != null && x.ApplicationNumber != string.Empty) ? " | " + x.ApplicationNumber : string.Empty) :


                    x.JobApplicationType.IdParent == IdDOTApplicationType ?
                    ((x.StreetWorkingOn != null && x.StreetWorkingOn != string.Empty) ? x.StreetWorkingOn : string.Empty) +
                    ((x.StreetFrom != null && x.StreetFrom != string.Empty) ? "-" + x.StreetFrom : string.Empty) +
                    ((x.StreetTo != null && x.StreetTo != string.Empty) ? "-" + x.StreetTo : string.Empty) +
                    ((x.ApplicationNumber != null && x.ApplicationNumber != string.Empty) ? " | " + x.ApplicationNumber : string.Empty) +
                    (x.JobApplicationType != null ? " | " + x.JobApplicationType.Description : string.Empty) :


                    x.JobApplicationType.IdParent == IdDEPApplicationType ?
                    (x.JobApplicationType != null ? x.JobApplicationType.Description + " | " : string.Empty) +
                    ((x.StreetWorkingOn != null && x.StreetWorkingOn != string.Empty) ? x.StreetWorkingOn + "-" : string.Empty) +
                    ((x.StreetFrom != null && x.StreetFrom != string.Empty) ? x.StreetFrom + "-" : string.Empty) +
                    ((x.StreetTo != null && x.StreetTo != string.Empty) ? x.StreetTo : string.Empty) : string.Empty
                }).ToArray();

                foreach (var item in result)
                {
                    item.ItemName = item.ItemName.TrimEnd('-').TrimEnd().TrimEnd('|');
                    item.ItemName = item.ItemName.TrimStart('-').TrimStart().TrimStart('|');

                }
                return Ok(result);
            }
            else
            {
                var result = rpoContext.JobApplications.Include("JobApplicationType").Where(c => c.IdJob == idJob && c.JobApplicationType.IdParent == idParent).Select(x => new IdItemName
                {

                    Id = x.Id.ToString(),
                    ItemName = x.JobApplicationType.IdParent == IdDOBApplicationType ?
                     (x.JobApplicationType != null ? x.JobApplicationType.Description : string.Empty) + ((x.ApplicationNumber != null && x.ApplicationNumber != string.Empty) ? " | " + x.ApplicationNumber : string.Empty) :


                     x.JobApplicationType.IdParent == IdDOTApplicationType ?
                     ((x.StreetWorkingOn != null && x.StreetWorkingOn != string.Empty) ? x.StreetWorkingOn : string.Empty) +
                     ((x.StreetFrom != null && x.StreetFrom != string.Empty) ? "-" + x.StreetFrom : string.Empty) +
                     ((x.StreetTo != null && x.StreetTo != string.Empty) ? "-" + x.StreetTo : string.Empty) +
                     ((x.ApplicationNumber != null && x.ApplicationNumber != string.Empty) ? " | " + x.ApplicationNumber : string.Empty) +
                     (x.JobApplicationType != null ? " | " + x.JobApplicationType.Description : string.Empty) :


                     x.JobApplicationType.IdParent == IdDEPApplicationType ?
                     (x.JobApplicationType != null ? x.JobApplicationType.Description + " | " : string.Empty) +
                     ((x.StreetWorkingOn != null && x.StreetWorkingOn != string.Empty) ? x.StreetWorkingOn + "-" : string.Empty) +
                     ((x.StreetFrom != null && x.StreetFrom != string.Empty) ? x.StreetFrom + "-" : string.Empty) +
                     ((x.StreetTo != null && x.StreetTo != string.Empty) ? x.StreetTo : string.Empty) : string.Empty
                }).ToArray();

                foreach (var item in result)
                {
                    item.ItemName = item.ItemName.TrimEnd('-').TrimEnd().TrimEnd('|');
                    item.ItemName = item.ItemName.TrimStart('-').TrimStart().TrimStart('|');

                }
                return Ok(result);
            }

            return null;

        }
        ///<summary>
        /// Gets the job application number against the document pw517 dropdown.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the job application number against the document pw517 list for dropdown..</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/JobApplicationNumberTypePW517/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetJobApplicationNumberTypeDropdownPW517(int idJob, int idDocument, int idParent = 0)
        {
            int IdDOBApplicationType = ApplicationType.DOB.GetHashCode();
            int IdDOTApplicationType = ApplicationType.DOT.GetHashCode();
            int IdDEPApplicationType = ApplicationType.DEP.GetHashCode();

            var result = rpoContext.JobApplications.Include("JobApplicationType").Where(c => c.IdJob == idJob).Select(x => new IdItemName
            {

                Id = x.Id.ToString(),
                ItemName = x.JobApplicationType.IdParent == IdDOBApplicationType ?
                (x.JobApplicationType != null ? x.JobApplicationType.Description : string.Empty) + ((x.ApplicationNumber != null && x.ApplicationNumber != string.Empty) ? " | " + x.ApplicationNumber : string.Empty) :


                x.JobApplicationType.IdParent == IdDOTApplicationType ?
                ((x.StreetWorkingOn != null && x.StreetWorkingOn != string.Empty) ? x.StreetWorkingOn : string.Empty) +
                ((x.StreetFrom != null && x.StreetFrom != string.Empty) ? "-" + x.StreetFrom : string.Empty) +
                ((x.StreetTo != null && x.StreetTo != string.Empty) ? "-" + x.StreetTo : string.Empty) +
                ((x.ApplicationNumber != null && x.ApplicationNumber != string.Empty) ? " | " + x.ApplicationNumber : string.Empty) +
                (x.JobApplicationType != null ? " | " + x.JobApplicationType.Description : string.Empty) :


                x.JobApplicationType.IdParent == IdDEPApplicationType ?
                (x.JobApplicationType != null ? x.JobApplicationType.Description + " | " : string.Empty) +
                ((x.StreetWorkingOn != null && x.StreetWorkingOn != string.Empty) ? x.StreetWorkingOn + "-" : string.Empty) +
                ((x.StreetFrom != null && x.StreetFrom != string.Empty) ? x.StreetFrom + "-" : string.Empty) +
                ((x.StreetTo != null && x.StreetTo != string.Empty) ? x.StreetTo : string.Empty) : string.Empty
            }).ToArray();

            foreach (var item in result)
            {
                item.ItemName = item.ItemName.TrimEnd('-').TrimEnd().TrimEnd('|');
                item.ItemName = item.ItemName.TrimStart('-').TrimStart().TrimStart('|');

            }
            return Ok(result);


        }
        ///<summary>
        /// Gets the DOB job application number against the document  dropdown.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the DOB job application number against the document list for dropdown..</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/DOBJobApplicationNumberType/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetDOBJobApplicationNumberTypeDropdown(int idJob, int idDocument, int idParent = 0)
        {
            int IdDOBApplicationType = ApplicationType.DOB.GetHashCode();
            var result = rpoContext.JobApplications.Include("JobApplicationType").Where(c => c.IdJob == idJob
            && c.JobApplicationType.IdParent == IdDOBApplicationType).Select(x => new IdItemName
            {
                Id = x.Id.ToString(),
                // ItemName = ((x.ApplicationNumber != null && x.ApplicationNumber != string.Empty) ? x.ApplicationNumber + " " : string.Empty) + (x.JobApplicationType != null ? x.JobApplicationType.Description : string.Empty)
                ItemName = (x.JobApplicationType != null ? x.JobApplicationType.Description : string.Empty)
+ ((x.ApplicationNumber != null && x.ApplicationNumber != string.Empty) ? " | " + x.ApplicationNumber : string.Empty)

            }).ToArray();
            foreach (var item in result)
            {
                item.ItemName = item.ItemName.TrimEnd('-').TrimEnd().TrimEnd('|');
                item.ItemName = item.ItemName.TrimStart('-').TrimStart().TrimStart('|');

            }
            return Ok(result);
        }
        ///<summary>
        /// Gets the DEP job application number against the document pw517 dropdown.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the DEP job application number against the document list for dropdown.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/DEPJobApplicationNumberType/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetDEPJobApplicationNumberTypeDropdown(int idJob, int idDocument, int idParent = 0)
        {
            int IdDOBApplicationType = ApplicationType.DEP.GetHashCode();
            var result = rpoContext.JobApplications.Include("JobApplicationType").Where(c => c.IdJob == idJob
            && c.JobApplicationType.IdParent == IdDOBApplicationType).Select(x => new IdItemName
            {
                Id = x.Id.ToString(),
                // ItemName = ((x.ApplicationNumber != null && x.ApplicationNumber != string.Empty) ? x.ApplicationNumber + " " : string.Empty) + (x.JobApplicationType != null ? x.JobApplicationType.Description : string.Empty)
                ItemName = (x.JobApplicationType != null ? x.JobApplicationType.Description : string.Empty)
+ ((!string.IsNullOrEmpty(x.StreetWorkingOn) || !string.IsNullOrEmpty(x.StreetFrom) || !string.IsNullOrEmpty(x.StreetTo)) ? ((x.StreetWorkingOn != null ? x.StreetWorkingOn : string.Empty) + " | " + (x.StreetFrom != null ? x.StreetFrom : string.Empty) + " | " + (x.StreetTo != null ? x.StreetTo : string.Empty)) : string.Empty)
            }).ToArray();
            foreach (var item in result)
            {
                item.ItemName = item.ItemName.TrimEnd('-').TrimEnd().TrimEnd('|');
                item.ItemName = item.ItemName.TrimStart('-').TrimStart().TrimStart('|');

            }
            return Ok(result);
        }
        ///<summary>
        /// Gets the DOT job application number against the document  dropdown.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the DOT job application number against the document list for dropdown..</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/DOTJobApplicationNumberType/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetDOTJobApplicationNumberTypeDropdown(int idJob, int idDocument, int idParent = 0)
        {
            int IdDOTApplicationType = ApplicationType.DOT.GetHashCode();
            var result = rpoContext.JobApplications.Include("JobApplicationType").Where(c => c.IdJob == idJob
            && c.JobApplicationType.IdParent == IdDOTApplicationType).Select(x => new IdItemName
            {
                Id = x.Id.ToString(),
                //  ItemName = ((x.StreetWorkingOn != null && x.StreetWorkingOn != string.Empty) ? x.StreetWorkingOn + " -" : string.Empty) + ((x.StreetFrom != null && x.StreetFrom != string.Empty) ? x.StreetFrom + " -" : string.Empty) + ((x.StreetTo != null && x.StreetTo != string.Empty) ? x.StreetTo : string.Empty) + ((x.ApplicationNumber != null && x.ApplicationNumber != string.Empty) ? " | " + x.ApplicationNumber  : string.Empty) + (x.JobApplicationType != null ? " | " + x.JobApplicationType.Description : string.Empty)
                ItemName = ((x.StreetWorkingOn != null && x.StreetWorkingOn != string.Empty) ? x.StreetWorkingOn : string.Empty) +
                ((x.StreetFrom != null && x.StreetFrom != string.Empty) ? "-" + x.StreetFrom : string.Empty) +
                ((x.StreetTo != null && x.StreetTo != string.Empty) ? "-" + x.StreetTo : string.Empty) +
                ((x.ApplicationNumber != null && x.ApplicationNumber != string.Empty) ? " | " + x.ApplicationNumber : string.Empty) +
                (x.JobApplicationType != null ? " | " + x.JobApplicationType.Description : string.Empty)

            }).ToArray();

            foreach (var item in result)
            {
                item.ItemName = item.ItemName.TrimEnd('-').TrimEnd().TrimEnd('|');
                item.ItemName = item.ItemName.TrimStart('-').TrimStart().TrimStart('|');

            }

            return Ok(result);
        }

        /// <summary>
        /// Gets the active employees dropdown.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the active employees list for dropdown.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/ActiveEmployeesDropdown/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetActiveEmployeesDropdown(int idJob, int idDocument, int idParent = 0)
        {
            var result = rpoContext.Employees.Where(x => x.IsActive == true && x.IsArchive == false).AsEnumerable().Select(c => new IdItemName
            {
                Id = c.Id.ToString(),
                ItemName = c.FirstName + " " + c.LastName
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the Ten Days Notice Salutation
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="idDocument">The identifier document.</param>
        /// <returns>Gets the Ten Days Notice Salutation list.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/TenDaysNoticeSalutation/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetTenDaysNoticeSalutationDropdown(int idJob, int idDocument, int idParent = 0)
        {
            List<IdItemName> idItemNameList = new List<IdItemName>();
            idItemNameList.Add(new IdItemName { Id = "Dear Madame", ItemName = "Dear Madame" });
            idItemNameList.Add(new IdItemName { Id = "Dear Sir", ItemName = "Dear Sir" });
            idItemNameList.Add(new IdItemName { Id = "To Whom It May Concern", ItemName = "To Whom It May Concern" });
            return Ok(idItemNameList);
        }

        /// <summary>
        /// Gets the ten days notice scanned signed.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="idDocument">The identifier document.</param>
        /// <returns> Gets the ten days notice scanned signed list.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/TenDaysNoticeScannedSigned/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetTenDaysNoticeScannedSigned(int idJob, int idDocument = 0)
        {
            idDocument = Document.TenDAYN62395.GetHashCode();
            List<int> jobDocument = rpoContext.JobDocuments.Where(x => x.IdJob == idJob && x.IdDocument == idDocument).Select(x => x.Id).ToList();
            int tenDaysNotice_Recipient = DocumentPlaceHolderField.TenDaysNotice_Recipient.GetHashCode();

            List<IdItemName> idItemNameList = rpoContext.JobDocumentFields
                .Where(x => jobDocument.Contains(x.IdJobDocument) && x.DocumentField.Id == tenDaysNotice_Recipient)
                .AsEnumerable()
                .Select(x => TenDaysNoticeScannedSigned(x)).Distinct().ToList();
            return Ok(idItemNameList);
        }

        private IdItemName TenDaysNoticeScannedSigned(JobDocumentField jobDocumentField)
        {
            int tenDaysNotice_AddressAdjacent = DocumentPlaceHolderField.TenDaysNotice_AddressAdjacent.GetHashCode();
            string itemname = jobDocumentField.Value.Split('\n') != null && jobDocumentField.Value.Split('\n').Count() > 0 ? jobDocumentField.Value.Split('\n')[0] : string.Empty;
            JobDocumentField adjacentJobDocumentField = rpoContext.JobDocumentFields.FirstOrDefault(x => x.IdJobDocument == jobDocumentField.IdJobDocument && x.DocumentField.Id == tenDaysNotice_AddressAdjacent);
            string adjacentaddress = adjacentJobDocumentField != null ? adjacentJobDocumentField.ActualValue : string.Empty;
            return new IdItemName
            {
                Id = Convert.ToString(jobDocumentField.IdJobDocument),
                ItemName = jobDocumentField.Value,
                //ItemName = itemname + (adjacentaddress != null && adjacentaddress != "" ? " (" + adjacentaddress + ")" : string.Empty)
            };
        }
        /// <summary>
        /// Gets thejobviolation number against the document dropdown.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets thejobviolation number against the document list for dropdown.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/GetJobViolation/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetJobViolationDropdown(int idJob, int idDocument = 0)
        {
            //var result = rpoContext.JobViolations.Where(x => x.IdJob == idJob).AsEnumerable().Select(c => new IdItemName
            //{
            //    Id = c.Id.ToString(),
            //    ItemName = c.SummonsNumber
            //}).ToArray();
            
            var binNumber = rpoContext.Jobs.Include("RfpAddress").Where(x => x.Id == idJob).Select(i=>i.RfpAddress.BinNumber).FirstOrDefault();

            var result = rpoContext.JobViolations.Where(x => x.BinNumber == binNumber).AsEnumerable().Select(c => new IdItemName
            {
                Id = c.Id.ToString(),
                ItemName = c.SummonsNumber
            }).ToArray();





            return Ok(result);
        }
        /// <summary>
        /// Gets the list of TR8Inspection Type list.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the list of TR8Inspection Type list.</returns>

        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/TR8InspectionType/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetTR8InspectionType(int idJob, int idDocument = 0)
        {
            return Ok(GetTR8InspectionType());
        }

        public List<IdItemName> GetTR8InspectionType()
        {
            List<IdItemName> result = new List<IdItemName>();
            result.Add(new IdItemName { Id = "opgPOFI", ItemName = "Protection of exposed foundation insulation" });
            result.Add(new IdItemName { Id = "opgIPAR", ItemName = "Insulation placement and R values" });
            result.Add(new IdItemName { Id = "opgFUPR", ItemName = "Fenestration u-factor and product rating" });
            result.Add(new IdItemName { Id = "opgFALK", ItemName = "Fenestration air leakage" });
            result.Add(new IdItemName { Id = "opgFNAR", ItemName = "Fenestration areas" });
            result.Add(new IdItemName { Id = "opgASIV", ItemName = "Air sealing and insulation — visual" });
            result.Add(new IdItemName { Id = "opgASIT", ItemName = "Air sealing and insulation — testing" });
            result.Add(new IdItemName { Id = "opgLDWS", ItemName = "Loading deck weather seals" });
            result.Add(new IdItemName { Id = "opgVEST", ItemName = "Vestibules" });
            result.Add(new IdItemName { Id = "opgFRPL", ItemName = "Fireplaces" });
            result.Add(new IdItemName { Id = "opgSHTD", ItemName = "Shutoff dampers" });
            result.Add(new IdItemName { Id = "opgHVEQ", ItemName = "HVAC and service water heating equipment" });
            result.Add(new IdItemName { Id = "opgHVSC", ItemName = "HVAC and service water heating system controls" });
            result.Add(new IdItemName { Id = "opgHVIS", ItemName = "HVAC insulation and sealing" });
            result.Add(new IdItemName { Id = "opgDLKT", ItemName = "Duct leakage testing" });
            result.Add(new IdItemName { Id = "opgEENC", ItemName = "Electrical energy consumption" });
            result.Add(new IdItemName { Id = "opgLIDU", ItemName = "Lighting in dwelling units" });
            result.Add(new IdItemName { Id = "opgILPW", ItemName = "Interior lighting power" });
            result.Add(new IdItemName { Id = "opgELPW", ItemName = "Exterior lighting power" });
            result.Add(new IdItemName { Id = "opgLTCT", ItemName = "Lighting controls" });
            result.Add(new IdItemName { Id = "opgELMO", ItemName = "Electrical motors" });
            result.Add(new IdItemName { Id = "opgMNTI", ItemName = "Maintenance information" });
            result.Add(new IdItemName { Id = "opgPMCT", ItemName = "Permanent certificate" });
            result.Add(new IdItemName { Id = "opgSLRR", ItemName = "Solar Ready Requirements" });

            return result;
        }
        /// <summary>
        /// Gets the list of TR108 Inspection Type list.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the list of TR108 Inspection Type list.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/TR108InspectionType/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetTR108InspectionType(int idJob, int idDocument = 0)
        {
            return Ok(GetTR108InspectionType());
        }

        public List<IdItemName> GetTR108InspectionType()
        {
            List<IdItemName> result = new List<IdItemName>();
            result.Add(new IdItemName { Id = "opgFGC", ItemName = "Flood Zone Compliance" });
            result.Add(new IdItemName { Id = "opgFAT", ItemName = "Fire Alarm Test" });
            result.Add(new IdItemName { Id = "opgPEPM", ItemName = "Photoluminescent Exit Path Markings" });
            result.Add(new IdItemName { Id = "opgEPS", ItemName = "Emergency Power Systems (Generators)" });
            result.Add(new IdItemName { Id = "opgSSW", ItemName = "Structural Steel - Welding" });
            result.Add(new IdItemName { Id = "opgSSEB", ItemName = "Structural Steel - Erection & Bolting" });
            result.Add(new IdItemName { Id = "opgSCFS", ItemName = "Structural Cold-Formed Steel" });
            result.Add(new IdItemName { Id = "opgCCIP", ItemName = "Concrete - Cast-In-Place" });
            result.Add(new IdItemName { Id = "opgCP", ItemName = "Concrete - Precast" });
            result.Add(new IdItemName { Id = "opgCP2", ItemName = "Concrete - Prestressed" });
            result.Add(new IdItemName { Id = "opgMasonry", ItemName = "Masonry" });
            result.Add(new IdItemName { Id = "opgWOSFOSE", ItemName = "Wood - Off-Site fabrication of Structural Elements" });
            result.Add(new IdItemName { Id = "opgWIOHLD", ItemName = "Wood - Installation of High-Load Diaphragms" });
            result.Add(new IdItemName { Id = "opgWIOMPCT", ItemName = "Wood - Installation of Metal-Plate-Connected Trusses" });
            result.Add(new IdItemName { Id = "opgWIOPJ", ItemName = "Wood - Installation of Prefabricated I-Joists" });
            result.Add(new IdItemName { Id = "opgSSP", ItemName = "Soils - Site Preparation" });
            result.Add(new IdItemName { Id = "opgSFPIPD", ItemName = "Soils - Fill placement & In-Place Density" });
            result.Add(new IdItemName { Id = "opgSI", ItemName = "Soils - Investigations (Borings/Test Pits)" });
            result.Add(new IdItemName { Id = "opgPFDPI", ItemName = "Pile Foundations & Drilled Pier Installation" });
            result.Add(new IdItemName { Id = "opgPF", ItemName = "Pier Foundations" });
            result.Add(new IdItemName { Id = "opgUnderpinning", ItemName = "Underpinning" });
            result.Add(new IdItemName { Id = "opgWPCWV", ItemName = "Wall Panels, Curtain Walls, and Veneers" });
            result.Add(new IdItemName { Id = "opgSFRM", ItemName = "Sprayed Fire-Resistant Materials" });
            result.Add(new IdItemName { Id = "opgEIFS", ItemName = "Exterior Insulation Finish Systems (EIFS)" });
            result.Add(new IdItemName { Id = "opgAMBB", ItemName = "Alternative Materials - OTCR Buildings Bulletin #________" });
            result.Add(new IdItemName { Id = "opgSCS", ItemName = "Smoke Control Systems" });
            result.Add(new IdItemName { Id = "opgMS", ItemName = "Mechanical Systems" });
            result.Add(new IdItemName { Id = "opgFOSFOPS", ItemName = "Fuel-Oil Storage and Fuel-Oil Piping Systems" });
            result.Add(new IdItemName { Id = "opgHPSP", ItemName = "High-Pressure Steam Piping (Welding)" });
            result.Add(new IdItemName { Id = "opgFGP", ItemName = "Fuel-Gas Piping (Welding)" });
            result.Add(new IdItemName { Id = "opgSSSS", ItemName = "Structural Safety - Structural Stability" });
            result.Add(new IdItemName { Id = "opgMD", ItemName = "Mechanical Demolition" });
            result.Add(new IdItemName { Id = "opgESSB", ItemName = "Excavation - Sheeting, Shoring, and Bracing" });
            result.Add(new IdItemName { Id = "opgSPTD", ItemName = "Soil Percolation Test - Drywell" });
            result.Add(new IdItemName { Id = "opgRMOB", ItemName = "Raising and Moving of a Building" });
            result.Add(new IdItemName { Id = "opgSPTS", ItemName = "Soil Percolation Test - Septic" });
            result.Add(new IdItemName { Id = "opgSSDDDSI", ItemName = "Site Storm Drainage Disposal and Detention System Installation" });

            result.Add(new IdItemName { Id = "opgSSI", ItemName = "Septic System Installation" });
            result.Add(new IdItemName { Id = "opgSS", ItemName = "Sprinkler Systems" });
            result.Add(new IdItemName { Id = "opgSS2", ItemName = "Standpipe Systems" });
            result.Add(new IdItemName { Id = "opgHS", ItemName = "Heating Systems" });
            result.Add(new IdItemName { Id = "opgChimneys", ItemName = "Chimneys" });
            result.Add(new IdItemName { Id = "opgFDFS", ItemName = "Firestop, Draftstop, and Fireblock systems" });

            result.Add(new IdItemName { Id = "opgAW", ItemName = "Aluminum Welding" });
            result.Add(new IdItemName { Id = "opgSIS", ItemName = "Seismic Isolation Systems" });
            result.Add(new IdItemName { Id = "opgCTC", ItemName = "Concrete Test Cylinders" });
            result.Add(new IdItemName { Id = "opgCDM", ItemName = "Concrete Design Mix" });
            result.Add(new IdItemName { Id = "opgPreliminary", ItemName = "Preliminary" });
            result.Add(new IdItemName { Id = "opgFF", ItemName = "Footing and Foundation" });
            result.Add(new IdItemName { Id = "opgLFE", ItemName = "Lowest Floor Elevation (attach FEMA form)" });
            result.Add(new IdItemName { Id = "opgFI", ItemName = "Frame Inspection" });
            result.Add(new IdItemName { Id = "opgECCI", ItemName = "Energy Code Compliance Inspections" });
            result.Add(new IdItemName { Id = "opgFRRC", ItemName = "Fire-Resistance Rated Construction" });
            result.Add(new IdItemName { Id = "opgPAEL", ItemName = "Public Assembly Emergency Lighting" });
            result.Add(new IdItemName { Id = "opgFinal", ItemName = "Final*" });

            return result;
        }
        /// <summary>
        /// Gets the list of TR114 Inspection Type list.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns> Gets the list of TR114 Inspection Type list.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/TR114InspectionType/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetTR114InspectionType(int idJob, int idDocument = 0)
        {
            return Ok(GetTR114InspectionType());
        }

        public List<IdItemName> GetTR114InspectionType()
        {
            List<IdItemName> result = new List<IdItemName>();
            result.Add(new IdItemName { Id = "opgSSWL", ItemName = "Structural Steel – Welding" });
            result.Add(new IdItemName { Id = "opgSSDT", ItemName = "Structural Steel – Details " });
            result.Add(new IdItemName { Id = "opgSSHB", ItemName = "Structural Steel – High Strength Bolting" });
            result.Add(new IdItemName { Id = "opgSCFS", ItemName = "Structural Cold-Formed Steel" });
            result.Add(new IdItemName { Id = "opgCCIP", ItemName = "Concrete – Cast-In-Place" });
            result.Add(new IdItemName { Id = "opgCPC ", ItemName = "Concrete – Precast" });
            result.Add(new IdItemName { Id = "opgCPS ", ItemName = "Concrete – Prestressed" });
            result.Add(new IdItemName { Id = "opgMASN", ItemName = "Masonry" });
            result.Add(new IdItemName { Id = "opgWHLP", ItemName = "Wood – Installation of High-Load Diaphragms" });
            result.Add(new IdItemName { Id = "opgWMPT", ItemName = "Wood – Installation of Metal-Plate-Connected Trusses" });
            result.Add(new IdItemName { Id = "opgWPIJ", ItemName = "Wood – Installation of Prefabricated I-Joists" });
            result.Add(new IdItemName { Id = "opgSUBI", ItemName = "Subgrade Inspection" });
            result.Add(new IdItemName { Id = "opgSFPI", ItemName = "Subsurface Conditions – Fill Placement & In-Place Density" });
            result.Add(new IdItemName { Id = "opgSINV", ItemName = "Subsurface Investigations (Borings/Test Pits)" });
            result.Add(new IdItemName { Id = "opgDPFE", ItemName = "Deep Foundation Elements" });
            result.Add(new IdItemName { Id = "opgHLPL", ItemName = "Helical Piles (BB # 2014-020)" });
            result.Add(new IdItemName { Id = "opgVMFE", ItemName = "Vertical Masonry Foundation Elements" });
            result.Add(new IdItemName { Id = "opgWPCV", ItemName = "Wall Panels, Curtain Walls, and Veneers" });
            result.Add(new IdItemName { Id = "opgSFRM", ItemName = "Sprayed fire-resistant materials" });
            result.Add(new IdItemName { Id = "opgMIFC", ItemName = "Mastic and Intumescent Fire-resistant Coatings" });
            result.Add(new IdItemName { Id = "opgEIFS", ItemName = "Exterior Insulation and Finish Systems (EIFS)" });
            result.Add(new IdItemName { Id = "opgALTM", ItemName = "Alternative Materials  - OTCR Buildings Bulletin #" });
            result.Add(new IdItemName { Id = "opgSMKC", ItemName = "Smoke Control Systems" });
            result.Add(new IdItemName { Id = "opgMESY", ItemName = " Mechanical Systems" });
            result.Add(new IdItemName { Id = "opgFOSP", ItemName = "Fuel-Oil Storage and Fuel-Oil Piping Systems" });
            result.Add(new IdItemName { Id = "opgHPSP", ItemName = "High-Pressure Steam Piping (Welding)" });
            result.Add(new IdItemName { Id = "opgHTHW", ItemName = "High Temperature Hot Water Piping (Welding" });
            result.Add(new IdItemName { Id = "opgHPFG", ItemName = "High-Pressure Fuel-Gas Piping (Welding)" });
            result.Add(new IdItemName { Id = "opgSSEB", ItemName = "Structural Stability – Existing Buildings" });
            result.Add(new IdItemName { Id = "opgEXV ", ItemName = "Excavations—Sheeting, Shoring, and Bracing" });
            result.Add(new IdItemName { Id = "opgUNDP", ItemName = "Underpinning" });
            result.Add(new IdItemName { Id = "opgMDEM", ItemName = "Mechanical Demolition" });
            result.Add(new IdItemName { Id = "opgRAIS", ItemName = "Raising and Moving of a Building" });
            result.Add(new IdItemName { Id = "opgSLSW", ItemName = "Soil Percolation Test - Private On-Site Storm Water Drainage Disposal Systems,  and Detention Facilities" });
            result.Add(new IdItemName { Id = "opgPSDD", ItemName = "Private On-Site Storm Water Drainage Disposal  Systems, and Detention Facilities Installation" });
            result.Add(new IdItemName { Id = "opgIPSD", ItemName = "Individual On-Site Private Sewage Disposal Systems Installation" });
            result.Add(new IdItemName { Id = "opgSLSD", ItemName = "Soil Percolation Test - Individual On-Site Private Sewage Disposal Systems" });
            result.Add(new IdItemName { Id = "opgSPNK", ItemName = "Sprinkler Systems" });
            result.Add(new IdItemName { Id = "opgSTNP", ItemName = "Standpipe Systems" });
            result.Add(new IdItemName { Id = "opgHEAT", ItemName = "Heating Systems" });
            result.Add(new IdItemName { Id = "opgCHMN", ItemName = "Chimneys" });
            result.Add(new IdItemName { Id = "opgFRPJ", ItemName = "Firestop, Draftstop, and Fireblock systems" });

            result.Add(new IdItemName { Id = "opgALWL", ItemName = "Aluminum Welding" });
            result.Add(new IdItemName { Id = "opgFLDZ", ItemName = "Flood Zone Compliance  (attach FEMA elevation/dry floodproofing certificate where applicable)" });
            result.Add(new IdItemName { Id = "opgLEPM", ItemName = "Luminous Egress Path Markings" });
            result.Add(new IdItemName { Id = "opgESPS", ItemName = "Emergency and Standby Power Systems (Generators)" });
            result.Add(new IdItemName { Id = "opgPIAN", ItemName = "Post-installed Anchors (BB# 2014-018, 2014-019)" });
            result.Add(new IdItemName { Id = "opgSIS", ItemName = "Seismic Isolation Systems" });
            result.Add(new IdItemName { Id = "opgCDM", ItemName = "Concrete Design Mix" });
            result.Add(new IdItemName { Id = "opgCSAT", ItemName = "Concrete Sampling and Testing" });
            result.Add(new IdItemName { Id = "opgPREL", ItemName = "Preliminary" });
            result.Add(new IdItemName { Id = "opgFTFO", ItemName = "Footing and Foundation" });
            result.Add(new IdItemName { Id = "opgLWFE", ItemName = "Lowest Floor Elevation" });
            result.Add(new IdItemName { Id = "opgSTWF", ItemName = "Structural Wood Frame" });
            result.Add(new IdItemName { Id = "opgECCI", ItemName = "Energy Code Compliance Inspections" });
            result.Add(new IdItemName { Id = "opgFRRC", ItemName = "Fire-Resistance Rated Construction" });
            result.Add(new IdItemName { Id = "opgPAEL", ItemName = "Public Assembly Emergency Lighting" });
            result.Add(new IdItemName { Id = "opgFI", ItemName = "Final*" });

            return result;
        }
        /// <summary>
        /// Gets the list of job application for list.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the list of job application for list.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/JobApplicationFor/{idJob}/{idDocument}/{idParent}")]
        public string GetJobApplicationForDropdown(int idJob, int idDocument, int idParent = 0)
        {
            string applicationFor = string.Empty;

            if (idParent > 0)
            {
                JobApplication jobApplication = rpoContext.JobApplications.Include("JobApplicationType.Parent").FirstOrDefault(x => x.Id == idParent);

                if (jobApplication != null && jobApplication.JobApplicationType != null && jobApplication.JobApplicationType.IdParent > 0)
                {
                    switch ((ApplicationType)jobApplication.JobApplicationType.IdParent)
                    {
                        case ApplicationType.DOB:
                            applicationFor = (!string.IsNullOrEmpty(jobApplication.FloorWorking) ? "FL: " + jobApplication.FloorWorking : string.Empty);
                            applicationFor = applicationFor + (!string.IsNullOrEmpty(jobApplication.ApplicationFor) ? (!string.IsNullOrEmpty(applicationFor) ? " " : string.Empty) + jobApplication.ApplicationFor : string.Empty);
                            break;
                        case ApplicationType.DOT:
                            applicationFor = (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) ? "ON: " + jobApplication.StreetWorkingOn : string.Empty);
                            applicationFor = applicationFor + (!string.IsNullOrEmpty(jobApplication.StreetFrom) ? (!string.IsNullOrEmpty(applicationFor) ? " " : string.Empty) + "FROM: " + jobApplication.StreetFrom : string.Empty);
                            applicationFor = applicationFor + (!string.IsNullOrEmpty(jobApplication.StreetTo) ? (!string.IsNullOrEmpty(applicationFor) ? " " : string.Empty) + "TO: " + jobApplication.StreetTo : string.Empty);
                            applicationFor = applicationFor + (!string.IsNullOrEmpty(jobApplication.ApplicationFor) ? (!string.IsNullOrEmpty(applicationFor) ? " " : string.Empty) + jobApplication.ApplicationFor : string.Empty);
                            break;
                        case ApplicationType.DEP:
                            applicationFor = (!string.IsNullOrEmpty(jobApplication.StreetWorkingOn) ? "ON: " + jobApplication.StreetWorkingOn : string.Empty);
                            applicationFor = applicationFor + (!string.IsNullOrEmpty(jobApplication.StreetFrom) ? (!string.IsNullOrEmpty(applicationFor) ? " " : string.Empty) + "FROM: " + jobApplication.StreetFrom : string.Empty);
                            applicationFor = applicationFor + (!string.IsNullOrEmpty(jobApplication.StreetTo) ? (!string.IsNullOrEmpty(applicationFor) ? " " : string.Empty) + "TO: " + jobApplication.StreetTo : string.Empty);
                            applicationFor = applicationFor + (!string.IsNullOrEmpty(jobApplication.ModelNumber) ? (!string.IsNullOrEmpty(applicationFor) ? " " : string.Empty) + jobApplication.ModelNumber : string.Empty);
                            applicationFor = applicationFor + (!string.IsNullOrEmpty(jobApplication.ApplicationFor) ? (!string.IsNullOrEmpty(applicationFor) ? " " : string.Empty) + jobApplication.ApplicationFor : string.Empty);
                            break;
                    }
                }


            }


            return applicationFor;
        }
        /// <summary>
        /// Get the Reasons List
        /// </summary>
        /// <returns></returns>
        public List<IdItemName> GetVarianceReasons()
        {
            List<IdItemName> result = new List<IdItemName>();
            result.Add(new IdItemName { Id = "001", ItemName = "City Construction Project" });
            result.Add(new IdItemName { Id = "002", ItemName = "Construction Activities With Minimal Noise Impact" });
            result.Add(new IdItemName { Id = "003", ItemName = "Emergency Work" });
            result.Add(new IdItemName { Id = "004", ItemName = "Public Safety" });
            result.Add(new IdItemName { Id = "005", ItemName = "Undue Hardship" });

            return result;
        }
        /// <summary>
        /// Gets the list of VarianceReason for list.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the list of VarianceReason for list.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/VarianceReason/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetVarianceReason(int idJob, int idDocument = 0, int idParent = 0)
        {
            return Ok(GetVarianceReasons());
        }
        /// <summary>
        /// Gets the list of Company for list.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the list of Company for list.</returns>
        // [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/CompanyDropdown/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetCompanyDropdown(int idJob, int idDocument = 0, int idParent = 0)
        {
            var result = rpoContext.Companies.Select(c => new
            {
                Id = c.Id.ToString(),
                ItemName = c.Name,
                Name = c.Name,
            }).ToArray();

            return Ok(result);
        }
        /// <summary>
        /// Gets the list of Conacts for list.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns> Gets the list of Conacts for list.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobdocumentdrodown/ContactDropdown/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetContactDropdown(int idJob, int idDocument = 0, int idParent = 0)
        {
            if (idParent == -1)
            {
                var result = rpoContext.Contacts
                    .Where(c => c.IdCompany == null).AsEnumerable()
                    .Select(c => new
                    {
                        Id = c.Id.ToString(),
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
                Company company = rpoContext.Companies.Find(idParent);
                if (company == null)
                {
                    return this.NotFound();
                }

                var result = rpoContext.Contacts
                    .Where(c => c.IdCompany == company.Id).AsEnumerable()
                    .Select(c => new
                    {
                        Id = c.Id.ToString(),
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
        /// Gets the list of JobNYCDOTApplicationNumberType for list.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the list of JobNYCDOTApplicationNumberType for list.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/JobNYCDOTApplicationNumberType/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetJobNYCDOTApplicationNumberType(int idJob, int idDocument, int idParent = 0)
        {
            var result = rpoContext.JobApplications.Include("JobApplicationType").Where(c => c.IdJob == idJob && c.JobApplicationType.Description.Equals("NYCDOT")).Select(x => new IdItemName
            {
                Id = x.Id.ToString(),
                ItemName = ((x.JobApplicationType != null && x.JobApplicationType.Description.Equals("NYCDOT")) ? x.JobApplicationType.Description
                + (x.StreetWorkingOn != null ? " - " + x.StreetWorkingOn : string.Empty)
                + (x.StreetFrom != null ? " | " + x.StreetFrom : string.Empty)
                + (x.StreetTo != null ? " | " + x.StreetTo : string.Empty) : string.Empty)
            }).ToArray();

            return Ok(result);
        }
        //ML
        /// <summary>
        /// Gets the list of JobNYCDOTWorkPermit for list.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the list of JobNYCDOTWorkPermit for list.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/JobNYCDOTWorkPermit/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetJobNYCDOTWorkPermitDropdown(int idJob, int idDocument, int idParent = 0)
        {
            var result = rpoContext.JobWorkTypes.Include("JobApplicationType")
                .Where(c => c.JobApplicationType.Description.Equals("NYCDOT")).Select(x => new IdItemName
                {
                    Id = x.Id.ToString(),
                    ItemName = x != null ? x.Description + (x.Code != null ? " - " + x.Code : string.Empty) : string.Empty
                }).ToArray();

            return Ok(result);
        }

        //ML
        /// <summary>
        /// Gets the list of M11CMWorkType for list.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns> Gets the list of M11CMWorkType for list.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/M11CMWorkType/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetM11CMWorkType(int idJob, int idDocument = 0)
        {
            return Ok(GetM11CMWorkType());
        }

        public List<IdItemName> GetM11CMWorkType()
        {
            List<IdItemName> result = new List<IdItemName>();
            result.Add(new IdItemName { Id = "opgMIC", ItemName = "MAJOR INSTALLATION CABLE – 110" });
            result.Add(new IdItemName { Id = "opgMIS", ItemName = "MAJOR INSTALLATION SEWER – 111 " });
            result.Add(new IdItemName { Id = "opgRTC", ItemName = "RAPID TRANSIT CONSTRUCTION – 112" });
            result.Add(new IdItemName { Id = "opgRWC", ItemName = "REPAIR WATER Cold - 113" });
            result.Add(new IdItemName { Id = "opgRS", ItemName = "REPAIR SEWER – 114" });
            result.Add(new IdItemName { Id = "opgFOL", ItemName = "FUEL OIL LINE – 116" });
            result.Add(new IdItemName { Id = "opgRRRC ", ItemName = "RESET, REPAIR OR REPLACE CURB – 118" });
            result.Add(new IdItemName { Id = "opgPS", ItemName = "PAVE STREET - 119" });
            result.Add(new IdItemName { Id = "opgPT", ItemName = "TREE PIT – 120" });
            result.Add(new IdItemName { Id = "opgCAMC", ItemName = "CONSTRUCT OR ALTER MANHOLE AND / OR CASTING – 121" });
            result.Add(new IdItemName { Id = "opgTPCB", ItemName = "TEST PITS, CORES OR BORINGS – 126" });
            result.Add(new IdItemName { Id = "opgIF", ItemName = "INSTALL FENCE - 132" });
            result.Add(new IdItemName { Id = "opgITS", ItemName = "INSTALL TRAFFIC SIGNALS – 133" });
            result.Add(new IdItemName { Id = "opgFR", ItemName = "FINAL RESTORATION - 135" });
            result.Add(new IdItemName { Id = "opgDCMIW", ItemName = "DEP CONTRACTOR MAJOR INSTALLATIONS-WATER - 136" });
            result.Add(new IdItemName { Id = "opgDCMIS", ItemName = "DEP CONTRACTOR MAJOR INSTALLATIONS-SEWER - 137" });
            result.Add(new IdItemName { Id = "opgRTS", ItemName = "REPAIR TRAFFIC SIGNALS - 157" });
            result.Add(new IdItemName { Id = "opgDCMR", ItemName = "DDC CONTRACTOR MAJOR RECONSTRUCTION - 158" });
            result.Add(new IdItemName { Id = "opgECMR", ItemName = "EDC CONTRACTOR MAJOR RECONSTRUCTION - 159" });
            result.Add(new IdItemName { Id = "opgSRC", ItemName = "SIDEWALK RECONSTRUCTION CONTRACTS - 160" });
            result.Add(new IdItemName { Id = "opgNBR", ItemName = "NYCDOT BRIDGES RECONSTRUCTION - 161" });
            result.Add(new IdItemName { Id = "opgNPRC", ItemName = "NYC PARKS RECONSTRUCTION CONTRACT - 162" });
            result.Add(new IdItemName { Id = "opgSCW", ItemName = "SCA CONTRACT WORK - 163" });
            result.Add(new IdItemName { Id = "opgNC", ItemName = "NYSDOT CONSTRUCTION - 164" });
            return result;
        }
        //ML
        /// <summary>
        /// Gets the list of M11CMWorkType2 for list.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the list of M11CMWorkType2 for list.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/M11CMWorkType2/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetM11CMWorkType2(int idJob, int idDocument = 0)
        {
            return Ok(GetM11CMWorkType2());
        }

        public List<IdItemName> GetM11CMWorkType2()
        {
            List<IdItemName> result = new List<IdItemName>();
            result.Add(new IdItemName { Id = "opgPMS_201", ItemName = "201 - PLACE MATERIAL ON STREET" });
            result.Add(new IdItemName { Id = "opgMSS_201A", ItemName = "201A - Material on street & sidewalk" });
            result.Add(new IdItemName { Id = "opgMS_201D", ItemName = "201D - Material on sidewalk" });
            result.Add(new IdItemName { Id = "opgMS2_201T", ItemName = "201T - Material on street" });
            result.Add(new IdItemName { Id = "opgCS_202", ItemName = "202 - CROSSING SIDEWALK" });
            result.Add(new IdItemName { Id = "opgPCSS_203", ItemName = "203 - PLACE CRANE OR SHOVEL ON STREET" });
            result.Add(new IdItemName { Id = "opgPCSS_203A", ItemName = "203A - Place Crane or Shovel on Street - Assist" });
            result.Add(new IdItemName { Id = "opgPEOTCS_204", ItemName = "204 - PLACE EQUIPMENT OTHER THAN CRANE OR SHOVEL ON STREET" });
            result.Add(new IdItemName { Id = "opgPSTS_205", ItemName = "205 - PLACE SHANTY OR TRAILER ON STREET" });
            result.Add(new IdItemName { Id = "opgTPW_208", ItemName = "208 - TEMPORARY PEDESTRIAN WALKWAY" });
            result.Add(new IdItemName { Id = "opgORS_211", ItemName = "211 - OCCUPANCY OF ROADWAY AS STIPULATED" });
            result.Add(new IdItemName { Id = "opgORS_211F", ItemName = "2011F - Full Roadway Closing" });
            result.Add(new IdItemName { Id = "opgPCS_214", ItemName = "214 - PLACE CONTAINER ON STREET" });
            result.Add(new IdItemName { Id = "opgPCS_214R", ItemName = "214R - Regular Container on street" });
            result.Add(new IdItemName { Id = "opgPCS_214M", ItemName = "214M - Mini Container on street" });
            result.Add(new IdItemName { Id = "opgPCS_214M2", ItemName = "214M - Mini Container on sidewalk" });
            result.Add(new IdItemName { Id = "opgPCS_214R2", ItemName = "214R- Regular Container on sidewalk" });
            result.Add(new IdItemName { Id = "opgPCS_214E", ItemName = "214E - Embargo Waiver" });
            result.Add(new IdItemName { Id = "opgOSS_215", ItemName = "215 - OCCUPANCY OF SIDEWALK AS STIPULATED" });
            result.Add(new IdItemName { Id = "opgOSS_215A", ItemName = "215A - Occupancy of sw/exc/mining/lining of vt" });
            result.Add(new IdItemName { Id = "opgTCS_221", ItemName = "221 - TEMPORARY CONSTRUCTION SIGN/MARKINGS" });
            result.Add(new IdItemName { Id = "opgCMS_204X", ItemName = "204X - Concrete Mixer on street" });
            result.Add(new IdItemName { Id = "opgBS_204O", ItemName = "204O - Bobcat on street" });
            result.Add(new IdItemName { Id = "opgGS_204G", ItemName = "204G - Generator on street" });
            result.Add(new IdItemName { Id = "opgCS_204C", ItemName = "204C - Compressor on street" });
            result.Add(new IdItemName { Id = "opgJBS_204J", ItemName = "204J - Jersey Barriers on street" });
            result.Add(new IdItemName { Id = "opgBMS_204N", ItemName = "204N - Bending Machine on street" });
            result.Add(new IdItemName { Id = "opgTS_204T", ItemName = "204T - Timbers on street" });
            result.Add(new IdItemName { Id = "opgHPS_204H", ItemName = "204H - Hoist / Platform on street" });
            result.Add(new IdItemName { Id = "opgMFS_204F", ItemName = "204F - Maintain Fence on street" });
            result.Add(new IdItemName { Id = "opgJBSS_204S", ItemName = "204S - Jersey Barriers on street & sidewalk" });
            result.Add(new IdItemName { Id = "opgPS_204P", ItemName = "204P - Portosans on street" });
            result.Add(new IdItemName { Id = "opgMS_204M", ItemName = "204M - Manlift on street" });
            result.Add(new IdItemName { Id = "opgCPS_204U", ItemName = "204U - Concrete Pump on street" });
            result.Add(new IdItemName { Id = "opgTCEG_204E", ItemName = "204E - Temp Chiller & Emergency Generator (st)" });
            result.Add(new IdItemName { Id = "opgBS_204B", ItemName = "204B - Backhoe on street" });
            result.Add(new IdItemName { Id = "opgBS2_204b", ItemName = "204b - Backhoe on sidewalk" });
            result.Add(new IdItemName { Id = "opgCS2_204C", ItemName = "204C - Compressor on sidewalk" });
            result.Add(new IdItemName { Id = "opgTCEG2_204e", ItemName = "204e - Temp Chiller & Emergency Generator (swk)" });
            result.Add(new IdItemName { Id = "opgGS2_204g", ItemName = "204g - Generator on sidewalk" });
            result.Add(new IdItemName { Id = "opgHPS2_204h", ItemName = "204h - Hoist / Platform on sidewalk" });
            result.Add(new IdItemName { Id = "opgJBS2_204j", ItemName = "204j - Jersey Barriers on sidewalk" });
            result.Add(new IdItemName { Id = "opgMS2_204m", ItemName = "204m - Manlift on sidewalk" });
            result.Add(new IdItemName { Id = "opgBMS2_204n", ItemName = "204n - Bending Machine on sidewalk" });
            result.Add(new IdItemName { Id = "opgBS2_204o", ItemName = "204o - Bobcat on sidewalk" });
            result.Add(new IdItemName { Id = "opgPS2_204p", ItemName = "204p - Portosans on sidewalk" });
            result.Add(new IdItemName { Id = "opgSTSS_204s", ItemName = "204s - Shanty or trailer on street & sidewalk" });
            result.Add(new IdItemName { Id = "opgTS2_204t", ItemName = "204t - Timbers on sidewalk" });
            result.Add(new IdItemName { Id = "opgCPS2_204u", ItemName = "204u - Concrete Pump on sidewalk" });
            result.Add(new IdItemName { Id = "opgCMS2_204x", ItemName = "204x - Concrete Mixer on sidewalk" });
            result.Add(new IdItemName { Id = "opgMFS2_204f", ItemName = "204f - Maintain Fence on sidewalk" });
            result.Add(new IdItemName { Id = "opgES_204Z", ItemName = "204Z - Excavator on Street" });
            result.Add(new IdItemName { Id = "opgES2_204z", ItemName = "204z - Excavator on Sidewalk" });
            result.Add(new IdItemName { Id = "opgSCSS_204R", ItemName = "204R - Storage containers on street & sidewalk" });
            result.Add(new IdItemName { Id = "opgCBSS_204r", ItemName = "204r - Concrete barriers on street & sidewalk" });
            result.Add(new IdItemName { Id = "opgPDS_204Q", ItemName = "204Q - Pile Driver on Street" });
            result.Add(new IdItemName { Id = "opgPDS2_204q", ItemName = "204q - Pile Driver on Sidewalk" });
            result.Add(new IdItemName { Id = "opgS_204a", ItemName = "204a - Shed" });
            result.Add(new IdItemName { Id = "opgGBS_204k", ItemName = "204k - Guard Booth on Sidewalk" });
            result.Add(new IdItemName { Id = "opgWM_204w", ItemName = "204w - Welding Machine" });
            result.Add(new IdItemName { Id = "opgHP_204i", ItemName = "204i - Hydraulic Pump" });
            result.Add(new IdItemName { Id = "opgV_204#", ItemName = "204# - Van" });
            result.Add(new IdItemName { Id = "opgFSS_204@", ItemName = "204@ - Forklift on Street-Sidewalk" });
            result.Add(new IdItemName { Id = "opgTS_204+", ItemName = "204+ - Trailer on Street" });
            result.Add(new IdItemName { Id = "opgDT_204I", ItemName = "204I - Delivery Truck" });
            result.Add(new IdItemName { Id = "opgET_204A", ItemName = "204A - Electric Transformer" });
            result.Add(new IdItemName { Id = "opgPBRS_204=", ItemName = "204= - Plastic barricades on roadway & sidewalk" });
            result.Add(new IdItemName { Id = "opgFJB_204*", ItemName = "204* - Fence on Jersey Barriers" });
            result.Add(new IdItemName { Id = "opgNTW_204!", ItemName = "204! - New track work" });
            result.Add(new IdItemName { Id = "opgFS_204$", ItemName = "204$ - Fence on Street" });
            result.Add(new IdItemName { Id = "opgWBS_204%", ItemName = "204% - Washout Box on Street" });
            result.Add(new IdItemName { Id = "opgBESS_204Y", ItemName = "204Y - Backhoe/Excavator on street & sidewalk" });
            result.Add(new IdItemName { Id = "opgSCR_204V", ItemName = "204V - Storage container on roadway" });
            result.Add(new IdItemName { Id = "opgMCSS_204W", ItemName = "204W - Mini Containers on street & sidewalk" });
            result.Add(new IdItemName { Id = "opgTS_204^", ItemName = "204^ - Trailer on sidewalk" });
            result.Add(new IdItemName { Id = "opgPESS_204;", ItemName = "204; - Place equipment  on street or sidewalk" });
            result.Add(new IdItemName { Id = "opgTTC_204/", ItemName = "204/ - Two toilets/compressor" });
            result.Add(new IdItemName { Id = "opgVMS_204VMS", ItemName = "204V, - VMS Board" });
            result.Add(new IdItemName { Id = "opgHPRS_204v", ItemName = "204v - Hoist/Platform on roadway & sidewalk" });
            result.Add(new IdItemName { Id = "opgBS3_204}", ItemName = "204} - Boomtruck on sidewalk" });
            result.Add(new IdItemName { Id = "opgBR3_204{", ItemName = "204{ - Boomtruck on roadway" });
            result.Add(new IdItemName { Id = "opgCRS3_204|", ItemName = "204| - Compressor on roadway & sidewalk" });
            result.Add(new IdItemName { Id = "opgEGRS_204]", ItemName = "204] - Escavator & Generator on roadway & sidew" });
            result.Add(new IdItemName { Id = "opgCBSS_204<", ItemName = "204< - Connex box on street or sidewalk" });
            result.Add(new IdItemName { Id = "opgMJBS_204//", ItemName = "204// - Manlift & Jersey Barriers on street or s" });
            result.Add(new IdItemName { Id = "opgSCS_204=", ItemName = "204_ - Shipping Container on Street" });
            result.Add(new IdItemName { Id = "opgBB_204-", ItemName = "204- - Bobcat & backhoe on st or sw" });
            result.Add(new IdItemName { Id = "opgDS3_204'", ItemName = "204` - Dumpster on Street" });
            result.Add(new IdItemName { Id = "opgBSQT_204~", ItemName = "204~ - Bobcat/Steel/Q-deck/Tiles" });
            result.Add(new IdItemName { Id = "opgJBMF_204)", ItemName = "204) - Jersey barriers, maintain fence" });
            result.Add(new IdItemName { Id = "opgPEOCS_204CS", ItemName = "204CS; - Place equipment other than crane or shov" });
            result.Add(new IdItemName { Id = "opgB3_204::", ItemName = "204:: - Barricades" });
            result.Add(new IdItemName { Id = "opgPER_204(%", ItemName = "204( - Place Equipment - Rehab" });
            result.Add(new IdItemName { Id = "opgFC_204d", ItemName = "204d - Fence on curb" });
            result.Add(new IdItemName { Id = "opgCBS_204D", ItemName = "204D - Concrete Barriers on Street" });
            result.Add(new IdItemName { Id = "opgLTS_204LS", ItemName = "204LS - Light Tower on Street" });
            result.Add(new IdItemName { Id = "opgGPS_204GS", ItemName = "204GS - Grout Pump on Street" });
            result.Add(new IdItemName { Id = "opgST_204I", ItemName = "204I - Support Truck" });
            result.Add(new IdItemName { Id = "opgTHS_204TW", ItemName = "24TW - Temporary Heaters on Sidewalk" });
            result.Add(new IdItemName { Id = "opgCB_204CB", ItemName = "24CB - Concrete Bucket" });
            result.Add(new IdItemName { Id = "opgGBS_204K", ItemName = "24K - Guard Booth on Street" });
            result.Add(new IdItemName { Id = "opgRS_204RS", ItemName = "24RS - Rolling Scaffold" });
            result.Add(new IdItemName { Id = "opgTBS_204TB", ItemName = "24TB - Tool Box on Swk" });
            result.Add(new IdItemName { Id = "opgAB_204AB", ItemName = "24AB - Arrowboard" });
            result.Add(new IdItemName { Id = "opgL2_204L", ItemName = "24L - Lull" });
            result.Add(new IdItemName { Id = "opgCX_204CX", ItemName = "24CX - Cement Mixer" });
            result.Add(new IdItemName { Id = "opgMMS_204MM", ItemName = "24MM - Mortar Mixer on Street" });
            result.Add(new IdItemName { Id = "opgMSS_204[", ItemName = "24[ - Manlift on Street & Sidewalk" });
            result.Add(new IdItemName { Id = "opgWTS_204WT", ItemName = "24WT - Water Tank on Street" });
            result.Add(new IdItemName { Id = "opgGBSS_204G", ItemName = "24G - Guard Booth on Street or Sidewalk" });
            result.Add(new IdItemName { Id = "opgCB_204B", ItemName = "24B - Concrete Buggy" });
            result.Add(new IdItemName { Id = "opgPBR_204P", ItemName = "24P - Plastic Barricades on Roadway" });
            result.Add(new IdItemName { Id = "opgPTY_204PR", ItemName = "24PR - Pump Truck on Roadway" });
            result.Add(new IdItemName { Id = "opgPYB_204Y", ItemName = "24Y - Place Yodock Barriers" });
            result.Add(new IdItemName { Id = "opgPBS_204S", ItemName = "24S - Plastic Barricades on Sidewalk" });
            result.Add(new IdItemName { Id = "opgFB_204F", ItemName = "24F - Fence on Barricade" });
            result.Add(new IdItemName { Id = "opgSB_204)", ItemName = "24) - Sidewalk Bridge" });
            result.Add(new IdItemName { Id = "opgBT_204%", ItemName = "24% - Bucket Truck" });
            result.Add(new IdItemName { Id = "opgFT_204^", ItemName = "24^ - Flatbed Truck" });
            result.Add(new IdItemName { Id = "opgTB_204&", ItemName = "24& - Trench Boxes" });
            result.Add(new IdItemName { Id = "opgSB_204C", ItemName = "24( - Storage Box" });
            result.Add(new IdItemName { Id = "opgSA_204SA", ItemName = "24_ - Storage Area" });
            result.Add(new IdItemName { Id = "opgME_204Me", ItemName = "24Me - Mini Excavator" });
            result.Add(new IdItemName { Id = "opgSC_204SC", ItemName = "24SC - Storage Container" });
            result.Add(new IdItemName { Id = "opgFC_204FC", ItemName = "24FC - Fuel Cages" });
            result.Add(new IdItemName { Id = "opgDP_204DW", ItemName = "24DW - Dewatering Pump" });
            result.Add(new IdItemName { Id = "opgCLF_204CL", ItemName = "24CL - Chain Link Fence" });
            result.Add(new IdItemName { Id = "opgTC_204TC", ItemName = "24TC - Turnstile Counter" });
            result.Add(new IdItemName { Id = "opgATA_204AT", ItemName = "24AT - Attenuator Truck with Arrow" });
            result.Add(new IdItemName { Id = "opgFT_204FT", ItemName = "24FT - Fuel Tank" });
            result.Add(new IdItemName { Id = "opgTESS_204ES", ItemName = "24ES - Temporary Electrical Shed" });
            result.Add(new IdItemName { Id = "opgBR_204BR", ItemName = "24BR - Barrels" });
            result.Add(new IdItemName { Id = "opgCT_204CT", ItemName = "24CT - Concrete Truck" });
            result.Add(new IdItemName { Id = "opgCB_204Cx", ItemName = "24Cx - Connex Box" });
            result.Add(new IdItemName { Id = "opgMO_204MO", ItemName = "24MO - Mobile Office" });
            result.Add(new IdItemName { Id = "opgBL_204BL", ItemName = "24BL - Boom Lift" });
            result.Add(new IdItemName { Id = "opgBR_204Br", ItemName = "24Br - Barriers" });
            result.Add(new IdItemName { Id = "opgOS_204OS", ItemName = "24OS - Odor Supression Tank" });
            result.Add(new IdItemName { Id = "opgAP_204As", ItemName = "24As - Asphalt Paver" });
            result.Add(new IdItemName { Id = "opgIR_204In", ItemName = "24In - Ingersoll Rand" });
            result.Add(new IdItemName { Id = "opgTS_204Ts", ItemName = "24Ts - Trailer Steps" });
            result.Add(new IdItemName { Id = "opgST_204ST", ItemName = "24ST - Settling Tank" });
            result.Add(new IdItemName { Id = "opgSP_204SP", ItemName = "24SP - Steel Plate" });
            result.Add(new IdItemName { Id = "opgSMSG_2044", ItemName = "2044 - Mud Sucker & Small Generator" });
            result.Add(new IdItemName { Id = "opgLD_2041", ItemName = "2041 - Loading Dock" });
            result.Add(new IdItemName { Id = "opgJH_2042", ItemName = "2042 - Jack Hammer" });
            result.Add(new IdItemName { Id = "opgSP_2047", ItemName = "2047 - Dump Truck" });
            result.Add(new IdItemName { Id = "opgDR_2048", ItemName = "2048 - Drill Rig" });
            result.Add(new IdItemName { Id = "opgGBMH_2040", ItemName = "2040 - Guard Booth/Manlift/Hoist" });
            result.Add(new IdItemName { Id = "opgTMB_2043", ItemName = "2043 - Toilets/manlift/barriers on st or sw" });
            result.Add(new IdItemName { Id = "opgRTE_2045", ItemName = "2045 - RT Excavator w bucket (2)/Bobcat/Dump Tr" });
            result.Add(new IdItemName { Id = "opgBES_2049", ItemName = "2049 - Backhoe/Excavator on street" });
            result.Add(new IdItemName { Id = "opgPSS_2046", ItemName = "2046 - Portosans on Street & Sidewalk" });

            return result;
        }
        //[Authorize]
        //[RpoAuthorize]
        //[HttpGet]
        //[Route("api/jobdocumentdrodown/VARPMTJobApplicationNumberType/{idJob}/{idDocument}/{idParent}")]
        //public IHttpActionResult GetVARPMTJobApplicationNumberTypeDropdown(int idJob, int idDocument = 0, int idParent = 0)
        //{
        //    // int IdVARPMTApplicationType = Document.AfterHoursPermitApplicationPW517.GetHashCode();

        //    // List<int> jobDocument = rpoContext.JobDocuments.Where(x => x.IdJob == idJob && x.IdDocument == IdVARPMTApplicationType).Select(x => x.Id).ToList();


        //    //var result = rpoContext.JobDocuments.Include("JobApplicationType").Where(x => x.IdJob == idJob && jobDocument.Contains(x.Id))
        //    //             .Select(x => new IdItemName
        //    //                    {
        //    //                        Id = x.Id.ToString(),
        //    //                        ItemName =((x.JobApplication.ApplicationNumber != null && x.JobApplication.ApplicationNumber != string.Empty) ? x.JobApplication.ApplicationNumber + " " : string.Empty) + (x.JobApplication.JobApplicationType != null ? x.JobApplication.JobApplicationType.Description : string.Empty)
        //    //                    }).ToArray();

        //    // return Ok(result);

        //    var result = rpoContext.JobApplications.Include("JobApplicationType").Where(c => c.IdJob == idJob
        //    && jobDocuments.Contains(c.) c.JobApplicationType.IdParent == IdVARPMTApplicationType).Select(x => new IdItemName
        //    {
        //        Id = x.Id.ToString(),
        //        ItemName = ((x.ApplicationNumber != null && x.ApplicationNumber != string.Empty) ? x.ApplicationNumber + " " : string.Empty) + (x.JobApplicationType != null ? x.JobApplicationType.Description : string.Empty)
        //    }).ToArray();

        //    idDocument = Document.VarianceAfterHoursPermit_VARPMT.GetHashCode();
        //    List<int> jobDocument = rpoContext.JobDocuments.Where(x => x.IdJob == idJob && x.IdDocument == idDocument).Select(x => x.Id).ToList();
        //    //int tenDaysNotice_Recipient = DocumentPlaceHolderField.TenDaysNotice_Recipient.GetHashCode();

        //    List<IdItemName> idItemNameList = rpoContext.JobDocumentFields
        //        .Where(x => jobDocument.Contains(x.IdJobDocument))
        //        .AsEnumerable()
        //        .Select(x => VARPMTVarianceAfterHoursPermit(x)).Distinct().ToList();
        //    return Ok(idItemNameList);

        //    //return Ok(result);
        //}
        /// <summary>
        /// Gets the list of VARPMTJobApplicationNumberType for list.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the list of VARPMTJobApplicationNumberType for list.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/VARPMTJobApplicationNumberType/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetVARPMTJobApplicationNumberTypeDropdown(int idJob, int idDocument = 0)
        {
            idDocument = Document.AfterHoursPermitApplicationPW517.GetHashCode();
            List<int> jobDocument = rpoContext.JobDocuments.Where(x => x.IdJob == idJob && x.IdDocument == idDocument).Select(x => x.Id).ToList();


            List<IdItemName> idItemNameList = rpoContext.JobDocumentFields.Include("JobDocument")
                .Where(x => jobDocument.Contains(x.IdJobDocument) && (x.DocumentField.Id == 542589))
                .AsEnumerable()
                .Select(x => VARPMTJobApplicationNumber(x)).Distinct().ToList();
            return Ok(idItemNameList);


        }
        /// <summary>
        /// Gets the list of LIC6documentType for list.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns> Gets the list of LIC6documentType for list.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/LIC6documentType/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetLIC6documentTypeDropdown(int idJob, int idDocument = 0)
        {
            //idDocument = Document.GeneralContractorRegistration.GetHashCode();
            //List<int> jobDocument = rpoContext.JobDocuments.Where(x => x.IdJob == idJob && x.IdDocument == idDocument).Select(x => x.Id).ToList();


            //List<IdItemName> idItemNameList = rpoContext.JobDocumentFields.Include("JobDocument")
            //    .Where(x => jobDocument.Contains(x.IdJobDocument) && (x.DocumentField.Id == 521507))
            //    .AsEnumerable()
            //    .Select(x => VARPMTJobApplicationNumber(x)).Distinct().ToList();
            //return Ok(idItemNameList);

            idDocument = Document.GeneralContractorRegistration.GetHashCode();
            List<int> jobDocument = rpoContext.JobDocuments.Where(x => x.IdJob == idJob && x.IdDocument == idDocument).Select(x => x.Id).ToList();
            int tenDaysNotice_Recipient = DocumentPlaceHolderField.TenDaysNotice_Recipient.GetHashCode();

            List<IdItemName> idItemNameList1 = new List<IdItemName>();

            foreach (var item in jobDocument)
            {
                int test = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == item).Select(x => x.IdDocumentField).FirstOrDefault();
                string actualvalue = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == item && x.IdDocumentField == test).Select(x => x.Value).FirstOrDefault();
                int value = Convert.ToInt32(actualvalue);
                int? contact = rpoContext.JobContacts.Where(x => x.Id == value).Select(x => x.IdContact).FirstOrDefault();
                Contact con = rpoContext.Contacts.Where(x => x.Id == contact).Select(x => x).FirstOrDefault();
                string fullName = con != null ? con.FirstName + " " + con.LastName : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                obj.ItemName = "LIC6-" + fullName;
                idItemNameList1.Add(obj);


            }


            //List<IdItemName> idItemNameList = rpoContext.JobDocumentFields
            //    //   .Where(x => jobDocument.Contains(x.IdJobDocument) && x.DocumentField.Id == tenDaysNotice_Recipient)
            //    .Where(x => jobDocument.Contains(x.IdJobDocument) && x.DocumentField.Field.FieldName == "Last Name")
            //    .AsEnumerable()
            //    .Select(x => LIC6List(x)).Distinct().ToList();
            return Ok(idItemNameList1);



        }
        //     rpoContext.JobContacts.Where(x => x.Id == Certifierid).FirstOrDefault()

        private IdItemName LIC6List(JobDocumentField jobDocumentField)
        {
            //int tenDaysNotice_AddressAdjacent = DocumentPlaceHolderField.TenDaysNotice_AddressAdjacent.GetHashCode();
            string itemname = jobDocumentField.Value.Split('\n') != null && jobDocumentField.Value.Split('\n').Count() > 0 ? jobDocumentField.Value.Split('\n')[0] : string.Empty;
            // JobDocumentField adjacentJobDocumentField = rpoContext.JobDocumentFields.FirstOrDefault(x => x.IdJobDocument == jobDocumentField.IdJobDocument && x.DocumentField.Id == tenDaysNotice_AddressAdjacent);
            //    string adjacentaddress = adjacentJobDocumentField != null ? adjacentJobDocumentField.ActualValue : string.Empty;

            int Certifierid = Convert.ToInt32(jobDocumentField.Value);

            JobContact jobContact = rpoContext.JobContacts.Where(x => x.Id == Certifierid).FirstOrDefault();

            string fullName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;


            return new IdItemName
            {
                Id = jobDocumentField.Value,
                // ItemName = itemname + (adjacentaddress != null && adjacentaddress != "" ? " (" + adjacentaddress + ")" : string.Empty)
                ItemName = "LIC6-" + fullName
            };
        }
        /// <summary>
        /// Gets the list of LIC7documentType for list.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the list of LIC7documentType for list.</returns>

        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/LIC7documentType/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetLIC7documentTypeDropdown(int idJob, int idDocument = 0)
        {
            idDocument = Document.SafetyRegistration.GetHashCode();
            List<int> jobDocument = rpoContext.JobDocuments.Where(x => x.IdJob == idJob && x.IdDocument == idDocument).Select(x => x.Id).ToList();


            List<IdItemName> idItemNameList1 = new List<IdItemName>();

            foreach (var item in jobDocument)
            {
                int test = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == item).Select(x => x.IdDocumentField).FirstOrDefault();
                string actualvalue = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == item && x.IdDocumentField == test).Select(x => x.Value).FirstOrDefault();
                int value = Convert.ToInt32(actualvalue);
                int? contact = rpoContext.JobContacts.Where(x => x.Id == value).Select(x => x.IdContact).FirstOrDefault();
                Contact con = rpoContext.Contacts.Where(x => x.Id == contact).Select(x => x).FirstOrDefault();
                string fullName = con != null ? con.FirstName + " " + con.LastName : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                obj.ItemName = "LIC7-" + fullName;
                idItemNameList1.Add(obj);


            }

            return Ok(idItemNameList1);


            //List<IdItemName> idItemNameList = rpoContext.JobDocumentFields.Include("JobDocument")
            //    .Where(x => jobDocument.Contains(x.IdJobDocument) && (x.DocumentField.Field.FieldName == "Applicant"))
            //    .AsEnumerable()
            //    .Select(x => LIC7List(x)).Distinct().ToList();
            //return Ok(idItemNameList);


        }


        private IdItemName LIC7List(JobDocumentField jobDocumentField)
        {
            //int tenDaysNotice_AddressAdjacent = DocumentPlaceHolderField.TenDaysNotice_AddressAdjacent.GetHashCode();
            string itemname = jobDocumentField.Value.Split('\n') != null && jobDocumentField.Value.Split('\n').Count() > 0 ? jobDocumentField.Value.Split('\n')[0] : string.Empty;
            // JobDocumentField adjacentJobDocumentField = rpoContext.JobDocumentFields.FirstOrDefault(x => x.IdJobDocument == jobDocumentField.IdJobDocument && x.DocumentField.Id == tenDaysNotice_AddressAdjacent);
            //    string adjacentaddress = adjacentJobDocumentField != null ? adjacentJobDocumentField.ActualValue : string.Empty;

            int Certifierid = Convert.ToInt32(jobDocumentField.Value);

            JobContact jobContact = rpoContext.JobContacts.Where(x => x.Id == Certifierid).FirstOrDefault();

            string fullName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;


            return new IdItemName
            {
                Id = jobDocumentField.Value,
                // ItemName = itemname + (adjacentaddress != null && adjacentaddress != "" ? " (" + adjacentaddress + ")" : string.Empty)
                ItemName = "LIC7-" + fullName
            };
        }



        /// <summary>
        /// Gets the list of BINTAKdocumentType for list.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the list of BINTAKdocumentType for list.</returns>

        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/BINTAKdocumentType/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetBINTAKTypeDropdown(int idJob, int idDocument = 0)
        {
            //idDocument = Document.GeneralContractorRegistration.GetHashCode();
            //List<int> jobDocument = rpoContext.JobDocuments.Where(x => x.IdJob == idJob && x.IdDocument == idDocument).Select(x => x.Id).ToList();


            //List<IdItemName> idItemNameList = rpoContext.JobDocumentFields.Include("JobDocument")
            //    .Where(x => jobDocument.Contains(x.IdJobDocument) && (x.DocumentField.Id == 521507))
            //    .AsEnumerable()
            //    .Select(x => VARPMTJobApplicationNumber(x)).Distinct().ToList();
            //return Ok(idItemNameList);
            //idDocument = Document.BoroughIntakeForm.GetHashCode();
            idDocument = Document.BoroughIntakeForm2.GetHashCode();
            int PW_108 = Document.Planorworkapprovalapplication2019.GetHashCode();
            int PW_144 = Document.Application2014Present.GetHashCode();
            int PW_6IA = Document.Certificateofoccupancyinspectionapplication.GetHashCode();
            int pw_7 = Document.Certifiateofoccupancyletterofcompletionfolderreviewrequest.GetHashCode();
            int PW_2 = Document.Workpermitapplication.GetHashCode();

            int PW_72022 = Document.CertificateofOccupancyWorksheet2022.GetHashCode();
            int PW_72023 = Document.LetterofCompletionRequestPW72022.GetHashCode();
            int PW_2112022 = Document.WorkpermitapplicationNew2022.GetHashCode();
            int PW_232022 = Document.Workpermitapplication2022.GetHashCode();
            int PW_1112022 = Document.PlanorworkapprovalapplicationNew2022.GetHashCode();

            List<int> pw2List = rpoContext.JobDocumentFields
                         .Join(rpoContext.JobDocuments,
                         post => post.IdJobDocument,
                         Jobdocument => Jobdocument.Id,
                         (post, meta) => new { Post = post, Meta = meta })
                         .Where(postAndMeta => postAndMeta.Post.ActualValue == "Initial Permit" && postAndMeta.Meta.IdDocument == PW_2 && postAndMeta.Meta.IdJob == idJob)
                         .Select(postAndMeta => postAndMeta.Meta.Id).ToList();

            List<int> pw_108List = rpoContext.JobDocumentFields
                          .Join(rpoContext.JobDocuments,
                          post => post.IdJobDocument,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })
                          .Where(postAndMeta => (postAndMeta.Post.ActualValue == "Withdrwal - Entire Job" || postAndMeta.Post.ActualValue == "Withdrwal - Partial") && postAndMeta.Meta.IdDocument == PW_108 && postAndMeta.Meta.IdJob == idJob)
                          .Select(postAndMeta => postAndMeta.Meta.Id).ToList();

            List<int> pw_114List = rpoContext.JobDocumentFields
                          .Join(rpoContext.JobDocuments,
                          post => post.IdJobDocument,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })
                          .Where(postAndMeta => postAndMeta.Post.ActualValue == "Post Approval Amendment" && postAndMeta.Meta.IdDocument == PW_144 && postAndMeta.Meta.IdJob == idJob)
                          .Select(postAndMeta => postAndMeta.Meta.Id).ToList();

            List<int> pw_7List = rpoContext.JobDocumentFields
                          .Join(rpoContext.JobDocuments,
                          post => post.IdJobDocument,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })
                          .Where(postAndMeta => (postAndMeta.Post.ActualValue == "Renewal of TCO" || postAndMeta.Post.ActualValue == "Final Certificate of Occupancy" || postAndMeta.Post.ActualValue == "Letter Of Completion") && postAndMeta.Meta.IdDocument == pw_7 && postAndMeta.Meta.IdJob == idJob)
                          .Select(postAndMeta => postAndMeta.Meta.Id).ToList();

            List<int> PW_6IAList = rpoContext.JobDocuments.Where(x => x.IdJob == idJob && x.IdDocument == PW_6IA)
             .Select(x => x.Id)
             .ToList();

            List<int> pw_72022List = rpoContext.JobDocumentFields
                       .Join(rpoContext.JobDocuments,
                       post => post.IdJobDocument,
                       meta => meta.Id,
                       (post, meta) => new { Post = post, Meta = meta })
                       .Where(postAndMeta => postAndMeta.Meta.IdDocument == PW_72022 && postAndMeta.Meta.IdJob == idJob && postAndMeta.Meta.IdJob == idJob)
                       .Select(postAndMeta => postAndMeta.Meta.Id).Distinct().ToList();

            List<int> pw_72023List = rpoContext.JobDocumentFields
                        .Join(rpoContext.JobDocuments,
                        post => post.IdJobDocument,
                        meta => meta.Id,
                        (post, meta) => new { Post = post, Meta = meta })
                        .Where(postAndMeta => postAndMeta.Meta.IdDocument == PW_72023 && postAndMeta.Meta.IdJob == idJob && postAndMeta.Meta.IdJob == idJob)
                        .Select(postAndMeta => postAndMeta.Meta.Id).Distinct().ToList();

            List<int> pw_2112022List = rpoContext.JobDocumentFields
                       .Join(rpoContext.JobDocuments,
                       post => post.IdJobDocument,
                       meta => meta.Id,
                       (post, meta) => new { Post = post, Meta = meta })
                       .Where(postAndMeta => postAndMeta.Meta.IdDocument == PW_2112022 && postAndMeta.Meta.IdJob == idJob && postAndMeta.Meta.IdJob == idJob)
                       .Select(postAndMeta => postAndMeta.Meta.Id).Distinct().ToList();

            List<int> pw_232022List = rpoContext.JobDocumentFields
                       .Join(rpoContext.JobDocuments,
                       post => post.IdJobDocument,
                       meta => meta.Id,
                       (post, meta) => new { Post = post, Meta = meta })
                       .Where(postAndMeta => postAndMeta.Meta.IdDocument == PW_232022 && postAndMeta.Meta.IdJob == idJob && postAndMeta.Meta.IdJob == idJob)
                       .Select(postAndMeta => postAndMeta.Meta.Id).Distinct().ToList();

            List<int> pw_1112022List = rpoContext.JobDocumentFields
                       .Join(rpoContext.JobDocuments,
                       post => post.IdJobDocument,
                       meta => meta.Id,
                       (post, meta) => new { Post = post, Meta = meta })
                       .Where(postAndMeta => postAndMeta.Meta.IdDocument == PW_1112022 && postAndMeta.Meta.IdJob == idJob && postAndMeta.Meta.IdJob == idJob)
                       .Select(postAndMeta => postAndMeta.Meta.Id).Distinct().ToList();

            List<int> alldocuments = new List<int>();
            alldocuments.AddRange(pw2List);
            alldocuments.AddRange(pw_108List);
            alldocuments.AddRange(pw_114List);
            alldocuments.AddRange(pw_7List);
            alldocuments.AddRange(PW_6IAList);
            alldocuments.AddRange(pw_72022List);
            alldocuments.AddRange(pw_72023List);
            alldocuments.AddRange(pw_2112022List);
            alldocuments.AddRange(pw_232022List);
            alldocuments.AddRange(pw_1112022List);

            List<IdItemName> idItemNameList = new List<IdItemName>();
            List<IdItemName> iditemPw2List = new List<IdItemName>();

            foreach (var item in pw2List)
            {


                var jobdocfiels = rpoContext.JobDocumentFields
                          .Join(rpoContext.DocumentFields,
                          post => post.IdDocumentField,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })

                          .Where(q => q.Post.IdJobDocument == item && q.Meta.APIUrl == "/api/jobdocumentdrodown/JobApplicationNumberType")
                          .Select(postAndMeta => postAndMeta.Post.Value + " - " + postAndMeta.Post.ActualValue).ToList();

                var job = rpoContext.JobDocuments.Where(a => a.Id == item).FirstOrDefault();
                JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == job.IdJobApplication);
                string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                obj.ItemName = "PW-2 - " + applicationNumber + " " + "Initial Permit";
                iditemPw2List.Add(obj);

            }

            List<IdItemName> iditemPW108List = new List<IdItemName>();
            foreach (var item in pw_108List)
            {
                var jobdocfiels = rpoContext.JobDocumentFields
                          .Join(rpoContext.DocumentFields,
                          post => post.IdDocumentField,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })

                          .Where(q => q.Post.IdJobDocument == item && q.Meta.APIUrl == "/api/jobdocumentdrodown/JobApplicationNumberType")
                          .Select(postAndMeta => postAndMeta.Post.Value + " - " + postAndMeta.Post.ActualValue).ToList();

                var job = rpoContext.JobDocuments.Where(a => a.Id == item).FirstOrDefault();
                JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == job.IdJobApplication);
                string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                string type = string.Empty;
                type = job.DocumentDescription.Trim();
                if (type.Contains("Withdrwal - Entire Job"))
                {
                    type = "Withdrwal - Entire Job";
                }
                if (type.Contains("Withdrwal - Partial"))
                {
                    type = "Withdrwal - Partial";
                }
                obj.ItemName = "PW-1-2019 - " + applicationNumber + " " + type;
                iditemPW108List.Add(obj);
            }


            List<IdItemName> iditemPW114List = new List<IdItemName>();
            foreach (var item in pw_114List)
            {
                var jobdocfiels = rpoContext.JobDocumentFields
                         .Join(rpoContext.DocumentFields,
                         post => post.IdDocumentField,
                         meta => meta.Id,
                         (post, meta) => new { Post = post, Meta = meta })

                         .Where(q => q.Post.IdJobDocument == item && q.Meta.APIUrl == "/api/jobdocumentdrodown/JobApplicationNumberType")
                         .Select(postAndMeta => postAndMeta.Post.Value + " - " + postAndMeta.Post.ActualValue).ToList();

                var job = rpoContext.JobDocuments.Where(a => a.Id == item).FirstOrDefault();
                JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == job.IdJobApplication);
                string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                obj.ItemName = "PW-114 - " + applicationNumber + " " + "Post Approval Amendment";
                iditemPW114List.Add(obj);
            }

            List<IdItemName> idnamepw7List = new List<IdItemName>();
            foreach (var item in pw_7List)
            {
                var jobdocfiels = rpoContext.JobDocumentFields
                          .Join(rpoContext.DocumentFields,
                          post => post.IdDocumentField,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })

                          .Where(q => q.Post.IdJobDocument == item && q.Meta.APIUrl == "/api/jobdocumentdrodown/JobApplicationNumberType")
                          .Select(postAndMeta => postAndMeta.Post.Value + " - " + postAndMeta.Post.ActualValue).ToList();

                var job = rpoContext.JobDocuments.Where(a => a.Id == item).FirstOrDefault();
                JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == job.IdJobApplication);
                string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                string type = string.Empty;
                type = job.DocumentDescription.Trim();
                if (type.Contains("Renewal of TCO"))
                {
                    type = "Renewal of TCO";
                }
                if (type.Contains("Final Certificate of Occupancy"))
                {
                    type = "Final Certificate of Occupancy";
                }
                if (type.Contains("Completion"))
                {
                    type = "Letter Of Completion";
                }
                obj.ItemName = "PW-7 - " + applicationNumber + " " + type;
                idnamepw7List.Add(obj);
            }
            List<IdItemName> idnamepw6IA = new List<IdItemName>();
            foreach (var item in PW_6IAList)
            {
                var jobdocfiels = rpoContext.JobDocumentFields
                          .Join(rpoContext.DocumentFields,
                          post => post.IdDocumentField,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })

                          .Where(q => q.Post.IdJobDocument == item && q.Meta.APIUrl == "/api/jobdocumentdrodown/JobApplicationNumberType")
                          .Select(postAndMeta => postAndMeta.Post.Value + " - " + postAndMeta.Post.ActualValue).ToList();

                var job = rpoContext.JobDocuments.Where(a => a.Id == item).FirstOrDefault();
                JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == job.IdJobApplication);
                string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                obj.ItemName = "PW-2 - " + applicationNumber + " " + "Initial Permit";
                idnamepw6IA.Add(obj);
            }

            //loop added by ML
            List<IdItemName> idnamepw72022List = new List<IdItemName>();
            foreach (var item in pw_72022List)
            {
                var jobdocfiels = rpoContext.JobDocumentFields
                          .Join(rpoContext.DocumentFields,
                          post => post.IdDocumentField,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })

                          .Where(q => q.Post.IdJobDocument == item && q.Meta.APIUrl == "/api/jobdocumentdrodown/JobApplicationNumberType")
                          .Select(postAndMeta => postAndMeta.Post.Value + " - " + postAndMeta.Post.ActualValue).Distinct().ToList();

                var job = rpoContext.JobDocuments.Where(a => a.Id == item).FirstOrDefault();
                JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == job.IdJobApplication);
                string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                string type = string.Empty;
                type = job.DocumentDescription.Trim();
                if (type.Contains("Core & Shell"))
                {
                    type = "Core & Shell";
                }
                if (type.Contains("TCO-Initial"))
                {
                    type = "TCO-Initial";
                }
                if (type.Contains("TCO Renewal with Change"))
                {
                    type = "TCO Renewal with Change";
                }
                if (type.Contains("Final"))
                {
                    type = "Final";
                }
                obj.ItemName = "PW-7-2022 - " + applicationNumber + " " + type;
                idnamepw72022List.Add(obj);
            }

            List<IdItemName> idnamepw72023List = new List<IdItemName>();
            foreach (var item in pw_72023List)
            {
                var jobdocfiels = rpoContext.JobDocumentFields
                          .Join(rpoContext.DocumentFields,
                          post => post.IdDocumentField,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })

                          .Where(q => q.Post.IdJobDocument == item && q.Meta.APIUrl == "/api/jobdocumentdrodown/JobApplicationNumberType")
                          .Select(postAndMeta => postAndMeta.Post.Value + " - " + postAndMeta.Post.ActualValue).Distinct().ToList();

                var job = rpoContext.JobDocuments.Where(a => a.Id == item).FirstOrDefault();
                JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == job.IdJobApplication);
                string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                string type = string.Empty;
                type = job.DocumentDescription.Trim();
                if (type.Contains("Letter of Completion"))
                {
                    type = "Letter of Completion";
                }
                if (type.Contains("Notification"))
                {
                    type = "Notification";
                }
                obj.ItemName = "PW-7 11/22 - " + applicationNumber + " " + type;
                idnamepw72023List.Add(obj);
            }
            List<IdItemName> iditemPW_2112022List = new List<IdItemName>();
            foreach (var item in pw_2112022List)
            {


                var jobdocfiels = rpoContext.JobDocumentFields
                          .Join(rpoContext.DocumentFields,
                          post => post.IdDocumentField,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })

                          .Where(q => q.Post.IdJobDocument == item && q.Meta.APIUrl == "/api/jobdocumentdrodown/JobApplicationNumberType")
                          .Select(postAndMeta => postAndMeta.Post.Value + " - " + postAndMeta.Post.ActualValue).ToList();

                var job = rpoContext.JobDocuments.Where(a => a.Id == item).FirstOrDefault();
                JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == job.IdJobApplication);
                string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                obj.ItemName = "PW-2 11/22 - " + applicationNumber + " " + "Initial Permit";
                iditemPW_2112022List.Add(obj);

            }
            List<IdItemName> iditemPW_232022List = new List<IdItemName>();
            foreach (var item in pw_232022List)
            {


                var jobdocfiels = rpoContext.JobDocumentFields
                          .Join(rpoContext.DocumentFields,
                          post => post.IdDocumentField,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })

                          .Where(q => q.Post.IdJobDocument == item && q.Meta.APIUrl == "/api/jobdocumentdrodown/JobApplicationNumberType")
                          .Select(postAndMeta => postAndMeta.Post.Value + " - " + postAndMeta.Post.ActualValue).ToList();

                var job = rpoContext.JobDocuments.Where(a => a.Id == item).FirstOrDefault();
                JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == job.IdJobApplication);
                string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                obj.ItemName = "PW-2 3/2022 - " + applicationNumber + " " + "Initial Permit";
                iditemPW_232022List.Add(obj);

            }
            List<IdItemName> iditemPW_1112022List = new List<IdItemName>();
            foreach (var item in pw_1112022List)
            {


                var jobdocfiels = rpoContext.JobDocumentFields
                          .Join(rpoContext.DocumentFields,
                          post => post.IdDocumentField,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })

                          .Where(q => q.Post.IdJobDocument == item && q.Meta.APIUrl == "/api/jobdocumentdrodown/JobApplicationNumberType")
                          .Select(postAndMeta => postAndMeta.Post.Value + " - " + postAndMeta.Post.ActualValue).ToList();

                var job = rpoContext.JobDocuments.Where(a => a.Id == item).FirstOrDefault();
                JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == job.IdJobApplication);
                string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                obj.ItemName = "PW-1 11/22 - " + applicationNumber + " " + "Initial Permit";
                iditemPW_1112022List.Add(obj);

            }


            idItemNameList.AddRange(iditemPw2List);
            idItemNameList.AddRange(iditemPW114List);
            idItemNameList.AddRange(iditemPW108List);
            idItemNameList.AddRange(idnamepw7List);
            idItemNameList.AddRange(idnamepw6IA);
            idItemNameList.AddRange(idnamepw72022List);
            idItemNameList.AddRange(idnamepw72023List);
            idItemNameList.AddRange(iditemPW_2112022List);
            idItemNameList.AddRange(iditemPW_232022List);
            idItemNameList.AddRange(iditemPW_1112022List);

            //  jobDocument1. Where(x => x.DocumentField.Field.FieldName == "Last Name"))

            // List<int> jobDocument = rpoContext.JobDocuments.Where(x => x.IdJob == idJob && (x.IdDocument == idDocument || x.IdDocument == PW_108 
            //                                             || x.IdDocument == PW_144 || x.IdDocument == PW_6IA || x.IdDocument == pw_7)).Select(x => x.Id).ToList();





            //List<IdItemName> idItemNameList = rpoContext.JobDocumentFields
            //     //   .Where(x => jobDocument.Contains(x.IdJobDocument) && x.DocumentField.Id == tenDaysNotice_Recipient)
            //     .Where(x => jobDocument.Contains(x.IdJobDocument) && x.DocumentField.Field.FieldName == "Last Name")
            //     .AsEnumerable()
            //     .Select(x => LIC6List(x)).Distinct().ToList();


            return Ok(idItemNameList);



        }
        /// <summary>
        /// Gets the list of BINTAKdocumentTypeNyApplication for list.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the list of BINTAKdocumentTypeNyApplication for list.</returns>         
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/BINTAKdocumentTypeNyApplication/{idJob}/{idDocument}/{idParent}/{idJobApplication}")]
        public IHttpActionResult GetBINTAKTypeDropdownByApplication(int idJob, int idDocument = 0, int idJobApplication = 0)
        {
            //idDocument = Document.GeneralContractorRegistration.GetHashCode();
            //List<int> jobDocument = rpoContext.JobDocuments.Where(x => x.IdJob == idJob && x.IdDocument == idDocument).Select(x => x.Id).ToList();


            //List<IdItemName> idItemNameList = rpoContext.JobDocumentFields.Include("JobDocument")
            //    .Where(x => jobDocument.Contains(x.IdJobDocument) && (x.DocumentField.Id == 521507))
            //    .AsEnumerable()
            //    .Select(x => VARPMTJobApplicationNumber(x)).Distinct().ToList();
            //return Ok(idItemNameList);

            idDocument = Document.BoroughIntakeForm2.GetHashCode();
            int PW_108 = Document.Planorworkapprovalapplication2019.GetHashCode();
            //added by M.B
            int PW_1 = Document.Planorworkapprovalapplication2022.GetHashCode();
            int PW_144 = Document.Application2014Present.GetHashCode();
            int PW_6IA = Document.Certificateofoccupancyinspectionapplication.GetHashCode();
            int pw_7 = Document.Certifiateofoccupancyletterofcompletionfolderreviewrequest.GetHashCode();
            int PW_2 = Document.Workpermitapplication.GetHashCode();
            int PW_72022 = Document.CertificateofOccupancyWorksheet2022.GetHashCode();
            int PW_72023 = Document.LetterofCompletionRequestPW72022.GetHashCode();
            int PW_2112022 = Document.WorkpermitapplicationNew2022.GetHashCode();
            int PW_232022 = Document.Workpermitapplication2022.GetHashCode();
            int PW_1112022 = Document.PlanorworkapprovalapplicationNew2022.GetHashCode();


            List<int> pw2List = rpoContext.JobDocumentFields
                         .Join(rpoContext.JobDocuments,
                         post => post.IdJobDocument,
                         Jobdocument => Jobdocument.Id,
                         (post, meta) => new { Post = post, Meta = meta })
                         .Where(postAndMeta => postAndMeta.Meta.IdDocument == PW_2 && postAndMeta.Meta.IdJob == idJob && postAndMeta.Meta.IdJobApplication == idJobApplication)
                         .Select(postAndMeta => postAndMeta.Meta.Id).Distinct().ToList();

            List<int> pw_108List = rpoContext.JobDocumentFields
                          .Join(rpoContext.JobDocuments,
                          post => post.IdJobDocument,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })
                          .Where(postAndMeta => postAndMeta.Meta.IdDocument == PW_108 && postAndMeta.Meta.IdJob == idJob && postAndMeta.Meta.IdJobApplication == idJobApplication)
                          .Select(postAndMeta => postAndMeta.Meta.Id).Distinct().ToList();


            //List<int> pw_114List = rpoContext.JobDocumentFields
            //              .Join(rpoContext.JobDocuments,
            //              post => post.IdJobDocument,
            //              meta => meta.Id,
            //              (post, meta) => new { Post = post, Meta = meta })
            //              .Where(postAndMeta => postAndMeta.Post.ActualValue == "Post Approval Amendment" && postAndMeta.Meta.IdDocument == PW_144 && postAndMeta.Meta.IdJob == idJob)
            //              .Select(postAndMeta => postAndMeta.Meta.Id).ToList();

            List<int> pw_7List = rpoContext.JobDocumentFields
                          .Join(rpoContext.JobDocuments,
                          post => post.IdJobDocument,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })
                          .Where(postAndMeta => postAndMeta.Meta.IdDocument == pw_7 && postAndMeta.Meta.IdJob == idJob && postAndMeta.Meta.IdJobApplication == idJobApplication)
                          .Select(postAndMeta => postAndMeta.Meta.Id).Distinct().ToList();

            List<int> PW_6IAList = rpoContext.JobDocuments.Where(x => x.IdJob == idJob && x.IdDocument == PW_6IA && x.IdJobApplication == idJobApplication)
             .Select(x => x.Id)
             .ToList();

            //added by M.B
            List<int> pw_1List = rpoContext.JobDocumentFields
                    .Join(rpoContext.JobDocuments,
                    post => post.IdJobDocument,
                    meta => meta.Id,
                    (post, meta) => new { Post = post, Meta = meta })
                    .Where(postAndMeta => postAndMeta.Meta.IdDocument == PW_1 && postAndMeta.Meta.IdJob == idJob && postAndMeta.Meta.IdJobApplication == idJobApplication)
                    .Select(postAndMeta => postAndMeta.Meta.Id).Distinct().ToList();

            //add by ML           
            List<int> pw_72022List = rpoContext.JobDocumentFields
                        .Join(rpoContext.JobDocuments,
                        post => post.IdJobDocument,
                        meta => meta.Id,
                        (post, meta) => new { Post = post, Meta = meta })
                        .Where(postAndMeta => postAndMeta.Meta.IdDocument == PW_72022 && postAndMeta.Meta.IdJob == idJob && postAndMeta.Meta.IdJobApplication == idJobApplication)
                        .Select(postAndMeta => postAndMeta.Meta.Id).Distinct().ToList();

            List<int> pw_72023List = rpoContext.JobDocumentFields
                        .Join(rpoContext.JobDocuments,
                        post => post.IdJobDocument,
                        meta => meta.Id,
                        (post, meta) => new { Post = post, Meta = meta })
                        .Where(postAndMeta => postAndMeta.Meta.IdDocument == PW_72023 && postAndMeta.Meta.IdJob == idJob && postAndMeta.Meta.IdJobApplication == idJobApplication)
                        .Select(postAndMeta => postAndMeta.Meta.Id).Distinct().ToList();

            List<int> pw_2112022List = rpoContext.JobDocumentFields
                       .Join(rpoContext.JobDocuments,
                       post => post.IdJobDocument,
                       meta => meta.Id,
                       (post, meta) => new { Post = post, Meta = meta })
                       .Where(postAndMeta => postAndMeta.Meta.IdDocument == PW_2112022 && postAndMeta.Meta.IdJob == idJob && postAndMeta.Meta.IdJobApplication == idJobApplication)
                       .Select(postAndMeta => postAndMeta.Meta.Id).Distinct().ToList();

            List<int> pw_232022List = rpoContext.JobDocumentFields
                       .Join(rpoContext.JobDocuments,
                       post => post.IdJobDocument,
                       meta => meta.Id,
                       (post, meta) => new { Post = post, Meta = meta })
                       .Where(postAndMeta => postAndMeta.Meta.IdDocument == PW_232022 && postAndMeta.Meta.IdJob == idJob && postAndMeta.Meta.IdJobApplication == idJobApplication)
                       .Select(postAndMeta => postAndMeta.Meta.Id).Distinct().ToList();

            List<int> pw_1112022List = rpoContext.JobDocumentFields
                .Join(rpoContext.JobDocuments,
                post => post.IdJobDocument,
                meta => meta.Id,
                (post, meta) => new { Post = post, Meta = meta })
                .Where(postAndMeta => postAndMeta.Meta.IdDocument == PW_1112022 && postAndMeta.Meta.IdJob == idJob && postAndMeta.Meta.IdJobApplication == idJobApplication)
                .Select(postAndMeta => postAndMeta.Meta.Id).Distinct().ToList();

            List<int> alldocuments = new List<int>();
            alldocuments.AddRange(pw2List);
            alldocuments.AddRange(pw_108List);
            //alldocuments.AddRange(pw_114List);
            alldocuments.AddRange(pw_7List);
            alldocuments.AddRange(PW_6IAList);
            //added by M.B
            alldocuments.AddRange(pw_1List);
            alldocuments.AddRange(pw_72022List);
            alldocuments.AddRange(pw_72023List);
            alldocuments.AddRange(pw_2112022List);
            alldocuments.AddRange(pw_232022List);
            alldocuments.AddRange(pw_1112022List);

            List<IdItemName> idItemNameList = new List<IdItemName>();
            List<IdItemName> iditemPw2List = new List<IdItemName>();

            foreach (var item in pw2List)
            {


                var jobdocfiels = rpoContext.JobDocumentFields
                          .Join(rpoContext.DocumentFields,
                          post => post.IdDocumentField,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })

                          .Where(q => q.Post.IdJobDocument == item && q.Meta.APIUrl == "/api/jobdocumentdrodown/JobApplicationNumberType")
                          .Select(postAndMeta => postAndMeta.Post.Value + " - " + postAndMeta.Post.ActualValue).ToList();

                var job = rpoContext.JobDocuments.Where(a => a.Id == item).FirstOrDefault();
                JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == job.IdJobApplication);
                string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                obj.ItemName = "PW-2 - " + applicationNumber + " " + "Initial Permit";
                iditemPw2List.Add(obj);

            }

            List<IdItemName> iditemPW108List = new List<IdItemName>();
            foreach (var item in pw_108List)
            {
                var jobdocfiels = rpoContext.JobDocumentFields
                          .Join(rpoContext.DocumentFields,
                          post => post.IdDocumentField,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })

                          .Where(q => q.Post.IdJobDocument == item && q.Meta.APIUrl == "/api/jobdocumentdrodown/JobApplicationNumberType")
                          .Select(postAndMeta => postAndMeta.Post.Value + " - " + postAndMeta.Post.ActualValue).Distinct().ToList();

                var job = rpoContext.JobDocuments.Where(a => a.Id == item).FirstOrDefault();
                JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == job.IdJobApplication);
                string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                string type = string.Empty;
                type = job.DocumentDescription.Trim();
                if (type.Contains("Withdrwal - Entire Job"))
                {
                    type = "Withdrwal - Entire Job";
                }
                if (type.Contains("Withdrwal - Partial"))
                {
                    type = "Withdrwal - Partial";
                }
                obj.ItemName = "PW-1-2019 - " + applicationNumber + " " + type;
                iditemPW108List.Add(obj);
            }



            //List<IdItemName> iditemPW114List = new List<IdItemName>();
            //foreach (var item in pw_114List)
            //{
            //    var jobdocfiels = rpoContext.JobDocumentFields
            //             .Join(rpoContext.DocumentFields,
            //             post => post.IdDocumentField,
            //             meta => meta.Id,
            //             (post, meta) => new { Post = post, Meta = meta })

            //             .Where(q => q.Post.IdJobDocument == item && q.Meta.APIUrl == "/api/jobdocumentdrodown/JobApplicationNumberType")
            //             .Select(postAndMeta => postAndMeta.Post.Value + " - " + postAndMeta.Post.ActualValue).ToList();

            //    var job = rpoContext.JobDocuments.Where(a => a.Id == item).FirstOrDefault();
            //    JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == job.IdJobApplication);
            //    string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
            //    IdItemName obj = new IdItemName();
            //    obj.Id = Convert.ToString(item);
            //    obj.ItemName = "PW-114 - " + applicationNumber + " " + "Post Approval Amendment";
            //    iditemPW114List.Add(obj);
            //}

            List<IdItemName> idnamepw7List = new List<IdItemName>();
            foreach (var item in pw_7List)
            {
                var jobdocfiels = rpoContext.JobDocumentFields
                          .Join(rpoContext.DocumentFields,
                          post => post.IdDocumentField,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })

                          .Where(q => q.Post.IdJobDocument == item && q.Meta.APIUrl == "/api/jobdocumentdrodown/JobApplicationNumberType")
                          .Select(postAndMeta => postAndMeta.Post.Value + " - " + postAndMeta.Post.ActualValue).Distinct().ToList();

                var job = rpoContext.JobDocuments.Where(a => a.Id == item).FirstOrDefault();
                JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == job.IdJobApplication);
                string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                string type = string.Empty;
                type = job.DocumentDescription.Trim();
                if (type.Contains("Renewal of TCO"))
                {
                    type = "Renewal of TCO";
                }
                if (type.Contains("Final Certificate of Occupancy"))
                {
                    type = "Final Certificate of Occupancy";
                }
                if (type.Contains("Completion"))
                {
                    type = "Letter Of Completion";
                }
                obj.ItemName = "PW-7 - " + applicationNumber + " " + type;
                idnamepw7List.Add(obj);
            }
            List<IdItemName> idnamepw6IA = new List<IdItemName>();
            foreach (var item in PW_6IAList)
            {
                var jobdocfiels = rpoContext.JobDocumentFields
                          .Join(rpoContext.DocumentFields,
                          post => post.IdDocumentField,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })

                          .Where(q => q.Post.IdJobDocument == item && q.Meta.APIUrl == "/api/jobdocumentdrodown/JobApplicationNumberType")
                          .Select(postAndMeta => postAndMeta.Post.Value + " - " + postAndMeta.Post.ActualValue).ToList();

                var job = rpoContext.JobDocuments.Where(a => a.Id == item).FirstOrDefault();
                JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == job.IdJobApplication);
                string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                string type = string.Empty;
                type = job.DocumentDescription;

                obj.ItemName = "PW-6IA - " + applicationNumber;
                idnamepw6IA.Add(obj);
            }

            //loop added by M.B
            List<IdItemName> iditemPW1List = new List<IdItemName>();
            foreach (var item in pw_1List)
            {
                var jobdocfiels = rpoContext.JobDocumentFields
                          .Join(rpoContext.DocumentFields,
                          post => post.IdDocumentField,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })

                          .Where(q => q.Post.IdJobDocument == item && q.Meta.APIUrl == "/api/jobdocumentdrodown/JobApplicationNumberType")
                          .Select(postAndMeta => postAndMeta.Post.Value + " - " + postAndMeta.Post.ActualValue).Distinct().ToList();

                var job = rpoContext.JobDocuments.Where(a => a.Id == item).FirstOrDefault();
                JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == job.IdJobApplication);
                string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                string type = string.Empty;
                type = job.DocumentDescription.Trim();
                if (type.Contains("Withdrwal - Entire Job"))
                {
                    type = "Withdrwal - Entire Job";
                }
                if (type.Contains("Withdrwal - Partial"))
                {
                    type = "Withdrwal - Partial";
                }
                obj.ItemName = "PW-1 3/2022 - " + applicationNumber + " " + type;
                iditemPW1List.Add(obj);
            }

            //loop added by ML
            List<IdItemName> idnamepw72022List = new List<IdItemName>();
            foreach (var item in pw_72022List)
            {
                var jobdocfiels = rpoContext.JobDocumentFields
                          .Join(rpoContext.DocumentFields,
                          post => post.IdDocumentField,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })

                          .Where(q => q.Post.IdJobDocument == item && q.Meta.APIUrl == "/api/jobdocumentdrodown/JobApplicationNumberType")
                          .Select(postAndMeta => postAndMeta.Post.Value + " - " + postAndMeta.Post.ActualValue).Distinct().ToList();

                var job = rpoContext.JobDocuments.Where(a => a.Id == item).FirstOrDefault();
                JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == job.IdJobApplication);
                string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                string type = string.Empty;
                type = job.DocumentDescription.Trim();
                if (type.Contains("Core & Shell"))
                {
                    type = "Core & Shell";
                }
                if (type.Contains("TCO-Initial"))
                {
                    type = "TCO-Initial";
                }
                if (type.Contains("TCO Renewal with Change"))
                {
                    type = "TCO Renewal with Change";
                }
                if (type.Contains("Final"))
                {
                    type = "Final";
                }
                obj.ItemName = "PW-7-2022 - " + applicationNumber + " " + type;
                idnamepw72022List.Add(obj);
            }

            List<IdItemName> idnamepw72023List = new List<IdItemName>();
            foreach (var item in pw_72023List)
            {
                var jobdocfiels = rpoContext.JobDocumentFields
                          .Join(rpoContext.DocumentFields,
                          post => post.IdDocumentField,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })

                          .Where(q => q.Post.IdJobDocument == item && q.Meta.APIUrl == "/api/jobdocumentdrodown/JobApplicationNumberType")
                          .Select(postAndMeta => postAndMeta.Post.Value + " - " + postAndMeta.Post.ActualValue).Distinct().ToList();

                var job = rpoContext.JobDocuments.Where(a => a.Id == item).FirstOrDefault();
                JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == job.IdJobApplication);
                string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                string type = string.Empty;
                type = job.DocumentDescription.Trim();              
                if (type.Contains("Letter of Completion"))
                {
                    type = "Letter of Completion";
                }
                if (type.Contains("Notification"))
                {
                    type = "Notification";
                }
                obj.ItemName = "PW-7 11/22 - " + applicationNumber + " " + type;
                idnamepw72023List.Add(obj);
            }
            List<IdItemName> iditemPW_2112022List = new List<IdItemName>();
            foreach (var item in pw_2112022List)
            {


                var jobdocfiels = rpoContext.JobDocumentFields
                          .Join(rpoContext.DocumentFields,
                          post => post.IdDocumentField,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })

                          .Where(q => q.Post.IdJobDocument == item && q.Meta.APIUrl == "/api/jobdocumentdrodown/JobApplicationNumberType")
                          .Select(postAndMeta => postAndMeta.Post.Value + " - " + postAndMeta.Post.ActualValue).ToList();

                var job = rpoContext.JobDocuments.Where(a => a.Id == item).FirstOrDefault();
                JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == job.IdJobApplication);
                string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                obj.ItemName = "PW-2 11/22 - " + applicationNumber + " " + "Initial Permit";
                iditemPW_2112022List.Add(obj);

            }
            List<IdItemName> iditemPW_232022List = new List<IdItemName>();
            foreach (var item in pw_232022List)
            {


                var jobdocfiels = rpoContext.JobDocumentFields
                          .Join(rpoContext.DocumentFields,
                          post => post.IdDocumentField,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })

                          .Where(q => q.Post.IdJobDocument == item && q.Meta.APIUrl == "/api/jobdocumentdrodown/JobApplicationNumberType")
                          .Select(postAndMeta => postAndMeta.Post.Value + " - " + postAndMeta.Post.ActualValue).ToList();

                var job = rpoContext.JobDocuments.Where(a => a.Id == item).FirstOrDefault();
                JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == job.IdJobApplication);
                string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                obj.ItemName = "PW-2 3/2022 - " + applicationNumber + " " + "Initial Permit";
                iditemPW_232022List.Add(obj);

            }
            List<IdItemName> iditemPW_1112022List = new List<IdItemName>();
            foreach (var item in pw_1112022List)
            {


                var jobdocfiels = rpoContext.JobDocumentFields
                          .Join(rpoContext.DocumentFields,
                          post => post.IdDocumentField,
                          meta => meta.Id,
                          (post, meta) => new { Post = post, Meta = meta })

                          .Where(q => q.Post.IdJobDocument == item && q.Meta.APIUrl == "/api/jobdocumentdrodown/JobApplicationNumberType")
                          .Select(postAndMeta => postAndMeta.Post.Value + " - " + postAndMeta.Post.ActualValue).ToList();

                var job = rpoContext.JobDocuments.Where(a => a.Id == item).FirstOrDefault();
                JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == job.IdJobApplication);
                string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                IdItemName obj = new IdItemName();
                obj.Id = Convert.ToString(item);
                obj.ItemName = "PW-1 11/22 - " + applicationNumber + " " + "Initial Permit";
                iditemPW_1112022List.Add(obj);

            }

            idItemNameList.AddRange(iditemPw2List);
            // idItemNameList.AddRange(iditemPW114List);        
            idItemNameList.AddRange(iditemPW108List);
            idItemNameList.AddRange(idnamepw7List);
            idItemNameList.AddRange(idnamepw6IA);
            //added by M.B
            idItemNameList.AddRange(iditemPW1List);
            idItemNameList.AddRange(idnamepw72022List);
            idItemNameList.AddRange(idnamepw72023List);
            idItemNameList.AddRange(iditemPW_2112022List);
            idItemNameList.AddRange(iditemPW_232022List);
            idItemNameList.AddRange(iditemPW_1112022List);
            //  jobDocument1. Where(x => x.DocumentField.Field.FieldName == "Last Name"))

            // List<int> jobDocument = rpoContext.JobDocuments.Where(x => x.IdJob == idJob && (x.IdDocument == idDocument || x.IdDocument == PW_108 
            //                                             || x.IdDocument == PW_144 || x.IdDocument == PW_6IA || x.IdDocument == pw_7)).Select(x => x.Id).ToList();





            //List<IdItemName> idItemNameList = rpoContext.JobDocumentFields
            //     //   .Where(x => jobDocument.Contains(x.IdJobDocument) && x.DocumentField.Id == tenDaysNotice_Recipient)
            //     .Where(x => jobDocument.Contains(x.IdJobDocument) && x.DocumentField.Field.FieldName == "Last Name")
            //     .AsEnumerable()
            //     .Select(x => LIC6List(x)).Distinct().ToList();


            return Ok(idItemNameList);



        }




        /// <summary>
        /// 
        /// 
        /// 
        /// Gets the list of pw2Subcontractor for list.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the list of pw2Subcontractor for list.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/pw2Subcontractor/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult Getpw2Subcontractor(int idJob, int idDocument = 0)
        {

            var list = new List<IdItemName>();
            list.Add(new IdItemName { Id = "Concrete", ItemName = "Concrete" });
            list.Add(new IdItemName { Id = "Demoltion", ItemName = "Demoltion" });
            list.Add(new IdItemName { Id = "None", ItemName = "None" });


            return Ok(list);
        }


        //sunay
        /// <summary>
        /// Gets the list of pw2Type for list.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the list of pw2Type for list.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/pw2Type/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult Getpw2Type(int idJob, int idDocument = 0)
        {

            var list = new List<IdItemName>();
            list.Add(new IdItemName { Id = "Initial Permit", ItemName = "Initial Permit" });
            list.Add(new IdItemName { Id = "No Work Permit", ItemName = "No Work Permit" });
            list.Add(new IdItemName { Id = "Renewal With Change", ItemName = "Renewal With Change" });
            list.Add(new IdItemName { Id = "Renewal With No Change", ItemName = "Renewal With No Change" });


            return Ok(list);
        }

        /// <summary>
        /// Gets the list of TR8Inspection Type list.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the list of TR8Inspection Type list.</returns>

        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobdocumentdrodown/TR82020InspectionType/{idJob}/{idDocument}/{idParent}")]
        public IHttpActionResult GetTR82020InspectionType(int idJob, int idDocument = 0)
        {
            return Ok(GetTR82020InspectionType());
        }

        public List<IdItemName> GetTR82020InspectionType()
        {
            List<IdItemName> result = new List<IdItemName>();
            result.Add(new IdItemName { Id = "opgPOFI", ItemName = "Protection of exposed foundation insulation" });
            result.Add(new IdItemName { Id = "opgIPAR", ItemName = "Insulation placement and R-values" });
            result.Add(new IdItemName { Id = "opgFUPR", ItemName = "Fenestration and door U-factor and product ratings" });
            result.Add(new IdItemName { Id = "opgFALK", ItemName = "Fenestration air leakage" });
            result.Add(new IdItemName { Id = "opgFNAR", ItemName = "Fenestration areas" });
            result.Add(new IdItemName { Id = "opgABIV", ItemName = "Air barrier − visual inspection" });
            result.Add(new IdItemName { Id = "opgABT", ItemName = "Air barrier − testing" });
            result.Add(new IdItemName { Id = "opgABCPTI", ItemName = "Air barrier continuity plan testing/inspection" });
            result.Add(new IdItemName { Id = "opgVEST", ItemName = "Vestibules" });
            result.Add(new IdItemName { Id = "opgFRPL", ItemName = "Fireplaces" });
            result.Add(new IdItemName { Id = "opgVADS", ItemName = "Ventilation and air distribution system" });
            result.Add(new IdItemName { Id = "opgSHTD", ItemName = "Shutoff dampers" });
            result.Add(new IdItemName { Id = "opgHVEQ", ItemName = "HVAC-R and service water heating equipment" });
            result.Add(new IdItemName { Id = "opgHVSC", ItemName = "HVAC-R and service water heating system controls" });
            result.Add(new IdItemName { Id = "opgHVSWP", ItemName = "HVAC-R and service water piping design and insulation" });
            result.Add(new IdItemName { Id = "opgDLKT", ItemName = "Duct leakage testing, insulation and design" });
            result.Add(new IdItemName { Id = "opgMET", ItemName = "Metering" });
            result.Add(new IdItemName { Id = "opgLIDU", ItemName = "Lighting in dwelling units" });
            result.Add(new IdItemName { Id = "opgILPW", ItemName = "Interior lighting power" });
            result.Add(new IdItemName { Id = "opgELPW", ItemName = "Exterior lighting power" });
            result.Add(new IdItemName { Id = "opgLTCT", ItemName = "Lighting controls" });
            result.Add(new IdItemName { Id = "opgELMO", ItemName = "Electrical motors and elevators" });
            result.Add(new IdItemName { Id = "opgMNTI", ItemName = "Maintenance information" });
            result.Add(new IdItemName { Id = "opgPMCT", ItemName = "Permanent certificate" });
            result.Add(new IdItemName { Id = "opgEVSER", ItemName = "Electric vehicle service equipment requirements" });

            return result;
        }


        private IdItemName VARPMTJobApplicationNumber(JobDocumentField jobDocumentField)
        {
            string itemname = jobDocumentField.ActualValue != null && jobDocumentField.ActualValue.ToString().Trim() != "" && jobDocumentField.ActualValue.Split('\n') != null && jobDocumentField.ActualValue.Split('\n').Count() > 0 ? jobDocumentField.ActualValue.Split('\n')[0] : string.Empty;

            JobDocumentField adjacentJobDocumentField = rpoContext.JobDocumentFields.FirstOrDefault(x => x.IdJobDocument == jobDocumentField.IdJobDocument && (x.DocumentField.Id == 543668));

            int jobApplicationId = int.Parse(jobDocumentField.Value);

            var jobApplication = rpoContext.JobApplications.Include("JobApplicationType").Where(d => d.Id == jobApplicationId && d.JobApplicationType.Id == d.IdJobApplicationType).Select(d => d.ApplicationNumber + " " + d.JobApplicationType.Description).FirstOrDefault();

            string adjacentaddress = adjacentJobDocumentField != null ? adjacentJobDocumentField.ActualValue : string.Empty;
            return new IdItemName
            {
                Id = Convert.ToString(jobDocumentField.IdJobDocument),
                //Id = jobDocumentField.JobDocument.Id.ToString(),
                // ItemName = adjacentaddress + (jobApplication != null && jobApplication != "" ? " (" + jobApplication + ")" : string.Empty)

                ItemName = (jobApplication != null && jobApplication != "" ? " [" + jobApplication + "]" : string.Empty) + (jobDocumentField != null ? " (" + jobDocumentField.JobDocument.DocumentDescription + ")" : string.Empty)
            };
        }


        /// <summary>
        /// Class IdItemName.
        /// </summary>
        public class IdItemName
        {
            /// <summary>
            /// Gets or sets the identifier.
            /// </summary>
            /// <value>The identifier.</value>
            public string Id { get; set; }

            /// <summary>
            /// Gets or sets the name of the item.
            /// </summary>
            /// <value>The name of the item.</value>
            public string ItemName { get; set; }
        }
        public class IdItemNameNew
        {
            /// <summary>
            /// Gets or sets the identifier.
            /// </summary>
            /// <value>The identifier.</value>
            public string Id { get; set; }

            /// <summary>
            /// Gets or sets the name of the item.
            /// </summary>
            /// <value>The name of the item.</value>
            public string ItemName { get; set; }
            public string SIANumber { get; set; }
        }
    }
}
