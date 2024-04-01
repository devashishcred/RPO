namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableTaskReminders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskReminders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdTask = c.Int(nullable: false),
                        RemindmeIn = c.Int(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .ForeignKey("dbo.Tasks", t => t.IdTask)
                .Index(t => t.IdTask)
                .Index(t => t.LastModifiedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskReminders", "IdTask", "dbo.Tasks");
            DropForeignKey("dbo.TaskReminders", "LastModifiedBy", "dbo.Employees");
            DropIndex("dbo.TaskReminders", new[] { "LastModifiedBy" });
            DropIndex("dbo.TaskReminders", new[] { "IdTask" });
            DropTable("dbo.TaskReminders");
        }
    }
}
