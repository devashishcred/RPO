namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobTimeNotes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobTimeNotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJob = c.Int(nullable: false),
                        TimeCategory = c.Int(nullable: false),
                        Service = c.String(),
                        ProgressNotes = c.String(),
                        TimeNoteDate = c.DateTime(nullable: false),
                        Hours = c.Int(nullable: false),
                        Minutes = c.Int(nullable: false),
                        Billable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jobs", t => t.IdJob)
                .Index(t => t.IdJob);
            
            CreateStoredProcedure(
                "dbo.JobTimeNote_Insert",
                p => new
                    {
                        IdJob = p.Int(),
                        TimeCategory = p.Int(),
                        Service = p.String(),
                        ProgressNotes = p.String(),
                        TimeNoteDate = p.DateTime(),
                        Hours = p.Int(),
                        Minutes = p.Int(),
                        Billable = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[JobTimeNotes]([IdJob], [TimeCategory], [Service], [ProgressNotes], [TimeNoteDate], [Hours], [Minutes], [Billable])
                      VALUES (@IdJob, @TimeCategory, @Service, @ProgressNotes, @TimeNoteDate, @Hours, @Minutes, @Billable)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[JobTimeNotes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[JobTimeNotes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.JobTimeNote_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdJob = p.Int(),
                        TimeCategory = p.Int(),
                        Service = p.String(),
                        ProgressNotes = p.String(),
                        TimeNoteDate = p.DateTime(),
                        Hours = p.Int(),
                        Minutes = p.Int(),
                        Billable = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[JobTimeNotes]
                      SET [IdJob] = @IdJob, [TimeCategory] = @TimeCategory, [Service] = @Service, [ProgressNotes] = @ProgressNotes, [TimeNoteDate] = @TimeNoteDate, [Hours] = @Hours, [Minutes] = @Minutes, [Billable] = @Billable
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.JobTimeNote_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[JobTimeNotes]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.JobTimeNote_Delete");
            DropStoredProcedure("dbo.JobTimeNote_Update");
            DropStoredProcedure("dbo.JobTimeNote_Insert");
            DropForeignKey("dbo.JobTimeNotes", "IdJob", "dbo.Jobs");
            DropIndex("dbo.JobTimeNotes", new[] { "IdJob" });
            DropTable("dbo.JobTimeNotes");
        }
    }
}
