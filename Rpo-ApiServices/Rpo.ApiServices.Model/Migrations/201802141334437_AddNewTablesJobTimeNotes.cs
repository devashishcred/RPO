namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewTablesJobTimeNotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobTimeNotes", "TimeNoteTime", c => c.String(maxLength: 5));
            AlterStoredProcedure(
                "dbo.JobTimeNote_Insert",
                p => new
                    {
                        IdJob = p.Int(),
                        IdJobTimeNoteCategory = p.Int(),
                        ProgressNotes = p.String(),
                        TimeNoteDate = p.DateTime(),
                        TimeNoteTime = p.String(maxLength: 5),
                        Billable = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[JobTimeNotes]([IdJob], [IdJobTimeNoteCategory], [ProgressNotes], [TimeNoteDate], [TimeNoteTime], [Billable], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@IdJob, @IdJobTimeNoteCategory, @ProgressNotes, @TimeNoteDate, @TimeNoteTime, @Billable, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
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
                        ProgressNotes = p.String(),
                        TimeNoteDate = p.DateTime(),
                        TimeNoteTime = p.String(maxLength: 5),
                        Billable = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[JobTimeNotes]
                      SET [IdJob] = @IdJob, [IdJobTimeNoteCategory] = @IdJobTimeNoteCategory, [ProgressNotes] = @ProgressNotes, [TimeNoteDate] = @TimeNoteDate, [TimeNoteTime] = @TimeNoteTime, [Billable] = @Billable, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobTimeNotes", "TimeNoteTime");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
