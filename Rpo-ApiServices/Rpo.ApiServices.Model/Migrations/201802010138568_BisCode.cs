namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BisCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Boroughs", "BisCode", c => c.Int(nullable: false));
            AlterStoredProcedure(
                "dbo.Borough_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                        BisCode = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Boroughs]([Description], [BisCode])
                      VALUES (@Description, @BisCode)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Boroughs]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Boroughs] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Borough_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 50),
                        BisCode = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Boroughs]
                      SET [Description] = @Description, [BisCode] = @BisCode
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Boroughs", "BisCode");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
