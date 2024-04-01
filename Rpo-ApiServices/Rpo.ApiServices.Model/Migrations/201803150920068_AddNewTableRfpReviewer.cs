namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewTableRfpReviewer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RfpReviewers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdRfp = c.Int(nullable: false),
                        IdReviewer = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.IdReviewer)
                .ForeignKey("dbo.Rfps", t => t.IdRfp)
                .Index(t => t.IdRfp)
                .Index(t => t.IdReviewer);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RfpReviewers", "IdRfp", "dbo.Rfps");
            DropForeignKey("dbo.RfpReviewers", "IdReviewer", "dbo.Employees");
            DropIndex("dbo.RfpReviewers", new[] { "IdReviewer" });
            DropIndex("dbo.RfpReviewers", new[] { "IdRfp" });
            DropTable("dbo.RfpReviewers");
        }
    }
}
