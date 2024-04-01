namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Createtable_DOBPermit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DOBPermitMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJob = c.Int(nullable: false),
                        IdJobApplication = c.Int(nullable: false),
                        IdWorkPermit = c.Int(),
                        Seq = c.String(),
                        Permit = c.String(),
                        NumberDocType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DOBPermitMappings");
        }
    }
}
