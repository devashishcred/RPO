namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeInPriceRangeTableForWorkType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectDetails", "IdRfpSubJobType", "dbo.RfpSubJobTypes");
            AddForeignKey("dbo.ProjectDetails", "IdRfpSubJobType", "dbo.RfpJobTypes", "Id");

            DropForeignKey("dbo.ProjectDetails", "IdRfpSubJobTypeCategory", "dbo.RfpSubJobTypeCategories");
            AddForeignKey("dbo.ProjectDetails", "IdRfpSubJobTypeCategory", "dbo.RfpJobTypes", "Id");

            DropForeignKey("dbo.RfpWorkTypes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpWorkTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpWorkTypeCategories", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpWorkTypeCategories", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpSubJobTypes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpSubJobTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpSubJobTypeCategories", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpSubJobTypeCategories", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpSubJobTypeCategories", "IdRfpJobType", "dbo.RfpJobTypes");
            DropForeignKey("dbo.RfpSubJobTypes", "IdRfpSubJobTypeCategory", "dbo.RfpSubJobTypeCategories");
            DropForeignKey("dbo.RfpWorkTypeCategories", "IdRfpSubJobType", "dbo.RfpSubJobTypes");
            DropForeignKey("dbo.RfpWorkTypes", "IdRfpWorkTypeCategory", "dbo.RfpWorkTypeCategories");
            DropIndex("dbo.RfpWorkTypes", new[] { "IdRfpWorkTypeCategory" });
            DropIndex("dbo.RfpWorkTypes", new[] { "CreatedBy" });
            DropIndex("dbo.RfpWorkTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.RfpWorkTypeCategories", new[] { "IdRfpSubJobType" });
            DropIndex("dbo.RfpWorkTypeCategories", new[] { "CreatedBy" });
            DropIndex("dbo.RfpWorkTypeCategories", new[] { "LastModifiedBy" });
            DropIndex("dbo.RfpSubJobTypes", "IX_RfpSubJobTypeName");
            DropIndex("dbo.RfpSubJobTypes", new[] { "IdRfpSubJobTypeCategory" });
            DropIndex("dbo.RfpSubJobTypes", new[] { "CreatedBy" });
            DropIndex("dbo.RfpSubJobTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.RfpSubJobTypeCategories", "IX_RfpSubJobTypeCategoryName");
            DropIndex("dbo.RfpSubJobTypeCategories", new[] { "IdRfpJobType" });
            DropIndex("dbo.RfpSubJobTypeCategories", new[] { "CreatedBy" });
            DropIndex("dbo.RfpSubJobTypeCategories", new[] { "LastModifiedBy" });
            DropTable("dbo.RfpWorkTypes");
            DropTable("dbo.RfpWorkTypeCategories");
            DropTable("dbo.RfpSubJobTypes");
            DropTable("dbo.RfpSubJobTypeCategories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RfpSubJobTypeCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        IdRfpJobType = c.Int(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RfpSubJobTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        IdRfpSubJobTypeCategory = c.Int(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RfpWorkTypeCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        IdRfpSubJobType = c.Int(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RfpWorkTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        Price = c.Double(nullable: false),
                        IsUnitCost = c.Boolean(nullable: false),
                        IsAdditionalUnitCost = c.Boolean(nullable: false),
                        IsDescription = c.Boolean(nullable: false),
                        IsWorkDescription = c.Boolean(nullable: false),
                        AdditionalUnitCost = c.Double(),
                        IdRfpWorkTypeCategory = c.Int(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.RfpSubJobTypeCategories", "LastModifiedBy");
            CreateIndex("dbo.RfpSubJobTypeCategories", "CreatedBy");
            CreateIndex("dbo.RfpSubJobTypeCategories", "IdRfpJobType");
            CreateIndex("dbo.RfpSubJobTypeCategories", "Name", unique: true, name: "IX_RfpSubJobTypeCategoryName");
            CreateIndex("dbo.RfpSubJobTypes", "LastModifiedBy");
            CreateIndex("dbo.RfpSubJobTypes", "CreatedBy");
            CreateIndex("dbo.RfpSubJobTypes", "IdRfpSubJobTypeCategory");
            CreateIndex("dbo.RfpSubJobTypes", "Name", unique: true, name: "IX_RfpSubJobTypeName");
            CreateIndex("dbo.RfpWorkTypeCategories", "LastModifiedBy");
            CreateIndex("dbo.RfpWorkTypeCategories", "CreatedBy");
            CreateIndex("dbo.RfpWorkTypeCategories", "IdRfpSubJobType");
            CreateIndex("dbo.RfpWorkTypes", "LastModifiedBy");
            CreateIndex("dbo.RfpWorkTypes", "CreatedBy");
            CreateIndex("dbo.RfpWorkTypes", "IdRfpWorkTypeCategory");
            AddForeignKey("dbo.RfpWorkTypes", "IdRfpWorkTypeCategory", "dbo.RfpWorkTypeCategories", "Id");
            AddForeignKey("dbo.RfpWorkTypeCategories", "IdRfpSubJobType", "dbo.RfpSubJobTypes", "Id");
            AddForeignKey("dbo.RfpSubJobTypes", "IdRfpSubJobTypeCategory", "dbo.RfpSubJobTypeCategories", "Id");
            AddForeignKey("dbo.RfpSubJobTypeCategories", "IdRfpJobType", "dbo.RfpJobTypes", "Id");
            AddForeignKey("dbo.RfpSubJobTypeCategories", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.RfpSubJobTypeCategories", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.RfpSubJobTypes", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.RfpSubJobTypes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.RfpWorkTypeCategories", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.RfpWorkTypeCategories", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.RfpWorkTypes", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.RfpWorkTypes", "CreatedBy", "dbo.Employees", "Id");
        }
    }
}
