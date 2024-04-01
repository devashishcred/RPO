namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobApplicationTypeChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobWorkTypes", "IdJobApplicationType", "dbo.JobApplicationTypes");
            DropIndex("dbo.JobWorkTypes", new[] { "JobApplicationType_Id" });
            DropIndex("dbo.JobWorkTypes", new[] { "IdJobApplicationType" });
            DropColumn("dbo.JobWorkTypes", "IdJobApplicationType");
            RenameColumn(table: "dbo.JobWorkTypes", name: "JobApplicationType_Id", newName: "IdJobApplicationType");
            AlterColumn("dbo.JobWorkTypes", "IdJobApplicationType", c => c.Int());
            CreateIndex("dbo.JobWorkTypes", "IdJobApplicationType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.JobWorkTypes", new[] { "IdJobApplicationType" });
            AlterColumn("dbo.JobWorkTypes", "IdJobApplicationType", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.JobWorkTypes", name: "IdJobApplicationType", newName: "JobApplicationType_Id");
            AddColumn("dbo.JobWorkTypes", "IdJobApplicationType", c => c.Int());
            CreateIndex("dbo.JobWorkTypes", "IdJobApplicationType");
            CreateIndex("dbo.JobWorkTypes", "JobApplicationType_Id");
            AddForeignKey("dbo.JobWorkTypes", "IdJobApplicationType", "dbo.JobApplicationTypes", "Id");
        }
    }
}
