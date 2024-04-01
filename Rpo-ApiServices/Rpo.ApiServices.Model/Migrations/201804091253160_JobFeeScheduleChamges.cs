namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobFeeScheduleChamges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "PONumber", c => c.String());
            AddColumn("dbo.JobFeeSchedules", "IdRfp", c => c.Int());
            AddColumn("dbo.JobFeeSchedules", "QuantityAchieved", c => c.Double());
            AddColumn("dbo.JobFeeSchedules", "QuantityPending", c => c.Double());
            AddColumn("dbo.JobFeeSchedules", "PONumber", c => c.String());
            AddColumn("dbo.JobFeeSchedules", "Status", c => c.String());
            AddColumn("dbo.JobFeeSchedules", "IsInvoiced", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobFeeSchedules", "InvoiceNumber", c => c.String());
            AddColumn("dbo.JobMilestones", "Status", c => c.String());
            CreateIndex("dbo.JobFeeSchedules", "IdRfp");
            AddForeignKey("dbo.JobFeeSchedules", "IdRfp", "dbo.Rfps", "Id");
            DropColumn("dbo.JobFeeSchedules", "Cost");
            DropColumn("dbo.JobFeeSchedules", "TotalCost");
            AlterStoredProcedure(
                "dbo.Job_Insert",
                p => new
                    {
                        JobNumber = p.String(),
                        IdRfpAddress = p.Int(),
                        IdRfp = p.Int(),
                        IdBorough = p.Int(),
                        HouseNumber = p.String(maxLength: 50),
                        StreetNumber = p.String(maxLength: 50),
                        FloorNumber = p.String(maxLength: 50),
                        Apartment = p.String(maxLength: 50),
                        SpecialPlace = p.String(maxLength: 50),
                        Block = p.String(maxLength: 50),
                        Lot = p.String(maxLength: 50),
                        DOTProjectTeam = p.String(),
                        ViolationProjectTeam = p.String(),
                        DEPProjectTeam = p.String(),
                        HasLandMarkStatus = p.Boolean(),
                        HasEnvironmentalRestriction = p.Boolean(),
                        HasOpenWork = p.Boolean(),
                        IdCompany = p.Int(),
                        IdJobContactType = p.Int(),
                        IdContact = p.Int(),
                        IdProjectManager = p.Int(),
                        IdProjectCoordinator = p.Int(),
                        IdSignoffCoordinator = p.Int(),
                        StartDate = p.DateTime(),
                        EndDate = p.DateTime(),
                        LastModiefiedDate = p.DateTime(),
                        Status = p.Int(),
                        ScopeGeneralNotes = p.String(),
                        HasHolidayEmbargo = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        PONumber = p.String(),
                    },
                body:
                    @"INSERT [dbo].[Jobs]([JobNumber], [IdRfpAddress], [IdRfp], [IdBorough], [HouseNumber], [StreetNumber], [FloorNumber], [Apartment], [SpecialPlace], [Block], [Lot], [DOTProjectTeam], [ViolationProjectTeam], [DEPProjectTeam], [HasLandMarkStatus], [HasEnvironmentalRestriction], [HasOpenWork], [IdCompany], [IdJobContactType], [IdContact], [IdProjectManager], [IdProjectCoordinator], [IdSignoffCoordinator], [StartDate], [EndDate], [LastModiefiedDate], [Status], [ScopeGeneralNotes], [HasHolidayEmbargo], [CreatedBy], [CreatedDate], [LastModifiedBy], [PONumber])
                      VALUES (@JobNumber, @IdRfpAddress, @IdRfp, @IdBorough, @HouseNumber, @StreetNumber, @FloorNumber, @Apartment, @SpecialPlace, @Block, @Lot, @DOTProjectTeam, @ViolationProjectTeam, @DEPProjectTeam, @HasLandMarkStatus, @HasEnvironmentalRestriction, @HasOpenWork, @IdCompany, @IdJobContactType, @IdContact, @IdProjectManager, @IdProjectCoordinator, @IdSignoffCoordinator, @StartDate, @EndDate, @LastModiefiedDate, @Status, @ScopeGeneralNotes, @HasHolidayEmbargo, @CreatedBy, @CreatedDate, @LastModifiedBy, @PONumber)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Jobs]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Jobs] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Job_Update",
                p => new
                    {
                        Id = p.Int(),
                        JobNumber = p.String(),
                        IdRfpAddress = p.Int(),
                        IdRfp = p.Int(),
                        IdBorough = p.Int(),
                        HouseNumber = p.String(maxLength: 50),
                        StreetNumber = p.String(maxLength: 50),
                        FloorNumber = p.String(maxLength: 50),
                        Apartment = p.String(maxLength: 50),
                        SpecialPlace = p.String(maxLength: 50),
                        Block = p.String(maxLength: 50),
                        Lot = p.String(maxLength: 50),
                        DOTProjectTeam = p.String(),
                        ViolationProjectTeam = p.String(),
                        DEPProjectTeam = p.String(),
                        HasLandMarkStatus = p.Boolean(),
                        HasEnvironmentalRestriction = p.Boolean(),
                        HasOpenWork = p.Boolean(),
                        IdCompany = p.Int(),
                        IdJobContactType = p.Int(),
                        IdContact = p.Int(),
                        IdProjectManager = p.Int(),
                        IdProjectCoordinator = p.Int(),
                        IdSignoffCoordinator = p.Int(),
                        StartDate = p.DateTime(),
                        EndDate = p.DateTime(),
                        LastModiefiedDate = p.DateTime(),
                        Status = p.Int(),
                        ScopeGeneralNotes = p.String(),
                        HasHolidayEmbargo = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        PONumber = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[Jobs]
                      SET [JobNumber] = @JobNumber, [IdRfpAddress] = @IdRfpAddress, [IdRfp] = @IdRfp, [IdBorough] = @IdBorough, [HouseNumber] = @HouseNumber, [StreetNumber] = @StreetNumber, [FloorNumber] = @FloorNumber, [Apartment] = @Apartment, [SpecialPlace] = @SpecialPlace, [Block] = @Block, [Lot] = @Lot, [DOTProjectTeam] = @DOTProjectTeam, [ViolationProjectTeam] = @ViolationProjectTeam, [DEPProjectTeam] = @DEPProjectTeam, [HasLandMarkStatus] = @HasLandMarkStatus, [HasEnvironmentalRestriction] = @HasEnvironmentalRestriction, [HasOpenWork] = @HasOpenWork, [IdCompany] = @IdCompany, [IdJobContactType] = @IdJobContactType, [IdContact] = @IdContact, [IdProjectManager] = @IdProjectManager, [IdProjectCoordinator] = @IdProjectCoordinator, [IdSignoffCoordinator] = @IdSignoffCoordinator, [StartDate] = @StartDate, [EndDate] = @EndDate, [LastModiefiedDate] = @LastModiefiedDate, [Status] = @Status, [ScopeGeneralNotes] = @ScopeGeneralNotes, [HasHolidayEmbargo] = @HasHolidayEmbargo, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [PONumber] = @PONumber
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobFeeSchedules", "TotalCost", c => c.Double());
            AddColumn("dbo.JobFeeSchedules", "Cost", c => c.Double());
            DropForeignKey("dbo.JobFeeSchedules", "IdRfp", "dbo.Rfps");
            DropIndex("dbo.JobFeeSchedules", new[] { "IdRfp" });
            DropColumn("dbo.JobMilestones", "Status");
            DropColumn("dbo.JobFeeSchedules", "InvoiceNumber");
            DropColumn("dbo.JobFeeSchedules", "IsInvoiced");
            DropColumn("dbo.JobFeeSchedules", "Status");
            DropColumn("dbo.JobFeeSchedules", "PONumber");
            DropColumn("dbo.JobFeeSchedules", "QuantityPending");
            DropColumn("dbo.JobFeeSchedules", "QuantityAchieved");
            DropColumn("dbo.JobFeeSchedules", "IdRfp");
            DropColumn("dbo.Jobs", "PONumber");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
