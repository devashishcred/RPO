namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldStaticDescriptioninDocumentFieldandAddFieldIsFrontEndDisplayinField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentFields", "StaticDescription", c => c.String());
            AddColumn("dbo.Fields", "IsDisplayInFrontend", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fields", "IsDisplayInFrontend");
            DropColumn("dbo.DocumentFields", "StaticDescription");
        }
    }
}
