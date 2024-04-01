namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Lastmodifiedby_jobfeeschedule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobFeeSchedules", "ModifiedBy", c => c.Int());
            CreateIndex("dbo.JobFeeSchedules", "ModifiedBy");
            AddForeignKey("dbo.JobFeeSchedules", "ModifiedBy", "dbo.Employees", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobFeeSchedules", "ModifiedBy", "dbo.Employees");
            DropIndex("dbo.JobFeeSchedules", new[] { "ModifiedBy" });
            DropColumn("dbo.JobFeeSchedules", "ModifiedBy");
        }
    }
}
