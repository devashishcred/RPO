namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApplicationStatusTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.JobApplications", "IdApplicationStatus", c => c.Int(nullable: false));
            CreateIndex("dbo.JobApplications", "IdApplicationStatus");
            AddForeignKey("dbo.JobApplications", "IdApplicationStatus", "dbo.ApplicationStatus", "Id");
            DropColumn("dbo.JobApplications", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobApplications", "Status", c => c.Int(nullable: false));
            DropForeignKey("dbo.JobApplications", "IdApplicationStatus", "dbo.ApplicationStatus");
            DropIndex("dbo.JobApplications", new[] { "IdApplicationStatus" });
            DropColumn("dbo.JobApplications", "IdApplicationStatus");
            DropTable("dbo.ApplicationStatus");
        }
    }
}
