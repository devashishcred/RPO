namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeInPaneltyCodeTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ViolationPaneltyCodes", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ViolationPaneltyCodes", "Description");
        }
    }
}
