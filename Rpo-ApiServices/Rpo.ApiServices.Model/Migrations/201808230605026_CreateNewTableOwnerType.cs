namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateNewTableOwnerType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OwnerTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.Name, unique: true, name: "IX_OwnerTypeName")
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OwnerTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.OwnerTypes", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.OwnerTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.OwnerTypes", new[] { "CreatedBy" });
            DropIndex("dbo.OwnerTypes", "IX_OwnerTypeName");
            DropTable("dbo.OwnerTypes");
        }
    }
}
