namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLastModifiedDateAndCreatedDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "CreatedBy", c => c.Int());
            AddColumn("dbo.Addresses", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.Addresses", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.Addresses", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.AddressTypes", "CreatedBy", c => c.Int());
            AddColumn("dbo.AddressTypes", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.AddressTypes", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.AddressTypes", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.Companies", "CreatedBy", c => c.Int());
            AddColumn("dbo.Companies", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.Companies", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.Companies", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.CompanyTypes", "CreatedBy", c => c.Int());
            AddColumn("dbo.CompanyTypes", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.CompanyTypes", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.CompanyTypes", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.Contacts", "CreatedBy", c => c.Int());
            AddColumn("dbo.Contacts", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.Contacts", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.Contacts", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.ContactLicenses", "CreatedBy", c => c.Int());
            AddColumn("dbo.ContactLicenses", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.ContactLicenses", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.ContactLicenses", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.ContactLicenseTypes", "CreatedBy", c => c.Int());
            AddColumn("dbo.ContactLicenseTypes", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.ContactLicenseTypes", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.ContactLicenseTypes", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.ContactTitles", "CreatedBy", c => c.Int());
            AddColumn("dbo.ContactTitles", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.ContactTitles", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.ContactTitles", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.ContactDocuments", "CreatedBy", c => c.Int());
            AddColumn("dbo.ContactDocuments", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.ContactDocuments", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.ContactDocuments", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.Prefixes", "CreatedBy", c => c.Int());
            AddColumn("dbo.Prefixes", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.Prefixes", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.Prefixes", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.Suffixes", "CreatedBy", c => c.Int());
            AddColumn("dbo.Suffixes", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.Suffixes", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.Suffixes", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.States", "CreatedBy", c => c.Int());
            AddColumn("dbo.States", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.States", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.States", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.AgentCertificates", "CreatedBy", c => c.Int());
            AddColumn("dbo.AgentCertificates", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.AgentCertificates", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.AgentCertificates", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.DocumentTypes", "CreatedBy", c => c.Int());
            AddColumn("dbo.DocumentTypes", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.DocumentTypes", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.DocumentTypes", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.EmployeeDocuments", "CreatedBy", c => c.Int());
            AddColumn("dbo.EmployeeDocuments", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.EmployeeDocuments", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.EmployeeDocuments", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.ApplicationStatus", "CreatedBy", c => c.Int());
            AddColumn("dbo.ApplicationStatus", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.ApplicationStatus", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.ApplicationStatus", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.Boroughs", "CreatedBy", c => c.Int());
            AddColumn("dbo.Boroughs", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.Boroughs", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.Boroughs", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.Cities", "CreatedBy", c => c.Int());
            AddColumn("dbo.Cities", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.Cities", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.Cities", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.EmailTypes", "CreatedBy", c => c.Int());
            AddColumn("dbo.EmailTypes", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.EmailTypes", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.EmailTypes", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.JobApplications", "CreatedBy", c => c.Int());
            AddColumn("dbo.JobApplications", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.JobApplications", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.Jobs", "CreatedBy", c => c.Int());
            AddColumn("dbo.Jobs", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.Jobs", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.JobContacts", "CreatedBy", c => c.Int());
            AddColumn("dbo.JobContacts", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.JobContacts", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.JobContacts", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.JobContactTypes", "CreatedBy", c => c.Int());
            AddColumn("dbo.JobContactTypes", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.JobContactTypes", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.JobContactTypes", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.JobDocuments", "CreatedBy", c => c.Int());
            AddColumn("dbo.JobDocuments", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.JobDocuments", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.JobDocuments", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.JobApplicationTypes", "CreatedBy", c => c.Int());
            AddColumn("dbo.JobApplicationTypes", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.JobApplicationTypes", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.JobApplicationTypes", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.JobWorkTypes", "CreatedBy", c => c.Int());
            AddColumn("dbo.JobWorkTypes", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.JobWorkTypes", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.JobWorkTypes", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.JobMilestones", "CreatedBy", c => c.Int());
            AddColumn("dbo.JobMilestones", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.JobTypes", "CreatedBy", c => c.Int());
            AddColumn("dbo.JobTypes", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.JobTypes", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.JobTypes", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.WorkTypes", "CreatedBy", c => c.Int());
            AddColumn("dbo.WorkTypes", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.WorkTypes", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.WorkTypes", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.Milestones", "CreatedBy", c => c.Int());
            AddColumn("dbo.Milestones", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.Milestones", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.Milestones", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.RfpAddresses", "CreatedBy", c => c.Int());
            AddColumn("dbo.RfpAddresses", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.RfpAddresses", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.RfpAddresses", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.OccupancyClassifications", "CreatedBy", c => c.Int());
            AddColumn("dbo.OccupancyClassifications", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.OccupancyClassifications", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.OccupancyClassifications", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.SeismicDesignCategories", "CreatedBy", c => c.Int());
            AddColumn("dbo.SeismicDesignCategories", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.SeismicDesignCategories", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.SeismicDesignCategories", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.RfpStatus", "CreatedBy", c => c.Int());
            AddColumn("dbo.RfpStatus", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.RfpStatus", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.RfpStatus", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.JobScopes", "CreatedBy", c => c.Int());
            AddColumn("dbo.JobScopes", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.Tasks", "CreatedBy", c => c.Int());
            AddColumn("dbo.Tasks", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.TaskNotes", "CreatedBy", c => c.Int());
            AddColumn("dbo.TaskNotes", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.TaskTypes", "CreatedBy", c => c.Int());
            AddColumn("dbo.TaskTypes", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.TaskTypes", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.TaskTypes", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.JobApplicationWorkPermitTypes", "CreatedBy", c => c.Int());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.JobApplicationWorkPermitTypes", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.ReferenceDocuments", "CreatedBy", c => c.Int());
            AddColumn("dbo.ReferenceDocuments", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.ReferenceDocuments", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.ReferenceDocuments", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.ReferenceLinks", "CreatedBy", c => c.Int());
            AddColumn("dbo.ReferenceLinks", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.ReferenceLinks", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.ReferenceLinks", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.TransmissionTypes", "CreatedBy", c => c.Int());
            AddColumn("dbo.TransmissionTypes", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.TransmissionTypes", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.TransmissionTypes", "LastModifiedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.TaskReminders", "CreatedBy", c => c.Int());
            AddColumn("dbo.TaskReminders", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.Verbiages", "CreatedBy", c => c.Int());
            AddColumn("dbo.Verbiages", "CreatedDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.Verbiages", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.Verbiages", "LastModifiedDate", c => c.DateTime(nullable: true));
            AlterColumn("dbo.Tasks", "LastModifiedDate", c => c.DateTime(nullable: true));
            AlterColumn("dbo.TaskNotes", "LastModifiedDate", c => c.DateTime(nullable: true));
            AlterColumn("dbo.TaskReminders", "LastModifiedDate", c => c.DateTime(nullable: true));
            CreateIndex("dbo.Addresses", "CreatedBy");
            CreateIndex("dbo.Addresses", "LastModifiedBy");
            CreateIndex("dbo.AddressTypes", "CreatedBy");
            CreateIndex("dbo.AddressTypes", "LastModifiedBy");
            CreateIndex("dbo.AgentCertificates", "CreatedBy");
            CreateIndex("dbo.AgentCertificates", "LastModifiedBy");
            CreateIndex("dbo.DocumentTypes", "CreatedBy");
            CreateIndex("dbo.DocumentTypes", "LastModifiedBy");
            CreateIndex("dbo.EmployeeDocuments", "CreatedBy");
            CreateIndex("dbo.EmployeeDocuments", "LastModifiedBy");
            CreateIndex("dbo.States", "CreatedBy");
            CreateIndex("dbo.States", "LastModifiedBy");
            CreateIndex("dbo.Companies", "CreatedBy");
            CreateIndex("dbo.Companies", "LastModifiedBy");
            CreateIndex("dbo.CompanyTypes", "CreatedBy");
            CreateIndex("dbo.CompanyTypes", "LastModifiedBy");
            CreateIndex("dbo.Contacts", "CreatedBy");
            CreateIndex("dbo.Contacts", "LastModifiedBy");
            CreateIndex("dbo.ContactLicenses", "CreatedBy");
            CreateIndex("dbo.ContactLicenses", "LastModifiedBy");
            CreateIndex("dbo.ContactLicenseTypes", "CreatedBy");
            CreateIndex("dbo.ContactLicenseTypes", "LastModifiedBy");
            CreateIndex("dbo.ContactTitles", "CreatedBy");
            CreateIndex("dbo.ContactTitles", "LastModifiedBy");
            CreateIndex("dbo.ContactDocuments", "CreatedBy");
            CreateIndex("dbo.ContactDocuments", "LastModifiedBy");
            CreateIndex("dbo.Prefixes", "CreatedBy");
            CreateIndex("dbo.Prefixes", "LastModifiedBy");
            CreateIndex("dbo.Suffixes", "CreatedBy");
            CreateIndex("dbo.Suffixes", "LastModifiedBy");
            CreateIndex("dbo.ApplicationStatus", "CreatedBy");
            CreateIndex("dbo.ApplicationStatus", "LastModifiedBy");
            CreateIndex("dbo.Boroughs", "CreatedBy");
            CreateIndex("dbo.Boroughs", "LastModifiedBy");
            CreateIndex("dbo.Cities", "CreatedBy");
            CreateIndex("dbo.Cities", "LastModifiedBy");
            CreateIndex("dbo.EmailTypes", "CreatedBy");
            CreateIndex("dbo.EmailTypes", "LastModifiedBy");
            CreateIndex("dbo.JobApplications", "CreatedBy");
            CreateIndex("dbo.JobApplications", "LastModifiedBy");
            CreateIndex("dbo.Jobs", "CreatedBy");
            CreateIndex("dbo.Jobs", "LastModifiedBy");
            CreateIndex("dbo.JobContacts", "CreatedBy");
            CreateIndex("dbo.JobContacts", "LastModifiedBy");
            CreateIndex("dbo.JobContactTypes", "CreatedBy");
            CreateIndex("dbo.JobContactTypes", "LastModifiedBy");
            CreateIndex("dbo.JobDocuments", "CreatedBy");
            CreateIndex("dbo.JobDocuments", "LastModifiedBy");
            CreateIndex("dbo.JobApplicationTypes", "CreatedBy");
            CreateIndex("dbo.JobApplicationTypes", "LastModifiedBy");
            CreateIndex("dbo.JobWorkTypes", "CreatedBy");
            CreateIndex("dbo.JobWorkTypes", "LastModifiedBy");
            CreateIndex("dbo.JobMilestones", "CreatedBy");
            CreateIndex("dbo.JobTypes", "CreatedBy");
            CreateIndex("dbo.JobTypes", "LastModifiedBy");
            CreateIndex("dbo.WorkTypes", "CreatedBy");
            CreateIndex("dbo.WorkTypes", "LastModifiedBy");
            CreateIndex("dbo.Milestones", "CreatedBy");
            CreateIndex("dbo.Milestones", "LastModifiedBy");
            CreateIndex("dbo.RfpAddresses", "CreatedBy");
            CreateIndex("dbo.RfpAddresses", "LastModifiedBy");
            CreateIndex("dbo.OccupancyClassifications", "CreatedBy");
            CreateIndex("dbo.OccupancyClassifications", "LastModifiedBy");
            CreateIndex("dbo.SeismicDesignCategories", "CreatedBy");
            CreateIndex("dbo.SeismicDesignCategories", "LastModifiedBy");
            CreateIndex("dbo.RfpStatus", "CreatedBy");
            CreateIndex("dbo.RfpStatus", "LastModifiedBy");
            CreateIndex("dbo.JobScopes", "CreatedBy");
            CreateIndex("dbo.Tasks", "CreatedBy");
            CreateIndex("dbo.TaskNotes", "CreatedBy");
            CreateIndex("dbo.TaskTypes", "CreatedBy");
            CreateIndex("dbo.TaskTypes", "LastModifiedBy");
            CreateIndex("dbo.JobApplicationWorkPermitTypes", "CreatedBy");
            CreateIndex("dbo.JobApplicationWorkPermitTypes", "LastModifiedBy");
            CreateIndex("dbo.ReferenceDocuments", "CreatedBy");
            CreateIndex("dbo.ReferenceDocuments", "LastModifiedBy");
            CreateIndex("dbo.ReferenceLinks", "CreatedBy");
            CreateIndex("dbo.ReferenceLinks", "LastModifiedBy");
            CreateIndex("dbo.TransmissionTypes", "CreatedBy");
            CreateIndex("dbo.TransmissionTypes", "LastModifiedBy");
            CreateIndex("dbo.TaskReminders", "CreatedBy");
            CreateIndex("dbo.Verbiages", "CreatedBy");
            CreateIndex("dbo.Verbiages", "LastModifiedBy");
            AddForeignKey("dbo.AgentCertificates", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.DocumentTypes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.DocumentTypes", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.AgentCertificates", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.EmployeeDocuments", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.EmployeeDocuments", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.States", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.States", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.AddressTypes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.AddressTypes", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.CompanyTypes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.CompanyTypes", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Companies", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Companies", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.ContactLicenseTypes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.ContactLicenseTypes", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.ContactLicenses", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.ContactLicenses", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.ContactTitles", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.ContactTitles", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Contacts", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.ContactDocuments", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.ContactDocuments", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Contacts", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Prefixes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Prefixes", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Suffixes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Suffixes", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Addresses", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Addresses", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.ApplicationStatus", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.ApplicationStatus", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Boroughs", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Boroughs", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Cities", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Cities", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.EmailTypes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.EmailTypes", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobApplications", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobContacts", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobContactTypes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobContactTypes", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobContacts", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Jobs", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobDocuments", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobDocuments", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobApplicationTypes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobWorkTypes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobWorkTypes", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobApplicationTypes", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Jobs", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobMilestones", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobTypes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobTypes", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.WorkTypes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.WorkTypes", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Milestones", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Milestones", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.RfpAddresses", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.RfpAddresses", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.OccupancyClassifications", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.OccupancyClassifications", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.SeismicDesignCategories", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.SeismicDesignCategories", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.RfpStatus", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.RfpStatus", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobScopes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Tasks", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.TaskNotes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.TaskTypes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.TaskTypes", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobApplicationWorkPermitTypes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobApplicationWorkPermitTypes", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobApplications", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.ReferenceDocuments", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.ReferenceDocuments", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.ReferenceLinks", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.ReferenceLinks", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.TransmissionTypes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.TransmissionTypes", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.TaskReminders", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Verbiages", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Verbiages", "LastModifiedBy", "dbo.Employees", "Id");
            AlterStoredProcedure(
                "dbo.Address_Insert",
                p => new
                    {
                        IdAddressType = p.Int(),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        City = p.String(),
                        IdState = p.Int(),
                        ZipCode = p.String(maxLength: 10),
                        Phone = p.String(maxLength: 15),
                        IdCompany = p.Int(),
                        IdContact = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Addresses]([IdAddressType], [Address1], [Address2], [City], [IdState], [ZipCode], [Phone], [IdCompany], [IdContact], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@IdAddressType, @Address1, @Address2, @City, @IdState, @ZipCode, @Phone, @IdCompany, @IdContact, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Addresses]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Addresses] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Address_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdAddressType = p.Int(),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        City = p.String(),
                        IdState = p.Int(),
                        ZipCode = p.String(maxLength: 10),
                        Phone = p.String(maxLength: 15),
                        IdCompany = p.Int(),
                        IdContact = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Addresses]
                      SET [IdAddressType] = @IdAddressType, [Address1] = @Address1, [Address2] = @Address2, [City] = @City, [IdState] = @IdState, [ZipCode] = @ZipCode, [Phone] = @Phone, [IdCompany] = @IdCompany, [IdContact] = @IdContact, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.AddressType_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        DisplayOrder = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[AddressTypes]([Name], [DisplayOrder], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @DisplayOrder, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[AddressTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[AddressTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.AddressType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        DisplayOrder = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[AddressTypes]
                      SET [Name] = @Name, [DisplayOrder] = @DisplayOrder, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Company_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        TrackingNumber = p.String(maxLength: 10),
                        TrackingExpiry = p.DateTime(),
                        IBMNumber = p.String(maxLength: 10),
                        SpecialInspectionAgencyNumber = p.String(maxLength: 10),
                        SpecialInspectionAgencyExpiry = p.DateTime(),
                        HICNumber = p.String(maxLength: 10),
                        HICExpiry = p.DateTime(),
                        TaxIdNumber = p.String(maxLength: 9),
                        InsuranceWorkCompensation = p.DateTime(),
                        InsuranceDisability = p.DateTime(),
                        InsuranceGeneralLiability = p.DateTime(),
                        InsuranceObstructionBond = p.DateTime(),
                        Notes = p.String(),
                        Url = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Companies]([Name], [TrackingNumber], [TrackingExpiry], [IBMNumber], [SpecialInspectionAgencyNumber], [SpecialInspectionAgencyExpiry], [HICNumber], [HICExpiry], [TaxIdNumber], [InsuranceWorkCompensation], [InsuranceDisability], [InsuranceGeneralLiability], [InsuranceObstructionBond], [Notes], [Url], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @TrackingNumber, @TrackingExpiry, @IBMNumber, @SpecialInspectionAgencyNumber, @SpecialInspectionAgencyExpiry, @HICNumber, @HICExpiry, @TaxIdNumber, @InsuranceWorkCompensation, @InsuranceDisability, @InsuranceGeneralLiability, @InsuranceObstructionBond, @Notes, @Url, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Companies]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Companies] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Company_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        TrackingNumber = p.String(maxLength: 10),
                        TrackingExpiry = p.DateTime(),
                        IBMNumber = p.String(maxLength: 10),
                        SpecialInspectionAgencyNumber = p.String(maxLength: 10),
                        SpecialInspectionAgencyExpiry = p.DateTime(),
                        HICNumber = p.String(maxLength: 10),
                        HICExpiry = p.DateTime(),
                        TaxIdNumber = p.String(maxLength: 9),
                        InsuranceWorkCompensation = p.DateTime(),
                        InsuranceDisability = p.DateTime(),
                        InsuranceGeneralLiability = p.DateTime(),
                        InsuranceObstructionBond = p.DateTime(),
                        Notes = p.String(),
                        Url = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Companies]
                      SET [Name] = @Name, [TrackingNumber] = @TrackingNumber, [TrackingExpiry] = @TrackingExpiry, [IBMNumber] = @IBMNumber, [SpecialInspectionAgencyNumber] = @SpecialInspectionAgencyNumber, [SpecialInspectionAgencyExpiry] = @SpecialInspectionAgencyExpiry, [HICNumber] = @HICNumber, [HICExpiry] = @HICExpiry, [TaxIdNumber] = @TaxIdNumber, [InsuranceWorkCompensation] = @InsuranceWorkCompensation, [InsuranceDisability] = @InsuranceDisability, [InsuranceGeneralLiability] = @InsuranceGeneralLiability, [InsuranceObstructionBond] = @InsuranceObstructionBond, [Notes] = @Notes, [Url] = @Url, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.CompanyType_Insert",
                p => new
                    {
                        ItemName = p.String(),
                        IdParent = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[CompanyTypes]([ItemName], [IdParent], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@ItemName, @IdParent, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[CompanyTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[CompanyTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.CompanyType_Update",
                p => new
                    {
                        Id = p.Int(),
                        ItemName = p.String(),
                        IdParent = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[CompanyTypes]
                      SET [ItemName] = @ItemName, [IdParent] = @IdParent, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Contact_Insert",
                p => new
                    {
                        PersonalType = p.Int(),
                        IdPrefix = p.Int(),
                        IdSuffix = p.Int(),
                        FirstName = p.String(maxLength: 50),
                        MiddleName = p.String(maxLength: 2),
                        LastName = p.String(maxLength: 50),
                        IdCompany = p.Int(),
                        IdContactTitle = p.Int(),
                        BirthDate = p.DateTime(),
                        WorkPhone = p.String(maxLength: 15),
                        WorkPhoneExt = p.String(maxLength: 5),
                        MobilePhone = p.String(maxLength: 15),
                        OtherPhone = p.String(maxLength: 15),
                        Email = p.String(maxLength: 255),
                        ContactImagePath = p.String(maxLength: 200),
                        ContactImageThumbPath = p.String(maxLength: 200),
                        Notes = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Contacts]([PersonalType], [IdPrefix], [IdSuffix], [FirstName], [MiddleName], [LastName], [IdCompany], [IdContactTitle], [BirthDate], [WorkPhone], [WorkPhoneExt], [MobilePhone], [OtherPhone], [Email], [ContactImagePath], [ContactImageThumbPath], [Notes], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@PersonalType, @IdPrefix, @IdSuffix, @FirstName, @MiddleName, @LastName, @IdCompany, @IdContactTitle, @BirthDate, @WorkPhone, @WorkPhoneExt, @MobilePhone, @OtherPhone, @Email, @ContactImagePath, @ContactImageThumbPath, @Notes, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Contacts]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Contacts] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Contact_Update",
                p => new
                    {
                        Id = p.Int(),
                        PersonalType = p.Int(),
                        IdPrefix = p.Int(),
                        IdSuffix = p.Int(),
                        FirstName = p.String(maxLength: 50),
                        MiddleName = p.String(maxLength: 2),
                        LastName = p.String(maxLength: 50),
                        IdCompany = p.Int(),
                        IdContactTitle = p.Int(),
                        BirthDate = p.DateTime(),
                        WorkPhone = p.String(maxLength: 15),
                        WorkPhoneExt = p.String(maxLength: 5),
                        MobilePhone = p.String(maxLength: 15),
                        OtherPhone = p.String(maxLength: 15),
                        Email = p.String(maxLength: 255),
                        ContactImagePath = p.String(maxLength: 200),
                        ContactImageThumbPath = p.String(maxLength: 200),
                        Notes = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Contacts]
                      SET [PersonalType] = @PersonalType, [IdPrefix] = @IdPrefix, [IdSuffix] = @IdSuffix, [FirstName] = @FirstName, [MiddleName] = @MiddleName, [LastName] = @LastName, [IdCompany] = @IdCompany, [IdContactTitle] = @IdContactTitle, [BirthDate] = @BirthDate, [WorkPhone] = @WorkPhone, [WorkPhoneExt] = @WorkPhoneExt, [MobilePhone] = @MobilePhone, [OtherPhone] = @OtherPhone, [Email] = @Email, [ContactImagePath] = @ContactImagePath, [ContactImageThumbPath] = @ContactImageThumbPath, [Notes] = @Notes, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.ContactLicense_Insert",
                p => new
                    {
                        IdContactLicenseType = p.Int(),
                        Number = p.String(maxLength: 15),
                        ExpirationLicenseDate = p.DateTime(),
                        IdContact = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[ContactLicenses]([IdContactLicenseType], [Number], [ExpirationLicenseDate], [IdContact], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@IdContactLicenseType, @Number, @ExpirationLicenseDate, @IdContact, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ContactLicenses]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ContactLicenses] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.ContactLicense_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdContactLicenseType = p.Int(),
                        Number = p.String(maxLength: 15),
                        ExpirationLicenseDate = p.DateTime(),
                        IdContact = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[ContactLicenses]
                      SET [IdContactLicenseType] = @IdContactLicenseType, [Number] = @Number, [ExpirationLicenseDate] = @ExpirationLicenseDate, [IdContact] = @IdContact, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.ContactLicenseType_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[ContactLicenseTypes]([Name], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ContactLicenseTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ContactLicenseTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.ContactLicenseType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[ContactLicenseTypes]
                      SET [Name] = @Name, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.ContactTitle_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 100),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[ContactTitles]([Name], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ContactTitles]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ContactTitles] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.ContactTitle_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 100),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[ContactTitles]
                      SET [Name] = @Name, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.ContactDocument_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 255),
                        IdContact = p.Int(),
                        DocumentPath = p.String(maxLength: 200),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[ContactDocuments]([Name], [IdContact], [DocumentPath], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @IdContact, @DocumentPath, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ContactDocuments]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ContactDocuments] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.ContactDocument_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 255),
                        IdContact = p.Int(),
                        DocumentPath = p.String(maxLength: 200),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[ContactDocuments]
                      SET [Name] = @Name, [IdContact] = @IdContact, [DocumentPath] = @DocumentPath, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Suffix_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 20),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Suffixes]([Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Description, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Suffixes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Suffixes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Suffix_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 20),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Suffixes]
                      SET [Description] = @Description, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.State_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        Acronym = p.String(maxLength: 2),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[States]([Name], [Acronym], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @Acronym, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[States]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[States] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.State_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        Acronym = p.String(maxLength: 2),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[States]
                      SET [Name] = @Name, [Acronym] = @Acronym, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.AgentCertificate_Insert",
                p => new
                    {
                        IdDocumentType = p.Int(),
                        NumberId = p.String(),
                        ExpirationDate = p.DateTime(),
                        Pin = p.String(maxLength: 10),
                        IdEmployee = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[AgentCertificates]([IdDocumentType], [NumberId], [ExpirationDate], [Pin], [IdEmployee], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@IdDocumentType, @NumberId, @ExpirationDate, @Pin, @IdEmployee, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[AgentCertificates]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[AgentCertificates] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.AgentCertificate_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdDocumentType = p.Int(),
                        NumberId = p.String(),
                        ExpirationDate = p.DateTime(),
                        Pin = p.String(maxLength: 10),
                        IdEmployee = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[AgentCertificates]
                      SET [IdDocumentType] = @IdDocumentType, [NumberId] = @NumberId, [ExpirationDate] = @ExpirationDate, [Pin] = @Pin, [IdEmployee] = @IdEmployee, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.DocumentType_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[DocumentTypes]([Name], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[DocumentTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[DocumentTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.DocumentType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[DocumentTypes]
                      SET [Name] = @Name, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.EmployeeDocument_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 255),
                        IdEmployee = p.Int(),
                        DocumentPath = p.String(maxLength: 200),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[EmployeeDocuments]([Name], [IdEmployee], [DocumentPath], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @IdEmployee, @DocumentPath, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[EmployeeDocuments]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[EmployeeDocuments] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.EmployeeDocument_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 255),
                        IdEmployee = p.Int(),
                        DocumentPath = p.String(maxLength: 200),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[EmployeeDocuments]
                      SET [Name] = @Name, [IdEmployee] = @IdEmployee, [DocumentPath] = @DocumentPath, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Borough_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                        BisCode = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Boroughs]([Description], [BisCode], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Description, @BisCode, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Boroughs]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Boroughs] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Borough_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 50),
                        BisCode = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Boroughs]
                      SET [Description] = @Description, [BisCode] = @BisCode, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.City_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        IdState = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Cities]([Name], [IdState], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @IdState, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Cities]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Cities] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.City_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        IdState = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Cities]
                      SET [Name] = @Name, [IdState] = @IdState, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.EmailType_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[EmailTypes]([Name], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[EmailTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[EmailTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.EmailType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[EmailTypes]
                      SET [Name] = @Name, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Job_Insert",
                p => new
                    {
                        JobNumber = p.String(),
                        IdRfpAddress = p.Int(),
                        IdRfp = p.Int(),
                        IdBorough = p.Int(),
                        HouseNumber = p.String(maxLength: 50),
                        StreetNumber = p.String(maxLength: 50),
                        FloorNumber = p.String(maxLength: 50),
                        Apartment = p.String(maxLength: 50),
                        SpecialPlace = p.String(maxLength: 50),
                        Block = p.String(maxLength: 50),
                        Lot = p.String(maxLength: 50),
                        HasLandMarkStatus = p.Boolean(),
                        HasEnvironmentalRestriction = p.Boolean(),
                        HasOpenWork = p.Boolean(),
                        IdCompany = p.Int(),
                        IdJobContactType = p.Int(),
                        IdContact = p.Int(),
                        IdProjectManager = p.Int(),
                        IdProjectCoordinator = p.Int(),
                        IdSignoffCoordinator = p.Int(),
                        StartDate = p.DateTime(),
                        EndDate = p.DateTime(),
                        LastModiefiedDate = p.DateTime(),
                        Status = p.Int(),
                        ScopeGeneralNotes = p.String(),
                        HasHolidayEmbargo = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Jobs]([JobNumber], [IdRfpAddress], [IdRfp], [IdBorough], [HouseNumber], [StreetNumber], [FloorNumber], [Apartment], [SpecialPlace], [Block], [Lot], [HasLandMarkStatus], [HasEnvironmentalRestriction], [HasOpenWork], [IdCompany], [IdJobContactType], [IdContact], [IdProjectManager], [IdProjectCoordinator], [IdSignoffCoordinator], [StartDate], [EndDate], [LastModiefiedDate], [Status], [ScopeGeneralNotes], [HasHolidayEmbargo], [CreatedBy], [CreatedDate], [LastModifiedBy])
                      VALUES (@JobNumber, @IdRfpAddress, @IdRfp, @IdBorough, @HouseNumber, @StreetNumber, @FloorNumber, @Apartment, @SpecialPlace, @Block, @Lot, @HasLandMarkStatus, @HasEnvironmentalRestriction, @HasOpenWork, @IdCompany, @IdJobContactType, @IdContact, @IdProjectManager, @IdProjectCoordinator, @IdSignoffCoordinator, @StartDate, @EndDate, @LastModiefiedDate, @Status, @ScopeGeneralNotes, @HasHolidayEmbargo, @CreatedBy, @CreatedDate, @LastModifiedBy)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Jobs]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Jobs] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Job_Update",
                p => new
                    {
                        Id = p.Int(),
                        JobNumber = p.String(),
                        IdRfpAddress = p.Int(),
                        IdRfp = p.Int(),
                        IdBorough = p.Int(),
                        HouseNumber = p.String(maxLength: 50),
                        StreetNumber = p.String(maxLength: 50),
                        FloorNumber = p.String(maxLength: 50),
                        Apartment = p.String(maxLength: 50),
                        SpecialPlace = p.String(maxLength: 50),
                        Block = p.String(maxLength: 50),
                        Lot = p.String(maxLength: 50),
                        HasLandMarkStatus = p.Boolean(),
                        HasEnvironmentalRestriction = p.Boolean(),
                        HasOpenWork = p.Boolean(),
                        IdCompany = p.Int(),
                        IdJobContactType = p.Int(),
                        IdContact = p.Int(),
                        IdProjectManager = p.Int(),
                        IdProjectCoordinator = p.Int(),
                        IdSignoffCoordinator = p.Int(),
                        StartDate = p.DateTime(),
                        EndDate = p.DateTime(),
                        LastModiefiedDate = p.DateTime(),
                        Status = p.Int(),
                        ScopeGeneralNotes = p.String(),
                        HasHolidayEmbargo = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Jobs]
                      SET [JobNumber] = @JobNumber, [IdRfpAddress] = @IdRfpAddress, [IdRfp] = @IdRfp, [IdBorough] = @IdBorough, [HouseNumber] = @HouseNumber, [StreetNumber] = @StreetNumber, [FloorNumber] = @FloorNumber, [Apartment] = @Apartment, [SpecialPlace] = @SpecialPlace, [Block] = @Block, [Lot] = @Lot, [HasLandMarkStatus] = @HasLandMarkStatus, [HasEnvironmentalRestriction] = @HasEnvironmentalRestriction, [HasOpenWork] = @HasOpenWork, [IdCompany] = @IdCompany, [IdJobContactType] = @IdJobContactType, [IdContact] = @IdContact, [IdProjectManager] = @IdProjectManager, [IdProjectCoordinator] = @IdProjectCoordinator, [IdSignoffCoordinator] = @IdSignoffCoordinator, [StartDate] = @StartDate, [EndDate] = @EndDate, [LastModiefiedDate] = @LastModiefiedDate, [Status] = @Status, [ScopeGeneralNotes] = @ScopeGeneralNotes, [HasHolidayEmbargo] = @HasHolidayEmbargo, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.JobType_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 100),
                        Content = p.String(),
                        Number = p.String(maxLength: 5),
                        IdParent = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[JobTypes]([Description], [Content], [Number], [IdParent], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Description, @Content, @Number, @IdParent, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[JobTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[JobTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.JobType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 100),
                        Content = p.String(),
                        Number = p.String(maxLength: 5),
                        IdParent = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[JobTypes]
                      SET [Description] = @Description, [Content] = @Content, [Number] = @Number, [IdParent] = @IdParent, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.WorkType_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                        Content = p.String(),
                        Number = p.String(maxLength: 5),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[WorkTypes]([Description], [Content], [Number], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Description, @Content, @Number, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[WorkTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[WorkTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.WorkType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 50),
                        Content = p.String(),
                        Number = p.String(maxLength: 5),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[WorkTypes]
                      SET [Description] = @Description, [Content] = @Content, [Number] = @Number, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Milestone_Insert",
                p => new
                    {
                        Name = p.String(),
                        Value = p.Double(),
                        Units = p.Int(),
                        IdRfp = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Milestones]([Name], [Value], [Units], [IdRfp], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @Value, @Units, @IdRfp, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Milestones]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Milestones] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Milestone_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(),
                        Value = p.Double(),
                        Units = p.Int(),
                        IdRfp = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Milestones]
                      SET [Name] = @Name, [Value] = @Value, [Units] = @Units, [IdRfp] = @IdRfp, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.RfpAddress_Insert",
                p => new
                    {
                        IdBorough = p.Int(),
                        HouseNumber = p.String(maxLength: 10),
                        Street = p.String(maxLength: 50),
                        ZipCode = p.String(maxLength: 5),
                        Block = p.String(maxLength: 50),
                        Lot = p.String(maxLength: 50),
                        BinNumber = p.String(maxLength: 50),
                        ComunityBoardNumber = p.String(maxLength: 50),
                        ZoneDistrict = p.String(maxLength: 50),
                        Overlay = p.String(maxLength: 50),
                        SpecialDistrict = p.String(maxLength: 50),
                        Map = p.String(maxLength: 50),
                        IdAddressType = p.Int(),
                        IdCompany = p.Int(),
                        NonProfit = p.Boolean(),
                        IdOwnerContact = p.Int(),
                        Title = p.String(maxLength: 50),
                        IdOccupancyClassification = p.Int(),
                        IsOcupancyClassification20082014 = p.Boolean(),
                        IdConstructionClassification = p.Int(),
                        IsConstructionClassification20082014 = p.Boolean(),
                        IdMultipleDwellingClassification = p.Int(),
                        IdPrimaryStructuralSystem = p.Int(),
                        IdStructureOccupancyCategory = p.Int(),
                        IdSeismicDesignCategory = p.Int(),
                        Stories = p.Int(),
                        Height = p.Int(),
                        Feet = p.Int(),
                        DwellingUnits = p.Int(),
                        GrossArea = p.Int(),
                        StreetLegalWidth = p.Int(),
                        IsLandmark = p.Boolean(),
                        IsLittleE = p.Boolean(),
                        TidalWetlandsMapCheck = p.Boolean(),
                        FreshwaterWetlandsMapCheck = p.Boolean(),
                        CoastalErosionHazardAreaMapCheck = p.Boolean(),
                        SpecialFloodHazardAreaCheck = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[RfpAddresses]([IdBorough], [HouseNumber], [Street], [ZipCode], [Block], [Lot], [BinNumber], [ComunityBoardNumber], [ZoneDistrict], [Overlay], [SpecialDistrict], [Map], [IdAddressType], [IdCompany], [NonProfit], [IdOwnerContact], [Title], [IdOccupancyClassification], [IsOcupancyClassification20082014], [IdConstructionClassification], [IsConstructionClassification20082014], [IdMultipleDwellingClassification], [IdPrimaryStructuralSystem], [IdStructureOccupancyCategory], [IdSeismicDesignCategory], [Stories], [Height], [Feet], [DwellingUnits], [GrossArea], [StreetLegalWidth], [IsLandmark], [IsLittleE], [TidalWetlandsMapCheck], [FreshwaterWetlandsMapCheck], [CoastalErosionHazardAreaMapCheck], [SpecialFloodHazardAreaCheck], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@IdBorough, @HouseNumber, @Street, @ZipCode, @Block, @Lot, @BinNumber, @ComunityBoardNumber, @ZoneDistrict, @Overlay, @SpecialDistrict, @Map, @IdAddressType, @IdCompany, @NonProfit, @IdOwnerContact, @Title, @IdOccupancyClassification, @IsOcupancyClassification20082014, @IdConstructionClassification, @IsConstructionClassification20082014, @IdMultipleDwellingClassification, @IdPrimaryStructuralSystem, @IdStructureOccupancyCategory, @IdSeismicDesignCategory, @Stories, @Height, @Feet, @DwellingUnits, @GrossArea, @StreetLegalWidth, @IsLandmark, @IsLittleE, @TidalWetlandsMapCheck, @FreshwaterWetlandsMapCheck, @CoastalErosionHazardAreaMapCheck, @SpecialFloodHazardAreaCheck, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[RfpAddresses]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[RfpAddresses] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.RfpAddress_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdBorough = p.Int(),
                        HouseNumber = p.String(maxLength: 10),
                        Street = p.String(maxLength: 50),
                        ZipCode = p.String(maxLength: 5),
                        Block = p.String(maxLength: 50),
                        Lot = p.String(maxLength: 50),
                        BinNumber = p.String(maxLength: 50),
                        ComunityBoardNumber = p.String(maxLength: 50),
                        ZoneDistrict = p.String(maxLength: 50),
                        Overlay = p.String(maxLength: 50),
                        SpecialDistrict = p.String(maxLength: 50),
                        Map = p.String(maxLength: 50),
                        IdAddressType = p.Int(),
                        IdCompany = p.Int(),
                        NonProfit = p.Boolean(),
                        IdOwnerContact = p.Int(),
                        Title = p.String(maxLength: 50),
                        IdOccupancyClassification = p.Int(),
                        IsOcupancyClassification20082014 = p.Boolean(),
                        IdConstructionClassification = p.Int(),
                        IsConstructionClassification20082014 = p.Boolean(),
                        IdMultipleDwellingClassification = p.Int(),
                        IdPrimaryStructuralSystem = p.Int(),
                        IdStructureOccupancyCategory = p.Int(),
                        IdSeismicDesignCategory = p.Int(),
                        Stories = p.Int(),
                        Height = p.Int(),
                        Feet = p.Int(),
                        DwellingUnits = p.Int(),
                        GrossArea = p.Int(),
                        StreetLegalWidth = p.Int(),
                        IsLandmark = p.Boolean(),
                        IsLittleE = p.Boolean(),
                        TidalWetlandsMapCheck = p.Boolean(),
                        FreshwaterWetlandsMapCheck = p.Boolean(),
                        CoastalErosionHazardAreaMapCheck = p.Boolean(),
                        SpecialFloodHazardAreaCheck = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[RfpAddresses]
                      SET [IdBorough] = @IdBorough, [HouseNumber] = @HouseNumber, [Street] = @Street, [ZipCode] = @ZipCode, [Block] = @Block, [Lot] = @Lot, [BinNumber] = @BinNumber, [ComunityBoardNumber] = @ComunityBoardNumber, [ZoneDistrict] = @ZoneDistrict, [Overlay] = @Overlay, [SpecialDistrict] = @SpecialDistrict, [Map] = @Map, [IdAddressType] = @IdAddressType, [IdCompany] = @IdCompany, [NonProfit] = @NonProfit, [IdOwnerContact] = @IdOwnerContact, [Title] = @Title, [IdOccupancyClassification] = @IdOccupancyClassification, [IsOcupancyClassification20082014] = @IsOcupancyClassification20082014, [IdConstructionClassification] = @IdConstructionClassification, [IsConstructionClassification20082014] = @IsConstructionClassification20082014, [IdMultipleDwellingClassification] = @IdMultipleDwellingClassification, [IdPrimaryStructuralSystem] = @IdPrimaryStructuralSystem, [IdStructureOccupancyCategory] = @IdStructureOccupancyCategory, [IdSeismicDesignCategory] = @IdSeismicDesignCategory, [Stories] = @Stories, [Height] = @Height, [Feet] = @Feet, [DwellingUnits] = @DwellingUnits, [GrossArea] = @GrossArea, [StreetLegalWidth] = @StreetLegalWidth, [IsLandmark] = @IsLandmark, [IsLittleE] = @IsLittleE, [TidalWetlandsMapCheck] = @TidalWetlandsMapCheck, [FreshwaterWetlandsMapCheck] = @FreshwaterWetlandsMapCheck, [CoastalErosionHazardAreaMapCheck] = @CoastalErosionHazardAreaMapCheck, [SpecialFloodHazardAreaCheck] = @SpecialFloodHazardAreaCheck, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.OccupancyClassification_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[OccupancyClassifications]([Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Description, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[OccupancyClassifications]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[OccupancyClassifications] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.OccupancyClassification_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[OccupancyClassifications]
                      SET [Description] = @Description, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.SeismicDesignCategory_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[SeismicDesignCategories]([Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Description, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[SeismicDesignCategories]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[SeismicDesignCategories] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.SeismicDesignCategory_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[SeismicDesignCategories]
                      SET [Description] = @Description, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Task_Insert",
                p => new
                    {
                        AssignedDate = p.DateTime(),
                        IdAssignedTo = p.Int(),
                        IdAssignedBy = p.Int(),
                        IdTaskType = p.Int(),
                        CompleteBy = p.DateTime(),
                        IdTaskStatus = p.Int(),
                        GeneralNotes = p.String(),
                        IdJobApplication = p.Int(),
                        IdWorkPermitType = p.Int(),
                        IdJob = p.Int(),
                        IdRfp = p.Int(),
                        IdContact = p.Int(),
                        IdCompany = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        ClosedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        IdExaminer = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Tasks]([AssignedDate], [IdAssignedTo], [IdAssignedBy], [IdTaskType], [CompleteBy], [IdTaskStatus], [GeneralNotes], [IdJobApplication], [IdWorkPermitType], [IdJob], [IdRfp], [IdContact], [IdCompany], [LastModifiedDate], [ClosedDate], [LastModifiedBy], [IdExaminer], [CreatedBy], [CreatedDate])
                      VALUES (@AssignedDate, @IdAssignedTo, @IdAssignedBy, @IdTaskType, @CompleteBy, @IdTaskStatus, @GeneralNotes, @IdJobApplication, @IdWorkPermitType, @IdJob, @IdRfp, @IdContact, @IdCompany, @LastModifiedDate, @ClosedDate, @LastModifiedBy, @IdExaminer, @CreatedBy, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Tasks]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Tasks] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Task_Update",
                p => new
                    {
                        Id = p.Int(),
                        AssignedDate = p.DateTime(),
                        IdAssignedTo = p.Int(),
                        IdAssignedBy = p.Int(),
                        IdTaskType = p.Int(),
                        CompleteBy = p.DateTime(),
                        IdTaskStatus = p.Int(),
                        GeneralNotes = p.String(),
                        IdJobApplication = p.Int(),
                        IdWorkPermitType = p.Int(),
                        IdJob = p.Int(),
                        IdRfp = p.Int(),
                        IdContact = p.Int(),
                        IdCompany = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        ClosedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        IdExaminer = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Tasks]
                      SET [AssignedDate] = @AssignedDate, [IdAssignedTo] = @IdAssignedTo, [IdAssignedBy] = @IdAssignedBy, [IdTaskType] = @IdTaskType, [CompleteBy] = @CompleteBy, [IdTaskStatus] = @IdTaskStatus, [GeneralNotes] = @GeneralNotes, [IdJobApplication] = @IdJobApplication, [IdWorkPermitType] = @IdWorkPermitType, [IdJob] = @IdJob, [IdRfp] = @IdRfp, [IdContact] = @IdContact, [IdCompany] = @IdCompany, [LastModifiedDate] = @LastModifiedDate, [ClosedDate] = @ClosedDate, [LastModifiedBy] = @LastModifiedBy, [IdExaminer] = @IdExaminer, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.TaskNote_Insert",
                p => new
                    {
                        IdTask = p.Int(),
                        Notes = p.String(),
                        LastModifiedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[TaskNotes]([IdTask], [Notes], [LastModifiedDate], [LastModifiedBy], [CreatedBy], [CreatedDate])
                      VALUES (@IdTask, @Notes, @LastModifiedDate, @LastModifiedBy, @CreatedBy, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[TaskNotes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[TaskNotes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.TaskNote_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdTask = p.Int(),
                        Notes = p.String(),
                        LastModifiedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[TaskNotes]
                      SET [IdTask] = @IdTask, [Notes] = @Notes, [LastModifiedDate] = @LastModifiedDate, [LastModifiedBy] = @LastModifiedBy, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.ReferenceDocument_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 500),
                        Keywords = p.String(maxLength: 4000),
                        Description = p.String(maxLength: 4000),
                        FileName = p.String(maxLength: 500),
                        ContentPath = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[ReferenceDocuments]([Name], [Keywords], [Description], [FileName], [ContentPath], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @Keywords, @Description, @FileName, @ContentPath, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ReferenceDocuments]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ReferenceDocuments] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.ReferenceDocument_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 500),
                        Keywords = p.String(maxLength: 4000),
                        Description = p.String(maxLength: 4000),
                        FileName = p.String(maxLength: 500),
                        ContentPath = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[ReferenceDocuments]
                      SET [Name] = @Name, [Keywords] = @Keywords, [Description] = @Description, [FileName] = @FileName, [ContentPath] = @ContentPath, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.ReferenceLink_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        Url = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[ReferenceLinks]([Name], [Url], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @Url, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ReferenceLinks]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ReferenceLinks] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.ReferenceLink_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        Url = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[ReferenceLinks]
                      SET [Name] = @Name, [Url] = @Url, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.TransmissionType_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[TransmissionTypes]([Name], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[TransmissionTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[TransmissionTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.TransmissionType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[TransmissionTypes]
                      SET [Name] = @Name, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Verbiage_Insert",
                p => new
                    {
                        Name = p.String(),
                        Content = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Verbiages]([Name], [Content], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @Content, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Verbiages]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Verbiages] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Verbiage_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(),
                        Content = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Verbiages]
                      SET [Name] = @Name, [Content] = @Content, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Verbiages", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.Verbiages", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.TaskReminders", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.TransmissionTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.TransmissionTypes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.ReferenceLinks", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.ReferenceLinks", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.ReferenceDocuments", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.ReferenceDocuments", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.JobApplications", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobApplicationWorkPermitTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobApplicationWorkPermitTypes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.TaskTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.TaskTypes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.TaskNotes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.Tasks", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.JobScopes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpStatus", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpStatus", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.SeismicDesignCategories", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.SeismicDesignCategories", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.OccupancyClassifications", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.OccupancyClassifications", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpAddresses", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpAddresses", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.Milestones", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.Milestones", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.WorkTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.WorkTypes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.JobTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobTypes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.JobMilestones", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.Jobs", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobApplicationTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobWorkTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobWorkTypes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.JobApplicationTypes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.JobDocuments", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobDocuments", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.Jobs", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.JobContacts", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobContactTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobContactTypes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.JobContacts", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.JobApplications", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.EmailTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.EmailTypes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.Cities", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.Cities", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.Boroughs", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.Boroughs", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.ApplicationStatus", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.ApplicationStatus", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.Addresses", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.Addresses", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.Suffixes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.Suffixes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.Prefixes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.Prefixes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.Contacts", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.ContactDocuments", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.ContactDocuments", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.Contacts", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.ContactTitles", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.ContactTitles", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.ContactLicenses", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.ContactLicenses", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.ContactLicenseTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.ContactLicenseTypes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.Companies", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.Companies", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.CompanyTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.CompanyTypes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.AddressTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.AddressTypes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.States", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.States", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.EmployeeDocuments", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.EmployeeDocuments", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.AgentCertificates", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.DocumentTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.DocumentTypes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.AgentCertificates", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.Verbiages", new[] { "LastModifiedBy" });
            DropIndex("dbo.Verbiages", new[] { "CreatedBy" });
            DropIndex("dbo.TaskReminders", new[] { "CreatedBy" });
            DropIndex("dbo.TransmissionTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.TransmissionTypes", new[] { "CreatedBy" });
            DropIndex("dbo.ReferenceLinks", new[] { "LastModifiedBy" });
            DropIndex("dbo.ReferenceLinks", new[] { "CreatedBy" });
            DropIndex("dbo.ReferenceDocuments", new[] { "LastModifiedBy" });
            DropIndex("dbo.ReferenceDocuments", new[] { "CreatedBy" });
            DropIndex("dbo.JobApplicationWorkPermitTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobApplicationWorkPermitTypes", new[] { "CreatedBy" });
            DropIndex("dbo.TaskTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.TaskTypes", new[] { "CreatedBy" });
            DropIndex("dbo.TaskNotes", new[] { "CreatedBy" });
            DropIndex("dbo.Tasks", new[] { "CreatedBy" });
            DropIndex("dbo.JobScopes", new[] { "CreatedBy" });
            DropIndex("dbo.RfpStatus", new[] { "LastModifiedBy" });
            DropIndex("dbo.RfpStatus", new[] { "CreatedBy" });
            DropIndex("dbo.SeismicDesignCategories", new[] { "LastModifiedBy" });
            DropIndex("dbo.SeismicDesignCategories", new[] { "CreatedBy" });
            DropIndex("dbo.OccupancyClassifications", new[] { "LastModifiedBy" });
            DropIndex("dbo.OccupancyClassifications", new[] { "CreatedBy" });
            DropIndex("dbo.RfpAddresses", new[] { "LastModifiedBy" });
            DropIndex("dbo.RfpAddresses", new[] { "CreatedBy" });
            DropIndex("dbo.Milestones", new[] { "LastModifiedBy" });
            DropIndex("dbo.Milestones", new[] { "CreatedBy" });
            DropIndex("dbo.WorkTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.WorkTypes", new[] { "CreatedBy" });
            DropIndex("dbo.JobTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobTypes", new[] { "CreatedBy" });
            DropIndex("dbo.JobMilestones", new[] { "CreatedBy" });
            DropIndex("dbo.JobWorkTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobWorkTypes", new[] { "CreatedBy" });
            DropIndex("dbo.JobApplicationTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobApplicationTypes", new[] { "CreatedBy" });
            DropIndex("dbo.JobDocuments", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobDocuments", new[] { "CreatedBy" });
            DropIndex("dbo.JobContactTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobContactTypes", new[] { "CreatedBy" });
            DropIndex("dbo.JobContacts", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobContacts", new[] { "CreatedBy" });
            DropIndex("dbo.Jobs", new[] { "LastModifiedBy" });
            DropIndex("dbo.Jobs", new[] { "CreatedBy" });
            DropIndex("dbo.JobApplications", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobApplications", new[] { "CreatedBy" });
            DropIndex("dbo.EmailTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.EmailTypes", new[] { "CreatedBy" });
            DropIndex("dbo.Cities", new[] { "LastModifiedBy" });
            DropIndex("dbo.Cities", new[] { "CreatedBy" });
            DropIndex("dbo.Boroughs", new[] { "LastModifiedBy" });
            DropIndex("dbo.Boroughs", new[] { "CreatedBy" });
            DropIndex("dbo.ApplicationStatus", new[] { "LastModifiedBy" });
            DropIndex("dbo.ApplicationStatus", new[] { "CreatedBy" });
            DropIndex("dbo.Suffixes", new[] { "LastModifiedBy" });
            DropIndex("dbo.Suffixes", new[] { "CreatedBy" });
            DropIndex("dbo.Prefixes", new[] { "LastModifiedBy" });
            DropIndex("dbo.Prefixes", new[] { "CreatedBy" });
            DropIndex("dbo.ContactDocuments", new[] { "LastModifiedBy" });
            DropIndex("dbo.ContactDocuments", new[] { "CreatedBy" });
            DropIndex("dbo.ContactTitles", new[] { "LastModifiedBy" });
            DropIndex("dbo.ContactTitles", new[] { "CreatedBy" });
            DropIndex("dbo.ContactLicenseTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.ContactLicenseTypes", new[] { "CreatedBy" });
            DropIndex("dbo.ContactLicenses", new[] { "LastModifiedBy" });
            DropIndex("dbo.ContactLicenses", new[] { "CreatedBy" });
            DropIndex("dbo.Contacts", new[] { "LastModifiedBy" });
            DropIndex("dbo.Contacts", new[] { "CreatedBy" });
            DropIndex("dbo.CompanyTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.CompanyTypes", new[] { "CreatedBy" });
            DropIndex("dbo.Companies", new[] { "LastModifiedBy" });
            DropIndex("dbo.Companies", new[] { "CreatedBy" });
            DropIndex("dbo.States", new[] { "LastModifiedBy" });
            DropIndex("dbo.States", new[] { "CreatedBy" });
            DropIndex("dbo.EmployeeDocuments", new[] { "LastModifiedBy" });
            DropIndex("dbo.EmployeeDocuments", new[] { "CreatedBy" });
            DropIndex("dbo.DocumentTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.DocumentTypes", new[] { "CreatedBy" });
            DropIndex("dbo.AgentCertificates", new[] { "LastModifiedBy" });
            DropIndex("dbo.AgentCertificates", new[] { "CreatedBy" });
            DropIndex("dbo.AddressTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.AddressTypes", new[] { "CreatedBy" });
            DropIndex("dbo.Addresses", new[] { "LastModifiedBy" });
            DropIndex("dbo.Addresses", new[] { "CreatedBy" });
            AlterColumn("dbo.TaskReminders", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.TaskNotes", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.Tasks", "LastModifiedDate", c => c.DateTime());
            DropColumn("dbo.Verbiages", "LastModifiedDate");
            DropColumn("dbo.Verbiages", "LastModifiedBy");
            DropColumn("dbo.Verbiages", "CreatedDate");
            DropColumn("dbo.Verbiages", "CreatedBy");
            DropColumn("dbo.TaskReminders", "CreatedDate");
            DropColumn("dbo.TaskReminders", "CreatedBy");
            DropColumn("dbo.TransmissionTypes", "LastModifiedDate");
            DropColumn("dbo.TransmissionTypes", "LastModifiedBy");
            DropColumn("dbo.TransmissionTypes", "CreatedDate");
            DropColumn("dbo.TransmissionTypes", "CreatedBy");
            DropColumn("dbo.ReferenceLinks", "LastModifiedDate");
            DropColumn("dbo.ReferenceLinks", "LastModifiedBy");
            DropColumn("dbo.ReferenceLinks", "CreatedDate");
            DropColumn("dbo.ReferenceLinks", "CreatedBy");
            DropColumn("dbo.ReferenceDocuments", "LastModifiedDate");
            DropColumn("dbo.ReferenceDocuments", "LastModifiedBy");
            DropColumn("dbo.ReferenceDocuments", "CreatedDate");
            DropColumn("dbo.ReferenceDocuments", "CreatedBy");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "LastModifiedBy");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "CreatedDate");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "CreatedBy");
            DropColumn("dbo.TaskTypes", "LastModifiedDate");
            DropColumn("dbo.TaskTypes", "LastModifiedBy");
            DropColumn("dbo.TaskTypes", "CreatedDate");
            DropColumn("dbo.TaskTypes", "CreatedBy");
            DropColumn("dbo.TaskNotes", "CreatedDate");
            DropColumn("dbo.TaskNotes", "CreatedBy");
            DropColumn("dbo.Tasks", "CreatedDate");
            DropColumn("dbo.Tasks", "CreatedBy");
            DropColumn("dbo.JobScopes", "CreatedDate");
            DropColumn("dbo.JobScopes", "CreatedBy");
            DropColumn("dbo.RfpStatus", "LastModifiedDate");
            DropColumn("dbo.RfpStatus", "LastModifiedBy");
            DropColumn("dbo.RfpStatus", "CreatedDate");
            DropColumn("dbo.RfpStatus", "CreatedBy");
            DropColumn("dbo.SeismicDesignCategories", "LastModifiedDate");
            DropColumn("dbo.SeismicDesignCategories", "LastModifiedBy");
            DropColumn("dbo.SeismicDesignCategories", "CreatedDate");
            DropColumn("dbo.SeismicDesignCategories", "CreatedBy");
            DropColumn("dbo.OccupancyClassifications", "LastModifiedDate");
            DropColumn("dbo.OccupancyClassifications", "LastModifiedBy");
            DropColumn("dbo.OccupancyClassifications", "CreatedDate");
            DropColumn("dbo.OccupancyClassifications", "CreatedBy");
            DropColumn("dbo.RfpAddresses", "LastModifiedDate");
            DropColumn("dbo.RfpAddresses", "LastModifiedBy");
            DropColumn("dbo.RfpAddresses", "CreatedDate");
            DropColumn("dbo.RfpAddresses", "CreatedBy");
            DropColumn("dbo.Milestones", "LastModifiedDate");
            DropColumn("dbo.Milestones", "LastModifiedBy");
            DropColumn("dbo.Milestones", "CreatedDate");
            DropColumn("dbo.Milestones", "CreatedBy");
            DropColumn("dbo.WorkTypes", "LastModifiedDate");
            DropColumn("dbo.WorkTypes", "LastModifiedBy");
            DropColumn("dbo.WorkTypes", "CreatedDate");
            DropColumn("dbo.WorkTypes", "CreatedBy");
            DropColumn("dbo.JobTypes", "LastModifiedDate");
            DropColumn("dbo.JobTypes", "LastModifiedBy");
            DropColumn("dbo.JobTypes", "CreatedDate");
            DropColumn("dbo.JobTypes", "CreatedBy");
            DropColumn("dbo.JobMilestones", "CreatedDate");
            DropColumn("dbo.JobMilestones", "CreatedBy");
            DropColumn("dbo.JobWorkTypes", "LastModifiedDate");
            DropColumn("dbo.JobWorkTypes", "LastModifiedBy");
            DropColumn("dbo.JobWorkTypes", "CreatedDate");
            DropColumn("dbo.JobWorkTypes", "CreatedBy");
            DropColumn("dbo.JobApplicationTypes", "LastModifiedDate");
            DropColumn("dbo.JobApplicationTypes", "LastModifiedBy");
            DropColumn("dbo.JobApplicationTypes", "CreatedDate");
            DropColumn("dbo.JobApplicationTypes", "CreatedBy");
            DropColumn("dbo.JobDocuments", "LastModifiedDate");
            DropColumn("dbo.JobDocuments", "LastModifiedBy");
            DropColumn("dbo.JobDocuments", "CreatedDate");
            DropColumn("dbo.JobDocuments", "CreatedBy");
            DropColumn("dbo.JobContactTypes", "LastModifiedDate");
            DropColumn("dbo.JobContactTypes", "LastModifiedBy");
            DropColumn("dbo.JobContactTypes", "CreatedDate");
            DropColumn("dbo.JobContactTypes", "CreatedBy");
            DropColumn("dbo.JobContacts", "LastModifiedDate");
            DropColumn("dbo.JobContacts", "LastModifiedBy");
            DropColumn("dbo.JobContacts", "CreatedDate");
            DropColumn("dbo.JobContacts", "CreatedBy");
            DropColumn("dbo.Jobs", "LastModifiedBy");
            DropColumn("dbo.Jobs", "CreatedDate");
            DropColumn("dbo.Jobs", "CreatedBy");
            DropColumn("dbo.JobApplications", "LastModifiedBy");
            DropColumn("dbo.JobApplications", "CreatedDate");
            DropColumn("dbo.JobApplications", "CreatedBy");
            DropColumn("dbo.EmailTypes", "LastModifiedDate");
            DropColumn("dbo.EmailTypes", "LastModifiedBy");
            DropColumn("dbo.EmailTypes", "CreatedDate");
            DropColumn("dbo.EmailTypes", "CreatedBy");
            DropColumn("dbo.Cities", "LastModifiedDate");
            DropColumn("dbo.Cities", "LastModifiedBy");
            DropColumn("dbo.Cities", "CreatedDate");
            DropColumn("dbo.Cities", "CreatedBy");
            DropColumn("dbo.Boroughs", "LastModifiedDate");
            DropColumn("dbo.Boroughs", "LastModifiedBy");
            DropColumn("dbo.Boroughs", "CreatedDate");
            DropColumn("dbo.Boroughs", "CreatedBy");
            DropColumn("dbo.ApplicationStatus", "LastModifiedDate");
            DropColumn("dbo.ApplicationStatus", "LastModifiedBy");
            DropColumn("dbo.ApplicationStatus", "CreatedDate");
            DropColumn("dbo.ApplicationStatus", "CreatedBy");
            DropColumn("dbo.EmployeeDocuments", "LastModifiedDate");
            DropColumn("dbo.EmployeeDocuments", "LastModifiedBy");
            DropColumn("dbo.EmployeeDocuments", "CreatedDate");
            DropColumn("dbo.EmployeeDocuments", "CreatedBy");
            DropColumn("dbo.DocumentTypes", "LastModifiedDate");
            DropColumn("dbo.DocumentTypes", "LastModifiedBy");
            DropColumn("dbo.DocumentTypes", "CreatedDate");
            DropColumn("dbo.DocumentTypes", "CreatedBy");
            DropColumn("dbo.AgentCertificates", "LastModifiedDate");
            DropColumn("dbo.AgentCertificates", "LastModifiedBy");
            DropColumn("dbo.AgentCertificates", "CreatedDate");
            DropColumn("dbo.AgentCertificates", "CreatedBy");
            DropColumn("dbo.States", "LastModifiedDate");
            DropColumn("dbo.States", "LastModifiedBy");
            DropColumn("dbo.States", "CreatedDate");
            DropColumn("dbo.States", "CreatedBy");
            DropColumn("dbo.Suffixes", "LastModifiedDate");
            DropColumn("dbo.Suffixes", "LastModifiedBy");
            DropColumn("dbo.Suffixes", "CreatedDate");
            DropColumn("dbo.Suffixes", "CreatedBy");
            DropColumn("dbo.Prefixes", "LastModifiedDate");
            DropColumn("dbo.Prefixes", "LastModifiedBy");
            DropColumn("dbo.Prefixes", "CreatedDate");
            DropColumn("dbo.Prefixes", "CreatedBy");
            DropColumn("dbo.ContactDocuments", "LastModifiedDate");
            DropColumn("dbo.ContactDocuments", "LastModifiedBy");
            DropColumn("dbo.ContactDocuments", "CreatedDate");
            DropColumn("dbo.ContactDocuments", "CreatedBy");
            DropColumn("dbo.ContactTitles", "LastModifiedDate");
            DropColumn("dbo.ContactTitles", "LastModifiedBy");
            DropColumn("dbo.ContactTitles", "CreatedDate");
            DropColumn("dbo.ContactTitles", "CreatedBy");
            DropColumn("dbo.ContactLicenseTypes", "LastModifiedDate");
            DropColumn("dbo.ContactLicenseTypes", "LastModifiedBy");
            DropColumn("dbo.ContactLicenseTypes", "CreatedDate");
            DropColumn("dbo.ContactLicenseTypes", "CreatedBy");
            DropColumn("dbo.ContactLicenses", "LastModifiedDate");
            DropColumn("dbo.ContactLicenses", "LastModifiedBy");
            DropColumn("dbo.ContactLicenses", "CreatedDate");
            DropColumn("dbo.ContactLicenses", "CreatedBy");
            DropColumn("dbo.Contacts", "LastModifiedDate");
            DropColumn("dbo.Contacts", "LastModifiedBy");
            DropColumn("dbo.Contacts", "CreatedDate");
            DropColumn("dbo.Contacts", "CreatedBy");
            DropColumn("dbo.CompanyTypes", "LastModifiedDate");
            DropColumn("dbo.CompanyTypes", "LastModifiedBy");
            DropColumn("dbo.CompanyTypes", "CreatedDate");
            DropColumn("dbo.CompanyTypes", "CreatedBy");
            DropColumn("dbo.Companies", "LastModifiedDate");
            DropColumn("dbo.Companies", "LastModifiedBy");
            DropColumn("dbo.Companies", "CreatedDate");
            DropColumn("dbo.Companies", "CreatedBy");
            DropColumn("dbo.AddressTypes", "LastModifiedDate");
            DropColumn("dbo.AddressTypes", "LastModifiedBy");
            DropColumn("dbo.AddressTypes", "CreatedDate");
            DropColumn("dbo.AddressTypes", "CreatedBy");
            DropColumn("dbo.Addresses", "LastModifiedDate");
            DropColumn("dbo.Addresses", "LastModifiedBy");
            DropColumn("dbo.Addresses", "CreatedDate");
            DropColumn("dbo.Addresses", "CreatedBy");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
