namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRfpJobTypeCumulativeCost : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RfpJobTypeCostRanges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdRfpJobType = c.Int(nullable: false),
                        MinimumQuantity = c.Int(),
                        MaximumQuantity = c.Int(),
                        Cost = c.Double(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RfpJobTypeCumulativeCosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdRfpJobType = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Cost = c.Double(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RfpJobTypeCumulativeCosts");
            DropTable("dbo.RfpJobTypeCostRanges");
        }
    }
}
