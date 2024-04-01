namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyTypeNested : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyTypes", "ItemName", c => c.String());
            AddColumn("dbo.CompanyTypes", "IdParent", c => c.Int());
            CreateIndex("dbo.CompanyTypes", "IdParent");
            AddForeignKey("dbo.CompanyTypes", "IdParent", "dbo.CompanyTypes", "Id");
            DropColumn("dbo.CompanyTypes", "Description");
            AlterStoredProcedure(
                "dbo.CompanyType_Insert",
                p => new
                    {
                        ItemName = p.String(),
                        IdParent = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[CompanyTypes]([ItemName], [IdParent])
                      VALUES (@ItemName, @IdParent)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[CompanyTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[CompanyTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.CompanyType_Update",
                p => new
                    {
                        Id = p.Int(),
                        ItemName = p.String(),
                        IdParent = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[CompanyTypes]
                      SET [ItemName] = @ItemName, [IdParent] = @IdParent
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.CompanyTypes", "Description", c => c.String());
            DropForeignKey("dbo.CompanyTypes", "IdParent", "dbo.CompanyTypes");
            DropIndex("dbo.CompanyTypes", new[] { "IdParent" });
            DropColumn("dbo.CompanyTypes", "IdParent");
            DropColumn("dbo.CompanyTypes", "ItemName");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
