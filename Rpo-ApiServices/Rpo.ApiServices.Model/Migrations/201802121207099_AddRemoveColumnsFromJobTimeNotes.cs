namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRemoveColumnsFromJobTimeNotes : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.JobTimeNotes", new[] { "IdJob" });
            AddColumn("dbo.JobTimeNotes", "CreatedBy", c => c.Int());
            AddColumn("dbo.JobTimeNotes", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.JobTimeNotes", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.JobTimeNotes", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.JobTimeNotes", "IdJob", c => c.Int());
            CreateIndex("dbo.JobTimeNotes", "IdJob");
            CreateIndex("dbo.JobTimeNotes", "CreatedBy");
            CreateIndex("dbo.JobTimeNotes", "LastModifiedBy");
            AddForeignKey("dbo.JobTimeNotes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobTimeNotes", "LastModifiedBy", "dbo.Employees", "Id");
            DropColumn("dbo.JobTimeNotes", "Service");
            DropColumn("dbo.JobTimeNotes", "Hours");
            DropColumn("dbo.JobTimeNotes", "Minutes");
            AlterStoredProcedure(
                "dbo.JobTimeNote_Insert",
                p => new
                    {
                        IdJob = p.Int(),
                        IdJobTimeNoteCategory = p.Int(),
                        ProgressNotes = p.String(),
                        TimeNoteDate = p.DateTime(),
                        Billable = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[JobTimeNotes]([IdJob], [IdJobTimeNoteCategory], [ProgressNotes], [TimeNoteDate], [Billable], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@IdJob, @IdJobTimeNoteCategory, @ProgressNotes, @TimeNoteDate, @Billable, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
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
                        Billable = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[JobTimeNotes]
                      SET [IdJob] = @IdJob, [IdJobTimeNoteCategory] = @IdJobTimeNoteCategory, [ProgressNotes] = @ProgressNotes, [TimeNoteDate] = @TimeNoteDate, [Billable] = @Billable, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobTimeNotes", "Minutes", c => c.Int());
            AddColumn("dbo.JobTimeNotes", "Hours", c => c.Int());
            AddColumn("dbo.JobTimeNotes", "Service", c => c.String());
            DropForeignKey("dbo.JobTimeNotes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobTimeNotes", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.JobTimeNotes", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobTimeNotes", new[] { "CreatedBy" });
            DropIndex("dbo.JobTimeNotes", new[] { "IdJob" });
            AlterColumn("dbo.JobTimeNotes", "IdJob", c => c.Int(nullable: false));
            DropColumn("dbo.JobTimeNotes", "LastModifiedDate");
            DropColumn("dbo.JobTimeNotes", "LastModifiedBy");
            DropColumn("dbo.JobTimeNotes", "CreatedDate");
            DropColumn("dbo.JobTimeNotes", "CreatedBy");
            CreateIndex("dbo.JobTimeNotes", "IdJob");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
