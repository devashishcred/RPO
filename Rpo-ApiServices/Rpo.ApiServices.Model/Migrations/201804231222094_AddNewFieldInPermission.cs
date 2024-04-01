namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewFieldInPermission : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Permissions", "ModuleName", c => c.String());
            DropColumn("dbo.DocumentFields", "StaticDescription");
            DropColumn("dbo.Fields", "IsDisplayInFrontend");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Fields", "IsDisplayInFrontend", c => c.Boolean(nullable: false));
            AddColumn("dbo.DocumentFields", "StaticDescription", c => c.String());
            DropColumn("dbo.Permissions", "ModuleName");
        }
    }
}
