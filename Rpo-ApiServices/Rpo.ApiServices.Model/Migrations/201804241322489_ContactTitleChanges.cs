namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactTitleChanges : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ContactTitles", "IX_ContactTitleName");
        }
        
        public override void Down()
        {
            CreateIndex("dbo.ContactTitles", "Name", unique: true, name: "IX_ContactTitleName");
        }
    }
}
