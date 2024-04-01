namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableForJobViolation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobViolations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJob = c.Int(),
                        SummonsNumber = c.String(nullable: false),
                        DateIssued = c.DateTime(),
                        HearingDate = c.DateTime(),
                        HearingLocation = c.String(),
                        HearingResult = c.String(),
                        StatusOfSummonsNotice = c.String(),
                        RespondentAddress = c.String(),
                        InspectionLocation = c.String(),
                        BalanceDue = c.Double(nullable: false),
                        RespondentName = c.String(),
                        IssuingAgency = c.String(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Jobs", t => t.IdJob)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.IdJob)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobViolations", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobViolations", "IdJob", "dbo.Jobs");
            DropForeignKey("dbo.JobViolations", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.JobViolations", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobViolations", new[] { "CreatedBy" });
            DropIndex("dbo.JobViolations", new[] { "IdJob" });
            DropTable("dbo.JobViolations");
        }
    }
}
