namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableforRFPEmail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RFPEmailAttachmentHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdRFPEmailHistory = c.Int(),
                        DocumentPath = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RFPEmailHistories", t => t.IdRFPEmailHistory)
                .Index(t => t.IdRFPEmailHistory);
            
            CreateTable(
                "dbo.RFPEmailCCHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdRFPEmailHistory = c.Int(),
                        IdContact = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.IdContact)
                .ForeignKey("dbo.RFPEmailHistories", t => t.IdRFPEmailHistory)
                .Index(t => t.IdRFPEmailHistory)
                .Index(t => t.IdContact);
            
            CreateTable(
                "dbo.RFPEmailHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdFromEmployee = c.Int(),
                        IdToCompany = c.Int(),
                        IdContactAttention = c.Int(),
                        IdTransmissionType = c.Int(),
                        IdEmailType = c.Int(),
                        SentDate = c.DateTime(nullable: false),
                        IdSentBy = c.Int(),
                        EmailMessage = c.String(),
                        IsAdditionalAtttachment = c.Boolean(nullable: false),
                        IsEmailSent = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.IdContactAttention)
                .ForeignKey("dbo.EmailTypes", t => t.IdEmailType)
                .ForeignKey("dbo.Employees", t => t.IdFromEmployee)
                .ForeignKey("dbo.Employees", t => t.IdSentBy)
                .ForeignKey("dbo.Companies", t => t.IdToCompany)
                .ForeignKey("dbo.TransmissionTypes", t => t.IdTransmissionType)
                .Index(t => t.IdFromEmployee)
                .Index(t => t.IdToCompany)
                .Index(t => t.IdContactAttention)
                .Index(t => t.IdTransmissionType)
                .Index(t => t.IdEmailType)
                .Index(t => t.IdSentBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RFPEmailHistories", "IdTransmissionType", "dbo.TransmissionTypes");
            DropForeignKey("dbo.RFPEmailHistories", "IdToCompany", "dbo.Companies");
            DropForeignKey("dbo.RFPEmailHistories", "IdSentBy", "dbo.Employees");
            DropForeignKey("dbo.RFPEmailCCHistories", "IdRFPEmailHistory", "dbo.RFPEmailHistories");
            DropForeignKey("dbo.RFPEmailAttachmentHistories", "IdRFPEmailHistory", "dbo.RFPEmailHistories");
            DropForeignKey("dbo.RFPEmailHistories", "IdFromEmployee", "dbo.Employees");
            DropForeignKey("dbo.RFPEmailHistories", "IdEmailType", "dbo.EmailTypes");
            DropForeignKey("dbo.RFPEmailHistories", "IdContactAttention", "dbo.Contacts");
            DropForeignKey("dbo.RFPEmailCCHistories", "IdContact", "dbo.Contacts");
            DropIndex("dbo.RFPEmailHistories", new[] { "IdSentBy" });
            DropIndex("dbo.RFPEmailHistories", new[] { "IdEmailType" });
            DropIndex("dbo.RFPEmailHistories", new[] { "IdTransmissionType" });
            DropIndex("dbo.RFPEmailHistories", new[] { "IdContactAttention" });
            DropIndex("dbo.RFPEmailHistories", new[] { "IdToCompany" });
            DropIndex("dbo.RFPEmailHistories", new[] { "IdFromEmployee" });
            DropIndex("dbo.RFPEmailCCHistories", new[] { "IdContact" });
            DropIndex("dbo.RFPEmailCCHistories", new[] { "IdRFPEmailHistory" });
            DropIndex("dbo.RFPEmailAttachmentHistories", new[] { "IdRFPEmailHistory" });
            DropTable("dbo.RFPEmailHistories");
            DropTable("dbo.RFPEmailCCHistories");
            DropTable("dbo.RFPEmailAttachmentHistories");
        }
    }
}
