namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesMerge : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.IdEmployee)
                .ForeignKey("dbo.JobFeeSchedules", t => t.JobFeeSchedule_Id)
                .ForeignKey("dbo.Tasks", t => t.IdTask)
                .Index(t => t.IdEmployee)
                .Index(t => t.IdTask)
                .Index(t => t.JobFeeSchedule_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskHistories", "IdTask", "dbo.Tasks");
            DropForeignKey("dbo.TaskHistories", "JobFeeSchedule_Id", "dbo.JobFeeSchedules");
            DropForeignKey("dbo.TaskHistories", "IdEmployee", "dbo.Employees");
            DropIndex("dbo.TaskHistories", new[] { "JobFeeSchedule_Id" });
            DropIndex("dbo.TaskHistories", new[] { "IdTask" });
            DropIndex("dbo.TaskHistories", new[] { "IdEmployee" });
            DropTable("dbo.TaskHistories");
        }
    }
}
