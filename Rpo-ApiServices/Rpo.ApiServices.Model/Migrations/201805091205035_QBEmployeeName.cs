namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QBEmployeeName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "QBEmployeeName", c => c.String());
            AlterStoredProcedure(
                "dbo.Employee_Insert",
                p => new
                    {
                        FirstName = p.String(maxLength: 50),
                        LastName = p.String(maxLength: 50),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        City = p.String(),
                        IdState = p.Int(),
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
                        Permissions = p.String(),
                        IsActive = p.Boolean(),
                        LoginPassword = p.String(maxLength: 25),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        EmergencyContactName = p.String(maxLength: 50),
                        EmergencyContactNumber = p.String(maxLength: 25),
                        LockScreenPassword = p.String(maxLength: 25),
                        AppleId = p.String(maxLength: 50),
                        ApplePassword = p.String(maxLength: 25),
                        IsArchive = p.Boolean(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        QBEmployeeName = p.String(),
                    },
                body:
                    @"INSERT [dbo].[Employees]([FirstName], [LastName], [Address1], [Address2], [City], [IdState], [ZipCode], [WorkPhone], [WorkPhoneExt], [MobilePhone], [HomePhone], [Email], [Ssn], [Dob], [StartDate], [FinalDate], [Notes], [TelephonePassword], [ComputerPassword], [EfillingPassword], [EfillingUserName], [IdGroup], [Permissions], [IsActive], [LoginPassword], [CreatedBy], [CreatedDate], [EmergencyContactName], [EmergencyContactNumber], [LockScreenPassword], [AppleId], [ApplePassword], [IsArchive], [LastModifiedBy], [LastModifiedDate], [QBEmployeeName])
                      VALUES (@FirstName, @LastName, @Address1, @Address2, @City, @IdState, @ZipCode, @WorkPhone, @WorkPhoneExt, @MobilePhone, @HomePhone, @Email, @Ssn, @Dob, @StartDate, @FinalDate, @Notes, @TelephonePassword, @ComputerPassword, @EfillingPassword, @EfillingUserName, @IdGroup, @Permissions, @IsActive, @LoginPassword, @CreatedBy, @CreatedDate, @EmergencyContactName, @EmergencyContactNumber, @LockScreenPassword, @AppleId, @ApplePassword, @IsArchive, @LastModifiedBy, @LastModifiedDate, @QBEmployeeName)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Employees]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Employees] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Employee_Update",
                p => new
                    {
                        Id = p.Int(),
                        FirstName = p.String(maxLength: 50),
                        LastName = p.String(maxLength: 50),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        City = p.String(),
                        IdState = p.Int(),
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
                        Permissions = p.String(),
                        IsActive = p.Boolean(),
                        LoginPassword = p.String(maxLength: 25),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        EmergencyContactName = p.String(maxLength: 50),
                        EmergencyContactNumber = p.String(maxLength: 25),
                        LockScreenPassword = p.String(maxLength: 25),
                        AppleId = p.String(maxLength: 50),
                        ApplePassword = p.String(maxLength: 25),
                        IsArchive = p.Boolean(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        QBEmployeeName = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[Employees]
                      SET [FirstName] = @FirstName, [LastName] = @LastName, [Address1] = @Address1, [Address2] = @Address2, [City] = @City, [IdState] = @IdState, [ZipCode] = @ZipCode, [WorkPhone] = @WorkPhone, [WorkPhoneExt] = @WorkPhoneExt, [MobilePhone] = @MobilePhone, [HomePhone] = @HomePhone, [Email] = @Email, [Ssn] = @Ssn, [Dob] = @Dob, [StartDate] = @StartDate, [FinalDate] = @FinalDate, [Notes] = @Notes, [TelephonePassword] = @TelephonePassword, [ComputerPassword] = @ComputerPassword, [EfillingPassword] = @EfillingPassword, [EfillingUserName] = @EfillingUserName, [IdGroup] = @IdGroup, [Permissions] = @Permissions, [IsActive] = @IsActive, [LoginPassword] = @LoginPassword, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [EmergencyContactName] = @EmergencyContactName, [EmergencyContactNumber] = @EmergencyContactNumber, [LockScreenPassword] = @LockScreenPassword, [AppleId] = @AppleId, [ApplePassword] = @ApplePassword, [IsArchive] = @IsArchive, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate, [QBEmployeeName] = @QBEmployeeName
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "QBEmployeeName");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
