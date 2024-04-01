namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prodmigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChecklistAddressProperties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChecklistAddressPropertyMapings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdChecklistAddressProperty = c.Int(),
                        IdChecklistItem = c.Int(),
                        Value = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChecklistAddressProperties", t => t.IdChecklistAddressProperty)
                .ForeignKey("dbo.ChecklistItems", t => t.IdChecklistItem)
                .Index(t => t.IdChecklistAddressProperty)
                .Index(t => t.IdChecklistItem);
            
            CreateTable(
                "dbo.ChecklistItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 200),
                        IdCheckListGroup = c.Int(nullable: false),
                        IdJobApplicationTypes = c.String(),
                        IdJobWorkTypes = c.String(),
                        IsActive = c.Boolean(),
                        IsUserfillable = c.Boolean(),
                        ReferenceNote = c.String(),
                        ExternalReferenceLink = c.String(),
                        InternalReferenceLink = c.String(),
                        IdReferenceDocument = c.String(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                        IdCreateFormDocument = c.Int(),
                        IdUploadFormDocument = c.Int(),
                        DisplayOrderPlumbingInspection = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CheckListGroups", t => t.IdCheckListGroup)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.DocumentMasters", t => t.IdCreateFormDocument)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .ForeignKey("dbo.DocumentMasters", t => t.IdUploadFormDocument)
                .Index(t => t.IdCheckListGroup)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy)
                .Index(t => t.IdCreateFormDocument)
                .Index(t => t.IdUploadFormDocument);
            
            CreateTable(
                "dbo.CheckListGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Type = c.String(maxLength: 50),
                        Displayorder = c.Int(),
                        IsActive = c.Boolean(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            CreateTable(
                "dbo.ChecklistJobViolationComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJobViolation = c.Int(nullable: false),
                        Description = c.String(),
                        CreatedDate = c.DateTime(),
                        LastModifiedDate = c.DateTime(),
                        CreatedBy = c.Int(),
                        LastModifiedBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.JobViolations", t => t.IdJobViolation)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.IdJobViolation)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            CreateTable(
                "dbo.CompositeChecklistDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdCompositeChecklist = c.Int(nullable: false),
                        IdJobChecklistHeader = c.Int(nullable: false),
                        IdJobChecklistGroup = c.Int(nullable: false),
                        IsParentCheckList = c.Boolean(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                        CompositeOrder = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompositeChecklists", t => t.IdCompositeChecklist)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.JobChecklistGroups", t => t.IdJobChecklistGroup)
                .ForeignKey("dbo.JobChecklistHeaders", t => t.IdJobChecklistHeader)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.IdCompositeChecklist)
                .Index(t => t.IdJobChecklistHeader)
                .Index(t => t.IdJobChecklistGroup)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            CreateTable(
                "dbo.CompositeChecklists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ParentJobId = c.Int(nullable: false),
                        IsCOProject = c.Boolean(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            CreateTable(
                "dbo.JobChecklistGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJobCheckListHeader = c.Int(nullable: false),
                        IdCheckListGroup = c.Int(nullable: false),
                        Displayorder1 = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CheckListGroups", t => t.IdCheckListGroup)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.JobChecklistHeaders", t => t.IdJobCheckListHeader)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.IdJobCheckListHeader)
                .Index(t => t.IdCheckListGroup)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            CreateTable(
                "dbo.JobChecklistHeaders",
                c => new
                    {
                        IdJobCheckListHeader = c.Int(nullable: false, identity: true),
                        ChecklistName = c.String(),
                        IdJob = c.Int(nullable: false),
                        IdJobApplication = c.Int(nullable: false),
                        NoOfFloors = c.Int(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.IdJobCheckListHeader)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.JobApplications", t => t.IdJobApplication)
                .ForeignKey("dbo.Jobs", t => t.IdJob)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.IdJob)
                .Index(t => t.IdJobApplication)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            CreateTable(
                "dbo.CompositeViolations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdCompositeChecklist = c.Int(nullable: false),
                        IdJobViolations = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobViolations", t => t.IdJobViolations)
                .Index(t => t.IdJobViolations);
            
            CreateTable(
                "dbo.JobCheckListCommentHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJobChecklistItemDetail = c.Int(nullable: false),
                        Description = c.String(),
                        Isinternal = c.Boolean(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.JobChecklistItemDetails", t => t.IdJobChecklistItemDetail)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.IdJobChecklistItemDetail)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            CreateTable(
                "dbo.JobChecklistItemDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJobChecklistGroup = c.Int(nullable: false),
                        IdChecklistItem = c.Int(nullable: false),
                        Displayorder = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        PartyResponsible1 = c.Int(),
                        PartyResponsible = c.String(),
                        ManualPartyResponsible = c.String(),
                        IdContact = c.Int(),
                        IdDesignApplicant = c.Int(),
                        IdInspector = c.Int(),
                        Stage = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsRequiredForTCO = c.Boolean(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.IdContact)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Contacts", t => t.IdDesignApplicant)
                .ForeignKey("dbo.ChecklistItems", t => t.IdChecklistItem)
                .ForeignKey("dbo.Contacts", t => t.IdInspector)
                .ForeignKey("dbo.JobChecklistGroups", t => t.IdJobChecklistGroup)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.IdJobChecklistGroup)
                .Index(t => t.IdChecklistItem)
                .Index(t => t.IdContact)
                .Index(t => t.IdDesignApplicant)
                .Index(t => t.IdInspector)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            CreateTable(
                "dbo.JobChecklistItemDueDates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJobChecklistItemDetail = c.Int(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.JobChecklistItemDetails", t => t.IdJobChecklistItemDetail)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.IdJobChecklistItemDetail)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            CreateTable(
                "dbo.JobCheckListProgressNoteHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJobChecklistItemDetail = c.Int(nullable: false),
                        Description = c.String(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.JobChecklistItemDetails", t => t.IdJobChecklistItemDetail)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.IdJobChecklistItemDetail)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            CreateTable(
                "dbo.JobPlumbingChecklistFloors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJobCheckListHeader = c.Int(nullable: false),
                        FloonNumber = c.String(),
                        FloorDisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobChecklistHeaders", t => t.IdJobCheckListHeader)
                .Index(t => t.IdJobCheckListHeader);
            
            CreateTable(
                "dbo.InspectionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        JobPlumbingChecklistFloors_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobPlumbingChecklistFloors", t => t.JobPlumbingChecklistFloors_Id)
                .Index(t => t.JobPlumbingChecklistFloors_Id);
            
            CreateTable(
                "dbo.JobPlumbingInspections",
                c => new
                    {
                        IdJobPlumbingInspection = c.Int(nullable: false, identity: true),
                        IdJobChecklistGroup = c.Int(nullable: false),
                        IdChecklistItem = c.Int(nullable: false),
                        DisplayOrder = c.Int(),
                        Status = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsRequiredForTCO = c.Boolean(nullable: false),
                        IdJobPlumbingCheckListFloors = c.Int(nullable: false),
                        WorkOrderNo = c.String(),
                        NextInspection = c.DateTime(),
                        Result = c.String(),
                        IsRequiredTCO_CO = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(),
                        LastModifiedDate = c.DateTime(),
                        CreatedBy = c.Int(),
                        LastModifiedBy = c.Int(),
                        PlumbingInspectionDisplayOrder = c.Int(),
                    })
                .PrimaryKey(t => t.IdJobPlumbingInspection)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.ChecklistItems", t => t.IdChecklistItem)
                .ForeignKey("dbo.JobChecklistGroups", t => t.IdJobChecklistGroup)
                .ForeignKey("dbo.JobPlumbingChecklistFloors", t => t.IdJobPlumbingCheckListFloors)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.IdJobChecklistGroup)
                .Index(t => t.IdChecklistItem)
                .Index(t => t.IdJobPlumbingCheckListFloors)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            CreateTable(
                "dbo.JobPlumbingInspectionComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJobPlumbingInspection = c.Int(nullable: false),
                        Description = c.String(),
                        CreatedDate = c.DateTime(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobPlumbingInspections", t => t.IdJobPlumbingInspection)
                .Index(t => t.IdJobPlumbingInspection);
            
            CreateTable(
                "dbo.JobPlumbingInspectionDueDates",
                c => new
                    {
                        IdJobPlumbingInspectionDueDate = c.Int(nullable: false, identity: true),
                        IdJobPlumbingInspection = c.Int(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.IdJobPlumbingInspectionDueDate)
                .ForeignKey("dbo.JobPlumbingInspections", t => t.IdJobPlumbingInspection)
                .Index(t => t.IdJobPlumbingInspection);
            
            CreateTable(
                "dbo.JobPlumbingInspectionProgressNoteHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJobPlumbingInspection = c.Int(nullable: false),
                        Description = c.String(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.JobPlumbingInspections", t => t.IdJobPlumbingInspection)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.IdJobPlumbingInspection)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            CreateTable(
                "dbo.JobProgressNotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJob = c.Int(nullable: false),
                        Notes = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Jobs", t => t.IdJob)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.IdJob)
                .Index(t => t.LastModifiedBy)
                .Index(t => t.CreatedBy);
            
            CreateTable(
                "dbo.ChecklistItemJobApplicationTypes",
                c => new
                    {
                        ChecklistItem_Id = c.Int(nullable: false),
                        JobApplicationType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ChecklistItem_Id, t.JobApplicationType_Id })
                .ForeignKey("dbo.ChecklistItems", t => t.ChecklistItem_Id, cascadeDelete: true)
                .ForeignKey("dbo.JobApplicationTypes", t => t.JobApplicationType_Id, cascadeDelete: true)
                .Index(t => t.ChecklistItem_Id)
                .Index(t => t.JobApplicationType_Id);
            
            CreateTable(
                "dbo.ChecklistItemJobWorkTypes",
                c => new
                    {
                        ChecklistItem_Id = c.Int(nullable: false),
                        JobWorkType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ChecklistItem_Id, t.JobWorkType_Id })
                .ForeignKey("dbo.ChecklistItems", t => t.ChecklistItem_Id, cascadeDelete: true)
                .ForeignKey("dbo.JobWorkTypes", t => t.JobWorkType_Id, cascadeDelete: true)
                .Index(t => t.ChecklistItem_Id)
                .Index(t => t.JobWorkType_Id);
            
            CreateTable(
                "dbo.ChecklistItemReferenceDocuments",
                c => new
                    {
                        ChecklistItem_Id = c.Int(nullable: false),
                        ReferenceDocument_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ChecklistItem_Id, t.ReferenceDocument_Id })
                .ForeignKey("dbo.ChecklistItems", t => t.ChecklistItem_Id, cascadeDelete: true)
                .ForeignKey("dbo.ReferenceDocuments", t => t.ReferenceDocument_Id, cascadeDelete: true)
                .Index(t => t.ChecklistItem_Id)
                .Index(t => t.ReferenceDocument_Id);
            
            CreateTable(
                "dbo.JobChecklistHeaderJobApplicationWorkPermitTypes",
                c => new
                    {
                        JobChecklistHeader_IdJobCheckListHeader = c.Int(nullable: false),
                        JobApplicationWorkPermitType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.JobChecklistHeader_IdJobCheckListHeader, t.JobApplicationWorkPermitType_Id })
                .ForeignKey("dbo.JobChecklistHeaders", t => t.JobChecklistHeader_IdJobCheckListHeader, cascadeDelete: true)
                .ForeignKey("dbo.JobApplicationWorkPermitTypes", t => t.JobApplicationWorkPermitType_Id, cascadeDelete: true)
                .Index(t => t.JobChecklistHeader_IdJobCheckListHeader)
                .Index(t => t.JobApplicationWorkPermitType_Id);
            
            AddColumn("dbo.JobApplications", "IsExternalApplication", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobApplications", "IsHighRise", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobDocuments", "IdJobchecklistItemDetails", c => c.Int());
            AddColumn("dbo.JobDocuments", "IdJobPlumbingInspections", c => c.Int());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "HasSuperintendentofconstruction", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobApplicationWorkPermitTypes", "HasSiteSafetyCoordinator", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobApplicationWorkPermitTypes", "HasSiteSafetyManager", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobViolations", "BinNumber", c => c.String());
            AddColumn("dbo.JobViolations", "HearingTime", c => c.DateTime());
            AddColumn("dbo.JobViolations", "violation_type_code", c => c.String());
            AddColumn("dbo.JobViolations", "violation_number", c => c.String());
            AddColumn("dbo.JobViolations", "device_number", c => c.String());
            AddColumn("dbo.JobViolations", "description_date", c => c.String());
            AddColumn("dbo.JobViolations", "ECBnumber", c => c.String());
            AddColumn("dbo.JobViolations", "violation_category", c => c.String());
            AddColumn("dbo.JobViolations", "violation_type", c => c.String());
            AddColumn("dbo.JobViolations", "DOB_Description", c => c.String());
            AddColumn("dbo.JobViolations", "Disposition_Date", c => c.DateTime());
            AddColumn("dbo.JobViolations", "Disposition_Comments", c => c.String());
            AddColumn("dbo.JobViolations", "PartyResponsible", c => c.Int());
            AddColumn("dbo.JobViolations", "ManualPartyResponsible", c => c.String());
            AddColumn("dbo.JobViolations", "IdContact", c => c.Int());
            AddColumn("dbo.JobViolations", "ViolationDescription", c => c.String());
            AddColumn("dbo.JobViolations", "TCOToggle", c => c.Boolean());
            AddColumn("dbo.JobViolations", "IsChecklistView", c => c.Boolean());
            AddColumn("dbo.JobViolations", "Type_ECB_DOB", c => c.String());
            AddColumn("dbo.JobViolations", "aggravated_level", c => c.String());
            AddColumn("dbo.JobViolations", "Infraction_Code1", c => c.String());
            AddColumn("dbo.JobViolations", "Section_Law_Description1", c => c.String());
            AddColumn("dbo.JobViolations", "Infraction_Code2", c => c.String());
            AddColumn("dbo.JobViolations", "Section_Law_Description2", c => c.String());
            AddColumn("dbo.JobViolations", "Infraction_Code3", c => c.String());
            AddColumn("dbo.JobViolations", "Section_Law_Description3", c => c.String());
            AddColumn("dbo.JobViolations", "Infraction_Code4", c => c.String());
            AddColumn("dbo.JobViolations", "Section_Law_Description4", c => c.String());
            AddColumn("dbo.JobViolations", "Infraction_Code5", c => c.String());
            AddColumn("dbo.JobViolations", "Section_Law_Description5", c => c.String());
            AddColumn("dbo.JobViolations", "Infraction_Code6", c => c.String());
            AddColumn("dbo.JobViolations", "Section_Law_Description6", c => c.String());
            AddColumn("dbo.JobViolations", "Infraction_Code7", c => c.String());
            AddColumn("dbo.JobViolations", "Section_Law_Description7", c => c.String());
            AddColumn("dbo.JobViolations", "Infraction_Code8", c => c.String());
            AddColumn("dbo.JobViolations", "Section_Law_Description8", c => c.String());
            AddColumn("dbo.JobViolations", "Infraction_Code9", c => c.String());
            AddColumn("dbo.JobViolations", "Section_Law_Description9", c => c.String());
            AddColumn("dbo.JobViolations", "Infraction_Code10", c => c.String());
            AddColumn("dbo.JobViolations", "Section_Law_Description10", c => c.String());
            AddColumn("dbo.JobViolations", "IsManually", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobViolations", "ISNViolation", c => c.String());
            AddColumn("dbo.JobViolations", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.JobViolations", "IsNewMailsent", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobViolations", "IsUpdateMailsent", c => c.Boolean(nullable: false));
            AddColumn("dbo.RfpAddresses", "SRORestrictedCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RfpAddresses", "LoftLawCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RfpAddresses", "EnvironmentalRestrictionsCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RfpAddresses", "CityOwnedCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobTransmittals", "IdChecklistItem", c => c.Int());
            CreateIndex("dbo.JobViolations", "IdContact");
            CreateIndex("dbo.JobTransmittals", "IdChecklistItem");
            AddForeignKey("dbo.JobViolations", "IdContact", "dbo.Contacts", "Id");
            AddForeignKey("dbo.JobTransmittals", "IdChecklistItem", "dbo.ChecklistItems", "Id");
            CreateStoredProcedure(
                "dbo.ChecklistAddressProperty_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 100),
                    },
                body:
                    @"INSERT [dbo].[ChecklistAddressProperties]([Name])
                      VALUES (@Name)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ChecklistAddressProperties]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ChecklistAddressProperties] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.ChecklistAddressProperty_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 100),
                    },
                body:
                    @"UPDATE [dbo].[ChecklistAddressProperties]
                      SET [Name] = @Name
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ChecklistAddressProperty_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ChecklistAddressProperties]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ChecklistAddressPropertyMaping_Insert",
                p => new
                    {
                        IdChecklistAddressProperty = p.Int(),
                        IdChecklistItem = p.Int(),
                        Value = p.String(),
                        IsActive = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[ChecklistAddressPropertyMapings]([IdChecklistAddressProperty], [IdChecklistItem], [Value], [IsActive])
                      VALUES (@IdChecklistAddressProperty, @IdChecklistItem, @Value, @IsActive)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ChecklistAddressPropertyMapings]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ChecklistAddressPropertyMapings] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.ChecklistAddressPropertyMaping_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdChecklistAddressProperty = p.Int(),
                        IdChecklistItem = p.Int(),
                        Value = p.String(),
                        IsActive = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[ChecklistAddressPropertyMapings]
                      SET [IdChecklistAddressProperty] = @IdChecklistAddressProperty, [IdChecklistItem] = @IdChecklistItem, [Value] = @Value, [IsActive] = @IsActive
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ChecklistAddressPropertyMaping_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ChecklistAddressPropertyMapings]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ChecklistItem_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 200),
                        IdCheckListGroup = p.Int(),
                        IdJobApplicationTypes = p.String(),
                        IdJobWorkTypes = p.String(),
                        IsActive = p.Boolean(),
                        IsUserfillable = p.Boolean(),
                        ReferenceNote = p.String(),
                        ExternalReferenceLink = p.String(),
                        InternalReferenceLink = p.String(),
                        IdReferenceDocument = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        IdCreateFormDocument = p.Int(),
                        IdUploadFormDocument = p.Int(),
                        DisplayOrderPlumbingInspection = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[ChecklistItems]([Name], [IdCheckListGroup], [IdJobApplicationTypes], [IdJobWorkTypes], [IsActive], [IsUserfillable], [ReferenceNote], [ExternalReferenceLink], [InternalReferenceLink], [IdReferenceDocument], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate], [IdCreateFormDocument], [IdUploadFormDocument], [DisplayOrderPlumbingInspection])
                      VALUES (@Name, @IdCheckListGroup, @IdJobApplicationTypes, @IdJobWorkTypes, @IsActive, @IsUserfillable, @ReferenceNote, @ExternalReferenceLink, @InternalReferenceLink, @IdReferenceDocument, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate, @IdCreateFormDocument, @IdUploadFormDocument, @DisplayOrderPlumbingInspection)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ChecklistItems]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ChecklistItems] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.ChecklistItem_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 200),
                        IdCheckListGroup = p.Int(),
                        IdJobApplicationTypes = p.String(),
                        IdJobWorkTypes = p.String(),
                        IsActive = p.Boolean(),
                        IsUserfillable = p.Boolean(),
                        ReferenceNote = p.String(),
                        ExternalReferenceLink = p.String(),
                        InternalReferenceLink = p.String(),
                        IdReferenceDocument = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        IdCreateFormDocument = p.Int(),
                        IdUploadFormDocument = p.Int(),
                        DisplayOrderPlumbingInspection = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[ChecklistItems]
                      SET [Name] = @Name, [IdCheckListGroup] = @IdCheckListGroup, [IdJobApplicationTypes] = @IdJobApplicationTypes, [IdJobWorkTypes] = @IdJobWorkTypes, [IsActive] = @IsActive, [IsUserfillable] = @IsUserfillable, [ReferenceNote] = @ReferenceNote, [ExternalReferenceLink] = @ExternalReferenceLink, [InternalReferenceLink] = @InternalReferenceLink, [IdReferenceDocument] = @IdReferenceDocument, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate, [IdCreateFormDocument] = @IdCreateFormDocument, [IdUploadFormDocument] = @IdUploadFormDocument, [DisplayOrderPlumbingInspection] = @DisplayOrderPlumbingInspection
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ChecklistItem_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ChecklistItems]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CheckListGroup_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        Type = p.String(maxLength: 50),
                        Displayorder = p.Int(),
                        IsActive = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[CheckListGroups]([Name], [Type], [Displayorder], [IsActive], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @Type, @Displayorder, @IsActive, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[CheckListGroups]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[CheckListGroups] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.CheckListGroup_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        Type = p.String(maxLength: 50),
                        Displayorder = p.Int(),
                        IsActive = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[CheckListGroups]
                      SET [Name] = @Name, [Type] = @Type, [Displayorder] = @Displayorder, [IsActive] = @IsActive, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CheckListGroup_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[CheckListGroups]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CompositeChecklistDetail_Insert",
                p => new
                    {
                        IdCompositeChecklist = p.Int(),
                        IdJobChecklistHeader = p.Int(),
                        IdJobChecklistGroup = p.Int(),
                        IsParentCheckList = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        CompositeOrder = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[CompositeChecklistDetails]([IdCompositeChecklist], [IdJobChecklistHeader], [IdJobChecklistGroup], [IsParentCheckList], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate], [CompositeOrder])
                      VALUES (@IdCompositeChecklist, @IdJobChecklistHeader, @IdJobChecklistGroup, @IsParentCheckList, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate, @CompositeOrder)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[CompositeChecklistDetails]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[CompositeChecklistDetails] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.CompositeChecklistDetail_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdCompositeChecklist = p.Int(),
                        IdJobChecklistHeader = p.Int(),
                        IdJobChecklistGroup = p.Int(),
                        IsParentCheckList = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        CompositeOrder = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[CompositeChecklistDetails]
                      SET [IdCompositeChecklist] = @IdCompositeChecklist, [IdJobChecklistHeader] = @IdJobChecklistHeader, [IdJobChecklistGroup] = @IdJobChecklistGroup, [IsParentCheckList] = @IsParentCheckList, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate, [CompositeOrder] = @CompositeOrder
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CompositeChecklistDetail_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[CompositeChecklistDetails]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CompositeChecklist_Insert",
                p => new
                    {
                        Name = p.String(),
                        ParentJobId = p.Int(),
                        IsCOProject = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[CompositeChecklists]([Name], [ParentJobId], [IsCOProject], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @ParentJobId, @IsCOProject, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[CompositeChecklists]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[CompositeChecklists] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.CompositeChecklist_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(),
                        ParentJobId = p.Int(),
                        IsCOProject = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[CompositeChecklists]
                      SET [Name] = @Name, [ParentJobId] = @ParentJobId, [IsCOProject] = @IsCOProject, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CompositeChecklist_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[CompositeChecklists]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.JobChecklistGroup_Insert",
                p => new
                    {
                        IdJobCheckListHeader = p.Int(),
                        IdCheckListGroup = p.Int(),
                        Displayorder1 = p.Int(),
                        IsActive = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[JobChecklistGroups]([IdJobCheckListHeader], [IdCheckListGroup], [Displayorder1], [IsActive], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@IdJobCheckListHeader, @IdCheckListGroup, @Displayorder1, @IsActive, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[JobChecklistGroups]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[JobChecklistGroups] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.JobChecklistGroup_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdJobCheckListHeader = p.Int(),
                        IdCheckListGroup = p.Int(),
                        Displayorder1 = p.Int(),
                        IsActive = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[JobChecklistGroups]
                      SET [IdJobCheckListHeader] = @IdJobCheckListHeader, [IdCheckListGroup] = @IdCheckListGroup, [Displayorder1] = @Displayorder1, [IsActive] = @IsActive, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.JobChecklistGroup_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[JobChecklistGroups]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.JobChecklistHeader_Insert",
                p => new
                    {
                        ChecklistName = p.String(),
                        IdJob = p.Int(),
                        IdJobApplication = p.Int(),
                        NoOfFloors = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[JobChecklistHeaders]([ChecklistName], [IdJob], [IdJobApplication], [NoOfFloors], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@ChecklistName, @IdJob, @IdJobApplication, @NoOfFloors, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @IdJobCheckListHeader int
                      SELECT @IdJobCheckListHeader = [IdJobCheckListHeader]
                      FROM [dbo].[JobChecklistHeaders]
                      WHERE @@ROWCOUNT > 0 AND [IdJobCheckListHeader] = scope_identity()
                      
                      SELECT t0.[IdJobCheckListHeader]
                      FROM [dbo].[JobChecklistHeaders] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[IdJobCheckListHeader] = @IdJobCheckListHeader"
            );
            
            CreateStoredProcedure(
                "dbo.JobChecklistHeader_Update",
                p => new
                    {
                        IdJobCheckListHeader = p.Int(),
                        ChecklistName = p.String(),
                        IdJob = p.Int(),
                        IdJobApplication = p.Int(),
                        NoOfFloors = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[JobChecklistHeaders]
                      SET [ChecklistName] = @ChecklistName, [IdJob] = @IdJob, [IdJobApplication] = @IdJobApplication, [NoOfFloors] = @NoOfFloors, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([IdJobCheckListHeader] = @IdJobCheckListHeader)"
            );
            
            CreateStoredProcedure(
                "dbo.JobChecklistHeader_Delete",
                p => new
                    {
                        IdJobCheckListHeader = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[JobChecklistHeaders]
                      WHERE ([IdJobCheckListHeader] = @IdJobCheckListHeader)"
            );
            
            CreateStoredProcedure(
                "dbo.JobCheckListCommentHistory_Insert",
                p => new
                    {
                        IdJobChecklistItemDetail = p.Int(),
                        Description = p.String(),
                        Isinternal = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[JobCheckListCommentHistories]([IdJobChecklistItemDetail], [Description], [Isinternal], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@IdJobChecklistItemDetail, @Description, @Isinternal, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[JobCheckListCommentHistories]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[JobCheckListCommentHistories] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.JobCheckListCommentHistory_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdJobChecklistItemDetail = p.Int(),
                        Description = p.String(),
                        Isinternal = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[JobCheckListCommentHistories]
                      SET [IdJobChecklistItemDetail] = @IdJobChecklistItemDetail, [Description] = @Description, [Isinternal] = @Isinternal, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.JobCheckListCommentHistory_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[JobCheckListCommentHistories]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.JobChecklistItemDetail_Insert",
                p => new
                    {
                        IdJobChecklistGroup = p.Int(),
                        IdChecklistItem = p.Int(),
                        Displayorder = p.Int(),
                        Status = p.Int(),
                        PartyResponsible1 = p.Int(),
                        PartyResponsible = p.String(),
                        ManualPartyResponsible = p.String(),
                        IdContact = p.Int(),
                        IdDesignApplicant = p.Int(),
                        IdInspector = p.Int(),
                        Stage = p.String(),
                        IsActive = p.Boolean(),
                        IsRequiredForTCO = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[JobChecklistItemDetails]([IdJobChecklistGroup], [IdChecklistItem], [Displayorder], [Status], [PartyResponsible1], [PartyResponsible], [ManualPartyResponsible], [IdContact], [IdDesignApplicant], [IdInspector], [Stage], [IsActive], [IsRequiredForTCO], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@IdJobChecklistGroup, @IdChecklistItem, @Displayorder, @Status, @PartyResponsible1, @PartyResponsible, @ManualPartyResponsible, @IdContact, @IdDesignApplicant, @IdInspector, @Stage, @IsActive, @IsRequiredForTCO, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[JobChecklistItemDetails]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[JobChecklistItemDetails] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.JobChecklistItemDetail_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdJobChecklistGroup = p.Int(),
                        IdChecklistItem = p.Int(),
                        Displayorder = p.Int(),
                        Status = p.Int(),
                        PartyResponsible1 = p.Int(),
                        PartyResponsible = p.String(),
                        ManualPartyResponsible = p.String(),
                        IdContact = p.Int(),
                        IdDesignApplicant = p.Int(),
                        IdInspector = p.Int(),
                        Stage = p.String(),
                        IsActive = p.Boolean(),
                        IsRequiredForTCO = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[JobChecklistItemDetails]
                      SET [IdJobChecklistGroup] = @IdJobChecklistGroup, [IdChecklistItem] = @IdChecklistItem, [Displayorder] = @Displayorder, [Status] = @Status, [PartyResponsible1] = @PartyResponsible1, [PartyResponsible] = @PartyResponsible, [ManualPartyResponsible] = @ManualPartyResponsible, [IdContact] = @IdContact, [IdDesignApplicant] = @IdDesignApplicant, [IdInspector] = @IdInspector, [Stage] = @Stage, [IsActive] = @IsActive, [IsRequiredForTCO] = @IsRequiredForTCO, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.JobChecklistItemDetail_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[JobChecklistItemDetails]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.JobChecklistItemDueDate_Insert",
                p => new
                    {
                        IdJobChecklistItemDetail = p.Int(),
                        DueDate = p.DateTime(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[JobChecklistItemDueDates]([IdJobChecklistItemDetail], [DueDate], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@IdJobChecklistItemDetail, @DueDate, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[JobChecklistItemDueDates]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[JobChecklistItemDueDates] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.JobChecklistItemDueDate_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdJobChecklistItemDetail = p.Int(),
                        DueDate = p.DateTime(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[JobChecklistItemDueDates]
                      SET [IdJobChecklistItemDetail] = @IdJobChecklistItemDetail, [DueDate] = @DueDate, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.JobChecklistItemDueDate_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[JobChecklistItemDueDates]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.JobCheckListProgressNoteHistory_Insert",
                p => new
                    {
                        IdJobChecklistItemDetail = p.Int(),
                        Description = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[JobCheckListProgressNoteHistories]([IdJobChecklistItemDetail], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@IdJobChecklistItemDetail, @Description, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[JobCheckListProgressNoteHistories]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[JobCheckListProgressNoteHistories] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.JobCheckListProgressNoteHistory_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdJobChecklistItemDetail = p.Int(),
                        Description = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[JobCheckListProgressNoteHistories]
                      SET [IdJobChecklistItemDetail] = @IdJobChecklistItemDetail, [Description] = @Description, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.JobCheckListProgressNoteHistory_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[JobCheckListProgressNoteHistories]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.JobPlumbingChecklistFloors_Insert",
                p => new
                    {
                        IdJobCheckListHeader = p.Int(),
                        FloonNumber = p.String(),
                        FloorDisplayOrder = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[JobPlumbingChecklistFloors]([IdJobCheckListHeader], [FloonNumber], [FloorDisplayOrder])
                      VALUES (@IdJobCheckListHeader, @FloonNumber, @FloorDisplayOrder)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[JobPlumbingChecklistFloors]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[JobPlumbingChecklistFloors] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.JobPlumbingChecklistFloors_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdJobCheckListHeader = p.Int(),
                        FloonNumber = p.String(),
                        FloorDisplayOrder = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[JobPlumbingChecklistFloors]
                      SET [IdJobCheckListHeader] = @IdJobCheckListHeader, [FloonNumber] = @FloonNumber, [FloorDisplayOrder] = @FloorDisplayOrder
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.JobPlumbingChecklistFloors_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[JobPlumbingChecklistFloors]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.JobPlumbingInspection_Insert",
                p => new
                    {
                        IdJobChecklistGroup = p.Int(),
                        IdChecklistItem = p.Int(),
                        DisplayOrder = p.Int(),
                        Status = p.Int(),
                        IsActive = p.Boolean(),
                        IsRequiredForTCO = p.Boolean(),
                        IdJobPlumbingCheckListFloors = p.Int(),
                        WorkOrderNo = p.String(),
                        NextInspection = p.DateTime(),
                        Result = p.String(),
                        IsRequiredTCO_CO = p.Boolean(),
                        CreatedDate = p.DateTime(),
                        LastModifiedDate = p.DateTime(),
                        CreatedBy = p.Int(),
                        LastModifiedBy = p.Int(),
                        PlumbingInspectionDisplayOrder = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[JobPlumbingInspections]([IdJobChecklistGroup], [IdChecklistItem], [DisplayOrder], [Status], [IsActive], [IsRequiredForTCO], [IdJobPlumbingCheckListFloors], [WorkOrderNo], [NextInspection], [Result], [IsRequiredTCO_CO], [CreatedDate], [LastModifiedDate], [CreatedBy], [LastModifiedBy], [PlumbingInspectionDisplayOrder])
                      VALUES (@IdJobChecklistGroup, @IdChecklistItem, @DisplayOrder, @Status, @IsActive, @IsRequiredForTCO, @IdJobPlumbingCheckListFloors, @WorkOrderNo, @NextInspection, @Result, @IsRequiredTCO_CO, @CreatedDate, @LastModifiedDate, @CreatedBy, @LastModifiedBy, @PlumbingInspectionDisplayOrder)
                      
                      DECLARE @IdJobPlumbingInspection int
                      SELECT @IdJobPlumbingInspection = [IdJobPlumbingInspection]
                      FROM [dbo].[JobPlumbingInspections]
                      WHERE @@ROWCOUNT > 0 AND [IdJobPlumbingInspection] = scope_identity()
                      
                      SELECT t0.[IdJobPlumbingInspection]
                      FROM [dbo].[JobPlumbingInspections] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[IdJobPlumbingInspection] = @IdJobPlumbingInspection"
            );
            
            CreateStoredProcedure(
                "dbo.JobPlumbingInspection_Update",
                p => new
                    {
                        IdJobPlumbingInspection = p.Int(),
                        IdJobChecklistGroup = p.Int(),
                        IdChecklistItem = p.Int(),
                        DisplayOrder = p.Int(),
                        Status = p.Int(),
                        IsActive = p.Boolean(),
                        IsRequiredForTCO = p.Boolean(),
                        IdJobPlumbingCheckListFloors = p.Int(),
                        WorkOrderNo = p.String(),
                        NextInspection = p.DateTime(),
                        Result = p.String(),
                        IsRequiredTCO_CO = p.Boolean(),
                        CreatedDate = p.DateTime(),
                        LastModifiedDate = p.DateTime(),
                        CreatedBy = p.Int(),
                        LastModifiedBy = p.Int(),
                        PlumbingInspectionDisplayOrder = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[JobPlumbingInspections]
                      SET [IdJobChecklistGroup] = @IdJobChecklistGroup, [IdChecklistItem] = @IdChecklistItem, [DisplayOrder] = @DisplayOrder, [Status] = @Status, [IsActive] = @IsActive, [IsRequiredForTCO] = @IsRequiredForTCO, [IdJobPlumbingCheckListFloors] = @IdJobPlumbingCheckListFloors, [WorkOrderNo] = @WorkOrderNo, [NextInspection] = @NextInspection, [Result] = @Result, [IsRequiredTCO_CO] = @IsRequiredTCO_CO, [CreatedDate] = @CreatedDate, [LastModifiedDate] = @LastModifiedDate, [CreatedBy] = @CreatedBy, [LastModifiedBy] = @LastModifiedBy, [PlumbingInspectionDisplayOrder] = @PlumbingInspectionDisplayOrder
                      WHERE ([IdJobPlumbingInspection] = @IdJobPlumbingInspection)"
            );
            
            CreateStoredProcedure(
                "dbo.JobPlumbingInspection_Delete",
                p => new
                    {
                        IdJobPlumbingInspection = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[JobPlumbingInspections]
                      WHERE ([IdJobPlumbingInspection] = @IdJobPlumbingInspection)"
            );
            
            CreateStoredProcedure(
                "dbo.JobPlumbingInspectionComment_Insert",
                p => new
                    {
                        IdJobPlumbingInspection = p.Int(),
                        Description = p.String(),
                        CreatedDate = p.DateTime(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[JobPlumbingInspectionComments]([IdJobPlumbingInspection], [Description], [CreatedDate], [LastModifiedDate])
                      VALUES (@IdJobPlumbingInspection, @Description, @CreatedDate, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[JobPlumbingInspectionComments]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[JobPlumbingInspectionComments] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.JobPlumbingInspectionComment_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdJobPlumbingInspection = p.Int(),
                        Description = p.String(),
                        CreatedDate = p.DateTime(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[JobPlumbingInspectionComments]
                      SET [IdJobPlumbingInspection] = @IdJobPlumbingInspection, [Description] = @Description, [CreatedDate] = @CreatedDate, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.JobPlumbingInspectionComment_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[JobPlumbingInspectionComments]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.JobPlumbingInspectionDueDate_Insert",
                p => new
                    {
                        IdJobPlumbingInspection = p.Int(),
                        DueDate = p.DateTime(),
                        CreatedDate = p.DateTime(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[JobPlumbingInspectionDueDates]([IdJobPlumbingInspection], [DueDate], [CreatedDate], [LastModifiedDate])
                      VALUES (@IdJobPlumbingInspection, @DueDate, @CreatedDate, @LastModifiedDate)
                      
                      DECLARE @IdJobPlumbingInspectionDueDate int
                      SELECT @IdJobPlumbingInspectionDueDate = [IdJobPlumbingInspectionDueDate]
                      FROM [dbo].[JobPlumbingInspectionDueDates]
                      WHERE @@ROWCOUNT > 0 AND [IdJobPlumbingInspectionDueDate] = scope_identity()
                      
                      SELECT t0.[IdJobPlumbingInspectionDueDate]
                      FROM [dbo].[JobPlumbingInspectionDueDates] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[IdJobPlumbingInspectionDueDate] = @IdJobPlumbingInspectionDueDate"
            );
            
            CreateStoredProcedure(
                "dbo.JobPlumbingInspectionDueDate_Update",
                p => new
                    {
                        IdJobPlumbingInspectionDueDate = p.Int(),
                        IdJobPlumbingInspection = p.Int(),
                        DueDate = p.DateTime(),
                        CreatedDate = p.DateTime(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[JobPlumbingInspectionDueDates]
                      SET [IdJobPlumbingInspection] = @IdJobPlumbingInspection, [DueDate] = @DueDate, [CreatedDate] = @CreatedDate, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([IdJobPlumbingInspectionDueDate] = @IdJobPlumbingInspectionDueDate)"
            );
            
            CreateStoredProcedure(
                "dbo.JobPlumbingInspectionDueDate_Delete",
                p => new
                    {
                        IdJobPlumbingInspectionDueDate = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[JobPlumbingInspectionDueDates]
                      WHERE ([IdJobPlumbingInspectionDueDate] = @IdJobPlumbingInspectionDueDate)"
            );
            
            CreateStoredProcedure(
                "dbo.JobPlumbingInspectionProgressNoteHistory_Insert",
                p => new
                    {
                        IdJobPlumbingInspection = p.Int(),
                        Description = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[JobPlumbingInspectionProgressNoteHistories]([IdJobPlumbingInspection], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@IdJobPlumbingInspection, @Description, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[JobPlumbingInspectionProgressNoteHistories]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[JobPlumbingInspectionProgressNoteHistories] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.JobPlumbingInspectionProgressNoteHistory_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdJobPlumbingInspection = p.Int(),
                        Description = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[JobPlumbingInspectionProgressNoteHistories]
                      SET [IdJobPlumbingInspection] = @IdJobPlumbingInspection, [Description] = @Description, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.JobPlumbingInspectionProgressNoteHistory_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[JobPlumbingInspectionProgressNoteHistories]
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.RfpAddress_Insert",
                p => new
                    {
                        IdBorough = p.Int(),
                        HouseNumber = p.String(maxLength: 10),
                        Street = p.String(maxLength: 50),
                        ZipCode = p.String(maxLength: 5),
                        Block = p.String(maxLength: 50),
                        Lot = p.String(maxLength: 50),
                        BinNumber = p.String(maxLength: 50),
                        ComunityBoardNumber = p.String(maxLength: 50),
                        ZoneDistrict = p.String(maxLength: 50),
                        Overlay = p.String(maxLength: 50),
                        SpecialDistrict = p.String(),
                        Map = p.String(maxLength: 50),
                        IdOwnerType = p.Int(),
                        IdCompany = p.Int(),
                        NonProfit = p.Boolean(),
                        IdOwnerContact = p.Int(),
                        IdSecondOfficerCompany = p.Int(),
                        IdSecondOfficer = p.Int(),
                        Title = p.String(maxLength: 50),
                        IdOccupancyClassification = p.Int(),
                        IsOcupancyClassification20082014 = p.Boolean(),
                        IdConstructionClassification = p.Int(),
                        IsConstructionClassification20082014 = p.Boolean(),
                        IdMultipleDwellingClassification = p.Int(),
                        IdPrimaryStructuralSystem = p.Int(),
                        IdStructureOccupancyCategory = p.Int(),
                        IdSeismicDesignCategory = p.Int(),
                        Stories = p.Int(),
                        Height = p.Int(),
                        Feet = p.Int(),
                        DwellingUnits = p.Int(),
                        GrossArea = p.String(),
                        StreetLegalWidth = p.Int(),
                        IsLandmark = p.Boolean(),
                        IsLittleE = p.Boolean(),
                        TidalWetlandsMapCheck = p.Boolean(),
                        FreshwaterWetlandsMapCheck = p.Boolean(),
                        CoastalErosionHazardAreaMapCheck = p.Boolean(),
                        SpecialFloodHazardAreaCheck = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        OutsideNYC = p.String(maxLength: 250),
                        IsBSADecision = p.Boolean(),
                        SRORestrictedCheck = p.Boolean(),
                        LoftLawCheck = p.Boolean(),
                        EnvironmentalRestrictionsCheck = p.Boolean(),
                        CityOwnedCheck = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[RfpAddresses]([IdBorough], [HouseNumber], [Street], [ZipCode], [Block], [Lot], [BinNumber], [ComunityBoardNumber], [ZoneDistrict], [Overlay], [SpecialDistrict], [Map], [IdOwnerType], [IdCompany], [NonProfit], [IdOwnerContact], [IdSecondOfficerCompany], [IdSecondOfficer], [Title], [IdOccupancyClassification], [IsOcupancyClassification20082014], [IdConstructionClassification], [IsConstructionClassification20082014], [IdMultipleDwellingClassification], [IdPrimaryStructuralSystem], [IdStructureOccupancyCategory], [IdSeismicDesignCategory], [Stories], [Height], [Feet], [DwellingUnits], [GrossArea], [StreetLegalWidth], [IsLandmark], [IsLittleE], [TidalWetlandsMapCheck], [FreshwaterWetlandsMapCheck], [CoastalErosionHazardAreaMapCheck], [SpecialFloodHazardAreaCheck], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate], [OutsideNYC], [IsBSADecision], [SRORestrictedCheck], [LoftLawCheck], [EnvironmentalRestrictionsCheck], [CityOwnedCheck])
                      VALUES (@IdBorough, @HouseNumber, @Street, @ZipCode, @Block, @Lot, @BinNumber, @ComunityBoardNumber, @ZoneDistrict, @Overlay, @SpecialDistrict, @Map, @IdOwnerType, @IdCompany, @NonProfit, @IdOwnerContact, @IdSecondOfficerCompany, @IdSecondOfficer, @Title, @IdOccupancyClassification, @IsOcupancyClassification20082014, @IdConstructionClassification, @IsConstructionClassification20082014, @IdMultipleDwellingClassification, @IdPrimaryStructuralSystem, @IdStructureOccupancyCategory, @IdSeismicDesignCategory, @Stories, @Height, @Feet, @DwellingUnits, @GrossArea, @StreetLegalWidth, @IsLandmark, @IsLittleE, @TidalWetlandsMapCheck, @FreshwaterWetlandsMapCheck, @CoastalErosionHazardAreaMapCheck, @SpecialFloodHazardAreaCheck, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate, @OutsideNYC, @IsBSADecision, @SRORestrictedCheck, @LoftLawCheck, @EnvironmentalRestrictionsCheck, @CityOwnedCheck)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[RfpAddresses]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[RfpAddresses] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.RfpAddress_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdBorough = p.Int(),
                        HouseNumber = p.String(maxLength: 10),
                        Street = p.String(maxLength: 50),
                        ZipCode = p.String(maxLength: 5),
                        Block = p.String(maxLength: 50),
                        Lot = p.String(maxLength: 50),
                        BinNumber = p.String(maxLength: 50),
                        ComunityBoardNumber = p.String(maxLength: 50),
                        ZoneDistrict = p.String(maxLength: 50),
                        Overlay = p.String(maxLength: 50),
                        SpecialDistrict = p.String(),
                        Map = p.String(maxLength: 50),
                        IdOwnerType = p.Int(),
                        IdCompany = p.Int(),
                        NonProfit = p.Boolean(),
                        IdOwnerContact = p.Int(),
                        IdSecondOfficerCompany = p.Int(),
                        IdSecondOfficer = p.Int(),
                        Title = p.String(maxLength: 50),
                        IdOccupancyClassification = p.Int(),
                        IsOcupancyClassification20082014 = p.Boolean(),
                        IdConstructionClassification = p.Int(),
                        IsConstructionClassification20082014 = p.Boolean(),
                        IdMultipleDwellingClassification = p.Int(),
                        IdPrimaryStructuralSystem = p.Int(),
                        IdStructureOccupancyCategory = p.Int(),
                        IdSeismicDesignCategory = p.Int(),
                        Stories = p.Int(),
                        Height = p.Int(),
                        Feet = p.Int(),
                        DwellingUnits = p.Int(),
                        GrossArea = p.String(),
                        StreetLegalWidth = p.Int(),
                        IsLandmark = p.Boolean(),
                        IsLittleE = p.Boolean(),
                        TidalWetlandsMapCheck = p.Boolean(),
                        FreshwaterWetlandsMapCheck = p.Boolean(),
                        CoastalErosionHazardAreaMapCheck = p.Boolean(),
                        SpecialFloodHazardAreaCheck = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        OutsideNYC = p.String(maxLength: 250),
                        IsBSADecision = p.Boolean(),
                        SRORestrictedCheck = p.Boolean(),
                        LoftLawCheck = p.Boolean(),
                        EnvironmentalRestrictionsCheck = p.Boolean(),
                        CityOwnedCheck = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[RfpAddresses]
                      SET [IdBorough] = @IdBorough, [HouseNumber] = @HouseNumber, [Street] = @Street, [ZipCode] = @ZipCode, [Block] = @Block, [Lot] = @Lot, [BinNumber] = @BinNumber, [ComunityBoardNumber] = @ComunityBoardNumber, [ZoneDistrict] = @ZoneDistrict, [Overlay] = @Overlay, [SpecialDistrict] = @SpecialDistrict, [Map] = @Map, [IdOwnerType] = @IdOwnerType, [IdCompany] = @IdCompany, [NonProfit] = @NonProfit, [IdOwnerContact] = @IdOwnerContact, [IdSecondOfficerCompany] = @IdSecondOfficerCompany, [IdSecondOfficer] = @IdSecondOfficer, [Title] = @Title, [IdOccupancyClassification] = @IdOccupancyClassification, [IsOcupancyClassification20082014] = @IsOcupancyClassification20082014, [IdConstructionClassification] = @IdConstructionClassification, [IsConstructionClassification20082014] = @IsConstructionClassification20082014, [IdMultipleDwellingClassification] = @IdMultipleDwellingClassification, [IdPrimaryStructuralSystem] = @IdPrimaryStructuralSystem, [IdStructureOccupancyCategory] = @IdStructureOccupancyCategory, [IdSeismicDesignCategory] = @IdSeismicDesignCategory, [Stories] = @Stories, [Height] = @Height, [Feet] = @Feet, [DwellingUnits] = @DwellingUnits, [GrossArea] = @GrossArea, [StreetLegalWidth] = @StreetLegalWidth, [IsLandmark] = @IsLandmark, [IsLittleE] = @IsLittleE, [TidalWetlandsMapCheck] = @TidalWetlandsMapCheck, [FreshwaterWetlandsMapCheck] = @FreshwaterWetlandsMapCheck, [CoastalErosionHazardAreaMapCheck] = @CoastalErosionHazardAreaMapCheck, [SpecialFloodHazardAreaCheck] = @SpecialFloodHazardAreaCheck, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate, [OutsideNYC] = @OutsideNYC, [IsBSADecision] = @IsBSADecision, [SRORestrictedCheck] = @SRORestrictedCheck, [LoftLawCheck] = @LoftLawCheck, [EnvironmentalRestrictionsCheck] = @EnvironmentalRestrictionsCheck, [CityOwnedCheck] = @CityOwnedCheck
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.JobPlumbingInspectionProgressNoteHistory_Delete");
            DropStoredProcedure("dbo.JobPlumbingInspectionProgressNoteHistory_Update");
            DropStoredProcedure("dbo.JobPlumbingInspectionProgressNoteHistory_Insert");
            DropStoredProcedure("dbo.JobPlumbingInspectionDueDate_Delete");
            DropStoredProcedure("dbo.JobPlumbingInspectionDueDate_Update");
            DropStoredProcedure("dbo.JobPlumbingInspectionDueDate_Insert");
            DropStoredProcedure("dbo.JobPlumbingInspectionComment_Delete");
            DropStoredProcedure("dbo.JobPlumbingInspectionComment_Update");
            DropStoredProcedure("dbo.JobPlumbingInspectionComment_Insert");
            DropStoredProcedure("dbo.JobPlumbingInspection_Delete");
            DropStoredProcedure("dbo.JobPlumbingInspection_Update");
            DropStoredProcedure("dbo.JobPlumbingInspection_Insert");
            DropStoredProcedure("dbo.JobPlumbingChecklistFloors_Delete");
            DropStoredProcedure("dbo.JobPlumbingChecklistFloors_Update");
            DropStoredProcedure("dbo.JobPlumbingChecklistFloors_Insert");
            DropStoredProcedure("dbo.JobCheckListProgressNoteHistory_Delete");
            DropStoredProcedure("dbo.JobCheckListProgressNoteHistory_Update");
            DropStoredProcedure("dbo.JobCheckListProgressNoteHistory_Insert");
            DropStoredProcedure("dbo.JobChecklistItemDueDate_Delete");
            DropStoredProcedure("dbo.JobChecklistItemDueDate_Update");
            DropStoredProcedure("dbo.JobChecklistItemDueDate_Insert");
            DropStoredProcedure("dbo.JobChecklistItemDetail_Delete");
            DropStoredProcedure("dbo.JobChecklistItemDetail_Update");
            DropStoredProcedure("dbo.JobChecklistItemDetail_Insert");
            DropStoredProcedure("dbo.JobCheckListCommentHistory_Delete");
            DropStoredProcedure("dbo.JobCheckListCommentHistory_Update");
            DropStoredProcedure("dbo.JobCheckListCommentHistory_Insert");
            DropStoredProcedure("dbo.JobChecklistHeader_Delete");
            DropStoredProcedure("dbo.JobChecklistHeader_Update");
            DropStoredProcedure("dbo.JobChecklistHeader_Insert");
            DropStoredProcedure("dbo.JobChecklistGroup_Delete");
            DropStoredProcedure("dbo.JobChecklistGroup_Update");
            DropStoredProcedure("dbo.JobChecklistGroup_Insert");
            DropStoredProcedure("dbo.CompositeChecklist_Delete");
            DropStoredProcedure("dbo.CompositeChecklist_Update");
            DropStoredProcedure("dbo.CompositeChecklist_Insert");
            DropStoredProcedure("dbo.CompositeChecklistDetail_Delete");
            DropStoredProcedure("dbo.CompositeChecklistDetail_Update");
            DropStoredProcedure("dbo.CompositeChecklistDetail_Insert");
            DropStoredProcedure("dbo.CheckListGroup_Delete");
            DropStoredProcedure("dbo.CheckListGroup_Update");
            DropStoredProcedure("dbo.CheckListGroup_Insert");
            DropStoredProcedure("dbo.ChecklistItem_Delete");
            DropStoredProcedure("dbo.ChecklistItem_Update");
            DropStoredProcedure("dbo.ChecklistItem_Insert");
            DropStoredProcedure("dbo.ChecklistAddressPropertyMaping_Delete");
            DropStoredProcedure("dbo.ChecklistAddressPropertyMaping_Update");
            DropStoredProcedure("dbo.ChecklistAddressPropertyMaping_Insert");
            DropStoredProcedure("dbo.ChecklistAddressProperty_Delete");
            DropStoredProcedure("dbo.ChecklistAddressProperty_Update");
            DropStoredProcedure("dbo.ChecklistAddressProperty_Insert");
            DropForeignKey("dbo.JobProgressNotes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobProgressNotes", "IdJob", "dbo.Jobs");
            DropForeignKey("dbo.JobProgressNotes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.JobPlumbingInspectionProgressNoteHistories", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobPlumbingInspectionProgressNoteHistories", "IdJobPlumbingInspection", "dbo.JobPlumbingInspections");
            DropForeignKey("dbo.JobPlumbingInspectionProgressNoteHistories", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.JobPlumbingInspectionDueDates", "IdJobPlumbingInspection", "dbo.JobPlumbingInspections");
            DropForeignKey("dbo.JobPlumbingInspectionComments", "IdJobPlumbingInspection", "dbo.JobPlumbingInspections");
            DropForeignKey("dbo.JobPlumbingInspections", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobPlumbingInspections", "IdJobPlumbingCheckListFloors", "dbo.JobPlumbingChecklistFloors");
            DropForeignKey("dbo.JobPlumbingInspections", "IdJobChecklistGroup", "dbo.JobChecklistGroups");
            DropForeignKey("dbo.JobPlumbingInspections", "IdChecklistItem", "dbo.ChecklistItems");
            DropForeignKey("dbo.JobPlumbingInspections", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.JobPlumbingChecklistFloors", "IdJobCheckListHeader", "dbo.JobChecklistHeaders");
            DropForeignKey("dbo.InspectionTypes", "JobPlumbingChecklistFloors_Id", "dbo.JobPlumbingChecklistFloors");
            DropForeignKey("dbo.JobCheckListProgressNoteHistories", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobCheckListProgressNoteHistories", "IdJobChecklistItemDetail", "dbo.JobChecklistItemDetails");
            DropForeignKey("dbo.JobCheckListProgressNoteHistories", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.JobChecklistItemDueDates", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobChecklistItemDueDates", "IdJobChecklistItemDetail", "dbo.JobChecklistItemDetails");
            DropForeignKey("dbo.JobChecklistItemDueDates", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.JobCheckListCommentHistories", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobCheckListCommentHistories", "IdJobChecklistItemDetail", "dbo.JobChecklistItemDetails");
            DropForeignKey("dbo.JobChecklistItemDetails", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobChecklistItemDetails", "IdJobChecklistGroup", "dbo.JobChecklistGroups");
            DropForeignKey("dbo.JobChecklistItemDetails", "IdInspector", "dbo.Contacts");
            DropForeignKey("dbo.JobChecklistItemDetails", "IdChecklistItem", "dbo.ChecklistItems");
            DropForeignKey("dbo.JobChecklistItemDetails", "IdDesignApplicant", "dbo.Contacts");
            DropForeignKey("dbo.JobChecklistItemDetails", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.JobChecklistItemDetails", "IdContact", "dbo.Contacts");
            DropForeignKey("dbo.JobCheckListCommentHistories", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.CompositeViolations", "IdJobViolations", "dbo.JobViolations");
            DropForeignKey("dbo.CompositeChecklistDetails", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.CompositeChecklistDetails", "IdJobChecklistHeader", "dbo.JobChecklistHeaders");
            DropForeignKey("dbo.CompositeChecklistDetails", "IdJobChecklistGroup", "dbo.JobChecklistGroups");
            DropForeignKey("dbo.JobChecklistGroups", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobChecklistGroups", "IdJobCheckListHeader", "dbo.JobChecklistHeaders");
            DropForeignKey("dbo.JobChecklistHeaders", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobChecklistHeaders", "IdJob", "dbo.Jobs");
            DropForeignKey("dbo.JobChecklistHeaderJobApplicationWorkPermitTypes", "JobApplicationWorkPermitType_Id", "dbo.JobApplicationWorkPermitTypes");
            DropForeignKey("dbo.JobChecklistHeaderJobApplicationWorkPermitTypes", "JobChecklistHeader_IdJobCheckListHeader", "dbo.JobChecklistHeaders");
            DropForeignKey("dbo.JobChecklistHeaders", "IdJobApplication", "dbo.JobApplications");
            DropForeignKey("dbo.JobChecklistHeaders", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.JobChecklistGroups", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.JobChecklistGroups", "IdCheckListGroup", "dbo.CheckListGroups");
            DropForeignKey("dbo.CompositeChecklistDetails", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.CompositeChecklistDetails", "IdCompositeChecklist", "dbo.CompositeChecklists");
            DropForeignKey("dbo.CompositeChecklists", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.CompositeChecklists", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.ChecklistJobViolationComments", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.ChecklistJobViolationComments", "IdJobViolation", "dbo.JobViolations");
            DropForeignKey("dbo.JobTransmittals", "IdChecklistItem", "dbo.ChecklistItems");
            DropForeignKey("dbo.JobViolations", "IdContact", "dbo.Contacts");
            DropForeignKey("dbo.ChecklistJobViolationComments", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.ChecklistAddressPropertyMapings", "IdChecklistItem", "dbo.ChecklistItems");
            DropForeignKey("dbo.ChecklistItems", "IdUploadFormDocument", "dbo.DocumentMasters");
            DropForeignKey("dbo.ChecklistItemReferenceDocuments", "ReferenceDocument_Id", "dbo.ReferenceDocuments");
            DropForeignKey("dbo.ChecklistItemReferenceDocuments", "ChecklistItem_Id", "dbo.ChecklistItems");
            DropForeignKey("dbo.ChecklistItems", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.ChecklistItemJobWorkTypes", "JobWorkType_Id", "dbo.JobWorkTypes");
            DropForeignKey("dbo.ChecklistItemJobWorkTypes", "ChecklistItem_Id", "dbo.ChecklistItems");
            DropForeignKey("dbo.ChecklistItemJobApplicationTypes", "JobApplicationType_Id", "dbo.JobApplicationTypes");
            DropForeignKey("dbo.ChecklistItemJobApplicationTypes", "ChecklistItem_Id", "dbo.ChecklistItems");
            DropForeignKey("dbo.ChecklistItems", "IdCreateFormDocument", "dbo.DocumentMasters");
            DropForeignKey("dbo.ChecklistItems", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.ChecklistItems", "IdCheckListGroup", "dbo.CheckListGroups");
            DropForeignKey("dbo.CheckListGroups", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.CheckListGroups", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.ChecklistAddressPropertyMapings", "IdChecklistAddressProperty", "dbo.ChecklistAddressProperties");
            DropIndex("dbo.JobChecklistHeaderJobApplicationWorkPermitTypes", new[] { "JobApplicationWorkPermitType_Id" });
            DropIndex("dbo.JobChecklistHeaderJobApplicationWorkPermitTypes", new[] { "JobChecklistHeader_IdJobCheckListHeader" });
            DropIndex("dbo.ChecklistItemReferenceDocuments", new[] { "ReferenceDocument_Id" });
            DropIndex("dbo.ChecklistItemReferenceDocuments", new[] { "ChecklistItem_Id" });
            DropIndex("dbo.ChecklistItemJobWorkTypes", new[] { "JobWorkType_Id" });
            DropIndex("dbo.ChecklistItemJobWorkTypes", new[] { "ChecklistItem_Id" });
            DropIndex("dbo.ChecklistItemJobApplicationTypes", new[] { "JobApplicationType_Id" });
            DropIndex("dbo.ChecklistItemJobApplicationTypes", new[] { "ChecklistItem_Id" });
            DropIndex("dbo.JobProgressNotes", new[] { "CreatedBy" });
            DropIndex("dbo.JobProgressNotes", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobProgressNotes", new[] { "IdJob" });
            DropIndex("dbo.JobPlumbingInspectionProgressNoteHistories", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobPlumbingInspectionProgressNoteHistories", new[] { "CreatedBy" });
            DropIndex("dbo.JobPlumbingInspectionProgressNoteHistories", new[] { "IdJobPlumbingInspection" });
            DropIndex("dbo.JobPlumbingInspectionDueDates", new[] { "IdJobPlumbingInspection" });
            DropIndex("dbo.JobPlumbingInspectionComments", new[] { "IdJobPlumbingInspection" });
            DropIndex("dbo.JobPlumbingInspections", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobPlumbingInspections", new[] { "CreatedBy" });
            DropIndex("dbo.JobPlumbingInspections", new[] { "IdJobPlumbingCheckListFloors" });
            DropIndex("dbo.JobPlumbingInspections", new[] { "IdChecklistItem" });
            DropIndex("dbo.JobPlumbingInspections", new[] { "IdJobChecklistGroup" });
            DropIndex("dbo.InspectionTypes", new[] { "JobPlumbingChecklistFloors_Id" });
            DropIndex("dbo.JobPlumbingChecklistFloors", new[] { "IdJobCheckListHeader" });
            DropIndex("dbo.JobCheckListProgressNoteHistories", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobCheckListProgressNoteHistories", new[] { "CreatedBy" });
            DropIndex("dbo.JobCheckListProgressNoteHistories", new[] { "IdJobChecklistItemDetail" });
            DropIndex("dbo.JobChecklistItemDueDates", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobChecklistItemDueDates", new[] { "CreatedBy" });
            DropIndex("dbo.JobChecklistItemDueDates", new[] { "IdJobChecklistItemDetail" });
            DropIndex("dbo.JobChecklistItemDetails", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobChecklistItemDetails", new[] { "CreatedBy" });
            DropIndex("dbo.JobChecklistItemDetails", new[] { "IdInspector" });
            DropIndex("dbo.JobChecklistItemDetails", new[] { "IdDesignApplicant" });
            DropIndex("dbo.JobChecklistItemDetails", new[] { "IdContact" });
            DropIndex("dbo.JobChecklistItemDetails", new[] { "IdChecklistItem" });
            DropIndex("dbo.JobChecklistItemDetails", new[] { "IdJobChecklistGroup" });
            DropIndex("dbo.JobCheckListCommentHistories", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobCheckListCommentHistories", new[] { "CreatedBy" });
            DropIndex("dbo.JobCheckListCommentHistories", new[] { "IdJobChecklistItemDetail" });
            DropIndex("dbo.CompositeViolations", new[] { "IdJobViolations" });
            DropIndex("dbo.JobChecklistHeaders", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobChecklistHeaders", new[] { "CreatedBy" });
            DropIndex("dbo.JobChecklistHeaders", new[] { "IdJobApplication" });
            DropIndex("dbo.JobChecklistHeaders", new[] { "IdJob" });
            DropIndex("dbo.JobChecklistGroups", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobChecklistGroups", new[] { "CreatedBy" });
            DropIndex("dbo.JobChecklistGroups", new[] { "IdCheckListGroup" });
            DropIndex("dbo.JobChecklistGroups", new[] { "IdJobCheckListHeader" });
            DropIndex("dbo.CompositeChecklists", new[] { "LastModifiedBy" });
            DropIndex("dbo.CompositeChecklists", new[] { "CreatedBy" });
            DropIndex("dbo.CompositeChecklistDetails", new[] { "LastModifiedBy" });
            DropIndex("dbo.CompositeChecklistDetails", new[] { "CreatedBy" });
            DropIndex("dbo.CompositeChecklistDetails", new[] { "IdJobChecklistGroup" });
            DropIndex("dbo.CompositeChecklistDetails", new[] { "IdJobChecklistHeader" });
            DropIndex("dbo.CompositeChecklistDetails", new[] { "IdCompositeChecklist" });
            DropIndex("dbo.JobTransmittals", new[] { "IdChecklistItem" });
            DropIndex("dbo.JobViolations", new[] { "IdContact" });
            DropIndex("dbo.ChecklistJobViolationComments", new[] { "LastModifiedBy" });
            DropIndex("dbo.ChecklistJobViolationComments", new[] { "CreatedBy" });
            DropIndex("dbo.ChecklistJobViolationComments", new[] { "IdJobViolation" });
            DropIndex("dbo.CheckListGroups", new[] { "LastModifiedBy" });
            DropIndex("dbo.CheckListGroups", new[] { "CreatedBy" });
            DropIndex("dbo.ChecklistItems", new[] { "IdUploadFormDocument" });
            DropIndex("dbo.ChecklistItems", new[] { "IdCreateFormDocument" });
            DropIndex("dbo.ChecklistItems", new[] { "LastModifiedBy" });
            DropIndex("dbo.ChecklistItems", new[] { "CreatedBy" });
            DropIndex("dbo.ChecklistItems", new[] { "IdCheckListGroup" });
            DropIndex("dbo.ChecklistAddressPropertyMapings", new[] { "IdChecklistItem" });
            DropIndex("dbo.ChecklistAddressPropertyMapings", new[] { "IdChecklistAddressProperty" });
            DropColumn("dbo.JobTransmittals", "IdChecklistItem");
            DropColumn("dbo.RfpAddresses", "CityOwnedCheck");
            DropColumn("dbo.RfpAddresses", "EnvironmentalRestrictionsCheck");
            DropColumn("dbo.RfpAddresses", "LoftLawCheck");
            DropColumn("dbo.RfpAddresses", "SRORestrictedCheck");
            DropColumn("dbo.JobViolations", "IsUpdateMailsent");
            DropColumn("dbo.JobViolations", "IsNewMailsent");
            DropColumn("dbo.JobViolations", "Status");
            DropColumn("dbo.JobViolations", "ISNViolation");
            DropColumn("dbo.JobViolations", "IsManually");
            DropColumn("dbo.JobViolations", "Section_Law_Description10");
            DropColumn("dbo.JobViolations", "Infraction_Code10");
            DropColumn("dbo.JobViolations", "Section_Law_Description9");
            DropColumn("dbo.JobViolations", "Infraction_Code9");
            DropColumn("dbo.JobViolations", "Section_Law_Description8");
            DropColumn("dbo.JobViolations", "Infraction_Code8");
            DropColumn("dbo.JobViolations", "Section_Law_Description7");
            DropColumn("dbo.JobViolations", "Infraction_Code7");
            DropColumn("dbo.JobViolations", "Section_Law_Description6");
            DropColumn("dbo.JobViolations", "Infraction_Code6");
            DropColumn("dbo.JobViolations", "Section_Law_Description5");
            DropColumn("dbo.JobViolations", "Infraction_Code5");
            DropColumn("dbo.JobViolations", "Section_Law_Description4");
            DropColumn("dbo.JobViolations", "Infraction_Code4");
            DropColumn("dbo.JobViolations", "Section_Law_Description3");
            DropColumn("dbo.JobViolations", "Infraction_Code3");
            DropColumn("dbo.JobViolations", "Section_Law_Description2");
            DropColumn("dbo.JobViolations", "Infraction_Code2");
            DropColumn("dbo.JobViolations", "Section_Law_Description1");
            DropColumn("dbo.JobViolations", "Infraction_Code1");
            DropColumn("dbo.JobViolations", "aggravated_level");
            DropColumn("dbo.JobViolations", "Type_ECB_DOB");
            DropColumn("dbo.JobViolations", "IsChecklistView");
            DropColumn("dbo.JobViolations", "TCOToggle");
            DropColumn("dbo.JobViolations", "ViolationDescription");
            DropColumn("dbo.JobViolations", "IdContact");
            DropColumn("dbo.JobViolations", "ManualPartyResponsible");
            DropColumn("dbo.JobViolations", "PartyResponsible");
            DropColumn("dbo.JobViolations", "Disposition_Comments");
            DropColumn("dbo.JobViolations", "Disposition_Date");
            DropColumn("dbo.JobViolations", "DOB_Description");
            DropColumn("dbo.JobViolations", "violation_type");
            DropColumn("dbo.JobViolations", "violation_category");
            DropColumn("dbo.JobViolations", "ECBnumber");
            DropColumn("dbo.JobViolations", "description_date");
            DropColumn("dbo.JobViolations", "device_number");
            DropColumn("dbo.JobViolations", "violation_number");
            DropColumn("dbo.JobViolations", "violation_type_code");
            DropColumn("dbo.JobViolations", "HearingTime");
            DropColumn("dbo.JobViolations", "BinNumber");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "HasSiteSafetyManager");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "HasSiteSafetyCoordinator");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "HasSuperintendentofconstruction");
            DropColumn("dbo.JobDocuments", "IdJobPlumbingInspections");
            DropColumn("dbo.JobDocuments", "IdJobchecklistItemDetails");
            DropColumn("dbo.JobApplications", "IsHighRise");
            DropColumn("dbo.JobApplications", "IsExternalApplication");
            DropTable("dbo.JobChecklistHeaderJobApplicationWorkPermitTypes");
            DropTable("dbo.ChecklistItemReferenceDocuments");
            DropTable("dbo.ChecklistItemJobWorkTypes");
            DropTable("dbo.ChecklistItemJobApplicationTypes");
            DropTable("dbo.JobProgressNotes");
            DropTable("dbo.JobPlumbingInspectionProgressNoteHistories");
            DropTable("dbo.JobPlumbingInspectionDueDates");
            DropTable("dbo.JobPlumbingInspectionComments");
            DropTable("dbo.JobPlumbingInspections");
            DropTable("dbo.InspectionTypes");
            DropTable("dbo.JobPlumbingChecklistFloors");
            DropTable("dbo.JobCheckListProgressNoteHistories");
            DropTable("dbo.JobChecklistItemDueDates");
            DropTable("dbo.JobChecklistItemDetails");
            DropTable("dbo.JobCheckListCommentHistories");
            DropTable("dbo.CompositeViolations");
            DropTable("dbo.JobChecklistHeaders");
            DropTable("dbo.JobChecklistGroups");
            DropTable("dbo.CompositeChecklists");
            DropTable("dbo.CompositeChecklistDetails");
            DropTable("dbo.ChecklistJobViolationComments");
            DropTable("dbo.CheckListGroups");
            DropTable("dbo.ChecklistItems");
            DropTable("dbo.ChecklistAddressPropertyMapings");
            DropTable("dbo.ChecklistAddressProperties");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
