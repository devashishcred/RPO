namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobApplicationAddDEPNewFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplications", "Description", c => c.String());
            AddColumn("dbo.JobApplications", "Purpose", c => c.String());
            AddColumn("dbo.JobApplications", "ModelNumber", c => c.String());
            AddColumn("dbo.JobApplications", "SerialNumber", c => c.String());
            AddColumn("dbo.JobApplications", "Manufacturer", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobApplications", "Manufacturer");
            DropColumn("dbo.JobApplications", "SerialNumber");
            DropColumn("dbo.JobApplications", "ModelNumber");
            DropColumn("dbo.JobApplications", "Purpose");
            DropColumn("dbo.JobApplications", "Description");
        }
    }
}
