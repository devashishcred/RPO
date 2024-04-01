namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobTimeNoteHistoryAddedHoursandMinutesChange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobViolationExplanationOfCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdViolation = c.Int(),
                        Code = c.String(),
                        CodeSection = c.String(),
                        Description = c.String(),
                        FaceAmount = c.String(),
                        IsFromAuth = c.Boolean(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.JobViolations", t => t.IdViolation)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.IdViolation)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            AddColumn("dbo.JobTimeNoteHistories", "TimeHours", c => c.Short());
            AddColumn("dbo.JobTimeNoteHistories", "TimeMinutes", c => c.Short());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobViolationExplanationOfCharges", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobViolationExplanationOfCharges", "IdViolation", "dbo.JobViolations");
            DropForeignKey("dbo.JobViolationExplanationOfCharges", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.JobViolationExplanationOfCharges", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobViolationExplanationOfCharges", new[] { "CreatedBy" });
            DropIndex("dbo.JobViolationExplanationOfCharges", new[] { "IdViolation" });
            DropColumn("dbo.JobTimeNoteHistories", "TimeMinutes");
            DropColumn("dbo.JobTimeNoteHistories", "TimeHours");
            DropTable("dbo.JobViolationExplanationOfCharges");
        }
    }
}
