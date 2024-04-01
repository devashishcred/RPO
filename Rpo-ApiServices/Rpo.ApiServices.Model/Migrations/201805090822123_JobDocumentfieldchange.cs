namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobDocumentfieldchange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobDocuments", "IdJobApplication", c => c.Int());
            CreateIndex("dbo.JobDocuments", "IdJobApplication");
            AddForeignKey("dbo.JobDocuments", "IdJobApplication", "dbo.JobApplications", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobDocuments", "IdJobApplication", "dbo.JobApplications");
            DropIndex("dbo.JobDocuments", new[] { "IdJobApplication" });
            DropColumn("dbo.JobDocuments", "IdJobApplication");
        }
    }
}
