namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdJobFeeScheduleMakeForeignkey : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TaskHistories", "IdJobFeeSchedule");
            RenameColumn(table: "dbo.TaskHistories", name: "JobFeeSchedule_Id", newName: "IdJobFeeSchedule");
            RenameIndex(table: "dbo.TaskHistories", name: "IX_JobFeeSchedule_Id", newName: "IX_IdJobFeeSchedule");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.TaskHistories", name: "IX_IdJobFeeSchedule", newName: "IX_JobFeeSchedule_Id");
            RenameColumn(table: "dbo.TaskHistories", name: "IdJobFeeSchedule", newName: "JobFeeSchedule_Id");
            AddColumn("dbo.TaskHistories", "IdJobFeeSchedule", c => c.Int());
        }
    }
}
