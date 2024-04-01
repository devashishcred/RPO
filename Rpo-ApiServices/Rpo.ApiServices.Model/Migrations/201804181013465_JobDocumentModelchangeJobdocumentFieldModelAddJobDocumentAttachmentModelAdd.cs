namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobDocumentModelchangeJobdocumentFieldModelAddJobDocumentAttachmentModelAdd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobDocuments", "LastModifiedBy", "dbo.Employees");
            DropIndex("dbo.JobDocuments", new[] { "LastModifiedBy" });
            CreateTable(
                "dbo.JobDocumentAttachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJobDocument = c.Int(nullable: false),
                        DocumentName = c.String(),
                        Path = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobDocuments", t => t.IdJobDocument)
                .Index(t => t.IdJobDocument);
            
            CreateTable(
                "dbo.JobDocumentFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJobDocument = c.Int(nullable: false),
                        IdDocumentField = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocumentFields", t => t.IdDocumentField)
                .ForeignKey("dbo.JobDocuments", t => t.IdJobDocument)
                .Index(t => t.IdJobDocument)
                .Index(t => t.IdDocumentField);
            
            AddColumn("dbo.JobDocuments", "IdDocument", c => c.Int(nullable: false));
            AddColumn("dbo.JobDocuments", "DocumentName", c => c.String());
            AddColumn("dbo.JobDocuments", "DocumentPath", c => c.String());
            AddColumn("dbo.JobDocuments", "IsArchived", c => c.Boolean(nullable: false));
            CreateIndex("dbo.JobDocuments", "IdDocument");
            AddForeignKey("dbo.JobDocuments", "IdDocument", "dbo.DocumentMasters", "Id");
            DropColumn("dbo.JobDocuments", "Name");
            DropColumn("dbo.JobDocuments", "Content");
            DropColumn("dbo.JobDocuments", "LastModifiedBy");
            DropColumn("dbo.JobDocuments", "LastModifiedDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobDocuments", "LastModifiedDate", c => c.DateTime());
            AddColumn("dbo.JobDocuments", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.JobDocuments", "Content", c => c.Binary());
            AddColumn("dbo.JobDocuments", "Name", c => c.String());
            DropForeignKey("dbo.JobDocumentFields", "IdJobDocument", "dbo.JobDocuments");
            DropForeignKey("dbo.JobDocumentFields", "IdDocumentField", "dbo.DocumentFields");
            DropForeignKey("dbo.JobDocumentAttachments", "IdJobDocument", "dbo.JobDocuments");
            DropForeignKey("dbo.JobDocuments", "IdDocument", "dbo.DocumentMasters");
            DropIndex("dbo.JobDocumentFields", new[] { "IdDocumentField" });
            DropIndex("dbo.JobDocumentFields", new[] { "IdJobDocument" });
            DropIndex("dbo.JobDocumentAttachments", new[] { "IdJobDocument" });
            DropIndex("dbo.JobDocuments", new[] { "IdDocument" });
            DropColumn("dbo.JobDocuments", "IsArchived");
            DropColumn("dbo.JobDocuments", "DocumentPath");
            DropColumn("dbo.JobDocuments", "DocumentName");
            DropColumn("dbo.JobDocuments", "IdDocument");
            DropTable("dbo.JobDocumentFields");
            DropTable("dbo.JobDocumentAttachments");
            CreateIndex("dbo.JobDocuments", "LastModifiedBy");
            AddForeignKey("dbo.JobDocuments", "LastModifiedBy", "dbo.Employees", "Id");
        }
    }
}
