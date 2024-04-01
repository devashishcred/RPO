namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewFieldsInJobContactType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DocumentTypes", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.JobContactTypes", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.TaskTypes", "Name", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.DocumentTypes", "Name", unique: true, name: "IX_DocumentTypeName");
            CreateIndex("dbo.JobContactTypes", "Name", unique: true, name: "IX_JobContactTypeName");
            CreateIndex("dbo.TaskTypes", "Name", unique: true, name: "IX_TaskTypeName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TaskTypes", "IX_TaskTypeName");
            DropIndex("dbo.JobContactTypes", "IX_JobContactTypeName");
            DropIndex("dbo.DocumentTypes", "IX_DocumentTypeName");
            AlterColumn("dbo.TaskTypes", "Name", c => c.String());
            AlterColumn("dbo.JobContactTypes", "Name", c => c.String());
            AlterColumn("dbo.DocumentTypes", "Name", c => c.String(maxLength: 50));
        }
    }
}
