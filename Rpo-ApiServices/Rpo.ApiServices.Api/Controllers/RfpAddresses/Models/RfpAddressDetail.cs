using System;
using Rpo.ApiServices.Model.Models;

namespace Rpo.ApiServices.Api.Controllers.RfpAddresses
{
    public class RfpAddressDetail
    {
        public int Id { get; set; }

        public int? IdBorough { get; set; }

        public Borough Borough { get; set; }

        public string HouseNumber { get; set; }

        public string Street { get; set; }

        public string ZipCode { get; set; }

        public string Block { get; set; }

        public string Lot { get; set; }

        public string BinNumber { get; set; }

        public string ComunityBoardNumber { get; set; }

        public string ZoneDistrict { get; set; }

        public string Overlay { get; set; }

        public string SpecialDistrict { get; set; }

        public string Map { get; set; }

        public int? IdOwnerType { get; set; }

        public virtual OwnerType OwnerType { get; set; }

        public int? IdCompany { get; set; }

        public Company Company { get; set; }

        public bool NonProfit { get; set; }

        public int? IdOwnerContact { get; set; }

        public virtual Contact OwnerContact { get; set; }

        public string Title { get; set; }

        public int? IdOccupancyClassification { get; set; }

        public virtual OccupancyClassification OccupancyClassification { get; set; }

        public bool IsOcupancyClassification20082014 { get; set; }

        public int? IdConstructionClassification { get; set; }

        public virtual ConstructionClassification ConstructionClassification { get; set; }

        public bool IsConstructionClassification20082014 { get; set; }

        public int? IdMultipleDwellingClassification { get; set; }

        public virtual MultipleDwellingClassification MultipleDwellingClassification { get; set; }

        public int? IdPrimaryStructuralSystem { get; set; }

        public virtual PrimaryStructuralSystem PrimaryStructuralSystem { get; set; }

        public int? IdStructureOccupancyCategory { get; set; }

        public virtual StructureOccupancyCategory StructureOccupancyCategory { get; set; }

        public int? IdSeismicDesignCategory { get; set; }

        public virtual SeismicDesignCategory SeismicDesignCategory { get; set; }

        public int? Stories { get; set; }

        public int? Height { get; set; }

        public int? Feet { get; set; }

        public int? DwellingUnits { get; set; }

        public string GrossArea { get; set; }

        public int? StreetLegalWidth { get; set; }

        public bool IsLandmark { get; set; }

        public bool IsLittleE { get; set; }

        public bool TidalWetlandsMapCheck { get; set; }

        public bool FreshwaterWetlandsMapCheck { get; set; }

        public bool CoastalErosionHazardAreaMapCheck { get; set; }

        public bool SpecialFloodHazardAreaCheck { get; set; }

        public int? CreatedBy { get; set; }

        public string CreatedByEmployeeName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public string LastModifiedByEmployeeName { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public string SecondOfficer { get; set; }

        public int? IdSecondOfficer { get; set; }

        public int? IdSecondOfficerCompany { get; set; }

        public string SecondOfficerCompany { get; set; }
        public string OutsideNYC { get; set; }
        public bool IsBSADecision { get; set; }
        public bool SRORestrictedCheck { get; set; }
        public bool LoftLawCheck { get; set; }
        public bool EnvironmentalRestrictionsCheck { get; set; }
        public bool CityOwnedCheck { get; set; }

    }
}