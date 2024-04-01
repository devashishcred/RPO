namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PermissionClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Permissions", "PermissionClass", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Permissions", "PermissionClass");
        }
    }
}
