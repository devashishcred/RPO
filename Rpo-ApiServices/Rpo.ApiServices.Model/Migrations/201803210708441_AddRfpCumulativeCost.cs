namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRfpCumulativeCost : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobTypes", "IdParent", "dbo.JobTypes");
            DropForeignKey("dbo.JobTypes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.JobTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.WorkTypes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.WorkTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobTypeWorkTypes", "JobType_Id", "dbo.JobTypes");
            DropForeignKey("dbo.JobTypeWorkTypes", "WorkType_Id", "dbo.WorkTypes");
            DropForeignKey("dbo.WorkTypeNotes", "IdWorkType", "dbo.WorkTypes");
            DropIndex("dbo.JobTypes", new[] { "IdParent" });
            DropIndex("dbo.JobTypes", new[] { "CreatedBy" });
            DropIndex("dbo.JobTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.WorkTypes", new[] { "CreatedBy" });
            DropIndex("dbo.WorkTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.WorkTypeNotes", new[] { "IdWorkType" });
            DropIndex("dbo.JobTypeWorkTypes", new[] { "JobType_Id" });
            DropIndex("dbo.JobTypeWorkTypes", new[] { "WorkType_Id" });
            DropStoredProcedure("dbo.JobType_Insert");
            DropStoredProcedure("dbo.JobType_Update");
            DropStoredProcedure("dbo.JobType_Delete");
            DropStoredProcedure("dbo.WorkType_Insert");
            DropStoredProcedure("dbo.WorkType_Update");
            DropStoredProcedure("dbo.WorkType_Delete");
            DropStoredProcedure("dbo.WorkTypeNote_Insert");
            DropStoredProcedure("dbo.WorkTypeNote_Update");
            DropStoredProcedure("dbo.WorkTypeNote_Delete");
            DropTable("dbo.JobTypes");
            DropTable("dbo.WorkTypes");
            DropTable("dbo.WorkTypeNotes");
            DropTable("dbo.JobTypeWorkTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.JobTypeWorkTypes",
                c => new
                    {
                        JobType_Id = c.Int(nullable: false),
                        WorkType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.JobType_Id, t.WorkType_Id });
            
            CreateTable(
                "dbo.WorkTypeNotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdWorkType = c.Int(nullable: false),
                        Note = c.String(),
                        IdProjectDetail = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WorkTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                        Content = c.String(),
                        Number = c.String(maxLength: 5),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JobTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 100),
                        Content = c.String(),
                        Number = c.String(maxLength: 5),
                        IdParent = c.Int(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.JobTypeWorkTypes", "WorkType_Id");
            CreateIndex("dbo.JobTypeWorkTypes", "JobType_Id");
            CreateIndex("dbo.WorkTypeNotes", "IdWorkType");
            CreateIndex("dbo.WorkTypes", "LastModifiedBy");
            CreateIndex("dbo.WorkTypes", "CreatedBy");
            CreateIndex("dbo.JobTypes", "LastModifiedBy");
            CreateIndex("dbo.JobTypes", "CreatedBy");
            CreateIndex("dbo.JobTypes", "IdParent");
            AddForeignKey("dbo.WorkTypeNotes", "IdWorkType", "dbo.WorkTypes", "Id");
            AddForeignKey("dbo.JobTypeWorkTypes", "WorkType_Id", "dbo.WorkTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.JobTypeWorkTypes", "JobType_Id", "dbo.JobTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.WorkTypes", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.WorkTypes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobTypes", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobTypes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobTypes", "IdParent", "dbo.JobTypes", "Id");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
