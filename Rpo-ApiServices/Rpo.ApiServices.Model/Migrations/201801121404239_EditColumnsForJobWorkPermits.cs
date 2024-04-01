namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditColumnsForJobWorkPermits : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplicationWorkPermitTypes", "IsPersonResponsible", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobApplicationWorkPermitTypes", "IdContactResponsible", c => c.Int(nullable: false));
            AddColumn("dbo.JobApplicationWorkPermitTypes", "IdResponsibility", c => c.Int(nullable: false));
            CreateIndex("dbo.JobApplicationWorkPermitTypes", "IdContactResponsible");
            AddForeignKey("dbo.JobApplicationWorkPermitTypes", "IdContactResponsible", "dbo.Contacts", "Id");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "CompanyResponsible");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "PersonalResponsible");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobApplicationWorkPermitTypes", "PersonalResponsible", c => c.String());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "CompanyResponsible", c => c.String());
            DropForeignKey("dbo.JobApplicationWorkPermitTypes", "IdContactResponsible", "dbo.Contacts");
            DropIndex("dbo.JobApplicationWorkPermitTypes", new[] { "IdContactResponsible" });
            DropColumn("dbo.JobApplicationWorkPermitTypes", "IdResponsibility");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "IdContactResponsible");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "IsPersonResponsible");
        }
    }
}
