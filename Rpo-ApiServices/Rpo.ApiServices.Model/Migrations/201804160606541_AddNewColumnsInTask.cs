namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewColumnsInTask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "JobBillingType", c => c.Int());
            AddColumn("dbo.Tasks", "IdJobFeeSchedule", c => c.Int());
            AddColumn("dbo.Tasks", "IdRfpJobType", c => c.Int());
            CreateIndex("dbo.Tasks", "IdJobFeeSchedule");
            CreateIndex("dbo.Tasks", "IdRfpJobType");
            AddForeignKey("dbo.Tasks", "IdJobFeeSchedule", "dbo.JobFeeSchedules", "Id");
            AddForeignKey("dbo.Tasks", "IdRfpJobType", "dbo.RfpJobTypes", "Id");
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
                        IdWorkPermitType = p.Int(),
                        IdJob = p.Int(),
                        IdRfp = p.Int(),
                        JobBillingType = p.Int(),
                        IdJobFeeSchedule = p.Int(),
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
                    },
                body:
                    @"INSERT [dbo].[Tasks]([AssignedDate], [IdAssignedTo], [IdAssignedBy], [IdTaskType], [CompleteBy], [IdTaskStatus], [GeneralNotes], [IdJobApplication], [IdWorkPermitType], [IdJob], [IdRfp], [JobBillingType], [IdJobFeeSchedule], [IdRfpJobType], [IdContact], [IdCompany], [LastModifiedDate], [ClosedDate], [LastModifiedBy], [IdExaminer], [CreatedBy], [CreatedDate], [TaskNumber])
                      VALUES (@AssignedDate, @IdAssignedTo, @IdAssignedBy, @IdTaskType, @CompleteBy, @IdTaskStatus, @GeneralNotes, @IdJobApplication, @IdWorkPermitType, @IdJob, @IdRfp, @JobBillingType, @IdJobFeeSchedule, @IdRfpJobType, @IdContact, @IdCompany, @LastModifiedDate, @ClosedDate, @LastModifiedBy, @IdExaminer, @CreatedBy, @CreatedDate, @TaskNumber)
                      
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
                        IdWorkPermitType = p.Int(),
                        IdJob = p.Int(),
                        IdRfp = p.Int(),
                        JobBillingType = p.Int(),
                        IdJobFeeSchedule = p.Int(),
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
                    },
                body:
                    @"UPDATE [dbo].[Tasks]
                      SET [AssignedDate] = @AssignedDate, [IdAssignedTo] = @IdAssignedTo, [IdAssignedBy] = @IdAssignedBy, [IdTaskType] = @IdTaskType, [CompleteBy] = @CompleteBy, [IdTaskStatus] = @IdTaskStatus, [GeneralNotes] = @GeneralNotes, [IdJobApplication] = @IdJobApplication, [IdWorkPermitType] = @IdWorkPermitType, [IdJob] = @IdJob, [IdRfp] = @IdRfp, [JobBillingType] = @JobBillingType, [IdJobFeeSchedule] = @IdJobFeeSchedule, [IdRfpJobType] = @IdRfpJobType, [IdContact] = @IdContact, [IdCompany] = @IdCompany, [LastModifiedDate] = @LastModifiedDate, [ClosedDate] = @ClosedDate, [LastModifiedBy] = @LastModifiedBy, [IdExaminer] = @IdExaminer, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [TaskNumber] = @TaskNumber
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "IdRfpJobType", "dbo.RfpJobTypes");
            DropForeignKey("dbo.Tasks", "IdJobFeeSchedule", "dbo.JobFeeSchedules");
            DropIndex("dbo.Tasks", new[] { "IdRfpJobType" });
            DropIndex("dbo.Tasks", new[] { "IdJobFeeSchedule" });
            DropColumn("dbo.Tasks", "IdRfpJobType");
            DropColumn("dbo.Tasks", "IdJobFeeSchedule");
            DropColumn("dbo.Tasks", "JobBillingType");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
