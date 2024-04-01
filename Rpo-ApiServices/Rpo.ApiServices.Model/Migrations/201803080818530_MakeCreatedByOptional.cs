namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeCreatedByOptional : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RfpJobTypes", new[] { "CreatedBy" });
            AlterColumn("dbo.RfpJobTypes", "CreatedBy", c => c.Int());
            CreateIndex("dbo.RfpJobTypes", "CreatedBy");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RfpJobTypes", new[] { "CreatedBy" });
            AlterColumn("dbo.RfpJobTypes", "CreatedBy", c => c.Int(nullable: false));
            CreateIndex("dbo.RfpJobTypes", "CreatedBy");
        }
    }
}
