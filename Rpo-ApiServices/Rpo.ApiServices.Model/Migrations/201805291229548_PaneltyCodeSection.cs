namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaneltyCodeSection : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobViolations", "PaneltyCodeSection", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobViolations", "PaneltyCodeSection");
        }
    }
}
