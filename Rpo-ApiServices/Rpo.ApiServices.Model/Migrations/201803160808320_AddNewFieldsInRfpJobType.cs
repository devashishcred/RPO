namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewFieldsInRfpJobType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RfpJobTypes", "IdParent", c => c.Int());
            AddColumn("dbo.RfpJobTypes", "Level", c => c.Int(nullable: false));
            AddColumn("dbo.RfpJobTypes", "ServiceDescription", c => c.String());
            AddColumn("dbo.RfpJobTypes", "AppendWorkDescription", c => c.Boolean(nullable: false));
            AddColumn("dbo.RfpJobTypes", "CustomServiceDescription", c => c.Boolean(nullable: false));
            AddColumn("dbo.RfpJobTypes", "AdditionalUnitCost", c => c.Boolean(nullable: false));
            CreateIndex("dbo.RfpJobTypes", "IdParent");
            AddForeignKey("dbo.RfpJobTypes", "IdParent", "dbo.RfpJobTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RfpJobTypes", "IdParent", "dbo.RfpJobTypes");
            DropIndex("dbo.RfpJobTypes", new[] { "IdParent" });
            DropColumn("dbo.RfpJobTypes", "AdditionalUnitCost");
            DropColumn("dbo.RfpJobTypes", "CustomServiceDescription");
            DropColumn("dbo.RfpJobTypes", "AppendWorkDescription");
            DropColumn("dbo.RfpJobTypes", "ServiceDescription");
            DropColumn("dbo.RfpJobTypes", "Level");
            DropColumn("dbo.RfpJobTypes", "IdParent");
        }
    }
}
