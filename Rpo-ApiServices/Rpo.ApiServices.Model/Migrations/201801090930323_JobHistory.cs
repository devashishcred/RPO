namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 200),
                        IdEmployee = c.Int(nullable: false),
                        HistoryDate = c.DateTime(nullable: false),
                        JobHistoryType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.IdEmployee)
                .Index(t => t.IdEmployee);
            
            AddColumn("dbo.JobWorkTypes", "Code", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobHistories", "IdEmployee", "dbo.Employees");
            DropIndex("dbo.JobHistories", new[] { "IdEmployee" });
            DropColumn("dbo.JobWorkTypes", "Code");
            DropTable("dbo.JobHistories");
        }
    }
}
