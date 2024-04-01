namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCloseDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "ClosedDate", c => c.DateTime());
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
                        IdContact = p.Int(),
                        IdCompany = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        ClosedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Tasks]([AssignedDate], [IdAssignedTo], [IdAssignedBy], [IdTaskType], [CompleteBy], [IdTaskStatus], [GeneralNotes], [IdJobApplication], [IdWorkPermitType], [IdJob], [IdRfp], [IdContact], [IdCompany], [LastModifiedDate], [ClosedDate], [LastModifiedBy])
                      VALUES (@AssignedDate, @IdAssignedTo, @IdAssignedBy, @IdTaskType, @CompleteBy, @IdTaskStatus, @GeneralNotes, @IdJobApplication, @IdWorkPermitType, @IdJob, @IdRfp, @IdContact, @IdCompany, @LastModifiedDate, @ClosedDate, @LastModifiedBy)
                      
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
                        IdContact = p.Int(),
                        IdCompany = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        ClosedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Tasks]
                      SET [AssignedDate] = @AssignedDate, [IdAssignedTo] = @IdAssignedTo, [IdAssignedBy] = @IdAssignedBy, [IdTaskType] = @IdTaskType, [CompleteBy] = @CompleteBy, [IdTaskStatus] = @IdTaskStatus, [GeneralNotes] = @GeneralNotes, [IdJobApplication] = @IdJobApplication, [IdWorkPermitType] = @IdWorkPermitType, [IdJob] = @IdJob, [IdRfp] = @IdRfp, [IdContact] = @IdContact, [IdCompany] = @IdCompany, [LastModifiedDate] = @LastModifiedDate, [ClosedDate] = @ClosedDate, [LastModifiedBy] = @LastModifiedBy
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "ClosedDate");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
