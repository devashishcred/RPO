namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesJobTimeNoteTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobTimeNoteCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.JobTimeNotes", "IdJobTimeNoteCategory", c => c.Int());
            AlterColumn("dbo.JobTimeNotes", "Hours", c => c.Int());
            AlterColumn("dbo.JobTimeNotes", "Minutes", c => c.Int());
            CreateIndex("dbo.JobTimeNotes", "IdJobTimeNoteCategory");
            AddForeignKey("dbo.JobTimeNotes", "IdJobTimeNoteCategory", "dbo.JobTimeNoteCategories", "Id");
            DropColumn("dbo.JobTimeNotes", "TimeCategory");
            AlterStoredProcedure(
                "dbo.JobTimeNote_Insert",
                p => new
                    {
                        IdJob = p.Int(),
                        IdJobTimeNoteCategory = p.Int(),
                        Service = p.String(),
                        ProgressNotes = p.String(),
                        TimeNoteDate = p.DateTime(),
                        Hours = p.Int(),
                        Minutes = p.Int(),
                        Billable = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[JobTimeNotes]([IdJob], [IdJobTimeNoteCategory], [Service], [ProgressNotes], [TimeNoteDate], [Hours], [Minutes], [Billable])
                      VALUES (@IdJob, @IdJobTimeNoteCategory, @Service, @ProgressNotes, @TimeNoteDate, @Hours, @Minutes, @Billable)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[JobTimeNotes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[JobTimeNotes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.JobTimeNote_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdJob = p.Int(),
                        IdJobTimeNoteCategory = p.Int(),
                        Service = p.String(),
                        ProgressNotes = p.String(),
                        TimeNoteDate = p.DateTime(),
                        Hours = p.Int(),
                        Minutes = p.Int(),
                        Billable = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[JobTimeNotes]
                      SET [IdJob] = @IdJob, [IdJobTimeNoteCategory] = @IdJobTimeNoteCategory, [Service] = @Service, [ProgressNotes] = @ProgressNotes, [TimeNoteDate] = @TimeNoteDate, [Hours] = @Hours, [Minutes] = @Minutes, [Billable] = @Billable
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobTimeNotes", "TimeCategory", c => c.Int(nullable: false));
            DropForeignKey("dbo.JobTimeNotes", "IdJobTimeNoteCategory", "dbo.JobTimeNoteCategories");
            DropIndex("dbo.JobTimeNotes", new[] { "IdJobTimeNoteCategory" });
            AlterColumn("dbo.JobTimeNotes", "Minutes", c => c.Int(nullable: false));
            AlterColumn("dbo.JobTimeNotes", "Hours", c => c.Int(nullable: false));
            DropColumn("dbo.JobTimeNotes", "IdJobTimeNoteCategory");
            DropTable("dbo.JobTimeNoteCategories");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
