namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdJobWorkTypes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobApplications", "IdJobWorkType", "dbo.JobWorkTypes");
            DropIndex("dbo.JobApplications", new[] { "IdJobWorkType" });
            AddColumn("dbo.JobApplications", "IdJobWorkTypes", c => c.String());
            DropColumn("dbo.JobApplications", "IdJobWorkType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobApplications", "IdJobWorkType", c => c.Int());
            DropColumn("dbo.JobApplications", "IdJobWorkTypes");
            CreateIndex("dbo.JobApplications", "IdJobWorkType");
            AddForeignKey("dbo.JobApplications", "IdJobWorkType", "dbo.JobWorkTypes", "Id");
        }
    }
}
