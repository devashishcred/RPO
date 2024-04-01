namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupInAddPermissionsField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "Permissions", c => c.String());
            AlterStoredProcedure(
                "dbo.Group_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 100),
                        Description = p.String(),
                        Permissions = p.String(),
                        IdGroupPermission = p.Int(),
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
                        ContactsExport = p.Int(),
                        Company = p.Int(),
                        CompanyExport = p.Int(),
                        RFP = p.Int(),
                        Tasks = p.Int(),
                        Reports = p.Int(),
                        ReferenceLinks = p.Int(),
                        ReferenceDocuments = p.Int(),
                        UserGroup = p.Int(),
                        Masters = p.Int(),
                        IsActive = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Groups]([Name], [Description], [Permissions], [IdGroupPermission], [EmployeeEmployeeInfo], [EmployeeContactInfo], [EmployeePersonalInfo], [EmployeeAgentCertificates], [EmployeeSystemAccessInformation], [EmployeeUserGroup], [EmployeeDocuments], [EmployeeStatus], [Jobs], [Contacts], [ContactsExport], [Company], [CompanyExport], [RFP], [Tasks], [Reports], [ReferenceLinks], [ReferenceDocuments], [UserGroup], [Masters], [IsActive])
                      VALUES (@Name, @Description, @Permissions, @IdGroupPermission, @EmployeeEmployeeInfo, @EmployeeContactInfo, @EmployeePersonalInfo, @EmployeeAgentCertificates, @EmployeeSystemAccessInformation, @EmployeeUserGroup, @EmployeeDocuments, @EmployeeStatus, @Jobs, @Contacts, @ContactsExport, @Company, @CompanyExport, @RFP, @Tasks, @Reports, @ReferenceLinks, @ReferenceDocuments, @UserGroup, @Masters, @IsActive)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Groups]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Groups] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Group_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 100),
                        Description = p.String(),
                        Permissions = p.String(),
                        IdGroupPermission = p.Int(),
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
                        ContactsExport = p.Int(),
                        Company = p.Int(),
                        CompanyExport = p.Int(),
                        RFP = p.Int(),
                        Tasks = p.Int(),
                        Reports = p.Int(),
                        ReferenceLinks = p.Int(),
                        ReferenceDocuments = p.Int(),
                        UserGroup = p.Int(),
                        Masters = p.Int(),
                        IsActive = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Groups]
                      SET [Name] = @Name, [Description] = @Description, [Permissions] = @Permissions, [IdGroupPermission] = @IdGroupPermission, [EmployeeEmployeeInfo] = @EmployeeEmployeeInfo, [EmployeeContactInfo] = @EmployeeContactInfo, [EmployeePersonalInfo] = @EmployeePersonalInfo, [EmployeeAgentCertificates] = @EmployeeAgentCertificates, [EmployeeSystemAccessInformation] = @EmployeeSystemAccessInformation, [EmployeeUserGroup] = @EmployeeUserGroup, [EmployeeDocuments] = @EmployeeDocuments, [EmployeeStatus] = @EmployeeStatus, [Jobs] = @Jobs, [Contacts] = @Contacts, [ContactsExport] = @ContactsExport, [Company] = @Company, [CompanyExport] = @CompanyExport, [RFP] = @RFP, [Tasks] = @Tasks, [Reports] = @Reports, [ReferenceLinks] = @ReferenceLinks, [ReferenceDocuments] = @ReferenceDocuments, [UserGroup] = @UserGroup, [Masters] = @Masters, [IsActive] = @IsActive
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "Permissions");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
