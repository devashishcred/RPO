namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewColumnsForViolationAndPermitInJobDocuments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobDocuments", "DocumentDescription", c => c.String());
            AddColumn("dbo.JobDocuments", "IdJobApplicationWorkPermitType", c => c.Int());
            AddColumn("dbo.JobDocuments", "IdJobViolation", c => c.Int());
            AddColumn("dbo.JobDocuments", "JobDocumentFor", c => c.String());
            CreateIndex("dbo.JobDocuments", "IdJobApplicationWorkPermitType");
            CreateIndex("dbo.JobDocuments", "IdJobViolation");
            AddForeignKey("dbo.JobDocuments", "IdJobApplicationWorkPermitType", "dbo.JobApplicationWorkPermitTypes", "Id");
            AddForeignKey("dbo.JobDocuments", "IdJobViolation", "dbo.JobViolations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobDocuments", "IdJobViolation", "dbo.JobViolations");
            DropForeignKey("dbo.JobDocuments", "IdJobApplicationWorkPermitType", "dbo.JobApplicationWorkPermitTypes");
            DropIndex("dbo.JobDocuments", new[] { "IdJobViolation" });
            DropIndex("dbo.JobDocuments", new[] { "IdJobApplicationWorkPermitType" });
            DropColumn("dbo.JobDocuments", "JobDocumentFor");
            DropColumn("dbo.JobDocuments", "IdJobViolation");
            DropColumn("dbo.JobDocuments", "IdJobApplicationWorkPermitType");
            DropColumn("dbo.JobDocuments", "DocumentDescription");
        }
    }
}
