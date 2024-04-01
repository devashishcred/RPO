namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cuimigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        CompanyName = c.String(),
                        IdContcat = c.Int(nullable: false),
                        EmailAddress = c.String(nullable: false, maxLength: 255),
                        LoginPassword = c.String(maxLength: 25),
                        ProfileImage = c.String(maxLength: 200),
                        IsActive = c.Boolean(nullable: false),
                        IdGroup = c.Int(nullable: false),
                        Permissions = c.String(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedByCus = c.Int(),
                        LastModifiedDate = c.DateTime(),
                        RenewalDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        CustomerConsent = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.IdContcat)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Groups", t => t.IdGroup)
                .ForeignKey("dbo.Customers", t => t.LastModifiedByCus)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.IdContcat)
                .Index(t => t.IdGroup)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy)
                .Index(t => t.LastModifiedByCus);
            
            CreateTable(
                "dbo.ClientNoteCustomers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJobChecklistItemDetail = c.Int(nullable: false),
                        Idcustomer = c.Int(nullable: false),
                        Description = c.String(),
                        Isinternal = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Idcustomer)
                .ForeignKey("dbo.JobChecklistItemDetails", t => t.IdJobChecklistItemDetail)
                .Index(t => t.IdJobChecklistItemDetail)
                .Index(t => t.Idcustomer);
            
            CreateTable(
                "dbo.ClientNotePlumbingCustomers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJobPlumbingInspection = c.Int(nullable: false),
                        Idcustomer = c.Int(nullable: false),
                        Description = c.String(),
                        Isinternal = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Idcustomer)
                .ForeignKey("dbo.JobPlumbingInspections", t => t.IdJobPlumbingInspection)
                .Index(t => t.IdJobPlumbingInspection)
                .Index(t => t.Idcustomer);
            
            CreateTable(
                "dbo.CustomerInvitationStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdContact = c.Int(nullable: false),
                        IdJob = c.Int(),
                        CUI_Invitatuionstatus = c.Int(nullable: false),
                        InvitationSentCount = c.Int(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        ReminderCount = c.Int(nullable: false),
                        UpdatedDate = c.DateTime(),
                        EmailAddress = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.IdContact)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Jobs", t => t.IdJob)
                .Index(t => t.IdContact)
                .Index(t => t.IdJob)
                .Index(t => t.CreatedBy);
            
            CreateTable(
                "dbo.CustomerJobAccesses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdCustomer = c.Int(nullable: false),
                        IdJob = c.Int(nullable: false),
                        CUI_Status = c.Int(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Customers", t => t.IdCustomer)
                .ForeignKey("dbo.Jobs", t => t.IdJob)
                .Index(t => t.IdCustomer)
                .Index(t => t.IdJob)
                .Index(t => t.CreatedBy);
            
            CreateTable(
                "dbo.CustomerJobNames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(),
                        IdCustomerJobAccess = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerJobAccesses", t => t.IdCustomerJobAccess)
                .Index(t => t.IdCustomerJobAccess);
            
            CreateTable(
                "dbo.CustomerNotifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NotificationMessage = c.String(),
                        NotificationDate = c.DateTime(nullable: false),
                        IdCustomerNotified = c.Int(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        IsView = c.Boolean(nullable: false),
                        RedirectionUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.IdCustomerNotified)
                .Index(t => t.IdCustomerNotified);
            
            CreateTable(
                "dbo.CustomerNotificationSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdCustomer = c.Int(nullable: false),
                        ProjectAccessEmail = c.Boolean(nullable: false),
                        ProjectAccessInApp = c.Boolean(nullable: false),
                        ViolationEmail = c.Boolean(nullable: false),
                        ViolationInapp = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.IdCustomer)
                .Index(t => t.IdCustomer);
            
            CreateTable(
                "dbo.CustomerPasswordResets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdCustomer = c.Int(nullable: false),
                        EmailAddress = c.String(),
                        RequestDate = c.DateTime(nullable: false),
                        IsPasswordchanged = c.Boolean(nullable: false),
                        PasswordChangedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.IdCustomer)
                .Index(t => t.IdCustomer);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        NewsImagePath = c.String(),
                        URL = c.String(),
                        Description = c.String(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .Index(t => t.CreatedBy);
            
            AddColumn("dbo.Addresses", "LastModifiedByCus", c => c.Int());
            AddColumn("dbo.Contacts", "IsHidden", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobContacts", "IshiddenFromCustomer", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Addresses", "LastModifiedByCus");
            AddForeignKey("dbo.Addresses", "LastModifiedByCus", "dbo.Customers", "Id");
            CreateStoredProcedure(
                "dbo.Customer_Insert",
                p => new
                    {
                        FirstName = p.String(),
                        LastName = p.String(),
                        CompanyName = p.String(),
                        IdContcat = p.Int(),
                        EmailAddress = p.String(maxLength: 255),
                        LoginPassword = p.String(maxLength: 25),
                        ProfileImage = p.String(maxLength: 200),
                        IsActive = p.Boolean(),
                        IdGroup = p.Int(),
                        Permissions = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedByCus = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        RenewalDate = p.DateTime(),
                        Status = p.Int(),
                        CustomerConsent = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Customers]([FirstName], [LastName], [CompanyName], [IdContcat], [EmailAddress], [LoginPassword], [ProfileImage], [IsActive], [IdGroup], [Permissions], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedByCus], [LastModifiedDate], [RenewalDate], [Status], [CustomerConsent])
                      VALUES (@FirstName, @LastName, @CompanyName, @IdContcat, @EmailAddress, @LoginPassword, @ProfileImage, @IsActive, @IdGroup, @Permissions, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedByCus, @LastModifiedDate, @RenewalDate, @Status, @CustomerConsent)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Customers]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Customers] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Customer_Update",
                p => new
                    {
                        Id = p.Int(),
                        FirstName = p.String(),
                        LastName = p.String(),
                        CompanyName = p.String(),
                        IdContcat = p.Int(),
                        EmailAddress = p.String(maxLength: 255),
                        LoginPassword = p.String(maxLength: 25),
                        ProfileImage = p.String(maxLength: 200),
                        IsActive = p.Boolean(),
                        IdGroup = p.Int(),
                        Permissions = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedByCus = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        RenewalDate = p.DateTime(),
                        Status = p.Int(),
                        CustomerConsent = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Customers]
                      SET [FirstName] = @FirstName, [LastName] = @LastName, [CompanyName] = @CompanyName, [IdContcat] = @IdContcat, [EmailAddress] = @EmailAddress, [LoginPassword] = @LoginPassword, [ProfileImage] = @ProfileImage, [IsActive] = @IsActive, [IdGroup] = @IdGroup, [Permissions] = @Permissions, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedByCus] = @LastModifiedByCus, [LastModifiedDate] = @LastModifiedDate, [RenewalDate] = @RenewalDate, [Status] = @Status, [CustomerConsent] = @CustomerConsent
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Customer_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Customers]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ClientNoteCustomer_Insert",
                p => new
                    {
                        IdJobChecklistItemDetail = p.Int(),
                        Idcustomer = p.Int(),
                        Description = p.String(),
                        Isinternal = p.Boolean(),
                        CreatedDate = p.DateTime(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[ClientNoteCustomers]([IdJobChecklistItemDetail], [Idcustomer], [Description], [Isinternal], [CreatedDate], [LastModifiedDate])
                      VALUES (@IdJobChecklistItemDetail, @Idcustomer, @Description, @Isinternal, @CreatedDate, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ClientNoteCustomers]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ClientNoteCustomers] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.ClientNoteCustomer_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdJobChecklistItemDetail = p.Int(),
                        Idcustomer = p.Int(),
                        Description = p.String(),
                        Isinternal = p.Boolean(),
                        CreatedDate = p.DateTime(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[ClientNoteCustomers]
                      SET [IdJobChecklistItemDetail] = @IdJobChecklistItemDetail, [Idcustomer] = @Idcustomer, [Description] = @Description, [Isinternal] = @Isinternal, [CreatedDate] = @CreatedDate, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ClientNoteCustomer_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ClientNoteCustomers]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ClientNotePlumbingCustomer_Insert",
                p => new
                    {
                        IdJobPlumbingInspection = p.Int(),
                        Idcustomer = p.Int(),
                        Description = p.String(),
                        Isinternal = p.Boolean(),
                        CreatedDate = p.DateTime(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[ClientNotePlumbingCustomers]([IdJobPlumbingInspection], [Idcustomer], [Description], [Isinternal], [CreatedDate], [LastModifiedDate])
                      VALUES (@IdJobPlumbingInspection, @Idcustomer, @Description, @Isinternal, @CreatedDate, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ClientNotePlumbingCustomers]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ClientNotePlumbingCustomers] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.ClientNotePlumbingCustomer_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdJobPlumbingInspection = p.Int(),
                        Idcustomer = p.Int(),
                        Description = p.String(),
                        Isinternal = p.Boolean(),
                        CreatedDate = p.DateTime(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[ClientNotePlumbingCustomers]
                      SET [IdJobPlumbingInspection] = @IdJobPlumbingInspection, [Idcustomer] = @Idcustomer, [Description] = @Description, [Isinternal] = @Isinternal, [CreatedDate] = @CreatedDate, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ClientNotePlumbingCustomer_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ClientNotePlumbingCustomers]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CustomerInvitationStatus_Insert",
                p => new
                    {
                        IdContact = p.Int(),
                        IdJob = p.Int(),
                        CUI_Invitatuionstatus = p.Int(),
                        InvitationSentCount = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        ReminderCount = p.Int(),
                        UpdatedDate = p.DateTime(),
                        EmailAddress = p.String(),
                    },
                body:
                    @"INSERT [dbo].[CustomerInvitationStatus]([IdContact], [IdJob], [CUI_Invitatuionstatus], [InvitationSentCount], [CreatedBy], [CreatedDate], [ReminderCount], [UpdatedDate], [EmailAddress])
                      VALUES (@IdContact, @IdJob, @CUI_Invitatuionstatus, @InvitationSentCount, @CreatedBy, @CreatedDate, @ReminderCount, @UpdatedDate, @EmailAddress)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[CustomerInvitationStatus]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[CustomerInvitationStatus] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.CustomerInvitationStatus_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdContact = p.Int(),
                        IdJob = p.Int(),
                        CUI_Invitatuionstatus = p.Int(),
                        InvitationSentCount = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        ReminderCount = p.Int(),
                        UpdatedDate = p.DateTime(),
                        EmailAddress = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[CustomerInvitationStatus]
                      SET [IdContact] = @IdContact, [IdJob] = @IdJob, [CUI_Invitatuionstatus] = @CUI_Invitatuionstatus, [InvitationSentCount] = @InvitationSentCount, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [ReminderCount] = @ReminderCount, [UpdatedDate] = @UpdatedDate, [EmailAddress] = @EmailAddress
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CustomerInvitationStatus_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[CustomerInvitationStatus]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CustomerJobAccess_Insert",
                p => new
                    {
                        IdCustomer = p.Int(),
                        IdJob = p.Int(),
                        CUI_Status = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[CustomerJobAccesses]([IdCustomer], [IdJob], [CUI_Status], [CreatedBy], [CreatedDate])
                      VALUES (@IdCustomer, @IdJob, @CUI_Status, @CreatedBy, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[CustomerJobAccesses]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[CustomerJobAccesses] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.CustomerJobAccess_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdCustomer = p.Int(),
                        IdJob = p.Int(),
                        CUI_Status = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[CustomerJobAccesses]
                      SET [IdCustomer] = @IdCustomer, [IdJob] = @IdJob, [CUI_Status] = @CUI_Status, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CustomerJobAccess_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[CustomerJobAccesses]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CustomerJobName_Insert",
                p => new
                    {
                        ProjectName = p.String(),
                        IdCustomerJobAccess = p.Int(),
                        IsActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[CustomerJobNames]([ProjectName], [IdCustomerJobAccess], [IsActive], [CreatedDate])
                      VALUES (@ProjectName, @IdCustomerJobAccess, @IsActive, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[CustomerJobNames]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[CustomerJobNames] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.CustomerJobName_Update",
                p => new
                    {
                        Id = p.Int(),
                        ProjectName = p.String(),
                        IdCustomerJobAccess = p.Int(),
                        IsActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[CustomerJobNames]
                      SET [ProjectName] = @ProjectName, [IdCustomerJobAccess] = @IdCustomerJobAccess, [IsActive] = @IsActive, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CustomerJobName_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[CustomerJobNames]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CustomerNotificationSetting_Insert",
                p => new
                    {
                        IdCustomer = p.Int(),
                        ProjectAccessEmail = p.Boolean(),
                        ProjectAccessInApp = p.Boolean(),
                        ViolationEmail = p.Boolean(),
                        ViolationInapp = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[CustomerNotificationSettings]([IdCustomer], [ProjectAccessEmail], [ProjectAccessInApp], [ViolationEmail], [ViolationInapp])
                      VALUES (@IdCustomer, @ProjectAccessEmail, @ProjectAccessInApp, @ViolationEmail, @ViolationInapp)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[CustomerNotificationSettings]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[CustomerNotificationSettings] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.CustomerNotificationSetting_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdCustomer = p.Int(),
                        ProjectAccessEmail = p.Boolean(),
                        ProjectAccessInApp = p.Boolean(),
                        ViolationEmail = p.Boolean(),
                        ViolationInapp = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[CustomerNotificationSettings]
                      SET [IdCustomer] = @IdCustomer, [ProjectAccessEmail] = @ProjectAccessEmail, [ProjectAccessInApp] = @ProjectAccessInApp, [ViolationEmail] = @ViolationEmail, [ViolationInapp] = @ViolationInapp
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CustomerNotificationSetting_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[CustomerNotificationSettings]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CustomerPasswordReset_Insert",
                p => new
                    {
                        IdCustomer = p.Int(),
                        EmailAddress = p.String(),
                        RequestDate = p.DateTime(),
                        IsPasswordchanged = p.Boolean(),
                        PasswordChangedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[CustomerPasswordResets]([IdCustomer], [EmailAddress], [RequestDate], [IsPasswordchanged], [PasswordChangedDate])
                      VALUES (@IdCustomer, @EmailAddress, @RequestDate, @IsPasswordchanged, @PasswordChangedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[CustomerPasswordResets]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[CustomerPasswordResets] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.CustomerPasswordReset_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdCustomer = p.Int(),
                        EmailAddress = p.String(),
                        RequestDate = p.DateTime(),
                        IsPasswordchanged = p.Boolean(),
                        PasswordChangedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[CustomerPasswordResets]
                      SET [IdCustomer] = @IdCustomer, [EmailAddress] = @EmailAddress, [RequestDate] = @RequestDate, [IsPasswordchanged] = @IsPasswordchanged, [PasswordChangedDate] = @PasswordChangedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CustomerPasswordReset_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[CustomerPasswordResets]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.News_Insert",
                p => new
                    {
                        Title = p.String(),
                        NewsImagePath = p.String(),
                        URL = p.String(),
                        Description = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[News]([Title], [NewsImagePath], [URL], [Description], [CreatedBy], [CreatedDate])
                      VALUES (@Title, @NewsImagePath, @URL, @Description, @CreatedBy, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[News]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[News] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.News_Update",
                p => new
                    {
                        Id = p.Int(),
                        Title = p.String(),
                        NewsImagePath = p.String(),
                        URL = p.String(),
                        Description = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[News]
                      SET [Title] = @Title, [NewsImagePath] = @NewsImagePath, [URL] = @URL, [Description] = @Description, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.News_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[News]
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Address_Insert",
                p => new
                    {
                        IdAddressType = p.Int(),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        City = p.String(),
                        IdState = p.Int(),
                        ZipCode = p.String(maxLength: 10),
                        Phone = p.String(maxLength: 15),
                        Fax = p.String(maxLength: 15),
                        IdCompany = p.Int(),
                        IdContact = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        IsMainAddress = p.Boolean(),
                        LastModifiedByCus = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Addresses]([IdAddressType], [Address1], [Address2], [City], [IdState], [ZipCode], [Phone], [Fax], [IdCompany], [IdContact], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate], [IsMainAddress], [LastModifiedByCus])
                      VALUES (@IdAddressType, @Address1, @Address2, @City, @IdState, @ZipCode, @Phone, @Fax, @IdCompany, @IdContact, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate, @IsMainAddress, @LastModifiedByCus)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Addresses]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Addresses] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Address_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdAddressType = p.Int(),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        City = p.String(),
                        IdState = p.Int(),
                        ZipCode = p.String(maxLength: 10),
                        Phone = p.String(maxLength: 15),
                        Fax = p.String(maxLength: 15),
                        IdCompany = p.Int(),
                        IdContact = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        IsMainAddress = p.Boolean(),
                        LastModifiedByCus = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Addresses]
                      SET [IdAddressType] = @IdAddressType, [Address1] = @Address1, [Address2] = @Address2, [City] = @City, [IdState] = @IdState, [ZipCode] = @ZipCode, [Phone] = @Phone, [Fax] = @Fax, [IdCompany] = @IdCompany, [IdContact] = @IdContact, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate, [IsMainAddress] = @IsMainAddress, [LastModifiedByCus] = @LastModifiedByCus
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Contact_Insert",
                p => new
                    {
                        PersonalType = p.Int(),
                        IdPrefix = p.Int(),
                        IdSuffix = p.Int(),
                        FirstName = p.String(maxLength: 50),
                        MiddleName = p.String(maxLength: 2),
                        LastName = p.String(maxLength: 50),
                        IdCompany = p.Int(),
                        IdContactTitle = p.Int(),
                        BirthDate = p.DateTime(),
                        WorkPhone = p.String(maxLength: 15),
                        WorkPhoneExt = p.String(maxLength: 5),
                        MobilePhone = p.String(maxLength: 15),
                        OtherPhone = p.String(maxLength: 15),
                        Email = p.String(maxLength: 255),
                        ContactImagePath = p.String(maxLength: 200),
                        ContactImageThumbPath = p.String(maxLength: 200),
                        Notes = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        FaxNumber = p.String(),
                        IdPrimaryCompanyAddress = p.Int(),
                        IsPrimaryCompanyAddress = p.Boolean(),
                        IsActive = p.Boolean(),
                        IsHidden = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Contacts]([PersonalType], [IdPrefix], [IdSuffix], [FirstName], [MiddleName], [LastName], [IdCompany], [IdContactTitle], [BirthDate], [WorkPhone], [WorkPhoneExt], [MobilePhone], [OtherPhone], [Email], [ContactImagePath], [ContactImageThumbPath], [Notes], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate], [FaxNumber], [IdPrimaryCompanyAddress], [IsPrimaryCompanyAddress], [IsActive], [IsHidden])
                      VALUES (@PersonalType, @IdPrefix, @IdSuffix, @FirstName, @MiddleName, @LastName, @IdCompany, @IdContactTitle, @BirthDate, @WorkPhone, @WorkPhoneExt, @MobilePhone, @OtherPhone, @Email, @ContactImagePath, @ContactImageThumbPath, @Notes, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate, @FaxNumber, @IdPrimaryCompanyAddress, @IsPrimaryCompanyAddress, @IsActive, @IsHidden)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Contacts]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Contacts] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Contact_Update",
                p => new
                    {
                        Id = p.Int(),
                        PersonalType = p.Int(),
                        IdPrefix = p.Int(),
                        IdSuffix = p.Int(),
                        FirstName = p.String(maxLength: 50),
                        MiddleName = p.String(maxLength: 2),
                        LastName = p.String(maxLength: 50),
                        IdCompany = p.Int(),
                        IdContactTitle = p.Int(),
                        BirthDate = p.DateTime(),
                        WorkPhone = p.String(maxLength: 15),
                        WorkPhoneExt = p.String(maxLength: 5),
                        MobilePhone = p.String(maxLength: 15),
                        OtherPhone = p.String(maxLength: 15),
                        Email = p.String(maxLength: 255),
                        ContactImagePath = p.String(maxLength: 200),
                        ContactImageThumbPath = p.String(maxLength: 200),
                        Notes = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        FaxNumber = p.String(),
                        IdPrimaryCompanyAddress = p.Int(),
                        IsPrimaryCompanyAddress = p.Boolean(),
                        IsActive = p.Boolean(),
                        IsHidden = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Contacts]
                      SET [PersonalType] = @PersonalType, [IdPrefix] = @IdPrefix, [IdSuffix] = @IdSuffix, [FirstName] = @FirstName, [MiddleName] = @MiddleName, [LastName] = @LastName, [IdCompany] = @IdCompany, [IdContactTitle] = @IdContactTitle, [BirthDate] = @BirthDate, [WorkPhone] = @WorkPhone, [WorkPhoneExt] = @WorkPhoneExt, [MobilePhone] = @MobilePhone, [OtherPhone] = @OtherPhone, [Email] = @Email, [ContactImagePath] = @ContactImagePath, [ContactImageThumbPath] = @ContactImageThumbPath, [Notes] = @Notes, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate, [FaxNumber] = @FaxNumber, [IdPrimaryCompanyAddress] = @IdPrimaryCompanyAddress, [IsPrimaryCompanyAddress] = @IsPrimaryCompanyAddress, [IsActive] = @IsActive, [IsHidden] = @IsHidden
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.News_Delete");
            DropStoredProcedure("dbo.News_Update");
            DropStoredProcedure("dbo.News_Insert");
            DropStoredProcedure("dbo.CustomerPasswordReset_Delete");
            DropStoredProcedure("dbo.CustomerPasswordReset_Update");
            DropStoredProcedure("dbo.CustomerPasswordReset_Insert");
            DropStoredProcedure("dbo.CustomerNotificationSetting_Delete");
            DropStoredProcedure("dbo.CustomerNotificationSetting_Update");
            DropStoredProcedure("dbo.CustomerNotificationSetting_Insert");
            DropStoredProcedure("dbo.CustomerJobName_Delete");
            DropStoredProcedure("dbo.CustomerJobName_Update");
            DropStoredProcedure("dbo.CustomerJobName_Insert");
            DropStoredProcedure("dbo.CustomerJobAccess_Delete");
            DropStoredProcedure("dbo.CustomerJobAccess_Update");
            DropStoredProcedure("dbo.CustomerJobAccess_Insert");
            DropStoredProcedure("dbo.CustomerInvitationStatus_Delete");
            DropStoredProcedure("dbo.CustomerInvitationStatus_Update");
            DropStoredProcedure("dbo.CustomerInvitationStatus_Insert");
            DropStoredProcedure("dbo.ClientNotePlumbingCustomer_Delete");
            DropStoredProcedure("dbo.ClientNotePlumbingCustomer_Update");
            DropStoredProcedure("dbo.ClientNotePlumbingCustomer_Insert");
            DropStoredProcedure("dbo.ClientNoteCustomer_Delete");
            DropStoredProcedure("dbo.ClientNoteCustomer_Update");
            DropStoredProcedure("dbo.ClientNoteCustomer_Insert");
            DropStoredProcedure("dbo.Customer_Delete");
            DropStoredProcedure("dbo.Customer_Update");
            DropStoredProcedure("dbo.Customer_Insert");
            DropForeignKey("dbo.News", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.CustomerPasswordResets", "IdCustomer", "dbo.Customers");
            DropForeignKey("dbo.CustomerNotificationSettings", "IdCustomer", "dbo.Customers");
            DropForeignKey("dbo.CustomerNotifications", "IdCustomerNotified", "dbo.Customers");
            DropForeignKey("dbo.CustomerJobNames", "IdCustomerJobAccess", "dbo.CustomerJobAccesses");
            DropForeignKey("dbo.CustomerJobAccesses", "IdJob", "dbo.Jobs");
            DropForeignKey("dbo.CustomerJobAccesses", "IdCustomer", "dbo.Customers");
            DropForeignKey("dbo.CustomerJobAccesses", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.CustomerInvitationStatus", "IdJob", "dbo.Jobs");
            DropForeignKey("dbo.CustomerInvitationStatus", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.CustomerInvitationStatus", "IdContact", "dbo.Contacts");
            DropForeignKey("dbo.ClientNotePlumbingCustomers", "IdJobPlumbingInspection", "dbo.JobPlumbingInspections");
            DropForeignKey("dbo.ClientNotePlumbingCustomers", "Idcustomer", "dbo.Customers");
            DropForeignKey("dbo.ClientNoteCustomers", "IdJobChecklistItemDetail", "dbo.JobChecklistItemDetails");
            DropForeignKey("dbo.ClientNoteCustomers", "Idcustomer", "dbo.Customers");
            DropForeignKey("dbo.Addresses", "LastModifiedByCus", "dbo.Customers");
            DropForeignKey("dbo.Customers", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.Customers", "LastModifiedByCus", "dbo.Customers");
            DropForeignKey("dbo.Customers", "IdGroup", "dbo.Groups");
            DropForeignKey("dbo.Customers", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.Customers", "IdContcat", "dbo.Contacts");
            DropIndex("dbo.News", new[] { "CreatedBy" });
            DropIndex("dbo.CustomerPasswordResets", new[] { "IdCustomer" });
            DropIndex("dbo.CustomerNotificationSettings", new[] { "IdCustomer" });
            DropIndex("dbo.CustomerNotifications", new[] { "IdCustomerNotified" });
            DropIndex("dbo.CustomerJobNames", new[] { "IdCustomerJobAccess" });
            DropIndex("dbo.CustomerJobAccesses", new[] { "CreatedBy" });
            DropIndex("dbo.CustomerJobAccesses", new[] { "IdJob" });
            DropIndex("dbo.CustomerJobAccesses", new[] { "IdCustomer" });
            DropIndex("dbo.CustomerInvitationStatus", new[] { "CreatedBy" });
            DropIndex("dbo.CustomerInvitationStatus", new[] { "IdJob" });
            DropIndex("dbo.CustomerInvitationStatus", new[] { "IdContact" });
            DropIndex("dbo.ClientNotePlumbingCustomers", new[] { "Idcustomer" });
            DropIndex("dbo.ClientNotePlumbingCustomers", new[] { "IdJobPlumbingInspection" });
            DropIndex("dbo.ClientNoteCustomers", new[] { "Idcustomer" });
            DropIndex("dbo.ClientNoteCustomers", new[] { "IdJobChecklistItemDetail" });
            DropIndex("dbo.Customers", new[] { "LastModifiedByCus" });
            DropIndex("dbo.Customers", new[] { "LastModifiedBy" });
            DropIndex("dbo.Customers", new[] { "CreatedBy" });
            DropIndex("dbo.Customers", new[] { "IdGroup" });
            DropIndex("dbo.Customers", new[] { "IdContcat" });
            DropIndex("dbo.Addresses", new[] { "LastModifiedByCus" });
            DropColumn("dbo.JobContacts", "IshiddenFromCustomer");
            DropColumn("dbo.Contacts", "IsHidden");
            DropColumn("dbo.Addresses", "LastModifiedByCus");
            DropTable("dbo.News");
            DropTable("dbo.CustomerPasswordResets");
            DropTable("dbo.CustomerNotificationSettings");
            DropTable("dbo.CustomerNotifications");
            DropTable("dbo.CustomerJobNames");
            DropTable("dbo.CustomerJobAccesses");
            DropTable("dbo.CustomerInvitationStatus");
            DropTable("dbo.ClientNotePlumbingCustomers");
            DropTable("dbo.ClientNoteCustomers");
            DropTable("dbo.Customers");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
