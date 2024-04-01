// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Richa Patel
// Created          : 04-17-2018
//
// Last Modified By : Richa Patel
// Last Modified On : 04-18-2018
// ***********************************************************************
// <copyright file="Configuration.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

/// <summary>
/// The Migrations namespace.
/// </summary>
namespace Rpo.ApiServices.Model.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Model.Models.Enums;
    using Rpo.Identity.Core;
    using Rpo.Identity.Core.Managers;
    using Rpo.Identity.Core.Models;
    using Rpo.Identity.Core.Models.Enumerations;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<RpoContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
           //AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Rpo.ApiServices.Model.RpoContext context)
        {
            base.Seed(context);

            if (!context.RpoIdentityClients.Any())
            {
                BuildClientsList().ForEach(c =>
                {
                    context.RpoIdentityClients.AddOrUpdate(ic => ic.Id, c);
                });

                context.SaveChanges();
            }

            if (!context.Groups.Any(g => g.Name == "Administrators"))
            {
                context.Groups.AddOrUpdate(
                  g => g.Name,
                  new Group
                  {
                      Name = "Administrators",
                      Description = "Built-In Group with administration rights",
                      Permissions = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75",
                      IsActive = true
                  }
                );
                context.SaveChanges();
            }

            if (!context.Permissions.Any())
            {
                context.Permissions.AddOrUpdate(
                e => e.Name,
                new Permission { Name = "ViewAddress", DisplayName = "View", GroupName = "Address", ModuleName = "Other", PermissionClass = "fa fa-eye", DisplayOrder = 5 },
                new Permission { Name = "AddEditAddress", DisplayName = "Add/Edit", GroupName = "Address", ModuleName = "Other", PermissionClass = "fa fa-edit", DisplayOrder = 5 },
                new Permission { Name = "DeleteAddress", DisplayName = "Delete", GroupName = "Address", ModuleName = "Other", PermissionClass = "fa fa-trash", DisplayOrder = 5 },

                new Permission { Name = "ViewCompany", DisplayName = "View", GroupName = "Company", ModuleName = "Other", PermissionClass = "fa fa-eye", DisplayOrder = 2 },
                new Permission { Name = "AddEditCompany", DisplayName = "Add/Edit", GroupName = "Company", ModuleName = "Other", PermissionClass = "fa fa-edit", DisplayOrder = 2 },
                new Permission { Name = "DeleteCompany", DisplayName = "Delete", GroupName = "Company", ModuleName = "Other", PermissionClass = "fa fa-trash", DisplayOrder = 2 },
                new Permission { Name = "ExportCompany", DisplayName = "Export", GroupName = "Company", ModuleName = "Other", PermissionClass = "fa fa-cloud-download", DisplayOrder = 2 },

                new Permission { Name = "ViewContact", DisplayName = "View", GroupName = "Contact", ModuleName = "Other", PermissionClass = "fa fa-eye", DisplayOrder = 6 },
                new Permission { Name = "AddEditContact", DisplayName = "Add/Edit", GroupName = "Contact", ModuleName = "Other", PermissionClass = "fa fa-edit", DisplayOrder = 6 },
                new Permission { Name = "DeleteContact", DisplayName = "Delete", GroupName = "Contact", ModuleName = "Other", PermissionClass = "fa fa-trash", DisplayOrder = 6 },
                new Permission { Name = "ExportContact", DisplayName = "Export", GroupName = "Contact", ModuleName = "Other", PermissionClass = "fa fa-cloud-download", DisplayOrder = 6 },

                new Permission { Name = "ViewRFP", DisplayName = "View", GroupName = "RFP", ModuleName = "Other", PermissionClass = "fa fa-eye", DisplayOrder = 3 },
                new Permission { Name = "AddEditRFP", DisplayName = "Add/Edit", GroupName = "RFP", ModuleName = "Other", PermissionClass = "fa fa-edit", DisplayOrder = 3 },
                new Permission { Name = "DeleteRFP", DisplayName = "Delete", GroupName = "RFP", ModuleName = "Other", PermissionClass = "fa fa-trash", DisplayOrder = 3 },

                new Permission { Name = "ViewJob", DisplayName = "View", GroupName = "Job", ModuleName = "Job", PermissionClass = "fa fa-eye", DisplayOrder = 1 },
                new Permission { Name = "AddEditJob", DisplayName = "Add/Edit", GroupName = "Job", ModuleName = "Job", PermissionClass = "fa fa-edit", DisplayOrder = 1 },
                new Permission { Name = "DeleteJob", DisplayName = "Delete", GroupName = "Job", ModuleName = "Job", PermissionClass = "fa fa-trash", DisplayOrder = 1 },

                new Permission { Name = "AddEditApplicationsWorkPermits", DisplayName = "Add/Edit", GroupName = "Applications & Work permits", ModuleName = "Job", PermissionClass = "fa fa-edit", DisplayOrder = 2 },
                new Permission { Name = "DeleteApplicationsWorkPermits", DisplayName = "Delete", GroupName = "Applications & Work permits", ModuleName = "Job", PermissionClass = "fa fa-trash", DisplayOrder = 2 },

                new Permission { Name = "ViewTransmittals", DisplayName = "View", GroupName = "Transmittals", ModuleName = "Job", PermissionClass = "fa fa-eye", DisplayOrder = 3 },
                new Permission { Name = "AddTransmittals", DisplayName = "Add", GroupName = "Transmittals", ModuleName = "Job", PermissionClass = "fa fa-edit", DisplayOrder = 3 },
                new Permission { Name = "DeleteTransmittals", DisplayName = "Delete", GroupName = "Transmittals", ModuleName = "Job", PermissionClass = "fa fa-trash", DisplayOrder = 3 },
                new Permission { Name = "PrintExportTransmittals", DisplayName = "Export", GroupName = "Transmittals", ModuleName = "Job", PermissionClass = "fa fa-cloud-download", DisplayOrder = 3 },

                new Permission { Name = "ViewJobScope", DisplayName = "View", GroupName = "Job scope", ModuleName = "Job", PermissionClass = "fa fa-eye", DisplayOrder = 4 },
                new Permission { Name = "AddEditJobScope", DisplayName = "Add/Edit", GroupName = "Job scope", ModuleName = "Job", PermissionClass = "fa fa-edit", DisplayOrder = 4 },
                new Permission { Name = "DeleteJobScope", DisplayName = "Delete", GroupName = "Job scope", ModuleName = "Job", PermissionClass = "fa fa-trash", DisplayOrder = 4 },

                ////new Permission { Name = "ViewJobMilestone", DisplayName = "View", GroupName = "Job milestone", ModuleName = "Job", PermissionClass = "fa fa-eye", DisplayOrder = 5 },
                ////new Permission { Name = "AddEditJobMilestone", DisplayName = "Add/Edit", GroupName = "Job milestone", ModuleName = "Job", PermissionClass = "fa fa-edit", DisplayOrder = 5 },
                ////new Permission { Name = "DeleteJobMilestone", DisplayName = "Delete", GroupName = "Job milestone", ModuleName = "Job", PermissionClass = "fa fa-trash", DisplayOrder = 5 },

                new Permission { Name = "ViewJobMilestone", DisplayName = "View", GroupName = "Billing Points", ModuleName = "Job", PermissionClass = "fa fa-eye", DisplayOrder = 5 },
                new Permission { Name = "AddEditJobMilestone", DisplayName = "Add/Edit", GroupName = "Billing Points", ModuleName = "Job", PermissionClass = "fa fa-edit", DisplayOrder = 5 },
                new Permission { Name = "DeleteJobMilestone", DisplayName = "Delete", GroupName = "Billing Points", ModuleName = "Job", PermissionClass = "fa fa-trash", DisplayOrder = 5 },


                new Permission { Name = "AddEditJobTasks", DisplayName = "Add/Edit", GroupName = "Job Tasks", ModuleName = "Job", PermissionClass = "fa fa-edit", DisplayOrder = 6 },
                new Permission { Name = "DeleteJobTasks", DisplayName = "Delete", GroupName = "Job Tasks", ModuleName = "Job", PermissionClass = "fa fa-trash", DisplayOrder = 6 },

                new Permission { Name = "ViewReport", DisplayName = "View", GroupName = "Report", ModuleName = "Other", PermissionClass = "fa fa-eye", DisplayOrder = 4 },
                new Permission { Name = "ExportReport", DisplayName = "Export", GroupName = "Report", ModuleName = "Other", PermissionClass = "fa fa-cloud-download", DisplayOrder = 4 },

                new Permission { Name = "ViewReferenceLinks", DisplayName = "View Reference Links", GroupName = "Reference Links", ModuleName = "Other", PermissionClass = "fa fa-eye", DisplayOrder = 10 },

                new Permission { Name = "ViewReferenceDocument", DisplayName = "View", GroupName = "Reference Document", ModuleName = "Other", PermissionClass = "fa fa-eye", DisplayOrder = 9 },
                new Permission { Name = "AddEditReferenceDocument", DisplayName = "Add/Edit", GroupName = "Reference Document", ModuleName = "Other", PermissionClass = "fa fa-edit", DisplayOrder = 9 },
                new Permission { Name = "DeleteReferenceDocument", DisplayName = "Delete", GroupName = "Reference Document", ModuleName = "Other", PermissionClass = "fa fa-trash", DisplayOrder = 9 },

                new Permission { Name = "ViewMasterData", DisplayName = "View", GroupName = "Master Data", ModuleName = "Other", PermissionClass = "fa fa-eye", DisplayOrder = 8 },
                new Permission { Name = "AddEditMasterData", DisplayName = "Add/Edit", GroupName = "Master Data", ModuleName = "Other", PermissionClass = "fa fa-edit", DisplayOrder = 8 },
                new Permission { Name = "DeleteMasterData", DisplayName = "Delete", GroupName = "Master Data", ModuleName = "Other", PermissionClass = "fa fa-trash", DisplayOrder = 8 },

                new Permission { Name = "ViewEmployeeUserGroup", DisplayName = "View", GroupName = "User Group", ModuleName = "Other", PermissionClass = "fa fa-eye", DisplayOrder = 1 },
                new Permission { Name = "AddEditEmployeeUserGroup", DisplayName = "Add/Edit", GroupName = "User Group", ModuleName = "Other", PermissionClass = "fa fa-edit", DisplayOrder = 1 },
                new Permission { Name = "DeleteEmployeeUserGroup", DisplayName = "Delete", GroupName = "User Group", ModuleName = "Other", PermissionClass = "fa fa-trash", DisplayOrder = 1 },

                new Permission { Name = "ViewEmployee", DisplayName = "View", GroupName = "Employee", ModuleName = "Employee", PermissionClass = "fa fa-eye", DisplayOrder = 1 },
                new Permission { Name = "AddEditEmployee", DisplayName = "Add/Edit", GroupName = "Employee", ModuleName = "Employee", PermissionClass = "fa fa-edit", DisplayOrder = 1 },
                new Permission { Name = "DeleteEmployee", DisplayName = "Delete", GroupName = "Employee", ModuleName = "Employee", PermissionClass = "fa fa-trash", DisplayOrder = 1 },

                new Permission { Name = "ViewEmployeeInfo", DisplayName = "View", GroupName = "Employee Info", ModuleName = "Employee", PermissionClass = "fa fa-eye", DisplayOrder = 2 },
                new Permission { Name = "EditEmployeeInfo", DisplayName = "Edit", GroupName = "Employee Info", ModuleName = "Employee", PermissionClass = "fa fa-edit", DisplayOrder = 2 },

                new Permission { Name = "ViewContactInfo", DisplayName = "View", GroupName = "Contact Info", ModuleName = "Employee", PermissionClass = "fa fa-eye", DisplayOrder = 3 },
                new Permission { Name = "EditContactInfo", DisplayName = "Edit", GroupName = "Contact Info", ModuleName = "Employee", PermissionClass = "fa fa-edit", DisplayOrder = 3 },

                new Permission { Name = "ViewPersonalInformation", DisplayName = "View", GroupName = "Personal Information", ModuleName = "Employee", PermissionClass = "fa fa-eye", DisplayOrder = 8 },
                new Permission { Name = "EditPersonalInformation", DisplayName = "Edit", GroupName = "Personal Information", ModuleName = "Employee", PermissionClass = "fa fa-edit", DisplayOrder = 8 },

                new Permission { Name = "ViewAgentCertificates", DisplayName = "View", GroupName = "Agent Certificates", ModuleName = "Employee", PermissionClass = "fa fa-eye", DisplayOrder = 7 },
                new Permission { Name = "EditAgentCertificates", DisplayName = "Edit", GroupName = "Agent Certificates", ModuleName = "Employee", PermissionClass = "fa fa-edit", DisplayOrder = 7 },

                new Permission { Name = "ViewSystemAccessInformation", DisplayName = "View", GroupName = "System Access Information", ModuleName = "Employee", PermissionClass = "fa fa-eye", DisplayOrder = 6 },
                new Permission { Name = "EditSystemAccessInformation", DisplayName = "Edit", GroupName = "System Access Information", ModuleName = "Employee", PermissionClass = "fa fa-edit", DisplayOrder = 6 },

                new Permission { Name = "ViewDocuments", DisplayName = "View", GroupName = "Document(s)", ModuleName = "Employee", PermissionClass = "fa fa-eye", DisplayOrder = 9 },
                new Permission { Name = "EditDocuments", DisplayName = "Edit", GroupName = "Document(s)", ModuleName = "Employee", PermissionClass = "fa fa-edit", DisplayOrder = 9 },

                new Permission { Name = "ViewStatus", DisplayName = "View", GroupName = "Status", ModuleName = "Employee", PermissionClass = "fa fa-eye", DisplayOrder = 10 },
                new Permission { Name = "EditStatus", DisplayName = "Edit", GroupName = "Status", ModuleName = "Employee", PermissionClass = "fa fa-edit", DisplayOrder = 10 },

                new Permission { Name = "ViewPhoneInfo", DisplayName = "View", GroupName = "Phone info", ModuleName = "Employee", PermissionClass = "fa fa-eye", DisplayOrder = 5 },
                new Permission { Name = "EditPhoneInfo", DisplayName = "Edit", GroupName = "Phone info", ModuleName = "Employee", PermissionClass = "fa fa-edit", DisplayOrder = 5 },

                new Permission { Name = "ViewEmergencyContactInfo", DisplayName = "View", GroupName = "Emergency contact info", ModuleName = "Employee", PermissionClass = "fa fa-eye", DisplayOrder = 4 },
                new Permission { Name = "EditEmergencyContactInfo", DisplayName = "Edit", GroupName = "Emergency contact info", ModuleName = "Employee", PermissionClass = "fa fa-edit", DisplayOrder = 4 },

                new Permission { Name = "ViewFeeScheduleMaster", DisplayName = "View", GroupName = "Fee Schedule Master", ModuleName = "Other", PermissionClass = "fa fa-eye", DisplayOrder = 7 },
                new Permission { Name = "AddEditFeeScheduleMaster", DisplayName = "Add/Edit", GroupName = "Fee Schedule Master", ModuleName = "Other", PermissionClass = "fa fa-edit", DisplayOrder = 7 },
                new Permission { Name = "DeleteFeeScheduleMaster", DisplayName = "Delete", GroupName = "Fee Schedule Master", ModuleName = "Other", PermissionClass = "fa fa-trash", DisplayOrder = 7 },

                new Permission { Name = "AddEditJobDocuments", DisplayName = "Add/Edit", GroupName = "Job Documents", ModuleName = "Job", PermissionClass = "fa fa-edit", DisplayOrder = 7 },
                new Permission { Name = "DeleteJobDocuments", DisplayName = "Delete", GroupName = "Job Documents", ModuleName = "Job", PermissionClass = "fa fa-trash", DisplayOrder = 7 },

                new Permission { Name = "AddEditViolation", DisplayName = "Add/Edit", GroupName = "Violation", ModuleName = "Job", PermissionClass = "fa fa-edit", DisplayOrder = 8 },
                new Permission { Name = "DeleteViolation", DisplayName = "Delete", GroupName = "Violation", ModuleName = "Job", PermissionClass = "fa fa-trash", DisplayOrder = 8 },

                ////new Permission { Name = "CostJobScope", DisplayName = "Cost", GroupName = "Job Scope", ModuleName = "Job", PermissionClass = "fa fa-eye123", DisplayOrder = 4 },
                new Permission { Name = "CostJobMilestone", DisplayName = "Cost", GroupName = "Billing Points", ModuleName = "Job", PermissionClass = "fa fa-usd", DisplayOrder = 5 },

                new Permission { Name = "EditCompletedJobTasks", DisplayName = "Edit Completed", GroupName = "Job Tasks", ModuleName = "Job", PermissionClass = "fa fa-edit", DisplayOrder = 6 },

                new Permission { Name = "ViewComplateScopeReport", DisplayName = "View Completed Scope / Billing Points but not Invoiced", GroupName = "Report", ModuleName = "Other", PermissionClass = "fa fa-eye", DisplayOrder = 4 },
                new Permission { Name = "ViewClosedScopeReport", DisplayName = "View Closed Jobs with Open Billing Points", GroupName = "Report", ModuleName = "Other", PermissionClass = "fa fa-eye", DisplayOrder = 4 }
                );

                context.SaveChanges();
            }

            if (!context.SystemSettings.Any())
            {
                context.SystemSettings.AddOrUpdate(
                e => e.Name,
                new SystemSetting { Name = "New task created" },
                new SystemSetting { Name = "Task reminder" },
                new SystemSetting { Name = "Progress note added to task" },
                new SystemSetting { Name = "Billable task (Scope Billing & Additional Billing) marked complete" },
                new SystemSetting { Name = "Proposal has 'In Draft' status for more than 2 days" },
                new SystemSetting { Name = "Proposal has 'Submitted to client' status for more than 7 days" },
                new SystemSetting { Name = "Proposal submitted for review" },
                new SystemSetting { Name = "Proposal approved" },
                new SystemSetting { Name = "General note added to RFP" },
                new SystemSetting { Name = "New job created" },
                new SystemSetting { Name = "New milestone added to job" },
                new SystemSetting { Name = "Job milestone completed" },
                new SystemSetting { Name = "New scope added to job" },
                new SystemSetting { Name = "Permits pulled by system" },
                new SystemSetting { Name = "Application status pulled by system" },
                new SystemSetting { Name = "Permits about to expire" },
                new SystemSetting { Name = "Insurances about to expire" },
                new SystemSetting { Name = "New member added to job project team" },
                new SystemSetting { Name = "Job put on-hold" },
                new SystemSetting { Name = "Job put in-progress from on-hold" },
                new SystemSetting { Name = "Job marked completed" },
                new SystemSetting { Name = "Job re-opened" },
                new SystemSetting { Name = "Task is marked unattainable" },
                new SystemSetting { Name = "RFP converted to Job" },
                new SystemSetting { Name = "Violation hearing date is updated by system" },
                new SystemSetting { Name = "Status Of summons is updated by system" },
                new SystemSetting { Name = "Company expiry date updated" }
                );

                context.SaveChanges();
            }

            if (!context.States.Any())
            {
                context.States.AddOrUpdate(
                e => e.Acronym,
                new State { Acronym = "AL", Name = "Alabama" },
                new State { Acronym = "AK", Name = "Alaska" },
                new State { Acronym = "AZ", Name = "Arizona" },
                new State { Acronym = "AR", Name = "Arkansas" },
                new State { Acronym = "CA", Name = "California" },
                new State { Acronym = "CO", Name = "Colorado" },
                new State { Acronym = "CT", Name = "Connecticut" },
                new State { Acronym = "DE", Name = "Delaware" },
                new State { Acronym = "FL", Name = "Florida" },
                new State { Acronym = "GA", Name = "Georgia" },
                new State { Acronym = "HI", Name = "Hawaii" },
                new State { Acronym = "ID", Name = "Idaho" },
                new State { Acronym = "IL", Name = "Illinois" },
                new State { Acronym = "IN", Name = "Indiana" },
                new State { Acronym = "IA", Name = "Iowa" },
                new State { Acronym = "KS", Name = "Kansas" },
                new State { Acronym = "KY", Name = "Kentucky" },
                new State { Acronym = "LA", Name = "Louisiana" },
                new State { Acronym = "ME", Name = "Maine" },
                new State { Acronym = "MD", Name = "Maryland" },
                new State { Acronym = "MA", Name = "Massachusetts" },
                new State { Acronym = "MI", Name = "Michigan" },
                new State { Acronym = "MN", Name = "Minnesota" },
                new State { Acronym = "MS", Name = "Mississippi" },
                new State { Acronym = "MO", Name = "Missouri" },
                new State { Acronym = "MT", Name = "Montana" },
                new State { Acronym = "NE", Name = "Nebraska" },
                new State { Acronym = "NV", Name = "Nevada" },
                new State { Acronym = "NH", Name = "New Hampshire" },
                new State { Acronym = "NJ", Name = "New Jersey" },
                new State { Acronym = "NM", Name = "New Mexico" },
                new State { Acronym = "NY", Name = "New York" },
                new State { Acronym = "NC", Name = "North Carolina" },
                new State { Acronym = "ND", Name = "North Dakota" },
                new State { Acronym = "OH", Name = "Ohio" },
                new State { Acronym = "OK", Name = "Oklahoma" },
                new State { Acronym = "OR", Name = "Oregon" },
                new State { Acronym = "PA", Name = "Pennsylvania" },
                new State { Acronym = "RI", Name = "Rhode Island" },
                new State { Acronym = "SC", Name = "South Carolina" },
                new State { Acronym = "SD", Name = "South Dakota" },
                new State { Acronym = "TN", Name = "Tennessee" },
                new State { Acronym = "TX", Name = "Texas" },
                new State { Acronym = "UT", Name = "Utah" },
                new State { Acronym = "VT", Name = "Vermont" },
                new State { Acronym = "VA", Name = "Virginia" },
                new State { Acronym = "WA", Name = "Washington" },
                new State { Acronym = "WV", Name = "West Virginia" },
                new State { Acronym = "WI", Name = "Wisconsin" },
                new State { Acronym = "WY", Name = "Wyoming" }
                );

                context.SaveChanges();
            }          
            if (!context.DocumentTypes.Any())
            {
                context.DocumentTypes.AddOrUpdate(dt => dt.Name, new DocumentType { Name = "DOB Filing Representative" });
                context.DocumentTypes.AddOrUpdate(dt => dt.Name, new DocumentType { Name = "FDNY" });
                context.DocumentTypes.AddOrUpdate(dt => dt.Name, new DocumentType { Name = "Professional Engineer" });
                context.DocumentTypes.AddOrUpdate(dt => dt.Name, new DocumentType { Name = "Registered Architect" });
                context.DocumentTypes.AddOrUpdate(dt => dt.Name, new DocumentType { Name = "OSHA (various certifications)" });
                context.DocumentTypes.AddOrUpdate(dt => dt.Name, new DocumentType { Name = "Supported Scaffold Training" });
                context.DocumentTypes.AddOrUpdate(dt => dt.Name, new DocumentType { Name = "Other" });
                context.SaveChanges();
            }
            if (!context.Prefixes.Any())
            {
                context.Prefixes.AddOrUpdate(p => p.Name, new Prefix { Name = "Mr." });
                context.Prefixes.AddOrUpdate(p => p.Name, new Prefix { Name = "Ms." });
                context.Prefixes.AddOrUpdate(p => p.Name, new Prefix { Name = "Mrs." });
                context.SaveChanges();
            }

            //if (!context.Verbiages.Any())
            //{
            //    context.Verbiages.AddOrUpdate(p => p.Name, new Rpo.ApiServices.Model.Models.Verbiage { Name = "Introduction", Content = "Introduction", IsDefault = true, IsEditable = true, VerbiageType = VerbiageType.Introduction });
            //    context.Verbiages.AddOrUpdate(p => p.Name, new Rpo.ApiServices.Model.Models.Verbiage { Name = "scope", Content = "Scope", IsDefault = true, IsEditable = false, VerbiageType = VerbiageType.Scope });
            //    context.Verbiages.AddOrUpdate(p => p.Name, new Rpo.ApiServices.Model.Models.Verbiage { Name = "Additional Scope", Content = "Additional Scope", IsDefault = false, IsEditable = true, VerbiageType = VerbiageType.AdditionalScope });
            //    context.Verbiages.AddOrUpdate(p => p.Name, new Rpo.ApiServices.Model.Models.Verbiage { Name = "Conclusion", Content = "Conclusion", IsDefault = true, IsEditable = true, VerbiageType = VerbiageType.Conclusion });
            //    context.Verbiages.AddOrUpdate(p => p.Name, new Rpo.ApiServices.Model.Models.Verbiage { Name = "cost", Content = "cost", IsDefault = false, IsEditable = false, VerbiageType = VerbiageType.Cost });
            //    context.Verbiages.AddOrUpdate(p => p.Name, new Rpo.ApiServices.Model.Models.Verbiage { Name = "milestone", Content = "milestone", IsDefault = false, IsEditable = false, VerbiageType = VerbiageType.Milestone });
            //    context.Verbiages.AddOrUpdate(p => p.Name, new Rpo.ApiServices.Model.Models.Verbiage { Name = "Sign", Content = "Sign", IsDefault = true, IsEditable = true, VerbiageType = VerbiageType.Sign });

            //    context.SaveChanges();
            //}

            if (!context.Sufixes.Any())
            {
                context.Sufixes.AddOrUpdate(p => p.Description, new Suffix { Description = "AIA" });
                context.Sufixes.AddOrUpdate(p => p.Description, new Suffix { Description = "Jr." });
                context.Sufixes.AddOrUpdate(p => p.Description, new Suffix { Description = "MD" });
                context.Sufixes.AddOrUpdate(p => p.Description, new Suffix { Description = "PE" });
                context.Sufixes.AddOrUpdate(p => p.Description, new Suffix { Description = "RA" });
                context.Sufixes.AddOrUpdate(p => p.Description, new Suffix { Description = "Sr. III" });
                context.SaveChanges();
            }

            if (!context.DEPCostSettings.Any())
            {
                context.DEPCostSettings.AddOrUpdate(p => p.Description, new DEPCostSetting { Name = "Hydrant Permit", Description = "", Price = 55, NumberOfDays = 30 });
                context.DEPCostSettings.AddOrUpdate(p => p.Description, new DEPCostSetting { Name = "Hydrant Water Use", Description = "", Price = 13.5, NumberOfDays = 1 });
                context.SaveChanges();
            }


            if (!context.ContactTitles.Any())
            {
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Account Executive" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Account Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Account Manager/sales" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Account Payable Supervisor" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Account Receivable/Payable" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Account Representative" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Accountant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Accounting Department" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Accounting Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Accounts Department" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Accounts Payable" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Acting Assistant Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Acting Borough Commissioner Of Manhattan" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Acting Chief/External Operations" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Acting Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Acting Deputy Chief Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Acting Deputy Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Acting Director Of Code Development" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Acting Senior Vice President, Network ST" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Admin" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Admin. Staff Analyst" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Administrative Assistant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Administrative Associate" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Administrative Manager And assistant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Administrative Manager/Pens Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Administrator Of Facility" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Advisory Committee" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "AEU Supervisor At Dob" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "After Sales Operational Deputy Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Agency Chief Contracting Officer(ACCO)" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Agent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Agent For Owner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Agent For Shorehaven Condo 1" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "AIA Partner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "AIA Point Of Contact" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Ambassador" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Apartment Owner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "APM" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Arborist" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Architect" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Architect AIA" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Architectural Designer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Architectural Specialist" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Architectural coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Architectural Design Services" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Architectural Designer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Area Chief Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Area Construction Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Arterial Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Artistic Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Asbestos Investigator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Asbestos Surveyor" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "ASID Principal" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Project Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Asset Assistant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Secretary" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Architect" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Chief" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Chief Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Chief Inspector" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Chief Plan" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Civil Project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Council" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Director Of Operation" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Director Of Special Projects" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant District Attorney Trial Bureau" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant General Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Marketing Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Plan Examiner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Procurement Agent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Professor" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Project accountant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Property Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Risk Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Superintendent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant To Andrewj.Bast" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant To Borough Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant To Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant To Downtown Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant To The Chief Financial Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant To The Facilities Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant To The Planning Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Vice President" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Vice President,Plant Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant Vice President,Administration" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assistant commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Assoc.Ex. Direct" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Associate" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Associate Architect" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Associate Attorney" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Associate Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Associate Construction Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Associate Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Associate Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Associate Executive Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Associate Manager Of Civil Engineering" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Associate Partner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Associate Partner AIA Leed Ap" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Associate Principal" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Associate Project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Associate Project Manager Level 111" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Associate Property Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Associate VP" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Associate/Project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Asst District Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Asst Superintendent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Asst. Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Asst. Secretary" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Asst. Superintendent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Attorney" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Attorney & Counselor At Law" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Attorney at Law" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Attorneys & Counselors At Law" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Attorneys at Law" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Auth. Sign" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Auth. Signatory" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Authorized  Agent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Authorized  Signatory" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Authorized Signer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Bank officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Bellvve Hospital, Modernization" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Benefits Consultant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Billing Address" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Bim Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Board Chairperson" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Board member" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Board President" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Board v.p." });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Bookkeeper" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Borough Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Borough Commissioner-Manhattan" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Borough Commissioner, Brooklyn" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Borough Commissioner,Manhattan" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Borough Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Borough  Engineer, Manhattan" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Borough   Engineer-Manhattan" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Borough  Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Borough Planner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Borough Traffic Comm. Of Brooklyn" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Branch Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Broad President" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Broadcast Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Broker" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Broker Associate" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Bronx Borough Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Bronx Coordinator, Special Operations" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Brooklyn Borough Coordinator Hiqa" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Brooklyn Borough Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Brooklyn Borough Traffic Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Brooklyn Borough Traffic Engr." });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Brooklyn Lng Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Brooklyn Forestry Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Building Code Specialist" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Building Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Building Owner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Building Vault Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Bureau Of Water & Sewer Operations,Cros" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Bus Shelter Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Bus Stop Management" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Business Development Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Business Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "C.E.O." });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "C.E.O. & President" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "C.O.O" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Cad" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Canopy Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Cao" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Captain" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Carpenter" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Ccm" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "CEO" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Certificate  Of  Occupancy Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Certified Building Commissioning Profess" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Certified Public Accountant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "CFO" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chairman" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chairman & CEO" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chairman and CEO" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chairman, CEO" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chairman,Chief Executive" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chairman,Co-Chief  Executive" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chairman,President" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief/Owner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Estimator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Plan Examiner-Queens" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Project Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Analysis" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Attorney" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Construction Inspector, Manhattan" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Deputy County Clerk" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Development Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Electrical Inspector" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Engineer Of Waterway Bridges" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Executive" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Executive Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Financial Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Financial Officer / General Counsel" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Forester" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Forester Of The Bronx" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Inspector - Construction Manhattan" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Inspector Of Fire Prevention" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Inspector-Hazard Control Group" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Of Construction Division - Bronx" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Of Architecture And Engineering" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Of City Bridge Maintenance" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Of Const." });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Of Electrical Inspection" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Of Mechanical" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Of Off Street Parking" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Of Operations" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Of Operations Manhattan" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Of Sign Shop" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Of Staff" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Of Traffic Enforcement Agents" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Operating Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Plan Examiner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Plan Examiner / elevator Division" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Plumbing Inspector" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Plumbing Inspector 4/06" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Project Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief Zoning Specialist" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chief, Cuny Programs" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Chiropractor" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "City Dot Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "City Planner-Manhattan Office" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "City Planning Technical / High-Rise" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Civil engineering  Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Civil Superintendent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Claims  Representative" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Clerical  Assistant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Client Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Cm Services For NYCDep" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Co-Chief Executives" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Co-Op Board President" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Co-Op  Shareholder" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Co-Owner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Co-President" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Code & Zoning Specialist" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Code Administrator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Code and Zoning Specialist" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Code Compliance Manager-East Side access" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Code Specialist" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Collection Representative Bureau Of Cust" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "College Aide" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Comm.For T.P.A" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Commander, Usphs" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Commercial Lines Account Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Commercial Lines Underwriter" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Commercial Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Commissioner Of Marine and aviation" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Commissioner, Bureau Of Bridges" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Communication  Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Community Affairs Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Community Affairs Officer, 20th Pct." });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Condo owner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Condo Secretary" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Condo Unit Owner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Confidential Investigator ll" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Consent Specialist Division Of Franchise" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Construction Administration Specialist" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Construction Appointments" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Construction Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Construction forestry Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Construction Inspector" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Construction Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Construction Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Construction Project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Construction Representative" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Construction section squad Leader" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Construction Site Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Consultant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Consultant / fitter" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Consultant To The Building Industry" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Consulting Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Consulting Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Consulting Structural Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Contract analyst/Expense Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Contract Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Contract Manager, APM" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Contractor" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Controlled inspections" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Controller" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Coo/CFO" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Coordinator Of Special Events" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Coordinator,Adopt-a-Highway" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Corp Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Corp. Sec." });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Corporate Occupier & Investor Services" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Corporate Paralegal" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Corporate Safety Administrator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Cost Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Cosmetic Dentistry" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Council Member" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Council On Foreign Relations" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Counsel,Supreme Court Division" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Counsellor At Law" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Counselor at Law" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Cpe Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "CPO" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Cranes And Derricks" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Customer Project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Dasny" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Dentist" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Dep. Dir. Of Technical Services" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Department Head, Building Structures" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Dept. Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy assistant Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Borough Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy borough engr Of Manhattan" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Chief Engineer Of Construction" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Chief" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Chief Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Chief Inspector" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Chief Inspector, Cda Unit" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy chief Of Operations" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Chief Project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy  Dir,Arterial Coordination" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Director - Queens Forestry" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Director Of Bus Stop Management" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Director Of Cip/Ca Studio" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Director Of Construction" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Director Of Events" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Director Of Gov Relations" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Director Of Labor Relations" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Director Of Parking Meters" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Director, Dot-Special Events" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Director,Route 9A Project" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Executive Director Of Franchise" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Operations Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Permanent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Deputy Program" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Desion Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Designer/sales Consultant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Detective" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Development Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Development Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Dir Fac Mgmt" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Dir. Of Prop" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Dir. Planning & Development" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Dir.Wtrr" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director - Estimating" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director - Lab" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director - Mechanical Engineering" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director - Sapo" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director - Sidewalk Management" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Bridge & Tunnel Operations" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director c.c." });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Dcms" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Global Store Development" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director  Mechanical" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of architecture" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of arterial Maintenance" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Authorized Parking" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Building Compliance" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Buildings,Grounds And Security" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Business Development" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Capital Projects Manhattan" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Cartography" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Code Compliance" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Construction Services" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Construction." });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Construction Coordination" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Construction Services" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Customer Service" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Development" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Development & Asset Management" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Discontinued Operations" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Economic Development" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Economic Development & Commu" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Engineering" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Engineering & Assistant Floo" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Engineering & Operations" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Events" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Field Operations" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Finance" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Forestry" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Golf Division" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director  Of Health-care Architecture" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Housing And Residence Life" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of In-special Inspections" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Intergovernmental And Commun" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Land Contour Program" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Land Scape Development" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Manhattan Public affairs" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Marketing" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Operations" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Operations And Enforcement" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Operations Ny & Nj" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Operations/project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Parking Engineering" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Pharmacy" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Planning, Design & Construct" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Plumbing/ Fire Protection" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Production" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Program Management" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Project Development" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Project Management" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Project Managers" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Properly Management" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Public Safety" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Real Estate Development" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Real Estate Development & Ma" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Relations" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Safety" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Sales" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Sales&Marketing" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Security" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Sidewalk Pedestrian Ramps" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Sign Sales" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Street Lighting" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Sustainability Enforcement" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Traffic Signals" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Training & Development" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Of Waterfront Permits" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Street Furniture Franchise Unit" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director, Arterial Coordination" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director, Borough Operations & Constitute" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director, Bus Stop Management" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director, Capital Projects" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director, Mid-Atlantic Region" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director, Office Of Land Review" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director, Operations & Design" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director, Otcr" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director, Permits & Approvals" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director, Public affairs" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director, Systems Engineering Management" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director-Permit Renewal Unit" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Director Street Maintenance" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Distinctive Sidewalk Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "District Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "District Manager (Below 14ST)" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "District Manager (Lower Manhattan)" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "District Manager,New York State" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "District Regional Sales Manager - New Je" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Division Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Division Of Operations Planning" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Division of School Facilities" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Dmsion Permit Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Divisional Vice President" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Dob Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Dob Filing" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Dot Permit Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Ecb Nyc Branch manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Editor" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Ehs Site Safety Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Electric Service Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Electrical Department Head" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Electrical Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Electrician" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Elevator Division Chief" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Energy And Sustainability Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Energy Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Engineer in charge" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Engineer In Charge Of Bridge Painting" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Engineer In Charge Of Capital Projects" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Engineer In Charge Of East River Bridges" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Engineer On Mcdonald ave/Church Ave" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Engineer-In-Charge, Manhattan" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Engineering assistant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Engineering  Consultant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Engineering  Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Engineering Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Engineering/Estimating  Administrator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Environmental Consultant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Environmental Project Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Equipment Use Permits" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Esq" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Esquire" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Estimator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Estimator/Assistant Project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Event Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Evp" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Examiner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Excavation Unit" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive Administrative Assistant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive  Assistant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive Assistant Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive Assistant To The Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive Director Of Construction Site" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive Director Of Facilities" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive Director Of Ocmc-Highways/Brid" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive Director Of Operations" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive Director, Code Development" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive Director, Cranes And Derricks" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive Director, Csff & Franchises" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive Director, Forensic Engineering" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive Director, Technical Affairs" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive  Director-Ocmc/permit Management" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive Producer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive Vice President" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive Vice President & Coo" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive Vice President For Project Dev" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive Vice President,Construction" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Executive Vice-President" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Expediter" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Export Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Facilities Development" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Facilities Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Field Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Field Accountant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Field Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Field Secretary" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Field Superintendent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Field Supervisor" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Financial adviser" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Financial  Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Fire Department Examiner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Fire Suppression Contractor" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "First Deputy Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "First Vice President" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Fit-out architect" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Forensic Investigator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Forestry" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Forestry Permit Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Former Chief" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Founder & Managing Partner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Founder & Principal" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Franchises, Concessions & Consents" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Full Level tax Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Funeral Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Funeral Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "G.I.S/Event  Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "General  Contractor" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "General Council" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "General Counsel" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "General Foreman" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "General Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "General Manager Of Construction" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "General Partner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "General Superintendent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "General Superintendent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "General Supt." });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Geo-technical Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "h.l.q.a" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Head Of Development" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Head Of School" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Headmaster" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Health Insurance Expert" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "High-Rise Supervisor" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Highway Inspection & Quality Assurance" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Highways" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Historic Battery Park Restoration" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Hospital Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Human Resource Guy" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Hup Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Ign Person" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Implementation specialist" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Inspection Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Inspector" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Inspector - Scaffold Safety team" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Inspector, Commanding Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Inspector/Project Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Installation Supervisor" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Insurance" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Insurance Person" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Intergovernmental & Community Affairs Li" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Interim CEO" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Interior Design" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Interior Designer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Intern" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Investigator - Meter Division" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Isa Certified Arborist" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Its Coord.,Traffic Contr" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Job Captain" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Job  Cost assistant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Key Person" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Labor Foreman" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Laboratory  Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Laborer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Land Owner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Land Representative" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Land Use Review-Legal Affairs" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Lawyer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Lcsw" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Lead Assoc" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Lead Construction Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Lead Estimator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Lead Project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Leed Ap" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Legal Assistant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Licensed Agent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Licensed Real Estate Salesperson" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Llc Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Loading Zones" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Location Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Logistics  Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Lower Manhattan Borough Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Lower Manhattan Police Traffic Person" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "m&e  Construction" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "m.e.p" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Management Executive" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Manager Agent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Manager Of Development" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Manager Of Field Service" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Manager Of Real Estate Administration" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Manager,  Engineering Principal" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Managing Agent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Managing Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Managing Director North East Region" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Managing Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Managing Member" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Managing Partner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Managing Principal" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Manhattan Acting Borough Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Manhattan Borough Chief" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Manhattan Borough Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Manhattan Borough Coordinator Hiqa" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Manhattan Borough Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Manhattan Borough Traffic Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Manhattan Chief Electrical Inspector" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Manhattan Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Manufacturing Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Marketing & Production Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Marketing Associate" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Marketing  Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Master Electrician" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Master  Plumber" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Mechanical Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Mechanical Estimator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Mechanical Project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Member" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Mep Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Mep Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Mep Engineer/superintendent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Mep Project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Meps Project Managers" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Merchandise Planning Operations" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Messenger, Courier" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Meter Division" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Mohammed Assistant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Mortgage Banker" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Mp" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Ms." });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "National Production Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "National Sales Executive" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "New York City Council member" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "New York Licensed Asbestos Inspector" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "New York Metro Director Environmental Op" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Newsstand Division" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Nick's Attorney" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Nj Operations Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "No Longer Works Here" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "No Longer Works There" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Nordest Services, LLC" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "NY Operations Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "NYC Department Of Transportation" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "NYPL, VP Of Design & Construction" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Office  Administration" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Office Administrator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Office Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Office Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Office Manager,Summons Unit" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Offsite Engineering Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Oil burner Installer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Operations Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Operations Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Owner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Owner Rep For Ferry Point Park" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Owner's Agent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Owner's Rep" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Owner's Rep." });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Owner's  Representative" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Owner/President" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Owners Rep" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "p.e." });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Parish Council Member" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Parks Dept Interagency Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Partner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Partner, Professional Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Partner, Project Executive" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Partner/designer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Partner/Director Of Development" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Partner/Shareholder" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Partners" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Pe" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Pelham Line 5 Station rehab" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Permit Clerk" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Permit Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Permits Division" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Pizzarottiibc LLC" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Plan Clerk" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Plan Examiner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Plan Examiner - Builders Pavement" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Planning Division" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Plant Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Plumber" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Plumber (Ps 260)" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Plumbing & Fibre Protection engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Plumbing Chief" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Plumbing Inspector" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Pm" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Pmcs Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Policy Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Preconstruction coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Preconstruction Surveyor" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "President" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Preservation Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "President" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "President & CEO" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "President, Chief Executive" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "President, Chief Operating Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "President/laboratory   Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "President And CEO" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "President And CEO" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "President And Principle Broker" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "President Of Board Of Managers For Shore" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "President Of The Board" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "President, Chief  Executive" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "President,Co-Chief Executive" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "President-Structural Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "President/ Owner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "President/ Technical Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "President/Owner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Principal" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Principal architect" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Principal Director Of New York Studio" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Principal Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Principal Transportation planner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Procurement Agent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Procurement Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Producer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Professor" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Professional Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Program Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Program Director, Cultural Institutions" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project accountant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project administrator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project architect" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Designer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Developer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Director,Special Projects" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project  Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Engineer & Design" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Engineer / Estimator Project Executi ve" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Manager - Mep" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Management Associate Housing And" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Manager & Estimator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Manager - Antenna Division" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Manager - Ocmc Streets" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Manager Assistant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Manager Ocmc Streets" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Manager, Ocmc-Streets" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Manager, Senior Associate" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Manager, Technical Services" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Manager, Urban Land Engineering" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Manager-Administration" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Manager-Interiors" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Officer ll" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Planner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Safety Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Secretary" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Specialist" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Super" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Superintendent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Technician" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Property Development Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Property Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Proprietor" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Public Art Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Public Information Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Public Service Commission" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Purchasing Agent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Purchasing Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Quality Assurance Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Queens - Construction Division" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Queens Borough Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Queens Borough Commissioner's Office" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Queens Borough Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Queens Deputy Borough commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "R.A. Principal" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "RA" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "RA, Ncarb Partner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Rabbi" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Radio Room" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Real Estate Contact" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Real Estate Investments" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Real Estate Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Records Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Rector" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Regional Materials Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Regional Property Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Regional Sales  Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Regional Vice President" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Registered Architect" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Registered Landscape Architect" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Registered Patent Attorney" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Repairs Department Supervisor" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Representative" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Resident Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Resident Engineer Cr0-3120S" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Residential Building Manager Housing & p" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Right Of Way" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Risk Management Administrative Assistant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Risk Management Administrator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Rla- Associate" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Roadwork And Outdoor Specialty Division" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Robert Piccolo" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Route 9A Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Route 9A Project Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Safety And Loss Control Consultant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Safety Consultant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Safety Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Safety Director & Project Superintendent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Safety Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Safety Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Sales & Design" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Sales & Marketing Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Sales Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Sales Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Sales Support" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Sapo Program Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "School Safety Unit" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Second Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Secretary" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Secretary/Reception" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "See Notes" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Self" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Account Executive" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Account Executive/Broker" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Account Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Administrative Marketing Assistant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Architectural Designer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior associate" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior associate director of facilities" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Associate Vice President" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Branch Associate" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior  Construction Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Architectural Designer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Consultant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Deputy Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Design Associate" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Designer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Development Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Electrical Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Estimator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Executive Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior FAA Medical Examiner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Field Superintendent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Loan Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Maintenance Supervisor" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Managing Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Managing Partner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior MEP Project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Pastor" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Pre-construction Executive" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Principal" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Program Manager, Architecture" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Project / Field Assistant Project" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Project advocate" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Project architect" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Project Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Project Executive" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Project Manager Facilities Develop" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Project Manager, Construction" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Project Manager, PE" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Project Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Project Superintendent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Project- Field Assistant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Property Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Staff Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Structural Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Superintendent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Traffic Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Transportation Planner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Vice President" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Vice President Of Security & Operations" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Vice President,Mid-Atlantic Region" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Vice President,Operations Manage" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Vice President-Design & Consulting" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior VP" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Zoning Specialist" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senior Project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Senor Project Architect" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Service & Parts Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Service Adviser" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Service  Department" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Service Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Service Manager -" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Sewer Approvals" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Shareholder" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Shareholder - 88 Cpw 2-3s Trust" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Sidewalk And Inspection Management" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Site Project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Site Representative" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Site Safety Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Site  Safety inspector" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Site Safety Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Site Safety Manager/Csfsm" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Small Business Manager - Midtown" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Special Counsel To Mcsam Hotel Group LLC" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Special Inspector" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Special Projects Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Sr Vice President" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Sr. Construction Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Sr. Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Sr. Engineer, Project Engr" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Sr.  Landscape architect" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Sr. Project Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Sr. Project Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Sr. Project Manager, Leed Ap" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Sr. Project Manager/Architect" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Sr. Purchasing Agent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Sr. Vice President" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Ssm" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Staff Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Staten Island Borough Commissioner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Store Owner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Structural Department Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Structural Designer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Structural engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Structural Section Chief" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Structural Superintendent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Structural/Mechanical/electrical" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Subcontractor" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Super" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Superintendent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Superintendent Of Construction" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Supervising Inspector" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Supervisor" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Supervisor (Construction Division)" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Supervisor window Cleaning Division" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Supt" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Surveying Consultant" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Surveyor" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Svp, Facilities Mgmt & Re Development" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "T.A Liaison For Department Of Transport" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Tech" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Technical" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Technical affairs" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Technical Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Technical Director Of Construction" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Technical Director Of Plumbing & Five Su" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Technical inspector" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Technical Operations Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Technical Principal" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Technology  Management,Bfp" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Telecom Department" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Topographic Planner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Toshiba National Accounts" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Track Superintendent" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Traffic Analyst Support" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Traffic Engineer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Traffic Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Training Coordinator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Transit Planner,Executive v.p." });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Treasurer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Treasurer And vice President For Finance" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Trustee" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Trustee Of Shareholder" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Unit Owner" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "V.P." });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vic President- Engineering" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice Chairman" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice Chairman / Founding Member" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President & Director" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President & Director Of Operations" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President & General Counsel" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President & General Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President - Acquisitions" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President - Chief Operating Officer" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President - Interiors Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President - On campus Development" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President- Operations" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President - Operations Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President / Project Executive" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President And General Counsel" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President And General Manager" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President Asset Management" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President Construction" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President Developing & Construction" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President For Finance & Administrator" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President Manager Of Design" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President Of Acquisitions" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President Of Building Operations" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President Of Business Development" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President Of Construction" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President Of Development" });
                context.ContactTitles.AddOrUpdate(ct => ct.Name, new ContactTitle { Name = "Vice President Of Engineering" });
                context.SaveChanges();
            }

            if (!context.ContactLicenseTypes.Any())
            {
                context.ContactLicenseTypes.AddOrUpdate(ct => ct.Name, new ContactLicenseType { Name = "Architect" });
                context.ContactLicenseTypes.AddOrUpdate(ct => ct.Name, new ContactLicenseType { Name = "Carting" });
                context.ContactLicenseTypes.AddOrUpdate(ct => ct.Name, new ContactLicenseType { Name = "Concrete Safety Manager" });
                context.ContactLicenseTypes.AddOrUpdate(ct => ct.Name, new ContactLicenseType { Name = "Construction Superintendent" });
                context.ContactLicenseTypes.AddOrUpdate(ct => ct.Name, new ContactLicenseType { Name = "Engineer" });
                context.ContactLicenseTypes.AddOrUpdate(ct => ct.Name, new ContactLicenseType { Name = "Expeditor" });
                context.ContactLicenseTypes.AddOrUpdate(ct => ct.Name, new ContactLicenseType { Name = "Fire Suppression Contractor" });
                context.ContactLicenseTypes.AddOrUpdate(ct => ct.Name, new ContactLicenseType { Name = "Landmark Architect" });
                context.ContactLicenseTypes.AddOrUpdate(ct => ct.Name, new ContactLicenseType { Name = "Master Plumber" });
                context.ContactLicenseTypes.AddOrUpdate(ct => ct.Name, new ContactLicenseType { Name = "Oil Burner Installer" });
                context.ContactLicenseTypes.AddOrUpdate(ct => ct.Name, new ContactLicenseType { Name = "Registered Landscape Architect" });
                context.ContactLicenseTypes.AddOrUpdate(ct => ct.Name, new ContactLicenseType { Name = "Sign Hanger" });
                context.ContactLicenseTypes.AddOrUpdate(ct => ct.Name, new ContactLicenseType { Name = "Site Safety Coordinator" });
                context.ContactLicenseTypes.AddOrUpdate(ct => ct.Name, new ContactLicenseType { Name = "Site Safety Manager" });
                context.ContactLicenseTypes.AddOrUpdate(ct => ct.Name, new ContactLicenseType { Name = "Others" });
                context.SaveChanges();
            }

            if (!context.RfpJobTypes.Any())
            {
                context.RfpJobTypes.AddOrUpdate(ct => ct.Name, new RfpJobType { Name = "DOB" });
                context.RfpJobTypes.AddOrUpdate(ct => ct.Name, new RfpJobType { Name = "DOT" });
                context.RfpJobTypes.AddOrUpdate(ct => ct.Name, new RfpJobType { Name = "DEP" });
                context.RfpJobTypes.AddOrUpdate(ct => ct.Name, new RfpJobType { Name = "VIOLATIONS" });
                context.SaveChanges();
            }


            if (!context.JobApplicationTypes.Any())
            {
                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType { Description = "DOB", IdParent = null });
                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType { Description = "DOT", IdParent = null });
                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType { Description = "Violation", IdParent = null });
                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType { Description = "DEP", IdParent = null });

                context.SaveChanges();

                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType
                {
                    Description = "ALT I",
                    IdParent = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id
                });
                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType
                {
                    Description = "ALT II",
                    IdParent = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id
                });

                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType
                {
                    Description = "ALT III",
                    IdParent = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id
                });

                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType
                {
                    Description = "CONDO LOT SUBDIVISION",
                    IdParent = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id
                });

                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType
                {
                    Description = "DOB NOW",
                    IdParent = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id
                });

                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType
                {
                    Description = "FULL DEMOLITION",
                    IdParent = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id
                });
                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType
                {
                    Description = "NEW BUILDING",
                    IdParent = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id
                });

                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType
                {
                    Description = "PLACE OF ASSEMBLY",
                    IdParent = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id
                });


                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType
                {
                    Description = "TAX LOT SUBDIVISION",
                    IdParent = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id
                });



                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType
                {
                    Description = "REGISTRATIONS",
                    IdParent = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id
                });

                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType
                {
                    Description = "MISC",
                    IdParent = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id
                });


                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType
                {
                    Description = "PARKS",
                    IdParent = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id
                });

                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType
                {
                    Description = "CONSUMER AFFAIRS",
                    IdParent = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id
                });

                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType
                {
                    Description = "NYCDOT",
                    IdParent = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOT").Id
                });

                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType
                {
                    Description = "NYSDOT",
                    IdParent = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOT").Id
                });

                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType
                {
                    Description = "OCMC-HL",
                    IdParent = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOT").Id
                });

                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType
                {
                    Description = "NYS-Thruway",
                    IdParent = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOT").Id
                });

                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType
                {
                    Description = "Hydrant",
                    IdParent = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DEP").Id
                });



                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType
                {
                    Description = "Sewer",
                    IdParent = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DEP").Id
                });

                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType
                {
                    Description = "Water",
                    IdParent = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DEP").Id,
                });

                context.JobApplicationTypes.AddOrUpdate(ct => ct.Description, new JobApplicationType
                {
                    Description = "Boiler",
                    IdParent = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DEP").Id
                });

                context.SaveChanges();
            }

            if (!context.JobWorkTypes.Any())
            {
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT I").Id, Description = "Boiler", Code = "BL" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT I").Id, Description = "Fuel Burning", Code = "FB" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT I").Id, Description = "Fuel Storage", Code = "FS" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT I").Id, Description = "Mechanical", Code = "MH" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT I").Id, Description = "Other", Code = "OT" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT I").Id, Description = "Earthwork", Code = "OT/EA" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT I").Id, Description = "Foundation", Code = "OT/FO" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT I").Id, Description = "General Construction", Code = "OT/GC" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT I").Id, Description = "Support of Excavation", Code = "OT/SOE" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT I").Id, Description = "Underpinning", Code = "P OT/UN" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT I").Id, Description = "Plumbing", Code = "PL" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT I").Id, Description = "Standpipe", Code = "SD" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT I").Id, Description = "Sprinkler", Code = "SP" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT I").Id, Description = "Structural", Code = "ST" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT I").Id, Description = "TCO Renewal", Code = "TCO" });

                context.SaveChanges();

                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT II").Id, Description = "Boiler", Code = "BL" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT II").Id, Description = "Fire Alarm modification", Code = "FA/Mod" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT II").Id, Description = "Fire Alarm New", Code = "FA/New" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT II").Id, Description = "Fuel Burning", Code = "FB" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT II").Id, Description = "Fire Suppression", Code = "FP" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT II").Id, Description = "Fuel Storage", Code = "FS" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT II").Id, Description = "Mechanical", Code = "MH" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT II").Id, Description = "Other", Code = "OT" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT II").Id, Description = "Earthwork/Excavation", Code = "OT/EA" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT II").Id, Description = "Foundation", Code = "OT/FO" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT II").Id, Description = "General Construction", Code = "OT/GC" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT II").Id, Description = "Support of Excavation", Code = "OT/SOE" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT II").Id, Description = "Underpinning", Code = "P OT/UN" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT II").Id, Description = "Plumbing", Code = "PL" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT II").Id, Description = "Standpipe", Code = "SD" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT II").Id, Description = "Sprinkler", Code = "SP" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT II").Id, Description = "Structural", Code = "ST" });

                context.SaveChanges();

                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT III").Id, Description = "Builders Pavement Plan", Code = "BPP" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT III").Id, Description = "Chute", Code = "EQ/CHUTE" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT III").Id, Description = "Fire Protection Plan", Code = "FPP" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT III").Id, Description = "Marquee", Code = "MARQUEE" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT III").Id, Description = "General Construction", Code = "OT/GC" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT III").Id, Description = "Landscaping", Code = "OT/LAN" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "ALT III").Id, Description = "Other", Code = "OT" });

                context.SaveChanges();

                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "CONDO LOT SUBDIVISION").Id, Description = "Condo", Code = "Condo" });

                context.SaveChanges();

                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB NOW").Id, Description = "Curb Cut", Code = "CC" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB NOW").Id, Description = "EQ Fence", Code = "EQ- FN" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB NOW").Id, Description = "EQ- Scaffold", Code = "EQ- Scaffold" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB NOW").Id, Description = "EQ- Sidewalk Shed", Code = "EQ-SHED" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB NOW").Id, Description = "Antenna", Code = "OT/ANT" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB NOW").Id, Description = "Plumbing", Code = "PL" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB NOW").Id, Description = "Standpipe", Code = "SD" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB NOW").Id, Description = "Sign", Code = "Sign" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB NOW").Id, Description = "Sprinkler", Code = "SP" });

                context.SaveChanges();

                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "CONSUMER AFFAIRS").Id, Description = "Sidewalk Café Filing", Code = "SWC" });

                context.SaveChanges();

                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "FULL DEMOLITION").Id, Description = "DM", Code = "Demolition" });

                context.SaveChanges();

                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "TAX LOT SUBDIVISION").Id, Description = "Subdivision/Merger", Code = "SDIV" });

                context.SaveChanges();

                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "PLACE OF ASSEMBLY").Id, Description = "Place of Assembly", Code = "POA" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "PLACE OF ASSEMBLY").Id, Description = "Temporary Place of Assembly", Code = "TPA" });

                context.SaveChanges();

                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NEW BUILDING").Id, Description = "Boiler", Code = "BL" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NEW BUILDING").Id, Description = "Fuel Burning", Code = "FB" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NEW BUILDING").Id, Description = "Fuel Storage", Code = "FS" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NEW BUILDING").Id, Description = "Mechanical", Code = "MH" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NEW BUILDING").Id, Description = "New Building", Code = "NB" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NEW BUILDING").Id, Description = "Other", Code = "OT" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NEW BUILDING").Id, Description = "Earthwork/Excavation", Code = "OT/EA" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NEW BUILDING").Id, Description = "Foundation", Code = "OT/FO" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NEW BUILDING").Id, Description = "Support of Excavation", Code = "OT/SOE" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NEW BUILDING").Id, Description = "Underpinning", Code = "P OT/UN" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NEW BUILDING").Id, Description = "Plumbing", Code = "PL" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NEW BUILDING").Id, Description = "Standpipe", Code = "SD" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NEW BUILDING").Id, Description = "Sprinkler", Code = "SP" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NEW BUILDING").Id, Description = "Structural", Code = "ST" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NEW BUILDING").Id, Description = "TCO Renewal", Code = "TCO" });

                context.SaveChanges();

                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "REGISTRATIONS").Id, Description = "New Safety Reg", Code = "New Safety Reg" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "REGISTRATIONS").Id, Description = "Renew Safety Reg - no change", Code = "Renew Safety Reg - no change" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "REGISTRATIONS").Id, Description = "Renew Safety Reg - with change", Code = "Renew Safety Reg - with change" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "REGISTRATIONS").Id, Description = "New General Contractor Reg", Code = "New General Contractor Reg" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "REGISTRATIONS").Id, Description = "Renew General Contractor Reg - no change", Code = "Renew General Contractor Reg - no change" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "REGISTRATIONS").Id, Description = "Renew General Contractor Reg - with change", Code = "Renew General Contractor Reg - with change" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "REGISTRATIONS").Id, Description = "New Constr Supt Reg.", Code = "New Constr Supt Reg." });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "REGISTRATIONS").Id, Description = "Renew Constr Supt Reg.", Code = "Renew Constr Supt Reg." });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "REGISTRATIONS").Id, Description = "New Tracking No.", Code = "New Tracking No." });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "REGISTRATIONS").Id, Description = "Insurance Update", Code = "Insurance Update" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "REGISTRATIONS").Id, Description = "New DOB NOW acct", Code = "New DOB NOW acct" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "REGISTRATIONS").Id, Description = "New E-Filing acct", Code = "New E-Filing acct" });

                context.SaveChanges();

                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "MISC").Id, Description = "FDNY Fuel Storage", Code = "Fuel Strg" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "MISC").Id, Description = "Site Safety Plan Approval with BEST", Code = "SSP - BEST" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "MISC").Id, Description = "Site Safety Plan Approval non BEST", Code = "SSP" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "MISC").Id, Description = "Temp Hoist Filing", Code = "Hoist filing" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "MISC").Id, Description = "Elev Device Removal", Code = "Elev Removal" });

                context.SaveChanges();

                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "PARKS").Id, Description = "Elev Removal", Code = "Elev Device Removal" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "PARKS").Id, Description = "Tree Plan Approval", Code = "Tree Filing" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "PARKS").Id, Description = "Tree Planting Permit", Code = "Tree Permit" });

                context.SaveChanges();

                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Open Sidewalk To Install Foundation", Code = "0100", Content = "Open Sidewalk To Install Foundation" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Transformer Vault - In Roadway", Code = "0106", Content = "Transformer Vault - In Roadway" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Transformer Vault - In Sidewalk Area", Code = "0107", Content = "Transformer Vault - In Sidewalk Area" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Major Installations - Cable", Code = "0110", Content = "Major Installations - Cable" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Rapid Transit Construct/ Alteration", Code = "0112", Content = "Rapid Transit Construct/ Alteration" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Fuel Oil Line", Code = "0116", Content = "Fuel Oil Line" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Vault Construction Or Alteration", Code = "0117", Content = "Vault Construction Or Alteration" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Reset,Repair Or Replace Curb", Code = "0118", Content = "Reset,Repair Or Replace Curb" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Pave Street-w/ Engineering & Insp Fee", Code = "0119", Content = "Pave Street-w/ Engineering & Insp Fee" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Tree Pits", Code = "0120", Content = "Tree Pits" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Construct Or Alter Manhole &/or Casting", Code = "0121", Content = "Construct Or Alter Manhole &/or Casting" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Repair Electric/Communications", Code = "0124", Content = "Repair Electric/Communications" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Leader Drain Under Sidewalk", Code = "0125", Content = "Leader Drain Under Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Test Pits. Cores Or Boring", Code = "0126", Content = "Test Pits. Cores Or Boring" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Conduit Construction And Franchise", Code = "0127", Content = "Conduit Construction And Franchise" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Install Street Furniture", Code = "0129", Content = "Install Street Furniture" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Land Fill", Code = "0130", Content = "Land Fill" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Private Sewer", Code = "0131", Content = "Private Sewer" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Install Fence", Code = "0132", Content = "Install Fence" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Install Traffic Signals", Code = "0133", Content = "Install Traffic Signals" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Repair Petroleum Leak", Code = "0134", Content = "Repair Petroleum Leak" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Final Restoration Only", Code = "0135", Content = "Final Restoration Only" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Installation Of Fire Alarm Box", Code = "0138", Content = "Installation Of Fire Alarm Box" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Installation Of Bus Shelter", Code = "0139", Content = "Installation Of Bus Shelter" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Inst./Remove Public Phone/Tech-Kiosk", Code = "0151", Content = "Inst./Remove Public Phone/Tech-Kiosk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "St.Opng/Install.Test Pit/Monit Well/Pipe", Code = "0153", Content = "St.Opng/Install.Test Pit/Monit Well/Pipe" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Install Traffic Street Lights", Code = "0154", Content = "Install Traffic Street Lights" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Regrade/Replace Street Hardware/Casting", Code = "0155", Content = "Regrade/Replace Street Hardware/Casting" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Repair Traffic Street Light", Code = "0156", Content = "Repair Traffic Street Light" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Repair Traffic Signals", Code = "0157", Content = "Repair Traffic Signals" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Install Newsstand", Code = "0165", Content = "Install Newsstand" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Pavement Cores", Code = "0166", Content = "Pavement Cores" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Restoration Re-Dig", Code = "0167", Content = "Restoration Re-Dig" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Core Re-Dig", Code = "0168", Content = "Core Re-Dig" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Micro Trenching", Code = "0172", Content = "Micro Trenching" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Regrade Hardware", Code = "0173", Content = "Regrade Hardware" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Tree Pit/Storm Water Inlet", Code = "0180", Content = "Tree Pit/Storm Water Inlet" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Installation Of Poles", Code = "0181", Content = "Installation Of Poles" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Removal Of Poles", Code = "0182", Content = "Removal Of Poles" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Relocation Of Poles", Code = "0183", Content = "Relocation Of Poles" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Replacement Of Poles", Code = "0184", Content = "Replacement Of Poles" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Place Material On Street", Code = "0201", Content = "Place Material On Street" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Crossing Sidewalk", Code = "0202", Content = "Crossing Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Place Crane Or Shovel On Street", Code = "0203", Content = "Place Crane Or Shovel On Street" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Place Equipment Other Than Crane Or Shov", Code = "0204", Content = "Place Equipment Other Than Crane Or Shov" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Place Shanty Or Trailer On Street", Code = "0205", Content = "Place Shanty Or Trailer On Street" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Temporary Pedestrian Walk", Code = "0208", Content = "Temporary Pedestrian Walk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Install Temp. Decorative Light On Street", Code = "0210", Content = "Install Temp. Decorative Light On Street" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Occupancy Of Roadway As Stipulated", Code = "0211", Content = "Occupancy Of Roadway As Stipulated" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Place Container On The Street", Code = "0214", Content = "Place Container On The Street" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Occupancy Of Sidewalk As Stipulated", Code = "0215", Content = "Occupancy Of Sidewalk As Stipulated" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Install Bike Rack On Sidewalk", Code = "0218", Content = "Install Bike Rack On Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Place Temporary Security Structure", Code = "0219", Content = "Place Temporary Security Structure" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Temporary Construction Signs/Markings", Code = "0221", Content = "Temporary Construction Signs/Markings" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Repair Sidewalk", Code = "0401", Content = "Repair Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Construct New Sidewalk", Code = "0402", Content = "Construct New Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Replace Sidewalk", Code = "0403", Content = "Replace Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Construct New Sidewalk With Heating Pipe", Code = "0404", Content = "Construct New Sidewalk With Heating Pipe" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYCDOT").Id, Description = "Construct New Sidewalk Blg.Pavement", Code = "0405", Content = "Construct New Sidewalk Blg.Pavement" });

                context.SaveChanges();

                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Open Sidewalk To Install Foundation", Code = "0100", Content = "Open Sidewalk To Install Foundation" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Transformer Vault - In Roadway", Code = "0106", Content = "Transformer Vault - In Roadway" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Transformer Vault - In Sidewalk Area", Code = "0107", Content = "Transformer Vault - In Sidewalk Area" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Major Installations - Cable", Code = "0110", Content = "Major Installations - Cable" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Rapid Transit Construct/ Alteration", Code = "0112", Content = "Rapid Transit Construct/ Alteration" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Fuel Oil Line", Code = "0116", Content = "Fuel Oil Line" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Vault Construction Or Alteration", Code = "0117", Content = "Vault Construction Or Alteration" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Reset,Repair Or Replace Curb", Code = "0118", Content = "Reset,Repair Or Replace Curb" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Pave Street-w/ Engineering & Insp Fee", Code = "0119", Content = "Pave Street-w/ Engineering & Insp Fee" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Tree Pits", Code = "0120", Content = "Tree Pits" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Construct Or Alter Manhole &/or Casting", Code = "0121", Content = "Construct Or Alter Manhole &/or Casting" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Repair Electric/Communications", Code = "0124", Content = "Repair Electric/Communications" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Leader Drain Under Sidewalk", Code = "0125", Content = "Leader Drain Under Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Test Pits. Cores Or Boring", Code = "0126", Content = "Test Pits. Cores Or Boring" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Conduit Construction And Franchise", Code = "0127", Content = "Conduit Construction And Franchise" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Install Street Furniture", Code = "0129", Content = "Install Street Furniture" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Land Fill", Code = "0130", Content = "Land Fill" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Private Sewer", Code = "0131", Content = "Private Sewer" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Install Fence", Code = "0132", Content = "Install Fence" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Install Traffic Signals", Code = "0133", Content = "Install Traffic Signals" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Repair Petroleum Leak", Code = "0134", Content = "Repair Petroleum Leak" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Final Restoration Only", Code = "0135", Content = "Final Restoration Only" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Installation Of Fire Alarm Box", Code = "0138", Content = "Installation Of Fire Alarm Box" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Installation Of Bus Shelter", Code = "0139", Content = "Installation Of Bus Shelter" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Inst./Remove Public Phone/Tech-Kiosk", Code = "0151", Content = "Inst./Remove Public Phone/Tech-Kiosk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "St.Opng/Install.Test Pit/Monit Well/Pipe", Code = "0153", Content = "St.Opng/Install.Test Pit/Monit Well/Pipe" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Install Traffic Street Lights", Code = "0154", Content = "Install Traffic Street Lights" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Regrade/Replace Street Hardware/Casting", Code = "0155", Content = "Regrade/Replace Street Hardware/Casting" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Repair Traffic Street Light", Code = "0156", Content = "Repair Traffic Street Light" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Repair Traffic Signals", Code = "0157", Content = "Repair Traffic Signals" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Install Newsstand", Code = "0165", Content = "Install Newsstand" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Pavement Cores", Code = "0166", Content = "Pavement Cores" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Restoration Re-Dig", Code = "0167", Content = "Restoration Re-Dig" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Core Re-Dig", Code = "0168", Content = "Core Re-Dig" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Micro Trenching", Code = "0172", Content = "Micro Trenching" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Regrade Hardware", Code = "0173", Content = "Regrade Hardware" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Tree Pit/Storm Water Inlet", Code = "0180", Content = "Tree Pit/Storm Water Inlet" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Installation Of Poles", Code = "0181", Content = "Installation Of Poles" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Removal Of Poles", Code = "0182", Content = "Removal Of Poles" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Relocation Of Poles", Code = "0183", Content = "Relocation Of Poles" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Replacement Of Poles", Code = "0184", Content = "Replacement Of Poles" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Place Material On Street", Code = "0201", Content = "Place Material On Street" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Crossing Sidewalk", Code = "0202", Content = "Crossing Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Place Crane Or Shovel On Street", Code = "0203", Content = "Place Crane Or Shovel On Street" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Place Equipment Other Than Crane Or Shov", Code = "0204", Content = "Place Equipment Other Than Crane Or Shov" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Place Shanty Or Trailer On Street", Code = "0205", Content = "Place Shanty Or Trailer On Street" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Temporary Pedestrian Walk", Code = "0208", Content = "Temporary Pedestrian Walk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Install Temp. Decorative Light On Street", Code = "0210", Content = "Install Temp. Decorative Light On Street" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Occupancy Of Roadway As Stipulated", Code = "0211", Content = "Occupancy Of Roadway As Stipulated" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Place Container On The Street", Code = "0214", Content = "Place Container On The Street" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Occupancy Of Sidewalk As Stipulated", Code = "0215", Content = "Occupancy Of Sidewalk As Stipulated" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Install Bike Rack On Sidewalk", Code = "0218", Content = "Install Bike Rack On Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Place Temporary Security Structure", Code = "0219", Content = "Place Temporary Security Structure" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Temporary Construction Signs/Markings", Code = "0221", Content = "Temporary Construction Signs/Markings" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Repair Sidewalk", Code = "0401", Content = "Repair Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Construct New Sidewalk", Code = "0402", Content = "Construct New Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Replace Sidewalk", Code = "0403", Content = "Replace Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Construct New Sidewalk With Heating Pipe", Code = "0404", Content = "Construct New Sidewalk With Heating Pipe" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYSDOT").Id, Description = "Construct New Sidewalk Blg.Pavement", Code = "0405", Content = "Construct New Sidewalk Blg.Pavement" });

                context.SaveChanges();

                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Open Sidewalk To Install Foundation", Code = "0100", Content = "Open Sidewalk To Install Foundation" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Transformer Vault - In Roadway", Code = "0106", Content = "Transformer Vault - In Roadway" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Transformer Vault - In Sidewalk Area", Code = "0107", Content = "Transformer Vault - In Sidewalk Area" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Major Installations - Cable", Code = "0110", Content = "Major Installations - Cable" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Rapid Transit Construct/ Alteration", Code = "0112", Content = "Rapid Transit Construct/ Alteration" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Fuel Oil Line", Code = "0116", Content = "Fuel Oil Line" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Vault Construction Or Alteration", Code = "0117", Content = "Vault Construction Or Alteration" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Reset,Repair Or Replace Curb", Code = "0118", Content = "Reset,Repair Or Replace Curb" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Pave Street-w/ Engineering & Insp Fee", Code = "0119", Content = "Pave Street-w/ Engineering & Insp Fee" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Tree Pits", Code = "0120", Content = "Tree Pits" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Construct Or Alter Manhole &/or Casting", Code = "0121", Content = "Construct Or Alter Manhole &/or Casting" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Repair Electric/Communications", Code = "0124", Content = "Repair Electric/Communications" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Leader Drain Under Sidewalk", Code = "0125", Content = "Leader Drain Under Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Test Pits. Cores Or Boring", Code = "0126", Content = "Test Pits. Cores Or Boring" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Conduit Construction And Franchise", Code = "0127", Content = "Conduit Construction And Franchise" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Install Street Furniture", Code = "0129", Content = "Install Street Furniture" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Land Fill", Code = "0130", Content = "Land Fill" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Private Sewer", Code = "0131", Content = "Private Sewer" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Install Fence", Code = "0132", Content = "Install Fence" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Install Traffic Signals", Code = "0133", Content = "Install Traffic Signals" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Repair Petroleum Leak", Code = "0134", Content = "Repair Petroleum Leak" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Final Restoration Only", Code = "0135", Content = "Final Restoration Only" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Installation Of Fire Alarm Box", Code = "0138", Content = "Installation Of Fire Alarm Box" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Installation Of Bus Shelter", Code = "0139", Content = "Installation Of Bus Shelter" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Inst./Remove Public Phone/Tech-Kiosk", Code = "0151", Content = "Inst./Remove Public Phone/Tech-Kiosk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "St.Opng/Install.Test Pit/Monit Well/Pipe", Code = "0153", Content = "St.Opng/Install.Test Pit/Monit Well/Pipe" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Install Traffic Street Lights", Code = "0154", Content = "Install Traffic Street Lights" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Regrade/Replace Street Hardware/Casting", Code = "0155", Content = "Regrade/Replace Street Hardware/Casting" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Repair Traffic Street Light", Code = "0156", Content = "Repair Traffic Street Light" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Repair Traffic Signals", Code = "0157", Content = "Repair Traffic Signals" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Install Newsstand", Code = "0165", Content = "Install Newsstand" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Pavement Cores", Code = "0166", Content = "Pavement Cores" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Restoration Re-Dig", Code = "0167", Content = "Restoration Re-Dig" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Core Re-Dig", Code = "0168", Content = "Core Re-Dig" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Micro Trenching", Code = "0172", Content = "Micro Trenching" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Regrade Hardware", Code = "0173", Content = "Regrade Hardware" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Tree Pit/Storm Water Inlet", Code = "0180", Content = "Tree Pit/Storm Water Inlet" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Installation Of Poles", Code = "0181", Content = "Installation Of Poles" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Removal Of Poles", Code = "0182", Content = "Removal Of Poles" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Relocation Of Poles", Code = "0183", Content = "Relocation Of Poles" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Replacement Of Poles", Code = "0184", Content = "Replacement Of Poles" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Place Material On Street", Code = "0201", Content = "Place Material On Street" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Crossing Sidewalk", Code = "0202", Content = "Crossing Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Place Crane Or Shovel On Street", Code = "0203", Content = "Place Crane Or Shovel On Street" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Place Equipment Other Than Crane Or Shov", Code = "0204", Content = "Place Equipment Other Than Crane Or Shov" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Place Shanty Or Trailer On Street", Code = "0205", Content = "Place Shanty Or Trailer On Street" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Temporary Pedestrian Walk", Code = "0208", Content = "Temporary Pedestrian Walk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Install Temp. Decorative Light On Street", Code = "0210", Content = "Install Temp. Decorative Light On Street" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Occupancy Of Roadway As Stipulated", Code = "0211", Content = "Occupancy Of Roadway As Stipulated" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Place Container On The Street", Code = "0214", Content = "Place Container On The Street" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Occupancy Of Sidewalk As Stipulated", Code = "0215", Content = "Occupancy Of Sidewalk As Stipulated" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Install Bike Rack On Sidewalk", Code = "0218", Content = "Install Bike Rack On Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Place Temporary Security Structure", Code = "0219", Content = "Place Temporary Security Structure" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Temporary Construction Signs/Markings", Code = "0221", Content = "Temporary Construction Signs/Markings" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Repair Sidewalk", Code = "0401", Content = "Repair Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Construct New Sidewalk", Code = "0402", Content = "Construct New Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Replace Sidewalk", Code = "0403", Content = "Replace Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Construct New Sidewalk With Heating Pipe", Code = "0404", Content = "Construct New Sidewalk With Heating Pipe" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "OCMC-HL").Id, Description = "Construct New Sidewalk Blg.Pavement", Code = "0405", Content = "Construct New Sidewalk Blg.Pavement" });

                context.SaveChanges();

                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Open Sidewalk To Install Foundation", Code = "0100", Content = "Open Sidewalk To Install Foundation" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Transformer Vault - In Roadway", Code = "0106", Content = "Transformer Vault - In Roadway" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Transformer Vault - In Sidewalk Area", Code = "0107", Content = "Transformer Vault - In Sidewalk Area" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Major Installations - Cable", Code = "0110", Content = "Major Installations - Cable" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Rapid Transit Construct/ Alteration", Code = "0112", Content = "Rapid Transit Construct/ Alteration" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Fuel Oil Line", Code = "0116", Content = "Fuel Oil Line" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Vault Construction Or Alteration", Code = "0117", Content = "Vault Construction Or Alteration" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Reset,Repair Or Replace Curb", Code = "0118", Content = "Reset,Repair Or Replace Curb" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Pave Street-w/ Engineering & Insp Fee", Code = "0119", Content = "Pave Street-w/ Engineering & Insp Fee" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Tree Pits", Code = "0120", Content = "Tree Pits" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Construct Or Alter Manhole &/or Casting", Code = "0121", Content = "Construct Or Alter Manhole &/or Casting" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Repair Electric/Communications", Code = "0124", Content = "Repair Electric/Communications" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Leader Drain Under Sidewalk", Code = "0125", Content = "Leader Drain Under Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Test Pits. Cores Or Boring", Code = "0126", Content = "Test Pits. Cores Or Boring" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Conduit Construction And Franchise", Code = "0127", Content = "Conduit Construction And Franchise" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Install Street Furniture", Code = "0129", Content = "Install Street Furniture" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Land Fill", Code = "0130", Content = "Land Fill" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Private Sewer", Code = "0131", Content = "Private Sewer" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Install Fence", Code = "0132", Content = "Install Fence" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Install Traffic Signals", Code = "0133", Content = "Install Traffic Signals" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Repair Petroleum Leak", Code = "0134", Content = "Repair Petroleum Leak" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Final Restoration Only", Code = "0135", Content = "Final Restoration Only" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Installation Of Fire Alarm Box", Code = "0138", Content = "Installation Of Fire Alarm Box" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Installation Of Bus Shelter", Code = "0139", Content = "Installation Of Bus Shelter" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Inst./Remove Public Phone/Tech-Kiosk", Code = "0151", Content = "Inst./Remove Public Phone/Tech-Kiosk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "St.Opng/Install.Test Pit/Monit Well/Pipe", Code = "0153", Content = "St.Opng/Install.Test Pit/Monit Well/Pipe" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Install Traffic Street Lights", Code = "0154", Content = "Install Traffic Street Lights" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Regrade/Replace Street Hardware/Casting", Code = "0155", Content = "Regrade/Replace Street Hardware/Casting" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Repair Traffic Street Light", Code = "0156", Content = "Repair Traffic Street Light" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Repair Traffic Signals", Code = "0157", Content = "Repair Traffic Signals" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Install Newsstand", Code = "0165", Content = "Install Newsstand" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Pavement Cores", Code = "0166", Content = "Pavement Cores" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Restoration Re-Dig", Code = "0167", Content = "Restoration Re-Dig" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Core Re-Dig", Code = "0168", Content = "Core Re-Dig" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Micro Trenching", Code = "0172", Content = "Micro Trenching" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Regrade Hardware", Code = "0173", Content = "Regrade Hardware" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Tree Pit/Storm Water Inlet", Code = "0180", Content = "Tree Pit/Storm Water Inlet" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Installation Of Poles", Code = "0181", Content = "Installation Of Poles" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Removal Of Poles", Code = "0182", Content = "Removal Of Poles" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Relocation Of Poles", Code = "0183", Content = "Relocation Of Poles" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Replacement Of Poles", Code = "0184", Content = "Replacement Of Poles" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Place Material On Street", Code = "0201", Content = "Place Material On Street" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Crossing Sidewalk", Code = "0202", Content = "Crossing Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Place Crane Or Shovel On Street", Code = "0203", Content = "Place Crane Or Shovel On Street" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Place Equipment Other Than Crane Or Shov", Code = "0204", Content = "Place Equipment Other Than Crane Or Shov" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Place Shanty Or Trailer On Street", Code = "0205", Content = "Place Shanty Or Trailer On Street" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Temporary Pedestrian Walk", Code = "0208", Content = "Temporary Pedestrian Walk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Install Temp. Decorative Light On Street", Code = "0210", Content = "Install Temp. Decorative Light On Street" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Occupancy Of Roadway As Stipulated", Code = "0211", Content = "Occupancy Of Roadway As Stipulated" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Place Container On The Street", Code = "0214", Content = "Place Container On The Street" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Occupancy Of Sidewalk As Stipulated", Code = "0215", Content = "Occupancy Of Sidewalk As Stipulated" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Install Bike Rack On Sidewalk", Code = "0218", Content = "Install Bike Rack On Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Place Temporary Security Structure", Code = "0219", Content = "Place Temporary Security Structure" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Temporary Construction Signs/Markings", Code = "0221", Content = "Temporary Construction Signs/Markings" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Repair Sidewalk", Code = "0401", Content = "Repair Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Construct New Sidewalk", Code = "0402", Content = "Construct New Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Replace Sidewalk", Code = "0403", Content = "Replace Sidewalk" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Construct New Sidewalk With Heating Pipe", Code = "0404", Content = "Construct New Sidewalk With Heating Pipe" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "NYS-Thruway").Id, Description = "Construct New Sidewalk Blg.Pavement", Code = "0405", Content = "Construct New Sidewalk Blg.Pavement" });

                context.SaveChanges();

                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "Hydrant").Id, Description = "Hydrant Water", Code = "DEPHW" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "Hydrant").Id, Description = "Hydrant Tap", Code = "DEPHT" });

                context.SaveChanges();

                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "Sewer").Id, Description = "Sewer Tap", Code = "DEPST" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "Sewer").Id, Description = "Sewer Plug", Code = "DEPSP" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "Sewer").Id, Description = "Sewer Relay", Code = "DEPSR" });

                context.SaveChanges();

                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "Water").Id, Description = "Water Tap", Code = "DEPWT" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "Water").Id, Description = "Water Wet Connection", Code = "DEPWWC" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "Water").Id, Description = "Plug Water", Code = "DEPPW" });
                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "Water").Id, Description = "Water Meter", Code = "DEPWM" });

                context.SaveChanges();

                context.JobWorkTypes.AddOrUpdate(ct => new { ct.Description, ct.IdJobApplicationType }, new JobWorkType { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "Boiler").Id, Description = "Boiler", Code = "DEPB" });

                context.SaveChanges();
            }


            if (!context.Contacts.Any())
            {
                context.Contacts.AddOrUpdate(
                c => c.FirstName,
                new Contact
                {
                    LastName = "Jhonson",
                    MiddleName = "M",
                    FirstName = "Mick",
                    Email = "someone@gmail.com",
                    BirthDate = new DateTime(1997, 04, 03),
                    MobilePhone = "8793458765",
                    IdPrefix = context.Prefixes.FirstOrDefault().Id,
                    IdContactTitle = context.ContactTitles.FirstOrDefault().Id
                }
            );
                context.SaveChanges();
            }

            //if (!context.WorkTypes.Any())
            //{
            //    context.WorkTypes.AddOrUpdate(wt => wt.Description,

            //        new WorkType { Content = "Establish new Safety Registration Endorsement", Description = "Safety Reg" },
            //        new WorkType { Content = "Renew / Update Safety Registration Endorsement", Description = "Renew Safety Reg" },
            //        new WorkType { Content = "Site Superintendent Registration", Description = "Site Super Reg" },
            //        new WorkType { Content = "Establish Insurance Tracking Number", Description = "New tracking no" },
            //        new WorkType { Content = "Establish General Contractor Registration", Description = "New contractor reg" },
            //        new WorkType { Content = "Renew General Contractor Registration", Description = "New contractor reg" },
            //        new WorkType { Content = "Update Insurance for existing account and track expiration", Description = "Update ins" },
            //        new WorkType { Content = "Update additional policies for existing account simultaneous to primary renewal", Description = "Update ins xtra" },
            //        new WorkType { Content = "Set up Inspection Ready / DOB NOW account", Description = "DOB NOW" },
            //        new WorkType { Content = "Filing service includes preparation of formwork, cursory review of plans for formatting compliance, submission of initial inspection designations based on requirements supplied by applicant of record, schedule Department of Buildings preliminary inspection and obtain approval of the application", Description = "Demolition" },
            //        new WorkType { Content = "General Construciton", Description = "OT/GC" },
            //        new WorkType { Content = "Other, Describe: (fillable field)", Description = "OT" },
            //        new WorkType { Content = "Mechanical", Description = "MH" },
            //        new WorkType { Content = "Plumbing", Description = "PL" },
            //        new WorkType { Content = "Structural", Description = "ST" },
            //        new WorkType { Content = "Sprinkler", Description = "SP" },
            //        new WorkType { Content = "Standpipe", Description = "SD" },
            //        new WorkType { Content = "Boiler", Description = "BL" },
            //        new WorkType { Content = "Fuel Burning", Description = "FB" },
            //        new WorkType { Content = "Fuel Storage", Description = "FS" },
            //        new WorkType { Content = "Foundation", Description = "OT/FO" },
            //        new WorkType { Content = "Excavation", Description = "OT/EX" },
            //        new WorkType { Content = "Support of Excavation", Description = "OT/SOE" },
            //        new WorkType { Content = "Underpinning", Description = "OT/UN" },
            //        new WorkType { Content = "Earthwork", Description = "OT/EA" },
            //        new WorkType { Content = "Prepare Alteration type II filing for fire alarm system. We will file and obtain approval from the NYC Department of Buildings and the NYC Fire Department. Owner to supply base building Letter of Approval for existing system modifications.", Description = "FA" },
            //        new WorkType { Content = "Prepare Alteration type II filing for fire suppression system. We will file and obtain approval from the NYC Department of Department of Small Business Services and the NYC Fire Department. Owner to supply base building Letter of Approval for existing system modifications.", Description = "FS" },
            //        new WorkType { Content = "Prepare Alteration type II filing for pre action system. We will file and obtain approval from the NYC Department of Buildings and the NYC Fire Department.", Description = "PRE ACTION" },
            //        new WorkType { Content = "Curb Cut", Description = "CC" },
            //        new WorkType { Content = "Landscape", Description = "OT/LAN" },
            //        new WorkType { Content = "Antenna", Description = "OT/ANT" },
            //        new WorkType { Content = "Marquee", Description = "OT/MAR" },
            //        new WorkType { Content = "Fence", Description = "EQ/FENCE" },
            //        new WorkType { Content = "Scoffold", Description = "EQ/SCAFFOLD" },
            //        new WorkType { Content = "Shed", Description = "EQ/SHED" },
            //        new WorkType { Content = "Chute", Description = "EQ/CHUTE" }
            //        );

            //    context.SaveChanges();

            //    context.JobTypes.AddOrUpdate(
            //        j => j.Description,
            //        new JobType
            //        {
            //            Description = "DOB",
            //            Number = "1",
            //            Children = new JobType[] {
            //            new JobType
            //            {
            //                Description = "Registrations",
            //                Number = "1.1",
            //                Content = "As part of registration services, RPO will provided the below",
            //                WorkTypes = new WorkType[]{
            //                    context.WorkTypes.First(wt => wt.Description == "Safety Reg"),
            //                    context.WorkTypes.First(wt => wt.Description == "Renew Safety Reg"),
            //                    context.WorkTypes.First(wt => wt.Description == "Site Super Reg"),
            //                    context.WorkTypes.First(wt => wt.Description == "New tracking no"),
            //                    context.WorkTypes.First(wt => wt.Description == "New contractor reg"),
            //                    context.WorkTypes.First(wt => wt.Description == "New contractor reg"),
            //                    context.WorkTypes.First(wt => wt.Description == "Update ins"),
            //                    context.WorkTypes.First(wt => wt.Description == "Update ins xtra"),
            //                    context.WorkTypes.First(wt => wt.Description == "DOB NOW")
            //                }
            //            },
            //            new JobType
            //            {
            //                Description = "DEMOLITIONS",
            //                Number = "1.2",
            //                Content="FULL DEMOLITION:",
            //                 WorkTypes = new WorkType[]{
            //                    context.WorkTypes.First(wt => wt.Description == "Demolition")
            //                 }
            //            },
            //            new JobType
            //            {
            //                Description = "ALT II GC.",
            //                Number = "1.3",
            //                Content="Filing and approval of an Alteration Type II application at the Department of Buildings. Filing services include preparation of formwork, submission of initial inspection designations based on requirements supplied by AOR or EOR prior to approval and attendance at up to three plan exams. Worktypes included in filing:",
            //                WorkTypes = new WorkType[]{
            //                    context.WorkTypes.First(wt => wt.Description == "OT/GC"),
            //                    context.WorkTypes.First(wt => wt.Description == "OT"),
            //                    context.WorkTypes.First(wt => wt.Description == "MH"),
            //                    context.WorkTypes.First(wt => wt.Description == "PL"),
            //                    context.WorkTypes.First(wt => wt.Description == "ST"),
            //                    context.WorkTypes.First(wt => wt.Description == "SP"),
            //                    context.WorkTypes.First(wt => wt.Description == "SD"),
            //                    context.WorkTypes.First(wt => wt.Description == "BL"),
            //                    context.WorkTypes.First(wt => wt.Description == "FB"),
            //                    context.WorkTypes.First(wt => wt.Description == "FS"),
            //                    context.WorkTypes.First(wt => wt.Description == "OT/FO"),
            //                    context.WorkTypes.First(wt => wt.Description == "OT/EX"),
            //                    context.WorkTypes.First(wt => wt.Description == "OT/SOE"),
            //                    context.WorkTypes.First(wt => wt.Description == "OT/UN"),
            //                    context.WorkTypes.First(wt => wt.Description == "OT/EA"),
            //                }
            //            },
            //            new JobType
            //            {
            //                Description = "ALT II FA",
            //                Number = "1.4",
            //                Content="FIRE ALARM FILING",
            //                WorkTypes = new WorkType[]{
            //                    context.WorkTypes.First(wt => wt.Description == "FA")
            //                    }
            //            },
            //            new JobType
            //            {
            //                Description = "ALT II FS",
            //                Number = "1.5",
            //                Content="FIRE SUPPRESSION SYSTEM",
            //                WorkTypes = new WorkType[]{
            //                    context.WorkTypes.First(wt => wt.Description == "FS")
            //                }
            //            },
            //            new JobType
            //            {
            //                Description = "ALT II PRE ACTION",
            //                Number = "1.6",
            //                Content="PRE ACTION SYSTEM",
            //                WorkTypes = new WorkType[]{
            //                    context.WorkTypes.First(wt => wt.Description == "PRE ACTION"),

            //                }
            //            },
            //            new JobType
            //            {
            //                Description = "ALT III",
            //                Number = "1.7",
            //                Content="Filing and approval of an Alteration Type III application at the Department of Buildings. Filing services include preparation of formwork, submission of initial inspection designations based on requirements supplied by AOR or EOR prior to approval and attendance at up to three plan exams. Worktype included in filing:",
            //                WorkTypes = new WorkType[]{
            //                    context.WorkTypes.First(wt => wt.Description == "OT/GC"),
            //                    context.WorkTypes.First(wt => wt.Description == "OT"),
            //                    context.WorkTypes.First(wt => wt.Description == "OT/FO"),
            //                    context.WorkTypes.First(wt => wt.Description == "OT/EX"),
            //                    context.WorkTypes.First(wt => wt.Description == "OT/SOE"),
            //                    context.WorkTypes.First(wt => wt.Description == "OT/UN"),
            //                    context.WorkTypes.First(wt => wt.Description == "OT/EA"),
            //                    context.WorkTypes.First(wt => wt.Description == "CC"),
            //                    context.WorkTypes.First(wt => wt.Description == "OT/LAN"),
            //                    context.WorkTypes.First(wt => wt.Description == "OT/ANT"),
            //                    context.WorkTypes.First(wt => wt.Description == "OT/MAR"),
            //                    context.WorkTypes.First(wt => wt.Description == "EQ/FENCE"),
            //                    context.WorkTypes.First(wt => wt.Description == "EQ/SCAFFOLD"),
            //                    context.WorkTypes.First(wt => wt.Description == "EQ/SHED"),
            //                    context.WorkTypes.First(wt => wt.Description == "EQ/CHUTE")
            //                }
            //            },
            //            },
            //        }
            //        //new JobType { Description = "DOT", Number = "2" },
            //        //new JobType { Description = "Violation", Number = "3" },
            //        //new JobType { Description = "DEP", Number = "5" }
            //        );
            //    context.SaveChanges();
            //}

            if (!context.ReferenceLinks.Any())
            {
                context.ReferenceLinks.AddOrUpdate(
                    r => r.Name,
                    new ReferenceLink { Name = "BIS", Url = "http://a810-bisweb.nyc.gov/bisweb/bsqpm01.jsp" },
                    new ReferenceLink { Name = "ZOLA", Url = "http://maps.nyc.gov/doitt/nycitymap/template?applicationName=ZOLA" },
                    new ReferenceLink { Name = "OATH", Url = "http://www1.nyc.gov/site/oath/index.page" },
                    new ReferenceLink { Name = "City Maps", Url = "http://maps.nyc.gov/doitt/nycitymap/" },
                    new ReferenceLink { Name = "OASIS Map", Url = "http://www.oasisnyc.net/map.aspx" },
                    new ReferenceLink { Name = "HPD", Url = "http://www1.nyc.gov/site/hpd/index.page" },
                    new ReferenceLink { Name = "Dept.of city planning", Url = "http://www1.nyc.gov/site/planning/index.page" },
                    new ReferenceLink { Name = "ACRIS(DOF)", Url = "http://www1.nyc.gov/site/finance/taxes/acris.page" },
                    new ReferenceLink { Name = "Zoning Maps", Url = "http://www1.nyc.gov/site/planning/zoning/index-map.page" });

            }

            if (!context.CompanyTypes.Any())
            {
                context.CompanyTypes.AddOrUpdate(
                r => r.ItemName,
                new CompanyType { ItemName = "Home Owners" },
                new CompanyType { ItemName = "Owner's REP" },
                new CompanyType { ItemName = "Engineer" },
                new CompanyType { ItemName = "Architect" },
                new CompanyType { ItemName = "Asbestos Investigator" },
                new CompanyType { ItemName = "State Agencies" },
                new CompanyType { ItemName = "Property Managers" },
                new CompanyType { ItemName = "Developers" },
                new CompanyType { ItemName = "Consultants" },
                new CompanyType { ItemName = "Lobbyist" },
                new CompanyType { ItemName = "Special Inspection" },
                new CompanyType { ItemName = "Concrete Testing Lab" },
                new CompanyType { ItemName = "Concrete Producer" },

                new CompanyType
                {
                    ItemName = "City Agency",
                    Children = new HashSet<CompanyType> {
                        new CompanyType { ItemName = "DOB" },
                        new CompanyType { ItemName = "DOT" },
                        new CompanyType { ItemName = "DEP" },
                        new CompanyType { ItemName = "FDNY" },
                        new CompanyType { ItemName = "ECB" },
                        new CompanyType { ItemName = "SCA" },
                        new CompanyType { ItemName = "SBS" } }
                },
                new CompanyType
                {
                    ItemName = "General Contractor",
                    Children = new HashSet<CompanyType> {
                    new CompanyType { ItemName = "1/2/3 Family" },
                    new CompanyType { ItemName = "Safety Reg" },
                    new CompanyType { ItemName = "Demolition" },
                    new CompanyType { ItemName = "Construction" },
                    new CompanyType { ItemName = "Concrete" }}
                });
                context.SaveChanges();
            }

            //if (!context.FDNYPenaltySchedules.Any())
            //{
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 1", DescriptionOfViolation = "Portable Fire Extinguishers and Fire Hoses", OATHViolationCode = "BF01", FirstViolationPenalty = 600, FirstViolationMitigatedPenalty = 300, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 1500, SecondSubsequentViolationMitigatedPenalty = 750, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 2", DescriptionOfViolation = "Combustible Waste Containers", OATHViolationCode = "BF02", FirstViolationPenalty = 500, FirstViolationMitigatedPenalty = 250, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 1500, SecondSubsequentViolationMitigatedPenalty = 750, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 3", DescriptionOfViolation = "Permits", OATHViolationCode = "BF03", FirstViolationPenalty = 700, FirstViolationMitigatedPenalty = 350, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 1750, SecondSubsequentViolationMitigatedPenalty = 875, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 4", DescriptionOfViolation = "Unlawful Quantity or Location of Regulated Material", OATHViolationCode = "BF04", FirstViolationPenalty = 600, FirstViolationMitigatedPenalty = 300, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 1500, SecondSubsequentViolationMitigatedPenalty = 750, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 5", DescriptionOfViolation = "Recordkeeping", OATHViolationCode = "BF05", FirstViolationPenalty = 700, FirstViolationMitigatedPenalty = 350, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 1750, SecondSubsequentViolationMitigatedPenalty = 900, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 6", DescriptionOfViolation = "Signs, Postings, Notices and Instructions", OATHViolationCode = "BF06", FirstViolationPenalty = 600, FirstViolationMitigatedPenalty = 300, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 1500, SecondSubsequentViolationMitigatedPenalty = 750, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 7", DescriptionOfViolation = "Labels and Markings", OATHViolationCode = "BF07", FirstViolationPenalty = 600, FirstViolationMitigatedPenalty = 300, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 1500, SecondSubsequentViolationMitigatedPenalty = 750, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 8", DescriptionOfViolation = "Storage, Accumulation and Removal of Combustible Material and Waste", OATHViolationCode = "BF08", FirstViolationPenalty = 700, FirstViolationMitigatedPenalty = 350, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 1750, SecondSubsequentViolationMitigatedPenalty = 900, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 9", DescriptionOfViolation = "Rooftop Access and Means of Egress", OATHViolationCode = "BF09", FirstViolationPenalty = 950, FirstViolationMitigatedPenalty = 475, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 2375, SecondSubsequentViolationMitigatedPenalty = 1185, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 10", DescriptionOfViolation = "Overcrowding", OATHViolationCode = "BF10", FirstViolationPenalty = 950, FirstViolationMitigatedPenalty = 475, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 2375, SecondSubsequentViolationMitigatedPenalty = 1185, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 11", DescriptionOfViolation = "General Maintenance", OATHViolationCode = "BF11", FirstViolationPenalty = 750, FirstViolationMitigatedPenalty = 375, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 1875, SecondSubsequentViolationMitigatedPenalty = 935, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 12", DescriptionOfViolation = "Fire Protection Systems", OATHViolationCode = "BF12", FirstViolationPenalty = 950, FirstViolationMitigatedPenalty = 475, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 2375, SecondSubsequentViolationMitigatedPenalty = 1200, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC-12", DescriptionOfViolation = "Fire Protection Systems – Failure to Prevent Unnecessary/Unwarranted Alarms", OATHViolationCode = "BF-35", FirstViolationPenalty = 750, FirstViolationMitigatedPenalty = 375, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 1875, SecondSubsequentViolationMitigatedPenalty = 935, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 13", DescriptionOfViolation = "Flame-Resistant Material s", OATHViolationCode = "BF13", FirstViolationPenalty = 900, FirstViolationMitigatedPenalty = 450, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 2250, SecondSubsequentViolationMitigatedPenalty = 1125, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 14", DescriptionOfViolation = "Fire-Rated Doors and Windows", OATHViolationCode = "BF14", FirstViolationPenalty = 900, FirstViolationMitigatedPenalty = 450, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 2250, SecondSubsequentViolationMitigatedPenalty = 1125, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 15", DescriptionOfViolation = "Fire-Rated Construction", OATHViolationCode = "BF15", FirstViolationPenalty = 900, FirstViolationMitigatedPenalty = 450, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 2250, SecondSubsequentViolationMitigatedPenalty = 1125, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 16", DescriptionOfViolation = "Ventilation", OATHViolationCode = "BF16", FirstViolationPenalty = 900, FirstViolationMitigatedPenalty = 450, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 2250, SecondSubsequentViolationMitigatedPenalty = 1125, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 17", DescriptionOfViolation = "Certificates of Fitness and Certificates of Qualification", OATHViolationCode = "BF17", FirstViolationPenalty = 750, FirstViolationMitigatedPenalty = 375, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 1875, SecondSubsequentViolationMitigatedPenalty = 935, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 18", DescriptionOfViolation = "Certificates of Approval, Certificates of License and Company Certificates", OATHViolationCode = "BF18", FirstViolationPenalty = 750, FirstViolationMitigatedPenalty = 375, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 1875, SecondSubsequentViolationMitigatedPenalty = 935, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 19", DescriptionOfViolation = "Affidavits, Design and Installation Documents and Other Documentation", OATHViolationCode = "BF19", FirstViolationPenalty = 600, FirstViolationMitigatedPenalty = 300, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 1500, SecondSubsequentViolationMitigatedPenalty = 750, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 20", DescriptionOfViolation = "Inspection and Testing", OATHViolationCode = "BF20", FirstViolationPenalty = 600, FirstViolationMitigatedPenalty = 300, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 1500, SecondSubsequentViolationMitigatedPenalty = 750, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 21", DescriptionOfViolation = "Portable Containers", OATHViolationCode = "BF21", FirstViolationPenalty = 600, FirstViolationMitigatedPenalty = 300, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 1500, SecondSubsequentViolationMitigatedPenalty = 750, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 22", DescriptionOfViolation = "Stationary Tanks", OATHViolationCode = "BF22", FirstViolationPenalty = 750, FirstViolationMitigatedPenalty = 375, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 1875, SecondSubsequentViolationMitigatedPenalty = 935, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 23", DescriptionOfViolation = "Storage Facilities", OATHViolationCode = "BF23", FirstViolationPenalty = 500, FirstViolationMitigatedPenalty = 250, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 1500, SecondSubsequentViolationMitigatedPenalty = 750, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 24", DescriptionOfViolation = "Storage of Hazardous Materials and Commodities", OATHViolationCode = "BF24", FirstViolationPenalty = 500, FirstViolationMitigatedPenalty = 250, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 1500, SecondSubsequentViolationMitigatedPenalty = 750, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 25", DescriptionOfViolation = "Electrical Hazards", OATHViolationCode = "BF25", FirstViolationPenalty = 900, FirstViolationMitigatedPenalty = 450, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 2250, SecondSubsequentViolationMitigatedPenalty = 1125, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 26", DescriptionOfViolation = "Heating and Refrigerating Equipment and Systems", OATHViolationCode = "BF26", FirstViolationPenalty = 750, FirstViolationMitigatedPenalty = 375, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 1875, SecondSubsequentViolationMitigatedPenalty = 935, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 27", DescriptionOfViolation = "Electrical Lighting Hazards", OATHViolationCode = "BF27", FirstViolationPenalty = 750, FirstViolationMitigatedPenalty = 375, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 1875, SecondSubsequentViolationMitigatedPenalty = 935, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 28", DescriptionOfViolation = "Open Fires, Open Flames and Sparks", OATHViolationCode = "BF28", FirstViolationPenalty = 900, FirstViolationMitigatedPenalty = 450, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 2250, SecondSubsequentViolationMitigatedPenalty = 1125, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 29", DescriptionOfViolation = "Designated Handling and Use Rooms or Areas", OATHViolationCode = "BF29", FirstViolationPenalty = 600, FirstViolationMitigatedPenalty = 300, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 1500, SecondSubsequentViolationMitigatedPenalty = 750, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "VC 30", DescriptionOfViolation = "Emergency Planning and Preparedness", OATHViolationCode = "BF30", FirstViolationPenalty = 950, FirstViolationMitigatedPenalty = 475, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 2250, SecondSubsequentViolationMitigatedPenalty = 1200, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "Admin. Code § 15-220.1", DescriptionOfViolation = "False Certification", OATHViolationCode = "BF32", FirstViolationPenalty = 2500, FirstViolationMitigatedPenalty = 0, FirstViolationMaximumPenalty = 5000, SecondSubsequentViolationPenalty = 4500, SecondSubsequentViolationMitigatedPenalty = 0, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "Admin. Code § 15-231", DescriptionOfViolation = "Failure to Comply with Commissioner’s Order to Correct and Certify", OATHViolationCode = "BF31", FirstViolationPenalty = 1250, FirstViolationMitigatedPenalty = 0, FirstViolationMaximumPenalty = 5000, SecondSubsequentViolationPenalty = 3500, SecondSubsequentViolationMitigatedPenalty = 0, SecondSubsequentViolationMaximumPenalty = 5000 });
            //    context.FDNYPenaltySchedules.AddOrUpdate(r => r.OATHViolationCode, new FDNYPenaltySchedule { Category_RCNY = "FC 1404.1", DescriptionOfViolation = "Smoking on Construction Site", OATHViolationCode = "BF33", FirstViolationPenalty = 1000, FirstViolationMitigatedPenalty = 0, FirstViolationMaximumPenalty = 1000, SecondSubsequentViolationPenalty = 2400, SecondSubsequentViolationMitigatedPenalty = 0, SecondSubsequentViolationMaximumPenalty = 2400 });
            //    context.SaveChanges();
            //}

            //if (!context.DOTPenaltySchedules.Any())
            //{
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 10­157(b), (c), (e) ", Description = "Failure to provide appropriate equipment to bicycle operator delivering on behalf of a business using a bicycle for commercial purposes (FIRST OFFENSE) ", Penalty = 100, DefaultPenalty = 100 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 10­157(b), (c), (e) ", Description = "Failure to provide appropriate equipment to bicycle operator delivering on behalf of a business using a bicycle for commercial purposes (SECOND OR SUBSEQUENT OFFENSE) ", Penalty = 250, DefaultPenalty = 250 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 10-157(d) ", Description = "Failure to produce or maintain a roster by a business using a bicycle for commercial purposes (FIRST OFFENSE) ", Penalty = 100, DefaultPenalty = 100 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 10-157(d) ", Description = "Failure to produce or maintain a roster by a business using a bicycle for commercial purposes (SECOND OR SUBSEQUENT OFFENSE) ", Penalty = 250, DefaultPenalty = 250 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 10-157(f) ", Description = "Failure to properly equip bicycle used on behalf of a business using a bicycle for commercial purposes (FIRST OFFENSE) ", Penalty = 100, DefaultPenalty = 100 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 10-157(f) ", Description = "Failure to properly equip bicycle used on behalf of a business using a bicycle for commercial purposes (SECOND OR SUBSEQUENT OFFENSE)", Penalty = 250, DefaultPenalty = 250 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 10-157.1 ", Description = "Failure to post Commercial Bicyclist Safety Poster containing required information (FIRST OFFENSE) ", Penalty = 100, DefaultPenalty = 100 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 10-157.1 ", Description = "Failure to post Commercial Bicyclist Safety Poster containing required information (SECOND OR SUBSEQUENT OFFENSE) ", Penalty = 250, DefaultPenalty = 250 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-102(i) ", Description = "Use/opening of street without DOT permit ", Penalty = 1500, DefaultPenalty = 5000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-102(i) ", Description = "Use/opening of protected street without DOT permit ", Penalty = 1800, DefaultPenalty = 5000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-102(i) ", Description = "Working without DOT permit on controlled access highway ", Penalty = 4000, DefaultPenalty = 5000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-102(ii) ", Description = "Failure to comply with terms and conditions of DOT permit ", Penalty = 1200, DefaultPenalty = 3600 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-102(ii) ", Description = "Failure to comply with terms and conditions of DOT permit on controlled access highway ", Penalty = 4000, DefaultPenalty = 5000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-107 ", Description = "Street closing without DOT permit ", Penalty = 1800, DefaultPenalty = 5000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-108 ", Description = "Failure to have DOT permit on site or in field office ", Penalty = 50, DefaultPenalty = 150 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-109(a) ", Description = "Failure to provide adequate protection at worksite ", Penalty = 1200, DefaultPenalty = 3600 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-109(b) ", Description = "Identifying signs improperly displayed or missing ", Penalty = 100, DefaultPenalty = 300 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-117(a) ", Description = "Constructing vault without license or revocable consent ", Penalty = 500, DefaultPenalty = 1500 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-119 ", Description = "Vault opening without proper protection ", Penalty = 250, DefaultPenalty = 750 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-121(a) ", Description = "Construction materials/equipment stored on street without DOT permit ", Penalty = 750, DefaultPenalty = 2250 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-121(b) (2) ", Description = "Debris/construction materials obstructing gutters/sidewalk, etc. ", Penalty = 250, DefaultPenalty = 750 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-121(b) (3) ", Description = "Construction material/equipment without proper reflective markings ", Penalty = 250, DefaultPenalty = 750 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-121(b) (4) ", Description = "Material/equipment without name & address of owner ", Penalty = 100, DefaultPenalty = 300 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-121(b) ", Description = "Construction material/equipment within 5 feet of surface of railroad tracks  ", Penalty = 500, DefaultPenalty = 1500 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-121(b) (6) ", Description = "No street protection under construction material/equipment ", Penalty = 250, DefaultPenalty = 750 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-121(b) (7) ", Description = "Obstruction of fire hydrant or bus stop ", Penalty = 500, DefaultPenalty = 1500 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-122 ", Description = "Sand/dirt/rubbish/debris not removed from site within 7 days ", Penalty = 250, DefaultPenalty = 750 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-123 ", Description = "Commercial refuse container stored on the street without DOT permit ", Penalty = 750, DefaultPenalty = 2250 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-123 ", Description = "No street protection under commercial refuse container ", Penalty = 250, DefaultPenalty = 750 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-124(a) ", Description = "Canopy without DOT permit ", Penalty = 100, DefaultPenalty = 300 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-125 ", Description = "Post/pole/flagpole socket/lamppost without DOT permit/consent ", Penalty = 100, DefaultPenalty = 300 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-126 ", Description = "Use/movement/removal of crane/building/structure without DOT permit ", Penalty = 1000, DefaultPenalty = 3000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-133 ", Description = "Unauthorized projections and encroachments on City property ", Penalty = 250, DefaultPenalty = 750 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-133.1 ", Description = "Maintaining an unlawful sidewalk ATM booth (Automated Teller Machine Booth) (FIRST DAY OF VIOLATION) ", Penalty = 2500, DefaultPenalty = 5000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-133.1 ", Description = "Failure to remove an unlawful sidewalk ATM booth (CONTINUING VIOLATION-for every 5-day period the violation remains) ", Penalty = 5000, DefaultPenalty = 5000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-136 ", Description = "Goods/wares/merchandise obstructing sidewalk ", Penalty = 150, DefaultPenalty = 500 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-136(b) ", Description = "Vehicle(s) on sidewalk ", Penalty = 50, DefaultPenalty = 300 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-138(b) ", Description = "Defacement of roadway or sidewalk ", Penalty = 250, DefaultPenalty = 750 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-147(d) ", Description = "Failure to replace loose, slippery or broken utility maintenance hole (manhole) covers, castings ", Penalty = 250, DefaultPenalty = 750 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-151 ", Description = "Failure to comply with commissioner's order ", Penalty = 250, DefaultPenalty = 400 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-176(b) ", Description = "Unlawful bicycle riding ", Penalty = 50, DefaultPenalty = 100 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-176(c) ", Description = "Riding bicycle on sidewalk in manner which endangers any person or property (FIRST OFFENSE) ", Penalty = 100, DefaultPenalty = 300 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "", Description = "Riding bicycle on sidewalk in manner which endangers any person or property (SECOND OR SUBSEQUENT OFFENSE) ", Penalty = 200, DefaultPenalty = 600 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-176(c) ", Description = "Riding bicycle on sidewalk in manner which endangers any person or property and causes physical contact with a person (FIRST OFFENSE) ", Penalty = 200, DefaultPenalty = 500 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-176(c) ", Description = "Riding bicycle on sidewalk in manner which endangers any person or property and causes physical contact with a person (SECOND OR SUBSEQUENT OFFENSE) ", Penalty = 400, DefaultPenalty = 1000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19­176.2(b) ", Description = "Operation of motorized scooter within the city of New York ", Penalty = 500, DefaultPenalty = 500 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-190(a) ", Description = "Right of way -failure to yield ", Penalty = 100, DefaultPenalty = 100 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-190(b) ", Description = "Right of way -failure to yield, physical injury ", Penalty = 250, DefaultPenalty = 250 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-191(a) ", Description = "Leaving the scene -property damage (FIRST OFFENSE) ", Penalty = 500, DefaultPenalty = 1000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-191(a) ", Description = "Leaving the scene -property damage (SECOND OR SUBSEQUENT OFFENSE) ", Penalty = 1000, DefaultPenalty = 2000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-191(b) ", Description = "Leaving the scene -physical injury (FIRST OFFENSE) ", Penalty = 2000, DefaultPenalty = 2000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-191(b) ", Description = "Leaving the scene -physical injury (SECOND OR SUBSEQUENT OFFENSE) ", Penalty = 5000, DefaultPenalty = 5000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-191(b) ", Description = "Leaving the scene -serious physical injury (FIRST OFFENSE) ", Penalty = 10000, DefaultPenalty = 10000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-191(b) ", Description = "Leaving the scene -serious physical injury (SECOND OR SUBSEQUENT OFFENSE) ", Penalty = 15000, DefaultPenalty = 15000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-191(b) ", Description = "Leaving the scene -death (FIRST OFFENSE) ", Penalty = 15000, DefaultPenalty = 15000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-191(b) ", Description = "Leaving the scene -death (SECOND OR SUBSEQUENT OFFENSE) ", Penalty = 20000, DefaultPenalty = 20000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "Admin. Code § 19-196 ", Description = "Operating an all-terrain vehicle in the city of New York (FIRST OFFENSE) ", Penalty = 500, DefaultPenalty = 500 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "", Description = "Operating an all-terrain vehicle in the city of New York (SECOND OR SUBSEQUENT OFFENSE REGARDLESS OF INTERVAL ", Penalty = 1000, DefaultPenalty = 1000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-02(a)(1)(ii) ", Description = "BETWEEN OFFENSES) Failure to provide name and telephone number of emergency contact ", Penalty = 250, DefaultPenalty = 500 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-02(c)(2) ", Description = "Failure to display required signs at work site ", Penalty = 250, DefaultPenalty = 350 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-02(m) ", Description = "Illegally working on a street during an embargo ", Penalty = 1200, DefaultPenalty = 3600 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-04(f)(1) ", Description = "Canopy erected/maintained on a restricted street ", Penalty = 150, DefaultPenalty = 450 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-05(d)(8)(vi) ", Description = "Divisible construction materials or equipment stored on the roadway at a height greater than 5 feet ", Penalty = 500, DefaultPenalty = 1500 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-05(d)(8)(vi) ", Description = "Divisible construction materials or equipment stored on the sidewalk at a height greater than 5 feet ", Penalty = 500, DefaultPenalty = 1000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-05(d)(10) ", Description = "Failure to provide space for loading & unloading of materials on the roadway ", Penalty = 250, DefaultPenalty = 750 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-05(d)(12) ", Description = "Mixing mortar on roadway without protection ", Penalty = 250, DefaultPenalty = 750 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-05(d)(16) ", Description = "Failure to house overhead cables/hoses/wires with 14 feet minimum clearance on the sidewalk ", Penalty = 250, DefaultPenalty = 500 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-05(h)(1) ", Description = "Construction shanty/trailer without DOT permit ", Penalty = 150, DefaultPenalty = 450 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-05(h)(4) ", Description = "Failure to remove shanties/trailers from roadway/sidewalk ", Penalty = 250, DefaultPenalty = 750 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-05(i)(1) ", Description = "Crossing sidewalk with a motorized vehicle without DOT permit ", Penalty = 250, DefaultPenalty = 500 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-07(a)(2) ", Description = "Opening a utility access cover without an authorization number during an embargo. ", Penalty = 1500, DefaultPenalty = 5000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-07(a)(4) ", Description = "Restricting more than 11 feet of roadway by opening covers/gratings ", Penalty = 500, DefaultPenalty = 1600 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-07(a)(5) ", Description = "Opening more than two consecutive covers/gratings ", Penalty = 500, DefaultPenalty = 1600 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-07(a)(6) ", Description = "Toolcart stored on sidewalk failed to provide a 5 foot minimum walkway ", Penalty = 250, DefaultPenalty = 500 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-07(a)(6) ", Description = "Toolcart stored on roadway without a DOT permit ", Penalty = 250, DefaultPenalty = 750 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-07(a)(6) ", Description = "Toolcart stored on sidewalk obstructing hydrant, bus stop, or driveway ", Penalty = 500, DefaultPenalty = 500 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-07(b)(2) ", Description = "Failure to repair defective street condition found within an area extending 12 inches outward from the perimeter of the cover/grating ", Penalty = 250, DefaultPenalty = 750 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-07(b)(2) ", Description = "Failure to obtain DOT permit for street plate covering defective condition ", Penalty = 1500, DefaultPenalty = 5000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-07(b)(3) ", Description = "Utility cover/street hardware not flush with surrounding area ", Penalty = 1000, DefaultPenalty = 3000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-07(c)(1) ", Description = "Doing non-emergency work on a critical roadway during restricted hours ", Penalty = 2000, DefaultPenalty = 3000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-07(c)(4)(i) ", Description = "Opening a utility access cover without an authorization number ", Penalty = 2000, DefaultPenalty = 3000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-07(c)(4)(iv) ", Description = "Failure to perform emergency work around the clock(covers/gratings) ", Penalty = 1000, DefaultPenalty = 3000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-07(c)(4)(v) ", Description = "Failure to notify DOT of completion of emergency work (covers/gratings) ", Penalty = 250, DefaultPenalty = 750 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-09 (f)(4)(v) ", Description = "Except as in NYC Administrative Code § 19-152, failure to install or seal expansion joints as per subsection ", Penalty = 250, DefaultPenalty = 750 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-09(f)(4)(viii) ", Description = "Except as in NYC Administrative Code § 19-152, failure to fully replace defective sidewalk flag ", Penalty = 250, DefaultPenalty = 500 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-09(f)(4) (xiv) ", Description = "Except as in NYC Administrative Code § 19-152, failure to install pedestrian ramp as per DOT drawings ", Penalty = 400, DefaultPenalty = 1000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-09(f)(4) (xvi) ", Description = "Except as in NYC Administrative Code § 19-152, failure to obtain DOT approval for distinctive sidewalk ", Penalty = 250, DefaultPenalty = 750 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-09(f)(4) (xvi)(C) ", Description = "Failure to replace distinctive sidewalk in kind ", Penalty = 250, DefaultPenalty = 500 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-09(g)(1)(i) ", Description = "Failure to install curb before commencing any roadway paving operation or sidewalk construction ", Penalty = 250, DefaultPenalty = 500 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "", Description = "Failure to mark out proposed excavation area; failure to limit geographical area to be marked out and/or use of excessive or oversized markings. ", Penalty = 1500, DefaultPenalty = 5000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-11(e)(1) ", Description = "Failure to notify City 24 hours before street work ", Penalty = 250, DefaultPenalty = 500 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-11(e)(2) ", Description = "Failure to use only hand held tools, rockwheels or other DOT approved tools to precut pavement ", Penalty = 250, DefaultPenalty = 400 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-11(e)(3)(i) ", Description = "Excavation down 5 feet or greater without shoring/sheeting/bracing ", Penalty = 1500, DefaultPenalty = 4500 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-11(e)(3)(ii) ", Description = "Tunneling/jacking without a DOT permit ", Penalty = 400, DefaultPenalty = 1200 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-11(e)(4)(ii) ", Description = "Failure to plate excavation in driving lane or intersection ", Penalty = 1200, DefaultPenalty = 3600 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-11(e)(4)(v) ", Description = "Failure to post flagperson at worksite to give directions ", Penalty = 800, DefaultPenalty = 2400 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-11(e)(5) ", Description = "Failure to maintain a 5-foot pedestrian walkway on sidewalk ", Penalty = 250, DefaultPenalty = 750 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-11(e)(8)(i) ", Description = "Unsuitable backfill material used ", Penalty = 250, DefaultPenalty = 750 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-11(e)(8)(vi) ", Description = "Restoration sunken more than 2 inches ", Penalty = 500, DefaultPenalty = 1000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-11(e)(9)(i) ", Description = "Temporary pavement not flush with surrounding area ", Penalty = 500, DefaultPenalty = 1000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-11(e)(10) (ii) ", Description = "Failure to properly place and ramp plating and decking ", Penalty = 1200, DefaultPenalty = 3600 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-11(e)(10) (iii) ", Description = "Failure to properly fasten plating and decking ", Penalty = 1200, DefaultPenalty = 3600 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-11(e)(10) (v) ", Description = "Failure to post 'Steel Plates Ahead' or 'Raise Plow' sign; failure to countersink plates flush with roadway ", Penalty = 250, DefaultPenalty = 750 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-11(e)(10) (vi) ", Description = "Failure to use skid resistant plating and/or decking on roadway ", Penalty = 1000, DefaultPenalty = 5000 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-11(e) (10(vii) ", Description = "Failure to remove plating and decking after final restoration or prior to DOT permit expiration. ", Penalty = 250, DefaultPenalty = 500 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-11(e)(10) (viii) ", Description = "Failure to add required identifying information to plating and decking ", Penalty = 250, DefaultPenalty = 500 });
            //    context.DOTPenaltySchedules.Add(new DOTPenaltySchedule { Section = "34 RCNY § 2-11(e)(11) ", Description = "Failure to restore concrete-base ", Penalty = 0, DefaultPenalty = 0 });
            //    context.SaveChanges();
            //}

            //if (!context.DOHMHCoolingTowerPenaltySchedules.Any())
            //{
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-03", Description = "No maintenance program and plan", PenaltyFirstViolation = 1000, PenaltyRepeatViolation = 2000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY§ 8-03", Description = "Maintenance program and plan incomplete or not on premises", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-04(a)", Description = "Routine monitoring not conducted, documented at least once a week when tower is in use", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY§ 8-04(b)", Description = "Compliance inspections not conducted, documented at least once every 90 days when the tower is in use", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-04(c)", Description = "Routine maintenance according to maintenance program and plan not conducted or documented", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY§ 8-04(d)", Description = "Twice yearly or other required cleaning not conducted or documented", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-04(e)", Description = "Aerosol control do not meet manufacturer's design specifications or drift loss reduction requirements in new or existing towers when required", PenaltyFirstViolation = 1000, PenaltyRepeatViolation = 2000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY§ 8-05(a)", Description = "Daily automatic or approved alternative water treatment plan not provided", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY§ 8-05(b)", Description = "Cooling water system not continually recirculated and no acceptable alternative", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-05(c)(1)", Description = "Use of an unqualified biocide applicator", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-05(c)(2)", Description = "Use of an unregistered biocide product", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-05(c)(3)", Description = "No records of all chemicals and biocides added", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-05(c)(4)", Description = "Sufficient quantities and combinations of chemicals not added as specified in the maintenance program and plan", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-05(d)", Description = "Using unacceptable alternative nonchemical water treatment device", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-05(e)", Description = "Use of captured rainwater or recycled water as makeup water not in accordance with approved alternative water source plan", PenaltyFirstViolation = 1000, PenaltyRepeatViolation = 2000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-05(f)(1)", Description = "Minimum daily water quality measurements not taken or recorded", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-05(f)(2)", Description = "Failure to collect, analyze or record weekly biological process control indicators", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-05(f)(3)", Description = "Legionella samples not collected or analyzed, or results not recorded or reported to the Department as required", PenaltyFirstViolation = 1000, PenaltyRepeatViolation = 2000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-05(f)(4)", Description = "Failure to monitor and sample from representative locations and times", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-05(f)(5)", Description = "Required corrective actions not taken based on bacteriological results", PenaltyFirstViolation = 1000, PenaltyRepeatViolation = 2000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-06(a)", Description = "Improper or inadequate shutdown procedures", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-06(b)(1)", Description = "Improper or inadequate start-up procedures", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-06(b)(2)", Description = "Legionella samples not collected, analyzed before system start-up", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-06(c)", Description = "New cooling tower not or inadequately cleaned and disinfected prior to operating", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-07(a)", Description = "Failure to document all inspections, logs, tests, cleaning, and disinfection in accordance with the maintenance program and plan", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-07(a)", Description = "Failure to retain records for at least 3 years", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-07(a)", Description = "Required records not kept at the cooling tower premises", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-07(c)", Description = "Department of Buildings Cooling Tower Registration Number not posted as required", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "24 RCNY § 8-07(d)", Description = "Records not made immediately available to Department upon request", PenaltyFirstViolation = 500, PenaltyRepeatViolation = 1000 });
            //    context.DOHMHCoolingTowerPenaltySchedules.Add(new DOHMHCoolingTowerPenaltySchedule { SectionOfLaw = "State Sanitary Code Part 4", Description = "Miscellaneous provisions", PenaltyFirstViolation = 250, PenaltyRepeatViolation = 250 });
            //    context.SaveChanges();
            //}

            //if (!context.DOBPenaltySchedules.Any())
            //{
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "1 RCNY-Misc, RS-Misc", Classification = "Class 1", InfractionCode = "B179", ViolationDescription = "Miscellaneous violations.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "1 RCNY-Misc, RS-Misc", Classification = "Class 2", InfractionCode = "B279", ViolationDescription = "Miscellaneous violations.", Cure = true, Stipulation = true, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "1 RCNY-Misc, RS-Misc", Classification = "Class 3", InfractionCode = "B379", ViolationDescription = "Miscellaneous violations.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "1 RCNY 5-02", Classification = "Class 2", InfractionCode = "B2B3", ViolationDescription = "Failure to meet the requirements of licensing/ identification/ qualification as required by 1 RCNY 5-02.", Cure = true, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "1 RCNY 104-20", Classification = "Class 1", InfractionCode = "B144", ViolationDescription = "Licensed Rigger designated an unqualified foreman.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "1 RCNY 104-20", Classification = "Class 2", InfractionCode = "B244", ViolationDescription = "Licensed Rigger designated an unqualified foreman.", Cure = false, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "1 RCNY 49-03", Classification = "Class 1", InfractionCode = "B1B7", ViolationDescription = "Outdoor Advertising Company failed to comply with Commissioner’s sign-related Order.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "1 RCNY 101-07", Classification = "Class 2", InfractionCode = "B2B4", ViolationDescription = "Failure of approved agency to comply with requirements of 1 RCNY 101-07.", Cure = true, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "1 RCNY 103-04(b)(5)(iii)", Classification = "Class 2", InfractionCode = "B2F2", ViolationDescription = "Removal of public protection from unsafe façade without approval from the department.", Cure = false, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "1 RCNY 3319-01(u)", Classification = "Class 1", InfractionCode = "B1H9", ViolationDescription = "Failed to provide / maintain the required documents.", Cure = false, Stipulation = true, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-228.5", Classification = "Class 2", InfractionCode = "B242", ViolationDescription = "Failure to file an Architect/Engineer report certifying that exit/directional signs are connected to emergency power source/storage battery equipment.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-369, BC 1020.2 (2008 code), & BC 1023.2 (2014 code)", Classification = "Class 1", InfractionCode = "B127", ViolationDescription = "Failure to provide unobstructed exit passageway.", Cure = false, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = false, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 12500, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-371, BC 715.3.7 (2008 code) & BC 715.4.8 (2014 code)", Classification = "Class 2", InfractionCode = "B252", ViolationDescription = "Exit door not self-closing.", Cure = true, Stipulation = false, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-382 & BC 1006.3", Classification = "Class 2", InfractionCode = "B237", ViolationDescription = "Failure to provide power for emergency exit lighting.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-383(b), BC 403.16 (2008 code) & BC 403.5.5 (2014 code)", Classification = "Class 1", InfractionCode = "B134", ViolationDescription = "Failure to install luminous egress or photoluminescent exit path marking in a high-rise building.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-393, BC 1019.1.7 (2008 code) & BC 1022.8 (2014 code)", Classification = "Class 2", InfractionCode = "B235", ViolationDescription = "Stair and/or floor identification signs missing and/or defective.", Cure = true, Stipulation = true, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-509, BC 3111.1 (2008 code) & BC 3112.1 (2014 code)", Classification = "Class 3", InfractionCode = "B307", ViolationDescription = "Fence exceeds permitted height.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-528, BC 1024.1.3 (2008 code) & BC 1028.1.3 (2014 code)", Classification = "Class 2", InfractionCode = "B219", ViolationDescription = "Approved Place of Assembly plans not available for inspection.", Cure = true, Stipulation = false, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 2500, AggravatedPenalty_I = 1250, AggravatedDefaultPenalty_I = 5000, AggravatedPenalty_II = 2500, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-901(z)(1) & PC 301.6", Classification = "Class 2", InfractionCode = "B270", ViolationDescription = "Piping installed in elevator/counterweight hoistway.", Cure = true, Stipulation = false, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-904 & FGC 406.6.2", Classification = "Class 1", InfractionCode = "B156", ViolationDescription = "Gas being supplied to building without inspection and certification by DOB.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-904 & FGC 406.6.2", Classification = "Class 2", InfractionCode = "B256", ViolationDescription = "Gas being supplied to building without inspection and certification by DOB.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-921(a), PC 107.3 (2008 code) & PC 107.4 (2014 code)", Classification = "Class 1", InfractionCode = "B158", ViolationDescription = "Failure to have new or altered plumbing system tested.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-921(a), PC 107.3 (2008 code) & PC 107.4 (2014 code)", Classification = "Class 2", InfractionCode = "B258", ViolationDescription = "Failure to have new or altered plumbing system tested.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-972(h), BC 907.2.12.3 (2008 code) & BC 907.2.13.3 (2014 code)", Classification = "Class 2", InfractionCode = "B240", ViolationDescription = "Failure to install an acceptable two-way voice communication system with central station connection.", Cure = true, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 10000, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-3017", Classification = "Class 1", InfractionCode = "B1C7, A1C7", ViolationDescription = "Performed unlicensed electrical work.", Cure = false, Stipulation = false, StandardPenalty = 4800, MitigatedPenalty = false, DefaultPenalty = 24000, AggravatedPenalty_I = 12000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 24000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-3018(b)", Classification = "Class 1", InfractionCode = "B1C9", ViolationDescription = "Electrical work without a permit.", Cure = false, Stipulation = false, StandardPenalty = 1600, MitigatedPenalty = true, DefaultPenalty = 8000, AggravatedPenalty_I = 4000, AggravatedDefaultPenalty_I = 16000, AggravatedPenalty_II = 8000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-3018(b)", Classification = "Class 2", InfractionCode = "B2C4", ViolationDescription = "Electrical work without a permit.", Cure = true, Stipulation = true, StandardPenalty = 800, MitigatedPenalty = true, DefaultPenalty = 4000, AggravatedPenalty_I = 2000, AggravatedDefaultPenalty_I = 8000, AggravatedPenalty_II = 4000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-3018(b)", Classification = "Class 3", InfractionCode = "B309", ViolationDescription = "Electrical work without a permit.", Cure = true, Stipulation = true, StandardPenalty = 400, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-3018(b)", Classification = "Class 3", InfractionCode = "B310", ViolationDescription = "Failure to conspicuously post electrical work permit while work is in progress.", Cure = true, Stipulation = true, StandardPenalty = 400, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-3018(b)", Classification = "Class 1", InfractionCode = "B1C8", ViolationDescription = "Electrical work does not conform to approved submittal documents/amendments.", Cure = false, Stipulation = false, StandardPenalty = 1000, MitigatedPenalty = false, DefaultPenalty = 5000, AggravatedPenalty_I = 2500, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 5000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-3018(b)", Classification = "Class 2", InfractionCode = "B2C3", ViolationDescription = "Electrical work does not conform to approved submittal documents/amendments.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 2500, AggravatedPenalty_I = 1250, AggravatedDefaultPenalty_I = 5000, AggravatedPenalty_II = 2500, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-3018(b)", Classification = "Class 3", InfractionCode = "B308", ViolationDescription = "Electrical work does not conform to approved submittal documents/amendments.", Cure = true, Stipulation = true, StandardPenalty = 300, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-3018(i)", Classification = "Class 2", InfractionCode = "B2C5", ViolationDescription = "Installed more than the authorized number of electric meters.", Cure = false, Stipulation = false, StandardPenalty = 2400, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6000, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-Misc, 28-Misc, BC-Misc", Classification = "Class 1", InfractionCode = "B106", ViolationDescription = "Miscellaneous violations.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-Misc, 28-Misc, BC-Misc", Classification = "Class 2", InfractionCode = "B206", ViolationDescription = "Miscellaneous violations.", Cure = true, Stipulation = true, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 12500, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "27-Misc, 28-Misc, BC-Misc", Classification = "Class 3", InfractionCode = "B306", ViolationDescription = "Miscellaneous violations.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-104.2.2", Classification = "Class 2", InfractionCode = "B210", ViolationDescription = "Failure to provide approved/accepted construction documents at job site at time of inspection.", Cure = true, Stipulation = false, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 2500, AggravatedPenalty_I = 1250, AggravatedDefaultPenalty_I = 5000, AggravatedPenalty_II = 2500, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.1", Classification = "Class 2", InfractionCode = "B250", ViolationDescription = "Failure to obtain a temporary construction permit prior to installation/use of sidewalk shed.", Cure = true, Stipulation = false, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 2500, AggravatedPenalty_I = 1250, AggravatedDefaultPenalty_I = 5000, AggravatedPenalty_II = 2500, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.1", Classification = "Class 1", InfractionCode = "B1C6", ViolationDescription = "Work After Hours Without a Variance Permit contrary to 28-105.12.5.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = true, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.1", Classification = "Class 2", InfractionCode = "B2C2", ViolationDescription = "Work After Hours Without a Variance Permit contrary to 28-105.12.5.", Cure = false, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.1", Classification = "Class 1", InfractionCode = "B101", ViolationDescription = "Work without a permit.", Cure = false, Stipulation = false, StandardPenalty = 1600, MitigatedPenalty = true, DefaultPenalty = 8000, AggravatedPenalty_I = 4000, AggravatedDefaultPenalty_I = 16000, AggravatedPenalty_II = 0, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.1", Classification = "Class 2", InfractionCode = "B201", ViolationDescription = "Work without a permit.", Cure = true, Stipulation = true, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 12500, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.1", Classification = "Class 3", InfractionCode = "B301", ViolationDescription = "Work without a permit.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.1", Classification = "Class 1", InfractionCode = "B113", ViolationDescription = "Construction or alteration work w/o a permit in manufacturing district for residential use.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.1", Classification = "Class 2", InfractionCode = "B213", ViolationDescription = "Construction or alteration work w/o a permit in manufacturing district for residential use.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.1", Classification = "Class 1", InfractionCode = "B121", ViolationDescription = "Demolition work without required demolition permit.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.1", Classification = "Class 1", InfractionCode = "B157", ViolationDescription = "Plumbing work without a permit in manufacturing district for residential use.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.1", Classification = "Class 2", InfractionCode = "B257", ViolationDescription = "Plumbing work without a permit in manufacturing district for residential use.", Cure = false, Stipulation = true, StandardPenalty = 2500, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.1", Classification = "Class 2", InfractionCode = "B274", ViolationDescription = "Outdoor sign on display structure without a permit.", Cure = false, Stipulation = true, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.1", Classification = "Class 1", InfractionCode = "B160", ViolationDescription = "Outdoor Ad Co sign on display structure without a permit.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.11", Classification = "Class 2", InfractionCode = "B220", ViolationDescription = "Failure to post or properly post permit for work at premises.", Cure = true, Stipulation = true, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.12.1", Classification = "Class 2", InfractionCode = "B2A1", ViolationDescription = "Outdoor sign permit application contrary to Code and ZR requirements.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 10000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.12.2", Classification = "Class 1", InfractionCode = "B182", ViolationDescription = "Work does not conform to approved construction documents and/or approved amendments.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.12.2", Classification = "Class 2", InfractionCode = "B282", ViolationDescription = "Work does not conform to approved construction documents and/or approved amendments.", Cure = true, Stipulation = true, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.12.2", Classification = "Class 3", InfractionCode = "B382", ViolationDescription = "Work does not conform to approved construction documents and/or approved amendments.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.12.2", Classification = "Class 1", InfractionCode = "B114", ViolationDescription = "Work does not conform to approved construction documents and/or approved amendments in a manufacturing district for residential use.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.12.2", Classification = "Class 2", InfractionCode = "B214", ViolationDescription = "Work does not conform to approved construction documents and/or approved amendments in a manufacturing district for residential use.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.12.2", Classification = "Class 1", InfractionCode = "B125", ViolationDescription = "Place of Assembly contrary to approved construction documents.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.12.2", Classification = "Class 2", InfractionCode = "B225", ViolationDescription = "Place of Assembly contrary to approved construction documents.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 2500, AggravatedPenalty_I = 1250, AggravatedDefaultPenalty_I = 5000, AggravatedPenalty_II = 2500, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-105.12.2", Classification = "Class 1", InfractionCode = "B161", ViolationDescription = "Outdoor Ad Co sign work does not conform to approved construction documents or amendments.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-110.1(20)", Classification = "Class 1", InfractionCode = "B185", ViolationDescription = "Failure to provide evidence of workers attending construction & safety course.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-116.1", Classification = "Class 2", InfractionCode = "B2A7", ViolationDescription = "Failure of permit holder to provide inspection access to and/or expose ongoing construction or work on an active and permitted worksite.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-116.2.4.2", Classification = "Class 2", InfractionCode = "B2G1", ViolationDescription = "Failure to conduct or file a final inspection of permitted work with the Department.", Cure = true, Stipulation = true, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-116.4.1", Classification = "Class 2", InfractionCode = "B262", ViolationDescription = "Operation of service equipment without Certificate of Compliance.", Cure = true, Stipulation = true, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-117.1", Classification = "Class 1", InfractionCode = "B122", ViolationDescription = "Operation of a Place of Assembly without a current Certificate of Operation.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-117.1", Classification = "Class 2", InfractionCode = "B222", ViolationDescription = "Operation of a Place of Assembly without a current Certificate of Operation.", Cure = true, Stipulation = false, StandardPenalty = 800, MitigatedPenalty = true, DefaultPenalty = 4000, AggravatedPenalty_I = 2000, AggravatedDefaultPenalty_I = 8000, AggravatedPenalty_II = 4000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-118.1", Classification = "Class 1", InfractionCode = "B107", ViolationDescription = "Building or open lot occupied without a valid certificate of occupancy.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-118.3", Classification = "Class 1", InfractionCode = "B108", ViolationDescription = "Altered/changed building occupied without a valid Certificate of Occupancy as per §28-118.3.1 - §28-118.3.2.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-118.3", Classification = "Class 2", InfractionCode = "B208", ViolationDescription = "Altered/changed building occupied without a valid Certificate of Occupancy as per §28-118.3.1 - §28-118.3.2.", Cure = true, Stipulation = true, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-118.3", Classification = "Class 1", InfractionCode = "B124", ViolationDescription = "Change in occupancy/use of C of O as per §28-118.3.1 - §28-118.3.2 by operating a Place of Assembly as per when current C of O does not allow such occupancy.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-118.3", Classification = "Class 2", InfractionCode = "B224", ViolationDescription = "Change in occupancy/use of C of O as per §28-118.3.1 - §28-118.3.2 by operating a Place of Assembly as per when current C of O does not allow such occupancy.", Cure = true, Stipulation = false, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 2500, AggravatedPenalty_I = 1250, AggravatedDefaultPenalty_I = 5000, AggravatedPenalty_II = 2500, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-118.3.2", Classification = "Class 1", InfractionCode = "B103", ViolationDescription = "Occupancy contrary to that allowed by the Certificate of Occupancy or Buildings Department records.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-118.3.2", Classification = "Class 2", InfractionCode = "B203", ViolationDescription = "Occupancy contrary to that allowed by the Certificate of Occupancy or Buildings Department records.", Cure = true, Stipulation = true, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-118.3.2", Classification = "Class 3", InfractionCode = "B303", ViolationDescription = "Occupancy contrary to that allowed by the Certificate of Occupancy or Buildings Department records.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-118.3.2.1", Classification = "Class 3", InfractionCode = "B320", ViolationDescription = "Address, block and/or lot, or metes and bounds of zoning lot contrary to Certificate of Occupancy.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-201.1", Classification = "Class 1", InfractionCode = "B187", ViolationDescription = "Unlawful acts. Failure to comply with Commissioner’s order.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-201.1; 28-207.4", Classification = "Class 1", InfractionCode = "B1F9", ViolationDescription = "Failure to obey a Vacate Order from the Commissioner per 28-207.4.", Cure = false, Stipulation = false, StandardPenalty = 4800, MitigatedPenalty = false, DefaultPenalty = 24000, AggravatedPenalty_I = 12000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 24000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-201.1", Classification = "Class 1", InfractionCode = "B1G2", ViolationDescription = "Unlawful acts. Failure to comply with a law, rule, or Commissioner's order involving construction and/or equipment safety operations.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-201.1", Classification = "Class 2", InfractionCode = "B2G4", ViolationDescription = "Unlawful acts. Failure to comply with a law, rule, or Commissioner's order involving construction and/or equipment safety operations.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 10000, AggravatedPenalty_I = 10000, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-202.1", Classification = "Class 1", InfractionCode = "B172", ViolationDescription = "Additional daily penalty for Class 1 violation of 28-210.1 or 28-210-2.", Cure = false, Stipulation = false, StandardPenalty = 0, MitigatedPenalty = false, DefaultPenalty = 45000, AggravatedPenalty_I = 0, AggravatedDefaultPenalty_I = 0, AggravatedPenalty_II = 0, AggravatedDefaultMaxPenalty_II = 0 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-202.1", Classification = "Class 2", InfractionCode = "B298", ViolationDescription = "Additional monthly penalty for continued violation of 28-210.1.", Cure = false, Stipulation = false, StandardPenalty = 0, MitigatedPenalty = false, DefaultPenalty = 10000, AggravatedPenalty_I = 0, AggravatedDefaultPenalty_I = 0, AggravatedPenalty_II = 0, AggravatedDefaultMaxPenalty_II = 0 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-202.1", Classification = "Class 1", InfractionCode = "B199", ViolationDescription = "Additional daily civil penalties for continued violations.", Cure = false, Stipulation = false, StandardPenalty = 0, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 0, AggravatedDefaultPenalty_I = 0, AggravatedPenalty_II = 0, AggravatedDefaultMaxPenalty_II = 0 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-202.1", Classification = "Class 2", InfractionCode = "B299", ViolationDescription = "Additional monthly civil penalties for continued violations.", Cure = false, Stipulation = false, StandardPenalty = 0, MitigatedPenalty = false, DefaultPenalty = 10000, AggravatedPenalty_I = 0, AggravatedDefaultPenalty_I = 0, AggravatedPenalty_II = 0, AggravatedDefaultMaxPenalty_II = 0 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-202.1", Classification = "Class 2", InfractionCode = "B297", ViolationDescription = "Additional monthly penalty for continued violation of 28-210.2.", Cure = false, Stipulation = false, StandardPenalty = 0, MitigatedPenalty = false, DefaultPenalty = 10000, AggravatedPenalty_I = 0, AggravatedDefaultPenalty_I = 0, AggravatedPenalty_II = 0, AggravatedDefaultMaxPenalty_II = 0 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-202.1", Classification = "Class 1", InfractionCode = "B1E6", ViolationDescription = "Additional daily penalty for Class 1 violation of 28-210.3 – permanent dwelling offered/used/converted for other than permanent residential purposes.", Cure = false, Stipulation = false, StandardPenalty = 0, MitigatedPenalty = false, DefaultPenalty = 45000, AggravatedPenalty_I = 0, AggravatedDefaultPenalty_I = 0, AggravatedPenalty_II = 0, AggravatedDefaultMaxPenalty_II = 0 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-204.4", Classification = "Class 2", InfractionCode = "B263", ViolationDescription = "Failure to comply with the Commissioner's order to file a certificate of correction with the Department of Buildings.", Cure = false, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-207.2.2", Classification = "Class 1", InfractionCode = "B112", ViolationDescription = "Unlawfully continued work while on notice of a Stop Work Order.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-207.2.5", Classification = "Class 1", InfractionCode = "B1F8", ViolationDescription = "Tampered with, removed, or defaced a written posted Stop Work Order.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-207.4.4", Classification = "Class 1", InfractionCode = "B1G1", ViolationDescription = "Removed or defaced a written posted Vacate Order.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-210.1", Classification = "Class 1", InfractionCode = "B105", ViolationDescription = "1- or 2-family residence converted to or maintained as a dwelling for more than the number of families legally authorized by the C of O or official records - Less than three additional dwelling units.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-210.1", Classification = "Class 1", InfractionCode = "B1E8", ViolationDescription = "Multiple dwelling converted, maintained, or occupied with 3 or more additional dwelling units than legally authorized by the C of O or official records.", Cure = false, Stipulation = false, StandardPenalty = 15000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-210.2", Classification = "Class 1", InfractionCode = "B1E9", ViolationDescription = "Industrial/manufacturing building converted, maintained, or occupied for residential use for 3 or more additional dwelling units than legally authorized by the C of O or official records.", Cure = false, Stipulation = false, StandardPenalty = 15000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 15000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-210.2", Classification = "Class 2", InfractionCode = "B216", ViolationDescription = "Industrial/manufacturing building converted, maintained, or occupied for residential use contrary to the C of O or official records for less than 3 additional dwelling units.", Cure = false, Stipulation = false, StandardPenalty = 15000, MitigatedPenalty = false, DefaultPenalty = 15000, AggravatedPenalty_I = 15000, AggravatedDefaultPenalty_I = 15000, AggravatedPenalty_II = 15000, AggravatedDefaultMaxPenalty_II = 15000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-210.3", Classification = "Class 1", InfractionCode = "B1E5", ViolationDescription = "Permanent dwelling offered/used/converted for other than permanent residential purposes.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 15000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-210.3", Classification = "Class 2", InfractionCode = "B2F3", ViolationDescription = "Permanent dwelling offered/used/converted for other than permanent residential purposes.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-211.1", Classification = "Class 1", InfractionCode = "B153", ViolationDescription = "Filed a certificate, form, application etc., containing a material false statement(s).", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-211.1", Classification = "Class 1", InfractionCode = "B188", ViolationDescription = "Filed a certificate of correction or other related materials containing material false statement(s).", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-217.1.1", Classification = "Class 2", InfractionCode = "B286", ViolationDescription = "Failure to submit required report of inspection of potentially compromised building.", Cure = true, Stipulation = true, StandardPenalty = 800, MitigatedPenalty = true, DefaultPenalty = 4000, AggravatedPenalty_I = 2000, AggravatedDefaultPenalty_I = 8000, AggravatedPenalty_II = 4000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-217.16", Classification = "Class 1", InfractionCode = "B1A9", ViolationDescription = "Failure to immediately notify Department that building or structure has become potentially compromised.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 1", InfractionCode = "B189", ViolationDescription = "Failure to maintain building in code compliant manner. Lack of required number of means of egress for every floor per BC 1018.1 (2008 code); 27-366; BC 1021.1 (2014 code).", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 2", InfractionCode = "B285", ViolationDescription = "Failure to maintain building in code compliant manner. Exhaust discharge must be no closer than 10 feet from building openings as per MC 501.2 and RS 13-1 Sec. 2-2.1.4.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 1", InfractionCode = "B102", ViolationDescription = "Failure to maintain building in code-compliant manner.", Cure = false, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = false, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 12500, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 2", InfractionCode = "B202", ViolationDescription = "Failure to maintain building in code-compliant manner.", Cure = true, Stipulation = true, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 3", InfractionCode = "B302", ViolationDescription = "Failure to maintain building in code-compliant manner.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 1", InfractionCode = "B154", ViolationDescription = "Failure to maintain building in code-compliant manner: service equipment – boiler.", Cure = false, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = false, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 12500, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 2", InfractionCode = "B254", ViolationDescription = "Failure to maintain building in code-compliant manner: service equipment – boiler.", Cure = true, Stipulation = true, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 3", InfractionCode = "B354", ViolationDescription = "Failure to maintain building in code-compliant manner: service equipment – boiler.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 1", InfractionCode = "B126", ViolationDescription = "Failure to maintain building in code-compliant manner: Use of prohibited door and/or hardware per BC 1008.1.8; 27-371(j).", Cure = false, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = false, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 12500, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 2", InfractionCode = "B226", ViolationDescription = "Failure to maintain building in code-compliant manner: Use of prohibited door and/or hardware per BC 1008.1.8; 27-371(j).", Cure = true, Stipulation = false, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 1", InfractionCode = "B132", ViolationDescription = "Failure to maintain building in code-compliant manner: illumination for exits, exit discharges and public corridors per BC 1006.1; 27-381.", Cure = false, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = false, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 12500, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 2", InfractionCode = "B232", ViolationDescription = "Failure to maintain building in code-compliant manner: illumination for exits, exit discharges and public corridors per BC 1006.1; 27-381.", Cure = true, Stipulation = false, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 1", InfractionCode = "B133", ViolationDescription = "Failure to maintain building in code-compliant manner: floor numbering signs missing and/or defective per BC 1019.1.7 (2008 code); 27-392; BC 1022.8 (2014 code).", Cure = false, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = false, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 12500, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 2", InfractionCode = "B233", ViolationDescription = "Failure to maintain building in code-compliant manner: floor numbering signs missing and/or defective per BC 1019.1.7 (2008 code); 27-392; BC 1022.8 (2014 code).", Cure = true, Stipulation = true, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 1", InfractionCode = "B136", ViolationDescription = "Failure to maintain building in code-compliant manner: high-rise to provide exit sign requirement(s) within exits per BC 1011.1.1; 27-383.1.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 2", InfractionCode = "B236", ViolationDescription = "Failure to maintain building in code-compliant manner: high-rise to provide exit sign requirement(s) within exits per BC 1011.1.1; 27-383.1.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 1", InfractionCode = "B137", ViolationDescription = "Failure to maintain building in code-compliant manner: lack of emergency power or storage battery connection to exit signs per BC 1011.5.3; 27-384(c).", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 1", InfractionCode = "B138", ViolationDescription = "Failure to maintain building in code-compliant manner: lack of emergency lighting for exits, exit discharges and public corridors per BC 1006.1; 27-542.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 2", InfractionCode = "B238", ViolationDescription = "Failure to maintain building in code-compliant manner: lack of emergency lighting for exits, exit discharges and public corridors per BC 1006.1; 27-542.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 2", InfractionCode = "B249", ViolationDescription = "Failure to maintain building in code-compliant manner: failure to provide non-combustible proscenium curtain or stage water curtain per BC 410.3.5; 27-546.", Cure = true, Stipulation = true, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 1", InfractionCode = "B139", ViolationDescription = "Failure to maintain building in code-compliant manner: no fire stopping per BC 712.3; 27-345.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 2", InfractionCode = "B239", ViolationDescription = "Failure to maintain building in code-compliant manner: no fire stopping per BC 712.3; 27-345.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 1", InfractionCode = "B140", ViolationDescription = "Failure to maintain building in code-compliant manner: Improper exit/exit access doorway arrangement per BC 1014.2 (2008 code); 27-361; BC 1015.2 (2014 code).", Cure = false, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = false, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 12500, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 1", InfractionCode = "B155", ViolationDescription = "Failure to maintain building in code-compliant manner: lack of a system of automatic sprinklers where required per BC 903.2; 27-954.", Cure = false, Stipulation = false, StandardPenalty = 1000, MitigatedPenalty = false, DefaultPenalty = 5000, AggravatedPenalty_I = 2500, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 5000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 2", InfractionCode = "B255", ViolationDescription = "Failure to maintain building in code-compliant manner: lack of a system of automatic sprinklers where required per BC 903.2; 27-954.", Cure = false, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 2", InfractionCode = "B266", ViolationDescription = "Failure to maintain building in code-compliant manner re: installation/maintenance of plumbing materials/ equipment per PC102.3; 27-902.", Cure = true, Stipulation = true, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 2", InfractionCode = "B267", ViolationDescription = "Failure to maintain building in code-compliant manner: Gas vent reduced or undersized as per FGC 504.2; 27-887.", Cure = false, Stipulation = false, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 2", InfractionCode = "B268", ViolationDescription = "Failure to maintain building in code-compliant manner: failure to comply with law for water supply system per PC 602.3; 27-908(c).", Cure = false, Stipulation = false, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 2", InfractionCode = "B269", ViolationDescription = "Failure to maintain building in code-compliant manner: failure to comply with law for drainage system per PC 702.1; 27-911.", Cure = false, Stipulation = false, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 2", InfractionCode = "B273", ViolationDescription = "Failure to maintain building in code-compliant manner: Plumbing fixture(s) not trapped and/or vented per PC 916.1 & PC 1002.1; 27-901(o).", Cure = false, Stipulation = false, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 1", InfractionCode = "B163", ViolationDescription = "Failure to maintain building in code-compliant manner: Misc sign violation by Outdoor Ad Co as per 27-498 through 27-508 & BC H103.1.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-301.1", Classification = "Class 2", InfractionCode = "B278", ViolationDescription = "Failure to maintain sign in accordance w Tit.27; Tit.28; ZR; RCNY.", Cure = false, Stipulation = true, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-302.1", Classification = "Class 1", InfractionCode = "B104", ViolationDescription = "Failure to maintain building wall(s) or appurtenances.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-302.1", Classification = "Class 2", InfractionCode = "B204", ViolationDescription = "Failure to maintain building wall(s) or appurtenances.", Cure = true, Stipulation = true, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-302.1", Classification = "Class 3", InfractionCode = "B304", ViolationDescription = "Failure to maintain building wall(s) or appurtenances.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-302.3", Classification = "Class 1", InfractionCode = "B1E3", ViolationDescription = "Failure of registered design professional to immediately notify the department of unsafe façade condition(s).", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-302.4", Classification = "Class 2", InfractionCode = "B230", ViolationDescription = "Failure to submit a required report of critical examination documenting condition of exterior wall and appurtenances.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-302.5", Classification = "Class 1", InfractionCode = "B1E4", ViolationDescription = "Failure to take required measures to secure public safety – unsafe façade.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-302.5", Classification = "Class 2", InfractionCode = "B227", ViolationDescription = "Failure to file an amended report acceptable to this Department indicating correction of unsafe conditions.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-303.7", Classification = "Class 2", InfractionCode = "B265", ViolationDescription = "Failure to file a complete boiler inspection report.", Cure = false, Stipulation = false, StandardPenalty = 625, MitigatedPenalty = false, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-304.1", Classification = "Class 1", InfractionCode = "B151", ViolationDescription = "Failure to maintain elevator or conveying system.", Cure = false, Stipulation = false, StandardPenalty = 12500, MitigatedPenalty = false, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 12500, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-304.1", Classification = "Class 2", InfractionCode = "B251", ViolationDescription = "Failure to maintain elevator or conveying system.", Cure = true, Stipulation = true, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-304.1", Classification = "Class 3", InfractionCode = "B351", ViolationDescription = "Failure to maintain elevator or conveying system.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-304.6", Classification = "Class 1", InfractionCode = "B1F7", ViolationDescription = "Failure to inspect or test elevator or conveying system.", Cure = false, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = false, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 12500, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-304.6", Classification = "Class 2", InfractionCode = "B2F8", ViolationDescription = "Failure to inspect or test elevator or conveying system.", Cure = true, Stipulation = true, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-304.10", Classification = "Class 2", InfractionCode = "B2F9", ViolationDescription = "Failure to provide notice of elevator to be out of service for alteration work.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 10000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-305.4.4", Classification = "Class 2", InfractionCode = "B287", ViolationDescription = "Failure to submit required report of condition assessment of retaining wall.", Cure = true, Stipulation = true, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-305.4.6", Classification = "Class 1", InfractionCode = "B1B1", ViolationDescription = "Failure to immediately notify Department of unsafe condition observed during condition assessment of retaining wall.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-305.4.7.3", Classification = "Class 2", InfractionCode = "B288", ViolationDescription = "Failure to file an amended condition assessment acceptable to Department indicating correction of unsafe conditions.", Cure = true, Stipulation = true, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-317.3", Classification = "Class 2", InfractionCode = "B289", ViolationDescription = "Failure to register cooling tower prior to operation.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-317.3.1", Classification = "Class 2", InfractionCode = "B290", ViolationDescription = "Failure to notify of discontinued use or removal of cooling tower.", Cure = true, Stipulation = true, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-317.5", Classification = "Class 2", InfractionCode = "B291", ViolationDescription = "Failure to file an annual certification of cooling tower inspection / testing / cleaning / disinfecting / maintenance plan per Adm. Code §17-194.1.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "Misc. Chapter 4 of Title 28 – Unlicensed Activity", Classification = "Class 1", InfractionCode = "B191, A191", ViolationDescription = "Illegally engaging in any business or occupation without a required license or other authorization.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-401.9", Classification = "Class 1", InfractionCode = "B147", ViolationDescription = "Failure to file evidence of liability &/or property damage insurance.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-401.9", Classification = "Class 1", InfractionCode = "B148", ViolationDescription = "Failure to file evidence of compliance with Workers Comp, law and/or disability benefits law.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-404.1", Classification = "Class 1", InfractionCode = "B141", ViolationDescription = "Supervision or use of rigging equipment without a Rigger's license.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-404.4.1", Classification = "Class 2", InfractionCode = "B259", ViolationDescription = "Licensed Master/Special Rigger failed to place appropriate “Danger” sign while using rigging equipment.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-405.1", Classification = "Class 1", InfractionCode = "B143", ViolationDescription = "Supervision or use of power-operated hoisting machine without a Hoisting Machine Operator's license.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-406.1", Classification = "Class 1", InfractionCode = "B1C5", ViolationDescription = "Unlicensed concrete testing activity.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-408.1", Classification = "Class 1", InfractionCode = "B190, A190", ViolationDescription = "Performing unlicensed plumbing work without a master plumber license.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-415.1", Classification = "Class 1", InfractionCode = "B1B8", ViolationDescription = "Hoisting, lowering, hanging, or attaching of outdoor sign not performed or supervised by a properly licensed sign hanger.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-502.2", Classification = "Class 1", InfractionCode = "B1B2", ViolationDescription = "Outdoor Advertising Company engaged in outdoor advertising business without a valid registration.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-502.2.1", Classification = "Class 1", InfractionCode = "B1B3", ViolationDescription = "Outdoor Advertising Company failed to submit complete/accurate information as prescribed in 1 RCNY Chapter 49.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-502.2.2", Classification = "Class 1", InfractionCode = "B1B4", ViolationDescription = "Outdoor Advertising Company failed to post, renew, or replenish bond or other form of security.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-502.5", Classification = "Class 1", InfractionCode = "B1B5", ViolationDescription = "Outdoor Advertising Company failed to post required information at sign location.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-502.6", Classification = "Class 1", InfractionCode = "B162", ViolationDescription = "Misc sign violation by Outdoor Advertising Company of Title 27; Title 28; ZR; or BC", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-504.1.2", Classification = "Class 2", InfractionCode = "B2A3", ViolationDescription = "Failure to complete / implement / amend bicycle access plan or provide request for exception.", Cure = false, Stipulation = false, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-504.1.4", Classification = "Class 2", InfractionCode = "B2A5", ViolationDescription = "Failure to post a bicycle access plan/letter of exception / notice of availability of plan / letter.", Cure = false, Stipulation = false, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "28-504.1.4", Classification = "Class 2", InfractionCode = "B2A6", ViolationDescription = "Failure to timely file bicycle access plan or amendment with DOT as prescribed in 34 RCNY 2-19.", Cure = false, Stipulation = false, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 105.8.2", Classification = "Class 2", InfractionCode = "B207", ViolationDescription = "Temporary Construction Equipment on Site – Expired Permit.", Cure = true, Stipulation = true, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 903.6", Classification = "Class 2", InfractionCode = "B2B7", ViolationDescription = "Failure to paint dedicated sprinkler piping/valves in accordance with section.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 903.6", Classification = "Class 2", InfractionCode = "B2B8", ViolationDescription = "Failure to provide / maintain painting certification of sprinkler and combination sprinkler / standpipe systems in accordance with section.", Cure = true, Stipulation = false, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 905.11", Classification = "Class 2", InfractionCode = "B2B9", ViolationDescription = "Failure to paint dedicated standpipe / valves in accordance with section.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 905.11", Classification = "Class 2", InfractionCode = "B2C1", ViolationDescription = "Failure to provide / maintain painting certification of standpipe and combination sprinkler / standpipe systems in accordance with section.", Cure = true, Stipulation = false, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 1016.2", Classification = "Class 2", InfractionCode = "B221", ViolationDescription = "Failure to maintain building in code-compliant manner: provide required corridor width per BC 1016.2; 27-369.", Cure = true, Stipulation = false, StandardPenalty = 625, MitigatedPenalty = true, DefaultPenalty = 3125, AggravatedPenalty_I = 1563, AggravatedDefaultPenalty_I = 6250, AggravatedPenalty_II = 3125, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 1704.4", Classification = "Class 2", InfractionCode = "B2B5", ViolationDescription = "Failure to perform special inspections and verifications for concrete construction as required by section and Table 1704.4.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 10000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 1704.21.1 (2008 code) & BC 1704.23.1 (2014 code)", Classification = "Class 1", InfractionCode = "B1C1", ViolationDescription = "Failure to perform successful hydrostatic pressure test of sprinkler system.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 1704.22.1 (2008 code) & BC 1704.24.1 (2014 code)", Classification = "Class 1", InfractionCode = "B1C2", ViolationDescription = "Failure to perform successful hydrostatic pressure test of standpipe system.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 1905.6.3.2 (2008 code) & BC 1905.6.3.3 (2014 code)", Classification = "Class 2", InfractionCode = "B2B6", ViolationDescription = "Failure to comply with ASTM C31 standards for concrete cylinder test samples.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 10000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3010.1 & 27-1006", Classification = "Class 1", InfractionCode = "B152", ViolationDescription = "Failure to promptly report an elevator accident involving personal injury requiring the services of a physician or damage to property.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3301.1.3 (2014 code)", Classification = "Class 1", InfractionCode = "B1F1", ViolationDescription = "Failure to comply with manufacturer specifications.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3301.1.3 (2014 code)", Classification = "Class 2", InfractionCode = "B2F6", ViolationDescription = "Failure to comply with manufacturer specifications.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3301.2 & 27-1009(a)", Classification = "Class 1", InfractionCode = "B109", ViolationDescription = "Failure to safeguard all persons and property affected by construction operations.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3301.2 & 27-1009(a)", Classification = "Class 2", InfractionCode = "B209", ViolationDescription = "Failure to safeguard all persons and property affected by construction operations.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 10000, AggravatedPenalty_I = 10000, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3301.2 & 27-1009(a)", Classification = "Class 1", InfractionCode = "B115", ViolationDescription = "Failure to institute / maintain safety equipment measures or temporary construction – No guard rails.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3301.2 & 27-1009(a)", Classification = "Class 1", InfractionCode = "B118", ViolationDescription = "Failure to institute / maintain safety equipment measures or temporary construction – No toe boards.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3301.2 & 27-1009(a)", Classification = "Class 1", InfractionCode = "B120", ViolationDescription = "Failure to institute / maintain safety equipment measures or temporary construction – No handrails.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3301.7 (2014 code)", Classification = "Class 1", InfractionCode = "B1F2", ViolationDescription = "Failure to maintain / display on site documents required by BC Chapter 33.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = true, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3301.7 (2014 code)", Classification = "Class 2", InfractionCode = "B2F7", ViolationDescription = "Failure to maintain / display on site documents required by BC Chapter 33.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3301.8", Classification = "Class 1", InfractionCode = "B192", ViolationDescription = "Failure to promptly notify the Department of an accident or damage to adjoining property at construction / demolition site.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3301.9", Classification = "Class 2", InfractionCode = "B217", ViolationDescription = "Project Information Panel / Sidewalk Shed Parapet Panel / Construction Sign not provided or not in compliance with section.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3303.4 & 27-1018", Classification = "Class 1", InfractionCode = "B181", ViolationDescription = "Failure to maintain adequate housekeeping per section requirements.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3303.4 & 27-1018", Classification = "Class 2", InfractionCode = "B212", ViolationDescription = "Failure to maintain adequate housekeeping per section requirements.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3303.4.5 & 27-1018", Classification = "Class 1", InfractionCode = "B194", ViolationDescription = "Unsafe storage of materials during construction or demolition.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3303.4.6 & 27-1018", Classification = "Class 1", InfractionCode = "B183", ViolationDescription = "Unsafe storage of combustible material and equipment.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3303.7.3", Classification = "Class 1", InfractionCode = "B1B9", ViolationDescription = "Smoking at construction / demolition site.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3303.7.3", Classification = "Class 2", InfractionCode = "B2A8", ViolationDescription = "Smoking at construction / demolition site.", Cure = false, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = false, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3303.7.3", Classification = "Class 2", InfractionCode = "B2A9", ViolationDescription = "Failure to post No Smoking signs at construction / demolition sites per Fire Code.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3303.8.1", Classification = "Class 1", InfractionCode = "B1C3", ViolationDescription = "Failure to provide standpipe or air pressurized alarm system for standpipe system during construction or demolition operation.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3303.8.1", Classification = "Class 1", InfractionCode = "B1C4", ViolationDescription = "Failure to conduct proper planned removal from service of standpipe system and / or standpipe air pressurized alarm.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3303.15", Classification = "Class 2", InfractionCode = "B2F1", ViolationDescription = "Failure to perform proper concrete washout water procedures.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 10000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3304.3 & 1 RCNY 52-01(a)", Classification = "Class 1", InfractionCode = "B111", ViolationDescription = "Failure to notify the Department prior to the commencement of earthwork.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3304.3 & 1 RCNY 52-01(b)", Classification = "Class 2", InfractionCode = "B215", ViolationDescription = "Failure to notify the Department prior to the cancellation of earthwork .", Cure = false, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3304.4 & 27-1032", Classification = "Class 1", InfractionCode = "B110", ViolationDescription = "Failure to provide protection at sides of excavation.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3304.12 (2014 code)", Classification = "Class 1", InfractionCode = "B1F3", ViolationDescription = "Failure to perform slurry operations in accordance with section.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3305.3.1.2.1 (2014 code)", Classification = "Class 1", InfractionCode = "B1F4", ViolationDescription = "Failure to obtain registered design professional evaluation prior to using existing structure to support formwork loads.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3305.3.2 (2014 code)", Classification = "Class 1", InfractionCode = "B1F5", ViolationDescription = "No site-specific formwork design drawings present per 3301.7.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3305.3.3.2 (2014 code)", Classification = "Class 1", InfractionCode = "B1F6", ViolationDescription = "Failure to perform required formwork observation.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3306 & 27-1039", Classification = "Class 1", InfractionCode = "B117", ViolationDescription = "Failure to carry out demolition operations as required by section.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3306.2.1", Classification = "Class 1", InfractionCode = "B176", ViolationDescription = "Failure to provide safety zone for demolition operations.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3306.3& 27-195", Classification = "Class 1", InfractionCode = "B116", ViolationDescription = "Failure to provide required notification prior to the commencement of demolition.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3306.5", Classification = "Class 1", InfractionCode = "B175", ViolationDescription = "Mechanical demolition without plans on site.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3307.1", Classification = "Class 2", InfractionCode = "B223", ViolationDescription = "Pedestrian protection does not meet code specifications.", Cure = false, Stipulation = false, StandardPenalty = 2400, MitigatedPenalty = false, DefaultPenalty = 10000, AggravatedPenalty_I = 6000, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3307.1.1 (2008 code) & BC 3307.4.6 (2014 code)", Classification = "Class 1", InfractionCode = "B159", ViolationDescription = "Prohibited Outdoor Advertising Company sign on sidewalk shed or construction fence.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3307.1.1 (2008 code) & BC 3307.4.6 (2014 code)", Classification = "Class 2", InfractionCode = "B2F4", ViolationDescription = "Posting of unlawful signs, information, pictorial representation, business or advertising messages on protective structures.", Cure = true, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3307.3 (2008 Code) and BC 3307.1 (2014 Code)", Classification = "Class 1", InfractionCode = "B1E7", ViolationDescription = "Failure to provide pedestrian protection for sidewalks and walkways.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3307.3.1 (2008 code), 27-1021(a) & BC 3307.6.2 (2014 code)", Classification = "Class 1", InfractionCode = "B131", ViolationDescription = "Failure to provide sidewalk shed where required.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3307.6.4 (2008 code) & BC 3307.6.4.11 (2014 code)", Classification = "Class 2", InfractionCode = "B2F5", ViolationDescription = "Sidewalk shed does not meet color specification.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3307.7", Classification = "Class 2", InfractionCode = "B211", ViolationDescription = "Job site fence not constructed or maintained pursuant to subsection.", Cure = true, Stipulation = false, StandardPenalty = 800, MitigatedPenalty = true, DefaultPenalty = 4000, AggravatedPenalty_I = 2000, AggravatedDefaultPenalty_I = 8000, AggravatedPenalty_II = 4000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3309.4 & 27-1031", Classification = "Class 1", InfractionCode = "B123", ViolationDescription = "Failure to protect adjoining structures during excavation operations.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3301.3 & BC 3310.5 & BC 3310.5.2", Classification = "Class 1", InfractionCode = "B119", ViolationDescription = "Failure to designate and / or have Site Safety Manager or Site Safety Coordinator present at site as required.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3310.8.2 (2008 code) & BC 3310.8.2.1 (2014 code)", Classification = "Class 1", InfractionCode = "B193", ViolationDescription = "Site Safety Manager / Coordinator failed to immediately notify the Department of conditions as required.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3310.9.1", Classification = "Class 1", InfractionCode = "B174", ViolationDescription = "No Concrete Safety Manager present at site as required.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3314.1.1 (2008 code), 27-1050.1 & BC 3314.4.1.5 (2014 code)", Classification = "Class 2", InfractionCode = "B261", ViolationDescription = "Failed to notify Department prior to installation or removal of Suspended Scaffold.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3314.2 & 27-1042", Classification = "Class 1", InfractionCode = "B130", ViolationDescription = "Erected or installed supported scaffold 40 feet or higher without a permit.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3314.4.3.1 (2008 code), 27-1045 & BC 3314.4.3 (2014 code)", Classification = "Class 1", InfractionCode = "B149", ViolationDescription = "Failure to perform safe / proper inspection of Suspended Scaffold.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3314.4.3.1 (2008 code), 27-1045(b) & BC 3314.4.3.4 (2014 code)", Classification = "Class 1", InfractionCode = "B150", ViolationDescription = "No record of daily inspection of Suspended Scaffold performed by authorized person at site.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3314.4.5 (2008 code & BC 3314.4.5.1 (2014 code)", Classification = "Class 1", InfractionCode = "B129", ViolationDescription = "Unqualified supervisor or worker performing work on scaffold.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3314.4.6 (2008 code) & BC 3314.4.5.8 (2014 Code)", Classification = "Class 2", InfractionCode = "B2G3", ViolationDescription = "Scaffold training certificate card not readily available for inspection.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3314.6.3 & 27-1009", Classification = "Class 1", InfractionCode = "B146", ViolationDescription = "Failure to provide / use lifeline while working on scaffold.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3314.6.3 & 27-1009", Classification = "Class 2", InfractionCode = "B246", ViolationDescription = "Failure to provide / use lifeline while working on scaffold.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3316.2 & BC 3319.1", Classification = "Class 1", InfractionCode = "B142", ViolationDescription = "Inadequate safety measures: Operation of crane / derrick / hoisting equipment in unsafe manner.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3319.3", Classification = "Class 1", InfractionCode = "B177", ViolationDescription = "Operation of a crane / derrick without a Certificate of Operation / Certificate of Approval.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3319.3 & 27-1057(d)", Classification = "Class 2", InfractionCode = "B253", ViolationDescription = "Operation of a crane / derrick without a Certificate of Onsite Inspection.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 10000, AggravatedPenalty_I = 10000, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3319.8", Classification = "Class 1", InfractionCode = "B1A1", ViolationDescription = "Failure to provide erection, jumping, climbing, dismantling plan for tower / climber crane.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3319.8.2", Classification = "Class 1", InfractionCode = "B1A2", ViolationDescription = "Failure to conduct a safety coordination meeting.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3319.8.3", Classification = "Class 1", InfractionCode = "B1A3", ViolationDescription = "Failure to conduct a pre-jump safety meeting.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 12500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3319.8.4", Classification = "Class 1", InfractionCode = "B1A4", ViolationDescription = "Failure to notify the Department prior to pre-jump or safety coordination meeting.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3319.8.4.2", Classification = "Class 1", InfractionCode = "B1A5", ViolationDescription = "Failure to provide time schedule indicating erection, jumping, climbing, or dismantling of crane.", Cure = false, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = false, DefaultPenalty = 6500, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 12500, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3319.8.6", Classification = "Class 1", InfractionCode = "B1A6", ViolationDescription = "No meeting log available", Cure = false, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = false, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 12500, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3319.8.7", Classification = "Class 1", InfractionCode = "B1A7", ViolationDescription = "Failure to file a complete and acceptable tower / climber Installation Report per BC 3319.8.7.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "BC 3319.8.8", Classification = "Class 1", InfractionCode = "B1A8", ViolationDescription = "Erection, jumping, climbing, dismantling operations of a tower or climber crane not in accordance with 3319.8.8.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC-Misc", Classification = "Class 1", InfractionCode = "B1D1", ViolationDescription = "Miscellaneous violation of the Electrical Code Technical Standards.", Cure = false, Stipulation = false, StandardPenalty = 1600, MitigatedPenalty = false, DefaultPenalty = 8000, AggravatedPenalty_I = 4000, AggravatedDefaultPenalty_I = 16000, AggravatedPenalty_II = 8000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC-Misc", Classification = "Class 2", InfractionCode = "B2C6", ViolationDescription = "Miscellaneous violation of the Electrical Code Technical Standards.", Cure = true, Stipulation = true, StandardPenalty = 800, MitigatedPenalty = true, DefaultPenalty = 4000, AggravatedPenalty_I = 2000, AggravatedDefaultPenalty_I = 8000, AggravatedPenalty_II = 4000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC-Misc", Classification = "Class 3", InfractionCode = "B311", ViolationDescription = "Miscellaneous violation of the Electrical Code Technical Standards.", Cure = true, Stipulation = true, StandardPenalty = 400, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 110.2(A)", Classification = "Class 1", InfractionCode = "B1D2", ViolationDescription = "Unapproved / unsafe / unsuitable electrical equipment, apparatus, materials, devices, appliances or wiring in use.", Cure = false, Stipulation = false, StandardPenalty = 1600, MitigatedPenalty = false, DefaultPenalty = 8000, AggravatedPenalty_I = 4000, AggravatedDefaultPenalty_I = 16000, AggravatedPenalty_II = 8000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 110.2(A)", Classification = "Class 2", InfractionCode = "B2C9", ViolationDescription = "Unapproved / unsafe / unsuitable electrical equipment, apparatus, materials, devices, appliances or wiring in use.", Cure = true, Stipulation = true, StandardPenalty = 800, MitigatedPenalty = true, DefaultPenalty = 4000, AggravatedPenalty_I = 2000, AggravatedDefaultPenalty_I = 8000, AggravatedPenalty_II = 4000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 110.2(B)", Classification = "Class 2", InfractionCode = "B2D1", ViolationDescription = "Constructed electrical installation without required commissioner's approval per section.", Cure = false, Stipulation = false, StandardPenalty = 2400, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6000, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 110.2", Classification = "Class 2", InfractionCode = "B2C8", ViolationDescription = "Failure to use approved conductors and / or equipment.", Cure = false, Stipulation = false, StandardPenalty = 1000, MitigatedPenalty = true, DefaultPenalty = 5000, AggravatedPenalty_I = 2500, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 5000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 110.2", Classification = "Class 3", InfractionCode = "B313", ViolationDescription = "Failure to use approved conductors and / or equipment.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 110.12", Classification = "Class 3", InfractionCode = "B312", ViolationDescription = "Failure to close unused openings (knockouts) in outlet/panel box.", Cure = true, Stipulation = true, StandardPenalty = 300, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 110.14(A)", Classification = "Class 2", InfractionCode = "B2C7", ViolationDescription = "Failure to properly connect conductors to terminals.", Cure = false, Stipulation = false, StandardPenalty = 1200, MitigatedPenalty = true, DefaultPenalty = 6000, AggravatedPenalty_I = 3000, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 110.25", Classification = "Class 1", InfractionCode = "B1D3", ViolationDescription = "Electrical closet not dedicated to electrical distribution equipment only.", Cure = false, Stipulation = false, StandardPenalty = 1200, MitigatedPenalty = false, DefaultPenalty = 6000, AggravatedPenalty_I = 3000, AggravatedDefaultPenalty_I = 12000, AggravatedPenalty_II = 6000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 110.25", Classification = "Class 2", InfractionCode = "B2D2", ViolationDescription = "Electrical closet not dedicated to electrical distribution equipment only.", Cure = true, Stipulation = true, StandardPenalty = 600, MitigatedPenalty = true, DefaultPenalty = 3000, AggravatedPenalty_I = 1500, AggravatedDefaultPenalty_I = 6000, AggravatedPenalty_II = 3000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 110.25", Classification = "Class 3", InfractionCode = "B314", ViolationDescription = "Electrical closet not dedicated to electrical distribution equipment only.", Cure = true, Stipulation = true, StandardPenalty = 300, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 110.26", Classification = "Class 2", InfractionCode = "B2D3", ViolationDescription = "Failure to provide / maintain sufficient access / work space about electrical equipment.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 2500, AggravatedPenalty_I = 1250, AggravatedDefaultPenalty_I = 5000, AggravatedPenalty_II = 2500, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 210.8", Classification = "Class 2", InfractionCode = "B2D5", ViolationDescription = "Failure to install Ground-fault circuit interrupter (GFCI) protection as required.", Cure = false, Stipulation = false, StandardPenalty = 2400, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6000, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 210.12(B)", Classification = "Class 2", InfractionCode = "B2D4", ViolationDescription = "Failure to provide Arc-fault circuit interrupter (AFCI) protection in dwelling units.", Cure = false, Stipulation = false, StandardPenalty = 2400, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6000, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 210.52(A)", Classification = "Class 3", InfractionCode = "B315", ViolationDescription = "Failure to provide proper spacing between receptacle outlets.", Cure = true, Stipulation = true, StandardPenalty = 300, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 230.72(A)", Classification = "Class 1", InfractionCode = "B1D4", ViolationDescription = "Failure to properly group / label disconnects.", Cure = false, Stipulation = false, StandardPenalty = 1000, MitigatedPenalty = true, DefaultPenalty = 5000, AggravatedPenalty_I = 2500, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 5000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 230.72(A)", Classification = "Class 2", InfractionCode = "B2D6", ViolationDescription = "Failure to properly group / label disconnects.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 2500, AggravatedPenalty_I = 1250, AggravatedDefaultPenalty_I = 5000, AggravatedPenalty_II = 2500, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 240.3", Classification = "Class 1", InfractionCode = "B1D5", ViolationDescription = "Failure to provide adequate circuit overcurrent protection device per table.", Cure = false, Stipulation = false, StandardPenalty = 4800, MitigatedPenalty = false, DefaultPenalty = 24000, AggravatedPenalty_I = 12000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 24000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 240.3", Classification = "Class 2", InfractionCode = "B2D8", ViolationDescription = "Failure to provide adequate circuit overcurrent protection device per table.", Cure = false, Stipulation = false, StandardPenalty = 2400, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6000, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 240.3", Classification = "Class 3", InfractionCode = "B316", ViolationDescription = "Failure to provide adequate circuit overcurrent protection device per table.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 240.4", Classification = "Class 1", InfractionCode = "B1D6", ViolationDescription = "Failure to protect conductor(s) against overcurrent per EC.", Cure = false, Stipulation = false, StandardPenalty = 4800, MitigatedPenalty = false, DefaultPenalty = 24000, AggravatedPenalty_I = 12000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 24000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 240.4", Classification = "Class 2", InfractionCode = "B2D9", ViolationDescription = "Failure to protect conductor(s) against overcurrent per EC.", Cure = false, Stipulation = false, StandardPenalty = 2400, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6000, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 240.21", Classification = "Class 2", InfractionCode = "B2D7", ViolationDescription = "Tap conductors not in compliance with section.", Cure = false, Stipulation = false, StandardPenalty = 1000, MitigatedPenalty = true, DefaultPenalty = 5000, AggravatedPenalty_I = 2500, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 5000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 250.4", Classification = "Class 1", InfractionCode = "B1D7", ViolationDescription = "Failure to ground electrical systems.", Cure = false, Stipulation = false, StandardPenalty = 4800, MitigatedPenalty = false, DefaultPenalty = 24000, AggravatedPenalty_I = 12000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 24000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 250.4", Classification = "Class 1", InfractionCode = "B1D8", ViolationDescription = "Failure to properly bond electrical systems.", Cure = false, Stipulation = false, StandardPenalty = 3000, MitigatedPenalty = false, DefaultPenalty = 15000, AggravatedPenalty_I = 7500, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 15000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 250.4", Classification = "Class 2", InfractionCode = "B2E1", ViolationDescription = "Failure to properly bond electrical systems.", Cure = false, Stipulation = false, StandardPenalty = 1500, MitigatedPenalty = true, DefaultPenalty = 7500, AggravatedPenalty_I = 3750, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 7500, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 250.4", Classification = "Class 3", InfractionCode = "B317", ViolationDescription = "Failure to properly bond electrical systems.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 250.4", Classification = "Class 2", InfractionCode = "B2E2", ViolationDescription = "Failure to provide adequate grounding of electrical systems.", Cure = false, Stipulation = false, StandardPenalty = 2400, MitigatedPenalty = true, DefaultPenalty = 10000, AggravatedPenalty_I = 6000, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 250.64", Classification = "Class 1", InfractionCode = "B1D9", ViolationDescription = "Failure to install grounding electrode conductor in accordance with section.", Cure = false, Stipulation = false, StandardPenalty = 4800, MitigatedPenalty = false, DefaultPenalty = 24000, AggravatedPenalty_I = 12000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 24000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 300.10", Classification = "Class 2", InfractionCode = "B2E3", ViolationDescription = "Failure to provide effective electrical continuity for metal raceways / enclosures / cable armor.", Cure = false, Stipulation = false, StandardPenalty = 1200, MitigatedPenalty = true, DefaultPenalty = 6000, AggravatedPenalty_I = 3000, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 300.11", Classification = "Class 2", InfractionCode = "B2E4", ViolationDescription = "Failure to secure / support raceways / cable assemblies / boxes / cabinets / fittings.", Cure = true, Stipulation = true, StandardPenalty = 800, MitigatedPenalty = true, DefaultPenalty = 4000, AggravatedPenalty_I = 2000, AggravatedDefaultPenalty_I = 8000, AggravatedPenalty_II = 4000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 314.23", Classification = "Class 3", InfractionCode = "B318", ViolationDescription = "Failure to secure electrical device enclosure per section requirement.", Cure = true, Stipulation = true, StandardPenalty = 300, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 314.25", Classification = "Class 2", InfractionCode = "B2E5", ViolationDescription = "Failure to provide cover / faceplate / lampholder / luminaire canopy for electrical outlet.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 2500, AggravatedPenalty_I = 1250, AggravatedDefaultPenalty_I = 5000, AggravatedPenalty_II = 2500, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 358.12", Classification = "Class 2", InfractionCode = "B2E6", ViolationDescription = "Prohibited use of electrical metallic tubing (EMT).", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 2500, AggravatedPenalty_I = 1250, AggravatedDefaultPenalty_I = 5000, AggravatedPenalty_II = 2500, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 358.30", Classification = "Class 2", InfractionCode = "B2E7", ViolationDescription = "Failure to properly secure / support electrical metallic tubing (EMT).", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 2500, AggravatedPenalty_I = 1250, AggravatedDefaultPenalty_I = 5000, AggravatedPenalty_II = 2500, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 408.4", Classification = "Class 3", InfractionCode = "B319", ViolationDescription = "Failure to provide required circuit directory / identification.", Cure = true, Stipulation = true, StandardPenalty = 200, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 410.30", Classification = "Class 2", InfractionCode = "B2E8", ViolationDescription = "Luminaires and Lampholders not installed in an approved manner.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 2500, AggravatedPenalty_I = 1250, AggravatedDefaultPenalty_I = 5000, AggravatedPenalty_II = 2500, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 590.4(G)", Classification = "Class 2", InfractionCode = "B2E9", ViolationDescription = "Improper splicing of temporary wiring.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 2500, AggravatedPenalty_I = 1250, AggravatedDefaultPenalty_I = 5000, AggravatedPenalty_II = 2500, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 590.4(J)", Classification = "Class 1", InfractionCode = "B1E2", ViolationDescription = "Failure to provide proper support for temporary wiring.", Cure = false, Stipulation = false, StandardPenalty = 1600, MitigatedPenalty = false, DefaultPenalty = 8000, AggravatedPenalty_I = 4000, AggravatedDefaultPenalty_I = 16000, AggravatedPenalty_II = 8000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "EC 590.4(J)", Classification = "Class 1", InfractionCode = "B1E1", ViolationDescription = "Failure to protect temporary wiring from improper contact per section.", Cure = false, Stipulation = false, StandardPenalty = 1600, MitigatedPenalty = false, DefaultPenalty = 8000, AggravatedPenalty_I = 4000, AggravatedDefaultPenalty_I = 16000, AggravatedPenalty_II = 8000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "PC-Misc, FGC-Misc, MC-Misc", Classification = "Class 1", InfractionCode = "B180", ViolationDescription = "Miscellaneous violations.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "PC-Misc, FGC-Misc, MC-Misc", Classification = "Class 2", InfractionCode = "B280", ViolationDescription = "Miscellaneous violations.", Cure = true, Stipulation = true, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "PC-Misc, FGC-Misc, MC-Misc", Classification = "Class 3", InfractionCode = "B380", ViolationDescription = "Miscellaneous violations.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "RS 6-1", Classification = "Class 1", InfractionCode = "B135", ViolationDescription = "Failure to file affidavits and / or comply with other requirements set forth for photoluminescent exit path marking.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = true, DefaultPenalty = 12500, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 12500, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR 11-62", Classification = "Class 2", InfractionCode = "B2B1", ViolationDescription = "Violation of discretionary Zoning conditions on privately owned public space.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 10000, AggravatedPenalty_I = 10000, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR 42-543", Classification = "Class 1", InfractionCode = "B170", ViolationDescription = "Outdoor Advertising Company sign in M District exceeds height limit.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR 22-00", Classification = "Class 2", InfractionCode = "B205", ViolationDescription = "Illegal use in residential district.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 12500, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR 22-00", Classification = "Class 3", InfractionCode = "B385", ViolationDescription = "Illegal use in residential district.", Cure = true, Stipulation = true, StandardPenalty = 500, MitigatedPenalty = false, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR 22-32", Classification = "Class 1", InfractionCode = "B164", ViolationDescription = "Outdoor Advertising Company has impermissible advertising sign in an R District.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR 22-342", Classification = "Class 1", InfractionCode = "B168", ViolationDescription = "Outdoor Advertising Company sign in R District exceeds height limits.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR 25-41", Classification = "Class 2", InfractionCode = "B283", ViolationDescription = "Violation of parking regulations in a residential district.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR 25-41", Classification = "Class 3", InfractionCode = "B383", ViolationDescription = "Violation of parking regulations in a residential district.", Cure = true, Stipulation = false, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR 32-00", Classification = "Class 2", InfractionCode = "B247", ViolationDescription = "Illegal use in a commercial district.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR 32-63", Classification = "Class 1", InfractionCode = "B165", ViolationDescription = "Outdoor Advertising Company advertising sign not permitted in specified C District.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR 32-64", Classification = "Class 2", InfractionCode = "B276", ViolationDescription = "Sign(s) in specified C District exceed(s) surface area restrictions.", Cure = false, Stipulation = true, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR 32-64", Classification = "Class 1", InfractionCode = "B166", ViolationDescription = "Outdoor Advertising Company sign(s) in specified C Districts exceed surface area limits.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR 32-652", Classification = "Class 2", InfractionCode = "B277", ViolationDescription = "Sign in specified C District extends beyond street line limitation.", Cure = false, Stipulation = true, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR 32-653", Classification = "Class 2", InfractionCode = "B275", ViolationDescription = "Prohibited sign on awning, canopy, or marquee in C District.", Cure = false, Stipulation = true, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR 32-655", Classification = "Class 1", InfractionCode = "B169", ViolationDescription = "Outdoor Advertising Company sign exceeds permitted height for specified C District.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR 42-00", Classification = "Class 2", InfractionCode = "B248", ViolationDescription = "Illegal use in a manufacturing district.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR 42-52", Classification = "Class 1", InfractionCode = "B171", ViolationDescription = "Outdoor Advertising Sign not permitted in M District.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR 42-53", Classification = "Class 1", InfractionCode = "B167", ViolationDescription = "Outdoor Advertising sign in M District exceeds surface area limits.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR 105-20", Classification = "Class 2", InfractionCode = "B2G2", ViolationDescription = "Damaged or removed a tree within a Special Natural Area District without certification, authorization or special permit.", Cure = false, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = false, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR-Misc.", Classification = "Class 2", InfractionCode = "B284", ViolationDescription = "Miscellaneous violations of the Zoning Resolution.", Cure = true, Stipulation = false, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR-Misc.", Classification = "Class 3", InfractionCode = "B384", ViolationDescription = "Miscellaneous violations of the Zoning Resolution.", Cure = true, Stipulation = false, StandardPenalty = 500, MitigatedPenalty = true, DefaultPenalty = 500, AggravatedPenalty_I = 500, AggravatedDefaultPenalty_I = 500, AggravatedPenalty_II = 500, AggravatedDefaultMaxPenalty_II = 500 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR-Misc.", Classification = "Class 1", InfractionCode = "B178", ViolationDescription = "Misc sign violation under the Zoning Resolution by an Outdoor Advertising Company.", Cure = false, Stipulation = true, StandardPenalty = 10000, MitigatedPenalty = true, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "ZR-Misc.", Classification = "Class 2", InfractionCode = "B281", ViolationDescription = "Misc sign violation under the Zoning Resolution.", Cure = false, Stipulation = true, StandardPenalty = 1250, MitigatedPenalty = true, DefaultPenalty = 6250, AggravatedPenalty_I = 3125, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 6250, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "Misc. – ZR Misc.– Title 28", Classification = "Class 1", InfractionCode = "B1B6", ViolationDescription = "Misc outdoor sign violation of ZR and / or Building Code.", Cure = false, Stipulation = false, StandardPenalty = 10000, MitigatedPenalty = false, DefaultPenalty = 25000, AggravatedPenalty_I = 25000, AggravatedDefaultPenalty_I = 25000, AggravatedPenalty_II = 25000, AggravatedDefaultMaxPenalty_II = 25000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "Misc. – ZR Misc.– Title 28", Classification = "Class 2", InfractionCode = "B2A2", ViolationDescription = "Misc outdoor sign violation of ZR and / or Building Code.", Cure = false, Stipulation = false, StandardPenalty = 2500, MitigatedPenalty = false, DefaultPenalty = 10000, AggravatedPenalty_I = 6250, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.DOBPenaltySchedules.AddOrUpdate(r => r.InfractionCode, new DOBPenaltySchedule { SectionOfLaw = "Misc. ZR", Classification = "Class 2", InfractionCode = "B2B2", ViolationDescription = "Misc. violation of condition on as of right privately owned public space.", Cure = false, Stipulation = false, StandardPenalty = 5000, MitigatedPenalty = false, DefaultPenalty = 10000, AggravatedPenalty_I = 10000, AggravatedDefaultPenalty_I = 10000, AggravatedPenalty_II = 10000, AggravatedDefaultMaxPenalty_II = 10000 });
            //    context.SaveChanges();
            //}

            //if (!context.DEPNoiseCodePenaltySchedules.Any())
            //{
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "10-108", ViolationDescription = "Noise from sound device exceeding permit levels", Compliance = "Reduce noise to conform to permit forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "4th", Penalty_1 = 250, Penalty_2 = 500, Penalty_3 = 750, Penalty_4 = 1000, DefaultPenalty_1 = 250, DefaultPenalty_2 = 500, DefaultPenalty_3 = 750, DefaultPenalty_4 = 1000, Stipulation_1 = false, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-100", ViolationDescription = "Failed To Conspicuously Post an Accurate and Complete Construction Noise Mitigation Plan (CNMP)", Compliance = "Post complete Plan forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 220, Penalty_2 = 440, Penalty_3 = 660, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-101(a)", ViolationDescription = "Failed to Exercise Noise Mitigation Option Within 5 Business Days of Construction Tool Exceeding Noise Standard", Compliance = "Replace tool with equipment that complies with noise standard forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-101(a)", ViolationDescription = "Failed To Self-Certify Equipment Maintenance in CNMP", Compliance = "Certify equipment forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 220, Penalty_2 = 440, Penalty_3 = 660, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-101(b)", ViolationDescription = "Failed to Equip Construction Equipment with Noise Reduction Device", Compliance = "Equip construction equipment with noise reduction device", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-101(c)", ViolationDescription = "Failed to Mitigate Noise From Internal Combustion Engine", Compliance = "Mitigate noise from internal combustion engine forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-101(d)", ViolationDescription = "Failed to Cover Compressors/Generat ors/Pumps with Noise-Insulating Fabric", Compliance = "Cover compressor with noise insulating fabric forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-101(g)", ViolationDescription = "Failed to Use Perimeter Noise Barriers", Compliance = "Use noise perimeter barriers forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-101(h)", ViolationDescription = "Failed to Create and Utilize Noise Mitigation Training Program", Compliance = "Create and utilize training program forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 220, Penalty_2 = 440, Penalty_3 = 660, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-101(i)", ViolationDescription = "Failed to Coordinate Work Schedule with Sensitive Facility Receptor", Compliance = "Coordinate work schedule forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 220, Penalty_2 = 440, Penalty_3 = 660, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-101(j)", ViolationDescription = "Failure to Comply with CNMP or File Alternative Plan w/in 3 Days of Inspection", Compliance = "Comply with CNMP or file Alternative Plan forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-101(l)", ViolationDescription = "Failed to Utilize Temporary Barrier", Compliance = "Use temporary barrier forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-101(m)", ViolationDescription = "Failed to Utilize Noise Barrier on Sandblasting Perimeter Barrier", Compliance = "Use noise barrier forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (a)(1)(B)(ii)", ViolationDescription = "Failed to Use Specified Pile Driver When Working w /in 100 ft.of Receptor", Compliance = "Use specified pile driver forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (a)(1)(B)(iii)", ViolationDescription = "Failed to Equip Pile Driver w / Exhaust Muffler", Compliance = "Equip pile driver with muffler forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (a)(1)(B)(v)", ViolationDescription = "Failed to Pre - Augur / Pre - Trench Pile Holes", Compliance = "Pre - augur / pretrench holes forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (a)(1)(B)(vi)", ViolationDescription = "Failed to Properly Secure Impact Cushions to Piles", Compliance = "Secure cushions to piles forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (a)(1)(C)(i - iv)", ViolationDescription = "Failed to Properly Construct Portable Noise Barrier", Compliance = "Construct portable noise barrier forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (a)(2)(B)(iii)", ViolationDescription = "Failed to Equip Jackhammer With Effective Muffler", Compliance = "Equip jackhammer with muffler forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (a)(2)(C)(i)", ViolationDescription = "Failed to Properly Construct Portable Noise Barrier", Compliance = "Properly construct portable barrier forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (a)(2)(C)(ii)", ViolationDescription = "Exceeded Maximum Height of 15 Feet for Free - Standing Barrier", Compliance = "Reduce barrier height to 15 Feet forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (a)(2)(C)(iii)(b)", ViolationDescription = "Failed to Use Multiple Tents for Multiple Jackhammers", Compliance = "Use multiple tents forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (a)(2)(C)(iii)(d)", ViolationDescription = "Failed to Use Double Layer of Mitigation During Emergency Jackhammering w /in 500 ft.of Residential Receptor", Compliance = "Use double layer of mitigation forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (a)(2)(C)(iii)©", ViolationDescription = "Failed to Move Noise Tents as Jackhammer Work Progresses", Compliance = "Move tents forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (a)(2)(C)(iii)€", ViolationDescription = "Failed to Use Tents on Either Side of Jackhammer Where Surrounded by Receptors", Compliance = "Use tents forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (a)(3)(B)(iii)", ViolationDescription = "Failed to Wrap Noise Shroud Around Head of Hoe Ram w /in 200 ft.of Receptor", Compliance = "Use wrap shroud forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (a)(3)(C)(i - iv)", ViolationDescription = "Failed to Properly Construct Portable Noise Barrier", Compliance = "Construct proper noise barrier forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (a)(4)(C)(i)", ViolationDescription = "Failed to Lay Heavy Rubber Blast Mats Over Blast Site", Compliance = "Lay heavy mats forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (a)(4)(C)(ii - iii)", ViolationDescription = "Failed to Properly Construct Portable Noise Barrier", Compliance = "Construct proper noise barrier forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (b)(1)(B)(iv)", ViolationDescription = "Failed to Cover Vac - Truck’s Suction Component w / Noise - Reducing Enclosure", Compliance = "Cover vac truck with noise reducing enclosure forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (b)(1)(C)(i - iv)", ViolationDescription = "Failed to Properly Construct Portable Noise Barrier", Compliance = "Construct proper noise barrier forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (c)(1)(B)(ii)", ViolationDescription = "Failed to Install Bed Liner", Compliance = "Install bed liner forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (c)(1)(B)(v)", ViolationDescription = "Failed to Equip Truck with Effective Muffler", Compliance = "Equip truck with effective muffler forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (c)(1)(B)(vii)", ViolationDescription = "Failed to Keep Housing Doors Closed During Engine Operation", Compliance = "Close housing doors during engine operation forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (c)(1)(C)(i - iii)", ViolationDescription = "Failed to Properly Construct Portable Noise Barrier", Compliance = "Construct proper noise barrier forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (d)(1)(B)(v)", ViolationDescription = "Failed to Equip Crane with Effective Muffler", Compliance = "Equip crane with effective muffler forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (d)(2)(B)(i)", ViolationDescription = "Failed to Equip Auger Drill Rig with Effective Muffler", Compliance = "Equip auger drill with effective muffler forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (d)(2)(B)(ii)", ViolationDescription = "Failed to Lubricate All Moving Parts of Auger Drill Rig", Compliance = "Lubricate all moving parts of auger drill forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (d)(2)(B)(iii)", ViolationDescription = "Failed to Properly Clear Debris From Drill Bits", Compliance = "Clean all debris from drill bits forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (d)(2)(C)(i - iv)", ViolationDescription = "Failed to Properly Construct Portable Noise Barriers", Compliance = "Construct proper noise barrier forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (d)(3)(A)(i)", ViolationDescription = "Failed to Properly Install Street Plates", Compliance = "Properly install street plates forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (d)(4)(A)(i)(a - c)", ViolationDescription = "Failed to Equip Work Vehicles with Proper Backup Alarms", Compliance = "Equip work vehicles with proper backup alarm forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (d)(4)(B)(ii - iv)", ViolationDescription = "Failed to Properly Construct Portable Noise Barriers", Compliance = "Construct proper noise barrier forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (e)(1)(C)(i - iii)", ViolationDescription = "Failed to Properly Construct Portable Noise Barriers", Compliance = "Construct proper noise barrier forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (e)(1)(C)(iv)", ViolationDescription = "Failed to Use Multiple Tents During Use of Multiple Concrete Saws", Compliance = "Use multiple tents forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (e)(1)(C)(v)", ViolationDescription = "Failed to Move Noise Tent As Concrete Saw Work Progressed", Compliance = "Move tents forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (e)(1)(C)(vi)", ViolationDescription = "Failed to Use Double Layer of Mitigation for Noise Barrier During Emergency Concrete Sawing within 500 ft.of Residential Receptor", Compliance = "Use double layer of mitigation forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-102 (e)(1)(C)(vii)", ViolationDescription = "Failed to Use Two Tents on Either Side of Saw When Surrounded by Receptors", Compliance = "Use two tents on either side of saw forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-104", ViolationDescription = "Failed to File Alternative Noise Mitigation Plan (ANMP)", Compliance = "File ANMP forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 220, Penalty_2 = 440, Penalty_3 = 660, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-105", ViolationDescription = "Failed to Conspicuously Post Utility Noise Mitigation Plan (UNMP)", Compliance = "Conspicuously post UNMP forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 220, Penalty_2 = 440, Penalty_3 = 660, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-106(a)", ViolationDescription = "Failed to Exercise Noise Mitigation Option within 5 Days of Construction Tool Exceeding Noise Standard", Compliance = "Replace tool with equipment that complies with noise standard forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-106(a)", ViolationDescription = "Failed to Self-Certify Equipment Maintenance in UNMP", Compliance = "Certify equipment forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 220, Penalty_2 = 440, Penalty_3 = 660, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-106(b)", ViolationDescription = "Failed to Equip Construction Tool with Noise Reduction Device", Compliance = "Equip tool with noise reduction device", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-106(c)", ViolationDescription = "Failed to Comply with Additional Noise Mitigation Measures for Specialized Vehicles", Compliance = "Use additional noise mitigation measures for specialized trucks forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-106(d)", ViolationDescription = "Failed to Cover Equipment with Noise-Insulating Fabric", Compliance = "Cover equipment with noise insulating fabric forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-106(h)", ViolationDescription = "Failed to Properly Install and Secure Street Plates", Compliance = "Properly install street plates forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-106(i)", ViolationDescription = "Failed to Notify Residents within 200 Feet of Construction Activity", Compliance = "Notify residents forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 220, Penalty_2 = 440, Penalty_3 = 660, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-106(j)", ViolationDescription = "Failed to Respond to Noise Complaints/Notice from DEP", Compliance = "Respond to notice forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 220, Penalty_2 = 440, Penalty_3 = 660, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-106(l)", ViolationDescription = "Failed to Create and Utilize Noise Mitigation Training Program", Compliance = "Create and utilize training program forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 220, Penalty_2 = 440, Penalty_3 = 660, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-106(m)", ViolationDescription = "Failed to Coordinate Work Schedule with Sensitive Receptor Owner", Compliance = "Coordinate work schedule forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 220, Penalty_2 = 440, Penalty_3 = 660, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "15 RCNY 28-106(n)", ViolationDescription = "Failed to Correct Excessive Noise Condition/File ANMP", Compliance = "Correct excessive noise and/or file ANMP forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-206(c)", ViolationDescription = "Failed to Comply with Commissioner's Order to Provide Access for Testing", Compliance = "Allow access forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 220, Penalty_2 = 440, Penalty_3 = 660, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = false, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-207(c)", ViolationDescription = "Refused DEP entry into public area(s) of premises", Compliance = "Allow entry forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 350, Penalty_2 = 700, Penalty_3 = 1050, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-207(d)", ViolationDescription = "Refusal to allow authorized employee to perform sound testing", Compliance = "Allow sound testing forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 350, Penalty_2 = 700, Penalty_3 = 1050, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-208", ViolationDescription = "Operating equipment without a valid registration", Compliance = "Obtain registration forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 350, Penalty_2 = 700, Penalty_3 = 1050, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-209", ViolationDescription = "Interfering w/or obstructing DEP Personnel", Compliance = "Cease interference with adm. personnel - forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 350, Penalty_2 = 700, Penalty_3 = 1050, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = true, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-210", ViolationDescription = "False/misleading statements: unlawful repro/alteration of documents", Compliance = "Submit documented information - forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 350, Penalty_2 = 700, Penalty_3 = 1050, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = false, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-211", ViolationDescription = "Failure to post certificate or tunneling permit", Compliance = "Post certificate forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 350, Penalty_2 = 700, Penalty_3 = 1050, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = true, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-216(d)", ViolationDescription = "Failure to comply with noise abatement contract requirements", Compliance = "Comply with abatement requirements forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 650, Penalty_2 = 1300, Penalty_3 = 1950, Penalty_4 = 0, DefaultPenalty_1 = 2625, DefaultPenalty_2 = 5250, DefaultPenalty_3 = 7875, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-218(a)", ViolationDescription = "Causing or permitting unreasonable noise (10PM to 7AM)", Compliance = "Cease unreasonable noise forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 75, Penalty_2 = 150, Penalty_3 = 350, Penalty_4 = 0, DefaultPenalty_1 = 150, DefaultPenalty_2 = 250, DefaultPenalty_3 = 500, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = true, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-218(a)", ViolationDescription = "Causing or permitting unreasonable noise (7AM to 10PM)", Compliance = "Cease unreasonable noise forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 75, Penalty_2 = 150, Penalty_3 = 350, Penalty_4 = 0, DefaultPenalty_1 = 100, DefaultPenalty_2 = 225, DefaultPenalty_3 = 500, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = true, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-218(a-1)", ViolationDescription = "Causing or permitting unreasonable noise for commercial activities or purposes or through a device installed within or upon a building", Compliance = "Cease unreasonable noise forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 350, Penalty_2 = 700, Penalty_3 = 1050, Penalty_4 = 0, DefaultPenalty_1 = 1000, DefaultPenalty_2 = 2000, DefaultPenalty_3 = 3000, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = true, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-218(e)", ViolationDescription = "Failure to comply with Commissioner’s Order or mitigation measures re noise from refuse collection facility", Compliance = "Comply with Commissioner's Order forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 350, Penalty_2 = 700, Penalty_3 = 1050, Penalty_4 = 0, DefaultPenalty_1 = 1000, DefaultPenalty_2 = 2000, DefaultPenalty_3 = 3000, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = true, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-218.1", ViolationDescription = "Use of mobile telephones in a place of public performance", Compliance = "", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 50, Penalty_2 = 50, Penalty_3 = 50, Penalty_4 = 0, DefaultPenalty_1 = 50, DefaultPenalty_2 = 50, DefaultPenalty_3 = 50, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = true, Stipulation_3 = true, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-220(a)", ViolationDescription = "Failure to adopt/implement Noise Mitigation Plan for construction site", Compliance = "Adopt Mitigation Plan forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 875, Penalty_2 = 1750, Penalty_3 = 2625, Penalty_4 = 0, DefaultPenalty_1 = 1400, DefaultPenalty_2 = 2800, DefaultPenalty_3 = 4200, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-220(b)", ViolationDescription = "Failure to ensure that all construction workers are familiar with noise mitigation plan", Compliance = "Inform workers about noise mitigation plan requirements forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 1400, DefaultPenalty_2 = 2800, DefaultPenalty_3 = 4200, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-220(b)", ViolationDescription = "Inadequate/insufficien tly detailed noise mitigation plan", Compliance = "Provide additional information to Noise Plan forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 1400, DefaultPenalty_2 = 2800, DefaultPenalty_3 = 4200, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-220(c)", ViolationDescription = "Failure to keep/have available for inspection copy of noise mitigation plan", Compliance = "Make plan available forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 1400, DefaultPenalty_2 = 2800, DefaultPenalty_3 = 4200, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-220(d)", ViolationDescription = "Failure to amend noise mitigation plan", Compliance = "Amend noise mitigation plan forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 1400, DefaultPenalty_2 = 2800, DefaultPenalty_3 = 4200, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-220(e)", ViolationDescription = "Failure to file noise mitigation plan when required", Compliance = "File noise mitigation plan forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 400, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 1400, DefaultPenalty_2 = 2800, DefaultPenalty_3 = 4200, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-222", ViolationDescription = "Construction activities at impermissible times/days", Compliance = "Cease construction activities at impermissible times/days forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 1400, Penalty_2 = 2800, Penalty_3 = 4200, Penalty_4 = 0, DefaultPenalty_1 = 3500, DefaultPenalty_2 = 7000, DefaultPenalty_3 = 10500, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-223(b)", ViolationDescription = "Failure to submit certification for emergency work within 3 days of starting work", Compliance = "Submit certification forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 875, Penalty_2 = 1750, Penalty_3 = 2625, Penalty_4 = 0, DefaultPenalty_1 = 3500, DefaultPenalty_2 = 7000, DefaultPenalty_3 = 10500, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-223(d)", ViolationDescription = "Failure to respond to request for conference or to amend noise mitigation plan", Compliance = "Respond to request forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 875, Penalty_2 = 1750, Penalty_3 = 2625, Penalty_4 = 0, DefaultPenalty_1 = 3500, DefaultPenalty_2 = 7000, DefaultPenalty_3 = 10500, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-224", ViolationDescription = "Construction work not in compliance with noise mitigation plan", Compliance = "Stop construction work forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 1400, Penalty_2 = 2800, Penalty_3 = 4200, Penalty_4 = 0, DefaultPenalty_1 = 3500, DefaultPenalty_2 = 7000, DefaultPenalty_3 = 10500, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-225(a)", ViolationDescription = "Sell/offer/operate/per mit operation of refuse compacting vehicle producing excessive noise", Compliance = "Stop operation of compacting vehicle forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 560, Penalty_2 = 1120, Penalty_3 = 1680, Penalty_4 = 0, DefaultPenalty_1 = 1400, DefaultPenalty_2 = 2800, DefaultPenalty_3 = 4200, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-225(b)", ViolationDescription = "Operate/cause operation of refuse compacting vehicle producing excessive noise (11PM to 7AM)", Compliance = "Stop operation of compacting vehicle forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 700, Penalty_2 = 1400, Penalty_3 = 2100, Penalty_4 = 0, DefaultPenalty_1 = 1400, DefaultPenalty_2 = 2800, DefaultPenalty_3 = 4200, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-226(a)", ViolationDescription = "Operating air compressor without appropriate muffler w/no exhaust leaks", Compliance = "Stop operation of air compressor forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 560, Penalty_2 = 1120, Penalty_3 = 1680, Penalty_4 = 0, DefaultPenalty_1 = 1400, DefaultPenalty_2 = 2800, DefaultPenalty_3 = 4200, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-226(b)", ViolationDescription = "Excessive noise from air compressor (measured @ 1 meter)", Compliance = "Stop operation of air compressor forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 560, Penalty_2 = 1120, Penalty_3 = 1680, Penalty_4 = 0, DefaultPenalty_1 = 1400, DefaultPenalty_2 = 2800, DefaultPenalty_3 = 4200, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-226(c)", ViolationDescription = "Excessive noise from air compressor (measured @ receiving property)", Compliance = "Stop operation of air compressor forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 560, Penalty_2 = 1120, Penalty_3 = 1680, Penalty_4 = 0, DefaultPenalty_1 = 1400, DefaultPenalty_2 = 2800, DefaultPenalty_3 = 4200, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-227(a)", ViolationDescription = "Noise from circulation device in excess of 42 dB(A)", Compliance = "Stop operation of circulation device forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 0, Penalty_2 = 1120, Penalty_3 = 1680, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-227(b)", ViolationDescription = "Cumulative impact from circulation device exceeded 45 dB(A)", Compliance = "Stop operation of circulation device forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 0, Penalty_2 = 1120, Penalty_3 = 1680, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-227(c)", ViolationDescription = "Failure to reduce cumulative impact from multiple circulation devices exceeding 50 dB(A)", Compliance = "Reduce cumulative impact noise from circulation device forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 0, Penalty_2 = 1120, Penalty_3 = 1680, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-228", ViolationDescription = "Unreasonable noise from construction devices", Compliance = "Stop operation of construction device forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 560, Penalty_2 = 1120, Penalty_3 = 1680, Penalty_4 = 0, DefaultPenalty_1 = 1400, DefaultPenalty_2 = 2800, DefaultPenalty_3 = 4200, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-228.1", ViolationDescription = "Unreasonable noise from engine exhaust", Compliance = "Stop operation of engine exhaust forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 560, Penalty_2 = 1120, Penalty_3 = 1680, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-229", ViolationDescription = "Unreasonable noise from handling/transporting of container or construction material", Compliance = "Stop handling and transportation of construction material forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 560, Penalty_2 = 1120, Penalty_3 = 1680, Penalty_4 = 0, DefaultPenalty_1 = 1400, DefaultPenalty_2 = 2800, DefaultPenalty_3 = 4200, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-230(a)", ViolationDescription = "Operation/caused operation of paving breaker w/o pneumatic discharge muffler", Compliance = "Use paving breaker with pneumatic discharge muffler forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 560, Penalty_2 = 1120, Penalty_3 = 1680, Penalty_4 = 0, DefaultPenalty_1 = 1400, DefaultPenalty_2 = 2800, DefaultPenalty_3 = 4200, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-230(b)", ViolationDescription = "Sold/offer for sale/operate/permit operation of paving breaker producing over 95 dB(A)", Compliance = "Stop operation of paving breaker forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 560, Penalty_2 = 1120, Penalty_3 = 1680, Penalty_4 = 0, DefaultPenalty_1 = 1400, DefaultPenalty_2 = 2800, DefaultPenalty_3 = 4200, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-231(a)*", ViolationDescription = "Made/caused/permitte d music from commercial establishment in excess of permitted levels", Compliance = "Cease operation of commercial music forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 3200, Penalty_2 = 6400, Penalty_3 = 9600, Penalty_4 = 0, DefaultPenalty_1 = 8000, DefaultPenalty_2 = 16000, DefaultPenalty_3 = 24000, DefaultPenalty_4 = 0, Stipulation_1 = false, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-231(d)", ViolationDescription = "Violation of variance from limits set forth in 24-231(a)", Compliance = "Cease operation of commercial music forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 560, Penalty_2 = 1120, Penalty_3 = 1680, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = false, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-232(a)", ViolationDescription = "Excessive noise from sound source @ commercial or business establishment", Compliance = "Stop operation of sound source forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 560, Penalty_2 = 1120, Penalty_3 = 1680, Penalty_4 = 0, DefaultPenalty_1 = 1400, DefaultPenalty_2 = 2800, DefaultPenalty_3 = 4200, DefaultPenalty_4 = 0, Stipulation_1 = false, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-233(a)", ViolationDescription = "Unreasonable noise: personal audio device", Compliance = "Stop operation of personal audio device forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 70, Penalty_2 = 140, Penalty_3 = 210, Penalty_4 = 0, DefaultPenalty_1 = 175, DefaultPenalty_2 = 350, DefaultPenalty_3 = 525, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = true, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-233(b)(1)", ViolationDescription = "Unreasonable noise – personal audio device (public right-of-way)", Compliance = "Stop operation of personal audio device forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 140, Penalty_2 = 280, Penalty_3 = 420, Penalty_4 = 0, DefaultPenalty_1 = 175, DefaultPenalty_2 = 350, DefaultPenalty_3 = 525, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = true, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-233(b)(2)", ViolationDescription = "Unreasonable noise – personal audio device (motor vehicle)", Compliance = "Stop operation of personal audio device forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 140, Penalty_2 = 280, Penalty_3 = 420, Penalty_4 = 0, DefaultPenalty_1 = 350, DefaultPenalty_2 = 700, DefaultPenalty_3 = 1050, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-234", ViolationDescription = "Excess noise from sound reproduction device on rapid transit (subway, bus, ferry)", Compliance = "Stop operation of sound reproduction device forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 70, Penalty_2 = 140, Penalty_3 = 210, Penalty_4 = 0, DefaultPenalty_1 = 175, DefaultPenalty_2 = 350, DefaultPenalty_3 = 525, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = true, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-235", ViolationDescription = "Permitting animal to cause unreasonable noise", Compliance = "Cease permitting animal to cause unreasonable noise forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 70, Penalty_2 = 140, Penalty_3 = 210, Penalty_4 = 0, DefaultPenalty_1 = 175, DefaultPenalty_2 = 350, DefaultPenalty_3 = 525, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = true, Stipulation_3 = true, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-236(a)", ViolationDescription = "Excess noise from motor vehicles (10,000 lbs. or less)", Compliance = "Stop operating vehicle that causes excessive noise forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 210, Penalty_2 = 420, Penalty_3 = 840, Penalty_4 = 0, DefaultPenalty_1 = 525, DefaultPenalty_2 = 1050, DefaultPenalty_3 = 1575, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-236(b)", ViolationDescription = "Excess noise from motorcycles", Compliance = "Stop operating motorcycle that causes excessive noise forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 880, Penalty_2 = 1720, Penalty_3 = 2600, Penalty_4 = 0, DefaultPenalty_1 = 1440, DefaultPenalty_2 = 2800, DefaultPenalty_3 = 4200, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-236(c)", ViolationDescription = "Excess noise from motor vehicles (10,000 lbs. or greater)", Compliance = "Stop operating vehicle that causes excessive noise forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 880, Penalty_2 = 1720, Penalty_3 = 2600, Penalty_4 = 0, DefaultPenalty_1 = 1440, DefaultPenalty_2 = 2800, DefaultPenalty_3 = 4200, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-236(d)", ViolationDescription = "Non-emergency use of compression brake system", Compliance = "Stop using compression brake for non-emergencies forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 880, Penalty_2 = 1720, Penalty_3 = 2600, Penalty_4 = 0, DefaultPenalty_1 = 1440, DefaultPenalty_2 = 2800, DefaultPenalty_3 = 4200, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-237(a)", ViolationDescription = "Unauthorized use of motor vehicle claxon", Compliance = "Cease use of claxon forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 350, Penalty_2 = 700, Penalty_3 = 1050, Penalty_4 = 0, DefaultPenalty_1 = 1000, DefaultPenalty_2 = 2000, DefaultPenalty_3 = 3000, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = true, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-237(b)", ViolationDescription = "Unauthorized use of motor vehicle air horn/gong", Compliance = "Cease use of air horn forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 350, Penalty_2 = 700, Penalty_3 = 1050, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = true, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-237(c)", ViolationDescription = "Unauthorized use of steam whistle", Compliance = "Cease use of steam whistle forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 350, Penalty_2 = 700, Penalty_3 = 1050, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = true, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-237(d)", ViolationDescription = "Improper use of sound signal device (food vendor)", Compliance = "Cease use of sound signal device forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 350, Penalty_2 = 700, Penalty_3 = 1050, Penalty_4 = 0, DefaultPenalty_1 = 1000, DefaultPenalty_2 = 2000, DefaultPenalty_3 = 3000, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = true, Stipulation_3 = true, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-238(a)", ViolationDescription = "Improper audible burglar alarm/no automatic termination", Compliance = "Cease use of burglar alarm forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 280, Penalty_2 = 560, Penalty_3 = 840, Penalty_4 = 0, DefaultPenalty_1 = 700, DefaultPenalty_2 = 1400, DefaultPenalty_3 = 2100, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = true, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-238(b)", ViolationDescription = "Audible status indicator on motor vehicle in operation", Compliance = "Cease use of status indicator forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 280, Penalty_2 = 560, Penalty_3 = 840, Penalty_4 = 0, DefaultPenalty_1 = 700, DefaultPenalty_2 = 1400, DefaultPenalty_3 = 2100, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = true, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-239(b)", ViolationDescription = "Vehicle owner failure to display owner’s local precinct number", Compliance = "Display local precinct number forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 100, Penalty_2 = 200, Penalty_3 = 300, Penalty_4 = 0, DefaultPenalty_1 = 350, DefaultPenalty_2 = 700, DefaultPenalty_3 = 1050, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = true, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-241(a)", ViolationDescription = "Unauthorized use of emergency signal device", Compliance = "Cease use of signal device", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 1400, DefaultPenalty_2 = 2800, DefaultPenalty_3 = 4200, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-241(b)", ViolationDescription = "Failed to file certification regarding test of emergency signal device", Compliance = "File certification forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 1400, DefaultPenalty_2 = 2800, DefaultPenalty_3 = 4200, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-242(a)", ViolationDescription = "Operating lawn care devices at unauthorized times or so as to create unreasonable noise", Compliance = "Stop operation of lawn care device forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 220, Penalty_2 = 440, Penalty_3 = 660, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = true, Stipulation_3 = true, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-242(b)", ViolationDescription = "Operation of leaf blower without muffler", Compliance = "Stop operation of lawn care device forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 220, Penalty_2 = 440, Penalty_3 = 660, Penalty_4 = 0, DefaultPenalty_1 = 875, DefaultPenalty_2 = 1750, DefaultPenalty_3 = 2625, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = true, Stipulation_3 = true, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-244(a)", ViolationDescription = "Unreasonable noise from sound reproduction device", Compliance = "Cease operation of sound reproduction device forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 440, Penalty_2 = 880, Penalty_3 = 1320, Penalty_4 = 0, DefaultPenalty_1 = 1750, DefaultPenalty_2 = 3500, DefaultPenalty_3 = 5250, DefaultPenalty_4 = 0, Stipulation_1 = true, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-244(b)", ViolationDescription = "Unreasonable noise from sound reproduction device for commercial/bus. advert. Purposes", Compliance = "Cease operation of sound reproduction device forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 700, Penalty_2 = 1400, Penalty_3 = 2100, Penalty_4 = 0, DefaultPenalty_1 = 1750, DefaultPenalty_2 = 3500, DefaultPenalty_3 = 5250, DefaultPenalty_4 = 0, Stipulation_1 = false, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-245", ViolationDescription = "Failure to have operating certificate or tunneling permit", Compliance = "Obtain permit forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 1050, Penalty_2 = 2100, Penalty_3 = 3150, Penalty_4 = 0, DefaultPenalty_1 = 2625, DefaultPenalty_2 = 5250, DefaultPenalty_3 = 7875, DefaultPenalty_4 = 0, Stipulation_1 = false, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.DEPNoiseCodePenaltySchedules.Add(new DEPNoiseCodePenaltySchedule { SectionOfLaw = "24-257(b)(7)", ViolationDescription = "Breaking of Board ordered seal", Compliance = "Cease tampering with seal - forthwith", Offense_1 = "1st", Offense_2 = "2nd", Offense_3 = "3rd", Offense_4 = "", Penalty_1 = 1600, Penalty_2 = 1600, Penalty_3 = 2480, Penalty_4 = 0, DefaultPenalty_1 = 4000, DefaultPenalty_2 = 4000, DefaultPenalty_3 = 4000, DefaultPenalty_4 = 0, Stipulation_1 = false, Stipulation_2 = false, Stipulation_3 = false, Stipulation_4 = false });
            //    context.SaveChanges();
            //}

            if (!context.Boroughes.Any())
            {

                context.Boroughes.AddOrUpdate(
                r => r.Description,
                new Borough { County = "New York", Description = "Manhattan", BisCode = 1 },
                new Borough { County = "Bronx", Description = "Bronx", BisCode = 2 },
                new Borough { County = "Kings", Description = "Brooklyn", BisCode = 3 },
                new Borough { County = "Queens", Description = "Queens", BisCode = 4 },
                new Borough { County = "Richmond", Description = "Staten Island", BisCode = 5 });

                context.SaveChanges();
            }

            if (!context.JobContactTypes.Any())
            {
                context.JobContactTypes.AddOrUpdate(
                r => r.Name,
                new JobContactType { Name = "Architect" },
                new JobContactType { Name = "Asbestos Inspector" },
                new JobContactType { Name = "Assist. Project Manager" },
                new JobContactType { Name = "Attorney" },
                new JobContactType { Name = "Boiler Installer" },
                new JobContactType { Name = "Borings Engineer" },
                new JobContactType { Name = "BPP Architect" },
                new JobContactType { Name = "Bureau Commander" },
                new JobContactType { Name = "Carting Contractor" },
                new JobContactType { Name = "Chief Fire Protector" },
                new JobContactType { Name = "Concrete Producer" },
                new JobContactType { Name = "Concrete Safety Manager" },
                new JobContactType { Name = "Concrete Subcontractor" },
                new JobContactType { Name = "Concrete Testing Lab" },
                new JobContactType { Name = "Construction Manager" },
                new JobContactType { Name = "Construction Superintendent" },
                new JobContactType { Name = "Demoltion Subcontractor" },
                new JobContactType { Name = "DOT Contractor" },
                new JobContactType { Name = "Electrician/ Vendor" },
                new JobContactType { Name = "Engineer" },
                new JobContactType { Name = "Exterminator" },
                new JobContactType { Name = "Filing Representative" },
                new JobContactType { Name = "Fire Alarm Engineer" },
                new JobContactType { Name = "FO General Contractor" },
                new JobContactType { Name = "FO Site Safety Manager" },
                new JobContactType { Name = "FO Structural Engineer" },
                new JobContactType { Name = "General Contractor" },
                new JobContactType { Name = "Landmark Architect" },
                new JobContactType { Name = "Landmarks Rep" },
                new JobContactType { Name = "Landscape Applicant" },
                new JobContactType { Name = "Main Client Only" },
                new JobContactType { Name = "Mechanical Engineer" },
                new JobContactType { Name = "MEP Engineer" },
                new JobContactType { Name = "No Longer Retained" },
                new JobContactType { Name = "Other" },
                new JobContactType { Name = "Owner's Rep" },
                new JobContactType { Name = "Piles Engineer" },
                new JobContactType { Name = "Plumbing Contractor" },
                new JobContactType { Name = "Plumbing Engineer" },
                new JobContactType { Name = "Principal" },
                new JobContactType { Name = "Project Engineer" },
                new JobContactType { Name = "Project Manager" },
                new JobContactType { Name = "Property Manager" },
                new JobContactType { Name = "Resident Engineer" },
                new JobContactType { Name = "Secondary Client" },
                new JobContactType { Name = "Shoring Engineer" },
                new JobContactType { Name = "Sign Hanger" },
                new JobContactType { Name = "Sign-Off Engineer" },
                new JobContactType { Name = "Sign-Off MEP Engineer" },
                new JobContactType { Name = "Sign-Off ST Engineer" },
                new JobContactType { Name = "Site Safety Coordinator" },
                new JobContactType { Name = "Site Safety Manager" },
                new JobContactType { Name = "Structural Engineer" },
                new JobContactType { Name = "Sub-Contractor" },
                new JobContactType { Name = "Sub-Tenant" },
                new JobContactType { Name = "Superintendent of Construction" },
                new JobContactType { Name = "Surveyor" },
                new JobContactType { Name = "Tenant" },
                new JobContactType { Name = "Underpin/Shoring Engineer" });
                context.SaveChanges();
            }

            if (!context.TransmissionTypes.Any())
            {
                context.TransmissionTypes.AddOrUpdate(
                r => r.Name,
                new TransmissionType { Name = "E-Mail" },
                new TransmissionType { Name = "E-Mail & FEDEX Priority" },
                new TransmissionType { Name = "E-Mail & FEDEX Standard" },
                new TransmissionType { Name = "E-Mail & Messenger" },
                new TransmissionType { Name = "FEDEX Priority" },
                new TransmissionType { Name = "FEDEX Standard" },
                new TransmissionType { Name = "Fax" },
                new TransmissionType { Name = "Hand Delivered" },
                new TransmissionType { Name = "Mail" },
                new TransmissionType { Name = "Messenger" },
                new TransmissionType { Name = "Pickup" },
                new TransmissionType { Name = "Round Trip Messenger" },
                new TransmissionType { Name = "Rush Messenger" });
                context.SaveChanges();
            }
            if (!context.TaskStatuses.Any())
            {
                context.TaskStatuses.AddOrUpdate(
                r => r.Name,
                new TaskStatus { Name = "Pending", Id = 1 },
                //new TaskStatus { Name = "In Progress", Id = 2 },
                new TaskStatus { Name = "Completed", Id = 3 },
                new TaskStatus { Name = "Unattainable", Id = 4 });
                context.SaveChanges();
            }

            if (!context.RfpStatuses.Any())
            {
                context.RfpStatuses.AddOrUpdate(
                r => r.Name,
                new RfpStatus { Name = "In Draft", DisplayName = "In Draft", Id = 1 },
                new RfpStatus { Name = "Pending Review by RPO", DisplayName = "Pending Review by RPO", Id = 2 },
                new RfpStatus { Name = "Submitted to Client", DisplayName = "Submitted to Client", Id = 3 },
                new RfpStatus { Name = "Cancelled", DisplayName = "Cancelled", Id = 4 },
                new RfpStatus { Name = "Lost", DisplayName = "Lost", Id = 5 },
                new RfpStatus { Name = "On Hold", DisplayName = "On Hold", Id = 6 },
                new RfpStatus { Name = "Accepted by Client/Pending Retainer", DisplayName = "Accepted by Client/Pending Retainer", Id = 7 },
                new RfpStatus { Name = "Reviewed at RPO", DisplayName = "Reviewed at RPO", Id = 8 },
                new RfpStatus { Name = "Retained/Active", DisplayName = "Retained/Active", Id = 9 });

                context.SaveChanges();
            }
            //context.JobStatuses.AddOrUpdate(
            //    r => r.Name,
            //    new JobStatus { Name = "In Progress", Id = 1 },
            //    new JobStatus { Name = "On Hold", Id = 2 },
            //    new JobStatus { Name = "Completed", Id = 3 });
            //context.SaveChanges();

            if (!context.TaskTypes.Any())
            {
                context.TaskTypes.AddOrUpdate(
                r => r.Name,
                new TaskType { Name = "After Hour Permits" },
                new TaskType { Name = "Appointments" },
                new TaskType { Name = "c/o Copy" },
                new TaskType { Name = "Check Plumbing Sign Off" },
                new TaskType { Name = "Check Withdrawal" },
                new TaskType { Name = "File Job" },
                new TaskType { Name = "Get Bis Information" },
                new TaskType { Name = "Landmark Pickup" },
                new TaskType { Name = "Other" },
                new TaskType { Name = "Permit Renewal" },
                new TaskType { Name = "Post Approval Amendment" },
                new TaskType { Name = "Prepare Forms" },
                new TaskType { Name = "Pull Permits" },
                new TaskType { Name = "Review Jobs" },
                new TaskType { Name = "Sign Off" },
                new TaskType { Name = "Update Insurance" },
                new TaskType { Name = "Proposal Review" },
                new TaskType { Name = "Obtain COC for violation(s)" },
                new TaskType { Name = "Obtain DOT stipulations" });
                context.SaveChanges();
            }

            if (!context.EmailTypes.Any())
            {
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "After Hours Permit(s)" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "Amendment Approvals" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "Amendments" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "Approvals" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "Completion" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "Drawings only" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "For Use and Information" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "Memo" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "Miscellaneous" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "New Filing" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "Notice of Expiring Permits - DEP" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "Notice of Expiring Permits - DOB" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "Notice of Expiring Permits - DOT" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "Objection(s)" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "Objection(s) - 2nd Notice" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "Objection(s) - Final Notice" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "Permit Applications" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "Permit(s)" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "Permit(s) - DEP" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "Permit(s) - DOB" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "Permit(s) - DOT" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "Research" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "Sign-off" });
                context.EmailTypes.AddOrUpdate(ct => ct.Name, new EmailType { Name = "Violations" });
                context.SaveChanges();
            }

            if (!context.ApplicationStatus.Any())
            {
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id, DisplayName = "Approved", Name = "Approved" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id, DisplayName = "Assigned", Name = "Assigned" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id, DisplayName = "Audit", Name = "Audit" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id, DisplayName = "Cancelled", Name = "Cancelled" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id, DisplayName = "Completed", Name = "Completed" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id, DisplayName = "Disapproved", Name = "Disapproved" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id, DisplayName = "Filed", Name = "Filed" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id, DisplayName = "Folder Sent", Name = "Folder Sent" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id, DisplayName = "New", Name = "New" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id, DisplayName = "On Hold", Name = "On Hold" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id, DisplayName = "Paperwork", Name = "Paperwork" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id, DisplayName = "Partial Approval", Name = "Partial Approval" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id, DisplayName = "Partial Permitted", Name = "Partial Permitted" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id, DisplayName = "Permitted", Name = "Permitted" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id, DisplayName = "Pre-Filed", Name = "Pre-Filed" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id, DisplayName = "Ready for Sign Off", Name = "Ready for Sign Off" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id, DisplayName = "Revision", Name = "Revision" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id, DisplayName = "Suspended", Name = "Suspended" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOB").Id, DisplayName = "Withdrawn", Name = "Withdrawn" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOT").Id, DisplayName = "N", Name = "New" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOT").Id, DisplayName = "P", Name = "Permitted" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOT").Id, DisplayName = "U", Name = "Unattainable" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DOT").Id, DisplayName = "C", Name = "Completed" });

                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DEP").Id, DisplayName = "Pending", Name = "Pending" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DEP").Id, DisplayName = "Approved", Name = "Approved" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DEP").Id, DisplayName = "Permitted", Name = "Permitted" });
                context.ApplicationStatus.AddOrUpdate(ct => new { ct.Name, ct.IdJobApplicationType }, new ApplicationStatus { IdJobApplicationType = context.JobApplicationTypes.FirstOrDefault(g => g.Description == "DEP").Id, DisplayName = "Closed", Name = "Closed" });

                context.SaveChanges();
            }

            if (!context.OccupancyClassifications.Any())
            {
                context.OccupancyClassifications.AddOrUpdate(
                r => r.Description,
                new OccupancyClassification { Is_2008_2014 = true, Code = "A-1", Description = "Assembly - Theaters, Concert Halls" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "A-2", Description = "Assembly - Eating & Drinking" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "A-3", Description = "Assembly - Other" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "A-4", Description = "Assembly - Indoor Sporting Events" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "A-5", Description = "Assembly - Outdoors" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "B", Description = "Business" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "E", Description = "Educational" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "F-1", Description = "Factory & lndustiral - Moderate Hazard" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "F-2", Description = "Factory & Industrial - Low Hazard" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "H-1", Description = "High Hazard-Explosive Detonation" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "H-2", Description = "High Hazard-Accelerated Burning" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "H-3", Description = "High Hazard-Readily Supports Combustion" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "H-4", Description = "High Hazard-Health" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "H-5", Description = "High Hazard-Semiconductors" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "I-1", Description = "Institutional - Assisted Living" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "I-2", Description = "Institutional - Assisted Incapacitated" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "I-3", Description = "Institutional - Restrained" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "I-4", Description = "Institutional - Daytime Custodial Care" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "M", Description = "Mercantile" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "R-1", Description = "Residential-Hotels,Dormitoris,Congregat" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "R-2", Description = "Residential-Apartment Housed" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "R-3", Description = "Residential-1 & 2 Family Houses" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "S-1", Description = "Storage - Moderate Hazard" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "S-2", Description = "Storage - Low Hazard" },
                new OccupancyClassification { Is_2008_2014 = true, Code = "U", Description = "Utility & Miscellaneous" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "A", Description = "Hazard" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "B-1", Description = "Storage (Moderate Hazard)" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "B-2", Description = "Storage (Low Hazard)" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "C", Description = "Mercantile" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "COM", Description = "Old Code- Commercial Bldgs" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "D-1", Description = "Industrial (Moderate Hazard)" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "D-2", Description = "Industrial (Low Hazard)" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "E", Description = "Business" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "F-1A", Description = "Assembly (Theaters)" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "F-1B", Description = "Assembly (ChurchmConcert Halls)" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "F-2", Description = "Assembly (Out Doors)" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "F-3", Description = "Assembly (Museums)" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "F-4", Description = "Assembly (Restaurants)" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "G", Description = "Education" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "H-1", Description = "Institutional (Restrained)" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "H-2", Description = "Institutional (Incapacitated)" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "J-0", Description = "Residential 3 Family Dwelling" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "J-1", Description = "Residential (Hotels)" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "J-2", Description = "Residential Apt House" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "J-3", Description = "Residential - 1 and 2 Family Houses" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "K", Description = "Miscellanous" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "PUB", Description = "Old Code- Public Bldgs" },
                new OccupancyClassification { Is_2008_2014 = false, Code = "RES", Description = "Old Code-Resid Bldgs" });
                context.SaveChanges();
            }

            if (!context.ConstructionClassifications.Any())
            {
                context.ConstructionClassifications.AddOrUpdate(
                r => r.Description,

                new ConstructionClassification { Is_2008_2014 = true, Code = "1-A", Description = "3 Hour Protected (Non - Combustible)" },
                new ConstructionClassification { Is_2008_2014 = true, Code = "1-B", Description = "2 Hour Protected (Non - Combustible)" },
                new ConstructionClassification { Is_2008_2014 = true, Code = "2-A", Description = "1 Hour Protected (Non - Combustible)" },
                new ConstructionClassification { Is_2008_2014 = true, Code = "2-B", Description = "Unprotected (Non - Combustible)" },
                new ConstructionClassification { Is_2008_2014 = true, Code = "3-A", Description = "Protected Wood Joist (Combustible)" },
                new ConstructionClassification { Is_2008_2014 = true, Code = "3-B", Description = "Unprotected Wood Joist (Combustible)" },
                new ConstructionClassification { Is_2008_2014 = true, Code = "4-HT", Description = "Heavy Timber (combustible)" },
                new ConstructionClassification { Is_2008_2014 = true, Code = "5-A", Description = "Protected Wood Frame (Combustible)" },
                new ConstructionClassification { Is_2008_2014 = true, Code = "5-B", Description = "Unprotected Wood Frame (Combustible)" },
                new ConstructionClassification { Is_2008_2014 = false, Code = "1", Description = "Fireproof Structures" },
                new ConstructionClassification { Is_2008_2014 = false, Code = "2", Description = "Fire-protected Structures" },
                new ConstructionClassification { Is_2008_2014 = false, Code = "3", Description = "Non-fireproof structures" },
                new ConstructionClassification { Is_2008_2014 = false, Code = "4", Description = "Wood Frame Structures" },
                new ConstructionClassification { Is_2008_2014 = false, Code = "5", Description = "Metal Structures" },
                new ConstructionClassification { Is_2008_2014 = false, Code = "6", Description = "Heavy Timber Structures" },
                new ConstructionClassification { Is_2008_2014 = false, Code = "I-A", Description = "4 Hour Protected" },
                new ConstructionClassification { Is_2008_2014 = false, Code = "I-B", Description = "3 Hour Protected" },
                new ConstructionClassification { Is_2008_2014 = false, Code = "I-C", Description = "2 Hour Protected" },
                new ConstructionClassification { Is_2008_2014 = false, Code = "I-D", Description = "1 Hour Protected" },
                new ConstructionClassification { Is_2008_2014 = false, Code = "I-E", Description = "Unprotected" },
                new ConstructionClassification { Is_2008_2014 = false, Code = "II-A", Description = "Heavy Timber" },
                new ConstructionClassification { Is_2008_2014 = false, Code = "II-B", Description = "Protected Wood Joist" },
                new ConstructionClassification { Is_2008_2014 = false, Code = "II-C", Description = "Unprotected Wood Joist" },
                new ConstructionClassification { Is_2008_2014 = false, Code = "II-D", Description = "Protected Wood Frame" },
                new ConstructionClassification { Is_2008_2014 = false, Code = "II-E", Description = "Unprotected Wood Frame" });
                context.SaveChanges();
            }

            if (!context.MultipleDwellingClassifications.Any())
            {
                context.MultipleDwellingClassifications.AddOrUpdate(
                r => r.Description,
                new MultipleDwellingClassification { Code = "CAA", Description = "COMMERCIAL ALTERED 'A'" },
                new MultipleDwellingClassification { Code = "CAB", Description = "COMMERCIAL ALTERED 'B'" },
                new MultipleDwellingClassification { Code = "CANL", Description = "CONVERTED NEW LAW" },
                new MultipleDwellingClassification { Code = "COL", Description = "CONVERTED OLD LAW" },
                new MultipleDwellingClassification { Code = "HACA", Description = "HEREAFTER CONVERTED CLASS A" },
                new MultipleDwellingClassification { Code = "HACB", Description = "HEREAFTER CONVERTED CLASS B" },
                new MultipleDwellingClassification { Code = "HAEA", Description = "HEREAFTER ERECTED CLASS A" },
                new MultipleDwellingClassification { Code = "HAEB", Description = "HEREAFTER ERECTED CLASS B (HOTELS)" },
                new MultipleDwellingClassification { Code = "HCA", Description = "HERETOFOR E CONVERTED CLASS A" },
                new MultipleDwellingClassification { Code = "HCB", Description = "HCB HERETOFORE CONVERTED CLASS B" },
                new MultipleDwellingClassification { Code = "HEXA", Description = "HERETF ERECTED EXISTING CLASS A (HOTELS)" },
                new MultipleDwellingClassification { Code = "HEXB", Description = "HERETF ERECTED EXISTING CLASS B (HOTELS)" },
                new MultipleDwellingClassification { Code = "JAR", Description = "JOINT ARTIST IN RESIDENCE" },
                new MultipleDwellingClassification { Code = "LH", Description = "LH LODGING HOUSE" },
                new MultipleDwellingClassification { Code = "NL", Description = "NEW LAW TENEMENTS" },
                new MultipleDwellingClassification { Code = "NLSR", Description = "NEW LAW SINGLE ROOM" },
                new MultipleDwellingClassification { Code = "OL", Description = "OLD LAW TENEMENTS " },
                new MultipleDwellingClassification { Code = "OLSR", Description = "OLD LAW SINGLE ROOM" },
                new MultipleDwellingClassification { Code = "Y", Description = "Y Y-TYPE BUILDING (CLASS B)" });
                context.SaveChanges();
            }

            if (!context.PrimaryStructuralSystems.Any())
            {
                context.PrimaryStructuralSystems.AddOrUpdate(
                r => r.Description,
                new PrimaryStructuralSystem { Description = "Concrete (CIP)" },
                new PrimaryStructuralSystem { Description = "Concrete (Precast)" },
                new PrimaryStructuralSystem { Description = "Masory" },
                new PrimaryStructuralSystem { Description = "Steel (Structural)" },
                new PrimaryStructuralSystem { Description = "Steel (Cold-Formed)" },
                new PrimaryStructuralSystem { Description = "Steel (Encased in Concrete)" },
                new PrimaryStructuralSystem { Description = "Wood" });
                context.SaveChanges();
            }

            if (!context.StructureOccupancyCategories.Any())
            {
                context.StructureOccupancyCategories.AddOrUpdate(
                r => r.Description,
                new StructureOccupancyCategory { Code = "1", Description = "Low Hazard to Human Life" },
                new StructureOccupancyCategory { Code = "2", Description = "Other than 1 3 or 4" },
                new StructureOccupancyCategory { Code = "3", Description = "Substantial Hazard to Human Life" },
                new StructureOccupancyCategory { Code = "4", Description = "Essential Facility" });
                context.SaveChanges();

            }

            if (!context.SeismicDesignCategories.Any())
            {
                context.SeismicDesignCategories.AddOrUpdate(
                r => r.Description,
                //new SeismicDesignCategory { Code = "A", Description = "Category" },
                new SeismicDesignCategory { Code = "B", Description = "Category" },
                new SeismicDesignCategory { Code = "C", Description = "Category" },
                new SeismicDesignCategory { Code = "D", Description = "Category" }
                //new SeismicDesignCategory { Code = "X", Description = "Category" }
                );
                context.SaveChanges();
            }
        }

        private void CreateRole(RpoIdentityDbContext context, RpoRoles role)
        {
            using (RpoRoleManager roleManager = new RpoRoleManager(new RoleStore<IdentityRole>(context)))
            {
                IdentityRole identityRole = roleManager.FindByName(role.ToString());

                if (identityRole == null)
                {
                    identityRole = new IdentityRole(role.ToString());
                    roleManager.Create(identityRole);
                }
            }
        }
        private void SetRoles(RpoIdentityDbContext context)
        {
        }

        private RpoIdentityUser CreateUser(RpoIdentityDbContext context, string userName, string email, string password)
        {
            RpoUserManager userManager = new RpoUserManager(new UserStore<RpoIdentityUser>(context));

            RpoIdentityUser user = userManager.FindByName(userName);

            if (user == null)
            {
                user = new RpoIdentityUser
                {
                    UserName = userName,
                    Email = email,
                    EmailConfirmed = true,
                    LockoutEnabled = true
                };

                userManager.Create(user, password);
                user = userManager.FindByName(userName);
            }

            return user;
        }

        private static List<RpoIdentityClient> BuildClientsList()
        {
            List<RpoIdentityClient> clientsList = new List<RpoIdentityClient>
            {
                // TODO: É necessário desacoplar algumas informações e configurações.
                new RpoIdentityClient
                {
                    Id = "web",
                    Secret = Helper.GetHash("abc@123"),
                    Name = "AngularJS front-end Application",
                    ApplicationType =  ApplicationTypes.JavaScript,
                    Active = true,
                    RefreshTokenLifeTime = 7200,
#if DEBUG
                    AllowedOrigin = "*"
#else
                    AllowedOrigin = "http://rpoapp.azurewebsites.net"
#endif
                }
            };

            return clientsList;
        }


    }
}
