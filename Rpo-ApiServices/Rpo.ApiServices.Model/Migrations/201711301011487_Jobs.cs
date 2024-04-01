namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Jobs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobNumber = c.String(),
                        IdRfpAddress = c.Int(nullable: false),
                        IdBorough = c.Int(nullable: false),
                        HouseNumber = c.String(maxLength: 50),
                        StreetNumber = c.String(maxLength: 50),
                        FloorNumber = c.String(maxLength: 50),
                        Apartment = c.String(maxLength: 50),
                        SpecialPlace = c.String(maxLength: 50),
                        Block = c.String(maxLength: 50),
                        Lot = c.String(maxLength: 50),
                        HasLandMarkStatus = c.Boolean(nullable: false),
                        HasEnvironmentalRestriction = c.Boolean(nullable: false),
                        HasOpenWork = c.Boolean(nullable: false),
                        IdCompany = c.Int(nullable: false),
                        JobContactType = c.Int(nullable: false),
                        IdContact = c.Int(nullable: false),
                        IdProjectManager = c.Int(nullable: false),
                        IdProjectCoordinator = c.Int(nullable: false),
                        IdSignoffCoordinator = c.Int(nullable: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Boroughs", t => t.IdBorough)
                .ForeignKey("dbo.Companies", t => t.IdCompany)
                .ForeignKey("dbo.Contacts", t => t.IdContact)
                .ForeignKey("dbo.Employees", t => t.IdProjectCoordinator)
                .ForeignKey("dbo.Employees", t => t.IdProjectManager)
                .ForeignKey("dbo.RfpAddresses", t => t.IdRfpAddress)
                .ForeignKey("dbo.Employees", t => t.IdSignoffCoordinator)
                .Index(t => t.IdRfpAddress)
                .Index(t => t.IdBorough)
                .Index(t => t.IdCompany)
                .Index(t => t.IdContact)
                .Index(t => t.IdProjectManager)
                .Index(t => t.IdProjectCoordinator)
                .Index(t => t.IdSignoffCoordinator);
            
            CreateTable(
                "dbo.JobApplications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJob = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jobs", t => t.IdJob)
                .Index(t => t.IdJob);
            
            CreateTable(
                "dbo.JobApplicationWorkPermitTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJobApplication = c.Int(nullable: false),
                        IdWorkType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobApplications", t => t.IdJobApplication)
                .ForeignKey("dbo.WorkTypes", t => t.IdWorkType)
                .Index(t => t.IdJobApplication)
                .Index(t => t.IdWorkType);
            
            CreateTable(
                "dbo.JobContacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJob = c.Int(nullable: false),
                        IdCompany = c.Int(nullable: false),
                        IdContact = c.Int(nullable: false),
                        JobContactType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.IdCompany)
                .ForeignKey("dbo.Contacts", t => t.IdContact)
                .ForeignKey("dbo.Jobs", t => t.IdJob)
                .Index(t => t.IdJob)
                .Index(t => t.IdCompany)
                .Index(t => t.IdContact);
            
            CreateTable(
                "dbo.JobDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJob = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jobs", t => t.IdJob)
                .Index(t => t.IdJob);
            
            CreateTable(
                "dbo.JobTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJob = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jobs", t => t.IdJob)
                .Index(t => t.IdJob);
            
            CreateTable(
                "dbo.JobTransmittals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJob = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Recipient = c.String(),
                        Sender = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jobs", t => t.IdJob)
                .Index(t => t.IdJob);
            
            CreateTable(
                "dbo.JobJobTypes",
                c => new
                    {
                        Job_Id = c.Int(nullable: false),
                        JobType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Job_Id, t.JobType_Id })
                .ForeignKey("dbo.Jobs", t => t.Job_Id, cascadeDelete: true)
                .ForeignKey("dbo.JobTypes", t => t.JobType_Id, cascadeDelete: true)
                .Index(t => t.Job_Id)
                .Index(t => t.JobType_Id);
            
            CreateStoredProcedure(
                "dbo.Job_Insert",
                p => new
                    {
                        JobNumber = p.String(),
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
                        JobContactType = p.Int(),
                        IdContact = p.Int(),
                        IdProjectManager = p.Int(),
                        IdProjectCoordinator = p.Int(),
                        IdSignoffCoordinator = p.Int(),
                        StartDate = p.DateTime(),
                        EndDate = p.DateTime(),
                        Status = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Jobs]([JobNumber], [IdRfpAddress], [IdBorough], [HouseNumber], [StreetNumber], [FloorNumber], [Apartment], [SpecialPlace], [Block], [Lot], [HasLandMarkStatus], [HasEnvironmentalRestriction], [HasOpenWork], [IdCompany], [JobContactType], [IdContact], [IdProjectManager], [IdProjectCoordinator], [IdSignoffCoordinator], [StartDate], [EndDate], [Status])
                      VALUES (@JobNumber, @IdRfpAddress, @IdBorough, @HouseNumber, @StreetNumber, @FloorNumber, @Apartment, @SpecialPlace, @Block, @Lot, @HasLandMarkStatus, @HasEnvironmentalRestriction, @HasOpenWork, @IdCompany, @JobContactType, @IdContact, @IdProjectManager, @IdProjectCoordinator, @IdSignoffCoordinator, @StartDate, @EndDate, @Status)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Jobs]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Jobs] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Job_Update",
                p => new
                    {
                        Id = p.Int(),
                        JobNumber = p.String(),
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
                        JobContactType = p.Int(),
                        IdContact = p.Int(),
                        IdProjectManager = p.Int(),
                        IdProjectCoordinator = p.Int(),
                        IdSignoffCoordinator = p.Int(),
                        StartDate = p.DateTime(),
                        EndDate = p.DateTime(),
                        Status = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Jobs]
                      SET [JobNumber] = @JobNumber, [IdRfpAddress] = @IdRfpAddress, [IdBorough] = @IdBorough, [HouseNumber] = @HouseNumber, [StreetNumber] = @StreetNumber, [FloorNumber] = @FloorNumber, [Apartment] = @Apartment, [SpecialPlace] = @SpecialPlace, [Block] = @Block, [Lot] = @Lot, [HasLandMarkStatus] = @HasLandMarkStatus, [HasEnvironmentalRestriction] = @HasEnvironmentalRestriction, [HasOpenWork] = @HasOpenWork, [IdCompany] = @IdCompany, [JobContactType] = @JobContactType, [IdContact] = @IdContact, [IdProjectManager] = @IdProjectManager, [IdProjectCoordinator] = @IdProjectCoordinator, [IdSignoffCoordinator] = @IdSignoffCoordinator, [StartDate] = @StartDate, [EndDate] = @EndDate, [Status] = @Status
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Job_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Jobs]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Job_Delete");
            DropStoredProcedure("dbo.Job_Update");
            DropStoredProcedure("dbo.Job_Insert");
            DropForeignKey("dbo.JobTransmittals", "IdJob", "dbo.Jobs");
            DropForeignKey("dbo.JobTasks", "IdJob", "dbo.Jobs");
            DropForeignKey("dbo.Jobs", "IdSignoffCoordinator", "dbo.Employees");
            DropForeignKey("dbo.Jobs", "IdRfpAddress", "dbo.RfpAddresses");
            DropForeignKey("dbo.Jobs", "IdProjectManager", "dbo.Employees");
            DropForeignKey("dbo.Jobs", "IdProjectCoordinator", "dbo.Employees");
            DropForeignKey("dbo.JobJobTypes", "JobType_Id", "dbo.JobTypes");
            DropForeignKey("dbo.JobJobTypes", "Job_Id", "dbo.Jobs");
            DropForeignKey("dbo.JobDocuments", "IdJob", "dbo.Jobs");
            DropForeignKey("dbo.JobContacts", "IdJob", "dbo.Jobs");
            DropForeignKey("dbo.JobContacts", "IdContact", "dbo.Contacts");
            DropForeignKey("dbo.JobContacts", "IdCompany", "dbo.Companies");
            DropForeignKey("dbo.Jobs", "IdContact", "dbo.Contacts");
            DropForeignKey("dbo.Jobs", "IdCompany", "dbo.Companies");
            DropForeignKey("dbo.Jobs", "IdBorough", "dbo.Boroughs");
            DropForeignKey("dbo.JobApplicationWorkPermitTypes", "IdWorkType", "dbo.WorkTypes");
            DropForeignKey("dbo.JobApplicationWorkPermitTypes", "IdJobApplication", "dbo.JobApplications");
            DropForeignKey("dbo.JobApplications", "IdJob", "dbo.Jobs");
            DropIndex("dbo.JobJobTypes", new[] { "JobType_Id" });
            DropIndex("dbo.JobJobTypes", new[] { "Job_Id" });
            DropIndex("dbo.JobTransmittals", new[] { "IdJob" });
            DropIndex("dbo.JobTasks", new[] { "IdJob" });
            DropIndex("dbo.JobDocuments", new[] { "IdJob" });
            DropIndex("dbo.JobContacts", new[] { "IdContact" });
            DropIndex("dbo.JobContacts", new[] { "IdCompany" });
            DropIndex("dbo.JobContacts", new[] { "IdJob" });
            DropIndex("dbo.JobApplicationWorkPermitTypes", new[] { "IdWorkType" });
            DropIndex("dbo.JobApplicationWorkPermitTypes", new[] { "IdJobApplication" });
            DropIndex("dbo.JobApplications", new[] { "IdJob" });
            DropIndex("dbo.Jobs", new[] { "IdSignoffCoordinator" });
            DropIndex("dbo.Jobs", new[] { "IdProjectCoordinator" });
            DropIndex("dbo.Jobs", new[] { "IdProjectManager" });
            DropIndex("dbo.Jobs", new[] { "IdContact" });
            DropIndex("dbo.Jobs", new[] { "IdCompany" });
            DropIndex("dbo.Jobs", new[] { "IdBorough" });
            DropIndex("dbo.Jobs", new[] { "IdRfpAddress" });
            DropTable("dbo.JobJobTypes");
            DropTable("dbo.JobTransmittals");
            DropTable("dbo.JobTasks");
            DropTable("dbo.JobDocuments");
            DropTable("dbo.JobContacts");
            DropTable("dbo.JobApplicationWorkPermitTypes");
            DropTable("dbo.JobApplications");
            DropTable("dbo.Jobs");
        }
    }
}
