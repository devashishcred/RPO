namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobViolationNote : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobViolationNotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJobViolation = c.Int(nullable: false),
                        Notes = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.JobViolations", t => t.IdJobViolation)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.IdJobViolation)
                .Index(t => t.LastModifiedBy)
                .Index(t => t.CreatedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobViolationNotes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobViolationNotes", "IdJobViolation", "dbo.JobViolations");
            DropForeignKey("dbo.JobViolationNotes", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.JobViolationNotes", new[] { "CreatedBy" });
            DropIndex("dbo.JobViolationNotes", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobViolationNotes", new[] { "IdJobViolation" });
            DropTable("dbo.JobViolationNotes");
        }
    }
}
