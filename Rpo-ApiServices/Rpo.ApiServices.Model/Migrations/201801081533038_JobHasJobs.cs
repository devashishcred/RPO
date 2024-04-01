namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobHasJobs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobJobs",
                c => new
                    {
                        Job_Id = c.Int(nullable: false),
                        Job_Id1 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Job_Id, t.Job_Id1 })
                .ForeignKey("dbo.Jobs", t => t.Job_Id)
                .ForeignKey("dbo.Jobs", t => t.Job_Id1)
                .Index(t => t.Job_Id)
                .Index(t => t.Job_Id1);
            
            CreateStoredProcedure(
                "dbo.JobJob_Insert",
                p => new
                    {
                        Job_Id = p.Int(),
                        Job_Id1 = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[JobJobs]([Job_Id], [Job_Id1])
                      VALUES (@Job_Id, @Job_Id1)"
            );
            
            CreateStoredProcedure(
                "dbo.JobJob_Delete",
                p => new
                    {
                        Job_Id = p.Int(),
                        Job_Id1 = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[JobJobs]
                      WHERE (([Job_Id] = @Job_Id) AND ([Job_Id1] = @Job_Id1))"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.JobJob_Delete");
            DropStoredProcedure("dbo.JobJob_Insert");
            DropForeignKey("dbo.JobJobs", "Job_Id1", "dbo.Jobs");
            DropForeignKey("dbo.JobJobs", "Job_Id", "dbo.Jobs");
            DropIndex("dbo.JobJobs", new[] { "Job_Id1" });
            DropIndex("dbo.JobJobs", new[] { "Job_Id" });
            DropTable("dbo.JobJobs");
        }
    }
}
