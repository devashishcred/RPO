namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RfpCompanyAndAddressOptional : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Rfps", new[] { "IdRfpAddress" });
            DropIndex("dbo.Rfps", new[] { "IdCompany" });
            AlterColumn("dbo.Rfps", "IdRfpAddress", c => c.Int());
            AlterColumn("dbo.Rfps", "IdCompany", c => c.Int());
            CreateIndex("dbo.Rfps", "IdRfpAddress");
            CreateIndex("dbo.Rfps", "IdCompany");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Rfps", new[] { "IdCompany" });
            DropIndex("dbo.Rfps", new[] { "IdRfpAddress" });
            AlterColumn("dbo.Rfps", "IdCompany", c => c.Int(nullable: false));
            AlterColumn("dbo.Rfps", "IdRfpAddress", c => c.Int(nullable: false));
            CreateIndex("dbo.Rfps", "IdCompany");
            CreateIndex("dbo.Rfps", "IdRfpAddress");
        }
    }
}
