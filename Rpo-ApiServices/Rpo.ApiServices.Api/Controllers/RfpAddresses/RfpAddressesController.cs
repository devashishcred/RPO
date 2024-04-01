// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-21-2018
// ***********************************************************************
// <copyright file="RfpAddressesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Rfp Addresses Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.RfpAddresses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using HtmlAgilityPack;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using System.Data.SqlClient;
    using System.Configuration;
    using Microsoft.ApplicationBlocks.Data;

    /// <summary>
    /// Class Rfp Addresses Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class RfpAddressesController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the RFP addresses in List.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Get the Address List.</returns>

        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetRfpAddresses([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewAddress)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddAddress)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteAddress))
            {
                var rfpAddresses = rpoContext.RfpAddresses
                .Include("Borough")
                .Include("OwnerType")
                .Include("Company")
                .Include("OwnerContact")
                .Include("SecondOfficerCompany")
                .Include("SecondOfficer")
                .Include("OccupancyClassification")
                .Include("ConstructionClassification")
                .Include("MultipleDwellingClassification")
                .Include("PrimaryStructuralSystem")
                .Include("StructureOccupancyCategory")
                .Include("SeismicDesignCategory").AsQueryable();

                var recordsTotal = rfpAddresses.Count();
                var recordsFiltered = recordsTotal;

                var result = rfpAddresses
                    .AsEnumerable()
                    .Select(c => Format(c))
                    .AsQueryable()
                    .DataTableParameters(dataTableParameters, out recordsFiltered)
                    .ToArray();

                return Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Draw,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal,
                    Data = result
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        [Authorize]
        [RpoAuthorize]
        [Route("api/AddressListPost")]
        public IHttpActionResult RfpAddressesList(DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewAddress)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddAddress)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteAddress))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();

                DataSet ds = new DataSet();

                SqlParameter[] spParameter = new SqlParameter[6];

                //spParameter[0] = new SqlParameter("@FirstName", SqlDbType.NVarChar, 50);
                //spParameter[0].Direction = ParameterDirection.Input;
                //spParameter[0].Value = Firstname;

                //spParameter[1] = new SqlParameter("@LastName", SqlDbType.NVarChar, 50);
                //spParameter[1].Direction = ParameterDirection.Input;
                //spParameter[1].Value = Lastname;

                //spParameter[2] = new SqlParameter("@IdContactLicenseType", SqlDbType.Int);
                //spParameter[2].Direction = ParameterDirection.Input;
                //spParameter[2].Value = dataTableParameters.IdContactLicenseType != null ? dataTableParameters.IdContactLicenseType : null;

                spParameter[0] = new SqlParameter("@PageIndex", SqlDbType.Int);
                spParameter[0].Direction = ParameterDirection.Input;
                spParameter[0].Value = dataTableParameters.Start;

                spParameter[1] = new SqlParameter("@PageSize", SqlDbType.Int);
                spParameter[1].Direction = ParameterDirection.Input;
                spParameter[1].Value = dataTableParameters.Length;

                spParameter[2] = new SqlParameter("@Column", SqlDbType.NVarChar, 50);
                spParameter[2].Direction = ParameterDirection.Input;
                spParameter[2].Value = dataTableParameters.OrderedColumn != null ? dataTableParameters.OrderedColumn.Column : null;

                spParameter[3] = new SqlParameter("@Dir", SqlDbType.VarChar, 50);
                spParameter[3].Direction = ParameterDirection.Input;
                spParameter[3].Value = dataTableParameters.OrderedColumn != null ? dataTableParameters.OrderedColumn.Dir : null;

                spParameter[4] = new SqlParameter("@Search", SqlDbType.NVarChar, 50);
                spParameter[4].Direction = ParameterDirection.Input;
                spParameter[4].Value = !string.IsNullOrEmpty(dataTableParameters.Search) ? dataTableParameters.Search : string.Empty;

                //spParameter[8] = new SqlParameter("@IdCompany", SqlDbType.Int);
                //spParameter[8].Direction = ParameterDirection.Input;
                //spParameter[8].Value = dataTableParameters.IdCompany != null ? dataTableParameters.IdCompany : null;

                //spParameter[9] = new SqlParameter("@Individual", SqlDbType.Int);
                //spParameter[9].Direction = ParameterDirection.Input;
                //spParameter[9].Value = dataTableParameters.Individual != null ? dataTableParameters.Individual : null;

                spParameter[5] = new SqlParameter("@RecordCount", SqlDbType.Int);
                spParameter[5].Direction = ParameterDirection.Output;



                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "Address_Pagination_List", spParameter);



                int totalrecord = 0;
                int Recordcount = 0;
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    totalrecord = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                    Recordcount = int.Parse(spParameter[5].SqlValue.ToString());

                }
                if (ds.Tables.Count > 1)
                {
                    ds.Tables[1].TableName = "Data";
                }


                return Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Start,
                    RecordsFiltered = Recordcount,
                    RecordsTotal = totalrecord,
                    Data = ds.Tables[1]
                });

            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the RFP addresses List for dropdown.
        /// </summary>
        /// <returns>Address List.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/rfpaddresses/dropdown")]
        public IHttpActionResult GetRfpAddressesDropdown()
        {
            var result = rpoContext.RfpAddresses.Include("Borough").AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = (!string.IsNullOrEmpty(c.HouseNumber) ? c.HouseNumber : string.Empty) + " " + (c.Street != null ? c.Street + ", " : string.Empty)
                                         + (c.Borough != null ? c.Borough.Description + (c.OutsideNYC != null ? ", " + c.OutsideNYC : string.Empty)
                                         + (c.ZipCode != "" ? ", " + c.ZipCode : string.Empty) : string.Empty),
                HouseNumber = c.HouseNumber,
                Street = c.Street,
                Block = c.Block,
                Lot = c.Lot,
                ZipCode = c.ZipCode,
                IsLandmark = c.IsLandmark,
                IsLittleE = c.IsLittleE,
                IdBorough = c.IdBorough
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the RFP address in detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Address Detail.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(RfpAddress))]
        public IHttpActionResult GetRfpAddress(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewAddress)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddAddress)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteAddress))
            {
                RfpAddress rfpAddress = rpoContext.RfpAddresses.Include("Borough")
                    .Include("SecondOfficerCompany")
                    .Include("SecondOfficer")
                    .Include("OwnerType")
                    .Include("OwnerContact")
                    .Include("Company").Where(x => x.Id == id).FirstOrDefault();
                if (rfpAddress == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(rfpAddress));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the RFP address.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="rfpAddress">The RFP address.</param>
        /// <returns>update the address information.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRfpAddress(int id, RfpAddress rfpAddress)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddAddress))
            {
                if (rfpAddress == null)
                {
                    return BadRequest();
                }

                if (id != rfpAddress.Id)
                {
                    return BadRequest();
                }

                rfpAddress.HouseNumber = rfpAddress.HouseNumber.Trim();
                rfpAddress.Street = rfpAddress.Street.Trim();
                if (rfpAddress.OutsideNYC != null)
                {
                    rfpAddress.OutsideNYC = rfpAddress.OutsideNYC.Trim();
                }
                if (this.RfpAddressExists(Convert.ToInt32(rfpAddress.IdBorough), rfpAddress.HouseNumber, rfpAddress.Street.Trim(), rfpAddress.Id))
                {
                    throw new RpoBusinessException(StaticMessages.RfpAddressExistsMessage);
                }

                IgnoreNavigationProperties(rfpAddress);

                rfpAddress.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    rfpAddress.LastModifiedBy = employee.Id;
                }

                rpoContext.Entry(rfpAddress).State = EntityState.Modified;

                try
                {
                    rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RfpAddressExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Ignores the navigation properties assign null.
        /// </summary>
        /// <param name="rfpAddress">The RFP address.</param>
        [Authorize]
        [RpoAuthorize]
        private static void IgnoreNavigationProperties(RfpAddress rfpAddress)
        {
            rfpAddress.OwnerType = null;
            rfpAddress.Company = null;
            rfpAddress.ConstructionClassification = null;
            rfpAddress.MultipleDwellingClassification = null;
            rfpAddress.OccupancyClassification = null;
            rfpAddress.OwnerContact = null;
            rfpAddress.PrimaryStructuralSystem = null;
            rfpAddress.SeismicDesignCategory = null;
            rfpAddress.StructureOccupancyCategory = null;
            rfpAddress.Borough = null;
            rfpAddress.CreatedByEmployee = null;
            rfpAddress.LastModifiedByEmployee = null;
        }

        /// <summary>
        /// Posts the RFP address into db.
        /// </summary>
        /// <param name="rfpAddress">The RFP address.</param>
        /// <returns>create a new address.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(RfpAddress))]
        public IHttpActionResult PostRfpAddress(RfpAddress rfpAddress)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddAddress))
            {
                if (rfpAddress == null)
                {
                    return BadRequest();
                }

                rfpAddress.HouseNumber = rfpAddress.HouseNumber.Trim();
                rfpAddress.Street = rfpAddress.Street.Trim();
                if (rfpAddress.OutsideNYC != null)
                {
                    rfpAddress.OutsideNYC = rfpAddress.OutsideNYC.Trim();
                }
                if (this.RfpAddressExists(Convert.ToInt32(rfpAddress.IdBorough), rfpAddress.HouseNumber, rfpAddress.Street, rfpAddress.Id))
                {
                    throw new RpoBusinessException(StaticMessages.RfpAddressExistsMessage);
                }

                IgnoreNavigationProperties(rfpAddress);
                rfpAddress.LastModifiedDate = DateTime.UtcNow;
                rfpAddress.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    rfpAddress.LastModifiedBy = employee.Id;
                    rfpAddress.CreatedBy = employee.Id;
                }

                rpoContext.RfpAddresses.Add(rfpAddress);
                rpoContext.SaveChanges();

                return this.CreatedAtRoute("DefaultApi", new { id = rfpAddress.Id }, rfpAddress);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the RFP address.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>delete the address.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(RfpAddress))]
        public IHttpActionResult DeleteRfpAddress(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteAddress))
            {
                RfpAddress rfpAddress = rpoContext.RfpAddresses.Find(id);
                if (rfpAddress == null)
                {
                    return this.NotFound();
                }

                rpoContext.RfpAddresses.Remove(rfpAddress);
                rpoContext.SaveChanges();

                return Ok(rfpAddress);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the bis addres information.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Get the bis information from the address</returns>
        [Authorize]
        [RpoAuthorize]
        [Route("api/bis/getadressinfo")]
        [ResponseType(typeof(BisAddressResponseDTO))]
        [HttpPost]
        public IHttpActionResult GetBisAddresInfo(BisAddressRequestDTO request)
        {
            List<BisAddressResponseDTO> bisAddressResponseDTOList = new List<BisAddressResponseDTO>();

            var borough = rpoContext.Boroughes.FirstOrDefault(b => b.Description == request.borough);

            if (borough == null)
            {
                return this.NotFound();
            }

            //string houseNumber_Street = request.houseNumber + " " + request.streetName;
            //List<OpenMapAddress> openmapDataList = new List<OpenMapAddress>();

            //if (request.isExactMatch)
            //{
            //    openmapDataList = rpoContext.OpenMapAddress.Where(x => x.Borough == borough.Description && x.HouseNumber_Street == houseNumber_Street).ToList();
            //}
            //else
            //{
            //    openmapDataList = rpoContext.OpenMapAddress.Where(x => x.Borough == borough.Description && x.HouseNumber_Street.Contains(houseNumber_Street)).ToList();
            //}

            //if (openmapDataList != null && openmapDataList.Count > 1)
            //{
            //    foreach (var item in openmapDataList)
            //    {
            //        if (item != null)
            //        {
            //            BisAddressResponseDTO bisAddressResponseDTO = new BisAddressResponseDTO();
            //            bisAddressResponseDTO.ZoneDistrinct = item.ZoneDistrict;
            //            bisAddressResponseDTO.Overlays = item.Overlay;
            //            bisAddressResponseDTO.Map = item.Map;
            //            bisAddressResponseDTO.Strories = item.Stories;
            //            bisAddressResponseDTO.DwellingUnits = item.DwellingUnits;
            //            bisAddressResponseDTO.GrossArea = item.GrossArea;
            //            bisAddressResponseDTO.houseNumber = item.HouseNumber_Street.Split(' ').FirstOrDefault();
            //            bisAddressResponseDTO.streetName = item.HouseNumber_Street.Replace(bisAddressResponseDTO.houseNumber, "");
            //            bisAddressResponseDTO.borough = item.Borough;

            //            bisAddressResponseDTOList.Add(bisAddressResponseDTO);
            //        }
            //    }

            //    return Ok(bisAddressResponseDTOList);
            //}
            //else if (openmapDataList != null)
            //{
            //    OpenMapAddress openmapData = openmapDataList.FirstOrDefault();

            //    BisAddressResponseDTO result = new BisAddressResponseDTO();
            //    if (openmapData != null)
            //    {
            //        result.ZoneDistrinct = openmapData.ZoneDistrict;
            //        result.Overlays = openmapData.Overlay;
            //        result.Map = openmapData.Map;
            //        result.Strories = openmapData.Stories;
            //        result.DwellingUnits = openmapData.DwellingUnits;
            //        result.GrossArea = openmapData.GrossArea;
            //        result.houseNumber = openmapData.HouseNumber_Street.Split(' ').FirstOrDefault();
            //        result.streetName = openmapData.HouseNumber_Street.Replace(result.houseNumber, "");
            //        request.streetName = openmapData.HouseNumber_Street.Replace(result.houseNumber, "");
            //        result.borough = openmapData.Borough;
            //    }
            //    else
            //    {
            //        result.houseNumber = request.houseNumber;
            //        result.streetName = request.streetName;
            //        result.borough = borough != null ? borough.Description : string.Empty;

            //    }

            string urlAddress = $"https://a810-bisweb.nyc.gov/bisweb/PropertyProfileOverviewServlet?houseno={request.houseNumber}&street={HttpUtility.UrlEncode(request.streetName)}&boro={borough.BisCode}";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12;
            HtmlAgilityPack.HtmlDocument doc = new HtmlWeb().Load(urlAddress);

            BisAddressResponseDTO result = new BisAddressResponseDTO();

            var descendants = doc.DocumentNode.Descendants();

            // body > center > table:nth - child(2) > tbody > tr:nth - child(2) > td:nth - child(2)
            // / html / body / center / table[2] / tbody / tr[2] / td[2] <b>&nbsp;Special District: &nbsp;</b>
            var bisAddress = descendants.FirstOrDefault(n => n.HasClass("maininfo") && n.InnerHtml.Any());

            var zipCode = descendants.FirstOrDefault(n => n.HasClass("maininfo") && n.InnerHtml.Contains(borough.Description.ToUpper() + "&nbsp;&nbsp;&nbsp;"));

            var block = descendants.FirstOrDefault(n => n.HasClass("content") && n.InnerHtml.Contains("<b>Tax Block</b>"));

            var lot = descendants.FirstOrDefault(n => n.HasClass("content") && n.InnerHtml.Contains("<b>Tax Lot</b>"));

            var bin = descendants.FirstOrDefault(n => n.HasClass("maininfo") && n.InnerHtml.Contains("BIN#&nbsp;&nbsp;"));

            var communityBoard = descendants.FirstOrDefault(n => n.HasClass("content") && n.InnerHtml.Contains("<b>Community Board</b>"));

            var specialDistrict = descendants.FirstOrDefault(n => n.HasClass("content") && n.InnerHtml.Contains("<b>&nbsp;Special District: &nbsp;</b>"));

            var landmarkStatus = descendants.FirstOrDefault(n => n.HasClass("content") && n.InnerHtml.Contains("<b>Landmark Status:</b>"));

            var environmentalRestrictions = descendants.FirstOrDefault(n => n.HasClass("content") && n.InnerHtml.Contains("<b>Environmental Restrictions:</b>"));
            var loftLaw = descendants.FirstOrDefault(n => n.HasClass("content") && n.InnerHtml.Contains("<b>Loft Law:</b>"));
            var sRORestricted = descendants.FirstOrDefault(n => n.HasClass("content") && n.InnerHtml.Contains("<b>SRO Restricted:</b>"));
            var cityOwned = descendants.FirstOrDefault(n => n.HasClass("content") && n.InnerHtml.Contains("<b>City Owned:</b>"));

            var tidalWetlandsMapCheck = descendants.FirstOrDefault(n => n.HasClass("content") && n.InnerHtml.Contains("<b>&nbsp;&nbsp;&nbsp;&nbsp;Tidal Wetlands Map Check: </b>"));

            var freshwaterWetlandsMapCheck = descendants.FirstOrDefault(n => n.HasClass("content") && n.InnerHtml.Contains("<b>&nbsp;&nbsp;&nbsp;&nbsp;Freshwater Wetlands Map Check: </b>"));

            var coastalErosionHazardAreaMapCheck = descendants.FirstOrDefault(n => n.HasClass("content") && n.InnerHtml.Contains("<b>&nbsp;&nbsp;&nbsp;&nbsp;Coastal Erosion Hazard Area Map Check: </b>"));

            var specialFloodHazardAreaCheck = descendants.FirstOrDefault(n => n.HasClass("content") && n.InnerHtml.Contains("<b>&nbsp;&nbsp;&nbsp;&nbsp;Special Flood Hazard Area Check: </b>"));

            var bsaDecision = descendants.FirstOrDefault(n => n.HasClass("content") && n.InnerHtml.Contains("<b>BSA Decision:</b>"));

            if (bisAddress != null)
            {
                result.BisAddress = bisAddress.InnerHtml.Split(new string[] { "&nbsp;" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                result.BisAddress = !string.IsNullOrEmpty(result.BisAddress) ? Regex.Replace(result.BisAddress, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                result.BisAddress = !string.IsNullOrEmpty(result.BisAddress) ? Regex.Replace(result.BisAddress, " {2,}", " ").Trim() : string.Empty;
            }

            if (zipCode != null)
            {
                result.Zip = zipCode.InnerHtml.Split(new string[] { "&nbsp;" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();

                result.Zip = !string.IsNullOrEmpty(result.Zip) ? Regex.Replace(result.Zip, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
            }

            if (block != null)
            {
                result.Block = block.ParentNode.ChildNodes.LastOrDefault(n => n.Name == "td")?.InnerText;
                if (result.Block != null)
                {
                    result.Block = result.Block.Trim(':', ' ');
                }

                result.Block = !string.IsNullOrEmpty(result.Block) ? Regex.Replace(result.Block, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
            }

            if (lot != null)
            {
                result.Lot = lot.ParentNode.ChildNodes.LastOrDefault(n => n.Name == "td")?.InnerText.Trim(':', ' ');
                result.Lot = !string.IsNullOrEmpty(result.Lot) ? Regex.Replace(result.Lot, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;

            }

            if (bin != null)
            {
                result.Bin = bin.InnerHtml.Split(new string[] { "&nbsp;" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                result.Bin = !string.IsNullOrEmpty(result.Bin) ? Regex.Replace(result.Bin, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
            }

            if (communityBoard != null)
            {
                result.CommunityBoard = communityBoard.ParentNode.ChildNodes.Where(n => n.Name == "td").Skip(5).FirstOrDefault()?.InnerText.Trim(':', ' ');
                result.CommunityBoard = !string.IsNullOrEmpty(result.CommunityBoard) ? Regex.Replace(result.CommunityBoard, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
            }

            if (specialDistrict != null)
            {
                result.SpecialDistrict = specialDistrict.ParentNode.ChildNodes.LastOrDefault(n => n.Name == "td")?.InnerText;
                result.SpecialDistrict = !string.IsNullOrEmpty(result.SpecialDistrict) && (result.SpecialDistrict.ToUpper() != "UNKNOWN") ? Regex.Replace(result.SpecialDistrict, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
            }

            if (landmarkStatus != null)
            {
                string landmark = landmarkStatus.ParentNode.ChildNodes.Where(n => n.Name == "td").Skip(1).FirstOrDefault()?.InnerText;
                if (!string.IsNullOrEmpty(landmark) && (landmark.ToLower() != "no" || landmark.ToLower() != "n/a"))
                {
                    result.Landmark = true;
                }
            }
            if (bsaDecision != null)
            {
                string decision = bsaDecision.ParentNode.ChildNodes.Where(n => n.Name == "td").Skip(1).FirstOrDefault()?.InnerText;
                if (!string.IsNullOrEmpty(decision) && (decision.ToLower() != "no" || decision.ToLower() != "n/a"))
                {
                    result.BSADecision = true;
                }
            }
            if (cityOwned != null)
            {
                //string city_Owned = cityOwned.ParentNode.ChildNodes.Where(n => n.Name == "td").Skip(1).FirstOrDefault()?.InnerText;
                string city_Owned=cityOwned.ParentNode.ChildNodes.Where(n => n.Name == "td").Skip(3).FirstOrDefault()?.InnerText;
                if (!string.IsNullOrEmpty(city_Owned) && (city_Owned.ToLower() != "no") && (city_Owned.ToLower() != "n/a"))
                {
                    result.cityOwned = true;
                }
            }
            if (loftLaw != null)
            {
                string little_E = loftLaw.ParentNode.ChildNodes.Where(n => n.Name == "td").Skip(1).FirstOrDefault()?.InnerText;
                if (!string.IsNullOrEmpty(little_E) && (little_E.ToLower() != "no") && (little_E.ToLower() != "n/a"))
                {
                    result.loftLaw = true;
                }
            }
            if (sRORestricted != null)
            {
                string little_E = sRORestricted.ParentNode.ChildNodes.Where(n => n.Name == "td").Skip(1).FirstOrDefault()?.InnerText;
                if (!string.IsNullOrEmpty(little_E) && (little_E.ToLower() != "no") && (little_E.ToLower() != "n/a"))
                {
                    result.sRORestricted = true;
                }
            }
            if (environmentalRestrictions != null)
            {
                string little_E = environmentalRestrictions.ParentNode.ChildNodes.Where(n => n.Name == "td").Skip(1).FirstOrDefault()?.InnerText;
                if (!string.IsNullOrEmpty(little_E) && (little_E.ToLower() != "no") && (little_E.ToLower() != "n/a"))
                {
                    result.Little_E = true;
                }
            }

            if (tidalWetlandsMapCheck != null)
            {
                string tidalWetlandsMapCheck_td = tidalWetlandsMapCheck.ParentNode.ChildNodes.Where(n => n.Name == "td").Skip(1).FirstOrDefault()?.InnerText;
                if (!string.IsNullOrEmpty(tidalWetlandsMapCheck_td) && tidalWetlandsMapCheck_td.ToLower() == "yes")
                {
                    result.TidalWetlandsMapCheck = true;
                }
            }

            if (freshwaterWetlandsMapCheck != null)
            {
                string freshwaterWetlandsMapCheck_td = freshwaterWetlandsMapCheck.ParentNode.ChildNodes.Where(n => n.Name == "td").Skip(1).FirstOrDefault()?.InnerText;
                if (!string.IsNullOrEmpty(freshwaterWetlandsMapCheck_td) && freshwaterWetlandsMapCheck_td.ToLower() == "yes")
                {
                    result.FreshwaterWetlandsMapCheck = true;
                }
            }

            if (specialFloodHazardAreaCheck != null)
            {
                string specialFloodHazardAreaCheck_td = specialFloodHazardAreaCheck.ParentNode.ChildNodes.Where(n => n.Name == "td").Skip(1).FirstOrDefault()?.InnerText;
                if (!string.IsNullOrEmpty(specialFloodHazardAreaCheck_td) && specialFloodHazardAreaCheck_td.ToLower() == "yes")
                {
                    result.SpecialFloodHazardAreaCheck = true;
                }
            }

            if (coastalErosionHazardAreaMapCheck != null)
            {
                string coastalErosionHazardAreaMapCheck_td = coastalErosionHazardAreaMapCheck.ParentNode.ChildNodes.Where(n => n.Name == "td").Skip(1).FirstOrDefault()?.InnerText;
                if (!string.IsNullOrEmpty(coastalErosionHazardAreaMapCheck_td) && coastalErosionHazardAreaMapCheck_td.ToLower() == "yes")
                {
                    result.CoastalErosionHazardAreaMapCheck = true;
                }
            }

            if (!string.IsNullOrWhiteSpace(result.Zip) || !string.IsNullOrWhiteSpace(result.Block) || !string.IsNullOrWhiteSpace(result.Lot)
                || !string.IsNullOrWhiteSpace(result.Bin) || !string.IsNullOrWhiteSpace(result.CommunityBoard) || !string.IsNullOrWhiteSpace(result.SpecialDistrict)
                || !string.IsNullOrWhiteSpace(result.ZoneDistrinct)
                || !string.IsNullOrWhiteSpace(result.Overlays) || !string.IsNullOrWhiteSpace(result.Map) || !string.IsNullOrWhiteSpace(result.Strories)
                || !string.IsNullOrWhiteSpace(result.DwellingUnits) || !string.IsNullOrWhiteSpace(result.GrossArea))
            {
                result.IsResultFound = true;
            }
            else
            {
                result.IsResultFound = false;
            }

            OpenMapAddress openMapAddress = null;

            if (result.IsResultFound)
            {
                openMapAddress = rpoContext.OpenMapAddress.FirstOrDefault(x => x.Borough == borough.Description && x.Block == result.Block && x.Lot == result.Lot);

                if (openMapAddress != null)
                {
                    result.ZoneDistrinct = openMapAddress.ZoneDistrict;
                    result.Overlays = openMapAddress.Overlay;
                    result.Map = openMapAddress.Map;
                    result.Strories = openMapAddress.Stories;
                    result.DwellingUnits = openMapAddress.DwellingUnits;
                    result.GrossArea = openMapAddress.GrossArea;
                }
            }

            if (openMapAddress != null || !string.IsNullOrWhiteSpace(result.SpecialDistrict)
                    || !string.IsNullOrWhiteSpace(result.CommunityBoard)
                    || !string.IsNullOrWhiteSpace(result.Bin)
                    || !string.IsNullOrWhiteSpace(result.Lot)
                    || !string.IsNullOrWhiteSpace(result.Block)
                    || !string.IsNullOrWhiteSpace(result.Zip))
            {
                result.houseNumber = request.houseNumber;
                result.streetName = request.streetName;
                result.borough = borough != null ? borough.Description : string.Empty;

                bisAddressResponseDTOList.Add(result);
            }

            return Ok(bisAddressResponseDTOList);
            //}
            //else
            //{
            //    return Ok(bisAddressResponseDTOList);
            //}
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                rpoContext.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// RFPs the address exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool RfpAddressExists(int id)
        {
            return rpoContext.RfpAddresses.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified RFP address.
        /// </summary>
        /// <param name="rfpAddress">The RFP address.</param>
        /// <returns>RfpAddressDTO.</returns>
        private RfpAddressDTO Format(RfpAddress rfpAddress)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new RfpAddressDTO
            {
                Id = rfpAddress.Id,
                IdBorough = rfpAddress.IdBorough,
                BoroughBisCode = rfpAddress.Borough != null ? rfpAddress.Borough.BisCode : 0,
                IdOwnerType = rfpAddress.IdOwnerType,
                IdCompany = rfpAddress.IdCompany,
                IdConstructionClassification = rfpAddress.IdConstructionClassification,
                IdMultipleDwellingClassification = rfpAddress.IdMultipleDwellingClassification,
                IdOccupancyClassification = rfpAddress.IdOccupancyClassification,
                IdOwnerContact = rfpAddress.IdOwnerContact,
                IdPrimaryStructuralSystem = rfpAddress.IdPrimaryStructuralSystem,
                IdSeismicDesignCategory = rfpAddress.IdSeismicDesignCategory,
                IdStructureOccupancyCategory = rfpAddress.IdStructureOccupancyCategory,
                OwnerType = rfpAddress.OwnerType != null ? rfpAddress.OwnerType.Name : string.Empty,
                Company = rfpAddress.Company != null ? rfpAddress.Company.Name : string.Empty,
                BinNumber = rfpAddress.BinNumber,
                Block = rfpAddress.Block,
                Borough = rfpAddress.Borough != null ? rfpAddress.Borough.Description : string.Empty,
                CoastalErosionHazardAreaMapCheck = rfpAddress.CoastalErosionHazardAreaMapCheck,
                IsLandmark = rfpAddress.IsLandmark,
                IsLittleE = rfpAddress.IsLittleE,
                TidalWetlandsMapCheck = rfpAddress.TidalWetlandsMapCheck,
                FreshwaterWetlandsMapCheck = rfpAddress.FreshwaterWetlandsMapCheck,
                SpecialFloodHazardAreaCheck = rfpAddress.SpecialFloodHazardAreaCheck,
                ZipCode = rfpAddress.ZipCode,
                ComunityBoardNumber = rfpAddress.ComunityBoardNumber,
                ConstructionClassification = rfpAddress.ConstructionClassification != null ? rfpAddress.ConstructionClassification.Description : string.Empty,
                DwellingUnits = rfpAddress.DwellingUnits,
                Feet = rfpAddress.Feet,
                GrossArea = rfpAddress.GrossArea,
                Height = rfpAddress.Height,
                HouseNumber = rfpAddress.HouseNumber,
                IsConstructionClassification20082014 = rfpAddress.IsConstructionClassification20082014,
                IsOcupancyClassification20082014 = rfpAddress.IsOcupancyClassification20082014,
                Lot = rfpAddress.Lot,
                Map = rfpAddress.Map,
                MultipleDwellingClassification = rfpAddress.MultipleDwellingClassification != null ? rfpAddress.MultipleDwellingClassification.Description : string.Empty,
                NonProfit = rfpAddress.NonProfit,
                OccupancyClassification = rfpAddress.OccupancyClassification != null ? rfpAddress.OccupancyClassification.Description : string.Empty,
                Overlay = rfpAddress.Overlay,
                OwnerContact = rfpAddress.OwnerContact != null ? rfpAddress.OwnerContact.FirstName + " " + rfpAddress.OwnerContact.LastName : string.Empty,
                SecondOfficer = rfpAddress.SecondOfficer != null ? rfpAddress.SecondOfficer.FirstName + " " + rfpAddress.SecondOfficer.LastName : string.Empty,
                IdSecondOfficer = rfpAddress.IdSecondOfficer,
                IdSecondOfficerCompany = rfpAddress.IdSecondOfficerCompany,
                SecondOfficerCompany = rfpAddress.SecondOfficerCompany != null ? rfpAddress.SecondOfficerCompany.Name : string.Empty,
                PrimaryStructuralSystem = rfpAddress.PrimaryStructuralSystem != null ? rfpAddress.PrimaryStructuralSystem.Description : string.Empty,
                SeismicDesignCategory = rfpAddress.SeismicDesignCategory != null ? rfpAddress.SeismicDesignCategory.Description : string.Empty,
                SpecialDistrict = rfpAddress.SpecialDistrict,
                Stories = rfpAddress.Stories,
                Street = rfpAddress.Street,
                StreetLegalWidth = rfpAddress.StreetLegalWidth,
                StructureOccupancyCategory = rfpAddress.StructureOccupancyCategory != null ? rfpAddress.StructureOccupancyCategory.Description : string.Empty,
                Title = rfpAddress.Title,
                ZoneDistrict = rfpAddress.ZoneDistrict,
                CreatedBy = rfpAddress.CreatedBy,
                LastModifiedBy = rfpAddress.LastModifiedBy,
                CreatedByEmployeeName = rfpAddress.CreatedByEmployee != null ? rfpAddress.CreatedByEmployee.FirstName + " " + rfpAddress.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = rfpAddress.LastModifiedByEmployee != null ? rfpAddress.LastModifiedByEmployee.FirstName + " " + rfpAddress.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = rfpAddress.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpAddress.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpAddress.CreatedDate,
                LastModifiedDate = rfpAddress.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpAddress.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpAddress.LastModifiedDate,
                OutsideNYC = rfpAddress.OutsideNYC,
                IsBSADecision = rfpAddress.IsBSADecision,
                SRORestrictedCheck = rfpAddress.SRORestrictedCheck,
                LoftLawCheck = rfpAddress.LoftLawCheck,
                EnvironmentalRestrictionsCheck = rfpAddress.EnvironmentalRestrictionsCheck,
                CityOwnedCheck = rfpAddress.CityOwnedCheck,

            };
        }

        private RfpAddressDetail FormatDetails(RfpAddress rfpAddress)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new RfpAddressDetail
            {
                Id = rfpAddress.Id,
                IdBorough = rfpAddress.IdBorough,
                Borough = rfpAddress.Borough,
                IdOwnerType = rfpAddress.IdOwnerType,
                IdCompany = rfpAddress.IdCompany,
                IdConstructionClassification = rfpAddress.IdConstructionClassification,
                IdMultipleDwellingClassification = rfpAddress.IdMultipleDwellingClassification,
                IdOccupancyClassification = rfpAddress.IdOccupancyClassification,
                IdOwnerContact = rfpAddress.IdOwnerContact,
                IdPrimaryStructuralSystem = rfpAddress.IdPrimaryStructuralSystem,
                IdSeismicDesignCategory = rfpAddress.IdSeismicDesignCategory,
                IdStructureOccupancyCategory = rfpAddress.IdStructureOccupancyCategory,
                OwnerType = rfpAddress.OwnerType,
                Company = rfpAddress.Company,
                BinNumber = rfpAddress.BinNumber,
                Block = rfpAddress.Block,
                CoastalErosionHazardAreaMapCheck = rfpAddress.CoastalErosionHazardAreaMapCheck,
                IsLandmark = rfpAddress.IsLandmark,
                IsLittleE = rfpAddress.IsLittleE,
                TidalWetlandsMapCheck = rfpAddress.TidalWetlandsMapCheck,
                FreshwaterWetlandsMapCheck = rfpAddress.FreshwaterWetlandsMapCheck,
                SpecialFloodHazardAreaCheck = rfpAddress.SpecialFloodHazardAreaCheck,
                ZipCode = rfpAddress.ZipCode,
                ComunityBoardNumber = rfpAddress.ComunityBoardNumber,
                ConstructionClassification = rfpAddress.ConstructionClassification,
                DwellingUnits = rfpAddress.DwellingUnits,
                Feet = rfpAddress.Feet,
                GrossArea = rfpAddress.GrossArea,
                Height = rfpAddress.Height,
                HouseNumber = rfpAddress.HouseNumber,
                IsConstructionClassification20082014 = rfpAddress.IsConstructionClassification20082014,
                IsOcupancyClassification20082014 = rfpAddress.IsOcupancyClassification20082014,
                Lot = rfpAddress.Lot,
                Map = rfpAddress.Map,
                MultipleDwellingClassification = rfpAddress.MultipleDwellingClassification,
                NonProfit = rfpAddress.NonProfit,
                OccupancyClassification = rfpAddress.OccupancyClassification,
                Overlay = rfpAddress.Overlay,
                OwnerContact = rfpAddress.OwnerContact,
                PrimaryStructuralSystem = rfpAddress.PrimaryStructuralSystem,
                SeismicDesignCategory = rfpAddress.SeismicDesignCategory,
                SpecialDistrict = rfpAddress.SpecialDistrict,
                Stories = rfpAddress.Stories,
                Street = rfpAddress.Street,
                StreetLegalWidth = rfpAddress.StreetLegalWidth,
                StructureOccupancyCategory = rfpAddress.StructureOccupancyCategory,
                Title = rfpAddress.Title,
                SecondOfficer = rfpAddress.SecondOfficer != null ? rfpAddress.SecondOfficer.FirstName + " " + rfpAddress.SecondOfficer.LastName : string.Empty,
                IdSecondOfficer = rfpAddress.IdSecondOfficer,
                IdSecondOfficerCompany = rfpAddress.IdSecondOfficerCompany,
                SecondOfficerCompany = rfpAddress.SecondOfficerCompany != null ? rfpAddress.SecondOfficerCompany.Name : string.Empty,
                ZoneDistrict = rfpAddress.ZoneDistrict,
                CreatedBy = rfpAddress.CreatedBy,
                LastModifiedBy = rfpAddress.LastModifiedBy,
                CreatedByEmployeeName = rfpAddress.CreatedByEmployee != null ? rfpAddress.CreatedByEmployee.FirstName + " " + rfpAddress.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = rfpAddress.LastModifiedByEmployee != null ? rfpAddress.LastModifiedByEmployee.FirstName + " " + rfpAddress.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = rfpAddress.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpAddress.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpAddress.CreatedDate,
                LastModifiedDate = rfpAddress.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpAddress.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpAddress.LastModifiedDate,
                OutsideNYC = rfpAddress.OutsideNYC,
                IsBSADecision = rfpAddress.IsBSADecision,
                SRORestrictedCheck = rfpAddress.SRORestrictedCheck,
                LoftLawCheck = rfpAddress.LoftLawCheck,
                EnvironmentalRestrictionsCheck = rfpAddress.EnvironmentalRestrictionsCheck,
                CityOwnedCheck = rfpAddress.CityOwnedCheck,
            };
        }

        private bool RfpAddressExists(int idBorough, string houseNumber, string street, int id)
        {
            return this.rpoContext.RfpAddresses.Count(e => e.IdBorough == idBorough && e.HouseNumber.ToLower().Trim() == houseNumber.ToLower().Trim() && e.Street.ToLower().Trim() == street.ToLower().Trim() && e.Id != id) > 0;
        }
    }
}