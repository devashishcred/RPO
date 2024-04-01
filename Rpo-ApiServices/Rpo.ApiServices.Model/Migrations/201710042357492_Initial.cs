namespace Rpo.ApiServices.Model.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AgentCertificates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdDocumentType = c.Int(nullable: false),
                        NumberId = c.String(),
                        ExpirationDate = c.DateTime(nullable: false),
                        Pin = c.String(maxLength: 10),
                        IdEmployee = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocumentTypes", t => t.IdDocumentType, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.IdEmployee, cascadeDelete: true)
                .Index(t => t.IdDocumentType)
                .Index(t => t.IdEmployee);
            
            CreateTable(
                "dbo.DocumentTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Address1 = c.String(maxLength: 50),
                        Address2 = c.String(maxLength: 50),
                        IdCity = c.Int(nullable: false),
                        ZipCode = c.String(maxLength: 10),
                        WorkPhone = c.String(maxLength: 15),
                        WorkPhoneExt = c.String(maxLength: 5),
                        MobilePhone = c.String(maxLength: 15),
                        HomePhone = c.String(maxLength: 15),
                        Email = c.String(nullable: false, maxLength: 255),
                        Ssn = c.String(maxLength: 10),
                        Dob = c.DateTime(nullable: false),
                        StartDate = c.DateTime(),
                        FinalDate = c.DateTime(),
                        Notes = c.String(),
                        TelephonePassword = c.String(maxLength: 25),
                        ComputerPassword = c.String(maxLength: 25),
                        EfillingPassword = c.String(maxLength: 25),
                        EfillingUserName = c.String(maxLength: 25),
                        IdGroup = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.IdCity, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.IdGroup, cascadeDelete: true)
                .Index(t => t.FirstName, unique: false, name: "IX_EmployeeName")
                .Index(t => t.LastName, unique: true, name: "IX_GroupName")
                .Index(t => t.IdCity)
                .Index(t => t.Email, unique: true, name: "IX_EmployeeEmail")
                .Index(t => t.IdGroup);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        IdState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.States", t => t.IdState, cascadeDelete: true)
                .Index(t => t.IdState);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Acronym = c.String(maxLength: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmployeeDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        Content = c.Binary(),
                        IdEmployee = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.IdEmployee, cascadeDelete: true)
                .Index(t => t.IdEmployee);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        EmployeeEmployeeInfo = c.Int(nullable: false),
                        EmployeeContactInfo = c.Int(nullable: false),
                        EmployeePersonalInfo = c.Int(nullable: false),
                        EmployeeAgentCertificates = c.Int(nullable: false),
                        EmployeeSystemAccessInformation = c.Int(nullable: false),
                        EmployeeUserGroup = c.Int(nullable: false),
                        EmployeeDocuments = c.Int(nullable: false),
                        EmployeeStatus = c.Int(nullable: false),
                        Jobs = c.Int(nullable: false),
                        Contacts = c.Int(nullable: false),
                        Company = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_GroupName");
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.RpoIdentityClients",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Secret = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ApplicationType = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        RefreshTokenLifeTime = c.Int(nullable: false),
                        AllowedOrigin = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RpoIdentityRefreshTokens",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Subject = c.String(nullable: false, maxLength: 50),
                        ClientId = c.String(nullable: false, maxLength: 50),
                        IssuedUtc = c.DateTime(nullable: false),
                        ExpiresUtc = c.DateTime(nullable: false),
                        ProtectedTicket = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateStoredProcedure(
                "dbo.AgentCertificate_Insert",
                p => new
                    {
                        IdDocumentType = p.Int(),
                        NumberId = p.String(),
                        ExpirationDate = p.DateTime(),
                        Pin = p.String(maxLength: 10),
                        IdEmployee = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[AgentCertificates]([IdDocumentType], [NumberId], [ExpirationDate], [Pin], [IdEmployee])
                      VALUES (@IdDocumentType, @NumberId, @ExpirationDate, @Pin, @IdEmployee)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[AgentCertificates]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[AgentCertificates] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.AgentCertificate_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdDocumentType = p.Int(),
                        NumberId = p.String(),
                        ExpirationDate = p.DateTime(),
                        Pin = p.String(maxLength: 10),
                        IdEmployee = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[AgentCertificates]
                      SET [IdDocumentType] = @IdDocumentType, [NumberId] = @NumberId, [ExpirationDate] = @ExpirationDate, [Pin] = @Pin, [IdEmployee] = @IdEmployee
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.AgentCertificate_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[AgentCertificates]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.DocumentType_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                    },
                body:
                    @"INSERT [dbo].[DocumentTypes]([Name])
                      VALUES (@Name)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[DocumentTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[DocumentTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.DocumentType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                    },
                body:
                    @"UPDATE [dbo].[DocumentTypes]
                      SET [Name] = @Name
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.DocumentType_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[DocumentTypes]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Employee_Insert",
                p => new
                    {
                        FirstName = p.String(maxLength: 50),
                        LastName = p.String(maxLength: 50),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        IdCity = p.Int(),
                        ZipCode = p.String(maxLength: 10),
                        WorkPhone = p.String(maxLength: 15),
                        WorkPhoneExt = p.String(maxLength: 5),
                        MobilePhone = p.String(maxLength: 15),
                        HomePhone = p.String(maxLength: 15),
                        Email = p.String(maxLength: 255),
                        Ssn = p.String(maxLength: 10),
                        Dob = p.DateTime(),
                        StartDate = p.DateTime(),
                        FinalDate = p.DateTime(),
                        Notes = p.String(),
                        TelephonePassword = p.String(maxLength: 25),
                        ComputerPassword = p.String(maxLength: 25),
                        EfillingPassword = p.String(maxLength: 25),
                        EfillingUserName = p.String(maxLength: 25),
                        IdGroup = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Employees]([FirstName], [LastName], [Address1], [Address2], [IdCity], [ZipCode], [WorkPhone], [WorkPhoneExt], [MobilePhone], [HomePhone], [Email], [Ssn], [Dob], [StartDate], [FinalDate], [Notes], [TelephonePassword], [ComputerPassword], [EfillingPassword], [EfillingUserName], [IdGroup])
                      VALUES (@FirstName, @LastName, @Address1, @Address2, @IdCity, @ZipCode, @WorkPhone, @WorkPhoneExt, @MobilePhone, @HomePhone, @Email, @Ssn, @Dob, @StartDate, @FinalDate, @Notes, @TelephonePassword, @ComputerPassword, @EfillingPassword, @EfillingUserName, @IdGroup)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Employees]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Employees] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Employee_Update",
                p => new
                    {
                        Id = p.Int(),
                        FirstName = p.String(maxLength: 50),
                        LastName = p.String(maxLength: 50),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        IdCity = p.Int(),
                        ZipCode = p.String(maxLength: 10),
                        WorkPhone = p.String(maxLength: 15),
                        WorkPhoneExt = p.String(maxLength: 5),
                        MobilePhone = p.String(maxLength: 15),
                        HomePhone = p.String(maxLength: 15),
                        Email = p.String(maxLength: 255),
                        Ssn = p.String(maxLength: 10),
                        Dob = p.DateTime(),
                        StartDate = p.DateTime(),
                        FinalDate = p.DateTime(),
                        Notes = p.String(),
                        TelephonePassword = p.String(maxLength: 25),
                        ComputerPassword = p.String(maxLength: 25),
                        EfillingPassword = p.String(maxLength: 25),
                        EfillingUserName = p.String(maxLength: 25),
                        IdGroup = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Employees]
                      SET [FirstName] = @FirstName, [LastName] = @LastName, [Address1] = @Address1, [Address2] = @Address2, [IdCity] = @IdCity, [ZipCode] = @ZipCode, [WorkPhone] = @WorkPhone, [WorkPhoneExt] = @WorkPhoneExt, [MobilePhone] = @MobilePhone, [HomePhone] = @HomePhone, [Email] = @Email, [Ssn] = @Ssn, [Dob] = @Dob, [StartDate] = @StartDate, [FinalDate] = @FinalDate, [Notes] = @Notes, [TelephonePassword] = @TelephonePassword, [ComputerPassword] = @ComputerPassword, [EfillingPassword] = @EfillingPassword, [EfillingUserName] = @EfillingUserName, [IdGroup] = @IdGroup
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Employee_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Employees]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.City_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        IdState = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Cities]([Name], [IdState])
                      VALUES (@Name, @IdState)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Cities]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Cities] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.City_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        IdState = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Cities]
                      SET [Name] = @Name, [IdState] = @IdState
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.City_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Cities]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.State_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        Acronym = p.String(maxLength: 2),
                    },
                body:
                    @"INSERT [dbo].[States]([Name], [Acronym])
                      VALUES (@Name, @Acronym)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[States]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[States] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.State_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        Acronym = p.String(maxLength: 2),
                    },
                body:
                    @"UPDATE [dbo].[States]
                      SET [Name] = @Name, [Acronym] = @Acronym
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.State_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[States]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.EmployeeDocument_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 255),
                        Content = p.Binary(),
                        IdEmployee = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[EmployeeDocuments]([Name], [Content], [IdEmployee])
                      VALUES (@Name, @Content, @IdEmployee)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[EmployeeDocuments]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[EmployeeDocuments] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.EmployeeDocument_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 255),
                        Content = p.Binary(),
                        IdEmployee = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[EmployeeDocuments]
                      SET [Name] = @Name, [Content] = @Content, [IdEmployee] = @IdEmployee
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.EmployeeDocument_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[EmployeeDocuments]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Group_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 100),
                        Description = p.String(),
                        EmployeeEmployeeInfo = p.Int(),
                        EmployeeContactInfo = p.Int(),
                        EmployeePersonalInfo = p.Int(),
                        EmployeeAgentCertificates = p.Int(),
                        EmployeeSystemAccessInformation = p.Int(),
                        EmployeeUserGroup = p.Int(),
                        EmployeeDocuments = p.Int(),
                        EmployeeStatus = p.Int(),
                        Jobs = p.Int(),
                        Contacts = p.Int(),
                        Company = p.Int(),
                        IsActive = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Groups]([Name], [Description], [EmployeeEmployeeInfo], [EmployeeContactInfo], [EmployeePersonalInfo], [EmployeeAgentCertificates], [EmployeeSystemAccessInformation], [EmployeeUserGroup], [EmployeeDocuments], [EmployeeStatus], [Jobs], [Contacts], [Company], [IsActive])
                      VALUES (@Name, @Description, @EmployeeEmployeeInfo, @EmployeeContactInfo, @EmployeePersonalInfo, @EmployeeAgentCertificates, @EmployeeSystemAccessInformation, @EmployeeUserGroup, @EmployeeDocuments, @EmployeeStatus, @Jobs, @Contacts, @Company, @IsActive)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Groups]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Groups] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Group_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 100),
                        Description = p.String(),
                        EmployeeEmployeeInfo = p.Int(),
                        EmployeeContactInfo = p.Int(),
                        EmployeePersonalInfo = p.Int(),
                        EmployeeAgentCertificates = p.Int(),
                        EmployeeSystemAccessInformation = p.Int(),
                        EmployeeUserGroup = p.Int(),
                        EmployeeDocuments = p.Int(),
                        EmployeeStatus = p.Int(),
                        Jobs = p.Int(),
                        Contacts = p.Int(),
                        Company = p.Int(),
                        IsActive = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Groups]
                      SET [Name] = @Name, [Description] = @Description, [EmployeeEmployeeInfo] = @EmployeeEmployeeInfo, [EmployeeContactInfo] = @EmployeeContactInfo, [EmployeePersonalInfo] = @EmployeePersonalInfo, [EmployeeAgentCertificates] = @EmployeeAgentCertificates, [EmployeeSystemAccessInformation] = @EmployeeSystemAccessInformation, [EmployeeUserGroup] = @EmployeeUserGroup, [EmployeeDocuments] = @EmployeeDocuments, [EmployeeStatus] = @EmployeeStatus, [Jobs] = @Jobs, [Contacts] = @Contacts, [Company] = @Company, [IsActive] = @IsActive
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Group_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Groups]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Group_Delete");
            DropStoredProcedure("dbo.Group_Update");
            DropStoredProcedure("dbo.Group_Insert");
            DropStoredProcedure("dbo.EmployeeDocument_Delete");
            DropStoredProcedure("dbo.EmployeeDocument_Update");
            DropStoredProcedure("dbo.EmployeeDocument_Insert");
            DropStoredProcedure("dbo.State_Delete");
            DropStoredProcedure("dbo.State_Update");
            DropStoredProcedure("dbo.State_Insert");
            DropStoredProcedure("dbo.City_Delete");
            DropStoredProcedure("dbo.City_Update");
            DropStoredProcedure("dbo.City_Insert");
            DropStoredProcedure("dbo.Employee_Delete");
            DropStoredProcedure("dbo.Employee_Update");
            DropStoredProcedure("dbo.Employee_Insert");
            DropStoredProcedure("dbo.DocumentType_Delete");
            DropStoredProcedure("dbo.DocumentType_Update");
            DropStoredProcedure("dbo.DocumentType_Insert");
            DropStoredProcedure("dbo.AgentCertificate_Delete");
            DropStoredProcedure("dbo.AgentCertificate_Update");
            DropStoredProcedure("dbo.AgentCertificate_Insert");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Employees", "IdGroup", "dbo.Groups");
            DropForeignKey("dbo.EmployeeDocuments", "IdEmployee", "dbo.Employees");
            DropForeignKey("dbo.Employees", "IdCity", "dbo.Cities");
            DropForeignKey("dbo.Cities", "IdState", "dbo.States");
            DropForeignKey("dbo.AgentCertificates", "IdEmployee", "dbo.Employees");
            DropForeignKey("dbo.AgentCertificates", "IdDocumentType", "dbo.DocumentTypes");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Groups", "IX_GroupName");
            DropIndex("dbo.EmployeeDocuments", new[] { "IdEmployee" });
            DropIndex("dbo.Cities", new[] { "IdState" });
            DropIndex("dbo.Employees", new[] { "IdGroup" });
            DropIndex("dbo.Employees", "IX_EmployeeEmail");
            DropIndex("dbo.Employees", new[] { "IdCity" });
            DropIndex("dbo.Employees", "IX_GroupName");
            DropIndex("dbo.Employees", "IX_EmployeeName");
            DropIndex("dbo.AgentCertificates", new[] { "IdEmployee" });
            DropIndex("dbo.AgentCertificates", new[] { "IdDocumentType" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.RpoIdentityRefreshTokens");
            DropTable("dbo.RpoIdentityClients");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Groups");
            DropTable("dbo.EmployeeDocuments");
            DropTable("dbo.States");
            DropTable("dbo.Cities");
            DropTable("dbo.Employees");
            DropTable("dbo.DocumentTypes");
            DropTable("dbo.AgentCertificates");
        }
    }
}
