namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDOBProjectTeamInJob : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Jobs", "IdProjectCoordinator", "dbo.Employees");
            DropForeignKey("dbo.Jobs", "IdSignoffCoordinator", "dbo.Employees");
            DropIndex("dbo.Jobs", new[] { "IdProjectCoordinator" });
            DropIndex("dbo.Jobs", new[] { "IdSignoffCoordinator" });
            AddColumn("dbo.Jobs", "DOBProjectTeam", c => c.String());
            DropColumn("dbo.Jobs", "IdProjectCoordinator");
            DropColumn("dbo.Jobs", "IdSignoffCoordinator");
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
                        DOBProjectTeam = p.String(),
                        ViolationProjectTeam = p.String(),
                        DEPProjectTeam = p.String(),
                        HasLandMarkStatus = p.Boolean(),
                        HasEnvironmentalRestriction = p.Boolean(),
                        HasOpenWork = p.Boolean(),
                        IdCompany = p.Int(),
                        IdJobContactType = p.Int(),
                        IdContact = p.Int(),
                        IdProjectManager = p.Int(),
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
                        OCMCNumber = p.String(),
                        StreetWorkingOn = p.String(),
                        StreetWorkingFrom = p.String(),
                        StreetWorkingTo = p.String(),
                    },
                body:
                    @"INSERT [dbo].[Jobs]([JobNumber], [IdRfpAddress], [IdRfp], [IdBorough], [HouseNumber], [StreetNumber], [FloorNumber], [Apartment], [SpecialPlace], [Block], [Lot], [DOTProjectTeam], [DOBProjectTeam], [ViolationProjectTeam], [DEPProjectTeam], [HasLandMarkStatus], [HasEnvironmentalRestriction], [HasOpenWork], [IdCompany], [IdJobContactType], [IdContact], [IdProjectManager], [StartDate], [EndDate], [LastModiefiedDate], [Status], [ScopeGeneralNotes], [HasHolidayEmbargo], [CreatedBy], [CreatedDate], [LastModifiedBy], [PONumber], [OCMCNumber], [StreetWorkingOn], [StreetWorkingFrom], [StreetWorkingTo])
                      VALUES (@JobNumber, @IdRfpAddress, @IdRfp, @IdBorough, @HouseNumber, @StreetNumber, @FloorNumber, @Apartment, @SpecialPlace, @Block, @Lot, @DOTProjectTeam, @DOBProjectTeam, @ViolationProjectTeam, @DEPProjectTeam, @HasLandMarkStatus, @HasEnvironmentalRestriction, @HasOpenWork, @IdCompany, @IdJobContactType, @IdContact, @IdProjectManager, @StartDate, @EndDate, @LastModiefiedDate, @Status, @ScopeGeneralNotes, @HasHolidayEmbargo, @CreatedBy, @CreatedDate, @LastModifiedBy, @PONumber, @OCMCNumber, @StreetWorkingOn, @StreetWorkingFrom, @StreetWorkingTo)
                      
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
                        DOBProjectTeam = p.String(),
                        ViolationProjectTeam = p.String(),
                        DEPProjectTeam = p.String(),
                        HasLandMarkStatus = p.Boolean(),
                        HasEnvironmentalRestriction = p.Boolean(),
                        HasOpenWork = p.Boolean(),
                        IdCompany = p.Int(),
                        IdJobContactType = p.Int(),
                        IdContact = p.Int(),
                        IdProjectManager = p.Int(),
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
                        OCMCNumber = p.String(),
                        StreetWorkingOn = p.String(),
                        StreetWorkingFrom = p.String(),
                        StreetWorkingTo = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[Jobs]
                      SET [JobNumber] = @JobNumber, [IdRfpAddress] = @IdRfpAddress, [IdRfp] = @IdRfp, [IdBorough] = @IdBorough, [HouseNumber] = @HouseNumber, [StreetNumber] = @StreetNumber, [FloorNumber] = @FloorNumber, [Apartment] = @Apartment, [SpecialPlace] = @SpecialPlace, [Block] = @Block, [Lot] = @Lot, [DOTProjectTeam] = @DOTProjectTeam, [DOBProjectTeam] = @DOBProjectTeam, [ViolationProjectTeam] = @ViolationProjectTeam, [DEPProjectTeam] = @DEPProjectTeam, [HasLandMarkStatus] = @HasLandMarkStatus, [HasEnvironmentalRestriction] = @HasEnvironmentalRestriction, [HasOpenWork] = @HasOpenWork, [IdCompany] = @IdCompany, [IdJobContactType] = @IdJobContactType, [IdContact] = @IdContact, [IdProjectManager] = @IdProjectManager, [StartDate] = @StartDate, [EndDate] = @EndDate, [LastModiefiedDate] = @LastModiefiedDate, [Status] = @Status, [ScopeGeneralNotes] = @ScopeGeneralNotes, [HasHolidayEmbargo] = @HasHolidayEmbargo, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [PONumber] = @PONumber, [OCMCNumber] = @OCMCNumber, [StreetWorkingOn] = @StreetWorkingOn, [StreetWorkingFrom] = @StreetWorkingFrom, [StreetWorkingTo] = @StreetWorkingTo
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Jobs", "IdSignoffCoordinator", c => c.Int());
            AddColumn("dbo.Jobs", "IdProjectCoordinator", c => c.Int());
            DropColumn("dbo.Jobs", "DOBProjectTeam");
            CreateIndex("dbo.Jobs", "IdSignoffCoordinator");
            CreateIndex("dbo.Jobs", "IdProjectCoordinator");
            AddForeignKey("dbo.Jobs", "IdSignoffCoordinator", "dbo.Employees", "Id");
            AddForeignKey("dbo.Jobs", "IdProjectCoordinator", "dbo.Employees", "Id");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
