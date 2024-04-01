namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DEPCostSettingTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DEPCostSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Price = c.Double(),
                        NumberOfDays = c.Int(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DEPCostSettings", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.DEPCostSettings", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.DEPCostSettings", new[] { "LastModifiedBy" });
            DropIndex("dbo.DEPCostSettings", new[] { "CreatedBy" });
            DropTable("dbo.DEPCostSettings");
        }
    }
}
