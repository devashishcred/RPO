namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReferredNotMadatoryOnRfp : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Rfps", new[] { "IdReferredByCompany" });
            DropIndex("dbo.Rfps", new[] { "IdReferredByContact" });
            AlterColumn("dbo.Rfps", "IdReferredByCompany", c => c.Int());
            AlterColumn("dbo.Rfps", "IdReferredByContact", c => c.Int());
            CreateIndex("dbo.Rfps", "IdReferredByCompany");
            CreateIndex("dbo.Rfps", "IdReferredByContact");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Rfps", new[] { "IdReferredByContact" });
            DropIndex("dbo.Rfps", new[] { "IdReferredByCompany" });
            AlterColumn("dbo.Rfps", "IdReferredByContact", c => c.Int(nullable: false));
            AlterColumn("dbo.Rfps", "IdReferredByCompany", c => c.Int(nullable: false));
            CreateIndex("dbo.Rfps", "IdReferredByContact");
            CreateIndex("dbo.Rfps", "IdReferredByCompany");
        }
    }
}
