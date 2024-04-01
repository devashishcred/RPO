
namespace Rpo.ApiServices.Api.Controllers.RfpAddresses
{
    /// <summary>
    /// Class BIS Address Response DTO.
    /// </summary>
    public class BisAddressResponseDTO
    {
        /// <summary>
        /// Gets or sets the zip.
        /// </summary>
        /// <value>The zip.</value>
        public string BisAddress { get; set; }

        /// <summary>
        /// Gets or sets the zip.
        /// </summary>
        /// <value>The zip.</value>
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets the block.
        /// </summary>
        /// <value>The block.</value>
        public string Block { get; set; }

        /// <summary>
        /// Gets or sets the lot.
        /// </summary>
        /// <value>The lot.</value>
        public string Lot { get; set; }

        /// <summary>
        /// Gets or sets the bin.
        /// </summary>
        /// <value>The bin.</value>
        public string Bin { get; set; }

        /// <summary>
        /// Gets or sets the community board.
        /// </summary>
        /// <value>The community board.</value>
        public string CommunityBoard { get; set; }

        /// <summary>
        /// Gets or sets the special district.
        /// </summary>
        /// <value>The special district.</value>
        public string SpecialDistrict { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BisAddresResponseDTO"/> is landmark.
        /// </summary>
        /// <value><c>true</c> if landmark; otherwise, <c>false</c>.</value>
        public bool Landmark { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [little e].
        /// </summary>
        /// <value><c>true</c> if [little e]; otherwise, <c>false</c>.</value>
        public bool Little_E { get; set; }

        /// <summary>
        /// Gets or sets the zone distrinct.
        /// </summary>
        /// <value>The zone distrinct.</value>
        public string ZoneDistrinct { get; set; }

        /// <summary>
        /// Gets or sets the overlays.
        /// </summary>
        /// <value>The overlays.</value>
        public string Overlays { get; set; }

        /// <summary>
        /// Gets or sets the map.
        /// </summary>
        /// <value>The map.</value>
        public string Map { get; set; }

        /// <summary>
        /// Gets or sets the strories.
        /// </summary>
        /// <value>The strories.</value>
        public string Strories { get; set; }

        /// <summary>
        /// Gets or sets the dwelling units.
        /// </summary>
        /// <value>The dwelling units.</value>
        public string DwellingUnits { get; set; }

        /// <summary>
        /// Gets or sets the gross area.
        /// </summary>
        /// <value>The gross area.</value>
        public string GrossArea { get; set; }
        
        /// <summary>
        /// Gets or sets the house number.
        /// </summary>
        /// <value>The house number.</value>
        public string houseNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the street.
        /// </summary>
        /// <value>The name of the street.</value>
        public string streetName { get; set; }

        /// <summary>
        /// Gets or sets the borough.
        /// </summary>
        /// <value>The borough.</value>
        public string borough { get; set; }

        public bool FreshwaterWetlandsMapCheck { get; set; }

        public bool SpecialFloodHazardAreaCheck { get; set; }

        public bool TidalWetlandsMapCheck { get; set; }

        public bool CoastalErosionHazardAreaMapCheck { get; set; }

        public bool IsResultFound { get; set; }
        public bool BSADecision { get; set; }
        public bool cityOwned { get; set; }
        public bool loftLaw { get; set; }
        public bool sRORestricted { get; set; }
    }
}