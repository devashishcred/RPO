namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeReferenceDocuments : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ReferenceDocuments", "Keywords", c => c.String(maxLength: 4000));
            AlterColumn("dbo.ReferenceDocuments", "Description", c => c.String(maxLength: 4000));
            Sql("alter table dbo.ReferenceDocuments alter column keywords nvarchar(4000)");
            Sql("alter table dbo.ReferenceDocuments alter column description nvarchar(4000)");
            AlterStoredProcedure(
                "dbo.ReferenceDocument_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        Keywords = p.String(maxLength: 4000),
                        Description = p.String(maxLength: 4000),
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
            
            AlterStoredProcedure(
                "dbo.ReferenceDocument_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        Keywords = p.String(maxLength: 4000),
                        Description = p.String(maxLength: 4000),
                        FileName = p.String(maxLength: 50),
                        Content = p.Binary(),
                    },
                body:
                    @"UPDATE [dbo].[ReferenceDocuments]
                      SET [Name] = @Name, [Keywords] = @Keywords, [Description] = @Description, [FileName] = @FileName, [Content] = @Content
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReferenceDocuments", "Description", c => c.String(maxLength: 200));
            AlterColumn("dbo.ReferenceDocuments", "Keywords", c => c.String(maxLength: 200));
            //throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
