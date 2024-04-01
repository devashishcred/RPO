namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteIdJobWorkType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobApplications", "IdJobWorkType", "dbo.JobWorkTypes");
            DropIndex("dbo.JobApplications", new[] { "IdJobWorkType" });
            DropColumn("dbo.JobApplications", "IdJobWorkType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobApplications", "IdJobWorkType", c => c.Int());
            CreateIndex("dbo.JobApplications", "IdJobWorkType");
            AddForeignKey("dbo.JobApplications", "IdJobWorkType", "dbo.JobWorkTypes", "Id");
        }
    }
}
