namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobStatusNotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "JobStatusNotes", c => c.String());
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
                        ProjectDescription = p.String(),
                        QBJobName = p.String(),
                        IdReferredByCompany = p.Int(),
                        IdReferredByContact = p.Int(),
                        JobStatusNotes = p.String(),
                    },
                body:
                    @"INSERT [dbo].[Jobs]([JobNumber], [IdRfpAddress], [IdRfp], [IdBorough], [HouseNumber], [StreetNumber], [FloorNumber], [Apartment], [SpecialPlace], [Block], [Lot], [DOTProjectTeam], [DOBProjectTeam], [ViolationProjectTeam], [DEPProjectTeam], [HasLandMarkStatus], [HasEnvironmentalRestriction], [HasOpenWork], [IdCompany], [IdJobContactType], [IdContact], [IdProjectManager], [StartDate], [EndDate], [LastModiefiedDate], [Status], [ScopeGeneralNotes], [HasHolidayEmbargo], [CreatedBy], [CreatedDate], [LastModifiedBy], [PONumber], [OCMCNumber], [StreetWorkingOn], [StreetWorkingFrom], [StreetWorkingTo], [ProjectDescription], [QBJobName], [IdReferredByCompany], [IdReferredByContact], [JobStatusNotes])
                      VALUES (@JobNumber, @IdRfpAddress, @IdRfp, @IdBorough, @HouseNumber, @StreetNumber, @FloorNumber, @Apartment, @SpecialPlace, @Block, @Lot, @DOTProjectTeam, @DOBProjectTeam, @ViolationProjectTeam, @DEPProjectTeam, @HasLandMarkStatus, @HasEnvironmentalRestriction, @HasOpenWork, @IdCompany, @IdJobContactType, @IdContact, @IdProjectManager, @StartDate, @EndDate, @LastModiefiedDate, @Status, @ScopeGeneralNotes, @HasHolidayEmbargo, @CreatedBy, @CreatedDate, @LastModifiedBy, @PONumber, @OCMCNumber, @StreetWorkingOn, @StreetWorkingFrom, @StreetWorkingTo, @ProjectDescription, @QBJobName, @IdReferredByCompany, @IdReferredByContact, @JobStatusNotes)
                      
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
                        ProjectDescription = p.String(),
                        QBJobName = p.String(),
                        IdReferredByCompany = p.Int(),
                        IdReferredByContact = p.Int(),
                        JobStatusNotes = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[Jobs]
                      SET [JobNumber] = @JobNumber, [IdRfpAddress] = @IdRfpAddress, [IdRfp] = @IdRfp, [IdBorough] = @IdBorough, [HouseNumber] = @HouseNumber, [StreetNumber] = @StreetNumber, [FloorNumber] = @FloorNumber, [Apartment] = @Apartment, [SpecialPlace] = @SpecialPlace, [Block] = @Block, [Lot] = @Lot, [DOTProjectTeam] = @DOTProjectTeam, [DOBProjectTeam] = @DOBProjectTeam, [ViolationProjectTeam] = @ViolationProjectTeam, [DEPProjectTeam] = @DEPProjectTeam, [HasLandMarkStatus] = @HasLandMarkStatus, [HasEnvironmentalRestriction] = @HasEnvironmentalRestriction, [HasOpenWork] = @HasOpenWork, [IdCompany] = @IdCompany, [IdJobContactType] = @IdJobContactType, [IdContact] = @IdContact, [IdProjectManager] = @IdProjectManager, [StartDate] = @StartDate, [EndDate] = @EndDate, [LastModiefiedDate] = @LastModiefiedDate, [Status] = @Status, [ScopeGeneralNotes] = @ScopeGeneralNotes, [HasHolidayEmbargo] = @HasHolidayEmbargo, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [PONumber] = @PONumber, [OCMCNumber] = @OCMCNumber, [StreetWorkingOn] = @StreetWorkingOn, [StreetWorkingFrom] = @StreetWorkingFrom, [StreetWorkingTo] = @StreetWorkingTo, [ProjectDescription] = @ProjectDescription, [QBJobName] = @QBJobName, [IdReferredByCompany] = @IdReferredByCompany, [IdReferredByContact] = @IdReferredByContact, [JobStatusNotes] = @JobStatusNotes
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "JobStatusNotes");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
