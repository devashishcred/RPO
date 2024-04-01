namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTableCreatedJobWorkPermitHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobWorkPermitHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdWorkPermit = c.Int(nullable: false),
                        IdJobApplication = c.Int(nullable: false),
                        Description = c.String(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.JobApplications", t => t.IdJobApplication)
                .ForeignKey("dbo.JobApplicationWorkPermitTypes", t => t.IdWorkPermit)
                .Index(t => t.IdWorkPermit)
                .Index(t => t.IdJobApplication)
                .Index(t => t.CreatedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobWorkPermitHistories", "IdWorkPermit", "dbo.JobApplicationWorkPermitTypes");
            DropForeignKey("dbo.JobWorkPermitHistories", "IdJobApplication", "dbo.JobApplications");
            DropForeignKey("dbo.JobWorkPermitHistories", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.JobWorkPermitHistories", new[] { "CreatedBy" });
            DropIndex("dbo.JobWorkPermitHistories", new[] { "IdJobApplication" });
            DropIndex("dbo.JobWorkPermitHistories", new[] { "IdWorkPermit" });
            DropTable("dbo.JobWorkPermitHistories");
        }
    }
}
