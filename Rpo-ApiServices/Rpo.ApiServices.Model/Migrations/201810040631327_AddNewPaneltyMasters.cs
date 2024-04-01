namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewPaneltyMasters : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DEPNoiseCodeOffenses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdDEPNoiseCodePenaltySchedule = c.Int(nullable: false),
                        Offense = c.String(),
                        Penalty = c.Double(),
                        DefaultPenalty = c.Double(),
                        Stipulation = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DEPNoiseCodePenaltySchedules", t => t.IdDEPNoiseCodePenaltySchedule)
                .Index(t => t.IdDEPNoiseCodePenaltySchedule);
            
            CreateTable(
                "dbo.DEPNoiseCodePenaltySchedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SectionOfLaw = c.String(),
                        ViolationDescription = c.String(),
                        Compliance = c.String(),
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
            
            CreateTable(
                "dbo.DOHMHCoolingTowerPenaltySchedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SectionOfLaw = c.String(),
                        Description = c.String(),
                        PenaltyFirstViolation = c.Double(),
                        PenaltyRepeatViolation = c.Double(),
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
            
            CreateTable(
                "dbo.DOTPenaltySchedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Section = c.String(),
                        Description = c.String(),
                        Penalty = c.Double(),
                        DefaultPenalty = c.Double(),
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
            
            CreateTable(
                "dbo.FDNYPenaltySchedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Category_RCNY = c.String(),
                        DescriptionOfViolation = c.String(),
                        OATHViolationCode = c.String(),
                        FirstViolationPenalty = c.Double(),
                        FirstViolationMitigatedPenalty = c.Double(),
                        FirstViolationMaximumPenalty = c.Double(),
                        SecondSubsequentViolationPenalty = c.Double(),
                        SecondSubsequentViolationMitigatedPenalty = c.Double(),
                        SecondSubsequentViolationMaximumPenalty = c.Double(),
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
            DropForeignKey("dbo.FDNYPenaltySchedules", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.FDNYPenaltySchedules", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.DOTPenaltySchedules", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.DOTPenaltySchedules", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.DOHMHCoolingTowerPenaltySchedules", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.DOHMHCoolingTowerPenaltySchedules", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.DEPNoiseCodePenaltySchedules", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.DEPNoiseCodeOffenses", "IdDEPNoiseCodePenaltySchedule", "dbo.DEPNoiseCodePenaltySchedules");
            DropForeignKey("dbo.DEPNoiseCodePenaltySchedules", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.FDNYPenaltySchedules", new[] { "LastModifiedBy" });
            DropIndex("dbo.FDNYPenaltySchedules", new[] { "CreatedBy" });
            DropIndex("dbo.DOTPenaltySchedules", new[] { "LastModifiedBy" });
            DropIndex("dbo.DOTPenaltySchedules", new[] { "CreatedBy" });
            DropIndex("dbo.DOHMHCoolingTowerPenaltySchedules", new[] { "LastModifiedBy" });
            DropIndex("dbo.DOHMHCoolingTowerPenaltySchedules", new[] { "CreatedBy" });
            DropIndex("dbo.DEPNoiseCodePenaltySchedules", new[] { "LastModifiedBy" });
            DropIndex("dbo.DEPNoiseCodePenaltySchedules", new[] { "CreatedBy" });
            DropIndex("dbo.DEPNoiseCodeOffenses", new[] { "IdDEPNoiseCodePenaltySchedule" });
            DropTable("dbo.FDNYPenaltySchedules");
            DropTable("dbo.DOTPenaltySchedules");
            DropTable("dbo.DOHMHCoolingTowerPenaltySchedules");
            DropTable("dbo.DEPNoiseCodePenaltySchedules");
            DropTable("dbo.DEPNoiseCodeOffenses");
        }
    }
}
