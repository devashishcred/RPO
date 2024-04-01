namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeJobTimeNoteCategoryNameUnique : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JobTimeNoteCategories", "Name", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.JobTimeNoteCategories", "Name", unique: true, name: "IX_JobTimeNoteCategoryName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.JobTimeNoteCategories", "IX_JobTimeNoteCategoryName");
            AlterColumn("dbo.JobTimeNoteCategories", "Name", c => c.String());
        }
    }
}
