namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Jobs2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplications", "IdJobType", c => c.Int(nullable: false));
            AddColumn("dbo.JobApplications", "ApplicationNumber", c => c.String());
            AddColumn("dbo.JobApplications", "For", c => c.String());
            AddColumn("dbo.JobApplications", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.JobApplications", "Floor", c => c.String());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "Code", c => c.String());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "EstimatedCost", c => c.String());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "Withdrawn", c => c.DateTime());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "Filed", c => c.DateTime());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "Issued", c => c.DateTime());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "Expires", c => c.DateTime());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "SignedOff", c => c.DateTime());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "CompanyResponsible", c => c.String());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "PersonalResponsible", c => c.String());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "WorkDescription", c => c.String());
            AddColumn("dbo.JobTasks", "Assigned", c => c.DateTime());
            AddColumn("dbo.JobTasks", "IdTo", c => c.Int(nullable: false));
            AddColumn("dbo.JobTasks", "IdBy", c => c.Int(nullable: false));
            AddColumn("dbo.JobTasks", "CompleteBy", c => c.DateTime());
            AddColumn("dbo.JobTasks", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.JobTasks", "GeneralNotes", c => c.String());
            AddColumn("dbo.JobTasks", "IdJobApplication", c => c.Int(nullable: false));
            AddColumn("dbo.JobTasks", "IdWorkType", c => c.Int(nullable: false));
            AddColumn("dbo.JobTasks", "ProgressCompletionNote", c => c.String());
            CreateIndex("dbo.JobApplications", "IdJobType");
            CreateIndex("dbo.JobTasks", "IdTo");
            CreateIndex("dbo.JobTasks", "IdBy");
            CreateIndex("dbo.JobTasks", "IdJobApplication");
            CreateIndex("dbo.JobTasks", "IdWorkType");
            AddForeignKey("dbo.JobApplications", "IdJobType", "dbo.JobTypes", "Id");
            AddForeignKey("dbo.JobTasks", "IdBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobTasks", "IdJobApplication", "dbo.JobApplications", "Id");
            AddForeignKey("dbo.JobTasks", "IdTo", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobTasks", "IdWorkType", "dbo.WorkTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobTasks", "IdWorkType", "dbo.WorkTypes");
            DropForeignKey("dbo.JobTasks", "IdTo", "dbo.Employees");
            DropForeignKey("dbo.JobTasks", "IdJobApplication", "dbo.JobApplications");
            DropForeignKey("dbo.JobTasks", "IdBy", "dbo.Employees");
            DropForeignKey("dbo.JobApplications", "IdJobType", "dbo.JobTypes");
            DropIndex("dbo.JobTasks", new[] { "IdWorkType" });
            DropIndex("dbo.JobTasks", new[] { "IdJobApplication" });
            DropIndex("dbo.JobTasks", new[] { "IdBy" });
            DropIndex("dbo.JobTasks", new[] { "IdTo" });
            DropIndex("dbo.JobApplications", new[] { "IdJobType" });
            DropColumn("dbo.JobTasks", "ProgressCompletionNote");
            DropColumn("dbo.JobTasks", "IdWorkType");
            DropColumn("dbo.JobTasks", "IdJobApplication");
            DropColumn("dbo.JobTasks", "GeneralNotes");
            DropColumn("dbo.JobTasks", "Status");
            DropColumn("dbo.JobTasks", "CompleteBy");
            DropColumn("dbo.JobTasks", "IdBy");
            DropColumn("dbo.JobTasks", "IdTo");
            DropColumn("dbo.JobTasks", "Assigned");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "WorkDescription");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "PersonalResponsible");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "CompanyResponsible");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "SignedOff");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "Expires");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "Issued");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "Filed");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "Withdrawn");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "EstimatedCost");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "Code");
            DropColumn("dbo.JobApplications", "Floor");
            DropColumn("dbo.JobApplications", "Status");
            DropColumn("dbo.JobApplications", "For");
            DropColumn("dbo.JobApplications", "ApplicationNumber");
            DropColumn("dbo.JobApplications", "IdJobType");
        }
    }
}
