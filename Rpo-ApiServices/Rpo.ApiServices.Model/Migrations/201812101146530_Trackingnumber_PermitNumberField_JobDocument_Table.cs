namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Trackingnumber_PermitNumberField_JobDocument_Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobDocuments", "TrackingNumber", c => c.String());
            AddColumn("dbo.JobDocuments", "PermitNumber", c => c.String());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "IsCompleted", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobApplicationWorkPermitTypes", "IsCompleted");
            DropColumn("dbo.JobDocuments", "PermitNumber");
            DropColumn("dbo.JobDocuments", "TrackingNumber");
        }
    }
}
