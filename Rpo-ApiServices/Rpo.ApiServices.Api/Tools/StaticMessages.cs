// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-14-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-16-2018
// ***********************************************************************
// <copyright file="StaticMessages.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Static Messages.</summary>
// ***********************************************************************

/// <summary>
/// The Tools namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Tools
{
    /// <summary>
    /// Class Static Messages.
    /// </summary>
    public class StaticMessages
    {
        public static string ViolationPaneltyCodeExistsMessage = "Entered Violation Penalty Code already exists";

        public static string AddressTypeNameExistsMessage = "Entered Address Type already exists";

        public static string OwnerTypeNameExistsMessage = "Entered Owner Type already exists";

        public static string DOBPenaltyScheduleInfractionCodeExistsMessage = "Entered Infraction Code already exists";

        public static string FDNYPenaltyScheduleOATHViolationCodeExistsMessage = "Entered OATH Violation Code already exists";
        
        public static string JobContactGroupNameExistsMessage = "Entered Job Contact Group already exists";
        
        //public static string TrackingNumberAlreadyInOtherJobExistsMessage = "Tracking# already exists in other job";

        public static string RfpAddressExistsMessage = "Entered Address already exists";

        public static string JobTypeNameExistsMessage = "Entered Job Type already exists";

        public static string JobTypePartofExistsMessage = "Child Item can't be part of.";

        public static string JobTypeDescriptionExistsMessage = "Entered Job Type Description already exists";

        public static string JobSubTypeExistsMessage = "Entered Job Sub Type already exists";

        public static string ServiceGroupExistsMessage = "Entered Service Group already exists";

        public static string ServicedeletedtaskMessage = "Task can not be completed as mapped service item has been removed from scope.";

        public static string ServiceItemExistsMessage = "Entered Service Item already exists";

        public static string ServiceItemParentExistsMessage = "Selected Service Item is already part of another service item";

        public static string ServiceItemInactiveMessage = "Cannot disable the service item, as it's a parent item to other items";

        public static string ServiceItemExistsPartofMessage = "You can't select same service as " +"Partof.";

        public static string SuffixExistsMessage = "Entered Suffix already exists";

        public static string PrefixExistsMessage = "Entered Prefix already exists";

        public static string RfpJobTypeNameExistsMessage = "Entered Job Type already exists";

        public static string RfpSubJobTypeCategoryNameExistsMessage = "Entered Job Type Description already exists";

        public static string RfpSubJobTypeNameExistsMessage = "Entered Job Sub-type already exists";

        public static string RfpWorkTypeCategoryNameExistsMessage = "Entered Service Group already exists";

        public static string RfpWorkTypeNameExistsMessage = "Entered Service Item already exists";

        public static string AddressTypePriorityExistsMessage = "Entered Address Type priority order already exists";

        public static string OnlyCreatorCanDeleteTaskMessage = "Only the creator or assigner can delete the task";

        public static string CompletedTaskCannotDeleteMessage = "Completed tasks cannot be deleted";

        public static string MultipleDwellingClassificationDescriptionExistsMessage = "Entered Multiple Dwelling Classification already exists";

        public static string CompanyTypeNameExistsMessage = "Entered Company Type already exists";

        public static string DocumentTypeNameExistsMessage = "Entered Certificate Type already exists";

        public static string TaskTypeNameExistsMessage = "Entered Task Type already exists";

        public static string TransmissionTypeNameExistsMessage = "Entered Sent Via already exists";

        public static string OccupancyClassificationNameExistsMessage = "Entered Occupancy Classification already exists";

        public static string SeismicDesignCategoryNameExistsMessage = "Entered Seismic Design Category already exists";

        public static string StructureOccupancyCategoryNameExistsMessage = "Entered Structure Occupancy Category already exists";

        public static string TaxIdTypeNameExistsMessage = "Entered Company Tax Type already exists";

        public static string ConstructionClassificationNameExistsMessage = "Entered Construction Classification already exists";

        public static string PrimaryStructuralSystemNameExistsMessage = "Entered Primary Structural System already exists";

        public static string StateNameExistsMessage = "Entered State already exists";

        public static string ContactLicenseTypeNameExistsMessage = "Entered Contact License Type already exists";

        public static string EmailTypeNameExistsMessage = "Entered Email Type already exists";

        public static string JobContactTypeNameExistsMessage = "Entered Job Contact Type already exists";

        public static string ContactTitleNameExistsMessage = "Entered Contact Title already exists";

        public static string JobTimeNoteCategoryNameExistsMessage = "Entered Job Time Note Category already exists";

        public static string EmployeeInactiveValidationMessage = "Inactivation failed. {0} has task(s) and/or job(s) assigned";

        public static string EmployeeDeleteValidationMessage = "Delete operation failed. {0} has task(s) and/or job(s) assigned";

        public static string DeleteReferenceExistExceptionMessage = "Record cannot be deleted since it is associated with other records";

        //public static string NewJobAssignedNotificationMessage = "New job# ##JobNumber## is assigned to you";

        public static string NewTaskAssignedNotificationMessage = "New task# ##TaskNumber## assigned to you";

        public static string TaskCompleteNotificationMessage = "Task# ##TaskNumber## is marked complete";

        public static string NewProgressNoteAddedNotificationMessage = "New progress note added to task# ##TaskNumber##";

        public static string TaskReminderNotificationMessage = "Your task# ##TaskNumber## is due on ##DueDate##";

        public static string NewRfpProgressNoteAddedNotificationMessage = "New general note added in RFP# ##RfpNumber##";

        public static string RFPSubmitForReviewNotificationMessage = "RFP# ##RfpNumber## is submited for review";

        public static string RFPReviewedAndApprovedNotificationMessage = "RFP# ##RfpNumber## is reviewed and approved to submit to client";

        public static string EmployeeIsSetInactiveTokenMessage = "User is not active";

        public static string EmployeeIsDeletedTokenMessage = "User is deleted";

        public static string FeeScheduleNotExistsMessage = "At least one service item must be selected";

        public static string DuplicateMilestoneServiceMessage = "Few service items are associated with multiple milestones. Please associate a service item with single milestone";

        public static string ReferenceLinkNameExistsMessage = "Entered Reference Link already exists";

        public static string FeeScheduleQuantityExceedsLimitMessage = "You cannot complete this task as the pending quantity exceeds service quantity";

        public static string FeeScheduleAllreadyMessage = "You cannot complete this task as the service item in scope has already been completed.";

        public static string JobMilestoneAllreadyMessage = "You cannot complete this task as the Milestone in scope has already been completed.";

        //public static string TaskAlreadyCompleteStatusMessage = "Completed Task can not be edited";

        public static string FeeScheduleRemovedTaskNotEditMessage = "You are not allowed to edit the task as related job scope is removed";

        public static string InvalidInvoiceDateMessage = "Invoice date is in invalid format";

        public static string NewJobMileStoneNotificationMessage = "New milestone is added in job# ##JobNumber##";

        public static string JobMilestoneCompletedNotificationMessage = "Milestone is completed in Job# ##JobNumber##";

        public static string NoRecordFoundMessage = "No record found for the entered filter criteria";

        public static string NoPermissionMessage = "You don't have permission to access this feature";

        public static string JobTimeNotesAddedHistoryMessage = "##Hours## time notes has been added with following details : ##JobTimeNoteDescription##";

        public static string FeeScheduleQuantityTimeNotesExceedsLimitMessage = "You cannot add time notes as the pending quantity exceeds service quantity";
        public static string FeeScheduleJobimeNotesExceedsLimitMessage = "You cannot add time notes as the pending hours exceeds service hours";

        public static string ViolationNotFound = "Entered Summons Number does not exist";

        public static string TrackingNumberAleadyExists = "Tracking# already exists in Job# {0}";

        //public static string JobApplicationLocationAleadyExists = "Location already exists in same job";
        public static string JobApplicationLocationAleadyExists = "Same Location already exist for selected type in the Job";

        public static string ApplicationNumberAleadyExists = "Application# already exists in Job# {0}";

        public static string ApplicationNumberRequired = "Application# required";

        public static string ApplicationTypeRequired = "Application Type required";

        //public static string JobViolationNumberExistsMessage = "Entered Summons# already exists in Job# {0}";

        public static string JobViolationNumberExistsMessage = "The Violation# {0} is already in the system for Job# {1}.";

        public static string SupportDocumentExistForThisDocument = "Record cannot be deleted since it is associated with other support document records";

        public static string JobWorkTypeNameExistsMessage = "Entered Job Work Type already exists";

        public static string JobWorkTypeCodeExistsMessage = "Entered Job Work Type Code already exists";

        public static string DocumentNameExistsMessage = "Entered Document already exists";
        public static string DocumentCodeExistsMessage = "Entered Code already exists";

        public static string CompanyLicenseTypeNameExistsMessage = "Entered Company License Type already exists";

        public static string CheckListGroupNameExistsMessage= "Entered Checklist Group Name already exists";
        public static string CheckListGroupOrderExistsMessage = "Entered Checklist Group order already exists";
        public static string ChecklistItemNameExistsMessage= "Entered Checklist Item Name already exists";
        public static string NoChecklistItemMatches = "Can Not Generate Checklist Because No Checklist Items Match This Selection Of Application, Permit and Group";

        public static string ChecklistViewNotExists = "No Records Exists for the Checklist";
        public static string ManageGroupOrder_Successful = "ChecklistGroup Order Modified Successfully";

        public static string ManageOrder_UnSuccessful = "ManageOrder unsuccessful / Body has null parameters ";

        public static string ManageGroupItem_Successful = "ChecklistItem Order Modified Successfully";

        public static string DeleteCheckListItem_InActive = "ChecklistItem made Inactive - associated with active checklist";

        public static string DeleteCheckListItem_Success = "ChecklistItem deleted Successfully";

        public static string DeleteItemfromchecklist_Success = "Item from checklist deleted Successfully";
        public static string MailSendingFailed = "Mail sending failed";

    }
}
