namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableSystemSetting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SystemSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.LastModifiedBy)
                .Index(t => t.CreatedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SystemSettings", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.SystemSettings", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.SystemSettings", new[] { "CreatedBy" });
            DropIndex("dbo.SystemSettings", new[] { "LastModifiedBy" });
            DropTable("dbo.SystemSettings");
        }
    }
}
