namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobWorkTypeChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobWorkTypes", "Cost", c => c.Double());
            AddColumn("dbo.JobWorkTypes", "IdJobApplicationType", c => c.Int());
            AlterColumn("dbo.JobWorkTypes", "Description", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.JobWorkTypes", "IdJobApplicationType");
            AddForeignKey("dbo.JobWorkTypes", "IdJobApplicationType", "dbo.JobApplicationTypes", "Id");
            DropColumn("dbo.JobApplicationTypes", "Number");
            DropColumn("dbo.JobWorkTypes", "Number");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobWorkTypes", "Number", c => c.String(maxLength: 5));
            AddColumn("dbo.JobApplicationTypes", "Number", c => c.String(maxLength: 5));
            DropForeignKey("dbo.JobWorkTypes", "IdJobApplicationType", "dbo.JobApplicationTypes");
            DropIndex("dbo.JobWorkTypes", new[] { "IdJobApplicationType" });
            AlterColumn("dbo.JobWorkTypes", "Description", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.JobWorkTypes", "IdJobApplicationType");
            DropColumn("dbo.JobWorkTypes", "Cost");
        }
    }
}
