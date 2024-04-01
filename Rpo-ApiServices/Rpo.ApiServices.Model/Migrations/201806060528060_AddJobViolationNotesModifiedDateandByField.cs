namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJobViolationNotesModifiedDateandByField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobViolations", "NotesLastModifiedBy", c => c.Int());
            AddColumn("dbo.JobViolations", "NotesLastModifiedDate", c => c.DateTime());
            CreateIndex("dbo.JobViolations", "NotesLastModifiedBy");
            AddForeignKey("dbo.JobViolations", "NotesLastModifiedBy", "dbo.Employees", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobViolations", "NotesLastModifiedBy", "dbo.Employees");
            DropIndex("dbo.JobViolations", new[] { "NotesLastModifiedBy" });
            DropColumn("dbo.JobViolations", "NotesLastModifiedDate");
            DropColumn("dbo.JobViolations", "NotesLastModifiedBy");
        }
    }
}
