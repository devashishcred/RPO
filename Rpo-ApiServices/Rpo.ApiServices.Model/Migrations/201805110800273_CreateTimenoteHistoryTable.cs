namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTimenoteHistoryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobTimeNoteHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 400),
                        IdEmployee = c.Int(),
                        HistoryDate = c.DateTime(nullable: false),
                        IdJobTimeNote = c.Int(nullable: false),
                        IdJobFeeSchedule = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.IdEmployee)
                .ForeignKey("dbo.JobFeeSchedules", t => t.IdJobFeeSchedule)
                .ForeignKey("dbo.JobTimeNotes", t => t.IdJobTimeNote)
                .Index(t => t.IdEmployee)
                .Index(t => t.IdJobTimeNote)
                .Index(t => t.IdJobFeeSchedule);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobTimeNoteHistories", "IdJobTimeNote", "dbo.JobTimeNotes");
            DropForeignKey("dbo.JobTimeNoteHistories", "IdJobFeeSchedule", "dbo.JobFeeSchedules");
            DropForeignKey("dbo.JobTimeNoteHistories", "IdEmployee", "dbo.Employees");
            DropIndex("dbo.JobTimeNoteHistories", new[] { "IdJobFeeSchedule" });
            DropIndex("dbo.JobTimeNoteHistories", new[] { "IdJobTimeNote" });
            DropIndex("dbo.JobTimeNoteHistories", new[] { "IdEmployee" });
            DropTable("dbo.JobTimeNoteHistories");
        }
    }
}
