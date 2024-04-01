// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-21-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-21-2018
// ***********************************************************************
// <copyright file="RfpAddressDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************


namespace Rpo.ApiServices.Api.Controllers.RfpAddresses
{
    using System;
   
    public class RfpAddressDTO
    {
        public int Id { get; set; }

        public int? IdBorough { get; set; }

        public string Borough { get; set; }

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

        public string OwnerType { get; set; }

        public int? IdCompany { get; set; }

        public string Company { get; set; }

        public bool NonProfit { get; set; }

        public int? IdOwnerContact { get; set; }

        public string OwnerContact { get; set; }

        public string Title { get; set; }

        public int? IdOccupancyClassification { get; set; }

        public string OccupancyClassification { get; set; }

        public bool IsOcupancyClassification20082014 { get; set; }

        public int? IdConstructionClassification { get; set; }

        public string ConstructionClassification { get; set; }

        public bool IsConstructionClassification20082014 { get; set; }

        public int? IdMultipleDwellingClassification { get; set; }

        public string MultipleDwellingClassification { get; set; }

        public int? IdPrimaryStructuralSystem { get; set; }

        public string PrimaryStructuralSystem { get; set; }

        public int? IdStructureOccupancyCategory { get; set; }

        public string StructureOccupancyCategory { get; set; }

        public int? IdSeismicDesignCategory { get; set; }

        public string SeismicDesignCategory { get; set; }

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
        
        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public string CreatedByEmployeeName { get; set; }

        public string LastModifiedByEmployeeName { get; set; }

        public int BoroughBisCode { get; set; }

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