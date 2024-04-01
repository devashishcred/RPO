namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeFieldsNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ContactLicenses", "ExpirationLicenseDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ContactLicenses", "ExpirationLicenseDate", c => c.DateTime(nullable: false));
        }
    }
}
