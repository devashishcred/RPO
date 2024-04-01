namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeInWorkPermitHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobWorkPermitHistories", "NewNumber", c => c.String());
            AddColumn("dbo.JobWorkPermitHistories", "OldNumber", c => c.String());
            DropColumn("dbo.JobWorkPermitHistories", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobWorkPermitHistories", "Description", c => c.String());
            DropColumn("dbo.JobWorkPermitHistories", "OldNumber");
            DropColumn("dbo.JobWorkPermitHistories", "NewNumber");
        }
    }
}
