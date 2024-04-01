namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubjectInTheRfpEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RFPEmailHistories", "EmailSubject", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RFPEmailHistories", "EmailSubject");
        }
    }
}
