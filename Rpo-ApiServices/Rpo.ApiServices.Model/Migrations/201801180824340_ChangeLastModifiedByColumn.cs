namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeLastModifiedByColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplications", "LastModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.JobMilestones", "ModifiedBy", c => c.Int());
            AddColumn("dbo.JobScopes", "ModifiedBy", c => c.Int());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "LastModifiedDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.JobMilestones", "ModifiedBy");
            CreateIndex("dbo.JobScopes", "ModifiedBy");
            AddForeignKey("dbo.JobMilestones", "ModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobScopes", "ModifiedBy", "dbo.Employees", "Id");
            DropColumn("dbo.JobMilestones", "LastModifiedBy");
            DropColumn("dbo.JobScopes", "LastModifiedBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobScopes", "LastModifiedBy", c => c.String());
            AddColumn("dbo.JobMilestones", "LastModifiedBy", c => c.String());
            DropForeignKey("dbo.JobScopes", "ModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobMilestones", "ModifiedBy", "dbo.Employees");
            DropIndex("dbo.JobScopes", new[] { "ModifiedBy" });
            DropIndex("dbo.JobMilestones", new[] { "ModifiedBy" });
            DropColumn("dbo.JobApplicationWorkPermitTypes", "LastModifiedDate");
            DropColumn("dbo.JobScopes", "ModifiedBy");
            DropColumn("dbo.JobMilestones", "ModifiedBy");
            DropColumn("dbo.JobApplications", "LastModifiedDate");
        }
    }
}
