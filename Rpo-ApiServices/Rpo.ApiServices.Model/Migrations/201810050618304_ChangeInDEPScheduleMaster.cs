namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeInDEPScheduleMaster : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DEPNoiseCodeOffenses", "IdDEPNoiseCodePenaltySchedule", "dbo.DEPNoiseCodePenaltySchedules");
            DropIndex("dbo.DEPNoiseCodeOffenses", new[] { "IdDEPNoiseCodePenaltySchedule" });
            AddColumn("dbo.DEPNoiseCodePenaltySchedules", "Offense_1", c => c.String());
            AddColumn("dbo.DEPNoiseCodePenaltySchedules", "Penalty_1", c => c.Double());
            AddColumn("dbo.DEPNoiseCodePenaltySchedules", "DefaultPenalty_1", c => c.Double());
            AddColumn("dbo.DEPNoiseCodePenaltySchedules", "Stipulation_1", c => c.Boolean(nullable: false));
            AddColumn("dbo.DEPNoiseCodePenaltySchedules", "Offense_2", c => c.String());
            AddColumn("dbo.DEPNoiseCodePenaltySchedules", "Penalty_2", c => c.Double());
            AddColumn("dbo.DEPNoiseCodePenaltySchedules", "DefaultPenalty_2", c => c.Double());
            AddColumn("dbo.DEPNoiseCodePenaltySchedules", "Stipulation_2", c => c.Boolean(nullable: false));
            AddColumn("dbo.DEPNoiseCodePenaltySchedules", "Offense_3", c => c.String());
            AddColumn("dbo.DEPNoiseCodePenaltySchedules", "Penalty_3", c => c.Double());
            AddColumn("dbo.DEPNoiseCodePenaltySchedules", "DefaultPenalty_3", c => c.Double());
            AddColumn("dbo.DEPNoiseCodePenaltySchedules", "Stipulation_3", c => c.Boolean(nullable: false));
            AddColumn("dbo.DEPNoiseCodePenaltySchedules", "Offense_4", c => c.String());
            AddColumn("dbo.DEPNoiseCodePenaltySchedules", "Penalty_4", c => c.Double());
            AddColumn("dbo.DEPNoiseCodePenaltySchedules", "DefaultPenalty_4", c => c.Double());
            AddColumn("dbo.DEPNoiseCodePenaltySchedules", "Stipulation_4", c => c.Boolean(nullable: false));
            DropTable("dbo.DEPNoiseCodeOffenses");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.DEPNoiseCodePenaltySchedules", "Stipulation_4");
            DropColumn("dbo.DEPNoiseCodePenaltySchedules", "DefaultPenalty_4");
            DropColumn("dbo.DEPNoiseCodePenaltySchedules", "Penalty_4");
            DropColumn("dbo.DEPNoiseCodePenaltySchedules", "Offense_4");
            DropColumn("dbo.DEPNoiseCodePenaltySchedules", "Stipulation_3");
            DropColumn("dbo.DEPNoiseCodePenaltySchedules", "DefaultPenalty_3");
            DropColumn("dbo.DEPNoiseCodePenaltySchedules", "Penalty_3");
            DropColumn("dbo.DEPNoiseCodePenaltySchedules", "Offense_3");
            DropColumn("dbo.DEPNoiseCodePenaltySchedules", "Stipulation_2");
            DropColumn("dbo.DEPNoiseCodePenaltySchedules", "DefaultPenalty_2");
            DropColumn("dbo.DEPNoiseCodePenaltySchedules", "Penalty_2");
            DropColumn("dbo.DEPNoiseCodePenaltySchedules", "Offense_2");
            DropColumn("dbo.DEPNoiseCodePenaltySchedules", "Stipulation_1");
            DropColumn("dbo.DEPNoiseCodePenaltySchedules", "DefaultPenalty_1");
            DropColumn("dbo.DEPNoiseCodePenaltySchedules", "Penalty_1");
            DropColumn("dbo.DEPNoiseCodePenaltySchedules", "Offense_1");
            CreateIndex("dbo.DEPNoiseCodeOffenses", "IdDEPNoiseCodePenaltySchedule");
            AddForeignKey("dbo.DEPNoiseCodeOffenses", "IdDEPNoiseCodePenaltySchedule", "dbo.DEPNoiseCodePenaltySchedules", "Id");
        }
    }
}
