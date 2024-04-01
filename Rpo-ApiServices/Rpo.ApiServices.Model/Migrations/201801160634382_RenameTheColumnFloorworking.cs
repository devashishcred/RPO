namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameTheColumnFloorworking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplications", "FloorWorking", c => c.String());
            DropColumn("dbo.JobApplications", "Floor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobApplications", "Floor", c => c.String());
            DropColumn("dbo.JobApplications", "FloorWorking");
        }
    }
}
