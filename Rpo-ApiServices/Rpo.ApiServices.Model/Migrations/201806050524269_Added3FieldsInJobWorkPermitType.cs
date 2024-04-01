namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added3FieldsInJobWorkPermitType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplicationWorkPermitTypes", "PermitType", c => c.String());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "ForPurposeOf", c => c.String());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "EquipmentType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobApplicationWorkPermitTypes", "EquipmentType");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "ForPurposeOf");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "PermitType");
        }
    }
}
