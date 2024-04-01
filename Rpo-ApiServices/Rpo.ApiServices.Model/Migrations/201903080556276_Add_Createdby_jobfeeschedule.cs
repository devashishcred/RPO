namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Createdby_jobfeeschedule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobFeeSchedules", "CreatedBy", c => c.Int());
            AddColumn("dbo.JobFeeSchedules", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.JobFeeSchedules", "LastModified", c => c.DateTime());
            CreateIndex("dbo.JobFeeSchedules", "CreatedBy");
            AddForeignKey("dbo.JobFeeSchedules", "CreatedBy", "dbo.Employees", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobFeeSchedules", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.JobFeeSchedules", new[] { "CreatedBy" });
            DropColumn("dbo.JobFeeSchedules", "LastModified");
            DropColumn("dbo.JobFeeSchedules", "CreatedDate");
            DropColumn("dbo.JobFeeSchedules", "CreatedBy");
        }
    }
}
