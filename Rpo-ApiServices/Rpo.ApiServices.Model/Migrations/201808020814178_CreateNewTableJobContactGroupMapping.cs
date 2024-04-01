namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateNewTableJobContactGroupMapping : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobContactJobContactGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJobContact = c.Int(nullable: false),
                        IdJobContactGroup = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobContacts", t => t.IdJobContact)
                .ForeignKey("dbo.JobContactGroups", t => t.IdJobContactGroup)
                .Index(t => t.IdJobContact)
                .Index(t => t.IdJobContactGroup);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobContactJobContactGroups", "IdJobContactGroup", "dbo.JobContactGroups");
            DropForeignKey("dbo.JobContactJobContactGroups", "IdJobContact", "dbo.JobContacts");
            DropIndex("dbo.JobContactJobContactGroups", new[] { "IdJobContactGroup" });
            DropIndex("dbo.JobContactJobContactGroups", new[] { "IdJobContact" });
            DropTable("dbo.JobContactJobContactGroups");
        }
    }
}
