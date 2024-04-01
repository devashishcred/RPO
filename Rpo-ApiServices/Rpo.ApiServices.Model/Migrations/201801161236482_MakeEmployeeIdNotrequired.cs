namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeEmployeeIdNotrequired : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.JobHistories", new[] { "IdEmployee" });
            AlterColumn("dbo.JobHistories", "IdEmployee", c => c.Int());
            CreateIndex("dbo.JobHistories", "IdEmployee");
        }
        
        public override void Down()
        {
            DropIndex("dbo.JobHistories", new[] { "IdEmployee" });
            AlterColumn("dbo.JobHistories", "IdEmployee", c => c.Int(nullable: false));
            CreateIndex("dbo.JobHistories", "IdEmployee");
        }
    }
}
