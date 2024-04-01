namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJobTrasmittalTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobTransmittalAttachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJobTransmittal = c.Int(),
                        DocumentPath = c.String(maxLength: 500),
                        Name = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobTransmittals", t => t.IdJobTransmittal)
                .Index(t => t.IdJobTransmittal);
            
            CreateTable(
                "dbo.JobTransmittalCCs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJobTransmittal = c.Int(),
                        IdContact = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.IdContact)
                .ForeignKey("dbo.JobTransmittals", t => t.IdJobTransmittal)
                .Index(t => t.IdJobTransmittal)
                .Index(t => t.IdContact);
            
            AddColumn("dbo.JobTransmittals", "TransmittalNumber", c => c.String());
            AddColumn("dbo.JobTransmittals", "IdFromEmployee", c => c.Int());
            AddColumn("dbo.JobTransmittals", "IdToCompany", c => c.Int());
            AddColumn("dbo.JobTransmittals", "IdContactAttention", c => c.Int());
            AddColumn("dbo.JobTransmittals", "IdTransmissionType", c => c.Int());
            AddColumn("dbo.JobTransmittals", "IdEmailType", c => c.Int());
            AddColumn("dbo.JobTransmittals", "SentDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.JobTransmittals", "IdSentBy", c => c.Int());
            AddColumn("dbo.JobTransmittals", "EmailMessage", c => c.String());
            AddColumn("dbo.JobTransmittals", "EmailSubject", c => c.String(maxLength: 100));
            AddColumn("dbo.JobTransmittals", "IsAdditionalAtttachment", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobTransmittals", "IsEmailSent", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobTransmittals", "CreatedBy", c => c.Int());
            AddColumn("dbo.JobTransmittals", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.JobTransmittals", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.JobTransmittals", "LastModifiedDate", c => c.DateTime());
            CreateIndex("dbo.JobTransmittals", "IdFromEmployee");
            CreateIndex("dbo.JobTransmittals", "IdToCompany");
            CreateIndex("dbo.JobTransmittals", "IdContactAttention");
            CreateIndex("dbo.JobTransmittals", "IdTransmissionType");
            CreateIndex("dbo.JobTransmittals", "IdEmailType");
            CreateIndex("dbo.JobTransmittals", "IdSentBy");
            CreateIndex("dbo.JobTransmittals", "CreatedBy");
            CreateIndex("dbo.JobTransmittals", "LastModifiedBy");
            AddForeignKey("dbo.JobTransmittals", "IdContactAttention", "dbo.Contacts", "Id");
            AddForeignKey("dbo.JobTransmittals", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobTransmittals", "IdEmailType", "dbo.EmailTypes", "Id");
            AddForeignKey("dbo.JobTransmittals", "IdFromEmployee", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobTransmittals", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobTransmittals", "IdSentBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobTransmittals", "IdToCompany", "dbo.Companies", "Id");
            AddForeignKey("dbo.JobTransmittals", "IdTransmissionType", "dbo.TransmissionTypes", "Id");
            DropColumn("dbo.JobTransmittals", "Number");
            DropColumn("dbo.JobTransmittals", "Date");
            DropColumn("dbo.JobTransmittals", "Recipient");
            DropColumn("dbo.JobTransmittals", "Sender");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobTransmittals", "Sender", c => c.String());
            AddColumn("dbo.JobTransmittals", "Recipient", c => c.String());
            AddColumn("dbo.JobTransmittals", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.JobTransmittals", "Number", c => c.Int(nullable: false));
            DropForeignKey("dbo.JobTransmittals", "IdTransmissionType", "dbo.TransmissionTypes");
            DropForeignKey("dbo.JobTransmittals", "IdToCompany", "dbo.Companies");
            DropForeignKey("dbo.JobTransmittals", "IdSentBy", "dbo.Employees");
            DropForeignKey("dbo.JobTransmittals", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobTransmittalCCs", "IdJobTransmittal", "dbo.JobTransmittals");
            DropForeignKey("dbo.JobTransmittalCCs", "IdContact", "dbo.Contacts");
            DropForeignKey("dbo.JobTransmittalAttachments", "IdJobTransmittal", "dbo.JobTransmittals");
            DropForeignKey("dbo.JobTransmittals", "IdFromEmployee", "dbo.Employees");
            DropForeignKey("dbo.JobTransmittals", "IdEmailType", "dbo.EmailTypes");
            DropForeignKey("dbo.JobTransmittals", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.JobTransmittals", "IdContactAttention", "dbo.Contacts");
            DropIndex("dbo.JobTransmittalCCs", new[] { "IdContact" });
            DropIndex("dbo.JobTransmittalCCs", new[] { "IdJobTransmittal" });
            DropIndex("dbo.JobTransmittalAttachments", new[] { "IdJobTransmittal" });
            DropIndex("dbo.JobTransmittals", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobTransmittals", new[] { "CreatedBy" });
            DropIndex("dbo.JobTransmittals", new[] { "IdSentBy" });
            DropIndex("dbo.JobTransmittals", new[] { "IdEmailType" });
            DropIndex("dbo.JobTransmittals", new[] { "IdTransmissionType" });
            DropIndex("dbo.JobTransmittals", new[] { "IdContactAttention" });
            DropIndex("dbo.JobTransmittals", new[] { "IdToCompany" });
            DropIndex("dbo.JobTransmittals", new[] { "IdFromEmployee" });
            DropColumn("dbo.JobTransmittals", "LastModifiedDate");
            DropColumn("dbo.JobTransmittals", "LastModifiedBy");
            DropColumn("dbo.JobTransmittals", "CreatedDate");
            DropColumn("dbo.JobTransmittals", "CreatedBy");
            DropColumn("dbo.JobTransmittals", "IsEmailSent");
            DropColumn("dbo.JobTransmittals", "IsAdditionalAtttachment");
            DropColumn("dbo.JobTransmittals", "EmailSubject");
            DropColumn("dbo.JobTransmittals", "EmailMessage");
            DropColumn("dbo.JobTransmittals", "IdSentBy");
            DropColumn("dbo.JobTransmittals", "SentDate");
            DropColumn("dbo.JobTransmittals", "IdEmailType");
            DropColumn("dbo.JobTransmittals", "IdTransmissionType");
            DropColumn("dbo.JobTransmittals", "IdContactAttention");
            DropColumn("dbo.JobTransmittals", "IdToCompany");
            DropColumn("dbo.JobTransmittals", "IdFromEmployee");
            DropColumn("dbo.JobTransmittals", "TransmittalNumber");
            DropTable("dbo.JobTransmittalCCs");
            DropTable("dbo.JobTransmittalAttachments");
        }
    }
}
