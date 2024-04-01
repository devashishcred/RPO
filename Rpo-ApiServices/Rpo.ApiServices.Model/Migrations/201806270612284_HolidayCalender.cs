namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HolidayCalender : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HolidayCalenders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        HolidayDate = c.DateTime(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HolidayCalenders", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.HolidayCalenders", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.HolidayCalenders", new[] { "LastModifiedBy" });
            DropIndex("dbo.HolidayCalenders", new[] { "CreatedBy" });
            DropTable("dbo.HolidayCalenders");
        }
    }
}
