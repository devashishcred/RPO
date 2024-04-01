namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeInJobTask : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.JobTaskNotes", newName: "TaskNotes");
            RenameTable(name: "dbo.JobTasks", newName: "Tasks");
            DropIndex("dbo.Tasks", new[] { "IdJob" });
            DropIndex("dbo.Tasks", new[] { "IdAssignedTo" });
            DropIndex("dbo.Tasks", new[] { "IdAssignedBy" });
            DropIndex("dbo.Tasks", new[] { "IdJobApplication" });
            DropIndex("dbo.Tasks", new[] { "IdWorkPermitType" });
            CreateTable(
                "dbo.TaskStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaskTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tasks", "AssignedDate", c => c.DateTime());
            AddColumn("dbo.Tasks", "IdTaskType", c => c.Int(nullable: false));
            AddColumn("dbo.Tasks", "IdTaskStatus", c => c.Int());
            AddColumn("dbo.Tasks", "IdRfp", c => c.Int());
            AddColumn("dbo.Tasks", "IdContact", c => c.Int());
            AddColumn("dbo.Tasks", "IdCompany", c => c.Int());
            AddColumn("dbo.Tasks", "LastModifiedDate", c => c.DateTime());
            AddColumn("dbo.Tasks", "LastModifiedBy", c => c.Int());
            AlterColumn("dbo.TaskNotes", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.TaskNotes", "LastModifiedBy", c => c.Int());
            AlterColumn("dbo.Tasks", "IdJob", c => c.Int());
            AlterColumn("dbo.Tasks", "IdAssignedTo", c => c.Int());
            AlterColumn("dbo.Tasks", "IdAssignedBy", c => c.Int());
            AlterColumn("dbo.Tasks", "IdJobApplication", c => c.Int());
            AlterColumn("dbo.Tasks", "IdWorkPermitType", c => c.Int());
            CreateIndex("dbo.Tasks", "IdAssignedTo");
            CreateIndex("dbo.Tasks", "IdAssignedBy");
            CreateIndex("dbo.Tasks", "IdTaskType");
            CreateIndex("dbo.Tasks", "IdTaskStatus");
            CreateIndex("dbo.Tasks", "IdJobApplication");
            CreateIndex("dbo.Tasks", "IdWorkPermitType");
            CreateIndex("dbo.Tasks", "IdJob");
            CreateIndex("dbo.Tasks", "IdRfp");
            CreateIndex("dbo.Tasks", "IdContact");
            CreateIndex("dbo.Tasks", "IdCompany");
            CreateIndex("dbo.Tasks", "LastModifiedBy");
            CreateIndex("dbo.TaskNotes", "LastModifiedBy");
            AddForeignKey("dbo.Tasks", "IdCompany", "dbo.Companies", "Id");
            AddForeignKey("dbo.Tasks", "IdContact", "dbo.Contacts", "Id");
            AddForeignKey("dbo.Tasks", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.TaskNotes", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Tasks", "IdRfp", "dbo.Rfps", "Id");
            AddForeignKey("dbo.Tasks", "IdTaskStatus", "dbo.TaskStatus", "Id");
            AddForeignKey("dbo.Tasks", "IdTaskType", "dbo.TaskTypes", "Id");
            DropColumn("dbo.Tasks", "Assigned");
            DropColumn("dbo.Tasks", "Status");
            DropColumn("dbo.Tasks", "ProgressCompletionNote");
            CreateStoredProcedure(
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
                        LastModifiedBy = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Tasks]([AssignedDate], [IdAssignedTo], [IdAssignedBy], [IdTaskType], [CompleteBy], [IdTaskStatus], [GeneralNotes], [IdJobApplication], [IdWorkPermitType], [IdJob], [IdRfp], [IdContact], [IdCompany], [LastModifiedDate], [LastModifiedBy])
                      VALUES (@AssignedDate, @IdAssignedTo, @IdAssignedBy, @IdTaskType, @CompleteBy, @IdTaskStatus, @GeneralNotes, @IdJobApplication, @IdWorkPermitType, @IdJob, @IdRfp, @IdContact, @IdCompany, @LastModifiedDate, @LastModifiedBy)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Tasks]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Tasks] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
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
                        LastModifiedBy = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Tasks]
                      SET [AssignedDate] = @AssignedDate, [IdAssignedTo] = @IdAssignedTo, [IdAssignedBy] = @IdAssignedBy, [IdTaskType] = @IdTaskType, [CompleteBy] = @CompleteBy, [IdTaskStatus] = @IdTaskStatus, [GeneralNotes] = @GeneralNotes, [IdJobApplication] = @IdJobApplication, [IdWorkPermitType] = @IdWorkPermitType, [IdJob] = @IdJob, [IdRfp] = @IdRfp, [IdContact] = @IdContact, [IdCompany] = @IdCompany, [LastModifiedDate] = @LastModifiedDate, [LastModifiedBy] = @LastModifiedBy
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Task_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Tasks]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.TaskNote_Insert",
                p => new
                    {
                        IdTask = p.Int(),
                        Notes = p.String(),
                        LastModifiedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[TaskNotes]([IdTask], [Notes], [LastModifiedDate], [LastModifiedBy])
                      VALUES (@IdTask, @Notes, @LastModifiedDate, @LastModifiedBy)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[TaskNotes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[TaskNotes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.TaskNote_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdTask = p.Int(),
                        Notes = p.String(),
                        LastModifiedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[TaskNotes]
                      SET [IdTask] = @IdTask, [Notes] = @Notes, [LastModifiedDate] = @LastModifiedDate, [LastModifiedBy] = @LastModifiedBy
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.TaskNote_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[TaskNotes]
                      WHERE ([Id] = @Id)"
            );
            
            DropStoredProcedure("dbo.JobTask_Insert");
            DropStoredProcedure("dbo.JobTask_Update");
            DropStoredProcedure("dbo.JobTask_Delete");
            DropStoredProcedure("dbo.JobTaskNote_Insert");
            DropStoredProcedure("dbo.JobTaskNote_Update");
            DropStoredProcedure("dbo.JobTaskNote_Delete");
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.TaskNote_Delete");
            DropStoredProcedure("dbo.TaskNote_Update");
            DropStoredProcedure("dbo.TaskNote_Insert");
            DropStoredProcedure("dbo.Task_Delete");
            DropStoredProcedure("dbo.Task_Update");
            DropStoredProcedure("dbo.Task_Insert");
            AddColumn("dbo.Tasks", "ProgressCompletionNote", c => c.String());
            AddColumn("dbo.Tasks", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Tasks", "Assigned", c => c.DateTime());
            DropForeignKey("dbo.Tasks", "IdTaskType", "dbo.TaskTypes");
            DropForeignKey("dbo.Tasks", "IdTaskStatus", "dbo.TaskStatus");
            DropForeignKey("dbo.Tasks", "IdRfp", "dbo.Rfps");
            DropForeignKey("dbo.TaskNotes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.Tasks", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.Tasks", "IdContact", "dbo.Contacts");
            DropForeignKey("dbo.Tasks", "IdCompany", "dbo.Companies");
            DropIndex("dbo.TaskNotes", new[] { "LastModifiedBy" });
            DropIndex("dbo.Tasks", new[] { "LastModifiedBy" });
            DropIndex("dbo.Tasks", new[] { "IdCompany" });
            DropIndex("dbo.Tasks", new[] { "IdContact" });
            DropIndex("dbo.Tasks", new[] { "IdRfp" });
            DropIndex("dbo.Tasks", new[] { "IdJob" });
            DropIndex("dbo.Tasks", new[] { "IdWorkPermitType" });
            DropIndex("dbo.Tasks", new[] { "IdJobApplication" });
            DropIndex("dbo.Tasks", new[] { "IdTaskStatus" });
            DropIndex("dbo.Tasks", new[] { "IdTaskType" });
            DropIndex("dbo.Tasks", new[] { "IdAssignedBy" });
            DropIndex("dbo.Tasks", new[] { "IdAssignedTo" });
            AlterColumn("dbo.Tasks", "IdWorkPermitType", c => c.Int(nullable: false));
            AlterColumn("dbo.Tasks", "IdJobApplication", c => c.Int(nullable: false));
            AlterColumn("dbo.Tasks", "IdAssignedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.Tasks", "IdAssignedTo", c => c.Int(nullable: false));
            AlterColumn("dbo.Tasks", "IdJob", c => c.Int(nullable: false));
            AlterColumn("dbo.TaskNotes", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.TaskNotes", "LastModifiedDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Tasks", "LastModifiedBy");
            DropColumn("dbo.Tasks", "LastModifiedDate");
            DropColumn("dbo.Tasks", "IdCompany");
            DropColumn("dbo.Tasks", "IdContact");
            DropColumn("dbo.Tasks", "IdRfp");
            DropColumn("dbo.Tasks", "IdTaskStatus");
            DropColumn("dbo.Tasks", "IdTaskType");
            DropColumn("dbo.Tasks", "AssignedDate");
            DropTable("dbo.TaskTypes");
            DropTable("dbo.TaskStatus");
            CreateIndex("dbo.Tasks", "IdWorkPermitType");
            CreateIndex("dbo.Tasks", "IdJobApplication");
            CreateIndex("dbo.Tasks", "IdAssignedBy");
            CreateIndex("dbo.Tasks", "IdAssignedTo");
            CreateIndex("dbo.Tasks", "IdJob");
            RenameTable(name: "dbo.Tasks", newName: "JobTasks");
            RenameTable(name: "dbo.TaskNotes", newName: "JobTaskNotes");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
