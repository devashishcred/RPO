namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeApplicationAddNewColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplications", "StreetWorkingOn", c => c.String());
            AddColumn("dbo.JobApplications", "StreetFrom", c => c.String());
            AddColumn("dbo.JobApplications", "StreetTo", c => c.String());
            AddColumn("dbo.JobViolations", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobViolations", "Notes");
            DropColumn("dbo.JobApplications", "StreetTo");
            DropColumn("dbo.JobApplications", "StreetFrom");
            DropColumn("dbo.JobApplications", "StreetWorkingOn");
        }
    }
}
