namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDisplayOrderInDocument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentMasters", "DisplayOrder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocumentMasters", "DisplayOrder");
        }
    }
}
