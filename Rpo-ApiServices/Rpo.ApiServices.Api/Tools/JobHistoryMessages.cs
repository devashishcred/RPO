// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-12-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-16-2018
// ***********************************************************************
// <copyright file="JobHistoryMessages.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job History Messages.</summary>
// ***********************************************************************

/// <summary>
/// The Tools namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Tools
{
    /// <summary>
    /// Class Job History Messages.
    /// </summary>
    public class JobHistoryMessages
    {
        ///// <summary>
        ///// The add application message
        ///// </summary>
        //public static string AddApplicationMessage = "Created a new Application of {0} type";

        public static string NoSetstring = "'Not Set'";

        public static string AddApplication_DOB = "New DOB application of type ##ApplicationType## has been created. Application status is ##ApplicationStatus##. Application number is ##ApplicationNumber##.";
        public static string EditApplicationNumber_DOB = "DOB application of type ##ApplicationType## has been updated. Application status is  ##ApplicationStatus##. Application number is ##ApplicationNumber##.";
      //  public static string EditApplicationStatus_DOB = "DOB application of type ##ApplicationType## has been updated. Application status is  ##ApplicationNumber##. Application number is ##ApplicationStatus##";
        public static string DeleteApplication_DOB = "DOB application# ##ApplicationNumber## of type ##ApplicationType## has been deleted.";

        // public static string AddApplication_DOB = "New DOB application of type ##ApplicationType## has been created. Application status is ##ApplicationStatus##. Application number is ##ApplicationNumber##";
        // public static string EditApplicationNumber_DOB = "DOB application of type ##ApplicationType## has been updated with application number ##ApplicationNumber##. Application status is ##ApplicationStatus##";
        //public static string EditApplicationStatus_DOB = "Status of DOB application# ##ApplicationNumber## of type ##ApplicationType## has been updated to ##NewApplicationStatus## from ##OldApplicationStatus##";
        // public static string DeleteApplication_DOB = "DOB application# ##ApplicationNumber## of type ##ApplicationType## has been deleted";

        public static string AddApplication_DOT = "New DOT location of type ##ApplicationType## has been created. Location is ##LocationDetails##. Status of the Location is ##ApplicationStatus##. Tracking# for the location is ##Tracking##.";
        public static string EditApplicationLocation_DOT = "DOT location of type ##ApplicationType## has been updated. Location is ##LocationDetails##. Status of the Location is ##ApplicationStatus##. Tracking# for the location is ##Tracking##.";
        public static string DeleteApplication_DOT = "DOT location of type ##ApplicationType## has been deleted. Location is  ##LocationDetails##. Status of the Location is ##ApplicationStatus##. Tracking# for the location is ##Tracking##.";

        //public static string AddApplication_DOT = "New DOT location ##LocationDetails## of type ##ApplicationType## has been created. Status of the Location is ##ApplicationStatus##.Tracking# for the location is ##Tracking##";
        //public static string EditApplicationLocation_DOT = "DOT location ##OldLocationDetails## with tracking# ##ApplicationNumber## of type ##ApplicationType## has been updated to ##NewLocationDetails##. Location status is ##ApplicationStatus##";
        //public static string EditLocationStatus_DOT = "Status of DOT location ##LocationDetails## with tracking# ##ApplicationNumber## of type ##ApplicationType## has been updated to ##ApplicationStatus##";
        //public static string DeleteApplication_DOT = "DOT location of type ##LocationDetails## with tracking# ##ApplicationNumber## of type ##ApplicationType## has been deleted";

        public static string AddApplication_DEP = "New DEP application of type ##ApplicationType## has been created. Application status is ##ApplicationStatus##. Location is ##LocationDetails##. Work description is ##Description##.";
        public static string EditApplication_DEP = "DEP application of type ##ApplicationType## has been updated. Application status is ##ApplicationStatus##. Location is ##LocationDetails##. Work description is ##Description##.";
        public static string DeleteApplication_DEP = "DEP application of type ##ApplicationType## has been deleted. Location is ##LocationDetails##.";
        //public static string AddApplication_DEP = "New DEP application of type ##ApplicationType## has been created. Application status is ##ApplicationStatus##";
        //public static string EditApplicationStatus_DEP = "Status of DEP application of type ##ApplicationType## has been updated to ##NewApplicationStatus## from ##OldApplicationStatus##";
        //public static string DeleteApplication_DEP = "DEP application of type ##ApplicationType## has been deleted";

        public static string AddApplication_Violation = "New Violation ##Violation## has been created.";
        public static string EditApplication_Violation = "Violation ##Violation## has been updated.";
        public static string DeleteApplication_Violation = "Violation ##Violation## has been deleted.";


        public static string AddWorkPermit_DOB = "DOB Permit of type ##PermitType## created. Application Type is ##ApplicationType##. Application Number is ##ApplicationNumber##. Estimated Cost is ##EstimatedCost##. Work Description is ##WorkDescription##. Permit Responsibility is for ##RPOORPersonName##. Permit Filed on ##FiledDate##. Permit Issued on ##IssuedDate##. Permit Expiry on ##ExpiryDate##. Permittee is ##Permittee##.";
        public static string EditWorkPermit_DOB = "DOB Permit of type ##PermitType## updated. Application Type is ##ApplicationType##. Application Number is ##ApplicationNumber##. Estimated Cost is ##EstimatedCost##. Work Description is ##WorkDescription##. Permit Responsibility is for ##RPOORPersonName##. Permit Filed on ##FiledDate##. Permit Issued on ##IssuedDate##. Permit Expiry on ##ExpiryDate##. Permittee is ##Permittee##.";
        public static string DeleteWorkPermit_DOB = "DOB Permit of type ##PermitType## deleted. Application type is ##ApplicationType##. Application number is ##ApplicationNumber##.";
        //public static string AddWorkPermit_DOB = "DOB Permit of type ##PermitType## Created. Estimated Cost is ##EstimatedCost##.Work Description is ##WorkDescription##.Permit Responsibility is for ##RPOORPersonName## Permit Filed on ##FiledDate## Permit Issued on ##IssuedDate## Permit Expiry on ##ExpiryDate## Permittee is ##Permittee##";
        //public static string UpdateWorkPermit_DOB = "DOB Permit of type ##PermitType## Updated. Estimated Cost is ##EstimatedCost##.Work Description is ##WorkDescription##.Permit Responsibility is for ##RPOORPersonName## Permit Filed on ##FiledDate## Permit Issued on ##IssuedDate## Permit Expiry on ##ExpiryDate## Permittee is ##Permittee##";
        // public static string DeleteWorkPermit_DOB = "DOB permit of type ##PermitType## has been deleted for application# ##ApplicationNumber##";

        public static string AddWorkPermit_DOT = "New DOT permit# ##PermitNumber## of type ##PermitType## has been created. Location is ##LocationDetails##. Tracking# is ##ApplicationNumber##.";
        public static string EditWorkPermit_DOT = "DOT permit# ##OldPermitNumber## of type ##PermitType## has been updated. New Permit # is ##NewPermitNumber##. Location is ##LocationDetails##. Tracking# is ##ApplicationNumber##.";
        public static string CompleteWorkPermit_DOT = "DOT permit# ##PermitNumber## of type ##PermitType## has been completed. Location is ##LocationDetails##. Tracking# is ##ApplicationNumber##.";
        // public static string AddWorkPermit_DOT = "New DOT permit# ##PermitNumber## of type ##PermitType## has been created for tracking# ##ApplicationNumber## of location ##LocationDetails##";
        //public static string EditWorkpermitMessage = "DOT permit# of type ##permit_type## has been updated to ##new_value## from ##old_value## for tracking# ##tracking## of location ##location##";
        //public static string DeleteWorkPermit_DOT = "DOT permit# ##PermitNumber## of type ##PermitType## has been delete for tracking# ##ApplicationNumber## of location ##LocationDetails##";

        public static string AddWorkPermit_DEP = "New DEP permit# ##PermitNumber## of type ##PermitType## has been created. This permit is for DEP application type ##ApplicationType##. Location is ##LocationDetails##. Permit Issued on ##PermitIssued##. Permit Expiry Date is ##PermitExpiry##.";
        public static string EditWorkPermit_DEP = "DEP permit# ##PermitNumber## of type ##PermitType## has been updated. This permit is for DEP application type ##ApplicationType##. Location is ##LocationDetails##. Permit Issued on ##PermitIssued##. Permit Expiry Date is ##PermitExpiry##.";
        public static string DeleteWorkPermit_DEP = "DEP permit# ##PermitNumber## of type ##PermitType## has been deleted. This permit is for DEP application type ##ApplicationType##. Location is ##LocationDetails##.";
        //public static string AddWorkPermit_DEP = "New DEP permit of type ##PermitType## has been created";
        //public static string DeleteWorkPermit_DEP = "DEP permit of type ##PermitType## has been deleted";

        public static string AddJob = "Job# ##jobnumber## Created. Job Address is ##JobAddress##. Floor is ##Floor##. Apartment is ##Apartment##. Project Description is ##ProjectDescription##. Job Type includes ##JobType##. ##ProjectTeam## ##RFP##";
        public static string EditJob = "Job# ##jobnumber## Updated. Job Address is ##JobAddress##. Floor is ##Floor##. Apartment is ##Apartment##. Project Description is ##ProjectDescription##. Job Type includes ##JobType##. ##ProjectTeam## ";
        public static string JobStatusChangeMessage = "Job status has been updated to ##NewJobStatus## from ##OldJobStatus## : ##Reason##";
        public static string JobStatusCompletedMessage = "Job status has been updated to ##NewJobStatus## from ##OldJobStatus##";
        public static string RFPLinkMessage = "RFP# ##RFP## has been mapped to the Job.";
        //public static string JobStatusChangeMessage = "Job status has been updated to ##NewJobStatus## from ##OldJobStatus## ##Reason##";
        //public static string AddJob = "##jobnumber## Created. Job Address is ##JobAddress## | Floor is ##Floor## | Apartment is ##Apartment## Job Type is ##JobType## Project Team includes ##Team## ";

        public static string AddContact = "Job Contact ##JobContactName## added to the Job. Contact Type is ##ContactType##. Mailing address of the contact is ##MailingAddress##. ##MainClientBillingClient##";
        public static string UpdateContact = "Job Contact ##JobContactName## updated. Contact type is ##ContactType##. Mailing address of the contact is ##MailingAddress##. ##MainClientBillingClient##";
        public static string DeleteContact = "Job contact ##JobContactName## (##ContactType##) has been deleted from the job.";
        //public static string AddContact = "Job Contact ##JobContactName## added to the Job. Contact Type is ##ContactType##. Mailing address of the contact is ##MailingAddress##. Contact is set as ##MainClientBillingClient##";
        //public static string UpdateContact = "Job contact ##JobContactName## (##ContactType##) has been updated to the job as ##MainClientBillingClient## with mailing address ##MailingAddress##";
        //public static string DeleteContact = "Job contact ##JobContactName## (##ContactType##) has been deleted from the job";


        public static string AddScope = "New Service Item ##ServiceItemName## added to the job scope.";
        public static string DeleteScope = "Service item ##ServiceItemName## is removed form the job scope.";
        public static string ChangeStatusScopeMessage = "Service item ##ServiceItemName## has been updated to Complete.";
        //public static string DeleteScope = "Service Item ##ServiceItemName## of Billing Point ##BillingPointName## with PO# ##PONumber## and Invoice# ##InvoiceNumber## has been deleted from the job";
        //public static string AddScope = "New Service Item ##ServiceItemName## has been added to the job scope. Quantity added is ##ServiceItemQuantity##";
        //public static string ChangeStatusScopeMessage = "##hours## has been logged for Scope Item ##ServiceItemName## as additional Services";

        public static string AddMilestone = "New Billing Point ##BillingPointName## has been added to the job.";
        public static string EditMilestone = "Billing Point ##BillingPointName## has been updated.";
        public static string DeleteMilestone = "Billing Point ##BillingPointName## has been deleted.";
        public static string ChangeStatusMilestone = "Billing Point ##BillingPointName## status has been updated to ##NewStatus## from ##OldStatus## ";
        //public static string ChangeStatusMilestone = "Status of Billing Point ##BillingPointName## has been updated to ##NewStatus## from ##OldStatus##";
        //public static string DeleteMilestone = "Billing Point ##BillingPointName## with PO# ##PONumber## and Invoice# ##InvoiceNumber## has been deleted from the job";
        //public static string EditMilestone = "Billing Point ##BillingPointName## with PO# ##PONumber## and Invoice# ##InvoiceNumber## has been updated to  ##NewValue## from ##OldValue##";
        //public static string AddMilestone = "New Billing Point ##BillingPointName## has been added to the job";

        public static string TransmittalSend = "Transmittal# ##TransmittalNumber## has been sent to ##TOContactName## via ##SentVia## with subject ##Subject##.";
        public static string TransmittalDelete = "Transmittal# ##TransmittalNumber## sent to ##TOContactName## via ##SentVia## with subject ##Subject## has been deleted from the job.";
        //public static string TransmittalSend = "Transmittal# ##TransmittalNumber## has been sent to ##TOContactName## of ##TOCompanyName## via ##SentVia## with subject ##Subject##";
        //public static string TransmittalDelete = "Transmittal# ##TransmittalNumber## sent to ##TOContactName## via ##SentVia## with subject ##Subject## has been deleted from the job";



        public static string UpdateApplication_DOB = "Job# ##jobnumber## DOB Application Number ##applicationnumber## status update from ##oldstatus## to ##newstatus##";

        public static string UpdateApplication_DOBCronjob = "DOB Application Status Updated: Job# ##jobnumber## - ##jobaddress## <##ApplicationType##> - <##ApplicationNumber##> status updated from '##oldstatus##' to '##newstatus##'";

        public static string UpdateApplication_Permit_DOBCronjob = "DOB Permit Status Updated: Job# ##jobnumber## - ##jobaddress## -<##ApplicationNumber##> - ##Permit##. ##Updateddates##.";






        //"New DOT location ##LocationDetails## of type ##ApplicationType## has been created.Status of the Location is ##ApplicationStatus##";

        //"New DOT location ##LocationDetails## of type ##ApplicationType## has been created.Status of the Location is ##ApplicationStatus##";








        public static string InvoiceDateMilestone = "Invoice date of Invoice# ##InvoiceNumber## has been updated to ##NewInvoiceDate## from ##OldInvoiceDate## for billing point ##BillingPointName##";

        public static string InvoiceNumberMilestone = "Invoice# ##InvoiceNumber## has been updated to ##NewInvoiceNumber## from ##OldInvoiceNumber## for billing point ##BillingPointName##";

        public static string PONumberMilestone = "PO# ##PONumber## has been updated to ##NewPONumber## from ##OldPONumber## for billing point ##BillingPointName##";


        public static string InvoiceDateScope = "Invoice date of Invoice# ##InvoiceNumber## has been updated to ##NewInvoiceDate## from ##OldInvoiceDate## for service item ##ServiceItemName##";

        public static string InvoiceNumberScope = "Invoice# ##InvoiceNumber## has been updated to ##NewInvoiceNumber## from ##OldInvoiceNumber## for service item ##ServiceItemName##";

        public static string PONumberScope = "PO# ##PONumber## has been updated to ##NewPONumber## from ##OldPONumber## for service item ##ServiceItemName##";

       public static string ChangeStatusScope = "##Hours## has been logged for Service Item ##ServiceItemName##. Status of the service item has been updated to ##NewServiceItemStatus## with reference to Timenote added in job# <a href=\"##RedirectionLinkJob##\">##JobNumber##</a>";
        










        // public static string AddWorkPermit_DOB = "DOB Permit of type ##PermitType## Created. Estimated Cost is ##EstimatedCost##.Work Description is ##WorkDescription##.Permit Responsibility is for ##RPOORPersonName## Permit Filed on ##FiledDate## Permit Issued on ##IssuedDate## Permit Expiry on ##ExpiryDate## Permittee is ##Permittee##";//"New DOB permit of type ##PermitType## has been created for application# ##ApplicationNumber##";
        //public static string UpdateWorkPermit_DOB = "DOB Permit of type ##PermitType## Updated. Estimated Cost is ##EstimatedCost##.Work Description is ##WorkDescription##.Permit Responsibility is for ##RPOORPersonName## Permit Filed on ##FiledDate## Permit Issued on ##IssuedDate## Permit Expiry on ##ExpiryDate## Permittee is ##Permittee##";//"New DOB permit of type ##PermitType## has been created for application# ##ApplicationNumber##";











        public static string TimeJobEntries = "##hours## out of ##totalhours## has been logged for Scope Item ##ServiceItemName## with reference to Task# ##Task_ID## of type ##task_type##";

        public static string DOBExpiry = "DOB Tracking number/insurance dates for the company ##CompanyName## are about to expire in 30 days. This company is part of the following jobs ##jobs##";
        public static string DOTExpiry = "DOT Tracking number/insurance dates for the company ##CompanyName## are about to expire in 30 days. This company is part of the following jobs ##jobs##";
        /// <summary>
        /// The edit application message
        /// </summary>
        //public static string EditApplicationMessage = "Edited the Application of {0} type";

        /// <summary>
        /// The delete application message
        /// </summary>
        //public static string DeleteApplicationMessage = "Deleted the Application of {0} type";

        /// <summary>
        /// The add workpermit message
        /// </summary>
        public static string AddWorkpermitMessage = "Created the new Work Permit of {0} type for application of {1} type";

        /// <summary>
        /// The edit workpermit message
        /// </summary>
        

        /// <summary>
        /// The delete workpermit message
        /// </summary>
        //public static string DeleteWorkpermitMessage = "Deleted the Work Permit of {0} type for Application of {1} type";



        /// <summary>
        /// The add milestone without type message
        /// </summary>
        //public static string AddMilestoneMessage = "Added a new billing point";




        /// <summary>
        /// The edit milestone without type message
        /// </summary>
        //public static string EditMilestoneMessage = "Edited the billing point";

        /// <summary>
        /// The change status milestone message
        /// </summary>
        //public static string ChangeStatusMilestoneMessage = "Change Status of a billing point";


        /// <summary>
        /// The delete milestone without type message
        /// </summary>
        //public static string DeleteMilestoneMessage = "Deleted the billing point";

        /// <summary>
        /// The invoice date milestone message
        /// </summary>
        //public static string InvoiceDateMilestoneMessage = "Invoice Date is updated in billing point";

        /// <summary>
        /// The invoice number milestone message
        /// </summary>
        //public static string InvoiceNumberMilestoneMessage = "Invoice number is updated in billing point";

        /// <summary>
        /// The PO number milestone message
        /// </summary>
        //public static string PONumberMilestoneMessage = "PO number is updated in billing point";

        /// <summary>
        /// The add scope message
        /// </summary>
        //public static string AddScopeMessage = "Added new scope";

        /// <summary>
        /// The edit scope message
        /// </summary>
        //public static string EditScopeMessage = "Edited the scope";

        /// <summary>
        /// The delete scope message
        /// </summary>
        //public static string DeleteScopeMessage = "Deleted the scope";

        /// <summary>
        /// The invoice date scope message
        /// </summary>
        //public static string InvoiceDateScopeMessage = "Invoice Date is updated in scope";

        /// <summary>
        /// The invoice number scope message
        /// </summary>
        //public static string InvoiceNumberScopeMessage = "Invoice number is updated in scope";

        /// <summary>
        /// The po number scope message
        /// </summary>
        //public static string PONumberScopeMessage = "PO number is updated in scope";

        /// <summary>
        /// The change status scope message
        /// </summary>

        /// <summary>
        /// The add contact message
        /// </summary>
        //public static string AddContactMessage = "New job contact <job_contact_name> (<contact_type>) has been added to the job as <main client / billing client> with mailing address";

        /// <summary>
        /// The edit contact message
        /// </summary>
        public static string EditContactMessage = "Edited the contact";

        /// <summary>
        /// The delete contact message
        /// </summary>
        //public static string DeleteContactMessage = "Deleted the contact";

        /// <summary>
        /// The add job message
        /// </summary>
        //public static string AddJobMessage = "Created the job";

        /// <summary>
        /// The edit job message
        /// </summary>
        //public static string EditJobMessage = "Edited the job details";

        /// <summary>
        /// The deleted job message
        /// </summary>
        //public static string DeletedJobMessage = "Deleted the job";

        /// <summary>
        /// The job status change message
        /// </summary>
        //public static string JobStatusChangeMessage = "Status of the job is changed to {0}";

        /// <summary>
        /// The transmittal send message
        /// </summary>
        //public static string TransmittalSendMessage = "New Job Transmittal is sent";

        /// <summary>
        /// The transmittal delete message
        /// </summary>
        //public static string TransmittalDeleteMessage = "Job Transmittal is deleted";
    }
}