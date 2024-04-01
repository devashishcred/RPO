namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeinjobHistoryDiscriptionFiledMaxLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JobHistories", "Description", c => c.String(maxLength: 400));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JobHistories", "Description", c => c.String(maxLength: 200));
        }
    }
}
