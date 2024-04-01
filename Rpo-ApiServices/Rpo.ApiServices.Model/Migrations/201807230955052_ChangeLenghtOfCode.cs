namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeLenghtOfCode : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JobWorkTypes", "Code", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JobWorkTypes", "Code", c => c.String(maxLength: 10));
        }
    }
}
