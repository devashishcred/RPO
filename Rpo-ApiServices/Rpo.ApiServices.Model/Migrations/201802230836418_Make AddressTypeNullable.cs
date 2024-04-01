namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeAddressTypeNullable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RfpAddresses", new[] { "IdAddressType" });
            AlterColumn("dbo.RfpAddresses", "IdAddressType", c => c.Int());
            CreateIndex("dbo.RfpAddresses", "IdAddressType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RfpAddresses", new[] { "IdAddressType" });
            AlterColumn("dbo.RfpAddresses", "IdAddressType", c => c.Int(nullable: false));
            CreateIndex("dbo.RfpAddresses", "IdAddressType");
        }
    }
}
