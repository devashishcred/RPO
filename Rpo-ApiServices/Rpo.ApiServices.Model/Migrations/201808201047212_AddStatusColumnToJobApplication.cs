namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusColumnToJobApplication : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplications", "JobApplicationStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobApplications", "JobApplicationStatus");
        }
    }
}
