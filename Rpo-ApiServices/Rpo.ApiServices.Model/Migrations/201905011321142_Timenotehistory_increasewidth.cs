namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Timenotehistory_increasewidth : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JobTimeNoteHistories", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JobTimeNoteHistories", "Description", c => c.String(maxLength: 400));
        }
    }
}
