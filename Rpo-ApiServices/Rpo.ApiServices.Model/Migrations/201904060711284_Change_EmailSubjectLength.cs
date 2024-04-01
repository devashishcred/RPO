namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_EmailSubjectLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RFPEmailHistories", "EmailSubject", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RFPEmailHistories", "EmailSubject", c => c.String(maxLength: 100));
        }
    }
}
