namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OpenMapAddressAddNew : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OpenMapAddresses", "Block", c => c.String());
            AddColumn("dbo.OpenMapAddresses", "Lot", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OpenMapAddresses", "Lot");
            DropColumn("dbo.OpenMapAddresses", "Block");
        }
    }
}
