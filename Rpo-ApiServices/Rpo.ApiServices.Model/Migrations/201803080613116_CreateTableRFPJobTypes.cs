namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableRFPJobTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RfpJobTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .ForeignKey("dbo.RfpJobTypes", t => t.IdRfpJobType)
                .Index(t => t.Name, unique: true, name: "IX_RfpSubJobTypeCategoryName")
                .Index(t => t.IdRfpJobType)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .ForeignKey("dbo.RfpSubJobTypeCategories", t => t.IdRfpSubJobTypeCategory)
                .Index(t => t.Name, unique: true, name: "IX_RfpSubJobTypeName")
                .Index(t => t.IdRfpSubJobTypeCategory)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .ForeignKey("dbo.RfpSubJobTypes", t => t.IdRfpSubJobType)
                .Index(t => t.Name, unique: true, name: "IX_RfpWorkTypeCategoryName")
                .Index(t => t.IdRfpSubJobType)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            CreateTable(
                "dbo.RfpWorkTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        Price = c.Double(nullable: false),
                        IsFixPrice = c.Boolean(nullable: false),
                        IdRfpWorkTypeCategory = c.Int(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .ForeignKey("dbo.RfpWorkTypeCategories", t => t.IdRfpWorkTypeCategory)
                .Index(t => t.Name, unique: true, name: "IX_RfpWorkTypeName")
                .Index(t => t.IdRfpWorkTypeCategory)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RfpWorkTypes", "IdRfpWorkTypeCategory", "dbo.RfpWorkTypeCategories");
            DropForeignKey("dbo.RfpWorkTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpWorkTypes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpWorkTypeCategories", "IdRfpSubJobType", "dbo.RfpSubJobTypes");
            DropForeignKey("dbo.RfpWorkTypeCategories", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpWorkTypeCategories", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpSubJobTypes", "IdRfpSubJobTypeCategory", "dbo.RfpSubJobTypeCategories");
            DropForeignKey("dbo.RfpSubJobTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpSubJobTypes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpSubJobTypeCategories", "IdRfpJobType", "dbo.RfpJobTypes");
            DropForeignKey("dbo.RfpSubJobTypeCategories", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpSubJobTypeCategories", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpJobTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpJobTypes", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.RfpWorkTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.RfpWorkTypes", new[] { "CreatedBy" });
            DropIndex("dbo.RfpWorkTypes", new[] { "IdRfpWorkTypeCategory" });
            DropIndex("dbo.RfpWorkTypes", "IX_RfpWorkTypeName");
            DropIndex("dbo.RfpWorkTypeCategories", new[] { "LastModifiedBy" });
            DropIndex("dbo.RfpWorkTypeCategories", new[] { "CreatedBy" });
            DropIndex("dbo.RfpWorkTypeCategories", new[] { "IdRfpSubJobType" });
            DropIndex("dbo.RfpWorkTypeCategories", "IX_RfpWorkTypeCategoryName");
            DropIndex("dbo.RfpSubJobTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.RfpSubJobTypes", new[] { "CreatedBy" });
            DropIndex("dbo.RfpSubJobTypes", new[] { "IdRfpSubJobTypeCategory" });
            DropIndex("dbo.RfpSubJobTypes", "IX_RfpSubJobTypeName");
            DropIndex("dbo.RfpSubJobTypeCategories", new[] { "LastModifiedBy" });
            DropIndex("dbo.RfpSubJobTypeCategories", new[] { "CreatedBy" });
            DropIndex("dbo.RfpSubJobTypeCategories", new[] { "IdRfpJobType" });
            DropIndex("dbo.RfpSubJobTypeCategories", "IX_RfpSubJobTypeCategoryName");
            DropIndex("dbo.RfpJobTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.RfpJobTypes", new[] { "CreatedBy" });
            DropTable("dbo.RfpWorkTypes");
            DropTable("dbo.RfpWorkTypeCategories");
            DropTable("dbo.RfpSubJobTypes");
            DropTable("dbo.RfpSubJobTypeCategories");
            DropTable("dbo.RfpJobTypes");
        }
    }
}
