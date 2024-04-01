namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdJobWorkType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplications", "IdJobWorkType", c => c.Int());
            CreateIndex("dbo.JobApplications", "IdJobWorkType");
            AddForeignKey("dbo.JobApplications", "IdJobWorkType", "dbo.JobWorkTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobApplications", "IdJobWorkType", "dbo.JobWorkTypes");
            DropIndex("dbo.JobApplications", new[] { "IdJobWorkType" });
            DropColumn("dbo.JobApplications", "IdJobWorkType");
        }
    }
}
