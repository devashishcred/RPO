namespace Rpo.ApiServices.Model.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ProjectTeamMemberOptional : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Jobs", new[] { "IdProjectCoordinator" });
            DropIndex("dbo.Jobs", new[] { "IdSignoffCoordinator" });
            AlterColumn("dbo.Jobs", "IdProjectCoordinator", c => c.Int());
            AlterColumn("dbo.Jobs", "IdSignoffCoordinator", c => c.Int());
            CreateIndex("dbo.Jobs", "IdProjectCoordinator");
            CreateIndex("dbo.Jobs", "IdSignoffCoordinator");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Jobs", new[] { "IdSignoffCoordinator" });
            DropIndex("dbo.Jobs", new[] { "IdProjectCoordinator" });
            AlterColumn("dbo.Jobs", "IdSignoffCoordinator", c => c.Int(nullable: false));
            AlterColumn("dbo.Jobs", "IdProjectCoordinator", c => c.Int(nullable: false));
            CreateIndex("dbo.Jobs", "IdSignoffCoordinator");
            CreateIndex("dbo.Jobs", "IdProjectCoordinator");
        }
    }
}
