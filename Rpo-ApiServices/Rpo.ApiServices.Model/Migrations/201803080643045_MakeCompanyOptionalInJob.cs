namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeCompanyOptionalInJob : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Jobs", new[] { "IdCompany" });
            AlterColumn("dbo.Jobs", "IdCompany", c => c.Int());
            CreateIndex("dbo.Jobs", "IdCompany");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Jobs", new[] { "IdCompany" });
            AlterColumn("dbo.Jobs", "IdCompany", c => c.Int(nullable: false));
            CreateIndex("dbo.Jobs", "IdCompany");
        }
    }
}
