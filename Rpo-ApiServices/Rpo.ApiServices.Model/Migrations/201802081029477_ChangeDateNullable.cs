namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDateNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Addresses", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Addresses", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.AddressTypes", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.AddressTypes", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.Employees", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.AgentCertificates", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.AgentCertificates", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.DocumentTypes", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.DocumentTypes", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.EmployeeDocuments", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.EmployeeDocuments", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.States", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.States", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.Companies", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Companies", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.CompanyTypes", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.CompanyTypes", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.Contacts", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Contacts", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.ContactLicenses", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.ContactLicenses", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.ContactLicenseTypes", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.ContactLicenseTypes", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.ContactTitles", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.ContactTitles", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.ContactDocuments", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.ContactDocuments", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.Prefixes", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Prefixes", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.Suffixes", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Suffixes", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.ApplicationStatus", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.ApplicationStatus", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.Boroughs", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Boroughs", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.Cities", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Cities", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.EmailTypes", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.EmailTypes", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.JobApplications", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.JobApplications", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Jobs", "LastModiefiedDate", c => c.DateTime());
            AlterColumn("dbo.Jobs", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.JobContacts", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.JobContacts", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.JobContactTypes", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.JobContactTypes", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.JobDocuments", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.JobDocuments", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.JobApplicationTypes", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.JobApplicationTypes", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.JobWorkTypes", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.JobWorkTypes", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.JobMilestones", "LastModified", c => c.DateTime());
            AlterColumn("dbo.JobMilestones", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.JobTypes", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.JobTypes", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.WorkTypes", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.WorkTypes", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.Milestones", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Milestones", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.RfpAddresses", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.RfpAddresses", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.OccupancyClassifications", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.OccupancyClassifications", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.SeismicDesignCategories", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.SeismicDesignCategories", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.RfpStatus", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.RfpStatus", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.JobScopes", "LastModified", c => c.DateTime());
            AlterColumn("dbo.JobScopes", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Tasks", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.Tasks", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.TaskNotes", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.TaskNotes", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.TaskTypes", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.TaskTypes", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.JobApplicationWorkPermitTypes", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.JobApplicationWorkPermitTypes", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.ReferenceDocuments", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.ReferenceDocuments", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.ReferenceLinks", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.ReferenceLinks", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.TransmissionTypes", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.TransmissionTypes", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.TaskReminders", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.TaskReminders", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Verbiages", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Verbiages", "LastModifiedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Verbiages", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Verbiages", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TaskReminders", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TaskReminders", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TransmissionTypes", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TransmissionTypes", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ReferenceLinks", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ReferenceLinks", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ReferenceDocuments", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ReferenceDocuments", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobApplicationWorkPermitTypes", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobApplicationWorkPermitTypes", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TaskTypes", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TaskTypes", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TaskNotes", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TaskNotes", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Tasks", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Tasks", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobScopes", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobScopes", "LastModified", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RfpStatus", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RfpStatus", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SeismicDesignCategories", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SeismicDesignCategories", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OccupancyClassifications", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OccupancyClassifications", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RfpAddresses", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RfpAddresses", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Milestones", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Milestones", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.WorkTypes", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.WorkTypes", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobTypes", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobTypes", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobMilestones", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobMilestones", "LastModified", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobWorkTypes", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobWorkTypes", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobApplicationTypes", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobApplicationTypes", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobDocuments", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobDocuments", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobContactTypes", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobContactTypes", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobContacts", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobContacts", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Jobs", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Jobs", "LastModiefiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobApplications", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobApplications", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EmailTypes", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EmailTypes", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Cities", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Cities", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Boroughs", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Boroughs", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ApplicationStatus", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ApplicationStatus", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Suffixes", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Suffixes", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Prefixes", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Prefixes", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ContactDocuments", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ContactDocuments", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ContactTitles", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ContactTitles", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ContactLicenseTypes", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ContactLicenseTypes", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ContactLicenses", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ContactLicenses", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Contacts", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Contacts", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CompanyTypes", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CompanyTypes", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Companies", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Companies", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.States", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.States", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EmployeeDocuments", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EmployeeDocuments", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DocumentTypes", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DocumentTypes", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AgentCertificates", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AgentCertificates", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Employees", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AddressTypes", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AddressTypes", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Addresses", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Addresses", "CreatedDate", c => c.DateTime(nullable: false));
        }
    }
}
