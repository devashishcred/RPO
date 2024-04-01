using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpo.ApiServices.Model.Models
{
    public class RfpAddress
    {
        [Key]
        public int Id { get; set; }

        public int? IdBorough { get; set; }

        [ForeignKey("IdBorough")]
        public Borough Borough { get; set; }

        [StringLength(10)]
        public string HouseNumber { get; set; }

        [StringLength(50)]
        public string Street { get; set; }

        [MaxLength(5)]
        public string ZipCode { get; set; }

        [StringLength(50)]
        public string Block { get; set; }

        [StringLength(50)]
        public string Lot { get; set; }

        [StringLength(50)]
        public string BinNumber { get; set; }

        [StringLength(50)]
        public string ComunityBoardNumber { get; set; }

        [StringLength(50)]
        public string ZoneDistrict { get; set; }

        [StringLength(50)]
        public string Overlay { get; set; }

       // [StringLength(50)]
        public string SpecialDistrict { get; set; }

        [StringLength(50)]
        public string Map { get; set; }

        public int? IdOwnerType { get; set; }

        [ForeignKey("IdOwnerType")]
        public virtual OwnerType OwnerType { get; set; }

        public int? IdCompany { get; set; }

        [ForeignKey("IdCompany")]
        public Company Company { get; set; }

        public bool NonProfit { get; set; }

        public int? IdOwnerContact { get; set; }

        [ForeignKey("IdOwnerContact")]
        public virtual Contact OwnerContact { get; set; }

        public int? IdSecondOfficerCompany { get; set; }

        [ForeignKey("IdSecondOfficerCompany")]
        public Company SecondOfficerCompany { get; set; }

        public int? IdSecondOfficer { get; set; }

        [ForeignKey("IdSecondOfficer")]
        public virtual Contact SecondOfficer { get; set; }


        [StringLength(50)]
        public string Title { get; set; }

        public int? IdOccupancyClassification { get; set; }

        [ForeignKey("IdOccupancyClassification")]
        public virtual OccupancyClassification OccupancyClassification { get; set; }

        public bool IsOcupancyClassification20082014 { get; set; }

        public int? IdConstructionClassification { get; set; }

        [ForeignKey("IdConstructionClassification")]
        public virtual ConstructionClassification ConstructionClassification { get; set; }

        public bool IsConstructionClassification20082014 { get; set; }

        public int? IdMultipleDwellingClassification { get; set; }

        [ForeignKey("IdMultipleDwellingClassification")]
        public virtual MultipleDwellingClassification MultipleDwellingClassification { get; set; }

        public int? IdPrimaryStructuralSystem { get; set; }

        [ForeignKey("IdPrimaryStructuralSystem")]
        public virtual PrimaryStructuralSystem PrimaryStructuralSystem { get; set; }

        public int? IdStructureOccupancyCategory { get; set; }

        [ForeignKey("IdStructureOccupancyCategory")]
        public virtual StructureOccupancyCategory StructureOccupancyCategory { get; set; }

        public int? IdSeismicDesignCategory { get; set; }

        [ForeignKey("IdSeismicDesignCategory")]
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

        [ForeignKey("CreatedBy")]
        public Employee CreatedByEmployee { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        [ForeignKey("LastModifiedBy")]
        public Employee LastModifiedByEmployee { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [StringLength(250)]
        public string OutsideNYC { get; set; }
        public bool IsBSADecision { get; set; }
        public bool SRORestrictedCheck { get; set; }
        public bool LoftLawCheck { get; set; }
        public bool EnvironmentalRestrictionsCheck { get; set; }
        public bool CityOwnedCheck { get; set; }

    }
}
