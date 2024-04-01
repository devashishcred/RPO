namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHolidayEmbargoInJob : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobJobTypes", "Job_Id", "dbo.Jobs");
            DropForeignKey("dbo.JobJobTypes", "JobType_Id", "dbo.JobTypes");
            DropIndex("dbo.JobJobTypes", new[] { "Job_Id" });
            DropIndex("dbo.JobJobTypes", new[] { "JobType_Id" });
            CreateTable(
                "dbo.JobJobApplicationTypes",
                c => new
                    {
                        Job_Id = c.Int(nullable: false),
                        JobApplicationType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Job_Id, t.JobApplicationType_Id })
                .ForeignKey("dbo.Jobs", t => t.Job_Id, cascadeDelete: true)
                .ForeignKey("dbo.JobApplicationTypes", t => t.JobApplicationType_Id, cascadeDelete: true)
                .Index(t => t.Job_Id)
                .Index(t => t.JobApplicationType_Id);
            
            AddColumn("dbo.Jobs", "HasLittleE", c => c.Boolean(nullable: false));
            AddColumn("dbo.Jobs", "HasHolidayEmbargo", c => c.Boolean(nullable: false));
            DropTable("dbo.JobJobTypes");
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
                        HasLittleE = p.Boolean(),
                        HasHolidayEmbargo = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Jobs]([JobNumber], [IdRfpAddress], [IdRfp], [IdBorough], [HouseNumber], [StreetNumber], [FloorNumber], [Apartment], [SpecialPlace], [Block], [Lot], [HasLandMarkStatus], [HasEnvironmentalRestriction], [HasOpenWork], [IdCompany], [JobContactType], [IdContact], [IdProjectManager], [IdProjectCoordinator], [IdSignoffCoordinator], [StartDate], [EndDate], [LastModiefiedDate], [Status], [ScopeGeneralNotes], [HasLittleE], [HasHolidayEmbargo])
                      VALUES (@JobNumber, @IdRfpAddress, @IdRfp, @IdBorough, @HouseNumber, @StreetNumber, @FloorNumber, @Apartment, @SpecialPlace, @Block, @Lot, @HasLandMarkStatus, @HasEnvironmentalRestriction, @HasOpenWork, @IdCompany, @JobContactType, @IdContact, @IdProjectManager, @IdProjectCoordinator, @IdSignoffCoordinator, @StartDate, @EndDate, @LastModiefiedDate, @Status, @ScopeGeneralNotes, @HasLittleE, @HasHolidayEmbargo)
                      
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
                        HasLittleE = p.Boolean(),
                        HasHolidayEmbargo = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Jobs]
                      SET [JobNumber] = @JobNumber, [IdRfpAddress] = @IdRfpAddress, [IdRfp] = @IdRfp, [IdBorough] = @IdBorough, [HouseNumber] = @HouseNumber, [StreetNumber] = @StreetNumber, [FloorNumber] = @FloorNumber, [Apartment] = @Apartment, [SpecialPlace] = @SpecialPlace, [Block] = @Block, [Lot] = @Lot, [HasLandMarkStatus] = @HasLandMarkStatus, [HasEnvironmentalRestriction] = @HasEnvironmentalRestriction, [HasOpenWork] = @HasOpenWork, [IdCompany] = @IdCompany, [JobContactType] = @JobContactType, [IdContact] = @IdContact, [IdProjectManager] = @IdProjectManager, [IdProjectCoordinator] = @IdProjectCoordinator, [IdSignoffCoordinator] = @IdSignoffCoordinator, [StartDate] = @StartDate, [EndDate] = @EndDate, [LastModiefiedDate] = @LastModiefiedDate, [Status] = @Status, [ScopeGeneralNotes] = @ScopeGeneralNotes, [HasLittleE] = @HasLittleE, [HasHolidayEmbargo] = @HasHolidayEmbargo
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.JobJobTypes",
                c => new
                    {
                        Job_Id = c.Int(nullable: false),
                        JobType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Job_Id, t.JobType_Id });
            
            DropForeignKey("dbo.JobJobApplicationTypes", "JobApplicationType_Id", "dbo.JobApplicationTypes");
            DropForeignKey("dbo.JobJobApplicationTypes", "Job_Id", "dbo.Jobs");
            DropIndex("dbo.JobJobApplicationTypes", new[] { "JobApplicationType_Id" });
            DropIndex("dbo.JobJobApplicationTypes", new[] { "Job_Id" });
            DropColumn("dbo.Jobs", "HasHolidayEmbargo");
            DropColumn("dbo.Jobs", "HasLittleE");
            DropTable("dbo.JobJobApplicationTypes");
            CreateIndex("dbo.JobJobTypes", "JobType_Id");
            CreateIndex("dbo.JobJobTypes", "Job_Id");
            AddForeignKey("dbo.JobJobTypes", "JobType_Id", "dbo.JobTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.JobJobTypes", "Job_Id", "dbo.Jobs", "Id", cascadeDelete: true);
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
