namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateJobViolationDocumentTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobViolationDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        IdJobViolation = c.Int(nullable: false),
                        DocumentPath = c.String(maxLength: 200),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.JobViolations", t => t.IdJobViolation)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.IdJobViolation)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobViolationDocuments", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobViolationDocuments", "IdJobViolation", "dbo.JobViolations");
            DropForeignKey("dbo.JobViolationDocuments", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.JobViolationDocuments", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobViolationDocuments", new[] { "CreatedBy" });
            DropIndex("dbo.JobViolationDocuments", new[] { "IdJobViolation" });
            DropTable("dbo.JobViolationDocuments");
        }
    }
}
