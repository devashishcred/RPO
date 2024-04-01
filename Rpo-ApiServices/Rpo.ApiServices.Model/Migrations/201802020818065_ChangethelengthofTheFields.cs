namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class ChangethelengthofTheFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RfpAddresses", "ZipCode", c => c.String(maxLength: 50));
            AlterColumn("dbo.RfpAddresses", "Block", c => c.String(maxLength: 50));
            AlterColumn("dbo.RfpAddresses", "Lot", c => c.String(maxLength: 50));
            AlterColumn("dbo.RfpAddresses", "BinNumber", c => c.String(maxLength: 50));
            AlterColumn("dbo.RfpAddresses", "ComunityBoardNumber", c => c.String(maxLength: 50));
        }

        public override void Down()
        {
            AlterColumn("dbo.RfpAddresses", "ZipCode", c => c.String(maxLength: 5));
            AlterColumn("dbo.RfpAddresses", "Block", c => c.String(maxLength: 5));
            AlterColumn("dbo.RfpAddresses", "Lot", c => c.String(maxLength: 5));
            AlterColumn("dbo.RfpAddresses", "BinNumber", c => c.String(maxLength: 5));
            AlterColumn("dbo.RfpAddresses", "ComunityBoardNumber", c => c.String(maxLength: 5));
        }
    }
}
