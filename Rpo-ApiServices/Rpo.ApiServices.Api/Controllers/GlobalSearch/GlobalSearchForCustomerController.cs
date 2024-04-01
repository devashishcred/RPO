using Rpo.ApiServices.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Rpo.ApiServices.Api.Controllers.GlobalSearch
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using System.Xml;
    using Api.Jobs;
    using Enums;
    using Filters;
    using Rpo.ApiServices.Model;
    public class GlobalSearchForCustomerController : ApiController
    {
        private RpoContext db = new RpoContext();
        /// <summary>
        /// Gets the global search.
        /// </summary>
        /// <param name="globalSearchType">Type of the global search.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns>GlobalSearchResponse.</returns>
      //   [HttpGet]
        public GlobalSearchResponse GetGlobalSearchForCustomer(int globalSearchType, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return new GlobalSearchResponse();
            }
            var employee = db.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            GlobalSearchResponse globalSearchResponse = new GlobalSearchResponse();
            switch ((GlobalSearchType)globalSearchType)
            {
                //Search by the Application Number in to jobapplication
                //case GlobalSearchType.ApplicationNumber:
                //    int dobApplicationType = ApplicationType.DOB.GetHashCode();
                //    var application = db.JobApplications.Include("JobApplicationType").Where(x => x.ApplicationNumber.Contains(searchText.Trim()) && x.JobApplicationType != null && x.JobApplicationType.IdParent == dobApplicationType).Select(x => x.IdJob).Distinct().ToList();
                //    globalSearchResponse.SearchResult = application;
                //    break;
                // get the result of selected companies wise 
                case GlobalSearchType.CompanyName:
                    var company = db.Companies.Where(x => x.Name.Contains(searchText.Trim())).Select(x => x.Id).ToList();
                    globalSearchResponse.SearchResult = company;
                    break;
                // get the result of selected contact wise
                case GlobalSearchType.ContactName:
                    var contact = db.Contacts.AsEnumerable();
                    searchText = searchText.ToLower();
                    foreach (var item in searchText.Split(' '))
                    {
                        contact = contact.Where(x =>
                           (x.FirstName != null && x.FirstName.ToLower().Contains(item.Trim().ToLower()))
                        || (x.LastName != null && x.LastName.ToLower().Contains(item.Trim().ToLower()))
                        || (x.MiddleName != null && x.MiddleName.ToLower().Contains(item.Trim().ToLower())));
                    }

                    globalSearchResponse.SearchResult = contact.Select(x => x.Id).ToList();
                    break;
                // get the result of selected jobnumber
                case GlobalSearchType.JobNumber:
                    //var job = db.Jobs.Where(x => x.JobNumber.Contains(searchText.Trim())).Select(x => x.Id).ToList();
                    //globalSearchResponse.SearchResult = job;
                    //break;
                    var job = db.Jobs.Where(x => x.JobNumber.Contains(searchText.Trim())).Select(x => x.Id).ToList();
                    var customerjobaccess = db.CustomerJobAccess.Where(x => x.IdCustomer == employee.Id).ToList().Select(y => y.IdJob);

                    List<int> matchingjobs = new List<int>();
                        foreach (var j in job)
                    {
                        if (customerjobaccess.Contains(j))
                        {
                            matchingjobs.Add(j);
                        }
                    }
                    globalSearchResponse.SearchResult = matchingjobs;
                    break;
                // get the result of selected RFPNumber
                case GlobalSearchType.RFPNumber:
                    var rfp = db.Rfps.Where(x => x.RfpNumber.Contains(searchText.Trim())).Select(x => x.Id).ToList();
                    globalSearchResponse.SearchResult = rfp;
                    break;
                // get the result of selected Address
                case GlobalSearchType.Address:
                    string[] strArray = searchText.Split(',');
                    string address = string.Empty;
                    string borough = string.Empty;
                    //if(strArray.Count()>0)
                    //{
                    //    address = searchText.Split(',')[0].Trim();
                    //}
                    //if (strArray.Count() > 1)
                    //{
                    //    borough = searchText.Split(',')[1].Trim();
                    //}
                    var jobAddress = db.Jobs.Include("RfpAddress.Borough").Select(x => new
                    {
                        Id = x.Id,
                        //HouseNumber = (x.RfpAddress != null ? x.RfpAddress.HouseNumber : string.Empty),
                        //Street = (x.RfpAddress != null ? x.RfpAddress.Street : string.Empty),
                        Borough = (x.RfpAddress != null && x.RfpAddress.Borough != null ? x.RfpAddress.Borough.Description : string.Empty).Trim(),
                        ZipCode = (x.RfpAddress != null ? x.RfpAddress.ZipCode : string.Empty),
                        NewAddress = (x.RfpAddress != null ? x.RfpAddress.HouseNumber + " " + x.RfpAddress.Street : string.Empty).Trim()
                        // }).Where(x => x.HouseNumber.Trim().ToLower().Equals(searchText.Trim().ToLower()) || x.Street.Trim().ToLower().Equals(searchText.Trim().ToLower()) || (x.NewAddress+", "+ x.Borough).ToLower().Trim().Equals(searchText.ToLower().Trim()) || x.ZipCode.Trim().ToLower().Equals(searchText.Trim().ToLower()) || x.Borough.Trim().ToLower().Equals((!string.IsNullOrEmpty(searchText) ? searchText.Trim().ToLower() : ""))).AsQueryable();
                    }).Where(x => (x.NewAddress + ", " + x.Borough).ToLower().Trim().Contains(searchText.ToLower().Trim())).AsQueryable();

                    #region ML Changes
                    //var jobAddress = db.Jobs.Include("RfpAddress.Borough").AsQueryable();
                    //foreach (var item in searchText.Split(','))
                    //{
                    //    jobAddress = jobAddress.Where(x =>
                    //       (x.RfpAddress != null && x.RfpAddress.HouseNumber.Contains(item.Trim()))
                    //    || (x.RfpAddress != null && x.RfpAddress.Street.Contains(item.Trim()))
                    //    || (x.RfpAddress != null && x.RfpAddress.Borough != null && x.RfpAddress.Borough.Description.Contains(item.Trim()))
                    //    || (x.RfpAddress != null && x.RfpAddress.ZipCode.Contains(item.Trim())));
                    //}
                    #endregion 
                    globalSearchResponse.SearchResult = jobAddress.Select(x => x.Id).ToList();

                    break;
                // get the result of selected specialplaceName
                case GlobalSearchType.SpecialPlaceName:
                    var jobSpecialPlace = db.Jobs.Where(x => x.SpecialPlace.Contains(searchText.Trim())).Select(x => x.Id).ToList();
                    globalSearchResponse.SearchResult = jobSpecialPlace;
                    break;
                // get the result of selected TransmittalNo
                case GlobalSearchType.TransmittalNumber:
                    var jobTransmittal = db.JobTransmittals.Where(x => x.TransmittalNumber.Contains(searchText.Trim()) && (x.IsDraft == false || x.IsDraft == null)).Select(x => x.IdJob).Distinct().ToList();
                    globalSearchResponse.SearchResult = jobTransmittal;
                    break;
                // get the result of selected ZoneDistrict of RFP address
                case GlobalSearchType.ZoneDistrict:
                    var jobZoneDistrict = db.Jobs.Include("RfpAddress").Where(x => x.RfpAddress.ZoneDistrict.Contains(searchText.Trim())).Select(x => x.Id).ToList();
                    globalSearchResponse.SearchResult = jobZoneDistrict;
                    break;
                // get the result of selected Overlay of RFP address
                case GlobalSearchType.Overlay:
                    var jobOverlay = db.Jobs.Include("RfpAddress").Where(x => x.RfpAddress.Overlay.Contains(searchText.Trim())).Select(x => x.Id).ToList();
                    globalSearchResponse.SearchResult = jobOverlay;
                    break;
                // get the result of selected Tracking number
                //case GlobalSearchType.TrackingNumber:
                //    int dotApplicationType = ApplicationType.DOT.GetHashCode();
                //    var tracking = db.JobApplications.Include("JobApplicationType").Where(x => x.ApplicationNumber.Contains(searchText.Trim()) && x.JobApplicationType != null && x.JobApplicationType.IdParent == dotApplicationType).Select(x => x.IdJob).Distinct().ToList();
                //    globalSearchResponse.SearchResult = tracking;
                //    break;
                // get the result of selected violation number
                case GlobalSearchType.ViolationNumber:
                    //var violation = db.JobViolations.Where(x => x.SummonsNumber.Contains(searchText.Trim())).Select(x => x.IdJob ?? 0).Distinct().ToList();
                    //globalSearchResponse.SearchResult = violation;
                    //break;
                    var jobViolation = db.JobViolations.Where(x => x.SummonsNumber.Contains(searchText.Trim())).Distinct().ToList();
                    foreach (var j in jobViolation)
                    {
                        var rfpaddresses = db.RfpAddresses.Where(x => j.BinNumber == x.BinNumber).ToList().Select(x => x.Id);
                        var idjobs = rfpaddresses != null ? db.Jobs.Where(x => rfpaddresses.Contains(x.IdRfpAddress)).ToList().Select(y => y.Id) : null;
                        var projectaccess = db.CustomerJobAccess.Where(x => x.IdCustomer == employee.Id).Where(z => idjobs.Contains(z.IdJob)).Select(y => y.IdJob).ToList();
                    }
                break;
                // get the result of selected Task number
                case GlobalSearchType.Task:
                    var task = db.Tasks.Where(x => x.TaskNumber.Contains(searchText.Trim())).Select(x => x.Id).ToList();
                    globalSearchResponse.SearchResult = task;
                    break;
            }

            return globalSearchResponse;
        }
    }
}