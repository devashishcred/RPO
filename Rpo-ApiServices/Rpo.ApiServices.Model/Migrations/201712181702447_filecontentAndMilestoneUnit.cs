namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class filecontentAndMilestoneUnit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Milestones", "Units", c => c.Int(nullable: false));
            AddColumn("dbo.ReferenceDocuments", "ContentPath", c => c.String(nullable: false));
            DropColumn("dbo.ReferenceDocuments", "Content");
            AlterStoredProcedure(
                "dbo.Milestone_Insert",
                p => new
                    {
                        Name = p.String(),
                        Value = p.Double(),
                        Units = p.Int(),
                        IdRfp = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Milestones]([Name], [Value], [Units], [IdRfp])
                      VALUES (@Name, @Value, @Units, @IdRfp)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Milestones]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Milestones] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Milestone_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(),
                        Value = p.Double(),
                        Units = p.Int(),
                        IdRfp = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Milestones]
                      SET [Name] = @Name, [Value] = @Value, [Units] = @Units, [IdRfp] = @IdRfp
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.ReferenceDocument_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        Keywords = p.String(maxLength: 4000),
                        Description = p.String(maxLength: 4000),
                        FileName = p.String(maxLength: 50),
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
                        Name = p.String(maxLength: 50),
                        Keywords = p.String(maxLength: 4000),
                        Description = p.String(maxLength: 4000),
                        FileName = p.String(maxLength: 50),
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
            AddColumn("dbo.ReferenceDocuments", "Content", c => c.Binary(nullable: false));
            DropColumn("dbo.ReferenceDocuments", "ContentPath");
            DropColumn("dbo.Milestones", "Units");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
