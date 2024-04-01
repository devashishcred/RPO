namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Showcope_Partof_Added_jobtype : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RfpJobTypes", "IsShowScope", c => c.Boolean(nullable: false));
            AddColumn("dbo.RfpJobTypes", "PartOf", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RfpJobTypes", "PartOf");
            DropColumn("dbo.RfpJobTypes", "IsShowScope");
        }
    }
}
