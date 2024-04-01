namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SolveTheJobApplicationTypeError : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobWorkTypes", "JobApplicationType_Id", c => c.Int(nullable: true));
            DropTable("dbo.JobApplicationTypeJobWorkTypes");
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobWorkTypes", "JobApplicationType_Id");
        }
    }
}
