namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyDocument : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        IdCompany = c.Int(nullable: false),
                        DocumentPath = c.String(maxLength: 200),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.IdCompany, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.IdCompany)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            CreateStoredProcedure(
                "dbo.CompanyDocument_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 255),
                        IdCompany = p.Int(),
                        DocumentPath = p.String(maxLength: 200),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[CompanyDocuments]([Name], [IdCompany], [DocumentPath], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @IdCompany, @DocumentPath, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[CompanyDocuments]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[CompanyDocuments] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.CompanyDocument_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 255),
                        IdCompany = p.Int(),
                        DocumentPath = p.String(maxLength: 200),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[CompanyDocuments]
                      SET [Name] = @Name, [IdCompany] = @IdCompany, [DocumentPath] = @DocumentPath, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CompanyDocument_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[CompanyDocuments]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.CompanyDocument_Delete");
            DropStoredProcedure("dbo.CompanyDocument_Update");
            DropStoredProcedure("dbo.CompanyDocument_Insert");
            DropForeignKey("dbo.CompanyDocuments", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.CompanyDocuments", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.CompanyDocuments", "IdCompany", "dbo.Companies");
            DropIndex("dbo.CompanyDocuments", new[] { "LastModifiedBy" });
            DropIndex("dbo.CompanyDocuments", new[] { "CreatedBy" });
            DropIndex("dbo.CompanyDocuments", new[] { "IdCompany" });
            DropTable("dbo.CompanyDocuments");
        }
    }
}
