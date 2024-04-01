namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameJobApplicationForColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplications", "ApplicationFor", c => c.String());
            DropColumn("dbo.JobApplications", "For");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobApplications", "For", c => c.String());
            DropColumn("dbo.JobApplications", "ApplicationFor");
        }
    }
}
