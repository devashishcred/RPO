namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJobIdColumninJobHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobHistories", "IdJob", c => c.Int(nullable: false));
            CreateIndex("dbo.JobHistories", "IdJob");
            AddForeignKey("dbo.JobHistories", "IdJob", "dbo.Jobs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobHistories", "IdJob", "dbo.Jobs");
            DropIndex("dbo.JobHistories", new[] { "IdJob" });
            DropColumn("dbo.JobHistories", "IdJob");
        }
    }
}
