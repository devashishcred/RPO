namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobWorkContent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobTypes", "Content", c => c.String());
            AddColumn("dbo.WorkTypes", "Content", c => c.String());
            AlterStoredProcedure(
                "dbo.JobType_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 100),
                        Content = p.String(),
                        Number = p.String(maxLength: 5),
                        IdParent = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[JobTypes]([Description], [Content], [Number], [IdParent])
                      VALUES (@Description, @Content, @Number, @IdParent)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[JobTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[JobTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.JobType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 100),
                        Content = p.String(),
                        Number = p.String(maxLength: 5),
                        IdParent = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[JobTypes]
                      SET [Description] = @Description, [Content] = @Content, [Number] = @Number, [IdParent] = @IdParent
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.WorkType_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                        Content = p.String(),
                        Number = p.String(maxLength: 5),
                    },
                body:
                    @"INSERT [dbo].[WorkTypes]([Description], [Content], [Number])
                      VALUES (@Description, @Content, @Number)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[WorkTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[WorkTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.WorkType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 50),
                        Content = p.String(),
                        Number = p.String(maxLength: 5),
                    },
                body:
                    @"UPDATE [dbo].[WorkTypes]
                      SET [Description] = @Description, [Content] = @Content, [Number] = @Number
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkTypes", "Content");
            DropColumn("dbo.JobTypes", "Content");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
