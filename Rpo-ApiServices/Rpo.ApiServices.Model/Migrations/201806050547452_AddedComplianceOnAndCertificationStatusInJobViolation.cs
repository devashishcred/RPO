namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedComplianceOnAndCertificationStatusInJobViolation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobViolations", "IdViolationPaneltyCode", "dbo.ViolationPaneltyCodes");
            DropIndex("dbo.JobViolations", new[] { "IdViolationPaneltyCode" });
            AddColumn("dbo.JobViolations", "ComplianceOn", c => c.DateTime());
            AddColumn("dbo.JobViolations", "CertificationStatus", c => c.String());
            DropColumn("dbo.JobViolations", "IdViolationPaneltyCode");
            DropColumn("dbo.JobViolations", "Description");
            DropColumn("dbo.JobViolations", "PaneltyCodeSection");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobViolations", "PaneltyCodeSection", c => c.String());
            AddColumn("dbo.JobViolations", "Description", c => c.String());
            AddColumn("dbo.JobViolations", "IdViolationPaneltyCode", c => c.Int());
            DropColumn("dbo.JobViolations", "CertificationStatus");
            DropColumn("dbo.JobViolations", "ComplianceOn");
            CreateIndex("dbo.JobViolations", "IdViolationPaneltyCode");
            AddForeignKey("dbo.JobViolations", "IdViolationPaneltyCode", "dbo.ViolationPaneltyCodes", "Id");
        }
    }
}
