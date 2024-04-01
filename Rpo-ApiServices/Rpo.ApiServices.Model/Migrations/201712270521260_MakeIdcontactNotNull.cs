namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeIdcontactNotNull : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Rfps", new[] { "IdContact" });
            AlterColumn("dbo.Rfps", "IdContact", c => c.Int());
            CreateIndex("dbo.Rfps", "IdContact");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Rfps", new[] { "IdContact" });
            AlterColumn("dbo.Rfps", "IdContact", c => c.Int(nullable: false));
            CreateIndex("dbo.Rfps", "IdContact");
        }
    }
}
