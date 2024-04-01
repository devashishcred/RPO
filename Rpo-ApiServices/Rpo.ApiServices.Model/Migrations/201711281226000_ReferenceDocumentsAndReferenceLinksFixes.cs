namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReferenceDocumentsAndReferenceLinksFixes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReferenceDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Keywords = c.String(maxLength: 200),
                        Description = c.String(maxLength: 200),
                        FileName = c.String(nullable: false, maxLength: 50),
                        Content = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.ReferenceLinks", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.ReferenceLinks", "Url", c => c.String(nullable: false));
            CreateStoredProcedure(
                "dbo.ReferenceDocument_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        Keywords = p.String(maxLength: 200),
                        Description = p.String(maxLength: 200),
                        FileName = p.String(maxLength: 50),
                        Content = p.Binary(),
                    },
                body:
                    @"INSERT [dbo].[ReferenceDocuments]([Name], [Keywords], [Description], [FileName], [Content])
                      VALUES (@Name, @Keywords, @Description, @FileName, @Content)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ReferenceDocuments]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ReferenceDocuments] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.ReferenceDocument_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        Keywords = p.String(maxLength: 200),
                        Description = p.String(maxLength: 200),
                        FileName = p.String(maxLength: 50),
                        Content = p.Binary(),
                    },
                body:
                    @"UPDATE [dbo].[ReferenceDocuments]
                      SET [Name] = @Name, [Keywords] = @Keywords, [Description] = @Description, [FileName] = @FileName, [Content] = @Content
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ReferenceDocument_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ReferenceDocuments]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.ReferenceDocument_Delete");
            DropStoredProcedure("dbo.ReferenceDocument_Update");
            DropStoredProcedure("dbo.ReferenceDocument_Insert");
            AlterColumn("dbo.ReferenceLinks", "Url", c => c.String());
            AlterColumn("dbo.ReferenceLinks", "Name", c => c.String(maxLength: 50));
            DropTable("dbo.ReferenceDocuments");
        }
    }
}
