namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentFieldsPdffieldscolumnadded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TaskHistories", "IdEmployee", "dbo.Employees");
            DropForeignKey("dbo.TaskHistories", "JobFeeSchedule_Id", "dbo.JobFeeSchedules");
            DropForeignKey("dbo.TaskHistories", "IdTask", "dbo.Tasks");
            DropIndex("dbo.TaskHistories", new[] { "IdEmployee" });
            DropIndex("dbo.TaskHistories", new[] { "IdTask" });
            DropIndex("dbo.TaskHistories", new[] { "JobFeeSchedule_Id" });
            AddColumn("dbo.DocumentFields", "PdfFields", c => c.String());
            DropTable("dbo.TaskHistories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TaskHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 400),
                        IdEmployee = c.Int(),
                        HistoryDate = c.DateTime(nullable: false),
                        IdTask = c.Int(nullable: false),
                        IdJobFeeSchedule = c.Int(),
                        JobFeeSchedule_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.DocumentFields", "PdfFields");
            CreateIndex("dbo.TaskHistories", "JobFeeSchedule_Id");
            CreateIndex("dbo.TaskHistories", "IdTask");
            CreateIndex("dbo.TaskHistories", "IdEmployee");
            AddForeignKey("dbo.TaskHistories", "IdTask", "dbo.Tasks", "Id");
            AddForeignKey("dbo.TaskHistories", "JobFeeSchedule_Id", "dbo.JobFeeSchedules", "Id");
            AddForeignKey("dbo.TaskHistories", "IdEmployee", "dbo.Employees", "Id");
        }
    }
}
