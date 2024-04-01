namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDisplayOrderColumnInPermission : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Permissions", "DisplayOrder", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Permissions", "DisplayOrder");
        }
    }
}
