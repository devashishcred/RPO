namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Worktype : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplications", "IdJobWorkType", c => c.String());
            DropColumn("dbo.JobApplications", "IdJobWorkTypes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobApplications", "IdJobWorkTypes", c => c.String());
            DropColumn("dbo.JobApplications", "IdJobWorkType");
        }
    }
}
