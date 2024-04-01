namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewTablesForSendEmail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyEmailAttachmentHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdCompanyEmailHistory = c.Int(),
                        DocumentPath = c.String(maxLength: 500),
                        Name = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompanyEmailHistories", t => t.IdCompanyEmailHistory)
                .Index(t => t.IdCompanyEmailHistory);
            
            CreateTable(
                "dbo.CompanyEmailCCHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdCompanyEmailHistory = c.Int(),
                        IdContact = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.IdContact)
                .ForeignKey("dbo.CompanyEmailHistories", t => t.IdCompanyEmailHistory)
                .Index(t => t.IdCompanyEmailHistory)
                .Index(t => t.IdContact);
            
            CreateTable(
                "dbo.CompanyEmailHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdCompany = c.Int(),
                        IdFromEmployee = c.Int(),
                        IdToCompany = c.Int(),
                        IdContactAttention = c.Int(),
                        IdTransmissionType = c.Int(),
                        IdEmailType = c.Int(),
                        SentDate = c.DateTime(nullable: false),
                        IdSentBy = c.Int(),
                        EmailMessage = c.String(),
                        EmailSubject = c.String(maxLength: 100),
                        IsAdditionalAtttachment = c.Boolean(nullable: false),
                        IsEmailSent = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.IdCompany)
                .ForeignKey("dbo.Contacts", t => t.IdContactAttention)
                .ForeignKey("dbo.EmailTypes", t => t.IdEmailType)
                .ForeignKey("dbo.Employees", t => t.IdFromEmployee)
                .ForeignKey("dbo.Employees", t => t.IdSentBy)
                .ForeignKey("dbo.Companies", t => t.IdToCompany)
                .ForeignKey("dbo.TransmissionTypes", t => t.IdTransmissionType)
                .Index(t => t.IdCompany)
                .Index(t => t.IdFromEmployee)
                .Index(t => t.IdToCompany)
                .Index(t => t.IdContactAttention)
                .Index(t => t.IdTransmissionType)
                .Index(t => t.IdEmailType)
                .Index(t => t.IdSentBy);
            
            CreateTable(
                "dbo.ContactEmailAttachmentHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdContactEmailHistory = c.Int(),
                        DocumentPath = c.String(maxLength: 500),
                        Name = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContactEmailHistories", t => t.IdContactEmailHistory)
                .Index(t => t.IdContactEmailHistory);
            
            CreateTable(
                "dbo.ContactEmailCCHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdContactEmailHistory = c.Int(),
                        IdContact = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.IdContact)
                .ForeignKey("dbo.ContactEmailHistories", t => t.IdContactEmailHistory)
                .Index(t => t.IdContactEmailHistory)
                .Index(t => t.IdContact);
            
            CreateTable(
                "dbo.ContactEmailHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdContact = c.Int(),
                        IdFromEmployee = c.Int(),
                        IdToCompany = c.Int(),
                        IdContactAttention = c.Int(),
                        IdTransmissionType = c.Int(),
                        IdEmailType = c.Int(),
                        SentDate = c.DateTime(nullable: false),
                        IdSentBy = c.Int(),
                        EmailMessage = c.String(),
                        EmailSubject = c.String(maxLength: 100),
                        IsAdditionalAtttachment = c.Boolean(nullable: false),
                        IsEmailSent = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.IdContact)
                .ForeignKey("dbo.Contacts", t => t.IdContactAttention)
                .ForeignKey("dbo.EmailTypes", t => t.IdEmailType)
                .ForeignKey("dbo.Employees", t => t.IdFromEmployee)
                .ForeignKey("dbo.Employees", t => t.IdSentBy)
                .ForeignKey("dbo.Companies", t => t.IdToCompany)
                .ForeignKey("dbo.TransmissionTypes", t => t.IdTransmissionType)
                .Index(t => t.IdContact)
                .Index(t => t.IdFromEmployee)
                .Index(t => t.IdToCompany)
                .Index(t => t.IdContactAttention)
                .Index(t => t.IdTransmissionType)
                .Index(t => t.IdEmailType)
                .Index(t => t.IdSentBy);
            
            CreateIndex("dbo.AddressTypes", "DisplayOrder", unique: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContactEmailHistories", "IdTransmissionType", "dbo.TransmissionTypes");
            DropForeignKey("dbo.ContactEmailHistories", "IdToCompany", "dbo.Companies");
            DropForeignKey("dbo.ContactEmailHistories", "IdSentBy", "dbo.Employees");
            DropForeignKey("dbo.ContactEmailHistories", "IdFromEmployee", "dbo.Employees");
            DropForeignKey("dbo.ContactEmailHistories", "IdEmailType", "dbo.EmailTypes");
            DropForeignKey("dbo.ContactEmailCCHistories", "IdContactEmailHistory", "dbo.ContactEmailHistories");
            DropForeignKey("dbo.ContactEmailAttachmentHistories", "IdContactEmailHistory", "dbo.ContactEmailHistories");
            DropForeignKey("dbo.ContactEmailHistories", "IdContactAttention", "dbo.Contacts");
            DropForeignKey("dbo.ContactEmailHistories", "IdContact", "dbo.Contacts");
            DropForeignKey("dbo.ContactEmailCCHistories", "IdContact", "dbo.Contacts");
            DropForeignKey("dbo.CompanyEmailHistories", "IdTransmissionType", "dbo.TransmissionTypes");
            DropForeignKey("dbo.CompanyEmailHistories", "IdToCompany", "dbo.Companies");
            DropForeignKey("dbo.CompanyEmailHistories", "IdSentBy", "dbo.Employees");
            DropForeignKey("dbo.CompanyEmailHistories", "IdFromEmployee", "dbo.Employees");
            DropForeignKey("dbo.CompanyEmailHistories", "IdEmailType", "dbo.EmailTypes");
            DropForeignKey("dbo.CompanyEmailHistories", "IdContactAttention", "dbo.Contacts");
            DropForeignKey("dbo.CompanyEmailCCHistories", "IdCompanyEmailHistory", "dbo.CompanyEmailHistories");
            DropForeignKey("dbo.CompanyEmailAttachmentHistories", "IdCompanyEmailHistory", "dbo.CompanyEmailHistories");
            DropForeignKey("dbo.CompanyEmailHistories", "IdCompany", "dbo.Companies");
            DropForeignKey("dbo.CompanyEmailCCHistories", "IdContact", "dbo.Contacts");
            DropIndex("dbo.ContactEmailHistories", new[] { "IdSentBy" });
            DropIndex("dbo.ContactEmailHistories", new[] { "IdEmailType" });
            DropIndex("dbo.ContactEmailHistories", new[] { "IdTransmissionType" });
            DropIndex("dbo.ContactEmailHistories", new[] { "IdContactAttention" });
            DropIndex("dbo.ContactEmailHistories", new[] { "IdToCompany" });
            DropIndex("dbo.ContactEmailHistories", new[] { "IdFromEmployee" });
            DropIndex("dbo.ContactEmailHistories", new[] { "IdContact" });
            DropIndex("dbo.ContactEmailCCHistories", new[] { "IdContact" });
            DropIndex("dbo.ContactEmailCCHistories", new[] { "IdContactEmailHistory" });
            DropIndex("dbo.ContactEmailAttachmentHistories", new[] { "IdContactEmailHistory" });
            DropIndex("dbo.CompanyEmailHistories", new[] { "IdSentBy" });
            DropIndex("dbo.CompanyEmailHistories", new[] { "IdEmailType" });
            DropIndex("dbo.CompanyEmailHistories", new[] { "IdTransmissionType" });
            DropIndex("dbo.CompanyEmailHistories", new[] { "IdContactAttention" });
            DropIndex("dbo.CompanyEmailHistories", new[] { "IdToCompany" });
            DropIndex("dbo.CompanyEmailHistories", new[] { "IdFromEmployee" });
            DropIndex("dbo.CompanyEmailHistories", new[] { "IdCompany" });
            DropIndex("dbo.CompanyEmailCCHistories", new[] { "IdContact" });
            DropIndex("dbo.CompanyEmailCCHistories", new[] { "IdCompanyEmailHistory" });
            DropIndex("dbo.CompanyEmailAttachmentHistories", new[] { "IdCompanyEmailHistory" });
            DropIndex("dbo.AddressTypes", new[] { "DisplayOrder" });
            DropTable("dbo.ContactEmailHistories");
            DropTable("dbo.ContactEmailCCHistories");
            DropTable("dbo.ContactEmailAttachmentHistories");
            DropTable("dbo.CompanyEmailHistories");
            DropTable("dbo.CompanyEmailCCHistories");
            DropTable("dbo.CompanyEmailAttachmentHistories");
        }
    }
}
