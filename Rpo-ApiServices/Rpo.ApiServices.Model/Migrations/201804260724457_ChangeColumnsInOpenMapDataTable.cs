namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeColumnsInOpenMapDataTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OpenMapAddresses", "HouseNumber_Street", c => c.String());
            DropColumn("dbo.OpenMapAddresses", "HouseNumber");
            DropColumn("dbo.OpenMapAddresses", "Street");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OpenMapAddresses", "Street", c => c.String());
            AddColumn("dbo.OpenMapAddresses", "HouseNumber", c => c.String());
            DropColumn("dbo.OpenMapAddresses", "HouseNumber_Street");
        }
    }
}
