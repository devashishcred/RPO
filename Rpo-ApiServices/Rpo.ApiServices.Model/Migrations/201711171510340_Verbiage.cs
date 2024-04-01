namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Verbiage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Verbiages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateStoredProcedure(
                "dbo.Verbiage_Insert",
                p => new
                    {
                        Name = p.String(),
                        Content = p.String(),
                    },
                body:
                    @"INSERT [dbo].[Verbiages]([Name], [Content])
                      VALUES (@Name, @Content)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Verbiages]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Verbiages] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Verbiage_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(),
                        Content = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[Verbiages]
                      SET [Name] = @Name, [Content] = @Content
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Verbiage_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Verbiages]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Verbiage_Delete");
            DropStoredProcedure("dbo.Verbiage_Update");
            DropStoredProcedure("dbo.Verbiage_Insert");
            DropTable("dbo.Verbiages");
        }
    }
}
