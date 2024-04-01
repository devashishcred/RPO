namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsPlaceHolderfieldintblDocumentFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentFields", "IsPlaceHolder", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocumentFields", "IsPlaceHolder");
        }
    }
}
