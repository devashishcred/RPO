// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-30-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-30-2018
// ***********************************************************************
// <copyright file="GlobalSearchController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Global Search Controller</summary>
// ***********************************************************************

/// <summary>
/// The GlobalSearch namespace.
/// </summary>
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

    /// <summary>
    /// Class Global Search Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class GlobalSearchController : ApiController
    {
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the global search.
        /// </summary>
        /// <param name="globalSearchType">Type of the global search.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns>GlobalSearchResponse.</returns>
        public GlobalSearchResponse GetGlobalSearch(int globalSearchType, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return new GlobalSearchResponse();
            }

            GlobalSearchResponse globalSearchResponse = new GlobalSearchResponse();
            switch ((GlobalSearchType)globalSearchType)
            {
                //Search by the Application Number in to jobapplication
                case GlobalSearchType.ApplicationNumber:
                    int dobApplicationType = ApplicationType.DOB.GetHashCode();
                    var application = db.JobApplications.Include("JobApplicationType").Where(x => x.ApplicationNumber.Contains(searchText.Trim()) && x.JobApplicationType != null && x.JobApplicationType.IdParent == dobApplicationType).Select(x => x.IdJob).Distinct().ToList();
                    globalSearchResponse.SearchResult = application;
                    break;
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
                    var job = db.Jobs.Where(x => x.JobNumber.Contains(searchText.Trim())).Select(x => x.Id).ToList();
                    globalSearchResponse.SearchResult = job;
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
                    globalSearchResponse.SearchResult = jobAddress.Select(x=>x.Id).ToList();

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
                case GlobalSearchType.TrackingNumber:
                    int dotApplicationType = ApplicationType.DOT.GetHashCode();
                    var tracking = db.JobApplications.Include("JobApplicationType").Where(x => x.ApplicationNumber.Contains(searchText.Trim()) && x.JobApplicationType != null && x.JobApplicationType.IdParent == dotApplicationType).Select(x => x.IdJob).Distinct().ToList();
                    globalSearchResponse.SearchResult = tracking;
                    break;
                // get the result of selected violation number
                case GlobalSearchType.ViolationNumber:
                    var violation = db.JobViolations.Where(x => x.SummonsNumber.Contains(searchText.Trim())).Select(x => x.IdJob ?? 0).Distinct().ToList();
                    globalSearchResponse.SearchResult = violation;
                    break;
                // get the result of selected Task number
                case GlobalSearchType.Task:
                    var task = db.Tasks.Where(x => x.TaskNumber.Contains(searchText.Trim())).Select(x => x.Id).ToList();
                    globalSearchResponse.SearchResult = task;
                    break;
            }

            return globalSearchResponse;
        }

        ///// <summary>
        ///// Manuals the crone submitted RFP.
        ///// </summary>
        ///// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        //[HttpGet]
        //[Authorize]
        //[RpoAuthorize]
        //[Route("api/GlobalSearch/ManualCroneSubmittedRFP")]
        //public bool ManualCroneSubmittedRFP()
        //{
        //    RfpTwoDaysInSubmittedUserNotificationJob.SendMailNotification();
        //    return true;
        //}

        ///// <summary>
        ///// Manuals the crone in draft RFP.
        ///// </summary>
        ///// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        //[HttpGet]
        //[Authorize]
        //[RpoAuthorize]
        //[Route("api/GlobalSearch/ManualCroneInDraftRFP")]
        //public bool ManualCroneInDraftRFP()
        //{
        //    RfpTwoDaysInDraftUserNotificationJob.SendMailNotification();
        //    return true;
        //}

        ///// <summary>
        ///// Manuals the crone in draft admin RFP.
        ///// </summary>
        ///// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        //[HttpGet]
        //[Authorize]
        //[RpoAuthorize]
        //[Route("api/GlobalSearch/ManualCroneInDraftAdminRFP")]
        //public bool ManualCroneInDraftAdminRFP()
        //{
        //    RfpTwoDaysInDraftJob.SendMailNotification();
        //    return true;
        //}

        ///// <summary>
        ///// Manuals the crone in task reminder RFP.
        ///// </summary>
        ///// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        //[HttpGet]
        //[Authorize]
        //[RpoAuthorize]
        //[Route("api/GlobalSearch/ManualCroneInTaskReminderRFP")]
        //public bool ManualCroneInTaskReminderRFP()
        //{
        //    TaskReminderJob.SendMailNotification();
        //    return true;
        //}

        ///// <summary>
        ///// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        ///// </summary>
        ///// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        ///// <summary>
        ///// Creates the XML for customer.
        ///// </summary>
        ///// <returns>System.String.</returns>
        //[HttpGet]
        //[Authorize]
        //[RpoAuthorize]
        //[Route("api/GlobalSearch/CreateXMLForCustomer")]
        //public string CreateXMLForCustomer()
        //{
        //    string strRequestXML = string.Empty;
        //    XmlDocument inputXMLDoc = null;
        //    inputXMLDoc = new XmlDocument();
        //    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", null, null));
        //    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"10.0\""));
        //    XmlElement quickBoosXML = inputXMLDoc.CreateElement("QBXML");
        //    inputXMLDoc.AppendChild(quickBoosXML);
        //    XmlElement quickBooksXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
        //    quickBoosXML.AppendChild(quickBooksXMLMsgsRq);
        //    quickBooksXMLMsgsRq.SetAttribute("onError", "stopOnError");
        //    XmlElement customerAddRq = inputXMLDoc.CreateElement("CustomerAddRq");
        //    customerAddRq.SetAttribute("requestID", "ProjectId1");
        //    quickBooksXMLMsgsRq.AppendChild(customerAddRq);
        //    XmlElement customerAdd = inputXMLDoc.CreateElement("CustomerAdd");
        //    customerAddRq.AppendChild(customerAdd);
        //    customerAdd.AppendChild(this.MakeSimpleElem(inputXMLDoc, "Name", "TestBusinessName TestProjectNumber"));
        //    customerAdd.AppendChild(this.MakeSimpleElem(inputXMLDoc, "CompanyName", "TestBusinessName TestProjectNumber"));
        //    XmlElement billAddress = inputXMLDoc.CreateElement("BillAddress");
        //    customerAdd.AppendChild(billAddress);
        //    billAddress.AppendChild(this.MakeSimpleElem(inputXMLDoc, "Addr1", "TestAddress1 TestAddress1"));
        //    billAddress.AppendChild(this.MakeSimpleElem(inputXMLDoc, "Addr2", "TestAddress2 TestAddress2"));
        //    billAddress.AppendChild(this.MakeSimpleElem(inputXMLDoc, "Addr3", "TestAddress3"));
        //    billAddress.AppendChild(this.MakeSimpleElem(inputXMLDoc, "City", "Test City"));
        //    billAddress.AppendChild(this.MakeSimpleElem(inputXMLDoc, "State", "Test State"));
        //    billAddress.AppendChild(this.MakeSimpleElem(inputXMLDoc, "PostalCode", "Postal"));
        //    strRequestXML = inputXMLDoc.OuterXml;

        //    return strRequestXML;
        //}

        ///// <summary>
        ///// Creates the XML for time notes.
        ///// </summary>
        ///// <returns>System.String.</returns>
        //[HttpGet]
        //[Authorize]
        //[RpoAuthorize]
        //[Route("api/GlobalSearch/CreateXMLForTimeNotes")]
        //public string CreateXMLForTimeNotes()
        //{
        //    string strRequestXML = string.Empty;
        //    XmlDocument inputXMLDoc = null;
        //    inputXMLDoc = new XmlDocument();
        //    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", null, null));
        //    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"10.0\""));
        //    XmlElement quickBoosXML = inputXMLDoc.CreateElement("QBXML");
        //    inputXMLDoc.AppendChild(quickBoosXML);
        //    XmlElement quickBooksXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
        //    quickBoosXML.AppendChild(quickBooksXMLMsgsRq);
        //    quickBooksXMLMsgsRq.SetAttribute("onError", "stopOnError");
        //    XmlElement timeTrackingAddRq = inputXMLDoc.CreateElement("TimeTrackingAddRq");
        //    timeTrackingAddRq.SetAttribute("requestID", "ProjectId1");
        //    quickBooksXMLMsgsRq.AppendChild(timeTrackingAddRq);
        //    XmlElement timeTrackingAdd = inputXMLDoc.CreateElement("TimeTrackingAdd");
        //    timeTrackingAddRq.AppendChild(timeTrackingAdd);
        //    timeTrackingAdd.AppendChild(this.MakeSimpleElem(inputXMLDoc, "TxnDate", DateTime.Now.ToString()));

        //    XmlElement entityRef = inputXMLDoc.CreateElement("EntityRef");
        //    timeTrackingAdd.AppendChild(entityRef);
        //    entityRef.AppendChild(this.MakeSimpleElem(inputXMLDoc, "FullName", "Prajesh H Baria"));

        //    XmlElement customerRef = inputXMLDoc.CreateElement("CustomerRef");
        //    timeTrackingAdd.AppendChild(customerRef);
        //    customerRef.AppendChild(this.MakeSimpleElem(inputXMLDoc, "FullName", "TestBusinessName TestProjectNumber"));

        //    XmlElement itemServiceRef = inputXMLDoc.CreateElement("ItemServiceRef");
        //    timeTrackingAdd.AppendChild(itemServiceRef);
        //    itemServiceRef.AppendChild(this.MakeSimpleElem(inputXMLDoc, "FullName", "Building Cost"));

        //    timeTrackingAdd.AppendChild(this.MakeSimpleElem(inputXMLDoc, "Duration", "PT2H0M0S"));

        //    //XmlElement payrollItemWageRef = inputXMLDoc.CreateElement("PayrollItemWageRef");
        //    //timeTrackingAdd.AppendChild(payrollItemWageRef);
        //    //payrollItemWageRef.AppendChild(this.MakeSimpleElem(inputXMLDoc, "FullName", "TestAddress1 TestAddress1"));

        //    timeTrackingAdd.AppendChild(this.MakeSimpleElem(inputXMLDoc, "Notes", "Test Notes"));

        //    //< !--BillableStatus may have one of the following values: Billable, NotBillable, HasBeenBilled-- >
        //    timeTrackingAdd.AppendChild(this.MakeSimpleElem(inputXMLDoc, "BillableStatus", "Billable"));

        //    strRequestXML = inputXMLDoc.OuterXml;

        //    return strRequestXML;
        //}

        ///// <summary>
        ///// Makes the simple elem.
        ///// </summary>
        ///// <param name="doc">The document.</param>
        ///// <param name="tagName">Name of the tag.</param>
        ///// <param name="tagVal">The tag value.</param>
        ///// <returns>XmlElement.</returns>
        //private XmlElement MakeSimpleElem(XmlDocument doc, string tagName, string tagVal)
        //{
        //    XmlElement elem = doc.CreateElement(tagName);
        //    elem.InnerText = tagVal;
        //    return elem;
        //}


        /// <summary>
        /// Manuals the crone in Job Violation 
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [HttpGet]
        [Route("api/GlobalSearch/ManualCroneJobViolationUpdate")]
        public bool ManualCroneJobViolationUpdate()
        {
            JobViolationUpdateResult.SendMailNotification();
            return true;
        }



        /// <summary>
        /// Manuals the crone in Company Expiry Date Update
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [HttpGet]
        [Route("api/GlobalSearch/ManualCroneJobCompanyExpiryUpdate")]
        public bool ManualCroneJobCompanyExpiryUpdate()
        {
            CompanyExpiryUpdate.SendMailNotification();
            return true;
        }

    }
}