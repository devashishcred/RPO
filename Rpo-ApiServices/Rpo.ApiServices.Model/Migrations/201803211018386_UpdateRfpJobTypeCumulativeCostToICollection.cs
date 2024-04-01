namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRfpJobTypeCumulativeCostToICollection : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.RfpJobTypeCostRanges", "IdRfpJobType");
            CreateIndex("dbo.RfpJobTypeCumulativeCosts", "IdRfpJobType");
            AddForeignKey("dbo.RfpJobTypeCostRanges", "IdRfpJobType", "dbo.RfpJobTypes", "Id");
            AddForeignKey("dbo.RfpJobTypeCumulativeCosts", "IdRfpJobType", "dbo.RfpJobTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RfpJobTypeCumulativeCosts", "IdRfpJobType", "dbo.RfpJobTypes");
            DropForeignKey("dbo.RfpJobTypeCostRanges", "IdRfpJobType", "dbo.RfpJobTypes");
            DropIndex("dbo.RfpJobTypeCumulativeCosts", new[] { "IdRfpJobType" });
            DropIndex("dbo.RfpJobTypeCostRanges", new[] { "IdRfpJobType" });
        }
    }
}
