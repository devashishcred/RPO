namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddParent_jobfeeschedule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobFeeSchedules", "IdParentof", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobFeeSchedules", "IdParentof");
        }
    }
}
