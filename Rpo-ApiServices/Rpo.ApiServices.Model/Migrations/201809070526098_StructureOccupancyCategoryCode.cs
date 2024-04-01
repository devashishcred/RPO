namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StructureOccupancyCategoryCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SeismicDesignCategories", "Code", c => c.String(maxLength: 10));
            AddColumn("dbo.StructureOccupancyCategories", "Code", c => c.String(maxLength: 10));
            AlterStoredProcedure(
                "dbo.SeismicDesignCategory_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        Code = p.String(maxLength: 10),
                    },
                body:
                    @"INSERT [dbo].[SeismicDesignCategories]([Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate], [Code])
                      VALUES (@Description, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate, @Code)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[SeismicDesignCategories]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[SeismicDesignCategories] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.SeismicDesignCategory_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        Code = p.String(maxLength: 10),
                    },
                body:
                    @"UPDATE [dbo].[SeismicDesignCategories]
                      SET [Description] = @Description, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate, [Code] = @Code
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.StructureOccupancyCategory_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        Code = p.String(maxLength: 10),
                    },
                body:
                    @"INSERT [dbo].[StructureOccupancyCategories]([Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate], [Code])
                      VALUES (@Description, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate, @Code)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[StructureOccupancyCategories]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[StructureOccupancyCategories] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.StructureOccupancyCategory_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        Code = p.String(maxLength: 10),
                    },
                body:
                    @"UPDATE [dbo].[StructureOccupancyCategories]
                      SET [Description] = @Description, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate, [Code] = @Code
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.StructureOccupancyCategories", "Code");
            DropColumn("dbo.SeismicDesignCategories", "Code");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
