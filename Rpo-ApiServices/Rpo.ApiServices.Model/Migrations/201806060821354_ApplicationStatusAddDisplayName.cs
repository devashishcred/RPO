namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationStatusAddDisplayName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationStatus", "DisplayName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationStatus", "DisplayName");
        }
    }
}
