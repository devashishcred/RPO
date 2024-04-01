namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_IdmilestoneFieldinTaskandTaskHiostory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "IdMilestone", c => c.Int());
            AddColumn("dbo.TaskHistories", "IdMilestone", c => c.Int());
            CreateIndex("dbo.Tasks", "IdMilestone");
            CreateIndex("dbo.TaskHistories", "IdMilestone");
            AddForeignKey("dbo.Tasks", "IdMilestone", "dbo.JobMilestones", "Id");
            AddForeignKey("dbo.TaskHistories", "IdMilestone", "dbo.JobMilestones", "Id");
            AlterStoredProcedure(
                "dbo.Task_Insert",
                p => new
                    {
                        AssignedDate = p.DateTime(),
                        IdAssignedTo = p.Int(),
                        IdAssignedBy = p.Int(),
                        IdTaskType = p.Int(),
                        CompleteBy = p.DateTime(),
                        IdTaskStatus = p.Int(),
                        GeneralNotes = p.String(),
                        IdJobApplication = p.Int(),
                        IdWorkPermitType = p.String(),
                        IdJob = p.Int(),
                        IdRfp = p.Int(),
                        JobBillingType = p.Int(),
                        IdJobFeeSchedule = p.Int(),
                        ServiceQuantity = p.Double(),
                        IdRfpJobType = p.Int(),
                        IdContact = p.Int(),
                        IdCompany = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        ClosedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        IdExaminer = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        TaskNumber = p.String(),
                        TaskDuration = p.String(),
                        IdJobViolation = p.Int(),
                        IdJobType = p.Int(),
                        IdMilestone = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Tasks]([AssignedDate], [IdAssignedTo], [IdAssignedBy], [IdTaskType], [CompleteBy], [IdTaskStatus], [GeneralNotes], [IdJobApplication], [IdWorkPermitType], [IdJob], [IdRfp], [JobBillingType], [IdJobFeeSchedule], [ServiceQuantity], [IdRfpJobType], [IdContact], [IdCompany], [LastModifiedDate], [ClosedDate], [LastModifiedBy], [IdExaminer], [CreatedBy], [CreatedDate], [TaskNumber], [TaskDuration], [IdJobViolation], [IdJobType], [IdMilestone])
                      VALUES (@AssignedDate, @IdAssignedTo, @IdAssignedBy, @IdTaskType, @CompleteBy, @IdTaskStatus, @GeneralNotes, @IdJobApplication, @IdWorkPermitType, @IdJob, @IdRfp, @JobBillingType, @IdJobFeeSchedule, @ServiceQuantity, @IdRfpJobType, @IdContact, @IdCompany, @LastModifiedDate, @ClosedDate, @LastModifiedBy, @IdExaminer, @CreatedBy, @CreatedDate, @TaskNumber, @TaskDuration, @IdJobViolation, @IdJobType, @IdMilestone)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Tasks]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Tasks] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Task_Update",
                p => new
                    {
                        Id = p.Int(),
                        AssignedDate = p.DateTime(),
                        IdAssignedTo = p.Int(),
                        IdAssignedBy = p.Int(),
                        IdTaskType = p.Int(),
                        CompleteBy = p.DateTime(),
                        IdTaskStatus = p.Int(),
                        GeneralNotes = p.String(),
                        IdJobApplication = p.Int(),
                        IdWorkPermitType = p.String(),
                        IdJob = p.Int(),
                        IdRfp = p.Int(),
                        JobBillingType = p.Int(),
                        IdJobFeeSchedule = p.Int(),
                        ServiceQuantity = p.Double(),
                        IdRfpJobType = p.Int(),
                        IdContact = p.Int(),
                        IdCompany = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        ClosedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        IdExaminer = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        TaskNumber = p.String(),
                        TaskDuration = p.String(),
                        IdJobViolation = p.Int(),
                        IdJobType = p.Int(),
                        IdMilestone = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Tasks]
                      SET [AssignedDate] = @AssignedDate, [IdAssignedTo] = @IdAssignedTo, [IdAssignedBy] = @IdAssignedBy, [IdTaskType] = @IdTaskType, [CompleteBy] = @CompleteBy, [IdTaskStatus] = @IdTaskStatus, [GeneralNotes] = @GeneralNotes, [IdJobApplication] = @IdJobApplication, [IdWorkPermitType] = @IdWorkPermitType, [IdJob] = @IdJob, [IdRfp] = @IdRfp, [JobBillingType] = @JobBillingType, [IdJobFeeSchedule] = @IdJobFeeSchedule, [ServiceQuantity] = @ServiceQuantity, [IdRfpJobType] = @IdRfpJobType, [IdContact] = @IdContact, [IdCompany] = @IdCompany, [LastModifiedDate] = @LastModifiedDate, [ClosedDate] = @ClosedDate, [LastModifiedBy] = @LastModifiedBy, [IdExaminer] = @IdExaminer, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [TaskNumber] = @TaskNumber, [TaskDuration] = @TaskDuration, [IdJobViolation] = @IdJobViolation, [IdJobType] = @IdJobType, [IdMilestone] = @IdMilestone
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskHistories", "IdMilestone", "dbo.JobMilestones");
            DropForeignKey("dbo.Tasks", "IdMilestone", "dbo.JobMilestones");
            DropIndex("dbo.TaskHistories", new[] { "IdMilestone" });
            DropIndex("dbo.Tasks", new[] { "IdMilestone" });
            DropColumn("dbo.TaskHistories", "IdMilestone");
            DropColumn("dbo.Tasks", "IdMilestone");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
