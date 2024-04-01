namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddressTypeDisplayOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AddressTypes", "DisplayOrder", c => c.Int(nullable: false));
            AlterStoredProcedure(
                "dbo.AddressType_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        DisplayOrder = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[AddressTypes]([Name], [DisplayOrder])
                      VALUES (@Name, @DisplayOrder)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[AddressTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[AddressTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.AddressType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        DisplayOrder = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[AddressTypes]
                      SET [Name] = @Name, [DisplayOrder] = @DisplayOrder
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.AddressTypes", "DisplayOrder");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
