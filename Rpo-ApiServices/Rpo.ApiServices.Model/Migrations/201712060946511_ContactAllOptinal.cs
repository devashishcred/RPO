namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactAllOptinal : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Contacts", "IX_ContactEmail");
            AlterColumn("dbo.Contacts", "LastName", c => c.String(maxLength: 50));
            AlterColumn("dbo.Contacts", "Email", c => c.String(maxLength: 255));
            CreateIndex("dbo.Contacts", "Email", unique: true, name: "IX_ContactEmail");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Contacts", "IX_ContactEmail");
            AlterColumn("dbo.Contacts", "Email", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Contacts", "LastName", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.Contacts", "Email", unique: true, name: "IX_ContactEmail");
        }
    }
}
