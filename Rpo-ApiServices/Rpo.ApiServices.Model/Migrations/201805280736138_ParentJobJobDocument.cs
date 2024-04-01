namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParentJobJobDocument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationStatus", "IdJobApplicationType", c => c.Int());
            AddColumn("dbo.JobDocuments", "IdParent", c => c.Int());
            CreateIndex("dbo.ApplicationStatus", "IdJobApplicationType");
            CreateIndex("dbo.JobDocuments", "IdParent");
            AddForeignKey("dbo.ApplicationStatus", "IdJobApplicationType", "dbo.JobApplicationTypes", "Id");
            AddForeignKey("dbo.JobDocuments", "IdParent", "dbo.JobDocuments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobDocuments", "IdParent", "dbo.JobDocuments");
            DropForeignKey("dbo.ApplicationStatus", "IdJobApplicationType", "dbo.JobApplicationTypes");
            DropIndex("dbo.JobDocuments", new[] { "IdParent" });
            DropIndex("dbo.ApplicationStatus", new[] { "IdJobApplicationType" });
            DropColumn("dbo.JobDocuments", "IdParent");
            DropColumn("dbo.ApplicationStatus", "IdJobApplicationType");
        }
    }
}
