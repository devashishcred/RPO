namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeForeignKeyinJobApplication : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobApplications", "IdJobType", "dbo.JobTypes");
            DropForeignKey("dbo.JobApplicationWorkPermitTypes", "IdWorkType", "dbo.WorkTypes");
            DropIndex("dbo.JobApplications", new[] { "IdJobType" });
            DropIndex("dbo.JobApplicationWorkPermitTypes", new[] { "IdWorkType" });
            AddColumn("dbo.JobApplications", "IdJobApplicationType", c => c.Int(nullable: false));
            AddColumn("dbo.JobApplicationWorkPermitTypes", "IdJobWorkType", c => c.Int(nullable: false));
            CreateIndex("dbo.JobApplications", "IdJobApplicationType");
            CreateIndex("dbo.JobApplicationWorkPermitTypes", "IdJobWorkType");
            AddForeignKey("dbo.JobApplications", "IdJobApplicationType", "dbo.JobApplicationTypes", "Id");
            AddForeignKey("dbo.JobApplicationWorkPermitTypes", "IdJobWorkType", "dbo.JobWorkTypes", "Id");
            DropColumn("dbo.JobApplications", "IdJobType");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "IdWorkType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobApplicationWorkPermitTypes", "IdWorkType", c => c.Int(nullable: false));
            AddColumn("dbo.JobApplications", "IdJobType", c => c.Int(nullable: false));
            DropForeignKey("dbo.JobApplicationWorkPermitTypes", "IdJobWorkType", "dbo.JobWorkTypes");
            DropForeignKey("dbo.JobApplications", "IdJobApplicationType", "dbo.JobApplicationTypes");
            DropIndex("dbo.JobApplicationWorkPermitTypes", new[] { "IdJobWorkType" });
            DropIndex("dbo.JobApplications", new[] { "IdJobApplicationType" });
            DropColumn("dbo.JobApplicationWorkPermitTypes", "IdJobWorkType");
            DropColumn("dbo.JobApplications", "IdJobApplicationType");
            CreateIndex("dbo.JobApplicationWorkPermitTypes", "IdWorkType");
            CreateIndex("dbo.JobApplications", "IdJobType");
            AddForeignKey("dbo.JobApplicationWorkPermitTypes", "IdWorkType", "dbo.WorkTypes", "Id");
            AddForeignKey("dbo.JobApplications", "IdJobType", "dbo.JobTypes", "Id");
        }
    }
}
