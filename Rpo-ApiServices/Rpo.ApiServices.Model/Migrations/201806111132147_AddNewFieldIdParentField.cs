namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewFieldIdParentField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentFields", "IdParentField", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocumentFields", "IdParentField");
        }
    }
}
