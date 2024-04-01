namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateMasterforJobStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RfpStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Jobs", "IdJobStatus", c => c.Int());
            AddColumn("dbo.Rfps", "IdRfpStatus", c => c.Int());
            CreateIndex("dbo.Jobs", "IdJobStatus");
            CreateIndex("dbo.Rfps", "IdRfpStatus");
            AddForeignKey("dbo.Jobs", "IdJobStatus", "dbo.JobStatus", "Id");
            AddForeignKey("dbo.Rfps", "IdRfpStatus", "dbo.RfpStatus", "Id");
            DropColumn("dbo.Jobs", "Status");
            DropColumn("dbo.Rfps", "Status");
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
                        IdJobStatus = p.Int(),
                        ScopeGeneralNotes = p.String(),
                        HasHolidayEmbargo = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Jobs]([JobNumber], [IdRfpAddress], [IdRfp], [IdBorough], [HouseNumber], [StreetNumber], [FloorNumber], [Apartment], [SpecialPlace], [Block], [Lot], [HasLandMarkStatus], [HasEnvironmentalRestriction], [HasOpenWork], [IdCompany], [IdJobContactType], [IdContact], [IdProjectManager], [IdProjectCoordinator], [IdSignoffCoordinator], [StartDate], [EndDate], [LastModiefiedDate], [IdJobStatus], [ScopeGeneralNotes], [HasHolidayEmbargo])
                      VALUES (@JobNumber, @IdRfpAddress, @IdRfp, @IdBorough, @HouseNumber, @StreetNumber, @FloorNumber, @Apartment, @SpecialPlace, @Block, @Lot, @HasLandMarkStatus, @HasEnvironmentalRestriction, @HasOpenWork, @IdCompany, @IdJobContactType, @IdContact, @IdProjectManager, @IdProjectCoordinator, @IdSignoffCoordinator, @StartDate, @EndDate, @LastModiefiedDate, @IdJobStatus, @ScopeGeneralNotes, @HasHolidayEmbargo)
                      
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
                        IdJobStatus = p.Int(),
                        ScopeGeneralNotes = p.String(),
                        HasHolidayEmbargo = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Jobs]
                      SET [JobNumber] = @JobNumber, [IdRfpAddress] = @IdRfpAddress, [IdRfp] = @IdRfp, [IdBorough] = @IdBorough, [HouseNumber] = @HouseNumber, [StreetNumber] = @StreetNumber, [FloorNumber] = @FloorNumber, [Apartment] = @Apartment, [SpecialPlace] = @SpecialPlace, [Block] = @Block, [Lot] = @Lot, [HasLandMarkStatus] = @HasLandMarkStatus, [HasEnvironmentalRestriction] = @HasEnvironmentalRestriction, [HasOpenWork] = @HasOpenWork, [IdCompany] = @IdCompany, [IdJobContactType] = @IdJobContactType, [IdContact] = @IdContact, [IdProjectManager] = @IdProjectManager, [IdProjectCoordinator] = @IdProjectCoordinator, [IdSignoffCoordinator] = @IdSignoffCoordinator, [StartDate] = @StartDate, [EndDate] = @EndDate, [LastModiefiedDate] = @LastModiefiedDate, [IdJobStatus] = @IdJobStatus, [ScopeGeneralNotes] = @ScopeGeneralNotes, [HasHolidayEmbargo] = @HasHolidayEmbargo
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Rfp_Insert",
                p => new
                    {
                        RfpNumber = p.String(),
                        IdRfpStatus = p.Int(),
                        IdRfpAddress = p.Int(),
                        IdBorough = p.Int(),
                        HouseNumber = p.String(maxLength: 50),
                        StreetNumber = p.String(maxLength: 50),
                        FloorNumber = p.String(maxLength: 50),
                        Apartment = p.String(maxLength: 50),
                        SpecialPlace = p.String(maxLength: 50),
                        Block = p.String(maxLength: 50),
                        Lot = p.String(maxLength: 50),
                        HasLandMarkStatus = p.Boolean(),
                        HasEnvironmentalRestriction = p.Boolean(),
                        HasOpenWork = p.Boolean(),
                        IdCompany = p.Int(),
                        IdContact = p.Int(),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        Phone = p.String(maxLength: 14),
                        Email = p.String(maxLength: 250),
                        IdReferredByCompany = p.Int(),
                        IdReferredByContact = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        GoNextStep = p.Int(),
                        LastUpdatedStep = p.Int(),
                        CompletedStep = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Rfps]([RfpNumber], [IdRfpStatus], [IdRfpAddress], [IdBorough], [HouseNumber], [StreetNumber], [FloorNumber], [Apartment], [SpecialPlace], [Block], [Lot], [HasLandMarkStatus], [HasEnvironmentalRestriction], [HasOpenWork], [IdCompany], [IdContact], [Address1], [Address2], [Phone], [Email], [IdReferredByCompany], [IdReferredByContact], [LastModifiedDate], [GoNextStep], [LastUpdatedStep], [CompletedStep])
                      VALUES (@RfpNumber, @IdRfpStatus, @IdRfpAddress, @IdBorough, @HouseNumber, @StreetNumber, @FloorNumber, @Apartment, @SpecialPlace, @Block, @Lot, @HasLandMarkStatus, @HasEnvironmentalRestriction, @HasOpenWork, @IdCompany, @IdContact, @Address1, @Address2, @Phone, @Email, @IdReferredByCompany, @IdReferredByContact, @LastModifiedDate, @GoNextStep, @LastUpdatedStep, @CompletedStep)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Rfps]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Rfps] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Rfp_Update",
                p => new
                    {
                        Id = p.Int(),
                        RfpNumber = p.String(),
                        IdRfpStatus = p.Int(),
                        IdRfpAddress = p.Int(),
                        IdBorough = p.Int(),
                        HouseNumber = p.String(maxLength: 50),
                        StreetNumber = p.String(maxLength: 50),
                        FloorNumber = p.String(maxLength: 50),
                        Apartment = p.String(maxLength: 50),
                        SpecialPlace = p.String(maxLength: 50),
                        Block = p.String(maxLength: 50),
                        Lot = p.String(maxLength: 50),
                        HasLandMarkStatus = p.Boolean(),
                        HasEnvironmentalRestriction = p.Boolean(),
                        HasOpenWork = p.Boolean(),
                        IdCompany = p.Int(),
                        IdContact = p.Int(),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        Phone = p.String(maxLength: 14),
                        Email = p.String(maxLength: 250),
                        IdReferredByCompany = p.Int(),
                        IdReferredByContact = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        GoNextStep = p.Int(),
                        LastUpdatedStep = p.Int(),
                        CompletedStep = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Rfps]
                      SET [RfpNumber] = @RfpNumber, [IdRfpStatus] = @IdRfpStatus, [IdRfpAddress] = @IdRfpAddress, [IdBorough] = @IdBorough, [HouseNumber] = @HouseNumber, [StreetNumber] = @StreetNumber, [FloorNumber] = @FloorNumber, [Apartment] = @Apartment, [SpecialPlace] = @SpecialPlace, [Block] = @Block, [Lot] = @Lot, [HasLandMarkStatus] = @HasLandMarkStatus, [HasEnvironmentalRestriction] = @HasEnvironmentalRestriction, [HasOpenWork] = @HasOpenWork, [IdCompany] = @IdCompany, [IdContact] = @IdContact, [Address1] = @Address1, [Address2] = @Address2, [Phone] = @Phone, [Email] = @Email, [IdReferredByCompany] = @IdReferredByCompany, [IdReferredByContact] = @IdReferredByContact, [LastModifiedDate] = @LastModifiedDate, [GoNextStep] = @GoNextStep, [LastUpdatedStep] = @LastUpdatedStep, [CompletedStep] = @CompletedStep
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rfps", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Jobs", "Status", c => c.Int(nullable: false));
            DropForeignKey("dbo.Rfps", "IdRfpStatus", "dbo.RfpStatus");
            DropForeignKey("dbo.Jobs", "IdJobStatus", "dbo.JobStatus");
            DropIndex("dbo.Rfps", new[] { "IdRfpStatus" });
            DropIndex("dbo.Jobs", new[] { "IdJobStatus" });
            DropColumn("dbo.Rfps", "IdRfpStatus");
            DropColumn("dbo.Jobs", "IdJobStatus");
            DropTable("dbo.RfpStatus");
            DropTable("dbo.JobStatus");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
