namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobTransmittalAddTaskReference : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobTransmittals", "IdTask", c => c.Int());
            CreateIndex("dbo.JobTransmittals", "IdTask");
            AddForeignKey("dbo.JobTransmittals", "IdTask", "dbo.Tasks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobTransmittals", "IdTask", "dbo.Tasks");
            DropIndex("dbo.JobTransmittals", new[] { "IdTask" });
            DropColumn("dbo.JobTransmittals", "IdTask");
        }
    }
}
