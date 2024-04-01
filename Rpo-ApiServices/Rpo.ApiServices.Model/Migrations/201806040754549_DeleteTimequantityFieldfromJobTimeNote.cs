namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteTimequantityFieldfromJobTimeNote : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.JobTimeNotes", "TimeQuantity");
            AlterStoredProcedure(
                "dbo.JobTimeNote_Insert",
                p => new
                    {
                        IdJob = p.Int(),
                        JobBillingType = p.Int(),
                        IdJobFeeSchedule = p.Int(),
                        IdRfpJobType = p.Int(),
                        IsQuickbookSynced = p.Boolean(),
                        QuickbookSyncedDate = p.DateTime(),
                        QuickbookSyncError = p.String(),
                        ProgressNotes = p.String(),
                        TimeNoteDate = p.DateTime(),
                        TimeHours = p.Short(),
                        TimeMinutes = p.Short(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[JobTimeNotes]([IdJob], [JobBillingType], [IdJobFeeSchedule], [IdRfpJobType], [IsQuickbookSynced], [QuickbookSyncedDate], [QuickbookSyncError], [ProgressNotes], [TimeNoteDate], [TimeHours], [TimeMinutes], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@IdJob, @JobBillingType, @IdJobFeeSchedule, @IdRfpJobType, @IsQuickbookSynced, @QuickbookSyncedDate, @QuickbookSyncError, @ProgressNotes, @TimeNoteDate, @TimeHours, @TimeMinutes, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
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
                        JobBillingType = p.Int(),
                        IdJobFeeSchedule = p.Int(),
                        IdRfpJobType = p.Int(),
                        IsQuickbookSynced = p.Boolean(),
                        QuickbookSyncedDate = p.DateTime(),
                        QuickbookSyncError = p.String(),
                        ProgressNotes = p.String(),
                        TimeNoteDate = p.DateTime(),
                        TimeHours = p.Short(),
                        TimeMinutes = p.Short(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[JobTimeNotes]
                      SET [IdJob] = @IdJob, [JobBillingType] = @JobBillingType, [IdJobFeeSchedule] = @IdJobFeeSchedule, [IdRfpJobType] = @IdRfpJobType, [IsQuickbookSynced] = @IsQuickbookSynced, [QuickbookSyncedDate] = @QuickbookSyncedDate, [QuickbookSyncError] = @QuickbookSyncError, [ProgressNotes] = @ProgressNotes, [TimeNoteDate] = @TimeNoteDate, [TimeHours] = @TimeHours, [TimeMinutes] = @TimeMinutes, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobTimeNotes", "TimeQuantity", c => c.Double());
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}