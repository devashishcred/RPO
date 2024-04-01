namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateJobApplicationType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobApplicationTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 100),
                        Content = c.String(),
                        Number = c.String(maxLength: 5),
                        IdParent = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobApplicationTypes", t => t.IdParent)
                .Index(t => t.IdParent);

            CreateTable(
                "dbo.JobWorkTypes",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Description = c.String(nullable: false, maxLength: 50),
                    Content = c.String(),
                    Number = c.String(maxLength: 5)
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.JobApplicationTypeJobWorkTypes",
                c => new
                {
                    JobApplicationType_Id = c.Int(nullable: false),
                    JobWorkType_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.JobApplicationType_Id, t.JobWorkType_Id })
                .ForeignKey("dbo.JobApplicationTypes", t => t.JobApplicationType_Id, cascadeDelete: true)
                .ForeignKey("dbo.JobWorkTypes", t => t.JobWorkType_Id, cascadeDelete: true)
                .Index(t => t.JobApplicationType_Id)
                .Index(t => t.JobWorkType_Id);

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobApplicationTypeJobWorkTypes", "JobWorkType_Id", "dbo.JobWorkTypes");
            DropForeignKey("dbo.JobApplicationTypeJobWorkTypes", "JobApplicationType_Id", "dbo.JobApplicationTypes");
            DropForeignKey("dbo.JobApplicationTypes", "IdParent", "dbo.JobApplicationTypes");
            DropIndex("dbo.JobApplicationTypeJobWorkTypes", new[] { "JobWorkType_Id" });
            DropIndex("dbo.JobApplicationTypeJobWorkTypes", new[] { "JobApplicationType_Id" });
            DropIndex("dbo.JobApplicationTypes", new[] { "IdParent" });
            DropTable("dbo.JobApplicationTypeJobWorkTypes");
            DropTable("dbo.JobWorkTypes");
            DropTable("dbo.JobApplicationTypes");
        }
    }
}
