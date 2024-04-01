namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeIdJobApplicationTypeNotnull : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.JobWorkTypes", new[] { "IdJobApplicationType" });
            AlterColumn("dbo.JobWorkTypes", "IdJobApplicationType", c => c.Int(nullable: false));
            CreateIndex("dbo.JobWorkTypes", "IdJobApplicationType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.JobWorkTypes", new[] { "IdJobApplicationType" });
            AlterColumn("dbo.JobWorkTypes", "IdJobApplicationType", c => c.Int());
            CreateIndex("dbo.JobWorkTypes", "IdJobApplicationType");
        }
    }
}
