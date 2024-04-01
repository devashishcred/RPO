namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeIdCompanyNullable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.JobContacts", new[] { "IdCompany" });
            DropIndex("dbo.JobContacts", new[] { "IdContact" });
            AlterColumn("dbo.JobContacts", "IdCompany", c => c.Int());
            AlterColumn("dbo.JobContacts", "IdContact", c => c.Int());
            CreateIndex("dbo.JobContacts", "IdCompany");
            CreateIndex("dbo.JobContacts", "IdContact");
        }
        
        public override void Down()
        {
            DropIndex("dbo.JobContacts", new[] { "IdContact" });
            DropIndex("dbo.JobContacts", new[] { "IdCompany" });
            AlterColumn("dbo.JobContacts", "IdContact", c => c.Int(nullable: false));
            AlterColumn("dbo.JobContacts", "IdCompany", c => c.Int(nullable: false));
            CreateIndex("dbo.JobContacts", "IdContact");
            CreateIndex("dbo.JobContacts", "IdCompany");
        }
    }
}
