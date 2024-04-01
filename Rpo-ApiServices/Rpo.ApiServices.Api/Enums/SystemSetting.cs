// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : RICHA PATEL
// Created          : 04-23-2018
//
// Last Modified By : RICHA PATEL
// Last Modified On : 04-23-2018
// ***********************************************************************
// <copyright file="SystemSetting.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary>Enum System Setting.</summary>
// ***********************************************************************

/// <summary>
/// The Enums namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Enums
{
    /// <summary>
    /// Enum System Setting.
    /// </summary>
    public enum SystemSetting
    {
        /// <summary>
        /// The new task is created
        /// </summary>
        NewTaskIsCreated = 1,

        /// <summary>
        /// The task reminder mail
        /// </summary>
        TaskReminderMail = 2,

        /// <summary>
        /// The when a progress note is added toa task
        /// </summary>
        WhenAProgressNoteIsAddedToaTask = 3,

        /// <summary>
        /// The when task is marked complete
        /// </summary>
        WhenTaskIsMarkedComplete = 4,

        /// <summary>
        /// The when RFP is in draft for more than2days
        /// </summary>
        WhenRFPIsIndraftForMoreThan2days = 5,

        /// <summary>
        /// The when RFP is submitted to client for more than aweek
        /// </summary>
        WhenRFPIsSubmittedToClientForMoreThanAweek = 6,

        /// <summary>
        /// The when RFP is submitted for review
        /// </summary>
        WhenRFPIsSubmittedForReview = 7,

        /// <summary>
        /// The when proposal is reviewed and marked for sending when user presses approve send
        /// </summary>
        WhenProposalIsReviewedAndMarkedForSendingwhenUserPressesApproveSend = 8,

        /// <summary>
        /// The when a progress note is added to RFP
        /// </summary>
        WhenAProgressNoteIsAddedToRFP = 9,

        /// <summary>
        /// The new job is created
        /// </summary>
        NewJobIsCreated = 10,
        /// <summary>
        /// The when new job milestone is added
        /// </summary>
        WhenNewJobMilestoneIsAdded = 11,

        /// <summary>
        /// The when job milestone is completed
        /// </summary>
        WhenJobMilestoneIsCompleted = 12,

        /// <summary>
        /// The when job scope is added
        /// </summary>
        WhenJobScopeIsAdded = 13,

        /// <summary>
        /// The when permits are pulled automatically by system
        /// </summary>
        WhenPermitsArePulledAutomaticallyBySystem = 14,

        /// <summary>
        /// The when application status are pulled automatically by system
        /// </summary>
        WhenApplicationStatusArePulledAutomaticallyBySystem = 15,

        /// <summary>
        /// The when permits are about to be expired
        /// </summary>
        WhenPermitsAreAboutToBeExpired = 16,

        /// <summary>
        /// The when insurances are about to be expired
        /// </summary>
        DOBInsurancesAreAboutToBeExpired = 17,

        NewMemberAddedToJobProjectTeam = 18,

        JobPutOnHold = 19,

        JobPutInProgressFromHold = 20,

        JobMarkedCompleted = 21,

        JobReOpen = 22,

        WhenTaskIsMarkedUnattainable = 23,

        RFPConvertedToJob = 24,

        WhenHearingDateIsUpdatedBySystem = 25,

        WhenStatusOfSummonsIsUpdatedBySystem=26,

        WhenCompanyExpiryDateUpdated = 27,

        DOTInsurancesAreAboutToBeExpired = 28,

            ScopeBilling=29,
        AdditionalBilling=30,
            ContactInActive=31
    }
}