namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateEmailLogTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskEmailReminderLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FromEmail = c.String(maxLength: 500),
                        FromName = c.String(maxLength: 100),
                        ToEmail = c.String(maxLength: 1000),
                        CcEmail = c.String(maxLength: 1000),
                        BccEmail = c.String(maxLength: 1000),
                        EmailBody = c.String(),
                        IdTask = c.Int(nullable: false),
                        IdEmployee = c.Int(nullable: false),
                        EmailSubject = c.String(maxLength: 200),
                        IsMailSent = c.Boolean(nullable: false),
                        ToName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TaskEmailReminderLogs");
        }
    }
}
