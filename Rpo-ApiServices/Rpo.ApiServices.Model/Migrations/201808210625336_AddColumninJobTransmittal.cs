namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumninJobTransmittal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobTransmittals", "IsDraft", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobTransmittals", "IsDraft");
        }
    }
}
