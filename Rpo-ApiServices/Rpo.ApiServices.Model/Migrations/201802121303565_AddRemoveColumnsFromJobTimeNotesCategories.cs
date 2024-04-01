namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRemoveColumnsFromJobTimeNotesCategories : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobTimeNoteCategories", "CreatedBy", c => c.Int());
            AddColumn("dbo.JobTimeNoteCategories", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.JobTimeNoteCategories", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.JobTimeNoteCategories", "LastModifiedDate", c => c.DateTime());
            CreateIndex("dbo.JobTimeNoteCategories", "CreatedBy");
            CreateIndex("dbo.JobTimeNoteCategories", "LastModifiedBy");
            AddForeignKey("dbo.JobTimeNoteCategories", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.JobTimeNoteCategories", "LastModifiedBy", "dbo.Employees", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobTimeNoteCategories", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobTimeNoteCategories", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.JobTimeNoteCategories", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobTimeNoteCategories", new[] { "CreatedBy" });
            DropColumn("dbo.JobTimeNoteCategories", "LastModifiedDate");
            DropColumn("dbo.JobTimeNoteCategories", "LastModifiedBy");
            DropColumn("dbo.JobTimeNoteCategories", "CreatedDate");
            DropColumn("dbo.JobTimeNoteCategories", "CreatedBy");
        }
    }
}
