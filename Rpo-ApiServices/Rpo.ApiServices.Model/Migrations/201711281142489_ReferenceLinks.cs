namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReferenceLinks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReferenceLinks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateStoredProcedure(
                "dbo.ReferenceLink_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        Url = p.String(),
                    },
                body:
                    @"INSERT [dbo].[ReferenceLinks]([Name], [Url])
                      VALUES (@Name, @Url)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ReferenceLinks]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ReferenceLinks] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.ReferenceLink_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        Url = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[ReferenceLinks]
                      SET [Name] = @Name, [Url] = @Url
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ReferenceLink_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ReferenceLinks]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.ReferenceLink_Delete");
            DropStoredProcedure("dbo.ReferenceLink_Update");
            DropStoredProcedure("dbo.ReferenceLink_Insert");
            DropTable("dbo.ReferenceLinks");
        }
    }
}
