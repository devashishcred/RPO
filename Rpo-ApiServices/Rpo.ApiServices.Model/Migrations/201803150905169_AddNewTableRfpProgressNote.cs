namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewTableRfpProgressNote : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RfpProgressNotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdRfp = c.Int(nullable: false),
                        Notes = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .ForeignKey("dbo.Rfps", t => t.IdRfp)
                .Index(t => t.IdRfp)
                .Index(t => t.LastModifiedBy)
                .Index(t => t.CreatedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RfpProgressNotes", "IdRfp", "dbo.Rfps");
            DropForeignKey("dbo.RfpProgressNotes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpProgressNotes", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.RfpProgressNotes", new[] { "CreatedBy" });
            DropIndex("dbo.RfpProgressNotes", new[] { "LastModifiedBy" });
            DropIndex("dbo.RfpProgressNotes", new[] { "IdRfp" });
            DropTable("dbo.RfpProgressNotes");
        }
    }
}
