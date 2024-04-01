namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdJobApplicationTypeCompalsoryInApplicationStatus : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ApplicationStatus", new[] { "IdJobApplicationType" });
            AlterColumn("dbo.ApplicationStatus", "IdJobApplicationType", c => c.Int(nullable: false));
            CreateIndex("dbo.ApplicationStatus", "IdJobApplicationType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ApplicationStatus", new[] { "IdJobApplicationType" });
            AlterColumn("dbo.ApplicationStatus", "IdJobApplicationType", c => c.Int());
            CreateIndex("dbo.ApplicationStatus", "IdJobApplicationType");
        }
    }
}
