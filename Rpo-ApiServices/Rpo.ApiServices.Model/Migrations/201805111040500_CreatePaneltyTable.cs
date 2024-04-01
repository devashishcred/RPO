namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePaneltyTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ViolationPaneltyCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaneltyCode = c.String(),
                        CodeSection = c.String(),
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
            
            AddColumn("dbo.JobViolations", "PaneltyAmount", c => c.Double());
            AddColumn("dbo.JobViolations", "IdViolationPaneltyCode", c => c.Int());
            AddColumn("dbo.JobViolations", "Description", c => c.String());
            AlterColumn("dbo.JobViolations", "BalanceDue", c => c.Double());
            CreateIndex("dbo.JobViolations", "IdViolationPaneltyCode");
            AddForeignKey("dbo.JobViolations", "IdViolationPaneltyCode", "dbo.ViolationPaneltyCodes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobViolations", "IdViolationPaneltyCode", "dbo.ViolationPaneltyCodes");
            DropForeignKey("dbo.ViolationPaneltyCodes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.ViolationPaneltyCodes", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.ViolationPaneltyCodes", new[] { "LastModifiedBy" });
            DropIndex("dbo.ViolationPaneltyCodes", new[] { "CreatedBy" });
            DropIndex("dbo.JobViolations", new[] { "IdViolationPaneltyCode" });
            AlterColumn("dbo.JobViolations", "BalanceDue", c => c.Double(nullable: false));
            DropColumn("dbo.JobViolations", "Description");
            DropColumn("dbo.JobViolations", "IdViolationPaneltyCode");
            DropColumn("dbo.JobViolations", "PaneltyAmount");
            DropTable("dbo.ViolationPaneltyCodes");
        }
    }
}
