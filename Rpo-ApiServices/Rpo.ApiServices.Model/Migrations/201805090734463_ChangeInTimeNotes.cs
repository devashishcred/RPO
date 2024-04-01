namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeInTimeNotes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobTimeNotes", "IdJobTimeNoteCategory", "dbo.JobTimeNoteCategories");
            DropIndex("dbo.JobTimeNotes", new[] { "IdJobTimeNoteCategory" });
            AddColumn("dbo.JobTimeNotes", "JobBillingType", c => c.Int(nullable: false));
            AddColumn("dbo.JobTimeNotes", "IdJobFeeSchedule", c => c.Int());
            AddColumn("dbo.JobTimeNotes", "IdRfpJobType", c => c.Int());
            AddColumn("dbo.JobTimeNotes", "IsQuickbookSynced", c => c.Boolean());
            AddColumn("dbo.JobTimeNotes", "QuickbookSyncedDate", c => c.DateTime());
            AddColumn("dbo.JobTimeNotes", "QuickbookSyncError", c => c.String());
            AddColumn("dbo.JobTimeNotes", "TimeQuantity", c => c.Double());
            CreateIndex("dbo.JobTimeNotes", "IdJobFeeSchedule");
            CreateIndex("dbo.JobTimeNotes", "IdRfpJobType");
            AddForeignKey("dbo.JobTimeNotes", "IdJobFeeSchedule", "dbo.JobFeeSchedules", "Id");
            AddForeignKey("dbo.JobTimeNotes", "IdRfpJobType", "dbo.RfpJobTypes", "Id");
            DropColumn("dbo.JobTimeNotes", "IdJobTimeNoteCategory");
            DropColumn("dbo.JobTimeNotes", "TimeNoteTime");
            DropColumn("dbo.JobTimeNotes", "Billable");
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
                        TimeQuantity = p.Double(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[JobTimeNotes]([IdJob], [JobBillingType], [IdJobFeeSchedule], [IdRfpJobType], [IsQuickbookSynced], [QuickbookSyncedDate], [QuickbookSyncError], [ProgressNotes], [TimeNoteDate], [TimeQuantity], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@IdJob, @JobBillingType, @IdJobFeeSchedule, @IdRfpJobType, @IsQuickbookSynced, @QuickbookSyncedDate, @QuickbookSyncError, @ProgressNotes, @TimeNoteDate, @TimeQuantity, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
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
                        TimeQuantity = p.Double(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[JobTimeNotes]
                      SET [IdJob] = @IdJob, [JobBillingType] = @JobBillingType, [IdJobFeeSchedule] = @IdJobFeeSchedule, [IdRfpJobType] = @IdRfpJobType, [IsQuickbookSynced] = @IsQuickbookSynced, [QuickbookSyncedDate] = @QuickbookSyncedDate, [QuickbookSyncError] = @QuickbookSyncError, [ProgressNotes] = @ProgressNotes, [TimeNoteDate] = @TimeNoteDate, [TimeQuantity] = @TimeQuantity, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobTimeNotes", "Billable", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobTimeNotes", "TimeNoteTime", c => c.String(maxLength: 5));
            AddColumn("dbo.JobTimeNotes", "IdJobTimeNoteCategory", c => c.Int());
            DropForeignKey("dbo.JobTimeNotes", "IdRfpJobType", "dbo.RfpJobTypes");
            DropForeignKey("dbo.JobTimeNotes", "IdJobFeeSchedule", "dbo.JobFeeSchedules");
            DropIndex("dbo.JobTimeNotes", new[] { "IdRfpJobType" });
            DropIndex("dbo.JobTimeNotes", new[] { "IdJobFeeSchedule" });
            DropColumn("dbo.JobTimeNotes", "TimeQuantity");
            DropColumn("dbo.JobTimeNotes", "QuickbookSyncError");
            DropColumn("dbo.JobTimeNotes", "QuickbookSyncedDate");
            DropColumn("dbo.JobTimeNotes", "IsQuickbookSynced");
            DropColumn("dbo.JobTimeNotes", "IdRfpJobType");
            DropColumn("dbo.JobTimeNotes", "IdJobFeeSchedule");
            DropColumn("dbo.JobTimeNotes", "JobBillingType");
            CreateIndex("dbo.JobTimeNotes", "IdJobTimeNoteCategory");
            AddForeignKey("dbo.JobTimeNotes", "IdJobTimeNoteCategory", "dbo.JobTimeNoteCategories", "Id");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
