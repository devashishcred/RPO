namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobsMilestonesScopes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobMilestones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJob = c.Int(nullable: false),
                        Name = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jobs", t => t.IdJob)
                .Index(t => t.IdJob);
            
            CreateTable(
                "dbo.JobScopes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJob = c.Int(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jobs", t => t.IdJob)
                .Index(t => t.IdJob);
            
            AddColumn("dbo.Jobs", "ScopeGeneralNotes", c => c.String());
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
                        JobContactType = p.Int(),
                        IdContact = p.Int(),
                        IdProjectManager = p.Int(),
                        IdProjectCoordinator = p.Int(),
                        IdSignoffCoordinator = p.Int(),
                        StartDate = p.DateTime(),
                        EndDate = p.DateTime(),
                        LastModiefiedDate = p.DateTime(),
                        Status = p.Int(),
                        ScopeGeneralNotes = p.String(),
                    },
                body:
                    @"INSERT [dbo].[Jobs]([JobNumber], [IdRfpAddress], [IdRfp], [IdBorough], [HouseNumber], [StreetNumber], [FloorNumber], [Apartment], [SpecialPlace], [Block], [Lot], [HasLandMarkStatus], [HasEnvironmentalRestriction], [HasOpenWork], [IdCompany], [JobContactType], [IdContact], [IdProjectManager], [IdProjectCoordinator], [IdSignoffCoordinator], [StartDate], [EndDate], [LastModiefiedDate], [Status], [ScopeGeneralNotes])
                      VALUES (@JobNumber, @IdRfpAddress, @IdRfp, @IdBorough, @HouseNumber, @StreetNumber, @FloorNumber, @Apartment, @SpecialPlace, @Block, @Lot, @HasLandMarkStatus, @HasEnvironmentalRestriction, @HasOpenWork, @IdCompany, @JobContactType, @IdContact, @IdProjectManager, @IdProjectCoordinator, @IdSignoffCoordinator, @StartDate, @EndDate, @LastModiefiedDate, @Status, @ScopeGeneralNotes)
                      
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
                        JobContactType = p.Int(),
                        IdContact = p.Int(),
                        IdProjectManager = p.Int(),
                        IdProjectCoordinator = p.Int(),
                        IdSignoffCoordinator = p.Int(),
                        StartDate = p.DateTime(),
                        EndDate = p.DateTime(),
                        LastModiefiedDate = p.DateTime(),
                        Status = p.Int(),
                        ScopeGeneralNotes = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[Jobs]
                      SET [JobNumber] = @JobNumber, [IdRfpAddress] = @IdRfpAddress, [IdRfp] = @IdRfp, [IdBorough] = @IdBorough, [HouseNumber] = @HouseNumber, [StreetNumber] = @StreetNumber, [FloorNumber] = @FloorNumber, [Apartment] = @Apartment, [SpecialPlace] = @SpecialPlace, [Block] = @Block, [Lot] = @Lot, [HasLandMarkStatus] = @HasLandMarkStatus, [HasEnvironmentalRestriction] = @HasEnvironmentalRestriction, [HasOpenWork] = @HasOpenWork, [IdCompany] = @IdCompany, [JobContactType] = @JobContactType, [IdContact] = @IdContact, [IdProjectManager] = @IdProjectManager, [IdProjectCoordinator] = @IdProjectCoordinator, [IdSignoffCoordinator] = @IdSignoffCoordinator, [StartDate] = @StartDate, [EndDate] = @EndDate, [LastModiefiedDate] = @LastModiefiedDate, [Status] = @Status, [ScopeGeneralNotes] = @ScopeGeneralNotes
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobScopes", "IdJob", "dbo.Jobs");
            DropForeignKey("dbo.JobMilestones", "IdJob", "dbo.Jobs");
            DropIndex("dbo.JobScopes", new[] { "IdJob" });
            DropIndex("dbo.JobMilestones", new[] { "IdJob" });
            DropColumn("dbo.Jobs", "ScopeGeneralNotes");
            DropTable("dbo.JobScopes");
            DropTable("dbo.JobMilestones");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
