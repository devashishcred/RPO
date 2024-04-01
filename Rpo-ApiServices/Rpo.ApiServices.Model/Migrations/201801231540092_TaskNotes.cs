namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskNotes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobTasks", "IdWorkType", "dbo.WorkTypes");
            DropIndex("dbo.JobTasks", new[] { "IdWorkType" });
            RenameColumn(table: "dbo.JobTasks", name: "IdBy", newName: "IdAssignedBy");
            RenameColumn(table: "dbo.JobTasks", name: "IdTo", newName: "IdAssignedTo");
            RenameIndex(table: "dbo.JobTasks", name: "IX_IdTo", newName: "IX_IdAssignedTo");
            RenameIndex(table: "dbo.JobTasks", name: "IX_IdBy", newName: "IX_IdAssignedBy");
            CreateTable(
                "dbo.JobTaskNotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdTask = c.Int(nullable: false),
                        Notes = c.String(),
                        LastModifiedDate = c.DateTime(nullable: false),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobTasks", t => t.IdTask)
                .Index(t => t.IdTask);
            
            AddColumn("dbo.JobTasks", "IdWorkPermitType", c => c.Int(nullable: false));
            CreateIndex("dbo.JobTasks", "IdWorkPermitType");
            AddForeignKey("dbo.JobTasks", "IdWorkPermitType", "dbo.JobApplicationWorkPermitTypes", "Id");
            DropColumn("dbo.JobTasks", "IdWorkType");
            CreateStoredProcedure(
                "dbo.JobTask_Insert",
                p => new
                    {
                        IdJob = p.Int(),
                        Assigned = p.DateTime(),
                        IdAssignedTo = p.Int(),
                        IdAssignedBy = p.Int(),
                        CompleteBy = p.DateTime(),
                        Status = p.Int(),
                        GeneralNotes = p.String(),
                        IdJobApplication = p.Int(),
                        IdWorkPermitType = p.Int(),
                        ProgressCompletionNote = p.String(),
                    },
                body:
                    @"INSERT [dbo].[JobTasks]([IdJob], [Assigned], [IdAssignedTo], [IdAssignedBy], [CompleteBy], [Status], [GeneralNotes], [IdJobApplication], [IdWorkPermitType], [ProgressCompletionNote])
                      VALUES (@IdJob, @Assigned, @IdAssignedTo, @IdAssignedBy, @CompleteBy, @Status, @GeneralNotes, @IdJobApplication, @IdWorkPermitType, @ProgressCompletionNote)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[JobTasks]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[JobTasks] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.JobTask_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdJob = p.Int(),
                        Assigned = p.DateTime(),
                        IdAssignedTo = p.Int(),
                        IdAssignedBy = p.Int(),
                        CompleteBy = p.DateTime(),
                        Status = p.Int(),
                        GeneralNotes = p.String(),
                        IdJobApplication = p.Int(),
                        IdWorkPermitType = p.Int(),
                        ProgressCompletionNote = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[JobTasks]
                      SET [IdJob] = @IdJob, [Assigned] = @Assigned, [IdAssignedTo] = @IdAssignedTo, [IdAssignedBy] = @IdAssignedBy, [CompleteBy] = @CompleteBy, [Status] = @Status, [GeneralNotes] = @GeneralNotes, [IdJobApplication] = @IdJobApplication, [IdWorkPermitType] = @IdWorkPermitType, [ProgressCompletionNote] = @ProgressCompletionNote
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.JobTask_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[JobTasks]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.JobTaskNote_Insert",
                p => new
                    {
                        IdTask = p.Int(),
                        Notes = p.String(),
                        LastModifiedDate = p.DateTime(),
                        LastModifiedBy = p.String(),
                    },
                body:
                    @"INSERT [dbo].[JobTaskNotes]([IdTask], [Notes], [LastModifiedDate], [LastModifiedBy])
                      VALUES (@IdTask, @Notes, @LastModifiedDate, @LastModifiedBy)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[JobTaskNotes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[JobTaskNotes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.JobTaskNote_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdTask = p.Int(),
                        Notes = p.String(),
                        LastModifiedDate = p.DateTime(),
                        LastModifiedBy = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[JobTaskNotes]
                      SET [IdTask] = @IdTask, [Notes] = @Notes, [LastModifiedDate] = @LastModifiedDate, [LastModifiedBy] = @LastModifiedBy
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.JobTaskNote_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[JobTaskNotes]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.JobTaskNote_Delete");
            DropStoredProcedure("dbo.JobTaskNote_Update");
            DropStoredProcedure("dbo.JobTaskNote_Insert");
            DropStoredProcedure("dbo.JobTask_Delete");
            DropStoredProcedure("dbo.JobTask_Update");
            DropStoredProcedure("dbo.JobTask_Insert");
            AddColumn("dbo.JobTasks", "IdWorkType", c => c.Int(nullable: false));
            DropForeignKey("dbo.JobTasks", "IdWorkPermitType", "dbo.JobApplicationWorkPermitTypes");
            DropForeignKey("dbo.JobTaskNotes", "IdTask", "dbo.JobTasks");
            DropIndex("dbo.JobTaskNotes", new[] { "IdTask" });
            DropIndex("dbo.JobTasks", new[] { "IdWorkPermitType" });
            DropColumn("dbo.JobTasks", "IdWorkPermitType");
            DropTable("dbo.JobTaskNotes");
            RenameIndex(table: "dbo.JobTasks", name: "IX_IdAssignedBy", newName: "IX_IdBy");
            RenameIndex(table: "dbo.JobTasks", name: "IX_IdAssignedTo", newName: "IX_IdTo");
            RenameColumn(table: "dbo.JobTasks", name: "IdAssignedTo", newName: "IdTo");
            RenameColumn(table: "dbo.JobTasks", name: "IdAssignedBy", newName: "IdBy");
            CreateIndex("dbo.JobTasks", "IdWorkType");
            AddForeignKey("dbo.JobTasks", "IdWorkType", "dbo.WorkTypes", "Id");
        }
    }
}
