namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDEPFieldsInJobApplication : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplications", "StartDate", c => c.DateTime());
            AddColumn("dbo.JobApplications", "EndDate", c => c.DateTime());
            AddColumn("dbo.JobApplications", "IsIncludeSunday", c => c.Boolean());
            AddColumn("dbo.JobApplications", "IsIncludeSaturday", c => c.Boolean());
            AddColumn("dbo.JobApplications", "IsIncludeHoliday", c => c.Boolean());
            AddColumn("dbo.JobApplications", "TotalDays", c => c.Int());
            AddColumn("dbo.JobApplications", "WaterCost", c => c.Double());
            AddColumn("dbo.JobApplications", "HydrantCost", c => c.Double());
            AddColumn("dbo.JobApplications", "TotalCost", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobApplications", "TotalCost");
            DropColumn("dbo.JobApplications", "HydrantCost");
            DropColumn("dbo.JobApplications", "WaterCost");
            DropColumn("dbo.JobApplications", "TotalDays");
            DropColumn("dbo.JobApplications", "IsIncludeHoliday");
            DropColumn("dbo.JobApplications", "IsIncludeSaturday");
            DropColumn("dbo.JobApplications", "IsIncludeSunday");
            DropColumn("dbo.JobApplications", "EndDate");
            DropColumn("dbo.JobApplications", "StartDate");
        }
    }
}
