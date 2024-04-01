namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyTypeArray : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CompanyCompanyTypes",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        CompanyType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.CompanyType_Id })
                .ForeignKey("dbo.Companies", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.CompanyTypes", t => t.CompanyType_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.CompanyType_Id);
            
            CreateStoredProcedure(
                "dbo.CompanyType_Insert",
                p => new
                    {
                        Description = p.String(),
                    },
                body:
                    @"INSERT [dbo].[CompanyTypes]([Description])
                      VALUES (@Description)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[CompanyTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[CompanyTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.CompanyType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[CompanyTypes]
                      SET [Description] = @Description
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CompanyType_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[CompanyTypes]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.CompanyType_Delete");
            DropStoredProcedure("dbo.CompanyType_Update");
            DropStoredProcedure("dbo.CompanyType_Insert");
            DropForeignKey("dbo.CompanyCompanyTypes", "CompanyType_Id", "dbo.CompanyTypes");
            DropForeignKey("dbo.CompanyCompanyTypes", "Company_Id", "dbo.Companies");
            DropIndex("dbo.CompanyCompanyTypes", new[] { "CompanyType_Id" });
            DropIndex("dbo.CompanyCompanyTypes", new[] { "Company_Id" });
            DropTable("dbo.CompanyCompanyTypes");
            DropTable("dbo.CompanyTypes");
        }
    }
}
