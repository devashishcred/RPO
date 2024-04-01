namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaneltyScheduleMasters : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DOBPenaltySchedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SectionOfLaw = c.String(),
                        Classification = c.String(),
                        InfractionCode = c.String(),
                        ViolationDescription = c.String(),
                        Cure = c.Boolean(nullable: false),
                        Stipulation = c.Boolean(nullable: false),
                        StandardPenalty = c.Double(),
                        MitigatedPenalty = c.Boolean(nullable: false),
                        DefaultPenalty = c.Double(),
                        AggravatedPenalty_I = c.Double(),
                        AggravatedDefaultPenalty_I = c.Double(),
                        AggravatedPenalty_II = c.Double(),
                        AggravatedDefaultMaxPenalty_II = c.Double(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DOBPenaltySchedules", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.DOBPenaltySchedules", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.DOBPenaltySchedules", new[] { "LastModifiedBy" });
            DropIndex("dbo.DOBPenaltySchedules", new[] { "CreatedBy" });
            DropTable("dbo.DOBPenaltySchedules");
        }
    }
}
