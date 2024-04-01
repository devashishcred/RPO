namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewTableJobContactGroup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobContactGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IdJob = c.Int(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Jobs", t => t.IdJob)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.IdJob)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobContactGroups", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobContactGroups", "IdJob", "dbo.Jobs");
            DropForeignKey("dbo.JobContactGroups", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.JobContactGroups", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobContactGroups", new[] { "CreatedBy" });
            DropIndex("dbo.JobContactGroups", new[] { "IdJob" });
            DropTable("dbo.JobContactGroups");
        }
    }
}
