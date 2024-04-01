using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Tools
{
    public class InAppNotificationMessage
    {
        public static string NewJobAssigned = "New job# <a href=\"##RedirectionLink##\">##JobNumber##</a> on address ##HouseStreetNameBorrough## ##SpecialPlaceName## has been created & assigned to you. The job is created for ##CompanyName## and contact ##ContactName##.";
        public static string NewMemberAddedToJobProjectTeam = "Job# <a href=\"##RedirectionLink##\">##JobNumber##</a> on address ##HouseStreetNameBorrough## ##SpecialPlaceName## has been assigned to you. The job is for ##CompanyName## and contact ##ContactName##. ##EmployeeName## added you to the job.";
        public static string JobPutOnHold = "Job - <a href=\"##RedirectionLink##\">##JobNumber##</a> at Address - ##HouseStreetNameBorrough## ##SpecialPlaceName## has been put on hold. The reason is ##StatusReason##. The job was put on hold by ##EmployeeName##.";
        public static string JobPutInProgressFromHold = "Job - <a href=\"##RedirectionLink##\">##JobNumber##</a> at Address - ##HouseStreetNameBorrough## ##SpecialPlaceName## has been put back in progress. The reason is ##StatusReason##. The job was put back in progress by ##EmployeeName##.";
        public static string JobMarkedCompleted = "Job - <a href=\"##RedirectionLink##\">##JobNumber##</a> at Address - ##HouseStreetNameBorrough## ##SpecialPlaceName## has been marked as complete by ##EmployeeName##.";
        public static string JobReOpen = "Job - <a href=\"##RedirectionLink##\">##JobNumber##</a> at Address - ##HouseStreetNameBorrough## ##SpecialPlaceName## has been reopened. The job was reopened by ##EmployeeName##.";
        public static string NewJobScopeAdded = "New Service Item ##ServiceItemName## has been added to Job# <a href=\"##RedirectionLink##\">##JobNumber##</a> at Address - ##HouseStreetNameBorrough## ##SpecialPlaceName##";
        public static string NewJobMileStoneAdded = "New Billing Point ##BillingPointName## is added to Job# <a href=\"##RedirectionLink##\">##JobNumber##</a> at Address - ##HouseStreetNameBorrough## ##SpecialPlaceName##";
        public static string ProgressNoteAddedToRFP = "Progress note - ##ProgressNote## added in RFP# <a href=\"##RedirectionLink##\">##RfpNumber##</a> at address ##RFPFullAddress##";
        public static string ProgressNoteAddedToJob = "Progress note - ##ProgressNote## added in Job# <a href=\"##RedirectionLink##\">##JobNumber##</a> at address ##JobFullAddress##";
        public static string NewTaskAssigned = "New Task# <a href=\"##RedirectionLink##\">##TaskNumber##</a> of type ##TaskType## with details ##TaskDetails## is assigned to you. Task was assigned by ##AssignBy##. The task is due on ##DueDate##.";
        public static string _NewTaskAssigned = "New Task# <a ##RedirectionLink##>##TaskNumber##</a> of type ##TaskType## with details ##TaskDetails## is assigned to you. Task was assigned by ##AssignBy##. The task is due on ##DueDate##.";
        //  public static string ProgressNoteAddedToTask = "Progress note ##ProgressNote## has been added in task# <a href=\"##RedirectionLink##\">##TaskNumber##</a> of type ##TaskType##, ##TaskDetails##.";
        public static string ProgressNoteAddedToTask = "Progress note ##ProgressNote## has been added in task# <a href=\"##RedirectionLink##\">##TaskNumber##</a> of type ##TaskType##.";
        public static string JobMilestoneCompleted = "Billing Point ##BillingPointName## of Job# <a href=\"##RedirectionLink##\">##JobNumber##</a> at Address - ##HouseStreetNameBorrough## ##SpecialPlaceName## is marked as complete.";
        public static string TaskCompleted = "Task# <a href=\"##RedirectionLink##\">##TaskNumber##</a> of type ##TaskType## with details ##TaskDetails## is marked as complete.";
        public static string _TaskCompleted = "Task# <a ##RedirectionLink##>##TaskNumber##</a> of type ##TaskType## with details ##TaskDetails## is marked as complete.";
        public static string TaskUnattainable = "Task# <a href=\"##RedirectionLink##\">##TaskNumber##</a> of type ##TaskType## with details ##TaskDetails## is marked as unattainable.";
        public static string _TaskUnattainable = "Task# <a ##RedirectionLink##>##TaskNumber##</a> of type ##TaskType## with details ##TaskDetails## is marked as unattainable.";
        public static string TaskIsDueBeforeTwoDays = ">##JobNumber## at ##JobAddress## | Your Task# <a href=\"##RedirectionLink##\">##TaskNumber##</a> of type ##TaskType## with details ##TaskDetails## is due on ##DueDate##.";
        //public static string TaskReminder = "Your Task# <a href=\"##RedirectionLink##\">##TaskNumber##</a> of type ##TaskType## with details ##TaskDetails## is due on ##DueDate##.";
        //  public static string TaskReminder = "Your Task# <a href=\"\" [routerLink]=\"##RedirectionLink##\" routerLinkActive=\"active\">##TaskNumber##</a> of type ##TaskType## with details ##TaskDetails## is due on ##DueDate##.";
        public static string TaskReminder = " This is a task reminder for ##Job## at ##JobAddress##. <a href=\"\" [routerLink]=\"##RedirectionLink##\" routerLinkActive=\"active\">##TaskNumber##</a> of type ##TaskType## with details ##TaskDetails## is due on ##DueDate##.";
        public static string RFPIsInDraftForMoreThan2Days = "RFP# <a href=\"##RedirectionLink##\">##RfpNumber##</a> for ##CompanyName## / ##ContactName## at address ##RFPFullAddress## has been in draft for more than 2 days.";
        public static string _RFPIsInDraftForMoreThan2Days = "RFP# <a ##RedirectionLink##>##RfpNumber##</a> for ##CompanyName## / ##ContactName## at address ##RFPFullAddress## has been in draft for more than 2 days.";
        public static string RFPSubmitForReview = "RFP# <a href=\"##RedirectionLink##\">##RfpNumber##</a> for ##CompanyName## / ##ContactName## at address ##RFPFullAddress## has been submitted for review by ##EmployeeName##.";
        public static string _RFPSubmitForReview = "RFP# <a ##RedirectionLink##>##RfpNumber##</a> for ##CompanyName## / ##ContactName## at address ##RFPFullAddress## has been submitted for review by ##EmployeeName##.";
        public static string ProposalIsReviewedAndMarkedForSending = "RFP# <a href=\"##RedirectionLink##\">##RfpNumber##</a> for ##CompanyName## / ##ContactName## on address ##RFPFullAddress## has been reviewed by ##ReviewerName## and can be submitted to client.";
        public static string _ProposalIsReviewedAndMarkedForSending = "RFP# <a ##RedirectionLink##>##RfpNumber##</a> for ##CompanyName## / ##ContactName## on address ##RFPFullAddress## has been reviewed by ##ReviewerName## and can be submitted to client.";
        public static string FollowupWithClient = "Proposal RFP# <a href=\"##RedirectionLink##\">##RfpNumber##</a> at address ##RFPFullAddress## was submitted to the client ##CompanyName##, ##ContactName## on ##SubmittedDate##. Please follow-up with the client to proceed further.";
        public static string RFPConvertedToJob = "RFP# <a href=\"##RedirectionLink##\">##RfpNumber##</a> for ##CompanyName## / ##ContactName## at address ##RFPFullAddress## has been converted to Job <a href=\"##RedirectionLinkJob##\">##JobNumber##</a>.";
        public static string HearingDateIsUpdatedBySystem = "Hearing date for Violation#  <a href=\"##RedirectionLink##\">##ViolationNumber##</a>  for Job# ##Job## at address - ##HouseStreetNameBorrough## ##SpecialPlaceName## has been updated from ##PreviousHearingDate## to ##NewHearingDate## on OATH";
        public static string StatusOfSummonsIsUpdatedBySystem = "Status of Summons for Violation# <a href=\"##RedirectionLink##\">##ViolationNumber##</a> for Job# ##Job## at address - ##HouseStreetNameBorrough## has been updated from ##PreviousStatus## to ##NewStatus## on OATH";
        //   public static string WhenCompanyExpiryDateUpdated = "##DateName## of Company ##CompanyName## has been updated from ##OldValue## to ##NewValue##";
        // public static string _WhenCompanyExpiryDateUpdated = "##DateName## of Company <a ##RedirectionLink##>##CompanyName##</a> has been updated from ##OldValue## to ##NewValue##";
        public static string _WhenCompanyExpiryDateUpdated = "<a href=\"##RedirectionLink##\">##CompanyName##</a> tracking number and insurance dates are updated by the system with reference to BIS. This company is part of the following jobs ##Jobs##";

        public static string _DOBPermit_Expiry = "Job# <a href=\"##RedirectionLink##\">##JobNumber##</a>: Permit ##PermitCode##, for Application type ##ApplicationType## and application # ##ApplicationNo## is about to expire in 30 days.";
        public static string _DEPPermit_Expiry = "Job# <a href=\"##RedirectionLink##\">##JobNumber##</a>: Permit ##PermitType##, Permit# ##PermitNo##, for DEP Application type ##ApplicationType## is about to expire in 30 days.";
        public static string _DOTPermit_Expiry = "Job# <a href=\"##RedirectionLink##\">##JobNumber##</a>: Permit ##PermitType##, Permit# ##PermitNo##, for DOT Location ##Location##, Tracking# ##TrackingNo## is about to expire in 30 days.";

        public static string UpdateViolation_Cronjob = "Violation Information Updated: Job# ##jobnumber## - ##jobaddress## Violation# <##ViolationNumber##> updated. ##CommanMessage## ";
        public static string UpdateECBViolation_Cronjob = "Violation Information Updated: Job# <a href=\"##RedirectionLink##\">##jobnumber##</a> - ##jobaddress## Violation# <a href=\"##RedirectionLinkForViolation##\"><##ViolationNumber##></a> updated. ##CommanMessage## ";
        //public static string ECBViolation_Cronjob = "Violation Information Updated: Violation# <##ViolationNumber##> updated for Job# ##jobnumbers##.";
        //public static string DOBViolation_Cronjob = "Violation Information Updated: Violation# <##ViolationNumber##> updated for Job# ##jobnumbers##.";
        public static string Violation_UpdateCronjob = "Violation Information Updated: Project# ##jobnumber## - ##jobaddress## Violation# <##ViolationNumber##> updated.##CommanMessage## ";
        public static string CustomerJobaccess= "Now You Have Access To Project ##jobnumber## Located At Address ##jobaddress##";
        public static string Violation_NewCronjob = "New Violation issued: Violation# <##ViolationNumber##> is recorded at Project# ##jobnumbers##.";
      
    }

    public class EmailNotificationSubject
    {
        public static string NewJobAssigned = "New job assigned";
        public static string NewMemberAddedToJobProjectTeam = "New member added to Job project team";
        public static string JobPutOnHold = "Job Put on Hold";
        public static string JobPutInProgressFromHold = "Job Put in progress from Hold";
        public static string JobMarkedCompleted = "Job marked completed";
        public static string JobReOpen = "Job Re-Open";
        public static string NewTaskAssigned = "New Task Assigned";
        public static string ProgressNoteAddedToTask = "Progress note added to Task";
        public static string JobMilestoneCompleted = "Billing Point - '##BillingPointName##' Completed. Job###Job##:##JobAddress##";
        public static string AdditionalCompleted = "Additional Service Item -  '##ServiceItemName##' Completed Job###Job##:##JobAddress##";
        public static string TaskReminder = "##TaskType## task reminder due on ##DueDate##";
        public static string RFPIsInDraftForMoreThan2Days = "RFP# ##RfpNumber## in draft since 2 days";
        public static string RFPSubmitForReview = "RFP Submit for Review";
        public static string ProposalIsReviewedAndMarkedForSending = "RFP# ##RfpNumber## reviewed & approved";
        public static string FollowupWithClient = "RFP ##RFPNumber## submitted to client since 7 days";
        public static string RFPConvertedToJob = "RFP converted to Job";
        public static string VARPMTApproved = "VARPMT ##AHVReferenceNo## of ##ApplicationNo## with ##Status## ";
    }
}