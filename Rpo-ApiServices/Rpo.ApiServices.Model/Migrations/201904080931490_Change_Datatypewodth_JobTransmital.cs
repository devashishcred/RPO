namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Datatypewodth_JobTransmital : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JobTransmittals", "EmailSubject", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JobTransmittals", "EmailSubject", c => c.String(maxLength: 100));
        }
    }
}
