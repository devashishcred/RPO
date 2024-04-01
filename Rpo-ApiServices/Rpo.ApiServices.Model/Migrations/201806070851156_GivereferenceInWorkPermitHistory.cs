namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GivereferenceInWorkPermitHistory : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.JobWorkPermitHistories", new[] { "IdWorkPermit" });
            DropIndex("dbo.JobWorkPermitHistories", new[] { "IdJobApplication" });
            AlterColumn("dbo.JobWorkPermitHistories", "IdWorkPermit", c => c.Int());
            AlterColumn("dbo.JobWorkPermitHistories", "IdJobApplication", c => c.Int());
            CreateIndex("dbo.JobWorkPermitHistories", "IdWorkPermit");
            CreateIndex("dbo.JobWorkPermitHistories", "IdJobApplication");
        }
        
        public override void Down()
        {
            DropIndex("dbo.JobWorkPermitHistories", new[] { "IdJobApplication" });
            DropIndex("dbo.JobWorkPermitHistories", new[] { "IdWorkPermit" });
            AlterColumn("dbo.JobWorkPermitHistories", "IdJobApplication", c => c.Int(nullable: false));
            AlterColumn("dbo.JobWorkPermitHistories", "IdWorkPermit", c => c.Int(nullable: false));
            CreateIndex("dbo.JobWorkPermitHistories", "IdJobApplication");
            CreateIndex("dbo.JobWorkPermitHistories", "IdWorkPermit");
        }
    }
}
