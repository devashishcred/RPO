namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobMilestoneNewColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobFeeSchedules", "InvoicedDate", c => c.DateTime());
            AddColumn("dbo.JobMilestones", "IsInvoiced", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobMilestones", "InvoicedDate", c => c.DateTime());
            AddColumn("dbo.JobMilestones", "InvoiceNumber", c => c.String());
            AddColumn("dbo.JobMilestones", "PONumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobMilestones", "PONumber");
            DropColumn("dbo.JobMilestones", "InvoiceNumber");
            DropColumn("dbo.JobMilestones", "InvoicedDate");
            DropColumn("dbo.JobMilestones", "IsInvoiced");
            DropColumn("dbo.JobFeeSchedules", "InvoicedDate");
        }
    }
}
