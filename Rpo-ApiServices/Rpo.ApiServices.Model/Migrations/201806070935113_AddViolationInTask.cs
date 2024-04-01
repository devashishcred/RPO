namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddViolationInTask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "IdJobViolation", c => c.Int());
            CreateIndex("dbo.Tasks", "IdJobViolation");
            AddForeignKey("dbo.Tasks", "IdJobViolation", "dbo.JobViolations", "Id");
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
                        TaskDuration = p.Double(),
                        IdJobViolation = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Tasks]([AssignedDate], [IdAssignedTo], [IdAssignedBy], [IdTaskType], [CompleteBy], [IdTaskStatus], [GeneralNotes], [IdJobApplication], [IdWorkPermitType], [IdJob], [IdRfp], [JobBillingType], [IdJobFeeSchedule], [ServiceQuantity], [IdRfpJobType], [IdContact], [IdCompany], [LastModifiedDate], [ClosedDate], [LastModifiedBy], [IdExaminer], [CreatedBy], [CreatedDate], [TaskNumber], [TaskDuration], [IdJobViolation])
                      VALUES (@AssignedDate, @IdAssignedTo, @IdAssignedBy, @IdTaskType, @CompleteBy, @IdTaskStatus, @GeneralNotes, @IdJobApplication, @IdWorkPermitType, @IdJob, @IdRfp, @JobBillingType, @IdJobFeeSchedule, @ServiceQuantity, @IdRfpJobType, @IdContact, @IdCompany, @LastModifiedDate, @ClosedDate, @LastModifiedBy, @IdExaminer, @CreatedBy, @CreatedDate, @TaskNumber, @TaskDuration, @IdJobViolation)
                      
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
                        TaskDuration = p.Double(),
                        IdJobViolation = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Tasks]
                      SET [AssignedDate] = @AssignedDate, [IdAssignedTo] = @IdAssignedTo, [IdAssignedBy] = @IdAssignedBy, [IdTaskType] = @IdTaskType, [CompleteBy] = @CompleteBy, [IdTaskStatus] = @IdTaskStatus, [GeneralNotes] = @GeneralNotes, [IdJobApplication] = @IdJobApplication, [IdWorkPermitType] = @IdWorkPermitType, [IdJob] = @IdJob, [IdRfp] = @IdRfp, [JobBillingType] = @JobBillingType, [IdJobFeeSchedule] = @IdJobFeeSchedule, [ServiceQuantity] = @ServiceQuantity, [IdRfpJobType] = @IdRfpJobType, [IdContact] = @IdContact, [IdCompany] = @IdCompany, [LastModifiedDate] = @LastModifiedDate, [ClosedDate] = @ClosedDate, [LastModifiedBy] = @LastModifiedBy, [IdExaminer] = @IdExaminer, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [TaskNumber] = @TaskNumber, [TaskDuration] = @TaskDuration, [IdJobViolation] = @IdJobViolation
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "IdJobViolation", "dbo.JobViolations");
            DropIndex("dbo.Tasks", new[] { "IdJobViolation" });
            DropColumn("dbo.Tasks", "IdJobViolation");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
