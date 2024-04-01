// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Richa Patel
// Created          : 04-17-2018
//
// Last Modified By : Richa Patel
// Last Modified On : 04-18-2018
// ***********************************************************************
// <copyright file="Permission.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary>Enum Permission</summary>
// ***********************************************************************

/// <summary>
/// The Enums namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Enums
{

    /// <summary>
    /// Enum Permission
    /// </summary>
    public enum Permission
    {

        /// <summary>
        /// The view address
        /// </summary>
        ViewAddress = 1,

        /// <summary>
        /// The add address
        /// </summary>
        AddAddress = 2,


        /// <summary>
        /// The delete address
        /// </summary>
        DeleteAddress = 3,

        /// <summary>
        /// The view company
        /// </summary>
        ViewCompany = 4,

        /// <summary>
        /// The add company
        /// </summary>
        AddCompany = 5,


        /// <summary>
        /// The delete company
        /// </summary>
        DeleteCompany = 6,

        /// <summary>
        /// The export company
        /// </summary>
        ExportCompany = 7,

        /// <summary>
        /// The view contact
        /// </summary>
        ViewContact = 8,
        /// <summary>
        /// The add contact
        /// </summary>
        AddContact = 9,

        /// <summary>
        /// The delete contact
        /// </summary>
        DeleteContact = 10,
        /// <summary>
        /// The export contact
        /// </summary>
        ExportContact = 11,

        /// <summary>
        /// The view RFP
        /// </summary>
        ViewRFP = 12,
        /// <summary>
        /// The add RFP
        /// </summary>
        AddRFP = 13,

        /// <summary>
        /// The delete RFP
        /// </summary>
        DeleteRFP = 14,

        /// <summary>
        /// The view job
        /// </summary>
        ViewJob = 15,
        /// <summary>
        /// The add job
        /// </summary>
        AddJob = 16,
        /// <summary>
        /// The delete job
        /// </summary>
        DeleteJob = 17,

        /// <summary>
        /// The add applications workpermits
        /// </summary>
        AddApplicationsWorkPermits = 18,

        /// <summary>
        /// The delete applications workpermits
        /// </summary>
        DeleteApplicationsWorkPermits = 19,
        /// <summary>
        /// The add transmittals
        /// </summary>
        ViewTransmittals = 20,
        /// <summary>
        /// The edit transmittals
        /// </summary>
        AddTransmittals = 21,
        /// <summary>
        /// The delete transmittals
        /// </summary>
        DeleteTransmittals = 22,
        /// <summary>
        /// The ps transmittals
        /// </summary>
        PrintExportTransmittals = 23,
        /// <summary>
        /// The add jobscope
        /// </summary>
        ViewJobScope = 24,
        /// <summary>
        /// The edit jobscope
        /// </summary>
        AddJobScope = 25,
        /// <summary>
        /// The delete jobscope
        /// </summary>
        DeleteJobScope = 26,
        /// <summary>
        /// The view jobmilestone
        /// </summary>
        ViewJobMilestone = 27,
        /// <summary>
        /// The add jobmilestone
        /// </summary>
        AddJobMilestone = 28,
        /// <summary>
        /// The edit jobmilestone
        /// </summary>
        DeleteJobMilestone = 29,

        /// <summary>
        /// The add job tasks
        /// </summary>
        AddJobTasks = 30,

        /// <summary>
        /// The delete job tasks
        /// </summary>
        DeleteJobTasks = 31,
        /// <summary>
        /// The view report
        /// </summary>
        ViewReport = 32,
        /// <summary>
        /// The export report
        /// </summary>
        ExportReport = 33,
        /// <summary>
        /// The view reference links
        /// </summary>
        ViewReferenceLinks = 34,
        /// <summary>
        /// The view reference document
        /// </summary>
        ViewReferenceDocument = 35,
        /// <summary>
        /// The add reference document
        /// </summary>
        AddReferenceDocument = 36,

        /// <summary>
        /// The delete reference document
        /// </summary>
        DeleteReferenceDocument = 37,
        /// <summary>
        /// The view master data
        /// </summary>
        ViewMasterData = 38,
        /// <summary>
        /// The add master data
        /// </summary>
        AddMasterData = 39,

        /// <summary>
        /// The delete master data
        /// </summary>
        DeleteMasterData = 40,
        /// <summary>
        /// The view employee user group
        /// </summary>
        ViewEmployeeUserGroup = 41,
        /// <summary>
        /// The add employee user group
        /// </summary>
        AddEmployeeUserGroup = 42,

        /// <summary>
        /// The delete employee user group
        /// </summary>
        DeleteEmployeeUserGroup = 43,
        /// <summary>
        /// The view employee
        /// </summary>
        ViewEmployee = 44,
        /// <summary>
        /// The add employee
        /// </summary>
        AddEmployee = 45,

        /// <summary>
        /// The delete employee
        /// </summary>
        DeleteEmployee = 46,
        /// <summary>
        /// The view employee information
        /// </summary>
        ViewEmployeeInfo = 47,
        /// <summary>
        /// The edit employee information
        /// </summary>
        EditEmployeeInfo = 48,
        /// <summary>
        /// The view contact information
        /// </summary>
        ViewContactInfo = 49,
        /// <summary>
        /// The edit contact information
        /// </summary>
        EditContactInfo = 50,
        /// <summary>
        /// The view personal information
        /// </summary>
        ViewPersonalInformation = 51,
        /// <summary>
        /// The edit personal information
        /// </summary>
        EditPersonalInformation = 52,
        /// <summary>
        /// The view agent certificates
        /// </summary>
        ViewAgentCertificates = 53,
        /// <summary>
        /// The edit agent certificates
        /// </summary>
        EditAgentCertificates = 54,
        /// <summary>
        /// The view system access information
        /// </summary>
        ViewSystemAccessInformation = 55,
        /// <summary>
        /// The edit system access information
        /// </summary>
        EditSystemAccessInformation = 56,

        /// <summary>
        /// The view documents
        /// </summary>
        ViewDocuments = 57,
        /// <summary>
        /// The edit documents
        /// </summary>
        EditDocuments = 58,
        /// <summary>
        /// The view status
        /// </summary>
        ViewStatus = 59,
        /// <summary>
        /// The edit status
        /// </summary>
        EditStatus = 60,
        /// <summary>
        /// The view phoneinfo
        /// </summary>
        ViewPhoneInfo = 61,
        /// <summary>
        /// The edit phoneinfo
        /// </summary>
        EditPhoneInfo = 62,
        /// <summary>
        /// The view emergencycontactinfo
        /// </summary>
        ViewEmergencyContactInfo = 63,
        /// <summary>
        /// The edit emergencycontactinfo
        /// </summary>
        EditEmergencyContactInfo = 64,
        /// <summary>
        /// The view fee schedule master
        /// </summary>
        ViewFeeScheduleMaster = 65,
        /// <summary>
        /// The create fee schedule master
        /// </summary>
        AddFeeScheduleMaster = 66,

        /// <summary>
        /// The delete fee schedule master
        /// </summary>
        DeleteFeeScheduleMaster = 67,
        /// <summary>
        /// The add job documents
        /// </summary>
        AddJobDocuments = 68,

        /// <summary>
        /// The delete job documents
        /// </summary>
        DeleteJobDocuments = 69,

        /// <summary>
        /// The add edit violation
        /// </summary>
        AddECBViolations = 70,

        /// <summary>
        /// The delete violation
        /// </summary>
        DeleteECBViolations = 71,

        /// <summary>
        /// The cost job scope
        /// </summary>
        CostJobScope = 72,

        /// <summary>
        /// Edit Completed Job Tasks
        /// </summary>
        EditCompletedJobTasks = 73,

        /// <summary>
        /// Completed Scope / Billing Points but not Invoiced
        /// </summary>
        ViewComplateScopeReport = 74,

        /// <summary>
        /// Closed Jobs with Open Billing Points
        /// </summary>
        ViewClosedScopeReport = 75,

        ViewChecklist = 76,
        AddEditChecklist = 77,
        DeleteChecklist = 78,

        /// <summary>
        /// The view customer
        /// </summary>
        ViewCustomer = 79,
        /// <summary>
        /// The add customer
        /// </summary>
        AddCustomer = 80,

        /// <summary>
        /// The delete customer
        /// </summary>
        DeleteCustomer = 81,
        ViewChecklistClientNote = 82,
        /// <summary>
        /// The view Dashboard information
        /// </summary>
        ViewDashboard = 83,
        ViewSendEmailRPO = 84,
        ViewSendEmailContact = 85,
        ExportChecklist = 86,
        ViewProjectContacts = 87,
        AddProjectContacts = 88,
        DeleteProjectContacts = 89,
        ViewJobTasks = 90,
        ViewECBViolations = 91,
        ViewDOBViolations = 92,
        AddDOBViolations = 93,
        DeleteDOBViolations = 94,
        ViewApplicationsWorkPermits = 95,
        ExportJobs = 96,
        ViewAllViolationsReport = 97,
        ViewPermitExpiryReport = 98,

        ViewAHVPermitExpiryReport = 99,
        ViewApplicationStatusReport = 100,
        ViewContractorInsurancesExpiryReport = 101,
        ViewContactLicenseExpiryReport = 102,
        ViewConsolidatedStatusReport = 103,
        ViewProposalsReport = 104,
        ViewUnsyncTimenoteReport = 105
    }
}