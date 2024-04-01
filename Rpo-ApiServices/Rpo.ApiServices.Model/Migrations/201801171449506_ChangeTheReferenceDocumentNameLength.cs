namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTheReferenceDocumentNameLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ReferenceDocuments", "Name", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.ReferenceDocuments", "FileName", c => c.String(nullable: false, maxLength: 500));
            AlterStoredProcedure(
                "dbo.ReferenceDocument_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 500),
                        Keywords = p.String(maxLength: 4000),
                        Description = p.String(maxLength: 4000),
                        FileName = p.String(maxLength: 500),
                        ContentPath = p.String(),
                    },
                body:
                    @"INSERT [dbo].[ReferenceDocuments]([Name], [Keywords], [Description], [FileName], [ContentPath])
                      VALUES (@Name, @Keywords, @Description, @FileName, @ContentPath)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ReferenceDocuments]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ReferenceDocuments] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.ReferenceDocument_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 500),
                        Keywords = p.String(maxLength: 4000),
                        Description = p.String(maxLength: 4000),
                        FileName = p.String(maxLength: 500),
                        ContentPath = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[ReferenceDocuments]
                      SET [Name] = @Name, [Keywords] = @Keywords, [Description] = @Description, [FileName] = @FileName, [ContentPath] = @ContentPath
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReferenceDocuments", "FileName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.ReferenceDocuments", "Name", c => c.String(nullable: false, maxLength: 50));
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
