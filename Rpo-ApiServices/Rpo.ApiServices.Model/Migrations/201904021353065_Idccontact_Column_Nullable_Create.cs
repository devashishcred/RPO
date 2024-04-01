namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Idccontact_Column_Nullable_Create : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Jobs", new[] { "IdContact" });
            AlterColumn("dbo.Jobs", "IdContact", c => c.Int());
            CreateIndex("dbo.Jobs", "IdContact");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Jobs", new[] { "IdContact" });
            AlterColumn("dbo.Jobs", "IdContact", c => c.Int(nullable: false));
            CreateIndex("dbo.Jobs", "IdContact");
        }
    }
}
