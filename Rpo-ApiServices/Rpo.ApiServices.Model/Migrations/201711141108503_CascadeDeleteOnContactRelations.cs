namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CascadeDeleteOnContactRelations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ContactDocuments", "IdContact", "dbo.Contacts");
            DropForeignKey("dbo.Addresses", "IdContact", "dbo.Contacts");
            DropForeignKey("dbo.ContactLicenses", "IdContact", "dbo.Contacts");
            AddForeignKey("dbo.ContactDocuments", "IdContact", "dbo.Contacts", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Addresses", "IdContact", "dbo.Contacts", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ContactLicenses", "IdContact", "dbo.Contacts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContactLicenses", "IdContact", "dbo.Contacts");
            DropForeignKey("dbo.Addresses", "IdContact", "dbo.Contacts");
            DropForeignKey("dbo.ContactDocuments", "IdContact", "dbo.Contacts");
            AddForeignKey("dbo.ContactLicenses", "IdContact", "dbo.Contacts", "Id");
            AddForeignKey("dbo.Addresses", "IdContact", "dbo.Contacts", "Id");
            AddForeignKey("dbo.ContactDocuments", "IdContact", "dbo.Contacts", "Id");
        }
    }
}
